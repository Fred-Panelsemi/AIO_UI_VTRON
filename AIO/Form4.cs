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
using System.Text.RegularExpressions; //字元切割

namespace INXPID
{
    public partial class Form4 : Form
    {
        public static DataGridView dgvatg = new DataGridView();
        Button dgvBtn = new Button();
        string[] NovaRow = { "Gamma", "WLv", "Wx", "Wy", "△Lv %", "△x", "△y", "Target WLv", "[10:8] ", "[7:0] ", "[10:8] ", "[7:0] ", "[10:8] ", "[7:0] " };
        //public static i3_Init i3init = new i3_Init();
        public static i3_Init i3init = null;
        public static Label lbl_counter = new Label();
        public static Label lbl_target = new Label();
        public static Label lbl_ctime = new Label();
        //public static TextBox txt_ctime = new TextBox();
        public static ListBox lst_get = new ListBox();
        public static Label lbl_slaveaddr = new Label();
        public static Label lbl_icbver = new Label();
        public static Label lbl_pbaddr = new Label();
        public static Label lbl_ipbver = new Label();
        public static Label lbl_lb = new Label();
        public static Label lblDUTY = new Label();

        public static Label lblL0ADJ = new Label();  //v1.1

        int gmax = mvars.GMAterminals;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            int svi = 0;
                      
            lbl_Cx.Text = mvars.UUT.Cx.ToString();
            lbl_Cy.Text = mvars.UUT.Cy.ToString(); 
            lbl_CLv.Text = mvars.UUT.CLv.ToString(); 
            lbl_CACh.Text = mvars.UUT.CAch.ToString(); 
            lbl_OverBet.Text = "x" + mCAs.CAATG.OverBet.ToString();
            lbl_PBAddr.Text = mvars.iPBaddr.ToString(); 
            lbl_SlaveAddr.Text = "";
            lbl_LB.Text = "";
            //lbl_SlaveAddr.Text = mvars.iCBMCUflashIndex.ToString(); 
            //lbl_LB.Text = mvars.iLBaddr.ToString(); 

            if (mvars.UUT.MTP == 1) { lbl_MTP.Text = "enMTP"; }
            else { lbl_MTP.Text = "DAC only"; }
            
