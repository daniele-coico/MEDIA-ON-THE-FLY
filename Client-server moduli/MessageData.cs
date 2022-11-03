using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDIA_ON_THE_FLY
{
    public static class MessageData
    {
        /*  COMPOSIZIONE MESSAGGIO
        Il pacchetto inviato/ricevuto dal server avrà dimensione 2048 byte.
        È quindi necessario definire i vari offset per poter trovare i dati necessari.
        Al momento è solo necessario definire la grandezza del messaggio, dove si trova
        il nome utente, e dove si trova l'EOF del messaggio.
        
        I primi 8 byte sono dedicati ai valori variabili del pacchetto, che sono quindi la dimensione
        del messaggio effettivo, e la posizione dell'EOF.
        Nei 64 byte successivi troviamo il nome utente.
        Il resto è il messaggio.
        */

        public const int MaxBufferSize = 2048;

        private const int OffsetMsgSize = 0x00;
        private const int OffsetEOF = 0x04;
        private const int OffsetUsername = 0x08;
        private const int OffsetMessage = 0x48;

        private static readonly byte[] EOFPosition = Encoding.UTF8.GetBytes(StateObject.EndMessaggeLimiter);

        public static byte[] GeneraBufferDaInviare(StateObject stateObject)
        {
            byte[] buffer = new byte[MaxBufferSize];

            // Dati da inserire nel buffer
            byte[] username = Encoding.UTF8.GetBytes(stateObject.username);             // Username
            byte[] messaggio = Encoding.UTF8.GetBytes(stateObject.MessaggioDaInviare);  // Messaggio
            byte[] EOF = Encoding.UTF8.GetBytes(StateObject.EndMessaggeLimiter);        // EOF
            byte[] EOFPosition = Encoding.UTF8.GetBytes((OffsetMessage + messaggio.Length).ToString()); // Posizione dell'EOF
            byte[] msgSize = Encoding.UTF8.GetBytes(messaggio.Length.ToString());       // Dimensione messaggio (primi 4 byte)

            // Controllo che il nome non sia eccessivamente lungo
            if (username.Length > OffsetMessage - OffsetUsername)
                username = Encoding.UTF8.GetBytes("NoNameUser");

            // Creo i "puntatori" del buffer che mi servono
            int ptrBuffer, ptrMessaggio = 0x0;

            // Scrivo la size del messaggio nel buffer
            for (ptrBuffer = OffsetMsgSize; ptrBuffer < msgSize.Length; ptrBuffer++)
                buffer[ptrBuffer] = msgSize[ptrBuffer];

            // Salvo la posizione dell'EOF:
            for (ptrBuffer = OffsetEOF, ptrMessaggio = 0; ptrBuffer < OffsetUsername && ptrMessaggio < EOFPosition.Length; ptrBuffer++, ptrMessaggio++)
                buffer[ptrBuffer] = EOFPosition[ptrMessaggio];

            // Scrivo l'username nel buffer
            for (ptrBuffer = OffsetUsername, ptrMessaggio = 0x0; ptrBuffer < OffsetUsername + username.Length && ptrBuffer < OffsetMessage; ptrBuffer++, ptrMessaggio++)
                buffer[ptrBuffer] = username[ptrMessaggio];

            // Completo il buffer con il messaggio (EOF non presente)
            for (ptrBuffer = OffsetMessage, ptrMessaggio = 0x0; ptrBuffer < OffsetMessage + messaggio.Length; ptrBuffer++, ptrMessaggio++)
                buffer[ptrBuffer] = messaggio[ptrMessaggio];

            // ptrBuffer è alla fine, bisogna quindi aggiungere l'EOF
            int tempPtrBufferPos = ptrBuffer;

            // Inserisco l'EOF nel buffer:
            for (ptrMessaggio = 0x0; ptrBuffer < tempPtrBufferPos + EOF.Length && ptrBuffer < MaxBufferSize; ptrBuffer++, ptrMessaggio++)
                buffer[ptrBuffer] = EOF[ptrMessaggio];

            // Creo il buffer togliendo gli spazi vuoti alla fine
            byte[] shortBuffer = new byte[ptrBuffer];
            for (int i = 0; i < shortBuffer.Length; i++)
                shortBuffer[i] = buffer[i];

            return shortBuffer;
        } 

        public static byte[] GeneraBufferDaInviare(string name, string message)
        {
            byte[] buffer = new byte[MaxBufferSize];

            // Dati da inserire nel buffer
            byte[] username = Encoding.UTF8.GetBytes(name);                             // Username
            byte[] messaggio = Encoding.UTF8.GetBytes(message);                         // Messaggio
            byte[] EOF = Encoding.UTF8.GetBytes(StateObject.EndMessaggeLimiter);        // EOF
            byte[] EOFPosition = Encoding.UTF8.GetBytes((OffsetMessage + messaggio.Length).ToString()); // Posizione dell'EOF
            byte[] msgSize = Encoding.UTF8.GetBytes(messaggio.Length.ToString());       // Dimensione messaggio (primi 4 byte)

            // Controllo che il nome non sia eccessivamente lungo
            if (username.Length > OffsetMessage - OffsetUsername)
                username = Encoding.UTF8.GetBytes("NoNameUser");

            // Creo i "puntatori" del buffer che mi servono
            int ptrBuffer, ptrMessaggio;

            // Scrivo la size del messaggio nel buffer
            for (ptrBuffer = OffsetMsgSize; ptrBuffer < msgSize.Length; ptrBuffer++)
                buffer[ptrBuffer] = msgSize[ptrBuffer];

            // Salvo la posizione dell'EOF:
            for (ptrBuffer = OffsetEOF, ptrMessaggio = 0x0; ptrBuffer < OffsetUsername && ptrMessaggio < EOFPosition.Length; ptrBuffer++, ptrMessaggio++)
                buffer[ptrBuffer] = EOFPosition[ptrMessaggio];

            // Scrivo l'username nel buffer
            for (ptrBuffer = OffsetUsername, ptrMessaggio = 0x0; ptrBuffer < OffsetUsername + username.Length && ptrBuffer < OffsetMessage; ptrBuffer++, ptrMessaggio++)
                buffer[ptrBuffer] = username[ptrMessaggio];

            // Completo il buffer con il messaggio (EOF non presente)
            for (ptrBuffer = OffsetMessage, ptrMessaggio = 0x0; ptrBuffer < OffsetMessage + messaggio.Length; ptrBuffer++, ptrMessaggio++)
                buffer[ptrBuffer] = messaggio[ptrMessaggio];

            // ptrBuffer è alla fine, bisogna quindi aggiungere l'EOF
            int tempPtrBufferPos = ptrBuffer;

            // Inserisco l'EOF nel buffer:
            for (ptrMessaggio = 0x0; ptrBuffer < tempPtrBufferPos + EOF.Length && ptrBuffer < MaxBufferSize; ptrBuffer++, ptrMessaggio++)
                buffer[ptrBuffer] = EOF[ptrMessaggio];

            // Creo il buffer togliendo gli spazi vuoti alla fine
            byte[] shortBuffer = new byte[ptrBuffer];
            for (int i = 0; i < shortBuffer.Length; i++)
                shortBuffer[i] = buffer[i];

            return shortBuffer;
        }

        public static string ConvertiBufferRiceuto(byte[] buffer, out string username)
        {
            int i = 0;

            /*** Prelevo la posizione dell'EOF ***/
            int EOFPosition; byte[] byteEOFPosition = new byte[OffsetUsername - OffsetEOF];

            for (int ptr = OffsetEOF; ptr < OffsetUsername; ptr++, i++)
            {
                if (IsByteZero(buffer[ptr]))
                    break;

                byteEOFPosition[i] = buffer[ptr];
            }

            i = 0;
            EOFPosition = Convert.ToInt32(Encoding.UTF8.GetString(byteEOFPosition));

            /*** Ottengo il nome ***/
            byte[] byteUsername = new byte[OffsetMessage - OffsetUsername];
            for (int ptr = OffsetUsername; ptr < OffsetMessage; ptr++, i++)
            {
                if (IsByteZero(buffer[ptr]))
                    break;

                byteUsername[i] = buffer[ptr];
            }

            i = 0;
            username = Encoding.UTF8.GetString(byteUsername).Trim('\0');

            /*** Ottengo il messaggio ***/
            int ptrBuffer = 0x0;
            byte[] byteMessaggio = new byte[EOFPosition - OffsetMessage];

            for (ptrBuffer = OffsetMessage; ptrBuffer < EOFPosition; ptrBuffer++, i++)
            {
                if (IsByteZero(buffer[ptrBuffer]))
                    break;

                byteMessaggio[i] = buffer[ptrBuffer];
            }

            i = 0;

            /*** Verifico che l'EOF corrisponda ***/
            byte[] byteEOF = new byte[StateObject.EndMessaggeLimiter.Length];

            while (IsByteZero(buffer[ptrBuffer]) == false)
            {
                byteEOF[i] = buffer[ptrBuffer];
                ptrBuffer++; i++;
            }

            if ((Encoding.UTF8.GetString(byteEOF) == StateObject.EndMessaggeLimiter) == false)
            {
                return "ERRORE EOF";
            }

            /*** Se tutto va bene ritorno il messaggio ***/
            return Encoding.UTF8.GetString(byteMessaggio);
        }

        private static bool IsByteZero(byte byteToCheck) => byteToCheck == 0x00 ? true : false;

        public static byte[] GeneraBufferDaInviare(string messaggio)
        {
            return Encoding.UTF8.GetBytes(messaggio + StateObject.EndMessaggeLimiter);
        }
    }
}
