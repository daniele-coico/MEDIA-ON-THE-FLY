using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Management;
using MEDIA_ON_THE_FLY.Settings;
using static MEDIA_ON_THE_FLY.Logger.Logger;

namespace MEDIA_ON_THE_FLY
{
    public partial class formPlayer : Form
    {
        WMPSettings WMPSettings = MOTF.Config.WMPSettings; // Imposto le impostazioni del WMP

        private readonly FileInfo remoteFile;
        private readonly string remoteFilePath;
        private readonly string remoteFileDirectory;
        private readonly string localfilePath = Path.Combine(Application.StartupPath, @"/tmp/loaded_video.mp4");

        public formPlayer(WMPSettings wmpSettings)
        {
            InitializeComponent();
            LogInfo("Inizializzo i componenti");
            WMPSettings = wmpSettings;

            switch (WMPSettings.PlayMode)
            {
                case WMPSettings.PLAY_MODE.FILE:
                    remoteFile = new FileInfo(WMPSettings.FilePath);
                    remoteFilePath = remoteFile.FullName;
                    remoteFileDirectory = remoteFile.DirectoryName; // Imposto la cartella da controllare
                    break;
                case WMPSettings.PLAY_MODE.PLAYLIST:
                    remoteFilePath = wmpSettings.FilePath;
                    remoteFileDirectory = wmpSettings.FilePath;
                    break;
                case WMPSettings.PLAY_MODE.FOLDER:
                    break;
                default:
                    return;
            }

            LogInfo($"Ho impostato la modalità di riproduzione {WMPSettings.PlayMode}");

            // Imposto la posizione d'avvio del form
            // Il counter del monitor parte da 0 quindi tolgo 1 dalla variabile passata
            Bounds = Screen.AllScreens[wmpSettings.Monitor].Bounds;

            // Imposto l'icona della finestra
            Icon = Properties.Resources.img_641;
        }

        private void StartNewVideo(string filePath = null)
        {
            // Se il MediaPlayer è null allora ne creo uno nuovo
            if (wmpMedia == null)
                wmpMedia = new AxWMPLib.AxWindowsMediaPlayer();

            LogDebug("WMP esistente/creato");

            // Se non viene passato alcun parametro imposto
            // il file locale come video da riprodurre.
            if (filePath is null && WMPSettings.PlayMode == WMPSettings.PLAY_MODE.FILE)
                filePath = localfilePath;

            // Se la modalità di riproduzione è FOLDER allora credo una playlist
            // e inserisco tutti i video presenti nella cartella.
            // TODO: filtrare con le estensioni valide.
            if (WMPSettings.PlayMode == WMPSettings.PLAY_MODE.FOLDER)
            {
                wmpMedia.currentPlaylist = wmpMedia.newPlaylist("temp", "");
                foreach (string videoPath in Directory.GetFiles(remoteFilePath))
                    wmpMedia.currentPlaylist.appendItem(wmpMedia.newMedia(videoPath));
            }
            else
            {
                wmpMedia.URL = filePath;   // Imposto la path video
            }

            LogDebug($"Impostata la path per il/i video: [{filePath}]");

            WindowState = FormWindowState.Maximized;    // Massimizzo lo stato della finestra

            wmpMedia.settings.volume = WMPSettings.Volume;  // Imposto il volume
            wmpMedia.Ctlenabled = false;                    // Tolgo i controlli
            wmpMedia.settings.setMode("loop", true);        // Imposto il player per andare in loop

            wmpMedia.Ctlcontrols.play();    // Avvio il video

            // Tolgo l'interfaccia e nascondo il label
            wmpMedia.uiMode = "none";
            lblStatoVideo.Visible = false;
        }

        private void formPlayer_Load(object sender, EventArgs e)
        {
            LogInfo("Player caricato");

            // Controllo nuovamente l'esistenza del file/cartella
            switch (WMPSettings.PlayMode)
            {
                case WMPSettings.PLAY_MODE.FILE:
                    if (!File.Exists(remoteFilePath))
                        Close();

                    LogInfo("Copio il video in locale e avvio la riproduzione");

                    fswVideo.Path = remoteFileDirectory;    // Dico al FileSystemWatcher quale cartella guardare
                    fswVideo.Filter = remoteFile.Name;      // Imposto il filtro per controllare un solo video

                    File.Copy(remoteFilePath, localfilePath, true); // Copio il file nella cartella locale

                    StartNewVideo(); // Avvio il video

                    break;
                case WMPSettings.PLAY_MODE.PLAYLIST:
                    // TODO
                    break;
                case WMPSettings.PLAY_MODE.FOLDER:
                    if (!Directory.Exists(remoteFilePath))
                        Close();

                    LogInfo("Avvio la riprodzione video della cartella");

                    fswVideo.Path = remoteFileDirectory;
                    StartNewVideo(remoteFileDirectory);

                    break;
                default:
                    break;
            }
        }

