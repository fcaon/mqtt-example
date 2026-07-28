namespace MqttExample.Utils.FileOperation
{
    public static class FileRead
    {
        public static string AllContent(string filePath, string fileName)
        {
            string fileContent = string.Empty;
            try
            {
                fileContent = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                PrintErroConsole.PrintException("Error reading config file.", e);
            }

            return fileContent;
        }
    }
}
