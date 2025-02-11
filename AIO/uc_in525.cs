using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace AIO
{
    public partial class uc_in525 : UserControl
    {
        public static ComboBox cmbdeviceid = null;
        public static ComboBox cmbhPb = null;
        public static DataGridView dgvBin = null;
        public static Button btnW = null;
        public static Button btnR = null;
        public static Button btnL = null;
        public static Button btnS = null;
        public static Label lblRchecksum = null;
        public static Label lblLchecksum = null;
        public static CheckBox chkMTP = null;
        public static TextBox txt183 = null;
        public static byte[] gBinArr = new byte[0x4C];

        public uc_in525()
        {
            InitializeComponent();
            cmbdeviceid = cmb_deviceID;
            cmbdeviceid.Items.Clear();
            for (int i = 0; i < Form1.cmbdeviceid.Items.Count; i++)
            {
                cmbdeviceid.Items.Add(Form1.cmbdeviceid.Items[i].ToString());
            }

            if (mvars.deviceID == "0300" && cmbdeviceid.FindString(" XB01") != -1) { int j = cmbdeviceid.FindString(" XB01"); cmbdeviceid.Text = cmbdeviceid.Items[j].ToString(); }


            dgvBin = dgv_Bin;
            btnL = btn_Load;
            btnR = btn_Read;
            btnW = btn_Write;
            btnS = btn_Save;
            lblRchecksum = lbl_Readchecksum;
            lblLchecksum = lbl_Loadchecksum;
            chkMTP = chk_MTP;
            cmb_BinDes.LostFocus += new EventHandler(cmb_BinDes_LostFocus);
            cmb_BinVal.LostFocus += new EventHandler(cmb_BinVal_LostFocus);
            cmb_BinVal.GotFocus += new EventHandler(cmb_BinVal_GotFocus);
            cmb_BinVol.LostFocus += new EventHandler(cmb_BinVol_LostFocus);
            txt183 = textBox183;
            cmbhPb = cmb_hPB;
        }

        public static UInt16 CalChecksumIndex(byte[] arr, UInt32 IndexStart, UInt32 IndexEnd)
        {
            UInt32 Checksum = 0;
            UInt32 i;
            for (i = IndexStart; i <= IndexEnd; i++)
            {
                Checksum += (UInt32)(arr[i]);
                if (Checksum > 65535)
                {
                    Checksum %= 65536;
                }
            }
            return (UInt16)Checksum;
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
            r1 = dgvBin.GetCellDisplayRectangle(-1, -1, true);
            //r1.X += 1;
            r1.Y += 1;
            r1.Width --;
            r1.Height -= 2;
            e.Graphics.FillRectangle(new SolidBrush(DefaultBackColor), r1);
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            e.Graphics.DrawString(". b i n", new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.White), r1, format);
        }




        private void uc_in525_Load(object sender, EventArgs e)
        {
            Skeleton_Bin(16, 5, 480, 270, "525");
            for (int SvR = 0; SvR < mvars.in525df.Length; SvR++)
            {
                dgvBin.Rows[SvR / 16].Cells[SvR % 16].Value = mvars.in525df[0, SvR];
            }
            lbl_BinDes.Text = "Click ! Register Select";
            cmb_BinDes.Items.Clear(); cmb_BinDes.Items.AddRange(mvars.in525exp); cmb_BinDes.Left = lbl_BinDes.Left - 1; cmb_BinDes.Top = lbl_BinDes.Top - 3; cmb_BinDes.Width = 202;
            //lbl_BinHex.Visible = btnW.Visible; lbl_BinHex.Left = lbl_BinDes.Left + 1; lbl_BinHex.Top = lbl_BinDes.Top + 20;
            //lbl_BinVal.Visible = btnW.Visible; lbl_BinVal.Left = lbl_BinHex.Left + lbl_BinHex.Width + 5; lbl_BinVal.Top = lbl_BinDes.Top + 20;
            cmb_BinVal.Items.Clear(); cmb_BinVal.Left = lbl_BinVal.Left - 1; cmb_BinVal.Top = lbl_BinVal.Top - 3; cmb_BinVal.Width = 52;
            lbl_BinDes.Text = "Click ! Register Select"; lbl_BinVal.Text = ""; lbl_BinHex.Text = ""; lbl_BinVol.Text = "";
            //lbl_BinVol.Left = lbl_BinVal.Left + cmb_BinVal.Width + 3; lbl_BinVol.Top = lbl_BinDes.Top + 20;
            cmb_BinVol.Items.Clear(); cmb_BinVol.Left = lbl_BinVol.Left - 1; cmb_BinVol.Top = lbl_BinVol.Top - 3; cmb_BinVol.Width = 66;

            if (mvars.UUT.MTP == 1) { chkMTP.Checked = true; } else { chkMTP.Checked = false; }
            cmbhPb.Location = cmb_LANpos.Location;
            cmbhPb.Size = cmb_LANpos.Size;
            cmbhPb.BringToFront();
            cmbhPb.Items.Clear();
            for (int svi = 0; svi < Form1.cmbhPB.Items.Count; svi++)
            {
                cmbhPb.Items.Add(Form1.cmbhPB.Items[svi].ToString());
            }
            cmbhPb.Text = cmbhPb.Items[0].ToString();
        }

        private void cmb_deviceID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1.cmbdeviceid.SelectedIndex = cmb_deviceID.SelectedIndex;
        }

        #region change describe
        private void cmb_BinDes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape) { cmb_BinDes.Visible = false; }
        }
        void cmb_BinDes_LostFocus(object sender, EventArgs e)
        {
            cmb_BinDes.Tag = false;
            int index = mp.HexToDec(cmb_BinDes.Text.Substring(0, 2));
            cmb_BinVal.Items.Clear();
            float svi = Convert.ToSingle(lbl_BinVal.Text);
            switch (index)
            {
                case 2:
                    cmb_BinVol.Items.Clear();
                    for (float i = 0; i <= 15; i++)
                    {
                        cmb_BinVol.Items.Add(" " + i + " ms");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 3:
                    cmb_BinVol.Items.Clear();
                    for (float i = 0; i <= 15; i++)
                    {
                        cmb_BinVol.Items.Add(" " + (i + 1) + " ms");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 4:
                    cmb_BinVol.Items.Clear();
                    cmb_BinVol.Items.Add(" Disable");
                    for (float i = 1; i <= 15; i++)
                    {
                        cmb_BinVol.Items.Add(" " + (i * 100));
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 6:
                    cmb_BinVol.Items.Clear();
                    for (float i = 0; i <= 3; i++)
                    {
                        cmb_BinVol.Items.Add(" " + (i * 200 + 200) + "ms");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 7:
                    cmb_BinVol.Items.Clear();
                    cmb_BinVol.Items.Add(" Disable");
                    for (float i = 1; i <= 7; i++)
                    {
                        cmb_BinVol.Items.Add(" " + (i * 200 + 400) + "ms");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:    //0Ah  "Boost Output Voltage";
                    cmb_BinVol.Items.Clear();
                    for (float i = 135; i <= 198; i++)
                    {
                        cmb_BinVol.Items.Add(" " + (i / 10).ToString("##0.0") + " v");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 16:    //10h  "BK1 Output Voltage";
                    cmb_BinVol.Items.Clear();
                    for (float i = 22; i <= 37; i++)
                    {
                        cmb_BinVol.Items.Add(" " + (i / 10).ToString("##0.0") + " v");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 17:    //11h  "BK2 Output Voltage";
                    cmb_BinVol.Items.Clear();
                    for (float i = 80; i <= 235; i += 5)
                    {
                        cmb_BinVol.Items.Add(" " + (i / 100).ToString("##0.00") + " v");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 18:    //12h  "BK3 Output Voltage";
                    cmb_BinVol.Items.Clear();
                    for (float i = 80; i <= 235; i += 5)
                    {
                        cmb_BinVol.Items.Add(" " + (i / 100).ToString("##0.0") + " v");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 19:    //13h  "BK4 Output Voltage";
                    cmb_BinVol.Items.Clear();
                    for (float i = 48; i <= 111; i++)
                    {
                        cmb_BinVol.Items.Add(" " + (i / 10).ToString("##0.0") + " v");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
                case 33:    //21h  "VGL Output Voltage";
                    cmb_BinVol.Items.Clear();
                    for (float i = -30; i >= -205; i -= 5)
                    {
                        cmb_BinVol.Items.Add(" " + (i / 10).ToString("##0.0") + " v");
                    }
                    cmb_BinVol.Text = cmb_BinVol.Items[Convert.ToInt32(svi)].ToString();
                    break;
            }
            lbl_BinDes.Visible = true;
            cmb_BinDes.Visible = false;
        }
        private void cmb_BinDes_MouseLeave(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ComboBox) && ((ComboBox)sender).Parent != null) { ((ComboBox)sender).Parent.Focus(); }
        }
        private void cmb_BinDes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_BinDes.Text = cmb_BinDes.Items[cmb_BinDes.SelectedIndex].ToString();
            int index = mp.HexToDec(cmb_BinDes.Text.Substring(0, 2));
            lbl_BinHex.Text = index.ToString();
            lbl_BinVal.Text = mp.HexToDec((string)dgvBin.Rows[index / 16].Cells[(index % 16)].Value).ToString();
            float svi = Convert.ToSingle(lbl_BinVal.Text);
            lbl_BinVol.Visible = false;
            switch (index)
            {
                case 9:    //09h
                    break;
                case 10:    //0Ah  "Boost Output Voltage";
                    lbl_BinVol.Text = ((135 + svi) / 10).ToString() + " v";
                    lbl_BinVol.Visible = true;
                    break;
                case 16:    //10h  "BK1 Output Voltage";
                    lbl_BinVol.Text = ((22 + svi) / 10).ToString() + " v";
                    lbl_BinVol.Visible = true;
                    break;
                case 17:    //11h  "BK2 Output Voltage";
                    lbl_BinVol.Text = ((80 + svi * 5) / 100).ToString() + " v";
                    lbl_BinVol.Visible = true;
                    break;
                case 18:    //12h  "BK3 Output Voltage";
                    lbl_BinVol.Text = ((80 + svi * 5) / 10).ToString() + " v";
                    lbl_BinVol.Visible = true;
                    break;
                case 19:    //13h  "BK4 Output Voltage";
                    lbl_BinVol.Text = ((48 + svi * 5) / 10).ToString() + " v";
                    lbl_BinVol.Visible = true;
                    break;
                case 33:    //21h  "VGL Output Voltage";
                    lbl_BinVol.Text = ((-30 - svi * 5) / 10).ToString() + " v";
                    lbl_BinVol.Visible = true;
                    break;
                //case 57:    //39h
                //    int svbinI = mp.HexToDec(dgvBin.Rows[(0x39) / 16].Cells[(0x39) % 16].Value.ToString());
                //    string svbinS = mp.DecToBin(svbinI, 8);
                //    chk_StatusB7.Checked = false; if (svbinS.Substring(0, 1) == "1") { chk_StatusB7.Checked = true; }
                //    chk_StatusB5.Checked = false; if (svbinS.Substring(2, 1) == "1") { chk_StatusB5.Checked = true; }
                //    chk_StatusB4.Checked = false; if (svbinS.Substring(3, 1) == "1") { chk_StatusB4.Checked = true; }
                //    chk_StatusB3.Checked = false; if (svbinS.Substring(4, 1) == "1") { chk_StatusB3.Checked = true; }
                //    chk_StatusB2.Checked = false; if (svbinS.Substring(5, 1) == "1") { chk_StatusB2.Checked = true; }
                //    chk_StatusB1.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB1.Checked = true; }
                //    chk_StatusB0.Checked = false; if (svbinS.Substring(7, 1) == "1") { chk_StatusB0.Checked = true; }
                //    break;
            }
        }
        private void lbl_BinDes_Click(object sender, EventArgs e)
        {
            lbl_BinDes.Visible = false;
            if (lbl_BinDes.Text.IndexOf("Click") != -1) cmb_BinDes.Text = cmb_BinDes.Items[0].ToString();
            cmb_BinDes.Visible = true;
            cmb_BinDes.Focus();
        }
        
        
        
        #endregion change describe


        #region change value
        private void cmb_BinVal_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_BinVal.Text = cmb_BinVal.Text;
        }
        private void cmb_BinVal_MouseLeave(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ComboBox) && ((ComboBox)sender).Parent != null) { ((ComboBox)sender).Parent.Focus(); }
        }
        private void cmb_BinVal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                if (mp.IsNumeric(Convert.ToInt32(cmb_BinVal.Text)) == true && Convert.ToInt32(cmb_BinVal.Text) >= Convert.ToInt32(cmb_BinVal.Items[0]) && Convert.ToInt32(cmb_BinVal.Text) <= Convert.ToInt32(cmb_BinVal.Items[1])) { cmb_BinVal.Visible = false; }
            }
            else if (e.KeyCode == Keys.Escape) { cmb_BinVal.Visible = false; }
        }
        void cmb_BinVal_LostFocus(object sender, EventArgs e)
        {

        }
        void cmb_BinVal_GotFocus(object sender, EventArgs e)
        {
            cmb_BinVal.SelectionStart = 0;
            cmb_BinVal.SelectionLength = cmb_BinVal.Text.Length;
        }
        #endregion change value



        #region change Voltage
        private void lbl_BinVol_Click(object sender, EventArgs e)
        {
            lbl_BinVol.Visible = false;
            cmb_BinVol.Text = lbl_BinVol.Text;
            cmb_BinVol.Left = lbl_BinVol.Left - 1;
            cmb_BinVol.Visible = true;
            cmb_BinVol.Focus();
        }
        private void cmb_BinVol_MouseLeave(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ComboBox) && ((ComboBox)sender).Parent != null) { ((ComboBox)sender).Parent.Focus(); }
        }
        private void cmb_BinVol_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_BinVol.Text = cmb_BinVol.Text.Trim();
            int svi = cmb_BinVol.SelectedIndex;
            int index = mp.HexToDec(lbl_BinDes.Text.Substring(0, 2));
            lbl_BinHex.Text = "0x" + mp.DecToHex(svi, 2) + " =";
            lbl_BinVal.Text = svi.ToString();
            dgvBin.Rows[index / 16].Cells[index % 16].Value = mp.DecToHex(svi, 2);
        }
        void cmb_BinVol_LostFocus(object sender, EventArgs e)
        {
            lbl_BinVol.Visible = true;
            cmb_BinVol.Visible = false;
        }
        private void cmb_BinVol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                if (mp.IsNumeric(Convert.ToSingle(cmb_BinVol.Text.Trim().Substring(0, cmb_BinVol.Text.Trim().Length - 2))) == true) { cmb_BinVol.Visible = false; }
            }
            else if (e.KeyCode == Keys.Escape) { cmb_BinVol.Visible = false; }
        }
        #endregion change Voltage



        #region IN525 UnLock
        public static void in525UnlockWP1(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;                   //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x6A;
            mvars.RS485_WriteDataBuffer[10 + svns] = 0x95;
            mp.funSendMessageTo();
        }
        public static void in525UnlockWP2(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x28;                   //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0xC9;
            mvars.RS485_WriteDataBuffer[10 + svns] = 0x36;
            mp.funSendMessageTo();
        }
        public static void in525UnlockWP3(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x38;                   //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0xA3;
            mvars.RS485_WriteDataBuffer[10 + svns] = 0x5C;
            mp.funSendMessageTo();
        }
        public static void in525UnlockWP4(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x42;                   //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x99;
            mvars.RS485_WriteDataBuffer[10 + svns] = 0x66;
            mp.funSendMessageTo();
        }
        public static bool Unlock_in525()
        {
            bool svDone = true;
            //UNLOCK +4
            mvars.lblCmd = "IN525_UNLOCKWP1";
            in525UnlockWP1(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svDone = false; }
            mvars.lblCmd = "IN525_UNLOCKWP2";
            in525UnlockWP2(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svDone = false; }
            mvars.lblCmd = "IN525_UNLOCKWP3";
            in525UnlockWP3(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svDone = false; }
            mvars.lblCmd = "IN525_UNLOCKWP4";
            in525UnlockWP4(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svDone = false; }
            return svDone;            
        }
        #endregion IN525 UnLock



        public static void in525STARST(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x02;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x49;                   //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x80;
            mp.funSendMessageTo();
        }



        #region IN525 Write ALL
        public static void in525WrALL1(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0D + 0x26);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D + 0x26;            //Size 0x02~0x27
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x26 + 1;               //Write Size Data+Reg
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x02;                   //Register 0x02~0x27
            for (uint i = 0x02; i <= 0x27; i++)
                mvars.RS485_WriteDataBuffer[9 + 1 + (i - 0x02)] = uc_in525.gBinArr[i];
            mp.funSendMessageTo();
        }
        public static void in525WrALL2(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0D + 0x0E);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D + 0x0E;                                     //Size 0x2A ~ 0x37
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x0E + 1;                                        //Write Size Data+Reg
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x2A;                                            //Register 0x2A ~ 0x37
            for (uint i = 0x2A; i <= 0x37; i++)
                mvars.RS485_WriteDataBuffer[9 + 1 + (i - 0x2A)] = uc_in525.gBinArr[i];
            mp.funSendMessageTo();
        }
        public static void in525WrALL3(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0D + 0x08);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D + 0x08;                                     //Size 0x3A ~ 0x41
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x08 + 1;                                        //Write Size Data+Reg
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x3A;                                            //Register 0x3A ~ 0x41
            for (uint i = 0x3A; i <= 0x41; i++)
                mvars.RS485_WriteDataBuffer[9 + 1 + (i - 0x3A)] = uc_in525.gBinArr[i];
            mp.funSendMessageTo();
        }
        public static void in525WrALL4(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0D + 0x02);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D + 0x02;                                     //Size 0x44 ~ 0x45
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x02 + 1;                                        //Write Size Data+Reg
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x44;                                            //Register 0x44 ~ 0x45
            for (uint i = 0x44; i <= 0x45; i++)
                mvars.RS485_WriteDataBuffer[9 + 1 + (i - 0x44)] = uc_in525.gBinArr[i];
            mp.funSendMessageTo();
        }
        public static bool WrALL_in525()
        {
            bool svDone = true;
            //UNLOCK +4
            mvars.lblCmd = "IN525_WR1";
            in525WrALL1(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svDone = false; }
            mvars.lblCmd = "IN525_WR2";
            in525WrALL2(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svDone = false; }
            mvars.lblCmd = "IN525_WR3";
            in525WrALL3(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svDone = false; }
            mvars.lblCmd = "IN525_WR4";
            in525WrALL4(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svDone = false; }

            UInt16 checksum = 0;
            checksum += CalChecksumIndex(gBinArr, 0x02, 0x27);
            checksum += CalChecksumIndex(gBinArr, 0x2A, 0x37);
            checksum += CalChecksumIndex(gBinArr, 0x3A, 0x41);
            checksum += CalChecksumIndex(gBinArr, 0x44, 0x45);
            checksum += CalChecksumIndex(gBinArr, 0x46, 0x4B);
            lblRchecksum.Text = "W：0x" + checksum.ToString("X4");

            return svDone;
        }
        #endregion IN525 Write ALL



        public static void in525MTP(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x02;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x46;                   //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x80;                   //0x80 DAC to MTP
            mp.funSendMessageTo();
        }

        public static void in525SetFlag(byte svWRAddr)    //最新重新定義
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x02;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x48;                   //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x80;                   //0x80
            mp.funSendMessageTo();
        }


        private void btn_Write_Click(object sender, EventArgs e)
        {
            mp.markreset(99, false);
            string sverr = "0";
            mvars.flgSelf = true;


            for (int i = 0; i < gBinArr.Length; i++)
                gBinArr[i] = Convert.ToByte((string)dgvBin.Rows[i / 16].Cells[i % 16].Value, 16);


            if (Unlock_in525() == false) { sverr = "-1"; goto Err; }

            mp.doDelayms(60);
            mvars.lblCmd = "IN525_STARST";
            in525STARST(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-2"; goto Err; }
            mp.doDelayms(60);
            //+1
            mvars.lblCmd = "IN516_WRITE_ALL";
            if (WrALL_in525() == false) { sverr = "-3"; goto Err; }
            if (chkMTP.Checked)
            {//+3
                mvars.lblCmd = "IN525_STARST";
                in525STARST(Convert.ToByte(uc_in525.txt183.Text, 16));
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-4"; goto Err; }

                mvars.lblCmd = "IN525_MTP";
                in525MTP(Convert.ToByte(uc_in525.txt183.Text, 16));
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-5"; goto Err; }
                mp.doDelayms(60);

                mvars.lblCmd = "IN516_SETFLAG";
                in525SetFlag(Convert.ToByte(uc_in525.txt183.Text, 16));
                mp.doDelayms(60);
               
                //mvars.lblCmd = "IN516_WPRST"; 
                //mp.mhin516WriteSingleReg(false, 0x20, 0x3A, 0x02, 0x80); 
            }


        Err:
            mvars.lstget.Items.Add("ErrCode " + sverr);
        }


        public static void in525DACRead(byte svWRAddr)    //最新版定義方法
        {
            #region Novastar Setup
            byte svns = 2;  
            mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x02;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x47;                   //Register 0x47
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x00;                   //0x00  DAC Mode
            mp.funSendMessageTo();
        }
        public static void in525MTPRead(byte svWRAddr)    //最新重新定義
        {
            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x02;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x47;                   //Register 0x47
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x01;                   //0x01 MTP Mode
            mp.funSendMessageTo();
        }
        public static void in525ReadALL(byte svWRAddr)    //最新重新定義
        {
            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWrRd;    //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;                   //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x00;                   //Read Size
            mvars.RS485_WriteDataBuffer[10 + svns] = 0x4C;                  //Read Size (0x00~0x4B)
            mp.funSendMessageTo();
        }

        private void btn_Read_Click(object sender, EventArgs e)
        {
            mp.markreset(99, false);
            string sverr = "0";
            mvars.flgSelf = true;


            if (chkMTP.Checked)
            {
                mvars.lblCmd = "IN525_MTPREAD";
                in525MTPRead(Convert.ToByte(uc_in525.txt183.Text, 16));
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-1"; goto Err; }
            }
            else
            {
                mvars.lblCmd = "IN525_DACREAD";
                in525DACRead(Convert.ToByte(uc_in525.txt183.Text, 16));
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-2"; goto Err; }
            }

            mvars.lblCmd = "IN525_READALL";
            in525ReadALL(Convert.ToByte(uc_in525.txt183.Text, 16));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-3"; goto Err; }

            UInt16 checksum = 0;
            checksum += CalChecksumIndex(gBinArr, 0x02, 0x27);
            checksum += CalChecksumIndex(gBinArr, 0x2A, 0x37);
            checksum += CalChecksumIndex(gBinArr, 0x3A, 0x41);
            checksum += CalChecksumIndex(gBinArr, 0x44, 0x45);
            checksum += CalChecksumIndex(gBinArr, 0x46, 0x4B);
            lbl_Readchecksum.Text = "0x" + checksum.ToString("X4");
            string path = mvars.strLogPath + @"\IN525_Read_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".bin";
            File.WriteAllBytes(path, gBinArr);

            for (int i = 0; i < 0x4C; i++)
            {
                dgvBin.Rows[(i / 16)].Cells[(i % 16)].Value = "";
            }

            mp.doDelayms(300);

            for (int i = 0; i < 0x4C; i++)
            {
                dgvBin.Rows[(i / 16)].Cells[(i % 16)].Value = mp.DecToHex(gBinArr[i], 2);
            }

        Err:
            mvars.lstget.Items.Add("ErrCode " + sverr);




            
        }

        private void chk_MTP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMTP.Checked == true) { mvars.UUT.MTP = 1; }
            else { mvars.UUT.MTP = 0; }
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select file",
                InitialDirectory = ".\\",
                Filter = "Bin files (*.*)|*.bin"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                byte[] BinArr;
                BinArr = File.ReadAllBytes(dialog.FileName);
                for (UInt16 i = 0; i < BinArr.Length; i++)
                    gBinArr[i] = BinArr[i];

                //for (int i = 0; i < gBinArr.Length; i++)
                //    dgvBin.Rows[(i / 16)].Cells[(i % 16)].Value = Byte2HexString(gBinArr[i]);

                UInt16 checksum = 0;
                checksum += CalChecksumIndex(gBinArr, 0x02, 0x27);
                checksum += CalChecksumIndex(gBinArr, 0x2A, 0x37);
                checksum += CalChecksumIndex(gBinArr, 0x3A, 0x41);
                checksum += CalChecksumIndex(gBinArr, 0x44, 0x45);
                checksum += CalChecksumIndex(gBinArr, 0x46, 0x4B);
                lbl_Loadchecksum.Text = "0x" + checksum.ToString("X4");
                mvars.lstget.Items.Add("IN525 bin load OK，CheckSum " + lbl_Loadchecksum.Text);
                for (int i = 0; i < gBinArr.Length; i++)
                {
                    dgvBin.Rows[(i / 16)].Cells[(i % 16)].Value = mp.Byte2HexString(gBinArr[i]);
                }
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //byte[] BinArr = new byte[0x46];
            for (int i = 0; i < gBinArr.Length; i++)
            {
                gBinArr[i] = Convert.ToByte((string)dgvBin.Rows[(i / 16)].Cells[(i % 16)].Value, 16);
            }
                
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Bin files (*.*)|*.bin",
                Title = "Save bin file"
            };
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                File.WriteAllBytes(saveFileDialog1.FileName, gBinArr);
                UInt16 checksum = 0;
                checksum += CalChecksumIndex(gBinArr, 0x02, 0x27);
                checksum += CalChecksumIndex(gBinArr, 0x2A, 0x37);
                checksum += CalChecksumIndex(gBinArr, 0x3A, 0x41);
                checksum += CalChecksumIndex(gBinArr, 0x44, 0x45);
                checksum += CalChecksumIndex(gBinArr, 0x46, 0x4B);
                lbl_Loadchecksum.Text = "0x" + checksum.ToString("X4");
                mvars.lstget.Items.Add("IN525 bin Saved，CheckSum " + lbl_Loadchecksum.Text);

            }
        }

        private void btn_pwicinfo_Click(object sender, EventArgs e)
        {
            mp.markreset(99, false);
            string sverr = "0";
            mvars.flgSelf = true;

            mvars.lblCmd = "PWIC_INFO";
            mp.mhPWICINFO(txt183.Text);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-1"; goto Err; }
            MessageBox.Show(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1], mvars.strUInameMe + mvars.UImajor);
            return;
        Err:
            MessageBox.Show("ErrCode " + sverr + "," + mvars.lGet[mvars.lCount - 1], mvars.strUInameMe + mvars.UImajor);
        }

        private void cmb_hPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1.cmbhPB.SelectedIndex = cmbhPb.SelectedIndex;
        }
    }
}
