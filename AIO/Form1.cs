using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;               //應用程式管理
using System.Text.RegularExpressions;   //字元切割
using System.Runtime.InteropServices;   //FindWindow
using System.Reflection;                //Assembly
using System.Net;                       //IPHostEntry
using System.Runtime.Serialization.Formatters.Binary; //Object to binary array
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics.Eventing.Reader;


namespace AIO
{
    public partial class Form1 : Form
    {
        //設定本視窗獲得焦點
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetForegroundWindow", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetF();             //獲得本窗體的控制代碼
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetF(IntPtr hWnd);    //設定此窗體為活動窗體
                                                        //設定本視窗獲得焦點

        #region GetWindowsRec 取得顯示表單位置
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(int hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
        //Rectangle myRect = new Rectangle();
        #endregion



        public static NotifyIcon notifyIcon = new NotifyIcon();

        public static Form frm1 = null;
        public static Form frm2 = new Form();               //FormShow[2]
        public static Form i3init = null;                   //FormShow[6]

        public static Label lbl_form = new Label();

        public static CheckBox chkIsRealtime = null;
        public static CheckBox chkboardcast = null;
        public static CheckBox chkBC = null;
        public static CheckBox chkformsize = null;

        public static ComboBox cmbdeviceid = null;
        public static ComboBox cmbhPB = null;
        public static ComboBox cmbCM603 = null;
        public static ComboBox cmbFPGAsel = null;

        public static ToolStripLabel tslblStatus = null;
        public static ToolStripLabel tslbltarget = null;
        public static ToolStripLabel tslblHW = null;
        public static ToolStripLabel tslblTime = null;
        public static ToolStripLabel tslblpull = null;
        public static ToolStripLabel tslblrsin = null;
        public static ToolStripLabel tslblproj = null;
        public static ToolStripLabel tslbldeviceID = null;
        public static ToolStripStatusLabel tslblCOM = null;
        public static Panel pnlfrm1 = null;
        public static uc_FPGAreg ucFpgareg = null;          //FormShow[8]
        public static uc_atg ucatg = null;                  //FormShow[10]
        public static uc_cm603 uc603 = null;
        public static uc_Flash ucflash = null;
        public static uc_MCU ucmcu = null;
        public static uc_ca410 ucca410 = null;
        public static uc_C12Ademura ucdmr = null;
        public static uc_PictureAdjust ucpicadj = null;
        public static uc_box ucbox = null;
        public static uc_scrConfigF ucscrF = null;
        public static uc_coding uccoding = null;
        public static MenuStrip _menuStrip1 = null;

        public static string[] cmbDevice;                   //USB Device
        public static int pvindex;
        public static ToolStripMenuItem hProject = null; //
        public static ToolStripMenuItem hTool = null;
        public static ToolStripMenuItem hLan = null;
        public static ToolStripMenuItem hUser = null;
        public static ToolStripMenuItem hPictureadjust = null;
        public static ToolStripMenuItem hScreenconfig = null;
        //public static ToolStripMenuItem hOta = null;
        public static ToolStripMenuItem tsmnuota = null;
        public static ToolStripMenuItem hsend = null;
        public static ToolStripMenuItem hsave = null;
        public static ToolStripMenuItem hsingle = null;
        public static ToolStripMenuItem tsmnuuser = null;
        public static ToolStripMenuItem tsmnulogout = null;
        public static ToolStripSeparator tsspruser = null;


        public static ToolStripMenuItem hEDIDud = null;
        public static ToolStripMenuItem hExit = null;
        public static ToolStripMenuItem tsmnucheck485 = null;

        public static List<string> svuiregadr = new List<string>(new string[mvars.GAMMA_SIZE / 4]);     //指定 List 長度(FPFA A共1024*4=4096 bytes，FPGA B共1024*4=4096 bytes)
        
        public static ListBox lstsvuiregadr = null;
        public static ListBox lstget1 = null;
        public static ListBox lstm = null;
        public static ListBox lstmcuR60000 = null;
        public static ListBox lstmcuW60000 = null;
        public static ListBox lstmcuR66000 = null;  //v0030
        public static ListBox lstmcuW66000 = null;  //v0030

        //svuiregadr 只記錄數值內容  !!!!! 沒有帶reg addr與逗號
        public static Button btnfocus = null;

        public static List<LEDScreenInfo> screenInfoList = null;
        public static typNVSend[] nvsender = new typNVSend[0];      //Primary 目前先規劃只有1組485串全部的單屏
        public static typNVSend[] nvhdmi = new typNVSend[0];

        public static Socket nsckC;
        public static Socket nsckF;
        public static string sendData;
        public static System.Windows.Forms.Timer tmeota = null;
        public static DataGridView dgvformmsg = null;
        public static DataGridView dgvota = null;
        public static string serverPortnsckF = "7788";
        public static string serverPublicIP;
        public static string hostIP;

        public static DateTime dts = DateTime.Now;
        public static DateTime dte = DateTime.Now;

        #region 0709
        public int pvcmbpidindex = -1;
        public bool pvcmbpidclick = false;
        public string pvcmbpidkeycode = "";
        #endregion 0709

        #region FPGA register
        //public static bool[] PvFPGAkwForeCyan = new bool[300];
        //public static int PvFPGAkwForeCyanCount = 0;
        public static bool[] PvFPGAtxtBackCyan = new bool[300];
        public static bool PvFPGAtxtBackCyanEn = false;
        #endregion FPGA register

        #region sck區域常數
        private const int sckClosed = 0;
        private const int sckOpen = 1;
        private const int sckListening = 2;
        private const int sckConnectionPending = 3;
        private const int sckResolvingHost = 4;
        private const int sckHostResolved = 5;
        private const int sckConnecting = 6;
        private const int sckConnected = 7;
        private const int sckClosing = 8;
        private const int sckError = 9;
        #endregion sck區域常數

        public static int[] hwCards;                    //以 hwCards 取代 nvDevices []IP/COM []screen

        public static typhwCard[] hwCard;

        byte svbcBurningCnt;

        //Thread _th;
        ManualResetEvent _shutdownEvent;
        ManualResetEvent _pauseEvent;

        
        
        
        #region Form1啟動相關，避免重複啟動程序 @ public Form1()
        public Form1()
        {
            //避免重複啟動程式
            string currPrsName = Process.GetCurrentProcess().ProcessName;
            Process[] allProcessWithThisName = Process.GetProcessesByName(currPrsName);
            if (allProcessWithThisName.Length > 1)
            {
                //MessageBox.Show("Already Running");
                System.Environment.Exit(0);
                return;
            }
            else
            {
                InitializeComponent();
                initForm();
            }

        }
        public void initForm()
        {
            pnl_frm1.Location = new Point(0, 70);
            pnl_frm1.Size = new Size(stus_frm1.Width - 135, stus_frm1.Top - 71);
            chkboardcast = chk_boardcast;
            cmbdeviceid = cmb_deviceID;
            cmbhPB = cmb_hPB;
            cmbCM603 = cmb_CM603;
            cmbFPGAsel = cmb_FPGAsel;
            tslblStatus = tslbl_Status;
            tslbldeviceID = tslbl_deviceid;
            tslblCOM = tslbl_COM;
            tslblHW = tslbl_HW;
            tslblpull = tslbl_Pull;
            tslblrsin = tslbl_RSIn;
            pnlfrm1 = pnl_frm1;
            tslbltarget = tslbl_target;
            dgvformmsg = dgv_formmsg;
            dgvota = dgv_ota;
            tmeota = tme_ota;
            tsmnuuser = tsmnu_user;
            tsmnulogout = tsmnu_logout;
            tsspruser = tsspr_user;

            mvars.handleIDMe = FindWindow(null, mvars.strUInameMe);
            mvars.UImajor = "0.0";
            mvars.txtErrMsg = "2";
            mvars.tslblRSIn = tslbl_RSIn;   //mk5
            mvars.tslblPull = tslbl_Pull;   //mk5

            hExit = h_exit;
            hProject = h_project;
            hProject.ToolTipText = "";
            hTool = h_tool;
            hTool.ToolTipText = "";
            hLan = h_lan;
            hUser = h_user;
            hPictureadjust = h_pictureadjust;
            hScreenconfig = h_screenconfig;
            //hOta = h_ota;
            tsmnuota = tsmnu_ota;
            hsend = tsmnu_send;
            hsave = tsmnu_save;
            hsingle = tsmnu_single;
            hEDIDud = tsmnu_EDIDud;
            tsmnucheck485 = tsmnu_check485;

            chkformsize = chk_formsize;
            lstget1 = lst_get1;
            lstm = lst_m;
            lstsvuiregadr = lst_svuiregadr;
            btnfocus = btn_focus;
            lstmcuR60000 = lst_mcuR60000;
            lstmcuW60000 = lst_mcuW60000;
            lstmcuR66000 = lst_mcuR66000;
            lstmcuW66000 = lst_mcuW66000;

            _menuStrip1 = menuStrip1;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 16);
            Array.Resize(ref mvars.ReadDataBuffer, 65);
            Array.Resize(ref mvars.verMCUS, 5);     //5是長度[0],[1],[2],[3],[4]
            mvars.RegData = new string[3, 128];

            Array.Resize(ref mvars.UUT.ex20d10, 4); //使用[0][1][2][3]
            mvars.UUT.ex20d10[0] = 3f;
            mvars.UUT.ex20d10[1] = 2.5f;
            mvars.UUT.ex20d10[2] = 7.5f;
            mvars.UUT.ex20d10[3] = 1.6f;
            mvars.binhead = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8 ", "9", "A", "B", "C", "D", "E", "F" };

            mvars.sp1 = serialPort1;

            frm1 = this;
        }

        #endregion Form1啟動相關 (避免重複啟動程序 @ public Form1())
        //                                    [0]     [1]        [2]        [3]         [4]        [5]        [6]          [7]       [8]     [9]    [10]                                                      
        string[] dgvhUtxt = new string[] { "Index", "SvrP2", "txtfMCU", "txtfFPGA", "txtfEDID", "lstfMCU", "lstfFPGA", "lstfEDID", "fLen", "fCnt","Ready" };
        int[] dgvhUw = new int[] {            40,     40,        35,        35,         35,        60,        60,          60,       40,     40 ,   60};

        private void Form1_Load(object sender, EventArgs e)
        {
            //_th = new Thread(listen_data);
            //_th.Start();

            //tsmnu_demomode_Click(sender, null);
            chkBC = chk_boardcast;

            Screen[] screens = Screen.AllScreens;
            string Svs = screens[0].Bounds.ToString();
            if (Svs.Substring(Svs.Length - 1, 1) == "}") Svs = Svs.Substring(0, Svs.Length - 1);    //Svs={X=0,Y=0,Width=1366,Height=768
            int resW = 1366;
            int resH = 768;
            int resX = 0;
            int resY = 0;
            string[] Svss = Svs.Split(',');
            foreach (var word in Svss)
            {
                if (word.IndexOf("Width=", 0) != -1)
                {
                    string[] Svsss = word.Split('=');
                    resW = Convert.ToInt16(Svsss[1]);
                }
                else if (word.IndexOf("Height=", 0) != -1)
                {
                    string[] Svsss = word.Split('=');
                    resH = Convert.ToInt16(Svsss[1]);
                }
                else if (word.IndexOf("X=", 0) != -1)
                {
                    string[] Svsss = word.Split('=');
                    resX = Convert.ToInt16(Svsss[1]);
                }
                else if (word.IndexOf("Y=", 0) != -1)
                {
                    string[] Svsss = word.Split('=');
                    resY = Convert.ToInt16(Svsss[1]);
                }
            }


            pnl_frm1.Location = new Point(0, 0);
            pnl_frm1.Dock = DockStyle.Fill;
            pnl_frm1.SendToBack();

            mvars.UUT.Disk = new string[27, 3];
            mp.DiskStateDrive();
            //for (byte svi = 0; svi < mvars.UUT.Disk.Length / 3; svi++)
            //{
            //    if (mvars.UUT.Disk[svi, 0] != "" && mvars.UUT.Disk[svi, 0] != null)
            //    {
            //        //Form1.lstget1.Items.Add(mvars.UUT.Disk[svi, 1].Substring(0, 2) + " [" + mvars.UUT.Disk[svi, 2] + "]");
            //    }
            //}

            string str = System.Environment.GetEnvironmentVariable("SystemRoot"); //--> c:\windows
            string rootdir = str.Substring(0, 2); //--> c:
            mvars.strLogPath = rootdir + @"\Log\AIO_UI\";
            if (Directory.Exists(rootdir + @"\log\")) { }
            else Directory.CreateDirectory(rootdir + @"\log\");
            if (Directory.Exists(mvars.strLogPath)) { }
            else Directory.CreateDirectory(mvars.strLogPath);

            #region 版本去除小數點          
            string svsubs = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
            mvars.UImajor = "";
            for (int svi = 0; svi < svsubs.Length; svi++)
            {
                if (svsubs.Substring(svi, 1) != ".") mvars.UImajor += svsubs.Substring(svi, 1);
            }
            #endregion 版本去除小數點

            mvars.strStartUpPath = System.Windows.Forms.Application.StartupPath;
            mvars.strUInameMe = ("aio").ToUpper();
            this.Text = mvars.strUInameMe;

            InitialTray(mvars.strUInameMe);

            mvars.handleIDMe = FindWindow(null, mvars.strUInameMe);
            tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor;
            tslbl_Status.Text = "";

            if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\") == false)
            {
                /*
                MessageBox.Show(@"\Parameter\ not exist", "INXPID", MessageBoxButtons.OKCancel);
                if (Result == DialogResult.OK)
                {
                    MessageBox.Show("按下OK", "有回傳值顯示");
                }
                else if (Result == DialogResult.Cancel)
                {
                    MessageBox.Show("按下Cancel", "有回傳值顯示");
                }
                */
                MessageBox.Show("Folder " + mvars.strStartUpPath + @"\Parameter\ not exist", mvars.strUInameMe);
                this.Close();
                Environment.Exit(Environment.ExitCode);
                InitializeComponent();
            }

            //MultiLanguage.DefaultLanguage 前往 MultiLanguage.cs 修改 搜尋"當前預設語言"
            tsmnu_cn_Click(tsmnu_eng, null);
            if (MultiLanguage.DefaultLanguage == "en-US") { tsmnu_eng.Checked = true; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { tsmnu_cht.Checked = true; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { tsmnu_cn.Checked = true; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { tsmnu_jp.Checked = true; }
            MultiLanguage.prelan = MultiLanguage.DefaultLanguage;

            #region 雷sir畢
            tscmb_pid.Items.AddRange(mvars.deviceAll);

            mvars.ICver = 0;
            mvars.deviceID = "";
            mvars.deviceName = "";
            string svGammaSettingRead = "";
            //0037 add judge is or not at the autogamma mode ( if (chk_atgmode.Checked) && else....)
            if (chk_atgmode.Checked)
            {
                if (File.Exists(mvars.strStartUpPath + @"\Parameter\GammaSetting_AIO.txt")) { mp.fileGammaSettingAIO(false, ref svGammaSettingRead); }
                else
                {
                    string svs = "";
                    if (MultiLanguage.DefaultLanguage == "en-US")
                    {
                        svs = @"Please confirm file existed ? ""\Parameter\GammaSetting_AIO.txt""";
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    {
                        svs = @"請確認檔案是否存在 ""\Parameter\GammaSetting_AIO.txt""";
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    {
                        svs = @"请确认档案是否存在 ""\Parameter\GammaSetting_AIO.txt""";
                    }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                    {
                        svs = @"確認 ""\Parameter\GammaSetting_AIO.txt"" はあります";
                    }
                    MessageBox.Show(svs);
                    this.Close();
                    Environment.Exit(Environment.ExitCode);
                    InitializeComponent();
                }

                if (mvars.ICver >= 5)
                {
                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma") == false)
                    {
                        string svs = "";
                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            svs = @"""ICver,05"" read from the GammaSetting_AIO.txt must reference the ""DefaultGamma_cm603V.gma"" file";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            svs = @"""GammaSetting_AIO.txt""讀取的""ICver,05"" 必須參考""DefaultGamma_cm603V.gma""文件";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            svs = @"""GammaSetting_AIO.txt""读取的""ICver,05"" 必须参考""DefaultGamma_cm603V.gma""文档";
                        }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        {
                            svs = @"""GammaSetting_AIO.txt""読む""ICver,05"" 必ず参照してください""DefaultGamma_cm603V.gma""ファイル";
                        }
                        MessageBox.Show(svs);
                        this.Close();
                        Environment.Exit(Environment.ExitCode);
                        InitializeComponent();
                    }
                    else mvars.defaultGammafile = mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma";
                }
                else
                {
                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma") == false)
                    {
                        string svs = "";
                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            svs = @"""ICver," + mvars.ICver + @""" read from the ""GammaSetting_AIO.txt"" must reference the ""DefaultGamma_cm603.gma"" file";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            svs = @"""GammaSetting_AIO.txt""讀取的""ICver," + mvars.ICver + @""" 必須參考 ""DefaultGamma_cm603.gma"" 文件";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            svs = @"""GammaSetting_AIO.txt""读取的""ICver," + mvars.ICver + @""" 必须参考 ""DefaultGamma_cm603.gma"" 文档";
                        }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        {
                            svs = @"""GammaSetting_AIO.txt""読む""ICver,05"" 必ず参照してください""DefaultGamma_cm603V.gma""ファイル";
                        }
                        MessageBox.Show(svs);
                        this.Close();
                        Environment.Exit(Environment.ExitCode);
                        InitializeComponent();
                    }
                    else { mvars.defaultGammafile = mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma"; }
                }

                string[] svdevice = svGammaSettingRead.Split('.');
                /// h_pid 填寫正確的 deviceName，h_project.DropDownClosed()已經不作用
                if (svdevice.Length == 2)
                {
                    if (svdevice[1] == "0200" && (svdevice[0].ToUpper() == "C12A" || svdevice[0].ToUpper() == "C12B"))
                    {
                        mvars.deviceName = svdevice[0]; mvars.deviceID = svdevice[1]; mvars.deviceNo = "0";
                        mvars.GMAvalue = 3.6f;
                        string svstr = " 1-1_1, 1-1_2, 1-1_3, 1-1_4, 1-2_1, 1-2_2, 1-2_3, 1-2_4, 2-1_1, 2-1_2, 2-1_3, 2-1_4, 2-2_1, 2-2_2, 2-2_3, 2-2_4";
                        mvars.I2C_CMD = svstr.Split(',');
                        Form1.cmbhPB.Items.Clear();
                        Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                        Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x60; mvars.SercomCmdWrRd = 0x61;
                        if (svdevice[0].ToUpper() == "C12A") tscmb_pid.Text = tscmb_pid.Items[0].ToString();
                        else if (svdevice[0].ToUpper() == "C12B") tscmb_pid.Text = tscmb_pid.Items[3].ToString();
                        h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;
                    }
                    else if (svdevice[1] == "0300" && svdevice[0].ToUpper() == "H5512A")
                    {
                        mvars.deviceName = svdevice[0]; mvars.deviceID = svdevice[1]; mvars.deviceNo = "1";
                        mvars.GMAvalue = 3.6f;
                        tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;
                    }
                    else if (svdevice[1] == "0400" && svdevice[0].ToUpper() == "CUSTOM_UI")
                    {
                        mvars.deviceName = svdevice[0]; mvars.deviceID = svdevice[1]; mvars.deviceNo = "2";
                        mvars.GMAvalue = 3.6f;
                        string svstr = " 1-1_1, 1-1_2, 1-1_3, 1-1_4, 1-2_1, 1-2_2, 1-2_3, 1-2_4";
                        mvars.I2C_CMD = svstr.Split(',');
                        Form1.cmbhPB.Items.Clear();
                        Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                        Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                        tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;
                    }
                    else if (svdevice[1] == "0500" && svdevice[0].ToUpper() == "PRIMARY")
                    {
                        mvars.deviceName = svdevice[0]; mvars.deviceID = svdevice[1]; mvars.deviceNo = "4";
                        mvars.GMAvalue = 3.2f;
                        string svstr = " A, B, C, D, E, F, G, H";
                        mvars.I2C_CMD = svstr.Split(',');
                        Form1.cmbhPB.Items.Clear();
                        Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                        Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                        mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                        tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;

                        Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_p.Length);
                        mvars.uiregadr_default = mvars.uiregadr_default_p;
                        mvars.uiregadrdefault = "";
                        for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                        {
                            mvars.uiregadrdefault += "~" + mvars.uiregadr_default[j].Split(',')[0] + "," + mvars.uiregadr_default[j].Split(',')[2];
                            svuiregadr.RemoveAt(j);
                            svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                            svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8));   //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                            svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                        }
                        if (mvars.uiregadrdefault.Length > 1 && mvars.uiregadrdefault.Substring(0, 1) == "~") mvars.uiregadrdefault = mvars.uiregadrdefault.Substring(1, mvars.uiregadrdefault.Length - 1);

                        cmb_FPGAsel.Items.Clear();
                        string[] svf = new string[] { " ABCD", " EFGH", " ALL" };
                        if (MultiLanguage.DefaultLanguage == "en-US") { svf = new string[] { " 5678", " 1234", " ALL" }; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svf = new string[] { " 右畫面", " 左畫面", " 單屏" }; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svf = new string[] { " 右画面", " 左画面", " 单屏" }; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svf = new string[] { " 右画面", " 左画面", " 全画面" }; }
                        cmb_FPGAsel.Items.AddRange(svf);
                        cmb_FPGAsel.Text = cmb_FPGAsel.Items[mvars.FPGAsel].ToString();
                    }
                    else if (svdevice[1] == "0600" && svdevice[0].ToUpper() == "TV130")
                    {
                        mvars.deviceName = svdevice[0]; mvars.deviceID = svdevice[1]; mvars.deviceNo = "5";
                        mvars.GMAvalue = 3.2f;
                        string svstr = " A, B, C, D, E, F, G, H, I, J, K, L";
                        mvars.I2C_CMD = svstr.Split(',');
                        Form1.cmbhPB.Items.Clear();
                        Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                        Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                        mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                        tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;

                        Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_tc.Length);
                        mvars.uiregadr_default = mvars.uiregadr_default_tc;
                        mvars.uiregadrdefault = "";
                        for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                        {
                            mvars.uiregadrdefault += "~" + mvars.uiregadr_default[j].Split(',')[0] + "," + mvars.uiregadr_default[j].Split(',')[2];
                            svuiregadr.RemoveAt(j);
                            svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                            svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8));   //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                            svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                        }
                        if (mvars.uiregadrdefault.Length > 1 && mvars.uiregadrdefault.Substring(0, 1) == "~") mvars.uiregadrdefault = mvars.uiregadrdefault.Substring(1, mvars.uiregadrdefault.Length - 1);

                        cmb_FPGAsel.Items.Clear();
                        string[] svf = new string[] { " A", " B", " ALL" };
                        if (MultiLanguage.DefaultLanguage == "en-US") { svf = new string[] { " A", " B", " ALL" }; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svf = new string[] { " 左畫面", " 右畫面", " 單屏" }; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svf = new string[] { " 左画面", " 右画面", " 单屏" }; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svf = new string[] { " 左画面", " 右画面", " 全画面" }; }
                        cmb_FPGAsel.Items.AddRange(svf);
                        cmb_FPGAsel.Text = cmb_FPGAsel.Items[mvars.FPGAsel].ToString();
                    }
                    else if (svdevice[1] == "1000" && svdevice[0].ToUpper() == "CARPSTREAMER")
                    {
                        mvars.deviceName = svdevice[0]; mvars.deviceID = svdevice[1]; mvars.deviceNo = "6";
                        mvars.GMAvalue = 3.2f;
                        string svstr = " 1, 2, 3, 4";
                        mvars.I2C_CMD = svstr.Split(',');
                        Form1.cmbhPB.Items.Clear();
                        Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                        Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                        mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                        tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;

                        //mvars.FPGAsel = 0;

                        Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_cb.Length);
                        mvars.uiregadr_default = mvars.uiregadr_default_cb;
                        mvars.uiregadrdefault = "";
                        for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                        {
                            mvars.uiregadrdefault += "~" + mvars.uiregadr_default[j].Split(',')[0] + "," + mvars.uiregadr_default[j].Split(',')[2];
                            svuiregadr.RemoveAt(j);
                            svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                            svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8));   //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                            svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                        }
                        if (mvars.uiregadrdefault.Length > 1 && mvars.uiregadrdefault.Substring(0, 1) == "~") mvars.uiregadrdefault = mvars.uiregadrdefault.Substring(1, mvars.uiregadrdefault.Length - 1);

                        cmb_FPGAsel.Items.Clear();
                        cmb_FPGAsel.Visible = true;

                        cmbCM603.Items.Clear();
                        cmbCM603.Items.Add(" R");
                        cmbCM603.Items.Add(" G");
                        cmbCM603.Items.Add(" B");
                    }
                    else
                    {
                        string svs = "";
                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            svs = @"Unknow deviceName""" + svdevice[0] + @""" and deviceID""" + svdevice[1] + @"""";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            svs = @"未知的裝置名稱 """ + svdevice[0] + @""" 與裝置ID """ + svdevice[1] + @"""";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            svs = @"未知的装置名称 """ + svdevice[0] + @""" 与装置ID """ + svdevice[1] + @"""";
                        }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        {
                            svs = @"不明の装置名前 """ + svdevice[0] + @""" と装置ID """ + svdevice[1] + @"""";
                        }
                        MessageBox.Show(svs);
                        this.Close();
                        Environment.Exit(Environment.ExitCode);
                        InitializeComponent();
                    }
                    Form1.cmbdeviceid.Items.Clear();//m
                    Form1.cmbdeviceid.Items.Add(" " + mvars.deviceID);
                    if (mvars.deviceID.Substring(0, 2) == "05")
                    {
                        for (int i = 1; i <= 16; i++) Form1.cmbdeviceid.Items.Add(" 05" + mp.DecToHex(i, 2));
                    }
                    Form1.cmbdeviceid.SelectedIndex = 0;
                }
                tslbl_deviceid.Text = mvars.deviceID;
                cmbCM603.SelectedIndex = 0;

            }
            else
            {
                mvars.deviceName = "Primary"; mvars.deviceID = "0500"; mvars.deviceNo = "4";
                mvars.GMAvalue = 3.2f;
                string svstr = " A, B, C, D, E, F, G, H";
                mvars.I2C_CMD = svstr.Split(',');
                Form1.cmbhPB.Items.Clear();
                Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;

                Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_p.Length);
                mvars.uiregadr_default = mvars.uiregadr_default_p;
                mvars.uiregadrdefault = "";
                for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                {
                    mvars.uiregadrdefault += "~" + mvars.uiregadr_default[j].Split(',')[0] + "," + mvars.uiregadr_default[j].Split(',')[2];
                    svuiregadr.RemoveAt(j);
                    svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                    svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8));   //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                    svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                }
                if (mvars.uiregadrdefault.Length > 1 && mvars.uiregadrdefault.Substring(0, 1) == "~") mvars.uiregadrdefault = mvars.uiregadrdefault.Substring(1, mvars.uiregadrdefault.Length - 1);

                cmb_FPGAsel.Items.Clear();
                string[] svf = new string[] { " ABCD", " EFGH", " ALL" };
                if (MultiLanguage.DefaultLanguage == "en-US") { svf = new string[] { " 5678", " 1234", " ALL" }; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svf = new string[] { " 右畫面", " 左畫面", " 單屏" }; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { svf = new string[] { " 右画面", " 左画面", " 单屏" }; }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { svf = new string[] { " 右画面", " 左画面", " 全画面" }; }
                cmb_FPGAsel.Items.AddRange(svf);
                cmb_FPGAsel.Text = cmb_FPGAsel.Items[mvars.FPGAsel].ToString();
            }

            #endregion

            FPGAregister();
            btn_focus.SetBounds(50, 50, 1, 1);

            #region 網路IP

            cmb_HostIP.Items.Clear();
            //取得外網IP
            //WebClient webclient = new WebClient();
            //try { txt_HostIP.Text = webclient.DownloadString("http://api.ipify.org"); }
            //catch (Exception ex) { txt_HostIP.Text = ""; }
            //if (txt_HostIP.Text != "") cmb_HostIP.Items.Add("Public IP," + txt_HostIP.Text);
            //webclient.Dispose();
            //取得內網IP
            txt_HostIP.Text = GetLocalIP();
            if (txt_HostIP.Text != "") cmb_HostIP.Items.Add("Host IP," + txt_HostIP.Text);
            if (cmb_HostIP.Items.Count > 0)
            {
                label1.Text = cmb_HostIP.Items[0].ToString().Split(',')[0].ToString() + " :";
                txt_HostIP.Text = cmb_HostIP.Items[0].ToString().Split(',')[1].ToString();
            }
            cmb_HostIP.Items.Add("Phone IP,10.157.105.144");
            cmb_HostIP.Items.Add("192.168.43.12");
            cmb_HostIP.Items.Add(txt_HostIP.Text.Split('.')[0] + "." + txt_HostIP.Text.Split('.')[1] + "." + txt_HostIP.Text.Split('.')[2] + "." + (int.Parse(txt_HostIP.Text.Split('.')[3]) + 1));
            cmb_HostIP.Items.Add("192.168.123.117");
            cmb_HostIP.Items.Add("192.168.123.232");

            label1.Text = "Host IP :";
            txt_HostIP.Text = GetLocalIP();

            

            ////netspeed.speed();
            ////netspeed.ShowInterfaceSpeedAndQueue();
            //List<string> lst = new List<string>();
            //netspeed.pingResult(ref lst, txt_serverPublicIP.Text);
            //foreach (string svs in lst)
            //{
            //    lstget1.Items.Add(svs);
            //}

            tslbl_timer1.Visible = false;
            tslbl_timer1spr.Visible = false;
            tslbl_chc.Visible = false;
            tslbl_chcspr.Visible = false;
            tslbl_Sxd.Visible = false;
            tslbl_Sxdspr.Visible = false;

            Skeleton_formmsg(2, 888, 1, 1);
            Skeleton_ota(dgv_ota, dgvhUtxt.Length, 5, 1, 1);
            #endregion 網路IP

            #region mvars.lstget   mvars.lstMsgIn
            //mvars.lstget.SetBounds(12, 26, 494, 184);
            mvars.lstMsgIn.SetBounds(12, 245, 494, 79);
            Font fnt = new Font("Arial", 9, FontStyle.Bold);
            //mvars.lstget.Font = fnt;
            //this.Controls.AddRange(new Control[] { mvars.lstget, mvars.lstMsgIn });
            this.Controls.AddRange(new Control[] { mvars.lstMsgIn });
            //mvars.lstget.Visible = false;
            mvars.lstMsgIn.Visible = false;
            #endregion vars.lstget    mvars.lstMsgIn

            mvars.tmeRSIn = tme_RSIn;
            mvars.tmePull = tme_Pull;
            mvars.tmeWarn = tme_Warn;
            mvars.txtErrMsg = "2";
            h_project.Visible = chk_atgmode.Checked;
            h_pid.Visible = chk_atgmode.Checked;
            h_pictureadjust.Visible = !chk_atgmode.Checked;
            h_screenconfig.Visible = !chk_atgmode.Checked;
            tslbl_Pull.Visible = chk_atgmode.Checked;
            tslbl_msgpull.Visible = chk_atgmode.Checked;
            tslbl_RSIn.Visible = chk_atgmode.Checked;
            tslbl_msgrsin.Visible = chk_atgmode.Checked;
            tslbl_deviceid.Visible = chk_atgmode.Checked;
            tslbl_msgdeviceid.Visible = chk_atgmode.Checked;
            //tsmnu_ca410.Visible = chk_atgmode.Checked;
            //tssep_ca410.Visible = chk_atgmode.Checked;
            //this.Location = new Point(50, 50);
            this.Location = new Point(resX + resW / 5, resY + resH / 5);
            if (chk_formsize.Checked == false) chk_formsize.Text = this.Width + "," + this.Height;
            chk_formsize.Text += "," + this.Left + "," + this.Top;
            mp.funSaveLogs("Load success");
            //tsmnu_demomode_Click(sender, null);

        }

        //↓原裝listen_data()請參考 "...多執行緒(2)....doc" 文件
        public static void listen_binary(object svobjF)
        {
            int receive_data_size = 0;
            string fileName = mvars.strLogPath + "temp.bin";
            fileName = svobjF.ToString().Split('~')[1] + "temp.bin";
            if (File.Exists(fileName)) File.Delete(fileName);
            svobjF = svobjF.ToString().Split('~')[0];
            //fileName = mvars.strLogPath + "temp.bin";
            //if (File.Exists(fileName)) File.Delete(fileName);

            int receivedBytesLen;
            //int fileNameLen;
            //int count = 0;

            int first = 1;
            //receivedBytesLen = 0;
            //receive_data_size = 0;
            //fileNameLen = 0;
            //fileName = "";
            //fileName = @"D:\A_桃流\AIO\20230818_裡面架構大致相同分bin格式傳送與bin轉txt格式\網路資料測試\20230818_AIO_v0026續\AIO\bin\Debug\Parameter\Update\FPGA";//\test.bin";
            //fileName = @"D:\";
            byte[] clientData = new byte[1024 * 50000];
            //5000 = 5MB 50000 = 50MB //定義傳輸每段資料大小，值越大傳越快  
            //byte[] clientData = new byte[8192];  
            //string receivedPath = System.Windows.Forms.Application.StartupPath;
            BinaryWriter bWrite = null;
            bWrite = new BinaryWriter(File.Open(fileName, FileMode.Create));
            //MemoryStream ms = null;
            //string file_type = "";
            string display_data = "";
            string content;
            double cal_size;


            do
            {
                try
                {
                    receivedBytesLen = nsckF.Receive(clientData);           //設定Server端為Listen()
                                                                            //接收資料 (receivedBytesLen = 資料長度)  
                    if (first == 1) //第一筆資料為檔名  
                    {
                        //fileNameLen = BitConverter.ToInt32(clientData, 0);
                        ////轉換檔名的位元組為整數 (檔名長度)  
                        //fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
                        //// 1 int = 4 byte  轉換Byte為字串  
                        //file_type = fileName.Substring(fileName.Length - 3, 3);
                        ////取得檔名  
                        ////-----------  
                        //content = Encoding.ASCII.GetString(clientData, 4 +
                        //fileNameLen, receivedBytesLen - 4 - fileNameLen);
                        ////取得檔案內容 起始(檔名以後) 長度(扣除檔名長度)  
                        //display_data += content;
                        ////-----------  
                        //bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Create));
                        ////CREATE 覆蓋舊檔 APPEND 延續舊檔  
                        //ms = new MemoryStream();
                        //bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                        ////寫入資料 ，跳過起始檔名長度，接收長度減掉檔名長度  
                        //ms.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                        ////寫入資料 ，呈現於BITMAP用  

                        //bWrite = new BinaryWriter(File.Open(fileName, FileMode.Create));
                        bWrite.Write(clientData, 0, receivedBytesLen);
                    }
                    else //第二筆接收為資料  
                    {
                        //-----------  
                        content = Encoding.ASCII.GetString(clientData, 0, receivedBytesLen);
                        display_data += content;
                        //-----------  
                        bWrite.Write(clientData/*, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen*/, 0, receivedBytesLen);
                        //每筆接收起始 0 結束為當次Receive長度  
                        //ms.Write(clientData, 0, receivedBytesLen);
                        //寫入資料 ，呈現於BITMAP用  
                    }
                    receive_data_size += receivedBytesLen;
                    //計算資料每筆資料長度並累加，後面可以輸出總值看是否有完整接收  
                    cal_size = receive_data_size;
                    cal_size /= 1024;
                    cal_size = Math.Round(cal_size, 2);
                    //updateuiText(textBox1, cal_size.ToString());
                    first++;
                    Thread.Sleep(200); //每次接收不能太快，否則會資料遺失 
                }
                catch (Exception ex)
                {
                    if (bWrite != null) bWrite.Close();
                    string svs = "File Receiving fail." + ex.Message;
                    if (receive_data_size != int.Parse(svobjF.ToString()) && File.Exists(fileName)) File.Delete(fileName);
                }
            } while (nsckF.Available != 0);         //如果還沒接收完則繼續接收，設定Server端為Listen()
            
            if (bWrite != null) bWrite.Close();
            //↓斷點
            if (receive_data_size != int.Parse(svobjF.ToString()) && File.Exists(fileName))
            {
                if (IsFileLocked(fileName) == false) File.Delete(fileName);
            }

            nsckF.Shutdown(SocketShutdown.Both);

            //updateuiText(textBox2, fileName);
            //if (file_type == "jpg") //如果是圖則呈現在視窗上  
            //{
            //    //pictureBox1.Visible = true;  
            //    //richTextBox1.Visible = false;  
            //    updateui(pictureBox1);
            //    updateui2(richTextBox1);
            //    Bitmap Img = new Bitmap(ms);
            //    Bitmap imageOut = new Bitmap(Img, 1200, 600);
            //    pictureBox1.Image = imageOut;
            //}
            //else
            //{
            //    //pictureBox1.Visible = false;  
            //    //richTextBox1.Visible = true;  
            //    updateui(richTextBox1);
            //    updateui2(pictureBox1);
            //    //richTextBox1.Text = display_data;  
            //    updateuiText(richTextBox1, display_data);
            //}
            //Thread.Sleep(3000);
            //ms.Close();
            //count++;
            //first = 1;
            //bWrite.Close();


            //nsckF.Close();  //設定Server端為Listen()
            //nsckF.Dispose();
            //mp.doDelayms(10);
        }

        public static void listen_text(object svobjF)
        {
            int receive_data_size = 0;
            string fileName = mvars.strLogPath + "temp.txt";
            fileName = svobjF.ToString().Split('~')[1] + "temp.txt";
            if (File.Exists(fileName)) File.Delete(fileName);
            svobjF = svobjF.ToString().Split('~')[0];
            int receivedBytesLen;
            //int fileNameLen;
            //int count = 0;

            int first = 1;
            //receivedBytesLen = 0;
            //receive_data_size = 0;
            //fileNameLen = 0;
            //fileName = "";
            //fileName = @"D:\A_桃流\AIO\20230818_裡面架構大致相同分bin格式傳送與bin轉txt格式\網路資料測試\20230818_AIO_v0026續\AIO\bin\Debug\Parameter\Update\FPGA";//\test.bin";
            //fileName = svobjP.ToString() + "temp.txt";
            byte[] clientData = new byte[1024 * 50000];
            //5000 = 5MB 50000 = 50MB //定義傳輸每段資料大小，值越大傳越快  
            //byte[] clientData = new byte[8192];  
            //string receivedPath = System.Windows.Forms.Application.StartupPath;
            //BinaryWriter bWrite = null;
            //MemoryStream ms = null;
            //string file_type = "";
            string display_data = "";
            string content;
            double cal_size;


            if (nsckF.Connected)
            {
                do
                {
                    receivedBytesLen = nsckF.Receive(clientData);           //設定Server端為Listen()
                                                                            //接收資料 (receivedBytesLen = 資料長度)  
                    if (first == 1)
                    {
                        content = Encoding.ASCII.GetString(clientData, 0, receivedBytesLen);
                        display_data += content;
                    }
                    else
                    {
                        content = Encoding.ASCII.GetString(clientData, 0, receivedBytesLen);
                        display_data += content;

                    }
                    receive_data_size += receivedBytesLen;
                    //計算資料每筆資料長度並累加，後面可以輸出總值看是否有完整接收  
                    cal_size = receive_data_size;
                    cal_size /= 1024;
                    cal_size = Math.Round(cal_size, 2);
                    //updateuiText(textBox1, cal_size.ToString());
                    first++;
                    Thread.Sleep(10);               //每次接收不能太快，否則會資料遺失 
                } while (nsckF.Available != 0);     //如果還沒接收完則繼續接收，設定Server端為Listen()

                //↓斷點
                if (receive_data_size != int.Parse(svobjF.ToString()) && File.Exists(fileName))
                {
                    if (IsFileLocked(fileName) == false) File.Delete(fileName);
                }
                else if (receive_data_size == int.Parse(svobjF.ToString()) && File.Exists(fileName) == false) File.WriteAllText(fileName, display_data);

                nsckF.Shutdown(SocketShutdown.Both);
            }
        }


        public static void listen_binaryC(object svobjF)
        {
            int receive_data_size = 0;
            string fileName = mvars.strLogPath + "temp.bin";
            fileName = svobjF.ToString().Split('~')[1] + "temp.bin";
            if (File.Exists(fileName)) File.Delete(fileName);
            svobjF = svobjF.ToString().Split('~')[0];
            int receivedBytesLen;
            int first = 1;
            byte[] clientData = new byte[1024 * 50000];
            BinaryWriter bWrite = null;
            string display_data = "";
            string content;
            double cal_size;


            do
            {
                try
                {
                    receivedBytesLen = nsckF.Receive(clientData);           //設定Server端為Listen()
                                                                            //接收資料 (receivedBytesLen = 資料長度)  
                    if (first == 1) //第一筆資料為檔名  
                    {
                        bWrite = new BinaryWriter(File.Open(fileName, FileMode.Create));
                        bWrite.Write(clientData, 0, receivedBytesLen);
                    }
                    else //第二筆接收為資料  
                    {
                        //-----------  
                        content = Encoding.ASCII.GetString(clientData, 0, receivedBytesLen);
                        display_data += content;
                        //-----------  
                        bWrite.Write(clientData/*, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen*/, 0, receivedBytesLen);
                        //每筆接收起始 0 結束為當次Receive長度  
                        //ms.Write(clientData, 0, receivedBytesLen);
                        //寫入資料 ，呈現於BITMAP用  
                    }
                    receive_data_size += receivedBytesLen;
                    //計算資料每筆資料長度並累加，後面可以輸出總值看是否有完整接收  
                    cal_size = receive_data_size;
                    cal_size /= 1024;
                    cal_size = Math.Round(cal_size, 2);
                    //updateuiText(textBox1, cal_size.ToString());
                    first++;
                    Thread.Sleep(100); //每次接收不能太快，否則會資料遺失 
                }
                catch (Exception ex)
                {
                    if (bWrite != null) bWrite.Close();
                    string svs = "File Receiving fail." + ex.Message;
                    if (receive_data_size != int.Parse(svobjF.ToString()) && File.Exists(fileName)) File.Delete(fileName);
                }
            } while (nsckF.Available != 0);         //如果還沒接收完則繼續接收，設定Server端為Listen()

            if (bWrite != null) bWrite.Close();
            //↓斷點
            if (receive_data_size != int.Parse(svobjF.ToString()) && File.Exists(fileName))
            {
                if (IsFileLocked(fileName) == false) File.Delete(fileName);
            }
        }


        public static void listen_textC(object svobjF)
        {
            int receive_data_size = 0;
            string fileName = mvars.strLogPath + "temp.txt";
            fileName = svobjF.ToString().Split('~')[1] + "temp.txt";
            if (File.Exists(fileName)) File.Delete(fileName);
            svobjF = svobjF.ToString().Split('~')[0];
            int receivedBytesLen;
            //int fileNameLen;
            //int count = 0;

            int first = 1;
            //receivedBytesLen = 0;
            //receive_data_size = 0;
            //fileNameLen = 0;
            //fileName = "";
            //fileName = @"D:\A_桃流\AIO\20230818_裡面架構大致相同分bin格式傳送與bin轉txt格式\網路資料測試\20230818_AIO_v0026續\AIO\bin\Debug\Parameter\Update\FPGA";//\test.bin";
            //fileName = svobjP.ToString() + "temp.txt";
            byte[] clientData = new byte[1024 * 50000];
            //5000 = 5MB 50000 = 50MB //定義傳輸每段資料大小，值越大傳越快  
            //byte[] clientData = new byte[8192];  
            //string receivedPath = System.Windows.Forms.Application.StartupPath;
            //BinaryWriter bWrite = null;
            //MemoryStream ms = null;
            //string file_type = "";
            string display_data = "";
            string content;
            double cal_size;


            if (nsckC.Connected)
            {
                do
                {
                    receivedBytesLen = nsckC.Receive(clientData);           //設定Server端為Listen()
                                                                            //接收資料 (receivedBytesLen = 資料長度)  
                    if (first == 1)
                    {
                        content = Encoding.ASCII.GetString(clientData, 0, receivedBytesLen);
                        display_data += content;
                    }
                    else
                    {
                        content = Encoding.ASCII.GetString(clientData, 0, receivedBytesLen);
                        display_data += content;

                    }
                    receive_data_size += receivedBytesLen;
                    //計算資料每筆資料長度並累加，後面可以輸出總值看是否有完整接收  
                    cal_size = receive_data_size;
                    cal_size /= 1024;
                    cal_size = Math.Round(cal_size, 2);
                    //updateuiText(textBox1, cal_size.ToString());
                    first++;
                    Thread.Sleep(10);               //每次接收不能太快，否則會資料遺失 
                } while (nsckC.Available != 0);     //如果還沒接收完則繼續接收，設定Server端為Listen()

                //↓斷點
                if (receive_data_size != int.Parse(svobjF.ToString()) && File.Exists(fileName))
                {
                    if (IsFileLocked(fileName) == false) File.Delete(fileName);
                }
                else if (receive_data_size == int.Parse(svobjF.ToString()) && File.Exists(fileName) == false) File.WriteAllText(fileName, display_data);
            }
        }



        public static bool IsFileLocked(string file)
        {
            try
            {
                using (File.Open(file, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException exception)
            {
                var errorCode = Marshal.GetHRForException(exception) & 65535;
                return errorCode == 32 || errorCode == 33;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region OTA sck_Client


        /// <summary>
        /// 燒錄程序
        /// </summary>
        /// <param name="dir"></param>
        /// 
        private void toolStripStatusLabel15_MouseDown(object sender, MouseEventArgs e)
        {
            svbcBurningCnt = 0;
            mvars.actFunc = "bcBurning";
            tme_ota.Enabled = true;
            svatgcount++;
            if (svatgcount > 2)
            {
                label15_DoubleClick(null, null);
                svatgcount = 0;
            }
        }
        private void label15_Click(object sender, EventArgs e) { }
        private void label15_DoubleClick(object sender, EventArgs e)
        {
            dgvformmsg.Visible = false;
            tme_ota.Enabled = dgvformmsg.Visible;

            mvars.flgForceUpdate = true;
            MessageBox.Show(this, "選擇資料夾內需包含三個子資料夾" + "\r\n" + " .../MCU" + "\r\n" + ".../FPGA" + "\r\n" + ".../EDID", "提示");

            if (dgvformmsg.Visible == true)
            {
                Skeleton_ota(dgv_ota, dgvhUtxt.Length, 5, 1, 1);
                dgvota.Visible = true;
            }

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "請選擇升級檔案所在資料夾";
            dialog.SelectedPath = "c:";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "資料夾路徑不能為空", "提示");
                    return;
                }
                string sDir = dialog.SelectedPath;
                string value = "3";
                mp.InputBox("Update", "How many screens", ref value, this.Left + 100, this.Top + 100, 150, 120, 1, "");
                if (value == "0" || mp.IsNumeric(value) == false) value = "3";
                Form1.nvsendercls_p(ref Form1.nvsender, 1, 1);  //485共有多少單屏串接
                Form1.nvsender[0].regPoCards = ushort.Parse(value);
                lstget1.Items.Clear();
                cUpdate(sDir);
            }
            else
            {
                mvars.flgForceUpdate = false;
            }
        }
        public static void cUpdate(string svpath)
        {
            bool svdemode = mvars.demoMode;
            //if (mvars.demoMode) tsmnu_demomode_Click(null, null);
            //if (mvars.demoMode) return;
            string svdeviceID = mvars.deviceID;

            #region parameter initial (pidinit)
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }
            if (mvars.demoMode == false && Form1.tslblStatus.Text.Substring(0, "No Any".Length) == "No Any")
            {
                lstget1.Items.Add(Form1.tslblStatus.Text);
                return;
            }

            #endregion parameter initial
            if (chkformsize.Checked == false && dgvformmsg.Visible == false)
            {
                dgvota.Location = new Point(12, 30);
                dgvota.Width = 600;

                dgvformmsg.DataSource = null;
            }

            mp.showStatus1("", lstget1, "");
            mp.showStatus1("MODEL: " + mvars.projectName + "Update .... ", lstget1, "");

            hwCard = new typhwCard[Form1.nvsender[0].regPoCards];
            bool[] svundoA = new bool[Form1.nvsender[0].regPoCards];

            DateTime t1 = DateTime.Now;
            string svUpdate = "000";    //只要MCU/FPGA/EDID任一個有執行完Update就會將對應的"0"更新為"1"，最後只要判斷有"1"的存在就MCUreset
            string sverr = "0";
            Byte[] tmp;
            string sDir = mvars.strStartUpPath + @"\Parameter\Update\MCU";
            string svmsg = "";



            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            mvars.FPGAsel = 2;
            mvars.lblCmd = "PRIID_SERIESMODE";
            mp.mIDSERIESMODE();
            mvars.lblCmd = "PRIID_AUTOID";
            mp.mAUTOID("1");

            int svWaitSec = 6;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Add("  Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Add("  請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Add("  请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Add("  待って " + svWaitSec + "秒"); }
                lstget1.TopIndex = lstget1.Items.Count - 1;
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                svWaitSec--;
            } while (svWaitSec > 0);
            mvars.lblCmd = "PRIID_WRITEID";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mWRGETDEVID();


            for (int i = 1; i <= Form1.nvsender[0].regPoCards; i++)
            {
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(i, 2);
                mvars.lblCmd = "MCU_VERSION";
                mp.mhVersion();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    Form1.nvsender[0].regPoCards = (ushort)(i - 1);
                    break;
                }
                else
                {
                    lstget1.Items.Add("   " + mvars.deviceID);
                }
            }



            goto upMCU;
            #region ----------------------------------------------------- ↓從頭到尾全部確認過如果有在boot mode就先燒錄讓他回到app mode


            if (mvars.deviceID.Substring(0, 2) == "05" && File.Exists(mvars.strStartUpPath + @"\Parameter\v0059.hex") == true)
            {
                //統一廣播先燒一次app code
                #region 開啟hex file
                string sTextLine;
                Byte[] ucNvmUserPage = new byte[mvars.NVM_USER_PAGE];
                bool bBreak = false;
                bool bHexParseSuccess = false;

                System.IO.StreamReader file = new System.IO.StreamReader(mvars.strStartUpPath + @"\Parameter\v0059.hex");
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
                    if (sverr == "-1")
                        mp.showStatus1("  --> Error Hex File，Flash Address greater than Bin Size", lstget1, "");
                    else
                        mp.showStatus1("  --> Error Hex File", lstget1, "");
                    mp.showStatus1("  --> MCU Update... fail，Please contact the server administrator", lstget1, "");
                }
                else
                {
                    //string sFilePath = txt_nMCUFileNameFull.Text.Replace(".hex", "(hex2bin).bin");
                    //File.WriteAllBytes(sFilePath, mvars.gMcuBinFile);
                    //File.WriteAllBytes("C:\\Users\\" + Environment.UserName + "\\Documents\\NVM_User_Page.bin", ucNvmUserPage);
                    //lst_get1.Items.Add("Hex 2 Bin OK");
                    //lst_get1.Items.Add(sFilePath);

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

                        Array.Resize(ref tmp, 16);
                        mp.Copy(mvars.gMcuBinFile, mvars.MCU_APP_VERSION_ADDR, tmp, 0, 16);
                        mp.showStatus1("  --> " + "  AppCode..... " + Encoding.ASCII.GetString(tmp), lstget1, "");
                        mp.showStatus1("  --> " + "  Version..... " + split[0], lstget1, "");
                        mp.showStatus1("  --> " + "  Date.......... " + split[1], lstget1, "");
                        mp.showStatus1("  --> " + "  Time.......... " + split[2], lstget1, "");
                        mp.showStatus1("  --> " + "  Project....... " + split[3], lstget1, "");
                    }
                    catch (Exception ex)
                    {
                        sverr = "-2";
                        mp.showStatus1(ex.Message, lstget1, "");
                    }

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
                            catch (Exception ex)
                            {
                                mp.showStatus1(ex.Message, lstget1, "");
                            }
                        }
                        tmp = Array.FindAll(tmp, val => val != 0).ToArray();

                        string MCU_Ver = System.Text.Encoding.ASCII.GetString(tmp);
                        MCU_Ver = MCU_Ver.Replace("?", "");
                        string[] splitBoot = MCU_Ver.Split(new Char[] { '\t' });

                        Array.Resize(ref tmp, 16);
                        mp.Copy(mvars.gMcuBinFile, mvars.MCU_BOOT_VERSION_ADDR, tmp, 0, 16);
                        mp.showStatus1("  --> " + "  BootCode... " + Encoding.ASCII.GetString(tmp), lstget1, "");
                        mp.showStatus1("  --> " + "  Version..... " + splitBoot[0], lstget1, "");
                        mp.showStatus1("  --> " + "  Date.......... " + splitBoot[1], lstget1, "");
                        mp.showStatus1("  --> " + "  Time.......... " + splitBoot[2], lstget1, "");
                        mp.showStatus1("  --> " + "  Project....... " + splitBoot[3], lstget1, "");
                    }
                    catch (Exception ex)
                    {
                        sverr = "-3";
                        mp.showStatus1(ex.Message, lstget1, "");
                    }


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
                    //btn_nMCUBOOT.Enabled = true;
                }

                tmp = new byte[16];
                mp.Copy(mvars.gMcuBinFile, mvars.MCU_BOOT_VERSION_ADDR, tmp, 0, 16);
                string svs = System.Text.Encoding.ASCII.GetString(tmp);
                mvars.HexBootVer = Convert.ToUInt16(svs.Substring(svs.Length - 4, 4));
                mp.Copy(mvars.gMcuBinFile, mvars.MCU_APP_VERSION_ADDR, tmp, 0, 16);
                svs = System.Text.Encoding.ASCII.GetString(tmp);
                mvars.HexAppVer = Convert.ToUInt16(svs.Substring(svs.Length - 4, 4));

                #endregion 開啟hex

                #region 統一廣播先燒app code以便返回到app mode
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                UInt32 PacketSize = mvars.MCU_BLOCK_SIZE;
                UInt32 Count = mvars.APP_SIZE / PacketSize;
                //byte sverrc = 0;
                for (int svcnt = 0; svcnt < Count; svcnt++)
                {
                reBlWr:
                    if (svundoA[0] == false)
                    {
                        string txt44 = (svcnt * PacketSize + mvars.APP_START_ADDR).ToString("X8");
                        Application.DoEvents();
                        if (txt44 == "00060000" || txt44 == "00062000"
                         || txt44 == "00064000" || txt44 == "00066000"
                         || txt44 == "00068000" || txt44 == "0006A000"
                         || txt44 == "0006C000")
                        {
                            continue;
                        }
                        mp.Copy(mvars.gMcuBinFile, (int)(svcnt * PacketSize + mvars.APP_START_ADDR), mvars.gFlashRdPacketArr, 0, (int)PacketSize);
                        mvars.lblCmd = "MCU_APPWRITE";
                        mp.pMCUAPPWRITE(0x15, txt44);   //Primary dedicated
                        //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                        //{
                        //    //只有發送卡的一包2048才需要再重新計算
                        //    //int m = (int)svcnt;
                        //    //svcnt = (int)(m / 16) * 16;
                        //    if (sverrc < 3) { sverrc++; goto reBlWr; }
                        //    else { svundoA[0] = true; }
                        //}
                        //else
                        //{
                        //    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = " --> Bootloader Wr cnt：" + svcnt + " of " + (Count - 1); }
                        //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " --> 引導程序代碼寫入" + svcnt + " of " + (Count - 1); }
                        //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " --> 引导程序代码写入" + svcnt + " of " + (Count - 1); }
                        //    else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " --> Bootloader Wr cnt：" + svcnt + " of " + (Count - 1); }
                        //    mp.showStatus1(svmsg, lstget1, "");
                        //}
                    }
                    else
                    {
                        //燒完app code後還是一直維持在boot mode
                        if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = " -> There is a major abnormality,Please notify maintenance staff"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> 發生重大異常,請通知維修人員處理"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> 发生重大异常,请通知维修人员处理"; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> There is a major abnormality,Please notify maintenance staff"; }
                        mp.showStatus1(svmsg, lstget1, "");
                        return;
                    }
                }
                #endregion 統一廣播先燒app code以便返回到app mode






            }


            #endregion -------------------------------------------------- ↑從頭到尾全部確認過如果有在boot mode就先燒錄讓他回到app mode




            //goto dirEDID;

            upMCU:
            sDir = svpath + @"\MCU";
            #region Update MCU     
            if (Directory.Exists(sDir))
            {
                string svFileName = mp.fileSearch(sDir, "*.hex");
                if (svFileName != "")
                {
                    if (mvars.flgForceUpdate) mp.funSaveLogs("(Update)  forceUpdate"); else mp.funSaveLogs("(Update)  normalUpdate");

                    svFileName = svFileName.Split('+')[0];
                    mp.showStatus1("MCU Update... " + sDir + @"\" + svFileName, lstget1, "");

                    if (mvars.deviceID.Substring(0, 2) == "05")
                    {
                        #region 開啟hex file
                        string sTextLine;
                        Byte[] ucNvmUserPage = new byte[mvars.NVM_USER_PAGE];
                        bool bBreak = false;
                        bool bHexParseSuccess = false;

                        System.IO.StreamReader file = new System.IO.StreamReader(sDir + @"\" + svFileName);
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
                            if (sverr == "-1")
                                mp.showStatus1("  --> Error Hex File，Flash Address greater than Bin Size", lstget1, "");
                            else
                                mp.showStatus1("  --> Error Hex File", lstget1, "");
                            mp.showStatus1("  --> MCU Update... fail，Please contact the server administrator", lstget1, "");
                        }
                        else
                        {
                            //string sFilePath = txt_nMCUFileNameFull.Text.Replace(".hex", "(hex2bin).bin");
                            //File.WriteAllBytes(sFilePath, mvars.gMcuBinFile);
                            //File.WriteAllBytes("C:\\Users\\" + Environment.UserName + "\\Documents\\NVM_User_Page.bin", ucNvmUserPage);
                            //lst_get1.Items.Add("Hex 2 Bin OK");
                            //lst_get1.Items.Add(sFilePath);

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

                                Array.Resize(ref tmp, 16);
                                mp.Copy(mvars.gMcuBinFile, mvars.MCU_APP_VERSION_ADDR, tmp, 0, 16);
                                mp.showStatus1("  --> " + "  AppCode..... " + Encoding.ASCII.GetString(tmp), lstget1, "");
                                mp.showStatus1("  --> " + "  Version..... " + split[0], lstget1, "");
                                mp.showStatus1("  --> " + "  Date.......... " + split[1], lstget1, "");
                                mp.showStatus1("  --> " + "  Time.......... " + split[2], lstget1, "");
                                mp.showStatus1("  --> " + "  Project....... " + split[3], lstget1, "");
                            }
                            catch (Exception ex) 
                            {
                                sverr = "-2";
                                mp.showStatus1("  --> " + ex.Message, lstget1, "");
                            }

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
                                    catch (Exception ex) 
                                    {
                                        mp.showStatus1("  --> " + ex.Message, lstget1, "");
                                    }
                                }
                                tmp = Array.FindAll(tmp, val => val != 0).ToArray();

                                string MCU_Ver = System.Text.Encoding.ASCII.GetString(tmp);
                                MCU_Ver = MCU_Ver.Replace("?", "");
                                string[] splitBoot = MCU_Ver.Split(new Char[] { '\t' });

                                Array.Resize(ref tmp, 16);
                                mp.Copy(mvars.gMcuBinFile, mvars.MCU_BOOT_VERSION_ADDR, tmp, 0, 16);
                                mp.showStatus1("  --> " + "  BootCode... " + Encoding.ASCII.GetString(tmp), lstget1, "");
                                mp.showStatus1("  --> " + "  Version..... " + splitBoot[0], lstget1, "");
                                mp.showStatus1("  --> " + "  Date.......... " + splitBoot[1], lstget1, "");
                                mp.showStatus1("  --> " + "  Time.......... " + splitBoot[2], lstget1, "");
                                mp.showStatus1("  --> " + "  Project....... " + splitBoot[3], lstget1, "");
                            }
                            catch (Exception ex) 
                            {
                                sverr = "-3";
                                mp.showStatus1("  --> " + ex.Message, lstget1, "");
                            }


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
                        }

                        tmp = new byte[16];
                        mp.Copy(mvars.gMcuBinFile, mvars.MCU_BOOT_VERSION_ADDR, tmp, 0, 16);
                        string svs = System.Text.Encoding.ASCII.GetString(tmp);
                        mvars.HexBootVer = Convert.ToUInt16(svs.Substring(svs.Length - 4, 4));
                        mp.Copy(mvars.gMcuBinFile, mvars.MCU_APP_VERSION_ADDR, tmp, 0, 16);
                        svs = System.Text.Encoding.ASCII.GetString(tmp);
                        mvars.HexAppVer = Convert.ToUInt16(svs.Substring(svs.Length - 4, 4));

                        #endregion 開啟hex

                        if (sverr == "0")
                            mp.cFLASHWRITE_pMCU(Form1.nvsender[0].regPoCards, lstget1, 0);  //svDevices是總數所以從1起 svDevCnt從0起
                    }

                    mp.showStatus1("", lstget1, "");
                    mp.showStatus1("  --> " + "MCU Update... Finish", lstget1, "");

                    if (mvars.errCode == "0") svUpdate = svUpdate.Remove(0, 1).Insert(0, "1");
                }
                else
                {
                    mp.showStatus1("", lstget1, "");
                    mp.showStatus1("  --> " + "MCU Update... Fail, No *.Hex file", lstget1, "");
                }
            }
            #endregion Update MCU

            //goto ex;

            dirFPGA:
            //dgvota.Rows[0].Cells[3].Value = "20230510_Primary_6170h_OnSync_u12_b_v6170.bin,20230510_Primary_6171h_OnSync_u10_a_v6171.bin";
            sDir = svpath + @"\FPGA";
            #region Update FPGA
            //D:\A_桃流\AIO\20230901_AIO_v0026_開始確認檔案接收後的燒錄\AIO\bin\Debug\Parameter\Update\FPGA
            //sDir = mvars.strStartUpPath + @"\Parameter\Update\FPGA";
            if (mvars.deviceID.Substring(0, 2) == "05" && Directory.Exists(sDir))
            {
                string[] svverFPGAm = new string[2];
                for (byte svw = 1; svw <= 2; svw++)
                {
                    sverr = "0";
                    
                    tmp = new byte[] { (byte)(64 + svw) };
                    string svsort = "*_" + Encoding.ASCII.GetString(tmp) + "*.bin";
                    string svFileName = mp.fileSearch(sDir, svsort);
                    if (svFileName != "")
                    {
                        svFileName = svFileName.Split('+')[0];
                        mp.showStatus1("", lstget1, "");
                        mp.showStatus1("FPGA_" + Encoding.ASCII.GetString(tmp) + " Update... " + sDir + @"\" + svFileName, lstget1, "");
                        
                        if (mp.GetBin(sDir + @"\" + svFileName))
                        {
                            int checksum = (int)mp.CalCheckSum(mvars.ucTmp, 0, mvars.ucTmp.Length);
                            if (mvars.ucTmp.Length % (32 * 1024) != 0)
                            {
                                uint quotient = (uint)mvars.ucTmp.Length / (32 * 1024);
                                Array.Resize(ref mvars.ucTmp, (int)(quotient + 1) * 32 * 1024);
                            }
                            mp.showStatus1("  --> " + " size：" + mvars.ucTmp.Length + "  checksum：0x" + checksum.ToString("X4"), lstget1, "");
                            mp.showStatus1("", lstget1, "");

                            string[] svs = svFileName.Split('_');
                            for (int svi = 0; svi < svs.Length; svi++)
                            {
                                if (svs[svi].ToUpper().Substring(0, 1) == "V")
                                {
                                    string txtverfbin = svs[svi].Substring(1, svs[svi].Length - 1);
                                    txtverfbin = txtverfbin.Replace(".", string.Empty);
                                    int count = 0;
                                    foreach (char c in txtverfbin)
                                    {
                                        if (c == '.')
                                        {
                                            count++;
                                        }
                                    }
                                    if (count == 2) break;
                                }
                            }
                            mvars.flashselQ = svw;

                            #region FPGA W
                            if (sverr == "0" && mvars.demoMode == false) mp.cFLASHWRITE_pCB("FPGA_" + Encoding.ASCII.GetString(tmp), lstget1, svFileName);

                            if (mvars.errCode == "0") svUpdate = svUpdate.Remove(1, 1).Insert(1, "1");
                            #endregion FPGA W
                            
                            mvars.flashselQ = 0;

                            mp.showStatus1("FPGA_" + Encoding.ASCII.GetString(tmp) + " Update... DONE", lstget1, "");
                            mp.showStatus1("", lstget1, "");

                        }
                        else sverr = "Load " + sDir + @"\" + svFileName + " fail";
                    }
                    else sverr = @"There are no bin file in the folder """ + sDir + @"""";
                }
            }
            #endregion Update FPGA

            //goto ex;

            dirEDID:
            sDir = svpath + @"\EDID";
            #region EDID
            //sDir = mvars.strStartUpPath + @"\Parameter\Update\EDID";
            if (mvars.deviceID.Substring(0, 2) == "05" && Directory.Exists(sDir))
            {
                sverr = "0";
                bool[] svundo = new bool[Form1.nvsender[0].regPoCards];
                DirectoryInfo di = new DirectoryInfo(sDir);
                FileInfo[] afi = di.GetFiles("*.*");
                string f;

                if (afi.Length > 0)
                {
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                    mp.showStatus1("EDID Update... ", lstget1, "");

                    for (int i = 0; i < afi.Length; i++)
                    {
                        f = afi[i].Name.ToLower();
                        if (f.IndexOf(".hex", 0) != -1)
                        {
                            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                            mp.showStatus1(sDir + @"\" + f + " writing... addr 000000，per.1024", lstget1, "");
                            if (mvars.demoMode == false) mp.cLT8668Write(lstget1, sDir + @"\" + f, "000000", true, 1024, false);
                        }
                        else if (f.IndexOf("myfg0fg1bg_sram.txt", 0) != -1)
                        {
                            mp.showStatus1(sDir + @"\" + f + " writing... addr 054000，per.1024", lstget1, "");
                            if (mvars.demoMode == false) mp.cLT8668Write(lstget1, sDir + @"\" + f, "054000", true, 1024, true);
                        }
                        else if (f.IndexOf("mycmd_sram.txt", 0) != -1)
                        {
                            mp.showStatus1(sDir + @"\" + f + " writing... addr 060000，per.1024", lstget1, "");
                            if (mvars.demoMode == false) mp.cLT8668Write(lstget1, sDir + @"\" + f, "060000", true, 1024, false);
                        }
                        else if (f.IndexOf("fontdata.txt", 0) != -1)
                        {
                            mp.showStatus1(sDir + @"\" + f + " writing... addr 068000，per.1024", lstget1, "");
                            if (mvars.demoMode == false) mp.cLT8668Write(lstget1, sDir + @"\" + f, "068000", true, 1024, false);
                        }
                    }
                    svUpdate = svUpdate.Remove(2, 1).Insert(2, "1");
                }

                if (svUpdate.IndexOf("1", 0) != -1)
                {
                    mvars.lblCmd = "LT8668_Reset_L";
                    if (mvars.demoMode == false) mp.LT8668_Reset_L();
                    mp.doDelayms(500);
                    mvars.lblCmd = "LT8668_Reset_H";
                    if (mvars.demoMode == false) mp.LT8668_Reset_H();
                    mp.doDelayms(1000);

                    //20231023 單獨對龍訊下reset會發生scale回到預設值(scale on)，但是對於特定解析度(非960*540長寬比的解析度要下scale off)，要的話就要去回讀EDID然後判斷解析度在去下正確的scale on或off
                    //總而言之更新完就直接對MCU下Reset
                    mvars.lblCmd = "MCU_RESET";
                    if (mvars.demoMode == false) mp.McuSW_Reset();
                    mp.showStatus1("MCU Reset", lstget1, "");

                    svWaitSec = 14;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US")
                            mp.showStatus1(" -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec", lstget1, "");
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            mp.showStatus1(" -> " + mvars.lblCmd + "  請稍後 " + svWaitSec + " 秒", lstget1, "");
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                            mp.showStatus1(" -> " + mvars.lblCmd + "  请稍后 " + svWaitSec + " 秒", lstget1, "");
                        else if (MultiLanguage.DefaultLanguage == "ja-JP")
                            mp.showStatus1(" -> " + mvars.lblCmd + "  待って " + svWaitSec + " 秒", lstget1, "");
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        svWaitSec--;
                    } while (svWaitSec > 0);

                    #region ----------------------------------------------------- ↓以版本確認svundo
                    for (byte svd = (byte)Form1.nvsender[0].regPoCards; svd >=1 ; svd--)
                    {
                        if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                        else
                            mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);      //" 05" + mp.DecToHex(i, 2)

                        Byte[] arr = new byte[2]; //Byte[] rd_arr = new byte[2];
                        uc_box.LT8668rd_arr = new byte[1];
                        byte VerHi = 0, VerLo = 0;

                        mvars.lblCmd = "LT8668_Bin_WrRd";
                        arr[0] = 0x82;
                        if (mvars.demoMode == false) mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                        else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.ReadDataBuffer[7] = 1; }
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                        {
                            VerHi = mvars.ReadDataBuffer[7];
                            mvars.lblCmd = "LT8668_Bin_WrRd";
                            arr[0] = 0x83;
                            if (mvars.demoMode == false) mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                            else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.ReadDataBuffer[7] = 20; }
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) VerLo = mvars.ReadDataBuffer[7];
                        }
                        mvars.verEDID = VerHi + "." + VerLo;
                        mp.showStatus1("(" + sverr + ") ID." + svd.ToString("00") + ",EDID version," + mvars.verEDID, lstget1, "");
                    }
                    #endregion -------------------------------------------------- ↑以版本確認svundo
                    mp.showStatus1("", lstget1, "");
                }
            }
            #endregion EDID

            if (svUpdate.Substring(0, 1) == "1")
                mp.showStatus1("MCU Updated ....... " + mvars.verMCU + "(" + mvars.verMCUA + "." + mvars.verMCUB + ")", lstget1, "");
            
            if (svUpdate.Substring(1, 1) == "1")
                mp.showStatus1("FPGA Updated ..... " + mvars.verFPGAm[0] + "-" + mvars.verFPGAm[1], lstget1, "");
            
            if (svUpdate.Substring(2, 1) == "1") 
                mp.showStatus1("EDID Updated ....... " + mvars.verEDID, lstget1, "");
            
        ex:
            mvars.deviceID = svdeviceID;
            mp.showStatus1("", lstget1, "");
            mp.showStatus1("Updated ....... " + (DateTime.Now - t1).TotalSeconds.ToString("##0") + "s", lstget1, "");
            mp.showStatus1("", lstget1, "");
        }

        private void Skeleton_formmsg(int SvCols, int SvRows, int SvLBw, int SvLBh)
        {
            int SvR = 0;
            int SvC = 0;    //SvCols=mvars.NovaGamma.Length=9
            dgv_formmsg.ReadOnly = true;
            Font f = new Font("Arial", 7);
            dgv_formmsg.Font = f;
            //是否允許使用者自行新增
            dgv_formmsg.AllowUserToAddRows = false;
            dgv_formmsg.AllowUserToResizeRows = false;
            dgv_formmsg.AllowUserToResizeColumns = false;
            dgv_formmsg.CellBorderStyle = DataGridViewCellBorderStyle.None;

            SvC = 0;
            dgv_formmsg.Columns.Add("Col" + SvC.ToString(), "List");
            dgv_formmsg.Columns[SvC].Width = 60;
            dgv_formmsg.Columns[SvC].SortMode = DataGridViewColumnSortMode.NotSortable;
            //dgv_formmsg.ColumnHeadersHeight = 24;
            dgv_formmsg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SvC = 1;
            dgv_formmsg.Columns.Add("Col" + SvC.ToString(), "Log");
            dgv_formmsg.Columns[SvC].Width = 500;
            dgv_formmsg.Columns[SvC].SortMode = DataGridViewColumnSortMode.NotSortable;
            //dgv_formmsg.ColumnHeadersHeight = 24;
            dgv_formmsg.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv_formmsg.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 7);

            dgv_formmsg.ShowCellToolTips = false;

            DataGridViewRowCollection rows = dgv_formmsg.Rows;
            for (SvR = 0; SvR < SvRows; SvR++)
            {
                rows.Add();
                DataGridViewRow row = dgv_formmsg.Rows[SvR]; row.Height = 18;
            }
            dgv_formmsg.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgv_formmsg.RowHeadersWidth = 72;
            dgv_formmsg.RowHeadersDefaultCellStyle.Padding = new Padding(50);
            dgv_formmsg.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;//靠左顯示

            SvLBw = 3;
            for (int i = 0; i < dgv_formmsg.ColumnCount; i++)
                SvLBw += dgv_formmsg.Columns[i].Width;
            dgv_formmsg.Width = SvLBw + 22;
            //if (dgv_formmsg.Width < this.Width) dgv_formmsg.Width = this.Width - 40;
            dgv_formmsg.Width = this.Width - 40;
            dgv_formmsg.Columns[1].Width = dgv_formmsg.Width - 60;
            dgv_formmsg.Height = 24 + 20 + 20 + 20 + 20 + 20;
            //dgv_formmsg.Visible = true;
            dgv_formmsg.Location = new Point(12, 50);
            //ScrollBar
            dgv_formmsg.ScrollBars = ScrollBars.Both;
            dgv_formmsg.TopLeftHeaderCell.Value = "";
            dgv_formmsg.Rows[0].Cells[0].Selected = false;
            mvars.dgvRows = 0;
        }

        private void Skeleton_ota(DataGridView _dgv, int SvCols, int SvRows, int SvLBw, int SvLBh)
        {
            int SvR = 0;
            int SvC = 0;    //SvCols=mvars.NovaGamma.Length=9
            _dgv.ReadOnly = true;
            Font f = new Font("Arial", 7);
            _dgv.Font = f;
            //是否允許使用者自行新增
            _dgv.AllowUserToAddRows = false;
            _dgv.AllowUserToResizeRows = false;
            _dgv.AllowUserToResizeColumns = false;
            _dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;

            for (SvC = 0; SvC < SvCols; SvC++)
            {
                _dgv.Columns.Add("Col" + SvC.ToString(), dgvhUtxt[SvC]);
                _dgv.Columns[SvC].Width = dgvhUw[SvC];
                _dgv.Columns[SvC].SortMode = DataGridViewColumnSortMode.NotSortable;
                _dgv.ColumnHeadersHeight = 24;
                _dgv.Columns[SvC].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            _dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 7);

            _dgv.ShowCellToolTips = false;

            DataGridViewRowCollection rows = _dgv.Rows;
            for (SvR = 0; SvR < SvRows; SvR++)
            {
                rows.Add();
                DataGridViewRow row = _dgv.Rows[SvR]; row.Height = 18;
            }
            _dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //_dgv.RowHeadersWidth = 72;
            _dgv.RowHeadersDefaultCellStyle.Padding = new Padding(50);
            _dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;//靠左顯示

            SvLBw = 3;
            for (int i = 0; i < _dgv.ColumnCount; i++)
                SvLBw += _dgv.Columns[i].Width;
            _dgv.Width = dgv_formmsg.Width;
            _dgv.Height = 24 + 20 + 20;
            //_dgv.Visible = true;
            _dgv.Location = new Point(dgv_formmsg.Left, dgv_formmsg.Top + dgv_formmsg.Height + 10);
            //ScrollBar
            _dgv.ScrollBars = ScrollBars.Both;
            _dgv.TopLeftHeaderCell.Value = "";
            _dgv.Rows[0].Cells[0].Selected = false;
        }
        #endregion OTA



        #region 利用Socket建立基礎連線
        private static void ReceiveMsgFromServer()
        {
            while (true)
            {
                if (nsckC.Connected == false)
                    break;
                byte[] buffer = new byte[1024];
                buffer = new byte[1024 * 5000];
                int rec = nsckC.Receive(buffer);
                if (rec == 0)
                {
                    //ShowMsg("Server Loss!");
                    break;
                }
                string data = System.Text.Encoding.UTF8.GetString(buffer, 0, rec);
                //ShowMsg("Server :" + receText);
                if (data.Length != 0) nsckC_DataArrival(data);
            }
        }

        public static void nsckC_DataArrival(string data)
        {
            mp.showStatus1("(IN)      " + data, lstget1, "");
            string RX = data.ToUpper();

            string[] svdata = data.Split('@');
            mvars.sckSindex = Convert.ToByte(svdata[0].Split(',')[0]);
            string svreceiveIP = svdata[0].Split(',')[1];

            byte[] sendBytes;
            sendData = "";
            if (!(RX.Length >= "SERVERBUSY".Length && RX.IndexOf("SERVERBUSY", 0) != -1) && svreceiveIP == hostIP)
            {
                mvars.lblCmd = data;
                string[] svstr = svdata[1].Split(',');
                #region ----------------------------- Authentic --------------------------------
                if (svdata[1].Length > "ShowAuthFrm,".Length && svdata[1].Substring(0, "ShowAuthFrm,".Length) == "ShowAuthFrm,")
                {
                    //mvars.sckSindex = Convert.ToByte(RX.Split(',')[1]);   //移到最上層
                    sendData = mvars.sckSindex + "@@GetRsCount~~" + hostIP;
                    mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                    sendBytes = Encoding.UTF8.GetBytes(sendData);
                    nsckC.Send(sendBytes);

                    
                }

                if (svdata[1].Length > "Rs,".Length && svdata[1].Substring(0, "Rs,".Length) == "Rs,")
                {
                    sendData = mvars.sckSindex + "@@Request~~" + hostIP + "~~" + mvars.strNamePC + "~~" + mvars.deviceName + "~~514170~~" + mvars.strNameLogin;
                    mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                    sendBytes = Encoding.UTF8.GetBytes(sendData);
                    nsckC.Send(sendBytes);
                }

                if (svdata[1].Length > "Validated,".Length && svdata[1].Substring(0, "Validated,".Length) == "Validated,")
                {
                    #region multiLanguage
                    if (MultiLanguage.DefaultLanguage == "en-US")
                        mp.showStatus1("Validated", lstget1, "");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        mp.showStatus1("已取得資料庫認證", lstget1, "");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        mp.showStatus1("已取得资料库认证", lstget1, "");
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        mp.showStatus1("Validated", lstget1, "");
                    #endregion multiLanguage

                    //獲得Server來的對應 Port
                    if (svdata[1].Split(',').Length >= 4) { Form1.serverPortnsckF = svdata[1].Split(',')[5]; dgvota.Rows[0].Cells[1].Value = svdata[1].Split(',')[5]; }

                    sendData = mvars.sckSindex + "@@BarcodeID~~" + hostIP + "~~" + mvars.deviceName + "~~" + mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4) + "~~" + mvars.verFPGAm[0] + "-" + mvars.verFPGAm[1] + "~~" + mvars.verEDID;
                    mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                    sendBytes = Encoding.UTF8.GetBytes(sendData);
                    nsckC.Send(sendBytes);
                }
                if (svdata[1].Length > "InValidate,".Length && svdata[1].Substring(0, "InValidate,".Length) == "InValidate,")
                {
                    mp.showStatus1("(Err) " + tsmnuota.Text + "_" + svdata[1], lstget1, "");
                }
                #endregion -------------------------- Authentic --------------------------------


                #region ----------------------------- Barcode (mp.fileDelete) ------------------
                if (svdata[1].Length > "fBarcodeID,".Length && svdata[1].Substring(0, "fBarcodeID,".Length) == "fBarcodeID,")
                {
                    //svstr = svdata[1].Split(',');
                    //svstr[0]=fBarcode
                    //svstr[1]=Client HostIP
                    //svstr[2]=0500
                    //svstr[3]=TW063192N
                    //svstr[4]=Primary
                    //svstr[5]=MCU hex file name，from server/parts/barcode(model)/MCU/...
                    //svstr[6]=FPGA bin file name，from server/parts/barcode(model)/FPGA/... (...v6170.bin+...v6171.bin，split char'+')
                    //svstr[7]=EDID all file name，from server/parts/barcode(model)/EDID/... (...FontData.txt+LT8668...HDMI...V1.16.hex+...sram.txt+...sram.txt)

                    if (svstr[5] == "0" && svstr[6] == "0" && svstr[7] == "0")
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US")
                            mp.showStatus1("Firmware version is up to date", lstget1, "");
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            mp.showStatus1("固件版本是最新的", lstget1, "");
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                            mp.showStatus1("固件版本是最新的", lstget1, "");
                        else if (MultiLanguage.DefaultLanguage == "ja-JP")
                            mp.showStatus1("Firmware version is up to date", lstget1, "");
                        sendData = mvars.sckSindex + "@@UpToDate~~" + mvars.verMCU + "~~" + mvars.verFPGAm[0] + "-" + mvars.verFPGAm[1] + "~~" + mvars.verEDID + "~~" + hostIP + "~~" + mvars.strNamePC + "~~" + mvars.deviceName + "~~" + mvars.strNameLogin;
                        mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);

                        Thread.Sleep(1000);
                        nsckC.Shutdown(SocketShutdown.Both);
                        //mp.showStatus1("", lstget1, "");
                        //mp.showStatus1("File received " + svs + " sec", lstget1, "");
                        //mp.showStatus1("Socket disconnected", lstget1, "");
                        //mp.showStatus1("Firmware updating ..... ", lstget1, "");
                        if (mvars.FormShow[11] == false) tmeota.Enabled = false;

                        if (mvars.FormShow[11] == false)
                        {
                            dgvota[0, 1].Style.BackColor = Color.White;
                            dgvota.Rows[0].Cells[1].Value = "";
                            dgvota.Rows[0].Cells[8].Value = "0";
                            dgvota.Rows[0].Cells[9].Value = "0";
                            dgvota.Rows[0].Cells[10].Value = "";
                            dgvota.Visible = false;
                            dgvota.Visible = false;
                            hUser.Enabled = true;
                            hExit.Enabled = true;
                        }
                    }
                    else
                    {
                        dgvota.Rows[0].Cells[5].Value = "";             //[5]lstfilestrMCU.Items.Clear();
                        dgvota.Rows[0].Cells[6].Value = "";             //[6]lstfilestrFPGA.Items.Clear();
                        dgvota.Rows[0].Cells[7].Value = "";             //[7]lstfilestrEDID.Items.Clear();

                        dgvota.Rows[0].Cells[2].Value = svstr[5];       //txtfilestrMCU.Text = svstr[5];
                        dgvota.Rows[0].Cells[3].Value = svstr[6];       //txtfilestrFPGA.Text = svstr[6];
                        dgvota.Rows[0].Cells[4].Value = svstr[7];       //txtfilestrEDID.Text = svstr[7];

                        if (svstr[5].IndexOf(mvars.verMCU, 0) == -1 ||
                            svstr[6].IndexOf(mvars.verFPGAm[0], 0) == -1 ||
                            svstr[6].IndexOf(mvars.verFPGAm[1], 0) == -1 ||
                            svstr[7].IndexOf(mvars.verEDID, 0) == -1 ||
                            svstr[7].IndexOf(".txt", 0) != -1)
                        {
                            #region multiLanguage
                            if (MultiLanguage.DefaultLanguage == "en-US")
                                mp.showStatus1("There is new firmware on the server to provide update", lstget1, "");
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                mp.showStatus1("伺服器有新版本提供更新", lstget1, "");
                            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                mp.showStatus1("伺服器有新版本提供更新", lstget1, "");
                            else if (MultiLanguage.DefaultLanguage == "ja-JP")
                                mp.showStatus1("アップデートを提供するための新しいファームウェアがサーバー上にあります", lstget1, "");
                            #endregion multiLanguage

                            if (svstr[5].IndexOf(mvars.verMCU, 0) == -1 && Directory.Exists(mvars.strStartUpPath + @"\Parameter\Update\MCU") == true)
                                mp.fileDelete(mvars.strStartUpPath + @"\Parameter\Update\MCU");
                            if ((svstr[6].IndexOf(mvars.verFPGAm[0], 0) == -1 || svstr[6].IndexOf(mvars.verFPGAm[1], 0) == -1) && Directory.Exists(mvars.strStartUpPath + @"\Parameter\Update\FPGA") == true)
                                mp.fileDelete(mvars.strStartUpPath + @"\Parameter\Update\FPGA");
                            if ((svstr[7].IndexOf(mvars.verEDID, 0) == -1 || svstr[7].IndexOf(".txt", 0) != -1) && Directory.Exists(mvars.strStartUpPath + @"\Parameter\Update\EDID") == true)
                                mp.fileDelete(mvars.strStartUpPath + @"\Parameter\Update\EDID");

                            // FPGArequest/EDIDrequest有兩個地方重複同樣的碼 finish xxxx file
                            if (svstr[5] != "0")
                            {
                                string[] svf = svstr[5].Split('+');
                                sendData = mvars.sckSindex + "@@MCUrequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                                dgvota.Rows[0].Cells[5].Value = "";
                                //一次傳一個檔案名，直到lstfilestrMCU.items.counter==0
                                sendData += "~~" + svf[0];
                                mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                                if (svf.Length - 1 > 0)
                                {
                                    for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[5].Value += svf[svm] + "\r";
                                    if (dgvota.Rows[0].Cells[5].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[5].Value.ToString().Length - "\r".Length) != -1)
                                        dgvota.Rows[0].Cells[5].Value = dgvota.Rows[0].Cells[5].Value.ToString().Substring(0, dgvota.Rows[0].Cells[5].Value.ToString().Length - "\r".Length);
                                }
                            }
                            else if (svstr[6] != "0")
                            {
                                if (svstr[6].IndexOf(mvars.verFPGAm[0], 0) == -1 || svstr[6].IndexOf(mvars.verFPGAm[1], 0) == -1)
                                {
                                    string[] svf = svstr[6].Split('+');
                                    sendData = mvars.sckSindex + "@@FPGArequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                                    dgvota.Rows[0].Cells[6].Value = "";
                                    //一次傳一個檔案名，直到lstfilestrFPGA.items.counter==0
                                    sendData += "~~" + svf[0];
                                    //mp.showStatus1(svf[0], lstget1, "");
                                    mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                                    if (svf.Length - 1 > 0)
                                    {
                                        for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[6].Value += svf[svm] + "\r";
                                        if (dgvota.Rows[0].Cells[6].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[6].Value.ToString().Length - "\r".Length) != -1)
                                            dgvota.Rows[0].Cells[6].Value = dgvota.Rows[0].Cells[6].Value.ToString().Substring(0, dgvota.Rows[0].Cells[6].Value.ToString().Length - "\r".Length);
                                    }
                                }
                            }
                            else if (svstr[7] != "0")
                            {
                                if (svstr[7].IndexOf(mvars.verEDID, 0) == -1 || svstr[7].IndexOf(".txt", 0) != -1)
                                {
                                    string[] svf = svstr[7].Split('+');
                                    sendData = mvars.sckSindex + "@@EDIDrequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                                    dgvota.Rows[0].Cells[7].Value = "";
                                    //一次傳一個檔案名，直到lstfilestrEDID.items.counter==0
                                    sendData += "~~" + svf[0];
                                    mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                                    if (svf.Length - 1 > 0)
                                    {
                                        for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[7].Value += svf[svm] + "\r";
                                        if (dgvota.Rows[0].Cells[7].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[7].Value.ToString().Length - "\r".Length) != -1)
                                            dgvota.Rows[0].Cells[7].Value = dgvota.Rows[0].Cells[7].Value.ToString().Substring(0, dgvota.Rows[0].Cells[7].Value.ToString().Length - "\r".Length);
                                    }
                                }
                            }
                            dgvota.Rows[1].Cells[0].Value = sendData;
                            sendBytes = Encoding.UTF8.GetBytes(sendData);
                            nsckC.Send(sendBytes);
                        }
                    }
                }
                if (svdata[1].Length > "errBarcodeID,".Length && svdata[1].Substring(0, "errBarcodeID,".Length) == "errBarcodeID,")
                {
                    mvars.lstMsgIn.Items.Add("OTAerr~" + data);
                    mp.showStatus1("(Err) " + tsmnuota.Text + "," + data, lstget1, "");
                }

                #endregion -------------------------- Barcode (mp.fileDelete) ------------------


                #region ----------------------------- 收到 Server 的 Send xxxx file ------------ 建立...\Parameter\Update\MCU or FPGA or EDID 
                if ((svdata[1].Length > "SendMCUfile,".Length && svdata[1].Substring(0, "SendMCUfile,".Length) == "SendMCUfile,") ||
                    (svdata[1].Length > "SendFPGAfile,".Length && svdata[1].Substring(0, "SendFPGAfile,".Length) == "SendFPGAfile,") ||
                    (svdata[1].Length > "SendEDIDfile,".Length && svdata[1].Substring(0, "SendEDIDfile,".Length) == "SendEDIDfile,"))
                {
                    //mvars.flgAccept = true;
                    // from Server --> "Sendxxxfile," + mvars.clientIP[mvars.handleIDfrom] + "," + svs + "," + mvars.ucTmp.Length;

                    //svstr = svdata[1].Split(',');

                    string svfilename = svstr[2].Split('\\')[svstr[2].Split('\\').Length - 1];
                    serverPortnsckF = svstr[1];
                    dgvota.Rows[0].Cells[8].Value = svstr[3];   //getfilelength
                    dgvota.Rows[0].Cells[9].Value = "0";        //getdatacount

                    Array.Resize(ref mvars.ucTmp, Convert.ToInt32(svstr[3]));
                    if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Update") == false) Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Update");

                    if (svdata[1].Length > "SendMCUfile,".Length && svdata[1].Substring(0, "SendMCUfile,".Length) == "SendMCUfile,")
                    {
                        if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Update\MCU") == false) Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Update\MCU");
                        dgvota.Rows[0].Cells[10].Value = "@@ReadyToAccept~~MCU~~";
                        sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "0";
                        mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        Thread.Sleep(2000);
                        sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);

                        nsckF.Close();
                        if (nsckF != null) nsckF.Dispose();
                        try
                        {
                            dgvota[0, 1].Style.BackColor = Color.Pink;
                            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), int.Parse(Form1.serverPortnsckF));   //準備連到server的IPaddress
                                                                                                                                    //ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), 6699);   //20240119 準備連到serverF的IPaddress
                            nsckF = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            nsckF.Connect(ipEnd);
                            if (nsckF.Connected)
                            {
                                dgvota[0, 1].Style.BackColor = Color.LightGreen;
                                //nsckF.Send(sendBytes);    /20240119 準備連到serverF的IPaddress
                            }
                            Thread _th = new Thread(listen_text);
                            _th.IsBackground = true;
                            _th.Start(dgvota.Rows[0].Cells[8].Value.ToString() + "~" + mvars.strStartUpPath + @"\Parameter\Update\MCU\");
                            //一旦呼叫執行緒物件的 Start 方法令它開始執行，其 IsAlive 屬性值就會等於 true，直到該執行緒的委派方法執行完畢，那條執行緒便隨之結束。
                            //while (_th.IsAlive);
                            //{
                            //   Thread.Sleep(200);
                            //}
                            //更簡單的作法可以等待執行緒結束：呼叫 Thread 物件的 Join 方法。
                            _th.Join();

                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Update\MCU\" + "temp.txt"))
                            {
                                File.Move(mvars.strStartUpPath + @"\Parameter\Update\MCU\" + "temp.txt", mvars.strStartUpPath + @"\Parameter\Update\MCU\" + svfilename);
                                sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "1";
                                mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                                sendBytes = Encoding.UTF8.GetBytes(sendData);
                                nsckC.Send(sendBytes);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        //if (File.Exists(mvars.strStartUpPath + @"\Parameter\Update\MCU\" + "temp.txt"))
                        //{
                        //    File.Move(mvars.strStartUpPath + @"\Parameter\Update\MCU\" + "temp.txt", mvars.strStartUpPath + @"\Parameter\Update\MCU\" + svfilename);
                        //    sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "1";
                        //    mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        //    sendBytes = Encoding.UTF8.GetBytes(sendData);
                        //    nsckC.Send(sendBytes);
                        //}
                        ////mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        ////sendBytes = Encoding.UTF8.GetBytes(sendData);
                        ////nsckC.Send(sendBytes);
                    }

                    if (svdata[1].Length > "SendFPGAfile,".Length && svdata[1].Substring(0, "SendFPGAfile,".Length) == "SendFPGAfile,")
                    {
                        if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Update\FPGA") == false) Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Update\FPGA");
                        dgvota.Rows[0].Cells[10].Value = "@@ReadyToAccept~~FPGA~~";
                        sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "0";
                        mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        Thread.Sleep(2000);
                        sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);

                        nsckF.Close();
                        if (nsckF != null) nsckF.Dispose();
                        try
                        {
                            dgvota[0, 1].Style.BackColor = Color.Pink;
                            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), int.Parse(Form1.serverPortnsckF));   //準備連到server的IPaddress
                                                                                                                                    //ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), 6699);   //20240119 準備連到serverF的IPaddress
                            nsckF = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            nsckF.Connect(ipEnd);
                            if (nsckF.Connected)
                            {
                                dgvota[0, 1].Style.BackColor = Color.LightGreen;
                                //nsckF.Send(sendBytes);    /20240119 準備連到serverF的IPaddress
                            }
                            Thread _th = new Thread(listen_binary);
                            _th.IsBackground = true;
                            _th.Start(dgvota.Rows[0].Cells[8].Value.ToString() + "~" + mvars.strStartUpPath + @"\Parameter\Update\FPGA\");    //[8]getfilelength
                                                                                                                                              //一旦呼叫執行緒物件的 Start 方法令它開始執行，其 IsAlive 屬性值就會等於 true，直到該執行緒的委派方法執行完畢，那條執行緒便隨之結束。
                                                                                                                                              //while (_th.IsAlive);
                                                                                                                                              //{
                                                                                                                                              //   Thread.Sleep(200);
                                                                                                                                              //}
                                                                                                                                              //更簡單的作法可以等待執行緒結束：呼叫 Thread 物件的 Join 方法。
                            _th.Join();

                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Update\FPGA\" + "temp.bin"))     //斷點
                            {
                                File.Move(mvars.strStartUpPath + @"\Parameter\Update\FPGA\" + "temp.bin", mvars.strStartUpPath + @"\Parameter\Update\FPGA\" + svfilename);
                                sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "1";
                                mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                                sendBytes = Encoding.UTF8.GetBytes(sendData);
                                nsckC.Send(sendBytes);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        //if (File.Exists(mvars.strStartUpPath + @"\Parameter\Update\FPGA\" + "temp.bin"))     //斷點
                        //{
                        //    File.Move(mvars.strStartUpPath + @"\Parameter\Update\FPGA\" + "temp.bin", mvars.strStartUpPath + @"\Parameter\Update\FPGA\" + svfilename);
                        //    sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "1";
                        //    mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        //    sendBytes = Encoding.UTF8.GetBytes(sendData);
                        //    nsckC.Send(sendBytes);
                        //}
                        ////mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        ////sendBytes = Encoding.UTF8.GetBytes(sendData);
                        ////nsckC.Send(sendBytes);
                    }

                    if (svdata[1].Length > "SendEDIDfile,".Length && svdata[1].Substring(0, "SendEDIDfile,".Length) == "SendEDIDfile,")
                    {
                        //↓斷點
                        if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Update\EDID") == false) Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Update\EDID");
                        dgvota.Rows[0].Cells[10].Value = "@@ReadyToAccept~~EDID~~";
                        sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "0";
                        mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        Thread.Sleep(2000);
                        sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);

                        nsckF.Close();
                        if (nsckF != null) nsckF.Dispose();
                        try
                        {
                            dgvota[0, 1].Style.BackColor = Color.Pink;
                            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), int.Parse(Form1.serverPortnsckF));   //準備連到server的IPaddress
                                                                                                                                    //ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), 6699);   //20240119 準備連到serverF的IPaddress
                            nsckF = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            nsckF.Connect(ipEnd);
                            if (nsckF.Connected)
                            {
                                dgvota[0, 1].Style.BackColor = Color.LightGreen;
                                //nsckF.Send(sendBytes);    /20240119 準備連到serverF的IPaddress
                            }
                            Thread _th = new Thread(listen_text);
                            _th.IsBackground = true;
                            _th.Start(dgvota.Rows[0].Cells[8].Value.ToString() + "~" + mvars.strStartUpPath + @"\Parameter\Update\EDID\");    //[8]getfilelength
                                                                                                                                              //一旦呼叫執行緒物件的 Start 方法令它開始執行，其 IsAlive 屬性值就會等於 true，直到該執行緒的委派方法執行完畢，那條執行緒便隨之結束。
                                                                                                                                              //while (_th.IsAlive);
                                                                                                                                              //{
                                                                                                                                              //   Thread.Sleep(200);
                                                                                                                                              //}
                                                                                                                                              //更簡單的作法可以等待執行緒結束：呼叫 Thread 物件的 Join 方法。
                            _th.Join();

                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Update\EDID\" + "temp.txt"))     //斷點
                            {
                                File.Move(mvars.strStartUpPath + @"\Parameter\Update\EDID\" + "temp.txt", mvars.strStartUpPath + @"\Parameter\Update\EDID\" + svfilename);
                                sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "1";
                                mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                                sendBytes = Encoding.UTF8.GetBytes(sendData);
                                nsckC.Send(sendBytes);
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        //if (File.Exists(mvars.strStartUpPath + @"\Parameter\Update\EDID\" + "temp.txt"))     //斷點
                        //{
                        //    File.Move(mvars.strStartUpPath + @"\Parameter\Update\EDID\" + "temp.txt", mvars.strStartUpPath + @"\Parameter\Update\EDID\" + svfilename);
                        //    sendData = mvars.sckSindex + dgvota.Rows[0].Cells[10].Value.ToString() + "1";
                        //    mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        //    sendBytes = Encoding.UTF8.GetBytes(sendData);
                        //    nsckC.Send(sendBytes);
                        //}
                        ////mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        ////sendBytes = Encoding.UTF8.GetBytes(sendData);
                        ////nsckC.Send(sendBytes);
                    }
                }
                #endregion -------------------------- 收到 Server 的 Send xxxx file ------------ 建立...\Parameter\Update\MCU or FPGA or EDID


                #region ----------------------------- finish xxxx file --------------------------------
                if (svdata[1].Length > "finishMCUfile,".Length && svdata[1].Substring(0, "finishMCUfile,".Length) == "finishMCUfile,")
                {
                    if (dgvota.Rows[0].Cells[5].Value.ToString().Length > 1)
                    {
                        sendData = mvars.sckSindex + "@@MCUrequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                        string[] svf = new string[dgvota.Rows[0].Cells[5].Value.ToString().Split(char.Parse("\r")).Length];
                        if (dgvota.Rows[0].Cells[5].Value.ToString().IndexOf("\r", 0) == -1) svf[0] = dgvota.Rows[0].Cells[5].Value.ToString();
                        dgvota.Rows[0].Cells[5].Value = "";
                        //一次傳一個檔案名，直到lstfilestrMCU.items.counter==0
                        sendData += "~~" + svf[0];
                        mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                        if (svf.Length - 1 > 0)
                        {
                            for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[5].Value += svf[svm] + "\r";
                            if (dgvota.Rows[0].Cells[5].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[5].Value.ToString().Length - "\r".Length) != -1)
                                dgvota.Rows[0].Cells[5].Value = dgvota.Rows[0].Cells[5].Value.ToString().Substring(0, dgvota.Rows[0].Cells[5].Value.ToString().Length - "\r".Length);
                        }
                        mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);
                    }
                    else
                    {
                        if (dgvota.Rows[0].Cells[3].Value.ToString() != "0")
                        {
                            if (dgvota.Rows[0].Cells[3].Value.ToString().IndexOf(mvars.verFPGAm[0], 0) == -1 || dgvota.Rows[0].Cells[3].Value.ToString().IndexOf(mvars.verFPGAm[1], 0) == -1)
                            {
                                string[] svf = dgvota.Rows[0].Cells[3].Value.ToString().Split('+');
                                sendData = mvars.sckSindex + "@@FPGArequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                                dgvota.Rows[0].Cells[6].Value = "";
                                //一次傳一個檔案名，直到lstfilestrFPGA.items.counter==0
                                sendData += "~~" + svf[0];
                                mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                                if (svf.Length - 1 > 0)
                                {
                                    for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[6].Value += svf[svm] + "\r";
                                    if (dgvota.Rows[0].Cells[6].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[6].Value.ToString().Length - "\r".Length) != -1)
                                        dgvota.Rows[0].Cells[6].Value = dgvota.Rows[0].Cells[6].Value.ToString().Substring(0, dgvota.Rows[0].Cells[6].Value.ToString().Length - "\r".Length);
                                }
                                sendBytes = Encoding.UTF8.GetBytes(sendData);
                                nsckC.Send(sendBytes);
                            }
                        }
                        else if (dgvota.Rows[0].Cells[4].Value.ToString() != "0")
                        {
                            if (dgvota.Rows[0].Cells[4].Value.ToString().IndexOf(mvars.verEDID, 0) == -1 || dgvota.Rows[0].Cells[4].Value.ToString().IndexOf(".txt", 0) != -1)
                            {
                                string[] svf = dgvota.Rows[0].Cells[4].Value.ToString().Split('+');
                                sendData = mvars.sckSindex + "@@EDIDrequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                                dgvota.Rows[0].Cells[7].Value = "";
                                //一次傳一個檔案名，直到lstfilestrEDID.items.counter==0
                                sendData += "~~" + svf[0];
                                mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                                if (svf.Length - 1 > 0)
                                {
                                    for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[7].Value += svf[svm] + "\r";
                                    if (dgvota.Rows[0].Cells[7].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[7].Value.ToString().Length - "\r".Length) != -1)
                                        dgvota.Rows[0].Cells[7].Value = dgvota.Rows[0].Cells[7].Value.ToString().Substring(0, dgvota.Rows[0].Cells[7].Value.ToString().Length - "\r".Length);
                                }
                                sendBytes = Encoding.UTF8.GetBytes(sendData);
                                nsckC.Send(sendBytes);
                            }
                        }
                        else
                        {
                            sendData = mvars.sckSindex + "@@FileReceiveDone~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                            mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                            sendBytes = Encoding.UTF8.GetBytes(sendData);
                            nsckC.Send(sendBytes);
                            Thread.Sleep(1000);
                            nsckC.Shutdown(SocketShutdown.Both);
                            dte = DateTime.Now;
                            string svs = DateDiff(dts, dte).Split('H')[1];
                            svs = (60 * Convert.ToInt16(svs.Split('M')[0]) + Convert.ToInt16(svs.Split('M')[1])).ToString();
                            mp.showStatus1("", lstget1, "");
                            mp.showStatus1("File received " + svs + " sec", lstget1, "");
                            mp.showStatus1("Socket disconnected", lstget1, "");
                            mp.showStatus1("Firmware updating ..... ", lstget1, "");
                            if (mvars.FormShow[11] == false) tmeota.Enabled = false;


                            //僅測試傳遞檔案的時候可以關閉 cUpdate 但是要打開 延遲
                            if (mvars.demoMode) Thread.Sleep(2000);
                            else cUpdate(mvars.strStartUpPath + @"\Parameter\Update");

                            //sendData = mvars.sckSindex + "@@Updated~~" + mvars.verMCU + "~~" + mvars.verFPGAm[0] + "-" + mvars.verFPGAm[1] + "~~" + mvars.verEDID + "~~" + mvars.deviceName;
                            //mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                            //sendBytes = Encoding.UTF8.GetBytes(sendData);
                            //nsckC.Send(sendBytes);
                            //dte = DateTime.Now;
                            //string svs = DateDiff(dts, dte).Split('H')[1];
                            //svs = (60 * Convert.ToInt16(svs.Split('M')[0]) + Convert.ToInt16(svs.Split('M')[1])).ToString();
                            //mp.showStatus1("OTA Done ... " + svs + " sec", lstget1, "");

                            //Thread.Sleep(1000);
                            //nsckC.Shutdown(SocketShutdown.Both);

                            if (mvars.FormShow[11] == false)
                            {
                                dgvota[0, 1].Style.BackColor = Color.White;
                                dgvota.Rows[0].Cells[1].Value = "";
                                dgvota.Rows[0].Cells[8].Value = "0";
                                dgvota.Rows[0].Cells[9].Value = "0";
                                dgvota.Rows[0].Cells[10].Value = "";
                                dgvota.Visible = false;
                                dgvota.Visible = false;
                                hUser.Enabled = true;
                                hExit.Enabled = true;
                            }
                            //if (mvars.FormShow[6] == true) Form1.i3init.Close();
                        }
                    }
                }
                if (svdata[1].Length > "finishFPGAfile,".Length && svdata[1].Substring(0, "finishFPGAfile,".Length) == "finishFPGAfile,")
                {
                    if (dgvota.Rows[0].Cells[6].Value.ToString().Length > 1)
                    {
                        sendData = mvars.sckSindex + "@@FPGArequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                        string[] svf = dgvota.Rows[0].Cells[6].Value.ToString().Split(char.Parse("\r"));
                        if (dgvota.Rows[0].Cells[6].Value.ToString().IndexOf("\r", 0) == -1) svf[0] = dgvota.Rows[0].Cells[6].Value.ToString();
                        dgvota.Rows[0].Cells[6].Value = "";
                        //一次傳一個檔案名，直到lstfilestrFPGA.items.counter==0
                        sendData += "~~" + svf[0];
                        mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                        if (svf.Length - 1 > 0)
                        {
                            for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[6].Value += svf[svm] + "\r";
                            if (dgvota.Rows[0].Cells[6].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[6].Value.ToString().Length - "\r".Length) != -1)
                                dgvota.Rows[0].Cells[6].Value = dgvota.Rows[0].Cells[6].Value.ToString().Substring(0, dgvota.Rows[0].Cells[6].Value.ToString().Length - "\r".Length);
                        }
                        sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);
                    }
                    else
                    {
                        //EDID
                        if (dgvota.Rows[0].Cells[4].Value.ToString() != "0")
                        {
                            if (dgvota.Rows[0].Cells[4].Value.ToString().IndexOf(mvars.verEDID, 0) == -1 || dgvota.Rows[0].Cells[4].Value.ToString().IndexOf(".txt", 0) != -1)
                            {
                                string[] svf = dgvota.Rows[0].Cells[4].Value.ToString().Split('+');
                                sendData = mvars.sckSindex + "@@EDIDrequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                                dgvota.Rows[0].Cells[7].Value = "";
                                //一次傳一個檔案名，直到lst_filestr.items.counter==0
                                sendData += "~~" + svf[0];
                                mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                                if (svf.Length - 1 > 0)
                                {
                                    for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[7].Value += svf[svm] + "\r";
                                    if (dgvota.Rows[0].Cells[7].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[7].Value.ToString().Length - "\r".Length) != -1)
                                        dgvota.Rows[0].Cells[7].Value = dgvota.Rows[0].Cells[7].Value.ToString().Substring(0, dgvota.Rows[0].Cells[7].Value.ToString().Length - "\r".Length);
                                }
                                sendBytes = Encoding.UTF8.GetBytes(sendData);
                                nsckC.Send(sendBytes);
                            }
                        }
                        else
                        {
                            sendData = mvars.sckSindex + "@@FileReceiveDone~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                            mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                            sendBytes = Encoding.UTF8.GetBytes(sendData);
                            nsckC.Send(sendBytes);
                            Thread.Sleep(1000);
                            nsckC.Shutdown(SocketShutdown.Both);
                            dte = DateTime.Now;
                            string svs = DateDiff(dts, dte).Split('H')[1];
                            svs = (60 * Convert.ToInt16(svs.Split('M')[0]) + Convert.ToInt16(svs.Split('M')[1])).ToString();
                            mp.showStatus1("", lstget1, "");
                            mp.showStatus1("File received " + svs + " sec", lstget1, "");
                            mp.showStatus1("Socket disconnected", lstget1, "");
                            mp.showStatus1("Firmware updating ..... ", lstget1, "");
                            if (mvars.FormShow[11] == false) tmeota.Enabled = false;


                            //僅測試傳遞檔案的時候可以關閉 cUpdate 但是要打開 延遲
                            if (mvars.demoMode) Thread.Sleep(2000);
                            else cUpdate(mvars.strStartUpPath + @"\Parameter\Update");

                            //sendData = mvars.sckSindex + "@@Updated~~" + mvars.verMCU + "~~" + mvars.verFPGAm[0] + "-" + mvars.verFPGAm[1] + "~~" + mvars.verEDID + "~~" + mvars.deviceName;
                            //mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                            //sendBytes = Encoding.UTF8.GetBytes(sendData);
                            //nsckC.Send(sendBytes);
                            //dte = DateTime.Now;
                            //string svs = DateDiff(dts, dte).Split('H')[1];
                            //svs = (60 * Convert.ToInt16(svs.Split('M')[0]) + Convert.ToInt16(svs.Split('M')[1])).ToString();
                            //mp.showStatus1("OTA Done ... " + svs + " sec", lstget1, "");

                            //Thread.Sleep(1000);
                            //nsckC.Shutdown(SocketShutdown.Both);

                            if (mvars.FormShow[11] == false)
                            {
                                dgvota[0, 1].Style.BackColor = Color.White;
                                dgvota.Rows[0].Cells[1].Value = "";
                                dgvota.Rows[0].Cells[8].Value = "0";
                                dgvota.Rows[0].Cells[9].Value = "0";
                                dgvota.Rows[0].Cells[10].Value = "";
                                dgvota.Visible = false;
                                dgvota.Visible = false;
                                hUser.Enabled = true;
                                hExit.Enabled = true;
                            }
                            //if (mvars.FormShow[6] == true) Form1.i3init.Close();
                        }
                    }
                }
                if (svdata[1].Length > "finishEDIDfile,".Length && svdata[1].Substring(0, "finishEDIDfile,".Length) == "finishEDIDfile,")
                {
                    if (dgvota.Rows[0].Cells[7].Value.ToString().Length > 1)
                    {
                        sendData = mvars.sckSindex + "@@EDIDrequest~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                        string[] svf = dgvota.Rows[0].Cells[7].Value.ToString().Split(char.Parse("\r"));
                        if (dgvota.Rows[0].Cells[7].Value.ToString().IndexOf("\r", 0) == -1) svf[0] = dgvota.Rows[0].Cells[7].Value.ToString();
                        dgvota.Rows[0].Cells[7].Value = "";
                        //一次傳一個檔案名，直到lstfilestrFPGA.items.counter==0
                        sendData += "~~" + svf[0];
                        mp.showStatus1("(OUT)  " + sendData + ",file:" + svf[0], lstget1, "");
                        if (svf.Length - 1 > 0)
                        {
                            for (byte svm = 1; svm < svf.Length; svm++) dgvota.Rows[0].Cells[7].Value += svf[svm] + "\r";
                            if (dgvota.Rows[0].Cells[7].Value.ToString().IndexOf("\r", dgvota.Rows[0].Cells[7].Value.ToString().Length - "\r".Length) != -1)
                                dgvota.Rows[0].Cells[7].Value = dgvota.Rows[0].Cells[7].Value.ToString().Substring(0, dgvota.Rows[0].Cells[7].Value.ToString().Length - "\r".Length);
                        }
                        sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);
                    }
                    else
                    {
                        sendData = mvars.sckSindex + "@@FileReceiveDone~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                        mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);
                        Thread.Sleep(1000);
                        nsckC.Shutdown(SocketShutdown.Both);
                        dte = DateTime.Now;
                        string svs = DateDiff(dts, dte).Split('H')[1];
                        svs = (60 * Convert.ToInt16(svs.Split('M')[0]) + Convert.ToInt16(svs.Split('M')[1])).ToString();
                        mp.showStatus1("", lstget1, "");
                        mp.showStatus1("File received " + svs + " sec", lstget1, "");
                        mp.showStatus1("Socket disconnected", lstget1, "");
                        mp.showStatus1("Firmware updating ..... ", lstget1, "");
                        if (mvars.FormShow[11] == false) tmeota.Enabled = false;


                        //僅測試傳遞檔案的時候可以關閉 cUpdate 但是要打開 延遲
                        if (mvars.demoMode) Thread.Sleep(2000);
                        else cUpdate(mvars.strStartUpPath + @"\Parameter\Update");

                        //sendData = mvars.sckSindex + "@@Updated~~" + mvars.verMCU + "~~" + mvars.verFPGAm[0] + "-" + mvars.verFPGAm[1] + "~~" + mvars.verEDID + "~~" + mvars.deviceName;
                        //mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                        //sendBytes = Encoding.UTF8.GetBytes(sendData);
                        //nsckC.Send(sendBytes);
                        //dte = DateTime.Now;
                        //string svs = DateDiff(dts, dte).Split('H')[1];
                        //svs = (60 * Convert.ToInt16(svs.Split('M')[0]) + Convert.ToInt16(svs.Split('M')[1])).ToString();
                        //mp.showStatus1("OTA Done ... " + svs + " sec", lstget1, "");

                        //Thread.Sleep(1000);
                        //nsckC.Shutdown(SocketShutdown.Both);

                        if (mvars.FormShow[11] == false)
                        {
                            dgvota[0, 1].Style.BackColor = Color.White;
                            dgvota.Rows[0].Cells[1].Value = "";
                            dgvota.Rows[0].Cells[8].Value = "0";
                            dgvota.Rows[0].Cells[9].Value = "0";
                            dgvota.Rows[0].Cells[10].Value = "";
                            dgvota.Visible = false;
                            dgvota.Visible = false;
                            hUser.Enabled = true;
                            hExit.Enabled = true;
                        }
                        //if (mvars.FormShow[6] == true) Form1.i3init.Close();
                    }
                }

                #endregion ----------------------------- finish xxxx file --------------------------------

                if ((svdata[1].Length > "SendBinaryFail,".Length && svdata[1].Substring(0, "SendBinaryFail,".Length) == "SendBinaryFail,") ||
                    (svdata[1].Length > "SendTextFail,".Length && svdata[1].Substring(0, "SendTextFail,".Length) == "SendTextFail,"))
                {
                    //svstr = svdata[1].Split(',');
                    //mvars.flgAccept = false;

                    //sendData = mvars.sckSindex + "@@FileReceiveDone~~" + hostIP + "~~" + mvars.strNamePC + "~~" + Form1.serverPortnsckF;
                    //sck_Client.SendData(sendData);
                    //tme1.Enabled = false;
                    //lstget1.Items.Add(svstr[6]);
                    //txtserverPortnsckF.BackColor = Color.White;
                    //txt_serverPortnsckF.Text = "";
                    dgvota.Rows[0].Cells[1].Value = "";
                    //lbl_timer1.Text = "0";
                    //lbl_chc.Text = "0";
                    //lblgetfilelength.Text = "0";
                    //lbl_pocketsize.Text = "0";
                    //lblgetdatacount.Text = "0";
                    //lbl_counter.Text = "0";
                    //lbl_getmods.Text = "0";
                    dgvota.Rows[0].Cells[10].Value = "";
                }
            }
            else if (RX.Length >= "SERVERBUSY".Length && RX.IndexOf("SERVERBUSY", 0) != -1)
            {
                //MessageBox.Show(svdata[1]);
                mp.showStatus1("", lstget1, "");
                mp.showStatus1("(Waitting) Server Busy ---  |", lstget1, "");
                mp.showStatus1("", lstget1, "");
                //mp.showStatus1("", lstget1, "");
                //mp.showStatus1("", lstget1, "");

                sendData = data.Split('=')[1];
                //mp.showStatus1("(OUT)  " + sendData, lstget1, "");
                sendBytes = Encoding.UTF8.GetBytes(sendData);
                nsckC.Send(sendBytes);
            }
            Thread.Sleep(1000);
        }


        private static void SendMsgToServer(object inputText)
        {
            while (true)
            {
                if (inputText != null && inputText != "")
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(inputText.ToString());
                    nsckC.Send(buffer);
                }
            }
        }

        private static void ShowMsg(string s)
        {
            Console.WriteLine(s);
        }

        #endregion 利用Socket建立基礎連線







        public static void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (System.IO.File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    System.IO.File.Delete(d);//直接删除其中的文件   
                }
                else
                    DeleteFolder(d);//遞歸删除子文件夹   
            }
            Directory.Delete(dir);//删除已空文件夹   
        }

        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 檢查目標目錄是否以目錄分割字元結束如果不是則新增之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判斷目標目錄是否存在如果不存在則新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目錄的檔案列表，該裡面是包含檔案以及目錄路徑的一個數組
                // 如果你指向copy目標檔案下面的檔案而不包含目錄請使用下面的方法
                // string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                // 遍歷所有的檔案和目錄
                foreach (string file in fileList)
                {
                    // 先當作目錄處理如果存在這個目錄就遞迴Copy該目錄下面的檔案
                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    // 否則直接Copy檔案
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch
            {
                Console.WriteLine("無法複製!");
            }
        }

        

        private void InitialTray(string svname)
        {
            //notifyIcon = new NotifyIcon();
            ////設定通知欄提示的文字
            //notifyIcon.BalloonTipText = "Still running";
            ////設定通知欄在滑鼠移至Icon上的要顯示的文字
            //notifyIcon.Text = svname;
            ////決定一個Logo
            //notifyIcon.Icon = (System.Drawing.Icon)(Properties.Resources.aio);
            ////設定按下Icon發生的事件
            //notifyIcon.Click += (sender, e) =>
            //{
            //    //取消再通知欄顯示Icon
            //    notifyIcon.Visible = false;
            //    //顯示在工具列
            //    this.ShowInTaskbar = true;
            //    //顯示程式的視窗
            //    this.Show();
            //};


            notifyIcon = new NotifyIcon
            {
                //設定通知欄提示的文字
                BalloonTipText = "Still running",
                //設定通知欄在滑鼠移至Icon上的要顯示的文字
                Text = svname,
                //決定一個Logo
                Icon = (System.Drawing.Icon)(Properties.Resources.aio),
            };
            //設定按下Icon發生的事件
            notifyIcon.Click += (sender, e) =>
            {
                //取消再通知欄顯示Icon
                notifyIcon.Visible = false;
                //顯示在工具列
                this.ShowInTaskbar = true;
                //顯示程式的視窗
                this.Show();
            };
        }


        private void FPGAregister()
        {
            if (mvars.deviceID.Substring(0, 2) == "03")
            {
                #region H55
                mvars.FPGA_BK_SEL = 31;
                mvars.FPGA_OM_RD = 32;
                mvars.FPGA_OM_ADDR = 33;
                mvars.FPGA_OM_Wdata = 34;
                mvars.FPGA_OM_Rdata = 35;
                mvars.FPGA_GRAY_R = 48;
                mvars.FPGA_GRAY_G = 49;
                mvars.FPGA_GRAY_B = 50;
                #endregion H55
            }
            else if (mvars.deviceID.Substring(0, 2) == "06")
            {
                #region TV130
                mvars.FPGA_Flash_SW = 22;
                mvars.FPGA_Flash_SEL = 23;

                mvars.FPGA_XB_HPD = 25;

                mvars.FPGA_BK_SEL = 31;

                mvars.FPGA_DUTY_SEL = 70;
                mvars.FPGA_GOP_Ts = 71;
                mvars.FPGA_Gop_T2F = 72;
                mvars.FPGA_Gop_T2P = 73;
                mvars.FPGA_Gop_T2B = 74;
                mvars.FPGA_GOP_T3F1 = 75;
                mvars.FPGA_Gop_T3P = 76;
                mvars.FPGA_GOP_T3B1 = 77;
                mvars.FPGA_GOP_T3F2 = 78;
                mvars.FPGA_GOP_T3B2 = 79;
                mvars.FPGA_GOP_T4P = 80;
                mvars.FPGA_GOP_T4F = 81;
                
                mvars.FPGA_Flash_State = 200;
                mvars.FPGA_CBRd_State = 201;
                mvars.FPGA_UxRd_CNT = 202;
                mvars.FPGA_UxChk_NG = 203;
                mvars.FPGA_SYS_Stus0= 204;
                mvars.FPGA_SYS_Stus1 = 205;
                mvars.FPGA_LS_i2cNG = 206;
                #endregion TV130
            }

        }








        private void MyBackgroundTask()
        {
            for (int i = 0; i < 10000; i++)
            {
                //Console.Write("[" + Thread.CurrentThread.ManagedThreadId + "]");
                if (i % 4 == 0) { tslbl_Status.Text = "Net check and wait . . /"; }
                else if (i % 4 == 1) { tslbl_Status.Text = "Net check and wait . . |"; }
                else if (i % 4 == 2) { tslbl_Status.Text = "Net check and wait . . \\"; }
                else if (i % 4 == 3) { tslbl_Status.Text = "Net check and wait . . --"; }
            }
        }

        public static void nvsendercls_p(ref typNVSend[] svsender, byte svsenCnt, int svpoIndex)
        {
            Array.Resize(ref svsender, svsenCnt);
            for (int svsen = 0; svsen < svsenCnt; svsen++)
            {
                svsender[svsen].regBoxMark = new string[svpoIndex];
                svsender[svsen].regPoCards = 0;
                for (int svpo = 0; svpo < svpoIndex; svpo++)
                {
                    //for (int svca = 0; svca < 4; svca++)
                    //{
                    //    svsender[svsen].regBoxMark[svpo] = mvars.regbookmark;
                    //}
                    svsender[svsen].regBoxMark[svpo] = mvars.regbookmark;
                    svsender[svsen].regBoxMark[svpo] = mp.replaceBoxMark(svsender[0].regBoxMark[svpo], 21, mvars.in485.ToString());
                    svsender[svsen].regBoxMark[svpo] = mp.replaceBoxMark(svsender[0].regBoxMark[svpo], 22, mvars.inHDMI.ToString());
                }
            }
        }





        private void cmb_hPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.deviceID.Substring(0, 2) == "03" && mvars.verMCU != null)
            {
                if (mvars.verMCU.Substring(0, 4) == "OBB-" || mvars.verMCU.Substring(0, 4) == "ABB-" || mvars.verMCU.Substring(0, 4) == "BBB-" ||
                         mvars.verMCU.Substring(0, 4) == "OCB-" || mvars.verMCU.Substring(0, 4) == "ACB-" || mvars.verMCU.Substring(0, 4) == "BCB-")
                {
                    mvars.iPBaddr = Convert.ToByte(cmb_hPB.Text.Trim());
                    mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1);
                    if (cmbCM603.Text.Trim() == "R") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1); }
                    else if (cmbCM603.Text.Trim() == "G") { mvars.cm603WRaddr = 214; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                    else if (cmbCM603.Text.Trim() == "B") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                    mvars.deviceID = "03" + string.Format("{0:00}", (byte)((mvars.iPBaddr - 1) / 2 + 1));
                }
                else
                {
                    MessageBox.Show("H5512A(\"OBB/ABB/ACB/OCB/BBB/BCB\" MCU ver check error (read: " + mvars.verMCU + ")" + "\r\n" + "\r\n" +
                                     "Please check the Hardware", "H5512A");
                    this.Dispose();
                    this.Close();
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "02")
            {
                if (mvars.verMCU == "One-200408-T0004")
                {
                    switch (cmb_hPB.Text)
                    {
                        case " PB.A++": mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x70; mvars.SercomCmdWrRd = 0x71; break;
                        case " PB.B++": mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x72; mvars.SercomCmdWrRd = 0x73; break;
                        case " PB.C++": mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x74; mvars.SercomCmdWrRd = 0x75; break;
                        case " PB.D++": mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x76; mvars.SercomCmdWrRd = 0x77; break;
                        case " PB.E + -": mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x78; mvars.SercomCmdWrRd = 0x79; break;
                        case " PB.F + -": mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x7A; mvars.SercomCmdWrRd = 0x7B; break;
                        case " PB.G + -": mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x7C; mvars.SercomCmdWrRd = 0x7D; break;
                        case " PB.H + -": mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x7E; mvars.SercomCmdWrRd = 0x7F; break;
                        default: mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x70; mvars.SercomCmdWrRd = 0x71; break;
                    }
                }
                else
                {
                    switch (cmb_hPB.Text)
                    {
                        case " 2-1_1": mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                        case " 2-1_2": mvars.SercomCmdWr = 0x52; mvars.SercomCmdWrRd = 0x53; break;
                        case " 2-1_3": mvars.SercomCmdWr = 0x54; mvars.SercomCmdWrRd = 0x55; break;
                        case " 2-1_4": mvars.SercomCmdWr = 0x56; mvars.SercomCmdWrRd = 0x57; break;
                        case " 2-2_1": mvars.SercomCmdWr = 0x58; mvars.SercomCmdWrRd = 0x59; break;
                        case " 2-2_2": mvars.SercomCmdWr = 0x5A; mvars.SercomCmdWrRd = 0x5B; break;
                        case " 2-2_3": mvars.SercomCmdWr = 0x5C; mvars.SercomCmdWrRd = 0x5D; break;
                        case " 2-2_4": mvars.SercomCmdWr = 0x5E; mvars.SercomCmdWrRd = 0x5F; break;
                        case " 1-1_1": mvars.SercomCmdWr = 0x60; mvars.SercomCmdWrRd = 0x61; break;
                        case " 1-1_2": mvars.SercomCmdWr = 0x62; mvars.SercomCmdWrRd = 0x63; break;
                        case " 1-1_3": mvars.SercomCmdWr = 0x64; mvars.SercomCmdWrRd = 0x65; break;
                        case " 1-1_4": mvars.SercomCmdWr = 0x66; mvars.SercomCmdWrRd = 0x67; break;
                        case " 1-2_1": mvars.SercomCmdWr = 0x68; mvars.SercomCmdWrRd = 0x69; break;
                        case " 1-2_2": mvars.SercomCmdWr = 0x6A; mvars.SercomCmdWrRd = 0x6B; break;
                        case " 1-2_3": mvars.SercomCmdWr = 0x6C; mvars.SercomCmdWrRd = 0x6D; break;
                        case " 1-2_4": mvars.SercomCmdWr = 0x6E; mvars.SercomCmdWrRd = 0x6F; break;
                        default: mvars.SercomCmdWr = 0x60; mvars.SercomCmdWrRd = 0x61; break;
                    }
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "04")
            {
                switch (cmb_hPB.Text)
                {
                    case " 1-1_1": mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                    case " 1-1_2": mvars.SercomCmdWr = 0x52; mvars.SercomCmdWrRd = 0x53; break;
                    case " 1-1_3": mvars.SercomCmdWr = 0x54; mvars.SercomCmdWrRd = 0x55; break;
                    case " 1-1_4": mvars.SercomCmdWr = 0x56; mvars.SercomCmdWrRd = 0x57; break;
                    case " 1-2_1": mvars.SercomCmdWr = 0x58; mvars.SercomCmdWrRd = 0x59; break;
                    case " 1-2_2": mvars.SercomCmdWr = 0x5A; mvars.SercomCmdWrRd = 0x5B; break;
                    case " 1-2_3": mvars.SercomCmdWr = 0x5C; mvars.SercomCmdWrRd = 0x5D; break;
                    case " 1-2_4": mvars.SercomCmdWr = 0x5E; mvars.SercomCmdWrRd = 0x5F; break;
                    default: mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "05")
            {
                switch (cmb_hPB.Text)
                {
                    case " A": mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                    case " B": mvars.SercomCmdWr = 0x52; mvars.SercomCmdWrRd = 0x53; break;
                    case " C": mvars.SercomCmdWr = 0x54; mvars.SercomCmdWrRd = 0x55; break;
                    case " D": mvars.SercomCmdWr = 0x56; mvars.SercomCmdWrRd = 0x57; break;
                    case " E": mvars.SercomCmdWr = 0x58; mvars.SercomCmdWrRd = 0x59; break;
                    case " F": mvars.SercomCmdWr = 0x5A; mvars.SercomCmdWrRd = 0x5B; break;
                    case " G": mvars.SercomCmdWr = 0x5C; mvars.SercomCmdWrRd = 0x5D; break;
                    case " H": mvars.SercomCmdWr = 0x5E; mvars.SercomCmdWrRd = 0x5F; break;
                    default: mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                }
                mvars.nualg = true;
            }
            else if (mvars.deviceID.Substring(0, 2) == "06")
            {
                switch (cmb_hPB.Text)
                {
                    case " A": mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                    case " B": mvars.SercomCmdWr = 0x52; mvars.SercomCmdWrRd = 0x53; break;
                    case " C": mvars.SercomCmdWr = 0x54; mvars.SercomCmdWrRd = 0x55; break;
                    case " D": mvars.SercomCmdWr = 0x56; mvars.SercomCmdWrRd = 0x57; break;
                    case " E": mvars.SercomCmdWr = 0x58; mvars.SercomCmdWrRd = 0x59; break;
                    case " F": mvars.SercomCmdWr = 0x5A; mvars.SercomCmdWrRd = 0x5B; break;
                    case " G": mvars.SercomCmdWr = 0x5C; mvars.SercomCmdWrRd = 0x5D; break;
                    case " H": mvars.SercomCmdWr = 0x5E; mvars.SercomCmdWrRd = 0x5F; break;
                    case " I": mvars.SercomCmdWr = 0x60; mvars.SercomCmdWrRd = 0x61; break;
                    case " J": mvars.SercomCmdWr = 0x62; mvars.SercomCmdWrRd = 0x63; break;
                    case " K": mvars.SercomCmdWr = 0x64; mvars.SercomCmdWrRd = 0x65; break;
                    case " L": mvars.SercomCmdWr = 0x66; mvars.SercomCmdWrRd = 0x67; break;
                    default: mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                }
                mvars.iPBaddr = (byte)(cmb_hPB.SelectedIndex + 1);
                mvars.nualg = true;
            }
            else if (mvars.deviceID.Substring(0, 2) == "10")
            {
                switch (cmb_hPB.Text)
                {
                    case " 1": mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                    case " 2": mvars.SercomCmdWr = 0x52; mvars.SercomCmdWrRd = 0x53; break;
                    case " 3": mvars.SercomCmdWr = 0x54; mvars.SercomCmdWrRd = 0x55; break;
                    case " 4": mvars.SercomCmdWr = 0x56; mvars.SercomCmdWrRd = 0x57; break;
                    default: mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51; break;
                }
                mvars.nualg = true;
            }
        }



        private void cmb_CM603_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.deviceID.Substring(0, 2) == "03")
            {
                mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1);
                if (cmbCM603.Text.Trim() == "R") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1); }
                else if (cmbCM603.Text.Trim() == "G") { mvars.cm603WRaddr = 214; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                else if (cmbCM603.Text.Trim() == "B") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                else if (cmbCM603.Text.Trim() == "M") { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                mvars.deviceID = "03" + string.Format("{0:00}", (byte)((mvars.iPBaddr - 1) / 2 + 1));
            }
            else if (mvars.deviceID.Substring(0, 2) == "05" || 
                     mvars.deviceID.Substring(0, 2) == "06" ||
                     mvars.deviceID.Substring(0, 2) == "10")
            {
                if (cmbCM603.Text.Trim() == "R") { mvars.cm603WRaddr = 0xD4; }         //212
                else if (cmbCM603.Text.Trim() == "G") { mvars.cm603WRaddr = 0xD6; }    //214
                else if (cmbCM603.Text.Trim() == "B") { mvars.cm603WRaddr = 0xDA; }    //218
                else if (cmbCM603.Text.Trim() == "M") { mvars.cm603WRaddr = 0xD8; }    //216
            }
        }


        private void h_exit_Click(object sender, EventArgs e)
        {
            string[] svB = Form1.chkformsize.Text.Split(',');
            if (svB.Length >= 2)
                this.Size = new Size(Convert.ToInt16(Form1.chkformsize.Text.Split(',')[0]), Convert.ToInt16(Form1.chkformsize.Text.Split(',')[1]));
            if (svB.Length == 4)
                this.Location = new Point(Convert.ToInt16(Form1.chkformsize.Text.Split(',')[2]), Convert.ToInt16(Form1.chkformsize.Text.Split(',')[3]));
            if (mvars.actFunc == "" || mvars.actFunc == "Form1") { mvars.sp1.Close(); this.Close(); }
            else
            {
                #region restore singleID
                chkBC.Checked = false;
                cmbdeviceid.SelectedIndex = 0;
                #endregion restore singleID
                if (mvars.actFunc == "AutoGamma")
                {
                    #region ATG
                    if (mvars.deviceID == "0200")
                    {

                    }
                    else if (mvars.deviceID.Substring(0,2) == "03")
                    {
                        mp.markreset(99, false); mvars.flgSelf = true;
                        for (int i = 0; i < mvars.verMCUS.Length; i++)
                        {
                            mvars.deviceID = "03" + string.Format("{0:00}", i + 1);
                            Form1.tslbldeviceID.Text = mvars.deviceID;
                            Form1.pvindex = mvars.FPGA_DIP_SW;
                            int strData = 1;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(strData);
                            Form1.pvindex = mvars.FPGA_AL_CTRL;
                            strData = 32;
                            mvars.lblCmd = "FPGA_SPI_W";
                            mp.mhFPGASPIWRITE(strData);
                        }
                    }
                    mvars.FormShow[10] = false;
                    mvars.Break = true;
                    Form1.ucatg.Dispose();
                    #endregion ATG
                }
                else if (h_exit.Text.IndexOf(" Flash") > 0 || mvars.actFunc == "Flash") { mvars.FormShow[14] = false; Form1.ucflash.Dispose(); }
                else if (h_exit.Text.IndexOf(" Hex") > 0 || mvars.actFunc == "Hex") { mvars.FormShow[14] = false; Form1.ucflash.Dispose(); }
                else if (h_exit.Text.IndexOf(" MCU") > 0 || mvars.actFunc == "MCU") { mvars.FormShow[15] = false; Form1.ucmcu.Dispose(); }
                else if (h_exit.Text.IndexOf(" Demura") > 0 || mvars.actFunc == "dmr") { mvars.FormShow[7] = false; Form1.ucdmr.Dispose(); }
                else if (h_exit.Text.IndexOf(" FPGA") > 0 || mvars.actFunc == "FPGAreg") { mvars.FormShow[8] = false; Form1.ucFpgareg.Dispose(); }
                else if (mvars.actFunc == "cm603") { mvars.FormShow[10] = false; Form1.uc603.Dispose(); }
                //else if (h_exit.Text == "Exit IN525 bin") { Form1.ucin525.Dispose(); }
                else if (mvars.actFunc == "ca410") { mvars.FormShow[16] = false; Form1.ucca410.Dispose(); }
                else if (mvars.actFunc == "pictureadjust") 
                {
                    if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = "05" + Form1.cmbdeviceid.SelectedIndex.ToString("00");
                    mvars.FormShow[17] = false;
                    Form1.ucpicadj.Dispose();
                }
                else if (mvars.actFunc == "screenconfig") 
                {
                    this.ControlBox = true;
                    mvars.FormShow[11] = false;
                    Form1.hsend.Visible = false;
                    Form1.hsave.Visible = false;
                    Form1.hsingle.Visible = false;
                    Form1.tsspruser.Visible = false;
                    Form1.tsmnuuser.Visible = false;
                    Form1.tsmnulogout.Visible = false;
                    Form1.tsmnuota.Visible = false;
                    Form1.hEDIDud.Visible = mvars.flgsuperuser;                  
                    Form1.ucbox.Dispose();
                    Form1.lstget1.Items.Clear();
                }
                else if (mvars.actFunc == "more") { mvars.FormShow[3] = false; Form1.uccoding.Dispose(); this.Size = new Size(int.Parse(chk_formsize.Text.Split(',')[0]), int.Parse(chk_formsize.Text.Split(',')[1])); }
                else if (mvars.actFunc == "ota") 
                {
                    lst_get1.Location = new Point(14, 46);
                    lst_get1.Size = new Size(717, 80);
                    lst_get1.BackColor = Control.DefaultBackColor;
                    dgvformmsg.Visible = false; 
                }
                else if (h_pid.Visible == true) { h_pid.Enabled = true; }


                if (mvars.deviceID.Substring(0, 2) == "03") mvars.deviceID = "0300";
                else if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = "0500";
                else if (mvars.deviceID.Substring(0, 2) == "06") mvars.deviceID = "0600";

                mvars.actFunc = "";

                h_project.Enabled = true;
                hTool.Enabled = true;
                h_pid.Visible = chk_atgmode.Checked; h_pid.Enabled = true;
                h_user.Enabled = true;
                h_lan.Enabled = true;
                //h_exit.Text = "關閉(&E)";
                if (MultiLanguage.prelan == "en-US") h_exit.Text = "EXIT(&E)";
                else if (MultiLanguage.prelan == "zh-CN") h_exit.Text = "离开(&E)";
                else if (MultiLanguage.prelan == "zh-CHT") h_exit.Text = "關閉(&E)";
                else if (MultiLanguage.prelan == "ja-JP") h_exit.Text = "閉じる(&E)";

                h_pictureadjust.Visible = !chk_atgmode.Checked; h_pictureadjust.Enabled = true;
                h_screenconfig.Visible = !chk_atgmode.Checked; h_screenconfig.Enabled = true;

                ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
                string exatg = resource.GetString("ex");
                if (exatg != "" && exatg != null) h_exit.Text = exatg;

                mvars.Break = false;
                Form1.pnlfrm1.SendToBack();
                if (chk_formsize.Checked)
                    this.Size = new Size(Convert.ToInt16(chk_formsize.Text.Split(',')[0]), Convert.ToInt16(chk_formsize.Text.Split(',')[1]));
            }
        }


        

        
        #region Sendmessage FindWindow
        public struct COPYDATASTRUCT
        {
            public int cbData;
            public IntPtr dwData;
            public string lpData;
        }
        public struct COPYDATASTRUCTr
        {
            public IntPtr dwData; //可以是任意值
            public int cbData; //指定lpData内存区域的字节数
            [MarshalAs(UnmanagedType.LPStr)]    //using System.Runtime.InteropServices;
            public string lpData; //发送给目录窗口所在进程的数据
        }
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        public const int WM_COPYDATA = 0x004A;
        public static string WndProcStr;


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_COPYDATA)
            {
                COPYDATASTRUCTr cds = new COPYDATASTRUCTr();
                Type t = cds.GetType();
                cds = (COPYDATASTRUCTr)m.GetLParam(t);
                WndProcStr = cds.lpData;
                mvars.lstMsgIn.Items.Add(WndProcStr);
            }
            else { base.WndProc(ref m); }
        }

        
        #endregion Sendmessage FindWindow







        private void tslbl_HW_TextChanged(object sender, EventArgs e)
        {
            //if (tslblHW.Text == "232" || tslblHW.Text == "485" || tslblHW.Text == "USB") { tslblHW.BackColor = Color.Blue; tslblHW.ForeColor = Color.White; }
            //else { tslblHW.BackColor = Control.DefaultBackColor; tslblHW.ForeColor = Color.Black; }
        }





        private void tsmnu_H5512AATG_Click(object sender, EventArgs e)
        {

        }

        private void tsmnu_H5512AFPGAreg_Click(object sender, EventArgs e)
        {
            bool svdone = true;
            mvars.deviceID = "0300";
            if (mvars.demoMode) { Form1.tslblCOM.Text = "COM D"; }
            else
            {
                string SvIF;
                tslbl_HW.Text = "Interface"; tslbl_HW.BackColor = Control.DefaultBackColor; tslbl_HW.ForeColor = Color.Black;
                mp.cmdHW(255, 128, 128, out SvIF);
                if (SvIF == "232")
                {
                    //if (mvars.Comm.Any(s => Form1.tslblCOM.Text.Contains(s)) == false) { Form1.tslblCOM.ForeColor = Color.Red; return; }
                    mp.markreset(9, false); mvars.flgSelf = true; mvars.flgDelFB = false;
                    int i;
                    for (i = 0; i < mvars.Comm.Length; i++)
                    {
                        mp.Sp1open(mvars.Comm[i]);
                        mvars.lblCmd = "MCU_VERSION";
                        mp.mhVersion();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1) { Form1.tslblCOM.Text = mvars.Comm[i].ToString(); break; }
                        mp.CommClose();
                    }
                    if (i < mvars.Comm.Length)
                    {
                        mvars.lblCmd = "OBB_XBS";
                        mp.OBB_XBS();
                        if (mvars.verMCUS.Length < 99)
                        {
                            string svstr = "";
                            for (i = 0; i < mvars.verMCUS.Length; i++)
                            {
                                mvars.lblCmd = "MCU_VERSION";
                                mvars.deviceID = "03" + string.Format("{0:00}", (i + 1));
                                mp.mhVersion();
                                if (mvars.verMCUS[i] == "-1" || mvars.verMCUS[i] == "null") { break; }
                                svstr += ", " + (2 * i + 1).ToString() + ", " + (2 * i + 2).ToString();
                            }
                            if (svstr.Length < 3) { Form1.tslblStatus.Text = "Error，no X-Board"; svdone = false; }
                            else
                            {
                                svstr = svstr.Substring(1, svstr.Length - 1);
                                cmb_hPB.Items.Clear();
                                cmb_hPB.Items.AddRange(svstr.Split(','));
                                cmb_hPB.Text = cmb_hPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                                Form1.tslblStatus.Text = "XB amount " + mvars.verMCUS.Length;
                            }
                        }
                        else { Form1.tslblStatus.Text = "Error，XB amount feedback " + mvars.verMCUS.Length; svdone = false; }
                    }
                    else { Form1.tslblStatus.Text = "Error，Please check the BridgeBoard(0x0300)， no response"; }
                }
                else { Form1.tslblStatus.Text = "Error，No CommPort"; }

                //if (mvars.demoMode == false && mvars.sp1.IsOpen) mvars.sp1.Close();
                //mvars.lstget.Items.Add(Form1.tslblStatus.Text);
                //if (Form1.tslblStatus.Text.Substring(0, 5) == "Error") { return; }
                //mp.doDelayms(10);


                if (svdone)
                {
                    h_project.Enabled = !h_project.Enabled;
                    hTool.Enabled = !hTool.Enabled;
                    h_exit.Text = "Exit FPGA register";
                    this.Size = new Size(1200, 550);
                    pnl_frm1.BringToFront();
                    ucFpgareg = new uc_FPGAreg
                    {
                        Parent = pnl_frm1,
                        Dock = DockStyle.Fill
                    };
                }
            }
        }

        private void tsmnu_H5512Acm603_Click(object sender, EventArgs e)
        {
            bool svdone = true;
            mp.markreset(9, false);
            mvars.flgSelf = true;
            if (mp.Sp1open(Form1.tslblCOM.Text).IndexOf("Open", 0) > -1)
            {
                mvars.lblCmd = "MCU_VERSION";
                mp.mhVersion();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { tslblStatus.Text = "Error，disconnected with " + mvars.deviceName + "，pls reconnect"; svdone = false; }
                else
                {
                    mvars.lblCmd = "OBB_XBS";
                    mp.OBB_XBS();
                    if (mvars.verMCUS.Length < 99)
                    {
                        string svstr = "";
                        for (byte i = 0; i < mvars.verMCUS.Length; i++)
                        {
                            mvars.lblCmd = "MCU_VERSION";
                            mvars.deviceID = "03" + string.Format("{0:00}", (i + 1));
                            mp.mhVersion();
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { break; }
                            if (mvars.verMCUS[i] == "-1" || mvars.verMCUS[i] == "null") { break; }
                            svstr += ", " + (2 * i + 1).ToString() + ", " + (2 * i + 2).ToString();
                        }
                        if (svstr.Length < 3) { Form1.tslblStatus.Text = "Error，no X-Board"; svdone = false; }
                        else
                        {
                            svstr = svstr.Substring(1, svstr.Length - 1);
                            cmb_hPB.Items.Clear();
                            cmb_hPB.Items.AddRange(svstr.Split(','));
                            cmb_hPB.Text = cmb_hPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                            Form1.tslblStatus.Text = "XB amount " + mvars.verMCUS.Length;
                        }
                    }
                    else { Form1.tslblStatus.Text = "Error，XB amount feedback " + mvars.verMCUS.Length; svdone = false; }
                }
                if (svdone)
                {
                    h_project.Enabled = !h_project.Enabled;
                    hTool.Enabled = !hTool.Enabled;
                    h_exit.Text = "Exit CM603 bin";
                    //this.Size = new Size(1020, 430);
                    //pnl_frm1.BringToFront();
                    //ucbin = new uc_bin();
                    //ucbin.Parent = pnl_frm1;
                    //ucbin.Dock = DockStyle.Fill;

                    this.Size = new Size(880, 610);
                    pnl_frm1.BringToFront();
                    uc603 = new uc_cm603
                    {
                        Parent = pnl_frm1,
                        Dock = DockStyle.Fill
                    };
                }
            }
            else { tslblStatus.Text = "Error，lost commport " + tslblCOM.Text; }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (mvars.deviceName == "USB_DB" && Form1.tslblHW.Text == "USB") { SLUSBXpressDLL.SI_Close(SLUSBXpressDLL.hUSBDevice); }

            if (nsckC != null && nsckC.Connected)
            {
                sendData = "@@Quit~~" + hostIP;
                byte[] sendBytes = Encoding.UTF8.GetBytes(sendData);
                nsckC.Send(sendBytes);
            }

            mp.funSaveLogs("Unload");
        }






        



        



        #region 0709
        private void tsmnu_pid_MouseDown(object sender, MouseEventArgs e)
        {
            pvcmbpidclick = false;
            pvcmbpidkeycode = "";
            tsmnu_pid.Visible = false;
            tscmb_pid.Text = "";
            if (h_pid.Visible == true)
            {
                int index = tscmb_pid.FindString(h_pid.Text);
                tscmb_pid.SelectedIndex = index;
            }
            tscmb_pid.Visible = true;
            tscmb_pid.Focus();
            h_project.ShowDropDown();
        }

        private void tscmb_pid_Click(object sender, EventArgs e)
        {
            pvcmbpidclick = true;
            if (label6.Text == "Up" || label6.Text == "Down")
            {
                tscmb_pid.Visible = false;
                label4.Text = pvcmbpidclick.ToString();
                label5.Text = pvcmbpidindex.ToString();
                label6.Text = pvcmbpidkeycode.ToString();
                h_project.HideDropDown();
            }
            else
            {
                pvcmbpidindex = -1;
                label4.Text = pvcmbpidclick.ToString();
                label5.Text = pvcmbpidindex.ToString();
                label6.Text = pvcmbpidkeycode.ToString();
            }
        }

        private void tscmb_pid_SelectedIndexChanged(object sender, EventArgs e)
        {
            pvcmbpidindex = tscmb_pid.SelectedIndex;
            label4.Text = pvcmbpidclick.ToString();
            label5.Text = pvcmbpidindex.ToString();
            label6.Text = pvcmbpidkeycode.ToString();
        }

        private void tscmb_pid_KeyDown(object sender, KeyEventArgs e)
        {
            pvcmbpidkeycode = e.KeyCode.ToString();
            if (e.KeyCode == Keys.Enter)
            {
                tsmnu_pid.Visible = true;
                tscmb_pid.Visible = false;
                label4.Text = pvcmbpidclick.ToString();
                label5.Text = pvcmbpidindex.ToString();
                label6.Text = pvcmbpidkeycode.ToString();
                h_project.HideDropDown();
            }
            label6.Text = pvcmbpidkeycode.ToString();
        }

        private void tscmb_pid_DropDownClosed(object sender, EventArgs e)
        {
            if (tscmb_pid.SelectedIndex != -1 && pvcmbpidclick == true)
            {
                //直接點選項目
                pvcmbpidindex = tscmb_pid.SelectedIndex;

                label4.Text = pvcmbpidclick.ToString();
                label5.Text = pvcmbpidindex.ToString();
                label6.Text = pvcmbpidkeycode.ToString();

                h_project.HideDropDown();
            }
            else
            {
                tscmb_pid.Visible = false;
                tsmnu_pid.Visible = true;
            }
        }

        private void h_project_DropDownClosed(object sender, EventArgs e)
        {
            label4.Text = pvcmbpidclick.ToString();
            label5.Text = pvcmbpidindex.ToString();
            label6.Text = pvcmbpidkeycode.ToString();

            tscmb_pid.Visible = false; tsmnu_pid.Visible = true;
            if (pvcmbpidclick == false && pvcmbpidindex != -1 && pvcmbpidkeycode != Keys.Enter.ToString())
            {
                //只有捲動
                if (pvcmbpidkeycode == Keys.Up.ToString() || pvcmbpidkeycode == Keys.Down.ToString())
                {

                }
                pvcmbpidindex = -1;
                tscmb_pid.SelectedIndex = pvcmbpidindex;
            }
            else if (pvcmbpidclick == true && pvcmbpidindex == -1 && pvcmbpidkeycode == "")
            {
                //下拉後只點擊了空白
            }
            else
            {
                if ((pvcmbpidclick && pvcmbpidindex != -1) || (pvcmbpidindex != -1 && pvcmbpidkeycode == Keys.Enter.ToString()))
                {

                    int index = tscmb_pid.FindString(tscmb_pid.Text);
                    if (index != -1)
                    {
                        mvars.deviceID = mvars.deviceIDall[index];
                        mvars.deviceName = mvars.deviceAll[index];
                        h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;
                        mvars.nualg = true;

                        cmb_FPGAsel.Visible = true;

                        if (mvars.deviceID.Substring(0, 2) == "02") mvars.deviceNo = "0";
                        else if (mvars.deviceID.Substring(0, 2) == "03") { mvars.deviceNo = "1"; mvars.ICver = 0; mvars.nualg = false; }
                        else if (mvars.deviceID.Substring(0, 2) == "04") mvars.deviceNo = "2";
                        else if (mvars.deviceID.Substring(0, 2) == "05") 
                        { 
                            mvars.deviceNo = "4"; mvars.dualduty = 1; 
                            mvars.pGMA = new typGammaIC[2];

                            cmb_FPGAsel.Items.Clear();
                            string[] svf = new string[] { " ABCD", " EFGH", " ALL" };
                            if (MultiLanguage.DefaultLanguage == "en-US") { svf = new string[] { " 5678", " 1234", " ALL" }; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svf = new string[] { " 右畫面", " 左畫面", " 單屏" }; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svf = new string[] { " 右画面", " 左画面", " 单屏" }; }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svf = new string[] { " 右画面", " 左画面", " 全画面" }; }
                            cmb_FPGAsel.Items.AddRange(svf);
                            cmb_FPGAsel.Text = cmb_FPGAsel.Items[mvars.FPGAsel].ToString();
                        }
                        else if (mvars.deviceID.Substring(0, 2) == "06") { mvars.deviceNo = "5"; mvars.dualduty = 1; mvars.pGMA = new typGammaIC[2]; }
                        else if (mvars.deviceID.Substring(0, 2) == "10") { mvars.deviceNo = "6"; mvars.dualduty = 1; mvars.pGMA = new typGammaIC[2]; }

                        tslbl_deviceid.Text = mvars.deviceID;
                        #region 預設雷sir畢 (RM93C30_INX_V05)
                        mvars.TuningArea.Mark = "01";
                        mvars.TuningArea.mX = 0;
                        mvars.TuningArea.mY = 0;
                        mvars.TuningArea.tW = 1920;
                        mvars.TuningArea.tH = 1080;
                        mvars.TuningArea.Loading = 30;
                        mvars.TuningArea.rX = 1;
                        mvars.TuningArea.rY = 1;


                        //mvars.UUT.ICver = 0;        在 Form1.Form_Load 預設為0
                        mvars.UUT.Cx = 0.29f;
                        mvars.UUT.Cy = 0.31f;
                        mvars.UUT.CLv = 1050;
                        mvars.UUT.CAch = 00; mvars.UUT.OverBet = 1;
                        mCAs.CAATG.CAsel = 1; mvars.UIinTest = 0;
                        mvars.UUT.exGray = 220; mvars.UUT.GMAposATD = 0;
                        mvars.UUT.intSpace = 60;
                        mvars.UUT.gmafilepath = @"d:\PIDgma";
                        mvars.UUT.MTP = 1;
                        mvars.UUT.CoolCounts = 2;

                        mvars.iPBaddr = 1;
                        mvars.flgex20d10 = false;
                        mvars.UUT.ex20d10[0] = 3f;
                        mvars.UUT.ex20d10[1] = 2.5f;
                        mvars.UUT.ex20d10[2] = 7.5f;
                        mvars.UUT.ex20d10[3] = 1.6f;
                        #endregion
                    }
                    pvcmbpidclick = false;
                    pvcmbpidindex = -1;
                }
            }
            pvcmbpidkeycode = "";
        }
        #endregion 0709





        private void tsmnu_pidFPGAreg_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }
            #endregion parameter initial

            h_project.Enabled = !h_project.Enabled;
            //hTool.Enabled = !hTool.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 FPGA 暫存器(&E)";

            #region 多語系高級切換
            //string svs1 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs1 = "Exit FPGAreg (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs1 = "離開 FPGA 暫存器(&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs1 = "离开 FPGA 暂存器(&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs1 = "閉じる FPGAreg (&E)";
            //h_exit.Text = svs1;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("exfpga");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換


            this.Size = new Size(1050, 650);
            pnl_frm1.BringToFront();
            ucFpgareg = new uc_FPGAreg
            {
                Parent = pnl_frm1,
                Dock = DockStyle.Fill
            };
        }

        private void tsmnu_pidCM603_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (chk_formsize.Checked && Form1.tslblStatus.Text.ToUpper().IndexOf("ERR", 0) != -1 && mvars.demoMode == false) { return; }
            #endregion parameter initial

            if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma") == false && File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma") == false)
            {
                string svs1 = "";
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    svs1 = @"There is no defaultgamma file ""DefaultGamma_cm603.gma"" or ""DefaultGamma_cm603V.gma"" file in the ""Parameter"" folder";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    svs1 = @"在""Parameter""資料夾沒有 ""DefaultGamma_cm603.gma"" 或 ""DefaultGamma_cm603V.gma"" 檔案 ";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    svs1 = @"在""Parameter""资料夹没有 ""DefaultGamma_cm603.gma"" 或 ""DefaultGamma_cm603V.gma"" 档案 ";
                }
                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                {
                    svs1 = @"""Parameter""フォルダー内に ""DefaultGamma_cm603.gma"" 或 ""DefaultGamma_cm603V.gma"" はありません ";
                }
                MessageBox.Show(svs1);
                return;
            }
            else
            {
                string svs1 = "";
                if (mvars.ICver >= 5)
                {
                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma") == false)
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            svs1 = @"""ICver""" + mvars.ICver + @" need ""DefaultGamma_cm603V.gma"" file";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            svs1 = @"""ICver""" + mvars.ICver + @" 需要使用 ""DefaultGamma_cm603V.gma"" 檔案";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            svs1 = @"""ICver""" + mvars.ICver + @" 需要使用 ""DefaultGamma_cm603V.gma"" 档案";
                        }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        {
                            svs1 = @"""ICver""" + mvars.ICver + @" その ""DefaultGamma_cm603V.gma"" ファイルは必要です";
                        }
                        MessageBox.Show(svs1);
                        return;
                    }
                    else mvars.defaultGammafile = mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma";
                    mp.fileDefaultGammaV(false);
                    if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06") { mvars.mCM603P("0"); }
                    //else if (mvars.deviceID.Substring(0, 2) == "04" && mvars.deviceNameSub == "B(4)") { mvars.mCM603B4("0"); }        /// 暫時關閉
                    //else if (mvars.deviceID.Substring(0, 2) == "04" && mvars.deviceNameSub == "B(4t)") { mvars.mCM603B4t("0"); }      /// 暫時關閉
                    else if (mvars.deviceID.Substring(0, 2) == "10") { mvars.mCM603C("0"); }
                    else
                    {
                        MessageBox.Show("deviceID and cm603 default is not match", mvars.strUInameMe + "v" + mvars.UImajor);
                        return;
                    }
                    if (mp.fileDefaultGammaV(false) == false)
                    {
                        MessageBox.Show(mvars.defaultGammafile + " is read error", mvars.strUInameMe + "v" + mvars.UImajor);
                        return;
                    }
                }
                else
                {
                    /// 暫時關閉
                    //if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma") == false)
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US")
                    //    {
                    //        svs1 = @"""ICver""" + mvars.ICver + @" need ""DefaultGamma_cm603.gma"" file";
                    //    }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    //    {
                    //        svs1 = @"""ICver""" + mvars.ICver + @" 需要使用 ""DefaultGamma_cm603.gma"" 檔案";
                    //    }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    //    {
                    //        svs1 = @"""ICver""" + mvars.ICver + @" 需要使用 ""DefaultGamma_cm603.gma"" 档案";
                    //    }
                    //    MessageBox.Show(svs1);
                    //    return;
                    //}
                    //else mvars.defaultGammafile = mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma";
                    //mp.fileDefaultGammaAIO(false, mvars.defaultGammafile);
                    //mvars.mCM603("0");
                }
            }

            h_project.Enabled = !h_project.Enabled;
            //hTool.Enabled = !hTool.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 bin(&E)";

            #region 多語系高級切換
            //string svs1 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs1 = "Exit FPGAreg (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs1 = "離開 FPGA 暫存器(&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs1 = "离开 FPGA 暂存器(&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs1 = "閉じる FPGAreg (&E)";
            //h_exit.Text = svs1;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("excm603bin");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換



            mvars.BINtype = tsmnu_pidCM603.Text;
            
            this.Size = new Size(880, 610);
            pnl_frm1.BringToFront();
            uc603 = new uc_cm603
            {
                Parent = pnl_frm1,
                Dock = DockStyle.Fill
            };
        }
        private void tsmnu_pidIN525_Click(object sender, EventArgs e)
        {
            //mp.pidinit();
            //h_project.Enabled = !h_project.Enabled;
            //hTool.Enabled = !hTool.Enabled;
            //h_pid.Enabled = !h_pid.Enabled;
            //h_exit.Text = "Exit IN525 bin";

            //this.Size = new Size(880, 610);
            //pnl_frm1.BringToFront();
            //ucin525 = new uc_in525
            //{
            //    Parent = pnl_frm1,
            //    Dock = DockStyle.Fill
            //};
            //Form1.tslblStatus.Text = "";
        }



        private void tsmnu_pidATG_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            //mp.pidinit(); //由uc_atg_btn_PAGMA去執行這個流程

            mp.cmdHW(255, 128, 128, out string SvIF);
            if (SvIF == "232")
            {
                mp.markreset(999, false);

                if (Form1.tsmnucheck485.Checked) { SvIF = mvars.Comm[0]; mvars.Comm[0] = Form1.tslblCOM.Text; Form1.tsmnucheck485.Checked = false; }
                int i = 0;
                mvars.verMCU = "-1";
                //for (i = 0; i < mvars.Comm.Length; i++)
                //{
                mp.Sp1open(mvars.Comm[i]);
                mvars.lblCmd = "MCU_VERSION";
                mp.mhVersion();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                {
                    mvars.msgA = "(pidinit)  " + (i + 1) + "." + mvars.Comm[i] + " open MCU_VERSION DONE";
                    Form1.tslblCOM.Text = mvars.Comm[i];
                    //     break;
                }
                else
                {
                    mvars.msgA = "(pidinit)  " + (i + 1) + "." + mvars.Comm[i] + " open MCU_VERSION error";
                    mp.CommClose();
                }
                Form1.tslblStatus.Text = mvars.verMCU;
                //}
                mp.funSaveLogs(mvars.msgA); mvars.msgA = "";

                if (mvars.verMCU.IndexOf("-2.", 0) != -1)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") Form1.tslblStatus.Text = "Error,Select deviceID:" + mvars.deviceID + " <> read:" + mvars.verMCU.Split('.')[1];
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") Form1.tslblStatus.Text = "Error,硬體ID:" + mvars.deviceID + " 與回讀ID:" + mvars.verMCU.Split('.')[1] + " 不符合";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") Form1.tslblStatus.Text = "Error,硬体ID:" + mvars.deviceID + " 与回读ID:" + mvars.verMCU.Split('.')[1] + " 不符合";
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") Form1.tslblStatus.Text = "Error,Select deviceID:" + mvars.deviceID + " <> read:" + mvars.verMCU.Split('.')[1];
                    return;
                }



                Form1.cmbhPB.SelectedIndex = 0;
                Form1.cmbCM603.SelectedIndex = 0;
            }
            else
            {
                Form1.tslblCOM.Text = "";
                if (MultiLanguage.DefaultLanguage == "en-US") { Form1.tslblStatus.Text = @"No Any ""USB Serial Device"""; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { Form1.tslblStatus.Text = @"沒有任何 ""USB 序列裝置"""; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { Form1.tslblStatus.Text = @"沒有任何 ""USB 串行设备"""; }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { Form1.tslblStatus.Text = @"No Any ""USB Serial Device"""; }
            }



            if (chk_formsize.Checked && Form1.tslblStatus.Text.ToUpper().IndexOf("ERR", 0) != -1 && mvars.demoMode == false) { return; }
            #endregion parameter initial

            if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma") == false && File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma") == false)
            {
                string svs1 = "";
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    svs1 = @"There is no defaultgamma file ""DefaultGamma_cm603.gma"" or ""DefaultGamma_cm603V.gma"" file in the ""Parameter"" folder";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    svs1 = @"在""Parameter""資料夾沒有 ""DefaultGamma_cm603.gma"" 或 ""DefaultGamma_cm603V.gma"" 檔案 ";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    svs1 = @"在""Parameter""资料夹没有 ""DefaultGamma_cm603.gma"" 或 ""DefaultGamma_cm603V.gma"" 档案 ";
                }
                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                {
                    svs1 = @"在""Parameter""フォルダ ""DefaultGamma_cm603.gma"" また ""DefaultGamma_cm603V.gma""はないです ";
                }
                MessageBox.Show(svs1);
                return;
            }
            else
            {
                string svs1 = "";
                if (mvars.ICver >= 5)
                {
                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma") == false)
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            svs1 = @"""ICver""" + mvars.ICver + @" need ""DefaultGamma_cm603V.gma"" file";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            svs1 = @"""ICver""" + mvars.ICver + @" 需要使用 ""DefaultGamma_cm603V.gma"" 檔案";
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            svs1 = @"""ICver""" + mvars.ICver + @" 需要使用 ""DefaultGamma_cm603V.gma"" 档案";
                        }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        {
                            svs1 = @"""ICver""" + mvars.ICver + @" その ""DefaultGamma_cm603V.gma"" ファイルは必要です";
                        }
                        MessageBox.Show(svs1);
                        return;
                    }
                    else mvars.defaultGammafile = mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603V.gma";
                    if (mp.fileDefaultGammaV(false) == false)
                    {
                        MessageBox.Show(mvars.defaultGammafile + " is read error", mvars.strUInameMe + "v" + mvars.UImajor);
                        return;
                    }
                    //if (mvars.deviceID.Substring(0, 2) == "05") { mvars.mCM603P("0"); }
                    ////else if (mvars.deviceID.Substring(0, 2) == "04" && mvars.deviceNameSub == "B(4)") { mvars.mCM603B4("0"); }        /// 暫時關閉
                    ////else if (mvars.deviceID.Substring(0, 2) == "04" && mvars.deviceNameSub == "B(4t)") { mvars.mCM603B4t("0"); }      /// 暫時關閉
                    //else
                    //{
                    //    MessageBox.Show("deviceID and cm603 default is not match", mvars.strUInameMe + "v" + mvars.UImajor);
                    //    return;
                    //}                
                }
                else
                {
                    /// 暫時關閉
                    //if (File.Exists(mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma") == false)
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US")
                    //    {
                    //        svs1 = @"""ICver""" + mvars.ICver + @" need ""DefaultGamma_cm603.gma"" file";
                    //    }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    //    {
                    //        svs1 = @"""ICver""" + mvars.ICver + @" 需要使用 ""DefaultGamma_cm603.gma"" 檔案";
                    //    }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    //    {
                    //        svs1 = @"""ICver""" + mvars.ICver + @" 需要使用 ""DefaultGamma_cm603.gma"" 档案";
                    //    }
                    //    MessageBox.Show(svs1);
                    //    return;
                    //}
                    //else mvars.defaultGammafile = mvars.strStartUpPath + @"\Parameter\DefaultGamma_cm603.gma";
                    //mp.fileDefaultGammaAIO(false, mvars.defaultGammafile);
                    //mvars.mCM603("0");
                }
            }

            string svGammaSettingRead = "";
            mp.fileGammaSettingAIO(false, ref svGammaSettingRead);

            string[] svs = null;
            int svi;
            Thread t1 = null;

            if (mvars.UUT.gmafilepath.IndexOf(@"\\", 0) == 0 && mvars.UUT.gmafilepath.Length > 3 && mvars.UUT.gmafilepath.Substring(0, 2) == @"\\") //"\\\\"=\\
            {
                // type1 \\10.93.83.9\工廠單位資料傳遞\pixinLED\PID3\PanelID  
                // type2 \\hp01553p\archived
                mvars.msgA = mvars.UUT.gmafilepath.Substring(2, mvars.UUT.gmafilepath.Length - 2);
                svs = Regex.Split(mvars.msgA, @"\\", RegexOptions.IgnoreCase);
                if (svs.Length > 1) { mvars.msgA = @"\\" + svs[0]; }
            }
            else if ((mvars.UUT.gmafilepath.IndexOf(":\\", 0) > -1))
            {
                // d:\MyComputer\pixinLED\PID3\PanelID
                svs = Regex.Split(mvars.UUT.gmafilepath, @":\\", RegexOptions.IgnoreCase);
                if (svs.Length > 0) { mvars.msgA = svs[0].ToUpper() + ":"; }
            }
            else { svi = -1; goto Err; }
            //重組
            for (svi = 1; svi < svs.Length; svi++) { mvars.msgA = mvars.msgA + "\\" + svs[svi]; }
            //if (msgA.Substring(msgA.Length - 1, 1) != @"\") { msgA = msgA + @"\"; }
            //D:PIDgma
            if (Convert.ToInt32(Convert.ToChar(mvars.msgA.Substring(0, 1))) >= 65 && Convert.ToInt32(Convert.ToChar(mvars.msgA.Substring(0, 1))) <= 65 + 25)
            {
                if (mvars.UUT.Disk[Convert.ToInt32(Convert.ToChar(mvars.msgA.Substring(0, 1))) - 64, 0].IndexOf('F', 0) == -1) { svi = -2; goto Err; }
                if (Directory.Exists(mvars.msgA) == false) { Directory.CreateDirectory(mvars.msgA); }
                mvars.UUT.gmafilepath = mvars.msgA;
            }
            else if (mvars.msgA.Substring(0, 2) == @"//" || mvars.msgA.Substring(0, 2) == @"\\")
            {
                t1 = new Thread(MyBackgroundTask);
                t1.Start();
                mp.doDelayms(1000);
                Application.DoEvents();
                if (Directory.Exists(mvars.msgA) == false) { svi = -3; goto Err; }
            }

            if ((mvars.iPBaddr - 1) != cmb_hPB.SelectedIndex && mvars.UUT.GMAposATD == 0)
            {
                if (MessageBox.Show("AutoGamma position confirm" + "\r\n" + "\r\n" +
                                    "   GammaSetting_AIO.txt : " + cmb_hPB.Items[mvars.iPBaddr - 1] + "\r\n" +
                                    "   UI select :     " + cmb_hPB.Text + "\r\n" + "\r\n" + "是(Y)  change to " + cmb_hPB.Items[mvars.iPBaddr - 1] + "\r\n" +
                                                                                      "否(N)                  " + cmb_hPB.Text
                    , "INXPID v" + mvars.UImajor, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                { cmb_hPB.Text = cmb_hPB.Items[mvars.iPBaddr - 1].ToString(); }
                else
                {
                    mvars.iPBaddr = Convert.ToByte(cmb_hPB.SelectedIndex + 1);
                }
            }

            h_project.Enabled = !h_project.Enabled;
            hTool.Enabled = !hTool.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 AutoGamma(&E)";

            #region 多語系高級切換
            //string svs2 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs2 = "Exit AutoGamma (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs2 = "離開 AutoGamma (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs2 = "离开 AutoGamma (&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs2 = "閉じる AutoGamma (&E)";
            //h_exit.Text = svs2;

            //範例 form VAM Control v101
            //ComponentResourceManager resource = new ComponentResourceManager(typeof(UC_OneScreenInfo));
            //string mst = resource.GetString("mtitle");
            //string ms = resource.GetString("mdelsb");
            //if (MessageBox.Show(ms, mst, MessageBoxButtons.YesNo) != DialogResult.Yes)
            //{
            //    return;
            //}

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("exatg");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換

            this.Size = new Size(1200, 550);
            pnl_frm1.BringToFront();
            ucatg = new uc_atg
            {
                Parent = pnl_frm1,
                Dock = DockStyle.Fill
            };
            return;
        Err:
            if (svi >= 0) { MessageBox.Show("GammaSetting_AIO.txt file fail @ \"" + svs[svi] + "\""); }
            else
            {
                if (svi == -1) { MessageBox.Show("GammaSetting_AIO.txt file fail @ gmafilepath", "mainFilePath Error"); }
                else if (svi == -2) { MessageBox.Show("GammaSetting_AIO.txt file fail @ " + mvars.msgA + " Unknow", "mainFilePath Error"); }
                else if (svi == -3) { t1.Abort(); MessageBox.Show("gmafilepath not exist @ " + "\n\r" + mvars.msgA + " path not exist", "mainFilePath UnReachable"); }
            }
        }

        private void tsmnu_pidFlash_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }
            #endregion parameter initial

            h_project.Enabled = !h_project.Enabled;
            //hTool.Enabled = !hTool.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 Flash(&E)";

            #region 多語系高級切換
            //string svs2 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs2 = "Exit Flash (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs2 = "離開 Flash(&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs2 = "离开 Flash(&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs2 = "閉じる Flash (&E)";
            //h_exit.Text = svs2;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("exflash");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換

            this.Size = new Size(800, 650);
            mvars.actFunc = "Flash";
            pnl_frm1.BringToFront();
            ucflash = new uc_Flash
            {
                Parent = pnl_frm1,
                Dock = DockStyle.Fill
            };
        }

        private void tsmnu_Pattern_Click(object sender, EventArgs e)
        {
            frm2 = new Form2 { Visible = true };
        }


        private void tsmnu_demomode_Click(object sender, EventArgs e)
        {
            if (mvars.demoMode == false)
            {
                mvars.Break = true;
                mvars.demoMode = true;

                if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    //svs = "演示模式";
                    tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    //svs = "演示模式";
                    tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式";
                }
                else if (MultiLanguage.DefaultLanguage == "en-US") 
                { 
                    //svs = "Demo mode"; 
                    tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ Demo mode"; 
                }
                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                {
                    //svs = "Demo mode"; 
                    tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ デモモード";
                }
                tslblCOM.Text = "COM D";
            }
            else
            {
                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor;
                mvars.demoMode = false;
                this.Text = mvars.strUInameMe;
                if (mvars.Comm != null && mvars.Comm[0] != "") tslblCOM.Text = mvars.Comm[0]; else tslblCOM.Text = "";
            }
            tsmnu_demomode.Checked = mvars.demoMode;
        }

        private void cmb_deviceID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.deviceID == "0300" && mvars.verMCU != null)
            {
                if (mvars.verMCU.Substring(0, 4) == "OBB-" || mvars.verMCU.Substring(0, 4) == "ABB-" || mvars.verMCU.Substring(0, 4) == "BBB-" ||
                         mvars.verMCU.Substring(0, 4) == "OCB-" || mvars.verMCU.Substring(0, 4) == "ACB-" || mvars.verMCU.Substring(0, 4) == "BCB-")
                {
                    if (cmbdeviceid.Text.Trim().ToUpper().IndexOf("ALL", 0) > -1) { mvars.deviceID = "0310"; }
                    else { mvars.deviceID = cmbdeviceid.Text.Trim().Replace("XB", "03"); }
                    Form1.tslbldeviceID.Text = mvars.deviceID;
                }
                else
                {
                    MessageBox.Show("H5512A(\"OBB/ABB/ACB/OCB/BBB/BCB\" MCU ver check error (read: " + mvars.verMCU + ")" + "\r\n" + "\r\n" +
                                     "Please check the Hardware", "H5512A");

                }
            }
            else if (mvars.deviceID.Substring(0,2) == "05")
            {
                mvars.deviceID = cmbdeviceid.Text.Trim();
            }
        }

        





        private void tsmnu_pidauto_Click(object sender, EventArgs e)
        {

        }


        

        public void LoadAll(Form form)
        {
            if (form.Name.IndexOf("Form1", 0) > -1)
            {
                MultiLanguage.LoadLanguage(form, typeof(Form1));
                if (chk_formsize.Checked) { this.Size = new Size(Convert.ToInt16(chk_formsize.Text.Split(',')[0]), Convert.ToInt16(chk_formsize.Text.Split(',')[1])); }
                if (svatgcount == 11)
                {
                    h_project.Visible = chk_atgmode.Checked;
                    h_pid.Visible = chk_atgmode.Checked;
                    h_pictureadjust.Visible = !chk_atgmode.Checked;
                    h_screenconfig.Visible = !chk_atgmode.Checked;
                    tslbl_Pull.Visible = chk_atgmode.Checked;
                    tslbl_msgpull.Visible = chk_atgmode.Checked;
                    tslbl_RSIn.Visible = chk_atgmode.Checked;
                    tslbl_msgrsin.Visible = chk_atgmode.Checked;
                    tslbl_deviceid.Visible = chk_atgmode.Checked;
                    tslbl_msgdeviceid.Visible = chk_atgmode.Checked;
                    this.BringToFront();
                }
            }
            else if (form.Name == "Form2")
            {
                MultiLanguage.LoadLanguage(form, typeof(Form2));
            }
        }

        void lanclick()
        {
            //修改默認語言  
            MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
            //對所有打開的窗口重新加載語言 
            foreach (Form form in Application.OpenForms)
            {
                LoadAll(form);
            }
            //繼續語言修改
            #region 20230514 以下因為畫面上暫時不會顯示提示文字所以先取消(後續如果有提示文字則要進入更正)
            /*
            string svs = "";
            if (MultiLanguage.prelan == "en-US")
            {
                if (MultiLanguage.DefaultLanguage == "zh-CHT") tslbl_doc.Text = "   發行版本";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") tslbl_doc.Text = "   发行版本";
                else if (MultiLanguage.DefaultLanguage == "ja-JP") tslbl_doc.Text = "   正式版";

                for (int i = 0; i < lstget1.Items.Count; i++)
                {
                    svs = "";
                    if (lstget1.Items[i].ToString().Length >= 4)
                    {
                        svs = lstget1.Items[i].ToString();
                        if (lstget1.Items[i].ToString().Substring(0, 4) == "Demo")
                        {
                            if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            {
                                svs = "演示模式";
                                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式";
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                            {
                                svs = "演示模式";
                                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式";
                            }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP")
                            {
                                svs = "デモモード";
                                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ デモモード";
                            }
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                        else if (lstget1.Items[i].ToString().Substring(0, 4) == "Plea")
                        {
                            if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            {
                                if (lstget1.Items[i].ToString().IndexOf("check device", 0) != -1) svs = "請檢查硬體";
                                else if (lstget1.Items[i].ToString().IndexOf("Please Click ", 0) != -1) svs = @"請執行 ""系統 \ 硬體連接"" 或取消 ""登錄 \ 演示模式""";
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                            {
                                if (lstget1.Items[i].ToString().IndexOf("check device", 0) != -1) svs = "请检查硬件";
                                else if (lstget1.Items[i].ToString().IndexOf("Please Click ", 0) != -1) svs = @"请执行 ""系统 \ 硬件连接"" 或取消 ""登录 \ 演示模式""";
                            }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP")
                            {
                                if (lstget1.Items[i].ToString().IndexOf("check device", 0) != -1) svs = "ハードウェアを確認してください";
                                else if (lstget1.Items[i].ToString().IndexOf("Please Click ", 0) != -1) svs = @"""システム \ ハードウェア接続"" を実行するか ""ログイン \ デモモード""をキャンセルしてください";
                            }
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                    }
                }
            }
            else if (MultiLanguage.prelan == "zh-CHT")
            {
                if (MultiLanguage.DefaultLanguage == "en-US") tslbl_doc.Text = "   Release version";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") tslbl_doc.Text = "   发行版本";
                else if (MultiLanguage.DefaultLanguage == "ja-JP") tslbl_doc.Text = "   正式版";

                for (int i = 0; i < lstget1.Items.Count; i++)
                {
                    svs = "";
                    if (lstget1.Items[i].ToString().Length >= 4)
                    {
                        svs = lstget1.Items[i].ToString();
                        if (lstget1.Items[i].ToString().Substring(0, 4) == "演示模式")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US") { svs = "Demo mode"; tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ Demo mode"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = "演示模式"; tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式"; }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP")
                            {
                                svs = "デモモード";
                                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ デモモード";
                            }
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                        else if (lstget1.Items[i].ToString().Substring(0, 4) == "請取消 ")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US")
                            {
                                svs = @"Please cancel ""User \ demo""";
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                            {
                                svs = @"请取消 ""登录 \ 演示模式""";
                            }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP")
                            {
                                svs = @"""デモモード""をキャンセルしてください";
                            }
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                        else if (lstget1.Items[i].ToString().Substring(0, 4) == "請檢查硬")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US") svs = "Please check device";
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") svs = "请检查硬件";
                            else if (MultiLanguage.DefaultLanguage == "ja-JP") svs = "ハードウェアを確認してください";
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                    }
                }
            }
            else if (MultiLanguage.prelan == "zh-CN")
            {
                if (MultiLanguage.DefaultLanguage == "en-US") tslbl_doc.Text = "   Release version";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") tslbl_doc.Text = "   發行版本";
                else if (MultiLanguage.DefaultLanguage == "ja-JP") tslbl_doc.Text = "   正式版";

                for (int i = 0; i < lstget1.Items.Count; i++)
                {
                    svs = "";
                    if (lstget1.Items[i].ToString().Length >= 4)
                    {
                        svs = lstget1.Items[i].ToString();
                        if (lstget1.Items[i].ToString().Substring(0, 4) == "演示模式")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US") { svs = "Demo mode"; tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ Demo mode"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = "演示模式"; tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式"; }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP")
                            {
                                svs = "デモモード";
                                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ デモモード";
                            }
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                        else if (lstget1.Items[i].ToString().Substring(0, 4) == "请执行 ")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US")
                            {
                                svs = @"Please Click ""System \ Connect"" or disable ""User \ demo""";
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            {
                                svs = @"請執行 ""系統 \ 硬體連接"" 或取消 ""登錄 \ 演示模式""";
                            }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP")
                            {
                                svs = @"""システム \ ハードウェア接続"" を実行するか ""ログイン \ デモモード""をキャンセルしてください";
                            }
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                        else if (lstget1.Items[i].ToString().Substring(0, 4) == "请检查硬")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US") svs = "Please check device";
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") svs = "請檢查硬體";
                            else if (MultiLanguage.DefaultLanguage == "ja-JP") svs = "ハードウェアを確認してください";
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                    }
                }
            }
            else if (MultiLanguage.prelan == "ja-JP")
            {
                if (MultiLanguage.DefaultLanguage == "en-US") tslbl_doc.Text = "   Release version";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") tslbl_doc.Text = "   發行版本";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") tslbl_doc.Text = "   发行版本";

                for (int i = 0; i < lstget1.Items.Count; i++)
                {
                    svs = "";
                    if (lstget1.Items[i].ToString().Length >= 4)
                    {
                        svs = lstget1.Items[i].ToString();
                        if (lstget1.Items[i].ToString().Substring(0, "デモモード".Length) == "デモモード")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US") { svs = "Demo mode"; tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ Demo mode"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = "演示模式"; tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = "演示模式"; tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式"; }
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                        else if (lstget1.Items[i].ToString().Substring(0, "デモモード".Length) == "デモモード")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US")
                            {
                                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ Demo mode";
                                svs = @"Please cancel ""User \ demo""";
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                            {
                                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式";
                                svs = @"請取消 ""登錄 \ 演示模式""";
                            }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                            {
                                tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式";
                                svs = @"请取消 ""登录 \ 演示模式""";
                            }
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                        else if (lstget1.Items[i].ToString().Substring(0, "ハードウェア".Length) == "ハードウェア")
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US") svs = "Please check device";
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") svs = "請檢查硬體";
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") svs = "请检查硬件";
                            lstget1.Items.RemoveAt(i); lstget1.Items.Insert(i, svs);
                        }
                    }
                }
            }
            */
            #endregion
            MultiLanguage.prelan = MultiLanguage.DefaultLanguage;
        }

        private void tsmnu_cn_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmnu = (ToolStripMenuItem)sender;
            MultiLanguage.DefaultLanguage = tsmnu.Tag.ToString();
            lanclick();

            cmb_FPGAsel.Items.Clear();
            string[] svf = new string[] { " 5678", " 1234", " ALL" };
            if (tsmnu.Tag.ToString() == "en-US")
            {
                tsmnu_eng.Checked = true;
                tsmnu_cn.Checked = false;
                tsmnu_cht.Checked = false;
                tsmnu_jp.Checked = false;
                tslbl_Pull.Text = "Pull";
                tslbl_RSIn.Text = "RSIn";
                tslbl_HW.Text = "Interface";
                tslbl_deviceid.Text = "deviceID";
                tslbl_Status.Text = "Status";
                //tslbl_about.Text = "FamilyMart 1*3 screens splicing";
                if (mvars.demoMode) tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ Demo mode";
                else tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor;
                if (mvars.deviceName.ToUpper() == "PRIMARY" && mvars.deviceID.Substring(0, 2) == "05") svf = new string[] { " 5678", " 1234", " ALL" };
            }
            else if (tsmnu.Tag.ToString() == "zh-CHT")
            {
                tsmnu_eng.Checked = false;
                tsmnu_cn.Checked = false;
                tsmnu_cht.Checked = true;
                tsmnu_jp.Checked = false;
                tslbl_Pull.Text = "接收";
                tslbl_RSIn.Text = "回傳";
                tslbl_HW.Text = "介面";
                tslbl_deviceid.Text = "裝置";
                tslbl_Status.Text = "狀態";
                //tslbl_about.Text = "FamilyMart 1x3 拼接屏組合用";
                if (mvars.demoMode) tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式";
                else tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor;
                if (mvars.deviceName.ToUpper() == "PRIMARY" && mvars.deviceID.Substring(0, 2) == "05") svf = new string[] { " 右畫面", " 左畫面", " 單屏" };
            }
            else if (tsmnu.Tag.ToString() == "zh-CN")
            {
                tsmnu_eng.Checked = false;
                tsmnu_cn.Checked = true;
                tsmnu_cht.Checked = false;
                tsmnu_jp.Checked = false;
                tslbl_Pull.Text = "接收";
                tslbl_RSIn.Text = "回传";
                tslbl_HW.Text = "介面";
                tslbl_deviceid.Text = "装置";
                tslbl_Status.Text = "状态";
                //tslbl_about.Text = "FamilyMart 1x3拼接屏组合用";
                if (mvars.demoMode) tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ 演示模式";
                else tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor;
                if (mvars.deviceName.ToUpper() == "PRIMARY" && mvars.deviceID.Substring(0, 2) == "05") svf = new string[] { " 右画面", " 左画面", " 单屏" };
            }
            else if (tsmnu.Tag.ToString() == "ja-JP")
            {
                tsmnu_eng.Checked = false;
                tsmnu_cn.Checked = false;
                tsmnu_cht.Checked = false;
                tsmnu_jp.Checked = true;
                tslbl_Pull.Text = "買収";
                tslbl_RSIn.Text = "戻る";
                tslbl_HW.Text = "インターフェース";
                tslbl_deviceid.Text = "デバイス";
                tslbl_Status.Text = "スターテス";
                //tslbl_about.Text = "FamilyMart 1x3 屏組み合わせ";
                if (mvars.demoMode) tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor + " @ デモモード";
                else tslbl_Time.Text = DateTime.Now.ToShortDateString() + " v" + mvars.UImajor;
                if (mvars.deviceName.ToUpper() == "PRIMARY" && mvars.deviceID.Substring(0, 2) == "05") svf = new string[] { " 右画面", " 左画面", " 全画面" };
            }
            cmb_FPGAsel.Items.AddRange(svf);
            cmb_FPGAsel.Text = cmb_FPGAsel.Items[mvars.FPGAsel].ToString();
        }

        private void tsmnu_pidMCU_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }
            #endregion parameter initial

            h_project.Enabled = !h_project.Enabled;
            //hTool.Enabled = !hTool.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 MCU(&E)";

            #region 多語系高級切換
            //string svs2 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs2 = "Exit MCU (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs2 = "離開 MCU (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs2 = "离开 MCU (&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs2 = "閉じる MCU (&E)";
            //h_exit.Text = svs2;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("exmcu");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換

            this.Size = new Size(1050, 650);
            pnl_frm1.BringToFront();
            ucmcu = new uc_MCU
            {
                Parent = pnl_frm1,
                Dock = DockStyle.Fill
            };
        }



        #region sendmessage

        private void tme_Pull_Tick(object sender, EventArgs e)
        {
            string SvMsgIn;
            bool SvMsgInErr;
            int Svi;

            tslbl_RSIn.ForeColor = Color.Red;
            tslbl_Pull.ForeColor = Color.Green;
            if (mvars.lstMsgIn.Items.Count > 0 && tme_Pull.Enabled == true)
            {
                tme_Pull.Enabled = false; tslbl_Pull.ForeColor = Color.Red;
                SvMsgIn = mvars.lstMsgIn.Items[0].ToString();  //放到變數後移除已開始執行的任務
                mp.funSaveLogs("(R) " + SvMsgIn);
                mvars.lstMsgIn.Items.RemoveAt(0);

                if (SvMsgIn.IndexOf("QUIT", 0) != -1 || SvMsgIn.IndexOf("quit", 0) != -1)
                {
                    mp.funSaveLogs("(Q) P");
                    this.Close();
                    Environment.Exit(Environment.ExitCode);
                }
                else if (SvMsgIn.ToUpper().IndexOf("OTAERR", 0) != -1)
                {
                    #region OTA 錯誤回饋
                    mvars.strData = SvMsgIn.Split('~');
                    mp.showStatus1(mvars.strData[1], lst_get1, "");
                    tmeota.Enabled = false;
                    tsmnu_ota.Visible = true;
                    h_exit.Enabled = true;
                    #endregion OTA 錯誤回饋
                }
                else
                {
                    mvars.lblCompose = "";
                    SvMsgInErr = true;
                    mvars.handleIDfrom = -1;
                    byte[] sarr = System.Text.Encoding.Default.GetBytes("DONE");
                    COPYDATASTRUCT cds;
                    cds.dwData = (IntPtr)2500;
                    cds.cbData = sarr.Length;
                    //
                    //Call funSaveLog("(D) " & inprocessCmdHd & "(R) " & inProcessCmd)
                    //
                    if (SvMsgIn.IndexOf("@@", 0) != -1)
                    {
                        if (mvars.strData != null) Array.Clear(mvars.strData, 0, mvars.strData.Length);
                        mvars.strData = SvMsgIn.Split('@');
                        mvars.handleIDfrom = Convert.ToInt32(mvars.strData[0]);
                        Svi = SvMsgIn.IndexOf("@@", 0) + ("@@").Length;
                        SvMsgIn = SvMsgIn.Substring(Svi, SvMsgIn.Length - Svi);
                        if (mvars.strData != null) Array.Clear(mvars.strData, 0, mvars.strData.Length);
                        mvars.strData = SvMsgIn.Split(',');
                        if (mvars.strData.Length > 1 && mvars.handleIDfrom != -1 && mvars.handleIDfrom != 0)
                        {
                            if (mvars.strData[0].IndexOf(mvars.strUInameMe, 0) != -1)
                            {
                                mvars.flgSelf = false;      // 由pull進入的動作都不是由按鍵操作的
                                Form1.lstget1.Items.Clear(); Form1.lstget1.Refresh();
                                mvars.lblCompose = mvars.strData[1];
                                mvars.nvBoardcast = false;  //預設非廣播
                                if (mvars.strData.Length == 2)
                                {
                                    /// strData[0]:AIO，strData[1]:command，單帶 command
                                    if (mvars.strData[1] == "HIDE")
                                    {
                                        #region UI HIDE
                                        SvMsgInErr = false;
                                        this.ShowInTaskbar = true;
                                        this.Hide();
                                        notifyIcon.Visible = true;
                                        notifyIcon.ShowBalloonTip(1000, mvars.strUInameMe, "v" + mvars.UImajor, ToolTipIcon.Info);
                                        cds.lpData = mvars.handleIDMe + "@@" + mvars.strUInameMe + "," + mvars.strData[1] + ",DONE";
                                        mp.funSaveLogs("(S) " + cds.lpData);
                                        SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                        tme_Pull.Enabled = true;
                                        #endregion UI HIDE
                                    }
                                    else if (mvars.strData[1] == "SLIENTHIDE")
                                    {
                                        #region UI SLIENTHIDE
                                        SvMsgInErr = false;
                                        this.ShowInTaskbar = true;
                                        this.Hide();
                                        notifyIcon.Visible = true;
                                        notifyIcon.ShowBalloonTip(10, mvars.strUInameMe, "v" + mvars.UImajor, ToolTipIcon.Info);
                                        cds.lpData = mvars.handleIDMe + "@@" + mvars.strUInameMe + "," + mvars.strData[1] + ",DONE";
                                        mp.funSaveLogs("(S) " + cds.lpData);
                                        SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                        tme_Pull.Enabled = true;
                                        #endregion UI SLIENTHIDE
                                    }
                                    else if (mvars.strData[1] == "UNHIDE")
                                    {
                                        #region UI UNHIDE(Use lst_get1)
                                        SvMsgInErr = false;
                                        
                                        this.Show();
                                        lst_get1.Items.Clear();
                                        this.tslbl_Status.Text = "";
                                        cds.lpData = mvars.handleIDMe + "@@" + mvars.strUInameMe + "," + mvars.strData[1] + ",DONE";
                                        mp.funSaveLogs("(S) " + cds.lpData);
                                        SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                        tme_Pull.Enabled = true;
                                        #endregion UI UNHIDE
                                    }
                                    else if (mvars.strData[1] == "PIDINIT")
                                    {
                                        #region PIDINIT  upgraded + Primary 單箱體
                                        if (mvars.strData.Length == 2)
                                        {
                                            int hWnd = FindWindow(null, @"MarsServerProvider");
                                            if (hWnd != 0)
                                            {
                                                mp.Taskkill("MarsServerProvider*");
                                                hWnd = FindWindow(null, @"MonitorSite V2.6");
                                                if (hWnd != 0) mp.Taskkill("MonitorSite V2.6");
                                                SvMsgInErr = false;
                                                cds.lpData = mvars.handleIDMe + "@@ERROR," + mvars.strUInameMe + "," + mvars.strData[1] + ",MarsServerProvider is exist, please remove NovaLCT software";
                                                mp.funSaveLogs("(S) " + cds.lpData);
                                                SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                tme_Pull.Enabled = true;
                                            }
                                            else
                                            {
                                                mvars.Break = false;
                                                mvars.demoMode = false;
                                                //mvars._nCommPort = "COM xx";
                                                tsmnu_demomode.Checked = false;
                                                SvMsgInErr = false; mvars.lblCompose = mvars.strData[1];
                                                mp.pidinit();
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (mvars.strData[1] == "BOXREAD")
                                    {
                                        #region BOXREAD  Primary TV130 (v0037 modify cBOXREAD)
                                        SvMsgInErr = false;
                                        mp.cBOXREAD(Form1.lstget1);
                                        #endregion BOXREAD
                                    }
                                    else if (mvars.strData[1] == "VERSION")
                                    {
                                        #region VERSION 未啟用
                                        //mp.cVERSION(Convert.ToByte(mvars.strData[2]), Convert.ToByte(mvars.strData[3]), Convert.ToByte(mvars.strData[4]), Convert.ToByte(mvars.strData[5]));
                                        #endregion
                                    }
                                    else if (mvars.strData[1] == "MCU_0X64000CLS")
                                    {
                                        #region MCU_FLASHCLS 未啟用
                                        //if (mvars.demoMode)
                                        //{
                                        //    SvMsgInErr = false;
                                        //    Array.Resize(ref Form1.svuiregadr, mvars.uiregadr_default.Length);
                                        //    for (int svm = 0; svm < mvars.struiregadrdef.Split('~').Length; svm++)
                                        //    {
                                        //        Form1.svuiregadr[svm] = mvars.struiregadrdef.Split('~')[svm].Split(',')[1];
                                        //    }
                                        //    cds.lpData = mvars.handleIDMe + "@@DEMO MODE" + mvars.strUInameMe + "," + mvars.lblCompose + ",DONE";
                                        //    mp.funSaveLogs("(S) " + cds.lpData);
                                        //    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                        //    tme_Pull.Enabled = true;
                                        //}
                                        //else
                                        //{
                                        //    SvMsgInErr = false;
                                        //    mvars.nvBoardcast = true;
                                        //    Array.Resize(ref Form1.svuiregadr, mvars.uiregadr_default.Length);
                                        //    for (int svm = 0; svm < mvars.struiregadrdef.Split('~').Length; svm++)
                                        //    {
                                        //        Form1.svuiregadr[svm] = mvars.struiregadrdef.Split('~')[svm].Split(',')[1];
                                        //    }
                                        //    mp.cMCUFLASHCLS(0, 0, 0, 0);
                                        //}
                                        #endregion
                                    }
                                    else if (mvars.strData[1] == "MCU_0X64000CLS")
                                    {
                                        #region MCU_0x64000CLS 未啟用
                                        //if (mvars.nualg)
                                        //{
                                        //    #region svuiuser Default [0]~[39]
                                        //    uc_brightnessAdj.svuiuserDef[0] = "128";     //辨識用(read only)
                                        //    uc_brightnessAdj.svuiuserDef[1] = "3";       //demura版本    
                                        //    uc_brightnessAdj.svuiuserDef[2] = "1";       //User Gamma
                                        //    uc_brightnessAdj.svuiuserDef[3] = "1";       //User Brightness
                                        //    uc_brightnessAdj.svuiuserDef[4] = "0";       //WT PG mode   
                                        //    uc_brightnessAdj.svuiuserDef[5] = "0";       //單燈板
                                        //    uc_brightnessAdj.svuiuserDef[6] = "0";       //大屏demura
                                        //    uc_brightnessAdj.svuiuserDef[7] = "255";     //87碼
                                        //    uc_brightnessAdj.svuiuserDef[8] = "511";     //87碼
                                        //    uc_brightnessAdj.svuiuserDef[9] = "1";       //GAMUT開關
                                        //    uc_brightnessAdj.svuiuserDef[10] = "220";    //Gamma     (Read Only, 紀錄 userxy )     
                                        //    uc_brightnessAdj.svuiuserDef[11] = "512";    //Gamma 32
                                        //    uc_brightnessAdj.svuiuserDef[12] = "1024";   //Gamma 64   
                                        //    uc_brightnessAdj.svuiuserDef[13] = "2048";   //Gamma 128
                                        //    uc_brightnessAdj.svuiuserDef[14] = "4080";   //Gamma 255
                                        //    uc_brightnessAdj.svuiuserDef[15] = "1024";   //gRratio 
                                        //    uc_brightnessAdj.svuiuserDef[16] = "1024";   //gGratio
                                        //    uc_brightnessAdj.svuiuserDef[17] = "1024";   //gBratio
                                        //    uc_brightnessAdj.svuiuserDef[18] = "4080";   //White-tracking RED for PG mode
                                        //    uc_brightnessAdj.svuiuserDef[19] = "4080";   //White-tracking GREEN for PG mode
                                        //    uc_brightnessAdj.svuiuserDef[20] = "4080";   //White-tracking BLUE for PG mode
                                        //    uc_brightnessAdj.svuiuserDef[21] = "16384";
                                        //    uc_brightnessAdj.svuiuserDef[22] = "0";
                                        //    uc_brightnessAdj.svuiuserDef[23] = "0";
                                        //    uc_brightnessAdj.svuiuserDef[24] = "0";
                                        //    uc_brightnessAdj.svuiuserDef[25] = "16384";
                                        //    uc_brightnessAdj.svuiuserDef[26] = "0";
                                        //    uc_brightnessAdj.svuiuserDef[27] = "0";
                                        //    uc_brightnessAdj.svuiuserDef[28] = "0";
                                        //    uc_brightnessAdj.svuiuserDef[29] = "16384";
                                        //    uc_brightnessAdj.svuiuserDef[30] = "6850";   //pidxr     (Read Only, 紀錄userxy)
                                        //    uc_brightnessAdj.svuiuserDef[31] = "3146";   //pidyr     (Read Only, 紀錄userxy)
                                        //    uc_brightnessAdj.svuiuserDef[32] = "1944";   //pidxg     (Read Only, 紀錄userxy)
                                        //    uc_brightnessAdj.svuiuserDef[33] = "7481";   //pidyg     (Read Only, 紀錄userxy)
                                        //    uc_brightnessAdj.svuiuserDef[34] = "1240";   //pidxb     (Read Only, 紀錄userxy)
                                        //    uc_brightnessAdj.svuiuserDef[35] = "0723";   //pidyb     (Read Only, 紀錄userxy)
                                        //    uc_brightnessAdj.svuiuserDef[36] = "2906";   //pidxw     (Read Only, 紀錄userxy)
                                        //    uc_brightnessAdj.svuiuserDef[37] = "3113";   //pidyw     (Read Only, 紀錄userxy)
                                        //    uc_brightnessAdj.svuiuserDef[38] = "8835";   //pidcct    (Read Only, 紀錄userCCT)
                                        //    uc_brightnessAdj.svuiuserDef[39] = "0";      //GAMUT紀錄    (Read Only, 多工紀錄器)  
                                        //    /*
                                        //                                                0000000000000000 - (0)預設色域PID
                                        //                                                0000000000000001 - (1)色域PAL
                                        //                                                0000000000000010 - (2)色域NTSC

                                        //     */
                                        //    #endregion
                                        //}

                                        //if (mvars.demoMode)
                                        //{
                                        //    SvMsgInErr = false;
                                        //    Array.Resize(ref Form1.svuiregadr, mvars.uiregadr_default.Length);
                                        //    //Form1.svuiregadr = mvars.struiregadrdef.Split('~');
                                        //    for (int svm = 0; svm < mvars.struiregadrdef.Split('~').Length; svm++)
                                        //    {
                                        //        Form1.svuiregadr[svm] = mvars.struiregadrdef.Split('~')[svm].Split(',')[1];
                                        //    }
                                        //    cds.lpData = mvars.handleIDMe + "@@DEMO MODE" + mvars.strUInameMe + "," + mvars.lblCompose + ",DONE";
                                        //    mp.funSaveLogs("(S) " + cds.lpData);
                                        //    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                        //    tme_Pull.Enabled = true;
                                        //}
                                        //else
                                        //{
                                        //    SvMsgInErr = false;
                                        //    mvars.nvBoardcast = true;
                                        //    Array.Resize(ref Form1.svuiregadr, mvars.uiregadr_default.Length);
                                        //    for (int svm = 0; svm < mvars.struiregadrdef.Split('~').Length; svm++)
                                        //    {
                                        //        Form1.svuiregadr[svm] = mvars.struiregadrdef.Split('~')[svm].Split(',')[1];
                                        //    }
                                        //    mp.cMCUFLASHCLS(0, 0, 0, 0);
                                        //}
                                        #endregion
                                    }
                                    else if (mvars.strData[1] == "USERREAD")
                                    {
                                        #region USERREAD  未啟用
                                        //if (mvars.demoMode)
                                        //{
                                        //    SvMsgInErr = false;
                                        //    Array.Resize(ref Form1.svuiregadr, mvars.uiregadr_default.Length);
                                        //    //Form1.svuiregadr = mvars.struiregadrdef.Split('~');
                                        //    for (int svm = 0; svm < mvars.struiregadrdef.Split('~').Length; svm++)
                                        //    {
                                        //        Form1.svuiregadr[svm] = mvars.struiregadrdef.Split('~')[svm].Split(',')[1];
                                        //    }
                                        //    cds.lpData = mvars.handleIDMe + "@@DEMO MODE" + mvars.strUInameMe + "," + mvars.lblCompose + ",DONE";
                                        //    mp.funSaveLogs("(S) " + cds.lpData);
                                        //    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                        //    tme_Pull.Enabled = true;
                                        //}
                                        //else
                                        //{
                                        //    SvMsgInErr = false;
                                        //    mvars.nvBoardcast = false;
                                        //    mp.cUSERREAD();
                                        //}
                                        #endregion BOXREAD
                                    }
                                    else if (mvars.strData[1] == "OCP_OFF")
                                    {
                                        #region OCP_OFF Primary啟用 
                                        SvMsgInErr = false;
                                        if (mvars.deviceID.Substring(0, 2) == "05")
                                        {
                                            mvars.lblCmd = "OCP_OFF_" + mvars.deviceName.ToUpper();
                                            mp.cOCPONOFF(Form1.lstget1, false, false);
                                        }
                                        else
                                        {
                                            cds.lpData = mvars.handleIDMe + "@@" + mvars.strUInameMe + "," + mvars.strData[1] + ",model:" + mvars.deviceName + " not support,DONE";
                                            mp.funSaveLogs("(S) " + cds.lpData);
                                            SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                            tme_Pull.Enabled = true;
                                        }
                                        #endregion OCP_OFF
                                    }
                                    else if (mvars.strData[1] == "OCP_ON")
                                    {
                                        #region OCP_ON  Primary啟用 
                                        if (mvars.deviceID.Substring(0, 2) == "05")
                                        {
                                            mvars.lblCmd = "OCP_ON_" + mvars.deviceName.ToUpper();
                                            SvMsgInErr = false;
                                        }
                                        else
                                        {
                                            cds.lpData = mvars.handleIDMe + "@@" + mvars.strUInameMe + "," + mvars.strData[1] + ",model:" + mvars.deviceName + " not support,DONE";
                                            mp.funSaveLogs("(S) " + cds.lpData);
                                            SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                            tme_Pull.Enabled = true;
                                        }
                                        mp.cOCPONOFF(Form1.lstget1, true, true);
                                        #endregion OCP_ON
                                    }
                                    else if (mvars.strData[1] == "AUTOOCP")
                                    {
                                        #region AUTOOCP(Use lst_get1)  
                                        mvars.lblCmd = "AUTOOCP_PRIMARY";
                                        SvMsgInErr = false;
                                        mp.cAUTOOCP(null, lst_get1, 4, 6, 19, false, "No1");
                                        #endregion OCP_ON
                                    }
                                }
                                else if (mvars.strData.Length > 2)
                                {
                                    /// Primary 廣播ID 05A0，所以廣播指令格式 cmd,A0,,,,,
                                    /// strData[0]:AIO，strData[1]:command，strData[2]:A0，,,,
                                    for (int i = 2; i < mvars.strData.Length; i++) { mvars.lblCompose += "," + mvars.strData[i]; }
                                    if (mvars.demoMode)
                                    {
                                        #region Demomode
                                        SvMsgInErr = false;
                                        if (mvars.strData[1] == "COM_MODEL")
                                        {
                                            #region COM_MODEL upgraded + Primary 
                                            //mvars.GAMMA_SIZE = 2048;
                                            if (mvars.strData.Length == 3)
                                            {
                                                if (Convert.ToByte(mvars.strData[2]) < 0 || Convert.ToByte(mvars.strData[2]) > 4)
                                                {
                                                    cds.lpData = mvars.handleIDMe.ToString() + "@@ERROR," + mvars.strUInameMe + "," + mvars.lblCompose + ", index need between 0~3,(" + mvars.strData[2] + ")";
                                                    mp.funSaveLogs("(S) " + cds.lpData);
                                                    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                }
                                                else
                                                {
                                                    mvars.deviceID = "0200";
                                                    //mvars.GMAindex = Convert.ToByte(mvars.strData[2]);
                                                    //if (mvars.svnova == false) mvars.deviceID = mvars.deviceIDall[mvars.GMAindex];
                                                    //if (mvars.GMAindex == 2) { mvars.numcu = false; mvars.GAMMA_SIZE = 512; }
                                                    //mvars.GMAtype = mvars.deviceAll[mvars.GMAindex];
                                                    mvars.deviceNo = mvars.strData[2];
                                                    if (mvars.svnova == false) mvars.deviceID = mvars.deviceIDall[Convert.ToByte(mvars.deviceNo)];
                                                    if (mvars.deviceNo == "2") { mvars.numcu = false; mvars.GAMMA_SIZE = 512; }
                                                    mvars.deviceName = mvars.deviceAll[Convert.ToByte(mvars.deviceNo)];
                                                    cds.lpData = mvars.handleIDMe.ToString() + "@@DEMO MODE," + mvars.strUInameMe + "," + mvars.lblCompose + ",DONE," + mvars.deviceName + "," + mvars.deviceID;
                                                    mp.funSaveLogs("(S) " + cds.lpData);
                                                    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                }
                                            }
                                            #endregion COM_MODEL
                                        }
                                        else
                                        {
                                            cds.lpData = mvars.handleIDMe + "@@DEMO MODE," + mvars.strUInameMe + "," + mvars.lblCompose + ",DONE";
                                            mp.funSaveLogs("(S) " + cds.lpData);
                                            SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                        }
                                        tme_Pull.Enabled = true;
                                        #endregion Demomode
                                    }
                                    else
                                    {
                                        if (mvars.strData[1] == "COM_MODEL")
                                        {
                                            #region COM_MODEL Primary TV130 CrapStreamer 
                                            if (mvars.strData.Length == 3)
                                            {
                                                if (Convert.ToByte(mvars.strData[2]) < 0 || Convert.ToByte(mvars.strData[2]) > 20)
                                                {
                                                    SvMsgInErr = false;
                                                    cds.lpData = mvars.handleIDMe.ToString() + "@@ERROR," + mvars.strUInameMe + "," + mvars.lblCompose + ", index need between 0~3,(" + mvars.strData[2] + ")";
                                                    mp.funSaveLogs("(S) " + cds.lpData);
                                                    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                }
                                                else if (mvars.strData[2] == "4" || mvars.strData[2] == "5" || mvars.strData[2] == "6")
                                                {
                                                    SvMsgInErr = false;

                                                    mvars.deviceID = "0200";
                                                    //mvars.GMAindex = Convert.ToByte(mvars.strData[2]);
                                                    //if (mvars.svnova == false) mvars.deviceID = mvars.deviceIDall[mvars.GMAindex];
                                                    //if (mvars.GMAindex == 2) { mvars.numcu = false; mvars.GAMMA_SIZE = 512; }
                                                    //mvars.GMAtype = mvars.deviceAll[mvars.GMAindex];
                                                    //mvars.deviceID = mvars.deviceIDall[mvars.GMAindex];  //Ubi
                                                    
                                                    mvars.flgSendmessage = true;    //海康相機應對措施，程序中不關commport
                                                    
                                                    tscmb_pid.SelectedIndex = int.Parse(mvars.deviceNo);
                                                    tscmb_pid.Text = tscmb_pid.Items[tscmb_pid.SelectedIndex].ToString();
                                                    h_pid.Text = tscmb_pid.Text;
                                                    cmb_FPGAsel.Visible = true;

                                                    mvars.GMAvalue = 3.2f;

                                                    if (mvars.strData[2] == "4")
                                                    {
                                                        #region Primary , the same as Form1_Load
                                                        mvars.deviceName = "Primary"; mvars.deviceID = "0500"; mvars.deviceNo = "4";
                                                        string svstr = " A, B, C, D, E, F, G, H";
                                                        mvars.I2C_CMD = svstr.Split(',');
                                                        Form1.cmbhPB.Items.Clear();
                                                        Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                                        Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                                                        mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                                                        tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;

                                                        Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_p.Length);
                                                        mvars.uiregadr_default = mvars.uiregadr_default_p;
                                                        mvars.uiregadrdefault = "";
                                                        for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                                                        {
                                                            mvars.uiregadrdefault += "~" + mvars.uiregadr_default[j].Split(',')[0] + "," + mvars.uiregadr_default[j].Split(',')[2];
                                                            svuiregadr.RemoveAt(j);
                                                            svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                                                            svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8));   //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                                                            svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                                                        }
                                                        if (mvars.uiregadrdefault.Length > 1 && mvars.uiregadrdefault.Substring(0, 1) == "~") mvars.uiregadrdefault = mvars.uiregadrdefault.Substring(1, mvars.uiregadrdefault.Length - 1);

                                                        cmb_FPGAsel.Items.Clear();
                                                        string[] svf = new string[] { " ABCD", " EFGH", " ALL" };
                                                        if (MultiLanguage.DefaultLanguage == "en-US") { svf = new string[] { " 5678", " 1234", " ALL" }; }
                                                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svf = new string[] { " 右畫面", " 左畫面", " 單屏" }; }
                                                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svf = new string[] { " 右画面", " 左画面", " 单屏" }; }
                                                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svf = new string[] { " 右画面", " 左画面", " 全画面" }; }
                                                        cmb_FPGAsel.Items.AddRange(svf);
                                                        cmb_FPGAsel.Text = cmb_FPGAsel.Items[mvars.FPGAsel].ToString();

                                                        cmbdeviceid.Items.Clear();
                                                        for (int i = 1; i <= 16; i++) cmbdeviceid.Items.Add(" 05" + mp.DecToHex(i, 2));
                                                        cmbdeviceid.SelectedIndex = 0;
                                                        #endregion Primary
                                                    }
                                                    else if (mvars.strData[2] == "5")
                                                    {
                                                        #region TV130 , the same as Form1_Load
                                                        mvars.deviceName = "TV130"; mvars.deviceID = "0600"; mvars.deviceNo = "5";
                                                        string svstr = " 1, 2, 3, 4";
                                                        mvars.I2C_CMD = svstr.Split(',');
                                                        Form1.cmbhPB.Items.Clear();
                                                        Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                                        Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                                                        mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                                                        tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;

                                                        Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_tc.Length);
                                                        mvars.uiregadr_default = mvars.uiregadr_default_tc;
                                                        mvars.uiregadrdefault = "";
                                                        for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                                                        {
                                                            mvars.uiregadrdefault += "~" + mvars.uiregadr_default[j].Split(',')[0] + "," + mvars.uiregadr_default[j].Split(',')[2];
                                                            svuiregadr.RemoveAt(j);
                                                            svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                                                            svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8));   //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                                                            svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                                                        }
                                                        if (mvars.uiregadrdefault.Length > 1 && mvars.uiregadrdefault.Substring(0, 1) == "~") mvars.uiregadrdefault = mvars.uiregadrdefault.Substring(1, mvars.uiregadrdefault.Length - 1);

                                                        cmb_FPGAsel.Items.Clear();
                                                        cmb_FPGAsel.Visible = true;

                                                        cmbCM603.Items.Clear();
                                                        cmbCM603.Items.Add(" R");
                                                        cmbCM603.Items.Add(" G");
                                                        cmbCM603.Items.Add(" B");
                                                        #endregion TV130
                                                    }
                                                    else if (mvars.strData[2] == "6")
                                                    {
                                                        #region CarpStreamer , the same as Form1_Load
                                                        mvars.deviceName = "CarpStreamer"; mvars.deviceID = "1000"; mvars.deviceNo = "6";
                                                        mvars.GMAvalue = 3.2f;
                                                        string svstr = " 1, 2, 3, 4";
                                                        mvars.I2C_CMD = svstr.Split(',');
                                                        Form1.cmbhPB.Items.Clear();
                                                        Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                                        Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                                                        mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                                                        tscmb_pid.Text = tscmb_pid.Items[int.Parse(mvars.deviceNo)].ToString(); h_pid.Text = tscmb_pid.Text; h_pid.Visible = true;

                                                        Array.Resize(ref mvars.uiregadr_default, mvars.uiregadr_default_cb.Length);
                                                        mvars.uiregadr_default = mvars.uiregadr_default_cb;
                                                        mvars.uiregadrdefault = "";
                                                        for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                                                        {
                                                            mvars.uiregadrdefault += "~" + mvars.uiregadr_default[j].Split(',')[0] + "," + mvars.uiregadr_default[j].Split(',')[2];
                                                            svuiregadr.RemoveAt(j);
                                                            svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                                                            svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8));   //x032000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                                                            svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                                                        }
                                                        if (mvars.uiregadrdefault.Length > 1 && mvars.uiregadrdefault.Substring(0, 1) == "~") mvars.uiregadrdefault = mvars.uiregadrdefault.Substring(1, mvars.uiregadrdefault.Length - 1);

                                                        cmb_FPGAsel.Items.Clear();
                                                        cmb_FPGAsel.Visible = true;

                                                        cmbCM603.Items.Clear();
                                                        cmbCM603.Items.Add(" R");
                                                        cmbCM603.Items.Add(" G");
                                                        cmbCM603.Items.Add(" B");
                                                        #endregion CarpStreamer , the same as Form1_Load
                                                    }

                                                    if (mvars.svnova == false) mvars.deviceID = mvars.deviceIDall[Convert.ToByte(mvars.deviceNo)];
                                                    if (mvars.deviceNo == "2") { mvars.numcu = false; mvars.GAMMA_SIZE = 512; }
                                                    mvars.deviceName = mvars.deviceAll[Convert.ToByte(mvars.deviceNo)];
                                                    cds.lpData = mvars.handleIDMe.ToString() + "@@" + mvars.strUInameMe + "," + mvars.lblCompose + ",DONE," + mvars.deviceName + "," + mvars.deviceID;
                                                    mp.funSaveLogs("(S) " + cds.lpData);
                                                    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                }
                                                else
                                                {
                                                    SvMsgInErr = false;
                                                    cds.lpData = mvars.handleIDMe.ToString() + "@@ERROR," + mvars.strUInameMe + "," + mvars.lblCompose + ", unknow model ,(" + mvars.strData[2] + ")";
                                                    mp.funSaveLogs("(S) " + cds.lpData);
                                                    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                }
                                                tme_Pull.Enabled = true;
                                            }
                                            #endregion COM_MODEL
                                        }
                                        else if (mvars.strData[1] == "PGRGB_10BIT")
                                        {
                                            #region PGRGB_10BIT    TV130,Primary(v0030)啟用
                                            //mvars.nvBoardcast = false;          //前景與背景同灰階
                                            if ((mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 3) || (mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 6 + 1))
                                            {
                                                if (mvars.deviceID.Substring(0, 2) == "05")
                                                {
                                                    SvMsgInErr = false;
                                                    if (mvars.strData.Length == 3) { mvars.nvBoardcast = true; mp.cPGRGB10BITp(255, 0, 0, 0, 0, "-1", "-1", "-1"); }
                                                    else { mp.cPGRGB10BITp(255, Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[5]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[6]) - 1), "-1", "-1", "-1"); }
                                                }
                                                else if (mvars.deviceID.Substring(0, 2) == "06" && mvars.strData.Length == 3)
                                                {
                                                    SvMsgInErr = false;
                                                    mp.cPGRGB10BITt(0, 0, 0, 0xA0, 0, "-1", "-1", "-1", "0", "0", "0");
                                                }
                                            }
                                            else if ((mvars.strData[2].ToUpper() == "OFF" && mvars.strData.Length == 3) || (mvars.strData[2].ToUpper() == "OFF" && mvars.strData.Length == 6 + 1))
                                            {
                                                if (mvars.deviceID.Substring(0, 2) == "05")
                                                {
                                                    SvMsgInErr = false;
                                                    if (mvars.strData.Length == 3) { mvars.nvBoardcast = true; mp.cPGRGB10BITp(0, 0, 0, 0, 0, "-1", "-1", "-1"); }
                                                    else { mp.cPGRGB10BITp(0, Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[5]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[6]) - 1), "-1", "-1", "-1"); }
                                                }
                                                else if (mvars.deviceID.Substring(0, 2) == "06" && mvars.strData.Length == 3)
                                                {
                                                    SvMsgInErr = false;
                                                    mvars.nvBoardcast = true; mp.cPGRGB10BITt(0, 0, 0, 0xA0, 0, "-0", "-0", "-0", "0", "0", "0");
                                                }
                                            }

                                            else if ((mvars.strData.Length == 5 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4])) ||
                                                    (mvars.strData.Length == 9 && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]) && mp.IsNumeric(mvars.strData[7]) && mp.IsNumeric(mvars.strData[8])) ||
                                                    (mvars.strData.Length == 13 && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]) && mp.IsNumeric(mvars.strData[7]) && mp.IsNumeric(mvars.strData[8])))
                                            {
                                                //5:[2]=R,[3]=G,[4]=B   [1]lblcmd,[2]r,[3]g,[4]b
                                                //9:[6]=R,[7]=G,[8]=B   [1]lblcmd,[2]svip,[3]svsen,[4]svpo,[5]svca,[6]R,[7]G,[8]B   svca拿來做選擇哪一塊燈板
                                                SvMsgInErr = false;
                                                if (mvars.deviceID.Substring(0, 2) == "06")
                                                {
                                                    if (mvars.strData.Length == 5)
                                                        mp.cPGRGB10BITt(0, 0, 0, 0xA0, 0, mvars.strData[2], mvars.strData[3], mvars.strData[4],"0","0","0");
                                                        //mp.cPGRGB10BIT(2, 0, 0, 0, 0, mvars.strData[2], mvars.strData[3], mvars.strData[4]);
                                                    //else if (mvars.strData.Length == 9)
                                                    //    mp.cPGRGB10BIT(3, 0, 0, 0, Convert.ToByte(mvars.strData[5]), mvars.strData[6], mvars.strData[7], mvars.strData[8]);
                                                }
                                                else if (mvars.deviceID.Substring(0, 2) == "05")
                                                {
                                                    if (mvars.strData.Length == 5)
                                                        mp.cPGRGB10BITp(2, 0, 0, 0, 2, mvars.strData[2], mvars.strData[3], mvars.strData[4]);
                                                    else if (mvars.strData.Length == 9 && mvars.strData[2] == "0" && mvars.strData[3] == "0")
                                                        mp.cPGRGB10BITp(2, 0, 0, Convert.ToByte(mvars.strData[4]), Convert.ToByte(mvars.strData[5]), mvars.strData[6], mvars.strData[7], mvars.strData[8]);
                                                    else if (mvars.strData.Length == 9 && mvars.strData[2] == "0" && mvars.strData[3] != "0")
                                                        mp.cPGRGB10BITp(3, 0, Convert.ToByte(mvars.strData[3]), Convert.ToByte(mvars.strData[4]), 0, mvars.strData[6], mvars.strData[7], mvars.strData[8]);
                                                }
                                            }
                                            else if ((mvars.strData[2].ToUpper() == "FL" && mvars.strData.Length == 7) || (mvars.strData[2].ToUpper() == "FL" && mvars.strData.Length == 10 + 1 + 3))
                                            {
                                                SvMsgInErr = false;
                                                if (mvars.deviceID.Substring(0, 2) == "06")
                                                {
                                                    if (mvars.strData.Length == 7)      //廣播所有燈箱的指定燈板
                                                        mp.cPGRGB10BIT(0, 0, 0, 0, mvars.strData[3], mvars.strData[4], mvars.strData[5], mvars.strData[6], "-1", "-1", "-1");
                                                    else                                //指定燈箱與燈板
                                                        mp.cPGRGB10BITt(Convert.ToByte(mvars.strData[3]), Convert.ToByte(mvars.strData[4]), Convert.ToByte(mvars.strData[5]), Convert.ToByte(mvars.strData[6]), Convert.ToByte(mvars.strData[7]), mvars.strData[8].Trim(), mvars.strData[9].Trim(), mvars.strData[10].Trim(), mvars.strData[11].Trim(), mvars.strData[12].Trim(), mvars.strData[13].Trim());
                                                }
                                            }
                                            else if ((mvars.strData[2].ToUpper() == "FP" && mvars.strData.Length == 10) || (mvars.strData[2].ToUpper() == "FP" && mvars.strData.Length == 13 + 1))    //前景
                                            {
                                                SvMsgInErr = false;
                                                if (mvars.deviceID.Substring(0, 2) == "06")
                                                {
                                                    if (mvars.strData.Length == 10)
                                                        mp.cPGRGB10BIT(0, 0, 0, 0, mvars.strData[3], mvars.strData[4], mvars.strData[5], mvars.strData[6], mvars.strData[7], mvars.strData[8], mvars.strData[9]);
                                                    else
                                                        mp.cPGRGB10BIT(Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[5]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[6]) - 1), mvars.strData[7], mvars.strData[8], mvars.strData[9], mvars.strData[10], mvars.strData[11], mvars.strData[12], mvars.strData[13]);
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (mvars.strData[1] == "ID")
                                        {
                                            #region BOXREAD Primary TV130
                                            SvMsgInErr = false;
                                            if (mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 3)
                                                mp.cIDONOFF(Form1.lstget1, 1);
                                            else if (mvars.strData[2].ToUpper() == "OFF" && mvars.strData.Length == 3)
                                                mp.cIDONOFF(Form1.lstget1, 0);
                                            #endregion BOXREAD
                                        }
                                        else if (mvars.strData[1] == "DROP")
                                        {
                                            #region DROP  Primary TV130
                                            mvars.nvBoardcast = false;          //單燈板調製都是在非廣播下設定
                                            if (mvars.strData.Length == 7)
                                            {
                                                if (mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]))
                                                {
                                                    if (Convert.ToInt16(mvars.strData[2]) >= 0 && Convert.ToInt16(mvars.strData[2]) <= 255)
                                                    {
                                                        SvMsgInErr = false;
                                                        if (mvars.deviceID.Substring(0, 2) == "05") mp.cDROPp(Convert.ToByte(mvars.strData[2]), 0, 0, Convert.ToByte(mvars.strData[5]), Convert.ToByte(mvars.strData[6])); //單屏
                                                        else if (mvars.deviceID.Substring(0, 2) == "06") mp.cDROPt(Convert.ToByte(mvars.strData[2]), 0, 0, Convert.ToByte(mvars.strData[5]), Convert.ToByte(mvars.strData[6])); //單屏                                                                                                                                                          //
                                                    }
                                                }
                                            }
                                            else if (mvars.strData.Length == 3)
                                            {
                                                if (mvars.deviceID.Substring(0, 2) == "06")
                                                {
                                                    SvMsgInErr = false;
                                                    mp.cDROPt(Convert.ToByte(mvars.strData[2]), 0, 0, 0, 0xA0); 
                                                }
                                            }
                                            #endregion ENG_GMA
                                        }
                                        else if (mvars.strData[1] == "ENG_GMA")
                                        {
                                            #region ENG_GMA   
                                            mvars.nvBoardcast = false;          //單燈板調製都是在非廣播下設定(Convert.ToByte(mvars.strData[5])任意值，因為到副程式後會對所有的屏做ENG_GMA_EN的OFF動作
                                            if (mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 7)
                                            {
                                                if (mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]))
                                                {
                                                    if (mvars.deviceID.Substring(0, 2) == "05")
                                                    {
                                                        if (mvars.strData[4] == "160")
                                                        {
                                                            SvMsgInErr = false;
                                                            mp.cENGGMAONWRITEp(0, 0, 160, Convert.ToByte(mvars.strData[5]), Convert.ToByte(mvars.strData[6])); //單屏
                                                        }
                                                    }
                                                }
                                            }
                                            else if (mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 8)
                                            {
                                                if (mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]) && mp.IsNumeric(mvars.strData[7]))
                                                {
                                                    if (mvars.deviceID.Substring(0, 2) == "06")
                                                    {
                                                        SvMsgInErr = false;
                                                        mp.cENGGMAONWRITEt(0, 0, 0, 0, Convert.ToByte(mvars.strData[6]), Convert.ToByte(mvars.strData[7])); //單屏
                                                    }
                                                }
                                            }
                                            else if (mvars.strData[2].ToUpper() == "OFF")
                                            {
                                                if (mvars.strData.Length == 7)
                                                {
                                                    #region Primary OFF
                                                    if (mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]))
                                                    {
                                                        SvMsgInErr = false;
                                                        if (mvars.strData[4] == "160") mp.cENGGMAONWRITEp(3, 0, 160, Convert.ToByte(mvars.strData[5]), Convert.ToByte(mvars.strData[6])); //單屏
                                                    }
                                                    #endregion Primary OFF
                                                }
                                                else if (mvars.strData.Length == 9)
                                                {
                                                    #region TV130 OFF
                                                    if (mp.IsNumeric(mvars.strData[6]) && mp.IsNumeric(mvars.strData[7]) && mp.IsNumeric(mvars.strData[8]))
                                                    {
                                                        SvMsgInErr = false;

                                                        mp.cENGGMAONWRITEt((byte)(3 + Convert.ToByte(mvars.strData[8])), 0, 0, 0, Convert.ToByte(mvars.strData[6]), Convert.ToByte(mvars.strData[7])); //單屏                                                    }
                                                    }
                                                    #endregion TV130 OFF
                                                }
                                            }
                                            else if (mvars.strData[2].ToUpper() == "FIX")
                                            {
                                                if (mvars.strData.Length == 7)
                                                {
                                                    #region Primary FIX
                                                    if (mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]))
                                                    {
                                                        SvMsgInErr = false;
                                                        if (mvars.strData[4] == "160") mp.cENGGMAONWRITEp(2, 0, 160, Convert.ToByte(mvars.strData[5]), Convert.ToByte(mvars.strData[6])); //單屏                                                   
                                                    }
                                                    #endregion Primary FIX
                                                }
                                                else if (mvars.strData.Length == 8)
                                                {
                                                    #region TV130 FIX
                                                    if (mp.IsNumeric(mvars.strData[6]) && mp.IsNumeric(mvars.strData[7]))
                                                    {
                                                        SvMsgInErr = false;
                                                        mp.cENGGMAONWRITEt(2, 0, 0, 0, Convert.ToByte(mvars.strData[6]), Convert.ToByte(mvars.strData[7])); //單屏                                                }
                                                    }
                                                    #endregion TV130 FIX
                                                }
                                            }
                                            else if (mvars.strData.Length == 30 && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[27]))
                                            {
                                                SvMsgInErr = false;
                                                if (mvars.deviceID.Substring(0, 2) == "05")
                                                {
                                                    #region Primary
                                                    int[] svreg = new int[12];
                                                    int[] svdata = new int[12];
                                                    if (mvars.strData[3] == "160" && (mvars.strData[5] == "0" || mvars.strData[5] == "1" || mvars.strData[5] == "2"))
                                                    {
                                                        if (mvars.strData[5] == "0")
                                                        {
                                                            for (int i = 7; i <= 29; i += 2)
                                                            {
                                                                Form1.svuiregadr[(i - 7) / 2 + 91] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 103] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 115] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 127] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 139] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 151] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 163] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 175] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 187] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 199] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 211] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 223] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 235] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 247] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 259] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 271] = mvars.strData[i];
                                                            }
                                                        }
                                                        else if (mvars.strData[5] == "1")
                                                        {
                                                            for (int i = 7; i <= 29; i += 2)
                                                            {
                                                                Form1.svuiregadr[(i - 7) / 2 + 91 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 103 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 115 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 127 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 139 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 151 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 163 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 175 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 187 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 199 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 211 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 223 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 235 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 247 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 259 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 271 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                            }
                                                        }
                                                        else if (mvars.strData[5] == "2")
                                                        {
                                                            for (int i = 7; i <= 29; i += 2)
                                                            {
                                                                Form1.svuiregadr[(i - 7) / 2 + 91] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 103] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 115] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 127] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 139] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 151] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 163] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 175] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 187] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 199] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 211] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 223] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 235] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 247] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 259] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 271] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 91 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 103 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 115 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 127 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 139 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 151 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 163 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 175 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 187 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 199 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 211 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 223 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 235 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 247 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 259 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 271 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                            }
                                                        }
                                                    }
                                                    mp.cENGGMAONWRITEp(1, 0, Convert.ToByte(mvars.strData[3]), Convert.ToByte(mvars.strData[4]), Convert.ToByte(mvars.strData[5]));
                                                    #endregion Primary
                                                }
                                            }
                                            else if (mvars.strData.Length == 19 && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]) && mp.IsNumeric(mvars.strData[18]))
                                            {
                                                if (mvars.deviceID.Substring(0, 2) == "06")
                                                {
                                                    #region TV130
                                                    SvMsgInErr = false;
                                                    if (mvars.strData[6] == "0")
                                                    {
                                                        for (int i = 0; i < 12; i += 2)
                                                        {
                                                            Form1.svuiregadr[i * 12 + 61] = mvars.strData[7];
                                                            Form1.svuiregadr[i * 12 + 62] = mvars.strData[8];
                                                            Form1.svuiregadr[i * 12 + 63] = mvars.strData[9];
                                                            Form1.svuiregadr[i * 12 + 64] = mvars.strData[10];
                                                            Form1.svuiregadr[i * 12 + 65] = mvars.strData[11];
                                                            Form1.svuiregadr[i * 12 + 66] = mvars.strData[12];
                                                            Form1.svuiregadr[i * 12 + 67] = mvars.strData[13];
                                                            Form1.svuiregadr[i * 12 + 68] = mvars.strData[14];
                                                            Form1.svuiregadr[i * 12 + 69] = mvars.strData[15];
                                                            Form1.svuiregadr[i * 12 + 70] = mvars.strData[16];
                                                            Form1.svuiregadr[i * 12 + 71] = mvars.strData[17];
                                                            Form1.svuiregadr[i * 12 + 72] = mvars.strData[18];
                                                        }
                                                        mp.cENGGMAONWRITEt(1, 0, 0, 0, Convert.ToByte(mvars.strData[5]), 0);
                                                    }
                                                    else
                                                    {
                                                        int svlb = Convert.ToInt16(mvars.strData[6]) - 1;
                                                        byte svFPGAsel = Convert.ToByte(svlb % 4 / 2);

                                                        int svms = 61 + (svlb % 4 - svFPGAsel * 2) * 36 + svlb / 4 * 12 + svFPGAsel * 1024;    /// TV130
                                                        //int svme = 72 + (svlb % 4 - svFPGAsel * 2) * 36 + svlb / 4 * 12;    /// TV130
                                                        //string[] sRegDec = new string[13];   //addr
                                                        //string[] sDataDec = new string[13];  //data

                                                        for (int i = 0; i < 12; i++)
                                                        {
                                                            Form1.svuiregadr[svms] = mvars.strData[7 + i];
                                                            svms += 1;
                                                        }
                                                        mp.cENGGMAONWRITEt(1, 0, 0, 0, Convert.ToByte(mvars.strData[5]), Convert.ToByte(mvars.strData[6]));
                                                    }
                                                    #endregion TV130
                                                }
                                            }
                                            else if (mvars.strData.Length == 30 && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[27]))
                                            {
                                                SvMsgInErr = false;
                                                if (mvars.deviceID.Substring(0, 2) == "05")
                                                {
                                                    int[] svreg = new int[12];
                                                    int[] svdata = new int[12];
                                                    if (mvars.strData[3] == "160" && (mvars.strData[5] == "0" || mvars.strData[5] == "1" || mvars.strData[5] == "2"))
                                                    {
                                                        if (mvars.strData[5] == "0")
                                                        {
                                                            for (int i = 7; i <= 29; i += 2)
                                                            {
                                                                Form1.svuiregadr[(i - 7) / 2 + 91] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 103] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 115] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 127] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 139] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 151] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 163] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 175] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 187] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 199] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 211] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 223] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 235] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 247] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 259] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 271] = mvars.strData[i];
                                                            }
                                                        }
                                                        else if (mvars.strData[5] == "1")
                                                        {
                                                            for (int i = 7; i <= 29; i += 2)
                                                            {
                                                                Form1.svuiregadr[(i - 7) / 2 + 91 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 103 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 115 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 127 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 139 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 151 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 163 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 175 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 187 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 199 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 211 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 223 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 235 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 247 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 259 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 271 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                            }
                                                        }
                                                        else if (mvars.strData[5] == "2")
                                                        {
                                                            for (int i = 7; i <= 29; i += 2)
                                                            {
                                                                Form1.svuiregadr[(i - 7) / 2 + 91] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 103] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 115] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 127] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 139] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 151] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 163] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 175] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 187] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 199] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 211] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 223] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 235] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 247] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 259] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 271] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 91 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 103 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 115 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 127 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 139 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 151 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 163 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 175 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 187 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 199 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 211 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 223 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 235 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 247 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 259 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                                Form1.svuiregadr[(i - 7) / 2 + 271 + (int)(mvars.GAMMA_SIZE / 8)] = mvars.strData[i];
                                                            }
                                                        }
                                                    }
                                                }
                                                mp.cENGGMAONWRITEp(1, 0, Convert.ToByte(mvars.strData[3]), Convert.ToByte(mvars.strData[4]), Convert.ToByte(mvars.strData[5]));
                                            }
                                            #endregion ENG_GMA
                                        }
                                        else if (mvars.strData[1] == "UIREGADR_ON")
                                        {
                                            #region UIREGADR_ON  未啟用
                                            //if (mvars.strData.Length == 3 && mp.IsNumeric(mvars.strData[2]))    //ex. 12345@@VAM Control(Q),UIREGADR_ON,32
                                            //{
                                            //    SvMsgInErr = false;
                                            //    mvars.nvBoardcast = true;
                                            //    mp.cUIREGADRON(mvars.strData[2], 0, 0, 0, 0);
                                            //}
                                            //else if (mvars.strData.Length == 5 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]))
                                            //{
                                            //    if (Convert.ToByte(mvars.strData[3]) < 2)
                                            //    {
                                            //        //全屏廣播(160)附加可以指定單屏左右畫面 (畫面左=0,畫面右=1,單屏=2) 
                                            //        SvMsgInErr = false;
                                            //        mvars.nvBoardcast = false;
                                            //        mp.cUIREGADRON(mvars.strData[4], 160, 160, 160, Convert.ToByte(mvars.strData[3]));
                                            //    }
                                            //}
                                            //else if (mvars.strData.Length == 7 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]))
                                            //{
                                            //    if (Convert.ToByte(mvars.strData[5]) < 3)
                                            //    {
                                            //        //(任意數字,任意數字,指定單屏,單屏右畫面輸入0/單屏左畫面輸入1/單屏全畫面輸入2)  (strData.Length = 7)
                                            //        SvMsgInErr = false;
                                            //        mvars.nvBoardcast = true;
                                            //        mp.cUIREGADRON(mvars.strData[6], 0, 0, Convert.ToByte(mvars.strData[4]), Convert.ToByte(mvars.strData[5]));
                                            //    }
                                            //}
                                            #endregion UIREGADR_ON
                                        }
                                        else if (mvars.strData[1] == "UIREGADR_OFF")
                                        {
                                            #region UIREGADR_OFF   未啟用
                                            //if (mvars.strData.Length == 3 && mp.IsNumeric(mvars.strData[2]))    //ex. 12345@@VAM Control(Q),UIREGADR_ON,32
                                            //{
                                            //    SvMsgInErr = false;
                                            //    mvars.nvBoardcast = true;
                                            //    mp.cUIREGADROFF(mvars.strData[2], 0, 0, 0, 0);
                                            //}
                                            //else if (mvars.strData.Length == 5 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]))
                                            //{
                                            //    if (Convert.ToByte(mvars.strData[3]) < 2)
                                            //    {
                                            //        //全屏廣播(160)附加可以指定單屏左右畫面 (畫面左=0,畫面右=1,單屏=2) 
                                            //        SvMsgInErr = false;
                                            //        mp.cUIREGADROFF(mvars.strData[4], 160, 160, 160, Convert.ToByte(mvars.strData[3]));
                                            //    }
                                            //}
                                            //else if (mvars.strData.Length == 7 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]))
                                            //{
                                            //    if (Convert.ToByte(mvars.strData[5]) < 3)
                                            //    {
                                            //        //(任意數字,任意數字,指定單屏,單屏右畫面輸入0/單屏左畫面輸入1/單屏全畫面輸入2)  (strData.Length = 7)
                                            //        SvMsgInErr = false;
                                            //        mp.cUIREGADROFF(mvars.strData[6], 0, 0, Convert.ToByte(mvars.strData[4]), Convert.ToByte(mvars.strData[5]));
                                            //    }
                                            //}
                                            #endregion UIREGADR_OFF
                                        }
                                        else if (mvars.strData[1] == "VERSION")
                                        {
                                            #region VERSION 未啟用
                                            //if (mvars.strData.Length == 5 + 1 && mp.IsNumeric(mvars.strData[mvars.strData.Length - 4]) && mp.IsNumeric(mvars.strData[mvars.strData.Length - 3]) && mp.IsNumeric(mvars.strData[mvars.strData.Length - 2]) && mp.IsNumeric(mvars.strData[mvars.strData.Length - 1]))
                                            //{
                                            //    if ((mvars.strData[2] == "0" && mvars.strData[3] == "0" && mvars.strData[4] == "0" && mvars.strData[5] == "0" && mvars.svnova == false) ||
                                            //    (mvars.strData[2] != "0" && mvars.strData[3] != "0" && mvars.strData[4] != "0" && mvars.strData[5] != "0" && mvars.svnova))
                                            //    {
                                            //        SvMsgInErr = false;
                                            //        //mp.cVERSION(Convert.ToByte(mvars.strData[2]), Convert.ToByte(mvars.strData[3]), Convert.ToByte(mvars.strData[4]), Convert.ToByte(mvars.strData[5]));
                                            //    }
                                            //}
                                            #endregion
                                        }
                                        else if (mvars.strData[1] == "OMSEL")
                                        {
                                            #region OM_SEL (FPGA R004)  Primary  TV130 CarpStreamer
                                            if (mvars.strData.Length == 5)
                                            {
                                                SvMsgInErr = false;
                                                mp.cOMSEL(mvars.strData[2], mvars.strData[3], mvars.strData[4]);
                                            }
                                            else if (mvars.strData.Length == 7)
                                            {
                                                if (mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]))
                                                {
                                                    if (Convert.ToInt16(mvars.strData[2]) >= 0 && Convert.ToInt16(mvars.strData[2]) <= 21)
                                                    {
                                                        SvMsgInErr = false;
                                                        mp.cOMSEL(Convert.ToByte(mvars.strData[2]), 0, 0, Convert.ToByte(mvars.strData[5]), Convert.ToByte(mvars.strData[6]));                                                   
                                                    }
                                                }
                                            }
                                            #endregion OM_SEL 
                                        }
                                        else if (mvars.strData[1] == "COSREAD" && mvars.strData.Length == 6)
                                        {
                                            #region COSREAD (Primary)
                                            if (mvars.deviceID.Substring(0, 2) == "05")
                                            {
                                                string svdeviceID = mvars.deviceID;
                                                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(mvars.strData[4]), 2);
                                                if (mvars.demoMode)
                                                {
                                                    SvMsgInErr = false;
                                                    cds.lpData = mvars.handleIDMe + "@@" + mvars.strUInameMe + "," + mvars.strData[1] + ",model:" + mvars.deviceName + " not support,DONE";
                                                    mp.funSaveLogs("(S) " + cds.lpData);
                                                    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                    tme_Pull.Enabled = true;
                                                }
                                                else
                                                {
                                                    mvars.lblCmd = "MCU_FLASH_R66000";
                                                    mp.mhMCUFLASHREAD("66000".PadLeft(8, '0'), 8192);
                                                }
                                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                                {
                                                    if (mvars.strR66K == "")
                                                    {
                                                        SvMsgInErr = false;
                                                        cds.lpData = mvars.handleIDMe + "@@ERROR," + mvars.strUInameMe + "," + mvars.strData[1] + "," + mvars.strData[2] + "," + mvars.strData[3] + "," + mvars.strData[4] + "," + mvars.strData[5] + ",The ID." + mvars.strData[4] + @" was been ""Restore StandAlone"" ";
                                                        mp.funSaveLogs("(S) " + cds.lpData);
                                                        SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                        tme_Pull.Enabled = true;
                                                    }
                                                    else
                                                    {
                                                        Form1.lstmcuR66000.Items.Clear();
                                                        Form1.lstmcuR66000.Items.AddRange(mvars.strR66K.Split('~'));
                                                        if (Form1.lstmcuR66000.Items.Count > 43)
                                                        {
                                                            string svs = Form1.lstmcuR66000.Items[31].ToString();
                                                            if (svs.Substring(0, 2) == "0,")
                                                            {
                                                                SvMsgInErr = false;
                                                                cds.lpData = mvars.handleIDMe + "@@ERROR," + mvars.strUInameMe + "," + mvars.strData[1] + "," + mvars.strData[2] + "," + mvars.strData[3] + "," + mvars.strData[4] + "," + mvars.strData[5] + ",There is no recorded value in MCUFLASH";
                                                                mp.funSaveLogs("(S) " + cds.lpData);
                                                                SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                                tme_Pull.Enabled = true;
                                                            }
                                                            else
                                                            {
                                                                for (int i = 92; i < 103; i++)
                                                                    svs += "," + Form1.lstmcuR66000.Items[i - 60].ToString();

                                                                SvMsgInErr = false;
                                                                cds.lpData = mvars.handleIDMe + "@@" + mvars.strUInameMe + "," + mvars.strData[1] + "," + mvars.strData[2] + "," + mvars.strData[3] + "," + mvars.strData[4] + "," + mvars.strData[5] + ",DONE," + svs;
                                                                mp.funSaveLogs("(S) " + cds.lpData);
                                                                SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                                tme_Pull.Enabled = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            SvMsgInErr = false;
                                                            cds.lpData = mvars.handleIDMe + "@@ERROR," + mvars.strUInameMe + "," + mvars.strData[1] + "," + mvars.strData[2] + "," + mvars.strData[3] + "," + mvars.strData[4] + "," + mvars.strData[5] + ",The number of characters(" + Form1.lstmcuR66000.Items.Count + ") does not match";
                                                            mp.funSaveLogs("(S) " + cds.lpData);
                                                            SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                            tme_Pull.Enabled = true;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    SvMsgInErr = false;
                                                    cds.lpData = mvars.handleIDMe + "@@ERROR," + mvars.strUInameMe + "," + mvars.strData[1] + "," + mvars.strData[2] + "," + mvars.strData[3] + "," + mvars.strData[4] + "," + mvars.strData[5] + ",MCUFLASH READ Fail";
                                                    mp.funSaveLogs("(S) " + cds.lpData);
                                                    SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                                                    tme_Pull.Enabled = true;
                                                }
                                            }
                                            #endregion
                                        }
                                        else if (mvars.strData[1] == "BOXID")
                                        {
                                            #region BOXID    未啟用
                                            //mvars.nvBoardcast = false;          //
                                            //if (mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 6)
                                            //{
                                            //    //直接切換PG模式
                                            //    SvMsgInErr = false;
                                            //    mp.cBOXIDONOFF(1, mvars.strData[3], mvars.strData[4], mvars.strData[5], 1);
                                            //}
                                            //else if (mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 7)
                                            //{
                                            //    //最後一碼可管理PG
                                            //    SvMsgInErr = false; 
                                            //    mp.cBOXIDONOFF(1, mvars.strData[3], mvars.strData[4], mvars.strData[5], Convert.ToByte(mvars.strData[6]));
                                            //}
                                            //else if (mvars.strData[2].ToUpper() == "OFF" && mvars.strData.Length == 3)
                                            //{
                                            //    //直接切換PG模式
                                            //    SvMsgInErr = false;
                                            //    mp.cBOXIDONOFF(0, "0", "0", "0", 0);
                                            //}
                                            //else if (mvars.strData[2].ToUpper() == "OFF" && mvars.strData.Length == 4)
                                            //{
                                            //    //最後一碼可管理PG
                                            //    SvMsgInErr = false;
                                            //    mp.cBOXIDONOFF(0, "0", "0", "0", Convert.ToByte(mvars.strData[3]));
                                            //}
                                            #endregion BOXID
                                        }
                                        else if (mvars.strData[1] == "UIREGREADALL")
                                        {
                                            #region UIREGREADALL    未啟用

                                            //mvars.nvBoardcast = false;
                                            //if (mvars.strData.Length == 6 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]))
                                            //{
                                            //    SvMsgInErr = false; //mvars.lblCompose = mvars.strData[1]; 
                                            //    mp.cUIREGREADALL(Convert.ToByte(Convert.ToByte(mvars.strData[2]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[5]) - 1));
                                            //}
                                            #endregion UIREGREADALL
                                        }
                                        else if (mvars.strData[1] == "WALL_ALG")
                                        {
                                            #region WALL_ALG 未啟用
                                            //if (mvars.nualg)
                                            //{
                                            //    if (mvars.strData.Length == 6 + 1 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]))
                                            //    {
                                            //        SvMsgInErr = false; 
                                            //        mvars.nvBoardcast = false;
                                            //        mp.cWALLALG(mvars.strData[2], Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[5]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[6]) - 1));
                                            //    }
                                            //    else if (mvars.strData.Length == 3 && mp.IsNumeric(mvars.strData[2]))
                                            //    {
                                            //        SvMsgInErr = false; 
                                            //        mvars.nvBoardcast = true;
                                            //        mp.cWALLALG(mvars.strData[2], 0, 0, 0, 0);
                                            //    }
                                            //}
                                            #endregion WALL_ALG v0023改
                                        }
                                        else if (mvars.strData[1] == "WT_GRAY")
                                        {
                                            #region WT_GRAY   未啟用
                                            //mvars.nvBoardcast = false;          //都是非廣播下設定
                                            //if ((mvars.strData.Length == 5 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4])) ||
                                            //        (mvars.strData.Length == 8 + 1 && mp.IsNumeric(mvars.strData[5]) && mp.IsNumeric(mvars.strData[6]) && mp.IsNumeric(mvars.strData[7])))
                                            //{
                                            //    SvMsgInErr = false; 
                                            //    if (mvars.strData.Length == 5) { mvars.nvBoardcast = true; mp.cWTPGGRAY(0, 0, 0, 0, mvars.strData[mvars.strData.Length - 3], mvars.strData[mvars.strData.Length - 2], mvars.strData[mvars.strData.Length - 1]); }
                                            //    else { mp.cWTPGGRAY(Convert.ToByte(Convert.ToByte(mvars.strData[2]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[5]) - 1), mvars.strData[mvars.strData.Length - 3], mvars.strData[mvars.strData.Length - 2], mvars.strData[mvars.strData.Length - 1]); }
                                            //}
                                            //else if ((mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 3) || (mvars.strData[2].ToUpper() == "ON" && mvars.strData.Length == 6 + 1))
                                            //{
                                            //    SvMsgInErr = false; 
                                            //    if (mvars.strData.Length == 3) { mvars.nvBoardcast = true; mp.cWTPGONOFF(true, 0, 0, 0, 0); }
                                            //    else { mvars.nvBoardcast = false; mp.cWTPGONOFF(true, Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[5]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[6]) - 1)); }
                                            //}
                                            //else if ((mvars.strData[2].ToUpper() == "OFF" && mvars.strData.Length == 3) || (mvars.strData[2].ToUpper() == "OFF" && mvars.strData.Length == 6 + 1))
                                            //{
                                            //    SvMsgInErr = false; 
                                            //    if (mvars.strData.Length == 3) { mvars.nvBoardcast = true; mp.cWTPGONOFF(false, 0, 0, 0, 0); }
                                            //    else { mvars.nvBoardcast = false; mp.cWTPGONOFF(false, Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[5]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[6]) - 1)); }
                                            //}
                                            #endregion
                                        }
                                        else if (mvars.strData[1] == "MCU_FLASHCLS")
                                        {
                                            #region MCU_FLASHCLS   未啟用
                                            //mvars.nvBoardcast = false;
                                            //if (mvars.strData.Length == 2 || (mvars.strData.Length == 6 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5])))
                                            //{
                                            //    SvMsgInErr = false;
                                            //    Array.Resize(ref Form1.svuiregadr, mvars.uiregadr_default.Length);
                                            //    for (int svm = 0; svm < mvars.struiregadrdef.Split('~').Length; svm++)
                                            //    {
                                            //        Form1.svuiregadr[svm] = mvars.struiregadrdef.Split('~')[svm].Split(',')[1];
                                            //    }
                                            //    if (mvars.strData.Length == 2)
                                            //    {
                                            //        mvars.nvBoardcast = true;
                                            //        mp.cMCUFLASHCLS(0, 0, 0, 0);
                                            //    }
                                            //    else
                                            //    {
                                            //        mp.cMCUFLASHCLS(Convert.ToByte(Convert.ToByte(mvars.strData[2]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[3]) - 1), Convert.ToByte(Convert.ToByte(mvars.strData[4]) - 1), Convert.ToUInt16(Convert.ToUInt16(mvars.strData[5]) - 1));
                                            //    }
                                            //}
                                            #endregion
                                        }
                                        else if (mvars.strData[1] == "MCU_0X64000CLS")
                                        {
                                            #region MCU_0x64000CLS   未啟用
                                            //if (mvars.nualg)
                                            //{
                                            //    #region svuiuser Default [0]~[39]
                                            //    uc_brightnessAdj.svuiuserDef[0] = "128";     //辨識用(read only)
                                            //    uc_brightnessAdj.svuiuserDef[1] = "3";       //demura版本    
                                            //    uc_brightnessAdj.svuiuserDef[2] = "1";       //User Gamma
                                            //    uc_brightnessAdj.svuiuserDef[3] = "1";       //User Brightness
                                            //    uc_brightnessAdj.svuiuserDef[4] = "0";       //WT PG mode   
                                            //    uc_brightnessAdj.svuiuserDef[5] = "0";       //單燈板
                                            //    uc_brightnessAdj.svuiuserDef[6] = "0";       //大屏demura
                                            //    uc_brightnessAdj.svuiuserDef[7] = "255";     //87碼
                                            //    uc_brightnessAdj.svuiuserDef[8] = "511";     //87碼
                                            //    uc_brightnessAdj.svuiuserDef[9] = "1";       //GAMUT開關
                                            //    uc_brightnessAdj.svuiuserDef[10] = "220";    //Gamma     (Read Only, 紀錄 userxy )     
                                            //    uc_brightnessAdj.svuiuserDef[11] = "512";    //Gamma 32
                                            //    uc_brightnessAdj.svuiuserDef[12] = "1024";   //Gamma 64   
                                            //    uc_brightnessAdj.svuiuserDef[13] = "2048";   //Gamma 128
                                            //    uc_brightnessAdj.svuiuserDef[14] = "4080";   //Gamma 255
                                            //    uc_brightnessAdj.svuiuserDef[15] = "1024";   //gRratio 
                                            //    uc_brightnessAdj.svuiuserDef[16] = "1024";   //gGratio
                                            //    uc_brightnessAdj.svuiuserDef[17] = "1024";   //gBratio
                                            //    uc_brightnessAdj.svuiuserDef[18] = "4080";   //White-tracking RED for PG mode
                                            //    uc_brightnessAdj.svuiuserDef[19] = "4080";   //White-tracking GREEN for PG mode
                                            //    uc_brightnessAdj.svuiuserDef[20] = "4080";   //White-tracking BLUE for PG mode
                                            //    uc_brightnessAdj.svuiuserDef[21] = "16384";
                                            //    uc_brightnessAdj.svuiuserDef[22] = "0";
                                            //    uc_brightnessAdj.svuiuserDef[23] = "0";
                                            //    uc_brightnessAdj.svuiuserDef[24] = "0";
                                            //    uc_brightnessAdj.svuiuserDef[25] = "16384";
                                            //    uc_brightnessAdj.svuiuserDef[26] = "0";
                                            //    uc_brightnessAdj.svuiuserDef[27] = "0";
                                            //    uc_brightnessAdj.svuiuserDef[28] = "0";
                                            //    uc_brightnessAdj.svuiuserDef[29] = "16384";
                                            //    uc_brightnessAdj.svuiuserDef[30] = "6850";   //pidxr     (Read Only, 紀錄userxy)
                                            //    uc_brightnessAdj.svuiuserDef[31] = "3146";   //pidyr     (Read Only, 紀錄userxy)
                                            //    uc_brightnessAdj.svuiuserDef[32] = "1944";   //pidxg     (Read Only, 紀錄userxy)
                                            //    uc_brightnessAdj.svuiuserDef[33] = "7481";   //pidyg     (Read Only, 紀錄userxy)
                                            //    uc_brightnessAdj.svuiuserDef[34] = "1240";   //pidxb     (Read Only, 紀錄userxy)
                                            //    uc_brightnessAdj.svuiuserDef[35] = "0723";   //pidyb     (Read Only, 紀錄userxy)
                                            //    uc_brightnessAdj.svuiuserDef[36] = "2906";   //pidxw     (Read Only, 紀錄userxy)
                                            //    uc_brightnessAdj.svuiuserDef[37] = "3113";   //pidyw     (Read Only, 紀錄userxy)
                                            //    uc_brightnessAdj.svuiuserDef[38] = "8835";   //pidcct    (Read Only, 紀錄userCCT)
                                            //    uc_brightnessAdj.svuiuserDef[39] = "0";      //GAMUT紀錄    (Read Only, 多工紀錄器)  
                                            //    /*
                                            //                                                0000000000000000 - (0)預設色域PID
                                            //                                                0000000000000001 - (1)色域PAL
                                            //                                                0000000000000010 - (2)色域NTSC
                                            //     */
                                            //    #endregion
                                            //}
                                            #endregion
                                        }
                                        else if (mvars.strData[1] == "FPGA_REG_W")
                                        {
                                            #region FPGA_REG_W    未啟用
                                            //mvars.nvBoardcast = false;
                                            //if (mvars.strData.Length >= 4 && mvars.strData.Length % 2 == 0)
                                            //{
                                            //    SvMsgInErr = false; 
                                            //    int[] svreg = new int[(mvars.strData.Length - 2) / 2];
                                            //    int[] svdata = new int[svreg.Length];
                                            //    for (int i = 0; i < svreg.Length; i++)
                                            //    {
                                            //        svreg[i] = Convert.ToInt16(mvars.strData[2 + i * 2]);
                                            //        svdata[i] = Convert.ToInt16(mvars.strData[3 + i * 2]);
                                            //    }
                                            //    mvars.nvBoardcast = true; mp.cFPGAREGW(0, 0, 0, svreg, svdata);
                                            //}
                                            //else if (mvars.strData.Length >= 7 && mvars.strData.Length % 2 == 1)
                                            //{
                                            //    SvMsgInErr = false; 
                                            //    int[] svreg = new int[(mvars.strData.Length - 5) / 2];
                                            //    int[] svdata = new int[svreg.Length];
                                            //    for (int i = 0; i < svreg.Length; i++)
                                            //    {
                                            //        svreg[i] = Convert.ToInt16(mvars.strData[5 + i * 2]);
                                            //        svdata[i] = Convert.ToInt16(mvars.strData[6 + i * 2]);
                                            //    }
                                            //    mvars.nvBoardcast = false; mp.cFPGAREGW(Convert.ToUInt16(mvars.strData[2]), Convert.ToUInt16(mvars.strData[3]), Convert.ToUInt16(mvars.strData[4]), svreg, svdata);
                                            //}
                                            #endregion
                                        }
                                        else if (mvars.strData[1] == "USERBRIG")
                                        {
                                            #region USERBRIG    未啟用
                                            if (mvars.nualg)
                                            {
                                                //if (mvars.strData.Length == 6 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]))
                                                //{
                                                //    int i = 0;
                                                //    for (i = 0; i < Form1.hwCards; i++)
                                                //    {
                                                //        if ((Convert.ToInt32(mvars.strData[3]) - 1) == Form1.hwCard[i].iSender && (Convert.ToInt32(mvars.strData[4]) - 1) == Form1.hwCard[i].iPort && (Convert.ToInt32(mvars.strData[5]) - 1) == Form1.hwCard[i].iScan) break;
                                                //    }
                                                //    if (i < Form1.hwCards)
                                                //    {
                                                //        SvMsgInErr = false; 
                                                //        mvars.nvBoardcast = false;
                                                //        byte svn = Convert.ToByte(mvars.strData[2]);
                                                //        mp.cUSERBRIG(ref svn, Convert.ToUInt16(mvars.strData[3]), Convert.ToUInt16(mvars.strData[4]), Convert.ToUInt16(mvars.strData[5]));
                                                //    }
                                                //}
                                                //else if (mvars.strData.Length == 3 && mp.IsNumeric(mvars.strData[2]))
                                                //{
                                                //    SvMsgInErr = false; 
                                                //    mvars.nvBoardcast = true;
                                                //    byte svn = Convert.ToByte(mvars.strData[2]);
                                                //    mp.cUSERBRIG(ref svn, 0, 0, 0);
                                                //}
                                            }
                                            #endregion USERBRIG
                                        }
                                        else if (mvars.strData[1] == "USERCCT")
                                        {
                                            #region USERCCT    未啟用
                                            //if (mvars.nualg)
                                            //{
                                            //    if (mvars.strData.Length == 7 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]) && mp.IsNumeric(mvars.strData[4]) && mp.IsNumeric(mvars.strData[5]))
                                            //    {
                                            //        int i = 0;
                                            //        for (i = 0; i < Form1.hwCards; i++)
                                            //        {
                                            //            if ((Convert.ToInt32(mvars.strData[4]) - 1) == Form1.hwCard[i].iSender && (Convert.ToInt32(mvars.strData[5]) - 1) == Form1.hwCard[i].iPort && (Convert.ToInt32(mvars.strData[6]) - 1) == Form1.hwCard[i].iScan) break;
                                            //        }
                                            //        if (i < Form1.hwCards)
                                            //        {
                                            //            SvMsgInErr = false; 
                                            //            mvars.nvBoardcast = false;
                                            //            byte svbrig = Convert.ToByte(mvars.strData[2]);
                                            //            int svn = Convert.ToInt16(mvars.strData[3]);
                                            //            mp.cUSERCCT(ref svbrig, ref svn, Convert.ToUInt16(mvars.strData[4]), Convert.ToUInt16(mvars.strData[5]), Convert.ToUInt16(mvars.strData[6]));
                                            //        }
                                            //    }
                                            //    else if (mvars.strData.Length == 4 && mp.IsNumeric(mvars.strData[2]) && mp.IsNumeric(mvars.strData[3]))
                                            //    {
                                            //        SvMsgInErr = false; 
                                            //        mvars.nvBoardcast = true;
                                            //        byte svbrig = Convert.ToByte(mvars.strData[2]);
                                            //        int svn = Convert.ToInt16(mvars.strData[3]);
                                            //        mp.cUSERCCT(ref svbrig, ref svn, 0, 0, 0);
                                            //    }
                                            //}
                                            #endregion USERCCT
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Form1.lstget1.Items.Add("(Err) " + SvMsgIn + @" without + " + mvars.strUInameMe + ",error");
                                mp.funSaveLogs("(Err) " + SvMsgIn + @" without + " + mvars.strUInameMe + ",error");
                            }
                        }
                        else
                        {
                            Form1.lstget1.Items.Add("(Err) " + SvMsgIn + @" Split(',').Length <= 1 or handle ID = " + mvars.handleIDfrom + ",error");
                            mp.funSaveLogs("(Err) " + SvMsgIn + @" Split(',').Length <= 1 or handle ID = " + mvars.handleIDfrom + ",error");
                        }
                    }
                    else
                    {
                        Form1.lstget1.Items.Add("(Err) " + SvMsgIn + @" without ""@@""");
                        mp.funSaveLogs("(Err) " + SvMsgIn + @" without ""@@""");
                    }


                    if (SvMsgInErr)
                    {
                        if (mvars.handleIDfrom == 0 || mvars.handleIDfrom == -1) tme_Pull.Enabled = true;
                        else
                        {
                            Form1.lstget1.Items.Add(mvars.handleIDMe + "@@ERROR,Plesae check Command\"" + SvMsgIn + "\"");
                            cds.lpData = mvars.handleIDMe + "@@ERROR,Plesae check Command\"" + SvMsgIn + "\"";
                            mp.funSaveLogs("(S) " + cds.lpData);
                            SendMessage(mvars.handleIDfrom, WM_COPYDATA, 0, ref cds);
                            tme_Pull.Enabled = true;
                        }
                    }
                }
            }
        }

        #endregion sendmessage

        private void tsmnu_ca410_Click(object sender, EventArgs e)
        {
            #region parameter initial
            //mvars.flgSelf = true;
            //mp.pidinit();
            #endregion parameter initial
            //if (chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }

            h_project.Enabled = !h_project.Enabled;
            hTool.Enabled = !hTool.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_exit.Text = "Exit CA410";

            this.Size = new Size(1050, 650);
            pnl_frm1.BringToFront();
            ucca410 = new uc_ca410
            {
                Parent = pnl_frm1,
                //Dock = DockStyle.Fill
            };
        }



        private void tme_RSIn_Tick(object sender, EventArgs e)
        {
            bool svDone = false;
            string Svs = "-1";
            if (dgvformmsg.Visible == false)
            {
                tslbl_Pull.ForeColor = Color.Red;
                tslbl_RSIn.ForeColor = Color.Green;
            }
            if (mvars.flgReceived == true)
            {
                tme_RSIn.Enabled = false; tslbl_RSIn.ForeColor = Color.Red;
                mvars.flgSend = false; //1113
                if (mvars.strReceive.IndexOf("ERROR", 0) == -1)
                {
                    if (mvars.strData != null) Array.Clear(mvars.strData, 0, mvars.strData.Length);
                    mvars.strData = mvars.strReceive.Split(',');    //sample [0]DONE,[1]DeviceID,[2]DeviceID,[3]Cmd,[4]DataCount,[5]DataCount,[6]MCU Communication,[7]ip,[8]ip,[9]CheckSum,[10]CheckSum
                    //
                    if (mvars.strData.Length > 5)
                    {
                        //字串數吻合則先予以SvDone = True , case 中再去判斷[6]是否回"3"
                        if ((mvars.strData.Length - 1) == Convert.ToInt32(mvars.strData[4]) * 256 + Convert.ToInt32(mvars.strData[5])) { svDone = true; }
                    }
                }
                else
                {
                    if (mvars.strData != null) Array.Clear(mvars.strData, 0, mvars.strData.Length);
                    mvars.strData = mvars.strReceive.Split(',');
                    if (mvars.svnova)
                    {
                        if (mvars.lblCmd == "EndcCMD") { svDone = false; }
                    }
                    else
                    {
                        mvars.strData = mvars.strReceive.Split(',');    //sample [0]ERROR_Datacount Overflow @ case 0,[1]....."
                        if (mvars.strReceive.IndexOf("Timeout") > -1) { Svs += "," + mvars.strReceive; mvars.txtErrMsg = "5"; mvars.errClose = true; tme_Warn.Enabled = true; this.Enabled = false; }
                        mvars.strData = mvars.strData[0].Split('_');    //sample [0]ERROR_[1]Datacount Overflow @ case 0"
                    }
                }
                //
                switch (mvars.lblCmd)
                {
                    case "GammaSet":
                        if (svDone && mvars.ReadDataBuffer[6 - 1] == 3) { Svs = "DONE"; mp.doDelayms(1000); }
                        break;
                    case "GammaTimes":
                        if (svDone && mvars.ReadDataBuffer[6 - 1] == 3)
                        {
                            if (mvars.ReadDataBuffer[12 - 1] == 63 || mvars.ReadDataBuffer[12 - 1] == 62 || mvars.ReadDataBuffer[12 - 1] == 60 || mvars.ReadDataBuffer[12 - 1] == 56 || mvars.ReadDataBuffer[12 - 1] == 48 || mvars.ReadDataBuffer[12 - 1] == 32 || mvars.ReadDataBuffer[12 - 1] == 0)
                            {
                                switch (mvars.ReadDataBuffer[12 - 1])
                                {
                                    case 63:
                                        Svs = "t6";
                                        break;
                                    case 62:
                                        Svs = "t5";
                                        break;
                                    case 60:
                                        Svs = "t4";
                                        break;
                                    case 56:
                                        Svs = "t3";
                                        break;
                                    case 48:
                                        Svs = "t2";
                                        break;
                                    case 32:
                                        Svs = "t1";
                                        break;
                                    case 0:
                                        Svs = "t0";
                                        break;
                                }
                            }
                        }
                        break;
                    default:
                        if (mvars.svnova)// || mvars.GMAtype == "C12A")
                        {
                            if (mvars.lblCmd == "EndcCMD" && svDone) Svs = "DONE"; 
                        }
                        else { if (svDone) Svs = "DONE"; }
                        break;
                }
                if (Svs == "-1" || Svs.IndexOf("-1,") != -1 || Svs.IndexOf("ERROR,") != -1)
                {
                    if (Svs.IndexOf("ERROR,", 0) == -1) { mvars.lGet[mvars.lCount] = mvars.lblCmd + ",ERROR,1"; }
                    else { mvars.lGet[mvars.lCount] = mvars.lblCmd + "," + Svs; }
                    if (mvars.strData.Length == 2) { mvars.lGet[mvars.lCount] += "," + mvars.strData[1]; }
                }
                else
                {
                    if (Svs.IndexOf("DONE", 0) != -1) mvars.lGet[mvars.lCount] = mvars.lblCmd + ",DONE,1";
                    else mvars.lGet[mvars.lCount] = mvars.lblCmd + ",DONE,1," + Svs;
                }
                mvars.lCount++;
                tslbltarget.Text = mvars.lCount.ToString();
                //需關閉,如果拖慢速度的話
                //if (mvars.FormShow[4]) { Form4.lblcount.Text = mvars.lCount.ToString(); }
                //
                mvars.flgSend = false; mvars.flgReceived = false;
                if (mvars.flgSelf == true) { mvars.flgSend = false; mvars.flgReceived = false; }
                else
                {
                    if (mvars.lCounts == mvars.lCount)
                    {
                        // for C12A SendMessage用
                        // 目前的 funSendMessage 的程序只會回傳 message = mvars.lCmd[mvars.lCounts - 1] + mvars.lGet[mvars.lCounts - 1];
                        if (mvars.lblCompose == "IPSET" || mvars.lblCompose == "VERSION")
                        {
                            mvars.lCmd[mvars.lCounts - 1] = mvars.lblCompose + ",";
                            if (mvars.verMCU != "-1" && mvars.verFPGA != "-1")
                            {
                                mvars.lGet[mvars.lCounts - 1] = "UI," + mvars.UImajor + ",MCU," + mvars.verMCU + ",FPGA," + mvars.verFPGA;
                            }
                            else
                            {
                                mvars.lGet[mvars.lCounts - 1] = "ERROR,UI," + mvars.UImajor + ",MCU," + mvars.verMCU + ",FPGA," + mvars.verFPGA;
                            }
                            lst_get1.Items.Add(mvars.lGet[mvars.lCounts - 1]); lst_get1.TopIndex = lst_get1.Items.Count - 1;
                        }
                        else
                        {
                            if (mvars.lblCmd == "EndcCMD")
                            {
                                if (mvars.lGet[mvars.lCounts - 1].IndexOf("DONE", 0) > -1) 
                                {
                                    if (mvars.lblCompose == "PIDINIT") lst_get1.Items.Insert(0, "DONE," + Form1.tslblCOM.Text + "," + Form1.tslblStatus.Text);
                                    else lst_get1.Items.Insert(0, "DONE"); 
                                }
                                else 
                                { 
                                    if (mvars.lblCompose == "PIDINIT") lst_get1.Items.Insert(0, "ERROR," + Form1.tslblStatus.Text);
                                    else lst_get1.Items.Insert(0, "ERROR"); 
                                }
                                lst_get1.TopIndex = 0;
                            }
                        }
                        funSendMessage("");
                    }
                    else { mvars.flgSend = false; mvars.flgReceived = false; }
                }
            }
        }

        private void tme_Warn_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt16(mvars.txtErrMsg) > -1)
            {
                //label_CtrlSystemCount.Text = "Control System ： Seeking after " + mvars.txtErrMsg + "s";
                label_CtrlSystemCount.Text = mvars.txtErrMsg + "s";
                mvars.txtErrMsg = (Convert.ToInt16(mvars.txtErrMsg) - 1).ToString();
                mvars.Break = false;
            }
            else if (mvars.txtErrMsg == "-2")
            {
                if (lbl_form.Text == "form1")
                {
                    tme_Warn.Enabled = false; this.TopMost = false; this.Enabled = true; tme_Warn.Interval = 1000;
                    if (this.Visible == false) { this.Visible = true; }
                }
                else
                {
                    //lstget1.Items.Add(lbl_form.Text + " initialize..." + lstget1.Items.Count);
                }
            }
            else if (mvars.txtErrMsg == "-1")
            {
                tme_Warn.Enabled = false;
                if (MultiLanguage.DefaultLanguage == "en-US") { label_CtrlSystemCount.Text = "Unknow"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { label_CtrlSystemCount.Text = "未知"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { label_CtrlSystemCount.Text = "未知"; }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { label_CtrlSystemCount.Text = "知らない"; }
                //tme_C.Enabled = true;
            }
        }

        private void funSendMessage(string message)
        {
            //int hWnd = FindWindow(null, @"WT_UI"); if (hWnd == 0 && mvars.handleIDfrom != 0) { hWnd = mvars.handleIDfrom; }
            int hWnd = mvars.handleIDfrom;
            if (hWnd == 0) mp.funSaveLogs("(Err) No Send_UI");
            else
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes("DONE");
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)3000;
                cds.cbData = sarr.Length;
                if (mvars.lblCompose == "") { mvars.lblCompose = mvars.lblCmd; }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                {
                    message = "@@ERROR," + mvars.strUInameMe + "," + mvars.lblCompose;
                }
                else
                {
                    message = "@@" + mvars.strUInameMe + "," + mvars.lblCompose;
                }
                if (lst_get1.Items.Count > 0)
                {
                    for (int svi = 0; svi < lst_get1.Items.Count; svi++)
                    {
                        message += "," + lst_get1.Items[svi].ToString();
                    }
                }
                cds.lpData = mvars.handleIDMe + message;
                mp.funSaveLogs("(S) " + cds.lpData);
                SendMessage(hWnd, WM_COPYDATA, 0, ref cds);
            }
            mvars.lblCompose = "";
            mvars.flgSelf = true;
            tme_Pull.Enabled = true;
        }

        private void tsmnu_piddmr_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }
            #endregion parameter initial

            h_project.Enabled = !h_project.Enabled;
            //hTool.Enabled = !hTool.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 Demura(&E)";

            #region 多語系高級切換
            //string svs2 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs2 = "Exit PictureAdjust (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs2 = "離開 影像調整 (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs2 = "离开 影像调整 (&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs2 = "閉じる PictureAdjust (&E)";
            //h_exit.Text = svs2;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("exdemura");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換


            this.Size = new Size(880, 500);
            pnl_frm1.BringToFront();
            ucdmr = new uc_C12Ademura
            {
                Parent = pnl_frm1,
            };
        }

        private void cmb_FPGAsel_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvars.FPGAsel = Convert.ToByte(cmbFPGAsel.SelectedIndex);
        }


        private void markreset(int svtotalcounts, bool svdelfb, bool selfrun)
        {
            //mvars.lstget.Items.Clear();
            Form1.lstget1.Items.Clear();
            mvars.t1 = DateTime.Now;
            mvars.strReceive = "";
            mvars.lCounts = svtotalcounts;
            mvars.lCount = 0;
            Array.Resize(ref mvars.lCmd, svtotalcounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, svtotalcounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.flgDelFB = svdelfb;
            mvars.flgSelf = selfrun;
            Form1.tslblStatus.Text = "";
            mvars.flgSend = false;
            mvars.flgReceived = false;
        }



        private void h_pictureadjust_Click(object sender, EventArgs e)
        {
            h_pictureadjust.Enabled = false;
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "MCU".Length) != "MCU") { h_pictureadjust.Enabled = true; return; }
            #endregion parameter initial

            h_project.Enabled = !h_project.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 影像調整(&E)";
            h_pictureadjust.Visible = false;
            h_screenconfig.Visible = false;
            tsmnu_ota.Visible = false; mvars.flgForceUpdate = false;


            #region 多語系高級切換
            string svs2 = "";
            if (MultiLanguage.DefaultLanguage == "en-US")
                svs2 = "Exit Demura (&E)";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                svs2 = "離開 影像調整 (&E)";
            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                svs2 = "离开 Demura (&E)";
            else if (MultiLanguage.DefaultLanguage == "ja-JP")
                svs2 = "閉じる Demura (&E)";
            h_exit.Text = svs2;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("expictureadjust");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換

            this.Size = new Size(710, 700);
            pnl_frm1.BringToFront();
            ucpicadj = new uc_PictureAdjust
            {
                Parent = pnl_frm1,
                Dock = DockStyle.Fill
            };
        }

        private void h_screenconfig_Click(object sender, EventArgs e)
        {
            if (mvars.FormShow[11] == false)
            {
                #region parameter initial
                mvars.flgSelf = true;
                mp.pidinit();
                if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "MCU".Length) != "MCU") { h_pictureadjust.Enabled = true; return; }
                #endregion parameter initial

                #region 多語系高級切換
                //ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
                //string exatg = resource.GetString("exscrF");
                //if (exatg != "" && exatg != null) h_exit.Text = exatg;
                #endregion 多語系高級切換

                //0037 modify
                menuStrip1.Visible = false;

                this.Size = new Size(520, 550);     //uc_scrconfigF 520,(368+62)=430
                pnl_frm1.BringToFront();
                ucscrF = new uc_scrConfigF
                {
                    Parent = pnl_frm1,
                    Dock = DockStyle.Fill
                };
                Form1.ActiveForm.ControlBox = false;
            }
        }


        private void h_ota_Click(object sender, EventArgs e)
        {
            
        }

        private string GetLocalIP()
        {
            IPHostEntry Host;
            Host = Dns.GetHostEntry(Dns.GetHostName());
            //cmb_HostIP.Items.AddRange(Host.AddressList);
            foreach (IPAddress ip in Host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return ("127.1");
        }


        public static void Form1_hide()
        {
            if (mvars.FormShow[6]) Form1.i3init.Close();
            Form1.ActiveForm.Hide();
        }


        byte svatgcount = 0;
        private void tslbl_atgmode_MouseDown(object sender, MouseEventArgs e)
        {
            //v0037 add 避免非Primary機種進入拼接模式
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                svatgcount++;
                if (svatgcount > 10)
                {
                    chk_atgmode.Checked = !chk_atgmode.Checked;
                    foreach (Form form in Application.OpenForms)
                    {
                        LoadAll(form);
                    }
                    svatgcount = 0;
                }
            }
                
        }

        private void tsmnu_send_Click(object sender, EventArgs e)
        {
            if (mvars.FormShow[11] == false)
            {
                #region parameter initial
                mvars.flgSelf = true;
                mp.pidinit();
                if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "MCU".Length) != "MCU") { h_pictureadjust.Enabled = true; return; }
                #endregion parameter initial

                this.Size = new Size(520, 430);
                pnl_frm1.BringToFront();
                ucscrF = new uc_scrConfigF
                {
                    Parent = pnl_frm1,
                    Dock = DockStyle.Fill
                };
                menuStrip1.Visible = false;
            }
        }

        private void tsmnu_single_Click(object sender, EventArgs e)
        {
            //string v = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //{
            //    if (mp.InputBox("Screen Config", "『Restore Stand Alone 』?", ref v, this.Left + 300, this.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
            //}
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //{
            //    if (mp.InputBox("大屏拼接", "是否執行『復原單屏』?", ref v, this.Left + 300, this.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
            //}
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //{
            //    if (mp.InputBox("大屏拼接", "是否執行『复原单屏』?", ref v, this.Left + 300, this.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
            //}
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //{
            //    if (mp.InputBox("大画面設定", "是否執行『単一画面に戻す』?", ref v, this.Left + 300, this.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
            //}
            //uc_box.independentbox();
        }

        private void tsmnu_save_Click(object sender, EventArgs e)
        {
            //uc_box.btn
        }


        
        private void tsmnu_EDIDud_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }
            #endregion parameter initial

            h_project.Enabled = !h_project.Enabled;
            //hTool.Enabled = !hTool.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 Hex(&E)";

            #region 多語系高級切換
            //string svs2 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs2 = "Exit Hex (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs2 = "離開 Hex (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs2 = "离开 Hex (&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs2 = "閉じる Hex (&E)";
            //h_exit.Text = svs2;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("exhex");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換

            this.Size = new Size(800, 650);
            mvars.actFunc = "Hex";
            pnl_frm1.BringToFront();
            ucflash = new uc_Flash
            {
                Parent = pnl_frm1,
                Dock = DockStyle.Fill
            };
        }

        private void tsmnu_user_Click(object sender, EventArgs e)
        {
            string value = "";
            string svmsg = "Please enter password";
            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "Please enter password"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "請輸入密碼"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "请输入密码"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = "Please enter password"; }
            // GetWindowRec 取得顯示表單位置
            RECT rct;                   
            GetWindowRect(mvars.handleIDMe, out rct);
            if (mp.InputBox("", svmsg, ref value, rct.Left + 100, rct.Top + 100, 0, 0, 5, "*") == DialogResult.Cancel) { return; }
            else
            {
                mvars.flgForceUpdate = false;
                if (value == "666" || mvars.msgA == "666" || value == "mmm" || mvars.msgA == "mmmm" || mvars.msgA == "")
                {
                    tsmnu_user.Visible = false;
                    tsmnu_logout.Visible = true;
                    mvars.flgsuperuser = false;
                    tsmnu_ota.Visible = true;
                    tsmnu_EDIDud.Visible = false;
                    if (mvars.msgA == "mmmm" || mvars.msgA == "") mvars.flgForceUpdate = true;
                    if (mvars.msgA == "") { mvars.demoMode = true; tsmnu_demomode.Checked = true; }
                }
                else if (value == "666888" || mvars.msgA == "666888")
                {
                    tsmnu_user.Visible = false;
                    tsmnu_logout.Visible = true;
                    mvars.flgsuperuser = true;
                    tsmnu_EDIDud.Visible = true;
                }
            }
        }

        private void tsmnu_logout_Click(object sender, EventArgs e)
        {
            tsmnu_logout.Visible = false;
            tsmnu_user.Visible = true;
            tsmnu_ota.Visible = false;
            tsmnu_EDIDud.Visible = false;
        }

        private void chk_boardcast_CheckedChanged(object sender, EventArgs e)
        {
            cmbdeviceid.Enabled = !chk_boardcast.Checked;
            mvars.nvBoardcast = chk_boardcast.Checked;
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (chk_boardcast.Checked)
                    mvars.deviceID = "05A0";
                else
                    mvars.deviceID = "05" + cmbdeviceid.SelectedIndex.ToString("00");
            }
        }

        private void tsmnu_more_Click(object sender, EventArgs e)
        {
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }
            #endregion parameter initial

            h_project.Enabled = !h_project.Enabled;
            h_pid.Enabled = !h_pid.Enabled;
            h_user.Enabled = !h_user.Enabled;
            h_lan.Enabled = !h_lan.Enabled;
            h_exit.Text = "離開 more(&M)";

            #region 多語系高級切換
            //string svs2 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs2 = "Exit more (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs2 = "離開 more (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs2 = "离开 more (&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs2 = "閉じる more (&E)";
            //h_exit.Text = svs2;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("exmore");
            if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換

            this.Size = new Size(750, 550);
            mvars.actFunc = "ota";
            pnl_frm1.BringToFront();
            uccoding = new uc_coding
            {
                Parent = pnl_frm1,
                Dock = DockStyle.Fill
            };
        }



        private void tme_ota_Tick(object sender, EventArgs e)
        {
            if (mvars.actFunc == "bcBurning")
            {
                svbcBurningCnt++;
                if (svbcBurningCnt > 4)
                {
                    mvars.actFunc = "";
                    tme_ota.Enabled = false;
                }
            }

            else// if (mvars.actFunc == "ota")
            {
                if (nsckC.Connected == true)
                {
                    if (mvars.flgValidate == false)
                    {
                        txt_serverPort.BackColor = Color.LightGreen;
                        mvars.flgValidate = true;
                        sendData = "@@ValidateRequest~~" + hostIP;
                        mp.showStatus1("(OUT)  " + sendData, lst_get1, "");
                        byte[] sendBytes = Encoding.UTF8.GetBytes(sendData);
                        nsckC.Send(sendBytes);
                    }
                    else
                    {
                        byte[] buffer = new byte[1024];
                        try
                        {
                            int rec = nsckC.Receive(buffer);
                            if (rec > 0 && buffer[0] != 0)
                            {
                                string data = System.Text.Encoding.UTF8.GetString(buffer, 0, rec);
                                if (data.Length != 0)
                                {
                                    if (data.ToUpper().IndexOf("ERROR", 0) != -1)
                                    {
                                        sendData = mvars.lblCmd;
                                        mp.showStatus1("(OUT)  " + sendData, lst_get1, "");
                                        byte[] sendBytes = Encoding.UTF8.GetBytes(sendData);
                                        nsckC.Send(sendBytes);
                                    }
                                    nsckC_DataArrival(data);
                                }
                            }
                            else
                            {
                                mp.showStatus1("(Err) buffer[0] = 0 rec=" + rec.ToString() + "_" + mvars.lblCmd, lst_get1, "");
                                sendData = mvars.lblCmd;
                                mp.showStatus1("(OUT)  " + sendData, lst_get1, "");
                                byte[] sendBytes = Encoding.UTF8.GetBytes(sendData);
                                nsckC.Send(sendBytes);
                            }
                        }
                        catch (Exception ex)
                        {
                            mp.showStatus1("(Err) " + ex.Message + "_" + mvars.lblCmd, lst_get1, "");
                        }
                    }
                }
                else
                {
                    //tme_ota.Enabled = false;
                    if (mvars.FormShow[11] == false)
                    {
                        txt_serverPort.BackColor = Color.White;
                        txt_serverPortnsckF.Text = "";
                        lbl_timer1.Text = "0";
                        lbl_chc.Text = "0";
                        lbl_pocketsize.Text = "0";
                        lbl_counter.Text = "0";
                        lbl_getmods.Text = "0";
                        h_exit.Enabled = true;
                        tsmnu_ota.Visible = true;
                        tsmnu_ota.Enabled = true;
                    } 
                }
                if (mvars.FormShow[6] == false) { this.Enabled = true; this.Focus(); tme_ota.Enabled = false; }
            }
            //else if (mvars.actFunc == "screenconfig")
            //{
            //    this.Refresh();
            //    svA %= 360;
            //    Graphics g = this.CreateGraphics();
            //    int svA1 = svA - 30;
            //    int i = 0;
            //    do
            //    {
            //        svA1 -= i;
            //        svA1 += 360;
            //        svA1 %= 360;
            //        string[] svp1 = computePos(svA1).Split(',');
            //        g.FillEllipse(new SolidBrush(Color.FromArgb(255 - i * 20, 64, 255, 0)), Convert.ToInt16(svp1[0]) - 20, Convert.ToInt16(svp1[1]) - 20, 40, 40);
            //        i++;
            //    }
            //    while (i < 10);
            //    svA += 5;
            //    g.Dispose();
            //}
        }


        int svA = 0;
        float svf = (float)(Math.PI / 180);
        int svOx = 200;
        int svOy = 300;
        int svr = 50;
        string computePos(int svA1)
        {
            string svCx = "";
            string svCy = "";
            svCx = (svOx + svr * Math.Sin(Math.PI / 180 * svA1)).ToString("##0");
            svCy = (svOy + svr * Math.Cos(Math.PI / 180 * svA1)).ToString("##0");
            return svCx + "," + svCy;
        }



        byte[] obj2binArray(object obj)
        {
            if (obj == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }



        private void cmb_HostIP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_HostIP.Top == txt_serverPublicIP.Top)
            {
                txt_serverPublicIP.Text = cmb_HostIP.Text;
                if (cmb_HostIP.Text.IndexOf(",", 0) != -1) { label1.Text = cmb_HostIP.Text.Split(',')[0].ToString() + " :"; txt_serverPublicIP.Text = cmb_HostIP.Text.Split(',')[1].ToString(); }
                else txt_serverPublicIP.Text = cmb_HostIP.Text;
            }
            else if (cmb_HostIP.Top == txt_HostIP.Top)
            {
                txt_HostIP.Text = cmb_HostIP.Text;
                if (cmb_HostIP.Text.IndexOf(",", 0) != -1) { label1.Text = cmb_HostIP.Text.Split(',')[0].ToString() + " :"; txt_HostIP.Text = cmb_HostIP.Text.Split(',')[1].ToString(); }
                else txt_HostIP.Text = cmb_HostIP.Text;
            }
            cmb_HostIP.Visible = false;
        }

        private void txt_HostIP_MouseUp(object sender, MouseEventArgs e)
        {
            cmb_HostIP.BringToFront();
            cmb_HostIP.Location = txt_HostIP.Location;
            cmb_HostIP.Visible = true;
        }


        private void txt_serverPublicIP_MouseUp(object sender, MouseEventArgs e)
        {
            cmb_HostIP.BringToFront();
            cmb_HostIP.Location = txt_serverPublicIP.Location;
            cmb_HostIP.Visible = true;
        }

        private void cmb_HostIP_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) cmb_HostIP.Visible = false;
            else if (e.KeyCode == Keys.Enter)
            {
                if (cmb_HostIP.Text.Split('.').Length == 4)
                {
                    if (cmb_HostIP.Top == txt_serverPublicIP.Top) txt_serverPublicIP.Text = cmb_HostIP.Text;
                    cmb_HostIP.Visible = false;
                }
            }
        }
        public void _thStop()
        {
            //// trigger stop
            //_shutdownEvent.Set();
            //// if thread suspend, let it resume.
            //_pauseEvent.Set();
            ////_th.Join();
            //_th = null;
        }


        private void tsmnu_check485_Click(object sender, EventArgs e)
        {
            lst_get1.Items.Clear();
            if (mvars.demoMode == false)
            {
                try
                {
                    if (mvars.sp1.IsOpen) mvars.sp1.Close();
                }
                catch (Exception ex)
                {
                    Form1.lstget1.Items.Add(ex.Message);
                    return;
                }

                tslblHW.Text = "Interface"; tslblHW.BackColor = Control.DefaultBackColor; tslblHW.ForeColor = Color.Black;
                string svcom = mp.FindComPort();
                if (svcom != "") tslblCOM.Text = mvars.Comm[0]; else tslblCOM.Text = "";
            }
            else if (mvars.demoMode) lst_get1.Items.Add("Demo mode (COM demo)");
            //MessageBox.Show(Form1.tslblCOM.Text, mvars.strUInameMe + "_v" + mvars.UImajor);
            mp.funSaveLogs("check485 " + mvars.msgA); mvars.msgA = "";
            this.Enabled = true;
        }


        private void lst_get1_DoubleClick(object sender, EventArgs e)
        {
            if (lst_get1.Items[lst_get1.SelectedIndex].ToString().ToUpper().IndexOf("COM",0) != -1 && lst_get1.Items[lst_get1.SelectedIndex].ToString().ToUpper().IndexOf("DEMO", 0) == -1)
            {
                int svi = lst_get1.Items[lst_get1.SelectedIndex].ToString().IndexOf("--", 0);
                tslbl_COM.Text = lst_get1.Items[lst_get1.SelectedIndex].ToString().Substring(0, svi);
                tsmnu_check485.Checked = true;
                tslbl_COM.Text = mvars.Comm[0];
            }
        }

        private void tsmnu_version_Click(object sender, EventArgs e)
        {
            lstget1.Items.Clear();
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "Error".Length) == "Error") { return; }
            #endregion parameter initial

            for (int i = 0; i < 16; i++)
            {

            }
        }

        private void tsmnu_about_Click(object sender, EventArgs e)
        {
            string svmsg = "\r\n" +  "\r\n";
            //svmsg += "\r\n" + "\r\n" + "\r\n" + "\r\n" + "\r\n";
            svmsg += "  v0031 released : 2023 / 12 / 27" + "\r\n" + "             " ;
            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg += "[ Add pecial case of 2x2 FamilyMart content application ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg += "[ add 2x2 FamilyMart 内容應用程序的特例 ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg += "[ 增加 2x2 FamilyMart 内容应用程序的特例 ]"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg += "[ 追加 2x2 FamilyMart コンテンツの特殊なケース ]"; }
            svmsg += "\r\n" + "\r\n" + "  v0032 released : 2024 / 01 / 04" + "\r\n" + "             ";
            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg += "[ Add 2x2 FullHD splice ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg += "[ 增加 2x2 FHD 拼接應用 ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg += "[ 增加 2x2 FHD 拼接应用 ]"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg += "[ 追加 2x2 FullHD 組み合わせ ]"; }
            svmsg += "\r\n" + "\r\n" + "  v0033 released : 2024 / 01 / 10" + "\r\n" + "             ";
            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg += "[ Provide splice combination 8rows and 8columns ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg += "[ 提供最大拼接組合8x8 ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg += "[ 提供最大拼接组合8x8 ]"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg += "[ Provide splice combination 8rows and 8columns ]"; }
            svmsg += "\r\n" + "\r\n" + "  v0034 released : 2024 / 01 / 19" + "\r\n" + "             ";
            if (MultiLanguage.DefaultLanguage == "en-US") 
            { 
                svmsg += @"[ HDMI and Microusb Back wiring display default is "" 1 "" ]" + "\r\n";
                svmsg += "             [ Users connect to the server to download the released firmware ]";
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") 
            { 
                svmsg += @"[ HDMI and Microusb 背面走線顯示預設 is "" 1 "" ]" + "\r\n";
                svmsg += "             [ 使用者連線伺服器下載發行韌體 ]";
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") 
            { 
                svmsg += @"[ HDMI and Microusb 背面走线显示预设 "" 1 "" ]" + "\r\n";
                svmsg += "             [ 使用者连线伺服器下载发行韧体 ]";
            }
            else if (MultiLanguage.DefaultLanguage == "ja-JP")
            { 
                svmsg += @"[ HDMI and Microusb 後ろ配線表示デフォルト "" 1 "" ]" + "\r\n";
                svmsg += "             [ ユーザーはサーバーに接続してファームウェアをダウンロードしてリリースします ]";
            }
            svmsg += "\r\n" + "\r\n" + "  v0035 released : 2024 / 01 / 29" + "\r\n" + "             ";
            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg += "[ Single screen can be set drop by user ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg += "[ 使用者可以自行變更單屏drop ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg += "[ 使用者可以自行变更单屏drop ]"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg += "[ ユーザーが自分でシングルスクリーンドロップを変更できる ]"; }

            svmsg += "\r\n" + "\r\n" + "  v0036 released : 2024 / 02 / 06" + "\r\n" + "             ";
            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg += "[ Users connect to the server to download the released firmware(II) ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg += "[ 使用者連線伺服器下載發行韌體(II) ]"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg += "[ 使用者连线伺服器下载发行韧体(II) ]"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg += "[ ユーザーはサーバーに接続してファームウェアをダウンロードしてリリースします(II) ]"; }

            svmsg += "\r\n" + "\r\n" + "\r\n" + "\r\n" + "\r\n";
            mp.msgBox(mvars.strUInameMe + " v" + mvars.UImajor, svmsg, this.Location.X + 100, this.Location.Y + 100, 500, 300, 6);
        }

        private void tsmnu_ota_Click(object sender, EventArgs e)
        {
            int lstcount = 0;
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "MCU".Length) != "MCU") { h_pictureadjust.Enabled = true; return; }
            #endregion parameter initial


            #region 多語系高級切換
            //ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            //string exatg = resource.GetString("exota");
            //if (exatg != "" && exatg != null) h_exit.Text = exatg;
            #endregion 多語系高級切換

            mvars.strNamePC = Dns.GetHostName();    //TW063192N
            mvars.strNameLogin = Environment.UserName;

            if (mvars.FormShow[11])
            {
                i3init = new i3_Init();
                i3init.Show();
                int svm = i3_Init.dgvformmsg.RowCount;
                i3init.Location = new Point(this.Location.X + 20, this.Location.Y + 70);
                i3init.Text = tsmnu_ota.Text;
                //i3init.TopMost = true;

                DialogResult svr;
                string svmsg = "";
                if (MultiLanguage.DefaultLanguage == "en-US") svmsg = "Prepare to connect to the server  IP : " + txt_serverPublicIP.Text + "\r\n" + "\r\n" + " If you want to broadcast updates, " + "\r\n" + "Please connect each single screen with MicroUSB cable。";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") svmsg = "準備與伺服器連線  IP : " + txt_serverPublicIP.Text + "\r\n" + "\r\n" + " 如果要廣播更新請將各單屏以MicroUSB線串接。";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") svmsg = "准备与伺服器连线  IP : " + txt_serverPublicIP.Text + "\r\n" + "\r\n" + " 如果要广播更新请将各单屏以MicroUSB线串接。";
                else if (MultiLanguage.DefaultLanguage == "ja-JP") svmsg = "Prepare to connect to the server  IP : " + txt_serverPublicIP.Text + "\r\n" + "\r\n" + " If you want to broadcast updates, " + "\r\n" + "Please connect each single screen with MicroUSB cable。";
                svr = mp.msgBox(tsmnu_ota.Text, svmsg, this.Left + 100, this.Top + 200, 250, 130, 4);
                //mvars.actFunc = "ota";
                if (svr == DialogResult.OK)
                {
                    this.Enabled = false;
                    i3_Init.dgvformmsg.Visible = true;
                    serverPublicIP = txt_serverPublicIP.Text;
                    hostIP = txt_HostIP.Text;
                    //mvars.actFunc = "ota";
                    if (MultiLanguage.DefaultLanguage == "en-US")
                        svmsg = "Connecting and authentication .....";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        svmsg = "連線認證中 .....";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        svmsg = "连线认证中 .....";
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        svmsg = "Connecting and authentication .....";
                    mp.showStatus1(svmsg, lst_get1, "");
                    tmeota.Enabled = true;

                    #region 單一nsck
                    if (nsckF != null) nsckF.Dispose();
                    nsckF = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (nsckC != null) nsckC.Dispose();
                    nsckC = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    mp.doDelayms(500);
                    IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), int.Parse(txt_serverPort.Text));

                    //如果socket.connect()函数卡住了，如何停止它？
                    //https://cloud.tencent.com/developer/ask/sof/100136860/answer/102997797
                    using (var nsckC = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(1000);
                            nsckC.Close();
                        });
                        try
                        {
                            nsckC.Connect(ipEnd);
                        }
                        catch (SocketException sex)
                        {
                            if (sex.ErrorCode == 10038)
                            {                              
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                    svmsg = "Error ! Server connection timeout";
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                    svmsg = "錯誤 ! 伺服器連線逾時";
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                    svmsg = "错误 ! 伺服器连线逾时";
                                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                                    svmsg = "間違い ! サーバー接続タイムアウト";
                                mp.showStatus1(svmsg, lst_get1, "");
                                //this.Enabled = true;
                                return;
                            }
                            else
                                throw;
                        }
                    }
                    dts = DateTime.Now;
                    nsckC.Connect(ipEnd);

                    i3_Init.dgvota.Rows[0].Cells[2].Value = "";
                    i3_Init.dgvota.Rows[0].Cells[3].Value = "";
                    i3_Init.dgvota.Rows[0].Cells[4].Value = "";
                    mvars.flgValidate = false;

                    if (MultiLanguage.DefaultLanguage == "en-US")
                        svmsg = "Connected";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        svmsg = "已連線";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        svmsg = "已连线";
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        svmsg = "Connected";
                    mp.showStatus1(svmsg, lst_get1, "");

                    lbl_te.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + " ~ ";

                    #endregion 單一nsck

                }
                else if (svr == DialogResult.Cancel)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US")
                        svmsg = "User break";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        svmsg = "使用者已中斷";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        svmsg = "使用者已中断";
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        svmsg = "User break";
                    mp.showStatus1(svmsg, lst_get1, "");
                    i3init.Close();
                }
            }
            else
            {
                h_project.Enabled = false;
                h_tool.Enabled = false;
                h_user.Enabled = false;
                h_lan.Enabled = false;
                h_pictureadjust.Enabled = false;
                h_screenconfig.Enabled = false;

                lbl_timer1.Text = "0";
                lbl_chc.Text = "0";
                lbl_pocketsize.Text = "0";
                lbl_counter.Text = "0";
                lbl_getmods.Text = "0";
                dgvota.Rows[0].Cells[10].Value = "";

                #region 多語系高級切換
                //string svs2 = "";
                //if (MultiLanguage.DefaultLanguage == "en-US")
                //    svs2 = "Exit Firmware Update (&E)";
                //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //    svs2 = "離開 韌體更新 (&E)";
                //else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //    svs2 = "离开 韧体更新 (&E)";
                //else if (MultiLanguage.DefaultLanguage == "ja-JP")
                //    svs2 = "閉じる ファームウェアアップデート (&E)";
                //h_exit.Text = svs2;

                ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
                string exatg = resource.GetString("exota");
                if (exatg != "" && exatg != null) h_exit.Text = exatg;
                #endregion 多語系高級切換

                dgvformmsg.Visible = false;
                dgvformmsg.DataSource = null;
                dgvformmsg.Rows.Clear();
                Skeleton_formmsg(2, 888, 1, 1);
                dgvformmsg.Visible = true;
                mp.showStatus1("Welcome", lst_get1, "");
                dgvota.Visible = true;
                lst_get1.Visible = false;
                lst_get1.Items.Clear();
                //lst_get1.Top = dgvota.Top + dgvota.Height + 30;

                DialogResult svr;
                string svmsg = "";
                if (MultiLanguage.DefaultLanguage == "en-US") svmsg = "Prepare to connect to the server  IP : " + txt_serverPublicIP.Text + "\r\n" + "\r\n" + " If you want to broadcast updates, " + "\r\n" + "Please connect each single screen with MicroUSB cable。";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") svmsg = "準備與伺服器連線  IP : " + txt_serverPublicIP.Text + "\r\n" + "\r\n" + " 如果要廣播更新請將各單屏以MicroUSB線串接。";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") svmsg = "准备与伺服器连线  IP : " + txt_serverPublicIP.Text + "\r\n" + "\r\n" + " 如果要广播更新请将各单屏以MicroUSB线串接。";
                else if (MultiLanguage.DefaultLanguage == "ja-JP") svmsg = "Prepare to connect to the server  IP : " + txt_serverPublicIP.Text + "\r\n" + "\r\n" + " If you want to broadcast updates, " + "\r\n" + "Please connect each single screen with MicroUSB cable。";
                svr = mp.msgBox(tsmnu_ota.Text, svmsg, this.Left + 100, this.Top + 200, 250, 130, 4);
                mvars.actFunc = "ota";
                if (svr == DialogResult.OK)
                {
                    serverPublicIP = txt_serverPublicIP.Text;
                    hostIP = txt_HostIP.Text;
                    h_exit.Enabled = false;
                    mvars.actFunc = "ota";
                    if (MultiLanguage.DefaultLanguage == "en-US")
                        svmsg = "Connecting and authentication .....";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        svmsg = "連線認證中 .....";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        svmsg = "连线认证中 .....";
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        svmsg = "Connecting and authentication .....";
                    mp.showStatus1(svmsg, lst_get1, "");


                    #region 單一nsck
                    if (nsckF != null) nsckF.Dispose();
                    nsckF = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (nsckC != null) nsckC.Dispose();
                    nsckC = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    mp.doDelayms(500);
                    IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), int.Parse(txt_serverPort.Text));

                    //如果socket.connect()函数卡住了，如何停止它？
                    //https://cloud.tencent.com/developer/ask/sof/100136860/answer/102997797
                    using (var nsckC = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(1000);
                            nsckC.Close();
                        });
                        try
                        {
                            nsckC.Connect(ipEnd);
                        }
                        catch (SocketException sex)
                        {
                            if (sex.ErrorCode == 10038)
                            {
                                Form1.dgvformmsg.Rows.Clear();
                                DataGridViewRowCollection rows = dgv_formmsg.Rows;
                                for (int SvR = 0; SvR < 888; SvR++)
                                {
                                    rows.Add();
                                    DataGridViewRow row = dgv_formmsg.Rows[SvR]; row.Height = 18;
                                }
                                if (MultiLanguage.DefaultLanguage == "en-US")
                                    svmsg = "Error ! Server connection timeout";
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                                    svmsg = "錯誤 ! 伺服器連線逾時";
                                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                                    svmsg = "错误 ! 伺服器连线逾时";
                                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                                    svmsg = "間違い ! サーバー接続タイムアウト";
                                mp.showStatus1(svmsg, lst_get1, "");

                                lst_get1.Items.Clear();
                                lst_get1.SetBounds(12, 41, 627, 112);
                                h_user.Enabled = true;
                                h_exit.Enabled = true;
                                return;
                            }
                            else
                                throw;
                        }
                    }
                    dts = DateTime.Now;
                    nsckC.Connect(ipEnd);

                    dgvota.Rows[0].Cells[2].Value = "";
                    dgvota.Rows[0].Cells[3].Value = "";
                    dgvota.Rows[0].Cells[4].Value = "";
                    mvars.flgValidate = false;

                    if (MultiLanguage.DefaultLanguage == "en-US")
                        svmsg = "Connected";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        svmsg = "已連線";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        svmsg = "已连线";
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        svmsg = "Connected";
                    mp.showStatus1(svmsg, lst_get1, "");

                    lbl_te.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + " ~ ";

                    if (lstcount > 0) lst_get1.Items.RemoveAt(lstcount);
                    lst_get1.Items.Add("");
                    lstcount = lst_get1.Items.Count - 1;

                    Form1.nvsendercls_p(ref Form1.nvsender, 1, 1);      // 485共有多少單屏串接
                    Form1.nvsender[0].regPoCards = 16;                  // 預設最大數16個屏

                    tmeota.Enabled = true;
                    tslbl_timer1.Visible = true;
                    tslbl_timer1spr.Visible = true;
                    tslbl_chc.Visible = true;
                    tslbl_chcspr.Visible = true;
                    tslbl_Sxd.Visible = true;
                    tslbl_Sxdspr.Visible = true;
                    #endregion 單一nsck

                    #region 因應單一nsck而關閉
                    //mp.doDelayms(1500);
                    //for (byte svid = 0; svid < 5; svid++)
                    //{
                    //    txt_serverPort.Text = (6688 + svid * 2).ToString();
                    //    Application.DoEvents();

                    //    IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(serverPublicIP), int.Parse(txt_serverPort.Text));
                    //    nsckC = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //    nsckC.Connect(ipEnd);
                    //    byte svi = 0;
                    //    do
                    //    {
                    //        mp.doDelayms(200);
                    //        svi++;
                    //    }
                    //    while (svi < 25);
                    //    if (nsckC.Connected == true && nsckC.Poll(0, SelectMode.SelectRead)) break;
                    //    nsckC.Close();
                    //}
                    //dgvota.Rows[0].Cells[2].Value = "";
                    //dgvota.Rows[0].Cells[3].Value = "";
                    //dgvota.Rows[0].Cells[4].Value = "";

                    ////lbl_timer1.Text = "0";
                    //mvars.flgValidate = false;
                    //if (nsckC.Connected == false)
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US")
                    //        svmsg = "Server is full or does not exist";
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    //        svmsg = "伺服器滿線或不存在";
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    //        svmsg = "伺服器满线或不存在";
                    //    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                    //        svmsg = "Server is full or does not exist";
                    //    mp.showStatus1(svmsg, lst_get1, "");

                    //    h_project.Enabled = !h_project.Enabled;
                    //    h_pid.Enabled = !h_pid.Enabled;
                    //    h_tool.Enabled = !h_tool.Enabled;
                    //    h_user.Enabled = !h_user.Enabled;
                    //    h_lan.Enabled = !h_lan.Enabled;
                    //    h_pictureadjust.Visible = false;
                    //    h_screenconfig.Visible = false;
                    //    tsmnu_ota.Visible = true;
                    //    txt_serverPort.Text = "6688";
                    //    return;
                    //}
                    //else
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US")
                    //        svmsg = "Connected";
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    //        svmsg = "已連線";
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    //        svmsg = "已连线";
                    //    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                    //        svmsg = "Connected";
                    //    mp.showStatus1(svmsg, lst_get1, "");

                    //    lbl_te.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + " ~ ";

                    //    if (lstcount > 0) lst_get1.Items.RemoveAt(lstcount);
                    //    lst_get1.Items.Add("");
                    //    lstcount = lst_get1.Items.Count - 1;

                    //    Form1.nvsendercls_p(ref Form1.nvsender, 1, 1);      // 485共有多少單屏串接
                    //    Form1.nvsender[0].regPoCards = 16;                  // 預設最大數16個屏

                    //    tmeota.Enabled = true;
                    //    tslbl_timer1.Visible = true;
                    //    tslbl_timer1spr.Visible = true;
                    //    tslbl_chc.Visible = true;
                    //    tslbl_chcspr.Visible = true;
                    //    tslbl_Sxd.Visible = true;
                    //    tslbl_Sxdspr.Visible = true;
                    //}
                    #endregion 因應單一nsck而關閉
                }
                else if (svr == DialogResult.Cancel)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US")
                        svmsg = "User break";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        svmsg = "使用者已中斷";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        svmsg = "使用者已中断";
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                        svmsg = "User break";
                    mp.showStatus1(svmsg, lst_get1, "");
                    lst_get1.Items.Clear();
                    lst_get1.SetBounds(12, 41, 627, 112);
                    dgvformmsg.Visible = false;
                    dgvota.Visible = false;
                    h_exit_Click(null, null);
                }
            }
        }

        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days.ToString() + "D" + ts.Hours.ToString() + "H" + ts.Minutes.ToString() + "M" + ts.Seconds.ToString();
            return dateDiff;
        }

        private void tsmnu_drop_Click(object sender, EventArgs e)
        {
            lstget1.Items.Clear();
            #region parameter initial
            mvars.flgSelf = true;
            mp.pidinit();
            if (mvars.demoMode == false && chk_formsize.Checked && Form1.tslblStatus.Text.Substring(0, "MCU".Length) != "MCU") { h_pictureadjust.Enabled = true; return; }
            #endregion parameter initial

            mvars.lstmcuW62000 = lst_mcuW62000;
            mvars.lstmcuR62000 = lst_mcuR62000;

            lst_mcuW62000.Items.Clear();
            lst_mcuW62000.Items.AddRange(mvars.uiregadrdefault.Split('~'));
            if (lst_mcuW62000.Items.Count == 0) { return; }

            int i;
            string value = mvars.dropvalue;
            string svs1 = "";

            if (MultiLanguage.DefaultLanguage == "en-US") svs1 = @"    Input Drop GaryScale between 0 to 255";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") svs1 = @"    請輸入 Drop 介於 0 到 255 之間的灰階";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") svs1 = @"    请输入 Drop 介于 0 到 255 之间的灰阶";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") svs1 = @"    0 ～ 255 の間のグレースケールを入力してください";

            RECT rct;
            GetWindowRect(mvars.handleIDMe, out rct);
            //myRect.X = rct.Left;
            //myRect.Y = rct.Top;
            //myRect.Width = rct.Right - rct.Left + 1;
            //myRect.Height = rct.Bottom - rct.Top + 1;
            if (mp.InputBox("", svs1, ref value, rct.Left + 100, rct.Top + 100, 0, 0, 1, "") == DialogResult.Cancel) { return; }
            else
            {
                if (mp.IsNumeric(value) && mvars.dropvalue.Split(',').Length == 1) mvars.dropvalue = value;
                else return;
            }

            #region 燈板Gamma可調(工程模式) (on : 1 / off : 0)
            int svms = 0;               //Primary
            int svme = 1;               //Primary
            for (i = 5; i <= 5; i++)
            {
                string[] svs0 = lst_mcuW62000.Items[i].ToString().Split(',');
                lst_mcuW62000.Items.RemoveAt(i);
                string svss1 = svs0[0] + ",1";
                lst_mcuW62000.Items.Insert(i, svss1);
            }
            svms = 91;      /// Primary
            svme = 283;     /// Primary
            for (i = 0; i < mvars.uiregadr_default.Length; i++)
            {
                if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
            }
            for (i = svms; i < mvars.uiregadr_default.Length; i++)
            {
                if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
            }
            //lbl_GMAdrop.Text = svme.ToString();
            for (i = svms; i < svme; i++)
            {
                string[] svs0 = lst_mcuW62000.Items[i].ToString().Split(',');
                lst_mcuW62000.Items.RemoveAt(i);
                string svss1 = svs0[0] + "," + (Convert.ToInt16(svs0[1]) * Convert.ToInt16(mvars.dropvalue) / 255).ToString();
                lst_mcuW62000.Items.Insert(i, svss1);
            }


            svms = 0;               //Primary
            svme = 1;               //Primary
            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            string[] sRegDec;           //addr
            string[] sDataDec;          //data
            string svs;
            svs = "MCU Flash 0x62000 FlashWrite,";
            lst_get1.Items.Add(svs);
            markreset(3, false, true);
            string svmsg = "";
            /*
                Primary MCU Flash 
                0x62000採特殊方式 切 FPGA A4096 / FPGA B4096 Total = 8192
                如果只有single duty則是填 0~6 , 10~282 = 283bytes
                如果是dual duty則是填 0~6 , 10~282 , 300~572 = 572bytes
                0x64000則不分區，所以僅填單側0~39個 = 40bytes
             */

            #region 清空 MCU Flash 用
            if (lst_mcuW62000.Items.Count <= 0)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x62000" + "\r\n" + "\r\n" + @"""OK""  Earse MCU flash and " + cmbFPGAsel.Text + " UIREG restore default"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x62000 沒有內容" + "\r\n" + "\r\n" + @"""OK""  繼續進行清空微控閃存並且將 UIREG 還原預設值"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x62000 没有内容" + "\r\n" + "\r\n" + @"""OK""  继续进行清空微控闪存  UIREG 还原预设值"; }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = "0x62000には値がありません" + "\r\n" + "\r\n" + @"""OK""  引き続きマイクロコントローラーのフラッシュメモリをクリアし UIREG デフォルトに戻す"; }
                // GetWindowRec 取得顯示表單位置
                GetWindowRect(mvars.handleIDMe, out rct);
                if (mp.InputBox("", svmsg, ref value, rct.Left + 100, rct.Top + 100, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                mvars.lblCmd = "MCUFLASH_W62000";
                mp.mhMCUFLASHWRITE("62000".PadLeft(8, '0'), ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                {
                    Form1.svuiregadr[j] = mvars.uiregadr_default[j].Split(',')[2];
                    Form1.svuiregadr[j + (int)(mvars.GAMMA_SIZE / 8)] = mvars.uiregadr_default[j].Split(',')[2];
                }

                for (i = 0; i < mvars.uiregadr_default.Length; i++)
                {
                    if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                }
                for (i = svms; i < mvars.uiregadr_default.Length; i++)
                {
                    if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                }
                //lbl_GMAdrop.Text = svme.ToString();

                sRegDec = new string[svme];   //addr
                sDataDec = new string[svme];  //data
                for (i = 0; i < svme; i++)
                {
                    sRegDec[i] = i.ToString();
                    sDataDec[i] = Form1.svuiregadr[i];
                }
                mvars.lblCmd = "FPGA_REG_W";
                mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
                else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
                this.Enabled = true;
                return;
            }
            #endregion

            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "Brightness drop write" + "\r\n" + "\r\n" + "Note! Single-screen fine-tuning data will be replaced by drop"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "亮度drop 寫入" + "\r\n" + "\r\n" + "注意 ! 單屏微調的數據將會被drop取代"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " 亮度drop 写入" + "\r\n" + "\r\n" + "注意 ! 单屏微调的数据将会被drop取代"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = "明るさドロップ書き込み" + "\r\n" + "\r\n" + "注意: 単一画面の微調整データはドロップによって置き換えられます"; }
            // GetWindowRec 取得顯示表單位置
            GetWindowRect(mvars.handleIDMe, out rct);
            if (mp.InputBox("", svmsg, ref value, rct.Left + 100, rct.Top + 100, 0, 0, 2, "") == DialogResult.Cancel) { return; }


            #region 與sendmessage共用
            //if (chkNVBC.Checked)
            //    if (MessageBox.Show(chkNVBC.Text, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) return;

            if (mvars.svnova == false && mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            if (mvars.dualduty == 1) svme = mvars.uiregadr_default.Length;
            else
            {
                for (i = 0; i < mvars.uiregadr_default.Length; i++)
                {
                    if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                }
                for (i = svms; i < mvars.uiregadr_default.Length; i++)
                {
                    if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                }
                //lbl_GMAdrop.Text = svme.ToString();
            }
            sRegDec = new string[svme];   //addr
            sDataDec = new string[svme];  //data
            for (i = 0; i < svme; i++)
            {
                sRegDec[i] = lst_mcuW62000.Items[i].ToString().Split(',')[0];
                sDataDec[i] = lst_mcuW62000.Items[i].ToString().Split(',')[1];
            }
            this.Enabled = false;
            mvars.lblCmd = "MCUFLASH_W62000";
            svs = "MCU Flash 0x62000 FlashWrite,";
            lst_get1.Items.Add(svs + " ....");
            mp.doDelayms(10);
            mp.cUIREGADRwENG("62000", sRegDec, sDataDec);

            //if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
            //else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
            this.Enabled = true;
            lstget1.Items.Clear();
            //string[] svss = mvars.strReceive.Split(',');
            //lst_get1.Items.Add("  ↑，" + svss[7] + "s");
            //lst_get1.TopIndex = lst_get1.Items.Count - 1;
            #endregion 與sendmessage共用
            #endregion
        }
    }




    #region 參考 C# Socket大檔案傳輸
    //https://home.gamer.com.tw/creationDetail.php?sn=3612377

    //傳送端
    //private void button1_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        IPAddress ipAddress = IPAddress.Parse("192.168.1.103");
    //        IPEndPoint ipEnd = new IPEndPoint(ipAddress, 5656);
    //        foreach (string fname in System.IO.Directory.GetFiles("test_folder"))
    //        {
    //            SocketclientSock = new
    //            Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
    //            clientSock.Connect(ipEnd);
    //            string fileName = System.Windows.Forms.Application.StartupPath;
    //            //string filePath = "\\test_folder\\test0605.txt";  
    //            byte[] fileNameByte = Encoding.ASCII.GetBytes(/*filePath*/"\\" + fname);
    //            //將檔案路徑與名稱轉為位元組  

    //            byte[] fileData = File.ReadAllBytes(fileName + "\\" + fname);
    //            //把指定路徑讀取到的檔案轉成BYTE  
    //            byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
    //            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
    //            //將檔案路徑與名稱位元組編碼成位元組序列  

    //            fileNameLen.CopyTo(clientData, 0); //將檔案路徑與名稱加入 clientData  
    //            fileNameByte.CopyTo(clientData, 4); //將檔案路徑與名稱加入 clientData  
    //            fileData.CopyTo(clientData, 4 + fileNameByte.Length);
    //            //將指定路徑讀取到資料寫入clientData  
    //            clientSock.Send(clientData); //送出檔案封包  
    //            clientSock.Close();
    //            //Thread.Sleep(5000);  
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show("File Sending fail." + ex.Message);
    //    }
    //}

    //private void button2_Click(object sender, EventArgs e)
    //{
    //    //string supportedExtensions = "*.txt,*.jpg,*.exe";  
    //    int count = 0;
    //    string supportedExtensions;
    //    supportedExtensions = "*." + textBox1.Text;

    //    if (textBox1.Text == "")
    //    {
    //        //取得資料夾內指定的所有檔案  
    //        foreach (string fname in System.IO.Directory.GetFiles("test_folder"))
    //        {
    //            MessageBox.Show(fname);
    //        }
    //    }
    //    else
    //    {
    //        //取得資料夾內指定類型的檔案  
    //        foreach (string fname in
    //       System.IO.Directory.GetFiles("test_folder", "*.*", SearchOption.AllDirectories).
    //       Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower())))
    //        {
    //            count++;
    //            MessageBox.Show(fname);
    //        }

    //        if (count == 0)
    //        {
    //            MessageBox.Show("No Match File");
    //        }
    //    }
    //}



    //接收端
    //private void updateui(Control ctl)
    //{
    //    if (ctl.InvokeRequired)
    //    {
    //        ChangeUI ui = new ChangeUI(updateui);
    //        ctl.Invoke(ui, ctl);
    //    }
    //    else
    //    {
    //        ctl.Visible = true;
    //    }
    //}

    //private void updateui2(Control ctl)
    //{
    //    if (ctl.InvokeRequired)
    //    {
    //        ChangeUI ui = new ChangeUI(updateui2);
    //        ctl.Invoke(ui, ctl);
    //    }
    //    else
    //    {
    //        ctl.Visible = false;
    //    }
    //}

    //private void updateuiText(Control ctl, string str)
    //{
    //    if (ctl.InvokeRequired)
    //    {
    //        ChangeUIText ui = new ChangeUIText(updateuiText);
    //        ctl.Invoke(ui, ctl, str);
    //    }
    //    else
    //    {
    //        ctl.Text = str;
    //    }
    //}

    //private void listen_data()
    //{
    //    try
    //    {
    //        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 5656);
    //        Socket sock = new Socket(AddressFamily.InterNetwork,
    //        SocketType.Stream, ProtocolType.IP);
    //        int receivedBytesLen;
    //        int recieve_data_size;
    //        int fileNameLen;
    //        string fileName;
    //        int count = 0;
    //        sock.Bind(ipEnd);
    //        sock.Listen(10);

    //        while (true)
    //        {
    //            int first = 1;
    //            receivedBytesLen = 0;
    //            recieve_data_size = 0;
    //            fileNameLen = 0;
    //            fileName = "";
    //            Socket clientSock = sock.Accept();
    //            byte[] clientData = new byte[1024 * 50000];
    //            //5000 = 5MB 50000 = 50MB //定義傳輸每段資料大小，值越大傳越快  
    //            //byte[] clientData = new byte[8192];  
    //            string receivedPath = System.Windows.Forms.Application.StartupPath;
    //            BinaryWriter bWrite = null;
    //            MemoryStream ms = null;
    //            string file_type = "";
    //            string display_data = "";
    //            string content = "";
    //            double cal_size = 0;

    //            do
    //            {
    //                receivedBytesLen = clientSock.Receive(clientData);
    //                //接收資料 (receivedBytesLen = 資料長度)  

    //                if (first == 1) //第一筆資料為檔名  
    //                {
    //                    fileNameLen = BitConverter.ToInt32(clientData, 0);
    //                    //轉換檔名的位元組為整數 (檔名長度)  
    //                    fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
    //                    // 1 int = 4 byte  轉換Byte為字串  
    //                    file_type = fileName.Substring(fileName.Length - 3, 3);
    //                    //取得檔名  
    //                    //-----------  
    //                    content = Encoding.ASCII.GetString(clientData, 4 +
    //                    fileNameLen, receivedBytesLen - 4 - fileNameLen);
    //                    //取得檔案內容 起始(檔名以後) 長度(扣除檔名長度)  
    //                    display_data += content;
    //                    //-----------  
    //                    bWrite = new BinaryWriter(File.Open(receivedPath +
    //                    fileName, FileMode.Create));
    //                    //CREATE 覆蓋舊檔 APPEND 延續舊檔  
    //                    ms = new MemoryStream();
    //                    bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 -
    //                    fileNameLen);
    //                    //寫入資料 ，跳過起始檔名長度，接收長度減掉檔名長度  
    //                    ms.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 -
    //                    fileNameLen);
    //                    //寫入資料 ，呈現於BITMAP用  
    //                }
    //                else //第二筆接收為資料  
    //                {
    //                    //-----------  
    //                    content = Encoding.ASCII.GetString(clientData, 0,
    //                    receivedBytesLen);
    //                    display_data += content;
    //                    //-----------  
    //                    bWrite.Write(clientData/*, 4 + fileNameLen, receivedBytesLen - 4 -
    //                        fileNameLen*/, 0, receivedBytesLen);
    //                    //每筆接收起始 0 結束為當次Receive長度  
    //                    ms.Write(clientData, 0, receivedBytesLen);
    //                    //寫入資料 ，呈現於BITMAP用  
    //                }
    //                recieve_data_size += receivedBytesLen;
    //                //計算資料每筆資料長度並累加，後面可以輸出總值看是否有完整接收  
    //                cal_size = recieve_data_size;
    //                cal_size /= 1024;
    //                cal_size = Math.Round(cal_size, 2);
    //                updateuiText(textBox1, cal_size.ToString());
    //                first++;
    //                Thread.Sleep(10); //每次接收不能太快，否則會資料遺失  
    //            } while (clientSock.Available != 0); //如果還沒接收完則繼續接收  
    //                                                 //while(receivedBytesLen != 0);  

    //            updateuiText(textBox2, fileName);

    //            if (file_type == "jpg") //如果是圖則呈現在視窗上  
    //            {
    //                //pictureBox1.Visible = true;  
    //                //richTextBox1.Visible = false;  
    //                updateui(pictureBox1);
    //                updateui2(richTextBox1);
    //                Bitmap Img = new Bitmap(ms);
    //                Bitmap imageOut = new Bitmap(Img, 1200, 600);
    //                pictureBox1.Image = imageOut;
    //            }
    //            else
    //            {
    //                //pictureBox1.Visible = false;  
    //                //richTextBox1.Visible = true;  
    //                updateui(richTextBox1);
    //                updateui2(pictureBox1);
    //                //richTextBox1.Text = display_data;  
    //                updateuiText(richTextBox1, display_data);
    //            }
    //            Thread.Sleep(3000);
    //            ms.Close();
    //            count++;
    //            first = 1;
    //            bWrite.Close();
    //            clientSock.Close();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //MessageBox.Show("File Receiving fail." + ex.Message);  
    //    }
    //}

    #endregion 參考 C# Socket大檔案傳輸







    public class TransparentPanel : Panel
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }
    }


}
