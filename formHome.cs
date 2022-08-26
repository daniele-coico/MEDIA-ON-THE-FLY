using SetDocs;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    public partial class formHome : Form
    {
        // Variabili
        public static readonly string versione = "0.2.2";

        private MOTF.PLAY_MODE _playMode;   // Modalità con la quale l'utente riprodurrà i file
        private string _filePath;           // Path del file da controllare
        private string _driveType;          // Ultimo tipo di dispositivo utilizzato per salvare il file
        private int _volume;                // Volume per il WMP impostato dalla trackbar

        private Thread threadUpdate;        // Thread dedicato al check degli update   

        public static bool DEBUG { get; } = true;

        public formHome()
        {
            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();

            // Imposto l'icona del programma
            Icon = Properties.Resources.img_641;

            // Thread per la verifica degli aggiornamenti
            threadUpdate = new Thread(CheckUpdate);
            threadUpdate.Name = "Thread update";
            if (DEBUG == false)
                threadUpdate.Start();

            if (File.Exists(MOTF.LOCK_PATH))
                tboxLog.Text = MOTF.Log($"L'ultima volta MEDIA ON-THE-FLY non è stato chiuso correttamente");
            else
            {
                tboxLog.Text = MOTF.Log($"MEDIA ON-THE-FLY avviato! - versione {versione}");
                File.WriteAllText(MOTF.LOCK_PATH, "");
            }


            // Argomenti:
            /*
             * -connect: specificato quando ci vuole connettere ad un server in modo automatico, dopo questo argomento è necessario un IP
             * -server:  da utilizzare quando l'applicazione deve entrare subito in modalità server
             */

            string[] args = Environment.GetCommandLineArgs();
            bool leaveLoop = false;

            for (int i = 1; i < args.Length && leaveLoop == false; i++)
            {
                string argument = args[i];

                switch (argument)
                {
                    case "-connect":
                        // Dopo il connect deve esserci un IP, senza di esso non faccio la connessione
                        if (string.IsNullOrEmpty(args[i++]) == false)
                        {
                            formFileCheck formFileCheck = new formFileCheck("Tentativo di connessione al server", "L'applicazione è stata riavvata a causa di un aggiornamento o dal server, attendi...", true);
                            formFileCheck.Show();
                            Application.DoEvents();
                            
                            // Inoltre, se il client è stato riavviato da server, devo dare il tempo di completare un
                            // eventuale update e avvio del server stesso. Metto il thread in sleep.
                            Thread.Sleep(60 * 1000);
                            new formClient(args[0], args[i++]).Show();
                            leaveLoop = true;
                            if (threadUpdate.IsAlive)
                                threadUpdate.Abort();

                            formFileCheck.Close();
                            formFileCheck.Dispose();
                        }
                        break;
                    case "-server":
                        if (threadUpdate.IsAlive)
                            threadUpdate.Abort();
                        new formServer().ShowDialog();
                        leaveLoop = true;
                        break;
                    default:
                        break;
                }
            }

        }

        private void StartPlayer()
        {
            if (_playMode == MOTF.PLAY_MODE.FILE || _playMode == MOTF.PLAY_MODE.FOLDER)
                if (string.IsNullOrEmpty(tboxPath.Text))
                {
                    MessageBox.Show("Seleziona una cartella o un file da riprodurre", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            // In base all'item selezionato nella cbox
            // imposto il monitor in cui mostrare il nuovo form.
            int selectedMonitor = comboBox1.SelectedIndex;

            // Faccio un'ulteriore controllo sul monitor
            // e se esiste apro il formPlayer
            if (Screen.AllScreens.Length < selectedMonitor)
                MessageBox.Show("Il monitor selezionato non esiste");
            else
            {
                // Apro il form player
                MessageBox.Show("Per uscire dal Media Player premere ESC", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SaveSettings(selectedMonitor);  // Salvo le impostazioni
                Hide();
                new formPlayer(_filePath, selectedMonitor, (int)_playMode, _volume).ShowDialog();
                Show();
            }
        }

        private void AutoStartPlayer(int playMode = 0, int monitor = 1, string filePath = "", int volume = 0, string driveType = "Fixed")
        {
            if (playMode == (int)MOTF.PLAY_MODE.FILE || playMode == (int)MOTF.PLAY_MODE.FOLDER)
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Seleziona una cartella o un file da riprodurre", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            // Faccio un'ulteriore controllo sul monitor
            // e se esiste apro il formPlayer
            if (Screen.AllScreens.Length < monitor)
                MessageBox.Show("Il monitor selezionato non esiste");
            else
            {
                // Apro il form player
                SaveSettings(monitor);  // Salvo le impostazioni
                Hide();
                new formPlayer(filePath, monitor, playMode, volume, driveType).ShowDialog();
                Show();
            }
        }

        private void SaveSettings(int monitor = 0)
        {
            // Scrivo le impostazioni
            WriteSettings writeSettings = new WriteSettings(MOTF.USER_FOLDER, "config");
            writeSettings.AddSetting("play_mode", (int)_playMode);
            writeSettings.AddSetting("monitor", monitor);
            writeSettings.AddSetting("path", _filePath);
            writeSettings.AddSetting("volume", _volume);
            writeSettings.AddSetting("driveType", MOTF.GetFileDriveType(_filePath).ToString());

            // copia sempre in locale etc. da fare
        }

        private void CheckUpdate()
        {
            // Online versione
            // https://raw.githubusercontent.com/daniele-coico/MEDIA-ON-THE-FLY/master/update/master_version

            while (true)
            {
                // Ottengo la versione disponibile online
                System.Net.WebClient webClient = new System.Net.WebClient();
                byte[] data = webClient.DownloadData("https://raw.githubusercontent.com/daniele-coico/MEDIA-ON-THE-FLY/master/update/master_version");
                string versioneOnline = System.Text.Encoding.ASCII.GetString(data).Trim();

                // Se la versione attuale è diversa da quella online
                // allora scarico quella più recente.
                if (versione != versioneOnline)
                {
                    // Scarico l'installer e lo salvo
                    webClient.DownloadFile("https://raw.githubusercontent.com/daniele-coico/MEDIA-ON-THE-FLY/master/update/Installer.exe", Application.StartupPath + @".\Installer.exe");

                    // Avvio l'installer con argomento True per avviare
                    // l'aggiornamento automatico.
                    System.Diagnostics.Process.Start(@".\Installer.exe", true.ToString());

                    Application.Exit();
                }

                // Metto il thread in sleep per almeno 120 secondi
                Thread.Sleep(300 * 1000);
            }
        }

        private void formHome_Load(object sender, EventArgs e)
        {
            // Controllo se il programma è impostato
            // per avviarsi con Windows.
            if (MOTF.CheckSettingsFile() && MOTF.CheckStartupItem())
            {
                tboxLog.Text = MOTF.Log("Impostazione d'avvio trovata! Avvio il player con le ultime impostazioni");
                cboxAvvio.Checked = true;

                // Lettura impostazioni
                ReadSettings readSettings = new ReadSettings(MOTF.USER_FOLDER, "config");
                _playMode = (MOTF.PLAY_MODE)readSettings.ReadInt("play_mode");
                int monitor = readSettings.ReadInt("monitor");
                _filePath = readSettings.ReadString("path");
                _volume = readSettings.ReadInt("volume");
                _driveType = readSettings.ReadString("driveType");

                // Impostazione dei vari controlli utente
                comboBox1.SelectedItem = $"{monitor}";
                tboxPath.Text = _filePath;
                tbarVolume.Value = _volume;

                AutoStartPlayer((int)_playMode, monitor, _filePath, _volume, _driveType);
            }

            int numeroMonitor = 1; // Il numero minimo di monitor è 1 (?)

            // Aggiungo tutti i monitor all'interno della ComboBox
            foreach (Screen screen in Screen.AllScreens)
            {
                // Se lo schermo è primario lo specifico nella cbox
                if (screen.Primary)
                {
                    comboBox1.Items.Add($"{numeroMonitor} - primario");
                    comboBox1.SelectedItem = $"{numeroMonitor} - primario";
                }
                else
                    comboBox1.Items.Add($"{numeroMonitor}");

                // Incremento il numero di monitor
                numeroMonitor++;
            }
        }

        private void formHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(MOTF.LOCK_PATH);
            MOTF.Log("!! MOTF chiuso !!");
        }

        #region "Componenti WinForm"
        private void btnCartella_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Seleziona cartella con i video da riprodurre";
            folderBrowserDialog.ShowNewFolderButton = true;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                _filePath = folderBrowserDialog.SelectedPath;
                tboxPath.Text = _filePath;
            }
            else
                MessageBox.Show("Percorso non valido", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnPlaylist_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void tbarVolume_Scroll(object sender, EventArgs e)
        {
            _volume = tbarVolume.Value;
        }

        private void cboxCartella_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxCartella.Checked)
            {
                btnCartella.Enabled = true;
                btnSeleziona.Enabled = false;
                btnPlaylist.Enabled = false;
                cboxPlaylist.Enabled = false;
                _playMode = MOTF.PLAY_MODE.FOLDER;
            }
            else
            {
                cboxPlaylist.Enabled = true;
                btnSeleziona.Enabled = true;
                btnPlaylist.Enabled = true;
                _playMode = MOTF.PLAY_MODE.FILE;
            }
        }

        private void cboxAvvio_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxAvvio.Checked)
                MOTF.AddStartupKey(Application.ExecutablePath.ToString());
            else
                MOTF.RemoveStartupKey();
        }

        private void cboxPlaylist_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxPlaylist.Checked)
            {
                cboxCartella.Enabled = false;
                btnSeleziona.Enabled = false;
                btnCartella.Enabled = false;
                btnPlaylist.Enabled = true;
                _playMode = MOTF.PLAY_MODE.PLAYLIST;
            }
            else
            {
                cboxCartella.Enabled = true;
                btnSeleziona.Enabled = true;
                btnPlaylist.Enabled = false;
                _playMode = MOTF.PLAY_MODE.FILE;
            }
        }

        private void btnSeleziona_Click(object sender, EventArgs e)
        {
            // Apro un file dialog e consento all'utente di selezionare un file
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "File video|*.mp4";
            fileDialog.Title = "Seleziona file";

            // Se il DialogResult è OK allora imposto il filePath
            if (fileDialog.ShowDialog() == DialogResult.OK)
                _filePath = fileDialog.FileName;

            // Imposto la path nella tbox
            tboxPath.Text = _filePath;
        }

        private void btnChiudi_Click(object sender, EventArgs e)
        {
            // Chiudo l'applicazione
            Application.Exit();
        }

        private void btnAvvia_Click(object sender, EventArgs e)
        {
            StartPlayer();
        }
        #endregion

        private void btnServer_Click(object sender, EventArgs e)
        {
            Hide();
            new formServer().ShowDialog();
            Show();
        }

        static formClient formClient;

        private void btnConnettiAServer_Click(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            if (formClient == null || formClient.IsDisposed)
            {
                formClient = new formClient();
                formClient.Show();
            }
            else
                formClient.Visible = true;

            threadUpdate.Abort();
        }
    }
}