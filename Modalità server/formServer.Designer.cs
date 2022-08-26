
namespace MEDIA_ON_THE_FLY
{
    partial class formServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formServer));
            this.listboxClient = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tboxIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tboxPorta = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tboxLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listboxClient
            // 
            this.listboxClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listboxClient.FormattingEnabled = true;
            this.listboxClient.ItemHeight = 16;
            this.listboxClient.Location = new System.Drawing.Point(18, 146);
            this.listboxClient.Name = "listboxClient";
            this.listboxClient.ScrollAlwaysVisible = true;
            this.listboxClient.Size = new System.Drawing.Size(630, 84);
            this.listboxClient.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Modalità server";
            // 
            // tboxIP
            // 
            this.tboxIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F);
            this.tboxIP.Location = new System.Drawing.Point(157, 51);
            this.tboxIP.Name = "tboxIP";
            this.tboxIP.ReadOnly = true;
            this.tboxIP.Size = new System.Drawing.Size(228, 28);
            this.tboxIP.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Client connessi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP SERVER";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(466, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "PORTA";
            // 
            // tboxPorta
            // 
            this.tboxPorta.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F);
            this.tboxPorta.Location = new System.Drawing.Point(547, 51);
            this.tboxPorta.Name = "tboxPorta";
            this.tboxPorta.ReadOnly = true;
            this.tboxPorta.Size = new System.Drawing.Size(101, 28);
            this.tboxPorta.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 24);
            this.label5.TabIndex = 8;
            this.label5.Text = "Log";
            // 
            // tboxLog
            // 
            this.tboxLog.BackColor = System.Drawing.Color.White;
            this.tboxLog.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxLog.Location = new System.Drawing.Point(18, 270);
            this.tboxLog.Multiline = true;
            this.tboxLog.Name = "tboxLog";
            this.tboxLog.ReadOnly = true;
            this.tboxLog.Size = new System.Drawing.Size(630, 417);
            this.tboxLog.TabIndex = 9;
            // 
            // formServer
            // 
            this.ClientSize = new System.Drawing.Size(669, 699);
            this.Controls.Add(this.tboxLog);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tboxPorta);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tboxIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listboxClient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MEDIA ON-THE-FLY";
            this.Load += new System.EventHandler(this.formServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listboxClient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tboxPorta;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tboxLog;
    }
}

