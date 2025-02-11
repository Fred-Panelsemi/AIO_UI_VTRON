using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;

namespace AIO
{
    public partial class uc_coding : UserControl
    {
        public static CheckBox chknIsReadback = null;
        public static CheckBox chkNVBC = null;
        public static ListBox lstother = null;
        public static ListBox lstget1 = null;

        #region boxset物件

        public static TabControl tabControlScreenInfoList = null;

        //public static int[] nX = null;  //給NovaStar的 BOX SET 使用 (排序有幾欄)
        //public static int[] nY = null;  //給NovaStar的 BOX SET 使用 (由小到大排序, 再判別有幾列)
        //public static Label lblCoorHead = new Label();      //座標標題
        //public static Label lblBoxRowsHead = new Label();   //顯示屏列分割數標題
        //public static Label lblBoxWhead = new Label();      //箱體水平畫素標題
        //public static Label lblBoxHhead = new Label();      //箱體垂直畫素標題
        //public static Button btnBoxSend = new Button();     //BoxSet
        //public static Button btnBoxSave = new Button();     //BoxSave
        //public static Button btnBoxLoad = new Button();     //載入設定檔案
        //public static int ciR = 20 / 2;  //圓半徑
        //public static int cabs = 1;
        //public static int cabIndex = 0;


        public static Button dgvBtn = null;
        public static DataGridView dgvbox = new DataGridView();
        #endregion boxset物件

        byte[] gFlashRdPacketArr = new byte[32768];
        public static ushort flashreadsize = 2048;
        public static ComboBox FPGAFlashModecbx;
        public static bool svforceAll = false;

        private TextBox txtFlashFileName = null;
        private TextBox txtFlashFileNameFull = null;
        public static ComboBox cmbFlashSel = null;


        public uc_coding()
        {
            InitializeComponent();
            lstget1 = lst_get1;
            chknIsReadback = chk_nIsReadback;
            chkNVBC = chk_NVBC;
            dgvBtn = btn_dgv;
            dgvbox = dgv_box;
            dgvbox.ReadOnly = true;
            dgvbox.AllowUserToAddRows = false;
            dgvbox.AllowUserToResizeRows = false;
            dgvbox.AllowUserToResizeColumns = false;
            dgvbox.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvbox.ShowCellToolTips = false;
            dgvbox.ScrollBars = ScrollBars.Both;
            FPGAFlashModecbx = FPGA_FlashMode_cbx;

            txtFlashFileName = txt_FlashFileName;
            txtFlashFileNameFull = txt_FlashFileNameFull;
            cmbFlashSel = cmb_FlashSel;
        }






        private void btn_browse_Click(object sender, EventArgs e)
        {
            if (MCU_00_rdbtn.Checked == true)
                Array.Clear(mvars.gMcuBinFile, 0, mvars.gMcuBinFile.Length);
            else
                for (uint i = 0; i < mvars.gMcuBinFile.Length; i++)
                    mvars.gMcuBinFile[i] = 0xFF;


            using (openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Title = "開啟 hex 檔";
                openFileDialog1.Filter = "hex files (*.hex)|*.hex";//|All files (*.*)|*.*"; 去掉;//就可以跟All files結合
                openFileDialog1.RestoreDirectory = true;
                string sverr = "0";
                Byte[] tmp;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (mvars.deviceID.Substring(0, 2) == "02")
                    {
                        #region C12A/B
                        string[] Svstr;
                        txt_nMCUFileNameFull.Text = openFileDialog1.FileName;
                        Svstr = txt_nMCUFileNameFull.Text.Split('\\');
                        if (Svstr.Length > 0) { txt_nMCUFileName.Text = Svstr[Svstr.Length - 1]; }
                        else { txt_nMCUFileName.Text = " *.hex (double click here to select hex file)"; }


                        string sTextLine;
                        Byte[] ucNvmUserPage = new byte[mvars.NVM_USER_PAGE];
                        bool bBreak = false;
                        bool bHexParseSuccess = false;
                        //bReturnFlag = false;

                        System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
                        UInt32 lFlashAddr, lExtSegAddr = 0, lTmp = 0;
                        while ((sTextLine = file.ReadLine()) != null)
                        {
                            UInt32 lLineIndex = 0;
                            if (sTextLine.Substring(0, 1) == ":")
                            {
                                UInt32 lRunningChecksum = 0;
                                byte ucRecordLength = Convert.ToByte(sTextLine.Substring(1, 2), 16);
                                lLineIndex = lLineIndex + 2;
                                lRunningChecksum = lRunningChecksum + ucRecordLength;

                                byte ucAddrTmp = Convert.ToByte(sTextLine.Substring(3, 2), 16);
                                lRunningChecksum = lRunningChecksum + ucAddrTmp;
                                UInt32 lAddrOffset = ucAddrTmp;
                                lAddrOffset = lAddrOffset * 256;

                                ucAddrTmp = Convert.ToByte(sTextLine.Substring(5, 2), 16);
                                lRunningChecksum = lRunningChecksum + ucAddrTmp;
                                lAddrOffset = lAddrOffset + ucAddrTmp;

                                byte ucRecType = Convert.ToByte(sTextLine.Substring(7, 2), 16);
                                lRunningChecksum = lRunningChecksum + ucRecType;

                                lLineIndex = 10;
                                byte iHexDataIndex = 0, ucToHexData = 0, ucLineChecksum = 0, ucLineLen = 0;
                                switch (ucRecType)  //Select Case ucRecType
                                {
                                    case 0x00:
                                        for (iHexDataIndex = 0; iHexDataIndex < ucRecordLength; iHexDataIndex++)
                                        {
                                            ucToHexData = Convert.ToByte(sTextLine.Substring((int)lLineIndex - 1, 2), 16);
                                            lLineIndex = lLineIndex + 2;
                                            lFlashAddr = lAddrOffset + iHexDataIndex + lExtSegAddr;
                                            if (lFlashAddr >= mvars.NVM_USER_PAGE_START && lFlashAddr <= mvars.NVM_USER_PAGE_END)
                                            {
                                                ucNvmUserPage[lFlashAddr - mvars.NVM_USER_PAGE_START] = ucToHexData;
                                            }
                                            else if (lFlashAddr >= mvars.gMcuBinFile.Length)
                                            {
                                                bBreak = true;
                                                //OP_Msg1(OP_Msg_lbx, "Flash Address greater than Bin Size");
                                                sverr = "-1";
                                                break;
                                            }
                                            else
                                            {
                                                mvars.gMcuBinFile[lFlashAddr] = ucToHexData;
                                                mvars.gbBlockWrite[lFlashAddr / mvars.MCU_BLOCK_SIZE] = true;
                                            }
                                            lRunningChecksum = lRunningChecksum + ucToHexData;
                                        }
                                        ucLineLen = (byte)sTextLine.Length;
                                        ucLineChecksum = Convert.ToByte(sTextLine.Substring(ucLineLen - 2, 2), 16);
                                        lTmp = (ucLineChecksum + lRunningChecksum) % 256;
                                        if (lTmp != 0)
                                        {
                                            bBreak = true;
                                        }
                                        break;
                                    case 0x01:  //End-of-File Record
                                        bHexParseSuccess = true;
                                        break;
                                    case 0x04:  //
                                        lExtSegAddr = (UInt32)(Convert.ToUInt16(sTextLine.Substring((int)lLineIndex - 1, 4), 16) * 65536);
                                        break;
                                    default:    //Unknown record type
                                        bBreak = true;
                                        break;
                                }
                                if (bBreak == true)
                                {
                                    break;
                                }
                            }   //if (sTextLine.Substring(1, 1) == ":")
                        }
                        file.Close();

                        //Check Flag
                        if (bHexParseSuccess == false || bBreak == true)
                        {
                            if (sverr == "-1") { lst_get1.Items.Add("Error Hex File，Flash Address greater than Bin Size"); }
                            else { lst_get1.Items.Add("Error Hex File"); }
                            btn_nMCUBOOT.Enabled = false;
                        }
                        else
                        {
                            string sFilePath = txt_nMCUFileNameFull.Text.Replace(".hex", "(hex2bin).bin");
                            File.WriteAllBytes(sFilePath, mvars.gMcuBinFile);
                            File.WriteAllBytes("C:\\Users\\" + Environment.UserName + "\\Documents\\NVM_User_Page.bin", ucNvmUserPage);
                            lst_get1.Items.Add("Hex 2 Bin OK");
                            lst_get1.Items.Add(sFilePath);

                            //App Hex MCU Infomation
                            tmp = new byte[0x100];
                            Byte[] isString = new byte[6];

                            try
                            {
                                mp.Copy(mvars.gMcuBinFile, 0x7FF00, tmp, 0, 0x100);
                                tmp = Array.FindAll(tmp, val => val != 191).ToArray();
                                for (uint i = 0; i < tmp.Length; i++)
                                {
                                    if (tmp[i] != 0 && tmp[i + 1] == 0)
                                    {
                                        tmp[i + 1] = (byte)'\t';
                                        i++;
                                    }
                                }
                                tmp = Array.FindAll(tmp, val => val != 0).ToArray();

                                string MCU_Ver = System.Text.Encoding.ASCII.GetString(tmp);
                                MCU_Ver = MCU_Ver.Replace("?", "");
                                string[] split = MCU_Ver.Split(new Char[] { '\t' });

                                textBox290.Text = split[0];
                                textBox292.Text = split[1];
                                textBox294.Text = split[2];
                                textBox293.Text = split[3];
                                textBox291.Text = split[4];
                                textBox289.Text = split[5];
                                Array.Resize(ref tmp, 16);
                                mp.Copy(mvars.gMcuBinFile, 0x7FFE0, tmp, 0, 16);
                                textBox2.Text = System.Text.Encoding.ASCII.GetString(tmp);

                                //lstget1.Items.Add("Annotation ：" + split[5]);
                                //lstget1.Items.Add("Code Mode ：" + split[4]);
                                //lstget1.Items.Add("Project Name ：" + split[3]);
                                //lstget1.Items.Add("Time ：" + split[2]);
                                //lstget1.Items.Add("Date ：" + split[1]);
                                //lstget1.Items.Add("MCU Version ：" + split[0]);
                            }
                            catch (Exception ex) { lstget1.Items.Add(ex.Message); }

                            //Boot Hex MCU Infomation
                            try
                            {
                                Array.Resize(ref tmp, 0x100);
                                mp.Copy(mvars.gMcuBinFile, 0xFF00, tmp, 0, 0x100);
                                tmp = Array.FindAll(tmp, val => val != 191).ToArray();
                                for (uint i = 0; i < (tmp.Length - 1); i++)
                                {
                                    try
                                    {
                                        if (tmp[i] != 0 && tmp[i + 1] == 0)
                                        {
                                            tmp[i + 1] = (byte)'\t';
                                            i++;
                                        }
                                    }
                                    catch (Exception ex) { lstget1.Items.Add(ex.Message); }
                                }
                                tmp = Array.FindAll(tmp, val => val != 0).ToArray();

                                string MCU_Ver = System.Text.Encoding.ASCII.GetString(tmp);
                                MCU_Ver = MCU_Ver.Replace("?", "");
                                string[] splitBoot = MCU_Ver.Split(new Char[] { '\t' });

                                textBox220.Text = splitBoot[0];
                                textBox296.Text = splitBoot[1];
                                textBox298.Text = splitBoot[2];
                                textBox297.Text = splitBoot[3];
                                textBox295.Text = splitBoot[4];
                                textBox213.Text = splitBoot[5];

                                Array.Resize(ref tmp, 16);
                                mp.Copy(mvars.gMcuBinFile, 0xFFF0, tmp, 0, 16);
                                textBox1.Text = System.Text.Encoding.ASCII.GetString(tmp);

                                //lstget1.Items.Add("Annotation ：" + splitBoot[5]);
                                //lstget1.Items.Add("Code Mode ：" + splitBoot[4]);
                                //lstget1.Items.Add("Project Name ：" + splitBoot[3]);
                                //lstget1.Items.Add("Time ：" + splitBoot[2]);
                                //lstget1.Items.Add("Date ：" + splitBoot[1]);
                                //lstget1.Items.Add("MCU Version ：" + splitBoot[0]);
                            }
                            catch (Exception ex) { lstget1.Items.Add(ex.Message); }


                            //Calculate Checksum
                            //UInt16 checksum = mp.CalChecksum(mvars.gMcuBinFile, 0, (UInt16)mvars.gMcuBinFile.Length - 1);
                            //textBox299.Text = "Checksum：0x" + checksum.ToString("X4");
                            //UInt32 checksum32 = CalChecksum32(gMcuBinFile, 0, (UInt16)gMcuBinFile.Length - 1);
                            //textBox236.Text = "Checksum：0x" + checksum32.ToString("X8");



                            //Log
                            string[] sLines = new string[0];
                            for (UInt32 i = 0; i < mvars.gbBlockWrite.Length; i++)
                            {
                                if (mvars.gbBlockWrite[i] == true)
                                {
                                    System.Array.Resize(ref sLines, sLines.Length + 1);
                                    sLines[sLines.Length - 1] = Convert.ToUInt32(i * mvars.MCU_BLOCK_SIZE).ToString("X8") + ":" + mvars.gbBlockWrite[i];
                                }
                            }
                            btn_nMCUBOOT.Enabled = true;
                        }
                        #endregion C12A/B
                    }
                    else
                    {
                        #region 目前未設判斷式但是先提供給0500(Primary)使用
                        string[] Svstr;
                        txt_nMCUFileNameFull.Text = openFileDialog1.FileName;
                        Svstr = txt_nMCUFileNameFull.Text.Split('\\');
                        if (Svstr.Length > 0) { txt_nMCUFileName.Text = Svstr[Svstr.Length - 1]; }
                        else { txt_nMCUFileName.Text = " *.hex (double click here to select hex file)"; }


                        string sTextLine;
                        Byte[] ucNvmUserPage = new byte[mvars.NVM_USER_PAGE];
                        bool bBreak = false;
                        bool bHexParseSuccess = false;
                        //bReturnFlag = false;

                        System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
                        UInt32 lFlashAddr, lExtSegAddr = 0, lTmp = 0;
                        while ((sTextLine = file.ReadLine()) != null)
                        {
                            UInt32 lLineIndex = 0;
                            if (sTextLine.Substring(0, 1) == ":")
                            {
                                UInt32 lRunningChecksum = 0;
                                byte ucRecordLength = Convert.ToByte(sTextLine.Substring(1, 2), 16);
                                lLineIndex = lLineIndex + 2;
                                lRunningChecksum = lRunningChecksum + ucRecordLength;

                                byte ucAddrTmp = Convert.ToByte(sTextLine.Substring(3, 2), 16);
                                lRunningChecksum = lRunningChecksum + ucAddrTmp;
                                UInt32 lAddrOffset = ucAddrTmp;
                                lAddrOffset = lAddrOffset * 256;

                                ucAddrTmp = Convert.ToByte(sTextLine.Substring(5, 2), 16);
                                lRunningChecksum = lRunningChecksum + ucAddrTmp;
                                lAddrOffset = lAddrOffset + ucAddrTmp;

                                byte ucRecType = Convert.ToByte(sTextLine.Substring(7, 2), 16);
                                lRunningChecksum = lRunningChecksum + ucRecType;

                                lLineIndex = 10;
                                byte iHexDataIndex = 0, ucToHexData = 0, ucLineChecksum = 0, ucLineLen = 0;
                                switch (ucRecType)  //Select Case ucRecType
                                {
                                    case 0x00:
                                        for (iHexDataIndex = 0; iHexDataIndex < ucRecordLength; iHexDataIndex++)
                                        {
                                            ucToHexData = Convert.ToByte(sTextLine.Substring((int)lLineIndex - 1, 2), 16);
                                            lLineIndex = lLineIndex + 2;
                                            lFlashAddr = lAddrOffset + iHexDataIndex + lExtSegAddr;
                                            if (lFlashAddr >= mvars.NVM_USER_PAGE_START && lFlashAddr <= mvars.NVM_USER_PAGE_END)
                                            {
                                                ucNvmUserPage[lFlashAddr - mvars.NVM_USER_PAGE_START] = ucToHexData;
                                            }
                                            else if (lFlashAddr >= mvars.gMcuBinFile.Length)
                                            {
                                                bBreak = true;
                                                //OP_Msg1(OP_Msg_lbx, "Flash Address greater than Bin Size");
                                                sverr = "-1";
                                                break;
                                            }
                                            else
                                            {
                                                mvars.gMcuBinFile[lFlashAddr] = ucToHexData;
                                                mvars.gbBlockWrite[lFlashAddr / mvars.MCU_BLOCK_SIZE] = true;
                                            }
                                            lRunningChecksum = lRunningChecksum + ucToHexData;
                                        }
                                        ucLineLen = (byte)sTextLine.Length;
                                        ucLineChecksum = Convert.ToByte(sTextLine.Substring(ucLineLen - 2, 2), 16);
                                        lTmp = (ucLineChecksum + lRunningChecksum) % 256;
                                        if (lTmp != 0)
                                        {
                                            bBreak = true;
                                        }
                                        break;
                                    case 0x01:  //End-of-File Record
                                        bHexParseSuccess = true;
                                        break;
                                    case 0x04:  //
                                        lExtSegAddr = (UInt32)(Convert.ToUInt16(sTextLine.Substring((int)lLineIndex - 1, 4), 16) * 65536);
                                        break;
                                    default:    //Unknown record type
                                        bBreak = true;
                                        break;
                                }
                                if (bBreak == true)
                                {
                                    break;
                                }
                            }   //if (sTextLine.Substring(1, 1) == ":")
                        }
                        file.Close();

                        //Check Flag
                        if (bHexParseSuccess == false || bBreak == true)
                        {
                            if (sverr == "-1") { lst_get1.Items.Add("Error Hex File，Flash Address greater than Bin Size"); }
                            else { lst_get1.Items.Add("Error Hex File"); }
                            btn_nMCUBOOT.Enabled = false;
                        }
                        else
                        {
                            string sFilePath = txt_nMCUFileNameFull.Text.Replace(".hex", "(hex2bin).bin");
                            File.WriteAllBytes(sFilePath, mvars.gMcuBinFile);
                            File.WriteAllBytes("C:\\Users\\" + Environment.UserName + "\\Documents\\NVM_User_Page.bin", ucNvmUserPage);
                            lst_get1.Items.Add("Hex 2 Bin OK");
                            lst_get1.Items.Add(sFilePath);

                            //App Hex MCU Infomation
                            tmp = new byte[0x100];
                            Byte[] isString = new byte[6];

                            try
                            {
                                mp.Copy(mvars.gMcuBinFile, mvars.MCU_INFO_ADDR, tmp, 0, 0x100);
                                tmp = Array.FindAll(tmp, val => val != 191).ToArray();
                                for (uint i = 0; i < tmp.Length; i++)
                                {
                                    if (tmp[i] != 0 && tmp[i + 1] == 0)
                                    {
                                        tmp[i + 1] = (byte)'\t';
                                        i++;
                                    }
                                }
                                tmp = Array.FindAll(tmp, val => val != 0).ToArray();

                                string MCU_Ver = System.Text.Encoding.ASCII.GetString(tmp);
                                MCU_Ver = MCU_Ver.Replace("?", "");
                                string[] split = MCU_Ver.Split(new Char[] { '\t' });

                                textBox290.Text = split[0];
                                textBox292.Text = split[1];
                                textBox294.Text = split[2];
                                textBox293.Text = split[3];
                                textBox291.Text = split[4];
                                textBox289.Text = split[5];
                                Array.Resize(ref tmp, 16);
                                mp.Copy(mvars.gMcuBinFile, mvars.MCU_APP_VERSION_ADDR, tmp, 0, 16);
                                textBox2.Text = System.Text.Encoding.ASCII.GetString(tmp);

                                //lstget1.Items.Add("Annotation ：" + split[5]);
                                //lstget1.Items.Add("Code Mode ：" + split[4]);
                                //lstget1.Items.Add("Project Name ：" + split[3]);
                                //lstget1.Items.Add("Time ：" + split[2]);
                                //lstget1.Items.Add("Date ：" + split[1]);
                                //lstget1.Items.Add("MCU Version ：" + split[0]);
                            }
                            catch (Exception ex) { lstget1.Items.Add(ex.Message); }

                            //Boot Hex MCU Infomation
                            try
                            {
                                Array.Resize(ref tmp, 0x100);
                                mp.Copy(mvars.gMcuBinFile, mvars.MCU_BOOT_INFO_ADDR, tmp, 0, 0x100);
                                tmp = Array.FindAll(tmp, val => val != 191).ToArray();
                                for (uint i = 0; i < (tmp.Length - 1); i++)
                                {
                                    try
                                    {
                                        if (tmp[i] != 0 && tmp[i + 1] == 0)
                                        {
                                            tmp[i + 1] = (byte)'\t';
                                            i++;
                                        }
                                    }
                                    catch (Exception ex) { lstget1.Items.Add(ex.Message); }
                                }
                                tmp = Array.FindAll(tmp, val => val != 0).ToArray();

                                string MCU_Ver = System.Text.Encoding.ASCII.GetString(tmp);
                                MCU_Ver = MCU_Ver.Replace("?", "");
                                string[] splitBoot = MCU_Ver.Split(new Char[] { '\t' });

                                textBox220.Text = splitBoot[0];
                                textBox296.Text = splitBoot[1];
                                textBox298.Text = splitBoot[2];
                                textBox297.Text = splitBoot[3];
                                textBox295.Text = splitBoot[4];
                                textBox213.Text = splitBoot[5];

                                Array.Resize(ref tmp, 16);
                                mp.Copy(mvars.gMcuBinFile, mvars.MCU_BOOT_VERSION_ADDR, tmp, 0, 16);
                                textBox1.Text = System.Text.Encoding.ASCII.GetString(tmp);

                                //lstget1.Items.Add("Annotation ：" + splitBoot[5]);
                                //lstget1.Items.Add("Code Mode ：" + splitBoot[4]);
                                //lstget1.Items.Add("Project Name ：" + splitBoot[3]);
                                //lstget1.Items.Add("Time ：" + splitBoot[2]);
                                //lstget1.Items.Add("Date ：" + splitBoot[1]);
                                //lstget1.Items.Add("MCU Version ：" + splitBoot[0]);
                            }
                            catch (Exception ex) { lstget1.Items.Add(ex.Message); }


                            //Calculate Checksum
                            //UInt16 checksum = mp.CalChecksum(mvars.gMcuBinFile, 0, (UInt16)(mvars.gMcuBinFile.Length - 1));
                            //textBox299.Text = "Checksum：0x" + checksum.ToString("X4");
                            //UInt32 checksum32 = mp.CalChecksum32(mvars.gMcuBinFile, 0, (UInt16)(mvars.gMcuBinFile.Length - 1));
                            //textBox236.Text = "Checksum：0x" + checksum32.ToString("X8");



                            //Log
                            string[] sLines = new string[0];
                            for (UInt32 i = 0; i < mvars.gbBlockWrite.Length; i++)
                            {
                                if (mvars.gbBlockWrite[i] == true)
                                {
                                    System.Array.Resize(ref sLines, sLines.Length + 1);
                                    sLines[sLines.Length - 1] = Convert.ToUInt32(i * mvars.MCU_BLOCK_SIZE).ToString("X8") + ":" + mvars.gbBlockWrite[i];
                                }
                            }
                            btn_nMCUBOOT.Enabled = true;
                        }
                        #endregion 
                    }

                    tmp = new byte[16];
                    mp.Copy(mvars.gMcuBinFile, mvars.MCU_BOOT_VERSION_ADDR, tmp, 0, 16);
                    string svs = System.Text.Encoding.ASCII.GetString(tmp);
                    mvars.HexBootVer = Convert.ToUInt16(svs.Substring(svs.Length - 4, 4));
                    mp.Copy(mvars.gMcuBinFile, mvars.MCU_APP_VERSION_ADDR, tmp, 0, 16);
                    svs = System.Text.Encoding.ASCII.GetString(tmp);
                    mvars.HexAppVer = Convert.ToUInt16(svs.Substring(svs.Length - 4, 4));

                }
            }
        }
        private void btn_nMCUBOOT_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (mvars.demoMode == false)
            {
                //byte[] tmp = new byte[16];
                //mp.Copy(mvars.gMcuBinFile, mvars.MCU_BOOT_VERSION_ADDR, tmp, 0, 16);
                //string textBox1 = System.Text.Encoding.ASCII.GetString(tmp);
                //ushort HexBootVer = Convert.ToUInt16(textBox1.Substring(textBox1.Length - 4, 4));
                //mp.Copy(mvars.gMcuBinFile, 0x0007FFE0, tmp, 0, 16);
                //textBox1 = System.Text.Encoding.ASCII.GetString(tmp);
                //ushort HexAppVer = Convert.ToUInt16(textBox1.Substring(textBox1.Length - 4, 4));

                //ushort svundos;
                //bool[] svundo = null;
                //bool[] svundoB = null;
                //bool[] svundoA = null;
                //bool[] svunlock = null;
                //bool[] svUserbreak = null;
                //int svtodos;    // 在第一階段(判斷版本與上鎖)未發生異常準備燒錄(todo)的片數
                //int svactdos;   // 在第二階段(bootcode OK，appcpde OK)的片數

                typhwCard[] svhwCard = new typhwCard[1];
                Array.Clear(svhwCard, 0, 1);

                btn.Enabled = false;
                btn_nMCUR.Enabled = false;
                btn_browse.Enabled = false;
                btn_show.Enabled = true;
                btn_offshow.Enabled = true;
                mvars.Break = false;
                bool svnvboardcast = mvars.nvBoardcast;
                bool svisReadback = mvars.isReadBack;
                //short sverrc;
                //string txt44;
                //UInt32 PacketSize;
                //UInt32 Count;
                //int svlstc = 0;

                lstget1.Items.Clear();
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    lstget1.Items.Add("Processing ... please wait");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    lstget1.Items.Add("請稍後 .....");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    lstget1.Items.Add("请稍后 .....");
                }

                System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
                sw1.Reset();
                sw1.Start();

                mp.cFLASHWRITE_pMCU(1, lst_get1);

