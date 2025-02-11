namespace AIO
{
    partial class i3_Pat
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
            this.lbl_Mark = new System.Windows.Forms.Label();
            this.pic_Mark = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Mark)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Mark
            // 
            this.lbl_Mark.BackColor = System.Drawing.Color.Red;
            this.lbl_Mark.Font = new System.Drawing.Font("Arial", 9F);
            this.lbl_Mark.Location = new System.Drawing.Point(3, 4);
            this.lbl_Mark.Name = "lbl_Mark";
            this.lbl_Mark.Size = new System.Drawing.Size(17, 13);
            this.lbl_Mark.TabIndex = 138;
            this.lbl_Mark.Visible = false;
            // 
            // pic_Mark
            // 
            this.pic_Mark.BackColor = System.Drawing.Color.White;
            this.pic_Mark.Location = new System.Drawing.Point(41, 9);
            this.pic_Mark.Name = "pic_Mark";
            this.pic_Mark.Size = new System.Drawing.Size(17, 13);
            this.pic_Mark.TabIndex = 203;
            this.pic_Mark.TabStop = false;
            this.pic_Mark.Tag = "mark";
            this.pic_Mark.Visible = false;
            // 
            // i3_Pat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(120, 46);
            this.ControlBox = false;
            this.Controls.Add(this.pic_Mark);
            this.Controls.Add(this.lbl_Mark);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "i3_Pat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.i3_Pat_FormClosed);
            this.Load += new System.EventHandler(this.i3_Pat_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.i3_Pat_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Mark)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Label lbl_Mark;
        internal System.Windows.Forms.PictureBox pic_Mark;
    }
}