using MEDIA_ON_THE_FLY.Settings;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using static MEDIA_ON_THE_FLY.Settings.LogSettings;

namespace MEDIA_ON_THE_FLY.Logger
{
    public static class Logger
    {
        private static LogSettings LogSettings { get; set; } = new LogSettings();
        private static ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        private static string _logFilePath;

        public static void InitializeLogger(LogSettings logSettings)
        {
            LogSettings = logSettings;
            _logFilePath = Path.Combine(LogSettings.LogPath, LogSettings.LogFileName);

            Thread thread = new Thread(FlushLog);
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Name = "LogThread";
            thread.Start();
        }

        public static void LogDebug(string text)
        {
            if (LogSettings.CurrentLogLevel == LOG_LEVEL.DEBUG)
                Log(text, "DEBUG", true);
        }

        public static void LogInfo(string text)
        {
            if (LogSettings.CurrentLogLevel <= LOG_LEVEL.INFO)
                Log(text, "INFO", true);
        }

        public static void LogWarning(string text)
        {
            if (LogSettings.CurrentLogLevel <= LOG_LEVEL.WARNING)
                Log(text, "WARNING", true);
        }

        public static void LogError(string text)
        {
            if (LogSettings.CurrentLogLevel <= LOG_LEVEL.ERROR)
                Log(text, "ERROR", true);
        }

        /// <summary>
        /// Questo metodo mette in coda un log che verrà gestito da un thread dedicato.
        /// </summary>
        /// <param name="txt">Stringa da aggiungere al Log</param>
        /// <returns>Stirnga formattata in modo corretto</returns>
        private static string Log(string txt, string logLevel, bool orario = true)
        {
            string LogText = $"[{DateTime.Now}] [{logLevel}]\t[{txt}]"; // formatto il testo in modo corretto

            if (orario == false)
                LogText = $"[{logLevel}]\t[{txt}]";

            _logQueue.Enqueue(LogText);
            return LogText;
        }

        /// <summary>
        /// Questo metodo fa un get dalla coda e scrive il log su file.
        /// </summary>
        private static void FlushLog()
        {
            StreamWriter streamWriter;

            if (Directory.Exists(LogSettings.LogPath) == false)
            {
                Directory.CreateDirectory(LogSettings.LogPath);
            }

            if (!File.Exists(_logFilePath))
            {
                streamWriter = File.CreateText(_logFilePath);
            }
            else
            {
                var fileStream = File.Open(_logFilePath, FileMode.Append);
                streamWriter = new StreamWriter(fileStream);
            }

            streamWriter.AutoFlush = true;

            while (true)
            {
                // Flush the log queue to the file
                while (_logQueue.TryDequeue(out string logEntry))
                {
                    streamWriter.WriteLine(logEntry);
                }

                Thread.Sleep(LogSettings.SleepSpoolTime);
            }
        }
    }
}
