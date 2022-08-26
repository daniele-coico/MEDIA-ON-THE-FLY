using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    internal class MOTF
    {
        // Sotto cartelle per nome PC e nome utente in uso
        public static readonly string   MACHINENAME_AND_USERNAME = $@"\{Environment.MachineName}\{Environment.UserName}";

        // Cartelle utili all'applicazione
        public static readonly string   MOTF_FOLDER = Application.StartupPath + @"\motf";
        public static readonly string   USER_FOLDER = MOTF_FOLDER + MACHINENAME_AND_USERNAME;
        public static readonly string   TEMP_FOLDER = USER_FOLDER + @"\tmp";

        // Path file utili
        private static readonly string  LOG_PATH = USER_FOLDER + @"\log.txt";
        public  static readonly string  LOCK_PATH = USER_FOLDER + @"\motf.lock";

        /// <summary>
        /// Questo enumeratore indica vari modi di come l'utente intende può riprodurre i video
        /// </summary>
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

            // È possibile che il programma non possa scrivere all'interno del file di log.
            // Questo può succedere quando il file di log si trova su un server e si perde la connessione
            // con esso.
            try
            {
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
            }
            catch (Exception e)
            {
                LogText = ErrorLog(LogText, e);
            }

            LogText += "\r\n";
            return LogText;
        }

        /// <summary>
        /// Questo metodo viene richiamato per scrivere un log di emergenza in un file diverso.
        /// All'interno del file di log d'emergenza, che si trova nella cartella documenti dell'utente che ha
        /// avviato MOTF, si troverà l'ultimo messaggio da scrivere più l'eccezione.
        /// </summary>
        /// <param name="text">Ultimo messaggio di log da scrivere</param>
        /// <param name="e">Eccezione da loggare</param>
        /// <returns>Testo modificato da mostrare all'utente</returns>
        private static string ErrorLog(string text, Exception e = null)
        {
            string ErrorLogFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MEDIA ON-THE-FLY\\error.txt";

            text += $"\n!!! Errore nella scrittura del file di log: {e.Message} !!!\n{e.StackTrace}\n";

            // Inserisco tutti i dati anche nel file
            // Se non esiste lo creo
            if (!File.Exists(ErrorLogFile))
            {
                StreamWriter streamWriter = File.CreateText(ErrorLogFile);
                streamWriter.Flush();
                streamWriter.Close();
            }
            else
                File.AppendAllLines(ErrorLogFile, new[] { text });

            return text;
        }

        public static bool CheckSettingsFile() => File.Exists(USER_FOLDER + @"\config.set");

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

        /// <summary>
        /// Questa classe esegue un'ulteriore controllo con il metodo Exists della classe File ma controlla anche su quale
        /// dispositivo si può trovare il file.
        /// </summary>
        /// <param name="filePath">Path del file da riprodurre</param>
        /// <param name="fileLocation">Tipo di dispositivo su cui si trova il file</param>
        /// <returns>True se il file è stato trovato - False nel caso contrario</returns>
        public static bool FileExist(string filePath, out DriveType fileLocation)
        {
            bool fileExist = false;

            if (File.Exists(filePath) == false)
            {
                // Se anche questo controllo è falso vuol dire che il
                // disco che stiamo controllando non è presente fisicamente
                // nel sistema.
                if (FileRootIsPhysicalDisk(filePath) == false)
                    fileLocation = DriveType.Unknown;
                else
                    fileLocation = DriveType.Fixed;

                fileExist = false;
            }
            else
            {
                fileLocation = GetFileDriveType(filePath);
                fileExist = true;
            }

            return fileExist;
        }

        /// <summary>
        /// Questo metodo viene richiamato per sapere in che tipo di dispositivo è salvato il file.
        /// </summary>
        /// <param name="path">Path del file da riprodurre</param>
        /// <returns>Viene ritornato il tipo di dispositivo sulla quale è salvato il file.</returns>
        public static DriveType GetFileDriveType(string path)
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo(Directory.GetDirectoryRoot(path));
                return driveInfo.DriveType;
            }
            // Se la root non è conforma verrà lanciata un'eccezione da gestire - probabilmente il file è in rete.
            catch (ArgumentException)
            {
                Log("Il file da riprodurre non è su disco ma potrebbe esistere - ArgumentException");
            }
            // È anche possibile che il file non esista.
            catch (FileNotFoundException)
            {
                Log("Il file non si trova - FileNotFoundException");
            }

            // Controllo la root della path manualmente.
            if (FileRootIsPhysicalDisk(path))
                return DriveType.Fixed;
            else
                return DriveType.Unknown;
        }

        /// <summary>
        /// Il seguente metodo verifica i dischi logici presenti nel sistema e li distingue da quelli fisici.
        /// Viene poi controllata la root del file passato con i dischi fisici per capire se il file si può trovare
        /// in uno di questi dispositivi di massa.
        /// </summary>
        /// <param name="path">Path del file da riprodurre</param>
        /// <returns>True se il file si trovava in un disco fisico - False in caso contrario</returns>
        public static bool FileRootIsPhysicalDisk(string path)
        {
            bool IsFileOnDisk = false;
            string pathParent = Directory.GetParent(path).FullName;

            string[] logicalDisk = System.Environment.GetLogicalDrives();   // Dischi logici presenti nel sistema
            List<string> physicalDisk = new List<string>();                 // Dischi fisici presenti nel sistema

            string fileRoot = Path.GetPathRoot(pathParent);   // Root del file

            // Verifico i singoli dischi logici.
            // Se sono fisici li aggiungo alla lista.
            foreach (string logicalDiskItem in logicalDisk)
            {
                DriveInfo driveInfo = new DriveInfo(logicalDiskItem);
                DriveType driveType = driveInfo.DriveType;
                Log($"Disco logico trovato: {logicalDiskItem}");

                if (driveType != DriveType.Network && driveType != DriveType.Unknown && driveType != DriveType.NoRootDirectory)
                {
                    Log($"{logicalDiskItem} è anche un disco fisico");
                    physicalDisk.Add(logicalDiskItem);
                }
            }

            // Verifico la root con ogni disco fisico.
            // Se c'è una corrispondenza allora il file potrebbe esistere su disco.
            foreach (string root in physicalDisk)
                if (root == fileRoot)
                {
                    Log("La root del file da riprodurre corrisponde ad uno dei dischi trovati");
                    IsFileOnDisk = true;
                }

            return IsFileOnDisk;
        }
    }
}