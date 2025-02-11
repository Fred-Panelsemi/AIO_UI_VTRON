using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIO
{
    public partial class i3_Init : Form
    {

        public static Button btn_1 = null;
        public static Button btn_0 = null;
        public static Label lbl_1 = null;
        Label lblcounter = null;

        public static DataGridView dgvformmsg = null;
        public static DataGridView dgvota = null;

        public i3_Init()
        {
            InitializeComponent();
        }

        private void i3_Init_Load(object sender, EventArgs e)
        {
            this.Icon = (System.Drawing.Icon)(Properties.Resources.inf1);
            mvars.FormShow[6] = true;
            if (mvars.FormShow[11])
            {
                this.Size = new Size(1000, 500);
                dgvformmsg = new DataGridView();
                dgvformmsg.BackgroundColor = Control.DefaultBackColor;
                dgvformmsg.Height = Form1.i3init.Height - 60;
                dgvformmsg.Width = Form1.i3init.Width - 30;
                dgvota = new DataGridView();

                lbl_1 = new Label();
                lbl_1.SetBounds(1, 1, 385, 198); lbl_1.Text = ""; lbl_1.Font = new Font("Arial", 9); lbl_1.BackColor = Color.White;
                this.Controls.AddRange(new Control[] { dgvformmsg, dgvota, lblcounter, lbl_1 });

                dgvformmsg.Visible = false;
                dgvota.Visible = false;
                Skeleton_formmsg(2, 888, 1, 1);
                Skeleton_ota(dgvota, 11, 5, 1, 1);
            }
            else
            {
                this.TopMost = true;
                lbl_1 = new Label();
                lbl_1.SetBounds(1, 1, 385, 198); lbl_1.Text = ""; lbl_1.Font = new Font("Arial", 9); lbl_1.BackColor = Color.White;
                btn_1 = new Button();
                btn_0 = new Button();
                btn_1.SetBounds(168, 214, 87, 23); btn_1.Font = new Font("Arial", 9);
                btn_0.SetBounds(275, 214, 87, 23); btn_0.Font = new Font("Arial", 9);
                this.Controls.AddRange(new Control[] { btn_1, btn_0, lbl_1, lblcounter });
                btn_1.Enabled = false; btn_0.Enabled = false; lbl_1.Visible = false;
                btn_1.Click += btn_1_Click;
                btn_0.Click += btn_0_Click;
            }
        }

        private void i3_Init_FormClosed(object sender, FormClosedEventArgs e)
        {
            mvars.FormShow[6] = false;
            if (this.Controls.Contains(btn_1)) { this.Controls.Remove(btn_1); }
            if (this.Controls.Contains(btn_0)) { this.Controls.Remove(btn_0); }
            if (this.Controls.Contains(lbl_1)) { this.Controls.Remove(lbl_1); }
            if (this.Controls.Contains(lblcounter)) { this.Controls.Remove(lblcounter); }
            if (this.Controls.Contains(dgvformmsg)) { this.Controls.Remove(dgvformmsg); }
        }

        private void tme_Warn_Tick(object sender, EventArgs e)
        {
            if (MultiLanguage.DefaultLanguage == "en-US") lbl_1.Text = mvars.msWn + "   " + (mvars.UUT.CoolCounts - mvars.mvWn).ToString() + " sec" + "\r\n" + "\r\n" + " \r\n" + "                                      Start (Y)     or     Exit (N)  ?";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") lbl_1.Text = mvars.msWn + "   " + (mvars.UUT.CoolCounts - mvars.mvWn).ToString() + " 秒" + "\r\n" + "\r\n" + " \r\n" + "                                     開始 (是)     或     取消 (否) ?";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") lbl_1.Text = mvars.msWn + "   " + (mvars.UUT.CoolCounts - mvars.mvWn).ToString() + " 秒" + "\r\n" + "\r\n" + " \r\n" + "                                     开始 (是)     或     取消 (否) ?";

            mvars.mvWn += 1;
            if (mvars.FormShow[5]) { Form2.i3pat.lbl_Mark.Text = new string(' ', mvars.UUT.CoolCounts) + (mvars.UUT.CoolCounts - mvars.mvWn + 1); }
            if (mvars.mvWn >= mvars.UUT.CoolCounts)
            {
                tme_Warn.Enabled = false;
                this.Close();
            }
            else if (mvars.mvWn > 2) { btn_1.Enabled = true; btn_0.Enabled = true; btn_1.Focus(); }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer tme = (Timer)sender;
            if (MultiLanguage.DefaultLanguage == "en-US") lbl_1.Text = mvars.msWn + "   " + (Convert.ToInt16(lblcounter.Text) - mvars.mvWn).ToString() + " sec" + "\r\n" + "\r\n" + " \r\n" + "                                      Start (Y)     or     Exit (N)  ?";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") lbl_1.Text = mvars.msWn + "   " + (Convert.ToInt16(lblcounter.Text) - mvars.mvWn).ToString() + " 秒" + "\r\n" + "\r\n" + " \r\n" + "                                     開始 (是)     或     取消 (否) ?";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") lbl_1.Text = mvars.msWn + "   " + (Convert.ToInt16(lblcounter.Text) - mvars.mvWn).ToString() + " 秒" + "\r\n" + "\r\n" + " \r\n" + "                                     开始 (是)     或     取消 (否) ?";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") lbl_1.Text = mvars.msWn + "   " + (Convert.ToInt16(lblcounter.Text) - mvars.mvWn).ToString() + " 秒" + "\r\n" + "\r\n" + " \r\n" + "                                     开始 (是)     或     取消 (否) ?";
            mvars.mvWn += 1;
            if (mvars.FormShow[5]) { Form2.i3pat.lbl_Mark.Text = new string(' ', Convert.ToInt16(lblcounter.Text)) + (Convert.ToInt16(lblcounter.Text) - mvars.mvWn + 1); }
            if (mvars.mvWn >= Convert.ToInt16(lblcounter.Text))
            {
                tme.Enabled = false;
                this.Close();
            }
            else if (mvars.mvWn > 2) { btn_1.Enabled = true; btn_0.Enabled = true; btn_1.Focus(); }
        }

        private void btn_1_Click(object sender, EventArgs e)
        {
            mvars.msWn = "1"; this.Close(); 
        }

        private void btn_0_Click(object sender, EventArgs e)
        {
            mvars.msWn = "0"; this.Close(); //i3_Init_FormClosed(null, null);
        }


        void Skeleton_formmsg(int SvCols, int SvRows, int SvLBw, int SvLBh)
        {
            int SvR = 0;
            int SvC = 0;    //SvCols=mvars.NovaGamma.Length=9
            dgvformmsg.ReadOnly = true;
            Font f = new Font("Arial", 7);
            dgvformmsg.Font = f;
            //是否允許使用者自行新增
            dgvformmsg.AllowUserToAddRows = false;
            dgvformmsg.AllowUserToResizeRows = false;
            dgvformmsg.AllowUserToResizeColumns = false;
            dgvformmsg.CellBorderStyle = DataGridViewCellBorderStyle.None;

            SvC = 0;
            dgvformmsg.Columns.Add("Col" + SvC.ToString(), "List");
            dgvformmsg.Columns[SvC].Width = 60;
            dgvformmsg.Columns[SvC].SortMode = DataGridViewColumnSortMode.NotSortable;
            //dgvformmsg.ColumnHeadersHeight = 24;
            dgvformmsg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SvC = 1;
            dgvformmsg.Columns.Add("Col" + SvC.ToString(), "Log");
            dgvformmsg.Columns[SvC].Width = 500;
            dgvformmsg.Columns[SvC].SortMode = DataGridViewColumnSortMode.NotSortable;
            //dgvformmsg.ColumnHeadersHeight = 24;
            dgvformmsg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvformmsg.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 7);

            dgvformmsg.ShowCellToolTips = false;

            DataGridViewRowCollection rows = dgvformmsg.Rows;
            for (SvR = 0; SvR < SvRows; SvR++)
            {
                rows.Add();
                DataGridViewRow row = dgvformmsg.Rows[SvR]; row.Height = 18;
            }
            dgvformmsg.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvformmsg.RowHeadersWidth = 72;
            dgvformmsg.RowHeadersDefaultCellStyle.Padding = new Padding(50);
            dgvformmsg.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;//靠左顯示

            SvLBw = 3;
            for (int i = 0; i < dgvformmsg.ColumnCount; i++)
                SvLBw += dgvformmsg.Columns[i].Width;
            //dgvformmsg.Width = SvLBw + 22;
            //dgvformmsg.Width = SvLBw + 122;

            dgvformmsg.Columns[1].Width = dgvformmsg.Width - 60;
            //dgvformmsg.Height = 24 + 20 + 20 + 20 + 20 + 20;
            dgvformmsg.Location = new Point(10, 10);

            //dgvformmsg.Height = Form1.i3init.Height - 60;
            //dgvformmsg.Width = Form1.i3init.Width - 30;
            dgvformmsg.BackgroundColor = Control.DefaultBackColor;

            dgvformmsg.ScrollBars = ScrollBars.Both;
            dgvformmsg.TopLeftHeaderCell.Value = "";
            dgvformmsg.Rows[0].Cells[0].Selected = false;
            
            dgvformmsg.BorderStyle = BorderStyle.None;
            dgvformmsg.RowHeadersVisible = false;
            dgvformmsg.ColumnHeadersVisible = false;
            dgvformmsg.DataSource = null;
            mvars.dgvRows = 0;
        }

        private void Skeleton_ota(DataGridView _dgv, int SvCols, int SvRows, int SvLBw, int SvLBh)
        {
            int SvR = 0;
            int SvC = 0;    //SvCols=mvars.NovaGamma.Length=9
            _dgv.ReadOnly = true;
            Font f = new Font("Arial", 7);
            _dgv.Font = f;
            //是否允許使用者自行新增
            _dgv.AllowUserToAddRows = false;
            _dgv.AllowUserToResizeRows = false;
            _dgv.AllowUserToResizeColumns = false;
            _dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;

            for (SvC = 0; SvC < SvCols; SvC++)
            {
                _dgv.Columns.Add("Col" + SvC.ToString(), SvC.ToString());
                _dgv.Columns[SvC].Width = 30;
                _dgv.Columns[SvC].SortMode = DataGridViewColumnSortMode.NotSortable;
                _dgv.ColumnHeadersHeight = 24;
                _dgv.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            _dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 7);

            _dgv.ShowCellToolTips = false;

            DataGridViewRowCollection rows = _dgv.Rows;
            for (SvR = 0; SvR < SvRows; SvR++)
            {
                rows.Add();
                DataGridViewRow row = _dgv.Rows[SvR]; row.Height = 18;
            }
            _dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //_dgv.RowHeadersWidth = 72;
            _dgv.RowHeadersDefaultCellStyle.Padding = new Padding(50);
            _dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;//靠左顯示

            SvLBw = 3;
            for (int i = 0; i < _dgv.ColumnCount; i++)
                SvLBw += _dgv.Columns[i].Width;
            //_dgv.Width = dgv_formmsg.Width;
            _dgv.Height = 24 + 20 + 20;
            //_dgv.Visible = true;
           // _dgv.Location = new Point(dgv_formmsg.Left, dgv_formmsg.Top + dgv_formmsg.Height + 10);
            //ScrollBar
            _dgv.ScrollBars = ScrollBars.Both;
            _dgv.TopLeftHeaderCell.Value = "";
            _dgv.Rows[0].Cells[0].Selected = false;
        }

    }
}
