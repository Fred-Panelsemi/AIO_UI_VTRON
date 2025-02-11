
namespace AIO
{
    partial class uc_MCU
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_verMCU = new System.Windows.Forms.Button();
            this.lbl_verMCU = new System.Windows.Forms.Label();
            this.lst_get1 = new System.Windows.Forms.ListBox();
            this.btn_idSerialmode = new System.Windows.Forms.Button();
            this.btn_autoID = new System.Windows.Forms.Button();
            this.txt_autoID = new System.Windows.Forms.TextBox();
            this.btn_wrgetDevID = new System.Windows.Forms.Button();
            this.tme_warn = new System.Windows.Forms.Timer(this.components);
            this.btn_BoxRead = new System.Windows.Forms.Button();
            this.btn_IDON = new System.Windows.Forms.Button();
            this.btn_IDOFF = new System.Windows.Forms.Button();
            this.btn_OCPOnOff = new System.Windows.Forms.Button();
            this.chk_SWOCP2nd = new System.Windows.Forms.CheckBox();
            this.chk_SWOCP1st = new System.Windows.Forms.CheckBox();
            this.btn_SWRESET = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.cmb_deviceID = new System.Windows.Forms.ComboBox();
            this.btn_wrDevID = new System.Windows.Forms.Button();
            this.chk_boardcast = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_verMCU
            // 
            this.btn_verMCU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_verMCU.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_verMCU.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_verMCU.Location = new System.Drawing.Point(11, 33);
            this.btn_verMCU.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_verMCU.Name = "btn_verMCU";
            this.btn_verMCU.Size = new System.Drawing.Size(113, 24);
            this.btn_verMCU.TabIndex = 297;
            this.btn_verMCU.Text = "Version";
            this.btn_verMCU.UseVisualStyleBackColor = false;
            this.btn_verMCU.Click += new System.EventHandler(this.btn_verMCU_Click);
            // 
            // lbl_verMCU
            // 
            this.lbl_verMCU.AutoSize = true;
            this.lbl_verMCU.Font = new System.Drawing.Font("Arial", 8.25F);
            this.lbl_verMCU.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_verMCU.Location = new System.Drawing.Point(134, 38);
            this.lbl_verMCU.Name = "lbl_verMCU";
            this.lbl_verMCU.Size = new System.Drawing.Size(32, 14);
            this.lbl_verMCU.TabIndex = 897;
            this.lbl_verMCU.Text = "- - - -";
            this.lbl_verMCU.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lst_get1
            // 
            this.lst_get1.BackColor = System.Drawing.SystemColors.Control;
            this.lst_get1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lst_get1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.lst_get1.FormattingEnabled = true;
            this.lst_get1.HorizontalScrollbar = true;
            this.lst_get1.ItemHeight = 14;
            this.lst_get1.Location = new System.Drawing.Point(11, 230);
            this.lst_get1.Name = "lst_get1";
            this.lst_get1.Size = new System.Drawing.Size(636, 168);
            this.lst_get1.TabIndex = 898;
            // 
            // btn_idSerialmode
            // 
            this.btn_idSerialmode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_idSerialmode.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_idSerialmode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_idSerialmode.Location = new System.Drawing.Point(134, 59);
            this.btn_idSerialmode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_idSerialmode.Name = "btn_idSerialmode";
            this.btn_idSerialmode.Size = new System.Drawing.Size(113, 24);
            this.btn_idSerialmode.TabIndex = 899;
            this.btn_idSerialmode.Text = "ID && Series Mode";
            this.btn_idSerialmode.UseVisualStyleBackColor = false;
            this.btn_idSerialmode.Click += new System.EventHandler(this.btn_idSerialmode_Click);
            // 
            // btn_autoID
            // 
            this.btn_autoID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_autoID.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_autoID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_autoID.Location = new System.Drawing.Point(134, 85);
            this.btn_autoID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_autoID.Name = "btn_autoID";
            this.btn_autoID.Size = new System.Drawing.Size(113, 24);
            this.btn_autoID.TabIndex = 900;
            this.btn_autoID.Text = "AUTO ID";
            this.btn_autoID.UseVisualStyleBackColor = false;
            this.btn_autoID.Click += new System.EventHandler(this.btn_autoID_Click);
            // 
            // txt_autoID
            // 
            this.txt_autoID.Font = new System.Drawing.Font("Arial", 8.25F);
            this.txt_autoID.Location = new System.Drawing.Point(251, 88);
            this.txt_autoID.Name = "txt_autoID";
            this.txt_autoID.Size = new System.Drawing.Size(34, 20);
            this.txt_autoID.TabIndex = 901;
            this.txt_autoID.Text = "1";
            // 
            // btn_wrgetDevID
            // 
            this.btn_wrgetDevID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_wrgetDevID.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_wrgetDevID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_wrgetDevID.Location = new System.Drawing.Point(134, 111);
            this.btn_wrgetDevID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_wrgetDevID.Name = "btn_wrgetDevID";
            this.btn_wrgetDevID.Size = new System.Drawing.Size(113, 24);
            this.btn_wrgetDevID.TabIndex = 902;
            this.btn_wrgetDevID.Text = "Wr Get DevID";
            this.btn_wrgetDevID.UseVisualStyleBackColor = false;
            this.btn_wrgetDevID.Click += new System.EventHandler(this.btn_wrgetDevID_Click);
            // 
            // tme_warn
            // 
            this.tme_warn.Tick += new System.EventHandler(this.tme_warn_Tick);
            // 
            // btn_BoxRead
            // 
            this.btn_BoxRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_BoxRead.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_BoxRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_BoxRead.Location = new System.Drawing.Point(11, 59);
            this.btn_BoxRead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_BoxRead.Name = "btn_BoxRead";
            this.btn_BoxRead.Size = new System.Drawing.Size(113, 24);
            this.btn_BoxRead.TabIndex = 903;
            this.btn_BoxRead.Text = "BoxRead";
            this.btn_BoxRead.UseVisualStyleBackColor = false;
            this.btn_BoxRead.Click += new System.EventHandler(this.btn_BoxRead_Click);
            // 
            // btn_IDON
            // 
            this.btn_IDON.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_IDON.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_IDON.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_IDON.Location = new System.Drawing.Point(11, 137);
            this.btn_IDON.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_IDON.Name = "btn_IDON";
            this.btn_IDON.Size = new System.Drawing.Size(56, 24);
            this.btn_IDON.TabIndex = 905;
            this.btn_IDON.Tag = "1";
            this.btn_IDON.Text = "ID ON";
            this.btn_IDON.UseVisualStyleBackColor = false;
            this.btn_IDON.Click += new System.EventHandler(this.btn_IDON_Click);
            // 
            // btn_IDOFF
            // 
            this.btn_IDOFF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_IDOFF.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_IDOFF.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_IDOFF.Location = new System.Drawing.Point(68, 137);
            this.btn_IDOFF.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_IDOFF.Name = "btn_IDOFF";
            this.btn_IDOFF.Size = new System.Drawing.Size(56, 24);
            this.btn_IDOFF.TabIndex = 906;
            this.btn_IDOFF.Tag = "0";
            this.btn_IDOFF.Text = "ID OFF";
            this.btn_IDOFF.UseVisualStyleBackColor = false;
            this.btn_IDOFF.Click += new System.EventHandler(this.btn_IDON_Click);
            // 
            // btn_OCPOnOff
            // 
            this.btn_OCPOnOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_OCPOnOff.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_OCPOnOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_OCPOnOff.Location = new System.Drawing.Point(317, 59);
            this.btn_OCPOnOff.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_OCPOnOff.Name = "btn_OCPOnOff";
            this.btn_OCPOnOff.Size = new System.Drawing.Size(113, 24);
            this.btn_OCPOnOff.TabIndex = 907;
            this.btn_OCPOnOff.Tag = "1";
            this.btn_OCPOnOff.Text = "OCP On / Off";
            this.btn_OCPOnOff.UseVisualStyleBackColor = false;
            this.btn_OCPOnOff.Click += new System.EventHandler(this.btn_OCPOnOff_Click);
            // 
            // chk_SWOCP2nd
            // 
            this.chk_SWOCP2nd.AutoSize = true;
            this.chk_SWOCP2nd.Font = new System.Drawing.Font("Arial", 8.25F);
            this.chk_SWOCP2nd.Location = new System.Drawing.Point(436, 72);
            this.chk_SWOCP2nd.Name = "chk_SWOCP2nd";
            this.chk_SWOCP2nd.Size = new System.Drawing.Size(114, 18);
            this.chk_SWOCP2nd.TabIndex = 910;
            this.chk_SWOCP2nd.Text = "Stage2. 使用偵測";
            this.chk_SWOCP2nd.UseVisualStyleBackColor = true;
            // 
            // chk_SWOCP1st
            // 
            this.chk_SWOCP1st.AutoSize = true;
            this.chk_SWOCP1st.Font = new System.Drawing.Font("Arial", 8.25F);
            this.chk_SWOCP1st.Location = new System.Drawing.Point(436, 54);
            this.chk_SWOCP1st.Name = "chk_SWOCP1st";
            this.chk_SWOCP1st.Size = new System.Drawing.Size(114, 18);
            this.chk_SWOCP1st.TabIndex = 909;
            this.chk_SWOCP1st.Text = "Stage1. 開機偵測";
            this.chk_SWOCP1st.UseVisualStyleBackColor = true;
            // 
            // btn_SWRESET
            // 
            this.btn_SWRESET.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_SWRESET.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_SWRESET.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_SWRESET.Location = new System.Drawing.Point(317, 111);
            this.btn_SWRESET.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_SWRESET.Name = "btn_SWRESET";
            this.btn_SWRESET.Size = new System.Drawing.Size(113, 24);
            this.btn_SWRESET.TabIndex = 911;
            this.btn_SWRESET.Tag = "1";
            this.btn_SWRESET.Text = "SW RESET";
            this.btn_SWRESET.UseVisualStyleBackColor = false;
            this.btn_SWRESET.Click += new System.EventHandler(this.btn_SWRESET_Click);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial", 9F);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(526, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 15);
            this.label10.TabIndex = 913;
            this.label10.Text = "deviceID";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label10.Visible = false;
            // 
            // cmb_deviceID
            // 
            this.cmb_deviceID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_deviceID.Font = new System.Drawing.Font("Arial", 8F);
            this.cmb_deviceID.FormattingEnabled = true;
            this.cmb_deviceID.Location = new System.Drawing.Point(11, 8);
            this.cmb_deviceID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmb_deviceID.Name = "cmb_deviceID";
            this.cmb_deviceID.Size = new System.Drawing.Size(113, 22);
            this.cmb_deviceID.TabIndex = 912;
            this.cmb_deviceID.SelectedIndexChanged += new System.EventHandler(this.cmb_deviceID_SelectedIndexChanged);
            // 
            // btn_wrDevID
            // 
            this.btn_wrDevID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_wrDevID.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btn_wrDevID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_wrDevID.Location = new System.Drawing.Point(11, 111);
            this.btn_wrDevID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_wrDevID.Name = "btn_wrDevID";
            this.btn_wrDevID.Size = new System.Drawing.Size(113, 24);
            this.btn_wrDevID.TabIndex = 914;
            this.btn_wrDevID.Text = "Wr DevID(復原ID)";
            this.btn_wrDevID.UseVisualStyleBackColor = false;
            this.btn_wrDevID.Click += new System.EventHandler(this.btn_wrDevID_Click);
            // 
            // chk_boardcast
            // 
            this.chk_boardcast.AutoSize = true;
            this.chk_boardcast.Font = new System.Drawing.Font("Arial", 9F);
            this.chk_boardcast.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chk_boardcast.Location = new System.Drawing.Point(436, 114);
            this.chk_boardcast.Name = "chk_boardcast";
            this.chk_boardcast.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chk_boardcast.Size = new System.Drawing.Size(85, 19);
            this.chk_boardcast.TabIndex = 939;
            this.chk_boardcast.Text = "BoardCast";
            this.chk_boardcast.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chk_boardcast.UseVisualStyleBackColor = true;
            this.chk_boardcast.CheckedChanged += new System.EventHandler(this.chk_boardcast_CheckedChanged);
            // 
            // uc_MCU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_boardcast);
            this.Controls.Add(this.btn_wrDevID);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmb_deviceID);
            this.Controls.Add(this.btn_BoxRead);
            this.Controls.Add(this.btn_SWRESET);
            this.Controls.Add(this.chk_SWOCP2nd);
            this.Controls.Add(this.chk_SWOCP1st);
            this.Controls.Add(this.btn_OCPOnOff);
            this.Controls.Add(this.btn_IDOFF);
            this.Controls.Add(this.btn_IDON);
            this.Controls.Add(this.btn_wrgetDevID);
            this.Controls.Add(this.txt_autoID);
            this.Controls.Add(this.btn_autoID);
            this.Controls.Add(this.btn_idSerialmode);
            this.Controls.Add(this.lst_get1);
            this.Controls.Add(this.lbl_verMCU);
            this.Controls.Add(this.btn_verMCU);
            this.Name = "uc_MCU";
            this.Size = new System.Drawing.Size(661, 404);
            this.Load += new System.EventHandler(this.uc_MCU_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btn_verMCU;
        internal System.Windows.Forms.Label lbl_verMCU;
        private System.Windows.Forms.ListBox lst_get1;
        internal System.Windows.Forms.Button btn_idSerialmode;
        internal System.Windows.Forms.Button btn_autoID;
        private System.Windows.Forms.TextBox txt_autoID;
        internal System.Windows.Forms.Button btn_wrgetDevID;
        private System.Windows.Forms.Timer tme_warn;
        internal System.Windows.Forms.Button btn_BoxRead;
        internal System.Windows.Forms.Button btn_IDON;
        internal System.Windows.Forms.Button btn_IDOFF;
        internal System.Windows.Forms.Button btn_OCPOnOff;
        private System.Windows.Forms.CheckBox chk_SWOCP2nd;
        private System.Windows.Forms.CheckBox chk_SWOCP1st;
        internal System.Windows.Forms.Button btn_SWRESET;
        private System.Windows.Forms.Label label10;
        internal System.Windows.Forms.ComboBox cmb_deviceID;
        internal System.Windows.Forms.Button btn_wrDevID;
        internal System.Windows.Forms.CheckBox chk_boardcast;
    }
}