                mvars.nvBoardcast = svnvboardcast;
                mvars.isReadBack = svisReadback;
                btn.Enabled = true;
                btn_nMCUR.Enabled = true;
                btn_browse.Enabled = true;
                btn_show.Enabled = true;
                btn_offshow.Enabled = true;
                btn_nMCUBREAK.Visible = false; btn_nMCUBREAK.Enabled = false;
            }
            else
            {
                btn_nMCUBREAK.Visible = true;
                mp.doDelayms(2000);
                btn_nMCUBREAK.Visible = false;
            }
            mvars.flgForceUpdate = false;
            btn_nMCUBOOT.ForeColor = System.Drawing.Color.FromArgb(0, 192, 0);
        }





        private void uc_coding_Load(object sender, EventArgs e)
        {
            this.Width = 777;

            //grp_nova.Location = new Point(10, 0);
            cmb_deviceID.Items.Clear();
            for (int i = 0; i < Form1.cmbdeviceid.Items.Count; i++)
            {
                cmb_deviceID.Items.Add(Form1.cmbdeviceid.Items[i].ToString());
            }
            cmb_deviceID.Text = Form1.cmbdeviceid.Text;

            cmb_FlashSel.Items.Clear();
            if (mvars.deviceID.Substring(0,2)=="05" || mvars.deviceID.Substring(0,2)=="06")
            {
                cmb_FlashSel.Items.Add("Default");
                cmb_FlashSel.Items.Add("FPGA_A");
                cmb_FlashSel.Items.Add("FPGA_B");
                //如何讓TabPage隱藏
                // from http://tina-tripmemo.blogspot.com/2015/10/tabcontroltabpage.html
                tabpage_lb.Parent = null;
            }
            else
            {
                //如何讓TabPage顯示
                //from http://tina-tripmemo.blogspot.com/2015/10/tabcontroltabpage.html
                tabpage_lb.Parent = tabControl1;
            }
            cmb_FlashSel.SelectedIndex = 0;

            if (mvars.flgbootloader)
            {
                tabpage_bmp.Parent = null;
                tabpage_lb.Parent = null;
                groupBox35.Enabled = false;
                groupBox70.Enabled = false;

                //tabpage_dmr.Parent = null;
                btn_nDMRW.Visible = false;
                btn_nDMRdraw.Visible = false;
                btn_nDMRbmp.Visible= false;
                btn_browseDMR.Visible = false;
                txt_DMRFileName.Enabled = false;

                btn_BoxRESETn.Visible = false;
                label8.Visible = false;
                numUD_boxCols.Visible = false;
                label7.Visible = false;
                numUD_boxRows.Visible = false;
                dgvbox.Visible = false;
            }


            lstget1.SetBounds(422, 137, 277, 294);
            lstget1.BackColor = System.Drawing.Color.White;
            //txt_filepathfull.SetBounds(19, 412, 386, 21);

            dgvbox.SetBounds(20, 155+20, 390, 250);
            btn_nDMRBREAK.SetBounds(609, 10, 66, 23);

            btn_nMCUBREAK.Location = btn_nMCUBOOT.Location;

            label8.Location = new System.Drawing.Point(22, 134+20);
            numUD_boxCols.Location = new System.Drawing.Point(90, 131+20);
            label7.Location = new System.Drawing.Point(157, 134+20);
            numUD_boxRows.Location = new System.Drawing.Point(225, 131+20);
            btn_BoxRESETn.Location = new System.Drawing.Point(301, 128+20);
            Skeleton_nBox((int)numUD_boxCols.Value, (int)numUD_boxRows.Value, 0, 0);

            if (mvars.svnova) { SPI_ReadSize_cbx.SelectedIndex = 3; SPI_ReadSize_cbx.Enabled = false; }
            else { SPI_ReadSize_cbx.SelectedIndex = 3; SPI_ReadSize_cbx.Enabled = true; }

            lbl_sender.Visible = !chkNVBC.Checked;
            numericUpDown_sender.Visible = !chkNVBC.Checked;
            lbl_port.Visible = !chkNVBC.Checked;
            numericUpDown_port.Visible = !chkNVBC.Checked;
            lbl_connect.Visible = !chkNVBC.Checked;
            numericUpDown_scan.Visible = !chkNVBC.Checked;

            if (mvars.demoMode)
            {
                numericUpDown_ip.Maximum = 2;
                numericUpDown_ip.Value = 1;
            }
            else
            {
                //numericUpDown_ip.Maximum = mvars.nvCommList.Count;
                //numericUpDown_ip.Value = mvars.nvCommList.IndexOf(mvars._nCommPort) + 1;
            }

            //if (MultiLanguage.DefaultLanguage == "en-US")
            //{
            //    Form1.frmscrControl.Text = "Firmware Update";
            //}
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT") 
            //{
            //    Form1.frmscrControl.Text = "韌體更新";
            //}
            //else if (MultiLanguage.DefaultLanguage == "zh-CN") 
            //{
            //    Form1.frmscrControl.Text = "韧体更新";
            //}

            mvars.FormShow[3] = true;
            mvars.actFunc = "more";
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            //第一步：
            //設定TabControl控制項（名稱以tclDemo為例）的DrawMode屬性為：OwnerDrawFixed；用於指定由使用者來繪製標題
            //第二步：註冊TabControl控制項的DrawItem事件：
            //繪製標題
            // from https://topic.alibabacloud.com/tc/a/c--tabcontrol-color_1_31_31880410.html

            System.Drawing.SolidBrush bru = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255));
            System.Drawing.Font font = new System.Drawing.Font("Arial", 9F);//設定標籤字型樣式
            System.Drawing.SolidBrush bruFont = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0));// 標籤字型顏色
            System.Drawing.StringFormat StrFormat = new System.Drawing.StringFormat();
            StrFormat.LineAlignment = System.Drawing.StringAlignment.Center;// 設定文字垂直方向置中

            StrFormat.Alignment = System.Drawing.StringAlignment.Center;// 設定文字水平方向置中    

            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                if (i == 1) bru = new System.Drawing.SolidBrush(System.Drawing.Color.Fuchsia);
                else if (i == 2) bru = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 192, 0));
                else if (i == 3) bru = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 192, 255));
                System.Drawing.Rectangle recChild = tabControl1.GetTabRect(i);
                recChild.X += 2;
                recChild.Width -= 4;
                recChild.Y += 2;
                recChild.Height -= 2;
                e.Graphics.FillRectangle(bru, recChild);
                e.Graphics.DrawString(tabControl1.TabPages[i].Text, font, bruFont, recChild, StrFormat);
            }




            /*
             //擷取TabControl主控制項的工作區域

            Rectangle rec = tclDemo.ClientRectangle;

            //擷取背景圖片，我的背景圖片在項目資源檔中。

            Image backImage = Resources.楓葉;

            //建立一個StringFormat對象，用於對標籤文字的布局設定

            StringFormat StrFormat = new StringFormat();

            StrFormat.LineAlignment = StringAlignment.Center;// 設定文字垂直方向置中

            StrFormat.Alignment = StringAlignment.Center;// 設定文字水平方向置中          

            // 標籤背景填充顏色，也可以是圖片

　　　　SolidBrush bru = new SolidBrush(Color.FromArgb(72, 181, 250));

            SolidBrush bruFont = new SolidBrush(Color.FromArgb(217, 54, 26));// 標籤字型顏色

            Font font = new System.Drawing.Font("微軟雅黑",12F);//設定標籤字型樣式

            //繪製主控制項的背景

            e.Graphics.DrawImage(backImage, 0, 0, tclDemo.Width, tclDemo.Height);

            //繪製標籤樣式

            for (int i = 0; i < tclDemo.TabPages.Count; i++)

            {

                //擷取標籤頭的工作區域

                Rectangle recChild = tclDemo.GetTabRect(i);

                //繪製標籤頭背景顏色

                e.Graphics.FillRectangle(bru, recChild);

                //繪製標籤頭的文字

                e.Graphics.DrawString(tclDemo.TabPages[i].Text,font,bruFont,recChild,StrFormat);

            }
             */
        }


        private void chk_NVBC_Click(object sender, EventArgs e)
        {
            //lbl_ip.Visible = !chkNVBC.Checked;
            //numericUpDown_ip.Visible = !chkNVBC.Checked;
            lbl_sender.Visible = !chkNVBC.Checked;
            numericUpDown_sender.Visible = !chkNVBC.Checked;
            lbl_port.Visible = !chkNVBC.Checked;
            numericUpDown_port.Visible = !chkNVBC.Checked;
            lbl_connect.Visible = !chkNVBC.Checked;
            numericUpDown_scan.Visible = !chkNVBC.Checked;
        }


        private void uc_coding_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(groupBox2.Width.ToString());
        }







        private void btn_browseFPGA_Click(object sender, EventArgs e)
        {
            using (openFileDialog1 = new OpenFileDialog())
            {
                //openFileDialog1.FileName = "開啟檔案"; 預設開啟的檔案名
                openFileDialog1.Title = "開啟 FPGA bin 檔";
                openFileDialog1.Filter = "bin files (*.bin)|*.bin";//|All files (*.*)|*.*"; 去掉;//就可以跟All files結合

                //openFileDialog1.Filter = "Bin files (*.*)|*Primary*_A.bin";
                //openFileDialog1.FilterIndex = 2; 兩種檔案過濾再改2 .bin & *.* 預設值1
                openFileDialog1.RestoreDirectory = true;
                //openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
                string[] Svstr;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txt_FlashFileNameFull.Text = openFileDialog1.FileName;
                    Svstr = txt_FlashFileNameFull.Text.Split('\\');
                    if (Svstr.Length > 0)
                    {
                        txt_FlashFileName.Text = Svstr[Svstr.Length - 1];
                        if (mp.GetBin(txt_FlashFileNameFull.Text))
                        {
                            int checksum = (int)mp.CalCheckSum(mvars.ucTmp, 0, mvars.ucTmp.Length);
                            //lbl_nBinChecksum.Text = mvars.ucTmp.Length / 1024 + "Kbytes   checksum：0x" + checksum.ToString("X4");
                            //lbl_nBinChecksum.Text = "size：" + mvars.ucTmp.Length  + "  checksum：0x" + checksum.ToString("X4");
                            btn_nFPGAW.Enabled = true;

                            mp.GetBin(txt_FlashFileNameFull.Text);

                            //if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                            //{
                            //    byte[] Tmp = new byte[8 * 1024 * 1024];
                            //    Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                            //    Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                            //    Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                            //}

                            if (mvars.ucTmp.Length % (32 * 1024) != 0)
                            {
                                uint quotient = (uint)mvars.ucTmp.Length / (32 * 1024);
                                Array.Resize(ref mvars.ucTmp, (int)(quotient + 1) * 32 * 1024);
                            }
                            lbl_nBinChecksum.Text = "size：" + mvars.ucTmp.Length + "  checksum：0x" + checksum.ToString("X4");


                            txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                            string[] svs = txt_FlashFileName.Text.Split('_');
                            for (int svi = 0; svi < svs.Length; svi++)
                            {
                                if (svs[svi].ToUpper().Substring(0, 1) == "V")
                                {
                                    string txtverfbin = svs[svi].Substring(1, svs[svi].Length - 1);
                                    txt_verFbin.Text = txtverfbin.Replace(".", string.Empty);
                                    int count = 0;
                                    foreach (char c in txtverfbin)
                                    {
                                        if (c == '.')
                                        {
                                            count++;
                                        }
                                    }
                                    if (count == 2) { txt_verFbin.Enabled = false; break; } else { txt_verFbin.Text = ""; txt_verFbin.Enabled = true; }
                                }
                            }
                        }
                        else
                        {
                            lstget1.Items.Add("Please check bin file again");
                            lbl_nBinChecksum.Text = "checksum";
                            txt_FlashFileName.Text = "";
                            //lbl_nBinChecksum.Text = "Mbytes";
                            btn_nFPGAW.Enabled = false;
                        }
                    }
                    else
                    {
                        lbl_nBinChecksum.Text = "checksum";
                        txt_FlashFileName.Text = "";
                        lbl_nBinChecksum.Text = "Mbytes";
                        lstget1.Items.Add("Please check bin filename again");
                        btn_nFPGAW.Enabled = false;
                    }
                    lstget1.TopIndex = lstget1.Items.Count - 1;
                }
            }
            btn_draw.Enabled = btn_nFPGAW.Enabled;
            btn_nBMP.Enabled = btn_nFPGAW.Enabled;
        }



        private void btn_show_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string value = "Show FW";
                if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                        "    Change Function ?" + "\r\n" + "\r\n" + @"    ""Show FW"" or ""Read FW""", ref value, 39) == DialogResult.OK)
                {
                    if (value.ToUpper() == "SHOW FW")
                    {
                        btn_show.ForeColor = System.Drawing.Color.Black;
                        if (MultiLanguage.DefaultLanguage == "en-US") btn_show.Text = "Show FW";
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") btn_show.Text = "顯示硬體版本";
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") btn_show.Text = "显示硬件版本";
                    }
                    else if (value.ToUpper() == "READ FW")
                    {
                        btn_show.ForeColor = System.Drawing.Color.Red;
                        if (MultiLanguage.DefaultLanguage == "en-US") btn_show.Text = "Read FW";
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") btn_show.Text = "回讀硬體版本";
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") btn_show.Text = "回读硬件版本";
                    }
                }
            }
        }

        private void btn_show_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;

            //string[] svpcbaver = null;
            //int svcounts = 0;

            mvars.lCounts = 9999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;

            //string txt44;
            uc_coding.lstget1.Items.Clear();
            if (mvars.FormShow[3]) uc_coding.lstother.Items.Clear();
            mvars.flgSelf = true;
            mvars.nvBoardcast = false;
            mvars.isReadBack = true;

            //bool svpc = true;
            //if (Screen.AllScreens.GetUpperBound(0) == 0) { svpc = false; }

            #region NovaStar 有含重新填寫 Form1.lstm
            Form1.lstm.Items.Clear();

            //Form1.svhs = "";
            //Form1.svhp = "";
            //Form1.svhc = "";
            //int svcards = 0;
            //for (byte svip = 0; svip < mvars.nvCommList.Count; svip++)
            //{
            //    if (mvars._marsCtrlSystem.UnInitialize())
            //    {
            //        mp.funSaveLogs("cBOXIDONOFF,Unloaded marsCtrlSystem.UnInitialize");
            //    }

            //    mvars._nCommPort = mvars.nvCommList[svip];
            //    Form1.nScreenCnt = 0;
            //    Form1.nSenderCnt = 0;
            //    if (mvars._marsCtrlSystem.Initialize(mvars._nCommPort, out Form1.nScreenCnt, out Form1.nSenderCnt) == false)
            //    {
            //        mvars.errCode = "-1";
            //        mp.funSaveLogs("cBOXIDONOFF," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",marsCtrlSystem.Initialize fail");
            //    }
            //    else
            //    {
            //        mvars.iSender = 0;
            //        mvars.iPort = 0;
            //        mvars.iScan = 0;

            //        #region 接收卡資訊
            //        Form1.NovaStarDeviceResult = null;
            //        svcounts = 0;
            //        do
            //        {
            //            Form1.NovaStarDeviceResult = mvars._marsCtrlSystem.GetAllScannerStatusByCom();
            //            mp.doDelayms(100);
            //            svcounts += 1;
            //            if (Form1.NovaStarDeviceResult != null) break;
            //        }
            //        while (svcounts <= 10);
            //        if (Form1.NovaStarDeviceResult != null && svcounts <= 10)
            //        {
            //            typhwCard[] svhwCard = new typhwCard[Form1.NovaStarDeviceResult.Count];
            //            Array.Clear(svhwCard, 0, Form1.NovaStarDeviceResult.Count);

            //            svpcbaver = new string[Form1.NovaStarDeviceResult.Count];

            //            if (Form1.NovaStarDeviceResult.Count > 0)
            //            {
            //                svcards += Form1.NovaStarDeviceResult.Count;
            //                if (btn.Text.ToUpper() == "OFF" || btn.Text.IndexOf("關閉") != -1 || btn.Text.IndexOf("关闭") != -1)
            //                {
            //                    mvars.isReadBack = false;
            //                    mvars.nvBoardcast = true;
            //                    mvars.lblCmd = "PG_ASCTEXT";
            //                    mp.mPGASCTEXT(0, 0, "", 0);
            //                    if (svpc == false)
            //                    {
            //                        Form1.pvindex = 1;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(0);
            //                        Form1.pvindex = 21;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(4);
            //                        Form1.pvindex = 255;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(0);
            //                        Form1.pvindex = 255;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(1);
            //                        Form1.pvindex = 255;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(0);
            //                    }
            //                }
            //                else
            //                {
            //                    for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
            //                    {
            //                        Form1.lstm.Items.Add("  -> " + mvars.nvCommList[svip] + ",S" + Form1.NovaStarDeviceResult[svi].SenderIndex + ",P" + Form1.NovaStarDeviceResult[svi].PortIndex + ",C" + Form1.NovaStarDeviceResult[svi].ScannerIndex);

            //                        mvars.iSender = (byte)Form1.NovaStarDeviceResult[svi].SenderIndex;
            //                        mvars.iPort = (byte)Form1.NovaStarDeviceResult[svi].PortIndex;
            //                        mvars.iScan = (byte)Form1.NovaStarDeviceResult[svi].ScannerIndex;

            //                        svhwCard[svi].iSender = (byte)Form1.NovaStarDeviceResult[svi].SenderIndex;
            //                        svhwCard[svi].iPort = (byte)Form1.NovaStarDeviceResult[svi].PortIndex;
            //                        svhwCard[svi].iScan = (byte)Form1.NovaStarDeviceResult[svi].ScannerIndex;

            //                        if (Form1.svhs == "") Form1.svhs = Form1.NovaStarDeviceResult[svi].SenderIndex.ToString();
            //                        if (Form1.svhp == "") Form1.svhp = Form1.NovaStarDeviceResult[svi].PortIndex.ToString();
            //                        if (Form1.svhc == "") Form1.svhc = Form1.NovaStarDeviceResult[svi].ScannerIndex.ToString();

            //                        Form1.pvindex = 0;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        svhwCard[svi].verFPGA = mvars.verFPGA;

            //                        int svy = 20;
            //                        Form1.pvindex = 206;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            //                        {
            //                            if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1] == "0.0.0.0") svy += 135;
            //                        }
            //                        Form1.pvindex = 207;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            //                        {
            //                            if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1] == "0.0.0.0") svy += 135;
            //                        }

            //                        string svuser = "";
            //                        ///Addr 0			達哥專用(Read-only)	128
            //                        Form1.pvindex = 102;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(0);
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + svi; }
            //                        ///RData
            //                        Form1.pvindex = 104;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + svi; mvars.lstmcuR64000.Items.Add(svi.ToString("00") + ",-1"); }
            //                        else svuser = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
            //                        ///Addr 21	CG_MAT_A1	16	轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)	16384
            //                        Form1.pvindex = 102;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(21);
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + svi; }
            //                        ///RData
            //                        Form1.pvindex = 104;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + svi; mvars.lstmcuR64000.Items.Add(svi.ToString("00") + ",-1"); }
            //                        else svuser += "." + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
            //                        ///Addr 22	CG_MAT_A2	16	轉換矩陣a2 16-bit值(MSB = 0是正數, MSB = 1是負數)	0
            //                        Form1.pvindex = 102;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(22);
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + svi; }
            //                        ///RData
            //                        Form1.pvindex = 104;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + svi; mvars.lstmcuR64000.Items.Add(svi.ToString("00") + ",-1"); }
            //                        else svuser += mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
            //                        if (svuser.Substring(0, "128".Length) == "128" && svuser.IndexOf(".163840", 0) == -1) svuser = "O";
            //                        else svuser = "X";

            //                        if (svpc == false)
            //                        {
            //                            Form1.pvindex = 1;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(3);
            //                            Form1.pvindex = 21;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(0);
            //                            Form1.pvindex = 255;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(0);
            //                            Form1.pvindex = 255;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(1);
            //                            Form1.pvindex = 255;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(0);
            //                        }

            //                        Form1.pvindex = 254;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { svpcbaver[svi] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]; }
            //                        else svpcbaver[svi] = "-1";

            //                        mvars.lblCmd = "READ_MCUBOOTVER";
            //                        txt44 = (0xFFF0).ToString("X8");
            //                        mp.mhMCUBLREAD(txt44, 16);  //get verMCUB
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            //                        {
            //                            svhwCard[svi].verMCUB = mvars.verMCUB;
            //                            if (mp.IsNumeric(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)) == false) { svhwCard[svi].CurrentBootVer = 0; }
            //                            else { svhwCard[svi].CurrentBootVer = Convert.ToUInt16(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)); }
            //                        }

            //                        mvars.lblCmd = "READ_MCUAPPVER";
            //                        txt44 = (0x0007FFE0).ToString("X8");
            //                        mp.mhMCUBLREAD(txt44, 16);  //get verMCUA
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            //                        {
            //                            svhwCard[svi].verMCUA = mvars.verMCUA;
            //                            if (mp.IsNumeric(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)) == false) { svhwCard[svi].CurrentAppVer = 0; }
            //                            else { svhwCard[svi].CurrentAppVer = Convert.ToUInt16(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)); }
            //                        }

            //                        uc_coding.lstget1.Items.Add((svi + 1).ToString("00") + "  Sender" + (svhwCard[svi].iSender + 1) + " Port" + (svhwCard[svi].iPort + 1) + " Card" + (svhwCard[svi].iScan + 1));
            //                        if (mvars.FormShow[3]) uc_coding.lstother.Items.Add((svi + 1).ToString("00") + ",S" + (svhwCard[svi].iSender + 1) + ",P" + (svhwCard[svi].iPort + 1) + ",C" + (svhwCard[svi].iScan + 1));
            //                        uc_coding.lstget1.Items.Add("  --> PCBA " + svpcbaver[svi]);
            //                        uc_coding.lstget1.Items.Add("  --> FPGA " + mvars.verFPGA);
            //                        uc_coding.lstget1.Items.Add("  --> " + mvars.verMCUB + ", no." + svhwCard[svi].CurrentBootVer);
            //                        uc_coding.lstget1.Items.Add("  --> " + mvars.verMCUA + ", no." + svhwCard[svi].CurrentAppVer);
            //                        uc_coding.lstget1.Items.Add("  --> CCT enabled = " + svuser);
            //                        uc_coding.lstget1.Items.Add("");
            //                        uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;



            //                        mp.funSaveLogs(" (ShowFW) " + "Sender," + svhwCard[svi].iSender + 1 + ",Port," + (svhwCard[svi].iPort + 1).ToString("000") + ",Card," + svhwCard[svi].iScan + 1 + ",PCBA," + svpcbaver[svi] + ",FPGA," + mvars.verFPGA + "," + svhwCard[svi].verMCUB + "," + svhwCard[svi].verMCUA);


            //                        if (btn.Text.Length > 4 &&
            //                            (btn.Text.ToUpper().Substring(0, 4) == "SHOW" || btn.Text.Substring(0, 2) == "顯示" || btn.Text.Substring(0, 2) == "显示" ||
            //                             btn.Text.ToUpper().Substring(0, 4) == "ENAB" || btn.Text.Substring(0, 2) == "開啟" || btn.Text.Substring(0, 2) == "开启")
            //                            )
            //                        {
            //                            ///svs = String.Format("{0,-16}", svs); 不滿設定長度16，"-"後面補空白，沒有符號前面補空白
            //                            string svs = mvars.nvCommList[svip] + "_S" + (svhwCard[svi].iSender + 1) + "P" + (svhwCard[svi].iPort + 1) + "C" + (svhwCard[svi].iScan + 1);
            //                            if (svs.ToUpper().IndexOf("USB", 0) != -1) svs = "USB_S" + (svhwCard[svi].iSender + 1) + "P" + (svhwCard[svi].iPort + 1) + "C" + (svhwCard[svi].iScan + 1);

            //                            //mvars.lblCmd = "PG_ASCTEXT";
            //                            //mp.mPGASCTEXT(5, 20, svpcbaver[svi] + "~" + mvars.verFPGA + "~" + svhwCard[svi].CurrentBootVer + "~" + svhwCard[svi].CurrentAppVer, 1);

            //                            mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
            //                            mp.mhFPGASPIWRITEasc("", "", 1);
            //                            mp.mhFPGASPIWRITEasc(svs, mvars.verFPGA + "_" + svhwCard[svi].CurrentBootVer + "." + svhwCard[svi].CurrentAppVer + "-" + svuser, 1);
            //                            if (svy > 255)
            //                            {
            //                                Form1.pvindex = 116;
            //                                mvars.lblCmd = "FPGA_SPI_W";
            //                                mp.mhFPGASPIWRITE(svy);
            //                            }
            //                        }
            //                    }
            //                }
            //                mp.funSaveLogs("  -> " + mvars.nvCommList[svip] + ",hwCards," + Form1.NovaStarDeviceResult.Count);
            //            }
            //            else
            //            {
            //                mvars.errCode = "-99";
            //                if (mvars.flgSelf)
            //                {
            //                    uc_coding.lstget1.Items.Add("  -> " + mvars.nvCommList[svip] + ",No Connectors");
            //                    uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            //                }
            //                mp.funSaveLogs("cBOXIDONOFF," + mvars.nvCommList[svip] + ",No Connectors");
            //            }
            //        }
            //        else
            //        {
            //            mvars.errCode = "-5";
            //            if (mvars.flgSelf)
            //            {
            //                uc_coding.lstget1.Items.Add("  -> " + "NovaStarDeviceResult,fail");
            //                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            //            }
            //            mp.funSaveLogs("cBOXIDONOFF,NovaStarDeviceResult,fail");
            //        }
            //        #endregion
            //        Form1.lstm.Items.Add(" ↑ " + svip + "," + mvars.nvCommList[svip] + ",Sender accumulation," + Form1.nSenderCnt + ",hwCard accumulation," + Form1.NovaStarDeviceResult.Count + "," + Form1.svhs + "," + Form1.svhp + "," + Form1.svhc);
            //    }
            //}

        #endregion NovaStar 有含重新填寫 Form1.lstm


        Ex:
            //uc_coding.lstget1.Items.Add("Receiving card amount " + svcards);
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;

            //if (btn.Text.ToUpper().Substring(0, 4) == "DISA") btn.Text = "Enable Mapping";
            //else if (btn.Text.ToUpper().Substring(0, 4) == "ENAB") btn.Text = "Disable Mapping";
            //else if (btn.Text.Substring(0, 2) == "關閉") btn.Text = "開啟Mapping";
            //else if (btn.Text.Substring(0, 2) == "关闭") btn.Text = "开启Mapping";
            //else if (btn.Text.Substring(0, 2) == "開啟") btn.Text = "關閉Mapping";
            //else if (btn.Text.Substring(0, 2) == "开启") btn.Text = "关闭Mapping";

            btn.Enabled = true;
        }

        private void btn_browseLBbin_Click(object sender, EventArgs e)
        {
            string sNewFileNamle;
            //bReturnFlag = false;
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "開啟 bin 檔",
                InitialDirectory = ".\\",
                Filter = "bin files (*.bin)|*.bin"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_nLBFileNameFull.Text = dialog.FileName;
                string[] Svstr = txt_nLBFileNameFull.Text.Split('\\');

                if (Svstr.Length > 0)
                {
                    txt_nLBFileName.Text = Svstr[Svstr.Length - 1];

                    byte[] gBinFile = File.ReadAllBytes(txt_nLBFileNameFull.Text);
                    if (gBinFile.Length != (1 * 1024 * 1024))   //1048576
                    {
                        Array.Resize(ref gBinFile, 1 * 1024 * 1024);
                        sNewFileNamle = dialog.FileName.Replace(".bin", "_1MB.bin");
                        File.WriteAllBytes(sNewFileNamle, gBinFile);
                    }

                    Array.Resize(ref mvars.ucTmp, 1 * 1024 * 1024);

                    Array.Copy(gBinFile, mvars.ucTmp, gBinFile.Length);

                    //checksum = mp.CalChecksum(gBinFile, 0, (int)gBinFile.Length - 1);
                    int checksum = (int)mp.CalCheckSum(mvars.ucTmp, 0, mvars.ucTmp.Length);
                    label1.Text = "Checksum：0x" + checksum.ToString("X4") + mvars.ucTmp.Length;
                    btn_nLBpW.Enabled = true;


                    //if (mp.GetBin(txt_nLBFileNameFull.Text) == true)
                    //{
                    //    int checksum = (int)mp.CalCheckSum(mvars.ucTmp, 0, mvars.ucTmp.Length);
                    //    lbl_nBinChecksum.Text = mvars.ucTmp.Length / 1024 + "Kbytes   checksum：0x" + checksum.ToString("X4");
                    //    btn_nFPGAW.Enabled = true;
                    //}
                    //else
                    //{
                    //    lstget1.Items.Add("Please check bin file again");
                    //    lbl_nBinChecksum.Text = "checksum";
                    //    txt_nFlashFileName.Text = "";
                    //    lbl_nBinChecksum.Text = "Mbytes";
                    //    btn_nFPGAW.Enabled = false;
                    //}
                }
                else
                {
                    lbl_nBinChecksum.Text = "checksum";
                    txt_FlashFileName.Text = "";
                    lbl_nBinChecksum.Text = "Mbytes";
                    lstget1.Items.Add("Please check bin filename again");
                    btn_nFPGAW.Enabled = false;
                }
                lstget1.TopIndex = lstget1.Items.Count - 1;















                //gBinFile = File.ReadAllBytes(textBox208.Text);
                //if (gBinFile.Length != (1 * 1024 * 1024) && SPI_1MB_rdbtn.Checked)
                //{
                //    Array.Resize(ref gBinFile, 1 * 1024 * 1024);
                //    sNewFileNamle = dialog.FileName.Replace(".bin", "_1MB.bin");
                //    File.WriteAllBytes(sNewFileNamle, gBinFile);
                //}

                //checksum = CalChecksum(gBinFile, 0, (UInt32)gBinFile.Length - 1);
                //textBox207.Text = "Checksum：0x" + checksum.ToString("X4");
                //bReturnFlag = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstget1.SetBounds(422, 134, 277, 294);
            if (MultiLanguage.DefaultLanguage == "en-US") chkNVBC.Text = "ALL";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") chkNVBC.Text = "廣播";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") chkNVBC.Text = "广播";
            dgvbox.Visible = false;
            cmb_box.LostFocus -= new EventHandler(cmb_LostFocus);
            label8.Visible = false;
            numUD_boxCols.Visible = false;
            label7.Visible = false;
            numUD_boxRows.Visible = false;
            btn_BoxRESETn.Visible = false;
            lbl_FlashSel.Text = "FlashSel";

            if (FPGA_Flash_Mode_rbtn.Checked)
            {
                chkNVBC.Checked = true;
                mvars.svnova = true;
                FPGA_Flash_Mode_rbtn.Checked = false;
                FPGA_Reg_Mode_rbtn.Checked = true;
                callOnClick(FPGA_FlashPathSend_btn);
            }

            cmb_FlashSel.Visible = false;
            if (tabControl1.SelectedTab.Name == "tabpage_mcu")
            { 
                if (mvars.svnova == false)
                {
                    if (mvars.deviceID.Substring(0, 2) == "05")
                    {

                    }
                }
                else
                {
                    lbl_jedecid.Visible = false; chk_NVBC.Checked = true;
                    chk_NVBC_Click(null, null);
                }
            }
            else
            {
                lbl_jedecid.Visible = true;
                lbl_FlashSel.Visible = true;
                cmb_FlashSel.Visible = true;
                cmb_FlashSel.Items.Clear();
                if (tabControl1.SelectedTab.Name == "tabpage_bmp")
                {
                    #region FPGA
                    if (mvars.svnova == false)
                    {
                        if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")
                        {
                            cmb_FlashSel.Items.Add("Default");
                            cmb_FlashSel.Items.Add("FPGA_A");
                            cmb_FlashSel.Items.Add("FPGA_B");
                        }
                        cmb_FlashSel.SelectedIndex = 0;
                    }

                    //lstget1.Items.Add(tabpage_bmp.Text);
                    //Skeleton_nBox((int)numUD_boxCols.Value, (int)numUD_boxRows.Value, 0, 0);
                    //dgvbox.Visible = true;
                    //label8.Visible = true;
                    //numUD_boxCols.Visible = true;
                    //label7.Visible = true;
                    //numUD_boxRows.Visible = true;
                    //btn_BoxRESETn.Visible = true;
                    //cmb_box.LostFocus += new EventHandler(cmb_LostFocus);
                    #endregion
                }
                else if (tabControl1.SelectedTab.Name == "tabpage_dmr")
                {
                    #region Demura
                    if (mvars.flgbootloader == false)
                    {
                        if (mvars.svnova == false)
                        {
                            if (mvars.deviceID.Substring(0, 2) == "05")
                            {
                                cmb_FlashSel.Items.Add("Default");
                                cmb_FlashSel.Items.Add("CB_1");
                                cmb_FlashSel.Items.Add("CB_2");
                                cmb_FlashSel.Items.Add("CB_3");
                                cmb_FlashSel.Items.Add("CB_4");
                                cmb_FlashSel.Items.Add("XB_1");
                                cmb_FlashSel.Items.Add("XB_2");
                                cmb_FlashSel.Items.Add("XB_3");
                                cmb_FlashSel.Items.Add("XB_4");
                                cmb_FlashSel.Items.Add("XB_5");
                                cmb_FlashSel.Items.Add("XB_6");
                                cmb_FlashSel.Items.Add("XB_7");
                                cmb_FlashSel.Items.Add("XB_8");
                            }
                            else if (mvars.deviceID.Substring(0, 2) == "06")
                            {
                                cmb_FlashSel.Items.Add("Default");
                                cmb_FlashSel.Items.Add("CB_1");
                                cmb_FlashSel.Items.Add("CB_2");
                                cmb_FlashSel.Items.Add("LB_1");
                                cmb_FlashSel.Items.Add("LB_2");
                                cmb_FlashSel.Items.Add("LB_3");
                                cmb_FlashSel.Items.Add("LB_4");
                                cmb_FlashSel.Items.Add("LB_5");
                                cmb_FlashSel.Items.Add("LB_6");
                                cmb_FlashSel.Items.Add("LB_7");
                                cmb_FlashSel.Items.Add("LB_8");
                                cmb_FlashSel.Items.Add("LB_9");
                                cmb_FlashSel.Items.Add("LB_10");
                                cmb_FlashSel.Items.Add("LB_11");
                                cmb_FlashSel.Items.Add("LB_12");
                            }
                            cmb_FlashSel.SelectedIndex = 0;
                        }
                        else
                        {
                            Skeleton_nBox((int)numUD_boxCols.Value, (int)numUD_boxRows.Value, 0, 0);
                            dgvbox.Visible = true;
                            label8.Visible = true;
                            numUD_boxCols.Visible = true;
                            label7.Visible = true;
                            numUD_boxRows.Visible = true;
                            btn_BoxRESETn.Visible = true;
                            cmb_box.LostFocus += new EventHandler(cmb_LostFocus);
                        }
                    }
                    else
                    {
                        txt_filepathfull.Visible = false;
                        chk_NVBC.Checked = false;
                        chk_NVBC_Click(null, null);
                    }
                    #endregion
                }
                else if (tabControl1.SelectedTab.Name == "tabpage_lb")
                {
                    if (mvars.svnova == false)
                    {
                        if (mvars.deviceID.Substring(0, 2) == "05")
                        {
                            cmb_FlashSel.Items.Add("CB_1");
                            cmb_FlashSel.Items.Add("CB_2");
                            cmb_FlashSel.Items.Add("CB_3");
                            cmb_FlashSel.Items.Add("CB_4");
                        }
                        cmb_FlashSel.SelectedIndex = 0;
                    }
                    else
                    {
                        FPGA_Flash_Mode_rbtn.Checked = true;
                    }
                    //chkNVBC.Checked = true;
                    //mvars.svnova = true;
                    //FPGA_Flash_Mode_rbtn.Checked = true;
                    //FPGA_Reg_Mode_rbtn.Checked = false;
                    //callOnClick(FPGA_FlashPathSend_btn);
                }
            }
        }
        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            mvars.flgForceUpdate = false;
        }

        private void btn_nFPGAW_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;

            System.Drawing.Color svc;
            if (tabControl1.SelectedTab.Name == "tabpage_mcu") svc = System.Drawing.Color.FromArgb(0, 192, 0);
            else svc = System.Drawing.Color.Black;

            if (e.Button == MouseButtons.Right)
            {
                string value = "false";
                if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                        "    Force Update ?" + "\r\n" + "\r\n" + "    false/true", ref value, 139) == DialogResult.OK)
                {
                    if (value == "true") { mvars.flgForceUpdate = true; btn.ForeColor = System.Drawing.Color.Red; }
                    else if (value.ToLower() == "bmp") { mvars.flgForceUpdate = true; btn.ForeColor = System.Drawing.Color.FromArgb(255, 255, 0); }
                    else if (value.ToLower() == "trueall") { svforceAll = true;btn.ForeColor = System.Drawing.Color.Red;btn.Text = "Force O"; }
                    else { btn.ForeColor = svc; }
                }
                mp.funSaveLogs("(" + tabControl1.SelectedTab.Text + ") Forcr Update:" + value);
            }
        }



        private void btn_nDMRR_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;

            lstget1.SetBounds(422, 137, 277, 294); /// 0104
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();


            btn_nDMRBREAK.Visible = true;
            btn_nDMRBREAK.Enabled = true;

            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (cmbFlashSel.Text.ToUpper().IndexOf("FPGA", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("DEMURA", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("XB_", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("CB_", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("LB_", 0) != -1)
                    mp.cFLASHREAD_pCB(cmbFlashSel.Text, lst_get1);
            }




                mvars.nvBoardcast = false;


            
            



        Ex:
            mvars.Break = false;    /// 0104

            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;

            mvars.nvBoardcast = true;
            mvars.flashselQ = 0;
            mvars.lblCmd = "FLASH_TYPE";
            mp.mhFLASHTYPE();

            if (btn.Tag.ToString().ToUpper() == "FPGA") { btn_nFPGAR.Enabled = true; btn_nFPGABREAK.Enabled = false; }
            else if (btn.Tag.ToString().ToUpper() == "DMR") { btn_nDMRR.Enabled = true; btn_nDMRBREAK.Enabled = false; btn_nDMRBREAK.Visible = false; }

            sw.Stop();
            btn.Enabled = true;
        }

        private void btn_nDMRBREAK_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            mvars.Break = true;
            btn.Enabled = false;
        }

        private void btn_browseDMR_Click(object sender, EventArgs e)
        {
            /*
            foreach (var path in Directory.GetFiles(@"C:\Name\Folder\"))
            {
                Console.WriteLine(path); // full path
                Console.WriteLine(System.IO.Path.GetFileName(path)); // file name
            }
            */

            /*
             * 讀取資料夾內所有文字檔案，一次讀取一行，並儲存在 List 裡面
            List<string> myList = new List<string>();
            // 執行檔路徑下的 MyDir 資料夾
            string folderName = System.Windows.Forms.Application.StartupPath + @"\MyDir";
            // 取得資料夾內所有檔案
            foreach (string fname in System.IO.Directory.GetFiles(folderName))
            {
                string line;
                // 一次讀取一行
                System.IO.StreamReader file = new System.IO.StreamReader(fname);
                while ((line = file.ReadLine()) != null)
                {
                    myList.Add(line.Trim());
                }
                file.Close();
            }
            */

            /*

            

            */
            if (mvars.deviceID.Substring(0, 2) == "02")
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "請選擇大屏 demura bin 所在主資料夾";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        MessageBox.Show(this, "資料夾路徑不能為空", "提示");
                        return;
                    }
                    //this.LoadingText = "處理中...";
                    //this.LoadingDisplay = true;
                    //Action<string> a = DaoRuData;
                    //a.BeginInvoke(dialog.SelectedPath, asyncCallback, a);
                    lstget1.Items.Clear();
                    lst_filepathfull.Items.Clear();
                    cmb_box.Items.Clear();
                    dirSearch(dialog.SelectedPath);
                    if (lstget1.Items.Count > 0) { btn_nDMRdraw.Enabled = true; btn_nDMRbmp.Enabled = true; }
                    else { btn_nDMRdraw.Enabled = false; btn_nDMRbmp.Enabled = false; }
                }
            }
            else
            {
                string svs = "";
                if (MultiLanguage.DefaultLanguage == "en-US")
                    svs = "Select Demura Flash file";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    svs = "請選擇 Demura 閃存檔案";
                else if (MultiLanguage.DefaultLanguage == "zh-CN" || MultiLanguage.DefaultLanguage == "ja-JP")
                    svs = "请选择 Demura 闪存档案";
                openFileDialog1 = new OpenFileDialog
                {
                    Title = svs,
                    InitialDirectory = ".\\",
                };
                //sting svfilter = "";
                //if (btn_browseDMR.BackColor == Color.Transparent)
                //    openFileDialog1.Filter = "Bin files (*.*)|*" + CB_1 + "*.bin";
                //else
                //    openFileDialog1.Filter = "Bin files (*.*)|*.bin";

                openFileDialog1.Filter = "bin files (*.bin)|*.bin";
                string[] Svstr;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txt_FlashFileNameFull.Text = openFileDialog1.FileName;
                    Svstr = txt_FlashFileNameFull.Text.Split('\\');
                    if (Svstr.Length > 0)
                    {
                        txt_DMRFileName.Text = Svstr[Svstr.Length - 1];
                        if (mp.GetBin(txt_FlashFileNameFull.Text))
                        {
                            byte[] gBinDemura1 = new byte[mvars.ucTmp.Length];
                            Array.Copy(mvars.ucTmp, gBinDemura1, mvars.ucTmp.Length);
                            mp.BinFilter_00_FF(ref gBinDemura1);
                            Array.Resize(ref mvars.ucTmp, gBinDemura1.Length);
                            Array.Copy(gBinDemura1, mvars.ucTmp, gBinDemura1.Length);
                            int checksum = (int)mp.CalCheckSum(mvars.ucTmp, 0, mvars.ucTmp.Length);
                            if (mvars.ucTmp.Length % (32 * 1024) != 0)
                            {
                                uint quotient = (uint)mvars.ucTmp.Length / (32 * 1024);
                                Array.Resize(ref mvars.ucTmp, (int)(quotient + 1) * 32 * 1024);
                            }
                            lbl_nDMRChecksum.Text = "size：" + mvars.ucTmp.Length + "  checksum：0x" + checksum.ToString("X4");

                            //Barcode Infomation
                            Byte[] Barcode = new byte[32];
                            Array.Copy(mvars.ucTmp, 0x0003FF5D, Barcode, 0, Barcode.Length);
                            lbl_barcode.Text = System.Text.Encoding.ASCII.GetString(Barcode);
                        }
                        else
                        {
                            lstget1.Items.Add("Please check bin file again");
                            lbl_nBinChecksum.Text = "checksum";
                            txt_DMRFileName.Text = "";
                            btn_nFPGAW.Enabled = false;
                        }
                    }
                    else
                    {
                        lbl_nBinChecksum.Text = "checksum";
                        txt_DMRFileName.Text = "";
                        lstget1.Items.Add("Please check bin filename again");
                        btn_nFPGAW.Enabled = false;
                    }
                    lstget1.TopIndex = lstget1.Items.Count - 1;
                }
            }      
        }

        void dirSearch(string sDir)
        {
            // List<string> filelist = new List<string>();
            // 
            try
            {
                //先找出所有目錄
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    //先針對目錄的檔案做處理
                    foreach (string f in Directory.GetFiles(d, "*.bin"))
                    {
                        Console.WriteLine(f);
                        //filelist.Add(f);
                        lst_filepathfull.Items.Add(f);
                        lstget1.Items.Add(f.Split('\\')[f.Split('\\').Length - 1]);
                        cmb_box.Items.Add(lstget1.Items[lstget1.Items.Count - 1].ToString().Split('.')[0]);
                    }
                    //此目錄完再針對每個子目錄做處理
                    dirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            if (lstget1.Items.Count <= 0)
            {
                foreach (string f in Directory.GetFiles(sDir, "*.bin"))
                {
                    Console.WriteLine(f);
                    //filelist.Add(f);
                    lst_filepathfull.Items.Add(f);
                    lstget1.Items.Add(f.Split('\\')[f.Split('\\').Length - 1]);
                    cmb_box.Items.Add(lstget1.Items[lstget1.Items.Count - 1].ToString().Split('.')[0]);
                }
            }
            lstget1.Refresh();

            //lstget1.Items.AddRange(filelist);
            /*
            //查詢資料夾內容清單
            string[] filecollection;
            string filepath = "f:\\uploads";
            FileInfo thefileinfo;

            filecollection = Directory.GetFiles(filepath, "*.txt");
            for (int i = 0; i < filecollection.Length; i++)
            {
                thefileinfo = new FileInfo(filecollection[i]);
                Console.WriteLine(thefileinfo.Name.ToString());
            }
            */
        }

        private void txt_nDMRFileName_DoubleClick(object sender, EventArgs e)
        {
            using (openFileDialog1 = new OpenFileDialog())
            {
                //openFileDialog1.FileName = "開啟檔案"; 預設開啟的檔案名
                openFileDialog1.Title = "開啟 bin 檔";
                openFileDialog1.Filter = "bin files (*.bin)|*.bin";//|All files (*.*)|*.*"; 去掉;//就可以跟All files結合
                //openFileDialog1.FilterIndex = 2; 兩種檔案過濾再改2 .bin & *.* 預設值1
                openFileDialog1.RestoreDirectory = true;
                //openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
                string[] Svstr;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txt_FlashFileNameFull.Text = openFileDialog1.FileName;
                    Svstr = txt_FlashFileNameFull.Text.Split('\\');
                    if (Svstr.Length > 0)
                    {
                        if (tabControl1.SelectedTab == tabpage_dmr) { txt_DMRFileName.Text = Svstr[Svstr.Length - 1]; }
                        else { txt_FlashFileName.Text = Svstr[Svstr.Length - 1]; }
                        if (mp.GetBin(txt_FlashFileNameFull.Text) == true)
                        {
                            int checksum = (int)mp.CalCheckSum(mvars.ucTmp, 0, mvars.ucTmp.Length);

                            txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                            if (tabControl1.SelectedTab == tabpage_dmr)
                            {
                                btn_nDMRW.Enabled = true;
                                lbl_nDMRChecksum.Text = mvars.ucTmp.Length / 1024 + "Kbytes   checksum：0x" + checksum.ToString("X4");
                            }
                            else
                            {
                                btn_nFPGAW.Enabled = true;
                                txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                                string[] svs = txt_FlashFileName.Text.Split('_');
                                for (int svi = 0; svi < svs.Length; svi++)
                                {
                                    if (svs[svi].ToUpper().Substring(0, 1) == "V")
                                    {
                                        string txtverfbin = svs[svi].Substring(1, svs[svi].Length - 1);
                                        txt_verFbin.Text = txtverfbin.Replace(".", string.Empty);
                                        int count = 0;
                                        foreach (char c in txtverfbin)
                                        {
                                            if (c == '.')
                                            {
                                                count++;
                                            }
                                        }
                                        if (count == 2) { txt_verFbin.Enabled = false; break; } else { txt_verFbin.Text = ""; txt_verFbin.Enabled = true; }
                                    }
                                }
                                lbl_nBinChecksum.Text = mvars.ucTmp.Length / 1024 + "Kbytes   checksum：0x" + checksum.ToString("X4");
                            }
                        }
                        else
                        {
                            if (tabControl1.SelectedTab == tabpage_dmr) { btn_nDMRW.Enabled = false; txt_DMRFileName.Text = " *.bin (double click here to select bin file)"; lbl_nDMRChecksum.Text = "checksum"; }
                            else { btn_nFPGAW.Enabled = false; txt_FlashFileName.Text = " *.bin (double click here to select bin file)"; lbl_nBinChecksum.Text = "checksum"; }
                        }
                    }
                    else
                    {
                        if (tabControl1.SelectedTab == tabpage_dmr) { btn_nDMRW.Enabled = false; txt_DMRFileName.Text = " *.bin (double click here to select bin file)"; lbl_nDMRChecksum.Text = "checksum"; }
                        else { btn_nFPGAW.Enabled = false; txt_FlashFileName.Text = " *.bin (double click here to select bin file)"; lbl_nBinChecksum.Text = "checksum"; }
                    }
                }
            }
        }

        private void btn_nDMRW_Click(object sender, EventArgs e)
        {
            /*
            Button btn = (Button)sender;
            int svDev = 0;

            #region config
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            ushort svundos = 0;
            bool[] svundo = null;
            string txtverfbin = "";
            byte svEraseNwrNchk = 0x3F;
            //Write only mvars.c12aflashitem = 0(CB)-- > 0x3F and mvars.c12aflashitem = 1(XB)-- > 0x39
            //Erase and Write and Check mvars.c12aflashitem = 0(CB)-- >  = 0x42
            svEraseNwrNchk = 0x42;

            string[] svpcbaver = null;
            mvars.lCounts = 19999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            int i = 0;
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;
            //short sverrc = 0;
            //string txt44;
            UInt32 FlashSize;
            ushort PacketSize;
            UInt32 Count;
            lstget1.Items.Clear();
            mvars.flgSelf = true;
            if (btn.Tag.ToString() == "F") mvars.flashselQ = 1;          //FPGA
            else if (btn.Tag.ToString() == "D") mvars.flashselQ = 2;    //Demura
            int svcounts;
            int svlstc = 0;
            #endregion config

            lstget1.Items.Clear();

            sw1.Reset();
            sw1.Start();


            ushort svsen = (ushort)numericUpDown_sender.Value;
            ushort svpo = (ushort)numericUpDown_port.Value;
            ushort svca = (ushort)numericUpDown_connect.Value;


            if (btn.Tag.ToString() == "F")
            {
                txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                //lstget1.Items.Add("FPGA version compare ...");
                //lstget1.TopIndex = lstget1.Items.Count - 1;
                //txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                string[] svs = txt_nFlashFileName.Text.Split('_');
                for (int svi = 0; svi < svs.Length; svi++)
                {
                    if (svs[svi].ToUpper().Substring(0, 1) == "V")
                    {
                        txtverfbin = svs[svi].Substring(1, svs[svi].Length - 1);
                        txt_verFbin.Text = txtverfbin.Replace(".", string.Empty);
                        int count = 0;
                        foreach (char c in txtverfbin)
                        {
                            if (c == '.')
                            {
                                count++;
                            }
                        }
                        if (count == 2) { txt_verFbin.Enabled = false; break; } else { txt_verFbin.Text = ""; txt_verFbin.Enabled = true; }
                    }
                }
                mp.doDelayms(10);
            }


            if (mvars.svnova)
            {
                #region NovaStar
                if (Form1.lstm.Items.Count != 0)
                {
                    bool svchk = false;
                    ushort svctrlsys = 0;
                    for (int svlc = 0; svlc < Form1.lstm.Items.Count; svlc++)
                    {
                        if (Form1.lstm.Items[svlc].ToString().IndexOf('↑') != -1)
                        {
                            ushort svc = Convert.ToUInt16(Form1.lstm.Items[svlc].ToString().Split(',')[3]);
                            if (svsen - svc <= 0)
                            {
                                svchk = true;
                                svctrlsys = Convert.ToUInt16(Form1.lstm.Items[svlc].ToString().Split(',')[1]);
                                //排頭訊息
                                Form1.svhs = Form1.lstm.Items[svlc].ToString().Split(',')[6];
                                Form1.svhp = Form1.lstm.Items[svlc].ToString().Split(',')[7];
                                Form1.svhc = Form1.lstm.Items[svlc].ToString().Split(',')[8];
                            }
                            else { svsen -= svc; }
                        }
                        //因應網絡型發送卡無法廣播, 所以需要透過 lstm 來找出是哪一個 IP(發送卡), 如果每一個 IP 下都有 2 台發送卡
                        //  ................S0,P,,,,
                        //  ................S1,p,,,,
                        // " ↑ " + svip + "," + mvars.nvCommList[svip] + ",Sender accumulation,2,...
                        //  ................S0,P,,,,
                        //  ................S1,p,,,,
                        // " ↑ " + svip + "," + mvars.nvCommList[svip] + ",Sender accumulation,2,...
                        // Sender 都是重複性的從 0 開始起算, 所以輸入的 svsen 要越過不同的 CtrlSysCnt 的時候就必須先減掉發送卡數量 --> svsen -= svc
                    }
                    if (svchk)
                    {
                        mvars.iSender = Convert.ToByte(Form1.svhs);
                        mvars.iPort = Convert.ToByte(Form1.svhp);
                        mvars.iScan = Convert.ToUInt16(Form1.svhc);
                        if (mvars.nvBoardcast == false)
                        {
                            svchk = false;
                            mvars.iSender = (byte)(svsen - 1);
                            mvars.iPort = (byte)(svpo - 1);
                            mvars.iScan = (ushort)(svca - 1);
                            int svlc = 0;
                            for (svlc = 0; svlc < Form1.lstm.Items.Count; svlc++)
                            {
                                if (Form1.lstm.Items[svlc].ToString().IndexOf("S" + mvars.iSender + ",P" + mvars.iPort + ",C" + mvars.iScan, 0) != -1) { svchk = true; break; }
                            }
                        }
                    }
                    if (svchk)
                    {
                        for (byte svCtrlSysCnt = 0; svCtrlSysCnt < mvars.nvCommList.Count; svCtrlSysCnt++)
                        {
                            if (mvars._marsCtrlSystem.UnInitialize())
                            {
                                mp.funSaveLogs(tabControl1.SelectedTab.Name + ",Unloaded marsCtrlSystem.UnInitialize");
                            }

                            mvars._nCommPort = mvars.nvCommList[svCtrlSysCnt];
                            Form1.nScreenCnt = 0;
                            Form1.nSenderCnt = 0;
                            if (mvars._marsCtrlSystem.Initialize(mvars._nCommPort, out Form1.nScreenCnt, out Form1.nSenderCnt) == false)
                            {
                                mvars.errCode = "-1";
                                mp.funSaveLogs(tabControl1.SelectedTab.Name + "," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",marsCtrlSystem.Initialize fail");
                            }
                            else
                            {
                                Form1.NovaStarDeviceResult = null;
                                svcounts = 0;
                                do
                                {
                                    Form1.NovaStarDeviceResult = mvars._marsCtrlSystem.GetAllScannerStatusByCom();
                                    mp.doDelayms(100);
                                    svcounts += 1;
                                    if (Form1.NovaStarDeviceResult != null) break;
                                }
                                while (svcounts <= 10);
                                if (Form1.NovaStarDeviceResult != null && svcounts <= 10)
                                {
                                    if (Form1.NovaStarDeviceResult.Count > 0)
                                    {
                                        Array.Resize(ref Form1.hwCard, Form1.NovaStarDeviceResult.Count);
                                        Array.Resize(ref svundo, Form1.NovaStarDeviceResult.Count);
                                        Array.Resize(ref svpcbaver, Form1.NovaStarDeviceResult.Count);
                                        if (mvars.flgSelf == false) { lstget1.Items.Clear(); }
                                        for (int j = 0; j < Form1.NovaStarDeviceResult.Count; j++)  //Form1.NovaStarDeviceResult.Count = Form1.NovaStarDeviceResult.Count;
                                        {
                                            Form1.hwCard[j].iSender = Form1.NovaStarDeviceResult[j].SenderIndex;
                                            Form1.hwCard[j].iPort = Form1.NovaStarDeviceResult[j].PortIndex;
                                            Form1.hwCard[j].iScan = Form1.NovaStarDeviceResult[j].ScannerIndex;
                                            svundo[j] = false;
                                        }
                                        Form1.hwCards = Form1.hwCard.Length;
                                        mvars.lCounts = 9999;
                                        Array.Resize(ref mvars.lCmd, mvars.lCounts);
                                        Array.Resize(ref mvars.lGet, mvars.lCounts);
                                        mvars.lCount = 0;
                                        mvars.strReceive = "";
                                        mvars.flgDelFB = true;
                                        mvars.flgSelf = true;
                                        mvars.nvBoardcast = false;
                                        mvars.isReadBack = true;

                                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                                        sw.Reset();
                                        sw.Start();
                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            //mvars.errCode = "000";
                                            mvars.iSender = (byte)Form1.NovaStarDeviceResult[svDevCnt].SenderIndex;
                                            mvars.iPort = (byte)Form1.NovaStarDeviceResult[svDevCnt].PortIndex;
                                            mvars.iScan = (byte)Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex;

                                            Form1.pvindex = 0;
                                            mvars.lblCmd = "FPGA_SPI_R";
                                            mp.mhFPGASPIREAD();
                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                            Form1.hwCard[svDevCnt].verFPGA = mvars.verFPGA;
                                            Form1.pvindex = 254;
                                            mvars.lblCmd = "FPGA_SPI_R";
                                            mp.mhFPGASPIREAD();
                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                            else svpcbaver[svDevCnt] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                                            //if (svundo[svDevCnt] == false)
                                            //{
                                                //mvars.lblCmd = "PG_ASCTEXT";
                                                //mp.mPGASCTEXT(5, 20, "PCBA" + svpcbaver[svDevCnt] + " FPGA" + mvars.verFPGA, 1);
                                                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                                if (svpcbaver[svDevCnt] != "1" && svpcbaver[svDevCnt] != "2") { svundo[svDevCnt] = true; }
                                                if (btn.Tag.ToString() == "F" && txt_verFbin.Text != "" && mvars.verFPGA != "127.31.15")
                                                {
                                                    if (Convert.ToInt16(txtverfbin.Split('.')[0]) < (Convert.ToInt16(mvars.verFPGA.Split('.')[0]))) { if (svundo[svDevCnt] == false) svundo[svDevCnt] = !mvars.flgForceUpdate; }
                                                    else
                                                    {
                                                        if (Convert.ToInt16(txtverfbin.Split('.')[0]) == (Convert.ToInt16(mvars.verFPGA.Split('.')[0])) && Convert.ToInt16(txtverfbin.Split('.')[1]) < (Convert.ToInt16(mvars.verFPGA.Split('.')[1]))) { if (svundo[svDevCnt] == false) svundo[svDevCnt] = !mvars.flgForceUpdate; }
                                                        else
                                                        {
                                                            if (Convert.ToInt16(txtverfbin.Split('.')[1]) == (Convert.ToInt16(mvars.verFPGA.Split('.')[1])) && Convert.ToInt16(txtverfbin.Split('.')[2]) == (Convert.ToInt16(mvars.verFPGA.Split('.')[2]))) { if (svundo[svDevCnt] == false) svundo[svDevCnt] = !mvars.flgForceUpdate; }
                                                        }
                                                    }
                                                }
                                            //}
                                            //非廣播判斷
                                            if (chkNVBC.Checked == false)
                                            {
                                                if (mvars.iSender + 1 == numericUpDown_sender.Value && mvars.iPort + 1 == numericUpDown_port.Value && mvars.iScan + 1 == numUDScan.Value) svDev = svDevCnt;
                                                else svundo[svDevCnt] = true;
                                            }
                                        }
                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            mvars.iSender = (byte)Form1.NovaStarDeviceResult[svDevCnt].SenderIndex;
                                            mvars.iPort = (byte)Form1.NovaStarDeviceResult[svDevCnt].PortIndex;
                                            mvars.iScan = (byte)Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex;

                                            mvars.lblCmd = "PG_ASCTEXT";
                                            mp.mPGASCTEXT(5, 20, "", 1);
                                            if (svundo[svDevCnt])
                                            {
                                                mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
                                                mp.mhFPGASPIWRITEasc("PCBA" + svpcbaver[svDevCnt] + " FPGA" + mvars.verFPGA, "UnDo", 1);
                                                svundos++;
                                            }
                                        }
                                        lstget1.Items.Add("Read version " + Convert.ToString(string.Format("{0:0.#}", sw.Elapsed.TotalSeconds)) + "s");
                                        lstget1.TabIndex = lstget1.Items.Count - 1;




                                        if (btn.Tag.ToString() == "D")
                                        {
                                            //svundos = 0;
                                            //for (int j = 0; j < Form1.NovaStarDeviceResult.Count; j++) { if (svundo[j]) svundos++; };

                                            //if (svundos <= Form1.NovaStarDeviceResult.Count)
                                            //{
                                            //    for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                            //    {
                                            //        if (svundo[svDevCnt] == false)
                                            //        {

                                            //        }
                                            //    }
                                            //}
                                            //svundos = 0;
                                            //for (int j = 0; j < Form1.NovaStarDeviceResult.Count; j++)
                                            //{
                                            //    if (svundo[j]) svundos++;
                                            //}
                                            #region Bin Write
                                            //if (mvars.flgForceUpdate) { Array.Clear(svundo, 0, svundo.Length); }
                                            //if (svundos < Form1.NovaStarDeviceResult.Count || mvars.flgForceUpdate)
                                            if (svundos < Form1.NovaStarDeviceResult.Count)
                                            {
                                                lstget1.Items.RemoveAt(0);
                                                lstget1.Items.Add("Demura update");
                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                mvars.nvBoardcast = true;       //廣播

                                                if (svEraseNwrNchk == 0x42)
                                                {
                                                    Form1.pvindex = 1;
                                                    mvars.lblCmd = "FPGA_SPI_W";
                                                    mp.mhFPGASPIWRITE(3);      // 01 SI_SEL PG mode
                                                    Form1.pvindex = 255;
                                                    mvars.lblCmd = "FPGA_SPI_W255";
                                                    mp.mhFPGASPIWRITE(0);
                                                    mp.mhFPGASPIWRITE(1);
                                                    mp.mhFPGASPIWRITE(0);
                                                }

                                                mvars.lblCmd = "FLASH_TYPE";
                                                mp.mhFLASHTYPE();
                                                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-3"; goto Ex; }
                                                mvars.lblCmd = "FLASH_FUNCQE";
                                                mp.mhFUNCQE();
                                                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-4"; goto Ex; }
                                                mvars.lblCmd = "FUNC_ENABLE";
                                                mp.mhFUNCENABLE();
                                                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-4.5"; goto Ex; }
                                                mvars.lblCmd = "FUNC_STATUS";
                                                mp.mhFUNCSTATUS();

                                                mvars.nvBoardcast = false;

                                                for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                                                {
                                                    if (chkNVBC.Checked)
                                                    {
                                                        if (svundo[svi] == false)
                                                        {
                                                            mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                                                            mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                                                            mvars.iScan = (byte)Form1.hwCard[svi].iScan;

                                                            mp.funSendMessageTo(true);
                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svi] = true; }
                                                            else
                                                            {
                                                                if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                                {
                                                                    if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                    svundo[svi] = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Form1.hwCard[svi].iSender != svsen - 1 || Form1.hwCard[svi].iPort != svpo - 1 || Form1.hwCard[svi].iScan != svca - 1) { svundo[svi] = true; }
                                                    }
                                                    
                                                    if (svundo[svi]) svundos++;
                                                }

                                                if (svundos < Form1.NovaStarDeviceResult.Count)
                                                {
                                                    //寫入使用Boardcast
                                                    //mvars.nvBoardcast = svNVBC;

                                                    //mvars.lblCmd = "HW_RESET_FPGA"; 
                                                    //mp.mhSWRESET(SvNova, 0x80);
                                                    //Form1.lst_get1.Items.Add(" --> Wait 5s"); Form1.lst_get1.TopIndex = Form1.lst_get1.Items.Count - 1;
                                                    //mp.doDelaySec(5);                   
                                                    sw1.Reset();
                                                    sw1.Start();
                                                    if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                    {
                                                        byte[] Tmp = new byte[8 * 1024 * 1024];
                                                        Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                        Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                        Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                    }
                                                    FlashSize = (UInt32)(mvars.ucTmp.Length);   //8 * 1024 * 1024
                                                    PacketSize = 2048;                          //1024,2048,4096,8192,16384
                                                    Count = FlashSize / PacketSize;

                                                    //byte svsends = 1;
                                                    //byte svpos = 3;
                                                    //byte svcas = 2;
                                                    lstget1.Items.Add(" -> ");
                                                    svcounts = lstget1.Items.Count - 1;
                                                    byte svrW = 0;
                                                    for (i = 0; i < Count; i++)
                                                    {
                                                        svrW = 0;
                                                        string txt36 = (i * PacketSize).ToString("X8");
                                                    reWr:
                                                        mvars.nvBoardcast = true;
                                                        mvars.lblCmd = "FLASH_WRITE_" + i.ToString("0000");

                                                        if (i % 16 == 0)
                                                        {
                                                            mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 500);
                                                            lstget1.Items.RemoveAt(svcounts);
                                                            lstget1.Items.Insert(svcounts, " -> " + txt36 + " @ " + String.Format("{0:00}", ((i * 100) / Count) + "% -- ") + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                            lstget1.TopIndex = lstget1.Items.Count - 1;
                                                        }
                                                        else { mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 30); }

                                                        mvars.nvBoardcast = false;
                                                        for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                                                        {
                                                            if (svundo[svi] == false)
                                                            {
                                                                mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                                                                mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                                                                mvars.iScan = (byte)Form1.hwCard[svi].iScan;
                                                                byte[] data = null;
                                                                int readLen = 28;
                                                                int svct = 0;
                                                            reRd:
                                                                bool res = mvars._marsCtrlSystem.GetDataInnolux(mvars.iSender, mvars.iPort, mvars.iScan, readLen, out data);
                                                                if (res)
                                                                {
                                                                    string strOutput = "";
                                                                    for (int svn = 0; svn < data.Length; svn++)
                                                                    {
                                                                        if (svn != 0 && svn % 16 == 0)
                                                                        {
                                                                            strOutput += "\r\n";
                                                                        }
                                                                        strOutput += data[svn].ToString("X2") + " ";
                                                                    }
                                                                    //ResetRichTextBox(strOutput);
                                                                    if (data[6] != 3)
                                                                    {
                                                                        if (svct == 0)
                                                                        {
                                                                            svct++;
                                                                            goto reRd;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (svrW < 3)
                                                                            {
                                                                                int m = (int)i;
                                                                                i = (m / 16) * 16;
                                                                                svrW++;
                                                                                goto reWr;
                                                                            }
                                                                            else { svundo[svi] = true; svundos++; }
                                                                        }
                                                                    }
                                                                    else { svct = 0; }
                                                                }
                                                                else
                                                                {
                                                                    uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svi].iSender + 1 + "P" + Form1.hwCard[svi].iPort + 1 + "C" + Form1.hwCard[svi].iScan + 1 + ",res,false,rW" + svrW);
                                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                    if (svrW < 3)
                                                                    {
                                                                        int m = (int)i;
                                                                        i = (m / 16) * 16;
                                                                        svrW++;
                                                                        goto reWr;
                                                                    }
                                                                    else { svundo[svi] = true; svundos++; }
                                                                }
                                                            }
                                                        }
                                                        if (mvars.Break) { mvars.errCode = "-11"; break; }
                                                        if (svundos >= Form1.NovaStarDeviceResult.Count) { mvars.errCode = "-12"; break; }
                                                    }
                                                    if (mvars.Break) { mvars.Break = false; }
                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        lstget1.Items.RemoveAt(svcounts);
                                                        lstget1.Items.Insert(svcounts, " -> " + mvars.strFLASHtype[mvars.flashselQ - 1] +
                                                        " write  @ 100% - - - - " + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                    }
                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                }
                                            }
                                            #endregion Bin Write
                                        }
                                        else if (btn.Tag.ToString() == "F")
                                        {
                                            #region Bin Write
                                            //if (mvars.flgForceUpdate) { Array.Clear(svundo, 0, svundo.Length); }
                                            //if (svundos < Form1.NovaStarDeviceResult.Count || mvars.flgForceUpdate)
                                            if (svundos < Form1.NovaStarDeviceResult.Count)
                                            {
                                                lstget1.Items.RemoveAt(0);
                                                lstget1.Items.Add("FPGA update");
                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                mvars.nvBoardcast = true;       //廣播

                                                if (svEraseNwrNchk == 0x42)
                                                {
                                                    Form1.pvindex = 1;
                                                    mvars.lblCmd = "FPGA_SPI_W";
                                                    mp.mhFPGASPIWRITE(3);      // 01 SI_SEL PG mode
                                                    Form1.pvindex = 255;
                                                    mvars.lblCmd = "FPGA_SPI_W255";
                                                    mp.mhFPGASPIWRITE(0);
                                                    mp.mhFPGASPIWRITE(1);
                                                    mp.mhFPGASPIWRITE(0);
                                                }

                                                mvars.lblCmd = "FLASH_TYPE";
                                                mp.mhFLASHTYPE();
                                                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-3"; goto Ex; }
                                                mvars.lblCmd = "FLASH_FUNCQE";
                                                mp.mhFUNCQE();
                                                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-4"; goto Ex; }
                                                mvars.lblCmd = "FUNC_ENABLE";
                                                mp.mhFUNCENABLE();
                                                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-4.5"; goto Ex; }
                                                mvars.lblCmd = "FUNC_STATUS";
                                                mp.mhFUNCSTATUS();

                                                mvars.nvBoardcast = false;

                                                for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                                                {
                                                    if (svundo[svi] == false)
                                                    {
                                                        mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                                                        mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                                                        mvars.iScan = (byte)Form1.hwCard[svi].iScan;

                                                        mp.funSendMessageTo(true);
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svi] = true; }
                                                        else
                                                        {
                                                            if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                            {
                                                                if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                svundo[svi] = true;
                                                            }
                                                        }
                                                        if (svundo[svi]) svundos++;
                                                    }
                                                }

                                                if (svundos < Form1.NovaStarDeviceResult.Count)
                                                {
                                                    //寫入使用Boardcast
                                                    //mvars.nvBoardcast = svNVBC;

                                                    //mvars.lblCmd = "HW_RESET_FPGA"; 
                                                    //mp.mhSWRESET(SvNova, 0x80);
                                                    //Form1.lst_get1.Items.Add(" --> Wait 5s"); Form1.lst_get1.TopIndex = Form1.lst_get1.Items.Count - 1;
                                                    //mp.doDelaySec(5);                   
                                                    sw1.Reset();
                                                    sw1.Start();
                                                    if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                    {
                                                        byte[] Tmp = new byte[8 * 1024 * 1024];
                                                        Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                        Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                        Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                    }
                                                    FlashSize = (UInt32)(mvars.ucTmp.Length);   //8 * 1024 * 1024
                                                    PacketSize = 2048;                          //1024,2048,4096,8192,16384
                                                    Count = FlashSize / PacketSize;

                                                    //byte svsends = 1;
                                                    //byte svpos = 3;
                                                    //byte svcas = 2;
                                                    lstget1.Items.Add(" -> ");
                                                    svcounts = lstget1.Items.Count - 1;
                                                    byte svrW = 0;
                                                    for (i = 0; i < Count; i++)
                                                    {
                                                        svrW = 0;
                                                        string txt36 = (i * PacketSize).ToString("X8");
                                                    reWr:
                                                        mvars.nvBoardcast = true;
                                                        mvars.lblCmd = "FLASH_WRITE_" + i.ToString("0000");

                                                        if (i % 16 == 0)
                                                        {
                                                            mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 500);
                                                            lstget1.Items.RemoveAt(svcounts);
                                                            lstget1.Items.Insert(svcounts, " -> " + txt36 + " @ " + String.Format("{0:00}", ((i * 100) / Count) + "% -- ") + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                            lstget1.TopIndex = lstget1.Items.Count - 1;
                                                        }
                                                        else { mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 30); }

                                                        mvars.nvBoardcast = false;
                                                        for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                                                        {
                                                            if (svundo[svi] == false)
                                                            {
                                                                mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                                                                mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                                                                mvars.iScan = (byte)Form1.hwCard[svi].iScan;
                                                                byte[] data = null;
                                                                int readLen = 28;
                                                                int svct = 0;
                                                            reRd:
                                                                bool res = mvars._marsCtrlSystem.GetDataInnolux(mvars.iSender, mvars.iPort, mvars.iScan, readLen, out data);
                                                                if (res)
                                                                {
                                                                    string strOutput = "";
                                                                    for (int svn = 0; svn < data.Length; svn++)
                                                                    {
                                                                        if (svn != 0 && svn % 16 == 0)
                                                                        {
                                                                            strOutput += "\r\n";
                                                                        }
                                                                        strOutput += data[svn].ToString("X2") + " ";
                                                                    }
                                                                    //ResetRichTextBox(strOutput);
                                                                    if (data[6] != 3)
                                                                    {
                                                                        if (svct == 0)
                                                                        {
                                                                            svct++;
                                                                            goto reRd;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (svrW < 3)
                                                                            {
                                                                                int m = (int)i;
                                                                                i = (m / 16) * 16;
                                                                                svrW++;
                                                                                goto reWr;
                                                                            }
                                                                            else svundo[svi] = true;
                                                                        }
                                                                    }
                                                                    else { svct = 0; }
                                                                }
                                                                else
                                                                {
                                                                    uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svi].iSender + 1 + "P" + Form1.hwCard[svi].iPort + 1 + "C" + Form1.hwCard[svi].iScan + 1 + ",res,false,rW" + svrW);
                                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                    if (svrW < 3)
                                                                    {
                                                                        int m = (int)i;
                                                                        i = (m / 16) * 16;
                                                                        svrW++;
                                                                        goto reWr;
                                                                    }
                                                                    else svundo[svi] = true;
                                                                }
                                                            }
                                                        }
                                                        if (mvars.Break) { mvars.errCode = "-11"; break; }
                                                    }
                                                    if (mvars.Break) { mvars.Break = false; }
                                                    if (mvars.errCode == "0")
                                                    {
                                                        lstget1.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ - 1] +
                                                        " write  @ 100% - - - - " + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                    }
                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                }
                                            }
                                            #endregion Bin Write
                                        }


                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            if (svundo[svDevCnt])
                                            {
                                                svundos++;
                                                lstget1.Items.Add("Ctrlsys" + svCtrlSysCnt +
                                                        "S" + (Form1.NovaStarDeviceResult[svDevCnt].SenderIndex + 1) +
                                                        "P" + (Form1.NovaStarDeviceResult[svDevCnt].PortIndex + 1) +
                                                        "C" + (Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex + 1) + ",UnDo");
                                            }
                                            else
                                            {
                                                lstget1.Items.Add("Ctrlsys" + svCtrlSysCnt +
                                                        "S" + (Form1.NovaStarDeviceResult[svDevCnt].SenderIndex + 1) +
                                                        "P" + (Form1.NovaStarDeviceResult[svDevCnt].PortIndex + 1) +
                                                        "C" + (Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex + 1) + ",Stanby");
                                            }
                                        }

                                        lstget1.Items.Add("");
                                        lstget1.Items.Add(mvars.nvCommList[svCtrlSysCnt] + " is " + svundos + " of " + Form1.NovaStarDeviceResult.Count + " UnDo");
                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                    }
                                    else
                                    {
                                        mp.funSaveLogs("(Err) Read NovaStar hardware...fail，No NovaStar Receiver");
                                        #region 訊息顯示  錯誤，沒有接收卡硬體
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            lstget1.Items.Add("Read Receiving Card...fail，no NovaStar hardware");
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            lstget1.Items.Add("錯誤，沒有接收卡硬體");
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            lstget1.Items.Add("错误，没有接收卡硬件");
                                        }
                                        goto Ex;
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                    else { mvars.errCode = "-1"; }
                }
                else { mvars.errCode = "-2"; }
                #endregion NovaStar
            }
            else
            {
                #region USB



                #endregion USB
            }


        Ex:
            mvars.flgForceUpdate = false; btn_nFPGAW.ForeColor = Color.Black;
            //if (mvars.errCode == "0" && svundos < Form1.NovaStarDeviceResult.Count)
            if (svundos < Form1.NovaStarDeviceResult.Count)
            {
                mvars.nvBoardcast = true;       //廣播
                lstget1.Items.Add(" -> FPGA HW_RESET after Write"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mvars.lblCmd = "HW_RESET_FPGA";
                mp.mhFPGARESET(0x80);
                lstget1.Items.Add(" --> Wait 3s"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mp.doDelayms(3000);

                byte svflashQ = mvars.flashselQ;
                mvars.flashselQ = 0;
                mvars.lblCmd = "FLASH_TYPE";
                mp.mhFLASHTYPE();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    lstget1.Items.Add(" -> " + mvars.strFLASHtype[svflashQ - 1] + " write OK but FLASH_TYPE switch to \"OPEN\" fail");
                    mvars.flashselQ = svflashQ;
                    mvars.errCode = "-14";
                }
                mp.doDelayms(1000);

                Form1.pvindex = 1;
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                Form1.pvindex = 255;
                mvars.lblCmd = "FPGA_SPI_W255";
                mp.mhFPGASPIWRITE(0);
                mp.mhFPGASPIWRITE(1);
                mp.mhFPGASPIWRITE(0);

                mvars.lblCmd = "HW_RESET_FPGA";
                mp.mhFPGARESET(0x80);
                lstget1.Items.Add(" --> Wait 3s"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mp.doDelayms(3000);

                #region FPGA version
                if (btn.Tag.ToString() == "F")
                {
                    mvars.nvBoardcast = false;       //非廣播
                    mvars.isReadBack = true;
                    for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                    {
                        mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                        mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                        mvars.iScan = (byte)Form1.hwCard[svi].iScan;

                        Form1.pvindex = 0;
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]); mvars.errCode = "-2"; }
                        Form1.hwCard[svi].verFPGA = mvars.verFPGA;
                        Form1.pvindex = 254;
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]); mvars.errCode = "-2"; }
                        svpcbaver[svi] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                        uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svi].iSender + 1 + "P" + Form1.hwCard[svi].iPort + 1 + "C" + Form1.hwCard[svi].iScan + 1 + ",PCBAver," + svpcbaver[svi] + ",FPGAver," + mvars.verFPGA); uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                        mvars.lblCmd = "PG_ASCTEXT";
                        mp.mPGASCTEXT(5, 20, "PCBA" + svpcbaver[svi] + " FPGA" + mvars.verFPGA, 1);
                    }
                }
                #endregion FPGA version

                //uc_coding.lstget1.Items.Add(" --> Done and Compare OK");
                uc_coding.lstget1.Items.Add(" --> " + svundos + " of " + Form1.NovaStarDeviceResult.Count + " undo");
            }
            else { uc_coding.lstget1.Items.Add(" --> Error code:" + mvars.errCode); }
            uc_coding.lstget1.Items.Add("End");
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (openFileDialog1 = new OpenFileDialog())
            {
                //openFileDialog1.FileName = "開啟檔案"; 預設開啟的檔案名
                openFileDialog1.Title = "開啟 bin 檔";
                openFileDialog1.Filter = "bin files (*.bin)|*.bin";//|All files (*.*)|*.*"; 去掉;//就可以跟All files結合
                //openFileDialog1.FilterIndex = 2; 兩種檔案過濾再改2 .bin & *.* 預設值1
                openFileDialog1.RestoreDirectory = true;
                //openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
                string[] Svstr;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    int svopc = 0;
                    System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
                    sw1.Reset();
                    sw1.Start();
                    do
                    {
                        txt_FlashFileNameFull.Text = openFileDialog1.FileName;
                        Svstr = txt_FlashFileNameFull.Text.Split('\\');
                        if (Svstr.Length > 0)
                        {
                            if (tabControl1.SelectedTab == tabpage_dmr) { txt_DMRFileName.Text = Svstr[Svstr.Length - 1]; }
                            else { txt_FlashFileName.Text = Svstr[Svstr.Length - 1]; }
                            if (mp.GetBin(txt_FlashFileNameFull.Text) == true)
                            {
                                int checksum = (int)mp.CalCheckSum(mvars.ucTmp, 0, mvars.ucTmp.Length);

                                txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                                if (tabControl1.SelectedTab == tabpage_dmr)
                                {
                                    btn_nDMRW.Enabled = true;
                                    lbl_nDMRChecksum.Text = mvars.ucTmp.Length / 1024 + "Kbytes   checksum：0x" + checksum.ToString("X4");
                                }
                                else
                                {
                                    btn_nFPGAW.Enabled = true;
                                    txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                                    string[] svs = txt_FlashFileName.Text.Split('_');
                                    for (int svi = 0; svi < svs.Length; svi++)
                                    {
                                        if (svs[svi].ToUpper().Substring(0, 1) == "V")
                                        {
                                            string txtverfbin = svs[svi].Substring(1, svs[svi].Length - 1);
                                            txt_verFbin.Text = txtverfbin.Replace(".", string.Empty);
                                            int count = 0;
                                            foreach (char c in txtverfbin)
                                            {
                                                if (c == '.')
                                                {
                                                    count++;
                                                }
                                            }
                                            if (count == 2) { txt_verFbin.Enabled = false; break; } else { txt_verFbin.Text = ""; txt_verFbin.Enabled = true; }
                                        }
                                    }
                                    lbl_nBinChecksum.Text = mvars.ucTmp.Length / 1024 + "Kbytes   checksum：0x" + checksum.ToString("X4");
                                }
                            }
                            else
                            {
                                if (tabControl1.SelectedTab == tabpage_dmr) { btn_nDMRW.Enabled = false; txt_DMRFileName.Text = " *.bin (double click here to select bin file)"; lbl_nDMRChecksum.Text = "checksum"; }
                                else { btn_nFPGAW.Enabled = false; txt_FlashFileName.Text = " *.bin (double click here to select bin file)"; lbl_nBinChecksum.Text = "checksum"; }
                            }
                        }
                        else
                        {
                            if (tabControl1.SelectedTab == tabpage_dmr) { btn_nDMRW.Enabled = false; txt_DMRFileName.Text = " *.bin (double click here to select bin file)"; lbl_nDMRChecksum.Text = "checksum"; }
                            else { btn_nFPGAW.Enabled = false; txt_FlashFileName.Text = " *.bin (double click here to select bin file)"; lbl_nBinChecksum.Text = "checksum"; }
                        }
                        svopc++;
                    }
                    while (svopc < 4096);
                    lstget1.Items.Add("Read version " + Convert.ToString(string.Format("{0:0.#}", sw1.Elapsed.TotalSeconds)) + "s");
                }
            }
        }


        private string NunToChar(int number)
        {
            if (65 <= number && 90 >= number)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] btNumber = new byte[] { (byte)number };
                return asciiEncoding.GetString(btNumber);
            }
            return "數字不在轉換范圍內";
        }


        int svColIndex;
        int svRowIndex;
        System.Drawing.Rectangle r1;
        private void Skeleton_nBox(int SvCols, int SvRows, int SvLBw, int SvLBh)
        {
            dgvbox.Paint -= new PaintEventHandler(dgvNS_RowPostPaint);
            dgvbox.Paint -= new PaintEventHandler(dgvNS_CornerPaint);
            //dgvbox.Paint -= new PaintEventHandler(dgvbox_CellPaint);
            dgvbox.Columns.Clear(); dgvbox.Rows.Clear();
            int SvR = 0;
            int SvC = 0;
            for (SvC = 0; SvC < SvCols; SvC++)
            {
                dgvbox.Columns.Add("Col" + (SvC).ToString(), NunToChar(65 + SvC)); dgvbox.Columns[(SvC)].Width = 115; dgvbox.Columns[(SvC)].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvbox.ColumnHeadersHeight = 20;
            }
            //dgvbox.Visible = true;
            dgvbox.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 8);
            dgvbox.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvbox.ShowCellToolTips = false;
            DataGridViewRowCollection rows = dgvbox.Rows;
            for (SvR = 0; SvR < SvRows; SvR++)
            {
                rows.Add();
                dgvbox.Rows[SvR].DefaultCellStyle.WrapMode = DataGridViewTriState.True; DataGridViewRow row = dgvbox.Rows[SvR]; row.Height = 23;
                for (SvC = 0; SvC < SvCols; SvC++)
                {
                    dgvbox.Rows[SvR].Cells[(SvC)].Style.BackColor = System.Drawing.Color.FromArgb(255, 192, 128);
                }
            }
            dgvbox.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvbox.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvbox.RowHeadersWidth = 45;
            dgvbox.RowHeadersDefaultCellStyle.Padding = new Padding(50);
            dgvbox.Paint += new PaintEventHandler(dgvNS_RowPostPaint);
            dgvbox.Paint += new PaintEventHandler(dgvNS_CornerPaint);
            //dgvbox.Paint += new PaintEventHandler(dgvbox_CellPaint);

            dgvBtn.Location = new System.Drawing.Point(dgv_box.Left + 2, dgvbox.Top + 2);
            dgvBtn.Height = dgvbox.ColumnHeadersHeight - 1;
            dgvBtn.Width = dgvbox.RowHeadersWidth - 1;
            dgvbox.Font = new System.Drawing.Font("細明體", 9);

            r1 = dgvbox.GetCellDisplayRectangle(0, 0, true);
        }
        void dgvNS_RowPostPaint(object sender, PaintEventArgs e)
        {
            System.Drawing.Rectangle r1_1;
            System.Drawing.Rectangle r1;
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            for (int j = 0; j < dgvbox.Rows.Count; j++)
            {
                r1 = dgvbox.GetCellDisplayRectangle(-1, j, false); //get the column header cell
                r1.X += 1;
                r1.Y += 1;
                r1_1 = r1;
                r1_1.Width -= 2;
                r1_1.Height -= 2;
                e.Graphics.FillRectangle(new System.Drawing.SolidBrush(dgvbox.ColumnHeadersDefaultCellStyle.BackColor), r1_1);
                format.Alignment = System.Drawing.StringAlignment.Center;
                format.LineAlignment = System.Drawing.StringAlignment.Center;
                e.Graphics.DrawString((j + 1).ToString("0"),
                dgvbox.ColumnHeadersDefaultCellStyle.Font, new System.Drawing.SolidBrush(dgvbox.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
        }
        private void dgvNS_CornerPaint(object sender, PaintEventArgs e)
        {
            System.Drawing.Rectangle r1;
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            r1 = dgvbox.GetCellDisplayRectangle(-1, -1, true);
            //r1.X += 1;
            r1.Y += 1;
            r1.Width = r1.Width - 1;
            r1.Height = r1.Height - 2;
            e.Graphics.FillRectangle(new System.Drawing.SolidBrush(DefaultBackColor), r1);
            format = new System.Drawing.StringFormat();
            format.Alignment = System.Drawing.StringAlignment.Center;
            format.LineAlignment = System.Drawing.StringAlignment.Center;
            e.Graphics.DrawString("", new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold), new System.Drawing.SolidBrush(System.Drawing.Color.White), r1, format);
        }

        private void dgv_box_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (cmb_box.Visible && cmb_box.Text != "")
                {

                }

                svColIndex = e.ColumnIndex;
                svRowIndex = e.RowIndex;
                r1 = dgvbox.GetCellDisplayRectangle(svColIndex, svRowIndex, true);
                //txt_box.SetBounds(dgvbox.Left + r1.X, dgvbox.Top + r1.Y, r1.Width, r1.Height);
                //txt_box.Visible = true;
                //txt_box.BringToFront();
                //txt_box.Focus();
                cmb_box.SetBounds(dgvbox.Left + r1.X, dgvbox.Top + r1.Y, r1.Width, r1.Height);
                cmb_box.Text = "";
                cmb_box.Visible = true;
                cmb_box.BringToFront();
                cmb_box.Focus();
            }
        }



        private void cmb_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                cmb_box.Visible = false;
            }
            else if (e.KeyCode == Keys.Return)
            {
                dgvbox.Rows[svRowIndex].Cells[svColIndex].Value = cmb_box.Text;
                cmb_box.Visible = false;

            }
        }

        private void lst_get1_MouseClick(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabpage_dmr")
            {
                //string[] svs = lst_filepathfull.Items[lstget1.SelectedIndex].ToString().Split('\\');
                //string svs1 = "";
                //for (byte i = 0; i < svs.Length - 1; i++)
                //{
                //    svs1 += "\\" + svs[i];
                //}
                //svs1 = svs1.Substring(1, svs1.Length - 1);
                //txt_filepathfull.Text = svs1 + "\\";
                if (lstget1.Items.Count > 0) txt_filepathfull.Text = lst_filepathfull.Items[lstget1.SelectedIndex].ToString();
            }
        }

        private void cmb_LostFocus(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;

            cmb.Visible = false;
        }




        private void btn_nBMP_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch();

            swtotal.Reset();
            swtotal.Start();

            if ((txt_FlashFileName.Text.Trim().ToUpper().IndexOf("*.BIN") == -1 && txt_FlashFileName.Text.Length > 0) ||
                (txt_DMRFileName.Text.Trim().ToUpper().IndexOf("*.BIN") == -1 && txt_DMRFileName.Text.Length > 0))
            {
                txtFlashFileNameFull.Text = openFileDialog1.FileName;
                string[] Svstr = txtFlashFileNameFull.Text.Split('\\');
                if (Svstr.Length == 0) { MessageBox.Show("Bin file length = " + Svstr.Length, mvars.strUInameMe + mvars.UImajor); return; }

                if (mvars.deviceID.Substring(0, 2) == "05")
                {
                    if (cmbFlashSel.Text.ToUpper().IndexOf("FPGA", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("DEMURA", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("XB_", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("CB_", 0) != -1 || cmbFlashSel.Text.ToUpper().IndexOf("LB_", 0) != -1)
                    {
                        if (cmbFlashSel.Text.ToUpper().IndexOf("CB_", 0) != -1)
                        {
                            Byte[] Barcode = new byte[32];
                            mp.Copy(mvars.ucTmp, 0x0003FF5D, Barcode, 0, Barcode.Length);
                            lbl_barcode.Text = cmbFlashSel.Text.Trim() + " Barcode：" + System.Text.Encoding.ASCII.GetString(Barcode);
                        }
                        //mvars.lstget.Items.Add(cmbFlashSel.Text + " Flash Write");
                        //lbl_fileofflashread.Text = @"...\Parameter\*Read.bin";

                        if ((tabControl1.SelectedTab == tabpage_bmp && txtFlashFileName.Text.IndexOf(cmb_FlashSel.Text.Trim(), 0) != -1) ||
                            (tabControl1.SelectedTab == tabpage_dmr && txt_DMRFileName.Text.IndexOf(cmb_FlashSel.Text.Trim(), 0) != -1))
                            mp.cFLASHWRITE_pCB(cmbFlashSel.Text, lst_get1);
                        else
                        {
                            if (cmbFlashSel.Text.IndexOf("FPGA", 0) != -1)
                            {
                                if (btn.BackColor == System.Drawing.Color.FromArgb(128, 255, 128) &&
                                txtFlashFileName.Text.ToLower().IndexOf(cmb_FlashSel.Text.Trim().ToLower().Substring(4, 2), 0) == -1)
                                {
                                    string value = "";
                                    string prompt = " 檔名與燒錄項目不符";
                                    if (MultiLanguage.DefaultLanguage == "en-US") prompt = " Please check Filename and Flashitem";
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN") prompt = " 档名与烧录项目不符";
                                    else if (MultiLanguage.DefaultLanguage == "ja-JP") prompt = " Please check Filename and Flashitem";
                                    mp.InputBox(mvars.strUInameMe, prompt, ref value, 0, 0, 200, 50, 1, "");
                                }
                                else if (btn.BackColor == System.Drawing.Color.FromArgb(128, 255, 128) &&
                                        txtFlashFileName.Text.ToLower().IndexOf(cmb_FlashSel.Text.Trim().ToLower().Substring(4, 2), 0) != -1)

                                {
                                    uc_coding.lstget1.Items.Clear();
                                    mp.cFLASHWRITE_pCB(cmbFlashSel.Text, lst_get1);
                                }
                            }
                            else
                            {
                                if (btn.BackColor == System.Drawing.Color.FromArgb(128, 255, 128) &&
                                txt_DMRFileName.Text.IndexOf(cmb_FlashSel.Text.Trim(), 0) == -1)
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
                        uc_coding.lstget1.Items.Add("End " + Convert.ToString(string.Format("{0:####}", swtotal.Elapsed.TotalSeconds)) + "sec");
                    }
                    else
                    {
                        uc_coding.lstget1.Items.Clear();
                        if (MultiLanguage.DefaultLanguage=="en-US")
                            uc_coding.lstget1.Items.Add("Please check FlashSel !!");
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            uc_coding.lstget1.Items.Add("請確認閃存記憶體種類 !!");
                        else if (MultiLanguage.DefaultLanguage == "zh-CN" || MultiLanguage.DefaultLanguage == "ja-JP")
                            uc_coding.lstget1.Items.Add("请确认闪存记忆体选项 !!");
                    }
                }
                else
                {
                    #region C12A/B
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
                    Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = System.Drawing.Color.Blue; Form1.tslblHW.ForeColor = System.Drawing.Color.White;

                    if (mvars.c12aflashitem == 0 && mvars.flashselQ == 0) { MessageBox.Show("FlashType @ OPEN，Please Select \"FPGA\" or \"DEMURA\"", "Flash"); return; }
                    string svs = "";
                    if (mvars.c12aflashitem == 0)
                    {
                        svs = cmbFlashSel.Text.Trim().Substring(2, cmbFlashSel.Text.Trim().Length - 2) + " FlashWrite,";
                        //mvars.lstget.Items.Add(svs + " ....");
                        mp.cFLASHWRITE_C12ACB();
                    }
                    //else if (mvars.c12aflashitem == 1)
                    //{
                    //    svs = "C12A XBoard " + cmbhPB.Text + " FlashWrite,";
                    //    mvars.lstget.Items.Add(svs + " ....");
                    //    //mp.cFLASHWRITE_C12AXB(false);
                    //}
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
                    #endregion
                }
            }








            
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            sw1.Stop();
            swtotal.Stop();
            btn.Enabled = true;
        }

        private void btn_BoxRESETn_Click(object sender, EventArgs e)
        {
            Skeleton_nBox((int)numUD_boxCols.Value, (int)numUD_boxRows.Value, 0, 0);
        }

        private void btn_nDMRbmp_Click(object sender, EventArgs e)
        {
            /*
            Button btn = (Button)sender;
            bool svbmpW = false;
            if (btn.BackColor == Color.FromArgb(255, 255, 128)) { svbmpW = true; }
            int svDev = 0;

            #region config
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch();
            ushort svundos = 0;
            bool[] svundo = null;
            string txtverfbin = "";
            byte svEraseNwrNchk = 0x3F;
            //Write only mvars.c12aflashitem = 0(CB)-- > 0x3F and mvars.c12aflashitem = 1(XB)-- > 0x39
            //Erase and Write and Check mvars.c12aflashitem = 0(CB)-- >  = 0x42
            svEraseNwrNchk = 0x42;

            string[] svpcbaver = null;
            mvars.lCounts = 19999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            int i = 0;
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;
            //short sverrc = 0;
            //string txt44;
            UInt32 FlashSize;
            ushort PacketSize;
            UInt32 Count;
            lstget1.Items.Clear();
            mvars.flgSelf = true;
            if (tabControl1.SelectedTab.Name == "tabpage_bmp") mvars.flashselQ = 1;         //FPGA
            else if (tabControl1.SelectedTab.Name == "tabpage_dmr") mvars.flashselQ = 2;    //Demura
            int svcounts;
            int svlstc = 0;
            #endregion config

            mvars.c12aflashitem = 0;    //CB

            lstget1.Items.Clear();

            swtotal.Reset();
            swtotal.Start();

            ushort svsen = (ushort)numericUpDown_sender.Value;
            ushort svpo = (ushort)numericUpDown_port.Value;
            ushort svca = (ushort)numericUpDown_connect.Value;

            #region FPGA
            if (tabControl1.SelectedTab.Name == "tabpage_bmp")
            {
                txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                string[] svs = txt_nFlashFileName.Text.Split('_');
                for (int svi = 0; svi < svs.Length; svi++)
                {
                    if (svs[svi].ToUpper().Substring(0, 1) == "V")
                    {
                        txtverfbin = svs[svi].Substring(1, svs[svi].Length - 1);
                        txt_verFbin.Text = txtverfbin.Replace(".", string.Empty);
                        int count = 0;
                        foreach (char c in txtverfbin)
                        {
                            if (c == '.')
                            {
                                count++;
                            }
                        }
                        if (count == 2) { txt_verFbin.Enabled = false; break; } else { txt_verFbin.Text = ""; txt_verFbin.Enabled = true; }
                    }
                }                
            }
            #endregion FPGA

            if (mvars.svnova)
            {
                #region NovaStar
                if (Form1.lstm.Items.Count != 0)
                {
                    int svbxW = 480;
                    int svbxH = 270;
                    int svfpgaW = (svbxW * 3 / 1024) * 1024;
                    int svfpgaH = (svbxH / 256) * 256;

                    bool svchk = false;
                    ushort svctrlsys = 0;
                    for (int svlc = 0; svlc < Form1.lstm.Items.Count; svlc++)
                    {
                        if (Form1.lstm.Items[svlc].ToString().IndexOf('↑') != -1)
                        {
                            ushort svc = Convert.ToUInt16(Form1.lstm.Items[svlc].ToString().Split(',')[3]);
                            if (svsen - svc <= 0)
                            {
                                svchk = true;
                                svctrlsys = Convert.ToUInt16(Form1.lstm.Items[svlc].ToString().Split(',')[1]);
                                //排頭訊息
                                Form1.svhs = Form1.lstm.Items[svlc].ToString().Split(',')[6];
                                Form1.svhp = Form1.lstm.Items[svlc].ToString().Split(',')[7];
                                Form1.svhc = Form1.lstm.Items[svlc].ToString().Split(',')[8];
                            }
                            else { svsen -= svc; }
                        }
                        //因應網絡型發送卡無法廣播, 所以需要透過 lstm 來找出是哪一個 IP(發送卡), 如果每一個 IP 下都有 2 台發送卡
                        //  ................S0,P,,,,
                        //  ................S1,p,,,,
                        // " ↑ " + svip + "," + mvars.nvCommList[svip] + ",Sender accumulation,2,...
                        //  ................S0,P,,,,
                        //  ................S1,p,,,,
                        // " ↑ " + svip + "," + mvars.nvCommList[svip] + ",Sender accumulation,2,...
                        // Sender 都是重複性的從 0 開始起算, 所以輸入的 svsen 要越過不同的 CtrlSysCnt 的時候就必須先減掉發送卡數量 --> svsen -= svc
                    }
                    if (svchk)
                    {
                        mvars.iSender = Convert.ToByte(Form1.svhs);
                        mvars.iPort = Convert.ToByte(Form1.svhp);
                        mvars.iScan = Convert.ToUInt16(Form1.svhc);
                        if (mvars.nvBoardcast == false)
                        {
                            svchk = false;
                            mvars.iSender = (byte)(svsen - 1);
                            mvars.iPort = (byte)(svpo - 1);
                            mvars.iScan = (ushort)(svca - 1);
                            int svlc = 0;
                            for (svlc = 0; svlc < Form1.lstm.Items.Count; svlc++)
                            {
                                if (Form1.lstm.Items[svlc].ToString().IndexOf("S" + mvars.iSender + ",P" + mvars.iPort + ",C" + mvars.iScan, 0) != -1) { svchk = true; break; }
                            }
                        }
                    }
                    if (svchk)
                    {
                        //只執行一個通訊埠或是單一個IP
                        for (byte svCtrlSysCnt = (byte)svctrlsys; svCtrlSysCnt <= (byte)svctrlsys; svCtrlSysCnt++)
                        {
                            if (mvars._marsCtrlSystem.UnInitialize())
                            {
                                mp.funSaveLogs(tabControl1.SelectedTab.Name + ",Unloaded marsCtrlSystem.UnInitialize");
                            }

                            mvars._nCommPort = mvars.nvCommList[svCtrlSysCnt];
                            Form1.nScreenCnt = 0;
                            Form1.nSenderCnt = 0;
                            if (mvars._marsCtrlSystem.Initialize(mvars._nCommPort, out Form1.nScreenCnt, out Form1.nSenderCnt) == false)
                            {
                                mvars.errCode = "-1";
                                mp.funSaveLogs(tabControl1.SelectedTab.Name + "," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",marsCtrlSystem.Initialize fail");
                            }
                            else
                            {
                                Form1.NovaStarDeviceResult = null;
                                svcounts = 0;
                                do
                                {
                                    Form1.NovaStarDeviceResult = mvars._marsCtrlSystem.GetAllScannerStatusByCom();
                                    mp.doDelayms(100);
                                    svcounts += 1;
                                    if (Form1.NovaStarDeviceResult != null) break;
                                }
                                while (svcounts <= 10);
                                if (Form1.NovaStarDeviceResult != null && svcounts <= 10)
                                {
                                    if (Form1.NovaStarDeviceResult.Count > 0)
                                    {
                                        Array.Resize(ref Form1.hwCard, Form1.NovaStarDeviceResult.Count);
                                        Array.Resize(ref svundo, Form1.NovaStarDeviceResult.Count);
                                        Array.Resize(ref svpcbaver, Form1.NovaStarDeviceResult.Count);
                                        if (mvars.flgSelf == false) { lstget1.Items.Clear(); }
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)  
                                        {
                                            Form1.hwCard[svDevCnt].iSender = Form1.NovaStarDeviceResult[svDevCnt].SenderIndex;
                                            Form1.hwCard[svDevCnt].iPort = Form1.NovaStarDeviceResult[svDevCnt].PortIndex;
                                            Form1.hwCard[svDevCnt].iScan = Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex;
                                            svundo[svDevCnt] = false;
                                        }
                                        Form1.hwCards = Form1.hwCard.Length;
                                        mvars.lCounts = 9999;
                                        Array.Resize(ref mvars.lCmd, mvars.lCounts);
                                        Array.Resize(ref mvars.lGet, mvars.lCounts);
                                        mvars.lCount = 0;
                                        mvars.strReceive = "";
                                        mvars.flgDelFB = true;
                                        mvars.flgSelf = true;
                                        mvars.nvBoardcast = false;
                                        mvars.isReadBack = true;

                                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                                        sw.Reset();
                                        sw.Start();
                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            mvars.iSender = (byte)Form1.NovaStarDeviceResult[svDevCnt].SenderIndex;
                                            mvars.iPort = (byte)Form1.NovaStarDeviceResult[svDevCnt].PortIndex;
                                            mvars.iScan = (byte)Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex;

                                            Form1.pvindex = 0;
                                            mvars.lblCmd = "FPGA_SPI_R";
                                            mp.mhFPGASPIREAD();
                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                            Form1.hwCard[svDevCnt].verFPGA = mvars.verFPGA;
                                            Form1.pvindex = 254;
                                            mvars.lblCmd = "FPGA_SPI_R";
                                            mp.mhFPGASPIREAD();
                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                            else svpcbaver[svDevCnt] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];

                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                            if (svpcbaver[svDevCnt] != "1" && svpcbaver[svDevCnt] != "2") { svundo[svDevCnt] = true; }

                                            if (mvars.flashselQ == 1 && txt_verFbin.Text != "" && mvars.verFPGA != "127.31.15")
                                            {
                                                if (Convert.ToInt16(txtverfbin.Split('.')[0]) < (Convert.ToInt16(mvars.verFPGA.Split('.')[0]))) { if (svundo[svDevCnt] == false) svundo[svDevCnt] = !mvars.flgForceUpdate; }
                                                else
                                                {
                                                    if (Convert.ToInt16(txtverfbin.Split('.')[0]) == (Convert.ToInt16(mvars.verFPGA.Split('.')[0])) && Convert.ToInt16(txtverfbin.Split('.')[1]) < (Convert.ToInt16(mvars.verFPGA.Split('.')[1]))) { if (svundo[svDevCnt] == false) svundo[svDevCnt] = !mvars.flgForceUpdate; }
                                                    else
                                                    {
                                                        if (Convert.ToInt16(txtverfbin.Split('.')[1]) == (Convert.ToInt16(mvars.verFPGA.Split('.')[1])) && Convert.ToInt16(txtverfbin.Split('.')[2]) == (Convert.ToInt16(mvars.verFPGA.Split('.')[2]))) { if (svundo[svDevCnt] == false) svundo[svDevCnt] = !mvars.flgForceUpdate; }
                                                    }
                                                }
                                            }

                                            //非廣播判斷
                                            if (chkNVBC.Checked == false)
                                            {
                                                if (mvars.iSender + 1 == numericUpDown_sender.Value && mvars.iPort + 1 == numericUpDown_port.Value && mvars.iScan + 1 == numUDScan.Value) svDev = svDevCnt;
                                                else svundo[svDevCnt] = true;
                                            }
                                        }
                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            mvars.iSender = (byte)Form1.NovaStarDeviceResult[svDevCnt].SenderIndex;
                                            mvars.iPort = (byte)Form1.NovaStarDeviceResult[svDevCnt].PortIndex;
                                            mvars.iScan = (byte)Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex;

                                            mvars.lblCmd = "PG_ASCTEXT";
                                            mp.mPGASCTEXT(5, 20, "", 1);
                                            if (svundo[svDevCnt])
                                            {                                                
                                                mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
                                                mp.mhFPGASPIWRITEasc("PCBA" + svpcbaver[svDevCnt] + " FPGA" + mvars.verFPGA, "UnDo", 1);
                                                svundos++;
                                            }
                                            else
                                            {
                                                mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
                                                mp.mhFPGASPIWRITEasc("PCBA" + svpcbaver[svDevCnt] + " FPGA" + mvars.verFPGA, "Prepare", 1);
                                            }
                                        }
                                        lstget1.Items.Add("Read version " + Convert.ToString(string.Format("{0:0.#}", sw.Elapsed.TotalSeconds)) + "s");
                                        lstget1.TabIndex = lstget1.Items.Count - 1;




                                        if (tabControl1.SelectedTab.Name == "tabpage_dmr")
                                        {
                                            #region Bin Write
                                            if (svundos < Form1.NovaStarDeviceResult.Count)
                                            {
                                                lstget1.Items.RemoveAt(0);
                                                if (mvars.flashselQ == 1) lstget1.Items.Add("FPGA update");
                                                else if (mvars.flashselQ == 2) lstget1.Items.Add("Demura update");
                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                if (chkNVBC.Checked)
                                                {
                                                    mvars.nvBoardcast = true;       //廣播
                                                    mvars.iSender = Convert.ToByte(Form1.svhs);
                                                    mvars.iPort = Convert.ToByte(Form1.svhp);
                                                    mvars.iScan = Convert.ToUInt16(Form1.svhc);

                                                    if (svbmpW)
                                                    {
                                                        mvars.lblCmd = "PG_ASCTEXT";
                                                        mp.mPGASCTEXT(5, 20, "", 0);
                                                        Form1.pvindex = 1;
                                                        mvars.lblCmd = "FPGA_SPI_W";
                                                        mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                                                        Form1.pvindex = 255;
                                                        mvars.lblCmd = "FPGA_SPI_W255";
                                                        mp.mhFPGASPIWRITE(0);
                                                        mp.mhFPGASPIWRITE(1);
                                                        mp.mhFPGASPIWRITE(0);
                                                        Form1.pvindex = 19;
                                                        mvars.lblCmd = "FPGA_SPI_R";
                                                        mp.mhFPGASPIREAD();
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                        {
                                                            svbxW = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]);
                                                            svfpgaW = (svbxW * 3 / 1024) * 1024;
                                                        }
                                                        Form1.pvindex = 62;
                                                        mvars.lblCmd = "FPGA_SPI_R";
                                                        mp.mhFPGASPIREAD();
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                        {
                                                            svbxH = 270 * (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]) + 1);
                                                            svfpgaH = (svbxH / 256) * 256;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (svEraseNwrNchk == 0x42)
                                                        {
                                                            Form1.pvindex = 1;
                                                            mvars.lblCmd = "FPGA_SPI_W";
                                                            mp.mhFPGASPIWRITE(3);      // 01 SI_SEL PG mode
                                                            Form1.pvindex = 255;
                                                            mvars.lblCmd = "FPGA_SPI_W255";
                                                            mp.mhFPGASPIWRITE(0);
                                                            mp.mhFPGASPIWRITE(1);
                                                            mp.mhFPGASPIWRITE(0);
                                                        }
                                                    }

                                                    mvars.lblCmd = "FLASH_TYPE";
                                                    mp.mhFLASHTYPE();
                                                    mvars.lblCmd = "FLASH_FUNCQE";
                                                    mp.mhFUNCQE();
                                                    mvars.lblCmd = "FUNC_ENABLE";
                                                    mp.mhFUNCENABLE();
                                                    mvars.lblCmd = "FUNC_STATUS";
                                                    mp.mhFUNCSTATUS();

                                                    //demura檢查狀態
                                                    mvars.nvBoardcast = false;
                                                    for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                    {
                                                        if (svundo[svDevCnt] == false)
                                                        {
                                                            mvars.iSender = (byte)Form1.hwCard[svDevCnt].iSender;
                                                            mvars.iPort = (byte)Form1.hwCard[svDevCnt].iPort;
                                                            mvars.iScan = (byte)Form1.hwCard[svDevCnt].iScan;

                                                            mp.funSendMessageTo(true);
                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                                            else
                                                            {
                                                                if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                                {
                                                                    if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                    svundo[svDevCnt] = true;
                                                                }
                                                            }
                                                            if (svundo[svDevCnt]) svundos++;
                                                        }
                                                    }

                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        sw1.Reset();
                                                        sw1.Start();

                                                        int W = 0;
                                                        int H = 0;
                                                        //demura判斷有無延伸螢幕
                                                        if (svbmpW)
                                                        {
                                                            W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                                            H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                                            Form2.ShowExtendScreen(W, H);
                                                            if (mvars.ratioX == 1 && mvars.ratioY == 1)
                                                            {
                                                                lstget1.Items.Add("No Extend Screen");
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                string svs = "BREAK Update ?" + "\r\n" + "\r\n" + "No Extend Screen";
                                                                mp.killMSGname = "FLASH UPDATE";
                                                                mp.killMSGsec = 3;
                                                                mp.KillMessageBoxStart();
                                                                if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }

                                                                //mp.ShowCursor(0);
                                                                svbmpW = false;
                                                            }
                                                        }

                                                        if (svbmpW)
                                                        {
                                                            #region BMPwrite
                                                            Form2.i3pat = new i3_Pat();
                                                            Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                                                            Form2.i3pat.BackColor = Color.Black;
                                                            Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                                            Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            Form2.i3pat.pictureBox1.Location = new Point(0, 0);
                                                            Form2.i3pat.pictureBox1.Visible = true;
                                                            Form2.i3pat.pictureBox1.BringToFront();
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            //demura創建全黑圖片
                                                            if (mvars.flashselQ == 2)
                                                            {
                                                                Bitmap bmpb = new Bitmap(svbxW, svbxH);
                                                                Graphics g1 = Graphics.FromImage(bmpb);
                                                                g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, svbxW, svbxH));
                                                                bmpb.Save(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                mp.doDelayms(100);
                                                                g1.Dispose();
                                                                bmpb.Dispose();
                                                            }

                                                            Count = (uint)(8 * 1024 * 1024) / (uint)(svfpgaW * svfpgaH);
                                                            string sverr = "";
                                                            int svn = 0;
                                                            int svm = svn;
                                                            int svnc = 0;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1) + " / " + Count);
                                                                svnc = lstget1.Items.Count - 1;
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                int svbmpcnt = 0;
                                                                int svmst = svn;
                                                                Image[,] img = new Image[(int)numUD_boxRows.Value, (int)numUD_boxCols.Value];
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    if (svy == 0) { svmst = svn; }
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        if (mvars.flashselQ == 1)
                                                                        {
                                                                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                            svm = svmst;
                                                                            mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                            img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                        }
                                                                        else if (mvars.flashselQ == 2)
                                                                        {
                                                                            if (dgvbox.Rows[svy].Cells[svx].Value == null)
                                                                            {
                                                                                img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                            }
                                                                            else
                                                                            {
                                                                                string svstr = dgvbox.Rows[svy].Cells[svx].Value.ToString() + ".bin";
                                                                                txt_filepathfull.Text = "";
                                                                                for (int j = 0; j < lst_filepathfull.Items.Count; j++)
                                                                                {
                                                                                    if (lst_filepathfull.Items[j].ToString().IndexOf(svstr, 0) != -1) { txt_filepathfull.Text = lst_filepathfull.Items[j].ToString(); break; }
                                                                                }
                                                                                if (txt_filepathfull.Text != "")
                                                                                {
                                                                                    mp.GetBin(txt_filepathfull.Text);
                                                                                    if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                                                    {
                                                                                        byte[] Tmp = new byte[8 * 1024 * 1024];
                                                                                        Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                                                        Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                                                        Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                                                    }
                                                                                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                    svm = svmst;
                                                                                    mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                                    img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                }
                                                                                else { img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp"); }
                                                                            }
                                                                        }

                                                                        if (svx == 0) { svn = svm; }
                                                                        svbmpcnt++;
                                                                    }
                                                                }
                                                                Image imgf = bmp.MergeImages(img);

                                                                Form2.i3pat.pictureBox1.Width = imgf.Width;
                                                                Form2.i3pat.pictureBox1.Height = imgf.Height;
                                                                Form2.i3pat.pictureBox1.Image = imgf;
                                                                Form2.i3pat.pictureBox1.Refresh();

                                                                imgf.Save(mvars.strStartUpPath + @"\Parameter\coding_" + svi + ".bmp");

                                                                //Flash Address
                                                                int svdt = 5000;
                                                                mp.doDelayms(svdt);

                                                                mvars.nvBoardcast = true;
                                                                mvars.iSender = Convert.ToByte(Form1.svhs);
                                                                mvars.iPort = Convert.ToByte(Form1.svhp);
                                                                mvars.iScan = Convert.ToByte(Form1.svhc);
                                                                //Program Video command
                                                                string txt36 = (svi * svfpgaW * svfpgaH).ToString("X8");
                                                                mvars.lblCmd = "FLASHWBMP";
                                                                mp.mhFLASHWRITEBMP(txt36, svfpgaH);

                                                                mvars.nvBoardcast = false;
                                                                mp.doDelayms(10000);
                                                                for (int svj = 0; svj < Form1.NovaStarDeviceResult.Count; svj++)
                                                                {
                                                                    if (svundo[svj] == false)
                                                                    {
                                                                        mvars.iSender = (byte)Form1.hwCard[svj].iSender;
                                                                        mvars.iPort = (byte)Form1.hwCard[svj].iPort;
                                                                        mvars.iScan = (byte)Form1.hwCard[svj].iScan;
                                                                        int svcnt = 0;
                                                                        while (true)
                                                                        {
                                                                            mvars.lblCmd = "FLASHWBMP_STATUS";
                                                                            mp.mhFLASHWRITEBMPSTATUS();
                                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svj] = true; break; }
                                                                            else
                                                                            {
                                                                                if ((mvars.hFuncStatus & 0x0001) == 0x0001)
                                                                                {
                                                                                    svcnt++;
                                                                                    if (svcnt > 10) { break; }
                                                                                    mp.doDelayms(200);
                                                                                }
                                                                                else { break; }
                                                                            }
                                                                        }
                                                                        //Error Status , Break.
                                                                        if (svundo[svj] == false && mvars.hFuncStatus == 0x00)
                                                                        {
                                                                            //mp.doDelayms(50);
                                                                            //lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                            lstget1.Items.Add(" --> S" + mvars.iSender+1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Done");
                                                                        }
                                                                        else
                                                                        {
                                                                            sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                            lstget1.Items.Add(" --> S" + mvars.iSender + 1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Undo," + mvars.hFuncStatus);
                                                                            svundo[svj] = true;
                                                                        }
                                                                    }
                                                                }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                imgf.Dispose();
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        img[svy, svx].Dispose();
                                                                    }
                                                                }
                                                                Array.Clear(img, 0, img.Length);

                                                                svundos = 0;
                                                                for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                {
                                                                    if (svundo[svDevCnt]) svundos++;
                                                                }
                                                                if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }

                                                                if (mvars.Break) { break; }
                                                            }
                                                            Form2.i3pat.Dispose();
                                                            #endregion BMPwrite
                                                        }
                                                        else
                                                        {
                                                            #region CODEwrite
                                                            FlashSize = (UInt32)(mvars.ucTmp.Length);   //8 * 1024 * 1024
                                                            PacketSize = 2048;                          //1024,2048,4096,8192,16384
                                                            Count = FlashSize / PacketSize;
                                                            svcounts = lstget1.Items.Count - 1;
                                                            byte svrW = 0;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1) + " / " + Count);
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                mvars.nvBoardcast = true;
                                                                mvars.iSender = Convert.ToByte(Form1.svhs);
                                                                mvars.iPort = Convert.ToByte(Form1.svhp);
                                                                mvars.iScan = Convert.ToByte(Form1.svhc);
                                                                //Program Normal command
                                                                svrW = 0;
                                                                string txt36 = (svi * PacketSize).ToString("X8");
                                                            reWr:
                                                                mvars.nvBoardcast = true;
                                                                mvars.lblCmd = "FLASH_WRITE_" + svi.ToString("0000");

                                                                if (svi % Count == 0)
                                                                {
                                                                    mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 500);
                                                                    lstget1.Items.RemoveAt(svcounts);
                                                                    lstget1.Items.Insert(svcounts, " -> " + txt36 + " @ " + String.Format("{0:00}", ((svi * 100) / Count) + "% -- ") + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                }
                                                                else { mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 30); }

                                                                mvars.nvBoardcast = false;
                                                                for (int svj = 0; svj < Form1.NovaStarDeviceResult.Count; svj++)
                                                                {
                                                                    if (svundo[svj] == false)
                                                                    {
                                                                        mvars.iSender = (byte)Form1.hwCard[svj].iSender;
                                                                        mvars.iPort = (byte)Form1.hwCard[svj].iPort;
                                                                        mvars.iScan = (byte)Form1.hwCard[svj].iScan;
                                                                        byte[] data = null;
                                                                        int readLen = 28;
                                                                        int svct = 0;
                                                                    reRd:
                                                                        bool res = mvars._marsCtrlSystem.GetDataInnolux(mvars.iSender, mvars.iPort, mvars.iScan, readLen, out data);
                                                                        if (res)
                                                                        {
                                                                            string strOutput = "";
                                                                            for (int svn = 0; svn < data.Length; svn++)
                                                                            {
                                                                                if (svn != 0 && svn % Count == 0)
                                                                                {
                                                                                    strOutput += "\r\n";
                                                                                }
                                                                                strOutput += data[svn].ToString("X2") + " ";
                                                                            }
                                                                            //ResetRichTextBox(strOutput);
                                                                            if (data[6] != 3)
                                                                            {
                                                                                if (svct == 0)
                                                                                {
                                                                                    svct++;
                                                                                    goto reRd;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (svrW < 3)
                                                                                    {
                                                                                        uint m = svi;
                                                                                        svi = (m / Count) * Count;
                                                                                        svrW++;
                                                                                        goto reWr;
                                                                                    }
                                                                                    else { svundo[svj] = true; }
                                                                                }
                                                                            }
                                                                            else { svct = 0; }
                                                                        }
                                                                        else
                                                                        {
                                                                            uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svj].iSender + 1 + "P" + Form1.hwCard[svj].iPort + 1 + "C" + Form1.hwCard[svj].iScan + 1 + ",res,false,rW" + svrW);
                                                                            lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                            if (svrW < 3)
                                                                            {
                                                                                uint m = svi;
                                                                                svi = (m / Count) * Count;
                                                                                svrW++;
                                                                                goto reWr;
                                                                            }
                                                                            else { svundo[svj] = true; }
                                                                        }
                                                                    }
                                                                }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                svundos = 0;
                                                                for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                {
                                                                    if (svundo[svDevCnt]) svundos++;
                                                                }
                                                                if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }
                                                                else
                                                                {
                                                                    lstget1.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ - 1] +
                                                                        " write  @ 100% - - - - " + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                                }

                                                                if (mvars.Break) { break; }                                                            }
                                                            #endregion CODEwrite
                                                        }//else svbmpW == false
                                                    }
                                                }
                                                else
                                                {
                                                    mvars.nvBoardcast = false;

                                                    mvars.iSender = Convert.ToByte(numericUpDown_sender.Value - 1);
                                                    mvars.iPort = Convert.ToByte(numericUpDown_port.Value - 1);
                                                    mvars.iScan = Convert.ToUInt16(numericUpDown_connect.Value - 1);

                                                    mvars.lblCmd = "PG_ASCTEXT";
                                                    mp.mPGASCTEXT(5, 20, "", 0);
                                                    Form1.pvindex = 1;
                                                    mvars.lblCmd = "FPGA_SPI_W";
                                                    mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                                                    Form1.pvindex = 255;
                                                    mvars.lblCmd = "FPGA_SPI_W255";
                                                    mp.mhFPGASPIWRITE(0);
                                                    mp.mhFPGASPIWRITE(1);
                                                    mp.mhFPGASPIWRITE(0);

                                                    if (svbmpW)
                                                    {
                                                        Form1.pvindex = 19;
                                                        mvars.lblCmd = "FPGA_SPI_R";
                                                        mp.mhFPGASPIREAD();
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                        {
                                                            svbxW = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]);
                                                            svfpgaW = (svbxW * 3 / 1024) * 1024;
                                                        }
                                                        Form1.pvindex = 62;
                                                        mvars.lblCmd = "FPGA_SPI_R";
                                                        mp.mhFPGASPIREAD();
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                        {
                                                            svbxH = 270 * (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]) + 1);
                                                            svfpgaH = (svbxH / 256) * 256;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (svEraseNwrNchk == 0x42)
                                                        {
                                                            Form1.pvindex = 1;
                                                            mvars.lblCmd = "FPGA_SPI_W";
                                                            mp.mhFPGASPIWRITE(3);      // 01 SI_SEL PG mode
                                                            Form1.pvindex = 255;
                                                            mvars.lblCmd = "FPGA_SPI_W255";
                                                            mp.mhFPGASPIWRITE(0);
                                                            mp.mhFPGASPIWRITE(1);
                                                            mp.mhFPGASPIWRITE(0);
                                                        }
                                                    }

                                                    mvars.lblCmd = "FLASH_TYPE";
                                                    mp.mhFLASHTYPE();
                                                    mvars.lblCmd = "FLASH_FUNCQE";
                                                    mp.mhFUNCQE();
                                                    mvars.lblCmd = "FUNC_ENABLE";
                                                    mp.mhFUNCENABLE();
                                                    mvars.lblCmd = "FUNC_STATUS";
                                                    mp.mhFUNCSTATUS();

                                                    if (svundo[svDev] == false)
                                                    {
                                                        mp.funSendMessageTo(true);
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDev] = true; }
                                                        else
                                                        {
                                                            if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                            {
                                                                if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                svundo[svDev] = true;
                                                            }
                                                        }
                                                        if (svundo[svDev]) svundos++;
                                                    }

                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        sw1.Reset();
                                                        sw1.Start();

                                                        int W = 0;
                                                        int H = 0;
                                                        //判斷有無延伸螢幕
                                                        if (svbmpW)
                                                        {
                                                            W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                                            H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                                            Form2.ShowExtendScreen(W, H);
                                                            if (mvars.ratioX == 1 && mvars.ratioY == 1)
                                                            {
                                                                lstget1.Items.Add("No Extend Screen");
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                string svs = "BREAK Update ?" + "\r\n" + "\r\n" + "No Extend Screen";
                                                                mp.killMSGname = "FLASH UPDATE";
                                                                mp.killMSGsec = 3;
                                                                mp.KillMessageBoxStart();
                                                                if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }

                                                                //mp.ShowCursor(0);
                                                                svbmpW = false;
                                                            }
                                                        }

                                                        if (svbmpW)
                                                        {
                                                            #region BMPwrite
                                                            Form2.i3pat = new i3_Pat();
                                                            Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                                                            Form2.i3pat.BackColor = Color.Black;
                                                            Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                                            Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            Form2.i3pat.pictureBox1.Location = new Point(0, 0);
                                                            Form2.i3pat.pictureBox1.Visible = true;
                                                            Form2.i3pat.pictureBox1.BringToFront();
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            //創建全黑圖片
                                                            if (mvars.flashselQ == 2)
                                                            {
                                                                Bitmap bmpb = new Bitmap(svbxW, svbxH);
                                                                Graphics g1 = Graphics.FromImage(bmpb);
                                                                g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, svbxW, svbxH));
                                                                bmpb.Save(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                mp.doDelayms(100);
                                                                g1.Dispose();
                                                                bmpb.Dispose();
                                                            }

                                                            Count = (uint)(8 * 1024 * 1024) / (uint)(svfpgaW * svfpgaH);
                                                            string sverr = "";
                                                            int svn = 0;
                                                            int svm = svn;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1) + " / " + Count);
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                //mp.saveFS_mk2(mvars.strStartUpPath + @"\Parameter\codingmk2_" + svi + ".bmp", ref svn); svm = svn;
                                                                //mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svi + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                //Bitmap bmpf = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svi + ".bmp");

                                                                int svbmpcnt = 0;
                                                                int svmst = svn;
                                                                Image[,] img = new Image[(int)numUD_boxRows.Value, (int)numUD_boxCols.Value];
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    if (svy == 0) { svmst = svn; }
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        if (mvars.flashselQ == 1)
                                                                        {
                                                                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                            svm = svmst;
                                                                            mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                            img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                        }
                                                                        else if (mvars.flashselQ == 2)
                                                                        {
                                                                            if (dgvbox.Rows[svy].Cells[svx].Value == null)
                                                                            {
                                                                                img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                            }
                                                                            else
                                                                            {
                                                                                string svstr = dgvbox.Rows[svy].Cells[svx].Value.ToString() + ".bin";
                                                                                txt_filepathfull.Text = "";
                                                                                for (int j = 0; j < lst_filepathfull.Items.Count; j++)
                                                                                {
                                                                                    if (lst_filepathfull.Items[j].ToString().IndexOf(svstr, 0) != -1) { txt_filepathfull.Text = lst_filepathfull.Items[j].ToString(); break; }
                                                                                }
                                                                                if (txt_filepathfull.Text != "")
                                                                                {
                                                                                    mp.GetBin(txt_filepathfull.Text);
                                                                                    if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                                                    {
                                                                                        byte[] Tmp = new byte[8 * 1024 * 1024];
                                                                                        Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                                                        Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                                                        Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                                                    }
                                                                                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                    svm = svmst;
                                                                                    mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                                    img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                }
                                                                                else { img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp"); }
                                                                            }
                                                                        }

                                                                        if (svx == 0) { svn = svm; }
                                                                        svbmpcnt++;
                                                                    }
                                                                }
                                                                Image imgf = bmp.MergeImages(img);

                                                                Form2.i3pat.pictureBox1.Width = imgf.Width;
                                                                Form2.i3pat.pictureBox1.Height = imgf.Height;
                                                                Form2.i3pat.pictureBox1.Image = imgf;
                                                                Form2.i3pat.pictureBox1.Refresh();

                                                                imgf.Save(mvars.strStartUpPath + @"\Parameter\coding_" + svi + ".bmp");

                                                                int svdt = 1000;
                                                                mp.doDelayms(svdt);
                                                                string txt36 = (svi * svfpgaW * svfpgaH).ToString("X8");
                                                                //Program Video command
                                                                mvars.lblCmd = "FLASHWBMP";
                                                                mp.mhFLASHWRITEBMP(txt36, svfpgaH);
                                                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                                                {
                                                                    lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]);
                                                                    sverr = "-11." + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count);
                                                                    svundo[svDev] = true;
                                                                }

                                                                if (svundo[svDev] == false)
                                                                {
                                                                    mp.doDelayms(8000);
                                                                    int svcnt = 0;
                                                                    while (true)
                                                                    {
                                                                        mvars.lblCmd = "FLASHWBMP_STATUS";
                                                                        mp.mhFLASHWRITEBMPSTATUS();
                                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDev] = true; break; }
                                                                        else
                                                                        {
                                                                            if ((mvars.hFuncStatus & 0x0001) == 0x0001)
                                                                            {
                                                                                svcnt++;
                                                                                if (svcnt > 7) { break; }
                                                                                mp.doDelayms(200);
                                                                            }
                                                                            else { break; }
                                                                        }
                                                                    }
                                                                    //Error Status , Break.
                                                                    if (svundo[svDev] == false && mvars.hFuncStatus == 0x00)
                                                                    {
                                                                        //mp.doDelayms(50);
                                                                        //lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                        lstget1.Items.Add(" --> S" + mvars.iSender + 1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Done");
                                                                    }
                                                                    else
                                                                    {
                                                                        sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                        svundo[svDev] = true;
                                                                        lstget1.Items.Add(" --> S" + mvars.iSender + 1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Undo," + mvars.hFuncStatus);
                                                                    }
                                                                }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                imgf.Dispose();
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        img[svy, svx].Dispose();
                                                                    }
                                                                }
                                                                Array.Clear(img, 0, img.Length);
                                                                
                                                                svundos = 0;
                                                                for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                {
                                                                    if (svundo[svDevCnt]) svundos++;
                                                                }
                                                                if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }

                                                                if (mvars.Break) { break; }
                                                            }
                                                            Form2.i3pat.Dispose();
                                                            #endregion BMPwrite
                                                        }
                                                        else
                                                        {
                                                            #region CODEwrite
                                                            FlashSize = (UInt32)(mvars.ucTmp.Length);   //8 * 1024 * 1024
                                                            PacketSize = 2048;                          //1024,2048,4096,8192,16384
                                                            Count = FlashSize / PacketSize;
                                                            svcounts = lstget1.Items.Count - 1;
                                                            byte svrW = 0;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1) + " / " + Count);
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                //Program Normal command
                                                                svrW = 0;
                                                                string txt36 = (svi * PacketSize).ToString("X8");
                                                            reWr:
                                                                mvars.nvBoardcast = true;
                                                                mvars.lblCmd = "FLASH_WRITE_" + svi.ToString("0000");

                                                                if (svi % Count == 0)
                                                                {
                                                                    mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 500);
                                                                    lstget1.Items.RemoveAt(svcounts);
                                                                    lstget1.Items.Insert(svcounts, " -> " + txt36 + " @ " + String.Format("{0:00}", ((svi * 100) / Count) + "% -- ") + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                }
                                                                else { mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 30); }

                                                                if (svundo[svDev] == false)
                                                                {
                                                                    byte[] data = null;
                                                                    int readLen = 28;
                                                                    int svct = 0;
                                                                reRd:
                                                                    bool res = mvars._marsCtrlSystem.GetDataInnolux(mvars.iSender, mvars.iPort, mvars.iScan, readLen, out data);
                                                                    if (res)
                                                                    {
                                                                        string strOutput = "";
                                                                        for (int svn = 0; svn < data.Length; svn++)
                                                                        {
                                                                            if (svn != 0 && svn % Count == 0)
                                                                            {
                                                                                strOutput += "\r\n";
                                                                            }
                                                                            strOutput += data[svn].ToString("X2") + " ";
                                                                        }
                                                                        //ResetRichTextBox(strOutput);
                                                                        if (data[6] != 3)
                                                                        {
                                                                            if (svct == 0)
                                                                            {
                                                                                svct++;
                                                                                goto reRd;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (svrW < 3)
                                                                                {
                                                                                    uint m = svi;
                                                                                    svi = (m / Count) * Count;
                                                                                    svrW++;
                                                                                    goto reWr;
                                                                                }
                                                                                else { svundo[svDev] = true; }
                                                                            }
                                                                        }
                                                                        else { svct = 0; }
                                                                    }
                                                                    else
                                                                    {
                                                                        uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svi].iSender + 1 + "P" + Form1.hwCard[svi].iPort + 1 + "C" + Form1.hwCard[svi].iScan + 1 + ",res,false,rW" + svrW);
                                                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                        if (svrW < 3)
                                                                        {
                                                                            uint m = svi;
                                                                            svi = (m / Count) * Count;
                                                                            svrW++;
                                                                            goto reWr;
                                                                        }
                                                                        else { svundo[svDev] = true; }
                                                                    }
                                                                }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                svundos = 0;
                                                                for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                {
                                                                    if (svundo[svDevCnt]) svundos++;
                                                                }
                                                                if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }

                                                                if (mvars.Break) { break; }
                                                            }
                                                            if (svundo[svDev] == false) lstget1.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ - 1] + " write  @ 100% - - - - " + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                            #endregion CODEwrite
                                                        }//else svbmpW == false
                                                    }
                                                }
                                            }
                                            #endregion Bin Write
                                        }
                                        else if (tabControl1.SelectedTab.Name == "tabpage_bmp")
                                        {
                                            #region Bin Write
                                            if (svundos < Form1.NovaStarDeviceResult.Count)
                                            {
                                                lstget1.Items.RemoveAt(0);
                                                if (mvars.flashselQ == 1) lstget1.Items.Add("FPGA update");
                                                else if (mvars.flashselQ == 2) lstget1.Items.Add("Demura update");
                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                if (chkNVBC.Checked)
                                                {
                                                    mvars.nvBoardcast = true;       //廣播
                                                    mvars.iSender = Convert.ToByte(Form1.svhs);
                                                    mvars.iPort = Convert.ToByte(Form1.svhp);
                                                    mvars.iScan = Convert.ToUInt16(Form1.svhc);

                                                    if (svbmpW)
                                                    {
                                                        mvars.lblCmd = "PG_ASCTEXT";
                                                        mp.mPGASCTEXT(5, 20, "", 0);
                                                        Form1.pvindex = 1;
                                                        mvars.lblCmd = "FPGA_SPI_W";
                                                        mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                                                        Form1.pvindex = 255;
                                                        mvars.lblCmd = "FPGA_SPI_W255";
                                                        mp.mhFPGASPIWRITE(0);
                                                        mp.mhFPGASPIWRITE(1);
                                                        mp.mhFPGASPIWRITE(0);
                                                        Form1.pvindex = 19;
                                                        mvars.lblCmd = "FPGA_SPI_R";
                                                        mp.mhFPGASPIREAD();
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                        {
                                                            svbxW = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]);
                                                            svfpgaW = (svbxW * 3 / 1024) * 1024;
                                                        }
                                                        Form1.pvindex = 62;
                                                        mvars.lblCmd = "FPGA_SPI_R";
                                                        mp.mhFPGASPIREAD();
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                        {
                                                            svbxH = 270 * (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]) + 1);
                                                            svfpgaH = (svbxH / 256) * 256;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (svEraseNwrNchk == 0x42)
                                                        {
                                                            Form1.pvindex = 1;
                                                            mvars.lblCmd = "FPGA_SPI_W";
                                                            mp.mhFPGASPIWRITE(3);      // 01 SI_SEL PG mode
                                                            Form1.pvindex = 255;
                                                            mvars.lblCmd = "FPGA_SPI_W255";
                                                            mp.mhFPGASPIWRITE(0);
                                                            mp.mhFPGASPIWRITE(1);
                                                            mp.mhFPGASPIWRITE(0);
                                                        }
                                                    }

                                                    mvars.lblCmd = "FLASH_TYPE";
                                                    mp.mhFLASHTYPE();
                                                    mvars.lblCmd = "FLASH_FUNCQE";
                                                    mp.mhFUNCQE();
                                                    mvars.lblCmd = "FUNC_ENABLE";
                                                    mp.mhFUNCENABLE();
                                                    mvars.lblCmd = "FUNC_STATUS";
                                                    mp.mhFUNCSTATUS();

                                                    //FPGA檢查狀態
                                                    mvars.nvBoardcast = false;
                                                    for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                    {
                                                        if (svundo[svDevCnt] == false)
                                                        {
                                                            mvars.iSender = (byte)Form1.hwCard[svDevCnt].iSender;
                                                            mvars.iPort = (byte)Form1.hwCard[svDevCnt].iPort;
                                                            mvars.iScan = (byte)Form1.hwCard[svDevCnt].iScan;

                                                            mp.funSendMessageTo(true);
                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                                            else
                                                            {
                                                                if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                                {
                                                                    if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                    svundo[svDevCnt] = true;
                                                                }
                                                            }
                                                            if (svundo[svDevCnt]) svundos++;
                                                        }
                                                    }

                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        sw1.Reset();
                                                        sw1.Start();

                                                        int W = 0;
                                                        int H = 0;
                                                        //FPGA判斷有無延伸螢幕
                                                        if (svbmpW)
                                                        {
                                                            W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                                            H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                                            Form2.ShowExtendScreen(W, H);
                                                            if (mvars.ratioX == 1 && mvars.ratioY == 1)
                                                            {
                                                                lstget1.Items.Add("No Extend Screen");
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                string svs = "BREAK Update ?" + "\r\n" + "\r\n" + "No Extend Screen";
                                                                mp.killMSGname = "FLASH UPDATE";
                                                                mp.killMSGsec = 3;
                                                                mp.KillMessageBoxStart();
                                                                if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }

                                                                //mp.ShowCursor(0);
                                                                svbmpW = false;
                                                            }
                                                        }

                                                        if (svbmpW)
                                                        {
                                                            #region BMPwrite
                                                            Form2.i3pat = new i3_Pat();
                                                            Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                                                            Form2.i3pat.BackColor = Color.Black;
                                                            Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                                            Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            Form2.i3pat.pictureBox1.Location = new Point(0, 0);
                                                            Form2.i3pat.pictureBox1.Visible = true;
                                                            Form2.i3pat.pictureBox1.BringToFront();
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            //FPGA創建全黑圖片
                                                            if (mvars.flashselQ == 2)
                                                            {
                                                                Bitmap bmpb = new Bitmap(svbxW, svbxH);
                                                                Graphics g1 = Graphics.FromImage(bmpb);
                                                                g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, svbxW, svbxH));
                                                                bmpb.Save(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                mp.doDelayms(100);
                                                                g1.Dispose();
                                                                bmpb.Dispose();
                                                            }

                                                            Count = (uint)(8 * 1024 * 1024) / (uint)(svfpgaW * svfpgaH);
                                                            string sverr = "";
                                                            int svn = 0;
                                                            int svm = svn;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1) + " / " + Count);
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                int svbmpcnt = 0;
                                                                int svmst = svn;
                                                                Image[,] img = new Image[(int)numUD_boxRows.Value, (int)numUD_boxCols.Value];
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    if (svy == 0) { svmst = svn; }
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        if (mvars.flashselQ == 1)
                                                                        {
                                                                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                            svm = svmst;
                                                                            mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                            img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                        }
                                                                        else if (mvars.flashselQ == 2)
                                                                        {
                                                                            if (dgvbox.Rows[svy].Cells[svx].Value == null)
                                                                            {
                                                                                img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                            }
                                                                            else
                                                                            {
                                                                                string svstr = dgvbox.Rows[svy].Cells[svx].Value.ToString() + ".bin";
                                                                                txt_filepathfull.Text = "";
                                                                                for (int j = 0; j < lst_filepathfull.Items.Count; j++)
                                                                                {
                                                                                    if (lst_filepathfull.Items[j].ToString().IndexOf(svstr, 0) != -1) { txt_filepathfull.Text = lst_filepathfull.Items[j].ToString(); break; }
                                                                                }
                                                                                if (txt_filepathfull.Text != "")
                                                                                {
                                                                                    mp.GetBin(txt_filepathfull.Text);
                                                                                    if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                                                    {
                                                                                        byte[] Tmp = new byte[8 * 1024 * 1024];
                                                                                        Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                                                        Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                                                        Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                                                    }
                                                                                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                    svm = svmst;
                                                                                    mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                                    img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                }
                                                                                else { img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp"); }
                                                                            }
                                                                        }
                                                                        
                                                                        if (svx == 0) { svn = svm; }
                                                                        svbmpcnt++;
                                                                    }
                                                                }
                                                                Image imgf = bmp.MergeImages(img);

                                                                Form2.i3pat.pictureBox1.Width = imgf.Width;
                                                                Form2.i3pat.pictureBox1.Height = imgf.Height;
                                                                Form2.i3pat.pictureBox1.Image = imgf;
                                                                Form2.i3pat.pictureBox1.Refresh();

                                                                imgf.Save(mvars.strStartUpPath + @"\Parameter\coding_" + svi + ".bmp");

                                                                //Flash Address
                                                                int svdt = 5000;
                                                                mp.doDelayms(svdt);

                                                                mvars.nvBoardcast = true;
                                                                mvars.iSender = Convert.ToByte(Form1.svhs);
                                                                mvars.iPort = Convert.ToByte(Form1.svhp);
                                                                mvars.iScan = Convert.ToByte(Form1.svhc);
                                                                //Program Video command
                                                                string txt36 = (svi * svfpgaW * svfpgaH).ToString("X8");
                                                                mvars.lblCmd = "FLASHWBMP";
                                                                mp.mhFLASHWRITEBMP(txt36, svfpgaH);

                                                                mvars.nvBoardcast = false;
                                                                mp.doDelayms(10000);
                                                                for (int svj = 0; svj < Form1.NovaStarDeviceResult.Count; svj++)
                                                                {
                                                                    if (svundo[svj] == false)
                                                                    {
                                                                        mvars.iSender = (byte)Form1.hwCard[svj].iSender;
                                                                        mvars.iPort = (byte)Form1.hwCard[svj].iPort;
                                                                        mvars.iScan = (byte)Form1.hwCard[svj].iScan;
                                                                        int svcnt = 0;
                                                                        while (true)
                                                                        {
                                                                            mvars.lblCmd = "FLASHWBMP_STATUS";
                                                                            mp.mhFLASHWRITEBMPSTATUS();
                                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svj] = true; break; }
                                                                            else
                                                                            {
                                                                                if ((mvars.hFuncStatus & 0x0001) == 0x0001)
                                                                                {
                                                                                    svcnt++;
                                                                                    if (svcnt > 10) { break; }
                                                                                    mp.doDelayms(200);
                                                                                }
                                                                                else { break; }
                                                                            }
                                                                        }
                                                                        //Error Status , Break.
                                                                        if (svundo[svj] == false && mvars.hFuncStatus == 0x00)
                                                                        {
                                                                            //mp.doDelayms(100);
                                                                            //lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                            lstget1.Items.Add(" --> S" + mvars.iSender + 1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Done");
                                                                        }
                                                                        else
                                                                        {
                                                                            sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                            svundo[svj] = true;
                                                                            lstget1.Items.Add(" --> S" + mvars.iSender + 1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Undo," + mvars.hFuncStatus);
                                                                        }
                                                                    }
                                                                }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                imgf.Dispose();
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        img[svy, svx].Dispose();
                                                                    }
                                                                }
                                                                Array.Clear(img, 0, img.Length);

                                                                svundos = 0;
                                                                for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                {
                                                                    if (svundo[svDevCnt]) svundos++;
                                                                }
                                                                if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }
                                                                else
                                                                {
                                                                    //mp.doDelayms(100);
                                                                    //lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                    lstget1.Items.Add(" --> S" + mvars.iSender + 1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Done");
                                                                }

                                                                if (mvars.Break) { break; }                                                               
                                                            }
                                                            Form2.i3pat.Dispose();
                                                            #endregion BMPwrite
                                                        }
                                                        else
                                                        {
                                                            #region CODEwrite
                                                            FlashSize = (UInt32)(mvars.ucTmp.Length);   //8 * 1024 * 1024
                                                            PacketSize = 2048;                          //1024,2048,4096,8192,16384
                                                            Count = FlashSize / PacketSize;
                                                            svcounts = lstget1.Items.Count - 1;
                                                            byte svrW = 0;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1) + " / " + Count);
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                mvars.nvBoardcast = true;
                                                                mvars.iSender = Convert.ToByte(Form1.svhs);
                                                                mvars.iPort = Convert.ToByte(Form1.svhp);
                                                                mvars.iScan = Convert.ToByte(Form1.svhc);
                                                                //Program Normal command
                                                                svrW = 0;
                                                                string txt36 = (svi * PacketSize).ToString("X8");
                                                            reWr:
                                                                mvars.nvBoardcast = true;
                                                                mvars.lblCmd = "FLASH_WRITE_" + svi.ToString("0000");

                                                                if (svi % Count == 0)
                                                                {
                                                                    mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 500);
                                                                    lstget1.Items.RemoveAt(svcounts);
                                                                    lstget1.Items.Insert(svcounts, " -> " + txt36 + " @ " + String.Format("{0:00}", ((svi * 100) / Count) + "% -- ") + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                }
                                                                else { mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 30); }

                                                                mvars.nvBoardcast = false;
                                                                for (int svj = 0; svj < Form1.NovaStarDeviceResult.Count; svj++)
                                                                {
                                                                    if (svundo[svj] == false)
                                                                    {
                                                                        mvars.iSender = (byte)Form1.hwCard[svj].iSender;
                                                                        mvars.iPort = (byte)Form1.hwCard[svj].iPort;
                                                                        mvars.iScan = (byte)Form1.hwCard[svj].iScan;
                                                                        byte[] data = null;
                                                                        int readLen = 28;
                                                                        int svct = 0;
                                                                    reRd:
                                                                        bool res = mvars._marsCtrlSystem.GetDataInnolux(mvars.iSender, mvars.iPort, mvars.iScan, readLen, out data);
                                                                        if (res)
                                                                        {
                                                                            string strOutput = "";
                                                                            for (int svn = 0; svn < data.Length; svn++)
                                                                            {
                                                                                if (svn != 0 && svn % Count == 0)
                                                                                {
                                                                                    strOutput += "\r\n";
                                                                                }
                                                                                strOutput += data[svn].ToString("X2") + " ";
                                                                            }
                                                                            //ResetRichTextBox(strOutput);
                                                                            if (data[6] != 3)
                                                                            {
                                                                                if (svct == 0)
                                                                                {
                                                                                    svct++;
                                                                                    goto reRd;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (svrW < 3)
                                                                                    {
                                                                                        uint m = svi;
                                                                                        svi = (m / Count) * Count;
                                                                                        svrW++;
                                                                                        goto reWr;
                                                                                    }
                                                                                    else { svundo[svj] = true; }
                                                                                }
                                                                            }
                                                                            else { svct = 0; }
                                                                        }
                                                                        else
                                                                        {
                                                                            uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svj].iSender + 1 + "P" + Form1.hwCard[svj].iPort + 1 + "C" + Form1.hwCard[svj].iScan + 1 + ",res,false,rW" + svrW);
                                                                            lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                            if (svrW < 3)
                                                                            {
                                                                                uint m = svi;
                                                                                svi = (m / Count) * Count;
                                                                                svrW++;
                                                                                goto reWr;
                                                                            }
                                                                            else { svundo[svj] = true; }
                                                                        }
                                                                    }
                                                                }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                svundos = 0;
                                                                for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                {
                                                                    if (svundo[svDevCnt]) svundos++;
                                                                }
                                                                if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }
                                                                else
                                                                {
                                                                    lstget1.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ - 1] +
                                                                        " write  @ 100% - - - - " + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                                }

                                                                if (mvars.Break) { break; }
                                                            }
                                                            #endregion CODEwrite
                                                        }//else svbmpW == false
                                                    }
                                                }
                                                else
                                                {
                                                    mvars.nvBoardcast = false;

                                                    mvars.iSender = Convert.ToByte(numericUpDown_sender.Value - 1);
                                                    mvars.iPort = Convert.ToByte(numericUpDown_port.Value - 1);
                                                    mvars.iScan = Convert.ToUInt16(numericUpDown_connect.Value - 1);

                                                    mvars.lblCmd = "PG_ASCTEXT";
                                                    mp.mPGASCTEXT(5, 20, "", 0);
                                                    Form1.pvindex = 1;
                                                    mvars.lblCmd = "FPGA_SPI_W";
                                                    mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                                                    Form1.pvindex = 255;
                                                    mvars.lblCmd = "FPGA_SPI_W255";
                                                    mp.mhFPGASPIWRITE(0);
                                                    mp.mhFPGASPIWRITE(1);
                                                    mp.mhFPGASPIWRITE(0);

                                                    if (svbmpW)
                                                    {
                                                        Form1.pvindex = 19;
                                                        mvars.lblCmd = "FPGA_SPI_R";
                                                        mp.mhFPGASPIREAD();
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                        {
                                                            svbxW = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]);
                                                            svfpgaW = (svbxW * 3 / 1024) * 1024;
                                                        }
                                                        Form1.pvindex = 62;
                                                        mvars.lblCmd = "FPGA_SPI_R";
                                                        mp.mhFPGASPIREAD();
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                        {
                                                            svbxH = 270 * (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]) + 1);
                                                            svfpgaH = (svbxH / 256) * 256;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (svEraseNwrNchk == 0x42)
                                                        {
                                                            Form1.pvindex = 1;
                                                            mvars.lblCmd = "FPGA_SPI_W";
                                                            mp.mhFPGASPIWRITE(3);      // 01 SI_SEL PG mode
                                                            Form1.pvindex = 255;
                                                            mvars.lblCmd = "FPGA_SPI_W255";
                                                            mp.mhFPGASPIWRITE(0);
                                                            mp.mhFPGASPIWRITE(1);
                                                            mp.mhFPGASPIWRITE(0);
                                                        }
                                                    }

                                                    mvars.lblCmd = "FLASH_TYPE";
                                                    mp.mhFLASHTYPE();
                                                    mvars.lblCmd = "FLASH_FUNCQE";
                                                    mp.mhFUNCQE();
                                                    mvars.lblCmd = "FUNC_ENABLE";
                                                    mp.mhFUNCENABLE();
                                                    mvars.lblCmd = "FUNC_STATUS";
                                                    mp.mhFUNCSTATUS();

                                                    if (svundo[svDev] == false)
                                                    {
                                                        mp.funSendMessageTo(true);
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDev] = true; }
                                                        else
                                                        {
                                                            if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                            {
                                                                if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                svundo[svDev] = true;
                                                            }
                                                        }
                                                        if (svundo[svDev]) svundos++;
                                                    }

                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        sw1.Reset();
                                                        sw1.Start();

                                                        int W = 0;
                                                        int H = 0;
                                                        //判斷有無延伸螢幕
                                                        if (svbmpW)
                                                        {
                                                            W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                                            H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                                            Form2.ShowExtendScreen(W, H);
                                                            if (mvars.ratioX == 1 && mvars.ratioY == 1)
                                                            {
                                                                lstget1.Items.Add("No Extend Screen");
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                string svs = "BREAK Update ?" + "\r\n" + "\r\n" + "No Extend Screen";
                                                                mp.killMSGname = "FLASH UPDATE";
                                                                mp.killMSGsec = 3;
                                                                mp.KillMessageBoxStart();
                                                                if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }

                                                                //mp.ShowCursor(0);
                                                                svbmpW = false;
                                                            }
                                                        }

                                                        if (svbmpW)
                                                        {
                                                            #region BMPwrite
                                                            Form2.i3pat = new i3_Pat();
                                                            Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                                                            Form2.i3pat.BackColor = Color.Black;
                                                            Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                                            Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            Form2.i3pat.pictureBox1.Location = new Point(0, 0);
                                                            Form2.i3pat.pictureBox1.Visible = true;
                                                            Form2.i3pat.pictureBox1.BringToFront();
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            //創建全黑圖片
                                                            if (mvars.flashselQ == 2)
                                                            {
                                                                Bitmap bmpb = new Bitmap(svbxW, svbxH);
                                                                Graphics g1 = Graphics.FromImage(bmpb);
                                                                g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, svbxW, svbxH));
                                                                bmpb.Save(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                mp.doDelayms(100);
                                                                g1.Dispose();
                                                                bmpb.Dispose();
                                                            }

                                                            Count = (uint)(8 * 1024 * 1024) / (uint)(svfpgaW * svfpgaH);
                                                            string sverr = "";
                                                            int svn = 0;
                                                            int svm = svn;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1) + " / " + Count);
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                //mp.saveFS_mk2(mvars.strStartUpPath + @"\Parameter\codingmk2_" + svi + ".bmp", ref svn); svm = svn;
                                                                //mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svi + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                //Bitmap bmpf = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svi + ".bmp");

                                                                int svbmpcnt = 0;
                                                                int svmst = svn;
                                                                Image[,] img = new Image[(int)numUD_boxRows.Value, (int)numUD_boxCols.Value];
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    if (svy == 0) { svmst = svn; }
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        if (mvars.flashselQ == 1)
                                                                        {
                                                                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                            svm = svmst;
                                                                            mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                            img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                        }
                                                                        else if (mvars.flashselQ == 2)
                                                                        {
                                                                            if (dgvbox.Rows[svy].Cells[svx].Value == null)
                                                                            {
                                                                                img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                            }
                                                                            else
                                                                            {
                                                                                string svstr = dgvbox.Rows[svy].Cells[svx].Value.ToString() + ".bin";
                                                                                txt_filepathfull.Text = "";
                                                                                for (int j = 0; j < lst_filepathfull.Items.Count; j++)
                                                                                {
                                                                                    if (lst_filepathfull.Items[j].ToString().IndexOf(svstr, 0) != -1) { txt_filepathfull.Text = lst_filepathfull.Items[j].ToString(); break; }
                                                                                }
                                                                                if (txt_filepathfull.Text != "")
                                                                                {
                                                                                    mp.GetBin(txt_filepathfull.Text);
                                                                                    if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                                                    {
                                                                                        byte[] Tmp = new byte[8 * 1024 * 1024];
                                                                                        Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                                                        Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                                                        Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                                                    }
                                                                                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                    svm = svmst;
                                                                                    mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                                    img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                }
                                                                                else { img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp"); }
                                                                            }
                                                                        }

                                                                        if (svx == 0) { svn = svm; }
                                                                        svbmpcnt++;
                                                                    }
                                                                }
                                                                Image imgf = bmp.MergeImages(img);

                                                                Form2.i3pat.pictureBox1.Width = imgf.Width;
                                                                Form2.i3pat.pictureBox1.Height = imgf.Height;
                                                                Form2.i3pat.pictureBox1.Image = imgf;
                                                                Form2.i3pat.pictureBox1.Refresh();

                                                                imgf.Save(mvars.strStartUpPath + @"\Parameter\coding_" + svi + ".bmp");

                                                                int svdt = 1000;
                                                                mp.doDelayms(svdt);
                                                                string txt36 = (svi * svfpgaW * svfpgaH).ToString("X8");
                                                                //Program Video command
                                                                mvars.lblCmd = "FLASHWBMP";
                                                                mp.mhFLASHWRITEBMP(txt36, svfpgaH);
                                                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                                                {
                                                                    lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]);
                                                                    sverr = "-11." + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count);
                                                                    svundo[svDev] = true;
                                                                }

                                                                if (svundo[svDev] == false)
                                                                {
                                                                    mp.doDelayms(8000);
                                                                    int svcnt = 0;
                                                                    while (true)
                                                                    {
                                                                        mvars.lblCmd = "FLASHWBMP_STATUS";
                                                                        mp.mhFLASHWRITEBMPSTATUS();
                                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-11"; break; }
                                                                        else
                                                                        {
                                                                            if ((mvars.hFuncStatus & 0x0001) == 0x0001)
                                                                            {
                                                                                svcnt++;
                                                                                if (svcnt > 7) { break; }
                                                                                mp.doDelayms(200);
                                                                            }
                                                                            else { break; }
                                                                        }
                                                                    }
                                                                    if (svundo[svDev] == false && mvars.hFuncStatus == 0x00)
                                                                    {
                                                                        //mp.doDelayms(50);
                                                                        //lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                        lstget1.Items.Add(" --> S" + mvars.iSender + 1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Done");
                                                                    }
                                                                    else
                                                                    {
                                                                        sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                        //lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                        lstget1.Items.Add(" --> S" + mvars.iSender + 1 + " P" + mvars.iPort + 1 + " C" + mvars.iScan + 1 + " Undo," + mvars.hFuncStatus);
                                                                        svundo[svDev] = true;
                                                                        //break;
                                                                    }
                                                                }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                imgf.Dispose();
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        img[svy, svx].Dispose();
                                                                    }
                                                                }
                                                                Array.Clear(img, 0, img.Length);

                                                                svundos = 0;
                                                                for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                {
                                                                    if (svundo[svDevCnt]) svundos++;
                                                                }
                                                                if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }

                                                                if (mvars.Break) { break; }
                                                            }
                                                            Form2.i3pat.Dispose();
                                                            #endregion BMPwrite
                                                        }
                                                        else
                                                        {
                                                            #region CODEwrite
                                                            FlashSize = (UInt32)(mvars.ucTmp.Length);   //8 * 1024 * 1024
                                                            PacketSize = 2048;                          //1024,2048,4096,8192,16384
                                                            Count = FlashSize / PacketSize;
                                                            svcounts = lstget1.Items.Count - 1;
                                                            byte svrW = 0;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1) + " / " + Count);
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                //Program Normal command
                                                                svrW = 0;
                                                                string txt36 = (svi * PacketSize).ToString("X8");
                                                            reWr:
                                                                mvars.nvBoardcast = true;
                                                                mvars.lblCmd = "FLASH_WRITE_" + svi.ToString("0000");

                                                                if (svi % Count == 0)
                                                                {
                                                                    mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 500);
                                                                    lstget1.Items.RemoveAt(svcounts);
                                                                    lstget1.Items.Insert(svcounts, " -> " + txt36 + " @ " + String.Format("{0:00}", ((svi * 100) / Count) + "% -- ") + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                }
                                                                else { mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 30); }

                                                                if(svundo[svDev] == false)
                                                                {
                                                                    byte[] data = null;
                                                                    int readLen = 28;
                                                                    int svct = 0;
                                                                reRd:
                                                                    bool res = mvars._marsCtrlSystem.GetDataInnolux(mvars.iSender, mvars.iPort, mvars.iScan, readLen, out data);
                                                                    if (res)
                                                                    {
                                                                        string strOutput = "";
                                                                        for (int svn = 0; svn < data.Length; svn++)
                                                                        {
                                                                            if (svn != 0 && svn % Count == 0)
                                                                            {
                                                                                strOutput += "\r\n";
                                                                            }
                                                                            strOutput += data[svn].ToString("X2") + " ";
                                                                        }
                                                                        //ResetRichTextBox(strOutput);
                                                                        if (data[6] != 3)
                                                                        {
                                                                            if (svct == 0)
                                                                            {
                                                                                svct++;
                                                                                goto reRd;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (svrW < 3)
                                                                                {
                                                                                    uint m = svi;
                                                                                    svi = (m / Count) * Count;
                                                                                    svrW++;
                                                                                    goto reWr;
                                                                                }
                                                                                else { svundo[svDev] = true; }
                                                                            }
                                                                        }
                                                                        else { svct = 0; }
                                                                    }
                                                                    else
                                                                    {
                                                                        uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svi].iSender + 1 + "P" + Form1.hwCard[svi].iPort + 1 + "C" + Form1.hwCard[svi].iScan + 1 + ",res,false,rW" + svrW);
                                                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                        if (svrW < 3)
                                                                        {
                                                                            uint m = svi;
                                                                            svi = (m / Count) * Count;
                                                                            svrW++;
                                                                            goto reWr;
                                                                        }
                                                                        else { svundo[svDev] = true; }
                                                                    }
                                                                }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                svundos = 0;
                                                                for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                {
                                                                    if (svundo[svDevCnt]) svundos++;
                                                                }
                                                                if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }

                                                                if (mvars.Break) { break; }                                                            }
                                                            if (svundo[svDev] == false)  lstget1.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ - 1] + " write  @ 100% - - - - " + Convert.ToString(string.Format("{0:###}", sw1.Elapsed.TotalSeconds)) + "sec");
                                                            #endregion CODEwrite
                                                        }//else svbmpW == false
                                                    }
                                                }
                                            }
                                            #endregion Bin Write
                                        }
                                        if (mvars.Break) { mvars.Break = false; }



                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            if (svundo[svDevCnt])
                                            {
                                                svundos++;
                                                lstget1.Items.Add("Ctrlsys" + svCtrlSysCnt +
                                                        "S" + (Form1.NovaStarDeviceResult[svDevCnt].SenderIndex + 1) +
                                                        "P" + (Form1.NovaStarDeviceResult[svDevCnt].PortIndex + 1) +
                                                        "C" + (Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex + 1) + ",UnDo");
                                            }
                                            else
                                            {
                                                lstget1.Items.Add("Ctrlsys" + svCtrlSysCnt +
                                                        "S" + (Form1.NovaStarDeviceResult[svDevCnt].SenderIndex + 1) +
                                                        "P" + (Form1.NovaStarDeviceResult[svDevCnt].PortIndex + 1) +
                                                        "C" + (Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex + 1) + ",Stanby");
                                            }
                                        }

                                        lstget1.Items.Add("");
                                        lstget1.Items.Add(mvars.nvCommList[svCtrlSysCnt] + " is " + svundos + " of " + Form1.NovaStarDeviceResult.Count + " UnDo");
                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                    }
                                    else
                                    {
                                        mp.funSaveLogs("(Err) Read NovaStar hardware...fail，No NovaStar Receiver");
                                        #region 訊息顯示  錯誤，沒有接收卡硬體
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            lstget1.Items.Add("Read Receiving Card...fail，no NovaStar hardware");
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            lstget1.Items.Add("錯誤，沒有接收卡硬體");
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            lstget1.Items.Add("错误，没有接收卡硬件");
                                        }
                                        goto Ex;
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                    else { mvars.errCode = "-1"; }
                }
                else { mvars.errCode = "-2"; }
                #endregion NovaStar
            }
            else
            {
                #region USB



                #endregion USB
            }

        Ex:
            mvars.flgForceUpdate = false; btn.ForeColor = Color.Black;
            //if (mvars.errCode == "0" && svundos < Form1.NovaStarDeviceResult.Count)
            if (svundos < Form1.NovaStarDeviceResult.Count && svbmpW)
            {
                mvars.nvBoardcast = true;       //廣播
                lstget1.Items.Add(" -> FPGA HW_RESET after Write"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mvars.lblCmd = "HW_RESET_FPGA";
                mp.mhFPGARESET(0x80);
                lstget1.Items.Add(" --> Wait 3s"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mp.doDelayms(3000);

                byte svflashQ = mvars.flashselQ;
                mvars.flashselQ = 0;
                mvars.lblCmd = "FLASH_TYPE";
                mp.mhFLASHTYPE();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    lstget1.Items.Add(" -> " + mvars.strFLASHtype[svflashQ - 1] + " write OK but FLASH_TYPE switch to \"OPEN\" fail");
                    mvars.flashselQ = svflashQ;
                    mvars.errCode = "-14";
                }
                mp.doDelayms(1000);

                Form1.pvindex = 1;
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                Form1.pvindex = 255;
                mvars.lblCmd = "FPGA_SPI_W255";
                mp.mhFPGASPIWRITE(0);
                mp.mhFPGASPIWRITE(1);
                mp.mhFPGASPIWRITE(0);

                mvars.lblCmd = "HW_RESET_FPGA";
                mp.mhFPGARESET(0x80);
                lstget1.Items.Add(" --> Wait 3s"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mp.doDelayms(3000);

                #region FPGA version
                if (btn.Tag.ToString() == "F")
                {
                    mvars.nvBoardcast = false;       //非廣播
                    mvars.isReadBack = true;
                    for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                    {
                        mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                        mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                        mvars.iScan = (byte)Form1.hwCard[svi].iScan;

                        Form1.pvindex = 0;
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]); mvars.errCode = "-2"; }
                        Form1.hwCard[svi].verFPGA = mvars.verFPGA;
                        Form1.pvindex = 254;
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]); mvars.errCode = "-2"; }
                        svpcbaver[svi] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                        uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svi].iSender + 1 + "P" + Form1.hwCard[svi].iPort + 1 + "C" + Form1.hwCard[svi].iScan + 1 + ",PCBAver," + svpcbaver[svi] + ",FPGAver," + mvars.verFPGA); uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                        mvars.lblCmd = "PG_ASCTEXT";
                        mp.mPGASCTEXT(5, 20, "PCBA" + svpcbaver[svi] + " FPGA" + mvars.verFPGA, 1);
                    }
                }
                #endregion FPGA version

                //uc_coding.lstget1.Items.Add(" --> Done and Compare OK");
                uc_coding.lstget1.Items.Add(" --> " + svundos + " of " + Form1.NovaStarDeviceResult.Count + " undo");
            }
            else { uc_coding.lstget1.Items.Add(" --> Error code:" + mvars.errCode); }
            uc_coding.lstget1.Items.Add("End " + Convert.ToString(string.Format("{0:###}", swtotal.Elapsed.TotalSeconds)) + "sec");
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            sw1.Stop();
            swtotal.Stop();
            */
        }

        private void btn_nDMRdraw_Click(object sender, EventArgs e)
        {
            /*
            Button btn = (Button)sender;
            bool svbmpW = false;
            int svDev = 0;

            #region config
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch();
            ushort svundos = 0;
            bool[] svundo = null;
            string txtverfbin = "";
            byte svEraseNwrNchk = 0x3F;
            //Write only mvars.c12aflashitem = 0(CB)-- > 0x3F and mvars.c12aflashitem = 1(XB)-- > 0x39
            //Erase and Write and Check mvars.c12aflashitem = 0(CB)-- >  = 0x42
            svEraseNwrNchk = 0x42;

            string[] svpcbaver = null;
            mvars.lCounts = 19999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            int i = 0;
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;
            //short sverrc = 0;
            //string txt44;
            UInt32 FlashSize;
            ushort PacketSize;
            UInt32 Count;
            lstget1.Items.Clear();
            mvars.flgSelf = true;
            if (tabControl1.SelectedTab.Name == "tabpage_bmp") mvars.flashselQ = 1;         //FPGA
            else if (tabControl1.SelectedTab.Name == "tabpage_dmr") mvars.flashselQ = 2;    //Demura
            int svcounts;
            int svlstc = 0;
            #endregion config

            mvars.c12aflashitem = 0;    //CB

            lstget1.Items.Clear();

            swtotal.Reset();
            swtotal.Start();

            ushort svsen = (ushort)numericUpDown_sender.Value;
            ushort svpo = (ushort)numericUpDown_port.Value;
            ushort svca = (ushort)numericUpDown_connect.Value;

            if (tabControl1.SelectedTab.Name == "tabpage_bmp")
            {
                txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                //lstget1.Items.Add("FPGA version compare ...");
                //lstget1.TopIndex = lstget1.Items.Count - 1;
                //txt_verFbin.Text = ""; txt_verFbin.Enabled = true;
                string[] svs = txt_nFlashFileName.Text.Split('_');
                for (int svi = 0; svi < svs.Length; svi++)
                {
                    if (svs[svi].ToUpper().Substring(0, 1) == "V")
                    {
                        txtverfbin = svs[svi].Substring(1, svs[svi].Length - 1);
                        txt_verFbin.Text = txtverfbin.Replace(".", string.Empty);
                        int count = 0;
                        foreach (char c in txtverfbin)
                        {
                            if (c == '.')
                            {
                                count++;
                            }
                        }
                        if (count == 2) { txt_verFbin.Enabled = false; break; } else { txt_verFbin.Text = ""; txt_verFbin.Enabled = true; }
                    }
                }

                if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                {
                    byte[] Tmp = new byte[8 * 1024 * 1024];
                    Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                    Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                    Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                }

                mp.doDelayms(10);
            }

            if (mvars.svnova)
            {
                #region NovaStar
                if (Form1.lstm.Items.Count != 0)
                {
                    int svbxW = 480;
                    int svbxH = 270;
                    int svfpgaW = (svbxW * 3 / 1024) * 1024;
                    int svfpgaH = (svbxH / 256) * 256;

                    bool svchk = false;
                    ushort svctrlsys = 0;
                    for (int svlc = 0; svlc < Form1.lstm.Items.Count; svlc++)
                    {
                        if (Form1.lstm.Items[svlc].ToString().IndexOf('↑') != -1)
                        {
                            ushort svc = Convert.ToUInt16(Form1.lstm.Items[svlc].ToString().Split(',')[3]);
                            if (svsen - svc <= 0)
                            {
                                svchk = true;
                                svctrlsys = Convert.ToUInt16(Form1.lstm.Items[svlc].ToString().Split(',')[1]);
                                //排頭訊息
                                Form1.svhs = Form1.lstm.Items[svlc].ToString().Split(',')[6];
                                Form1.svhp = Form1.lstm.Items[svlc].ToString().Split(',')[7];
                                Form1.svhc = Form1.lstm.Items[svlc].ToString().Split(',')[8];
                            }
                            else { svsen -= svc; }
                        }
                        //因應網絡型發送卡無法廣播, 所以需要透過 lstm 來找出是哪一個 IP(發送卡), 如果每一個 IP 下都有 2 台發送卡
                        //  ................S0,P,,,,
                        //  ................S1,p,,,,
                        // " ↑ " + svip + "," + mvars.nvCommList[svip] + ",Sender accumulation,2,...
                        //  ................S0,P,,,,
                        //  ................S1,p,,,,
                        // " ↑ " + svip + "," + mvars.nvCommList[svip] + ",Sender accumulation,2,...
                        // Sender 都是重複性的從 0 開始起算, 所以輸入的 svsen 要越過不同的 CtrlSysCnt 的時候就必須先減掉發送卡數量 --> svsen -= svc
                    }
                    if (svchk)
                    {
                        mvars.iSender = Convert.ToByte(Form1.svhs);
                        mvars.iPort = Convert.ToByte(Form1.svhp);
                        mvars.iScan = Convert.ToUInt16(Form1.svhc);
                        if (mvars.nvBoardcast == false)
                        {
                            svchk = false;
                            mvars.iSender = (byte)(svsen - 1);
                            mvars.iPort = (byte)(svpo - 1);
                            mvars.iScan = (ushort)(svca - 1);
                            int svlc = 0;
                            for (svlc = 0; svlc < Form1.lstm.Items.Count; svlc++)
                            {
                                if (Form1.lstm.Items[svlc].ToString().IndexOf("S" + mvars.iSender + ",P" + mvars.iPort + ",C" + mvars.iScan, 0) != -1) { svchk = true; break; }
                            }
                        }
                    }
                    if (svchk)
                    {
                        //只執行一個通訊埠或是單一個IP
                        //for (byte svCtrlSysCnt = 0; svCtrlSysCnt < mvars.nvCommList.Count; svCtrlSysCnt++)
                        for (byte svCtrlSysCnt = (byte)svctrlsys; svCtrlSysCnt <= (byte)svctrlsys; svCtrlSysCnt++)
                        {
                            if (mvars._marsCtrlSystem.UnInitialize())
                            {
                                mp.funSaveLogs(tabControl1.SelectedTab.Name + ",Unloaded marsCtrlSystem.UnInitialize");
                            }

                            mvars._nCommPort = mvars.nvCommList[svCtrlSysCnt];
                            Form1.nScreenCnt = 0;
                            Form1.nSenderCnt = 0;
                            if (mvars._marsCtrlSystem.Initialize(mvars._nCommPort, out Form1.nScreenCnt, out Form1.nSenderCnt) == false)
                            {
                                mvars.errCode = "-1";
                                mp.funSaveLogs(tabControl1.SelectedTab.Name + "," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",marsCtrlSystem.Initialize fail");
                            }
                            else
                            {
                                Form1.NovaStarDeviceResult = null;
                                svcounts = 0;
                                do
                                {
                                    Form1.NovaStarDeviceResult = mvars._marsCtrlSystem.GetAllScannerStatusByCom();
                                    mp.doDelayms(100);
                                    svcounts += 1;
                                    if (Form1.NovaStarDeviceResult != null) break;
                                }
                                while (svcounts <= 10);
                                if (Form1.NovaStarDeviceResult != null && svcounts <= 10)
                                {
                                    if (Form1.NovaStarDeviceResult.Count > 0)
                                    {
                                        Array.Resize(ref Form1.hwCard, Form1.NovaStarDeviceResult.Count);
                                        Array.Resize(ref svundo, Form1.NovaStarDeviceResult.Count);
                                        Array.Resize(ref svpcbaver, Form1.NovaStarDeviceResult.Count);
                                        if (mvars.flgSelf == false) { lstget1.Items.Clear(); }
                                        for (int j = 0; j < Form1.NovaStarDeviceResult.Count; j++)  //Form1.NovaStarDeviceResult.Count = Form1.NovaStarDeviceResult.Count;
                                        {
                                            Form1.hwCard[j].iSender = Form1.NovaStarDeviceResult[j].SenderIndex;
                                            Form1.hwCard[j].iPort = Form1.NovaStarDeviceResult[j].PortIndex;
                                            Form1.hwCard[j].iScan = Form1.NovaStarDeviceResult[j].ScannerIndex;
                                            svundo[j] = false;
                                        }
                                        Form1.hwCards = Form1.hwCard.Length;
                                        mvars.lCounts = 9999;
                                        Array.Resize(ref mvars.lCmd, mvars.lCounts);
                                        Array.Resize(ref mvars.lGet, mvars.lCounts);
                                        mvars.lCount = 0;
                                        mvars.strReceive = "";
                                        mvars.flgDelFB = true;
                                        mvars.flgSelf = true;
                                        mvars.nvBoardcast = false;
                                        mvars.isReadBack = true;

                                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                                        sw.Reset();
                                        sw.Start();
                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            //mvars.errCode = "000";
                                            mvars.iSender = (byte)Form1.NovaStarDeviceResult[svDevCnt].SenderIndex;
                                            mvars.iPort = (byte)Form1.NovaStarDeviceResult[svDevCnt].PortIndex;
                                            mvars.iScan = (byte)Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex;

                                            Form1.pvindex = 0;
                                            mvars.lblCmd = "FPGA_SPI_R";
                                            mp.mhFPGASPIREAD();
                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                            Form1.hwCard[svDevCnt].verFPGA = mvars.verFPGA;
                                            Form1.pvindex = 254;
                                            mvars.lblCmd = "FPGA_SPI_R";
                                            mp.mhFPGASPIREAD();
                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDevCnt] = true; }
                                            else
                                            {
                                                svpcbaver[svDevCnt] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                                                if (svpcbaver[svDevCnt] != "1" && svpcbaver[svDevCnt] != "2") { svundo[svDevCnt] = true; }
                                            }

                                            //非廣播判斷
                                            if (chkNVBC.Checked == false)
                                            {
                                                if (mvars.iSender + 1 == numericUpDown_sender.Value && mvars.iPort + 1 == numericUpDown_port.Value && mvars.iScan + 1 == numUDScan.Value) svDev = svDevCnt;
                                                else svundo[svDevCnt] = true;
                                            }
                                        }
                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            mvars.iSender = (byte)Form1.NovaStarDeviceResult[svDevCnt].SenderIndex;
                                            mvars.iPort = (byte)Form1.NovaStarDeviceResult[svDevCnt].PortIndex;
                                            mvars.iScan = (byte)Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex;

                                            mvars.lblCmd = "PG_ASCTEXT";
                                            mp.mPGASCTEXT(5, 20, "", 1);
                                            if (svundo[svDevCnt])
                                            {
                                                mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
                                                mp.mhFPGASPIWRITEasc("PCBA" + svpcbaver[svDevCnt] + " FPGA" + mvars.verFPGA, "UnDo", 1);
                                                svundos++;
                                            }
                                            else
                                            {
                                                mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
                                                mp.mhFPGASPIWRITEasc("PCBA" + svpcbaver[svDevCnt] + " FPGA" + mvars.verFPGA, "Prepare", 1);
                                            }
                                        }
                                        lstget1.Items.Add("Read version " + Convert.ToString(string.Format("{0:0.#}", sw.Elapsed.TotalSeconds)) + "s");
                                        lstget1.TabIndex = lstget1.Items.Count - 1;




                                        if (tabControl1.SelectedTab.Name == "tabpage_dmr")
                                        {
                                            #region Bin Write
                                            if (svundos < Form1.NovaStarDeviceResult.Count)
                                            {
                                                lstget1.Items.RemoveAt(0);
                                                lstget1.Items.Add("Demura update");
                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                if (chkNVBC.Checked)
                                                {
                                                    mvars.nvBoardcast = true;       //廣播
                                                    mvars.iSender = Convert.ToByte(Form1.svhs);
                                                    mvars.iPort = Convert.ToByte(Form1.svhp);
                                                    mvars.iScan = Convert.ToUInt16(Form1.svhc);

                                                    mvars.lblCmd = "PG_ASCTEXT";
                                                    mp.mPGASCTEXT(5, 20, "", 0);
                                                    Form1.pvindex = 1;
                                                    mvars.lblCmd = "FPGA_SPI_W";
                                                    mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                                                    Form1.pvindex = 255;
                                                    mvars.lblCmd = "FPGA_SPI_W255";
                                                    mp.mhFPGASPIWRITE(0);
                                                    mp.mhFPGASPIWRITE(1);
                                                    mp.mhFPGASPIWRITE(0);

                                                    Form1.pvindex = 19;
                                                    mvars.lblCmd = "FPGA_SPI_R";
                                                    mp.mhFPGASPIREAD();
                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                    {
                                                        svbxW = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]);
                                                        svfpgaW = (svbxW * 3 / 1024) * 1024;
                                                    }
                                                    Form1.pvindex = 62;
                                                    mvars.lblCmd = "FPGA_SPI_R";
                                                    mp.mhFPGASPIREAD();
                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                    {
                                                        svbxH = 270 * (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]) + 1);
                                                        svfpgaH = (svbxH / 256) * 256;
                                                    }

                                                    mvars.lblCmd = "FLASH_TYPE";
                                                    mp.mhFLASHTYPE();
                                                    mvars.lblCmd = "FLASH_FUNCQE";
                                                    mp.mhFUNCQE();
                                                    mvars.lblCmd = "FUNC_ENABLE";
                                                    mp.mhFUNCENABLE();
                                                    mvars.lblCmd = "FUNC_STATUS";
                                                    mp.mhFUNCSTATUS();

                                                    mvars.nvBoardcast = false;

                                                    for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                                                    {
                                                        if (svundo[svi] == false)
                                                        {
                                                            mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                                                            mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                                                            mvars.iScan = (byte)Form1.hwCard[svi].iScan;

                                                            mp.funSendMessageTo(true);
                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svi] = true; }
                                                            else
                                                            {
                                                                if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                                {
                                                                    if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                    svundo[svi] = true;
                                                                }
                                                            }
                                                            if (svundo[svi]) svundos++;
                                                        }
                                                    }

                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        sw1.Reset();
                                                        sw1.Start();
                                                        if (mvars.ucTmp != null && mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                        {
                                                            byte[] Tmp = new byte[8 * 1024 * 1024];
                                                            Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                            Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                            Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                        }

                                                        int W = 0;
                                                        int H = 0;

                                                        W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                                        H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                                        Form2.ShowExtendScreen(W, H);
                                                        if (mvars.ratioX == 1 && mvars.ratioY == 1)
                                                        {
                                                            lstget1.Items.Add("No Extend Screen");
                                                            lstget1.TopIndex = lstget1.Items.Count - 1;

                                                            string svs = "BREAK Update ?" + "\r\n" + "\r\n" + "No Extend Screen";
                                                            //if (MessageBox.Show(svs, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) { SvErr = "-7"; goto Ex; }

                                                            mp.killMSGname = "Demura UPDATE";
                                                            mp.killMSGsec = 3;
                                                            mp.KillMessageBoxStart();
                                                            if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }

                                                            mp.ShowCursor(0);
                                                        }
                                                        else
                                                        {
                                                            #region BMPwrite
                                                            Form2.i3pat = new i3_Pat();
                                                            Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                                                            Form2.i3pat.BackColor = Color.Black;
                                                            Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                                            Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            Form2.i3pat.pictureBox1.Location = new Point(0, 0);
                                                            Form2.i3pat.pictureBox1.Visible = true;
                                                            Form2.i3pat.pictureBox1.BringToFront();
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            //創建全黑圖片
                                                            Bitmap bmpb = new Bitmap(svbxW, svbxH);
                                                            Graphics g1 = Graphics.FromImage(bmpb);
                                                            g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, svbxW, svbxH));
                                                            bmpb.Save(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");

                                                            Count = (uint)(8 * 1024 * 1024) / (uint)(svfpgaW * svfpgaH);
                                                            string sverr = "";
                                                            int svn = 0;
                                                            int svm = svn;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1));
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                int svbmpcnt = 0;
                                                                int svmst = svn;
                                                                Image[,] img = new Image[(int)numUD_boxRows.Value, (int)numUD_boxCols.Value];
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    if (svy == 0) { svmst = svn; }
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        if (dgvbox.Rows[svy].Cells[svx].Value == null)
                                                                        {
                                                                            img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");
                                                                        }
                                                                        else
                                                                        {
                                                                            string svstr = dgvbox.Rows[svy].Cells[svx].Value.ToString() + ".bin";
                                                                            txt_filepathfull.Text = "";
                                                                            for (int j = 0; j < lst_filepathfull.Items.Count; j++)
                                                                            {
                                                                                if (lst_filepathfull.Items[j].ToString().IndexOf(svstr, 0) != -1) { txt_filepathfull.Text = lst_filepathfull.Items[j].ToString(); break; }
                                                                            }
                                                                            if (txt_filepathfull.Text != "")
                                                                            {
                                                                                mp.GetBin(txt_filepathfull.Text);
                                                                                if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                                                {
                                                                                    byte[] Tmp = new byte[8 * 1024 * 1024];
                                                                                    Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                                                    Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                                                    Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                                                }
                                                                                if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                svm = svmst;
                                                                                mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                                img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                            }
                                                                            else { img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp"); }
                                                                        }

                                                                        if (svx == 0) { svn = svm; }
                                                                        svbmpcnt++;
                                                                    }
                                                                }
                                                                Image imgf = bmp.MergeImages(img);

                                                                Form2.i3pat.pictureBox1.Width = imgf.Width;
                                                                Form2.i3pat.pictureBox1.Height = imgf.Height;
                                                                Form2.i3pat.pictureBox1.Image = imgf;
                                                                Form2.i3pat.pictureBox1.Refresh();

                                                                imgf.Save(mvars.strStartUpPath + @"\Parameter\coding_" + svi + ".bmp");

                                                                //Flash Address
                                                                if (svbmpW)
                                                                {
                                                                    int svdt = 2000;
                                                                    mp.doDelayms(svdt);

                                                                    mvars.nvBoardcast = true;
                                                                    mvars.iSender = Convert.ToByte(Form1.svhs);
                                                                    mvars.iPort = Convert.ToByte(Form1.svhp);
                                                                    mvars.iScan = Convert.ToByte(Form1.svhc);
                                                                    //Program Video command
                                                                    string txt36 = (svi * svfpgaW * svfpgaH).ToString("X8");
                                                                    mvars.lblCmd = "FLASHWBMP";
                                                                    mp.mhFLASHWRITEBMP(txt36, svfpgaH);

                                                                    mvars.nvBoardcast = false;
                                                                    mp.doDelayms(12000);
                                                                    for (int svj = 0; svj < Form1.NovaStarDeviceResult.Count; svj++)
                                                                    {
                                                                        if (svundo[svj] == false)
                                                                        {
                                                                            mvars.iSender = (byte)Form1.hwCard[svj].iSender;
                                                                            mvars.iPort = (byte)Form1.hwCard[svj].iPort;
                                                                            mvars.iScan = (byte)Form1.hwCard[svj].iScan;
                                                                            int svcnt = 0;
                                                                            while (true)
                                                                            {
                                                                                mvars.lblCmd = "FLASHWBMP_STATUS";
                                                                                mp.mhFLASHWRITEBMPSTATUS();
                                                                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svj] = true; break; }
                                                                                else
                                                                                {
                                                                                    if ((mvars.hFuncStatus & 0x0001) == 0x0001)
                                                                                    {
                                                                                        svcnt++;
                                                                                        if (svcnt > 20) { break; }
                                                                                        mp.doDelayms(200);
                                                                                    }
                                                                                    else { break; }
                                                                                }
                                                                            }
                                                                            //Error Status , Break.
                                                                            if (svundo[svj] == false && mvars.hFuncStatus == 0x00)
                                                                            {
                                                                                mp.doDelayms(500);
                                                                                lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                            }
                                                                            else
                                                                            {
                                                                                sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                                svundo[svj] = true;
                                                                            }
                                                                        }
                                                                    }
                                                                    svundos = 0;
                                                                    for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                    {
                                                                        if (svundo[svDevCnt]) svundos++;
                                                                    }
                                                                    if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }
                                                                    else
                                                                    {
                                                                        mp.doDelayms(500);
                                                                        lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    mp.doDelayms(1500);
                                                                    lstget1.Items.Add(" --> BMP Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                }

                                                                imgf.Dispose();
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        img[svy, svx].Dispose();
                                                                    }
                                                                }
                                                                Array.Clear(img, 0, img.Length);
                                                                if (mvars.Break) { break; }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                            }
                                                            g1.Dispose();
                                                            bmpb.Dispose();
                                                            Form2.i3pat.Dispose();
                                                            #endregion BMPwrite
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    mvars.nvBoardcast = false;

                                                    mvars.iSender = Convert.ToByte(numericUpDown_sender.Value - 1);
                                                    mvars.iPort = Convert.ToByte(numericUpDown_port.Value - 1);
                                                    mvars.iScan = Convert.ToUInt16(numericUpDown_connect.Value - 1);

                                                    mvars.lblCmd = "PG_ASCTEXT";
                                                    mp.mPGASCTEXT(5, 20, "", 0);
                                                    Form1.pvindex = 1;
                                                    mvars.lblCmd = "FPGA_SPI_W";
                                                    mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                                                    Form1.pvindex = 255;
                                                    mvars.lblCmd = "FPGA_SPI_W255";
                                                    mp.mhFPGASPIWRITE(0);
                                                    mp.mhFPGASPIWRITE(1);
                                                    mp.mhFPGASPIWRITE(0);

                                                    Form1.pvindex = 19;
                                                    mvars.lblCmd = "FPGA_SPI_R";
                                                    mp.mhFPGASPIREAD();
                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                    {
                                                        svbxW = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]);
                                                        svfpgaW = (svbxW * 3 / 1024) * 1024;
                                                    }
                                                    Form1.pvindex = 62;
                                                    mvars.lblCmd = "FPGA_SPI_R";
                                                    mp.mhFPGASPIREAD();
                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                    {
                                                        svbxH = 270 * (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]) + 1);
                                                        svfpgaH = (svbxH / 256) * 256;
                                                    }

                                                    mvars.lblCmd = "FLASH_TYPE";
                                                    mp.mhFLASHTYPE();
                                                    mvars.lblCmd = "FLASH_FUNCQE";
                                                    mp.mhFUNCQE();
                                                    mvars.lblCmd = "FUNC_ENABLE";
                                                    mp.mhFUNCENABLE();
                                                    mvars.lblCmd = "FUNC_STATUS";
                                                    mp.mhFUNCSTATUS();

                                                    mvars.nvBoardcast = false;

                                                    if (svundo[svDev] == false)
                                                    {
                                                        mp.funSendMessageTo(true);
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDev] = true; }
                                                        else
                                                        {
                                                            if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                            {
                                                                if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                svundo[svDev] = true;
                                                            }
                                                        }
                                                        if (svundo[svDev]) svundos++;
                                                    }

                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        sw1.Reset();
                                                        sw1.Start();
                                                        if (mvars.ucTmp != null && mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                        {
                                                            byte[] Tmp = new byte[8 * 1024 * 1024];
                                                            Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                            Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                            Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                        }

                                                        int W = 0;
                                                        int H = 0;

                                                        W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                                        H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                                        Form2.ShowExtendScreen(W, H);
                                                        if (mvars.ratioX == 1 && mvars.ratioY == 1)
                                                        {
                                                            lstget1.Items.Add("No Extend Screen");
                                                            lstget1.TopIndex = lstget1.Items.Count - 1;

                                                            string svs = "BREAK Update ?" + "\r\n" + "\r\n" + "No Extend Screen";
                                                            //if (MessageBox.Show(svs, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) { SvErr = "-7"; goto Ex; }

                                                            mp.killMSGname = "FLASH UPDATE";
                                                            mp.killMSGsec = 3;
                                                            mp.KillMessageBoxStart();
                                                            if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }

                                                            mp.ShowCursor(0);
                                                        }
                                                        else
                                                        {
                                                            #region BMPwrite
                                                            Form2.i3pat = new i3_Pat();
                                                            Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                                                            Form2.i3pat.BackColor = Color.Black;
                                                            Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                                            Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            Form2.i3pat.pictureBox1.Location = new Point(0, 0);
                                                            Form2.i3pat.pictureBox1.Visible = true;
                                                            Form2.i3pat.pictureBox1.BringToFront();
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            //創建全黑圖片
                                                            Bitmap bmpb = new Bitmap(svbxW, svbxH);
                                                            Graphics g1 = Graphics.FromImage(bmpb);
                                                            g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, svbxW, svbxH));
                                                            bmpb.Save(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");

                                                            Count = (uint)(8 * 1024 * 1024) / (uint)(svfpgaW * svfpgaH);
                                                            string sverr = "";
                                                            int svn = 0;
                                                            int svm = svn;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1));
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                                                int svbmpcnt = 0;
                                                                int svmst = svn;
                                                                Image[,] img = new Image[(int)numUD_boxRows.Value, (int)numUD_boxCols.Value];
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    if (svy == 0) { svmst = svn; }
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        if (dgvbox.Rows[svy].Cells[svx].Value == null)
                                                                        {
                                                                            img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp"); 
                                                                        }
                                                                        else
                                                                        {
                                                                            string svstr = dgvbox.Rows[svy].Cells[svx].Value.ToString() + ".bin";
                                                                            txt_filepathfull.Text = "";
                                                                            for (int j = 0; j < lst_filepathfull.Items.Count; j++)
                                                                            {
                                                                                if (lst_filepathfull.Items[j].ToString().IndexOf(svstr, 0) != -1) { txt_filepathfull.Text = lst_filepathfull.Items[j].ToString(); break; }
                                                                            }
                                                                            if (txt_filepathfull.Text != "")
                                                                            {
                                                                                mp.GetBin(txt_filepathfull.Text);
                                                                                if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                                                {
                                                                                    byte[] Tmp = new byte[8 * 1024 * 1024];
                                                                                    Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                                                    Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                                                    Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                                                }
                                                                                if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                                svm = svmst;
                                                                                mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                                img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                            }
                                                                            else { img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\bmpb.bmp"); }
                                                                        }
                                                                        
                                                                        if (svx == 0) { svn = svm; }
                                                                        svbmpcnt++;
                                                                    }
                                                                }
                                                                Image imgf = bmp.MergeImages(img);

                                                                Form2.i3pat.pictureBox1.Width = imgf.Width;
                                                                Form2.i3pat.pictureBox1.Height = imgf.Height;
                                                                Form2.i3pat.pictureBox1.Image = imgf;
                                                                Form2.i3pat.pictureBox1.Refresh();

                                                                imgf.Save(mvars.strStartUpPath + @"\Parameter\coding_" + svi + ".bmp");

                                                                //Flash Address
                                                                if (svbmpW)
                                                                {
                                                                    int svdt = 1000;
                                                                    mp.doDelayms(svdt);
                                                                    string txt36 = (svi * svfpgaW * svfpgaH).ToString("X8");
                                                                    //Program Video command
                                                                    mvars.lblCmd = "FLASHWBMP";
                                                                    mp.mhFLASHWRITEBMP(txt36, svfpgaH);
                                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                                                    {
                                                                        lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]);
                                                                        sverr = "-11." + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count);
                                                                        svundo[svDev] = true;
                                                                    }

                                                                    if (svundo[svDev] == false)
                                                                    {
                                                                        mp.doDelayms(8000);
                                                                        int svcnt = 0;
                                                                        while (true)
                                                                        {
                                                                            mvars.lblCmd = "FLASHWBMP_STATUS";
                                                                            mp.mhFLASHWRITEBMPSTATUS();
                                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-11"; break; }
                                                                            else
                                                                            {
                                                                                if ((mvars.hFuncStatus & 0x0001) == 0x0001)
                                                                                {
                                                                                    svcnt++;
                                                                                    if (svcnt > 7) { break; }
                                                                                    mp.doDelayms(200);
                                                                                }
                                                                                else { break; }
                                                                            }
                                                                        }
                                                                        if (mvars.hFuncStatus == 0x00)
                                                                        {
                                                                            mp.doDelayms(500);
                                                                            lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                        }
                                                                        else
                                                                        {
                                                                            sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                            svundo[svDev] = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    mp.doDelayms(1500);
                                                                    lstget1.Items.Add(" --> BMP Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                }

                                                                imgf.Dispose();
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        img[svy, svx].Dispose();
                                                                    }
                                                                }
                                                                Array.Clear(img, 0, img.Length);
                                                                if (mvars.Break) { break; }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                            }
                                                            g1.Dispose();
                                                            bmpb.Dispose();
                                                            Form2.i3pat.Dispose();
                                                            #endregion BMPwrite
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion Bin Write
                                        }
                                        else if (tabControl1.SelectedTab.Name == "tabpage_bmp")
                                        {
                                            #region Bin Write
                                            //if (mvars.flgForceUpdate) { Array.Clear(svundo, 0, svundo.Length); }
                                            //if (svundos < Form1.NovaStarDeviceResult.Count || mvars.flgForceUpdate)
                                            if (svundos < Form1.NovaStarDeviceResult.Count)
                                            {
                                                lstget1.Items.RemoveAt(0);
                                                lstget1.Items.Add("FPGA update");
                                                lstget1.TopIndex = lstget1.Items.Count - 1;


                                                if (chkNVBC.Checked)
                                                {
                                                    mvars.nvBoardcast = true;       //廣播
                                                    mvars.iSender = Convert.ToByte(Form1.svhs);
                                                    mvars.iPort = Convert.ToByte(Form1.svhp);
                                                    mvars.iScan = Convert.ToUInt16(Form1.svhc);

                                                    mvars.lblCmd = "PG_ASCTEXT";
                                                    mp.mPGASCTEXT(5, 20, "", 0);
                                                    Form1.pvindex = 1;
                                                    mvars.lblCmd = "FPGA_SPI_W";
                                                    mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                                                    Form1.pvindex = 255;
                                                    mvars.lblCmd = "FPGA_SPI_W255";
                                                    mp.mhFPGASPIWRITE(0);
                                                    mp.mhFPGASPIWRITE(1);
                                                    mp.mhFPGASPIWRITE(0);

                                                    Form1.pvindex = 19;
                                                    mvars.lblCmd = "FPGA_SPI_R";
                                                    mp.mhFPGASPIREAD();
                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                    {
                                                        svbxW = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]);
                                                        svfpgaW = (svbxW * 3 / 1024) * 1024;
                                                    }
                                                    Form1.pvindex = 62;
                                                    mvars.lblCmd = "FPGA_SPI_R";
                                                    mp.mhFPGASPIREAD();
                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                    {
                                                        svbxH = 270 * (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]) + 1);
                                                        svfpgaH = (svbxH / 256) * 256;
                                                    }

                                                    mvars.lblCmd = "FLASH_TYPE";
                                                    mp.mhFLASHTYPE();
                                                    mvars.lblCmd = "FLASH_FUNCQE";
                                                    mp.mhFUNCQE();
                                                    mvars.lblCmd = "FUNC_ENABLE";
                                                    mp.mhFUNCENABLE();
                                                    mvars.lblCmd = "FUNC_STATUS";
                                                    mp.mhFUNCSTATUS();

                                                    mvars.nvBoardcast = false;

                                                    for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                                                    {
                                                        if (svundo[svi] == false)
                                                        {
                                                            mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                                                            mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                                                            mvars.iScan = (byte)Form1.hwCard[svi].iScan;

                                                            mp.funSendMessageTo(true);
                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svi] = true; }
                                                            else
                                                            {
                                                                if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                                {
                                                                    if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                    uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                    svundo[svi] = true;
                                                                }
                                                            }
                                                            if (svundo[svi]) svundos++;
                                                        }
                                                    }

                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        sw1.Reset();
                                                        sw1.Start();
                                                        if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                        {
                                                            byte[] Tmp = new byte[8 * 1024 * 1024];
                                                            Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                            Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                            Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                        }

                                                        int W = 0;
                                                        int H = 0;

                                                        W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                                        H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                                        Form2.ShowExtendScreen(W, H);
                                                        if (mvars.ratioX == 1 && mvars.ratioY == 1)
                                                        {
                                                            lstget1.Items.Add("No Extend Screen");
                                                            lstget1.TopIndex = lstget1.Items.Count - 1;

                                                            string svs = "BREAK Update ?" + "\r\n" + "\r\n" + "No Extend Screen";
                                                            //if (MessageBox.Show(svs, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) { SvErr = "-7"; goto Ex; }

                                                            mp.killMSGname = "FLASH UPDATE";
                                                            mp.killMSGsec = 3;
                                                            mp.KillMessageBoxStart();
                                                            if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }


                                                            mp.ShowCursor(0);
                                                        }
                                                        else
                                                        {
                                                            #region BMPwrite
                                                            Form2.i3pat = new i3_Pat();
                                                            Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                                                            Form2.i3pat.BackColor = Color.Black;
                                                            Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                                            Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            Form2.i3pat.pictureBox1.Location = new Point(0, 0);
                                                            Form2.i3pat.pictureBox1.Visible = true;
                                                            Form2.i3pat.pictureBox1.BringToFront();
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            //創建全黑圖片
                                                            Bitmap bmpb = new Bitmap(svbxW, svbxH);
                                                            Graphics g1 = Graphics.FromImage(bmpb);
                                                            g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, svbxW, svbxH));
                                                            bmpb.Save(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");

                                                            Count = (uint)(8 * 1024 * 1024) / (uint)(svfpgaW * svfpgaH);
                                                            string sverr = "";
                                                            int svn = 0;
                                                            int svm = svn;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1));
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                //mp.saveFS_mk2(mvars.strStartUpPath + @"\Parameter\codingmk2_" + svi + ".bmp", ref svn); svm = svn;
                                                                //mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svi + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                //Bitmap bmpf = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svi + ".bmp");

                                                                int svbmpcnt = 0;
                                                                int svmst = svn;
                                                                Image[,] img = new Image[(int)numUD_boxRows.Value, (int)numUD_boxCols.Value];
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    if (svy == 0) { svmst = svn; }
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        mp.GetBin(txt_nFlashFileNameFull.Text);
                                                                        if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                                        {
                                                                            byte[] Tmp = new byte[8 * 1024 * 1024];
                                                                            Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                                            Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                                            Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                                        }
                                                                        if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                        svm = svmst;
                                                                        mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                        img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                        if (svx == 0) { svn = svm; }
                                                                        svbmpcnt++;
                                                                    }
                                                                }
                                                                Image imgf = bmp.MergeImages(img);

                                                                Form2.i3pat.pictureBox1.Width = imgf.Width;
                                                                Form2.i3pat.pictureBox1.Height = imgf.Height;
                                                                Form2.i3pat.pictureBox1.Image = imgf;
                                                                Form2.i3pat.pictureBox1.Refresh();

                                                                imgf.Save(mvars.strStartUpPath + @"\Parameter\coding_" + svi + ".bmp");

                                                                //Flash Address
                                                                if (svbmpW)
                                                                {
                                                                    int svdt = 5000;
                                                                    mp.doDelayms(svdt);

                                                                    mvars.nvBoardcast = true;
                                                                    mvars.iSender = Convert.ToByte(Form1.svhs);
                                                                    mvars.iPort = Convert.ToByte(Form1.svhp);
                                                                    mvars.iScan = Convert.ToByte(Form1.svhc);
                                                                    //Program Video command
                                                                    string txt36 = (svi * svfpgaW * svfpgaH).ToString("X8");
                                                                    mvars.lblCmd = "FLASHWBMP";
                                                                    mp.mhFLASHWRITEBMP(txt36, svfpgaH);

                                                                    mvars.nvBoardcast = false;
                                                                    mp.doDelayms(10000);
                                                                    for (int svj = 0; svj < Form1.NovaStarDeviceResult.Count; svj++)
                                                                    {
                                                                        if (svundo[svj] == false)
                                                                        {
                                                                            mvars.iSender = (byte)Form1.hwCard[svj].iSender;
                                                                            mvars.iPort = (byte)Form1.hwCard[svj].iPort;
                                                                            mvars.iScan = (byte)Form1.hwCard[svj].iScan;
                                                                            int svcnt = 0;
                                                                            while (true)
                                                                            {
                                                                                mvars.lblCmd = "FLASHWBMP_STATUS";
                                                                                mp.mhFLASHWRITEBMPSTATUS();
                                                                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svj] = true; break; }
                                                                                else
                                                                                {
                                                                                    if ((mvars.hFuncStatus & 0x0001) == 0x0001)
                                                                                    {
                                                                                        svcnt++;
                                                                                        if (svcnt > 10) { break; }
                                                                                        mp.doDelayms(200);
                                                                                    }
                                                                                    else { break; }
                                                                                }
                                                                            }
                                                                            //Error Status , Break.
                                                                            if (svundo[svj] == false && mvars.hFuncStatus == 0x00)
                                                                            {
                                                                                mp.doDelayms(500);
                                                                                lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                            }
                                                                            else
                                                                            {
                                                                                sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                                svundo[svj] = true;
                                                                            }
                                                                        }
                                                                    }
                                                                    svundos = 0;
                                                                    for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                                                    {
                                                                        if (svundo[svDevCnt]) svundos++;
                                                                    }
                                                                    if (svundos >= Form1.NovaStarDeviceResult.Count) { break; }
                                                                    else
                                                                    {
                                                                        mp.doDelayms(500);
                                                                        lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    mp.doDelayms(1500);
                                                                    lstget1.Items.Add(" --> BMP Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                }

                                                                imgf.Dispose();
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        img[svy, svx].Dispose();
                                                                    }
                                                                }
                                                                Array.Clear(img, 0, img.Length);
                                                                if (mvars.Break) { break; }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                            }
                                                            g1.Dispose();
                                                            bmpb.Dispose();
                                                            Form2.i3pat.Dispose();
                                                            #endregion BMPwrite
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    mvars.nvBoardcast = false;

                                                    mvars.iSender = Convert.ToByte(numericUpDown_sender.Value - 1);
                                                    mvars.iPort = Convert.ToByte(numericUpDown_port.Value - 1);
                                                    mvars.iScan = Convert.ToUInt16(numericUpDown_connect.Value - 1);

                                                    mvars.lblCmd = "PG_ASCTEXT";
                                                    mp.mPGASCTEXT(5, 20, "", 0);
                                                    Form1.pvindex = 1;
                                                    mvars.lblCmd = "FPGA_SPI_W";
                                                    mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                                                    Form1.pvindex = 255;
                                                    mvars.lblCmd = "FPGA_SPI_W255";
                                                    mp.mhFPGASPIWRITE(0);
                                                    mp.mhFPGASPIWRITE(1);
                                                    mp.mhFPGASPIWRITE(0);

                                                    Form1.pvindex = 19;
                                                    mvars.lblCmd = "FPGA_SPI_R";
                                                    mp.mhFPGASPIREAD();
                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                    {
                                                        svbxW = Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]);
                                                        svfpgaW = (svbxW * 3 / 1024) * 1024;
                                                    }
                                                    Form1.pvindex = 62;
                                                    mvars.lblCmd = "FPGA_SPI_R";
                                                    mp.mhFPGASPIREAD();
                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                    {
                                                        svbxH = 270 * (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]) + 1);
                                                        svfpgaH = (svbxH / 256) * 256;
                                                    }

                                                    mvars.lblCmd = "FLASH_TYPE";
                                                    mp.mhFLASHTYPE();
                                                    mvars.lblCmd = "FLASH_FUNCQE";
                                                    mp.mhFUNCQE();
                                                    mvars.lblCmd = "FUNC_ENABLE";
                                                    mp.mhFUNCENABLE();
                                                    mvars.lblCmd = "FUNC_STATUS";
                                                    mp.mhFUNCSTATUS();

                                                    mvars.nvBoardcast = false;

                                                    if (svundo[svDev] == false)
                                                    {
                                                        mp.funSendMessageTo(true);
                                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundo[svDev] = true; }
                                                        else
                                                        {
                                                            if (!((mvars.hFuncStatus & 0x02) == 0x02))
                                                            {
                                                                if (mvars.flashselQ == 1) { uc_coding.lstget1.Items.Add("FPGA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                else if (mvars.flashselQ == 2) { uc_coding.lstget1.Items.Add("DEMURA FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02"); }
                                                                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                                                                svundo[svDev] = true;
                                                            }
                                                        }
                                                        if (svundo[svDev]) svundos++;
                                                    }

                                                    if (svundos < Form1.NovaStarDeviceResult.Count)
                                                    {
                                                        sw1.Reset();
                                                        sw1.Start();
                                                        if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                        {
                                                            byte[] Tmp = new byte[8 * 1024 * 1024];
                                                            Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                            Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                            Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                        }

                                                        int W = 0;
                                                        int H = 0;

                                                        W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                                                        H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                                                        Form2.ShowExtendScreen(W, H);
                                                        if (mvars.ratioX == 1 && mvars.ratioY == 1)
                                                        {
                                                            lstget1.Items.Add("No Extend Screen");
                                                            lstget1.TopIndex = lstget1.Items.Count - 1;

                                                            string svs = "BREAK Update ?" + "\r\n" + "\r\n" + "No Extend Screen";
                                                            //if (MessageBox.Show(svs, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) { SvErr = "-7"; goto Ex; }

                                                            mp.killMSGname = "FLASH UPDATE";
                                                            mp.killMSGsec = 3;
                                                            mp.KillMessageBoxStart();
                                                            if (MessageBox.Show(svs + "\r\n" + "\r\n" + "MessageBox close @ " + mp.killMSGsec + "s later", mp.killMSGname, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) { mvars.Break = true; }


                                                            mp.ShowCursor(0);
                                                        }
                                                        else
                                                        {
                                                            #region BMPwrite
                                                            Form2.i3pat = new i3_Pat();
                                                            Form2.i3pat.Location = new System.Drawing.Point(W, 0);
                                                            Form2.i3pat.BackColor = Color.Black;
                                                            Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                                                            Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            Form2.i3pat.pictureBox1.Location = new Point(0, 0);
                                                            Form2.i3pat.pictureBox1.Visible = true;
                                                            Form2.i3pat.pictureBox1.BringToFront();
                                                            Form2.i3pat.Show();
                                                            Form2.i3pat.TopMost = true;
                                                            //創建全黑圖片
                                                            Bitmap bmpb = new Bitmap(svbxW, svbxH);
                                                            Graphics g1 = Graphics.FromImage(bmpb);
                                                            g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, svbxW, svbxH));
                                                            bmpb.Save(mvars.strStartUpPath + @"\Parameter\bmpb.bmp");

                                                            Count = (uint)(8 * 1024 * 1024) / (uint)(svfpgaW * svfpgaH);
                                                            string sverr = "";
                                                            int svn = 0;
                                                            int svm = svn;
                                                            for (UInt32 svi = 0; svi < Count; svi++)
                                                            {
                                                                lstget1.Items.Add("Write Counter：" + string.Format("{0:00}", svi + 1));
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                                //mp.saveFS_mk2(mvars.strStartUpPath + @"\Parameter\codingmk2_" + svi + ".bmp", ref svn); svm = svn;
                                                                //mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svi + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                //Bitmap bmpf = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svi + ".bmp");

                                                                int svbmpcnt = 0;
                                                                int svmst = svn;
                                                                Image[,] img = new Image[(int)numUD_boxRows.Value, (int)numUD_boxCols.Value];
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    if (svy == 0) { svmst = svn; }
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        mp.GetBin(txt_nFlashFileNameFull.Text);
                                                                        if (mvars.ucTmp.Length <= (8 * 1024 * 1024))
                                                                        {
                                                                            byte[] Tmp = new byte[8 * 1024 * 1024];
                                                                            Buffer.BlockCopy(mvars.ucTmp, 0, Tmp, 0, mvars.ucTmp.Length);
                                                                            Array.Resize(ref mvars.ucTmp, 8 * 1024 * 1024);
                                                                            Array.Copy(Tmp, mvars.ucTmp, Tmp.Length);
                                                                        }
                                                                        if (File.Exists(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp")) File.Delete(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                        svm = svmst;
                                                                        mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp", ref svm, svbxW, svbxH, svfpgaW, svfpgaH);
                                                                        img[svy, svx] = new Bitmap(mvars.strStartUpPath + @"\Parameter\codingmk6_" + svbmpcnt + ".bmp");
                                                                        if (svx == 0) { svn = svm; }
                                                                        svbmpcnt++;
                                                                    }
                                                                }
                                                                Image imgf = bmp.MergeImages(img);

                                                                Form2.i3pat.pictureBox1.Width = imgf.Width;
                                                                Form2.i3pat.pictureBox1.Height = imgf.Height;
                                                                Form2.i3pat.pictureBox1.Image = imgf;
                                                                Form2.i3pat.pictureBox1.Refresh();

                                                                imgf.Save(mvars.strStartUpPath + @"\Parameter\coding_" + svi + ".bmp");

                                                                //Flash Address
                                                                if (svbmpW)
                                                                {
                                                                    int svdt = 1000;
                                                                    mp.doDelayms(svdt);
                                                                    string txt36 = (svi * svfpgaW * svfpgaH).ToString("X8");
                                                                    //Program Video command
                                                                    mvars.lblCmd = "FLASHWBMP";
                                                                    mp.mhFLASHWRITEBMP(txt36, svfpgaH);
                                                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                                                    {
                                                                        lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]);
                                                                        sverr = "-11." + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count);
                                                                        svundo[svDev] = true;
                                                                    }

                                                                    if (svundo[svDev] == false)
                                                                    {
                                                                        mp.doDelayms(8000);
                                                                        int svcnt = 0;
                                                                        while (true)
                                                                        {
                                                                            mvars.lblCmd = "FLASHWBMP_STATUS";
                                                                            mp.mhFLASHWRITEBMPSTATUS();
                                                                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-11"; break; }
                                                                            else
                                                                            {
                                                                                if ((mvars.hFuncStatus & 0x0001) == 0x0001)
                                                                                {
                                                                                    svcnt++;
                                                                                    if (svcnt > 7) { break; }
                                                                                    mp.doDelayms(200);
                                                                                }
                                                                                else { break; }
                                                                            }
                                                                        }
                                                                        //Error Status , Break.
                                                                        //if ((mvars.hFuncStatus & 0x0002) == 0x0002)
                                                                        //{
                                                                        //    sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                        //    break;
                                                                        //}
                                                                        if (mvars.hFuncStatus == 0x00)
                                                                        {
                                                                            mp.doDelayms(500);
                                                                            lstget1.Items.Add(" --> Write Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                        }
                                                                        else
                                                                        {
                                                                            sverr = "-2." + (svi + 1) + "." + Form1.numUDSender.Value + "." + Form1.numUDPort.Value + "." + Form1.numUDScan.Value + "." + mvars.hFuncStatus;
                                                                            svundo[svDev] = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    mp.doDelayms(1500);
                                                                    lstget1.Items.Add(" --> BMP Counter：" + (svi + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(svi + 1) * 100 / Count) + "%");
                                                                }

                                                                imgf.Dispose();
                                                                for (int svy = 0; svy < img.GetLength(0); svy++)
                                                                {
                                                                    for (int svx = 0; svx < img.GetLength(1); svx++)
                                                                    {
                                                                        img[svy, svx].Dispose();
                                                                    }
                                                                }
                                                                Array.Clear(img, 0, img.Length);
                                                                if (mvars.Break) { break; }
                                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                            }
                                                            g1.Dispose();
                                                            bmpb.Dispose();
                                                            Form2.i3pat.Dispose();
                                                            #endregion BMPwrite
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion Bin Write
                                        }
                                        if (mvars.Break) { mvars.Break = false; }

                                        svundos = 0;
                                        for (int svDevCnt = 0; svDevCnt < Form1.NovaStarDeviceResult.Count; svDevCnt++)
                                        {
                                            if (svundo[svDevCnt])
                                            {
                                                svundos++;
                                                lstget1.Items.Add("Ctrlsys" + svCtrlSysCnt +
                                                        "S" + (Form1.NovaStarDeviceResult[svDevCnt].SenderIndex + 1) +
                                                        "P" + (Form1.NovaStarDeviceResult[svDevCnt].PortIndex + 1) +
                                                        "C" + (Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex + 1) + ",UnDo");
                                            }
                                            else
                                            {
                                                lstget1.Items.Add("Ctrlsys" + svCtrlSysCnt +
                                                        "S" + (Form1.NovaStarDeviceResult[svDevCnt].SenderIndex + 1) +
                                                        "P" + (Form1.NovaStarDeviceResult[svDevCnt].PortIndex + 1) +
                                                        "C" + (Form1.NovaStarDeviceResult[svDevCnt].ScannerIndex + 1) + ",Stanby");
                                            }
                                        }

                                        lstget1.Items.Add("");
                                        lstget1.Items.Add(mvars.nvCommList[svCtrlSysCnt] + " is " + svundos + " of " + Form1.NovaStarDeviceResult.Count + " UnDo");
                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                    }
                                    else
                                    {
                                        mp.funSaveLogs("(Err) Read NovaStar hardware...fail，No NovaStar Receiver");
                                        #region 訊息顯示  錯誤，沒有接收卡硬體
                                        if (MultiLanguage.DefaultLanguage == "en-US")
                                        {
                                            lstget1.Items.Add("Read Receiving Card...fail，no NovaStar hardware");
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                        {
                                            lstget1.Items.Add("錯誤，沒有接收卡硬體");
                                        }
                                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                        {
                                            lstget1.Items.Add("错误，没有接收卡硬件");
                                        }
                                        goto Ex;
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                    else { mvars.errCode = "-1"; }
                }
                else { mvars.errCode = "-2"; }
                #endregion NovaStar
            }
            else
            {
                #region USB



                #endregion USB
            }

        Ex:
            mvars.flgForceUpdate = false; btn_nFPGAW.ForeColor = Color.Black;
            //if (mvars.errCode == "0" && svundos < Form1.NovaStarDeviceResult.Count)
            if (svundos < Form1.NovaStarDeviceResult.Count && svbmpW)
            {
                mvars.nvBoardcast = true;       //廣播
                lstget1.Items.Add(" -> FPGA HW_RESET after Write"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mvars.lblCmd = "HW_RESET_FPGA";
                mp.mhFPGARESET(0x80);
                lstget1.Items.Add(" --> Wait 3s"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mp.doDelayms(3000);

                byte svflashQ = mvars.flashselQ;
                mvars.flashselQ = 0;
                mvars.lblCmd = "FLASH_TYPE";
                mp.mhFLASHTYPE();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    lstget1.Items.Add(" -> " + mvars.strFLASHtype[svflashQ - 1] + " write OK but FLASH_TYPE switch to \"OPEN\" fail");
                    mvars.flashselQ = svflashQ;
                    mvars.errCode = "-14";
                }
                mp.doDelayms(1000);

                Form1.pvindex = 1;
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(0);      // 01 SI_SEL PC mode
                Form1.pvindex = 255;
                mvars.lblCmd = "FPGA_SPI_W255";
                mp.mhFPGASPIWRITE(0);
                mp.mhFPGASPIWRITE(1);
                mp.mhFPGASPIWRITE(0);

                mvars.lblCmd = "HW_RESET_FPGA";
                mp.mhFPGARESET(0x80);
                lstget1.Items.Add(" --> Wait 3s"); lstget1.TopIndex = lstget1.Items.Count - 1;
                mp.doDelayms(3000);

                #region FPGA version
                if (btn.Tag.ToString() == "F")
                {
                    mvars.nvBoardcast = false;       //非廣播
                    mvars.isReadBack = true;
                    for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
                    {
                        mvars.iSender = (byte)Form1.hwCard[svi].iSender;
                        mvars.iPort = (byte)Form1.hwCard[svi].iPort;
                        mvars.iScan = (byte)Form1.hwCard[svi].iScan;

                        Form1.pvindex = 0;
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]); mvars.errCode = "-2"; }
                        Form1.hwCard[svi].verFPGA = mvars.verFPGA;
                        Form1.pvindex = 254;
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]); mvars.errCode = "-2"; }
                        svpcbaver[svi] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                        uc_coding.lstget1.Items.Add("S" + Form1.hwCard[svi].iSender + 1 + "P" + Form1.hwCard[svi].iPort + 1 + "C" + Form1.hwCard[svi].iScan + 1 + ",PCBAver," + svpcbaver[svi] + ",FPGAver," + mvars.verFPGA); uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                        mvars.lblCmd = "PG_ASCTEXT";
                        mp.mPGASCTEXT(5, 20, "PCBA" + svpcbaver[svi] + " FPGA" + mvars.verFPGA, 1);
                    }
                }
                #endregion FPGA version

                //uc_coding.lstget1.Items.Add(" --> Done and Compare OK");
                uc_coding.lstget1.Items.Add(" --> " + svundos + " of " + Form1.NovaStarDeviceResult.Count + " undo");
            }
            else { uc_coding.lstget1.Items.Add(" --> Error code:" + mvars.errCode); }
            uc_coding.lstget1.Items.Add("End " + Convert.ToString(string.Format("{0:###}", swtotal.Elapsed.TotalSeconds)) + "sec");
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            sw1.Stop();
            swtotal.Stop();
            */
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

        private void FPGA_Flash_Mode_rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (FPGA_Flash_Mode_rbtn.Checked)
                FPGA_FlashMode_cbx.Enabled = true;
            else
                FPGA_FlashMode_cbx.Enabled = false;
        }


        private void FPGA_FlashMode_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1.cmbhPB.Text = Form1.cmbhPB.Items[FPGA_FlashMode_cbx.SelectedIndex].ToString();
            callOnClick(FPGA_FlashPathSend_btn);
            callOnClick(SPI_ReadJedecId_btn);
        }



        private void FPGA_FlashPathSend_btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            btn.Enabled = false;

            #region config (judge mvars.flashselQ)
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch();

            mvars.lCounts = 19999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;
            lstget1.Items.Clear();
            mvars.flgSelf = true;
            #endregion config

            mvars.c12aflashitem = 1;    //0:CB Flash 1:XB Flash

            swtotal.Reset();
            swtotal.Start();

            //if (chkNVBC.Checked)
            //{
            //    mvars.iSender = Convert.ToByte(Form1.svhs);
            //    mvars.iPort = Convert.ToByte(Form1.svhp);
            //    mvars.iScan = Convert.ToUInt16(Form1.svhc);
            //}
            //else
            //{
            //    mvars.iSender = (byte)(numericUpDown_sender.Value - 1);
            //    mvars.iPort = (byte)(numericUpDown_port.Value - 1);
            //    mvars.iScan = (ushort)(numericUpDown_scan.Value - 1);
            //}



            if (mvars.svnova)
            {
                #region NovaStar
                //mvars.lblCmd = "SPI_FLASHPATH";
                //if (FPGA_Reg_Mode_rbtn.Checked)
                //    mp.mSPI_FLASHPATHSEL(16);                                       //SPI Flash Path Select
                //else
                //    mp.mSPI_FLASHPATHSEL((byte)FPGA_FlashMode_cbx.SelectedIndex);   //SPI Flash Path Select
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
                //else
                //{
                //    if (mp.IsNumeric(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)) == false) { Form1.hwCard[svDevCnt].CurrentBootVer = 0; }
                //    else { Form1.hwCard[svDevCnt].CurrentBootVer = Convert.ToUInt16(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)); }
                //}


                #endregion NovaStar
            }
            else
            {
                #region USB



                #endregion USB
            }

        Ex:
            mvars.flgForceUpdate = false; btn.ForeColor = System.Drawing.Color.Black;
            uc_coding.lstget1.Items.Add("End " + Convert.ToString(string.Format("{0:###}", swtotal.Elapsed.TotalSeconds)) + "sec");
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            sw1.Stop();
            swtotal.Stop();
            btn.Enabled = true;
            //Byte[] OUTBuffer = new byte[513];   //Allocate a memory buffer equal to the OUT endpoint size + 1
            //Byte[] INBuffer = new byte[65];     //Allocate a memory buffer equal to the IN endpoint size + 1
            //bReturnFlag = false;

            //OUTBuffer[2 + 1] = 0x05;                                        //Cmd
            //OUTBuffer[3 + 1] = 0x00;                                        //Size
            //OUTBuffer[4 + 1] = 0x0A;                                        //Size
            //if (FPGA_Reg_Mode_rbtn.Checked)
            //    OUTBuffer[5 + 1] = 16;                                    //SPI Flash Path Select
            //else
            //    OUTBuffer[5 + 1] = (byte)FPGA_FlashMode_cbx.SelectedIndex; //SPI Flash Path Select
            //FillOutBuffer(OUTBuffer);
            ////Send To MCU
            //if (WriteReadMcuTask(OUTBuffer, INBuffer) == true)
            //{
            //    OP_Msg1(OP_Msg_lbx, "FlashSel_cbx_SelectedIndexChanged OK");
            //    bReturnFlag = true;
            //}
        }

        private void SPI_ReadJedecId_btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            btn.Enabled = false;

            #region config (judge mvars.flashselQ)
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch swtotal = new System.Diagnostics.Stopwatch();
            mvars.lCounts = 19999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;
            lstget1.Items.Clear();
            mvars.flgSelf = true;
            #endregion config

            mvars.c12aflashitem = 1;    //0:CB Flash 1:XB Flash

            swtotal.Reset();
            swtotal.Start();


            if (mvars.svnova)
            {
                #region NovaStar

                if (chkNVBC.Checked)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") MessageBox.Show("Please select cabinet");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") MessageBox.Show("回讀請指定箱體");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") MessageBox.Show("回读请指定箱体");
                    btn.Enabled = true;
                    return;
                }
                //mvars.iSender = (byte)(numericUpDown_sender.Value - 1);
                //mvars.iPort = (byte)(numericUpDown_port.Value - 1);
                //mvars.iScan = (ushort)(numericUpDown_scan.Value - 1);

                lbl_jedecid.Text = "";
                mvars.lblCmd = "READ_JEDECID";
                mp.mSPI_READJEDECID();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
                else
                {
                    lbl_jedecid.Text = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                    string str = lbl_jedecid.Text;
                    if (str.Substring((str.Length - 2), 2) == "13")
                        SPI_512KB_rdbtn.Checked = true;
                    if (str.Substring((str.Length - 2), 2) == "14")
                        SPI_1MB_rdbtn.Checked = true;
                    if (str.Substring((str.Length - 2), 2) == "15")
                        SPI_2MB_rdbtn.Checked = true;
                    if (str.Substring(0, 2) == "EF")
                        SPI_Vender_cbx.Text = "WINBOND";
                    if (str.Substring(0, 2) == "C2")
                        SPI_Vender_cbx.Text = "MXIC";
                }


                #endregion NovaStar
            }
            else
            {
                #region USB



                #endregion USB
            }

        Ex:
            mvars.flgForceUpdate = false; btn.ForeColor = System.Drawing.Color.Black;
            uc_coding.lstget1.Items.Add("End " + Convert.ToString(string.Format("{0:###}", swtotal.Elapsed.TotalMilliseconds)) + "ms");
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            sw1.Stop();
            swtotal.Stop();
            btn.Enabled = true;
        }

        private void btn_nLBR_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (FPGA_Reg_Mode_rbtn.Checked)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") MessageBox.Show("Please select FPGA SPI Mode");
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") MessageBox.Show("回讀請指定 FPGA SPI 模式");
                else if (MultiLanguage.DefaultLanguage == "zh-CN") MessageBox.Show("回读请指定 FPGA SPI 模式");
                btn.Enabled = true;
                return;
            }

            //lstget1.SetBounds(21, 134, 636, 294);
            #region config
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mvars.Break = false;
            mvars.lCounts = 19999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            //int i;
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;
            UInt32 FlashSize;
            ushort PacketSize;
            UInt32 Count;
            lstget1.Items.Clear();
            mvars.flgSelf = true;

            mvars.c12aflashitem = 1;    //0:CB Flash 1:XB Flash
            byte[] FlashRd_Arr = new byte[1];

            #endregion config

            if (mvars.svnova)
            {
                if (chkNVBC.Checked)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") MessageBox.Show("Please select cabinet");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") MessageBox.Show("回讀請指定箱體");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") MessageBox.Show("回读请指定箱体");
                    btn.Enabled = true;
                    return;
                }
                //mvars.iSender = (byte)(numericUpDown_sender.Value - 1);
                //mvars.iPort = (byte)(numericUpDown_port.Value - 1);
                //mvars.iScan = (ushort)(numericUpDown_scan.Value - 1);

                #region re-get Cards non-necessary
                //Form1.NovaStarDeviceResult = null;
                //int svcounts = 0;
                //do
                //{
                //    Form1.NovaStarDeviceResult = mvars._marsCtrlSystem.GetAllScannerStatusByCom();
                //    mp.doDelayms(100);
                //    svcounts += 1;
                //    if (Form1.NovaStarDeviceResult != null) break;
                //}
                //while (svcounts <= 10);
                //if (Form1.NovaStarDeviceResult != null && svcounts <= 10)
                //{
                //    lstget1.Items.Clear();
                //    Form1.hwCards = Form1.NovaStarDeviceResult.Count;
                //    Array.Resize(ref Form1.hwCard, Form1.NovaStarDeviceResult.Count);
                //    Array.Clear(Form1.hwCard, 0, Form1.NovaStarDeviceResult.Count);
                //    #region 訊息顯示 接收卡數量
                //    if (MultiLanguage.DefaultLanguage == "en-US")
                //    {
                //        lstget1.Items.Add("Read Receiving Card... amount " + Form1.hwCards);
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //    {
                //        lstget1.Items.Add("接收卡數量 " + Form1.hwCards);
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //    {
                //        lstget1.Items.Add("接收卡数量 " + Form1.hwCards);
                //    }
                //    #endregion
                //    if (mvars.flgSelf == false) { lstget1.Items.Clear(); }

                //    if (Form1.hwCards == 0)
                //    {
                //        lstget1.Items.Add(" --> 沒有接收卡");
                //        lst_get1.Items.Add("");
                //        mvars.errCode = "-1"; goto Ex;
                //    }
                //    if (btn.Tag.ToString().ToUpper() == "FPGA") { btn_nFPGAR.Enabled = false; btn_nFPGABREAK.Enabled = true; }
                //    else if (btn.Tag.ToString().ToUpper() == "DMR") { btn_nDMRR.Enabled = false; btn_nDMRBREAK.Enabled = true; }
                //    for (int j = 0; j < Form1.hwCards; j++)
                //    {
                //        Form1.hwCard[j].iSender = Form1.NovaStarDeviceResult[j].SenderIndex;
                //        Form1.hwCard[j].iPort = Form1.NovaStarDeviceResult[j].PortIndex;
                //        Form1.hwCard[j].iScan = Form1.NovaStarDeviceResult[j].ScannerIndex;
                //    }
                //}
                //else
                //{
                //    mp.funSaveLogs("(Err) Read NovaStar hardware...fail，no NovaStar hardware");
                //    #region 訊息顯示  錯誤，沒有接收卡硬體
                //    if (MultiLanguage.DefaultLanguage == "en-US")
                //    {
                //        lstget1.Items.Add("Read Receiving Card...fail，no NovaStar hardware");
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //    {
                //        lstget1.Items.Add("錯誤，沒有接收卡硬體");
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //    {
                //        lstget1.Items.Add("错误，没有接收卡硬件");
                //    }
                //    goto Ex;
                //    #endregion
                //}
                #endregion
            }

            #region callOnClick(FPGA_FlashPathSend_btn);

            //for (i = 0; i < FPGA_FlashMode_cbx.Items.Count; i++)
            //{
            //    if (FPGA_FlashMode_cbx.Text == FPGA_FlashMode_cbx.Items[i].ToString()) { break; }
            //}
            //if (i >= FPGA_FlashMode_cbx.Items.Count)
            //{
            //    if (MultiLanguage.DefaultLanguage == "en-US") MessageBox.Show("Please select LB");
            //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") MessageBox.Show("回讀請指定燈板");
            //    else if (MultiLanguage.DefaultLanguage == "zh-CN") MessageBox.Show("回读请指定灯板");
            //}

            //mvars.lblCmd = "SPI_FLASHPATH";
            //if (FPGA_Reg_Mode_rbtn.Checked)
            //    mp.mSPI_FLASHPATHSEL(16);                                       //SPI Flash Path Select
            //else
            //    mp.mSPI_FLASHPATHSEL((byte)i);   //SPI Flash Path Select
            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
            #endregion

            #region callOnClick(SPI_ReadJedecId_btn);
            lbl_jedecid.Text = "";
            mvars.lblCmd = "READ_JEDECID";
            mp.mSPI_READJEDECID();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
            else
            {
                lbl_jedecid.Text = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                string str = lbl_jedecid.Text;
                if (str.Substring((str.Length - 2), 2) == "13")
                    SPI_512KB_rdbtn.Checked = true;
                if (str.Substring((str.Length - 2), 2) == "14")
                    SPI_1MB_rdbtn.Checked = true;
                if (str.Substring((str.Length - 2), 2) == "15")
                    SPI_2MB_rdbtn.Checked = true;
                if (str.Substring(0, 2) == "EF")
                    SPI_Vender_cbx.Text = "WINBOND";
                if (str.Substring(0, 2) == "C2")
                    SPI_Vender_cbx.Text = "MXIC";
            }
            #endregion

            mvars.errCode = "0";
            FlashSize = 1 * 1024 * 1024;
            uc_coding.flashreadsize = 2048;
            PacketSize = uc_coding.flashreadsize;
            Count = FlashSize / PacketSize;
            Array.Resize(ref FlashRd_Arr, (int)FlashSize);
            sw.Reset();
            sw.Start();
            mvars.nvBoardcast = false;
            //for (i = 0; i < Count; i++)
            //{
            //    string textBox36 = (i * PacketSize).ToString("X8");
            //    lstget1.Items.Add(String.Format("{0:00}", ((i * 100) / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
            //    lstget1.TopIndex = lstget1.Items.Count - 1;
            //    Application.DoEvents();
            //    mvars.lblCmd = "FLASH_READ_" + i.ToString("0000");
            //    mp.mhFLASHREAD(textBox36, PacketSize);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
            //    {
            //        lst_get1.Items.Add(" -> " + (mvars.iSender + 1) + "," + (mvars.iPort + 1) + "," + (mvars.iScan + 1) + "," + FPGA_FlashMode_cbx.Text +
            //            " FLASH_READ_" + i.ToString("0000") + " fail @ " + String.Format("{0:00}", ((i * 100) / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
            //        lst_get1.Items.Add("");
            //        mvars.errCode = "-1";
            //        break;
            //    }
            //    else
            //    {
            //        for (UInt32 m = 0; m < PacketSize; m++) { FlashRd_Arr[i * PacketSize + m] = mvars.gFlashRdPacketArr[m]; }
            //    }
            //    if (mvars.Break)
            //    {
            //        lst_get1.Items.Add(" -> " + (mvars.iSender + 1) + "," + (mvars.iPort + 1) + "," + (mvars.iScan + 1) + "," + FPGA_FlashMode_cbx.Text +
            //            " FLASH_READ_" + i.ToString("0000") + " BREAK @ " + String.Format("{0:00}", ((i * 100) / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
            //        lst_get1.Items.Add("");
            //        mvars.errCode = "-2";
            //        break;
            //    }
            //}




        Ex:
            if (mvars.errCode == "0")
            {
                string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\MiniLedLog\\S" + numericUpDown_sender.Value + ",P" + numericUpDown_port.Value + ",C" + numericUpDown_scan.Value + "," + FPGA_FlashMode_cbx.Text + "Read.bin";

                //Check FolderPath Exist
                if (!Directory.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\MiniLedLog"))
                    Directory.CreateDirectory("C:\\Users\\" + Environment.UserName + "\\Documents\\MiniLedLog");

                List<byte> Wcontent = FlashRd_Arr.ToList();
                int num = 0;
                num = Wcontent.Count();
                if (num > 0)
                {
                    byte[] buffer = Wcontent.ToArray();
                    FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    fileStream.Write(buffer, 0, num);
                    fileStream.Close();
                }
                lst_get1.Items.Add(" -> " + path);
                lst_get1.TopIndex = lst_get1.Items.Count - 1;
            }
            else { uc_coding.lstget1.Items.Add(" --> Error code:" + mvars.errCode); }
            uc_coding.lstget1.Items.Add("End," + Convert.ToString(string.Format("{0:#.0}", sw.Elapsed.TotalMinutes)) + "m");
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;

            mvars.nvBoardcast = true;
        }

        private void btn_nLBW_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            #region config
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mvars.lCounts = 19999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;
            lstget1.Items.Clear();
            mvars.flgSelf = true;
            mvars.c12aflashitem = 1;    //0:CB Flash 1:XB Flash
            byte svEraseNwrNchk = 0x39;
            #endregion config

            if (mvars.svnova)
            {
                if (chkNVBC.Checked)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") MessageBox.Show("Please select cabinet");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") MessageBox.Show("回讀請指定箱體");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") MessageBox.Show("回读请指定箱体");
                    btn.Enabled = true;
                    return;
                }
                //mvars.iSender = (byte)(numericUpDown_sender.Value - 1);
                //mvars.iPort = (byte)(numericUpDown_port.Value - 1);
                //mvars.iScan = (ushort)(numericUpDown_scan.Value - 1);

                #region re-get Cards non-necessary
                //Form1.NovaStarDeviceResult = null;
                //int svcounts = 0;
                //do
                //{
                //    Form1.NovaStarDeviceResult = mvars._marsCtrlSystem.GetAllScannerStatusByCom();
                //    mp.doDelayms(100);
                //    svcounts += 1;
                //    if (Form1.NovaStarDeviceResult != null) break;
                //}
                //while (svcounts <= 10);
                //if (Form1.NovaStarDeviceResult != null && svcounts <= 10)
                //{
                //    lstget1.Items.Clear();
                //    Form1.hwCards = Form1.NovaStarDeviceResult.Count;
                //    Array.Resize(ref Form1.hwCard, Form1.NovaStarDeviceResult.Count);
                //    Array.Clear(Form1.hwCard, 0, Form1.NovaStarDeviceResult.Count);
                //    #region 訊息顯示 接收卡數量
                //    if (MultiLanguage.DefaultLanguage == "en-US")
                //    {
                //        lstget1.Items.Add("Read Receiving Card... amount " + Form1.hwCards);
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //    {
                //        lstget1.Items.Add("接收卡數量 " + Form1.hwCards);
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //    {
                //        lstget1.Items.Add("接收卡数量 " + Form1.hwCards);
                //    }
                //    #endregion
                //    if (mvars.flgSelf == false) { lstget1.Items.Clear(); }
                //    for (int j = 0; j < Form1.hwCards; j++)
                //    {
                //        Form1.hwCard[j].iScr = Form1.NovaStarDeviceResult[j].SenderIndex;
                //        Form1.hwCard[j].iPort = Form1.NovaStarDeviceResult[j].PortIndex;
                //        Form1.hwCard[j].iScan = Form1.NovaStarDeviceResult[j].ScannerIndex;
                //    }
                //}
                //else
                //{
                //    mp.funSaveLogs("(Err) Read NovaStar hardware...fail，no NovaStar hardware");
                //    #region 訊息顯示  錯誤，沒有接收卡硬體
                //    if (MultiLanguage.DefaultLanguage == "en-US")
                //    {
                //        lstget1.Items.Add("Read Receiving Card...fail，no NovaStar hardware");
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //    {
                //        lstget1.Items.Add("錯誤，沒有接收卡硬體");
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //    {
                //        lstget1.Items.Add("错误，没有接收卡硬件");
                //    }
                //    goto Ex;
                //    #endregion
                //}
                #endregion
            }







            #region Bin Write
            mvars.lblCmd = "READ_JEDECID";
            mp.mSPI_READJEDECID();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
            else
            {
                mvars.flashJEDECID = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                lbl_jedecid.Text = mvars.flashJEDECID;
                string str = lbl_jedecid.Text;
                if (str.Substring((str.Length - 2), 2) == "13")
                    SPI_512KB_rdbtn.Checked = true;
                if (str.Substring((str.Length - 2), 2) == "14")
                    SPI_1MB_rdbtn.Checked = true;
                if (str.Substring((str.Length - 2), 2) == "15")
                    SPI_2MB_rdbtn.Checked = true;
                if (str.Substring(0, 2) == "EF")
                    SPI_Vender_cbx.Text = "WINBOND";
                if (str.Substring(0, 2) == "C2")
                    SPI_Vender_cbx.Text = "MXIC";
            }
            //mvars.lblCmd = "SPI_CLOCK";
            //mp.mSPI_CLK(SPI_Clk_cbx.Text);
            //mvars.lblCmd = "SPI_WRITE_EN";
            //mp.mSPI_WRITE_EN();
            mvars.lblCmd = "FUNC_STATUS";
            mp.mhFUNCSTATUS();
            if (S_WrEn_chk.Checked)
            {
                if (!((mvars.hFuncStatus & 0x02) == 0x02))
                {
                    uc_coding.lstget1.Items.Add(" SPI_STATUS = " + mvars.hFuncStatus + " <> 0x02");
                    uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                    mvars.errCode = "-3";
                    goto Ex;
                }
            }
            // flasherase
            sw.Reset();
            sw.Start();
            mvars.lblCmd = "FLASH_ERASE";
            mp.mhFLASHERASE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-7"; goto Ex; }
            while (true)
            {
                mvars.lblCmd = "FUNC_STATUS";
                mp.mhFUNCSTATUS(); 
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-8"; break; }
                if (mp.DecToBin(mvars.hFuncStatus, 8).Substring(7, 1) == "0") { break; }
                Application.DoEvents();
                uc_coding.lstget1.Items.Add(" -> Waiting for " + Form1.cmbhPB.Text + " Flash erasing... " + string.Format("{0:#.0}", sw.Elapsed.TotalSeconds));
                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                if (sw.Elapsed.TotalSeconds > 300)
                {
                    uc_coding.lstget1.Items.Add(Form1.cmbhPB.Text + " FLASH_ERASE Timeout > 300 sec");
                    mvars.errCode = "-9"; break;
                }
                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                Thread.Sleep(1000);
            }
            sw.Stop();

            if (mvars.errCode == "0")
            {
                //if (S_FlashWrRd_rdbtn.Checked == true)
                //    OUTBuffer[2 + 1] = 0x39;                //Cmd
                //else
                //    OUTBuffer[2 + 1] = 0x3E;                //Cmd

                UInt32 FlashSize = (UInt32)(mvars.ucTmp.Length);      //1 * 1024 * 1024
                UInt16 PacketSize = 2048;   //1024,2048,4096,8192,16384
                UInt32 Count = FlashSize / PacketSize;
                sw.Reset();
                sw.Start();
                if (FlashSize != (1 * 1024 * 1024))
                {
                    mvars.errCode = "-12";
                    goto Ex;
                }
                if (mvars.svnova) { mvars.isReadBack = true; }
                for (UInt32 i = 0; i < Count; i++)
                {
                    string txt36 = (i * PacketSize).ToString("X8");
                    if (i % 128 == 0)
                    {
                        uc_coding.lstget1.Items.Add(" -> " + Form1.cmbhPB.Text + " Flash writing addr" + txt36 + " @ " + Convert.ToString(string.Format("{0:#.0}", sw.Elapsed.TotalSeconds)));
                        uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
                    }
                    mvars.lblCmd = "FLASH_WRITE_" + i.ToString("0000");
                    mp.mhFLASHWRITEPAGEQIO(txt36, PacketSize, svEraseNwrNchk, 500);
                    if (mvars.svnova == false || mvars.isReadBack == true)
                    {
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mvars.errCode = "-13"; break; }
                    }
                }
            }

        #endregion Bin Write


        Ex:
            if (mvars.errCode == "0")
            {

                uc_coding.lstget1.Items.Add(" --> Done and Compare OK");
            }
            else { uc_coding.lstget1.Items.Add(" --> Error code:" + mvars.errCode); }
            uc_coding.lstget1.Items.Add("End");
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;

            //string[] svpcbaver = null;
            //int svcounts = 0;

            mvars.lCounts = 9999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;

            //string txt44;
            uc_coding.lstget1.Items.Clear();
            if (mvars.FormShow[3]) uc_coding.lstother.Items.Clear();
            mvars.flgSelf = true;
            mvars.nvBoardcast = false;
            mvars.isReadBack = true;

            //bool svpc = true;
            //if (Screen.AllScreens.GetUpperBound(0) == 0) { svpc = false; }

            #region NovaStar 有含重新填寫 Form1.lstm
            //Form1.lstm.Items.Clear();
            //Form1.svhs = "";
            //Form1.svhp = "";
            //Form1.svhc = "";
            //int svcards = 0;
            //for (byte svip = 0; svip < mvars.nvCommList.Count; svip++)
            //{
            //    if (mvars._marsCtrlSystem.UnInitialize())
            //    {
            //        mp.funSaveLogs("cBOXIDONOFF,Unloaded marsCtrlSystem.UnInitialize");
            //    }

            //    mvars._nCommPort = mvars.nvCommList[svip];
            //    Form1.nScreenCnt = 0;
            //    Form1.nSenderCnt = 0;
            //    if (mvars._marsCtrlSystem.Initialize(mvars._nCommPort, out Form1.nScreenCnt, out Form1.nSenderCnt) == false)
            //    {
            //        mvars.errCode = "-1";
            //        mp.funSaveLogs("cBOXIDONOFF," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",marsCtrlSystem.Initialize fail");
            //    }
            //    else
            //    {
            //        mvars.iSender = 0;
            //        mvars.iPort = 0;
            //        mvars.iScan = 0;

            //        #region 接收卡資訊
            //        Form1.NovaStarDeviceResult = null;
            //        svcounts = 0;
            //        do
            //        {
            //            Form1.NovaStarDeviceResult = mvars._marsCtrlSystem.GetAllScannerStatusByCom();
            //            mp.doDelayms(100);
            //            svcounts += 1;
            //            if (Form1.NovaStarDeviceResult != null) break;
            //        }
            //        while (svcounts <= 10);
            //        if (Form1.NovaStarDeviceResult != null && svcounts <= 10)
            //        {
            //            typhwCard[] svhwCard = new typhwCard[Form1.NovaStarDeviceResult.Count];
            //            Array.Clear(svhwCard, 0, Form1.NovaStarDeviceResult.Count);

            //            svpcbaver = new string[Form1.NovaStarDeviceResult.Count];

            //            if (Form1.NovaStarDeviceResult.Count > 0)
            //            {
            //                //mvars.isReadBack = true;
            //                //mvars.nvBoardcast = false;

            //                svcards += Form1.NovaStarDeviceResult.Count;
            //                if (btn.Text.ToUpper() == "OFF" || btn.Text.IndexOf("關閉") != -1 || btn.Text.IndexOf("关闭") != -1)
            //                {
            //                    //for (int i = 0; i < Form1.NovaStarDeviceResult.Count; i++)
            //                    //{
            //                    //    mvars.iSender = (byte)Form1.NovaStarDeviceResult[i].SenderIndex;
            //                    //    mvars.iPort = (byte)Form1.NovaStarDeviceResult[i].PortIndex;
            //                    //    mvars.iScan = (byte)Form1.NovaStarDeviceResult[i].ScannerIndex;

            //                    //    mvars.lblCmd = "PG_ASCTEXT";
            //                    //    mp.mPGASCTEXT(0, 0, "", 0);
            //                    //}

            //                    mvars.isReadBack = false;
            //                    mvars.nvBoardcast = true;
            //                    mvars.lblCmd = "PG_ASCTEXT";
            //                    mp.mPGASCTEXT(0, 0, "", 0);
            //                    if (svpc == false)
            //                    {
            //                        Form1.pvindex = 1;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(0);
            //                        Form1.pvindex = 21;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(4);
            //                        Form1.pvindex = 255;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(0);
            //                        Form1.pvindex = 255;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(1);
            //                        Form1.pvindex = 255;
            //                        mvars.lblCmd = "FPGA_SPI_W";
            //                        mp.mhFPGASPIWRITE(0);
            //                    }
            //                }
            //                else
            //                {
            //                    for (int svi = 0; svi < Form1.NovaStarDeviceResult.Count; svi++)
            //                    {
            //                        Form1.lstm.Items.Add("  -> " + mvars.nvCommList[svip] + ",S" + Form1.NovaStarDeviceResult[svi].SenderIndex + ",P" + Form1.NovaStarDeviceResult[svi].PortIndex + ",C" + Form1.NovaStarDeviceResult[svi].ScannerIndex);

            //                        mvars.iSender = (byte)Form1.NovaStarDeviceResult[svi].SenderIndex;
            //                        mvars.iPort = (byte)Form1.NovaStarDeviceResult[svi].PortIndex;
            //                        mvars.iScan = (byte)Form1.NovaStarDeviceResult[svi].ScannerIndex;

            //                        svhwCard[svi].iSender = (byte)Form1.NovaStarDeviceResult[svi].SenderIndex;
            //                        svhwCard[svi].iPort = (byte)Form1.NovaStarDeviceResult[svi].PortIndex;
            //                        svhwCard[svi].iScan = (byte)Form1.NovaStarDeviceResult[svi].ScannerIndex;

            //                        if (Form1.svhs == "") Form1.svhs = Form1.NovaStarDeviceResult[svi].SenderIndex.ToString();
            //                        if (Form1.svhp == "") Form1.svhp = Form1.NovaStarDeviceResult[svi].PortIndex.ToString();
            //                        if (Form1.svhc == "") Form1.svhc = Form1.NovaStarDeviceResult[svi].ScannerIndex.ToString();

            //                        Form1.pvindex = 0;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        svhwCard[svi].verFPGA = mvars.verFPGA;

            //                        int svy = 20;
            //                        Form1.pvindex = 206;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            //                        {
            //                            if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1] == "0.0.0.0") svy += 135;
            //                        }
            //                        Form1.pvindex = 207;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            //                        {
            //                            if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1] == "0.0.0.0") svy += 135;
            //                        }

            //                        if (svpc == false)
            //                        {
            //                            Form1.pvindex = 1;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(3);
            //                            Form1.pvindex = 21;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(0);
            //                            Form1.pvindex = 255;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(0);
            //                            Form1.pvindex = 255;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(1);
            //                            Form1.pvindex = 255;
            //                            mvars.lblCmd = "FPGA_SPI_W";
            //                            mp.mhFPGASPIWRITE(0);
            //                        }

            //                        Form1.pvindex = 254;
            //                        mvars.lblCmd = "FPGA_SPI_R";
            //                        mp.mhFPGASPIREAD();
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { svpcbaver[svi] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]; }
            //                        else svpcbaver[svi] = "-1";

            //                        mvars.lblCmd = "READ_MCUBOOTVER";
            //                        txt44 = (0xFFF0).ToString("X8");
            //                        mp.mhMCUBLREAD(txt44, 16);  //get verMCUB
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            //                        {
            //                            svhwCard[svi].verMCUB = mvars.verMCUB;
            //                            if (mp.IsNumeric(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)) == false) { svhwCard[svi].CurrentBootVer = 0; }
            //                            else { svhwCard[svi].CurrentBootVer = Convert.ToUInt16(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)); }
            //                        }

            //                        mvars.lblCmd = "READ_MCUAPPVER";
            //                        txt44 = (0x0007FFE0).ToString("X8");
            //                        mp.mhMCUBLREAD(txt44, 16);  //get verMCUA
            //                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            //                        {
            //                            svhwCard[svi].verMCUA = mvars.verMCUA;
            //                            if (mp.IsNumeric(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)) == false) { svhwCard[svi].CurrentAppVer = 0; }
            //                            else { svhwCard[svi].CurrentAppVer = Convert.ToUInt16(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)); }
            //                        }

            //                        uc_coding.lstget1.Items.Add((svi + 1).ToString("00") + "  Sender" + (svhwCard[svi].iSender + 1) + " Port" + (svhwCard[svi].iPort + 1) + " Card" + (svhwCard[svi].iScan + 1));
            //                        if (mvars.FormShow[3]) uc_coding.lstother.Items.Add((svi + 1).ToString("00") + ",S" + (svhwCard[svi].iSender + 1) + ",P" + (svhwCard[svi].iPort + 1) + ",C" + (svhwCard[svi].iScan + 1));
            //                        uc_coding.lstget1.Items.Add("  --> PCBA " + svpcbaver[svi]);
            //                        uc_coding.lstget1.Items.Add("  --> FPGA " + mvars.verFPGA);
            //                        uc_coding.lstget1.Items.Add("  --> " + mvars.verMCUB + ", no." + svhwCard[svi].CurrentBootVer);
            //                        uc_coding.lstget1.Items.Add("  --> " + mvars.verMCUA + ", no." + svhwCard[svi].CurrentAppVer);
            //                        uc_coding.lstget1.Items.Add("");
            //                        uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;



            //                        mp.funSaveLogs(" (ShowFW) " + "Sender," + svhwCard[svi].iSender + 1 + ",Port," + (svhwCard[svi].iPort + 1).ToString("000") + ",Card," + svhwCard[svi].iScan + 1 + ",PCBA," + svpcbaver[svi] + ",FPGA," + mvars.verFPGA + "," + svhwCard[svi].verMCUB + "," + svhwCard[svi].verMCUA);


            //                        if (btn.Text.Length > 4 &&
            //                            (btn.Text.ToUpper().Substring(0, 4) == "SHOW" || btn.Text.Substring(0, 2) == "顯示" || btn.Text.Substring(0, 2) == "显示" ||
            //                             btn.Text.ToUpper().Substring(0, 4) == "ENAB" || btn.Text.Substring(0, 2) == "開啟" || btn.Text.Substring(0, 2) == "开启")
            //                            )
            //                        {
            //                            string svs = "";
            //                            //svs = svpcbaver[svi] + "~" + mvars.verFPGA + "~" + svhwCard[svi].CurrentBootVer + "~" + svhwCard[svi].CurrentAppVer;

            //                            //byte svtl = (byte)(16 - svs.Length);
            //                            //svs = String.Format("{0,-16}", svs); //   不滿設定長度16，"-"後面補空白，沒有符號前面補空白
            //                            //svs += "172.16.129.111";

            //                            svs = mvars.nvCommList[svip];

            //                            mvars.lblCmd = "PG_ASCTEXT";
            //                            //mp.mPGASCTEXT(5, 20, svpcbaver[svi] + "~" + mvars.verFPGA + "~" + svhwCard[svi].CurrentBootVer + "~" + svhwCard[svi].CurrentAppVer, 1);

            //                            if (svy > 255)
            //                            {
            //                                mp.mPGASCTEXT(15, 255, mvars.nvCommList[svip], 1);
            //                                mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
            //                                mp.mhFPGASPIWRITEasc(svs, "S" + (svhwCard[svi].iSender + 1) + ",P" + (svhwCard[svi].iPort + 1) + ",C" + (svhwCard[svi].iScan + 1), 1);
            //                                Form1.pvindex = 116;
            //                                mvars.lblCmd = "FPGA_SPI_W";
            //                                mp.mhFPGASPIWRITE(svy);
            //                            }
            //                            else
            //                            {
            //                                mp.mPGASCTEXT(15, svy, mvars.nvCommList[svip], 1);
            //                                mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
            //                                mp.mhFPGASPIWRITEasc(svs, "S" + (svhwCard[svi].iSender + 1) + ",P" + (svhwCard[svi].iPort + 1) + ",C" + (svhwCard[svi].iScan + 1), 1);
            //                            }
            //                        }
            //                    }
            //                }
            //                mp.funSaveLogs("  -> " + mvars.nvCommList[svip] + ",hwCards," + Form1.NovaStarDeviceResult.Count);
            //            }
            //            else
            //            {
            //                mvars.errCode = "-99";
            //                if (mvars.flgSelf)
            //                {
            //                    uc_coding.lstget1.Items.Add("  -> " + mvars.nvCommList[svip] + ",No Connectors");
            //                    uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            //                }
            //                mp.funSaveLogs("cBOXIDONOFF," + mvars.nvCommList[svip] + ",No Connectors");
            //            }
            //        }
            //        else
            //        {
            //            mvars.errCode = "-5";
            //            if (mvars.flgSelf)
            //            {
            //                uc_coding.lstget1.Items.Add("  -> " + "NovaStarDeviceResult,fail");
            //                uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;
            //            }
            //            mp.funSaveLogs("cBOXIDONOFF,NovaStarDeviceResult,fail");
            //        }
            //        #endregion
            //        Form1.lstm.Items.Add(" ↑ " + svip + "," + mvars.nvCommList[svip] + ",Sender accumulation," + Form1.nSenderCnt + ",hwCard accumulation," + Form1.NovaStarDeviceResult.Count + "," + Form1.svhs + "," + Form1.svhp + "," + Form1.svhc);
            //    }
            //}
            #endregion NovaStar 有含重新填寫 Form1.lstm


        Ex:
            //uc_coding.lstget1.Items.Add("Receiving card amount " + svcards);
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;

            //if (btn.Text.ToUpper().Substring(0, 4) == "DISA") btn.Text = "Enable Mapping";
            //else if (btn.Text.ToUpper().Substring(0, 4) == "ENAB") btn.Text = "Disable Mapping";
            //else if (btn.Text.Substring(0, 2) == "關閉") btn.Text = "開啟Mapping";
            //else if (btn.Text.Substring(0, 2) == "关闭") btn.Text = "开启Mapping";
            //else if (btn.Text.Substring(0, 2) == "開啟") btn.Text = "關閉Mapping";
            //else if (btn.Text.Substring(0, 2) == "开启") btn.Text = "关闭Mapping";

            btn.Enabled = true;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (FPGA_Reg_Mode_rbtn.Checked)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") MessageBox.Show("Please select FPGA SPI Mode");
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") MessageBox.Show("回讀請指定 FPGA SPI 模式");
                else if (MultiLanguage.DefaultLanguage == "zh-CN") MessageBox.Show("回读请指定 FPGA SPI 模式");
                btn.Enabled = true;
                return;
            }

            //lstget1.SetBounds(21, 134, 636, 294);
            #region config
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mvars.Break = false;
            mvars.lCounts = 19999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.errCode = "0";
            //int i;
            mvars.lCount = 0;
            mvars.strReceive = "";
            mvars.flgDelFB = true;
            UInt32 FlashSize;
            ushort PacketSize;
            UInt32 Count;
            lstget1.Items.Clear();
            mvars.flgSelf = true;

            mvars.c12aflashitem = 1;    //0:CB Flash 1:XB Flash
            byte[] FlashRd_Arr = new byte[1];

            #endregion config












            if (mvars.svnova)
            {
                if (chkNVBC.Checked)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") MessageBox.Show("Please select cabinet");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") MessageBox.Show("回讀請指定箱體");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") MessageBox.Show("回读请指定箱体");
                    btn.Enabled = true;
                    return;
                }
                //mvars.iSender = (byte)(numericUpDown_sender.Value - 1);
                //mvars.iPort = (byte)(numericUpDown_port.Value - 1);
                //mvars.iScan = (ushort)(numericUpDown_scan.Value - 1);

                #region re-get Cards non-necessary
                //Form1.NovaStarDeviceResult = null;
                //int svcounts = 0;
                //do
                //{
                //    Form1.NovaStarDeviceResult = mvars._marsCtrlSystem.GetAllScannerStatusByCom();
                //    mp.doDelayms(100);
                //    svcounts += 1;
                //    if (Form1.NovaStarDeviceResult != null) break;
                //}
                //while (svcounts <= 10);
                //if (Form1.NovaStarDeviceResult != null && svcounts <= 10)
                //{
                //    lstget1.Items.Clear();
                //    Form1.hwCards = Form1.NovaStarDeviceResult.Count;
                //    Array.Resize(ref Form1.hwCard, Form1.NovaStarDeviceResult.Count);
                //    Array.Clear(Form1.hwCard, 0, Form1.NovaStarDeviceResult.Count);
                //    #region 訊息顯示 接收卡數量
                //    if (MultiLanguage.DefaultLanguage == "en-US")
                //    {
                //        lstget1.Items.Add("Read Receiving Card... amount " + Form1.hwCards);
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //    {
                //        lstget1.Items.Add("接收卡數量 " + Form1.hwCards);
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //    {
                //        lstget1.Items.Add("接收卡数量 " + Form1.hwCards);
                //    }
                //    #endregion
                //    if (mvars.flgSelf == false) { lstget1.Items.Clear(); }

                //    if (Form1.hwCards == 0)
                //    {
                //        lstget1.Items.Add(" --> 沒有接收卡");
                //        lst_get1.Items.Add("");
                //        mvars.errCode = "-1"; goto Ex;
                //    }
                //    if (btn.Tag.ToString().ToUpper() == "FPGA") { btn_nFPGAR.Enabled = false; btn_nFPGABREAK.Enabled = true; }
                //    else if (btn.Tag.ToString().ToUpper() == "DMR") { btn_nDMRR.Enabled = false; btn_nDMRBREAK.Enabled = true; }
                //    for (int j = 0; j < Form1.hwCards; j++)
                //    {
                //        Form1.hwCard[j].iSender = Form1.NovaStarDeviceResult[j].SenderIndex;
                //        Form1.hwCard[j].iPort = Form1.NovaStarDeviceResult[j].PortIndex;
                //        Form1.hwCard[j].iScan = Form1.NovaStarDeviceResult[j].ScannerIndex;
                //    }
                //}
                //else
                //{
                //    mp.funSaveLogs("(Err) Read NovaStar hardware...fail，no NovaStar hardware");
                //    #region 訊息顯示  錯誤，沒有接收卡硬體
                //    if (MultiLanguage.DefaultLanguage == "en-US")
                //    {
                //        lstget1.Items.Add("Read Receiving Card...fail，no NovaStar hardware");
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //    {
                //        lstget1.Items.Add("錯誤，沒有接收卡硬體");
                //    }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //    {
                //        lstget1.Items.Add("错误，没有接收卡硬件");
                //    }
                //    goto Ex;
                //    #endregion
                //}
                #endregion
            }

            #region callOnClick(FPGA_FlashPathSend_btn);

            //for (i = 0; i < FPGA_FlashMode_cbx.Items.Count; i++)
            //{
            //    if (FPGA_FlashMode_cbx.Text == FPGA_FlashMode_cbx.Items[i].ToString()) { break; }
            //}
            //if (i >= FPGA_FlashMode_cbx.Items.Count)
            //{
            //    if (MultiLanguage.DefaultLanguage == "en-US") MessageBox.Show("Please select LB");
            //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") MessageBox.Show("回讀請指定燈板");
            //    else if (MultiLanguage.DefaultLanguage == "zh-CN") MessageBox.Show("回读请指定灯板");
            //}

            //mvars.lblCmd = "SPI_FLASHPATH";
            //if (FPGA_Reg_Mode_rbtn.Checked)
            //    mp.mSPI_FLASHPATHSEL(16);                                       //SPI Flash Path Select
            //else
            //    mp.mSPI_FLASHPATHSEL((byte)i);   //SPI Flash Path Select
            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
            #endregion

            #region callOnClick(SPI_ReadJedecId_btn);
            lbl_jedecid.Text = "";
            mvars.lblCmd = "READ_JEDECID";
            mp.mSPI_READJEDECID();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
            else
            {
                lbl_jedecid.Text = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                string str = lbl_jedecid.Text;
                if (str.Substring((str.Length - 2), 2) == "13")
                    SPI_512KB_rdbtn.Checked = true;
                if (str.Substring((str.Length - 2), 2) == "14")
                    SPI_1MB_rdbtn.Checked = true;
                if (str.Substring((str.Length - 2), 2) == "15")
                    SPI_2MB_rdbtn.Checked = true;
                if (str.Substring(0, 2) == "EF")
                    SPI_Vender_cbx.Text = "WINBOND";
                if (str.Substring(0, 2) == "C2")
                    SPI_Vender_cbx.Text = "MXIC";
            }
            #endregion

            mvars.errCode = "0";
            FlashSize = 1 * 1024 * 1024;
            uc_coding.flashreadsize = 2048;
            PacketSize = uc_coding.flashreadsize;
            Count = FlashSize / PacketSize;
            Array.Resize(ref FlashRd_Arr, (int)FlashSize);
            sw.Reset();
            sw.Start();
            mvars.nvBoardcast = false;
            //for (i = 0; i < Count; i++)
            //{
            //    string textBox36 = (i * PacketSize).ToString("X8");
            //    lstget1.Items.Add(String.Format("{0:00}", ((i * 100) / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
            //    lstget1.TopIndex = lstget1.Items.Count - 1;
            //    Application.DoEvents();
            //    mvars.lblCmd = "FLASH_READ_" + i.ToString("0000");
            //    mp.mhFLASHREAD(textBox36, PacketSize);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
            //    {
            //        lst_get1.Items.Add(" -> " + (mvars.iSender + 1) + "," + (mvars.iPort + 1) + "," + (mvars.iScan + 1) + "," + FPGA_FlashMode_cbx.Text +
            //            " FLASH_READ_" + i.ToString("0000") + " fail @ " + String.Format("{0:00}", ((i * 100) / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
            //        lst_get1.Items.Add("");
            //        mvars.errCode = "-1";
            //        break;
            //    }
            //    else
            //    {
            //        for (UInt32 m = 0; m < PacketSize; m++) { FlashRd_Arr[i * PacketSize + m] = mvars.gFlashRdPacketArr[m]; }
            //    }
            //    if (mvars.Break)
            //    {
            //        lst_get1.Items.Add(" -> " + (mvars.iSender + 1) + "," + (mvars.iPort + 1) + "," + (mvars.iScan + 1) + "," + FPGA_FlashMode_cbx.Text +
            //            " FLASH_READ_" + i.ToString("0000") + " BREAK @ " + String.Format("{0:00}", ((i * 100) / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
            //        lst_get1.Items.Add("");
            //        mvars.errCode = "-2";
            //        break;
            //    }
            //}




        Ex:
            if (mvars.errCode == "0")
            {
                string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\MiniLedLog\\S" + numericUpDown_sender.Value + ",P" + numericUpDown_port.Value + ",C" + numericUpDown_scan.Value + "," + FPGA_FlashMode_cbx.Text + "Read.bin";

                //Check FolderPath Exist
                if (!Directory.Exists("C:\\Users\\" + Environment.UserName + "\\Documents\\MiniLedLog"))
                    Directory.CreateDirectory("C:\\Users\\" + Environment.UserName + "\\Documents\\MiniLedLog");

                List<byte> Wcontent = FlashRd_Arr.ToList();
                int num = 0;
                num = Wcontent.Count();
                if (num > 0)
                {
                    byte[] buffer = Wcontent.ToArray();
                    FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    fileStream.Write(buffer, 0, num);
                    fileStream.Close();
                }
                lst_get1.Items.Add(" -> " + path);
                lst_get1.TopIndex = lst_get1.Items.Count - 1;
            }
            else { uc_coding.lstget1.Items.Add(" --> Error code:" + mvars.errCode); }
            uc_coding.lstget1.Items.Add("End," + Convert.ToString(string.Format("{0:#.0}", sw.Elapsed.TotalMinutes)) + "m");
            uc_coding.lstget1.Items.Add("");
            uc_coding.lstget1.TopIndex = uc_coding.lstget1.Items.Count - 1;

            mvars.nvBoardcast = true;
        }


        private void numericUpDown_ip_Click(object sender, EventArgs e)
        {
            
        }


        private void numericUpDown_sender_Click(object sender, EventArgs e)
        {
            //mvars.iSender = (byte)(numericUpDown_sender.Value - 1);
            //mvars.iPort = 0; numericUpDown_port.Value = mvars.iPort + 1;
            //mvars.iScan = 0; numericUpDown_scan.Value = mvars.iScan + 1;
        }

        private void numericUpDown_port_Click(object sender, EventArgs e)
        {
            //mvars.iPort = (byte)(numericUpDown_port.Value - 1);
            //mvars.iScan = 0; numericUpDown_scan.Value = mvars.iScan + 1;
        }

        private void numericUpDown_scan_Click(object sender, EventArgs e)
        {
            //mvars.iScan = (UInt16)(numericUpDown_scan.Value - 1);
        }

        private void numericUpDown_ip_ValueChanged(object sender, EventArgs e)
        {
            //chkNVBC.Checked = true;
            //chk_NVBC_Click(null, null);
            //if (mvars.demoMode) 
            //{
            //    if (numericUpDown_ip.Value == 0) mvars.tabscrControl.TabPages[0].Text = "Demo 所有显示屏";
            //    else mvars.tabscrControl.TabPages[0].Text = "Demo IP110.11.11." + numericUpDown_ip.Value + "-显示屏";
            //}
            //else 
            //{
            //    if (numericUpDown_ip.Value == 0)
            //    {
            //        if (MultiLanguage.DefaultLanguage == "en-US")
            //        {
            //            mvars.tabscrControl.TabPages[0].Text = "All Screen";
            //        }
            //        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //        {
            //            mvars.tabscrControl.TabPages[0].Text = "所有顯示屏";
            //        }
            //        else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //        {
            //            mvars.tabscrControl.TabPages[0].Text = "所有显示屏";
            //        }
            //    }
            //    else
            //    {
            //        uc_box.svIPSel = (byte)(numericUpDown_ip.Value - 1);
            //        mvars.iSender = 0; numericUpDown_sender.Value = mvars.iSender + 1;
            //        mvars.iPort = 0; numericUpDown_port.Value = mvars.iPort + 1;
            //        mvars.iScan = 0; numericUpDown_scan.Value = mvars.iScan + 1;                   
            //        if (MultiLanguage.DefaultLanguage == "en-US")
            //        {
            //            mvars.tabscrControl.TabPages[0].Text = mvars.nvCommList[uc_box.svIPSel] + "-Screen";
            //        }
            //        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //        {
            //            mvars.tabscrControl.TabPages[0].Text = mvars.nvCommList[uc_box.svIPSel] + "-顯示屏";
            //        }
            //        else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //        {
            //            mvars.tabscrControl.TabPages[0].Text = mvars.nvCommList[uc_box.svIPSel] + "-显示屏";
            //        }
            //    }
            //}
        }

        private void btn_nMCUBREAK_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (MultiLanguage.DefaultLanguage == "en-US")
            {
                txt_nMCUFileNameFull.Text = "User BREAK ! The process will stop at the this card finish";   
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            {
                txt_nMCUFileNameFull.Text = "使用者中斷 ! 燒錄程序會在這一接收卡完成後停止";
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CN")
            {
                txt_nMCUFileNameFull.Text = "使用者中断 ! 烧录程序会在这一接收卡完成后停止";
            }
            mvars.Break = true;
            btn.Enabled = false;
        }

        private void lbl_jedecid_Click(object sender, EventArgs e) { }
        private void lbl_jedecid_DoubleClick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabpage_bmp || tabControl1.SelectedTab == tabpage_dmr)
            {
                mp.markreset(999, false);
                if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

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

                //btn.Enabled = true;
                if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
            }
            else if (tabControl1.SelectedTab == tabpage_mcu)
            {

            }
            else if (tabControl1.SelectedTab == tabpage_bmp)
            {

            }
        }

        private void cmb_FlashSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_FlashSel.SelectedIndex == 0) { mvars.flashselQ = 0; lbl_FlashSel.Text = "FlashSel"; }
            else
            {
                if ((mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06"))
                {
                    if (tabControl1.SelectedTab == tabpage_bmp) mvars.flashselQ = (byte)cmb_FlashSel.SelectedIndex;
                    else if (tabControl1.SelectedTab == tabpage_dmr) mvars.flashselQ = (byte)(cmb_FlashSel.SelectedIndex + 2);
                    lbl_FlashSel.Text = "FlashSel(" + mvars.flashselQ + ")";
                }
            }
        }

        private void btn_browseDMR_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && btn_browseDMR.BackColor == System.Drawing.Color.Transparent) btn_browseDMR.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            else if (e.Button == MouseButtons.Right && btn_browseDMR.BackColor == System.Drawing.Color.FromArgb(255, 255, 128)) btn_browseDMR.BackColor = System.Drawing.Color.Transparent;
        }

        private void btn_LT8668L_Click(object sender, EventArgs e)
        {
            //string svs = "";

            OffsetAddr.Text = "060000";

            //MessageBox.Show("16 " + Convert.ToUInt32(OffsetAddr.Text, 16));
            // = 393216
            //MessageBox.Show("X6 " + Convert.ToUInt32(OffsetAddr.Text, 16).ToString("X6"));
            // = 060000
        }

        private void btn_browseLT_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "請選擇 LT8668 升級檔案所在資料夾";
            dialog.SelectedPath = "c:";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "資料夾路徑不能為空", "提示");
                    return;
                }
                lstget1.Items.Clear();
                lst_filepathfull.Items.Clear();
                cmb_box.Items.Clear();
                chk_LT8668_FlashAddr.Checked = false;
                chk_Addr_54000.Checked = false;
                chk_OSD_FlashAddr.Checked = false;
                chk_OSD1_FlashAddr.Checked = false;
                //dirSearch(dialog.SelectedPath);
                // List<string> filelist = new List<string>();
                // 
                string sDir = dialog.SelectedPath;
                try
                {
                    //先找出所有目錄
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        //先針對目錄的檔案做處理
                        foreach (string f in Directory.GetFiles(d, "*.hex|FontData.txt|Mycmd_sram.txt|Myfg0fg1bg_sram.txt"))
                        {
                            Console.WriteLine(f);
                            //filelist.Add(f);
                            lst_filepathfull.Items.Add(f);
                            lstget1.Items.Add(f.Split('\\')[f.Split('\\').Length - 1]);
                            cmb_box.Items.Add(lstget1.Items[lstget1.Items.Count - 1].ToString().Split('.')[0]);
                        }
                        //此目錄完再針對每個子目錄做處理
                        dirSearch(d);
                    }
                }
                catch (System.Exception excpt)
                {
                    Console.WriteLine(excpt.Message);
                }
                if (lstget1.Items.Count <= 0)
                {
                    DirectoryInfo di = new DirectoryInfo(sDir);
                    FileInfo[] afi = di.GetFiles("*.*");
                    grp_path.Text = sDir + @"\";
                    string f;
                    IList<string> list = new List<string>();
                    for (int i = 0; i < afi.Length; i++)
                    {
                        f = afi[i].Name.ToLower();
                        //if (f.EndsWith(".rmvb") || f.EndsWith(".rm") || f.EndsWith(".avi") || f.EndsWith(".mp4"))
                        //{
                        //    list.Add(f);
                        //}
                        if (f.IndexOf(".hex", 0) != -1 && chk_LT8668_FlashAddr.Checked == false)
                        {
                            chk_LT8668_FlashAddr.Checked = true;
                            chk_LT8668_FlashAddr.Text = f;
                            lbl_LT8668.Text = f;
                        }
                        else if (f.IndexOf("myfg0fg1bg_sram.txt", 0) != -1)
                        {
                            chk_Addr_54000.Checked = true;
                            chk_Addr_54000.Text = f;
                            lbl_Addr_54000.Text = f;
                        }
                        else if (f.IndexOf("mycmd_sram.txt", 0) != -1)
                        {
                            chk_OSD_FlashAddr.Checked = true;
                            chk_OSD_FlashAddr.Text = "OSD(" + f + ")";
                            lbl_OSD_FlashAddr.Text = f;
                        }
                        else if (f.IndexOf("fontdata.txt", 0) != -1)
                        {
                            chk_OSD1_FlashAddr.Checked = true;
                            chk_OSD1_FlashAddr.Text = "OSD1(" + f + ")";
                            lbl_OSD1_FlashAddr.Text = f;
                        }

                    }

                }
                lstget1.Refresh();


                if (lstget1.Items.Count > 0) { btn_nDMRdraw.Enabled = true; btn_nDMRbmp.Enabled = true; }
                else { btn_nDMRdraw.Enabled = false; btn_nDMRbmp.Enabled = false; }
            }
        }


        public static string OffsetAddrText;
        private void btn_LT8668W_Click(object sender, EventArgs e)
        {
            /*
            if (LT8668_FlashAddr_rbtn.Checked)
                OffsetAddr.Text = "000000";
            else if (OSD_FlashAddr_rbtn.Checked)
                OffsetAddr.Text = "060000";
            else if (OSD1_FlashAddr_rbtn.Checked)
                OffsetAddr.Text = "068000";
            else if (EDID_FlashAddr_rbtn.Checked)
                OffsetAddr.Text = "070000";
            else if (Addr_54000_rbtn.Checked)
                OffsetAddr.Text = "054000";
            */
            if (chk_LT8668_FlashAddr.Checked)
            {
                OffsetAddrText = "000000";
                mp.cLT8668Write(lst_get1, grp_path.Text + lbl_LT8668.Text, OffsetAddrText, FastRd_chk.Checked, Convert.ToUInt16(RdWr_Size.Text), false);
            }
            if (chk_Addr_54000.Checked)
            {
                OffsetAddrText = "054000";
                mp.cLT8668Write(lst_get1, grp_path.Text + lbl_Addr_54000.Text, OffsetAddrText, FastRd_chk.Checked, Convert.ToUInt16(RdWr_Size.Text), true);
            }
            if (chk_OSD_FlashAddr.Checked)
            {
                OffsetAddrText = "060000";
                mp.cLT8668Write(lst_get1, grp_path.Text + lbl_OSD_FlashAddr.Text, OffsetAddrText, FastRd_chk.Checked, Convert.ToUInt16(RdWr_Size.Text), false);
            }
            if (chk_OSD1_FlashAddr.Checked)
            {
                OffsetAddrText = "068000";
                mp.cLT8668Write(lst_get1, grp_path.Text + lbl_OSD1_FlashAddr.Text, OffsetAddrText, FastRd_chk.Checked, Convert.ToUInt16(RdWr_Size.Text), false);
            }
            if (chk_EDID_FlashAddr.Checked)
            {
                OffsetAddrText = "070000";
                mp.cLT8668Write(lst_get1, grp_path.Text + chk_EDID_FlashAddr.Text, OffsetAddrText, FastRd_chk.Checked, Convert.ToUInt16(RdWr_Size.Text), true);
            }

            

        }

        private void btn_LT8668ver_Click(object sender, EventArgs e)
        {
            Byte[] arr = new byte[2]; //Byte[] rd_arr = new byte[2];
            uc_box.LT8668rd_arr = new byte[1];
            byte VerHi = 0, VerLo = 0;

            mvars.lblCmd = "LT8668_Bin_WrRd";
            arr[0] = 0x82; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.ReplaceAt(mvars.errCode, 1, "1"); }
            else
            {
                VerHi = mvars.ReadDataBuffer[7];
                mvars.lblCmd = "LT8668_Bin_WrRd";
                arr[0] = 0x83; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) VerHi = 0;
                else VerLo = mvars.ReadDataBuffer[7];
            }
            lstget1.Items.Add("LT866 ver: " + VerHi + "." + VerLo);
        }
    }
}
