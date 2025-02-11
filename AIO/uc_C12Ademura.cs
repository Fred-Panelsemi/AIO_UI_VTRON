using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;   // GetWindowRec 取得顯示表單位置
using System.IO;

namespace AIO
{
    

    public partial class uc_C12Ademura : UserControl
    {
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


        public bool svFPGALHL = false;
        public static bool svnova = false;

        public static bool[] cmbhClick = { false, false, false, false, false };
        public static byte cmbhClickindex = 4;
        public static byte[] cmbhSelectedIndex = { 0, 0, 0, 0, 0 };
        public static string[] cmbhContent = { " Enable", " Disable" };
        public static CheckBox chkNVBC = null;
        public static CheckBox chkrealtime = null;
        public static ComboBox cmbdeviceid = null;
        public static ComboBox cmbFPGAsel = null;


        private void InitForm()
        {
            if (mvars.deviceID.Substring(0,2) == "05")
            {
                Array.Resize(ref mvars.chkomsel, 8);
                mvars.chkomsel[0] = chk_pFRC;
                mvars.chkomsel[1] = chk_pWTtile;
                mvars.chkomsel[2] = chk_pDMRsingle;
                mvars.chkomsel[3] = chk_pWTsingle;
                mvars.chkomsel[4] = chk_pDMRmulti;
                mvars.chkomsel[5] = chk_pWTmulti;
                mvars.chkomsel[6] = chk_pUIen;
                mvars.chkomsel[7] = chk_pALLdis;
                mvars.chkomsel[0].Tag = "12";
                mvars.chkomsel[1].Tag = "13";
                mvars.chkomsel[2].Tag = "14";
                mvars.chkomsel[3].Tag = "15";
                mvars.chkomsel[4].Tag = "16";
                mvars.chkomsel[5].Tag = "17";
                mvars.chkomsel[6].Tag = "18";
                mvars.chkomsel[7].Tag = "63";
            }


            ToolTip toolTip1 = new ToolTip();
            toolTip1.SetToolTip(chk_WTPGONOFF, "Checked = Enable");
            toolTip1.SetToolTip(chk_RealTime, "Checked = RealTime Effect");

            mvars.numUDWTR = numUD_WTR;
            mvars.numUDWTG = numUD_WTG;
            mvars.numUDWTB = numUD_WTB;
            mvars.chkWTONOFF = chk_WTPGONOFF;

            mvars.numUDegma = numUD_egma;
            mvars.cmbegma = cmb_egma;

            mvars.lstmcuW60000 = lst_mcuW60000;
            mvars.lstmcuR60000 = lst_mcuR60000;
            mvars.lstmcuW62000 = lst_mcuW62000;
            mvars.lstmcuR62000 = lst_mcuR62000;
            mvars.lstmcuW64000 = lst_mcuW64000;
            mvars.lstmcuR64000 = lst_mcuR64000;
            mvars.lstmcuW66000 = lst_mcuW66000;
            mvars.lstmcuR66000 = lst_mcuR66000;
            //mvars.lstmcuW68000 = lst_mcuW68000;
            //mvars.lstmcuR68000 = lst_mcuR68000;

            mvars.btnmcuW60000 = btn_mcuW60000;
            mvars.btnmcuR60000 = btn_mcuR60000;
            mvars.btnmcuW60000cls = btn_mcuW60000cls;
            mvars.btnmcuR60000cls = btn_mcuR60000cls;

            mvars.btnmcuW62000 = btn_mcuW62000;
            mvars.btnmcuR62000 = btn_mcuR62000;
            mvars.btnmcuW62000cls = btn_mcuW62000cls;
            mvars.btnmcuR62000cls = btn_mcuR62000cls;

            mvars.btnmcuW64000 = btn_mcuW64000;
            mvars.btnmcuR64000 = btn_mcuR64000;
            mvars.btnmcuW64000cls = btn_mcuW64000cls;
            mvars.btnmcuR64000cls = btn_mcuR64000cls;

            mvars.btnmcuW66000 = btn_mcuW66000;
            mvars.btnmcuR66000 = btn_mcuR66000;
            mvars.btnmcuW66000cls = btn_mcuW66000cls;
            mvars.btnmcuR66000cls = btn_mcuR66000cls;

            //mvars.btnmcuW68000 = btn_mcuW68000;
            //mvars.btnmcuR68000 = btn_mcuR68000;
            //mvars.btnmcuW68000cls = btn_mcuW68000cls;
            //mvars.btnmcuR68000cls = btn_mcuR68000cls;

            chkNVBC = chk_NVBC;
            chkrealtime = chk_RealTime;

            cmbdeviceid = cmb_deviceID;
            cmbFPGAsel = cmb_FPGAsel;
        }

        public uc_C12Ademura()
        {
            InitializeComponent();
            InitForm();
        }

        private void uc_C12Ademura_Load(object sender, EventArgs e)
        {
            



            mvars.toolTip1.AutoPopDelay = 3000;
            mvars.toolTip1.InitialDelay = 500;
            mvars.toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            mvars.toolTip1.ShowAlways = true;
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (MultiLanguage.DefaultLanguage == "en-US") 
                {
                    mvars.toolTip1.SetToolTip(btn_mcuR60000, @"Read format：0~1023(Reg no.,FPGA A)，1024~2047(Reg no.,FPGA B)");
                    mvars.toolTip1.SetToolTip(btn_mcuW60000, @"Single table data，please confirm ""FPGA sel."" before Write");

                    mvars.toolTip1.SetToolTip(btn_mcuW62000cls, "def：Single table data");
                    mvars.toolTip1.SetToolTip(btn_mcuR62000, @"Read format：0~1023(Reg no.,FPGA A)，1024~2047(Reg no.,FPGA B)");
                    mvars.toolTip1.SetToolTip(btn_mcuW62000, @"Single table data，please confirm ""FPGA sel."" before Write");

                    mvars.toolTip1.SetToolTip(btn_mcuW64000cls, "def：Single table data");
                    mvars.toolTip1.SetToolTip(btn_mcuR64000, @"Read format：0~511(Reg no.,single table data (non left and right))");
                    mvars.toolTip1.SetToolTip(btn_mcuW64000, @"Single table data，please confirm ""FPGA sel."" before Write");

                    mvars.toolTip1.SetToolTip(btn_uiregadrclsdef, "def：Reg no.,FPGA A data~FPGA Bdata");    //，寫入時須搭配 FPGA選擇
                    mvars.toolTip1.SetToolTip(btn_uiregadrread0x12, "Read format：Reg no.,FPGA A data~FPGA B data");
                    mvars.toolTip1.SetToolTip(btn_uiregadrreadRADWRb, "Read format：Reg no.,FPGA A data~FPGA B data");
                    label26.Text = "Hardware ID                  MCU 0x60000       MCU 0x62000        MCU 0x64000       Reg Table(realtime)            MCU 0x66000(Notes)";
                    label24.Text = "FPGA Sel.";
                    chk_NVBC.Text = "Boardcast";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    mvars.toolTip1.SetToolTip(btn_mcuR60000, @"回讀格式：0~1023(暫存器編號,右畫面內容)，1024~2047(暫存器編號,左畫面內容)");
                    mvars.toolTip1.SetToolTip(btn_mcuW60000, @"單一表格資料，寫入前請先確認""FPGA選擇""");

                    mvars.toolTip1.SetToolTip(btn_mcuW62000cls, "def：單一表格資料");
                    mvars.toolTip1.SetToolTip(btn_mcuR62000, @"回讀格式：0~1023(暫存器編號,右畫面內容)，1024~2047(暫存器編號,左畫面內容)");
                    mvars.toolTip1.SetToolTip(btn_mcuW62000, @"單一表格資料，寫入前請先確認""FPGA選擇""");

                    mvars.toolTip1.SetToolTip(btn_mcuW64000cls, "def：單一表格資料");
                    mvars.toolTip1.SetToolTip(btn_mcuR64000, @"回讀格式：0~511(暫存器編號,表格資料(不分左右畫面))");
                    mvars.toolTip1.SetToolTip(btn_mcuW64000, @"單一表格資料，寫入前請先確認""FPGA選擇""");

                    mvars.toolTip1.SetToolTip(btn_uiregadrclsdef, "def：暫存器編號,右畫面內容~左畫面內容");    //，寫入時須搭配 FPGA選擇
                    mvars.toolTip1.SetToolTip(btn_uiregadrread0x12, "回讀格式：暫存器編號,右畫面內容~左畫面內容");
                    mvars.toolTip1.SetToolTip(btn_uiregadrreadRADWRb, "回讀格式：暫存器編號,右畫面內容~左畫面內容");
                    label26.Text = "硬體ID                            MCU 0x60000       MCU 0x62000        MCU 0x64000       Reg Table(即時)                  MCU 0x66000(訊息)";
                    label24.Text = "FPGA選擇";
                    chk_NVBC.Text = "大屏廣播";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    mvars.toolTip1.SetToolTip(btn_mcuR60000, @"回读格式：0~1023(暫存器編號,右画面内容)，1024~2047(暫存器編號,左画面内容)");
                    mvars.toolTip1.SetToolTip(btn_mcuW60000, @"单一表格资料，写入前请先确认""FPGA选择""");

                    mvars.toolTip1.SetToolTip(btn_mcuW62000cls, "def：单一表格资料");
                    mvars.toolTip1.SetToolTip(btn_mcuR62000, @"回读格式：0~1023(暫存器編號,右画面内容)，1024~2047(暫存器編號,左画面内容)");
                    mvars.toolTip1.SetToolTip(btn_mcuW62000, @"单一表格资料，写入前请先确认""FPGA选择""");
                   
                    mvars.toolTip1.SetToolTip(btn_mcuW64000cls, "def：单一表格资料");
                    mvars.toolTip1.SetToolTip(btn_mcuR64000, @"回读格式：0~511(暫存器編號,表格资料(不分左右画面))");
                    mvars.toolTip1.SetToolTip(btn_mcuW64000, @"单一表格资料，写入前请先确认""FPGA选择""");
                    
                    mvars.toolTip1.SetToolTip(btn_uiregadrclsdef, "def：暂存器编号,右画面内容~左画面内容");    //，寫入時須搭配 FPGA選擇
                    mvars.toolTip1.SetToolTip(btn_uiregadrread0x12, "回读格式：暂存器编号,右画面内容~左画面内容");
                    mvars.toolTip1.SetToolTip(btn_uiregadrreadRADWRb, "回读格式：暂存器编号,右画面内容~左画面内容");
                    label26.Text = "硬体ID                            MCU 0x60000       MCU 0x62000        MCU 0x64000       Reg Table(即时)                  MCU 0x66000(讯息)";
                    label24.Text = "FPGA选择";
                    chk_NVBC.Text = "大屏广播";
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "10")
            {
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    mvars.toolTip1.SetToolTip(btn_mcuR60000, @"Read format：0~1023(Reg no.,FPGA A)，1024~2047(Reg no.,FPGA B)");
                    mvars.toolTip1.SetToolTip(btn_mcuW60000, @"Single table data，please confirm ""FPGA sel."" before Write");

                    mvars.toolTip1.SetToolTip(btn_mcuW62000cls, "def：Single table data");
                    mvars.toolTip1.SetToolTip(btn_mcuR62000, @"Read format：0~1023(Reg no.,FPGA A)，1024~2047(Reg no.,FPGA B)");
                    mvars.toolTip1.SetToolTip(btn_mcuW62000, @"Single table data，please confirm ""FPGA sel."" before Write");

                    mvars.toolTip1.SetToolTip(btn_mcuW64000cls, "def：Single table data");
                    mvars.toolTip1.SetToolTip(btn_mcuR64000, @"Read format：0~511(Reg no.,single table data (non left and right))");
                    mvars.toolTip1.SetToolTip(btn_mcuW64000, @"Single table data，please confirm ""FPGA sel."" before Write");

                    mvars.toolTip1.SetToolTip(btn_uiregadrclsdef, "def：Reg no.,FPGA A data~FPGA Bdata");    //，寫入時須搭配 FPGA選擇
                    mvars.toolTip1.SetToolTip(btn_uiregadrread0x12, "Read format：Reg no.,FPGA A data~FPGA B data");
                    mvars.toolTip1.SetToolTip(btn_uiregadrreadRADWRb, "Read format：Reg no.,FPGA A data~FPGA B data");
                    label26.Text = "Hardware ID                  MCU 0x30000       MCU 0x32000        MCU 0x34000       Reg Table(realtime)            MCU 0x36000(Notes)";
                    label24.Text = "FPGA Sel.";
                    chk_NVBC.Text = "Boardcast";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    mvars.toolTip1.SetToolTip(btn_mcuR60000, @"回讀格式：0~1023(暫存器編號,右畫面內容)，1024~2047(暫存器編號,左畫面內容)");
                    mvars.toolTip1.SetToolTip(btn_mcuW60000, @"單一表格資料，寫入前請先確認""FPGA選擇""");

                    mvars.toolTip1.SetToolTip(btn_mcuW62000cls, "def：單一表格資料");
                    mvars.toolTip1.SetToolTip(btn_mcuR62000, @"回讀格式：0~1023(暫存器編號,右畫面內容)，1024~2047(暫存器編號,左畫面內容)");
                    mvars.toolTip1.SetToolTip(btn_mcuW62000, @"單一表格資料，寫入前請先確認""FPGA選擇""");

                    mvars.toolTip1.SetToolTip(btn_mcuW64000cls, "def：單一表格資料");
                    mvars.toolTip1.SetToolTip(btn_mcuR64000, @"回讀格式：0~511(暫存器編號,表格資料(不分左右畫面))");
                    mvars.toolTip1.SetToolTip(btn_mcuW64000, @"單一表格資料，寫入前請先確認""FPGA選擇""");

                    mvars.toolTip1.SetToolTip(btn_uiregadrclsdef, "def：暫存器編號,右畫面內容~左畫面內容");    //，寫入時須搭配 FPGA選擇
                    mvars.toolTip1.SetToolTip(btn_uiregadrread0x12, "回讀格式：暫存器編號,右畫面內容~左畫面內容");
                    mvars.toolTip1.SetToolTip(btn_uiregadrreadRADWRb, "回讀格式：暫存器編號,右畫面內容~左畫面內容");
                    label26.Text = "硬體ID                            MCU 0x30000       MCU 0x32000        MCU 0x34000       Reg Table(即時)                  MCU 0x36000(訊息)";
                    label24.Text = "FPGA選擇";
                    chk_NVBC.Text = "大屏廣播";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    mvars.toolTip1.SetToolTip(btn_mcuR60000, @"回读格式：0~1023(暫存器編號,右画面内容)，1024~2047(暫存器編號,左画面内容)");
                    mvars.toolTip1.SetToolTip(btn_mcuW60000, @"单一表格资料，写入前请先确认""FPGA选择""");

                    mvars.toolTip1.SetToolTip(btn_mcuW62000cls, "def：单一表格资料");
                    mvars.toolTip1.SetToolTip(btn_mcuR62000, @"回读格式：0~1023(暫存器編號,右画面内容)，1024~2047(暫存器編號,左画面内容)");
                    mvars.toolTip1.SetToolTip(btn_mcuW62000, @"单一表格资料，写入前请先确认""FPGA选择""");

                    mvars.toolTip1.SetToolTip(btn_mcuW64000cls, "def：单一表格资料");
                    mvars.toolTip1.SetToolTip(btn_mcuR64000, @"回读格式：0~511(暫存器編號,表格资料(不分左右画面))");
                    mvars.toolTip1.SetToolTip(btn_mcuW64000, @"单一表格资料，写入前请先确认""FPGA选择""");

                    mvars.toolTip1.SetToolTip(btn_uiregadrclsdef, "def：暂存器编号,右画面内容~左画面内容");    //，寫入時須搭配 FPGA選擇
                    mvars.toolTip1.SetToolTip(btn_uiregadrread0x12, "回读格式：暂存器编号,右画面内容~左画面内容");
                    mvars.toolTip1.SetToolTip(btn_uiregadrreadRADWRb, "回读格式：暂存器编号,右画面内容~左画面内容");
                    label26.Text = "硬体ID                            MCU 0x30000       MCU 0x32000        MCU 0x34000       Reg Table(即时)                  MCU 0x36000(讯息)";
                    label24.Text = "FPGA选择";
                    chk_NVBC.Text = "大屏广播";
                }

                btn_mcuW60000.Tag = "30000";
                btn_mcuR60000.Tag = "30000";
                btn_mcuW62000.Tag = "32000";
                btn_mcuR62000.Tag = "32000";
                btn_mcuW64000.Tag = "34000";
                btn_mcuR64000.Tag = "34000";
                btn_mcuW66000.Tag = "36000";
                btn_mcuR66000.Tag = "36000";

            }


