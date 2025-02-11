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

using System.Reflection;

namespace AIO
{
    public partial class uc_Flash : UserControl
    {
        public static TextBox txtFlashFileName = null;
        public static TextBox txtFlashFileNameFull = null;
        public static Button btnFLASHWRITE = null;
        public static Button btnFLASHREAD = null;
        public static Label lblBinChecksum = null;
        public static ComboBox cmbhPB = null;
        public static ComboBox cmbdeviceid = null;
        public static ComboBox cmbFlashSel = null;


        public uc_Flash()
        {
            InitializeComponent();
            cmbdeviceid = cmb_deviceID;
            cmbdeviceid.Items.Clear();
            for (int i = 0; i < Form1.cmbdeviceid.Items.Count; i++)
            {
                cmbdeviceid.Items.Add(Form1.cmbdeviceid.Items[i].ToString());
            }
            if (mvars.deviceID == "0300" && cmbdeviceid.FindString(" XB01") != -1) { int j = cmbdeviceid.FindString(" XB01"); cmbdeviceid.Text = cmbdeviceid.Items[j].ToString(); }
            
            cmbhPB = cmb_hPB;
            cmbhPB.Items.Clear();
            if (mvars.deviceID == "0200" || mvars.deviceID == "0400")
            {
                for (int i = 0; i < Form1.cmbhPB.Items.Count; i++)
                {
                    cmbhPB.Items.Add(Form1.cmbhPB.Items[i].ToString());
                    if (mvars.deviceID == "0400" && i >= 7) { break; }
                }
            }
            

            txtFlashFileName = txt_FlashFileName;
            txtFlashFileNameFull = txt_FlashFileNameFull;
            btnFLASHWRITE = btn_FLASHWRITE;
            btnFLASHREAD = btn_FLASHREAD;
            lblBinChecksum = lbl_BinChecksum;
            cmbhPB = cmb_hPB;
            cmbdeviceid = cmb_deviceID;
            cmbFlashSel = cmb_FlashSel;
        }

