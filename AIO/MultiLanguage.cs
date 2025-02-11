using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIO
{
    class MultiLanguage
    {
        public static string prelan = "";
        //變數DefaultLanguage，用於儲存當前預設語言
        public static string DefaultLanguage = "zh-CN";
        //public static string DefaultLanguage = "en-US";
        //public static string DefaultLanguage = "zh-CHT";

        //函式SetDefaultLanguage修改當前預設語言
        /// <summary>
        /// 修改預設語言
        /// </summary>
        /// <param name="lang">待設定預設語言</param>
        public static void SetDefaultLanguage(string lang)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            DefaultLanguage = lang;
            Properties.Settings.Default.DefaultLanguage = lang;
            Properties.Settings.Default.Save();
        }

        //函式LoadLanguage用於載入語言或切換語言
        /// <summary>
        /// 載入語言
        /// </summary>
        /// <param name="form">載入語言的視窗</param>
        /// <param name="formType">視窗的型別</param>
        public static void LoadLanguage(Form form, Type formType)
        {
            if (form != null)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(formType);
                resources.ApplyResources(form, "$this");
                Loading(form, resources);
            }
        }

        /// <summary>
        /// 載入語言
        /// </summary>
        /// <param name="control">控制元件</param>
        /// <param name="resources">語言資源</param>
        private static void Loading(Control control, System.ComponentModel.ComponentResourceManager resources)
        {
            if (control is MenuStrip)
            {
                //將資源與控制元件對應
                resources.ApplyResources(control, control.Name);
                MenuStrip ms = (MenuStrip)control;
                if (ms.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem c in ms.Items)
                    {
                        //遍歷選單
                        Loading(c, resources);
                    }
                }
            }
            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                Loading(c, resources);
            }
        }

        /// <summary>
        /// 遍歷選單
        /// </summary>
        /// <param name="item">選單項</param>
        /// <param name="resources">語言資源</param>
        private static void Loading(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem)
            {
                if (item.Name != "h_pid") resources.ApplyResources(item, item.Name);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if (tsmi.DropDownItems.Count > 0)
                {
                    for (int i = 0; i < tsmi.DropDownItems.Count; i++)
                    {
                        if (tsmi.DropDownItems[i].GetType().Name == "ToolStripMenuItem")
                        {
                            ToolStripMenuItem c = (ToolStripMenuItem)tsmi.DropDownItems[i];
                            Loading(c, resources);
                        }
                    }
                    //foreach (ToolStripMenuItem c in tsmi.DropDownItems)
                    //{
                    //    Loading(c, resources);
                    //}
                }
            }
        }



    }


    public static class JapanHelper
    {
        /// <summary>
        /// 設置當前程序的界面語言
        /// </summary>
        /// <param name="lang">language:zh-CN, en-US</param>
        /// <param name="form">窗體實例</param>
        /// <param name="formType">窗體類型</param>
        public static void SetLang(string lang, Form form)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            if (form != null)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(form.GetType());
                resources.ApplyResources(form, "$this");
                AppLang(form, resources);
            }
        }

        /// <summary>
        /// 遍歷窗體所有控件，針對其設置當前界面語言
        /// </summary>
        /// <param name="control"></param>
        /// <param name="resources"></param>
        private static void AppLang(Control control, System.ComponentModel.ComponentResourceManager resources)
        {
            if (control is MenuStrip)
            {
                //將資源應用與對應的屬性
                resources.ApplyResources(control, control.Name);
                MenuStrip ms = (MenuStrip)control;
                if (ms.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem c in ms.Items)
                    {
                        //調用 遍歷菜單 設置語言
                        AppLang(c, resources);
                    }
                }
            }

            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                AppLang(c, resources);
            }
        }
        /// <summary>
        /// 遍歷菜單
        /// </summary>
        /// <param name="item"></param>
        /// <param name="resources"></param>
        private static void AppLang(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem)
            {
                resources.ApplyResources(item, item.Name);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if (tsmi.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem c in tsmi.DropDownItems)
                    {
                        //if (tsmi != ToolStripSeparator)
                        //{ }
                        AppLang(c, resources);
                    }
                }
            }
        }
    }

    public static class uclan
    {
        /// <summary>
        /// 設置當前程序的界面語言
        /// </summary>
        /// <param name="lang">language:zh-CN, en-US</param>
        /// <param name="uc">窗體實例</param>
        public static void SetLang(string lang, UserControl uc)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            if (uc != null)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(uc.GetType());
                resources.ApplyResources(uc, "$this");
                AppLang(uc, resources);
            }
        }

        /// <summary>
        /// 遍歷窗體所有控件，針對其設置當前界面語言
        /// </summary>
        /// <param name="control"></param>
        /// <param name="resources"></param>
        private static void AppLang(Control control, System.ComponentModel.ComponentResourceManager resources)
        {
            if (control is MenuStrip)
            {
                //將資源應用與對應的屬性
                resources.ApplyResources(control, control.Name);
                MenuStrip ms = (MenuStrip)control;
                if (ms.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem c in ms.Items)
                    {
                        //調用 遍歷菜單 設置語言
                        AppLang(c, resources);
                    }
                }
            }

            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                AppLang(c, resources);
            }
        }
        /// <summary>
        /// 遍歷菜單
        /// </summary>
        /// <param name="item"></param>
        /// <param name="resources"></param>
        private static void AppLang(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem)
            {
                resources.ApplyResources(item, item.Name);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if (tsmi.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem c in tsmi.DropDownItems)
                    {
                        //if (tsmi != ToolStripSeparator)
                        //{ }
                        AppLang(c, resources);
                    }
                }
            }
        }
    }

    public static class formlan
    {
        /// <summary>
        /// 設置當前程序的界面語言
        /// </summary>
        /// <param name="lang">language:zh-CN, en-US</param>
        /// <param name="uc">窗體實例</param>
        public static void SetLang(string lang, Form form)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            if (form != null)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(form.GetType());
                resources.ApplyResources(form, "$this");
                AppLang(form, resources);
            }
        }

        /// <summary>
        /// 遍歷窗體所有控件，針對其設置當前界面語言
        /// </summary>
        /// <param name="control"></param>
        /// <param name="resources"></param>
        private static void AppLang(Control control, System.ComponentModel.ComponentResourceManager resources)
        {
            if (control is MenuStrip)
            {
                //將資源應用與對應的屬性
                resources.ApplyResources(control, control.Name);
                MenuStrip ms = (MenuStrip)control;
                if (ms.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem c in ms.Items)
                    {
                        //調用 遍歷菜單 設置語言
                        AppLang(c, resources);
                    }
                }
            }

            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                AppLang(c, resources);
            }
        }
        /// <summary>
        /// 遍歷菜單
        /// </summary>
        /// <param name="item"></param>
        /// <param name="resources"></param>
        private static void AppLang(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem)
            {
                resources.ApplyResources(item, item.Name);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if (tsmi.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem c in tsmi.DropDownItems)
                    {
                        //if (tsmi != ToolStripSeparator)
                        //{ }
                        AppLang(c, resources);
                    }
                }
            }
        }
    }

}
