using SetDocs;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    public partial class formServer : Form
    {
        // Variabili
        private Thread threadServer;        // Thread dedicato alla modalità server
        private Thread threadPing;          // Thread dedicato al ping dei client
        private Thread threadUpdate;        // Thread dedicato al check degli update   

        // Lista di StateObject che contengono le connessioni accettate
        public static ArrayList clientsConnectedToServer = new ArrayList();

        // Socket principale del server.  
        public static StateObject serverSocket = new StateObject(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static Socket tempClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public formServer()
        {
            InitializeComponent();

            // Imposto l'icona del programma
            Icon = Properties.Resources.img_641;
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

        public static bool IsClientVersionOutOfDate(string version)
        {
            if (formHome.versione == version)
                return false;

            string[] serverVersion = formHome.versione.Split('.');
            string[] clientVersion = version.Split('.');

            if (Convert.ToInt32(serverVersion[2]) > Convert.ToInt32(clientVersion[2]))
            {
                return true;
            }
            else
            {
                if (Convert.ToInt32(serverVersion[1]) > Convert.ToInt32(clientVersion[1]))
                {
                    return true;
                }
                else
                {
                    if (Convert.ToInt32(serverVersion[0]) > Convert.ToInt32(clientVersion[0]))
                        return true;
                    else
                        return false;
                }
            }
        }

        private void StartListening()
        {
            // Creo un EndPoint che contiene IP e porta del server
            IPAddress ipAddress = IPAddress.Parse(GetLocalIP());
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 20000);

            try
            {
                // Faccio il bind della socket alla scheda di rete
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                tboxLog.Text = MOTF.Log("!!! Server avviato !!!", ' ');

                // Il server deve essere sempre attivo
                while (true)
                {
                    // Appena un client prova a connettersi accetto la connessione
                    // e la metto sulla socket temporanea.
                    tempClientSocket = serverSocket.Accept();

                    // Creo un nuovo thread per il client appena connesso.
                    Thread newClientThread = new Thread(GestisciConnessione)
                    {
                        Name = $"{DateTime.Now.Ticks}-{DateTime.Now.Millisecond}"
                    };
                    newClientThread.Start();
                }
            }
            catch (Exception e)
            {
                tboxLog.Text += MOTF.Log(e.ToString());
            }
        }

        private void GestisciConnessione()
        {
            string ultimoMessaggio, nomeUtente = String.Empty;

            // Copio le informazioni della socket temporanea
            // e creo uno StateObject con queste informazioni
            SocketInformation socketInformation = tempClientSocket.DuplicateAndClose(System.Diagnostics.Process.GetCurrentProcess().Id);

            // Copia la socket temporanea nello StateObject
            StateObject state = new StateObject(socketInformation);

            // Aggiungo alla lista dei client quello appena connesso
            clientsConnectedToServer.Add(state);

            // Loggo e aggiungo alla lista presente nel form
            tboxLog.Text += MOTF.Log($"Nuova connessione stabilita - {clientsConnectedToServer.Count} client connessi");
            tboxLog.Text += MOTF.Log($" * IP: {state.IPAddress} - La socket di questo client gira sul thread {Thread.CurrentThread.Name}\n *", ' ', false);

            // Pulisco la socket temporanea
            tempClientSocket.Dispose();

            // Check dei client per verificare che siano ancora connessi
            ChiudiConnessioneClient();

            try
            {
                while (state.Connected)
                {
                    int bytesLetti = state.Receive(state.buffer);

                    if (bytesLetti > 0)
                    {
                        ultimoMessaggio = MessageData.ConvertiBufferRiceuto(state.buffer, out nomeUtente);
                        tboxLog.Text += MOTF.Log($"{Thread.CurrentThread.Name} - PC: {nomeUtente} - MOTF versione: {ultimoMessaggio}");

                        if (IsClientVersionOutOfDate(ultimoMessaggio))
                            listboxClient.Items.Add($"{state.IPAddress} - {nomeUtente} - versione {ultimoMessaggio} OUT OF DATE (doppio click per aggiornare)");
                        else
                            listboxClient.Items.Add($"{state.IPAddress} - {nomeUtente} - versione {ultimoMessaggio}");
                    }

                    // Pulisco il buffer
                    state.buffer = new byte[MessageData.MaxBufferSize];
                    ultimoMessaggio = string.Empty;
                }
            }
            catch (SocketException e)
            {
                tboxLog.Text += MOTF.Log($"Client {Thread.CurrentThread.Name} ({nomeUtente}) errore:\n{e.Message}");

                // Quando viene chiusa la connessione allora rimuoviamo l'oggetto dalla lista.
                clientsConnectedToServer.Remove(state);
            }
        }

        private void ChiudiConnessioneClient()
        {
            foreach (StateObject stateObject in clientsConnectedToServer)
                if (IsSocketConnectedAndOnline(stateObject) == false)
                {
                    clientsConnectedToServer.Remove(stateObject);
                    listboxClient.Items.Remove(stateObject.IPAddress);
                    tboxLog.Text += MOTF.Log($"Il client {stateObject.IPAddress} non è più online", '*');

                    stateObject.Close();
                    stateObject.Dispose();
                }
        }

        private void Send(string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Invio il messaggio appena ricevuto a tutti i client
            tboxLog.Text += MOTF.Log($"Invio il messaggio a {clientsConnectedToServer.Count} client");
            foreach (StateObject client in clientsConnectedToServer)
            {
                // Richiamo la callback per l'invio del messaggio
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
        }

        private void Send(byte[] data)
        {
            // Invio il messaggio appena ricevuto a tutti i client
            tboxLog.Text += MOTF.Log($"* Invio il messaggio a {clientsConnectedToServer.Count} client *");
            foreach (StateObject client in clientsConnectedToServer)
            {
                // Richiamo la callback per l'invio del messaggio
                client.BeginSend(data, 0, data.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                if (ar != null)
                {
                    // Prendo l'oggetto dall'AsyncResult
                    Socket handler = (Socket)ar.AsyncState;

                    // Invio i dati.  
                    handler.EndSend(ar);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Ping()
        {
            // Prima di fare un qualsiasi ping aspetto almeno 3 minuti.
            // Questo periodo di tempo vale anche tra un ping e l'altro.
            Thread.Sleep(180 * 1000);

            int nPing = 0, nError = 0;
            tboxLog.Text += MOTF.Log($"* PING IN CORSO - {clientsConnectedToServer.Count} CLIENT *");

            if (clientsConnectedToServer.Count > 0)
            {
                foreach (StateObject item in clientsConnectedToServer)
                {
                    if (IsSocketConnectedAndOnline(item) == false)
                    {
                        tboxLog.Text += MOTF.Log($"ERRORE CON CLIENT {item.IPAddress}", '!');
                        nError++;
                    }
                    else
                        nPing++;
                }

                tboxLog.Text += MOTF.Log($"Client OK: {nPing} - Errore di ping: {nError}");
            }
            else
                tboxLog.Text += MOTF.Log("Nessun client connesso");

            ChiudiConnessioneClient();

        }

        /// <summary>
        /// Metodo per verificare se una socket è effettivamente connessa e se può rispondere ad eventuali messaggi
        /// </summary>
        /// <param name="stateObject"></param>
        /// <returns></returns>
        private bool IsSocketConnectedAndOnline(StateObject stateObject)
        {
            // https://stackoverflow.com/questions/2661764/how-to-check-if-a-socket-is-connected-disconnected-in-c

            bool isPolling = stateObject.Poll(1000, SelectMode.SelectRead);
            bool isNotAvaiable = (stateObject.Available == 0);

            // Se isPolling e isNotAvaible sono true, allora vuol dire che la connessione con la socket
            // non è più disponibile.
            if ((isPolling && isNotAvaiable) || stateObject.Connected == false)
                return false;
            else
                return true;
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
                if (formHome.versione != versioneOnline)
                {
                    // Dico a tutti i client di effettuare un riavvio
                    // Prima di riconnettersi ci sarà un'attessa di 60 secondi (per dare tempo all'update del server)
                    Send("RESTART:UPDATE");

                    // Scarico l'installer e lo salvo
                    webClient.DownloadFile("https://raw.githubusercontent.com/daniele-coico/MEDIA-ON-THE-FLY/master/update/Installer.exe", Application.StartupPath + @".\Installer.exe");

                    // Avvio l'installer con argomento True per avviare
                    // l'aggiornamento automatico.
                    System.Diagnostics.Process.Start(@".\Installer.exe", $"{true.ToString()} -server");

                    Application.Exit();
                }

                // Metto il thread in sleep per almeno 5 minuti
                Thread.Sleep(300 * 1000);
            }
        }

        private void formServer_Load(object sender, EventArgs e)
        {
            // Prima di procedere con la modalità server chiedo all'utente se vuole continuare.
            DialogResult rispostaUtente = MessageBox.Show("Leggere: avviando la modalità server non sarà possibile usare le funzioni del player." +
                "Il file di log mostrerà tutto ciò che succede in questa modalità.\n" +
                "Se si chiude il server mentre ci sono dei client connessi, quest'ultimi continueranno a funzionare normalmente ma non saranno" +
                "in grado di ricevere eventuali update o file di configurazione da riprodurre.",
                "Modalità server",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);

            // In caso di risposta positiva allora continuerò normalmente, altrimenti chiudo la finestra.
            if (rispostaUtente == DialogResult.OK)
            {
                // Genero i thread necessari
                threadUpdate = new Thread(CheckUpdate);
                threadUpdate.Name = "Thread update";
                if (formHome.DEBUG == false)
                    threadUpdate.Start();

                // Imposto le componenti grafiche
                tboxIP.Text = GetLocalIP();
                tboxPorta.Text = "20000";

                threadServer = new Thread(StartListening);
                threadServer.Name = "Thread server";
                threadServer.Start();

                threadPing = new Thread(Ping);
                threadPing.Name = "Thread ping";
                threadPing.Start();
            }
            else
            {
                Close();
            }
        }
    }
}