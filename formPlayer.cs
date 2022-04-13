using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace MEDIA_ON_THE_FLY
{
    public partial class formPlayer : Form
    {
        private readonly int _monitor;
        private readonly int _volume;

        private readonly MOTF.PLAY_MODE _playMode;
        private readonly DriveType _lastDriveType;

        private readonly FileInfo remoteFile;
        private readonly string remoteFilePath;
        private readonly string remoteFileDirectory;
        private readonly string localfilePath = Application.StartupPath + @"/tmp/loaded_video.mp4";

        public formPlayer(string filePath, int monitor = 1, int playMode = (int)MOTF.PLAY_MODE.FILE, int volume = 0, string driveType = "Fixed")
        {
            InitializeComponent();
            MOTF.Log("Inizializzo i componenti");

            // Non posso passare per parametro un ENUM
            // quindi faccio il cast dell'intero passato.
            _playMode = (MOTF.PLAY_MODE)playMode;

            switch (_playMode)
            {
                case MOTF.PLAY_MODE.FILE:
                    remoteFile = new FileInfo(filePath);
                    remoteFilePath = remoteFile.FullName;
                    remoteFileDirectory = remoteFile.DirectoryName; // Imposto la cartella da controllare
                    break;
                case MOTF.PLAY_MODE.PLAYLIST:
                    remoteFilePath = filePath;
                    remoteFileDirectory = filePath;
                    break;
                case MOTF.PLAY_MODE.FOLDER:
                    break;
                default:
                    return;
            }

            // Ultimo DriveType dell'ultimo dispositivo utilizzato
            _lastDriveType = SetDriveType(driveType);

            MOTF.Log($"Ho impostato la modalità di riproduzione: {_playMode}");

            // Imposto la posizione d'avvio del form
            // Il counter del _monitor parte da 0 quindi tolgo 1 dalla variabile passata
            Bounds = Screen.AllScreens[monitor].Bounds;
            _monitor = monitor - 1;

            _volume = volume;

            // Imposto l'icona della finestra
            Icon = Properties.Resources.img_641;
        }

        private void StartNewVideo(string filePath = null)
        {
            // Se il MediaPlayer è null allora ne creo uno nuovo
            if (wmpMedia == null)
                wmpMedia = new AxWMPLib.AxWindowsMediaPlayer();

            MOTF.Log("WMP esistente/creato");

            // Se non viene passato alcun parametro imposto
            // il file locale come video da riprodurre.
            if (filePath == null && _playMode == MOTF.PLAY_MODE.FILE)
                filePath = localfilePath;

            // Se la modalità di riproduzione è FOLDER allora creo una playlist
            // e inserisco tutti i video presenti nella cartella.
            // TODO: filtrare con le estensioni valide.
            if (_playMode == MOTF.PLAY_MODE.FOLDER)
            {
                wmpMedia.currentPlaylist = wmpMedia.newPlaylist("temp", "");
                foreach (string videoPath in Directory.GetFiles(remoteFilePath))
                    wmpMedia.currentPlaylist.appendItem(wmpMedia.newMedia(videoPath));
            }
            else
            {
                wmpMedia.URL = filePath;   // Imposto la path video
            }

            MOTF.Log("Impostata la path per il/i video");

            WindowState = FormWindowState.Maximized; // Massimizzo lo stato della finestra

            wmpMedia.settings.volume = _volume;  // Imposto il _volume
            wmpMedia.Ctlenabled = false;        // Tolgo i controlli
            wmpMedia.settings.setMode("loop", true);    // Imposto il player per andare in loop

            wmpMedia.Ctlcontrols.play();    // Avvio il video

            // Tolgo l'interfaccia e nascondo il label
            wmpMedia.uiMode = "none";
            lblStatoVideo.Visible = false;
        }

        private void EscKeyPress()
        {
            MOTF.Log("Esco dallo schermo intero");

            wmpMedia.fullScreen = false;
            Size = new System.Drawing.Size(800, 600);   // Imposto una size stnadard
            WindowState = FormWindowState.Normal;       // Metto la finestra a normal
            FormBorderStyle = FormBorderStyle.Sizable;    // Imposto il BorderStyle

            Location = new Point(Location.X, Screen.AllScreens[_monitor].WorkingArea.Y); // Correggo la location
        }

        private DriveType SetDriveType(string driveTypeString)
        {
            switch (driveTypeString)
            {
                case "Unknown":
                    return DriveType.Unknown;
                case "NoRootDirectory":
                    return DriveType.NoRootDirectory;
                case "Removable":
                    return DriveType.Removable;
                case "Fixed":
                    return DriveType.Fixed;
                case "Network":
                    return DriveType.Network;
                case "CDRom":
                    return DriveType.CDRom;
                case "Ram":
                    return DriveType.Ram;
                default:
                    return DriveType.Unknown;
            }
        }

        private void formPlayer_Load(object sender, EventArgs e)
        {
            int fileCheckStatus = 1;
            MOTF.Log("Player caricato");

            // Controllo nuovamente l'esistenza del file/cartella
            switch (_playMode)
            {
                case MOTF.PLAY_MODE.FILE:
                    // Se il file non esiste inizio a fare più controlli
                    if (!File.Exists(remoteFilePath))
                    {
                        formFileCheck formFileCheck = new formFileCheck(remoteFilePath, _lastDriveType);
                        formFileCheck.Show();
                        Application.DoEvents(); // Per qualche motivo se non lasciamo questa istruzione il fileCheckStatus non viene aggiornato.
                        fileCheckStatus = formFileCheck.CheckStatus;
                    }

                    if (fileCheckStatus == 0)
                    {
                        MOTF.Log("File non trovato");
                        MessageBox.Show("Il file da riprodurre non esiste. Controlla la directory inserita e se il dispositivo dove si trova il file è collegato", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close();
                        return;
                    }

                    MOTF.Log("Copio il video in locale e avvio la riproduzione");

                    fswVideo.Path = remoteFileDirectory;    // Dico al FileSystemWatcher quale cartella guardare
                    fswVideo.Filter = remoteFile.Name;      // Imposto il filtro per controllare un solo video

                    File.Copy(remoteFilePath, localfilePath, true); // Copio il file nella cartella locale

                    StartNewVideo(); // Avvio il video

                    break;
                case MOTF.PLAY_MODE.PLAYLIST:
                    // TODO
                    break;
                case MOTF.PLAY_MODE.FOLDER:
                    if (!Directory.Exists(remoteFilePath))
                        Close();

                    MOTF.Log("Avvio la riprodzione video della cartella");

                    fswVideo.Path = remoteFileDirectory;
                    StartNewVideo(remoteFileDirectory);

                    break;
                default:
                    break;
            }
        }

        protected virtual bool IsFileLocked(FileInfo file)
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
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                MOTF.Log("File da eliminare bloccato");
                return true;
            }

            //file is not locked
            return false;
        }

        private void fswVideo_Changed(object sender, FileSystemEventArgs e)
        {
            // Quando lo stato del FileSystemWatcher cambia allora
            // importo un nuovo video
            lblStatoVideo.Visible = true;

            wmpMedia.Ctlcontrols.stop(); // Fermo il media player      
            wmpMedia.close(); // Chiudo la risora in uso dal wmp
            fswVideo.Path = remoteFileDirectory;

            FileInfo info = new FileInfo(localfilePath); // Prelevo informazioni riguardo al file locale

            MOTF.Log("È cambiato il file video! Chiudo l'oggetto WMP e importo il nuovo file");

            while (IsFileLocked(info)) // Fintato che il file è bloccato non faccio niente
            {
                Application.DoEvents();
            }

            Thread.Sleep(500); // Metto in pausa il thread per almeno 500ms

            try
            {
                File.Copy(remoteFilePath, localfilePath, true); // Provo a copiare il file
            }
            catch (IOException)
            {
                // Se non riesco faccio una pausa più lunga
                Console.Write("Errore copia video");
                lblStatoVideo.Text = "Nuovo tentativo copia...";
                MOTF.Log("Errore nella copia video - nuovo tentativo");
                Thread.Sleep(3000);
            }
            finally
            {
                File.Copy(remoteFilePath, localfilePath, true); // Provo nuovamente la copia
            }

            while (IsFileLocked(info))
            {
                Application.DoEvents();
            }

            MOTF.Log("Nuovo file copiato");
            Thread.Sleep(500); // Metto in pausa il thread per almeno 500ms

            MOTF.Log("Inizio a riprodurlo");
            lblStatoVideo.Visible = false;
            StartNewVideo(); // Avvio il video
        }

        private void formPlayer_FormClosed(object sender, FormClosedEventArgs e)
        {
            MOTF.Log("Esco dal player");
            wmpMedia.close();
        }

        private void wmpMedia_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            // Se premo ESC o CANC esco dallo schermo intero
            if (e.nKeyCode == (short)Keys.Escape || e.nKeyCode == (short)Keys.Delete)
                EscKeyPress();
        }

        private void formPlayer_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                MOTF.Log("Torno a schermo intero");
                FormBorderStyle = FormBorderStyle.None;

                Rectangle rect = Screen.FromHandle(this.Handle).WorkingArea;
                rect.Location = new Point(0, 0);
                MaximizedBounds = rect;

                WindowState = FormWindowState.Maximized;

                wmpMedia.fullScreen = true;
            }
        }

        private void formPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            // Se premo ESC o CANC esco dallo schermo intero
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete)
                EscKeyPress();
        }

        private void wmpMedia_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {

        }

        private void lblStatoVideo_Click(object sender, EventArgs e)
        {

        }
    }
}
