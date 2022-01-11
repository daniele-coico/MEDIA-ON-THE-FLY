using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace MEDIA_ON_THE_FLY
{
    public partial class formPlayer : Form
    {
        private readonly int monitor;
        private readonly int volume;
        
        private readonly MOTF.PLAY_MODE playMode;

        private readonly FileInfo remoteFile;
        private readonly string remoteFilePath;
        private readonly string remoteFileDirectory;
        private readonly string localfilePath = Application.StartupPath + @"/tmp/loaded_video.mp4";
        
        public formPlayer(string _filePath, int monitor = 1, int _playMode = (int)MOTF.PLAY_MODE.FILE, int volume = 0)
        {
            InitializeComponent();
            MOTF.Log("Inizializzo i componenti");

            // Non posso passare per parametro un ENUM
            // quindi faccio il cast dell'intero passato.
            playMode = (MOTF.PLAY_MODE)_playMode;

            switch (playMode)
            { 
                case MOTF.PLAY_MODE.FILE:
                    remoteFile = new FileInfo(_filePath);
                    remoteFilePath = remoteFile.FullName;
                    remoteFileDirectory = remoteFile.DirectoryName; // Imposto la cartella da controllare
                    break;
                case MOTF.PLAY_MODE.PLAYLIST:
                    remoteFilePath = _filePath;
                    remoteFileDirectory = _filePath;
                    break;
                case MOTF.PLAY_MODE.FOLDER:
                    break;
                default:
                    return;
            }

            MOTF.Log($"Ho impostato la modalità di riproduzione {playMode}");

            // Imposto la posizione d'avvio del form
            // Il counter del monitor parte da 0 quindi tolgo 1 dalla variabile passata
            Bounds = Screen.AllScreens[monitor].Bounds;
            this.monitor = monitor;

            this.volume = volume;

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
            if (filePath == null && playMode == MOTF.PLAY_MODE.FILE)
                filePath = localfilePath;

            // Se la modalità di riproduzione è FOLDER allora credo una playlist
            // e inserisco tutti i video presenti nella cartella.
            // TODO: filtrare con le estensioni valide.
            if (playMode == MOTF.PLAY_MODE.FOLDER)
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

            wmpMedia.settings.volume = volume;  // Imposto il volume
            wmpMedia.Ctlenabled = false;        // Tolgo i controlli
            wmpMedia.settings.setMode("loop", true);    // Imposto il player per andare in loop

            wmpMedia.Ctlcontrols.play();    // Avvio il video

            // Tolgo l'interfaccia e nascondo il label
            wmpMedia.uiMode = "none";
            lblStatoVideo.Visible = false;
        }

        private void formPlayer_Load(object sender, EventArgs e)
        {
            MOTF.Log("Player caricato");

            // Controllo nuovamente l'esistenza del file/cartella
            switch (playMode)
            {
                case MOTF.PLAY_MODE.FILE:
                    if (!File.Exists(remoteFilePath))
                        Close();

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
            {
                MOTF.Log("Esco dallo schermo intero");
                Size = new Size(800, 600);  // Imposto una size stnadard
                WindowState = FormWindowState.Normal;       // Metto la finestra a normal
                FormBorderStyle = FormBorderStyle.Sizable;  // Imposto il BorderStyle

                Location = new Point(Location.X, Screen.AllScreens[monitor].WorkingArea.Y); // Correggo la location
            }
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
            }
        }

        private void formPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            // Se premo ESC o CANC esco dallo schermo intero
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete)
            {
                MOTF.Log("Esco dallo schermo intero");
                Size = new System.Drawing.Size(800, 600);   // Imposto una size stnadard
                WindowState = FormWindowState.Normal;       // Metto la finestra a normal
                FormBorderStyle = FormBorderStyle.Sizable;    // Imposto il BorderStyle

                Location = new Point(Location.X, Screen.AllScreens[monitor].WorkingArea.Y); // Correggo la location
            }
        }

        private void wmpMedia_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {

        }
    }
}
