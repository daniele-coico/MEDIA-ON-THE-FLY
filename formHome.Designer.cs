
namespace MEDIA_ON_THE_FLY
{
    partial class formHome
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
            this.btnSeleziona = new System.Windows.Forms.Button();
            this.tboxPath = new System.Windows.Forms.TextBox();
            this.lblTesto = new System.Windows.Forms.Label();
            this.btnAvvia = new System.Windows.Forms.Button();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCrediti = new System.Windows.Forms.Label();
            this.lblAvanzate = new System.Windows.Forms.Label();
            this.cboxPlaylist = new System.Windows.Forms.CheckBox();
            this.btnPlaylist = new System.Windows.Forms.Button();
            this.cboxAvvio = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboxCartella = new System.Windows.Forms.CheckBox();
            this.btnCartella = new System.Windows.Forms.Button();
            this.tbarVolume = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.cboxCopiaSempre = new System.Windows.Forms.CheckBox();
            this.tboxLog = new System.Windows.Forms.TextBox();
            this.btnServer = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnConnettiAServer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbarVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSeleziona
            // 
            this.btnSeleziona.Location = new System.Drawing.Point(520, 49);
            this.btnSeleziona.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSeleziona.Name = "btnSeleziona";
            this.btnSeleziona.Size = new System.Drawing.Size(147, 27);
            this.btnSeleziona.TabIndex = 0;
            this.btnSeleziona.Text = "Seleziona";
            this.btnSeleziona.UseVisualStyleBackColor = true;
            this.btnSeleziona.Click += new System.EventHandler(this.btnSeleziona_Click);
            // 
            // tboxPath
            // 
            this.tboxPath.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tboxPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxPath.Location = new System.Drawing.Point(20, 48);
            this.tboxPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tboxPath.Name = "tboxPath";
            this.tboxPath.ReadOnly = true;
            this.tboxPath.Size = new System.Drawing.Size(493, 27);
            this.tboxPath.TabIndex = 1;
            // 
            // lblTesto
            // 
            this.lblTesto.AutoSize = true;
            this.lblTesto.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTesto.Location = new System.Drawing.Point(13, 14);
            this.lblTesto.Name = "lblTesto";
            this.lblTesto.Size = new System.Drawing.Size(571, 31);
            this.lblTesto.TabIndex = 2;
            this.lblTesto.Text = "Seleziona il file da riprodurre e controllare nel tempo.";
            // 
            // btnAvvia
            // 
            this.btnAvvia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvvia.Location = new System.Drawing.Point(352, 702);
            this.btnAvvia.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAvvia.Name = "btnAvvia";
            this.btnAvvia.Size = new System.Drawing.Size(315, 47);
            this.btnAvvia.TabIndex = 3;
            this.btnAvvia.Text = "AVVIA";
            this.btnAvvia.UseVisualStyleBackColor = true;
            this.btnAvvia.Click += new System.EventHandler(this.btnAvvia_Click);
            // 
            // btnChiudi
            // 
            this.btnChiudi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChiudi.Location = new System.Drawing.Point(20, 702);
            this.btnChiudi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(315, 47);
            this.btnChiudi.TabIndex = 4;
            this.btnChiudi.Text = "CHIUDI";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(20, 121);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(235, 28);
            this.comboBox1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 31);
            this.label1.TabIndex = 6;
            this.label1.Text = "Seleziona il monitor";
            // 
            // lblCrediti
            // 
            this.lblCrediti.AutoSize = true;
            this.lblCrediti.Enabled = false;
            this.lblCrediti.Location = new System.Drawing.Point(17, 750);
            this.lblCrediti.Name = "lblCrediti";
            this.lblCrediti.Size = new System.Drawing.Size(178, 16);
            this.lblCrediti.TabIndex = 7;
            this.lblCrediti.Text = "Sviluppato da Coico Daniele";
            // 
            // lblAvanzate
            // 
            this.lblAvanzate.AutoSize = true;
            this.lblAvanzate.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvanzate.Location = new System.Drawing.Point(13, 182);
            this.lblAvanzate.Name = "lblAvanzate";
            this.lblAvanzate.Size = new System.Drawing.Size(247, 31);
            this.lblAvanzate.TabIndex = 8;
            this.lblAvanzate.Text = "Impostazioni avanzate";
            this.lblAvanzate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboxPlaylist
            // 
            this.cboxPlaylist.AutoSize = true;
            this.cboxPlaylist.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxPlaylist.Location = new System.Drawing.Point(19, 220);
            this.cboxPlaylist.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboxPlaylist.Name = "cboxPlaylist";
            this.cboxPlaylist.Size = new System.Drawing.Size(176, 24);
            this.cboxPlaylist.TabIndex = 9;
            this.cboxPlaylist.Text = "Utilizza una playlist";
            this.cboxPlaylist.UseVisualStyleBackColor = true;
            this.cboxPlaylist.CheckedChanged += new System.EventHandler(this.cboxPlaylist_CheckedChanged);
            // 
            // btnPlaylist
            // 
            this.btnPlaylist.Enabled = false;
            this.btnPlaylist.Location = new System.Drawing.Point(520, 218);
            this.btnPlaylist.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPlaylist.Name = "btnPlaylist";
            this.btnPlaylist.Size = new System.Drawing.Size(147, 27);
            this.btnPlaylist.TabIndex = 10;
            this.btnPlaylist.Text = "Crea";
            this.btnPlaylist.UseVisualStyleBackColor = true;
            this.btnPlaylist.Click += new System.EventHandler(this.btnPlaylist_Click);
            // 
            // cboxAvvio
            // 
            this.cboxAvvio.AutoSize = true;
            this.cboxAvvio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxAvvio.Location = new System.Drawing.Point(19, 258);
            this.cboxAvvio.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboxAvvio.Name = "cboxAvvio";
            this.cboxAvvio.Size = new System.Drawing.Size(340, 24);
            this.cboxAvvio.TabIndex = 11;
            this.cboxAvvio.Text = "Avvia MEDIA ON-THE-FLY con Windows";
            this.cboxAvvio.UseVisualStyleBackColor = true;
            this.cboxAvvio.CheckedChanged += new System.EventHandler(this.cboxAvvio_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 286);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(330, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Verrà eseguito l\'ultimo set di impostazioni";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboxCartella
            // 
            this.cboxCartella.AutoSize = true;
            this.cboxCartella.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxCartella.Location = new System.Drawing.Point(19, 325);
            this.cboxCartella.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboxCartella.Name = "cboxCartella";
            this.cboxCartella.Size = new System.Drawing.Size(303, 24);
            this.cboxCartella.TabIndex = 13;
            this.cboxCartella.Text = "Riproduci tutti i video da una cartella";
            this.cboxCartella.UseVisualStyleBackColor = true;
            this.cboxCartella.CheckedChanged += new System.EventHandler(this.cboxCartella_CheckedChanged);
            // 
            // btnCartella
            // 
            this.btnCartella.Enabled = false;
            this.btnCartella.Location = new System.Drawing.Point(520, 324);
            this.btnCartella.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCartella.Name = "btnCartella";
            this.btnCartella.Size = new System.Drawing.Size(147, 27);
            this.btnCartella.TabIndex = 14;
            this.btnCartella.Text = "Seleziona la cartella";
            this.btnCartella.UseVisualStyleBackColor = true;
            this.btnCartella.Click += new System.EventHandler(this.btnCartella_Click);
            // 
            // tbarVolume
            // 
            this.tbarVolume.Location = new System.Drawing.Point(275, 112);
            this.tbarVolume.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbarVolume.Maximum = 100;
            this.tbarVolume.Name = "tbarVolume";
            this.tbarVolume.Size = new System.Drawing.Size(392, 56);
            this.tbarVolume.TabIndex = 15;
            this.tbarVolume.TickFrequency = 5;
            this.tbarVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbarVolume.Scroll += new System.EventHandler(this.tbarVolume_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(268, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 31);
            this.label3.TabIndex = 16;
            this.label3.Text = "Volume";
            // 
            // cboxCopiaSempre
            // 
            this.cboxCopiaSempre.AutoSize = true;
            this.cboxCopiaSempre.Enabled = false;
            this.cboxCopiaSempre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxCopiaSempre.Location = new System.Drawing.Point(19, 364);
            this.cboxCopiaSempre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboxCopiaSempre.Name = "cboxCopiaSempre";
            this.cboxCopiaSempre.Size = new System.Drawing.Size(343, 24);
            this.cboxCopiaSempre.TabIndex = 17;
            this.cboxCopiaSempre.Text = "Copia sempre il file da riprodurre in locale";
            this.cboxCopiaSempre.UseVisualStyleBackColor = true;
            // 
            // tboxLog
            // 
            this.tboxLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tboxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tboxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxLog.ForeColor = System.Drawing.Color.Silver;
            this.tboxLog.Location = new System.Drawing.Point(0, 783);
            this.tboxLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tboxLog.Name = "tboxLog";
            this.tboxLog.ReadOnly = true;
            this.tboxLog.Size = new System.Drawing.Size(681, 15);
            this.tboxLog.TabIndex = 18;
            this.tboxLog.Text = "Testo";
            this.tboxLog.DoubleClick += new System.EventHandler(this.tboxLog_DoubleClick);
            // 
            // btnServer
            // 
            this.btnServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServer.Location = new System.Drawing.Point(20, 561);
            this.btnServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(315, 47);
            this.btnServer.TabIndex = 19;
            this.btnServer.Text = "Avvia modalità server";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 453);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 31);
            this.label4.TabIndex = 20;
            this.label4.Text = "Modalità server";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.label5.Location = new System.Drawing.Point(16, 484);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(503, 60);
            this.label5.TabIndex = 21;
            this.label5.Text = "Con la modalità server, tutte le istanze che si connettono potranno\r\nessere tenut" +
    "e sotto controllo. Le istanze avviate da un\'eseguibile\r\ncondiviso verranno mante" +
    "nute aggiornate in modo sicuro.";
            // 
            // btnConnettiAServer
            // 
            this.btnConnettiAServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnettiAServer.Location = new System.Drawing.Point(352, 561);
            this.btnConnettiAServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnConnettiAServer.Name = "btnConnettiAServer";
            this.btnConnettiAServer.Size = new System.Drawing.Size(315, 47);
            this.btnConnettiAServer.TabIndex = 22;
            this.btnConnettiAServer.Text = "Connettiti ad un server";
            this.btnConnettiAServer.UseVisualStyleBackColor = true;
            this.btnConnettiAServer.Click += new System.EventHandler(this.btnConnettiAServer_Click);
            // 
            // formHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 798);
            this.Controls.Add(this.btnConnettiAServer);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.tboxLog);
            this.Controls.Add(this.cboxCopiaSempre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbarVolume);
            this.Controls.Add(this.btnCartella);
            this.Controls.Add(this.cboxCartella);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboxAvvio);
            this.Controls.Add(this.btnPlaylist);
            this.Controls.Add(this.cboxPlaylist);
            this.Controls.Add(this.lblAvanzate);
            this.Controls.Add(this.lblCrediti);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.btnAvvia);
            this.Controls.Add(this.lblTesto);
            this.Controls.Add(this.tboxPath);
            this.Controls.Add(this.btnSeleziona);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "formHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MEDIA ON-THE-FLY";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formHome_FormClosed);
            this.Load += new System.EventHandler(this.formHome_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbarVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSeleziona;
        private System.Windows.Forms.TextBox tboxPath;
        private System.Windows.Forms.Label lblTesto;
        private System.Windows.Forms.Button btnAvvia;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCrediti;
        private System.Windows.Forms.Label lblAvanzate;
        private System.Windows.Forms.CheckBox cboxPlaylist;
        private System.Windows.Forms.Button btnPlaylist;
        private System.Windows.Forms.CheckBox cboxAvvio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cboxCartella;
        private System.Windows.Forms.Button btnCartella;
        private System.Windows.Forms.TrackBar tbarVolume;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cboxCopiaSempre;
        private System.Windows.Forms.TextBox tboxLog;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnConnettiAServer;
    }
}