            mvars.actFunc = "dmr";
            mvars.FormShow[7] = true;

            cmb_deviceID.Items.Clear();
            for (int i = 0; i < Form1.cmbdeviceid.Items.Count; i++)
            {
                cmb_deviceID.Items.Add(Form1.cmbdeviceid.Items[i].ToString());
            }
            cmb_deviceID.Text = Form1.cmbdeviceid.Text;

            cmb_FPGAsel.Items.Clear();
            cmb_FPGAsel.Visible = false;
            if (Form1.cmbFPGAsel.Items.Count != 0)
            {
                cmb_FPGAsel.Visible = true;
                for (int i = 0; i < Form1.cmbFPGAsel.Items.Count; i++) cmb_FPGAsel.Items.Add(Form1.cmbFPGAsel.Items[i].ToString());
                cmb_FPGAsel.Text = cmb_FPGAsel.Items[mvars.FPGAsel].ToString();

                if (mvars.deviceID.Substring(0, 2) == "03" && cmb_FPGAsel.FindString(" XB01") != -1)
                {
                    int j = cmb_FPGAsel.FindString(" XB01");
                    cmb_FPGAsel.Text = cmb_FPGAsel.Items[j].ToString();
                }
                //else if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")
                //    lbl_FPGAsel.Visible = true;
            }

            if (MultiLanguage.DefaultLanguage == "en-US") { mvars.strR66Kdefault = "0,RECORD,255~1,RESERVE,0~2,RESERVE,0~3,RESERVE,0~4,POS X,0~5,POS Y,0~6,W,960~7,H,540~8,DRAW X,0~9,DRAW Y,0"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { mvars.strR66Kdefault = "0,使用紀錄,255~1,保留,0~2,保留,0~3,保留,0~4,位置 X,0~5,位置 Y,0~6,屏寬,960~7,屏高,540~8,畫圖 X,0~9,畫圖 Y,0"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { mvars.strR66Kdefault = "0,使用纪录,255~1,保留,0~2,保留,0~3,保留,0~4,位置 X,0~5,位置 Y,0~6,屏宽,960~7,屏高,540~8,画图 X,0~9,画图 Y,0"; }
        }


        private void markreset(int svtotalcounts, bool svdelfb, bool selfrun)
        {
            lst_get1.Items.Clear();
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







        

        
        private void chk_R100_Click(object sender, EventArgs e)
        {
            if (chk_NVBC.Checked && mvars.FPGAsel != 2) cmb_FPGAsel.SelectedIndex = 2;

            CheckBox chk = (CheckBox) sender;      
            chk.Enabled = false;
            Form1.pvindex = 100;
            string Svs = "FPGA_SPI_W" + Form1.pvindex.ToString("000");
            mvars.lblCmd = "FPGA_SPI_W";
            if (mvars.deviceID.Substring(0, 2) == "05") mp.mhFPGASPIWRITE(mvars.FPGAsel, Convert.ToInt32(chk.Tag));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { lst_get1.Items.Add(Svs + ",ERROR"); }
            else { lst_get1.Items.Add(Svs + ",DONE"); }
            lst_get1.TopIndex = lst_get1.Items.Count - 1;
            chk.Enabled = true;
        }









        private void numUD_ugma_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //    {
            //        if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //        Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //    }
            //    mp.cUSERGMA(uc_C12Ademura.svnova, numUD_ugma32.Value.ToString(), numUD_ugma64.Value.ToString(), numUD_ugma128.Value.ToString(), numUD_ugma255.Value.ToString());
            //}
        }
        
        private void numUD_ubrig_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //    {
            //        if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //        Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //    }
            //    mp.cUSERBRIG(uc_C12Ademura.svnova, numUD_ubrigR.Value.ToString(), numUD_ubrigG.Value.ToString(), numUD_ubrigB.Value.ToString());
            //}
        }
        
        private void numUD_WT_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //    {
            //        if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //        Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //    }
            //    mp.cWTGRAY_cf(uc_C12Ademura.svnova, numUD_WTR.Value.ToString(), numUD_WTG.Value.ToString(), numUD_WTB.Value.ToString());
            //}
        }
        
        

        

