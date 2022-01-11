using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    internal class MOTF
    {

        private static string LOG_PATH = Application.StartupPath + "\\log.txt";    // Director nella quale salvare il file di log

        public enum PLAY_MODE
        {
            FILE = 0,
            PLAYLIST = 1,
            FOLDER = 2
        }

        /// <summary>
        /// La funzione di Log serve a mostrare tutte le operazioni effettuate dal programma.
        /// L'output viene anche salvato su un file di testo.
        /// </summary>
        /// <param name="txt">Stringa da aggiungere al Log</param>
        /// <returns>Stirnga formattata in modo corretto</returns>
        public static string Log(string txt,
                         char firstChar = '>',
                         bool orario = true)
        {
            string LogText = $"{firstChar} [{DateTime.Now}] {txt}"; // formatto il testo in modo corretto

            if (orario == false)
                LogText = $"{firstChar} {txt}";

            // Inserisco tutti i dati anche nel file
            // Se non esiste lo creo
            if (!File.Exists(LOG_PATH))
            {
                StreamWriter streamWriter = File.CreateText(LOG_PATH);
                streamWriter.Flush();
                streamWriter.Close();
            }
            else
                File.AppendAllLines(LOG_PATH, new[] { LogText });

            return LogText;
        }

        public static bool CheckSettingsFile() => File.Exists(Application.StartupPath + "\\config.set");

        /// <summary>
        /// Metodo per verificare l'esistenza di una chiave che consenta l'avvio automatico del programma.
        /// </summary>
        /// <returns>Presenza della chiave su registro per l'avvio automatico.</returns>
        public static bool CheckStartupItem()
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rkApp.GetValue("MOTF") == null)
                // The value doesn't exist, the application is not set to run at startup
                return false;
            else
                // The value exists, the application is set to run at startup
                return true;
        }

        /// <summary>
        /// Aggiunge al registro una chiave per avviare in automatico il programma.
        /// </summary>
        public static void AddStartupKey(string filePath)
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (CheckStartupItem() == false)
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("MOTF", filePath);
        }

        /// <summary>
        /// Rimuove dal registro la chiave per avviare in automatico il programma.
        /// </summary>
        public static void RemoveStartupKey()
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (CheckStartupItem())
                // Remove the value from the registry so that the application doesn't start
                rkApp.DeleteValue("MOTF", false);
        }
    }
}