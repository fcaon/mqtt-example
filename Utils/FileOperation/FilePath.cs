namespace MqttExample.Utils.FileOperation
{
    public static class FilePath
    {
        public static string Get(string fileName)
        {
            string path = string.Empty;

            var directory = Environment.CurrentDirectory;
            if (!directory.EndsWith("\\") || !directory.EndsWith("/"))
            {
                if (directory.Contains("\\"))
                    directory += "\\";
                else
                    directory += "/";
            }

            path = directory + fileName;

            return path;
        }
    }
}
