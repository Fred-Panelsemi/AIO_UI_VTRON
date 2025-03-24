using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;   //Regex

using System.IO;

namespace AIO
{
    public partial class uc_atg : UserControl
    {
        readonly string[] NovaRow = { "Gamma", "WLv", "Wx", "Wy", "△Lv %", "△x", "△y", "Target WLv", "[10:8]", "[7:0]", "[10:8]", "[7:0]", "[10:8]", "[7:0]" };
        readonly string[] bk1Row = { "r", "WL", "Wx", "Wy", "△Lv", "△x", "△y", "", "", "", "", "", "", "" };
        public static ListBox lstget = null;
        public static i3_Init i3init = null;
        public static Label lblL0ADJ = null;
        public static Label lblDUTY = null;
        public static DataGridView dgvatg = null;
        public static Label lblctime = null;
        public static Label lblPBAddr = null;
        public static Label lblcount = new Label();
        public static Label lbldLvlimit = null;
        public static Label lbldLvmax = null;
        public static TextBox txtUUTID = null;

        public static Label lblamb = null;
        public static float[] svG0volt = new float[] { 0.1f, 0.1f, 0.1f };

        public static Button btnBreak = null;
        public static Label lblnext = null;
        public static Button btnPAGMA = null;

        public uc_atg()
        {
            InitializeComponent();
            lstget = lst_get;
            lblL0ADJ = lbl_L0ADJ;
            lblDUTY = lbl_DUTY;
            dgvatg = dgv_ATG;
            lblctime = lbl_ctime;
            lblPBAddr = lbl_PBAddr;
            lblcount = lbl_count;
            txtUUTID = txt_UUTID;

            lbldLvlimit = lbl_dLvlimit;
            lbldLvmax = lbl_dLvmax;
            lblamb = lbl_amb;

            btnBreak = btn_Break;
            btnPAGMA = btn_PAGMA;
            lblnext = lbl_next;
            lblnext.Location = lst_get.Location;
            lblnext.Size = lstget.Size;
        }


        private void uc_atg_Load(object sender, EventArgs e)
        {
            if (mvars.UIinTest == 0)
            {
                mvars.UUT.DLvLimit = 0.003f;        //1223 Release
                mvars.UUT.DLvTolminus = 0.0005f;    //1223 Release
                mvars.UUT.DLvTolplus = 0.0005f;     //1223 Release

                mvars.UUT.DLvLimit = 0.001f;        //20220406 Release
                mvars.UUT.DLvTolminus = 0.001f;     //20220406 Release
                mvars.UUT.DLvTolplus = 0.0005f;     //20220406 Release

                mvars.UUT.DLvLimit = 0.0005f;       //20230525 Release
                mvars.UUT.DLvTolminus = 0.0005f;    //20230525 Release
                mvars.UUT.DLvTolplus = 0.0005f;     //20230525 Release

            }
            else if (mvars.UIinTest == 1)
            {
                mvars.UUT.DLvLimit = 0.001f;        //20220120 Release
                mvars.UUT.DLvTolminus = 0.001f;     //20220120 Release
                mvars.UUT.DLvTolplus = 0.0005f;     //20220120 Release

                mvars.UUT.DLvLimit = 0.0005f;       //20230525 Release
                mvars.UUT.DLvTolminus = 0.0005f;    //20230525 Release
                mvars.UUT.DLvTolplus = 0.0005f;     //20230525 Release

                mvars.UUT.DLvLimit = 0.001f;       //20230627 TV130 try
                mvars.UUT.DLvTolminus = 0.001f;    //20230627 TV130 try
                mvars.UUT.DLvTolplus = 0.0005f;     //20230627 TV130 try


            }

            //if (File.Exists(mvars.defaultGammafile) == false) 
            //{
            //    string[] svfile = mvars.defaultGammafile.Split('\\');
            //    if (MultiLanguage.DefaultLanguage == "en-US") Form1.lstget1.Items.Add(svfile[svfile.Length - 1] + "un found");
            //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") Form1.lstget1.Items.Add("沒有" + svfile[svfile.Length - 1] + "檔案");
            //    else if (MultiLanguage.DefaultLanguage == "zh-CN") Form1.lstget1.Items.Add("没有" + svfile[svfile.Length - 1] + "档案");
            //    return; 
            //}
            //else
            //{
            //    if (mvars.ICver >= 5)
            //    {
            //        if (mvars.deviceID.Substring(0, 2) == "05") { mvars.mCM603P("0"); }
            //        else if (mvars.deviceID.Substring(0, 2) == "04" && mvars.deviceNameSub == "B(4)") { mvars.mCM603B4("0"); }
            //        else if (mvars.deviceID.Substring(0, 2) == "04" && mvars.deviceNameSub == "B(4t)") { mvars.mCM603B4t("0"); }
            //        else
            //        {
            //            MessageBox.Show("deviceID and cm603 default is not match", mvars.strUInameMe + "v" + mvars.UImajor);
            //            return;
            //        }
            //        if (mp.fileDefaultGammaV(false) == false)
            //        {
            //            MessageBox.Show(mvars.defaultGammafile + " is read error", mvars.strUInameMe + "v" + mvars.UImajor);
            //            return;
            //        }
            //        lbl_cm603Vref.Text = mvars.cm603Vref[0].ToString("##0.0") + "," + mvars.cm603Vref[1].ToString("##0.0") + "," + mvars.cm603Vref[2].ToString("##0.0") + "," + mvars.cm603Vref[3].ToString("##0.0");
            //    }
            //    else
            //    {
            //        lbl_cm603Vref.Text = mvars.cm603Vref[0].ToString("##0.0") + "," + mvars.cm603Vref[1].ToString("##0.0") + "," + mvars.cm603Vref[2].ToString("##0.0");
            //        mvars.mCM603("0");
            //        //  待建立 mp.fileDefaultGamma(false, mvars.defaultGammafile);
            //    }
            //}

            if (mvars.ICver < 5) lbl_cm603Vref.Text = mvars.cm603Vref[0].ToString("##0.0") + "," + mvars.cm603Vref[1].ToString("##0.0") + "," + mvars.cm603Vref[2].ToString("##0.0");
            else
            {
                if (mvars.deviceID.Substring(0, 2) == "10")
                    lbl_cm603Vref.Text = mvars.cm603Vref[0].ToString("##0.0") + "," + mvars.cm603Vref[1].ToString("##0.0") + "," + mvars.cm603Vref[2].ToString("##0.0");
                else
                    lbl_cm603Vref.Text = mvars.cm603Vref[0].ToString("##0.0") + "," + mvars.cm603Vref[1].ToString("##0.0") + "," + mvars.cm603Vref[2].ToString("##0.0") + "," + mvars.cm603Vref[3].ToString("##0.0");
            }
            int svi = 0;
            lbl_Cx.Text = mvars.UUT.Cx.ToString();
            lbl_Cy.Text = mvars.UUT.Cy.ToString();
            lbl_CLv.Text = (mvars.UUT.CLv / 0.95).ToString("0000");
            lbl_CLv.Text = mvars.UUT.CLv.ToString();
            lbl_CACh.Text = mvars.UUT.CAch.ToString("00") + "x" + mvars.UUT.OverBet;
            if (mvars.UUT.GMAposATD == 0) { lbl_PBAddr.Text = Form1.cmbhPB.Text; }
            else if (mvars.UUT.GMAposATD == 1) { lbl_PBAddr.Text = "Auto"; }
            if (mvars.UUT.MTP == 1) { lbl_MTP.Text = "enMTP"; }
            else { lbl_MTP.Text = "DAC only"; }

            lbl_VREF.Text = "VREF " + mvars.UUT.VREF + "v "+ svG0volt[0] + "," + svG0volt[1] + "," + svG0volt[2];

            #region tooltip
            mvars.toolTip1.AutoPopDelay = 3000;
            mvars.toolTip1.InitialDelay = 500;
            mvars.toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            mvars.toolTip1.ShowAlways = true;
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    mvars.toolTip1.SetToolTip(btn_PAGMA, @"FPGA 61.3x(up) singleduty，66.xx test，61.4x↑ new");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    mvars.toolTip1.SetToolTip(btn_PAGMA, @"FPGA 61.3x(以上) singleduty，66.xx 測試，61.4x(含以上)反向");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    mvars.toolTip1.SetToolTip(btn_PAGMA, @"FPGA 61.3x(以上) singleduty，66.xx 測試，61.4x(含以上)反向");
                }
            }
            #endregion tooltip

            #region 條件 mvars.deviceID == "02" || mvars.deviceID == "03" || mvars.deviceID == "04"
            if (mCAs.CAATG.CAsel == 0) 
            {
                lbl_Info.Text = "Gamma            ref. volt.                                     CA Ch.             iPBaddr." + "\r\n" +
                                "Cx                    ±" + "\r\n" +
                                "Cy                    ±                    UUT id" + "\r\n" +
                                "CLv >                ±" + "\r\n" +
                                "DLv                   ~" + "\r\n";
            }
            else if (mCAs.CAATG.CAsel == 1) 
            { 
                lbl_Info.Text = "Gamma            ref. volt.                                     C4 Ch.             iPBaddr." + "\r\n" +
                                "Cx                    ±" + "\r\n" +
                                "Cy                    ±                    UUT id" + "\r\n" +
                                "CLv >                x" + "\r\n" +
                                "DLv                   ~" + "\r\n";
            }
            else if (mCAs.CAATG.CAsel >= 254)
            {
                lbl_CACh.Visible = false;
                lbl_Info.Text = "Gamma            ref. volt.                         demoMeter Ch.demo    iPBaddr." + "\r\n" +
                                "Cx                    ±" + "\r\n" +
                                "Cy                    ±                    UUT id" + "\r\n" +
                                "CLv >                ±" + "\r\n" +
                                "DLv                   ~" + "\r\n";
            }

            if (mvars.flgex20d10) { lbl_Info.Text += "ex20d10 "; lbl_ex20d10.Text = mvars.UUT.ex20d10[0] + "~" + mvars.UUT.ex20d10[1] + "~" + mvars.UUT.ex20d10[2] + "~" + mvars.UUT.ex20d10[3]; }

