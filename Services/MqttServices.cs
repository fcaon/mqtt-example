using System.Text;
using MqttExample.Models.Mqtt;
using MqttExample.Utils;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Formatter;
using System.Security.Cryptography;

namespace MqttExample.Services
{
    public class MqttServices
    {
        private readonly IMqttClient mqttClient;
        private bool threadStarted;
        private readonly MqttConfig mqttConfig;

        public MqttServices()
        {
            var mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();
            mqttConfig = MqttConfiguration.GetConnectionConfig();
            threadStarted = !mqttConfig.startThread;
        }

        public async Task Start()
        {
            var mqttClientOptions = MqttClientOptionsAccess();

            try
            {
                MqttClientConnectResult connectResult = await mqttClient.ConnectAsync(
                    mqttClientOptions,
                    CancellationToken.None
                );
                Console.WriteLine($"{DateTime.Now} - MQTT connected => {mqttClient.IsConnected}");

                if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
                {
                    if (!threadStarted)
                    {
                        Thread trd = new(new ThreadStart(MqttReconnect)) { IsBackground = true };
                        trd.Start();
                    }

                    await Subscribe();

                    mqttClient.ApplicationMessageReceivedAsync += async e =>
                    {
                        /*>>>>>
                            AQUI VAI O CÓDIGO A SER EXECUTADO SEMPRE QUE NOVA MENSAGEM CHEGAR
                        <<<<<*/
                        Console.WriteLine(
                            $"Tópico -> {e.ApplicationMessage.Topic} - Message -> {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}"
                        );
                    };

                    mqttClient.DisconnectedAsync += async disconn =>
                    {
                        Console.WriteLine(
                            $"{DateTime.Now} - Disconnected from MQTT broker => {disconn.ClientWasConnected}"
                        );
                    };
                }
            }
            catch (OperationCanceledException operationCancel)
            {
                PrintErroConsole.PrintException(
                    "Operação de conexão ao Server MQTT CANCELADA. Reiniciando conexão ",
                    operationCancel
                );
                Console.WriteLine(
                    $"{DateTime.Now} - Status da conexão => {mqttClient.IsConnected}"
                );
            }
            catch (MqttCommunicationException mqttException)
            {
                PrintErroConsole.PrintException("Erro ao conectar ao Server MQTT. ", mqttException);
                Console.WriteLine($"{DateTime.Now} - Tentando reconectar...");
                await Start();
            }
            catch (InvalidOperationException invalidOperation)
            {
                PrintErroConsole.PrintException("Operacao invalida.", invalidOperation);
                Console.WriteLine($"{DateTime.Now} - Tentando nova conexão.");
                await mqttClient.DisconnectAsync(
                    MqttClientDisconnectOptionsReason.NormalDisconnection
                );
                await Start();
            }
        }

        private MqttClientOptions MqttClientOptionsAccess()
        {
            string serverMqttAddress = mqttConfig.host!;
            int serverMqttPort = mqttConfig.port;
            string protocolVersion = mqttConfig.protocolVersion!;
            MqttProtocolVersion mqttProtocolVersion;

            mqttProtocolVersion = protocolVersion switch
            {
                "5.0" or "5.00" or "5" => MqttProtocolVersion.V500,
                "3.11" => MqttProtocolVersion.V311,
                "3.10" or "3.1" => MqttProtocolVersion.V310,
                _ => MqttProtocolVersion.V310,
            };

            byte[] randomNumber = new byte[4];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            string clientId = "client_" + BitConverter.ToUInt32(randomNumber);

            Console.WriteLine(
                $"Conectando ao server MQTT => {serverMqttAddress} - {serverMqttPort}"
            );

            return new MqttClientOptionsBuilder()
                .WithTcpServer(serverMqttAddress, serverMqttPort)
                .WithClientId($"client{clientId}")
                .WithKeepAlivePeriod(TimeSpan.MaxValue)
                .WithProtocolVersion(mqttProtocolVersion)
                .Build();
        }

        private async Task Subscribe()
        {
            MqttFactory mqttFactory = new MqttFactory();
            String[] topics = MqttConfiguration.GetTopicsSubscribe();

            foreach (var topic in topics)
            {
                Console.WriteLine($"Topico de subscricao => {topic}");
                var mqttSubscribeOptions = mqttFactory
                    .CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(topicFilter =>
                    {
                        topicFilter.WithTopic(topic);
                    })
                    .Build();

                var resultSubscribe = await mqttClient.SubscribeAsync(
                    mqttSubscribeOptions,
                    CancellationToken.None
                );

                foreach (var item in resultSubscribe.Items)
                    Console.WriteLine($"Resultado da subscricao => {item.TopicFilter}");
            }
        }

        private async void MqttReconnect()
        {
            threadStarted = true;
            while (true)
            {
                try
                {
                    await mqttClient.PingAsync(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now} - MQTT Ping Error: {ex.Message}");
                    Console.WriteLine("No connection... Reconnecting...");
                    var mqttDisconnectOptions = new MqttClientDisconnectOptions
                    {
                        Reason = MqttClientDisconnectOptionsReason.NormalDisconnection
                    };
                    await mqttClient.DisconnectAsync(mqttDisconnectOptions, CancellationToken.None);
                    Thread.Sleep(1000);
                    await Start();
                }
                Thread.Sleep(mqttConfig.threadSleepTime);
            }
        }
    }
}
