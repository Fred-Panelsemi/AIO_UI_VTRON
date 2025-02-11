using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Nova.Mars.SDK;
using System.IO;

namespace AIO
{
    public partial class uc_cm603 : UserControl
    {
        public static DataGridView dgvBin = null;
        public static Button dgvBtn = new Button();
        public static ListBox lstget1 = null;
        public static Bitmap bmpf = null;
        public string[] svf = null;
        public static ComboBox cmbhPB = null;

        //int Xst = 0;
        //int Yst = 0;
        //int Xw = 480;
        //int Yw = 540;
        //int W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        //int H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;



        public uc_cm603()
        {
            InitializeComponent();
            cmbhPB = cmb_hPB;
            cmbhPB.Items.Clear();
            for (int i = 0; i < Form1.cmbhPB.Items.Count; i++)
            {
                cmbhPB.Items.Add(Form1.cmbhPB.Items[i].ToString());
            }
            cmbhPB.Text = cmbhPB.Items[0].ToString();

            cmb_deviceID.Items.Clear ();
            for (int i = 0; i < Form1.cmbdeviceid.Items.Count; i++)
            {
                cmb_deviceID.Items.Add(Form1.cmbdeviceid.Items[i].ToString());
            }
            cmb_deviceID.Text = Form1.cmbdeviceid.Items[0].ToString();

            dgvBin = dgv_Bin;
            lstget1 = lst_get;

            cmb_BinDes.LostFocus += new EventHandler(cmb_BinDes_LostFocus);
            cmb_BinVal.LostFocus += new EventHandler(cmb_BinVal_LostFocus);
            cmb_BinVal.GotFocus += new EventHandler(cmb_BinVal_GotFocus);
            cmb_BinVol.LostFocus += new EventHandler(cmb_BinVol_LostFocus);

            //mvars.pGMA.Data = new string[3, 18]{
            //    {"00","00","00","03","00","0E","00","10","00","1E","00","22","00","2E","00","38","00","61"},
            //    {"00","00","00","03","00","0E","00","10","00","1E","00","23","00","2E","00","3B","00","60"},
            //    {"00","00","00","03","00","0E","00","10","00","1E","00","24","00","2E","00","3B","00","62"} };
            Array.Clear(mvars.pGMA[0].Data, 0, mvars.pGMA[0].Data.Length);
            if (mvars.pGMA.Length == 2) Array.Clear(mvars.pGMA[1].Data, 0, mvars.pGMA[1].Data.Length);
            ///
        }
        private void uc_cm603_Load(object sender, EventArgs e)
        {
            dgvBin = dgv_Bin;
            dgvBin.Rows.Clear();
            dgvBin.Columns.Clear();
            Skeleton_Bin(16, 8, 480, 270, "603");



            //dgvBin.Controls.Add(dgvBtn);
            //dgvBtn.Left = 2;
            //dgvBtn.Top = 2;
            //dgvBtn.Height = dgvBin.ColumnHeadersHeight - 1;
            //dgvBtn.Width = dgvBin.RowHeadersWidth - 1;
            //dgvBtn.Font = new Font("Arial", 9, FontStyle.Bold);
            //dgvBtn.Text = "R ct0,0";  //{ { "R","M" }, { "G","B" } };    // [Addrctrl,A0] 預設 [0,0] = R
            //Form1.cmbCM603.Text = mvars.cm603Addr_c12a[0, 0];
            //dgvBtn.ForeColor = Color.Red;
            //dgvBtn.Click += new EventHandler(btn_Bin_Click);

            dgvBtn = btn_dgv;
            dgvBtn.Left = dgvBin.Left+1;
            dgvBtn.Top = dgvBin.Top+1;
            dgvBtn.Height = dgvBin.ColumnHeadersHeight - 1;
            dgvBtn.Width = dgvBin.RowHeadersWidth - 1;
            dgvBtn.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvBtn.Text = "R ct0,0";  //{ { "R","M" }, { "G","B" } };    // [Addrctrl,A0] 預設 [0,0] = R
            grp_DeviceAddr.Text = "Device Address   W:" + mp.DecToHex(mvars.cm603WA0Ctrl[0, 0], 2) + " R:" + mp.DecToHex(mvars.cm603RA0Ctrl[0, 0], 2);
            Form1.cmbCM603.Text = mvars.cm603Addr_c12a[0, 0];
            dgvBtn.ForeColor = Color.Red;

            /// 暫時未使用
            if (mvars.svnova)
            {
                #region NovaStar LAN position
                label1.Visible = true;
                cmb_LANpos.Visible = true;
                chk_LANpos.Visible = true;
                //chk_LANpos.Enabled = false;
                //cmb_LANpos.Items.Clear();
                ////mk4
                //if (Form1.nvHWcards > 0)
                //{
                //    List<LEDScreenInfo> screenInfoList = null;
                //    OperateResult result = mvars._marsCtrlSystem.ReadLEDScreenInfo(out screenInfoList);
                //    if (result == OperateResult.OK)
                //    {
                //        if (uc_cm603.bmpf != null) { uc_cm603.bmpf.Dispose(); }
                //        if (mvars.FormShow[5]) { Form2.i3pat.Dispose(); }

                //        mp.checkExtendScreen(W, H);
                //        if (mp.upperBound == 0)
                //        {
                //            uc_coding.lstget1.Items.RemoveAt(uc_coding.lstget1.Items.Count - 1);
                //            uc_coding.lstget1.Items.Add("No Extend Screen");
                //            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;

                //            string svs = "BREAK ?" + "\r\n" + "\r\n" + "No Extend Screen";

                //            mp.killMSGname = "FLASH UPDATE";
                //            mp.killMSGsec = 3;
                //            mp.KillMessageBoxStart();
                //            if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }

                //            //ShowCursor(0);
                //        }
                //        else
                //        {
                //            Graphics g3 = null;
                //            bmpf = new Bitmap(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                //            //g3 = Form2.i3pat.CreateGraphics();
                //            g3 = Graphics.FromImage(bmpf);
                //            g3.Clear(Color.Black);
                //            Pen p3 = new Pen(Color.White, 1);
                //            SolidBrush b3 = new SolidBrush(Color.White);
                //            Font f3 = new Font("Arial", 15);

                //            markRESET();
                //            for (int i = 0; i < Form1.nvHWcards; i++)
                //            {
                //                mvars.iSender = Form1.NovaStarDeviceResult[i].SenderIndex;
                //                mvars.iPort = Form1.NovaStarDeviceResult[i].PortIndex;
                //                mvars.iScan = Form1.NovaStarDeviceResult[i].ScannerIndex;
                //                Form1.numUDSender.Value = mvars.iSender + 1;
                //                Form1.numUDPort.Value = mvars.iPort + 1;
                //                Form1.numUDScan.Value = mvars.iScan + 1;

                //                for (int j = 0; j < screenInfoList[0].ScanBoardInfoList.Count; j++)
                //                {
                //                    if (screenInfoList[0].ScanBoardInfoList[j].SenderIndex == mvars.iSender && screenInfoList[0].ScanBoardInfoList[j].PortIndex == mvars.iPort && screenInfoList[0].ScanBoardInfoList[j].ConnectIndex == mvars.iScan)
                //                    {
                //                        Xst = screenInfoList[0].ScanBoardInfoList[j].X;
                //                        Yst = screenInfoList[0].ScanBoardInfoList[j].Y;
                //                        Xw = screenInfoList[0].ScanBoardInfoList[j].Width;
                //                        Yw = screenInfoList[0].ScanBoardInfoList[j].Height;

                //                        g3.DrawRectangle(p3, Xst, Yst, Xw - 1, Yw - 1);
                //                        g3.DrawString("Sender" + Form1.numUDSender.Value + " OUT" + Form1.numUDPort.Value + " Receiver" + Form1.numUDScan.Value, f3, b3, Xst, Yst);
                //                        g3.DrawString("Sender" + Form1.numUDSender.Value + " OUT" + Form1.numUDPort.Value + " Receiver" + Form1.numUDScan.Value, f3, b3, Xst, Yst + Yw / 2);

                //                        cmb_LANpos.Items.Add("Sender" + Form1.numUDSender.Value + "~OUT" + Form1.numUDPort.Value + "~Receiver" + Form1.numUDScan.Value);
                //                    }
                //                }
                //            }

                //            g3.Dispose();
                //            f3.Dispose();
                //            p3.Dispose();
                //            b3.Dispose();

                //            chk_LANpos.Enabled = true;
                //        }
                //        svf = new string[cmb_LANpos.Items.Count];
                //        for (int svc = 0; svc < cmb_LANpos.Items.Count; svc++)
                //        {
                //            svf[svc] = cmb_LANpos.Items[svc].ToString();
                //        }
                //    }
                //}
                #endregion NovaStar LAN position
            }

            lbl_cm603Loadchecksum.Text = "Load";
            lbl_cm603Readchecksum.Text = "Read";

            //if (mvars.deviceID.Substring(0, 2) == "03")
            //{
            //    cmb_deviceID.Visible = false;
            //    mvars.mCM603("1");
            //}

            cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString();

            lbl_BinDes.Text = "Click ! Register Select";
            cmb_BinDes.Items.Clear(); cmb_BinDes.Items.AddRange(mvars.cm603exp); cmb_BinDes.Left = lbl_BinDes.Left - 1; cmb_BinDes.Top = lbl_BinDes.Top - 3; cmb_BinDes.Width = 202;
            cmb_BinVal.Items.Add("0");
            cmb_BinVal.Items.Add("1023");

            /// 顯示R
            for (int SvR = 0; SvR < 8; SvR++)
            {
                for (int SvC = 0; SvC < 16; SvC++)
                {
                    dgvBin.Rows[SvR].Cells[SvC].Value = mvars.cm603df[0, SvC + SvR * 16];
                }
            }
            mvars.actFunc = "cm603";
        }
        void markRESET()
        {
            mvars.strReceive = "";
            mvars.lCounts = 9999;
            mvars.lCount = 0;
            Array.Resize(ref mvars.lCmd, mvars.lCounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, mvars.lCounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.lCount = 0;
            mvars.errCode = "0";
            mvars.flgDelFB = true;
            mvars.Break = false;
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

            dgvBin.Rows[0].Cells[0].Selected = false;
        }
        private void dgvBin_CornerPaint(object sender, PaintEventArgs e)
        {
            Rectangle r1;
            //StringFormat format = new StringFormat();
            r1 = dgvBin.GetCellDisplayRectangle(-1, -1, true);
            //r1.X += 1;
            r1.Y += 1;
            r1.Width--;
            r1.Height-= 2;
            e.Graphics.FillRectangle(new SolidBrush(DefaultBackColor), r1);
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            //e.Graphics.DrawString("Bin", new Font("Arial", 8, FontStyle.Bold), new SolidBrush(dgvBin.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            e.Graphics.DrawString(". b i n", new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.White), r1, format);
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
                e.Graphics.DrawString("0x" + (j * 10).ToString("00"),
                dgvBin.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgvBin.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
        }
        private void btn_Bin_Click(object sender, EventArgs e)
        {
            int svi = 0;
            if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04" || mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")
            {
                if (mvars.ICver < 5)
                {
                    if (dgvBtn.Text.Substring(0, 1) == "R") { dgvBtn.Text = "G ct0,1"; svi = 1; dgvBtn.ForeColor = Color.DarkGreen; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                    else if (dgvBtn.Text.Substring(0, 1) == "G") { dgvBtn.Text = "B ct1,1"; svi = 2; dgvBtn.ForeColor = Color.Blue; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                    else if (dgvBtn.Text.Substring(0, 1) == "B") { dgvBtn.Text = "R ct0,0"; svi = 0; dgvBtn.ForeColor = Color.Magenta; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                }
                if (mvars.ICver >= 5)
                {
                    if (dgvBtn.Text.Substring(0, 1) == "R") { dgvBtn.Text = "G ct0,1"; svi = 1; dgvBtn.ForeColor = Color.DarkGreen; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                    else if (dgvBtn.Text.Substring(0, 1) == "G") { dgvBtn.Text = "B ct1,1"; svi = 2; dgvBtn.ForeColor = Color.Blue; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                    else if (dgvBtn.Text.Substring(0, 1) == "B") { dgvBtn.Text = "M ct1,0"; svi = 3; dgvBtn.ForeColor = Color.Magenta; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                    else if (dgvBtn.Text.Substring(0, 1) == "M") { dgvBtn.Text = "R ct0,0"; svi = 0; dgvBtn.ForeColor = Color.Red; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "03")     //H5512A
            {
                mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1);
                if (dgvBtn.Text.Substring(0, 1) == "R")
                {
                    dgvBtn.Text = "G ct0,1"; svi = 1; dgvBtn.ForeColor = Color.DarkGreen; cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString();
                    mvars.cm603WRaddr = 214; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2);
                }
                else if (dgvBtn.Text.Substring(0, 1) == "G")
                {
                    dgvBtn.Text = "B ct0,0"; svi = 2; dgvBtn.ForeColor = Color.Blue; cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString();
                    mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2);
                }
                else if (dgvBtn.Text.Substring(0, 1) == "B")
                {
                    dgvBtn.Text = "R ct0,0"; svi = 0; dgvBtn.ForeColor = Color.Red; cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString();
                    mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1);
                }
                mvars.deviceID = "03" + string.Format("{0:00}", (byte)((mvars.iPBaddr - 1) / 2 + 1));
            }
            Form1.cmbCM603.Text = dgvBtn.Text.Substring(0, 1);

            lbl_BinDes.Text = "Click ! Register Select";
            lbl_BinVal.Text = "";
            lbl_BinMsg.Text = "";
            lbl_BinHex.Text = "";
            lbl_BinVol.Text = "";
            if (lbl_cm603Readchecksum.Text.Length > 0 && lbl_cm603Readchecksum.Text.Substring(0, 2) != "0x") { lbl_cm603Readchecksum.Text = "checksum"; }
            if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")
            {
                if (dgvBtn.Text.Substring(0, 1) == "R" || dgvBtn.Text.Substring(0, 1) == "G" || dgvBtn.Text.Substring(0, 1) == "B")
                {
                    for (int SvR = 0; SvR < dgvBin.Rows.Count; SvR++)
                    {
                        for (int SvC = 0; SvC < dgvBin.ColumnCount; SvC++)
                        {
                            dgvBin.Rows[SvR].Cells[SvC].Value = mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR];
                            mvars.cm603dfB[svi, SvC + dgvBin.ColumnCount * SvR] = (byte)mp.HexToDec(mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR]);
                        }
                    }
                    byte[] BinArr = new byte[128];
                    Buffer.BlockCopy(mvars.cm603dfB, BinArr.Length * svi, BinArr, 0, BinArr.Length);
                    for (int i = 0; i < 4; i++) BinArr[i] = 0;
                    for (int i = 0x1C; i < 0x20; i++) BinArr[i] = 0;
                    for (int i = 0x20; i < 0x24; i++) BinArr[i] = 0;
                    for (int i = 0x26; i < 0x30; i++) BinArr[i] = 0;
                    for (int i = 0x34; i < 0x3E; i++) BinArr[i] = 0;
                    for (int i = 0x58; i < 0x60; i++) BinArr[i] = 0;
                    for (int i = 0x66; i < 0x7C; i++) BinArr[i] = 0;
                    lbl_cm603Readchecksum.Text = "0x" + mp.DecToHex((int)mp.CalCheckSum(BinArr, 0, BinArr.Length), 4);
                    //int svbinI = mvars.cm603dfB[svi, (0x3E) * 2] + mvars.cm603dfB[svi, (0x3E) * 2 + 1];
                    //string svbinS = mp.DecToBin(svbinI, 16);
                    //svbinS = svbinS.Substring(svbinS.Length - 1, 1);
                    //cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[Convert.ToInt32(svbinS)].ToString();   //mk4

                    mvars.cm603WRaddr = mvars.cm603WA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())];

                    int svbinI = mvars.cm603dfB[svi, (0x3F) * 2] + mvars.cm603dfB[svi, (0x3F) * 2 + 1];
                    string svbinS = mp.DecToBin(svbinI, 16);
                    chk_StatusB9.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB9.Checked = true; }
                    chk_StatusB7.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB7.Checked = true; }
                    chk_StatusB4.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB4.Checked = true; }
                    chk_StatusB3.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB3.Checked = true; }
                    chk_StatusB2.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB2.Checked = true; }
                    chk_StatusB1.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB1.Checked = true; }
                    chk_StatusB0.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB0.Checked = true; }
                }
            }
            else
            {
                if (dgvBtn.Text.Substring(0, 1) == "R" || dgvBtn.Text.Substring(0, 1) == "G" || dgvBtn.Text.Substring(0, 1) == "B")
                {
                    for (int SvR = 0; SvR < dgvBin.Rows.Count; SvR++)
                    {
                        for (int SvC = 0; SvC < dgvBin.ColumnCount; SvC++)
                        {
                            dgvBin.Rows[SvR].Cells[SvC].Value = mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR];
                            mvars.cm603dfB[svi, SvC + dgvBin.ColumnCount * SvR] = (byte)mp.HexToDec(mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR]);
                        }
                    }
                }
            }


        }




        private void cmb_BinDes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape) { cmb_BinDes.Visible = false; }
        }
        private void cmb_BinDes_LostFocus(object sender, EventArgs e)
        {
            cmb_BinDes.Tag = false;
            int index = mp.HexToDec(cmb_BinDes.Text.Substring(0, 2));
            lbl_BinMsg.Text = "0x" + mp.DecToHex((index * 2), 2) + " && " + "0x" + mp.DecToHex((index * 2) + 1, 2);
            cmb_BinVal.Items.Clear();
            if (index == 0x3E) { cmb_BinVal.Items.Add("2"); cmb_BinVal.Items.Add("3"); }
            else if (index == 0x3F) { cmb_BinVal.Items.Add("0"); cmb_BinVal.Items.Add("640"); } //繼續
            else { cmb_BinVal.Items.Add("0"); cmb_BinVal.Items.Add("1023"); }
            //從紀錄值取出
            //int svi = 0;
            //if (dgvBtn.Text.Substring(0, 1) == "R") svi = 0;
            //else if (dgvBtn.Text.Substring(0, 1) == "G") svi = 1;
            //else if (dgvBtn.Text.Substring(0, 1) == "B") svi = 2;
            //int svbinI = mvars.cm603dfB[svi, index * 2] * 256 + mvars.cm603dfB[svi, index * 2 + 1];
            //從表格取出
            int svbinI = mp.HexToDec((string)dgvBin.Rows[(index * 2) / 16].Cells[((index * 2) % 16)].Value + (string)dgvBin.Rows[(index * 2) / 16].Cells[((index * 2) % 16 + 1)].Value);
            string svbinS = mp.DecToBin(svbinI, 16);
            lbl_BinVal.Text = mp.BinToDec(svbinS.Substring(6, 10)).ToString();
            string svCRCcheck = svbinS.Substring(2, 4).ToString();  //在sheet"AIO DB"
            lbl_BinHex.Text = "0x" + mp.BinToHex(svbinS.Substring(2, 14), 4) + " = 00 " + svCRCcheck + " " + svbinS.Substring(6, 10) + " (0x" + mp.BinToHex(svbinS.Substring(6, 10), 4) + ") =";
            lbl_BinVal.Left = lbl_BinHex.Left + lbl_BinHex.Width + 5;
            lbl_BinVol.Left = lbl_BinVal.Left + cmb_BinVal.Width + 3;
            if (mvars.cm603exp[index].IndexOf("VREF", 0) != -1)
            {
                lbl_BinVol.Text = (0.01953 * Convert.ToInt32(lbl_BinVal.Text)).ToString("##0.0###");

                //txt_CM603Vref1.Text = (1023 - vsc_CM603Vref.Value).ToString();
                //txt_CM603Vref0.Text = (Convert.ToInt32(txt_CM603Vref1.Text) * 0.01953).ToString("0.####");
                //txt_CM603BK1v000.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g000.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v001.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g001.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v002.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g002.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v003.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g003.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v004.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g004.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v005.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g005.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v006.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g006.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v007.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g007.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v008.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g008.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v009.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g009.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v010.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g010.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v011.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g011.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v012.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g012.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v013.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g013.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
                //txt_CM603BK1v014.Text = (0.01953 * Convert.ToInt32(txt_CM603BK1g014.Text) * Convert.ToInt32(txt_CM603Vref1.Text) / 1024).ToString("0.####");
            }
            else
            {
                svbinI = mp.HexToDec((string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16)].Value + (string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16 + 1)].Value);
                svbinS = mp.DecToBin(svbinI, 16);
                lbl_BinVol.Text = ((0.01953 * mp.BinToDec(svbinS.Substring(6, 10)) * Convert.ToInt32(lbl_BinVal.Text)) / 1024).ToString("##0.0###");
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
        }




        private void cmb_BinVal_GotFocus(object sender, EventArgs e)
        {
            cmb_BinVal.SelectionStart = 0;
            cmb_BinVal.SelectionLength = cmb_BinVal.Text.Length;
        }
        private void cmb_BinVal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                if (mp.IsNumeric(Convert.ToInt32(cmb_BinVal.Text)) == true && Convert.ToInt32(cmb_BinVal.Text) >= Convert.ToInt32(cmb_BinVal.Items[0]) && Convert.ToInt32(cmb_BinVal.Text) <= Convert.ToInt32(cmb_BinVal.Items[1])) { cmb_BinVal.Visible = false; }
            }
            else if (e.KeyCode == Keys.Escape) { cmb_BinVal.Visible = false; }
        }
        private void cmb_BinVal_LostFocus(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmb_BinVal.Text) >= Convert.ToInt32(cmb_BinVal.Items[0]) &&
                Convert.ToInt32(cmb_BinVal.Text) <= Convert.ToInt32(cmb_BinVal.Items[1]))
            {
                int svi = 0;
                if (dgvBtn.Text.Substring(0, 1) == "R") svi = 0;
                else if (dgvBtn.Text.Substring(0, 1) == "G") svi = 1;
                else if (dgvBtn.Text.Substring(0, 1) == "B") svi = 2;
                //
                int index = mp.HexToDec(lbl_BinDes.Text.Substring(0, 2));
                lbl_BinVal.Text = cmb_BinVal.Text;
                cmb_BinDes.Tag = false;
                UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(cmb_BinVal.Text), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                string svCRCcheck = (mp.DecToBin((int)ulCRC, 16).Substring(2, 4));
                lbl_BinHex.Text = "0x" + mp.BinToHex(svCRCcheck + mp.DecToBin(Convert.ToInt32(lbl_BinVal.Text), 10), 4) + " = 00 " + svCRCcheck + " " + mp.DecToBin(Convert.ToInt32(lbl_BinVal.Text), 10) + " (0x" + mp.BinToHex(mp.DecToBin(Convert.ToInt32(lbl_BinVal.Text), 10), 4) + ") =";
                if (mvars.cm603exp[index].IndexOf("VREF", 0) != -1)
                {
                    lbl_BinVol.Text = (0.01953 * Convert.ToInt32(lbl_BinVal.Text)).ToString("##0.0###");
                }
                else
                {
                    int svbinI = mp.HexToDec((string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16)].Value + (string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16 + 1)].Value);
                    string svbinS = mp.DecToBin(svbinI, 16);
                    lbl_BinVol.Text = ((0.01953 * mp.BinToDec(svbinS.Substring(6, 10)) * Convert.ToInt32(lbl_BinVal.Text)) / 1024).ToString("##0.0###");
                }
                dgvBin.Rows[(index * 2) / 16].Cells[((index * 2) % 16)].Value = (mp.BinToHex(svCRCcheck + mp.DecToBin(Convert.ToInt32(lbl_BinVal.Text), 10), 4)).Substring(0, 2);
                dgvBin.Rows[(index * 2) / 16].Cells[((index * 2) % 16 + 1)].Value = (mp.BinToHex(svCRCcheck + mp.DecToBin(Convert.ToInt32(lbl_BinVal.Text), 10), 4)).Substring(2, 2);
                //
                mvars.RegData[svi, index * 2] = dgvBin.Rows[(index * 2) / 16].Cells[((index * 2) % 16)].Value.ToString();
                mvars.RegData[svi, index * 2 + 1] = dgvBin.Rows[(index * 2) / 16].Cells[((index * 2) % 16) + 1].Value.ToString();
                //修改的內容到WriteDone後再更新到cm603dfB

                lbl_BinVal.Visible = true;
                cmb_BinVal.Visible = false;
            }
        }
        private void cmb_BinVal_MouseLeave(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ComboBox) && ((ComboBox)sender).Parent != null) { ((ComboBox)sender).Parent.Focus(); }
        }
        private void cmb_BinVal_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_BinVal.Text = cmb_BinVal.Text;
        }
        private void lbl_BinVal_Click(object sender, EventArgs e)
        {
            int index = mp.HexToDec(cmb_BinDes.Text.Substring(0, 2));
            if (mvars.cm603exp[index].IndexOf("Reserved", 0) == -1 && mvars.cm603exp[index].IndexOf("CMI", 0) == -1 && mvars.cm603exp[index].IndexOf("MTP", 0) == -1 && index <= 0x3F)
            {
                lbl_BinVal.Visible = false;
                //if (lbl_BinVal.Text.IndexOf("Click") != -1) cmb_BinVal.Text = lbl_BinVal.Text;
                cmb_BinVal.Text = lbl_BinVal.Text;
                cmb_BinVal.Left = lbl_BinVal.Left - 1;
                cmb_BinVal.Text = lbl_BinVal.Text.Trim();
                cmb_BinVal.Visible = true;
                cmb_BinVal.Focus();
            }
        }




        private void cmb_BinVol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                if (mp.IsNumeric(Convert.ToSingle(cmb_BinVol.Text.Trim().Substring(0, cmb_BinVol.Text.Trim().Length - 2))) == true) { cmb_BinVol.Visible = false; }
            }
            else if (e.KeyCode == Keys.Escape) { cmb_BinVol.Visible = false; }
        }
        private void cmb_BinVol_LostFocus(object sender, EventArgs e)
        {
            lbl_BinVol.Visible = true;
            cmb_BinVol.Visible = false;
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
            //cmb_BinVol.Visible = false;
            dgvBin.Rows[index / 16].Cells[index % 16].Value = mp.DecToHex(svi, 2);
        }



        
        



        private void cmb_DeviceAddrCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            grp_DeviceAddr.Text = "Device Address   W:" + mp.DecToHex(mvars.cm603WA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())], 2) +
                " R:" + mp.DecToHex(mvars.cm603RA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())], 2);
        }

        private void cmb_DeviceAddrA0_SelectedIndexChanged(object sender, EventArgs e)
        {
            grp_DeviceAddr.Text = "Device Address   W:" + mp.DecToHex(mvars.cm603WA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())], 2) +
                " R:" + mp.DecToHex(mvars.cm603RA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())], 2);
        }

        private void chk_LANpos_CheckedChanged(object sender, EventArgs e)
        {
            if (mvars.svnova)
            {
                //if (mvars.FormShow[5]) { Form2.i3pat.Dispose(); }
                //if (chk_LANpos.Checked)
                //{
                //    Form2.i3pat = new i3_Pat();
                //    Form2.i3pat.Location = new Point(mvars.UUT.resX, mvars.UUT.resY);
                //    Form2.i3pat.Size = new Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                //    Form2.i3pat.BackColor = Color.Black;
                //    Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                //    Form2.i3pat.pic_Mark.Location = new Point(0, 0);
                //    Form2.i3pat.pic_Mark.Size = new Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                //    Form2.i3pat.pic_Mark.Visible = true;
                //    Form2.i3pat.Show();
                //    Form2.i3pat.TopMost = true;

                //    Form2.i3pat.pic_Mark.Image = bmpf;
                //    Form2.i3pat.pic_Mark.Refresh();
                //}
            }
        }

        private void btn_cm603Save_Click(object sender, EventArgs e)
        {
            //string svfilepath = mvars.strStartUpPath + "\\Parameter\\temp\\" + svf[cmb_LANpos.SelectedIndex].ToString() + "\\";
            //if (!Directory.Exists(svfilepath)) { MessageBox.Show(svfilepath + "\r\n" + "not exist"); return; }

            //svfilepath = mvars.strStartUpPath + "\\Parameter\\gammaIC\\" + svf[cmb_LANpos.SelectedIndex].ToString() + "\\";
            //if (Directory.Exists(svfilepath)) { Form1.DeleteFolder(svfilepath); }
            //Directory.CreateDirectory(svfilepath);

            //Form1.CopyDir(mvars.strStartUpPath + "\\Parameter\\temp\\" + svf[cmb_LANpos.SelectedIndex].ToString() + "\\", mvars.strStartUpPath + "\\Parameter\\gammaIC\\" + svf[cmb_LANpos.SelectedIndex].ToString() + "\\");

            //svfilepath = mvars.strStartUpPath + "\\Parameter\\temp\\" + svf[cmb_LANpos.SelectedIndex].ToString() + "\\";
            //if (Directory.Exists(svfilepath)) { Form1.DeleteFolder(svfilepath); }
            //lstget1.Items.Add("gamma bin files Saved");
            //lstget1.Items.Add("Cabinet gamma bin files @ " + svfilepath);
            //lstget1.Items.Add("");
            //lstget1.TopIndex = lstget1.Items.Count - 1;


            byte[] BinArr = new byte[128];
            //Array.Clear(mvars.cm603df, 0, mvars.cm603df.Length);
            int svi = 0;
            SaveFileDialog dialog = new SaveFileDialog();
            if (dgvBtn.Text.Substring(0, 1) == "R") { svi = 0; dialog.Title = "Save CM603(" + dgvBtn.Text.Substring(0, 1) + ") bin file"; }
            else if (dgvBtn.Text.Substring(0, 1) == "G") { svi = 1; dialog.Title = "Save CM603(" + dgvBtn.Text.Substring(0, 1) + ") bin file"; }
            else if (dgvBtn.Text.Substring(0, 1) == "B") { svi = 2; dialog.Title = "Save CM603(" + dgvBtn.Text.Substring(0, 1) + ") bin file"; }
            else if (dgvBtn.Text.Substring(0, 1) == "M") { svi = 3; dialog.Title = "Save CM603(" + dgvBtn.Text.Substring(0, 1) + ") bin file"; }

            dialog.InitialDirectory = mvars.strStartUpPath + @"\Parameter";
            dialog.FileName = "";
            dialog.Filter = "Bin files |*.bin";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < 128; i++)
                {
                    //BinArr[i] = Convert.ToByte((string)dgvBin.Rows[(i / 16) + 1].Cells[(i % 16) + 1].Value, 16);
                    mvars.cm603df[svi, i] = (string)dgvBin.Rows[i / 16].Cells[i % 16].Value;

                }




                //參考frmCM603後的修改 00h 01h都為0
                mvars.cm603df[svi, 0 * 2] = "00";
                mvars.cm603df[svi, 0 * 2 + 1] = "00";
                mvars.cm603dfB[svi, 0 * 2] = 0;
                mvars.cm603dfB[svi, 0 * 2 + 1] = 0;
                mvars.cm603df[svi, 1 * 2] = "00";
                mvars.cm603df[svi, 1 * 2 + 1] = "00";
                mvars.cm603dfB[svi, 1 * 2] = 0;
                mvars.cm603dfB[svi, 1 * 2 + 1] = 0;
                
                for (int index = 2; index < mvars.cm603exp.Length; index++)
                {
                    //if (mvars.cm603exp[index].IndexOf("Reserved", 0) != -1 ||
                    //        mvars.cm603exp[index].IndexOf("CMI", 0) != -1 ||
                    //        mvars.cm603exp[index].IndexOf("Status", 0) != -1 ||
                    //        mvars.cm603exp[index].IndexOf("MTP", 0) != -1)
                    //{
                    //    mvars.cm603df[svi, index * 2] = "00";
                    //    mvars.cm603df[svi, index * 2 + 1] = "00";
                    //}
                    //else
                    //{
                        mvars.cm603df[svi, index * 2] = (string)dgvBin.Rows[(index * 2) / 16].Cells[((index * 2) % 16)].Value;
                        mvars.cm603df[svi, index * 2 + 1] = (string)dgvBin.Rows[(index * 2) / 16].Cells[((index * 2) % 16 + 1)].Value;
                    //}
                    mvars.cm603dfB[svi, index * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, index * 2]);
                    mvars.cm603dfB[svi, index * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, index * 2 + 1]);
                }
                Buffer.BlockCopy(mvars.cm603dfB, BinArr.Length * svi, BinArr, 0, BinArr.Length);
                File.WriteAllBytes(dialog.FileName, BinArr);
                Array.Copy(mvars.cm603df, mvars.RegData, mvars.cm603df.Length);    //新cm603 bin 模式

                //pGMA.Data = new string[3, 18]{
                //{"00","00","00","03","00","0E","00","23","00","43","00","BC","00","CA","00","D8","00","E6"},
                //{"00","00","00","03","00","0E","00","23","00","43","00","6E","00","A4","00","DB","00","33"},
                //{"00","00","00","03","00","0E","00","23","00","43","00","6E","00","A4","00","DB","00","33"} };

                //for (int index = 0x04; index <= 0x0C; index++)
                //{
                //    int svbinI = mvars.cm603dfB[svi, index * 2] * 256 + mvars.cm603dfB[svi, index * 2 + 1];
                //    string svbinS = mp.DecToBin(svbinI, 16);
                //    svbinI = mp.BinToDec(svbinS.Substring(6, 10));
                //    mvars.pGMA.Data[svi, (index - 0x04) * 2] = mp.DecToHex(svbinI / 256, 2);
                //    mvars.pGMA.Data[svi, (index - 0x04) * 2 + 1] = mp.DecToHex(svbinI % 256, 2);
                //}
            }
        }
        private void btn_cm603Read_Click(object sender, EventArgs e)
        {
            lstget1.Items.Clear();
            lst_hpb.Items.Clear();
            //dgvBtn.Text = mvars.cm603Addr_c12a[0, 0] + " ct0，0";

            btn_cm603Load.Enabled = false;
            btn_cm603Read.Enabled = false;
            btn_cm603Save.Enabled = false;
            btn_cm603Write.Enabled = false;
            //return;

            string svfilepath = "";
            if (mvars.svnova)
            {
                #region NovaStar LAN position
                svfilepath = mvars.strStartUpPath + "\\Parameter\\temp\\" + svf[cmb_LANpos.SelectedIndex].ToString() + "\\";
                if (Directory.Exists(svfilepath)) { Form1.DeleteFolder(svfilepath); }
                Directory.CreateDirectory(svfilepath);
                #endregion NovaStar LAN position
            }

            mp.markreset(99, false);

            //int svi = 0;
            mvars.lblCmd = "CM603_READ";
            if (mvars.demoMode)
            {
                MessageBox.Show("AddressCtrl=" + cmb_DeviceAddrCtrl.Text + "，A0=" + cmb_DeviceAddrA0.Text, " @ Demo mode");
            }
            else
            {
                byte svc = 0;
                if (cmb_DeviceAddrA0.Enabled == false && cmb_DeviceAddrCtrl.Enabled == false)
                {
                    if (mvars.deviceID.Substring(0, 2) == "02" || 
                        mvars.deviceID.Substring(0, 2) == "04" || 
                        mvars.deviceID.Substring(0, 2) == "05" || 
                        mvars.deviceID.Substring(0, 2) == "06" ||
                        mvars.deviceID.Substring(0, 2) == "10")     
                    {
                        if (dgvBtn.Text.Substring(0, 1) == "R") { svc = 0; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "G") { svc = 1; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "B") { svc = 2; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "M") { svc = 3; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                    }
                    else if (mvars.deviceID.Substring(0, 2) == "03")     //H5512A
                    {
                        if (dgvBtn.Text.Substring(0, 1) == "R") { svc = 0; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "G") { svc = 1; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "B") { svc = 2; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                    }
                }
                else
                {
                    if (Form1.cmbhPB.Text.IndexOf("_1", 0) == -1 && grp_DeviceAddr.Text == "Device Address   W:D8 R:D9")
                    {
                        MessageBox.Show("AddressCtrl=" + cmb_DeviceAddrCtrl.Text + "，A0=" + cmb_DeviceAddrA0.Text + " @ " + Form1.cmbhPB.Text.Trim().Substring(0, "1-1".Length) + "_1", "XB CM603 Read");
                        btn_cm603Read.Enabled = true; return;
                    }
                }
                mvars.cm603WRaddr = mvars.cm603RA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())];
                mp.mhcm603ReadAll(mvars.cm603WRaddr);
                byte[] BinArr = new byte[mvars.cm603df.GetLength(1)];     /// find ".Length / 3" change to "GetLength(1)" [3,128] GetLength(1)=128
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { lstget1.Items.Add(Form1.cmbhPB.Text + "," + mvars.msrColor[svc + 2].Substring(0, 1) + ",READ,ERROR,1"); }
                else
                {
                    if (svc == 3)
                    {
                        lstget1.Items.Add(Form1.cmbhPB.Text + ",cm603(M),READ,DONE,1");
                        lst_hpb.Items.Add(Form1.cmbhPB.Text + ",cm603(M),checked");
                    }
                    else
                    {
                        lstget1.Items.Add(Form1.cmbhPB.Text + "," + mvars.msrColor[svc + 2].Substring(0, 1) + ",READ,DONE,1");
                        lst_hpb.Items.Add(Form1.cmbhPB.Text + "," + mvars.msrColor[svc + 2].Substring(0, 1) + ",checked");
                    }

                    Array.Copy(mvars.RegData, svc * mvars.RegData.GetLength(1), mvars.cm603df, svc * mvars.RegData.GetLength(1), mvars.RegData.GetLength(1));
                    Array.Clear(BinArr, 0, BinArr.Length);
                    for (int svj = 0; svj < mvars.cm603df.GetLength(1); svj++)  /// 128
                    {
                        mvars.cm603dfB[svc, svj] = (byte)mp.HexToDec(mvars.cm603df[svc, svj]);
                        BinArr[svj] = mvars.cm603dfB[svc, svj];
                    }
                    for (int i = 0; i < 4; i++) BinArr[i] = 0;
                    for (int i = 0x1C; i < 0x24; i++) BinArr[i] = 0; lstget1.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0");
                    //for (int i = 0x20; i < 0x24; i++) BinArr[i] = 0; lst_get1.Items.Add("Caution，Addr 0x20 ~ 0x19 data = 0");
                    for (int i = 0x26; i < 0x30; i++) BinArr[i] = 0; lstget1.Items.Add("Caution，Addr 0x26 ~ 0x2F data = 0");
                    for (int i = 0x34; i < 0x3E; i++) BinArr[i] = 0; lstget1.Items.Add("Caution，Addr 0x34 ~ 0x3D data = 0");
                    for (int i = 0x58; i < 0x60; i++) BinArr[i] = 0; lstget1.Items.Add("Caution，Addr 0x58 ~ 0x5F data = 0");
                    for (int i = 0x66; i < 0x7C; i++) BinArr[i] = 0; lstget1.Items.Add("Caution，Addr 0x66 ~ 0x7B data = 0"); lstget1.TopIndex = lstget1.Items.Count - 1;
                    lbl_cm603Readchecksum.Text = "0x" + mp.DecToHex((int)mp.CalCheckSum(BinArr, 0, mvars.cm603df.GetLength(1)), 4);
                    //
                    int svbinI = BinArr[(0x3E) * 2] * 256 + BinArr[(0x3E) * 2 + 1];
                    string svbinS = mp.DecToBin(svbinI, 16);
                    if (mvars.deviceName != "H5512A")
                    {
                        svbinI = BinArr[(0x3E) * 2] * 256 + BinArr[(0x3E) * 2 + 1];
                        svbinS = mp.DecToBin(svbinI, 16);
                        if (svbinS.Substring(15, 1) != cmb_DeviceAddrCtrl.Text.Trim() || svbinS.Substring(14, 1) != "1")
                        {
                            lstget1.Items.RemoveAt(0);
                            lstget1.Items.Add("DONE,But DeviceAddress was different with with CM603 Read");
                            MessageBox.Show("WARNING !!!" + "\r\n" + "\r\n" + "DeviceAddress was different with CM603 Read");
                        }
                    }
                    //
                    svbinI = BinArr[(0x3F) * 2] * 256 + BinArr[(0x3F) * 2 + 1];
                    svbinS = mp.DecToBin(svbinI, 16);
                    chk_StatusB9.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB9.Checked = true; }
                    chk_StatusB7.Checked = false; if (svbinS.Substring(8, 1) == "1") { chk_StatusB7.Checked = true; }
                    chk_StatusB4.Checked = false; if (svbinS.Substring(11, 1) == "1") { chk_StatusB4.Checked = true; }
                    chk_StatusB3.Checked = false; if (svbinS.Substring(12, 1) == "1") { chk_StatusB3.Checked = true; }
                    chk_StatusB2.Checked = false; if (svbinS.Substring(13, 1) == "1") { chk_StatusB2.Checked = true; }
                    chk_StatusB1.Checked = false; if (svbinS.Substring(14, 1) == "1") { chk_StatusB1.Checked = true; }
                    chk_StatusB0.Checked = false; if (svbinS.Substring(15, 1) == "1") { chk_StatusB0.Checked = true; }
                    /// pGMA.Data
                    if (mvars.ICver >= 5)
                    {
                        if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06" || mvars.deviceID.Substring(0, 2) == "10")
                        {
                            if (svc == 3)
                            {
                                svbinI = mvars.cm603dfB[svc, 4] * 256 + mvars.cm603dfB[svc, 5];
                                svbinS = mp.DecToBin(svbinI, 16);
                                mvars.pGMA[0].Data[0, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.4=pGMA.0(gray0)
                                mvars.pGMA[0].Data[0, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.5=pGMA.1(gray0)

                                svbinI = mvars.cm603dfB[svc, 6] * 256 + mvars.cm603dfB[svc, 7];
                                svbinS = mp.DecToBin(svbinI, 16);
                                mvars.pGMA[0].Data[1, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.6=pGMA.0(gray0)
                                mvars.pGMA[0].Data[1, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.7=pGMA.1(gray0)

                                svbinI = mvars.cm603dfB[svc, 8] * 256 + mvars.cm603dfB[svc, 9];
                                svbinS = mp.DecToBin(svbinI, 16);
                                mvars.pGMA[0].Data[2, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.8=pGMA.0(gray0)
                                mvars.pGMA[0].Data[2, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.9=pGMA.1(gray0)
                            }
                            else
                            {
                                for (int svj = 4; svj <= 26; svj += 2)
                                {
                                    svbinI = mvars.cm603dfB[svc, svj] * 256 + mvars.cm603dfB[svc, svj+1];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    mvars.pGMA[0].Data[svc, 29 - svj-1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2); /// 603.29-4-1=pGMA.24(gray255)，603.29-26-1=pGMA.2(gray1)
                                    mvars.pGMA[0].Data[svc, 29 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);   /// 603.29-4  =pGMA.25(gray255)，603.29-26  =pGMA.3(gray1)
                                }
                            }
                        }
                        else if (mvars.deviceID.Substring(0,2)=="02" || mvars.deviceID.Substring(0,2)== "04")
                        {
                            if (mvars.deviceNameSub == "B(4t)")
                            {
                                //未編輯
                            }
                            else if (mvars.deviceNameSub == "B(4)")
                            {
                                //未編輯
                            }
                            else
                            {
                                for (int svj = 9; svj <= 25; svj += 2)
                                {
                                    svbinI = mvars.cm603dfB[svc, svj - 1] * 256 + mvars.cm603dfB[svc, svj];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    mvars.pGMA[0].Data[svc, 26 - svj - 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);  //26-9-1=16
                                    mvars.pGMA[0].Data[svc, 26 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      //26-9  =17
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int svj = 9; svj <= 25; svj += 2)
                        {
                            svbinI = mvars.cm603dfB[svc, svj - 1] * 256 + mvars.cm603dfB[svc, svj];
                            svbinS = mp.DecToBin(svbinI, 16);
                            mvars.pGMA[0].Data[svc, 26 - svj - 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);  //26-9-1=16(max)
                            mvars.pGMA[0].Data[svc, 26 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      //26-9  =17(max)
                        }
                    }
                    

                    for (int SvR = 0; SvR < dgv_Bin.Rows.Count; SvR++)
                    {
                        for (int SvC = 0; SvC < dgv_Bin.ColumnCount; SvC++)
                        {
                            dgv_Bin.Rows[SvR].Cells[SvC].Value = mvars.cm603df[svc, SvC + dgv_Bin.ColumnCount * SvR];
                        }
                    }
                }
            }
            lstget1.Items.Add("");
            lstget1.Items.Add("");

            //if (uc_C12Ademura.svnova == false) { mp.CommClose(); }
            if (mvars.svnova)
            {
                #region NovaStar LAN position
                lstget1.Items.Add("Panel gamma reads = " + lst_hpb.Items.Count);
                lstget1.Items.Add("Cabinet gamma bin files @ " + svfilepath);
                #endregion NovaStar LAN position
            }
            lstget1.Items.Add("");
            lstget1.TopIndex = lstget1.Items.Count - 1;

            btn_cm603Load.Enabled = true;
            btn_cm603Read.Enabled = true;
            btn_cm603Save.Enabled = true;
            btn_cm603Write.Enabled = true;

            ///lbl_cm603Readchecksum.Text = "         ";
        }


        private void btn_cm603Write_Click(object sender, EventArgs e)
        {
            if (chk_MTP.Checked)
            {
                mvars.UUT.MTP = 1;
                if (MessageBox.Show("Write gammaIC bin files @ MTP mode", mvars.strUInameMe,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                { return; }
            }
            else { mvars.UUT.MTP = 0; }

            markRESET();

            //byte svm;
            btn_cm603Load.Enabled = false;
            btn_cm603Read.Enabled = false;
            btn_cm603Save.Enabled = false;
            btn_cm603Write.Enabled = false;

            lstget1.Items.Clear();
            int svc = 0;
            if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04" || mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")     //C12A
            {
                if (dgvBtn.Text.Substring(0, 1) == "R") { svc = 0; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                else if (dgvBtn.Text.Substring(0, 1) == "G") { svc = 1; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                else if (dgvBtn.Text.Substring(0, 1) == "B") { svc = 2; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                else if (dgvBtn.Text.Substring(0, 1) == "M") { svc = 3; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
            }
            else if (mvars.deviceID.Substring(0, 2) == "03")     //H5512A
            {
                if (dgvBtn.Text.Substring(0, 1) == "R") { svc = 0; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                else if (dgvBtn.Text.Substring(0, 1) == "G") { svc = 1; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                else if (dgvBtn.Text.Substring(0, 1) == "B") { svc = 2; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
            }
            mvars.cm603WRaddr = mvars.cm603RA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())];

            bool svdone = true;
            //DateTime t1;//= DateTime.Now;
            byte dataH;// = 0;
            byte dataL;// = 0;
            if (mvars.demoMode == false)
            {
                //UNLOCK +2
                mvars.lblCmd = "CM603_UNLOCKWP1";
                if (svdone) { mp.mhcm603UnlockWP1(mvars.cm603WRaddr); }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                mvars.lblCmd = "CM603_UNLOCKWP2";
                if (svdone) { mp.mhcm603UnlockWP2(mvars.cm603WRaddr); }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                //先RESET StatusReg +1 
                for (Form1.pvindex = 0x3F; Form1.pvindex <= 0x3F; Form1.pvindex++)
                {
                    UInt32 u32Val = mp.CRC_Cal(640, (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    dataH = (byte)((u32Val + 640) / 256);
                    dataL = (byte)((u32Val + 640) % 256);
                    mvars.lblCmd = "CM603_WRITE_0x" + mp.DecToHex(Form1.pvindex, 2);
                    if (svdone) { mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL); }
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                }
                //LOCK +2
                mvars.lblCmd = "CM603_LOCKWP1";
                if (svdone) { mp.mhcm603lockWP1(mvars.cm603WRaddr); }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                mvars.lblCmd = "CM603_LOCKWP2";
                if (svdone) { mp.mhcm603lockWP2(mvars.cm603WRaddr); }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                //UNLOCK +2
                mvars.lblCmd = "CM603_UNLOCKWP1";
                if (svdone) { mp.mhcm603UnlockWP1(mvars.cm603WRaddr); }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                mvars.lblCmd = "CM603_UNLOCKWP2";
                if (svdone) { mp.mhcm603UnlockWP2(mvars.cm603WRaddr); }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
            }


            int svbinI;
            string svbinS;
            for (Form1.pvindex = 0x02; Form1.pvindex <= 0x3E; Form1.pvindex++)
            {
                if (mvars.cm603exp[Form1.pvindex].ToUpper().IndexOf("RESERVED", 0) == -1 && mvars.cm603exp[Form1.pvindex].ToUpper().IndexOf("CMI", 0) == -1 && mvars.cm603exp[Form1.pvindex].ToUpper().IndexOf("MTP", 0) == -1)
                {
                    if (mvars.demoMode == false)
                    {
                        dataH = Convert.ToByte((string)dgv_Bin.Rows[(Form1.pvindex * 2 / 16)].Cells[(Form1.pvindex * 2 % 16)].Value, 16);
                        dataL = Convert.ToByte((string)dgv_Bin.Rows[(Form1.pvindex * 2 / 16)].Cells[(Form1.pvindex * 2 % 16) + 1].Value, 16);
                        mvars.lblCmd = "CM603_WRITE_0x" + mp.DecToHex(Form1.pvindex, 2);
                        if (svdone) { mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL); }
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                    }
                    /// pGMA.Data
                    if (mvars.ICver >= 5)
                    {
                        if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")
                        {
                            mvars.cm603df[svc, Form1.pvindex * 2] = (string)dgv_Bin.Rows[(Form1.pvindex * 2 / 16)].Cells[(Form1.pvindex * 2 % 16)].Value;
                            mvars.cm603df[svc, Form1.pvindex * 2 + 1] = (string)dgv_Bin.Rows[(Form1.pvindex * 2 / 16)].Cells[(Form1.pvindex * 2 % 16) + 1].Value;
                            mvars.cm603dfB[svc, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svc, Form1.pvindex * 2]);
                            mvars.cm603dfB[svc, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svc, Form1.pvindex * 2 + 1]);

                            mvars.cm603Vref[svc] = mp.HexToDec((string)dgv_Bin.Rows[(0x1F * 2 / 16)].Cells[(0x1F * 2 % 16)].Value) * 256 + mp.HexToDec((string)dgv_Bin.Rows[(0x1F * 2 / 16)].Cells[(0x1F * 2 % 16) + 1].Value);

                            if (svc == 3)
                            {
                                if (Form1.pvindex <= 4)
                                {
                                    svbinI = mvars.cm603dfB[svc, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svc, Form1.pvindex * 2 + 1];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    mvars.pGMA[0].Data[Form1.pvindex - 2, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.4=pGMA.0(gray0)
                                    mvars.pGMA[0].Data[Form1.pvindex - 2, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.5=pGMA.1(gray0)
                                }
                                else if (Form1.pvindex == 0x12)
                                {
                                    mvars.cm603df[svc, 0x1F * 2] = (string)dgv_Bin.Rows[(0x1F * 2 / 16)].Cells[(0x1F * 2 % 16)].Value;
                                    mvars.cm603df[svc, 0x1F * 2 + 1] = (string)dgv_Bin.Rows[(0x1F * 2 / 16)].Cells[(0x1F * 2 % 16) + 1].Value;
                                    mvars.cm603dfB[svc, 0x1F * 2] = (byte)mp.HexToDec(mvars.cm603df[svc, 0x1F * 2]);
                                    mvars.cm603dfB[svc, 0x1F * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svc, 0x1F * 2 + 1]);
                                    svbinI = mvars.cm603dfB[svc, (byte)0x1F * 2] * 256 + mvars.cm603dfB[svc, (byte)0x1F * 2 + 1];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    int cm603VrefCode = mp.BinToDec(svbinS.Substring(6, 10));
                                    mvars.cm603Vref[svc] = Convert.ToSingle((0.01953 * cm603VrefCode).ToString("##0.0#"));

                                    /// Vgamma_code轉電壓
                                    svbinI = mvars.cm603dfB[svc, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svc, Form1.pvindex * 2 + 1];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    mvars.UUT.VREF = Convert.ToSingle((mvars.cm603Vref[svc] * mp.BinToDec(svbinS.Substring(6, 10)) / 1024).ToString("##0.0###"));
                                }
                            }
                            else
                            {
                                if (Form1.pvindex >= 0x02 && Form1.pvindex <= 0x0D)
                                {
                                    svbinI = mvars.cm603dfB[svc, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svc, Form1.pvindex * 2 + 1];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    mvars.pGMA[0].Data[svc, 2 + mvars.pGMA[0].Data.GetLength(1) - Form1.pvindex * 2] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);     ///4*2=8;  24-(4*2)=16
                                    mvars.pGMA[0].Data[svc, 2 + mvars.pGMA[0].Data.GetLength(1) - Form1.pvindex * 2 + 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);   ///4*2-1=7;24-(4*2-1)=17
                                }
                                else if (Form1.pvindex == 0x1F)
                                {
                                    ///Gamma & VCOM output Voltage = (0.01953*VREF code* Vgamma_code)/1024
                                    svbinI = mvars.cm603dfB[svc, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svc, Form1.pvindex * 2 + 1];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    int cm603VrefCode = mp.BinToDec(svbinS.Substring(6, 10));
                                    mvars.cm603Vref[svc] = Convert.ToSingle((0.01953 * cm603VrefCode).ToString("##0.0#"));
                                }
                                //for (int svj = 4; svj <= 26; svj += 2)
                                //{
                                //    svbinI = mvars.cm603dfB[svc, svj] * 256 + mvars.cm603dfB[svc, svj + 1];
                                //    svbinS = mp.DecToBin(svbinI, 16);
                                //    mvars.pGMA.Data[svc, 29 - svj - 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);   /// 603.29-4-1=pGMA.24(gray255)，603.29-26-1=pGMA.2(gray1)
                                //    mvars.pGMA.Data[svc, 29 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);       /// 603.29-4  =pGMA.25(gray255)，603.29-26  =pGMA.3(gray1)
                                //}
                            }
                        }
                        else if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04")
                        {
                            if (mvars.deviceNameSub == "B(4t)")
                            {
                                //未編輯
                            }
                            else if (mvars.deviceNameSub == "B(4)")
                            {
                                //未編輯
                            }
                            else
                            {
                                for (int svj = 9; svj <= 25; svj += 2)
                                {
                                    svbinI = mvars.cm603dfB[svc, svj - 1] * 256 + mvars.cm603dfB[svc, svj];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    mvars.pGMA[0].Data[svc, 26 - svj - 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);  //26-9-1=16
                                    mvars.pGMA[0].Data[svc, 26 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      //26-9  =17
                                }
                            }
                        }
                    }
                    else
                    {
                        //for (int svj = 9; svj <= 25; svj += 2)
                        //{
                        //    svbinI = mvars.cm603dfB[svc, svj - 1] * 256 + mvars.cm603dfB[svc, svj];
                        //    svbinS = mp.DecToBin(svbinI, 16);
                        //    mvars.pGMA.Data[svc, 26 - svj - 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);  //26-9-1=16(max)
                        //    mvars.pGMA.Data[svc, 26 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      //26-9  =17(max)
                        //}

                        if (Form1.pvindex >= 0x04 && Form1.pvindex <= 0x0C)
                        {
                            svbinI = mvars.cm603dfB[svc, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svc, Form1.pvindex * 2 + 1];
                            svbinS = mp.DecToBin(svbinI, 16);
                            mvars.pGMA[0].Data[svc, 24 - Form1.pvindex * 2] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);         //4*2=8;  24-(4*2)=16
                            mvars.pGMA[0].Data[svc, 24 - Form1.pvindex * 2 + 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);   //4*2-1=7;24-(4*2-1)=17
                        }
                    }
                }
            }

            if (mvars.demoMode == false)
            {
                mvars.lblCmd = "CM603_LOCKWP1";
                if (svdone) { mp.mhcm603lockWP1(mvars.cm603WRaddr); }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                mvars.lblCmd = "CM603_LOCKWP2";
                if (svdone) { mp.mhcm603lockWP2(mvars.cm603WRaddr); }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1 && svdone) { svdone = false; }
                //if (svdone) { lstget1.Items.Add(svss[0] + "," + svss[1] + ",DONE,@ " + ((DateTime.Now - t1).TotalSeconds).ToString("###0.0") + "s"); svdonecount++; }
                //else { lstget1.Items.Add(svss[0] + "," + svss[1] + ",ERROR,@ " + ((DateTime.Now - t1).TotalSeconds).ToString("###0.0") + "s"); }
            }
            if (svdone) lstget1.Items.Add("cm603(" + dgvBtn.Text.Substring(0, 1) + ") write @ socket " + cmbhPB.Text + " Successed");
            else lstget1.Items.Add("cm603(" + dgvBtn.Text.Substring(0, 1) + ") write @ socket " + cmbhPB.Text + " fail");
            btn_cm603Load.Enabled = true;
            btn_cm603Read.Enabled = true;
            btn_cm603Save.Enabled = true;
            btn_cm603Write.Enabled = true;
            lstget1.TopIndex = lstget1.Items.Count - 1;
        }

        private void lst_hpb_Click(object sender, EventArgs e)
        {
            //string[] svss = lst_hpb.Items[lst_hpb.SelectedIndex].ToString().Split(',');
            //byte svm = 0;
            //for (svm = 0; svm < Form1.cmbhPB.Items.Count; svm++)
            //{
            //    if (Form1.cmbhPB.Items[svm].ToString().Trim().IndexOf(svss[0].Trim(), 0) > -1) { Form1.cmbhPB.Text = Form1.cmbhPB.Items[svm].ToString(); cmb_hPB.Text = cmb_hPB.Items[svm].ToString(); break; }
            //}

            //int svi = 0;
            //if (lst_hpb.Items[lst_hpb.SelectedIndex].ToString().IndexOf(",R,", 0) > -1) { dgvBtn.Text = "R ct0,0"; svi = 0; dgvBtn.ForeColor = Color.Red; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
            //else if (lst_hpb.Items[lst_hpb.SelectedIndex].ToString().IndexOf(",G,", 0) > -1) { dgvBtn.Text = "G ct0,1"; svi = 1; dgvBtn.ForeColor = Color.Green; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
            //else if (lst_hpb.Items[lst_hpb.SelectedIndex].ToString().IndexOf(",B,", 0) > -1) { dgvBtn.Text = "B ct1,1"; svi = 2; dgvBtn.ForeColor = Color.Blue; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }

            //string svfilepath = mvars.strStartUpPath + "\\Parameter\\temp\\" + svf[cmb_LANpos.SelectedIndex].ToString() + "\\";
            //if (!Directory.Exists(svfilepath)) { svfilepath = mvars.strStartUpPath + "\\Parameter\\gammaIC\\" + svf[cmb_LANpos.SelectedIndex].ToString() + "\\"; }
            //else
            //{
            //    //string[] svss = lst_hpb.Items[lst_hpb.SelectedIndex].ToString().Split((','));
            //    byte[] BinArr = File.ReadAllBytes(svfilepath + svss[0] + "," + svss[1] + ".bin");
            //    if (BinArr.Length != 128) { MessageBox.Show("File Length <> 128 bytes" + "\r\n" + "\r\n" + "FileLen " + BinArr.Length, "Error"); return; }
            //    else
            //    {
            //        Buffer.BlockCopy(BinArr, 0, mvars.cm603dfB, svi * BinArr.Length, BinArr.Length);
            //        for (int svj = 0; svj < BinArr.Length; svj++) { mvars.cm603df[svi, svj] = mp.DecToHex(mvars.cm603dfB[svi, svj], 2); dgv_Bin.Rows[svj / 16].Cells[svj % 16].Value = mvars.cm603df[svi, svj]; }
            //    }
            //}
            //if (!Directory.Exists(svfilepath)) { MessageBox.Show("No gammafile folder"); return; }
        }

        private void lst_hpb_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right) { lst_hpb.ClearSelected(); }
            //MessageBox.Show("SelectedIndex " + lst_hpb.SelectedIndex);
        }

        private void btn_cm603Load_Click(object sender, EventArgs e)
        {
            int svi = 0;
            byte[] BinArr;
            OpenFileDialog dialog = new OpenFileDialog();
            if (dgvBtn.Text.Substring(0, 1) == "R") { svi = 0; dialog.Title = "Select CM603(" + dgvBtn.Text.Substring(0, 1) + ") bin file"; }
            else if (dgvBtn.Text.Substring(0, 1) == "G") { svi = 1; dialog.Title = "Select CM603(" + dgvBtn.Text.Substring(0, 1) + ") bin file"; }
            else if (dgvBtn.Text.Substring(0, 1) == "B") { svi = 2; dialog.Title = "Select CM603(" + dgvBtn.Text.Substring(0, 1) + ") bin file"; }
            else if (dgvBtn.Text.Substring(0, 1) == "M") { svi = 3; dialog.Title = "Select CM603(" + dgvBtn.Text.Substring(0, 1) + ") bin file"; }
            dialog.InitialDirectory = mvars.strStartUpPath + @"\Parameter";
            dialog.FileName = "";
            dialog.Filter = "Bin files (*.bin)|*.bin";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                BinArr = File.ReadAllBytes(dialog.FileName);
                if (BinArr.Length != 128) { MessageBox.Show("File Length <> 128 bytes" + "\r\n" + "\r\n" + "FileLen " + BinArr.Length, "Error"); }
                else
                {
                    Buffer.BlockCopy(BinArr, 0, mvars.cm603dfB, BinArr.Length * svi, BinArr.Length);
                    for (int svj = 0; svj < BinArr.Length; svj++) { mvars.cm603df[svi, svj] = mp.DecToHex(mvars.cm603dfB[svi, svj], 2); }
                }

                Array.Copy(mvars.cm603df, mvars.RegData, mvars.cm603df.Length);
                for (int SvR = 0; SvR < dgvBin.Rows.Count; SvR++)
                {
                    for (int SvC = 0; SvC < dgvBin.ColumnCount; SvC++)
                    {
                        dgvBin.Rows[SvR].Cells[SvC].Value = (mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR]);
                    }
                }
                //pGMA.Data
                if (mvars.deviceID.Substring(0, 2) == "05")
                {
                    if (svi == 3)
                    {
                        Form1.pvindex = 0x02;
                        int svbinI = mvars.cm603dfB[svi, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svi, Form1.pvindex * 2 + 1];
                        string svbinS = mp.DecToBin(svbinI, 16);
                        mvars.pGMA[0].Data[0, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);
                        mvars.pGMA[0].Data[0, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);
                        Form1.pvindex = 0x03;
                        svbinI = mvars.cm603dfB[svi, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svi, Form1.pvindex * 2 + 1];
                        svbinS = mp.DecToBin(svbinI, 16);
                        mvars.pGMA[0].Data[1, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);
                        mvars.pGMA[0].Data[1, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);
                        Form1.pvindex = 0x04;
                        svbinI = mvars.cm603dfB[svi, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svi, Form1.pvindex * 2 + 1];
                        svbinS = mp.DecToBin(svbinI, 16);
                        mvars.pGMA[0].Data[2, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);
                        mvars.pGMA[0].Data[2, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);
                    }
                    else
                    {
                        /// cm603bin轉pGMA
                        for (Form1.pvindex = 0x02; Form1.pvindex <= 0x0D; Form1.pvindex++)
                        {
                            int svbinI = mvars.cm603dfB[svi, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svi, Form1.pvindex * 2 + 1];
                            string svbinS = mp.DecToBin(svbinI, 16);
                            mvars.pGMA[0].Data[svi, 28 - Form1.pvindex * 2] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);     /// 28-2*2=24  28-3*2=22
                            mvars.pGMA[0].Data[svi, 29 - Form1.pvindex * 2] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);     /// 29-2*2=25  29-3*2=23
                        }
                    }
                }
                else if (mvars.deviceID.Substring(0, 2) == "02") 
                {
                    if (mvars.deviceNameSub == "B(4)")
                    {

                    }
                    else if (mvars.deviceNameSub == "B(4t)")
                    {

                    }
                    else
                    {
                        for (int svj = 9; svj <= 25; svj += 2)
                        {
                            int svbinI = mvars.cm603dfB[svi, svj - 1] * 256 + mvars.cm603dfB[svi, svj];
                            string svbinS = mp.DecToBin(svbinI, 16);
                            mvars.pGMA[0].Data[svi, 26 - svj - 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);  //26-9-1=16
                            mvars.pGMA[0].Data[svi, 26 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      //26-9  =17
                        }
                    }
                }
                BinArr = new byte[128];
                Buffer.BlockCopy(mvars.cm603dfB, BinArr.Length * svi, BinArr, 0, BinArr.Length);

                for (int i = 0x00; i <= 0x01; i++) { BinArr[i * 2] = 0x00; BinArr[i * 2 + 1] = 0x00; }; lst_get.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0xFF");
                for (int i = 0x0E; i <= 0x11; i++) { BinArr[i * 2] = 0x00; BinArr[i * 2 + 1] = 0xFF; }; lst_get.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0xFF");
                for (int i = 0x13; i <= 0x17; i++) { BinArr[i * 2] = 0x00; BinArr[i * 2 + 1] = 0xFF; }; lst_get.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0xFF");
                for (int i = 0x1A; i <= 0x1E; i++) { BinArr[i * 2] = 0x00; BinArr[i * 2 + 1] = 0xFF; }; lst_get.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0xFF");
                for (int i = 0x2C; i <= 0x2F; i++) { BinArr[i * 2] = 0x00; BinArr[i * 2 + 1] = 0xFF; }; lst_get.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0xFF");
                for (int i = 0x33; i <= 0x3D; i++) { BinArr[i * 2] = 0x00; BinArr[i * 2 + 1] = 0xFF; }; lst_get.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0xFF");
                for (int i = 0x13; i <= 0x17; i++) { BinArr[i * 2] = 0x00; BinArr[i * 2 + 1] = 0xFF; }; lst_get.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0xFF");
                for (int i = 0x3F; i <= 0x3F; i++) { BinArr[i * 2] = 0x00; BinArr[i * 2 + 1] = 0xFF; }; lst_get.Items.Add("Caution，Addr 0x1C ~ 0x23 data = 0xFF");
                lbl_cm603Loadchecksum.Text = "0x" + mp.DecToHex((int)mp.CalCheckSum(BinArr, 0, BinArr.Length), 4);
            }
        }

        private void lbl_cm603Readchecksum_Click(object sender, EventArgs e) { }
        private void lbl_cm603Readchecksum_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select gma file",
                InitialDirectory = mvars.strStartUpPath,
                FileName = "",
                Filter = "gma files (*.gma)|*.gma"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool svnewV = false;
                mvars.defaultGammafile= dialog.FileName;
                StreamReader sTAread = File.OpenText(mvars.defaultGammafile);
                while (true)
                {
                    string svs = sTAread.ReadLine();
                    if (svs == null) break;
                    if (svs.Length > 0 && svs.ToUpper().IndexOf("TFT-VREF") != -1) { svnewV = true; break; }
                }
                sTAread.Close();
                if (mvars.ICver>=5 || mvars.deviceID.Substring(0, 2) == "05")
                {
                    if (svnewV) mp.fileDefaultGammaV(false);
                    else mp.fileDefaultGammaAIO(false, mvars.defaultGammafile);
                }
                else
                {
                    mvars.fileCM603Gamma(false, dialog.FileName);
                    if (dgvBtn.Text.Substring(0, 1) == "R" || dgvBtn.Text.Substring(0, 1) == "G" || dgvBtn.Text.Substring(0, 1) == "B")
                    {
                        int svi = 0;
                        if (dgvBtn.Text.Substring(0, 1) == "R") svi = 0;
                        else if (dgvBtn.Text.Substring(0, 1) == "G") svi = 1;
                        else if (dgvBtn.Text.Substring(0, 1) == "B") svi = 2;
                        for (int SvR = 0; SvR < dgvBin.Rows.Count; SvR++)
                        {
                            for (int SvC = 0; SvC < dgvBin.ColumnCount; SvC++)
                            {
                                dgvBin.Rows[SvR].Cells[SvC].Value = mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR];
                                mvars.cm603dfB[svi, SvC + dgvBin.ColumnCount * SvR] = (byte)mp.HexToDec(mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR]);
                            }
                        }
                    }
                } 
            }
        }

        private void lbl_BinDes_Click(object sender, EventArgs e)
        {
            lbl_BinDes.Visible = false;
            if (lbl_BinDes.Text.IndexOf("Click") != -1) cmb_BinDes.Text = cmb_BinDes.Items[0].ToString();
            cmb_BinDes.Visible = true;
            cmb_BinDes.Focus();
        }

        private void cmb_hPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.deviceName == "H5512A" && mvars.verMCU != null)
            {
                if (mvars.verMCU.Substring(0, 4) == "OBB-" || mvars.verMCU.Substring(0, 4) == "ABB-" || mvars.verMCU.Substring(0, 4) == "BBB-" ||
                         mvars.verMCU.Substring(0, 4) == "OCB-" || mvars.verMCU.Substring(0, 4) == "ACB-" || mvars.verMCU.Substring(0, 4) == "BCB-")
                {
                    mvars.iPBaddr = Convert.ToByte(cmb_hPB.Text.Trim());
                    mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1);
                    if (btn_dgv.Text.Trim().Substring(0, 1) == "R") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1); }
                    else if (btn_dgv.Text.Trim().Substring(0, 1) == "G") { mvars.cm603WRaddr = 214; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                    else if (btn_dgv.Text.Trim().Substring(0, 1) == "B") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                    mvars.deviceID = "03" + string.Format("{0:00}", (byte)((mvars.iPBaddr - 1) / 2 + 1));
                }
                else
                {
                    MessageBox.Show("H5512A(\"OBB/ABB/ACB/OCB/BBB/BCB\" MCU ver check error (read: " + mvars.verMCU + ")" + "\r\n" + "\r\n" +
                                     "Please check the Hardware", "H5512A");
                    //this.Dispose();
                    //this.Close();
                }
            }
            else
            {
                Form1.cmbhPB.SelectedIndex = cmb_hPB.SelectedIndex;
            }
        }

        private void btn_dgv_Click(object sender, EventArgs e)
        {
            int svi = 0;
            if (mvars.deviceID.Substring(0, 2) == "02" || 
                mvars.deviceID.Substring(0, 2) == "04" ||
                mvars.deviceID.Substring(0, 2) == "05" ||
                mvars.deviceID.Substring(0, 2) == "06" ||
                mvars.deviceID.Substring(0, 2) == "10")
            {
                if (mvars.ICver < 5)
                {
                    if (dgvBtn.Text.Substring(0, 1) == "R") { dgvBtn.Text = "G ct0,1"; svi = 1; dgvBtn.ForeColor = Color.DarkGreen; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                    else if (dgvBtn.Text.Substring(0, 1) == "G") { dgvBtn.Text = "B ct1,1"; svi = 2; dgvBtn.ForeColor = Color.Blue; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                    else if (dgvBtn.Text.Substring(0, 1) == "B") { dgvBtn.Text = "R ct0,0"; svi = 0; dgvBtn.ForeColor = Color.Magenta; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                }
                if (mvars.ICver >= 5)
                {
                    if (mvars.deviceID.Substring(0, 2) == "10")
                    {
                        if (dgvBtn.Text.Substring(0, 1) == "R") { dgvBtn.Text = "G ct0,1"; svi = 1; dgvBtn.ForeColor = Color.DarkGreen; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "G") { dgvBtn.Text = "B ct1,1"; svi = 2; dgvBtn.ForeColor = Color.Blue; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "B") { dgvBtn.Text = "R ct0,0"; svi = 0; dgvBtn.ForeColor = Color.Red; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                    }
                    else
                    {
                        if (dgvBtn.Text.Substring(0, 1) == "R") { dgvBtn.Text = "G ct0,1"; svi = 1; dgvBtn.ForeColor = Color.DarkGreen; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "G") { dgvBtn.Text = "B ct1,1"; svi = 2; dgvBtn.ForeColor = Color.Blue; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "B") { dgvBtn.Text = "M ct1,0"; svi = 3; dgvBtn.ForeColor = Color.Magenta; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[1].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                        else if (dgvBtn.Text.Substring(0, 1) == "M") { dgvBtn.Text = "R ct0,0"; svi = 0; dgvBtn.ForeColor = Color.Red; cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString(); cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); }
                    }
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "03")     //H5512A
            {
                mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1);
                if (dgvBtn.Text.Substring(0, 1) == "R")
                {
                    dgvBtn.Text = "G ct0,1"; svi = 1; dgvBtn.ForeColor = Color.DarkGreen; cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[1].ToString(); cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString();
                    mvars.cm603WRaddr = 214; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2);
                }
                else if (dgvBtn.Text.Substring(0, 1) == "G")
                {
                    dgvBtn.Text = "B ct0,0"; svi = 2; dgvBtn.ForeColor = Color.Blue; cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString();
                    mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2);
                }
                else if (dgvBtn.Text.Substring(0, 1) == "B")
                {
                    dgvBtn.Text = "R ct0,0"; svi = 0; dgvBtn.ForeColor = Color.Red; cmb_DeviceAddrA0.Text = cmb_DeviceAddrA0.Items[0].ToString(); cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[0].ToString();
                    mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1);
                }
                mvars.deviceID = "03" + string.Format("{0:00}", (byte)((mvars.iPBaddr - 1) / 2 + 1));
            }
            Form1.cmbCM603.SelectedIndex = svi;
            Form1.cmbCM603.Text = Form1.cmbCM603.Items[svi].ToString();

            lbl_BinDes.Text = "Click ! Register Select";
            lbl_BinVal.Text = "";
            lbl_BinMsg.Text = "";
            lbl_BinHex.Text = "";
            lbl_BinVol.Text = "";
            if (lbl_cm603Readchecksum.Text.Length > 0 && lbl_cm603Readchecksum.Text.Substring(0, 2) != "0x") { lbl_cm603Readchecksum.Text = "checksum"; }
            if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")
            {
                if (dgvBtn.Text.Substring(0, 1) == "R" || dgvBtn.Text.Substring(0, 1) == "G" || dgvBtn.Text.Substring(0, 1) == "B" || dgvBtn.Text.Substring(0, 1) == "M")
                {
                    for (int SvR = 0; SvR < dgvBin.Rows.Count; SvR++)
                    {
                        for (int SvC = 0; SvC < dgvBin.ColumnCount; SvC++)
                        {
                            dgvBin.Rows[SvR].Cells[SvC].Value = mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR];
                            mvars.cm603dfB[svi, SvC + dgvBin.ColumnCount * SvR] = (byte)mp.HexToDec(mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR]);
                        }
                    }
                    byte[] BinArr = new byte[128];
                    Buffer.BlockCopy(mvars.cm603dfB, BinArr.Length * svi, BinArr, 0, BinArr.Length);
                    for (int i = 0; i < 4; i++) BinArr[i] = 0;
                    for (int i = 0x1C; i < 0x20; i++) BinArr[i] = 0;
                    for (int i = 0x20; i < 0x24; i++) BinArr[i] = 0;
                    for (int i = 0x26; i < 0x30; i++) BinArr[i] = 0;
                    for (int i = 0x34; i < 0x3E; i++) BinArr[i] = 0;
                    for (int i = 0x58; i < 0x60; i++) BinArr[i] = 0;
                    for (int i = 0x66; i < 0x7C; i++) BinArr[i] = 0;
                    lbl_cm603Readchecksum.Text = "0x" + mp.DecToHex((int)mp.CalCheckSum(BinArr, 0, BinArr.Length), 4);
                    //int svbinI = mvars.cm603dfB[svi, (0x3E) * 2] + mvars.cm603dfB[svi, (0x3E) * 2 + 1];
                    //string svbinS = mp.DecToBin(svbinI, 16);
                    //svbinS = svbinS.Substring(svbinS.Length - 1, 1);
                    //cmb_DeviceAddrCtrl.Text = cmb_DeviceAddrCtrl.Items[Convert.ToInt32(svbinS)].ToString();   //mk4

                    mvars.cm603WRaddr = mvars.cm603WA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())];

                    int svbinI = mvars.cm603dfB[svi, (0x3F) * 2] + mvars.cm603dfB[svi, (0x3F) * 2 + 1];
                    string svbinS = mp.DecToBin(svbinI, 16);
                    chk_StatusB9.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB9.Checked = true; }
                    chk_StatusB7.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB7.Checked = true; }
                    chk_StatusB4.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB4.Checked = true; }
                    chk_StatusB3.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB3.Checked = true; }
                    chk_StatusB2.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB2.Checked = true; }
                    chk_StatusB1.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB1.Checked = true; }
                    chk_StatusB0.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB0.Checked = true; }
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "10")
            {
                if (dgvBtn.Text.Substring(0, 1) == "R" || dgvBtn.Text.Substring(0, 1) == "G" || dgvBtn.Text.Substring(0, 1) == "B")
                {
                    for (int SvR = 0; SvR < dgvBin.Rows.Count; SvR++)
                    {
                        for (int SvC = 0; SvC < dgvBin.ColumnCount; SvC++)
                        {
                            dgvBin.Rows[SvR].Cells[SvC].Value = mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR];
                            mvars.cm603dfB[svi, SvC + dgvBin.ColumnCount * SvR] = (byte)mp.HexToDec(mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR]);
                        }
                    }
                    byte[] BinArr = new byte[128];
                    Buffer.BlockCopy(mvars.cm603dfB, BinArr.Length * svi, BinArr, 0, BinArr.Length);
                    for (int i = 0; i < 4; i++) BinArr[i] = 0;
                    for (int i = 0x1C; i < 0x20; i++) BinArr[i] = 0;
                    for (int i = 0x20; i < 0x24; i++) BinArr[i] = 0;
                    for (int i = 0x26; i < 0x30; i++) BinArr[i] = 0;
                    for (int i = 0x34; i < 0x3E; i++) BinArr[i] = 0;
                    for (int i = 0x58; i < 0x60; i++) BinArr[i] = 0;
                    for (int i = 0x66; i < 0x7C; i++) BinArr[i] = 0;
                    lbl_cm603Readchecksum.Text = "0x" + mp.DecToHex((int)mp.CalCheckSum(BinArr, 0, BinArr.Length), 4);
                    mvars.cm603WRaddr = mvars.cm603WA0Ctrl[Convert.ToInt32(cmb_DeviceAddrA0.Text.Trim()), Convert.ToInt32(cmb_DeviceAddrCtrl.Text.Trim())];
                    int svbinI = mvars.cm603dfB[svi, (0x3F) * 2] + mvars.cm603dfB[svi, (0x3F) * 2 + 1];
                    string svbinS = mp.DecToBin(svbinI, 16);
                    chk_StatusB9.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB9.Checked = true; }
                    chk_StatusB7.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB7.Checked = true; }
                    chk_StatusB4.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB4.Checked = true; }
                    chk_StatusB3.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB3.Checked = true; }
                    chk_StatusB2.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB2.Checked = true; }
                    chk_StatusB1.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB1.Checked = true; }
                    chk_StatusB0.Checked = false; if (svbinS.Substring(6, 1) == "1") { chk_StatusB0.Checked = true; }
                }
            }
            else
            {
                if (dgvBtn.Text.Substring(0, 1) == "R" || dgvBtn.Text.Substring(0, 1) == "G" || dgvBtn.Text.Substring(0, 1) == "B")
                {
                    for (int SvR = 0; SvR < dgvBin.Rows.Count; SvR++)
                    {
                        for (int SvC = 0; SvC < dgvBin.ColumnCount; SvC++)
                        {
                            dgvBin.Rows[SvR].Cells[SvC].Value = mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR];
                            mvars.cm603dfB[svi, SvC + dgvBin.ColumnCount * SvR] = (byte)mp.HexToDec(mvars.cm603df[svi, SvC + dgvBin.ColumnCount * SvR]);
                        }
                    }
                }
            }
        }

        
    }
}
