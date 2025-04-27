using MEDIA_ON_THE_FLY.Settings;
using static MEDIA_ON_THE_FLY.Logger.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Count() > 0 && !string.IsNullOrEmpty(args[0]))
            {
                MOTF.LoadSettings(args[0]);
            }
            else
            {
                MOTF.LoadSettings(Config.ConfigPath);
            }

            // Imposto il logger
            InitializeLogger(MOTF.Config.LogSettings);
            LogInfo($"### MEDIA ON-THE-FLY avviato - Versione {Application.ProductVersion} ###");

            // Check file di lock
            if (File.Exists(Config.LockFilePath))
                LogWarning($"'L'ultima volta MEDIA ON-THE-FLY non è stato chiuso correttamente");
            else
                File.WriteAllText(Config.LockFilePath, "");

            // Inizio UI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new formHome());
        }
    }
}
