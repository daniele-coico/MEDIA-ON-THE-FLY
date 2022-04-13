namespace MEDIA_ON_THE_FLY
{
    partial class formFileCheck
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formFileCheck));
            this.lblTesto = new System.Windows.Forms.Label();
            this.pbarCheck = new System.Windows.Forms.ProgressBar();
            this.lblCheck = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTesto
            // 
            this.lblTesto.AutoSize = true;
            this.lblTesto.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTesto.Location = new System.Drawing.Point(13, 14);
            this.lblTesto.Name = "lblTesto";
            this.lblTesto.Size = new System.Drawing.Size(444, 31);
            this.lblTesto.TabIndex = 3;
            this.lblTesto.Text = "Sto verificando l\'esistenza del file, attendi";
            // 
            // pbarCheck
            // 
            this.pbarCheck.Location = new System.Drawing.Point(12, 101);
            this.pbarCheck.Name = "pbarCheck";
            this.pbarCheck.Size = new System.Drawing.Size(857, 34);
            this.pbarCheck.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbarCheck.TabIndex = 4;
            // 
            // lblCheck
            // 
            this.lblCheck.AutoSize = true;
            this.lblCheck.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheck.Location = new System.Drawing.Point(17, 50);
            this.lblCheck.Name = "lblCheck";
            this.lblCheck.Size = new System.Drawing.Size(170, 23);
            this.lblCheck.TabIndex = 5;
            this.lblCheck.Text = "Verifica esistenza file.";
            // 
            // formFileCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 153);
            this.Controls.Add(this.lblCheck);
            this.Controls.Add(this.pbarCheck);
            this.Controls.Add(this.lblTesto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formFileCheck";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Verifica file";
            this.Load += new System.EventHandler(this.formFileCheck_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTesto;
        private System.Windows.Forms.ProgressBar pbarCheck;
        private System.Windows.Forms.Label lblCheck;
    }
}