            Array.Resize(ref mvars.Gamma2d2, gmax + 1); //在btn_PAGMA操作時已經先讀取所屬gamma長度
            if (mvars.GMAtype == "C12A")
            {
                lbl_LB.Visible = false;
                lbl_Info.Text = "Gamma             vref                               CA Ch. 00    Box. 1   iPBaddr. 1   " + "\r\n" +
                                "Cx                     ±" + "\r\n" +
                                "Cy                     ±" + "\r\n" +
                                "CLv >                x                   UUT id";
                lbl_OverBet.Location = new Point(257, 34);
                lbl_CACh.Location = new Point(300, 18);
                lbl_SlaveAddr.Location = new Point(361, 18);
                lbl_PBAddr.Location = new Point(433, 18);
                

                if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma") == true)
                {
                    if (mvars.fileCM603Gamma(false, mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma") == false) { mvars.mCM603("1"); }
                }
                else
                {
                    mvars.mCM603("1");
                }
                for (int svj = 0; svj <= 2; svj++)
                {
                    int svbinI = mvars.cm603dfB[svj, (byte)0x1F * 2] * 256 + mvars.cm603dfB[svj, (byte)0x1F * 2 + 1];
                    string svbinS = mp.DecToBin(svbinI, 16);
                    mvars.cm603VrefCode[svj] = mp.BinToDec(svbinS.Substring(6, 10));
                    mvars.cm603Vref[svj] = Convert.ToSingle((0.01953 * mvars.cm603VrefCode[svi]).ToString("##0.0#"));
                }
                lbl_cm603Vref.Text = mvars.cm603Vref[0].ToString("##0") + "," + mvars.cm603Vref[1].ToString("##0") + "," + mvars.cm603Vref[2].ToString("##0");
                lbl_cm603Vref.Visible = true;

                for (svi = 0; svi <= 2; svi++)
                {
                    for (int svg = 0x04; svg <= 0x0C; svg++)
                    {
                        int sL = (mvars.pGMA.Data.Length / 3) - (svg - 0x03) * 2 + 1;
                        int sH = (mvars.pGMA.Data.Length / 3) - (svg - 0x03) * 2;
                        int svtr = mp.HexToDec(mvars.pGMA.Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA.Data[svi, sL]);
                        UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, svg * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, svg * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, svg * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, svg * 2]);
                        mvars.cm603dfB[svi, svg * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, svg * 2 + 1]);
                    }
                }
                Skeleton_ATG(mvars.cm603Gamma.Length, NovaRow.Length, 480, 270);
                String fnum;
                //mvars.GMAvalue = (float)3.6;
                lbl_GMAvalue.Text = mvars.GMAvalue.ToString();
                for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
                {
                    mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), mvars.GMAvalue));
                    if (mvars.Gamma2d2[svi] < 10) { fnum = String.Format("{0:##0.0#}", mvars.Gamma2d2[svi]); }
                    else { fnum = String.Format("{0:##0.0}", mvars.Gamma2d2[svi]); }
                    //dgvatg.Rows[7].Cells[svi].Style.BackColor = Color.White;
                    dgvatg.Rows[8].Cells[svi].Value = mvars.pGMA.Data[0, 2 * svi + 0];
                    dgvatg.Rows[9].Cells[svi].Value = mvars.pGMA.Data[0, 2 * svi + 1];
                    dgvatg.Rows[10].Cells[svi].Value = mvars.pGMA.Data[1, 2 * svi + 0];
                    dgvatg.Rows[11].Cells[svi].Value = mvars.pGMA.Data[1, 2 * svi + 1];
                    dgvatg.Rows[12].Cells[svi].Value = mvars.pGMA.Data[2, 2 * svi + 0];
                    dgvatg.Rows[13].Cells[svi].Value = mvars.pGMA.Data[2, 2 * svi + 1];
                }
                mvars.Gamma2d2[0] = 0;
                mvars.Gamma2d2[gmax] = 100;

                if (mvars.flgdualduty)
                {
                    for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
                    {
                        dgvatg.Rows[8].Cells[svi + (mvars.cm603Gamma.Length + 1)].Value = mvars.pGMA1.Data[0, 2 * svi + 0];
                        dgvatg.Rows[9].Cells[svi + (mvars.cm603Gamma.Length + 1)].Value = mvars.pGMA1.Data[0, 2 * svi + 1];
                        dgvatg.Rows[10].Cells[svi + (mvars.cm603Gamma.Length + 1)].Value = mvars.pGMA1.Data[1, 2 * svi + 0];
                        dgvatg.Rows[11].Cells[svi + (mvars.cm603Gamma.Length + 1)].Value = mvars.pGMA1.Data[1, 2 * svi + 1];
                        dgvatg.Rows[12].Cells[svi + (mvars.cm603Gamma.Length + 1)].Value = mvars.pGMA1.Data[2, 2 * svi + 0];
                        dgvatg.Rows[13].Cells[svi + (mvars.cm603Gamma.Length + 1)].Value = mvars.pGMA1.Data[2, 2 * svi + 1];
                    }
                }

                for (svi = 0; svi < dgvatg.ColumnCount; svi++) { dgvatg.Rows[7].Cells[svi].Style.BackColor = Color.White; }

                lbl_icbver.SetBounds(283 + 50, 36, 40, 16);
                lbl_icbver.Font = new Font("Arial", 9);
                lbl_icbver.Text = "- - - -";
                groupBox3.Controls.AddRange(new Control[] { lbl_icbver });
                lbl_icbver.BringToFront();
                lbl_ipbver.BringToFront();

                lblDUTY.Location = new Point(215, 113);
                this.Controls.AddRange(new Control[] { lblDUTY });

                lblL0ADJ.Text = mvars.L0_ADJ + "f";
                lblL0ADJ.Location = new Point(283 + 140, 36);
                lblL0ADJ.AutoSize = true;
                lblL0ADJ.Font = new Font("Arial", 9);
                lblL0ADJ.ForeColor = Color.FromArgb(0, 0, 192);
                groupBox3.Controls.AddRange(new Control[] { lblL0ADJ });
                lblL0ADJ.BringToFront();
                if (mCAs.CAATG.caSel == 1) { chk_ZeroCAL.Checked = true; }
            }
            lst_get.TopIndex = lst_get.Items.Count - 1;
            //
            if (mvars.UUT.MTP == 1) { Form1.chkMTP.Checked = true; } else { Form1.chkMTP.Checked = false; }
            //
            lbl_counter.AutoSize = true;
            lbl_counter.SetBounds(221, 113, 13, 13);
            lbl_counter.Text = "0";
            lbl_ctime.Font = new Font("Arial", 9);
            lbl_ctime.AutoSize = true;
            lbl_ctime.SetBounds(520, 113, 13, 13);
            lbl_ctime.Text = "0";
            lbl_target.AutoSize = true;
            lbl_target.SetBounds(255, 113, 13, 13);
            lbl_target.Text = "0";
            lst_get.SetBounds(517, 13, 667, 88);
            lst_get.Items.Clear();
            //txt_ctime.SetBounds(520, 108, 667, 110);
            //txt_ctime.BorderStyle = BorderStyle.None;
            //txt_ctime.Enabled = false;
            //txt_ctime.Text = "tc";
            this.Controls.AddRange(new Control[] { lbl_counter, lbl_ctime, lst_get, lbl_ctime, lbl_target });//, txt, bOK, bCancel });
            lbl_ctime.DoubleClick += lbl_ctime_DoubleClick;

            mvars.FormShow[4] = true;
            txt_UUTID.Focus();
        }
        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dgvatg.Controls.Contains(dgvBtn)) { this.Controls.Remove(dgvBtn); }
            
            if (this.Controls.Contains(lbl_counter)) { this.Controls.Remove(lbl_counter); }
            if (this.Controls.Contains(lbl_ctime)) { this.Controls.Remove(lbl_ctime); }
            if (this.Controls.Contains(lbl_target)) { this.Controls.Remove(lbl_target); }
            if (this.Controls.Contains(lst_get)) { this.Controls.Remove(lst_get); }
            if (this.Controls.Contains(lblDUTY)) { this.Controls.Remove(lblDUTY); }
            if (groupBox3.Controls.Contains(lbl_icbver)) { groupBox3.Controls.Remove(lbl_icbver); }
            if (groupBox3.Controls.Contains(lbl_ipbver)) { groupBox3.Controls.Remove(lbl_ipbver); }
            if (groupBox3.Controls.Contains(lblL0ADJ)) { groupBox3.Controls.Remove(lblL0ADJ); }


            if (this.Controls.Contains(dgvatg))
            {
                dgvatg.Columns.Clear();
                dgvatg.Rows.Clear();
                //dgvatg.ColumnHeadersHeight = (dgvatg.ColumnHeadersHeight + 6) / 2;
                this.Controls.Remove(dgvatg);
            }
            if (dgvatg.Controls.Contains(dgvBtn)) { dgvatg.Controls.Remove(dgvBtn); }
            mvars.FormShow[4] = false;
            Form1.chkMTP.Checked = false;
            if (mvars.FormShow[5]) { Form2.i3pat.Close(); }
            if (mvars.FormShow[6]) { i3init.Close(); i3init.Dispose(); }
            //Form1.lbl_form.Text = "form1";
            mvars.lblform.Text = "form1";
        }
        private void lbl_ctime_DoubleClick(object sender, EventArgs e)
        {
            mvars.byPass = true;
        }
        private void Skeleton_ATG(int SvCols, int SvRows, int SvLBw, int SvLBh)
        {
            int SvR = 0;
            int SvC = 0;    //SvCols=mvars.NovaGamma.Length=9
            dgvatg.ReadOnly = true;
            Font f = new Font("Arial", 7);
            dgvatg.Font = f;
            //是否允許使用者自行新增
            dgvatg.AllowUserToAddRows = false;
            dgvatg.AllowUserToResizeRows = false;
            dgvatg.AllowUserToResizeColumns = false;
            for (SvC = 0; SvC < SvCols; SvC++)
            {
                if (mvars.GMAtype == "C12A")
                {
                    dgvatg.Columns.Add("Col" + (SvC).ToString(), mvars.cm603Gamma[SvC].ToString());
                    dgvatg.Columns[(SvC)].Width = 32;
                    dgvatg.Columns[(SvC)].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvatg.ColumnHeadersHeight = 22;
                dgvatg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (mvars.GMAtype == "C12A" && mvars.flgdualduty)
            {
                dgvatg.Columns.Add("Col" + (SvC).ToString(),"");
                dgvatg.Columns[(SvC)].Width = 39;
                dgvatg.Columns[(SvC)].SortMode = DataGridViewColumnSortMode.NotSortable;
                for (SvC = dgvatg.ColumnCount; SvC < SvCols * 2 + 1; SvC++)
                {
                    dgvatg.Columns.Add("Col" + (SvC).ToString(), mvars.cm603Gamma[SvC - (SvCols + 1)].ToString());
                    dgvatg.Columns[(SvC)].Width = 32;
                    dgvatg.Columns[(SvC)].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvatg.ColumnHeadersHeight = 22;
                    dgvatg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                
                dgvatg.Columns.Add("Col" + (SvC).ToString(), "");
                dgvatg.Columns[(SvC)].Width = 68;
                dgvatg.Columns[(SvC)].SortMode = DataGridViewColumnSortMode.NotSortable;
                for (SvC = dgvatg.ColumnCount; SvC < SvCols * 3 + 2; SvC++)
                {
                    dgvatg.Columns.Add("Col" + (SvC).ToString(), mvars.cm603Gamma[SvC - (SvCols + 1) * 2].ToString());
                    dgvatg.Columns[(SvC)].Width = 32;
                    dgvatg.Columns[(SvC)].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvatg.ColumnHeadersHeight = 22;
                    dgvatg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            dgvatg.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            //
            this.Controls.Add(dgvatg);
            dgvatg.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvatg.ShowCellToolTips = false;
            //
            DataGridViewRowCollection rows = dgvatg.Rows;
            for (SvR = 0; SvR < SvRows; SvR++)
            {
                rows.Add();
                dgvatg.Rows[SvR].DefaultCellStyle.WrapMode = DataGridViewTriState.True; DataGridViewRow row = dgvatg.Rows[SvR]; row.Height = 18;
                for (SvC = 0; SvC < SvCols / 2; SvC++)
                {
                    dgvatg.Rows[SvR].Cells[(SvC * 2 + 1)].Style.BackColor = Color.FromArgb(255, 192, 128);
                }

                if (mvars.GMAtype == "C12A" && mvars.flgdualduty)
                {
                    dgvatg.Rows[SvR].Cells[SvCols].Style.BackColor = Control.DefaultBackColor;
                    for (SvC = (SvCols + 2) / 2; SvC < (SvCols * 2 + 1) / 2; SvC++)
                    {
                        dgvatg.Rows[SvR].Cells[(SvC * 2 + 1)].Style.BackColor = Color.FromArgb(255, 192, 128);
                    }
                    dgvatg.Rows[SvR].Cells[SvCols * 2 + 1].Style.BackColor = Control.DefaultBackColor;
                    
                    for (SvC = (SvCols * 2 + 2) / 2; SvC < (SvCols * 3 + 1) / 2; SvC++)
                    {
                        dgvatg.Rows[SvR].Cells[(SvC * 2 + 1)].Style.BackColor = Color.FromArgb(255, 192, 128);
                    }
                }
            }
            dgvatg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvatg.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvatg.RowHeadersWidth = 68;
            dgvatg.RowHeadersDefaultCellStyle.Padding = new Padding(50);
            dgvatg.Paint += new PaintEventHandler(dgvatg_RowPostPaint);
            dgvatg.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvatg_CellClick);
            //dgvatg.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvatg_CellDoubleClick);
            dgvatg.Location = new Point(-1, btn_PAGMA.Top + btn_PAGMA.Height + 10);
            dgvatg.Size = new Size(100, 500);
            dgvatg.Width = this.Width - 20;
            dgvatg.Visible = true;
            //Button
            dgvatg.Controls.Add(dgvBtn);    //動態新增
            Point p1 = new Point(2, 2);
            dgvBtn.Location = p1;
            dgvBtn.Height = dgvatg.ColumnHeadersHeight - 1;
            dgvBtn.Width = dgvatg.RowHeadersWidth - 1;
            //dgvBtn.Click -= new EventHandler(dgvBtn_Click);
            //dgvBtn.Click += new EventHandler(dgvBtn_Click);
            dgvBtn.Visible = true;
            //ScrollBar
            dgvatg.ScrollBars = ScrollBars.Both;
            dgvatg.TopLeftHeaderCell.Value = "";
            //
            dgvatg.Rows[0].Cells[0].Selected = false;
        }
        
        void dgvatg_RowPostPaint(object sender, PaintEventArgs e)
        {
            Rectangle r1_1;
            Rectangle r1;
            StringFormat format = new StringFormat();
            for (int j = 0; j < NovaRow.Length; j++)
            {
                r1 = dgvatg.GetCellDisplayRectangle(-1, j, true); //get the column header cell
                r1.X += 1;
                r1.Y += 1;
                r1_1 = r1;
                r1_1.Width -= 2;
                r1_1.Height -= 2;
                e.Graphics.FillRectangle(new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.BackColor), r1_1);
                //StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString((NovaRow[j]).ToString(),
                dgvatg.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                //e.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(r1.X, r1.Bottom - 2), new Point(r1.X + r1.Width - 2, r1.Bottom - 2));
            }
            if (mvars.GMAtype == "C12A" && mvars.flgdualduty)
            {
                for (int j = 0; j < NovaRow.Length; j++)
                {
                    r1 = dgvatg.GetCellDisplayRectangle(19, j, true); //get the column header cell
                    r1.X += 1;
                    r1.Y += 1;
                    r1_1 = r1;
                    r1_1.Width -= 2;
                    r1_1.Height -= 2;
                    e.Graphics.FillRectangle(new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.BackColor), r1_1);
                    //StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString((NovaRow[j]).ToString() + "  ",
                    dgvatg.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                    //e.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(r1.X, r1.Bottom - 2), new Point(r1.X + r1.Width - 2, r1.Bottom - 2));
                }
            }
            

            //列上的R/G/B圖塊與文字
            Font f = new Font("Arial", 8, FontStyle.Bold);
            r1 = dgvatg.GetCellDisplayRectangle(-1, 8, true);
            r1.X += 3;
            r1.Y += 1;
            r1.Width = r1.Width / 4;
            r1.Height = r1.Height * 2 - 3;
            e.Graphics.FillRectangle(new SolidBrush(Color.Red), r1);
            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString("R", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            r1 = dgvatg.GetCellDisplayRectangle(-1, 10, true);
            r1.X += 3;
            r1.Y += 1;
            r1.Width = r1.Width / 4;
            r1.Height = r1.Height * 2 - 3;
            e.Graphics.FillRectangle(new SolidBrush(Color.Green), r1);
            e.Graphics.DrawString("G", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            r1 = dgvatg.GetCellDisplayRectangle(-1, 12, true);
            r1.X += 3;
            r1.Y += 1;
            r1.Width = r1.Width / 4;
            r1.Height = r1.Height * 2 - 3;
            e.Graphics.FillRectangle(new SolidBrush(Color.Blue), r1);
            e.Graphics.DrawString("B", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
        }
        void dgvatg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1) { dgvatg.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = false; }
        }
        

        

        private void btn_Break_Click(object sender, EventArgs e)
        {
            //string str = "GTAZB\\JiangjBen_123";
            //string[] sArray = mvars.UUT.mainGMAfilepath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (string svs in sArray)
            //{ MessageBox.Show(svs); }
            mvars.Break = true;
        }

        private void btn_PAGMA_Click(object sender, EventArgs e)
        {
            //測試用, 需關閉
            //mvars.UUT.demo = true;

            if (mvars.L0_ADJ < 0.001) { mvars.L0_ADJ = 0.01f; }     //DLvLimit

            if (mvars.GMAtype == "C12A" && mvars.flgdualduty)
            {
                if ((mvars.cm603gmaRatio[0, mvars.cm603Gamma[gmax]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[gmax]]) < 0.95) { MessageBox.Show("Autogamma will be DualDuty function but DualDuty Ratio was lost，please check the ratio", "INXPID v" + mvars.UImajor); return; }
            }


            DateTime t1 = DateTime.Now;
            int svi;
            string svs;
            int svj;
            mvars.lCount = 0;mvars.lCounts = 999;
            

            mvars.byPass = false;
            mvars.Break = false;
            btn_Break.Enabled = true;
            lst_get.Items.Clear();
            if (mvars.UUT.demo == false) { if (mCAs.CAATG.PlugSP || mCAs.CAATG.PlugIn) lst_get.Items.Add(" -> " + mCAs.CAATG.Class + "_" + mCAs.CAATG.ProbeInfo + " preCH" + mvars.UUT.CAch.ToString("00") + "  x" + mCAs.CAATG.OverBet.ToString() + " connected"); }
            else lst_get.Items.Add(" -> CA-21-_012345 preCH" + mvars.UUT.CAch.ToString("00") + "  x" + mCAs.CAATG.OverBet.ToString() + " connected");

            string value = "1";
            if (txt_UUTID.Text == "")
            {
                if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" + 
                    "    Please input UUT ID", ref value, 39) == DialogResult.Cancel) { return; }//MessageBox.Show("Please input UUT ID", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                else txt_UUTID.Text = value;
            }
            else
            {
                value = txt_UUTID.Text;
                if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                    "    Please input UUT ID", ref value, 39) == DialogResult.Cancel) { return; }
                else txt_UUTID.Text = value;
            }
            mvars.UUT.ID = txt_UUTID.Text;

            if (mp.fileGammaSetting(false) == false) { MessageBox.Show("Please check " + mvars.GMAsettingfile + " and Correction content"); }

            //lbl_SlaveAddr.Text = mvars.iSlaveAddr.ToString(); lbl_icbver.Text = "- - - -";
            lbl_PBAddr.Text = mvars.iPBaddr.ToString(); lbl_ipbver.Text = "- - - -";
            //lbl_LB.Text = mvars.iLBaddr.ToString();
            if (mvars.UUT.MTP == 1) { lbl_MTP.Text = "En MTP"; } else { lbl_MTP.Text = "DAC only"; }
            
            btn_PAGMA.Enabled = false; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;

            if (mCAs.CAATG.caSel == 0)
            {
                if (mCAs.CAATG.PlugSP == false && mCAs.CAATG.PlugIn == false && mvars.Break == false)
                {
                    if (chk_ZeroCAL.Checked == true) { svs = "UUT ID : " + txt_UUTID.Text + "\r\n" + "\r\n" + "Probe set \" 0-CAL \""; }
                    else { svs = "   UUT ID : " + txt_UUTID.Text; }//svs = "\r\n" + "\r\n" + "\r\n" + "\r\n" + "UUT ID : "; }
                    if (MessageBox.Show(svs, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        if (chk_ZeroCAL.Checked == true) lst_get.Items.Add(" --> CA-210/310 connect and \"0-CAL\".... Please wait");
                        else lst_get.Items.Add(" --> CA-210/310 connect .... Please wait");
                        mp.doDelayms(100);
                        if (mvars.UUT.demo == false) { mCAs.CAinit(); }
                        else { mp.doDelayms(2000); mCAs.msg = ""; }
                        if (mCAs.msg != "")
                        {
                            if (MessageBox.Show(mCAs.msg + "\n" + "CA init Error\r\nRetry?", "CA init", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            {
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                lst_get.Items.RemoveAt(lst_get.Items.Count - 1);

                                lst_get.Items.Add(" --> CA-210/310 connect fail，" + mCAs.msg);
                                mCAs.CAATG.PlugSP = false; mCAs.CAATG.PlugIn = false; mvars.Break = false;
                                return;
                            }
                        }
                        else
                        {
                            if (mvars.UUT.demo == false) { mCAs.CAzero(); }
                            lst_get.Items.RemoveAt(lst_get.Items.Count - 1);
                            lst_get.Items.Add(" -> " + mCAs.CAATG.Class + "_" + mCAs.CAATG.ProbeInfo + " preCH" + mvars.UUT.CAch.ToString("00") + "  x" + mCAs.CAATG.OverBet.ToString() + " connected");
                        }
                    }
                    else
                    {
                        lst_get.Items.Add(" --> Please re-AutoGamma"); mp.doDelayms(100);
                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                        return;
                    }
                }
            }
            else if (mCAs.CAATG.caSel == 1)
            {
                if (mCAs.CAATG.PlugSP == false && mCAs.CAATG.PlugIn == false && mvars.Break == false)
                {
                    svs = "   UUT ID : " + txt_UUTID.Text;
                    if (MessageBox.Show(svs, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        lst_get.Items.Add(" --> CA-410 connect .... Please wait");

                        mp.doDelayms(100);
                        if (mvars.UUT.demo == false) { mCA4.AutoConnect(); }
                        else { mp.doDelayms(2000); mCAs.msg = ""; }
                        lst_get.Items.RemoveAt(lst_get.Items.Count - 1);
                        if (mCAs.msg != "")
                        {
                            if (MessageBox.Show(mCAs.msg + "\n" + "CA init Error\r\nRetry?", "CA init", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            {
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                

                                lst_get.Items.Add(" -> CA-410 connect fail，" + mCAs.msg);
                                mCAs.CAATG.PlugSP = false; mCAs.CAATG.PlugIn = false; mvars.Break = false;
                                return;
                            }
                        }
                        else { lst_get.Items.Add(" -> " + mCAs.CAATG.Class + "_" + mCAs.CAATG.ProbeInfo + " preCH" + mvars.UUT.CAch.ToString("00") + "  x" + mCAs.CAATG.OverBet.ToString() + " connected"); }
                    }
                    else
                    {
                        lst_get.Items.Add(" --> Please re-AutoGamma"); mp.doDelayms(100);
                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                        return;
                    }
                }
                else
                {
                    if (lst_get.Items.Count > 0) { lst_get.Items.RemoveAt(lst_get.Items.Count - 1); }
                    lst_get.Items.Add(" -> " + mCAs.CAATG.Class + "_" + mCAs.CAATG.ProbeInfo + " preCH" + mvars.UUT.CAch.ToString("00") + "  x" + mCAs.CAATG.OverBet.ToString() + " connected");
                }
                if (mvars.UUT.demo == false && chk_ZeroCAL.Checked ) { lst_get.Items.Add(" --> CA-410 0-CAL"); mCA4.CAzero(); lst_get.Items.RemoveAt(lst_get.Items.Count - 1); }                
                //lst_get.Items.Add(" -> " + mCAs.CAATG.Class + "_" + mCAs.CAATG.ProbeInfo + " preCH" + mvars.UUT.CAch.ToString("00") + "  x" + mCAs.CAATG.OverBet.ToString() + " connected");
            }



            if (mvars.UUT.demo == false && mCAs.CAATG.PlugSP == false && mCAs.CAATG.PlugIn == false && mvars.Break == false)
            {
                mp.funSaveLogs("(pAGMA) CA connect fail");
                MessageBox.Show("CA connect fail", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                return;
            }
            tsb_Status.Text= "Wait .... \" Light-On the LED Board \" appeared then Power-on";
            value = "1";
            if (mp.InputBox(mvars.strUInameMe,
                "       1. Light-On the LED Board confirm LED status" + "\r\n" + "\r\n" +
                "       2. Check " + mCAs.CAATG.Class + " probe @ \" MEAS \"" + "\r\n" + "\r\n" +
                "       3. Put probe @ measurement position" + "\r\n" + "\r\n" + "\r\n" +
                " Input White-Tracking Bet ( 0.1 ~ " + (950 / mvars.UUT.CLv).ToString("#0.0") + ")"
                , ref value, 145) == DialogResult.Cancel)
            {
                lst_get.Items.Add(" -> User Canceled");
                lst_get.Items.Add(" --> Please re-AutoGamma");
                mp.funSaveLogs("(pAGMA) User break @ Input White-Tracking Bet");
                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                return;
            }
            if (mp.IsNumeric(value) == false) { value = "1"; }
            if (Convert.ToSingle(value) <= 0) { value = "1"; }
            mvars.UUT.WTLvBet = Convert.ToSingle(value);
            if (mCAs.CAATG.caSel == 0)
            {
                if (mvars.UUT.CLv * mvars.UUT.WTLvBet > 950)
                {
                    mvars.UUT.WTLvBet = Convert.ToSingle((950 / mvars.UUT.CLv).ToString("#0.0"));
                    if (MessageBox.Show("Ratio change to \"" + mvars.UUT.WTLvBet.ToString() + "\"" + "\r\n" + "\r\n" + "New CLv  " + (mvars.UUT.CLv * mvars.UUT.WTLvBet).ToString("###0.0"), mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        lst_get.Items.Add(" -> User Canceled");
                        lst_get.Items.Add(" --> Please re-AutoGamma");
                        mp.funSaveLogs("(pAGMA) User break @ Ratio change to for New CLv");
                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                        return;
                    }
                }
            }
            lbl_CLv.Text = (mvars.UUT.CLv * mvars.UUT.WTLvBet).ToString("###0.0");
            if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\")) { Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Gamma\"); }

            string svsdisk = "";
            mvars.flgDirMTP = false;
            if (File.Exists(mvars.UUT.mainGMAfilepath + @"\" + mvars.UUT.ID + @"\") == true)
            {
                DirectoryInfo currentDir = new DirectoryInfo(mvars.UUT.mainGMAfilepath + @"\" + mvars.UUT.ID + @"\");
                FileInfo[] listFile = currentDir.GetFiles(mvars.UUT.ID + "_OK.gma");
                foreach (FileInfo getInfo in listFile)
                {
                    lst_get.Items.Add(getInfo.Name);
                    svsdisk = getInfo.FullName;
                    if (MessageBox.Show(getInfo.Name + " existed，MTP-only ??" + "\r\n" + "\r\n" + "\r\n" + "                        是 (Y)   MTP-only" + "\r\n" + "\r\n" + "                        否 (N)   Run AutoGamma", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (mvars.UUT.MTP == 1) { mvars.flgDirMTP = true; }
                    }
                }
                if (svsdisk != "" && mvars.flgDirMTP == false)
                {
                    listFile = currentDir.GetFiles("*.*");
                    //刪除一個資料夾下的所有檔案以及子目錄檔案並清空空資料夾
                    foreach (FileInfo getInfo in listFile)
                    {
                        //如果有子檔案刪除檔案
                        if (File.Exists(getInfo.FullName)) { File.Delete(getInfo.FullName); }
                    }
                    //刪除空資料夾
                    Directory.Delete(mvars.UUT.mainGMAfilepath + @"\" + mvars.UUT.ID + @"\");
                }
            }
            
            if (mp.fileGammaSetting(true) == false) { MessageBox.Show("Please check " + mvars.GMAsettingfile + " and Correction content"); }
            
            btn_PAGMA.Enabled = false; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
            txt_UUTID.Enabled = btn_PAGMA.Enabled;
            
            String fnum;
            float svuutbetlv = (float)(mvars.UUT.CLv * 1.005);
            if (mvars.GMAtype == "C12A")
            {
                if (mvars.flgdualduty == false)               
                {
                    for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
                    {
                        if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 0.009) fnum = String.Format("{0:0.0000}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 1 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 0.009)
                            fnum = String.Format("{0:0.0##}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 10 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 1)
                            fnum = String.Format("{0:0.0#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 100 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 10)
                            fnum = String.Format("{0:0.#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        else
                            fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        //
                        if (svi > 0 && Convert.ToSingle(fnum) == 0) { if (svi == (mvars.cm603Gamma.Length - 1)) { dgvatg.Rows[7].Cells[svi].Value = svuutbetlv; } }
                        else { dgvatg.Rows[7].Cells[svi].Value = fnum; }
                    }
                }
            }
            
            //
            Array.Clear(mvars.pAGMA.BX, 0, mvars.pAGMA.BX.Length);    //WRGBD,0~33 [0]Blu [1]W [2]R [3]G [4]B [5]D
            Array.Clear(mvars.pAGMA.BY, 0, mvars.pAGMA.BY.Length);
            Array.Clear(mvars.pAGMA.BZ, 0, mvars.pAGMA.BZ.Length);
            Array.Clear(mvars.pAGMA.Lv, 0, mvars.pAGMA.Lv.Length);
            Array.Clear(mvars.pAGMA.Sx, 0, mvars.pAGMA.Sx.Length);
            Array.Clear(mvars.pAGMA.Sy, 0, mvars.pAGMA.Sy.Length);
            Array.Clear(mvars.pAGMA.GMAlog, 0, mvars.pAGMA.GMAlog.Length);
            //
            if (mvars.GMAtype == "C12A")
            {
                if (mvars.fileCM603Gamma(false, mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma"))
                {
                    for (svi = 0; svi <= 2; svi++)
                    {
                        //把 BANK0 搬到 BANK1
                        for (svj = 68; svj <= 85; svj++)  //0x22 ~ 0x2A
                        {
                            if (svj % 2 == 1)
                            {
                                int svbinI = mvars.cm603dfB[svi, svj - 61] * 256 + mvars.cm603dfB[svi, svj - 60];
                                string svbinS = mp.DecToBin(svbinI, 16);
                                mvars.pGMA1.Data[svi, 86 - svj - 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);  //26-9-1=16
                                mvars.pGMA1.Data[svi, 86 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      //26-9  =17
                            }
                        }
                    }
                    tsb_Status.Text = " --> DefaultGamma_cm603"; lbl_cm603Vref.Text = mvars.cm603Vref[0].ToString("##0") + "," + mvars.cm603Vref[1].ToString("##0") + "," + mvars.cm603Vref[2].ToString("##0");
                }
                else { mvars.mCM603("1"); tsb_Status.Text = " --> DefaultGamma_cm603 not exist，change to mCM603(1)"; }
            }
            svj = 0;
            if (mvars.GMAtype == "C12A") { svj = mvars.cm603Gamma.Length; }
            for (svi = 0; svi < svj; svi++)
            {
                dgvatg.Rows[8].Cells[svi].Value = mvars.pGMA.Data[0, 2 * svi + 0];
                dgvatg.Rows[9].Cells[svi].Value = mvars.pGMA.Data[0, 2 * svi + 1];
                dgvatg.Rows[10].Cells[svi].Value = mvars.pGMA.Data[1, 2 * svi + 0];
                dgvatg.Rows[11].Cells[svi].Value = mvars.pGMA.Data[1, 2 * svi + 1];
                dgvatg.Rows[12].Cells[svi].Value = mvars.pGMA.Data[2, 2 * svi + 0];
                dgvatg.Rows[13].Cells[svi].Value = mvars.pGMA.Data[2, 2 * svi + 1];
            }
            lst_get.Items.Add(tsb_Status.Text + " ..... go AutoGamma"); lst_get.TopIndex = lst_get.Items.Count - 1;
            //
            if (mvars.UUT.demo == false)
            {
                int W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                int H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                mp.ShowExtendScreen(W, H);
                if (mvars.ratioX == 1 && mvars.ratioY == 1)
                {
                    lst_get.Items.RemoveAt(lst_get.Items.Count - 1);
                    lst_get.Items.Add("No Extend Screen");
                    lst_get.Items.Add(" --> Please re-AutoGamma"); lst_get.TopIndex = lst_get.Items.Count - 1;
                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                    return;
                }
                else
                {
                    mvars.TuningArea.rX = mvars.ratioX;
                    mvars.TuningArea.rY = mvars.ratioY;
                    Form2.i3pat = new i3_Pat();
                    Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                    Form2.i3pat.BackColor = Color.Blue;
                    Form2.i3pat.Show();
                    Form2.i3pat.Visible = false;
                    Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                    Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                    Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                    //i3pat.Show();
                    Form2.i3pat.TopMost = true;
                    Form2.i3pat.Visible = true;
                    //
                    if (Form2.i3pat != null)
                    {
                        Form2.fileTuningArea(false);
                        Form2.i3pat.lbl_Mark.Width = mvars.TuningArea.tW;
                        Form2.i3pat.lbl_Mark.Height = mvars.TuningArea.tH;
                        Form2.i3pat.lbl_Mark.Location = new Point(mvars.TuningArea.mX, mvars.TuningArea.mY);
                        Form2.i3pat.lbl_Mark.ForeColor = Color.White;
                        Form2.i3pat.lbl_Mark.BackColor = Color.FromArgb(80, 80, 80);
                        Form2.i3pat.lbl_Mark.Visible = true;
                        Application.DoEvents();
                    }
                }
            }
            else // demo mode
            {
                int W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                int upperBound;
                Screen[] screens = Screen.AllScreens;
                upperBound = screens.GetUpperBound(0);
                upperBound = 0;
                if (upperBound == 0)
                {
                    string Svs = screens[upperBound].Bounds.ToString();
                    if (Svs.Substring(Svs.Length - 1, 1) == "}") Svs = Svs.Substring(0, Svs.Length - 1);
                    string[] Svss = Svs.Split(',');
                    foreach (var word in Svss)
                    {
                        if (word.IndexOf("Width=", 0) != -1)
                        {
                            string[] Svsss = word.Split('=');
                            mvars.UUT.resW = Convert.ToSingle(Svsss[1]);
                        }
                        else if (word.IndexOf("Height=", 0) != -1)
                        {
                            string[] Svsss = word.Split('=');
                            mvars.UUT.resH = Convert.ToSingle(Svsss[1]);
                        }
                    }
                    mvars.ratioX = 1;
                    mvars.ratioY = 1;
                }
                mvars.TuningArea.rX = mvars.ratioX;
                mvars.TuningArea.rY = mvars.ratioY;
                Form2.i3pat = new i3_Pat();
                Form2.i3pat.BackColor = Color.Blue;
                Form2.i3pat.Show();
                Form2.i3pat.Visible = false;
                Form2.i3pat.Location = new System.Drawing.Point(W / 5 * 3, 500);
                Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW / 10), Convert.ToInt16(mvars.UUT.resW / 10));
                //i3pat.Show();
                Form2.i3pat.TopMost = true;
                Form2.i3pat.Visible = true;
                //
                if (Form2.i3pat != null)
                {
                    Form2.fileTuningArea(false);
                    Form2.i3pat.lbl_Mark.Size = Form2.i3pat.Size;
                    Form2.i3pat.lbl_Mark.BorderStyle = BorderStyle.FixedSingle;

                    Form2.i3pat.lbl_Mark.Font = new Font("Arial", 16);
                    Form2.i3pat.lbl_Mark.Location = new Point(0, 0);
                    Form2.i3pat.lbl_Mark.ForeColor = Color.White;
                    Form2.i3pat.lbl_Mark.BackColor = Color.FromArgb(80, 80, 80);
                    Form2.i3pat.lbl_Mark.Visible = true;
                    Application.DoEvents();
                }
            }
            //
            if (mvars.FormShow[6] == false) { i3init = new INXPID.i3_Init(); i3init.Show(); i3init.Left = 300; }
            i3init.Visible = true;
            mvars.msgA = "";
            mvars.mvWn = 0;
            mvars.msWn = "Please confirm information @ FullDarkPattern" + "\r\n" + "\r\n" +
                "    Meter: " + mCAs.CAATG.Class + " CH." + mvars.UUT.CAch.ToString("00") +
                " x" + mCAs.CAATG.OverBet + "\r\n" + "    UUT.ID: " + mvars.UUT.ID + "\r\n" +
                "    Resolution: " + mvars.UUT.resW.ToString() + "x" + mvars.UUT.resH.ToString() + "\r\n" + "\r\n" +
                "    CAUTION        " + lbl_MTP.Text + " Mode" + "\r\n" +
                "    CAUTION        Final judge with GammaRead Mode" + "\r\n" + "\r\n" +
                "    Show DarkPattern and Stanby ! ";
            i3_Init.lbl_1.Text = mvars.msWn;
            i3_Init.lbl_1.Visible = true;
            i3init.tme_Warn.Enabled = true;
            do
            {
                Application.DoEvents();
                mp.doDelayms(100);
            } while (mvars.msgA == "");
            if (mvars.msgA == "0")
            {
                lst_get.Items.Add(" User Cancel @ CoolCounter");
                lst_get.Items.Add(" --> Please re-AutoGamma"); lst_get.TopIndex = lst_get.Items.Count - 1;
                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                if (mvars.FormShow[5]) Form2.i3pat.lbl_Mark.Text = "";Form2.i3pat.Close();
                return;
            }
            if (mvars.UUT.MTP == 1)
            {
                mp.funSaveLogs("(ATG MTP) ID." + mvars.UUT.ID + "," + mCAs.CAATG.Class + "(" + mCAs.CAATG.ProbeSN + ")Ch." + mvars.UUT.CAch + " x" + mCAs.CAATG.OverBet + " Warm" + (mvars.mvWn - 1).ToString() + "s Target " + mvars.UUT.Cx + "," + mvars.UUT.Cy + "," + mvars.UUT.CLv + " Saveto " + mvars.UUT.mainGMAfilepath);
            }
            else
            {
                mp.funSaveLogs("(ATG DAC) ID." + mvars.UUT.ID + "," + mCAs.CAATG.Class + "(" + mCAs.CAATG.ProbeSN + ")Ch." + mvars.UUT.CAch + " x" + mCAs.CAATG.OverBet + " Warm" + (mvars.mvWn - 1).ToString() + "s Target " + mvars.UUT.Cx + "," + mvars.UUT.Cy + "," + mvars.UUT.CLv + " Saveto " + mvars.UUT.mainGMAfilepath);
            }
            if (mvars.FormShow[5]) Form2.i3pat.lbl_Mark.Text = "";

            //mvars.WndProcStr = "99@@" + mvars.strUInameMe + ",PAGMA," + mvars.iSlaveAddr + "," + mvars.GMAtype;
            //Form1.lst_MsgIn.Items.Add(mvars.WndProcStr);
            //Form1.lst_MsgIn.TopIndex = Form1.lst_MsgIn.Items.Count - 1;
            //do
            //{
            //    Application.DoEvents();
            //    mp.DelayWait(100);
            //} while (mvars.lCount < mvars.lCounts);
            //for (int i = 0; i < mvars.lCounts; i++) { if (mvars.lGet[i].IndexOf("ERROR", 0) != -1) { tsb_Status.Text = "ERROR"; break; } }
            //if (tsb_Status.Text.IndexOf("ERROR", 0) == -1) { lst_get.Items.Add("AutoGamma,DONE,1@ " + ((DateTime.Now - t1).TotalMilliseconds / 1000).ToString("###0.0") + "s"); }
            //else { lst_get.Items.Add("AutoGamma,ERROR,1,errCode" + mvars.ATGerr); }
            //lst_get.TopIndex = lst_get.Items.Count - 1;

            mvars.flgSelf = true;
            //mp.cCM603AGMA_mk2();
            if (mvars.strReceive.IndexOf("DONE", 0) > -1)
            {
                string[] svss = mvars.strReceive.Split(',');
                Form4.lst_get.Items.Add("  ↑ ATG DONE，" + svss[7] + "s");
            }
            else
            {
                string[] svss = mvars.strReceive.Split(',');
                Form4.lst_get.Items.Add("  ↑ ATG Fail，" + svss[7] + "s，errCode" + mvars.errCode);
            }
            btn_PAGMA.Enabled = true;
            txt_UUTID.Enabled = true;
            btn_Break.Enabled = false;
        }
        private void btn_PAGMA_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (mvars.GMAtype == "C12A" && mvars.flgdualduty)
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Title = "Select csv file";
                    dialog.InitialDirectory = mvars.strStartUpPath;
                    dialog.FileName = "";
                    dialog.Filter = "csv files (*.*)|*.csv";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        StreamReader sTAread = File.OpenText(dialog.FileName);
                        int svi = 0; bool svb = false;
                        while (true)
                        {
                            string data = sTAread.ReadLine();
                            if (data == null) { svi -= 1; break; }
                            if (svb == false && data.Substring(0, "0,0,0,0".Length) == "0,0,0,0") svb = true;
                            if (svb)
                            {
                                string[] Svss = data.Split(',');
                                mvars.cm603gmaRatio[0, svi] = Convert.ToSingle(Svss[2]); mvars.cm603gmaRatio[1, svi] = Convert.ToSingle(Svss[3]);
                                svi += 1;
                            }
                        }
                        if (svi != 255) { MessageBox.Show("Gray count not match 0 ~ 255", mvars.strUInameMe + "_UI v" + mvars.UImajor); }
                        string svs;
                        byte svDs = 2;
                        for (svDs = 0; svDs <= 1; svDs++)
                        {
                            for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
                            {
                                if (mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]] < 0.009)
                                    svs = String.Format("{0:0.0000}", mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]]);
                                else if (mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]] < 1 && (mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]]) >= 0.009)
                                    svs = String.Format("{0:0.0##}", mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]]);
                                else if (mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]] < 10 && (mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]]) >= 1)
                                    svs = String.Format("{0:0.0#}", mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]]);
                                else if (mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]] < 100 && (mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]]) >= 10)
                                    svs = String.Format("{0:0.#}", mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]]);
                                else
                                    svs = String.Format("{0:###0}", mvars.UUT.CLv * 2.005 * mvars.cm603gmaRatio[svDs, mvars.cm603Gamma[svi]]);

                                dgvatg.Rows[7].Cells[svi + svDs * (mvars.cm603Gamma.Length + 1)].Value = svs;
                            }
                        }
                        for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
                        {
                            if (mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]) < 0.009) svs = String.Format("{0:0.0000}", mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]));
                            else if (mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]) < 1 && mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]) >= 0.009)
                                svs = String.Format("{0:0.0##}", mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]));
                            else if (mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]) < 10 && mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]) >= 1)
                                svs = String.Format("{0:0.0#}", mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]));
                            else if (mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]) < 100 && mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]) >= 10)
                                svs = String.Format("{0:0.#}", mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]));
                            else
                                svs = String.Format("{0:###0}", mvars.UUT.CLv * 1.005 * (mvars.cm603gmaRatio[0, mvars.cm603Gamma[svi]] + mvars.cm603gmaRatio[1, mvars.cm603Gamma[svi]]));

                            dgvatg.Rows[7].Cells[svi + svDs * (mvars.cm603Gamma.Length + 1)].Value = svs;
                        }
                        sTAread.Close();
                    }
                } 
            }
        }

        private void lbl_cm603Vref_DoubleClick(object sender, EventArgs e)
        {
            string value = lbl_cm603Vref.Text;
            if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                    "    Please input Vref(R,G,B)", ref value, 39) == DialogResult.Cancel) { return; }//MessageBox.Show("Please input UUT ID", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            else
            {
                string[] Svss = value.Split(',');
                if (Svss.Length != 3) { return; }
                else
                {
                    for (int svi = 0; svi <= 2; svi++)
                    {
                        mvars.cm603Vref[svi] = Convert.ToSingle(Svss[svi]);
                        mvars.cm603VrefCode[svi] = Convert.ToInt16(mvars.cm603Vref[svi] / 0.01953);

                        UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(mvars.cm603VrefCode[svi]), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        string svbinS = "00" + (mp.DecToBin((int)ulCRC, 16).Substring(2, 4)) + mp.DecToBin(mvars.cm603VrefCode[svi], 10);
                        mvars.cm603df[svi, (byte)0x1F * 2] = mp.BinToHex(svbinS.Substring(0, 8), 2);
                        mvars.cm603df[svi, (byte)0x1F * 2 + 1] = mp.BinToHex(svbinS.Substring(8, 8), 2);
                        mvars.cm603dfB[svi, (byte)0x1F * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, (byte)0x1F * 2]);
                        mvars.cm603dfB[svi, (byte)0x1F * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, (byte)0x1F * 2 + 1]);
                    }
                    lbl_cm603Vref.Text = value;
                }
            }
        }

        private void lbl_GMAvalue_DoubleClick(object sender, EventArgs e)
        {
            if (mvars.UUT.demo)
            {
                string value = mvars.GMAvalue.ToString();
                if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                    "    New Gamma value", ref value, 39) == DialogResult.Cancel) { return; }//MessageBox.Show("Please input UUT ID", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                else
                {
                    if (mp.IsNumeric(value))
                    {
                        if (Convert.ToSingle(value) >= 1 && Convert.ToSingle(value) <=4) mvars.GMAvalue = Convert.ToSingle(value); lbl_GMAvalue.Text = value;
                    }
                }           
            }
        }

        private void txt_UUTID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { btn_PAGMA_Click(null, null); }
        }
    }
}
