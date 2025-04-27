using static MEDIA_ON_THE_FLY.Logger.Logger;
using MEDIA_ON_THE_FLY.Settings;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    public class MOTF
    {
        public static Config Config;

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

            Logger.Logger.LogInfo($"Aggiunta chiave di avvio automatico per il programma [{filePath}]");
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

            Logger.Logger.LogInfo($"Rimossa chiave di avvio automatico per il programma");
        }

        public static void LoadSettings(string configPath)
        {
            if (!File.Exists(configPath))
            {
                LogWarning($"Non ho trovato il file di configurazione [{configPath}] - creo un file default");
                CreateDefaultSettings(configPath);
                return;
            }

            try
            {
                string plainJsonFile = File.ReadAllText(configPath);
                var jsonObj = JsonConvert.DeserializeObject<Config>(plainJsonFile, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    DefaultValueHandling = DefaultValueHandling.Include
                });

                Config = jsonObj;
            }
            catch (Exception ex)
            {
                LogWarning($"Il file di configurazione non è stato caricato correttamente: {ex.Message}");
                CreateDefaultSettings(configPath);
            }
        }

        public static void SaveSettings(string configPath)
        {
            string serializedObject = JsonConvert.SerializeObject(MOTF.Config, Formatting.Indented);
            var writer = File.CreateText(configPath);
            writer.Write(serializedObject);
            writer.Close();
            writer.Dispose();
        }

        public static void CreateDefaultSettings(string configPath)
        {
            try
            {
                // Crea un file di configurazione predefinito
                Config = new Config();

                string serializedObject = JsonConvert.SerializeObject(MOTF.Config, Formatting.Indented);
                var writer = File.CreateText(configPath);
                writer.Write(serializedObject);
                writer.Close();
                writer.Dispose();
            }
            catch (Exception ex)
            {
                LogWarning($"Il file di configurazione non è stato creato! Errore: {ex.Message}");
            }
        }
    }
}