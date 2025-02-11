using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIO
{
    public partial class uc_ca410 : UserControl
    {
        static Label lblinfo = null;
        static ListBox lstmeas = null;
        static Button btndisconnect = null;
        static Button btnzerocal = null;
        static Button btnmeas = null;
        static Button btnautoconnect = null;
        static Button btnsetting = null;
        static Button btnbreak = null;
        static Label lbltimer = null;
        static GroupBox gboxbtn = null;
        //public static string[] CA4CH = new string[100];
        //public static string[] CA4chTx = new string[100];
        //public static string[] CA4chTy = new string[100];
        //public static string[] CA4chTLv = new string[100];
        static TabControl tabCAmode = new TabControl();
        static Label lblLv = null;
        static Label lblsx = null;
        static Label lblsy = null;
        static Label lblBX = null;
        static Label lblBY = null;
        static Label lblBZ = null;
        static Label lblJEITA = null;
        static Label lblFMA = null;
        public static ComboBox cmbMeastimes = null;
        static NumericUpDown numUDtimes = null;
        static NumericUpDown numUDtime = null;
        public static DataGridView dgvSBList = new DataGridView();
        public static BindingSource dgvSBListBS = null;
        public static string[] msrData = new string[32768];
        public static int msrCount = 0;
        ToolTip toolTips = new ToolTip();
        static bool Break = false;


        public uc_ca410()
        {
            InitializeComponent();
            initForm();
            InitScreenInfoDGV();
        }

        private void InitScreenInfoDGV()
        {

            this.Controls.AddRange(new Control[] { dgvSBList });
            Font fnt = new Font("Arial", 8);
            dgvSBList.Font = fnt;
            dgvSBList.AllowUserToAddRows = false;
            dgvSBList.AllowUserToDeleteRows = false;
            dgvSBList.AllowUserToResizeColumns = false;
            dgvSBList.AllowUserToResizeRows = false;
            dgvSBList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            dgvSBList.BackgroundColor = System.Drawing.Color.AliceBlue;

            System.Windows.Forms.DataGridViewCellStyle dgvCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();

            dgvCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            dgvCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dgvCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dgvCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgvCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dgvCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvSBList.ColumnHeadersDefaultCellStyle = dgvCellStyle1;
            //dgvSBList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSBList.EnableHeadersVisualStyles = false;
            dgvSBList.Location = new System.Drawing.Point(268, 65);
            dgvSBList.MultiSelect = false;
            dgvSBList.Name = "dataGridView_SBList";
            dgvSBList.ReadOnly = true;
            dgvSBList.RowHeadersVisible = false;
            dgvSBList.RowTemplate.Height = 23;
            dgvSBList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSBList.Size = new System.Drawing.Size(386, 133);
            dgvSBList.TabIndex = 13;

            dgvSBList.Columns.Clear();

            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "No";
            column.HeaderText = "No";
            column.Name = "No";
            column.ReadOnly = true;
            column.Width = 65;
            column.Tag = "No";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "X";
            column.HeaderText = "X";
            column.Name = "X";
            column.ReadOnly = true;
            column.Width = 55;
            column.Tag = "X";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Y";
            column.HeaderText = "Y";
            column.Name = "Y";
            column.ReadOnly = true;
            column.Width = 55;
            column.Tag = "Y";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Z";
            column.HeaderText = "Z";
            column.Name = "Z";
            column.ReadOnly = true;
            column.Width = 55;
            column.Tag = "Z";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "x";
            column.HeaderText = "x";
            column.Name = "x";
            column.ReadOnly = true;
            column.Width = 50;
            column.Tag = "x";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "y";
            column.HeaderText = "y";
            column.Name = "y";
            column.ReadOnly = true;
            column.Width = 50;
            column.Tag = "y";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Lv";
            column.HeaderText = "Lv";
            column.Name = "Lv";
            column.ReadOnly = true;
            column.Width = 55;
            column.Tag = "Lv";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "up";
            column.HeaderText = "u'";
            column.Name = "up";
            column.ReadOnly = true;
            column.Width = 50;
            column.Tag = "up";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "vp";
            column.HeaderText = "v'";
            column.Name = "vp";
            column.ReadOnly = true;
            column.Width = 50;
            column.Tag = "vp";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "date";
            column.HeaderText = "date";
            column.Name = "date";
            column.ReadOnly = true;
            column.Width = 65;
            column.Tag = "date";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "time";
            column.HeaderText = "time";
            column.Name = "time";
            column.ReadOnly = true;
            column.Width = 65;
            column.Tag = "time";
            column.Resizable = DataGridViewTriState.False;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvSBList.Columns.Add(column);
            dgvSBList.AutoGenerateColumns = false;
            dgvSBList.BringToFront();
            //string[] svdata = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11" };
            //dgvSBList.Rows.Add(svdata);
        }


        private void uc_ca410_Load(object sender, EventArgs e)
        {
            mvars.actFunc = "ca410";
            mvars.FormShow[16] = true;
            mCA4.CAp.CAchID = "0";

        }

        public void initForm()
        {
            tabCAmode = tabControl1;

            lblinfo = lbl_info;

            lstmeas = listBox1;
            btnautoconnect = button1;
            btnzerocal = button2;
            btndisconnect = button3;
            btnmeas = button4;
            btnsetting = button5;
            btnbreak = button6;
            lbltimer = label1;
            gboxbtn = gbox_btn;

            cmb_MeasTimes.Text = cmb_MeasTimes.Items[0].ToString();

            lblLv = lbl_Lv;
            lblsx = lbl_sx;
            lblsy = lbl_sy;
            lblBX = lbl_BX;
            lblBY = lbl_BY;
            lblBZ = lbl_BZ;

            lblJEITA = lbl_JEITA;
            lblFMA = lbl_FMA;

            cmbMeastimes = cmb_MeasTimes;

            numUDtimes = numUD_Times;
            numUDtime = numUD_Time;

            //if (Sync_Select == 4) { lblinfo.Text = "[ " + strSyncmode[Sync_Select] + "    " + Sync_Fq + "Hz"; }
            //else if (Sync_Select == 5) { lblinfo.Text = "[ " + strSyncmode[Sync_Select] + "    " + Sync_msec + "msec"; }
            //else { lblinfo.Text = "[ " + strSyncmode[Sync_Select] + "    "; }
            //lblinfo.Text += " ]  [ " + strSped[SPD_Select] + " ]  [ " + CA4CH[mCA4.CAp.CAchNo] + " ]";

            label1.Text = "";

            btnautoconnect.Enabled = !mCA4.CAp.PlugSP;
            btnzerocal.Enabled = mCA4.CAp.PlugSP;
            btndisconnect.Enabled = mCA4.CAp.PlugSP;
            btnmeas.Enabled = mCA4.CAp.PlugSP;
            btnsetting.Enabled = mCA4.CAp.PlugSP;
        }


        //private static void GetErrorMessage(int errornum)
        //{
        //    string errormessage = "";
        //    if (errornum != 0)
        //    {
        //        //Get Error message from Error number
        //        err = GlobalFunctions.CASDK2_GetLocalizedErrorMsgFromErrorCode(0, errornum, ref errormessage);
        //        Console.WriteLine(errormessage);
        //    }

        //}

        public static void getJeita(string svh)
        {
            double JEITA = 0.0;

            string sverr = mCA4.geterrMsg(mCA4._objCa.Measure());
            if (sverr != "OK") { lstmeas.Items.Add("measurement " + sverr + ",Return"); return; }
            sverr = mCA4.geterrMsg(mCA4._objProbe.get_FlckrJEITA(ref JEITA));
            if (sverr != "OK") { lstmeas.Items.Add("get_FlckrJEITA " + sverr + ",Return"); return; }
            lblJEITA.Text = JEITA.ToString("###0.0###");
        }
        public static void getFMA(string svh)
        {
            double FMA = 0.0;

            string sverr = mCA4.geterrMsg(mCA4._objCa.Measure());
            if (sverr != "OK") { lstmeas.Items.Add("measurement " + sverr + ",Return"); return; }
            sverr = mCA4.geterrMsg(mCA4._objProbe.get_FlckrFMA(ref FMA));
            if (sverr != "OK") { lstmeas.Items.Add("get_FlckrFMA " + sverr + ",Return"); return; }
            lblFMA.Text = FMA.ToString("###0.0");
        }
        public static void getLxy(string svh)
        {
            double Lv = 0.0;
            double sx = 0.0;
            double sy = 0.0;
            double X = 0.0;
            double Y = 0.0;
            double Z = 0.0;
            double ud = 0.0;
            double vd = 0.0;
            string sverr = mCA4.geterrMsg(mCA4._objCa.Measure());
            if (sverr != "OK") { lstmeas.Items.Add("measurement " + sverr + ",Return"); return; }
            if (svh == "") { msrData[msrCount] = string.Format("{0:000}", msrCount); } else msrData[msrCount] = svh;
            //mCA4.GetErrorMessage(objProbe.get_X(ref X));
            mCA4._objProbe.get_X(ref X);
            if (X < 10) { lblBX.Text = X.ToString("###0.0###"); } else { lblBX.Text = X.ToString("###0.0#"); }
            msrData[msrCount] += "," + lblBX.Text;
            //mCA4.GetErrorMessage(objProbe.get_Y(ref Y));
            mCA4._objProbe.get_Y(ref Y);
            if (Y < 10) { lblBY.Text = Y.ToString("###0.0###"); } else { lblBY.Text = Y.ToString("###0.0#"); }
            msrData[msrCount] += "," + lblBY.Text;
            //mCA4.GetErrorMessage(objProbe.get_Z(ref Z));
            mCA4._objProbe.get_Z(ref Z);
            if (Z < 10) { lblBZ.Text = Z.ToString("###0.0###"); } else { lblBZ.Text = Z.ToString("###0.0#"); }
            msrData[msrCount] += "," + lblBZ.Text;
            //mCA4.GetErrorMessage(objProbe.get_sx(ref sx));
            mCA4._objProbe.get_sx(ref sx);
            lblsx.Text = sx.ToString("###0.0##"); 
            msrData[msrCount] += "," + lblsx.Text;

            //mCA4.GetErrorMessage(objProbe.get_sy(ref sy));
            mCA4._objProbe.get_sy(ref sy);
            lblsy.Text = sy.ToString("##0.0##"); 
            msrData[msrCount] += "," + lblsy.Text;

            //sverr = mCA4.geterrMsg(objProbe.get_Lv(ref Lv));
            mCA4._objProbe.get_Lv(ref Lv);
            if (Lv < 10) { lblLv.Text = Lv.ToString("###0.0###"); } else { lblLv.Text = Lv.ToString("###0.0#"); }
            msrData[msrCount] += "," + lblLv.Text;
            //mCA4._mCA4.GetErrorMessage(mCA4._objProbe.get_ud(ref ud));
            mCA4._objProbe.get_ud(ref ud);
            msrData[msrCount] += "," + ud.ToString("###0.0##");
            //mCA4.GetErrorMessage(mCA4._objProbe.get_vd(ref vd)); 
            mCA4._objProbe.get_vd(ref vd);
            msrData[msrCount] += "," + vd.ToString("###0.0##");
            msrData[msrCount] += "," + DateTime.Now.Month + "." + DateTime.Now.Day;
            msrData[msrCount] += "," + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
        }
        private static void Measurement()
        {
            string sverr = "";

            if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "COL")
            {
                sverr = mCA4.geterrMsg(mCA4._objCa.put_DisplayMode(mCA4.MODE_Lvxy));
                if (sverr != "OK") { lstmeas.Items.Add("put_MODE_Lvxy " + sverr + ",Return"); return; }
            }
            else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "JEI")
            {
                sverr = mCA4.geterrMsg(mCA4._objCa.put_DisplayMode(mCA4.MODE_JEITA));
                if (sverr != "OK") { lstmeas.Items.Add("put_MODE_JEITA " + sverr + ",Return"); return; }
            }
            else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "FMA")
            {
                sverr = mCA4.geterrMsg(mCA4._objCa.put_DisplayMode(mCA4.MODE_FMA));
                if (sverr != "OK") { lstmeas.Items.Add("put_MODE_FMA " + sverr + ",Return"); return; }
            }


            if (cmbMeastimes.SelectedIndex == 1)
            {
                #region Interval-Times
                Array.Clear(msrData, 0, msrData.Length);
                msrCount = 0;
                lbltimer.Text = DateTime.Now.ToString();
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                do
                {
                    if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "COL") { getLxy(msrCount.ToString()); }
                    else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "JEI") { getJeita(""); }
                    else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "FMA") { getFMA(""); }
                    if ((int)numUDtime.Value > 0) { mp.doDelayms((int)numUDtime.Value * 1000); }
                    msrCount++;
                    if (Break) break;
                } while (msrCount < 32768);
                Break = false;
                lbltimer.Text += " use " + sw.ElapsedMilliseconds + "ms";
                #endregion
            }
            else if (cmbMeastimes.SelectedIndex == 2)
            {
                #region Interval-Times
                Array.Clear(msrData, 0, msrData.Length);
                msrCount = 0;
                lbltimer.Text = DateTime.Now.ToString();
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                do
                {
                    if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "COL") { getLxy(""); }
                    else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "JEI") { getJeita(""); }
                    else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "FMA") { getFMA(""); }
                    if ((int)numUDtime.Value > 0) { mp.doDelayms((int)numUDtime.Value * 1000); }
                    msrCount++;
                    if (Break) break;
                } while (msrCount < (int)numUDtimes.Value);
                Break = false;
                lbltimer.Text += " use " + sw.ElapsedMilliseconds + "ms";
                #endregion
            }
            else if (cmbMeastimes.SelectedIndex == 3)
            {
                #region Interval-Time
                Array.Clear(msrData, 0, msrData.Length);
                msrCount = 0;
                lbltimer.Text = DateTime.Now.ToString();
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                do
                {
                    if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "COL") { getLxy(""); }
                    else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "JEI") { getJeita(""); }
                    else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "FMA") { getFMA(""); }
                    msrCount++;
                    if (Break) break;
                } while (sw.Elapsed.TotalMilliseconds <= (int)numUDtime.Value * 1000);
                Break = false;
                lbltimer.Text += " ~ " + DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
                #endregion
            }
            else if (cmbMeastimes.SelectedIndex == 0)
            {
                #region Single
                if (msrCount >= 32768) { MessageBox.Show("Max count 32768"); return; }
                if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "COL") { getLxy(msrCount.ToString()); }
                else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "JEI") { getJeita(""); }
                else if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "FMA") { getFMA(""); }
                msrCount++;

                #endregion
            }



            //GetErrorMessage(objCa.put_DisplayMode(MODE_Lvxy));  //Set mode:Color Lvxy
        }


        private void button1_Click(object sender, EventArgs e)      // connect 
        {
            mCA4.AutoConnect();

            if (mCA4.autoconnectflag)
            {
                lblinfo.Text += " ]  [ " + mCA4.strSped[mCA4.SPD_Select] + " ]  [ " + mCA4.CA4CH[mCA4.CAp.CAchNo] + " ]";

                btnautoconnect.Enabled = !mCA4.autoconnectflag;
                btnzerocal.Enabled = mCA4.autoconnectflag;
                btndisconnect.Enabled = mCA4.autoconnectflag;
                btnmeas.Enabled = mCA4.autoconnectflag;
                btnsetting.Enabled = mCA4.autoconnectflag;

            }
        }

        public static Timer timer = null;

        private void button2_Click(object sender, EventArgs e)      //0-cal 
        {
            this.Enabled = false;
            mCA4.CAzero();
            this.Enabled = true;
            if (mCAs.msg == "OK") 
            {
                btnmeas.Enabled = true;
                btnsetting.Enabled = true;
            }
            else
            {
                btnmeas.Enabled = false;
                btnsetting.Enabled = false;
            }
            //mp.killMSGname = "0-CAL";
            //mp.killMSGsec = 2;
            //mp.KillMessageBoxStart();
            //MessageBox.Show(svstr + "，" + mCA4.CAp.Class + "\r\n" + "\r\n" + "MessageBox close @ " + mUser.killMSGsec + "s later", "0-CAL");
        }

        private void button3_Click(object sender, EventArgs e)      // disconnect 
        {
            mCAs.CAremote(false);
            btnautoconnect.Enabled = !mCA4.CAp.PlugSP;
            btnzerocal.Enabled = mCA4.CAp.PlugSP;
            btndisconnect.Enabled = mCA4.CAp.PlugSP;
            btnmeas.Enabled = mCA4.CAp.PlugSP;
            btnsetting.Enabled = mCA4.CAp.PlugSP;
        }

        private void button4_Click(object sender, EventArgs e)      // measure 
        {
            if (mCAs.CAATG.Zeroed == false)
            {
                this.Enabled = false;
                mCA4.CAzero();
                this.Enabled = true;
                return;
            }


            btnbreak.Enabled = true;
            gboxbtn.Enabled = false;

            mp.CAmeasF();
            if (mp.CAFxLv == -1 && mCAs.CAATG.CAsel == 1) { mCA4.CAzero(); }
            Measurement();
            if (tabCAmode.TabPages[tabCAmode.SelectedIndex].Text.Trim().Substring(0, 3) == "COL")
            {
                if (cmbMeastimes.SelectedIndex == 0)
                {
                    dgvSBList.Rows.Add(msrData[msrCount - 1].Split(','));
                    dgvSBList.FirstDisplayedScrollingRowIndex = dgvSBList.Rows.Count - 1;
                    dgvSBList.Rows[0].Selected = false;
                }
                else
                {
                    for (int i = 0; i < msrCount; i++)
                    {
                        dgvSBList.Rows.Add(msrData[i].Split(','));
                    }
                    dgvSBList.FirstDisplayedScrollingRowIndex = dgvSBList.Rows.Count - 1;
                    dgvSBList.Rows[0].Selected = false;
                }
            }
            gboxbtn.Enabled = true;
            btnbreak.Enabled = false;
        }
    }
}
