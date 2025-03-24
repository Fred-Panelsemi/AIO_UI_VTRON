using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
//using System.Drawing;
using System.IO;
using System.Diagnostics;               //Process，taskkill
using System.Management;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using eDRun;



namespace AIO
{
    class mp
    {

        #region 釋放commport
        /// 先取得Serial Port的Handle
        /// 再把Handle Close就釋放了
        //    using Microsoft.Win32.SafeHandles;
        //    using System.Reflection;
        //    private void ComClose()
        //      {
        //    ComPort.Close();
        //    object stream = typeof(SerialPort).GetField("internalSerialStream", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ComPort);
        //    SafeFileHandle handle_Com1 = (SafeFileHandle)stream.GetType().GetField("_handle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(stream);
        //    handle_Com1.Close();
        //      }
        #endregion



        /// <summary>\
        /// 20230322起刪除以下未被參考的副程式，需查閱請找 20230317(含) 以前備存 
        /// Unlock_603()    
        /// Gamma_603_G0()
        /// Vref_603()
        /// Lock_603()
        /// TFTVREF_603
        /// cWTGRAY_cf(string svR, string svG, string svB)
        /// </summary>


        [DllImport("winmm")]
        static extern uint timeGetTime();

        [DllImport("winmm")]
        public static extern void timeBeginPeriod(int t);

        [DllImport("winmm")]
        public static extern void timeEndPeriod(int t);

        public static string[] strC = new string[6] { "BLU", "W", "R", "G", "B", "D" };
        public static int gmax = mvars.GMAterminals;
        static string strCAFxLv;
        static string strCAFx;
        static string strCAFy;
        public static float CAFxLv;
        public static float CAFxX;
        public static float CAFxY;
        public static float CAFxZ;
        public static float CAFx;
        public static float CAFy;

        //public static float G0Rratio;
        //public static float G0Gratio;
        //public static float G0Bratio;



        public static byte[] FlashRd_Arr = new byte[1];






        #region Transfer
        public static int HexToDec(string SvVa)
        {
            int SvintVal = 0;
            for (int svi = SvVa.Length; svi > 0; svi--)
            {
                string Svtemp = SvVa.Substring(SvVa.Length - svi, 1);
                if (Svtemp == "A" || Svtemp == "a")
                {
                    SvintVal += 10 * (int)Math.Pow(16, (svi - 1));
                }
                else if (Svtemp == "B" || Svtemp == "b")
                {
                    SvintVal += 11 * (int)Math.Pow(16, (svi - 1));
                }
                else if (Svtemp == "C" || Svtemp == "c")
                {
                    SvintVal += 12 * (int)Math.Pow(16, (svi - 1));
                }
                else if (Svtemp == "D" || Svtemp == "d")
                {
                    SvintVal += 13 * (int)Math.Pow(16, (svi - 1));
                }
                else if (Svtemp == "E" || Svtemp == "e")
                {
                    SvintVal += 14 * (int)Math.Pow(16, (svi - 1));
                }
                else if (Svtemp == "F" || Svtemp == "f")
                {
                    SvintVal += 15 * (int)Math.Pow(16, (svi - 1));
                }
                else
                {
                    SvintVal += Convert.ToInt32(Svtemp) * (int)Math.Pow(16, (svi - 1));
                }
            }
            return SvintVal;
        }
        public static string Byte2HexString(byte tmp)
        {
            byte[] arr = { tmp };
            string str = BitConverter.ToString(arr);
            return str;
        }
        public static string DecToHex(int SvVa, int SvBits)
        {
            string os = "";
            string[] temp = new String[4];
            int i;
            //int intl = SvVa.ToString().Length - 1;
            int Svj = SvVa;
            
            for (i = 0; i <= 3; i++)
            {
                temp[i] = (Svj % 16).ToString();
                //Svj = Svj / 16;
                Svj /= 16;
                switch (temp[i])
                {
                    case "10":
                        temp[i] = "A"; break;
                    case "11":
                        temp[i] = "B"; break;
                    case "12":
                        temp[i] = "C"; break;
                    case "13":
                        temp[i] = "D"; break;
                    case "14":
                        temp[i] = "E"; break;
                    case "15":
                        temp[i] = "F"; break;
                }
                os = temp[i] + os;
            }
            if (SvBits < 4)
            {
                os = os.Substring(4 - SvBits, SvBits);  //=Right()
            }
            return os;
        }


        public static string ByteToHex(int SvVa)
        {
            string os = "";
            string[] temp = new String[4];
            int i;
            //int intl = SvVa.ToString().Length - 1;
            int Svj = SvVa;
            
            for (i = 0; i <= 1; i++)
            {
                temp[i] = (Svj % 16).ToString();
                //Svj = Svj / 16;
                Svj /= 16;
                switch (temp[i])
                {
                    case "10":
                        temp[i] = "A"; break;
                    case "11":
                        temp[i] = "B"; break;
                    case "12":
                        temp[i] = "C"; break;
                    case "13":
                        temp[i] = "D"; break;
                    case "14":
                        temp[i] = "E"; break;
                    case "15":
                        temp[i] = "F"; break;
                }
                os = temp[i] + os;
            }
            return os;
        }


        public static int BinToDec(string binarys)
        {
            int returnInt = 0;
            for (int i = 1; i <= binarys.Length; i++)
            {
                returnInt += Convert.ToInt32(binarys.Substring(binarys.Length - i, 1)) * Convert.ToInt32(Math.Pow(2, (i - 1)));
            }
            return returnInt;
        }
        public static string BinToHex(string binarys, int SvBits)
        {
            int returnInt = 0;
            for (int i = 1; i <= binarys.Length; i++)
            {
                returnInt += Convert.ToInt32(binarys.Substring(binarys.Length - i, 1)) * Convert.ToInt32(Math.Pow(2, (i - 1)));
            }
            return DecToHex(returnInt, SvBits);
        }
        public static string DecToBin(int SvNumber, int SvBits)
        {
            string os = "";
            int i;
            int j;
            for (i = SvBits - 1; i >= 0; i--)
            {
                j = Convert.ToInt32(Math.Pow(2, i));
                if ((SvNumber / j) == 1)
                {
                    //os = os + 1;
                    os += "1";
                    SvNumber -= Convert.ToInt32(Math.Pow(2, i));
                }
                else { os += 0; } //os = os + 0;
            }
            return os;
        }
        public static bool IsNumeric(object Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            //bool isNum;
            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            //double retNum;
            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out double retNum);
            return isNum;
        }
        public static string replaceRxx(string svStr, string svmIndex, string svW)
        {
            string svs;
            if (svStr.Length > 2)
            {
                if (svStr.Substring(0, 1) == "~") { svStr = svStr.Substring(1, svStr.Length - 1); }
                string[] svmk = svStr.Split('~');
                for (int m = 0; m < svmk.Length; m++)
                {
                    if (svmk[m].IndexOf(svmIndex, 0) > -1) { svmk[m] = svmIndex + svW; break; }
                }
                svs = svmk[0];
                if (svmk.Length > 1) { for (int i = 1; i < svmk.Length; i++) { svs += "~" + svmk[i]; } }
            }
            else { svs = svmIndex + svW; }
            return svs;
        }
        public static void RegData2Byte_splited(int sRegAddr, string sRegData, ref byte[] arr, UInt16 Index)
        {
            arr[Index + 0] = (byte)(sRegAddr / 256);
            arr[Index + 1] = (byte)(sRegAddr % 256);
            arr[Index + 2] = (byte)(Convert.ToUInt16(sRegData) / 256);
            arr[Index + 3] = (byte)(Convert.ToUInt16(sRegData) % 256);
        }
        public static void RegData2Byte(string sRegData, ref byte[] arr, UInt16 Index)
        {
            string sTmp;
            sTmp = sRegData.Replace(" ", "");
            string[] split = sTmp.Split(new Char[] { ',' });
            arr[Index + 0] = (byte)(Convert.ToUInt16(split[0]) / 256);
            arr[Index + 1] = (byte)(Convert.ToUInt16(split[0]) % 256);
            arr[Index + 2] = (byte)(Convert.ToUInt16(split[1]) / 256);
            arr[Index + 3] = (byte)(Convert.ToUInt16(split[1]) % 256);
        }
        public static void RegData1Byte(string sRegData, ref byte[] arr, UInt16 Index)
        {
            string sTmp;
            sTmp = sRegData.Replace(" ", "");
            string[] split = sTmp.Split(new Char[] { ',' });
            arr[Index + 0] = (byte)(Convert.ToUInt16(split[1]) / 256);
            arr[Index + 1] = (byte)(Convert.ToUInt16(split[1]) % 256);
        }
        #endregion Transfer


        public static Screen[] screens = null;
        public static int upperBound;
        public static void ShowExtendScreen(ref int extX, ref int extY)
        {
            screens = Screen.AllScreens;
            //{Bounds = {X = 0 Y = 0 Width = 1366 Height = 768} WorkingArea = {X = 0 Y = 0 Width = 1366 Height = 728} Primary = true DeviceName = "\\\\.\\DISPLAY1"}
            //{Bounds = {X = 1366 Y = 0 Width = 1920 Height = 1080} WorkingArea = {X = 1366 Y = 0 Width = 1920 Height = 1040} Primary = false DeviceName = "\\\\.\\DISPLAY2"}
            upperBound = screens.GetUpperBound(0);
            if (upperBound == 0)
            {
                string Svs = screens[upperBound].Bounds.ToString();                                     //Svs={X=0,Y=0,Width=1366,Height=768}
                if (Svs.Substring(Svs.Length - 1, 1) == "}") Svs = Svs.Substring(0, Svs.Length - 1);    //Svs={X=0,Y=0,Width=1366,Height=768
                string[] Svss = Svs.Split(',');
                foreach (var word in Svss)
                {
                    if (word.IndexOf("Width=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        mvars.UUT.resW = Convert.ToSingle(Svsss[1]);
                        Form2.priRes[0] = (int)mvars.UUT.resW;
                    }
                    else if (word.IndexOf("Height=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        mvars.UUT.resH = Convert.ToSingle(Svsss[1]);
                        Form2.priRes[1] = (int)mvars.UUT.resH;
                    }
                    else if (word.IndexOf("X=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        extX = Convert.ToInt16(Svsss[1]);
                    }
                    else if (word.IndexOf("Y=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        extY = Convert.ToInt16(Svsss[1]);
                    }
                }
                mvars.ratioX = 1;
                mvars.ratioY = 1;
            }
            else
            {
                string Svs = screens[upperBound].Bounds.ToString();
                string[] Svss = Svs.Split('}');
                Svss = Svss[0].Split(',');
                foreach (var word in Svss)
                {
                    if (word.IndexOf("Width=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        mvars.UUT.resW = Convert.ToInt16(Svsss[1]);
                    }
                    else if (word.IndexOf("Height=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        mvars.UUT.resH = Convert.ToInt16(Svsss[1]);
                    }
                    else if (word.IndexOf("X=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        extX = Convert.ToInt16(Svsss[1]);
                    }
                    else if (word.IndexOf("Y=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        extY = Convert.ToInt16(Svsss[1]);
                    }
                }
                mvars.ratioX = 0;
                mvars.ratioY = 0;
            }
        }

        public static void checkExtendScreen(ref int extX, ref int extY)
        {
            screens = Screen.AllScreens;
            //{Bounds = {X = 0 Y = 0 Width = 1366 Height = 768} WorkingArea = {X = 0 Y = 0 Width = 1366 Height = 728} Primary = true DeviceName = "\\\\.\\DISPLAY1"}
            //{Bounds = {X = 1366 Y = 0 Width = 1920 Height = 1080} WorkingArea = {X = 1366 Y = 0 Width = 1920 Height = 1040} Primary = false DeviceName = "\\\\.\\DISPLAY2"}
            upperBound = screens.GetUpperBound(0);
            if (upperBound == 0)
            {
                string Svs = screens[upperBound].Bounds.ToString();                                     //Svs={X=0,Y=0,Width=1366,Height=768}
                if (Svs.Substring(Svs.Length - 1, 1) == "}") Svs = Svs.Substring(0, Svs.Length - 1);    //Svs={X=0,Y=0,Width=1366,Height=768
                string[] Svss = Svs.Split(',');
                foreach (var word in Svss)
                {
                    if (word.IndexOf("Width=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        mvars.UUT.resW = Convert.ToSingle(Svsss[1]);
                        Form2.priRes[0] = (int)mvars.UUT.resW;
                    }
                    else if (word.IndexOf("Height=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        mvars.UUT.resH = Convert.ToSingle(Svsss[1]);
                        Form2.priRes[1] = (int)mvars.UUT.resH;
                    }
                    else if (word.IndexOf("X=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        extX = Convert.ToInt16(Svsss[1]);
                    }
                    else if (word.IndexOf("Y=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        extY = Convert.ToInt16(Svsss[1]);
                    }
                }
            }
            else
            {
                //string Svs = screens[upperBound].Bounds.ToString(); //遇到 4k2k 的時候無法正確地打出畫面
                string Svs = screens[upperBound].Bounds.ToString();
                foreach (var word in screens)
                {
                    if (!(word.Bounds.ToString().IndexOf("X=0", 0) != -1 && word.Bounds.ToString().IndexOf("Y=0", 0) != -1)) { Svs = word.Bounds.ToString(); break; }
                }
                string[] Svss = Svs.Split('}');
                Svss = Svss[0].Split(',');
                foreach (var word in Svss)
                {
                    if (word.IndexOf("Width=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        mvars.UUT.resW = Convert.ToInt16(Svsss[1]);
                    }
                    else if (word.IndexOf("Height=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        mvars.UUT.resH = Convert.ToInt16(Svsss[1]);
                    }
                    else if (word.IndexOf("X=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        extX = Convert.ToInt16(Svsss[1]);
                    }
                    else if (word.IndexOf("Y=", 0) != -1)
                    {
                        string[] Svsss = word.Split('=');
                        extY = Convert.ToInt16(Svsss[1]);
                    }
                }
            }
        }


        public static DialogResult InputBox(string title, string promptText, ref string value, int exHeight)
        {
            Form frm = new Form();
            Label lbl = new Label();
            TextBox txt = new TextBox();
            Button bOK = new Button();
            Button bCancel = new Button();


            frm.Text = title;
            lbl.Text = promptText;
            txt.Text = value;

            bOK.Text = "OK";
            bCancel.Text = "Cancel";
            bOK.DialogResult = DialogResult.OK;
            bCancel.DialogResult = DialogResult.Cancel;

            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new System.Drawing.Point(100, 100);
            lbl.SetBounds(0, 10, 400, exHeight); 
            lbl.Font = new System.Drawing.Font("Arial", 10);
            frm.ClientSize = new System.Drawing.Size(lbl.Right + 50, Math.Max(113, 55 + exHeight));
            txt.SetBounds(10, frm.Height - 75, frm.Width - 35, 12);
            txt.Font = new System.Drawing.Font("Arial", 9);
            bOK.SetBounds(lbl.Width + (frm.Width - lbl.Width) / 2 + 5, 11, 60, 23); bOK.Font = new System.Drawing.Font("Arial", 8);
            bCancel.SetBounds(lbl.Width + (frm.Width - lbl.Width) / 2 + 5, 40, 60, 23); bCancel.Font = new System.Drawing.Font("Arial", 8);

            bOK.Left = frm.Width - bOK.Width - 30;
            bCancel.Left = frm.Width - bOK.Width - 30;

            frm.Controls.AddRange(new Control[] { lbl, txt, bOK, bCancel });
            frm.FormBorderStyle = FormBorderStyle.FixedDialog;
            frm.MinimizeBox = false;
            frm.MaximizeBox = false;
            frm.AcceptButton = bOK;
            frm.CancelButton = bCancel;
            bOK.BringToFront();
            bCancel.BringToFront();

            DialogResult dialogResult = frm.ShowDialog();
            value = txt.Text;
            return dialogResult;
        }

        public static DialogResult InputBox(string title, string promptText, ref string value, int exLeft, int exTop, int exWidth, int exHeight, byte svinputmode, string svpassword)
        {
            frm = new Form();
            Label lbl = new Label();
            TextBox txt = new TextBox();
            Button bOK = new Button();
            Button bCancel = new Button();

            if (title == "") title = mvars.strUInameMe;
            frm.Text = title;
            lbl.Text = promptText;
            txt.Text = value;
            if (svpassword == "") { txt.PasswordChar = default(char); }
            else { txt.PasswordChar = Convert.ToChar(svpassword); }

            bOK.Text = "OK";
            bCancel.Text = "Cancel";
            bOK.DialogResult = DialogResult.OK;
            bCancel.DialogResult = DialogResult.Cancel;

            if (exLeft == 0 && exTop == 0) { frm.StartPosition = FormStartPosition.CenterScreen; }
            else { frm.StartPosition = FormStartPosition.Manual; frm.Location = new System.Drawing.Point(exLeft, exTop); }
            frm.MinimizeBox = false;
            frm.MaximizeBox = false;
            //frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            frm.AcceptButton = bOK;
            frm.CancelButton = bCancel;

            lbl.AutoSize = true;
            lbl.SetBounds(10, 10, 63, 15); 
            lbl.Font = new System.Drawing.Font("Arial", 9);
            txt.Font = new System.Drawing.Font("Arial", 10);
            txt.SetBounds(lbl.Left + lbl.Width + 16, lbl.Top, frm.Width - 35, 12);
            //frm.Size = new Size(lbl.Left + 273, Math.Max(121 + lbl.Top, 55 + exHeight));
            txt.Width = frm.Width - txt.Left - 36;

            bOK.SetBounds(lbl.Width + (frm.Width - lbl.Width) / 2 + 5, 11, 66, 31); bOK.Font = new System.Drawing.Font("Arial", 9);
            bCancel.SetBounds(lbl.Width + (frm.Width - lbl.Width) / 2 + 5, 40, 66, 31); bCancel.Font = new System.Drawing.Font("Arial", 9);

            if (svinputmode == 1)
            {
                frm.Controls.AddRange(new Control[] { lbl, txt, bOK, bCancel });
                txt.Location = new System.Drawing.Point(lbl.Left + 5, lbl.Top + lbl.Height + 30);
                txt.Width = lbl.Width + 20;
                bOK.Location = new System.Drawing.Point(txt.Left + txt.Width + 10, lbl.Top);
                bCancel.Location = new System.Drawing.Point(txt.Left + txt.Width + 10, lbl.Top + bOK.Height + 10);
                frm.Size = new System.Drawing.Size(bOK.Left + bOK.Width + 30, txt.Top + txt.Height + 50);
                if (value == "") { txt.Visible = false; bCancel.Visible = false; }
            }
            else if (svinputmode == 2)
            {
                frm.Controls.AddRange(new Control[] { lbl, txt, bOK, bCancel });
                txt.Location = new System.Drawing.Point(lbl.Left + 5, lbl.Top + lbl.Height + 30);
                txt.Width = lbl.Width + 20;
                bOK.Location = new System.Drawing.Point(txt.Left + txt.Width + 10, lbl.Top);
                bCancel.Location = new System.Drawing.Point(txt.Left + txt.Width + 10, lbl.Top + bOK.Height + 10);
                int svw = bOK.Left + bOK.Width + 30;
                int svh = txt.Top + txt.Height + 50;
                if (svw < 350) svw = 350;
                if (svh < 250) svh = 250;
                frm.Size = new System.Drawing.Size(svw, svh);
                bOK.Location = new System.Drawing.Point(svw - bOK.Width - 30, lbl.Top);
                bCancel.Location = new System.Drawing.Point(svw - bOK.Width - 30, lbl.Top + bOK.Height + 10);
                txt.Visible = false;
            }
            else if (svinputmode == 3)
            {
                frm.Controls.AddRange(new Control[] { lbl, bOK, bCancel });
                lbl.Location = new System.Drawing.Point(30, 40);
                bOK.Location = new System.Drawing.Point(lbl.Left + lbl.Width + 50, 10);
                bCancel.Location = new System.Drawing.Point(lbl.Left + lbl.Width + 50, bOK.Top + bOK.Height + 10);
                frm.Size = new System.Drawing.Size(Math.Max(bOK.Left + bOK.Width + 30, exWidth), Math.Max(lbl.Top + lbl.Height + 100, exHeight));
                frm.ControlBox = false;
            }    
            else if (svinputmode == 5)
            {
                frm.Controls.AddRange(new Control[] { lbl, txt, bOK, bCancel });
                txt.Location = new System.Drawing.Point(lbl.Left + 5, lbl.Top + lbl.Height + 30);
                txt.Width = lbl.Width + 20;
                bOK.Location = new System.Drawing.Point(txt.Left + txt.Width + 10, lbl.Top);
                bCancel.Location = new System.Drawing.Point(txt.Left + txt.Width + 10, lbl.Top + bOK.Height + 10);
                int svw = bOK.Left + bOK.Width + 30;
                int svh = txt.Top + txt.Height + 50;
                frm.Size = new System.Drawing.Size(svw, svh);
                if (svw < 350) svw = 350;
                if (svh < 150) svh = 150;
                frm.Size = new System.Drawing.Size(svw, svh);
                txt.Top = frm.Height - 50 - txt.Height;
                txt.Width = frm.Width - bOK.Left - 15;
                bOK.Location = new System.Drawing.Point(frm.Width - bOK.Width - 30, lbl.Top);
                bCancel.Location = new System.Drawing.Point(frm.Width - bOK.Width - 30, lbl.Top + bOK.Height + 10);
            }
            else if (svinputmode == 99)
            {
                Label lbl2 = new Label();
                TextBox txt2 = new TextBox();
                if (svpassword == "") { txt2.PasswordChar = default(char); }
                else { txt2.PasswordChar = Convert.ToChar(svpassword); }
                Label lbl1 = new Label();
                TextBox txt1 = new TextBox();
                if (svpassword == "") { txt1.PasswordChar = default; }
                else { txt1.PasswordChar = Convert.ToChar(svpassword); }

                lbl2.AutoSize = false;
                lbl2.Text = "Input old password";
                lbl2.SetBounds(87, 39, 63, 15); lbl2.Font = new System.Drawing.Font("Arial", 9);
                txt2.SetBounds(lbl2.Left + lbl2.Width + 16, lbl2.Top, frm.Width - 35, 12);
                lbl1.AutoSize = false;
                lbl1.Text = "Input new password";
                lbl1.SetBounds(87, lbl2.Top + 39, 63, 15); lbl1.Font = new System.Drawing.Font("Arial", 9);
                txt1.SetBounds(lbl1.Left + lbl2.Width + 16, lbl1.Top, frm.Width - 35, 12);
                lbl.AutoSize = false;
                lbl.Text = "Confirm old password";
                lbl.SetBounds(87, lbl1.Top + 39, 63, 15); lbl.Font = new System.Drawing.Font("Arial", 9);
                txt.SetBounds(lbl.Left + lbl2.Width + 16, lbl.Top, frm.Width - 35, 12);

                frm.Size = new System.Drawing.Size(lbl.Left + 273, Math.Max(111 + lbl.Top, 55 + exHeight));
                txt2.Width = frm.Width - txt.Left - 36;
                txt1.Width = frm.Width - txt.Left - 36;
                txt.Width = frm.Width - txt.Left - 36;

                frm.Controls.AddRange(new Control[] { lbl2, txt2, lbl1, txt1, lbl, txt, bOK, bCancel });
                bOK.ForeColor = System.Drawing.Color.Black;
                bOK.BackColor = Control.DefaultBackColor;
                bOK.Location = new System.Drawing.Point(frm.Width - 298, frm.Height - 68);
                bCancel.Location = new System.Drawing.Point(frm.Width - 173, frm.Height - 68);
            }
            else if (svinputmode == 0)
            {
                //message only
                Panel pnl = new Panel();
                Label lblhead = new Label
                {
                    AutoSize = true,
                    Font = new System.Drawing.Font("Arial", 9)
                };
                lblhead.SetBounds(7, 13, 100, 13);
                lbl.Location = new System.Drawing.Point(27, 28);
                frm.Controls.AddRange(new Control[] { lbl, bOK, pnl, lblhead });
                lblhead.Text = "Total Number of Device ";// + Form1.nCtrlSystemCnt;
                lblhead.BringToFront();
                bOK.Size = new System.Drawing.Size(25, 25);
                if (lbl.Width > frm.Width) { frm.Width = lbl.Width + 50; }
                bOK.Location = new System.Drawing.Point(frm.Width - lblhead.Height - 12, 2);
                bOK.Text = "X";
                bOK.BackColor = System.Drawing.Color.Red;
                bOK.ForeColor = System.Drawing.Color.White;
                System.Drawing.Font fnt = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
                bOK.Font = fnt;

                frm.FormBorderStyle = FormBorderStyle.None;
                pnl.Dock = DockStyle.Fill;
                pnl.BorderStyle = BorderStyle.FixedSingle;
            }
            frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            bOK.Focus();
            frm.TopMost = true;

            DialogResult dialogResult = frm.ShowDialog();
            value = txt.Text;
            mvars.msgA = txt.Text;
            return dialogResult;
        }

        public static DialogResult msgBox(string title, string promptText, int exLeft, int exTop, int exWidth, int exHeight, byte svinputmode)
        {
            frm = new Form();
            Label lbl = new Label();
            TextBox txt = new TextBox();
            Button bOK = new Button();
            Button bCancel = new Button();

            if (title == "") title = mvars.strUInameMe;
            frm.Text = title;
            lbl.Text = promptText;

            bOK.Text = "OK";
            bCancel.Text = "Cancel";
            bOK.DialogResult = DialogResult.OK;
            bCancel.DialogResult = DialogResult.Cancel;

            if (exLeft == 0 && exTop == 0) { frm.StartPosition = FormStartPosition.CenterScreen; }
            else { frm.StartPosition = FormStartPosition.Manual; frm.Location = new System.Drawing.Point(exLeft, exTop); }
            frm.MinimizeBox = false;
            frm.MaximizeBox = false;
            frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            frm.AcceptButton = bOK;
            frm.CancelButton = bCancel;

            lbl.AutoSize = true;
            lbl.SetBounds(10, 10, 63, 15);
            lbl.Font = new System.Drawing.Font("Arial", 10);
            txt.Font = new System.Drawing.Font("Arial", 10);
            txt.SetBounds(lbl.Left + lbl.Width + 16, lbl.Top, frm.Width - 35, 12);
            frm.Size = new System.Drawing.Size(lbl.Left + 273, Math.Max(121 + lbl.Top, 55 + exHeight));
            txt.Width = frm.Width - txt.Left - 36;

            bOK.SetBounds(lbl.Width + (frm.Width - lbl.Width) / 2 + 5, 11, 66, 31); bOK.Font = new System.Drawing.Font("Arial", 9);
            bCancel.SetBounds(lbl.Width + (frm.Width - lbl.Width) / 2 + 5, 40, 66, 31); bCancel.Font = new System.Drawing.Font("Arial", 9);

            if (svinputmode == 1)
            {
                frm.Controls.AddRange(new Control[] { lbl });
                frm.Size = new System.Drawing.Size(lbl.Left + lbl.Width + 80, lbl.Top + lbl.Height * 2 + 80);
            }
            else if (svinputmode == 6)
            {
                frm.Controls.AddRange(new Control[] { lbl });
                frm.Size = new System.Drawing.Size(lbl.Left + lbl.Width + 80, lbl.Top + lbl.Height + 50);
            }
            else if (svinputmode == 2)
            {
                frm.Controls.AddRange(new Control[] { lbl });
                frm.Size = new System.Drawing.Size(lbl.Left + lbl.Width + 80, lbl.Top + lbl.Height * 2 + 80);
                frm.ControlBox = false;
            }
            else if (svinputmode == 3)
            {
                frm.Controls.AddRange(new Control[] { lbl, bOK });
                frm.Size = new System.Drawing.Size(lbl.Left + lbl.Width + 80, lbl.Top + lbl.Height * 2 + 80);
                bOK.Location = new System.Drawing.Point(frm.Width - bOK.Width - 40, frm.Height - bOK.Height - 50);
                bOK.Visible = true;
                frm.ControlBox = false;
            }
            else if (svinputmode == 4)
            {
                frm.Controls.AddRange(new Control[] { lbl, bOK, bCancel });
                frm.Size = new System.Drawing.Size(Math.Max(lbl.Left + lbl.Width + 80, exWidth), Math.Max(lbl.Top + lbl.Height * 2 + 80, exHeight));
                bCancel.Location = new System.Drawing.Point(frm.Width - bCancel.Width - 30, frm.Height - bOK.Height - 50);
                bCancel.Visible = true;
                bOK.Location = new System.Drawing.Point(frm.Width - bOK.Width - 30 - bCancel.Width - 10, frm.Height - bOK.Height - 50);
                bOK.Visible = true;
                frm.ControlBox = false;
            }
            else if (svinputmode == 5)
            {
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    bOK.Text = "Yes";
                    bCancel.Text = "No";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    bOK.Text = "是";
                    bCancel.Text = "否";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    bOK.Text = "是";
                    bCancel.Text = "否";
                }
                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                {
                    bOK.Text = "Yes";
                    bCancel.Text = "No";
                }


                frm.Controls.AddRange(new Control[] { lbl, bOK, bCancel });
                frm.Size = new System.Drawing.Size(Math.Max(lbl.Left + lbl.Width + 80, exWidth), Math.Max(lbl.Top + lbl.Height * 2 + 80, exHeight));
                bCancel.Location = new System.Drawing.Point(frm.Width - bCancel.Width - 30, frm.Height - bOK.Height - 50);
                bCancel.Visible = true;
                bOK.Location = new System.Drawing.Point(frm.Width - bOK.Width - 30 - bCancel.Width - 10, frm.Height - bOK.Height - 50);
                bOK.Visible = true;
                frm.ControlBox = false;
            }

            frm.TopMost = true;

            DialogResult dialogResult = frm.ShowDialog();
            return dialogResult;
        }

        public static DialogResult YesNoCancelBox(string title, string promptText, int exHeight)
        {
            Form frm = new Form();
            Label lbl = new Label();
            TextBox txt = new TextBox();
            Button bYes = new Button();
            Button bNo = new Button();
            Button bCancel = new Button();


            frm.Text = title;
            lbl.Text = promptText;

            if (MultiLanguage.DefaultLanguage == "en-US")
            {
                bYes.Text = "Yes";
                bNo.Text = "No";
                bCancel.Text = "Cancel";
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            {
                bYes.Text = "是";
                bNo.Text = "否";
                bCancel.Text = "取消";
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CN")
            {
                bYes.Text = "是";
                bNo.Text = "否";
                bCancel.Text = "取消";
            }

            bYes.DialogResult = DialogResult.Yes;
            bYes.Font = new System.Drawing.Font("Arial", 8);
            bNo.DialogResult = DialogResult.No;
            bNo.Font = new System.Drawing.Font("Arial", 8);
            bCancel.DialogResult = DialogResult.Cancel;
            bCancel.Font = new System.Drawing.Font("Arial", 8);
            bCancel.SetBounds(frm.Width - 6 - 60, frm.Height - 58, 50, 28);
            bNo.SetBounds(bCancel.Left - 6 - 60, frm.Height - 58, 50, 28);
            bYes.SetBounds(bNo.Left - 6 - 60, frm.Height - 58, 50, 28);
            frm.Controls.AddRange(new Control[] { lbl, bYes, bNo, bCancel });

            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new System.Drawing.Point(100, 100);
            lbl.SetBounds(30, 15, 400, exHeight);
            lbl.AutoSize = true;
            lbl.Font = new System.Drawing.Font("Arial", 10);
            //frm.ClientSize = new Size(lbl.Right + 50, Math.Max(113, 30 + exHeight));
            frm.Size = new System.Drawing.Size(Math.Max(lbl.Left + lbl.Width + lbl.Left, 300), Math.Max(exHeight, 30 + lbl.Top + lbl.Height + 80));

            bCancel.Location = new System.Drawing.Point(frm.Width - 30 - 60, frm.Height - 3 * bYes.Height);
            bNo.Location = new System.Drawing.Point(bCancel.Left - 6 - 60, frm.Height - 3 * bYes.Height);
            bYes.Location = new System.Drawing.Point(bNo.Left - 6 - 60, frm.Height - 3 * bYes.Height);

            frm.FormBorderStyle = FormBorderStyle.FixedDialog;
            frm.MinimizeBox = false;
            frm.MaximizeBox = false;
            bYes.BringToFront();
            bNo.BringToFront();
            bCancel.BringToFront();

            DialogResult dialogResult = frm.ShowDialog();
            return dialogResult;
        }

        public static string CommClose4SWRESET()
        {
            try { mvars.sp1.Close(); return "DONE"; }
            catch (Exception ex) { return "ERROR," + ex.Message; }
        }
        public static void CommPortSeek()
        {
            Form1.tslblStatus.Text = "";
            Form1.tslblCOM.Text = "";
            //檢查是否含有串口
            string[] str = SerialPort.GetPortNames();
            if (str.Length == 0) 
            { 
                //mvars.lstget.Items.Add("No Commport");
                Form1.lstget1.Items.Add("No Commport");
                Form1.tslblStatus.Text = "No Commport"; 
                mvars.Comm = null; return; 
            }
            if (mvars.sp1.IsOpen) { CommClose(); }
            //獲取有多少个串口
            string[] svcom = new string[str.Length];
            int j = 0;
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    SerialPort sp = new SerialPort(str[i]);
                    sp.RtsEnable = true;
                    sp.Open();
                    doDelayms(1);
                    sp.Close();
                    svcom[j]= str[i];
                    j++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (j > 0) 
            {
                Array.Resize(ref mvars.Comm, j);
                for (int i = 0; i < j; i++) mvars.Comm[i] = svcom[i];
                Form1.tslblCOM.Text = mvars.Comm.Length + " port"; 
                //mvars.lstget.Items.Add("COM seek result : " + mvars.Comm.Length);
            }
            else 
            {
                //mvars.lstget.Items.Add("No Commport"); 
                Form1.tslblStatus.Text = "No Commport"; 
                mvars.Comm = null; 
            }
        }


        public static void CommClose()
        {
            if (mvars.flgSendmessage == false)
            {
                try { mvars.sp1.Close(); }
                catch (Exception ex)
                {
                    //mvars.lstget.Items.Add(ex.Message);
                    Form1.lstget1.Items.Add(ex.Message);
                }
            }         
        }
        public static string Sp1open(string svcom)
        {
            try
            {
                if (mvars.sp1.IsOpen) { mvars.sp1.Close(); }
                //mvars.sp1.Close();
                mvars.sp1.PortName = svcom;
                mvars.sp1.RtsEnable = true;
                if (!mvars.sp1.IsOpen) { mvars.sp1.Open(); }
                return mvars.sp1.PortName + "：Open";
            }
            catch (Exception ex)
            {
                return "false," + ex.Message;
            }
        }
        public static void cmdHW(int R, int G, int B, out string svCaption)
        {
            Form1.tslblHW.Text = "Interface";
            Form1.tslblCOM.Text = "";
            if (Form1.dgvformmsg.Visible == false)
            {
                Form1.tslblHW.BackColor = Control.DefaultBackColor;
                Form1.tslblHW.ForeColor = System.Drawing.Color.Black;
            }
            if (Form1.cmbDevice != null) { Array.Clear(Form1.cmbDevice, 0, Form1.cmbDevice.Length); }
            #region 0709
            Form1.tslblStatus.Text = "";
            #endregion 0709
            if (R == 255 & G == 128 & B == 128) //485
            {
                string svcom = mp.FindComPort();
                if (svcom != "") { Form1.tslblHW.Text = "232"; Form1.tslblCOM.Text = svcom; }
                }
            else if (R == 255 & G == 192 & B == 128) //USB
            {
                if (SLUSBXpressDLL.hUSBDevice > 1) { SLUSBXpressDLL.SI_Close(SLUSBXpressDLL.hUSBDevice); }
                if (mp.GetDeviceList() == true)
                {
                    SLUSBXpressDLL.Status = SLUSBXpressDLL.SI_SetTimeouts(1500, 1000);
                    if (SLUSBXpressDLL.Status == SLUSBXpressDLL.SI_SUCCESS)
                    {
                        SLUSBXpressDLL.hUSBDevice = 1;
                        SLUSBXpressDLL.Status = SLUSBXpressDLL.SI_Open(0, ref SLUSBXpressDLL.hUSBDevice);
                        if (SLUSBXpressDLL.Status == SLUSBXpressDLL.SI_SUCCESS) { Form1.tslblHW.Text = "USB"; Form1.tslbldeviceID.Text = Form1.cmbDevice[0]; } else { Form1.tslblStatus.Text = "SI_Open Fail"; }
                    }
                    else { Form1.tslblStatus.Text = "SI_SetTimeouts Fail"; }
                }
                else { Form1.tslblStatus.Text = "No USB Device"; }
            }
            if (Form1.dgvformmsg.Visible == false && (Form1.tslblHW.Text == "232" || Form1.tslblHW.Text == "485" || Form1.tslblHW.Text == "USB"))
            {
                Form1.tslblHW.BackColor = System.Drawing.Color.Blue;
                Form1.tslblHW.ForeColor = System.Drawing.Color.White;
            }
            svCaption = Form1.tslblHW.Text;
        }

        public static bool GetDeviceList()
        {
            int DevNum = 0;
            StringBuilder DevStr = new StringBuilder(SLUSBXpressDLL.SI_MAX_DEVICE_STRLEN);
            int i;
            Array.Resize(ref Form1.cmbDevice, 0);
            SLUSBXpressDLL.Status = SLUSBXpressDLL.SI_GetNumDevices(ref DevNum);
            if (SLUSBXpressDLL.Status == SLUSBXpressDLL.SI_SUCCESS)
            {
                if (DevNum > 0) { Array.Resize(ref Form1.cmbDevice, DevNum); }
                for (i = 0; i < DevNum; i++)
                {
                    SLUSBXpressDLL.Status = SLUSBXpressDLL.SI_GetProductString(i, DevStr, SLUSBXpressDLL.SI_RETURN_SERIAL_NUMBER);
                    Form1.cmbDevice[i] = DevStr.ToString();
                }
            }
            if (Form1.cmbDevice.Length > 0) { return true; }
            else { return false; }
        }
        public static void funSaveLogs(string logMessage)
        {
            using (StreamWriter sw = File.AppendText(mvars.strLogPath + DateTime.Now.ToString("yyyyMMdd") + ".txt"))
            {
                if (logMessage.IndexOf("Load", 0) != -1) sw.WriteLine("\r\n");
                sw.Write(DateTime.Now.ToString("hh:mm:ss") + "  :"); sw.WriteLine($" {logMessage}");
            }
            //↓↑
        }











        public static void DiskStateDrive()
        {
            mvars.ListDrivesInfo = DriveInfo.GetDrives();
            if (mvars.ListDrivesInfo.Length > 0)
            {
                DriveInfo svdisk;
                for (byte svi = 0; svi < mvars.ListDrivesInfo.Length; svi++)
                {
                    svdisk = mvars.ListDrivesInfo[svi];
                    try
                    {
                        mvars.UUT.Disk[svi + 3, 0] = svdisk.DriveType.ToString();
                        mvars.UUT.Disk[svi + 3, 1] = svdisk.Name;
                        mvars.UUT.Disk[svi + 3, 2] = svdisk.VolumeLabel;
                        if (mvars.UUT.Disk[svi + 3, 0].ToUpper() == "FIXED") { mvars.UUT.extendDisk = Convert.ToByte(Convert.ToInt32(Convert.ToChar(svdisk.Name.Substring(0, 1)))); }
                    }
                    catch { mvars.UUT.Disk[svi + 3, 2] = "Not Exist"; }
                }
            }
            for (byte svi = 67; svi <= 65 + 25; svi++)
            {
                if (mvars.UUT.Disk[svi - 64, 0] != "" && mvars.UUT.Disk[svi - 64, 0] != null)
                {
                    if (mvars.UUT.existedDisk < svi) { mvars.UUT.existedDisk = svi; }
                    else
                    {
                        if (mvars.UUT.emptyDisk > 0)
                        {
                            if (svi - mvars.UUT.emptyDisk <= 1) { mvars.UUT.existedDisk = svi; }
                            else { break; }
                        }
                    }
                }
                else
                {
                    if (svi > mvars.UUT.existedDisk)
                    {
                        if (mvars.UUT.emptyDisk == 0) { mvars.UUT.emptyDisk = svi; }
                        else { break; }
                    }
                }
            }
            /*
            Console.WriteLine("磁碟代號:" + vListDrivesInfo.Name);
            Console.WriteLine("磁碟標籤:" + vListDrivesInfo.VolumeLabel);
            Console.WriteLine("磁碟類型:" + vListDrivesInfo.DriveType.ToString());
            Console.WriteLine("磁碟格式:" + vListDrivesInfo.DriveFormat);
            Console.WriteLine("磁碟大小:" + vListDrivesInfo.TotalSize.ToString());
            Console.WriteLine("剩餘空間:" + vListDrivesInfo.AvailableFreeSpace.ToString());
            Console.WriteLine("總剩餘空間(含磁碟配碟):" + vListDrivesInfo.TotalFreeSpace.ToString());
            Console.WriteLine("=================================");*/

        }
        public static bool ChecksumCheck(byte[] Buffer)
        {
            byte[] ReadSizeArr = { Buffer[4 + 1], Buffer[3 + 1] };
            UInt16 ReadSize = BitConverter.ToUInt16(ReadSizeArr, 0);
            byte[] ChecksumArr = { Buffer[ReadSize], Buffer[ReadSize - 1] };
            UInt16 Checksum = BitConverter.ToUInt16(ChecksumArr, 0);

            if (CalChecksum(Buffer, 1, (UInt16)(ReadSize - 2)) == Checksum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static unsafe void Copy(byte[] source, int sourceOffset, byte[] target, int targetOffset, int count)
        {
            // checked project --> create --> Enabled Unfafse code
            // If either array is not instantiated, you cannot complete the copy.
            if ((source == null) || (target == null))
            {
                throw new System.ArgumentException();
            }
            // If either offset, or the number of bytes to copy, is negative, you cannot complete the copy.
            if ((sourceOffset < 0) || (targetOffset < 0) || (count < 0))
            {
                throw new System.ArgumentException();
            }
            // If the number of bytes from the offset to the end of the array is 
            // less than the number of bytes you want to copy, you cannot complete
            // the copy. 
            if ((source.Length - sourceOffset < count) ||
                (target.Length - targetOffset < count))
            {
                throw new System.ArgumentException();
            }
            // The following fixed statement pins the location of the source and
            // target objects in memory so that they will not be moved by garbage
            // collection.
            fixed (byte* pSource = source, pTarget = target)
            {
                // Copy the specified number of bytes from source to target.
                for (int i = 0; i < count; i++)
                {
                    pTarget[targetOffset + i] = pSource[sourceOffset + i];
                }
            }
        }
        
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static UInt16 CalChecksum(byte[] arr, UInt16 index, UInt16 size)
        {
            UInt32 Checksum = 0;
            UInt16 i;
            for (i = index; i < (size + 1); i++)
            {
                Checksum += (UInt32)(arr[i]);
                if (Checksum > 65535) Checksum %= 65536;
                //{
                //    Checksum = Checksum % 65536;
                //}
            }
            return (UInt16)Checksum;
        }
        public static long CalCheckSum(byte[] bBuff, int iOffset, long iLen)
        {
            long SvCheckSum = 0;
            for (long i = iOffset; i < iLen; i++)
            {
                SvCheckSum += bBuff[i];
                //SvCheckSum = SvCheckSum % 65536;
                SvCheckSum %= 65536;
            }
            return SvCheckSum;
        }
        public static UInt16 CalChecksumIndex(byte[] arr, UInt32 IndexStart, UInt32 IndexEnd)
        {
            UInt32 Checksum = 0;
            UInt32 i;
            for (i = IndexStart; i <= IndexEnd; i++)
            {
                Checksum += (UInt32)(arr[i]);
                if (Checksum > 65535)
                {
                    Checksum %= 65536;
                }
            }
            return (UInt16)Checksum;
        }
        public static UInt32 CalChecksum32(byte[] arr, UInt32 index, UInt32 size)
        {
            UInt64 Checksum = 0;
            UInt32 i;
            for (i = index; i < (size + 1); i++)
            {
                Checksum += (UInt64)(arr[i]);
                if (Checksum > 0xFFFFFFFF)
                {
                    Checksum %= 0x100000000;
                }
            }

            return (UInt32)Checksum;
        }
        public static void FillOutBufferNu(byte[] Buffer)
        {
            //if (mvars.deviceID.Length == 0)
            //{
            //    Buffer[0 + mvars.svbytens] = 0xAB;    //Device ID 
            //    Buffer[1 + mvars.svbytens] = 0xCD;    //Device ID            }
            //}
            //else
            //{
            //    Buffer[0 + mvars.svbytens] = Convert.ToByte(mvars.deviceID.Substring(0, 2), 16);    //Device ID 
            //    Buffer[1 + mvars.svbytens] = Convert.ToByte(mvars.deviceID.Substring(2, 2), 16);    //Device ID
            //}
            //string svs = mvars.lblCmd;
            //byte[] WriteSizeArr = { Buffer[4 + mvars.svbytens], Buffer[3 + mvars.svbytens] };
            //UInt16 WriteSize = BitConverter.ToUInt16(WriteSizeArr, 0);
            ////Byte[] IP = StringToByteArray(mvars.sIP);
            //Buffer[WriteSize - 4 + mvars.svbytens] = 0x00;
            //Buffer[WriteSize - 3 + mvars.svbytens] = 0x01;
            //byte[] arr = BitConverter.GetBytes(CalChecksum(Buffer, mvars.svbytens, WriteSize));
            //Buffer[WriteSize - 2 + mvars.svbytens] = arr[1];  //Checksum
            //Buffer[WriteSize - 1 + mvars.svbytens] = arr[0];      //Checksum
        }

        public static void FillOutBuffer(byte[] Buffer)
        {
            Buffer[0] = 0x01;                //Report ID
            if (mvars.deviceID.Length == 0)
            {
                Buffer[0 + 1] = 0xAB;    //Device ID 
                Buffer[1 + 1] = 0xCD;    //Device ID            }
            }
            else
            {
                Buffer[0 + 1] = Convert.ToByte(mvars.deviceID.Substring(0, 2), 16);    //Device ID 
                Buffer[1 + 1] = Convert.ToByte(mvars.deviceID.Substring(2, 2), 16);    //Device ID
            }
            byte[] WriteSizeArr = { Buffer[4 + 1], Buffer[3 + 1] };
            UInt16 WriteSize = BitConverter.ToUInt16(WriteSizeArr, 0);
            //Byte[] IP = StringToByteArray(mvars.sIP);
            Buffer[WriteSize - 3] = 0x00;
            Buffer[WriteSize - 2] = 0x01;
            byte[] arr = BitConverter.GetBytes(CalChecksum(Buffer, 1, WriteSize));
            Buffer[WriteSize - 1] = arr[1];  //Checksum
            Buffer[WriteSize] = arr[0];      //Checksum
        }
        public static void Wait(int SvCountMax)
        {
            int Svi;

            //mvars.flgWaitout = false;
            if (SvCountMax > 32766 || SvCountMax < 0) { SvCountMax = 32766; }
            Svi = 1;
            do
            {
                Svi++;

                timeBeginPeriod(1);
                uint start = timeGetTime();
                do
                {
                    if ((timeGetTime() - start) >= 20) { break; }
                    System.Windows.Forms.Application.DoEvents();
                } while ((timeGetTime() - start) < 20);
                timeEndPeriod(1);

                System.Windows.Forms.Application.DoEvents();
                if (mvars.flgSend == false && mvars.flgReceived == false) { break; }
                if (mvars.Break) { break; }
                //break;
            } while (Svi < SvCountMax);
            mvars.flgSend = false; mvars.flgReceived = false; 
        }


        public static void doDelayms(int Svms)
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
        

        public static bool GetBin(string SvFlashFileNameFull)
        {
            //繼續,要分離給txt_hfile....
            //string[] SvStr = SvFlashFileNameFull.Split(',');
            //If Dir(txt_FlashFileNameFull.Text) = "" Or InStr(UCase(txt_FlashFileNameFull.Text), ".BIN") = 0 Then GoTo Ex
            if (File.Exists(SvFlashFileNameFull))   //判斷檔案存在
            {
                SvFlashFileNameFull = SvFlashFileNameFull.ToUpper();
                if (SvFlashFileNameFull.IndexOf(".BIN", 0) > 0)
                {

                    using (var fs = new FileStream(SvFlashFileNameFull, FileMode.Open))
                    {
                        Array.Resize(ref mvars.ucTmp, (int)fs.Length);
                        fs.Read(mvars.ucTmp, 0, (int)fs.Length);
                        //if (mvars.deviceID.Substring(0, 2) == "05")
                        //{
                        //    if (mvars.ucTmp.Length % (32 * 1024) != 0)
                        //    {
                        //        uint quotient = (uint)mvars.ucTmp.Length / (32 * 1024);
                        //        Array.Resize(ref mvars.ucTmp, (int)(quotient + 1) * 32 * 1024);
                        //    }
                        //}
                    }
                    /*
                    //存檔 txt格式
                    using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"D:\WriteLines2.txt"))
                    {
                        //int svi = 0;
                        //foreach (byte line in mvars.ucTmp)
                        //{
                        //    file.WriteLine(svi.ToString() + " " + line);
                        //    svi++;
                        //}
                        
                        string svs = "";
                        for (int svi=0;svi<Convert.ToInt32(frm1.lbl_FlashWindex1.Text);svi++)
                        {
                            for (int svj= (svi * 2048)+0; svj<(svi * 2048)+2048; svj++)
                            {
                                if ((svi * 2048) +2048 > mvars.ucTmp.Length){ break; }
                                svs = svs + " " + mvars.ucTmp[svj].ToString();
                            }
                            file.WriteLine(svs);
                            svs = "";
                        }
                    }                    */
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }

        public static void BinFilter_00_FF(ref byte[] BinFile)
        {
            UInt32 addr = 0;

            for (UInt32 i = 0; i < (UInt32)BinFile.Length; i++)
            {
                if (!(BinFile[i] == 0x00 || BinFile[i] == 0xFF))
                    addr = i;
            }
            Array.Resize(ref BinFile, (int)addr + 1);
        }


        //未使用
        public static void cFLASHWRITE_C12ABMP_mk2(string svpath)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            mvars.lCounts = 9999;
            Array.Resize(ref mvars.lCmd, mvars.lCounts);
            Array.Resize(ref mvars.lGet, mvars.lCounts);
            mvars.lCount = 0;
            mvars.flgDelFB = true;
            sw.Reset();
            sw.Start();

            string SvErr = "0"; mvars.errCode = "0";




            //int svpos = 16;
            //int svscans = 4;

            bool svnvbc = mvars.nvBoardcast;
            mvars.nvBoardcast = true;

            //    Form1.numUDSender.Value = Convert.ToByte(mvars.iSenderhead) + 1;
            //    Form1.numUDPort.Value = Convert.ToByte(mvars.iPorthead) + 1;
            //    Form1.numUDScan.Value = Convert.ToByte(mvars.iScanhead) + 1;
            //    mvars.iSender = Convert.ToByte(mvars.iSenderhead);
            //    mvars.iPort = Convert.ToByte(mvars.iPorthead);
            //    mvars.iScan = Convert.ToByte(mvars.iScanhead);

            //    int svre = 0;
            //reErase:

            //    mvars.lblCmd = "FLASH_TYPE";
            //    mhFLASHTYPE(uc_C12Ademura.svnova);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-3"; goto Ex; }
            //    mvars.lblCmd = "FLASH_FUNCQE";
            //    mhFUNCQE(uc_C12Ademura.svnova);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }
            //    mvars.lblCmd = "FUNC_ENABLE";
            //    mhFUNCENABLE(uc_C12Ademura.svnova);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4.5"; goto Ex; }
            //    mvars.lblCmd = "FUNC_STATUS";
            //    mhFUNCSTATUS(uc_C12Ademura.svnova);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-5"; goto Ex; }
            //    if (!((mvars.hFuncStatus & 0x02) == 0x02))
            //    {
            //        mvars.lstget.Items.Add(Form1.cmbhPB.Text + " FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02");
            //        mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
            //        SvErr = "-6"; goto Ex;
            //    }
            //    //flasherase
            //    sw1.Reset();
            //    sw1.Start();
            //    mvars.lblCmd = "FLASH_ERASE";
            //    mhFLASHERASE(uc_C12Ademura.svnova);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-7"; goto Ex; }
            //    mp.doDelayms(500);
            //    while (true)
            //    {
            //        mvars.lblCmd = "FUNC_STATUS";
            //        mhFUNCSTATUS(uc_C12Ademura.svnova);
            //        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-8"; break; }
            //        else
            //        {
            //            if (mp.DecToBin(mvars.hFuncStatus, 8).Substring(7, 1) == "0")
            //            {
            //                mvars.lstget.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ - 1] +
            //                        " Flash erasing... DONE," + string.Format("{0:0}", sw1.Elapsed.TotalSeconds) + "sec");
            //                mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
            //                break;
            //            }
            //            else
            //            {
            //                mvars.lstget.Items.Add(" -> Waiting for " + mvars.strFLASHtype[mvars.flashselQ - 1] +
            //                        " Flash erasing... " + string.Format("{0:0}", sw1.Elapsed.TotalSeconds) + "sec");

            //                if (sw1.Elapsed.TotalSeconds > 300)
            //                {
            //                    mvars.lstget.Items.Add(Form1.cmbhPB.Text + " FLASH_ERASE Timeout > 300 sec");
            //                    SvErr = "-9"; break;
            //                }
            //                mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
            //            }
            //            mp.doDelayms(1000);
            //        }
            //    }
            //    sw1.Stop();

            //    if (sw1.Elapsed.TotalSeconds < 19)
            //    {
            //        mvars.lstget.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ - 1] + " Flash erasing... < 19 sec");
            //        mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
            //        if (svre < 2) { svre++; goto reErase; }
            //        SvErr = "-10"; goto Ex;
            //    }

            //    if (SvErr != "0") goto Ex;


            //int svi = mvars.lstget.Items.Count; //檔案起始
            //if (File.Exists(svpath))
            //{
            //    StreamReader sTAread = File.OpenText(svpath);
            //    string[] svs = svpath.Split('\\');
            //    while (true)
            //    {
            //        string data = sTAread.ReadLine();
            //        if (data == null) { break; }
            //        mvars.lstget.Items.Add(data);
            //    }
            //    sTAread.Close();

            //    Form2.pathBoxID = mvars.lstget.Items[svi].ToString(); Form2.tslblpathboxid.Text = Form2.pathBoxID;
            //    int svrows = Convert.ToInt16(mvars.lstget.Items[svi + 1].ToString().Split('x')[0]);

            //    string[] dataBoxID = new string[svrows];

            //    svi = 1;
            //    do
            //    {
            //        dataBoxID[svi - 1] = mvars.lstget.Items[mvars.lstget.Items.Count - svi].ToString();
            //        svi++;
            //    }
            //    while (svi <= svrows);
            //}




            //int W = 0; ;
            //int H = 0;
            if (SvErr == "0")
            {
                mp.doDelayms(3000);

                int extX = 0;
                int extY = 0;
                mp.checkExtendScreen(ref extX, ref extY);
                if (mvars.FormShow[2]) { Form2.lbledid.Text = mvars.UUT.resW.ToString() + "X" + mvars.UUT.resH; }
                mvars.TuningArea.rX = Convert.ToSingle(Math.Round((float)mvars.UUT.resW / 384, 4, MidpointRounding.AwayFromZero));
                mvars.TuningArea.rY = Convert.ToSingle(Math.Round((float)mvars.UUT.resH / 216, 4, MidpointRounding.AwayFromZero));
                if (mp.upperBound == 0)
                {
                    //mvars.lstget.Items.RemoveAt(mvars.lstget.Items.Count - 1);
                    //mvars.lstget.Items.Add("No Extend Screen");
                    //mvars.lstget.Items.Add(" --> Please re-FLASHWRITE"); 
                    //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;

                    if (mvars.FormShow[2])
                    {
                        Form2.lbledidhead.Text = "Primary / EDID";
                        Form2.chkshowextend.Checked = false;
                    }
                    if (mvars.FormShow[5] == false)
                    {
                        //Form2.i3pat = new i3_Pat();
                        //Form2.i3pat.BackColor = Color.FromArgb(96, 96, 96);
                        //Form2.i3pat.Show();
                        //Form2.i3pat.Visible = false;
                        //Form2.i3pat.Location = new System.Drawing.Point(0, 0);
                        //Form2.i3pat.FormBorderStyle = FormBorderStyle.Sizable;
                        //Form2.i3pat.ControlBox = true;
                        //Form2.i3pat.MinimizeBox = false;
                        //Form2.i3pat.MaximizeBox = false;
                        //Form2.i3pat.Size = new Size(200, 200);
                        //Form2.i3pat.TopMost = true;
                        //Form2.i3pat.Visible = true;

                        Form2.i3pat = new i3_Pat
                        {
                            BackColor = System.Drawing.Color.FromArgb(96, 96, 96),
                            Visible = false,
                            Location = new System.Drawing.Point(0, 0),
                            FormBorderStyle = FormBorderStyle.Sizable,
                            ControlBox = true,
                            MinimizeBox = false,
                            MaximizeBox = false,
                            Size = new System.Drawing.Size(200, 200),
                            TopMost = true                          
                        };
                        Form2.i3pat.Visible = true;
                        Form2.i3pat.Show();
                    }
                    Form2.fileTuningArea(true);
                    Form2.lblexcorner.Text = extX + "," + extY + " " + mp.screens[mp.upperBound].DeviceName;
                    if (mp.screens[mp.upperBound].Primary) { Form2.lblexcorner.Text += " Primary"; }
                    if (Form2.i3pat.FormBorderStyle == FormBorderStyle.Sizable) { Form2.i3pat.Text = Form2.lblexcorner.Text; }
                }
                else
                {
                    if (mvars.FormShow[2])
                    {
                        Form2.lbledidhead.Text = "Extend / EDID";
                        Form2.chkshowextend.Checked = true;
                    }
                    //Form2.i3pat = new i3_Pat();
                    //Form2.i3pat.lbl_Mark.Visible = false;
                    //Form2.i3pat.BackColor = Color.Black;
                    //Form2.i3pat.Visible = false;
                    //Form2.i3pat.Location = new System.Drawing.Point(extX, extY);
                    //Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                    //Form2.i3pat.ControlBox = false;
                    //Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                    //Form2.i3pat.TopMost = true;
                    //Form2.i3pat.Visible = true;
                    //Form2.i3pat.Show();
                    if (mvars.FormShow[2])
                    {
                        Form2.lblexcorner.Text = extX + "," + extY + " " + mp.screens[mp.upperBound].DeviceName;
                        if (mp.screens[mp.upperBound].Primary) { Form2.lblexcorner.Text += " Primary"; }
                    }
                }
                




                Form2.bxw = 480;    //C12B
                Form2.bxh = 540;    //C12B
                System.Drawing.Bitmap bmpf = null;

                int svws = 1;
                int svhs = 1;
                if (mvars.UUT.resW > Form2.bxw) { svws = Convert.ToInt16(mvars.UUT.resW) / Form2.bxw; }
                if (mvars.UUT.resH > Form2.bxh) { svhs = Convert.ToInt16(mvars.UUT.resH) / Form2.bxh; }


                //Video programming
                if (mvars.FormShow[2]) { Form2.svpbf = new PictureBox[svhs, svws]; }
                if (mvars.FormShow[5]) { i3_Pat.addcontrol(svhs, svws, Form2.bxw, Form2.bxh); }
                int svn = 0; ;

                for (svn = 0; svn < (8 * 1024 * 1024) / (1024 * (Convert.ToInt16(Form2.bxh / 256) * 256)); svn++)
                {
                    if (mvars.FormShow[5]) Form2.i3pat.Close();

                    Form2.i3pat = new i3_Pat();
                    Form2.i3pat.lbl_Mark.Visible = false;
                    Form2.i3pat.BackColor = System.Drawing.Color.Black;
                    Form2.i3pat.Visible = false;
                    Form2.i3pat.Location = new System.Drawing.Point(extX, extY);
                    Form2.i3pat.FormBorderStyle = FormBorderStyle.None;
                    Form2.i3pat.ControlBox = false;
                    Form2.i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                    Form2.i3pat.TopMost = true;
                    Form2.i3pat.Visible = true;
                    Form2.i3pat.Show();

                    i3_Pat.addcontrol(2, 4, 480, 540);

                    //mvars.lstget.Items.Add("Write Counter：" + string.Format("{0:00}", svn + 1));
                    //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                    for (int svh = 0; svh < svhs; svh++)
                    {
                        for (int svw = 0; svw < svws; svw++)
                        {
                            mp.GetBin(Form2.pathBoxID + @"\" + Form2.dataBoxID[svh].Split(',')[svw] + @"\" + Form2.dataBoxID[svh].Split(',')[svw] + ".bin");
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\demura.bmp")) { File.Delete(mvars.strStartUpPath + @"\Parameter\demura.bmp"); }
                            mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\demura.bmp", ref svn, Form2.bxw, Form2.bxh, 1024, Convert.ToInt16(Form2.bxh / 256) * 256);
                            bmpf = new System.Drawing.Bitmap(mvars.strStartUpPath + @"\Parameter\demura.bmp");

                            System.Drawing.Bitmap bmps = null;
                            if (mvars.FormShow[2])
                            {
                                Form2.svpbf[svh, svw] = new System.Windows.Forms.PictureBox();
                                Form2.svpbf[svh, svw].SetBounds(Convert.ToInt16(svw * Form2.bxw / mvars.TuningArea.rX), Convert.ToInt16(svh * Form2.bxh / mvars.TuningArea.rY), Convert.ToInt16(Form2.bxw / mvars.TuningArea.rX), Convert.ToInt16(Form2.bxh / mvars.TuningArea.rY));
                                Form2.picfront.Controls.AddRange(new Control[] { Form2.svpbf[svh, svw] });
                                Form2.svpbf[svh, svw].Tag = "add";
                                Form2.svpbf[svh, svw].Visible = true;
                                //縮圖↓
                                int imageFromWidth = Form2.bxw;
                                int imageFromHeight = Form2.bxh;
                                System.Drawing.Image.GetThumbnailImageAbort callb = new System.Drawing.Image.GetThumbnailImageAbort(() => { return false; });
                                //呼叫Image物件自帶的GetThumbnailImage()進行圖片縮略
                                System.Drawing.Image reducedImage = bmpf.GetThumbnailImage(Form2.svpbf[svh, svw].Width, Form2.svpbf[svh, svw].Height, callb, IntPtr.Zero);
                                //將圖片以指定的格式儲存到到指定的位置
                                reducedImage.Save(mvars.strStartUpPath + @"\Parameter\demura_small.bmp");
                                bmps = new System.Drawing.Bitmap(mvars.strStartUpPath + @"\Parameter\demura_small.bmp");
                                //縮圖↑
                                Form2.svpbf[svh, svw].Image = bmps;
                                Form2.svpbf[svh, svw].Refresh();
                            }
                            if (mvars.FormShow[5])
                            {
                                i3_Pat.pb[svh, svw].Image = bmpf;
                                i3_Pat.pb[svh, svw].Visible = true;
                                i3_Pat.pb[svh, svw].Refresh();
                            }
                            bmps.Dispose();
                            bmpf.Dispose();
                        }
                    }
                    if (bmpf != null) bmpf.Dispose();
                }
            }
        }



        public static void CAmeasF()
        {
            if (mCAs.CAATG.CAsel == 0)
            {
                try
                {
                    doDelayms(100);
                    mCAs.objCa.Measure();
                    if (mCAs.CAATG.OverBet > 1)
                    {
                        CAFxLv = mCAs.objProbe.Lv * mCAs.CAATG.OverBet;
                        CAFxX = mCAs.objProbe.X * mCAs.CAATG.OverBet;
                        CAFxY = mCAs.objProbe.Y * mCAs.CAATG.OverBet;
                        CAFxZ = mCAs.objProbe.Z * mCAs.CAATG.OverBet;
                    }
                    else
                    {
                        CAFxLv = mCAs.objProbe.Lv;
                        CAFxX = mCAs.objProbe.X;
                        CAFxY = mCAs.objProbe.Y;
                        CAFxZ = mCAs.objProbe.Z;
                    }
                    CAFx = mCAs.objProbe.sx;
                    CAFy = mCAs.objProbe.sy;
                }
                catch { CAFxLv = -1; CAFx = 0; CAFy = 0; uc_atg.lstget.Items.Add(" --> " + mCAs.CAATG.Class + " Error Value @ CAmeasF"); }
            }
            else if (mCAs.CAATG.CAsel == 1)
            {
                double Lv = 0.0;
                double sx = 0.0;
                double sy = 0.0;
                double X = 0.0;
                double Y = 0.0;
                double Z = 0.0;
                //double JEITA = 0.0;
                //double FMA = 0.0;
                //double ud = 0.0;
                //double vd = 0.0;
                //double T = 0.0;
                string sverr = mCA4.geterrMsg(mCA4._objCa.Measure());
                if (sverr == "OK")
                {
                    mCA4.geterrMsg(mCA4._objProbe.get_Lv(ref Lv));
                    mCA4.geterrMsg(mCA4._objProbe.get_sx(ref sx));
                    mCA4.geterrMsg(mCA4._objProbe.get_sy(ref sy));
                    mCA4.geterrMsg(mCA4._objProbe.get_X(ref X));
                    mCA4.geterrMsg(mCA4._objProbe.get_Y(ref Y));
                    mCA4.geterrMsg(mCA4._objProbe.get_Z(ref Z));
                    if (mCAs.CAATG.OverBet > 1)
                    {
                        CAFxLv = (float)(Lv * mCAs.CAATG.OverBet);
                        CAFxX = (float)(X * mCAs.CAATG.OverBet);
                        CAFxY = (float)(Y * mCAs.CAATG.OverBet);
                        CAFxZ = (float)(Z * mCAs.CAATG.OverBet);
                    }
                    else
                    {
                        CAFxLv = (float)Lv;
                        CAFxX = (float)X;
                        CAFxY = (float)Y;
                        CAFxZ = (float)Z;
                    }
                    CAFx = (float)sx;
                    CAFy = (float)sy;
                }
                else
                {
                    CAFxLv = -1; CAFx = 0; CAFy = 0; uc_atg.lstget.Items.Add("-> " + mCAs.CAATG.Class + " Error Value @ CAmeasF");
                }
            }




            if (CAFxLv < 0.0009) strCAFxLv = CAFxLv.ToString("###0.0####");
            else if (CAFxLv >= 0.0009 && CAFxLv < 0.005) strCAFxLv = CAFxLv.ToString("###0.0###");
            else if (CAFxLv >= 0.0005 && CAFxLv < 1) strCAFxLv = CAFxLv.ToString("###0.0##");
            else if (CAFxLv >= 1 && CAFxLv < 10) strCAFxLv = CAFxLv.ToString("###0.0#");
            else if (CAFxLv >= 10 && CAFxLv < 100) strCAFxLv = CAFxLv.ToString("###0.#");
            else strCAFxLv = CAFxLv.ToString("###0");
            CAFxX = Convert.ToSingle(CAFxX.ToString("####0.0####"));
            CAFxY = Convert.ToSingle(CAFxY.ToString("####0.0####"));
            CAFxZ = Convert.ToSingle(CAFxZ.ToString("####0.0####"));
            strCAFx = CAFx.ToString("#0.000");
            strCAFy = CAFy.ToString("#0.000");

            CAFxLv = Convert.ToSingle(strCAFxLv);
            CAFx = Convert.ToSingle(strCAFx);
            CAFy = Convert.ToSingle(strCAFy);
        }

        public static void saveFS_mk6(string FileNameFull, ref int svcodeindex, int svbxW, int svbxH, int svfpgaW, int svfpgaH)
        {
            mp.doDelayms(100);

            //svbxW = 480; 
            //svfpgaW = 1024;
            //svbxH = 540;  
            //svfpgaH = 512;

            int svi;
            int svj;

            int svaccumulation = svcodeindex;
            svcodeindex = 0;

            byte[] svp = new byte[svbxW * 3 * svbxH + 54];
            byte[] svbin = new byte[svbxW * 3 * svbxH];
            for (svi = 0; svi < svp.Length; svi++) { svp[svi] = 0; }

            svp[0] = 66;
            svp[1] = 77;
            svp[2] = 54;
            int svn = (int)(svbxW * 3 * svbxH);
            //while (svn > 65536) { svn = (svn % 65536); }
            //while (svn > 256) { svn = svn / 256; }
            while (svn > 65536) { svn %= 65536; }
            while (svn > 256) { svn /= 256; }
            svp[3] = (byte)(svn % 256);

            svn = (int)(svbxW * 3 * svbxH);
            //while (svn > 65536) { svn = (svn / 65536); }
            while (svn > 65536) { svn /= 65536; }
            svp[4] = (byte)(svn);
            svp[10] = 54;
            svp[14] = 40;
            svp[18] = (byte)((svbxW) % 256);
            svp[19] = (byte)((svbxW) / 256);
            svp[22] = (byte)(svbxH % 256);
            svp[23] = (byte)(svbxH / 256);
            svp[26] = 1;
            svp[28] = 24;
            svp[35] = svp[3];
            svp[36] = svp[4];

            int svbmphead = 54;

            int sv1p = svbmphead + svbxW * 3 * svbxH;   //bmp total
            int svRcounter = 0;
            for (svj = 1; svj <= svbxH; svj++)
            {
                int svRst = sv1p - svj * svbxW * 3;
                int svBinst = (svj - 1) * svbxW * 3;
                int svchecksum = 0;
                int svw;
                for (svw = 0; svw < svbxW * 3; svw++)
                {
                    svp[svRst + svw] = Convert.ToByte(mvars.ucTmp[svcodeindex + svaccumulation]); svchecksum += svp[svRst + svw]; svchecksum %= 65536;
                    svbin[svBinst + svw] = Convert.ToByte(mvars.ucTmp[svcodeindex + svaccumulation]);

                    svp[svRst + svbxW * 3 - 2] = Convert.ToByte(svchecksum / 256);
                    svp[svRst + svbxW * 3 - 1] = Convert.ToByte(svchecksum % 256);
                    svbin[svBinst + svbxW * 3 - 2] = Convert.ToByte(svchecksum / 256);
                    svbin[svBinst + svbxW * 3 - 1] = Convert.ToByte(svchecksum % 256);

                    svcodeindex++;
                    if (svw >= svfpgaW) { break; }
                }
                //
                if (svp[svRst + svw] == 0) { svp[svRst + svw + 1] = 0x66; svbin[svBinst + svw + 1] = 0x66; } else { svp[svRst + svw + 1] = 0x88; svbin[svBinst + svw + 1] = 0x88; }
                if (svp[svRst + svbxW * 3 - 2] == 0) { svp[svRst + svbxW * 3 - 3] = 0x66; svbin[svBinst + svbxW * 3 - 3] = 0x66; } else { svp[svRst + svbxW * 3 - 3] = 0x88; svbin[svBinst + svbxW * 3 - 3] = 0x88; }

                //防錯特殊辨識
                svp[svRst + 1280] = 0x66; svp[svRst + 1281] = 0x88; svp[svRst + 1282] = 0x66; svp[svRst + 1283] = 0x88;
                svbin[svBinst + 1280] = 0x66; svbin[svBinst + 1281] = 0x88; svbin[svBinst + 1282] = 0x66; svbin[svBinst + 1283] = 0x88;

                //Reverse
                for (svw = 0; svw < svbxW * 3; svw++)
                {
                    if ((svRst + svw) % 3 == 0)
                    {
                        byte svo = svp[svRst + svw];
                        svp[svRst + svw] = svp[svRst + svw + 2];
                        svp[svRst + svw + 2] = svo;
                    }
                }
                svRcounter++;
                if (svRcounter >= svfpgaH) { break; }
            }


            File.WriteAllBytes(FileNameFull, svp);
            if (File.Exists(mvars.strStartUpPath + @"\Parameter\coding" + (svaccumulation / (svfpgaW * svfpgaH)).ToString() + ".bin")) { File.Delete(mvars.strStartUpPath + @"\Parameter\coding" + (svaccumulation / (svfpgaW * svfpgaH)).ToString() + ".bin"); }
            File.WriteAllBytes(mvars.strStartUpPath + @"\Parameter\coding" + (svaccumulation / (svfpgaW * svfpgaH)).ToString() + ".bin", svbin);
            mp.doDelayms(100);
            svcodeindex += svaccumulation;
        }

        public static bool SeekAGMA(string svc, int svi3bR, int svi3bG, int svi3bB, bool svshow, int svmkR, int svmkG, int svmkB, System.Drawing.Point svmkpt, System.Drawing.Size svmkSz)
        {
            if (mvars.TuningArea.Mark != "pg")
            {
                if (mvars.FormShow[5] == false) return false;
                else
                {
                    Form2.i3pat.lbl_Mark.Visible = svshow;
                    if (svc.ToUpper() == "X")
                    {
                        Form2.i3pat.BackColor = System.Drawing.Color.FromArgb(svi3bR, svi3bG, svi3bB);
                    }
                    else
                    {
                        if (svc.ToUpper() == "W") Form2.i3pat.BackColor = System.Drawing.Color.White;
                        else if (svc.ToUpper() == "R") Form2.i3pat.BackColor = System.Drawing.Color.Red;
                        else if (svc.ToUpper() == "G") Form2.i3pat.BackColor = System.Drawing.Color.Green;
                        else if (svc.ToUpper() == "B") Form2.i3pat.BackColor = System.Drawing.Color.Blue;
                        else if (svc.ToUpper() == "D") Form2.i3pat.BackColor = System.Drawing.Color.Black;
                        else if (svc.ToUpper() == "C") Form2.i3pat.BackColor = System.Drawing.Color.Cyan;
                        else if (svc.ToUpper() == "P") Form2.i3pat.BackColor = System.Drawing.Color.Purple;
                        else if (svc.ToUpper() == "Y") Form2.i3pat.BackColor = System.Drawing.Color.Yellow;
                        else if (svc.ToUpper() == "O") Form2.i3pat.BackColor = System.Drawing.Color.Orange;
                        else if (svc.ToUpper() == "M") Form2.i3pat.BackColor = System.Drawing.Color.Magenta;
                        //Form2.i3pat.lbl_Mark.BackColor = Form2.i3pat.BackColor;
                    }
                    Form2.i3pat.lbl_Mark.Location = svmkpt;
                    Form2.i3pat.lbl_Mark.Size = svmkSz;
                    Form2.i3pat.lbl_Mark.BackColor = System.Drawing.Color.FromArgb(svmkR, svmkG, svmkB);
                    Application.DoEvents();
                    doDelayms(300);
                    return true;
                }
            }
            else
            {
                if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04")
                {
                    int svXs = svmkpt.X;
                    int svYs = svmkpt.Y;

                    Form1.pvindex = 108;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svXs);                // 108 X_START
                    Form1.pvindex = 109;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svXs + 120 - 1);      // 109 X_END
                    Form1.pvindex = 110;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svYs);                // 110 Y_START
                    Form1.pvindex = 111;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svYs + 135 - 1);      // 111 Y_END

                    Form1.pvindex = 105;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE((svmkR * 4));         // 105 GRAY_R
                    Form1.pvindex = 106;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE((svmkG * 4));         // 106 GRAY_G
                    Form1.pvindex = 107;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE((svmkB * 4));         // 107 GRAY_B

                    //Form1.pvindex = 255;
                    //mvars.lblCmd = "FPGA_SPI_W255";
                    //mp.mhFPGASPIWRITE(0);
                    //mvars.lblCmd = "FPGA_SPI_W255";
                    //mp.mhFPGASPIWRITE(1);
                    //mvars.lblCmd = "FPGA_SPI_W255";
                    //mp.mhFPGASPIWRITE(0);

                    doDelayms(500);
                    if (mvars.deviceID.Substring(0, 2) == "04") { doDelayms(200); }
                    return true;
                }
                else if (mvars.deviceID.Substring(0, 2) == "06")
                {
                    //int[] svreg = new int[] { mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B };
                    //int[] svdata = new int[] { svmkpt.X, svmkpt.X + svmkSz.Width - 1, svmkpt.Y, svmkpt.Y + svmkSz.Height - 1, svmkR * 4, svmkG * 4, svmkB * 4 };
                    //mvars.lblCmd = "FPGA_SPI_W";
                    //mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);

                    //int svXs = svmkpt.X;
                    //int svYs = svmkpt.Y;
                    Form1.pvindex = 108;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkpt.X);                     // 108 X_START
                    Form1.pvindex = 109;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkpt.X + svmkSz.Width - 1);  // 109 X_END
                    Form1.pvindex = 110;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkpt.Y);                     // 110 Y_START
                    Form1.pvindex = 111;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkpt.Y + svmkSz.Height - 1); // 111 Y_END
                    Form1.pvindex = 105;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkR * 4);                // 105 GRAY_R
                    Form1.pvindex = 106;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkG * 4);                // 106 GRAY_G
                    Form1.pvindex = 107;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkB * 4);                // 107 GRAY_B
                    doDelayms(500);
                    return true;
                }
                else if (mvars.deviceID.Substring(0, 2) == "10")
                {
                    Form1.pvindex = mvars.FPGA_GRAY_R;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svmkR * 4);    // 38 GRAY_R
                    Form1.pvindex = mvars.FPGA_GRAY_G;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svmkG * 4);    // 39 GRAY_G
                    Form1.pvindex = mvars.FPGA_GRAY_B;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svmkB * 4);    // 40 GRAY_B
                    doDelayms(500);
                    return true;
                }
                else { return false; }
            }
        }
        public static bool ShowAGMAnT(string svc, int svi3br, int svi3bg, int svi3bb, bool svshow, int svmkbr, int svmkbg, int svmkbb, bool svshowmark, int svms)
        {
            if (mvars.demoMode) { return true; }
            if (svms < 100) { svms = 100; }
            else if (svms > 500) { svms = 500; }
            if (mvars.TuningArea.Mark != "pg")
            {
                if (mvars.FormShow[5] == false) return false;
                else
                {
                    Form2.i3pat.lbl_Mark.Visible = svshow;
                    if (svc.ToUpper() == "X")
                    {
                        Form2.i3pat.BackColor = System.Drawing.Color.FromArgb(svi3br, svi3bg, svi3bb);
                    }
                    else
                    {
                        if (svc.ToUpper() == "W") Form2.i3pat.BackColor = System.Drawing.Color.White;
                        else if (svc.ToUpper() == "R") Form2.i3pat.BackColor = System.Drawing.Color.Red;
                        else if (svc.ToUpper() == "G") Form2.i3pat.BackColor = System.Drawing.Color.Green;
                        else if (svc.ToUpper() == "B") Form2.i3pat.BackColor = System.Drawing.Color.Blue;
                        else if (svc.ToUpper() == "D") Form2.i3pat.BackColor = System.Drawing.Color.Black;
                        else if (svc.ToUpper() == "C") Form2.i3pat.BackColor = System.Drawing.Color.Cyan;
                        else if (svc.ToUpper() == "P") Form2.i3pat.BackColor = System.Drawing.Color.Purple;
                        else if (svc.ToUpper() == "Y") Form2.i3pat.BackColor = System.Drawing.Color.Yellow;
                        else if (svc.ToUpper() == "O") Form2.i3pat.BackColor = System.Drawing.Color.Orange;
                        else if (svc.ToUpper() == "M") Form2.i3pat.BackColor = System.Drawing.Color.Magenta;
                    }
                    Form2.i3pat.lbl_Mark.BackColor = System.Drawing.Color.FromArgb(svmkbr, svmkbg, svmkbb);
                    Application.DoEvents();
                    doDelayms(svms);
                    return true;
                }
            }
            else
            {
                if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04")
                {
                    Form1.pvindex = mvars.FPGA_GRAY_R;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE((svmkbr * 4));    // 105 GRAY_R
                    Form1.pvindex = mvars.FPGA_GRAY_G;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE((svmkbg * 4));    // 106 GRAY_G
                    Form1.pvindex = mvars.FPGA_GRAY_B;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE((svmkbb * 4));    // 107 GRAY_B
                    //Form1.pvindex = 255;
                    //mvars.lblCmd = "FPGA_SPI_W255";
                    //mp.mhFPGASPIWRITE(0); 
                    //Form1.pvindex = 255;
                    //mvars.lblCmd = "FPGA_SPI_W255";
                    //mp.mhFPGASPIWRITE(1); 
                    //Form1.pvindex = 255;
                    //mvars.lblCmd = "FPGA_SPI_W255";
                    //mp.mhFPGASPIWRITE(0);
                    doDelayms(svms);
                    return true;
                }
                else if (mvars.deviceID.Substring(0, 2) == "06")
                {
                    Form1.pvindex = mvars.FPGA_GRAY_R;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkbr * 4);    // 105 GRAY_R
                    Form1.pvindex = mvars.FPGA_GRAY_G;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkbg * 4);    // 106 GRAY_G
                    Form1.pvindex = mvars.FPGA_GRAY_B;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svmkbb * 4);    // 107 GRAY_B
                    doDelayms(svms);
                    return true;
                }
                else if (mvars.deviceID.Substring(0, 2) == "10")
                {
                    //Form1.pvindex = mvars.FPGA_PT_BANK;
                    //mvars.lblCmd = "FPGA_SPI_W";
                    //mp.mhFPGASPIWRITE(3);    
                    Form1.pvindex = mvars.FPGA_GRAY_R;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svmkbr * 4);    // 38 GRAY_R
                    Form1.pvindex = mvars.FPGA_GRAY_G;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svmkbg * 4);    // 39 GRAY_G
                    Form1.pvindex = mvars.FPGA_GRAY_B;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(svmkbb * 4);    // 40 GRAY_B
                    doDelayms(svms);
                    return true;
                }
                else { return false; }
            }
        }
        public static void mhcm603WriteMulti(byte svAddr, byte svIc, byte ucReg, byte ucSize) //最新混合體 AutoGamm中更改了Gamma_603,Vref_603
        {
            int Svi = 0;
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + (0x0C + ucSize + 1));
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;         //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = (byte)((0x0C + ucSize + 1) / 256);    //Size(Datacount)
            mvars.RS485_WriteDataBuffer[4 + svns] = (byte)((0x0C + ucSize + 1) % 256);    //Size(Datacount)
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svAddr / 2);        //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                      //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = (byte)(ucSize + 1);        //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = (byte)(ucReg);             //Register 0x02~3E
            for (int i = ucReg; i < (ucReg + ucSize); i++)
            {
                if (mvars.cm603exp[(ucReg + i) / 2].ToUpper().IndexOf("RESERVED", 0) == -1 && mvars.cm603exp[(ucReg + i) / 2].ToUpper().IndexOf("CMI", 0) == -1 && mvars.cm603exp[(ucReg + i) / 2].ToUpper().IndexOf("MTP", 0) == -1)
                {
                    if (Svi % 2 == 0)    //HiByte + 0x4000 || 0x8000
                    {
                        if (mvars.UUT.MTP == 1) { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = (byte)(mp.HexToDec(mvars.cm603df[svIc, ucReg + i]) | 0x40); }
                        else { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = (byte)(mp.HexToDec(mvars.cm603df[svIc, ucReg + i]) | 0x80); }
                    }
                    else { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = (byte)mp.HexToDec(mvars.cm603df[svIc, ucReg + i]); }

                    //Svi++;
                }
                else
                {
                    if (Svi % 2 == 0)    //HiByte + 0x4000 || 0x8000
                    {
                        if (mvars.UUT.MTP == 1) { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = (byte)(0x00 | 0x40); }
                        else { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = (byte)(0x00 | 0x80); }
                    }
                    else { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = 0x00; }
                    //Svi++;
                }
                Svi++;
            }
            mp.funSendMessageTo();
        }
        public static void funSendMessage()
        {
            int hWnd = Form1.FindWindow(null, @"WT_UI");
            if (hWnd == 0)
            {
                mp.funSaveLogs("(E) No WT_UI");
                //mvars.tmePull.Enabled = true;
                //return "No WT_UI";
            }
            else
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes("DONE");
                Form1.COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)2500;
                cds.cbData = sarr.Length;
                string message = mvars.lCmd[0] + mvars.lGet[0];

                //mvars.lstget.Items.Clear();
                //mvars.lstget.Items.Add(message); 
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                
                if (mvars.lCounts > 1)
                {
                    for (int svi = 1; svi < mvars.lCounts; svi++) { message = mvars.lCmd[svi] + mvars.lGet[svi]; }
                }
                if (message.IndexOf("ERROR", 0) > -1) { cds.lpData = mvars.handleIDMe.ToString() + "@@ERROR," + mvars.strUInameMe + "," + message; }
                else
                {
                    if (mvars.lblCompose == "") { cds.lpData = mvars.handleIDMe.ToString() + "@@" + mvars.strUInameMe + "," + message; }
                    else { cds.lpData = mvars.handleIDMe.ToString() + "@@" + mvars.strUInameMe + "," + mvars.lblCompose + ",DONE"; }
                }
                mp.funSaveLogs("(S) " + message);
                Form1.SendMessage(hWnd, Form1.WM_COPYDATA, 0, ref cds);
                mvars.lblCompose = "";
                //mvars.tmePull.Enabled = true;
            }
            mvars.flgSelf = true;
            mvars.tmePull.Enabled = true;
        }


        public static string USB_Error_Handler(string sErrCode)
        {
            switch (sErrCode)
            {
                //
                case "00A2": return "ERROR_TX_PACKET_TIMEOUT";
                //FPGA
                case "1001": return "FPGA DONE";
                //I2C
                case "2004": return "ERROR_I2C_TIMEOUT";
                case "2005": return "ERROR_I2C_SDA_SCL_LOW";
                case "2006": return "ERROR_I2C_NACK";
                case "2007": return "ERROR_I2C_ERR_BUS";
                case "2008": return "ERROR_I2C_CLKHOLD";
                case "2010": return "ERROR_I2C_ADDR_NACK";
                case "2011": return "ERROR_I2C_DATA_NACK";
                case "2012": return "ERROR_I2C_SCL_LOW";
                case "2013": return "ERROR_I2C_SDA_LOW";
                case "2014": return "ERROR_I2C_BUS_LOW";
                //SPI
                case "3010": return "SPI_FLASH_STATUS_BUSY";
                case "3011": return "SPI_FLASH_STATUS_ERROR_UNKNOWN";
                case "3012": return "SPI_FLASH_WE_ERROR_UNKNOWN";
                case "3013": return "SPI_FLASH_256_ERROR";
                case "3014": return "SPI_FLASH_WR_CHK_TIMEOUT";
                case "301F": return "SPI_FLASH_ERROR";
                case "3020": return "SPI_ERROR_CODE_BUSY";
                case "3021": return "SPI_ERROR_CODE_256";
                case "3022": return "SPI_ERROR_CODE_WR_TIMEOUT";
                case "3023": return "SPI_ERROR_CODE_WE";
                case "3024": return "SPI_ERROR_CODE_SECTOR_ERASE";
                case "3025": return "SPI_ERROR_CODE_BUSY_TIMEOUT";
                case "3026": return "SPI_ERROR_CODE_MEM_CMP";
                //QSPI
                case "3100": return "QSPI_ERROR_CODE_BUSY";
                case "3101": return "QSPI_ERROR_CODE_256";
                case "3102": return "QSPI_ERROR_CODE_WR_TIMEOUT";
                case "3103": return "QSPI_ERROR_CODE_WE";
                case "3104": return "QSPI_ERROR_CODE_SECTOR_ERASE";
                case "3105": return "QSPI_ERROR_CODE_BUSY_TIMEOUT";
                case "3106": return "QSPI_ERROR_CODE_MEM_CMP";
                case "31FF": return "QSPI_ERROR_CODE_UNKNOWN";
                //MCU Flash
                case "4001": return "MCU_FLASH_ADDR_ERROR";
                case "4002": return "MCU_FLASH_CMP_ERROR";

                default: return "Inpub Buffer Return NG";
            }
        }
        public static string WriteReadMcuTask(byte[] OUTBuffer, ref uint BytesWritten, byte[] INBuffer, ref uint BytesRead)
        {
            uint PacketSize = Convert.ToUInt16(OUTBuffer[3 + 1] * 256 + OUTBuffer[4 + 1]);
            try
            {
                if (mvars.flgSendmessage == false)
                {
                    mvars.sp1.Close();
                    if (!mvars.sp1.IsOpen) { mvars.sp1.Open(); }
                }

                if (PacketSize > 512)
                {
                    byte[] arr = new byte[513];
                    arr[0] = 0; //Report ID
                    for (UInt16 i = 0; i < (PacketSize / 512 + 1); i++)
                    {
                        Copy(OUTBuffer, 512 * i + 1, arr, 1, 512);
                        mvars.sp1.Write(arr, 1, 512);
                    }
                }
                else { mvars.sp1.Write(OUTBuffer, 1, 512); }
            }
            catch (Exception ex)
            {
                CommClose();
                return "false," + ex.Message;
            }
            if (mvars.lblCmd == "MCU_RESET") { string svs = CommClose4SWRESET(); return "true," + svs; }
            else   //Read From MCU
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Reset();
                sw.Start();
                int Length;
                int ReceiveSize = 1;
                while (sw.Elapsed.TotalMilliseconds < 1000)
                {
                    if (mvars.sp1.BytesToRead > 0)
                    {

                        if (mvars.sp1.BytesToRead > INBuffer.Length) { CommClose(); return "false,BytesToRead > INBuffer length"; }

                        Length = mvars.sp1.Read(INBuffer, ReceiveSize, mvars.sp1.BytesToRead);
                        ReceiveSize += Length;
                        PacketSize = Convert.ToUInt16(INBuffer[3 + 1] * 256 + INBuffer[4 + 1]);
                        if ((ReceiveSize - 1) == PacketSize)
                            break;
                    }
                }

                if (sw.Elapsed.TotalMilliseconds >= 1000)
                {
                    //if (mvars.lblCmd != "MCU_RESET") { CommClose(); return "false,TimeOut"; }
                    //else { string svs = CommClose4SWRESET(); return "true," + svs ; }
                    CommClose(); return "false,TimeOut";
                }
                else
                {
                    if (mp.ChecksumCheck(INBuffer) == true)
                    {
                        if (INBuffer[5 + 1] == mvars.ReturnOK) { CommClose(); return "true"; }
                        else
                        {
                            CommClose();
                            return "false,Inpub Buffer Return NG";
                        }
                    }
                    else
                    {
                        CommClose();
                        return "false,Inpub Buffer Checksum Error";
                    }
                }
            }
        }

        //public static string WriteReadMcuTaskNu(byte[] OUTBuffer, ref uint BytesWritten, byte[] INBuffer, ref uint BytesRead)
        //{
        //    uint PacketSize = Convert.ToUInt16(OUTBuffer[3 + mvars.svbytens] * 256 + OUTBuffer[4 + mvars.svbytens]);
        //    try
        //    {
        //        if (mvars.flgSendmessage == false)
        //        {
        //            mvars.sp1.Close();
        //            if (!mvars.sp1.IsOpen) { mvars.sp1.Open(); }
        //        }

        //        if (PacketSize > 512)
        //        {
        //            byte[] arr = new byte[513];
        //            arr[0] = 0; //Report ID
        //            for (UInt16 i = 0; i < (PacketSize / 512 + 1); i++)
        //            {
        //                Copy(OUTBuffer, 512 * i + 1, arr, 1, 512);
        //                mvars.sp1.Write(arr, mvars.svbytens, 512);
        //            }
        //        }
        //        else { mvars.sp1.Write(OUTBuffer, mvars.svbytens, 512); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommClose();
        //        return "false," + ex.Message;
        //    }
        //    if (mvars.lblCmd == "MCU_RESET") { string svs = CommClose4SWRESET(); return "true," + svs; }
        //    else   //Read From MCU
        //    {
        //        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        //        sw.Reset();
        //        sw.Start();
        //        int Length;
        //        int ReceiveSize = 1;
        //        while (sw.Elapsed.TotalMilliseconds < 1000)
        //        {
        //            if (mvars.sp1.BytesToRead > 0)
        //            {

        //                if (mvars.sp1.BytesToRead > INBuffer.Length) { CommClose(); return "false,BytesToRead > INBuffer length"; }

        //                Length = mvars.sp1.Read(INBuffer, ReceiveSize, mvars.sp1.BytesToRead);
        //                ReceiveSize += Length;
        //                PacketSize = Convert.ToUInt16(INBuffer[4] * 256 + INBuffer[5]);
        //                if ((ReceiveSize - 1) == PacketSize)
        //                    break;
        //            }
        //        }

        //        if (sw.Elapsed.TotalMilliseconds >= 1000)
        //        {
        //            //if (mvars.lblCmd != "MCU_RESET") { CommClose(); return "false,TimeOut"; }
        //            //else { string svs = CommClose4SWRESET(); return "true," + svs ; }
        //            CommClose(); return "false,TimeOut";
        //        }
        //        else
        //        {
        //            if (mp.ChecksumCheck(INBuffer) == true)
        //            {
        //                if (INBuffer[5 + 1] == mvars.ReturnOK) { CommClose(); return "true"; }
        //                else
        //                {
        //                    CommClose();
        //                    return "false,Inpub Buffer Return NG";
        //                }
        //            }
        //            else
        //            {
        //                CommClose();
        //                return "false,Inpub Buffer Checksum Error";
        //            }
        //        }
        //    }
        //}


        public static string WriteReadMcuTask(byte[] OUTBuffer, byte[] INBuffer)
        {
            uint PacketSize = Convert.ToUInt16(OUTBuffer[3 + 1] * 256 + OUTBuffer[4 + 1]);

            //Write To MCU
            try
            {
                if (mvars.flgSendmessage == false)
                {
                    mvars.sp1.Close();
                    if (!mvars.sp1.IsOpen) { mvars.sp1.Open(); }
                }

                if (PacketSize > 64)
                {
                    byte[] arr = new byte[65];
                    arr[0] = 0; //Report ID
#if false
                    for (UInt16 i = 0; i < (PacketSize / 64 + 1); i++)
                    {
                        Copy(OUTBuffer, 64 * i + 1, arr, 1, 64);
                        sp.Write(arr, 1, 64);
                    }
#else
                    UInt16 i;
                    for (i = 0; i < (PacketSize / 64); i++)
                    {
                        Copy(OUTBuffer, 64 * i + 1, arr, 1, 64);
                        mvars.sp1.Write(arr, 1, 64);
                    }
                    Copy(OUTBuffer, 64 * i + 1, arr, 1, 64);
                    mvars.sp1.Write(arr, 1, (int)PacketSize % 64);
#endif
                }
                else
#if false
                    mvars.sp1.Write(OUTBuffer, 1, 64);
#else
                    mvars.sp1.Write(OUTBuffer, 1, (int)PacketSize);
#endif
            }
            catch (Exception ex)
            {
                 CommClose();
                return "false," + ex.Message;
            }

            //Read From MCU
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Reset();
            sw.Start();
            int Length;
            int ReceiveSize = 1;
            while (sw.Elapsed.TotalMilliseconds < 1000)
            {
                try
                {
                    if (mvars.sp1.BytesToRead > 0)
                    {
                        //Length = serialPort1.Read(INBuffer, ReceiveSize, INBuffer.Length - 1);
                        Length = mvars.sp1.Read(INBuffer, ReceiveSize, mvars.sp1.BytesToRead);
                        ReceiveSize += Length;
                        PacketSize = Convert.ToUInt16(INBuffer[3 + 1] * 256 + INBuffer[4 + 1]);
                        if ((ReceiveSize - 1) == PacketSize)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    CommClose();
                    return "false," + ex.Message;
                }
            }
            if (sw.Elapsed.TotalMilliseconds >= 1000)
            {
                CommClose(); return "false,TimeOut";
            }
            else
            {
                if (ChecksumCheck(INBuffer) == true)
                {
                    if (INBuffer[5 + 1] == mvars.ReturnOK) { return "true"; }
                    else
                    {
                        CommClose();
                        Byte[] tmp = { INBuffer[6 + 1], INBuffer[7 + 1] };
                        string sErrCode = BitConverter.ToString(tmp).Replace("-", "");
                        return "false," + USB_Error_Handler(sErrCode);
                    }
                }
                else { CommClose(); return "false,Inpub Buffer Checksum Error"; }
            }
        }


        public static void funSendMessageTo()
        {
            string strTemp = "";
            string Svs;
            int Svi;
            Byte[] tmp = null;
            if (mvars.svnova == false)
            {
                Wait(0);    //1227 重新啟用


                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) FillOutBufferNu(mvars.RS485_WriteDataBuffer);
                //else FillOutBuffer(mvars.RS485_WriteDataBuffer);
                FillOutBuffer(mvars.RS485_WriteDataBuffer);



                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";

                uint BytesWritten = 0, BytesRead = 0;
                Svi = 0;
                mvars.flgSend = true; mvars.flgReceived = false;
                string WriteStatus = "";
                if (mvars.demoMode)
                {
                    mvars.ReadDataBuffer[5 + 1] = 3;
                    mvars.ReadDataBuffer[6 + 1] = 0;
                    mvars.ReadDataBuffer[7 + 1] = 0;
                    mvars.ReadDataBuffer[8 + 1] = 0;
                    mvars.ReadDataBuffer[9 + 1] = 0;
                }
                else
                {
                    if (mvars.deviceID.Substring(0, 2) == "04")
                        WriteStatus = WriteReadMcuTask(mvars.RS485_WriteDataBuffer, mvars.ReadDataBuffer);
                    else if (mvars.deviceID.Substring(0, 2) == "02"
                          || mvars.deviceID.Substring(0, 2) == "03"
                          || mvars.deviceID.Substring(0, 2) == "05"
                          || mvars.deviceID.Substring(0, 2) == "06"
                          || mvars.deviceID.Substring(0, 2) == "10")
                        WriteStatus = mp.WriteReadMcuTask(mvars.RS485_WriteDataBuffer, ref BytesWritten, mvars.ReadDataBuffer, ref BytesRead);
                    //else
                    //    WriteStatus = mp.WriteReadMcuTaskNu(mvars.RS485_WriteDataBuffer, ref BytesWritten, mvars.ReadDataBuffer, ref BytesRead);

                }
                Svs = "-1";
                if (WriteStatus.IndexOf("true", 0) > -1)
                {
                    if (mvars.ReadDataBuffer[5 + 1] == 3)
                    {
                        switch (mvars.lblCmd)
                        {
                            case "READ_JEDECID":
                                #region READ_JEDID
                                {
                                    mvars.flashJEDECID = "-1";
                                    tmp = new byte[] { mvars.ReadDataBuffer[6 + 1], mvars.ReadDataBuffer[7 + 1], mvars.ReadDataBuffer[8 + 1] };
                                    mvars.flashJEDECID = BitConverter.ToString(tmp).Replace("-", "");
                                    Svs = mvars.flashJEDECID;
                                }
                                #endregion
                                break;
                            case "UI_FPGAALG_W":
                                Svs = "DONE";
                                break;
                            case "LT8668_Bin_WrRd":
                                #region LT8668
                                for (Svi = 0; Svi < uc_box.LT8668rd_arr.Length; Svi++)
                                    Svs += "," + Byte2HexString(mvars.ReadDataBuffer[6 + 1 + Svi]);
                                Svs = Svs.Substring(1, Svs.Length - 1);
                                break;
                            #endregion LT8668
                            case "FPGA_SPI_W":
                                #region FPGA_SPI_W
                                Svs = "DONE";
                                //if (mvars.FormShow[8])
                                //{
                                //    if (Form1.pvindex == 32767)
                                //    {
                                //        mvars.lblFPGAtxtf.BackColor = Color.White;
                                //        if (mvars.lblFPGAkwf.BackColor == Control.DefaultBackColor) { mvars.lblFPGAkwf.ForeColor = Color.Cyan; }
                                //        else if (mvars.lblFPGAkwf.BackColor == Color.Cyan) { mvars.lblFPGAkwf.ForeColor = Color.White; }
                                //    }
                                //    else
                                //    {
                                //        if (Form1.pvindex == 1)
                                //        {
                                //            mvars.FPGAtxt[Form1.pvindex] = mp.BinToDec(mvars.lblFPGAtxt[Form1.pvindex].Text).ToString();
                                //        }
                                //        else { mvars.FPGAtxt[Form1.pvindex] = mvars.lblFPGAtxt[Form1.pvindex].Text; }
                                //        mvars.lblFPGAtxt[Form1.pvindex].BackColor = Color.White;
                                //        if (mvars.lblFPGAkw[Form1.pvindex].BackColor == Control.DefaultBackColor) { mvars.lblFPGAkw[Form1.pvindex].ForeColor = Color.Cyan; }
                                //        else if (mvars.lblFPGAkw[Form1.pvindex].BackColor == Color.Cyan) { mvars.lblFPGAkw[Form1.pvindex].ForeColor = Color.White; }
                                //    }   
                                //}
                                #endregion
                                break;
                            case "FPGA_SPI_R":
                                #region FPGA_SPI_R
                                if (mvars.deviceID.Substring(0, 2) == "05")
                                {
                                    if (mvars.FormShow[8] == true && uc_FPGAreg.gboxFPGAstatus.Visible == true)      //uc_FPFAreg
                                    {
                                        #region Primary FPGA status
                                        string[] svsts = new string[2];
                                        svsts[1] = DecToBin((int)mvars.ReadDataBuffer[7], 8);
                                        svsts[0] = DecToBin((int)mvars.ReadDataBuffer[8], 8);
                                        if (Form1.pvindex == 0x6000)
                                        {
                                            uc_FPGAreg.lblststxt6000.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6000[12].Text = "";
                                            for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6000[m].Text = "- -"; }
                                            //if (svsts[1].Substring(4, 4) == "1010")
                                            //{
                                            uc_FPGAreg.lblsts6000[12].Text = svsts[1].Substring(4, 1);
                                            for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6000[m].Text = svsts[0].Substring(7 - m, 1); }
                                            //}
                                        }
                                        else if (Form1.pvindex == 0x6010)
                                        {
                                            uc_FPGAreg.lblststxt6010.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            for (int m = 11; m >= 8; m--) { uc_FPGAreg.lblsts6010[m].Text = "- -"; }
                                            for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6010[m].Text = "- -"; }
                                            //if (svsts[1].Substring(0, 4) == "0101")
                                            //{
                                            for (int m = 11; m >= 8; m--) { uc_FPGAreg.lblsts6010[m].Text = svsts[1].Substring(15 - m, 1); }
                                            for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6010[m].Text = svsts[0].Substring(7 - m, 1); }
                                            //}
                                        }
                                        else if (Form1.pvindex == 0x6011)
                                        {
                                            uc_FPGAreg.lblststxt6011.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6011[14].Text = "- -";
                                            uc_FPGAreg.lblsts6011[8].Text = "- -";
                                            uc_FPGAreg.lblsts6011[7].Text = "- -";
                                            for (int m = 4; m >= 0; m--) { uc_FPGAreg.lblsts6011[m].Text = "- -"; }
                                            //if (svsts[0].Substring(1, 2) == "00")
                                            //{
                                            uc_FPGAreg.lblsts6011[14].Text = svsts[1].Substring(1, 1);
                                            uc_FPGAreg.lblsts6011[8].Text = BinToDec(svsts[1].Substring(2, 6)).ToString();
                                            uc_FPGAreg.lblsts6011[7].Text = svsts[0].Substring(0, 1);
                                            for (int m = 4; m >= 0; m--) { uc_FPGAreg.lblsts6011[m].Text = svsts[0].Substring(7 - m, 1); }
                                            //}
                                        }
                                        else if (Form1.pvindex == 0x6012)
                                        {
                                            uc_FPGAreg.lblststxt6012.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6012[8].Text = "- -";
                                            for (int m = 6; m >= 0; m--) { uc_FPGAreg.lblsts6012[m].Text = "- -"; }
                                            //if (svsts[1].Substring(0, 3) == "111" && svsts[0].Substring(0, 1) == "0")
                                            //{
                                            uc_FPGAreg.lblsts6012[8].Text = BinToDec(svsts[1].Substring(3, 5)).ToString();
                                            for (int m = 6; m >= 0; m--) { uc_FPGAreg.lblsts6012[m].Text = svsts[0].Substring(7 - m, 1); }
                                            //}
                                        }
                                        else if (Form1.pvindex == 0x6020)
                                        {
                                            uc_FPGAreg.lblststxt6020.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            for (int m = 13; m >= 12; m--) { uc_FPGAreg.lblsts6020[m].Text = svsts[1].Substring(15 - m, 1); }
                                            uc_FPGAreg.lblsts6020[8].Text = BinToDec(svsts[1].Substring(4, 4)).ToString();
                                            uc_FPGAreg.lblsts6020[0].Text = mvars.ReadDataBuffer[8].ToString();
                                        }
                                        else if (Form1.pvindex == 0x6021)
                                        {
                                            uc_FPGAreg.lblststxt6021.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            for (int m = 15; m >= 12; m--) { uc_FPGAreg.lblsts6021[m].Text = svsts[1].Substring(15 - m, 1); }
                                            uc_FPGAreg.lblsts6021[8].Text = BinToDec(svsts[1].Substring(4, 4)).ToString();
                                            uc_FPGAreg.lblsts6021[0].Text = BinToDec(svsts[0].Substring(0, 8)).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6040)
                                        {
                                            uc_FPGAreg.lblststxt6040.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            for (int m = 3; m >= 2; m--) { uc_FPGAreg.lblsts6040[m].Text = "- -"; }
                                            uc_FPGAreg.lblsts6040[0].Text = "- -";
                                            //if (svsts[1] == "01011010")
                                            //{
                                            for (int m = 3; m >= 2; m--) { uc_FPGAreg.lblsts6040[m].Text = svsts[0].Substring(7 - m, 1); }
                                            uc_FPGAreg.lblsts6040[0].Text = BinToDec(svsts[0].Substring(1, 2)).ToString();
                                            //}
                                        }
                                        else if (Form1.pvindex == 0x6041)
                                        {
                                            uc_FPGAreg.lblststxt6041.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6041[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6042)
                                        {
                                            uc_FPGAreg.lblststxt6042.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6042[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6043)
                                        {
                                            uc_FPGAreg.lblststxt6043.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6043[0].Text = "- -";
                                            //if (svsts[1].Substring(0, 4) == "1010")
                                            //{
                                            uc_FPGAreg.lblsts6043[0].Text = mvars.ReadDataBuffer[8].ToString();
                                            //}
                                        }
                                        else if (Form1.pvindex == 0x6044)
                                        {
                                            uc_FPGAreg.lblststxt6044.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6044[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6045)
                                        {
                                            uc_FPGAreg.lblststxt6045.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6045[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6046)
                                        {
                                            uc_FPGAreg.lblststxt6046.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6046[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6047)
                                        {
                                            uc_FPGAreg.lblststxt6047.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6047[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6048)
                                        {
                                            uc_FPGAreg.lblststxt6048.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6048[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6049)
                                        {
                                            uc_FPGAreg.lblststxt6049.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6049[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x604a)
                                        {
                                            uc_FPGAreg.lblststxt604a.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts604a[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x604b)
                                        {
                                            uc_FPGAreg.lblststxt604b.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts604b[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                        }
                                        else if (Form1.pvindex == 0x6060)
                                        {
                                            uc_FPGAreg.lblststxt6060.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6060[8].Text = "- -";
                                            uc_FPGAreg.lblsts6060[0].Text = "- -";
                                            //if (svsts[1].Substring(0, 4) == "1010")
                                            //{
                                            uc_FPGAreg.lblsts6060[8].Text = BinToDec(svsts[1].Substring(6, 2)).ToString();
                                            uc_FPGAreg.lblsts6060[0].Text = mvars.ReadDataBuffer[8].ToString();
                                            //}
                                        }
                                        else if (Form1.pvindex == 0x6061)
                                        {
                                            uc_FPGAreg.lblststxt6061.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6061[m].Text = "- -"; }
                                            //if (svsts[1].Substring(0, 8) == "01011010")
                                            //{
                                            for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6061[m].Text = svsts[0].Substring(7 - m, 1); }
                                            //}
                                        }
                                        else if (Form1.pvindex == 0x6062)
                                        {
                                            uc_FPGAreg.lblststxt6062.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                            uc_FPGAreg.lblsts6062[8].Text = mvars.ReadDataBuffer[7].ToString();
                                            uc_FPGAreg.lblsts6062[0].Text = mvars.ReadDataBuffer[8].ToString();
                                        }
                                        #endregion Primary FPGA status
                                    }
                                    else
                                    {
                                        if (Form1.pvindex == 0)
                                        {
                                            mvars.verFPGAm = new string[2];
                                            mvars.verFPGA = "-1";
                                            strTemp = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[6 + 1]), 2) + "." + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[7 + 1]), 2);
                                            mvars.verFPGAm[0] = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[6 + 1]), 2) + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[7 + 1]), 2);
                                            mvars.verFPGA = strTemp;
                                            strTemp = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[8 + 1]), 2) + "." + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[9 + 1]), 2);
                                            mvars.verFPGAm[1] = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[8 + 1]), 2) + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[9 + 1]), 2);
                                            strTemp = mvars.verFPGA + "-" + strTemp;
                                        }
                                        else if (Form1.pvindex == 1)
                                        {
                                            //將數字前面補0，補足設定的長度
                                            strTemp = Convert.ToString(mvars.ReadDataBuffer[7 + 1], 2).PadLeft(8, '0') + "-" + Convert.ToString(mvars.ReadDataBuffer[9 + 1], 2).PadLeft(8, '0');
                                        }
                                        else
                                        {
                                            strTemp = (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]) + "-" + (mvars.ReadDataBuffer[8 + 1] * 256 + mvars.ReadDataBuffer[9 + 1]);
                                        }
                                        Svs = "R" + Form1.pvindex.ToString("000") + "," + strTemp;
                                    }
                                }
                                else if (mvars.deviceID.Substring(0, 2) == "06")
                                {
                                    if (Form1.pvindex == 0)
                                    {
                                        mvars.verFPGA = "-1";
                                        strTemp = Convert.ToString(mvars.ReadDataBuffer[6 + 1], 2).PadLeft(8, '0') + Convert.ToString(mvars.ReadDataBuffer[7 + 1], 2).PadLeft(8, '0');
                                        strTemp = (Convert.ToInt32(strTemp.Substring(0, 7), 2)).ToString() + "." + (Convert.ToInt32(strTemp.Substring(7, 5), 2)).ToString() + "." + (Convert.ToInt32(strTemp.Substring(12, 4), 2)).ToString();
                                        mvars.verFPGA = strTemp;
                                        strTemp = Convert.ToString(mvars.ReadDataBuffer[8 + 1], 2).PadLeft(8, '0') + Convert.ToString(mvars.ReadDataBuffer[9 + 1], 2).PadLeft(8, '0');
                                        strTemp = (Convert.ToInt32(strTemp.Substring(0, 7), 2)).ToString() + "." + (Convert.ToInt32(strTemp.Substring(7, 5), 2)).ToString() + "." + (Convert.ToInt32(strTemp.Substring(12, 4), 2)).ToString();
                                        strTemp = mvars.verFPGA + "-" + strTemp;
                                    }
                                    else
                                    {
                                        strTemp = (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]) + "-" + (mvars.ReadDataBuffer[8 + 1] * 256 + mvars.ReadDataBuffer[9 + 1]);
                                    }
                                    Svs = "R" + Form1.pvindex.ToString("000") + "," + strTemp;
                                }
                                else
                                {
                                    if (mvars.FormShow[8] == true)      //uc_FPFAreg
                                    {
                                        if (uc_FPGAreg.gboxFPGAstatus.Visible == true)
                                        {
                                            string[] svsts = new string[2];
                                            svsts[1] = DecToBin((int)mvars.ReadDataBuffer[7], 8);
                                            svsts[0] = DecToBin((int)mvars.ReadDataBuffer[8], 8);
                                            #region FPGAstatus
                                            if (Form1.pvindex == 0x6000)
                                            {
                                                uc_FPGAreg.lblststxt6000.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6000[12].Text = "";
                                                for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6000[m].Text = "- -"; }
                                                //if (svsts[1].Substring(4, 4) == "1010")
                                                //{
                                                uc_FPGAreg.lblsts6000[12].Text = svsts[1].Substring(4, 1);
                                                for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6000[m].Text = svsts[0].Substring(7 - m, 1); }
                                                //}
                                            }
                                            else if (Form1.pvindex == 0x6010)
                                            {
                                                uc_FPGAreg.lblststxt6010.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                for (int m = 11; m >= 8; m--) { uc_FPGAreg.lblsts6010[m].Text = "- -"; }
                                                for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6010[m].Text = "- -"; }
                                                //if (svsts[1].Substring(0, 4) == "0101")
                                                //{
                                                for (int m = 11; m >= 8; m--) { uc_FPGAreg.lblsts6010[m].Text = svsts[1].Substring(15 - m, 1); }
                                                for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6010[m].Text = svsts[0].Substring(7 - m, 1); }
                                                //}
                                            }
                                            else if (Form1.pvindex == 0x6011)
                                            {
                                                uc_FPGAreg.lblststxt6011.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6011[14].Text = "- -";
                                                uc_FPGAreg.lblsts6011[8].Text = "- -";
                                                uc_FPGAreg.lblsts6011[7].Text = "- -";
                                                for (int m = 4; m >= 0; m--) { uc_FPGAreg.lblsts6011[m].Text = "- -"; }
                                                //if (svsts[0].Substring(1, 2) == "00")
                                                //{
                                                uc_FPGAreg.lblsts6011[14].Text = svsts[1].Substring(1, 1);
                                                uc_FPGAreg.lblsts6011[8].Text = BinToDec(svsts[1].Substring(2, 6)).ToString();
                                                uc_FPGAreg.lblsts6011[7].Text = svsts[0].Substring(0, 1);
                                                for (int m = 4; m >= 0; m--) { uc_FPGAreg.lblsts6011[m].Text = svsts[0].Substring(7 - m, 1); }
                                                //}
                                            }
                                            else if (Form1.pvindex == 0x6012)
                                            {
                                                uc_FPGAreg.lblststxt6012.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6012[8].Text = "- -";
                                                for (int m = 6; m >= 0; m--) { uc_FPGAreg.lblsts6012[m].Text = "- -"; }
                                                //if (svsts[1].Substring(0, 3) == "111" && svsts[0].Substring(0, 1) == "0")
                                                //{
                                                uc_FPGAreg.lblsts6012[8].Text = BinToDec(svsts[1].Substring(3, 5)).ToString();
                                                for (int m = 6; m >= 0; m--) { uc_FPGAreg.lblsts6012[m].Text = svsts[0].Substring(7 - m, 1); }
                                                //}
                                            }
                                            else if (Form1.pvindex == 0x6020)
                                            {
                                                uc_FPGAreg.lblststxt6020.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                for (int m = 13; m >= 12; m--) { uc_FPGAreg.lblsts6020[m].Text = svsts[1].Substring(15 - m, 1); }
                                                uc_FPGAreg.lblsts6020[8].Text = BinToDec(svsts[1].Substring(4, 4)).ToString();
                                                uc_FPGAreg.lblsts6020[0].Text = mvars.ReadDataBuffer[8].ToString();
                                            }
                                            else if (Form1.pvindex == 0x6021)
                                            {
                                                uc_FPGAreg.lblststxt6021.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                for (int m = 15; m >= 12; m--) { uc_FPGAreg.lblsts6021[m].Text = svsts[1].Substring(15 - m, 1); }
                                                uc_FPGAreg.lblsts6021[8].Text = BinToDec(svsts[1].Substring(4, 4)).ToString();
                                                uc_FPGAreg.lblsts6021[0].Text = BinToDec(svsts[0].Substring(0, 8)).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6040)
                                            {
                                                uc_FPGAreg.lblststxt6040.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                for (int m = 3; m >= 2; m--) { uc_FPGAreg.lblsts6040[m].Text = "- -"; }
                                                uc_FPGAreg.lblsts6040[0].Text = "- -";
                                                //if (svsts[1] == "01011010")
                                                //{
                                                for (int m = 3; m >= 2; m--) { uc_FPGAreg.lblsts6040[m].Text = svsts[0].Substring(7 - m, 1); }
                                                uc_FPGAreg.lblsts6040[0].Text = BinToDec(svsts[0].Substring(1, 2)).ToString();
                                                //}
                                            }
                                            else if (Form1.pvindex == 0x6041)
                                            {
                                                uc_FPGAreg.lblststxt6041.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6041[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6042)
                                            {
                                                uc_FPGAreg.lblststxt6042.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6042[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6043)
                                            {
                                                uc_FPGAreg.lblststxt6043.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6043[0].Text = "- -";
                                                //if (svsts[1].Substring(0, 4) == "1010")
                                                //{
                                                uc_FPGAreg.lblsts6043[0].Text = mvars.ReadDataBuffer[8].ToString();
                                                //}
                                            }
                                            else if (Form1.pvindex == 0x6044)
                                            {
                                                uc_FPGAreg.lblststxt6044.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6044[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6045)
                                            {
                                                uc_FPGAreg.lblststxt6045.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6045[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6046)
                                            {
                                                uc_FPGAreg.lblststxt6046.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6046[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6047)
                                            {
                                                uc_FPGAreg.lblststxt6047.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6047[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6048)
                                            {
                                                uc_FPGAreg.lblststxt6048.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6048[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6049)
                                            {
                                                uc_FPGAreg.lblststxt6049.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6049[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x604a)
                                            {
                                                uc_FPGAreg.lblststxt604a.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts604a[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x604b)
                                            {
                                                uc_FPGAreg.lblststxt604b.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts604b[0].Text = (mvars.ReadDataBuffer[7] * 256 + mvars.ReadDataBuffer[8]).ToString();
                                            }
                                            else if (Form1.pvindex == 0x6060)
                                            {
                                                uc_FPGAreg.lblststxt6060.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6060[8].Text = "- -";
                                                uc_FPGAreg.lblsts6060[0].Text = "- -";
                                                //if (svsts[1].Substring(0, 4) == "1010")
                                                //{
                                                uc_FPGAreg.lblsts6060[8].Text = BinToDec(svsts[1].Substring(6, 2)).ToString();
                                                uc_FPGAreg.lblsts6060[0].Text = mvars.ReadDataBuffer[8].ToString();
                                                //}
                                            }
                                            else if (Form1.pvindex == 0x6061)
                                            {
                                                uc_FPGAreg.lblststxt6061.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6061[m].Text = "- -"; }
                                                //if (svsts[1].Substring(0, 8) == "01011010")
                                                //{
                                                for (int m = 7; m >= 0; m--) { uc_FPGAreg.lblsts6061[m].Text = svsts[0].Substring(7 - m, 1); }
                                                //}
                                            }
                                            else if (Form1.pvindex == 0x6062)
                                            {
                                                uc_FPGAreg.lblststxt6062.Text = mp.DecToHex(mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1], 4);
                                                uc_FPGAreg.lblsts6062[8].Text = mvars.ReadDataBuffer[7].ToString();
                                                uc_FPGAreg.lblsts6062[0].Text = mvars.ReadDataBuffer[8].ToString();
                                            }
                                            #endregion FPGAstatus
                                        }
                                        else
                                        {
                                            if (Form1.pvindex == 32767)
                                            {
                                                strTemp = (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]).ToString();
                                                Svs = "R" + Form1.pvindex.ToString("00000") + "," + strTemp;
                                            }
                                            else
                                            {
                                                if (Form1.pvindex == 0)
                                                {
                                                    mvars.verFPGA = "-1";
                                                    if (mvars.deviceID.Substring(0, 2) == "03")
                                                        strTemp = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[6 + 1]), 2) + "." + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[7 + 1]), 2);
                                                    else if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04")
                                                        strTemp = (Convert.ToInt32(strTemp.Substring(0, 7), 2)).ToString() + "." + (Convert.ToInt32(strTemp.Substring(7, 5), 2)).ToString() + "." + (Convert.ToInt32(strTemp.Substring(12, 4), 2)).ToString();
                                                    else if (mvars.deviceID.Substring(0, 2) == "10")
                                                        strTemp = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[6 + 1]), 2) + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[7 + 1]), 2);
                                                    mvars.verFPGA = strTemp;
                                                }
                                                else if (Form1.pvindex == 1)
                                                {
                                                    if (mvars.deviceID.Substring(0, 2) == "03") { strTemp = Convert.ToString(mvars.ReadDataBuffer[8 + 1], 2).PadLeft(8, '0'); }
                                                    else if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04") { strTemp = (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]).ToString(); }
                                                    else if (mvars.deviceID.Substring(0, 2) == "10") strTemp = Convert.ToString(mvars.ReadDataBuffer[7 + 1], 2).PadLeft(8, '0');
                                                }
                                                else
                                                {
                                                    strTemp = (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]).ToString();
                                                    if (mvars.deviceID.Substring(0,2) == "10")
                                                    {
                                                        if (Form1.pvindex >= 6 && Form1.pvindex <= 9)
                                                        {
                                                            strTemp = mp.BinToDec(mp.DecToBin((mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]), 16).Substring(0, 4)).ToString() + "." +
                                                                mp.BinToDec(mp.DecToBin((mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]), 16).Substring(3, 12)).ToString();
                                                        }
                                                    }
                                                }
                                                Svs = "R" + Form1.pvindex.ToString("000") + "," + strTemp;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Form1.pvindex == 0)
                                        {
                                            mvars.verFPGA = "-1";
                                            if (mvars.deviceID.Substring(0, 2) == "03")
                                            {
                                                strTemp = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[6 + 1]), 2) + "." + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[7 + 1]), 2);
                                                mvars.verFPGA = strTemp;
                                            }
                                            else if (mvars.deviceID.Substring(0, 2) == "05")
                                            {
                                                strTemp = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[6 + 1]), 2) + "." + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[7 + 1]), 2);
                                                mvars.verFPGA = strTemp + "-" + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[8 + 1]), 2) + "." + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[9 + 1]), 2);
                                            }
                                            else if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04" || mvars.deviceID.Substring(0, 2) == "06")
                                            {
                                                strTemp = Convert.ToString(mvars.ReadDataBuffer[6 + 1], 2).PadLeft(8, '0') + Convert.ToString(mvars.ReadDataBuffer[7 + 1], 2).PadLeft(8, '0');
                                                mvars.verFPGA = (Convert.ToInt32(strTemp.Substring(0, 7), 2)).ToString() + "." + (Convert.ToInt32(strTemp.Substring(7, 5), 2)).ToString() + "." + (Convert.ToInt32(strTemp.Substring(12, 4), 2)).ToString();
                                            }
                                            else if (mvars.deviceID.Substring(0, 2) == "10")
                                            {
                                                strTemp = DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[6 + 1]), 2) + DecToHex(Convert.ToInt16(mvars.ReadDataBuffer[7 + 1]), 2);
                                                mvars.verFPGA = strTemp;
                                            }
                                            Svs = "R" + Form1.pvindex.ToString("000") + "," + mvars.verFPGA;
                                        }
                                        else if (Form1.pvindex == 2) { strTemp = (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]).ToString(); Svs = "R" + Form1.pvindex.ToString("000") + "," + strTemp; }
                                        else { strTemp = (mvars.ReadDataBuffer[6 + 1] * 256 + mvars.ReadDataBuffer[7 + 1]).ToString(); Svs = "R" + Form1.pvindex.ToString("000") + "," + strTemp; }
                                    }

                                }
                                #endregion
                                break;
                            case "FUNC_STATUS":
                                #region FUNC_STATUS
                                Svs = Byte2HexString(mvars.ReadDataBuffer[6 + 1]);
                                mvars.hFuncStatus = mvars.ReadDataBuffer[6 + 1];
                                #endregion
                                break;
                            case "OBB_XBS":
                                #region OBB_XBS
                                Array.Resize(ref mvars.verMCUS, Convert.ToInt16(mvars.ReadDataBuffer[6 + 1]));
                                Array.Clear(mvars.verMCUS, 0, mvars.verMCUS.Length);
                                Svs = "OBB_XBS," + mvars.ReadDataBuffer[6 + 1];
                                #endregion
                                break;
                            case "PWIC_INFO":
                                #region PWIC_INFO
                                strTemp = Byte2HexString(mvars.ReadDataBuffer[6 + 1]) + Byte2HexString(mvars.ReadDataBuffer[7 + 1]) + Byte2HexString(mvars.ReadDataBuffer[8 + 1]);
                                strTemp += Byte2HexString(mvars.ReadDataBuffer[9 + 1]);
                                strTemp += Byte2HexString(mvars.ReadDataBuffer[10 + 1]) + Byte2HexString(mvars.ReadDataBuffer[11 + 1]);
                                Svs = strTemp;
                                #endregion
                                break;
                            case "MCU_FLASH_R60000":
                                #region MCU_FLASH_R60000
                                int i = 0;
                                mvars.strR60K = "";
                                do
                                {
                                    Svs = (mvars.ReadDataBuffer[6 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[7 + (i * 4) + 1]).ToString() + "," + (mvars.ReadDataBuffer[8 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[9 + (i * 4) + 1]).ToString();
                                    if (Svs == "65535,65535" || (i == 0 && Svs == "0,0")) break;
                                    //if (i == 0 && Svs == "0,0") break;
                                    mvars.strR60K += "~" + Svs;
                                    i++;
                                }
                                while (i < mvars.FPGA_MTP_SIZE / 4);
                                if (mvars.strR60K.Length > 1 && mvars.strR60K.Substring(0, 1) == "~") mvars.strR60K = mvars.strR60K.Substring(1, mvars.strR60K.Length - 1);
                                #endregion
                                break;
                            case "MCU_FLASH_R62000":
                                #region MCU_FLASH_R62000
                                i = 0;
                                mvars.strR62K = "";
                                do
                                {
                                    Svs = (mvars.ReadDataBuffer[6 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[7 + (i * 4) + 1]).ToString() + "," + (mvars.ReadDataBuffer[8 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[9 + (i * 4) + 1]).ToString();
                                    if (Svs.IndexOf(",65535") != -1 || (i == 0 && Svs == "0,0")) break;
                                    mvars.strR62K += "~" + Svs;
                                    i++;
                                }
                                while (i < mvars.GAMMA_MTP_SIZE / 4);  
                                if (mvars.strR62K.Length > 1 && mvars.strR62K.Substring(0, 1) == "~") mvars.strR62K = mvars.strR62K.Substring(1, mvars.strR62K.Length - 1);
                                #endregion
                                break;
                            case "MCU_FLASH_R64000":
                                #region MCU_FLASH_R64000
                                mvars.strR64K = "";
                                i = 0;
                                do
                                {
                                    Svs = (mvars.ReadDataBuffer[6 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[7 + (i * 4) + 1]).ToString() + "," + (mvars.ReadDataBuffer[8 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[9 + (i * 4) + 1]).ToString();
                                    if (Svs.IndexOf(",65535") != -1 || (i == 0 && Svs == "0,0")) break;
                                    mvars.strR64K += "~" + Svs;
                                    i++;
                                }
                                while (i < mvars.GAMMA_MTP_SIZE / 4);
                                if (mvars.strR64K.Length > 1 && mvars.strR64K.Substring(0, 1) == "~") mvars.strR64K = mvars.strR64K.Substring(1, mvars.strR64K.Length - 1);
                                #endregion
                                break;
                            case "MCU_FLASH_R66000":
                                #region MCU_FLASH_R66000
                                mvars.strR66K = "";
                                i = 0;
                                do
                                {
                                    Svs = (mvars.ReadDataBuffer[6 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[7 + (i * 4) + 1]).ToString() + "," + (mvars.ReadDataBuffer[8 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[9 + (i * 4) + 1]).ToString();
                                    if (Svs == "65535,65535" || (i == 0 && Svs == "0,0")) break; 
                                    mvars.strR66K += "~" + Svs;
                                    i++;
                                }
                                while (i < mvars.GAMMA_MTP_SIZE / 4);
                                if (mvars.strR66K.Length > 1 && mvars.strR66K.Substring(0, 1) == "~") mvars.strR66K = mvars.strR66K.Substring(1, mvars.strR66K.Length - 1);
                                #endregion
                                break;
                            case "MCU_FLASH_R30000":
                                #region MCU_FLASH_R30000
                                i = 0;
                                mvars.strR60K = "";
                                do
                                {
                                    Svs = (mvars.ReadDataBuffer[6 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[7 + (i * 4) + 1]).ToString() + "," + (mvars.ReadDataBuffer[8 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[9 + (i * 4) + 1]).ToString();
                                    if (Svs == "65535,65535" || Svs == "0,0") break;
                                    //if (i == 0 && Svs == "0,0") break;
                                    mvars.strR60K += "~" + Svs;
                                    i++;
                                }
                                while (i < mvars.FPGA_MTP_SIZE / 4);
                                if (mvars.strR60K.Length > 1 && mvars.strR60K.Substring(0, 1) == "~") mvars.strR60K = mvars.strR60K.Substring(1, mvars.strR60K.Length - 1);
                                #endregion
                                break;
                            case "MCU_FLASH_R32000":
                                #region MCU_FLASH_R32000
                                i = 0;
                                mvars.strR62K = "";
                                do
                                {
                                    Svs = (mvars.ReadDataBuffer[6 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[7 + (i * 4) + 1]).ToString() + "," + (mvars.ReadDataBuffer[8 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[9 + (i * 4) + 1]).ToString();
                                    if (Svs.IndexOf(",65535") != -1 || Svs == "0,0") break;
                                    mvars.strR62K += "~" + Svs;
                                    i++;
                                }
                                while (i < mvars.GAMMA_MTP_SIZE / 4);
                                if (mvars.strR62K.Length > 1 && mvars.strR62K.Substring(0, 1) == "~") mvars.strR62K = mvars.strR62K.Substring(1, mvars.strR62K.Length - 1);
                                #endregion
                                break;
                            case "MCU_FLASH_R34000":
                                #region MCU_FLASH_R34000
                                mvars.strR64K = "";
                                i = 0;
                                do
                                {
                                    Svs = (mvars.ReadDataBuffer[6 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[7 + (i * 4) + 1]).ToString() + "," + (mvars.ReadDataBuffer[8 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[9 + (i * 4) + 1]).ToString();
                                    if (Svs.IndexOf(",65535") != -1 || Svs == "0,0") break;
                                    mvars.strR64K += "~" + Svs;
                                    i++;
                                }
                                while (i < mvars.GAMMA_MTP_SIZE / 4);
                                if (mvars.strR64K.Length > 1 && mvars.strR64K.Substring(0, 1) == "~") mvars.strR64K = mvars.strR64K.Substring(1, mvars.strR64K.Length - 1);
                                #endregion
                                break;
                            case "MCU_FLASH_R36000":
                                #region MCU_FLASH_R36000
                                mvars.strR66K = "";
                                i = 0;
                                do
                                {
                                    Svs = (mvars.ReadDataBuffer[6 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[7 + (i * 4) + 1]).ToString() + "," + (mvars.ReadDataBuffer[8 + (i * 4) + 1] * 256 + mvars.ReadDataBuffer[9 + (i * 4) + 1]).ToString();
                                    if (Svs == "65535,65535" || Svs == "0,0") break;
                                    mvars.strR66K += "~" + Svs;
                                    i++;
                                }
                                while (i < mvars.GAMMA_MTP_SIZE / 4);
                                if (mvars.strR66K.Length > 1 && mvars.strR66K.Substring(0, 1) == "~") mvars.strR66K = mvars.strR66K.Substring(1, mvars.strR66K.Length - 1);
                                #endregion
                                break;
                            case "UIREGRAD_READ":
                                #region UIREGARD_READ
                                Svs = "DONE";
                                #endregion
                                break;
                            case "READ_MCUBOOTVER":
                                mvars.verMCUB = "-1";
                                #region READ_MCUBOOTVER
                                tmp = new byte[16];
                                Copy(mvars.ReadDataBuffer, 6 + 1, tmp, 0, 16);
                                mvars.verMCUB = Encoding.ASCII.GetString(tmp);
                                Svs = "MCU_BOOTID," + mvars.verMCUB;
                                #endregion
                                break;
                            case "READ_MCUAPPVER":
                                mvars.verMCUA = "-1";
                                #region READ_MCUAPPVER
                                tmp = new byte[16];
                                Copy(mvars.ReadDataBuffer, 6 + 1, tmp, 0, 16);
                                mvars.verMCUA = Encoding.ASCII.GetString(tmp);
                                Svs = "MCU_APPID," + mvars.verMCUA;
                                #endregion
                                break;
                            case "MCU_VERSION":
                                #region MCU_VERSION
                                string svdeviceID = Byte2HexString(mvars.ReadDataBuffer[1]) + Byte2HexString(mvars.ReadDataBuffer[2]);
                                if (svdeviceID == mvars.deviceID)
                                {
                                    tmp = new byte[16];
                                    Copy(mvars.ReadDataBuffer, 6 + 1, tmp, 0, 16);
                                    strTemp = System.Text.Encoding.ASCII.GetString(tmp);
                                    Svs = "DeviceID," + svdeviceID + ",MCU Version," + strTemp;
                                    if (mvars.deviceID.Substring(2, 2) == "00") mvars.verMCU = strTemp;
                                    else
                                    {
                                        if (mvars.deviceID.Substring(0, 2) == "03") { mvars.verMCUS[Convert.ToInt16(mvars.deviceID.Substring(2, 2)) - 1] = strTemp; }
                                    }
                                    // 29更新
                                    mvars.verMCU = strTemp;
                                }
                                else
                                {
                                    mvars.verMCU = "-2." + svdeviceID;
                                    Svs = "-1,DeviceID not match";
                                }


                                //if (mvars.deviceID.Substring(0, 2) == "02" 
                                // || mvars.deviceID.Substring(0, 2) == "03"
                                // || mvars.deviceID.Substring(0, 2) == "04" 
                                // || mvars.deviceID.Substring(0, 2) == "05"
                                // || mvars.deviceID.Substring(0, 2) == "06"
                                // || mvars.deviceID.Substring(0, 2) == "10")
                                //{
                                //    string svdeviceID = Byte2HexString(mvars.ReadDataBuffer[1]) + Byte2HexString(mvars.ReadDataBuffer[2]);
                                //    if (svdeviceID == mvars.deviceID)
                                //    {
                                //        tmp = new byte[16];
                                //        Copy(mvars.ReadDataBuffer, 6 + 1, tmp, 0, 16);
                                //        strTemp = System.Text.Encoding.ASCII.GetString(tmp);
                                //        Svs = "DeviceID," + svdeviceID + ",MCU Version," + strTemp ;
                                //        if (mvars.deviceID.Substring(2, 2) == "00") mvars.verMCU = strTemp; 
                                //        else 
                                //        {
                                //            if (mvars.deviceID.Substring(0, 2) == "03") { mvars.verMCUS[Convert.ToInt16(mvars.deviceID.Substring(2, 2)) - 1] = strTemp; }
                                //        }
                                //        // 29更新
                                //        mvars.verMCU = strTemp;
                                //    }
                                //    else
                                //    {
                                //        mvars.verMCU = "-2." + svdeviceID;
                                //        Svs = "-1,DeviceID not match";
                                //    }
                                //}                               
                                #endregion
                                break;
                            case "MCU_INFORMATION":
                                #region MCU_INFORMATION
                                if (mvars.deviceID.Substring(0, 2) == "02"
                                 || mvars.deviceID.Substring(0, 2) == "03"
                                 || mvars.deviceID.Substring(0, 2) == "04"
                                 || mvars.deviceID.Substring(0, 2) == "05"
                                 || mvars.deviceID.Substring(0, 2) == "06")
                                {
                                    mvars.projectName = "UnKnow";
                                    UInt16 MCU_VerLen = (UInt16)(Convert.ToUInt16(mvars.ReadDataBuffer[3 + 1] * 256 + mvars.ReadDataBuffer[4 + 1]) - 10);
                                    tmp = new byte[MCU_VerLen];
                                    Copy(mvars.ReadDataBuffer, 6 + 1, tmp, 0, MCU_VerLen);
                                    string MCU_Ver = System.Text.Encoding.ASCII.GetString(tmp);
                                    MCU_Ver = ContinueAsciiFilter(MCU_Ver, "\0");
                                    string[] split = MCU_Ver.Split(new Char[] { '\0', '\t' });
                                    //textBox290.Text = split[0];
                                    //textBox292.Text = split[1];
                                    //textBox294.Text = split[2];
                                    //ProjectName_txt.Text = split[3];
                                    //textBox291.Text = split[4];
                                    //textBox289.Text = split[5];
                                    //OP_Msg1(OP_Msg_lbx, "Annotation ：" + split[5]);
                                    //OP_Msg1(OP_Msg_lbx, "Code Mode ：" + split[4]);
                                    //OP_Msg1(OP_Msg_lbx, "Project Name ：" + split[3]);
                                    //OP_Msg1(OP_Msg_lbx, "Time ：" + split[2]);
                                    //OP_Msg1(OP_Msg_lbx, "Date ：" + split[1]);
                                    //OP_Msg1(OP_Msg_lbx, "MCU Version ：" + split[0]);

                                    Svs = split[3] + "," + split[0];
                                    mvars.projectName = split[3];

                                    for (byte sva = 0; sva < mvars.deviceAll.Length; sva++)
                                    {
                                        if (mvars.deviceAll[sva] == mvars.projectName)
                                        {
                                            mvars.deviceID = mvars.deviceIDall[sva];
                                            mvars.deviceName = mvars.projectName;
                                            break;
                                        }
                                    }
                                }
                                #endregion
                                break;
                            default:    //9個字判斷
                                if (mvars.lblCmd.Substring(0, 9) == "CM603_WRI")
                                {
                                    #region CM603_WRI
                                    Svs = "DONE";
                                    //if (Form1.cmbCM603.Text.Trim().Substring(0, 1) == "R") Svi = 0;
                                    //else if (Form1.cmbCM603.Text.Trim().Substring(0, 1) == "G") Svi = 1;
                                    //else if (Form1.cmbCM603.Text.Trim().Substring(0, 1) == "B") Svi = 2;
                                    //if (Form1.pvindex >= 0x04 && Form1.pvindex <= 0x0C)
                                    //{
                                    //    int svbinI = mvars.cm603dfB[Svi, (byte)Form1.pvindex * 2] * 256 + mvars.cm603dfB[Svi, (byte)Form1.pvindex * 2 + 1];
                                    //    string svbinS = mp.DecToBin(svbinI, 16);
                                    //    svbinI = mp.BinToDec(svbinS.Substring(6, 10));
                                    //    mvars.pGMA.Data[Svi, (mvars.pGMA.Data.Length / 3) - (Form1.pvindex - 0x03) * 2] = mp.DecToHex(svbinI, 4).Substring(0, 2);
                                    //    mvars.pGMA.Data[Svi, (mvars.pGMA.Data.Length / 3) - (Form1.pvindex - 0x03) * 2 + 1] = mp.DecToHex(svbinI, 4).Substring(2, 2);
                                    //}
                                    #endregion
                                }
                                else if (mvars.lblCmd.Substring(0, 9) == "CM603_REA") //R[0,0]0xD5 G[1,0]0xD7 B[1,1]0xDB
                                {
                                    #region CM603_REA
                                    Svs = "DONE";
                                    if (mvars.deviceID.Substring(0, 2) == "02" || 
                                        mvars.deviceID.Substring(0, 2) == "04" || 
                                        mvars.deviceID.Substring(0, 2) == "05" ||
                                        mvars.deviceID.Substring(0, 2) == "06" ||
                                        mvars.deviceID.Substring(0, 2) == "10")
                                    {
                                        //C12A
                                        if (Form1.cmbCM603.Text.Trim().Substring(0, 1) == "R") { Svi = 0; }
                                        else if (Form1.cmbCM603.Text.Trim().Substring(0, 1) == "G") { Svi = 1; }
                                        else if (Form1.cmbCM603.Text.Trim().Substring(0, 1) == "B") { Svi = 2; }
                                        else if (Form1.cmbCM603.Text.Trim().Substring(0, 1) == "M") { Svi = 3; }
                                        for (int SvR = 0; SvR < 8; SvR++)
                                        {
                                            for (int svC = 0; svC < 16; svC++)
                                            {
                                                mvars.RegData[Svi, svC + 16 * SvR] = mp.DecToHex(mvars.ReadDataBuffer[6 + 1 + svC + 16 * SvR], 2);
                                            }
                                        }
                                    }
                                    else if (mvars.deviceID.Substring(0,2) == "03")
                                    {
                                        //H512A
                                        Svs = "DONE";
                                        if (uc_cm603.dgvBtn.Text.Trim().Substring(0, 1) == "R") { Svi = 0; }
                                        else if (uc_cm603.dgvBtn.Text.Trim().Substring(0, 1) == "G") { Svi = 1; }
                                        else if (uc_cm603.dgvBtn.Text.Trim().Substring(0, 1) == "B") { Svi = 2; }
                                        for (int SvR = 0; SvR < 8; SvR++)
                                        {
                                            for (int SvC = 0; SvC < 16; SvC++)
                                            {
                                                mvars.RegData[Svi, SvC + 16 * SvR] = mp.DecToHex(mvars.ReadDataBuffer[6 + 1 + SvC + 16 * SvR], 2);
                                                uc_cm603.dgvBin.Rows[SvR].Cells[SvC].Value = mp.DecToHex(mvars.ReadDataBuffer[6 + 1 + SvC + uc_cm603.dgvBin.ColumnCount * SvR], 2);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (mvars.lblCmd.Substring(0, 9) == "IN516_REA")
                                {
                                    #region IN516_REA
                                    Svs = "DONE"; //Svi = 0;
                                    //bool svbreak = false;
                                    //for (int SvR = 0; SvR < Form1.dgvBin.Rows.Count; SvR++)
                                    //{
                                    //    for (int svC = 0; svC < Form1.dgvBin.ColumnCount; svC++)
                                    //    {
                                    //        mvars.in516read[svC + Form1.dgvBin.ColumnCount * SvR] = mp.DecToHex(mvars.ReadDataBuffer[6 + 1 + svC + Form1.dgvBin.ColumnCount * SvR], 2);
                                    //        Form1.dgvBin.Rows[SvR].Cells[svC].Value = mp.DecToHex(mvars.ReadDataBuffer[6 + 1 + svC + Form1.dgvBin.ColumnCount * SvR], 2);
                                    //        Svi++;
                                    //        if (Svi > 0x3A) { svbreak = true; break; }
                                    //    }
                                    //    if (svbreak) { break; }
                                    //}
                                    #endregion
                                }
                                else if (mvars.lblCmd.Substring(0, 9) == "IN525_REA")
                                {
                                    #region IN525_REA 已取消
                                    //Svs = "DONE"; //Svi = 0;
                                    //Array.Clear(uc_in525.gBinArr, 0, 0x4C);
                                    //for (int SvR = 0; SvR < 0x4C; SvR++)
                                    //{
                                    //    uc_in525.gBinArr[SvR] = mvars.ReadDataBuffer[6 + 1 + SvR];
                                    //}
                                    #endregion
                                }
                                else if (mvars.lblCmd.Substring(0, 9) == "GammaSet_") Svs = "DONE";
                                else if (mvars.lblCmd.Substring(0, 9) == "FLASH_REA")
                                {
                                    int PacketSize = 2048;
                                    if (mvars.svnova == false) { PacketSize = 32768; }
                                    Copy(mvars.ReadDataBuffer, 6 + 1, mvars.gFlashRdPacketArr, 0, PacketSize);
                                    Svs = "DONE";
                                }
                                else if (mvars.lblCmd.Substring(0, 9) == "rREGDECAR")
                                {
                                    #region rREGDECARR_起始位址_讀取計數
                                    i = Convert.ToInt16(mvars.lblCmd.Split('_')[0]);
                                    int RegCnt = Convert.ToInt16(mvars.lblCmd.Split('_')[0]);
                                    for (i = 0; i < RegCnt; i++)
                                    {
                                        //DataDec_A[i] = ((UInt16)(INBuffer[6 + i * 2 + 1] * 256 + INBuffer[7 + i * 2 + 1])).ToString();
                                        //DataDec_B[i] = ((UInt16)(INBuffer[6 + i * 2 + RegCnt * 2 + 1] * 256 + INBuffer[7 + i * 2 + RegCnt * 2 + 1])).ToString();
                                    }
                                    #endregion
                                    Svs = "DONE";
                                }
                                else { Svs = "DONE"; }
                                break;
                        }
                    }
                    else
                    {
                        if (mvars.lblCmd == "MCU_RESET") { Svs = "DONE"; }
                    }
                }
                else { Svs = "-1," + WriteStatus; }
                if (Svs == "-1" || Svs.IndexOf("-1,") != -1)
                {
                    mvars.lGet[mvars.lCount] = mvars.lCmd[mvars.lCount] + "ERROR," + mvars.iPBaddr.ToString() + "," + Svs;
                }
                else
                {
                    if (Svs.IndexOf("DONE", 0) != -1) mvars.lGet[mvars.lCount] = mvars.lCmd[mvars.lCount] + "DONE";
                    else mvars.lGet[mvars.lCount] = mvars.lCmd[mvars.lCount] + "DONE," + mvars.iPBaddr.ToString() + "," + Svs;
                }
                if (mvars.flgDelFB == false && mvars.lblCmd != "EndcCMD") 
                { 
                    //mvars.lstget.Items.Add(mvars.lGet[mvars.lCount]); 
                    //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1; 
                }
                mvars.lCount++;
                mvars.flgSend = false; mvars.flgReceived = false;
                //如果c類程序無法確認mvars.lCounts的次數那最後會下EndcMCD指令給tme_RSin去完成最後的SendMessage回傳
                if (mvars.flgSelf == true)
                {
                    mvars.flgSend = false; mvars.flgReceived = false;
                }
                else
                {
                    if (mvars.lCounts == mvars.lCount) { funSendMessage(); }
                    else { mvars.flgSend = false; mvars.flgReceived = false; }
                }
                if (mvars.deviceID == "0400") { doDelayms(100); }
            }
        }
        public static void mhcm603ReadAll(byte svAddr) //c12a 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 25; mvars.NumBytesToRead = 0x80 + 14;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 513) { Array.Clear(mvars.ReadDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 513); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWrRd;       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                      //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                      //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svAddr / 2);        //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                      //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;                      //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;                      //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x00;                      //Read Size
            mvars.RS485_WriteDataBuffer[10 + svns] = 0x80;                     //Read Size
            mp.funSendMessageTo();
        }







        public static void mhVersion()                                              // 0x01 
        {
            #region 2023版公用程序
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x09);
            }
            else
            {
                svns = 1;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 512 + svns);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 65);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[svns + 2] = 0x01;   ///Cmd
            mvars.RS485_WriteDataBuffer[svns + 3] = 0x00;   ///Size
            mvars.RS485_WriteDataBuffer[svns + 4] = 0x09;   ///Size    [svns + 4] + ip + checksum = 9筆data
            mp.funSendMessageTo();
        }

        public static void mhMCUinf()                                              // 0x03 
        {
            #region 2023版公用程序
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x09);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[svns + 2] = 0x03;   ///Cmd
            mvars.RS485_WriteDataBuffer[svns + 3] = 0x00;   ///Size
            mvars.RS485_WriteDataBuffer[svns + 4] = 0x09;   ///Size    [svns + 4] + ip + checksum = 9筆data
            mp.funSendMessageTo();
        }


        public static void mhFLASHTYPE()                                            // 0x04 2022版公用程序加入 
        {
            #region 2022版公用程序
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0A);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 65); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x04;            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0A;            //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)mvars.flashselQ;
            mp.funSendMessageTo();
        }

        public static void OBB_XBS()                                                // 0x05 c12a  最新混合體 
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x09);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[svns + 2] = 0x05;            //Cmd
            mvars.RS485_WriteDataBuffer[svns + 3] = 0x00;            //Size
            mvars.RS485_WriteDataBuffer[svns + 4] = 0x09;            //Size    [svns + 4]=9筆data
            mp.funSendMessageTo();
        }

        public static void mhFPGASPIREAD()                                          // 0x10 2022版公用程序加入 
        {
            // line 4890有使用問題 svverFPGA = mvars.verFPGA.Split('-')[mvars.flashselQ-1];  為什麼使用mvars.flashselQ
            // line 5015有使用問題 
            /*
            if (MultiLanguage.DefaultLanguage == "en-US")
                lst_get1.Items.Add(" -> " + svtitle +
                " write OK but Version check : " + svverFPGA + " >> " + mvars.verFPGA.Split('-')[svflashQ - 1]);
            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                lst_get1.Items.Add(" -> " + svtitle +
                " 寫入完成後版本確認 : " + svverFPGA + " >> " + mvars.verFPGA.Split('-')[svflashQ - 1]);
            else if (MultiLanguage.DefaultLanguage == "zh-CN")
                lst_get1.Items.Add(" -> " + svtitle +
                " 写入完成后版本确认 : " + svverFPGA + " >> " + mvars.verFPGA.Split('-')[svflashQ - 1]);
            */
            #region 2023版公用程序
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0B);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 65);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x10;   ///Cmd
            mvars.RS485_WriteDataBuffer[svns + 3] = 0x00;   ///Size
            mvars.RS485_WriteDataBuffer[svns + 4] = 0x0B;   ///Size    
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)((Form1.pvindex / 256) | 0x80); //Reg Address
            mvars.RS485_WriteDataBuffer[6 + svns] = (byte)(Form1.pvindex % 256);          //Reg Address
            funSendMessageTo();
        }

        public static void mhFPGASPIWRITE(byte svFsel, int svData)                  // 0x11 0500 相容 
        {
            #region 2023版公用程序
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;   
                if (mvars.deviceID.Substring(0, 2) == "05") Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
                else Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0D);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 65);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x11;                           ///Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                           ///Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;                           ///Size
                                                                                    ///byte[] WriteSizeArr = { mvars.RS485_WriteDataBuffer[4 + 1], mvars.RS485_WriteDataBuffer[3 + 1] };
                                                                                    ///UInt16 WriteSize = BitConverter.ToUInt16(WriteSizeArr, 0);
            mvars.RS485_WriteDataBuffer[5 + svns] = svFsel;                         ///0:ABCD 1:EFGH 2:Boardcast
            mvars.RS485_WriteDataBuffer[6 + svns] = (byte)(Form1.pvindex / 256);    ///Reg Address
            mvars.RS485_WriteDataBuffer[7 + svns] = (byte)(Form1.pvindex % 256);    ///Reg Address
            mvars.RS485_WriteDataBuffer[8 + svns] = (byte)(svData / 256);           ///Reg Data
            mvars.RS485_WriteDataBuffer[9 + svns] = (byte)(svData % 256);           ///Reg Data
            mp.funSendMessageTo();
        }

        public static void mhFPGASPIWRITE(byte svFsel, int[] svreg, int[] svdata)   // 0x11 0500 相容 
        {
            #region 2023版公用程序
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                if (mvars.deviceID.Substring(0, 2) == "05") Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + (svreg.Length * 5 + 10));
                else Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + (svreg.Length * 4 + 9));
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 65);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06")
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x11;                                           ///Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = Convert.ToByte((svreg.Length * 5 + 10) / 256);  ///Size
                mvars.RS485_WriteDataBuffer[4 + svns] = Convert.ToByte((svreg.Length * 5 + 10) % 256);  ///Size
                for (int j = 0; j < svreg.Length; j++)
                {
                    mvars.RS485_WriteDataBuffer[5 + (j * 5) + svns] = svFsel;                                               ///0:ABCD 1:EFGH 2:Boardcast
                    mvars.RS485_WriteDataBuffer[6 + (j * 5) + svns] = Convert.ToByte(svreg[j] / 256);                       ///Addr
                    mvars.RS485_WriteDataBuffer[7 + (j * 5) + svns] = Convert.ToByte(svreg[j] % 256);                       ///Addr
                    mvars.RS485_WriteDataBuffer[8 + (j * 5) + svns] = Convert.ToByte(Convert.ToInt32(svdata[j]) / 256);     ///Data
                    mvars.RS485_WriteDataBuffer[9 + (j * 5) + svns] = Convert.ToByte(Convert.ToInt32(svdata[j]) % 256);     ///Data
                }
            }
            else
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x11;                                           //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = Convert.ToByte((svreg.Length * 4 + 9) / 256);   //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = Convert.ToByte((svreg.Length * 4 + 9) % 256);   //Size
                                                                                                        //byte[] WriteSizeArr = { mvars.RS485_WriteDataBuffer[4 + 1], mvars.RS485_WriteDataBuffer[3 + 1] };
                                                                                                        //UInt16 WriteSize = BitConverter.ToUInt16(WriteSizeArr, 0);
                for (int j = 0; j < svreg.Length; j++)
                {
                    mvars.RS485_WriteDataBuffer[5 + (j * 4) + svns] = Convert.ToByte(svreg[j] / 256);                       //Addr
                    mvars.RS485_WriteDataBuffer[6 + (j * 4) + svns] = Convert.ToByte(svreg[j] % 256);                       //Addr
                    mvars.RS485_WriteDataBuffer[7 + (j * 4) + svns] = Convert.ToByte(Convert.ToInt32(svdata[j]) / 256);     //Data
                    mvars.RS485_WriteDataBuffer[8 + (j * 4) + svns] = Convert.ToByte(Convert.ToInt32(svdata[j]) % 256);     //Data
                }
            }



            funSendMessageTo();
        }

        public static void mhFPGASPIWRITE(int svData)                               // 0x11 
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0D);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 65);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x11;          //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;          //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D;          //Size
                                                                   //byte[] WriteSizeArr = { mvars.RS485_WriteDataBuffer[4 + 1], mvars.RS485_WriteDataBuffer[3 + 1] };
                                                                   //UInt16 WriteSize = BitConverter.ToUInt16(WriteSizeArr, 0);
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(Form1.pvindex / 256);            //Reg Address
            mvars.RS485_WriteDataBuffer[6 + svns] = (byte)(Form1.pvindex % 256);            //Reg Address
            mvars.RS485_WriteDataBuffer[7 + svns] = (byte)(svData / 256);  //Reg Data
            mvars.RS485_WriteDataBuffer[8 + svns] = (byte)(svData % 256);  //Reg Data
            mp.funSendMessageTo();
        }

        public static void mhMCUFLASHREAD(string svtxt44, int ReadSize)             // 0x14 C12B
        {
            //UInt16 ReadSize = (UInt16)mvars.MCU_60000_SIZE; //1024,2048,4096,8192,16384 (需與cFLASHWRITE_C12ACB 設定值相同)
            //UInt16 ReadSize = 1024; //1024,2048,4096,8192,16384 (需與cFLASHWRITE_C12ACB 設定值相同)
            byte[] bytes = BitConverter.GetBytes(ReadSize);
            byte[] flash_addr_arr = StringToByteArray(svtxt44);

            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 2; mvars.NumBytesToRead = ReadSize + 65;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 65 + 8192);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x14;                   //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];      //QSPI address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];      //QSPI address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];      //QSPI address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];      //QSPI address.
            mvars.RS485_WriteDataBuffer[9 + svns] = bytes[1];               //Read Size
            mvars.RS485_WriteDataBuffer[10 + svns] = bytes[0];              //Read Size
            funSendMessageTo();
        }

        public static void mhMCUFLASHWRITE(string svtxt44, string svstraddr)        // 0x15 C12A/H5512A 
        {
            UInt16 WriteSize = 2048;
            if (svstraddr == "60000") WriteSize = 1024;
            byte[] WrArr = BitConverter.GetBytes(WriteSize);
            UInt16 PacketSize = (UInt16)(WriteSize + 0x0F);
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            byte[] flash_addr_arr = StringToByteArray(svtxt44);

            //byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            //if (mvars.svnova)
            //{
            //    mvars.Delaymillisec = 500; mvars.NumBytesToRead = 28;
            //    if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            //    Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            //}
            //else
            //{
            //    svns = 1;
            //    if (mvars.RS485_WriteDataBuffer.Length == 513 + 8192) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513 + 32768); }
            //    else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513 + 32768); }
            //    if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
            //    else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            //}

            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 500; mvars.NumBytesToRead = 28;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513 + 8192);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);


            mvars.RS485_WriteDataBuffer[2 + svns] = 0x15;               //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];           //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];  //address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];  //address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];  //address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];  //address
            mvars.RS485_WriteDataBuffer[9 + svns] = WrArr[1];           //Write Size
            mvars.RS485_WriteDataBuffer[10 + svns] = WrArr[0];          //Write Size
            for (UInt16 i = 0; i < WriteSize; i++)                      //Data
            { mvars.RS485_WriteDataBuffer[11 + svns + i] = mvars.gFlashRdPacketArr[i]; }
            funSendMessageTo();
        }

        public static void mhFLASHERASE()                                           // 0x30:CB，0x3B:XB 最新混合體 
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                if (mvars.c12aflashitem == 0) Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
                else if (mvars.c12aflashitem == 1) Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0C);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            if (mvars.c12aflashitem == 0)
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x30;           //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;           //Size
                mvars.RS485_WriteDataBuffer[5 + svns] = 0xC7;           //QSPI EraseSector Command
                mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;           //QSPI address
                mvars.RS485_WriteDataBuffer[7 + svns] = 0x00;           //QSPI address
                mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;           //QSPI address
                mvars.RS485_WriteDataBuffer[9 + svns] = 0x00;           //QSPI address
            }
            else if (mvars.c12aflashitem == 1)
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x3B;           //Cmd(0x3B.SPI Cmd Write)
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0C;           //Size  + Write Size
                mvars.RS485_WriteDataBuffer[5 + svns] = 0x00;           //Write Size
                mvars.RS485_WriteDataBuffer[6 + svns] = 0x01;           //Write Size
                mvars.RS485_WriteDataBuffer[7 + svns] = 0xC7;           //SPI Flash Command:Chip Erase
            }
            mp.funSendMessageTo();
        }

        public static void mhFUNCENABLE()                                           // 0x30:CB Primary，0x3B:XB 最新混合體 
        {
            #region 2022版公用程序
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                if (mvars.c12aflashitem == 0 || mvars.deviceID.Substring(0, 2) == "05") Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
                else if (mvars.c12aflashitem == 1) Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0C);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x30;           //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;           //Size    最後一筆+1, 例.[13]則Size=0x0E 0~13=14
                mvars.RS485_WriteDataBuffer[5 + svns] = 0x06;           //Enable, 完成後會MCU會自行切回Disable(0x04)
                mvars.RS485_WriteDataBuffer[6 + svns] = 0xFF;           //QSPI address
                mvars.RS485_WriteDataBuffer[7 + svns] = 0xFF;           //QSPI address
                mvars.RS485_WriteDataBuffer[8 + svns] = 0xFF;           //QSPI address
                mvars.RS485_WriteDataBuffer[9 + svns] = 0xFF;           //QSPI address
            }
            else
            {
                if (mvars.c12aflashitem == 0)
                {
                    mvars.RS485_WriteDataBuffer[2 + svns] = 0x30;           //Cmd
                    mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                    mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;           //Size    最後一筆+1, 例.[13]則Size=0x0E 0~13=14
                    mvars.RS485_WriteDataBuffer[5 + svns] = 0x06;           //Enable, 完成後會MCU會自行切回Disable(0x04)
                    mvars.RS485_WriteDataBuffer[6 + svns] = 0xFF;           //QSPI address
                    mvars.RS485_WriteDataBuffer[7 + svns] = 0xFF;           //QSPI address
                    mvars.RS485_WriteDataBuffer[8 + svns] = 0xFF;           //QSPI address
                    mvars.RS485_WriteDataBuffer[9 + svns] = 0xFF;           //QSPI address
                }
                else if (mvars.c12aflashitem == 1)
                {
                    mvars.RS485_WriteDataBuffer[2 + svns] = 0x3B;           //Cmd(0x3B.SPI Cmd Write)
                    mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                    mvars.RS485_WriteDataBuffer[4 + svns] = 0x0C;           //Size  + Write Size
                    mvars.RS485_WriteDataBuffer[5 + svns] = 0x00;           //Write Size
                    mvars.RS485_WriteDataBuffer[6 + svns] = 0x01;           //Write Size
                    mvars.RS485_WriteDataBuffer[7 + svns] = 0x06;           //SPI Flash Command:
                }
            }
            mp.funSendMessageTo();
        }

        public static void mhFUNCSTATUS()                                           // 0x31:CB，0x3A:XB 最新混合體 
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 50; mvars.NumBytesToRead = 28;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                if (mvars.c12aflashitem == 0) Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0C);
                else if (mvars.c12aflashitem == 1) Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            if (mvars.c12aflashitem == 0)   //CB
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x31;           //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0C;           //Size
                mvars.RS485_WriteDataBuffer[5 + svns] = 0x05;           //QSPI Read Status Command
                mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;           //Read Size
                mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;           //Read Size
            }
            else if (mvars.c12aflashitem == 1)  //XB
            {
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x3A;           //Cmd(0x3A.SPI Cmd Read)
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;           //Size  + Write Size
                mvars.RS485_WriteDataBuffer[5 + svns] = 0x00;           //Write Size
                mvars.RS485_WriteDataBuffer[6 + svns] = 0x01;           //Write Size
                mvars.RS485_WriteDataBuffer[7 + svns] = 0x05;           //SPI Flash Command:Read Status
                mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;           //Read Size
                mvars.RS485_WriteDataBuffer[9 + svns] = 0x01;           //Read Size
            }
            mp.funSendMessageTo();
        }

        public static void mSPI_READJEDECID()                                       // 0x31 2022版公用程序加入 
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 20; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0C);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 513); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x31;           //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0C;           //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x9F;           //QSPI ReadJedecId Command
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;           //Read Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;           //Read Size
            mp.funSendMessageTo();
        }



        public static void mhFUNCQE()                                               // 0x32 2022版公用程序加入 
        {
            #region 2022版公用程序
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 513); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x32;            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;            //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x01;            //QSPI Quad Enable
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;            //QSPI address
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x02;            //QSPI address
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x40;            //QSPI address
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x00;            //QSPI address
            mp.funSendMessageTo();
        }


        public static void mhcm603WriteSingleReg(byte svAddr, byte ucReg, byte ucSize, byte dataH, byte dataL) //c12a 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + (0x0C + ucSize + 1));
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;         //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = (byte)((0x0C + ucSize + 1) / 256);    //Size(Datacount)
            mvars.RS485_WriteDataBuffer[4 + svns] = (byte)((0x0C + ucSize + 1) % 256);    //Size(Datacount)
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svAddr / 2);        //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                      //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = (byte)(ucSize + 1);        //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = (byte)(ucReg);             //Register 0x00~3F
            int Svi = 0;
            for (int i = ucReg; i < (ucReg + ucSize); i++)
            {
                if (Svi % 2 == 0)    //HiByte + 0x4000 || 0x8000
                {
                    if (mvars.UUT.MTP == 1) { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = (byte)(dataH | 0x40); }
                    else { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = (byte)(dataH | 0x80); }
                }
                else { mvars.RS485_WriteDataBuffer[9 + svns + (i - ucReg)] = dataL; }  //LoByte 直上
                Svi++;
            }
            mp.funSendMessageTo();
        }
        public static void mhcm603lockWP1(byte svWRAddr) //c12a 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;                   //Register
            UInt16 u16Val = mp.RegVal(0x0000);
            mvars.RS485_WriteDataBuffer[9 + svns] = (byte)(u16Val / 256);
            mvars.RS485_WriteDataBuffer[10 + svns] = (byte)(u16Val % 256);
            mp.funSendMessageTo();
        }
        public static void mhcm603lockWP2(byte svWRAddr) //c12a 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x01;                   //Register
            UInt16 u16Val = mp.RegVal(0x0000);
            mvars.RS485_WriteDataBuffer[9 + svns] = (byte)(u16Val / 256);
            mvars.RS485_WriteDataBuffer[10 + svns] = (byte)(u16Val % 256);
            mp.funSendMessageTo();
        }
        
        public static void mhMCUFLASHWRITE(byte svcmd)                     //0x28 Custom_UI
        {
            //UInt16 WriteSize = 1024;
            //byte[] WrArr = BitConverter.GetBytes(WriteSize);
            UInt16 PacketSize = (UInt16)(9 + 512);
            byte[] bytes = BitConverter.GetBytes(PacketSize);

            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 500; mvars.NumBytesToRead = 28;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 1024);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);


            mvars.RS485_WriteDataBuffer[2 + svns] = svcmd;              //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];           //Size
            for (UInt16 i = 0; i < mvars.GAMMA_SIZE; i++) { mvars.RS485_WriteDataBuffer[5 + svns + i] = mvars.gFlashRdPacketArr[i]; }              
            funSendMessageTo();
        }



        
        public static void mhMCUFLASHREAD(byte svcmd)                      //0x27(0x60000) 0x29(0x62000)
        {
            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 2; mvars.NumBytesToRead = 1024 + 65;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x09);
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 1024);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = svcmd;                   //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x09;                   //Size
            funSendMessageTo();
        }


        
        public static UInt16 RegVal(UInt16 Val)   //public static UInt16 RegVal(bool svMTP, UInt16 Val)
        {
            UInt16 u16DacMTP = 0x8000;
            if (mvars.UUT.MTP == 1) { u16DacMTP = 0x4000; }
            UInt32 ulData = Val, ulTmp;
            UInt32 ulCRC = CRC_Cal(ulData, (UInt16)BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
            ulTmp = ulData + ulCRC + u16DacMTP;
            return (UInt16)ulTmp;
        }
        public static void mhcm603UnlockWP1(byte svWRAddr) //c12a 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;                   //Register
            UInt16 u16Val = mp.RegVal(0x0296);
            mvars.RS485_WriteDataBuffer[9 + svns] = (byte)(u16Val / 256);   //0x80
            mvars.RS485_WriteDataBuffer[10 + svns] = (byte)(u16Val % 256);  //0x80
            mp.funSendMessageTo();
        }
        public static void mhcm603UnlockWP2(byte svWRAddr) //c12a 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2; mvars.NumBytesToRead = 14;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWr;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svWRAddr / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                   //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;                   //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x01;                   //Register
            UInt16 u16Val = mp.RegVal(0x0169);
            mvars.RS485_WriteDataBuffer[9 + svns] = (byte)(u16Val / 256);
            mvars.RS485_WriteDataBuffer[10 + svns] = (byte)(u16Val % 256);
            mp.funSendMessageTo();
        }


        



        public static bool fileDefaultGammaV(bool svWrite)
        {
            int svi;
            int svj;
            mvars.cm603Gamma = new int[13] { 0, 1, 8, 16, 24, 40, 64, 96, 128, 160, 192, 224, 255 };
            mvars.GMAterminals = mvars.cm603Gamma.Length - 1;
            mvars.GMAvalue = (float)3.2;
            Array.Resize(ref mvars.Gamma2d2, mvars.cm603Gamma.Length);
            for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
            {
                /// GMA12　gray1
                if (svi == 1)
                {
                    if (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "10")
                        mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), 2.2));          /// 1
                    else
                        mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), 2.3));          /// 1
                }
                else if (svi == 2) mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), 3));       /// 8
                //else if (svi == 3) mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), 3.0));   /// 16
                //else if (svi == 4) mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), 3.0));   /// 24
                //else if (svi == 5) mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), 3.2));   /// 40
                //else if (svi == 6) mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), 3.2));   /// 64
                else mvars.Gamma2d2[svi] = Convert.ToSingle(100 * Math.Pow(Math.Round((double)mvars.cm603Gamma[svi] / 255, 8), mvars.GMAvalue));

                string fnum;
                if (mvars.Gamma2d2[svi] >= 10) { fnum = String.Format("{0:##0.0}", mvars.Gamma2d2[svi]); }
                else if (mvars.Gamma2d2[svi] >= 1 && mvars.Gamma2d2[svi] < 10) { fnum = String.Format("{0:##0.0#}", mvars.Gamma2d2[svi]); }
                else if (mvars.Gamma2d2[svi] >= 0.1 && mvars.Gamma2d2[svi] < 1) { fnum = String.Format("{0:##0.0##}", mvars.Gamma2d2[svi]); }
                else { fnum = String.Format("{0:##0.0######}", mvars.Gamma2d2[svi]); }
                mvars.Gamma2d2[svi] = Convert.ToSingle(fnum);
            }
            mvars.Gamma2d2[0] = 0;
            mvars.Gamma2d2[mvars.cm603Gamma.Length - 1] = 100;

            if (svWrite)
            {
                FileInfo copyFile = new FileInfo(mvars.defaultGammafile);
                if (copyFile.Exists) { copyFile.Delete(); }
                StreamWriter sTAwrite = File.CreateText(mvars.defaultGammafile);
                sTAwrite.WriteLine("");
                sTAwrite.WriteLine("'-- TFT-Vref_" + mvars.UUT.VREF);


                if (mvars.deviceID.Substring(0,2)=="10")
                    sTAwrite.WriteLine("'-- MVref_" + mvars.cm603Vref[0]);
                else
                    sTAwrite.WriteLine("'-- MVref_" + mvars.cm603Vref[3]);


                svi = 0;
                svj = 0;
                string[,] svgmavolt = new string[3, 13];
                int[] svvrefcode = new int[4];
                if (mvars.deviceNameSub == "B(4)" || 
                    mvars.deviceID.Substring(0, 2) == "05" ||
                    mvars.deviceID.Substring(0, 2) == "06" ||
                    mvars.deviceID.Substring(0, 2) == "10")
                {
                    int svgmacode;
                    for (svi = 0; svi < 3; svi++)
                    {
                        svvrefcode[svi] = (int)(Math.Round(mvars.cm603Vref[svi] / 0.01953, 0));
                        sTAwrite.WriteLine("'-- " + mvars.msrColor[svi + 2].Substring(0, 1) + "Vref_" + mvars.cm603Vref[svi]);
                        for (Form1.pvindex = 0x02; Form1.pvindex <= 0x0D; Form1.pvindex++)
                        {
                            svj = svgmavolt.GetLength(1) - Form1.pvindex + 1;
                            svgmacode = mp.BinToDec(mp.DecToBin(mvars.cm603dfB[svi, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svi, Form1.pvindex * 2 + 1], 16).ToString().Substring(6, 10));
                            svgmavolt[svi, svj] = (Math.Round(((0.01953 * svvrefcode[svi] * svgmacode / 1024)*10000),0)/10000).ToString("##0.0000");
                            sTAwrite.WriteLine(" " + mvars.msrColor[svi + 2].Substring(0, 1) + " GMA_" + svj.ToString("00") + ":volt_" + svgmavolt[svi, svj]);
                        }
                        /// cm603M 0灰階最低電壓
                        byte svcm603M = 3;
                        svvrefcode[svcm603M] = (int)(Math.Round(mvars.cm603Vref[svcm603M] / 0.01953, 0));
                        Form1.pvindex = svi + 2;
                        svj = 0;
                        svgmacode = mp.BinToDec(mp.DecToBin(mvars.cm603dfB[svcm603M, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svcm603M, Form1.pvindex * 2 + 1], 16).ToString().Substring(6, 10));
                        svgmavolt[svi, svj] = (Math.Round(((0.01953 * svvrefcode[svcm603M] * svgmacode / 1024) * 10000), 0) / 10000).ToString("##0.0000");
                        sTAwrite.WriteLine(" " + mvars.msrColor[svi + 2].Substring(0, 1) + " GMA_" + svj.ToString("00") + ":volt_" + svgmavolt[svi, svj]);

                        sTAwrite.WriteLine(" " + mvars.msrColor[svi + 2].Substring(0, 1) + " dvg_01:volt_" + mvars.cgma1dv[mvars.dualduty, svi]);
                    }
                }
                sTAwrite.Flush(); //清除緩衝區
                sTAwrite.Close(); //關閉檔案
                return true;
            }
            else
            {
                if (File.Exists(mvars.defaultGammafile))
                {
                    string[,] svgmavolt0 = new string[3, mvars.cm603Gamma.Length];
                    string[,] svgmavolt1 = new string[3, mvars.cm603Gamma.Length];
                    mvars.pGMA[0].Data = new string[3, mvars.cm603Gamma.Length * 2];
                    #region mvars.cm603df
                    mvars.cm603df = new string[,]
                    {
                        {   "00", "00", "00", "00", "39", "1D", "20", "FB", "1C", "DB", "24", "B9", "10", "A0", "1C", "87",
                            "3C", "6F", "20", "56", "08", "43", "3C", "30", "08", "1C", "38", "0B", "00", "00", "00", "00",
                            "00", "00", "00", "00", "29", "25", "00", "00", "00", "00", "02", "3F", "3B", "5B", "25", "F0",
                            "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "11", "08", "21", "66",
                            "1E", "AB", "34", "55", "02", "19", "11", "FA", "39", "DA", "0D", "BA", "25", "9A", "25", "7B",
                            "05", "54", "0D", "22", "00", "00", "15", "AB", "00", "00", "00", "00", "00", "00", "00", "00",
                            "29", "25", "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                            "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "18", "02", "00", "00"
                        },
                        {   "00", "00", "00", "00", "19", "58", "29", "25", "18", "F0", "10", "CA", "08", "A2", "08", "84",
                            "04", "64", "30", "48", "10", "38", "08", "29", "34", "19", "38", "0B", "00", "00", "00", "00",
                            "00", "00", "00", "00", "29", "25", "00", "00", "00", "00", "02", "3F", "3B", "5B", "25", "F0",
                            "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "11", "08", "21", "66",
                            "19", "58", "29", "25", "18", "F0", "10", "CA", "08", "A2", "08", "84", "04", "64", "30", "48",
                            "00", "00", "00", "00", "2E", "68", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                            "29", "25", "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                            "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "18", "02", "00", "00"
                        },
                        {   "00", "00", "00", "00", "0D", "48", "11", "1B", "08", "EE", "10", "CA", "38", "A6", "1C", "87",
                            "10", "67", "28", "4A", "08", "3A", "1C", "2A", "34", "19", "38", "0B", "00", "00", "00", "00",
                            "00", "00", "00", "00", "29", "25", "00", "00", "00", "00", "02", "3F", "3B", "5B", "25", "F0",
                            "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "11", "08", "21", "66",
                            "1E", "AB", "34", "55", "02", "19", "11", "FA", "39", "DA", "0D", "BA", "25", "9A", "25", "7B",
                            "05", "54", "0D", "22", "00", "00", "15", "AB", "00", "00", "00", "00", "00", "00", "00", "00",
                            "29", "25", "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                            "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "14", "03", "00", "00"
                        },
                        {   "00", "00", "00", "00", "20", "09", "20", "09", "20", "09", "20", "09", "20", "09", "20", "09",
                            "20", "09", "20", "09", "20", "09", "20", "09", "20", "09", "38", "0B", "00", "00", "00", "00",
                            "00", "00", "00", "00", "29", "25", "00", "00", "00", "00", "02", "3F", "3B", "5B", "25", "F0",
                            "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "11", "08", "21", "66",
                            "1E", "AB", "34", "55", "02", "19", "11", "FA", "39", "DA", "0D", "BA", "25", "9A", "25", "7B",
                            "05", "54", "0D", "22", "00", "00", "15", "AB", "00", "00", "00", "00", "00", "00", "00", "00",
                            "29", "25", "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                            "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "14", "03", "00", "00"
                        }
                    };
                    if (mvars.deviceID.Substring(0,2) == "10")
                    {
                        mvars.cm603df = new string[,]{{ "00", "00", "00", "00", "0D", "6E", "11", "FA", "31", "D5", "09", "B4", "39", "96", "2D", "74",
                                                        "09", "55", "25", "37", "35", "1C", "3C", "F7", "20", "DD", "10", "EC", "00", "00", "00", "00",
                                                        "00", "00", "00", "00", "1E", "20", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                                        "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "21", "66",
                                                        "0D", "6E", "11", "FA", "31", "D5", "09", "B4", "39", "96", "2D", "74", "09", "55", "25", "37",
                                                        "35", "1C", "3C", "F7", "20", "DD", "10", "EC", "00", "00", "00", "00", "00", "00", "00", "00",
                                                        "1E", "20", "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "1B", "FF",
                                                        "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "18", "02", "00", "00" },

                                                      { "00", "00", "00", "00", "0D", "6E", "0A", "16", "25", "E3", "15", "B8", "0D", "8F", "31", "6B",
                                                        "2D", "41", "15", "20", "1D", "09", "20", "FB", "1C", "ED", "34", "DE", "00", "00", "00", "00",
                                                        "00", "00", "00", "00", "06", "48", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                                        "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "21", "66",
                                                        "0D", "6E", "0A", "16", "25", "E3", "15", "B8", "31", "6B", "29", "9B", "2D", "41", "15", "20",
                                                        "1D", "09", "20", "FB", "1C", "ED", "34", "DE", "00", "00", "00", "00", "00", "00", "00", "00",
                                                        "06", "48", "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "1B", "FF",
                                                        "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "18", "02", "00", "00" },

                                                      { "00", "00", "00", "00", "08", "1C", "3A", "12", "25", "E3", "15", "B8", "15", "8D", "29", "69",
                                                        "2D", "41", "39", "1D", "11", "08", "04", "FC", "00", "F2", "10", "EC", "00", "00", "00", "00",
                                                        "00", "00", "00", "00", "0E", "3E", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00",
                                                        "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "21", "66",
                                                        "08", "1C", "3A", "12", "25", "E3", "15", "B8", "15", "8D", "0E", "3E", "2D", "41", "39", "1D",
                                                        "11", "08", "04", "FC", "00", "F2", "10", "EC", "00", "00", "00", "00", "00", "00", "00", "00",
                                                        "00", "3E", "00", "00", "1B", "FF", "00", "00", "00", "00", "00", "00", "00", "00", "1B", "FF",
                                                        "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "14", "03", "00", "00" }};
                        mvars.cm603dfB = new byte[3, 128];
                        mvars.cm603Vref = new float[3];         //[0]R,[1]G,[2]B
                    }
                    #endregion mvars.cm603df
                    mvars.cm603dfB = new byte[4, 128];
                    mvars.cm603Vref = new float[4];         //[0]R,[1]G,[2]B,[3]M

                    if (mvars.dualduty > 0) mvars.pGMA[1].Data = new string[mvars.pGMA[0].Data.GetLength(0), mvars.pGMA[0].Data.GetLength(1)];
                    bool svbk1 = false;
                    StreamReader sTAread = File.OpenText(mvars.defaultGammafile);
                    svi = 0;
                    svj = 0;
                    while (true)
                    {
                        string data = sTAread.ReadLine();
                        if (data == null) { break; }
                        if (data != null && data != "")
                        {
                            if (svbk1 == false)
                            {
                                if (data.ToUpper().IndexOf("TFT-VREF") != -1)
                                {
                                    string[] Svs1 = data.Split('_');
                                    mvars.UUT.VREF = float.Parse(Svs1[1]);
                                }
                                if (data.ToUpper().Trim().IndexOf("RVREF_") != -1) { svi = 0; string[] Svs1 = data.Split('_'); mvars.cm603Vref[svi] = float.Parse(Svs1[1]); }
                                else if (data.ToUpper().Trim().IndexOf("GVREF_") != -1) { svi = 1; string[] Svs1 = data.Split('_'); mvars.cm603Vref[svi] = float.Parse(Svs1[1]); }
                                else if (data.ToUpper().Trim().IndexOf("BVREF_") != -1) { svi = 2; string[] Svs1 = data.Split('_'); mvars.cm603Vref[svi] = float.Parse(Svs1[1]); }
                                else if (data.ToUpper().Trim().IndexOf("MVREF_") != -1 && 
                                    (mvars.deviceID.Substring(0, 2) == "05" ||
                                     mvars.deviceID.Substring(0, 2) == "06")) 
                                { 
                                    svi = 3; 
                                    string[] Svs1 = data.Split('_'); 
                                    mvars.cm603Vref[svi] = float.Parse(Svs1[1]); 
                                }

                                string sv = data.ToUpper().Replace(" ", "");
                                if (sv.Substring(0, 5) == mvars.msrColor[svi + 2].Substring(0, 1) + "GMA_" && sv.IndexOf(":VOLT_") != -1)
                                {
                                    string[] svsp = data.Split(':');
                                    svgmavolt0[svi, int.Parse(svsp[0].Split('_')[1])] = svsp[1].Split('_')[1];
                                    svj++;
                                }
                                else if (sv.Substring(0, 5) == mvars.msrColor[svi + 2].Substring(0, 1) + "DVG_" && sv.IndexOf(":VOLT_") != -1)
                                {
                                    string[] svsp = data.Split(':');
                                    mvars.cgma1dv[mvars.dualduty, svi] = float.Parse(svsp[1].Split('_')[1]);
                                }
                            }
                            else
                            {
                                string sv = data.ToUpper().Replace(" ", "");
                                if (sv.Substring(0, 5) == mvars.msrColor[svi + 2].Substring(0, 1) + "GMA_" && sv.IndexOf(":VOLT_") != -1)
                                {
                                    string[] svsp = data.Split(':');
                                    svgmavolt1[svi, int.Parse(svsp[0].Split('_')[1])] = svsp[1].Split('_')[1];
                                    svj++;
                                }
                                else if (sv.Substring(0, 5) == mvars.msrColor[svi + 2].Substring(0, 1) + "DVG_" && sv.IndexOf(":VOLT_") != -1)
                                {
                                    string[] svsp = data.Split(':');
                                    mvars.cgma1dv[mvars.dualduty, svi] = float.Parse(svsp[1].Split('_')[1]);
                                }
                            }
                        }
                    }
                    sTAread.Close();

                    /// pGMA轉cm603df/dfB
                    UInt32 ulCRC;
                    if (mvars.deviceNameSub == "B(4)" || 
                        mvars.deviceID.Substring(0, 2) == "05" || 
                        mvars.deviceID.Substring(0, 2) == "06")
                    {
                        mvars.cm603Gamma = new int[13] { 0, 1, 8, 16, 24, 40, 64, 96, 128, 160, 192, 224, 255 };
                        mvars.GMAterminals = mvars.cm603Gamma.Length - 1;

                        /// 電壓轉階數
                        byte svcm603M = 3;
                        int svgmacode;

                        for (byte svdd = 0; svdd <= mvars.dualduty; svdd++)
                        {
                            for (svi = 0; svi < 3; svi++)
                            {
                                uc_atg.svG0volt[svi] = Convert.ToSingle(svgmavolt0[svi, 0]);

                                for (Form1.pvindex = 0x02 + 30 * svdd; Form1.pvindex <= 0x0D + 30 * svdd; Form1.pvindex++)
                                {
                                    svj = svgmavolt0.GetLength(1) - (Form1.pvindex - 30 * svdd) + 1;                                    /// duty0:13-2+1=12  duty1:13-(32-30)+1=12
                                    svgmacode = Convert.ToInt16(float.Parse(svgmavolt0[svi, svj]) * 1024 / mvars.cm603Vref[svi]);
                                    mvars.pGMA[svdd].Data[svi, svj * 2] = mp.DecToHex(svgmacode, 4).Substring(0, 2);
                                    mvars.pGMA[svdd].Data[svi, svj * 2 + 1] = mp.DecToHex(svgmacode, 4).Substring(2, 2);

                                    ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                                }
                                /// cm603M 0灰階最低電壓
                                Form1.pvindex = svi + 2 + 30 * svdd;    /// R-GMA13:0x02,G-GMA13:0x03,B-GMA13:0x04    R:0x20,G:0x21,B:0x22
                                svj = 0;
                                svgmacode = Convert.ToInt16(float.Parse(svgmavolt0[svi, svj]) * 1024 / mvars.cm603Vref[svcm603M]);
                                mvars.pGMA[svdd].Data[svi, svj * 2] = mp.DecToHex(svgmacode, 4).Substring(0, 2);
                                mvars.pGMA[svdd].Data[svi, svj * 2 + 1] = mp.DecToHex(svgmacode, 4).Substring(2, 2);
                                ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                mvars.cm603df[svcm603M, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2]);
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1]);
                                ///
                                Form1.pvindex = 0x12 + 30 * svdd;   //VCOM1-BK0:TFT-VREF
                                svgmacode = Convert.ToInt16(mvars.UUT.VREF * 1024 / mvars.cm603Vref[svi]);
                                ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                                ///cm603M
                                mvars.cm603df[svcm603M, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2]);
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1]);
                                ///
                                Form1.pvindex = 0x18 + 25 * svdd;   //VCOM1MIN-BK0  VCOM1MIN-BK1
                                mvars.cm603df[svi, Form1.pvindex * 2] = "00";
                                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = "00";
                                mvars.cm603dfB[svi, Form1.pvindex * 2] = 0;
                                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = 0;
                                ///cm603M
                                mvars.cm603df[svcm603M, Form1.pvindex * 2] = "00";
                                mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1] = "00";
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2] = 0;
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2 + 1] = 0;
                                ///
                                Form1.pvindex = 0x19 + 25 * svdd;   //VCOM1MAX-BK0  VCOM1MAX-BK1
                                svgmacode = 1023;
                                ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                                ///cm603M
                                mvars.cm603df[svcm603M, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2]);
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1]);
                                ///
                                Form1.pvindex = 0x1F;   //VREF:Gamma-VREF
                                svgmacode = Convert.ToInt16(mvars.cm603Vref[svi] / 0.01953);

                                ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);

                                mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                                ///cm603M
                                mvars.cm603df[svcm603M, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2]);
                                mvars.cm603dfB[svcm603M, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svcm603M, Form1.pvindex * 2 + 1]);
                            }
                        }
                        mvars.RegData = new string[4, 128];
                        Array.Copy(mvars.cm603df, mvars.RegData, mvars.cm603df.Length);    //新cm603 bin 模式
                    }
                    else if (mvars.deviceID.Substring(0, 2) == "10")
                    {
                        mvars.cm603Gamma = new int[13] { 0, 1, 8, 16, 24, 40, 64, 96, 128, 160, 192, 224, 255 };
                        mvars.GMAterminals = mvars.cm603Gamma.Length - 1;

                        // 電壓轉階數
                        for (byte svdd = 0; svdd <= mvars.dualduty; svdd++)
                        {
                            for (svi = 0; svi < 3; svi++)
                            {
                                uc_atg.svG0volt[svi] = Convert.ToSingle(svgmavolt0[svi, 0]);
                                //電壓依序遞減(GMA_11:volt_ ~ GMA_01:volt_)  03h(CM603 pin:10 name:Vr2) ~0Dh(CM603 pin:22 name:Vr12
                                for (Form1.pvindex = 0x03 + 30 * svdd; Form1.pvindex <= 0x0D + 30 * svdd; Form1.pvindex++)
                                {
                                    svj = svgmavolt0.GetLength(1) - (Form1.pvindex - 30 * svdd) + 1;                                    // duty0: svj=13-0x03+1=11  duty1: svj=13-(33-30)+1=11
                                    int svgmacode = Convert.ToInt16(float.Parse(svgmavolt0[svi, svj]) * 1024 / mvars.cm603Vref[svi]);   // [svi,0}=0.1364, [svi,12]=3.307, [svi,12]=3.82
                                    mvars.pGMA[svdd].Data[svi, svj * 2] = mp.DecToHex(svgmacode, 4).Substring(0, 2);
                                    mvars.pGMA[svdd].Data[svi, svj * 2 + 1] = mp.DecToHex(svgmacode, 4).Substring(2, 2);

                                    ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                                }

                                #region 最高電壓(GMA_12:volt_) 由 VCOM(12h,pin24) 取代 Vr1(02h,pin9)，先把VCOMmin(18h)設為0=0x0000, VCOMmax(19h)設為1023=0x03FF(pGMA)=0x1BFF(cm603df)
                                for (Form1.pvindex = 0x18 + 30 * svdd; Form1.pvindex <= 0x18 + 30 * svdd; Form1.pvindex++)
                                {
                                    int svgmacode = 0;
                                    ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                                }

                                for (Form1.pvindex = 0x19 + 30 * svdd; Form1.pvindex <= 0x19 + 30 * svdd; Form1.pvindex++)
                                {
                                    int svgmacode = 1023;
                                    ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                                    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                                    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                                }
                                for (Form1.pvindex = 0x12 + 30 * svdd; Form1.pvindex <= 0x12 + 30 * svdd; Form1.pvindex++)
                                {
                                    svj = 12;                                                                                           // duty0: svj=13-2+1=12  duty1: svj=13-(32-30)+1=12
                                    int svgmacode = Convert.ToInt16(float.Parse(svgmavolt0[svi, svj]) * 1024 / mvars.cm603Vref[svi]);
                                    mvars.pGMA[svdd].Data[svi, svj * 2] = mp.DecToHex(svgmacode, 4).Substring(0, 2);
                                    mvars.pGMA[svdd].Data[svi, svj * 2 + 1] = mp.DecToHex(svgmacode, 4).Substring(2, 2);

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
                    }
                    return true;
                }
                else { return false; }
            }
        }


        public static bool fileDefaultGammaAIO(bool svWrite, string fullname)
        {
            if (svWrite)
            {
                FileInfo copyFile = new FileInfo(fullname);
                if (copyFile.Exists) { copyFile.Delete(); }
                StreamWriter sTAwrite = File.CreateText(fullname);
                sTAwrite.WriteLine("");
                int i;
                for (i = 0; i < 3; i++)
                {
                    sTAwrite.WriteLine("        '-- " + mvars.msrColor[i + 2].Substring(0, 1) + "Vref_" + mvars.cm603Vref[i]);
                    for (int j = 0; j < mvars.cm603Gamma.Length * 2; j++)
                    {
                        if (j > 0 && j % 2 == 0) { sTAwrite.WriteLine(""); }
                        sTAwrite.WriteLine("          cm603gray(" + i.ToString() + "," + j.ToString() + ") = \"" + mp.DecToHex(mvars.cm603Gamma[j / 2], 4).Substring((j % 2) * 2, 2) +
                            "\": Data(" + i.ToString() + "," + j.ToString() + ") = \"" + mvars.pGMA[mvars.dualduty].Data[i, j] + "\"");
                    }
                    sTAwrite.WriteLine("");
                    sTAwrite.WriteLine("");
                }

                sTAwrite.WriteLine("        '-- MVref_" + mvars.cm603Vref[i]);

                sTAwrite.Flush(); //清除緩衝區
                sTAwrite.Close(); //關閉檔案
                return true;
            }
            else
            {
                if (File.Exists(fullname))
                {
                    bool svbk0 = false;

                    StreamReader sTAread = File.OpenText(fullname);
                    int svi = 0; 
                    while (true)
                    {
                        string data = sTAread.ReadLine();
                        if (data == null) { break; }
                        if (data != null && data != "")
                        {
                            if (svbk0 == false) { if (data.ToUpper().IndexOf("RVREF_") != -1) { svbk0 = true; svi = 0; } }
                            if (svbk0)
                            {
                                int i = svi / (mvars.cm603Gamma.Length * 2);
                                int j = svi % (mvars.cm603Gamma.Length * 2);
                                if (data.ToUpper().IndexOf("CM603GRAY(" + i.ToString() + "," + j.ToString(), 0) != -1 && data.ToUpper().IndexOf("DATA(" + i.ToString() + "," + j.ToString(), 0) != -1)
                                {
                                    string[] Svs1 = data.Split('=', '\r', '"');
                                    mvars.pGMA[mvars.dualduty].Data[i, j] = Svs1[5];
                                    svi++;
                                }
                                else if (data.ToUpper().IndexOf(mvars.msrColor[i + 2].Substring(0, 1) + "VREF_") != -1)
                                {
                                    mvars.cm603Vref[i] = Convert.ToSingle(data.Substring(("        '-- XVREF_").Length, data.Length - ("        '-- XVREF_").Length));
                                    //mvars.cm603VrefCode[i] = Convert.ToInt16(Math.Round(mvars.cm603Vref[i] / 0.01953, 0));
                                }
                                else if (data.ToUpper().IndexOf("MVREF_") != -1)
                                {
                                    mvars.cm603Vref[i] = Convert.ToSingle(data.Substring(("        '-- XVREF_").Length, data.Length - ("        '-- XVREF_").Length));
                                    //mvars.cm603VrefCode[i] = Convert.ToInt16(Math.Round(mvars.cm603Vref[i] / 0.01953, 0));
                                }
                            }
                        }
                    }
                    sTAread.Close();

                    //pGMA轉cm603df/dfB
                    int sH;
                    int sL;
                    int svtr;
                    UInt32 ulCRC;
                    if (mvars.deviceNameSub == "B(4t)")
                    {
                        for (svi = 0; svi < 3; svi++)
                        {
                            for (Form1.pvindex = 0x02; Form1.pvindex <= 0x0D; Form1.pvindex++)
                            {
                                sH = (mvars.pGMA[mvars.dualduty].Data.Length / 3) - (Form1.pvindex - 0x02) * 2 - 4;     //26 - (0x02-0x02)*2-4 = 22
                                sL = (mvars.pGMA[mvars.dualduty].Data.Length / 3) - (Form1.pvindex - 0x02) * 2 - 3;     //26 - (0x02-0x02)*2-3 = 23
                                svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                                ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                            }
                        }
                        //最高灰階最高電壓
                        sH = 24;
                        sL = 25;
                        Form1.pvindex = 2;  //R
                        svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sL]); //[0,24][0,25]
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);

                        Form1.pvindex = 3;  //G
                        svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sL]);     //[1,24][1,25]
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);

                        Form1.pvindex = 4;  //B
                        svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sL]);     //[2,24][2,25]
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    else if (mvars.deviceNameSub == "B(4)" || mvars.deviceID.Substring(0, 2) == "05")
                    {
                        string svname = fullname.Substring(0, fullname.Length - ".gma".Length) + "_V.txt";
                        StreamWriter sTAwrite = File.CreateText(svname);

                        for (svi = 0; svi < 3; svi++)
                        {
                            string[,] svgmavolt = new string[3, 15];
                            string svss = "";
                            for (Form1.pvindex = 0x02; Form1.pvindex <= 0x0D; Form1.pvindex++)
                            {
                                sH = (mvars.pGMA[mvars.dualduty].Data.Length / 3) - (Form1.pvindex - 0x02) * 2 - 2;     //26 - (0x02-0x02)*2-2 = 24
                                sL = (mvars.pGMA[mvars.dualduty].Data.Length / 3) - (Form1.pvindex - 0x02) * 2 - 1;     //26 - (0x02-0x02)*2-1 = 25
                                svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                                ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                                mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                                int svgmacode = mp.BinToDec(mp.DecToBin(mvars.cm603dfB[svi, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svi, Form1.pvindex * 2 + 1], 16).ToString().Substring(6, 10));
                                svgmavolt[svi, Form1.pvindex] = (Math.Round(((mvars.cm603Vref[svi] * svgmacode) * 1), 0) / 1000).ToString("##0.0000");
                                svss += "," + svgmavolt[svi, Form1.pvindex];
                            }
                            sTAwrite.WriteLine(svi + svss);
                            //string[,] svgmavolt = new string[3, 13];
                            //for (Form1.pvindex = 0x02; Form1.pvindex <= 0x0D; Form1.pvindex++)
                            //{
                            //    svj = svgmavolt.GetLength(1) - Form1.pvindex + 1;
                            //    svgmacode = mp.BinToDec(mp.DecToBin(mvars.cm603dfB[svi, Form1.pvindex * 2] * 256 + mvars.cm603dfB[svi, Form1.pvindex * 2 + 1], 16).ToString().Substring(6, 10));
                            //    svgmavolt[svi, svj] = (Math.Round(((0.01953 * svvrefcode[svi] * svgmacode / 1024) * 10000), 0) / 10000).ToString("##0.0000");
                            //    sTAwrite.WriteLine(" " + mvars.msrColor[svi + 2].Substring(0, 1) + " GMA_" + svj.ToString("00") + ":volt_" + svgmavolt[svi, svj]);
                            //}
                        }
                        sTAwrite.Flush(); //清除緩衝區
                        sTAwrite.Close(); //關閉檔案
                        //0灰階最低電壓
                        sH = 0;
                        sL = 1;
                        Form1.pvindex = 2;  //R
                        svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sL]); //[0,0][0,1]
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);

                        Form1.pvindex = 3;  //G
                        svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sL]);     //[1,0][1,1]
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);

                        Form1.pvindex = 4;  //B
                        svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - 2, sL]);     //[2,0][2,1]
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }




                    return true;
                }
                else { return false; }
            }
        }


        public static void fileGammaSettingAIO(bool svWrite, ref string svdevice)
        {
            //                [0]      [1]   [2]     [3]     [4]      [5]       [6]          [7]                [8]            [9]       [10]       [11]      [12]       [13]    [14]        [15]
            string[] svH = { "ICver,", "Cx,", "Cy,", "CLv,", "CAch,", "CAsel,", "OPTcheck,", "GammaFilePath,", "CoolCounts,", "iPBaddr,", "MTP,", "ex20d10,", "Duty,", "Project," };
            if (svWrite)
            {
                // 刪除檔案
                FileInfo copyFile = new FileInfo(mvars.strStartUpPath + @"\Parameter\GammaSetting_AIO.txt");
                if (copyFile.Exists) { copyFile.Delete(); }
                // 建立檔案
                StreamWriter sTAwrite = File.CreateText(mvars.strStartUpPath + @"\Parameter\GammaSetting_AIO.txt");
                sTAwrite.WriteLine(svH[0] + mvars.ICver);
                sTAwrite.WriteLine(svH[1] + mvars.UUT.Cx);
                sTAwrite.WriteLine(svH[2] + mvars.UUT.Cy);
                sTAwrite.WriteLine(svH[3] + mvars.UUT.CLv);
                //[4] CAch
                if (mvars.UUT.OverBet == 1) { sTAwrite.WriteLine(svH[4] + mvars.UUT.CAch); }
                else { sTAwrite.WriteLine(svH[4] + mvars.UUT.CAch + "~" + mvars.UUT.OverBet); }
                //[5] CAsel
                if (mvars.UIinTest == 0) { sTAwrite.WriteLine(svH[5] + mCAs.CAATG.CAsel); }
                else { sTAwrite.WriteLine(svH[5] + mCAs.CAATG.CAsel + "." + mvars.UIinTest); }  
                //[6] OPTcheck
                if (mvars.msrgammacurve == false && mvars.UUT.GMAposATD == 0) { sTAwrite.WriteLine(svH[6] + mvars.UUT.exGray); }
                else
                {
                    int sve = mvars.UUT.GMAposATD;
                    if (mvars.msrgammacurve) { if (mvars.msrgammacurveEd == 41) { sve += 2; } else if (mvars.msrgammacurveEd == 255) { sve += 4; } }
                    sTAwrite.WriteLine(svH[6] + mvars.UUT.exGray + "." + sve);
                }

                sTAwrite.WriteLine(svH[7] + mvars.UUT.gmafilepath);
                sTAwrite.WriteLine(svH[8] + mvars.UUT.CoolCounts);
                sTAwrite.WriteLine(svH[9] + mvars.iPBaddr);
                sTAwrite.WriteLine(svH[10] + mvars.UUT.MTP);

                if (mvars.flgex20d10) { sTAwrite.WriteLine(svH[11] + mvars.UUT.ex20d10[0] + "~" + mvars.UUT.ex20d10[1] + "~" + mvars.UUT.ex20d10[2] + "~" + mvars.UUT.ex20d10[3]); }

                string svs = mvars.duty[0].ToString();
                if (mvars.dualduty == 1) svs += "~" + mvars.duty[1].ToString();
                sTAwrite.WriteLine(svH[12] + svs);

                sTAwrite.WriteLine(svH[13] + svdevice);
                sTAwrite.Flush(); //清除緩衝區
                sTAwrite.Close(); //關閉檔案
            }
            else    //Read
            {
                ////                [0]      [1]   [2]     [3]     [4]              [5]               [6]                  [7]               [8]            [9]        [10]     [11]       [12]       [13]            [14]        [15]
                //string[] svH = { "ICver,", "Cx,", "Cy,", "CLv,", "CAch~overBet,", "CAsel.inTest,", "OPTcheck.msrGamma,", "GammaFilePath,", "CoolCounts,", "iPBaddr,", "MTP,", "ex20d10,", "Duty,", "Project.id," };
                string[] lines = File.ReadAllLines(mvars.strStartUpPath + @"\Parameter\GammaSetting_AIO.txt");
                for (byte svL = 0; svL < lines.Length; svL++)
                {
                    for (byte j = 0; j < svH.Length; j++)
                    {
                        if (lines[svL].ToLower().IndexOf(svH[j].ToLower(), 0) != -1)
                        {
                            string svtrimed = lines[svL].Substring(svH[j].Length, lines[svL].Length - svH[j].Length);

                            if (j == 0) { mvars.ICver = Convert.ToByte(svtrimed); break; }
                            else if (j == 1) { mvars.UUT.Cx = Convert.ToSingle(svtrimed); break; }
                            else if (j == 2) { mvars.UUT.Cy = Convert.ToSingle(svtrimed); break; }
                            else if (j == 3) { mvars.UUT.CLv = Convert.ToSingle(svtrimed); break; }
                            else if (j == 4)
                            {
                                string[] svs1 = svtrimed.Split('~');
                                mvars.UUT.CAch = Convert.ToByte(svs1[0]);
                                if (svs1.Length == 2) { mvars.UUT.OverBet = Convert.ToSingle(svs1[1]); }
                                if (mvars.UUT.OverBet == 0) mvars.UUT.OverBet = 1;
                                break;
                            }
                            else if (j == 5)
                            {
                                mCAs.CAATG.CAsel = 1; mvars.UIinTest = 0; mCAs.CAATG.Demo = false;
                                string[] svs1 = svtrimed.Split('.');
                                mCAs.CAATG.CAsel = Convert.ToByte(svs1[0]); if (mCAs.CAATG.CAsel >= 254) { mCAs.CAATG.Demo = true; }
                                if (svs1.Length == 2) { mvars.UIinTest = Convert.ToByte(mp.DecToBin(Convert.ToInt16(svs1[1]), 8).Substring(7, 1)); }
                                break;
                            }
                            else if (j == 6)
                            {
                                mvars.UUT.GMAposATD = 0; mvars.msrgammacurve = false; mvars.UUT.exGray = 220;
                                string[] svs1 = svtrimed.Split('.');
                                if (svs1[0] == "0") { mvars.UUT.exGray = 0; } else { mvars.UUT.exGray = Convert.ToByte(svs1[0]); }
                                if (svs1.Length == 2)
                                {
                                    mvars.UUT.GMAposATD = Convert.ToByte(mp.DecToBin(Convert.ToInt16(svs1[1]), 8).Substring(7, 1));
                                    if (mp.DecToBin(Convert.ToInt16(svs1[1]), 8).Substring(6, 1) == "1") { mvars.msrgammacurve = true; mvars.msrgammacurveEd = 41; mvars.msrgammacurveGp = 1; mvars.msrgammacurveSt = 0; }
                                    else if (mp.DecToBin(Convert.ToInt16(svs1[1]), 8).Substring(5, 1) == "1") { mvars.msrgammacurve = true; mvars.msrgammacurveEd = 255; mvars.msrgammacurveGp = 1; mvars.msrgammacurveSt = 0; }
                                }
                                break;
                            }
                            else if (j == 7) 
                            { 
                                mvars.UUT.gmafilepath = svtrimed;

                                //0037 add
                                byte svi;
                                byte svj = 0;
                                for (svi = 0; svi < mvars.UUT.Disk.Length / 3; svi++)
                                {
                                    if (mvars.UUT.Disk[svi, 0] != "" && mvars.UUT.Disk[svi, 0] != null)
                                    {
                                        if (svj == 0) svj = svi;
                                        if (mvars.UUT.gmafilepath.IndexOf(mvars.UUT.Disk[svi, 1], 0) != -1) break; 
                                    }
                                }
                                if (svi >= mvars.UUT.Disk.Length / 3) 
                                    mvars.UUT.gmafilepath = mvars.UUT.Disk[svj, 1] + mvars.UUT.gmafilepath.Substring(mvars.UUT.Disk[svj, 1].Length, mvars.UUT.gmafilepath.Length - mvars.UUT.Disk[svj, 1].Length);
                                if (Directory.Exists(mvars.UUT.gmafilepath)) { }
                                else Directory.CreateDirectory(mvars.UUT.gmafilepath);

                                break; 
                            }
                            else if (j == 8) 
                            { 
                                mvars.UUT.CoolCounts = Convert.ToInt32(svtrimed);
                                if (mvars.UUT.CoolCounts < 3) mvars.UUT.CoolCounts = 3;
                                break; 
                            }
                            else if (j == 9)
                            {
                                mvars.iPBaddr = 1;
                                string svs1 = svtrimed;
                                if (mp.IsNumeric(svs1)) { if (Convert.ToByte(svs1) >= 1 && Convert.ToByte(svs1) <= 16) mvars.iPBaddr = Convert.ToByte(svs1); }
                                else
                                { if (svs1.Length == 1 && (Convert.ToChar(svs1) >= 65 || Convert.ToChar(svs1) <= 80)) mvars.iPBaddr = (byte)(Convert.ToChar(svs1) - 64); }
                                break;
                            }
                            else if (j == 10) { if (svtrimed == "0") mvars.UUT.MTP = 0; else { mvars.UUT.MTP = 1; } break; }
                            else if (j == 11)
                            {
                                string[] svs1 = lines[svL].Split(',')[1].Split('~');
                                if (svs1.Length == 4) mvars.flgex20d10 = true;
                                for (int m = 0; m < 4; m++) { mvars.UUT.ex20d10[m] = Convert.ToSingle(svs1[m]); }
                                break;
                            }
                            else if (j == 12)
                            {
                                string[] svs1 = svtrimed.Split('~');
                                mvars.duty = new byte[svs1.Length];
                                mvars.dualduty = 0;
                                mvars.duty[0] = Convert.ToByte(svs1[0]);
                                if (svs1.Length > 1) 
                                { 
                                    mvars.duty[1] = Convert.ToByte(svs1[1]);
                                    mvars.pGMA = new typGammaIC[2];  //BANK0+BANK1
                                    mvars.dualduty = (byte)(svs1.Length - 1);
                                }
                                break;
                            }
                            else if (j == 13)
                            {
                                string[] svs1 = svtrimed.Split('.');
                                if (svs1.Length == 2) { svdevice = svtrimed; }
                                else { svdevice = ""; }
                                break;
                            }
                        }
                    }
                }
            }
        }


       




        public static void cFLASHREAD_C12ACB()
        {
            // flashselQ在 C12ACB與pCB的用法有差異

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mp.markreset(9999, false);
            mvars.flgDelFB = true;
            string SvErr = "0";

            UInt32 FlashSize = 8 * 1024 * 1024;     //8MB

            //byte[] FlashRd8MB_Arr = new byte[FlashSize];
            Array.Resize(ref FlashRd_Arr, (int)FlashSize);

            mvars.lblCmd = "FLASH_TYPE";
            mhFLASHTYPE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { SvErr = "-3"; goto Ex; }


            if (SvErr == "0")
            {
                UInt32 PacketSize = 2048;  
                //if (mvars.svnova == false) { PacketSize = 32768; }
                UInt32 Count = FlashSize / PacketSize;
                //
                sw.Reset();
                sw.Start();
                for (UInt32 i = 0; i < Count; i++)
                {
                    string txt36 = (i * PacketSize).ToString("X8");
                    if (txt36 == "00000000")
                    {
                        mvars.lblCmd = "FLASH_WREAR";
                        mhWREAR("00");
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }
                    }
                    if (txt36 == "01000000")
                    {
                        mvars.lblCmd = "FLASH_WREAR";
                        mhWREAR("01");
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }
                    }

                    if (i % 128 == 0)
                    {
        //                mvars.lstget.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ] +
        //" reading addr" + txt36 + " @ " + String.Format("{0:00}", ((i * 100) / Count) + "% - - - - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
        //                mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;

                        Form1.lstget1.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ] +
        " reading addr" + txt36 + " @ " + String.Format("{0:00}", ((i * 100) / Count) + "% - - - - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
                        Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                    }

                    mvars.lblCmd = "FLASH_READ_" + i.ToString("0000");
                    mhFLASHREAD(txt36, PacketSize);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-13"; break; }
                    else
                    {
                        //for (UInt32 j = 0; j < PacketSize; j++)
                        //{
                        //    FlashRd8MB_Arr[i * PacketSize + j] = mvars.gFlashRdPacketArr[j];
                        //}

                        for (UInt32 j = 0; j < PacketSize; j++)
                        {
                            FlashRd_Arr[i * PacketSize + j] = mvars.gFlashRdPacketArr[j];
                        }

                    }
                }

            }
            File.WriteAllBytes(mvars.strStartUpPath + @"\Parameter\" + mvars.strFLASHtype[mvars.flashselQ] +  "FlashRead.bin", FlashRd_Arr);
        Ex:
            mvars.flgDelFB = false;
            //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
            Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;

            mvars.lCounts = mvars.lCount;
            Form1.tslbltarget.Text = mvars.lCounts.ToString();
            mvars.errCode = SvErr;
        }

        public static void cFLASHWRITE_C12ACB()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mp.markreset(9999, false);
            mvars.flgDelFB = true;
            string SvErr = "0";
            string svcomm = Form1.tslblCOM.Text;
            bool svNVBC = mvars.nvBoardcast;
            mvars.nIsReadback = true;

            string svverFPGA = "";
            string svverFPGAerased = "";
            if (mvars.flashselQ == 1)
            {
                //回讀不使用Boardcast(需要建立依序詢問)
                mvars.nvBoardcast = false;
                mvars.lblCmd = "MCU_VERSION";
                mp.mhVersion();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) 
                {
                    //mvars.lstget.Items.Add(mvars.lGet[mvars.lCount - 1]); 
                    Form1.lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]);
                    SvErr = "-1"; 
                    goto Ex; 
                }
                Form1.pvindex = 0;
                mvars.lblCmd = "FPGA_SPI_R";
                mp.mhFPGASPIREAD();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) 
                {
                    //mvars.lstget.Items.Add(mvars.lGet[mvars.lCount - 1]); 
                    Form1.lstget1.Items.Add(mvars.lGet[mvars.lCount - 1]);
                    SvErr = "-2"; 
                    goto Ex; 
                }
                svverFPGA = mvars.verFPGA;
                //mvars.lstget.Items.Add("VERSION,MCU," + mvars.verMCU + ",FPGA," + mvars.verFPGA);
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.Items.Add("VERSION,MCU," + mvars.verMCU + ",FPGA," + mvars.verFPGA);
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
            }

            mvars.nvBoardcast = svNVBC;
            mvars.lblCmd = "FLASH_TYPE";
            mhFLASHTYPE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { SvErr = "-3"; goto Ex; }
            mvars.lblCmd = "FLASH_FUNCQE";
            mhFUNCQE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }
            mvars.lblCmd = "FUNC_ENABLE";
            mhFUNCENABLE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4.5"; goto Ex; }
            mvars.lblCmd = "FUNC_STATUS";
            mhFUNCSTATUS();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-5"; goto Ex; }
            if (!((mvars.hFuncStatus & 0x02) == 0x02))
            {
                //mvars.lstget.Items.Add(Form1.cmbhPB.Text + " FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02");
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.Items.Add(Form1.cmbhPB.Text + " FUNCSTATUS @ " + mvars.hFuncStatus + " <> 0x02");
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                SvErr = "-6"; goto Ex;
            }
            //flasherase
            sw.Reset();
            sw.Start();
            mvars.lblCmd = "FLASH_ERASE";
            mhFLASHERASE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-7"; goto Ex; }
            while (true)
            {
                //回讀不使用Boardcast(需要建立依序詢問)
                mvars.nvBoardcast = false;
                mvars.lblCmd = "FUNC_STATUS";
                mhFUNCSTATUS();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-8"; break; }
                if (mp.DecToBin(mvars.hFuncStatus, 8).Substring(7, 1) == "0") { break; }
                Application.DoEvents();


                //mvars.lstget.Items.Add(" -> Waiting for " + mvars.strFLASHtype[mvars.flashselQ] +
                //            " Flash erasing... " + string.Format("{0:#.0}", sw.Elapsed.TotalSeconds) + "sec");
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;

                Form1.lstget1.Items.Add(" -> Waiting for " + mvars.strFLASHtype[mvars.flashselQ] +
                            " Flash erasing... " + string.Format("{0:#.0}", sw.Elapsed.TotalSeconds) + "sec");
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;

                if (sw.Elapsed.TotalSeconds > 300)
                {
                    //mvars.lstget.Items.Add(Form1.cmbhPB.Text + " FLASH_ERASE Timeout > 300 sec");
                    Form1.lstget1.Items.Add(Form1.cmbhPB.Text + " FLASH_ERASE Timeout > 300 sec");
                    SvErr = "-9"; break;
                }
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                Thread.Sleep(1000);
            }
            sw.Stop();

            if (SvErr != "0") goto Ex;
            // end flasherase

            if (SvErr == "0")
            {
                //寫入使用Boardcast
                mvars.nvBoardcast = svNVBC;

                //mvars.lblCmd = "HW_RESET_FPGA"; 
                //mp.mhSWRESET(0x80);
                //mvars.lstget.Items.Add(" --> Wait 5s"); mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                //mp.doDelaySec(5);


                UInt32 FlashSize = (UInt32)(mvars.ucTmp.Length);      //8 * 1024 * 1024
                UInt16 PacketSize = 2048;   //1024,2048,4096,8192,16384
                UInt32 Count = FlashSize / PacketSize;
                sw.Reset();
                sw.Start();
                if (FlashSize != (8 * 1024 * 1024))
                {
                    SvErr = "-10";
                    goto Ex;
                }
                if (mvars.svnova) { mvars.nIsReadback = true; }
                for (UInt32 i = 0; i < Count; i++)
                {
                    string txt36 = (i * PacketSize).ToString("X8");
                    if (i % 128 == 0)
                    {
                        //mvars.lstget.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ] +
                        //            " writing addr" + txt36 + " @ " + String.Format("{0:00}", ((i * 100) / Count) + "% - - - - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
                        //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;

                        Form1.lstget1.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ] + " writing addr" + txt36 + " @ " + String.Format("{0:00}", ((i * 100) / Count) + "% - - - - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
                        Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                    }
                    mvars.lblCmd = "FLASH_WRITE_" + i.ToString("0000");
                    mhFLASHWRITEPAGEQIO(txt36, PacketSize);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                    {
                        SvErr = "-11." + String.Format("{0:00}", ((i * 100) / Count)); break;
                    }
                    if (mvars.Break) { SvErr = "-11"; break; }
                }
                if (mvars.Break) { mvars.Break = false; }
                if (SvErr == "0")
                {
                    //mvars.lstget.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ] + " write  @ 100% - - - - " + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
                    Form1.lstget1.Items.Add(" -> " + mvars.strFLASHtype[mvars.flashselQ] + " write  @ 100% - - - - " + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
                }
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
            }
        Ex:
            byte svflashQ = mvars.flashselQ;
            if (SvErr == "0")
            {
                //mvars.lstget.Items.Add(" -> FPGA SW_RESET after Write"); 
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.Items.Add(" -> FPGA SW_RESET after Write");
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                mvars.lblCmd = "SW_RESET_FPGA";
                mp.mhFPGARESET(0x81);
                //mvars.lstget.Items.Add(" --> Wait 10s"); 
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.Items.Add(" --> Wait 10s");
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                mp.doDelayms(10000);

                mvars.flashselQ = 0;
                mvars.lblCmd = "FLASH_TYPE";
                mhFLASHTYPE();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    //mvars.lstget.Items.Add(" -> " + mvars.strFLASHtype[svflashQ] + " write OK but FLASH_TYPE switch to \"OPEN\" fail");
                    Form1.lstget1.Items.Add(" -> " + mvars.strFLASHtype[svflashQ] + " write OK but FLASH_TYPE switch to \"OPEN\" fail");
                    mvars.flashselQ = svflashQ;
                    SvErr = "-14";
                }
                mp.doDelayms(1000);
                //Form1.cmbhFlash.Text = Form1.cmbhFlash.Items[mvars.flashselQ].ToString();
                uc_Flash.cmbFlashSel.SelectedIndex = 0;
                //mvars.lstget.Items.Add(" -> MCU_RESET after FLASH_TYPE switch to Default"); 
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.Items.Add(" -> MCU_RESET after FLASH_TYPE switch to Default");
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                mvars.lblCmd = "MCU_RESET";
                mp.mhSWRESET(0xFF);
                //mvars.lstget.Items.Add(" --> Wait 17s"); 
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.Items.Add(" --> Wait 17s");
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                mp.doDelayms(17000);

                sw.Reset();
                sw.Start();
                if (mvars.svnova == false)
                {
                    Form1.tslblCOM.Text = "x COM"; Form1.tslblCOM.ForeColor = System.Drawing.Color.Red; SvErr = "-1";
                    mp.CommPortSeek();
                    if (mvars.Comm.Length != 0)
                    {
                        if (Array.IndexOf(mvars.Comm, svcomm) != -1)
                        {
                            //mvars.lstget.Items.RemoveAt(mvars.lstget.Items.Count - 1);
                            Form1.lstget1.Items.RemoveAt(Form1.lstget1.Items.Count - 1);
                            Form1.tslblCOM.Text = svcomm; Form1.tslblCOM.ForeColor = System.Drawing.Color.Black; SvErr = "0";
                        }
                    }
                }
                sw.Stop();
                if (sw.Elapsed.TotalSeconds > 300 || SvErr != "0")
                {
                    //mvars.lstget.Items.Add(" write OK but MCU_RESET NG @ " + string.Format("{0:#.0}", sw.Elapsed.TotalSeconds)); 
                    //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                    Form1.lstget1.Items.Add(" write OK but MCU_RESET NG @ " + string.Format("{0:#.0}", sw.Elapsed.TotalSeconds));
                    Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                }
                else
                {
                    if (svflashQ == 1)
                    {
                        //回讀不使用Boardcast(需要建立依序詢問)
                        mvars.nvBoardcast = false;
                        mvars.lblCmd = "MCU_VERSION";
                        mp.mhVersion();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-16"; }
                        Form1.pvindex = 0;
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        //mvars.lstget.Items.Add("VERSION,MCU," + mvars.verMCU + ",FPGA," + mvars.verFPGA);
                        //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                        Form1.lstget1.Items.Add("VERSION,MCU," + mvars.verMCU + ",FPGA," + mvars.verFPGA);
                        Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;

                        mvars.flgDelFB = false;
                        if (svverFPGAerased == mvars.verFPGA)
                        {
                            //mvars.lstget.Items.Add(mvars.strFLASHtype[svflashQ] + " write OK but Version check : " + svverFPGA + " >> " + svverFPGAerased + " >> " + mvars.verFPGA);

                            Form1.lstget1.Items.Add(mvars.strFLASHtype[svflashQ] +
                                " write OK but Version check : " + svverFPGA + " >> " + svverFPGAerased + " >> " + mvars.verFPGA);
                        }
                        else
                        {
                            //mvars.lstget.Items.Add(mvars.strFLASHtype[svflashQ] + " write OK and Version check : " + svverFPGA + " >> " + svverFPGAerased + " >> " + mvars.verFPGA);

                            Form1.lstget1.Items.Add(mvars.strFLASHtype[svflashQ] +
                                " write OK and Version check : " + svverFPGA + " >> " + svverFPGAerased + " >> " + mvars.verFPGA);
                        }
                    }
                    else
                    {
                        //mvars.lstget.Items.Add(mvars.strFLASHtype[svflashQ] + " write OK and HW_RESET OK");
                        Form1.lstget1.Items.Add(mvars.strFLASHtype[svflashQ] + " write OK and HW_RESET OK");
                    }
                }
                //mvars.lstget.Items.Add(" --> HOTPLUG_ON and wait 3s to LightOn"); mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.Items.Add(" --> HOTPLUG_ON and wait 3s to LightOn");
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                mvars.flgDelFB = true;

                //寫入使用Boardcast
                mvars.nvBoardcast = svNVBC;
                //Form1.pvindex = 25;   //
                //mvars.lblCmd = "FPGA_SPI_W"; 
                //mp.mhFPGASPIWRITE(15);
                Form1.pvindex = 255;
                //mvars.lblCmd = "FPGA_SPI_W255";
                //mp.mhFPGASPIWRITE(0);
                //mvars.lblCmd = "FPGA_SPI_W255";
                //mp.mhFPGASPIWRITE(1);
                //mvars.lblCmd = "FPGA_SPI_W255";
                //mp.mhFPGASPIWRITE(0);
                mvars.flgDelFB = false;
                //mvars.lstget.Items.Add(" --> Wait 3s"); mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;
                Form1.lstget1.Items.Add(" --> Wait 3s"); 
                Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                //mvars.nvBoardcast = false;  //使用完畢關閉
                mp.doDelayms(3000);
            }
            else
            {
                //mvars.lstget.Items.Add(mvars.strFLASHtype[svflashQ] + " write fail ErrCode " + SvErr);
                Form1.lstget1.Items.Add(mvars.strFLASHtype[svflashQ] + " write fail ErrCode " + SvErr);
            }

            if (mvars.demoMode == false && mvars.sp1.IsOpen) mvars.sp1.Close();

            mvars.flgDelFB = false;
            Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;

            mvars.lCounts = mvars.lCount;
            Form1.tslbltarget.Text = mvars.lCounts.ToString();
            mvars.errCode = SvErr;
            mvars.nvBoardcast = svNVBC;
        }


        public static void SaveBinFile(string sPathFileName, byte[] BinArr)
        {
            string sPath = Path.GetDirectoryName(sPathFileName);
            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);
            File.WriteAllBytes(sPathFileName, BinArr);
        }







        #region cFLASHWRITE ******************************************************************
        // for uc_coding
        public static void cFLASHWRITE_pMCU(int svDevices, ListBox lstget1)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mp.markreset(999, false);
            mvars.flgDelFB = true;
            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            mvars.isReadBack = true;
            string txt44;
            UInt32 PacketSize;
            UInt32 Count;
            int svlstc = 0;

            svDevices = 1;

            typhwCard[] svhwCard = new typhwCard[svDevices];
            Array.Clear(svhwCard, 0, 1);
            bool[] svundo = new bool[svDevices];
            bool[] svundoB = new bool[svDevices];
            bool[] svundoA = new bool[svDevices];
            bool[] svunlock = new bool[svDevices];
            bool[] svUserbreak = new bool[svDevices];
            int svtodos;    // 在第一階段(判斷版本與上鎖)未發生異常準備燒錄(todo)的片數
            int svactdos;   // 在第二階段(bootcode OK，appcpde OK)的片數
            svtodos = svDevices;
            ushort svundos = 0;



            string sverr = "000000"; //[0]更新前回讀boot fail=1
                                     //[1]更新前回讀app fail=1
                                     //[2]更新前回讀verMCU fail=1
                                     //[3]更新後回讀boot fail=1
                                     //[4]更新後回讀app fail=1



            #region ----------------------------------------------------- ↓版本回讀，判斷 Undo
            for (int svDevCnt = 0; svDevCnt < svDevices; svDevCnt++)
            {
                //get verMCUB
                mvars.lblCmd = "READ_MCUBOOTVER";
                mp.mhMCUFLASHREAD(mvars.MCU_BOOT_VERSION_ADDR.ToString("X8"), 16);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    mp.ReplaceAt(sverr, 1, "1");
                    mp.funSaveLogs("(pMCU)  " + mvars.lGet[mvars.lCount - 1]);
                }
                else
                {
                    if (mp.IsNumeric(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)) == false) { svhwCard[svDevCnt].CurrentBootVer = 0; }
                    //mvars.verMCUB = Boot-220324-0008                                       
                    else { svhwCard[svDevCnt].CurrentBootVer = Convert.ToInt16(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)); }
                    //svhwCard[svDevCnt].CurrentBootVer = 8
                }
                svhwCard[svDevCnt].verMCUB = mvars.verMCUB; //mvars.verMCUB = Boot-220324-0008

                //get verMCUA
                mvars.lblCmd = "READ_MCUAPPVER";
                mp.mhMCUFLASHREAD(mvars.MCU_APP_VERSION_ADDR.ToString("X8"), 16);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    mp.ReplaceAt(sverr, 2, "1");
                    mp.funSaveLogs("(pMCU)  " + mvars.lGet[mvars.lCount - 1]);
                }
                else
                {
                    if (mp.IsNumeric(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)) == false) { svhwCard[svDevCnt].CurrentAppVer = 0; }
                    //mvars.verMCUB = App-220518-T0039
                    else { svhwCard[svDevCnt].CurrentAppVer = Convert.ToInt16(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)); }
                    //svhwCard[svDevCnt].CurrentAppVer = 39
                }
                svhwCard[svDevCnt].verMCUA = mvars.verMCUA; //mvars.verMCUA = App-220518-T0039

                mvars.lblCmd = "MCU_VERSION";
                mp.mhVersion();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    mp.ReplaceAt(sverr, 3, "1");
                    mp.funSaveLogs("(pMCU)  " + mvars.lGet[mvars.lCount - 1]);
                }
                svhwCard[svDevCnt].verMCU = mvars.verMCU;   //mvars.verMCU = App-220518-T0039

                if (sverr.Substring(0, 3) != "000")
                {
                    lstget1.Items.Add(" ->  IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00") + ",errCode: " + sverr);
                    svundo[svDevCnt] = true;
                }
                else
                {
                    mvars.lblCmd = "READ_NVM_USERPAGE";
                    //txt44 = (0xFFF0).ToString("X8");
                    mp.mNvmUserPageRd();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                    {
                        svundo[svDevCnt] = true;
                        lstget1.Items.Add(" ->  IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00") + ",NvmUserPageRd,Error");
                        lstget1.Items.Insert(svlstc, " -> Code Protect：UnKnow");
                        mp.funSaveLogs("(pMCU)  " + mvars.lGet[mvars.lCount - 1] + ",Code Protect：UnKnow");
                    }
                    else
                    {
                        Byte BootSizeVal, Hyst; UInt16 BOD_Level;
                        mp.Copy(mvars.ReadDataBuffer, 6 + 1, mvars.NVM_UserPage, 0, mvars.NVM_UserPage.Length);
                        if ((mvars.NVM_UserPage[0] & 0x01) == 0x01) { mvars.sBOD33 = "BOD33 Disable"; } //[0]56 [1]146 [2]154 [3]222
                        else { mvars.sBOD33 = "BOD33 Enable"; }
                        BootSizeVal = (Byte)((mvars.NVM_UserPage[3] & 0x3C) / 4);
                        mvars.sBootSizeVal = ((15 - BootSizeVal) * 8192).ToString();
                        BOD_Level = (UInt16)(mvars.NVM_UserPage[0] + mvars.NVM_UserPage[1] * 256);
                        BOD_Level = (UInt16)((BOD_Level & 0x01FE) / 2);
                        mvars.sBOD_Level = BOD_Level.ToString();
                        Hyst = (Byte)((mvars.NVM_UserPage[1] & 0x78) / 8); mvars.sHyst = Hyst.ToString();
                        mvars.sBODminus = (1.5 + BOD_Level * 0.006).ToString(); mvars.sBODplus = (1.5 + BOD_Level * 0.006 + Hyst * 0.006).ToString();
                        if (mvars.sBootSizeVal == "0" || mvars.sBootSizeVal == "65536")  // sBootSizeVal = 65536
                        {
                            if (mvars.sBootSizeVal == "0") svunlock[svDevCnt] = true;           // 解除鎖定
                            else if (mvars.sBootSizeVal == "65536") svunlock[svDevCnt] = false; // 未解除鎖定 Code Protect

                            lstget1.Items.Add("(" + sverr + ") IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00"));
                            lstget1.Items.Add(" -> " + mvars.sBOD33 + ",Boot Protect Size = " + mvars.sBootSizeVal);
                            lstget1.Items.Add(" -> BOD Value：" + mvars.sBOD_Level);
                            lstget1.Items.Add(" -> HYST Value：" + mvars.sHyst);
                            lstget1.Items.Add(" -> BOD-：" + mvars.sBODminus + ",BOD+：" + mvars.sBODplus);
                            lstget1.Items.Add(" -> Code Locked：" + !svunlock[svDevCnt]); svlstc = lstget1.Items.Count - 1;
                            lstget1.Items.Add(" -> Hex ver. (Boot,App)：" + mvars.HexBootVer + "," + mvars.HexAppVer);
                            lstget1.Items.Add(" -> Current ver. (Boot,App,MCU)：" + svhwCard[svDevCnt].CurrentBootVer + "," + svhwCard[svDevCnt].CurrentAppVer + "," + svhwCard[svDevCnt].verMCU);

                            //先不管廣播一律單一
                            //if (mvars.nvBoardcast == false)
                            //{
                            //    if (mvars.iSender + 1 != numericUpDown_sender.Value || mvars.iPort + 1 != numericUpDown_port.Value || mvars.iScan + 1 != numericUpDown_scan.Value)
                            //    {
                            //        svundo[svDevCnt] = true;
                            //    }
                            //    //else if (HexBootVer <= svhwCard[svDevCnt].CurrentBootVer && HexAppVer <= svhwCard[svDevCnt].CurrentAppVer && mvars.flgForceUpdate == false) svundo[svDevCnt] = true;
                            //}
                            if (mvars.HexBootVer <= svhwCard[svDevCnt].CurrentBootVer && mvars.HexAppVer <= svhwCard[svDevCnt].CurrentAppVer && mvars.flgForceUpdate == false) svundo[svDevCnt] = true;
                        }
                        else
                        {
                            lstget1.Items.Add(" -> IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00") + ",sBootSizeVal," + mvars.sBootSizeVal + ",Error");
                            lstget1.Items.Insert(svlstc, " -> Code Protect：UnKnow(≠ 0 & 65536)");
                            mp.funSaveLogs("(pMCU)  " + mvars.lGet[mvars.lCount - 1] + ",Code Protect：UnKnow(≠ 0 & 65536)");
                            svundo[svDevCnt] = true;
                        }
                    }
                }
                if (svundo[svDevCnt]) { lstget1.Items.Add(" --> UnDo"); svundos++; }
                lstget1.Items.Add("");
                lstget1.TopIndex = lstget1.Items.Count - 1;

                if (lstget1.Items.Count > 0) { mp.funSaveLogs(lstget1.Items[lstget1.Items.Count - 1].ToString()); }
            }

            svtodos -= svundos;
            lstget1.Items.Add("Read version " + Convert.ToString(string.Format("{0:0.#}", sw.Elapsed.TotalSeconds)) + "s，next step devices: " + svtodos);
            lstget1.Items.Add("");
            lstget1.TopIndex = lstget1.Items.Count - 1;

            /// svundo的累加判斷
            /// --> mvars.errCode != "000"
            /// --> mNvmUserPageRd feedback Error
            /// --> 非廣播而且不是選擇的接收卡
            /// --> mvars.sBootSizeVal != "0" and mvars.sBootSizeVal != "65536" ，一定是這兩個值
            /// --> Hex檔內含的(HexBootVer/HexAppVer)版本比硬體回讀的(CurrentBootVer/CurrentAppVer)舊而且沒有使用強燒旗標
            #endregion -------------------------------------------------- ↑版本回讀


            #region  ---------------------------------------------------- ↓Bootcode/Appcode 燒錄
            svactdos = 0;
            if (svundos < svDevices)
            {
                for (int svDevCnt = 0; svDevCnt < svDevices; svDevCnt++)
                {
                    if (mvars.Break && svundo[svDevCnt] == false)
                    {
                        /// 0104
                        svUserbreak[svDevCnt] = true;
                        //mvars.iSender = (byte)svhwCard[svDevCnt].iSender;
                        //mvars.iPort = (byte)svhwCard[svDevCnt].iPort;
                        //mvars.iScan = (byte)svhwCard[svDevCnt].iScan;
                        //lstget1.Items.Add(" S" + (mvars.iSender + 1) + "P" + (mvars.iPort + 1) + "C" + (mvars.iScan + 1) + "，User Break");
                        //lstget1.TopIndex = lstget1.Items.Count - 1;
                    }
                    else
                    {
                        #region↓ Boot code write
                        if (svundo[svDevCnt] == false)
                        {
                            if (mvars.HexBootVer > svhwCard[svDevCnt].CurrentBootVer || mvars.flgForceUpdate || uc_coding.svforceAll)
                            {
                                sw.Reset();
                                sw.Start();

                                lstget1.Items.Add((svactdos + 1) + " of " + svtodos + " IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00") + ",Bootcode Update");
                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                if (svhwCard[svDevCnt].verMCU.IndexOf("App", 0) != -1)
                                {
                                    if (svundoB[svDevCnt] == false && svunlock[svDevCnt] == false)    //svunlock[svDevCnt] == false = Locked
                                    {
                                        mvars.lblCmd = "DIS_PROTBOD33";
                                        mp.pDisProtBOD33();
                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                        {
                                            lstget1.Items.Add(" -> Disable CodeProtect,Error");
                                            mp.funSaveLogs("(pMCU)  " + mvars.lGet[mvars.lCount - 1] + ",Disable CodeProtect,Error");
                                            svunlock[svDevCnt] = false;
                                        }
                                        else { svunlock[svDevCnt] = true; }
                                    }

                                    if (svundoB[svDevCnt] == false)
                                    {
                                        if (mvars.flgForceUpdate) { lstget1.Items.Add(" -> ForceALL Update to HexBootVer(" + mvars.HexBootVer + ")"); }
                                        else { lstget1.Items.Add(" -> Update to HexBootVer(" + mvars.HexBootVer + ") > CurrentBootVer(" + svhwCard[svDevCnt].CurrentBootVer + ")"); }
                                        lstget1.TopIndex = lstget1.Items.Count - 1;

                                        if (svundoA[svDevCnt] == false)
                                        {
                                            //mvars.lblCmd = "MCU_RESET";
                                            //mp.McuSW_Reset();
                                            //lstget1.Items.Add(" --> " + mvars.lblCmd + " wait for 12s"); lstget1.TopIndex = lstget1.Items.Count - 1;
                                            //mp.doDelayms(12000);
                                            //lstget1.Items.RemoveAt(lstget1.Items.Count - 1);

                                            mvars.lblCmd = "MCU_RESET";
                                            mp.mhMCUSWRESET();
                                            int svWaitSec = 10;
                                            do
                                            {
                                                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec"); }
                                                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  請稍後 " + svWaitSec + " 秒"); }
                                                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  请稍后 " + svWaitSec + " 秒"); }
                                                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  待って " + svWaitSec + " 秒"); }
                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                Application.DoEvents();
                                                mp.doDelayms(1000);
                                                lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                                                svWaitSec--;
                                            } while (svWaitSec > 0);
                                        }

                                        PacketSize = mvars.MCU_BLOCK_SIZE;
                                        Count = mvars.BOOT_SIZE / PacketSize;
                                        byte sverrc = 0;
                                        lstget1.Items.Add(" --> Boot Write");
                                        svlstc = lstget1.Items.Count - 1;
                                        lstget1.Items.Add("");
                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                        for (int svcnt = 0; svcnt < Count; svcnt++)
                                        {
                                        reBtWr:
                                            if (svundoB[svDevCnt] == false)
                                            {
                                                txt44 = (svcnt * PacketSize).ToString("X8");
                                                Application.DoEvents();
                                                mp.Copy(mvars.gMcuBinFile, (int)(svcnt * PacketSize), mvars.gFlashRdPacketArr, 0, (int)PacketSize);

                                                mvars.lblCmd = "MCU_BLWRITE";
                                                mp.pMCUBLWRITE(0x15, txt44);   //Primary dedicated
                                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                                {
                                                    //只有發送卡的一包2048才需要再重新計算
                                                    //int m = (int)svcnt;
                                                    //svcnt = (int)((m / 16) * 16);
                                                    if (sverrc < 3) { sverrc++; goto reBtWr; }
                                                    else { svundoB[svDevCnt] = true; }
                                                }
                                                else
                                                {
                                                    lstget1.Items.RemoveAt(svlstc);
                                                    lstget1.Items.Insert(svlstc, " --> Boot Wr cnt：" + svcnt + " of " + (Count - 1));
                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                }
                                            }
                                        }
                                        lstget1.Items.RemoveAt(svlstc);
                                        lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                                        lstget1.TopIndex = lstget1.Items.Count - 1;

                                        //Primary 起不用再回讀比對
                                    }
                                    if (svundoB[svDevCnt] == false)
                                    {
                                        lstget1.Items.Add(" -> DONE," + Convert.ToString(string.Format("{0:0.#}", sw.Elapsed.TotalSeconds)) + "s");
                                    }
                                    else
                                    {
                                        lstget1.Items.Add(" -> Fail," + Convert.ToString(string.Format("{0:0.#}", sw.Elapsed.TotalSeconds)) + "s");
                                    }
                                    lstget1.TopIndex = lstget1.Items.Count - 1;

                                    if (svundoB[svDevCnt] == false && svunlock[svDevCnt])
                                    {
                                        /// 0104 
                                        mvars.lblCmd = "EN_PROTBOD33";
                                        mp.pEnProtBOD33();
                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                        {
                                            svunlock[svDevCnt] = false;
                                            lstget1.Items.Add(" -> Enable CodeProtect");
                                            mp.funSaveLogs("(pMCU)  Enable CodeProtect");
                                        }
                                        else
                                        {
                                            svunlock[svDevCnt] = true;
                                            lstget1.Items.Add(" -> Enable CodeProtect Fail");
                                            mp.funSaveLogs("(pMCU)  " + mvars.lGet[mvars.lCount - 1] + ",Enable CodeProtect Fail");
                                        }
                                    }
                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                }
                                else { lstget1.Items.Add(" -> Error,Not App mode"); lstget1.TopIndex = lstget1.Items.Count - 1; }
                            }
                            /// svundoB[svDevCnt] 在燒錄過程中發生異常
                        }

                        /// 0104 一律上 code EN_PROTect
                        if (svunlock[svDevCnt])
                        {
                            mvars.lblCmd = "EN_PROTBOD33";
                            mp.pEnProtBOD33();
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                            {
                                svunlock[svDevCnt] = false;
                                /// 0104
                                lstget1.Items.Add(" -> Code protected");
                            }
                            else
                            {
                                /// 0104
                                lstget1.Items.Add(" -> Code protect fail");
                            }
                            lstget1.Items.Add("");
                            lstget1.TopIndex = lstget1.Items.Count - 1;
                        }
                        #endregion Boot code write

                        /// svundoB 在寫入失敗 = true，在回讀錯誤 = true，在回讀比較檔案內容時錯誤 = true

                        #region↓ App code write
                        if (svundoB[svDevCnt] == false)
                        {
                            if (mvars.HexAppVer > svhwCard[svDevCnt].CurrentAppVer || mvars.flgForceUpdate || uc_coding.svforceAll)
                            {
                                sw.Reset();
                                sw.Start();

                                lstget1.Items.Add((svactdos + 1) + " of " + svtodos + " IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00") + ",Appcode Update");
                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                if (mvars.flgForceUpdate) { lstget1.Items.Add(" -> Force Update to HexAppVer(" + mvars.HexAppVer + ")"); }
                                else { lstget1.Items.Add(" -> Update to HexAppVer(" + mvars.HexAppVer + ") > CurrentAppVer(" + svhwCard[svDevCnt].CurrentAppVer + ")"); }
                                lstget1.TopIndex = lstget1.Items.Count - 1;

                                mvars.lblCmd = "MCU_VERSION";
                                mp.mhVersion();             //get verMCU = App-220518-T0039
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundoA[svDevCnt] = true; }
                                else
                                {
                                    if (svhwCard[svDevCnt].verMCU.IndexOf("App", 0) != -1)  //verMCU = App-220518-T0039
                                    {
                                        mvars.lblCmd = "MCU_BLMODE";
                                        mp.mhMCUBLMODE();
                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                        {
                                            lstget1.Items.Add(" -> App mode --> Boot mode Fail");
                                            lstget1.TopIndex = lstget1.Items.Count - 1;
                                            mp.funSaveLogs("(pMCU)  " + mvars.lGet[mvars.lCount - 1] + ",App mode switch to Boot mode Fail");
                                            svundoA[svDevCnt] = true;
                                        }
                                        else
                                        {
                                            lstget1.Items.Add(" -> App mode --> Boot mode");
                                            lstget1.TopIndex = lstget1.Items.Count - 1;
                                            mvars.lblCmd = "MCU_RESET";
                                            mp.mhMCUSWRESET();

                                            int svWaitSec = 5;
                                            do
                                            {
                                                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec"); }
                                                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  請稍後 " + svWaitSec + " 秒"); }
                                                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  请稍后 " + svWaitSec + " 秒"); }
                                                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  待って " + svWaitSec + " 秒"); }
                                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                                Application.DoEvents();
                                                mp.doDelayms(1000);
                                                lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                                                svWaitSec--;
                                            } while (svWaitSec > 0);

                                            //lstget1.Items.Add(" -> " + mvars.lblCmd + " wait for 10s");
                                            //lstget1.TopIndex = lstget1.Items.Count - 1;
                                            //mp.doDelayms(10000);
                                            lstget1.Items.RemoveAt(lstget1.Items.Count - 1);    /// lstget1.Items.Add(" -> " + mvars.lblCmd + " wait for 10s");
                                            lstget1.Items.RemoveAt(lstget1.Items.Count - 1);    /// lstget1.Items.Add(" -> App mode --> Boot mode");
                                        }
                                    }

                                    if (svundoA[svDevCnt] == false)
                                    {
                                        PacketSize = mvars.MCU_BLOCK_SIZE;
                                        Count = mvars.APP_SIZE / PacketSize;
                                        byte sverrc = 0;
                                        lstget1.Items.Add(" --> Bootloader Write");
                                        svlstc = lstget1.Items.Count - 1;
                                        lstget1.Items.Add("");
                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                        for (int svcnt = 0; svcnt < Count; svcnt++)
                                        {
                                        reBlWr:
                                            if (svundoA[svDevCnt] == false)
                                            {
                                                txt44 = (svcnt * PacketSize + mvars.APP_START_ADDR).ToString("X8");
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
                                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                                {
                                                    //只有發送卡的一包2048才需要再重新計算
                                                    //int m = (int)svcnt;
                                                    //svcnt = (int)(m / 16) * 16;
                                                    if (sverrc < 3) { sverrc++; goto reBlWr; }
                                                    else { svundoA[svDevCnt] = true; }
                                                }
                                                else
                                                {
                                                    lstget1.Items.RemoveAt(svlstc);
                                                    lstget1.Items.Insert(svlstc, " --> Bootloader Wr cnt：" + svcnt + " of " + (Count - 1));
                                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                                }
                                            }
                                        }
                                        lstget1.Items.RemoveAt(svlstc);                     /// lstget1.Items.Insert(svlstc, " --> Bootloader Wr cnt：" + svcnt + " of " + (Count - 1));
                                        lstget1.Items.RemoveAt(lstget1.Items.Count - 1);    /// lstget1.Items.Add("");
                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                        mp.doDelayms(200);  ///500

                                        mvars.isReadBack = true;

                                        Array.Clear(mvars.McuFlashArr, 0, mvars.McuFlashArr.Length);
                                        #region Primary 起不用再回讀比對
                                        //if (svundoA[svDevCnt] == false)
                                        //{
                                        //    lstget1.Items.Add(" --> Bootloader Read");
                                        //    svlstc = lstget1.Items.Count - 1;
                                        //    lstget1.Items.Add("");
                                        //    lstget1.TopIndex = lstget1.Items.Count - 1;
                                        //    PacketSize = mvars.MCU_BLOCK_SIZE;
                                        //    Count = mvars.APP_SIZE / PacketSize;
                                        //    for (int svcnt = 0; svcnt < Count; svcnt++)
                                        //    {
                                        //        txt44 = (svcnt * PacketSize + mvars.APP_START_ADDR).ToString("X8");
                                        //        mvars.lblCmd = "MCU_BLREAD";
                                        //        mp.mhMCUBLREAD(txt44, (int)PacketSize);
                                        //        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundoA[svDevCnt] = true; }
                                        //        else
                                        //        {
                                        //            for (UInt32 j = 0; j < PacketSize; j++)
                                        //            {
                                        //                mvars.McuFlashArr[svcnt * PacketSize + j + mvars.BOOT_SIZE] = mvars.gFlashRdPacketArr[j];
                                        //            }
                                        //            lstget1.Items.RemoveAt(svlstc);
                                        //            lstget1.Items.Insert(svlstc, " --> Bootloader Rd cnt：" + svcnt + " of " + (Count - 1));
                                        //            lstget1.TopIndex = lstget1.Items.Count - 1;
                                        //        }
                                        //    }
                                        //    lstget1.Items.RemoveAt(svlstc);
                                        //    lstget1.Items.RemoveAt(lstget1.Items.Count - 1);    /// lstget1.Items.Add("");
                                        //    string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\Boot_Read.bin";
                                        //    File.WriteAllBytes(path, mvars.McuFlashArr);
                                        //    if (svundoA[svDevCnt] == false)
                                        //    {
                                        //        for (uint j = mvars.APP_START_ADDR; j < mvars.APP_END_ADDR; j++)
                                        //        {
                                        //            if (j >= 0x60000 && j < 0x6A000) continue;
                                        //            if (mvars.gMcuBinFile[j] != mvars.McuFlashArr[j])
                                        //            {
                                        //                lstget1.Items.Add(" -> Compare fail @ addr：" + j);
                                        //                lstget1.TopIndex = lstget1.Items.Count - 1;
                                        //                svundoA[svDevCnt] = true;
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        #endregion Primary 起不用再回讀比對
                                    }
                                    if (svundoA[svDevCnt] == false)
                                    {
                                        mvars.lblCmd = "MCU_RESET";
                                        mp.McuSW_Reset();

                                        int svWaitSec = 14;
                                        do
                                        {
                                            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec"); }
                                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  請稍後 " + svWaitSec + " 秒"); }
                                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  请稍后 " + svWaitSec + " 秒"); }
                                            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Add(" -> " + mvars.lblCmd + "  待って " + svWaitSec + " 秒"); }
                                            lstget1.TopIndex = lstget1.Items.Count - 1;
                                            Application.DoEvents();
                                            mp.doDelayms(1000);
                                            lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                                            svWaitSec--;
                                        } while (svWaitSec > 0);

                                        //lstget1.Items.Add(" -> " + mvars.lblCmd + " wait for 10s");
                                        //lstget1.TopIndex = lstget1.Items.Count - 1;
                                        //mp.doDelayms(10000);
                                        lstget1.Items.RemoveAt(lstget1.Items.Count - 1);    ///lstget1.Items.Add(" -> " + mvars.lblCmd + " wait for 8s");
                                    }
                                    if (svundoA[svDevCnt] == false)
                                    {
                                        mvars.lblCmd = "MCU_VERSION";
                                        mp.mhVersion();
                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svundoA[svDevCnt] = true; }
                                        else { svactdos++; }
                                        svhwCard[svDevCnt].verMCU = mvars.verMCU;
                                        lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                                        //lstget1.Items.Add(" -> " + mvars.verMCU);     0104
                                        if (mp.IsNumeric(mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4)) == false) { svhwCard[svDevCnt].CurrentAppVer = 0; }
                                        else { svhwCard[svDevCnt].CurrentAppVer = Convert.ToInt16(mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4)); }
                                        lstget1.Items.Add(" -> Update to HexAppVer(" + mvars.HexAppVer + ")，CurrentAppVer(" + svhwCard[svDevCnt].CurrentAppVer + ")");
                                        lstget1.Items.Add("");
                                        lstget1.TopIndex = lstget1.Items.Count - 1;
                                    }
                                    if (svundoB[svDevCnt] == false)
                                    {
                                        mvars.lblCmd = "READ_MCUBOOTVER";
                                        mp.mhMCUFLASHREAD(mvars.MCU_BOOT_VERSION_ADDR.ToString("X8"), 16);
                                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                        {
                                            if (mp.IsNumeric(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)) == false) { svhwCard[svDevCnt].CurrentBootVer = 0; }
                                            else { svhwCard[svDevCnt].CurrentBootVer = Convert.ToInt16(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)); }
                                        }
                                        svhwCard[svDevCnt].verMCUB = mvars.verMCUB; //mvars.verMCUB = Boot-220324-0008
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion -------------------------------------------------- ↑Bootcode/Appcode 燒錄


            do
            {
                mvars.lblCmd = "FPGA_SPI_R";
                mp.mhFPGASPIREAD();
                Application.DoEvents();
                mp.doDelayms(1000);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { break; }
            } while (true);


            svundos = 0;
            for (int svDevCnt = 0; svDevCnt < svDevices; svDevCnt++)
            {
                if (svUserbreak[svDevCnt])
                {
                    lstget1.Items.Add("IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00") + "，User Break");
                    svundos++;
                }
                else
                {
                    if (svundoB[svDevCnt])
                    {
                        string svs = "，Bootcode update fail，redo again and don't trun off the power";
                        if (MultiLanguage.DefaultLanguage == "en-US") { svs = "，Bootcode update fail，redo again and don't trun off the power"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = "，Bootcode 更新失敗，請勿重啟電源，立即重新執行強制燒錄"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = "，Bootcode 更新失败，请勿重启电源，立即重新执行强制烧录"; }
                        lstget1.Items.Add("IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00") + svs);
                        svundos++;
                    }
                    else if (svundoA[svDevCnt])
                    {
                        string svs = "，Appcode update fail";
                        if (MultiLanguage.DefaultLanguage == "en-US") { svs = "，Appcode update fail"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = "，Appcode 更新失敗，請重新執行燒錄"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = "，Appcode 更新失败，请重新执行烧录"; }
                        lstget1.Items.Add("IP" + mvars.deviceID.Substring(0, 2) + svDevCnt.ToString("00") + svs);
                        svundos++;
                    }
                }
            }

            lstget1.Items.Add("");
            if (svundos >= svDevices) lstget1.Items.Add(svundos + " of " + svDevices + " Undo");
            else
            {
                lstget1.Items.Add(svactdos + " of " + svtodos + " Done");
                lstget1.Items.Add("");
                if (svundos > 0) lstget1.Items.Add(svundos + " of " + svDevices + " Undo");
            }


            lstget1.Items.RemoveAt(0);
            lstget1.TopIndex = lstget1.Items.Count - 1;



        ExNovaAGMA:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }

            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (sverr == "000000")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30";
                mvars.errCode = "0";
            }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
                mvars.errCode = sverr;
            }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }



        //for cUpdate
        public static void cFLASHWRITE_pMCU(int svDevices, ListBox lstget1, byte svDev)
        {
            //svDev編號從0起算
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            mvars.isReadBack = true;
            string txt44;
            UInt32 PacketSize;
            UInt32 Count;
            int svlstc = 0;
            string svmsg;

            if (svDevices == 0) svDevices = 1;

            //typhwCard[] svhwCard = new typhwCard[svDevices];
            ushort svundos = 0;
            bool[] svundo = new bool[svDevices];
            bool[] svundoB = new bool[svDevices];
            bool[] svundoA = new bool[svDevices];
            bool[] svunlock = new bool[svDevices];  //預設未解鎖 unlock = false 也就是假設是鎖定狀態

            int svWaitSec;

            string sverr = "000000"; //[0]更新前回讀boot fail =1
                                     //[1]更新前回讀app fail =1
                                     //[2]更新前回讀verMCU fail =1
                                     //[3]code Boot的版本<=硬體的版本 =1
                                     //[4]code App的版本<=硬體的版本 =1
                                     //[5]更新後回讀verMCU fail =1
            bool svAllowBootModeBC = true;     //0052版沒有在boot mode廣播燒app code的功能，0053起新增

            #region ----------------------------------------------------- ↓從頭到尾全部確認過如果有在boot mode就先燒錄讓他回到app mode
            for (byte svd = 1; svd <= svDevices; svd++)
            {
                //string svmsg;
                sverr = "000";
                if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                else
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);      //" 05" + mp.DecToHex(i, 2)

                mvars.lblCmd = "MCU_VERSION";
                if (mvars.demoMode == false) mp.mhVersion();
                else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.verMCU = "App-230612-P0052"; }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                {
                    Form1.hwCard[svd - 1].verMCU = mvars.verMCU;   //AppMode --> mvars.verMCU = App-230612-P0052，BootMode --> mvars.verMCU = Boot-230221-0003

                    bool svreboot = false;
                reUnboot:
                    if (mvars.verMCU.ToUpper().Substring(0, 3) == "BOO")
                    {
                        #region↓ App code write(要先將app燒完回到app mode狀態)(只有使用svundoA)(只要其中有一台出現無法返回app mode就跳出燒錄並顯示警告訊息)
                        sw.Reset();
                        sw.Start();
                        svmsg = "";
                        if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = "ID." + svd.ToString("00") + " is Boot mode,need return to app mode first"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg="ID." + svd.ToString("00") + " 正在 Boot 模式,必須先回復到 app 模式"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg="ID." + svd.ToString("00") + " 正在 Boot 模式,必须先回复到 app 模式"; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg="ID." + svd.ToString("00") + " is Boot mode,need return to app mode first"; }
                        mp.showStatus1(svmsg, lstget1, "");
                        mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + lstget1.Items[lstget1.Items.Count - 1]);

                        PacketSize = mvars.MCU_BLOCK_SIZE;
                        Count = mvars.APP_SIZE / PacketSize;
                        byte sverrc = 0;
                        svmsg = " --> Bootloader Write";
                        mp.showStatus1(svmsg, lstget1, "");
                        mp.showStatus1("", lstget1, "");
                        for (int svcnt = 0; svcnt < Count; svcnt++)
                        {
                        reBlWr:
                            if (svundoA[svd - 1] == false)
                            {
                                txt44 = (svcnt * PacketSize + mvars.APP_START_ADDR).ToString("X8");
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
                                if (mvars.demoMode == false) mp.pMCUAPPWRITE(0x15, txt44);   //Primary dedicated
                                else { mvars.lGet[mvars.lCount - 1] = "DONE"; mvars.lCount++; }
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                {
                                    //只有發送卡的一包2048才需要再重新計算
                                    //int m = (int)svcnt;
                                    //svcnt = (int)(m / 16) * 16;
                                    if (sverrc < 3) { sverrc++; goto reBlWr; }
                                    else { svundoA[svd - 1] = true; }
                                }
                                else
                                {
                                    svmsg = " --> Bootloader Wr cnt：" + svcnt + " of " + (Count - 1);
                                    mp.showStatus1(svmsg, lstget1, "");
                                    mp.showStatus1("", lstget1, "");
                                }
                            }
                        }
                        if (lstget1.Visible == true)
                        {
                            lstget1.Items.RemoveAt(svlstc);                     /// lstget1.Items.Insert(svlstc, " --> Bootloader Wr cnt：" + svcnt + " of " + (Count - 1));
                            lstget1.Items.RemoveAt(lstget1.Items.Count - 1);    /// lstget1.Items.Add("");
                            lstget1.TopIndex = lstget1.Items.Count - 1;
                        }
                        mp.doDelayms(200);  ///500

                        mvars.lblCmd = "MCU_RESET";
                        if (mvars.demoMode == false)
                            mp.McuSW_Reset();

                        svWaitSec = 14;
                        do
                        {
                            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg=" -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg=" -> " + mvars.lblCmd + "  請稍後 " + svWaitSec + " 秒"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg=" -> " + mvars.lblCmd + "  请稍后 " + svWaitSec + " 秒"; }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg=" -> " + mvars.lblCmd + "  待って " + svWaitSec + " 秒"; }
                            mp.showStatus1(svmsg, lstget1, "");
                            Application.DoEvents();
                            mp.doDelayms(1000);
                            if (lstget1.Visible == true) lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                            svWaitSec--;
                        } while (svWaitSec > 0);

                        mvars.lblCmd = "MCU_VERSION";
                        mp.mhVersion();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                        {
                            if (mvars.verMCU.ToUpper().Substring(0, 3) == "BOO")
                            {
                                if (svreboot == false) { svreboot = true; goto reUnboot; }
                                else
                                {
                                    //燒完app code後還是一直維持在boot mode
                                    svmsg = "";
                                    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Add(" -> There is a major abnormality,Please notify maintenance staff"); }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Add(" -> 發生重大異常,請通知維修人員處理"); }
                                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Add(" -> 发生重大异常,请通知维修人员处理"); }
                                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Add(" -> There is a major abnormality,Please notify maintenance staff"); }
                                    if (lstget1.Visible == true)
                                    {

                                    }
                                    lstget1.TopIndex = lstget1.Items.Count - 1;
                                    mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + lstget1.Items[lstget1.Items.Count - 1]);
                                    goto ExNovaAGMA;
                                }
                            }
                            else
                            {
                                Form1.hwCard[svd - 1].verMCU = mvars.verMCU;   //AppMode --> mvars.verMCU = App-230612-P0052，BootMode --> mvars.verMCU = Boot-230221-0003

                                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Add(" -> It is App mode now"); }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Add(" -> 已經是 App 模式"); }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Add(" -> 已經是 App 模式"); }
                                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Add(" -> It is App mode now"); }
                                lstget1.TopIndex = lstget1.Items.Count - 1;
                                mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + lstget1.Items[lstget1.Items.Count - 1]);
                            }
                        }
                        else
                        {
                            //non "DONE" s
                            sverr = mp.ReplaceAt(sverr, 3, "1");
                            mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + mvars.lGet[mvars.lCount - 1]);
                        }
                        #endregion ↑ App code write(要先將app燒完回到app mode狀態)
                    }

                    //get verMCUB
                    mvars.lblCmd = "READ_MCUBOOTVER";
                    if (mvars.demoMode == false) mp.mhMCUFLASHREAD(mvars.MCU_BOOT_VERSION_ADDR.ToString("X8"), 16);
                    else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.verMCUB = "Boot-230221-0003"; }
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                    {
                        sverr = mp.ReplaceAt(sverr, 1, "1");
                        mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + mvars.lGet[mvars.lCount - 1]);
                    }
                    else
                    {
                        if (mp.IsNumeric(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)) == false) { Form1.hwCard[svd - 1].CurrentBootVer = 0; }
                        //mvars.verMCUB = Boot-230221-0003                                       
                        else { Form1.hwCard[svd - 1].CurrentBootVer = Convert.ToInt16(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)); }
                        //svhwCard[svd-1].CurrentBootVer = 3:(boot code沒有廣播功能) 4:(boot code增加廣播功能)
                        if (mvars.HexBootVer <= Form1.hwCard[svd - 1].CurrentBootVer) svundoB[svd - 1] = true;
                    }

                    //get verMCUA
                    mvars.lblCmd = "READ_MCUAPPVER";
                    if (mvars.demoMode == false) mp.mhMCUFLASHREAD(mvars.MCU_APP_VERSION_ADDR.ToString("X8"), 16);
                    else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.verMCUA = "App-230612-P0052"; }
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    {
                        if (mp.IsNumeric(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)) == false) { Form1.hwCard[svd - 1].CurrentAppVer = 0; }
                        //mvars.verMCUA = App-230612-P0052
                        else { Form1.hwCard[svd - 1].CurrentAppVer = Convert.ToInt16(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)); }
                        //svhwCard[svd-1].CurrentAppVer = 52
                        if (mvars.HexAppVer <= Form1.hwCard[svd - 1].CurrentAppVer) svundoA[svd - 1] = true;
                    }
                    else
                    {
                        //non "DNOE" situation
                        sverr = mp.ReplaceAt(sverr, 2, "1");
                        mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + mvars.lGet[mvars.lCount - 1]);
                    }

                    mp.showStatus1("", lstget1, "");    //not record
                    mp.showStatus1("(" + sverr + ") ID." + svd.ToString("00"), lstget1, "");
                    mp.showStatus1(" -> Hexfile  ver. (Boot,App,MCU)：" + mvars.HexBootVer + "," + mvars.HexAppVer + ",---", lstget1, "");

                    if (sverr.Substring(0, 3) == "111")
                    {
                        mp.showStatus1(" ->  Error,errCode: " + sverr, lstget1, "");

                        svundo[svd - 1] = true;     // 3個版本都沒回，可能是MCU已經掛了
                        svundoA[svd - 1] = true;    // 既然都沒回應那要把其他的錯誤旗標也拉起來
                        svundoB[svd - 1] = true;    // 既然都沒回應那要把其他的錯誤旗標也拉起來
                        //goto ExNovaAGMA;
                    }
                    else
                    {
                        mp.showStatus1(" -> Current ver. (Boot,App,MCU)："
                                            + Form1.hwCard[svd - 1].CurrentBootVer + ","
                                            + Form1.hwCard[svd - 1].CurrentAppVer + ","
                                            + Form1.hwCard[svd - 1].verMCU, lstget1, "");

                        //強制燒錄
                        if (mvars.flgForceUpdate)
                        {
                            svundoA[svd - 1] = false;
                            svundoB[svd - 1] = false;
                            svmsg = " -> Force Update Enabled";
                            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = " -> Force Update Enabled"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> 啟用強制更新"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> 启用强制更新"; }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> Force Update Enabled"; }
                            mp.showStatus1(svmsg, lstget1, "");
                        }

                        mvars.lblCmd = "READ_NVM_USERPAGE";
                        if (mvars.demoMode == false) mp.mNvmUserPageRd();
                        else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; }
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                        {
                            if (mvars.demoMode == false)
                            {
                                Byte BootSizeVal, Hyst; UInt16 BOD_Level;
                                mp.Copy(mvars.ReadDataBuffer, 6 + 1, mvars.NVM_UserPage, 0, mvars.NVM_UserPage.Length);
                                if ((mvars.NVM_UserPage[0] & 0x01) == 0x01) { mvars.sBOD33 = "BOD33 Disable"; } //[0]56 [1]146 [2]154 [3]222
                                else { mvars.sBOD33 = "BOD33 Enable"; }
                                BootSizeVal = (Byte)((mvars.NVM_UserPage[3] & 0x3C) / 4);
                                mvars.sBootSizeVal = ((15 - BootSizeVal) * 8192).ToString();
                                BOD_Level = (UInt16)(mvars.NVM_UserPage[0] + mvars.NVM_UserPage[1] * 256);
                                BOD_Level = (UInt16)((BOD_Level & 0x01FE) / 2);
                                mvars.sBOD_Level = BOD_Level.ToString();
                                Hyst = (Byte)((mvars.NVM_UserPage[1] & 0x78) / 8); mvars.sHyst = Hyst.ToString();
                                mvars.sBODminus = (1.5 + BOD_Level * 0.006).ToString(); mvars.sBODplus = (1.5 + BOD_Level * 0.006 + Hyst * 0.006).ToString();

                                if (mvars.sBootSizeVal == "65536")  // 未解除鎖定 Code Protect
                                {
                                    mvars.lblCmd = "DIS_PROTBOD33";
                                    mp.pDisProtBOD33();
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                                    {
                                        svunlock[svd - 1] = true;
                                        svmsg = " -> Disable CodeProtect,DONE";
                                        mp.showStatus1(svmsg, lstget1, "");
                                    }
                                    else
                                    {
                                        svmsg = " -> Disable CodeProtect,Error";
                                        mp.showStatus1(svmsg, lstget1, "");
                                    }
                                }
                            }
                            else
                            {
                                svmsg = " -> DemoMode Disable CodeProtect,DONE";
                                mp.showStatus1(svmsg, lstget1, "");
                            }
                        }
                        else
                        {
                            svmsg = " -> Code Protect：UnKnow";
                            mp.showStatus1(svmsg, lstget1, "");
                        }
                    }

                    /// svundo的累加判斷
                    /// --> mvars.errCode != "000"
                    /// --> mNvmUserPageRd feedback Error
                    /// --> 非廣播而且不是選擇的接收卡
                    /// --> mvars.sBootSizeVal != "0" and mvars.sBootSizeVal != "65536" ，一定是這兩個值
                    /// --> Hex檔內含的(HexBootVer/HexAppVer)版本比硬體回讀的(CurrentBootVer/CurrentAppVer)舊而且沒有使用強燒旗標
                }
                else
                {
                    //non "DONE" situation
                    sverr = mp.ReplaceAt(sverr, 3, "1");
                    mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + mvars.lGet[mvars.lCount - 1]);
                    svundo[svd - 1] = true;     // 3個版本都沒回，可能是MCU已經掛了
                    svundoA[svd - 1] = true;    // 既然都沒回應那要把其他的錯誤旗標也拉起來
                    svundoB[svd - 1] = true;    // 既然都沒回應那要把其他的錯誤旗標也拉起來
                    break;
                }
            }
            #endregion -------------------------------------------------- ↑從頭到尾全部確認過如果有在boot mode就先燒錄讓他回到app mode


            #region ----------------------------------------------------- ↓Boot code Update
            svundos = 0;
            for (byte svd = 1; svd <= svDevices; svd++) if (svundoB[svd - 1]) svundos++;

            if (svundos < svDevices)
            {
                for (byte svd = 1; svd <= svDevices; svd++)
                {
                    if (svundoB[svd - 1] == false)
                    {
                        mp.showStatus1("", lstget1, "");

                        //if (svAllowBBC)
                        //{
                        svmsg = " ID.A0 (ALL Devices)";
                        mp.showStatus1(svmsg, lstget1, "");

                        mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                        //}
                        //else
                        //{
                        //    lstget1.Items.Add(" ID." + svd.ToString("00"));
                        //    if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                        //    else mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);
                        //}

                        mvars.lblCmd = "MCU_RESET";
                        if (mvars.demoMode == false) mp.mhMCUSWRESET();
                        svWaitSec = 10;
                        do
                        {
                            svmsg = " -> Boardcast MCU Reset, Please wait " + svWaitSec + " sec";
                            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg=" -> Boardcast MCU Reset, Please wait " + svWaitSec + " sec"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg=" -> MCU 廣播重置中, 請稍後 " + svWaitSec + " 秒"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg=" -> MCU 广播重置中, 请稍后 " + svWaitSec + " 秒"; }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg=" -> Boardcast MCU Reset, Please wait " + svWaitSec + " sec"; }
                            mp.showStatus1(svmsg, lstget1, "(pMCU)  ");

                            Application.DoEvents();
                            mp.doDelayms(1000);
                            //lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                            svWaitSec--;
                        } while (svWaitSec > 0);

                        PacketSize = mvars.MCU_BLOCK_SIZE;
                        Count = mvars.BOOT_SIZE / PacketSize;
                        byte sverrc = 0;

                        mp.showStatus1(" --> Boot Write", lstget1, "(pMCU)  ");

                        for (int svcnt = 0; svcnt < Count; svcnt++)
                        {
                        reBtWr:
                            txt44 = (svcnt * PacketSize).ToString("X8");
                            Application.DoEvents();
                            mp.Copy(mvars.gMcuBinFile, (int)(svcnt * PacketSize), mvars.gFlashRdPacketArr, 0, (int)PacketSize);

                            mvars.lblCmd = "MCU_BLWRITE";
                            if (mvars.demoMode == false) mp.pMCUBLWRITE(0x15, txt44);   //Primary dedicated
                            else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; }
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                            {
                                mp.doDelayms(200);  //廣播燒錄的話加一下 dodelayms 時間
                                sverrc = 0;
                                mp.showStatus1("--> Boot Wr cnt：" + svcnt + " of " + (Count - 1), lstget1, "(pMCU)  ");
                            }
                            else
                            {
                                //non "DONE" situation
                                //只有發送卡的一包2048才需要再重新計算
                                //int m = (int)svcnt;
                                //svcnt = (int)((m / 16) * 16);
                                if (sverrc < 3) { sverrc++; goto reBtWr; }
                                else
                                {
                                    //待編輯，Appcode 未燒錄完成的程式碼
                                    //
                                    //
                                    //
                                    //
                                    //
                                    //
                                    //
                                    //
                                }
                            }
                        }
                        mp.doDelayms(500);

                        if (svunlock[svd - 1])
                        {
                            /// 0104 
                            mvars.lblCmd = "EN_PROTBOD33";
                            if (mvars.demoMode == false) mp.pEnProtBOD33();
                            else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; }
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                            {
                                svunlock[svd - 1] = false;
                                mp.showStatus1(" -> Enable CodeProtect", lstget1, "(pMCU)  ");
                            }
                            else
                            {
                                mp.showStatus1(" -> Enable CodeProtect Fail", lstget1, "(pMCU)  ");
                            }
                        }
                        svmsg = " -> Boot Code Updated";
                        if (MultiLanguage.DefaultLanguage == "en-US") { svmsg=" -> Boot Code Updated"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> Boot Code 已更新"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> Boot Code 已更新"; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> Boot Code Updated"; }
                        mp.showStatus1(svmsg, lstget1, "(pMCU)  ");
                    }
                    if (svAllowBootModeBC) break;   //保留給後續非廣播
                }
            }
            //get verMCUB //0052版的boot code是0003版沒有在boot mode廣播燒app code的功能，0053+0004起新增
            mvars.lblCmd = "READ_MCUBOOTVER";
            if (mvars.demoMode == false) mp.mhMCUFLASHREAD(mvars.MCU_BOOT_VERSION_ADDR.ToString("X8"), 16);
            else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.verMCUB = "Boot-230221-0004"; }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) svAllowBootModeBC = false;
            else
            {
                if (Convert.ToInt16(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)) < 4) svAllowBootModeBC = false;
            }
            #endregion -------------------------------------------------- ↑Boot code Update


            #region ----------------------------------------------------- ↓App code Update
            svundos = 0;
            for (byte svd = 1; svd <= svDevices; svd++) if (svundoA[svd - 1]) svundos++;

            if (svundos < svDevices)
            {
                //for (byte svd = 1; svd <= svDevices; svd++)
                //{
                //    if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                //    else mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);
                //    if (svundoA[svd - 1])
                //    {
                //        mvars.lblCmd = "CMDBYPASS_ON";
                //        mp.pMCUCMDBYPASS(0x11);
                //        lstget1.Items.Add("");
                //        lstget1.Items.Add("(" + sverr + ") ID." + svd.ToString("00"));
                //        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                //            lstget1.Items.Add(" --> MCU_CMD_ByPass,DONE");
                //        else
                //            lstget1.Items.Add(" --> MCU_CMD_ByPass,Fail");
                //    }
                //}
                //string svmsg;

                for (byte svd = 1; svd <= svDevices; svd++)
                {
                    if (svundoA[svd - 1] == false)
                    {
                        mp.showStatus1("", lstget1, "");
                        if (svAllowBootModeBC)
                        {
                            mp.showStatus1(" ID.A0 (ALL Devices)", lstget1, "");
                            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                        }
                        else
                        {
                            mp.showStatus1(" ID." + svd.ToString("00"), lstget1, "");
                            if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                            else
                                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);      //" 05" + mp.DecToHex(i, 2)
                        }

                        mvars.lblCmd = "MCU_BLMODE";
                        if (mvars.demoMode == false) mp.mhMCUBLMODE();
                        else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; }
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                        {
                            mp.showStatus1(" -> App mode --> Boot mode", lstget1, "");
                            mvars.lblCmd = "MCU_RESET";
                            if (mvars.demoMode == false) mp.mhMCUSWRESET();
                            svWaitSec = 6;
                            do
                            {
                                svmsg = " -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec";
                                if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = " -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec"; }
                                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> " + mvars.lblCmd + "  請稍後 " + svWaitSec + " 秒"; }
                                else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> " + mvars.lblCmd + "  请稍后 " + svWaitSec + " 秒"; }
                                else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> " + mvars.lblCmd + "  待って " + svWaitSec + " 秒"; }
                                mp.showStatus1(svmsg, lstget1, "");

                                Application.DoEvents();
                                mp.doDelayms(1000);
                                //lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                                //lstget1.TopIndex = lstget1.Items.Count - 1;
                                svWaitSec--;
                            } while (svWaitSec > 0);
                            //lstget1.Items.RemoveAt(lstget1.Items.Count - 1);    /// lstget1.Items.Add(" -> App mode --> Boot mode");
                            //lstget1.TopIndex = lstget1.Items.Count - 1;
                        }
                        else
                        {
                            //non "DONE" situation
                            //燒完app code後還是一直維持在boot mode
                            svmsg = " -> (Boardcast) The App mode switch to Boot mode is Fail,Please re-Do";
                            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg=" -> (Boardcast) The App mode switch to Boot mode is Fail,Please re-Do"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> (廣播) App 模式切換到 Boot 模式發生異常,請重新執行"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> (广播) App 模式切换到 Boot 模式发生异常,请重新执行"; }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> (Boardcast) The App mode switch to Boot mode is Fail,Please re-Do"; }
                            mp.showStatus1(svmsg, lstget1, "");
                            goto ExNovaAGMA;
                        }

                        PacketSize = mvars.MCU_BLOCK_SIZE;
                        Count = mvars.APP_SIZE / PacketSize;
                        byte sverrc = 0;
                        mp.showStatus1(" --> Bootloader Write", lstget1, "");
                        mp.showStatus1("", lstget1, "");
                        for (int svcnt = 0; svcnt < Count; svcnt++)
                        {
                        reBl2Wr:
                            txt44 = (svcnt * PacketSize + mvars.APP_START_ADDR).ToString("X8");
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
                            if (mvars.demoMode == false) mp.pMCUAPPWRITE(0x15, txt44);   //Primary dedicated
                            else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; }
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                            {
                                mp.doDelayms(100);  //廣播燒錄的話加一下 dodelayms 時間
                                mp.showStatus1(" --> Bootloader Wr cnt：" + svcnt + " of " + (Count - 1), lstget1, "");
                            }
                            else
                            {
                                //non "DONE" situation
                                //只有發送卡的一包2048才需要再重新計算
                                //int m = (int)svcnt;
                                //svcnt = (int)(m / 16) * 16;
                                if (sverrc < 3) { sverrc++; goto reBl2Wr; }
                            }
                        }
                        mp.doDelayms(500);
                        mvars.lblCmd = "MCU_RESET";
                        if (mvars.demoMode == false) mp.McuSW_Reset();

                        svWaitSec = 14;
                        do
                        {
                            svmsg = " -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec";
                            if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = " -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> " + mvars.lblCmd + "  請稍後 " + svWaitSec + " 秒"; }
                            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> " + mvars.lblCmd + "  请稍后 " + svWaitSec + " 秒"; }
                            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> " + mvars.lblCmd + "  待って " + svWaitSec + " 秒"; }
                            mp.showStatus1(svmsg, lstget1, "");
                            Application.DoEvents();
                            mp.doDelayms(1000);
                            //lstget1.Items.RemoveAt(lstget1.Items.Count - 1);
                            svWaitSec--;
                        } while (svWaitSec > 0);

                        svmsg = " -> App Code Updated";
                        if (MultiLanguage.DefaultLanguage == "en-US") { svmsg=" -> App Code Updated"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> App Code 已更新"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> App Code 已更新"; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> App Code Updated"; }
                        mp.showStatus1(svmsg, lstget1, "");
                    }
                    if (svAllowBootModeBC) break;   //保留給後續非廣播
                }
            }

            #endregion -------------------------------------------------- ↑App code Update


            for (byte svd = 1; svd <= svDevices; svd++)
            {

                sverr = "000";
                if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                else
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);      //" 05" + mp.DecToHex(i, 2)

                mvars.lblCmd = "MCU_VERSION";
                if (mvars.demoMode == false) mp.mhVersion();
                else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.verMCU = "App-230612-P0060"; }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                {
                    Form1.hwCard[svd - 1].verMCU = mvars.verMCU;   //AppMode --> mvars.verMCU = App-230612-P0052，BootMode --> mvars.verMCU = Boot-230221-0003

                    //get verMCUB
                    mvars.lblCmd = "READ_MCUBOOTVER";
                    if (mvars.demoMode == false) mp.mhMCUFLASHREAD(mvars.MCU_BOOT_VERSION_ADDR.ToString("X8"), 16);
                    else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.verMCUB = "Boot-230221-0004"; }
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                    {
                        sverr = mp.ReplaceAt(sverr, 1, "1");
                        mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + mvars.lGet[mvars.lCount - 1]);
                    }
                    else
                    {
                        if (mp.IsNumeric(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)) == false) { Form1.hwCard[svd - 1].CurrentBootVer = 0; }
                        //mvars.verMCUB = Boot-230221-0003                                       
                        else { Form1.hwCard[svd - 1].CurrentBootVer = Convert.ToInt16(mvars.verMCUB.Substring(mvars.verMCUB.Length - 4, 4)); }
                        //svhwCard[svd-1].CurrentBootVer = 3
                        if (mvars.HexBootVer <= Form1.hwCard[svd - 1].CurrentBootVer) svundoB[svd - 1] = true;
                    }

                    //get verMCUA
                    mvars.lblCmd = "READ_MCUAPPVER";
                    if (mvars.demoMode == false) mp.mhMCUFLASHREAD(mvars.MCU_APP_VERSION_ADDR.ToString("X8"), 16);
                    else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; mvars.verMCUA = "App-230612-P0060"; }
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    {
                        if (mp.IsNumeric(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)) == false) { Form1.hwCard[svd - 1].CurrentAppVer = 0; }
                        //mvars.verMCUA = App-230612-P0052
                        else { Form1.hwCard[svd - 1].CurrentAppVer = Convert.ToInt16(mvars.verMCUA.Substring(mvars.verMCUA.Length - 4, 4)); }
                        //svhwCard[svd-1].CurrentAppVer = 52
                        if (mvars.HexAppVer <= Form1.hwCard[svd - 1].CurrentAppVer) svundoA[svd - 1] = true;
                    }
                    else
                    {
                        //non "DNOE" situation
                        sverr = mp.ReplaceAt(sverr, 2, "1");
                        mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + mvars.lGet[mvars.lCount - 1]);
                    }

                    mp.showStatus1("", lstget1, "");
                    mp.showStatus1("(" + sverr + ") ID." + svd.ToString("00"), lstget1, "");
                    mp.showStatus1(" -> Hexfile  ver. (Boot,App,MCU)：" + mvars.HexBootVer + "," + mvars.HexAppVer + ",---", lstget1, "");


                    if (sverr.Substring(0, 3) == "111")
                    {
                        mp.showStatus1(" ->  Error,errCode: " + sverr, lstget1, "");
                        svundo[svd - 1] = true;     // 3個版本都沒回，可能是MCU已經掛了
                        svundoA[svd - 1] = true;    // 既然都沒回應那要把其他的錯誤旗標也拉起來
                        svundoB[svd - 1] = true;    // 既然都沒回應那要把其他的錯誤旗標也拉起來
                        //goto ExNovaAGMA;
                    }
                    else
                    {
                        mp.showStatus1(" -> Current ver. (Boot,App,MCU)："
                            + Form1.hwCard[svd - 1].CurrentBootVer + ","
                            + Form1.hwCard[svd - 1].CurrentAppVer + ","
                            + Form1.hwCard[svd - 1].verMCU, lstget1, "");
                    }

                    /// svundo的累加判斷
                    /// --> mvars.errCode != "000"
                    /// --> mNvmUserPageRd feedback Error
                    /// --> 非廣播而且不是選擇的接收卡
                    /// --> mvars.sBootSizeVal != "0" and mvars.sBootSizeVal != "65536" ，一定是這兩個值
                    /// --> Hex檔內含的(HexBootVer/HexAppVer)版本比硬體回讀的(CurrentBootVer/CurrentAppVer)舊而且沒有使用強燒旗標
                }
                else
                {
                    //non "DONE" situation
                    sverr = mp.ReplaceAt(sverr, 3, "1");
                    mp.funSaveLogs("(pMCU)  ID." + svd.ToString("00") + mvars.lGet[mvars.lCount - 1]);
                }

                svundos = 0;
                for (byte svdev = 1; svdev <= svDevices; svdev++) if (svunlock[svdev - 1]) { svundos++; break; }
                if (svundos != 0)
                {
                    /// 0104 
                    mvars.lblCmd = "EN_PROTBOD33";
                    if (mvars.demoMode == false) mp.pEnProtBOD33();
                    else { mvars.lGet[mvars.lCount] = "DONE"; mvars.lCount++; }
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    {
                        mp.showStatus1(" -> Enable CodeProtect", lstget1, "(pMCU)  ");
                    }
                    else
                    {
                        mp.showStatus1(" -> Enable CodeProtect Fail", lstget1, "(pMCU)  ");
                    }
                }
            }


        ExNovaAGMA:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }

            //恢復USB單一
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";

            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            //if (sverr == "000000")
            //{
            mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30";
            mvars.errCode = "0";
            //}
            //else
            //{
            //    mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
            //    mvars.errCode = sverr;
            //}
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }





        public static void cFLASHWRITE_pCB(string svtitle, ListBox lstget1, string svcodename)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mp.markreset(9999, false);
            mvars.flgDelFB = true;
            string svErr = "0";
            int i;
            string svdeviceID = mvars.deviceID;
            string svverFPGA = "";
            ushort svundos = 0;
            bool[] svundo = new bool[Form1.nvsender[0].regPoCards];
            bool[] svbypass = new bool[Form1.nvsender[0].regPoCards];
            string svFlashSelName = "FPGA_";
            //Primary FPGA A side，mvars.flashselQ=1 
            //Primary FPGA B side，mvars.flashselQ=2 
            byte svflashselQ = mvars.flashselQ;



            #region ----------------------------------------------------- ↓以版本確認svundo
            for (byte svd = 1; svd <= Form1.nvsender[0].regPoCards; svd++) svundo[svd - 1] = true;

            for (byte svd = 1; svd <= Form1.nvsender[0].regPoCards; svd++)
            {
                if (svtitle.IndexOf("FPGA", 0) != -1)
                {
                    if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                    else
                        mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);      //" 05" + mp.DecToHex(i, 2)

                    Form1.pvindex = 0;
                    mvars.lblCmd = "FPGA_SPI_R";
                    mp.mhFPGASPIREAD();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) //{ mp.showStatus1(mvars.lGet[mvars.lCount - 1], lstget1, ""); svErr = "-2"; goto Ex; }
                    {
                        svverFPGA = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                        if (svflashselQ == 1) Form1.hwCard[svd - 1].verFPGA = svverFPGA.Split('-')[svflashselQ - 1];
                        else if (svflashselQ == 2) Form1.hwCard[svd - 1].verFPGA += "-" + svverFPGA.Split('-')[svflashselQ - 1];
                        byte[] tmp = new byte[] { (byte)(64 + svflashselQ) };
                        svFlashSelName = "FPGA_" + Encoding.ASCII.GetString(tmp);
                        mp.showStatus1("VERSION," + svFlashSelName + "," + svverFPGA.Split('-')[svflashselQ - 1], lstget1, "");
                        if (svcodename.IndexOf(svverFPGA.Split('-')[svflashselQ - 1], 0) == -1) svundo[svd - 1] = false;
                        if (mvars.flgForceUpdate && svundo[svd - 1]) svundo[svd - 1] = false;
                    }
                    else break;
                }
                else mp.showStatus1(svtitle, lstget1, "");
            }
            #endregion -------------------------------------------------- ↑以版本確認svundo

            #region ----------------------------------------------------- ↓FPGA code Update
            svundos = 0;
            for (byte svd = 1; svd <= Form1.nvsender[0].regPoCards; svd++) if (svundo[svd - 1]) svundos++;

            if (svundos < Form1.nvsender[0].regPoCards)
            {
                mp.showStatus1("", lstget1, "");
                for (byte svd = 1; svd <= Form1.nvsender[0].regPoCards; svd++)
                {
                    //if (svundo[svd - 1])
                    //{
                    //    if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                    //    else
                    //      mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);      //" 05" + mp.DecToHex(i, 2)
                    //    mvars.lblCmd = "CMDBYPASS_ON";
                    //    mp.pMCUCMDBYPASS(0x11);
                    //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    //        mp.showStatus1(" ID." + svd.ToString("00") + "," + svFlashSelName + ", byPass,OK", lstget1, "");
                    //    else mp.showStatus1(" ID." + svd.ToString("00") + "," + svFlashSelName + ", byPass,Fail", lstget1, ""); 
                    //}
                }
                string svmsg;
                for (byte svd = 1; svd <= Form1.nvsender[0].regPoCards; svd++)
                {
                    if (svundo[svd - 1] == false)
                    {
                        mp.showStatus1("", lstget1, "");
                        mp.showStatus1(" ID.A0 (ALL Devices)", lstget1, "");
                        mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";

                        mvars.lblCmd = "FLASH_TYPE";
                        mhFLASHTYPE();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { svErr = "-3"; goto Ex; }
                        //mvars.lblCmd = "FLASH_WRITEEN";
                        //mhFUNCENABLE();
                        //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svErr = "-4"; goto Ex; }
                        mvars.lblCmd = "FLASH_FUNCQE";
                        mhFUNCQE();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svErr = "-4"; goto Ex; }
                        if (svErr != "0") goto Ex;

                        System.Diagnostics.Stopwatch swj = new System.Diagnostics.Stopwatch();
                        swj.Reset();
                        swj.Start();

                        byte svcmd = 0x40;
                        if (svtitle.ToUpper().IndexOf("XB") != -1) svcmd = 0x42;
                        UInt32 FlashSize = (UInt32)mvars.ucTmp.Length;
                        ushort PacketSize = 32768;
                        UInt32 Count = FlashSize / PacketSize;
                        byte svdenominator = 8;
                        if (Count < 20) svdenominator = 1;
                        for (i = 0; i < Count; i++)
                        {
                            string textBox13 = (i * PacketSize).ToString("X8");
                            if (svtitle.ToUpper().IndexOf("CB") != -1 || svtitle.ToUpper().IndexOf("FPGA") != -1)
                            {
                                if (textBox13 == "00000000")
                                {
                                    mvars.lblCmd = "FLASH_WRITEEN";
                                    mhFUNCENABLE();
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svErr = "-4"; goto Ex; }

                                    mvars.lblCmd = "FLASH_WREAR";
                                    mp.mhWREAR("00");
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                    {
                                        mp.showStatus1(" -> " + svtitle +
                                        " QSPI_WREAR 00 fail @ " + String.Format("{0:00}", ((i + 1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", swj.Elapsed.TotalSeconds)) + "sec", lstget1, "");
                                        mp.showStatus1("", lstget1, "");
                                        goto Ex;
                                    }
                                }
                                if (textBox13 == "01000000")
                                {
                                    mvars.lblCmd = "FLASH_WRITEEN";
                                    mhFUNCENABLE();
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { svErr = "-4"; goto Ex; }

                                    mvars.lblCmd = "FLASH_WREAR";
                                    mp.mhWREAR("01");
                                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                                    {
                                        mp.showStatus1(" -> " + svtitle +
                                        " QSPI_WREAR 00 fail @ " + String.Format("{0:00}", ((i + 1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", swj.Elapsed.TotalSeconds)) + "sec", lstget1, "");
                                        mp.showStatus1("", lstget1, "");
                                        goto Ex;
                                    }

                                }
                            }
                            if ((i + 1) % svdenominator == 0)
                            {
                                mp.showStatus1(" -> " + svtitle +
                                    " flash_write " + textBox13 + " @ " + String.Format("{0:00}", ((i + 1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", swj.Elapsed.TotalSeconds)) + "sec", lstget1, "");
                            }
                            Application.DoEvents();
                            mvars.lblCmd = "FLASH_WRITE_" + i.ToString("0000");
                            Application.DoEvents();
                            mhFLASHWRITEPAGEQIO(textBox13, PacketSize, svcmd, 300);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                            {
                                mp.showStatus1(" -> " + svtitle +
                                    " flash_write " + i.ToString("0000") + " fail @ " + String.Format("{0:00}", ((i + 1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", swj.Elapsed.TotalSeconds)) + "sec", lstget1, "");
                                mp.showStatus1("", lstget1, "");
                                goto Ex;
                            }
                            if (mvars.Break)
                            {
                                mp.showStatus1(" -> " + svtitle +
                                    " flash_write " + i.ToString("0000") + " BREAK @ " + String.Format("{0:00}", ((i + 1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec", lstget1, "");
                                mp.showStatus1("", lstget1, "");
                                break;
                            }
                            mp.doDelayms(300);
                        }
                        svmsg = " -> FPGA Code Updated";
                        if (MultiLanguage.DefaultLanguage == "en-US") { svmsg = " -> FPGA Code Updated"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> FPGA Code 已更新"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> FPGA Code 已更新"; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> FPGA Code Updated"; }
                        mp.showStatus1(svmsg, lstget1, "");
                    }
                    if (mvars.deviceID.Substring(2, 2) == "A0") break;  //廣播即跳出
                }

                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";

                mvars.lblCmd = "CMDBYPASS_OFF";
                mp.pMCUCMDBYPASS(0x00);
                mp.doDelayms(1000);

                mvars.flashselQ = 0;
                mvars.lblCmd = "FLASH_TYPE";
                mhFLASHTYPE();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    mp.showStatus1(" -> " + svtitle + " write OK but FLASH_TYPE switch to \"Default\" fail", lstget1, "");
                    mvars.flashselQ = svflashselQ;
                    svErr = "-14";
                }
                else
                {
                    mp.doDelayms(500);
                    if (mvars.FormShow[14] == true) uc_Flash.cmbFlashSel.SelectedIndex = 0;
                    else if (mvars.FormShow[3] == true) uc_coding.cmbFlashSel.SelectedIndex = 0;
                }
                svmsg = " -> FPGA HW_RESET";
                if (MultiLanguage.DefaultLanguage == "en-US") svmsg = " -> FPGA HW_RESET";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") svmsg = " -> FPGA 硬體重置";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") svmsg = " -> FPGA 硬体重置";
                mp.showStatus1(svmsg, lstget1, "");
                mvars.lblCmd = "FPGA_HW_RESET";
                mp.mhFPGARESET(0x80);

                int svWaitSec = 7;
                do
                {
                    svmsg = " -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec";
                    if (MultiLanguage.DefaultLanguage == "en-US") { svmsg=" -> " + mvars.lblCmd + "  Please wait " + svWaitSec + " sec"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svmsg = " -> " + mvars.lblCmd + "  請稍後 " + svWaitSec + " 秒"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svmsg = " -> " + mvars.lblCmd + "  请稍后 " + svWaitSec + " 秒"; }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { svmsg = " -> " + mvars.lblCmd + "  待って " + svWaitSec + " 秒"; }
                    mp.showStatus1(svmsg, lstget1, "");
                    Application.DoEvents();
                    mp.doDelayms(1000);
                    svWaitSec--;
                } while (svWaitSec > 0);

                if (svtitle.ToUpper().IndexOf("FPGA", 0) != -1)
                {
                    mp.showStatus1("", lstget1, "");
                    for (byte svd = 1; svd <= Form1.nvsender[0].regPoCards; svd++)
                    {
                        if (svtitle.IndexOf("FPGA", 0) != -1)
                        {
                            if (svd == 1) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
                            else 
                                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svd, 2);      //" 05" + mp.DecToHex(i, 2)

                            Form1.pvindex = 0;
                            mvars.lblCmd = "FPGA_SPI_R";
                            mp.mhFPGASPIREAD();
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { mp.showStatus1(mvars.lGet[mvars.lCount - 1], lstget1, ""); svverFPGA = "0-0"; }
                            else svverFPGA = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                            if (svflashselQ == 1) Form1.hwCard[svd - 1].verFPGA = svverFPGA.Split('-')[svflashselQ - 1];
                            else if (svflashselQ == 2) Form1.hwCard[svd - 1].verFPGA += "-" + svverFPGA.Split('-')[svflashselQ - 1];
                            //byte[] tmp = new byte[] { (byte)(64 + svflashselQ) };
                            //svFlashSelName = "FPGA_" + Encoding.ASCII.GetString(tmp);

                            if (svcodename.IndexOf(svverFPGA.Split('-')[svflashselQ - 1], 0) == -1 && svverFPGA != "0-0")
                                mp.showStatus1(" ID." + svd.ToString("00") + "," + svFlashSelName + ",ver." + svverFPGA.Split('-')[svflashselQ - 1], lstget1, "");
                            else
                                mp.showStatus1(" ID." + svd.ToString("00") + "," + svFlashSelName + ",ver." + svverFPGA.Split('-')[svflashselQ - 1], lstget1, "");

                        }
                        else mp.showStatus1(svtitle, lstget1, "");
                    }
                }
                else
                {
                    svmsg = " -> " + svtitle + " write OK";
                    if (MultiLanguage.DefaultLanguage == "en-US") svmsg = " -> " + svtitle + " write OK";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") svmsg = " -> " + svtitle + " 寫入完成";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") svmsg = " -> " + svtitle + " 写入完成";
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") svmsg = " -> " + svtitle + " write OK";
                    mp.showStatus1(svmsg, lstget1, "");
                }
            }

            #endregion -------------------------------------------------- ↑FPGA code Update


        Ex:            
            if (mvars.demoMode == false && mvars.sp1.IsOpen) mvars.sp1.Close();
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
            mvars.flgDelFB = false;
            mp.showStatus1("", lstget1, "");
            mvars.lCounts = mvars.lCount;
            Form1.tslbltarget.Text = mvars.lCounts.ToString();
        }




        public static void cFLASHWRITE_pCB(string svtitle, ListBox lst_get1)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mp.markreset(9999, false);
            mvars.flgDelFB = true;
            string SvErr = "0";
            string svcomm = Form1.tslblCOM.Text;
            bool svNVBC = mvars.nvBoardcast;
            byte svFPGAsel = mvars.FPGAsel;
            mvars.nIsReadback = true;
            int i;

            string svverFPGA = "";
            if (svtitle.IndexOf("FPGA", 0) != -1)
            {
                //FPGA A side，mvars.flashselQ=0 
                //FPGA B side，mvars.flashselQ=1 
                //mvars.lblCmd = "MCU_VERSION";
                //mp.mhVersion();
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lst_get1.Items.Add(mvars.lGet[mvars.lCount - 1]); SvErr = "-1"; goto Ex; }
                Form1.pvindex = 0;
                mvars.lblCmd = "FPGA_SPI_R";
                mp.mhFPGASPIREAD();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { lst_get1.Items.Add(mvars.lGet[mvars.lCount - 1]); SvErr = "-2"; goto Ex; }
                //svverFPGA = mvars.verFPGA.Split('~')[mvars.flashselQ - 1];    //20230530 順源操作影像調整後發現默認值會導致drop失效，debug
                svverFPGA = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                if (svtitle.ToUpper().IndexOf("BOARDCAST",0) != -1)
                    lst_get1.Items.Add("VERSION,MCU," + mvars.verMCU + "," + svtitle + "," + svverFPGA);
                else
                    lst_get1.Items.Add("VERSION,MCU," + mvars.verMCU + "," + svtitle + "," + svverFPGA.Split('-')[mvars.flashselQ - 1]);
            }
            else lst_get1.Items.Add(svtitle);
            lst_get1.TopIndex = lst_get1.Items.Count - 1;

            if (svtitle.ToUpper().IndexOf("FPGA_BOARDCAST", 0) != -1)
            {
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";     //廣播啟用
                mvars.FPGAsel = 2;
            }

            lst_get1.TopIndex = lst_get1.Items.Count - 1;
            mvars.lblCmd = "FLASH_TYPE";
            mhFLASHTYPE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { SvErr = "-3"; goto Ex; }
            //mvars.lblCmd = "FLASH_WRITEEN";
            //mhFUNCENABLE();
            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }
            mvars.lblCmd = "FLASH_FUNCQE";
            mhFUNCQE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }
            if (SvErr != "0") goto Ex;

            System.Diagnostics.Stopwatch swj = new System.Diagnostics.Stopwatch();
            swj.Reset();
            swj.Start();

            byte svcmd = 0x40;
            if (svtitle.ToUpper().IndexOf("XB") != -1) svcmd = 0x42;
            UInt32 FlashSize = (UInt32)mvars.ucTmp.Length;
            ushort PacketSize = 32768;
            UInt32 Count = FlashSize / PacketSize;
            byte svdenominator = 8;
            if (Count < 20) svdenominator = 1;
            for (i = 0; i < Count; i++)
            {
                string textBox13 = (i * PacketSize).ToString("X8");
                if (svtitle.ToUpper().IndexOf("CB") != -1 || svtitle.ToUpper().IndexOf("FPGA") != -1)
                {
                    if (textBox13 == "00000000")
                    {
                        mvars.lblCmd = "FLASH_WRITEEN";
                        mhFUNCENABLE();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }

                        mvars.lblCmd = "FLASH_WREAR";
                        mp.mhWREAR("00");
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                        {
                            lst_get1.Items.Add(" -> " + svtitle +
                            " QSPI_WREAR 00 fail @ " + String.Format("{0:00}", ((i + 1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", swj.Elapsed.TotalSeconds)) + "sec");
                            lst_get1.Items.Add("");
                            goto Ex;
                        }
                    }
                    if (textBox13 == "01000000")
                    {
                        mvars.lblCmd = "FLASH_WRITEEN";
                        mhFUNCENABLE();
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }

                        mvars.lblCmd = "FLASH_WREAR";
                        mp.mhWREAR("01");
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                        {
                            lst_get1.Items.Add(" -> " + svtitle +
                            " QSPI_WREAR 00 fail @ " + String.Format("{0:00}", ((i + 1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", swj.Elapsed.TotalSeconds)) + "sec");
                            lst_get1.Items.Add("");
                            goto Ex;
                        }
                    }
                }
                if ((i + 1) % svdenominator == 0)
                {
                    lst_get1.Items.Add(" -> " + svtitle +
                        " flash_write " + textBox13 + " @ " + String.Format("{0:00}", ((i+1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", swj.Elapsed.TotalSeconds)) + "sec");
                    lst_get1.TopIndex = lst_get1.Items.Count - 1;
                }
                Application.DoEvents();
                mvars.lblCmd = "FLASH_WRITE_" + i.ToString("0000");
                Application.DoEvents();
                mhFLASHWRITEPAGEQIO(textBox13, PacketSize, svcmd, 300);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    lst_get1.Items.Add(" -> " + svtitle +
                        " flash_write " + i.ToString("0000") + " fail @ " + String.Format("{0:00}", ((i+1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", swj.Elapsed.TotalSeconds)) + "sec");
                    lst_get1.Items.Add("");
                    goto Ex;
                }
                if (mvars.Break)
                {
                    lst_get1.Items.Add(" -> " + svtitle +
                        " flash_write " + i.ToString("0000") + " BREAK @ " + String.Format("{0:00}", ((i+1) * 100 / Count) + "% - ") + Convert.ToString(string.Format("{0:###}", sw.Elapsed.TotalSeconds)) + "sec");
                    lst_get1.Items.Add("");
                    break;
                }
                mp.doDelayms(100);
            }
        Ex:
            byte svflashQ = mvars.flashselQ;
            if (SvErr == "0")
            {
                mvars.flashselQ = 0;
                mvars.lblCmd = "FLASH_TYPE";
                mhFLASHTYPE();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    lst_get1.Items.Add(" -> " + svtitle + " write OK but FLASH_TYPE switch to \"Default\" fail");
                    mvars.flashselQ = svflashQ;
                    SvErr = "-14";
                }
                else
                {
                    mp.doDelayms(500);
                    if (mvars.FormShow[14] == true) uc_Flash.cmbFlashSel.SelectedIndex = 0;
                    else if (mvars.FormShow[3] == true) uc_coding.cmbFlashSel.SelectedIndex = 0;
                }
                if (svtitle.ToUpper().IndexOf("FPGA",0) != -1)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") lst_get1.Items.Add(" -> FPGA HW_RESET");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") lst_get1.Items.Add(" -> FPGA 硬體重置");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") lst_get1.Items.Add(" -> FPGA 硬体重置");
                    lst_get1.TopIndex = lst_get1.Items.Count - 1;
                    mvars.lblCmd = "FPGA_HW_RESET";
                    mp.mhFPGARESET(0x80);
                    if (MultiLanguage.DefaultLanguage == "en-US") lst_get1.Items.Add(" --> FPGA initial and wait for 5sec");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") lst_get1.Items.Add(" --> 等待 FPGA 初始化 5秒");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") lst_get1.Items.Add(" --> 等待 FPGA 初始化时间 5秒");
                    lst_get1.TopIndex = lst_get1.Items.Count - 1;
                    mp.doDelayms(5000);

                    mvars.nvBoardcast = false;
                    Form1.pvindex = 0;
                    mvars.lblCmd = "FPGA_SPI_R";
                    mp.mhFPGASPIREAD();
                    string svverFPGAfinish = "-1,-1";
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                        svverFPGAfinish = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];

                    lst_get1.TopIndex = lst_get1.Items.Count - 1;
                    mvars.flgDelFB = false;
                    if (MultiLanguage.DefaultLanguage == "en-US")
                        lst_get1.Items.Add(" -> " + svtitle +
                        " write OK but Version check : " + svverFPGA.Split('-')[svflashQ - 1] + " >> " + svverFPGAfinish.Split('-')[svflashQ - 1]);
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        lst_get1.Items.Add(" -> " + svtitle +
                        " 寫入完成後版本確認 : " + svverFPGA.Split('-')[svflashQ - 1] + " >> " + svverFPGAfinish.Split('-')[svflashQ - 1]);
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        lst_get1.Items.Add(" -> " + svtitle +
                        " 写入完成后版本确认 : " + svverFPGA.Split('-')[svflashQ - 1] + " >> " + svverFPGAfinish.Split('-')[svflashQ - 1]);
                    mvars.verFPGAm[svflashQ - 1] = svverFPGAfinish.Split('-')[svflashQ - 1];
                    mvars.verFPGAm = svverFPGAfinish.Split('-');
                }
                else
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") lst_get1.Items.Add(" -> " + svtitle + " write OK");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") lst_get1.Items.Add(" -> " + svtitle + " 寫入完成");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") lst_get1.Items.Add(" -> " + svtitle + " 写入完成");
                }
            }
            else lst_get1.Items.Add(" -> " + svtitle + " flash_write fail ErrCode " + SvErr);
           
            if (mvars.demoMode == false && mvars.sp1.IsOpen) mvars.sp1.Close();

            mvars.flgDelFB = false;
            lst_get1.Items.Add("");
            lst_get1.Items.Add("");
            lst_get1.TopIndex = lst_get1.Items.Count - 1;

            mvars.lCounts = mvars.lCount;
            Form1.tslbltarget.Text = mvars.lCounts.ToString();
            mvars.errCode = SvErr;
            mvars.nvBoardcast = svNVBC;
        }





        #endregion cFLASHWRITE ******************************************************************







        #region cFLASHREAD =====================================================================

        public static void cFLASHREAD_pCB_backup(string svtitle, ListBox lst_get1)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mp.markreset(9999, false);
            mvars.flgDelFB = true;
            string SvErr = "0";

            uint FlashSize = (uint)mp.FlashRd_Arr.Length;
            int svQSPIsize = mp.FlashRd_Arr.Length / 1048576;

            mvars.lblCmd = "FLASH_TYPE";
            mhFLASHTYPE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { SvErr = "-1"; goto Ex; }

            //if (svRdstring == "QSPI_Rd4Bit") svSize = 0x10; else svSize = 0x0F;
            uint PacketSize = 2048;
            if (mvars.svnova == false) { PacketSize = 32768; }
            uint Count = (uint)FlashSize / PacketSize;

            sw.Reset();
            sw.Start();
            for (UInt32 i = 0; i < Count; i++)
            {
                string txt36 = (i * PacketSize).ToString("X8");
                if (txt36 == "00000000" && svQSPIsize == 32)
                {
                    mvars.lblCmd = "FLASH_WRITEEN";
                    mhFUNCENABLE();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-2"; goto Ex; }

                    mvars.lblCmd = "FLASH_WREAR";
                    mhWREAR("00");
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-3"; goto Ex; }
                }
                if (txt36 == "01000000" && svQSPIsize == 32)
                {
                    mvars.lblCmd = "FLASH_WRITEEN";
                    mhFUNCENABLE();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-4"; goto Ex; }

                    mvars.lblCmd = "FLASH_WREAR";
                    mhWREAR("01");
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { SvErr = "-5"; goto Ex; }
                }

                lst_get1.Items.Add(" -> " + svtitle + " Read counter：" + (i + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(i + 1) * 100 / Count) + "%");
                lst_get1.TopIndex = lst_get1.Items.Count - 1;

                mvars.lblCmd = "FLASH_READ_" + i.ToString("0000");
                mhFLASHREAD(txt36, PacketSize, "QSPI_Rd4Bit");
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    SvErr = "-6." + i;
                    string path = " -> Fail section file，" + mvars.strStartUpPath + @"\Parameter\" + svtitle + "_Readfailsection_" + i * PacketSize + ".bin";
                    SaveBinFile(path, FlashRd_Arr);
                    goto Ex;
                }
                else
                {
                    for (UInt32 j = 0; j < PacketSize; j++) FlashRd_Arr[i * PacketSize + j] = mvars.gFlashRdPacketArr[j];
                }
            }
        Ex:
            mvars.flgDelFB = false;
            if (SvErr == "0")
            {
                //File.WriteAllBytes(mvars.strStartUpPath + @"\Parameter\" + svtitle  + "FlashRead.bin", FlashRd_Arr);
                //string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\MiniLedLog\\Primary\\Flash\\Read.bin";
                string path = mvars.strStartUpPath + @"\Parameter\" + svtitle + "_Read.bin";
                SaveBinFile(path, FlashRd_Arr);
                lst_get1.Items.Add(" -> " + mvars.strStartUpPath + @"\Parameter\" + svtitle + "_Read.bin");
                if (MultiLanguage.DefaultLanguage == "en-US") lst_get1.Items.Add(" -> " + svtitle + " write OK，" + Convert.ToString(string.Format("{0:###0}", sw.Elapsed.TotalSeconds)) + "sec");
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") lst_get1.Items.Add(" -> " + svtitle + " 寫入完成，" + Convert.ToString(string.Format("{0:###0}", sw.Elapsed.TotalSeconds)) + "秒");
                else if (MultiLanguage.DefaultLanguage == "zh-CN") lst_get1.Items.Add(" -> " + svtitle + " 写入完成，" + Convert.ToString(string.Format("{0:###0}", sw.Elapsed.TotalSeconds)) + "秒");
            }
            else
            {
                if (MultiLanguage.DefaultLanguage == "en-US") lst_get1.Items.Add(" -> " + svtitle + " write Fail，Err code" + SvErr);
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") lst_get1.Items.Add(" -> " + svtitle + " 寫入失敗，異常代碼" + SvErr);
                else if (MultiLanguage.DefaultLanguage == "zh-CN") lst_get1.Items.Add(" -> " + svtitle + " 写入失败，异常代码" + SvErr);
            }
            lst_get1.Items.Add("");
            lst_get1.Items.Add("");
            lst_get1.TopIndex = lst_get1.Items.Count - 1;

            mvars.lCounts = mvars.lCount;
            Form1.tslbltarget.Text = mvars.lCounts.ToString();
            mvars.errCode = SvErr;
        }

        public static void cFLASHREAD_pCB(string svtitle, ListBox lst_get1)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            mp.markreset(9999, false);
            mvars.flgDelFB = true;
            string sverr = "0";
            string svJedecId = "FFFFFF";

            mvars.lblCmd = "FLASH_TYPE";
            mhFLASHTYPE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-1"; goto Ex; }

            mvars.lblCmd = "READ_JEDECID";
            mp.mSPI_READJEDECID();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-1.1"; goto Ex; }
            else svJedecId = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];

            if (svJedecId.Substring(0, 4) == "FFFF") { sverr = "-1.2"; goto Ex; }

            bool svQSPI32MB = false;
            uint FlashSize = 8 * 1024 * 1024;
            if (svJedecId == "C22017") FlashSize = 8 * 1024 * 1024;
            else if (svJedecId == "C22018") FlashSize = 16 * 1024 * 1024;
            else if (svJedecId == "C22019") { FlashSize = 32 * 1024 * 1024; svQSPI32MB = true; }

            uint PacketSize = 32768;
            if (mvars.svnova) PacketSize = 2048;

            ushort Count = (ushort)(FlashSize / PacketSize);
            Array.Resize(ref FlashRd_Arr, (int)FlashSize);


            sw.Reset();
            sw.Start();
            for (UInt32 i = 0; i < Count; i++)
            {
                string txt36 = (i * PacketSize).ToString("X8");
                if (txt36 == "00000000" && svQSPI32MB)
                {
                    mvars.lblCmd = "FLASH_WRITEEN";
                    mhFUNCENABLE();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-2"; goto Ex; }

                    mvars.lblCmd = "FLASH_WREAR";
                    mhWREAR("00");
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-3"; goto Ex; }
                }
                if (txt36 == "01000000" && svQSPI32MB)
                {
                    mvars.lblCmd = "FLASH_WRITEEN";
                    mhFUNCENABLE();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-4"; goto Ex; }

                    mvars.lblCmd = "FLASH_WREAR";
                    mhWREAR("01");
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) { sverr = "-5"; goto Ex; }
                }

                lst_get1.Items.Add(" -> " + svtitle + " Read counter：" + (i + 1) + " / " + Count + " , " + string.Format("{0:#.00}", (decimal)(i + 1) * 100 / Count) + "%");
                lst_get1.TopIndex = lst_get1.Items.Count - 1;

                mvars.lblCmd = "FLASH_READ_" + i.ToString("0000");

                //重點必須要注意XB要切換
                if (svtitle.ToUpper().IndexOf("XB_",0) != -1)
                    mhFLASHREAD(txt36, PacketSize, "QSPI_Rd1Bit");
                else
                    mhFLASHREAD(txt36, PacketSize, "QSPI_Rd4Bit");

                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                {
                    sverr = "-6." + i;
                    //string path = " -> Fail section file，" + mvars.strStartUpPath + @"\Parameter\" + svtitle + "_Readfailsection_" + i * PacketSize + ".bin";
                    //SaveBinFile(path, FlashRd_Arr);
                    goto Ex;
                }
                else
                {
                    for (UInt32 j = 0; j < PacketSize; j++) FlashRd_Arr[i * PacketSize + j] = mvars.gFlashRdPacketArr[j];
                }
            }
        Ex:
            mvars.flgDelFB = false;
            if (sverr == "0")
            {
                //File.WriteAllBytes(mvars.strStartUpPath + @"\Parameter\" + svtitle  + "FlashRead.bin", FlashRd_Arr);
                //string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\MiniLedLog\\Primary\\Flash\\Read.bin";
                //string path = mvars.strStartUpPath + @"\Parameter\" + svtitle + "_Read.bin";
                //SaveBinFile(path, FlashRd_Arr);
                lst_get1.Items.Add(" -> " + mvars.strStartUpPath + @"\Parameter\" + svtitle + "_Read.bin");
                if (MultiLanguage.DefaultLanguage == "en-US") lst_get1.Items.Add(" -> " + svtitle + " Read OK，" + Convert.ToString(string.Format("{0:###0}", sw.Elapsed.TotalSeconds)) + " sec");
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") lst_get1.Items.Add(" -> " + svtitle + " 回讀完成，" + Convert.ToString(string.Format("{0:###0}", sw.Elapsed.TotalSeconds)) + " 秒");
                else if (MultiLanguage.DefaultLanguage == "zh-CN") lst_get1.Items.Add(" -> " + svtitle + " 回读完成，" + Convert.ToString(string.Format("{0:###0}", sw.Elapsed.TotalSeconds)) + " 秒");
            }
            else
            {
                if (MultiLanguage.DefaultLanguage == "en-US") lst_get1.Items.Add(" -> " + svtitle + " Read Fail，Err code" + sverr);
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") lst_get1.Items.Add(" -> " + svtitle + " 回讀失敗，異常代碼" + sverr);
                else if (MultiLanguage.DefaultLanguage == "zh-CN") lst_get1.Items.Add(" -> " + svtitle + " 回读失败，异常代码" + sverr);
            }
            lst_get1.Items.Add("");
            lst_get1.Items.Add("");
            lst_get1.TopIndex = lst_get1.Items.Count - 1;

            mvars.lCounts = mvars.lCount;
            Form1.tslbltarget.Text = mvars.lCounts.ToString();
            mvars.errCode = sverr;
        }



        #endregion cFLASHREAD =====================================================================






        public static void mhFLASHREAD(string txt36, UInt32 svReadSize) //CB 0x33 ，XB 0x38 最新混合體
        {
            UInt16 ReadSize = (UInt16)svReadSize; //1024,2048,4096,8192,16384 (需與cFLASHWRITE_C12ACB 設定值相同)
            byte[] bytes = BitConverter.GetBytes(ReadSize);
            byte[] flash_addr_arr = StringToByteArray(txt36);
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 25; mvars.NumBytesToRead = ReadSize + 28; 
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x10); 
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.ReadDataBuffer, (int)svReadSize + 513); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            if (mvars.c12aflashitem == 0)
            {
                if (mvars.svnova) Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x10);
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x33;                   //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x10;                   //Size
                mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];      //QSPI address
                mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];      //QSPI address
                mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];      //QSPI address
                mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];      //QSPI address.
                mvars.RS485_WriteDataBuffer[9 + svns] = 0x08;                   //dummy cycle
                mvars.RS485_WriteDataBuffer[10 + svns] = bytes[1];              //Read Size
                mvars.RS485_WriteDataBuffer[11 + svns] = bytes[0];              //Read Size
            }
            else if (mvars.c12aflashitem == 1)
            {
                if (mvars.svnova) Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x38;                   //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                   //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                   //Size
                mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];      //QSPI address
                mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];      //QSPI address
                mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];      //QSPI address
                mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];      //QSPI address.
                mvars.RS485_WriteDataBuffer[9 + svns] = bytes[1];               //Read Size
                mvars.RS485_WriteDataBuffer[10 + svns] = bytes[0];              //Read Size
            }
            mp.funSendMessageTo();
        }

        public static void mhFLASHREAD(string textBox36, UInt32 svReadSize, string svRdstring)          //CB 0x33 ，XB 0x38 
        {
            /// 1024,2048,4096,8192,16384 (需與cFLASHWRITE_Primary 設定值相同)
            byte svns = 2;                                                          // 預設NovaStar使用,因為反應較慢儘量減少程序
            byte[] flash_addr_arr = StringToByteArray(textBox36);
            byte[] bytes = BitConverter.GetBytes(svReadSize);
            int svSize;
            if (svRdstring == "QSPI_Rd4Bit") svSize = 0x10; else svSize = 0x0F;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 300; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, (int)(svns + svSize + svReadSize));
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 513 + 32768);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];   //QSPI address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];   //QSPI address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];   //QSPI address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];   //QSPI address

            if (svRdstring == "QSPI_Rd4Bit")
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x33;           //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x10;           //Size
                mvars.RS485_WriteDataBuffer[9 + svns] = 0x08;           //dummy cycle              
                mvars.RS485_WriteDataBuffer[10 + svns] = bytes[1];      //Read Size
                mvars.RS485_WriteDataBuffer[11 + svns] = bytes[0];      //Read Size
            }
            else
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x35;           //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;           //Size
                mvars.RS485_WriteDataBuffer[9 + svns] = bytes[1];       //Read Size
                mvars.RS485_WriteDataBuffer[10 + svns] = bytes[0];      //Read Size
            }


            mp.funSendMessageTo();
        }


        public static void mhWREAR(string txt218)        //0x32 最新混合體
        {
            #region 2022版公用程序
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0D);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 513); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x32;                       /// Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                       /// Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D;                       /// Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0xC5;                       /// Write EAR
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                       /// Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;                       /// Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = Convert.ToByte(txt218, 16); /// Data
            mp.funSendMessageTo();
        }




        




        public static void mhFLASHWRITEPAGEQIO(string textBox13, UInt16 svWRSize, byte svEraWrChk32K, ushort svdelayms)     //CB 0x42 ，XB 0x39 新版正確 
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            UInt16 WriteSize = svWRSize; //1024,2048,4096,8192,16384 (需與來源 設定值相同)
            byte[] WrArr = BitConverter.GetBytes(WriteSize);
            byte[] flash_addr_arr = StringToByteArray(textBox13);
            UInt16 PacketSize = (UInt16)(WriteSize + 0x0F);
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            if (mvars.svnova)
            {
                mvars.Delaymillisec = svdelayms; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513 + 32768);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 513); 
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = svEraWrChk32K;          //Cmd 
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];               //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];               //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];      //QSPI address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];      //QSPI address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];      //QSPI address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];      //QSPI address.
            mvars.RS485_WriteDataBuffer[9 + svns] = WrArr[1];               //Write Size
            mvars.RS485_WriteDataBuffer[10 + svns] = WrArr[0];              //Write Size
            UInt32 FlashAddr = Convert.ToUInt32(textBox13, 16);
            for (UInt16 i = 0; i < WriteSize; i++) { mvars.RS485_WriteDataBuffer[11 + svns + i] = mvars.ucTmp[FlashAddr + i]; } //Data
            mp.funSendMessageTo();
        }

        



        public static void mhFLASHWRITEPAGEQIO(string txt36, UInt16 svReadSize)    //CB 0x3F ，XB 0x39 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            UInt16 WriteSize = svReadSize; //1024,2048,4096,8192,16384 (需與cFLASHWRITE_C12ACB 設定值相同)
            byte[] WrArr = BitConverter.GetBytes(WriteSize);
            byte[] flash_addr_arr = StringToByteArray(txt36);
            UInt16 PacketSize = (UInt16)(WriteSize + 0x0F);
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 30; mvars.NumBytesToRead = 28;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513 + 32768) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513 + 32768); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513 + 32768); }
                //PacketSize=2048+0x0F=2063,但是[11+svns+2047+4]=2047, 所以Size必須再加1
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            if (mvars.c12aflashitem == 0)   //CB
            {
                //mvars.RS485_WriteDataBuffer[2 + svns] = 0x34;              //Cmd Write & Read
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x3F;               //Cmd Write Check
            }
            else if (mvars.c12aflashitem == 1)  //XB
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x39;               //Cmd 
            }
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];              //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];              //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];     //QSPI address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];     //QSPI address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];     //QSPI address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];     //QSPI address.
            mvars.RS485_WriteDataBuffer[9 + svns] = WrArr[1];              //Write Size
            mvars.RS485_WriteDataBuffer[10 + svns] = WrArr[0];             //Write Size
            UInt32 FlashAddr = Convert.ToUInt32(txt36, 16);
            for (UInt16 i = 0; i < WriteSize; i++) { mvars.RS485_WriteDataBuffer[11 + svns + i] = mvars.ucTmp[FlashAddr + i]; } //Data
            mp.funSendMessageTo();
        }



        public static void mhFPGARESET(byte svcmd) //c12a New Model 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x09);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.ReadDataBuffer, 65);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = svcmd;          //Cmd Soft(0xFF)  Hard(0x80)
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x09;           //Size    
            mp.funSendMessageTo();
        }
        public static void mhSWRESET(byte svcmd) //c12a New Model 最新混合體
        {
            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                if (mvars.RS485_WriteDataBuffer != null) Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x09);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                if (mvars.RS485_WriteDataBuffer.Length == 513) { Array.Clear(mvars.RS485_WriteDataBuffer, 0, 513); }
                else { Array.Resize(ref mvars.RS485_WriteDataBuffer, 513); }
                if (mvars.ReadDataBuffer.Length == 65) { Array.Clear(mvars.ReadDataBuffer, 0, 65); }
                else { Array.Resize(ref mvars.ReadDataBuffer, 65); }
            }
            mvars.RS485_WriteDataBuffer[2 + svns] = svcmd;          //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x09;           //Size    最後一筆+1, 例.[13]則Size=0x0E 0~13=14
            mp.funSendMessageTo();
        }

        
        public static void mhPWICINFO(string svtxt259) //c12a New Model 最新混合體
        {
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.ReadDataBuffer, 513);
                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);

            //由 cmb_hPB選擇後切換 SercomCmdWr 與 SercomCmdWrRd
            if (mvars.deviceID == "0200")
            {
                if (mvars.iPBaddr <= 4) mvars.RS485_WriteDataBuffer[2 + svns] = 0x61;
                else if (mvars.iPBaddr > 4 && mvars.iPBaddr <= 8) mvars.RS485_WriteDataBuffer[2 + svns] = 0x69;
                else if (mvars.iPBaddr > 8 && mvars.iPBaddr <= 12) mvars.RS485_WriteDataBuffer[2 + svns] = 0x51;
                else if (mvars.iPBaddr > 12 && mvars.iPBaddr <= 16) mvars.RS485_WriteDataBuffer[2 + svns] = 0x59;
            }
            else if (mvars.deviceID == "0400")
            {
                if (mvars.SercomCmdWrRd <= 0x57) { mvars.RS485_WriteDataBuffer[2 + svns] = 0x51; }
                else if (mvars.SercomCmdWrRd > 0x57 && mvars.SercomCmdWrRd <= 0x5F) { mvars.RS485_WriteDataBuffer[2 + svns] = 0x59; }
            }
            //mvars.RS485_WriteDataBuffer[2 + svns] = mvars.SercomCmdWrRd;                        //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                                       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;                                       //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(Convert.ToByte(svtxt259, 16) / 2);   //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                                       //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;                                       //Write Size
            mvars.RS485_WriteDataBuffer[8 + svns] = 0xF0;                                       //Register
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x00;                                       //Read Size
            mvars.RS485_WriteDataBuffer[10 + svns] = 0x06;                                      //Read Size
            mp.funSendMessageTo();
        }



        
        public static float FormulaLinear(float SvWLx, float SvWL1, float SvInt1, float SvWL2, float SvInt2)
        {
            float SvFa;
            float SvFb;

            if (SvWL1 - SvWL2 == 0) { SvFa = 0; SvFb = SvInt1; }
            else
            {
                SvFa = (SvInt1 - SvInt2) / (SvWL1 - SvWL2);
                SvFb = SvInt1 - SvFa * SvWL1;
            }
            return SvFa * SvWLx + SvFb;
        }




        #region cm603 
        public static void Gamma_603()
        {
            for (byte svi = 0; svi <= 2; svi++)
            {
                if (svi == 0) { mvars.cm603WRaddr = 212; }
                else if (svi == 1) { mvars.cm603WRaddr = 214; }
                else if (svi == 2) { mvars.cm603WRaddr = 218; }
                if (mvars.deviceID.Substring(0, 2) == "03")
                {
                    mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF;
                    if (svi == 0) { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1); }
                    else if (svi == 1) { mvars.cm603WRaddr = 214; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                    else if (svi == 2) { mvars.cm603WRaddr = 212; mvars.SercomCmdWr = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 2); mvars.SercomCmdWrRd = (byte)(0x50 + ((mvars.iPBaddr - 1) % 2) * 4 + 1 + 2); }
                    Form1.pvindex = 0x3E;
                    mvars.cm603df[svi, Form1.pvindex * 2] = "00";
                    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = "00";
                    mvars.cm603dfB[svi, Form1.pvindex * 2] = 0;
                    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = 0;
                    Form1.cmbCM603.SelectedIndex = svi;
                    mvars.lblCmd = "GammaSet_ALL";
                    mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, (byte)Form1.pvindex, 2); //Address

                }
                else if (mvars.deviceID.Substring(0, 2) == "05" || 
                    mvars.deviceID.Substring(0, 2) == "06")
                {
                    for (Form1.pvindex = 0x02 + 30 * mvars.dualduty; Form1.pvindex <= 0x0D + 30 * mvars.dualduty; Form1.pvindex++)
                    {
                        /*
                        int svbinI = mUser.HexToDec((string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16)].Value + (string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16 + 1)].Value);
                        string svbinS = mUser.DecToBin(svbinI, 16);
                        lbl_BinVol.Text = ((0.01953 * mUser.BinToDec(svbinS.Substring(6, 10)) * Convert.ToInt32(lbl_BinVal.Text)) / 1024).ToString("##0.0###");
                        */
                        int sH = mvars.pGMA[mvars.dualduty].Data.GetLength(1) - (Form1.pvindex - (0x02 + 30 * mvars.dualduty)) * 2 - 2;     /// 26 - (0x02-0x02)*2-2 = 24
                        int sL = sH + 1;                                                                                                    /// 24+1 = 25
                        int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                        UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1);
                    mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, 0x02, 124); //GMA1BK0  GMA12BK0
                }
                else if (mvars.deviceID.Substring(0, 2) == "10")
                {
                    for (byte svdd = 0; svdd <= 1; svdd++)  //CarpStreamer不管bankselect所以2組bank都寫一樣的內容
                    {
                        for (Form1.pvindex = 0x12 + 30 * svdd; Form1.pvindex <= 0x12 + 30 * svdd; Form1.pvindex++)
                        {
                            int sH = gmax * 2;      /// 12*2 = 24
                            int sL = sH + 1;        /// 24+1 = 25
                            int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                            UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                            mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                            mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                            mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                            mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);

                            byte dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                            byte dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                            mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                        }
                    }
                }
                else
                {

                    for (Form1.pvindex = 0x04; Form1.pvindex <= 0x0C; Form1.pvindex++)
                    {
                        /*
                        int svbinI = mUser.HexToDec((string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16)].Value + (string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16 + 1)].Value);
                            string svbinS = mUser.DecToBin(svbinI, 16);
                            lbl_BinVol.Text = ((0.01953 * mUser.BinToDec(svbinS.Substring(6, 10)) * Convert.ToInt32(lbl_BinVal.Text)) / 1024).ToString("##0.0###");
                        */

                        int sH = mvars.pGMA[mvars.dualduty].Data.GetLength(1) - (Form1.pvindex - 0x03) * 2;
                        int sL = sH + 1;
                        int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                        UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                    mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1);
                    mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, 0x04, 18); //GMA1BK0  GMA12BK0
                }
            }
        }
        public static void Gamma_603_Drun()
        {
            for (byte svi = 0; svi <= 2; svi++)
            {
                if (svi == 0) { mvars.cm603WRaddr = 0xD4; }         //212
                else if (svi == 1) { mvars.cm603WRaddr = 0xD6; }    //214
                else if (svi == 2) { mvars.cm603WRaddr = 0xDA; }    //218
                else if (svi == 3) { mvars.cm603WRaddr = 0xD8; }    //216

                mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_ALL";
                mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, 0x02, 124); //GMA1BK0  GMA12BK0
            }
        }
        public static bool Unlock_603(byte svICs)
        {
            //      AddrCtrl HardWareA0   Wr    Rd
            //0:R     0         0         D4    D5
            //1:G     0         1         D6    D7  
            //2:B     1         1         DA    DB
            //3:M     1         0         D8    D9
            bool sverr = false;
            for (byte svi = 0; svi < svICs; svi++)
            {
                if (mvars.deviceID.Substring(0, 2) == "02" ||
                    mvars.deviceID.Substring(0, 2) == "04" ||
                    mvars.deviceID.Substring(0, 2) == "05" ||
                    mvars.deviceID.Substring(0, 2) == "06" ||
                    mvars.deviceID.Substring(0, 2) == "10")
                {
                    if (svi == 0) { mvars.cm603WRaddr = 0xD4; }         //212
                    else if (svi == 1) { mvars.cm603WRaddr = 0xD6; }    //214
                    else if (svi == 2) { mvars.cm603WRaddr = 0xDA; }    //218
                    else if (svi == 3) { mvars.cm603WRaddr = 0xD8; }    //216
                }

                Form1.cmbCM603.SelectedIndex = svi;
                Form1.cmbCM603.Text = Form1.cmbCM603.Items[svi].ToString();

                mvars.lblCmd = "CM603_UNLOCKWP1";
                mp.mhcm603UnlockWP1(mvars.cm603WRaddr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                mvars.lblCmd = "CM603_UNLOCKWP2";
                mp.mhcm603UnlockWP2(mvars.cm603WRaddr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                //RESET StatusReg +1 
                UInt32 u32Val = mp.CRC_Cal(640, (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                byte dataH = (byte)((u32Val + 640) / 256);
                byte dataL = (byte)((u32Val + 640) % 256);
                Form1.pvindex = 0x3F;
                mvars.lblCmd = "CM603_WRITE_0x" + mp.DecToHex(0x3F, 2);
                mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, 0x3F, 2, dataH, dataL);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                //LOCK +2
                mvars.lblCmd = "CM603_LOCKWP1";
                mp.mhcm603lockWP1(mvars.cm603WRaddr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                mvars.lblCmd = "CM603_LOCKWP2";
                mp.mhcm603lockWP2(mvars.cm603WRaddr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                //UNLOCK +2
                mvars.lblCmd = "CM603_UNLOCKWP1";
                mp.mhcm603UnlockWP1(mvars.cm603WRaddr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                mvars.lblCmd = "CM603_UNLOCKWP2";
                mp.mhcm603UnlockWP2(mvars.cm603WRaddr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
            }
            return sverr;
        }      
        public static bool Vref_603(byte svICs)
        {
            //      AddrCtrl HardWareA0   Wr    Rd
            //0:R     0         0         D4    D5
            //1:G     0         1         D6    D7  
            //2:B     1         1         DA    DB
            //3:M     1         0         D8    D9
            bool sverr = false;
            Form1.pvindex = 0x1F;
            for (byte svi = 0; svi < svICs; svi++)
            {
                uint svcm603code = Convert.ToUInt16(Math.Round(mvars.cm603Vref[svi] / 0.01953, 0));

                if (mvars.deviceID.Substring(0, 2) == "02" || 
                    mvars.deviceID.Substring(0, 2) == "04" || 
                    mvars.deviceID.Substring(0, 2) == "05" ||
                    mvars.deviceID.Substring(0, 2) == "06" ||
                    mvars.deviceID.Substring(0, 2) == "10")
                {
                    if (svi == 0) { mvars.cm603WRaddr = 0xD4; }         //212
                    else if (svi == 1) { mvars.cm603WRaddr = 0xD6; }    //214
                    else if (svi == 2) { mvars.cm603WRaddr = 0xDA; }    //218
                    else if (svi == 3) { mvars.cm603WRaddr = 0xD8; }    //216
                }
                UInt32 ulCRC = mp.CRC_Cal(svcm603code, (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                string svbinS = "00" + (mp.DecToBin((int)ulCRC, 16).Substring(2, 4)) + mp.DecToBin((int)svcm603code, 10);
                mvars.cm603df[svi, Form1.pvindex * 2] = mp.BinToHex(svbinS.Substring(0, 8), 2);
                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.BinToHex(svbinS.Substring(8, 8), 2);
                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);

                if (svi == 3) mvars.lblCmd = "GammaSet(M) Vref" + mvars.cm603Vref[svi].ToString("##0.0###");
                else mvars.lblCmd = "GammaSet(" + mvars.msrColor[svi + 2].Substring(0, 1) + ") Vref" + mvars.cm603Vref[svi].ToString("##0.0###");
                byte dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                byte dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);

                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
            }
            return sverr;
        }
        public static bool TFTVREF_603BK1VCOM(byte svICs)
        {
            //      AddrCtrl HardWareA0   Wr    Rd
            //0:R     0         0         D4    D5
            //1:G     0         1         D6    D7  
            //2:B     1         1         DA    DB
            //3:M     1         0         D8    D9
            bool sverr = false;
            for (byte svi = 0; svi < svICs; svi++)
            {
                if (mvars.deviceID.Substring(0, 2) == "02" ||
                    mvars.deviceID.Substring(0, 2) == "04" ||
                    mvars.deviceID.Substring(0, 2) == "05" ||
                    mvars.deviceID.Substring(0, 2) == "06")
                {
                    if (svi == 0) { mvars.cm603WRaddr = 0xD4; }         //212
                    else if (svi == 1) { mvars.cm603WRaddr = 0xD6; }    //214
                    else if (svi == 2) { mvars.cm603WRaddr = 0xDA; }    //218
                    else if (svi == 3) { mvars.cm603WRaddr = 0xD8; }    //216
                }
                if (mvars.dualduty == 0) Form1.pvindex = 0x18; else Form1.pvindex = 0x31;     // VCOM1MIN
                int svgmacode = (Int16)(0 * 1024 / mvars.cm603Vref[svi]);
                UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_Reg" + Form1.pvindex + "_VCOM1MIN";
                byte dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                byte dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }

                if (mvars.dualduty == 0) Form1.pvindex = 0x19; else Form1.pvindex = 0x32;     // VCOM1MAX
                svgmacode = 1023;
                ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_Reg" + Form1.pvindex + "_VCOM1MAX" + Math.Round(mvars.cm603Vref[svi], 0);
                dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }

                if (mvars.dualduty == 0) Form1.pvindex = 0x12; else Form1.pvindex = 0x30;     // VCOM1 = TFTVREF
                svgmacode = (Int16)(mvars.UUT.VREF * 1024 / mvars.cm603Vref[svi]);
                ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_Reg" + Form1.pvindex + "_TFTVREF" + mvars.UUT.VREF;
                dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
            }
            return sverr;
        }

        public static bool TFTVREF_603RVGMA1(byte svICs)
        {
            //      AddrCtrl HardWareA0   Wr    Rd
            //0:R     0         0         D4    D5
            //1:G     0         1         D6    D7  
            //2:B     1         1         DA    DB
            //3:M     1         0         D8    D9
            int svgmacode;
            UInt32 ulCRC;
            byte dataH;
            byte dataL;
            bool sverr = false;
            for (byte svi = 0; svi < svICs; svi++)
            {
                if (mvars.deviceID.Substring(0, 2) == "02" ||
                    mvars.deviceID.Substring(0, 2) == "04" ||
                    mvars.deviceID.Substring(0, 2) == "05" ||
                    mvars.deviceID.Substring(0, 2) == "06" ||
                    mvars.deviceID.Substring(0, 2) == "10")
                {
                    if (svi == 0) { mvars.cm603WRaddr = 0xD4; }         //212
                    else if (svi == 1) { mvars.cm603WRaddr = 0xD6; }    //214
                    else if (svi == 2) { mvars.cm603WRaddr = 0xDA; }    //218
                    else if (svi == 3) { mvars.cm603WRaddr = 0xD8; }    //216
                }
                // VCOM1MIN
                for (byte svdd = 0; svdd <= 1; svdd++)  //CarpStreamer不管bankselect所以2組bank都寫一樣的內容
                {
                    Form1.pvindex = 0x18 + 25 * svdd;
                    svgmacode = (Int16)(0 * 1024 / mvars.cm603Vref[svi]);
                    ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_Reg" + Form1.pvindex + "_VCOM1MIN";
                    dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }

                    // VCOM1MAX
                    Form1.pvindex = 0x19 + 25 * svdd;
                    svgmacode = 1023;
                    ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_Reg" + Form1.pvindex + "_VCOM1MAX" + Math.Round(mvars.cm603Vref[svi], 0);
                    dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }

                    if (svi == 0)
                    {
                        // Red VGMA1
                        Form1.pvindex = 0x02 + 30 * svdd;
                        svgmacode = (Int16)(mvars.UUT.VREF * 1024 / mvars.cm603Vref[svi]);
                        ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                        mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_Reg" + Form1.pvindex + "_TFTVREF" + mvars.UUT.VREF;
                        dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                        mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                    }
                }  
            }
            return sverr;
        }


        public static bool Gamma_603_G0(float[] svV, byte svICs)
        {
            bool sverr = false;
            if (mvars.deviceNameSub == "A" || mvars.deviceNameSub == "B" || mvars.deviceNameSub == "B(4t)")
            {
                if (mvars.deviceNameSub == "B(4t)") Form1.pvindex = 0x0D; else Form1.pvindex = 0x0C;
                for (byte svi = 0; svi < svICs; svi++)
                {
                    if (mvars.deviceID.Substring(0, 2) == "02" || mvars.deviceID.Substring(0, 2) == "04")
                    {
                        if (svi == 0) { mvars.cm603WRaddr = 0xD4; }         //212
                        else if (svi == 1) { mvars.cm603WRaddr = 0xD6; }    //214
                        else if (svi == 2) { mvars.cm603WRaddr = 0xDA; }    //218
                        else if (svi == 3) { mvars.cm603WRaddr = 0xD8; }    //216
                    }

                    mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_G0";
                    mvars.cm603df[svi, Form1.pvindex * 2] = "00";
                    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = "00";
                    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)0x00;
                    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)0x00;
                    byte dataH = 0;
                    byte dataL = 0;
                    mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);

                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                }
            }
            else if (mvars.deviceNameSub == "B(4)" || 
                      mvars.deviceID.Substring(0, 2) == "02" ||
                      mvars.deviceID.Substring(0, 2) == "04" ||
                      mvars.deviceID.Substring(0, 2) == "05" ||
                      mvars.deviceID.Substring(0, 2) == "06")
            {
                mvars.cm603WRaddr = 0xD8;   //216
                for (byte svi = 0; svi <= 2; svi++)
                {
                    Form1.pvindex = svi + (2 + mvars.dualduty * 30);    /// M02h/M03h/M04h    M20h/M21h/22h
                    mvars.lblCmd = "GammaSet_G0_" + "M" + mp.DecToHex(Form1.pvindex, 2) + "h";
                    byte sH = 0;
                    byte sL = 1;
                    string svs = mp.DecToHex((int)(svV[svi] * 1024 / 6), 4);
                    mvars.pGMA[mvars.dualduty].Data[svi, sH] = svs.Substring(0, 2);
                    mvars.pGMA[mvars.dualduty].Data[svi, sL] = svs.Substring(2, 2);
                    int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]); //[0,0][0,1]
                    UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    mvars.cm603df[3, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                    mvars.cm603df[3, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                    mvars.cm603dfB[3, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[3, Form1.pvindex * 2]);
                    mvars.cm603dfB[3, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[3, Form1.pvindex * 2 + 1]);
                    mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, mvars.cm603dfB[3, Form1.pvindex * 2], mvars.cm603dfB[3, Form1.pvindex * 2 + 1]);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }

                    uc_atg.dgvatg.Rows[8 + svi * 2].Cells[0].Value = svs.Substring(0, 2);
                    uc_atg.dgvatg.Rows[9 + svi * 2].Cells[0].Value = svs.Substring(2, 2);
                }
            }
            else if (mvars.deviceID.Substring(0, 2) == "10")
            {
                mvars.cm603WRaddr = 0xDA;   //218 藍色的GMA0
                string svs = mp.DecToHex((int)(svV[0] * 1024 / 6), 4);
                for (byte svdd = 0; svdd <= 1; svdd++)  //CarpStreamer不管bankselect所以2組bank都寫一樣的內容
                {
                    Form1.pvindex = 0x02 + 30 * svdd;
                    mvars.lblCmd = "GammaSet_G0_" + "B" + mp.DecToHex(Form1.pvindex, 2) + "h";
                    byte sH = 0;
                    byte sL = 1;
                    
                    mvars.pGMA[mvars.dualduty].Data[0, sH] = svs.Substring(0, 2);
                    mvars.pGMA[mvars.dualduty].Data[0, sL] = svs.Substring(2, 2);
                    mvars.pGMA[mvars.dualduty].Data[1, sH] = svs.Substring(0, 2);
                    mvars.pGMA[mvars.dualduty].Data[1, sL] = svs.Substring(2, 2);
                    mvars.pGMA[mvars.dualduty].Data[2, sH] = svs.Substring(0, 2);
                    mvars.pGMA[mvars.dualduty].Data[2, sL] = svs.Substring(2, 2);

                    int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[0, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[0, sL]); //[0,0][0,1]
                    UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    mvars.cm603df[2, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                    mvars.cm603df[2, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                    mvars.cm603dfB[2, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[2, Form1.pvindex * 2]);
                    mvars.cm603dfB[2, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[2, Form1.pvindex * 2 + 1]);
                    mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, mvars.cm603dfB[2, Form1.pvindex * 2], mvars.cm603dfB[2, Form1.pvindex * 2 + 1]);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = true; }
                }

                uc_atg.dgvatg.Rows[8 + 0 * 2].Cells[0].Value = svs.Substring(0, 2);
                uc_atg.dgvatg.Rows[9 + 0 * 2].Cells[0].Value = svs.Substring(2, 2);
                uc_atg.dgvatg.Rows[8 + 1 * 2].Cells[0].Value = svs.Substring(0, 2);
                uc_atg.dgvatg.Rows[9 + 1 * 2].Cells[0].Value = svs.Substring(2, 2);
                uc_atg.dgvatg.Rows[8 + 2 * 2].Cells[0].Value = svs.Substring(0, 2);
                uc_atg.dgvatg.Rows[9 + 2 * 2].Cells[0].Value = svs.Substring(2, 2);
            }
            return sverr;
        }
        public static void Gamma_603(byte svICs)
        {
            //      AddrCtrl HardWareA0   Wr    Rd
            //0:R     0         0         D4    D5
            //1:G     0         1         D6    D7  
            //2:B     1         1         DA    DB
            //3:M     1         0         D8    D9
            for (byte svi = 0; svi < svICs; svi++)
            {
                if (mvars.deviceID.Substring(0, 2) == "02" || 
                    mvars.deviceID.Substring(0, 2) == "04" || 
                    mvars.deviceID.Substring(0, 2) == "05" ||
                    mvars.deviceID.Substring(0, 2) == "06" ||
                    mvars.deviceID.Substring(0, 2) == "10")
                {
                    if (svi == 0) { mvars.cm603WRaddr = 0xD4; }         //212
                    else if (svi == 1) { mvars.cm603WRaddr = 0xD6; }    //214
                    else if (svi == 2) { mvars.cm603WRaddr = 0xDA; }    //218
                    else if (svi == 3) { mvars.cm603WRaddr = 0xD8; }    //216
                }

                if (mvars.deviceNameSub == "A" || mvars.deviceNameSub == "B")
                {
                    for (Form1.pvindex = 0x04; Form1.pvindex <= 0x0C; Form1.pvindex++)
                    {
                        int sH = mvars.pGMA[mvars.dualduty].Data.GetLength(1) - (Form1.pvindex - 0x04) * 2 - 2;      /// 18 - (0x04-0x04)*2-2 = 16
                        int sL = sH + 1;                                                                /// 16+1 = 17
                        int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                        UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                        mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                        mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                        mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    }
                }
                else if (mvars.deviceNameSub == "B(4)" || 
                         mvars.deviceID.Substring(0, 2) == "05" ||
                         mvars.deviceID.Substring(0, 2) == "06")
                {
                    if (svi == 3)
                    {
                        //0灰階最低電壓
                        for (Form1.pvindex = 0x02 + 30 * mvars.dualduty; Form1.pvindex <= 0x04 + 30 * mvars.dualduty; Form1.pvindex++)
                        {
                            int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - (0x02 + 30 * mvars.dualduty), 0]) * 256 +
                                       mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[Form1.pvindex - (0x02 + 30 * mvars.dualduty), 1]);
                            UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                            mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                            mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                            mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                            mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                        }
                        mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1);
                        mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, (byte)(0x02 + 30 * mvars.dualduty), 6); //GMA1BK0  GMA12BK0
                    }
                    else
                    {
                        for (Form1.pvindex = 0x02 + 30 * mvars.dualduty; Form1.pvindex <= 0x0D + 30 * mvars.dualduty; Form1.pvindex++)
                        {
                            /*
                            int svbinI = mUser.HexToDec((string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16)].Value + (string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16 + 1)].Value);
                            string svbinS = mUser.DecToBin(svbinI, 16);
                            lbl_BinVol.Text = ((0.01953 * mUser.BinToDec(svbinS.Substring(6, 10)) * Convert.ToInt32(lbl_BinVal.Text)) / 1024).ToString("##0.0###");
                            */
                            int sH = gmax * 2 - (Form1.pvindex % 30 - 0x02) * 2;    /// 12*2-(2%30-2)*2 = 24-(2-2)*2 = 24,22,20,18,,,
                            int sL = sH + 1;                                        /// 24+1 =                         25,23,21,19,,,
                            int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                            UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                            mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                            mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                            mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                            mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                        }
                        mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1);
                        mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, (byte)(0x02 + 30 * mvars.dualduty), (byte)(gmax * 2)); //每個bank有12組gamma共24byte
                    }
                }
                else if (mvars.deviceID.Substring(0, 2) == "10")
                {
                    for (byte svdd = 0; svdd <= 1; svdd++)  //CarpStreamer不管bankselect所以2組bank都寫一樣的內容
                    {
                        for (Form1.pvindex = 0x12 + 30 * svdd; Form1.pvindex <= 0x12 + 30 * svdd; Form1.pvindex++)
                        {
                            int sH = gmax * 2;      // 12*2 = 24
                            int sL = sH + 1;        // 24+1 = 25
                            int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                            UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                            mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                            mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                            mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                            mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                            byte dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                            byte dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                            mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                        }
                        for (Form1.pvindex = 0x03 + 30 * svdd; Form1.pvindex <= 0x0D + 30 * svdd; Form1.pvindex++)
                        {
                            int sH = gmax * 2 - (Form1.pvindex % 30 - 0x02) * 2;    // 12*2-(3%30-2)*2 = 24-(3-2)*2 = 22,20,18,,,
                            int sL = sH + 1;                                        // 24+1 =                         23,21,19,,,
                            int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                            UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                            mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                            mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                            mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                            mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                        }
                        mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1);
                        mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, (byte)(0x03 + 30 * svdd), 24);
                    }
                    //mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1);
                    //mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, 0x02, 124); 

                    //for (Form1.pvindex = 0x12 + 30 * mvars.dualduty; Form1.pvindex <= 0x12 + 30 * mvars.dualduty; Form1.pvindex++)
                    //{
                    //    int sH = gmax * 2;      /// 12*2 = 24
                    //    int sL = sH + 1;        /// 24+1 = 25
                    //    int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                    //    UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    //    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                    //    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                    //    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    //    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);

                    //    byte dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    //    byte dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    //    mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                    //}
                    //for (Form1.pvindex = 0x03 + 30 * mvars.dualduty; Form1.pvindex <= 0x0D + 30 * mvars.dualduty; Form1.pvindex++)
                    //{
                    //    /*
                    //    int svbinI = mUser.HexToDec((string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16)].Value + (string)dgvBin.Rows[(0x1F * 2) / 16].Cells[((0x1F * 2) % 16 + 1)].Value);
                    //    string svbinS = mUser.DecToBin(svbinI, 16);
                    //    lbl_BinVol.Text = ((0.01953 * mUser.BinToDec(svbinS.Substring(6, 10)) * Convert.ToInt32(lbl_BinVal.Text)) / 1024).ToString("##0.0###");
                    //    */
                    //    int sH = gmax * 2 - (Form1.pvindex % 30 - 0x02) * 2;    /// 12*2-(3%30-2)*2 = 24-(3-2)*2 = 22,20,18,,,
                    //    int sL = sH + 1;                                        /// 24+1 =                         23,21,19,,,
                    //    int svtr = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sH]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, sL]);
                    //    UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svtr), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    //    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(0, 2);
                    //    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svtr, 4).Substring(2, 2);
                    //    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    //    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    //}
                    //mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1);
                    //mp.mhcm603WriteMulti(mvars.cm603WRaddr, svi, (byte)(0x03 + 30 * mvars.dualduty), (byte)(gmax * 2 - 2)); //每個bank不含 VGMA0 有11組gamma共22byte
                    //if (svi == 2)
                    //{
                    //    if (mvars.dualduty == 0) Form1.pvindex = 0x02; else Form1.pvindex = 0x20;     // Blue VGMA1
                    //    //0灰階最低電壓
                    //    int svgmacode = mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, 0]) * 256 + mp.HexToDec(mvars.pGMA[mvars.dualduty].Data[svi, 1]);

                    //    UInt32 ulCRC = mp.CRC_Cal(Convert.ToUInt16(svgmacode), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                    //    mvars.cm603df[svi, Form1.pvindex * 2] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(0, 2);
                    //    mvars.cm603df[svi, Form1.pvindex * 2 + 1] = mp.DecToHex((int)ulCRC + svgmacode, 4).Substring(2, 2);
                    //    mvars.cm603dfB[svi, Form1.pvindex * 2] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    //    mvars.cm603dfB[svi, Form1.pvindex * 2 + 1] = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    //    mvars.lblCmd = "GammaSet_" + mvars.msrColor[svi + 2].Substring(0, 1) + "_Reg" + Form1.pvindex + "_G0";
                    //    byte dataH = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2]);
                    //    byte dataL = (byte)mp.HexToDec(mvars.cm603df[svi, Form1.pvindex * 2 + 1]);
                    //    mp.mhcm603WriteSingleReg(mvars.cm603WRaddr, (byte)Form1.pvindex, 2, dataH, dataL);
                    //}
                }

            }
        }
        #endregion cm603
















        //for 順源新調法(0階G,B電壓由高往低, 低於0.0001時中斷在當下灰階再減5階        

        public static void markreset(int svtotalcounts, bool svdelfb)
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
            mvars.errCode = "0";
            mvars.Break = false;
        }

        //public static void markreset(int svtotalcounts, bool svdelfb, bool selfrun)
        //{
        //    mvars.lstget.Items.Clear();
        //    mvars.t1 = DateTime.Now;
        //    mvars.strReceive = "";
        //    mvars.lCounts = svtotalcounts;
        //    mvars.lCount = 0;
        //    Array.Resize(ref mvars.lCmd, svtotalcounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
        //    Array.Resize(ref mvars.lGet, svtotalcounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
        //    mvars.flgDelFB = svdelfb;
        //    mvars.flgSelf = selfrun;
        //    Form1.tslblStatus.Text = "";
        //}

        public static void cUIREGWANY(string[] svuiuser)
        {
            mp.markreset(99, false);
            //if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            bool svnvBoardcast = mvars.nvBoardcast;
            mvars.nvBoardcast = false;



            mvars.lblCmd = "UIREGWANY";
            mp.mUIREGWANY(svuiuser);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-4"; goto ExNovaAGMA; }



        ExNovaAGMA:
            //if (mvars.svnova == false && mvars.UUT.demo == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
            mvars.nvBoardcast = svnvBoardcast;
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }









        public static void mUIREGWANY(string[] svuiuser)    //0x4C  最新版定義方法 svuiuser = "0,26~1,2...."
        {
            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 2; mvars.NumBytesToRead = 28;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + (4 + svuiuser.Length * 4 + 4 + 1));
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 65);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            int svjst = Convert.ToInt16(svuiuser[0].Split(',')[0]);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x4C;                                                       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = Convert.ToByte((4 + svuiuser.Length * 4 + 4 + 1) / 256);    //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = Convert.ToByte((4 + svuiuser.Length * 4 + 4 + 1) % 256);    //Size

            for (int j = 0; j < svuiuser.Length; j++)
            {
                int svjv = Convert.ToInt16(svuiuser[j].Split(',')[1]);
                mvars.RS485_WriteDataBuffer[5 + j * 4 + svns] = Convert.ToByte((j + svjst) / 256);              //Addr
                mvars.RS485_WriteDataBuffer[6 + j * 4 + svns] = Convert.ToByte((j + svjst) % 256);              //Addr
                mvars.RS485_WriteDataBuffer[7 + j * 4 + svns] = Convert.ToByte(svjv / 256);                     //Data
                mvars.RS485_WriteDataBuffer[8 + j * 4 + svns] = Convert.ToByte(svjv % 256);                     //Data
            }

            funSendMessageTo();
        }


        public static void pidinit()
        {
            mp.markreset(999, false);
            Form1.tslblStatus.Text = "pidinit";
            Form1.cmbdeviceid.Items.Clear();//m
            Form1.cmbdeviceid.Items.Add(" " + mvars.deviceID.Substring(0, 2) + "00");
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                for (int i = 1; i <= 16; i++) Form1.cmbdeviceid.Items.Add(" " + mvars.deviceID.Substring(0, 2) + mp.DecToHex(i, 2));
            }
            else if (mvars.deviceID.Substring(0, 2) == "06")
            {
                for (int i = 1; i <= 12; i++) Form1.cmbdeviceid.Items.Add(" " + mvars.deviceID.Substring(0, 2) + mp.DecToHex(i, 2));
            }

            if (mvars.demoMode) 
            {
                #region demoMode
                Form1.tslblCOM.Text = "COM D";
                if (mvars.deviceID.Substring(0, 2) == "05")
                {
                    mvars.verMCU = "0039";
                    mvars.verFPGA = "61.11-61.60";
                    mvars.verFPGAm = new string[mvars.verFPGA.Split('-').Length];
                    mvars.verFPGAm[0] = mvars.verFPGA.Split('-')[0].Replace(".", "");
                    mvars.verFPGAm[1] = mvars.verFPGA.Split('-')[1].Replace(".", "");
                    mvars.verEDID = "1.15";
                    Form1.tslblStatus.Text = "MCU-demo," + mvars.verMCU + ",FPGA-demo," + mvars.verFPGA + ",EDID-demo," + mvars.verEDID;
                    Form1.tslbldeviceID.Text = mvars.deviceID;
                }
                else if (mvars.deviceID.Substring(0, 2) == "06")
                {
                    Form1.tslblStatus.Text = "MCU,demo 0045,FPGA,demo 61.71-61.70";
                    Form1.tslbldeviceID.Text = mvars.deviceID;
                }
                doDelayms(1);
                #endregion demoMode
            }
            else
            {
                mvars.lblCompose = "PIDINIT";
                mp.cmdHW(255, 128, 128, out string SvIF);
                if (SvIF == "232")
                {
                    if (Form1.tsmnucheck485.Checked) { SvIF = mvars.Comm[0]; mvars.Comm[0] = Form1.tslblCOM.Text; Form1.tsmnucheck485.Checked = false; }
                    int i = 0;
                    mvars.verMCU = "-1";
                    mp.Sp1open(mvars.Comm[i]);
                    mvars.lblCmd = "MCU_VERSION";
                    mp.mhVersion();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                    {
                        mvars.msgA = "(pidinit)  " + (i + 1) + "." + mvars.Comm[i] + " open MCU_VERSION DONE";
                    }
                    else
                    {
                        mvars.msgA = "(pidinit)  " + (i + 1) + "." + mvars.Comm[i] + " open MCU_VERSION error";
                        CommClose();
                    }
                    mp.funSaveLogs(mvars.msgA); mvars.msgA = "";

                    if (mvars.verMCU.IndexOf("-2.",0) != -1)
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") Form1.tslblStatus.Text = "Error,Select deviceID:" + mvars.deviceID + " <> read:" + mvars.verMCU.Split('.')[1];
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") Form1.tslblStatus.Text = "Error,硬體ID:" + mvars.deviceID + " 與回讀ID:" + mvars.verMCU.Split('.')[1] + " 不符合";
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") Form1.tslblStatus.Text = "Error,硬体ID:" + mvars.deviceID + " 与回读ID:" + mvars.verMCU.Split('.')[1] + " 不符合";
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") Form1.tslblStatus.Text = "Error,Select deviceID:" + mvars.deviceID + " <> read:" + mvars.verMCU.Split('.')[1];
                        goto Ex;                    
                    }

                    //if (i >= mvars.Comm.Length)
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US") Form1.tslblStatus.Text = "Error,COM amount " + mvars.Comm.Length + ",but hardware no response";
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") Form1.tslblStatus.Text = "Error,通訊埠數量: " + mvars.Comm.Length + ",但是硬體沒有回應";
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN") Form1.tslblStatus.Text = "Error,通讯埠数量: " + mvars.Comm.Length + ",但是硬体没有回应";
                    //    else if (MultiLanguage.DefaultLanguage == "ja-JP") Form1.tslblStatus.Text = "Error,COM amount " + mvars.Comm.Length + ",but hardware no response";

                    //    goto Ex;
                    //    //return;                     
                    //}

                    if (mvars.deviceID == "0200")
                    {
                        if (mvars.verMCU == "" || mvars.verMCU == null) { Form1.tslblStatus.Text = "Error,Please check the Control-Board(0x0200) no response"; }
                        else
                        {
                            mvars.FPGA_START_ADDR = 0x60000;
                            mvars.GAMMA_START_ADDR = 0x62000;
                            if (mvars.verMCU.Substring(0, 1).ToUpper() == "O" || mvars.verMCU.Substring(0, 1).ToUpper() == "A" || mvars.verMCU.Substring(0, 1).ToUpper() == "B")
                            {
                                if (mvars.verMCU == "One-200408-T0004" || mvars.verMCU.Substring("One-200408-".Length, 1) == "R")
                                {
                                    string svstr = " PB.A + +, PB.B + +, PB.C + +, PB.D + +, PB.E + -, PB.F + -, PB.G + -, PB.H + -";
                                    mvars.I2C_CMD = svstr.Split(',');
                                    Form1.cmbhPB.Items.Clear();
                                    Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                    Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdClk = 0xFF; mvars.SercomCmdRd = 0xFF; mvars.SercomCmdWr = 0x70; mvars.SercomCmdWrRd = 0x71;
                                }
                                else
                                {
                                    string svstr = " 1-1_1, 1-1_2, 1-1_3, 1-1_4, 1-2_1, 1-2_2, 1-2_3, 1-2_4, 2-1_1, 2-1_2, 2-1_3, 2-1_4, 2-2_1, 2-2_2, 2-2_3, 2-2_4";
                                    mvars.I2C_CMD = svstr.Split(',');
                                    Form1.cmbhPB.Items.Clear();
                                    Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                    Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x60; mvars.SercomCmdWrRd = 0x61;
                                }
                                Form1.tslblStatus.Text = "MCU ver " + mvars.verMCU;
                                Form1.tslbldeviceID.Text = mvars.deviceID;
                            }
                            else { Form1.tslblStatus.Text = "Error,MCU response (" + mvars.verMCU + ") error"; }
                        }
                    }
                    else if (mvars.deviceName=="H5512A")
                    {
                        if (i < mvars.Comm.Length)
                        {
                            string svstr = "";
                            #region 透過BB回讀
                            //mvars.lblCmd = "OBB_XBS";
                            //mp.OBB_XBS();
                            //if (mvars.verMCUS.Length < 99)
                            //{
                            //    mvars.FPGA_START_ADDR = 0x30000;
                            //    mvars.GAMMA_START_ADDR = 0x32000;
                            //    cmbdeviceid.Items.Clear();//m
                            //    for (int j = 0; j < mvars.verMCUS.Length; j++)
                            //    {
                            //        mvars.lblCmd = "MCU_VERSION";
                            //        mvars.deviceID = "03" + string.Format("{0:00}", (j + 1));
                            //        mp.mhVersion();
                            //        if (mvars.verMCUS[j] == "-1" || mvars.verMCUS[j] == null) { break; }
                            //        cmbdeviceid.Items.Add(" XB" + string.Format("{0:00}", j + 1));  //m
                            //        svstr += ", " + (2 * j + 1).ToString() + ", " + (2 * j + 2).ToString();
                            //    }
                            //    if (mvars.verMCUS[0].IndexOf("ACB", 0) > -1 && Convert.ToInt16(mvars.verMCUS[0].Substring(mvars.verMCUS[0].Length - "0016".Length, "0016".Length)) >= 16) { cmbdeviceid.Items.Insert(0, " XB_ALL"); }
                            //    if (svstr.Length > 3)
                            //    {
                            //        svstr = svstr.Substring(1, svstr.Length - 1);
                            //        mvars.I2C_CMD = svstr.Split(',');
                            //        cmbhPB.Items.Clear();
                            //        cmbhPB.Items.AddRange(svstr.Split(','));
                            //        cmbhPB.Text = cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                            //        int j = cmbdeviceid.FindString(" XB01");//m
                            //        cmbdeviceid.Text = cmbdeviceid.Items[j].ToString();//m
                            //        Form1.tslblStatus.Text = "XB amount " + mvars.verMCUS.Length;
                            //    }
                            //    else { Form1.tslblStatus.Text = "Error，No X-Board"; }
                            //}
                            //else { Form1.tslblStatus.Text = "Error，X-Board seek fail"; }
                            #endregion 透過BB回讀

                            #region 循環式
                            Array.Resize(ref mvars.verMCUS, 2);
                            Array.Clear(mvars.verMCUS, 0, mvars.verMCUS.Length);
                            if (mvars.verMCUS.Length < 99)
                            {
                                mvars.FPGA_START_ADDR = 0x30000;
                                mvars.GAMMA_START_ADDR = 0x32000;
                                Form1.cmbdeviceid.Items.Clear();//m
                                for (int j = 0; j < mvars.verMCUS.Length; j++)
                                {
                                    mvars.lblCmd = "MCU_VERSION";
                                    mvars.deviceID = "03" + string.Format("{0:00}", (j + 1));
                                    mp.mhVersion();
                                    //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { mvars.verMCUS[j] = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]; }
                                    if (mvars.verMCUS[j] == "-1" || mvars.verMCUS[j] == null) { break; }
                                    Form1.cmbdeviceid.Items.Add(" XB" + string.Format("{0:00}", j + 1));  //m
                                    svstr += ", " + (2 * j + 1).ToString() + ", " + (2 * j + 2).ToString();
                                }
                                if (mvars.verMCUS[0].IndexOf("ACB", 0) > -1 && Convert.ToInt16(mvars.verMCUS[0].Substring(mvars.verMCUS[0].Length - "0016".Length, "0016".Length)) >= 16) { Form1.cmbdeviceid.Items.Insert(0, " XB_ALL"); }
                                if (svstr.Length > 3)
                                {
                                    svstr = svstr.Substring(1, svstr.Length - 1);

                                    mvars.I2C_CMD = svstr.Split(',');

                                    Form1.cmbhPB.Items.Clear();
                                    Form1.cmbhPB.Items.AddRange(svstr.Split(','));
                                    Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                                    int j = Form1.cmbdeviceid.FindString(" XB01");//m
                                    Form1.cmbdeviceid.Text = Form1.cmbdeviceid.Items[j].ToString();//m
                                    Form1.tslblStatus.Text = "XB amount " + mvars.verMCUS.Length;
                                }
                                else { Form1.tslblStatus.Text = "Error,No X-Board"; }
                            }
                            else { Form1.tslblStatus.Text = "Error,X-Board seek fail"; }
                            #endregion 循環式
                        }
                        else { Form1.tslblStatus.Text = "Error,Please check the BridgeBoard(0x0300) no response"; }
                    }
                    else if (mvars.deviceID == "0400")
                    {
                        if (mvars.verMCU == "" || mvars.verMCU == null) { Form1.tslblStatus.Text = "Error,Please check the Control-Board(0x" + 0200 + ") no response"; }
                        else
                        {
                            mvars.FPGA_START_ADDR = 0x60000;
                            mvars.GAMMA_START_ADDR = 0x62000;
                            if (mvars.verMCU.Substring(0, 1).ToUpper() == "O" || mvars.verMCU.Substring(0, 1).ToUpper() == "A" || mvars.verMCU.Substring(0, 1).ToUpper() == "B")
                            {
                                string svstr = " 1-1_1, 1-1_2, 1-1_3, 1-1_4, 1-2_1, 1-2_2, 1-2_3, 1-2_4";
                                mvars.I2C_CMD = svstr.Split(',');
                                Form1.cmbhPB.Items.Clear();
                                Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;

                                Form1.tslblStatus.Text = "MCU ver " + mvars.verMCU;
                                Form1.tslbldeviceID.Text = mvars.deviceID;
                            }
                            else { Form1.tslblStatus.Text = "Error,MCU response (" + mvars.verMCU + ") error"; }
                        }
                    }
                    else if (mvars.deviceID.Substring(0,2) == "05")
                    {
                        string svverFPGA = "-1~-1";
                        if (mvars.verMCU == "-1" || mvars.verMCU == null) { Form1.tslblStatus.Text = "Error," + mvars.Comm.Length + "COMs but CB(0x0500) no response"; }
                        else if (mvars.verMCU.Substring(mvars.verMCU.Length-5,1) != "P") { Form1.tslblStatus.Text = "Error,MCU ver: " + mvars.verMCU + " != (0x0500)"; }
                        else
                        {
                            mvars.FPGA_START_ADDR = 0x60000;
                            mvars.GAMMA_START_ADDR = 0x62000;
                            //mvars.ver
                            if (mvars.verMCU.Substring(0, 1).ToUpper() == "O" || mvars.verMCU.Substring(0, 1).ToUpper() == "A" || mvars.verMCU.Substring(0, 1).ToUpper() == "B")
                            {
                                Form1.pvindex = 0;
                                mvars.lblCmd = "FPGA_SPI_R";
                                mp.mhFPGASPIREAD();
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                                    svverFPGA = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];

                                if (svverFPGA.IndexOf("-",0) != -1)
                                {
                                    mvars.verFPGAm[0] = svverFPGA.Split('-')[0];
                                    mvars.verFPGAm[1] = svverFPGA.Split('-')[1];
                                }

                                string svstr = " A, B, C, D, E, F, G, H";
                                mvars.I2C_CMD = svstr.Split(',');
                                Form1.cmbhPB.Items.Clear();
                                Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                            }
                            else { Form1.tslblStatus.Text = "Error,MCU response (" + mvars.verMCU + ") error"; }
                        }
                        if (mvars.verMCU !="-1" && mvars.verFPGA != "-1")
                        {
                            Byte[] arr = new byte[2]; //Byte[] rd_arr = new byte[2];
                            uc_box.LT8668rd_arr = new byte[1];
                            byte VerHi = 0, VerLo = 0;

                            mvars.lblCmd = "LT8668_Bin_WrRd";
                            arr[0] = 0x82; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                            {
                                VerHi = mvars.ReadDataBuffer[7];
                                mvars.lblCmd = "LT8668_Bin_WrRd";
                                arr[0] = 0x83; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) VerHi = 0;
                                else VerLo = mvars.ReadDataBuffer[7];
                            }
                            mvars.verEDID = VerHi + "." + VerLo;

                            mvars.lblCmd = "MCU_INFORMATION";
                            mp.mhMCUinf();                            

                            Form1.tslblStatus.Text = "MCU," + mvars.verMCU + ",FPGA," + svverFPGA + ",EDID," + mvars.verEDID;
                            Form1.tslbldeviceID.Text = mvars.deviceID;
                            doDelayms(10);
                        }
                    }
                    else if (mvars.deviceID.Substring(0, 2) == "06")
                    {
                        if (mvars.verMCU == "-1" || mvars.verMCU == null) { Form1.tslblStatus.Text = "Error," + mvars.Comm.Length + "COMs but CB(0x0600) no response"; }
                        else if (mvars.verMCU.Substring(mvars.verMCU.Length - 7, 2) != "TC" && 
                                 mvars.verMCU.Substring(mvars.verMCU.Length - 7, 2) != "TS") 
                        { 
                            Form1.tslblStatus.Text = "Error,MCU ver: " + mvars.verMCU + " != " + mvars.verMCU.Substring(mvars.verMCU.Length - 7, 2); 
                        }
                        else
                        {
                            mvars.FPGA_START_ADDR = 0x60000;
                            mvars.GAMMA_START_ADDR = 0x62000;
                            if (mvars.verMCU.Substring(0, 1).ToUpper() == "O" || mvars.verMCU.Substring(0, 1).ToUpper() == "A" || mvars.verMCU.Substring(0, 1).ToUpper() == "B")
                            {
                                Form1.pvindex = 0;
                                mvars.lblCmd = "FPGA_SPI_R";
                                mp.mhFPGASPIREAD();
                                string svverFPGA = "-1~-1";
                                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                                {
                                    svverFPGA = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                                }
                                string svstr = " A, B, C, D, E, F, G, H, I, J, K, L";
                                mvars.I2C_CMD = svstr.Split(',');
                                Form1.cmbhPB.Items.Clear();
                                Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;

                                Form1.tslblStatus.Text = "MCU," + mvars.verMCU + ",FPGA," + svverFPGA;
                                Form1.tslbldeviceID.Text = mvars.deviceID;
                                doDelayms(100);
                            }
                            else { Form1.tslblStatus.Text = "Error,MCU response (" + mvars.verMCU + ") error"; }
                        }
                    }

                    else if (mvars.deviceID.Substring(0, 2) == "10")
                    {
                        string svverFPGA = "-1~-1";
                        if (mvars.verMCU == "-1" || mvars.verMCU == null) { Form1.tslblStatus.Text = "Error," + mvars.Comm.Length + "COMs but CB(0x" + mvars.deviceID + ") no response"; }
                        else if (mvars.verMCU.Split('-')[mvars.verMCU.Split('-').Length-2] != "CS") { Form1.tslblStatus.Text = "Error,MCU ver: " + mvars.verMCU + " != (0x" + mvars.deviceID + ")"; }
                        else
                        {
                            mvars.FPGA_START_ADDR = 0x60000;
                            mvars.GAMMA_START_ADDR = 0x62000;
                            //mvars.ver
                            if (mvars.verMCU.Substring(0, 1).ToUpper() == "O" || mvars.verMCU.Substring(0, 1).ToUpper() == "A" || mvars.verMCU.Substring(0, 1).ToUpper() == "B")
                            {
                                Form1.pvindex = 0;
                                mvars.lblCmd = "FPGA_SPI_R";
                                mp.mhFPGASPIREAD();
                                string svstr = " 1, 2, 3, 4";
                                mvars.I2C_CMD = svstr.Split(',');
                                Form1.cmbhPB.Items.Clear();
                                Form1.cmbhPB.Items.AddRange(mvars.I2C_CMD);
                                Form1.cmbhPB.Text = Form1.cmbhPB.Items[0].ToString(); mvars.SercomCmdWr = 0x50; mvars.SercomCmdWrRd = 0x51;
                            }
                            else { Form1.tslblStatus.Text = "Error,MCU response (" + mvars.verMCU + ") error"; }
                        }
                        if (mvars.verMCU != "-1" && mvars.verFPGA != "-1")
                        {
                            //Byte[] arr = new byte[2]; //Byte[] rd_arr = new byte[2];
                            //uc_box.LT8668rd_arr = new byte[1];
                            //byte VerHi = 0, VerLo = 0;
                            //mvars.lblCmd = "LT8668_Bin_WrRd";
                            //arr[0] = 0x82; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                            //{
                            //    VerHi = mvars.ReadDataBuffer[7];
                            //    mvars.lblCmd = "LT8668_Bin_WrRd";
                            //    arr[0] = 0x83; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) VerHi = 0;
                            //    else VerLo = mvars.ReadDataBuffer[7];
                            //}
                            //mvars.verEDID = VerHi + "." + VerLo;

                            mvars.lblCmd = "MCU_INFORMATION";
                            mp.mhMCUinf();

                            Form1.tslblStatus.Text = "MCU," + mvars.verMCU + ",FPGA," + mvars.verFPGA;// + ",EDID," + mvars.verEDID;
                            Form1.tslbldeviceID.Text = mvars.deviceID;
                            doDelayms(10);
                        }
                    }

                    Form1.cmbhPB.SelectedIndex = 0;
                    Form1.cmbCM603.SelectedIndex = 0;
                }
                else 
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { Form1.tslblStatus.Text=@"No Any ""USB Serial Device"""; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { Form1.tslblStatus.Text = @"沒有任何 ""USB 序列裝置"""; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { Form1.tslblStatus.Text = @"沒有任何 ""USB 串行设备"""; }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { Form1.tslblStatus.Text = @"No Any ""USB Serial Device"""; }
                }
                //訊息顯示
                if (mvars.flgSelf) mp.showStatus1(Form1.tslblStatus.Text, Form1.lstget1, "");

                Form1.cmbdeviceid.SelectedIndex = 0;
                Form1.cmbhPB.SelectedIndex = 0;
                Form1.cmbCM603.SelectedIndex = 0;

            Ex:
                if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Close(); }
                mvars.flgDelFB = false;
                mvars.lCounts = mvars.lCount + 1;
                mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mvars.flgSend = true; mvars.flgReceived = false;
                if (Form1.tslblStatus.Text.Substring(0, 5) == "Error") { mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0"))); }
                else
                {
                    mvars.strReceive = "DONE,1,16,0,0,10,3," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (30 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0")));
                }
                mvars.tmeRSIn.Enabled = true;
                mvars.flgReceived = true;
            }
        }








        public static Form frm = null;



        public static void Taskkill(string ProcessName)
        {
            try
            {
                using (Process P = new Process())
                {
                    P.StartInfo = new ProcessStartInfo()
                    {
                        FileName = "taskkill",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = "/F /IM \"" + ProcessName + "\""
                    };
                    P.Start();
                    P.WaitForExit(60000);
                }
            }
            catch
            {
                using (Process P = new Process())
                {
                    P.StartInfo = new ProcessStartInfo()
                    {
                        FileName = "tskill",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = "\"" + ProcessName + "\" /A /V"
                    };
                    P.Start();
                    P.WaitForExit(60000);
                }
            }
        }









        public static void cWTPGONOFF(bool SvON)   //1010
        {
            if (SvON) { mvars.lblCompose = "WT_GRAY_ON"; } else { mvars.lblCompose = "WT_GRAY_OFF"; }
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }
            int svdata;

            if (mvars.demoMode == false && SvON)
            {
                Form1.pvindex = 1; svdata = 3;
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(svdata);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }

                Form1.pvindex = 21; svdata = 257;
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(svdata);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }
                if (mvars.verFPGA.Length > 4)
                {
                    int svm = 3;
                    if (mvars.svnova && mvars.nvBoardcast == false) { svm += 3; }
                    for (int svi = mvars.FPGA_X_START; svi <= mvars.FPGA_Y_END; svi++)
                    {
                        Form1.pvindex = svi;
                        mvars.lblCmd = "FPGA_SPI_W";
                        mp.mhFPGASPIWRITE(Convert.ToInt16(mvars.strData[svm]));
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
                        svm++;
                    }
                }
            }
            else if (mvars.demoMode == false && SvON == false)
            {
                Form1.pvindex = 1; svdata = 0;
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(svdata);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-4"; }

                Form1.pvindex = 21; svdata = 4;
                mp.mhFPGASPIWRITE(svdata);
                mvars.lblCmd = "FPGA_SPI_W";
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-5";  }
            }
            Form1.pvindex = 255; 
            mvars.lblCmd = "FPGA_SPI_W255";
            mp.mhFPGASPIWRITE(0);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-255"; }

            Form1.pvindex = 255;
            mvars.lblCmd = "FPGA_SPI_W255";
            mp.mhFPGASPIWRITE(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-255";  }

            Form1.pvindex = 255;
            mvars.lblCmd = "FPGA_SPI_W255";
            mp.mhFPGASPIWRITE(0);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-255"; }


            //Ex:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (30 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0")));
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0"))); }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static void cWTPG()   //1010
        {
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            if (mvars.demoMode == false)
            {
                int svm = 2;
                if (mvars.svnova == true && mvars.nvBoardcast == false) { svm += 3; }
                //mvars.lblCmd = "FPGA_SPI_W";
                //Form1.pvindex = mvars.FPGA_GRAY_R; 
                //mp.mhFPGASPIWRITE(Convert.ToInt16(SvR)); 
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1";  }
                //mvars.lblCmd = "FPGA_SPI_W";
                //Form1.pvindex = mvars.FPGA_GRAY_G; 
                //mp.mhFPGASPIWRITE(Convert.ToInt16(SvG)); 
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2";  }
                //mvars.lblCmd = "FPGA_SPI_W";
                //Form1.pvindex = mvars.FPGA_GRAY_B; 
                //mp.mhFPGASPIWRITE(Convert.ToInt16(SvB)); 
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3";  }

                for (int svi = mvars.FPGA_GRAY_R; svi <= mvars.FPGA_GRAY_B; svi++)
                {
                    Form1.pvindex = svi;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(Convert.ToInt16(mvars.strData[svm]));
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
                    svm++;
                }
            }


            //Ex:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (30 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0")));
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0"))); }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }





        public static void mhREADJEDECID(bool svCB) //c12a Novastar共用
        {
            #region Novastar Setup
            byte svns = 2;
            mvars.Delaymillisec = 2; mvars.NumBytesToRead = 28;
            if (svCB) { Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E); }
            else { Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0C); }
            #endregion Novastar
            if (mvars.svnova == false)
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
                Array.Resize(ref mvars.ReadDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            if (svCB)
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x3A;            //Cmd(0x3A.SPI Cmd Read)
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;            //Size  + Write Size
                mvars.RS485_WriteDataBuffer[5 + svns] = 0x00;            //Write Size
                mvars.RS485_WriteDataBuffer[6 + svns] = 0x01;            //Write Size
                mvars.RS485_WriteDataBuffer[7 + svns] = 0x9F;            //SPI Flash Command:Read Identification
                mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;            //Read Size
                mvars.RS485_WriteDataBuffer[9 + svns] = 0x03;            //Read Size
            }
            else
            {
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x31;            //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = 0x0C;            //Size
                mvars.RS485_WriteDataBuffer[5 + svns] = 0x9F;            //QSPI ReadJedecId Command
                mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;            //Read Size
                mvars.RS485_WriteDataBuffer[7 + svns] = 0x03;            //Read Size
            }
            mp.funSendMessageTo();
        }


        public static byte ASCII_GetByte(string svs)
        {
            //int svbt = (int)svs;  //< br > Console.WriteLine(asc.ToString());
            //return (byte)svbt;

            byte[] array = System.Text.Encoding.ASCII.GetBytes(svs);
            int svbt = (int)(array[0]);
            //string ASCIIstr1 = Convert.ToString(asciicode); < br > Console.WriteLine(ASCIIstr1);     
            return (byte)svbt;
        }











        public static void cAGMAPrimary()
        {
            //適應20240111 2x8 300nits與Primary
            #region Primary.0500專用 demo mode另外寫一

            mvars.msgA += ",cPrimaryAGMA";
            uc_atg.lblctime.Text = DateTime.Now.ToString();
            mvars.lblCompose = "PAGMA"; mvars.lblCmd = mvars.lblCompose;

            mp.markreset(18888, false);

            float[,] sv20d10 = { { 1, 1, 1, 1 }, { 1, 1, 1, 1 } };  /// 跳水判斷用 分別存下10灰與20灰的亮度後再相除
            uc_atg.lblamb.Text = "";
            gmax = mvars.GMAterminals;      /// cm603 GMAterminals = 12 , cm603Gamma = new int[13] { 0, 1, 8, 16, 24, 40, 64, 96, 128, 160, 192, 224, 255 }
            byte svMTP = mvars.UUT.MTP;
            mvars.UUT.MTP = 0;              /// 先切為DAC Mode把真正的MTP先給svMTP暫存
            typMsrDataNormal[,] SvDefaultMsr = new typMsrDataNormal[6, mvars.cm603Gamma.Length];   //WRGBD,0~8 [0]Blu [1]W [2]R [3]G [4]B [5]D
            bool SvW255;
            string svs = "";
            int svg;
            int svg1 = 0;
            int svc = 1;
            int svi;
            i3_Init i3init = null;
            int svgNx;
            int svxbs = 8;      /// H5512A的XB數量 1=2條燈條(lbs=lightbars)，預設4塊(條)
            if (mvars.deviceID.Substring(0, 2) == "02") { svxbs = 4; }
            else if (mvars.deviceID.Substring(0, 2) == "04") { svxbs = 2; }
            else if (mvars.deviceID.Substring(0, 2) == "05") { svxbs = 8; }     /// W=120
            string svpwic = "";
            typMsrDataNormal[,] SvFullGrayMsr = new typMsrDataNormal[6, 256];   /// WRGBD,0~255 [0]Blu [1]W [2]R [3]G [4]B [5]D
            bool svreATD = false;
            byte sv603amount = 4;   /// Primary 4顆 cm603
            string svUUTIDtemp = mvars.UUT.ID + "_" + mvars.iPBaddr;
            mvars.Break = false;





            byte svDs = 0;



            bool svnewALG = false;
            int[] svATDg = new int[2];
            System.Drawing.Point svpt = new System.Drawing.Point(0, 0);
            System.Drawing.Size svsz = new System.Drawing.Size((int)mvars.UUT.resW, (int)mvars.UUT.resH);


            mvars.t1 = DateTime.Now;


            mvars.lblCmd = "MCU_VERSION";   //verMCU紀錄
            mp.mhVersion();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mvars.ATGerr = "-17.1"; goto ExNovaAGMA; }
            mvars.msgA += "," + mvars.verMCU;

            Form1.pvindex = 0;
            mvars.lblCmd = "FPGA_SPI_R";    //verFPGA紀錄
            mp.mhFPGASPIREAD();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mvars.ATGerr = "-17.2"; goto ExNovaAGMA; }

            //第一次新增
            if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-')[0] ==
                    mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-')[1]) { mvars.ATGerr = "-17.21"; goto ExNovaAGMA; }

            mvars.msgA += "," + mvars.verFPGA;
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                if (mvars.verFPGA.Substring(0, 2) == "61" && Convert.ToInt16(mvars.verFPGA.Substring(3, 2)) < 40)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") uc_atg.lstget.Items.Add("WARNING !! FPGA version " + mvars.verFPGA + " is not support dualduty");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") uc_atg.lstget.Items.Add("請注意 !! FPGA 版本 " + mvars.verFPGA + " 不支援雙duty");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") uc_atg.lstget.Items.Add("请注意 !! FPGA 版本 " + mvars.verFPGA + " 不支援双duty");
                    mvars.ATGerr = "-17.4"; goto ExNovaAGMA;
                }
                else if (mvars.verFPGA != null && mvars.verFPGA.Length > 3 && mvars.verFPGA.Substring(0, 2) != "66" || (mvars.verFPGA.Substring(0, 2) == "61" && Convert.ToInt16(mvars.verFPGA.Substring(3, 2)) >= 40))
                    svnewALG = true;
            }
            else
            {
                if (MultiLanguage.DefaultLanguage == "en-US") uc_atg.lstget.Items.Add("WARNING !! only for model.Primary ");
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") uc_atg.lstget.Items.Add("請注意 !! 僅供 Primary 機種使用");
                else if (MultiLanguage.DefaultLanguage == "zh-CN") uc_atg.lstget.Items.Add("请注意 !! 僅供 Primary 機種使用");
                mvars.ATGerr = "-17.4"; goto ExNovaAGMA;
            }







            #region 軟屏關演算法


            Form1.pvindex = mvars.FPGA_DIP_SW;  /// 
            int strData = 1;                    /// PC mode
            if (mvars.TuningArea.Mark.ToLower() == "pg") strData = 0;
            mvars.lblCmd = "FPGA_SPI_W";
            mp.mhFPGASPIWRITE(2, strData);
            Form1.pvindex = mvars.FPGA_AL_CTRL;
            if (svnewALG) strData = 0; else strData = 4;     /// new algorithm 0，else 1
            mvars.lblCmd = "FPGA_SPI_W";
            mp.mhFPGASPIWRITE(2, strData);
            mp.doDelayms(1000);

            #endregion 軟屏關演算法


            if (mCAs.CAATG.Demo == false)
            {
                int svL = 128;
                mCAs.CAremote(true);
                if (mCAs.CAATG.CAsel == 0)
                {
                    mCAs.objMemory.ChannelNO = mvars.UUT.CAch;
                    mCAs.objCa.DisplayMode = 0;
                }
                else if (mCAs.CAATG.CAsel == 1)
                {
                    mCA4._objMemory.put_ChannelNO(mvars.UUT.CAch);
                    mCA4._objCa.put_DisplayMode(mCA4.MODE_Lvxy);
                }
                float[] svLv = new float[2];
                SeekAGMA("X", 0, 0, 0, true, svL, svL, svL, svpt, svsz);
                CAmeasF();          //Lv109
                if (CAFxLv == -1 && mCAs.CAATG.CAsel == 1) { mCA4.CAzero(); }
                CAmeasF();

                if (CAFxLv < 10)
                {
                    svL = 240;
                    SeekAGMA("X", 0, 0, 0, true, svL, svL, svL, svpt, svsz);
                    CAmeasF();          //Lv109
                    if (CAFxLv == -1 && mCAs.CAATG.CAsel == 1) { mCA4.CAzero(); }
                    CAmeasF();
                }

                svLv[0] = CAFxLv;   //Lv109
                SeekAGMA("X", 0, 0, 0, true, svL + 10, svL + 10, svL + 10, svpt, svsz);
                CAmeasF();          //Lv140
                svLv[1] = CAFxLv;
                if (svL == 240)
                    svATDg[1] = 255;
                else
                {
                    svATDg[1] = Convert.ToInt32(10 * (1100 - svLv[0]) / (svLv[1] - svLv[0]) + 128);
                    if (svATDg[1] > 255) svATDg[1] = 255;
                }
                svATDg[0] = svATDg[1] - 63;     //[0]192 [1]255
                mCAs.CAremote(false);
            }


            mvars.Break = false;
            mvars.ATGerr = "0";
            DateTime svt1;
            if (mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }


            mCAs.CAremote(true);



            if (mvars.UUT.GMAposATD == 1 && mCAs.CAATG.Demo == false && mCAs.CAATG.PlugSP)
            {
                #region C12A/B & UI mCAs.CAATG.Demo == false 的自動找位置 mvars.UUT.GMAposATD = 1
                CAmeasF();
                if (CAFxLv == -1 && mCAs.CAATG.CAsel == 1) { mCA4.CAzero(); }
            reposATD:
                mvars.TuningArea.mX = 0;
                mvars.TuningArea.mY = 0;
                for (svi = 0; svi < svxbs; svi++)
                {
                    if (mvars.Break) { mvars.ATGerr = "-10"; break; }
                    mvars.iPBaddr = (byte)(svi + 1);
                    Form1.cmbhPB.SelectedIndex = mvars.iPBaddr - 1;
                    uc_atg.lblPBAddr.Text = Convert.ToChar(64 + mvars.iPBaddr).ToString();


                    if (svreATD)
                    {
                        mp.Unlock_603(sv603amount);
                        if (mvars.flgdirGamma == false)
                        {
                            mp.Vref_603(sv603amount);
                            mp.TFTVREF_603BK1VCOM(sv603amount);
                            mp.Gamma_603_G0(uc_atg.svG0volt, sv603amount);

                            mp.Gamma_603(sv603amount);
                        }
                    }

                    if (MultiLanguage.DefaultLanguage == "en-US") uc_atg.lstget.Items.Add(" > AutoDetect position .....  ");
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") uc_atg.lstget.Items.Add(" > 自動判斷位置 .....  ");
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") uc_atg.lstget.Items.Add(" > 自动判断灯板位置 .....  ");
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

                    /// 改一下就可以實現由mvars.TuningArea.tW = 120;
                    svpt = new System.Drawing.Point(svi * 120 + mvars.TuningArea.mX, mvars.TuningArea.mY);
                    svsz = new System.Drawing.Size(120, 1080);
                    SeekAGMA("X", 0, 0, 0, true, svATDg[0], svATDg[0], svATDg[0], svpt, svsz);
                    if (svi == 0) doDelayms(1000);
                    if (mCAs.CAATG.Demo)
                    {
                        uc_atg.lstget.Items.Add("  --> CA DEMO if this iPBaddr " + mvars.iPBaddr + "is you want please enter 5");
                        uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                    }
                    bool svbd = false;
                aTD:
                    CAmeasF();
                    uc_atg.lstget.Items.Add("  --> Gray" + svATDg[0] + "，Lv " + CAFxLv);
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                    if (CAFxLv > 3)     //Lv402
                    {

                        if (svreATD == false)
                        {
                            mp.Unlock_603(sv603amount);
                            if (mvars.flgdirGamma == false)
                            {
                                mp.Vref_603(sv603amount);
                                mp.TFTVREF_603BK1VCOM(sv603amount);
                                mp.Gamma_603_G0(uc_atg.svG0volt, sv603amount);

                                mp.Gamma_603(sv603amount);
                            }
                        }

                        SeekAGMA("X", 0, 0, 0, true, svATDg[1], svATDg[1], svATDg[1], svpt, svsz);
                        CAmeasF(); mp.doDelayms(100);
                        uc_atg.lstget.Items.Add("  --> Gray" + svATDg[1] + "，Lv" + CAFxLv + " = svLv");     //Lv1037
                        uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

                        float svLv = CAFxLv;
                        SeekAGMA("X", 0, 0, 0, true, 0, 0, 0, svpt, svsz);
                        if (mCAs.CAATG.Demo)
                        {
                            uc_atg.lstget.Items.Add("  --> CA DEMO if this iPBaddr " + mvars.iPBaddr + "is you want please enter 0");
                            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                        }
                        else
                        {
                            CAmeasF();
                            uc_atg.lstget.Items.Add("  --> Gray0，" + CAFxLv);
                            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                            mp.doDelayms(100);
                        }

                        if (CAFxLv < 0.5)
                        {
                            /// 有可能是左右同步顯示，所以解法是點亮然後對這個燈條下13個綁點由1.4v~0.2v看畫面是不是變暗
                            if (mvars.deviceID.Substring(0, 2) == "05")
                            {
                                SeekAGMA("X", 0, 0, 0, true, svATDg[1], svATDg[1], svATDg[1], svpt, svsz);
                                mvars.mCM603P("5");
                                mp.Gamma_603(sv603amount);

                                CAmeasF();
                                uc_atg.lstget.Items.Add("  --> Gray" + svATDg[1] + "，Lv" + CAFxLv);
                                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                                mp.doDelayms(100);

                                int svir;
                                if (CAFxLv > svLv * 0.5)        //Lv1029 切了mCM603P("5")但是沒反應
                                {
                                    svir = svi;    /// CB接反的左右同步顯示
                                    svi = 7 - svi;
                                    mvars.iPBaddr = (byte)(svi + 1);
                                    Form1.cmbhPB.SelectedIndex = svi;
                                    uc_atg.lblPBAddr.Text = Convert.ToChar(64 + mvars.iPBaddr).ToString();

                                    mp.Unlock_603(sv603amount);
                                    mp.Vref_603(sv603amount);
                                    mp.TFTVREF_603BK1VCOM(sv603amount);
                                    mp.Gamma_603_G0(uc_atg.svG0volt, sv603amount);

                                    mvars.mCM603P("5");
                                    mp.Gamma_603(sv603amount);

                                    CAmeasF();
                                    mp.doDelayms(100);

                                    if (CAFxLv < svLv * 0.5)
                                    {





                                        mvars.mCM603P("1");

                                        Form1.cmbhPB.SelectedIndex = svir;
                                        mp.Gamma_603_G0(uc_atg.svG0volt, sv603amount);
                                        mp.Gamma_603(sv603amount);

                                        Form1.cmbhPB.SelectedIndex = svi;
                                        mp.Gamma_603_G0(uc_atg.svG0volt, sv603amount);
                                        mp.Gamma_603(sv603amount);
                                        mvars.TuningArea.mX += svi * 120;
                                        CAmeasF(); mp.doDelayms(100);
                                        //if (CAFxLv >= svLv * 0.1)
                                        if (CAFxLv >= 3)
                                        {
                                            //uc_atg.lstget.Items.Add("  --> msrLv(" + CAFxLv + ") >= 0.1svLv(" + svLv * 0.1 + ")，checked");
                                            uc_atg.lstget.Items.Add("  --> " + Form1.cmbhPB.Text + "，SercomCmdWR( " + mvars.SercomCmdWrRd + " )，msrLv(" + CAFxLv + ") >= 30)，checked");
                                            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                                            svsz = new System.Drawing.Size(120, mvars.TuningArea.tH);
                                            svsz = new System.Drawing.Size(120, 1080);
                                            break;
                                        }
                                        else
                                        {
                                            uc_atg.lstget.Items.Add("  --> " + Form1.cmbhPB.Text + "，SercomCmdWR( " + mvars.SercomCmdWrRd + " )，msrLv(" + CAFxLv + ") < 30)，-501");
                                            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                                            mvars.ATGerr = "-501";
                                            goto ExNovaAGMA;
                                        }
                                    }
                                    else
                                    {
                                        Form1.cmbhPB.SelectedIndex = svir;





                                        mvars.mCM603P("1");

                                        mp.Gamma_603(sv603amount);

                                        Form1.pvindex = 66;
                                        mvars.lblCmd = "FPGA_SPI_W";
                                        mp.mhFPGASPIWRITE(1, 480);
                                        goto reposATD;
                                    }
                                }
                            }


















                            mvars.mCM603P("1");

                            mp.Gamma_603(sv603amount);
                            mvars.TuningArea.mX += svi * mvars.TuningArea.tW;
                            svsz = new System.Drawing.Size(120, mvars.TuningArea.tH);
                            svsz = new System.Drawing.Size(120, 1080);
                            break;
                        }
                    }
                    else if (CAFxLv <= 3 && CAFxLv > -1 && svbd == false)
                    {
                        SeekAGMA("X", 0, 0, 0, true, svATDg[1], svATDg[1], svATDg[1], svpt, svsz);
                        svbd = true;
                        goto aTD;
                    }
                    else if (CAFxLv == -1)
                    {
                        if (mCAs.CAATG.CAsel == 1) { mCA4.CAzero(); }
                        SeekAGMA("X", 0, 0, 0, true, svATDg[0], svATDg[0], svATDg[0], svpt, svsz);
                        svbd = true;
                        goto aTD;
                    }
                    else CAmeasF();

                    if (mvars.Break) { mvars.ATGerr = "-10"; break; }
                }
                if (svi >= svxbs)
                {
                    mvars.ATGerr = "-12";
                    if (svreATD == false)
                    {
                        Form1.btnfocus.Focus();

                        if (MultiLanguage.DefaultLanguage == "en-US")
                        {
                            DialogResult Result = MessageBox.Show(@"Write DefaultGamma auto and Re-Seek again ?", "AIO", MessageBoxButtons.OKCancel);
                            if (Result == DialogResult.OK) { svreATD = true; goto reposATD; }
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        {
                            DialogResult Result = MessageBox.Show(@"自動寫入 DefaultGamma 然後重新再找一遍 ?", "AIO", MessageBoxButtons.OKCancel);
                            if (Result == DialogResult.OK) { svreATD = true; goto reposATD; }
                        }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        {
                            DialogResult Result = MessageBox.Show(@"自动写入 DefaultGamma 然后重新再找一遍 ?", "AIO", MessageBoxButtons.OKCancel);
                            if (Result == DialogResult.OK) { svreATD = true; goto reposATD; }
                        }
                    }
                    else
                    {
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  AutoDetect Fail.");
                    }
                    goto ExNovaAGMA;
                }
                //uc_atg.txtUUTID.Text = mvars.UUT.ID + "_" + mvars.iPBaddr;
                #endregion AutoDetect
            }
            else
            {
                mp.Unlock_603(sv603amount);
                if (mvars.flgdirGamma == false)
                {
                    mp.Vref_603(sv603amount);
                    mp.TFTVREF_603BK1VCOM(sv603amount);
                    mp.Gamma_603_G0(uc_atg.svG0volt, sv603amount);

                    mp.Gamma_603(sv603amount);
                }
            }


            if (mvars.Break) { mvars.ATGerr = "-10"; goto ExNovaAGMA; }
            if (mvars.ATGerr != "0") goto ExNovaAGMA;


            //if (mvars.deviceID.Substring(0, 2) == "05" && mvars.dualduty == 1)    //20230530 修正 MTP=0 ATGerr=-20" (修改條件式)
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                for (svi = 0; svi <= 3; svi++)
                {
                    if (svi == 0) { mvars.cm603WRaddr = 0xD5; Form1.cmbCM603.SelectedIndex = svi; }
                    else if (svi == 1) { mvars.cm603WRaddr = 0xD7; Form1.cmbCM603.SelectedIndex = svi; }
                    else if (svi == 2) { mvars.cm603WRaddr = 0xDB; Form1.cmbCM603.SelectedIndex = svi; }
                    else if (svi == 3) { mvars.cm603WRaddr = 0xD9; Form1.cmbCM603.SelectedIndex = svi; }
                    mvars.lblCmd = "CM603_READ_" + mvars.cm603WRaddr;
                    mp.mhcm603ReadAll(mvars.cm603WRaddr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mvars.ATGerr = "-18"; }
                    else
                    {
                        int svbinI;
                        string svbinS;
                        int svj;

                        Array.Copy(mvars.RegData, svi * mvars.RegData.GetLength(1), mvars.cm603df, svi * mvars.RegData.GetLength(1), mvars.RegData.GetLength(1));

                        for (byte svm = 0; svm <= mvars.dualduty; svm++)
                        {
                            for (svj = 0; svj < mvars.cm603df.GetLength(1); svj++)  /// 128
                            {
                                mvars.cm603dfB[svi, svj] = (byte)mp.HexToDec(mvars.cm603df[svi, svj]);
                            }
                            if (svi == 3)
                            {
                                svbinI = mvars.cm603dfB[svi, 4] * 256 + mvars.cm603dfB[svi, 5];
                                svbinS = mp.DecToBin(svbinI, 16);
                                mvars.pGMA[svm].Data[0, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.4=pGMA.0(gray0)
                                mvars.pGMA[svm].Data[0, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.5=pGMA.1(gray0)

                                svbinI = mvars.cm603dfB[svi, 6] * 256 + mvars.cm603dfB[svi, 7];
                                svbinS = mp.DecToBin(svbinI, 16);
                                mvars.pGMA[svm].Data[1, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.6=pGMA.0(gray0)
                                mvars.pGMA[svm].Data[1, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.7=pGMA.1(gray0)

                                svbinI = mvars.cm603dfB[svi, 8] * 256 + mvars.cm603dfB[svi, 9];
                                svbinS = mp.DecToBin(svbinI, 16);
                                mvars.pGMA[svm].Data[2, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.8=pGMA.0(gray0)
                                mvars.pGMA[svm].Data[2, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.9=pGMA.1(gray0)
                            }
                            else
                            {
                                for (svj = 4 + 60 * svm; svj <= 26 + 60 * svm; svj += 2)
                                {
                                    svbinI = mvars.cm603dfB[svi, svj] * 256 + mvars.cm603dfB[svi, svj + 1];
                                    svbinS = mp.DecToBin(svbinI, 16);
                                    mvars.pGMA[svm].Data[svi, 28 - svj % 60] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);      /// 603.29-4-1=pGMA.24(gray255)，603.29-26-1=pGMA.2(gray1)
                                    mvars.pGMA[svm].Data[svi, 29 - svj % 60] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      /// 603.29-4  =pGMA.25(gray255)，603.29-26  =pGMA.3(gray1)
                                }
                            }
                        }
                    }
                }

                for (svi = 0; svi < mvars.cm603Gamma.Length; svi++)
                {
                    uc_atg.dgvatg.Rows[8].Cells[svi].Value = mvars.pGMA[0].Data[0, 2 * svi + 0];
                    uc_atg.dgvatg.Rows[9].Cells[svi].Value = mvars.pGMA[0].Data[0, 2 * svi + 1];
                    uc_atg.dgvatg.Rows[10].Cells[svi].Value = mvars.pGMA[0].Data[1, 2 * svi + 0];
                    uc_atg.dgvatg.Rows[11].Cells[svi].Value = mvars.pGMA[0].Data[1, 2 * svi + 1];
                    uc_atg.dgvatg.Rows[12].Cells[svi].Value = mvars.pGMA[0].Data[2, 2 * svi + 0];
                    uc_atg.dgvatg.Rows[13].Cells[svi].Value = mvars.pGMA[0].Data[2, 2 * svi + 1];
                }
                if (mvars.dualduty == 1)    //20230530 修正 MTP=0 ATGerr=-20" (新增判斷式，內容是既有的)
                {
                    for (svi = mvars.cm603Gamma.Length + 1; svi < mvars.cm603Gamma.Length * 2 + 1; svi++)
                    {
                        uc_atg.dgvatg.Rows[8].Cells[svi].Value = mvars.pGMA[1].Data[0, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 0];
                        uc_atg.dgvatg.Rows[9].Cells[svi].Value = mvars.pGMA[1].Data[0, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 1];
                        uc_atg.dgvatg.Rows[10].Cells[svi].Value = mvars.pGMA[1].Data[1, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 0];
                        uc_atg.dgvatg.Rows[11].Cells[svi].Value = mvars.pGMA[1].Data[1, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 1];
                        uc_atg.dgvatg.Rows[12].Cells[svi].Value = mvars.pGMA[1].Data[2, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 0];
                        uc_atg.dgvatg.Rows[13].Cells[svi].Value = mvars.pGMA[1].Data[2, 2 * (svi % (mvars.cm603Gamma.Length + 1)) + 1];
                        uc_atg.dgvatg.Rows[7].Cells[svi].Value = uc_atg.dgvatg.Rows[7].Cells[svi - mvars.cm603Gamma.Length - 1].Value;
                    }
                }
            }

            #region  等待按下 "下一步"
            if (mvars.deviceID.Substring(0, 2) == "05" && mvars.flgdirGamma == false)
            {
                if (MultiLanguage.DefaultLanguage == "en-US")
                    uc_atg.lblnext.Text = " Lv ";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    uc_atg.lblnext.Text = " 亮度 ";
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    uc_atg.lblnext.Text = " 亮度 ";

                #region 持續等候下一步
                svs = uc_atg.lblctime.Text;
                uc_atg.lblnext.Visible = true;
                ShowAGMAnT("X", 0, 0, 0, true, 128, 128, 128, false, 100);
                CAmeasF(); mp.doDelayms(100);
                if (CAFxLv < 50) { ShowAGMAnT("X", 0, 0, 0, true, 255, 255, 255, false, 100); }
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    uc_atg.btnBreak.Text = "NEXT";
                    uc_atg.lstget.Items.Add(" -> Panel select and press \"NEXT\" exit panel selection");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    uc_atg.btnBreak.Text = "下一步";
                    uc_atg.lstget.Items.Add(@" -> 選擇燈板位置然後按下""下一步""");
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    uc_atg.btnBreak.Text = "下一步";
                    uc_atg.lstget.Items.Add(@" -> 选择灯板位置然后按下""下一步""");
                }
                mvars.Break = false;
                uc_atg.btnBreak.Focus();
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                svi = 0;
                float svWLv = 1000000f;
                CAmeasF();
                //int svt = 10;
                do
                {
                    CAmeasF();
                    if (MultiLanguage.DefaultLanguage == "en-US")
                        uc_atg.lblnext.Text = " Lv " + CAFxLv.ToString();
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        uc_atg.lblnext.Text = " 亮度 " + CAFxLv.ToString();
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        uc_atg.lblnext.Text = " 亮度 " + CAFxLv.ToString();
                    mp.doDelayms(200);
                    if (MultiLanguage.DefaultLanguage == "en-US")
                        uc_atg.lblctime.Text = string.Format("{0:#####0.0}", CAFxLv) + "，(mininum record " + string.Format("{0:#####0.0}", svWLv) + ") press \"NEXT\" exit panel selection";
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                        uc_atg.lblctime.Text = string.Format("{0:#####0.0}", CAFxLv) + "，(mininum record " + string.Format("{0:#####0.0}", svWLv) + ") 按下 \"下一步\" 結束最暗燈板位置選擇";
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                        uc_atg.lblctime.Text = string.Format("{0:#####0.0}", CAFxLv) + "，(mininum record " + string.Format("{0:#####0.0}", svWLv) + ") 按下 \"下一步\" 结束最暗灯板位置选择";
                }
                while (mvars.Break != true);
                svi = 0;
                #endregion 持續等候下一步

                #region 倒數版 disabled
                //if (MultiLanguage.DefaultLanguage == "en-US") uc_atg.lblnext.Text = "♡ countdown   Lv";
                //else if (MultiLanguage.DefaultLanguage == "zh-CHT") uc_atg.lblnext.Text = "♡ 倒數   亮度";
                //else if (MultiLanguage.DefaultLanguage == "zh-CN") uc_atg.lblnext.Text = "♡ 倒数   亮度";

                //svs = uc_atg.lblctime.Text;
                //uc_atg.lblnext.Visible = true;
                //ShowAGMAnT("X", 0, 0, 0, true, 128, 128, 128, false, 100);
                //CAmeasF(); mp.doDelayms(100);
                //if (CAFxLv < 50) { ShowAGMAnT("X", 0, 0, 0, true, 255, 255, 255, false, 100); }
                //if (MultiLanguage.DefaultLanguage == "en-US")
                //{
                //    uc_atg.btnBreak.Text = "NEXT";
                //    uc_atg.lstget.Items.Add(" -> Panel select and press \"NEXT\" exit panel selection");
                //    uc_atg.lstget.Items.Add(" --> Wait for 10 sec .....");
                //}
                //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //{
                //    uc_atg.btnBreak.Text = "下一步";
                //    uc_atg.lstget.Items.Add(@" -> 選擇燈板位置然後按下""下一步""");
                //    uc_atg.lstget.Items.Add(" --> 等候 10 秒鐘 ......");
                //}
                //else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //{
                //    uc_atg.btnBreak.Text = "下一步";
                //    uc_atg.lstget.Items.Add(@" -> 选择灯板位置然后按下""下一步""");
                //    uc_atg.lstget.Items.Add(" --> 等候 10 秒钟 ......");
                //}
                //mvars.Break = false;
                //uc_atg.btnBreak.Focus();
                //uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                //svi = 0;
                //float svWLv = 1000000f;
                //CAmeasF();
                //int svt = 10;
                //do
                //{
                //    CAmeasF(); 
                //    if (CAFxLv < svWLv) svWLv = CAFxLv;
                //    uc_atg.lblnext.Text = "♡ " + svt.ToString() + "   " + CAFxLv.ToString();

                //    if (MultiLanguage.DefaultLanguage == "en-US")
                //        uc_atg.lblctime.Text = string.Format("{0:#####0.0}", CAFxLv) + "，(mininum record " + string.Format("{0:#####0.0}", svWLv) + ") press \"NEXT\" exit panel selection";
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                //        uc_atg.lblctime.Text = string.Format("{0:#####0.0}", CAFxLv) + "，(mininum record " + string.Format("{0:#####0.0}", svWLv) + ") 按下 \"下一步\" 結束最暗燈板位置選擇";
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                //        uc_atg.lblctime.Text = string.Format("{0:#####0.0}", CAFxLv) + "，(mininum record " + string.Format("{0:#####0.0}", svWLv) + ") 按下 \"下一步\" 结束最暗灯板位置选择";

                //    mp.doDelayms(200);
                //    svi++;
                //    if (svi % 5 == 0)
                //    {
                //        if (svi % 2 == 0) uc_atg.lblnext.Text = "♡ " + (10 - svi / 5) + "   " + CAFxLv.ToString();
                //        else { uc_atg.lblnext.Text = "♥ " + (10 - svi / 5) + "   " + CAFxLv.ToString(); }
                //        svt = 10 - svi / 5;
                //        uc_atg.lstget.Items.RemoveAt(uc_atg.lstget.Items.Count - 1);
                //        if (MultiLanguage.DefaultLanguage == "en-US") uc_atg.lstget.Items.Add(" --> Run autogamma after " + svt.ToString() + " sec .....");
                //        else if (MultiLanguage.DefaultLanguage == "zh-CHT") uc_atg.lstget.Items.Add(" --> " + svt.ToString() + " 秒鐘後自動執行 ......");
                //        else if (MultiLanguage.DefaultLanguage == "zh-CN") uc_atg.lstget.Items.Add(" --> " + svt.ToString() + " 秒钟后自动执行 ......");
                //        uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                //        if (svt <= 0) break;
                //    }
                //    else
                //    {
                //        if (svi % 2 == 0) uc_atg.lblnext.Text = "♡ " + (10 - svi / 5) + "   " + CAFxLv.ToString();
                //        else { uc_atg.lblnext.Text = "♥ " + (10 - svi / 5) + "   " + CAFxLv.ToString(); }
                //    }
                //}
                //while (mvars.Break != true);
                //svi = 0;
                //uc_atg.lstget.Items.RemoveAt(uc_atg.lstget.Items.Count - 1);
                #endregion 倒數版


                for (int m = 0; m < uc_atg.lstget.Items.Count; m++)
                {
                    if (uc_atg.lstget.Items[m].ToString().IndexOf("NEXT", 0) != -1 || uc_atg.lstget.Items[m].ToString().IndexOf("下一步", 0) != -1) { svi = m; break; }
                }
                if (svi != 0)
                {
                    uc_atg.lstget.Items.RemoveAt(uc_atg.lstget.Items.Count - 1);
                    if (mvars.Break)
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") uc_atg.lstget.Items.Add(" -> User press \"NEXT\" to keep gono");
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") uc_atg.lstget.Items.Add(@" -> 使用者按下""下一步""繼續執行");
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") uc_atg.lstget.Items.Add(@" -> 使用者按下""下一步""继续执行");
                    }
                    else
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") uc_atg.lstget.Items.Add(" -> Autorun");
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") uc_atg.lstget.Items.Add(" -> 自動執行");
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") uc_atg.lstget.Items.Add(" -> 自动执行");
                    }
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                }

                if (MultiLanguage.DefaultLanguage == "en-US") uc_atg.btnBreak.Text = "BREAK";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") uc_atg.btnBreak.Text = "中斷";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") uc_atg.btnBreak.Text = "中断";

                uc_atg.lblctime.Text = svs; svs = "";
                uc_atg.lblnext.Visible = false;
                uc_atg.lblnext.Text = "";
                mvars.Break = false;
            }
            #endregion 等待按下 "下一步"


            svUUTIDtemp = mvars.UUT.ID + "_" + Convert.ToChar(64 + mvars.iPBaddr);
            if (mvars.msrgammacurve)
            {
                try
                {
                    if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.csv")) { File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.csv"); }
                }
                catch (Exception ex)
                {
                    mvars.ATGerr = "-27";
                    goto ExNovaAGMA;
                }
            }

            if (mvars.flgdirGamma) goto dirmsrGamma;

            funSaveLogs("↓" + DateTime.Now + " (ATG)   ID:" + mvars.UUT.ID + "," + mvars.UUT.CLv + "," + mvars.UUT.Cx + "," + mvars.UUT.Cy + ",ch." + mvars.UUT.CAch + ",x" + mvars.UUT.WTLvBet +
        mvars.UImajor + ",DeviceID " + mvars.deviceID + ",MCU " + mvars.verMCU + ",FPGA " + mvars.verFPGA + ",PWIC " + svpwic);
            uc_atg.lstget.Items.Add(" -> " + mvars.UImajor + ",DeviceID," + mvars.deviceID + ",MCU," + mvars.verMCU + ",FPGA," + mvars.verFPGA + ",PWIC," + svpwic);
            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

            if (mvars.ATGerr == "-10") { goto ExNovaAGMA; }

            for (svi = 0; svi < 7; svi++)
            {
                for (svg = 1 + svDs; svg < gmax + svDs; svg++) { uc_atg.dgvatg.Rows[svi].Cells[svg].Value = ""; }
            }



            #region pre-measure Major-Dark

            svc = 5;
            ShowAGMAnT("X", 0, 0, 0, true, 0, 0, 0, false, 500);
            CAmeasF();
            SvDefaultMsr[svc, 0].Lv = CAFxLv;
            SvDefaultMsr[svc, 0].Sx = CAFx;
            SvDefaultMsr[svc, 0].Sy = CAFy;
            CAmeasF();
            SvDefaultMsr[svc, 0].Lv += CAFxLv;
            SvDefaultMsr[svc, 0].Sx += CAFx;
            SvDefaultMsr[svc, 0].Sy += CAFy;
            CAmeasF();
            SvDefaultMsr[svc, 0].Lv = (float)Math.Round((SvDefaultMsr[svc, 0].Lv + CAFxLv) / 3, 4);
            SvDefaultMsr[svc, 0].Sx = (float)Math.Round((SvDefaultMsr[svc, 0].Sx + CAFx) / 3, 4);
            SvDefaultMsr[svc, 0].Sy = (float)Math.Round((SvDefaultMsr[svc, 0].Sy + CAFy) / 3, 4);

            CAFxLv = SvDefaultMsr[svc, 0].Lv;
            CAFx = SvDefaultMsr[svc, 0].Sx;
            CAFy = SvDefaultMsr[svc, 0].Sy;

            SvDefaultMsr[1, 0].Lv = SvDefaultMsr[svc, 0].Lv;
            SvDefaultMsr[1, 0].Sx = SvDefaultMsr[svc, 0].Sx;
            SvDefaultMsr[1, 0].Sy = SvDefaultMsr[svc, 0].Sy;
            SvDefaultMsr[2, 0].Lv = SvDefaultMsr[svc, 0].Lv;
            SvDefaultMsr[2, 0].Sx = SvDefaultMsr[svc, 0].Sx;
            SvDefaultMsr[2, 0].Sy = SvDefaultMsr[svc, 0].Sy;
            SvDefaultMsr[3, 0].Lv = SvDefaultMsr[svc, 0].Lv;
            SvDefaultMsr[3, 0].Sx = SvDefaultMsr[svc, 0].Sx;
            SvDefaultMsr[3, 0].Sy = SvDefaultMsr[svc, 0].Sy;
            SvDefaultMsr[4, 0].Lv = SvDefaultMsr[svc, 0].Lv;
            SvDefaultMsr[4, 0].Sx = SvDefaultMsr[svc, 0].Sx;
            SvDefaultMsr[4, 0].Sy = SvDefaultMsr[svc, 0].Sy;
            uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value = SvDefaultMsr[svc, 0].Lv;
            uc_atg.dgvatg.Rows[2].Cells[0 + svDs].Value = SvDefaultMsr[svc, 0].Sx;
            uc_atg.dgvatg.Rows[3].Cells[0 + svDs].Value = SvDefaultMsr[svc, 0].Sy;

            uc_atg.lstget.Items.Add(" ->  pre " + strC[svc] + "Lv check " + SvDefaultMsr[svc, 0].Lv +
                ",x" + SvDefaultMsr[svc, 0].Sx + ",y" + SvDefaultMsr[svc, 0].Sy);
            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;








            if (SvDefaultMsr[svc, 0].Lv > (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus))
            {
                mvars.ATGerr = "-5";
                uc_atg.lstget.Items.Add(" -->  mCM603(" + svg1 + ") " + strC[svc].Substring(0, 1) + "Lv check fail，" + SvDefaultMsr[svc, 0].Lv + " > " + string.Format("{0:#0.0####}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + " cd/m^2");
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                funSaveLogs(" " + DateTime.Now + " (" + mvars.ATGerr + ")   ID:" + mvars.UUT.ID + "，mCM603(" + svg1 + ") " + strC[svc].Substring(0, 1) + "Lv check fail，" + SvDefaultMsr[svc, 0].Lv + " > " + string.Format("{0:#0.0####}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + " cd/m^2");
                //goto ExNovaAGMA;
            }
            #endregion pre-measure





            #region 最高可調可量測的最高階數 = 12 = GMAterminals = gmax
            mvars.byPass = false;
            svt1 = DateTime.Now;
            svgNx = gmax - 1;
            if (mvars.demoMode == false)
            {
                mCAs.CAremote(true);
                for (svg = gmax; svg > svgNx; svg--)
                {
                    ShowAGMAnT("X", 0, 0, 0, true, mvars.cm603Gamma[svg], mvars.cm603Gamma[svg], mvars.cm603Gamma[svg], false, 100);
                    panelsemi_gmax(svg, Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svg + svDs].Value));
                    uc_atg.lblctime.Text += " >> G" + mvars.cm603Gamma[svg] + " " + ((DateTime.Now - svt1).Seconds) + "s";
                    if (mvars.ATGerr != "0") { svg1 = svg; goto ExNovaAGMA; }
                }
                mCAs.CAremote(false);
            }
            #endregion 最高可調可量測的最高階數 = 12 = GMAterminals = gmax



            #region from svgNx(gray224) to g2(gray8)
            mCAs.CAremote(true);
            for (svg = svgNx; svg >= 1; svg--)  //0018版 for (svg = svgNx; svg >= 2; svg--)
            {
                svt1 = DateTime.Now;
                mvars.byPass = false;
                ShowAGMAnT("X", 0, 0, 0, true, mvars.cm603Gamma[svg], mvars.cm603Gamma[svg], mvars.cm603Gamma[svg], false, 500);
                if (mvars.demoMode == false)
                {
                    mvars.ATGerr = panelsemi_B4(svg, Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svg + svDs].Value));
                    if (mvars.ATGerr == "0") for (int svrow = 0; svrow < uc_atg.dgvatg.RowCount; svrow++) { uc_atg.dgvatg.Rows[svrow].Cells[svg + svDs].Style.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255); }
                    uc_atg.lblctime.Text += " >> G" + mvars.cm603Gamma[svg] + " " + ((DateTime.Now - svt1).Seconds) + "s";
                    uc_atg.lstget.Items.Add(uc_atg.lblctime.Text);
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                    if (mvars.ATGerr != "0") { goto ExNovaAGMA; }
                }
            }
            mCAs.CAremote(false);
            #endregion from svgNx(gray224) to g2(gray8)



            #region Dark measurement
            svc = 5;
            ShowAGMAnT("X", 0, 0, 0, true, 0, 0, 0, false, 500);
            CAmeasF();
            SvDefaultMsr[svc, 0].Lv = CAFxLv;
            SvDefaultMsr[svc, 0].Sx = CAFx;
            SvDefaultMsr[svc, 0].Sy = CAFy;
            CAmeasF();
            SvDefaultMsr[svc, 0].Lv += CAFxLv;
            SvDefaultMsr[svc, 0].Sx += CAFx;
            SvDefaultMsr[svc, 0].Sy += CAFy;
            CAmeasF();
            SvDefaultMsr[svc, 0].Lv = (float)Math.Round((SvDefaultMsr[svc, 0].Lv + CAFxLv) / 3, 4);
            SvDefaultMsr[svc, 0].Sx = (float)Math.Round((SvDefaultMsr[svc, 0].Sx + CAFx) / 3, 4);
            SvDefaultMsr[svc, 0].Sy = (float)Math.Round((SvDefaultMsr[svc, 0].Sy + CAFy) / 3, 4);

            CAFxLv = SvDefaultMsr[svc, 0].Lv;
            CAFx = SvDefaultMsr[svc, 0].Sx;
            CAFy = SvDefaultMsr[svc, 0].Sy;

            uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value = SvDefaultMsr[svc, 0].Lv;
            uc_atg.dgvatg.Rows[2].Cells[0 + svDs].Value = SvDefaultMsr[svc, 0].Sx;
            uc_atg.dgvatg.Rows[3].Cells[0 + svDs].Value = SvDefaultMsr[svc, 0].Sy;

            uc_atg.lstget.Items.Add(" ->  pre " + strC[svc] + "Lv check " + SvDefaultMsr[svc, 0].Lv +
                ",x" + SvDefaultMsr[svc, 0].Sx + ",y" + SvDefaultMsr[svc, 0].Sy);
            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
            #endregion 



            #region 端點(terminal spot)亮色度量測 W only
            SvW255 = true;
            svt1 = DateTime.Now;
            mCAs.CAremote(true);
            for (svc = 1; svc <= 4; svc++)
            {
                for (svg1 = gmax; svg1 >= 0; svg1--)
                {
                    if (svc == 1) { ShowAGMAnT("X", 0, 0, 0, true, mvars.cm603Gamma[svg1], mvars.cm603Gamma[svg1], mvars.cm603Gamma[svg1], false, 500); }
                    else if (svc == 2) { ShowAGMAnT("X", 0, 0, 0, true, mvars.cm603Gamma[svg1], 0, 0, false, 500); }
                    else if (svc == 3) { ShowAGMAnT("X", 0, 0, 0, true, 0, mvars.cm603Gamma[svg1], 0, false, 500); }
                    else if (svc == 4) { ShowAGMAnT("X", 0, 0, 0, true, 0, 0, mvars.cm603Gamma[svg1], false, 500); }
                    if (mvars.demoMode == false) CAmeasF();
                    else doDelayms(1600);
                    if (CAFxLv == -1)
                    {
                        mvars.ATGerr = "-8";
                        mp.funSaveLogs("( " + mvars.ATGerr + " ) MeasGray " + mvars.msrColor[svc].Substring(0, 1) + " G" + svg1 + " Lv = -1 (>1000)");
                        goto ExNovaAGMA;
                    }
                    else
                    {
                        if (CAFy * CAFxLv <= 0) { mvars.ATGerr = "-4"; goto ExNovaAGMA; }
                        else
                        {
                            SvDefaultMsr[svc, svg1].Lv = CAFxLv;
                            SvDefaultMsr[svc, svg1].Sx = CAFx;
                            SvDefaultMsr[svc, svg1].Sy = CAFy;
                        }
                    }
                    //svDs借用做為已運算的欄位基數
                    if (svc == 1)
                    {
                        uc_atg.dgvatg.Rows[1].Cells[svg1 + svDs].Value = CAFxLv;
                        uc_atg.dgvatg.Rows[2].Cells[svg1 + svDs].Value = CAFx;
                        uc_atg.dgvatg.Rows[3].Cells[svg1 + svDs].Value = CAFy;
                        uc_atg.dgvatg.Rows[5].Cells[svg1 + svDs].Value = string.Format("{0:0.0###}", Math.Abs(CAFx - mvars.UUT.Cx));
                        uc_atg.dgvatg.Rows[6].Cells[svg1 + svDs].Value = string.Format("{0:0.0###}", Math.Abs(CAFy - mvars.UUT.Cy));
                        if (Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svg1 + svDs].Value) > 0)
                        {
                            uc_atg.dgvatg.Rows[4].Cells[svg1 + svDs].Value = (100 * (CAFxLv / Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svg1 + svDs].Value) - 1)).ToString("#0.0");
                        }
                        if (svg1 < gmax && svg1 > 0)
                        {
                            if (CAFxLv - Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value) > 0 && Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value) - Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value) > 0)
                            {
                                uc_atg.dgvatg.Rows[0].Cells[svg1 + svDs].Value = Convert.ToSingle(Math.Log(Convert.ToDouble((CAFxLv - Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value)) / (Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value) - Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value)))) / Math.Log(Convert.ToDouble((float)mvars.cm603Gamma[svg1] / (float)mvars.cm603Gamma[gmax]))).ToString("#0.0");
                            }
                        }
                        else { uc_atg.dgvatg.Rows[0].Cells[svg1 + svDs].Value = ""; }
                    }
                    uc_atg.lstget.Items.Add(" --> " + mvars.msrColor[svc].Substring(0, 1) + " g" + mvars.cm603Gamma[svg1] + " Lv：" + CAFxLv + " x：" + CAFx + " y：" + CAFy); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                }
                if (SvW255) break;
            }
            uc_atg.lblctime.Text += " >> Wmeas " + ((DateTime.Now - svt1).Seconds) + "s";
            #endregion 端點(terminal spot)亮色度量測 W only



            #region 燒錄前確認 cm603(gmax)
            if (Math.Abs(Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[gmax + svDs].Value)) > 5 ||
                Math.Abs(Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[gmax + svDs].Value)) > 0.003 ||
                Math.Abs(Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[gmax + svDs].Value)) > 0.003)

            {
                ShowAGMAnT("X", 0, 0, 0, true, mvars.cm603Gamma[gmax], mvars.cm603Gamma[gmax], mvars.cm603Gamma[gmax], false, 100);
                svt1 = DateTime.Now;
                mvars.byPass = false;
                if (mvars.demoMode == false)
                {
                    mCAs.CAremote(true);
                    mvars.ATGerr = panelsemi_F(gmax, Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[gmax + svDs].Value));
                    mCAs.CAremote(false);
                    if (mvars.ATGerr != "0")
                    {
                        if (mvars.ATGerr == "-19")
                        {
                            Form1.btnfocus.Focus();
                            MessageBox.Show("WLv" + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + " < " + (mvars.UUT.CLv * mvars.UUT.WTLvBet).ToString("###0.###"));
                        }
                        goto ExNovaAGMA;
                    }
                    uc_atg.lblctime.Text += " >> F_cm603 " + ((DateTime.Now - svt1).Seconds) + "s";
                }
            }
            #endregion 燒錄前確認 cm603(gmax)



            #region OPTcheck exGray
            if (mvars.UUT.exGray > 0)
            {
                ShowAGMAnT("X", 0, 0, 0, true, mvars.UUT.exGray, mvars.UUT.exGray, mvars.UUT.exGray, false, 100);
                if (mvars.demoMode == false)
                {
                    CAmeasF(); doDelayms(100);
                    CAmeasF();
                    uc_atg.lstget.Items.Add(" ->  @ extra W" + mvars.UUT.exGray + "," + CAFxLv + ",x" + CAFx + ",y" + CAFy);
                }
                else uc_atg.lstget.Items.Add(" ->  @ extra W" + mvars.UUT.exGray + ",116.75,0.127,0.07");
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
            }
            #endregion OPTcheck



            #region 20 divide 10 measurement
            for (svc = 0; svc < 4; svc++)
            {
                for (svg = 0; svg <= 1; svg++)
                {
                    if (svc == 0) { ShowAGMAnT("X", 0, 0, 0, true, (20 - 10 * svg), (20 - 10 * svg), (20 - 10 * svg), false, 300); }          //W
                    else if (svc == 1) { ShowAGMAnT("X", 0, 0, 0, true, (20 - 10 * svg), 0, 0, false, 300); }     //R
                    else if (svc == 2) { ShowAGMAnT("X", 0, 0, 0, true, 0, (20 - 10 * svg), 0, false, 300); }     //G
                    else if (svc == 3) { ShowAGMAnT("X", 0, 0, 0, true, 0, 0, (20 - 10 * svg), false, 300); }     //B

                    if (mvars.demoMode == false)
                    {
                        CAmeasF();
                        CAmeasF();
                        sv20d10[svg, svc] = CAFxLv;
                    }
                }
                if (sv20d10[0, svc] / sv20d10[1, svc] < mvars.UUT.ex20d10[svc])
                {
                    //mvars.ATGerr = "-26." + (svc + 1);
                    uc_atg.lstget.Items.Add(" --> (Err." + mvars.ATGerr + ") " + mvars.msrColor[svc + 1] + "20/10(" + sv20d10[0, svc] / sv20d10[1, svc] + ")<" + mvars.UUT.ex20d10[svc] + "(Spec)");
                    mp.funSaveLogs("(Err." + mvars.ATGerr + ") " + mvars.msrColor[svc + 1] + "20 / 10(" + sv20d10[0, svc] / sv20d10[1, svc] + ") < " + mvars.UUT.ex20d10[svc] + "(Spec)");
                }
                else
                {
                    uc_atg.lstget.Items.Add(" --> " + mvars.msrColor[svc + 1] + "20/10(" + sv20d10[0, svc] / sv20d10[1, svc] + ")>" + mvars.UUT.ex20d10[svc] + "(Spec)");
                    mp.funSaveLogs(mvars.msrColor[svc + 1] + "20/10(" + sv20d10[0, svc] / sv20d10[1, svc] + ")>" + mvars.UUT.ex20d10[svc] + "(Spec)");
                }
            }
            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
            #endregion 20 divide 10 measurement



            #region UUTID.lxy file
            if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\"); }
            FileInfo copyFile = new FileInfo(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy");
            if (copyFile.Exists) { copyFile.Delete(); }
            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy")) { File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy"); }
            StreamWriter sTAwrite = File.CreateText(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy");
            svs = mvars.UUT.ID + ",duty," + mvars.duty[mvars.dualduty] + ",DLvlimit," + uc_atg.lbldLvlimit.Text + "~" + uc_atg.lbldLvmax.Text + ",L0ADJ," + uc_atg.lblL0ADJ.Text;
            for (svc = 1; svc <= 1; svc++)
            {
                svs = mvars.msrColor[svc].Substring(0, 1) + "Lv";
                for (svg = 0; svg <= gmax; svg++)
                {
                    svs += "," + uc_atg.dgvatg.Rows[1].Cells[svg + svDs].Value;
                }
                sTAwrite.WriteLine(svs);
                svs = mvars.msrColor[svc].Substring(0, 1) + "x";
                for (svg = 0; svg <= gmax; svg++)
                {
                    svs += "," + uc_atg.dgvatg.Rows[2].Cells[svg + svDs].Value;
                }
                sTAwrite.WriteLine(svs);
                svs = mvars.msrColor[svc].Substring(0, 1) + "y";
                for (svg = 0; svg <= gmax; svg++)
                {
                    svs += "," + uc_atg.dgvatg.Rows[3].Cells[svg + svDs].Value;
                }
                sTAwrite.WriteLine(svs);
            }
            sTAwrite.Flush();
            sTAwrite.Close();
            #endregion UUTID.lxy file



            mvars.UUT.MTP = svMTP;
            #region MTP Vref5.5v(Form1.pvindex = 0x1F)，TFTVREF2.5v(Form1.pvindex = 0x12)  Gamma_603()
            if (mvars.UUT.MTP == 1 && mvars.demoMode == false)
            {
                if (mvars.deviceNameSub == "B(4t)" || mvars.deviceNameSub == "B(4)" || mvars.deviceID.Substring(0, 2) == "05")
                {
                    uc_atg.lstget.Items.Add(" -->  Vref" + mvars.cm603Vref[0] + "," + mvars.cm603Vref[1] + "," + mvars.cm603Vref[2] + "," + mvars.cm603Vref[3] + " @ MTP," + mvars.UUT.MTP);
                }
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                if (mp.Vref_603(sv603amount)) { mvars.ATGerr = "-14"; uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  GammaSet_Vref fail"); }
                uc_atg.lstget.Items.Add(" -->  TFTVREF" + mvars.UUT.VREF + " @ MTP," + mvars.UUT.MTP);
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                if (mp.TFTVREF_603BK1VCOM(sv603amount)) { mvars.ATGerr = "-14"; uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  GammaSet_TFTVREF fail"); }

                if (mvars.ATGerr == "0")
                {
                    if (mvars.deviceID.Substring(0, 2) == "05")
                    {
                        mp.Gamma_603(sv603amount);
                        for (svi = 0; svi <= 3; svi++)
                        {
                            if (svi == 0) { mvars.cm603WRaddr = 0xD5; Form1.cmbCM603.SelectedIndex = svi; }
                            else if (svi == 1) { mvars.cm603WRaddr = 0xD7; Form1.cmbCM603.SelectedIndex = svi; }
                            else if (svi == 2) { mvars.cm603WRaddr = 0xDB; Form1.cmbCM603.SelectedIndex = svi; }
                            else if (svi == 3) { mvars.cm603WRaddr = 0xD9; Form1.cmbCM603.SelectedIndex = svi; }
                            mvars.lblCmd = "CM603_READ_" + mvars.cm603WRaddr;
                            mp.mhcm603ReadAll(mvars.cm603WRaddr);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mvars.ATGerr = "-18"; }
                            else
                            {
                                int svbinI;
                                string svbinS;
                                svs = ""; svc = 0;
                                int svj;
                                if (svi == 3)
                                {
                                    for (svj = 0x02 * 2 + mvars.dualduty * 60; svj <= 0x04 * 2 + mvars.dualduty * 60; svj += 2)
                                    {
                                        if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                        {
                                            svc++;
                                            svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                        }
                                    }
                                    if (mvars.dualduty == 0) svj = 0x12 * 2; else svj = 0x30 * 2;
                                    if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                    {
                                        svc++;
                                        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                    }
                                    svj = 0x1F * 2;
                                    if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                    {
                                        svc++;
                                        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                    }
                                }
                                else
                                {
                                    for (svj = 0x02 * 2 + mvars.dualduty * 60; svj <= 0x0D * 2 + mvars.dualduty * 60; svj += 2)
                                    {
                                        if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                        {
                                            svc++;
                                            svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                        }
                                    }
                                    if (mvars.dualduty == 0) svj = 0x12 * 2; else svj = 0x30 * 2;
                                    if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                    {
                                        svc++;
                                        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                    }
                                    svj = 0x1F * 2;
                                    if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                    {
                                        svc++;
                                        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                    }
                                }

                                if (svs != "") { mvars.ATGerr = "-20"; }
                                else
                                {
                                    Array.Copy(mvars.RegData, svi * mvars.RegData.GetLength(1), mvars.cm603df, svi * mvars.RegData.GetLength(1), mvars.RegData.GetLength(1));
                                    for (svj = 0; svj < mvars.cm603df.GetLength(1); svj++)  /// 128
                                    {
                                        mvars.cm603dfB[svi, svj] = (byte)mp.HexToDec(mvars.cm603df[svi, svj]);
                                    }
                                    if (svi == 3)
                                    {
                                        svbinI = mvars.cm603dfB[svi, 4] * 256 + mvars.cm603dfB[svi, 5];
                                        svbinS = mp.DecToBin(svbinI, 16);
                                        mvars.pGMA[mvars.dualduty].Data[0, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.4=pGMA.0(gray0)
                                        mvars.pGMA[mvars.dualduty].Data[0, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.5=pGMA.1(gray0)

                                        svbinI = mvars.cm603dfB[svi, 6] * 256 + mvars.cm603dfB[svi, 7];
                                        svbinS = mp.DecToBin(svbinI, 16);
                                        mvars.pGMA[mvars.dualduty].Data[1, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.6=pGMA.0(gray0)
                                        mvars.pGMA[mvars.dualduty].Data[1, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.7=pGMA.1(gray0)

                                        svbinI = mvars.cm603dfB[svi, 8] * 256 + mvars.cm603dfB[svi, 9];
                                        svbinS = mp.DecToBin(svbinI, 16);
                                        mvars.pGMA[mvars.dualduty].Data[2, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.8=pGMA.0(gray0)
                                        mvars.pGMA[mvars.dualduty].Data[2, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.9=pGMA.1(gray0)
                                    }
                                    else
                                    {
                                        for (svj = 4 + 60 * mvars.dualduty; svj <= 26 + 60 * mvars.dualduty; svj += 2)
                                        {
                                            svbinI = mvars.cm603dfB[svi, svj] * 256 + mvars.cm603dfB[svi, svj + 1];
                                            svbinS = mp.DecToBin(svbinI, 16);
                                            mvars.pGMA[mvars.dualduty].Data[svi, 28 - svj % 60] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);      /// 603.29-4-1=pGMA.24(gray255)，603.29-26-1=pGMA.2(gray1)
                                            mvars.pGMA[mvars.dualduty].Data[svi, 29 - svj % 60] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      /// 603.29-4  =pGMA.25(gray255)，603.29-26  =pGMA.3(gray1)
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (mvars.UUT.MTP == 0)
            {
                if (mvars.ATGerr == "0")        //20230530 修正 MTP=0 ATGerr=-20" (新增條件式)
                {
                    if (mvars.deviceID.Substring(0, 2) == "05")
                    {
                        mp.Gamma_603(sv603amount);  //20230530 修正 MTP=0 ATGerr=-20" (新增執行序)
                        for (svi = 0; svi <= 3; svi++)
                        {
                            if (svi == 0) { mvars.cm603WRaddr = 0xD5; Form1.cmbCM603.SelectedIndex = svi; }
                            else if (svi == 1) { mvars.cm603WRaddr = 0xD7; Form1.cmbCM603.SelectedIndex = svi; }
                            else if (svi == 2) { mvars.cm603WRaddr = 0xDB; Form1.cmbCM603.SelectedIndex = svi; }
                            else if (svi == 3) { mvars.cm603WRaddr = 0xD9; Form1.cmbCM603.SelectedIndex = svi; }
                            mvars.lblCmd = "CM603_READ_" + mvars.cm603WRaddr;
                            mp.mhcm603ReadAll(mvars.cm603WRaddr);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mvars.ATGerr = "-18"; }
                            else
                            {

                                int svbinI;
                                string svbinS;
                                svs = ""; svc = 0;
                                int svj;
                                if (svi == 3)
                                {
                                    for (svj = 0x02 * 2 + mvars.dualduty * 60; svj <= 0x04 * 2 + mvars.dualduty * 60; svj += 2)
                                    {
                                        if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                        {
                                            svc++;
                                            svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                        }
                                    }
                                    if (mvars.dualduty == 0) svj = 0x12 * 2; else svj = 0x30 * 2;
                                    if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                    {
                                        svc++;
                                        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                    }
                                    svj = 0x1F * 2;
                                    if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                    {
                                        svc++;
                                        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                    }
                                }
                                else
                                {
                                    for (svj = 0x02 * 2 + mvars.dualduty * 60; svj <= 0x0D * 2 + mvars.dualduty * 60; svj += 2)
                                    {
                                        if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                        {
                                            svc++;
                                            svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                        }
                                    }
                                    if (mvars.dualduty == 0) svj = 0x12 * 2; else svj = 0x30 * 2;
                                    if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                    {
                                        svc++;
                                        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                    }
                                    svj = 0x1F * 2;
                                    if ((mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1]) != (mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1]))
                                    {
                                        svc++;
                                        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj] + mvars.cm603df[svi, svj + 1] + "~Rd 0x" + mvars.RegData[svi, svj] + mvars.RegData[svi, svj + 1];
                                    }
                                }

                                if (svs != "") { mvars.ATGerr = "-20"; }
                                else
                                {
                                    Array.Copy(mvars.RegData, svi * mvars.RegData.GetLength(1), mvars.cm603df, svi * mvars.RegData.GetLength(1), mvars.RegData.GetLength(1));
                                    for (svj = 0; svj < mvars.cm603df.GetLength(1); svj++)  /// 128
                                    {
                                        mvars.cm603dfB[svi, svj] = (byte)mp.HexToDec(mvars.cm603df[svi, svj]);
                                    }
                                    if (svi == 3)
                                    {
                                        svbinI = mvars.cm603dfB[svi, 4] * 256 + mvars.cm603dfB[svi, 5];
                                        svbinS = mp.DecToBin(svbinI, 16);
                                        mvars.pGMA[mvars.dualduty].Data[0, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.4=pGMA.0(gray0)
                                        mvars.pGMA[mvars.dualduty].Data[0, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.5=pGMA.1(gray0)

                                        svbinI = mvars.cm603dfB[svi, 6] * 256 + mvars.cm603dfB[svi, 7];
                                        svbinS = mp.DecToBin(svbinI, 16);
                                        mvars.pGMA[mvars.dualduty].Data[1, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.6=pGMA.0(gray0)
                                        mvars.pGMA[mvars.dualduty].Data[1, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.7=pGMA.1(gray0)

                                        svbinI = mvars.cm603dfB[svi, 8] * 256 + mvars.cm603dfB[svi, 9];
                                        svbinS = mp.DecToBin(svbinI, 16);
                                        mvars.pGMA[mvars.dualduty].Data[2, 0] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);    /// 603.8=pGMA.0(gray0)
                                        mvars.pGMA[mvars.dualduty].Data[2, 1] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);    /// 603.9=pGMA.1(gray0)
                                    }
                                    else
                                    {
                                        for (svj = 4 + 60 * mvars.dualduty; svj <= 26 + 60 * mvars.dualduty; svj += 2)
                                        {
                                            svbinI = mvars.cm603dfB[svi, svj] * 256 + mvars.cm603dfB[svi, svj + 1];
                                            svbinS = mp.DecToBin(svbinI, 16);
                                            mvars.pGMA[mvars.dualduty].Data[svi, 28 - svj % 60] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);      /// 603.29-4-1=pGMA.24(gray255)，603.29-26-1=pGMA.2(gray1)
                                            mvars.pGMA[mvars.dualduty].Data[svi, 29 - svj % 60] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);      /// 603.29-4  =pGMA.25(gray255)，603.29-26  =pGMA.3(gray1)
                                        }
                                    }
                                }
                                //for (int svj = 5; svj <= 27; svj += 2)
                                //{
                                //    if ((mvars.cm603df[svi, svj - 1] + mvars.cm603df[svi, svj]) != (mvars.RegData[svi, svj - 1] + mvars.RegData[svi, svj]))
                                //    {
                                //        svc++;
                                //        svs = svs + "," + svi + "." + svj + "~Wt 0x" + mvars.cm603df[svi, svj - 1] + mvars.cm603df[svi, svj] + "~Rd 0x" + mvars.RegData[svi, svj - 1] + mvars.RegData[svi, svj];
                                //    }
                                //    mvars.cm603df[svi, svj - 1] = mvars.RegData[svi, svj - 1];
                                //    mvars.cm603df[svi, svj] = mvars.RegData[svi, svj];
                                //    mvars.cm603dfB[svi, svj] = (byte)mp.HexToDec(mvars.cm603df[svi, svj]);
                                //    mvars.cm603dfB[svi, svj - 1] = (byte)mp.HexToDec(mvars.cm603df[svi, svj - 1]);
                                //    svbinI = mvars.cm603dfB[svi, svj - 1] * 256 + mvars.cm603dfB[svi, svj];
                                //    svbinS = mp.DecToBin(svbinI, 16);
                                //    mvars.pGMA.Data[svi, 29 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(0, 2);   //29-5=24,29-7=22,,,29-27=2
                                //    mvars.pGMA.Data[svi, 30 - svj] = mp.BinToHex(svbinS.Substring(6, 10), 4).Substring(2, 2);   //30-5=25,30-7=23,,,30-27=3
                                //}
                                //if (svs != "") { mvars.ATGerr = "-20"; }
                            }
                        }
                    }
                }
            }
            #endregion MTP



            #region 暗態量測
            ShowAGMAnT("X", 0, 0, 0, true, 0, 0, 0, false, 500);
            CAmeasF();
            int svn = 0;
            do
            {
                CAmeasF();
                uc_atg.lstget.Items.Add(" --> DLv " + CAFxLv);
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                doDelayms(50);
                if (CAFxLv <= mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus &&
                    CAFxLv >= mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)
                {
                    break;
                }
                svn++;
            }
            while (svn < 30);
            uc_atg.dgvatg.Rows[1].Cells[uc_atg.dgvatg.ColumnCount - mvars.cm603Gamma.Length].Value = CAFxLv.ToString("#0.0####");
            uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value = CAFxLv;
            uc_atg.dgvatg.Rows[2].Cells[0 + svDs].Value = CAFx;
            uc_atg.dgvatg.Rows[3].Cells[0 + svDs].Value = CAFy;
            uc_atg.dgvatg.Rows[5].Cells[0 + svDs].Value = string.Format("{0:0.0###}", Math.Abs(CAFx - mvars.UUT.Cx));
            uc_atg.dgvatg.Rows[6].Cells[0 + svDs].Value = string.Format("{0:0.0###}", Math.Abs(CAFy - mvars.UUT.Cy));
            #endregion 暗態量測



            #region .gma file save 
            /// if (mvars.ICver >= 5) mp.fileDefaultGammaAIO(true, mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma");
            /// else mp.fileDefaultGamma(true, mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma");


            mvars.defaultGammafile = mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma";
            if (mvars.ICver >= 5 || mvars.deviceID.Substring(0, 2) == "05") { mp.fileDefaultGammaV(true); }
            #endregion .gma file save



            #region 結束訊息顯示  (ATGerr == "0" 內含 msrgammacurve )
            if (mvars.ATGerr == "0")
            {
                uc_atg.lstget.Items.Add(" --> ATGerr(" + mvars.ATGerr + ")");
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

                if (mvars.demoMode)
                {
                    uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value = 0.01;
                    uc_atg.dgvatg.Rows[2].Cells[0 + svDs].Value = 0.26;
                    uc_atg.dgvatg.Rows[3].Cells[0 + svDs].Value = 0.24;
                    uc_atg.dgvatg.Rows[5].Cells[0 + svDs].Value = 0;
                    uc_atg.dgvatg.Rows[6].Cells[0 + svDs].Value = 0;
                    uc_atg.lstget.Items.Add(" ->  @ DLv,0.01,x0.26,y0.24,R0x" +
                        uc_atg.dgvatg.Rows[8].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[9].Cells[gmax + svDs].Value +
                        ",G0x" + uc_atg.dgvatg.Rows[10].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[11].Cells[gmax + svDs].Value +
                        ",B0x" + uc_atg.dgvatg.Rows[12].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[13].Cells[gmax + svDs].Value);
                }
                else mCAs.CAremote(true);

                ShowAGMAnT("X", 0, 0, 0, true, 255, 255, 255, false, 100);
                if (mvars.demoMode == false) CAmeasF();
                if (mvars.demoMode == false) CAmeasF();
                if (mvars.demoMode == false)
                {
                    uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value = CAFxLv;
                    uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value = CAFx;
                    uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value = CAFy;
                    uc_atg.dgvatg.Rows[5].Cells[gmax + svDs].Value = string.Format("{0:0.0##}", Math.Abs(CAFx - mvars.UUT.Cx));
                    uc_atg.dgvatg.Rows[6].Cells[gmax + svDs].Value = string.Format("{0:0.0##}", Math.Abs(CAFy - mvars.UUT.Cy));
                    if (Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[gmax + svDs].Value) > 0)
                    {
                        uc_atg.dgvatg.Rows[4].Cells[gmax + svDs].Value = Math.Abs(100 * (1 - (CAFxLv / Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[gmax + svDs].Value)))).ToString("#0.0");
                    }
                }
                else
                {
                    uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value = 805.3;
                    uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value = 0.285;
                    uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value = 0.295;
                    uc_atg.dgvatg.Rows[5].Cells[gmax + svDs].Value = 0;
                    uc_atg.dgvatg.Rows[6].Cells[gmax + svDs].Value = 0;
                    uc_atg.lstget.Items.Add(" ->  @ WLv,805,x0.285,y0.295,R0x" +
                        uc_atg.dgvatg.Rows[8].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[9].Cells[gmax + svDs].Value +
                        ",G0x" + uc_atg.dgvatg.Rows[10].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[11].Cells[gmax + svDs].Value +
                        ",B0x" + uc_atg.dgvatg.Rows[12].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[13].Cells[gmax + svDs].Value);
                }
                uc_atg.lstget.Items.Add(" --> Lv re-Check .... △Lv" + uc_atg.dgvatg.Rows[4].Cells[gmax + svDs].Value + "，△xy" + uc_atg.dgvatg.Rows[5].Cells[gmax + svDs].Value + "," + uc_atg.dgvatg.Rows[6].Cells[gmax + svDs].Value);
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

                if (mvars.demoMode == false)
                {
                    mCAs.CAremote(false);
                    if (mvars.sp1.IsOpen) { mvars.sp1.Close(); }
                }

                /// measurement gammacurve↓
                if (mvars.TuningArea.Mark != "pg")
                {
                    if (Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[gmax + svDs].Value) <= 0.003 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[gmax + svDs].Value) <= 0.003 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[gmax + svDs].Value) <= 5 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value) >= (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus) &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value) <= (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus))
                    {
                        #region msrgammacurve  由 特調32灰階   downG32 之後 改到這裡
                        if (mvars.msrgammacurve)
                        {
                            svt1 = DateTime.Now;
                            mCAs.CAremote(true);
                            for (svc = 1; svc <= 4; svc++)
                            {
                                uc_atg.lstget.Items.Add(" -> " + mvars.TuningArea.Mark + "，msrGammaCurve " + mvars.msrColor[svc].Substring(0, 1) + " G" + mvars.msrgammacurveSt + " to G" + mvars.msrgammacurveEd + " Step" + mvars.msrgammacurveGp);
                                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                                for (svg = mvars.msrgammacurveSt; svg <= mvars.msrgammacurveEd; svg += mvars.msrgammacurveGp)
                                {
                                    if (svc > 1 && svg == 0)
                                    {
                                        SvFullGrayMsr[svc, svg].Lv = SvFullGrayMsr[1, svg].Lv;
                                        SvFullGrayMsr[svc, svg].Sx = SvFullGrayMsr[1, svg].Sx;
                                        SvFullGrayMsr[svc, svg].Sy = SvFullGrayMsr[1, svg].Sy;
                                    }
                                    else
                                    {
                                        if (svg <= 40)
                                        {
                                            if (svc == 1) { ShowAGMAnT("X", 0, 0, 0, true, svg, svg, svg, false, 500); }
                                            else if (svc == 2) { ShowAGMAnT("X", 0, 0, 0, true, svg, 0, 0, false, 500); }
                                            else if (svc == 3) { ShowAGMAnT("X", 0, 0, 0, true, 0, svg, 0, false, 500); }
                                            else if (svc == 4) { ShowAGMAnT("X", 0, 0, 0, true, 0, 0, svg, false, 500); }
                                        }
                                        else
                                        {
                                            if (svc == 1) { ShowAGMAnT("X", 0, 0, 0, true, svg, svg, svg, false, 200); }
                                            else if (svc == 2) { ShowAGMAnT("X", 0, 0, 0, true, svg, 0, 0, false, 200); }
                                            else if (svc == 3) { ShowAGMAnT("X", 0, 0, 0, true, 0, svg, 0, false, 200); }
                                            else if (svc == 4) { ShowAGMAnT("X", 0, 0, 0, true, 0, 0, svg, false, 200); }
                                        }
                                        CAmeasF();
                                        SvFullGrayMsr[svc, svg].Lv = -1; SvFullGrayMsr[svc, svg].Sx = 0; SvFullGrayMsr[svc, svg].Sy = 0;
                                        SvFullGrayMsr[svc, svg].Lv = CAFxLv;
                                        SvFullGrayMsr[svc, svg].Sx = CAFx;
                                        SvFullGrayMsr[svc, svg].Sy = CAFy;
                                    }
                                    uc_atg.lstget.Items.Add(" -> " + mvars.TuningArea.Mark + "，gammacurve " + mvars.msrColor[svc].Substring(0, 1) + svg.ToString("000") + " " + SvFullGrayMsr[svc, svg].Lv + "," + SvFullGrayMsr[svc, svg].Sx + "," + SvFullGrayMsr[svc, svg].Sy);
                                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                                    if (mvars.Break) { mvars.ATGerr = "-1"; goto ExNovaAGMA; }
                                }
                            }
                            if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\"); }

                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma")) { File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma"); }
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma"))
                            {
                                svs = "△min " + mvars.deltaMin + "=|" + mvars.deltaMinXYZ[0] + "|+|" + mvars.deltaMinXYZ[1] + "|+|" + mvars.deltaMinXYZ[2] + "| Cnt " + mvars.deltaMinAt;
                                file.WriteLine(svs);
                                svs = "Gray";
                                for (svg = 0; svg <= gmax; svg++)
                                {
                                    svs += "," + mvars.cm603Gamma[svg];
                                }
                                file.WriteLine(svs);
                                for (svc = 2; svc <= 4; svc++)
                                {
                                    svs = mvars.msrColor[svc].Substring(0, 1) + " 0x";
                                    for (svg = 0; svg <= gmax; svg++)
                                    {
                                        svs += "," + uc_atg.dgvatg.Rows[8 + (svc - 2) * 2].Cells[svg].Value + uc_atg.dgvatg.Rows[8 + (svc - 2) * 2 + 1].Cells[svg].Value;
                                    }
                                    file.WriteLine(svs);
                                }
                            }
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.csv")) { File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.csv"); }
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.csv"))
                            {
                                svs = "Vref,R," + mvars.cm603Vref[0] + ",,G," + mvars.cm603Vref[1] + ",,B," + mvars.cm603Vref[2];
                                file.WriteLine(svs);
                                svs = "Gray0Vol0," + mvars.deltaG0V0;
                                file.WriteLine(svs);
                                svs = "Gray,WLv,Wx,Wy,RLv,Rx,Ry,GLv,Gx,Gy,BLv,Bx,By";
                                file.WriteLine(svs);
                                for (svg = mvars.msrgammacurveSt; svg <= mvars.msrgammacurveEd; svg += mvars.msrgammacurveGp)
                                {
                                    svs = svg.ToString();
                                    for (svc = 1; svc <= 4; svc++)
                                    {
                                        svs += "," + SvFullGrayMsr[svc, svg].Lv + "," + SvFullGrayMsr[svc, svg].Sx + "," + SvFullGrayMsr[svc, svg].Sy;
                                    }
                                    file.WriteLine(svs);
                                }
                            }
                            ///uc_atg.lstget.Items.Add(" >> msrGray " + ((DateTime.Now - svt1).Seconds) + "s"); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                            ///uc_atg.lblctime.Text += " >> msrGray " + ((DateTime.Now - svt1).Seconds) + "s";
                        }
                        #endregion msrgammacurve


                        ShowAGMAnT("X", 0, 0, 0, true, 0, 0, 0, false, 500);
                        if (mvars.FormShow[6] == false) { i3init = new i3_Init(); i3init.Show(); i3init.Location = new System.Drawing.Point(0, 0); }
                        i3_Init.btn_0.Visible = false;
                        i3_Init.btn_1.Enabled = true; i3_Init.btn_1.Location = new System.Drawing.Point(275, 214);
                        i3_Init.btn_1.Text = "OK";
                        i3_Init.btn_1.Focus();
                        i3_Init.lbl_1.Visible = true;


                        Form2.i3pat.lbl_Mark.ForeColor = System.Drawing.Color.Blue;
                        Form2.i3pat.lbl_Mark.Text = "OK    OK    OK    OK" + "\r\n" + "\r\n" + "    OK    OK    OK" + "\r\n" + "\r\n" + "OK    OK    OK    OK" + "\r\n" + "\r\n" + "    OK    OK    OK";

                        Form2.i3pat.lbl_Mark.Text = "OK ★ " + mvars.UUT.ID + "\r\n" +
                            "W" + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + " x" + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + " y" + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + " D" + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value + "\r\n" +
                            "v" + mvars.UImajor + "," + mvars.deviceID + "," + mvars.verMCU + "," + mvars.verFPGA + "," + uc_atg.lblDUTY.Text;

                        Application.DoEvents();
                        //1126
                        if (mvars.flgDirMTP == false)
                        {
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\"); }
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy"); }
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy"))
                            {
                                File.Copy(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy", mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy");
                            }
                            else { funSaveLogs(" " + DateTime.Now + " (" + mvars.ATGerr + ")   ID: Can't find " + svUUTIDtemp + mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy file"); }



                            //1214
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\version.gma")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\version.gma"); }
                            sTAwrite = File.CreateText(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\version.gma");
                            svs = mvars.strUInameMe + "_UI v" + mvars.UImajor;
                            sTAwrite.WriteLine(svs);
                            sTAwrite.Flush();
                            sTAwrite.Close();




                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\"); }
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_OK.gma")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_OK.gma"); }
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma"))
                            {
                                File.Copy(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma", mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_OK.gma");
                            }
                            else { funSaveLogs(" " + DateTime.Now + "    ID: Can't find " + svUUTIDtemp + mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma file"); }
                        }
                        //1214 延續1.2.0.3版暫時修改 0.006~0.014
                        svs = "\r\n" + "\r\n" +
                            "       WLv: " + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + " >= " + (mvars.UUT.CLv * mvars.UUT.WTLvBet * 0.98).ToString("###0.#") + "(" + mvars.UUT.CLv + "x0.98)" + "\r\n" +
                            "       Wx: " + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + " <= " + mvars.UUT.Cx + " ± 0.003" + "\r\n" +
                            "       Wy: " + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + " <= " + mvars.UUT.Cy + " ± 0.003" + "\r\n" +
                            "       DLv: " + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value + " between " +
                                    string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)) +
                            " ~ " + string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + "\r\n" +
                            "         log saved !!";

                        if (mvars.UUT.MTP == 1)
                        {
                            mp.funSaveLogs("MTP_OK   ID: " + mvars.UUT.ID + "  result: " + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "cd/m^2," +
                                uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "," + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value);
                            uc_atg.lstget.Items.Add(" --> MTP .... OK"); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                            svs += "  MTP OK";
                            i3_Init.lbl_1.Text = svs;
                        }
                        else
                        {
                            mp.funSaveLogs("DAC_only   ID: " + mvars.UUT.ID + "  result: " + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "cd/m^2," +
                                uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "," + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value);
                            uc_atg.lstget.Items.Add(" --> DAC .... OK"); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                            svs += "  DAC OK";
                            i3_Init.lbl_1.Text = svs;
                        }

                        uc_atg.dgvatg.Rows[0].Cells[0 + svDs].Value = "OK";
                    }
                    else
                    {
                        ShowAGMAnT("X", 0, 0, 0, true, 0, 0, 0, false, 500);
                        if (mvars.FormShow[6] == false) { i3init = new i3_Init(); i3init.Show(); i3init.Location = new System.Drawing.Point(0, 0); }
                        i3_Init.btn_0.Visible = false;
                        i3_Init.btn_1.Enabled = true; i3_Init.btn_1.Location = new System.Drawing.Point(275, 214);
                        i3_Init.btn_1.Text = "OK";
                        i3_Init.btn_1.Focus();
                        i3_Init.lbl_1.Visible = true;


                        mvars.ATGerr = "-7";

                        Form2.i3pat.lbl_Mark.ForeColor = System.Drawing.Color.Red;
                        Form2.i3pat.lbl_Mark.Text = "NG    NG    NG    NG" + "\r\n" + "\r\n" + "    NG    NG    NG" + "\r\n" + "\r\n" + "NG    NG    NG    NG" + "\r\n" + "\r\n" + "    NG    NG    NG";

                        Form2.i3pat.lbl_Mark.Text = "NG ★ " + mvars.UUT.ID + "\r\n" +
                            "W" + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + " x" + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + " y" + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + " D" + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value + "\r\n" +
                            "v" + mvars.UImajor + "," + mvars.deviceID + "," + mvars.verMCU + "," + mvars.verFPGA + "," + uc_atg.lblDUTY.Text;

                        Application.DoEvents();

                        //1126
                        if (mvars.flgDirMTP == false)
                        {
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\"); }
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy"); }
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy"))
                            {
                                File.Copy(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy", mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy");
                                File.Copy(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy", mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy");
                                File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy");
                            }
                            else { funSaveLogs(" " + DateTime.Now + " (" + mvars.ATGerr + ")   ID: Can't find " + svUUTIDtemp + mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy file"); }

                        }
                        //1214 延續1.2.0.3版暫時修改 0.006~0.014
                        svs = "\r\n" + "\r\n" +
                            "       WLv: " + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "\r\n" +
                            "       Wx: " + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "\r\n" +
                            "       Wy: " + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + "\r\n" +
                            "       DLv: " + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value + " out of range " +
                            string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)) + " ~ " +
                            string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + "\r\n" +
                            "         log saved !!";

                        if (mvars.UUT.MTP == 1)
                        {
                            //1214 延續1.2.0.3版暫時修改 0.006~0.014
                            uc_atg.lstget.Items.Add(" --> MTP .... Fail code(" + mvars.ATGerr + ") ... out of spec.(spec. : WLv >= " + mvars.UUT.CLv * 0.98 +
                                " and Wdxdy ±0.003 and DLv between " +
                                string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)) + " ~ " +
                                string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + ")");

                            svs += "  MTP fail .... code(" + mvars.ATGerr + ") out of spec.";
                            i3_Init.lbl_1.Text = svs;
                        }
                        else
                        {
                            //1214 延續1.2.0.3版暫時修改 0.006~0.014                        
                            uc_atg.lstget.Items.Add(" --> DAC .... Fail code(" + mvars.ATGerr + ") ... out of spec.(spec. : WLv >= " + mvars.UUT.CLv * 0.98 +
                                " and Wdxdy ±0.003 and DLv between " +
                                string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)) + " ~ " +
                                string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + ")");
                            svs += "  DAC fail .... code(" + mvars.ATGerr + ") out of spec.";
                            i3_Init.lbl_1.Text = svs;
                        }
                        uc_atg.dgvatg.Rows[0].Cells[0 + svDs].Value = "NG";
                    }
                    //svMsg = "";

                    //svi
                    do
                    {
                        Application.DoEvents();
                        mp.doDelayms(100);
                    } while (i3init.Visible == true);
                    if (i3init != null) { i3init.Dispose(); }
                }
                /// measurement gammacurve↓
                else if (mvars.TuningArea.Mark == "pg")
                {
                    if (Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[gmax + svDs].Value) <= 0.003 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[gmax + svDs].Value) <= 0.003 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[gmax + svDs].Value) <= 5 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value) >= mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value) <= mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)
                    {
                        uc_atg.lstget.Items.Add("OK ★ " + mvars.UUT.ID + "W" + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + " x" + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + " y" + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + " D" + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value);

                        if (mvars.flgDirMTP == false)
                        {
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\"); }
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy"); }
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + mvars.UUT.ID + @"\" + mvars.UUT.ID + ".lxy"))
                            {
                                File.Copy(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy", mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy");
                            }
                            else { funSaveLogs(" " + DateTime.Now + " (" + mvars.ATGerr + ")   ID: Can't find " + svUUTIDtemp + mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy file"); }



                            //1214
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\version.gma")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\version.gma"); }
                            sTAwrite = File.CreateText(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\version.gma");
                            svs = mvars.strUInameMe + "_UI v" + mvars.UImajor;
                            sTAwrite.WriteLine(svs);
                            sTAwrite.Flush();
                            sTAwrite.Close();




                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\"); }
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_OK.gma")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_OK.gma"); }
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma"))
                            {
                                File.Copy(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma", mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_OK.gma");
                            }
                            else { funSaveLogs(" " + DateTime.Now + "    ID: Can't find " + svUUTIDtemp + mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".gma file"); }
                        }
                        //1214 延續1.2.0.3版暫時修改 0.006~0.014
                        svs = "\r\n" + "\r\n" +
                            "       WLv: " + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + " >= " + (mvars.UUT.CLv * mvars.UUT.WTLvBet * 0.98).ToString("###0.#") + "(" + mvars.UUT.CLv + "x0.98)" + "\r\n" +
                            "       Wx: " + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + " <= " + mvars.UUT.Cx + " ± 0.003" + "\r\n" +
                            "       Wy: " + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + " <= " + mvars.UUT.Cy + " ± 0.003" + "\r\n" +
                            "       DLv: " + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value + " between " +
                                    string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)) +
                            " ~ " + string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + "\r\n" +
                            "         log saved !!";

                        if (mvars.UUT.MTP == 1)
                        {
                            mp.funSaveLogs("MTP_OK   ID: " + mvars.UUT.ID + "  result: " + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "cd/m^2," +
                                uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "," + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value);
                            uc_atg.lstget.Items.Add(" --> MTP .... OK"); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                        }
                        else
                        {
                            mp.funSaveLogs("DAC_only   ID: " + mvars.UUT.ID + "  result: " + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "cd/m^2," +
                                uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "," + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value);
                            uc_atg.lstget.Items.Add(" --> DAC .... OK"); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                        }


                        #region msrgammacurve   由 特調32灰階   downG32 之後 改到這裡
                        if (mvars.msrgammacurve)
                        {
                            mCAs.CAremote(true);
                            for (svc = 1; svc <= 4; svc++)
                            {
                                uc_atg.lstget.Items.Add(" -> " + mvars.TuningArea.Mark + "，msrGammaCurve " + mvars.msrColor[svc].Substring(0, 1) + " G" + mvars.msrgammacurveSt + " to G" + mvars.msrgammacurveEd + " Step" + mvars.msrgammacurveGp);
                                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                                for (svg = mvars.msrgammacurveSt; svg <= mvars.msrgammacurveEd; svg += mvars.msrgammacurveGp)
                                {
                                    if (svc > 1 && svg == 0)
                                    {
                                        SvFullGrayMsr[svc, svg].Lv = SvFullGrayMsr[1, svg].Lv;
                                        SvFullGrayMsr[svc, svg].Sx = SvFullGrayMsr[1, svg].Sx;
                                        SvFullGrayMsr[svc, svg].Sy = SvFullGrayMsr[1, svg].Sy;
                                    }
                                    else
                                    {
                                        if (svg <= 40)
                                        {
                                            if (svc == 1) { ShowAGMAnT("X", 0, 0, 0, true, svg, svg, svg, false, 500); }
                                            else if (svc == 2) { ShowAGMAnT("X", 0, 0, 0, true, svg, 0, 0, false, 500); }
                                            else if (svc == 3) { ShowAGMAnT("X", 0, 0, 0, true, 0, svg, 0, false, 500); }
                                            else if (svc == 4) { ShowAGMAnT("X", 0, 0, 0, true, 0, 0, svg, false, 500); }
                                        }
                                        else
                                        {
                                            if (svc == 1) { ShowAGMAnT("X", 0, 0, 0, true, svg, svg, svg, false, 200); }
                                            else if (svc == 2) { ShowAGMAnT("X", 0, 0, 0, true, svg, 0, 0, false, 200); }
                                            else if (svc == 3) { ShowAGMAnT("X", 0, 0, 0, true, 0, svg, 0, false, 200); }
                                            else if (svc == 4) { ShowAGMAnT("X", 0, 0, 0, true, 0, 0, svg, false, 200); }
                                        }
                                        CAmeasF();
                                        SvFullGrayMsr[svc, svg].Lv = -1; SvFullGrayMsr[svc, svg].Sx = 0; SvFullGrayMsr[svc, svg].Sy = 0;
                                        SvFullGrayMsr[svc, svg].Lv = CAFxLv;
                                        SvFullGrayMsr[svc, svg].Sx = CAFx;
                                        SvFullGrayMsr[svc, svg].Sy = CAFy;
                                    }
                                    uc_atg.lstget.Items.Add(" -> " + mvars.TuningArea.Mark + "，gammacurve  " + mvars.msrColor[svc].Substring(0, 1) + svg.ToString("000") + " " + SvFullGrayMsr[svc, svg].Lv + "," + SvFullGrayMsr[svc, svg].Sx + "," + SvFullGrayMsr[svc, svg].Sy);
                                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                                    if (mvars.Break) { mvars.ATGerr = "-1"; goto ExNovaAGMA; }
                                }
                            }
                            if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\"); }

                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma")) { File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma"); }
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma"))
                            {
                                svs = "△min " + mvars.deltaMin + "=|" + mvars.deltaMinXYZ[0] + "|+|" + mvars.deltaMinXYZ[1] + "|+|" + mvars.deltaMinXYZ[2] + "| Cnt " + mvars.deltaMinAt;
                                file.WriteLine(svs);
                                svs = "Gray";
                                for (svg = 0; svg <= gmax; svg++)
                                {
                                    svs += "," + mvars.cm603Gamma[svg];
                                }
                                file.WriteLine(svs);
                                for (svc = 2; svc <= 4; svc++)
                                {
                                    svs = mvars.msrColor[svc].Substring(0, 1) + " 0x";
                                    for (svg = 0; svg <= gmax; svg++)
                                    {
                                        svs += "," + uc_atg.dgvatg.Rows[8 + (svc - 2) * 2].Cells[svg].Value + uc_atg.dgvatg.Rows[8 + (svc - 2) * 2 + 1].Cells[svg].Value;
                                    }
                                    file.WriteLine(svs);
                                }
                            }
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + mvars.UUT.ID + @"\" + mvars.UUT.ID + "_msrGammaCurve.csv")) { File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + mvars.UUT.ID + @"\" + mvars.UUT.ID + "_msrGammaCurve.csv"); }
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(mvars.strStartUpPath + @"\Parameter\Gamma\" + mvars.UUT.ID + @"\" + mvars.UUT.ID + "_msrGammaCurve.csv"))
                            {
                                svs = "Vref,R," + mvars.cm603Vref[0] + ",,G," + mvars.cm603Vref[1] + ",,B," + mvars.cm603Vref[2];
                                file.WriteLine(svs);
                                svs = "Gray0Vol0," + mvars.deltaG0V0;
                                file.WriteLine(svs);
                                svs = "Gray,WLv,Wx,Wy,RLv,Rx,Ry,GLv,Gx,Gy,BLv,Bx,By";
                                file.WriteLine(svs);
                                for (svg = mvars.msrgammacurveSt; svg <= mvars.msrgammacurveEd; svg += mvars.msrgammacurveGp)
                                {
                                    svs = svg.ToString();
                                    for (svc = 1; svc <= 4; svc++)
                                    {
                                        svs += "," + SvFullGrayMsr[svc, svg].Lv + "," + SvFullGrayMsr[svc, svg].Sx + "," + SvFullGrayMsr[svc, svg].Sy;
                                    }
                                    file.WriteLine(svs);
                                }
                            }
                            ///uc_atg.lstget.Items.Add(" >> msrGray " + ((DateTime.Now - svt1).Seconds) + "s"); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                        }
                        #endregion msrgammacurve

                    }
                    else
                    {
                        mvars.ATGerr = "-7";
                        uc_atg.lstget.Items.Add("NG ★ " + mvars.UUT.ID + "W" + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "x" + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "y" + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + "D" + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value);

                        if (mvars.flgDirMTP == false)
                        {
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\"); }
                            if (Directory.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_ok.lxy"); }
                            if (File.Exists(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy")) { File.Delete(mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy"); }
                            if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy"))
                            {
                                File.Copy(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy", mvars.UUT.gmafilepath + @"\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy");
                                File.Copy(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy", mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_fail.lxy");
                                File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy");
                            }
                            else { funSaveLogs(" " + DateTime.Now + " (" + mvars.ATGerr + ")   ID: Can't find " + svUUTIDtemp + mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + ".lxy file"); }

                        }
                        //1214 延續1.2.0.3版暫時修改 0.006~0.014
                        svs = "\r\n" + "\r\n" +
                            "       WLv: " + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "\r\n" +
                            "       Wx: " + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "\r\n" +
                            "       Wy: " + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + "\r\n" +
                            "       DLv: " + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value + " out of range " +
                            string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)) + " ~ " +
                            string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + "\r\n" +
                            "         log saved !!";

                        if (mvars.UUT.MTP == 1)
                        {
                            //1214 延續1.2.0.3版暫時修改 0.006~0.014
                            uc_atg.lstget.Items.Add(" --> MTP .... Fail code(" + mvars.ATGerr + ") ... out of spec.(spec. : WLv >= " + mvars.UUT.CLv * 0.98 +
                                " and Wdxdy ±0.003 and DLv between " +
                                string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)) + " ~ " +
                                string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + ")");

                            //svs += "  MTP fail .... code(" + mvars.ATGerr + ") out of spec.";
                            //i3_Init.lbl_1.Text = svs;
                        }
                        else
                        {
                            //1214 延續1.2.0.3版暫時修改 0.006~0.014                        
                            uc_atg.lstget.Items.Add(" --> DAC .... Fail code(" + mvars.ATGerr + ") ... out of spec.(spec. : WLv >= " + mvars.UUT.CLv * 0.98 +
                                " and Wdxdy ±0.003 and DLv between " +
                                string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus)) + " ~ " +
                                string.Format("{0:#0.00##}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + ")");
                            //svs += "  DAC fail .... code(" + mvars.ATGerr + ") out of spec.";
                            //i3_Init.lbl_1.Text = svs;
                        }
                    }
                }

                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                funSaveLogs(" " + DateTime.Now + " (ATG)   ID:" + mvars.UUT.ID + "," + i3_Init.lbl_1.Text);
            }
            else
            {
                if (svMTP == 1)
                {
                    uc_atg.lstget.Items.Add(" MTP NG   ID: " + mvars.UUT.ID + "  ErrorCode " + mvars.ATGerr);
                    if (mvars.ATGerr == "-20") mp.funSaveLogs("MTP_NG   ID: " + mvars.UUT.ID + "  GammaCompare result" + svs);
                }
                else if (svMTP == 0)
                {
                    uc_atg.lstget.Items.Add(" DAC NG   ID: " + mvars.UUT.ID + "  ErrorCode " + mvars.ATGerr);
                    if (mvars.ATGerr == "-20") mp.funSaveLogs("DAC_NG   ID: " + mvars.UUT.ID + "  GammaCompare result" + svs);
                }
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                if (mvars.TuningArea.Mark != "pg")
                {
                    if (ShowAGMAnT("X", 0, 0, 0, true, 0, 0, 0, false, 500))
                    {
                        Form2.i3pat.lbl_Mark.ForeColor = System.Drawing.Color.Red;
                        Form2.i3pat.lbl_Mark.Text = "NG  NG  NG" + "\r\n" + "\r\n" + "    NG    NG" + "\r\n" + "\r\n" + "NG    NG    NG" + "\r\n" + "\r\n" + "    NG    NG";

                        Form2.i3pat.lbl_Mark.Text = "NG X " + mvars.UUT.ID + "\r\n" +
                           "ATG ErrorCode " + mvars.ATGerr + "   " + mvars.ATGerr + mvars.ATGerr + "   " + mvars.ATGerr + " Before re-Check" + "\r\n" +
                           "v" + mvars.UImajor + "," + mvars.deviceID + "," + mvars.verMCU + "," + mvars.verFPGA + "," + uc_atg.lblDUTY.Text;

                        Application.DoEvents();
                    }
                }
                else
                {
                    ShowAGMAnT("X", 0, 0, 0, true, 255, 255, 255, false, 100);
                }
                if (mvars.FormShow[6] == false) { i3init = new i3_Init(); i3init.Show(); i3init.Location = new System.Drawing.Point(0, 0); }
                i3_Init.btn_0.Visible = false;
                i3_Init.btn_1.Enabled = true; i3_Init.btn_1.Location = new System.Drawing.Point(275, 214); i3_Init.btn_1.Text = "OK";
                i3_Init.btn_1.Text = "OK";
                i3_Init.btn_1.Focus();
                i3_Init.lbl_1.Visible = true;
                if (svMTP == 1)
                {
                    uc_atg.lstget.Items.Add(" --> MTP .... error，Errcode(" + mvars.ATGerr + ")");
                    i3_Init.lbl_1.Text = "MTP .... error " + "\r\n" + "  Errcode(" + mvars.ATGerr + ")";
                }
                else
                {
                    uc_atg.lstget.Items.Add(" --> DAC .... error，Errcode(" + mvars.ATGerr + ")");
                    i3_Init.lbl_1.Text = "DAC .... error " + "\r\n" + "  Errcode(" + mvars.ATGerr + ")";
                }
                if (mvars.ATGerr.IndexOf("-26", 0) != -1)
                {
                    if (mvars.ATGerr.IndexOf(".1", 0) != -1) { i3_Init.lbl_1.Text += "\r\n" + "g20/g10(" + sv20d10[0, 0] / sv20d10[1, 0] + ")<" + mvars.UUT.ex20d10[0] + "(Spec)"; }
                    else if (mvars.ATGerr.IndexOf(".2", 1) != -1) { i3_Init.lbl_1.Text += "\r\n" + "g20/g10(" + sv20d10[0, 1] / sv20d10[1, 1] + ")<" + mvars.UUT.ex20d10[1] + "(Spec)"; }
                    else if (mvars.ATGerr.IndexOf(".3", 1) != -1) { i3_Init.lbl_1.Text += "\r\n" + "g20/g10(" + sv20d10[0, 2] / sv20d10[1, 2] + ")<" + mvars.UUT.ex20d10[2] + "(Spec)"; }
                    else if (mvars.ATGerr.IndexOf(".4", 1) != -1) { i3_Init.lbl_1.Text += "\r\n" + "g20/g10(" + sv20d10[0, 3] / sv20d10[1, 3] + ")<" + mvars.UUT.ex20d10[3] + "(Spec)"; }
                }
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

                if (mvars.demoMode == false && mvars.sp1.IsOpen) mvars.sp1.Close();

                do
                {
                    Application.DoEvents();
                    doDelayms(100);
                } while (i3init.Visible == true);
                if (i3init != null) { i3init.Dispose(); }
            }   //ATGerr != "0"

        #endregion 結束訊息顯示


        dirmsrGamma:
            #region 直接量測 gammacurve mvars.flgdirGamma = true
            if (mvars.flgdirGamma)
            {
                uc_atg.lblctime.Text += " GammaCurve Measurement....";
                mCAs.CAremote(true);
                for (svc = 1; svc <= 4; svc++)
                {
                    uc_atg.lstget.Items.Add(" -> msrGammaCurve " + mvars.msrColor[svc].Substring(0, 1) + " G" + mvars.msrgammacurveSt + " to G" + mvars.msrgammacurveEd + " Step" + mvars.msrgammacurveGp);
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                    for (svg = mvars.msrgammacurveSt; svg <= mvars.msrgammacurveEd; svg += mvars.msrgammacurveGp)
                    {
                        if (svc > 1 && svg == 0)
                        {
                            SvFullGrayMsr[svc, svg].Lv = SvFullGrayMsr[1, svg].Lv;
                            SvFullGrayMsr[svc, svg].Sx = SvFullGrayMsr[1, svg].Sx;
                            SvFullGrayMsr[svc, svg].Sy = SvFullGrayMsr[1, svg].Sy;
                        }
                        else
                        {
                            if (svg <= 40)
                            {
                                if (svc == 1) { ShowAGMAnT("X", 0, 0, 0, true, svg, svg, svg, false, 500); }
                                else if (svc == 2) { ShowAGMAnT("X", 0, 0, 0, true, svg, 0, 0, false, 500); }
                                else if (svc == 3) { ShowAGMAnT("X", 0, 0, 0, true, 0, svg, 0, false, 500); }
                                else if (svc == 4) { ShowAGMAnT("X", 0, 0, 0, true, 0, 0, svg, false, 500); }
                            }
                            else
                            {
                                if (svc == 1) { ShowAGMAnT("X", 0, 0, 0, true, svg, svg, svg, false, 200); }
                                else if (svc == 2) { ShowAGMAnT("X", 0, 0, 0, true, svg, 0, 0, false, 200); }
                                else if (svc == 3) { ShowAGMAnT("X", 0, 0, 0, true, 0, svg, 0, false, 200); }
                                else if (svc == 4) { ShowAGMAnT("X", 0, 0, 0, true, 0, 0, svg, false, 200); }
                            }
                            CAmeasF();
                            SvFullGrayMsr[svc, svg].Lv = -1; SvFullGrayMsr[svc, svg].Sx = 0; SvFullGrayMsr[svc, svg].Sy = 0;
                            SvFullGrayMsr[svc, svg].Lv = CAFxLv;
                            SvFullGrayMsr[svc, svg].Sx = CAFx;
                            SvFullGrayMsr[svc, svg].Sy = CAFy;

                            uc_atg.lstget.Items.Add(" -> gammacurve " + mvars.msrColor[svc].Substring(0, 1) + svg.ToString("000") + " " + CAFxLv + "," + CAFy + "," + CAFx);
                            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                            if (mvars.Break) { mvars.ATGerr = "-1"; goto ExNovaAGMA; }

                        }
                    }
                }
                if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\") == false) { Directory.CreateDirectory(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\"); }

                if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma")) { File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma"); }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.gma"))
                {
                    svs = "△min " + mvars.deltaMin + "=|" + mvars.deltaMinXYZ[0] + "|+|" + mvars.deltaMinXYZ[1] + "|+|" + mvars.deltaMinXYZ[2] + "| Cnt " + mvars.deltaMinAt;
                    file.WriteLine(svs);
                    svs = "Gray";
                    for (svg = 0; svg <= gmax; svg++)
                    {
                        svs += "," + mvars.cm603Gamma[svg];
                    }
                    file.WriteLine(svs);
                    for (svc = 2; svc <= 4; svc++)
                    {
                        svs = mvars.msrColor[svc].Substring(0, 1) + " 0x";
                        for (svg = 0; svg <= gmax; svg++)
                        {
                            svs += "," + uc_atg.dgvatg.Rows[8 + (svc - 2) * 2].Cells[svg].Value + uc_atg.dgvatg.Rows[8 + (svc - 2) * 2 + 1].Cells[svg].Value;
                        }
                        file.WriteLine(svs);
                    }
                }
                if (File.Exists(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.csv")) { File.Delete(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.csv"); }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + @"\" + svUUTIDtemp + "_msrGammaCurve.csv"))
                {
                    svs = "Vref,R," + mvars.cm603Vref[0] + ",,G," + mvars.cm603Vref[1] + ",,B," + mvars.cm603Vref[2];
                    file.WriteLine(svs);
                    svs = "Gray0Vol0," + mvars.deltaG0V0;
                    file.WriteLine(svs);
                    svs = "Gray,WLv,Wx,Wy,RLv,Rx,Ry,GLv,Gx,Gy,BLv,Bx,By";
                    file.WriteLine(svs);
                    for (svg = mvars.msrgammacurveSt; svg <= mvars.msrgammacurveEd; svg += mvars.msrgammacurveGp)
                    {
                        svs = svg.ToString();
                        for (svc = 1; svc <= 4; svc++)
                        {
                            svs += "," + SvFullGrayMsr[svc, svg].Lv + "," + SvFullGrayMsr[svc, svg].Sx + "," + SvFullGrayMsr[svc, svg].Sy;
                        }
                        file.WriteLine(svs);
                    }
                }
                uc_atg.lstget.Items.Add(" >> dirmsrGray Done"); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
            }
        #endregion 直接量測 gammacurve mvars.flgdirGamma = true

        ExNovaAGMA:
            mCAs.CAdissconnect();

            #region 回復PC mode
            if (mvars.ATGerr != "0")
            {
                Form1.pvindex = 1;
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(2);    //LAN模式
                                         //if (mvars.dualduty)
                                         //{
                                         //    Form1.pvindex = 31;
                                         //    mvars.lblCmd = "FPGA_SPI_W";
                                         //    mp.mhFPGASPIWRITE(16 + 1 * svDs + 2 * svDs + 4 * svDs + 8 * svDs);

                //    Form1.pvindex = 79;
                //    mvars.lblCmd = "FPGA_SPI_W";
                //    mp.mhFPGASPIWRITE(1000);
                //    Form1.pvindex = 80;
                //    mvars.lblCmd = "FPGA_SPI_W";
                //    mp.mhFPGASPIWRITE(svduty[svDs]);
                //    Form1.pvindex = 81;
                //    mvars.lblCmd = "FPGA_SPI_W";
                //    mp.mhFPGASPIWRITE(svduty[svDs] - 1000);
                //    Form1.pvindex = 82;
                //    mvars.lblCmd = "FPGA_SPI_W";
                //    mp.mhFPGASPIWRITE(svduty[svDs]);
                //    Form1.pvindex = 83;
                //    mvars.lblCmd = "FPGA_SPI_W";
                //    mp.mhFPGASPIWRITE(svduty[svDs] - 1000);

                Form1.pvindex = 255;
                mvars.lblCmd = "FPGA_SPI_W255";
                mp.mhFPGASPIWRITE(0);
                Form1.pvindex = 255;
                mvars.lblCmd = "FPGA_SPI_W255";
                mp.mhFPGASPIWRITE(1);
                Form1.pvindex = 255;
                mvars.lblCmd = "FPGA_SPI_W255";
                mp.mhFPGASPIWRITE(0);
                //}
            }
            #endregion 回復PC mode



            uc_atg.lblctime.Text += ">> " + DateTime.Now;

            if (mvars.demoMode == false && mvars.sp1.IsOpen) mvars.sp1.Close();

            if (mvars.ATGerr == "0" || mvars.ATGerr == "-16")
            {
                if (mvars.UUT.MTP == 1) { if (mvars.Break) uc_atg.lstget.Items.Add(" --> MTP .... Break"); }
                else if (mvars.UUT.MTP == 0) { if (mvars.Break) uc_atg.lstget.Items.Add(" --> DAC .... Break"); }
                if (mvars.Break)
                {
                    mvars.fileCM603Gamma(true, mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + "_UserBreakErr0.gma");
                    mvars.Break = false;
                }
                uc_atg.lstget.Items.Add(" --> AutoGamma Finished");
            }
            else
            {
                if (mvars.Break)
                {
                    mvars.fileCM603Gamma(true, mvars.strStartUpPath + @"\Parameter\Gamma\" + svUUTIDtemp + "_USerBreakErr" + mvars.ATGerr + ".gma");
                    mvars.Break = false;
                }
                #region ATGerr
                string[] sverr = mvars.ATGerr.Split('.');
                switch (sverr[0])
                {
                    case "-1":
                        uc_atg.lstget.Items.Add(" --> GammaCurve User Break");
                        break;
                    case "-2":

                        break;
                    case "-3":

                        break;
                    case "-4":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + svUUTIDtemp + "  MeasGray @ " + mvars.msrColor[svc].Substring(0, 1) + "g" + svg1 + " Sy(" + CAFy + ") * Lv(" + CAFxLv + ") <= 0");
                        break;
                    case "-5":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + svUUTIDtemp + "  msr DLv(" + SvDefaultMsr[svc, 0].Lv + ") > " + string.Format("{0:#0.0####}", (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)) + " cd/m^2");
                        break;
                    case "-6":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + svUUTIDtemp + "  Wg0Lv" + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value + " > Wg1Lv" + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value);
                        break;
                    case "-7":
                        if (mvars.UUT.MTP == 1) uc_atg.lstget.Items.Add(" --> MTP_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  WLv" + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "/Wx" + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "/Wy" + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + " out of spec after MTP");
                        else uc_atg.lstget.Items.Add(" --> DAC_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  WLv" + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + "/Wx" + uc_atg.dgvatg.Rows[2].Cells[gmax + svDs].Value + "/Wy" + uc_atg.dgvatg.Rows[3].Cells[gmax + svDs].Value + " out of spec after DAC");
                        break;
                    case "-8":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  MeasGray @" + mvars.msrColor[svc].Substring(0, 1) + " g" + svg1 + " Lv=-1");
                        break;
                    case "-9":
                        //太廣泛,所以由各發生Lv-1的現象自行在uc_atg.lst_P填寫描述
                        break;
                    case "-10":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  User Break");
                        break;
                    case "-11":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  Wg" + svg1 + "Lv < 3 @ DRun");
                        break;
                    case "-12":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  AutoDetect fail");
                        break;
                    case "-13":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + "  Delta seek fail");
                        break;
                    case "-14":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... GammaSet fail");
                        break;
                    case "-15":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... IPSET fail");
                        break;
                    case "-16":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... posATD fail，CAFxLv = " + CAFxLv);
                        break;
                    case "-17":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... MCU/FPGA/DUTY read error");
                        break;
                    case "-18":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... GammaRead(18h) fail @ DAC mode");
                        break;
                    case "-19":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... WLv" + uc_atg.dgvatg.Rows[1].Cells[gmax + svDs].Value + " < " + (mvars.UUT.CLv * 0.98 * mvars.UUT.WTLvBet).ToString("###0.###") + "@ g255:" + uc_atg.dgvatg.Rows[8].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[9].Cells[gmax + svDs].Value + "," + uc_atg.dgvatg.Rows[10].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[11].Cells[gmax + svDs].Value + "," + uc_atg.dgvatg.Rows[12].Cells[gmax + svDs].Value + uc_atg.dgvatg.Rows[13].Cells[gmax + svDs].Value);
                        break;
                    case "-20":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... GammaRead(18h) compare fail");
                        break;
                    case "-21":
                        //uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... WLv" + uc_atg.dgvatg.Rows[1].Cells[1 + svDs].Value + " > " + svuutclv + "@ g1:" + uc_atg.dgvatg.Rows[8].Cells[1 + svDs * (gmax + 2)].Value + uc_atg.dgvatg.Rows[9].Cells[1 + svDs].Value + "," + uc_atg.dgvatg.Rows[10].Cells[1 + svDs].Value + uc_atg.dgvatg.Rows[11].Cells[1 + svDs].Value + "," + uc_atg.dgvatg.Rows[12].Cells[1 + svDs].Value + uc_atg.dgvatg.Rows[13].Cells[1 + svDs].Value);
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID);
                        break;
                    case "-22":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... DLv" + uc_atg.dgvatg.Rows[1].Cells[0 + svDs].Value + " Out of spec.");
                        break;
                    case "-23":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... MCU code ( " + mvars.verMCU + " ) too old，please update");
                        break;
                    case "-24":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... FPGA code ( " + mvars.verFPGA + " ) too old，please update");
                        break;
                    case "-25":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... pre WLv(" + SvDefaultMsr[1, gmax].Lv + ") error，Please Check Probe @ 0-CAL or Not White Pattern");
                        break;
                    case "-26":
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " .... 20/10 over spec(" + sv20d10[0, 0] / sv20d10[1, 0] + sv20d10[0, 1] / sv20d10[1, 1] + sv20d10[0, 2] / sv20d10[1, 2] + sv20d10[0, 3] / sv20d10[1, 3] + ")");
                        break;
                    case "-27":
                        uc_atg.lstget.Items.Add(" --> GammaCurve_fail(" + mvars.ATGerr + ")   ID: " + mvars.UUT.ID + " msrGammaCurve.csv file id open，can't be delete");
                        break;
                }
                #endregion ATGerr
            }    //if(mvars.ATGerr == "0" || mvars.ATGerr == "-16")

            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
            funSaveLogs("↑" + DateTime.Now + " (ATG)   ID:" + mvars.UUT.ID + ",Done,(" + mvars.ATGerr + ")");
            funSaveLogs("");
            if (mvars.FormShow[5]) { Form2.i3pat.Close(); }

            mvars.UUT.MTP = svMTP;

            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.ATGerr == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("##0") + ",0,0," + (30 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("##0"))); }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("##0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("##0"))); }
            uc_atg.lblctime.Text += "，" + (DateTime.Now - mvars.t1).TotalSeconds.ToString("##0") + "s";
            mvars.errCode = mvars.ATGerr;
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
            #endregion
        }





        public static UInt32 CRC_Cal(UInt32 lData, UInt32 lCrcPolynomial, UInt32 lDataBitCnt)
        {
            UInt32 /*lDivisor,*/ lDividend, lRemainder, lXorDividend, lCRC = 0, lCrcBitCnt = 0;
            for (uint i = 0; i < 32; i++)
            {
                if (lCrcPolynomial > (UInt32)Math.Pow(2, i))
                    lCrcBitCnt = i;
            }
            lDividend = lData * (UInt32)Math.Pow(2, lCrcBitCnt);
            for (int i = ((int)lDataBitCnt - 1); i >= 0; i--)
            {
                if ((lDividend & ((UInt32)Math.Pow(2, (i + lCrcBitCnt)))) != 0)
                {
                    lXorDividend = lDividend / ((UInt32)Math.Pow(2, i));
                    lRemainder = lDividend % ((UInt32)Math.Pow(2, i));
                    lCRC = lXorDividend ^ lCrcPolynomial;
                    lDividend = lCRC * ((UInt32)Math.Pow(2, i)) + lRemainder;
                    if (lDividend < (UInt32)Math.Pow(2, lCrcBitCnt))
                    {
                        i = 0;
                        lCRC = lDividend;
                    }
                }
            }
            return lCRC;
        }

        public static string ReplaceAt(string input, int index, string svoneword)
        {
            string svs1 = "";
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            string[] svs = new string[input.Length];

            for (int svi = 0; svi < input.Length; svi++)
            {
                svs[svi] = input.Substring(svi, 1);
            }
            svs[index - 1] = svoneword;

            svs1 = svs[0];
            for (int svi = 1; svi < input.Length; svi++)
            {
                svs1 += svs[svi];
            }
            return svs1;
        }


        public static void cOMSEL(string svdeviceID, string svFPGAsel, string svdata)           // Primary,TV130
        {
            /// svFPGAsel  0:ABCD 1:EFGH 2:Boardcast
            /// _______________________
            /// |  FPGA B  |  FPGA A  |
            /// |   EFGH   |   ABCD   |
            /// |  左畫面  |  右畫面  |
            /// |    1     |     0    |
            /// |          |          |
            /// =======================
            /// svFPGAsel  0:ABCD 1:EFGH 2:Boardcast
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            if (svdeviceID == "160") mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            else mvars.deviceID = mvars.deviceID.Substring(0, 2) + svdeviceID.PadLeft(2, '0');

            #region main
            mvars.isReadBack = false;
            mvars.lblCmd = "FPGA_SPI_W";
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                Form1.pvindex = 4;
                mp.mhFPGASPIWRITE(Convert.ToByte(svFPGAsel), Convert.ToInt32(svdata));
            }
            else if (mvars.deviceID.Substring(0, 2) == "06")
            {
                Form1.pvindex = 100;
                mp.mhFPGASPIWRITE(Convert.ToByte(svFPGAsel), Convert.ToInt32(svdata));
            }
            else if (mvars.deviceID.Substring(0, 2) == "10")
            {
                Form1.pvindex = 4;
                mp.mhFPGASPIWRITE(Convert.ToInt32(svdata));
            }
            else
            {
                Form1.pvindex = 100;
                mp.mhFPGASPIWRITE(Convert.ToInt32(svdata));
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
            #endregion main                                    

            //ExNovaAGMA:
            //mvars.nvBoardcast = svnvbc;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
            }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static void cOMSEL(int svdata, byte svip, byte svsen, byte svpo, byte svca)   //以 cDROP 為藍圖更改
        {
            /// svFPGAsel  0:ABCD 1:EFGH 2:Boardcast
            /// _______________________
            /// |  FPGA B  |  FPGA A  |
            /// |   EFGH   |   ABCD   |
            /// |  左畫面  |  右畫面  |
            /// |    1     |     0    |
            /// |          |          |
            /// =======================
            /// svFPGAsel  0:ABCD 1:EFGH 2:Boardcast
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;
            if (svpo==160) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            else mvars.deviceID = mvars.deviceID.Substring(0, 2) + svpo.ToString("00");
            if (svca > 2) svca = 2;
            mvars.FPGAsel = svca;
            #region main
            mvars.isReadBack = false;
            mvars.lblCmd = "FPGA_SPI_W";
            if (mvars.deviceID.Substring(0, 2) == "05")
            {
                Form1.pvindex = 4;
                mp.mhFPGASPIWRITE(mvars.FPGAsel, svdata);
            }
            else
            {
                Form1.pvindex = 100;
                mp.mhFPGASPIWRITE(svdata);
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
            #endregion main                                    

            //ExNovaAGMA:
            //mvars.nvBoardcast = svnvbc;
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
            }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        public static void mReadGamma_RegDecArray(int RegDec, int RegCnt)
        {
            #region 2023版公用程序
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0D);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 8193);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x12;                                //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                                //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D;                                //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(RegDec / 256);                //Reg Address
            mvars.RS485_WriteDataBuffer[6 + svns] = (byte)(RegDec % 256);                //Reg Address
            mvars.RS485_WriteDataBuffer[7 + svns] = (byte)(RegCnt / 256);                //Reg Count
            mvars.RS485_WriteDataBuffer[8 + svns] = (byte)(RegCnt % 256);                //Reg Count

            mp.funSendMessageTo();
        }

        //private void Gamma_Reg_A_dGV_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    int RowIndex = Gamma_Reg_A_dGV.CurrentCell.RowIndex;
        //    int ColumnIndex = Gamma_Reg_A_dGV.CurrentCell.ColumnIndex;
        //    if (RowIndex > 0 && ColumnIndex == 0)
        //    {
        //        UInt16 RegDec = Convert.ToUInt16(Gamma_Reg_A_dGV.Rows[RowIndex].Cells[ColumnIndex].Value);
        //        UInt16 DataDec = Convert.ToUInt16(Gamma_Reg_A_dGV.Rows[RowIndex].Cells[1].Value);
        //        WriteGamma_RegDec(varTxRx, FPGA_A, RegDec.ToString(), DataDec.ToString());
        //    }
        //}

        //private void Gamma_Reg_B_dGV_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    int RowIndex = Gamma_Reg_B_dGV.CurrentCell.RowIndex;
        //    int ColumnIndex = Gamma_Reg_B_dGV.CurrentCell.ColumnIndex;
        //    if (RowIndex > 0 && ColumnIndex == 0)
        //    {
        //        UInt16 RegDec = Convert.ToUInt16(Gamma_Reg_B_dGV.Rows[RowIndex].Cells[ColumnIndex].Value);
        //        UInt16 DataDec = Convert.ToUInt16(Gamma_Reg_B_dGV.Rows[RowIndex].Cells[1].Value);
        //        WriteGamma_RegDec(varTxRx, FPGA_B, RegDec.ToString(), DataDec.ToString());
        //    }
        //}
        //public static void mWriteGamma_RegDec(USB_TxRx varTxRx, Byte No, string RegDec, string DataDec)
        //{
        //    Byte[] OUTBuffer = new byte[513]; Byte[] INBuffer = new byte[513];
        //    varTxRx.OUTBuffer = OUTBuffer; varTxRx.INBuffer = INBuffer;
        //    varTxRx.bReturnFlag = false;

        //    OUTBuffer[2 + 1] = 0x13;            //Cmd
        //    OUTBuffer[3 + 1] = 0x00;            //Size
        //    OUTBuffer[4 + 1] = 0x0E;            //Size
        //    OUTBuffer[5 + 1] = No;              //FPGA Sel
        //    OUTBuffer[6 + 1] = Convert.ToByte(Convert.ToUInt16(RegDec) / 256);    //Reg Address
        //    OUTBuffer[7 + 1] = Convert.ToByte(Convert.ToUInt16(RegDec) % 256);    //Reg Address
        //    OUTBuffer[8 + 1] = Convert.ToByte(Convert.ToUInt16(DataDec) / 256);    //Data
        //    OUTBuffer[9 + 1] = Convert.ToByte(Convert.ToUInt16(DataDec) % 256);    //Data

        //    FillOutBuffer(OUTBuffer, varTxRx.sDevID, varTxRx.sIP);
        //    //Send To MCU
        //    if (WriteReadMcuTask(varTxRx) == true)
        //    {
        //        string sLine = "GammaReg" + RegDec + ", Data： " + DataDec;
        //        if (No == FPGA_A) sLine = "FPGA A , " + sLine;
        //        if (No == FPGA_B) sLine = "FPGA B , " + sLine;
        //        if (No == FPGA_ALL) sLine = "FPGA ALL , " + sLine;
        //        varTxRx.bReturnFlag = true;
        //        OP_Msg1(varTxRx.lbx, sLine);
        //    }
        //}

        //private void Brightness_btn_Click(object sender, EventArgs e)
        //{
        //    string[] sRegDec = { "5", "6", "7" };
        //    string[] sDataDec = { Brightness_txt.Text, Brightness_txt.Text, Brightness_txt.Text };
        //    WriteGamma_RegDecArray(varTxRx, FPGA_ALL, sRegDec, sDataDec);
        //}
        //public static void mWriteGamma_RegDecArray(USB_TxRx varTxRx, Byte No, string[] RegDec, string[] DataDec)
        //{
        //    Byte[] OUTBuffer = new byte[513]; Byte[] INBuffer = new byte[513];
        //    varTxRx.OUTBuffer = OUTBuffer; varTxRx.INBuffer = INBuffer;
        //    UInt16 PacketSize = (UInt16)(9 + RegDec.Length * 5 + 1);

        //    OUTBuffer[2 + 1] = 0x13;                        //Cmd
        //    OUTBuffer[3 + 1] = (Byte)(PacketSize / 256);    //Size
        //    OUTBuffer[4 + 1] = (Byte)(PacketSize % 256);    //Size
        //    for (uint i = 0; i < RegDec.Length; i++)
        //    {
        //        if (No == FPGA_A)
        //            OUTBuffer[5 + 1 + i * 5] = 0;           //Left
        //        else if (No == FPGA_B)
        //            OUTBuffer[5 + 1 + i * 5] = 1;           //Right
        //        else if (No == FPGA_ALL)
        //            OUTBuffer[5 + 1 + i * 5] = 2;           //ALL
        //        OUTBuffer[6 + 1 + i * 5] = Convert.ToByte(Convert.ToInt32(RegDec[i]) / 256);       //Reg Address
        //        OUTBuffer[7 + 1 + i * 5] = Convert.ToByte(Convert.ToInt32(RegDec[i]) % 256);       //Reg Address
        //        OUTBuffer[8 + 1 + i * 5] = Convert.ToByte(Convert.ToInt32(DataDec[i]) / 256);      //Reg Data
        //        OUTBuffer[9 + 1 + i * 5] = Convert.ToByte(Convert.ToInt32(DataDec[i]) % 256);      //Reg Data
        //    }
        //    FillOutBuffer(OUTBuffer, varTxRx.sDevID, varTxRx.sIP);
        //    //Send To MCU
        //    if (WriteReadMcuTask(varTxRx) == true)
        //    {
        //        OP_Msg1(varTxRx.lbx, "WriteGamma_RegDecArray OK");
        //    }
        //}




        public static void cFLASHREAD_C12AMCU(string svstraddr)
        {
            mp.markreset(99, true);     //delfb自行決定

            int PacketSize = 2048;
            int svc = svc = (int)mvars.GAMMA_SIZE / PacketSize;

            if (svstraddr == "60000") { PacketSize = 1024; svc = (int)mvars.FPGA_SIZE / PacketSize; }
            else if (svstraddr == "62000") { svc = (int)mvars.GAMMA_SIZE / PacketSize; mvars.strR62K = ""; }
            else if (svstraddr == "64000") { svc = (int)mvars.GAMMA_SIZE / PacketSize; mvars.strR64K = ""; }
            else if (svstraddr == "66000") { svc = (int)mvars.GAMMA_SIZE / PacketSize; mvars.strR66K = ""; }
            //else if (svstraddr == "68000") { svc = (int)mvars.GAMMA_SIZE / PacketSize; mvars.strR68K = ""; }

            string txt44 = (0 * PacketSize + mvars.FPGA_START_ADDR).ToString("X8");
            for (UInt32 i = 0; i < svc; i++)
            {
                if (svstraddr == "60000") txt44 = (i * PacketSize + mvars.FPGA_START_ADDR).ToString("X8");
                else if (svstraddr == "62000") txt44 = (i * PacketSize + mvars.GAMMA_START_ADDR).ToString("X8");
                else if (svstraddr == "64000") txt44 = (i * PacketSize + mvars.GAMMA_START_ADDR + 0x2000).ToString("X8");
                else if (svstraddr == "66000") txt44 = (i * PacketSize + mvars.GAMMA_START_ADDR + 0x4000).ToString("X8");
                //else if (svstraddr == "68000") txt44 = (i * PacketSize + mvars.GAMMA_START_ADDR + 0x6000).ToString("X8");
                Application.DoEvents();
                mvars.lblCmd = "MCU_FLASH_R" + svstraddr;
                mp.mhMCUFLASHREAD(txt44, PacketSize);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mvars.errCode = "-" + (i + 1); goto ExNovaAGMA; }
            }
        ExNovaAGMA:
            //if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }

            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                if (mvars.FormShow[7] == true)
                {
                    if (svstraddr == "60000")
                    {
                        mvars.lstmcuR60000.Items.Clear();
                        if (mvars.strR60K.Length > 3)
                        {
                            if (mvars.strR60K.Length > 1 && mvars.strR60K.Substring(0, 1) == "~") { mvars.strR60K = mvars.strR60K.Substring(1, mvars.strR60K.Length - 1); }
                            mvars.lstmcuR60000.Items.AddRange(mvars.strR60K.Split('~'));
                        }
                        //mvars.lstget.Items.Add(mvars.lblCompose + mvars.strR60K);
                        Form1.lstget1.Items.Add(mvars.lblCompose + mvars.strR60K);
                    }
                    else if (svstraddr == "62000")
                    {
                        mvars.lstmcuR62000.Items.Clear();
                        if (mvars.strR62K.Length > 3)
                        {
                            if (mvars.strR62K.Length > 1 && mvars.strR62K.Substring(0, 1) == "~") { mvars.strR62K = mvars.strR62K.Substring(1, mvars.strR62K.Length - 1); }
                            mvars.lstmcuR62000.Items.AddRange(mvars.strR62K.Split('~'));
                        }
                        //mvars.lstget.Items.Add(mvars.lblCompose + mvars.strR62K);
                        Form1.lstget1.Items.Add(mvars.lblCompose + mvars.strR62K);
                    }
                    else if (svstraddr == "64000")
                    {
                        mvars.lstmcuR64000.Items.Clear();
                        if (mvars.strR64K.Length > 3)
                        {
                            if (mvars.strR64K.Length > 1 && mvars.strR64K.Substring(0, 1) == "~") { mvars.strR64K = mvars.strR64K.Substring(1, mvars.strR64K.Length - 1); }
                            mvars.lstmcuR64000.Items.AddRange(mvars.strR64K.Split('~'));
                        }
                        //mvars.lstget.Items.Add(mvars.lblCompose + mvars.strR64K);
                        Form1.lstget1.Items.Add(mvars.lblCompose + mvars.strR64K);
                    }
                    else if (svstraddr == "66000")
                    {
                        mvars.lstmcuR66000.Items.Clear();
                        if (mvars.strR66K.Length > 3)
                        {
                            if (mvars.strR66K.Length > 1 && mvars.strR66K.Substring(0, 1) == "~") { mvars.strR66K = mvars.strR66K.Substring(1, mvars.strR66K.Length - 1); }
                            mvars.lstmcuR66000.Items.AddRange(mvars.strR66K.Split('~'));
                        }
                        //mvars.lstget.Items.Add(mvars.lblCompose + mvars.strR66K);
                        Form1.lstget1.Items.Add(mvars.lblCompose + mvars.strR66K);
                    }
                    //else if (svstraddr == "68000")
                    //{
                    //    mvars.lstmcuR68000.Items.Clear();
                    //    if (mvars.strR68K.Length > 3)
                    //    {
                    //        if (mvars.strR68K.Length > 1 && mvars.strR68K.Substring(0, 1) == "~") { mvars.strR68K = mvars.strR68K.Substring(1, mvars.strR68K.Length - 1); }
                    //        mvars.lstmcuR68000.Items.AddRange(mvars.strR68K.Split('~'));
                    //    }
                    //    mvars.lstget.Items.Add(mvars.lblCompose + mvars.strR68K);
                    //}
                }
                mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30";
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static string ContinueAsciiFilter(string sStr, string sAscii)
        {
            for (int i = 0; i < (sStr.Length - 1); i++)
            {
                if (sStr.Substring(i, 1) == sAscii && sStr.Substring(i + 1, 1) == sAscii)
                    sStr = sStr.Remove(i--, 1);
            }
            return sStr;
        }


        public static void fileDelete(string sDir)
        {
            DirectoryInfo di = new DirectoryInfo(sDir);
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }


        public static string fileSearch(string sDir, string svsubname)
        {
            // ex. svsubname = "*.*";
            string svs = "";
            try
            {
                //先找出所有目錄
                //foreach (string d in Directory.GetDirectories(sDir))
                //{
                //先針對目錄的檔案做處理
                foreach (string f in Directory.GetFiles(sDir, svsubname))
                {
                    Console.WriteLine(f);
                    //filelist.Add(f);
                    //lst_filepathfull.Items.Add(f);
                    svs += f.Split('\\')[f.Split('\\').Length - 1] + "+";
                    //cmb_box.Items.Add(lstget1.Items[lstget1.Items.Count - 1].ToString().Split('.')[0]);
                }
                //此目錄完再針對每個子目錄做處理
                //dirSearch(d); //單一目錄則不重複search
                //}
                if (svs.Length > 0 && svs.Substring(svs.Length - 1, 1) == "+") svs = svs.Substring(0, svs.Length - 1);
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return svs;
        }




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


        #region 20230328
        /// <summary>
        /// 
        /// 
        /// 

        public static void mUIREGARDRm(int svregSt, int svregEd)                //0x12 multi register 
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 8193);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x12;                      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                      //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D;                      //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(svregSt / 256);     //Reg Address
            mvars.RS485_WriteDataBuffer[6 + svns] = (byte)(svregSt % 256);     //Reg Address
            mvars.RS485_WriteDataBuffer[7 + svns] = (byte)(svregEd / 256);     //Reg Count
            mvars.RS485_WriteDataBuffer[8 + svns] = (byte)(svregEd % 256);     //Reg Count        
            funSendMessageTo();
        }

        public static void mUIREGARDWm(string[] RegDec, string[] DataDec)       //0x13 multi register 
        {
            #region 2023版公用程序
            byte svns = 2;  
            int PacketSize = 9 + RegDec.Length * 5 + 1;
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 20; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, PacketSize + 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x13;                           //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = (Byte)(PacketSize / 256);       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = (Byte)(PacketSize % 256);       //Size
            for (uint i = 0; i < RegDec.Length; i++)
            {
                mvars.RS485_WriteDataBuffer[5 + svns + i * 5] = mvars.FPGAsel;
                mvars.RS485_WriteDataBuffer[6 + svns + i * 5] = Convert.ToByte(Convert.ToInt32(RegDec[i]) / 256);       //Reg Address
                mvars.RS485_WriteDataBuffer[7 + svns + i * 5] = Convert.ToByte(Convert.ToInt32(RegDec[i]) % 256);       //Reg Address
                mvars.RS485_WriteDataBuffer[8 + svns + i * 5] = Convert.ToByte(Convert.ToInt32(DataDec[i]) / 256);      //Reg Data
                mvars.RS485_WriteDataBuffer[9 + svns + i * 5] = Convert.ToByte(Convert.ToInt32(DataDec[i]) % 256);      //Reg Data
            }
            funSendMessageTo();
        }

        public static void mUIREGARDW(string RegDec, string DataDec)            //0x13 single register 
        {
            #region 2023版公用程序
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 20; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0E);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x13;            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;            //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = mvars.FPGAsel;              //FPGA Sel
            mvars.RS485_WriteDataBuffer[6 + svns] = Convert.ToByte(Convert.ToUInt16(RegDec) / 256);    //Reg Address
            mvars.RS485_WriteDataBuffer[7 + svns] = Convert.ToByte(Convert.ToUInt16(RegDec) % 256);    //Reg Address
            mvars.RS485_WriteDataBuffer[8 + svns] = Convert.ToByte(Convert.ToUInt16(DataDec) / 256);    //Data
            mvars.RS485_WriteDataBuffer[9 + svns] = Convert.ToByte(Convert.ToUInt16(DataDec) % 256);    //Data
            funSendMessageTo();
        }

        public static void cUIREGADRwENG(string svstraddr, string[] RegDec, string[] DataDec)
        {
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            /*
                Primary MCU Flash 
                0x62000採特殊方式 切 FPGA A4096 / FPGA B4096 Total = 8192
                如果只有single duty則是填 0~6 , 10~282 = 283bytes
                如果是dual duty則是填 0~6 , 10~282 , 300~572 = 572bytes
                0x64000則不分區，所以僅填單側0~39個 = 40bytes
             */

            int svcounts = RegDec.Length;
            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            byte[] svR62karr = new byte[mvars.GAMMA_SIZE];

            mvars.lblCmd = "MCU_FLASH_R" + svstraddr;
            mhMCUFLASHREAD(svstraddr, BinArr);


            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }
            else
            {
                if (svstraddr == "62000")
                {
                    if (mvars.strR62K.Length > 1 && mvars.strR62K.Split('~').Length == 2048)
                    {
                        for (int svi = 0; svi < 1024; svi++)
                        {
                            svR62karr[0 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi].Split(',')[0]) / 256);
                            svR62karr[1 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi].Split(',')[0]) % 256);
                            svR62karr[2 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi].Split(',')[1]) / 256);
                            svR62karr[3 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi].Split(',')[1]) % 256);

                            svR62karr[0 + svi * 4 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi + 1024].Split(',')[0]) / 256);
                            svR62karr[1 + svi * 4 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi + 1024].Split(',')[0]) % 256);
                            svR62karr[2 + svi * 4 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi + 1024].Split(',')[1]) / 256);
                            svR62karr[3 + svi * 4 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi + 1024].Split(',')[1]) % 256);
                        }
                        //Save File
                        //path = "C:\\Users\\" + Environment.UserName + "\\Documents\\svR62karr.bin";
                        //SaveBinFile(path, svR62karr);
                    }
                    //else lbl_mcuR62000click.Text = "no record";
                }
                else if (svstraddr == "64000")
                {
                    if (mvars.strR64K.Length > 1 && mvars.strR64K.Split('~').Length == 2048)
                    {
                        for (int svi = 0; svi < 1024; svi++)
                        {
                            svR62karr[0 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR64K.Split('~')[svi].Split(',')[0]) / 256);
                            svR62karr[1 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR64K.Split('~')[svi].Split(',')[0]) % 256);
                            svR62karr[2 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR64K.Split('~')[svi].Split(',')[1]) / 256);
                            svR62karr[3 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR64K.Split('~')[svi].Split(',')[1]) % 256);
                            //0x64000 MCU 會同時下 FPGA A與B
                        }
                        //Save File
                        //path = "C:\\Users\\" + Environment.UserName + "\\Documents\\svR62karr.bin";
                        //SaveBinFile(path, svR62karr);
                    }
                    //else lbl_mcuR62000click.Text = "no record";
                }
            }

            if (svstraddr == "62000")
            {
                //mvars.FPGAsel=0，右側畫面，FPGA A，Gamma A
                for (UInt16 i = 0; i < RegDec.Length; i++)
                {
                    if (mvars.FPGAsel == 2 || mvars.FPGAsel == 0)
                    {
                        BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(RegDec[i]) / 256);
                        BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(RegDec[i]) % 256);
                        BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(DataDec[i]) / 256);
                        BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(DataDec[i]) % 256);
                    }
                    else
                    {
                        if (mvars.strR62K.Length < 2)
                        {
                            BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[0]) / 256);
                            BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[0]) % 256);
                            BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[2]) / 256);
                            BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[2]) % 256);
                        }
                        else
                        {
                            BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[i].Split(',')[0]) / 256);
                            BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[i].Split(',')[0]) % 256);
                            BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[i].Split(',')[1]) / 256);
                            BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[i].Split(',')[1]) % 256);
                        }
                    }
                }
                //mvars.FPGAsel=1，左側畫面，FPGA B，Gamma B
                for (UInt16 i = 0; i < svcounts; i++)
                {
                    if (mvars.FPGAsel == 2 || mvars.FPGAsel == 1)
                    {
                        BinArr[i * 4 + 0 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(RegDec[i]) / 256);
                        BinArr[i * 4 + 1 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(RegDec[i]) % 256);
                        BinArr[i * 4 + 2 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(DataDec[i]) / 256);
                        BinArr[i * 4 + 3 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(DataDec[i]) % 256);
                    }
                    else
                    {
                        if (mvars.strR62K.Length < 2)
                        {
                            BinArr[i * 4 + 0 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[0]) / 256);
                            BinArr[i * 4 + 1 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[0]) % 256);
                            BinArr[i * 4 + 2 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[2]) / 256);
                            BinArr[i * 4 + 3 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[2]) % 256);
                        }
                        else
                        {
                            //int sv0 = Convert.ToInt32(mvars.strR62K.Split('~')[i + 1024].Split(',')[0]);
                            //int sv1 = Convert.ToInt32(mvars.strR62K.Split('~')[i + 1024].Split(',')[1]);

                            BinArr[i * 4 + 0 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[i + 1024].Split(',')[0]) / 256);
                            BinArr[i * 4 + 1 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[i + 1024].Split(',')[0]) % 256);
                            BinArr[i * 4 + 2 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[i + 1024].Split(',')[1]) / 256);
                            BinArr[i * 4 + 3 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[i + 1024].Split(',')[1]) % 256);
                        }
                    }
                }
            }
            else if (svstraddr == "64000")
            {
                //不需要分左右畫面
                for (UInt16 i = 0; i < RegDec.Length; i++)
                {
                    BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(RegDec[i]) / 256);
                    BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(RegDec[i]) % 256);
                    BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(DataDec[i]) / 256);
                    BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(DataDec[i]) % 256);
                }
            }

            //Checksum
            UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
            BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
            BinArr[BinArr.Length - 1] = (byte)(checksum % 256);

            //Save File
            //path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr.bin";
            //SaveBinFile(path, BinArr);


            mvars.lblCmd = "MCU_FLASH_W" + svstraddr;
            mhMCUFLASHWRITE(svstraddr, ref BinArr);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }

            mvars.lblCmd = "MCU_FLASH_R" + svstraddr;
            mhMCUFLASHREAD(svstraddr, BinArr);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }
            else
            {
                if (svstraddr == "62000")
                {
                    mvars.lstmcuR62000.Items.Clear();
                    mvars.lstmcuR62000.Items.AddRange(mvars.strR62K.Split('~'));
                }
                else if (svstraddr == "64000")
                {
                    mvars.lstmcuR64000.Items.Clear();
                    mvars.lstmcuR64000.Items.AddRange(mvars.strR64K.Split('~'));
                }
            }

            mvars.lblCmd = "FPGA_REG_W";
            mpFPGAUIREGWarr(RegDec, DataDec);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
            else
            {
                if (svstraddr == "62000")
                {
                    if (mvars.FPGAsel == 0)
                    {
                        for (int i = 0; i < RegDec.Length; i++) Form1.svuiregadr[i] = DataDec[i];
                    }
                    if (mvars.FPGAsel == 1)
                    {
                        for (int i = 0; i < RegDec.Length; i++) Form1.svuiregadr[i + 1024] = DataDec[i];
                    }
                    if (mvars.FPGAsel == 2)
                    {
                        for (int i = 0; i < RegDec.Length; i++)
                        {
                            Form1.svuiregadr[i] = DataDec[i];
                            Form1.svuiregadr[i + 1024] = DataDec[i];
                        }
                    }
                    Form1.lstsvuiregadr.Items.Clear();
                    for (int i = 0; i < Form1.svuiregadr.Count; i++)
                    {
                        if (Form1.svuiregadr[i] != null) Form1.lstsvuiregadr.Items.Add(Form1.svuiregadr[i]);
                        else Form1.lstsvuiregadr.Items.Add(" ");
                    }
                }
            }

        Ex:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (30 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0")));
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0"))); }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        public static void cUIREGADRwENG(string svstraddr)
        {
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            /*
                Primary MCU Flash 
                0x62000採特殊方式 切 FPGA A4096 / FPGA B4096 Total = 8192
                如果只有single duty則是填 0~6 , 10~282 = 283bytes
                如果是dual duty則是填 0~6 , 10~282 , 300~572 = 572bytes
                0x64000則不分區，所以僅填單側0~39個 = 40bytes
             */

            int svcounts = mvars.uiregadr_default.Length;
            //svcounts = RegDec.Length;
            //byte[] svR62karr = new byte[mvars.GAMMA_SIZE];
            //string path;

            byte[] BinArr = new byte[mvars.GAMMA_SIZE];
            string[] RegDec = new string[svcounts];
            string[] DataDec = new string[svcounts];


            //if (mvars.FPGAsel != 2)
            //{
            //    mvars.lblCmd = "MCU_FLASH_R" + svstraddr;
            //    mhMCUFLASHREAD(svstraddr, BinArr);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }
            //    else
            //    {
            //        if (mvars.strR62K.Length > 1 && mvars.strR62K.Split('~').Length == 2048)
            //        {
            //            for (int svi = 0; svi < 1024; svi++)
            //            {
            //                svR62karr[0 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi].Split(',')[0]) / 256);
            //                svR62karr[1 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi].Split(',')[0]) % 256);
            //                svR62karr[2 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi].Split(',')[1]) / 256);
            //                svR62karr[3 + svi * 4] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi].Split(',')[1]) % 256);
            //                svR62karr[0 + svi * 4 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi + 1024].Split(',')[0]) / 256);
            //                svR62karr[1 + svi * 4 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi + 1024].Split(',')[0]) % 256);
            //                svR62karr[2 + svi * 4 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi + 1024].Split(',')[1]) / 256);
            //                svR62karr[3 + svi * 4 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.strR62K.Split('~')[svi + 1024].Split(',')[1]) % 256);
            //            }
            //            //Save File
            //            //path = "C:\\Users\\" + Environment.UserName + "\\Documents\\svR62karr.bin";
            //            //SaveBinFile(path, svR62karr);
            //        }
            //        //else lbl_mcuR62000click.Text = "no record";
            //    }
            //}

            //mvars.FPGAsel=0，右側畫面，FPGA A，Gamma A
            for (UInt16 i = 0; i < svcounts; i++)
            {
                if (mvars.FPGAsel == 2 || mvars.FPGAsel == 0)
                {
                    BinArr[i * 4 + 0] = (Byte)(i / 256);
                    BinArr[i * 4 + 1] = (Byte)(i % 256);
                    BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i]) / 256);
                    BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i]) % 256);
                }
                else
                {
                    BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[0]) / 256);
                    BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[0]) % 256);
                    BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[2]) / 256);
                    BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[2]) % 256);
                }
                RegDec[i] = i.ToString();
            }
            //mvars.FPGAsel=1，左側畫面，FPGA B，Gamma B
            for (UInt16 i = 0; i < svcounts; i++)
            {
                if (mvars.FPGAsel == 2 || mvars.FPGAsel == 1)
                {
                    BinArr[i * 4 + 0 + mvars.GAMMA_SIZE / 2] = (Byte)(i / 256);
                    BinArr[i * 4 + 1 + mvars.GAMMA_SIZE / 2] = (Byte)(i % 256);
                    BinArr[i * 4 + 2 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i]) / 256);
                    BinArr[i * 4 + 3 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i]) % 256);
                }
                else
                {
                    BinArr[i * 4 + 0 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[0]) / 256);
                    BinArr[i * 4 + 1 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[0]) % 256);
                    BinArr[i * 4 + 2 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[2]) / 256);
                    BinArr[i * 4 + 3 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(mvars.uiregadr_default[i].Split(',')[2]) % 256);
                }
            }
            //Checksum
            UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
            BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
            BinArr[BinArr.Length - 1] = (byte)(checksum % 256);

            //Save File
            //path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr.bin";
            //SaveBinFile(path, BinArr);


            mvars.lblCmd = "MCU_FLASH_W" + svstraddr;
            mhMCUFLASHWRITE(svstraddr, ref BinArr);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }

            mvars.lblCmd = "MCU_FLASH_R" + svstraddr;
            mhMCUFLASHREAD(svstraddr, BinArr);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }
            else
            {
                if (svstraddr == "62000")
                {
                    mvars.lstmcuR62000.Items.Clear();
                    mvars.lstmcuR62000.Items.AddRange(mvars.strR62K.Split('~'));
                }
            }
            
            for (byte svi = 0; svi < 2; svi++)
            {
                mvars.lblCmd = "FPGA_REG_W";
                for (int svj = 0; svj < svcounts; svj++) { DataDec[svj] = Form1.svuiregadr[svj + 0 * 1024]; }
                mpFPGAUIREGWarr(RegDec, DataDec);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
            }

            Form1.lstsvuiregadr.Items.Clear();
            for (int i = 0; i < Form1.svuiregadr.Count; i++)
            {
                if (Form1.svuiregadr[i] != null) Form1.lstsvuiregadr.Items.Add(Form1.svuiregadr[i]);
                else Form1.lstsvuiregadr.Items.Add(" ");
            }


        Ex:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (30 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0")));
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0"))); }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }



        public static void mpFPGAUIREGW(string RegDec, string DataDec)              //0x13 single register
        {
            #region 2023版公用程序(w/o novastar)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x13;               //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;               //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;               //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = mvars.FPGAsel;      //FPGA Sel
            mvars.RS485_WriteDataBuffer[6 + svns] = Convert.ToByte(Convert.ToUInt16(RegDec) / 256);     //Reg Address
            mvars.RS485_WriteDataBuffer[7 + svns] = Convert.ToByte(Convert.ToUInt16(RegDec) % 256);     //Reg Address
            mvars.RS485_WriteDataBuffer[8 + svns] = Convert.ToByte(Convert.ToUInt16(DataDec) / 256);    //Data
            mvars.RS485_WriteDataBuffer[9 + svns] = Convert.ToByte(Convert.ToUInt16(DataDec) % 256);    //Data
            funSendMessageTo();
        }

        public static void mpFPGAUIREGWarr(string[] RegDec, string[] DataDec)       //0x11 multi register
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            int PacketSize = 9 + RegDec.Length * 5 + 1;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, 8192);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x13;       //Cmd
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];   //Size
            for (uint i = 0; i < RegDec.Length; i++)
            {
                mvars.RS485_WriteDataBuffer[5 + svns + i * 5] = mvars.FPGAsel;                                      //FPGA SELECT
                mvars.RS485_WriteDataBuffer[6 + svns + i * 5] = Convert.ToByte(Convert.ToInt32(RegDec[i]) / 256);   //Reg Address
                mvars.RS485_WriteDataBuffer[7 + svns + i * 5] = Convert.ToByte(Convert.ToInt32(RegDec[i]) % 256);   //Reg Address
                mvars.RS485_WriteDataBuffer[8 + svns + i * 5] = Convert.ToByte(Convert.ToInt32(DataDec[i]) / 256);  //Reg Data
                mvars.RS485_WriteDataBuffer[9 + svns + i * 5] = Convert.ToByte(Convert.ToInt32(DataDec[i]) % 256);  //Reg Data
            }
            funSendMessageTo();
        }

        public static void mhMCUFLASHWRITE(string sFlashAddr, ref byte[] Arr)   // 0x15 Primary 
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            int PacketSize = Arr.Length + 0x0F;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, Arr.Length + 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x15;               //Cmd
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];           //Size
            byte[] flash_addr_arr = StringToByteArray(sFlashAddr.PadLeft(8, '0'));
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x00;               //cmd for IntFlash.
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];  //address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];  //address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];  //address
            byte[] WrArr = BitConverter.GetBytes(Arr.Length);
            mvars.RS485_WriteDataBuffer[9 + svns] = WrArr[1];           //Write Size
            mvars.RS485_WriteDataBuffer[10 + svns] = WrArr[0];          //Write Size
                                                                        //byte[] DataArr = File.ReadAllBytes(textBox41.Text);
            for (UInt16 i = 0; i < Arr.Length; i++)
                mvars.RS485_WriteDataBuffer[11 + svns + i] = Arr[i];    //Data
            funSendMessageTo();
        }

        public static void cUIREGADRwALL(string svstraddr, string[] RegDec, string[] DataDec)
        {
            mp.markreset(999, false);
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) { mvars.sp1.Open(); }

            /*
                Primary MCU Flash 
                0x64000則不分區，所以僅填單側0~39個 = 40bytes
                0x66000也不分區
             */

            int svcounts = RegDec.Length;

            byte[] BinArr = new byte[mvars.GAMMA_SIZE];

            for (UInt16 i = 0; i < RegDec.Length; i++)
            {
                BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(RegDec[i]) / 256);
                BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(RegDec[i]) % 256);
                BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(DataDec[i]) / 256);
                BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(DataDec[i]) % 256);
            }
            //Checksum
            UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
            BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
            BinArr[BinArr.Length - 1] = (byte)(checksum % 256);

            mvars.lblCmd = "MCU_FLASH_W" + svstraddr;
            mhMCUFLASHWRITE(svstraddr, ref BinArr);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; }

            mvars.lblCmd = "MCU_FLASH_R" + svstraddr;
            mhMCUFLASHREAD(svstraddr, BinArr);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
            else
            {
                if (svstraddr == "66000")
                {
                    mvars.lstmcuR66000.Items.Clear();
                    mvars.lstmcuR66000.Items.AddRange(mvars.strR66K.Split('~'));
                }
                else if (svstraddr == "64000")
                {
                    mvars.lstmcuR64000.Items.Clear();
                    mvars.lstmcuR64000.Items.AddRange(mvars.strR64K.Split('~'));
                }
            }

        Ex:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen == false) mvars.sp1.Close();
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (30 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0")));
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - mvars.t1).TotalSeconds.ToString("#0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - mvars.t1).TotalSeconds.ToString("#0"))); }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static void mhMCUFLASHREAD(string sFlashAddr, byte[] Arr)        // 0x14 Primary 
        {
            #region 2023版公用程序 no novastar
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 65 + Arr.Length);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x14;               //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;               //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;               //Size
            byte[] flash_addr_arr = StringToByteArray(sFlashAddr.PadLeft(8, '0'));
            mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];  //address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];  //address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];  //address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];  //address
            byte[] WrArr = BitConverter.GetBytes(Arr.Length);
            mvars.RS485_WriteDataBuffer[9 + svns] = WrArr[1];           //Write Size
            mvars.RS485_WriteDataBuffer[10 + svns] = WrArr[0];          //Write Size
            funSendMessageTo();
        }

        public static void cPGRGB10BITp(byte svtype, byte svip, byte svsen, byte svpo, ushort svca, string svr, string svg, string svb)   //1010
        {
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            //string svsp = "    cPGRGB10BIT,";

            #region USB
            if (svip == 0 && svsen == 0 && svpo == 0) mvars.deviceID = "05A0";
            else
            {
                if (svpo > 0) mvars.deviceID = "05" + mp.DecToHex(Convert.ToInt16(svpo), 2);
            }
            if (svca <= 1) mvars.FPGAsel = (byte)svca;
            else if (svca > 1) mvars.FPGAsel = 2;
            mvars.isReadBack = false;
            if (svtype == 0)
            {
                mvars.FPGAsel = 2;
                int[] svreg = { 1, 48, 49, 50, 51, 52 };         //PC mode
                int[] svdata = { 1, 1023, 1023, 1023, 0, 360 };
                mvars.lblCmd = "FPGA_SPI_W";
                if (mvars.deviceID.Substring(0, 2) == "05") mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                //else mp.mhFPGASPIWRITE(Convert.ToInt32(lblFPGAtxt[Svi].Text));
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
            }
            else if (svtype == 255)
            {
                mvars.FPGAsel = 2;
                int[] svreg = { 1, 48, 49, 50, 51, 52 };         //(1:PG mode)+(48:R)(49:G)(50:B)+(51_PT_Bank)+(52:PG_auto)
                int[] svdata = { 96, 0, 0, 512, 3, 360 };
                mvars.lblCmd = "FPGA_SPI_W";
                if (mvars.deviceID.Substring(0, 2) == "05") mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                //else mp.mhFPGASPIWRITE(Convert.ToInt32(lblFPGAtxt[Svi].Text));
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
            }
            else if (svtype == 2)
            {
                int[] svreg = { 1, 48, 49, 50, 51, 52 };         //(1:PG mode)+(48:R)(49:G)(50:B)+(51_PT_Bank)+(52:PG_auto)
                int[] svdata = { 96, int.Parse(svr), int.Parse(svg), int.Parse(svb), 3, 360 };
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
            }
            else if (svtype == 3)
            {
                int[] svreg = { 1, 51, 52, 53, 54, 55, 56, 57, 58, 59 };    //(1:PG mode)+(51_PT_Bank)+(52:PG_auto)+(53:R1)(54:G1)(55:B1)+(56:X)(57:Y)(58:W)(59:H)
                int svx = 0;
                int svy = 191;
                int svw = 120;
                int svh = 135;
                if (svsen == 1) { svx = 0; svy = 0; mvars.FPGAsel = 1; }
                else if (svsen == 2) { svx = 120; svy = 0; mvars.FPGAsel = 1; }
                else if (svsen == 3) { svx = 240; svy = 0; mvars.FPGAsel = 1; }
                else if (svsen == 4) { svx = 360; svy = 0;  mvars.FPGAsel = 1; }
                else if (svsen == 5) { svx = 0; svy = 0;  mvars.FPGAsel = 0; }
                else if (svsen == 6) { svx = 120; svy = 0;  mvars.FPGAsel = 0; }
                else if (svsen == 7) { svx = 240; svy = 0;  mvars.FPGAsel = 0; }
                else if (svsen == 8) { svx = 360; svy = 0;  mvars.FPGAsel = 0; }
                
                else if (svsen == 9) { svx = 0; svy = 135; mvars.FPGAsel = 1; }
                else if (svsen == 10) { svx = 120; svy = 135; mvars.FPGAsel = 1; }
                else if (svsen == 11) { svx = 240; svy = 135;  mvars.FPGAsel = 1; }
                else if (svsen == 12) { svx = 360; svy = 135;  mvars.FPGAsel = 1; }
                else if (svsen == 13) { svx = 0; svy = 135;  mvars.FPGAsel = 0; }
                else if (svsen == 14) { svx = 120; svy = 135;  mvars.FPGAsel = 0; }
                else if (svsen == 15) { svx = 240; svy = 135;  mvars.FPGAsel = 0; }
                else if (svsen == 16) { svx = 360; svy = 135;  mvars.FPGAsel = 0; }

                else if (svsen == 17) { svx = 0; svy = 270;  mvars.FPGAsel = 1; }
                else if (svsen == 18) { svx = 120; svy = 270;  mvars.FPGAsel = 1; }
                else if (svsen == 19) { svx = 240; svy = 270;  mvars.FPGAsel = 1; }
                else if (svsen == 20) { svx = 360; svy = 270; mvars.FPGAsel = 1; }
                else if (svsen == 21) { svx = 0; svy = 270;  mvars.FPGAsel = 0; }
                else if (svsen == 22) { svx = 120; svy = 270;  mvars.FPGAsel = 0; }
                else if (svsen == 23) { svx = 240; svy = 270;  mvars.FPGAsel = 0; }
                else if (svsen == 24) { svx = 360; svy = 270;  mvars.FPGAsel = 0; }
                
                else if (svsen == 25) { svx = 0; svy = 405;  mvars.FPGAsel = 1; }
                else if (svsen == 26) { svx = 120; svy = 405;  mvars.FPGAsel = 1; }
                else if (svsen == 27) { svx = 240; svy = 405;  mvars.FPGAsel = 1; }
                else if (svsen == 28) { svx = 360; svy = 405;  mvars.FPGAsel = 1; }
                else if (svsen == 29) { svx = 0; svy = 405;  mvars.FPGAsel = 0; }
                else if (svsen == 30) { svx = 120; svy = 405;  mvars.FPGAsel = 0; }
                else if (svsen == 31) { svx = 240; svy = 405;  mvars.FPGAsel = 0; }
                else if (svsen == 32) { svx = 360; svy = 405;  mvars.FPGAsel = 0; }

                int[] svdata = new int[] { 128, 12, 360, int.Parse(svr), int.Parse(svg), int.Parse(svb), svx, svy, svw, svh };
                mvars.lblCmd = "FPGA_SPI_W";
                mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
            }
            #endregion USB


            //ExNovaAGMA:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
                Form1.lstget1.Items.Add("FPGA_SPI_W" + Form1.pvindex + "Fail");
            }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static void cPGRGB10BIT(byte svtype, byte svip, byte svsen, byte svpo, ushort svca, string svr, string svg, string svb)   //1010
        {
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            //string svsp = "    cPGRGB10BIT,";
            byte svFPGAsel = mvars.FPGAsel;

            if (mvars.svnova)
            {
                #region NovaStar


                #endregion NovaStar
            }
            else
            {
                #region USB
                if (svip == 0 && svsen == 0 && svpo == 0) mvars.deviceID = mvars.deviceID.Substring(0,2) +  "A0";
                if (svca == 0) mvars.FPGAsel = 2;

                #region main
                mvars.isReadBack = false;

                if (svtype == 255)
                {
                    if (mvars.deviceID.Substring(0,2) == "05")
                    {
                        int[] svreg = { mvars.FPGA_SI_SEL, mvars.FPGA_PT_SEL, mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B, mvars.FPGA_AG_MOD, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                        int[] svdata = { 1, 257, 512, 512, 512, 0, 767, 0, 539, 0, 0, 0, 0, 0, 1, 0 };
                        mvars.lblCmd = "FPGA_SPI_W";
                        mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                    }
                    else if (mvars.deviceID.Substring(0, 2) == "06")
                    {
                        //                                                          前景R                                                                                                                                背景R                 
                        int[] svreg = { mvars.FPGA_SI_SEL, mvars.FPGA_PT_SEL, mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B, mvars.FPGA_AG_MOD, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                        int[] svdata = { 3, 257, 64, 64, 64, 0, 767, 0, 539, 0, 0, 0, 0, 0, 1, 0 };
                        mvars.lblCmd = "FPGA_SPI_W";
                        mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                    }
                }
                else if (svtype == 2)
                {
                    int[] svreg = { 105, 106, 107, 117, 118, 119 };
                    int[] svdata = { Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb) };
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                }
                else if (svtype == 3)
                {
                    int[] svreg = { mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B };
                    int[] svdata = { 2, Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), 383, 0, 539, 0, 0, 0 };
                    //mvars.lblCmd = "FPGA_SPI_W";
                    //mp.mhFPGASPIWRITE(2, svreg, svdata);
                    int svxs = 0;
                    int svxe = 191;
                    int svys = 0;
                    int svye = 179;
                    svreg = new int[] { mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B };
                    if (svca == 1) { svxs = 0; svxe = 191; svys = 0; svye = 179; mvars.FPGAsel = 0; }
                    else if (svca == 2) { svxs = 192; svxe = 383; svys = 0; svye = 179; mvars.FPGAsel = 0; }
                    else if (svca == 3) { svxs = 0; svxe = 191; svys = 0; svye = 179; mvars.FPGAsel = 1; }
                    else if (svca == 4) { svxs = 192; svxe = 383; svys = 0; svye = 179; mvars.FPGAsel = 1; }
                    else if (svca == 5) { svxs = 0; svxe = 191; svys = 180; svye = 359; mvars.FPGAsel = 0; }
                    else if (svca == 6) { svxs = 192; svxe = 383; svys = 180; svye = 359; mvars.FPGAsel = 0; }
                    else if (svca == 7) { svxs = 0; svxe = 191; svys = 180; svye = 359; mvars.FPGAsel = 1; }
                    else if (svca == 8) { svxs = 192; svxe = 383; svys = 180; svye = 359; mvars.FPGAsel = 1; }
                    else if (svca == 9) { svxs = 0; svxe = 191; svys = 360; svye = 539; mvars.FPGAsel = 0; }
                    else if (svca == 10) { svxs = 192; svxe = 383; svys = 360; svye = 539; mvars.FPGAsel = 0; }
                    else if (svca == 11) { svxs = 0; svxe = 191; svys = 360; svye = 539; mvars.FPGAsel = 1; }
                    else if (svca == 12) { svxs = 192; svxe = 383; svys = 360; svye = 539; mvars.FPGAsel = 1; }
                    svdata = new int[] { Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), svxs, svxe, svys, svye, 0, 0, 0 };
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                }
                else if (svtype == 0)
                {
                    int[] svreg = { 1, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                    int[] svdata = { 0, 0, 1, 0 };
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                }
                if (svtype != 0 && (mvars.deviceID.Substring(0, 2) == "05" || mvars.deviceID.Substring(0, 2) == "06"))
                {
                    mvars.isReadBack = true;
                    mvars.lblCmd = "FPGA_SPI_R";
                    Form1.pvindex = 105;
                    mp.mhFPGASPIREAD();
                    string svstr = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                    Form1.pvindex = 106;
                    mp.mhFPGASPIREAD();
                    svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                    Form1.pvindex = 107;
                    mp.mhFPGASPIREAD();
                    svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                    Form1.pvindex = 117;
                    mp.mhFPGASPIREAD();
                    svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                    Form1.pvindex = 118;
                    mp.mhFPGASPIREAD();
                    svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                    Form1.pvindex = 119;
                    mp.mhFPGASPIREAD();
                    svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                    Form1.lstget1.Items.Add(svstr);
                }
                mvars.isReadBack = false;
                //mp.funSaveLogs(svsp + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",OK");
                #endregion main

                #endregion USB
            }

            //ExNovaAGMA:
            //mvars.nvBoardcast = svnvbc;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
                Form1.lstget1.Items.Add("FPGA_SPI_W" + Form1.pvindex + "Fail");
            }
            mvars.FPGAsel = svFPGAsel;
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static void cPGRGB10BIT(byte svip, byte svsen, byte svpo, ushort svca, string svr, string svg, string svb, string sv1, string sv2, string sv3, string sv4)   //1010
        {
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            //string svsp = "    cPGRGB10BIT,";

            if (mvars.svnova)
            {
                #region NovaStar

                #endregion NovaStar
            }
            else
            {
                #region USB
                if (svip == 0 && svsen == 0 && svpo == 0) mvars.deviceID = mvars.deviceID.Substring(0,2) + "A0";
                else
                {
                    if (svpo > 0) mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svpo), 2);
                }
                if (svca <= 1) mvars.FPGAsel = (byte)svca;
                else if (svca > 1) mvars.FPGAsel = 2;
                mvars.isReadBack = false;



                if (sv2 == "-1" && sv3 == "-1" && sv4 == "-1")
                {
                    int svxs = 0;
                    int svxe = 191;
                    int svys = 0;
                    int svye = 179;
                    int[] svreg = { mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END};
                    if (sv1 == "1") { svxs = 0; svxe = 191; svys = 0; svye = 179; mvars.FPGAsel = 2; }
                    else if (sv1 == "2") { svxs = 192; svxe = 383; svys = 0; svye = 179; mvars.FPGAsel = 2; }
                    else if (sv1 == "3") { svxs = 384; svxe = 575; svys = 0; svye = 179; mvars.FPGAsel = 1; }
                    else if (sv1 == "4") { svxs = 576; svxe = 767; svys = 0; svye = 179; mvars.FPGAsel = 1; }
                    else if (sv1 == "5") { svxs = 0; svxe = 191; svys = 180; svye = 359; mvars.FPGAsel = 2; }
                    else if (sv1 == "6") { svxs = 192; svxe = 383; svys = 180; svye = 359; mvars.FPGAsel = 2; }
                    else if (sv1 == "7") { svxs = 384; svxe = 575; svys = 180; svye = 359; mvars.FPGAsel = 1; }
                    else if (sv1 == "8") { svxs = 576; svxe = 767; svys = 180; svye = 359; mvars.FPGAsel = 1; }
                    else if (sv1 == "9") { svxs = 0; svxe = 191; svys = 360; svye = 539; mvars.FPGAsel = 2; }
                    else if (sv1 == "10") { svxs = 120; svxe = 239; svys = 360; svye = 539; mvars.FPGAsel = 2; }
                    else if (sv1 == "11") { svxs = 384; svxe = 575; svys = 360; svye = 539; mvars.FPGAsel = 1; }
                    else if (sv1 == "12") { svxs = 576; svxe = 767; svys = 360; svye = 539; mvars.FPGAsel = 1; }

                    int[] svdata = {Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), svxs, svxe, svys, svye };

                    //int[] svreg = { mvars.FPGA_SI_SEL, mvars.FPGA_PT_SEL, mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B, mvars.FPGA_AG_MOD, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                    //int[] svdata = { 1, 257, Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), 0, 767, 0, 539, 0, 0, 0, 0, 0, 1, 0 };


                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                }
                else
                {
                    int[] svreg = { 105, 106, 107, 108, 109, 110, 111 };
                    int[] svdata = { Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), Convert.ToInt16(sv1), Convert.ToInt16(sv2), Convert.ToInt16(sv3), Convert.ToInt16(sv4) };
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                }
                #endregion USB
            }

            //ExNovaAGMA:
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
                Form1.lstget1.Items.Add("FPGA_SPI_W" + Form1.pvindex + "Fail");
            }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        public static void cPGRGB10BITt(byte svip, byte svsen, byte svpo, byte svca, byte svlb, string svr, string svg, string svb, string svBr, string svBg, string svBb)   //TV130 mvars.deviceID.Substring(0, 2)=06
        {
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            //string svsp = "    cPGRGB10BIT,";
            byte svFPGAsel = mvars.FPGAsel;

            if (mvars.svnova)
            {
                #region NovaStar


                #endregion NovaStar
            }
            else
            {
                #region USB
                if (svca == 0xA0) 
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                else
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svca), 2);

                if (svlb == 0) 
                    mvars.FPGAsel = 2;       //未指定燈板 = 整面
                else
                    mvars.FPGAsel = Convert.ToByte(svlb % 4 / 2);

                #region main
                mvars.isReadBack = false;

                if (svr == "-1" && svg == "-1" && svb == "-1")
                {
                    int[] svreg = { mvars.FPGA_SI_SEL, mvars.FPGA_PT_SEL, mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B, mvars.FPGA_AG_MOD, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                    int[] svdata = { 3, 257, 64, 64, 64, 0, 767, 0, 539, 0, 0, 0, 0, 0, 1, 0 };
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                }
                else if (svr == "-0" && svg == "-0" && svg == "-0")
                {
                    int[] svreg = { mvars.FPGA_SI_SEL, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG, mvars.FPGA_UD_REG };
                    int[] svdata = { 0, 0, 1, 0 };
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                }
                else
                {
                    if (svlb == 0)          //未指定燈板 = 整面
                    {
                        int[] svreg = { mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B };
                        int[] svdata = { Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), 0, 767, 0, 539, 0, 0, 0 };
                        mvars.lblCmd = "FPGA_SPI_W";
                        mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                    }
                    //else if (svtype == 2)
                    //{
                    //    int[] svreg = { 105, 106, 107, 117, 118, 119 };
                    //    int[] svdata = { Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb) };
                    //    mvars.lblCmd = "FPGA_SPI_W";
                    //    mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                    //}
                    else
                    {
                        int svxs = 0;
                        int svxe = 191;
                        int svys = 0;
                        int svye = 179;
                        int[] svreg = new int[] { mvars.FPGA_GRAY_R, mvars.FPGA_GRAY_G, mvars.FPGA_GRAY_B, mvars.FPGA_X_START, mvars.FPGA_X_END, mvars.FPGA_Y_START, mvars.FPGA_Y_END, mvars.FPGA_BGRL_R, mvars.FPGA_BGRL_G, mvars.FPGA_BGRL_B };
                        if (svlb == 1) { svxs = 0; svxe = 191; svys = 0; svye = 179; mvars.FPGAsel = 0; }
                        else if (svlb == 2) { svxs = 192; svxe = 383; svys = 0; svye = 179; mvars.FPGAsel = 0; }
                        else if (svlb == 3) { svxs = 0; svxe = 191; svys = 0; svye = 179; mvars.FPGAsel = 1; }
                        else if (svlb == 4) { svxs = 192; svxe = 383; svys = 0; svye = 179; mvars.FPGAsel = 1; }
                        else if (svlb == 5) { svxs = 0; svxe = 191; svys = 180; svye = 359; mvars.FPGAsel = 0; }
                        else if (svlb == 6) { svxs = 192; svxe = 383; svys = 180; svye = 359; mvars.FPGAsel = 0; }
                        else if (svlb == 7) { svxs = 0; svxe = 191; svys = 180; svye = 359; mvars.FPGAsel = 1; }
                        else if (svlb == 8) { svxs = 192; svxe = 383; svys = 180; svye = 359; mvars.FPGAsel = 1; }
                        else if (svlb == 9) { svxs = 0; svxe = 191; svys = 360; svye = 539; mvars.FPGAsel = 0; }
                        else if (svlb == 10) { svxs = 192; svxe = 383; svys = 360; svye = 539; mvars.FPGAsel = 0; }
                        else if (svlb == 11) { svxs = 0; svxe = 191; svys = 360; svye = 539; mvars.FPGAsel = 1; }
                        else if (svlb == 12) { svxs = 192; svxe = 383; svys = 360; svye = 539; mvars.FPGAsel = 1; }
                        int[] svdata = new int[] { Convert.ToInt16(svr), Convert.ToInt16(svg), Convert.ToInt16(svb), svxs, svxe, svys, svye, Convert.ToInt16(svBr), Convert.ToInt16(svBg), Convert.ToInt16(svBb) };
                        mvars.lblCmd = "FPGA_SPI_W";
                        mp.mhFPGASPIWRITE(mvars.FPGAsel, svreg, svdata);
                    }
                }


                //mvars.isReadBack = true;
                //mvars.lblCmd = "FPGA_SPI_R";
                //Form1.pvindex = 105;
                //mp.mhFPGASPIREAD();
                //string svstr = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                //Form1.pvindex = 106;
                //mp.mhFPGASPIREAD();
                //svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                //Form1.pvindex = 107;
                //mp.mhFPGASPIREAD();
                //svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                //Form1.pvindex = 117;
                //mp.mhFPGASPIREAD();
                //svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                //Form1.pvindex = 118;
                //mp.mhFPGASPIREAD();
                //svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                //Form1.pvindex = 119;
                //mp.mhFPGASPIREAD();
                //svstr += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                //Form1.lstget1.Items.Add(svstr);
                //mvars.isReadBack = false;


                //mp.funSaveLogs(svsp + mvars._nCommPort + ",ScreenCnt" + Form1.nScreenCnt + ",SenderCnt" + Form1.nSenderCnt + ",OK");
                #endregion main

                #endregion USB
            }

            //ExNovaAGMA:
            //mvars.nvBoardcast = svnvbc;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
                Form1.lstget1.Items.Add("FPGA_SPI_W" + Form1.pvindex + "Fail");
            }
            mvars.FPGAsel = svFPGAsel;
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }




        public static void mIDSERIESMODE()          //0x86
        {
            #region 2023版公用程序
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x86;       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 12;         //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 1;          //Display On
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x12;       //Serial Mode Key
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x34;       //Serial Mode Key
            funSendMessageTo();
        }

        public static void mpIDONOFF(byte svOnOff)          //0x86
        {
            #region 2023版公用程序
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x86;       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 10;         //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = svOnOff;    //Display On
            funSendMessageTo();
        }

        public static void mAUTOID(string svAutoID)     //0x87
        {
            #region 2023版公用程序
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x87;                       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 10;                         //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = Convert.ToByte(svAutoID);   //ID
            funSendMessageTo();
        }

        public static void mWRGETDEVID()     //0x8A
        {
            #region 2023版公用程序
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x8A;   //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0C;   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0xA1;   //Wr mcu parameter cmd.
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x56;   //Key
            mvars.RS485_WriteDataBuffer[7 + svns] = 0xA0;   //Length
            funSendMessageTo();
        }

        public static void mWRDEVID(int svid)     //0x8A
        {
            #region 2023版公用程序
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x8A;   //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D;   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0xA0;   //Wr mcu parameter cmd.
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x55;   //Key
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;   //Length
            mvars.RS485_WriteDataBuffer[8 + svns] = (byte)svid;
            funSendMessageTo();
        }

        public static void cBOXREAD(ListBox lstget1)
        {
            mvars.lblCompose = "BOXREAD";
            mvars.strReceive = "";
            mvars.lCounts = 9999;
            mvars.lCount = 0;
            Array.Resize(ref mvars.lCmd, mvars.lCounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, mvars.lCounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.lCount = 0;
            mvars.errCode = "0";
            mvars.flgDelFB = true;
            mvars.Break = false;
            lstget1.Items.Clear();
            DateTime t1 = DateTime.Now;

            bool svnvBoardcast = mvars.nvBoardcast;
            mvars.nvBoardcast = false;
            string svdeviceID = mvars.deviceID;
            //byte svFPGAsel = mvars.FPGAsel;
            //mvars.FPGAsel = 1;  //Primary FPGA read don't care mvars.FPGAsel

            Form1.lstm.Items.Clear();
            //bool bRes = false;
            for (byte svscr = 1; svscr <= 16; svscr++)
            {
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svscr), 2);

                mvars.lblCmd = "MCU_VERSION";
                mp.mhVersion();

                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                {
                    if (mvars.verMCU.Substring(mvars.verMCU.Length - 5, 1) == "P")
                    {
                        Form1.lstm.Items.Add(mvars.deviceID);
                        //uc_MCU
                        if (mvars.actFunc != "" && mvars.flgSelf == true) lstget1.Items.Add(mvars.deviceID + "," + mvars.verMCU);
                    }
                    else if (mvars.verMCU.Split('-').Length == 4)
                    {
                        Form1.lstm.Items.Add(mvars.deviceID);
                        //uc_MCU
                        if (mvars.actFunc != "" && mvars.flgSelf == true) lstget1.Items.Add(mvars.deviceID + "," + mvars.verMCU);
                    }
                }
                else
                {
                    if (mvars.actFunc != "" && mvars.flgSelf == true)
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Add("X " + mvars.deviceID + ",doesn't exist"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Add("X " + mvars.deviceID + ",不存在"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Add("X " + mvars.deviceID + ",不存在"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Add("X " + mvars.deviceID + ",存在しない"); }
                    }
                    else
                        if (mvars.deviceID.Substring(0, 2) == "05") break;
                }
            }
            if (Form1.lstm.Items.Count > 0)
            {
                List<string> mcuR66000 = new List<string>();
                for (int i = 0; i < Form1.lstm.Items.Count; i++)
                {
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + Form1.lstm.Items[i].ToString().Substring(2, 2);

                    //goto simple;


                    //#region 0x66000 Read (-..8/.9)
                    //mcuR66000.Clear();
                    //mvars.lblCmd = "MCU_FLASH_R66000";
                    //mp.mhMCUFLASHREAD("00066000", 8192);
                    //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) mvars.errCode = "-3";
                    //else
                    //{
                    //    if (mvars.strR66K == "") for (int j = 0; j < 2048; j++) Form1.lstmcuW66000.Items.Add("0,0");
                    //    else Form1.lstmcuR66000.Items.AddRange(mvars.strR66K.Split('~'));
                    //}
                    //#endregion 0x66000 Read



                simple:

                    Form1.pvindex = 0;         //Ver 
                    mvars.lblCmd = "FPGA_SPI_R";
                    mp.mhFPGASPIREAD();
                    string svX = "-1";
                    string svY = "-1";
                    string svW = "-1";
                    string svH = "-1";
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                    {
                        //if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Substring(0, 1) == "6")
                        if (Form1.lstm.Items[i].ToString().Substring(0, 2) == "05")
                        {
                            //v0037 modify
                            //Form1.pvindex = 66;         //X 
                            //mvars.lblCmd = "FPGA_SPI_R";
                            //mp.mhFPGASPIREAD();
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                            //    if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-').Length == 2)
                            //        svX = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-')[1];
                            //Form1.pvindex = 67;         //Y 
                            //mvars.lblCmd = "FPGA_SPI_R";
                            //mp.mhFPGASPIREAD();
                            //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                            //    if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-').Length == 2)
                            //        svY = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-')[1];

                            #region 0x66000 Read (-..8/.9)
                            mcuR66000.Clear();
                            mvars.lblCmd = "MCU_FLASH_R66000";
                            mp.mhMCUFLASHREAD("00066000", 8192);
                            if (mvars.strR66K == "")
                            {
                                mvars.errCode = "-3";
                                mvars.strReceive = "ERROR,AIO_UI v" + mvars.UImajor + @" need MCUFLASH_0X66K content,please check AIO version or redo ""Setting Screen""";
                            }
                            else
                            {
                                svX = mvars.strR66K.Split('~')[8].Split(',')[1];
                                svY = mvars.strR66K.Split('~')[5].Split(',')[1];
                                svW = "960";
                                svH = "540";
                            }
                            #endregion 0x66000 Read


                        }
                        else if (Form1.lstm.Items[i].ToString().Substring(0, 2) == "06")
                        {
                            #region TV130
                            Form1.pvindex = 19;         //W
                            mvars.lblCmd = "FPGA_SPI_R";
                            mp.mhFPGASPIREAD();
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                                if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-').Length == 2)
                                    svW = (Convert.ToInt16(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-')[1]) * 2).ToString();
                            Form1.pvindex = 20;         //H 
                            mvars.lblCmd = "FPGA_SPI_R";
                            mp.mhFPGASPIREAD();
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                                if (mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-').Length == 2)
                                    svH = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1].Split('-')[1];
                            svX = "0";
                            svY = "0";
                            #endregion
                        }
                    }
                    //Form1.lstget1.Items.RemoveAt(i);
                    Form1.lstget1.Items.Add(string.Format("ID.{0},I{1},S{2},P{3},C{4},X{5},Y{6},W{7},H{8},G{9}",
                                mp.HexToDec(mvars.deviceID.Substring(2, 2)),
                                0,
                                0,
                                mp.HexToDec(mvars.deviceID.Substring(2, 2)),
                                0,
                                svX,
                                svY,
                                svW,
                                svH,
                                0));
                    //exsample: https://learn.microsoft.com/zh-tw/dotnet/standard/base-types/trimming
                    if (mvars.actFunc != "" && mvars.flgSelf == true)
                    {
                        string svs = lstget1.Items[i].ToString();
                        lstget1.Items.RemoveAt(i);
                        lstget1.Items.Insert(i, svs + "," + Form1.lstget1.Items[Form1.lstget1.Items.Count - 1].ToString());
                    }
                    else if (mvars.errCode == "-3")
                    {
                        string svs = lstget1.Items[i].ToString();
                        lstget1.Items.RemoveAt(i);
                        Form1.lstget1.Items.Insert(i, string.Format("ID.{0},{1},{2},ID{3},{4},{5},{6},{7},{8},{9}",
                                mp.HexToDec(mvars.deviceID.Substring(2, 2)),
                                "-3",
                                "AIO v0037 need MCUFLASH_0X66K content",
                                "-1",
                                "-1",
                                svX,
                                svY,
                                svW,
                                svH,
                                "-1"));
                        mvars.errCode = "0";
                    }
                    Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;
                }

                //v0037 add
                for (int i = 0; i < Form1.lstm.Items.Count; i++)
                {
                    if (Form1.lstget1.Items[i].ToString().IndexOf("-3") != -1)
                    {
                        mvars.errCode = "-3";
                        break;
                    }
                }
            }
            else
            {
                mvars.errCode = "-1";
                mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - t1).TotalSeconds.ToString("0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - t1).TotalSeconds.ToString("0")));
            }




        exBxRd:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.nvBoardcast = svnvBoardcast;
            mvars.deviceID = svdeviceID;
            //mvars.FPGAsel = svFPGAsel;
            lstget1.Items.Insert(0, MultiLanguage.DefaultLanguage.Split('-')[1]);       //顯示目前語言
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                if (mvars.actFunc != "" && mvars.flgSelf == true) lstget1.Items.Insert(0, "↓BoxRead，" + (DateTime.Now - t1).TotalSeconds.ToString("0.0") + "s");
                mvars.strReceive = "DONE,1,16,0,0,10,0," + (DateTime.Now - t1).TotalSeconds.ToString("0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - t1).TotalSeconds.ToString("0")));
            }
            //else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            lstget1.TopIndex = lstget1.Items.Count - 1;
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static void cUIREGADRON(string sv10bitdec, byte svip, byte svsen, byte svpo, byte svca)   //1010
        {
            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            string svsp = "    cUIREGADRON,";
            string svbit = mp.DecToBin(Convert.ToInt32(sv10bitdec), 7);

            #region Primary
            if (svip == 0 && svsen == 0 && svpo == 0 && svca == 0)
            {
                #region 廣播
                mvars.deviceID = "05A0";
                mvars.FPGAsel = 2;
                #endregion 廣播
            }
            else if (svip == 160 && svsen == 160 && svpo == 160)
            {
                #region 廣播+畫面左側/畫面右側
                mvars.deviceID = "05A0";
                mvars.FPGAsel = svca;
                #endregion 廣播
            }
            else
            {
                #region 指定單屏+單屏/左/右
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + svpo.ToString("00");
                mvars.FPGAsel = svca;
                #endregion 
            }

            for (int svp = 0; svp < svbit.Length; svp++)
            {
                if (svbit.Substring(svp, 1) == "1")
                {
                    //Read
                    Form1.pvindex = 32;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //Addr
                    Form1.pvindex = 33;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svbit.Length - svp - 1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //WData
                    Form1.pvindex = 34;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mhFPGASPIWRITE(mvars.FPGAsel, 1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //Write mode
                    Form1.pvindex = 32;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mhFPGASPIWRITE(mvars.FPGAsel, 0);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                }
            }
            //Read mode
            Form1.pvindex = 32;
            mvars.lblCmd = "FPGA_SPI_W";
            mhFPGASPIWRITE(mvars.FPGAsel, 1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }

            mp.funSaveLogs(svsp + "," + sv10bitdec + ",OK");

            #endregion Primary


            //ExNovaAGMA:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }

            mvars.flgDelFB = false;
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
            }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }

        public static void cUIREGADROFF(string sv10bitdec, byte svip, byte svsen, byte svpo, byte svca)   //1010
        {
            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            string svsp = "    cUIREGADRON,";
            string svbit = mp.DecToBin(Convert.ToInt32(sv10bitdec), 7);

            #region Primary
            if (svip == 0 && svsen == 0 && svpo == 0 && svca == 0)
            {
                #region 廣播
                mvars.deviceID = "05A0";
                mvars.FPGAsel = 2;
                #endregion 廣播
            }
            else if (svip == 160 && svsen == 160 && svpo == 160)
            {
                #region 廣播+畫面左側/畫面右側
                mvars.deviceID = "05A0";
                mvars.FPGAsel = svca;
                #endregion 廣播
            }
            else
            {
                #region 指定單屏+單屏/左/右
                mvars.deviceID = mvars.deviceID.Substring(0,2) + svpo.ToString("00");
                mvars.FPGAsel = svca;
                #endregion 
            }

            for (int svp = 0; svp < svbit.Length; svp++)
            {
                if (svbit.Substring(svp, 1) == "1")
                {
                    //Read
                    Form1.pvindex = 32;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //Addr
                    Form1.pvindex = 33;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, svbit.Length - svp - 1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //WData
                    Form1.pvindex = 34;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mhFPGASPIWRITE(mvars.FPGAsel, 0);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                    //Write mode
                    Form1.pvindex = 32;
                    mvars.lblCmd = "FPGA_SPI_W";
                    mhFPGASPIWRITE(mvars.FPGAsel, 0);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                }
            }
            //Read mode
            Form1.pvindex = 32;
            mvars.lblCmd = "FPGA_SPI_W";
            mhFPGASPIWRITE(mvars.FPGAsel, 1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }

            mp.funSaveLogs(svsp + "," + sv10bitdec + ",OK");

            #endregion Primary


            //ExNovaAGMA:
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
            }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        static string[,] svuiregadrTB = null;
        public static void cENGGMAONWRITEp(byte svact, byte svip, byte svsen, byte svpo, byte svca)
        {
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            List<string> svuiregadr = new List<string>(new string[mvars.GAMMA_SIZE / 4]);
            string[] sRegDec;
            string[] sDataDec;
            string[] svuiuser = new string[40];

            string svdeviceID = mvars.deviceID;

            mvars.isReadBack = true;

            if (Form1.lstm.Items.Count == 0) Form1.lstm.Items.Add(mvars.deviceID.Substring(0, 2) + "01");


            #region Primary
            if (svact == 0)     
            {
                #region ENGGMA,ON
                Form1.lstget1.Items.Clear();
                Form1.lstsvuiregadr.Items.Clear();
                svuiregadrTB = new string[Form1.lstm.Items.Count, mvars.GAMMA_SIZE / 4];
                for (svpo = 0; svpo < Form1.lstm.Items.Count; svpo++)
                {
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + Form1.lstm.Items[svpo].ToString().Substring(2, 2);
                    mvars.FPGAsel = svca;
                    int svms = 91;                                          /// Primary
                    if (mvars.deviceID.Substring(0, 2) == "06") svms = 61;  /// TV130
                    int svme = mvars.uiregadr_default.Length;               /// Primary,TV130
                    if (mvars.dualduty == 0)
                    {
                        for (int svi = 0; svi < mvars.uiregadr_default.Length; svi++)
                        {
                            if (mvars.uiregadr_default[svi].IndexOf("WT_GMA", 0) != -1) { svms = svi; break; }
                        }
                        if (svsen == 160)
                        {
                            //單屏
                            for (int svi = svms; svi < mvars.uiregadr_default.Length; svi++)
                            {
                                if (mvars.uiregadr_default[svi].IndexOf("IDLE", 0) != -1) { svme = svi; break; }
                            }
                        }
                        else
                        {
                            //條屏
                        }
                    }
                    mvars.lblCmd = "UIREGRAD_READ";
                    mp.mUIREGARDRm(0, svme);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                    else
                    {
                        for (int i = 0; i < svme; i++)
                        {
                            Form1.svuiregadr[i] = (mvars.ReadDataBuffer[6 + i * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + 1]).ToString();
                            Form1.svuiregadr[i + 1024] = (mvars.ReadDataBuffer[6 + i * 2 + svme * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + svme * 2 + 1]).ToString();
                            svuiregadr[i] = (mvars.ReadDataBuffer[6 + i * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + 1]).ToString();
                            svuiregadr[i + 1024] = (mvars.ReadDataBuffer[6 + i * 2 + svme * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + svme * 2 + 1]).ToString();
                            svuiregadrTB[svpo, i] = (mvars.ReadDataBuffer[6 + i * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + 1]).ToString();
                            svuiregadrTB[svpo, i + 1024] = (mvars.ReadDataBuffer[6 + i * 2 + svme * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + svme * 2 + 1]).ToString();
                        }
                        Form1.lstsvuiregadr.Items.Clear();
                        for (int i = 0; i < Form1.svuiregadr.Count; i++)
                        {
                            if (Form1.svuiregadr[i] != null) Form1.lstsvuiregadr.Items.Add(Form1.svuiregadr[i]);
                            else Form1.lstsvuiregadr.Items.Add(" ");
                        }



                        Form1.lstget1.Items.Add(string.Format("No.{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                                                (svpo + 1).ToString(),
                                                Form1.lstsvuiregadr.Items[91].ToString(),
                                                Form1.lstsvuiregadr.Items[92].ToString(),
                                                Form1.lstsvuiregadr.Items[93].ToString(),
                                                Form1.lstsvuiregadr.Items[94].ToString(),
                                                Form1.lstsvuiregadr.Items[95].ToString(),
                                                Form1.lstsvuiregadr.Items[96].ToString(),
                                                Form1.lstsvuiregadr.Items[97].ToString(),
                                                Form1.lstsvuiregadr.Items[98].ToString(),
                                                Form1.lstsvuiregadr.Items[99].ToString(),
                                                Form1.lstsvuiregadr.Items[100].ToString(),
                                                Form1.lstsvuiregadr.Items[101].ToString(),
                                                Form1.lstsvuiregadr.Items[102].ToString()));

                        #region 還原 Drop (0523 disabled)
                        //if (svca == 2)
                        //{
                        //    for (byte svn = 0; svn < 2; svn++)
                        //    {
                        //        int svj = (int)(mvars.GAMMA_SIZE / 8 * svn);
                        //        for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                        //            Form1.svuiregadr[i + svj] = mvars.uiregadr_default[i].Split(',')[2];
                        //        Form1.svuiregadr[5 + svj] = "1";
                        //        sRegDec = new string[svme];   //addr
                        //        sDataDec = new string[svme];  //data
                        //        for (int i = 0; i < svme; i++)
                        //        {
                        //            sRegDec[i] = i.ToString();
                        //            sDataDec[i] = Form1.svuiregadr[i + svj];
                        //        }
                        //        mvars.lblCmd = "FPGA_REG_W" + svn;
                        //        mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                        //    }
                        //}
                        //else
                        //{
                        //    //單屏右側(svca=0)左側(svca=1)還原預設值
                        //    int svj = (int)(mvars.GAMMA_SIZE / 8 * svca);
                        //    for (int i = 0; i < mvars.uiregadr_default.Length; i++)
                        //        Form1.svuiregadr[i + svj] = mvars.uiregadr_default[i].Split(',')[2];
                        //    Form1.svuiregadr[5 + svj] = "1";
                        //    sRegDec = new string[svme];   //addr
                        //    sDataDec = new string[svme];  //data
                        //    for (int i = 0; i < svme; i++)
                        //    {
                        //        sRegDec[i] = i.ToString();
                        //        sDataDec[i] = Form1.svuiregadr[i + svj];
                        //    }
                        //    mvars.lblCmd = "FPGA_REG_W" + svca;
                        //    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                        //}
                        #endregion 還原 Drop

                        #region ENG_GMA_EN OFF
                        //Read mode
                        Form1.pvindex = 32;
                        mvars.lblCmd = "FPGA_SPI_W";
                        mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                        //Addr
                        Form1.pvindex = 33;
                        mvars.lblCmd = "FPGA_SPI_W";
                        mp.mhFPGASPIWRITE(mvars.FPGAsel, 5);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                        //WData
                        Form1.pvindex = 34;
                        mvars.lblCmd = "FPGA_SPI_W";
                        mhFPGASPIWRITE(mvars.FPGAsel, 0);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                        //Write mode
                        Form1.pvindex = 32;
                        mvars.lblCmd = "FPGA_SPI_W";
                        mhFPGASPIWRITE(mvars.FPGAsel, 0);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }

                        //Read mode
                        Form1.pvindex = 32;
                        mvars.lblCmd = "FPGA_SPI_W";
                        mp.mhFPGASPIWRITE(mvars.FPGAsel, 1);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-" + Form1.pvindex; }
                        #endregion ENG_GMA_EN OFF
                    }
                    for (int svi = 0; svi < svuiregadr.Count; svi++) Form1.svuiregadr[svi] = svuiregadr[svi];
                }
                #endregion ENGGMA,ON
            }
            else if (svact == 1)
            {
                #region ENGGMA
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svpo), 2);
                mvars.FPGAsel = svca;
                int svms = 91;                              /// Primary
                int svme = mvars.uiregadr_default.Length;   /// Primary
                if (mvars.dualduty == 0)
                {
                    for (int svi = 0; svi < mvars.uiregadr_default.Length; svi++)
                    {
                        if (mvars.uiregadr_default[svi].IndexOf("WT_GMA", 0) != -1) { svms = svi; break; }
                    }
                    //單屏
                    for (int svi = svms; svi < mvars.uiregadr_default.Length; svi++)
                    {
                        if (mvars.uiregadr_default[svi].IndexOf("IDLE", 0) != -1) { svme = svi; break; }
                    }
                }
                if (svca == 2)
                {
                    for (byte svn = 0; svn < 2; svn++)
                    {
                        int svj = (int)(mvars.GAMMA_SIZE / 8 * svn);
                        //在 Form1.tmePull已處理 Form1.svuiregadr 內容
                        Form1.svuiregadr[5 + svj] = "1";
                        //svuiregadrTB[svpo-1, 5 + svj] = Form1.svuiregadr[5 + svj];
                        sRegDec = new string[svme];   //addr
                        sDataDec = new string[svme];  //data
                        for (int i = 0; i < svme; i++)
                        {
                            sRegDec[i] = i.ToString();
                            sDataDec[i] = Form1.svuiregadr[i + svj];
                            //svuiregadrTB[svpo-1, i + svj] = Form1.svuiregadr[5 + svj];
                        }
                        mvars.lblCmd = "FPGA_REG_W" + svn;
                        mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                    }
                }
                else
                {
                    //單屏右側(svca=0)左側(svca=1)還原預設值
                    int svj = (int)(mvars.GAMMA_SIZE / 8 * svca);
                    //在 Form1.tmePull已處理 Form1.svuiregadr 內容
                    Form1.svuiregadr[5 + svj] = "1";
                    //svuiregadrTB[svpo-1, 5 + svj] = Form1.svuiregadr[5 + svj];
                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = i.ToString();
                        sDataDec[i] = Form1.svuiregadr[i + svj];
                        //svuiregadrTB[svpo-1, i + svj] = Form1.svuiregadr[5 + svj];
                    }
                    mvars.lblCmd = "FPGA_REG_W" + svca;
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                #endregion ENGGMA
            }
            else if (svact == 2)
            {
                #region ENGGMA,WRITE
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svpo), 2);
                mvars.FPGAsel = svca;
                byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                int svms = 91;      /// Primary
                int svme = 283;     /// Primary
                if (mvars.dualduty == 1) svme = mvars.uiregadr_default.Length;
                mvars.lblCmd = "UIREGRAD_READ";
                mp.mUIREGARDRm(0, svme);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                else
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
                    }
                    for (int svi = 0; svi < svme; svi++)
                    {
                        Form1.svuiregadr[svi] = (mvars.ReadDataBuffer[6 + svi * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + svi * 2 + 1]).ToString();
                        Form1.svuiregadr[svi + 1024] = (mvars.ReadDataBuffer[6 + svi * 2 + svme * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + svi * 2 + svme * 2 + 1]).ToString();
                        svuiregadrTB[svpo - 1, svi] = Form1.svuiregadr[svi];
                        svuiregadrTB[svpo - 1, svi + 1024] = Form1.svuiregadr[svi + 1024];
                    }
                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data

                    //左屏,右屏,全屏
                    if (svca == 2)
                    {
                        for (byte svn = 0; svn < svca; svn++)
                        {
                            int svj = (int)(mvars.GAMMA_SIZE / 8 * svn);
                            Form1.svuiregadr[5 + svj] = "1";
                            for (int i = 0; i < svme; i++)
                            {
                                //sRegDec[i] = i.ToString();
                                //sDataDec[i] = Form1.svuiregadr[i + svj];
                                BinArr[i * 4 + 0 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) / 256);
                                BinArr[i * 4 + 1 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) % 256);
                                BinArr[i * 4 + 2 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) / 256);
                                BinArr[i * 4 + 3 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) % 256);
                            }
                            //mvars.lblCmd = "FPGA_REG_W" + svn;
                            //mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                        }
                        //Checksum
                        UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                        BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                        BinArr[BinArr.Length - 1] = (byte)(checksum % 256);
                        //Save File
                        //string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr.bin";
                        //SaveBinFile(path, BinArr);
                        mvars.lblCmd = "MCU_FLASH_W62000";
                        mhMCUFLASHWRITE("62000", ref BinArr);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                    }
                    else
                    {
                        //單屏右側(svca=0)左側(svca=1)還原預設值
                        int svj = (int)(mvars.GAMMA_SIZE / 8 * svca);
                        Form1.svuiregadr[5 + svj] = "1";
                        if (svca == 0)
                        {
                            for (int i = 0; i < svme; i++)
                            {
                                BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(i) / 256);
                                BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(i) % 256);
                                BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i]) / 256);
                                BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i]) % 256);
                            }
                        }
                        else if (svca == 1)
                        {
                            for (int i = 0; i < svme; i++)
                            {
                                BinArr[i * 4 + 0 + (mvars.GAMMA_SIZE / 2 * svca)] = (Byte)(Convert.ToInt32(i) / 256);
                                BinArr[i * 4 + 1 + (mvars.GAMMA_SIZE / 2 * svca)] = (Byte)(Convert.ToInt32(i) % 256);
                                BinArr[i * 4 + 2 + (mvars.GAMMA_SIZE / 2 * svca)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) / 256);
                                BinArr[i * 4 + 3 + (mvars.GAMMA_SIZE / 2 * svca)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) % 256);
                            }
                        }
                        //Checksum
                        UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                        BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                        BinArr[BinArr.Length - 1] = (byte)(checksum % 256);

                        //Save File
                        //string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr.bin";
                        //SaveBinFile(path, BinArr);
                        mvars.lblCmd = "MCU_FLASH_W62000";
                        mhMCUFLASHWRITE("62000", ref BinArr);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                    }


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
                            for (int svi = 0; svi < svuiuser.Length; svi++)
                            {
                                svuiuser[svi] = lst[svi].Split(',')[1];
                            }
                            byte svFMgmaENreg = 5;
                            svuiuser[svFMgmaENreg] = "1";

                            for (UInt16 i = 0; i < svuiuser.Length; i++)
                            {
                                BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(i) / 256);
                                BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(i) % 256);
                                BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(svuiuser[i]) / 256);
                                BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(svuiuser[i]) % 256);
                            }

                            //Checksum
                            UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                            BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                            BinArr[BinArr.Length - 1] = (byte)(checksum % 256);

                            //Save File
                            //path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr.bin";
                            //SaveBinFile(path, BinArr);

                            mvars.lblCmd = "MCU_FLASH_W64000";
                            mhMCUFLASHWRITE("64000", ref BinArr);
                            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                        }
                        //else lbl_mcuR64000click.Text = "no record";
                        //lbl_mcuR64000click.Text = "< ";
                        //Form1.tslblStatus.Text = "0x64000 rd MCU Flash items：" + (mvars.lstmcuR64000.Items.Count).ToString();
                    }
                    #endregion 回讀 0x64000



                    //0x66000 Read (-..8/.9)
                    Form1.lstmcuR66000.Items.Clear();
                    mvars.lblCmd = "MCU_FLASH_R66000";
                    mp.mhMCUFLASHREAD("00066000", 8192);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) mvars.errCode = "-3";
                    else
                    {
                        if (mvars.strR66K == "") for (int j = 0; j < 2048; j++) Form1.lstmcuW66000.Items.Add("0,0");
                        else Form1.lstmcuW66000.Items.AddRange(mvars.strR66K.Split('~'));
                    }

                    for (int i = svms; i < svms + 12; i++)
                    {
                        Form1.lstmcuW66000.Items.RemoveAt(i - 60);
                        Form1.lstmcuW66000.Items.Insert(i - 60, i.ToString() + "," + Form1.svuiregadr[i]);
                    }
                    Array.Resize(ref BinArr, (int)mvars.GAMMA_SIZE);
                    for (int svi = 0; svi < mvars.GAMMA_SIZE / 4; svi++)
                    {
                        BinArr[svi * 4 + 0] = (Byte)(Convert.ToInt32(Form1.lstmcuW66000.Items[svi].ToString().Split(',')[0]) / 256);
                        BinArr[svi * 4 + 1] = (Byte)(Convert.ToInt32(Form1.lstmcuW66000.Items[svi].ToString().Split(',')[0]) % 256);
                        BinArr[svi * 4 + 2] = (Byte)(Convert.ToInt32(Form1.lstmcuW66000.Items[svi].ToString().Split(',')[1]) / 256);
                        BinArr[svi * 4 + 3] = (Byte)(Convert.ToInt32(Form1.lstmcuW66000.Items[svi].ToString().Split(',')[1]) % 256);
                    }
                    if (mvars.demoMode)
                    {
                        mvars.lblCmd = "MCU_FLASH_W66000";
                        mvars.lCount++;
                        mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                        mp.doDelayms(1000);
                        mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    }
                    else
                    {
                        mvars.lblCmd = "MCU_FLASH_W66000";
                        mp.mhMCUFLASHWRITE("66000", ref BinArr);
                        //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9." + (i + 1).ToString() + ".6"; goto eX; }
                    }


                }





                #endregion ENGGMA,WRITE
            }
            else if (svact == 3)
            {
                #region ENGGMA OFF
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svpo), 2);
                mvars.FPGAsel = svca;
                int svms = 91;                              /// Primary
                int svme = mvars.uiregadr_default.Length;   /// Primary
                if (mvars.dualduty == 0)
                {
                    for (int svi = 0; svi < mvars.uiregadr_default.Length; svi++)
                    {
                        if (mvars.uiregadr_default[svi].IndexOf("WT_GMA", 0) != -1) { svms = svi; break; }
                    }
                    //單屏
                    for (int svi = svms; svi < mvars.uiregadr_default.Length; svi++)
                    {
                        if (mvars.uiregadr_default[svi].IndexOf("IDLE", 0) != -1) { svme = svi; break; }
                    }
                }
                for (svpo = 0; svpo < Form1.lstm.Items.Count; svpo++)
                {
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + Form1.lstm.Items[svpo].ToString().Substring(2, 2);
                    mvars.FPGAsel = svca;

                    for (int i = 0; i < svme; i++)
                    {
                        Form1.svuiregadr[i] = svuiregadrTB[svpo, i];
                        Form1.svuiregadr[i + 1024] = svuiregadrTB[svpo, i + 1024];
                    }
                    //Form1.lstsvuiregadr.Items.Clear();
                    //for (int i = 0; i < Form1.svuiregadr.Count; i++)
                    //{
                    //    if (Form1.svuiregadr[i] != null) Form1.lstsvuiregadr.Items.Add(Form1.svuiregadr[i]);
                    //    else Form1.lstsvuiregadr.Items.Add(" ");
                    //}
                    #region 取自 ENG_GMA
                    if (svca == 2)
                    {
                        for (byte svn = 0; svn < 2; svn++)
                        {
                            int svj = (int)(mvars.GAMMA_SIZE / 8 * svn);
                            Form1.svuiregadr[5 + svj] = "1";
                            sRegDec = new string[svme];   //addr
                            sDataDec = new string[svme];  //data
                            for (int i = 0; i < svme; i++)
                            {
                                sRegDec[i] = i.ToString();
                                sDataDec[i] = Form1.svuiregadr[i + svj];
                            }
                            mvars.lblCmd = "FPGA_REG_W" + svn;
                            mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                        }
                    }
                    else
                    {
                        //單屏右側(svca=0)左側(svca=1)還原預設值
                        int svj = (int)(mvars.GAMMA_SIZE / 8 * svca);
                        Form1.svuiregadr[5 + svj] = "1";
                        sRegDec = new string[svme];   //addr
                        sDataDec = new string[svme];  //data
                        for (int i = 0; i < svme; i++)
                        {
                            sRegDec[i] = i.ToString();
                            sDataDec[i] = Form1.svuiregadr[i + svj];
                        }
                        mvars.lblCmd = "FPGA_REG_W" + svca;
                        mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                    }
                    #endregion 取自ENG_GMA
                }
                #endregion ENGGMA OFF
            }
            else if (svip == 160 && svsen == 160 && svpo == 160)
            {
                #region 廣播+畫面左側/畫面右側
                mvars.deviceID = "05A0";
                mvars.FPGAsel = svca;
                #endregion 廣播
            }
            else
            {
                #region 指定單屏+單屏/左/右
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svpo), 2);
                mvars.FPGAsel = svca;
                #endregion
            }


        #endregion Primary

        ExNovaAGMA:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.deviceID = svdeviceID;
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30";
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        static List<string> svuiregadr = new List<string>(new string[mvars.GAMMA_SIZE / 4]);
        public static void cENGGMAONWRITEt(byte svact, byte svip, byte svsen, byte svpo, byte svca, byte svlb)
        {
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            //List<string> svuiregadr = new List<string>(new string[mvars.GAMMA_SIZE / 4]);
            string[] sRegDec;
            string[] sDataDec;
            string[] svuiuser = new string[40];

            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;

            mvars.isReadBack = true;

            svlb--;

            #region TV130
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svca, 2);
            mvars.FPGAsel = Convert.ToByte(svlb % 4 / 2);
            if (svact == 0)
            {
                #region ENGGMA,ON (無還原Drop流程)
                Form1.lstget1.Items.Clear();
                Form1.lstsvuiregadr.Items.Clear();
                svuiregadrTB = new string[1, mvars.GAMMA_SIZE / 4];                                     
                //int svms = 61;                                          /// TV130
                int svme = mvars.uiregadr_default.Length;               /// TV130
                mvars.lblCmd = "UIREGRAD_READ";
                mp.mUIREGARDRm(0, svme);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                else
                {
                    for (int i = 0; i < svme; i++)
                    {
                        //使用 Form1.svuiregadr 做即時微調
                        Form1.svuiregadr[i] = (mvars.ReadDataBuffer[6 + i * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + 1]).ToString();
                        Form1.svuiregadr[i + 1024] = (mvars.ReadDataBuffer[6 + i * 2 + svme * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + svme * 2 + 1]).ToString();
                        
                        svuiregadr[i] = Form1.svuiregadr[i];                            // 上一次FIX的紀錄值
                        svuiregadr[i + 1024] = Form1.svuiregadr[i + 1024];              // 上一次FIX的紀錄值

                        svuiregadrTB[0, i] = Form1.svuiregadr[i];                    // 最初始值
                        svuiregadrTB[0, i + 1024] = Form1.svuiregadr[i + 1024];      // 最初始值
                    }
                    Form1.lstsvuiregadr.Items.Clear();
                    for (int i = 0; i < Form1.svuiregadr.Count; i++)
                    {
                        if (Form1.svuiregadr[i] != null) Form1.lstsvuiregadr.Items.Add(Form1.svuiregadr[i]);
                        else Form1.lstsvuiregadr.Items.Add(" ");
                    }


                    //Form1.lstget1.Items.Add(string.Format("No.{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                    //    svca.ToString(),
                    //    Form1.lstsvuiregadr.Items[61 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[62 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[63 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[64 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[65 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[66 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[67 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[68 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[69 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[70 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[71 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString(),
                    //    Form1.lstsvuiregadr.Items[72 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12].ToString()));

                    //無還原Drop流程

                    //svms = 61 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12;    /// TV130
                    //svme = 72 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12;    /// TV130
                    sRegDec = new string[13];   //addr
                    sDataDec = new string[13];  //data
                    for (int i = 0; i < 12; i++)
                    {
                        sRegDec[i] = (i + 61 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12).ToString();
                    }
                    sRegDec[12] = "5";

                    sDataDec[0] = "512";
                    sDataDec[1] = "1024";
                    sDataDec[2] = "2048";
                    sDataDec[3] = "4080";
                    sDataDec[4] = "512";
                    sDataDec[5] = "1024";
                    sDataDec[6] = "2048";
                    sDataDec[7] = "4080";
                    sDataDec[8] = "512";
                    sDataDec[9] = "1024";
                    sDataDec[10] = "2048";
                    sDataDec[11] = "4080";

                    //sDataDec[0] = "256";
                    //sDataDec[1] = "512";
                    //sDataDec[2] = "1024";
                    //sDataDec[3] = "2040";
                    //sDataDec[4] = "256";
                    //sDataDec[5] = "512";
                    //sDataDec[6] = "1024";
                    //sDataDec[7] = "2040";
                    //sDataDec[8] = "256";
                    //sDataDec[9] = "512";
                    //sDataDec[10] = "1024";
                    //sDataDec[11] = "2040";

                    sDataDec[12] = "1";

                    mvars.lblCmd = "FPGA_REG_W";
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                for (int svi = 0; svi < svuiregadr.Count; svi++) Form1.svuiregadr[svi] = svuiregadr[svi];
                #endregion ENGGMA,ON
            }
            else if (svact == 1)
            {
                #region ENGGMA 微調
                //for (byte svFPGAsel = 0; svFPGAsel < 2; svFPGAsel++)
                //{
                //int svj = (int)(mvars.GAMMA_SIZE / 8 * svFPGAsel);
                    int svms = 61 + (svlb % 4 - mvars.FPGAsel * 2) * 36 + svlb / 4 * 12 ;    /// TV130
                    sRegDec = new string[12];   //addr
                    sDataDec = new string[12];  //data
                    for (int i = 0; i < 12; i++)
                    {
                        sRegDec[i] = svms.ToString();
                        sDataDec[i] = Form1.svuiregadr[svms + mvars.FPGAsel * 1024];
                        svms++;
                    }
                mvars.lblCmd = "FPGA_REG_W_FPGAsel" + mvars.FPGAsel;
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                //}
                #endregion ENGGMA 微調
            }
            else if (svact == 2)
            {
                #region ENGGMA,FIX
                byte[] BinArr = new byte[mvars.GAMMA_SIZE];

                //int svms = 61;                                /// TV130
                int svme = mvars.uiregadr_default.Length;     /// TV130
                for (byte svn = 0; svn <= 1; svn++)
                {
                    int svj = 1024 * svn;
                    Form1.svuiregadr[5 + svj] = "1";
                    for (int i = 0; i <= svme; i++)
                    {
                        svuiregadr[i + svj] = Form1.svuiregadr[i + svj];                            // FIX的紀錄值

                        BinArr[i * 4 + 0 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) / 256);
                        BinArr[i * 4 + 1 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) % 256);
                        BinArr[i * 4 + 2 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) / 256);
                        BinArr[i * 4 + 3 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) % 256);
                    }
                    //mvars.lblCmd = "FPGA_REG_W" + svn;
                    //mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                //Checksum
                UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                BinArr[BinArr.Length - 1] = (byte)(checksum % 256);
                //Save File
                //string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\ENGGMAFIX.bin";
                //SaveBinFile(path, BinArr);
                mvars.lblCmd = "MCU_FLASH_W62000";
                mhMCUFLASHWRITE("62000", ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }

                #region 回讀 0x64000 (Disabled)
                //mvars.lblCmd = "MCU_FLASH_R64000";
                //mp.mhMCUFLASHREAD("00064000", 8192);
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                //{
                //    mvars.errCode = "-3";
                //    Form1.tslblStatus.Text = "0x64000 rd MCU Flash fail";
                //}
                //else
                //{
                //    if (mvars.strR64K.Length > 1)
                //    {
                //        //List<string> lst = new List<string>(new string[mvars.strR64K.Split('~').Length]);
                //        List<string> lst = new List<string>();
                //        lst.AddRange(mvars.strR64K.Split('~'));
                //        for (int svi = 0; svi < svuiuser.Length; svi++)
                //        {
                //            svuiuser[svi] = lst[svi].Split(',')[1];
                //        }
                //        byte svFMgmaENreg = 5;
                //        svuiuser[svFMgmaENreg] = "1";

                //        for (UInt16 i = 0; i < svuiuser.Length; i++)
                //        {
                //            BinArr[i * 4 + 0] = (Byte)(Convert.ToInt32(i) / 256);
                //            BinArr[i * 4 + 1] = (Byte)(Convert.ToInt32(i) % 256);
                //            BinArr[i * 4 + 2] = (Byte)(Convert.ToInt32(svuiuser[i]) / 256);
                //            BinArr[i * 4 + 3] = (Byte)(Convert.ToInt32(svuiuser[i]) % 256);
                //        }

                //        //Checksum
                //        checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                //        BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                //        BinArr[BinArr.Length - 1] = (byte)(checksum % 256);

                //        //Save File
                //        //path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr.bin";
                //        //SaveBinFile(path, BinArr);

                //        mvars.lblCmd = "MCU_FLASH_W64000";
                //        mhMCUFLASHWRITE("64000", ref BinArr);
                //        if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; }
                //    }
                //}
                #endregion 回讀 0x64000

                #endregion ENGGMA,WRITE
            }
            else if (svact == 3 || svact == 4)
            {
                #region ENGGMA OFF
                int svme = mvars.uiregadr_default.Length;     /// TV130
                if (svact == 3)
                {
                    //回復原始設定值
                    for (int i = 0; i <= svme; i++)
                    {
                        //使用 Form1.svuiregadr 做即時微調
                        Form1.svuiregadr[i] = svuiregadrTB[0, i];
                        Form1.svuiregadr[i + 1024] = svuiregadrTB[0, i + 1024];

                        svuiregadr[i] = svuiregadrTB[0, i];
                        svuiregadr[i + 1024] = svuiregadrTB[0, i];
                    }
                }
                else if (svact == 4)
                {
                    //回復上一次寫入的設定值
                    for (int i = 0; i <= svme; i++)
                    {
                        //使用 Form1.svuiregadr 做即時微調
                        Form1.svuiregadr[i] = svuiregadr[i];
                        Form1.svuiregadr[i + 1024] = svuiregadr[i + 1024];
                    }
                }

                sRegDec = new string[svme];   //addr
                sDataDec = new string[svme];  //data

                for(byte j = 0; j <= 1; j++)
                {
                    mvars.FPGAsel = j;
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = i.ToString();
                        sDataDec[i] = Form1.svuiregadr[i + mvars.FPGAsel * 1024];
                    }
                    mvars.lblCmd = "FPGA_REG_W" + j;
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }

                
                #endregion ENGGMA OFF
            }


        #endregion TV130

        ExNovaAGMA:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30";
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }




        public static void mpMCUBRIGPERCENT(byte svPercent, bool svMTP, bool svASC)       //0x93 MCU Brightness
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x93;           //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0E;           //Size

            mvars.RS485_WriteDataBuffer[5 + svns] = svPercent;      //Brightness percent

            if (svMTP) mvars.RS485_WriteDataBuffer[6 + svns] = 1; else mvars.RS485_WriteDataBuffer[6 + svns] = 0;
            if (svASC) mvars.RS485_WriteDataBuffer[7 + svns] = 1; else mvars.RS485_WriteDataBuffer[7 + svns] = 0;
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x12;
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x34;
            funSendMessageTo();
        }


        public static void mpMCUCCT(int svCCT, bool svMTP, bool svASC)       //0x92 MCUCCT
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x92;           //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0B;           //Size

            mvars.RS485_WriteDataBuffer[5 + svns] = Convert.ToByte(svCCT / 256);     //CCT
            mvars.RS485_WriteDataBuffer[6 + svns] = Convert.ToByte(svCCT % 256);     //CCT
            funSendMessageTo();
        }


        public static void cDROPp(int svgray, byte svip, byte svsen, byte svpo, byte svca)
        {
            //主體從cENGGMAONWRITEp複製，暫時取消 svsen==160 的判斷式
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            List<string> svuiregadr = new List<string>(new string[mvars.GAMMA_SIZE / 4]);
            string[] sRegDec;
            string[] sDataDec;
            string[] svuiuser = new string[40];
            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;
            int svms = 91;                              /// Primary
            int svme = mvars.uiregadr_default.Length;   /// Primary
            mvars.isReadBack = true;

            #region Primary
            if (svip == 160 && svsen == 160 && svpo == 160)
            {
                #region 廣播+畫面左側/畫面右側

                #endregion 廣播
            }
            else
            {
                for (int svi = 0; svi < mvars.uiregadr_default.Length; svi++)
                {
                    if (mvars.uiregadr_default[svi].IndexOf("WT_GMA", 0) != -1) { svms = svi; break; }
                }
                for (int svi = svms; svi < mvars.uiregadr_default.Length; svi++)
                {
                    if (mvars.uiregadr_default[svi].IndexOf("IDLE", 0) != -1) { svme = svi; break; }
                }
                //先回讀 UIREGARD 的現況
                mvars.lblCmd = "UIREGRAD_READ";
                mp.mUIREGARDRm(0, svme);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-1"; goto ExNovaAGMA; }
                for (int i = 0; i < svme; i++)
                {
                    Form1.svuiregadr[i] = (mvars.ReadDataBuffer[6 + i * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + 1]).ToString();
                    Form1.svuiregadr[i + 1024] = (mvars.ReadDataBuffer[6 + i * 2 + svme * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + svme * 2 + 1]).ToString();
                    svuiregadr[i] = (mvars.ReadDataBuffer[6 + i * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + 1]).ToString();
                    svuiregadr[i + 1024] = (mvars.ReadDataBuffer[6 + i * 2 + svme * 2 + 1] * 256 + mvars.ReadDataBuffer[7 + i * 2 + svme * 2 + 1]).ToString();
                }
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svpo, 2); ;
                mvars.FPGAsel = svca;
                string[] RegDec = new string[mvars.uiregadr_default.Length];
                string[] DataDec = new string[mvars.uiregadr_default.Length];
                mvars.lblCmd = "FPGA_REG_W";
                //還原 UIREGADR Default
                for (int svj = 0; svj < mvars.uiregadr_default.Length; svj++)
                {
                    RegDec[svj] = svj.ToString();
                    DataDec[svj] = mvars.uiregadr_default[svj].Split(',')[2];
                }
                mp.mpFPGAUIREGWarr(RegDec, DataDec);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; goto ExNovaAGMA; }
                byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                //清空 Flash 0x62000 & 0x64000
                mvars.lblCmd = "MCUFLASH_W62000";
                mp.mhMCUFLASHWRITE("00062000", ref BinArr);
                mvars.lblCmd = "MCUFLASH_W64000";
                mp.mhMCUFLASHWRITE("00064000", ref BinArr);
                //寫入 Drop 的計算值
                for (byte svn = 0; svn < 2; svn++)
                {
                    int svj = (int)(mvars.GAMMA_SIZE / 8 * svn);
                    for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                        Form1.svuiregadr[i + svj] = ((int)(Convert.ToSingle(mvars.uiregadr_default[i].Split(',')[2]) * svgray / 255)).ToString();
                    Form1.svuiregadr[5 + svj] = "1";
                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        sRegDec[i] = i.ToString();
                        sDataDec[i] = Form1.svuiregadr[i + svj];
                    }
                    mvars.lblCmd = "FPGA_REG_W" + svn;
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                for (byte svn = 0; svn < svca; svn++)
                {
                    int svj = (int)(mvars.GAMMA_SIZE / 8 * svn);
                    Form1.svuiregadr[5 + svj] = "1";
                    for (int i = 0; i < svme; i++)
                    {
                        //sRegDec[i] = i.ToString();
                        //sDataDec[i] = Form1.svuiregadr[i + svj];
                        BinArr[i * 4 + 0 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) / 256);
                        BinArr[i * 4 + 1 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) % 256);
                        BinArr[i * 4 + 2 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) / 256);
                        BinArr[i * 4 + 3 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) % 256);
                    }
                    //mvars.lblCmd = "FPGA_REG_W" + svn;
                    //mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                //Checksum
                UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                BinArr[BinArr.Length - 1] = (byte)(checksum % 256);
                //Save File
                //string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr_drop.bin";
                //string path = "C:\\temp\\BinArr_drop.bin";
                //SaveBinFile(path, BinArr);
                mvars.lblCmd = "MCU_FLASH_W62000";
                mhMCUFLASHWRITE("62000", ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
            }
            #endregion Primary

        ExNovaAGMA:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30";
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        public static void cDROPt(int svgray, byte svip, byte svsen, byte svpo, byte svca)
        {
            //主體從cENGGMAONWRITEp複製，暫時取消 svsen==160 的判斷式
            mp.markreset(999, false);
            mvars.flgDelFB = true;
            List<string> svuiregadr = new List<string>(new string[mvars.GAMMA_SIZE / 4]);
            string[] sRegDec;
            string[] sDataDec;
            string[] svuiuser = new string[40];
            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;
            int svms = 61;                              /// TV130
            int svme = mvars.uiregadr_default.Length;   /// TV130
            mvars.isReadBack = true;

            #region TV130
            if (svip == 160 && svsen == 160 && svpo == 160)
            {
                #region 廣播+畫面左側/畫面右側

                #endregion 廣播
            }
            else
            {
                if (svca == 0xA0) mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                else mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svca, 2);
                mvars.FPGAsel = 2;
                string[] RegDec = new string[mvars.uiregadr_default.Length];
                string[] DataDec = new string[mvars.uiregadr_default.Length];
                //mvars.lblCmd = "FPGA_REG_W";
                ////還原 UIREGADR Default
                //for (int svj = 0; svj < mvars.uiregadr_default.Length; svj++)
                //{
                //    RegDec[svj] = svj.ToString();
                //    DataDec[svj] = mvars.uiregadr_default[svj].Split(',')[2];
                //}
                //mp.mpFPGAUIREGWarr(RegDec, DataDec);
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-2"; goto ExNovaAGMA; }
                byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                //清空 Flash 0x62000 & 0x64000
                mvars.lblCmd = "MCUFLASH_W62000";
                mp.mhMCUFLASHWRITE("00062000", ref BinArr);
                //mvars.lblCmd = "MCUFLASH_W64000";
                //mp.mhMCUFLASHWRITE("00064000", ref BinArr);
                //寫入 Drop 的計算值
                for (int j = 0; j < mvars.uiregadr_default.Length; j++)
                {
                    Form1.svuiregadr.RemoveAt(j);
                    Form1.svuiregadr.Insert(j, mvars.uiregadr_default[j].Split(',')[2]);
                    Form1.svuiregadr.RemoveAt(j + (int)(mvars.GAMMA_SIZE / 8));   //x062000共存2048個參數(FPFA A與FPGA B各半)每個參數占用4個bytes=2048*4=8192 bytes=mvars.GAMMA_SIZE;
                    Form1.svuiregadr.Insert(j + (int)(mvars.GAMMA_SIZE / 8), mvars.uiregadr_default[j].Split(',')[2]);
                }
                for (byte svn = 0; svn < 2; svn++)
                {
                    int svj = (int)(mvars.GAMMA_SIZE / 8 * svn);
                    for (int i = svms; i < mvars.uiregadr_default.Length; i++)
                        Form1.svuiregadr[i + svj] = ((int)(Convert.ToSingle(mvars.uiregadr_default[i].Split(',')[2]) * svgray / 255)).ToString();
                    Form1.svuiregadr[5 + svj] = "1";
                    sRegDec = new string[svme];   //addr
                    sDataDec = new string[svme];  //data
                    for (int i = 0; i < svme; i++)
                    {
                        //real time
                        sRegDec[i] = i.ToString();
                        sDataDec[i] = Form1.svuiregadr[i + svj];
                        // MCU FLASH用
                        BinArr[i * 4 + 0 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) / 256);
                        BinArr[i * 4 + 1 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) % 256);
                        BinArr[i * 4 + 2 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) / 256);
                        BinArr[i * 4 + 3 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) % 256);
                    }
                    mvars.lblCmd = "FPGA_REG_W" + svn;
                    mp.mpFPGAUIREGWarr(sRegDec, sDataDec);
                }
                //for (byte svn = 0; svn < 2; svn++)
                //{
                //    int svj = (int)(mvars.GAMMA_SIZE / 8 * svn);
                //    Form1.svuiregadr[5 + svj] = "1";
                //    for (int i = 0; i < svme; i++)
                //    {
                //        BinArr[i * 4 + 0 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) / 256);
                //        BinArr[i * 4 + 1 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(i) % 256);
                //        BinArr[i * 4 + 2 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) / 256);
                //        BinArr[i * 4 + 3 + (mvars.GAMMA_SIZE / 2 * svn)] = (Byte)(Convert.ToInt32(Form1.svuiregadr[i + svj]) % 256);
                //    }
                //}
                //Checksum
                UInt16 checksum = CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                BinArr[BinArr.Length - 1] = (byte)(checksum % 256);
                //Save File
                //string path = "C:\\Users\\" + Environment.UserName + "\\Documents\\BinArr_drop.bin";
                //string path = "C:\\temp\\BinArr_drop.bin";
                //SaveBinFile(path, BinArr);
                mvars.lblCmd = "MCU_FLASH_W62000";
                mhMCUFLASHWRITE("62000", ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-3"; }
            }
        #endregion TV130

        ExNovaAGMA:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30";
            }
            else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }




        public static string replaceBoxMark(string svStr, int svmIndex, string svW)
        {
            string[] svmk = svStr.Split(',');
            if (svmIndex == 1)
            {
                if (svmk[svmIndex] == "0") { svmk[svmIndex] = svW; }
                else if (svmk[svmIndex] == "1") { svmk[svmIndex] = svW; }
                else if (svmk[svmIndex] == "2") { svmk[svmIndex] = svW; }
                else if (svmk[svmIndex] == "3") { svmk[svmIndex] = svW; }
            }
            else { svmk[svmIndex] = svW; }

            string svs = svmk[0];
            if (svmk.Length > 1) { for (int i = 1; i < svmk.Length; i++) { svs += "," + svmk[i]; } }
            return svs;
        }


        public static void cPrimarySplice(ListBox lstget1)
        {
            mvars.lblCompose = "SPLICE";
            mvars.strReceive = "";
            mvars.lCounts = 9999;
            mvars.lCount = 0;
            Array.Resize(ref mvars.lCmd, mvars.lCounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, mvars.lCounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.lCount = 0;
            mvars.errCode = "0";
            mvars.flgDelFB = true;
            mvars.Break = false;
            lstget1.Items.Clear();
            DateTime t1 = DateTime.Now;

            bool svnvBoardcast = mvars.nvBoardcast;
            mvars.nvBoardcast = false;
            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;
            mvars.FPGAsel = 2;

            Form1.lstm.Items.Clear();

            #region AutoID
            if (mvars.deviceID.Substring(0, 2) == "05") mvars.deviceID = "05A0";

            mvars.lblCmd = "PRIID_SERIESMODE";
            mp.mIDSERIESMODE();
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1) Form1.tslblStatus.Text = "ERROR," + mvars.deviceID + "," + mvars.lblCmd;
            else Form1.tslblStatus.Text = mvars.deviceID + "," + mvars.lblCmd + ",DONE";


            #endregion AutoID


            for (byte svscr = 1; svscr <= 12; svscr++)
            {
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(svscr, 2); ;
                mvars.lblCmd = "MCU_VERSION";
                mp.mhVersion();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                {
                    if (mvars.verMCU.Substring(mvars.verMCU.Length - 5, 1) == "P")
                    {
                        Form1.lstm.Items.Add(mvars.deviceID);
                        if (mvars.actFunc != "") lstget1.Items.Add(mvars.deviceID + "," + mvars.verMCU);
                    }
                }
                else
                {
                    if (mvars.actFunc != "")
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Add("X " + mvars.deviceID + ",doesn't exist"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Add("X " + mvars.deviceID + ",不存在"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Add("X " + mvars.deviceID + ",不存在"); }
                    }
                }
            }
            if (Form1.lstm.Items.Count > 0)
            {
                List<string> mcuR66000 = new List<string>();
                for (int i = 0; i < Form1.lstm.Items.Count; i++)
                {
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + Form1.lstm.Items[i].ToString().Substring(2, 2);
                    mcuR66000.Clear();
                    mvars.lblCmd = "MCU_FLASH_R66000";
                    mp.mhMCUFLASHREAD("00066000", 8192);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) mvars.errCode = "-3";
                    else mcuR66000.AddRange(mvars.strR66K.Split('~'));
                    //if (svmatch)
                    if (Convert.ToByte(mcuR66000[3].ToString().Split(',')[1]) == Convert.ToByte(mvars.deviceID.Substring(2, 2)))
                    {
                        Form1.pvindex = 66;         //X
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mvars.errCode = "-0.3"; Form1.hw[svip].card[i].BxGap = "1"; }
                        //else { Form1.hw[svip].card[i].BxGap = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]; }
                        Form1.pvindex = 67;         //Y
                        mvars.lblCmd = "FPGA_SPI_R";
                        mp.mhFPGASPIREAD();
                        //if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { mvars.errCode = "-0.3"; Form1.hw[svip].card[i].BxGap = "1"; }
                        //else { Form1.hw[svip].card[i].BxGap = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1]; }
                        //lst_mcuW66000.Items.Clear();
                        //lst_mcuW66000.Items.Add("0,255");   // 已記錄=1，未紀錄=255，
                        //lst_mcuW66000.Items.Add("1,0");     // (I)Sender
                        //lst_mcuW66000.Items.Add("2,0");     // (S)Port
                        //lst_mcuW66000.Items.Add("3,0");     // (P)Card or BoxNumber
                        //lst_mcuW66000.Items.Add("4,0");     // (C)X
                        //lst_mcuW66000.Items.Add("5,0");     // (X)Y
                        //lst_mcuW66000.Items.Add("6,960");   // (Y)W
                        //lst_mcuW66000.Items.Add("7,540");   // (W)H
                        //lst_mcuW66000.Items.Add("8,0");     // (H)畫線用
                        //lst_mcuW66000.Items.Add("9,0");     // (G)畫線用
                        //lst_mcuW66000.Items.Add("10,0");    //(畫線用)
                        //lst_mcuW66000.Items.Add("11,0");    //(畫線用)
                        Form1.lstget1.Items.Add(string.Format("No.{0},I{1},S{2},P{3},C{4},X{5},Y{6},W{7},H{8},G{9}",
                                (i + 1).ToString(),
                                0,
                                0,
                                mcuR66000[3].Split(',')[1],
                                0,
                                mcuR66000[5].Split(',')[1],
                                mcuR66000[6].Split(',')[1],
                                mcuR66000[7].Split(',')[1],
                                mcuR66000[8].Split(',')[1],
                                0));
                        if (mvars.actFunc != "")
                        {
                            string svs = lstget1.Items[i].ToString();
                            lstget1.Items.RemoveAt(i);
                            lstget1.Items.Insert(i, svs + "," + Form1.lstget1.Items[Form1.lstget1.Items.Count - 1].ToString());
                        }
                    }
                    else
                    {
                        //不吻合
                        Form1.lstget1.Items.Add(string.Format("No.{0},I{1},S{2},P{3},C{4},X{5},Y{6},W{7},H{8},G{9}",
                                (i + 1).ToString(),
                                0,
                                0,
                                mcuR66000[3].Split(',')[1],
                                0,
                                -1,
                                -1,
                                mcuR66000[7].Split(',')[1],
                                mcuR66000[8].Split(',')[1],
                                0));
                        if (mvars.actFunc != "")
                        {
                            string svs = lstget1.Items[i].ToString();
                            lstget1.Items.RemoveAt(i);
                            lstget1.Items.Insert(i, svs + "," + ", FPGAreg data ≠ 0x66000");
                        }
                    }
                    Form1.lstget1.TopIndex = Form1.lstget1.Items.Count - 1;

                    //mvars.lstget.Items.Add(Form1.lstget1.Items[Form1.lstget1.Items.Count - 1].ToString());
                    //mvars.lstget.TopIndex = mvars.lstget.Items.Count - 1;

                }
            }
            else
            {
                mvars.errCode = "-1";
                mvars.strReceive = "ERROR,1,16,0,0,10,0," + (DateTime.Now - t1).TotalSeconds.ToString("0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - t1).TotalSeconds.ToString("0")));
            }




        exBxRd:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.nvBoardcast = svnvBoardcast;
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;

            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                if (mvars.actFunc != "") lstget1.Items.Insert(0, "↓BoxRead，" + (DateTime.Now - t1).TotalSeconds.ToString("0.0") + "s");
                mvars.strReceive = "DONE,1,16,0,0,10,0," + (DateTime.Now - t1).TotalSeconds.ToString("0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - t1).TotalSeconds.ToString("0")));
            }
            //else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            lstget1.TopIndex = lstget1.Items.Count - 1;
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        public static void mhMCUBLWRITE(byte svcmd, string svtxt44) //0x17 Write only need    //0x15 Erase & Write  新版正確
        {
            UInt16 WriteSize = (UInt16)mvars.MCU_BLOCK_SIZE;
            byte[] WrArr = BitConverter.GetBytes(WriteSize);
            UInt16 PacketSize = (UInt16)(mvars.MCU_BLOCK_SIZE + 0x0F);
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            byte[] flash_addr_arr = StringToByteArray(svtxt44);


            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 500; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513 + (int)mvars.MCU_BLOCK_SIZE);
                Array.Resize(ref mvars.ReadDataBuffer, 65);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 65); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = svcmd;              //Cmd   
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];           //Size
            if (mvars.deviceID.Substring(0, 2) == "05") mvars.RS485_WriteDataBuffer[5 + svns] = 66;
            else mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];  //address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];  //address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];  //address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];  //address
            mvars.RS485_WriteDataBuffer[9 + svns] = WrArr[1];           //Write Size
            mvars.RS485_WriteDataBuffer[10 + svns] = WrArr[0];          //Write Size
            for (UInt16 i = 0; i < WriteSize; i++)                      //byte[] DataArr = File.ReadAllBytes(textBox41.Text);
            {
                mvars.RS485_WriteDataBuffer[11 + svns + i] = mvars.gFlashRdPacketArr[i];    //Data
            }
            funSendMessageTo();
        }



        public static void pMCUBLWRITE(byte svcmd, string svtxt44) //0x17 Write only need    //0x15 Erase & Write  新版正確
        {
            UInt16 WriteSize = (UInt16)mvars.MCU_BLOCK_SIZE;
            byte[] WrArr = BitConverter.GetBytes(WriteSize);
            UInt16 PacketSize = (UInt16)(mvars.MCU_BLOCK_SIZE + 0x0F);
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            byte[] flash_addr_arr = StringToByteArray(svtxt44);


            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 500; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513 + (int)mvars.MCU_BLOCK_SIZE);
                Array.Resize(ref mvars.ReadDataBuffer, 65);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 65); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = svcmd;              //Cmd   
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];           //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 66;
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];  //address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];  //address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];  //address
            mvars.RS485_WriteDataBuffer[9 + svns] = WrArr[1];           //Write Size
            mvars.RS485_WriteDataBuffer[10 + svns] = WrArr[0];          //Write Size
            for (UInt16 i = 0; i < WriteSize; i++)                      //byte[] DataArr = File.ReadAllBytes(textBox41.Text);
            {
                mvars.RS485_WriteDataBuffer[11 + svns + i] = mvars.gFlashRdPacketArr[i];    //Data
            }
            funSendMessageTo();
        }

        public static void pMCUAPPWRITE(byte svcmd, string svtxt44) //0x17 Write only need    //0x15 Erase & Write  新版正確
        {
            UInt16 WriteSize = (UInt16)mvars.MCU_BLOCK_SIZE;
            byte[] WrArr = BitConverter.GetBytes(WriteSize);
            UInt16 PacketSize = (UInt16)(mvars.MCU_BLOCK_SIZE + 0x0F);
            byte[] bytes = BitConverter.GetBytes(PacketSize);
            byte[] flash_addr_arr = StringToByteArray(svtxt44);


            byte svns = 2;  //預設NovaStar使用,因為反應較慢儘量減少程序
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 500; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + PacketSize);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513 + (int)mvars.MCU_BLOCK_SIZE);
                Array.Resize(ref mvars.ReadDataBuffer, 65);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 65); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = svcmd;              //Cmd   
            mvars.RS485_WriteDataBuffer[3 + svns] = bytes[1];           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = bytes[0];           //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 00;
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];  //address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];  //address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];  //address
            mvars.RS485_WriteDataBuffer[9 + svns] = WrArr[1];           //Write Size
            mvars.RS485_WriteDataBuffer[10 + svns] = WrArr[0];          //Write Size
            for (UInt16 i = 0; i < WriteSize; i++)                      //byte[] DataArr = File.ReadAllBytes(textBox41.Text);
            {
                mvars.RS485_WriteDataBuffer[11 + svns + i] = mvars.gFlashRdPacketArr[i];    //Data
            }
            funSendMessageTo();
        }


        public static void pMCUCMDBYPASS(byte svbypass)             //0x89  bypassON=0x11,off=0x00
        {
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 65);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 513); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x89;       //Cmd   
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D;       //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x10;       //Wr MCU Dac - Cmd
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x66;       //Wr MCU Dac - Key
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;       //Wr MCU Dac - Len 
            mvars.RS485_WriteDataBuffer[8 + svns] = svbypass;   //Wr MCU Dac - On(0x11) Off(0x00)
            funSendMessageTo();
        }



        public static void mhMCUBLREAD(string svtxt44, int ReadSize)  //0x14  新版正確
        {
            byte svns = 2;
            byte[] bytes = BitConverter.GetBytes(ReadSize);
            byte[] flash_addr_arr = StringToByteArray(svtxt44);

            if (mvars.svnova)
            {
                mvars.Delaymillisec = 30; mvars.NumBytesToRead = ReadSize + 65;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x0F);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 65 + ReadSize); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x14;               //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;               //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;               //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];  //address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];  //address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];  //address
            mvars.RS485_WriteDataBuffer[8 + svns] = flash_addr_arr[3];  //address
            mvars.RS485_WriteDataBuffer[9 + svns] = bytes[1];           //Read Size
            mvars.RS485_WriteDataBuffer[10 + svns] = bytes[0];          //Read Size
            funSendMessageTo();
        }

        public static void mhMCUBLMODE()    //0xFE  新版正確
        {
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 5; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x09);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 65); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0xFE;            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x09;            //Size
            funSendMessageTo();
        }

        public static void mhMCUSWRESET()   //0xFF  新版正確
        {
            byte svns = 2;
            if (mvars.svnova)
            {
                mvars.Delaymillisec = 2500; mvars.NumBytesToRead = 28;
                Array.Resize(ref mvars.RS485_WriteDataBuffer, svns + 0x09);
            }
            else
            {
                svns = 1;
                //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

                Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            }
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            Array.Resize(ref mvars.ReadDataBuffer, 65); Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0xFF;           //Cmd Soft(0xFF)  Hard(0x80)
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;           //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x09;           //Size    最後一筆+1, 例.[13]則Size=0x0E 0~13=14
            funSendMessageTo();
        }


        #region Code Protect 0x14,0x18
        public static void mNvmUserPageRd()                                     //0x14  新版正確
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 65 + 512);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x14;       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0F;       //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x00;       //address
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x80;       //address
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x40;       //address
            mvars.RS485_WriteDataBuffer[8 + svns] = 0x00;       //address
            mvars.RS485_WriteDataBuffer[9 + svns] = 0x02;       //Read Size
            mvars.RS485_WriteDataBuffer[10 + svns] = 0x00;      //Read Size
            mp.funSendMessageTo();
        }

        public static void mDisProtBOD33()                                      //0x18  新版正確
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 65 + 512);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x18;   //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x19;   //Size

            mvars.NVM_UserPage[0] |= 0x01;                  //Disable BOD33
            mvars.NVM_UserPage[3] &= 0xC3;                  //Clear BOOT PROT
            mvars.NVM_UserPage[3] |= 0x3C;                  //OR 0x0F left shift 2 bit,protect 0KB

            mvars.NVM_UserPage[8] = 0xFF;
            mvars.NVM_UserPage[9] = 0xFF;
            mvars.NVM_UserPage[10] = 0xFF;
            mvars.NVM_UserPage[11] = 0xFF;
            mvars.NVM_UserPage[12] = 0xFF;
            mvars.NVM_UserPage[13] = 0xFF;
            mvars.NVM_UserPage[14] = 0xFF;
            mvars.NVM_UserPage[15] = 0xFF;

            for (UInt16 i = 0; i < 16; i++)
                mvars.RS485_WriteDataBuffer[5 + svns + i] = mvars.NVM_UserPage[i];    //Data
            mp.funSendMessageTo();
        }

        public static void mEnProtBOD33()                                       //0x18  新版正確
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 65 + 512);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x18;   //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x19;   //Size

            mvars.NVM_UserPage[0] &= 0xFE;                  //Enable BOD33
            mvars.NVM_UserPage[3] &= 0xC3;                  //Clear BOOT PROT
            mvars.NVM_UserPage[3] |= 0x1C;                  //OR 0x07 left shift 2 bit,protect 64KB

            mvars.NVM_UserPage[8] = 0xFF;
            mvars.NVM_UserPage[9] = 0xFF;
            mvars.NVM_UserPage[10] = 0xFF;
            mvars.NVM_UserPage[11] = 0xFF;
            mvars.NVM_UserPage[12] = 0xFF;
            mvars.NVM_UserPage[13] = 0xFF;
            mvars.NVM_UserPage[14] = 0xFF;
            mvars.NVM_UserPage[15] = 0xFF;

            for (UInt16 i = 0; i < 16; i++)
                mvars.RS485_WriteDataBuffer[5 + svns + i] = mvars.NVM_UserPage[i];    //Data
            mp.funSendMessageTo();
        }


        public static void pDisProtBOD33()                                      //0x18  新版正確
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 1023);
            Array.Resize(ref mvars.ReadDataBuffer, 1023);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x19;            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0A;            //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x00;            //Disable

            mp.funSendMessageTo();
        }

        public static void pEnProtBOD33()                                       //0x18  新版正確
        {
            #region 2023版公用程序(w/o novastar，帶 PacketSize)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 1023);
            Array.Resize(ref mvars.ReadDataBuffer, 1023);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x19;            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0A;            //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x01;            //Enable
            mp.funSendMessageTo();
        }

        #endregion


        #endregion 20230328




        #region Commport ------------------------------------------
        //阿良的嵌入式系統技術學習區
        //from https://jimsun-embedded.blogspot.com/2020/03/c-wmicom-port.html
        public static string GetFullComputerDevices()
        {
            ManagementClass processClass = new ManagementClass("Win32_PnPEntity");
            ManagementObjectCollection Ports = processClass.GetInstances();
            string ComPort_number = "No COM Port device recognized";

            foreach (ManagementObject property in Ports)
            {
                //if (property.GetPropertyValue("Name").ToString().Contains("Prolific USB-to-Serial Comm Port"))  //以顯示名稱為Prolific USB-to-Serial Comm Port的UART轉USB晶片PL2303為基底的裝置為例
                if (property.GetPropertyValue("Name").ToString().Contains("USB Serial Port") || property.GetPropertyValue("Name").ToString().Contains("USB Serial Device"))  
                {
                    ComPort_number = property.GetPropertyValue("Name").ToString();
                    ComPort_number = ComPort_number.Substring(ComPort_number.IndexOf("COM")).TrimEnd(')'); //取得COMx的字串並將其放置到ComPort_number這個string變數

                    //微軟的String.Substring方法教學
                    //微軟的String.TrimEnd方法教學:

                }
            }
            return ComPort_number; //回傳字串，如果都沒有就會回傳預設的"No COM Port device recognized"字串
        }

        public static string GetCOMs_old()
        {
            ManagementClass processClass = new ManagementClass("Win32_PnPEntity");
            ManagementObjectCollection Ports = processClass.GetInstances();
            string ComPort_number = "";

            foreach (ManagementObject property in Ports)
            {
                //if (property.GetPropertyValue("Name").ToString().Contains("Prolific USB-to-Serial Comm Port"))  //以顯示名稱為Prolific USB-to-Serial Comm Port的UART轉USB晶片PL2303為基底的裝置為例
                if (property.GetPropertyValue("Name").ToString().Contains("USB Serial Port") || 
                    property.GetPropertyValue("Name").ToString().Contains("USB Serial Device") || 
                    property.GetPropertyValue("Name").ToString().Contains("USB 序列裝置") ||
                    property.GetPropertyValue("Name").ToString().Contains("USB序列裝置"))
                {
                    string ComPort= property.GetPropertyValue("Name").ToString();
                    ComPort_number += "," + ComPort.Substring(ComPort.IndexOf("COM")).TrimEnd(')'); //取得COMx的字串並將其放置到ComPort_number這個string變數

                    //微軟的String.Substring方法教學
                    //微軟的String.TrimEnd方法教學:

                }
            }
            if (ComPort_number.Length > 0 && ComPort_number.Substring(0, 1) == ",") ComPort_number = ComPort_number.Substring(1, ComPort_number.Length - 1);
            return ComPort_number;
        }


        public static string GetCOMs()
        {
            string ComPort_number = "";
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                //使用ManagementObjectSearcher來查詢註冊表中的裝置名稱
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();//取得所有ManagementBaseObject並轉成List
                string[] PortsName = new string[ports.Count];
                for (int i = 0; i < ports.Count; i++)
                {
                    PortsName[i] = ports[i]["DeviceID"] as string + "-"
                        + ports[i]["Caption"] as string;//取得裝置名稱與連接埠    串行设备
                    if (PortsName[i].Contains("USB Serial Port") ||
                        PortsName[i].Contains("USB Serial Device") ||
                        PortsName[i].Contains("USB 序列裝置") ||
                        PortsName[i].Contains("USB序列裝置") ||
                        PortsName[i].Contains("USB串行设备") ||
                        PortsName[i].Contains("USB 串行设备") ||
                        PortsName[i].Contains("USB串行設備") ||
                        PortsName[i].Contains("USB 串行設備"))
                    {
                        ComPort_number = ports[i]["DeviceID"] as string;
                        break; 
                    }
                }
                return ComPort_number;
            }
        }


        public static string FindComPort()
        {
            try
            {
                if (mvars.sp1.IsOpen) mvars.sp1.Close();
            }
            catch (Exception ex)
            {
                Form1.lstget1.Items.Add(ex.Message);
                return "";
            }
            //List<string> comports = ComPortNames("04D8", "000A");
            string svcom = ComPortNames("04D8", "000A");
            //if (comports.Count == 0)
            if (svcom.IndexOf("COM", 0) == -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { Form1.tslblStatus.Text = @"No Any ""USB Serial Device"""; }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { Form1.tslblStatus.Text = @"沒有任何 ""USB 序列裝置"""; }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { Form1.tslblStatus.Text = @"沒有任何 ""USB 串行设备"""; }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { Form1.tslblStatus.Text = @"No Any ""USB Serial Device"""; }
                Array.Resize(ref mvars.Comm, 0);
                return "";
            }
            else
            {
                //Array.Resize(ref mvars.Comm, comports.Count);
                //for (int i = 0; i < comports.Count; i++) mvars.Comm[i] = comports[i];
                //return comports[0];

                Array.Resize(ref mvars.Comm, svcom.Split(',').Length);
                for (int i = 0; i < svcom.Split(',').Length; i++) mvars.Comm[i] = svcom.Split(',')[i];
                return svcom.Substring(0, svcom.Length - 1);
            }
            
        }

        /*
        public static List<string> ComPortNames(String VID, String PID)
        {
            List<string> comports = new List<string>();
            try
            {
                String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
                Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);

                RegistryKey rk1 = Registry.LocalMachine;
                RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");

                foreach (String s3 in rk2.GetSubKeyNames())
                {
                    RegistryKey rk3 = rk2.OpenSubKey(s3);
                    foreach (String s in rk3.GetSubKeyNames())
                    {
                        if (_rx.Match(s).Success)
                        {
                            RegistryKey rk4 = rk3.OpenSubKey(s);
                            foreach (String s2 in rk4.GetSubKeyNames())
                            {
                                try
                                {
                                    RegistryKey rk5 = rk4.OpenSubKey(s2);
                                    string location = (string)rk5.GetValue("LocationInformation");
                                    RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                                    string portName = (string)rk6.GetValue("PortName");
                                    if (!String.IsNullOrEmpty(portName) && SerialPort.GetPortNames().Contains(portName))
                                        comports.Add((string)rk6.GetValue("PortName"));
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.lstget1.Items.Add(ex.Message);
            }
            return comports;
        }
        */

        public static string ComPortNames(String VID, String PID)
        {
            string svs = "";
            try
            {
                String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
                Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);

                RegistryKey rk1 = Registry.LocalMachine;
                RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");

                foreach (String s3 in rk2.GetSubKeyNames())
                {
                    RegistryKey rk3 = rk2.OpenSubKey(s3);
                    foreach (String s in rk3.GetSubKeyNames())
                    {
                        if (_rx.Match(s).Success)
                        {
                            RegistryKey rk4 = rk3.OpenSubKey(s);
                            foreach (String s2 in rk4.GetSubKeyNames())
                            {
                                try
                                {
                                    RegistryKey rk5 = rk4.OpenSubKey(s2);
                                    string location = (string)rk5.GetValue("LocationInformation");
                                    RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                                    string portName = (string)rk6.GetValue("PortName");
                                    if (!String.IsNullOrEmpty(portName) && SerialPort.GetPortNames().Contains(portName))
                                        //comports.Add((string)rk6.GetValue("PortName"));
                                        svs += (string)rk6.GetValue("PortName") + ",";
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
                return svs;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        #endregion Commport----------------------------------------




        #region LT8668 ============================================================= +
        public static void LT8668_Bin_Wr(Byte Addr, UInt16 WrSize, byte[] arr)   //0x60
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            UInt16 Size = (UInt16)(0x0C + WrSize);

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x60;                      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = (byte)(Size / 256);        //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = (byte)(Size % 256);        //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (Byte)(Addr / 2);          //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = (byte)(WrSize / 256);      //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = (byte)(WrSize % 256);      //Write Size
            for (UInt16 i = 0; i < WrSize; i++)
            {
                //sShow += "0x" + Byte2HexString(arr[i]) + ",";
                mvars.RS485_WriteDataBuffer[8 + svns + i] = arr[i];
            }
            funSendMessageTo();
        }



        public static void LT8668_Bin_Wr_Loop(string OffsetAddrText,byte[] gBinArr)     // 0x62
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 8193);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            //EDID
            int PacketSize = 256;
            byte[] BinArr = new byte[PacketSize];
            Array.Resize(ref BinArr, PacketSize);
            UInt16 Count = (UInt16)(BinArr.Length / PacketSize);
            for (int i = 0; i < Count; i++)
            {
                string FlashAddrText = (i * PacketSize + Convert.ToUInt32(OffsetAddrText, 16)).ToString("X6");
                byte[] flash_addr_arr = StringToByteArray(FlashAddrText);
                mvars.RS485_WriteDataBuffer[2 + svns] = 0x62;                                //Cmd
                mvars.RS485_WriteDataBuffer[3 + svns] = (byte)((0x0E + PacketSize) / 256);   //Size
                mvars.RS485_WriteDataBuffer[4 + svns] = (byte)((0x0E + PacketSize) % 256);   //Size
                mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];                   //Address
                mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];                   //Address
                mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];                   //Address
                mvars.RS485_WriteDataBuffer[8 + svns] = (byte)(PacketSize / 256);            //Packet Size
                mvars.RS485_WriteDataBuffer[9 + svns] = (byte)(PacketSize % 256);            //Packet Size
                for (UInt16 j = 0; j < PacketSize; j++)
                    mvars.RS485_WriteDataBuffer[10 + svns + j] = gBinArr[j];
                mp.funSendMessageTo();

            }
        }

        public static void LT8668_Bin_Wr_Loop(byte[] gBinArr, string FlashAddrText, int PacketSize, int Offset)     // 0x62
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 8193);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            //EDID
            byte[] flash_addr_arr = StringToByteArray(FlashAddrText);
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x62;                                //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = (byte)((0x0E + PacketSize) / 256);   //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = (byte)((0x0E + PacketSize) % 256);   //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = flash_addr_arr[0];                   //Address
            mvars.RS485_WriteDataBuffer[6 + svns] = flash_addr_arr[1];                   //Address
            mvars.RS485_WriteDataBuffer[7 + svns] = flash_addr_arr[2];                   //Address
            mvars.RS485_WriteDataBuffer[8 + svns] = (byte)(PacketSize / 256);            //Packet Size
            mvars.RS485_WriteDataBuffer[9 + svns] = (byte)(PacketSize % 256);            //Packet Size
            for (UInt16 j = 0; j < PacketSize; j++)
                mvars.RS485_WriteDataBuffer[10 + svns + j] = gBinArr[j + Offset];
            mp.funSendMessageTo();
        }



        public static void LT8668_ScaleONOPFF(Byte ScalerOff)   //0x8A
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x8A;              //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;              //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D;              //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0xB1;              //Wr mcu parameter cmd.
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x67;              //Key
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;              //Length
            mvars.RS485_WriteDataBuffer[8 + svns] = ScalerOff;         //On/Off
            funSendMessageTo();
        }

        public static void LT8668_Reset_L()   //0x83
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x83;                                            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                                            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0B;                                            //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x01;                                            //Reset IO
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x00;                                            //L
            funSendMessageTo();
        }

        public static void LT8668_Reset_H()   //0x83
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x83;                                            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                                            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0B;                                            //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x01;                                            //Reset IO
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x01;                                            //H            funSendMessageTo();
            funSendMessageTo();
        }

        public static void McuSW_Reset()   //0xFF
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0xFF;            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;            //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0B;            //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0x07;            //Delay 1s
            mvars.RS485_WriteDataBuffer[6 + svns] = 0xD0;            //Delay 1s
            funSendMessageTo();
        }

        public static void LT8668_Bin_WrRd(Byte Addr, UInt16 WrSize, byte[] arr, UInt16 RdSize, ref byte[] rd_arr)   //0x61
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            UInt16 Size = (UInt16)(0x0E + WrSize);
            //string sShow = "0x" + Byte2HexString(Addr) + ",";
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x61;                            //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = (byte)(Size / 256);              //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = (byte)(Size % 256);              //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = (byte)(Addr / 2);                //Device Address
            mvars.RS485_WriteDataBuffer[6 + svns] = (byte)(WrSize / 256);            //Write Size
            mvars.RS485_WriteDataBuffer[7 + svns] = (byte)(WrSize % 256);            //Write Size
            for (UInt16 i = 0; i < WrSize; i++)
            {
                mvars.RS485_WriteDataBuffer[8 + svns + i] = arr[i];
                //sShow += "0x" + Byte2HexString(arr[i]) + ",";
            }
            //sShow += "0x" + Byte2HexString((byte)(Addr + 1)) + ",";
            mvars.RS485_WriteDataBuffer[8 + svns + WrSize] = (byte)(RdSize / 256);   //Read Size
            mvars.RS485_WriteDataBuffer[9 + svns + WrSize] = (byte)(RdSize % 256);   //Read Size
            funSendMessageTo();
        }

        public static string HexFile2Bin(string FilePath, ref byte[] BinFile, byte FillData)
        {
            string sTextLine;
            bool bBreak = false;
            bool bHexParseSuccess = false;
            UInt32 lLineIndex;
            //Array Fill Data
            for (int i = 0; i < BinFile.Length; i++)
                BinFile[i] = FillData;
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(@FilePath);
            UInt32 lFlashAddr, lExtSegAddr = 0, lTmp;
            while ((sTextLine = file.ReadLine()) != null)
            {
                if (sTextLine.Substring(0, 1) == ":")
                {
                    Byte ucLineLen, ucLineChecksum;
                    UInt32 lRunningChecksum = 0;
                    byte ucRecordLength = Convert.ToByte(sTextLine.Substring(1, 2), 16);
                    //lLineIndex += 2;
                    lRunningChecksum += ucRecordLength;

                    byte ucAddrTmp = Convert.ToByte(sTextLine.Substring(3, 2), 16);
                    lRunningChecksum += ucAddrTmp;
                    UInt32 lAddrOffset = ucAddrTmp;
                    lAddrOffset *= 256;

                    ucAddrTmp = Convert.ToByte(sTextLine.Substring(5, 2), 16);
                    lRunningChecksum += ucAddrTmp;
                    lAddrOffset += ucAddrTmp;

                    byte ucRecType = Convert.ToByte(sTextLine.Substring(7, 2), 16);
                    lRunningChecksum += ucRecType;

                    lLineIndex = 10;

                    switch (ucRecType)  //Select Case ucRecType
                    {
                        case 0x00:
                            for (byte iHexDataIndex = 0; iHexDataIndex < ucRecordLength; iHexDataIndex++)
                            {
                                Byte ucToHexData;
                                ucToHexData = Convert.ToByte(sTextLine.Substring((int)lLineIndex - 1, 2), 16);
                                lLineIndex += 2;
                                lFlashAddr = lAddrOffset + iHexDataIndex + lExtSegAddr;
                                if (lFlashAddr >= BinFile.Length)
                                {
                                    //OP_Msg1(lbx, "Flash Address greater than Bin Size");
                                    return "Error,Flash Address greater than Bin Size";
                                    break;
                                }
                                else
                                    BinFile[lFlashAddr] = ucToHexData;

                                lRunningChecksum += ucToHexData;
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
                            //if(lExtSegAddr != 0 )
                            //    bBreak = true;
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
            }   //while ((sTextLine = file.ReadLine()) != null)

            file.Close();

            //Check Flag
            if (bHexParseSuccess == false || bBreak == true)
            {
                //OP_Msg1(lbx, "Error Hex File");
                return "Error,Hex File error";
            }
            else return "OK";
        }

        public static int GetBinFileEffectivLength(ref byte[] BinFile)
        {
            //byte[] BinFile;
            int addr = 0;

            //BinFile = File.ReadAllBytes(sPath);
            for (int i = 0; i < (UInt32)BinFile.Length; i++)
            {
                //if (!(BinFile[i] == 0x00 || BinFile[i] == 0xFF))
                if (!(BinFile[i] == 0xFF))
                    addr = i;
            }
            return addr + 1;
        }

        public static byte CRC8_Cal(CrcInfoTypeS CrcType, byte[] Arr, UInt32 BufLen)
        {
            UInt32 poly = CrcType.Poly;
            UInt32 crc = CrcType.CrcInit;
            UInt32 xorout = CrcType.XorOut;
            bool refin = CrcType.RefIn;
            bool refout = CrcType.RefOut;
            UInt32 data;
            Byte i;

            for (UInt32 j = 0; j < BufLen; j++)
            {
                data = Arr[j];
                if (refin)
                    data = BitsReverse(data, 8);
                crc ^= (data << 0);
                for (i = 0; i < 8; i++)
                {
                    if ((crc & 0x80) == 0x80)
                        crc = (crc << 1) ^ poly;
                    else
                        crc <<= 1;
                }
            }
            if (refout) crc = BitsReverse(crc, 8);
            crc ^= xorout;
            //crc &= 0xFF;
            return (byte)crc;
        }

        public static UInt32 BitsReverse(UInt32 inVal, UInt32 bits)
        {
            UInt32 outVal = 0;
            UInt32 i;
            for (i = 0; i < bits; i++)
                if ((inVal & ((UInt32)Math.Pow(2, i))) == (UInt32)(Math.Pow(2, i))) outVal |= (UInt32)(Math.Pow(2, (bits - 1 - i)));
            return outVal;
        }

        public static void BinFile2Bin(string FilePath, ref byte[] Bin)
        {
            byte[] BinArr;
            BinArr = File.ReadAllBytes(FilePath);
            //Array.Resize(ref gBinArr, BinArr.Length);
            for (UInt32 i = 0; i < BinArr.Length; i++)
                Bin[i] = BinArr[i];
        }

        public static void Txt_File2Bin(string FilePath, ref byte[] Bin)
        {
            string sTextLine;
            int Cnt = 0;
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(FilePath);
            while ((sTextLine = file.ReadLine()) != null)
            {
                sTextLine = sTextLine.Replace(" ", "");
                for (int i = 0; i < sTextLine.Length / 2; i++)
                    Bin[Cnt++] = Convert.ToByte(sTextLine.Substring(i * 2, 2), 16);
            }
        }


        public static void cLT8668Write(ListBox lstget1, string strpath, string OffsetAddrText, bool fastRd, UInt16 PacketSize, bool svSecterErase)
        {
            byte[] gBinArr = new byte[65536];

            #region ↓ConfPara_btn_Click
            Byte[] arr = new byte[17];
            //Configure Parameter
            //Enable I2C  
            mvars.lblCmd = "EnableI2C_Wr";
            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0xEE; arr[1] = 0x01; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0x5E; arr[1] = 0xC1; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0x58; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0x59; arr[1] = 0x50; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0x5A; arr[1] = 0x10; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0x58; arr[1] = 0x21; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            #endregion ↑ConfPara_btn_Click


            #region ↓BlockErase_btn_Click
            arr = new byte[17]; 
            byte[] flash_addr_arr = StringToByteArray(OffsetAddrText);
            //Block Erase
            mvars.lblCmd = "LT8668_Block Erase";
            arr[0] = 0x5A; arr[1] = 0x04;              mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0x5A; arr[1] = 0x00;              mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            arr[0] = 0x5B; arr[1] = flash_addr_arr[0]; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);   // flash address[23:16] 
            arr[0] = 0x5C; arr[1] = flash_addr_arr[1]; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);   // flash address[15:8] 
            arr[0] = 0x5D; arr[1] = flash_addr_arr[2]; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);   // flash address[7:0] 

            if (svSecterErase == false)
            { arr[0] = 0x5A; arr[1] = 0x01;            mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100); } // half-block erase (32KB) 
            else
            { arr[0] = 0x5A; arr[1] = 0x02;            mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100); } // Secter erase (4KB) 

            arr[0] = 0x5A; arr[1] = 0x00;              mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
            doDelayms(500);
            //if (LT8668_FlashAddr_rbtn.Checked)
            if (OffsetAddrText == "000000")
            {
                mvars.lblCmd = "LT8668_Bin_Wr";
                arr[0] = 0x5A; arr[1] = 0x04;                             mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
                arr[0] = 0x5A; arr[1] = 0x00;                             mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
                arr[0] = 0x5B; arr[1] = flash_addr_arr[0];                mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);    // flash address[23:16] 
                arr[0] = 0x5C; arr[1] = (byte)(flash_addr_arr[1] + 0x80); mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);   // flash address[15:8] 
                arr[0] = 0x5D; arr[1] = flash_addr_arr[2];                mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);   // flash address[7:0] 
                arr[0] = 0x5A; arr[1] = 0x01;                             mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);   // half-block erase (32KB) 
                arr[0] = 0x5A; arr[1] = 0x00;                             mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);
                doDelayms(500);
            }
            #endregion ↑BlockErase_btn_Click

            if (fastRd)
            {
                //Bin_Wr_Loop_btn_Click(sender, e);
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Reset(); sw.Start();

                //UInt16 PacketSize = Convert.ToUInt16(RdWr_Size.Text);

                if (OffsetAddrText == "000000")
                    Array.Resize(ref gBinArr, 0x10000);
                else if (OffsetAddrText == "060000" || OffsetAddrText == "068000")
                    Array.Resize(ref gBinArr, 0x6000);                
                else if (OffsetAddrText == "070000" || OffsetAddrText == "054000")
                {
                    Array.Resize(ref gBinArr, 0x100);
                    PacketSize = 256;
                }


                //Load
                if (OffsetAddrText == "000000")
                {
                    #region LT8668 hex / bin
                    uint EffectivLength;
                    if (strpath.Substring(strpath.Length - 3, 3) == "hex")
                    {
                        HexFile2Bin(strpath, ref gBinArr, 0xFF);
                        EffectivLength = (uint)GetBinFileEffectivLength(ref gBinArr);
                        CrcInfoTypeS CrcType = new CrcInfoTypeS
                        {
                            Poly = 0x31,
                            CrcInit = 0,
                            XorOut = 0,
                            RefOut = false,
                            RefIn = false
                        };
                        CrcType.CrcInit = CRC8_Cal(CrcType, gBinArr, EffectivLength);
                        Byte[] DefaultVal = new Byte[2];
                        DefaultVal[0] = 0xFF;
                        for (UInt16 i = 0; i < (gBinArr.Length - 1 - EffectivLength); i++)
                        {
                            CrcType.CrcInit = CRC8_Cal(CrcType, DefaultVal, 1);
                        }
                        gBinArr[65535] = (byte)CrcType.CrcInit;
                    }
                    else if (strpath.Substring(strpath.Length - 3, 3) == "bin")
                        BinFile2Bin(strpath, ref gBinArr);
                    #endregion
                }
                else if (OffsetAddrText == "060000" || OffsetAddrText == "068000" || OffsetAddrText == "054000")
                {
                    #region OSD / OSD1 / Addr_54000
                    if (strpath.Substring(strpath.Length - 3, 3) == "txt")
                    {
                        Txt_File2Bin(strpath, ref gBinArr);

                        //string sTextLine;
                        //int Cnt = 0;
                        //// Read the file and display it line by line.  
                        //System.IO.StreamReader file = new System.IO.StreamReader(@dialog.FileName);
                        //while ((sTextLine = file.ReadLine()) != null)
                        //{
                        //    sTextLine = sTextLine.Replace(" ", "");
                        //    for (int i = 0; i < sTextLine.Length / 2; i++)
                        //        gBinArr[Cnt++] = Convert.ToByte(sTextLine.Substring(i * 2, 2), 16);
                        //}
                        //Copy Value
                        //for (int i = 0; i < gBinArr.Length; i++)
                        //    Bin_Wr_dGV.Rows[(i / 16) + 1].Cells[(i % 16) + 1].Value = Byte2HexString(gBinArr[i]);
                    }
                    else if (strpath.Substring(strpath.Length - 3, 3) == "bin")
                    {
                        //Bin File
                        BinFile2Bin(strpath, ref gBinArr);
                    }
                    #endregion
                }
                else if (OffsetAddrText == "070000")
                {
                    BinFile2Bin(strpath, ref gBinArr);
                }





                //↓ Bin_Wr_Loop_btn_Click
                UInt16 Count = (UInt16)(gBinArr.Length / PacketSize);
                //Flash to FIFO
                mvars.lblCmd = "LT8668_Bin_Wr_Loop";
                for (int i = 0; i < Count; i++)
                {
                    string FlashAddrText = (i * PacketSize + Convert.ToUInt32(OffsetAddrText, 16)).ToString("X6");
                    mp.LT8668_Bin_Wr_Loop(gBinArr, FlashAddrText, PacketSize, i * PacketSize); mp.doDelayms(100);

                    //Send To MCU
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    {
                        mp.showStatus1("Write Counter：" + (i + 1) + " / " + Count + " , " +
                                string.Format("{0:#.00}", (decimal)(i + 1) * 100 / Count) + "%", lstget1, "");
                    }
                    else
                        break;
                    Application.DoEvents();
                }


            }
            else
            {
                ////Bin_Wr32_Loop_btn_Click(sender, e);
                //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                //sw.Reset(); sw.Start();
                //Byte[] arr = new byte[17];// Byte[] rd_arr = new byte[32];
                //if (LT8668_FlashAddr_rbtn.Checked)
                //    Array.Resize(ref gBinArr, 0x10000);
                //else if (OSD_FlashAddr_rbtn.Checked)
                //    Array.Resize(ref gBinArr, 0x6000);
                //else if (OSD1_FlashAddr_rbtn.Checked)
                //    Array.Resize(ref gBinArr, 0x6000);
                //else if (EDID_FlashAddr_rbtn.Checked)
                //    Array.Resize(ref gBinArr, 0x100);
                //for (UInt16 i = 0; i < gBinArr.Length / 32; i++)
                //{
                //    //WREN
                //    arr[0] = 0xFF; arr[1] = 0xE1; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0x03; arr[1] = 0x2E; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0x03; arr[1] = 0xEE; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0x5A; arr[1] = 0x04; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //                                                           //I2C TO FIFO
                //    arr[0] = 0x5E; arr[1] = 0xDF; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0x5A; arr[1] = 0x20; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0x58; arr[1] = 0x21; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    for (UInt16 j = 0; j < 32; j++)
                //    {
                //        arr[0] = 0x59;
                //        arr[1] = Convert.ToByte((string)Bin_Wr_dGV.Rows[i * 2 + 1 + j / 16].Cells[j % 16 + 1].Value, 16);
                //        mp.LT8668_Bin_Wr(0x86, 2, arr);    //DATA 1 TO 32
                //    }
                //    //FIFO TO FLASH
                //    FlashAddr.Text = (i * 32 + Convert.ToUInt32(OffsetAddr.Text, 16)).ToString("X6");
                //    byte[] flash_addr_arr = StringToByteArray(FlashAddr.Text);
                //    arr[0] = 0x5B; arr[1] = flash_addr_arr[0]; mp.LT8668_Bin_Wr(0x86, 2, arr);    // flash address[23:16] 
                //    arr[0] = 0x5C; arr[1] = flash_addr_arr[1]; mp.LT8668_Bin_Wr(0x86, 2, arr);    // flash address[15:8] 
                //    arr[0] = 0x5D; arr[1] = flash_addr_arr[2]; mp.LT8668_Bin_Wr(0x86, 2, arr);    // flash address[7:0] 
                //    arr[0] = 0x5A; arr[1] = 0x10; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                //    Application.DoEvents();
                //    WrTime_lbl.Text = Convert.ToString(string.Format("{0:#.0}", sw.Elapsed.TotalSeconds));
                //}
            }

            #region ↓WrDis_btn_Click
            arr = new byte[17]; //Byte[] rd_arr = new byte[16];
            //WRDI
            mvars.lblCmd = "LT8668_Bin_Wr";
            arr[0] = 0x5A; arr[1] = 0x08; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);    //
            arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr); mp.doDelayms(100);    //
            #endregion ↑WrDis_btn_Click

            #region ↓checkSum
            UInt16 checksum = 0;
            checksum += CalChecksumIndex(gBinArr, 0x0000, (uint)(gBinArr.Length - 1));
            mp.showStatus1("Checksum(0000H~" + Convert.ToString(gBinArr.Length - 1, 16).ToUpper() + "H)", lstget1, "");
            mp.showStatus1("Load：0x" + checksum.ToString("X4"), lstget1, "");
            mp.showStatus1("", lstget1, "");
            #endregion ↑checkSum
        }

        #endregion LT8668 ========================================================== -




        #region OCP ================================================================ +
        public static void mOCP_EnDis_Ctrl(Byte OCP_En)   //0x8A
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x8A;       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x0D;       //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0xD0;       //OCP En/Dis cmd.
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x88;       //Key
            mvars.RS485_WriteDataBuffer[7 + svns] = 0x01;       //Length
            mvars.RS485_WriteDataBuffer[8 + svns] = OCP_En;     //OCP Enable/Disable
            funSendMessageTo();
        }

        public static void cOCPONOFF(ListBox lstget1, bool svON1, bool svON2)
        {
            //要使用Sendmessage則不要設定 lblcompose = "";
            if (mvars.flgSelf) mvars.lblCompose = "PRIMARY_OCPONOFF";
            mvars.strReceive = "";
            mvars.lCounts = 9999;
            mvars.lCount = 0;
            Array.Resize(ref mvars.lCmd, mvars.lCounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, mvars.lCounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.lCount = 0;
            mvars.errCode = "0";
            mvars.flgDelFB = true;
            mvars.Break = false;
            lstget1.Items.Clear();
            DateTime t1 = DateTime.Now;

            bool svnvBoardcast = mvars.nvBoardcast;
            mvars.nvBoardcast = false;
            string svdeviceID = mvars.deviceID;
            //byte svFPGAsel = mvars.FPGAsel;
            //mvars.FPGAsel = 1;

            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                Byte OCP_En = 0;
                if (svON1) OCP_En |= 0x01;
                if (svON2) OCP_En |= 0x02;
                mp.mOCP_EnDis_Ctrl(OCP_En);
                mp.McuSW_Reset();
                mp.doDelayms(13000);
                //mp.pidinit();
                mp.cmdHW(255, 128, 128, out string SvIF);
                if (SvIF == "232")
                {
                    mp.Sp1open(mvars.Comm[0]);
                    mvars.lblCmd = "MCU_VERSION";
                    mp.mhVersion();
                }
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) mvars.errCode = "-1";




            exBxRd:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.nvBoardcast = svnvBoardcast;
            mvars.deviceID = svdeviceID;
            //mvars.FPGAsel = svFPGAsel;

            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                if (mvars.actFunc != "") lstget1.Items.Insert(0, "↓BoxRead，" + (DateTime.Now - t1).TotalSeconds.ToString("0.0") + "s");
                mvars.strReceive = "DONE,1,16,0,0,10,0," + (DateTime.Now - t1).TotalSeconds.ToString("0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - t1).TotalSeconds.ToString("0")));
            }
            //else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            lstget1.TopIndex = lstget1.Items.Count - 1;
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        public static void cIDONOFF(ListBox lstget1, byte svON)
        {
            //要使用Sendmessage則不要設定 lblcompose = "";
            if (mvars.flgSelf) mvars.lblCompose = "BOXES_IDONOFF";
            mvars.strReceive = "";
            mvars.lCounts = 9999;
            mvars.lCount = 0;
            Array.Resize(ref mvars.lCmd, mvars.lCounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, mvars.lCounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.lCount = 0;
            mvars.errCode = "0";
            mvars.flgDelFB = true;
            mvars.Break = false;
            lstget1.Items.Clear();
            DateTime t1 = DateTime.Now;
            string svdeviceID = mvars.deviceID;

            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";

            if (svON == 1) mvars.lblCmd = "BOXID_SHOW";
            else mvars.lblCmd = "BOXID_OFF";

            if (mvars.demoMode)
            {
                mvars.lCount++;             
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
                mp.mpIDONOFF(svON);

            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) mvars.errCode = "-1";


            exBxRd:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.deviceID = svdeviceID;
            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0") { mvars.strReceive = "DONE,1,16,0,0,10,3,0,0,0,30"; }
            else
            {
                mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27";
                Form1.lstget1.Items.Add("IDONOFF" + Form1.pvindex + "Fail");
            }
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        public static void mReadOCP_Result()   //0x73
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513+4096);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x73;       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x09;       //Size
            funSendMessageTo();
        }

        public static void mWrDmaxmin(UInt16[] Max, UInt16[] Min)   //0x8A
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x8A;      //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;      //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 45;        //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0xC0;      //Wr mcu parameter cmd.
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x77;      //Key
            mvars.RS485_WriteDataBuffer[7 + svns] = 32;        //Length
            for (UInt16 i = 0; i < 8; i++)
            {
                mvars.RS485_WriteDataBuffer[8 + svns + i * 4 + 0] = (Byte)(Max[i] / 256);//max hi
                mvars.RS485_WriteDataBuffer[8 + svns + i * 4 + 1] = (Byte)(Max[i] % 256);//max lo
                mvars.RS485_WriteDataBuffer[8 + svns + i * 4 + 2] = (Byte)(Min[i] / 256);//min hi
                mvars.RS485_WriteDataBuffer[8 + svns + i * 4 + 3] = (Byte)(Min[i] % 256);//min lo
            }
            funSendMessageTo();
        }

        public static void mOCP_Para_Ctrl()   //0x8A
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x8A;                                //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;                                //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 9 + 29;                              //Size
            mvars.RS485_WriteDataBuffer[5 + svns] = 0xE0;                                //Wr mcu parameter cmd.
            mvars.RS485_WriteDataBuffer[6 + svns] = 0x99;                                //Key
            mvars.RS485_WriteDataBuffer[7 + svns] = 26;                                  //Length
            for (UInt16 i = 0; i < 8; i++)
            {
                UInt16 Lmt1st;
                Byte Lmt2nd;
                //if (OCP_Lmt1st2nd_mnl_rbtn.Checked)
                //{
                //    Lmt1st = Convert.ToUInt16(V1_MTP_dGV.Rows[21 + i].Cells[1].Value);
                //    Lmt2nd = Convert.ToByte(V1_MTP_dGV.Rows[29 + i].Cells[1].Value);
                //}
                //else
                //{
                    Lmt1st = 10000;
                    Lmt2nd = 20;
                //}

                mvars.RS485_WriteDataBuffer[8 + svns + i * 2 + 0] = (Byte)(Lmt1st / 256);    //Lmt1st
                mvars.RS485_WriteDataBuffer[8 + svns + i * 2 + 1] = (Byte)(Lmt1st % 256);    //Lmt1st
                mvars.RS485_WriteDataBuffer[8 + svns + 16 + i] = Lmt2nd;                     //Lmt2nd
            }
            UInt16 OCP1stTime = 0;
            mvars.RS485_WriteDataBuffer[8 + svns + 24 + 0] = (Byte)(OCP1stTime / 256);       //OCP1stTime
            mvars.RS485_WriteDataBuffer[8 + svns + 24 + 1] = (Byte)(OCP1stTime % 256);       //OCP1stTime

            funSendMessageTo();
        }


        public static void cAUTOOCP(DataGridView MCU_OCP_Rd_dGV, ListBox lstget1, byte svpostReg, int svpostData, int svdefData, bool svtoExcel, string svexcelName)
        {
            //要使用Sendmessage則不要設定 lblcompose = "";
            if (mvars.flgSelf) mvars.lblCompose = "AUTOOCP";
            mvars.strReceive = "";
            mvars.lCounts = 9999;
            mvars.lCount = 0;
            Array.Resize(ref mvars.lCmd, mvars.lCounts); Array.Clear(mvars.lCmd, 0, mvars.lCmd.Length);
            Array.Resize(ref mvars.lGet, mvars.lCounts); Array.Clear(mvars.lGet, 0, mvars.lGet.Length);
            mvars.lCount = 0;
            mvars.errCode = "0";
            mvars.flgDelFB = true;
            mvars.Break = false;
            lstget1.Items.Clear();
            DateTime t1 = DateTime.Now;

            bool svnvBoardcast = mvars.nvBoardcast;
            mvars.nvBoardcast = false;
            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;
            mvars.FPGAsel = 2;

            bool SW_OCP_1st;
            bool SW_OCP_2nd;

            UInt16[,] DMax = new UInt16[8, 100], DMin = new UInt16[8, 100];
            UInt32[] DMaxCurrent = new UInt32[8], DMinCurrent = new UInt32[8];

            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                #region MCU_OCP_Rd_dGV
                if (MCU_OCP_Rd_dGV != null)
                {
                    //=====================V1 MTP=====================
                    //V1 MTP Read
                    string[] sRow_Name = { "", "Dmax", "Dmin", "MCU", "MCU_F", "FPGA", "FPGA_F", "Diff" };
                    string[] sCol_Name = { "No1", "No2", "No3", "No4", "No5", "No6", "No7", "No8" };

                    //
                    MCU_OCP_Rd_dGV.AllowUserToAddRows = false;
                    MCU_OCP_Rd_dGV.AllowUserToResizeColumns = false;
                    MCU_OCP_Rd_dGV.AllowUserToResizeRows = false;
                    MCU_OCP_Rd_dGV.AllowDrop = false;
                    MCU_OCP_Rd_dGV.ColumnCount = sCol_Name.Length + 1;
                    MCU_OCP_Rd_dGV.RowCount = sRow_Name.Length;
                    MCU_OCP_Rd_dGV.RowHeadersWidth = 10;
                    MCU_OCP_Rd_dGV.RowHeadersVisible = false;
                    MCU_OCP_Rd_dGV.ColumnHeadersVisible = false;

                    //MCU_OCP_Rd_dGV.Location = new Point(1, 1);

                    //Row Name
                    for (UInt16 i = 0; i < sRow_Name.Length; i++)
                    {
                        MCU_OCP_Rd_dGV.Rows[i].Height = 20;
                        MCU_OCP_Rd_dGV.Rows[i].Cells[0].Value = sRow_Name[i];
                    }
                    //Col Name
                    MCU_OCP_Rd_dGV.Columns[0].Width = 48;

                    for (UInt16 i = 0; i < sCol_Name.Length; i++)
                    {
                        MCU_OCP_Rd_dGV.Columns[i + 1].Width = 40;
                        MCU_OCP_Rd_dGV.Rows[0].Cells[i + 1].Value = sCol_Name[i];
                    }
                    MCU_OCP_Rd_dGV.Size = new System.Drawing.Size(5 + MCU_OCP_Rd_dGV.Columns[0].Width +
                        MCU_OCP_Rd_dGV.Columns[1].Width * sCol_Name.Length,
                        5 + MCU_OCP_Rd_dGV.Rows[1].Height * sRow_Name.Length);
                }
                #endregion


                mvars.lblCmd = "OCP_RdVar1";
                mp.mOCP_RdVar1();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                {
                    lstget1.Items.Add(mvars.lblCmd + " 发生异常");
                    mvars.errCode = "-1";
                    goto exBxRd;
                }
                else
                {
                    UInt16 Offset = 6 + 1;
                    Byte MTP_BitField1 = mvars.ReadDataBuffer[Offset + 1],
                        MTP_BitField2 = mvars.ReadDataBuffer[Offset + 2],
                        MTP_BitField3 = mvars.ReadDataBuffer[Offset + 3];
                    //MTP 
                    if ((MTP_BitField1 & 0x01) == 0x01) SW_OCP_1st = true; else SW_OCP_1st = false;
                    if ((MTP_BitField1 & 0x02) == 0x02) SW_OCP_2nd = true; else SW_OCP_2nd = false;
                }

                //Disable
                if (SW_OCP_1st || SW_OCP_2nd)
                {
                    //OCP_EnDis_Ctrl_btn_Click(sender, e);
                    mvars.lblCmd = "OCP_Enable_0";
                    mp.mOCP_EnDis_Ctrl(0);
                    mvars.lblCmd = "MCU_RESET";
                    mp.McuSW_Reset();
                    doDelayms(12000);
                }

                Form1.pvindex = svpostReg;         /// Post -WT for 單屏
                mvars.lblCmd = "FPGA_REG_W";
                mp.mhFPGASPIWRITE(2, svpostData);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                {
                    lstget1.Items.Add(@"切換 ""Post-WT for 單屏"" 发生异常");
                    mvars.errCode = "-1";
                    goto exBxRd;
                }
                doDelayms(200);
                int[] RegDec = { 1, 48, 49, 50 };
                int[] RegData = new int[4];
                RegData[0] = 96; RegData[1] = 0;
                RegData[2] = 0; RegData[3] = 0;
                //mpFPGAUIREGWarr(RegDec, RegData);
                mp.mhFPGASPIWRITE(mvars.FPGAsel, RegDec, RegData);
                doDelayms(3000);
                for (UInt16 m = 0; m < 100; m++)
                {
                    mvars.lblCmd = "OCPRead_Result";
                    mp.mReadOCP_Result();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                    {
                        lstget1.Items.Add(@"切換 ""OCPRead_Result"" 在 count " + m + " 发生异常");
                        mvars.errCode = "-2";
                        goto exBxRd;
                    }
                    else
                    {
                        UInt16[] McuAdc = new UInt16[8], FPGA_GrayScale = new UInt16[8];
                        UInt16[] svDMax = new UInt16[8], svDMin = new UInt16[8];
                        UInt32[] M_Formula = new UInt32[8], F_Formula = new UInt32[8];
                        for (UInt16 i = 0; i < 8; i++)
                        {
                            byte[] McuAdc16b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 0], mvars.ReadDataBuffer[6 + 1 + i * 16 + 1] };
                            McuAdc[i] = BitConverter.ToUInt16(McuAdc16b, 0);
                            byte[] M_Formula32b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 2], mvars.ReadDataBuffer[6 + 1 + i * 16 + 3], mvars.ReadDataBuffer[6 + 1 + i * 16 + 4], mvars.ReadDataBuffer[6 + 1 + i * 16 + 5] };
                            M_Formula[i] = BitConverter.ToUInt32(M_Formula32b, 0);
                            byte[] FPGA_GrayScale16b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 6], mvars.ReadDataBuffer[6 + 1 + i * 16 + 7] };
                            FPGA_GrayScale[i] = BitConverter.ToUInt16(FPGA_GrayScale16b, 0);
                            byte[] F_Formula32b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 8], mvars.ReadDataBuffer[6 + 1 + i * 16 + 9], mvars.ReadDataBuffer[6 + 1 + i * 16 + 10], mvars.ReadDataBuffer[6 + 1 + i * 16 + 11] };
                            F_Formula[i] = BitConverter.ToUInt32(F_Formula32b, 0);
                            byte[] DMax16b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 12], mvars.ReadDataBuffer[6 + 1 + i * 16 + 13] };
                            svDMax[i] = BitConverter.ToUInt16(DMax16b, 0);
                            byte[] DMin16b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 14], mvars.ReadDataBuffer[6 + 1 + i * 16 + 15] };
                            svDMin[i] = BitConverter.ToUInt16(DMin16b, 0);

                        }
                        if (MCU_OCP_Rd_dGV != null)
                        {
                            for (UInt16 i = 0; i < 8; i++)
                            {
                                MCU_OCP_Rd_dGV.Rows[1].Cells[i + 1].Value = svDMax[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[2].Cells[i + 1].Value = svDMin[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[3].Cells[i + 1].Value = McuAdc[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[4].Cells[i + 1].Value = M_Formula[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[5].Cells[i + 1].Value = FPGA_GrayScale[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[6].Cells[i + 1].Value = F_Formula[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[7].Cells[i + 1].Value = ((Int32)M_Formula[i] - F_Formula[i]).ToString();
                            }
                            doDelayms(30);
                            for (UInt16 j = 0; j < 8; j++)
                            {
                                DMin[j, m] = Convert.ToUInt16(MCU_OCP_Rd_dGV.Rows[3].Cells[j + 1].Value);
                                DMinCurrent[j] += DMin[j, m];
                            }
                        }
                        else
                        {
                            //for (UInt16 i = 0; i < 8; i++)
                            //{
                            //    MCU_OCP_Rd_dGV.Rows[1].Cells[i + 1].Value = svDMax[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[2].Cells[i + 1].Value = svDMin[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[3].Cells[i + 1].Value = McuAdc[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[4].Cells[i + 1].Value = M_Formula[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[5].Cells[i + 1].Value = FPGA_GrayScale[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[6].Cells[i + 1].Value = F_Formula[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[7].Cells[i + 1].Value = ((Int32)M_Formula[i] - F_Formula[i]).ToString();
                            //}
                            //doDelayms(30);
                            for (UInt16 j = 0; j < 8; j++)
                            {
                                //DMin[j, m] = Convert.ToUInt16(MCU_OCP_Rd_dGV.Rows[3].Cells[j + 1].Value);
                                DMin[j, m] = McuAdc[j];
                                DMinCurrent[j] += DMin[j, m];
                            }
                        }
                    }
                }

                RegData[0] = 96; RegData[1] = 1023;
                RegData[2] = 1023; RegData[3] = 1023;
                mvars.lblCmd = "FPGA_REG_W";
                //mpFPGAUIREGWarr(RegDec, RegData);
                mp.mhFPGASPIWRITE(mvars.FPGAsel, RegDec, RegData);
                doDelayms(3000);
                for (UInt16 m = 0; m < 100; m++)
                {
                    mvars.lblCmd = "OCPRead_Result";
                    mp.mReadOCP_Result();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                    {
                        lstget1.Items.Add(@"切換 ""OCPRead_Result"" 在 count " + m + " 发生异常");
                        mvars.errCode = "-2";
                        goto exBxRd;
                    }
                    else
                    {
                        UInt16[] McuAdc = new UInt16[8], FPGA_GrayScale = new UInt16[8];
                        UInt16[] svDMax = new UInt16[8], svDMin = new UInt16[8];
                        UInt32[] M_Formula = new UInt32[8], F_Formula = new UInt32[8];
                        for (UInt16 i = 0; i < 8; i++)
                        {
                            byte[] McuAdc16b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 0], mvars.ReadDataBuffer[6 + 1 + i * 16 + 1] };
                            McuAdc[i] = BitConverter.ToUInt16(McuAdc16b, 0);
                            byte[] M_Formula32b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 2], mvars.ReadDataBuffer[6 + 1 + i * 16 + 3], mvars.ReadDataBuffer[6 + 1 + i * 16 + 4], mvars.ReadDataBuffer[6 + 1 + i * 16 + 5] };
                            M_Formula[i] = BitConverter.ToUInt32(M_Formula32b, 0);
                            byte[] FPGA_GrayScale16b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 6], mvars.ReadDataBuffer[6 + 1 + i * 16 + 7] };
                            FPGA_GrayScale[i] = BitConverter.ToUInt16(FPGA_GrayScale16b, 0);
                            byte[] F_Formula32b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 8], mvars.ReadDataBuffer[6 + 1 + i * 16 + 9], mvars.ReadDataBuffer[6 + 1 + i * 16 + 10], mvars.ReadDataBuffer[6 + 1 + i * 16 + 11] };
                            F_Formula[i] = BitConverter.ToUInt32(F_Formula32b, 0);
                            byte[] DMax16b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 12], mvars.ReadDataBuffer[6 + 1 + i * 16 + 13] };
                            svDMax[i] = BitConverter.ToUInt16(DMax16b, 0);
                            byte[] DMin16b = { mvars.ReadDataBuffer[6 + 1 + i * 16 + 14], mvars.ReadDataBuffer[6 + 1 + i * 16 + 15] };
                            svDMin[i] = BitConverter.ToUInt16(DMin16b, 0);

                        }

                        if (MCU_OCP_Rd_dGV != null)
                        {
                            for (UInt16 i = 0; i < 8; i++)
                            {
                                MCU_OCP_Rd_dGV.Rows[1].Cells[i + 1].Value = svDMax[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[2].Cells[i + 1].Value = svDMin[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[3].Cells[i + 1].Value = McuAdc[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[4].Cells[i + 1].Value = M_Formula[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[5].Cells[i + 1].Value = FPGA_GrayScale[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[6].Cells[i + 1].Value = F_Formula[i].ToString();
                                MCU_OCP_Rd_dGV.Rows[7].Cells[i + 1].Value = ((Int32)M_Formula[i] - F_Formula[i]).ToString();
                            }
                            doDelayms(30);
                            for (UInt16 j = 0; j < 8; j++)
                            {
                                DMax[j, m] = Convert.ToUInt16(MCU_OCP_Rd_dGV.Rows[3].Cells[j + 1].Value);
                                DMaxCurrent[j] += DMax[j, m];
                            }
                        }
                        else
                        {
                            //for (UInt16 i = 0; i < 8; i++)
                            //{
                            //    MCU_OCP_Rd_dGV.Rows[1].Cells[i + 1].Value = svDMax[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[2].Cells[i + 1].Value = svDMin[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[3].Cells[i + 1].Value = McuAdc[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[4].Cells[i + 1].Value = M_Formula[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[5].Cells[i + 1].Value = FPGA_GrayScale[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[6].Cells[i + 1].Value = F_Formula[i].ToString();
                            //    MCU_OCP_Rd_dGV.Rows[7].Cells[i + 1].Value = ((Int32)M_Formula[i] - F_Formula[i]).ToString();
                            //}
                            //doDelayms(30);
                            for (UInt16 j = 0; j < 8; j++)
                            {
                                //DMax[j, m] = Convert.ToUInt16(MCU_OCP_Rd_dGV.Rows[3].Cells[j + 1].Value);
                                DMax[j, m] = McuAdc[j];
                                DMaxCurrent[j] += DMax[j, m];
                            }
                        }
                    }
                }

                UInt16[] svMax = new UInt16[8];
                UInt16[] svMin = new UInt16[8];

                for (UInt16 i = 0; i < 8; i++)
                {
                    DMaxCurrent[i] /= 100;
                    DMinCurrent[i] /= 100;
                    if (MCU_OCP_Rd_dGV != null)
                    {
                        MCU_OCP_Rd_dGV.Rows[1].Cells[i + 1].Value = DMaxCurrent[i].ToString();
                        MCU_OCP_Rd_dGV.Rows[2].Cells[i + 1].Value = DMinCurrent[i].ToString();
                    }
                }

                if (svtoExcel && MCU_OCP_Rd_dGV != null)
                {
                    string sLine = "XL Min,1,2,3,4,XL Max,1,2,3,4,XR Min,1,2,3,4,XR Max,1,2,3,4" + Environment.NewLine;
                    for (UInt16 i = 0; i < 100; i++)
                    {
                        sLine += (i + 1).ToString() + "," + DMin[0, i].ToString() + "," + DMin[1, i].ToString() + "," +
                        DMin[2, i].ToString() + "," + DMin[3, i].ToString() + ",," +
                        DMin[4, i].ToString() + "," + DMin[5, i].ToString() + ",," +
                        DMin[6, i].ToString() + "," + DMin[7, i].ToString() + ",," +
                        DMax[0, i].ToString() + "," + DMax[1, i].ToString() + "," +
                        DMax[2, i].ToString() + "," + DMax[3, i].ToString() + ",," +
                        DMax[4, i].ToString() + "," + DMax[5, i].ToString() + "," +
                        DMax[6, i].ToString() + "," + DMax[7, i].ToString() + ",,";
                        sLine += Environment.NewLine;
                    }
                    string pathMaxMinReport = mvars.strLogPath +
                        "OCP_" + svexcelName + "_" + System.DateTime.Now.ToString(("yyyyMMdd_HHmm")) + "_MaxMinReport.csv";
                    File.WriteAllText(pathMaxMinReport, sLine);
                    //if (OCP_OpenFile_chk.Checked == true)
                    //    Process.Start(@pathMaxMinReport);
                }

                RegData[0] = 97; RegData[1] = 0;
                RegData[2] = 0; RegData[3] = 0;
                mvars.lblCmd = "FPGA_REG_W";
                //mpFPGAUIREGWarr(RegDec, RegData);
                mp.mhFPGASPIWRITE(mvars.FPGAsel, RegDec, RegData);
                //Drop On
                mvars.lblCmd = "FPGA_REG_W";
                Form1.pvindex = svpostReg;         /// Post -WT for 單屏
                mp.mhFPGASPIWRITE(2, 13);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                {
                    lstget1.Items.Add("FPGA Reg" + svpostReg + @" 切換 ""13"" 发生异常");
                    mvars.errCode = "-1";
                    goto exBxRd;
                }

                doDelayms(200);

                if (MCU_OCP_Rd_dGV != null)
                {
                    for (UInt16 i = 0; i < 8; i++)
                    {
                        svMax[i] = Convert.ToUInt16(MCU_OCP_Rd_dGV.Rows[1].Cells[i + 1].Value);
                        svMin[i] = Convert.ToUInt16(MCU_OCP_Rd_dGV.Rows[2].Cells[i + 1].Value);
                    }
                }
                else
                {
                    for (UInt16 i = 0; i < 8; i++)
                    {
                        svMax[i] = Convert.ToUInt16(DMaxCurrent[i]);
                        svMin[i] = Convert.ToUInt16(DMinCurrent[i]);
                    }
                }
                mvars.lblCmd = "OCP_WrDmaxmin";
                mWrDmaxmin(svMax, svMin);

                doDelayms(200);

                //SW_OCP_1st_chk.Checked = true;
                //SW_OCP_2nd_chk.Checked = true;
                //OCP_Lmt1st2nd_dft_rbtn.Checked = true;  //Default

                //OCP_Para_Ctrl_btn_Click(sender, e);
                mvars.lblCmd = "OCP_Para_Ctrl";
                mp.mOCP_Para_Ctrl();
                doDelayms(200);

                //OCP_EnDis_Ctrl_btn_Click(sender, e);
                Byte OCP_En = 0;
                OCP_En |= 0x01;
                OCP_En |= 0x02;
                mvars.lblCmd = "OCP_Enable_" + OCP_En;
                mp.mOCP_EnDis_Ctrl(OCP_En);
                //Rd_McuVer_btn_Click(sender, e);
                mvars.lblCmd = "MCU_VERSION";
                mp.mhVersion();
                //Default
                mvars.lblCmd = "FPGA_REG_W";
                Form1.pvindex = svpostReg;         /// Post -WT for 單屏
                mp.mhFPGASPIWRITE(2, svdefData);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                {
                    lstget1.Items.Add(@"FPGA Reg" + svpostReg + " 切換 " + svdefData + " 发生异常");
                    mvars.errCode = "-1";
                    goto exBxRd;
                }

            }




            exBxRd:
            if (mvars.svnova == false && mvars.demoMode == false && mvars.sp1.IsOpen) { mp.CommClose(); }
            mvars.nvBoardcast = svnvBoardcast;
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;

            mvars.flgDelFB = false;
            mvars.lCounts = mvars.lCount + 1;
            mvars.lblCmd = "EndcCMD"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            mvars.flgSend = true; mvars.flgReceived = false;
            if (mvars.errCode == "0")
            {
                if (mvars.actFunc != "") lstget1.Items.Insert(0, "↓AUTOOCP，" + (DateTime.Now - t1).TotalSeconds.ToString("0.0") + "s");
                mvars.strReceive = "DONE,1,16,0,0,10,0," + (DateTime.Now - t1).TotalSeconds.ToString("0") + ",0,0," + (27 + Convert.ToInt16((DateTime.Now - t1).TotalSeconds.ToString("0")));
            }
            //else { mvars.strReceive = "ERROR,1,16,0,0,10,0,0,0,0,27"; }
            lstget1.TopIndex = lstget1.Items.Count - 1;
            mvars.tmeRSIn.Enabled = true;
            mvars.flgReceived = true;
        }


        public static void mOCP_RdVar1()   //0x8B
        {
            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            //if (mvars.deviceID == "" || mp.HexToDec(mvars.deviceID) >= 4096) svns = 0;

            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 2049);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion
            mvars.RS485_WriteDataBuffer[2 + svns] = 0x8B;       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x09;       //Size
            funSendMessageTo();
        }


        #endregion OCP ============================================================= -





        public static void showStatus1(string svmsg, ListBox lst, string svLogHeadline)
        {
            if (Form1.dgvformmsg.Visible == true)
            {
                if (svmsg != "")
                {
                    Form1.dgvformmsg.Rows[mvars.dgvRows].Cells[0].Value = DateTime.Now.ToString("hh:mm:ss");
                    Form1.dgvformmsg.Rows[mvars.dgvRows].Cells[1].Value = svmsg;
                    if (mvars.dgvRows > 10) Form1.dgvformmsg.FirstDisplayedScrollingRowIndex = mvars.dgvRows - 10;
                }
                mvars.dgvRows++;
            }
            else if (mvars.FormShow[6])
            {
                if (svmsg != "")
                {
                    i3_Init.dgvformmsg.Rows[mvars.dgvRows].Cells[0].Value = DateTime.Now.ToString("hh:mm:ss");
                    i3_Init.dgvformmsg.Rows[mvars.dgvRows].Cells[1].Value = svmsg;
                    if (mvars.dgvRows > 10) i3_Init.dgvformmsg.FirstDisplayedScrollingRowIndex = mvars.dgvRows - 10;
                }
                mvars.dgvRows++;
            }
            else
            {
                lst.Items.Add(svmsg);
                lst.TopIndex = lst.Items.Count - 1;
            }
            if (svmsg != "" || svLogHeadline != "") mp.funSaveLogs(svLogHeadline + svmsg);
        }


        public static void mhFPGASPIWRITEasc(string svData, string svData2, byte svonoff)   //0x20 新版正確
        {
            // sample
            //mvars.lblCmd = "FPGA_SPI_WASC";             //Char 
            //mp.mhFPGASPIWRITEasc("", "", 1);
            //mp.mhFPGASPIWRITEasc(svs, mvars.verFPGA + "_" + svhwCard[svi].CurrentBootVer + "." + svhwCard[svi].CurrentAppVer + "-" + svuser, 1);
            //



            #region 2023版公用程序 (無 Nova 參數)
            byte svns = 1;
            Array.Resize(ref mvars.RS485_WriteDataBuffer, 513);
            Array.Resize(ref mvars.ReadDataBuffer, 513);
            Array.Clear(mvars.ReadDataBuffer, 0, mvars.ReadDataBuffer.Length);
            Array.Clear(mvars.RS485_WriteDataBuffer, 0, mvars.RS485_WriteDataBuffer.Length);
            #endregion

            mvars.RS485_WriteDataBuffer[2 + svns] = 0x20;       //Cmd
            mvars.RS485_WriteDataBuffer[3 + svns] = 0x00;       //Size
            mvars.RS485_WriteDataBuffer[4 + svns] = 0x2A;       //Size
                                                                //byte[] WriteSizeArr = { mvars.RS485_WriteDataBuffer[4 + 1], mvars.RS485_WriteDataBuffer[3 + 1] };
                                                                //UInt16 WriteSize = BitConverter.ToUInt16(WriteSizeArr, 0);
            if (svonoff > 0) svonoff = 1;
            mvars.RS485_WriteDataBuffer[5 + svns] = svonoff;    //Display On
            for (int i = 0; i < 16; i++)
            {
                if ((i + 1) > svData.Length)
                    mvars.RS485_WriteDataBuffer[6 + svns + i] = 32;
                else
                    mvars.RS485_WriteDataBuffer[6 + svns + i] = ASCII_GetByte(svData.Substring(i, 1));
                if ((i + 1) > svData2.Length)
                    mvars.RS485_WriteDataBuffer[22 + svns + i] = 32;
                else
                    mvars.RS485_WriteDataBuffer[22 + svns + i] = ASCII_GetByte(svData2.Substring(i, 1));
            }
            mp.funSendMessageTo();
        }


        public static string panelsemi_gmax(int svG, float svUUTCLv)
        {
            string drun = "0";
            string Svs = "0";
            byte svCount = 1;
            byte svCounts = 60;
            byte Svover = 0;
            string svs1 = "";
            byte svC;
            int svVs;
            int svg1;
            UInt32 ulCRC;
            byte svDs = (byte)(mvars.dualduty * (mvars.cm603Gamma.Length + 1));     /// column起始 duty0=0，duty1=14
            int svr;
            for (svr = 0; svr < svG; svr++) { uc_atg.dgvatg.Rows[svr].Cells[svG + svDs].Style.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255); }


            int sH = 4 + (gmax - svG) * 2 + (mvars.dualduty * 60);   /// duty0:4+(12-12)*2 = 4    duty1:4+60 = 64
            int sL = 5 + (gmax - svG) * 2 + (mvars.dualduty * 60);   /// duty0:5+(12-12)*2 = 5    duty1:5+60 = 65


            do
            {
                CAmeasF();
                if (CAFxLv == -1)
                {
                    if (Svover > 4)
                    {
                        Svs = "-9";
                        uc_atg.lstget.Items.Add(" ---> ATG_fail(" + Svs + ")   ID: " + mvars.UUT.ID + "  W" + mvars.cm603Gamma[svG] + " Lv = -1 @ DRun" + svCount.ToString("00") + "-" + Svover + "," + CAFxLv + ",x" + CAFx + ",y" + CAFy);
                        goto Ex;
                    }
                    else
                    {
                        svs1 = ext.cm603v(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                        for (svC = 0; svC <= 2; svC++)
                        {
                            uc_atg.dgvatg.Rows[8 + svC * 2].Cells[svG + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svG * 2];
                            uc_atg.dgvatg.Rows[9 + svC * 2].Cells[svG + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svG * 2 + 1];
                            svVs = HexToDec(mvars.pGMA[mvars.dualduty].Data[svC, svG * 2] + mvars.pGMA[mvars.dualduty].Data[svC, svG * 2 + 1]);

                            ulCRC = ext.cCal(Convert.ToUInt16(svVs), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                            mvars.cm603df[svC, sH] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(0, 2);
                            mvars.cm603df[svC, sL] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(2, 2);
                            mvars.cm603dfB[svC, sH] = (byte)mp.HexToDec(mvars.cm603df[svC, sH]);
                            mvars.cm603dfB[svC, sL] = (byte)mp.HexToDec(mvars.cm603df[svC, sL]);
                        }
                    }
                    Svover++;
                    doDelayms(100);
                    goto reDRunW;
                }
                else if (CAFxLv <= 0)
                {
                    if (Svover > 4)
                    {
                        Svs = "-11";
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + Svs + ")   ID: " + mvars.UUT.ID + "  W" + mvars.cm603Gamma[svG] + "Lv < 5 @ DRun count" + svCount.ToString("00") + "-" + Svover + "," + CAFxLv + ",x" + CAFx + ",y" + CAFy);
                        goto Ex;
                    }
                    Svover++;
                    doDelayms(100);
                    goto reDRunW;
                }
                else Svover = 0;

                uc_atg.dgvatg.Rows[0].Cells[svG + svDs].Value = svCount;
                uc_atg.dgvatg.Rows[1].Cells[svG + svDs].Value = CAFxLv;
                uc_atg.dgvatg.Rows[2].Cells[svG + svDs].Value = CAFx;
                uc_atg.dgvatg.Rows[3].Cells[svG + svDs].Value = CAFy;
                if (Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svG + svDs].Value) > 0)
                {
                    uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value = (100 * (CAFxLv / Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svG + svDs].Value) - 1)).ToString("#0.0");
                    uc_atg.lstget.Items.Add(" -> G" + mvars.cm603Gamma[svG] + " Count: " + svCount + "，Lv" + CAFxLv + "(" + uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value + ")，x" + CAFx + "(" +
                        uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value + ")，y" + CAFy + "(" + uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value + ")");
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                }
                uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value = Math.Abs(Math.Round(CAFx - mvars.UUT.Cx, 4)).ToString();
                uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value = Math.Abs(Math.Round(CAFy - mvars.UUT.Cy, 4)).ToString();

                Application.DoEvents();
                if (Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) >= 0 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) <= 5 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) <= 0.002 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) <= 0.002)
                {
                    if (svCount < svCounts - 20)
                    {
                        svCount = (byte)(svCounts - 20);
                        svs1 = ext.cm603v(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                        goto detail;
                    }
                    uc_atg.lstget.Items.Add(" ---> G" + mvars.cm603Gamma[svG] + " | " +
                        uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value + " | <= 0.002(spec)" + "，| " +
                        uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value + " | <= 0.002(spec)" + "，" +
                        uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value + " @ 0 ~ 5%(spec)");
                    uc_atg.lstget.Items.Add("");
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                    break;
                }

            detail:
                svs1 = ext.cm603v(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                for (svC = 0; svC <= 2; svC++)
                {
                    for (svg1 = svG; svg1 >= 1; svg1--)
                    {
                        uc_atg.dgvatg.Rows[8 + svC * 2].Cells[svg1 + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2];
                        uc_atg.dgvatg.Rows[9 + svC * 2].Cells[svg1 + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2 + 1];
                        svVs = HexToDec(mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2] + mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2 + 1]);
                        ulCRC = ext.cCal(Convert.ToUInt16(svVs), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        sH = 4 + (gmax - svg1) * 2 + (mvars.dualduty * 60);
                        sL = 5 + (gmax - svg1) * 2 + (mvars.dualduty * 60);
                        mvars.cm603df[svC, sH] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(0, 2);
                        mvars.cm603df[svC, sL] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(2, 2);
                        mvars.cm603dfB[svC, sH] = (byte)mp.HexToDec(mvars.cm603df[svC, sH]);
                        mvars.cm603dfB[svC, sL] = (byte)mp.HexToDec(mvars.cm603df[svC, sL]);
                    }
                }

            reDRunW:
                mvars.lblCmd = "GammaSet_DRun";
                mp.Gamma_603_Drun();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                {
                    mvars.ATGerr = "-14";
                    uc_atg.lstget.Items.Add(" ---> ATG_fail(" + Svs + ")   ID: " + mvars.UUT.ID + "  W" + mvars.cm603Gamma[svG] + " GammaSet fail @ DRun");
                }
                else { doDelayms(200); Svs = "0"; }
                uc_atg.lstget.Items.Add(" ---> DRun g" + mvars.cm603Gamma[svG] + svs1); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                uc_atg.lstget.Items.Add("");
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                svCount++;

                if (mvars.Break == true) { break; }
            } while (svCount <= svCounts && mvars.Break == false && mvars.byPass == false);

        Ex:
            if (mvars.Break) drun = "-10";
            if (Svs == "0" && svCount > svCounts) drun = "-1";
            if (Svs != "0" && Svs.Substring(0, 1) == "-") drun = Svs;
            else if (Svs != "0" && Svs.Substring(0, 1) != "-") drun = "-20";
            else if (svCount > svCounts) drun = "-1";
            return drun;
        }

        public static string panelsemi_B4(int svG, float svUUTCLv)
        {
            string drun = "0";
            string svs = "0";
            byte svCount = 1;
            byte svover = 0;
            byte svC;
            int svVs;
            string svs1;
            int svg1;
            UInt32 ulCRC;
            byte svDs = (byte)(mvars.dualduty * (mvars.cm603Gamma.Length + 1));     /// column起始 duty0=0，duty1=14


            int svCounts = 120;
            if (svG > 1 && svG <= 3) { svCounts = 80; }
            else if (svG > 3) { svCounts = 70; }

            float svY = svUUTCLv;
            float svX = Convert.ToSingle((mvars.UUT.Cx * svY / mvars.UUT.Cy).ToString("####0.0####"));
            float svZ = Convert.ToSingle((svX / mvars.UUT.Cx - svX - svY).ToString("####0.0####"));

            for (int svrow = 0; svrow < uc_atg.dgvatg.RowCount; svrow++) { uc_atg.dgvatg.Rows[svrow].Cells[svG + svDs].Style.ForeColor = System.Drawing.Color.FromArgb(255, 0, 255); }

            ShowAGMAnT("X", 0, 0, 0, true, mvars.cm603Gamma[svG], mvars.cm603Gamma[svG], mvars.cm603Gamma[svG], false, 500);

            do
            {
                CAmeasF();
                if (CAFxLv == -1)
                {
                    svs = "-9";
                    uc_atg.lstget.Items.Add(" ---> ATG_fail(" + svs + ")   ID: " + mvars.UUT.ID + "  W" + svG + "Lv = -1 @ DRun_cm603_B4 " + svCount.ToString("00") + "-" + svover + "," + CAFxLv + ",x" + CAFx + ",y" + CAFy);
                    goto Ex;
                }
                else svover = 0;

                uc_atg.dgvatg.Rows[0].Cells[svG + svDs].Value = svCount;
                uc_atg.dgvatg.Rows[1].Cells[svG + svDs].Value = CAFxLv;
                uc_atg.dgvatg.Rows[2].Cells[svG + svDs].Value = CAFx;
                uc_atg.dgvatg.Rows[3].Cells[svG + svDs].Value = CAFy;
                if (Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svG + svDs].Value) > 0)
                {
                    uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value = (100 * (CAFxLv / Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svG + svDs].Value) - 1)).ToString("#0.0");
                    uc_atg.lstget.Items.Add(" -> G" + mvars.cm603Gamma[svG] + " Count: " + svCount + "，Lv" + CAFxLv + "(" + uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value + ")，x" + CAFx + "(" +
                        uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value + ")，y" + CAFy + "(" + uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value + ")");
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                }
                uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value = string.Format("{0:0.0###}", Math.Abs(CAFx - mvars.UUT.Cx));
                uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value = string.Format("{0:0.0###}", Math.Abs(CAFy - mvars.UUT.Cy));

                Application.DoEvents();
                if (mvars.cm603Gamma[svG] >= 160)
                {
                    if (Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) > -3 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) <= 5 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) <= 0.004 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) <= 0.004)
                    {
                        if (svCount < 25)
                        {
                            svCount = (byte)(25);
                            svs1 = ext.cm603B4(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                            goto detail;
                        }

                        uc_atg.lstget.Items.Add(" ---> DRun_cm603_B4 G" + mvars.cm603Gamma[svG] + " | " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) + " | <= 0.004" + "，| " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) + " | <= 0.004" + "，" +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) + " @ -3 ~ 5%");
                        uc_atg.lstget.Items.Add("");
                        uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                        break;
                    }
                }
                else if (mvars.cm603Gamma[svG] >= 40 && mvars.cm603Gamma[svG] < 160)
                {
                    if (Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) >= -3 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) <= 6 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) <= 0.008 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) <= 0.008)
                    {
                        if (svCount < 25)
                        {
                            svCount = (byte)(25);
                            svs1 = ext.cm603B4(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                            goto detail;
                        }

                        uc_atg.lstget.Items.Add(" ---> DRun_cm603_B4 G" + mvars.cm603Gamma[svG] + " | " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) + " | <= 0.008" + "，| " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) + " | <= 0.008" + "，" +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) + " @ -3 ~ 6%");
                        uc_atg.lstget.Items.Add("");
                        uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                        break;
                    }
                }
                else if (mvars.cm603Gamma[svG] >= 24 && mvars.cm603Gamma[svG] < 40)
                {
                    if (Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) >= -3 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) <= 6 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) <= 0.008 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) <= 0.008)
                    {
                        if (svCount < 25)
                        {
                            svCount = (byte)(25);
                            svs1 = ext.cm603B4(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                            goto detail;
                        }

                        uc_atg.lstget.Items.Add(" ---> DRun_cm603_B4 G" + mvars.cm603Gamma[svG] + " | " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) + " | <= 0.008" + "，| " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) + " | <= 0.008" + "，" +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) + " @ -3 ~ 6%");
                        uc_atg.lstget.Items.Add("");
                        uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                        break;
                    }
                }
                else if (mvars.cm603Gamma[svG] > 1 && mvars.cm603Gamma[svG] < 24)
                {
                    if (Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) > -5 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) <= 6 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) <= 0.009 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) <= 0.009)
                    {
                        if (svCount < 25)
                        {
                            svCount = (byte)(25);
                            svs1 = ext.cm603B4(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                            goto detail;
                        }

                        uc_atg.lstget.Items.Add(" ---> DRun_cm603_B4 G" + mvars.cm603Gamma[svG] + " | " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) + " | <= 0.009" + "，| " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) + " | <= 0.009" + "，" +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) + " @ -5 ~ 6%");
                        uc_atg.lstget.Items.Add("");
                        uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

                        break;
                    }
                }
                else if (mvars.cm603Gamma[svG] <= 1)
                {
                    if (Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) >= -40 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) <= 50 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) <= 0.12 &&
                        Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) <= 0.12)
                    {
                        if (svCount < 25)
                        {
                            svCount = (byte)(25);
                            svs1 = ext.cm603B4(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                            goto detail;
                        }

                        uc_atg.lstget.Items.Add(" ---> DRun_cm603_B4 G" + mvars.cm603Gamma[svG] + " | " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) + " | <= 0.12" + "，| " +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) + " | <= 0.12" + "，" +
                            Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) + " @ -40 ~ 50%");
                        uc_atg.lstget.Items.Add("");
                        uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

                        break;
                    }
                }

            detail:
                svs1 = ext.cm603B4(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy, svCount - 1);
                for (svC = 0; svC <= 2; svC++)
                {
                    for (svg1 = svG; svg1 >= 1; svg1--)
                    {
                        uc_atg.dgvatg.Rows[8 + svC * 2].Cells[svg1 + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2];
                        uc_atg.dgvatg.Rows[9 + svC * 2].Cells[svg1 + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2 + 1];
                        svVs = HexToDec(mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2] + mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2 + 1]);
                        ulCRC = ext.cCal(Convert.ToUInt16(svVs), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        int sH = 4 + (gmax - svg1) * 2 + (mvars.dualduty * 60);
                        int sL = 5 + (gmax - svg1) * 2 + (mvars.dualduty * 60);
                        mvars.cm603df[svC, sH] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(0, 2);
                        mvars.cm603df[svC, sL] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(2, 2);
                        mvars.cm603dfB[svC, sH] = (byte)mp.HexToDec(mvars.cm603df[svC, sH]);
                        mvars.cm603dfB[svC, sL] = (byte)mp.HexToDec(mvars.cm603df[svC, sL]);
                    }
                }

            reDRunW:
                mvars.lblCmd = "GammaSet_DRunB4";
                mp.Gamma_603_Drun();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                {
                    svs = "-14";
                    uc_atg.lstget.Items.Add(" ---> ATG_fail(" + svs + ")   ID: " + mvars.UUT.ID + "  W" + svG + " GammaSet fail @ DRun_cm603_B4");
                }
                else { doDelayms(200); svs = "0"; }
                uc_atg.lstget.Items.Add(" ---> DRun_cm603_B4 G" + mvars.cm603Gamma[svG] + svs1);
                uc_atg.lstget.Items.Add("");
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                svCount++;

                if (mvars.Break == true) { break; }
            } while (svCount <= svCounts && mvars.Break == false && mvars.byPass == false);
            uc_atg.dgvatg.Rows[0].Cells[svG + svDs].Value = svCount;
        Ex:
            uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
            if (mvars.Break) drun = "-10";
            if (svs == "0")
            {
                if (svG == 0 && (Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[svG + svDs].Value) < (mvars.UUT.DLvLimit - mvars.UUT.DLvTolminus) || Convert.ToSingle(uc_atg.dgvatg.Rows[1].Cells[svG + svDs].Value) > (mvars.UUT.DLvLimit + mvars.UUT.DLvTolplus)))
                {
                    drun = "-22";
                }
                else if (svCount > svCounts) drun = "-1";
            }
            if (svs != "0" && svs.Substring(0, 1) == "-") drun = svs;
            else if (svs != "0" && svs.Substring(0, 1) != "-") drun = "-20";
            else if (svCount > svCounts) drun = "-1";
            return drun;
        }

        public static string panelsemi_F(int svG, float svUUTCLv)
        {
            string drun = "0";
            string Svs = "0";
            int[] Svstep = new int[] { 1, 1, 1 };
            byte svover = 0;
            byte svCount = 1;
            string svs1 = "";
            byte svC;
            int svVs = 0;
            string svText6;
            //byte svDs = 0;
            byte svDs = (byte)(mvars.dualduty * (mvars.cm603Gamma.Length + 1));     /// column起始 duty0=0，duty1=14
            UInt32 ulCRC;

            float svY = Convert.ToSingle((svUUTCLv * (mvars.Gamma2d2[svG] / 100)).ToString("####0.#####"));
            float svX = Convert.ToSingle((mvars.UUT.Cx * svY / mvars.UUT.Cy).ToString("####0.#####"));
            float svZ = Convert.ToSingle((svX / mvars.UUT.Cx - svX - svY).ToString("####0.#####"));

            int svr;
            for (svr = 0; svr < gmax; svr++) { uc_atg.dgvatg.Rows[svr].Cells[svG + svDs].Style.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255); }

            int sH = (((mvars.pGMA[mvars.dualduty].Data.Length / 3) - 2) - (svG - gmax) * 2) - 8;   //8
            int sL = (((mvars.pGMA[mvars.dualduty].Data.Length / 3) - 2) - (svG - gmax) * 2) - 7;   //9

            do
            {
                CAmeasF();
                if (CAFxLv == -1)
                {
                    if (svover > 4)
                    {
                        Svs = "-9";
                        uc_atg.lstget.Items.Add(" ---> ATG_fail(" + Svs + ")   ID: " + mvars.UUT.ID + "  W" + svG + "Lv = -1 @ DRunF_cm603 " + svCount.ToString("00") + "-" + svover + "," + CAFxLv + ",x" + CAFx + ",y" + CAFy);
                        goto Ex;
                    }
                    else
                    {
                        svs1 = ext.cm603F(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy);
                        for (svC = 0; svC <= 2; svC++)
                        {
                            uc_atg.dgvatg.Rows[8 + svC * 2].Cells[svG + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svG * 2];
                            uc_atg.dgvatg.Rows[9 + svC * 2].Cells[svG + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svG * 2 + 1];
                            svVs = HexToDec(mvars.pGMA[mvars.dualduty].Data[svC, svG * 2] + mvars.pGMA[mvars.dualduty].Data[svC, svG * 2 + 1]);

                            ulCRC = ext.cCal(Convert.ToUInt16(svVs), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                            mvars.cm603df[svC, sH] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(0, 2);
                            mvars.cm603df[svC, sL] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(2, 2);
                            mvars.cm603dfB[svC, sH] = (byte)mp.HexToDec(mvars.cm603df[svC, sH]);
                            mvars.cm603dfB[svC, sL] = (byte)mp.HexToDec(mvars.cm603df[svC, sL]);
                        }
                    }
                    svover++;
                    doDelayms(500);
                    goto reDRunW;
                }
                else if (CAFxLv < 10)
                {
                    if (svover > 4)
                    {
                        Svs = "-11";
                        uc_atg.lstget.Items.Add(" --> ATG_fail(" + Svs + ")   ID: " + mvars.UUT.ID + "  W" + svG + " Lv <10 @ DRunF_cm603 " + svCount.ToString("00") + "-" + svover + "," + CAFxLv + ",x" + CAFx + ",y" + CAFy);
                        goto Ex;
                    }
                    svover++;
                    doDelayms(500);
                    goto reDRunW;
                }
                else svover = 0;

                uc_atg.dgvatg.Rows[1].Cells[svG + svDs].Value = CAFxLv;
                uc_atg.dgvatg.Rows[2].Cells[svG + svDs].Value = CAFx;
                uc_atg.dgvatg.Rows[3].Cells[svG + svDs].Value = CAFy;
                if (Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svG + svDs].Value) > 0)
                {
                    uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value = Math.Abs(100 * (1 - (CAFxLv / Convert.ToSingle(uc_atg.dgvatg.Rows[7].Cells[svG + svDs].Value)))).ToString("#0.0");
                    uc_atg.lstget.Items.Add(" -> G" + mvars.cm603Gamma[svG] + "，Lv" + CAFxLv + "(" + uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value + ")，x" + CAFx + "(" +
                        uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value + ")，y" + CAFy + "(" + uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value + ")");
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                }
                uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value = string.Format("{0:0.0###}", Math.Abs(CAFx - mvars.UUT.Cx));
                uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value = string.Format("{0:0.0###}", Math.Abs(CAFy - mvars.UUT.Cy));

                Application.DoEvents();
                if (Convert.ToSingle(uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value) <= 5 &&
                    Convert.ToSingle(uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value) <= 0.003 &&
                    Convert.ToSingle(uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value) <= 0.003)
                {
                    uc_atg.lstget.Items.Add(" -> G" + mvars.cm603Gamma[svG] + " | " +
                        uc_atg.dgvatg.Rows[5].Cells[svG + svDs].Value + " | <= 0.003" + " , | " +
                        uc_atg.dgvatg.Rows[6].Cells[svG + svDs].Value + " | <= 0.003" + " , " +
                        uc_atg.dgvatg.Rows[4].Cells[svG + svDs].Value + " @ 0 ~ 5%(spec)");
                    uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                    break;
                }
                else uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;

                svs1 = ext.cm603F(svG, ref mvars.pGMA[mvars.dualduty].Data, svUUTCLv, mvars.UUT.Cx, mvars.UUT.Cy, CAFxLv, CAFx, CAFy);
                for (svC = 0; svC <= 2; svC++)
                {
                    for (int svg1 = svG; svg1 >= 1; svg1--)
                    {
                        uc_atg.dgvatg.Rows[8 + svC * 2].Cells[svg1 + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2];
                        uc_atg.dgvatg.Rows[9 + svC * 2].Cells[svg1 + svDs].Value = mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2 + 1];
                        svVs = HexToDec(mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2] + mvars.pGMA[mvars.dualduty].Data[svC, svg1 * 2 + 1]);
                        ulCRC = ext.cCal(Convert.ToUInt16(svVs), (UInt16)mp.BinToDec("10011"), 10) * (UInt32)Math.Pow(2, 10);
                        sH = 4 + (gmax - svg1) * 2 + (mvars.dualduty * 60);
                        sL = 5 + (gmax - svg1) * 2 + (mvars.dualduty * 60);
                        mvars.cm603df[svC, sH] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(0, 2);
                        mvars.cm603df[svC, sL] = mp.DecToHex((int)ulCRC + svVs, 4).Substring(2, 2);
                        mvars.cm603dfB[svC, sH] = (byte)mp.HexToDec(mvars.cm603df[svC, sH]);
                        mvars.cm603dfB[svC, sL] = (byte)mp.HexToDec(mvars.cm603df[svC, sL]);
                    }
                }
            reDRunW:
                //svb = ShowAGMA("X", 0, 0, 0, true, 0, 0, 0, false);
                mvars.lblCmd = "GammaSet_DRun"; mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.Gamma_603();
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1)
                {
                    mvars.ATGerr = "-14";
                    uc_atg.lstget.Items.Add(" --> ATG_fail(" + Svs + ")   ID: " + mvars.UUT.ID + "  W" + svG + " GammaSet fail @ DRunF_cm603");
                }
                else { doDelayms(200); Svs = "0"; }//1500>>500
                uc_atg.lstget.Items.Add(" -> DRunF_cm603 g" + mvars.cm603Gamma[svG] + " XYZ (Msr：Tar) " + svCount.ToString("00") + svs1); uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                uc_atg.lstget.Items.Add("");
                uc_atg.lstget.TopIndex = uc_atg.lstget.Items.Count - 1;
                svCount++;
                //
                if (mvars.Break == true) { break; }
            } while (svCount <= 50 && mvars.Break == false && mvars.byPass == false);

        Ex:
            if (mvars.Break) drun = "-10";
            if (Svs == "0" && svCount > 50) drun = "-1";
            if (Svs != "0" && Svs.Substring(0, 1) == "-") drun = Svs;
            else if (Svs != "0" && Svs.Substring(0, 1) != "-") drun = "-20";
            else if (svCount > 50) drun = "-1";
            return drun;
        }





    }
}