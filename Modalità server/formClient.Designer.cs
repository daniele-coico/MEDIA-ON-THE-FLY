
namespace MEDIA_ON_THE_FLY
{
    partial class formClient
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formClient));
            this.label1 = new System.Windows.Forms.Label();
            this.tboxIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConnetti = new System.Windows.Forms.Button();
            this.cboxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.lblAutoUpdate = new System.Windows.Forms.Label();
            this.lblStato = new System.Windows.Forms.Label();
            this.cboxConnettiAuto = new System.Windows.Forms.CheckBox();
            this.btnDisconnetti = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Connessione a server";
            // 
            // tboxIP
            // 
            this.tboxIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F);
            this.tboxIP.Location = new System.Drawing.Point(410, 52);
            this.tboxIP.Name = "tboxIP";
            this.tboxIP.Size = new System.Drawing.Size(244, 28);
            this.tboxIP.TabIndex = 3;
            this.tboxIP.Text = "000.000.000.000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Inserisci l\'IP del server";
            // 
            // btnConnetti
            // 
            this.btnConnetti.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnetti.Location = new System.Drawing.Point(24, 355);
            this.btnConnetti.Name = "btnConnetti";
            this.btnConnetti.Size = new System.Drawing.Size(288, 38);
            this.btnConnetti.TabIndex = 10;
            this.btnConnetti.Text = "Connetti";
            this.btnConnetti.UseVisualStyleBackColor = true;
            this.btnConnetti.Click += new System.EventHandler(this.btnConnetti_Click);
            // 
            // cboxAutoUpdate
            // 
            this.cboxAutoUpdate.AutoSize = true;
            this.cboxAutoUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F);
            this.cboxAutoUpdate.Location = new System.Drawing.Point(18, 129);
            this.cboxAutoUpdate.Name = "cboxAutoUpdate";
            this.cboxAutoUpdate.Size = new System.Drawing.Size(409, 28);
            this.cboxAutoUpdate.TabIndex = 11;
            this.cboxAutoUpdate.Text = "Consenti al server di gestire gli aggiornamenti";
            this.cboxAutoUpdate.UseVisualStyleBackColor = true;
            this.cboxAutoUpdate.CheckedChanged += new System.EventHandler(this.cboxAutoUpdate_CheckedChanged);
            // 
            // lblAutoUpdate
            // 
            this.lblAutoUpdate.AutoSize = true;
            this.lblAutoUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblAutoUpdate.Location = new System.Drawing.Point(13, 167);
            this.lblAutoUpdate.Name = "lblAutoUpdate";
            this.lblAutoUpdate.Size = new System.Drawing.Size(496, 72);
            this.lblAutoUpdate.TabIndex = 12;
            this.lblAutoUpdate.Text = "Se hai avviato MEDIA ON-THE-FLY da un disco condiviso\r\nè consigliato abilitare qu" +
    "esta opzione. Disattivala solo se\r\nMOTF è installato localmente.";
            // 
            // lblStato
            // 
            this.lblStato.AutoSize = true;
            this.lblStato.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStato.ForeColor = System.Drawing.Color.Maroon;
            this.lblStato.Location = new System.Drawing.Point(19, 323);
            this.lblStato.Name = "lblStato";
            this.lblStato.Size = new System.Drawing.Size(182, 24);
            this.lblStato.TabIndex = 13;
            this.lblStato.Text = "Stato: non connesso";
            // 
            // cboxConnettiAuto
            // 
            this.cboxConnettiAuto.AutoSize = true;
            this.cboxConnettiAuto.Enabled = false;
            this.cboxConnettiAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxConnettiAuto.Location = new System.Drawing.Point(24, 399);
            this.cboxConnettiAuto.Name = "cboxConnettiAuto";
            this.cboxConnettiAuto.Size = new System.Drawing.Size(266, 22);
            this.cboxConnettiAuto.TabIndex = 14;
            this.cboxConnettiAuto.Text = "Connettiti al server all\'avvio di MOTF";
            this.cboxConnettiAuto.UseVisualStyleBackColor = true;
            this.cboxConnettiAuto.CheckedChanged += new System.EventHandler(this.cboxConnettiAuto_CheckedChanged);
            // 
            // btnDisconnetti
            // 
            this.btnDisconnetti.Enabled = false;
            this.btnDisconnetti.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnetti.Location = new System.Drawing.Point(366, 355);
            this.btnDisconnetti.Name = "btnDisconnetti";
            this.btnDisconnetti.Size = new System.Drawing.Size(288, 38);
            this.btnDisconnetti.TabIndex = 15;
            this.btnDisconnetti.Text = "Disconnetti";
            this.btnDisconnetti.UseVisualStyleBackColor = true;
            this.btnDisconnetti.Click += new System.EventHandler(this.btnDisconnetti_Click);
            // 
            // formClient
            // 
            this.ClientSize = new System.Drawing.Size(670, 441);
            this.Controls.Add(this.btnDisconnetti);
            this.Controls.Add(this.cboxConnettiAuto);
            this.Controls.Add(this.lblStato);
            this.Controls.Add(this.lblAutoUpdate);
            this.Controls.Add(this.cboxAutoUpdate);
            this.Controls.Add(this.btnConnetti);
            this.Controls.Add(this.tboxIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formClient";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MEDIA ON-THE-FLY";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClient_FormClosing);
            this.Load += new System.EventHandler(this.formClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnConnetti;
        private System.Windows.Forms.CheckBox cboxAutoUpdate;
        private System.Windows.Forms.Label lblAutoUpdate;
        private System.Windows.Forms.Label lblStato;
        private System.Windows.Forms.CheckBox cboxConnettiAuto;
        private System.Windows.Forms.Button btnDisconnetti;
    }
}

