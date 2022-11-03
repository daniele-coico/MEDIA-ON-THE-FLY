using SetDocs;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using static System.Windows.Forms.AxHost;

namespace MEDIA_ON_THE_FLY
{
    public partial class formServer : Form
    {
        // Variabili
        private Thread threadServer;        // Thread dedicato alla modalità server
        private Thread threadPing;          // Thread dedicato al ping dei client
        private Thread threadUpdate;        // Thread dedicato al check degli update   

        private bool autostart = false;     // Bool per segnalare se il server è stato avviato automaticamente

        // Lista di StateObject che contengono le connessioni accettate
        public static List<StateObject> clientsConnectedToServer = new List<StateObject>();

        // Socket principale del server.  
        public static StateObject serverSocket = new StateObject(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static Socket tempClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public formServer()
        {
            InitializeComponent();

            // Imposto l'icona del programma
            Icon = Properties.Resources.img_641;
        }

        public formServer(bool autostartMode)
        {
            InitializeComponent();

            // Imposto l'icona del programma
            Icon = Properties.Resources.img_641;

            autostart = autostartMode;
        }

        /// <summary>
        /// Metodo per ottenere l'IP locale del PC
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
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

            if (Convert.ToInt32(serverVersion[0]) > Convert.ToInt32(clientVersion[0]))
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
                    if (Convert.ToInt32(serverVersion[2]) > Convert.ToInt32(clientVersion[2]))
                        return true;
                    else
                        return false;
                }
            }
        }

        /// <summary>
        /// Metodo dedicato ad accettare le connessioni in entrata.
        /// La nuova connessione, quando viene accettata, viene subito spostata su un thread a parte.
        /// </summary>
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

        /// <summary>
        /// Questo metodo aggiunge un client alla ListBox.
        /// </summary>
        /// <param name="state"></param>
        private void UpdateListboxClient(StateObject state)
        {
            if (IsClientVersionOutOfDate(state.versione))
                listboxClient.Items.Add($"{state.IPAddress} - {state.username} - versione {state.versione} è OUT OF DATE (doppio click per aggiornare)");
            else
                listboxClient.Items.Add($"{state.IPAddress} - {state.username} - versione {state.versione}");
        }

        /// <summary>
        /// Questo metodo gestisce una connessione appena accettata.
        /// I dati della connessione vengono impostati da qua.
        /// </summary>
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

            try
            {
                while (state != null && state.Connected)
                {
                    int bytesLetti = state.Receive(state.buffer);

                    if (bytesLetti > 0)
                    {
                        // Converto l'ultimo messaggio ricevuto
                        ultimoMessaggio = MessageData.ConvertiBufferRiceuto(state.buffer, out nomeUtente);
                        
                        // Salvo i dati importanti all'interno dell'oggetto
                        state.username = nomeUtente;
                        state.versione = ultimoMessaggio;

                        tboxLog.Text += MOTF.Log($"{Thread.CurrentThread.Name} - PC: {state.username} - MOTF versione: {state.versione}");
                        UpdateListboxClient(state);
                    }

                    // Pulisco il buffer
                    state.buffer = new byte[MessageData.MaxBufferSize];
                    ultimoMessaggio = string.Empty;
                }
            }
            catch (SocketException e)
            {
                tboxLog.Text += MOTF.Log($"Client {Thread.CurrentThread.Name} ({state.username}) si è disconnesso dal server");
                
                // Quando viene chiusa la connessione allora rimuoviamo l'oggetto dalla lista.
                clientsConnectedToServer.Remove(state);
                Thread.CurrentThread.Abort();
            }
            catch (ObjectDisposedException e)
            {
                tboxLog.Text += MOTF.Log($"Il client {Thread.CurrentThread.Name} è stato disconnesso ma non rimosso:\n{e.Message}");
                Thread.CurrentThread.Abort();
            }
            catch (Exception e)
            {
                tboxLog.Text += MOTF.Log($"ERRORE: Il client {Thread.CurrentThread.Name} è stato disconnesso forzatamente:\n{e.Message}");
                clientsConnectedToServer.Remove(state);
                Thread.CurrentThread.Abort();
            }
        }

        /// <summary>
        /// Questo metodo si occupa di verificare che le connessioni siano a posto.
        /// I tunnel con i client disconnessi vengono chiuso del tutto.
        /// </summary>
        private void ChiudiConnessioneClient()
        {
            List<StateObject> tempList = clientsConnectedToServer;  // ATTENZIONE: le liste funzionano come variabili di riferimento.

            foreach (StateObject stateObject in tempList)
                if (IsSocketConnectedAndOnline(stateObject) == false)
                {
                    tboxLog.Text += MOTF.Log($"Il client {stateObject.IPAddress} non è più online", '*');

                    stateObject.Close();
                    stateObject.Dispose();
                }
        }

        /// <summary>
        /// Metodo per l'invio di un messaggio a TUTTI i client connessi.
        /// </summary>
        /// <param name="data">Stringa di dati da inviare</param>
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

        /// <summary>
        /// Metodo per l'invio di un messaggio a TUTTI i client connessi.
        /// </summary>
        /// <param name="data">Byte di dati da inviare</param>
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

        /// <summary>
        /// Metodo dedicato al ping periodico delle connessioni.
        /// </summary>
        private void Ping()
        {
            while (true)
            {
                // Prima di fare un qualsiasi ping aspetto almeno 30 secondi.
                // Questo periodo di tempo vale anche tra un ping e l'altro.
                Thread.Sleep(30 * 1000);

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

                // Pulisco i dati della vecchia ListBox e l'aggiorno
                listboxClient.Items.Clear();

                foreach (StateObject state in clientsConnectedToServer)
                    UpdateListboxClient(state);
            }
        }

        /// <summary>
        /// Metodo per verificare se una socket è effettivamente connessa e se può rispondere ad eventuali messaggi
        /// </summary>
        /// <param name="stateObject"></param>
        /// <returns></returns>
        private bool IsSocketConnectedAndOnline(StateObject stateObject)
        {
            // https://stackoverflow.com/questions/2661764/how-to-check-if-a-socket-is-connected-disconnected-in-c

            if (stateObject != null)
            {
                bool isPolling = stateObject.Poll(1000, SelectMode.SelectRead);
                bool isNotAvaiable = (stateObject.Available == 0);

                // Se isPolling e isNotAvaible sono true, allora vuol dire che la connessione con la socket
                // non è più disponibile.
                if ((isPolling && isNotAvaiable) || stateObject.Connected == false)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
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
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }

                // Metto il thread in sleep per almeno 5 minuti
                Thread.Sleep(300 * 1000);
            }
        }

        /// <summary>
        /// Quando viene avviato il server vengono generati tutti i thread necessari.
        /// </summary>
        private void AvvioServer()
        {
            // Genero i thread necessari
            threadUpdate = new Thread(CheckUpdate);
            threadUpdate.Name = "Thread update";
            if (formHome.CHECK_UPDATE == true)
                threadUpdate.Start();

            threadServer = new Thread(StartListening);
            threadServer.Name = "Thread server";
            threadServer.Start();

            threadPing = new Thread(Ping);
            threadPing.Name = "Thread ping";
            threadPing.Start();

            // Imposto le componenti grafiche
            tboxIP.Text = GetLocalIP();
            tboxPorta.Text = "20000";
        }

        /// <summary>
        /// In caso di stop del server vengono chiuse tutte le connessioni in modo sicuro.
        /// </summary>
        private void StopServer()
        {
            tboxLog.Text += MOTF.Log("Disconnessione in corso...", '*');

            foreach (StateObject stateObject in clientsConnectedToServer)
            {
                stateObject.Disconnect(true);
                stateObject.Dispose();
                MOTF.Log($"Socket {stateObject.username} disconnesso", ' ', false);
            }

            tboxLog.Text += MOTF.Log("Disconnessione terminata", '!', false);
            clientsConnectedToServer.Clear();
        }

        private void formServer_Load(object sender, EventArgs e)
        {
            DialogResult rispostaUtente;

            // Verifico se la modalità server è stata avviata automaticamente
            if (autostart)
                AvvioServer();
            else
            {
                // Prima di procedere con la modalità server chiedo all'utente se vuole continuare.
                rispostaUtente = MessageBox.Show("Leggere: avviando la modalità server non sarà possibile usare le funzioni del player." +
                "Il file di log mostrerà tutto ciò che succede in questa modalità.\n" +
                "Se si chiude il server mentre ci sono dei client connessi, quest'ultimi continueranno a funzionare normalmente ma non saranno" +
                "in grado di ricevere eventuali update o file di configurazione da riprodurre.",
                "Modalità server",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);

                // In caso di risposta positiva allora continuerò normalmente, altrimenti chiudo la finestra.
                if (rispostaUtente == DialogResult.OK)
                    AvvioServer();
                else
                    Close();
            }

        }

        private void btnDisconnetti_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void cboxServer_CheckedChanged(object sender, EventArgs e)
        {
            string[] launchedFrom = Environment.GetCommandLineArgs();

            if (cboxServer.Checked)
                new WriteSettings(MOTF.USER_FOLDER, "config").AddSetting("args[]", $"{launchedFrom[0]} -server", "Argomenti per l'avvio di MOTF");
            else
                new WriteSettings(MOTF.USER_FOLDER, "config").RemoveSetting("args[]");
        }

        private void formServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }
    }
}