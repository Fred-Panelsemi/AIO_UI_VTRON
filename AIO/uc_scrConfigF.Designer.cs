namespace AIO
{
    partial class uc_scrConfigF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_scrConfigF));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmb_nvCommList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.p2 = new System.Windows.Forms.Panel();
            this.rbtn_config = new System.Windows.Forms.RadioButton();
            this.dgv_box = new System.Windows.Forms.DataGridView();
            this.lbl_boxCols = new System.Windows.Forms.Label();
            this.numUD_boxCols = new System.Windows.Forms.NumericUpDown();
            this.lbl_boxRows = new System.Windows.Forms.Label();
            this.numUD_boxRows = new System.Windows.Forms.NumericUpDown();
            this.btn_browse = new System.Windows.Forms.Button();
            this.rbtn_load = new System.Windows.Forms.RadioButton();
            this.lbl_resolution = new System.Windows.Forms.Label();
            this.btn_next = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.lbl_amount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_boxCols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_boxRows)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmb_nvCommList);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmb_nvCommList
            // 
            this.cmb_nvCommList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_nvCommList.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_nvCommList, "cmb_nvCommList");
            this.cmb_nvCommList.Name = "cmb_nvCommList";
            this.cmb_nvCommList.SelectedIndexChanged += new System.EventHandler(this.cmb_nvCommList_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.p2);
            this.groupBox2.Controls.Add(this.rbtn_config);
            this.groupBox2.Controls.Add(this.dgv_box);
            this.groupBox2.Controls.Add(this.lbl_boxCols);
            this.groupBox2.Controls.Add(this.numUD_boxCols);
            this.groupBox2.Controls.Add(this.lbl_boxRows);
            this.groupBox2.Controls.Add(this.numUD_boxRows);
            this.groupBox2.Controls.Add(this.btn_browse);
            this.groupBox2.Controls.Add(this.rbtn_load);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Name = "label5";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Name = "label3";
            // 
            // p2
            // 
            resources.ApplyResources(this.p2, "p2");
            this.p2.Name = "p2";
            this.p2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.p2_MouseUp);
            // 
            // rbtn_config
            // 
            resources.ApplyResources(this.rbtn_config, "rbtn_config");
            this.rbtn_config.Checked = true;
            this.rbtn_config.ForeColor = System.Drawing.Color.Blue;
            this.rbtn_config.Name = "rbtn_config";
            this.rbtn_config.TabStop = true;
            this.rbtn_config.UseVisualStyleBackColor = true;
            // 
            // dgv_box
            // 
            this.dgv_box.BackgroundColor = System.Drawing.Color.White;
            this.dgv_box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_box.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_box.ColumnHeadersVisible = false;
            this.dgv_box.GridColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dgv_box, "dgv_box");
            this.dgv_box.Name = "dgv_box";
            this.dgv_box.RowHeadersVisible = false;
            this.dgv_box.RowTemplate.Height = 24;
            this.dgv_box.Tag = "113,85";
            this.dgv_box.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_box_CellClick);
            this.dgv_box.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_box_CellMouseMove);
            this.dgv_box.SelectionChanged += new System.EventHandler(this.dgv_box_SelectionChanged);
            // 
            // lbl_boxCols
            // 
            resources.ApplyResources(this.lbl_boxCols, "lbl_boxCols");
            this.lbl_boxCols.Name = "lbl_boxCols";
            // 
            // numUD_boxCols
            // 
            resources.ApplyResources(this.numUD_boxCols, "numUD_boxCols");
            this.numUD_boxCols.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numUD_boxCols.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUD_boxCols.Name = "numUD_boxCols";
            this.numUD_boxCols.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUD_boxCols.ValueChanged += new System.EventHandler(this.numUD_boxCols_ValueChanged);
            // 
            // lbl_boxRows
            // 
            resources.ApplyResources(this.lbl_boxRows, "lbl_boxRows");
            this.lbl_boxRows.Name = "lbl_boxRows";
            // 
            // numUD_boxRows
            // 
            resources.ApplyResources(this.numUD_boxRows, "numUD_boxRows");
            this.numUD_boxRows.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numUD_boxRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUD_boxRows.Name = "numUD_boxRows";
            this.numUD_boxRows.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUD_boxRows.ValueChanged += new System.EventHandler(this.numUD_boxRows_ValueChanged);
            // 
            // btn_browse
            // 
            resources.ApplyResources(this.btn_browse, "btn_browse");
            this.btn_browse.Name = "btn_browse";
            this.btn_browse.UseVisualStyleBackColor = true;
            // 
            // rbtn_load
            // 
            resources.ApplyResources(this.rbtn_load, "rbtn_load");
            this.rbtn_load.ForeColor = System.Drawing.Color.Blue;
            this.rbtn_load.Name = "rbtn_load";
            this.rbtn_load.UseVisualStyleBackColor = true;
            // 
            // lbl_resolution
            // 
            resources.ApplyResources(this.lbl_resolution, "lbl_resolution");
            this.lbl_resolution.ForeColor = System.Drawing.Color.Blue;
            this.lbl_resolution.Name = "lbl_resolution";
            // 
            // btn_next
            // 
            resources.ApplyResources(this.btn_next, "btn_next");
            this.btn_next.Name = "btn_next";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_close
            // 
            resources.ApplyResources(this.btn_close, "btn_close");
            this.btn_close.Name = "btn_close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lbl_amount
            // 
            resources.ApplyResources(this.lbl_amount, "lbl_amount");
            this.lbl_amount.Name = "lbl_amount";
            this.lbl_amount.DoubleClick += new System.EventHandler(this.dgv_box_SelectionChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // uc_scrConfigF
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.lbl_resolution);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lbl_amount);
            this.Controls.Add(this.groupBox1);
            this.Name = "uc_scrConfigF";
            this.Load += new System.EventHandler(this.uc_scrConfigF_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_boxCols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_boxRows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_nvCommList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.RadioButton rbtn_load;
        private System.Windows.Forms.RadioButton rbtn_config;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.DataGridView dgv_box;
        public System.Windows.Forms.Label lbl_boxCols;
        public System.Windows.Forms.NumericUpDown numUD_boxCols;
        public System.Windows.Forms.Label lbl_boxRows;
        public System.Windows.Forms.NumericUpDown numUD_boxRows;
        public System.Windows.Forms.Label lbl_resolution;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel p2;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label lbl_amount;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label6;
    }
}
