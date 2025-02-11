using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System.Windows;



namespace AIO
{
    class mscrPhysics
    {
        #region 獲取Windows10屏幕的縮放比例
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hDc);

        /// <summary>
        /// DeviceCap 常量
        /// </summary>
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;



        /// <summary>
        /*
        foreach (var screen in System.Windows.Forms.Screen.AllScreens)
        {
         Console.WriteLine("設備名稱:" + screen.DeviceName);//裝置名稱
         Console.WriteLine("Bounds:" + screen.Bounds.ToString( )); //螢幕解析度
         Console.WriteLine("工作區：" + screen.WorkingArea.ToString()); //實際工作區域
        }

        參考來源：
        http://msdn.microsoft.com/en-us/library/system.windows.forms.screen(v=vs.110).aspx




        获取所有窗体的显示比率：

        Screen[] s = Screen.AllScreens;

        ScreensRect = new Rectangle[s.Length];

        for (int i = 0; i < s.Length; i++)

        {

            ScreensRect[i] = s[i].WorkingArea;

        }

        获取：

        int iX = ScreensRect[1].X;

        int iY = ScreensRect[1].Y;
        ————————————————
        版权声明：本文为CSDN博主「hejialin666」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
        原文链接：https://blog.csdn.net/hejialin666/article/details/6057551



        */
        /// </summary>
        /// <returns></returns>



        public static Size Getsize()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            Size size = new Size();
            size.Width=GetDeviceCaps(hdc, DESKTOPHORZRES);
            size.Height=GetDeviceCaps(hdc, DESKTOPVERTRES);
            return size;
        }
        public static double GetW()
        {
            IntPtr hdc=GetDC(IntPtr.Zero);
            Size size = new Size();
            size.Width = GetDeviceCaps(hdc, DESKTOPHORZRES);
            size.Height = GetDeviceCaps(hdc, DESKTOPVERTRES);
            return (double)size.Width;
        }

        


        //public enum DeviceCap
        //{
        //    VERTRES = 10,
        //    PHYSICALWIDTH = 110,
        //    SCALINGFACTORX = 114,
        //    DESKTOPVERTRES = 117,

        //    //  http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html 
        //}


        public static double GetScreenScalingFactor()
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            var physicalScreenHeight = GetDeviceCaps(desktop, DESKTOPVERTRES);

            //var screenScalingFactor =
            //    (double)physicalScreenHeight / Screen.PrimaryScreen.Bounds.Height;
            // SystemParameters.PrimaryScreenHeight;

            //return screenScalingFactor;
            return 0d;
        }





        #endregion



    }
}
