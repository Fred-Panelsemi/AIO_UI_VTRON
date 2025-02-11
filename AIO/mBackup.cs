using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AIO
{
    class mBackup
    {
        [DllImport("winmm")]
        static extern uint timeGetTime();

        [DllImport("winmm")]
        public static extern void timeBeginPeriod(int t);

        [DllImport("winmm")]
        public static extern void timeEndPeriod(int t);


        #region Stat
        public static int mNumOfByteWrite = 0;
        public static int mNumOfByteReturn = 0;
        public static uint mQueue = 0u;
        public static uint mQa = 0u;



        public enum Stat
        {
            SUCCESS = 0,
            INITIAL_STATE = -2,
            CONST_IF_BASE = 1,
            CONST_IF_RANGE = 9999,
            IF_TSKSTR = 100,
            IF_TSKABT = 101,
            IF_GENERL = 102,
            IF_USERHANDLE = 103,
            IF_TESTPOINT = 104,
            IF_CHECKPASS = 105,
            IF_CHECKFAIL = 106,
            IF_USBDEV_OPEN = 107,
            IF_USBDEV_CLOSE = 108,
            IF_USB_SET_TOUT = 109,
            IF_USB_GET_TOUT = 110,
            IF_USB_NO_DEV = 111,
            CONST_WN_BASE = 10000,
            CONST_WN_RANGE = 19999,
            WN_GET_RANDOM_FAIL = 10001,
            WN_USBDEV_NOTOPEN = 10002,
            WN_4K_PAT_FLASH_EMPTY = 10003,
            CONST_ER_BASE = -10000,
            CONST_ER_RANGE = -19999,
            ER_SYSTEM = -10001,
            ER_SYS_KILLPR = -10021,
            ER_SYS_CLSCON = -10022,
            ER_SYS_CLSDIC = -10023,
            ER_SYS_OPCDAT = -10034,
            ER_SYS_NODIRE = -10035,
            ER_SYS_INITIL = -10036,
            ER_SYS_USRACN = -10037,
            ER_SYS_USRPWD = -10038,
            ER_SYS_USB_SETTOUT_FAIL = -10039,
            ER_SYS_USB_GETTOUT_FAIL = -10040,
            CONST_ER2_BASE = -20000,
            CONST_ER2_RANGE = -29999,
            ER2_NODATA_INPUT = -20001,
            ER2_SIWRITE_FAIL = -20002,
            ER2_SIWRITE_RTN_NOTMATCH = -20003,
            ER2_SIREAD_FAIL = -20004,
            ER2_SIREAD_RTN_NOTMATCH = -20005,
            ER2_USB_CMD_PASSVAL_FAIL = -20006,
            ER2_REGADDR_OUTRANGE = -20007,
            ER2_GET_ERRPARA = -20008,
            ER2_MCUVER_READ_FAIL = -20009,
            ER2_FPGA_RESET_ERR = -20010,
            ER2_FLASADDR_OUTRANGE = -20011,
            ER2_FLASH_ERASE_ERR = -20012,
            ER2_I2C_ADDR_ERR = -20013,
            ER2_REQUIRE_QTY_ERR = -20014,
            ER2_CURSOR_ENABLE_FAIL = -20015,
            ER2_CURSOR_WRITE_LOC_FAIL = -20016,
            ER2_CURSOR_COLOR_SET_FAIL = -20017,
            ER2_PATQTY_READ_FAIL = -20018,
            ER2_TT_MDF_UD_FAIL = -20019,
            ER2_PATQTY_INTVL_READ_FAIL = -20020,
            ER2_PAT_NO_LST_READ_FAIL = -20021,
            ER2_PAT_NO_LST_WRITE_FAIL = -20022,
            ER2_RGB_MSK_INV_SET_FAIL = -20023,
            ER2_GRAY_LVL_OUTRANGE = -20024,
            ER2_TARGET_8922_NOT_FOUND = -20025,
            ER2_TARGET_8902_NOT_FOUND = -20026,
            ER2_GPM_NOT_FOUND_TIMING = -20027,
            ER2_VDO_NOT_FOUND_TIMING = -20028,
            ER2_VIDEO_SPLIT_NO_CMD_DATA = -20029,
            ER2_PHOTO_SPLIT_NO_CMD_DATA = -20030,
            ER2_FPGA_READ_VER_ERR = -20031,
            ER2_FLASH_READ_ASIGN_QTY_ERR = -20032,
            ER2_FLASH_WRITE_ASIGN_DAT_ERR = -20033,
            ER2_SPI_FREQ_SET_FAIL = -20034,
            ER2_LSB_VER_READ_FAIL = -20035,
            ER2_LSB_I2C_BUS_ENAB_FAIL = -20036,
            ER2_LSB_TSTPGM_SET_FAIL = -20037,
            ER2_DB_NT_TCON_RST_FAIL = -20038,
            ER2_DB_TCON_RST_FAIL = -20039,
            ER2_ONorOFF_TCON_RELEASE_FAIL = -20040,
            ER2_LSB_TST_PGM_HIZ_FAIL = -20041,
            ER2_READ_DEMURA_PARA_FUNC_FAIL = -20042,
            ER2_FLASH_TARGET_ERR = -20043,
            ER2_FLASH_READ_QTY_NOTMATCH = -20044,
            ER2_LSB_FLSH_STAREG_READ_FAIL = -20045,
            ER2_LSB_FLSH_WP_SET_FAIL = -20046,
            ER2_DLB_FLSH_WP_SET_FAIL = -20047,
            ER2_POL_TIME_ERR = -20048,
            ER2_POL_COUNT_ERR = -20049,
            ER2_ERASE_BLK_QTY_ERR = -20050,
            ER2_ERASE_SCTR_QTY_ERR = -20051,
            ER2_FLSH_ADDR_ERR = -20052,
            ER2_FLSH_WP_SET_FAIL = -20053,
            ER2_FLSH_INFO_READ_FAIL = -20054,
            ER2_ELCS_SKIN_SWITCH_FAIL = -20055,
            ER2_TGT_TCON_ERR = -20056,
            ER2_SW_RSET_FAIL = -20057,
            ER2_DLY_TIME_ERR = -20058,
            ER2_DEVID_WRITE_FAIL = -20059,
            ER2_DEVID_READ_FAIL = -20060,
            CONST_ER3_BASE = -30000,
            CONST_ER3_RANGE = -39999,
            ER3_EXCEP_IN_WHILELOOP = -30001,
            ER3_EXCEP_OUT_WHILELOOP = -30002,
            ER3_EXCEP_INFUNC = -30003,
            ER3_ERRFORMAT_INPUT = -30004,
            ER3_REG_WRITE_FAIL = -30005,
            ER3_REG_WRI_VERIFY_FAIL = -30006,
            ER3_REG_READ_FAIL = -30007,
            ER3_FLASH_READ_FAIL = -30008,
            ER3_FLASH_WRITE_FAIL = -30009,
            ER3_I2C_READ_FAIL = -30010,
            ER3_I2C_WRITE_FAIL = -30011,
            ER3_I2C_READ_QTY_NOTMATCH = -30012,
            ER3_TSTPGM_SET_FAIL = -30013,
            ER3_BIT_SEL_SET_FAIL = -30014,
            ER3_LD_ENAB_SET_FAIL = -30015,
            ER3_MCU_RESET_FAIL = -30016,
            ER3_HV_SYNC_SET_FAIL = -30017,
            ER3_WP_GRAY_LVL_SET_FAIL = -30018,
            ER3_I2C_CMD_WRONG_LENGTH = -30019,
            ER3_AHB_BRIDGE_UNLOCK = -30020,
            ER3_I2C_WRITE_DATA_EMPTY = -30021,
            ER3_I2C_CMD_WRITE_FAIL = -30022,
            ER3_I2C_WRONG_READ_QTY = -30023,
            ER3_I2C_CMD_READ_FAIL = -30024,
            ER3_I2C_FRC_READ_POLLING_TOUT = -30025,
            ER3_I2C_FRC_READ_NOTFOND_TARGET = -30026,
            ER3_IN8922_I2C_CMD_FAIL = -30027,
            ER3_IN8902_I2C_CMD_FAIL = -30027,
            ER3_LR_ALIGN_SET_FAIL = -30028,
            ER3_8K_RW_B0_NOTMATCH = -30029,
            ER3_8K_SPI_BUS_SELE_FAIL = -30030,
            ER3_8K_UPDATE_ALL_REG_FAIL = -30031,
            ER3_8K_PC_MOD_SET_FAIL = -30032,
            ER3_8K_PWR_12V_SET_FAIL = -30033,
            ER3_DB_CHK_BOARD_FAIL = -30034,
            ER3_LSB_MCU_VER_ERR = -30035,
            ER3_DB_TST_PGM_SET_FAIL = -30036,
            ER3_LSB_FLASH_TST_PGM_FAIL = -30037,
            ER3_I2C_MAX_ADDR_OVER = -30038,
            ER3_I2C_RADDR_LST_QTY_ERR = -30039,
            ER3_LSB_FLSH_STAREG_SET_FAIL = -30040,
            ER3_DLB_FLSH_STAREG_SET_FAIL = -30041,
            ER3_DLB_CHIP_ERASE_FAIL = -30042,
            ER3_LSB_CHIP_ERASE_FAIL = -30043,
            ER3_DLB_FLSH_BLK_ERASE_FAIL = -30044,
            ER3_LSB_FLSH_BLK_ERASE_FAIL = -30045,
            ER3_DLB_FLSH_SCTR_ERASE_FAIL = -30046,
            ER3_LSB_FLSH_SCTR_ERASE_FAIL = -30047,
            ER3_DLB_FLSH_BZ_FLAG_READ_ERR = -30048,
            ER3_LSB_FLSH_BZ_FLAG_READ_ERR = -30049,
            ER3_CHK_DFLT_CODE_QTY_ERR = -30050,
            ER3_CHK_DFLT_CODE_RESULT_FAIL = -30051,
            ER3_LSB_FLSH_STAREG_WRI_FAIL = -30052,
            ER3_DLB_FLSH_STAREG_WRI_FAIL = -30053,
            ER3_DLB_FLSH_STAREG_READ_FAIL = -30054,
            ER3_DLB_FLSH_STAREG_COMPARE_FAIL = -30055,
            ER3_DLB_CHG_MODE_FAIL = -30056,
            ER3_SDP_DEMU_ONOFF_FAIL = -30057,
            ER3_SI_FLUSH_FAIL = -30058,
            ER3_WSK_INVPRT = -33001,
            ER3_WSK_COMERR = -33002,
            ER3_WSK_NOPORT = -33003,
            ER3_WSK_PRTCFG = -33004,
            ER3_WSK_COMTMO = -33005,
            ER3_WSK_WRNIPA = -33006,
            ER3_WSK_ETHCON = -33007,
            ER3_WSK_TCPLST = -33008,
            CONST_ER4_BASE = -40000,
            CONST_ER4_RANGE = -49999,
            ER4_SYS_NODLL = -40001,
            ER4_ENCRYPT_FAIL = -40002,
            ER4_DECRYPT_FAIL = -40003
        }

        public static Stat Set_pindef_DB(byte pindef)
        {
            Stat stat; //= Stat.SUCCESS;
            try
            {
                Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
                //Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                mvars.RS485_WriteDataBuffer[0] = 15;
                mvars.RS485_WriteDataBuffer[1] = pindef;
                mNumOfByteWrite = 16;
                mvars.NumBytesToRead = 1;
                if (USB_CmdSend_NoRead())
                {
                    stat = Stat.SUCCESS;
                }
                else
                {
                    stat = Stat.ER2_DEVID_READ_FAIL;
                    mp.funSaveLogs(stat + ",Signal set pin def fail of Download board,Set signal pin def fail");
                }
                return stat;
            }
            catch
            {
                stat = Stat.ER3_EXCEP_INFUNC;
                //ExceptionHandler(stat, ex);
                return stat;
            }
        }
        public static Stat Set_SPI_Signal_DB(byte VIN, byte WP, byte ROMSEL, byte AGING, byte RESERVE, byte SCL, byte SDA, byte LED, byte TST_PGM, byte SW, byte BINT, byte OUTCTRL, byte A0, byte A1)
        {
            Stat stat;// = Stat.SUCCESS;
            try
            {
                //Array.Clear(mReadDataBuffer, 0, mReadDataBuffer.Length);
                Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
                mvars.RS485_WriteDataBuffer[0] = 30;
                mvars.RS485_WriteDataBuffer[1] = VIN;
                mvars.RS485_WriteDataBuffer[2] = WP;
                mvars.RS485_WriteDataBuffer[3] = AGING;
                mvars.RS485_WriteDataBuffer[4] = ROMSEL;
                mvars.RS485_WriteDataBuffer[5] = RESERVE;
                mvars.RS485_WriteDataBuffer[6] = SCL;
                mvars.RS485_WriteDataBuffer[7] = SDA;
                mvars.RS485_WriteDataBuffer[8] = LED;
                mvars.RS485_WriteDataBuffer[9] = TST_PGM;
                mvars.RS485_WriteDataBuffer[10] = SW;
                mvars.RS485_WriteDataBuffer[11] = BINT;
                mvars.RS485_WriteDataBuffer[12] = OUTCTRL;
                mvars.RS485_WriteDataBuffer[13] = A0;
                mvars.RS485_WriteDataBuffer[14] = A1;
                mNumOfByteWrite = 16;
                mvars.NumBytesToRead = 1;
                if (USB_CmdSend_NoRead())
                {
                    stat = Stat.SUCCESS;
                }
                else
                {
                    stat = Stat.ER2_DEVID_READ_FAIL;
                    mp.funSaveLogs(stat + ",Signal set fail of Download board,Set signal fail");
                }
                return stat;
            }
            catch
            {
                stat = Stat.ER3_EXCEP_INFUNC;
                //ExceptionHandler(stat, ex);
                return stat;
            }
        }
        public static Stat Set_dely_DB(int delay, int delay1)
        {
            Stat stat;// = Stat.SUCCESS;
            try
            {
                //Array.Clear(mReadDataBuffer, 0, mReadDataBuffer.Length);
                //Array.Clear(mReadDataBuffer, 0, mWriteDataBuffer.Length);
                Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
                //Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                mvars.RS485_WriteDataBuffer[0] = 7;
                mvars.RS485_WriteDataBuffer[1] = (byte)((delay / 256) & 0xFF);
                mvars.RS485_WriteDataBuffer[2] = (byte)(delay & 0xFF);
                mvars.RS485_WriteDataBuffer[3] = (byte)((delay1 / 256) & 0xFF);
                mvars.RS485_WriteDataBuffer[4] = (byte)(delay1 & 0xFF);
                mNumOfByteWrite = 16;
                mvars.NumBytesToRead = 1;
                if (USB_CmdSend_NoRead())
                {
                    stat = Stat.SUCCESS;
                }
                else
                {
                    stat = Stat.ER2_DEVID_READ_FAIL;
                    mp.funSaveLogs(stat + ",Signal set delay fail of Download board,Set signal delay fail");
                }
                return stat;
            }
            catch
            {
                stat = Stat.ER3_EXCEP_INFUNC;
                //ExceptionHandler(stat, ex);
                return stat;
            }
        }

        public static bool USB_CmdSend_NoRead()
        {
            bool flag;
            try
            {
                //mWriteStatus = SIUSBXP.SI_Write(vUSBHandle, mWriteDataBuffer, mNumOfByteWrite, ref mNumOfByteReturn, mTmp);
                SLUSBXpressDLL.Status = SLUSBXpressDLL.SI_Write(SLUSBXpressDLL.hUSBDevice, ref mvars.RS485_WriteDataBuffer[0], mNumOfByteWrite, ref mNumOfByteReturn, 0);
                if (SLUSBXpressDLL.Status == 0)
                {
                    if (mNumOfByteReturn == mNumOfByteWrite) { return true; }
                    mp.funSaveLogs(Stat.ER2_SIWRITE_RTN_NOTMATCH + ",SI_write return mis-match,Req=" + mvars.RS485_WriteDataBuffer.Length + " ; Rtn=" + mNumOfByteReturn.ToString());
                    return false;
                }
                mp.funSaveLogs(Stat.ER2_SIWRITE_FAIL + ",SI_write fail,Write_Status=" + SLUSBXpressDLL.Status);
                return false;
            }
            catch //(Exception ex)
            {
                flag = false;
                //ExceptionHandler(Stat.ER3_EXCEP_INFUNC, ex);
                return flag;
            }
        }
        public static Stat Set_Signal_DB(byte VIN, byte TST_PGM, byte ROMSEL, byte AGING, byte RESERVE, byte SCL, byte SDA, byte LED)
        {
            Stat stat;// = Stat.SUCCESS;
            try
            {
                //Array.Clear(mReadDataBuffer, 0, mReadDataBuffer.Length);
                //Array.Clear(mReadDataBuffer, 0, mWriteDataBuffer.Length);
                Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
                Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                mvars.RS485_WriteDataBuffer[0] = 14;
                mvars.RS485_WriteDataBuffer[1] = VIN;
                mvars.RS485_WriteDataBuffer[2] = TST_PGM;
                mvars.RS485_WriteDataBuffer[3] = AGING;
                mvars.RS485_WriteDataBuffer[4] = ROMSEL;
                mvars.RS485_WriteDataBuffer[5] = RESERVE;
                mvars.RS485_WriteDataBuffer[6] = SCL;
                mvars.RS485_WriteDataBuffer[7] = SDA;
                mvars.RS485_WriteDataBuffer[8] = LED;
                mNumOfByteWrite = 16;
                mvars.NumBytesToRead = 1;
                if (USB_CmdSend_NoRead())
                {
                    stat = Stat.SUCCESS;
                }
                else
                {
                    stat = Stat.ER2_DEVID_READ_FAIL;
                    mp.funSaveLogs(stat + ",Signal set fail of Download board,Set signal fail");
                }
                return stat;
            }
            catch //(Exception ex)
            {
                stat = Stat.ER3_EXCEP_INFUNC;
                //ExceptionHandler(stat, ex);
                return stat;
            }
        }



        #endregion

        #region AutoDeleteMessageBox
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public const int WM_CLOSE = 0x10;
        public static string killMSGname = "";
        public static byte killMSGsec = 0;
        public static System.Windows.Forms.Timer timer = null;
        public static void KillMessageBoxStart()
        {
            //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = killMSGsec * 1000;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }
        public static void Timer_Tick(object sender, EventArgs e)
        {
            KillMessageBox();
            //停止Timer
            //((System.Windows.Forms.Timer)sender).Stop();
            timer.Stop();
        }
        public static void KillMessageBox()
        {
            IntPtr ptr = FindWindow(null, killMSGname);
            if (ptr != IntPtr.Zero) PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        #endregion


        public static byte CRC_Gamma(int gammavalue)    //cm603 in602c
        {
            byte[] array = new byte[4];
            byte[] array2 = new byte[10];
            for (int i = 0; i <= 9; i++)
            {
                if ((gammavalue & (int)Math.Pow(2.0, i)) != 0)
                {
                    array2[i] = 1;
                }
                else
                {
                    array2[i] = 0;
                }
            }
            array[3] = (byte)((array2[9] + array2[9] + array2[9] + array2[8] + array2[7] + array2[6] + array2[6] + array2[5] + array2[3] + array2[2]) % 2);
            array[2] = (byte)((array2[9] + array2[9] + array2[9] + array2[8] + array2[8] + array2[8] + array2[7] + array2[6] + array2[5] + array2[5] + array2[4] + array2[2] + array2[1]) % 2);
            array[1] = (byte)((array2[9] + array2[9] + array2[8] + array2[8] + array2[8] + array2[7] + array2[7] + array2[7] + array2[6] + array2[5] + array2[4] + array2[4] + array2[3] + array2[1] + array2[0]) % 2);
            array[0] = (byte)((array2[9] + array2[8] + array2[7] + array2[7] + array2[6] + array2[4] + array2[3] + array2[0]) % 2);
            return (byte)(array[3] * 8 + array[2] * 4 + array[1] * 2 + array[0]);
        }

        public static void WriteBinaryFile(string path, List<byte> Wcontent)
        {
            int num = Wcontent.Count();
            if (num > 0)
            {
                byte[] buffer = Wcontent.ToArray();
                FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                fileStream.Write(buffer, 0, num);
                fileStream.Close();
            }
        }

        public static void Waitms(int Svms)
        {
            timeBeginPeriod(1);
            uint start = timeGetTime();
            do
            {
                if ((timeGetTime() - start) >= Svms) { break; }
                Application.DoEvents();
            } while ((timeGetTime() - start) < Svms);
            timeEndPeriod(1);
        }

        public static void cUIREGONOFF(bool svOn, byte svdata)
        {
            DateTime t1 = DateTime.Now;

            //if (mvars.svnova == false && mvars.UUT.demo == false && mvars.sp1.IsOpen == false) mvars.sp1.Open();
            string strData;

            if (svOn == true) mvars.lblCompose += "_ON"; else mvars.lblCompose += "_OFF";
            mvars.strReceive = "";
            mvars.lCounts = 99; //Form1.lbl_target.Text = mvars.lCounts.ToString();
            mvars.lCount = 0; //Form1.lbl_counter.Text = "0";
            Array.Resize(ref mvars.lCmd, mvars.lCounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, mvars.lCounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.lCount = 0; //Form1.lbl_counter.Text = "0";
            mvars.errCode = "0";
            mvars.flgDelFB = true;
            mvars.Break = false;

            //bool svnvBoardcast = mvars.nvBoardcast;
            mvars.nvBoardcast = true;



            //Read
            Form1.pvindex = mvars.FPGA_OM_RW; strData = "1";
            mvars.lblCmd = "FPGA_SPI_W";
            mp.mhFPGASPIWRITE(Convert.ToInt16(strData));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
            //Addr
            Form1.pvindex = mvars.FPGA_OM_ADDR; strData = "0";
            mvars.lblCmd = "FPGA_SPI_W";
            mp.mhFPGASPIWRITE(Convert.ToInt16(strData));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }
            //WData
            Form1.pvindex = mvars.FPGA_OM_Wdata; strData = (svdata).ToString();   //1013 +10 --> +11
            mvars.lblCmd = "FPGA_SPI_W";
            mp.mhFPGASPIWRITE(Convert.ToInt16(strData));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
            //Write
            Form1.pvindex = mvars.FPGA_OM_RW; strData = "0";
            mvars.lblCmd = "FPGA_SPI_W";
            mp.mhFPGASPIWRITE(Convert.ToInt16(strData));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-4"; }
            //Read
            Form1.pvindex = mvars.FPGA_OM_RW; strData = "1";
            mvars.lblCmd = "FPGA_SPI_W";
            mp.mhFPGASPIWRITE(Convert.ToInt16(strData));
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-5"; }

            // ExNovaAGMA:
            //if (mvars.svnova == false && mvars.UUT.demo == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();

            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3," + (DateTime.Now - t1).TotalSeconds.ToString("0") + ",0,0," + (30 + Convert.ToInt16((DateTime.Now - t1).TotalSeconds.ToString("0")));
                //下達寫入指令後再寫到MCU的FLASH
                //if (mvars.lstmcuW62000.Items.Count == 0)
                //{
                //    string[] svss = Form1.nvsender[mvars.iSender].hwR62K[mvars.iPort, mvars.iScan].Split('~');
                //    mvars.lstmcuW62000.Items.AddRange(svss);
                //}
                //if (mvars.lstmcuW62000.Items.Count > 0)
                //{
                //    mvars.lstmcuW62000.Items.RemoveAt(0);
                //    mvars.lstmcuW62000.Items.Insert(0, "0," + svdata.ToString());    //1013 +10 --> +11
                //}
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - t1).TotalSeconds.ToString("0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - t1).TotalSeconds.ToString("0"))); }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static void mhUSERGMA(int svuiuser0, int svuiuser1, int svuiuser2, int svuiuser3, int svuiuser4)     //0x4A
        {
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x13);
            }
            else
            {
                svns = 1;
                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x4A;           //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x013;          //Size

            mvars.RS485_WriteDataBuffer[5 + svns] = Convert.ToByte(svuiuser0 / 256); //Data
            mvars.RS485_WriteDataBuffer[6 + svns] = Convert.ToByte(svuiuser0 % 256); //Data
            mvars.RS485_WriteDataBuffer[7 + svns] = Convert.ToByte(svuiuser1 / 256); //Data
            mvars.RS485_WriteDataBuffer[8 + svns] = Convert.ToByte(svuiuser1 % 256); //Data
            mvars.RS485_WriteDataBuffer[9 + svns] = Convert.ToByte(svuiuser2 / 256); //Data
            mvars.RS485_WriteDataBuffer[10 + svns] = Convert.ToByte(svuiuser2 % 256); //Data
            mvars.RS485_WriteDataBuffer[11 + svns] = Convert.ToByte(svuiuser3 / 256); //Data
            mvars.RS485_WriteDataBuffer[12 + svns] = Convert.ToByte(svuiuser3 % 256); //Data
            mvars.RS485_WriteDataBuffer[13 + svns] = Convert.ToByte(svuiuser4 / 256); //Data
            mvars.RS485_WriteDataBuffer[14 + svns] = Convert.ToByte(svuiuser4 % 256); //Data
            mp.funSendMessageTo();
        }
        public static void mhUSERBRIG(int svuiuser0, int svuiuser5, int svuiuser6, int svuiuser7)    //0x4A
        {
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x11);
            }
            else
            {
                svns = 1;
                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x4B;           //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x011;          //Size      ex.0x11=[0]~[16]

            mvars.RS485_WriteDataBuffer[5 + svns] = Convert.ToByte(svuiuser0 / 256); //Data
            mvars.RS485_WriteDataBuffer[6 + svns] = Convert.ToByte(svuiuser0 % 256); //Data
            mvars.RS485_WriteDataBuffer[7 + svns] = Convert.ToByte(svuiuser5 / 256); //Data
            mvars.RS485_WriteDataBuffer[8 + svns] = Convert.ToByte(svuiuser5 % 256); //Data
            mvars.RS485_WriteDataBuffer[9 + svns] = Convert.ToByte(svuiuser6 / 256); //Data
            mvars.RS485_WriteDataBuffer[10 + svns] = Convert.ToByte(svuiuser6 % 256); //Data
            mvars.RS485_WriteDataBuffer[11 + svns] = Convert.ToByte(svuiuser7 / 256); //Data
            mvars.RS485_WriteDataBuffer[12 + svns] = Convert.ToByte(svuiuser7 % 256); //Data

            //mvars.RS485_WriteDataBuffer[5 + svns] = Convert.ToByte(Convert.ToInt16(uc_brightnessAdj.svuiuser[0]) / 256); //Data
            //mvars.RS485_WriteDataBuffer[6 + svns] = Convert.ToByte(Convert.ToInt16(uc_brightnessAdj.svuiuser[0]) % 256); //Data
            //mvars.RS485_WriteDataBuffer[7 + svns] = Convert.ToByte(Convert.ToInt16(uc_brightnessAdj.svuiuser[5]) / 256); //Data
            //mvars.RS485_WriteDataBuffer[8 + svns] = Convert.ToByte(Convert.ToInt16(uc_brightnessAdj.svuiuser[5]) % 256); //Data
            //mvars.RS485_WriteDataBuffer[9 + svns] = Convert.ToByte(Convert.ToInt16(uc_brightnessAdj.svuiuser[6]) / 256); //Data
            //mvars.RS485_WriteDataBuffer[10 + svns] = Convert.ToByte(Convert.ToInt16(uc_brightnessAdj.svuiuser[6]) % 256); //Data
            //mvars.RS485_WriteDataBuffer[11 + svns] = Convert.ToByte(Convert.ToInt16(uc_brightnessAdj.svuiuser[7]) / 256); //Data
            //mvars.RS485_WriteDataBuffer[12 + svns] = Convert.ToByte(Convert.ToInt16(uc_brightnessAdj.svuiuser[7]) % 256); //Data

            mp.funSendMessageTo();
        }

        public static void mUIRegAdrW(string[] svuiuser)    //0x4C  最新版定義方法
        {
            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 2; mvars.NumBytesToRead = 28;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x29);
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 65);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x4C;           //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x029;          //Size

            mvars.RS485_WriteDataBuffer[5 + svns] = Convert.ToByte(0 / 256); //Addr
            mvars.RS485_WriteDataBuffer[6 + svns] = Convert.ToByte(0 % 256); //Addr
            mvars.RS485_WriteDataBuffer[7 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[0]) / 256); //Data
            mvars.RS485_WriteDataBuffer[8 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[0]) % 256); //Data

            mvars.RS485_WriteDataBuffer[9 + svns] = Convert.ToByte(1 / 256); //Addr
            mvars.RS485_WriteDataBuffer[10 + svns] = Convert.ToByte(1 % 256); //Addr
            mvars.RS485_WriteDataBuffer[11 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[1]) / 256); //Data
            mvars.RS485_WriteDataBuffer[12 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[1]) % 256); //Data

            mvars.RS485_WriteDataBuffer[13 + svns] = Convert.ToByte(2 / 256); //Addr
            mvars.RS485_WriteDataBuffer[14 + svns] = Convert.ToByte(2 % 256); //Addr
            mvars.RS485_WriteDataBuffer[15 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[2]) / 256); //Data
            mvars.RS485_WriteDataBuffer[16 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[2]) % 256); //Data

            mvars.RS485_WriteDataBuffer[17 + svns] = Convert.ToByte(3 / 256); //Addr
            mvars.RS485_WriteDataBuffer[18 + svns] = Convert.ToByte(3 % 256); //Addr
            mvars.RS485_WriteDataBuffer[19 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[3]) / 256); //Data
            mvars.RS485_WriteDataBuffer[20 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[3]) % 256); //Data

            mvars.RS485_WriteDataBuffer[21 + svns] = Convert.ToByte(4 / 256); //Addr
            mvars.RS485_WriteDataBuffer[22 + svns] = Convert.ToByte(4 % 256); //Addr
            mvars.RS485_WriteDataBuffer[23 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[4]) / 256); //Data
            mvars.RS485_WriteDataBuffer[24 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[4]) % 256); //Data

            mvars.RS485_WriteDataBuffer[25 + svns] = Convert.ToByte(5 / 256); //Addr
            mvars.RS485_WriteDataBuffer[26 + svns] = Convert.ToByte(5 % 256); //Addr
            mvars.RS485_WriteDataBuffer[27 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[5]) / 256); //Data
            mvars.RS485_WriteDataBuffer[28 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[5]) % 256); //Data

            mvars.RS485_WriteDataBuffer[29 + svns] = Convert.ToByte(6 / 256); //Addr
            mvars.RS485_WriteDataBuffer[30 + svns] = Convert.ToByte(6 % 256); //Addr
            mvars.RS485_WriteDataBuffer[31 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[6]) / 256); //Data
            mvars.RS485_WriteDataBuffer[32 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[6]) % 256); //Data

            mvars.RS485_WriteDataBuffer[33 + svns] = Convert.ToByte(7 / 256); //Addr
            mvars.RS485_WriteDataBuffer[34 + svns] = Convert.ToByte(7 % 256); //Addr
            mvars.RS485_WriteDataBuffer[35 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[7]) / 256); //Data
            mvars.RS485_WriteDataBuffer[36 + svns] = Convert.ToByte(Convert.ToInt16(svuiuser[7]) % 256); //Data

            mp.funSendMessageTo();
        }


        //查看 dgv_box_CellPainting 須至 v0028


        /*
        导入以下两个包：
     System.Drawing;
     System.Drawing.Imaging;
建产对象：
     Bitmap bm = new Bitmap("c:/1.bmp");
缩放：
     Bitmap bm1 = new Bitmap(bm,width,height);
格式转换：
     bm.save("c:/1.jpg",ImageFromat.Jpeg);
     bm1.Save("c:/1.gif", ImageFormat.Gif);
剪切一个区域：
     //剪切大小
     int cutwidth;  
     int cutheight;
     Graphics g;
     //以大小为剪切大小，像素格式为32位RGB创建一个位图对像
     Bitmap bm1 = new Bitmap(width,height,PixelFormat.Format32bppRgb)  ;
     //定义一个区域
     Rectangle rg = new Rectangle(0,0,cutwidth,cutheight);
     //要绘制到的位图
     g = Graphics.FromImage(bm1);
     //将bm内rg所指定的区域绘制到bm1
     g.DrawImage(bm,rg)

============================================
C#Bitmap代替另一个Bitmap的某部分
Bitmap bm = new Bitmap(宽度, 高度);// 新建一个 Bitmap 位图
System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm); // 根据新建的 Bitmap 位图，创建画布
g.Clear(System.Drawing.Color.Black);// 使用黑色重置画布
g.DrawImage(源位图, ......); // 绘制“源位图”，后面有若干参数控制大小、坐标等等功能。
==================================================

        /// <summary>
         /// 任意角度旋转
         /// </summary>
         /// <param name="bmp">原始图Bitmap</param>
         /// <param name="angle">旋转角度</param>
         /// <param name="bkColor">背景色</param>
         /// <returns>输出Bitmap</returns>
         public static Bitmap KiRotate(Bitmap bmp, float  angle, Color bkColor)
         {
             int w = bmp.Width + 2;
             int h = bmp.Height + 2;
 
              PixelFormat pf;
 
             if (bkColor == Color.Transparent)
             {
                  pf = PixelFormat.Format32bppArgb;
              }
             else
             {
                  pf = bmp.PixelFormat;
              }
 
              Bitmap tmp = new Bitmap(w, h, pf);
              Graphics g = Graphics.FromImage(tmp);
              g.Clear(bkColor);
              g.DrawImageUnscaled(bmp, 1, 1);
              g.Dispose();
 
              GraphicsPath path = new GraphicsPath();
              path.AddRectangle(new RectangleF(0f, 0f, w, h));
              Matrix mtrx = new Matrix();
              mtrx.Rotate(angle);
              RectangleF rct = path.GetBounds(mtrx);
 
              Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
              g = Graphics.FromImage(dst);
              g.Clear(bkColor);
              g.TranslateTransform(-rct.X, -rct.Y);
              g.RotateTransform(angle);
              g.InterpolationMode = InterpolationMode.HighQualityBilinear;
              g.DrawImageUnscaled(tmp, 0, 0);
              g.Dispose();
 
              tmp.Dispose();
 
             return dst;
          }


        */


        /*

                    #region 6. 設座標與單屏delay (重點 由最後屏到第一屏)
            for (int i = Form1.
        [0].regPoCards-1; i >=0; i--)
            {
                string[] svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                //=========================================================================== deviceID
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + svmk[2].PadLeft(2,'0');
                //===========================================================================
                ListBox lst = lst_mcuW60000;
                lst.Items.Clear();

                lst_get1.Items.Insert(0, "▪▪▪▪➧");
                if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, "ID" + svmk[2] + " Coordinating"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, "ID" + svmk[2] + " 座標設定中"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, "ID" + svmk[2] + " 座标设定中"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, "ID" + svmk[2] + " 座標設定"); }
                #region 0x60000 Read
                //List<string> arrW60K = new List<string>(new string[2048]);
                if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, "  R60K Read▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, "  回讀 R60K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, "  回读 R60K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, "  R60K 読む ▪▪▪"); }
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    mvars.strR60K = "";
                }
                else
                {
                    mvars.lblCmd = "MCU_FLASH_R60000";
                    mp.mhMCUFLASHREAD("60000".PadLeft(8, '0'), 8192);
                }
                lst_get1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                {
                    mvars.errCode = "-6." + svmk[2] + ".1"; goto eX;
                }
                else
                {
                    //List<string> arrW60K = new List<string>(new string[2048]);
                    if (mvars.strR60K.Length > 1)
                    {
                        if (mvars.strR60K.Split('~').Length != 2048) { mvars.errCode = "-6." + svmk[2] + ".11"; goto eX; }
                        else
                        {
                            lst.Items.AddRange(mvars.strR60K.Split('~'));
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 2048; j++) lst.Items.Add("0,0");
                    }
                }
                #endregion 0x60000 Read

                #region EDID
                UInt16 checksum;
                byte[] gBinArr = new byte[256];
                bool svload = false;
                string svname = svmk[10] + "X" + svmk[11];
                lst_get1.Items.Insert(1, "  EDID " + svname + " ▪▪▪");
                foreach (var spath in Directory.GetFiles(strpath))
                {
                    if (spath.IndexOf(svname, 0) != -1) 
                    {
                        byte[] Bin;
                        svload = true;
                        Bin = File.ReadAllBytes(spath);
                        checksum = 0;
                        for (int j = 0; j < Bin.Length; j++)
                        {
                            gBinArr[j] = Bin[j];
                        }
                        checksum += CalChecksumIndex(gBinArr, 0x0000, (uint)(gBinArr.Length - 1));
                        lst_get1.Items.RemoveAt(1);
                        lst_get1.Items.Insert(1, "  checksum 0x" + checksum.ToString("X4"));
                        break; 
                    }
                }
                mp.doDelayms(500);
                lst_get1.Items.RemoveAt(1);
                if (svload)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, " EDID Writing"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, " EDID 寫入"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, " EDID 写入"); }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, " EDID 写入"); }
                    //↓ ConfPara_btn_Click
                    Byte[] arr = new byte[17]; 
                    //Configure Parameter
                    //Enable I2C  
                    mvars.lblCmd = "EnableI2C_Wr";
                    if (mvars.demoMode)
                    {
                        arr[0] = 0xFF; arr[1] = 0xE0; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0xE0)");
                        arr[0] = 0xEE; arr[1] = 0x01; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x01)");
                        arr[0] = 0x5E; arr[1] = 0xC1; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0xC1)");
                        arr[0] = 0x58; arr[1] = 0x00; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x00)");
                        arr[0] = 0x59; arr[1] = 0x50; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x50)");
                        arr[0] = 0x5A; arr[1] = 0x10; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x10)");
                        arr[0] = 0x5A; arr[1] = 0x00; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x00)");
                        arr[0] = 0x58; arr[1] = 0x21; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x21)");
                    }
                    else
                    {
                        arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0xEE; arr[1] = 0x01; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5E; arr[1] = 0xC1; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x58; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x59; arr[1] = 0x50; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x10; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x58; arr[1] = 0x21; mp.LT8668_Bin_Wr(0x86, 2, arr);
                    }
                    lst_get1.Items.RemoveAt(1);
                    mp.doDelayms(500);

                    string OffsetAddrText = "070000";
                    lst_get1.Items.Insert(1, " Addr " + OffsetAddrText);
                    //↓ BlockErase_btn_Click
                    arr = new byte[17];                                                 
                    byte[] flash_addr_arr = mp.StringToByteArray(OffsetAddrText);    
                    //Block Erase
                    mvars.lblCmd = "Block_Erase_Bin_Wr";
                    if (mvars.demoMode)
                    {
                        arr[0] = 0x5A; arr[1] = 0x04;               //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x04)");
                        arr[0] = 0x5A; arr[1] = 0x00;               //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x04)");
                        arr[0] = 0x5B; arr[1] = flash_addr_arr[0];  //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(" + flash_addr_arr[0] + ")");
                        arr[0] = 0x5C; arr[1] = flash_addr_arr[1];  //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(" + flash_addr_arr[1] + ")");
                        arr[0] = 0x5D; arr[1] = flash_addr_arr[2];  //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(" + flash_addr_arr[2] + ")");
                        arr[0] = 0x5A; arr[1] = 0x02;               //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x01)");
                        arr[0] = 0x5A; arr[1] = 0x00;               //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x00)");
                    }
                    else
                    {
                        arr[0] = 0x5A; arr[1] = 0x04; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5B; arr[1] = flash_addr_arr[0]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[23:16] 
                        arr[0] = 0x5C; arr[1] = flash_addr_arr[1]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[15:8] 
                        arr[0] = 0x5D; arr[1] = flash_addr_arr[2]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[7:0] 
                        arr[0] = 0x5A; arr[1] = 0x02; mp.LT8668_Bin_Wr(0x86, 2, arr);               // Secter Erase (4KB)
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                    }
                    lst_get1.Items.RemoveAt(1);
                    mp.doDelayms(500);
                    //if (LT8668_FlashAddr_rbtn.Checked)
                    //{
                    //    arr[0] = 0x5A; arr[1] = 0x04; Bin_Wr(0x86, 2, arr);    
                    //    arr[0] = 0x5A; arr[1] = 0x00; Bin_Wr(0x86, 2, arr);    
                    //    arr[0] = 0x5B; arr[1] = flash_addr_arr[0]; Bin_Wr(0x86, 2, arr);    // flash address[23:16] 
                    //    arr[0] = 0x5C; arr[1] = (byte)(flash_addr_arr[1] + 0x80); Bin_Wr(0x86, 2, arr);    // flash address[15:8] 
                    //    arr[0] = 0x5D; arr[1] = flash_addr_arr[2]; Bin_Wr(0x86, 2, arr);    // flash address[7:0] 
                    //    arr[0] = 0x5A; arr[1] = 0x01; Bin_Wr(0x86, 2, arr);    // half-block erase (32KB) 
                    //    arr[0] = 0x5A; arr[1] = 0x00; Bin_Wr(0x86, 2, arr);    
                    //    Delay_ms(500);                                        
                    //}

                    //↓ Bin_Wr_Loop_btn_Click
                    mvars.lblCmd = "LT8668_Bin_Wr";
                    mp.LT8668_Bin_Wr_Loop(OffsetAddrText,gBinArr);      //070000,256
                    mp.doDelayms(500);
                    //WrDis_btn_Click
                    arr = new byte[17]; 
                    //WRDI
                    mvars.lblCmd = "WrDis_Bin_Wr";
                    if (mvars.demoMode)
                    {
                        arr[0] = 0x5A; arr[1] = 0x08; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x08)");
                        arr[0] = 0x5A; arr[1] = 0x00; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x00)");
                    }
                    else
                    {
                        arr[0] = 0x5A; arr[1] = 0x08; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                    }
                    mp.doDelayms(500);

                    //Scaler
                    arr = arr = new byte[2]; //Byte[] rd_arr = new byte[2];
                    //WRDI
                    mvars.lblCmd = "Scaler_Bin_Wr";
                    if (mvars.demoMode)
                    {
                        if (Form1.nvsender[0].regPoCards == 1)
                        {
                            lst_get1.Items.Insert(1, " Sacle ON");
                            //ON
                            arr[0] = 0xFF; arr[1] = 0xE0; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0xE0)");
                            arr[0] = 0xEE; arr[1] = 0x01; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x01)");

                            arr[0] = 0xFF; arr[1] = 0xE0; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0xE0)");
                            arr[0] = 0xB0; arr[1] = 0x00; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x00)");

                            arr[0] = 0xFF; arr[1] = 0xE0; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0xE0)");
                            arr[0] = 0xEE; arr[1] = 0x00; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x00)");
                        }
                        else
                        {
                            lst_get1.Items.Insert(1, " Sacle OFF");
                            //OFF
                            arr[0] = 0xFF; arr[1] = 0xE0; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0xE0)");
                            arr[0] = 0xEE; arr[1] = 0x01; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x01)");

                            arr[0] = 0xFF; arr[1] = 0xE0; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0xE0)");
                            arr[0] = 0xB0; arr[1] = 0x01; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x01)");

                            arr[0] = 0xFF; arr[1] = 0xE0; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0xE0)");
                            arr[0] = 0xEE; arr[1] = 0x00; //lst_get1.Items.RemoveAt(1); lst_get1.Items.Insert(1, "Bin_Wr(0x00)");
                        }
                    }
                    else
                    {
                        if (Form1.nvsender[0].regPoCards == 1)
                        {
                            lst_get1.Items.Insert(1, " Sacle ON");
                            //ON
                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xEE; arr[1] = 0x01; mp.LT8668_Bin_Wr(0x86, 2, arr);    //

                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xB0; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);    //Scale ON

                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xEE; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                        }
                        else
                        {
                            lst_get1.Items.Insert(1, " Sacle OFF");
                            //OFF
                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xEE; arr[1] = 0x01; mp.LT8668_Bin_Wr(0x86, 2, arr);    //

                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xB0; arr[1] = 0x01; mp.LT8668_Bin_Wr(0x86, 2, arr);    //Scale OFF

                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xEE; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                        }
                    }
                    lst_get1.Items.RemoveAt(1);
                    mp.doDelayms(500);

                    #region ScaleONOFF
                    if (Form1.nvsender[0].regPoCards == 1)
                    {
                        lst_get1.Items.Insert(1, "  Scale ON ▪▪▪");
                        mvars.lblCmd = "LT8668_SCALEON";
                    }
                    else
                    {
                        lst_get1.Items.Insert(1, "  Scale OFF ▪▪▪");
                        mvars.lblCmd = "LT8668_SCALEOFF";
                    }
                    if (mvars.demoMode)
                    {
                        mvars.lCount++;
                        mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                        mp.doDelayms(1000);
                        mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    }
                    else
                    {
                        if (Form1.nvsender[0].regPoCards == 1) mp.LT8668_ScaleONOPFF(0);
                        else mp.LT8668_ScaleONOPFF(1);
                    }
                    lst_get1.Items.RemoveAt(1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                    {
                        mvars.errCode = "-6." + svmk[2] + ".1"; goto eX;
                    }                   
                    #endregion ScaleONOFF
                }
                else
                {
                    { mvars.errCode = "-6." + svmk[2] + ".1"; goto eX; }
                }
                #endregion EDID   

                mvars.lblCmd = "FPGA_SPI_W";
                #region Delay 單屏設置 (Disable)
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                Form1.pvindex = 65;
                lst_get1.Items.Insert(1, "  R" + Form1.pvindex + " " + svmk[16] + " ▪▪▪");
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                }
                else
                {
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, Convert.ToInt32(svmk[16]));
                }
                lst_get1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                else { mvars.errCode = "-6." + svmk[2] + ".2"; goto eX; }
                #endregion Delay 

                #region X 座標(1=0,0=480)(1=960,0=1440) 分左右畫面
                for (int svsel = 1; svsel >= 0; svsel--)
                {
                    //==================== 分左右畫面
                    mvars.FPGAsel = (byte)svsel;
                    //==================== 
                    int svm = Convert.ToInt32(svmk[4]) + Convert.ToInt32(svmk[6]) / 2 * (1 - svsel);    //svsel=0:480,1440  svsel=1:0,960
                    Form1.pvindex = 66;     //X
                    lst_get1.Items.Insert(1, "  R" + Form1.pvindex + "F" + svsel + " " + svm + " ▪▪▪");
                    if (mvars.demoMode)
                    {
                        mvars.lCount++;
                        mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                        mp.doDelayms(1000);
                        mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    }
                    else
                    {
                        mp.mhFPGASPIWRITE(mvars.FPGAsel, svm);
                    }
                    lst_get1.Items.RemoveAt(1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                    else { mvars.errCode = "-6." + svmk[2] + ".3"; goto eX; }
                }
                #endregion X 座標(1=0,0=480)(1=960,0=1440) 分左右畫面

                #region Y 座標 單屏設置
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                Form1.pvindex = 67;     //Y
                lst_get1.Items.Insert(1, "  R" + Form1.pvindex + " " + svmk[5] + " ▪▪▪");
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                }
                else
                {
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, Convert.ToInt32(svmk[5]));
                }
                lst_get1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                else { mvars.errCode = "-6." + svmk[2] + ".4"; goto eX; }
                #endregion Y 座標 單屏設置

                #region 0x60000 Write
                if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, "  R60K Write▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, "  寫入 R60K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, "  写入 R60K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, "  R60K 書く▪▪▪"); }
                if (lst.Items.Count > 0)
                {
                    int j = 0;
                    #region R65 (disabled)
                    for (j = 1023; j >= 0; j--)
                    {
                        if (lst.Items[j].ToString().IndexOf("65,", 0) != -1)
                        {
                            lst.Items.RemoveAt(j);
                            lst.Items.Insert(j, "65," + svmk[16]); break;
                        }
                    }
                    if (j < 0)
                    {
                        lst.Items.RemoveAt(0);
                        lst.Items.Insert(0, "65," + svmk[16]);        //12968，25937，38906
                    }
                    for (j = 2047; j >= 1023; j--)
                    {
                        if (lst.Items[j].ToString().IndexOf("65,", 0) != -1)
                        {
                            lst.Items.RemoveAt(j);
                            lst.Items.Insert(j, "65," + svmk[16]); break;
                        }
                    }
                    if (j < 1023)
                    {
                        lst.Items.RemoveAt(1024);
                        lst.Items.Insert(1024, "65," + svmk[16]);
                    }
                    #endregion R65

                    #region R66
                    for (j = 1023; j >= 0; j--)
                    {
                        if (lst.Items[j].ToString().IndexOf("66,", 0) != -1)
                        {
                            lst.Items.RemoveAt(j);
                            int svm = Convert.ToInt32(svmk[4]) + (1 - (j / 1024)) * Convert.ToInt32(svmk[6]) / 2;   //0:480,1440,2880 右畫面(FPGA A)
                            lst.Items.Insert(j, "66," + svm.ToString()); break;
                        }
                    }
                    if (j < 0)
                    {
                        lst.Items.RemoveAt(1);
                        int svm = Convert.ToInt32(svmk[4]) + (1 - (j / 1024)) * Convert.ToInt32(svmk[6]) / 2;       //0:480,1440,2880 右畫面(FPGA A)
                        lst.Items.Insert(1, "66," + svm.ToString());
                    }
                    for (j = 2047; j >= 1023; j--)
                    {
                        if (lst.Items[j].ToString().IndexOf("66,", 0) != -1)
                        {
                            lst.Items.RemoveAt(j);
                            int svm = Convert.ToInt32(svmk[4]) + (1 - ((j + 1) / 1024)) * Convert.ToInt32(svmk[6]) / 2; //1:0,960,1920 左畫面(FPGA B)
                            lst.Items.Insert(j, "66," + svm.ToString()); break;
                        }
                    }
                    if (j < 1023)
                    {
                        lst.Items.RemoveAt(1024 + 1);
                        int svm = Convert.ToInt32(svmk[4]) + (1 - ((j + 2) / 1024)) * Convert.ToInt32(svmk[6]) / 2;     //1:0,960,1920 左畫面(FPGA B)
                        lst.Items.Insert(1024 + 1, "66," + svm.ToString());
                    }
                    #endregion R66

                    #region R67
                    for (j = 1023; j >= 0; j--)
                    {
                        if (lst.Items[j].ToString().IndexOf("67,", 0) != -1)
                        {
                            lst.Items.RemoveAt(j);
                            lst.Items.Insert(j, "67," + svmk[5]); break;
                        }
                    }
                    if (j < 0)
                    {
                        lst.Items.RemoveAt(2);
                        lst.Items.Insert(2, "67," + svmk[5]);
                    }
                    for (j = 2047; j >= 1023; j--)
                    {
                        if (lst.Items[j].ToString().IndexOf("67,", 0) != -1)
                        {
                            lst.Items.RemoveAt(j);
                            lst.Items.Insert(j, "67," + svmk[5]); break;
                        }
                    }
                    if (j < 1023)
                    {
                        lst.Items.RemoveAt(1024 + 2);
                        lst.Items.Insert(1024 + 2, "67," + svmk[5]);
                    }
                    #endregion R67
                }
                byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                for (int svi = 0; svi < 512; svi++)
                {
                    BinArr[svi * 4 + 0] = (Byte)(Convert.ToInt32(lst.Items[svi].ToString().Split(',')[0]) / 256);
                    BinArr[svi * 4 + 1] = (Byte)(Convert.ToInt32(lst.Items[svi].ToString().Split(',')[0]) % 256);
                    BinArr[svi * 4 + 2] = (Byte)(Convert.ToInt32(lst.Items[svi].ToString().Split(',')[1]) / 256);
                    BinArr[svi * 4 + 3] = (Byte)(Convert.ToInt32(lst.Items[svi].ToString().Split(',')[1]) % 256);
                    BinArr[svi * 4 + 0 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(lst.Items[svi + 1024].ToString().Split(',')[0]) / 256);
                    BinArr[svi * 4 + 1 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(lst.Items[svi + 1024].ToString().Split(',')[0]) % 256);
                    BinArr[svi * 4 + 2 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(lst.Items[svi + 1024].ToString().Split(',')[1]) / 256);
                    BinArr[svi * 4 + 3 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(lst.Items[svi + 1024].ToString().Split(',')[1]) % 256);
                }
                checksum = mp.CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                BinArr[BinArr.Length - 1] = (byte)(checksum % 256);
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                }
                else
                {
                    mvars.lblCmd = "MCU_FLASH_W60000";
                    mp.mhMCUFLASHWRITE("60000", ref BinArr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-6." + svmk[2] + ".5"; goto eX; }
                }
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                }
                else
                {
                    mvars.lblCmd = "MCU_FLASH_R60000";
                    mp.mhMCUFLASHREAD("60000", BinArr);
                }
                lst_get1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-6." + svmk[2] + ".6"; goto eX; }
                else
                {
                    lst_mcuR60000.Items.Clear();
                    lst_mcuR60000.Items.AddRange(mvars.strR60K.Split('~'));
                }
                #endregion 0x60000 Write



                lst_get1.Items.RemoveAt(1);
                if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, "ID" + svmk[2] + " Coordinating ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, "ID" + svmk[2] + " 座標設定 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, "ID" + svmk[2] + " 座標設定 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, "ID" + svmk[2] + " 座標設定 ▪▪▪ 終了"); }
                lst_get1.Items.RemoveAt(0);

               //nx:
                if (mvars.demoMode == false)
                {
                    mvars.lblCmd = "PRIID_OFF";
                    mp.mpIDONOFF(0);
                }               
                mp.doDelayms(500);
            }
            lst_get1.Items.Insert(0, "");
            #endregion 6. 設座標與單屏delay  (重點 由最後屏到第一屏)



        

        */

        #region chipset
        /*
        <chiplist>
              <SPI>
                <KB90XX>
                  <KB9012 id="0" page="128" size="131072" spicmd="KB"/>
                </KB90XX>
	            <ADESTO>
	              <AT25DN256 id="1F4000" page="256" size="32768"/>
	            </ADESTO>
                <AMIC>
                  <A25L05PT id="372020" page="256" size="65536"/>
                  <A25L05PU id="372010" page="256" size="65536"/>
                  <A25L10PT id="372021" page="256" size="131072"/>
                  <A25L10PU id="372011" page="256" size="131072"/>
                  <A25L20PT id="372022" page="256" size="262144"/>
                  <A25L20PU id="372012" page="256" size="262144"/>
                  <A25L40PT id="372023" page="256" size="524288"/>
                  <A25L40PU id="372013" page="256" size="524288"/>
                  <A25L80PT id="372024" page="256" size="1048576"/>
                  <A25L80PU id="372014" page="256" size="1048576"/>
                  <A25L16PT id="372025" page="256" size="2097152"/>
                  <A25L16PU id="372015" page="256" size="2097152"/>
                  <A25L512 id="373010" page="256" size="65536"/>
                  <A25L010 id="373011" page="256" size="131072"/>
                  <A25L020 id="373012" page="256" size="262144"/>
                  <A25L040 id="373013" page="256" size="524288"/>
                  <A25L080 id="373014" page="256" size="1048576"/>
                  <A25L016 id="373015" page="256" size="2097152"/>
                  <A25L032 id="373016" page="256" size="4194304"/>
                  <A25LQ16 id="374015" page="256" size="2097152"/>
                  <A25LQ32A id="374016" page="256" size="4194304"/>
                </AMIC>
                <APLUS>
                  <AF25BC08 page="32" size="1024" spicmd="95"/>
                  <AF25BC16 page="32" size="2048" spicmd="95"/>
                  <AF25BC32 page="32" size="4096" spicmd="95"/>
                  <AF25BC64 page="32" size="8192" spicmd="95"/>
                  <AF25BC128 page="64" size="16384" spicmd="95"/>
                  <AF25BC256 page="64" size="32768" spicmd="95"/>
                </APLUS>
	            <Boya>
	              <BY25D80 id="684014" page="256" size="1048576"/>
	            </Boya>
                <CATALYST_CSI>
                  <CAT25010 page="16" size="128" spicmd="95"/>
                  <CAT25020 page="16" size="256" spicmd="95"/>
                  <CAT25040 page="16" size="512" spicmd="95"/>
                  <CAT25080 page="32" size="1024" spicmd="95"/>
                  <CAT25160 page="32" size="2048" spicmd="95"/>
                  <CAT25320 page="32" size="4096" spicmd="95"/>
                  <CAT25640 page="64" size="8192" spicmd="95"/>
                  <CAT25128 page="64" size="16384" spicmd="95"/>
                  <CAT25256 page="64" size="32768" spicmd="95"/>
                  <CAT25C01 page="16" size="128" spicmd="95"/>
                  <CAT25C02 page="16" size="256" spicmd="95"/>
                  <CAT25C03 page="16" size="256" spicmd="95"/>
                  <CAT25C04 page="16" size="512" spicmd="95"/>
                  <CAT25C05 page="16" size="512" spicmd="95"/>
                  <CAT25C08 page="32" size="1024" spicmd="95"/>
                  <CAT25C09 page="32" size="1024" spicmd="95"/>
                  <CAT25C16 page="32" size="2048" spicmd="95"/>
                  <CAT25C17 page="32" size="2048" spicmd="95"/>
                  <CAT25C32 page="32" size="4096" spicmd="95"/>
                  <CAT25C33 page="32" size="4096" spicmd="95"/>
                  <CAT25C64 page="64" size="8192" spicmd="95"/>
                  <CAT25C65 page="64" size="8192" spicmd="95"/>
                  <CAT25C128 page="64" size="16384" spicmd="95"/>
                  <CAT25C256 page="64" size="32768" spicmd="95"/>
                </CATALYST_CSI>
                <EON>
                  <EN25B05 id="1C2010" page="256" size="65536"/>
                  <EN25B05T id="1C2010" page="256" size="65536"/>
                  <EN25B10 id="1C2011" page="256" size="131072"/>
                  <EN25B10T id="1C2011" page="256" size="131072"/>
                  <EN25B20 id="1C2012" page="256" size="262144"/>
                  <EN25B20T id="1C2012" page="256" size="262144"/>
                  <EN25B40 id="1C2013" page="256" size="524288"/>
                  <EN25B40T id="1C2013" page="256" size="524288"/>
                  <EN25B80 id="1C2014" page="256" size="1048576"/>
                  <EN25B80T id="1C2014" page="256" size="1048576"/>
                  <EN25B16 id="1C2015" page="256" size="2097152"/>
                  <EN25B16T id="1C2015" page="256" size="2097152"/>
                  <EN25B32 id="1C2016" page="256" size="4194304"/>
                  <EN25B32T id="1C2016" page="256" size="4194304"/>
                  <EN25B64 id="1C2017" page="256" size="8388608"/>
                  <EN25B64T id="1C2017" page="256" size="8388608"/>
                  <EN25F05 id="1C3110" otp="240" page="256" size="65536"/>
                  <EN25F10 id="1C3111" otp="496" page="256" size="131072"/>
                  <EN25F20 id="1C3112" otp="1008" page="256" size="262144"/>
                  <EN25F40 id="1C3113" otp="2032" page="256" size="524288"/>
                  <EN25F80 id="1C3114" otp="4080" page="256" size="1048576"/>
                  <EN25F16 id="1C3115" otp="8176" page="256" size="2097152"/>
                  <EN25F32 id="1C3116" otp="16368" page="256" size="4194304"/>
                  <EN25LF05 id="1C3110" page="256" size="65536"/>
                  <EN25LF10 id="1C3111" page="256" size="131072"/>
                  <EN25LF20 id="1C3112" page="256" size="262144"/>
                  <EN25LF40 id="1C3113" page="256" size="524288"/>
                  <EN25P05 id="1C2010" page="256" size="65536"/>
                  <EN25P10 id="1C2011" page="256" size="131072"/>
                  <EN25P20 id="1C2012" page="256" size="262144"/>
                  <EN25P40 id="1C2013" page="256" size="524288"/>
                  <EN25P80 id="1C2014" page="256" size="1048576"/>
                  <EN25P16 id="1C2015" page="256" size="2097152"/>
                  <EN25P32 id="1C2016" page="256" size="4194304"/>
                  <EN25P64 id="1C2017" page="256" size="8388608"/>
                  <EN25Q40 id="1C3013" otp="2032" page="256" size="524288"/>
                  <EN25Q80A id="1C3014" otp="4080" page="256" size="1048576"/>
                  <EN25Q16A id="1C3015" otp="8176" page="256" size="2097152"/>
                  <EN25Q32A id="1C3016" otp="16368" page="256" size="4194304"/>
                  <EN25Q32A id="1C7016" otp="16368" page="256" size="4194304"/>
                  <EN25Q32B id="1C3016" otp="16368" page="256" size="4194304"/>
                  <EN25Q64 id="1C3017" otp="32752" page="256" size="8388608"/>
                  <EN25Q128 id="1C3018" otp="65520" page="256" size="16777216"/>
                  <EN25QH16 id="1C7015" otp="8176" page="256" size="2097152"/>
                  <EN25QH32 id="1C7016" otp="16368" page="256" size="4194304"/>
                  <EN25QH64 id="1C7017" otp="32752" page="256" size="8388608"/>
                  <EN25QH128 id="1C7018" otp="65520" page="256" size="16777216"/>
	              <EN25QH256 id="1C7019" otp="0" page="256" size="33554432"/>
                  <EN25T80 id="1C5114" otp="4080" page="256" size="1048576"/>
                  <EN25T16 id="1C5115" otp="8176" page="256" size="2097152"/>
                  <EN25F64 id="1C3117" otp="32752" page="256" size="8388608"/>
                </EON>
                <PMC>
                  <PM25LD256C id="9D2F" page="256" size="32768"/>
                  <PM25LD512 id="9D20" page="256" size="65536"/>
                  <PM25LD512C id="9D20" page="256" size="65536"/>
                  <PM25LD010 id="9D21" page="256" size="131072"/>
                  <PM25LD010C id="9D21" page="256" size="131072"/>
                  <PM25LD020 id="9D22" page="256" size="262144"/>
                  <PM25LD020C id="9D22" page="256" size="262144"/>
                  <PM25LD040 id="9D7E" page="256" size="524288"/>
                  <PM25LD040C id="9D7E" page="256" size="524288"/>
                  <PM25LV512 id="9D7B" page="256" size="65536"/>
                  <PM25LV512A id="9D7B" page="256" size="65536"/>
                  <PM25LV010 id="9D7C" page="256" size="131072"/>
                  <PM25LV010A id="9D7C" page="256" size="131072"/>
                  <PM25LV020 id="9D7D" page="256" size="262144"/>
                  <PM25LV040 id="9D7E" page="256" size="524288"/>
                  <PM25LV080B id="9D13" page="256" size="1048576"/>
                  <PM25LV016B id="9D14" page="256" size="2097152"/>
                  <PM25WD020 id="9D32" page="256" size="262144"/>
                  <PM25WD040 id="9D33" page="256" size="524288"/>
                </PMC>
	            <PFLASH>
	              <Pm25LV010 id="7F9D7C" page="256" size="131072"/>
	              <Pm25LD010 id="7F9D21" page="256" size="131072"/>
	              <Pm25LV020 id="7F9D22" page="256" size="262144"/>
	              <Pm25W020 id="7F9D7D" page="256" size="262144"/>
	              <Pm25LV040 id="7F9D7E" page="256" size="524288"/>
	            </PFLASH>
	            <PUYA>
                <P25D40H id="856013" page="256" size="524288"/>
              </PUYA>
	            <TERRA>
	              <TS25L512A id="373010" page="256" size="65536"/>
	              <TS25L010A id="373011" page="256" size="131072"/>
	              <TS25L020A id="373012" page="256" size="262144"/>
	              <TS25L16AP id="202015" page="256" size="2097152"/>
	              <TS25L16BP id="202015" page="256" size="2097152"/>
	              <ZP25L16P id="202015" page="256" size="2097152"/>
	              <TS25L16PE id="208015" page="256" size="2097152"/>
	              <TS25L80PE id="208014" page="256" size="1048576"/>
	              <TS25L032A id="373016" page="256" size="4194304"/>
	              <TS25L40P id="202013" page="256" size="524288"/>
	            </TERRA>
	            <Generalplus> 
	              <GPR25L005E id="C22010" page="256" size="65536"/>
	              <GPR25L161B id="C22015" page="256" size="262144"/>
	              <GPR25L020B id="C22012" page="256" size="262144"/>
	              <GPR25L3203F id="C22016" page="256" size="4194304" script="GPR25L3203F_OTP.pas"/>
	            </Generalplus>
                <DEUTRON>
                  <AC25LV512 id="9D7B00" page="256" size="65536"/>
                  <AC25LV010 id="9D7C00" page="256" size="131072"/>
                </DEUTRON>
                <EFST>
                  <EM25LV512 id="9D7B00" page="256" size="65536"/>
                  <EM25LV010 id="9D7C00" page="256" size="131072"/>
                  <F25L004A id="8C2013" page="256" size="524288"/>
                  <F25L008A id="8C2014" page="256" size="1048576"/>
                  <F25L016A id="8C2015" page="256" size="2097152"/>
                  <F25L04UA id="8C8C8C" page="256" size="524288"/>
                  <F25L04P id="8C2013" page="256" size="524288"/>
                  <F25S04P id="8C3013" page="256" size="524288"/>
                  <F25L08P id="8C2014" page="256" size="1048576"/>
                  <F25L16P id="8C2015" page="256" size="2097152"/>
                  <F25L32P id="8C2016" page="256" size="4194304"/>
                  <F25L32Q id="8C4016" page="256" size="4194304"/>
                </EFST>
                <EXCELSEMI>
                  <ES25P10 id="4A2011" page="256" size="131072"/>
                  <ES25P20 id="4A2012" page="256" size="262144"/>
                  <ES25P40 id="4A2013" page="256" size="524288"/>
                  <ES25P80 id="4A2014" page="256" size="1048576"/>
                  <ES25P16 id="4A2015" page="256" size="2097152"/>
                  <ES25P32 id="4A2016" page="256" size="4194304"/>
                  <ES25M40A id="4A3213" page="256" size="524288"/>
                  <ES25M80A id="4A3214" page="256" size="1048576"/>
                  <ES25M16A id="4A3215" page="256" size="2097152"/>
                </EXCELSEMI>
                <FIDELIX>
                  <FM25Q08A id="F83214" page="256" size="1048576"/>
                  <FM25Q16A id="F83215" page="256" size="2097152"/>
                  <FM25Q16B id="F83215" page="256" size="2097152"/>
                  <FM25Q32A id="F83216" page="256" size="4194304"/>
                  <FM25Q64A id="F83217" page="256" size="8388608"/>
                </FIDELIX>
                <GIANTEC>
                  <GT25C01 page="8" size="128" spicmd="95"/>
                  <GT25C02 page="8" size="256" spicmd="95"/>
                  <GT25C04 page="8" size="512" spicmd="95"/>
                  <GT25C08 page="32" size="1024" spicmd="95"/>
                  <GT25C16 page="32" size="2048" spicmd="95"/>
                  <GT25C32 page="32" size="4096" spicmd="95"/>
                  <GT25C32A page="32" size="4096" spicmd="95"/>
                  <GT25C64 page="32" size="8192" spicmd="95"/>
                  <GT25C128 page="64" size="16384" spicmd="95"/>
                  <GT25C128A page="64" size="16384" spicmd="95"/>
                  <GT25C256 page="64" size="32768" spicmd="95"/>
                </GIANTEC>
                <GIGADEVICE>
                  <GD25D40 id="C83013" page="256" size="524288"/>
                  <GD25D80 id="C83014" page="256" size="1048576"/>
                  <GD25F40 id="C82013" page="256" size="524288"/>
                  <GD25F80 id="C82014" page="256" size="1048576"/>
                  <GD25Q512 id="C84010" page="256" size="65536"/>
                  <GD25Q10 id="C84011" page="256" size="131072"/>
                  <GD25Q20 id="C84012" page="256" size="262144"/>
	              <GD25LQ20C_1.8V id="C86012" page="256" size="262144"/>
                  <GD25Q40 id="C84013" page="256" size="524288"/>
                  <GD25Q80 id="C84014" page="256" size="1048576"/>
                  <GD25Q80B id="C84014" page="256" size="1048576"/>
	              <GD25Q80C id="C84014" page="256" size="1048576"/>
                  <GD25Q16 id="C84015" page="256" size="2097152"/>
                  <GD25Q16B id="C84015" page="256" size="2097152"/>
                  <GD25Q32 id="C84016" page="256" size="4194304"/>
                  <GD25Q32B id="C84016" page="256" size="4194304"/>
                  <GD25Q64 id="C84017" page="256" size="8388608"/>
                  <GD25Q64B id="C84017" page="256" size="8388608"/>
	              <GD25B64B id="C84017" page="256" size="8388608"/>
	              <GD25B64C id="C84017" page="256" size="8388608"/>
	              <GD25B64E id="C84017" page="256" size="8388608"/>	  
	              <GD25B128C id="C84018" page="256" size="16777216"/>
                  <GD25Q128B id="C84018" page="256" size="16777216"/>
                  <GD25Q128C id="C84018" page="256" size="16777216"/>
	              <GD25LQ064C_1.8V id="C86017" page="256" size="8388608"/>
	              <GD25LQ128C_1.8V id="C86018" page="256" size="16777216"/>
	              <GD25LQ256C_1.8V id="C86019" page="256" size="33554432"/>
                  <MD25T80 id="C83114" page="256" size="1048576"/>
                  <MD25D20 id="514012" page="256" size="262144"/>
                  <MD25D40 id="514013" page="256" size="524288"/>
                  <MD25D80 id="514014" page="256" size="1048576"/>
                  <MD25D16 id="514015" page="256" size="2097152"/>
                </GIGADEVICE>
                <ICE>
                  <ICE25P05 id="1C2010" page="128" size="65536"/>
                </ICE>
                <ICMIC>
                  <X25020 page="4" size="256" spicmd="95"/>
                  <X25021 page="4" size="256" spicmd="95"/>
                  <X25040 page="4" size="512" spicmd="95"/>
                  <X25041 page="4" size="512" spicmd="95"/>
                  <X25080 page="32" size="1024" spicmd="95"/>
                  <X25128 page="32" size="16384" spicmd="95"/>
                  <X25160 page="32" size="2048" spicmd="95"/>
                  <X25170 page="32" size="2048" spicmd="95"/>
                  <X25320 page="32" size="4096" spicmd="95"/>
                  <X25330 page="32" size="4096" spicmd="95"/>
                  <X25640 page="32" size="8192" spicmd="95"/>
                  <X25642 page="32" size="8192" spicmd="95"/>
                  <X25650 page="32" size="8192" spicmd="95"/>
                </ICMIC>
                <INTEGRAL>
                  <IN25AA020 page="16" size="256" spicmd="95"/>
                  <IN25AA040 page="16" size="512" spicmd="95"/>
                  <IN25AA080 page="16" size="1024" spicmd="95"/>
                  <IN25AA160 page="16" size="2048" spicmd="95"/>
                </INTEGRAL>
                <INTEL>
                  <QB25F016S33B id="898911" page="256" size="2097152"/>
                  <QB25F160S33B id="898911" page="256" size="2097152"/>
                  <QB25F320S33B id="898912" page="256" size="4194304"/>
                  <QB25F640S33B id="898913" page="256" size="8388608"/>
                  <QH25F016S33B id="898911" page="256" size="2097152"/>
                  <QH25F160S33B id="898911" page="256" size="2097152"/>
                  <QH25F320S33B id="898912" page="256" size="4194304"/>
                </INTEL>
                <ISSI>
                  <IS25C01 page="8" size="128" spicmd="95"/>
                  <IS25C02 page="16" size="256" spicmd="95"/>
                  <IS25C04 page="16" size="512" spicmd="95"/>
                  <IS25C08 page="16" size="1024" spicmd="95"/>
                  <IS25C16 page="16" size="2048" spicmd="95"/>
                  <IS25C32 page="32" size="4096" spicmd="95"/>
                  <IS25C32A page="32" size="4096" spicmd="95"/>
                  <IS25C64 page="64" size="8192" spicmd="95"/>
                  <IS25C128 page="64" size="16384" spicmd="95"/>
                  <IS25C256 page="64" size="32768" spicmd="95"/>
                </ISSI>
                <KHIC>
                  <KH25L1005 id="C22011" page="256" size="131072"/>
                  <KH25L1005A id="C22011" page="256" size="131072"/>
                  <KH25L2005 id="C22012" page="256" size="262144"/>
                  <KH25L4005 id="C22013" page="256" size="524288"/>
                  <KH25L4005A id="C22013" page="256" size="524288"/>
                  <KH25L512 id="C22010" page="256" size="65536"/>
                  <KH25L512A id="C22010" page="256" size="65536"/>
                  <KH25L8005 id="C22014" page="256" size="1048576"/>
                  <KH25L8036D id="C22615" page="256" size="1048576"/>
                </KHIC>
                <MACRONIX>
                  <MX25L1005 id="C22011" page="256" size="131072"/>
                  <MX25L1005A id="C22011" page="256" size="131072"/>
                  <MX25L1005C id="C22011" page="256" size="131072"/>
                  <MX25L1006E id="C22011" page="256" size="131072"/>
                  <MX25L1021E id="C22211" page="32" size="131072"/>
                  <MX25L1025C id="C22011" page="256" size="131072"/>
                  <MX25L1026E id="C22011" page="256" size="131072"/>
                  <MX25L12805D id="C22018" page="256" size="16777216"/>
                  <MX25L12835E id="C22018" page="256" size="16777216"/>
                  <MX25L12835F id="C22018" page="256" size="16777216"/>
                  <MX25L12836E id="C22018" page="256" size="16777216"/>
                  <MX25L12839F id="C22018" page="256" size="16777216"/>
                  <MX25L12845E id="C22018" page="256" size="16777216"/>
                  <MX25L12845G id="C22018" page="256" size="16777216"/>
                  <MX25L12845F id="C22018" page="256" size="16777216"/>
                  <MX25L12865E id="C22018" page="256" size="16777216"/>
                  <MX25L12865F id="C22018" page="256" size="16777216"/>
                  <MX25L12873F id="C22018" page="256" size="16777216"/>
                  <MX25L12875F id="C22018" page="256" size="16777216"/>
	              <MX25L25635E id="C22019" page="256" size="33554432"/>
                  <MX25L1605 id="C22015" page="256" size="2097152"/>
                  <MX25L1605A id="C22015" page="256" size="2097152"/>
                  <MX25L1605D id="C22015" page="256" size="2097152"/>
                  <MX25L1606E id="C22015" page="256" size="2097152"/>
                  <MX25L1633E id="C22415" page="256" size="2097152"/>
                  <MX25L1635D id="C22415" page="256" size="2097152"/>
                  <MX25L1635E id="C22515" page="256" size="2097152"/>
                  <MX25L1636D id="C22415" page="256" size="2097152"/>
                  <MX25L1636E id="C22515" page="256" size="2097152"/>
                  <MX25L1673E id="C22415" page="256" size="2097152"/>
                  <MX25L1675E id="C22415" page="256" size="2097152"/>
                  <MX25L2005 id="C22012" page="256" size="262144"/>
                  <MX25L2005C id="C22012" page="256" size="262144"/>
                  <MX25L2006E id="C22012" page="256" size="262144"/>
                  <MX25L2026C id="C22012" page="256" size="262144"/>
                  <MX25L2026E id="C22012" page="256" size="262144"/>
                  <MX25L3205 id="C22016" page="256" size="4194304"/>
                  <MX25L3205A id="C22016" page="256" size="4194304"/>
                  <MX25L3205D id="C22016" page="256" size="4194304"/>
                  <MX25L3206E id="C22016" page="256" size="4194304"/>
                  <MX25L3208E id="C22016" page="256" size="4194304"/>
                  <MX25L3225D id="C25E16" page="256" size="4194304"/>
                  <MX25L3233F id="C22016" page="256" size="4194304"/>
                  <MX25L3235D id="C25E16" page="256" size="4194304"/>
                  <MX25L3235E id="C22016" page="256" size="4194304"/>
                  <MX25L3236D id="C25E16" page="256" size="4194304"/>
                  <MX25L3237D id="C25E16" page="256" size="4194304"/>
                  <MX25L3239E id="C22536" page="256" size="4194304"/>
                  <MX25L3273E id="C22016" page="256" size="4194304"/>
                  <MX25L3273F id="C22016" page="256" size="4194304"/>
                  <MX25L3275E id="C22016" page="256" size="4194304"/>
                  <MX25L4005 id="C22013" page="256" size="524288"/>
                  <MX25L4005A id="C22013" page="256" size="524288"/>
                  <MX25L4005C id="C22013" page="256" size="524288"/>
                  <MX25L4006E id="C22013" page="256" size="524288"/>
                  <MX25L4026E id="C22013" page="256" size="524288"/>
                  <MX25L512 id="C22010" page="256" size="65536"/>
                  <MX25L512A id="C22010" page="256" size="65536"/>
                  <MX25L512C id="C22010" page="256" size="65536"/>
                  <MX25L5121E id="C22210" page="32" size="65536"/>
                  <MX25L6405 id="C22017" page="256" size="8388608"/>
                  <MX25L6405D id="C22017" page="256" size="8388608"/>
                  <MX25L6406E id="C22017" page="256" size="8388608"/>
                  <MX25L6408E id="C22017" page="256" size="8388608"/>
                  <MX25L6433F id="C22017" page="256" size="8388608"/>
                  <MX25L6435E id="C22017" page="256" size="8388608"/>
                  <MX25L6436E id="C22017" page="256" size="8388608"/>
	              <MX25L6436F id="C22017" page="256" size="8388608"/>
                  <MX25L6439E id="C22537" page="256" size="8388608"/>
                  <MX25L6445E id="C22017" page="256" size="8388608"/>
                  <MX25L6465E id="C22017" page="256" size="8388608"/>
                  <MX25L6473E id="C22017" page="256" size="8388608"/>
                  <MX25L6473F id="C22017" page="256" size="8388608"/>
                  <MX25L6475E id="C22017" page="256" size="8388608"/>
                  <MX25L8005 id="C22014" page="256" size="1048576"/>
                  <MX25L8006E id="C22014" page="256" size="1048576"/>
                  <MX25L8008E id="C22014" page="256" size="1048576"/>
                  <MX25L8035E id="C22014" page="256" size="1048576"/>
                  <MX25L8036E id="C22014" page="256" size="1048576"/>
                  <MX25L8073E id="C22014" page="256" size="1048576"/>
                  <MX25L8075E id="C22014" page="256" size="1048576"/>
	              <MX25L25673G id="C22019" page="256" size="33554432"/>
                  <MX25R512F id="C22810" page="256" size="65536"/>
                  <MX25R1035F id="C22811" page="256" size="131072"/>
                  <MX25R1635F id="C22815" page="256" size="2097152"/>
                  <MX25R2035F id="C22812" page="256" size="262144"/>
                  <MX25R3235F id="C22816" page="256" size="4194304"/>
                  <MX25R4035F id="C22813" page="256" size="524288"/>
                  <MX25R6435F id="C22817" page="256" size="8388608"/>
                  <MX25R8035F id="C22814" page="256" size="1048576"/>
                  <MX25U1001E_1.8V id="C22531" page="256" size="131072"/>
                  <MX25U12835F_1.8V id="C22518" page="256" size="16777216"/>
	              <MX25U25673G_1.8V id="C22539" page="256" size="33554432"/>
	              <MX25U25645G_1.8V id="C22539" page="256" size="33554432"/>
                  <MX25U1635E_1.8V id="C22535" page="256" size="2097152"/>
                  <MX25U1635F_1.8V id="C22535" page="256" size="2097152"/>
                  <MX25U2032E_1.8V id="C22532" page="256" size="262144"/>
                  <MX25U2033E_1.8V id="C22532" page="256" size="262144"/>
                  <MX25U3235E_1.8V id="C22536" page="256" size="4194304"/>
                  <MX25U3235F_1.8V id="C22536" page="256" size="4194304"/>
                  <MX25U4032E_1.8V id="C22533" page="256" size="524288"/>
                  <MX25U4033E_1.8V id="C22533" page="256" size="524288"/>
                  <MX25U4035_1.8V id="C22533" page="256" size="524288"/>
                  <MX25U5121E_1.8V id="C22530" page="256" size="65536"/>
                  <MX25U6435F_1.8V id="C22537" page="256" size="8388608"/>
                  <MX25U6473F_1.8V id="C22537" page="256" size="8388608"/>
                  <MX25U8032E_1.8V id="C22534" page="256" size="1048576"/>
                  <MX25U8033E_1.8V id="C22534" page="256" size="1048576"/>
                  <MX25U8035_1.8V id="C22534" page="256" size="1048576"/>
                  <MX25U8035E_1.8V id="C22534" page="256" size="1048576"/>
	              <MX25U12873F_1.8V id="C22538" page="256" size="16777216"/>
                  <MX25V1006E id="C22011" page="256" size="131072"/>
                  <MX25V1035F id="C22311" page="256" size="131072"/>
                  <MX25V2006E id="C22012" page="256" size="262144"/>
                  <MX25V2035F id="C22312" page="256" size="262144"/>
                  <MX25V512 id="C22010" page="256" size="65536"/>
                  <MX25V512C id="C22010" page="256" size="65536"/>
                  <MX25V512E id="C22010" page="256" size="65536"/>
                  <MX25V512F id="C22310" page="256" size="65536"/>
                  <MX25V4005 id="C22013" page="256" size="524288"/>
                  <MX25V4006E id="C22013" page="256" size="524288"/>
                  <MX25V4035 id="C22553" page="256" size="524288"/>
                  <MX25V4035F id="C22313" page="256" size="524288"/>
                  <MX25V8005 id="C22014" page="256" size="1048576"/>
                  <MX25V8006E id="C22014" page="256" size="1048576"/>
                  <MX25V8035 id="C22554" page="256" size="1048576"/>
                  <MX25V8035F id="C22314" page="256" size="1048576"/>
	              <MX66U51235F_1.8V id="C2253A" page="256" size="67108864"/>
	              <MX66U1G45G_1.8V id="C2253B" page="256" size="134217728"/>
	              <MX77L12850F id="C27518" page="256" size="16777216"/>
                </MACRONIX>
                <MICROCHIP>
                  <_25AA010A page="16" size="128" spicmd="95"/>
                  <_25AA020A page="16" size="256" spicmd="95"/>
                  <_25AA040 page="16" size="512" spicmd="95"/>
                  <_25AA040A page="16" size="512" spicmd="95"/>
                  <_25AA080 page="16" size="1024" spicmd="95"/>
                  <_25AA080A page="16" size="1024" spicmd="95"/>
                  <_25AA080B page="32" size="1024" spicmd="95"/>
                  <_25AA080C page="16" size="1024" spicmd="95"/>
                  <_25AA080D page="32" size="1024" spicmd="95"/>
                  <_25AA1024 page="256" size="131072" spicmd="95"/>
                  <_25AA128 page="64" size="16384" spicmd="95"/>
                  <_25AA160 page="16" size="2048" spicmd="95"/>
                  <_25AA160A page="16" size="2048" spicmd="95"/>
                  <_25AA160B page="32" size="2048" spicmd="95"/>
                  <_25AA256 page="64" size="32768" spicmd="95"/>
                  <_25AA320 page="32" size="4096" spicmd="95"/>
                  <_25AA512 page="128" size="65536" spicmd="95"/>
                  <_25AA640 page="32" size="8192" spicmd="95"/>
                  <_25C040 page="16" size="512" spicmd="95"/>
                  <_25C080 page="16" size="1024" spicmd="95"/>
                  <_25C160 page="16" size="2048" spicmd="95"/>
                  <_25C320 page="32" size="4096" spicmd="95"/>
                  <_25C640 page="32" size="8192" spicmd="95"/>
                  <_25LC010A page="16" size="128" spicmd="95"/>
                  <_25LC020A page="16" size="256" spicmd="95"/>
                  <_25LC040 page="16" size="512" spicmd="95"/>
                  <_25LC040A page="16" size="512" spicmd="95"/>
                  <_25LC080 page="16" size="1024" spicmd="95"/>
                  <_25LC080A page="16" size="1024" spicmd="95"/>
                  <_25LC080B page="32" size="1024" spicmd="95"/>
                  <_25LC080C page="16" size="1024" spicmd="95"/>
                  <_25LC080D page="32" size="1024" spicmd="95"/>
                  <_25LC1024 page="256" size="131072" spicmd="95"/>
                  <_25LC128 page="64" size="16384" spicmd="95"/>
                  <_25LC160 page="16" size="2048" spicmd="95"/>
                  <_25LC160A page="16" size="2048" spicmd="95"/>
                  <_25LC160B page="32" size="2048" spicmd="95"/>
                  <_25LC256 page="64" size="32768" spicmd="95"/>
                  <_25LC320 page="32" size="4096" spicmd="95"/>
                  <_25LC512 page="128" size="65536" spicmd="95"/>
                  <_25LC640 page="32" size="8192" spicmd="95"/>
                </MICROCHIP>
                <MICRON>
                  <N25Q032A id="20BA16" page="256" size="4194304"/>
                  <N25Q064A id="20BA17" page="256" size="8388608"/>
	              <N25Q256A13 id="20BA19" page="256" size="33554432"/>
	              <N25Q512A83 id="20BA20" page="256" size="67108864"/>
	              <N25W256A11 id="2CCB19" page="256" size="33554432"/>
	              <MT25QL128AB id="20BA18" page="256" size="16777216"/>
                  <MT25QL256A id="20BA19" page="256" size="33554432"/>
                  <MT25QL512A id="20BA20" page="256" size="67108864"/>
                  <MT25QL02GC id="20BA22" page="256" size="268435456"/>
                  <MT25QU256 id="20BB19" page="256" size="33554432"/>  
	              <N25Q00AA13G id="20BA21" page="256" size="134217728"/>
                </MICRON>
                <MSHINE>
                  <MS25X512 id="373010" page="256" size="65536"/>
                  <MS25X10 id="373011" page="256" size="131072"/>
                  <MS25X20 id="373012" page="256" size="262144"/>
                  <MS25X40 id="373013" page="256" size="524288"/>
                  <MS25X80 id="373014" page="256" size="1048576"/>
                  <MS25X16 id="373015" page="256" size="2097152"/>
                  <MS25X32 id="373016" page="256" size="4194304"/>
                </MSHINE>
                <NANTRONICS>
                  <N25S10 id="D53011" page="256" size="131072"/>
                  <N25S20 id="D53012" page="256" size="262144"/>
                  <N25S40 id="D53013" page="256" size="524288"/>
                  <N25S16 id="D53015" page="256" size="2097152"/>
                  <N25S32 id="D53016" page="256" size="4194304"/>
                  <N25S80 id="D53014" page="256" size="1048576"/>
                </NANTRONICS>
                <NEXFLASH>
                  <NX25P10 id="9D7F7C" page="256" size="131072"/>
                  <NX25P16 id="EF2015" page="256" size="2097152"/>
                  <NX25P20 id="9D7F7D" page="256" size="262144"/>
                  <NX25P32 id="EF2016" page="256" size="4194304"/>
                  <NX25P40 id="9D7F7E" page="256" size="524288"/>
                  <NX25P80 id="9D7F13" page="256" size="1048576"/>
                </NEXFLASH>
                <NUMONYX>
	              <M45PE16 id="204015" page="256" size="2097152" script="blockerase.pas"/>
                  <M25P05 id="202010" page="128" size="65536"/>
                  <M25P05A id="202010" page="256" size="65536"/>
                  <M25P10 id="202011" page="128" size="131072"/>
                  <M25P10A id="202011" page="256" size="131072"/>
                  <M25P20 id="202012" page="256" size="262144"/>
                  <M25P40 id="202013" page="256" size="524288"/>
                  <M25P80 id="202014" page="256" size="1048576"/>
                  <M25P16 id="202015" page="256" size="2097152"/>
                  <M25P32 id="202016" page="256" size="4194304"/>
                  <M25P64 id="202017" page="256" size="8388608"/>
                  <M25P128_ST25P28V6G id="202018" page="256" size="16777216"/>
                  <M25PE10 id="208011" page="256" size="131072"/>
                  <M25PE16 id="208015" page="256" size="2097152"/>
                  <M25PE20 id="208012" page="256" size="262144"/>
                  <M25PE40 id="208013" page="256" size="524288"/>
                  <M25PE80 id="208014" page="256" size="1048576"/>
                </NUMONYX>
                <PCT>
                  <PCT25LF020A id="BF4300" page="256" size="262144"/>
                  <PCT25VF010A id="BF4900" page="256" size="131072"/>
                  <PCT25VF016B id="BF2541" page="256" size="2097152"/>
                  <PCT25VF020A id="BF4300" page="256" size="262144"/>
                  <PCT25VF032B id="BF254A" page="256" size="4194304"/>
                  <PCT25VF040A id="BF4400" page="256" size="524288"/>
                  <PCT25VF040B id="BF258D" page="256" size="524288"/>
                  <PCT25VF080B id="BF258E" page="256" size="1048576"/>
                </PCT>
                <RAMTRON>
                  <FM25040 page="32" size="512" spicmd="95"/>
                  <FM25C160 page="32" size="2048" spicmd="95"/>
                  <FM25640 page="32" size="8192" spicmd="95"/>
                  <FM25CL04 page="32" size="512" spicmd="95"/>
                  <FM25CL64 page="32" size="8192" spicmd="95"/>
                  <FM25L16 page="32" size="2048" spicmd="95"/>
                  <FM25L256 page="32" size="32768" spicmd="95"/>
                  <FM25L512 page="32" size="65536" spicmd="95"/>
                  <FM25W256 page="32" size="32768" spicmd="95"/>
                </RAMTRON>
                <RENESAS>
                  <HN58X2502 page="16" size="256" spicmd="95"/>
                  <HN58X2504 page="16" size="512" spicmd="95"/>
                  <HN58X2508 page="16" size="1024" spicmd="95"/>
                  <HN58X25128 page="64" size="16384" spicmd="95"/>
                  <HN58X2516 page="16" size="2048" spicmd="95"/>
                  <HN58X25256 page="64" size="32768" spicmd="95"/>
                  <HN58X2532 page="32" size="4096" spicmd="95"/>
                  <HN58X2564 page="32" size="8192" spicmd="95"/>
                  <R1EX25002A page="16" size="256" spicmd="95"/>
                  <R1EX25004A page="16" size="512" spicmd="95"/>
                  <R1EX25008A page="16" size="1024" spicmd="95"/>
                  <R1EX25016A page="16" size="2048" spicmd="95"/>
                  <R1EX25032A page="32" size="4096" spicmd="95"/>
                  <R1EX25064A page="32" size="8192" spicmd="95"/>
                </RENESAS>
                <ROHM>
                  <BR25010 page="16" size="128" spicmd="95"/>
                  <BR25020 page="16" size="256" spicmd="95"/>
                  <BR25040 page="16" size="512" spicmd="95"/>
                  <BR25080 page="16" size="1024" spicmd="95"/>
                  <BR25160 page="16" size="2048" spicmd="95"/>
                  <BR25320 page="32" size="4096" spicmd="95"/>
                  <BR25H010 page="16" size="128" spicmd="95"/>
                  <BR25H020 page="16" size="256" spicmd="95"/>
                  <BR25H040 page="16" size="512" spicmd="95"/>
                  <BR25H080 page="16" size="1024" spicmd="95"/>
                  <BR25H160 page="16" size="2048" spicmd="95"/>
                  <BR25H320 page="32" size="4096" spicmd="95"/>
                  <BR25L010 page="16" size="128" spicmd="95"/>
                  <BR25L010 page="16" size="128" spicmd="95"/>
                  <BR25L020 page="16" size="256" spicmd="95"/>
                  <BR25L040 page="16" size="512" spicmd="95"/>
                  <BR25L080 page="16" size="1024" spicmd="95"/>
                  <BR25L160 page="16" size="2048" spicmd="95"/>
                  <BR25L320 page="32" size="4096" spicmd="95"/>
                  <BR25L640 page="32" size="8192" spicmd="95"/>
                  <BR25S320 page="32" size="4096" spicmd="95"/>
                  <BR25S640 page="32" size="8192" spicmd="95"/>
                  <BR25S128 page="64" size="16384" spicmd="95"/>
                  <BR25S256 page="64" size="32768" spicmd="95"/>
                  <BR95010 page="8" size="128" spicmd="95"/>
                  <BR95020 page="8" size="256" spicmd="95"/>
                  <BR95040 page="8" size="512" spicmd="95"/>
                  <BR95080 page="32" size="1024" spicmd="95"/>
                  <BR95160 page="32" size="2048" spicmd="95"/>
                </ROHM>
                <SAIFUN>
                  <SA25C1024H page="128" size="131072" spicmd="95"/>
                  <SA25C1024L page="128" size="131072" spicmd="95"/>
                  <SA25C512H page="128" size="65536" spicmd="95"/>
                  <SA25C512L page="128" size="65536" spicmd="95"/>
                </SAIFUN>
                <SANYO>
                  <LE25FU106BMA id="621D" page="256" size="131072"/>
                  <LE25FU206MA id="6244" page="256" size="262144"/>
                  <LE25FU406BMA id="621E" page="256" size="524288"/>
                  <LE25FW206M id="6226" page="256" size="262144"/>
                  <LE25FW406M id="6207" page="256" size="524288"/>
                  <LE25FW406AM id="621A" page="256" size="524288"/>
                  <LE25FW806M id="6226" page="256" size="1048576"/>
                </SANYO>
                <SEIKO>
                  <S-25A010A page="8" size="128" spicmd="95"/>
                  <S-25A020A page="8" size="256" spicmd="95"/>
                  <S-25A040A page="8" size="512" spicmd="95"/>
                  <S-25A080A page="32" size="1024" spicmd="95"/>
                  <S-25A160A page="32" size="2048" spicmd="95"/>
                  <S-25A320A page="32" size="4096" spicmd="95"/>
                  <S-25A640A page="32" size="8192" spicmd="95"/>
                  <S-25C010A page="8" size="128" spicmd="95"/>
                  <S-25C020A page="8" size="256" spicmd="95"/>
                  <S-25C040A page="8" size="512" spicmd="95"/>
                  <S-25C080A page="32" size="1024" spicmd="95"/>
                  <S-25C160A page="32" size="2048" spicmd="95"/>
                  <S-25C320A page="32" size="4096" spicmd="95"/>
                  <S-25C640A page="32" size="8192" spicmd="95"/>
                </SEIKO>
                <SIEMENS>
                  <SLA25010 page="16" size="128" spicmd="95"/>
                  <SLA25020 page="16" size="256" spicmd="95"/>
                  <SLA25040 page="16" size="512" spicmd="95"/>
                  <SLA25080 page="32" size="1024" spicmd="95"/>
                  <SLA25160 page="32" size="2048" spicmd="95"/>
                  <SLA25320 page="32" size="4096" spicmd="95"/>
                  <SLE25010 page="16" size="128" spicmd="95"/>
                  <SLE25020 page="16" size="256" spicmd="95"/>
                  <SLE25040 page="16" size="512" spicmd="95"/>
                  <SLE25080 page="32" size="1024" spicmd="95"/>
                  <SLE25160 page="32" size="2048" spicmd="95"/>
                  <SLE25320 page="32" size="4096" spicmd="95"/>
                </SIEMENS>
                <SPANSION>
                  <S25FL001D id="010210" page="256" size="131072"/>
                  <S25FL002D id="010211" page="256" size="262144"/>
                  <S25FL004A id="010212" page="256" size="524288"/>
                  <S25FL004D id="010212" page="256" size="524288"/>
                  <S25FL004K id="EF4013" page="256" size="524288"/>
                  <S25FL008A id="010213" page="256" size="1048576"/>
                  <S25FL008D id="010213" page="256" size="1048576"/>
                  <S25FL008K id="EF4014" page="256" size="1048576"/>
                  <S25FL016A id="010214" page="256" size="2097152"/>
                  <S25FL016K id="EF4015" page="256" size="2097152"/>
                  <S25FL032A id="010215" page="256" size="4194304"/>
                  <S25FL032K id="EF4016" page="256" size="4194304"/>
                  <S25FL032P id="010215" page="256" size="4194304"/>
                  <S25FL040A id="010212" page="256" size="524288"/>
                  <S25FL040A_BOT id="010226" page="256" size="524288"/>
                  <S25FL040A_TOP id="010225" page="256" size="524288"/>
                  <S25FL064A id="010216" page="256" size="8388608"/>
                  <S25FL064K id="EF4017" page="256" size="8388608"/>
                  <S25FL064P id="010216" page="256" size="8388608"/>
                  <S25FL116K id="014015" page="256" size="2097152"/>
                  <S25FL128K id="EF4018" page="256" size="16777216"/>
                  <S25FL128P id="012018" page="256" size="16777216"/>
                  <S25FL128S id="012018" page="256" size="16777216"/>
                  <S25FL132K id="014016" page="256" size="4194304"/>
                  <S25FL164K id="014017" page="256" size="8388608"/>
	              <S25FL256S id="010219" page="256" size="33554432"/>
                </SPANSION>
                <SST>
                  <SST25LF020A id="BF43" page="SSTB" size="262144"/>
                  <SST25LF040A id="BF44" page="SSTB" size="524288"/>
                  <SST25LF080A id="BF80" page="SSTB" size="1048576"/>
                  <SST25VF010 id="BF49" page="SSTB" size="131072"/>
                  <SST25VF010A id="BF49" page="SSTB" size="131072"/>
                  <SST25VF016B id="BF2541" page="SSTW" size="2097152"/>
                  <SST25VF020 id="BF43" page="SSTB" size="262144"/>
                  <SST25VF020A id="BF43" page="SSTB" size="262144"/>
                  <SST25VF020B id="BF258C" page="SSTW" size="262144"/>
                  <SST25VF032B id="BF254A" page="SSTW" size="4194304"/>
                  <SST25VF064C id="BF254B" page="256" size="8388608"/>
                  <SST25VF040 id="BF44" page="SSTB" size="524288"/>
                  <SST25VF040A id="BF44" page="SSTB" size="524288"/>
                  <SST25VF040B id="BF258D" page="SSTW" size="524288"/>
                  <SST25VF080B id="BF258E" page="SSTW" size="1048576"/>
                  <SST25VF512 id="BF48" page="SSTB" size="65536"/>
                  <SST25VF512A id="BF48" page="SSTB" size="65536"/>
                </SST>
                <ST>
                  <M25C16 page="16" size="2048" spicmd="95"/>
                  <M25PX16 id="207115" page="256" size="2097152"/>
                  <M25PX32 id="207116" page="256" size="4194304"/>
                  <M25PX64 id="207117" page="256" size="8388608"/>
                  <M25PX80 id="207114" page="256" size="1048576"/>
                  <M25W16 page="16" size="2048" spicmd="95"/>
                  <M35080-3 page="32" size="1024" spicmd="95"/>
                  <M35080-6 page="32" size="1024" spicmd="95"/>
                  <M35080V6 page="32" size="1024" spicmd="95"/>
                  <M35080VP page="32" size="1024" spicmd="95"/>
                  <M95010 page="8" size="128" spicmd="95"/>
                  <M95010R page="8" size="128" spicmd="95"/>
                  <M95010W page="8" size="128" spicmd="95"/>
                  <M95020 page="8" size="256" spicmd="95"/>
                  <M95020R page="8" size="256" spicmd="95"/>
                  <M95020W page="8" size="256" spicmd="95"/>
                  <M95040 page="8" size="512" spicmd="95"/>
                  <M95040R page="8" size="512" spicmd="95"/>
                  <M95040W page="8" size="512" spicmd="95"/>
                  <M95080 page="32" size="1024" spicmd="95"/>
                  <M95080R page="32" size="1024" spicmd="95"/>
                  <M95080W page="32" size="1024" spicmd="95"/>
                  <M95128 page="64" size="16384" spicmd="95"/>
                  <M95128R page="64" size="16384" spicmd="95"/>
                  <M95128W page="64" size="16384" spicmd="95"/>
                  <M95160 page="32" size="2048" spicmd="95"/>
                  <M95160R page="32" size="2048" spicmd="95"/>
                  <M95160W page="32" size="2048" spicmd="95"/>
                  <M95256 page="64" size="32768" spicmd="95"/>
                  <M95256R page="64" size="32768" spicmd="95"/>
                  <M95256W page="64" size="32768" spicmd="95"/>
                  <M95320 page="32" size="4096" spicmd="95"/>
                  <M95320R page="32" size="4096" spicmd="95"/>
                  <M95320W page="32" size="4096" spicmd="95"/>
                  <M95512R page="128" size="65536" spicmd="95"/>
                  <M95512W page="128" size="65536" spicmd="95"/>
                  <M95640 page="32" size="8192" spicmd="95"/>
                  <M95640R page="32" size="8192" spicmd="95"/>
                  <M95640W page="32" size="8192" spicmd="95"/>
                  <M95M01R page="256" size="131072" spicmd="95"/>
                  <M95M01W page="256" size="131072" spicmd="95"/>
                  <ST25C16 page="16" size="2048" spicmd="95"/>
                  <ST25P05 id="202010" page="128" size="65536"/>
                  <ST25P05A id="202010" page="256" size="65536"/>
                  <ST25P10 id="202011" page="128" size="131072"/>
                  <ST25P10A id="202011" page="256" size="131072"/>
                  <ST25P16 id="202015" page="256" size="2097152"/>
                  <ST25P20 id="202012" page="256" size="262144"/>
                  <ST25P32 id="202016" page="256" size="4194304"/>
                  <ST25P40 id="202013" page="256" size="524288"/>
                  <ST25P64 id="202017" page="256" size="8388608"/>
                  <ST25P80 id="202014" page="256" size="1048576"/>
                </ST>
                <WINBOND>
                  <W25P10 id="EF1000" page="256" size="131072"/>
                  <W25P16 id="EF2015" page="256" size="2097152"/>
                  <W25P20 id="EF1100" page="256" size="262144"/>
                  <W25P32 id="EF2016" page="256" size="4194304"/>
                  <W25P40 id="EF1200" page="256" size="524288"/>
                  <W25P64 id="EF2017" page="256" size="8388608"/>
                  <W25P80 id="EF2014" page="256" size="1048576"/>
                  <W25Q10EW_1.8V id="EF6011" page="256" size="131072"/>
                  <W25Q128BV id="EF4018" page="256" size="16777216"/>
                  <W25Q128FV id="EF4018" page="256" size="16777216"/>
	              <W25Q128JV id="EF7018" page="256" size="16777216"/>
	              <W25Q256FV id="EF4019" page="256" size="33554432"/>
	              <W25Q256JV id="EF4019" page="256" size="33554432"/>
	              <W25Q256JV id="EF7019" page="256" size="33554432"/>
                  <W25Q128FW_1.8V id="EF6018" page="256" size="16777216"/>
                  <W25Q16 id="EF4015" page="256" size="2097152"/>
                  <W25Q16BV id="EF4015" page="256" size="2097152"/>
                  <W25Q16CL id="EF4015" page="256" size="2097152"/>
                  <W25Q16CV id="EF4015" page="256" size="2097152"/>
                  <W25Q16DV id="EF4015" page="256" size="2097152"/>
                  <W25Q16FW_1.8V id="EF6015" page="256" size="2097152"/>
                  <W25Q16V id="EF4015" page="256" size="2097152"/>
                  <W25Q20CL id="EF4012" page="256" size="262144"/>
                  <W25Q20EW_1.8V id="EF6012" page="256" size="262144"/>
                  <W25Q32 id="EF4016" page="256" size="4194304"/>
                  <W25Q32BV id="EF4016" page="256" size="4194304"/>
                  <W25Q32FV id="EF4016" page="256" size="4194304"/>
                  <W25Q32FW_1.8V id="EF6016" page="256" size="4194304"/>
                  <W25Q32V id="EF4016" page="256" size="4194304"/>
                  <W25Q40BL id="EF4013" page="256" size="524288"/>
                  <W25Q40BV id="EF4013" page="256" size="524288"/>
                  <W25Q40CL id="EF4013" page="256" size="524288"/>
                  <W25Q40EW_1.8V id="EF6013" page="256" size="524288"/>
                  <W25Q64BV id="EF4017" page="256" size="8388608"/>
                  <W25Q64CV id="EF4017" page="256" size="8388608"/>
                  <W25Q64FV id="EF4017" page="256" size="8388608"/>
	              <W25Q64JV id="EF4017" page="256" size="8388608"/>
	              <W25Q64JW-IQ_1.8V id="EF6017" page="256" size="8388608"/>
	              <W25Q64JW-IM_1.8V id="EF8017" page="256" size="8388608"/>
                  <W25Q64FW_1.8V id="EF6017" page="256" size="8388608"/>
                  <W25Q80BL id="EF4014" page="256" size="1048576"/>
                  <W25Q80BV id="EF4014" page="256" size="1048576"/>
	              <W25Q80BW_1.8V id="EF5014" page="256" size="1048576"/>
                  <W25Q80DV id="EF4014" page="256" size="1048576"/>
                  <W25Q80EW_1.8V id="EF6014" page="256" size="1048576"/>
                  <W25X05 id="EF3010" page="256" size="65536"/>
                  <W25X05CL id="EF3010" page="256" size="65536"/>
                  <W25X10AV id="EF3011" page="256" size="131072"/>
                  <W25X10BL id="EF3011" page="256" size="131072"/>
                  <W25X10BV id="EF3011" page="256" size="131072"/>
                  <W25X10CL id="EF3011" page="256" size="131072"/>
                  <W25X10L id="EF3011" page="256" size="131072"/>
                  <W25X10V id="EF3011" page="256" size="131072"/>
                  <W25X16 id="EF3015" page="256" size="2097152"/>
                  <W25X16AL id="EF3015" page="256" size="2097152"/>
                  <W25X16AV id="EF3015" page="256" size="2097152"/>
                  <W25X16BV id="EF3015" page="256" size="2097152"/>
                  <W25X16V id="EF3015" page="256" size="2097152"/>
                  <W25X20AL id="EF3012" page="256" size="262144"/>
                  <W25X20AV id="EF3012" page="256" size="262144"/>
                  <W25X20BL id="EF3012" page="256" size="262144"/>
                  <W25X20BV id="EF3012" page="256" size="262144"/>
                  <W25X20CL id="EF3012" page="256" size="262144"/>
                  <W25X20L id="EF3012" page="256" size="262144"/>
                  <W25X20V id="EF3012" page="256" size="262144"/>
                  <W25X32 id="EF3016" page="256" size="4194304"/>
                  <W25X32AV id="EF3016" page="256" size="4194304"/>
                  <W25X32BV id="EF3016" page="256" size="4194304"/>
                  <W25X32V id="EF3016" page="256" size="4194304"/>
                  <W25X40AL id="EF3013" page="256" size="524288"/>
                  <W25X40AV id="EF3013" page="256" size="524288"/>
                  <W25X40BL id="EF3013" page="256" size="524288"/>
                  <W25X40BV id="EF3013" page="256" size="524288"/>
                  <W25X40CL id="EF3013" page="256" size="524288"/>
                  <W25X40L id="EF3013" page="256" size="524288"/>
                  <W25X40V id="EF3013" page="256" size="524288"/>
                  <W25X64 id="EF3017" page="256" size="8388608"/>
                  <W25X64BV id="EF3017" page="256" size="8388608"/>
                  <W25X64V id="EF3017" page="256" size="8388608"/>
                  <W25X80AL id="EF3014" page="256" size="1048576"/>
                  <W25X80AV id="EF3014" page="256" size="1048576"/>
                  <W25X80BV id="EF3014" page="256" size="1048576"/>
                  <W25X80L id="EF3014" page="256" size="1048576"/>
                  <W25X80V id="EF3014" page="256" size="1048576"/>
	              <W25M512JV id="EF7119" page="256" size="67108864"/>
	              <W25R256JV id="EF4019" page="256" size="33554432"/>
                </WINBOND>
                <XICOR>
                  <X25010 page="4" size="128" spicmd="95"/>
                  <X25043 page="4" size="512" spicmd="95"/>
                  <X25045 page="4" size="512" spicmd="95"/>
                  <X25F008 page="32" size="1024" spicmd="95"/>
                  <X25F016 page="32" size="2048" spicmd="95"/>
                  <X25F032 page="32" size="4096" spicmd="95"/>
                  <X25F064 page="32" size="8192" spicmd="95"/>
                  <X5043 page="16" size="512" spicmd="95"/>
                  <X5045 page="16" size="512" spicmd="95"/>
                </XICOR>
	            <XMC>
	              <XM25QU41B_1.8V id="205013" page="256" size="524288"/>
	              <XM25QU80B_1.8V id="205014" page="256" size="1048576"/>
	              <XM25QU16B_1.8V id="205015" page="256" size="2097152"/>
	              <XM25QU16C_1.8V id="205015" page="256" size="2097152"/>
	              <XM25LU32C_1.8V id="205016" page="256" size="4194304"/>
	              <XM25QU32C_1.8V id="205016" page="256" size="4194304"/>
	              <XM25LU64C_1.8V id="204117" page="256" size="8388608"/>
	              <XM25RU64C_1.8V id="204117" page="256" size="8388608"/>
	              <XM25QU64C_1.8V id="204117" page="256" size="8388608"/>	  
	              <XM25LU128C_1.8V id="204118" page="256" size="16777216"/>
	              <XM25RU128C_1.8V id="204118" page="256" size="16777216"/>	  
	              <XM25QU128C_1.8V id="204118" page="256" size="16777216"/>	  
	              <XM25RU256C_1.8V id="204119" page="256" size="33554432"/>
	              <XM25QU256C_1.8V id="204119" page="256" size="33554432"/>	  
	              <XM25RU512C_1.8V id="204120" page="256" size="67108864"/>
	              <XM25QU512C_1.8V id="204120" page="256" size="67108864"/>
                  <XM25QH10B id="204011" page="256" size="131072"/> 
	              <XM25QH20B id="204012" page="256" size="262144"/>
	              <XM25QH40B id="204013" page="256" size="524288"/>
	              <XM25QH80B id="204014" page="256" size="1048576"/>
	              <XM25QE16C id="204015" page="256" size="2097152"/>
	              <XM25QH16C id="204015" page="256" size="2097152"/>
	              <XM25QE32C id="204016" page="256" size="4194304"/>
	              <XM25QH32C id="204016" page="256" size="4194304"/>
	              <XM25RH64C id="204017" page="256" size="8388608"/>
	              <XM25QH64C id="204017" page="256" size="8388608"/>
	              <XM25RH128C id="204018" page="256" size="16777216"/>
	              <XM25QH128C id="204018" page="256" size="16777216"/>
	              <XM25RH256C id="204019" page="256" size="33554432"/>
	              <XM25QH256C id="204019" page="256" size="33554432"/>
	              <XM25RH512C id="204020" page="256" size="67108864"/>
	              <XM25QH512C id="204020" page="256" size="67108864"/>	  
	              <XM25QW16C id="204215" page="256" size="2097152"/>
	              <XM25QW32C id="204216" page="256" size="4194304"/>
	              <XM25QW64C id="204217" page="256" size="8388608"/>
	              <XM25QW128C id="204218" page="256" size="16777216"/>
	              <XM25QW256C id="204219" page="256" size="33554432"/>
	              <XM25QW512C id="204220" page="256" size="67108864"/>
	            </XMC>
                <ZEMPRO>
                  <TS25L512A id="372010" page="256" size="65536"/>
                  <TS25L010A id="373011" page="256" size="131072"/>
                  <TS25L020A id="373012" page="256" size="262144"/>
                  <TS25L16AP id="202015" page="256" size="2097152"/>
                  <TS25L16BP id="202015" page="256" size="2097152"/>
                  <TS25L16P id="372015" page="256" size="2097152"/>
                </ZEMPRO>
	            <Zbit>
	              <ZB25D16 id="5E4015" page="256" size="2097152"/>
	            </Zbit>
                <Berg_Micro>
                  <BG25Q40A id="E04013" page="256" size="524288"/>
                  <BG25Q80A id="E04014" page="256" size="1048576"/>
                  <BG25Q16A id="E04015" page="256" size="2097152"/>
	              <BG25Q32A id="E04016" page="256" size="4194304"/>
                </Berg_Micro>
                <ATMEL>
	              <AT45DB021D id="1F2300" page="264" size="270336" spicmd="45"/>
                  <AT45DB041D id="1F2400" page="264" size="540672" spicmd="45"/>
	              <AT45DB161D id="1F2600" page="528" size="2162688" spicmd="45"/>
	              <AT45DB321D id="1F2701" page="528" size="4325376" spicmd="45"/>
                  <AT25010 page="8" size="128" spicmd="95"/>
                  <AT25010A page="8" size="128" spicmd="95"/>
                  <AT25020 page="8" size="256" spicmd="95"/>
                  <AT25020A page="8" size="256" spicmd="95"/>
                  <AT25040 page="8" size="512" spicmd="95"/>
                  <AT25040A page="8" size="512" spicmd="95"/>
                  <AT25080 page="32" size="1024" spicmd="95"/>
                  <AT25080A page="32" size="1024" spicmd="95"/>
                  <AT25080B page="32" size="1024" spicmd="95"/>
                  <AT25160 page="32" size="2048" spicmd="95"/>
                  <AT25160A page="32" size="2048" spicmd="95"/>
                  <AT25160B page="32" size="2048" spicmd="95"/>
                  <AT25320 page="32" size="4096" spicmd="95"/>
                  <AT25320A page="32" size="4096" spicmd="95"/>
                  <AT25320B page="32" size="4096" spicmd="95"/>
                  <AT25640 page="32" size="8192" spicmd="95"/>
                  <AT25640A page="32" size="8192" spicmd="95"/>
                  <AT25640B page="32" size="8192" spicmd="95"/>
                  <AT25128 page="64" size="16384" spicmd="95"/>
                  <AT25128A page="64" size="16384" spicmd="95"/>
                  <AT25128B page="64" size="16384" spicmd="95"/>
                  <AT25256 page="64" size="32768" spicmd="95"/>
                  <AT25256A page="64" size="32768" spicmd="95"/>
                  <AT25256B page="64" size="32768" spicmd="95"/>
                  <AT25512 page="128" size="65536" spicmd="95"/>
                  <AT25DF021 id="1F4300" page="256" size="262144"/>
                  <AT25DF041 id="1F4400" page="256" size="524288"/>
                  <AT25DF041A id="1F4400" page="256" size="524288"/>
	              <AT25SF041 id="1F8400" page="256" size="524288"/>
                  <AT25DF081 id="1F4500" page="256" size="1048576"/>
                  <AT25DF081A id="1F4500" page="256" size="1048576"/>
                  <AT25DF161 id="1F4600" page="256" size="2097152"/>
                  <AT25DF321 id="1F4700" page="256" size="4194304"/>
                  <AT25DF321A id="1F4700" page="256" size="4194304"/>
                  <AT25DF641 id="1F4800" page="256" size="8388608"/>
                  <AT25F512 id="1F65" page="256" size="65536"/>
                  <AT25F512A id="1F65" page="128" size="65536"/>
	              <AT25F512B id="1F6500" page="256" size="65536"/>
                  <AT25F1024 id="1F60" page="256" size="131072"/>
                  <AT25F1024A id="1F60" page="256" size="131072"/>
                  <AT25F2048 id="1F63" page="256" size="262144"/>
                  <AT25F2048A id="1F63" page="256" size="262144"/>
                  <AT25F4096 id="1F64" page="256" size="524288"/>
                  <AT25F4096A id="1F64" page="256" size="524288"/>
                  <AT25HP256 page="128" size="32768" spicmd="95"/>
                  <AT25HP512 page="128" size="65536" spicmd="95"/>
                  <AT26DF081 id="1F4500" page="256" size="1048576"/>
                  <AT26DF081A id="1F4500" page="256" size="1048576"/>
                  <AT26DF161 id="1F4600" page="256" size="2097152"/>
                  <AT26DF161A id="1F4600" page="256" size="2097152"/>
                  <AT26DF321 id="1F4700" page="256" size="4194304"/>
                  <AT26DF321A id="1F4700" page="256" size="4194304"/>
                  <AT26F004 id="1F0400" page="256" size="524288"/>
                </ATMEL>
	            <ACE>
                  <ACE25A128G_1.8V id="E06018" page="256" size="16777216"/>
                </ACE>
                <ATO>
                  <ATO25Q32 id="9B3216" page="256" size="4194304"/>
                </ATO>
	            <DOUQI>
                  <DQ25Q64A id="544017" page="256" size="8388608"/>
                </DOUQI>
                <Fremont>
                  <FT25H16 id="0E4015" page="256" size="2097152"/>
                </Fremont>
                <Fudan>
	              <FM25Q04A id="A14013" page="256" size="524288"/>
                  <FM25Q32 id="A14016" page="256" size="4194304"/>
                </Fudan>
                <Genitop>
                  <GT25Q80A id="E04014" page="256" size="1048576"/>
                </Genitop>
                <Paragon>
                  <PN25F04A id="E04013" page="256" size="524288"/>
                </Paragon>
              </SPI>
              <I2C>
                <_24Cxxx>
                  <AT24C01 page="1" size="128" addrtype="0"/>
                  <_24C01 page="1" size="128" addrtype="1"/>
                  <_24C02 page="1" size="256" addrtype="1"/>
                  <_24C04 page="1" size="512" addrtype="2"/>
                  <_24C08 page="16" size="1024" addrtype="3"/>
                  <_24C16 page="16" size="2048" addrtype="4"/>
                  <_24C32 page="32" size="4096" addrtype="5"/>
                  <_24C64 page="32" size="8192" addrtype="5"/>
                  <_24C128 page="64" size="16384" addrtype="5"/>
                  <_24C256 page="64" size="32768" addrtype="5"/>
                  <_24C512 page="128" size="65536" addrtype="5"/>
                  <_24C1024 page="128" size="131072" addrtype="6"/>
                </_24Cxxx>
              </I2C>
              <Microwire>
                <Microchip>
                  <M93C86 size="2048" addrbitlen="10"/>
                  <M93C76 size="1024" addrbitlen="10"/>
                  <M93C66 size="512" addrbitlen="8"/>
                  <M93C56 size="256" addrbitlen="8"/>
                  <M93C46 size="128" addrbitlen="6"/>
                  <M93C06 size="16" addrbitlen="6"/>
                </Microchip>
              </Microwire>
          </chiplist>
        */
        #endregion chipset
    }
}
