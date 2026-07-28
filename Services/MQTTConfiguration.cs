using MqttExample.Models.Mqtt;
using MqttExample.Utils;
using MqttExample.Utils.FileOperation;
using MqttExample.Utils.JsonOperation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MqttExample.Services
{
    public static class MqttConfiguration
    {
        public static MqttConfig GetConnectionConfig()
        {
            String appSettings = String.Empty;
            String fileName = "appsettings.json";
            appSettings = FileRead.AllContent(FilePath.Get(fileName), fileName);

            var settings = JTokenToString.Convert(
                JObject.Parse(appSettings),
                "ServerMqttConnection"
            );

            MqttConfig config = JsonConvert.DeserializeObject<MqttConfig>(settings)!;

            return config;
        }

        public static String[] GetTopicsSubscribe()
        {
            String appSettings = String.Empty;
            String fileName = "appsettings.json";
            appSettings = FileRead.AllContent(FilePath.Get(fileName), fileName);

            string[] topicsSubscribe = Array.Empty<string>();
            try
            {
                var settings = JObject.Parse(appSettings);
                topicsSubscribe = settings
                    .SelectToken("MqttTopicsSubscribe")!
                    .ToObject<String[]>()!;
            }
            catch (Exception ex)
            {
                PrintErroConsole.PrintException(
                    "Erro ao consultar tópicos Subscricao MQTT -> ",
                    ex
                );
                _ = topicsSubscribe.Append("/#");
            }

            return topicsSubscribe;
        }
    }
}