            lbl_dLvlimit.Text = string.Format("{0:#0.0####}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus));
            lbl_dLvmax.Text = string.Format("{0:#0.0####}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus));

            lbl_cm603Vref.Visible = true;

            if (mvars.dualduty == 1) Skeleton_ATG(mvars.cm603Gamma.Length * 2 + 1, NovaRow.Length, 480, 270);
            else Skeleton_ATG(mvars.cm603Gamma.Length, NovaRow.Length, 480, 270);

            lbl_GMAvalue.Text = mvars.GMAvalue.ToString();

            lbl_DUTY.Text = "bk0:" + mvars.duty[0];
            for (svi = 1; svi < mvars.duty.Length; svi++)
            {
                lbl_DUTY.Text += "，bk1:" + mvars.duty[1];
            }
            //過渡版 R02H=RM93C90v05-RVGMA2 , G02H=RM93C90v05-GVGMA2 , B02H=RM93C90v05-BVGMA2
            //for (svi = 0; svi < 3; svi++)
            //{
            //    for (int svg = 0; svg < 18; svg += 2)
            //    {
            //        int sH = 18 - svg - 2;
            //        int sL = 18 - svg - 1;
            //        mvars.cm603dfB[svi, svg + 4] = (byte)mp.HexToDec(mvars.cm603df[svi, svg + 4]);
            //        mvars.cm603dfB[svi, svg + 5] = (byte)mp.HexToDec(mvars.cm603df[svi, svg + 5]);
            //        int svbinI = mvars.cm603dfB[svi, svg + 4] * 256 + mvars.cm603dfB[svi, svg + 5];
            //        string svbinS = mp.DecToBin(svbinI, 16);
            //        string svH = mp.BinToHex(svbinS.Substring(6, 10), 4);
            //        mvars.pGMA.Data[svi, sH] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);
            //        mvars.pGMA.Data[svi, sL] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);
            //    }
            //}

            //int svuutbetlv = Convert.ToInt16(Convert.ToSingle(lbl_CLv.Text) / 0.95f);
            int svuutbetlv = int.Parse(lbl_CLv.Text);
            string fnum;
            for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
            {
                dgvatg.Rows[8].Cells[svi].Value = mvars.pGMA[0].Data[0, 2 * svi + 0];
                dgvatg.Rows[9].Cells[svi].Value = mvars.pGMA[0].Data[0, 2 * svi + 1];
                dgvatg.Rows[10].Cells[svi].Value = mvars.pGMA[0].Data[1, 2 * svi + 0];
                dgvatg.Rows[11].Cells[svi].Value = mvars.pGMA[0].Data[1, 2 * svi + 1];
                dgvatg.Rows[12].Cells[svi].Value = mvars.pGMA[0].Data[2, 2 * svi + 0];
                dgvatg.Rows[13].Cells[svi].Value = mvars.pGMA[0].Data[2, 2 * svi + 1];


                if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 100)
                    fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 100 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 10)
                    fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 10 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 1)
                    fnum = String.Format("{0:###0.#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 1 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 0.1)
                    fnum = String.Format("{0:0.##}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                else 
                    fnum = String.Format("{0:0.###}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                dgvatg.Rows[7].Cells[svi].Value = fnum;
            }
            if (mvars.dualduty == 1)
            {
                for (svi = mvars.cm603Gamma.Length+1; svi < mvars.cm603Gamma.Length*2+1; svi++)
                {
                    dgvatg.Rows[8].Cells[svi].Value = mvars.pGMA[1].Data[0, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 0];
                    dgvatg.Rows[9].Cells[svi].Value = mvars.pGMA[1].Data[0, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 1];
                    dgvatg.Rows[10].Cells[svi].Value = mvars.pGMA[1].Data[1, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 0];
                    dgvatg.Rows[11].Cells[svi].Value = mvars.pGMA[1].Data[1, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 1];
                    dgvatg.Rows[12].Cells[svi].Value = mvars.pGMA[1].Data[2, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 0];
                    dgvatg.Rows[13].Cells[svi].Value = mvars.pGMA[1].Data[2, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 1];
                    dgvatg.Rows[7].Cells[svi].Value = dgvatg.Rows[7].Cells[svi- mvars.cm603Gamma.Length - 1].Value;
                }
            }
            
            for (svi = 0; svi < dgvatg.ColumnCount; svi++) { dgvatg.Rows[7].Cells[svi].Style.BackColor = Color.White; }
            dgvatg.Refresh();
            if (mvars.deviceNameSub == "B(4)" || mvars.deviceID.Substring(0, 2) == "05")
            {
                lblL0ADJ.Text = mvars.cgma1dv[mvars.dualduty, 0].ToString() + "v " + mvars.cgma1dv[mvars.dualduty, 1].ToString() + "v " + mvars.cgma1dv[mvars.dualduty, 2].ToString() + "v";
            }
            else
            {
                lblL0ADJ.Text = mvars.deltadtXYZdg[0].ToString() + "," + mvars.deltadtXYZdg[1].ToString() + "," + mvars.deltadtXYZdg[2].ToString();
            }
            if (mCAs.CAATG.CAsel == 1) { chk_ZeroCAL.Checked = true; }
            #endregion 條件 mvars.deviceID == "02" || mvars.deviceID == "03" || mvars.deviceID == "04"

            //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
            Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
            lstget.Width = Form1.pnlfrm1.Width - lstget.Left - 15;
            lblctime.Width = lstget.Width;
            lblnext.Width = lstget.Width;
            lblnext.Text = "";
            mvars.FormShow[10] = true;
            mvars.actFunc = "AutoGamma";
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
                if (SvC < mvars.cm603Gamma.Length) { dgvatg.Columns.Add("Col" + SvC.ToString(), mvars.cm603Gamma[SvC].ToString()); dgvatg.Columns[SvC].Width = 40; }
                if (mvars.dualduty == 1 && SvC >= mvars.cm603Gamma.Length)
                {
                    if (SvC == mvars.cm603Gamma.Length) { dgvatg.Columns.Add("Col" + SvC.ToString(), ""); dgvatg.Columns[SvC].Width = 30; }
                    else { dgvatg.Columns.Add("Col" + SvC.ToString(), mvars.cm603Gamma[SvC - (mvars.cm603Gamma.Length + 1)].ToString()); dgvatg.Columns[SvC].Width = 40; }
                }
                dgvatg.Columns[SvC].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvatg.ColumnHeadersHeight = 22;
                dgvatg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvatg.Columns[0].Width = 44;
            if (mvars.dualduty == 1)
            {
                dgvatg.Columns[mvars.cm603Gamma.Length].DefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64);
                dgvatg.Columns[mvars.cm603Gamma.Length + 1].Width = 44;
            }

            //if (SvC< mvars.cm603Gamma.Length) dgvatg.Columns.Add("Col" + (SvC).ToString(), mvars.cm603Gamma[SvC].ToString()); else
            //    {
            //        dgvatg.Columns.Add("Col" + (SvC).ToString(), "");
            //    }
            //    if (SvC < 1) dgvatg.Columns[(SvC)].Width = 44;
            //    else dgvatg.Columns[(SvC)].Width = 40;
            //    dgvatg.Columns[(SvC)].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    dgvatg.ColumnHeadersHeight = 22;
            //    dgvatg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //}


            dgvatg.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);

            dgvatg.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvatg.ShowCellToolTips = false;
            
