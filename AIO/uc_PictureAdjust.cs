using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AIO
{
    public partial class uc_PictureAdjust : UserControl
    {
        public static bool svNoExtScr = false;
        public static bool[] svPG;
        public static string[] svuiuserDef = new string[40];
        public static string[] svuiuser = new string[40];
        public static float[] pidxy = new float[8];
        public static float[] userxy = new float[8];
        public static int pidcct = 8300;
        public static int userCCT = 8300;
        public static bool cctmanual = false;
        public static string cgkw = "";     //Color Gamut KeyWord
        public static int[] pidgma = { 512, 1024, 2048, 4080 };
        public static int[] usergma = { 512, 1024, 2048, 4080 };
        public static float pidgv = 2.2f;
        public static float usergv = 2.2f;
        public static float[] dfusergray = new float[] { 0, 512, 1024, 2048, 4080 };
        public static double[] regusergray = new double[] { 512, 1024, 2048, 4080 };
        public static int svLmaxdelta = 0;
        public static int svLmindelta = 0;
        public static string lvgraymax = "1024";

        public static float rratio = 0;
        public static float gratio = 0;
        public static float bratio = 0;

        public static float xr = 0;
        public static float yr = 0;
        public static float xg = 0;
        public static float yg = 0;
        public static float xb = 0;
        public static float yb = 0;
        public static float xw = 0;
        public static float yw = 0;
        public static float xct = 0;
        public static float yct = 0;

        public static float[] conPAL =
        {
            0.6400f, 0.3300f,
            0.2900f, 0.6000f,
            0.1500f, 0.0600f,
            0.3127f, 0.3290f
        };
        public static float[] conNTSC =
        {
            0.6700f, 0.3300f,
            0.2100f, 0.7100f,
            0.1400f, 0.0800f,
            0.3101f, 0.3162f
        };
        byte svGmaENreg = 2;
        byte svBriENreg = 3;
        byte svCGENreg = 9;
        byte svGMAreg = 10;
        byte svCGREDXreg = 30;
        byte svCGREDYreg = 31;
        byte svCGGRNXreg = 32;
        byte svCGGRNYreg = 33;
        byte svCGBLUXreg = 34;
        byte svCGBLUYreg = 35;
        byte svCGWHIXreg = 36;
        byte svCGWHIYreg = 37;
        byte svCCTreg = 38;
        byte svUserRreg = 15;
        byte svUserGreg = 16;
        byte svUserBreg = 17;
        byte svGAMUTreg = 39;
        byte svMode = 101;
        byte svAddr = 102;
        byte svWdata = 103;
        byte svRdata = 104;
        byte svbrigValue = 1;
        byte svUserGMA1 = 11;
        byte svUserGMA2 = 12;
        byte svUserGMA3 = 13;
        byte svUserGMA4 = 14;
        byte svCGMATA1 = 21;
        byte svCGMATA2 = 22;
        byte svCGMATA3 = 23;
        byte svCGMATA4 = 24;
        byte svCGMATA5 = 25;
        byte svCGMATA6 = 26;
        byte svCGMATA7 = 27;
        byte svCGMATA8 = 28;
        byte svCGMATA9 = 29;

        List<string> lstR62 = new List<string>();
        byte svENG_GMA_EN = 0;
        byte svENG_BRI_EN = 0;

        public uc_PictureAdjust()
        {
            InitializeComponent();
        }

        private void uc_PictureAdjust_Load(object sender, EventArgs e)
        {
            uclan.SetLang(MultiLanguage.DefaultLanguage, this);
            int W = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int H = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            mp.checkExtendScreen(ref W, ref H);
            if (mvars.ratioX == 1 && mvars.ratioY == 1) { svNoExtScr = true; }

            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                svGmaENreg = 1;
                svBriENreg = 2;
                svCGENreg = 3;

                svGmaENreg = 5;
                svBriENreg = 6;

                svUserGMA1 = 10;
                svUserGMA2 = 11;
                svUserGMA3 = 12;
                svUserGMA4 = 13;

                svUserRreg = 14;
                svUserGreg = 15;
                svUserBreg = 16;

                svCGMATA1 = 17;
                svCGMATA2 = 18;
                svCGMATA3 = 19;
                svCGMATA4 = 20;
                svCGMATA5 = 21;
                svCGMATA6 = 22;
                svCGMATA7 = 23;
                svCGMATA8 = 24;
                svCGMATA9 = 25;

                svCGREDXreg = 26;
                svCGREDYreg = 27;
                svCGGRNXreg = 28;
                svCGGRNYreg = 29;
                svCGBLUXreg = 30;
                svCGBLUYreg = 31;
                svCGWHIXreg = 32;
                svCGWHIYreg = 33;
                svCCTreg = 34;
                svGMAreg = 35;
                svGAMUTreg = 39;

                svMode = 32;
                svAddr = 33;
                svWdata = 34;
                svRdata = 35;

                svbrigValue = 8;    //primary未使用register
            }

            //int svv = 0;
            mvars.flgSelf = true;

            mvars.nvBoardcast = false;
            mvars.isReadBack = true;

            string svs = "";

            mp.markreset(999, false); mvars.flgSelf = true;
            if (mvars.demoMode == false)
            {
                string[] svstr = new string[2];
                if (mvars.deviceID.Substring(0,2) == "05")
                {
                    //Read
                    Form1.pvindex = 32;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //Addr
                    Form1.pvindex = 33;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //Write mode
                    //RData
                    Form1.pvindex = svRdata;
                    mvars.lblCmd = "FPGA_SPI_R";
                    mp.mhFPGASPIREAD();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    else 
                        svstr = new string[] { (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]).ToString(), (mvars.ReadDataBuffer[8 + 1] * 256 + mvars.ReadDataBuffer[9 + 1]).ToString() };
                }
                else
                {
                    //Read Mode
                    Form1.pvindex = svMode;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(1);   //1=Read , 0=Write
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //Addr
                    Form1.pvindex = svAddr;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(0);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //RData
                    Form1.pvindex = svRdata;
                    mvars.lblCmd = "FPGA_SPI_R";
                    mp.mhFPGASPIREAD();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    else
                        svstr = new string[] { (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]).ToString() };
                }
                if (svstr[svstr.Length - 1] == "128")
                {
                    uc_PictureAdjust.lvgraymax = "1024";
                    mvars.nualg = true;
                    Array.Resize(ref svuiuser, 40); Array.Resize(ref svuiuserDef, 40);
                    for (int svi = 0; svi < uc_PictureAdjust.svuiuserDef.Length; svi++) uc_PictureAdjust.svuiuserDef[svi] = mvars.uiregadr_default[svi].Split(',')[2];
                    if (mvars.deviceID.Substring(0, 2) == "02")
                    {
                        //svv = 10;
                        Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_L232.Length);
                        mvars.uiregadr_default = mvars.uiregadr_default_L232;
                        mvars.struiregadrdef = "";
                        for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                        {
                            mvars.struiregadrdef += "~" + mvars.uiregadr_default[i].Split(',')[0] + "," + mvars.uiregadr_default[i].Split(',')[2];
                        }
                        mvars.struiregadrdef = mvars.struiregadrdef.Substring(1, mvars.struiregadrdef.Length - 1);
                        //for (int i = 0; i < 11; i++)
                        //{
                        //    mvars.uiregadrBCgma += "~" + mvars.uiregadr_default[i].Split(',')[0] + "," + mvars.uiregadr_default[i].Split(',')[2];
                        //}
                        //mvars.uiregadrBCgma = mvars.uiregadrBCgma.Substring(1, mvars.uiregadrBCgma.Length - 1);
                    }
                }
                else
                {
                    mvars.nualg = false;
                    Array.Resize(ref svuiuser, 8); Array.Resize(ref svuiuserDef, 8);

                    //svv = 0;
                    uc_PictureAdjust.lvgraymax = "255";

                    Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_L203.Length);
                    mvars.uiregadr_default = mvars.uiregadr_default_L203;
                    mvars.struiregadrdef = "";
                    for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                    {
                        mvars.struiregadrdef += "~" + mvars.uiregadr_default[i].Split(',')[0] + "," + mvars.uiregadr_default[i].Split(',')[2];
                    }
                    mvars.struiregadrdef = mvars.struiregadrdef.Substring(1, mvars.struiregadrdef.Length - 1);

                    //for (int i = 0; i < 11; i++)
                    //{
                    //    mvars.uiregadrBCgma += "~" + mvars.uiregadr_default[i].Split(',')[0] + "," + mvars.uiregadr_default[i].Split(',')[2];
                    //}
                    //mvars.uiregadrBCgma = mvars.uiregadrBCgma.Substring(1, mvars.uiregadrBCgma.Length - 1);
                }

                //高級設置
                lbl_showdetail.Enabled = mvars.nualg;
                //mp.cUIUSERREAD(ref svuiuser);
                mvars.lblCmd = "UIREGRAD_READ";
                mp.mUIREGARDRm(0, svuiuserDef.Length);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
                else
                {
                    for (int svi = 0; svi < svuiuserDef.Length; svi++)
                    {
                        svuiuser[svi] = (mvars.ReadDataBuffer[6 + svi * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + svi * 2 + 1]).ToString();
                        if ((mvars.ReadDataBuffer[6 + svi * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + svi * 2 + 1]) == (mvars.ReadDataBuffer[6 + svi * 2 + svuiuserDef.Length * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + svi * 2 + svuiuserDef.Length * 2 + 1]))
                        {
                            svuiuser[svi] = (mvars.ReadDataBuffer[6 + svi * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + svi * 2 + 1]).ToString();
                        }
                        else 
                            svuiuser[svi] = svuiuserDef[svi];
                    }
                    //if (mvars.deviceID.Substring(0, 2) == "05" && svuiuser[svGAMUTreg] == svuiuser[svGMAreg])     //20230530 順源操作默認值後發現問題 debug
                    if (mvars.deviceID.Substring(0, 2) == "05")
                    {
                        #region 回讀drop開關
                        mvars.lblCmd = "MCU_FLASH_R62000";
                        mp.mhMCUFLASHREAD("00062000", 8192);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                        {
                            mvars.errCode = "-3";
                            Form1.tslblStatus.Text = "0x62000 rd MCU Flash fail";
                        }
                        else
                        {
                            if (mvars.strR62K.Length > 1)
                            {
                                lstR62 = new List<string>();
                                lstR62.AddRange(mvars.strR62K.Split('~'));
                                svENG_GMA_EN = Convert.ToByte(lstR62[svGmaENreg].Split(',')[1]);     //Drop  User[5]
                                svENG_BRI_EN = Convert.ToByte(lstR62[svBriENreg].Split(',')[1]);     //Brig  User[6]
                            }
                            else svuiuser[svGAMUTreg] = "0";
                            //lbl_mcuR64000click.Text = "< ";
                            //Form1.tslblStatus.Text = "0x64000 rd MCU Flash items：" + (mvars.lstmcuR64000.Items.Count).ToString();
                        }
                        #endregion

                        #region 回讀 0x64000
                        mvars.lblCmd = "MCU_FLASH_R64000";
                        mp.mhMCUFLASHREAD("00064000", 8192);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                        {
                            mvars.errCode = "-3";
                            Form1.tslblStatus.Text = "0x64000 rd MCU Flash fail";
                        }
                        else
                        {
                            if (mvars.strR64K.Length > 1)
                            {
                                //List<string> lst = new List<string>(new string[mvars.strR64K.Split('~').Length]);
                                List<string> lst = new List<string>();
                                lst.AddRange(mvars.strR64K.Split('~'));
                                //string[] svr64 = new string[svuiuser.Length];
                                //for (int svi = 0; svi < svuiuser.Length; svi++)
                                //{
                                //    svr64[svi] = lst[svi].Split(',')[1];
                                //}
                                uc_PictureAdjust.svuiuser[svGAMUTreg] = lst[svGAMUTreg].Split(',')[1];
                            }
                            else svuiuser[svGAMUTreg] = "0";
                            //lbl_mcuR64000click.Text = "< ";
                            //Form1.tslblStatus.Text = "0x64000 rd MCU Flash items：" + (mvars.lstmcuR64000.Items.Count).ToString();
                        }
                        #endregion 回讀 0x64000

                    }
                }
            }
            else
            {
                if (mvars.nualg == true) { Array.Resize(ref svuiuser, 40); Array.Resize(ref svuiuserDef, 40); }
                else { Array.Resize(ref svuiuser, 8); Array.Resize(ref svuiuserDef, 8); }
            }
            mvars.flgDelFB = true;








            if (mvars.nualg)
            {
                #region svuiuser Default [0]~[39]
                svuiuserDef[0] = "128";     //辨識用(read only)
                svuiuserDef[1] = "1";       //單屏整面Gamma可調(使用者模式) (on : 1 / off : 0)
                svuiuserDef[2] = "1";       //單屏整面亮度可調(使用者模式) (on : 1 / off : 0)
                svuiuserDef[3] = "1";       //色域調整開關(使用者模式) (on : 1 / off : 0)
                svuiuserDef[4] = "0";       //White-tracking PG mode啟動(工程模式) (on : 1 / off : 0)
                svuiuserDef[5] = "0";       //燈板Gamma可調(工程模式) (on : 1 / off : 0)
                svuiuserDef[6] = "0";       //燈板亮度可調(工程模式) (on : 1 / off : 0)
                svuiuserDef[7] = "1";       //狀態讀取 (read-only) :   
                svuiuserDef[8] = "";        //Read only
                svuiuserDef[9] = "";        //Read only
                svuiuserDef[10] = "512";    //單屏_Gamma_32灰 (使用者模式)
                svuiuserDef[11] = "1024";   //單屏_Gamma_64灰 (使用者模式)
                svuiuserDef[12] = "2048";   //單屏_Gamma_128灰 (使用者模式)
                svuiuserDef[13] = "4080";   //單屏_Gamma_255灰 (使用者模式)
                svuiuserDef[14] = "1024";   //單屏_亮度_RED (使用者模式)
                svuiuserDef[15] = "1024";   //單屏_亮度_GRN (使用者模式)
                svuiuserDef[16] = "1024";   //單屏_亮度_BLU (使用者模式)
                svuiuserDef[17] = "16384";  //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[18] = "0";      //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[19] = "0";      //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[20] = "0";      //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[21] = "16384";  //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[22] = "0";      //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[23] = "0";      //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[24] = "0";      //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[25] = "16384";  //轉換矩陣a1 16-bit值(MSB = 0是正數, MSB = 1是負數)
                svuiuserDef[26] = "6850";   //primary 做完光學校正之紅色的色度值x (read-only)     
                svuiuserDef[27] = "3146";   //primary 做完光學校正之紅色的色度值y (read-only)
                svuiuserDef[28] = "1944";   //primary 做完光學校正之綠色的色度值x (read-only)
                svuiuserDef[29] = "7481";   //primary 做完光學校正之綠色的色度值y (read-only)
                svuiuserDef[30] = "1240";   //primary 做完光學校正之藍色的色度值x (read-only)
                svuiuserDef[31] = "0723";   //primary 做完光學校正之藍色的色度值y (read-only)
                svuiuserDef[32] = "2906";   //primary 做完光學校正之白點的色度值x (read-only)
                svuiuserDef[33] = "3113";   //primary 做完光學校正之白點的色度值y (read-only)
                svuiuserDef[34] = "8835";   //primary 做完光學校正之色溫量測值 (read-only)
                svuiuserDef[35] = "220";    //達哥專用, Gamma值(Read-only)
                svuiuserDef[36] = "";       //Read only
                svuiuserDef[37] = "";       //Read only
                svuiuserDef[38] = "";       //Read only
                svuiuserDef[39] = "";       //Read only
                /*
                                                            0000000000000000 - (0)預設色域PID
                                                            0000000000000001 - (1)色域PAL
                                                            0000000000000010 - (2)色域NTSC
                                                                       
                 */
                #endregion



                if (mvars.demoMode) Array.Copy(svuiuserDef, svuiuser, svuiuserDef.Length);
                else for (int svi = 0; svi < uc_PictureAdjust.svuiuserDef.Length; svi++) uc_PictureAdjust.svuiuserDef[svi] = mvars.uiregadr_default[svi].Split(',')[2];

                if (uc_PictureAdjust.svuiuser[svGMAreg] == "0")
                {
                    if (mvars.flgsuperuser) { uc_PictureAdjust.svuiuser[svGMAreg] = uc_PictureAdjust.svuiuserDef[svGMAreg]; }
                    else
                    {
                        #region 回讀 0x64000
                        mvars.lblCmd = "MCU_FLASH_R64000";
                        mp.mhMCUFLASHREAD("00064000", 8192);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                        {
                            mvars.errCode = "-3";
                            Form1.tslblStatus.Text = "0x64000 rd MCU Flash fail";
                        }
                        else
                        {
                            if (mvars.strR64K.Length > 1)
                            {
                                //List<string> lst = new List<string>(new string[mvars.strR64K.Split('~').Length]);
                                List<string> lst = new List<string>();
                                lst.AddRange(mvars.strR64K.Split('~'));
                                string[] svr64 = new string[svuiuser.Length];
                                for (int svi = 0; svi < svuiuser.Length; svi++)
                                {
                                    svr64[svi] = lst[svi].Split(',')[1];
                                }
                                uc_PictureAdjust.svuiuser[svGMAreg] = svr64[svGMAreg];
                            }
                            //else lbl_mcuR64000click.Text = "no record";
                            //lbl_mcuR64000click.Text = "< ";
                            //Form1.tslblStatus.Text = "0x64000 rd MCU Flash items：" + (mvars.lstmcuR64000.Items.Count).ToString();
                        }
                        #endregion 回讀 0x64000
                        if (uc_PictureAdjust.svuiuser[svGMAreg] == "0")
                        {
                            mp.funSaveLogs("BrightnessAdj Err) Gammavalue:" + uc_PictureAdjust.svuiuser[svGMAreg]);
                            MessageBox.Show("Please check log (Gammavalue data)", mvars.strUInameMe + mvars.UImajor);
                            goto ex;
                        }
                    }
                }
                if (uc_PictureAdjust.svuiuser[svCCTreg] == "0")
                {
                    if (mvars.flgsuperuser) { uc_PictureAdjust.svuiuser[svCCTreg] = uc_PictureAdjust.svuiuserDef[svCCTreg]; }
                    else
                    {
                        #region 回讀 0x64000
                        mvars.lblCmd = "MCU_FLASH_R64000";
                        mp.mhMCUFLASHREAD("00064000", 8192);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                        {
                            mvars.errCode = "-3";
                            Form1.tslblStatus.Text = "0x64000 rd MCU Flash fail";
                        }
                        else
                        {
                            if (mvars.strR64K.Length > 1)
                            {
                                //List<string> lst = new List<string>(new string[mvars.strR64K.Split('~').Length]);
                                List<string> lst = new List<string>();
                                lst.AddRange(mvars.strR64K.Split('~'));
                                string[] svr64 = new string[svuiuser.Length];
                                for (int svi = 0; svi < svuiuser.Length; svi++)
                                {
                                    svr64[svi] = lst[svi].Split(',')[1];
                                }
                                uc_PictureAdjust.svuiuser[svCCTreg] = svr64[svCCTreg];
                            }
                            //else lbl_mcuR64000click.Text = "no record";
                            //lbl_mcuR64000click.Text = "< ";
                            //Form1.tslblStatus.Text = "0x64000 rd MCU Flash items：" + (mvars.lstmcuR64000.Items.Count).ToString();
                        }
                        #endregion 回讀 0x64000
                        if (uc_PictureAdjust.svuiuser[svCCTreg] == "0")
                        {
                            mp.funSaveLogs("BrightnessAdj Err) CCT:" + uc_PictureAdjust.svuiuser[svCCTreg]);
                            MessageBox.Show("Please check log (CCT data)", mvars.strUInameMe + mvars.UImajor);
                            goto ex;
                        }
                    }
                }
                uc_PictureAdjust.pidgv = Convert.ToSingle(svuiuser[svGMAreg]) / 100;
                for (int svi = svCGREDXreg; svi <= svCGWHIYreg; svi++)
                {
                    if (mp.IsNumeric(svuiuser[svi]) == false) { mvars.errCode = "-" + svi; }
                    else
                    {
                        if (uc_PictureAdjust.svuiuser[svi] == "0")
                        {
                            if (mvars.flgsuperuser) { uc_PictureAdjust.svuiuser[svi] = uc_PictureAdjust.svuiuserDef[svi]; }
                            else
                            {
                                mp.funSaveLogs("BrightnessAdj Err) svuiuser[" + svi + "]:" + uc_PictureAdjust.svuiuser[svi]);

                                //string v = "";
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                {
                                    mp.msgBox("PictureAdjust", "There are no optical correction(WT) data" + "\r\n" + "\r\n" , Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 3);
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                {
                                    mp.msgBox("影像調整", "沒有光學校正資料(WT)" + "\r\n" + "\r\n", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 3);
                                }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                {
                                    mp.msgBox("影像调整", "沒有光學校正資料(WT)" + "\r\n" + "\r\n", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 3);
                                }
                                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                                {
                                    mp.msgBox("PictureAdjust", "There are no optical correction(WT) data" + "\r\n" + "\r\n", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 3);
                                }


                                //20230530先比大
                                float Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserGreg]);
                                if (Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]) > Amax) { Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]); }
                                if (Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]) > Amax) { Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]); }

                                int svp = (int)Math.Round(Math.Pow(Amax / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv) * 100, 0);
                                tbar_brightness.Value = svp;
                                lbl_brightness.Text = "(" + svp + "%)";

                                uc_PictureAdjust.pidcct = Convert.ToInt16(svuiuserDef[34]);

                                uc_PictureAdjust.pidxy[0] = Convert.ToSingle(svuiuserDef[26]);
                                uc_PictureAdjust.pidxy[1] = Convert.ToSingle(svuiuserDef[27]);
                                uc_PictureAdjust.pidxy[2] = Convert.ToSingle(svuiuserDef[28]);
                                uc_PictureAdjust.pidxy[3] = Convert.ToSingle(svuiuserDef[29]);
                                uc_PictureAdjust.pidxy[4] = Convert.ToSingle(svuiuserDef[30]);
                                uc_PictureAdjust.pidxy[5] = Convert.ToSingle(svuiuserDef[31]);
                                uc_PictureAdjust.pidxy[6] = Convert.ToSingle(svuiuserDef[32]);
                                uc_PictureAdjust.pidxy[7] = Convert.ToSingle(svuiuserDef[33]);



                                uc_PictureAdjust.xr = uc_PictureAdjust.pidxy[0];
                                uc_PictureAdjust.yr = uc_PictureAdjust.pidxy[1];
                                uc_PictureAdjust.xg = uc_PictureAdjust.pidxy[2];
                                uc_PictureAdjust.yg = uc_PictureAdjust.pidxy[3];
                                uc_PictureAdjust.xb = uc_PictureAdjust.pidxy[4];
                                uc_PictureAdjust.yb = uc_PictureAdjust.pidxy[5];
                                uc_PictureAdjust.xw = uc_PictureAdjust.pidxy[6];
                                uc_PictureAdjust.yw = uc_PictureAdjust.pidxy[7];

                                goto ex;
                            }
                        }
                        uc_PictureAdjust.pidxy[svi - svCGREDXreg] = Convert.ToSingle(uc_PictureAdjust.svuiuser[svi]) / 10000;
                        //更新 Default
                        uc_PictureAdjust.svuiuserDef[svi] = uc_PictureAdjust.svuiuser[svi];
                    }
                }
                uc_PictureAdjust.pidcct = Convert.ToInt16(uc_PictureAdjust.svuiuser[svCCTreg]);
                uc_PictureAdjust.xr = uc_PictureAdjust.pidxy[0];
                uc_PictureAdjust.yr = uc_PictureAdjust.pidxy[1];
                uc_PictureAdjust.xg = uc_PictureAdjust.pidxy[2];
                uc_PictureAdjust.yg = uc_PictureAdjust.pidxy[3];
                uc_PictureAdjust.xb = uc_PictureAdjust.pidxy[4];
                uc_PictureAdjust.yb = uc_PictureAdjust.pidxy[5];
                uc_PictureAdjust.xw = uc_PictureAdjust.pidxy[6];
                uc_PictureAdjust.yw = uc_PictureAdjust.pidxy[7];

                if (mvars.demoMode)
                {
                    #region svuiuser@demo       Array.Copy(uc_PictureAdjust.svuiuser, uc_PictureAdjust.svuiuserDef, uc_PictureAdjust.svuiuser.Length)
                    Array.Copy(uc_PictureAdjust.svuiuser, uc_PictureAdjust.svuiuserDef, uc_PictureAdjust.svuiuser.Length);
                    uc_PictureAdjust.svuiuser[svUserRreg] = "889";      //6500K
                    uc_PictureAdjust.svuiuser[svUserGreg] = "953";      //6500K
                    uc_PictureAdjust.svuiuser[svUserBreg] = "1024";     //6500K

                    uc_PictureAdjust.svuiuser[svCGREDXreg] = "6850";      //6500K
                    uc_PictureAdjust.svuiuser[svCGREDYreg] = "3146";      //6500K
                    uc_PictureAdjust.svuiuser[svCGGRNXreg] = "1944";      //6500K
                    uc_PictureAdjust.svuiuser[svCGGRNYreg] = "7481";      //6500K
                    uc_PictureAdjust.svuiuser[svCGBLUXreg] = "1240";      //6500K
                    uc_PictureAdjust.svuiuser[svCGBLUYreg] = "0723";      //6500K
                    uc_PictureAdjust.svuiuser[svCGWHIXreg] = "2906";      //6500K
                    uc_PictureAdjust.svuiuser[svCGWHIYreg] = "3113";      //6500K
                    uc_PictureAdjust.svuiuser[svCCTreg] = "6500";
                    uc_PictureAdjust.svuiuser[svGAMUTreg] = "0";

                    uc_PictureAdjust.pidxy[0] = 0.6939f;      //6500K
                    uc_PictureAdjust.pidxy[1] = 0.3060f;      //6500K
                    uc_PictureAdjust.pidxy[2] = 0.2100f;      //6500K
                    uc_PictureAdjust.pidxy[3] = 0.7388f;      //6500K
                    uc_PictureAdjust.pidxy[4] = 0.1288f;      //6500K
                    uc_PictureAdjust.pidxy[5] = 0.0690f;      //6500K
                    uc_PictureAdjust.pidxy[6] = 0.2862f;      //6500K
                    uc_PictureAdjust.pidxy[7] = 0.3001f;      //6500K
                    uc_PictureAdjust.pidcct = 6500;
                    #endregion svuiuser@demo
                }
                else
                {
                    if (Convert.ToInt16(uc_PictureAdjust.svuiuser[svUserRreg]) > Convert.ToInt16(uc_PictureAdjust.lvgraymax) ||
                        Convert.ToInt16(uc_PictureAdjust.svuiuser[svUserGreg]) > Convert.ToInt16(uc_PictureAdjust.lvgraymax) ||
                        Convert.ToInt16(uc_PictureAdjust.svuiuser[svUserBreg]) > Convert.ToInt16(uc_PictureAdjust.lvgraymax))
                    {
                        btn_default_Click(null, null);
                        return;
                    }
                }

                //補充
                //uc_PictureAdjust.svuiuserDef[7] = uc_PictureAdjust.svuiuser[7];      //Register Read Only    燈板demura
                //uc_PictureAdjust.svuiuserDef[8] = uc_PictureAdjust.svuiuser[8];      //Register Read Only    燈板demura
                uc_PictureAdjust.svuiuserDef[svCGENreg] = uc_PictureAdjust.svuiuser[svCGENreg];                                                     //色域開關
                uc_PictureAdjust.svuiuserDef[svGMAreg] = uc_PictureAdjust.svuiuser[svGMAreg];                                                       //Register Read Only    GammaValue    
                //if (svuiuser[svGAMUTreg] != svuiuser[svGMAreg])  uc_PictureAdjust.svuiuserDef[svGAMUTreg] = uc_PictureAdjust.svuiuser[svGAMUTreg];  //色域紀錄

                //GammaValue
                if (mp.IsNumeric(uc_PictureAdjust.svuiuser[svGMAreg]) == false) { mvars.errCode = "-10"; }
                else
                {
                    uc_PictureAdjust.pidgv = Convert.ToSingle(uc_PictureAdjust.svuiuser[svGMAreg]) / 100;
                    uc_PictureAdjust.svuiuserDef[svGMAreg] = uc_PictureAdjust.svuiuser[svGMAreg];
                    //pidgv = 2.2f;
                    usergv = pidgv;
                }

                //色域三角座標
                for (int svi = svCGREDXreg; svi <= svCGWHIYreg; svi++)
                {
                    if (mp.IsNumeric(svuiuser[svi]) == false) { mvars.errCode = "-" + svi; }
                    else
                    {
                        uc_PictureAdjust.pidxy[svi - svCGREDXreg] = Convert.ToSingle(uc_PictureAdjust.svuiuser[svi]) / 10000;
                        //更新 Default
                        uc_PictureAdjust.svuiuserDef[svi] = uc_PictureAdjust.svuiuser[svi];  //Register Read Only
                    }
                }
                xr = uc_PictureAdjust.pidxy[0];
                yr = uc_PictureAdjust.pidxy[1];
                xg = uc_PictureAdjust.pidxy[2];
                yg = uc_PictureAdjust.pidxy[3];
                xb = uc_PictureAdjust.pidxy[4];
                yb = uc_PictureAdjust.pidxy[5];
                xw = uc_PictureAdjust.pidxy[6];
                yw = uc_PictureAdjust.pidxy[7];
                Array.Copy(pidxy, userxy, pidxy.Length);

                //CCT
                uc_PictureAdjust.pidcct = Convert.ToInt16(uc_PictureAdjust.svuiuser[svCCTreg]);
                userCCT = pidcct;

                #region 0x64000 回讀判斷
                if (mvars.demoMode == false)
                {
                    mvars.lblCmd = "MCU_FLASH_R64000";
                    mp.mhMCUFLASHREAD("00064000", 8192);
                }
                else
                {
                    mvars.strR64K =
                    "0,128~" +
                    "1,1~" +
                    "2,1~" +
                    "3,1~" +
                    "4,0~" +
                    "5,0~" +
                    "6,0~" +
                    "7,1~" +
                    "8,0~" +
                    "9,0~" +
                    "10,512~" +
                    "11,1024~" +
                    "12,2048~" +
                    "13,4080~" +
                    "14,1024~" +
                    "15,1024~" +
                    "16,1024~" +
                    "17,16384~" +
                    "18,0~" +
                    "19,0~" +
                    "20,0~" +
                    "21,16384~" +
                    "22,0~" +
                    "23,0~" +
                    "24,0~" +
                    "25,16384~";

                    //pid CG
                    mvars.strR64K += "26,6850~27,3146~28,1944~29,7481~30,1240~31,0723~32,2906~33,3113~34,6500~35,220~";
                    //PAL CG
                    //mvars.strR64K += "30,6400~31,3300~32,2900~33,6000~34,1500~35,0600~36,3127~37,3290~38,11200";
                    //NTSC CG
                    //mvars.strR64K += "30,6700~31,3300~32,2100~33,7100~34,1400~35,0800~36,3101~37,3162~38,11200";
                    mvars.strR64K += "36,0~37,0~38,0~39,0";





                }
                if (mvars.strR64K.Split('~').Length > 1)
                {
                    if (mvars.strR64K.Substring(0, "0,128".Length) == "0,128")
                    {
                        string[] svs1 = mvars.strR64K.Split('~');
                        for (int i = 0; i < svuiuserDef.Length; i++)
                        {
                            if (Convert.ToInt16(svs1[i].Split(',')[0]) == svGMAreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { usergv = Convert.ToSingle(svs1[i].Split(',')[1]) / 100; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCGREDXreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userxy[0] = Convert.ToSingle(svs1[i].Split(',')[1]) / 10000; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCGREDYreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userxy[1] = Convert.ToSingle(svs1[i].Split(',')[1]) / 10000; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCGGRNXreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userxy[2] = Convert.ToSingle(svs1[i].Split(',')[1]) / 10000; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCGGRNYreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userxy[3] = Convert.ToSingle(svs1[i].Split(',')[1]) / 10000; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCGBLUXreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userxy[4] = Convert.ToSingle(svs1[i].Split(',')[1]) / 10000; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCGBLUYreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userxy[5] = Convert.ToSingle(svs1[i].Split(',')[1]) / 10000; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCGWHIXreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userxy[6] = Convert.ToSingle(svs1[i].Split(',')[1]) / 10000; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCGWHIYreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userxy[7] = Convert.ToSingle(svs1[i].Split(',')[1]) / 10000; }
                            else if (Convert.ToInt16(svs1[i].Split(',')[0]) == svCCTreg && Convert.ToInt16(svs1[i].Split(',')[1]) != 0) { userCCT = Convert.ToInt16(svs1[i].Split(',')[1]); }
                        }
                    }
                    else
                    {
                        mp.funSaveLogs("(BrightnessAdj Err) strR64K:" + mvars.strR64K);
                        MessageBox.Show("Please check log (strR64K)", mvars.strUInameMe + mvars.UImajor);
                        goto ex;
                    }
                }
                #endregion 0x64000 回讀判斷

                //色溫不同就要計算 xct 與 yct
                if (uc_PictureAdjust.userCCT == uc_PictureAdjust.pidcct) { uc_PictureAdjust.xct = xw; uc_PictureAdjust.yct = yw; }
                else
                {
                    float svxct = 0;
                    float svyct = 0;
                    bool svb = invma.CWCT2xy(ref svxct, ref svyct, uc_PictureAdjust.userCCT);     //計算自定義白點座標 獲得xct與yct
                    txt_newctxy2.Text = svxct + "," + svyct;
                    xct = svxct;
                    yct = svyct;
                    uc_PictureAdjust.svuiuser[svBriENreg] = "1";      //bit 0 : User 亮度可調開關 (on : 1 / off : 0)
                    uc_PictureAdjust.svuiuser[svCGWHIXreg] = (uc_PictureAdjust.xct * 10000).ToString();
                    uc_PictureAdjust.svuiuser[svCGWHIYreg] = (uc_PictureAdjust.yct * 10000).ToString();
                }
                lbl_xw.Text = uc_PictureAdjust.xct.ToString();
                lbl_yw.Text = uc_PictureAdjust.yct.ToString();

                if (mvars.strR64K.Split('~').Length > 1)
                {
                    svs = mvars.strR64K.Split('~')[0].Split(',')[1];
                    for (int svi = 1; svi < mvars.strR64K.Split('~').Length; svi++)
                    {
                        svs += "," + mvars.strR64K.Split('~')[svi].Split(',')[1];
                    }
                    //Form1.lstget1.Items.Add("strR64 " + svs);
                    //Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                }
            }
            else //if (mvars.nualg == false)
            {
                //只開放亮度調整
                #region svuiuser Default
                svuiuserDef[0] = "16";
                svuiuserDef[1] = "512";
                svuiuserDef[2] = "1024";
                svuiuserDef[3] = "2048";
                svuiuserDef[4] = "4080";
                svuiuserDef[5] = "255";
                svuiuserDef[6] = "255";
                svuiuserDef[7] = "255";
                #endregion

                uc_PictureAdjust.usergv = 2.2f;

                if (mvars.demoMode == false)
                {
                    string svs0 = "0";
                    mvars.lblCompose = "USER_READ_MCU";
                    mp.cFLASHREAD_C12AMCU("62000");
                    if (mvars.strR62K.Length > 0) { svs0 = mvars.strR62K.Split('~')[0].Split(',')[1]; }

                    mvars.lblCompose = "USER_READ_MCU";
                    mp.cFLASHREAD_C12AMCU("64000");

                    int svsum = 0;
                    mvars.chkcf[0].Checked = true;
                    mvars.chkcf[1].Checked = true;
                    if (mp.DecToBin(Convert.ToInt16(svuiuser[0]), 8).Substring(4, 1) == "1") { mvars.chkcf[3].Checked = true; }
                    mvars.chkcf[4].Checked = true;
                    mvars.chkcf[1].Checked = true;
                    for (int i = 0; i < mvars.chkcf.Length; i++)
                    {
                        if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
                    }

                    svsum = svsum | Convert.ToByte(svs0);

                    if (mvars.strR64K.Length > 0)
                    {
                        mvars.strR64K = mp.replaceRxx(mvars.strR64K, "0,", svsum.ToString());
                    }
                    //Form1.lstget1.Items.Add("UI User[8] Enable " + mvars.strR64K);
                    //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;

                    float Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserGreg]);
                    if (Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]) > Amax) { Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]); }
                    if (Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]) > Amax) { Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]); }

                    int svp = (int)Math.Round(Math.Pow(Amax / Convert.ToSingle(uc_PictureAdjust.lvgraymax), uc_PictureAdjust.usergv) * 100, 0);
                    tbar_brightness.Value = svp;
                    lbl_brightness.Text = "(" + svp + "%)";
                    svuiuser[svbrigValue] = svp.ToString();
                    txt_brightness.Text = Amax.ToString();
                    btn_default.Visible = true;
                }
            }












            //210122
            //Form1.lstget1.Items.Add("strR64 " + mvars.strR64K);
            //svs = uc_PictureAdjust.svuiuser[0];
            //for (int svi = 1; svi < uc_PictureAdjust.svuiuser.Length; svi++)
            //{
            //    svs += "," + uc_PictureAdjust.svuiuser[svi];
            //}
            //Form1.lstget1.Items.Add("UIuser " + svs);

            //啟動User_Gamma與User_Brightness


            #region 高級設置

            if (mvars.nualg)
            {
                txt_newctxy.Text = pidxy[6] + "," + pidxy[7];

                //先比大
                float Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserGreg]);
                if (Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]) > Amax) { Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]); }
                if (Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]) > Amax) { Amax = Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]); }

                int svp = (int)Math.Round(Math.Pow(Amax / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv) * 100, 0);
                tbar_brightness.Value = svp;
                lbl_brightness.Text = "(" + svp + "%)";

                //計算色溫是否屬於 manual 模式 , Gamnu 是否屬於 manual 模式
                cctmanual = false;
                if (svuiuser[svUserRreg] != uc_PictureAdjust.lvgraymax || uc_PictureAdjust.svuiuser[svUserGreg] != uc_PictureAdjust.lvgraymax || uc_PictureAdjust.svuiuser[svUserBreg] != uc_PictureAdjust.lvgraymax)
                {
                    float svxct1 = 0;
                    float svyct1 = 0;
                    int gRratio = 0;
                    int gGratio = 0;
                    int gBratio = 0;
                    bool svb1 = invma.CWCT2xy(ref svxct1, ref svyct1, uc_PictureAdjust.userCCT);     //計算自定義白點座標 獲得xct與yct
                    txt_newctxy2.Text = svxct1 + "," + svyct1;
                    svb1 = invma.CWCT2gRGBratio(tbar_brightness.Value, ref svxct1, ref svyct1, ref gRratio, ref gGratio, ref gBratio);   //提供xct與yct獲得R/G/B的亮度比例值(ratio*lvgraymax=0~1024間的灰階值)

                    if (svxct1 != uc_PictureAdjust.xct || svyct1 != uc_PictureAdjust.yct) { uc_PictureAdjust.xct = svxct1; uc_PictureAdjust.yct = svyct1; }

                    if (Math.Abs((Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]) / Convert.ToSingle(gRratio)) - 1) > 0.014 ||
                        Math.Abs((Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserGreg]) / Convert.ToSingle(gGratio)) - 1) > 0.014 ||
                        Math.Abs((Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]) / Convert.ToSingle(gBratio)) - 1) > 0.014)
                    {
                        cctmanual = true;
                        //CG
                        float svxct = 0;
                        float svyct = 0;
                        float svrratio = (float)Math.Pow(Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]) / Convert.ToSingle(lvgraymax), usergv);
                        float svgratio = (float)Math.Pow(Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserGreg]) / Convert.ToSingle(lvgraymax), usergv);
                        float svbratio = (float)Math.Pow(Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]) / Convert.ToSingle(lvgraymax), usergv);
                        invma.CWWPxy(ref svxct, ref svyct, svrratio, svgratio, svbratio);
                        txt_newctxy2.Text = svxct + "," + svyct;
                        uc_PictureAdjust.xct = svxct;
                        uc_PictureAdjust.yct = svyct;
                    }
                    lbl_xw.Text = uc_PictureAdjust.xct.ToString();
                    lbl_yw.Text = uc_PictureAdjust.yct.ToString();
                }

                lbl_xr.Text = uc_PictureAdjust.pidxy[0].ToString("0.0000");
                lbl_yr.Text = uc_PictureAdjust.pidxy[1].ToString("0.0000");
                lbl_xg.Text = uc_PictureAdjust.pidxy[2].ToString("0.0000");
                lbl_yg.Text = uc_PictureAdjust.pidxy[3].ToString("0.0000");
                lbl_xb.Text = uc_PictureAdjust.pidxy[4].ToString("0.0000");
                lbl_yb.Text = uc_PictureAdjust.pidxy[5].ToString("0.0000");
                lbl_xw.Text = uc_PictureAdjust.xct.ToString();
                lbl_yw.Text = uc_PictureAdjust.yct.ToString();
                string svm = mp.DecToBin(Convert.ToInt16(uc_PictureAdjust.svuiuser[svGAMUTreg]), 16);
                if (svm.Substring(svm.Length - 2, 2) == "00") { uc_PictureAdjust.cgkw = "(PID)"; }
                else if (svm.Substring(svm.Length - 2, 2) == "01") { uc_PictureAdjust.cgkw = "(PAL)"; }
                else if (svm.Substring(svm.Length - 2, 2) == "10") { uc_PictureAdjust.cgkw = "(NTSC)"; }

                lbl_brigBar.Left = lbl_brigGamma.Left;
                tbar_advSetting.Minimum = 10;
                tbar_advSetting.Maximum = 30;
                txt_advSetting.Text = uc_PictureAdjust.usergv.ToString();
                tbar_advSetting.Value = Convert.ToInt16(uc_PictureAdjust.usergv * 10);

                if (cctmanual)
                {
                    tbar_gr.Enabled = rbtn_usercct.Checked;
                    tbar_gg.Enabled = rbtn_usercct.Checked;
                    tbar_gb.Enabled = rbtn_usercct.Checked;
                    tbar_gr.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserRreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), uc_PictureAdjust.usergv) * 100, 0));
                    tbar_gg.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserGreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), uc_PictureAdjust.usergv) * 100, 0));
                    tbar_gb.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(uc_PictureAdjust.svuiuser[svUserBreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), uc_PictureAdjust.usergv) * 100, 0));
                    lbl_gr.Text = "(" + tbar_gr.Value.ToString() + "%)";
                    lbl_gg.Text = "(" + tbar_gg.Value.ToString() + "%)";
                    lbl_gb.Text = "(" + tbar_gb.Value.ToString() + "%)";
                }

                grp_cg.Location = grp_advSetting.Location;
                label8.Text = uc_PictureAdjust.cgkw;
            }
        ex:
            mvars.actFunc = "pictureadjust";

            if (mvars.demoMode) { mp.funSaveLogs("(BrightnessAdj) Load @ demomode"); }
            else { mp.funSaveLogs("(BrightnessAdj) Load numcu," + mvars.numcu + ",nualg," + mvars.nualg + ",strR64," + mvars.strR64K); }

            txt_focus.Focus();
            #endregion
        }






        private void lbl_brigGamma_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            //int svv = 0;
            //if (mvars.nualg) { svv = 10; }
            if (lbl.Tag.ToString() == "GMA")
            {
                lbl_brigBar.Left = lbl_brigGamma.Left;
                if (MultiLanguage.DefaultLanguage == "en-US") { grp_advSetting.Text = "Graysacle"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { grp_advSetting.Text = "對比度"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { grp_advSetting.Text = "对比度"; }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { grp_advSetting.Text = "对比"; }
                tbar_advSetting.Minimum = 10;
                tbar_advSetting.Maximum = 30;
                tbar_advSetting.Value = Convert.ToInt16(usergv * 10);
                txt_advSetting.Text = usergv.ToString();
                grp_cg.Visible = false;
                lbl_advSetting.Visible = true;
            }
            else if (lbl.Tag.ToString() == "CCT")
            {
                lbl_brigBar.Left = lbl_brigCCT.Left;
                if (MultiLanguage.DefaultLanguage == "en-US") { grp_advSetting.Text = "Color Temperature"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { grp_advSetting.Text = "色溫"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { grp_advSetting.Text = "色溫"; }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { grp_advSetting.Text = "色溫度"; }
                tbar_advSetting.Minimum = 20;
                tbar_advSetting.Maximum = 130;
                grp_cg.Visible = false;
                lbl_advSetting.Visible = false;

                //確認CCT是在自定義還是在黑體軌跡上
                if (mvars.nualg)
                {
                    //如果 userCCT==32767 則表示色溫是在自定義模式下,計算要傳給色域的xct,yct
                    if (cctmanual)
                    {
                        rbtn_cct.Checked = false;
                        tbar_advSetting.Enabled = false;
                        rbtn_usercct.Checked = true;
                        tbar_gr.Enabled = rbtn_usercct.Checked;
                        tbar_gg.Enabled = rbtn_usercct.Checked;
                        tbar_gb.Enabled = rbtn_usercct.Checked;
                    }
                    else
                    {
                        rbtn_cct.Checked = true;
                        tbar_advSetting.Enabled = true;
                        rbtn_usercct.Checked = false;
                        //if (uc_PictureAdjust.userCCT == uc_PictureAdjust.pidcct)
                        //{
                        //    xct = pidxy[6];
                        //    yct = pidxy[7];
                        //}
                        //else
                        //{
                        //    //CG
                        //    float svxct = 0;
                        //    float svyct = 0;
                        //    float svrratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserRreg]) / Convert.ToSingle(lvgraymax), usergv);
                        //    float svgratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserGreg]) / Convert.ToSingle(lvgraymax), usergv);
                        //    float svbratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserBreg]) / Convert.ToSingle(lvgraymax), usergv);
                        //    invma.CWWPxy(ref svxct, ref svyct, svrratio, svgratio, svbratio);
                        //    txt_newctxy2.Text = svxct + "," + svyct;
                        //    xct = svxct;
                        //    yct = svyct;
                        //}
                        //lbl_xw.Text = xct.ToString();
                        //lbl_yw.Text = yct.ToString();

                    }
                    tbar_advSetting.Value = userCCT / 100;
                    txt_advSetting.Text = tbar_advSetting.Value.ToString();

                    tbar_gr.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserRreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), pidgv) * 100, 0));
                    tbar_gg.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserGreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), pidgv) * 100, 0));
                    tbar_gb.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserBreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), pidgv) * 100, 0));
                    lbl_gr.Text = "(" + tbar_gr.Value.ToString() + "%)";
                    lbl_gg.Text = "(" + tbar_gg.Value.ToString() + "%)";
                    lbl_gb.Text = "(" + tbar_gb.Value.ToString() + "%)";

                    if (svuiuser[svUserRreg] == lvgraymax && svuiuser[svUserGreg] == lvgraymax && svuiuser[svUserBreg] == lvgraymax)
                    {
                        tbar_advSetting.Value = pidcct / 100; txt_advSetting.Text = pidcct.ToString();
                    }
                    else { tbar_advSetting.Value = userCCT / 100; txt_advSetting.Text = userCCT.ToString(); }
                }
            }
            else if (lbl.Tag.ToString() == "CG")
            {
                lbl_brigBar.Left = lbl_brigColorspace.Left;
                if (MultiLanguage.DefaultLanguage == "en-US") { grp_advSetting.Text = "Color Space"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { grp_advSetting.Text = "色域"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { grp_advSetting.Text = "色域"; }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { grp_advSetting.Text = "色域"; }
                grp_cg.Visible = true;

                label8.Text = cgkw;
                lbl_xr.Text = string.Format("{0:0.0000}", pidxy[0]);
                lbl_yr.Text = string.Format("{0:0.0000}", pidxy[1]);
                lbl_xg.Text = string.Format("{0:0.0000}", pidxy[2]);
                lbl_yg.Text = string.Format("{0:0.0000}", pidxy[3]);
                lbl_xb.Text = string.Format("{0:0.0000}", pidxy[4]);
                lbl_yb.Text = string.Format("{0:0.0000}", pidxy[5]);
                lbl_xw.Text = xct.ToString();
                lbl_yw.Text = yct.ToString();
                cgkw = label8.Text;
                if (cgkw == "(PAL)")
                {
                    lbl_xtr.Text = conPAL[0].ToString("0.0000");
                    lbl_ytr.Text = conPAL[1].ToString("0.0000");
                    lbl_xtg.Text = conPAL[2].ToString("0.0000");
                    lbl_ytg.Text = conPAL[3].ToString("0.0000");
                    lbl_xtb.Text = conPAL[4].ToString("0.0000");
                    lbl_ytb.Text = conPAL[5].ToString("0.0000");
                    lbl_xtw.Text = conPAL[6].ToString("0.0000");
                    lbl_ytw.Text = conPAL[7].ToString("0.0000");
                    string svm = mp.DecToBin(Convert.ToInt16(svuiuser[svGAMUTreg]), 16);
                    svm = mp.ReplaceAt(svm, 15, "0");
                    svm = mp.ReplaceAt(svm, 16, "1");
                    svuiuser[svGAMUTreg] = mp.BinToDec(svm).ToString();
                }
                else if (cgkw == "(NTSC)")
                {
                    lbl_xtr.Text = conNTSC[0].ToString("0.0000");
                    lbl_ytr.Text = conNTSC[1].ToString("0.0000");
                    lbl_xtg.Text = conNTSC[2].ToString("0.0000");
                    lbl_ytg.Text = conNTSC[3].ToString("0.0000");
                    lbl_xtb.Text = conNTSC[4].ToString("0.0000");
                    lbl_ytb.Text = conNTSC[5].ToString("0.0000");
                    lbl_xtw.Text = conNTSC[6].ToString("0.0000");
                    lbl_ytw.Text = conNTSC[7].ToString("0.0000");
                    string svm = mp.DecToBin(Convert.ToInt16(svuiuser[svGAMUTreg]), 16);
                    svm = mp.ReplaceAt(svm, 15, "1");
                    svm = mp.ReplaceAt(svm, 16, "0");
                    svuiuser[svGAMUTreg] = mp.BinToDec(svm).ToString();
                }
                else if (cgkw == "(User)")
                {
                    lbl_xtr.Text = userxy[0].ToString("0.0000");
                    lbl_ytr.Text = userxy[1].ToString("0.0000");
                    lbl_xtg.Text = userxy[2].ToString("0.0000");
                    lbl_ytg.Text = userxy[3].ToString("0.0000");
                    lbl_xtb.Text = userxy[4].ToString("0.0000");
                    lbl_ytb.Text = userxy[5].ToString("0.0000");
                    lbl_xtw.Text = userxy[6].ToString("0.0000");
                    lbl_ytw.Text = userxy[7].ToString("0.0000");
                    string svm = mp.DecToBin(Convert.ToInt16(svuiuser[svGAMUTreg]), 16);
                    svm = mp.ReplaceAt(svm, 15, "1");
                    svm = mp.ReplaceAt(svm, 16, "1");
                    svuiuser[svGAMUTreg] = mp.BinToDec(svm).ToString();
                }
                else if (cgkw == "(PID)")
                {
                    //label8.Text = "(PID)";
                    lbl_xtr.Text = pidxy[0].ToString("0.0000");
                    lbl_ytr.Text = pidxy[1].ToString("0.0000");
                    lbl_xtg.Text = pidxy[2].ToString("0.0000");
                    lbl_ytg.Text = pidxy[3].ToString("0.0000");
                    lbl_xtb.Text = pidxy[4].ToString("0.0000");
                    lbl_ytb.Text = pidxy[5].ToString("0.0000");
                    lbl_xtw.Text = xct.ToString("0.0000");
                    lbl_ytw.Text = yct.ToString("0.0000");
                    string svm = mp.DecToBin(Convert.ToInt16(svuiuser[svGAMUTreg]), 16);
                    svm = mp.ReplaceAt(svm, 15, "0");
                    svm = mp.ReplaceAt(svm, 16, "0");
                    svuiuser[svGAMUTreg] = mp.BinToDec(svm).ToString();
                }
            }

            #region 色溫區塊
            rbtn_cct.Visible = !lbl_advSetting.Visible;
            rbtn_usercct.Visible = !lbl_advSetting.Visible;
            lbl_hr.Visible = !lbl_advSetting.Visible;
            lbl_hg.Visible = !lbl_advSetting.Visible;
            lbl_hb.Visible = !lbl_advSetting.Visible;
            tbar_gb.Visible = !lbl_advSetting.Visible;
            tbar_gg.Visible = !lbl_advSetting.Visible;
            tbar_gr.Visible = !lbl_advSetting.Visible;
            lbl_gr.Visible = !lbl_advSetting.Visible;
            lbl_gg.Visible = !lbl_advSetting.Visible;
            lbl_gb.Visible = !lbl_advSetting.Visible;
            #endregion 色溫區塊
        }


        private void lbl_showdetail_Click(object sender, EventArgs e)
        {
            if (lbl_showdetail.Text == "︾︾")
            {
                lbl_showdetail.SetBounds(337, this.Height - 35 - 10, 25, 35);
                lbl_showdetail.Text = "︽︽";
                grp_brightnessAdv.Visible = true;
                btn_default.Top = this.Height - btn_default.Height - 10;
                rbtn_cct.Top = lbl_advSetting.Top;
                btn_default.Visible = true;
                grp_cg.Location = grp_advSetting.Location;
            }
            else
            {
                lbl_showdetail.SetBounds(337, 194, 25, 35);
                lbl_showdetail.Text = "︾︾";
                grp_brightnessAdv.Visible = false;
                btn_default.Visible = false;    
            }
        }




        private void tbar_advSetting_Scroll(object sender, EventArgs e)
        {
            if (lbl_brigBar.Left == lbl_brigGamma.Left)
            {
                txt_advSetting.Text = (Convert.ToSingle(tbar_advSetting.Value) / 10).ToString();
            }
            else if (lbl_brigBar.Left == lbl_brigCCT.Left)
            {
                if (rbtn_cct.Checked) { txt_advSetting.Text = (tbar_advSetting.Value * 100).ToString(); }
            }
        }
        private void tbar_advSetting_MouseUp(object sender, MouseEventArgs e)
        {
            this.Enabled = false;
            Form1.lstget1.Items.Clear();
            mvars.flgSelf = true;
            if (lbl_brigBar.Left == lbl_brigGamma.Left)
            {
                if ( txt_advSetting.Text == pidgv.ToString()) { svuiuser[svGmaENreg] = "0"; }
                else
                {
                    usergv = Convert.ToSingle(tbar_advSetting.Value) / 10;
                    float svfb = usergv / pidgv;
                    if (mvars.nualg)
                    {
                        svuiuser[svUserGMA1] = Math.Round(Math.Pow(32, svfb) * Math.Pow(255, (1 - svfb)) * 16, 0).ToString();
                        svuiuser[svUserGMA2] = Math.Round(Math.Pow(64, svfb) * Math.Pow(255, (1 - svfb)) * 16, 0).ToString();
                        svuiuser[svUserGMA3] = Math.Round(Math.Pow(128, svfb) * Math.Pow(255, (1 - svfb)) * 16, 0).ToString();
                        svuiuser[svUserGMA4] = Math.Round(Math.Pow(255, svfb) * Math.Pow(255, (1 - svfb)) * 16, 0).ToString();
                    }
                    svuiuser[svGmaENreg] = "1";  //Gamma可調開關 (on : 1 / off : 0)
                    svuiuser[svGMAreg] = (tbar_advSetting.Value * 10).ToString();

                    if (mvars.demoMode == false)
                    {
                        mvars.nvBoardcast = true;
                        mvars.lblCompose = "USER_GAMMA";
                        if (mvars.nualg)
                        {
                            string svdeviceID = mvars.deviceID;
                            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                            byte svFPGAsel = mvars.FPGAsel;
                            mvars.FPGAsel = 2;
                            mvars.lblCmd = "FPGA_REG_W";
                            for (byte svi = 0; svi < 2; svi++)
                            {
                                string[] RegDec = new string[svuiuser.Length];
                                string[] DataDec = new string[svuiuser.Length];
                                mvars.lblCmd = "FPGA_REG_W";
                                for (int svj = 0; svj < svuiuser.Length; svj++) { RegDec[svj] = svj.ToString(); DataDec[svj] = svuiuser[svj]; }
                                mp.mpFPGAUIREGWarr(RegDec, DataDec);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                            }
                            mp.doDelayms(100);
                            //不需要分左右畫面
                            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                            for (UInt16 i = 0; i < svuiuser.Length; i++)
                            {
                                BinArr[i * 4 + 0] = (Byte)(i / 256);
                                BinArr[i * 4 + 1] = (Byte)(i % 256);
                                BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(svuiuser[i]) / 256);
                                BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(svuiuser[i]) % 256);
                            }
                            //Checksum
                            UInt16 checksum = mp.CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                            BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                            BinArr[BinArr.Length - 1] = (byte)(checksum % 256);
                            mvars.lblCmd = "MCU_FLASH_W64000";
                            mp.mhMCUFLASHWRITE("64000", ref BinArr);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }

                            mvars.deviceID = svdeviceID;
                            mvars.FPGAsel = svFPGAsel;
                        }
                        else
                        {
                            int svsum = 0;
                            mvars.chkcf[1].Checked = true;
                            if (mvars.nualg) { svuiuser[svBriENreg] = "1"; }
                            else
                            {
                                for (int i = 0; i < mvars.chkcf.Length; i++)
                                {
                                    if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
                                }
                                mvars.flgSelf = true;
                                svuiuser[0] = svsum.ToString();
                            }
                        }
                    }
                }
            }
            else if (lbl_brigBar.Left == lbl_brigCCT.Left)
            {
                float svxct = 0;
                float svyct = 0;
                int gRratio = 0;
                int gGratio = 0;
                int gBratio = 0;
                bool svb;
                svb = invma.CWCT2xy(ref svxct, ref svyct, tbar_advSetting.Value * 100);     //計算自定義白點座標 獲得xct與yct
                txt_newctxy2.Text = svxct + "," + svyct;
                svb = invma.CWCT2gRGBratio(tbar_brightness.Value, ref svxct, ref svyct, ref gRratio, ref gGratio, ref gBratio);   //提供xct與yct獲得R/G/B的亮度比例值(ratio*lvgraymax=0~1024間的灰階值)

                //int svv = 0;
                if (mvars.nualg)
                {
                    //svv = 10;
                    //已經是經過gamma計算的亮度值
                    svuiuser[svUserRreg] = gRratio.ToString();
                    svuiuser[svUserGreg] = gGratio.ToString();
                    svuiuser[svUserBreg] = gBratio.ToString();
                    //User Gamma可調開關 (on : 1 / off : 0)
                    svuiuser[svGmaENreg] = "1";  
                }
                
                userCCT = tbar_advSetting.Value * 100;
                svuiuser[svCCTreg] = userCCT.ToString();

                if (mvars.demoMode == false)
                {
                    mvars.nvBoardcast = true;
                    mvars.lblCompose = "USER_CCT";
                    if (mvars.nualg)
                    {
                        string svdeviceID = mvars.deviceID;
                        mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                        byte svFPGAsel = mvars.FPGAsel;
                        mvars.FPGAsel = 2;

                        //mp.cUIREGADRwALL(svuiuser, false);
                        //frm_scrControl.lstget1.Items.Add("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",R64K " + mvars.strR64K);
                        mvars.lblCmd = "FPGA_REG_W";
                        for (byte svi = 0; svi < 2; svi++)
                        {
                            string[] RegDec = new string[svuiuser.Length];
                            string[] DataDec = new string[svuiuser.Length];
                            mvars.lblCmd = "FPGA_REG_W";
                            for (int svj = 0; svj < svuiuser.Length; svj++) { RegDec[svj] = svj.ToString(); DataDec[svj] = svuiuser[svj]; }
                            mp.mpFPGAUIREGWarr(RegDec, DataDec);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                        }
                        mvars.lblCmd = "MCUBRIGPERCENT";
                        mp.mpMCUCCT(Convert.ToInt16(svuiuser[svCCTreg]), true, true);

                        mvars.deviceID = svdeviceID;
                        mvars.FPGAsel = svFPGAsel;
                    }
                    else
                    {
                        int svsum = 0;
                        mvars.chkcf[1].Checked = true;
                        if (mvars.nualg) { svuiuser[svBriENreg] = "1"; }
                        else
                        {
                            for (int i = 0; i < mvars.chkcf.Length; i++)
                            {
                                if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
                            }
                            mvars.flgSelf = true;
                            svuiuser[0] = svsum.ToString();
                        }
                        //mp.cUSERBRIG(svsum, Convert.ToInt16(svuiuser[5]), Convert.ToInt16(svuiuser[6]), Convert.ToInt16(svuiuser[7]));
                        //frm_scrControl.lstget1.Items.Add("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + "," + svuiuser[5] + "," + svuiuser[6] + "," + svuiuser[7]);
                        //mp.funSaveLogs("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + "," + svuiuser[5] + "," + svuiuser[6] + "," + svuiuser[7]);
                    }
                }

                tbar_gr.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserRreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv) * 100, 0));
                tbar_gg.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserGreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv) * 100, 0));
                tbar_gb.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserBreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv) * 100, 0));
                lbl_gr.Text = "(" + tbar_gr.Value.ToString() + "%)";
                lbl_gg.Text = "(" + tbar_gg.Value.ToString() + "%)";
                lbl_gb.Text = "(" + tbar_gb.Value.ToString() + "%)";
            }
            this.Enabled = true;
            //Form1.lstget1.Items.Clear();
            //Form1.lstget1.Items.Add("R64K " + mvars.strR64K);
        }
       

        private void tbar_brightness_MouseUp(object sender, MouseEventArgs e)
        {
            //int svv = 0;
            float svxct = 0;
            float svyct = 0;
            if (mvars.nualg)
            {
                //svv = 10;
                if (cctmanual == false)
                {
                    svxct = 0;
                    svyct = 0;
                    int gRratio = 0;
                    int gGratio = 0;
                    int gBratio = 0;
                    bool svb;
                    if (userCCT == pidcct)
                    {
                        svxct = pidxy[6]; svyct = pidxy[7];
                    }
                    else
                    {
                        //0.0.1.5 bug tbar_advSetting.Value當操作到gamma的時候會變成 gamma value
                        //svb = invma.CWCT2xy(ref svxct, ref svyct, tbar_advSetting.Value * 100);     //計算自定義白點座標 獲得xct與yct
                        //修正如下
                        svb = invma.CWCT2xy(ref svxct, ref svyct, userCCT);     //計算自定義白點座標 獲得xct與yct
                    }
                    txt_newctxy2.Text = svxct + "," + svyct;
                    svb = invma.CWCT2gRGBratio(tbar_brightness.Value, ref svxct, ref svyct, ref gRratio, ref gGratio, ref gBratio);   //提供xct與yct獲得R/G/B的亮度比例值(ratio*lvgraymax=0~1024間的灰階值)
                    svuiuser[svUserRreg] = gRratio.ToString();
                    svuiuser[svUserGreg] = gGratio.ToString();
                    svuiuser[svUserBreg] = gBratio.ToString();
                }
                else
                {
                    //先退下 usergv
                    float svr = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserRreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv);
                    float svg = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserGreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv);
                    float svb = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserBreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv);
                    float Amax = svr;
                    if (svg > Amax) { Amax = svg; }
                    if (svb > Amax) { Amax = svb; }
                    uc_PictureAdjust.rratio = svr / Amax;
                    uc_PictureAdjust.gratio = svg / Amax;
                    uc_PictureAdjust.bratio = svb / Amax;
                    float svperc = Convert.ToSingle(tbar_brightness.Value) / 100;
                    svuiuser[svUserRreg] = string.Format("{0:#00}", (Math.Pow(uc_PictureAdjust.rratio * svperc, (1 / uc_PictureAdjust.usergv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax)));
                    svuiuser[svUserGreg] = string.Format("{0:#00}", (Math.Pow(uc_PictureAdjust.gratio * svperc, (1 / uc_PictureAdjust.usergv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax)));
                    svuiuser[svUserBreg] = string.Format("{0:#00}", (Math.Pow(uc_PictureAdjust.bratio * svperc, (1 / uc_PictureAdjust.usergv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax)));

                    //CG
                    svxct = 0;
                    svyct = 0;
                    float svrratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserRreg]) / Convert.ToSingle(lvgraymax), usergv);
                    float svgratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserGreg]) / Convert.ToSingle(lvgraymax), usergv);
                    float svbratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserBreg]) / Convert.ToSingle(lvgraymax), usergv);
                    invma.CWWPxy(ref svxct, ref svyct, svrratio, svgratio, svbratio);
                    txt_newctxy2.Text = svxct + "," + svyct;
                }
                xct = svxct;
                yct = svyct;
                lbl_xw.Text = xct.ToString();
                lbl_yw.Text = yct.ToString();
                //bit 0 : User 亮度可調開關 (on : 1 / off : 0)
                svuiuser[svbrigValue] = tbar_brightness.Value.ToString();
                svuiuser[svBriENreg] = "1";
                svuiuser[svCCTreg] = userCCT.ToString();
            }
            else
            {
                //舊版不調色溫所以都是同比例計算
                svuiuser[svUserGreg] = string.Format("{0:##0}", (Math.Pow(Convert.ToSingle(tbar_brightness.Value) / 100, (1 / usergv)) * Convert.ToInt16(lvgraymax))).ToString();
                svuiuser[svUserRreg] = string.Format("{0:##0}", (Convert.ToSingle(svuiuser[svUserGreg])));
                svuiuser[svUserBreg] = string.Format("{0:##0}", (Convert.ToSingle(svuiuser[svUserGreg])));
            }

            if (mvars.demoMode) { return; }
            this.Enabled = false;
            Form1.lstget1.Items.Clear();

            if (mvars.demoMode == false)
            {
                /// 有多少條屏
                /// 每條條屏有2顆FPGA要下參數
                mvars.nvBoardcast = true;
                mvars.lblCompose = "USER_BRIGHTNESS";
                if (mvars.nualg)
                {
                    string svdeviceID = mvars.deviceID;
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                    byte svFPGAsel = mvars.FPGAsel;
                    mvars.FPGAsel = 2;

                    //mp.cUIREGADRwALL(svuiuser, false);
                    //frm_scrControl.lstget1.Items.Add("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",R64K " + mvars.strR64K);
                    mvars.lblCmd = "FPGA_REG_W";
                    for (byte svi = 0; svi < 2; svi++)
                    {
                        string[] RegDec = new string[svuiuser.Length];
                        string[] DataDec = new string[svuiuser.Length];
                        mvars.lblCmd = "FPGA_REG_W";
                        for (int svj = 0; svj < svuiuser.Length; svj++) { RegDec[svj] = svj.ToString(); DataDec[svj] = svuiuser[svj]; }
                        mp.mpFPGAUIREGWarr(RegDec, DataDec);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                    }
                    mvars.lblCmd = "MCUBRIGPERCENT";
                    mp.mpMCUBRIGPERCENT(Convert.ToByte(svuiuser[svbrigValue]), true, true);

                    mp.doDelayms(1000);
                    mvars.lblCmd = "PRIID_OFF";
                    mp.mpIDONOFF(0);

                    mvars.deviceID = svdeviceID;
                    mvars.FPGAsel = svFPGAsel;
                }
                else
                {
                    int svsum = 0;
                    mvars.chkcf[1].Checked = true;
                    if (mvars.nualg) { svuiuser[svBriENreg] = "1"; }
                    else
                    {
                        for (int i = 0; i < mvars.chkcf.Length; i++)
                        {
                            if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
                        }
                        mvars.flgSelf = true;
                        svuiuser[0] = svsum.ToString();
                    }
                    //mp.cUSERBRIG(svsum, Convert.ToInt16(svuiuser[5]), Convert.ToInt16(svuiuser[6]), Convert.ToInt16(svuiuser[7]));
                    //frm_scrControl.lstget1.Items.Add("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + "," + svuiuser[5] + "," + svuiuser[6] + "," + svuiuser[7]);
                    //mp.funSaveLogs("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + "," + svuiuser[5] + "," + svuiuser[6] + "," + svuiuser[7]);
                }
                //frm_scrControl.lstget1.TopIndex = frm_scrControl.lstget1.Items.Count - 1;
                //mp.funSaveLogs("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",R64K " + mvars.strR64K);
            }

            else
            {
                //mvars.strR64K = "0,26~1,512~2,1024~3,2048~4,4080~5," + svuiuser[5] + "~6," + svuiuser[6] + "~7," + svuiuser[7];
                //Form1.lstget1.Items.Add("R64K " + mvars.strR64K);
            }
            tbar_gr.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserRreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv) * 100, 0));
            tbar_gg.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserGreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv) * 100, 0));
            tbar_gb.Value = Convert.ToInt16(Math.Round(Math.Pow(Convert.ToSingle(svuiuser[svUserBreg]) / Convert.ToSingle(uc_PictureAdjust.lvgraymax), usergv) * 100, 0));
            lbl_gr.Text = "(" + tbar_gr.Value.ToString() + "%)";
            lbl_gg.Text = "(" + tbar_gg.Value.ToString() + "%)";
            lbl_gb.Text = "(" + tbar_gb.Value.ToString() + "%)";

            if (tbar_brightness.Value == 0 && cctmanual == true)
            {
                rbtn_cct.Checked = true;
                rbtn_usercct.Checked = false;
                cctmanual = false;
                tbar_gr.Enabled = false;
                tbar_gg.Enabled = false;
                tbar_gb.Enabled = false;
                tbar_advSetting.Enabled = true;
            }
            this.Enabled = true;
            txt_focus.Focus();
        }

        private void tbar_brightness_Scroll(object sender, EventArgs e)
        {
            lbl_brightness.Text = "(" + tbar_brightness.Value.ToString() + "%)";
        }

        private void btn_default_Click(object sender, EventArgs e)
        {
            mvars.strR64K = "";
            this.Enabled = false;
            mvars.flgSelf = true;
            string svdeviceID = mvars.deviceID;
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            byte svFPGAsel = mvars.FPGAsel;
            mvars.FPGAsel = 2;
            if (mvars.nualg)
            {
                for (int svi = 0; svi < uc_PictureAdjust.svuiuserDef.Length; svi++) uc_PictureAdjust.svuiuserDef[svi] = mvars.uiregadr_default[svi].Split(',')[2];
                uc_PictureAdjust.svuiuserDef[svGmaENreg] = svENG_GMA_EN.ToString();
                uc_PictureAdjust.svuiuserDef[svBriENreg] = svENG_BRI_EN.ToString();
                svuiuser = svuiuserDef;
                userCCT = pidcct;
                usergv = pidgv;
                cgkw = "(" + btn_cgdefault.Tag.ToString() + ")";
                label8.Text = cgkw;
            }
            else
            {
                svuiuser[0] = "27";
                svuiuser[1] = "512";
                svuiuser[2] = "1024";
                svuiuser[3] = "2048";
                svuiuser[4] = "4080";
                svuiuser[5] = "255";
                svuiuser[6] = "255";
                svuiuser[7] = "255";
            }

            if (mvars.demoMode == false)
            {
                mvars.lblCmd = "FPGA_REG_W";
                for (byte svi = 0; svi < 2; svi++)
                {
                    string[] RegDec = new string[svuiuserDef.Length];
                    string[] DataDec = new string[svuiuserDef.Length];
                    mvars.lblCmd = "FPGA_REG_W";
                    for (int svj = 0; svj < svuiuserDef.Length; svj++) { RegDec[svj] = svj.ToString(); DataDec[svj] = svuiuserDef[svj]; }
                    mp.mpFPGAUIREGWarr(RegDec, DataDec);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                }
                mvars.lblCmd = "MCUBRIGPERCENT";
                mp.mpMCUBRIGPERCENT(100, true, true);
                byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                mvars.lblCmd = "MCUFLASH_W64000";
                mp.mhMCUFLASHWRITE("00064000", ref BinArr);
                mp.doDelayms(1000);
                mvars.lblCmd = "PRIID_OFF";
                mp.mpIDONOFF(0);
            }

            //frm_scrControl.lstget1.TopIndex = frm_scrControl.lstget1.Items.Count - 1;

            if (MultiLanguage.DefaultLanguage == "en-US") { grp_advSetting.Text = "Graysacle"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { grp_advSetting.Text = "對比度"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { grp_advSetting.Text = "对比度"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { grp_advSetting.Text = "对比"; }
            tbar_brightness.Minimum = 0;
            txt_brightness.Text = lvgraymax;
            tbar_brightness.Value = 100;
            lbl_brightness.Text = "(100%)";
            tbar_gr.Value = 100;
            lbl_gr.Text = "(100%)";
            tbar_gg.Value = 100;
            lbl_gg.Text = "(100%)";
            tbar_gb.Value = 100;
            lbl_gb.Text = "(100%)";

            rbtn_cct.Checked = true;
            tbar_advSetting.Enabled = true;
            rbtn_usercct.Checked = false;
            tbar_gr.Enabled = false;
            tbar_gg.Enabled = false;
            tbar_gb.Enabled = false;

            if (lbl_brigBar.Left == lbl_brigGamma.Left)
            {
                lbl_advSetting.Text = "Gamma Value";
                tbar_advSetting.Minimum = 10;
                tbar_advSetting.Maximum = 30;
                tbar_advSetting.Value = Convert.ToInt16(usergv * 10);
                txt_advSetting.Text = usergv.ToString();
            }
            else if (lbl_brigBar.Left == lbl_brigCCT.Left)
            {
                tbar_advSetting.Minimum = 20;
                tbar_advSetting.Maximum = 130;
                tbar_advSetting.Value = pidcct / 100; txt_advSetting.Text = pidcct.ToString();
                userCCT = pidcct;
            }
            else if (lbl_brigBar.Left == lbl_brigColorspace.Left)
            {
                lbl_xr.Text = string.Format("{0:0.0000}", pidxy[0]);
                lbl_yr.Text = string.Format("{0:0.0000}", pidxy[1]);
                lbl_xg.Text = string.Format("{0:0.0000}", pidxy[2]);
                lbl_yg.Text = string.Format("{0:0.0000}", pidxy[3]);
                lbl_xb.Text = string.Format("{0:0.0000}", pidxy[4]);
                lbl_yb.Text = string.Format("{0:0.0000}", pidxy[5]);
                lbl_xw.Text = xct.ToString();
                lbl_yw.Text = yct.ToString();
                lbl_xtr.Text = string.Format("{0:0.0000}", pidxy[0]);
                lbl_ytr.Text = string.Format("{0:0.0000}", pidxy[1]);
                lbl_xtg.Text = string.Format("{0:0.0000}", pidxy[2]);
                lbl_ytg.Text = string.Format("{0:0.0000}", pidxy[3]);
                lbl_xtb.Text = string.Format("{0:0.0000}", pidxy[4]);
                lbl_ytb.Text = string.Format("{0:0.0000}", pidxy[5]);
                lbl_xtw.Text = string.Format("{0:0.0000}", pidxy[6]);
                lbl_ytw.Text = string.Format("{0:0.0000}", pidxy[7]);
                label8.Text = "(PID)";
            }
            mvars.FPGAsel = svFPGAsel;
            mvars.deviceID = svdeviceID;
            this.Enabled = true;
        }

        private void lbl_eng_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (mvars.flgDirBrig) { mvars.flgDirBrig = false;lbl_eng.BackColor = Color.White; }
            else { mvars.flgDirBrig = true;lbl_eng.BackColor = Control.DefaultBackColor; }
        }

        private void lbl_eng_DoubleClick(object sender, EventArgs e)
        {
            txt_brightness.Visible = !(txt_brightness.Visible);
            txt_newctxy.Visible = !(txt_newctxy.Visible);
            txt_newctxy2.Visible= !(txt_newctxy2.Visible);
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }



        private void rbtn_cct_Click(object sender, EventArgs e)
        {
            if (rbtn_cct.Checked)
            {
                tbar_advSetting.Enabled = true;
                tbar_gr.Enabled = false;
                tbar_gg.Enabled = false;
                tbar_gb.Enabled = false;
                cctmanual = false;
            }
        }

        private void rbtn_usercct_Click(object sender, EventArgs e)
        {
            if (rbtn_usercct.Checked)
            {
                tbar_advSetting.Enabled = false;
                tbar_gr.Enabled = true;
                tbar_gg.Enabled = true;
                tbar_gb.Enabled = true;
                cctmanual = true;
            }
        }

        private void btn_cgpal_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            lbl_xr.Text = string.Format("{0:0.0000}", pidxy[0]);
            lbl_yr.Text = string.Format("{0:0.0000}", pidxy[1]);
            lbl_xg.Text = string.Format("{0:0.0000}", pidxy[2]);
            lbl_yg.Text = string.Format("{0:0.0000}", pidxy[3]);
            lbl_xb.Text = string.Format("{0:0.0000}", pidxy[4]);
            lbl_yb.Text = string.Format("{0:0.0000}", pidxy[5]);
            //if (userCCT == pidcct)
            //{
            //    lbl_xw.Text = string.Format("{0:0.0000}", pidxy[6]);
            //    lbl_yw.Text = string.Format("{0:0.0000}", pidxy[7]);
            //}
            //else
            //{
            lbl_xw.Text = string.Format("{0:0.0000}", xct);
            lbl_yw.Text = string.Format("{0:0.0000}", yct);
            //}
            cgkw = "(" + btn.Tag.ToString() + ")";
            label8.Text = cgkw;
            if (cgkw == "(PAL)")
            {
                lbl_xtr.Text = conPAL[0].ToString("0.0000");
                lbl_ytr.Text = conPAL[1].ToString("0.0000");
                lbl_xtg.Text = conPAL[2].ToString("0.0000");
                lbl_ytg.Text = conPAL[3].ToString("0.0000");
                lbl_xtb.Text = conPAL[4].ToString("0.0000");
                lbl_ytb.Text = conPAL[5].ToString("0.0000");
                lbl_xtw.Text = conPAL[6].ToString("0.0000");
                lbl_ytw.Text = conPAL[7].ToString("0.0000");

                string svm = mp.DecToBin(Convert.ToInt16(svuiuser[svGAMUTreg]), 16);
                svm = mp.ReplaceAt(svm, 15, "0");
                svm = mp.ReplaceAt(svm, 16, "1");
                svuiuser[svGAMUTreg] = mp.BinToDec(svm).ToString();
            }
            else if (cgkw == "(NTSC)")
            {
                lbl_xtr.Text = conNTSC[0].ToString("0.0000");
                lbl_ytr.Text = conNTSC[1].ToString("0.0000");
                lbl_xtg.Text = conNTSC[2].ToString("0.0000");
                lbl_ytg.Text = conNTSC[3].ToString("0.0000");
                lbl_xtb.Text = conNTSC[4].ToString("0.0000");
                lbl_ytb.Text = conNTSC[5].ToString("0.0000");
                lbl_xtw.Text = conNTSC[6].ToString("0.0000");
                lbl_ytw.Text = conNTSC[7].ToString("0.0000");

                string svm = mp.DecToBin(Convert.ToInt16(svuiuser[svGAMUTreg]), 16);
                svm = mp.ReplaceAt(svm, 15, "1");
                svm = mp.ReplaceAt(svm, 16, "0");
                svuiuser[svGAMUTreg] = mp.BinToDec(svm).ToString();
            }
            else if (cgkw == "(User)")
            {
                lbl_xtr.Text = userxy[0].ToString("0.0000");
                lbl_ytr.Text = userxy[1].ToString("0.0000");
                lbl_xtg.Text = userxy[2].ToString("0.0000");
                lbl_ytg.Text = userxy[3].ToString("0.0000");
                lbl_xtb.Text = userxy[4].ToString("0.0000");
                lbl_ytb.Text = userxy[5].ToString("0.0000");
                lbl_xtw.Text = userxy[6].ToString("0.0000");
                lbl_ytw.Text = userxy[7].ToString("0.0000");
            }
            else if (cgkw == "(PID)")
            {
                lbl_xtr.Text = pidxy[0].ToString("0.0000");
                lbl_ytr.Text = pidxy[1].ToString("0.0000");
                lbl_xtg.Text = pidxy[2].ToString("0.0000");
                lbl_ytg.Text = pidxy[3].ToString("0.0000");
                lbl_xtb.Text = pidxy[4].ToString("0.0000");
                lbl_ytb.Text = pidxy[5].ToString("0.0000");
                lbl_xtw.Text = xct.ToString("0.0000");
                lbl_ytw.Text = yct.ToString("0.0000");

                string svm = mp.DecToBin(Convert.ToInt16(svuiuser[svGAMUTreg]), 16);
                svm = mp.ReplaceAt(svm, 15, "0");
                svm = mp.ReplaceAt(svm, 16, "0");
                svuiuser[svGAMUTreg] = mp.BinToDec(svm).ToString();
            }
            if (mvars.demoMode == false)
            {
                if (cgkw == "(PID)")
                {
                    svuiuser[svCGMATA1] = "16384";
                    svuiuser[svCGMATA2] = "0";
                    svuiuser[svCGMATA3] = "0";
                    svuiuser[svCGMATA4] = "0";
                    svuiuser[svCGMATA5] = "16384";
                    svuiuser[svCGMATA6] = "0";
                    svuiuser[svCGMATA7] = "0";
                    svuiuser[svCGMATA8] = "0";
                    svuiuser[svCGMATA9] = "16384";
                }
                else
                {
                    string[] sva = new string[9];
                    bool svb = invma.CWCGcal(ref sva);
                    svuiuser[svCGMATA1] = sva[0];
                    svuiuser[svCGMATA2] = sva[1];
                    svuiuser[svCGMATA3] = sva[2];
                    svuiuser[svCGMATA4] = sva[3];
                    svuiuser[svCGMATA5] = sva[4];
                    svuiuser[svCGMATA6] = sva[5];
                    svuiuser[svCGMATA7] = sva[6];
                    svuiuser[svCGMATA8] = sva[7];
                    svuiuser[svCGMATA9] = sva[8];
                }
                this.Enabled = false;
                svuiuser[svBriENreg] = "1";

                #region 同 Gamma 調整
                mvars.nvBoardcast = true;
                mvars.lblCompose = "USER_GAMUT";
                if (mvars.nualg)
                {
                    string svdeviceID = mvars.deviceID;
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                    byte svFPGAsel = mvars.FPGAsel;
                    mvars.FPGAsel = 2;

                    //mp.cUIREGADRwALL(svuiuser, false);
                    //frm_scrControl.lstget1.Items.Add("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",R64K " + mvars.strR64K);
                    mvars.lblCmd = "FPGA_REG_W";
                    for (byte svi = 0; svi < 2; svi++)
                    {
                        string[] RegDec = new string[svuiuser.Length];
                        string[] DataDec = new string[svuiuser.Length];
                        mvars.lblCmd = "FPGA_REG_W";
                        for (int svj = 0; svj < svuiuser.Length; svj++) { RegDec[svj] = svj.ToString(); DataDec[svj] = svuiuser[svj]; }
                        mp.mpFPGAUIREGWarr(RegDec, DataDec);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                    }
                    //mvars.lblCmd = "MCUBRIGPERCENT";
                    //mp.mpMCUCCT(Convert.ToInt16(svuiuser[svCCTreg]), true, true);
                    mp.doDelayms(100);
                    //不需要分左右畫面
                    byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                    for (UInt16 i = 0; i < svuiuser.Length; i++)
                    {
                        BinArr[i * 4 + 0] = (Byte)(i / 256);
                        BinArr[i * 4 + 1] = (Byte)(i % 256);
                        BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(svuiuser[i]) / 256);
                        BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(svuiuser[i]) % 256);
                    }
                    //Checksum
                    UInt16 checksum = mp.CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                    BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                    BinArr[BinArr.Length - 1] = (byte)(checksum % 256);
                    mvars.lblCmd = "MCU_FLASH_W64000";
                    mp.mhMCUFLASHWRITE("64000", ref BinArr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }

                    mvars.deviceID = svdeviceID;
                    mvars.FPGAsel = svFPGAsel;
                }
                else
                {
                    int svsum = 0;
                    mvars.chkcf[1].Checked = true;
                    if (mvars.nualg) { svuiuser[svBriENreg] = "1"; }
                    else
                    {
                        for (int i = 0; i < mvars.chkcf.Length; i++)
                        {
                            if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
                        }
                        mvars.flgSelf = true;
                        svuiuser[0] = svsum.ToString();
                    }
                    //mp.cUSERBRIG(svsum, Convert.ToInt16(svuiuser[5]), Convert.ToInt16(svuiuser[6]), Convert.ToInt16(svuiuser[7]));
                    //frm_scrControl.lstget1.Items.Add("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + "," + svuiuser[5] + "," + svuiuser[6] + "," + svuiuser[7]);
                    //mp.funSaveLogs("adjBrightness," + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + "," + svuiuser[5] + "," + svuiuser[6] + "," + svuiuser[7]);
                }
                #endregion 同 Gamma 調整
                this.Enabled = true;
            }
        }

        private void tbar_gr_Scroll(object sender, EventArgs e)
        {
            TrackBar tbar = (TrackBar)sender;

            if (tbar.Tag.ToString() == "R") 
            {
                lbl_gr.Text = "(" + tbar.Value + "%)"; 
            }
            else if (tbar.Tag.ToString() == "G") 
            {
                lbl_gg.Text = "(" + tbar.Value + "%)";
            }
            else if (tbar.Tag.ToString() == "B") 
            {
                lbl_gb.Text = "(" + tbar.Value + "%)";
            }
        }

        private void tbar_gr_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBar tbar = (TrackBar)sender;

            svuiuser[svBriENreg] = "1";
            //int svv = 10;
            if (tbar.Tag.ToString() == "R")
            {
                svuiuser[svUserRreg] = string.Format("{0:#00}", (Math.Pow(tbar_gr.Value / Convert.ToSingle(100), (1 / uc_PictureAdjust.usergv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax)));
            }
            else if (tbar.Tag.ToString() == "G")
            {
                svuiuser[svUserGreg] = string.Format("{0:#00}", (Math.Pow(tbar_gg.Value / Convert.ToSingle(100), (1 / uc_PictureAdjust.usergv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax)));
            }
            else if (tbar.Tag.ToString() == "B")
            {
                svuiuser[svUserBreg] = string.Format("{0:#00}", (Math.Pow(tbar_gb.Value / Convert.ToSingle(100), (1 / uc_PictureAdjust.usergv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax)));
            }

            cctmanual = true;
            svuiuser[svCCTreg] = userCCT.ToString();
            mvars.nvBoardcast = true;
            mvars.lblCompose = "USER_CCT";
            if (mvars.nualg)
            {
                string svdeviceID = mvars.deviceID;
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                byte svFPGAsel = mvars.FPGAsel;
                mvars.FPGAsel = 2;

                mvars.lblCmd = "FPGA_REG_W";
                for (byte svi = 0; svi < 2; svi++)
                {
                    string[] RegDec = new string[svuiuser.Length];
                    string[] DataDec = new string[svuiuser.Length];
                    mvars.lblCmd = "FPGA_REG_W";
                    for (int svj = 0; svj < svuiuser.Length; svj++) { RegDec[svj] = svj.ToString(); DataDec[svj] = svuiuser[svj]; }
                    mp.mpFPGAUIREGWarr(RegDec, DataDec);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                }

                mvars.deviceID = svdeviceID;
                mvars.FPGAsel = svFPGAsel;
            }
            else
            {
                int svsum = 0;
                mvars.chkcf[1].Checked = true;
                if (mvars.nualg) { svuiuser[svBriENreg] = "1"; }
                else
                {
                    for (int i = 0; i < mvars.chkcf.Length; i++)
                    {
                        if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
                    }
                    mvars.flgSelf = true;
                    svuiuser[0] = svsum.ToString();
                }
            }

            //CG
            float svxct = 0;
            float svyct = 0;
            float svrratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserRreg]) / Convert.ToSingle(lvgraymax), usergv);
            float svgratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserGreg]) / Convert.ToSingle(lvgraymax), usergv);
            float svbratio = (float)Math.Pow(Convert.ToSingle(svuiuser[svUserBreg]) / Convert.ToSingle(lvgraymax), usergv);
            invma.CWWPxy(ref svxct, ref svyct, svrratio, svgratio, svbratio);
            txt_newctxy2.Text = svxct + "," + svyct;
            xct = svxct;
            yct = svyct;
            lbl_xw.Text = xct.ToString();
            lbl_yw.Text = yct.ToString();
        }

        private void txt_focus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && tbar_brightness.Value > 0)
            {
                tbar_brightness.Value -= 1;
            }
            if (e.KeyCode == Keys.Right && tbar_brightness.Value < 100)
            {
                tbar_brightness.Value += 1;
            }
            lbl_brightness.Text = "(" + tbar_brightness.Value.ToString() + "%)";
        }

        private void txt_focus_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                tbar_brightness_MouseUp(null, null);
        }

        private void tbar_brightness_MouseHover(object sender, EventArgs e)
        {
            txt_focus.Focus();
        }
    }
}