        private void callOnClick(Button btn)
        {
            //建立一個型別  
            Type t = typeof(Button);
            //引數物件  
            object[] p = new object[1];
            //產生方法  
            MethodInfo m = t.GetMethod("OnClick", BindingFlags.NonPublic | BindingFlags.Instance);
            //引數賦值。傳入函式  
            p[0] = EventArgs.Empty;
            //呼叫  
            m.Invoke(btn, p);
            return;

            /*
            //呼叫例子。  
            //呼叫Button1的onclick  
            callOnClick(Button1);

            //呼叫Button5的onclick  
            callOnClick(Button5);
            */

        }
        private void btn_FLASHREAD_Click(object sender, EventArgs e)
        {
            if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;

            string svs = "";

            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                //mvars.lstget.Items.Add(cmbFlashSel.Text + " Flash Read");
                Form1.lstget1.Items.Add(cmbFlashSel.Text + " Flash Read");
                lbl_fileofflashread.Text = @"...\Parameter\" + cmbFlashSel.Text + "_Read.bin";
                if (mvars.flashselQ > 0) mp.cFLASHREAD_pCB(cmbFlashSel.Text, lst_get1);
            }
            else
            {
                if (mvars.deviceID.IndexOf("05",0) != -1)
                {
                    //if (SelFlashRd_cbx.Text == "FPGA A")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 1; }
                    //else if (SelFlashRd_cbx.Text == "FPGA B")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 2; }
                    //else if (SelFlashRd_cbx.Text == "CB_1")
                    //{ FlashSize = 32 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 3; }
                    //else if (SelFlashRd_cbx.Text == "CB_2")
                    //{ FlashSize = 32 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 4; }
                    //else if (SelFlashRd_cbx.Text == "CB_3")
                    //{ FlashSize = 32 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 5; }
                    //else if (SelFlashRd_cbx.Text == "CB_4")
                    //{ FlashSize = 32 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 6; }
                    //else if (SelFlashRd_cbx.Text == "XB_1")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 7; QSPI_Rd1Bit_rdbtn.Checked = true; }
                    //else if (SelFlashRd_cbx.Text == "XB_2")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 8; QSPI_Rd1Bit_rdbtn.Checked = true; }
                    //else if (SelFlashRd_cbx.Text == "XB_3")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 9; QSPI_Rd1Bit_rdbtn.Checked = true; }
                    //else if (SelFlashRd_cbx.Text == "XB_4")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 10; QSPI_Rd1Bit_rdbtn.Checked = true; }
                    //else if (SelFlashRd_cbx.Text == "XB_5")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 11; QSPI_Rd1Bit_rdbtn.Checked = true; }
                    //else if (SelFlashRd_cbx.Text == "XB_6")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 12; QSPI_Rd1Bit_rdbtn.Checked = true; }
                    //else if (SelFlashRd_cbx.Text == "XB_7")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 13; QSPI_Rd1Bit_rdbtn.Checked = true; }
                    //else if (SelFlashRd_cbx.Text == "XB_8")
                    //{ FlashSize = 8 * 1024 * 1024; FlashSel_cbx.SelectedIndex = 14; QSPI_Rd1Bit_rdbtn.Checked = true; }
                    //else { FlashSel_cbx.SelectedIndex = 0; return; }
                }
                else
                {
                    if (mvars.c12aflashitem == 0 && mvars.flashselQ == 0) { MessageBox.Show("FlashType @ OPEN，Please Select \"FPGA\" or \"DEMURA\"", "Flash"); return; }
                    if (mvars.c12aflashitem == 0)
                    {
                        svs = cmbFlashSel.Text.Trim().Substring(2, cmbFlashSel.Text.Trim().Length - 2) + ",FLASHREAD,";
                        //mvars.lstget.Items.Add(svs + " ....");
                        Form1.lstget1.Items.Add(svs + " ....");
                        mp.cFLASHREAD_C12ACB();
                    }
                    else if (mvars.c12aflashitem == 1)
                    {
                        svs = "C12A XBoard " + cmbhPB.Text + " FlashWrite,";
                        //mvars.lstget.Items.Add(svs + " ....");
                        Form1.lstget1.Items.Add(svs + " ....");
                        //mp.cFLASHREAD_C12AXB();
                    }
                    if (mvars.errCode == "0")
                    {
                        cmb_FlashSel.Text = cmb_FlashSel.Items[mvars.flashselQ].ToString();
                        //mvars.lstget.Items.Add(svs + "DONE,1");
                        Form1.lstget1.Items.Add(svs + "DONE,1");
                    }
                    else 
                    {
                        //mvars.lstget.Items.Add(svs + "ERROR,1,ErrCode," + mvars.errCode); 
                        Form1.lstget1.Items.Add(svs + "ERROR,1,ErrCode," + mvars.errCode);
                    }
                }
                
            }
            //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
            Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
        }