            DataGridViewRowCollection rows = dgvatg.Rows;
            for (SvR = 0; SvR < SvRows; SvR++)
            {
                rows.Add();
                dgvatg.Rows[SvR].DefaultCellStyle.WrapMode = DataGridViewTriState.True; DataGridViewRow row = dgvatg.Rows[SvR]; row.Height = 18;
                for (SvC = 0; SvC < mvars.cm603Gamma.Length / 2; SvC++)
                {
                    dgvatg.Rows[SvR].Cells[(SvC * 2 + 1)].Style.BackColor = Color.FromArgb(255, 192, 128);
                }
                if (mvars.dualduty == 1)
                {
                    for (SvC = mvars.cm603Gamma.Length + 1; SvC < SvCols; SvC += 2)
                    {
                        dgvatg.Rows[SvR].Cells[SvC].Style.BackColor = Color.FromArgb(255, 192, 128);
                    }
                }
            }
            dgvatg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvatg.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvatg.RowHeadersWidth = 72;
            dgvatg.RowHeadersDefaultCellStyle.Padding = new Padding(50);
            dgvatg.Paint += new PaintEventHandler(dgvatg_RowPostPaint);
            dgvatg.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvatg_CellClick);
            //dgvatg.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvatg_CellDoubleClick);
            dgvatg.Location = new Point(10, btn_PAGMA.Top + btn_PAGMA.Height + 15);
            dgvatg.Size = new Size(100, 280);
            dgvatg.Width = this.Width - 100;
            dgvatg.Width = Form1.pnlfrm1.Width - 25;
            dgvatg.Visible = true;
            //Button
            //Point p1 = new Point(dgvatg.Left, dgvatg.Top);
            //dgvBtn.Location = dgvatg.Location;
            //dgvBtn.Height = dgvatg.ColumnHeadersHeight - 1;
            //dgvBtn.Width = dgvatg.RowHeadersWidth - 1;
            //dgvBtn.Click -= new EventHandler(dgvBtn_Click);
            //dgvBtn.Click += new EventHandler(dgvBtn_Click);
            //dgvBtn.Visible = true;
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
                r1 = dgvatg.GetCellDisplayRectangle(-1, j, false); //get the column header cell
                r1.X += 1;
                r1.Y += 1;
                r1_1 = r1;
                r1_1.Width -= 2;
                r1_1.Height -= 2;
                e.Graphics.FillRectangle(new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.BackColor), r1_1);
                //StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(NovaRow[j],
                dgvatg.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                //e.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(r1.X, r1.Bottom - 2), new Point(r1.X + r1.Width - 2, r1.Bottom - 2));
                //if (mvars.dualduty) dgvatg.Rows[j].Cells[mvars.cm603Gamma.Length].Style.BackColor = Control.DefaultBackColor;
            }
            if (mvars.dualduty == 1)
            {
                for (int j = 0; j < NovaRow.Length; j++)
                {
                    r1 = dgvatg.GetCellDisplayRectangle(mvars.cm603Gamma.Length, j, false); //get the column header cell
                    r1.X += 1;
                    r1.Y += 1;
                    r1_1 = r1;
                    r1_1.Width -= 2;
                    r1_1.Height -= 2;
                    //e.Graphics.FillRectangle(new SolidBrush(), r1_1);
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(bk1Row[j],
                    dgvatg.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(Color.FromArgb(255,255,255)), r1, format);
                }
            }
            //列上的R/G/B圖塊與文字
            Font f = new Font("Arial", 8, FontStyle.Bold);
            format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };    
            
            r1 = dgvatg.GetCellDisplayRectangle(-1, 8, true);
            r1.X += 3;
            r1.Y += 1;         
            r1.Height = r1.Height * 2 - 3;
            r1.Width /= 4;
            e.Graphics.FillRectangle(new SolidBrush(Color.Red), r1);
            e.Graphics.DrawString("R", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

            r1 = dgvatg.GetCellDisplayRectangle(-1, 10, true);
            r1.X += 3;
            r1.Y += 1;
            r1.Width /= 4;
            r1.Height = r1.Height * 2 - 3;
            e.Graphics.FillRectangle(new SolidBrush(Color.Green), r1);
            e.Graphics.DrawString("G", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

            r1 = dgvatg.GetCellDisplayRectangle(-1, 12, true);
            r1.X += 3;
            r1.Y += 1;
            r1.Width /= 4;
            r1.Height = r1.Height * 2 - 3;
            e.Graphics.FillRectangle(new SolidBrush(Color.Blue), r1);
            e.Graphics.DrawString("B", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

            if (mvars.dualduty == 1)
            {
                r1 = dgvatg.GetCellDisplayRectangle(mvars.cm603Gamma.Length, 8, true);
                r1.X += 3;
                r1.Y += 1;
                r1.Height = r1.Height * 2 - 3;
                r1.Width -= 8;
                e.Graphics.FillRectangle(new SolidBrush(Color.Red), r1);
                e.Graphics.DrawString("R", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

                r1 = dgvatg.GetCellDisplayRectangle(mvars.cm603Gamma.Length, 10, true);
                r1.X += 3;
                r1.Y += 1;
                r1.Width -= 8;
                r1.Height = r1.Height * 2 - 3;
                e.Graphics.FillRectangle(new SolidBrush(Color.Green), r1);
                e.Graphics.DrawString("G", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

                r1 = dgvatg.GetCellDisplayRectangle(mvars.cm603Gamma.Length, 12, true);
                r1.X += 3;
                r1.Y += 1;
                r1.Width -= 8;
                r1.Height = r1.Height * 2 - 3;
                e.Graphics.FillRectangle(new SolidBrush(Color.Blue), r1);
                e.Graphics.DrawString("B", f, new SolidBrush(dgvatg.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            

            //列上的最高階數(非可控點)的畫大叉
            //if (mvars.GMAtype == "S12A")
            //{
            //    r1 = dgvatg.GetCellDisplayRectangle(34, 0, true); //get the column header cell
            //    r1_1 = dgvatg.GetCellDisplayRectangle(34, 7, true);
            //    e.Graphics.DrawLine(new Pen(Color.Brown), new Point(r1.X, r1_1.Bottom - 2), new Point(r1.X + r1.Width - 2, r1.Y));
            //    e.Graphics.DrawLine(new Pen(Color.Brown), new Point(r1.X, r1.Y), new Point(r1.X + r1.Width - 2, r1_1.Bottom - 2));
            //    //
            //    r1 = dgvatg.GetCellDisplayRectangle(0, 10, true); //get the column header cell
            //    r1_1 = dgvatg.GetCellDisplayRectangle(34, 10, true);
            //    e.Graphics.DrawLine(new Pen(Color.Brown), new Point(r1.X, r1.Y), new Point(r1_1.X + r1_1.Width - 2, r1.Y));
            //    r1 = dgvatg.GetCellDisplayRectangle(0, 12, true); //get the column header cell
            //    r1_1 = dgvatg.GetCellDisplayRectangle(34, 12, true);
            //    e.Graphics.DrawLine(new Pen(Color.Brown), new Point(r1.X, r1.Y), new Point(r1_1.X + r1_1.Width - 2, r1.Y));
            //}
        }

        void dgvatg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1) { dgvatg.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = false; }
        }


        private void btn_PAGMA_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (Form1.chkformsize.Checked && Form1.tslblStatus.Text.ToUpper().IndexOf("ERR", 0) != -1 && mvars.demoMode == false) { return; }
            #endregion parameter initial

            if (btn_PAGMA.Text.ToUpper().IndexOf("AUTOGAMMA", 0) != -1 || btn_PAGMA.Text.ToUpper().IndexOf("GAMMACURVE", 0) != -1)
            {
                int svi;
                string svs;
                mvars.lCount = 0;
                mvars.lCounts = 1999; Form1.tslbltarget.Text = mvars.lCounts.ToString();
                Array.Resize(ref mvars.lCmd, mvars.lCounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
                Array.Resize(ref mvars.lGet, mvars.lCounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
                mvars.byPass = false;
                lstget.Items.Clear();
                btn_PAGMA.Enabled = false; 
                txt_UUTID.Enabled = btn_PAGMA.Enabled;
                mvars.Break = false;
                btn_Break.Enabled = false; 


                #region CA connect
                if (mCAs.CAATG.Demo == false)
                {
                    if (mCAs.CAATG.CAsel == 0 && mCAs.CAATG.PlugSP == false)
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            if (chk_ZeroCAL.Checked) lstget.Items.Add(" --> CA-210/310 connecting and \"0-CAL\".... Please wait");
                            else lstget.Items.Add(" --> CA-210/310 connect .... Please wait");
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            if (chk_ZeroCAL.Checked) lstget.Items.Add(" --> CA-210/310 連接中，\"0-CAL\".... 請稍後");
                            else lstget.Items.Add(" --> CA-210/310 連接中 .... 請稍後");
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            if (chk_ZeroCAL.Checked) lstget.Items.Add(" --> CA-210/310 连接中，\"0-CAL\".... 请稍后");
                            else lstget.Items.Add(" --> CA-210/310 连接中 .... 请稍后");
                        }
                        mp.doDelayms(100);
                        mCAs.CAinit();
                        lstget.Items.RemoveAt(lstget.Items.Count - 1);
                        if (mCAs.msg != "")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" --> CA-210/310 connect fail，" + mCAs.msg);
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" --> CA-210/310 連接失敗，" + mCAs.msg);
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" --> CA-210/310 连接失败，" + mCAs.msg);
                            btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                            mCAs.CAATG.PlugSP = false; mvars.Break = false;
                            return;
                        }
                        else
                        {
                            if (chk_ZeroCAL.Checked) { mCAs.CAzero(); }
                        }
                    }
                    else if (mCAs.CAATG.CAsel == 1 && mCAs.CAATG.PlugSP == false)
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            lstget.Items.Add(" --> CA-410 connect .... Please wait");
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            lstget.Items.Add(" --> CA-410 連接中 .... 請稍後");
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            lstget.Items.Add(" --> CA-410 连接中 .... 请稍后");
                        }
                        mp.doDelayms(100);
                        mCA4.AutoConnect();
                        lstget.Items.RemoveAt(lstget.Items.Count - 1);
                        if (mCAs.msg != "")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US")
                            {
                                lstget.Items.Add(" -> CA-410 connect fail，" + mCAs.msg);
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            {
                                lstget.Items.Add(" -> CA-410 連線失敗，" + mCAs.msg);
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                            {
                                lstget.Items.Add(" -> CA-410 连接失败，" + mCAs.msg);
                            }
                            btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                            mCAs.CAATG.PlugSP = false; mvars.Break = false;
                            return;
                        }
                    }

                    if (mCAs.CAATG.PlugSP)
                    {
                        if (mCAs.CAATG.CAsel == 1) { mCA4._objMemory.put_ChannelNO(mvars.UUT.CAch); }
                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            lstget.Items.Add(" -> " + mCAs.CAATG.Class + "_" + mCAs.CAATG.ProbeInfo + "，preCH." + mvars.UUT.CAch.ToString("00") + "，NDBetx" + mCAs.CAATG.OverBet.ToString() + "，connected");
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            lstget.Items.Add(" -> " + mCAs.CAATG.Class + "_" + mCAs.CAATG.ProbeInfo + "，預設頻道." + mvars.UUT.CAch.ToString("00") + "，ND倍率x" + mCAs.CAATG.OverBet.ToString() + "，已連接");
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            lstget.Items.Add(" -> " + mCAs.CAATG.Class + "_" + mCAs.CAATG.ProbeInfo + "，预设频道." + mvars.UUT.CAch.ToString("00") + "，ND倍率x" + mCAs.CAATG.OverBet.ToString() + "，已连接");
                        }
                    }
                    else
                    {
                        mp.funSaveLogs("(pAGMA) CA connect fail");
                        MessageBox.Show(lstget.Items[lstget.Items.Count - 1].ToString(), mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                        return;
                    }
                    svs = mCAs.CAATG.Class + "_" + mvars.UUT.CAch.ToString("00") + "x" + mCAs.CAATG.OverBet;
                }
                else
                {
                    mCAs.CAATG.Class = "CA-DEMO";
                    mCAs.CAATG.ProbeSN = "1234567890";
                    lstget.Items.Add(" -> CA-DEMO preCH" + mvars.UUT.CAch.ToString("00") + "  x" + mCAs.CAATG.OverBet.ToString() + " connected");
                    svs = mCAs.CAATG.Class;
                }
                #endregion CA connect


                #region UUTID (去除小數點，TV130後取消)
                //string svsubs = Regex.Replace(lbl_cm603Vref.Text.Split(',')[0], ",", "");
                //string value = "";
                //for (svi = 0; svi < svsubs.Length; svi++)
                //{
                //    if (svsubs.Substring(svi, 1) != ".") value += svsubs.Substring(svi, 1);
                //}
                //value = DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_" + value +"_" + string.Format("{0:####0.0}", lbl_CLv.Text);
                string value = DateTime.Now.Year + 
                               DateTime.Now.Month.ToString("00") + 
                               DateTime.Now.Day.ToString("00") + "_" + 
                               lbl_cm603Vref.Text.Split(',')[0] + "_" + 
                               lbl_VREF.Text.Split(' ')[1].Substring(0, lbl_VREF.Text.Split(' ')[1].Length-1) + "_" + 
                               string.Format("{0:####0.0}", lbl_CLv.Text);

                if (txt_UUTID.Text != "") value = txt_UUTID.Text;
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                                            "    Please input UUT ID", ref value, 71) == DialogResult.Cancel)
                    {
                        lstget.Items.Add(" -> User Canceled");
                        mp.funSaveLogs("(pAGMA) User break @ Input ID");
                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled; btn_Break.Enabled = false; return;
                    }
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                                            "    請輸入燈板 ID", ref value, 71) == DialogResult.Cancel)
                    {
                        lstget.Items.Add(" -> 使用者中斷");
                        mp.funSaveLogs("(pAGMA) User break @ Input ID");
                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled; btn_Break.Enabled = false; return;
                    }
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                                            "    请输入灯板 ID", ref value, 71) == DialogResult.Cancel)
                    {
                        lstget.Items.Add(" -> 使用者中断");
                        mp.funSaveLogs("(pAGMA) User break @ Input ID");
                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled; btn_Break.Enabled = false; return;
                    }
                }
                txt_UUTID.Text = value;
                mvars.UUT.ID = txt_UUTID.Text;
                #endregion UUTID


                #region ShowExtend 
                if (File.Exists(mvars.strStartUpPath + @"\Parameter\TuningArea.txt")) Form2.fileTuningArea(false); else mvars.TuningArea.Mark = "";

                string sverr = "0";
                if (mvars.demoMode == false)
                {
                    byte svFPGAsel = mvars.FPGAsel;
                    mvars.FPGAsel = 2;                          //左右FPGA 廣播模式
                    if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04")
                    {
                        #region C12A/B
                        //int W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                        //int H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                        int extX = 0;
                        int extY = 0;
                        mp.checkExtendScreen(ref extX, ref extY);
                        if (mp.upperBound == 0)
                        {
                            if (mvars.TuningArea.Mark.ToLower() != "pg")
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    lstget.Items.Add("No Extend Screen");
                                    if (MessageBox.Show("No Extend Screen" + "\r\n" + "\r\n" + "AutoGamma pattern change to PG apply ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    lstget.Items.Add("沒有延伸螢幕");
                                    if (MessageBox.Show("沒有延伸螢幕" + "\r\n" + "\r\n" + "切換到 PG 模式畫面 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    lstget.Items.Add("没有延伸萤幕");
                                    if (MessageBox.Show("没有延伸萤幕" + "\r\n" + "\r\n" + "切换到 PG 模式画面 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                            }
                            
                            Form1.pvindex = 105;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(256);     // 105 GRAY_R
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 106;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(256);     // 106 GRAY_G
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 107;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(256);     // 107 GRAY_B
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 1;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(3);       //1 SI_SEL PG模式(3)
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 21;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(257);     // 21 PT_SEL
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }                           
                            Form1.pvindex = 108;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(0);       // 108 X_START
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 109;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(479);     // 109 X_END
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 110;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(0);       // 110 Y_START
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 111;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(269);     // 111 Y_END
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }



                            /*
                             * 適用在 C12A 2.2.7  C12B 1.2.0  UI 1.3.0 版以前 PG mode會閃的對策
                            int svitvdp = 270;
                            Form1.pvindex = 11;         //IT_VDP    C12A 540/ C12B 270
                            mvars.lblCmd = "FPGA_SPI_R";
                            mp.mhFPGASPIREAD();        
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            else { svitvdp = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]); }
                            
                            int svitvfp = 510;
                            Form1.pvindex = 12;         //IT_VFP    C12A 240/ C12B 510
                            mvars.lblCmd = "FPGA_SPI_R";
                            mp.mhFPGASPIREAD();
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            else { svitvfp = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]); }
                            if (svitvdp + svitvfp == 780)
                            {
                                Form1.pvindex = 12;
                                mvars.lblCmd = "FPGA_SPI_W";
                                mp.mhFPGASPIWRITE(svitvfp - 20);   //IT_VFP
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            }
                            */



                            Form1.pvindex = 1;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(3);      // 01 SI_SEL PG mode
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 255;
                            mvars.lblCmd = "FPGA_SPI_W255";
                            mp.mhFPGASPIWRITE(0);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 255;
                            mvars.lblCmd = "FPGA_SPI_W255";
                            mp.mhFPGASPIWRITE(1);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            Form1.pvindex = 255;
                            mvars.lblCmd = "FPGA_SPI_W255";
                            mp.mhFPGASPIWRITE(0);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            if (sverr == "-16")
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US") 
                                {
                                    MessageBox.Show("AutoGamma pattern change to PG mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT") 
                                {
                                    MessageBox.Show("切換 PG 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN") 
                                {
                                    MessageBox.Show("切换 PG 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                            lstget.Items.RemoveAt(lstget.Items.Count - 1);
                            if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PG mode");
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PG 模式");
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PG 模式");
                            mvars.TuningArea.Mark = "pg";
                        }
                        else
                        {

                            //Screen actscr=Screen.FromPoint(new Point().this.)


                            if (mvars.TuningArea.Mark == "pg")
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    if (MessageBox.Show("AutoGamma pattern change to PC apply ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    if (MessageBox.Show("有延伸螢幕，切換 PC 模式顯示 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    if (MessageBox.Show("有延伸萤幕，切换 PC 模式显示 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }

                                if (mvars.TuningArea.Mark != "pg")
                                {
                                    sverr = "0";
                                    Form1.pvindex = 1;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(2);      // 01 SI_SEL PC mode
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = 255;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(0);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = 255;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(1);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = 255;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(0);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    if (sverr == "-16")
                                    {
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            MessageBox.Show("切換 PC 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            MessageBox.Show("切换 PC 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        return;
                                    }
                                }
                                
                                //Form2.i3pat = new i3_Pat();
                                //Form2.i3pat.Location = new System.Drawing.Point(extX, extY);
                                //Form2.i3pat.BackColor = Color.Blue;
                                //Form2.i3pat.Show();
                                //Form2.i3pat.Visible = false;
                                //Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                //Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                //Form2.i3pat.TopMost = true;
                                //Form2.i3pat.Visible = true;

                                Form2.i3pat = new i3_Pat
                                {
                                    Location = new System.Drawing.Point(extX, extY),
                                    BackColor = Color.FromArgb(80, 80, 80),
                                    Visible = false,
                                    FormBorderStyle = FormBorderStyle.None,
                                    Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH)),
                                    TopMost = true
                                };
                                Form2.i3pat.Visible = true;
                                Form2.i3pat.Show();
                                
                                if (mvars.FormShow[5])
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
                                if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PC mode");
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PC 模式");
                                else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PC 模式");
                                mvars.TuningArea.Mark = "1";
                            }
                        }
                        #endregion C12A/B
                    }
                    else if (mvars.deviceID.Substring(0, 2) == "03")
                    {
                        #region H55
                        if (mvars.TuningArea.Mark == "pg")
                        {
                            if (MessageBox.Show("AutoGamma pattern change to LAN apply?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                Form1.pvindex = 1;
                                mvars.lblCmd = "FPGA_SPI_W";
                                mp.mhFPGASPIWRITE(2);      // 01 SI_SEL PC mode
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                Form1.pvindex = 255;
                                mvars.lblCmd = "FPGA_SPI_W255";
                                mp.mhFPGASPIWRITE(0);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                Form1.pvindex = 255;
                                mvars.lblCmd = "FPGA_SPI_W255";
                                mp.mhFPGASPIWRITE(1);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                Form1.pvindex = 255;
                                mvars.lblCmd = "FPGA_SPI_W255";
                                mp.mhFPGASPIWRITE(0);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            }
                            if (sverr == "-16")
                            {
                                MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                lstget.Items.Add(" --> Please re-AutoGamma"); lstget.TopIndex = lstget.Items.Count - 1;
                                return;
                            }
                        }
                        int extX = 0;
                        int extY = 0;
                        mp.checkExtendScreen(ref extX, ref extY);
                        if (mp.upperBound == 0)
                        {
                            lstget.Items.RemoveAt(lstget.Items.Count - 1);
                            lstget.Items.Add("No Extend Screen");
                            lstget.Items.Add(" --> Please re-AutoGamma"); lstget.TopIndex = lstget.Items.Count - 1;
                            btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                            return;
                        }
                        else
                        {
                            //Form2.i3pat = new i3_Pat();
                            //Form2.i3pat.Location = new System.Drawing.Point(extX, extY);
                            //Form2.i3pat.BackColor = Color.FromArgb(5, 5, 5);
                            //Form2.i3pat.Show();
                            //Form2.i3pat.Visible = false;
                            //Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                            //Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                            //Form2.i3pat.TopMost = true;
                            //Form2.i3pat.Visible = true;

                            Form2.i3pat = new i3_Pat
                            {
                                Location = new System.Drawing.Point(extX, extY),
                                BackColor = Color.FromArgb(5, 5, 5),
                                Visible = false,
                                FormBorderStyle = FormBorderStyle.None,
                                Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH)),
                                TopMost = true,
                                Text = extX + "," + extY,
                            };
                            Form2.i3pat.Visible = true;
                            Form2.i3pat.Show();

                            if (mvars.FormShow[5])
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
                            lstget.Items.Add(" -> PC mode");
                            mvars.TuningArea.Mark = "1";
                        }
                        #endregion H55
                    }
                    else if (mvars.deviceID.Substring(0,2) == "05")
                    {
                        #region Primary
                        if (btn_PAGMA.Text.ToUpper().IndexOf("AUTOGAMMA", 0) != -1)
                        {
                            mvars.lblCmd = "FPGA_HW_RESET";
                            mp.mhFPGARESET(0x80);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) 
                            { 
                                mvars.ATGerr = "-17.0"; 
                                return; 
                            }
                            mp.doDelayms(2000);
                        }

                        int extX = 0;
                        int extY = 0;
                        mp.checkExtendScreen(ref extX, ref extY);
                        if (mp.upperBound == 0)
                        {
                            if (mvars.TuningArea.Mark.ToLower() != "pg")
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    lstget.Items.Add("No Extend Screen");
                                    if (MessageBox.Show("No Extend Screen" + "\r\n" + "\r\n" + "AutoGamma pattern change to PG apply ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    lstget.Items.Add("沒有延伸螢幕");
                                    if (MessageBox.Show("沒有延伸螢幕" + "\r\n" + "\r\n" + "切換到 PG 模式畫面 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    lstget.Items.Add("没有延伸萤幕");
                                    if (MessageBox.Show("没有延伸萤幕" + "\r\n" + "\r\n" + "切换到 PG 模式画面 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                            }

                            Form1.pvindex = 1;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(2,0);      // 01 DIP_SW PG 
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    MessageBox.Show("AutoGamma pattern change to PG mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    MessageBox.Show("切換 PG 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    MessageBox.Show("切换 PG 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                            lstget.Items.RemoveAt(lstget.Items.Count - 1);
                            if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PG mode");
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PG 模式");
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PG 模式");
                            mvars.TuningArea.Mark = "pg";
                        }
                        else
                        {
                            if (mvars.TuningArea.Mark == "pg")
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    if (MessageBox.Show("AutoGamma pattern change to PC apply ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    if (MessageBox.Show("有延伸螢幕，切換 PC 模式顯示 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    if (MessageBox.Show("有延伸萤幕，切换 PC 模式显示 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }

                                if (mvars.TuningArea.Mark != "pg")
                                {
                                    sverr = "0";
                                    Form1.pvindex = 1;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(2,1);      // 01 SI_SEL PC mode
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                                    {
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            MessageBox.Show("切換 PC 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            MessageBox.Show("切换 PC 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                mvars.lblCmd = "FPGA_SPI_W";
                                Form1.pvindex = 1;          /// DIP_SW
                                mp.mhFPGASPIWRITE(2, 1);    /// PC
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                                {
                                    if (MultiLanguage.DefaultLanguage == "en-US")
                                    {
                                        MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                    {
                                        MessageBox.Show("切換 PC 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                    {
                                        MessageBox.Show("切换 PC 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                    return;
                                }
                            }

                            Form2.i3pat = new i3_Pat
                            {
                                Location = new System.Drawing.Point(extX, extY),
                                BackColor = Color.FromArgb(80, 80, 80),
                                Visible = false,
                                FormBorderStyle = FormBorderStyle.None,
                                Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH)),
                                TopMost = true
                            };
                            Form2.i3pat.Visible = true;
                            Form2.i3pat.Show();

                            if (mvars.FormShow[5])
                            {
                                Form2.fileTuningArea(false);
                                Form2.i3pat.lbl_Mark.Width = Form2.i3pat.Width;
                                Form2.i3pat.lbl_Mark.Height = Form2.i3pat.Height;
                                Form2.i3pat.lbl_Mark.Location = new Point(extX, extY);
                                Form2.i3pat.lbl_Mark.ForeColor = Color.White;
                                Form2.i3pat.lbl_Mark.BackColor = Color.FromArgb(80, 80, 80);
                                Form2.i3pat.lbl_Mark.Visible = true;
                                Application.DoEvents();
                            }
                            if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PC mode");
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PC 模式");
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PC 模式");
                            mvars.TuningArea.Mark = "1";
                        }
                        #endregion Primary
                    }
                    else if (mvars.deviceID.Substring(0, 2) == "06")
                    {
                        #region TV130
                        int extX = 0;
                        int extY = 0;
                        rechk:
                        mp.checkExtendScreen(ref extX, ref extY);
                        if (mp.upperBound == 0)
                        {
                            if (mvars.TuningArea.Mark.ToLower() != "pg")
                            {
                                DialogResult svr;
                                string svs1 = "";
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    svs1 = "No Extend Screen" + "\r\n" + "\r\n" + "AutoGamma pattern change to PG apply ?";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    svs1 = "沒有延伸螢幕" + "\r\n" + "\r\n" + "切換到 PG 模式畫面 ?";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    svs1 = "没有延伸萤幕" + "\r\n" + "\r\n" + "切换到 PG 模式画面 ?";
                                }
                                svr = mp.YesNoCancelBox(mvars.strUInameMe + "_v" + mvars.UImajor, svs1, 200);
                                if (svr==DialogResult.Cancel)
                                {
                                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                    return;
                                }
                                else if (svr == DialogResult.Yes)
                                {
                                    int[] svreg = { mvars.FPGA_SI_SEL, mvars.FPGA_PT_SEL, mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B, mvars.FPGA_AG_MOD, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                                    int[] svdata = { 1, 257, 960, 960, 960, 0, 383, 0, 539, 0, 0, 0, 0, 0, 1, 0 };
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                    {
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            MessageBox.Show("AutoGamma pattern change to PG mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            MessageBox.Show("切換 PG 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            MessageBox.Show("切换 PG 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        return;
                                    }
                                    if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PG mode");
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PG 模式");
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PG 模式");
                                    mvars.TuningArea.Mark = "pg";
                                }
                                else
                                {
                                    int[] svreg = { mvars.FPGA_SI_SEL, mvars.FPGA_AG_MOD, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                                    int[] svdata = { 0, 0, 0, 1, 0 };
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                    {
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            MessageBox.Show("切換 PC 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            MessageBox.Show("切换 PC 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        return;
                                    }
                                    if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PC mode");
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PC 模式");
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PC 模式");
                                    goto rechk;
                                }
                            }
                            else
                            {
                                int[] svreg = { mvars.FPGA_SI_SEL, mvars.FPGA_PT_SEL, mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B, mvars.FPGA_AG_MOD, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                                int[] svdata = { 1, 257, 960, 960, 960, 0, 383, 0, 539, 0, 0, 0, 0, 0, 1, 0 };
                                mvars.lblCmd = "FPGA_SPI_W";
                                mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                {
                                    if (MultiLanguage.DefaultLanguage == "en-US")
                                    {
                                        MessageBox.Show("AutoGamma pattern change to PG mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                    {
                                        MessageBox.Show("切換 PG 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                    {
                                        MessageBox.Show("切换 PG 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                    return;
                                }
                                if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PG mode");
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PG 模式");
                                else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PG 模式");
                                mvars.TuningArea.Mark = "pg";
                            }
                            //Form1.pvindex = mvars.FPGA_GRAY_R;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 256);      // 105 GRAY_R
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_GRAY_G;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 256);     // 106 GRAY_G
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_GRAY_B;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 256);     // 107 GRAY_B
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_AG_MOD;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);       //1 AG_MOD Aging Mode Disable
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_PT_SEL;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 257);     // 21 PT_SEL
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_X_START;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);       // 108 X_START
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_X_END;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 383);     // 109 X_END
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_Y_START;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);       // 110 Y_START
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_Y_END;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 539);     // 111 Y_END
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_SI_SEL;
                            //mvars.lblCmd = "FPGA_SPI_W";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 3);      // 01 SI_SEL PG mode
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_UD_REG;
                            //mvars.lblCmd = "FPGA_SPI_W255";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_UD_REG;
                            //mvars.lblCmd = "FPGA_SPI_W255";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                            //Form1.pvindex = mvars.FPGA_UD_REG;
                            //mvars.lblCmd = "FPGA_SPI_W255";
                            //mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                        }
                        else
                        {
                            //Screen actscr=Screen.FromPoint(new Point().this.)
                            if (mvars.TuningArea.Mark == "pg")
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    if (MessageBox.Show("AutoGamma pattern change to PC apply ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    if (MessageBox.Show("有延伸螢幕，切換 PC 模式顯示 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    if (MessageBox.Show("有延伸萤幕，切换 PC 模式显示 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }

                                if (mvars.TuningArea.Mark != "pg")
                                {
                                    sverr = "0";
                                    Form1.pvindex = 1;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 2);      // 01 SI_SEL PC mode
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = 255;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = 255;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = 255;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    if (sverr == "-16")
                                    {
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            MessageBox.Show("切換 PC 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            MessageBox.Show("切换 PC 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        return;
                                    }

                                    Form2.i3pat = new i3_Pat
                                    {
                                        Location = new System.Drawing.Point(extX, extY),
                                        BackColor = Color.FromArgb(80, 80, 80),
                                        Visible = false,
                                        FormBorderStyle = FormBorderStyle.None,
                                        Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH)),
                                        TopMost = true
                                    };
                                    Form2.i3pat.Visible = true;
                                    Form2.i3pat.Show();

                                    if (mvars.FormShow[5])
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
                                    if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PC mode");
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PC 模式");
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PC 模式");
                                    mvars.TuningArea.Mark = "1";
                                }
                                else
                                {
                                    Form1.pvindex = mvars.FPGA_GRAY_R;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 128);      // 105 GRAY_R
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_GRAY_G;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 128);     // 106 GRAY_G
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_GRAY_B;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 128);     // 107 GRAY_B
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_AG_MOD;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);       //1 AG_MOD Aging Mode Disable
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_PT_SEL;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 257);     // 21 PT_SEL
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_X_START;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);       // 108 X_START
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_X_END;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 191);     // 109 X_END
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_Y_START;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);       // 110 Y_START
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_Y_END;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 179);     // 111 Y_END
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }

                                    Form1.pvindex = mvars.FPGA_SI_SEL;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 3);      // 01 SI_SEL PG mode
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_UD_REG;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_UD_REG;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    Form1.pvindex = mvars.FPGA_UD_REG;
                                    mvars.lblCmd = "FPGA_SPI_W255";
                                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                    if (sverr == "-16")
                                    {
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            MessageBox.Show("AutoGamma pattern change to PG mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            MessageBox.Show("切換 PG 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            MessageBox.Show("切换 PG 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        return;
                                    }
                                    lstget.Items.RemoveAt(lstget.Items.Count - 1);
                                    if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PG mode");
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PG 模式");
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PG 模式");
                                    mvars.TuningArea.Mark = "pg";
                                }
                            }
                            else
                            {
                                sverr = "0";
                                Form1.pvindex = 1;
                                mvars.lblCmd = "FPGA_SPI_W";
                                mp.mhFPGASPIWRITE(mvars.FPGAsel, 2);      // 01 SI_SEL PC mode
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                Form1.pvindex = 255;
                                mvars.lblCmd = "FPGA_SPI_W255";
                                mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                Form1.pvindex = 255;
                                mvars.lblCmd = "FPGA_SPI_W255";
                                mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                Form1.pvindex = 255;
                                mvars.lblCmd = "FPGA_SPI_W255";
                                mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-16"; }
                                if (sverr == "-16")
                                {
                                    if (MultiLanguage.DefaultLanguage == "en-US")
                                    {
                                        MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                    {
                                        MessageBox.Show("切換 PC 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                    {
                                        MessageBox.Show("切换 PC 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                    return;
                                }

                                Form2.i3pat = new i3_Pat
                                {
                                    Location = new System.Drawing.Point(extX, extY),
                                    BackColor = Color.FromArgb(80, 80, 80),
                                    Visible = false,
                                    FormBorderStyle = FormBorderStyle.None,
                                    Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH)),
                                    TopMost = true
                                };
                                Form2.i3pat.Visible = true;
                                Form2.i3pat.Show();

                                if (mvars.FormShow[5])
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
                                if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PC mode");
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PC 模式");
                                else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PC 模式");
                                mvars.TuningArea.Mark = "1";
                            }
                        }
                        #endregion TV130
                    }
                    else if (mvars.deviceID.Substring(0, 2) == "10")
                    {
                        #region CarpStreamer
                        mvars.FPGA_GRAY_R = 0x30;
                        mvars.FPGA_GRAY_G = 0x31;
                        mvars.FPGA_GRAY_B = 0x32;
                        mvars.FPGA_PT_BANK = 0x33;

                        if (btn_PAGMA.Text.ToUpper().IndexOf("AUTOGAMMA", 0) != -1)
                        {
                            mvars.lblCmd = "FPGA_HW_RESET";
                            mp.mhFPGARESET(0x80);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                            {
                                mvars.ATGerr = "-17.0";
                                return;
                            }
                            mp.doDelayms(2000);
                        }

                        int extX = 0;
                        int extY = 0;
                        mp.checkExtendScreen(ref extX, ref extY);
                        if (mp.upperBound == 0)
                        {
                            if (mvars.TuningArea.Mark.ToLower() != "pg")
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    lstget.Items.Add("No Extend Screen");
                                    if (MessageBox.Show("No Extend Screen" + "\r\n" + "\r\n" + "AutoGamma pattern change to PG apply ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    lstget.Items.Add("沒有延伸螢幕");
                                    if (MessageBox.Show("沒有延伸螢幕" + "\r\n" + "\r\n" + "切換到 PG 模式畫面 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    lstget.Items.Add("没有延伸萤幕");
                                    if (MessageBox.Show("没有延伸萤幕" + "\r\n" + "\r\n" + "切换到 PG 模式画面 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        return;
                                    }
                                }
                            }

                            Form1.pvindex = mvars.FPGA_PT_BANK;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(3);
                            Form1.pvindex = mvars.FPGA_GRAY_R;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(0);    // 38 GRAY_R
                            Form1.pvindex = mvars.FPGA_GRAY_G;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(0);    // 39 GRAY_G
                            Form1.pvindex = mvars.FPGA_GRAY_B;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(256);    // 40 GRAY_B
                            mp.doDelayms(200);

                            Form1.pvindex = 1;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(96);      // 01 DIP_SW PG 
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    MessageBox.Show("AutoGamma pattern change to PG mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    MessageBox.Show("切換 PG 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    MessageBox.Show("切换 PG 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                    lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                }
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                            lstget.Items.RemoveAt(lstget.Items.Count - 1);
                            if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PG mode");
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PG 模式");
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PG 模式");
                            mvars.TuningArea.Mark = "pg";
                        }
                        else
                        {
                            if (mvars.TuningArea.Mark == "pg")
                            {
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    if (MessageBox.Show("AutoGamma pattern change to PC apply ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    if (MessageBox.Show("有延伸螢幕，切換 PC 模式顯示 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    if (MessageBox.Show("有延伸萤幕，切换 PC 模式显示 ?", mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK) mvars.TuningArea.Mark = "1";
                                }

                                if (mvars.TuningArea.Mark != "pg")
                                {
                                    sverr = "0";
                                    Form1.pvindex = 1;
                                    mvars.lblCmd = "FPGA_SPI_W";
                                    mp.mhFPGASPIWRITE(1);      // 01 SI_SEL PC mode
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                                    {
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            MessageBox.Show("切換 PC 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            MessageBox.Show("切换 PC 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                            lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                        }
                                        btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                mvars.lblCmd = "FPGA_SPI_W";
                                Form1.pvindex = 1;          /// DIP_SW
                                mp.mhFPGASPIWRITE(1);       /// PC
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                                {
                                    if (MultiLanguage.DefaultLanguage == "en-US")
                                    {
                                        MessageBox.Show("AutoGamma pattern change to PC mode fail", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> Please re- " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                    {
                                        MessageBox.Show("切換 PC 模式發生異常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> 請重新執行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                    {
                                        MessageBox.Show("切换 PC 模式发生异常", mvars.strUInameMe + "_v" + mvars.UImajor);
                                        lstget.Items.Add(" --> 请重新执行 " + btn_PAGMA.Text); lstget.TopIndex = lstget.Items.Count - 1;
                                    }
                                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                    return;
                                }
                            }

                            Form2.i3pat = new i3_Pat
                            {
                                Location = new System.Drawing.Point(extX, extY),
                                BackColor = Color.FromArgb(80, 80, 80),
                                Visible = false,
                                FormBorderStyle = FormBorderStyle.None,
                                Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH)),
                                TopMost = true
                            };
                            Form2.i3pat.Visible = true;
                            Form2.i3pat.Show();

                            if (mvars.FormShow[5])
                            {
                                Form2.fileTuningArea(false);
                                Form2.i3pat.lbl_Mark.Width = Form2.i3pat.Width;
                                Form2.i3pat.lbl_Mark.Height = Form2.i3pat.Height;
                                Form2.i3pat.lbl_Mark.Location = new Point(extX, extY);
                                Form2.i3pat.lbl_Mark.ForeColor = Color.White;
                                Form2.i3pat.lbl_Mark.BackColor = Color.FromArgb(80, 80, 80);
                                Form2.i3pat.lbl_Mark.Visible = true;
                                Application.DoEvents();
                            }
                            if (MultiLanguage.DefaultLanguage == "en-US") lstget.Items.Add(" -> PC mode");
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") lstget.Items.Add(" -> PC 模式");
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") lstget.Items.Add(" -> PC 模式");
                            mvars.TuningArea.Mark = "1";
                        }
                        #endregion CarpStreamer
                    }
                    mvars.FPGAsel = svFPGAsel;
                }
                else // demo mode
                {
                    int W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                    Screen[] screens = Screen.AllScreens;
                    //int upperBound;
                    //upperBound = screens.GetUpperBound(0);
                    int upperBound = 0;
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
                    }
                    //Form2.i3pat = new i3_Pat();
                    //Form2.i3pat.BackColor = Color.Blue;
                    //Form2.i3pat.Show();
                    //Form2.i3pat.Visible = false;
                    //Form2.i3pat.Location = new System.Drawing.Point(W / 5 * 3, 500);
                    //Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                    //Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW / 10), Convert.ToInt16(mvars.UUT.resW / 10));
                    //Form2.i3pat.TopMost = true;
                    //Form2.i3pat.Visible = true;

                    Form2.i3pat = new i3_Pat
                    {
                        BackColor = Color.FromArgb(80, 80, 80),
                        Visible = false,
                        Location = new System.Drawing.Point(W / 5 * 3, 500),
                        FormBorderStyle = FormBorderStyle.None,
                        Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW / 10), Convert.ToInt16(mvars.UUT.resW / 10)),
                        TopMost = true
                    };
                    Form2.i3pat.Visible = true;
                    Form2.i3pat.Show();

                    if (mvars.FormShow[5])
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
                #endregion ShowExtend


                mvars.Break = false;
                btn_Break.Enabled = true;
                btn_PAGMA.Enabled = false; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    Form1.tslblStatus.Text = "Wait .... ";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    Form1.tslblStatus.Text = "請稍後 .... ";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    Form1.tslblStatus.Text = "请稍后 .... ";
                }

                if (mvars.demoMode) mvars.msgA = "(Demo) " + mvars.UUT.ID; else mvars.msgA = mvars.UUT.ID;
                //mvars.msgA += mvars.UUT.ID;
                if (mvars.TuningArea.Mark.ToLower() == "pg") mvars.msgA += " (PG " + svs + ")"; else mvars.msgA += " (PC " + svs + ")";

                if (btn_PAGMA.Text.ToUpper().IndexOf("GAMMACURVE", 0) != -1)
                {
                    if (btn_PAGMA.Text.ToUpper().Trim().Length > "GAMMACURVE".Length)
                    {
                        if (btn_PAGMA.Text.ToUpper().Trim().Length == "GAMMACURVE".Length + 1)
                        {
                            if (btn_PAGMA.Text.ToUpper().Trim().Substring("GAMMACURVE".Length, 1) == "1") { mvars.msrgammacurveEd = 41; mvars.msrgammacurveGp = 1; mvars.msrgammacurveSt = 0; }
                            else if (btn_PAGMA.Text.ToUpper().Trim().Substring("GAMMACURVE".Length, 1) == "2") { mvars.msrgammacurveEd = 255; mvars.msrgammacurveGp = 1; mvars.msrgammacurveSt = 0; }
                        }
                        else if (btn_PAGMA.Text.ToUpper().Trim().Split(',').Length == 4)
                        {
                            if (btn_PAGMA.Text.ToUpper().Trim().Split(',')[0] == "GAMMACURVE")
                            {
                                mvars.msrgammacurveSt = Convert.ToInt16(btn_PAGMA.Text.ToUpper().Trim().Split(',')[1]);
                                mvars.msrgammacurveEd = Convert.ToInt16(btn_PAGMA.Text.ToUpper().Trim().Split(',')[2]);
                                mvars.msrgammacurveGp = Convert.ToInt16(btn_PAGMA.Text.ToUpper().Trim().Split(',')[3]);
                            }
                        }
                    }
                    mvars.flgdirGamma = true;
                    mvars.msgA += " GammaCurve," + mvars.msrgammacurveSt + "," + mvars.msrgammacurveEd + "," + mvars.msrgammacurveGp;
                }
                else if (btn_PAGMA.Text.ToUpper().IndexOf("AUTOGAMMA", 0) != -1)
                { 
                    mvars.flgdirGamma = false;

                    #region White-Tracking Bet   ex. mvars.msgA += "," + mvars.UUT.WTLvBet;
                    value = "1";

                    if (MultiLanguage.DefaultLanguage == "en-US")
                    {
                        if (mCAs.CAATG.CAsel == 0)
                        {
                            if (mp.InputBox(mvars.strUInameMe,
                                                    "       1. Make sure the panel is powered on" + "\r\n" + "\r\n" +
                                                    "       2. Check " + mCAs.CAATG.Class + " probe @ \" MEAS \"" + "\r\n" + "\r\n" +
                                                    "       3. Put probe @ measurement position" + "\r\n" + "\r\n" + "\r\n" +
                                                    "    Input White-Tracking luminance ratio"
                        , ref value, 115) == DialogResult.Cancel)
                            {
                                lst_get.Items.Add(" -> User Canceled");
                                //lst_get.Items.Add(" --> Please re-AutoGamma");
                                //mp.funSaveLogs("(pAGMA) User break @ Input White-Tracking Bet");
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                        }
                        else
                        {
                            if (mp.InputBox(mvars.strUInameMe,
                                                    "       1. Make sure the panel is powered on" + "\r\n" + "\r\n" +
                                                    "       2. Put probe @ measurement position" + "\r\n" + "\r\n" + "\r\n" +
                                                    "    Input White-Tracking luminance ratio"
                                                    , ref value, 115) == DialogResult.Cancel)
                            {
                                lst_get.Items.Add(" -> User Canceled");
                                //lst_get.Items.Add(" --> Please re-AutoGamma");
                                //mp.funSaveLogs("(pAGMA) User break @ Input White-Tracking Bet");

                                if (mvars.FormShow[5]) { mvars.FormShow[5] = false; Form2.i3pat.Close(); }

                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                        }
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    {
                        if (mCAs.CAATG.CAsel == 0)
                        {
                            if (mp.InputBox(mvars.strUInameMe,
                                                                            "       1. 確認燈板電源開啟" + "\r\n" + "\r\n" +
                                                                            "       2. 檢查 " + mCAs.CAATG.Class + " 量測探頭檔位在 \" MEAS \"" + "\r\n" + "\r\n" +
                                                                            "       3. 量測探頭架設在燈板位置" + "\r\n" + "\r\n" + "\r\n" +
                                                                            "    輸入 White-Tracking 的亮度修正係數"
                                                                            , ref value, 115) == DialogResult.Cancel)
                            {
                                lst_get.Items.Add(" -> 使用者中斷");
                                //lst_get.Items.Add(" --> Please re-AutoGamma");
                                //mp.funSaveLogs("(pAGMA) User break @ Input White-Tracking Bet");
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                        }
                        else
                        {
                            if (mp.InputBox(mvars.strUInameMe,
                                                                            "       1. 確認燈板電源開啟" + "\r\n" + "\r\n" +
                                                                            "       2. 量測探頭架設在燈板位置" + "\r\n" + "\r\n" + "\r\n" +
                                                                            "    輸入 White-Tracking 的亮度修正係數"
                                                                            , ref value, 115) == DialogResult.Cancel)
                            {
                                lst_get.Items.Add(" -> 使用者中斷");
                                //lst_get.Items.Add(" --> Please re-AutoGamma");
                                //mp.funSaveLogs("(pAGMA) User break @ Input White-Tracking Bet");
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                        } 
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    {

                        if (mCAs.CAATG.CAsel == 0)
                        {
                            if (mp.InputBox(mvars.strUInameMe,
                                                    "       1. 确认灯板电源开启" + "\r\n" + "\r\n" +
                                                    "       2. 检查 " + mCAs.CAATG.Class + " 量测探头档位在 \" MEAS \"" + "\r\n" + "\r\n" +
                                                    "       3. 量测探头架设在灯板位置" + "\r\n" + "\r\n" + "\r\n" +
                                                    "    输入 White-Tracking 的亮度修正系数"
                                                    , ref value, 115) == DialogResult.Cancel)
                            {
                                lst_get.Items.Add(" -> 使用者中断");
                                //lst_get.Items.Add(" --> Please re-AutoGamma");
                                //mp.funSaveLogs("(pAGMA) User break @ Input White-Tracking Bet");
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                        }
                        else
                        {
                            if (mp.InputBox(mvars.strUInameMe,
                                                    "       1. 确认灯板电源开启" + "\r\n" + "\r\n" +
                                                    "       2. 量测探头架设在灯板位置" + "\r\n" + "\r\n" + "\r\n" +
                                                    "    输入 White-Tracking 的亮度修正系数"
                                                    , ref value, 115) == DialogResult.Cancel)
                            {
                                lst_get.Items.Add(" -> 使用者中断");
                                //lst_get.Items.Add(" --> Please re-AutoGamma");
                                //mp.funSaveLogs("(pAGMA) User break @ Input White-Tracking Bet");
                                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                return;
                            }
                        }

                    }

                    if (mp.IsNumeric(value) == false) { value = "1"; }
                    if (Convert.ToSingle(value) <= 0) { value = "1"; }
                    mvars.UUT.WTLvBet = Convert.ToSingle(value);
                    if (mCAs.CAATG.Demo == false && mCAs.CAATG.CAsel == 0)
                    {
                        if (mvars.UUT.CLv * mvars.UUT.WTLvBet > 950)
                        {
                            mvars.UUT.WTLvBet = Convert.ToSingle((950 / mvars.UUT.CLv).ToString("#0.0"));

                            if (MultiLanguage.DefaultLanguage == "en-US")
                            {
                                if (MessageBox.Show("CA210/310 can't measure over 1000nits，Ratio change to \"" + mvars.UUT.WTLvBet.ToString() + "\"" + "\r\n" + "\r\n" + "New CLv " + (mvars.UUT.CLv * mvars.UUT.WTLvBet).ToString("###0.0"), mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                {
                                    lst_get.Items.Add(" -> User Canceled");
                                    //lst_get.Items.Add(" --> Please re-AutoGamma");
                                    //mp.funSaveLogs("(pAGMA) User break @ Ratio change to for New CLv");
                                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                    return;
                                }
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            {
                                if (MessageBox.Show("CA210/310 無法量測超過 1000nits，頻道倍率 \"" + mvars.UUT.WTLvBet.ToString() + "\"" + "\r\n" + "\r\n" + "新亮度基準 " + (mvars.UUT.CLv * mvars.UUT.WTLvBet).ToString("###0.0"), mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                {
                                    lst_get.Items.Add(" -> 使用者中断");
                                    //lst_get.Items.Add(" --> Please re-AutoGamma");
                                    //mp.funSaveLogs("(pAGMA) User break @ Ratio change to for New CLv");
                                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                    return;
                                }
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                            {
                                if (MessageBox.Show("CA210/310 无法量测超过 1000nits，频道倍率 \"" + mvars.UUT.WTLvBet.ToString() + "\"" + "\r\n" + "\r\n" + "新亮度基准 " + (mvars.UUT.CLv * mvars.UUT.WTLvBet).ToString("###0.0"), mvars.strUInameMe + "_v" + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                {
                                    lst_get.Items.Add(" -> 使用者中断");
                                    //lst_get.Items.Add(" --> Please re-AutoGamma");
                                    //mp.funSaveLogs("(pAGMA) User break @ Ratio change to for New CLv");
                                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                                    return;
                                }
                            }
                        }
                    }
                    mvars.msgA += "," + mvars.UUT.WTLvBet;
                    #endregion

                    if (mvars.UUT.MTP == 1) mvars.msgA += " AutoGamma MTP WTbet" + mvars.UUT.WTLvBet;
                    else mvars.msgA += " AutoGamma DAC WTbet" + mvars.UUT.WTLvBet;
                }

                if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\")) { Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Gamma\"); }

                mvars.flgDirMTP = false;
                #region Direct MTP
                if (Directory.Exists(mvars.UUT.gmafilepath + @"\" + mvars.UUT.ID + @"\") == true && mvars.flgDirMTP)
                {
                    string svsdisk = "";
                    DirectoryInfo currentDir = new DirectoryInfo(mvars.UUT.gmafilepath + @"\" + mvars.UUT.ID + @"\");
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
                        Directory.Delete(mvars.UUT.gmafilepath + @"\" + mvars.UUT.ID + @"\");
                    }
                }
                #endregion

                String fnum;
                int svuutbetlv = (int)(Math.Round(mvars.UUT.CLv * mvars.UUT.WTLvBet, 0));
                lbl_CLv.Text = svuutbetlv.ToString();
                for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
                {
                    if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 100)
                        fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                    else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 100 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 10)
                        fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                    else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 10 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 1)
                        fnum = String.Format("{0:###0.#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                    else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 1 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 0.1)
                        fnum = String.Format("{0:0.##}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                    else
                        fnum = String.Format("{0:0.###}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                    dgvatg.Rows[7].Cells[svi].Value = fnum;
                }
                dgvatg.Refresh();

                lst_get.Items.Add(Form1.tslblStatus.Text + "go AutoGamma"); lst_get.TopIndex = lst_get.Items.Count - 1;

                mvars.defaultGammafile = mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma";
                mp.fileDefaultGammaV(false);


                if (mvars.FormShow[6] == false) { i3init = new i3_Init(); i3init.Show(); i3init.Left = 300; }
                i3init.Visible = true;
                mvars.mvWn = 0;


                //mvars.msWn = "Please confirm information @ FullDarkPattern" + "\r\n" + "\r\n" +
                //        "    Meter: " + mCAs.CAATG.Class + " CH." + mvars.UUT.CAch.ToString("00") +
                //        " x" + mCAs.CAATG.OverBet + "\r\n" + "    UUT.ID: " + mvars.UUT.ID + "\r\n" +
                //        "    Resolution: " + mvars.UUT.resW.ToString() + "x" + mvars.UUT.resH.ToString() + "\r\n" + "\r\n" +
                //        "    CAUTION        " + lbl_MTP.Text + " Mode" + "\r\n" +
                //        "    CAUTION        Final judge with GammaRead Mode" + "\r\n" + "\r\n" +
                //        "    Show DarkPattern and Stanby ! ";
                if (mvars.flgdirGamma == false)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US")
                    {
                        i3_Init.btn_1.Text = "Start (&Y)";
                        i3_Init.btn_0.Text = "Exit (&N)";
                        mvars.msWn =
                            "    Meter: " + mCAs.CAATG.Class + " CH." + mvars.UUT.CAch.ToString("00") +
                            " x" + mCAs.CAATG.OverBet + "\r\n" + "    UUT.ID: " + mvars.UUT.ID + "\r\n" +
                            "    CAUTION        " + lbl_MTP.Text + " Mode" + "\r\n" +
                            "               Show DarkPattern and Stanby ! ";
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    {
                        i3_Init.btn_1.Text = "開始 (&是)";
                        i3_Init.btn_0.Text = "取消 (&否)";
                        string svm = "暫寫模式";
                        if (lbl_MTP.Text.ToUpper() == "ENMTP") { svm = "燒錄模式"; }
                        mvars.msWn =
                            "    量測探頭: " + mCAs.CAATG.Class + " 頻道." + mvars.UUT.CAch.ToString("00") +
                            " x" + mCAs.CAATG.OverBet + "\r\n" + "    燈板ID: " + mvars.UUT.ID + "\r\n" +
                            "    解析度: " + mvars.UUT.resW.ToString() + "x" + mvars.UUT.resH.ToString() + "\r\n" + "\r\n" +
                            "    警告        " + svm + "\r\n" + "\r\n" + "\r\n" +
                            "               倒數計時中 ! ";
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    {
                        i3_Init.btn_1.Text = "开始 (&是)";
                        i3_Init.btn_0.Text = "取消 (&否)";
                        string svm = "暂写模式";
                        if (lbl_MTP.Text.ToUpper() == "ENMTP") { svm = "烧录模式"; }
                        mvars.msWn =
                            "    量测探头: " + mCAs.CAATG.Class + " 频道." + mvars.UUT.CAch.ToString("00") +
                            " x" + mCAs.CAATG.OverBet + "\r\n" + "    灯板ID: " + mvars.UUT.ID + "\r\n" +
                            "    解析度: " + mvars.UUT.resW.ToString() + "x" + mvars.UUT.resH.ToString() + "\r\n" + "\r\n" +
                            "    警告        " + svm + "\r\n" + "\r\n" + "\r\n" +
                            "               倒数计时中 ! ";
                    }
                }
                else
                {
                    i3_Init.btn_1.Text = "Start (&Y)";
                    i3_Init.btn_0.Text = "Exit (&N)";
                    mvars.msWn =
                        "    Meter: " + mCAs.CAATG.Class + " CH." + mvars.UUT.CAch.ToString("00") +
                        " x" + mCAs.CAATG.OverBet + "\r\n" + "    UUT.ID: " + mvars.UUT.ID + "\r\n" +
                        "    GammaCurve measure  " + "\r\n" + "\r\n" +
                        "    from ( g " + mvars.msrgammacurveSt + " )，to ( g " + mvars.msrgammacurveEd + " )，step ( g " + mvars.msrgammacurveGp + " )";
                }


                i3_Init.lbl_1.Text = mvars.msWn;
                i3_Init.lbl_1.Visible = true;
                i3init.tme_Warn.Enabled = true;
                do
                {
                    Application.DoEvents();
                    mp.doDelayms(100);
                } while (i3init.Visible == true);
                lst_get.Items.RemoveAt(lst_get.Items.Count - 1);
                if (mvars.msWn == "0")
                {
                    lst_get.Items.Add(" User Cancel @ CoolCounter");
                    lst_get.Items.Add(" --> Please re-AutoGamma"); lst_get.TopIndex = lst_get.Items.Count - 1;
                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
                    if (mvars.FormShow[5]) { Form2.i3pat.lbl_Mark.Text = ""; Form2.i3pat.Close(); }
                    return;
                }

                if (mvars.FormShow[5]) Form2.i3pat.lbl_Mark.Text = "";
                Form1.tslblStatus.Text = mvars.verMCU + "，" + mvars.verFPGA;
                lst_get.Items.Add(" -> " + btn_PAGMA.Text);
                mp.doDelayms(100);
                mvars.flgSelf = true;

                btn_Break.Enabled = true;

                if (mvars.deviceID.Substring(0, 2) == "05") mp.cAGMAPrimary();


                if (mvars.strReceive.IndexOf("DONE", 0) > -1)
                {
                    string[] svss = mvars.strReceive.Split(',');
                    uc_atg.lstget.Items.Add("  ↑ ATG DONE，" + svss[7] + "s");
                }
                else
                {
                    string[] svss = mvars.strReceive.Split(',');
                    uc_atg.lstget.Items.Add("  ↑ ATG Fail，" + svss[7] + "s，errCode" + mvars.errCode);
                }
                btn_Break.Enabled = false;
                btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled;
            }           
        }

        private void btn_PAGMA_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string value = " autogamma";
                if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                        "    Change RUN keyword ?" + "\r\n" + "\r\n" + " ex. autogamma" +
                        "\r\n" + "\r\n" + " ex. gammacurve1 = gammacurve,0,41,1" + "\r\n" + " ex. gammacurve2 = gammacurve,0,255,1"
                        , ref value, 139) == DialogResult.Cancel)
                {
                    lstget.Items.Add(" -> User Canceled");
                    lstget.Items.Add(" --> Please re-AutoGamma");
                    mp.funSaveLogs("(pAGMA) User break @ Change run keyword");
                    btn_PAGMA.Enabled = true; txt_UUTID.Enabled = btn_PAGMA.Enabled; chk_ZeroCAL.Enabled = btn_PAGMA.Enabled; btn_Break.Enabled = false; return;
                }
                else
                {
                    if (value.ToLower().IndexOf("autogamma", 0) != -1) { btn_PAGMA.Text = "    RUN AutoGamma"; }
                    else if (value.ToLower().IndexOf("gammacurve", 0) != -1) { btn_PAGMA.Text = "GammaCurve" + value.Substring("gammacurve".Length, value.Length - "gammacurve".Length); }
                }
            }
        }

        private void btn_Break_Click(object sender, EventArgs e)
        {
            mvars.Break = true;
        }

        private void btn_Break_KeyUp(object sender, KeyEventArgs e)
        {
            Button btn = (Button)sender;
            if ((btn.Text.IndexOf("NEXT", 0) != -1 || btn.Text.IndexOf("下一步", 0) != -1) && (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space || e.KeyCode == Keys.Escape))
            {
                mvars.Break = true;
            }
        }

        private void txt_UUTID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && mvars.deviceID.Substring(0,2) != "06") { btn_PAGMA_Click(null, null); }
        }

        private void lbl_GMAvalue_DoubleClick(object sender, EventArgs e)
        {
            string value = mvars.GMAvalue.ToString();
            if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                "    New Gamma value", ref value, 39) == DialogResult.Cancel) { return; }//MessageBox.Show("Please input UUT ID", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            else
            {
                if (mp.IsNumeric(value))
                {
                    if (Convert.ToSingle(value) >= 1 && Convert.ToSingle(value) <= 5)
                    {
                        mvars.GMAvalue = Convert.ToSingle(value); lbl_GMAvalue.Text = value;

                        int svuutbetlv = int.Parse(lbl_CLv.Text);
                        string fnum;
                        mvars.Gamma2d2[0] = 0; dgvatg.Rows[7].Cells[0].Value = 0;
                        for (byte svi = 1; svi < mvars.cm603Gamma.Length-1; svi++)
                        {
                            //mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), mvars.GMAvalue));
                            //if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 0.001)
                            //    fnum = String.Format("{0:#.0#####}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            //else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 1 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 0.001)
                            //    fnum = String.Format("{0:#.0##}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            //else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 10 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 1)
                            //    fnum = String.Format("{0:0.0#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            //else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 100 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 10)
                            //    fnum = String.Format("{0:0.#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            //else
                            //    fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            //dgvatg.Rows[7].Cells[svi].Value = fnum;
                            
                            if (svi == 1) mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), 2.5));
                            else mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), mvars.GMAvalue));

                            if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 100)
                                fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 100 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 10)
                                fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 10 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 1)
                                fnum = String.Format("{0:###0.#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 1 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 0.1)
                                fnum = String.Format("{0:0.##}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            else 
                                fnum = String.Format("{0:0.###}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                            dgvatg.Rows[7].Cells[svi].Value = fnum;
                        }
                        mvars.Gamma2d2[0] = 0;
                        mvars.Gamma2d2[mvars.cm603Gamma.Length - 1] = 100;
                        //mvars.Gamma2d2[mvars.cm603Gamma.Length - 1] = 100; mvars.Gamma2d2[mvars.cm603Gamma.Length - 1] = svuutbetlv;
                    }   
                }
            }
        }

        private void lbl_dLvlimit_DoubleClick(object sender, EventArgs e)
        {
            //string value = mvars.UUT.DLvLimit.ToString();
            //if (mp.InputBox("AutoGamma", "Change DLv spec.(typ)", ref value, 100) == DialogResult.Cancel) { return; }
            //else
            //{
            //    mvars.UUT.DLvLimit = Convert.ToSingle(value);
            //    lbl_dLvlimit.Text = value;
            //}
        }

        private void lbl_dLvlimit_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string value = mvars.UUT.DLvLimit + "," + mvars.UUT.DLvTolminus + "," + mvars.UUT.DLvTolplus;
                if (mp.InputBox("AutoGamma", "Change DLv spec.(Limit,Tolrence minus,Tolrence plus", ref value, 100) == DialogResult.Cancel) { return; }
                else
                {
                    if (value.Split(',').Length == 3)
                    {
                        mvars.UUT.DLvLimit = Convert.ToSingle(value.Split(',')[0]);
                        mvars.UUT.DLvTolminus = Convert.ToSingle(value.Split(',')[1]);
                        mvars.UUT.DLvTolplus = Convert.ToSingle(value.Split(',')[2]);

                        lbl_dLvlimit.Text = string.Format("{0:#0.0####}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus));
                        lbl_dLvmax.Text = string.Format("{0:#0.0####}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus));
                    }
                }
            }
        }

        private void lbl_cm603Vref_DoubleClick(object sender, EventArgs e)
        {
            string svs;
            if (mvars.deviceNameSub == "B(4)" || mvars.deviceID.Substring(0, 2) == "05") svs = "    Please input Vref (R,G,B,M)";
            else svs = "    Please input Vref (R,G,B)";


            //文字置換(string.Empty)
            lbl_cm603Vref.Text = lbl_cm603Vref.Text.Trim();
            string value = Regex.Replace(lbl_cm603Vref.Text, "v", ",");
            if (value.Substring(value.Length - 1, 1) == ",") { value = value.Substring(0, value.Length - 1); }
            if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                    svs, ref value, 69) == DialogResult.Cancel) { return; }
            else
            {
                string[] Svss = value.Split(',');
                if (Svss[0] == "" || Svss[1] == "" || Svss[2] == "") { return; }
                for (int svi = 0; svi <= 2; svi++)
                {
                    mvars.cm603Vref[svi] = Convert.ToSingle(Svss[svi]);
                    uint svcm603code = Convert.ToUInt16(Math.Round(mvars.cm603Vref[svi] / 0.01953, 0));

                    UInt32 ulCRC = mp.CRC_Cal(svcm603code, (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    string svbinS = "00" + (mp.DecToBin((int)ulCRC, 16).Substring(2, 4)) + mp.DecToBin((int)svcm603code, 10);
                    mvars.cm603df[svi, (byte)0x1F * 2] = mp.BinToHex(svbinS.Substring(0, 8), 2);
                    mvars.cm603df[svi, (byte)0x1F * 2 + 1] = mp.BinToHex(svbinS.Substring(8, 8), 2);
                    mvars.cm603dfB[svi, (byte)0x1F * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, (byte)0x1F * 2]);
                    mvars.cm603dfB[svi, (byte)0x1F * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, (byte)0x1F * 2 + 1]);
                }
                //lstget.Items.Add("");
                //lstget.Items.Add("Vref_Nud " + mvars.in602cVrefNud[0]);
                //lstget.Items.Add("TFT_Vref_Nud " + mvars.in602cVrefNud[1]);
                //lstget.Items.Add("TFT_Vreset_Nud " + mvars.in602cVrefNud[2]);
                //lstget.Items.Add("Gma0_Nud " + mvars.in602cVrefNud[3]);
                //lstget.Items.Add("");
            }
        }

        private void lbl_CLv_DoubleClick(object sender, EventArgs e)
        {
            string value = lbl_CLv.Text;
            if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                "    New Lv limit", ref value, 39) == DialogResult.Cancel) { return; }//MessageBox.Show("Please input UUT ID", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            else
            {
                if (mp.IsNumeric(value))
                {
                    if (Convert.ToSingle(value) >= 1)
                    {
                        mvars.UUT.CLv = Convert.ToSingle(value); lbl_CLv.Text = value;

                        //String fnum;
                        //float svuutbetlv = (float)(mvars.UUT.CLv * 1.005);
                        //for (int svi = 0; svi < mvars.in602cBLgamma.Length; svi++)
                        //{
                        //    mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.in602cBLgamma[svi] / 255, 8), mvars.GMAvalue));
                        //    if (mvars.Gamma2d2[svi] < 10) { fnum = String.Format("{0:##0.0#}", mvars.Gamma2d2[svi]); }
                        //    else { fnum = String.Format("{0:##0.0}", mvars.Gamma2d2[svi]); }
                        //    dgvatg.Rows[8].Cells[svi].Value = mvars.pGMA.Data[0, 2 * svi + 0];
                        //    dgvatg.Rows[9].Cells[svi].Value = mvars.pGMA.Data[0, 2 * svi + 1];
                        //    dgvatg.Rows[0].Cells[svi].Value = String.Format("{0:###0.0}", mvars.Gamma2d2[svi]);


                        //    if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 0.009) fnum = String.Format("{0:0.0000}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        //    else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 1 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 0.009)
                        //        fnum = String.Format("{0:0.0##}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        //    else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 10 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 1)
                        //        fnum = String.Format("{0:0.0#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        //    else if ((svuutbetlv * mvars.Gamma2d2[svi] / 100) < 100 && (svuutbetlv * mvars.Gamma2d2[svi] / 100) >= 10)
                        //        fnum = String.Format("{0:0.#}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));
                        //    else
                        //        fnum = String.Format("{0:###0}", (svuutbetlv * mvars.Gamma2d2[svi] / 100));

                        //    if (svi > 0 && Convert.ToSingle(fnum) == 0) { if (svi == (mvars.cm603Gamma.Length - 1)) { dgvatg.Rows[7].Cells[svi].Value = svuutbetlv; } }
                        //    else { dgvatg.Rows[7].Cells[svi].Value = fnum; }

                        //}
                    }
                }
            }
        }

        private void lbl_L0ADJ_DoubleClick(object sender, EventArgs e)
        {
            string value = lbl_L0ADJ.Text;

            if (mvars.deviceNameSub == "B(4)" || mvars.deviceID.Substring(0, 2) == "05")
            {
                value = Regex.Replace(value, " ", "");
                value = Regex.Replace(value, "v", ",");
                value = value.Substring(0, value.Length - 1);
                if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                                    "    Please g8-g1 delta voltage", ref value, 59) == DialogResult.Cancel) { return; }
                else
                {
                    Regex.Replace(value, " ", ",");
                    string[] Svss = value.Split(',');
                    if (Svss.Length != 3) { return; }
                    if (Svss.Length != 3) { return; }
                    else
                    {
                        if (Svss[0] == "" || Svss[1] == "" || Svss[2] == "") { return; }
                        for (int svi = 0; svi <= 2; svi++)
                        {
                            if (Convert.ToSingle(Svss[svi]) == 0) { Svss[svi] = "0.05"; }
                            mvars.cgma1dv[mvars.dualduty, svi] = Convert.ToSingle(Svss[svi]);
                        }
                        lbl_L0ADJ.Text = value;
                        lbl_L0ADJ.Text = mvars.cgma1dv[mvars.dualduty, 0].ToString() + "v " + mvars.cgma1dv[mvars.dualduty, 1].ToString() + "v " + mvars.cgma1dv[mvars.dualduty, 2].ToString() + "v";
                    }
                }
            }
            else
            {
                if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                                    "    Please input deltaXYZdg parameter(X,Y,Z)", ref value, 59) == DialogResult.Cancel) { return; }
                else
                {
                    string[] Svss = value.Split(',');
                    if (Svss.Length != 3) { return; }
                    else
                    {
                        if (Svss[0] == "" || Svss[1] == "" || Svss[2] == "") { return; }
                        for (int svi = 0; svi <= 2; svi++)
                        {
                            if (Convert.ToSingle(Svss[svi]) == 0) { Svss[svi] = "0.0005"; }
                            mvars.deltadtXYZdg[svi] = Convert.ToSingle(Svss[svi]);
                        }
                        lbl_L0ADJ.Text = value;
                        lbl_L0ADJ.Text = mvars.deltadtXYZdg[0].ToString() + "," + mvars.deltadtXYZdg[1].ToString() + "," + mvars.deltadtXYZdg[2].ToString();
                    }
                }
            }        
        }

        private void lbl_DD_DoubleClick(object sender, EventArgs e)
        {
            mvars.byPass = true;
        }

        private void lbl_CAmsr_DoubleClick(object sender, EventArgs e)
        {
            if (mCAs.CAATG.Demo == false && mCAs.CAATG.PlugSP) { mCAs.CAremote(true); }
            mp.CAmeasF();
            MessageBox.Show("Lv " + mp.CAFxLv + "，x " + mp.CAFx + "，y " + mp.CAFy);
            if (mCAs.CAATG.Demo == false && mCAs.CAATG.PlugSP) { mCAs.CAremote(false); }
        }

        private void lbl_ctime_DoubleClick(object sender, EventArgs e)
        {
            mvars.byPass = true;
        }

        private void btn_identify_Click(object sender, EventArgs e)
        {
            /// 暫時清除，需要時請由備份檔案中取回
        }

        
    }
}
