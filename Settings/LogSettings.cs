using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY.Settings
{
    public class LogSettings
    {
        public string LogPath { get; set; } = Path.Combine(Application.StartupPath, $@"Logs");      // Path del file di log
        public string LogFileName { get; set; } = $"Log_{DateTime.Now.ToString("yyyyMMdd")}.txt";   // Nome del file di log
        public LOG_LEVEL CurrentLogLevel { get; set; } = LOG_LEVEL.INFO;
        public int SleepSpoolTime { get; set; } = 2000;  // Tempo di attesa per lo spool del log in millisecondi
        
        public enum LOG_LEVEL
        {
            DEBUG,
            INFO,
            WARNING,
            ERROR
        }
    }
}
