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
    public partial class uc_MCU : UserControl
    {

        public uc_MCU()
        {
            InitializeComponent();
        }

        private void markreset(int svtotalcounts, bool svdelfb, bool selfrun)
        {
            //mvars.lstget.Items.Clear();
            mvars.t1 = DateTime.Now;
            mvars.strReceive = "";
            mvars.lCounts = svtotalcounts;
            mvars.lCount = 0;
            Array.Resize(ref mvars.lCmd, svtotalcounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, svtotalcounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.flgDelFB = svdelfb;
            Form1.tslblStatus.Text = "";
            mvars.flgSend = false;
            mvars.flgReceived = false;
            mvars.flgSelf = selfrun;
        }

        private void btn_verMCU_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                //if (mvars.deviceID.Substring(0, 2) == "05") { MessageBox.Show("Please select single side FPGA", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return;
            }

            this.Enabled = false;

            markreset(4, true, true);   //(1)如果是在Form1使用有效, 跳到mp則先隨便設

            lbl_verMCU.Text = "";
            lst_get1.Items.Clear();

            mvars.lblCmd = "MCU_VERSION"; 
            mp.mhVersion(); 
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lst_get1.Items.Add(mvars.lGet[mvars.lCount - 1]); }
            else
            {
                lbl_verMCU.Text = mvars.verMCU;
                if (mvars.verMCU.Substring(mvars.verMCU.Length-5,1) == "P") Form1.tslblStatus.Text = mvars.deviceID + "," + mvars.lblCmd + "," + mvars.verMCU;
                else
                {
                    //object[] I2C_CMD_C12A_Wall = {" 1-1_1"," 1-1_2"," 1-1_3"," 1-1_4",
                    //" 1-2_1"," 1-2_2"," 1-2_3"," 1-2_4",
                    //" 2-1_1"," 2-1_2"," 2-1_3"," 2-1_4",
                    //" 2-2_1"," 2-2_2"," 2-2_3"," 2-2_4",};
                    //cmb_nPB.Items.Clear();
                    //cmb_nPB.Items.AddRange(I2C_CMD_C12A_Wall);
                    //cmb_nPB.Text = " 1-1_1"; mvars.SercomCmdWr = 0x60; mvars.SercomCmdWrRd = 0x61;
                }
            }
            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
        }

        private void uc_MCU_Load(object sender, EventArgs e)
        {
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
                    mvars.toolTip1.SetToolTip(chk_SWOCP1st, @" Checked = Enable，");
                    mvars.toolTip1.SetToolTip(chk_SWOCP2nd, @" Checked = Enable，");
                    mvars.toolTip1.SetToolTip(chk_boardcast, @" Not linked to the home page");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    mvars.toolTip1.SetToolTip(chk_SWOCP1st, @" 勾選 = 啟用，");
                    mvars.toolTip1.SetToolTip(chk_SWOCP2nd, @" 勾選 = 啟用，");
                    mvars.toolTip1.SetToolTip(chk_boardcast, @" 未與主頁選項連動");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    mvars.toolTip1.SetToolTip(chk_SWOCP1st, @" 勾選 = 啟用，");
                    mvars.toolTip1.SetToolTip(chk_SWOCP2nd, @" 勾選 = 啟用，");
                    mvars.toolTip1.SetToolTip(chk_boardcast, @" 未与主页选项连动");
                }
                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                {
                    mvars.toolTip1.SetToolTip(chk_SWOCP1st, @" Checked = Enable，");
                    mvars.toolTip1.SetToolTip(chk_SWOCP2nd, @" Checked = Enable，");
                    mvars.toolTip1.SetToolTip(chk_boardcast, @" Not linked to the home page");
                }

            }
            #endregion tooltip

            cmb_deviceID.Items.Clear();
            for (int i = 0; i < Form1.cmbdeviceid.Items.Count; i++)
            {
                cmb_deviceID.Items.Add(Form1.cmbdeviceid.Items[i].ToString());
            }
            //chk_boardcast.Checked = Form1.chkboardcast.Checked;   //v0030取消連動
            mvars.actFunc = "MCU";
            mvars.FormShow[15] = true;
        }

        private void btn_idSerialmode_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                //if (mvars.deviceID.Substring(0, 2) == "05") { MessageBox.Show("Please select single side FPGA", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return;
            }

            this.Enabled = false;

            markreset(4, true, true);   //(1)如果是在Form1使用有效, 跳到mp則先隨便設
            
            string svdeviceID = mvars.deviceID;
            if (mvars.deviceID.Substring(0,2) == "05") mvars.deviceID = "05A0";
            label10.Text = mvars.deviceID;

            mvars.lblCmd = "PRIID_SERIESMODE";
            mp.mIDSERIESMODE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) Form1.tslblStatus.Text = "ERROR," + mvars.deviceID + "," + mvars.lblCmd;
            else Form1.tslblStatus.Text = mvars.deviceID + "," + mvars.lblCmd + ",DONE";

            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = svdeviceID;

            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
        }

        private void btn_autoID_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                //if (mvars.deviceID.Substring(0, 2) == "05") { MessageBox.Show("Please select single side FPGA", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return;
            }

            this.Enabled = false;

            markreset(4, true, true);   //(1)如果是在Form1使用有效, 跳到mp則先隨便設

            string svdeviceID = mvars.deviceID;
            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = "05A0";
            label10.Text = mvars.deviceID;
            mvars.lblCmd = "PRIID_AUTOID";
            mp.mAUTOID(txt_autoID.Text);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) Form1.tslblStatus.Text = "ERROR," + mvars.deviceID + "," + mvars.lblCmd;
            else Form1.tslblStatus.Text = mvars.deviceID + "," + mvars.lblCmd + ",DONE";

            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = svdeviceID;

            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
        }

        private void btn_wrgetDevID_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                //if (mvars.deviceID.Substring(0, 2) == "05") { MessageBox.Show("Please select single side FPGA", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return;
            }

            this.Enabled = false;

            markreset(4, true, true);   //(1)如果是在Form1使用有效, 跳到mp則先隨便設

            string svdeviceID = mvars.deviceID;
            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = "05A0";
            label10.Text = mvars.deviceID;
            mvars.lblCmd = "PRIID_WRITEID";
            mp.mWRGETDEVID();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) Form1.tslblStatus.Text = "ERROR," + mvars.deviceID + "," + mvars.lblCmd;
            else Form1.tslblStatus.Text = mvars.deviceID + "," + mvars.lblCmd + ",DONE";

            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = svdeviceID;

            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
        }

        private void tme_warn_Tick(object sender, EventArgs e)
        {

        }

        private void btn_BoxRead_Click(object sender, EventArgs e)
        {
            if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;

            //mvars.lstget.Items.Add("BOXREAD ....");
            mp.cBOXREAD(lst_get1);

            //if (mvars.errCode == "0") { mvars.lstget.Items.Add("BOXREAD,DONE,1"); }
            //else { mvars.lstget.Items.Add("BOXREAD,ERROR,1,ErrCode," + mvars.errCode); }
            //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
        }


        private void btn_IDON_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                //if (mvars.deviceID.Substring(0, 2) == "05") { MessageBox.Show("Please select single side FPGA", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return;
            }

            this.Enabled = false;
            Button btn = (Button)sender;
            markreset(4, true, true);   //(1)如果是在Form1使用有效, 跳到mp則先隨便設

            string svdeviceID = mvars.deviceID;
            if (chk_boardcast.Checked == true) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            //if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = "05A0";
            //if (mvars.deviceID.Substring(0, 2) == "06" && chk_boardcast.Checked) mvars.deviceID = "06A0";
            label10.Text = mvars.deviceID;
            if (btn.Tag.ToString() == "1") mvars.lblCmd = "PRIID_SHOW"; else mvars.lblCmd = "PRIID_OFF";
            mp.mpIDONOFF(Convert.ToByte(btn.Tag));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) Form1.tslblStatus.Text = "ERROR," + mvars.deviceID + "," + mvars.lblCmd;
            else Form1.tslblStatus.Text = mvars.deviceID + "," + mvars.lblCmd + ",DONE";

            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = svdeviceID;

            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
        }


        private void btn_OCPOnOff_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                //if (mvars.deviceID.Substring(0, 2) == "05") { MessageBox.Show("Please select single side FPGA", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return;
            }

            this.Enabled = false;
            markreset(4, true, true);   //(1)如果是在Form1使用有效, 跳到mp則先隨便設

            //chk_SWOCP1st.Checked = true;
            //chk_SWOCP2nd.Checked = true;

            string svdeviceID = mvars.deviceID;
            if (chk_boardcast.Checked) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";

            Byte OCP_En = 0;
            if (chk_SWOCP1st.Checked) OCP_En |= 0x01;
            if (chk_SWOCP2nd.Checked) OCP_En |= 0x02;
            mvars.lblCmd = "OCP_EnDis_Ctrl";
            mp.mOCP_EnDis_Ctrl(OCP_En);
            mvars.lblCmd = "MCU_VERSION";
            mp.mhVersion();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lst_get1.Items.Add(mvars.lGet[mvars.lCount - 1]); }
            else
            {
                lbl_verMCU.Text = mvars.verMCU;
            }
            if (mvars.demoMode == false) { mp.CommClose(); }

            //mp.cOCPONOFF(lst_get1, true);

            mvars.deviceID = svdeviceID;

            this.Enabled = true;
        }

        private void btn_SWRESET_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                //if (mvars.deviceID.Substring(0, 2) == "05") { MessageBox.Show("Please select single side FPGA", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return;
            }

            this.Enabled = false;
            markreset(4, true, true);   //(1)如果是在Form1使用有效, 跳到mp則先隨便設

            string svdeviceID = mvars.deviceID;
            if (chk_boardcast.Checked) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";

            mp.McuSW_Reset();

            mvars.deviceID = svdeviceID;

            this.Enabled = true;
        }

        private void btn_wrDevID_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                //if (mvars.deviceID.Substring(0, 2) == "05") { MessageBox.Show("Please select single side FPGA", mvars.strUInameMe + "_v" + mvars.UImajor); return; }

                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return;
            }

            this.Enabled = false;

            markreset(4, true, true);   //(1)如果是在Form1使用有效, 跳到mp則先隨便設

            string svdeviceID = mvars.deviceID;
            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = "05A0";
            label10.Text = mvars.deviceID;
            mvars.lblCmd = "PRIID_RESTORE";
            mp.mWRDEVID(Convert.ToInt16(txt_autoID.Text));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) Form1.tslblStatus.Text = "ERROR," + mvars.deviceID + "," + mvars.lblCmd;
            else Form1.tslblStatus.Text = mvars.deviceID + "," + mvars.lblCmd + ",DONE";

            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = svdeviceID;

            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
        }

        private void chk_boardcast_CheckedChanged(object sender, EventArgs e)
        {
            //Form1.chkboardcast.Checked = chk_boardcast.Checked;   //v0030取消連動
        }

        private void cmb_deviceID_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvars.deviceID = cmb_deviceID.Text.Trim();
        }
    }
}
