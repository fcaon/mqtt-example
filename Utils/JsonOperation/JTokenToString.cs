using Newtonsoft.Json.Linq;

namespace MqttExample.Utils.JsonOperation
{
    public static class JTokenToString
    {
        public static string Convert(JToken jToken)
        {
            string jTokenConvert = string.Empty;
            foreach (JToken item in jToken)
            {
                jTokenConvert += item.ToString();
            }

            return jTokenConvert;
        }

        public static string Convert(JObject jObject, string token)
        {
            return jObject.SelectToken(token)!.ToString();
        }
    }
}
