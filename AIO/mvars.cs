using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Net.NetworkInformation;

namespace AIO
{
    public struct typUUT
    {
        public float CLv;
        public float Cx;
        public float Cy;
        public float Cz;
        public byte CAch;               // CA頻道
        public float OverBet;           // 原本宣告給CAATG
        public byte GMAOPT;             // 1.3_5光學檢測版
        public int intSpace;
        public string gmafilepath;
        public int CoolCounts;
        public byte MTP;
        public float WTLvBet;
        public string[,] Disk;          // 在Form1_Load中宣告
        public byte extendDisk;
        public byte existedDisk;        // 準備用來作為自訂連線的網路磁碟，已經存在的，例. 最後一個已知存在的是D(Chr(68))  自訂連線則取E來使用
        public byte emptyDisk;          // 準備用來作為自訂連線的網路磁碟，已經存在的最接近，例. E(Chr(69))~G(Chr(71)) 之間取E來使用
        public float resW;
        public float resH;
        public float screenScalingFactor;
        public string ID;               // 模組序號
        public byte ALLCounts;
        public float DLvLimit;
        public float DLvTolminus;
        public float DLvTolplus;
        public string[] gammavalue;
        public byte GMAposATD;          // 1.3_5光學檢測版(S12A), C12A改為AutoGamma 位置偵測
        public byte exGray;
        public float[] ex20d10;
        public float VREF;
    }

    public struct typTuningArea
    {
        public string Mark;
        public int tW;
        public int tH;
        public int mX;
        public int mY;
        public bool TMode;
        public int Loading;
        public int Bwdots;
        public int Bhdots;
        public float rX;
        public float rY;
        public float eX;    //Autogamma方塊放大倍率
        public float eY;    //Autogamma方塊放大倍率
    }

    public struct typMsrDataNormal
    {
        public float Lv;
        public float Sx;
        public float Sy;
    }

    public struct typGammaIC
    {
        public string[,] Addr;
        public string[,] Data;
        //在mNOVAIC給預設值
        public float[] X;
        public float[] Y;
        public float[] Z;
    }

    public struct CrcInfoTypeS
    {
        public UInt32 Poly;
        public UInt32 CrcInit;
        public UInt32 XorOut;
        public bool RefIn;
        public bool RefOut;
    }

    public struct typNVSend
    {
        #region regBoxMark 說明
        /*
        C12A  [0] 0未選,1from,2to,3end
              [1] 發送卡 [2] 網口 [3] 接收卡 [4] 座標X [5] 座標Y [6] W [7] H [8] DrawX [9]DrawY [10]三角形的p1X [11]三角形的p1Y [12]三角形的p2X [13]三角形的p2Y [14]三角形的p3X [15]三角形的p3Y [16]BOXGAP [17]mcu ver [18]fpga ver
        */
        //public string[,] regBoxMark;      //[svpo, svcard] C12B
        //public ushort[] regPoCards;       //16個Port的連線點數
        /*
        Primary [0]
                [1]HDMI主編號(由1起算)
                [2]各組HDMI序號(由1起算) 
                [3]1=Start,2=body,99=Tail,100=single 
                [4]背面走線右側起算座標X 
                [5]由下往上座標Y 
                [6]欄寬 W 
                [7]列高 H 
                [8]正面走線左側起算座標X 
                [9]由上往下座標Y 
                [10]解析度W 
                [11]解析度H 
                [12]每個HDMI輸出內的X位置(背面) 
                [13]每個HDMI輸出內的Y位置(背面) 
                [14] 
                [15]HDMI R 
                [16]HDMI G 
                [17]HDMI B
                [18]verMCU 
                [19]verFPGA
                [20]verEDID
                [21]in485
                [22]inHDMI
                                                                                              背面走線座標X(從右到左走線)
        */
        public string[] regBoxMark;     //[svpo] svpo=deviceID  svcard不使用
        #endregion regBoxMark
        public ushort regPoCards;       //16個Port的連線點數
    }

    public struct typhwCard
    {
        public string verFPGA;
        public string verMCUA;
        public string verMCUB;
        public string verMCU;
        public string verEDID;

        public short CurrentBootVer;
        public short CurrentAppVer;

        public bool nualg;
        public byte pcbaver;
    }




    class mvars
    {
        //↓↑

        #region Hengchieh
        public static bool bHID = false;
        public static string flashJEDECID;
        public static byte hFuncStatus = 0xFF;   //QSPI Flash Status
        //2019.11.19 Hengchieh Added
        public static byte[] gFlashRdPacketArr = new byte[32768];
        public static Byte ReturnOK = 0x03;
        public static byte SercomCmdClk = 0xFF;
        public static byte SercomCmdRd = 0xFF;
        public static byte SercomCmdWr = 0x50;
        public static byte SercomCmdWrRd = 0x51;
        public static string sIP = "0001";

        #region uc_coding
        public static byte[] gMcuBinFile = new byte[MCU_FLASH_SIZE];
        public static bool[] gbBlockWrite = new bool[MCU_FLASH_SIZE / MCU_BLOCK_SIZE];
        public static byte[] McuFlashArr = new byte[MCU_FLASH_SIZE];
        public static int MCU_APP_VERSION_ADDR = 0x0007FFE0;
        public static int MCU_INFO_ADDR = 0x7FF00;
        public static int MCU_BOOT_INFO_ADDR = 0xFF00;
        public static int MCU_BOOT_VERSION_ADDR = 0xFFF0;

        #endregion uc_coding

        #region constant
        public const UInt32 MCU_FLASH_SIZE = 524288;
        public const UInt32 APP_SIZE = 0x70000;
        public const UInt32 BOOT_SIZE = 0x10000;
        public const UInt32 BOOT_END_ADDR = BOOT_SIZE - 1;
        public const UInt32 APP_START_ADDR = BOOT_END_ADDR + 1;
        public const UInt32 APP_END_ADDR = APP_START_ADDR + APP_SIZE - 1;
        public const UInt32 FPGA_SIZE = 0x0400;


        public static UInt32 GAMMA_SIZE = 8192;     //8192 目前是 Primary 機種專用，如果要跟其他機種共用則需要加條件式區別
        public const UInt32 FPGA_MTP_SIZE = 0x2000;    //8192 目前是 Primary 機種專用，如果要跟其他機種共用則需要加條件式區別
        public const UInt32 GAMMA_MTP_SIZE = 8192;     //8192 目前是 Primary 機種專用，如果要跟其他機種共用則需要加條件式區別


        public const UInt32 NVM_USER_PAGE = 512;
        public const UInt32 NVM_USER_PAGE_START = 0x00804000;
        public const UInt32 NVM_USER_PAGE_END = NVM_USER_PAGE_START + NVM_USER_PAGE;

        public const UInt32 MCU_BLOCK_SIZE = 8192;
        public const UInt32 MCU_ERASE_SIZE = 8192;

        ////h5512a
        //public const UInt32 FPGA_START_ADDR = 0x30000;
        //public const UInt32 FPGA_END_ADDR = FPGA_START_ADDR + FPGA_SIZE - 1;
        //public const UInt32 GAMMA_SIZE = 2048;
        //public const UInt32 GAMMA_START_ADDR = 0x32000;
        //public const UInt32 GAMMA_END_ADDR = GAMMA_START_ADDR + GAMMA_SIZE - 1;
        ////c12a
        //public const UInt32 FPGA_START_ADDR = 0x60000;
        //public const UInt32 FPGA_END_ADDR = FPGA_START_ADDR + FPGA_SIZE - 1;
        //public const UInt32 GAMMA_SIZE = 2048;
        //public const UInt32 GAMMA_START_ADDR = 0x62000;
        //public const UInt32 GAMMA_END_ADDR = GAMMA_START_ADDR + GAMMA_SIZE - 1;

        #endregion constant

        public static UInt32 FPGA_START_ADDR = 0x60000;
        public static UInt32 GAMMA_START_ADDR = 0x62000;

        #endregion Hengchieh


        #region uc_C12Ademura
        public static CheckBox[] chkcf = new CheckBox[6];
        public static CheckBox[] chkomsel = new CheckBox[4];

        public static NumericUpDown numUDWTR = null;
        public static NumericUpDown numUDWTG = null;
        public static NumericUpDown numUDWTB = null;
        public static CheckBox chkWTONOFF = null;

        public static NumericUpDown numUDegma = null;
        public static ComboBox cmbegma = null;

        public static ListBox lstmcuW60000 = null;
        public static ListBox lstmcuR60000 = null;
        public static ListBox lstmcuW62000 = null;
        public static ListBox lstmcuR62000 = null;
        public static ListBox lstmcuW64000 = null;
        public static ListBox lstmcuR64000 = null;
        public static ListBox lstmcuW66000 = null;
        public static ListBox lstmcuR66000 = null;
        public static ListBox lstmcuW68000 = null;
        //public static ListBox lstmcuR68000 = null;

        public static Button btnmcuW60000 = null;
        public static Button btnmcuR60000 = null;
        public static Button btnmcuW60000cls = null;
        public static Button btnmcuR60000cls = null;

        public static Button btnmcuW62000 = null;
        public static Button btnmcuR62000 = null;
        public static Button btnmcuW62000cls = null;
        public static Button btnmcuR62000cls = null;

        public static Button btnmcuW64000 = null;
        public static Button btnmcuR64000 = null;
        public static Button btnmcuW64000cls = null;
        public static Button btnmcuR64000cls = null;

        public static Button btnmcuW66000 = null;
        public static Button btnmcuR66000 = null;
        public static Button btnmcuW66000cls = null;
        public static Button btnmcuR66000cls = null;

        //public static Button btnmcuW68000 = null;
        //public static Button btnmcuR68000 = null;
        //public static Button btnmcuW68000cls = null;
        //public static Button btnmcuR68000cls = null;

        #endregion uc_C12Ademura



        #region A
        public static string ATGerr = "0";
        public static string actFunc = "";
        #endregion


        #region B
        public static bool Break;
        public static bool byPass = false;
        public static string BINtype = "";
        public static string[] binhead;

        public static int boxRows = 1;
        public static int boxCols = 1;

        #endregion


        #region C↓
        public static string[] Comm;
        //public static string CtrlSysComm;
        public static string[,] cm603Addr_c12a = { { "R", "M" }, { "G", "B" } };    // [A0,Addr] 預設 R
        //6Z01M007WC004_UG1_R_CM603_IQ089ZJ-01L_0317_0x12B8 _vendor.bin
        //6Z01M007WD004_UG2_G_CM603_IQ089ZJ-01L_0317_0x1182_vendor.bin
        //6Z01M007WE004_UG3_B_CM603_IQ089ZJ-01L_0317_0x132C_vendor.bin