        private void btn_mcuR62000_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            #region 純按鍵單次操作
            if (mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            else
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { MessageBox.Show("demo mode"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { MessageBox.Show("演示模式"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { MessageBox.Show("演示模式"); }
            }
            this.Enabled = false;
            markreset(11, false, true);
            mvars.lblCmd = "MCU_FLASH_R" + btn.Tag.ToString();
            mp.mhMCUFLASHREAD(btn.Tag.ToString().PadLeft(8,'0'), 8192);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
            {
                mvars.errCode = "-3";
                Form1.tslblStatus.Text = "0x" + btn.Tag.ToString() + " rd MCU Flash fail";
            }
            else
            {
                Form1.tslblStatus.Text = "Done";
                if (btn.Tag.ToString() == "60000" || btn.Tag.ToString() == "30000")
                {
                    mvars.lstmcuR60000.Items.Clear();
                    if (mvars.strR60K.Length > 1)
                    {
                        mvars.lstmcuR60000.Items.AddRange(mvars.strR60K.Split('~'));
                        if (mvars.deviceID.Substring(0, 2) == "05")
                        {
                            if (mvars.lstmcuR60000.Items[1024].ToString() != "0,0")
                            {
                                Form1.tslblStatus.Text = "0x" + btn.Tag.ToString() + " rd MCU Flash items：" + (mvars.lstmcuR60000.Items.Count).ToString() + "，FPGA B 1st[1024]：" + mvars.lstmcuR60000.Items[1024].ToString();
                            }
                            else
                            {
                                Form1.tslblStatus.Text = "0x" + btn.Tag.ToString() + " rd MCU Flash items：" + (mvars.lstmcuR60000.Items.Count).ToString();
                            }
                        }
                        else
                        {
                            Form1.tslblStatus.Text = "0x" + btn.Tag.ToString() + " rd MCU Flash items：" + (mvars.lstmcuR60000.Items.Count).ToString();
                        }
                    }
                    else
                    {
                        lbl_mcuR60000click.Text = "no record";
                        Form1.tslblStatus.Text = "0x" + btn.Tag.ToString() + " rd MCU Flash items：0";
                    }
                    lbl_mcuR60000click.Text = "< ";
                }
                else if (btn.Tag.ToString() == "62000" || btn.Tag.ToString() == "32000")
                {
                    mvars.lstmcuR62000.Items.Clear();
                    if (mvars.strR62K.Length > 1)
                    {
                        mvars.lstmcuR62000.Items.AddRange(mvars.strR62K.Split('~'));
                        //Save File
                        if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")
                        {
                            string[] svsA = new string[lst_mcuR62000.Items.Count / 2];
                            string[] svsB = new string[lst_mcuR62000.Items.Count / 2];
                            string pathA = "C:\\Users\\" + Environment.UserName + "\\Documents\\mcuR62000_A.txt";
                            string pathB = "C:\\Users\\" + Environment.UserName + "\\Documents\\mcuR62000_B.txt";
                            for (int i = 0; i < lst_mcuR62000.Items.Count / 2; i++)
                            {
                                svsA[i] = lst_mcuR62000.Items[i].ToString();
                                svsB[i] = lst_mcuR62000.Items[i + lst_mcuR62000.Items.Count / 2].ToString();
                            }
                            File.WriteAllLines(pathA, svsA);
                            File.WriteAllLines(pathB, svsB);
                            lst_get1.Items.Add("mcuR" + btn.Tag.ToString() + " partA read save to " + pathA);
                            lst_get1.Items.Add("mcuR" + btn.Tag.ToString() + " partB read save to " + pathB);
                        }
                        else
                        {
                            string[] svsA = new string[lst_mcuR62000.Items.Count / 2];
                            string pathA = "C:\\Users\\" + Environment.UserName + "\\Documents\\mcuR62000.txt";
                            for (int i = 0; i < lst_mcuR62000.Items.Count; i++)
                            {
                                svsA[i] = lst_mcuR62000.Items[i].ToString();
                            }
                            File.WriteAllLines(pathA, svsA);
                            lst_get1.Items.Add("mcuR" + btn.Tag.ToString() + " partA read save to " + pathA);
                        }
                    }
                    else lbl_mcuR62000click.Text = "no record";
                    lbl_mcuR62000click.Text = "< ";
                    Form1.tslblStatus.Text = "0x" + btn.Tag.ToString() + " rd MCU Flash items：" + (mvars.lstmcuR62000.Items.Count).ToString();
                }
                else if (btn.Tag.ToString() == "64000" || btn.Tag.ToString() == "34000")
                {
                    mvars.lstmcuR64000.Items.Clear();
                    if (mvars.strR64K.Length > 1) mvars.lstmcuR64000.Items.AddRange(mvars.strR64K.Split('~'));
                    else lbl_mcuR64000click.Text = "no record";
                    lbl_mcuR64000click.Text = "< ";
                    Form1.tslblStatus.Text = "0x" + btn.Tag.ToString() + " rd MCU Flash items：" + (mvars.lstmcuR64000.Items.Count).ToString();
                }
            }
            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
            #endregion 純按鍵操作
        }

        private void btn_mcuW62000_Click(object sender, EventArgs e)
        {
            string svdeviceID = mvars.deviceID;
            if (chkNVBC.Checked == true) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            RECT rct;                   //GetWindowRec 取得顯示表單位置
            Button btn = (Button)sender;
            ListBox lst = lst_mcuW60000;
            if (btn.Tag.ToString() == "60000") lst = lst_mcuW60000;
            else if (btn.Tag.ToString() == "62000") lst = lst_mcuW62000;
            int svms = 0;               //Primary
            int svme = 1;               //Primary
            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            string[] sRegDec;           //addr
            string[] sDataDec;          //data
            string svs;
            svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
            lst_get1.Items.Add(svs );
            markreset(3, false, true);
            /*
                Primary MCU Flash 
                0x62000採特殊方式 切 FPGA A4096 / FPGA B4096 Total = 8192
                如果只有single duty則是填 0~6 , 10~282 = 283bytes
                如果是dual duty則是填 0~6 , 10~282 , 300~572 = 572bytes
                0x64000則不分區，所以僅填單側0~39個 = 40bytes
             */

            #region 清空 MCU Flash 用
            if (lst.Items.Count <= 0)
            {
                string svmsg = "";
                if (mvars.deviceID.Substring(0, 2) == "05")
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x" + btn.Tag.ToString() + "\r\n" + "\r\n" + @"""OK""  will earse MCU flash and " + cmbFPGAsel.Text + " UIREG restore default"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x" + btn.Tag.ToString() + " 沒有內容" + "\r\n" + "\r\n" + @"""OK""  將繼續進行清空微控閃存並且將 " + cmbFPGAsel.Text + " UIREG 還原預設值"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x" + btn.Tag.ToString() + " 没有内容" + "\r\n" + "\r\n" + @"""OK""  将继续进行清空微控闪存 " + cmbFPGAsel.Text + " UIREG 还原预设值"; }
                }
                else
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x" + btn.Tag.ToString() + "\r\n" + "\r\n" + @"""OK""  will earse MCU flash and UIREG restore default"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x" + btn.Tag.ToString() + " 沒有內容" + "\r\n" + "\r\n" + @"""OK""  將繼續進行清空微控閃存並且將 UIREG 還原預設值"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x" + btn.Tag.ToString() + " 没有内容" + "\r\n" + "\r\n" + @"""OK""  将继续进行清空微控闪存 UIREG 还原预设值"; }
                }
                // GetWindowRec 取得顯示表單位置
                GetWindowRect(mvars.handleIDMe, out rct);
                string value = "";
                if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                //mp.mhMCUFLASHWRITE(btn.Tag.ToString().PadLeft(8, '0'), ref BinArr);
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                if (btn.Tag.ToString() == "60000" || btn.Tag.ToString() == "30000") { }
                else if (btn.Tag.ToString() == "62000" || btn.Tag.ToString() == "32000")
                {
                    for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                    {
                        Form1.svuiregadr[j] = mvars.uiregadr_default[j].Split(',')[2];
                        Form1.svuiregadr[j + (int)(mvars.GAMMA_SIZE / 8)] = mvars.uiregadr_default[j].Split(',')[2];
                    }

                    for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                    {
                        if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                    }
                    for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                    {
                        if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                    }
                    if (mvars.deviceID.Substring(0, 2) == "10") { svme = mvars.uiregadr_default.Length - 1; }
                    lbl_GMAdrop.Text = svme.ToString();

                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = i.ToString();
                        sDataDec[i] = Form1.svuiregadr[i];
                    }
                    mvars.lblCmd = "FPGA_REG_W";
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
                else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
                btn.Enabled = true;
                return;
            }
            #endregion

            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (mvars.FPGAsel != 2)
                {
                    string svmsg = "";
                    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "0x" + btn.Tag.ToString() + " single side write @ " + cmbFPGAsel.Text; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x" + btn.Tag.ToString() + " 單側寫入 @ " + cmbFPGAsel.Text; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x" + btn.Tag.ToString() + " 单侧写入 @ " + cmbFPGAsel.Text; }
                    // GetWindowRec 取得顯示表單位置
                    GetWindowRect(mvars.handleIDMe, out rct);
                    string value = "";
                    if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                }
            }

            #region 純按鍵單次操作 Disabled
            //if (mvars.demoMode == false)
            //{
            //    if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //    Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            //}
            //else
            //{
            //    if (MultiLanguage.DefaultLanguage == "en-US") { MessageBox.Show("demo mode"); }
            //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { MessageBox.Show("演示模式"); }
            //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { MessageBox.Show("演示模式"); }
            //}
            //this.Enabled = false;
            //markreset(999, false, true);
            //if (mvars.dualduty == 1) svi = lst_mcuW62000.Items.Count; else svi = Convert.ToInt32(lbl_GMAdrop.Text);
            //string[] sRegDec = new string[svi];   //addr
            //string[] sDataDec = new string[svi];  //data
            #region 多筆多次
            //for (int i = 0; i < svi; i++)
            //{
            //    mvars.lblCmd = "UIREGARDW_"+ i;
            //    mp.UIREGARDW(lst_mcuW62000.Items[i].ToString().Split(',')[0], lst_mcuW62000.Items[i].ToString().Split(',')[1]);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
            //    {
            //        if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Add("UIREGADR Write Error"); }
            //        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Add("UIREGADR寫入發生異常"); }
            //        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Add("UIREGADR写入发生异常"); }
            //    }
            //    lst_get1.TopIndex = lst_get1.Items.Count - 1;
            //}
            #endregion
            #region 多筆單次
            //for (int i = 0; i < svi; i++)
            //{
            //    sRegDec[i] = lst_mcuW62000.Items[i].ToString().Split(',')[0];
            //    sDataDec[i] = lst_mcuW62000.Items[i].ToString().Split(',')[1];
            //}
            //mvars.lblCmd = "UIREGARDWm";
            //mp.UIREGARDWm(sRegDec, sDataDec);
            //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
            //{
            //    if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Add("UIREGADR Write Error"); }
            //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Add("UIREGADR寫入發生異常"); }
            //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Add("UIREGADR写入发生异常"); }
            //}
            lst_get1.TopIndex = lst_get1.Items.Count - 1;
            #endregion
            //if (mvars.demoMode == false) { mp.CommClose(); }
            //this.Enabled = true;
            #endregion 純按鍵操作

            #region 與sendmessage共用
            if (chkNVBC.Checked)
            {
                if (MessageBox.Show(chkNVBC.Text, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) return;
            }

            if (mvars.svnova == false && mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            
            
            
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (btn.Tag.ToString() == "60000")
                {
                    svme = lst.Items.Count;
                }
                else if (btn.Tag.ToString() == "62000")
                {
                    if (mvars.dualduty == 1) svme = mvars.uiregadr_default.Length;
                    else
                    {
                        for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                        {
                            if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                        }
                        for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                        {
                            if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                        }
                        lbl_GMAdrop.Text = svme.ToString();
                    }
                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = lst.Items[i].ToString().Split(',')[0];
                        sDataDec[i] = lst.Items[i].ToString().Split(',')[1];
                    }
                    btn.Enabled = false;
                    mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                    svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
                    lst_get1.Items.Add(svs + " ....");
                    mp.doDelayms(10);
                    mp.cUIREGADRwENG(btn.Tag.ToString(), sRegDec, sDataDec);
                }
            }

