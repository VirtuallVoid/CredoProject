using Serilog;
using Serilog.Formatting.Compact;
using System;

namespace Application.Helpers
{
    public class Helper
    {
        public static void LogException(Exception ex, string requestJson, string responseJson, string operationName)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(new CompactJsonFormatter(), @"C:\Logs\CredoProject.log")
                .CreateLogger();

            var log = new
            {
                LogDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                Exception = ex,
                RequestJson = requestJson,
                ResponseJson = responseJson,
                OperationName = operationName
            };

            Log.Warning("Log {@Log}", log);
            Log.CloseAndFlush();
        }
    }
}
