
namespace MEDIA_ON_THE_FLY
{
    partial class formPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formPlayer));
            this.wmpMedia = new AxWMPLib.AxWindowsMediaPlayer();
            this.fswVideo = new System.IO.FileSystemWatcher();
            this.lblStatoVideo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.wmpMedia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fswVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // wmpMedia
            // 
            this.wmpMedia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wmpMedia.Enabled = true;
            this.wmpMedia.Location = new System.Drawing.Point(0, 0);
            this.wmpMedia.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.wmpMedia.Name = "wmpMedia";
            this.wmpMedia.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("wmpMedia.OcxState")));
            this.wmpMedia.Size = new System.Drawing.Size(800, 600);
            this.wmpMedia.TabIndex = 0;
            this.wmpMedia.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.wmpMedia_PlayStateChange);
            this.wmpMedia.KeyDownEvent += new AxWMPLib._WMPOCXEvents_KeyDownEventHandler(this.wmpMedia_KeyDownEvent);
            // 
            // fswVideo
            // 
            this.fswVideo.EnableRaisingEvents = true;
            this.fswVideo.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.fswVideo.SynchronizingObject = this;
            this.fswVideo.Changed += new System.IO.FileSystemEventHandler(this.fswVideo_Changed);
            // 
            // lblStatoVideo
            // 
            this.lblStatoVideo.AutoSize = true;
            this.lblStatoVideo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblStatoVideo.Font = new System.Drawing.Font("Consolas", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatoVideo.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblStatoVideo.Location = new System.Drawing.Point(10, 11);
            this.lblStatoVideo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatoVideo.Name = "lblStatoVideo";
            this.lblStatoVideo.Size = new System.Drawing.Size(545, 45);
            this.lblStatoVideo.TabIndex = 1;
            this.lblStatoVideo.Text = "Sto cambiando il video...";
            this.lblStatoVideo.Visible = false;
            // 
            // formPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.lblStatoVideo);
            this.Controls.Add(this.wmpMedia);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "formPlayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Media Player";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formPlayer_FormClosed);
            this.Load += new System.EventHandler(this.formPlayer_Load);
            this.SizeChanged += new System.EventHandler(this.formPlayer_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formPlayer_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.wmpMedia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fswVideo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer wmpMedia;
        private System.IO.FileSystemWatcher fswVideo;
        private System.Windows.Forms.Label lblStatoVideo;
    }
}