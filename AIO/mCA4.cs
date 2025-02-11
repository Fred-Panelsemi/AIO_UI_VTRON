using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CASDK2;

namespace AIO
{
    


    class mCA4
    {
        #region CASDK2
        //Declaration of objects
        public static CASDK2Ca200 _objCa200;
        //public static CASDK2.CASDK2Cas _objCas;
        public static CASDK2.CASDK2Ca _objCa;
        //public static CASDK2.CASDK2Probes _objProbes;
        //public static CASDK2.CASDK2OutputProbes _objOutputProbes;
        public static CASDK2.CASDK2Probe _objProbe;
        public static CASDK2.CASDK2Memory _objMemory;
        //public static CASDK2.CASDK2IntervalMetadata _intervalMetData;
        public static int err = 0;
        public const int MAXPROBE = 10;
        public const int MODE_Lvxy = 0;
        public const int MODE_Tduv = 1;
        public const int MODE_Lvdudv = 5;
        public const int MODE_FMA = 6;
        public const int MODE_XYZ = 7;
        public const int MODE_JEITA = 8;
        public const int MODE_LvPeld = 9;
        public const int MODE_Waveform = 10;
        public const int MODE_FMA2 = 11;
        public const int MODE_JEITA2 = 12;
        public const int MODE_Waveform2 = 13;
        public const int SPD_SLOW = 0;
        public const int SPD_FAST = 1;
        public const int SPD_LTDAUTO = 2;
        public const int SPD_AUTO = 3;
        public static byte SPD_Select = 1;
        public static string[] strSped = { "SLOW", "FAST", "LTD.AUTO", "AUTO" };
        public static int AVG_IColorNum = 1;
        public static int AVG_IFlickerNum = 1;
        public static string[] strSyncmode = { "NTSC", "PAL", "EXT", "UNIV", "INT", "MANUAL" };
        public const int RED = 0;
        public const int GREEN = 1;
        public const int BLUE = 2;
        public const int WHITE = 3;
        #endregion


        public static bool autoconnectflag = true; // auro or manual
        public static bool triggerfinish = true;

        public static typCAATG CAp;
        public static string[] CA4CH = new string[100];
        public static string[] CA4chTx = new string[100];
        public static string[] CA4chTy = new string[100];
        public static string[] CA4chTLv = new string[100];

        public static int Sync_Select = 1;
        public static double Sync_Fq = 60;
        public static double Sync_msec = 4;



