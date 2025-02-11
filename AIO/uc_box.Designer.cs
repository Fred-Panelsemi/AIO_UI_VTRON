namespace AIO
{
    partial class uc_box
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_box));
            this.dgv_box = new System.Windows.Forms.DataGridView();
            this.tme_busy = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.h_dgvhscroll = new System.Windows.Forms.HScrollBar();
            this.btn_dgv = new System.Windows.Forms.Button();
            this.lst_get1 = new System.Windows.Forms.ListBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.lbl_col = new System.Windows.Forms.Label();
            this.btn_single = new System.Windows.Forms.Button();
            this.lbl_dgvHscr = new System.Windows.Forms.Label();
            this.numUD_bxCs = new System.Windows.Forms.NumericUpDown();
            this.cmb_box = new System.Windows.Forms.ComboBox();
            this.numUD_bxRs = new System.Windows.Forms.NumericUpDown();
            this.btn_preview = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl_newRv = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lst_filepathfull = new System.Windows.Forms.ListBox();
            this.btn_resGen = new System.Windows.Forms.Button();
            this.lbl_newCv = new System.Windows.Forms.Label();
            this.lbl_colfrozen = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_cnt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.grp_dgvnewpos = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pnl_dgv = new AIO.DoubleBufferPanel();
            this.pnl_busy = new AIO.TransparentPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_bxCs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_bxRs)).BeginInit();
            this.grp_dgvnewpos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_box
            // 
            this.dgv_box.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgv_box, "dgv_box");
            this.dgv_box.Name = "dgv_box";
            this.dgv_box.RowTemplate.Height = 24;
            this.dgv_box.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_box_CellMouseClick);
            this.dgv_box.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgv_box_Scroll);
            this.dgv_box.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgv_box_MouseMove);
            this.dgv_box.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgv_box_MouseUp);
            // 
            // tme_busy
            // 
            this.tme_busy.Interval = 50;
            this.tme_busy.Tick += new System.EventHandler(this.tme_busy_Tick);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // h_dgvhscroll
            // 
            resources.ApplyResources(this.h_dgvhscroll, "h_dgvhscroll");
            this.h_dgvhscroll.LargeChange = 850;
            this.h_dgvhscroll.Maximum = 1020;
            this.h_dgvhscroll.Name = "h_dgvhscroll";
            this.h_dgvhscroll.SmallChange = 170;
            this.h_dgvhscroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.h_dgvhscroll_Scroll);
            this.h_dgvhscroll.ValueChanged += new System.EventHandler(this.h_dgvhscroll_ValueChanged);
            // 
            // btn_dgv
            // 
            resources.ApplyResources(this.btn_dgv, "btn_dgv");
            this.btn_dgv.Name = "btn_dgv";
            this.btn_dgv.UseVisualStyleBackColor = true;
            // 
            // lst_get1
            // 
            this.lst_get1.BackColor = System.Drawing.SystemColors.Control;
            this.lst_get1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.lst_get1, "lst_get1");
            this.lst_get1.FormattingEnabled = true;
            this.lst_get1.Name = "lst_get1";
            // 
            // btn_send
            // 
            resources.ApplyResources(this.btn_send, "btn_send");
            this.btn_send.Name = "btn_send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            this.btn_send.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_send_MouseUp);
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // lbl_col
            // 
            resources.ApplyResources(this.lbl_col, "lbl_col");
            this.lbl_col.Name = "lbl_col";
            // 
            // btn_single
            // 
            resources.ApplyResources(this.btn_single, "btn_single");
            this.btn_single.Name = "btn_single";
            this.btn_single.UseVisualStyleBackColor = true;
            this.btn_single.Click += new System.EventHandler(this.btn_single_Click);
            // 
            // lbl_dgvHscr
            // 
            this.lbl_dgvHscr.BackColor = System.Drawing.Color.Transparent;
            this.lbl_dgvHscr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lbl_dgvHscr, "lbl_dgvHscr");
            this.lbl_dgvHscr.Name = "lbl_dgvHscr";
            // 
            // numUD_bxCs
            // 
            resources.ApplyResources(this.numUD_bxCs, "numUD_bxCs");
            this.numUD_bxCs.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUD_bxCs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUD_bxCs.Name = "numUD_bxCs";
            this.numUD_bxCs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmb_box
            // 
            this.cmb_box.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_box, "cmb_box");
            this.cmb_box.Name = "cmb_box";
            // 
            // numUD_bxRs
            // 
            resources.ApplyResources(this.numUD_bxRs, "numUD_bxRs");
            this.numUD_bxRs.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUD_bxRs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUD_bxRs.Name = "numUD_bxRs";
            this.numUD_bxRs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btn_preview
            // 
            resources.ApplyResources(this.btn_preview, "btn_preview");
            this.btn_preview.Name = "btn_preview";
            this.btn_preview.UseVisualStyleBackColor = true;
            this.btn_preview.Click += new System.EventHandler(this.btn_preview_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // lbl_newRv
            // 
            resources.ApplyResources(this.lbl_newRv, "lbl_newRv");
            this.lbl_newRv.Name = "lbl_newRv";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lst_filepathfull
            // 
            resources.ApplyResources(this.lst_filepathfull, "lst_filepathfull");
            this.lst_filepathfull.FormattingEnabled = true;
            this.lst_filepathfull.Name = "lst_filepathfull";
            // 
            // btn_resGen
            // 
            resources.ApplyResources(this.btn_resGen, "btn_resGen");
            this.btn_resGen.Name = "btn_resGen";
            this.btn_resGen.UseVisualStyleBackColor = true;
            this.btn_resGen.Click += new System.EventHandler(this.btn_resGen_Click);
            // 
            // lbl_newCv
            // 
            resources.ApplyResources(this.lbl_newCv, "lbl_newCv");
            this.lbl_newCv.Name = "lbl_newCv";
            // 
            // lbl_colfrozen
            // 
            resources.ApplyResources(this.lbl_colfrozen, "lbl_colfrozen");
            this.lbl_colfrozen.Name = "lbl_colfrozen";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lbl_cnt
            // 
            resources.ApplyResources(this.lbl_cnt, "lbl_cnt");
            this.lbl_cnt.Name = "lbl_cnt";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // grp_dgvnewpos
            // 
            this.grp_dgvnewpos.Controls.Add(this.button3);
            this.grp_dgvnewpos.Controls.Add(this.label3);
            this.grp_dgvnewpos.Controls.Add(this.label2);
            this.grp_dgvnewpos.Controls.Add(this.lbl_cnt);
            this.grp_dgvnewpos.Controls.Add(this.label1);
            this.grp_dgvnewpos.Controls.Add(this.lbl_colfrozen);
            this.grp_dgvnewpos.Controls.Add(this.lbl_newCv);
            this.grp_dgvnewpos.Controls.Add(this.btn_resGen);
            this.grp_dgvnewpos.Controls.Add(this.lst_filepathfull);
            this.grp_dgvnewpos.Controls.Add(this.button1);
            this.grp_dgvnewpos.Controls.Add(this.lbl_newRv);
            this.grp_dgvnewpos.Controls.Add(this.label9);
            this.grp_dgvnewpos.Controls.Add(this.label7);
            this.grp_dgvnewpos.Controls.Add(this.btn_preview);
            this.grp_dgvnewpos.Controls.Add(this.numUD_bxRs);
            this.grp_dgvnewpos.Controls.Add(this.cmb_box);
            this.grp_dgvnewpos.Controls.Add(this.numUD_bxCs);
            resources.ApplyResources(this.grp_dgvnewpos, "grp_dgvnewpos");
            this.grp_dgvnewpos.Name = "grp_dgvnewpos";
            this.grp_dgvnewpos.TabStop = false;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Name = "panel1";
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Name = "label4";
            // 
            // pnl_dgv
            // 
            resources.ApplyResources(this.pnl_dgv, "pnl_dgv");
            this.pnl_dgv.Name = "pnl_dgv";
            // 
            // pnl_busy
            // 
            resources.ApplyResources(this.pnl_busy, "pnl_busy");
            this.pnl_busy.Name = "pnl_busy";
            // 
            // uc_box
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnl_dgv);
            this.Controls.Add(this.grp_dgvnewpos);
            this.Controls.Add(this.h_dgvhscroll);
            this.Controls.Add(this.lbl_dgvHscr);
            this.Controls.Add(this.btn_single);
            this.Controls.Add(this.lbl_col);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.lst_get1);
            this.Controls.Add(this.btn_dgv);
            this.Controls.Add(this.dgv_box);
            this.Controls.Add(this.pnl_busy);
            this.Name = "uc_box";
            this.Load += new System.EventHandler(this.uc_box_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_bxCs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_bxRs)).EndInit();
            this.grp_dgvnewpos.ResumeLayout(false);
            this.grp_dgvnewpos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.DataGridView dgv_box;
        private System.Windows.Forms.Timer tme_busy;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.HScrollBar h_dgvhscroll;
        private TransparentPanel pnl_busy;
        private System.Windows.Forms.Button btn_dgv;
        private System.Windows.Forms.ListBox lst_get1;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label lbl_col;
        private System.Windows.Forms.Button btn_single;
        public System.Windows.Forms.Label lbl_dgvHscr;
        public System.Windows.Forms.NumericUpDown numUD_bxCs;
        public System.Windows.Forms.ComboBox cmb_box;
        public System.Windows.Forms.NumericUpDown numUD_bxRs;
        private System.Windows.Forms.Button btn_preview;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label lbl_newRv;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lst_filepathfull;
        private System.Windows.Forms.Button btn_resGen;
        public System.Windows.Forms.Label lbl_newCv;
        public System.Windows.Forms.Label lbl_colfrozen;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lbl_cnt;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.GroupBox grp_dgvnewpos;
        private DoubleBufferPanel pnl_dgv;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Label label4;
    }
}
