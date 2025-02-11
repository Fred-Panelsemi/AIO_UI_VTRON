using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using AIO.Properties;

namespace AIO
{
    public partial class uc_scrConfigF : UserControl
    {
        //static Panel pnl = null;
        //static Bitmap bm;

        public struct typWH
        {
            public int Width;
            public int Height;
        }
        typWH r1;

        ContextMenuStrip fMenu = new ContextMenuStrip();
        ToolStripMenuItem show485 = new ToolStripMenuItem();
        ToolStripMenuItem in485LU = new ToolStripMenuItem();
        ToolStripMenuItem in485LD = new ToolStripMenuItem();
        ToolStripMenuItem in485RU = new ToolStripMenuItem();
        ToolStripMenuItem in485RD = new ToolStripMenuItem();
        ToolStripSeparator spFunc3 = new ToolStripSeparator();
        ToolStripMenuItem in485Rz = new ToolStripMenuItem();

        ToolStripSeparator spFunc1 = new ToolStripSeparator();
        ToolStripMenuItem showHDMI = new ToolStripMenuItem();
        ToolStripMenuItem inHdmiD = new ToolStripMenuItem();
        ToolStripMenuItem inHdmiL = new ToolStripMenuItem();
        ToolStripMenuItem inHdmiR = new ToolStripMenuItem();
        ToolStripMenuItem inHdmiU = new ToolStripMenuItem();
        ToolStripSeparator spFunc2 = new ToolStripSeparator();
        ToolStripMenuItem inHdmiF = new ToolStripMenuItem();
        ToolStripMenuItem inHdmiFz = new ToolStripMenuItem();
        /*
        ブラウズ
        閉じる
        次の
        閉じる 大画面設定
        選ぶ シリアルポート
        今使用中で
        大画面解像度
        桁
        行
        大画面構成する
        ロードする

        */
        //public static string svwiringstr;
        public static string sv485str;
        public static string svhdmistr;

        public uc_scrConfigF()
        {
            InitializeComponent();
            //pnl = pnl_box;
            //bm = new Bitmap(pnl.Width, pnl.Height);
        }

        private void p2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip = fMenu;
                in485Rz.Enabled = false;
                inHdmiF.Enabled = false;
                inHdmiFz.Enabled = false;

                //if (mvars.inHDMI == 6 && )

