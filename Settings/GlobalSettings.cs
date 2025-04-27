using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDIA_ON_THE_FLY.Settings
{
    public class GlobalSettings
    {
        public bool AutoStart { get; set; } = false;        // Avvio automatico del programma con Windows
        public bool AutoStartPlayer { get; set; } = false;  // Avvio automatico del player
        public int FileLockTimeout { get; set; } = 5000;    // Timeout in millisecondi per il lock del file
        public int SleepBeforeVideoRestart { get; set; } = 1000; // Tempo di attesa prima di riavviare il video
        public int MaxTryFileCopy { get; set; } = 3; // Numero massimo di tentativi per copiare il file
    }
}
