using System;
using System.IO;
using System.Windows.Forms;

namespace MEDIA_ON_THE_FLY
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /* LEGGI:
            * Gestisco tutte le eccezioni del programma e salvo tutti gli errori nel file di log
            * In questo modo posso sapere cosa succede al programma in qualsiasi momento
            */
            AppDomain currentDomain = default;
            currentDomain = AppDomain.CurrentDomain;
            // Handler for unhandled exceptions.
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            // Handler for exceptions in threads behind forms.
            Application.ThreadException += GlobalThreadExceptionHandler;

            // Creo la cartella motf se non esiste
            if (!System.IO.Directory.Exists(MOTF.MOTF_FOLDER))
                System.IO.Directory.CreateDirectory(MOTF.MOTF_FOLDER);

            // Creo la cartella utente se non esiste
            if (!System.IO.Directory.Exists(MOTF.USER_FOLDER))
                System.IO.Directory.CreateDirectory(MOTF.USER_FOLDER);

            // Creo la cartella tmp se non esiste
            if (!System.IO.Directory.Exists(MOTF.TEMP_FOLDER))
                System.IO.Directory.CreateDirectory(MOTF.TEMP_FOLDER);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new formHome());
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            File.AppendAllLines(MOTF.USER_FOLDER + "\\log.txt", new[] { "!!! ERRORE INASPETTATO !!!\n" + ex.Message + "\n" + ex.StackTrace });
            File.AppendAllLines(MOTF.USER_FOLDER + "\\log.txt", new[] { "!!! CINGOLI EXPRESS CHIUSO !!! \n" });
            Application.Exit();
        }

        private static void GlobalThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            File.AppendAllLines(MOTF.USER_FOLDER + "\\log.txt", new[] { "!!! ERRORE INASPETTATO !!!\n" + ex.Message + "\n" + ex.StackTrace });
            File.AppendAllLines(MOTF.USER_FOLDER + "\\log.txt", new[] { "!!! CINGOLI EXPRESS CHIUSO !!! \n" });
            Application.Exit();
        }
    }
}