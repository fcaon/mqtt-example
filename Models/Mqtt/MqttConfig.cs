namespace MqttExample.Models.Mqtt
{
    public class MqttConfig
    {
        public MqttConfig() { }

        public string? host { get; set; }

        public int port { get; set; }

        public string? username { get; set; }

        public string? password { get; set; }

        public string? protocolVersion { get; set; }

        public bool startThread { get; set; }

        public int threadSleepTime { get; set; }
    }
}
