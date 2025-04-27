using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY.Settings
{
    public class Config
    {
        public static string ConfigPath = Path.Combine(Application.StartupPath, "Config.json"); // Path del file di configurazione
        public static string LockFilePath = Path.Combine(Application.StartupPath, "MOTF.lock"); // File di lock per verificare l'ultimo stato di esecuzione del programma

        public GlobalSettings GlobalSettings { get; set; } = new GlobalSettings();
        public WMPSettings WMPSettings { get; set; } = new WMPSettings();
        public LogSettings LogSettings { get; set; } = new LogSettings();
    }
}
