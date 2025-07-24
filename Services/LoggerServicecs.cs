using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager.Services
{
    public enum LogSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }
    public class LoggerService
    {
        private static readonly string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "log.txt");
        public static void Log(Exception ex, string? preMessage = null , LogSeverity severity = LogSeverity.Info) 
        {
            if(preMessage != null) preMessage = preMessage + Environment.NewLine;
            string message = $"UTC {DateTime.UtcNow}: [{severity}]  {preMessage} {ex.Message}";
            if (ex.InnerException != null)
            {
                message += $"{Environment.NewLine} Inner Exception: {ex.InnerException.Message}";
            }

            string? directory = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.AppendAllText(logPath, message + Environment.NewLine);

        }

    }
}
