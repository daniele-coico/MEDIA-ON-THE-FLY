using System.Net;
using System.Net.Sockets;
using System.Runtime.Versioning;
using System.Text;

namespace MEDIA_ON_THE_FLY
{
    /// <summary>
    /// Lo StateObject può tornarci comodo perché in un solo oggetto
    /// possiamo contenere la socket e anche il buffer.
    /// Consigliata per quando si lavora con più socket (es: server).
    /// </summary>
    public class StateObject : Socket
    {
        public const string EndMessaggeLimiter = "%<EOF>%\0";
        public readonly string IPAddress = "127.0.0.1";

        public string username = "Default";
        public string versione = formHome.versione;

        // Buffer ricezione dati.
        public byte[] buffer = new byte[MessageData.MaxBufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        public string MessaggioDaInviare;

        public StateObject(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType)
        {
            MessaggioDaInviare = "";
        }

        public StateObject(SocketInformation socket) : base(socket)
        {
            MessaggioDaInviare = "";
            IPAddress = ((IPEndPoint)this.RemoteEndPoint).Address.ToString();
        }
    }
}