        //1050                              // 00    01    02    03    04    05    06    07    08    09    0A    0B    0C    0D    0E    0F
        public static string[,] cm603df = { { "00", "00", "00", "00", "06", "D0", "34", "55", "2A", "D8", "2E", "AF", "3A", "8A", "2E", "68",
                                              "1A", "44", "06", "22", "36", "00", "21", "D8", "39", "51", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "00", "00", "01", "D1", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "03", "84", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "1D", "1A",
                                              "1E", "AB", "34", "55", "36", "00", "21", "87", "09", "60", "2D", "41", "25", "24", "15", "06",
                                              "24", "E6", "18", "C5", "10", "2B", "15", "AB", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "01", "D1", "00", "00", "03", "84", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "18", "02", "00", "00" },

                                            { "00", "00", "00", "00", "3D", "00", "34", "55", "23", "06", "06", "D0", "2E", "9A", "1E", "6C",
                                              "16", "3C", "2E", "11", "25", "E3", "09", "B4", "01", "49", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "00", "00", "01", "D1", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "03", "84", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "1D", "1A",
                                              "3D", "00", "34", "55", "36", "00", "15", "7F", "2D", "52", "05", "2D", "1D", "09", "3C", "E4",
                                              "18", "BC", "38", "93", "10", "2B", "34", "55", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "01", "D1", "00", "00", "03", "84", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "18", "02", "00", "00" },

                                            { "00", "00", "00", "00", "34", "55", "34", "55", "16", "FB", "3A", "C6", "02", "92", "1A", "62",
                                              "06", "31", "16", "09", "1D", "DD", "39", "B0", "05", "47", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "00", "00", "01", "D1", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "03", "84", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "1D", "1A",
                                              "34", "55", "34", "55", "36", "00", "05", "80", "35", "50", "3D", "26", "08", "FD", "0C", "D5",
                                              "24", "AA", "24", "7E", "10", "2B", "34", "55", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "01", "D1", "00", "00", "03", "84", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                              "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "14", "03", "00", "00" }};
        public static byte[,] cm603dfB = new byte[3, 128];
        public static string[] cm603exp = {
            "00h soft ware WP1", "01h soft ware WP2",
            "02h Gamma01 of BANK1", "03h Gamma02 of BANK1", "04h Gamma03 of BANK1", "05h Gamma04 of BANK1", "06h Gamma05 of BANK1", "07h Gamma06 of BANK1", "08h Gamma07 of BANK1", "09h Gamma08 of BANK1", "0Ah Gamma09 of BANK1", "0Bh Gamma10 of BANK1", "0Ch Gamma11 of BANK1", "0Dh Gamma12 of BANK1",
            "0Eh Reserved", "0Fh Reserved", "10h CMI01", "11h CMI02", "12h VCOM1 of BANK1", "13h Reserved", "14h Reserved", "15h Reserved", "16h Reserved", "17h Reserved", "18h VCOM1 MIN of BANK1", "19h VCOM1 MAX of BANK1",
            "1Ah CMI03", "1Bh Reserved", "1Ch Reserved", "1Dh Reserved", "1Eh Reserved", "1Fh VREF",
            "20h Gamma01 of BANK2", "21h Gamma02 of BANK2", "22h Gamma03 of BANK2", "23h Gamma04 of BANK2", "24h Gamma05 of BANK2", "25h Gamma06 of BANK2", "26h Gamma07 of BANK2", "27h Gamma08 of BANK2", "28h Gamma09 of BANK2", "29h Gamma10 of BANK2", "2Ah Gamma11 of BANK2", "2Bh Gamma12 of BANK2",
            "2Ch Reserved", "2Dh Reserved", "2Eh Reserved", "2Fh Reserved" , "30h VCOM1 of BANK2" , "31h VCOM1 MIN of BANK2", "32h VCOM1 MAX of BANK2", "33h Reserved", "34h Reserved", "35h Reserved" ,
            "36h CMI04", "37h MTP remain time", "38h CMI05", "39h CMI06", "3Ah CMI07", "3Bh CMI08", "3Ch CMI09", "3Dh CMI10", "3Eh Control Register", "3Fh Status Register" };
        public static byte[,] cm603WA0Ctrl = { { 0xD4, 0xD8 }, { 0xD6, 0xDA } };
        public static byte[,] cm603RA0Ctrl = { { 0xD5, 0xD9 }, { 0xD7, 0xDB } };
        public static byte cm603WRaddr = 0xD4;
        public static float[] cm603Vref = new float[3]; //[0]R,[1]G,[2]B
        public static int[] cm603VrefCode = new int[3]; //[0]R,[1]G,[2]B
        public static int[] cm603Gamma = new int[9] { 0, 32, 64, 96, 128, 160, 192, 224, 255 };//RM93C30 AGND+0.1(壓差0.2)<=VGMA9<=VGMA8<=VGMA7<=VGMA6~<=VGMA1<=AVDD-0.1(壓差0.2)
        public static float[,] cm603gmaRatio = new float[2, 256];
        public static byte c12aflashitem = 0;   //0 CB/XB ，1 panel
        public static string[] cctlist = null;
        public static string[] ccttable = null;
        public static int cctmax = 12000;
        public static int cctmin = 5000;
        public static float[,] cgma1dv = new float[,] { { 0.1f, 0.1f, 0.04f }, { 0.1f, 0.1f, 0.04f } }; //[0,0]BK0R,[0,1]BK0G,[0,2]BK0B

        #region Code Protect
        public static byte[] NVM_UserPage = new byte[512];
        public static string sBOD33 = "BOD33 Disable", sBootSizeVal = "0", sBOD_Level = "", sHyst = "", sBODplus = "", sBODminus = "";
        #endregion

        #endregion C↑


        #region D↓
        public static int Delaymillisec;
        public static bool demoMode = false;
        public static DataGridView dgvBin = null;
        public static Button dgvBtn = new Button();
        public static string defaultGammafile = "";
        //↓ 0709 20221005
        public static string[] deviceAll = { "C12A", "H5512A", "Customize_UI", "C12B", "Primary", "TV130", "CarpStreamer" };
        public static string[] deviceIDall = { "0200", "0300", "0400", "0200", "0500", "0600", "1000" };
        public static string deviceID = "";
        public static string deviceName = "";       
        public static string deviceNo = "";         // 等同於 VAM Control(Q)的 mvars.GMAindex
        //↓目前先使用在C12A/B的CM603數量顯示  C12A無PB=3顆CM603+IN516，C12B舊版PB=3顆CM603+IN525，C12B新版PB=4顆CM603+IN525(再細分過渡版/正式版)
        public static string deviceNameSub = "";    //A，B，B(4t)=4temp版，B(4)=新正式版
        //public static bool dualduty = true;
        public static byte dualduty = 0;
        public static byte[] duty = new byte[] { 12 };  // 依峻富預設 1/12，bk0
        public static string dropvalue = "240";
        public static int dgvRows;
        #endregion D↑


        #region delta
        public static float deltaMin = 10000;
        public static int deltaMinAt = 0;
        public static float[] deltaRec = new float[200];
        public static string[,] deltaData = new string[3, 2];
        public static string[] deltaMinXYZ = new string[3];
        public static string deltaG0V0;

        public static float[,] deltaRGBLv = new float[200, 3];
        public static float[,] deltaRGBsx = new float[200, 3];
        public static float[,] deltaRGBsy = new float[200, 3];
        public static float[] deltadtXYZdg = new float[] { 0.003f, 0.002f, 0.002f };
        #endregion delta


        #region E
        public static string errCode = "0";
        public static bool errClose = false;
        #endregion E


        #region F
        public static bool flgSend;
        public static bool flgReceived;
        public static bool flgDelFB;
        public static bool flgSelf;
        public static bool flgDirMTP;
        public static byte flashselQ = 0;   //C12A 0:Default(每次使用完需要切回0),1:FPGA,2:DEMURA
        public static bool[] FormShow = new bool[21];
        public static long[] FLashWindex = new long[2];
        public static int FlashWriteBuffer = 2048;
        public static int FlashReadBuffer = 2048;
        public static bool FlashNonDivided = false;
        public static bool flgDirBrig = false;
        public static bool flgsenderonly = false;
        public static bool flgerrRead = false;
        public static bool flgdirGamma = false;
        public static bool flgex20d10 = false;
        public static byte FPGAsel = 2;
        public static bool flgsuperuser = false;
        public static bool flgbootloader = false;   //uc_coding
        public static bool flgForceUpdate = false;  //uc_coding

        public static bool flgValidate = false;
        public static bool[] flgValidateR = new bool[5];

        public static bool flgSendmessage = false;
        
        #endregion F


        #region FormShow
        /*
            [ 0] 
            [ 1] 
            [ 2]    Form2
            [ 3] 
            [ 4]
            [ 5]    i3_Pat
            [ 6]    i3Init
            [ 7]    uc_C12Ademura
            [ 8]    uc_FPFAreg
            [ 9]    
            [10]    uc_atg
            [11]    
        */
        #endregion


        #region G
        public static float[] Gamma2d2;
        public static int GMAterminals;
        public static float GMAvalue = (float)3.0;  //TV_BL
        #endregion G


        #region H
        public static int handleIDfrom;
        public static int handleIDMe = 0;
        public static byte hscF2sel;            //   hscF2sel   RForm2.hscroll1 GForm2.hscroll2 BForm2.hscroll3
        public static ushort HexBootVer;
        public static ushort HexAppVer;
        #endregion H


        #region I
        public static byte iPBaddr = 1;     //1~4,5~8,9~12,13~16
        public static bool isReadBack = true;
        public static string[] in525exp = {
            "00h WP1_A", "01h WP1_B",
            "02h ICD STR[3:0]", "03h ICD_DT[3:0]", "04h ICD_CNT[3:0]",
            "05h ICD_VGH[7:6] ICD_VGL[5:4] ICD_VCOM1[3:2] ICD_VCOM2[1:0]",
            "06h CHN_DCT[2:0]", "07h CHN_RST[1:0]", "08h CHN_DIS1[7:0]", "09h CHN_DIS2[6:0]", "0Ah BST_OUT[5:0]",
            "0Bh BST_HVS[3:0]", "0Ch BST_CLM[2:0]", "0Dh BST_SFT[0]",
            "0Eh BST_GD[1] BST_NMOS[0]", "0Fh BST_RES[1:0]", "10h BK1_OUT[3:0]", "11h BK2_OUT[4:0]",
            "12h BK3_OUT[4:0]", "13h BK4_OUT[5:0]", "14h BK4_HVS[3:0]", "15h BK1_DLY[3:0]", "16h BK2_DLY[3:0]",
            "17h BK3_DLY[3:0]", "18h BK3_CLM[1:0]", "19h BK_TYP[2:0]","1Ah VGH_L[5:0]","1Bh VGH_OFS[3:0]",
            "1Ch VGH_HVS[2:0]", "1Dh VGH_NMOS[0]", "1Eh VGH_RES[0]", "1Fh VGH_CLIM[2:0]","20h VGH_VLIM[3:0]",
            "21h VGL[5:0]", "22h VGL_HVS[2:0]", "23h VGL_RES[1:0]", "24h RESET_REV[0]",
            "25h RST_DET_LEV[7:6] RST_DET_DLY[5:4] RST_OUT_DLY[3:2] RST_INR_DLY[1:0]",
            "26h Address_Ctrl[0]", "27h Block_CRC[7:0]", "28h WP2_A[7:0]", "29h WP2_B[7:0]", "2Ah VREF[7:0]",
            "2Bh VREF_HVS[3:0]","2Ch GMA_Range[3:0]", "2Dh VGMA1[7:0]", "2Eh VGMA2[7:0]", "2Fh VGMA3[7:0]" , "30h VGMA4[7:0]" ,
            "31h INX_INT1[7:0]", "32h INX_INT2[7:0]", "33h INX_INT3[7:0]", "34h INX_INT4[7:0]", "35h INX_INT5[7:0]" ,
            "36h INX_INT6[7:0]", "37h Block2_CRC[7:0]", "38h WP3_A[7:0]", "39h WP3_B[7:0]",
            "3Ah VCM_STR[2] VCM_RNG[1:0]", "3Bh VCM1_MAX[7:0]", "3Ch VCM1_MIN[7:0]", "3Dh VCM1[7:0]",
            "3Eh VCM2_MAX[7:0]", "3Fh VCM2_MIN[7:0]","40h VCM2[7:0]", "41h Block3_CRC[7:0]",
            "42h WP4_A[7:0]", "43h WP4_B[7:0]", "44h HVS[0]", "45h Block4_CRC[7:0]", "46h WED_INX[7]", "47h RED_INX[0]",
            "48h INT_FLAG[7]", "49h STA_RST[7]", "4Ah CSB[7:3] PMB[2:0]",
            "4Bh WP_RST[7] OTP_FLAG[4] WP1_FLAG[3] WP2_FLAG[2] WP3_FLAG[1] WP4_FLAG[0]"};
        public static string[,] in525df = {  {"00", "00", "01", "00", "00", "00", "00", "02", "07", "78", "00", "0D", "04", "00", "03", "02",
                                                "0B", "1C", "04", "34", "0D", "00", "05", "05", "00", "05", "1A", "06", "00", "01", "01", "02",
                                                "0F", "08", "00", "00", "00", "21", "00", "F3", "00", "00", "A5", "0D", "0A", "F5", "9B", "A2",
                                                "18", "00", "00", "00", "00", "01", "00", "2A", "00", "00", "07", "9D", "69", "83", "40", "3E",
                                                "6A", "4A", "00", "00", "00", "1F", "00", "00", "80", "00", "00", "0F" } };
        public static byte[,] in525dfB = new byte[1, 0x4C];
        //0709 20221005
        public static string[] I2C_CMD = null;
        //20221005
        public static byte ICver = 0;       //RM93C30 ver

        public static byte in485 = 1;   //1.背面走線 X起點在右側 Y起點在下側   2.
        public static byte inHDMI = 1;    //1.每組HDMI下端入線    2. 每組HDMI背面走線的右側接入

        #endregion I


        #region L
        //public static ListBox lstget = new ListBox();
        public static ListBox lstMsgIn = new ListBox();

        public static string lblCmd;
        public static string lblCompose;
        public static string[] lCmd;
        public static string[] lGet;
        public static int lCount;
        public static int lCounts;          //累計單一命令需要由RS485接收到的計數
        public static Label lblform = new Label();  //=Form1.lbl_form
        public static DriveInfo[] ListDrivesInfo;
        public static int LBw = 170;
        public static int LBh = 96;
        #endregion L


        #region M
        public static string msWn = "";
        public static int mvWn = 0;
        public static string msgA;
        public static string[] msrColor = new string[6] { "BLU", "White", "Red", "Green", "Blue", "Dark" }; //new int[3, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
        public static bool msrgammacurve = false;
        public static int msrgammacurveSt = 0;
        public static int msrgammacurveEd = 41;
        public static int msrgammacurveGp = 1;
        #endregion M


        #region N
        public static int NumBytesToRead;
        public static bool nvBoardcast = false;
        public static bool nIsReadback = false;
        //public static List<string> nvcommList = new List<string>();
        public static bool numcu = false;
        public static bool nualg = false;
        #endregion N


        #region P
        public static typGammaIC[] pGMA = new typGammaIC[1];  //BANK0
        public static string projectName = "";
        #endregion P


        #region R
        public static byte[] RS485_WriteDataBuffer;
        public static byte[] ReadDataBuffer;
        public static float ratioX;
        public static float ratioY;
        public static string[,] RegData = new string[4, 128];    //btn_PAGMA宣告 [0]R,[1]G,[2]B,[3]M
                                                                 //public const string regbookmark = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";  //C12B
        public static string regbookmark = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";  // Primary   ([0]~[22] = 23 bytes)
                                                                                             // 詳細說明在 -->  public struct typNVSend
                                                                                             // 拼接的畫線 -->  nvsender[0].regBoxMark

        #endregion R


        #region S
        //public static SerialPort sp1 = new SerialPort();
        public static SerialPort sp1 = null;
        public static string strLogPath = @"c:\Log\AIO_UI\";
        public static string strUInameMe;
        public static string strReceive = "";
        public static string strStartUpPath;
        public static bool svnova = false;
        public static string strR60K;
        public static string strR62K;
        public static string strR64K;
        public static string strR66K;
        public static string strR66Kdefault = "0,RECORD,255~1,RESERVE,0~2,RESERVE,0~3,RESERVE,0~4,POS X,0~5,POS Y,0~6,W,960~7,H,540~8,DRAW X,0~9,DRAW Y,0";
        public static string[] strData;
        public static string[] strFLASHtype = { "OPEN", "FPGA", "DEMURA" };
        public static byte sckSindex = 0;
        public static byte sckFindex = 0;
        public static string strNamePC = "";
        public static string strNameLogin = "";
        public static int scrW = 960;
        public static int scrH = 540;
        #endregion S


        #region T
        public static Timer tmeRSIn = new Timer();
        public static Timer tmePull = new Timer();
        public static Timer tmeWarn = new Timer();

        public static ToolStripLabel tslblPull = new ToolStripLabel();  //mk5
        public static ToolStripLabel tslblRSIn = new ToolStripLabel();  //mk5

        public static typTuningArea TuningArea;
        public static string txtErrMsg;
        public static DateTime t1;
        public static ToolTip toolTip1 = new ToolTip();
        public static string txtOnCompleteHWInfoConfig = "";
        #endregion T


        #region U
        public static typUUT UUT;
        public static byte[] ucTmp;
        public static string UImajor;
        public static byte UIinTest = 0;
        #endregion U


        #region UIregadr_default
        public static string uiregadrdefault = "";
        public static string[] uiregadr_default = null;

        public static string[] uiregadr_default_L203 =
        {
            "0,MISC,16",
            "1,GMA1,512",
            "2,GMA2,1024",
            "3,GMA3,2048",
            "4,GMA4,4080",
            "5,CT_RED,255",
            "6,CT_GRN,255",
            "7,CT_BLU,255",
            "8,WT_PG_RED,4080",
            "9,WT_PG_GRN,4080",
            "10,WT_PG_BLU,4080",
            "11,WT_FM_RED_GMA1_1,512",
            "12,WT_FM_RED_GMA2_1,1024",
            "13,WT_FM_RED_GMA3_1,2048",
            "14,WT_FM_RED_GMA4_1,4080",
            "15,WT_FM_GRN_GMA1_1,512",
            "16,WT_FM_GRN_GMA2_1,1024",
            "17,WT_FM_GRN_GMA3_1,2048",
            "18,WT_FM_GRN_GMA4_1,4080",
            "19,WT_FM_BLU_GMA1_1,512",
            "20,WT_FM_BLU_GMA2_1,1024",
            "21,WT_FM_BLU_GMA3_1,2048",
            "22,WT_FM_BLU_GMA4_1,4080",
            "23,WT_FM_RED_GMA1_2,512",
            "24,WT_FM_RED_GMA2_2,1024",
            "25,WT_FM_RED_GMA3_2,2048",
            "26,WT_FM_RED_GMA4_2,4080",
            "27,WT_FM_GRN_GMA1_2,512",
            "28,WT_FM_GRN_GMA2_2,1024",
            "29,WT_FM_GRN_GMA3_2,2048",
            "30,WT_FM_GRN_GMA4_2,4080",
            "31,WT_FM_BLU_GMA1_2,512",
            "32,WT_FM_BLU_GMA2_2,1024",
            "33,WT_FM_BLU_GMA3_2,2048",
            "34,WT_FM_BLU_GMA4_2,4080",
            "35,WT_FM_RED_GMA1_3,512",
            "36,WT_FM_RED_GMA2_3,1024",
            "37,WT_FM_RED_GMA3_3,2048",
            "38,WT_FM_RED_GMA4_3,4080",
            "39,WT_FM_GRN_GMA1_3,512",
            "40,WT_FM_GRN_GMA2_3,1024",
            "41,WT_FM_GRN_GMA3_3,2048",
            "42,WT_FM_GRN_GMA4_3,4080",
            "43,WT_FM_BLU_GMA1_3,512",
            "44,WT_FM_BLU_GMA2_3,1024",
            "45,WT_FM_BLU_GMA3_3,2048",
            "46,WT_FM_BLU_GMA4_3,4080",
            "47,WT_FM_RED_GMA1_4,512",
            "48,WT_FM_RED_GMA2_4,1024",
            "49,WT_FM_RED_GMA3_4,2048",
            "50,WT_FM_RED_GMA4_4,4080",
            "51,WT_FM_GRN_GMA1_4,512",
            "52,WT_FM_GRN_GMA2_4,1024",
            "53,WT_FM_GRN_GMA3_4,2048",
            "54,WT_FM_GRN_GMA4_4,4080",
            "55,WT_FM_BLU_GMA1_4,512",
            "56,WT_FM_BLU_GMA2_4,1024",
            "57,WT_FM_BLU_GMA3_4,2048",
            "58,WT_FM_BLU_GMA4_4,4080",
            "59,WT_FM_RED_GMA1_5,512",
            "60,WT_FM_RED_GMA2_5,1024",
            "61,WT_FM_RED_GMA3_5,2048",
            "62,WT_FM_RED_GMA4_5,4080",
            "63,WT_FM_GRN_GMA1_5,512",
            "64,WT_FM_GRN_GMA2_5,1024",
            "65,WT_FM_GRN_GMA3_5,2048",
            "66,WT_FM_GRN_GMA4_5,4080",
            "67,WT_FM_BLU_GMA1_5,512",
            "68,WT_FM_BLU_GMA2_5,1024",
            "69,WT_FM_BLU_GMA3_5,2048",
            "70,WT_FM_BLU_GMA4_5,4080",
            "71,WT_FM_RED_GMA1_6,512",
            "72,WT_FM_RED_GMA2_6,1024",
            "73,WT_FM_RED_GMA3_6,2048",
            "74,WT_FM_RED_GMA4_6,4080",
            "75,WT_FM_GRN_GMA1_6,512",
            "76,WT_FM_GRN_GMA2_6,1024",
            "77,WT_FM_GRN_GMA3_6,2048",
            "78,WT_FM_GRN_GMA4_6,4080",
            "79,WT_FM_BLU_GMA1_6,512",
            "80,WT_FM_BLU_GMA2_6,1024",
            "81,WT_FM_BLU_GMA3_6,2048",
            "82,WT_FM_BLU_GMA4_6,4080",
            "83,WT_FM_RED_GMA1_7,512",
            "84,WT_FM_RED_GMA2_7,1024",
            "85,WT_FM_RED_GMA3_7,2048",
            "86,WT_FM_RED_GMA4_7,4080",
            "87,WT_FM_GRN_GMA1_7,512",
            "88,WT_FM_GRN_GMA2_7,1024",
            "89,WT_FM_GRN_GMA3_7,2048",
            "90,WT_FM_GRN_GMA4_7,4080",
            "91,WT_FM_BLU_GMA1_7,512",
            "92,WT_FM_BLU_GMA2_7,1024",
            "93,WT_FM_BLU_GMA3_7,2048",
            "94,WT_FM_BLU_GMA4_7,4080",
            "95,WT_FM_RED_GMA1_8,512",
            "96,WT_FM_RED_GMA2_8,1024",
            "97,WT_FM_RED_GMA3_8,2048",
            "98,WT_FM_RED_GMA4_8,4080",
            "99,WT_FM_GRN_GMA1_8,512",
            "100,WT_FM_GRN_GMA2_8,1024",
            "101,WT_FM_GRN_GMA3_8,2048",
            "102,WT_FM_GRN_GMA4_8,4080",
            "103,WT_FM_BLU_GMA1_8,512",
            "104,WT_FM_BLU_GMA2_8,1024",
            "105,WT_FM_BLU_GMA3_8,2048",
            "106,WT_FM_BLU_GMA4_8,4080",
            "107,WT_FM_RED_GMA1_9,512",
            "108,WT_FM_RED_GMA2_9,1024",
            "109,WT_FM_RED_GMA3_9,2048",
            "110,WT_FM_RED_GMA4_9,4080",
            "111,WT_FM_GRN_GMA1_9,512",
            "112,WT_FM_GRN_GMA2_9,1024",
            "113,WT_FM_GRN_GMA3_9,2048",
            "114,WT_FM_GRN_GMA4_9,4080",
            "115,WT_FM_BLU_GMA1_9,512",
            "116,WT_FM_BLU_GMA2_9,1024",
            "117,WT_FM_BLU_GMA3_9,2048",
            "118,WT_FM_BLU_GMA4_9,4080",
            "119,WT_FM_RED_GMA1_10,512",
            "120,WT_FM_RED_GMA2_10,1024",
            "121,WT_FM_RED_GMA3_10,2048",
            "122,WT_FM_RED_GMA4_10,4080",
            "123,WT_FM_GRN_GMA1_10,512",
            "124,WT_FM_GRN_GMA2_10,1024",
            "125,WT_FM_GRN_GMA3_10,2048",
            "126,WT_FM_GRN_GMA4_10,4080",
            "127,WT_FM_BLU_GMA1_10,512",
            "128,WT_FM_BLU_GMA2_10,1024",
            "129,WT_FM_BLU_GMA3_10,2048",
            "130,WT_FM_BLU_GMA4_10,4080",
            "131,WT_FM_RED_GMA1_11,512",
            "132,WT_FM_RED_GMA2_11,1024",
            "133,WT_FM_RED_GMA3_11,2048",
            "134,WT_FM_RED_GMA4_11,4080",
            "135,WT_FM_GRN_GMA1_11,512",
            "136,WT_FM_GRN_GMA2_11,1024",
            "137,WT_FM_GRN_GMA3_11,2048",
            "138,WT_FM_GRN_GMA4_11,4080",
            "139,WT_FM_BLU_GMA1_11,512",
            "140,WT_FM_BLU_GMA2_11,1024",
            "141,WT_FM_BLU_GMA3_11,2048",
            "142,WT_FM_BLU_GMA4_11,4080",
            "143,WT_FM_RED_GMA1_12,512",
            "144,WT_FM_RED_GMA2_12,1024",
            "145,WT_FM_RED_GMA3_12,2048",
            "146,WT_FM_RED_GMA4_12,4080",
            "147,WT_FM_GRN_GMA1_12,512",
            "148,WT_FM_GRN_GMA2_12,1024",
            "149,WT_FM_GRN_GMA3_12,2048",
            "150,WT_FM_GRN_GMA4_12,4080",
            "151,WT_FM_BLU_GMA1_12,512",
            "152,WT_FM_BLU_GMA2_12,1024",
            "153,WT_FM_BLU_GMA3_12,2048",
            "154,WT_FM_BLU_GMA4_12,4080",
            "155,WT_FM_RED_GMA1_13,512",
            "156,WT_FM_RED_GMA2_13,1024",
            "157,WT_FM_RED_GMA3_13,2048",
            "158,WT_FM_RED_GMA4_13,4080",
            "159,WT_FM_GRN_GMA1_13,512",
            "160,WT_FM_GRN_GMA2_13,1024",
            "161,WT_FM_GRN_GMA3_13,2048",
            "162,WT_FM_GRN_GMA4_13,4080",
            "163,WT_FM_BLU_GMA1_13,512",
            "164,WT_FM_BLU_GMA2_13,1024",
            "165,WT_FM_BLU_GMA3_13,2048",
            "166,WT_FM_BLU_GMA4_13,4080",
            "167,WT_FM_RED_GMA1_14,512",
            "168,WT_FM_RED_GMA2_14,1024",
            "169,WT_FM_RED_GMA3_14,2048",
            "170,WT_FM_RED_GMA4_14,4080",
            "171,WT_FM_GRN_GMA1_14,512",
            "172,WT_FM_GRN_GMA2_14,1024",
            "173,WT_FM_GRN_GMA3_14,2048",
            "174,WT_FM_GRN_GMA4_14,4080",
            "175,WT_FM_BLU_GMA1_14,512",
            "176,WT_FM_BLU_GMA2_14,1024",
            "177,WT_FM_BLU_GMA3_14,2048",
            "178,WT_FM_BLU_GMA4_14,4080",
            "179,WT_FM_RED_GMA1_15,512",
            "180,WT_FM_RED_GMA2_15,1024",
            "181,WT_FM_RED_GMA3_15,2048",
            "182,WT_FM_RED_GMA4_15,4080",
            "183,WT_FM_GRN_GMA1_15,512",
            "184,WT_FM_GRN_GMA2_15,1024",
            "185,WT_FM_GRN_GMA3_15,2048",
            "186,WT_FM_GRN_GMA4_15,4080",
            "187,WT_FM_BLU_GMA1_15,512",
            "188,WT_FM_BLU_GMA2_15,1024",
            "189,WT_FM_BLU_GMA3_15,2048",
            "190,WT_FM_BLU_GMA4_15,4080",
            "191,WT_FM_RED_GMA1_16,512",
            "192,WT_FM_RED_GMA2_16,1024",
            "193,WT_FM_RED_GMA3_16,2048",
            "194,WT_FM_RED_GMA4_16,4080",
            "195,WT_FM_GRN_GMA1_16,512",
            "196,WT_FM_GRN_GMA2_16,1024",
            "197,WT_FM_GRN_GMA3_16,2048",
            "198,WT_FM_GRN_GMA4_16,4080",
            "199,WT_FM_BLU_GMA1_16,512",
            "200,WT_FM_BLU_GMA2_16,1024",
            "201,WT_FM_BLU_GMA3_16,2048",
            "202,WT_FM_BLU_GMA4_16,4080"
        };

        public static string[] uiregadr_default_L232 =
        {
            "0,MARK,128",
            "1,DMR_ALG_VER,3",
            "2,USR_GMA_EN,1",
            "3,USR_BRI_EN,1",
            "4,WT_PG_EN,0",
            "5,FM_GMA_EN,1",
            "6,BS_DMR_EN,0",
            "7,RECORD_87_1,0",
            "8,RECORD_87_2,0",
            "9,USR_GMT_EN,1",
            "10,PID_GMA,220",
            "11,USR_GMA1,512",
            "12,USR_GMA1,1024",
            "13,USR_GMA1,2048",
            "14,USR_GMA1,4080",
            "15,USR_BRI_RED,1024",
            "16,USR_BRI_GRN,1024",
            "17,USR_BRI_BLU,1024",
            "18,WT_PG_RED,4080",
            "19,WT_PG_GRN,4080",
            "20,WT_PG_BLU,4080",
            "21,CG_MAT_A1,16384",
            "22,CG_MAT_A2,0",
            "23,CG_MAT_A3,0",
            "24,CG_MAT_A4,0",
            "25,CG_MAT_A5,16384",
            "26,CG_MAT_A6,0",
            "27,CG_MAT_A7,0",
            "28,CG_MAT_A8,0",
            "29,CG_MAT_A9,16384",
            "30,CG_RED_X,0",
            "31,CG_RED_Y,0",
            "32,CG_GRN_X,0",
            "33,CG_GRN_Y,0",
            "34,CG_BLU_X,0",
            "35,CG_BLU_Y,0",
            "36,CG_WHI_X,0",
            "37,CG_WHI_Y,0",
            "38,CT_VAL,0",
            "39,USR_GMT,0",
            "40,WT_FM_RED_GMA1_1,512",
            "41,WT_FM_RED_GMA2_1,1024",
            "42,WT_FM_RED_GMA3_1,2048",
            "43,WT_FM_RED_GMA4_1,4080",
            "44,WT_FM_GRN_GMA1_1,512",
            "45,WT_FM_GRN_GMA2_1,1024",
            "46,WT_FM_GRN_GMA3_1,2048",
            "47,WT_FM_GRN_GMA4_1,4080",
            "48,WT_FM_BLU_GMA1_1,512",
            "49,WT_FM_BLU_GMA2_1,1024",
            "50,WT_FM_BLU_GMA3_1,2048",
            "51,WT_FM_BLU_GMA4_1,4080",
            "52,WT_FM_RED_GMA1_2,512",
            "53,WT_FM_RED_GMA2_2,1024",
            "54,WT_FM_RED_GMA3_2,2048",
            "55,WT_FM_RED_GMA4_2,4080",
            "56,WT_FM_GRN_GMA1_2,512",
            "57,WT_FM_GRN_GMA2_2,1024",
            "58,WT_FM_GRN_GMA3_2,2048",
            "59,WT_FM_GRN_GMA4_2,4080",
            "60,WT_FM_BLU_GMA1_2,512",
            "61,WT_FM_BLU_GMA2_2,1024",
            "62,WT_FM_BLU_GMA3_2,2048",
            "63,WT_FM_BLU_GMA4_2,4080",
            "64,WT_FM_RED_GMA1_3,512",
            "65,WT_FM_RED_GMA2_3,1024",
            "66,WT_FM_RED_GMA3_3,2048",
            "67,WT_FM_RED_GMA4_3,4080",
            "68,WT_FM_GRN_GMA1_3,512",
            "69,WT_FM_GRN_GMA2_3,1024",
            "70,WT_FM_GRN_GMA3_3,2048",
            "71,WT_FM_GRN_GMA4_3,4080",
            "72,WT_FM_BLU_GMA1_3,512",
            "73,WT_FM_BLU_GMA2_3,1024",
            "74,WT_FM_BLU_GMA3_3,2048",
            "75,WT_FM_BLU_GMA4_3,4080",
            "76,WT_FM_RED_GMA1_4,512",
            "77,WT_FM_RED_GMA2_4,1024",
            "78,WT_FM_RED_GMA3_4,2048",
            "79,WT_FM_RED_GMA4_4,4080",
            "80,WT_FM_GRN_GMA1_4,512",
            "81,WT_FM_GRN_GMA2_4,1024",
            "82,WT_FM_GRN_GMA3_4,2048",
            "83,WT_FM_GRN_GMA4_4,4080",
            "84,WT_FM_BLU_GMA1_4,512",
            "85,WT_FM_BLU_GMA2_4,1024",
            "86,WT_FM_BLU_GMA3_4,2048",
            "87,WT_FM_BLU_GMA4_4,4080",
            "88,WT_FM_RED_GMA1_5,512",
            "89,WT_FM_RED_GMA2_5,1024",
            "90,WT_FM_RED_GMA3_5,2048",
            "91,WT_FM_RED_GMA4_5,4080",
            "92,WT_FM_GRN_GMA1_5,512",
            "93,WT_FM_GRN_GMA2_5,1024",
            "94,WT_FM_GRN_GMA3_5,2048",
            "95,WT_FM_GRN_GMA4_5,4080",
            "96,WT_FM_BLU_GMA1_5,512",
            "97,WT_FM_BLU_GMA2_5,1024",
            "98,WT_FM_BLU_GMA3_5,2048",
            "99,WT_FM_BLU_GMA4_5,4080",
            "100,WT_FM_RED_GMA1_6,512",
            "101,WT_FM_RED_GMA2_6,1024",
            "102,WT_FM_RED_GMA3_6,2048",
            "103,WT_FM_RED_GMA4_6,4080",
            "104,WT_FM_GRN_GMA1_6,512",
            "105,WT_FM_GRN_GMA2_6,1024",
            "106,WT_FM_GRN_GMA3_6,2048",
            "107,WT_FM_GRN_GMA4_6,4080",
            "108,WT_FM_BLU_GMA1_6,512",
            "109,WT_FM_BLU_GMA2_6,1024",
            "110,WT_FM_BLU_GMA3_6,2048",
            "111,WT_FM_BLU_GMA4_6,4080",
            "112,WT_FM_RED_GMA1_7,512",
            "113,WT_FM_RED_GMA2_7,1024",
            "114,WT_FM_RED_GMA3_7,2048",
            "115,WT_FM_RED_GMA4_7,4080",
            "116,WT_FM_GRN_GMA1_7,512",
            "117,WT_FM_GRN_GMA2_7,1024",
            "118,WT_FM_GRN_GMA3_7,2048",
            "119,WT_FM_GRN_GMA4_7,4080",
            "120,WT_FM_BLU_GMA1_7,512",
            "121,WT_FM_BLU_GMA2_7,1024",
            "122,WT_FM_BLU_GMA3_7,2048",
            "123,WT_FM_BLU_GMA4_7,4080",
            "124,WT_FM_RED_GMA1_8,512",
            "125,WT_FM_RED_GMA2_8,1024",
            "126,WT_FM_RED_GMA3_8,2048",
            "127,WT_FM_RED_GMA4_8,4080",
            "128,WT_FM_GRN_GMA1_8,512",
            "129,WT_FM_GRN_GMA2_8,1024",
            "130,WT_FM_GRN_GMA3_8,2048",
            "131,WT_FM_GRN_GMA4_8,4080",
            "132,WT_FM_BLU_GMA1_8,512",
            "133,WT_FM_BLU_GMA2_8,1024",
            "134,WT_FM_BLU_GMA3_8,2048",
            "135,WT_FM_BLU_GMA4_8,4080",
            "136,WT_FM_RED_GMA1_9,512",
            "137,WT_FM_RED_GMA2_9,1024",
            "138,WT_FM_RED_GMA3_9,2048",
            "139,WT_FM_RED_GMA4_9,4080",
            "140,WT_FM_GRN_GMA1_9,512",
            "141,WT_FM_GRN_GMA2_9,1024",
            "142,WT_FM_GRN_GMA3_9,2048",
            "143,WT_FM_GRN_GMA4_9,4080",
            "144,WT_FM_BLU_GMA1_9,512",
            "145,WT_FM_BLU_GMA2_9,1024",
            "146,WT_FM_BLU_GMA3_9,2048",
            "147,WT_FM_BLU_GMA4_9,4080",
            "148,WT_FM_RED_GMA1_10,512",
            "149,WT_FM_RED_GMA2_10,1024",
            "150,WT_FM_RED_GMA3_10,2048",
            "151,WT_FM_RED_GMA4_10,4080",
            "152,WT_FM_GRN_GMA1_10,512",
            "153,WT_FM_GRN_GMA2_10,1024",
            "154,WT_FM_GRN_GMA3_10,2048",
            "155,WT_FM_GRN_GMA4_10,4080",
            "156,WT_FM_BLU_GMA1_10,512",
            "157,WT_FM_BLU_GMA2_10,1024",
            "158,WT_FM_BLU_GMA3_10,2048",
            "159,WT_FM_BLU_GMA4_10,4080",
            "160,WT_FM_RED_GMA1_11,512",
            "161,WT_FM_RED_GMA2_11,1024",
            "162,WT_FM_RED_GMA3_11,2048",
            "163,WT_FM_RED_GMA4_11,4080",
            "164,WT_FM_GRN_GMA1_11,512",
            "165,WT_FM_GRN_GMA2_11,1024",
            "166,WT_FM_GRN_GMA3_11,2048",
            "167,WT_FM_GRN_GMA4_11,4080",
            "168,WT_FM_BLU_GMA1_11,512",
            "169,WT_FM_BLU_GMA2_11,1024",
            "170,WT_FM_BLU_GMA3_11,2048",
            "171,WT_FM_BLU_GMA4_11,4080",
            "172,WT_FM_RED_GMA1_12,512",
            "173,WT_FM_RED_GMA2_12,1024",
            "174,WT_FM_RED_GMA3_12,2048",
            "175,WT_FM_RED_GMA4_12,4080",
            "176,WT_FM_GRN_GMA1_12,512",
            "177,WT_FM_GRN_GMA2_12,1024",
            "178,WT_FM_GRN_GMA3_12,2048",
            "179,WT_FM_GRN_GMA4_12,4080",
            "180,WT_FM_BLU_GMA1_12,512",
            "181,WT_FM_BLU_GMA2_12,1024",
            "182,WT_FM_BLU_GMA3_12,2048",
            "183,WT_FM_BLU_GMA4_12,4080",
            "184,WT_FM_RED_GMA1_13,512",
            "185,WT_FM_RED_GMA2_13,1024",
            "186,WT_FM_RED_GMA3_13,2048",
            "187,WT_FM_RED_GMA4_13,4080",
            "188,WT_FM_GRN_GMA1_13,512",
            "189,WT_FM_GRN_GMA2_13,1024",
            "190,WT_FM_GRN_GMA3_13,2048",
            "191,WT_FM_GRN_GMA4_13,4080",
            "192,WT_FM_BLU_GMA1_13,512",
            "193,WT_FM_BLU_GMA2_13,1024",
            "194,WT_FM_BLU_GMA3_13,2048",
            "195,WT_FM_BLU_GMA4_13,4080",
            "196,WT_FM_RED_GMA1_14,512",
            "197,WT_FM_RED_GMA2_14,1024",
            "198,WT_FM_RED_GMA3_14,2048",
            "199,WT_FM_RED_GMA4_14,4080",
            "200,WT_FM_GRN_GMA1_14,512",
            "201,WT_FM_GRN_GMA2_14,1024",
            "202,WT_FM_GRN_GMA3_14,2048",
            "203,WT_FM_GRN_GMA4_14,4080",
            "204,WT_FM_BLU_GMA1_14,512",
            "205,WT_FM_BLU_GMA2_14,1024",
            "206,WT_FM_BLU_GMA3_14,2048",
            "207,WT_FM_BLU_GMA4_14,4080",
            "208,WT_FM_RED_GMA1_15,512",
            "209,WT_FM_RED_GMA2_15,1024",
            "210,WT_FM_RED_GMA3_15,2048",
            "211,WT_FM_RED_GMA4_15,4080",
            "212,WT_FM_GRN_GMA1_15,512",
            "213,WT_FM_GRN_GMA2_15,1024",
            "214,WT_FM_GRN_GMA3_15,2048",
            "215,WT_FM_GRN_GMA4_15,4080",
            "216,WT_FM_BLU_GMA1_15,512",
            "217,WT_FM_BLU_GMA2_15,1024",
            "218,WT_FM_BLU_GMA3_15,2048",
            "219,WT_FM_BLU_GMA4_15,4080",
            "220,WT_FM_RED_GMA1_16,512",
            "221,WT_FM_RED_GMA2_16,1024",
            "222,WT_FM_RED_GMA3_16,2048",
            "223,WT_FM_RED_GMA4_16,4080",
            "224,WT_FM_GRN_GMA1_16,512",
            "225,WT_FM_GRN_GMA2_16,1024",
            "226,WT_FM_GRN_GMA3_16,2048",
            "227,WT_FM_GRN_GMA4_16,4080",
            "228,WT_FM_BLU_GMA1_16,512",
            "229,WT_FM_BLU_GMA2_16,1024",
            "230,WT_FM_BLU_GMA3_16,2048",
            "231,WT_FM_BLU_GMA4_16,4080"
        };

        public static string struiregadrdef = "";

        public static string[] uiregadr_default_p =
        {
            "0,MARK,128",
            "1,USR_GMA_EN,1",
            "2,USR_BRI_EN,1",
            "3,USR_CG_EN,1",
            "4,ENG_WT_PG_EN,0",
            "5,ENG_GMA_EN,0",
            "6,ENG_BRI_EN,0",
            "7,STATUS,1",
            "8,IDLE,0",
            "9,IDLE,0",
            "10,GMA1_USR,512",
            "11,GMA2_USR,1024",
            "12,GMA3_USR,2048",
            "13,GMA4_USR,4080",
            "14,CT_RED_USR,1024",
            "15,CT_GRN_USR,1024",
            "16,CT_BLU_USR,1024",
            "17,CG_MAT_A1,16384",
            "18,CG_MAT_A2,0",
            "19,CG_MAT_A3,0",
            "20,CG_MAT_A4,0",
            "21,CG_MAT_A5,16384",
            "22,CG_MAT_A6,0",
            "23,CG_MAT_A7,0",
            "24,CG_MAT_A8,0",
            "25,CG_MAT_A9,16384",
            "26,CG_RED_X,0",
            "27,CG_RED_Y,0",
            "28,CG_GRN_X,0",
            "29,CG_GRN_Y,0",
            "30,CG_BLU_X,0",
            "31,CG_BLU_Y,0",
            "32,CG_WHI_X,0",
            "33,CG_WHI_Y,0",
            "34,CG_CT_VAL,0",
            "35,GMA_VAL,220",
            "36,IDLE,0",
            "37,IDLE,0",
            "38,IDLE,0",
            "39,IDLE,0",
            "40,WT_PG_RED_ENG,4080",
            "41,WT_PG_GRN_ENG,4080",
            "42,WT_PG_BLU_ENG,4080",
            "43,CT_RED_ENG_XB1_1,1024",
            "44,CT_GRN_ENG_XB1_1,1024",
            "45,CT_BLU_ENG_XB1_1,1024",
            "46,CT_RED_ENG_XB1_2,1024",
            "47,CT_GRN_ENG_XB1_2,1024",
            "48,CT_BLU_ENG_XB1_2,1024",
            "49,CT_RED_ENG_XB1_3,1024",
            "50,CT_GRN_ENG_XB1_3,1024",
            "51,CT_BLU_ENG_XB1_3,1024",
            "52,CT_RED_ENG_XB1_4,1024",
            "53,CT_GRN_ENG_XB1_4,1024",
            "54,CT_BLU_ENG_XB1_4,1024",
            "55,CT_RED_ENG_XB2_1,1024",
            "56,CT_GRN_ENG_XB2_1,1024",
            "57,CT_BLU_ENG_XB2_1,1024",
            "58,CT_RED_ENG_XB2_2,1024",
            "59,CT_GRN_ENG_XB2_2,1024",
            "60,CT_BLU_ENG_XB2_2,1024",
            "61,CT_RED_ENG_XB2_3,1024",
            "62,CT_GRN_ENG_XB2_3,1024",
            "63,CT_BLU_ENG_XB2_3,1024",
            "64,CT_RED_ENG_XB2_4,1024",
            "65,CT_GRN_ENG_XB2_4,1024",
            "66,CT_BLU_ENG_XB2_4,1024",
            "67,CT_RED_ENG_XB3_1,1024",
            "68,CT_GRN_ENG_XB3_1,1024",
            "69,CT_BLU_ENG_XB3_1,1024",
            "70,CT_RED_ENG_XB3_2,1024",
            "71,CT_GRN_ENG_XB3_2,1024",
            "72,CT_BLU_ENG_XB3_2,1024",
            "73,CT_RED_ENG_XB3_3,1024",
            "74,CT_GRN_ENG_XB3_3,1024",
            "75,CT_BLU_ENG_XB3_3,1024",
            "76,CT_RED_ENG_XB3_4,1024",
            "77,CT_GRN_ENG_XB3_4,1024",
            "78,CT_BLU_ENG_XB3_4,1024",
            "79,CT_RED_ENG_XB4_1,1024",
            "80,CT_GRN_ENG_XB4_1,1024",
            "81,CT_BLU_ENG_XB4_1,1024",
            "82,CT_RED_ENG_XB4_2,1024",
            "83,CT_GRN_ENG_XB4_2,1024",
            "84,CT_BLU_ENG_XB4_2,1024",
            "85,CT_RED_ENG_XB4_3,1024",
            "86,CT_GRN_ENG_XB4_3,1024",
            "87,CT_BLU_ENG_XB4_3,1024",
            "88,CT_RED_ENG_XB4_4,1024",
            "89,CT_GRN_ENG_XB4_4,1024",
            "90,CT_BLU_ENG_XB4_4,1024",
            "91,WT_GMA1_RED_ENG_XB1_1,512",
            "92,WT_GMA2_RED_ENG_XB1_1,1024",
            "93,WT_GMA3_RED_ENG_XB1_1,2048",
            "94,WT_GMA4_RED_ENG_XB1_1,4080",
            "95,WT_GMA1_GRN_ENG_XB1_1,512",
            "96,WT_GMA2_GRN_ENG_XB1_1,1024",
            "97,WT_GMA3_GRN_ENG_XB1_1,2048",
            "98,WT_GMA4_GRN_ENG_XB1_1,4080",
            "99,WT_GMA1_BLU_ENG_XB1_1,512",
            "100,WT_GMA2_BLU_ENG_XB1_1,1024",
            "101,WT_GMA3_BLU_ENG_XB1_1,2048",
            "102,WT_GMA4_BLU_ENG_XB1_1,4080",
            "103,WT_GMA1_RED_ENG_XB1_2,512",
            "104,WT_GMA2_RED_ENG_XB1_2,1024",
            "105,WT_GMA3_RED_ENG_XB1_2,2048",
            "106,WT_GMA4_RED_ENG_XB1_2,4080",
            "107,WT_GMA1_GRN_ENG_XB1_2,512",
            "108,WT_GMA2_GRN_ENG_XB1_2,1024",
            "109,WT_GMA3_GRN_ENG_XB1_2,2048",
            "110,WT_GMA4_GRN_ENG_XB1_2,4080",
            "111,WT_GMA1_BLU_ENG_XB1_2,512",
            "112,WT_GMA2_BLU_ENG_XB1_2,1024",
            "113,WT_GMA3_BLU_ENG_XB1_2,2048",
            "114,WT_GMA4_BLU_ENG_XB1_2,4080",
            "115,WT_GMA1_RED_ENG_XB1_3,512",
            "116,WT_GMA2_RED_ENG_XB1_3,1024",
            "117,WT_GMA3_RED_ENG_XB1_3,2048",
            "118,WT_GMA4_RED_ENG_XB1_3,4080",
            "119,WT_GMA1_GRN_ENG_XB1_3,512",
            "120,WT_GMA2_GRN_ENG_XB1_3,1024",
            "121,WT_GMA3_GRN_ENG_XB1_3,2048",
            "122,WT_GMA4_GRN_ENG_XB1_3,4080",
            "123,WT_GMA1_BLU_ENG_XB1_3,512",
            "124,WT_GMA2_BLU_ENG_XB1_3,1024",
            "125,WT_GMA3_BLU_ENG_XB1_3,2048",
            "126,WT_GMA4_BLU_ENG_XB1_3,4080",
            "127,WT_GMA1_RED_ENG_XB1_4,512",
            "128,WT_GMA2_RED_ENG_XB1_4,1024",
            "129,WT_GMA3_RED_ENG_XB1_4,2048",
            "130,WT_GMA4_RED_ENG_XB1_4,4080",
            "131,WT_GMA1_GRN_ENG_XB1_4,512",
            "132,WT_GMA2_GRN_ENG_XB1_4,1024",
            "133,WT_GMA3_GRN_ENG_XB1_4,2048",
            "134,WT_GMA4_GRN_ENG_XB1_4,4080",
            "135,WT_GMA1_BLU_ENG_XB1_4,512",
            "136,WT_GMA2_BLU_ENG_XB1_4,1024",
            "137,WT_GMA3_BLU_ENG_XB1_4,2048",
            "138,WT_GMA4_BLU_ENG_XB1_4,4080",
            "139,WT_GMA1_RED_ENG_XB2_1,512",
            "140,WT_GMA2_RED_ENG_XB2_1,1024",
            "141,WT_GMA3_RED_ENG_XB2_1,2048",
            "142,WT_GMA4_RED_ENG_XB2_1,4080",
            "143,WT_GMA1_GRN_ENG_XB2_1,512",
            "144,WT_GMA2_GRN_ENG_XB2_1,1024",
            "145,WT_GMA3_GRN_ENG_XB2_1,2048",
            "146,WT_GMA4_GRN_ENG_XB2_1,4080",
            "147,WT_GMA1_BLU_ENG_XB2_1,512",
            "148,WT_GMA2_BLU_ENG_XB2_1,1024",
            "149,WT_GMA3_BLU_ENG_XB2_1,2048",
            "150,WT_GMA4_BLU_ENG_XB2_1,4080",
            "151,WT_GMA1_RED_ENG_XB2_2,512",
            "152,WT_GMA2_RED_ENG_XB2_2,1024",
            "153,WT_GMA3_RED_ENG_XB2_2,2048",
            "154,WT_GMA4_RED_ENG_XB2_2,4080",
            "155,WT_GMA1_GRN_ENG_XB2_2,512",
            "156,WT_GMA2_GRN_ENG_XB2_2,1024",
            "157,WT_GMA3_GRN_ENG_XB2_2,2048",
            "158,WT_GMA4_GRN_ENG_XB2_2,4080",
            "159,WT_GMA1_BLU_ENG_XB2_2,512",
            "160,WT_GMA2_BLU_ENG_XB2_2,1024",
            "161,WT_GMA3_BLU_ENG_XB2_2,2048",
            "162,WT_GMA4_BLU_ENG_XB2_2,4080",
            "163,WT_GMA1_RED_ENG_XB2_3,512",
            "164,WT_GMA2_RED_ENG_XB2_3,1024",
            "165,WT_GMA3_RED_ENG_XB2_3,2048",
            "166,WT_GMA4_RED_ENG_XB2_3,4080",
            "167,WT_GMA1_GRN_ENG_XB2_3,512",
            "168,WT_GMA2_GRN_ENG_XB2_3,1024",
            "169,WT_GMA3_GRN_ENG_XB2_3,2048",
            "170,WT_GMA4_GRN_ENG_XB2_3,4080",
            "171,WT_GMA1_BLU_ENG_XB2_3,512",
            "172,WT_GMA2_BLU_ENG_XB2_3,1024",
            "173,WT_GMA3_BLU_ENG_XB2_3,2048",
            "174,WT_GMA4_BLU_ENG_XB2_3,4080",
            "175,WT_GMA1_RED_ENG_XB2_4,512",
            "176,WT_GMA2_RED_ENG_XB2_4,1024",
            "177,WT_GMA3_RED_ENG_XB2_4,2048",
            "178,WT_GMA4_RED_ENG_XB2_4,4080",
            "179,WT_GMA1_GRN_ENG_XB2_4,512",
            "180,WT_GMA2_GRN_ENG_XB2_4,1024",
            "181,WT_GMA3_GRN_ENG_XB2_4,2048",
            "182,WT_GMA4_GRN_ENG_XB2_4,4080",
            "183,WT_GMA1_BLU_ENG_XB2_4,512",
            "184,WT_GMA2_BLU_ENG_XB2_4,1024",
            "185,WT_GMA3_BLU_ENG_XB2_4,2048",
            "186,WT_GMA4_BLU_ENG_XB2_4,4080",
            "187,WT_GMA1_RED_ENG_XB3_1,512",
            "188,WT_GMA2_RED_ENG_XB3_1,1024",
            "189,WT_GMA3_RED_ENG_XB3_1,2048",
            "190,WT_GMA4_RED_ENG_XB3_1,4080",
            "191,WT_GMA1_GRN_ENG_XB3_1,512",
            "192,WT_GMA2_GRN_ENG_XB3_1,1024",
            "193,WT_GMA3_GRN_ENG_XB3_1,2048",
            "194,WT_GMA4_GRN_ENG_XB3_1,4080",
            "195,WT_GMA1_BLU_ENG_XB3_1,512",
            "196,WT_GMA2_BLU_ENG_XB3_1,1024",
            "197,WT_GMA3_BLU_ENG_XB3_1,2048",
            "198,WT_GMA4_BLU_ENG_XB3_1,4080",
            "199,WT_GMA1_RED_ENG_XB3_2,512",
            "200,WT_GMA2_RED_ENG_XB3_2,1024",
            "201,WT_GMA3_RED_ENG_XB3_2,2048",
            "202,WT_GMA4_RED_ENG_XB3_2,4080",
            "203,WT_GMA1_GRN_ENG_XB3_2,512",
            "204,WT_GMA2_GRN_ENG_XB3_2,1024",
            "205,WT_GMA3_GRN_ENG_XB3_2,2048",
            "206,WT_GMA4_GRN_ENG_XB3_2,4080",
            "207,WT_GMA1_BLU_ENG_XB3_2,512",
            "208,WT_GMA2_BLU_ENG_XB3_2,1024",
            "209,WT_GMA3_BLU_ENG_XB3_2,2048",
            "210,WT_GMA4_BLU_ENG_XB3_2,4080",
            "211,WT_GMA1_RED_ENG_XB3_3,512",
            "212,WT_GMA2_RED_ENG_XB3_3,1024",
            "213,WT_GMA3_RED_ENG_XB3_3,2048",
            "214,WT_GMA4_RED_ENG_XB3_3,4080",
            "215,WT_GMA1_GRN_ENG_XB3_3,512",
            "216,WT_GMA2_GRN_ENG_XB3_3,1024",
            "217,WT_GMA3_GRN_ENG_XB3_3,2048",
            "218,WT_GMA4_GRN_ENG_XB3_3,4080",
            "219,WT_GMA1_BLU_ENG_XB3_3,512",
            "220,WT_GMA2_BLU_ENG_XB3_3,1024",
            "221,WT_GMA3_BLU_ENG_XB3_3,2048",
            "222,WT_GMA4_BLU_ENG_XB3_3,4080",
            "223,WT_GMA1_RED_ENG_XB3_4,512",
            "224,WT_GMA2_RED_ENG_XB3_4,1024",
            "225,WT_GMA3_RED_ENG_XB3_4,2048",
            "226,WT_GMA4_RED_ENG_XB3_4,4080",
            "227,WT_GMA1_GRN_ENG_XB3_4,512",
            "228,WT_GMA2_GRN_ENG_XB3_4,1024",
            "229,WT_GMA3_GRN_ENG_XB3_4,2048",
            "230,WT_GMA4_GRN_ENG_XB3_4,4080",
            "231,WT_GMA1_BLU_ENG_XB3_4,512",
            "232,WT_GMA2_BLU_ENG_XB3_4,1024",
            "233,WT_GMA3_BLU_ENG_XB3_4,2048",
            "234,WT_GMA4_BLU_ENG_XB3_4,4080",
            "235,WT_GMA1_RED_ENG_XB4_1,512",
            "236,WT_GMA2_RED_ENG_XB4_1,1024",
            "237,WT_GMA3_RED_ENG_XB4_1,2048",
            "238,WT_GMA4_RED_ENG_XB4_1,4080",
            "239,WT_GMA1_GRN_ENG_XB4_1,512",
            "240,WT_GMA2_GRN_ENG_XB4_1,1024",
            "241,WT_GMA3_GRN_ENG_XB4_1,2048",
            "242,WT_GMA4_GRN_ENG_XB4_1,4080",
            "243,WT_GMA1_BLU_ENG_XB4_1,512",
            "244,WT_GMA2_BLU_ENG_XB4_1,1024",
            "245,WT_GMA3_BLU_ENG_XB4_1,2048",
            "246,WT_GMA4_BLU_ENG_XB4_1,4080",
            "247,WT_GMA1_RED_ENG_XB4_2,512",
            "248,WT_GMA2_RED_ENG_XB4_2,1024",
            "249,WT_GMA3_RED_ENG_XB4_2,2048",
            "250,WT_GMA4_RED_ENG_XB4_2,4080",
            "251,WT_GMA1_GRN_ENG_XB4_2,512",
            "252,WT_GMA2_GRN_ENG_XB4_2,1024",
            "253,WT_GMA3_GRN_ENG_XB4_2,2048",
            "254,WT_GMA4_GRN_ENG_XB4_2,4080",
            "255,WT_GMA1_BLU_ENG_XB4_2,512",
            "256,WT_GMA2_BLU_ENG_XB4_2,1024",
            "257,WT_GMA3_BLU_ENG_XB4_2,2048",
            "258,WT_GMA4_BLU_ENG_XB4_2,4080",
            "259,WT_GMA1_RED_ENG_XB4_3,512",
            "260,WT_GMA2_RED_ENG_XB4_3,1024",
            "261,WT_GMA3_RED_ENG_XB4_3,2048",
            "262,WT_GMA4_RED_ENG_XB4_3,4080",
            "263,WT_GMA1_GRN_ENG_XB4_3,512",
            "264,WT_GMA2_GRN_ENG_XB4_3,1024",
            "265,WT_GMA3_GRN_ENG_XB4_3,2048",
            "266,WT_GMA4_GRN_ENG_XB4_3,4080",
            "267,WT_GMA1_BLU_ENG_XB4_3,512",
            "268,WT_GMA2_BLU_ENG_XB4_3,1024",
            "269,WT_GMA3_BLU_ENG_XB4_3,2048",
            "270,WT_GMA4_BLU_ENG_XB4_3,4080",
            "271,WT_GMA1_RED_ENG_XB4_4,512",
            "272,WT_GMA2_RED_ENG_XB4_4,1024",
            "273,WT_GMA3_RED_ENG_XB4_4,2048",
            "274,WT_GMA4_RED_ENG_XB4_4,4080",
            "275,WT_GMA1_GRN_ENG_XB4_4,512",
            "276,WT_GMA2_GRN_ENG_XB4_4,1024",
            "277,WT_GMA3_GRN_ENG_XB4_4,2048",
            "278,WT_GMA4_GRN_ENG_XB4_4,4080",
            "279,WT_GMA1_BLU_ENG_XB4_4,512",
            "280,WT_GMA2_BLU_ENG_XB4_4,1024",
            "281,WT_GMA3_BLU_ENG_XB4_4,2048",
            "282,WT_GMA4_BLU_ENG_XB4_4,4080",
            "283,IDLE,0",
            "284,IDLE,0",
            "285,IDLE,0",
            "286,IDLE,0",
            "287,IDLE,0",
            "288,IDLE,0",
            "289,IDLE,0",
            "290,IDLE,0",
            "291,IDLE,0",
            "292,IDLE,0",
            "293,IDLE,0",
            "294,IDLE,0",
            "295,IDLE,0",
            "296,IDLE,0",
            "297,IDLE,0",
            "298,IDLE,0",
            "299,IDLE,0",
            "300,GMA1_USR,512",
            "301,GMA2_USR,1024",
            "302,GMA3_USR,2048",
            "303,GMA4_USR,4080",
            "304,CT_RED_USR,1024",
            "305,CT_GRN_USR,1024",
            "306,CT_BLU_USR,1024",
            "307,CG_MAT_A1,16384",
            "308,CG_MAT_A2,0",
            "309,CG_MAT_A3,0",
            "310,CG_MAT_A4,0",
            "311,CG_MAT_A5,16384",
            "312,CG_MAT_A6,0",
            "313,CG_MAT_A7,0",
            "314,CG_MAT_A8,0",
            "315,CG_MAT_A9,16384",
            "316,CG_RED_X,0",
            "317,CG_RED_X,0",
            "318,CG_RED_X,0",
            "319,CG_RED_X,16384",
            "320,CG_RED_X,0",
            "321,CG_RED_Y,0",
            "322,CG_WHI_X,0",
            "323,CG_WHI_Y,0",
            "324,CG_CT_VAL,0",
            "325,GMA_VAL,0",
            "326,IDLE,0",
            "327,IDLE,0",
            "328,IDLE,0",
            "329,IDLE,0",
            "330,WT_PG_RED_ENG,4080",
            "331,WT_PG_GRN_ENG,4080",
            "332,WT_PG_BLU_ENG,4080",
            "333,CT_RED_ENG_XB1_1,1024",
            "334,CT_GRN_ENG_XB1_1,1024",
            "335,CT_BLU_ENG_XB1_1,1024",
            "336,CT_RED_ENG_XB1_2,1024",
            "337,CT_GRN_ENG_XB1_2,1024",
            "338,CT_BLU_ENG_XB1_2,1024",
            "339,CT_RED_ENG_XB1_3,1024",
            "340,CT_GRN_ENG_XB1_3,1024",
            "341,CT_BLU_ENG_XB1_3,1024",
            "342,CT_RED_ENG_XB1_4,1024",
            "343,CT_GRN_ENG_XB1_4,1024",
            "344,CT_BLU_ENG_XB1_4,1024",
            "345,CT_RED_ENG_XB2_1,1024",
            "346,CT_GRN_ENG_XB2_1,1024",
            "347,CT_BLU_ENG_XB2_1,1024",
            "348,CT_RED_ENG_XB2_2,1024",
            "349,CT_GRN_ENG_XB2_2,1024",
            "350,CT_BLU_ENG_XB2_2,1024",
            "351,CT_RED_ENG_XB2_3,1024",
            "352,CT_GRN_ENG_XB2_3,1024",
            "353,CT_BLU_ENG_XB2_3,1024",
            "354,CT_RED_ENG_XB2_4,1024",
            "355,CT_GRN_ENG_XB2_4,1024",
            "356,CT_BLU_ENG_XB2_4,1024",
            "357,CT_RED_ENG_XB3_1,1024",
            "358,CT_GRN_ENG_XB3_1,1024",
            "359,CT_BLU_ENG_XB3_1,1024",
            "360,CT_RED_ENG_XB3_2,1024",
            "361,CT_GRN_ENG_XB3_2,1024",
            "362,CT_BLU_ENG_XB3_2,1024",
            "363,CT_RED_ENG_XB3_3,1024",
            "364,CT_GRN_ENG_XB3_3,1024",
            "365,CT_BLU_ENG_XB3_3,1024",
            "366,CT_RED_ENG_XB3_4,1024",
            "367,CT_GRN_ENG_XB3_4,1024",
            "368,CT_BLU_ENG_XB3_4,1024",
            "369,CT_RED_ENG_XB4_1,1024",
            "370,CT_GRN_ENG_XB4_1,1024",
            "371,CT_BLU_ENG_XB4_1,1024",
            "372,CT_RED_ENG_XB4_2,1024",
            "373,CT_GRN_ENG_XB4_2,1024",
            "374,CT_BLU_ENG_XB4_2,1024",
            "375,CT_RED_ENG_XB4_3,1024",
            "376,CT_GRN_ENG_XB4_3,1024",
            "377,CT_BLU_ENG_XB4_3,1024",
            "378,CT_RED_ENG_XB4_4,1024",
            "379,CT_GRN_ENG_XB4_4,1024",
            "380,CT_BLU_ENG_XB4_4,1024",
            "381,WT_GMA1_RED_ENG_XB1_1,512",
            "382,WT_GMA2_RED_ENG_XB1_1,1024",
            "383,WT_GMA3_RED_ENG_XB1_1,2048",
            "384,WT_GMA4_RED_ENG_XB1_1,4080",
            "385,WT_GMA1_GRN_ENG_XB1_1,512",
            "386,WT_GMA2_GRN_ENG_XB1_1,1024",
            "387,WT_GMA3_GRN_ENG_XB1_1,2048",
            "388,WT_GMA4_GRN_ENG_XB1_1,4080",
            "389,WT_GMA1_BLU_ENG_XB1_1,512",
            "390,WT_GMA2_BLU_ENG_XB1_1,1024",
            "391,WT_GMA3_BLU_ENG_XB1_1,2048",
            "392,WT_GMA4_BLU_ENG_XB1_1,4080",
            "393,WT_GMA1_RED_ENG_XB1_2,512",
            "394,WT_GMA2_RED_ENG_XB1_2,1024",
            "395,WT_GMA3_RED_ENG_XB1_2,2048",
            "396,WT_GMA4_RED_ENG_XB1_2,4080",
            "397,WT_GMA1_GRN_ENG_XB1_2,512",
            "398,WT_GMA2_GRN_ENG_XB1_2,1024",
            "399,WT_GMA3_GRN_ENG_XB1_2,2048",
            "400,WT_GMA4_GRN_ENG_XB1_2,4080",
            "401,WT_GMA1_BLU_ENG_XB1_2,512",
            "402,WT_GMA2_BLU_ENG_XB1_2,1024",
            "403,WT_GMA3_BLU_ENG_XB1_2,2048",
            "404,WT_GMA4_BLU_ENG_XB1_2,4080",
            "405,WT_GMA1_RED_ENG_XB1_3,512",
            "406,WT_GMA2_RED_ENG_XB1_3,1024",
            "407,WT_GMA3_RED_ENG_XB1_3,2048",
            "408,WT_GMA4_RED_ENG_XB1_3,4080",
            "409,WT_GMA1_GRN_ENG_XB1_3,512",
            "410,WT_GMA2_GRN_ENG_XB1_3,1024",
            "411,WT_GMA3_GRN_ENG_XB1_3,2048",
            "412,WT_GMA4_GRN_ENG_XB1_3,4080",
            "413,WT_GMA1_BLU_ENG_XB1_3,512",
            "414,WT_GMA2_BLU_ENG_XB1_3,1024",
            "415,WT_GMA3_BLU_ENG_XB1_3,2048",
            "416,WT_GMA4_BLU_ENG_XB1_3,4080",
            "417,WT_GMA1_RED_ENG_XB1_4,512",
            "418,WT_GMA2_RED_ENG_XB1_4,1024",
            "419,WT_GMA3_RED_ENG_XB1_4,2048",
            "420,WT_GMA4_RED_ENG_XB1_4,4080",
            "421,WT_GMA1_GRN_ENG_XB1_4,512",
            "422,WT_GMA2_GRN_ENG_XB1_4,1024",
            "423,WT_GMA3_GRN_ENG_XB1_4,2048",
            "424,WT_GMA4_GRN_ENG_XB1_4,4080",
            "425,WT_GMA1_BLU_ENG_XB1_4,512",
            "426,WT_GMA2_BLU_ENG_XB1_4,1024",
            "427,WT_GMA3_BLU_ENG_XB1_4,2048",
            "428,WT_GMA4_BLU_ENG_XB1_4,4080",
            "429,WT_GMA1_RED_ENG_XB2_1,512",
            "430,WT_GMA2_RED_ENG_XB2_1,1024",
            "431,WT_GMA3_RED_ENG_XB2_1,2048",
            "432,WT_GMA4_RED_ENG_XB2_1,4080",
            "433,WT_GMA1_GRN_ENG_XB2_1,512",
            "434,WT_GMA2_GRN_ENG_XB2_1,1024",
            "435,WT_GMA3_GRN_ENG_XB2_1,2048",
            "436,WT_GMA4_GRN_ENG_XB2_1,4080",
            "437,WT_GMA1_BLU_ENG_XB2_1,512",
            "438,WT_GMA2_BLU_ENG_XB2_1,1024",
            "439,WT_GMA3_BLU_ENG_XB2_1,2048",
            "440,WT_GMA4_BLU_ENG_XB2_1,4080",
            "441,WT_GMA1_RED_ENG_XB2_2,512",
            "442,WT_GMA2_RED_ENG_XB2_2,1024",
            "443,WT_GMA3_RED_ENG_XB2_2,2048",
            "444,WT_GMA4_RED_ENG_XB2_2,4080",
            "445,WT_GMA1_GRN_ENG_XB2_2,512",
            "446,WT_GMA2_GRN_ENG_XB2_2,1024",
            "447,WT_GMA3_GRN_ENG_XB2_2,2048",
            "448,WT_GMA4_GRN_ENG_XB2_2,4080",
            "449,WT_GMA1_BLU_ENG_XB2_2,512",
            "450,WT_GMA2_BLU_ENG_XB2_2,1024",
            "451,WT_GMA3_BLU_ENG_XB2_2,2048",
            "452,WT_GMA4_BLU_ENG_XB2_2,4080",
            "453,WT_GMA1_RED_ENG_XB2_3,512",
            "454,WT_GMA2_RED_ENG_XB2_3,1024",
            "455,WT_GMA3_RED_ENG_XB2_3,2048",
            "456,WT_GMA4_RED_ENG_XB2_3,4080",
            "457,WT_GMA1_GRN_ENG_XB2_3,512",
            "458,WT_GMA2_GRN_ENG_XB2_3,1024",
            "459,WT_GMA3_GRN_ENG_XB2_3,2048",
            "460,WT_GMA4_GRN_ENG_XB2_3,4080",
            "461,WT_GMA1_BLU_ENG_XB2_3,512",
            "462,WT_GMA2_BLU_ENG_XB2_3,1024",
            "463,WT_GMA3_BLU_ENG_XB2_3,2048",
            "464,WT_GMA4_BLU_ENG_XB2_3,4080",
            "465,WT_GMA1_RED_ENG_XB2_4,512",
            "466,WT_GMA2_RED_ENG_XB2_4,1024",
            "467,WT_GMA3_RED_ENG_XB2_4,2048",
            "468,WT_GMA4_RED_ENG_XB2_4,4080",
            "469,WT_GMA1_GRN_ENG_XB2_4,512",
            "470,WT_GMA2_GRN_ENG_XB2_4,1024",
            "471,WT_GMA3_GRN_ENG_XB2_4,2048",
            "472,WT_GMA4_GRN_ENG_XB2_4,4080",
            "473,WT_GMA1_BLU_ENG_XB2_4,512",
            "474,WT_GMA2_BLU_ENG_XB2_4,1024",
            "475,WT_GMA3_BLU_ENG_XB2_4,2048",
            "476,WT_GMA4_BLU_ENG_XB2_4,4080",
            "477,WT_GMA1_RED_ENG_XB3_1,512",
            "478,WT_GMA2_RED_ENG_XB3_1,1024",
            "479,WT_GMA3_RED_ENG_XB3_1,2048",
            "480,WT_GMA4_RED_ENG_XB3_1,4080",
            "481,WT_GMA1_GRN_ENG_XB3_1,512",
            "482,WT_GMA2_GRN_ENG_XB3_1,1024",
            "483,WT_GMA3_GRN_ENG_XB3_1,2048",
            "484,WT_GMA4_GRN_ENG_XB3_1,4080",
            "485,WT_GMA1_BLU_ENG_XB3_1,512",
            "486,WT_GMA2_BLU_ENG_XB3_1,1024",
            "487,WT_GMA3_BLU_ENG_XB3_1,2048",
            "488,WT_GMA4_BLU_ENG_XB3_1,4080",
            "489,WT_GMA1_RED_ENG_XB3_2,512",
            "490,WT_GMA2_RED_ENG_XB3_2,1024",
            "491,WT_GMA3_RED_ENG_XB3_2,2048",
            "492,WT_GMA4_RED_ENG_XB3_2,4080",
            "493,WT_GMA1_GRN_ENG_XB3_2,512",
            "494,WT_GMA2_GRN_ENG_XB3_2,1024",
            "495,WT_GMA3_GRN_ENG_XB3_2,2048",
            "496,WT_GMA4_GRN_ENG_XB3_2,4080",
            "497,WT_GMA1_BLU_ENG_XB3_2,512",
            "498,WT_GMA2_BLU_ENG_XB3_2,1024",
            "499,WT_GMA3_BLU_ENG_XB3_2,2048",
            "500,WT_GMA4_BLU_ENG_XB3_2,4080",
            "501,WT_GMA1_RED_ENG_XB3_3,512",
            "502,WT_GMA2_RED_ENG_XB3_3,1024",
            "503,WT_GMA3_RED_ENG_XB3_3,2048",
            "504,WT_GMA4_RED_ENG_XB3_3,4080",
            "505,WT_GMA1_GRN_ENG_XB3_3,512",
            "506,WT_GMA2_GRN_ENG_XB3_3,1024",
            "507,WT_GMA3_GRN_ENG_XB3_3,2048",
            "508,WT_GMA4_GRN_ENG_XB3_3,4080",
            "509,WT_GMA1_BLU_ENG_XB3_3,512",
            "510,WT_GMA2_BLU_ENG_XB3_3,1024",
            "511,WT_GMA3_BLU_ENG_XB3_3,2048",
            "512,WT_GMA4_BLU_ENG_XB3_3,4080",
            "513,WT_GMA1_RED_ENG_XB3_4,512",
            "514,WT_GMA2_RED_ENG_XB3_4,1024",
            "515,WT_GMA3_RED_ENG_XB3_4,2048",
            "516,WT_GMA4_RED_ENG_XB3_4,4080",
            "517,WT_GMA1_GRN_ENG_XB3_4,512",
            "518,WT_GMA2_GRN_ENG_XB3_4,1024",
            "519,WT_GMA3_GRN_ENG_XB3_4,2048",
            "520,WT_GMA4_GRN_ENG_XB3_4,4080",
            "521,WT_GMA1_BLU_ENG_XB3_4,512",
            "522,WT_GMA2_BLU_ENG_XB3_4,1024",
            "523,WT_GMA3_BLU_ENG_XB3_4,2048",
            "524,WT_GMA4_BLU_ENG_XB3_4,4080",
            "525,WT_GMA1_RED_ENG_XB4_1,512",
            "526,WT_GMA2_RED_ENG_XB4_1,1024",
            "527,WT_GMA3_RED_ENG_XB4_1,2048",
            "528,WT_GMA4_RED_ENG_XB4_1,4080",
            "529,WT_GMA1_GRN_ENG_XB4_1,512",
            "530,WT_GMA2_GRN_ENG_XB4_1,1024",
            "531,WT_GMA3_GRN_ENG_XB4_1,2048",
            "532,WT_GMA4_GRN_ENG_XB4_1,4080",
            "533,WT_GMA1_BLU_ENG_XB4_1,512",
            "534,WT_GMA2_BLU_ENG_XB4_1,1024",
            "535,WT_GMA3_BLU_ENG_XB4_1,2048",
            "536,WT_GMA4_BLU_ENG_XB4_1,4080",
            "537,WT_GMA1_RED_ENG_XB4_2,512",
            "538,WT_GMA2_RED_ENG_XB4_2,1024",
            "539,WT_GMA3_RED_ENG_XB4_2,2048",
            "540,WT_GMA4_RED_ENG_XB4_2,4080",
            "541,WT_GMA1_GRN_ENG_XB4_2,512",
            "542,WT_GMA2_GRN_ENG_XB4_2,1024",
            "543,WT_GMA3_GRN_ENG_XB4_2,2048",
            "544,WT_GMA4_GRN_ENG_XB4_2,4080",
            "545,WT_GMA1_BLU_ENG_XB4_2,512",
            "546,WT_GMA2_BLU_ENG_XB4_2,1024",
            "547,WT_GMA3_BLU_ENG_XB4_2,2048",
            "548,WT_GMA4_BLU_ENG_XB4_2,4080",
            "549,WT_GMA1_RED_ENG_XB4_3,512",
            "550,WT_GMA2_RED_ENG_XB4_3,1024",
            "551,WT_GMA3_RED_ENG_XB4_3,2048",
            "552,WT_GMA4_RED_ENG_XB4_3,4080",
            "553,WT_GMA1_GRN_ENG_XB4_3,512",
            "554,WT_GMA2_GRN_ENG_XB4_3,1024",
            "555,WT_GMA3_GRN_ENG_XB4_3,2048",
            "556,WT_GMA4_GRN_ENG_XB4_3,4080",
            "557,WT_GMA1_BLU_ENG_XB4_3,512",
            "558,WT_GMA2_BLU_ENG_XB4_3,1024",
            "559,WT_GMA3_BLU_ENG_XB4_3,2048",
            "560,WT_GMA4_BLU_ENG_XB4_3,4080",
            "561,WT_GMA1_RED_ENG_XB4_4,512",
            "562,WT_GMA2_RED_ENG_XB4_4,1024",
            "563,WT_GMA3_RED_ENG_XB4_4,2048",
            "564,WT_GMA4_RED_ENG_XB4_4,4080",
            "565,WT_GMA1_GRN_ENG_XB4_4,512",
            "566,WT_GMA2_GRN_ENG_XB4_4,1024",
            "567,WT_GMA3_GRN_ENG_XB4_4,2048",
            "568,WT_GMA4_GRN_ENG_XB4_4,4080",
            "569,WT_GMA1_BLU_ENG_XB4_4,512",
            "570,WT_GMA2_BLU_ENG_XB4_4,1024",
            "571,WT_GMA3_BLU_ENG_XB4_4,2048",
            "572,WT_GMA4_BLU_ENG_XB4_4,4080"

        };

        public static string[] uiregadr_default_cb =
{
            "0,MARK,128",
            "1,USR_GMA_EN,1",
            "2,USR_BRI_EN,1",
            "3,USR_CG_EN,1",
            "4,ENG_WT_PG_EN,0",
            "5,ENG_GMA_EN,0",
            "6,ENG_BRI_EN,0",
            "7,STATUS,1",
            "8,IDLE,0",
            "9,IDLE,0",
            "10,GMA1_USR,512",
            "11,GMA2_USR,1024",
            "12,GMA3_USR,2048",
            "13,GMA4_USR,4080",
            "14,CT_RED_USR,1024",
            "15,CT_GRN_USR,1024",
            "16,CT_BLU_USR,1024",
            "17,CG_MAT_A1,16384",
            "18,CG_MAT_A2,0",
            "19,CG_MAT_A3,0",
            "20,CG_MAT_A4,0",
            "21,CG_MAT_A5,16384",
            "22,CG_MAT_A6,0",
            "23,CG_MAT_A7,0",
            "24,CG_MAT_A8,0",
            "25,CG_MAT_A9,16384",
            "26,CG_RED_X,0",
            "27,CG_RED_Y,0",
            "28,CG_GRN_X,0",
            "29,CG_GRN_Y,0",
            "30,CG_BLU_X,0",
            "31,CG_BLU_Y,0",
            "32,CG_WHI_X,0",
            "33,CG_WHI_Y,0",
            "34,CG_CT_VAL,0",
            "35,GMA_VAL,220",
            "36,IDLE,0",
            "37,IDLE,0",
            "38,IDLE,0",
            "39,IDLE,0",
            "40,WT_PG_RED_ENG,4080",
            "41,WT_PG_GRN_ENG,4080",
            "42,WT_PG_BLU_ENG,4080",
            "43,CT_RED_ENG_XB1_1,1024",
            "44,CT_GRN_ENG_XB1_1,1024",
            "45,CT_BLU_ENG_XB1_1,1024",
            "46,CT_RED_ENG_XB1_2,1024",
            "47,CT_GRN_ENG_XB1_2,1024",
            "48,CT_BLU_ENG_XB1_2,1024",
            "49,CT_RED_ENG_XB1_3,1024",
            "50,CT_GRN_ENG_XB1_3,1024",
            "51,CT_BLU_ENG_XB1_3,1024",
            "52,CT_RED_ENG_XB1_4,1024",
            "53,CT_GRN_ENG_XB1_4,1024",
            "54,CT_BLU_ENG_XB1_4,1024",
            "55,CT_RED_ENG_XB2_1,1024",
            "56,CT_GRN_ENG_XB2_1,1024",
            "57,CT_BLU_ENG_XB2_1,1024",
            "58,CT_RED_ENG_XB2_2,1024",
            "59,CT_GRN_ENG_XB2_2,1024",
            "60,CT_BLU_ENG_XB2_2,1024",
            "61,CT_RED_ENG_XB2_3,1024",
            "62,CT_GRN_ENG_XB2_3,1024",
            "63,CT_BLU_ENG_XB2_3,1024",
            "64,CT_RED_ENG_XB2_4,1024",
            "65,CT_GRN_ENG_XB2_4,1024",
            "66,CT_BLU_ENG_XB2_4,1024",
            "67,CT_RED_ENG_XB3_1,1024",
            "68,CT_GRN_ENG_XB3_1,1024",
            "69,CT_BLU_ENG_XB3_1,1024",
            "70,CT_RED_ENG_XB3_2,1024",
            "71,CT_GRN_ENG_XB3_2,1024",
            "72,CT_BLU_ENG_XB3_2,1024",
            "73,CT_RED_ENG_XB3_3,1024",
            "74,CT_GRN_ENG_XB3_3,1024",
            "75,CT_BLU_ENG_XB3_3,1024",
            "76,CT_RED_ENG_XB3_4,1024",
            "77,CT_GRN_ENG_XB3_4,1024",
            "78,CT_BLU_ENG_XB3_4,1024",
            "79,CT_RED_ENG_XB4_1,1024",
            "80,CT_GRN_ENG_XB4_1,1024",
            "81,CT_BLU_ENG_XB4_1,1024",
            "82,CT_RED_ENG_XB4_2,1024",
            "83,CT_GRN_ENG_XB4_2,1024",
            "84,CT_BLU_ENG_XB4_2,1024",
            "85,CT_RED_ENG_XB4_3,1024",
            "86,CT_GRN_ENG_XB4_3,1024",
            "87,CT_BLU_ENG_XB4_3,1024",
            "88,CT_RED_ENG_XB4_4,1024",
            "89,CT_GRN_ENG_XB4_4,1024",
            "90,CT_BLU_ENG_XB4_4,1024",
            "91,WT_GMA1_RED_ENG_XB1_1,512",
            "92,WT_GMA2_RED_ENG_XB1_1,1024",
            "93,WT_GMA3_RED_ENG_XB1_1,2048",
            "94,WT_GMA4_RED_ENG_XB1_1,4080",
            "95,WT_GMA1_GRN_ENG_XB1_1,512",
            "96,WT_GMA2_GRN_ENG_XB1_1,1024",
            "97,WT_GMA3_GRN_ENG_XB1_1,2048",
            "98,WT_GMA4_GRN_ENG_XB1_1,4080",
            "99,WT_GMA1_BLU_ENG_XB1_1,512",
            "100,WT_GMA2_BLU_ENG_XB1_1,1024",
            "101,WT_GMA3_BLU_ENG_XB1_1,2048",
            "102,WT_GMA4_BLU_ENG_XB1_1,4080",
            "103,WT_GMA1_RED_ENG_XB1_2,512",
            "104,WT_GMA2_RED_ENG_XB1_2,1024",
            "105,WT_GMA3_RED_ENG_XB1_2,2048",
            "106,WT_GMA4_RED_ENG_XB1_2,4080",
            "107,WT_GMA1_GRN_ENG_XB1_2,512",
            "108,WT_GMA2_GRN_ENG_XB1_2,1024",
            "109,WT_GMA3_GRN_ENG_XB1_2,2048",
            "110,WT_GMA4_GRN_ENG_XB1_2,4080",
            "111,WT_GMA1_BLU_ENG_XB1_2,512",
            "112,WT_GMA2_BLU_ENG_XB1_2,1024",
            "113,WT_GMA3_BLU_ENG_XB1_2,2048",
            "114,WT_GMA4_BLU_ENG_XB1_2,4080",
            "115,WT_GMA1_RED_ENG_XB1_3,512",
            "116,WT_GMA2_RED_ENG_XB1_3,1024",
            "117,WT_GMA3_RED_ENG_XB1_3,2048",
            "118,WT_GMA4_RED_ENG_XB1_3,4080",
            "119,WT_GMA1_GRN_ENG_XB1_3,512",
            "120,WT_GMA2_GRN_ENG_XB1_3,1024",
            "121,WT_GMA3_GRN_ENG_XB1_3,2048",
            "122,WT_GMA4_GRN_ENG_XB1_3,4080",
            "123,WT_GMA1_BLU_ENG_XB1_3,512",
            "124,WT_GMA2_BLU_ENG_XB1_3,1024",
            "125,WT_GMA3_BLU_ENG_XB1_3,2048",
            "126,WT_GMA4_BLU_ENG_XB1_3,4080",
            "127,WT_GMA1_RED_ENG_XB1_4,512",
            "128,WT_GMA2_RED_ENG_XB1_4,1024",
            "129,WT_GMA3_RED_ENG_XB1_4,2048",
            "130,WT_GMA4_RED_ENG_XB1_4,4080",
            "131,WT_GMA1_GRN_ENG_XB1_4,512",
            "132,WT_GMA2_GRN_ENG_XB1_4,1024",
            "133,WT_GMA3_GRN_ENG_XB1_4,2048",
            "134,WT_GMA4_GRN_ENG_XB1_4,4080",
            "135,WT_GMA1_BLU_ENG_XB1_4,512",
            "136,WT_GMA2_BLU_ENG_XB1_4,1024",
            "137,WT_GMA3_BLU_ENG_XB1_4,2048",
            "138,WT_GMA4_BLU_ENG_XB1_4,4080",
            "139,WT_GMA1_RED_ENG_XB2_1,512",
            "140,WT_GMA2_RED_ENG_XB2_1,1024",
            "141,WT_GMA3_RED_ENG_XB2_1,2048",
            "142,WT_GMA4_RED_ENG_XB2_1,4080",
            "143,WT_GMA1_GRN_ENG_XB2_1,512",
            "144,WT_GMA2_GRN_ENG_XB2_1,1024",
            "145,WT_GMA3_GRN_ENG_XB2_1,2048",
            "146,WT_GMA4_GRN_ENG_XB2_1,4080",
            "147,WT_GMA1_BLU_ENG_XB2_1,512",
            "148,WT_GMA2_BLU_ENG_XB2_1,1024",
            "149,WT_GMA3_BLU_ENG_XB2_1,2048",
            "150,WT_GMA4_BLU_ENG_XB2_1,4080",
            "151,WT_GMA1_RED_ENG_XB2_2,512",
            "152,WT_GMA2_RED_ENG_XB2_2,1024",
            "153,WT_GMA3_RED_ENG_XB2_2,2048",
            "154,WT_GMA4_RED_ENG_XB2_2,4080",
            "155,WT_GMA1_GRN_ENG_XB2_2,512",
            "156,WT_GMA2_GRN_ENG_XB2_2,1024",
            "157,WT_GMA3_GRN_ENG_XB2_2,2048",
            "158,WT_GMA4_GRN_ENG_XB2_2,4080",
            "159,WT_GMA1_BLU_ENG_XB2_2,512",
            "160,WT_GMA2_BLU_ENG_XB2_2,1024",
            "161,WT_GMA3_BLU_ENG_XB2_2,2048",
            "162,WT_GMA4_BLU_ENG_XB2_2,4080",
            "163,WT_GMA1_RED_ENG_XB2_3,512",
            "164,WT_GMA2_RED_ENG_XB2_3,1024",
            "165,WT_GMA3_RED_ENG_XB2_3,2048",
            "166,WT_GMA4_RED_ENG_XB2_3,4080",
            "167,WT_GMA1_GRN_ENG_XB2_3,512",
            "168,WT_GMA2_GRN_ENG_XB2_3,1024",
            "169,WT_GMA3_GRN_ENG_XB2_3,2048",
            "170,WT_GMA4_GRN_ENG_XB2_3,4080",
            "171,WT_GMA1_BLU_ENG_XB2_3,512",
            "172,WT_GMA2_BLU_ENG_XB2_3,1024",
            "173,WT_GMA3_BLU_ENG_XB2_3,2048",
            "174,WT_GMA4_BLU_ENG_XB2_3,4080",
            "175,WT_GMA1_RED_ENG_XB2_4,512",
            "176,WT_GMA2_RED_ENG_XB2_4,1024",
            "177,WT_GMA3_RED_ENG_XB2_4,2048",
            "178,WT_GMA4_RED_ENG_XB2_4,4080",
            "179,WT_GMA1_GRN_ENG_XB2_4,512",
            "180,WT_GMA2_GRN_ENG_XB2_4,1024",
            "181,WT_GMA3_GRN_ENG_XB2_4,2048",
            "182,WT_GMA4_GRN_ENG_XB2_4,4080",
            "183,WT_GMA1_BLU_ENG_XB2_4,512",
            "184,WT_GMA2_BLU_ENG_XB2_4,1024",
            "185,WT_GMA3_BLU_ENG_XB2_4,2048",
            "186,WT_GMA4_BLU_ENG_XB2_4,4080",
            "187,WT_GMA1_RED_ENG_XB3_1,512",
            "188,WT_GMA2_RED_ENG_XB3_1,1024",
            "189,WT_GMA3_RED_ENG_XB3_1,2048",
            "190,WT_GMA4_RED_ENG_XB3_1,4080",
            "191,WT_GMA1_GRN_ENG_XB3_1,512",
            "192,WT_GMA2_GRN_ENG_XB3_1,1024",
            "193,WT_GMA3_GRN_ENG_XB3_1,2048",
            "194,WT_GMA4_GRN_ENG_XB3_1,4080",
            "195,WT_GMA1_BLU_ENG_XB3_1,512",
            "196,WT_GMA2_BLU_ENG_XB3_1,1024",
            "197,WT_GMA3_BLU_ENG_XB3_1,2048",
            "198,WT_GMA4_BLU_ENG_XB3_1,4080",
            "199,WT_GMA1_RED_ENG_XB3_2,512",
            "200,WT_GMA2_RED_ENG_XB3_2,1024",
            "201,WT_GMA3_RED_ENG_XB3_2,2048",
            "202,WT_GMA4_RED_ENG_XB3_2,4080",
            "203,WT_GMA1_GRN_ENG_XB3_2,512",
            "204,WT_GMA2_GRN_ENG_XB3_2,1024",
            "205,WT_GMA3_GRN_ENG_XB3_2,2048",
            "206,WT_GMA4_GRN_ENG_XB3_2,4080",
            "207,WT_GMA1_BLU_ENG_XB3_2,512",
            "208,WT_GMA2_BLU_ENG_XB3_2,1024",
            "209,WT_GMA3_BLU_ENG_XB3_2,2048",
            "210,WT_GMA4_BLU_ENG_XB3_2,4080",
            "211,WT_GMA1_RED_ENG_XB3_3,512",
            "212,WT_GMA2_RED_ENG_XB3_3,1024",
            "213,WT_GMA3_RED_ENG_XB3_3,2048",
            "214,WT_GMA4_RED_ENG_XB3_3,4080",
            "215,WT_GMA1_GRN_ENG_XB3_3,512",
            "216,WT_GMA2_GRN_ENG_XB3_3,1024",
            "217,WT_GMA3_GRN_ENG_XB3_3,2048",
            "218,WT_GMA4_GRN_ENG_XB3_3,4080",
            "219,WT_GMA1_BLU_ENG_XB3_3,512",
            "220,WT_GMA2_BLU_ENG_XB3_3,1024",
            "221,WT_GMA3_BLU_ENG_XB3_3,2048",
            "222,WT_GMA4_BLU_ENG_XB3_3,4080",
            "223,WT_GMA1_RED_ENG_XB3_4,512",
            "224,WT_GMA2_RED_ENG_XB3_4,1024",
            "225,WT_GMA3_RED_ENG_XB3_4,2048",
            "226,WT_GMA4_RED_ENG_XB3_4,4080",
            "227,WT_GMA1_GRN_ENG_XB3_4,512",
            "228,WT_GMA2_GRN_ENG_XB3_4,1024",
            "229,WT_GMA3_GRN_ENG_XB3_4,2048",
            "230,WT_GMA4_GRN_ENG_XB3_4,4080",
            "231,WT_GMA1_BLU_ENG_XB3_4,512",
            "232,WT_GMA2_BLU_ENG_XB3_4,1024",
            "233,WT_GMA3_BLU_ENG_XB3_4,2048",
            "234,WT_GMA4_BLU_ENG_XB3_4,4080",
            "235,WT_GMA1_RED_ENG_XB4_1,512",
            "236,WT_GMA2_RED_ENG_XB4_1,1024",
            "237,WT_GMA3_RED_ENG_XB4_1,2048",
            "238,WT_GMA4_RED_ENG_XB4_1,4080",
            "239,WT_GMA1_GRN_ENG_XB4_1,512",
            "240,WT_GMA2_GRN_ENG_XB4_1,1024",
            "241,WT_GMA3_GRN_ENG_XB4_1,2048",
            "242,WT_GMA4_GRN_ENG_XB4_1,4080",
            "243,WT_GMA1_BLU_ENG_XB4_1,512",
            "244,WT_GMA2_BLU_ENG_XB4_1,1024",
            "245,WT_GMA3_BLU_ENG_XB4_1,2048",
            "246,WT_GMA4_BLU_ENG_XB4_1,4080",
            "247,WT_GMA1_RED_ENG_XB4_2,512",
            "248,WT_GMA2_RED_ENG_XB4_2,1024",
            "249,WT_GMA3_RED_ENG_XB4_2,2048",
            "250,WT_GMA4_RED_ENG_XB4_2,4080",
            "251,WT_GMA1_GRN_ENG_XB4_2,512",
            "252,WT_GMA2_GRN_ENG_XB4_2,1024",
            "253,WT_GMA3_GRN_ENG_XB4_2,2048",
            "254,WT_GMA4_GRN_ENG_XB4_2,4080",
            "255,WT_GMA1_BLU_ENG_XB4_2,512",
            "256,WT_GMA2_BLU_ENG_XB4_2,1024",
            "257,WT_GMA3_BLU_ENG_XB4_2,2048",
            "258,WT_GMA4_BLU_ENG_XB4_2,4080",
            "259,WT_GMA1_RED_ENG_XB4_3,512",
            "260,WT_GMA2_RED_ENG_XB4_3,1024",
            "261,WT_GMA3_RED_ENG_XB4_3,2048",
            "262,WT_GMA4_RED_ENG_XB4_3,4080",
            "263,WT_GMA1_GRN_ENG_XB4_3,512",
            "264,WT_GMA2_GRN_ENG_XB4_3,1024",
            "265,WT_GMA3_GRN_ENG_XB4_3,2048",
            "266,WT_GMA4_GRN_ENG_XB4_3,4080",
            "267,WT_GMA1_BLU_ENG_XB4_3,512",
            "268,WT_GMA2_BLU_ENG_XB4_3,1024",
            "269,WT_GMA3_BLU_ENG_XB4_3,2048",
            "270,WT_GMA4_BLU_ENG_XB4_3,4080",
            "271,WT_GMA1_RED_ENG_XB4_4,512",
            "272,WT_GMA2_RED_ENG_XB4_4,1024",
            "273,WT_GMA3_RED_ENG_XB4_4,2048",
            "274,WT_GMA4_RED_ENG_XB4_4,4080",
            "275,WT_GMA1_GRN_ENG_XB4_4,512",
            "276,WT_GMA2_GRN_ENG_XB4_4,1024",
            "277,WT_GMA3_GRN_ENG_XB4_4,2048",
            "278,WT_GMA4_GRN_ENG_XB4_4,4080",
            "279,WT_GMA1_BLU_ENG_XB4_4,512",
            "280,WT_GMA2_BLU_ENG_XB4_4,1024",
            "281,WT_GMA3_BLU_ENG_XB4_4,2048",
            "282,WT_GMA4_BLU_ENG_XB4_4,4080"
        };

        public static string[] uiregadr_default_tc =
{
            "0,MARK,128",
            "1,USR_GMA_EN,1",
            "2,USR_BRI_EN,1",
            "3,USR_CG_EN,1",
            "4,ENG_WT_PG_EN,0",
            "5,ENG_GMA_EN,0",
            "6,ENG_BRI_EN,0",
            "7,STATUS_1,1",
            "8,STATUS_2,0",
            "9,IDLE,0",
            "10,GMA1_USR,512",
            "11,GMA2_USR,1024",
            "12,GMA3_USR,2048",
            "13,GMA4_USR,4080",
            "14,CT_RED_USR,1024",
            "15,CT_GRN_USR,1024",
            "16,CT_BLU_USR,1024",
            "17,CG_MAT_A1,16384",
            "18,CG_MAT_A2,0",
            "19,CG_MAT_A3,0",
            "20,CG_MAT_A4,0",
            "21,CG_MAT_A5,16384",
            "22,CG_MAT_A6,0",
            "23,CG_MAT_A7,0",
            "24,CG_MAT_A8,0",
            "25,CG_MAT_A9,16384",
            "26,CG_RED_X,0",
            "27,CG_RED_Y,0",
            "28,CG_GRN_X,0",
            "29,CG_GRN_Y,0",
            "30,CG_BLU_X,0",
            "31,CG_BLU_Y,0",
            "32,CG_WHI_X,0",
            "33,CG_WHI_Y,0",
            "34,CG_CT_VAL,0",
            "35,GMA_VAL,220",
            "36,IDLE,0",
            "37,IDLE,0",
            "38,IDLE,0",
            "39,IDLE,0",
            "40,WT_PG_RED_ENG,4080",
            "41,WT_PG_GRN_ENG,4080",
            "42,WT_PG_BLU_ENG,4080",
            "43,CT_RED_ENG_XBL_1,1024",
            "44,CT_GRN_ENG_XBL_1,1024",
            "45,CT_BLU_ENG_XBL_1,1024",
            "46,CT_RED_ENG_XBL_2,1024",
            "47,CT_GRN_ENG_XBL_2,1024",
            "48,CT_BLU_ENG_XBL_2,1024",
            "49,CT_RED_ENG_XBL_3,1024",
            "50,CT_GRN_ENG_XBL_3,1024",
            "51,CT_BLU_ENG_XBL_3,1024",
            "52,CT_RED_ENG_XBR_1,1024",
            "53,CT_GRN_ENG_XBR_1,1024",
            "54,CT_BLU_ENG_XBR_1,1024",
            "55,CT_RED_ENG_XBR_2,1024",
            "56,CT_GRN_ENG_XBR_2,1024",
            "57,CT_BLU_ENG_XBR_2,1024",
            "58,CT_RED_ENG_XBR_3,1024",
            "59,CT_GRN_ENG_XBR_3,1024",
            "60,CT_BLU_ENG_XBR_3,1024",
            "61,WT_GMA1_RED_ENG_XBL_1,512",
            "62,WT_GMA2_RED_ENG_XBL_1,1024",
            "63,WT_GMA3_RED_ENG_XBL_1,2048",
            "64,WT_GMA4_RED_ENG_XBL_1,4080",
            "65,WT_GMA1_GRN_ENG_XBL_1,512",
            "66,WT_GMA2_GRN_ENG_XBL_1,1024",
            "67,WT_GMA3_GRN_ENG_XBL_1,2048",
            "68,WT_GMA4_GRN_ENG_XBL_1,4080",
            "69,WT_GMA1_BLU_ENG_XBL_1,512",
            "70,WT_GMA2_BLU_ENG_XBL_1,1024",
            "71,WT_GMA3_BLU_ENG_XBL_1,2048",
            "72,WT_GMA4_BLU_ENG_XBL_1,4080",
            "73,WT_GMA1_RED_ENG_XBL_2,512",
            "74,WT_GMA2_RED_ENG_XBL_2,1024",
            "75,WT_GMA3_RED_ENG_XBL_2,2048",
            "76,WT_GMA4_RED_ENG_XBL_2,4080",
            "77,WT_GMA1_GRN_ENG_XBL_2,512",
            "78,WT_GMA2_GRN_ENG_XBL_2,1024",
            "79,WT_GMA3_GRN_ENG_XBL_2,2048",
            "80,WT_GMA4_GRN_ENG_XBL_2,4080",
            "81,WT_GMA1_BLU_ENG_XBL_2,512",
            "82,WT_GMA2_BLU_ENG_XBL_2,1024",
            "83,WT_GMA3_BLU_ENG_XBL_2,2048",
            "84,WT_GMA4_BLU_ENG_XBL_2,4080",
            "85,WT_GMA1_RED_ENG_XBL_3,512",
            "86,WT_GMA2_RED_ENG_XBL_3,1024",
            "87,WT_GMA3_RED_ENG_XBL_3,2048",
            "88,WT_GMA4_RED_ENG_XBL_3,4080",
            "89,WT_GMA1_GRN_ENG_XBL_3,512",
            "90,WT_GMA2_GRN_ENG_XBL_3,1024",
            "91,WT_GMA3_GRN_ENG_XBL_3,2048",
            "92,WT_GMA4_GRN_ENG_XBL_3,4080",
            "93,WT_GMA1_BLU_ENG_XBL_3,512",
            "94,WT_GMA2_BLU_ENG_XBL_3,1024",
            "95,WT_GMA3_BLU_ENG_XBL_3,2048",
            "96,WT_GMA4_BLU_ENG_XBL_3,4080",
            "97,WT_GMA1_RED_ENG_XBR_1,512",
            "98,WT_GMA2_RED_ENG_XBR_1,1024",
            "99,WT_GMA3_RED_ENG_XBR_1,2048",
            "100,WT_GMA4_RED_ENG_XBR_1,4080",
            "101,WT_GMA1_GRN_ENG_XBR_1,512",
            "102,WT_GMA2_GRN_ENG_XBR_1,1024",
            "103,WT_GMA3_GRN_ENG_XBR_1,2048",
            "104,WT_GMA4_GRN_ENG_XBR_1,4080",
            "105,WT_GMA1_BLU_ENG_XBR_1,512",
            "106,WT_GMA2_BLU_ENG_XBR_1,1024",
            "107,WT_GMA3_BLU_ENG_XBR_1,2048",
            "108,WT_GMA4_BLU_ENG_XBR_1,4080",
            "109,WT_GMA1_RED_ENG_XBR_2,512",
            "110,WT_GMA2_RED_ENG_XBR_2,1024",
            "111,WT_GMA3_RED_ENG_XBR_2,2048",
            "112,WT_GMA4_RED_ENG_XBR_2,4080",
            "113,WT_GMA1_GRN_ENG_XBR_2,512",
            "114,WT_GMA2_GRN_ENG_XBR_2,1024",
            "115,WT_GMA3_GRN_ENG_XBR_2,2048",
            "116,WT_GMA4_GRN_ENG_XBR_2,4080",
            "117,WT_GMA1_BLU_ENG_XBR_2,512",
            "118,WT_GMA2_BLU_ENG_XBR_2,1024",
            "119,WT_GMA3_BLU_ENG_XBR_2,2048",
            "120,WT_GMA4_BLU_ENG_XBR_2,4080",
            "121,WT_GMA1_RED_ENG_XBR_3,512",
            "122,WT_GMA2_RED_ENG_XBR_3,1024",
            "123,WT_GMA3_RED_ENG_XBR_3,2048",
            "124,WT_GMA4_RED_ENG_XBR_3,4080",
            "125,WT_GMA1_GRN_ENG_XBR_3,512",
            "126,WT_GMA2_GRN_ENG_XBR_3,1024",
            "127,WT_GMA3_GRN_ENG_XBR_3,2048",
            "128,WT_GMA4_GRN_ENG_XBR_3,4080",
            "129,WT_GMA1_BLU_ENG_XBR_3,512",
            "130,WT_GMA2_BLU_ENG_XBR_3,1024",
            "131,WT_GMA3_BLU_ENG_XBR_3,2048",
            "132,WT_GMA4_BLU_ENG_XBR_3,4080",
        };



        #endregion uiregadr_default


        #region V
        public static string verFPGA;
        public static string[] verFPGAm;
        public static string verMCU;
        public static string[] verMCUS;
        public static string verMCUB;
        public static string verMCUA;
        public static string verEDID;
        #endregion V


        #region W
        public static string WndProcStr = null;
        #endregion W


        #region FPGA register    
        public static byte FPGA_CODE_VER = 0;

        #region c12a/ui
        public static byte FPGA_SI_SEL = 1;
        public static byte FPGA_ID_NUM = 2;
        public static byte FPGA_IT_XSR = 3;
        public static byte FPGA_IT_YSR = 4;
        public static byte FPGA_IT_HSW = 5;
        public static byte FPGA_IT_HBP = 6;
        public static byte FPGA_IT_HDP = 7;
        public static byte FPGA_IT_HFP = 8;
        public static byte FPGA_IT_VSW = 9;
        public static byte FPGA_IT_VBP = 10;
        public static byte FPGA_IT_VDP = 11;
        public static byte FPGA_IT_VFP = 12;
        public static byte FPGA_IT_XBS = 13;
        public static byte FPGA_IT_XOF = 14;
        public static byte FPGA_IT_XST = 15;
        public static byte FPGA_IT_YBS = 16;
        public static byte FPGA_IT_YOF = 17;
        public static byte FPGA_IT_YST = 18;
        public static byte FPGA_IT_XDT = 19;
        public static byte FPGA_IT_YDT = 20;
        public static byte FPGA_PT_SEL = 21;
        public static byte FPGA_CU_XCU = 22;
        public static byte FPGA_CU_YCU = 23;
        public static byte FPGA_CU_SEL = 24;
        public static byte FPGA_XB_HPD = 25;
        public static byte FPGA_FN_FQ = 26;
        public static byte FPGA_FN1_DT = 27;
        public static byte FPGA_FN2_DT = 28;
        public static byte FPGA_FN3_DT = 29;
        public static byte FPGA_FN4_DT = 30;
        public static byte FPGA_BK_SEL = 31;
        //
        public static byte FPGA_OT_XSR = 50;
        public static byte FPGA_OT_YSR = 51;
        public static byte FPGA_OT_HSW = 52;
        public static byte FPGA_OT_HBP = 53;
        public static byte FPGA_OT_HDP = 54;
        public static byte FPGA_OT_HFP = 55;
        public static byte FPGA_OT_VSW = 56;
        public static byte FPGA_OT_VBP = 57;
        public static byte FPGA_OT_VDP = 58;
        public static byte FPGA_OT_VFP = 59;
        public static byte FPGA_BOX_GAP = 60;
        public static byte FPGA_AG_MOD = 61;
        public static byte FPGA_BOX_SEL = 62;
        public static byte FPGA_XB_GAP = 63;
        //C12A新增
        public static byte FPGA_GOP_Ts = 70;
        public static byte FPGA_GOP_T1 = 71;
        public static byte FPGA_Gop_T1F = 72;
        public static byte FPGA_Gop_T2F = 73;
        public static byte FPGA_Gop_T2P = 74;
        public static byte FPGA_Gop_T2B = 75;
        public static byte FPGA_Gop_T3F = 76;
        public static byte FPGA_Gop_T3P = 77;
        public static byte FPGA_Gop_T3B = 78;

        public static byte FPGA_Gop_TX2 = 79;
        public static byte FPGA_Gop_TX1_1 = 80;
        public static byte FPGA_Gop_TX3_1 = 81;
        public static byte FPGA_Gop_TX1_2 = 82;
        public static byte FPGA_Gop_TX3_2 = 83;

        public static byte FPGA_OM_SEL = 100;
        public static byte FPGA_OM_RW = 101;
        public static byte FPGA_OM_ADDR = 102;
        public static byte FPGA_OM_Wdata = 103;
        public static byte FPGA_OM_Rdata = 104;
        public static byte FPGA_GRAY_R = 105;
        public static byte FPGA_GRAY_G = 106;
        public static byte FPGA_GRAY_B = 107;
        public static byte FPGA_X_START = 108;
        public static byte FPGA_X_END = 109;
        public static byte FPGA_Y_START = 110;
        public static byte FPGA_Y_END = 111;
        public static byte FPGA_Data_Out = 112;
        public static byte FPGA_Char_Set = 113;
        public static byte FPGA_Char_Wdata = 114;
        public static byte FPGA_Char_Xpos = 115;
        public static byte FPGA_Char_Ypos = 116;
        public static byte FPGA_BGRL_R = 117;
        public static byte FPGA_BGRL_G = 118;
        public static byte FPGA_BGRL_B = 119;
        public static byte FPGA_FR_LCK = 120;

        public static byte FPGA_C0_msg_0 = 200;
        public static byte FPGA_C0_msg_1 = 201;
        public static byte FPGA_C1_msg_0 = 202;
        public static byte FPGA_C1_msg_1 = 203;
        public static byte FPGA_C1_msg_2 = 204;
        public static byte FPGA_CB_Rdsts = 205;
        public static byte FPGA_U0_RdCnt = 206;
        public static byte FPGA_U1_RdCnt = 207;
        public static byte FPGA_U2_RdCnt = 208;
        public static byte FPGA_U3_RdCnt = 209;
        public static byte FPGA_Ucksm_NG = 210;
        public static byte FPGA_SYS_Stus1 = 211;
        public static byte FPGA_SYS_Stus2 = 212;
        public static byte FPGA_LS_i2cNG = 213;

        public static byte FPGA_UD_REG = 255;

        public static byte FPGA_PT_BANK = 0x33;
        #endregion c12a/ui

        #region 預設h5512A table                          
        public static byte FPGA_DIP_SW = 1;
        public static byte FPGA_IP_NUM = 2;
        public static byte FPGA_DMR = 3;
        public static byte FPGA_AL_CTRL = 4;
        public static byte FPGA_DUTY = 16;
        public static byte FPGA_T2F = 17;
        public static byte FPGA_T2P = 18;
        public static byte FPGA_T2B = 19;
        public static byte FPGA_T3F = 20;
        public static byte FPGA_T3P = 21;
        public static byte FPGA_T3B = 22;
        public static byte FPGA_T1 = 23;
        //public static byte FPGA_BK_SEL = 31;
        public static byte FPGA_OM_RD = 32;
        //public static byte FPGA_OM_ADDR = 33;
        //public static byte FPGA_OM_Wdata = 34;
        //public static byte FPGA_OM_Rdata = 35;
        //public static byte FPGA_GRAY_R = 48;
        //public static byte FPGA_GRAY_G = 49;
        //public static byte FPGA_GRAY_B = 50;
        public static byte FPGA_START_X = 66;
        public static byte FPGA_START_Y = 67;
        #endregion 預設h5512A table

        #region TV130
        public static byte FPGA_Flash_SW = 22;
        public static byte FPGA_Flash_SEL = 23;
        public static byte FPGA_DUTY_SEL = 70;
        public static byte FPGA_GOP_T3F1 = 75;
        public static byte FPGA_GOP_T3B1 = 77;
        public static byte FPGA_GOP_T3F2 = 78;
        public static byte FPGA_GOP_T3B2 = 79;
        public static byte FPGA_GOP_T4P = 80;
        public static byte FPGA_GOP_T4F = 81;

        public static byte FPGA_Flash_State = 200;
        public static byte FPGA_CBRd_State = 201;
        public static byte FPGA_UxRd_CNT = 202;
        public static byte FPGA_UxChk_NG = 203;
        public static byte FPGA_SYS_Stus0 = 204;
        #endregion TV130

        #endregion FPGA register


        #region CM603 RM93C30 9綁點
        // 先刪除再參考版本 --> 20230209_電壓檔增加RGB的dvg1 
        #endregion CM603 RM93C30 9綁點


        #region 20221005  CM603 RM93C30 13綁點 (C12過渡版)
        // 先刪除再參考版本 --> 20230209_電壓檔增加RGB的dvg1 
        #endregion 20221005  CM603 RM93C30 13綁點 


        #region 20221025  CM603 RM93C30 13綁點 (C12正式版)
        // 先刪除再參考版本 --> 20230209_電壓檔增加RGB的dvg1 
        #endregion 20221025  CM603 RM93C30 13綁點 


        #region 20221025  CM603 RM93C30 13綁點 (Primary)

        /*

            VREF_code轉電壓
            for (int svj = 0; svj <= 2; svj++)
            {
                int svbinI = mvars.cm603dfB[svj, (byte)0x1F * 2] * 256 + mvars.cm603dfB[svj, (byte)0x1F * 2 + 1];
                string svbinS = mp.DecToBin(svbinI, 16);
                mvars.cm603VrefCode[svj] = mp.BinToDec(svbinS.Substring(6, 10));
                mvars.cm603Vref[svj] = Convert.ToSingle((0.01953 * mvars.cm603VrefCode[svj]).ToString("##0.0#"));
            }



        繼續增加mCM603B4與修改mCM603B
        */
        public static void mCM603P(string SvL)
        {
            int svi;
            int svj;
            UInt32 ulCRC;
            string[,] svgmavolt = null;
            cm603Gamma = new int[13] { 0, 1, 8, 16, 24, 40, 64, 96, 128, 160, 192, 224, 255 };
            GMAterminals = cm603Gamma.Length - 1;

            //cm603Gamma.Length = 13 ; [0]~[12]=最高可調可量測的最高階數 = GMAterminals
            if (SvL == "0" || SvL == "")
            {
                svgmavolt = new string[,]
                {
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" }
                };
            }
            else if (SvL == "1")
            {
                svgmavolt = new string[,]
                {
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" }
                };
            }
            else if (SvL == "5")
            {
                svgmavolt = new string[,]
                 {
                    {"0.1364","0.1559","0.2943","0.4887","0.6077","0.7033","0.7989","0.9013","1.0037","1.1061","1.2017","1.3041","1.4065" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6077","0.7033","0.7989","0.9013","1.0037","1.1061","1.2017","1.3041","1.4065" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6077","0.7033","0.7989","0.9013","1.0037","1.1061","1.2017","1.3041","1.4065" }
                 };
            }


            #region 電壓轉階數
            if (svgmavolt != null)
            {
                byte svcm603M = 3;
                int svgmacode;

                /// bank0與bank1中間相差30個register --> 0x20-0x02=0x1E=30
                /// dualduty=0，0x02+30*mvars.dualduty=0x02
                /// dualduty=1，0x02+30*mvars.dualduty=0x20

                for (svi = 0; svi < 3; svi++)
                {
                    for (Form1.pvindex = 0x02 + 30 * mvars.dualduty; Form1.pvindex <= 0x0D + 30 * mvars.dualduty; Form1.pvindex++)
                    {
                        svj = svgmavolt.GetLength(1) - (Form1.pvindex % 30) + 1;    /// 13-2+1=12
                        svgmacode = Convert.ToInt16(float.Parse(svgmavolt[svi, svj]) * 1024 / mvars.cm603Vref[svi]);
                        mvars.pGMA[mvars.dualduty].Data[svi, svj * 2] = mp.DecToHex(svgmacode, 4).Substring(0, 2);      /// Form1.pvindex = 0x02 = [24]
                        mvars.pGMA[mvars.dualduty].Data[svi, svj * 2 + 1] = mp.DecToHex(svgmacode, 4).Substring(2, 2);  /// Form1.pvindex = 0x02 = [25]

                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    /// 0灰階最低電壓 (cm603M 0x02,0x03,0x04  0x20,0x21,0x22) 
                    Form1.pvindex = svi + 0x02 + 30 * mvars.dualduty;    /// dualduty0,R:BK0,0x02，G:BK0,0x03，B:BK0,0x04；；dualduty1,R:BK1,0x20，G:BK1,0x21，B:BK1,0x22
                    svj = 0;
                    svgmacode = Convert.ToInt16(float.Parse(svgmavolt[svi, svj]) * 1024 / mvars.cm603Vref[svcm603M]);
                    mvars.pGMA[mvars.dualduty].Data[svi, 0] = mp.DecToHex(svgmacode, 4).Substring(0, 2);
                    mvars.pGMA[mvars.dualduty].Data[svi, 1] = mp.DecToHex(svgmacode, 4).Substring(2, 2);

                    ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    mvars.cm603df[svcm603M, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                    mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                    mvars.cm603dfB[svcm603M, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2]);
                    mvars.cm603dfB[svcm603M, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1]);
                }
            }
            mvars.RegData = new string[4, 128];
            Array.Copy(mvars.cm603df, mvars.RegData, mvars.cm603df.Length);    //新cm603 bin 模式
            #endregion 電壓轉階數
        }


        #endregion 20221025  CM603 RM93C30 13綁點 


        #region 20240220  CM603 RM93C30 13綁點 (CarpStreamer)
        public static void mCM603C(string SvL)
        {
            int svi;
            int svj;
            UInt32 ulCRC;
            string[,] svgmavolt0 = null;
            cm603Gamma = new int[13] { 0, 1, 8, 16, 24, 40, 64, 96, 128, 160, 192, 224, 255 };
            GMAterminals = cm603Gamma.Length - 1;

            //cm603Gamma.Length = 13 ; [0]~[12]=最高可調可量測的最高階數 = GMAterminals
            if (SvL == "0" || SvL == "")
            {
                svgmavolt0 = new string[,]
                {
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","4.3264" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","4.3264" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","4.3264" }
                };
            }
            else if (SvL == "1")
            {
                svgmavolt0 = new string[,]
                {
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6838","0.8516","1.1664","1.586","1.9875","2.3894","2.7910","3.3071","3.8264" }
                };
            }
            else if (SvL == "5")
            {
                svgmavolt0 = new string[,]
                 {
                    {"0.1364","0.1559","0.2943","0.4887","0.6077","0.7033","0.7989","0.9013","1.0037","1.1061","1.2017","1.3041","1.4065" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6077","0.7033","0.7989","0.9013","1.0037","1.1061","1.2017","1.3041","1.4065" },
                    {"0.1364","0.1559","0.2943","0.4887","0.6077","0.7033","0.7989","0.9013","1.0037","1.1061","1.2017","1.3041","1.4065" }
                 };
            }


            #region 電壓轉階數
            //for (byte svdd = 0; svdd <= mvars.dualduty; svdd++)
            for (byte svdd = 0; svdd <= 1; svdd++)  //CarpStreamer不管bankselect所以2組bank都寫一樣的內容
            {
                for (svi = 0; svi < 3; svi++)
                {
                    uc_atg.svG0volt[svi] = Convert.ToSingle(svgmavolt0[svi, 0]);
                    //電壓依序遞減(GMA_11:volt_ ~ GMA_01:volt_)  0x03(CM603 pin:10 name:Vr2) ~0x0D(CM603 pin:22 name:Vr12
                    //電壓依序遞減(GMA_11:volt_ ~ GMA_01:volt_)  0x20(CM603 pin:10 name:Vr2) ~0x2B(CM603 pin:22 name:Vr12
                    for (Form1.pvindex = 0x03 + 30 * svdd; Form1.pvindex <= 0x0D + 30 * svdd; Form1.pvindex++)
                    {
                        svj = svgmavolt0.GetLength(1) - (Form1.pvindex - 30 * svdd) + 1;                                    // duty0: svj=13-0x03+1=11  duty1: svj=13-(33-30)+1=11
                        int svgmacode = Convert.ToInt16(float.Parse(svgmavolt0[svi, svj]) * 1024 / mvars.cm603Vref[svi]);   // [svi,0}=0.1364, [svi,12]=3.307, [svi,12]=3.82
                        mvars.pGMA[mvars.dualduty].Data[svi, svj * 2] = mp.DecToHex(svgmacode, 4).Substring(0, 2);
                        mvars.pGMA[mvars.dualduty].Data[svi, svj * 2 + 1] = mp.DecToHex(svgmacode, 4).Substring(2, 2);

                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    #region 最高電壓(GMA_12:volt_)
                    //VCOMmin(0x18)設為0=0x0000, 
                    //VCOMmin(0x31)設為0=0x0000, 
                    for (Form1.pvindex = 0x18 + 25 * svdd; Form1.pvindex <= 0x18 + 25 * svdd; Form1.pvindex++)
                    {
                        int svgmacode = 0;
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    //VCOMmax(0x19)設為1023=0x03FF(pGMA)=0x1BFF(cm603df)
                    //VCOMmax(0x32)設為1023=0x03FF(pGMA)=0x1BFF(cm603df)
                    for (Form1.pvindex = 0x19 + 25 * svdd; Form1.pvindex <= 0x19 + 25 * svdd; Form1.pvindex++)
                    {
                        int svgmacode = 1023;
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    //VCOM(0x12,pin24) 取代 Vr1(0x02,pin9)
                    //VCOM(0x30,pin24) 取代 Vr1(0x02,pin9)
                    for (Form1.pvindex = 0x12 + 30 * svdd; Form1.pvindex <= 0x12 + 30 * svdd; Form1.pvindex++)
                    {
                        svj = 12;                                                                                           // duty0: svj=13-2+1=12  duty1: svj=13-(32-30)+1=12
                        int svgmacode = Convert.ToInt16(float.Parse(svgmavolt0[svi, svj]) * 1024 / mvars.cm603Vref[svi]);
                        mvars.pGMA[mvars.dualduty].Data[svi, svj * 2] = mp.DecToHex(svgmacode, 4).Substring(0, 2);
                        mvars.pGMA[mvars.dualduty].Data[svi, svj * 2 + 1] = mp.DecToHex(svgmacode, 4).Substring(2, 2);

                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    #endregion 最高電壓(GMA_12:volt_) 由 VCOM(12h,pin24) 取代 Vr1(02h,pin9)，先把VCOMmin(18h)設為0=0x0000, VCOMmax(19h)設為1023=0x03FF(pGMA)=0x1BFF(cm603df)


                    if (svi == 0)
                    {
                        //TFT-VREF 在 R 的 02h(CM603 pin:9 name:Vr1)
                        Form1.pvindex = 0x02 + 30 * svdd;
                        int svgmacode = Convert.ToInt16(mvars.UUT.VREF * 1024 / mvars.cm603Vref[svi]);
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    else if (svi == 2)
                    {
                        //最低電壓(GMA_00:volt_) 在 B 的 02h(CM603 pin:9 name:Vr1)
                        Form1.pvindex = 0x02 + 30 * svdd;
                        int svgmacode = Convert.ToInt16(float.Parse(svgmavolt0[svi, 0]) * 1024 / mvars.cm603Vref[svi]);                     // [svi,0}=0.1304
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                }
            }


            mvars.RegData = new string[3, 128];
            Array.Copy(mvars.cm603df, mvars.RegData, mvars.cm603df.Length);    //新cm603 bin 模式
            #endregion 電壓轉階數
        }


        #endregion 20240220  CM603 RM93C30 13綁點 (CarpStreamer)


        public static bool fileCM603Gamma(bool svWrite, string fullname)
        {
            if (svWrite)
            {
                FileInfo copyFile = new FileInfo(fullname);
                if (copyFile.Exists) { copyFile.Delete(); }
                StreamWriter sTAwrite = File.CreateText(fullname);
                for (int i = 0; i <= 2; i++)
                {
                    sTAwrite.WriteLine("");
                    sTAwrite.WriteLine("        '-- " + msrColor[i + 2].Substring(0, 1) + "Vref_" + cm603Vref[i]);
                    for (int j = 0; j < pGMA[0].Data.Length / 3; j++)
                    {
                        if (j > 0 && j % 2 == 0) { sTAwrite.WriteLine(""); }
                        sTAwrite.WriteLine("          cm603gray(" + i.ToString() + "," + j.ToString() + ") = \"" + mp.DecToHex(cm603Gamma[j / 2], 4).Substring((j % 2) * 2, 2) +
                            "\": Data(" + i.ToString() + "," + j.ToString() + ") = \"" + pGMA[0].Data[i, j] + "\"");
                    }
                    sTAwrite.WriteLine("");
                }

                //sTAwrite.WriteLine("BANK1");
                //for (int i = 0; i <= 2; i++)
                //{
                //    sTAwrite.WriteLine("");
                //    sTAwrite.WriteLine("        '-- " + msrColor[i + 2].Substring(0, 1) + "Vref_" + cm603Vref[i]);
                //    for (int j = 0; j < pGMA1.Data.Length / 3; j++)
                //    {
                //        if (j > 0 && j % 2 == 0) { sTAwrite.WriteLine(""); }
                //        sTAwrite.WriteLine("          cm603gray(" + i.ToString() + "," + j.ToString() + ") = \"" + mp.DecToHex(cm603Gamma[j / 2], 4).Substring((j % 2) * 2, 2) +
                //            "\": Data(" + i.ToString() + "," + j.ToString() + ") = \"" + pGMA1.Data[i, j] + "\"");
                //    }
                //    sTAwrite.WriteLine("");
                //}

                sTAwrite.Flush(); //清除緩衝區
                sTAwrite.Close(); //關閉檔案
                return true;
            }
            else
            {
                if (File.Exists(fullname))
                {
                    //if (mvars.deviceID.Substring(0,2)=="02" && mvars.deviceNameSub=="B")
                    pGMA[0].Data = new string[3, 18];
                    pGMA[1].Data = new string[3, 18];
                    bool svbk0 = false;
                    bool svbk1 = false;
                    StreamReader sTAread = File.OpenText(fullname);
                    int svi = 0; int svj = 0;
                    while (true)
                    {
                        string data = sTAread.ReadLine();
                        if (data == null) { break; }
                        if (data != null && data != "")
                        {
                            if (svbk0 == false && svbk1 == false) { if (data.ToUpper().IndexOf("RVREF_") != -1) { svbk0 = true; svi = 0; } }
                            if (svbk0 == true && svbk1 == false) { if (data.ToUpper().IndexOf("BANK1") != -1) { svbk0 = false; svbk1 = true; svj = 0; pGMA[1].Data = new string[3, 18]; } }
                            if (svbk0)
                            {
                                int i = svi / 18;
                                int j = svi % 18;
                                if (data.ToUpper().IndexOf("CM603GRAY(" + i.ToString() + "," + j.ToString(), 0) != -1 && data.ToUpper().IndexOf("DATA(" + i.ToString() + "," + j.ToString(), 0) != -1)
                                {
                                    string[] Svs1 = data.Split('=', '\r', '"');
                                    pGMA[0].Data[i, j] = Svs1[5];
                                    svi++;
                                }
                                else if (data.ToUpper().IndexOf(msrColor[i + 2].Substring(0, 1) + "VREF_") != -1)
                                {
                                    cm603Vref[i] = Convert.ToSingle(data.Substring(("        '-- XVREF_").Length, data.Length - ("        '-- XVREF_").Length));
                                    //cm603VrefCode[i] = Convert.ToInt16(cm603Vref[i] / 0.01953);
                                }
                            }
                            if (svbk1)
                            {
                                int i = svj / 18;
                                int j = svj % 18;
                                if (data.ToUpper().IndexOf("CM603GRAY(" + i.ToString() + "," + j.ToString(), 0) != -1 && data.ToUpper().IndexOf("DATA(" + i.ToString() + "," + j.ToString(), 0) != -1)
                                {
                                    string[] Svs1 = data.Split('=', '\r', '"');
                                    pGMA[1].Data[i, j] = Svs1[5];
                                    svj++;
                                }
                            }
                        }
                    }
                    sTAread.Close();

                    if (svi != 54) { return false; }
                    if (svj == 0) { Array.Copy(pGMA[0].Data, pGMA[0].Data, pGMA[0].Data.Length); }

                    if (pGMA[0].Data[2, 17] == "" || pGMA[0].Data[2, 17] == null || pGMA[1].Data[2, 17] == "" || pGMA[1].Data[2, 17] == null) { return false; }
                    else
                    {
                        for (svi = 0; svi <= 2; svi++)
                        {
                            for (int svg = 0x04; svg <= 0x0C; svg++)
                            {
                                int sL = (pGMA[0].Data.Length / 3) - (svg - 0x03) * 2 + 1;
                                int sH = (pGMA[0].Data.Length / 3) - (svg - 0x03) * 2;
                                int svtr = mp.HexToDec(pGMA[0].Data[svi, sH]) * 256 + mp.HexToDec(pGMA[0].Data[svi, sL]);
                                UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                cm603df[svi, svg * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                                cm603df[svi, svg * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                                RegData[svi, svg * 2] = cm603df[svi, svg * 2];
                                RegData[svi, svg * 2 + 1] = cm603df[svi, svg * 2 + 1];
                                cm603dfB[svi, svg * 2] = (byte)mp.HexToDec(cm603df[svi, svg * 2]);
                                cm603dfB[svi, svg * 2 + 1] = (byte)mp.HexToDec(cm603df[svi, svg * 2 + 1]);
                            }
                        }
                        for (svi = 0; svi <= 2; svi++)
                        {
                            int svtr = Convert.ToInt16(cm603Vref[svi] / 0.01953);
                            UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                            cm603df[svi, 0x1F * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                            cm603df[svi, 0x1F * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                            RegData[svi, 0x1F * 2] = cm603df[svi, 0x1F * 2];
                            RegData[svi, 0x1F * 2 + 1] = cm603df[svi, 0x1F * 2 + 1];
                            cm603dfB[svi, 0x1F * 2] = (byte)mp.HexToDec(cm603df[svi, 0x1F * 2]);
                            cm603dfB[svi, 0x1F * 2 + 1] = (byte)mp.HexToDec(cm603df[svi, 0x1F * 2 + 1]);
                        }

                        for (svi = 0; svi <= 2; svi++)
                        {
                            for (int svg = 0x22; svg <= 0x2A; svg++)
                            {
                                int sL = (pGMA[1].Data.Length / 3) - (svg - 0x21) * 2 + 1;
                                int sH = (pGMA[1].Data.Length / 3) - (svg - 0x21) * 2;
                                int svtr = mp.HexToDec(pGMA[1].Data[svi, sH]) * 256 + mp.HexToDec(pGMA[1].Data[svi, sL]);
                                UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                cm603df[svi, svg * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                                cm603df[svi, svg * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                                RegData[svi, svg * 2] = cm603df[svi, svg * 2];
                                RegData[svi, svg * 2 + 1] = cm603df[svi, svg * 2 + 1];
                                cm603dfB[svi, svg * 2] = (byte)mp.HexToDec(cm603df[svi, svg * 2]);
                                cm603dfB[svi, svg * 2 + 1] = (byte)mp.HexToDec(cm603df[svi, svg * 2 + 1]);
                            }
                        }
                        return true;
                    }
                }
                else { return false; }
            }
        }
    }
}
