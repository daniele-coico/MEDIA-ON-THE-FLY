using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    public partial class formFileCheck : Form
    {
        public static int CheckStatus = 0;
        private readonly string _remoteFilePath;
        private readonly DriveType _lastDriveType;

        private bool _fakeLoadingScreen = false;

        /// <summary>
        /// Questo costruttore consente di richiamare questa finestra cambiando i testi dei label.
        /// Può anche essere utilizzata come finestra fantoccio per "dimostrare" che il programma sta girando (impostare fakeLoadingScreen su true).
        /// </summary>
        /// <param name="title">Titolo del label</param>
        /// <param name="text">Testo della finestra</param>
        /// <param name="fakeLoadingScreen">Quando è true, la finestra non eseguirà NESSUNA operazione</param>
        public formFileCheck(string title, string text, bool fakeLoadingScreen = true)
        {
            _fakeLoadingScreen = fakeLoadingScreen;
            lblCheck.Text = title;
            lblTesto.Text = text;
            InitializeComponent();
        }

        public formFileCheck(string filePath, DriveType driveType)
        {
            InitializeComponent();
            _remoteFilePath = filePath;
            _lastDriveType = driveType;
        }

        private void FileCheck()
        {
            Application.DoEvents();

            // Controllo null
            lblCheck.Text = "Verifico il valore di input";
            if (string.IsNullOrEmpty(_remoteFilePath))
                MOTF.Log("Valore di input null", '*');

            // Controllo completo del file con la classe MOTF
            lblCheck.Text = "Cerco il file nell'ultima posizione conosciuta";
            if (MOTF.FileExist(_remoteFilePath, out DriveType driveType) == false)
                MOTF.Log("Non riesco a trovare il file in nessun disco", '*');
            else
            {
                MOTF.Log("File trovato!", '*');
                CheckStatus = 1;
                return;
            }

            // Faccio un ulteriore controllo ma sul dispositivo utilizzato per
            // salvare il file (HDD, CD-ROM, USB etc.).
            lblCheck.Text = "Verifico l'ultimo dispositivo utilizzato";
            MOTF.Log("Non riesco a trovare il file in nessun disco", '*');


            // In questo caso è possibile che il file esista, ma il programma
            // deve ancora collegarsi al server.
            if (_lastDriveType == DriveType.Network || _lastDriveType == DriveType.Unknown)
            {
                // Se il file si trova in un server diamo tempo al PC di collegarsi
                // in rete per cercarlo. Vengono fatti 3 tentativi.
                int tentativo = 0;
                int secondsElapesed = 15;

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();   // Creo uno Stopwatch

                // Faccio 3 tentativi
                while (tentativo < 3)
                {
                    sw.Start(); // Avvio lo stopwatch

                    // Informo l'utente tra quanto avviene il nuovo tentativo
                    secondsElapesed -= (int)sw.ElapsedMilliseconds / 1000;
                    lblCheck.Text = $"Il file potrebbe trovarsi in rete. Faccio un nuovo tentativo ({tentativo+1}/3) tra {secondsElapesed} secondi";

                    // Appena arrivo a 0 faccio un nuovo tentativo
                    if (secondsElapesed <= 0)
                    {
                        // Il controllo del file lo faccio normalmente, d'altronde se ci
                        // colleghiamo in rete non dovremmo avere più problemi.
                        if (File.Exists(_remoteFilePath))
                        {
                            CheckStatus = 1;
                            MOTF.Log($"File trovato in rete al {tentativo+1}° tentativo.", '*');
                            lblCheck.Text = $"File trovato! Avvio il video...";
                            this.Update();
                            Application.DoEvents();
                            Thread.Sleep(2000);
                            Close();
                            return;
                        }

                        tentativo++;
                        sw.Reset();
                    }
                    secondsElapesed = 15;
                    this.Update();
                    Application.DoEvents();
                }

            }
            // Controllo il tipo del DriveType dell'ultima esecuzione.
            // Se i due DriveType sono diversi probabilmente il file si trovava in un altro posto.
            else if (_lastDriveType != driveType)
            {
                // Il dispositivo su cui è collegato il file potrebbe
                // non essere presente.
                lblCheck.Text = "Il dispositivo sulla quale si trovava il file prima è ora scollegato";
                Application.DoEvents();
                Thread.Sleep(2000);
                Close();
                return;
            }
            // È possibile che i DriveType corrispondano.
            // Questo vuol dire che magari il file esiste, ma è in un'altra posizione
            // Per esempio: la chiavetta ha un'altra lettera.
            // Potrebbe anche essere una coincidenza e magari il file non esiste
            else
            {
                // È possibile che il percorso faccia riferimento al disco sbagliato
                if (MOTF.FileRootIsPhysicalDisk(_remoteFilePath))
                {
                    lblCheck.Text = "La posizione del file è cambiata dall'ultima volta";
                    Application.DoEvents();
                    Thread.Sleep(2000);
                    Close();
                    return;
                }
            }

            Close();
            return;
        }

        private void formFileCheck_Load(object sender, EventArgs e)
        {
            if (_fakeLoadingScreen == false)
                BeginInvoke((MethodInvoker) FileCheck);
        }
    }
}
