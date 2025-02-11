
namespace AIO
{
    partial class uc_Flash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_Flash));
            this.cmb_deviceID = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmb_hPB = new System.Windows.Forms.ComboBox();
            this.grp_flash = new System.Windows.Forms.GroupBox();
            this.lbl_barcode = new System.Windows.Forms.Label();
            this.lbl_BinSize = new System.Windows.Forms.Label();
            this.btn_GetBin = new System.Windows.Forms.Button();
            this.lbl_jedecid = new System.Windows.Forms.Label();
            this.lbl_FLASHINFO = new System.Windows.Forms.Label();
            this.btn_FLASHINFO = new System.Windows.Forms.Button();
            this.lbl_fileofflashread = new System.Windows.Forms.Label();
            this.btn_FLASHREAD = new System.Windows.Forms.Button();
            this.cmb_FlashSel = new System.Windows.Forms.ComboBox();
            this.lbl_BinChecksum = new System.Windows.Forms.Label();
            this.txt_FlashFileNameFull = new System.Windows.Forms.TextBox();
            this.btn_FLASHWRITE = new System.Windows.Forms.Button();
            this.txt_FlashFileName = new System.Windows.Forms.TextBox();
            this.grp_spi = new System.Windows.Forms.GroupBox();
            this.btn_jedecidread = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_spiFlashFileName = new System.Windows.Forms.TextBox();
            this.btn_spiFlashWR = new System.Windows.Forms.Button();
            this.SPI_1MB_rdbtn = new System.Windows.Forms.RadioButton();
            this.lst_get1 = new System.Windows.Forms.ListBox();
            this.chk_NVBC = new System.Windows.Forms.CheckBox();
            this.grp_ocp = new System.Windows.Forms.GroupBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.OCP_Enable_btn = new System.Windows.Forms.Button();
            this.OCP_Disable_btn = new System.Windows.Forms.Button();
            this.OCP_Auto_btn = new System.Windows.Forms.Button();
            this.textBox82 = new System.Windows.Forms.TextBox();
            this.MCU_OCP_Rd_dGV = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grp_flash.SuspendLayout();
            this.grp_spi.SuspendLayout();
            this.grp_ocp.SuspendLayout();
            this.groupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MCU_OCP_Rd_dGV)).BeginInit();
            this.SuspendLayout();
            // 
            // cmb_deviceID
            // 
            this.cmb_deviceID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmb_deviceID, "cmb_deviceID");
            this.cmb_deviceID.FormattingEnabled = true;
            this.cmb_deviceID.Items.AddRange(new object[] {
            resources.GetString("cmb_deviceID.Items"),
            resources.GetString("cmb_deviceID.Items1"),
            resources.GetString("cmb_deviceID.Items2"),
            resources.GetString("cmb_deviceID.Items3"),
            resources.GetString("cmb_deviceID.Items4"),
            resources.GetString("cmb_deviceID.Items5"),
            resources.GetString("cmb_deviceID.Items6"),
            resources.GetString("cmb_deviceID.Items7"),
            resources.GetString("cmb_deviceID.Items8"),
            resources.GetString("cmb_deviceID.Items9"),
            resources.GetString("cmb_deviceID.Items10"),
            resources.GetString("cmb_deviceID.Items11"),
            resources.GetString("cmb_deviceID.Items12"),
            resources.GetString("cmb_deviceID.Items13"),
            resources.GetString("cmb_deviceID.Items14"),
            resources.GetString("cmb_deviceID.Items15")});
            this.cmb_deviceID.Name = "cmb_deviceID";
            this.cmb_deviceID.SelectedIndexChanged += new System.EventHandler(this.cmb_deviceID_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cmb_hPB
            // 
            this.cmb_hPB.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.cmb_hPB, "cmb_hPB");
            this.cmb_hPB.FormattingEnabled = true;
            this.cmb_hPB.Name = "cmb_hPB";
            // 
            // grp_flash
            // 
            this.grp_flash.Controls.Add(this.lbl_barcode);
            this.grp_flash.Controls.Add(this.lbl_BinSize);
            this.grp_flash.Controls.Add(this.btn_GetBin);
            this.grp_flash.Controls.Add(this.lbl_jedecid);
            this.grp_flash.Controls.Add(this.lbl_FLASHINFO);
            this.grp_flash.Controls.Add(this.btn_FLASHINFO);
            this.grp_flash.Controls.Add(this.lbl_fileofflashread);
            this.grp_flash.Controls.Add(this.btn_FLASHREAD);
            this.grp_flash.Controls.Add(this.cmb_FlashSel);
            this.grp_flash.Controls.Add(this.lbl_BinChecksum);
            this.grp_flash.Controls.Add(this.txt_FlashFileNameFull);
            this.grp_flash.Controls.Add(this.btn_FLASHWRITE);
            this.grp_flash.Controls.Add(this.txt_FlashFileName);
            resources.ApplyResources(this.grp_flash, "grp_flash");
            this.grp_flash.Name = "grp_flash";
            this.grp_flash.TabStop = false;
            // 
            // lbl_barcode
            // 
            resources.ApplyResources(this.lbl_barcode, "lbl_barcode");
            this.lbl_barcode.Name = "lbl_barcode";
            // 
            // lbl_BinSize
            // 
            resources.ApplyResources(this.lbl_BinSize, "lbl_BinSize");
            this.lbl_BinSize.Name = "lbl_BinSize";
            // 
            // btn_GetBin
            // 
            resources.ApplyResources(this.btn_GetBin, "btn_GetBin");
            this.btn_GetBin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_GetBin.Name = "btn_GetBin";
            this.btn_GetBin.UseVisualStyleBackColor = true;
            this.btn_GetBin.Click += new System.EventHandler(this.btn_GetBin_Click);
            // 
            // lbl_jedecid
            // 
            resources.ApplyResources(this.lbl_jedecid, "lbl_jedecid");
            this.lbl_jedecid.BackColor = System.Drawing.Color.White;
            this.lbl_jedecid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_jedecid.Name = "lbl_jedecid";
            // 
            // lbl_FLASHINFO
            // 
            resources.ApplyResources(this.lbl_FLASHINFO, "lbl_FLASHINFO");
            this.lbl_FLASHINFO.Name = "lbl_FLASHINFO";
            // 
            // btn_FLASHINFO
            // 
            this.btn_FLASHINFO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.btn_FLASHINFO, "btn_FLASHINFO");
            this.btn_FLASHINFO.Name = "btn_FLASHINFO";
            this.btn_FLASHINFO.UseVisualStyleBackColor = false;
            this.btn_FLASHINFO.Click += new System.EventHandler(this.btn_FLASHINFO_Click);
            // 
            // lbl_fileofflashread
            // 
            resources.ApplyResources(this.lbl_fileofflashread, "lbl_fileofflashread");
            this.lbl_fileofflashread.Name = "lbl_fileofflashread";
            // 
            // btn_FLASHREAD
            // 
            this.btn_FLASHREAD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.btn_FLASHREAD, "btn_FLASHREAD");
            this.btn_FLASHREAD.Name = "btn_FLASHREAD";
            this.btn_FLASHREAD.UseVisualStyleBackColor = false;
            this.btn_FLASHREAD.Click += new System.EventHandler(this.btn_FLASHREAD_Click);
            // 
            // cmb_FlashSel
            // 
            this.cmb_FlashSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmb_FlashSel, "cmb_FlashSel");
            this.cmb_FlashSel.FormattingEnabled = true;
            this.cmb_FlashSel.Items.AddRange(new object[] {
            resources.GetString("cmb_FlashSel.Items"),
            resources.GetString("cmb_FlashSel.Items1"),
            resources.GetString("cmb_FlashSel.Items2")});
            this.cmb_FlashSel.Name = "cmb_FlashSel";
            this.cmb_FlashSel.SelectedIndexChanged += new System.EventHandler(this.cmb_FlashSel_SelectedIndexChanged);
            // 
            // lbl_BinChecksum
            // 
            resources.ApplyResources(this.lbl_BinChecksum, "lbl_BinChecksum");
            this.lbl_BinChecksum.Name = "lbl_BinChecksum";
            // 
            // txt_FlashFileNameFull
            // 
            resources.ApplyResources(this.txt_FlashFileNameFull, "txt_FlashFileNameFull");
            this.txt_FlashFileNameFull.Name = "txt_FlashFileNameFull";
            // 
            // btn_FLASHWRITE
            // 
            this.btn_FLASHWRITE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            resources.ApplyResources(this.btn_FLASHWRITE, "btn_FLASHWRITE");
            this.btn_FLASHWRITE.Name = "btn_FLASHWRITE";
            this.btn_FLASHWRITE.UseVisualStyleBackColor = false;
            this.btn_FLASHWRITE.Click += new System.EventHandler(this.btn_FLASHWRITE_Click);
            this.btn_FLASHWRITE.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_FLASHWRITE_MouseUp);
            // 
            // txt_FlashFileName
            // 
            this.txt_FlashFileName.BackColor = System.Drawing.Color.White;
            this.txt_FlashFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txt_FlashFileName, "txt_FlashFileName");
            this.txt_FlashFileName.Name = "txt_FlashFileName";
            this.txt_FlashFileName.TextChanged += new System.EventHandler(this.txt_FlashFileName_TextChanged);
            this.txt_FlashFileName.DoubleClick += new System.EventHandler(this.txt_FlashFileName_DoubleClick);
            // 
            // grp_spi
            // 
            this.grp_spi.Controls.Add(this.btn_jedecidread);
            this.grp_spi.Controls.Add(this.label1);
            this.grp_spi.Controls.Add(this.txt_spiFlashFileName);
            this.grp_spi.Controls.Add(this.btn_spiFlashWR);
            this.grp_spi.Controls.Add(this.SPI_1MB_rdbtn);
            this.grp_spi.Controls.Add(this.cmb_hPB);
            resources.ApplyResources(this.grp_spi, "grp_spi");
            this.grp_spi.Name = "grp_spi";
            this.grp_spi.TabStop = false;
            // 
            // btn_jedecidread
            // 
            this.btn_jedecidread.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.btn_jedecidread, "btn_jedecidread");
            this.btn_jedecidread.Name = "btn_jedecidread";
            this.btn_jedecidread.UseVisualStyleBackColor = false;
            this.btn_jedecidread.Click += new System.EventHandler(this.btn_jedecidread_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txt_spiFlashFileName
            // 
            this.txt_spiFlashFileName.BackColor = System.Drawing.Color.White;
            this.txt_spiFlashFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.txt_spiFlashFileName, "txt_spiFlashFileName");
            this.txt_spiFlashFileName.Name = "txt_spiFlashFileName";
            // 
            // btn_spiFlashWR
            // 
            this.btn_spiFlashWR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.btn_spiFlashWR, "btn_spiFlashWR");
            this.btn_spiFlashWR.Name = "btn_spiFlashWR";
            this.btn_spiFlashWR.UseVisualStyleBackColor = false;
            this.btn_spiFlashWR.Click += new System.EventHandler(this.btn_spiFlashWR_Click);
            // 
            // SPI_1MB_rdbtn
            // 
            resources.ApplyResources(this.SPI_1MB_rdbtn, "SPI_1MB_rdbtn");
            this.SPI_1MB_rdbtn.Checked = true;
            this.SPI_1MB_rdbtn.Name = "SPI_1MB_rdbtn";
            this.SPI_1MB_rdbtn.TabStop = true;
            this.SPI_1MB_rdbtn.UseVisualStyleBackColor = true;
            // 
            // lst_get1
            // 
            this.lst_get1.BackColor = System.Drawing.SystemColors.Control;
            this.lst_get1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.lst_get1, "lst_get1");
            this.lst_get1.FormattingEnabled = true;
            this.lst_get1.Name = "lst_get1";
            // 
            // chk_NVBC
            // 
            resources.ApplyResources(this.chk_NVBC, "chk_NVBC");
            this.chk_NVBC.BackColor = System.Drawing.SystemColors.Control;
            this.chk_NVBC.Name = "chk_NVBC";
            this.chk_NVBC.UseVisualStyleBackColor = false;
            this.chk_NVBC.CheckedChanged += new System.EventHandler(this.chk_NVBC_CheckedChanged);
            // 
            // grp_ocp
            // 
            this.grp_ocp.Controls.Add(this.groupBox20);
            this.grp_ocp.Controls.Add(this.MCU_OCP_Rd_dGV);
            resources.ApplyResources(this.grp_ocp, "grp_ocp");
            this.grp_ocp.Name = "grp_ocp";
            this.grp_ocp.TabStop = false;
            // 
            // groupBox20
            // 
            this.groupBox20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox20.Controls.Add(this.OCP_Enable_btn);
            this.groupBox20.Controls.Add(this.OCP_Disable_btn);
            this.groupBox20.Controls.Add(this.OCP_Auto_btn);
            this.groupBox20.Controls.Add(this.textBox82);
            resources.ApplyResources(this.groupBox20, "groupBox20");
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.TabStop = false;
            // 
            // OCP_Enable_btn
            // 
            this.OCP_Enable_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.OCP_Enable_btn, "OCP_Enable_btn");
            this.OCP_Enable_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.OCP_Enable_btn.Name = "OCP_Enable_btn";
            this.OCP_Enable_btn.UseVisualStyleBackColor = false;
            this.OCP_Enable_btn.Click += new System.EventHandler(this.OCP_Enable_btn_Click);
            // 
            // OCP_Disable_btn
            // 
            this.OCP_Disable_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.OCP_Disable_btn, "OCP_Disable_btn");
            this.OCP_Disable_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.OCP_Disable_btn.Name = "OCP_Disable_btn";
            this.OCP_Disable_btn.UseVisualStyleBackColor = false;
            this.OCP_Disable_btn.Click += new System.EventHandler(this.OCP_Disable_btn_Click);
            // 
            // OCP_Auto_btn
            // 
            this.OCP_Auto_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.OCP_Auto_btn, "OCP_Auto_btn");
            this.OCP_Auto_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.OCP_Auto_btn.Name = "OCP_Auto_btn";
            this.OCP_Auto_btn.UseVisualStyleBackColor = false;
            this.OCP_Auto_btn.Click += new System.EventHandler(this.OCP_Auto_btn_Click);
            // 
            // textBox82
            // 
            resources.ApplyResources(this.textBox82, "textBox82");
            this.textBox82.Name = "textBox82";
            // 
            // MCU_OCP_Rd_dGV
            // 
            this.MCU_OCP_Rd_dGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MCU_OCP_Rd_dGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn10});
            resources.ApplyResources(this.MCU_OCP_Rd_dGV, "MCU_OCP_Rd_dGV");
            this.MCU_OCP_Rd_dGV.Name = "MCU_OCP_Rd_dGV";
            this.MCU_OCP_Rd_dGV.RowTemplate.Height = 24;
            // 
            // dataGridViewTextBoxColumn10
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // uc_Flash
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grp_ocp);
            this.Controls.Add(this.grp_spi);
            this.Controls.Add(this.chk_NVBC);
            this.Controls.Add(this.lst_get1);
            this.Controls.Add(this.grp_flash);
            this.Controls.Add(this.cmb_deviceID);
            this.Name = "uc_Flash";
            this.Load += new System.EventHandler(this.uc_Flash_Load);
            this.grp_flash.ResumeLayout(false);
            this.grp_flash.PerformLayout();
            this.grp_spi.ResumeLayout(false);
            this.grp_spi.PerformLayout();
            this.grp_ocp.ResumeLayout(false);
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MCU_OCP_Rd_dGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ComboBox cmb_deviceID;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        internal System.Windows.Forms.ComboBox cmb_hPB;
        private System.Windows.Forms.GroupBox grp_flash;
        internal System.Windows.Forms.Label lbl_fileofflashread;
        internal System.Windows.Forms.Button btn_FLASHREAD;
        internal System.Windows.Forms.ComboBox cmb_FlashSel;
        internal System.Windows.Forms.Label lbl_BinChecksum;
        private System.Windows.Forms.TextBox txt_FlashFileNameFull;
        internal System.Windows.Forms.Button btn_FLASHWRITE;
        private System.Windows.Forms.TextBox txt_FlashFileName;
        private System.Windows.Forms.GroupBox grp_spi;
        private System.Windows.Forms.RadioButton SPI_1MB_rdbtn;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_spiFlashFileName;
        internal System.Windows.Forms.Button btn_spiFlashWR;
        internal System.Windows.Forms.Button btn_jedecidread;
        internal System.Windows.Forms.Button btn_FLASHINFO;
        internal System.Windows.Forms.Label lbl_FLASHINFO;
        internal System.Windows.Forms.Label lbl_jedecid;
        private System.Windows.Forms.ListBox lst_get1;
        private System.Windows.Forms.Button btn_GetBin;
        internal System.Windows.Forms.Label lbl_BinSize;
        internal System.Windows.Forms.Label lbl_barcode;
        public System.Windows.Forms.CheckBox chk_NVBC;
        private System.Windows.Forms.GroupBox grp_ocp;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Button OCP_Enable_btn;
        private System.Windows.Forms.Button OCP_Disable_btn;
        private System.Windows.Forms.Button OCP_Auto_btn;
        private System.Windows.Forms.TextBox textBox82;
        private System.Windows.Forms.DataGridView MCU_OCP_Rd_dGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
    }
}
