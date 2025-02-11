using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIO
{

    
    public struct typCAATG
    {
        public string BodyID;
        public float OverBet;       //亮度超過1000後降低ratio
        public bool OverBetEnabled;
        public bool PlugSP;         //No Extend card

        public int CAchNo;
        public int CAmode;
        public string CAchID;
        public string Class;
        public bool Demo;
        public bool FLIC;           //Probe是否支援
        public string ProbeSN;      //CA-210可以回傳ProbeID(數字)
        public string ProbeInfo;
        public bool Zeroed;

        public int radius;          //半徑
        public string version;
        public byte CAsel;
    }


    public struct typMsrData
    {
        public float[,] BX;
        public float[,] BY;
        public float[,] BZ;

        public float[,] Lv;
        public float[,] Sx;
        public float[,] Sy;

        public float[,] CCT;

    }


    class mCAs
    {
        public static CA200SRVRLib.Ca200 objCa200;
        public static CA200SRVRLib.Ca objCa;
        public static CA200SRVRLib.Probe objProbe;
        public static CA200SRVRLib.Memory objMemory;
        public static CA200SRVRLib.IProbeInfo objProbeInfo;

        public static typCAATG CAATG;

        public static long vbObjectError = -2147221504;
        public static string msg;



        public static void CAinit() 
        {
            try
            {
                msg = "";
                objCa200 = new CA200SRVRLib.Ca200();
                objCa200.AutoConnect();

                mCAs.objCa = objCa200.Cas.ItemOfNumber[1];

                objProbe = mCAs.objCa.Probes.ItemOfNumber[1];

                objCa.DisplayMode = 0;

                CAATG.ProbeSN = mCAs.objProbe.SerialNO;


                objProbeInfo = (CA200SRVRLib.IProbeInfo)objProbe;
                CAATG.Class = objProbeInfo.TypeName;

                

                mCAs.objMemory = objCa.Memory;
                CAATG.BodyID = objCa.ID;
                CAATG.CAchNo = objMemory.ChannelNO;
                CAATG.CAchID = objMemory.ChannelID;
                CAATG.ProbeInfo = objProbeInfo.TypeName;

                CAATG.ProbeSN = objProbeInfo.TypeNO.ToString();
                //int Svi = objProbeInfo.TypeNO;
                //switch (Svi)
                //{
                //    case 1001:

                //        //.ProbeInfo = "Measuring Probe (CA-P02/05)，不支援 Flickr 量測" 
                //        CAATG.ProbeInfo = "CA-P02/05(w/o FLick)";
                //        CAATG.FLIC = false;
                //        break;

                //    case 1002:
                //        //.ProbeInfo = "High luminance Measuring Probe (CA-PH02/05)，不支援 Flickr 量測"
                //        CAATG.ProbeInfo = "CA-PH02/05(w/o FLick)";
                //        CAATG.FLIC = false;
                //        break;

                //    case 2102:
                //        //.ProbeInfo = "Universal Measuring Probe (CA-PU12/15)，不支援 Flickr 量測"
                //        CAATG.ProbeInfo = "CA-PU12/15(w/o FLick)";
                //        CAATG.FLIC = false;
                //        break;

                //    case 2103:
                //        //.ProbeInfo = "Small Universal Measuring Probe (CA-PSU12/15)，不支援 Flickr 量測"
                //        CAATG.ProbeInfo = "CA-PSU12/15(w/o FLick)";
                //        CAATG.ProbeInfo = objProbeInfo.TypeName;
                //        CAATG.FLIC = false;
                //        break;

                //    case 2100:
                //        //.ProbeInfo = "LCD Flicker Measuring Probe (CA-P12/15)"
                //        CAATG.ProbeInfo = "CA-P12/15";
                //        CAATG.FLIC = true;
                //        break;

                //    case 2101:
                //        //.ProbeInfo = "Small LCD Flicker Measuring Probe (CA-PS12/15)"
                //        CAATG.ProbeInfo = "CA-PS12/15";
                //        CAATG.FLIC = true;
                //        break;

                //    case 2512:
                //        //.ProbeInfo = "LED Universal Measuring Φ27 Probe (CA-PU32/35)，不支援 Flickr 量測"
                //        CAATG.ProbeInfo = "CA-PU32/35(w/o FLick)";
                //        CAATG.FLIC = false;
                //        break;

                //    case 2513:
                //        //.ProbeInfo = "LED Universal Measuring Φ10 Probe (CA-PSU32/35)，不支援 Flickr 量測"
                //        CAATG.ProbeInfo = "CA-PSU32/35(w/o FLick)";
                //        CAATG.FLIC = false;
                //        break;

                //    case 2500:
                //        //.ProbeInfo = "LED Flicker Measuring Φ27 Probe (CA-P32/35)"
                //        CAATG.ProbeInfo = "CA-P32/35";
                //        CAATG.FLIC = true;
                //        break;

                //    case 2501:
                //        //.ProbeInfo = "LED Flicker Measuring Φ10 Probe (CA-PS32/35)"
                //        CAATG.ProbeInfo = "CA-PS32/35";
                //        CAATG.FLIC = true;
                //        break;


                //}
                objCa.RemoteMode = 0;
                CAATG.PlugSP = true;
            }
            catch (Exception er)
            {
                msg = DisplayError(er);
                CAATG.PlugSP = false;
            }
        }

        public static void CAzero()
        {
            objCa.RemoteMode = 1;
            try
            {
                objCa.CalZero();
                CAATG.Zeroed = true;
            }
            catch (Exception er)
            {
                CAATG.Zeroed = false; ;
                msg = DisplayError(er);
                //mvars.lstget.Items.Add(msg);
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
            }
            objCa.RemoteMode = 0;
        }

        public static string DisplayError(Exception er) 
        {
            String msg;
            msg = "Error from" + er.Source + "\r\n";
            msg += er.Message + "\r\n";
            msg += "HR:" + (er.HResult - (-2147221504)).ToString();
            //MessageBox.Show(msg);
            return msg;
        }

        public static void CAremote(bool Svb)
        {
            if (mCAs.CAATG.Demo == false && mCAs.CAATG.PlugSP)
            {
                if (mCAs.CAATG.CAsel == 0) { if (Svb) objCa.RemoteMode = 1; else objCa.RemoteMode = 0; }
            }
        }
        public static void CAdissconnect()
        {
            if (mCAs.CAATG.Demo == false && mCAs.CAATG.PlugSP)
            {
                if (mCAs.CAATG.CAsel == 0) { objCa.RemoteMode = 0; }
                else if (mCAs.CAATG.CAsel == 1) { mCA4.Disconnect(); }
            }
        }


    }
}
