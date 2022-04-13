using SetDocs;
using System;
using System.IO;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    public partial class formHome : Form
    {
        // Variabili
        private const string versione = "0.2";
        
        private string _filePath;           // Path del file da controllare
        private MOTF.PLAY_MODE _playMode;   // Modalità con la quale l'utente riprodurrà i file
        private int _volume;                // Volume per il WMP impostato dalla trackbar
        private string _driveType;          // Ultimo tipo di dispositivo utilizzato per salvare il file

        public formHome()
        {
            InitializeComponent();

            // Imposto l'icona del programma
            Icon = Properties.Resources.img_641;
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

            // Creo la cartella tmp se non esiste
            if (!System.IO.Directory.Exists(Application.StartupPath + @"\tmp"))
                System.IO.Directory.CreateDirectory(Application.StartupPath + @"\tmp");

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

        private void StartPlayer()
        {
            if (_playMode == MOTF.PLAY_MODE.FILE || _playMode == MOTF.PLAY_MODE.FOLDER)
                if (string.IsNullOrEmpty(tboxPath.Text))
                {
                    MessageBox.Show("Seleziona una cartella o un file da riprodurre", "Attenizone", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                new formPlayer(_filePath, selectedMonitor, (int)_playMode, _volume).ShowDialog();
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
                new formPlayer(filePath, monitor, playMode, volume, driveType).ShowDialog();
            }
        }

        private void SaveSettings(int monitor = 0)
        {
            // Scrivo le impostazioni
            WriteSettings writeSettings = new WriteSettings(Application.StartupPath, "config");
            writeSettings.AddSetting("play_mode", (int)_playMode);
            writeSettings.AddSetting("monitor", monitor);
            writeSettings.AddSetting("path", _filePath);
            writeSettings.AddSetting("volume", _volume);
            writeSettings.AddSetting("driveType", MOTF.GetFileDriveType(_filePath).ToString());

            // copia sempre in locale etc. da fare
        }

        private void formHome_Load(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\motf.lock"))
                tboxLog.Text = MOTF.Log($"L'ultima volta MEDIA ON-THE-FLY non è stato chiuso correttamente");
            else
            { 
                tboxLog.Text = MOTF.Log($"MEDIA ON-THE-FLY avviato! - versione {versione}");
                File.WriteAllText(Application.StartupPath + "\\motf.lock", "");
            }

            // Controllo se il programma è impostato
            // per avviarsi con Windows.
            if (MOTF.CheckSettingsFile() && MOTF.CheckStartupItem())
            {
                tboxLog.Text = MOTF.Log("Impostazione d'avvio trovata! Avvio il player con le ultime impostazioni");
                cboxAvvio.Checked = true;

                // Lettura impostazioni
                ReadSettings readSettings = new ReadSettings(Application.StartupPath, "config");
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
        private void cboxAvvio_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxAvvio.Checked)
                MOTF.AddStartupKey(Application.ExecutablePath.ToString());
            else
                MOTF.RemoveStartupKey();
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

        }

        private void tbarVolume_Scroll(object sender, EventArgs e)
        {
            _volume = tbarVolume.Value;
        }

        private void formHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(Application.StartupPath + "\\motf.lock");
            MOTF.Log("!! MOTF chiuso !!");
        }
    }
}
