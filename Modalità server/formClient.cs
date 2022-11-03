using SetDocs;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    public partial class formClient : Form
    {
        private string launchedFromPath;
        private string serverIP = "127.0.0.1";

        // Bool utile a indicare se questa istanza è collegata ad un server
        public bool connectedToServer = false;

        // Socket per la connessione al server
        //public static Socket client;
        public static StateObject client;

        public formClient()
        {
            SetArgs();
            InitializeComponent();
        }

        public formClient(string launchedFromPath, string IP)
        {
            this.launchedFromPath = launchedFromPath;
            serverIP = IP;
            InitializeComponent();
        }

        private string[] SetArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            launchedFromPath = args[0];
            return args;
        }

        /// <summary>
        /// Metodo per ottenere l'IP locale del PC
        /// </summary>
        /// <returns></returns>
        private string GetLocalIP()
        {
            string localIP;
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                }
            }
            catch (SocketException)
            {
                localIP = "127.0.0.1";
            }

            return localIP;
        }

        private bool StartClient(string ServerIPAddress, int sleepBeforeConnectionMilliseconds = 0)
        {
            Thread receiveThread = new Thread(GestisciRicezione);

            // Controllo l'IP che mi è stato passato - creo eventualmente l'IP address corretto
            if (IPAddress.TryParse(ServerIPAddress, out IPAddress ipAddress) == false)
                return false;

            // Mi collego al client
            try
            {
                // Imposto l'EndPoint per connettermi al server
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 20000);

                // Imposto la socket e mi connetto all'EndPoint.
                client = new StateObject(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.username = Environment.MachineName;
                client.Connect(remoteEP);

                MOTF.Log($"Socket connesso a {client.IPAddress}");

                // Metto l'ascolto (ricezione dei messaggi) su un altro thread.
                receiveThread.Name = $"RX{DateTime.Today.Ticks}-{DateTime.Now.Millisecond}";
                receiveThread.Start();

                // Cambio il nome di questo thread (invio dei messaggi).
                Thread.CurrentThread.Name = $"TX{DateTime.Today.Ticks}-{DateTime.Now.Millisecond}";
                MOTF.Log($"Thread TX: {Thread.CurrentThread.Name} - Thread RX: {receiveThread.Name}");
            }
            catch (Exception)
            {
                if (receiveThread != null)
                    receiveThread.Abort();
                client.Dispose();

                MessageBox.Show("Non è possibile connettersi al server - verifica la rete", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                client.MessaggioDaInviare = formHome.versione.ToString();
                client.Send(MessageData.GeneraBufferDaInviare(client));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return true;
        }

        private void GestisciRicezione(object obj)
        {
            try
            {
                string rawResponse = string.Empty;

                while (client.Connected)
                {
                    // Il client sarà in ricezione.
                    // La ricezione mette in sospensione il thread.
                    int bytesRead = client.Receive(client.buffer);

                    if (bytesRead > 0)
                    {
                        string decodedResponse;

                        decodedResponse = MessageData.ConvertiBufferRiceuto(client.buffer, out string username);

                        client.buffer = new byte[MessageData.MaxBufferSize];

                        // Stampo il messaggio a schermo
                        MOTF.Log($"Server {username}: {decodedResponse}");

                        switch (decodedResponse)
                        {
                            // Se ricevo questo messaggio dal server allora vuol dire che è stato fatto un aggiornamento
                            case "RESTART:UPDATE":
                                // Se MOTF gira su disco condiviso, allora riavvio questa istanza per aggiornarla.
                                if (cboxAutoUpdate.Checked == true)
                                {
                                    MOTF.Log($"Il server deve essere aggiornato - è necessario riavviare anche questa istanza per completare l'aggiornamento");
                                }
                                else
                                {
                                    Show();

                                    MOTF.Log($"Il server ha effettuato un aggiornamento - riavvio applicazione...");
                                    lblStato.Text = "Aggiornamento server - riavvio applicazione...";
                                    lblStato.ForeColor = System.Drawing.Color.Orange;

                                    Refresh();
                                    Thread.Sleep(5000);

                                    // Se il server è stato aggiornato probabilmente c'è anche un update
                                    formHome.CheckUpdate();
                                }

                                System.Diagnostics.Process.Start(launchedFromPath, $"-connect {tboxIP.Text}");
                                Application.Exit();

                                break;
                            default:
                                break;
                        }
                    }

                    // Pulisco lo StringBuilder e il buffer.
                    client.sb.Clear();
                    client.buffer = new byte[MessageData.MaxBufferSize];
                }
            }
            catch (Exception e)
            {
                MOTF.Log($"{Thread.CurrentThread.Name} errore:\n{e.Message}");
            }
        }

        private void formClient_Load(object sender, EventArgs e)
        {
            if (MOTF.FileRootIsPhysicalDisk(launchedFromPath))
                cboxAutoUpdate.Enabled = false;

            if (MOTF.GetFileDriveType(launchedFromPath) == DriveType.Network)
                cboxAutoUpdate.Checked = true;

            // Se l'applicazione viene lanciata con l'argomento connect allora
            // l'operazione di collegamento al server verrà effettuata automaticamente.
            if (!string.IsNullOrEmpty(serverIP) && serverIP != "127.0.0.1")
            {
                if (StartClient(serverIP) == false)
                {
                    MessageBox.Show("MEDIA ON-THE-PLAYER è stato avviato con la richiesta di connettersi ad un server, ma quest'ultimo non è disponibile.\n" +
                        $"Verifica la tua connessione e prova collegarti manualmente a {serverIP}", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }
        }

        private void btnConnetti_Click(object sender, EventArgs e)
        {
            tboxIP.Enabled = false;
            cboxAutoUpdate.Enabled = false;
            cboxConnettiAuto.Enabled = true;
            btnDisconnetti.Enabled = true;
            btnConnetti.Enabled = false;
            connectedToServer = true;

            lblStato.Text = "Stato: connesso";
            lblStato.ForeColor = System.Drawing.Color.DarkGreen;

            if (StartClient(tboxIP.Text) == false)
            {
                MessageBox.Show("Non sono riuscito a connettermi al server, verifica la rete o l'indrizzo IP.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connectedToServer = false;
                Close();
                Dispose();
            }
        }

        private void formClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

            if (connectedToServer)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void cboxConnettiAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxConnettiAuto.Checked)
                new WriteSettings(MOTF.USER_FOLDER, "config").AddSetting("args[]", $"{launchedFromPath} -connect {tboxIP.Text}", "Argomenti per l'avvio di MOTF");
            else
                new WriteSettings(MOTF.USER_FOLDER, "config").RemoveSetting("args[]");
        }

        private void cboxAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnDisconnetti_Click(object sender, EventArgs e)
        {
            client.Close();
            connectedToServer = false;

            tboxIP.Enabled = true;
            cboxAutoUpdate.Enabled = true;
            cboxConnettiAuto.Enabled = false;
            btnConnetti.Enabled = true;

            lblStato.Text = "Stato: disconnesso";
            lblStato.ForeColor = System.Drawing.Color.DarkRed;
        }
    }
}