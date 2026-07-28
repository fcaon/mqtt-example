namespace MqttExample.Utils
{
    public static class PrintErroConsole
    {
        public static void PrintException(String mensageError, Exception ex)
        {
            Console.WriteLine(
                $"{DateTime.Now} - {mensageError} Erro: {ex.Message} - {ex.StackTrace} - {ex.Source} - {ex.InnerException}"
            );
        }
    }
}
