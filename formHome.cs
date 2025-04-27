using MEDIA_ON_THE_FLY.Settings;
using SetDocs;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static MEDIA_ON_THE_FLY.Logger.Logger;

namespace MEDIA_ON_THE_FLY
{
    public partial class formHome : Form
    {
        public formHome()
        {
            // Inizializzo il form
            InitializeComponent();
            // Imposto l'icona del programma
            Icon = Properties.Resources.img_641;
            // Imposto i componenti del form in base alle impostazioni
            LoadFormSettings(MOTF.Config);
        }

        private void formHome_Load(object sender, EventArgs e)
        {
            // Verifica auto start del player
            if (MOTF.Config.GlobalSettings.AutoStartPlayer)
            {
                LogInfo("Programma impostato per la riproduzione automatica");
                cboxAvvio.Checked = true;
                StartPlayer(true);
            }

            int numeroMonitor = 0;

            // Aggiungo tutti i monitor all'interno della ComboBox
            foreach (Screen screen in Screen.AllScreens)
            {
                // Se lo schermo è primario lo specifico nella cbox
                if (screen.Primary)
                {
                    cboxMonitor.Items.Add($"{numeroMonitor} - primario");
                    cboxMonitor.SelectedItem = $"{numeroMonitor} - primario";
                }
                else
                    cboxMonitor.Items.Add($"{numeroMonitor}");

                // Incremento il numero di monitor
                numeroMonitor++;
            }
        }

        private void formHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete(Config.LockFilePath);
            LogInfo("!! MOTF chiuso !!");
        }

        private void StartPlayer(bool automaticStart = false)
        {
            if (MOTF.Config.WMPSettings.PlayMode == WMPSettings.PLAY_MODE.FILE || MOTF.Config.WMPSettings.PlayMode == WMPSettings.PLAY_MODE.FOLDER)
            {
                if (string.IsNullOrEmpty(tboxPath.Text))
                {
                    MessageBox.Show("Seleziona una cartella o un file da riprodurre", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (!automaticStart)
            {
                MOTF.Config.WMPSettings.Monitor = cboxMonitor.SelectedIndex;
            }

            // Faccio un'ulteriore controllo sul monitor
            // e se esiste apro il formPlayer
            if (MOTF.Config.WMPSettings.Monitor > Screen.AllScreens.Length)
            {
                if (automaticStart)
                {
                    LogWarning($"Il monitor salvato nelle impostazioni [{MOTF.Config.WMPSettings.Monitor}] non esiste. Uso il monitor principale.");
                    MOTF.Config.WMPSettings.Monitor = 0;
                }
                else
                {
                    MessageBox.Show("Il monitor selezionato non esiste");
                }
            }
            else
            {
                // Apro il form player
                if (!automaticStart)
                    MessageBox.Show("Per uscire dal Media Player premere ESC", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MOTF.SaveSettings(Config.ConfigPath);
                new formPlayer(MOTF.Config.WMPSettings).ShowDialog();
            }
        }

        private void LoadFormSettings(Config config)
        {
            tboxPath.Text = config.WMPSettings.FilePath;
            tbarVolume.Value = config.WMPSettings.Volume;
        }

        private void cboxPlaylist_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxPlaylist.Checked)
            {
                cboxCartella.Enabled = false;
                btnSeleziona.Enabled = false;
                btnCartella.Enabled = false;
                btnPlaylist.Enabled = true;
                MOTF.Config.WMPSettings.PlayMode = WMPSettings.PLAY_MODE.PLAYLIST;
            }
            else
            {
                cboxCartella.Enabled = true;
                btnSeleziona.Enabled = true;
                btnPlaylist.Enabled = false;
                MOTF.Config.WMPSettings.PlayMode = WMPSettings.PLAY_MODE.FILE;
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
                MOTF.Config.WMPSettings.PlayMode = WMPSettings.PLAY_MODE.FOLDER;
            }
            else
            {
                cboxPlaylist.Enabled = true;
                btnSeleziona.Enabled = true;
                btnPlaylist.Enabled = true;
                MOTF.Config.WMPSettings.PlayMode = WMPSettings.PLAY_MODE.FILE;
            }
        }

        private void tbarVolume_Scroll(object sender, EventArgs e)
        {
            MOTF.Config.WMPSettings.Volume = tbarVolume.Value;
        }

        private void btnSeleziona_Click(object sender, EventArgs e)
        {
            // Apro un file dialog e consento all'utente di selezionare un file
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "File video|*.mp4";
            fileDialog.Title = "Seleziona file";

            // Se il DialogResult è OK allora imposto il filePath
            if (fileDialog.ShowDialog() == DialogResult.OK)
                MOTF.Config.WMPSettings.FilePath = fileDialog.FileName;

            // Creo la cartella tmp se non esiste
            if (!System.IO.Directory.Exists(Application.StartupPath + @"\tmp"))
                System.IO.Directory.CreateDirectory(Application.StartupPath + @"\tmp");

            // Imposto la path nella tbox
            tboxPath.Text = fileDialog.FileName;
        }

        private void btnChiudi_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAvvia_Click(object sender, EventArgs e)
        {
            StartPlayer();
        }

        private void btnPlaylist_Click(object sender, EventArgs e)
        {

        }

        private void btnCartella_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Seleziona cartella con i video da riprodurre";
            folderBrowserDialog.ShowNewFolderButton = true;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                MOTF.Config.WMPSettings.FilePath = folderBrowserDialog.SelectedPath;
                tboxPath.Text = folderBrowserDialog.SelectedPath;
            }
            else
                MessageBox.Show("Percorso non valido", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