                if (mvars.boxCols == 2 && mvars.boxRows == 2)   
                {
                    if (mvars.inHDMI == 6) { inHdmiFz.Checked = false; inHdmiR.Checked = true; } //v0038 add
                    in485Rz.Enabled = true;
                    //v0037 disable
                    //if (mvars.in485 == 1 || mvars.in485 == 5) inHdmiF.Enabled = true;
                    //else inHdmiF.Enabled = false;   
                    //v0037 add
                    inHdmiF.Enabled = true;
                }
                else
                {
                    //v0037 從numUD_boxCols_ValueChanged與numUD_boxRows_ValueChanged搬過來的
                    if ((mvars.boxCols * mvars.boxRows > 4 && mvars.inHDMI == 5) || ((mvars.boxCols != 4 || mvars.boxRows != 4) && mvars.inHDMI == 6)) inHdmi_Click(inHdmiR, e); //v0038 add || ((mvars.boxCols != 4 || mvars.boxRows != 4) && mvars.inHDMI == 6)
                    if ((mvars.boxCols != 2 || mvars.boxRows != 2) && mvars.in485 == 5) in485_Click(in485RU, e);
                    if (mvars.boxCols == 4 && mvars.boxRows == 4) inHdmiFz.Enabled = true;  //v0038 add
                    //if (mvars.boxCols != 4 && mvars.boxRows != 4 && mvars.inHDMI == 6) inHdmi_Click(inHdmiR, e);
                }
            }
        }

        private void in485_Click(object sender, EventArgs e)
        {
            in485RU.Checked = false;
            in485RD.Checked = false;
            in485LU.Checked = false;
            in485LD.Checked = false;
            in485Rz.Checked = false;
            ToolStripMenuItem tsmnu = (ToolStripMenuItem)sender;
            tsmnu.Checked = true;
            mvars.in485 = Convert.ToByte(tsmnu.Tag.ToString());
            sv485str = tsmnu.Text;
            if (mvars.in485 == 5)
            {
                //v0038 dis
                //inHdmiD.Enabled = false;
                //inHdmiR.Enabled = false;
                //inHdmiU.Enabled = false;
                //inHdmiL.Enabled = false;
                //inHdmiF.Enabled = false;
                mvars.inHDMI = 5;
                inHdmi_Click(inHdmiF, e);
            }
            //v0036 Use and v0037 disable
            //else if (mvars.in485 == 1)
            //{
            //    inHdmiD.Enabled = true;
            //    inHdmiR.Enabled = true;
            //    inHdmiU.Enabled = true;
            //    inHdmiL.Enabled = true;
            //    inHdmiF.Enabled = true;
            //}
            //else
            //{
            //    inHdmiD.Enabled = true;
            //    inHdmiR.Enabled = true;
            //    inHdmiU.Enabled = true;
            //    inHdmiL.Enabled = true;
            //    inHdmiF.Enabled = false;
            //    if (mvars.inHDMI == 5) inHdmi_Click(inHdmiR, e);
            //}
            //v0037
            else
            {
                //v0038 disabled
                //inHdmiD.Enabled = true;
                //inHdmiR.Enabled = true;
                //inHdmiU.Enabled = true;
                //inHdmiL.Enabled = true;
                //inHdmiF.Enabled = true;   
            }
            //v0037 disabled
            //label3.Text = "485: " + mvars.in485 + "_" + sv485str;
            //label3.Text = "Microusb: " + mvars.in485 + "_" + sv485str;

            // v0037 add
            p2.Invalidate();
            button3_Click(null, null);
        }

        private void inHdmi_Click(object sender, EventArgs e)
        {
            inHdmiD.Checked = false;
            inHdmiR.Checked = false;
            inHdmiU.Checked = false;
            inHdmiL.Checked = false;
            inHdmiF.Checked = false;
            inHdmiFz.Checked = false;   //v0038 add
            ToolStripMenuItem tsmnu = (ToolStripMenuItem)sender;
            tsmnu.Checked = true;
            mvars.inHDMI = Convert.ToByte(tsmnu.Tag.ToString());
            //v0037
            //if (mvars.inHDMI == 5)
            //{
            //    in485RD.Enabled = true;
            //    in485RU.Enabled = false;
            //    in485LU.Enabled = false;
            //    in485LD.Enabled = false;
            //    in485Rz.Enabled = true;
            //}
            //else
            //{
                //v0038 disabled
                //in485RD.Enabled = true;
                //in485RU.Enabled = true;
                //in485LU.Enabled = true;
                //in485LD.Enabled = true;
                //in485Rz.Enabled = true; 
            //}
            svhdmistr = tsmnu.Text;

            //v0037 disable
            //label5.Text = "HDMI: " + mvars.inHDMI + "_" + svhdmistr;
            // v0037 add
            p2.Invalidate();
            button3_Click(null, null);

        }

        private void uc_scrConfigF_Load(object sender, EventArgs e)
        {
            cmb_nvCommList.Items.Clear();
            if (mvars.demoMode) cmb_nvCommList.Items.Add("COM D");
            else { for (byte i = 0; i < mvars.Comm.Length; i++) cmb_nvCommList.Items.Add(mvars.Comm[i]); }
            if (cmb_nvCommList.Items.Count > 0)
                cmb_nvCommList.Text = cmb_nvCommList.Items[0].ToString();

            numUD_boxCols.Value = mvars.boxCols;
            numUD_boxRows.Value = mvars.boxRows;


            #region 建立滑鼠右鍵功能表
            //v0037 modify label4 delete
            //if (MultiLanguage.DefaultLanguage == "en-US") svwiringstr = "Back wiring hint";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT") svwiringstr = "背面走線示意";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN") svwiringstr = "背面走线示意";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP") svwiringstr = "後ろ配線表示";
            //if (MultiLanguage.DefaultLanguage == "en-US") svwiringstr = "Front view(wiring hint)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT") svwiringstr = "正面圖示(起始提示)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN") svwiringstr = "正面图示(起始提示)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP") svwiringstr = "正面図(配線の注記)";

            this.ContextMenuStrip = fMenu;

            fMenu.Items.Add(show485);
            show485.Font = new Font(ContextMenuStrip.Font.FontFamily, 12);
            if (MultiLanguage.DefaultLanguage == "en-US") show485.Text = "Microusb rear view wiring hint";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") show485.Text = "Microusb 背面走線示意";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") show485.Text = "Microusb 背面走线示意";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") show485.Text = "Microusb 後ろ配線表示";
            show485.Tag = "show485";

            show485.DropDownItems.Add(in485RD);
            Image img = Resources.RDn;
            if (MultiLanguage.DefaultLanguage == "en-US") in485RD.Text = "Connect to PC from the lower right corner screen";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") in485RD.Text = "從右下角落屏連接PC";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") in485RD.Text = "从右下角落屏连接PC";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") in485RD.Text = "Connect to PC from the lower right corner screen";
            in485RD.Tag = "1";
            in485RD.CheckOnClick = true;
            in485RD.Image = img;
            in485RD.Click += new EventHandler(in485_Click);
            if (mvars.in485 == 1) { in485RD.Checked = true; sv485str = in485RD.Text; }

            show485.DropDownItems.Add(in485RU);
            img = Resources.RUn;
            if (MultiLanguage.DefaultLanguage == "en-US") in485RU.Text = "Connect to PC from the upper right corner screen";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") in485RU.Text = "從右上角落屏連接PC";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") in485RU.Text = "从右上角落屏连接PC";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") in485RU.Text = "Connect to PC from the upper right corner screen";
            in485RU.Tag = "2";
            in485RU.CheckOnClick = true;
            in485RU.Image = img;
            in485RU.Click += new EventHandler(in485_Click);
            if (mvars.in485 == 2) { in485RU.Checked = true; sv485str = in485RU.Text; }

            show485.DropDownItems.Add(in485LU);
            img = Resources.LUn;
            if (MultiLanguage.DefaultLanguage == "en-US") in485LU.Text = "Connect to PC from the upper left corner screen";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") in485LU.Text = "從左上角落屏連接PC";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") in485LU.Text = "从左上角落屏连接PC";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") in485LU.Text = "Connect to PC from the upper left corner screen";
            in485LU.Tag = "3";
            in485LU.CheckOnClick = true;
            in485LU.Image = img;
            in485LU.Click += new EventHandler(in485_Click);
            if (mvars.in485 == 3) { in485LU.Checked = true; sv485str = in485LU.Text; }

            show485.DropDownItems.Add(in485LD);
            img = Resources.LDn;
            if (MultiLanguage.DefaultLanguage == "en-US") in485LD.Text = "Connect to PC from the lower left corner screen";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") in485LD.Text = "從左下角落屏連接PC";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") in485LD.Text = "从左下角落屏连接PC";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") in485LD.Text = "Connect to PC from the lower left corner screen";
            in485LD.Tag = "4";
            in485LD.CheckOnClick = true;
            in485LD.Image = img;
            in485LD.Click += new EventHandler(in485_Click);
            if (mvars.in485 == 4) { in485LD.Checked = true; sv485str = in485LD.Text; }

            show485.DropDownItems.Add(spFunc3);
            show485.DropDownItems.Add(in485Rz);
            img = Resources.fhdz;
            if (MultiLanguage.DefaultLanguage == "en-US") in485Rz.Text = "Special connection 1 upper right screen import";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") in485Rz.Text = "特殊串接1右上屏導入";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") in485Rz.Text = "特殊串接1右上屏导入";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") in485Rz.Text = "Special connection 1 upper right screen import";
            in485Rz.Tag = "5";
            in485Rz.CheckOnClick = true;
            in485Rz.Image = img;
            in485Rz.Click += new EventHandler(in485_Click);
            if (mvars.in485 == 5) { in485Rz.Checked = true; sv485str = in485Rz.Text; }
            in485Rz.Enabled = false;


            fMenu.Items.Add(spFunc1);



            fMenu.Items.Add(showHDMI);
            showHDMI.Font = new Font(ContextMenuStrip.Font.FontFamily, 12);
            if (MultiLanguage.DefaultLanguage == "en-US") showHDMI.Text = "HDMI rear view wiring hint";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") showHDMI.Text = "HDMI 背面走線示意";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") showHDMI.Text = "HDMI 背面走线示意";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") showHDMI.Text = "HDMI 後ろ配線表示";
            showHDMI.Tag = "showHDMI";
            //showHDMI.CheckOnClick = true;
            //showHDMI.Click += new EventHandler(showHDMI_Click);
            //showHDMI.Checked = true;

            showHDMI.DropDownItems.Add(inHdmiD);
            img = Resources.D2U;
            inHdmiD.Image = img;
            if (MultiLanguage.DefaultLanguage == "en-US") inHdmiD.Text = "Vertical connection on the lower side";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") inHdmiD.Text = "縱向串接下側導入";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") inHdmiD.Text = "纵向串接下侧导入";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") inHdmiD.Text = "Vertical connection on the lower side";
            inHdmiD.Tag = "1";
            inHdmiD.CheckOnClick = true;
            inHdmiD.Click += new EventHandler(inHdmi_Click);
            if (mvars.inHDMI == 1) { inHdmiD.Checked = true; svhdmistr = inHdmiD.Text; }

            showHDMI.DropDownItems.Add(inHdmiR);
            img = Resources.R2L;
            inHdmiR.Image = img;
            if (MultiLanguage.DefaultLanguage == "en-US") inHdmiR.Text = "Horizontal connection of right imports";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") inHdmiR.Text = "橫向串接右側導入";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") inHdmiR.Text = "横向串接右侧导入";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") inHdmiR.Text = "Horizontal connection of right imports";
            inHdmiR.Tag = "2";
            inHdmiR.CheckOnClick = true;
            inHdmiR.Click += new EventHandler(inHdmi_Click);
            if (mvars.inHDMI == 2) { inHdmiR.Checked = true; svhdmistr = inHdmiR.Text; }

            showHDMI.DropDownItems.Add(inHdmiU);
            img = Resources.U2D;
            inHdmiU.Image = img;
            if (MultiLanguage.DefaultLanguage == "en-US") inHdmiU.Text = "Vertical connection on the upper side";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") inHdmiU.Text = "縱向串接上側導入";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") inHdmiU.Text = "纵向串接上侧导入";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") inHdmiU.Text = "Vertical connection on the upper side";
            inHdmiU.Tag = "3";
            inHdmiU.CheckOnClick = true;
            inHdmiU.Click += new EventHandler(inHdmi_Click);
            if (mvars.inHDMI == 3) { inHdmiU.Checked = true; svhdmistr = inHdmiU.Text; }

            showHDMI.DropDownItems.Add(inHdmiL);
            img = Resources.L2R;
            inHdmiL.Image = img;
            if (MultiLanguage.DefaultLanguage == "en-US") inHdmiL.Text = "Horizontal connection of left imports";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") inHdmiL.Text = "橫向串接左側導入";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") inHdmiL.Text = "横向串接左侧导入";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") inHdmiL.Text = "Horizontal connection of left imports";
            inHdmiL.Tag = "4";
            inHdmiL.CheckOnClick = true;
            inHdmiL.Click += new EventHandler(inHdmi_Click);
            if (mvars.inHDMI == 4) { inHdmiL.Checked = true; svhdmistr = inHdmiL.Text; }

            showHDMI.DropDownItems.Add(spFunc2);

            showHDMI.DropDownItems.Add(inHdmiF);
            img = Resources.fhd;
            inHdmiF.Image = img;
            //v0037
            if (MultiLanguage.DefaultLanguage == "en-US") inHdmiF.Text = "FHD 1920x1080";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") inHdmiF.Text = "FHD 1920x1080";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") inHdmiF.Text = "FHD 1920x1080";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") inHdmiF.Text = "FHD 1920x1080";
            inHdmiF.Tag = "5";
            inHdmiF.CheckOnClick = true;
            inHdmiF.Click += new EventHandler(inHdmi_Click);
            if (mvars.inHDMI == 5) { inHdmiF.Checked = true; svhdmistr = inHdmiF.Text; }
            inHdmiF.Enabled = false;

            //v0038 add
            showHDMI.DropDownItems.Add(inHdmiFz);
            img = Resources.fhdz;
            inHdmiFz.Image = img;
            if (MultiLanguage.DefaultLanguage == "en-US") inHdmiFz.Text = "FHDz 1920x1080";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") inHdmiFz.Text = "FHDz 1920x1080";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") inHdmiFz.Text = "FHDz 1920x1080";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") inHdmiFz.Text = "FHDz 1920x1080";
            inHdmiFz.Tag = "6";
            inHdmiFz.CheckOnClick = true;
            inHdmiFz.Click += new EventHandler(inHdmi_Click);
            if (mvars.inHDMI == 6) { inHdmiFz.Checked = true; svhdmistr = inHdmiFz.Text; }
            inHdmiFz.Enabled = false;



            //label4.Text = svwiringstr;

            //v0037 modify
            //label3.Text = "485: " + mvars.in485 + "_" + sv485str;
            //label5.Text = "HDMI: " + mvars.inHDMI + "_" + svhdmistr;
            if (MultiLanguage.DefaultLanguage == "en-US") { label3.Text = "Microusb start"; label5.Text = "HDMI x 1 start"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { label3.Text = "Microusb 起始"; label5.Text = "HDMI x 1 起始"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { label3.Text = "Microusb 起始"; label5.Text = "HDMI x 1 起始"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { label3.Text = "Microusb start"; label5.Text = "HDMI x 1 start"; }

            #endregion 建立滑鼠右鍵功能表

            if (MultiLanguage.DefaultLanguage == "en-US" || MultiLanguage.DefaultLanguage == "ja-JP")
                lbl_resolution.Left = label2.Left + label2.Width + 8;
            mvars.actFunc = "scrF";
            mvars.FormShow[20] = true;


            groupBox2.Visible = false;
            button3_Click(sender, e);

            //0037 add
            if (mvars.in485 == 5)
            {
                inHdmiD.Enabled = false;
                inHdmiR.Enabled = false;
                inHdmiU.Enabled = false;
                inHdmiL.Enabled = false;
            }
            groupBox2.Visible = true;
        }

        private void groupBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics gra1 = groupBox2.CreateGraphics();
            //gra1.FillEllipse(new SolidBrush(Color.FromArgb(255, 127, 127)), label3.Left + label3.Width, label3.Top, 15, 15);
            //gra1.FillEllipse(new SolidBrush(Color.FromArgb(127, 127, 255)), label5.Left + label5.Width, label5.Top, 15, 15);

            //v0037 modify
            gra1.DrawEllipse(new Pen(Color.FromArgb(255, 127, 127), 5), label3.Left + label3.Width + 3, label3.Top + 2, 11, 11);
            gra1.DrawEllipse(new Pen(Color.FromArgb(127, 127, 255), 5), label5.Left + label5.Width + 3, label5.Top + 2, 11, 11);
            
            
            gra1.Dispose();
            groupBox2.Paint -= new PaintEventHandler(groupBox2_Paint);
        }


        private void numUD_boxCols_ValueChanged(object sender, EventArgs e)
        {
            mvars.boxCols = (byte)numUD_boxCols.Value;
            //v0037 應該要等按下 p2_MouseUp或是Next 再去判斷
            //if (mvars.boxCols != 2 && mvars.inHDMI == 5) inHdmi_Click(inHdmiR, e);
            //if (mvars.boxCols != 2 && mvars.in485 == 5) in485_Click(in485RD, e);
            p2.Invalidate();
            button3_Click(null, null);

            //0037 add
            mp.doDelayms(50);
            string svs = lbl_amount.Text.Split('x')[1].Trim();
            if (MultiLanguage.DefaultLanguage == "en-US") { label3.Text = "Microusb start"; label5.Text = "HDMI x " + svs + " start"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { label3.Text = "Microusb 起始"; label5.Text = "HDMI x " + svs + " 起始"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { label3.Text = "Microusb 起始"; label5.Text = "HDMI x " + svs + " 起始"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { label3.Text = "Microusb start"; label5.Text = "HDMI x " + svs + " start"; }
            groupBox2.Invalidate();
            groupBox2.Paint += new PaintEventHandler(groupBox2_Paint);
        }
        private void numUD_boxRows_ValueChanged(object sender, EventArgs e)
        {
            mvars.boxRows = (byte)numUD_boxRows.Value;
            //v0037 應該要等按下 p2_MouseUp或是Next 再去判斷
            //if (mvars.boxCols != 2 && mvars.inHDMI == 5) inHdmi_Click(inHdmiR, e);
            //if (mvars.boxCols != 2 && mvars.in485 == 5) in485_Click(in485RD, e);
            p2.Invalidate();
            button3_Click(null, null);

            //0037 add
            mp.doDelayms(50);
            string svs = lbl_amount.Text.Split('x')[1].Trim();
            if (MultiLanguage.DefaultLanguage == "en-US") { label3.Text = "Microusb start"; label5.Text = "HDMI x " + svs + " start"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { label3.Text = "Microusb 起始"; label5.Text = "HDMI x " + svs + " 起始"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { label3.Text = "Microusb 起始"; label5.Text = "HDMI x " + svs + " 起始"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { label3.Text = "Microusb start"; label5.Text = "HDMI x " + svs + " start"; }
            groupBox2.Invalidate();
            groupBox2.Paint += new PaintEventHandler(groupBox2_Paint);
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            btn_next.Enabled = false;
            btn_close.Enabled = false;

            //v0037 add
            //if (mvars.boxCols * mvars.boxRows > 4 && mvars.inHDMI == 5) inHdmi_Click(inHdmiR, e);
            if ((mvars.boxCols * mvars.boxRows > 4 && mvars.inHDMI == 5) || ((mvars.boxCols != 4 || mvars.boxRows != 4) && mvars.inHDMI == 6)) inHdmi_Click(inHdmiR, e); //v0038
            if ((mvars.boxCols != 2 || mvars.boxRows != 2) && mvars.in485 == 5) in485_Click(in485RU, e);

            mvars.flgSelf = true;
            if (mvars.demoMode == false && Form1.chkformsize.Checked && Form1.tslblStatus.Text.Substring(0, "MCU".Length) != "MCU") { btn_next.Enabled = true; btn_close.Enabled = true; return; }
            
            mvars.actFunc = "";
            btn_next.Enabled = true;
            btn_close.Enabled = true;
            mvars.FormShow[20] = false; 
            this.Dispose();

            Form1._menuStrip1.Visible = true;
            Form1.hUser.Enabled = !Form1.hUser.Enabled;
            Form1.hLan.Enabled = !Form1.hLan.Enabled;
            Form1.hExit.Text = "離開 大屏拼接(&E)";
            Form1.hPictureadjust.Visible = false;
            Form1.hScreenconfig.Visible = true;
            Form1.hsend.Visible = true;
            Form1.hsave.Visible = true;
            Form1.hsingle.Visible = true;
            Form1.tsspruser.Visible = true;
            Form1.tsmnuuser.Visible = true;
            Form1.hEDIDud.Visible = mvars.flgsuperuser;
            #region 多語系高級切換
            string svs2 = "";
            //if (MultiLanguage.DefaultLanguage == "en-US")
            //    svs2 = "Exit ScreenConfig (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            //    svs2 = "離開 大平拼接 (&E)";
            //else if (MultiLanguage.DefaultLanguage == "zh-CN")
            //    svs2 = "离开 大屏拼接 (&E)";
            //else if (MultiLanguage.DefaultLanguage == "ja-JP")
            //    svs2 = "閉じる ScreenConfig (&E)";
            //Form1.hExit.Text = svs2;

            ComponentResourceManager resource = new ComponentResourceManager(typeof(Form1));
            string exatg = resource.GetString("exscreenconfig");
            if (exatg != "" && exatg != null) Form1.hExit.Text = exatg;
            #endregion 多語系高級切換
            Form1.ActiveForm.Size = new Size(1080, 670);
            Form1.ActiveForm.Location = new Point(100, 100);
            Form1.pnlfrm1.BringToFront();
            Form1.ucbox = new uc_box
            {
                Parent = Form1.pnlfrm1,
                Dock = DockStyle.Fill
            };
            //Form1.ActiveForm.ControlBox = false;
            //Form1.ActiveForm.FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Form1._menuStrip1.Visible = true;
            mvars.FormShow[20] = false;
            Form1.ucscrF.Dispose();
            mvars.actFunc = "";
            Form1.ActiveForm.Size = new Size(Convert.ToInt16(Form1.chkformsize.Text.Split(',')[0]), Convert.ToInt16(Form1.chkformsize.Text.Split(',')[1]));
        }

        private void dgv_box_CellClick(object sender, DataGridViewCellEventArgs e) { dgv_box.ClearSelection(); }

        private void dgv_box_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e) { dgv_box.ClearSelection(); }

        private void cmb_nvCommList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.demoMode) { btn_next.Enabled = true; }
            else
            {
                if (cmb_nvCommList.Text != "") { btn_next.Enabled = true; }
                else { btn_next.Enabled = false; }
            }
        }

        private void dgv_box_SelectionChanged(object sender, EventArgs e)
        {
            dgv_box.ClearSelection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            p2.Width = 300;
            p2.Height = (int)(p2.Width * 0.72);
            if (p2.Height == 215) p2.Height = 216;
            r1.Height = p2.Height / mvars.boxRows;
            r1.Width = p2.Width / mvars.boxCols;
            if (r1.Height <= 32) r1.Height = 32;
            if (r1.Height * 1.39 != r1.Width)
            {
                r1.Width = (int)(r1.Height * 1.39);
                p2.Width = r1.Width * mvars.boxCols;
                p2.Height = r1.Height * mvars.boxRows;
            }
            if (p2.Width > 300)
            {
                p2.Width = 300;// 224;
                r1.Width = p2.Width / mvars.boxCols;
                r1.Height = (int)(r1.Width * 0.72);
                p2.Height = r1.Height * mvars.boxRows;
                if (p2.Height == 215) p2.Height = 216;
            }
            if (p2.Height > 216)
            {
                p2.Height = 216;
                r1.Height = p2.Height / mvars.boxRows;
                r1.Width = (int)(r1.Height * 1.39);
            }

            //v0037 disabled
            //if (MultiLanguage.prelan == "en-US") lbl_amount.Text = (mvars.boxCols * mvars.boxRows).ToString() + " single screen combinations,HDMI x " + (((int)numUD_boxCols.Value * (int)numUD_boxRows.Value - 1) / 4 + 1);
            //else if (MultiLanguage.prelan == "zh-CN") lbl_amount.Text = (mvars.boxCols * mvars.boxRows).ToString() + " 个单屏组合,HDMI x " + (((int)numUD_boxCols.Value * (int)numUD_boxRows.Value - 1) / 4 + 1);
            //else if (MultiLanguage.prelan == "zh-CHT") lbl_amount.Text = (mvars.boxCols * mvars.boxRows).ToString() + " 個單屏組合,HDMI x " + (((int)numUD_boxCols.Value * (int)numUD_boxRows.Value - 1) / 4 + 1);
            //else if (MultiLanguage.prelan == "ja-JP") lbl_amount.Text = (mvars.boxCols * mvars.boxRows).ToString() + " の画面組み合わせ,HDMI x " + (((int)numUD_boxCols.Value * (int)numUD_boxRows.Value - 1) / 4 + 1);
            //lbl_resolution.Text = mvars.boxCols * 960 + " x " + mvars.boxRows * 540;

            p2.Paint += new PaintEventHandler(p2_Paint);
        }

        private void p2_Paint(object sender, PaintEventArgs e)
        {
            updatep2(p2, e.Graphics);
        }

        void updatep2(Panel panel, Graphics gra1)
        {
            typWH r1_1;
            r1_1 = r1;

            int svpL = (int)(r1_1.Height * 0.2);
            if (svpL > 15) svpL = 15;
            Pen pL = new Pen(Color.FromArgb(64, 64, 64), svpL);
            r1_1.Width -= 2;
            r1_1.Height -= 2;

            for (int SvC = 0; SvC < numUD_boxCols.Value; SvC++)
            {
                for (int SvR = 0; SvR < numUD_boxRows.Value; SvR++)
                {
                    int svx = SvC * r1_1.Width + 2;
                    int svx2 = SvC * r1_1.Width + r1_1.Width;
                    int svy = SvR * r1_1.Height + 2;
                    int svw = r1_1.Width - 2;
                    int svh = r1_1.Height - 2;
                    gra1.DrawRectangle(new Pen(Color.FromArgb(64, 64, 64), 1), svx, svy, svw, svh);
                    gra1.DrawLine(pL, svx, svy + (int)(svpL / 2), svx2, svy + (int)(svpL / 2));
                    Image img = Resources.panelsemi;
                    // Fred 2025/01/22 威創客製需求更改 >> Panel Semi Logo 移除 
                    //Rectangle compress = new Rectangle(svx + (int)(30 * ((float)svw / 160)), svy + (int)(35 * ((float)svh / 80)), (int)(img.Width * ((float)svw / 160)), (int)(img.Height * ((float)svh / 110)));
                    //gra1.DrawImage(img, compress);
                    img.Dispose();
                }
            }

            /// v0037 add
            byte svHdmis = 1;
            if (mvars.boxCols * mvars.boxRows > 1)
            {
                label6.Text = r1_1.Width + "," + r1_1.Height;
                int c1 = 20; c1 = 16; c1 = Convert.ToInt16(r1_1.Height * 0.15);
                int lf = 10;
                int tp = r1_1.Height / 3;
                lf = Convert.ToInt16(r1_1.Width * 0.065);
                tp = Convert.ToInt16(-r1_1.Height * 0.22);
                if (r1_1.Height < c1 * 2) c1 = Convert.ToInt16(r1_1.Height * 0.45);
                if (r1_1.Width < 45) lf = Convert.ToInt16((r1_1.Width - c1 * 2) / 2);
                if (mvars.in485 == 1)
                {
                    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 255, 0, 0)), lf, tp + r1_1.Height * (mvars.boxRows - 1), c1, c1);
                    //gra1.DrawEllipse(new Pen(Color.FromArgb(128, 255, 0, 0), 5), lf, tp + r1_1.Height / 2 * mvars.boxRows, c1, c1);
                    gra1.DrawEllipse(new Pen(Color.FromArgb(128, 255, 0, 0), 5), lf, tp + r1_1.Height * mvars.boxRows, c1, c1);     //v0037

                }
                else if (mvars.in485 == 2 || mvars.in485 == 5)
                {
                    //圓點記號
                    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 255, 0, 0)), lf, tp, c1, c1);
                    //gra1.DrawEllipse(new Pen(Color.FromArgb(128, 255, 0, 0), 5), lf, tp, c1, c1);
                    gra1.DrawEllipse(new Pen(Color.FromArgb(128, 255, 0, 0), 5), lf, tp + r1_1.Height, c1, c1);     //v0037

                    //箭頭記號
                    //gra1.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 0), 5), 50, 70, 50, 90);
                    //gra1.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 0), 5), 50, 90, 40, 80);
                    //gra1.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 0), 5), 50, 90, 60, 80);
                }
                else if (mvars.in485 == 3)
                {
                    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 255, 0, 0)), lf + r1_1.Width * (mvars.boxCols - 1), tp, c1, c1);
                    gra1.DrawEllipse(new Pen(Color.FromArgb(128, 255, 0, 0), 5), lf + r1_1.Width * (mvars.boxCols - 1), tp + r1_1.Height, c1, c1);
                }
                else if (mvars.in485 == 4)
                {
                    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 255, 0, 0)), lf + r1_1.Width * (mvars.boxCols - 1), tp + r1_1.Height * (mvars.boxRows - 1), c1, c1);
                    gra1.DrawEllipse(new Pen(Color.FromArgb(128, 255, 0, 0), 5), lf + r1_1.Width * (mvars.boxCols - 1), tp + r1_1.Height * mvars.boxRows, c1, c1);
                }

                lf += Convert.ToInt16(c1 * 1.5);
                if (mvars.inHDMI == 1)
                {
                    byte svi = 0;
                    svHdmis = Convert.ToByte(numUD_boxCols.Value);
                    for (int SvR = Convert.ToInt16(numUD_boxRows.Value); SvR >= 1; SvR--)
                    {
                        if (svi % 4 == 0)
                        {
                            for (int SvC = 0; SvC < numUD_boxCols.Value; SvC++)
                            {
                                //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), lf + Convert.ToInt16(c1 * 1.1) + SvC * r1_1.Width, tp + r1_1.Height * (SvR - 1), c1, c1);
                                gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf + SvC * r1_1.Width, tp + r1_1.Height * SvR, c1, c1);
                            }
                        }
                        svi++;
                        if (svi > 3) { svi = 0; svHdmis *= 2; }
                    }
                }
                else if (mvars.inHDMI == 5)
                {
                    gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf, tp + r1_1.Height, c1, c1);
                }
                else if (mvars.inHDMI == 2)
                {
                    //if (c1 != 20)
                    //{
                    //    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), c1 / 2, c1 / 2, c1, c1);
                    //    gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf, tp + r1_1.Height, c1, c1);
                    //}
                    //else
                    //{
                    //    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), r1_1.Width / 5, r1_1.Height / 5, c1, c1);
                    //    gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), r1_1.Width / 5, r1_1.Height / 5, c1, c1);
                    //}
                    //v0037 modify
                    byte svi = 0;
                    svHdmis = Convert.ToByte(numUD_boxCols.Value);
                    //for (int SvR = Convert.ToInt16(numUD_boxRows.Value); SvR >= 1; SvR--)
                    for (int SvC = 0; SvC < numUD_boxCols.Value; SvC++)
                    {
                        if (svi % 4 == 0)
                        {
                            for (int SvR = 1; SvR <= numUD_boxRows.Value; SvR++)
                            {
                                //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), lf + Convert.ToInt16(c1 * 1.1) + SvC * r1_1.Width, tp + r1_1.Height * (SvR - 1), c1, c1);
                                gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf + SvC * r1_1.Width, tp + r1_1.Height * SvR, c1, c1);
                            }
                        }
                        svi++;
                        if (svi > 3) { svi = 0; svHdmis *= 2; }
                    }
                }
                else if (mvars.inHDMI == 3)
                {
                    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), (p2.Width - 50) + 10 / mvars.boxCols, 30 / mvars.boxRows, c1, c1);
                    byte svi = 0;
                    svHdmis = Convert.ToByte(numUD_boxCols.Value);
                    for (int SvR = 1; SvR <= numUD_boxRows.Value; SvR++)
                    {
                        if (svi % 4 == 0)
                        {
                            for (int SvC = 0; SvC < numUD_boxCols.Value; SvC++)
                            {
                                //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), lf + Convert.ToInt16(c1 * 1.1) + SvC * r1_1.Width, tp + r1_1.Height * (SvR - 1), c1, c1);
                                gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf + SvC * r1_1.Width, tp + r1_1.Height * SvR, c1, c1);
                            }
                        }
                        svi++;
                        if (svi > 3) { svi = 0; svHdmis *= 2; }
                    }
                }
                else if (mvars.inHDMI == 4)
                {
                    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), (p2.Width - 50) + 10 / mvars.boxCols, (p2.Height - 50) + 10 / mvars.boxRows, c1, c1);
                    //v0037 modify
                    byte svi = 0;
                    svHdmis = Convert.ToByte(numUD_boxCols.Value);
                    //for (int SvR = Convert.ToInt16(numUD_boxRows.Value); SvR >= 1; SvR--)
                    for (int SvC = Convert.ToInt16(numUD_boxCols.Value - 1); SvC >= 0; SvC--)
                    {
                        if (svi % 4 == 0)
                        {
                            for (int SvR = 1; SvR <= numUD_boxRows.Value; SvR++)
                            {
                                //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), lf + Convert.ToInt16(c1 * 1.1) + SvC * r1_1.Width, tp + r1_1.Height * (SvR - 1), c1, c1);
                                gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf + SvC * r1_1.Width, tp + r1_1.Height * SvR, c1, c1);
                            }
                        }
                        svi++;
                        if (svi > 3) { svi = 0; svHdmis *= 2; }
                    }
                }
                else if (mvars.inHDMI == 6)
                {
                    //FHDz
                    if (mvars.boxCols == 4 && mvars.boxRows == 4)
                    {
                        svHdmis = 0;
                        for (int SvR = 0; SvR < numUD_boxRows.Value; SvR++)
                        {
                            if (SvR % 2 == 0)
                            {
                                for (int SvC = 0; SvC < numUD_boxCols.Value; SvC++)
                                {
                                    if (SvC % 2 == 0)
                                    {
                                        if (mvars.boxCols == 4 && mvars.boxRows == 4)
                                        {
                                            gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf + SvC * r1_1.Width, tp + r1_1.Height * (SvR + 1), c1, c1);
                                            gra1.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 255), 5), c1 + lf + SvC * r1_1.Width, (tp / 2) + r1_1.Height * (SvR + 1), lf + (SvC + 1) * r1_1.Width, (tp / 2) + r1_1.Height * (SvR + 1));
                                            gra1.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf + (SvC + 1) * r1_1.Width, (tp / 2) + r1_1.Height * (SvR + 1), lf + SvC * r1_1.Width, tp + r1_1.Height * (SvR + 2));
                                            gra1.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf + SvC * r1_1.Width, tp + r1_1.Height * (SvR + 2), lf + (SvC + 1) * r1_1.Width, tp + r1_1.Height * (SvR + 2));
                                            gra1.DrawRectangle(new Pen(Color.FromArgb(128, 20, 20, 20), 8), lf + (SvC + 1) * r1_1.Width, tp + r1_1.Height * (SvR + 2) - (c1 / 2), c1, c1);
                                            //gra1.FillRectangle(new SolidBrush(Color.FromArgb(128,20, 20, 20)), lf + (SvC + 1) * r1_1.Width, tp + r1_1.Height * (SvR + 2) - (c1 / 2), c1, c1);
                                        }                                     
                                        svHdmis++;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        byte svi = 0;
                        svHdmis = Convert.ToByte(numUD_boxCols.Value);
                        //for (int SvR = Convert.ToInt16(numUD_boxRows.Value); SvR >= 1; SvR--)
                        for (int SvC = 0; SvC < numUD_boxCols.Value; SvC++)
                        {
                            if (svi % 4 == 0)
                            {
                                for (int SvR = 1; SvR <= numUD_boxRows.Value; SvR++)
                                {
                                    //gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 0, 0, 255)), lf + Convert.ToInt16(c1 * 1.1) + SvC * r1_1.Width, tp + r1_1.Height * (SvR - 1), c1, c1);
                                    gra1.DrawEllipse(new Pen(Color.FromArgb(128, 0, 0, 255), 5), lf + SvC * r1_1.Width, tp + r1_1.Height * SvR, c1, c1);
                                }
                            }
                            svi++;
                            if (svi > 3) { svi = 0; svHdmis *= 2; }
                        }
                    }
                }
            }

            if (MultiLanguage.prelan == "en-US") lbl_amount.Text = (mvars.boxCols * mvars.boxRows).ToString() + " single screen combinations,HDMI x " + svHdmis;
            else if (MultiLanguage.prelan == "zh-CN") lbl_amount.Text = (mvars.boxCols * mvars.boxRows).ToString() + " 个单屏组合,HDMI x " + svHdmis;
            else if (MultiLanguage.prelan == "zh-CHT") lbl_amount.Text = (mvars.boxCols * mvars.boxRows).ToString() + " 個單屏組合,HDMI x " + svHdmis;
            else if (MultiLanguage.prelan == "ja-JP") lbl_amount.Text = (mvars.boxCols * mvars.boxRows).ToString() + " の画面組み合わせ,HDMI x " + svHdmis;
            lbl_resolution.Text = mvars.boxCols * 960 + " x " + mvars.boxRows * 540;


            if (MultiLanguage.DefaultLanguage == "en-US") { label3.Text = "Microusb start"; label5.Text = "HDMI x " + svHdmis + " start"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { label3.Text = "Microusb 起始"; label5.Text = "HDMI x " + svHdmis + " 起始"; }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { label3.Text = "Microusb 起始"; label5.Text = "HDMI x " + svHdmis + " 起始"; }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { label3.Text = "Microusb start"; label5.Text = "HDMI x " + svHdmis + " start"; }
            gra1 = groupBox2.CreateGraphics();
            //gra1.FillEllipse(new SolidBrush(Color.FromArgb(255, 127, 127)), label3.Left + label3.Width, label3.Top, 13, 13);
            //gra1.FillEllipse(new SolidBrush(Color.FromArgb(127, 127, 255)), label5.Left + label5.Width, label5.Top, 13, 13);

            //v0037 modify
            gra1.DrawEllipse(new Pen(Color.FromArgb(255, 127, 127), 5), label3.Left + label3.Width+3, label3.Top+2, 11, 11);
            gra1.DrawEllipse(new Pen(Color.FromArgb(127, 127, 255), 5), label5.Left + label5.Width+3, label5.Top+2, 11, 11);


            gra1.Dispose();
            pL.Dispose();
            p2.Paint -= new PaintEventHandler(p2_Paint);
        }

        
    }
}