        protected virtual bool IsFileLocked(FileInfo file, out string processBlocking, out string PID)
        {
            try
            {
                // Provo ad aprire il file richiesto
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                // Provo a recuperare l'applicazione che blocca il file
                processBlocking = GetFileLockingProcess(file.FullName, out PID);
                if (string.IsNullOrEmpty(processBlocking))
                {
                    LogWarning($"Il file [{file.FullName}] è bloccato e non riesco a cancellarlo. Non riesco a recuperare il processo che lo blocca");
                }
                else
                {
                    LogWarning($"Il file è bloccato da [{processBlocking}] con PID [{PID}]");
                }
                
                Thread.Sleep(MOTF.Config.GlobalSettings.FileLockTimeout);
                return true;
            }

            // File is not locked
            processBlocking = string.Empty;
            PID = string.Empty;
            return false;
        }

        protected virtual string GetFileLockingProcess(string filePath, out string processId)
        {
            try
            {
                string query = $"SELECT * FROM Win32_Process WHERE Handle IN (SELECT Handle FROM Win32_Lock WHERE Path = '{filePath.Replace("\\", "\\\\")}')";
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string processName = obj["Name"]?.ToString();
                        processId = obj["ProcessId"]?.ToString();
                        return processName;
                    }
                }
                
                processId = string.Empty;
                return string.Empty;
            }
            catch (Exception)
            {
                processId = string.Empty;
                return string.Empty;
            }
        }

        private void fswVideo_Changed(object sender, FileSystemEventArgs e)
        {
            // Quando lo stato del FileSystemWatcher cambia allora importo un nuovo video
            lblStatoVideo.Visible = true;

            // Fermo il media player
            wmpMedia.Ctlcontrols.stop();

            // Chiudo la risora in uso dal wmp
            wmpMedia.close();
            fswVideo.Path = remoteFileDirectory;

            // Prelevo informazioni riguardo al file locale
            FileInfo localFileInfo = new FileInfo(localfilePath);
            LogInfo("È cambiato il file video! Chiudo l'oggetto WMP e importo il nuovo file");

            // Fintato che il file è bloccato non faccio niente
            while (IsFileLocked(localFileInfo, out _, out _))
            {
                Application.DoEvents();
            }

            Thread.Sleep(MOTF.Config.GlobalSettings.SleepBeforeVideoRestart); 
            CopyFileLocaly(remoteFilePath, localfilePath);

            LogInfo("Nuovo file copiato - inizio riproduzione");
            Thread.Sleep(MOTF.Config.GlobalSettings.SleepBeforeVideoRestart); // Metto in pausa il thread per almeno 500ms

            lblStatoVideo.Visible = false;
            StartNewVideo(); // Avvio il video
        }

        private bool CopyFileLocaly(string sourcePath, string localPath, int tentative = 1)
        {
            bool result = false;

            if (tentative > MOTF.Config.GlobalSettings.MaxTryFileCopy)
            {
                LogError($"Non riesco a copiare il file [{sourcePath}] in locale dopo {tentative} tentativi");
                return false;
            }

            LogInfo($"Provo a copiare il file [{sourcePath}] in [{localPath}]. Tentativo [{tentative}]");

            try
            {
                // Provo a copiare il file
                File.Copy(remoteFilePath, localfilePath, true);
                result = true;
            }
            catch (IOException)
            {
                // Se non riesco metto in pausa il thread e riprovo
                lblStatoVideo.Text = "Nuovo tentativo copia file...";
                LogWarning($"Errore nella copia video - provo un nuovo tentativo");
                Thread.Sleep(MOTF.Config.GlobalSettings.FileLockTimeout);
                tentative++;
                return CopyFileLocaly(sourcePath, localPath, tentative);
            }

            return result;
        }

        private void formPlayer_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogInfo("Esco dal player");
            wmpMedia.close();
        }

        private void wmpMedia_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            // Se premo ESC o CANC esco dallo schermo intero
            if (e.nKeyCode == (short)Keys.Escape || e.nKeyCode == (short)Keys.Delete)
            {
                LogDebug("Esco dallo schermo intero");
                Size = new Size(WMPSettings.DefaultFormSize.Width, WMPSettings.DefaultFormSize.Height);  // Imposto una size stnadard
                WindowState = FormWindowState.Normal;       // Metto la finestra a normal
                FormBorderStyle = FormBorderStyle.Sizable;  // Imposto il BorderStyle

                Location = new Point(Location.X, Screen.AllScreens[WMPSettings.Monitor].WorkingArea.Y); // Correggo la location
            }
        }

        private void formPlayer_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                LogInfo("Torno a schermo intero");
                FormBorderStyle = FormBorderStyle.None;

                Rectangle rect = Screen.FromHandle(this.Handle).WorkingArea;
                rect.Location = new Point(0, 0);
                MaximizedBounds = rect;

                WindowState = FormWindowState.Maximized;
            }
        }

        private void formPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            // Se premo ESC o CANC esco dallo schermo intero
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete)
            {
                LogDebug("Esco dallo schermo intero");
                Size = new System.Drawing.Size(WMPSettings.DefaultFormSize.Width, WMPSettings.DefaultFormSize.Height);   // Imposto una size stnadard
                WindowState = FormWindowState.Normal;       // Metto la finestra a normal
                FormBorderStyle = FormBorderStyle.Sizable;    // Imposto il BorderStyle

                Location = new Point(Location.X, Screen.AllScreens[WMPSettings.Monitor].WorkingArea.Y); // Correggo la location
            }
        }

        private void wmpMedia_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {

        }
    }
}