        public static void AutoConnect()
        {
            _objCa200 = new CASDK2Ca200();   
            byte sverrcount = 0;
            string sverr = geterrMsg(_objCa200.AutoConnect());
            if (sverr != "OK") { mCAs.msg = sverr; return; } else { mCAs.msg = ""; }
            sverr = geterrMsg(_objCa200.get_SingleCa(ref _objCa)); if (sverr != "OK") { sverrcount++; }
            sverr = geterrMsg(_objCa.get_Memory(ref _objMemory)); if (sverr != "OK") { sverrcount++; }
            sverr = geterrMsg(_objCa.get_SingleProbe(ref _objProbe)); if (sverr != "OK") { sverrcount++; }

            sverr = geterrMsg(_objCa.put_AveragingMode(SPD_Select)); if (sverr != "OK") { sverrcount++; }

            if (sverrcount == 0) { mCA4.autoconnectflag = true; mCAs.CAATG.PlugSP = true; } else { mCA4.autoconnectflag = false; mCAs.CAATG.PlugSP = false; return; }

            string strerr = "";
            int svProbeno = 1;
            CASDK2MemoryStatus svData = new CASDK2MemoryStatus();
            for (int svch = 0; svch < CA4CH.Length; svch++)
            {
                strerr = geterrMsg(_objMemory.GetMemoryCHData(svProbeno, svch, ref svData));
                if (svData.sIDName == "") { CA4CH[svch] = "CH" + string.Format("{0:00}", svch) + "  " + "< CH" + string.Format("{0:00}", svch) + " >"; }
                else { CA4CH[svch] = "CH" + string.Format("{0:00}", svch) + "  " + svData.sIDName; }
                CA4chTLv[svch] = svData.fdClr_W_Lv_cdm2.ToString();
                CA4chTx[svch] = svData.fdClr_W_sx.ToString();
                CA4chTy[svch] = svData.fdClr_W_sy.ToString();
            }

            string svstr = "";
            strerr = geterrMsg(_objCa.get_CAVersion(ref svstr));
            mCA4.CAp.version = svstr;
            svstr = "";
            strerr = geterrMsg(_objProbe.get_SerialNO(ref svstr));
            mCAs.CAATG.ProbeSN = svstr;
            svstr = "";
            strerr = geterrMsg(_objProbe.get_TypeName(ref svstr));
            mCA4.CAp.ProbeInfo = svstr;

            svstr = "";
            strerr = geterrMsg(_objCa.GetAveragingMeasMode(ref AVG_IColorNum, ref AVG_IFlickerNum));

            svstr = "";
            double svDo = 0;
            strerr = geterrMsg(_objProbe.get_SyncMode(ref Sync_Select, ref svDo));
            if (Sync_Select == 4) { Sync_Fq = svDo; }
            else if (Sync_Select == 5) { Sync_msec = svDo; }

            int svtypeno = 0;
            strerr = geterrMsg(_objProbe.get_TypeNO(ref svtypeno));
            if (strerr == "OK")
            {
                mCAs.CAATG.Class = "CA-410";
                switch (svtypeno)
                {
                    case 00830:
                        mCAs.CAATG.ProbeInfo = "Φ10 mini probe";
                        mCAs.CAATG.radius = 5;
                        break;
                    case 00831:
                        mCAs.CAATG.ProbeInfo = "Φ10 mini high-luminance probe";
                        mCAs.CAATG.radius = 5;
                        break;
                    case 00810:
                        mCAs.CAATG.ProbeInfo = "Φ27 probe";
                        mCAs.CAATG.radius = 15;
                        break;
                    case 00811:
                        mCAs.CAATG.ProbeInfo = "Φ27 high-luminance probe";
                        mCAs.CAATG.radius = 15;
                        break;
                    case 00890:
                        mCAs.CAATG.ProbeInfo = "Φ10 probe";
                        mCAs.CAATG.radius = 5;
                        break;
                    case 00891:
                        mCAs.CAATG.ProbeInfo = "Φ10 high-luminance probe";
                        mCAs.CAATG.radius = 5;
                        break;
                    case 00840:
                        mCAs.CAATG.ProbeInfo = "Φ27 high-sensitivity probe";
                        mCAs.CAATG.radius = 15;
                        break;
                    case 00850:
                        mCAs.CAATG.ProbeInfo = "Φ10 high-sensitivity probe";
                        mCAs.CAATG.radius = 5;
                        break;
                    case 00100:
                        mCAs.CAATG.Class = "data processor";
                        mCAs.CAATG.ProbeInfo = "";
                        mCAs.CAATG.radius = 0;
                        break;
                }
                //mCAs.CAATG.FLIC = true;
                //sverr = geterrMsg(objMemory.put_ChannelNO(mCA4.CAp.CAchNo));
            }
        }
        public static string geterrMsg(int errornum)
        {
            string errormessage = "OK";
            if (errornum != 0)
            {
                //Get Error message from Error number
                err = GlobalFunctions.CASDK2_GetLocalizedErrorMsgFromErrorCode(0, errornum, ref errormessage);
            }
            return errormessage;
        }
        public static void CAzero()
        {
            mCAs.msg = geterrMsg(mCA4._objCa.CalZero());
            if (mCAs.msg == "OK") mCAs.CAATG.Zeroed = true;
            else mCAs.CAATG.Zeroed = false;
        }

        public static void Disconnect()
        {
            while (!triggerfinish)
            {
                System.Threading.Thread.Sleep(10);  //wait for completion of trigger measurement
            }
            //Disconnect CA-410
            if (autoconnectflag)
            {
                string sverr = geterrMsg(_objCa200.AutoDisconnect()); //Disconnect probe connected automatically
                autoconnectflag = false; mCAs.CAATG.PlugSP = false;
            }
            else
            {
                string sverr = geterrMsg(_objCa200.DisconnectAll());  //Disconnect probe connected manually
            }
        }

        

    }
}