            else if (mvars.deviceID.Substring(0, 2) == "06")
            {
                if (btn.Tag.ToString() == "60000")
                {
                    svme = lst.Items.Count;
                }
                else if (btn.Tag.ToString() == "62000")
                {
                    svme = 133;

                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = lst.Items[i].ToString().Split(',')[0];
                        sDataDec[i] = lst.Items[i].ToString().Split(',')[1];
                    }
                    btn.Enabled = false;
                    mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                    svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
                    lst_get1.Items.Add(svs + " ....");
                    mp.doDelayms(10);
                    mp.cUIREGADRwENG(btn.Tag.ToString(), sRegDec, sDataDec);
                }
            }





            else if (mvars.deviceID.Substring(0, 2) == "10")
            {
                if (btn.Tag.ToString() == "62000")
                {
                    if (mvars.dualduty == 1) svme = mvars.uiregadr_default.Length;
                    else
                    {
                        for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                        {
                            if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                        }
                        for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                        {
                            if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                        }
                        lbl_GMAdrop.Text = svme.ToString();
                    }
                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = lst.Items[i].ToString().Split(',')[0];
                        sDataDec[i] = lst.Items[i].ToString().Split(',')[1];
                    }
                    btn.Enabled = false;
                    mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                    svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
                    lst_get1.Items.Add(svs + " ....");
                    mp.doDelayms(10);
                    mp.cUIREGADRwENG(btn.Tag.ToString(), sRegDec, sDataDec);
                }
            }
                mvars.deviceID = svdeviceID;

            if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
            else {lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
            btn.Enabled = true;
            string[] svss = mvars.strReceive.Split(',');
            lst_get1.Items.Add("  ↑，" + svss[7] + "s");
            lst_get1.TopIndex = lst_get1.Items.Count - 1;
            #endregion 與sendmessage共用
        }



        private void btn_mcuR60000cls_Click(object sender, EventArgs e)
        {
            lst_mcuR60000.Items.Clear();
        }

        private void btn_mcuR62000cls_Click(object sender, EventArgs e)
        {
            lst_mcuR62000.Items.Clear();
        }



        private void btn_mcuW60000cls_Click(object sender, EventArgs e) { lst_mcuW60000.Items.Clear(); }





        private void chk_EGMAONOFF_Click(object sender, EventArgs e)
        {
            //if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //{
            //    if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //    Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //}
            //if (mvars.UUT.demo == false)
            //{
            //    chk_EGMAONOFF.Enabled = false;
            //    numUD_egma.Enabled = chk_EGMAONOFF.Enabled; cmb_egma.Enabled = chk_EGMAONOFF.Enabled;
            //    mvars.chkcf[0] = chk_ugmaONOFF;
            //    mvars.chkcf[1] = chk_ubrigONOFF;
            //    mvars.chkcf[2] = chk_WTPGONOFF;
            //    mvars.chkcf[3] = chk_EGMAONOFF;
            //    mvars.chkcf[4] = chk_MCU;
            //    mvars.chkcf[5] = chk_COLORDEPTH;
            //    byte svsum = 0;
            //    for (int i = 0; i < mvars.chkcf.Length; i++)
            //    {
            //        if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
            //    }
            //    mvars.flgSelf = true;
            //    mvars.lblCompose = "ENG_GMA";
            //    if (chk_EGMAONOFF.Checked) { mp.cUIREGONOFF(uc_C12Ademura.svnova, true, svsum); }
            //    else { mp.cUIREGONOFF(uc_C12Ademura.svnova, false, svsum); }
            //}
            //numUD_egma.Visible = chk_EGMAONOFF.Checked; cmb_egma.Visible = chk_EGMAONOFF.Checked;
            //chk_EGMAONOFF.Enabled = !chk_EGMAONOFF.Enabled;
            //numUD_egma.Enabled = chk_EGMAONOFF.Enabled; cmb_egma.Enabled = chk_EGMAONOFF.Enabled;
        }
        private void cmb_egma_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////1013 +10 --> +11
            //if (lst_mcuW62000.Items.Count == mvars.uiregadr_default.Length)
            //{
            //    //1013 +10 --> +11
            //    string[] svs = lst_mcuW62000.Items[cmb_egma.SelectedIndex + 11].ToString().Split(',');
            //    numUD_egma.Value = Convert.ToUInt16(svs[1]);
            //}
            //else
            //{
            //    string[] svss = Form1.nvsender[mvars.iSender].regBoxR62k[mvars.iPort, mvars.iScan].Split('~');
            //    //1013 +10 --> +11
            //    string[] svs = svss[cmb_egma.SelectedIndex + 11].Split(',');
            //    numUD_egma.Value = Convert.ToUInt16(svs[1]);
            //}
            //numUD_egma.Focus();
        }
        private void numUD_egma_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    numUD_egma.Enabled = false;
            //    if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //    {
            //        if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //        Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //    }

            //    byte svlb = (byte)(cmb_egma.SelectedIndex / 12);    //addr 191  cmb_egma.SelectedIndex = 180 svlb = 15
            //    //1013 +10 --> +11
            //    int svi = cmb_egma.SelectedIndex + 11;   //selectindex0=addr11

            //    string[] svss = Form1.nvsender[mvars.iSender].regBoxR62k[mvars.iPort, mvars.iScan].Split('~');
            //    mvars.lstmcuW62000.Items.Clear();
            //    mvars.lstmcuW62000.Items.AddRange(svss);

            //    string[] svs = mvars.lstmcuW62000.Items[svi].ToString().Split(',');
            //    mvars.lstmcuW62000.Items.RemoveAt(svi);
            //    mvars.lstmcuW62000.Items.Insert(svi, svs[0] + "," + numUD_egma.Value);

            //    mp.cENGGMA(uc_C12Ademura.svnova, svlb);

            //    numUD_egma.Enabled = true;
            //}
        }



        private void chk_WTPGONOFF_Click(object sender, EventArgs e)
        {
            //chk_WTPGONOFF.Enabled = false;
            //numUD_WTR.Enabled = chk_WTPGONOFF.Enabled; numUD_WTG.Enabled = chk_WTPGONOFF.Enabled; numUD_WTB.Enabled = chk_WTPGONOFF.Enabled;
            //if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //{
            //    if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //    Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //}
            //if (mvars.UUT.demo == false)
            //{
            //    mvars.chkcf[0] = chk_ugmaONOFF;
            //    mvars.chkcf[1] = chk_ubrigONOFF;
            //    mvars.chkcf[2] = chk_WTPGONOFF;
            //    mvars.chkcf[3] = chk_EGMAONOFF;
            //    mvars.chkcf[4] = chk_MCU;
            //    mvars.chkcf[5] = chk_COLORDEPTH;
            //    byte svsum = 0;
            //    for (int i = 0; i < mvars.chkcf.Length; i++)
            //    {
            //        if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
            //    }
            //    mvars.flgSelf = true;
            //    mvars.lblCompose = "WT_PG";
            //    if (chk_WTPGONOFF.Checked) { mp.cUIREGONOFF(uc_C12Ademura.svnova, true, svsum); }
            //    else { mp.cUIREGONOFF(uc_C12Ademura.svnova, false, svsum); }
            //}
            //numUD_WTR.Visible = chk_WTPGONOFF.Checked; numUD_WTG.Visible = chk_WTPGONOFF.Checked; numUD_WTB.Visible = chk_WTPGONOFF.Checked;
            //chk_WTPGONOFF.Enabled = !chk_WTPGONOFF.Enabled;
            //numUD_WTR.Enabled = chk_WTPGONOFF.Enabled; numUD_WTG.Enabled = chk_WTPGONOFF.Enabled; numUD_WTB.Enabled = chk_WTPGONOFF.Enabled;
        }

        private void chk_ubrigONOFF_Click(object sender, EventArgs e)
        {
            //chk_ubrigONOFF.Enabled = false;
            //numUD_ubrigR.Enabled = chk_ubrigONOFF.Enabled; numUD_ubrigG.Enabled = chk_ubrigONOFF.Enabled; numUD_ubrigB.Enabled = chk_ubrigONOFF.Enabled;
            //if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //{
            //    if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //    Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //}
            //if (mvars.UUT.demo == false)
            //{
            //    mvars.chkcf[0] = chk_ugmaONOFF;
            //    mvars.chkcf[1] = chk_ubrigONOFF;
            //    mvars.chkcf[2] = chk_WTPGONOFF;
            //    mvars.chkcf[3] = chk_EGMAONOFF;
            //    mvars.chkcf[4] = chk_MCU;
            //    mvars.chkcf[5] = chk_COLORDEPTH;
            //    byte svsum = 0;
            //    for (int i = 0; i < mvars.chkcf.Length; i++)
            //    {
            //        if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
            //    }
            //    mvars.flgSelf = true;
            //    mvars.lblCompose = "USER_BRI";
            //    if (chk_ubrigONOFF.Checked) { mp.cUIREGONOFF(uc_C12Ademura.svnova, true, svsum); }
            //    else { mp.cUIREGONOFF(uc_C12Ademura.svnova, false, svsum); }
            //}
            //numUD_ubrigR.Visible = chk_ubrigONOFF.Checked; numUD_ubrigG.Visible = chk_ubrigONOFF.Checked; numUD_ubrigB.Visible = chk_ubrigONOFF.Checked;
            //chk_ubrigONOFF.Enabled = !chk_ubrigONOFF.Enabled;
            //numUD_ubrigR.Enabled = chk_ubrigONOFF.Enabled; numUD_ubrigG.Enabled = chk_ubrigONOFF.Enabled; numUD_ubrigB.Enabled = chk_ubrigONOFF.Enabled;
        }

        private void chk_ugmaONOFF_Click(object sender, EventArgs e)
        {
            //chk_ugmaONOFF.Enabled = false;
            //numUD_ugma32.Enabled = chk_ugmaONOFF.Enabled; numUD_ugma64.Enabled = chk_ugmaONOFF.Enabled; numUD_ugma128.Enabled = chk_ugmaONOFF.Enabled; numUD_ugma255.Enabled = chk_ugmaONOFF.Enabled;
            //if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //{
            //    if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //    Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //}
            //if (mvars.UUT.demo == false)
            //{
            //    mvars.chkcf[0] = chk_ugmaONOFF;
            //    mvars.chkcf[1] = chk_ubrigONOFF;
            //    mvars.chkcf[2] = chk_WTPGONOFF;
            //    mvars.chkcf[3] = chk_EGMAONOFF;
            //    mvars.chkcf[4] = chk_MCU;
            //    mvars.chkcf[5] = chk_COLORDEPTH;
            //    byte svsum = 0;
            //    for (int i = 0; i < mvars.chkcf.Length; i++)
            //    {
            //        if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
            //    }
            //    mvars.flgSelf = true;
            //    mvars.lblCompose = "USER_GMA";
            //    if (chk_ugmaONOFF.Checked) { mp.cUIREGONOFF(uc_C12Ademura.svnova, true, svsum); }
            //    else { mp.cUIREGONOFF(uc_C12Ademura.svnova, false, svsum); }
            //}
            //numUD_ugma32.Visible = chk_ugmaONOFF.Checked; numUD_ugma64.Visible = chk_ugmaONOFF.Checked; numUD_ugma128.Visible = chk_ugmaONOFF.Checked; numUD_ugma255.Visible = chk_ugmaONOFF.Checked;
            //chk_ugmaONOFF.Enabled = !chk_ugmaONOFF.Enabled;
            //numUD_ugma32.Enabled = chk_ugmaONOFF.Enabled; numUD_ugma64.Enabled = chk_ugmaONOFF.Enabled; numUD_ugma128.Enabled = chk_ugmaONOFF.Enabled; numUD_ugma255.Enabled = chk_ugmaONOFF.Enabled;
        }
        private void chk_COLORDEPTH_Click(object sender, EventArgs e)
        {
            //chk_COLORDEPTH.Enabled = false;
            //if (uc_C12Ademura.svnova == false && mvars.UUT.demo == false)
            //{
            //    if (mp.sp1Open(Form1.lblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //    Form1.lblHW.Text = "232"; Form1.lblHW.BackColor = Color.Blue; Form1.lblHW.ForeColor = Color.White;
            //}
            //if (mvars.UUT.demo == false)
            //{
            //    mvars.chkcf[0] = chk_ugmaONOFF;
            //    mvars.chkcf[1] = chk_ubrigONOFF;
            //    mvars.chkcf[2] = chk_WTPGONOFF;
            //    mvars.chkcf[3] = chk_EGMAONOFF;
            //    mvars.chkcf[4] = chk_MCU;
            //    mvars.chkcf[5] = chk_COLORDEPTH;
            //    byte svsum = 0;
            //    for (int i = 0; i < mvars.chkcf.Length; i++)
            //    {
            //        if (mvars.chkcf[i].Checked) svsum += (byte)Math.Pow(2, i);
            //    }
            //    mvars.flgSelf = true;
            //    mvars.lblCompose = "COLOR_DEPTH";
            //    if (chk_COLORDEPTH.Checked) { mp.cUIREGONOFF(uc_C12Ademura.svnova, true, svsum); }
            //    else { mp.cUIREGONOFF(uc_C12Ademura.svnova, false, svsum); }
            //}
            //if (chk_COLORDEPTH.Checked) { lbl_ColorDepth.Text = "       COLOR DEPTH  10-BIT"; } else { lbl_ColorDepth.Text = "       COLOR DEPTH  8-BIT"; }
            //chk_COLORDEPTH.Enabled = !chk_COLORDEPTH.Enabled;
        }
        private void lst_mcuW60000_Click(object sender, EventArgs e)
        {
            string value = "";
            int svi = lst_mcuW60000.SelectedIndex;
            if (svi > -1)
            {
                value = lst_mcuW60000.Items[svi].ToString();
            }


            if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                    "    Input MCU FLASH0x60000 contant", ref value, 65) == DialogResult.Cancel)
            { return; }//MessageBox.Show("Please input UUT ID", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            else
            {
                if (value != "")
                {
                    if (svi > -1)
                    {
                        lst_mcuW60000.Items.RemoveAt(svi);
                        lst_mcuW60000.Items.Insert(svi, value);
                    }
                    else lst_mcuW60000.Items.Add(value);
                }
            }
        }

        private void btn_GMAdrop_Click(object sender, EventArgs e)
        {
            if (lst_mcuW62000.Items.Count == 0) { return; }
            
            int i;
            string value = mvars.dropvalue;
            string svs1 = "";

            if (MultiLanguage.DefaultLanguage == "en-US") svs1 = @"    Input Drop GaryScale between 0 to 255";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") svs1 = @"    請輸入 Drop 介於 0 到 255 之間的灰階";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") svs1 = @"    请输入 Drop 介于 0 到 255 之间的灰阶";
            //if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
            //        svs1 + "\r\n" + "\r\n" 
            //        , ref value, 95) == DialogResult.Cancel)
            //{ return; }

            RECT rct;
            GetWindowRect(mvars.handleIDMe, out rct);
            //myRect.X = rct.Left;
            //myRect.Y = rct.Top;
            //myRect.Width = rct.Right - rct.Left + 1;
            //myRect.Height = rct.Bottom - rct.Top + 1;
            if (mp.InputBox("", svs1, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 1, "") == DialogResult.Cancel) { return; }
            else
            {
                if (mp.IsNumeric(value) && mvars.dropvalue.Split(',').Length == 1) mvars.dropvalue = value;
                else return;
            }

            #region 燈板Gamma可調(工程模式) (on : 1 / off : 0)
            for (i = 5; i <= 5; i++)
            {
                string[] svs = lst_mcuW62000.Items[i].ToString().Split(',');
                lst_mcuW62000.Items.RemoveAt(i);
                string svss = svs[0] + ",1";
                lst_mcuW62000.Items.Insert(i, svss);
            }
            int svms = 91;      /// Primary
            int svme = 283;     /// Primary
            if (mvars.deviceID.Substring(0, 2) == "06")
            {
                svms = 61;      /// TV130
                svme = 133;     /// TV130
            }
            for (i = 0; i < mvars.uiregadr_default.Length; i++)
            {
                if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
            }
            for (i = svms; i < mvars.uiregadr_default.Length; i++)
            {
                if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
            }
            lbl_GMAdrop.Text = svme.ToString();
            for (i = svms; i < svme; i++)
            {
                string[] svs = lst_mcuW62000.Items[i].ToString().Split(',');
                lst_mcuW62000.Items.RemoveAt(i);
                string svss = svs[0] + "," + (Convert.ToInt16(svs[1]) * Convert.ToInt16(mvars.dropvalue) / 255).ToString();
                lst_mcuW62000.Items.Insert(i, svss);
            }
            #endregion
        }

        private void chk_NVBC_CheckedChanged(object sender, EventArgs e)
        {
            cmbdeviceid.Enabled = !chkNVBC.Checked;
            Form1.chkBC.Checked = chkNVBC.Checked;
        }







































        #region 20230328
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void btn_mcuW62000cls_Click(object sender, EventArgs e) { lst_mcuW62000.Items.Clear(); }

        private void btn_mcuW62000cls_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lst_mcuW62000.Items.Clear();
                lst_mcuW62000.Items.AddRange(mvars.uiregadrdefault.Split('~'));
            }
        }


        private void cmb_FPGAsel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                Form1.cmbFPGAsel.SelectedIndex = cmb_FPGAsel.SelectedIndex;
                Form1.cmbFPGAsel.Text = Form1.cmbFPGAsel.Items[mvars.FPGAsel].ToString();
                cmb_FPGAsel.Text = cmb_FPGAsel.Items[mvars.FPGAsel].ToString();
            }
        }




        private void lst_mcuW62000_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lst_mcuW62000_Click(object sender, EventArgs e)
        {
            if (lst_mcuW62000.Items.Count == 0) return;

            if (mvars.uiregadr_default[lst_mcuW62000.SelectedIndex].Split(',')[1] == "IDLE" || mvars.uiregadr_default[lst_mcuW62000.SelectedIndex].Split(',')[1] == "")
            {
                return;
            }
            string value = "";
            int svi = lst_mcuW62000.SelectedIndex;
            if (svi > -1) value = lst_mcuW62000.Items[svi].ToString().Split(',')[1];
            string svmsg = mvars.uiregadr_default[lst_mcuW62000.SelectedIndex].Split(',')[0] + "," + mvars.uiregadr_default[lst_mcuW62000.SelectedIndex].Split(',')[1];
            if (MultiLanguage.DefaultLanguage == "en-US") svmsg += "\r\n" + "\r\n" + "Parameter(number)";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") svmsg += "\r\n" + "\r\n" + "參數(數字)";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") svmsg += "\r\n" + "\r\n" + "参数(数字)";
            // GetWindowRec 取得顯示表單位置
            RECT rct;
            GetWindowRect(mvars.handleIDMe, out rct);
            if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 1, "") == DialogResult.Cancel) { return; }
            else
            {
                if (value != "" && mp.IsNumeric(value))
                {
                    if (svi > -1)
                    {
                        lst_mcuW62000.Items.RemoveAt(svi);
                        lst_mcuW62000.Items.Insert(svi, svi + "," + value.Trim());
                    }
                }
                else
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") svmsg = @"please follow the format ""number""";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") svmsg = @"請依據格式輸入 ""數字""";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") svmsg = @"请依据格式输入 ""数字""";
                    value = "";
                    mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 1, "");
                    return;
                }
            }
        }

        private void btn_mcuL62000_Click(object sender, EventArgs e)
        {

        }

        private void btn_uiregadrread0x12_Click(object sender, EventArgs e)
        {
            #region 純按鍵單次操作
            if (mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            else
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { MessageBox.Show("demo mode"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { MessageBox.Show("演示模式"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { MessageBox.Show("演示模式"); }
                return;
            }
            this.Enabled = false;
            markreset(2, false, true);

            if (mvars.FPGAsel != 2) cmb_FPGAsel.SelectedIndex = 2;
            string svstruiregadr = "";

            int svme = 283;     /// Primary
            //int svms = 91;      /// Primary
            //if (mvars.dualduty == 0)
            //{
            //    for (int i = 0; i < mvars.uiregadr_default.Length; i++)
            //    {
            //        if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
            //    }
            //    for (int i = svms; i < mvars.uiregadr_default.Length; i++)
            //    {
            //        if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
            //    }
            //}
            //else
            //{
                svme = mvars.uiregadr_default.Length;
            //}
            
            lbl_GMAdrop.Text = svme.ToString();

            mvars.lblCmd = "UIREGRAD_READ";
            mp.mUIREGARDRm(0, svme);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
            else
            {
                for (int svi = 0; svi < svme; svi++)
                {
                    Form1.svuiregadr[svi] = (mvars.ReadDataBuffer[6 + svi * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + svi * 2 + 1]).ToString();
                    Form1.svuiregadr[svi + 1024] = (mvars.ReadDataBuffer[6 + svi * 2 + svme * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + svi * 2 + svme * 2 + 1]).ToString();
                    svstruiregadr += ":" + svi + "," + Form1.svuiregadr[svi] + "~" + Form1.svuiregadr[svi + 1024];
                }
                if (svstruiregadr.Length > 1)
                {
                    lst_uiregadr.Items.Clear();
                    if (svstruiregadr.Substring(0, 1) == ":") svstruiregadr = svstruiregadr.Substring(1, svstruiregadr.Length - 1);
                    lst_uiregadr.Items.AddRange(svstruiregadr.Split(':'));
                }
                lst_uiregadr.TopIndex = 0;
                Form1.tslblStatus.Text = "MCU read uiregtable items：" + (lst_uiregadr.Items.Count).ToString();
            }
            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
            #endregion 純按鍵操作

        }

        string[,] svfpgareg = null;
        private void btn_uiregadrreadRADWR_Click(object sender, EventArgs e)
        {
            /// svFPGAsel  0:ABCD 1:EFGH 2:Boardcast
            /// _______________________
            /// |  FPGA B  |  FPGA A  |
            /// |   EFGH   |   ABCD   |
            /// |  左畫面  |  右畫面  |
            /// |    1     |     0    |
            /// |          |          |
            /// =======================
            Button btn = (Button)sender;

            #region 純按鍵單次操作
            if (lst_uiregadr.Items.Count == 0) 
            {
                string svmsg = "";
                if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container UIREGADR" + "\r\n" + "\r\n" + @"Please right-click "" cls def """; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "UIREGADR 裡面沒有內容" + "\r\n" + "\r\n" + @"請以滑鼠右鍵點擊 "" cls def """; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "UIREGADR 里面没有内容" + "\r\n" + "\r\n" + @"请以滑鼠右键点击 "" cls def """; }
                // GetWindowRec 取得顯示表單位置
                RECT rct;
                GetWindowRect(mvars.handleIDMe, out rct);
                string value = "";
                if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                return; 
            }

            if (mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            else
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { MessageBox.Show("demo mode"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { MessageBox.Show("演示模式"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { MessageBox.Show("演示模式"); }
                return;
            }
            this.Enabled = false;
            markreset(1999, false, true);

            if (mvars.FPGAsel != 2) cmb_FPGAsel.SelectedIndex = 2;
            string svstruiregadr = "";

            //Read
            Form1.pvindex = 32;
            mvars.lblCmd = "FPGA_SPI_W";
            mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-10"; }
            for (int svi = 0; svi < lst_uiregadr.Items.Count; svi++)
            {
                //Addr
                Form1.pvindex = 33;
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(mvars.FPGAsel, svi);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + svi; }
                //RData
                Form1.pvindex = 35;
                mvars.lblCmd = "FPGA_SPI_R";
                mp.mhFPGASPIREAD();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + svi; Form1.tslblStatus.Text = "FPGA RADWR uiregtable read err count：" + svi; }
                else
                {
                    Form1.svuiregadr[svi] = (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]).ToString();
                    Form1.svuiregadr[svi + 1024] = (mvars.ReadDataBuffer[8 + 1] * 256 + mvars.ReadDataBuffer[9 + 1]).ToString();
                    svstruiregadr += ":" + svi + "," + Form1.svuiregadr[svi] + "~" + Form1.svuiregadr[svi + 1024];
                    lst_uiregadr.TopIndex = svi;
                    Form1.tslblStatus.Text = "FPGA RADWR uiregtable read count：" + svi;
                    mp.doDelayms(10);
                }
            }
            if (svstruiregadr.Length > 1)
            {
                lst_uiregadr.Items.Clear();
                if (svstruiregadr.Substring(0, 1) == ":") svstruiregadr = svstruiregadr.Substring(1, svstruiregadr.Length - 1);
                lst_uiregadr.Items.AddRange(svstruiregadr.Split(':'));
            }
            Form1.tslblStatus.Text = "FPGA RADWR uiregtable items：" + (lst_uiregadr.Items.Count).ToString(); lst_uiregadr.TopIndex = 0;
            //ExNovaAGMA:
            if (mvars.demoMode == false && mvars.sp1.IsOpen) mvars.sp1.Close();
            lst_get1.TopIndex = lst_get1.Items.Count - 1;
            this.Enabled = true;
            #endregion 純按鍵操作
        }


        private void btn_uiregadrclsdef_Click(object sender, EventArgs e)
        {
            lst_uiregadr.Items.Clear();
        }
        private void btn_uiregadrclsdef_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lst_uiregadr.Items.Clear();
                List<string> lst = new List<string>();
                lst.AddRange(mvars.uiregadrdefault.Split('~'));
                for (int i = 0; i < lst.Count; i++)
                {
                    lst_uiregadr.Items.Add(lst[i] + "~" + lst[i].Split(',')[1].ToString());
                    Form1.svuiregadr.RemoveAt(i);
                    Form1.svuiregadr.Insert(i, mvars.uiregadr_default[i].Split(',')[2]);
                    Form1.svuiregadr.RemoveAt(i + (int)(mvars.GAMMA_SIZE / 8));   //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                    Form1.svuiregadr.Insert(i + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[i].Split(',')[2]);

                }
            }

        }
        
        private void lst_uiregadr_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lst_uiregadr_Click(object sender, EventArgs e)
        {
            if (lst_uiregadr.Items.Count == 0) return;

            if (mvars.uiregadr_default[lst_uiregadr.SelectedIndex].Split(',')[1] == "IDLE" || mvars.uiregadr_default[lst_uiregadr.SelectedIndex].Split(',')[1] == "")
            {
                return;
            }
            string value = "";
            int svi = lst_uiregadr.SelectedIndex;
            if (svi > -1) value = lst_uiregadr.Items[svi].ToString().Split(',')[1];
            string svmsg = mvars.uiregadr_default[lst_uiregadr.SelectedIndex].Split(',')[0] + "," + mvars.uiregadr_default[lst_uiregadr.SelectedIndex].Split(',')[1];
            if (MultiLanguage.DefaultLanguage == "en-US")
            {
                svmsg += "    Input algorithm table parameter" + "\r\n" + "\r\n" + "\r\n" + "parameter of Screen Right side ~ parameter of Screen Left side";
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            {
                svmsg += "    輸入演算法表格參數" + "\r\n" + "\r\n" + "\r\n" + "單屏右側參數 ~ 單屏左側參數";
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CN")
            {
                svmsg += "    输入演算法表格参数" + "\r\n" + "\r\n" + "\r\n" + "单屏右侧参数 ~ 单屏左侧参数";
            }
            // GetWindowRec 取得顯示表單位置
            RECT rct;
            GetWindowRect(mvars.handleIDMe, out rct);
            if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 1, "") == DialogResult.Cancel) { return; }
            else
            {
                if (value != "" && value.Split('~').Length == 2)
                {
                    if (svi > -1)
                    {
                        lst_uiregadr.Items.RemoveAt(svi);
                        lst_uiregadr.Items.Insert(svi, svi + "," + value.Split('~')[0].ToString().Trim() + "~" + value.Split('~')[1].ToString().Trim());
                    }
                }
                else
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") svmsg = @"please follow the format ""number~number""";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") svmsg = @"請依據格式輸入 ""數字~數字""";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") svmsg = @"请依据格式输入 ""数字~数字""";
                    value = "";
                    mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 1, "");
                    return;
                }
            }
        }



        private void btn_uiregadrw_Click(object sender, EventArgs e)
        {
            // GetWindowRec 取得顯示表單位置
            RECT rct;

            string svs;
            //int svi;
            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            string[] sRegDec;           //addr
            string[] sDataDec;          //data

            #region 清空 MCU Flash 用
            if (lst_mcuW62000.Items.Count <= 0)
            {
                string svmsg = "";
                if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x62000" + "\r\n" + "\r\n" + @"""OK""  will earse MCU flash and " + cmbFPGAsel.Text + " UIREG restore default"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x62000 沒有內容" + "\r\n" + "\r\n" + @"""OK""  將繼續進行清空微控閃存並且將 " + cmbFPGAsel.Text + " UIREG 還原預設值"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x62000 没有内容" + "\r\n" + "\r\n" + @"""OK""  将继续进行清空微控闪存 " + cmbFPGAsel.Text + " UIREG 还原预设值"; }
                // GetWindowRec 取得顯示表單位置
                GetWindowRect(mvars.handleIDMe, out rct);
                string value = "";
                if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }

                markreset(3, false, true);

                lst_get1.Items.Add("MCU Flash 0x62000 FlashWrite, ....");
                svs = "MCU Flash 0x62000 FlashWrite,";
                lst_get1.Items.Add(svs + " ....");

                for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                {
                    mvars.uiregadrdefault += "~" + mvars.uiregadr_default[j].Split(',')[0] + "," + mvars.uiregadr_default[j].Split(',')[2];
                    Form1.svuiregadr.RemoveAt(j);
                    Form1.svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                    Form1.svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8)); //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                    Form1.svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                }



                mvars.lblCmd = "MCUFLASH_W62000";
                mp.mhMCUFLASHWRITE("00062000", ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }

                int svms = 91;      /// Primary
                int svme = 283;     /// Primary
                for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                {
                    if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                }
                for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                {
                    if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                }
                lbl_GMAdrop.Text = svme.ToString();

                sRegDec = new string[svme];   //addr
                sDataDec = new string[svme];  //data
                for (int i = 0; i < svme; i++)
                {
                    sRegDec[i] = i.ToString();
                    sDataDec[i] = Form1.svuiregadr[i];
                }

                mvars.lblCmd = "FPGA_REG_W";
                mp.mpFPGAUIREGWarr(sRegDec, sDataDec);

                if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
                else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
                btn_mcuW62000.Enabled = true;
                return;
            }
            #endregion



            #region 與sendmessage共用
            if (chkNVBC.Checked)
            {
                if (MessageBox.Show(chkNVBC.Text, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) return;
            }

            if (mvars.svnova == false && mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            Button btn = (Button)sender;           
            btn.Enabled = false;
            mvars.lblCmd = "MCUFLASH_W62000";
            svs = "MCU Flash 0x62000 FlashWrite,";
            lst_get1.Items.Add(svs + " ....");
            mp.doDelayms(10);
            mp.cUIREGADRwENG("62000");
            if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
            else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
            btn.Enabled = true;
            string[] svss = mvars.strReceive.Split(',');
            lst_get1.Items.Add("  ↑，" + svss[7] + "s");
            lst_get1.TopIndex = lst_get1.Items.Count - 1;
            #endregion 與sendmessage共用

        }
        private void btn_mcuW64000_Click(object sender, EventArgs e)
        {
            RECT rct;                   //GetWindowRec 取得顯示表單位置
            Button btn = (Button)sender;
            ListBox lst = lst_mcuW60000;
            if (btn.Tag.ToString() == "60000") lst = lst_mcuW60000;
            else if (btn.Tag.ToString() == "62000") lst = lst_mcuW62000;
            else if (btn.Tag.ToString() == "64000") lst = lst_mcuW64000;
            int svms = 0;               //Primary
            int svme = 1;               //Primary
            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            string[] sRegDec;           //addr
            string[] sDataDec;          //data
            string svs;
            svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
            lst_get1.Items.Add(svs);
            markreset(3, false, true);
            string svmsg = "";
            string value = "";

            /*
                Primary MCU Flash 
                0x62000採特殊方式 切 FPGA A4096 / FPGA B4096 Total = 8192
                如果只有single duty則是填 0~6 , 10~282 = 283bytes
                如果是dual duty則是填 0~6 , 10~282 , 300~572 = 572bytes
                0x64000則不分區，所以僅填單側0~39個 = 40bytes
             */

            #region 清空 MCU Flash 用
            if (lst.Items.Count <= 0)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x" + btn_GMAdrop.Tag.ToString() + "\r\n" + "\r\n" + @"""OK""  will earse MCU flash and " + cmbFPGAsel.Text + " UIREG restore default"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x" + btn.Tag.ToString() + " 沒有內容" + "\r\n" + "\r\n" + @"""OK""  將繼續進行清空微控閃存並且將 " + cmbFPGAsel.Text + " UIREG 還原預設值"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x" + btn.Tag.ToString() + " 没有内容" + "\r\n" + "\r\n" + @"""OK""  将继续进行清空微控闪存 " + cmbFPGAsel.Text + " UIREG 还原预设值"; }
                // GetWindowRec 取得顯示表單位置
                GetWindowRect(mvars.handleIDMe, out rct);
                if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                mp.mhMCUFLASHWRITE(btn.Tag.ToString().PadLeft(8, '0'), ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                if (btn.Tag.ToString() == "60000")
                {

                }
                else if (btn.Tag.ToString() == "62000")
                {
                    for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                    {
                        Form1.svuiregadr[j] = mvars.uiregadr_default[j].Split(',')[2];
                        Form1.svuiregadr[j + (int)(mvars.GAMMA_SIZE / 8)] = mvars.uiregadr_default[j].Split(',')[2];
                    }

                    for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                    {
                        if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                    }
                    for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                    {
                        if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                    }
                    lbl_GMAdrop.Text = svme.ToString();

                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = i.ToString();
                        sDataDec[i] = Form1.svuiregadr[i];
                    }
                    mvars.lblCmd = "FPGA_REG_W";
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                else if (btn.Tag.ToString() == "64000")
                {
                    svme = 40;
                    for (int j = 0; j < svme; j++)
                    {
                        Form1.svuiregadr[j] = mvars.uiregadr_default[j].Split(',')[2];
                        Form1.svuiregadr[j + (int)(mvars.GAMMA_SIZE / 8)] = mvars.uiregadr_default[j].Split(',')[2];
                    }
                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = i.ToString();
                        sDataDec[i] = Form1.svuiregadr[i];
                    }
                    mvars.lblCmd = "FPGA_REG_W";
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
                else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
                btn.Enabled = true;
                return;
            }
            #endregion

            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "0x" + btn.Tag.ToString() + cmbFPGAsel.Text + "write"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x" + btn.Tag.ToString() + cmbFPGAsel.Text + "寫入"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x" + btn.Tag.ToString() + cmbFPGAsel.Text + "写入"; }
            // GetWindowRec 取得顯示表單位置
            GetWindowRect(mvars.handleIDMe, out rct);
            if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }

            #region 與sendmessage共用
            if (chkNVBC.Checked)
            {
                if (MessageBox.Show(chkNVBC.Text, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) return;
            }

            if (mvars.svnova == false && mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            if (btn.Tag.ToString() == "60000")
            {
                svme = lst.Items.Count;
            }
            else if (btn.Tag.ToString() == "62000")
            {
                if (mvars.dualduty == 1) svme = mvars.uiregadr_default.Length;
                else
                {
                    for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                    {
                        if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                    }
                    for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                    {
                        if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                    }
                    lbl_GMAdrop.Text = svme.ToString();
                }
                sRegDec = new string[svme];   //addr
                sDataDec = new string[svme];  //data
                for (int i = 0; i < svme; i++)
                {
                    sRegDec[i] = lst.Items[i].ToString().Split(',')[0];
                    sDataDec[i] = lst.Items[i].ToString().Split(',')[1];
                }
                btn.Enabled = false;
                mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
                lst_get1.Items.Add(svs + " ....");
                mp.doDelayms(10);
                mp.cUIREGADRwENG(btn.Tag.ToString(), sRegDec, sDataDec);
            }
            else if (btn.Tag.ToString() == "64000")
            {
                if (mvars.nualg) svme = 40;

                sRegDec = new string[svme];   //addr
                sDataDec = new string[svme];  //data
                for (int i = 0; i < svme; i++)
                {
                    sRegDec[i] = lst.Items[i].ToString().Split(',')[0];
                    sDataDec[i] = lst.Items[i].ToString().Split(',')[1];
                }
                btn.Enabled = false;
                mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
                lst_get1.Items.Add(svs + " ....");
                mp.doDelayms(10);
                mp.cUIREGADRwENG(btn.Tag.ToString(), sRegDec, sDataDec);
            }

            if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
            else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
            btn.Enabled = true;
            string[] svss = mvars.strReceive.Split(',');
            lst_get1.Items.Add("  ↑，" + svss[7] + "s");
            lst_get1.TopIndex = lst_get1.Items.Count - 1;
            #endregion 與sendmessage共用

        }


        private void btn_mcuW66000_Click(object sender, EventArgs e)
        {
            if (chk_NVBC.Checked && mvars.FPGAsel != 2) cmb_FPGAsel.SelectedIndex = 2;

            string svs = "";
            if (lst_mcuW66000.Items.Count <= 0)
            {
                string svmsg = "";
                if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x66000" + "\r\n" + "\r\n" + @"""OK"" to MCU flash erase"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x66000 裡面沒有內容" + "\r\n" + "\r\n" + @"""OK"" 繼續進行清空微控閃存"; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x66000 里面没有内容" + "\r\n" + "\r\n" + @"""OK"" 继续进行清空微控闪存"; }
                //MessageBox.Show(svmsg);
                // GetWindowRec 取得顯示表單位置
                RECT rct;
                GetWindowRect(mvars.handleIDMe, out rct);
                string value = "";
                if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                lst_get1.Items.Add("MCU Flash 0x66000 FlashWrite, ....");

                //mvars.GAMMA_SIZE = 8192;    //8192 目前是 Primary 機種專用，如果要跟其他機種共用則需要加條件式區別
                /*
                    Primary MCU Flash 
                    0x66000採特殊方式 切 A4096 / B4096 Total = 8192
                    因為這樣所以前9個Addr需要共用 
                    A填 0~9 + 10~282 = 283bytes
                    B也是填 0~9 + 300~572 = 283bytes
                    0x64000則不分區，所以僅填單側0~39個 = 40bytes
                 */
                byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                svs = "MCU Flash 0x66000 FlashWrite,";
                lst_get1.Items.Add(svs + " ....");
                mvars.lblCmd = "MCUFLASH_W66000";
                mp.mhMCUFLASHWRITE("00066000", ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }
                if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
                else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
                btn_mcuW66000.Enabled = true;
                return;
            }
            //int svi;

            #region 與sendmessage共用
            if (chkNVBC.Checked)
            {
                if (MessageBox.Show(chkNVBC.Text, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) return;
            }

            if (mvars.svnova == false && mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            Button btn = (Button)sender;

            string[] sRegDec = new string[lst_mcuW66000.Items.Count];   //addr
            string[] sDataDec = new string[lst_mcuW66000.Items.Count];  //data
            for (int i = 0; i < lst_mcuW66000.Items.Count; i++)
            {
                sRegDec[i] = lst_mcuW66000.Items[i].ToString().Split(',')[0];
                sDataDec[i] = lst_mcuW66000.Items[i].ToString().Split(',')[1];
            }
            btn.Enabled = false;
            mvars.lblCmd = "MCUFLASH_W66000";
            svs = "MCU Flash 0x66000 FlashWrite,";
            lst_get1.Items.Add(svs + " ....");
            mp.doDelayms(10);
            mp.cUIREGADRwALL("66000", sRegDec, sDataDec);
            if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
            else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
            btn.Enabled = true;
            string[] svss = mvars.strReceive.Split(',');
            lst_get1.Items.Add("  ↑，" + svss[7] + "s");
            lst_get1.TopIndex = lst_get1.Items.Count - 1;
            #endregion 與sendmessage共用
        }

        private void lst_mcuW66000_Click(object sender, EventArgs e)
        {
            string value = "";
            int svi = lst_mcuW66000.SelectedIndex;
            if (svi > -1)
            {
                value = lst_mcuW66000.Items[svi].ToString();
            }


            if (mp.InputBox(mvars.strUInameMe, "\r\n" + "\r\n" +
                    "    Input MCU FLASH0x66000 contant", ref value, 65) == DialogResult.Cancel)
            { return; }//MessageBox.Show("Please input UUT ID", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            else
            {
                if (value != "")
                {
                    if (svi > -1)
                    {
                        lst_mcuW66000.Items.RemoveAt(svi);
                        lst_mcuW66000.Items.Insert(svi, value);
                    }
                    else lst_mcuW66000.Items.Add(value);
                }
            }


            //if (lst_mcuW66000.Items.Count == 0) return;

            //string value = "";
            //int svi = lst_mcuW66000.SelectedIndex;
            //if (svi > -1)
            //{
            //    value = lst_mcuW66000.Items[svi].ToString().Split(',')[1];
            //}

            //string svmsg = "";
            //if (MultiLanguage.DefaultLanguage == "en-US") 
            //{
            //    svmsg = "\r\n" + "\r\n" + "    Input MCU FLASH0x66000 contant (Number only)" + "\r\n" + "\r\n" + "    " + mvars.strR66Kdefault.Split('~')[svi].Split(',')[0] + "," +
            //        mvars.strR66Kdefault.Split('~')[svi].Split(',')[1];
            //    if (svi == 0) { svmsg += "   for exsample，USED:1，IDLE:255"; }
            //}
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT") 
            //{
            //    svmsg = "\r\n" + "\r\n" + "    請輸入微控閃存 0x66000 內容 (僅限數字)" + "\r\n" + "\r\n" + "    " + mvars.strR66Kdefault.Split('~')[svi].Split(',')[0] + "," +
            //        mvars.strR66Kdefault.Split('~')[svi].Split(',')[1];
            //    if (svi == 0) { svmsg += "  說明，使用中:1，閒置:255"; }
            //}
            //else if (MultiLanguage.DefaultLanguage == "zh-CN") 
            //{
            //    svmsg = "\r\n" + "\r\n" + "    请输入微控闪存 0x66000 内容 (僅限數字)" + "\r\n" + "\r\n" + "    " + mvars.strR66Kdefault.Split('~')[svi].Split(',')[0] + "," +
            //        mvars.strR66Kdefault.Split('~')[svi].Split(',')[1];
            //    if (svi == 0) { svmsg += "   说明，使用中:1，闲置:255"; }
            //}

            //if (mp.InputBox(mvars.strUInameMe, svmsg, ref value, 95) == DialogResult.Cancel)
            //{ return; }//MessageBox.Show("Please input UUT ID", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
            //else
            //{
            //    if (value.Trim() != "")
            //    {
            //        if (svi > -1)
            //        {
            //            if (svi == 0 && (value != "1" && value != "255"))
            //            {
            //                if (MultiLanguage.DefaultLanguage == "en-US") svmsg = @"    Item 0 need keyin ""1"" for USED or ""255"" for IDLE";
            //                else if (MultiLanguage.DefaultLanguage == "zh-CHT") svmsg = @"    項目 0 需要填寫 ""1"" 使用中 or ""255"" 閒置";
            //                else if (MultiLanguage.DefaultLanguage == "zh-CN") svmsg = @"    项目 0 需要填写 ""1"" for 使用中 or ""255"" 闲置";
            //            }
            //            lst_mcuW66000.Items.RemoveAt(svi);
            //            lst_mcuW66000.Items.Insert(svi, svi + "," + value);
            //        }
            //        else lst_mcuW66000.Items.Add(value);
            //    }
            //}
        }

        private void btn_mcuW66000cls_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lst_mcuW66000.Items.Count > 0)
                {
                    string svmsg = "";
                    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x66000" + "\r\n" + "\r\n" + @"""OK"" to MCU flash erase and re-fill default"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x66000 裡面沒有內容" + "\r\n" + "\r\n" + @"""OK"" 清空並填入預設值"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x66000 里面没有内容" + "\r\n" + "\r\n" + @"""OK"" 清空并填入预设值"; }
                    //MessageBox.Show(svmsg);
                    // GetWindowRec 取得顯示表單位置
                    RECT rct;
                    GetWindowRect(mvars.handleIDMe, out rct);
                    string value = "";
                    if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                }
                lst_mcuW66000.Items.Clear();
                lst_mcuW66000.Items.Add("0,255");   // 已記錄=1，未紀錄=255，
                lst_mcuW66000.Items.Add("1,0");     // (0保留)Sender
                lst_mcuW66000.Items.Add("2,0");     // (0保留)Port
                lst_mcuW66000.Items.Add("3,0");     // (P)Card or BoxNumber
                lst_mcuW66000.Items.Add("4,0");     // (C)X
                lst_mcuW66000.Items.Add("5,0");     // (X)Y
                lst_mcuW66000.Items.Add("6,0");     // (Y)W
                lst_mcuW66000.Items.Add("7,960");   // (W)H
                lst_mcuW66000.Items.Add("8,540");   // (H)畫線用
                lst_mcuW66000.Items.Add("9,0");     // (0保留)畫線用
                lst_mcuW66000.Items.Add("10,0");    // (0保留)
                lst_mcuW66000.Items.Add("11,0");    // (0保留)
            }
        }

        private void btn_mcuW66000cls_Click(object sender, EventArgs e) { lst_mcuW62000.Items.Clear(); }


        private void btn_mcuR66000_Click(object sender, EventArgs e)
        {
            #region 純按鍵單次操作
            if (mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            else
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { MessageBox.Show("demo mode"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { MessageBox.Show("演示模式"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { MessageBox.Show("演示模式"); }
            }
            this.Enabled = false;
            markreset(11, false, true);
            mvars.lblCmd = "MCU_FLASH_R66000";
            mp.mhMCUFLASHREAD("00066000", 8192);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
            else
            {
                mvars.lstmcuR66000.Items.Clear();
                mvars.lstmcuR66000.Items.AddRange(mvars.strR66K.Split('~'));
            }
            if (mvars.demoMode == false) { mp.CommClose(); }
            this.Enabled = true;
            #endregion 純按鍵操作

        }

        private void lst_mcuR62000_Click(object sender, EventArgs e)
        {
            ListBox lst = (ListBox)sender;
            if (lst.Tag.ToString() == "60000")
            {
                if (lst.Items.Count <= 0) lbl_mcuR60000click.Text = "< ";
                else
                {
                    lbl_mcuR60000click.Text = lst_mcuR60000.SelectedIndex.ToString();
                    Form1.tslblStatus.Text = "0x60000 rd MCU Flash items：" + (mvars.lstmcuR60000.Items.Count).ToString();
                }
            }
            else if (lst.Tag.ToString() == "62000")
            {
                if (lst.Items.Count <= 0) lbl_mcuR62000click.Text = "< ";
                else
                {
                    lbl_mcuR62000click.Text = lst_mcuR62000.SelectedIndex.ToString();
                    Form1.tslblStatus.Text = "0x62000 rd MCU Flash items：" + (mvars.lstmcuR62000.Items.Count).ToString();
                }
            }
            else if (lst.Tag.ToString() == "64000")
            {
                if (lst.Items.Count <= 0) lbl_mcuR64000click.Text = "< ";
                else
                {
                    lbl_mcuR64000click.Text = lst_mcuR64000.SelectedIndex.ToString();
                    Form1.tslblStatus.Text = "0x64000 rd MCU Flash items：" + (mvars.lstmcuR64000.Items.Count).ToString();
                }
            }
        }


        private void cmb_deviceID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                Form1.cmbdeviceid.SelectedIndex = cmb_deviceID.SelectedIndex;
                //Form1.cmbdeviceid.Text = Form1.cmbdeviceid.Items[cmb_deviceID.SelectedIndex].ToString();
                //cmb_deviceID.Text = cmb_deviceID.Items[cmb_deviceID.SelectedIndex].ToString();
            }
        }

        private void btn_mcuW64000cls_Click(object sender, EventArgs e) { lst_mcuW64000.Items.Clear(); }

        private void btn_mcuW64000cls_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                lst_mcuW64000.Items.Clear();
                int svms = 39;      /// Primary
                int svme = 39;      /// Primary
                for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                {
                    if (mvars.uiregadr_default[i].IndexOf("WT_PG_RED_ENG", 0) != -1) { svms = i; break; }
                    lst_mcuW64000.Items.Add(mvars.uiregadrdefault.Split('~')[i]);
                }
            }
        }



        #endregion 20230328

        private void btn_mcuW60000_Click(object sender, EventArgs e)
        {
            RECT rct;                   //GetWindowRec 取得顯示表單位置
            Button btn = (Button)sender;
            ListBox lst = lst_mcuW60000;
            if (btn.Tag.ToString() == "60000" || btn.Tag.ToString() == "30000") lst = lst_mcuW60000;
            else if (btn.Tag.ToString() == "62000" || btn.Tag.ToString() == "32000") lst = lst_mcuW62000;
            int svms = 0;               //Primary
            int svme = 1;               //Primary
            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            //if (mvars.deviceID.Substring(0, 2) == "10") BinArr = new byte[2048];
            string[] sRegDec;           //addr
            string[] sDataDec;          //data
            string svs;
            svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
            lst_get1.Items.Add(svs );
            markreset(3, false, true);

            /*
                Primary MCU Flash 
                0x62000採特殊方式 切 FPGA A4096 / FPGA B4096 Total = 8192
                如果只有single duty則是填 0~6 , 10~282 = 283bytes
                如果是dual duty則是填 0~6 , 10~282 , 300~572 = 572bytes
                0x64000則不分區，所以僅填單側0~39個 = 40bytes
            */

            if (mvars.deviceID.Substring(0, 2) == "10") mvars.FPGAsel = 0;

            #region 清空 MCU Flash 用，寫完後直接離開
            if (lst.Items.Count <= 0)
            {
                string svmsg = "";
                if (mvars.deviceID.Substring(0, 2) == "05")
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x" + btn.Tag.ToString() + "\r\n" + "\r\n" + @"""OK""  will earse MCU flash and " + cmbFPGAsel.Text + " UIREG restore default"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x" + btn.Tag.ToString() + " 沒有內容" + "\r\n" + "\r\n" + @"""OK""  將繼續進行清空微控閃存並且將 " + cmbFPGAsel.Text + " UIREG 還原預設值"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x" + btn.Tag.ToString() + " 没有内容" + "\r\n" + "\r\n" + @"""OK""  将继续进行清空微控闪存 " + cmbFPGAsel.Text + " UIREG 还原预设值"; }
                }
                else 
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "There is no content in the container 0x" + btn.Tag.ToString() + "\r\n" + "\r\n" + @"""OK""  will earse MCU flash and UIREG restore default"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x" + btn.Tag.ToString() + " 沒有內容" + "\r\n" + "\r\n" + @"""OK""  將繼續進行清空微控閃存並且將 UIREG 還原預設值"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x" + btn.Tag.ToString() + " 没有内容" + "\r\n" + "\r\n" + @"""OK""  将继续进行清空微控闪存 UIREG 还原预设值"; }
                }
                // GetWindowRec 取得顯示表單位置
                GetWindowRect(mvars.handleIDMe, out rct);
                string value = "";
                if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                mp.mhMCUFLASHWRITE(btn.Tag.ToString().PadLeft(8, '0'), ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                if (btn.Tag.ToString() == "60000" || btn.Tag.ToString() == "30000") { }
                else if (btn.Tag.ToString() == "62000" || btn.Tag.ToString() == "32000")
                {
                    for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                    {
                        Form1.svuiregadr[j] = mvars.uiregadr_default[j].Split(',')[2];
                        Form1.svuiregadr[j + (int)(mvars.GAMMA_SIZE / 8)] = mvars.uiregadr_default[j].Split(',')[2];
                    }

                    for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                    {
                        if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                    }
                    for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                    {
                        if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                    }
                    lbl_GMAdrop.Text = svme.ToString();

                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = i.ToString();
                        sDataDec[i] = Form1.svuiregadr[i];
                    }
                    mvars.lblCmd = "FPGA_REG_W";
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
                else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }
                btn.Enabled = true;
                return;
            }
            #endregion

            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (mvars.FPGAsel != 2)
                {
                    string svmsg = "";
                    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "0x" + btn.Tag.ToString() + " single side write @ " + cmbFPGAsel.Text; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = "0x" + btn.Tag.ToString() + " 單側寫入 @ " + cmbFPGAsel.Text; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = "0x" + btn.Tag.ToString() + " 单侧写入 @ " + cmbFPGAsel.Text; }
                    // GetWindowRec 取得顯示表單位置
                    GetWindowRect(mvars.handleIDMe, out rct);
                    string value = "";
                    if (mp.InputBox("", svmsg, ref value, rct.Left + (rct.Right - rct.Left) / 2, rct.Top + (rct.Bottom - rct.Top) / 2, 0, 0, 2, "") == DialogResult.Cancel) { return; }
                }
            }

            #region 與sendmessage共用
            if (chkNVBC.Checked)
            {
                if (MessageBox.Show(chkNVBC.Text, mvars.strUInameMe, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) return;
            }

            if (mvars.svnova == false && mvars.demoMode == false)
            {
                if (mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false") { MessageBox.Show("COM error" + "\r\n" + "\r\n" + "Please check", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Color.Blue; Form1.tslblHW.ForeColor = Color.White;
            }
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (btn.Tag.ToString() == "60000")
                {
                    mvars.lblCmd = "MCU_FLASH_R" + btn.Tag.ToString();
                    mp.mhMCUFLASHREAD(btn.Tag.ToString().PadLeft(8, '0'), BinArr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                    else
                    {
                        if (mvars.strR60K.Length > 1)
                        {
                            mvars.lstmcuR60000.Items.AddRange(mvars.strR60K.Split('~'));
                            for (int svi = 0; svi < BinArr.Length / 4; svi++)
                            {
                                BinArr[svi * 4 + 0] = (Byte)(Convert.ToInt32(mvars.lstmcuR60000.Items[svi].ToString().Split(',')[0]) / 256);
                                BinArr[svi * 4 + 1] = (Byte)(Convert.ToInt32(mvars.lstmcuR60000.Items[svi].ToString().Split(',')[0]) % 256);
                                BinArr[svi * 4 + 2] = (Byte)(Convert.ToInt32(mvars.lstmcuR60000.Items[svi].ToString().Split(',')[1]) / 256);
                                BinArr[svi * 4 + 3] = (Byte)(Convert.ToInt32(mvars.lstmcuR60000.Items[svi].ToString().Split(',')[1]) % 256);
                            }
                        }
                        else
                        {
                            lbl_mcuR60000click.Text = "no record";
                            Form1.tslblStatus.Text = "0x60000 rd MCU Flash items：0";
                        }



                        //Save File
                        string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr_60000R.bin";
                        mp.SaveBinFile(path, BinArr);


                        sRegDec = new string[lst.Items.Count];   //addr
                        sDataDec = new string[lst.Items.Count];  //data
                        for (int i = 0; i < svme; i++)
                        {
                            sRegDec[i] = lst.Items[i].ToString().Split(',')[0];
                            sDataDec[i] = lst.Items[i].ToString().Split(',')[1];
                        }
                        if (mvars.FPGAsel == 2)
                        {
                            for (int svi = 0; svi < sRegDec.Length; svi++)
                            {
                                BinArr[svi * 4 + 0] = (Byte)(Convert.ToInt32(sRegDec[svi]) / 256);
                                BinArr[svi * 4 + 1] = (Byte)(Convert.ToInt32(sRegDec[svi]) % 256);
                                BinArr[svi * 4 + 2] = (Byte)(Convert.ToInt32(sDataDec[svi]) / 256);
                                BinArr[svi * 4 + 3] = (Byte)(Convert.ToInt32(sDataDec[svi]) % 256);
                                BinArr[svi * 4 + 0 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(sRegDec[svi]) / 256);
                                BinArr[svi * 4 + 1 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(sRegDec[svi]) % 256);
                                BinArr[svi * 4 + 2 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(sDataDec[svi]) / 256);
                                BinArr[svi * 4 + 3 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(sDataDec[svi]) % 256);
                            }
                        }
                        else
                        {
                            for (int svi = 0; svi < sRegDec.Length; svi++)
                            {
                                BinArr[svi * 4 + 0 + mvars.FPGAsel * mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(sRegDec[svi]) / 256);
                                BinArr[svi * 4 + 1 + mvars.FPGAsel * mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(sRegDec[svi]) % 256);
                                BinArr[svi * 4 + 2 + mvars.FPGAsel * mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(sDataDec[svi]) / 256);
                                BinArr[svi * 4 + 3 + mvars.FPGAsel * mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(sDataDec[svi]) % 256);
                            }
                        }
                        //Checksum
                        UInt16 checksum = mp.CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                        BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                        BinArr[BinArr.Length - 1] = (byte)(checksum % 256);

                        //Save File
                        path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr_60000WR.bin";
                        mp.SaveBinFile(path, BinArr);

                        mvars.lblCmd = "MCU_FLASH_W" + btn.Tag.ToString();
                        mp.mhMCUFLASHWRITE(btn.Tag.ToString(), ref BinArr);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }

                        mvars.lblCmd = "MCU_FLASH_R" + btn.Tag.ToString();
                        mp.mhMCUFLASHREAD(btn.Tag.ToString(), BinArr);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
                        else
                        {
                            mvars.lstmcuR60000.Items.Clear();
                            mvars.lstmcuR60000.Items.AddRange(mvars.strR60K.Split('~'));
                        }
                    }
                }
                else if (btn.Tag.ToString() == "62000")
                {
                    if (mvars.dualduty == 1) svme = mvars.uiregadr_default.Length;
                    else
                    {
                        for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                        {
                            if (mvars.uiregadr_default[i].IndexOf("WT_GMA", 0) != -1) { svms = i; break; }
                        }
                        for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                        {
                            if (mvars.uiregadr_default[i].IndexOf("IDLE", 0) != -1) { svme = i; break; }
                        }
                        lbl_GMAdrop.Text = svme.ToString();
                    }
                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = lst.Items[i].ToString().Split(',')[0];
                        sDataDec[i] = lst.Items[i].ToString().Split(',')[1];
                    }
                    btn.Enabled = false;
                    mvars.lblCmd = "MCUFLASH_W" + btn.Tag.ToString();
                    svs = "MCU Flash 0x" + btn.Tag.ToString() + " FlashWrite,";
                    lst_get1.Items.Add(svs + " ....");
                    mp.doDelayms(10);
                    mp.cUIREGADRwENG(btn.Tag.ToString(), sRegDec, sDataDec);
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "10")
            {
                if (btn.Tag.ToString() == "30000")
                {
                    sRegDec = new string[lst.Items.Count];   //addr
                    sDataDec = new string[lst.Items.Count];  //data
                    for (int svi = 0; svi < sRegDec.Length; svi++)
                    {
                        BinArr[svi * 4 + 0] = (Byte)(Convert.ToInt32(lst.Items[svi].ToString().Split(',')[0]) / 256);
                        BinArr[svi * 4 + 1] = (Byte)(Convert.ToInt32(lst.Items[svi].ToString().Split(',')[0]) % 256);
                        BinArr[svi * 4 + 2] = (Byte)(Convert.ToInt32(lst.Items[svi].ToString().Split(',')[1]) / 256);
                        BinArr[svi * 4 + 3] = (Byte)(Convert.ToInt32(lst.Items[svi].ToString().Split(',')[1]) % 256);
                    }
                    //Checksum
                    UInt16 checksum = mp.CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                    BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                    BinArr[BinArr.Length - 1] = (byte)(checksum % 256);

                    //Save File
                    string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr_" + btn.Tag.ToString() + "W.bin";
                    mp.SaveBinFile(path, BinArr);

                    mvars.lblCmd = "MCU_FLASH_W" + btn.Tag.ToString();
                    mp.mhMCUFLASHWRITE(btn.Tag.ToString(), ref BinArr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }

                    mvars.lblCmd = "MCU_FLASH_R" + btn.Tag.ToString();
                    mp.mhMCUFLASHREAD(btn.Tag.ToString(), BinArr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
                    else
                    {
                        mvars.lstmcuR60000.Items.Clear();
                        mvars.lstmcuR60000.Items.AddRange(mvars.strR60K.Split('~'));
                    }
                }
            }

            if (mvars.errCode == "0") lst_get1.Items.Add(svs + "DONE");
            else { lst_get1.Items.Add(svs + "ERROR,ErrCode," + mvars.errCode); }

            btn.Enabled = true;
            if (mvars.strReceive.Length > 10)
            {
                if (mvars.strReceive.Split(',').Length > 7)
                {
                    string[] svss = mvars.strReceive.Split(',');
                    lst_get1.Items.Add("  ↑，" + svss[7] + "s");
                    lst_get1.TopIndex = lst_get1.Items.Count - 1;
                }
            }

            #endregion 與sendmessage共用
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lst_mcuW62000.Items.Clear();
            for (int i = 0; i < lst_mcuR62000.Items.Count; i++)
            {
                lst_mcuW62000.Items.Add(lst_mcuR62000.Items[i].ToString());
            }
        }

        private void btn_mcuR64000cls_Click(object sender, EventArgs e)
        {
            lst_mcuR64000.Items.Clear();
        }

        private void btn_mcuR66000cls_Click(object sender, EventArgs e)
        {
            lst_mcuR66000.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lst_mcuW60000.Items.Clear();
            for (int i = 0; i < lst_mcuR60000.Items.Count; i++)
            {
                lst_mcuW60000.Items.Add(lst_mcuR60000.Items[i].ToString());
            }
        }
    }

}
