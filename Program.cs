using MqttExample.Services;

namespace MqttExample
{
    public static class Start
    {
        public static async Task Main(string[] args)
        {
            MqttServices mqttServices = new MqttServices();
            await mqttServices.Start();

            Task.Delay(-1).Wait();
        }
    }
}