        private void btn_FLASHWRITE_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string value = "";
                string prompt = " 強制寫入 ?";
                if (MultiLanguage.DefaultLanguage == "en-US") prompt = " Force Write ?";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") prompt = " 强制写入 ?";
                else if (MultiLanguage.DefaultLanguage == "ja-JP") prompt = " 強制書き込み ?";
                if (mp.InputBox(mvars.strUInameMe, prompt, ref value, 0, 0, 200, 50, 3, "") == DialogResult.OK)
                {
                    btn_FLASHWRITE.BackColor = Color.FromArgb(255, 0, 200);
                    btnFLASHWRITE.Text = prompt.Trim().Substring(0, prompt.Trim().Length - 2);
                }
            }
        }

        private void btn_FLASHWRITE_Click(object sender, EventArgs e)
        {
            if (txtFlashFileName.Text.Trim().ToUpper().IndexOf("*.BIN") == -1 && txtFlashFileName.Text.Length > 0)
            {
                txtFlashFileNameFull.Text = openFileDialog1.FileName;
                string[] Svstr = txtFlashFileNameFull.Text.Split('\\');
                if (Svstr.Length == 0) { MessageBox.Show("Bin file length = " + Svstr.Length, "INXPID v" + mvars.UImajor); return; }

                if (mvars.deviceID.Substring(0, 2) == "05")
                {
                    if (cmbFlashSel.Text.ToUpper().IndexOf("FPGA",0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("DEMURA", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("XB_", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("CB_", 0) != -1)
                    {
                        if (mvars.ucTmp.Length % (32 * 1024) != 0)
                        {
                            uint quotient = (uint)mvars.ucTmp.Length / (32 * 1024);
                            Array.Resize(ref mvars.ucTmp, (int)(quotient + 1) * 32 * 1024);
                        }
                        if (cmbFlashSel.Text.ToUpper().IndexOf("CB_", 0) != -1)
                        {
                            Byte[] Barcode = new byte[32];
                            mp.Copy(mvars.ucTmp, 0x0003FF5D, Barcode, 0, Barcode.Length);
                            lbl_barcode.Text = cmbFlashSel.Text.Trim() + " Barcode：" + System.Text.Encoding.ASCII.GetString(Barcode);
                        }
                        //mvars.lstget.Items.Add(cmbFlashSel.Text + " Flash Write");
                        lbl_fileofflashread.Text = @"...\Parameter\*Read.bin";

                        if ((btnFLASHWRITE.BackColor == Color.FromArgb(128, 255, 128) && 
                            txtFlashFileName.Text.IndexOf(cmb_FlashSel.Text.Trim(), 0) != -1) ||
                            btnFLASHWRITE.BackColor == Color.FromArgb(255, 0, 200))
                            mp.cFLASHWRITE_pCB(cmbFlashSel.Text, lst_get1);
                        //else if (btnFLASHWRITE.BackColor == Color.FromArgb(128, 255, 128) &&
                        //    txtFlashFileName.Text.ToLower().IndexOf(cmb_FlashSel.Text.Trim().ToLower().Substring(4, 2), 0) != -1 &&
                        //    cmb_FlashSel.Text.IndexOf("FPGA") != -1)
                        //    mp.cFLASHWRITE_pCB(cmbFlashSel.Text, lst_get1);
                        else
                        {
                            if (cmbFlashSel.Text.IndexOf("FPGA",0) != -1)
                            {
                                if (btnFLASHWRITE.BackColor == Color.FromArgb(128, 255, 128) &&
                                txtFlashFileName.Text.ToLower().IndexOf(cmb_FlashSel.Text.Trim().ToLower().Substring(4,2), 0) == -1)
                                {
                                    string value = "";
                                    string prompt = " 檔名與燒錄項目不符";
                                    if (MultiLanguage.DefaultLanguage == "en-US") prompt = " Please check Filename and Flashitem";
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN") prompt = " 档名与烧录项目不符";
                                    else if (MultiLanguage.DefaultLanguage == "ja-JP") prompt = " Please check Filename and Flashitem";
                                    mp.InputBox(mvars.strUInameMe, prompt, ref value, 0, 0, 200, 50, 1, "");
                                }
                                else if (btnFLASHWRITE.BackColor == Color.FromArgb(128, 255, 128) &&
                                        txtFlashFileName.Text.ToLower().IndexOf(cmb_FlashSel.Text.Trim().ToLower().Substring(4, 2), 0) != -1)
                                    mp.cFLASHWRITE_pCB(cmbFlashSel.Text, lst_get1);
                            }
                            else
                            {
                                if (btnFLASHWRITE.BackColor == Color.FromArgb(128, 255, 128) &&
                                txtFlashFileName.Text.IndexOf(cmb_FlashSel.Text.Trim(), 0) == -1)
                                {
                                    string value = "";
                                    string prompt = " 檔名與燒錄項目不符";
                                    if (MultiLanguage.DefaultLanguage == "en-US") prompt = " Please check Filename and Flashitem";
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN") prompt = " 档名与烧录项目不符";
                                    else if (MultiLanguage.DefaultLanguage == "ja-JP") prompt = " Please check Filename and Flashitem";
                                    mp.InputBox(mvars.strUInameMe, prompt, ref value, 0, 0, 200, 50, 1, "");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (mvars.c12aflashitem == 0)
                    {
                        if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                        {
                            byte[] Tmp = new byte[8 * 1024 * 1024];
                            Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                            Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                            Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                            string sNewFileNamle;
                            sNewFileNamle = txtFlashFileNameFull.Text.Replace(".bin", "_8MBforReadCompare.bin");
                            if (File.Exists(sNewFileNamle)) { File.Delete(sNewFileNamle); }
                            File.WriteAllBytes(sNewFileNamle, mvars.ucTmp);
                        }
                        else
                        {
                            MessageBox.Show("CB Flash bin file size error，please check again", "INXPID v" + mvars.UImajor);
                            return;
                        }
                    }
                    else if (mvars.c12aflashitem == 1)
                    {
                        if (mvars.ucTmp.Length <= (1 * 1024 * 1024))
                        {
                            string sNewFileNamle;
                            byte[] Tmp = new byte[1 * 1024 * 1024];
                            Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                            Array.Resize(ref mvars.ucTmp, 1 * 1024 * 1024);
                            Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                            sNewFileNamle = txtFlashFileNameFull.Text.Replace(".bin", "_1MBforReadCompare.bin");
                            if (File.Exists(sNewFileNamle)) { File.Delete(sNewFileNamle); }
                            File.WriteAllBytes(sNewFileNamle, mvars.ucTmp);
                        }
                        else
                        {
                            MessageBox.Show("XB Flash bin file size error，please check again", "INXPID v" + mvars.UImajor);
                            return;
                        }
                    }

                    if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                    Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;

                    if (mvars.c12aflashitem == 0 && mvars.flashselQ == 0) { MessageBox.Show("FlashType @ OPEN，Please Select \"FPGA\" or \"DEMURA\"", "Flash"); return; }
                    string svs = "";
                    if (mvars.c12aflashitem == 0)
                    {
                        svs = cmbFlashSel.Text.Trim().Substring(2, cmbFlashSel.Text.Trim().Length - 2) + " FlashWrite,";
                        //mvars.lstget.Items.Add(svs + " ....");
                        mp.cFLASHWRITE_C12ACB();
                    }
                    else if (mvars.c12aflashitem == 1)
                    {
                        svs = "C12A XBoard " + cmbhPB.Text + " FlashWrite,";
                        //mvars.lstget.Items.Add(svs + " ....");
                        ////mp.cFLASHWRITE_C12AXB(false);
                    }
                    if (mvars.errCode == "0")
                    {
                        cmb_FlashSel.Text = cmb_FlashSel.Items[mvars.flashselQ].ToString();
                        //mvars.lstget.Items.Add(svs + "DONE,1");
                    }
                    else 
                    { 
                        //mvars.lstget.Items.Add(svs + "ERROR,1,ErrCode," + mvars.errCode); 
                    }
                    //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                }
            }
        }

        private void btn_GetBin_Click(object sender, EventArgs e) { mp.GetBin(txt_FlashFileNameFull.Text); }
        private void txt_FlashFileName_TextChanged(object sender, EventArgs e) { }
        private void txt_FlashFileName_DoubleClick(object sender, EventArgs e)
        {
            if (cmbFlashSel.SelectedIndex == 0)
            {
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    lst_get1.Items.Add("  >> Please select Flash-type item first");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    lst_get1.Items.Add("  >> 請先選擇閃存種類");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    lst_get1.Items.Add("  >> 请先选择闪存种类");
                }
                lst_get1.TopIndex = lst_get1.Items.Count - 1;
            }
            else
            {
                using (openFileDialog1 = new OpenFileDialog())
                {
                    openFileDialog1.Title = "開啟 " + cmbFlashSel.Text + " 檔";
                    if (btnFLASHWRITE.BackColor == Color.FromArgb(255, 0, 200)) openFileDialog1.Filter = "Bin files (*.*)|*.bin";
                    else
                    {
                        if (mvars.flashselQ == 1) openFileDialog1.Filter = "Bin files (*.*)|*_A.bin|Bin files (*.*)|*_a.bin";
                        else if (mvars.flashselQ == 2) openFileDialog1.Filter = "Bin files (*.*)|*_B.bin|Bin files (*.*)|*_b.bin";
                        else if (mvars.flashselQ == 3) openFileDialog1.Filter = "Bin files (*.*)|*CB_1*.bin";
                        else if (mvars.flashselQ == 4) openFileDialog1.Filter = "Bin files (*.*)|*CB_2*.bin";
                        else if (mvars.flashselQ == 5) openFileDialog1.Filter = "Bin files (*.*)|*CB_3*.bin";
                        else if (mvars.flashselQ == 6) openFileDialog1.Filter = "Bin files (*.*)|*CB_4*.bin";
                        else if (mvars.flashselQ == 7) openFileDialog1.Filter = "Bin files (*.*)|*XB_1*.bin";
                        else if (mvars.flashselQ == 8) openFileDialog1.Filter = "Bin files (*.*)|*XB_2*.bin";
                        else if (mvars.flashselQ == 9) openFileDialog1.Filter = "Bin files (*.*)|*XB_3*.bin";
                        else if (mvars.flashselQ == 10) openFileDialog1.Filter = "Bin files (*.*)|*XB_4*.bin";
                        else if (mvars.flashselQ == 11) openFileDialog1.Filter = "Bin files (*.*)|*XB_5*.bin";
                        else if (mvars.flashselQ == 12) openFileDialog1.Filter = "Bin files (*.*)|*XB_6*.bin";
                        else if (mvars.flashselQ == 13) openFileDialog1.Filter = "Bin files (*.*)|*XB_7*.bin";
                        else if (mvars.flashselQ == 14) openFileDialog1.Filter = "Bin files (*.*)|*XB_8*.bin";

                    }
                    openFileDialog1.RestoreDirectory = true;
                    string[] Svstr;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        txt_FlashFileNameFull.Text = openFileDialog1.FileName;
                        Svstr = txt_FlashFileNameFull.Text.Split('\\');
                        if (Svstr.Length > 0)
                        {
                            txt_FlashFileName.Text = Svstr[Svstr.Length - 1];
                            callOnClick(btn_GetBin);
                            if (cmbFlashSel.Text.ToUpper().IndexOf("CB_", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("XB_", 0) != -1) { mp.BinFilter_00_FF(ref mvars.ucTmp); }
                            if (mvars.deviceID.Substring(0, 2) == "05")
                            {
                                if (mvars.ucTmp.Length % (32 * 1024) != 0)
                                {
                                    uint quotient = (uint)mvars.ucTmp.Length / (32 * 1024);
                                    Array.Resize(ref mvars.ucTmp, (int)(quotient + 1) * 32 * 1024);
                                }
                            }
                            int checksum = (int)mp.CalCheckSum(mvars.ucTmp, 0, mvars.ucTmp.Length);
                            lbl_BinChecksum.Text = mvars.ucTmp.Length + " bytes   checksum：0x" + checksum.ToString("X4");
                        }
                        else
                        {
                            txt_FlashFileName.Text = " *.bin (double click here to select bin file)";
                            lbl_BinChecksum.Text = "bytes";
                        }
                    }
                }
            }
        }



        private void uc_Flash_Load(object sender, EventArgs e)
        {
            //mp.h55formLoad(cmbhPB, cmbdeviceid);
            lst_get1.Size = new Size(731, 180);
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (mvars.actFunc == "Flash")
                {
                    //grp_hex.Enabled = false;
                    //List<string> svf = new List<string> { "Default", "FPGA_A", "FPGA_B", "DEMURA_1", "DEMURA_2", "DEMURA_3", "DEMURA_4", "FPGA_A", "FPGA_A", "FPGA_A" };
                    string[] svfUS = new string[]
                            { "Default", "FPGA_A", "FPGA_B", "CB_1", "CB_2", "CB_3", "CB_4", "XB_1", "XB_2", "XB_3", "XB_4", "XB_5", "XB_6", "XB_7", "XB_8" };
                    string[] svfCHT = new string[]
                            { "閃存種類", "FPGA_A", "FPGA_B", "CB_1", "CB_2", "CB_3", "CB_4", "XB_1", "XB_2", "XB_3", "XB_4", "XB_5", "XB_6", "XB_7", "XB_8" };
                    string[] svfCN = new string[]
                            { "闪存种类", "FPGA_A", "FPGA_B", "CB_1", "CB_2", "CB_3", "CB_4", "XB_1", "XB_2", "XB_3", "XB_4", "XB_5", "XB_6", "XB_7", "XB_8" };
                    string[] svfJP = new string[]
                            { "Default", "FPGA_A", "FPGA_B", "CB_1", "CB_2", "CB_3", "CB_4", "XB_1", "XB_2", "XB_3", "XB_4", "XB_5", "XB_6", "XB_7", "XB_8" };
                    cmbFlashSel.Items.Clear();
                    mvars.strFLASHtype = new string[svfUS.Length];
                    Array.Copy(svfUS, mvars.strFLASHtype, svfUS.Length);
                    if (MultiLanguage.DefaultLanguage == "en-US") cmbFlashSel.Items.AddRange(svfUS);
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") cmbFlashSel.Items.AddRange(svfCHT);
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") cmbFlashSel.Items.AddRange(svfCN);
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") cmbFlashSel.Items.AddRange(svfJP);
                }
                else if (mvars.actFunc == "Hex")
                {
                    grp_flash.Enabled = false;

                }
            }
            cmbdeviceid.SelectedIndex = Form1.cmbdeviceid.SelectedIndex;
            cmbFlashSel.SelectedIndex = 0;
            //mvars.actFunc = "Flash"; //在Form1的操作項中決定 Flash或是Hex
            mvars.FormShow[14] = true;
        }


        private void cmb_deviceID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.deviceID == "0300")
            {
                if (mvars.verMCU.Substring(0, 4) == "OBB-" || mvars.verMCU.Substring(0, 4) == "ABB-" || mvars.verMCU.Substring(0, 4) == "BBB-" ||
                         mvars.verMCU.Substring(0, 4) == "OCB-" || mvars.verMCU.Substring(0, 4) == "ACB-" || mvars.verMCU.Substring(0, 4) == "BCB-")
                {
                    if (cmbdeviceid.Text.Trim().ToUpper().IndexOf("ALL", 0) > -1) 
                    { 
                        mvars.deviceID = "0310"; 
                    }
                    else 
                    { 
                        mvars.deviceID = cmbdeviceid.Text.Trim().Replace("XB", "03"); 

                    }
                    Form1.tslbldeviceID.Text = mvars.deviceID;

                    //mvars.iPBaddr = Convert.ToByte(cmb_hPB.Text.Trim());
                    //mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1);
                    //mvars.deviceID = "03" + string.Format("{0:00}", (byte)((mvars.iPBaddr - 1) / 2 + 1));
                    //if (cmbCM603.Text.Trim() == "R") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1); }
                    //else if (cmbCM603.Text.Trim() == "G") { mvars.cm603WRaddr = 214; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                    //else if (cmbCM603.Text.Trim() == "B") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                }
                else
                {
                    MessageBox.Show("H5512A(\"OBB/ABB/ACB/OCB/BBB/BCB\" MCU ver check error (read: " + mvars.verMCU + ")" + "\r\n" + "\r\n" +
                                     "Please check the Hardware", "H5512A");

                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "05")
                Form1.cmbdeviceid.SelectedIndex = cmb_deviceID.SelectedIndex;
        }

        private void btn_spiFlashWR_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_jedecidread_Click(object sender, EventArgs e)
        {
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            Button btn = (Button)sender;

            btn.Enabled = false;
            mvars.lblCmd = "JEDECIDREAD";
            mp.mhFUNCSTATUS();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) mvars.errCode = "-1";
            btn.Enabled = true;
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
            //string[] svss = mvars.strReceive.Split(',');
            //Form1.lstget1.Items.Add("  ↑，" + svss[7] + "s");
            //Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
        }

        private void btn_FLASHINFO_Click(object sender, EventArgs e)
        {
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            Button btn = (Button)sender;

            btn.Enabled = false;

            mvars.lblCmd = "FLASH_TYPE";
            mp.mhFLASHTYPE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
            mvars.lblCmd = "READ_JEDECID";
            mp.mSPI_READJEDECID();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
            else
            {
                lbl_jedecid.Text = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                //string str = lbl_jedecid.Text;
                //if (str.Substring((str.Length - 2), 2) == "13")
                //    SPI_512KB_rdbtn.Checked = true;
                //if (str.Substring((str.Length - 2), 2) == "14")
                //    SPI_1MB_rdbtn.Checked = true;
                //if (str.Substring((str.Length - 2), 2) == "15")
                //    SPI_2MB_rdbtn.Checked = true;
                //if (str.Substring(0, 2) == "EF")
                //    SPI_Vender_cbx.Text = "WINBOND";
                //if (str.Substring(0, 2) == "C2")
                //    SPI_Vender_cbx.Text = "MXIC";
            }

            btn.Enabled = true;
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
        }

        private void cmb_FlashSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvars.flashselQ = (byte)cmbFlashSel.SelectedIndex;
            if (mvars.deviceID.Substring(0,2) != "05")
            {
                if (mvars.c12aflashitem == 0)   //CB 8MBytes
                {
                    //mvars.flashselQ = (byte)cmbFlash.SelectedIndex;
                    cmbhPB.Visible = true; if (mvars.flashselQ == 1) { cmbhPB.Visible = false; }
                    if (mvars.flashselQ == 0) { lbl_fileofflashread.Text = @"...\Parameter\*Read.bin"; }
                    else { lbl_fileofflashread.Text = @"...\Parameter\" + mvars.strFLASHtype[mvars.flashselQ] + "FlashRead.bin"; }
                }
                else
                {

                }
            }
            else
            {
                if (mvars.flashselQ <= 2)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") grp_flash.Text = "Flash ram 8Mbytes";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") grp_flash.Text = "閃存記憶體 8Mbytes";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") grp_flash.Text = "闪存记忆体 8Mbytes";
                    Array.Resize(ref mp.FlashRd_Arr, 8 * 1024 * 1024);
                }
                else if (mvars.flashselQ > 2 && mvars.flashselQ <= 6)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") grp_flash.Text = "Flash ram 32Mbytes";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") grp_flash.Text = "閃存記憶體 32Mbytes";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") grp_flash.Text = "闪存记忆体 32Mbytes";
                    Array.Resize(ref mp.FlashRd_Arr, 32 * 1024 * 1024);
                }
                else if (mvars.flashselQ > 6)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") grp_flash.Text = "Flash ram 8Mbytes";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") grp_flash.Text = "閃存記憶體 8Mbytes";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") grp_flash.Text = "闪存记忆体 8Mbytes";
                    Array.Resize(ref mp.FlashRd_Arr, 8 * 1024 * 1024);
                }
            }
        }


        private void chk_NVBC_CheckedChanged(object sender, EventArgs e)
        {
            cmb_deviceID.Enabled = !chk_NVBC.Checked;
            Form1.chkBC.Checked = chk_NVBC.Checked;
        }

        private void OCP_Auto_btn_Click(object sender, EventArgs e)
        {
            //mp.cAUTOOCP(null, lst_get1, 4, 6, 19, false, "No1");
            mp.cAUTOOCP(MCU_OCP_Rd_dGV, lst_get1, 4, 6, 19, false, "No1");
        }

        private void OCP_Disable_btn_Click(object sender, EventArgs e)
        {
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            Button btn = (Button)sender;

            btn.Enabled = false;

            Byte OCP_En = 0;
            mvars.lblCmd = "OCP_Enable_" + OCP_En;
            mp.mOCP_EnDis_Ctrl(OCP_En);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                lst_get1.Items.Add(mvars.lblCmd + " 发生异常");
            else
                lst_get1.Items.Add(mvars.lblCmd + " DONE");
            mvars.lblCmd = "MCU_VERSION";
            mp.mhVersion();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                lst_get1.Items.Add(mvars.lblCmd + " 发生异常");
            else
                lst_get1.Items.Add("MCUver " + mvars.verMCU);

            btn.Enabled = true;
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
        }

        private void OCP_Enable_btn_Click(object sender, EventArgs e)
        {
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            Button btn = (Button)sender;

            btn.Enabled = false;

            Byte OCP_En = 0;
            OCP_En |= 0x01;
            OCP_En |= 0x02;
            mvars.lblCmd = "OCP_Enable_" + OCP_En;
            mp.mOCP_EnDis_Ctrl(OCP_En);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                lst_get1.Items.Add(mvars.lblCmd + " 发生异常");
            else
                lst_get1.Items.Add(mvars.lblCmd + " DONE");
            mvars.lblCmd = "MCU_VERSION";
            mp.mhVersion();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                lst_get1.Items.Add(mvars.lblCmd + " 发生异常");
            else
                lst_get1.Items.Add("MCUver " + mvars.verMCU);

            btn.Enabled = true;
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
        }
    }
}
