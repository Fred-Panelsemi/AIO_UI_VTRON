using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIO
{
    public partial class uc_bin : UserControl
    {
        public static DataGridView dgvBin = null;
        public static Button dgvBtn = null;

        public uc_bin()
        {
            InitializeComponent();

            dgvBin = dgv_Bin;
            dgvBin.SetBounds(3, 44, 639, 180);
            Skeleton_Bin(16, 8, 480, 270, "603");

            //dgvBtn.SetBounds(2, 2, dgvBin.RowHeadersWidth - 1, dgvBin.ColumnHeadersHeight - 1);
            //dgvBtn.Text = mvars.cm603Addr_c12a[0, 0] + " ct0，0";  //{ { "R","X" }, { "G","B" } };    // [Addrctrl,A0] 預設 [0,0] = R
            //dgvBtn.ForeColor = Color.Red;
            //dgvBtn.Click += new EventHandler(btn_Bin_Click);
        }

        private void uc_bin_Load(object sender, EventArgs e)
        {
            cmb_BinDes.Items.AddRange(mvars.cm603exp);
            cmb_BinVal.Items.Add("0");
            cmb_BinVal.Items.Add("1023");
            if (mvars.BINtype.ToUpper() == "IN525")
            {
                Skeleton_Bin(16, 4, 480, 270, "525");
            }
        }


        private void Skeleton_Bin(int SvCols, int SvRows, int SvLBw, int SvLBh, string svIC)
        {
            int SvR = 0;
            int SvC = 0;
            dgvBin.ReadOnly = true;
            dgvBin.Font = new Font("Arial", 7);
            //是否允許使用者自行新增
            dgvBin.AllowUserToAddRows = false;
            dgvBin.AllowUserToResizeRows = false;
            dgvBin.AllowUserToResizeColumns = false;
            for (SvC = 0; SvC < SvCols; SvC++)
            {
                dgvBin.Columns.Add("Col" + (SvC).ToString(), mvars.binhead[SvC].ToString()); dgvBin.Columns[(SvC)].Width = 35; dgvBin.Columns[(SvC)].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvBin.ColumnHeadersHeight = 23;
                dgvBin.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvBin.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            //
            dgvBin.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvBin.ShowCellToolTips = false;
            //
            DataGridViewRowCollection rows = dgvBin.Rows;
            for (SvR = 0; SvR < SvRows; SvR++)
            {
                rows.Add();
                dgvBin.Rows[SvR].DefaultCellStyle.WrapMode = DataGridViewTriState.True; DataGridViewRow row = dgvBin.Rows[SvR]; row.Height = 19;
                for (SvC = 0; SvC < SvCols; SvC++)
                {
                    dgvBin.Rows[SvR].Cells[(SvC)].Style.BackColor = Color.FromArgb(255, 192, 128);
                }
            }
            dgvBin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBin.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            //dgvBin.Paint += new PaintEventHandler(dgvcm603_RowHeadPaint);
            dgvBin.RowHeadersWidth = 65;
            dgvBin.RowHeadersDefaultCellStyle.Padding = new Padding(50);
            dgvBin.Paint += new PaintEventHandler(dgvBin_RowPostPaint);
            dgvBin.Paint += new PaintEventHandler(dgvBin_CornerPaint);
            //ScrollBar
            dgvBin.ScrollBars = ScrollBars.Both;
            dgvBin.TopLeftHeaderCell.Value = "";
            //
            dgvBin.Rows[0].Cells[0].Selected = false;
        }
        void dgvBin_RowPostPaint(object sender, PaintEventArgs e)
        {
            Rectangle r1_1;
            Rectangle r1;
            StringFormat format = new StringFormat();
            for (int j = 0; j < dgvBin.Rows.Count; j++)
            {
                r1 = dgvBin.GetCellDisplayRectangle(-1, j, true); //get the column header cell
                r1.X += 1;
                r1.Y += 1;
                r1_1 = r1;
                r1_1.Width -= 2;
                r1_1.Height -= 2;
                e.Graphics.FillRectangle(new SolidBrush(dgvBin.ColumnHeadersDefaultCellStyle.BackColor), r1_1);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString("0x" + (j).ToString("00"),
                dgvBin.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgvBin.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
        }
        private void dgvBin_CornerPaint(object sender, PaintEventArgs e)
        {
            Rectangle r1;
            StringFormat format = new StringFormat();
            r1 = dgvBin.GetCellDisplayRectangle(-1, -1, true);
            //r1.X += 1;
            r1.Y += 1;
            r1.Width = r1.Width - 1;
            r1.Height = r1.Height - 2;
            e.Graphics.FillRectangle(new SolidBrush(DefaultBackColor), r1);
            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            //e.Graphics.DrawString("Bin", new Font("Arial", 8, FontStyle.Bold), new SolidBrush(dgvBin.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            e.Graphics.DrawString(". b i n", new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.White), r1, format);
        }

        private void btn_hRead_Click(object sender, EventArgs e)
        {

        }

        private void btn_hSave_Click(object sender, EventArgs e)
        {

        }

        private void lbl_BinDes_Click(object sender, EventArgs e)
        {
            lbl_BinDes.Visible = false;
            if (lbl_BinDes.Text.IndexOf("Click") != -1) cmb_BinDes.Text = cmb_BinDes.Items[0].ToString();
            cmb_BinDes.Visible = true;
            cmb_BinDes.Focus();
        }
    }
}
