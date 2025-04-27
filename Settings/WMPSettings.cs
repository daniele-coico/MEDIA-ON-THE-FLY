using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDIA_ON_THE_FLY.Settings
{
    public class WMPSettings
    {
        public PLAY_MODE PlayMode { get; set; } = PLAY_MODE.FILE; // Modalità con la quale l'utente riproduce il/i video
        public string FilePath { get; set; }                // Path del file da controllare
        public int Volume { get; set; } = 50;               // Volume per il WMP impostato dalla trackbar
        public int Monitor { get; set; } = 1;               // Monitor su cui riprodurre il video
        public Size DefaultFormSize { get; set; } = new Size(800, 600);     // Dimensione di default del form

        public enum PLAY_MODE
        {
            FILE,
            PLAYLIST,
            FOLDER
        }
    }
}
