namespace AIO
{
    partial class i3_Init
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
            this.components = new System.ComponentModel.Container();
            this.tme_Warn = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tme_Warn
            // 
            this.tme_Warn.Interval = 1000;
            this.tme_Warn.Tick += new System.EventHandler(this.tme_Warn_Tick);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // i3_Init
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 311);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "i3_Init";
            this.Text = "AutoGamma";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.i3_Init_FormClosed);
            this.Load += new System.EventHandler(this.i3_Init_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer tme_Warn;
        public System.Windows.Forms.Timer timer1;
    }
}