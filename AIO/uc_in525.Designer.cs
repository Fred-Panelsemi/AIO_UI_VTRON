
namespace AIO
{
    partial class uc_in525
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
            this.cmb_LANpos = new System.Windows.Forms.ComboBox();
            this.chk_LANpos = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_Readchecksum = new System.Windows.Forms.Label();
            this.lbl_Loadchecksum = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Load = new System.Windows.Forms.Button();
            this.chk_MTP = new System.Windows.Forms.CheckBox();
            this.btn_Read = new System.Windows.Forms.Button();
            this.btn_Write = new System.Windows.Forms.Button();
            this.dgv_Bin = new System.Windows.Forms.DataGridView();
            this.cmb_BinDes = new System.Windows.Forms.ComboBox();
            this.lbl_BinDes = new System.Windows.Forms.Label();
            this.lbl_BinHex = new System.Windows.Forms.Label();
            this.lbl_BinVal = new System.Windows.Forms.Label();
            this.cmb_BinVal = new System.Windows.Forms.ComboBox();
            this.cmb_BinVol = new System.Windows.Forms.ComboBox();
            this.lbl_BinVol = new System.Windows.Forms.Label();
            this.cmb_deviceID = new System.Windows.Forms.ComboBox();
            this.textBox183 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_pwicinfo = new System.Windows.Forms.Button();
            this.cmb_hPB = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Bin)).BeginInit();
            this.SuspendLayout();
            // 
            // cmb_LANpos
            // 
            this.cmb_LANpos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_LANpos.Font = new System.Drawing.Font("Arial", 8F);
            this.cmb_LANpos.FormattingEnabled = true;
            this.cmb_LANpos.Location = new System.Drawing.Point(655, 28);
            this.cmb_LANpos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmb_LANpos.Name = "cmb_LANpos";
            this.cmb_LANpos.Size = new System.Drawing.Size(159, 22);
            this.cmb_LANpos.TabIndex = 428;
            this.cmb_LANpos.Visible = false;
            // 
            // chk_LANpos
            // 
            this.chk_LANpos.AutoSize = true;
            this.chk_LANpos.BackColor = System.Drawing.Color.Transparent;
            this.chk_LANpos.Font = new System.Drawing.Font("Arial", 9F);
            this.chk_LANpos.Location = new System.Drawing.Point(754, 10);
            this.chk_LANpos.Name = "chk_LANpos";
            this.chk_LANpos.Size = new System.Drawing.Size(50, 19);
            this.chk_LANpos.TabIndex = 430;
            this.chk_LANpos.Text = "view";
            this.chk_LANpos.UseVisualStyleBackColor = false;
            this.chk_LANpos.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(655, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 429;
            this.label1.Text = "LAN POSITION";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // lbl_Readchecksum
            // 
            this.lbl_Readchecksum.AutoSize = true;
            this.lbl_Readchecksum.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Readchecksum.Location = new System.Drawing.Point(741, 112);
            this.lbl_Readchecksum.Name = "lbl_Readchecksum";
            this.lbl_Readchecksum.Size = new System.Drawing.Size(87, 14);
            this.lbl_Readchecksum.TabIndex = 427;
            this.lbl_Readchecksum.Text = "checksum_Read";
            this.lbl_Readchecksum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Loadchecksum
            // 
            this.lbl_Loadchecksum.AutoSize = true;
            this.lbl_Loadchecksum.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Loadchecksum.Location = new System.Drawing.Point(656, 112);
            this.lbl_Loadchecksum.Name = "lbl_Loadchecksum";
            this.lbl_Loadchecksum.Size = new System.Drawing.Size(86, 14);
            this.lbl_Loadchecksum.TabIndex = 426;
            this.lbl_Loadchecksum.Text = "checksum_Load";
            this.lbl_Loadchecksum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn_Save.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.Location = new System.Drawing.Point(655, 88);
            this.btn_Save.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(76, 23);
            this.btn_Save.TabIndex = 425;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Load
            // 
            this.btn_Load.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn_Load.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Load.Location = new System.Drawing.Point(655, 59);
            this.btn_Load.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(76, 23);
            this.btn_Load.TabIndex = 424;
            this.btn_Load.Text = "Load";
            this.btn_Load.UseVisualStyleBackColor = false;
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // chk_MTP
            // 
            this.chk_MTP.AutoSize = true;
            this.chk_MTP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.chk_MTP.Location = new System.Drawing.Point(744, 63);
            this.chk_MTP.Name = "chk_MTP";
            this.chk_MTP.Size = new System.Drawing.Size(15, 14);
            this.chk_MTP.TabIndex = 423;
            this.chk_MTP.UseVisualStyleBackColor = false;
            this.chk_MTP.CheckedChanged += new System.EventHandler(this.chk_MTP_CheckedChanged);
            // 
            // btn_Read
            // 
            this.btn_Read.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_Read.Font = new System.Drawing.Font("Arial", 9F);
            this.btn_Read.Location = new System.Drawing.Point(738, 88);
            this.btn_Read.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Read.Name = "btn_Read";
            this.btn_Read.Size = new System.Drawing.Size(76, 23);
            this.btn_Read.TabIndex = 422;
            this.btn_Read.Text = "Read";
            this.btn_Read.UseVisualStyleBackColor = false;
            this.btn_Read.Click += new System.EventHandler(this.btn_Read_Click);
            // 
            // btn_Write
            // 
            this.btn_Write.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_Write.Font = new System.Drawing.Font("Arial", 9F);
            this.btn_Write.Location = new System.Drawing.Point(738, 59);
            this.btn_Write.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Write.Name = "btn_Write";
            this.btn_Write.Size = new System.Drawing.Size(76, 23);
            this.btn_Write.TabIndex = 421;
            this.btn_Write.Text = "    Write";
            this.btn_Write.UseVisualStyleBackColor = false;
            this.btn_Write.Click += new System.EventHandler(this.btn_Write_Click);
            // 
            // dgv_Bin
            // 
            this.dgv_Bin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Bin.Location = new System.Drawing.Point(5, 82);
            this.dgv_Bin.Name = "dgv_Bin";
            this.dgv_Bin.RowTemplate.Height = 24;
            this.dgv_Bin.Size = new System.Drawing.Size(639, 180);
            this.dgv_Bin.TabIndex = 420;
            // 
            // cmb_BinDes
            // 
            this.cmb_BinDes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cmb_BinDes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_BinDes.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_BinDes.FormattingEnabled = true;
            this.cmb_BinDes.Items.AddRange(new object[] {
            "Click ! Register Select"});
            this.cmb_BinDes.Location = new System.Drawing.Point(342, 269);
            this.cmb_BinDes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmb_BinDes.Name = "cmb_BinDes";
            this.cmb_BinDes.Size = new System.Drawing.Size(52, 22);
            this.cmb_BinDes.TabIndex = 431;
            this.cmb_BinDes.TabStop = false;
            this.cmb_BinDes.Visible = false;
            this.cmb_BinDes.SelectedIndexChanged += new System.EventHandler(this.cmb_BinDes_SelectedIndexChanged);
            this.cmb_BinDes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_BinDes_KeyDown);
            this.cmb_BinDes.MouseLeave += new System.EventHandler(this.cmb_BinDes_MouseLeave);
            // 
            // lbl_BinDes
            // 
            this.lbl_BinDes.AutoSize = true;
            this.lbl_BinDes.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_BinDes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbl_BinDes.Location = new System.Drawing.Point(139, 17);
            this.lbl_BinDes.Name = "lbl_BinDes";
            this.lbl_BinDes.Size = new System.Drawing.Size(41, 14);
            this.lbl_BinDes.TabIndex = 432;
            this.lbl_BinDes.Text = "BinDes";
            this.lbl_BinDes.Click += new System.EventHandler(this.lbl_BinDes_Click);
            // 
            // lbl_BinHex
            // 
            this.lbl_BinHex.AutoSize = true;
            this.lbl_BinHex.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_BinHex.Location = new System.Drawing.Point(349, 299);
            this.lbl_BinHex.Name = "lbl_BinHex";
            this.lbl_BinHex.Size = new System.Drawing.Size(41, 14);
            this.lbl_BinHex.TabIndex = 433;
            this.lbl_BinHex.Text = "BinHex";
            this.lbl_BinHex.Visible = false;
            // 
            // lbl_BinVal
            // 
            this.lbl_BinVal.AutoSize = true;
            this.lbl_BinVal.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_BinVal.ForeColor = System.Drawing.Color.Black;
            this.lbl_BinVal.Location = new System.Drawing.Point(349, 313);
            this.lbl_BinVal.Name = "lbl_BinVal";
            this.lbl_BinVal.Size = new System.Drawing.Size(37, 14);
            this.lbl_BinVal.TabIndex = 434;
            this.lbl_BinVal.Text = "BinVal";
            this.lbl_BinVal.Visible = false;
            // 
            // cmb_BinVal
            // 
            this.cmb_BinVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cmb_BinVal.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_BinVal.FormattingEnabled = true;
            this.cmb_BinVal.Items.AddRange(new object[] {
            "Click ! Register Select"});
            this.cmb_BinVal.Location = new System.Drawing.Point(400, 269);
            this.cmb_BinVal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmb_BinVal.Name = "cmb_BinVal";
            this.cmb_BinVal.Size = new System.Drawing.Size(52, 22);
            this.cmb_BinVal.TabIndex = 435;
            this.cmb_BinVal.TabStop = false;
            this.cmb_BinVal.Visible = false;
            this.cmb_BinVal.SelectedIndexChanged += new System.EventHandler(this.cmb_BinVal_SelectedIndexChanged);
            this.cmb_BinVal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_BinVal_KeyDown);
            this.cmb_BinVal.MouseLeave += new System.EventHandler(this.cmb_BinVal_MouseLeave);
            // 
            // cmb_BinVol
            // 
            this.cmb_BinVol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cmb_BinVol.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_BinVol.FormattingEnabled = true;
            this.cmb_BinVol.Items.AddRange(new object[] {
            "Click ! Register Select"});
            this.cmb_BinVol.Location = new System.Drawing.Point(458, 269);
            this.cmb_BinVol.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmb_BinVol.Name = "cmb_BinVol";
            this.cmb_BinVol.Size = new System.Drawing.Size(52, 22);
            this.cmb_BinVol.TabIndex = 436;
            this.cmb_BinVol.TabStop = false;
            this.cmb_BinVol.Visible = false;
            this.cmb_BinVol.SelectedIndexChanged += new System.EventHandler(this.cmb_BinVol_SelectedIndexChanged);
            this.cmb_BinVol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_BinVol_KeyDown);
            this.cmb_BinVol.MouseLeave += new System.EventHandler(this.cmb_BinVol_MouseLeave);
            // 
            // lbl_BinVol
            // 
            this.lbl_BinVol.AutoSize = true;
            this.lbl_BinVol.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_BinVol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lbl_BinVol.Location = new System.Drawing.Point(186, 40);
            this.lbl_BinVol.Name = "lbl_BinVol";
            this.lbl_BinVol.Size = new System.Drawing.Size(37, 14);
            this.lbl_BinVol.TabIndex = 437;
            this.lbl_BinVol.Text = "BinVol";
            this.lbl_BinVol.Visible = false;
            this.lbl_BinVol.Click += new System.EventHandler(this.lbl_BinVol_Click);
            // 
            // cmb_deviceID
            // 
            this.cmb_deviceID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_deviceID.Font = new System.Drawing.Font("Arial", 8F);
            this.cmb_deviceID.FormattingEnabled = true;
            this.cmb_deviceID.Items.AddRange(new object[] {
            " 1-1_1",
            " 1-1_2",
            " 1-1_3",
            " 1-1_4",
            " 1-2_1",
            " 1-2_2",
            " 1-2_3",
            " 1-2_4",
            " 2-1_1",
            " 2-1_2",
            " 2-1_3",
            " 2-1_4",
            " 2-2_1",
            " 2-2_2",
            " 2-2_3",
            " 2-2_4"});
            this.cmb_deviceID.Location = new System.Drawing.Point(11, 14);
            this.cmb_deviceID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmb_deviceID.Name = "cmb_deviceID";
            this.cmb_deviceID.Size = new System.Drawing.Size(68, 22);
            this.cmb_deviceID.TabIndex = 880;
            this.cmb_deviceID.SelectedIndexChanged += new System.EventHandler(this.cmb_deviceID_SelectedIndexChanged);
            // 
            // textBox183
            // 
            this.textBox183.Font = new System.Drawing.Font("Arial", 9F);
            this.textBox183.Location = new System.Drawing.Point(619, 60);
            this.textBox183.Name = "textBox183";
            this.textBox183.Size = new System.Drawing.Size(24, 21);
            this.textBox183.TabIndex = 881;
            this.textBox183.Text = "40";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(521, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 882;
            this.label2.Text = "Device Address：";
            // 
            // btn_pwicinfo
            // 
            this.btn_pwicinfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_pwicinfo.Font = new System.Drawing.Font("Arial", 9F);
            this.btn_pwicinfo.Location = new System.Drawing.Point(5, 267);
            this.btn_pwicinfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_pwicinfo.Name = "btn_pwicinfo";
            this.btn_pwicinfo.Size = new System.Drawing.Size(76, 23);
            this.btn_pwicinfo.TabIndex = 883;
            this.btn_pwicinfo.Text = "IC Info";
            this.btn_pwicinfo.UseVisualStyleBackColor = false;
            this.btn_pwicinfo.Click += new System.EventHandler(this.btn_pwicinfo_Click);
            // 
            // cmb_hPB
            // 
            this.cmb_hPB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_hPB.Font = new System.Drawing.Font("Arial", 8F);
            this.cmb_hPB.FormattingEnabled = true;
            this.cmb_hPB.Items.AddRange(new object[] {
            " 1-1_1",
            " 1-1_2",
            " 1-1_3",
            " 1-1_4",
            " 1-2_1",
            " 1-2_2",
            " 1-2_3",
            " 1-2_4",
            " 2-1_1",
            " 2-1_2",
            " 2-1_3",
            " 2-1_4",
            " 2-2_1",
            " 2-2_2",
            " 2-2_3",
            " 2-2_4"});
            this.cmb_hPB.Location = new System.Drawing.Point(400, 299);
            this.cmb_hPB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmb_hPB.Name = "cmb_hPB";
            this.cmb_hPB.Size = new System.Drawing.Size(76, 22);
            this.cmb_hPB.TabIndex = 884;
            this.cmb_hPB.SelectedIndexChanged += new System.EventHandler(this.cmb_hPB_SelectedIndexChanged);
            // 
            // uc_in525
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmb_hPB);
            this.Controls.Add(this.btn_pwicinfo);
            this.Controls.Add(this.textBox183);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmb_deviceID);
            this.Controls.Add(this.lbl_BinVol);
            this.Controls.Add(this.cmb_BinVol);
            this.Controls.Add(this.cmb_BinVal);
            this.Controls.Add(this.lbl_BinVal);
            this.Controls.Add(this.lbl_BinHex);
            this.Controls.Add(this.lbl_BinDes);
            this.Controls.Add(this.cmb_BinDes);
            this.Controls.Add(this.cmb_LANpos);
            this.Controls.Add(this.chk_LANpos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_Readchecksum);
            this.Controls.Add(this.lbl_Loadchecksum);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Load);
            this.Controls.Add(this.chk_MTP);
            this.Controls.Add(this.btn_Read);
            this.Controls.Add(this.btn_Write);
            this.Controls.Add(this.dgv_Bin);
            this.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "uc_in525";
            this.Size = new System.Drawing.Size(847, 390);
            this.Load += new System.EventHandler(this.uc_in525_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Bin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ComboBox cmb_LANpos;
        internal System.Windows.Forms.CheckBox chk_LANpos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_Readchecksum;
        private System.Windows.Forms.Label lbl_Loadchecksum;
        internal System.Windows.Forms.Button btn_Save;
        internal System.Windows.Forms.Button btn_Load;
        internal System.Windows.Forms.CheckBox chk_MTP;
        internal System.Windows.Forms.Button btn_Read;
        internal System.Windows.Forms.Button btn_Write;
        private System.Windows.Forms.DataGridView dgv_Bin;
        internal System.Windows.Forms.ComboBox cmb_BinDes;
        private System.Windows.Forms.Label lbl_BinDes;
        internal System.Windows.Forms.Label lbl_BinHex;
        internal System.Windows.Forms.Label lbl_BinVal;
        internal System.Windows.Forms.ComboBox cmb_BinVal;
        internal System.Windows.Forms.ComboBox cmb_BinVol;
        internal System.Windows.Forms.Label lbl_BinVol;
        internal System.Windows.Forms.ComboBox cmb_deviceID;
        private System.Windows.Forms.TextBox textBox183;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Button btn_pwicinfo;
        internal System.Windows.Forms.ComboBox cmb_hPB;
    }
}
