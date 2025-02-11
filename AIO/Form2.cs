using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Diagnostics;


namespace AIO
{
    public partial class Form2 : Form
    {
        static Button[] btn = new Button[99];
        static Label[] lblShowFull = new Label[99];
        public static CheckBox chkshowextend = null;
        public static ToolStripLabel tslblstatus = null;
        public static ToolStripLabel tslblpathboxid = null;
        public static Label lblexcorner = null;
        public static Label lbledid = null;
        public static Label lbledidhead = null;
        Label lblMark;

        Point mouse_offset;
        public static i3_Pat i3pat = null;
        public static byte pvampMaxcount = 0;
        public static byte pvDir = 0;   //0上 1下 2左 3右
        public static string[] patsel = { " FULL", " CheckBox", " AutoGamma", " BMP", " GrayScale", " Coding", " RT" };

        public static string pathBoxID = "";
        public static string[] dataBoxID = null;

        public static int[] priRes = { 1366, 768 };

        public static int bxw = 480;
        public static int bxh = 540;

        public static PictureBox picfront = null;
        public static PictureBox[,] svpbf;

        public Form2()
        {
            InitializeComponent();
            //this.Size = new Size(630, 400);

            //搭配設置 public static CheckBox chk_showextend = new CheckBox();
            //chk_showextend.SetBounds(487, 50, 15, 14);
            //chk_showextend.Font = new Font("Arial", 8);
            //this.Controls.AddRange(new Control[] { chk_showextend });
            //chk_showextend.Click += chk_ShowPattern_Click;
            //解除設置 chk_showextend.Click -= chk_ShowPattern_Click;
            //解除設置 if (this.Controls.Contains(chk_showextend)) { this.Controls.Remove(chk_showextend); }

        }

        private void callOnClick(Button btn)
        {
            //建立一個型別  
            Type t = typeof(Button);
            //引數物件  
            object[] p = new object[1];
            //產生方法  
            MethodInfo m = t.GetMethod("OnClick", BindingFlags.NonPublic | BindingFlags.Instance);
            //引數賦值。傳入函式  
            p[0] = EventArgs.Empty;
            //呼叫  
            m.Invoke(btn, p);
            return;

            /*
            //呼叫例子。  
            //呼叫Button1的onclick  
            callOnClick(Button1);

            //呼叫Button5的onclick  
            callOnClick(Button5);
            */

        }


        private void Form2_Load(object sender, EventArgs e)
        {
            int svi = 20;
            int svt = pic_Front.Top + pic_Front.Height + 7;
            lblShowFull[0] = label1; label1.Location = new Point(svi += 15, svt);
            lblShowFull[1] = label2; label2.Location = new Point(svi += 15, svt);
            lblShowFull[2] = label3; label3.Location = new Point(svi += 15, svt);
            lblShowFull[3] = label4; label4.Location = new Point(svi += 15, svt);
            lblShowFull[4] = label5; label5.Location = new Point(svi += 15, svt);
            lblShowFull[5] = label6; label6.Location = new Point(svi += 15, svt);
            lblShowFull[6] = label7; label7.Location = new Point(svi += 15, svt);
            lblShowFull[7] = label8; label8.Location = new Point(svi += 15, svt);
            lblShowFull[8] = label9; label9.Location = new Point(svi += 15, svt);
            lblShowFull[9] = label10; label10.Location = new Point(svi += 15, svt);
            lblShowFull[10] = label11; label11.Location = new Point(svi += 15, svt);
            lblShowFull[11] = label12; label12.Location = new Point(svi += 15, svt);
            lblShowFull[12] = label13; label13.Location = new Point(svi += 15, svt);
            svt += 26; ;
            svi = lblShowFull[0].Left + 40;
            label14.Location = new Point(svi, svt);          //R
            label15.Location = new Point(svi, svt + 20);     //G
            label16.Location = new Point(svi, svt + 40);     //B
            svi = lblShowFull[0].Left + 164;
            label17.Location = new Point(svi, svt);         //R value
            label18.Location = new Point(svi, svt + 20);    //G value
            label19.Location = new Point(svi, svt + 40);    //B value
            svi = lblShowFull[0].Left + 58;
            hScrollBar1.Location = new Point(svi, svt);      //R adjust
            hScrollBar2.Location = new Point(svi, svt + 20); //G adjust
            hScrollBar3.Location = new Point(svi, svt + 40); //B adjust
            chk_Lock.Location = new Point(lblShowFull[0].Left - 25, svt - 1);
            chk_inner.Location = new Point(chk_Lock.Left - 2, chk_Lock.Top + 25);
            //2 在把他們加入EventHandler中
            lblShowFull[0].Click += new EventHandler(mylbl_Click);
            lblShowFull[1].Click += new EventHandler(mylbl_Click);
            lblShowFull[2].Click += new EventHandler(mylbl_Click);
            lblShowFull[3].Click += new EventHandler(mylbl_Click);
            lblShowFull[4].Click += new EventHandler(mylbl_Click);
            lblShowFull[5].Click += new EventHandler(mylbl_Click);
            lblShowFull[6].Click += new EventHandler(mylbl_Click);
            lblShowFull[7].Click += new EventHandler(mylbl_Click);
            lblShowFull[8].Click += new EventHandler(mylbl_Click);
            lblShowFull[9].Click += new EventHandler(mylbl_Click);
            lblShowFull[10].Click += new EventHandler(mylbl_Click);
            lblShowFull[11].Click += new EventHandler(mylbl_Click);
            lblShowFull[12].Click += new EventHandler(mylbl_Click);
            
            cmbPatSel.Items.Clear();
            cmbPatSel.Items.AddRange(patsel);
            cmbPatSel.SelectedIndex = 0;
            
            tslblstatus = tslbl_status;
            tslblstatus.Text = "";

            lblMark = lbl_Mark;

            //TuningArea 改到Autogamma的lblShowPattern[0]黑畫面再讀取

            mvars.FormShow[2] = true;

            chkshowextend = chk_showextend;
            lblexcorner = lbl_exCorner;
            lbledid = lbl_EDID;
            lbledidhead = label22;
            picfront = pic_Front;

            pic_Front.Width = 384;
            pic_Front.Height = 216;
            label41.Text = pic_Front.Left.ToString();
            label42.Text = pic_Front.Top.ToString();
            label43.Text = pic_Front.Width.ToString();
            label44.Text = pic_Front.Height.ToString();

            mvars.UUT.resW = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            mvars.UUT.resH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            priRes[0] = (int)mvars.UUT.resW;
            priRes[1] = (int)mvars.UUT.resH;
            mvars.TuningArea.rX = (float)Math.Round(mvars.UUT.resW / (float)384, 4);
            mvars.TuningArea.rY = (float)Math.Round(mvars.UUT.resH / (float)216, 4);

            lbl_EDIDpri.Text = priRes[0] + "X" + priRes[1];
            tslblpathboxid = tslbl_pathBoxID;
            tslbl_pathBoxID.Text = "";

            if (chk_normalsize.Checked) this.Size = new Size(Convert.ToInt16(chk_normalsize.Text.Split(',')[0]), Convert.ToInt16(chk_normalsize.Text.Split(',')[1]));
        }


        void mylbl_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            pic_Front.Image = null;
            pic_Front.Refresh();
            hScrollBar1.Value = lbl.BackColor.R; label17.Text = lbl.BackColor.R.ToString();
            hScrollBar2.Value = lbl.BackColor.G; label18.Text = lbl.BackColor.G.ToString();
            hScrollBar3.Value = lbl.BackColor.B; label19.Text = lbl.BackColor.B.ToString();
            if (chk_Lock.Checked == true && lbl.Tag == null) { chk_Lock.Checked = false; }

            label23.Visible = false;
            txt_ShowCheckBoardW.Visible = false;
            label24.Visible = false;
            txt_ShowCheckBoardH.Visible = false;

            /*string nowcolor;
            nowcolor = string.Format("&H{0:x2}{1:x2}{2:x2}", Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text));
            MessageBox.Show(nowcolor);*/
            if (cmbPatSel.SelectedIndex == 2)
            {
                pic_Front.BackColor = lbl.BackColor;
                txt_mX.Visible = false; label30.Visible = false;
                txt_mY.Visible = false; label33.Visible = false;
                txt_tW.Visible = false; label27.Visible = false;
                txt_tH.Visible = false; label28.Visible = false;
                //
                if (label17.Text == "0" && label18.Text == "0" && label19.Text == "0")
                {                   
                    Form2.fileTuningArea(false);
                    //if (chkshowextend.Checked)
                    //{
                        mvars.TuningArea.rX = Convert.ToSingle(Math.Round((float)mvars.UUT.resW / pic_Front.Width, 4, MidpointRounding.AwayFromZero));
                        mvars.TuningArea.rY = Convert.ToSingle(Math.Round((float)mvars.UUT.resH / pic_Front.Height, 4, MidpointRounding.AwayFromZero));
                    //}
                    //else
                    //{
                    //    mvars.TuningArea.rX = Convert.ToSingle(Math.Round((float)priRes[0] / pic_Front.Width, 4, MidpointRounding.AwayFromZero));
                    //    mvars.TuningArea.rY = Convert.ToSingle(Math.Round((float)priRes[1] / pic_Front.Height, 4, MidpointRounding.AwayFromZero));
                    //}

                    #region TuningArea
                    if (Convert.ToInt16(mvars.TuningArea.mX / mvars.TuningArea.rX) < 0) { label37.Text = 0.ToString(); }
                    else { label37.Text = Convert.ToInt16(mvars.TuningArea.mX / mvars.TuningArea.rX).ToString(); }
                    if (Convert.ToInt16(mvars.TuningArea.mY / mvars.TuningArea.rY) < 0) { label38.Text = 0.ToString(); }
                    else { label38.Text = Convert.ToInt16(mvars.TuningArea.mY / mvars.TuningArea.rY).ToString(); }
                    lbl_Mark.Left = Convert.ToInt16(label37.Text) + pic_Front.Left;
                    lbl_Mark.Top = Convert.ToInt16(label38.Text) + pic_Front.Top;

                    if (Convert.ToInt16(mvars.TuningArea.tW / mvars.TuningArea.rX) > pic_Front.Width) { label39.Text = pic_Front.Width.ToString(); }
                    else if (Convert.ToInt16(mvars.TuningArea.tW / mvars.TuningArea.rX) < 4) { label39.Text = "4"; }
                    else { label39.Text = Convert.ToInt16(mvars.TuningArea.tW / mvars.TuningArea.rX).ToString(); }
                    if (Convert.ToInt16(mvars.TuningArea.tH / mvars.TuningArea.rY) > pic_Front.Height) { label40.Text = pic_Front.Height.ToString(); }
                    else if (Convert.ToInt16(mvars.TuningArea.tH / mvars.TuningArea.rY) < 4) { label40.Text = "4"; }
                    else { label40.Text = Convert.ToInt16(mvars.TuningArea.tH / mvars.TuningArea.rY).ToString(); }
                    lbl_Mark.Width = Convert.ToInt16(label39.Text);
                    lbl_Mark.Height = Convert.ToInt16(label40.Text);

                    mvars.TuningArea.mX = Convert.ToInt16((lbl_Mark.Left - pic_Front.Left) * mvars.TuningArea.rX);
                    mvars.TuningArea.mY = Convert.ToInt16((lbl_Mark.Top - pic_Front.Top) * mvars.TuningArea.rY);
                    //mvars.TuningArea.tW = Convert.ToInt16((lbl_Mark.Width) * mvars.TuningArea.rX);
                    //mvars.TuningArea.tH = Convert.ToInt16((lbl_Mark.Height) * mvars.TuningArea.rY);

                    pic_Mark.Location = lbl_Mark.Location;
                    pic_Mark.Size = lbl_Mark.Size;

                    textBox1.Text = mvars.TuningArea.mX.ToString();
                    textBox2.Text = mvars.TuningArea.mY.ToString();
                    textBox3.Text = mvars.TuningArea.tW.ToString();
                    textBox4.Text = mvars.TuningArea.tH.ToString();
                    #endregion TuningArea




                    lbl_Mark.Visible = true;
                    lbl_Mark.BackColor = Color.White;

                    txt_mX.Visible = true; txt_mX.Text = mvars.TuningArea.mX.ToString(); label30.Visible = true;
                    txt_mY.Visible = true; txt_mY.Text = mvars.TuningArea.mY.ToString(); label33.Visible = true;
                    txt_tW.Visible = true; txt_tW.Text = mvars.TuningArea.tW.ToString(); label27.Visible = true;
                    txt_tH.Visible = true; txt_tH.Text = mvars.TuningArea.tH.ToString(); label28.Visible = true;

                    if (mvars.FormShow[5])
                    {
                        i3pat.lbl_Mark.Visible = true;
                        i3pat.lbl_Mark.Location = new Point(mvars.TuningArea.mX, mvars.TuningArea.mY);
                        i3pat.lbl_Mark.BackColor = lbl_Mark.BackColor;
                    }
                }
                else
                {
                    lbl_Mark.Visible = false;
                    if (mvars.TuningArea.TMode) { mvars.TuningArea.TMode = false; }
                    if (chkshowextend.Checked && lbl_Mark.Visible == true) { fileTuningArea(true); }
                    if (mvars.FormShow[5]) { i3pat.lbl_Mark.Visible = false; }
                }
                chk_inner.Visible = lbl_Mark.Visible;
            }
            else if (cmbPatSel.Text == " GrayScale")
            {
                lbl_Mark.Visible = true;
                lbl_Mark.BackColor = Color.White;


                if (mvars.FormShow[5])
                {
                    i3pat.lbl_Mark.Visible = true;
                    i3pat.lbl_Mark.Location = new Point(mvars.TuningArea.mX, mvars.TuningArea.mY);
                    i3pat.lbl_Mark.BackColor = lbl_Mark.BackColor;
                }
            }
        }




        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label17.Text = hScrollBar1.Value.ToString();
            mvars.hscF2sel = 1;
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            label18.Text = hScrollBar2.Value.ToString();
            mvars.hscF2sel = 2;
        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            label19.Text = hScrollBar3.Value.ToString();
            mvars.hscF2sel = 3;
            /*if (chk_Lock.CheckState == CheckState.Checked)
            {
                hScrollBar2.Value = Convert.ToInt16(label19.Text);
                hScrollBar1.Value = Convert.ToInt16(label19.Text);
            }*/
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            label17.Text = hScrollBar1.Value.ToString();
        }



        private void hScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            label18.Text = hScrollBar2.Value.ToString();
        }

        private void hScrollBar3_ValueChanged(object sender, EventArgs e)
        {
            label19.Text = hScrollBar3.Value.ToString();
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            label47.Text = mouse_offset.X.ToString();
            label48.Text = mouse_offset.Y.ToString();
            /*
            string newColor;
            Color c = string.Format("#{0:x2}{1:x2}{2:x2}", label17.Text, label18.Text, label19.Text);
            pic_Front.BackColor = string.Format("#{0:x2}{1:x2}{2:x2}",label17.Text, label18.Text, label19.Text);
            newColor = ColorTranslator.ToHtml(Color.FromArgb(Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
            string nowcolor;
            nowcolor = string.Format("#{0:x2}{1:x2}{2:x2}", Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text));
            pic_Front.BackColor = nowcolor;
            */
            if (cmbPatSel.SelectedIndex == 0 || cmbPatSel.SelectedIndex == 2 || cmbPatSel.SelectedIndex == 6)
            {
                if (chk_Lock.Checked == true && mvars.hscF2sel == 1)
                {
                    hScrollBar2.Value = hScrollBar1.Value;
                    hScrollBar3.Value = hScrollBar1.Value;
                }
                else if (chk_Lock.Checked == true && mvars.hscF2sel == 2)
                {
                    hScrollBar1.Value = hScrollBar2.Value;
                    hScrollBar3.Value = hScrollBar2.Value;
                }
                else if (chk_Lock.Checked == true && mvars.hscF2sel == 3)
                {
                    hScrollBar2.Value = hScrollBar3.Value;
                    hScrollBar1.Value = hScrollBar3.Value;
                }

                if (chk_inner.Checked)
                {
                    lbl_Mark.BackColor = Color.FromArgb(Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text));
                    if (chkshowextend.Checked == true) { i3pat.lbl_Mark.BackColor = lbl_Mark.BackColor; i3pat.lbl_Mark.Visible = true; }
                }
                else
                {
                    pic_Front.BackColor = Color.FromArgb(Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text));
                    if (chkshowextend.Checked == true) { i3pat.BackColor = pic_Front.BackColor; }
                }

                if (txt_tW.Visible == true)
                {
                    if (mp.IsNumeric(txt_tW.Text) && Convert.ToInt16(txt_tW.Text) != mvars.TuningArea.tW)
                    {
                        if (Convert.ToInt16(txt_tW.Text) < 1) { txt_tW.Text = mvars.TuningArea.tW.ToString(); }
                        else if (Convert.ToInt16(txt_tW.Text) > mvars.UUT.resW) { txt_tW.Text = mvars.UUT.resW.ToString(); }
                        
                        if (Convert.ToInt16(txt_mX.Text) + Convert.ToInt16(txt_tW.Text) > mvars.UUT.resW) { txt_mX.Text = (mvars.UUT.resW - Convert.ToInt16(txt_tW.Text)).ToString(); }
                        lbl_Mark.Width = Convert.ToInt16(Convert.ToInt16(txt_tW.Text) / mvars.TuningArea.rX);
                        if (Convert.ToInt16(lbl_Mark.Width) < 4) { lbl_Mark.Width = 4; }
                        if (mvars.FormShow[5]) { i3pat.lbl_Mark.Width = Convert.ToInt16(txt_tW.Text); }
                        label39.Text = lbl_Mark.Width.ToString();
                        mvars.TuningArea.tW = Convert.ToInt16(txt_tW.Text);
                    }

                    if (mp.IsNumeric(txt_tH.Text) && Convert.ToInt16(txt_tH.Text) != mvars.TuningArea.tH)
                    {
                        if (Convert.ToInt16(txt_tH.Text) < 1) { txt_tH.Text = mvars.TuningArea.tH.ToString(); }
                        else if (Convert.ToInt16(txt_tH.Text) > mvars.UUT.resH) { txt_tH.Text = mvars.UUT.resH.ToString(); }

                        if (Convert.ToInt16(txt_mY.Text) + Convert.ToInt16(txt_tH.Text) > mvars.UUT.resH) { txt_mY.Text = (mvars.UUT.resH - Convert.ToInt16(txt_tH.Text)).ToString(); }
                        lbl_Mark.Height = Convert.ToInt16(Convert.ToInt16(txt_tH.Text) / mvars.TuningArea.rY);
                        if (Convert.ToInt16(lbl_Mark.Height) < 4) { lbl_Mark.Height = 4; }
                        if (mvars.FormShow[5]) { i3pat.lbl_Mark.Height = Convert.ToInt16(txt_tH.Text); }
                        label40.Text = lbl_Mark.Height.ToString();
                        mvars.TuningArea.tH = Convert.ToInt16(txt_tH.Text);
                    }

                    if (mp.IsNumeric(txt_mX.Text) && Convert.ToInt16(txt_mX.Text) != mvars.TuningArea.mX)
                    {
                        if (Convert.ToInt16(txt_mX.Text) < 0) { txt_mX.Text = mvars.TuningArea.mX.ToString(); }
                        else if (Convert.ToInt16(txt_mX.Text) > mvars.UUT.resW - Convert.ToInt16(txt_tW.Text)) { txt_mX.Text = (mvars.UUT.resW - Convert.ToInt16(txt_tW.Text)).ToString(); }

                        lbl_Mark.Left = Convert.ToInt16(Convert.ToInt16(txt_mX.Text) / mvars.TuningArea.rX) + Convert.ToInt16(label41.Text);
                        if (mvars.FormShow[5]) { i3pat.lbl_Mark.Left = Convert.ToInt16(txt_mX.Text); }
                        label37.Text = lbl_Mark.Left.ToString();
                        mvars.TuningArea.mX = Convert.ToInt16(txt_mX.Text);
                    }

                    if (mp.IsNumeric(txt_mY.Text) && Convert.ToInt16(txt_mY.Text) != mvars.TuningArea.mY)
                    {
                        if (Convert.ToInt16(txt_mY.Text) < 0) { txt_mY.Text = mvars.TuningArea.mY.ToString(); }
                        else if (Convert.ToInt16(txt_mY.Text) > mvars.UUT.resH - Convert.ToInt16(txt_tH.Text)) { txt_mY.Text = (mvars.UUT.resH - Convert.ToInt16(txt_tH.Text)).ToString(); }

                        lbl_Mark.Top = Convert.ToInt16(Convert.ToInt16(txt_mY.Text) / mvars.TuningArea.rY) + Convert.ToInt16(label42.Text);
                        if (mvars.FormShow[5]) { i3pat.lbl_Mark.Top = Convert.ToInt16(txt_mY.Text); }
                        label38.Text = lbl_Mark.Top.ToString();
                        mvars.TuningArea.mY = Convert.ToInt16(txt_mY.Text);
                    }
                }
            }

        }



        private void chk_Lock_Click(object sender, EventArgs e)
        {
            if (chk_Lock.Checked == true)
            {
                hScrollBar2.Value = hScrollBar1.Value;
                hScrollBar3.Value = hScrollBar1.Value;
            }
        }

        //下拉式清單方塊，選取時所引發的事件
        private void cmbPatSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mvars.TuningArea.TMode)
            {
                pvampMaxcount = 0;
                mvars.TuningArea.TMode = false;
            }
            #region 卸載動態控件
            if (svpbf != null)
            {
                for (int outer = svpbf.GetLowerBound(0); outer <= svpbf.GetUpperBound(0); outer++)
                    for (int inner = svpbf.GetLowerBound(1); inner <= svpbf.GetUpperBound(1); inner++)
                        svpbf[outer, inner].Dispose();
                #region method1                        
                //foreach(Control ctl in pic_Front.Controls)
                //{
                //    if (ctl is PictureBox && ctl.Tag.ToString() == "add") { ctl.Dispose(); }
                //}
                #endregion method1
            }
            #endregion 卸載動態控件


            pic_Mark.Visible = false;
            //取得清單項目索引值
            chk_inner.Visible = false;
            int index = cmbPatSel.SelectedIndex;
            hScrollBar1.Value = 128; 
            hScrollBar2.Value = 128; 
            hScrollBar3.Value = 128; 
            pic_Front.Image = null;
            //pic_Front.Refresh();
            pic_Front.BackColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            if (chk_showextend.Checked && lbl_Mark.Visible == true) { fileTuningArea(true); }
            lbl_Mark.Visible = false;
            txt_bmpFileName.Visible = false;
            if (mvars.FormShow[5]) { i3pat.BackgroundImage = null; i3pat.lbl_Mark.Visible = false; i3pat.pic_Mark.Visible = false; }
            label23.Text = "col";label24.Text = "row";
            txt_mX.Visible = false; label30.Visible = false;
            txt_mY.Visible = false; label33.Visible = false;
            txt_tW.Visible = false; label27.Visible = false;
            txt_tH.Visible = false; label28.Visible = false;
            txt_ms.Visible = false; label35.Visible = false;
            switch (index)
            {
                case 0: //" FULL"
                    timer3.Enabled = true;
                    label1.Visible = true; 
                    label2.Visible = true;
                    label3.Visible = true; 
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                    label8.Visible = true; 
                    label9.Visible = true;
                    label10.Visible = true;
                    label11.Visible = true;
                    label12.Visible = true;
                    label13.Visible = true;
                    label14.Visible = true;
                    label15.Visible = true;
                    label16.Visible = true; 
                    label17.Visible = true; 
                    label18.Visible = true; 
                    label19.Visible = true; 
                    hScrollBar1.Visible = true; 
                    hScrollBar2.Visible = true; 
                    hScrollBar3.Visible = true; 
                    chk_Lock.Visible = true;
                    btnShowCheckBoard.Visible = false;
                    label23.Visible = false;
                    txt_ShowCheckBoardW.Visible = false;
                    label24.Visible = false;
                    txt_ShowCheckBoardH.Visible = false;
                    if (chk_inner.Checked == true) { chk_inner.Checked = false; }
                    break;
                case 1: //" CheckBox"
                    timer3.Enabled = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                    label12.Visible = false;
                    label13.Visible = false;
                    label14.Visible = false;
                    label15.Visible = false;
                    label16.Visible = false;
                    label17.Visible = false;
                    label18.Visible = false;
                    label19.Visible = false;
                    hScrollBar1.Visible = false;
                    hScrollBar2.Visible = false;
                    hScrollBar3.Visible = false;
                    chk_Lock.Visible = false;
                    btnShowCheckBoard.Visible = true; btnShowCheckBoard.Location = new Point(pic_Front.Left + 73, lblShowFull[0].Top + 4);
                    label23.Visible = true; label23.Location = new Point(btnShowCheckBoard.Left + 80, lblShowFull[0].Top + 7);
                    txt_ShowCheckBoardW.Visible = true; txt_ShowCheckBoardW.Location = new Point(btnShowCheckBoard.Left + 104, lblShowFull[0].Top + 4);
                    txt_ShowCheckBoardW.Text = "16";
                    label24.Visible = true; label24.Location = new Point(btnShowCheckBoard.Left + 160, lblShowFull[0].Top + 7);
                    txt_ShowCheckBoardH.Visible = true; txt_ShowCheckBoardH.Location = new Point(btnShowCheckBoard.Left + 188, lblShowFull[0].Top + 4);
                    txt_ShowCheckBoardH.Text = "8";
                    if (chk_inner.Checked == true) { chk_inner.Checked = false; }
                    btnShowCheckBoard_Click(sender, null);
                    break;
                case 2:     //" AutoGamma"
                    timer3.Enabled = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                    label8.Visible = true;
                    label9.Visible = true;
                    label10.Visible = true;
                    label11.Visible = true;
                    label12.Visible = true;
                    label13.Visible = true;
                    label14.Visible = true;
                    label15.Visible = true;
                    label16.Visible = true;
                    label17.Visible = true;
                    label18.Visible = true;
                    label19.Visible = true;
                    hScrollBar1.Visible = true;
                    hScrollBar2.Visible = true;
                    hScrollBar3.Visible = true;
                    chk_Lock.Visible = true;
                    btnShowCheckBoard.Visible = false;
                    label23.Visible = false;
                    txt_ShowCheckBoardW.Visible = false;
                    label24.Visible = false;
                    txt_ShowCheckBoardH.Visible = false;

                    txt_mX.Location = new Point(lblShowFull[0].Left + 200, label17.Top + 3); //txt_mX.Text = mvars.TuningArea.mX.ToString();
                    label30.Location = new Point(txt_mX.Left, txt_mX.Top - 12); 
                    txt_mY.Location = new Point(lblShowFull[0].Left + 250, label17.Top + 3); //txt_mY.Text = mvars.TuningArea.mY.ToString();
                    label33.Location = new Point(txt_mY.Left, txt_mY.Top - 12); 
                    txt_tW.Location = new Point(lblShowFull[0].Left + 250, label17.Top + 38); //txt_tW.Text = mvars.TuningArea.tW.ToString();
                    label27.Location = new Point(txt_tW.Left, txt_tW.Top - 12);
                    txt_tH.Location = new Point(lblShowFull[0].Left + 300, label17.Top + 38); //txt_tH.Text = mvars.TuningArea.tH.ToString();
                    label28.Location = new Point(txt_tH.Left, txt_tH.Top - 12);
                    break;
                case 3: //" BMP"
                    timer3.Enabled = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                    label12.Visible = false;
                    label13.Visible = false;
                    label14.Visible = false;
                    label15.Visible = false;
                    label16.Visible = false;
                    label17.Visible = false;
                    label18.Visible = false;
                    label19.Visible = false;
                    hScrollBar1.Visible = false;
                    hScrollBar2.Visible = false;
                    hScrollBar3.Visible = false;
                    chk_Lock.Visible = false;
                    btnShowCheckBoard.Visible = false;
                    label23.Visible = false;
                    txt_ShowCheckBoardW.Visible = false;
                    label24.Visible = false;
                    txt_ShowCheckBoardH.Visible = false;
                    btnShowCheckBoard.Visible = true; btnShowCheckBoard.Location = new Point(80, lblShowFull[0].Top + 4);
                    txt_bmpFileName.Visible = true;txt_bmpFileName.Location = new Point(80, lblShowFull[0].Top + 33);
                    txt_bmpFileName.Text = "*.bmp (double click here)";
                    if (chk_inner.Checked == true) { chk_inner.Checked = false; }
                    break;
                case 4:     //" GrayScale"
                    timer3.Enabled = false;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                    label8.Visible = true;
                    label9.Visible = true;
                    label10.Visible = true;
                    label11.Visible = true;
                    label12.Visible = true;
                    label13.Visible = true;
                    label14.Visible = false;
                    label15.Visible = false;
                    label16.Visible = false;
                    label17.Visible = false;
                    label18.Visible = false;
                    label19.Visible = false;
                    hScrollBar1.Visible = false;
                    hScrollBar2.Visible = false;
                    hScrollBar3.Visible = false;
                    chk_Lock.Visible = false;
                    
                    btnShowCheckBoard.Visible = true; btnShowCheckBoard.Location = new Point(pic_Front.Left + 73, lblShowFull[0].Top + lblShowFull[0].Height + 14);
                    label23.Visible = true; label23.Location = new Point(btnShowCheckBoard.Left + 80, lblShowFull[0].Top + lblShowFull[0].Height + 17);
                    txt_ShowCheckBoardW.Visible = true; txt_ShowCheckBoardW.Location = new Point(btnShowCheckBoard.Left + 104, lblShowFull[0].Top + lblShowFull[0].Height + 14);
                    label24.Visible = true; label24.Location = new Point(btnShowCheckBoard.Left + 160, lblShowFull[0].Top + lblShowFull[0].Height + 17);
                    txt_ShowCheckBoardH.Visible = true; txt_ShowCheckBoardH.Location = new Point(btnShowCheckBoard.Left + 188, lblShowFull[0].Top + lblShowFull[0].Height + 14);
                    
                    if (chk_inner.Checked == true) { chk_inner.Checked = false; }

                    break;

                case 5: //" Coding"
                    timer3.Enabled = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                    label12.Visible = false;
                    label13.Visible = false;
                    label14.Visible = false;
                    label15.Visible = false;
                    label16.Visible = false;
                    label17.Visible = false;
                    label18.Visible = false;
                    label19.Visible = false;
                    hScrollBar1.Visible = false;
                    hScrollBar2.Visible = false;
                    hScrollBar3.Visible = false;
                    chk_Lock.Visible = false;
                    btnShowCheckBoard.Visible = false;
                    label23.Visible = false;
                    txt_ShowCheckBoardW.Visible = false;
                    label24.Visible = false;
                    txt_ShowCheckBoardH.Visible = false;
                    btnShowCheckBoard.Visible = true; btnShowCheckBoard.Location = new Point(pic_Front.Left + 73, lblShowFull[0].Top + 4);
                    txt_bmpFileName.Visible = true; txt_bmpFileName.Location = new Point(pic_Front.Left + 73, lblShowFull[0].Top + 33);
                    label27.Location = new Point(txt_bmpFileName.Left, txt_bmpFileName.Top + 29);
                    txt_tW.Location = new Point(label27.Left, label27.Top + 12);
                    label28.Location = new Point(label27.Left + 50, txt_bmpFileName.Top + 29);
                    txt_tH.Location = new Point(label27.Left + 50, label28.Top + 12);
                    txt_tW.Text = "480";
                    txt_tH.Text = "540";
                    label27.Visible = true;
                    txt_tW.Visible = true;
                    label28.Visible = true;
                    txt_tH.Visible = true;
                    txt_bmpFileName.Text = "*.bin (double click here)";
                    if (chk_inner.Checked == true) { chk_inner.Checked = false; }
                    //
                    break;
                case 6: //" RT"
                    timer3.Enabled = true;
                    timer1.Enabled = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                    label8.Visible = true;
                    label9.Visible = true;
                    label10.Visible = true;
                    label11.Visible = true;
                    label12.Visible = true;
                    label13.Visible = true;
                    label14.Visible = true;
                    label15.Visible = true;
                    label16.Visible = true;
                    label17.Visible = true;
                    label18.Visible = true;
                    label19.Visible = true;
                    hScrollBar1.Visible = true;
                    hScrollBar2.Visible = true;
                    hScrollBar3.Visible = true;
                    chk_Lock.Visible = true;
                    btnShowCheckBoard.Visible = false;
                    label23.Visible = false;
                    txt_ShowCheckBoardW.Visible = false;
                    label24.Visible = false;
                    txt_ShowCheckBoardH.Visible = false;

                    txt_mX.Location = new Point(lblShowFull[0].Left + 200, label17.Top + 3); //txt_mX.Text = mvars.TuningArea.mX.ToString();
                    label30.Location = new Point(txt_mX.Left, txt_mX.Top - 12);
                    txt_mY.Location = new Point(lblShowFull[0].Left + 250, label17.Top + 3); //txt_mY.Text = mvars.TuningArea.mY.ToString();
                    label33.Location = new Point(txt_mY.Left, txt_mY.Top - 12);
                    txt_tW.Location = new Point(lblShowFull[0].Left + 250, label17.Top + 38); //txt_tW.Text = mvars.TuningArea.tW.ToString();
                    label27.Location = new Point(txt_tW.Left, txt_tW.Top - 12);
                    txt_tH.Location = new Point(lblShowFull[0].Left + 300, label17.Top + 38); //txt_tH.Text = mvars.TuningArea.tH.ToString();
                    label28.Location = new Point(txt_tH.Left, txt_tH.Top - 12);
                    txt_ms.Location = new Point(lblShowFull[0].Left + 350, label17.Top + 38); //txt_tH.Text = mvars.TuningArea.tH.ToString();
                    label35.Location = new Point(txt_ms.Left, txt_tH.Top - 12);
                    //
                    lbl_Mark.Visible = true;
                    lbl_Mark.BackColor = Color.White;

                    txt_mX.Visible = true; txt_mX.Text = mvars.TuningArea.mX.ToString(); label30.Visible = true;
                    txt_mY.Visible = true; txt_mY.Text = mvars.TuningArea.mY.ToString(); label33.Visible = true;
                    txt_tW.Visible = true; txt_tW.Text = mvars.TuningArea.tW.ToString(); label27.Visible = true;
                    txt_tH.Visible = true; txt_tH.Text = mvars.TuningArea.tH.ToString(); label28.Visible = true;

                    txt_ms.Visible = true; txt_ms.Text = timer1.Interval.ToString(); label35.Visible = true;
                    chk_inner.Visible = lbl_Mark.Visible;

                    RT(txt_ms.Text);
                    break;
            }
        }

        private void lbl_Mark_MouseDown(object sender, MouseEventArgs e)
        {
            if (mvars.TuningArea.TMode)
            {
                if (e.Button == MouseButtons.Left)  //shrink
                {
                    pvampMaxcount = 0;
                    if (mvars.TuningArea.tW >= 1)
                    {
                        mvars.TuningArea.tW = Convert.ToInt32(mvars.TuningArea.tW * 0.9);   //Convert.ToInt32(0.9)=1
                        mvars.TuningArea.tH = Convert.ToInt32(mvars.TuningArea.tH * 0.9);
                    }
                    //
                    if (lbl_Mark.Width >= 10)
                    {
                        lbl_Mark.Width = Convert.ToInt32(mvars.TuningArea.tW / mvars.TuningArea.rX);
                        lbl_Mark.Height = Convert.ToInt32(mvars.TuningArea.tH / mvars.TuningArea.rY);
                    }
                    
                    pic_Mark.Width = lbl_Mark.Width;
                    pic_Mark.Height = lbl_Mark.Height;
                }
                else if (e.Button == MouseButtons.Right)    //amplify
                {
                    int svi = 0;
                    if (lbl_Mark.Width >= lbl_Mark.Height)
                    {
                        if (Convert.ToInt32(lbl_Mark.Width * 1.1) + Convert.ToInt32(label25.Text) <= pic_Front.Width)
                        {
                            lbl_Mark.Width = Convert.ToInt32(lbl_Mark.Width * 1.1);
                            lbl_Mark.Height = Convert.ToInt32(lbl_Mark.Height * 1.1);
                        }
                        else
                        {
                            svi = lbl_Mark.Width;
                            lbl_Mark.Width = pic_Front.Width - Convert.ToInt32(label25.Text);
                            lbl_Mark.Height = lbl_Mark.Height + (lbl_Mark.Width - svi);
                            if (pvampMaxcount <= 2) { pvampMaxcount += 1; lbl_Mark.Height = lbl_Mark.Height + (lbl_Mark.Width - svi); }
                            else { if (lbl_Mark.Height < pic_Front.Height - Convert.ToInt32(label26.Text)) lbl_Mark.Height = pic_Front.Height - Convert.ToInt32(label26.Text); }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(lbl_Mark.Height * 1.1) + Convert.ToInt32(label26.Text) <= pic_Front.Height)
                        {
                            lbl_Mark.Height = Convert.ToInt32(lbl_Mark.Height * 1.1);
                            lbl_Mark.Width = Convert.ToInt32(lbl_Mark.Width * 1.1);
                        }
                        else
                        {
                            svi = lbl_Mark.Height;
                            lbl_Mark.Height = pic_Front.Height - Convert.ToInt32(label26.Text);
                            if (pvampMaxcount <= 2) { pvampMaxcount += 1; lbl_Mark.Width = lbl_Mark.Width + (lbl_Mark.Height - svi); }
                            else { if (lbl_Mark.Width < pic_Front.Width - Convert.ToInt32(label25.Text)) lbl_Mark.Width = pic_Front.Width - Convert.ToInt32(label25.Text); }
                        }
                    }
                    if (lbl_Mark.Height >= pic_Front.Height) { lbl_Mark.Height = pic_Front.Height; }
                    if (lbl_Mark.Width >= pic_Front.Width) { lbl_Mark.Width = pic_Front.Width; }
                    if (lbl_Mark.Height >= pic_Front.Height - Convert.ToInt32(label26.Text)) { lbl_Mark.Height = pic_Front.Height - Convert.ToInt32(label26.Text); }
                    if (lbl_Mark.Width >= pic_Front.Width - Convert.ToInt32(label25.Text)) { lbl_Mark.Width = pic_Front.Width - Convert.ToInt32(label25.Text); }
                    //
                    mvars.TuningArea.tW = Convert.ToInt32(mvars.TuningArea.rX * lbl_Mark.Width);
                    mvars.TuningArea.tH = Convert.ToInt32(mvars.TuningArea.rY * lbl_Mark.Height);
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    pvampMaxcount = 0;
                    mvars.TuningArea.TMode = false;
                }
                
                label39.Text = lbl_Mark.Width.ToString();
                label40.Text = lbl_Mark.Height.ToString();
                
                if (mvars.FormShow[5])
                {
                    i3pat.lbl_Mark.Width = mvars.TuningArea.tW;
                    i3pat.lbl_Mark.Height = mvars.TuningArea.tH;
                }
                pic_Mark.Width = lbl_Mark.Width;
                pic_Mark.Height = lbl_Mark.Height;
            }
            else
            {
                if (e.Button == MouseButtons.Left) { mouse_offset = new Point(-e.X, -e.Y); txt_tW.Text = lbl_Mark.Width.ToString(); txt_tH.Text = lbl_Mark.Height.ToString(); }
                else if (e.Button == MouseButtons.Right)
                {
                    if (MessageBox.Show("Re - Size mode ON ?? ", "ATG Area Setup", MessageBoxButtons.OKCancel) == DialogResult.OK) { mvars.TuningArea.TMode = true; }
                }
            }

            
        }
        private void lbl_Mark_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mvars.TuningArea.TMode == false)
            {
                Label lbl = (Label)sender;
                Point mousePos = PointToClient(Control.MousePosition);//Control.MousePosition可以取得滑鼠在螢幕中的座標值，但因按鈕是附著於Form視窗，因此必須使用PointToClient方法把螢幕做標值轉換為Form視窗中的座標值。
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);//使用者可能是點擊按鈕左上角以外的位置，因此需使用Offset修正，否則移動放開後按鈕總會落在滑鼠的右下方。

                if (mousePos.X < pic_Front.Left) mousePos.X = pic_Front.Left;
                if (mousePos.Y < pic_Front.Top) mousePos.Y = pic_Front.Top;
                if (mousePos.X > pic_Front.Left + pic_Front.Width - lbl_Mark.Width) mousePos.X = pic_Front.Left + pic_Front.Width - lbl_Mark.Width;
                if (mousePos.Y > pic_Front.Top + pic_Front.Height - lbl_Mark.Height) mousePos.Y = pic_Front.Top + pic_Front.Height - lbl_Mark.Height;
                lbl.Location = mousePos;
                label25.Text = (mousePos.X - pic_Front.Left).ToString();    //=lbl_Mark.Left absolute X
                label26.Text = (mousePos.Y - pic_Front.Top).ToString();     //=lbl_Mark.Top  absolute Y   
                label37.Text = lbl_Mark.Left.ToString();    //相對於pic_Front的X
                label38.Text = lbl_Mark.Top.ToString();     //相對於pic_Front的Y
                //
                mvars.TuningArea.mX = Convert.ToInt32((mousePos.X - pic_Front.Left) * mvars.TuningArea.rX);
                mvars.TuningArea.mY = Convert.ToInt32((mousePos.Y - pic_Front.Top) * mvars.TuningArea.rY);
                label51.Text = mvars.TuningArea.mX.ToString();
                label52.Text = mvars.TuningArea.mY.ToString();
                label23.Text = label51.Text;
                label24.Text = label52.Text;
                txt_mX.Text = label51.Text;
                txt_mY.Text = label52.Text;
                if (mvars.FormShow[5]) { i3pat.lbl_Mark.Location = new Point(mvars.TuningArea.mX, mvars.TuningArea.mY); }
                //
                pic_Mark.Location = lbl_Mark.Location;
                //
                pic_Front.Refresh();
            }
        }

        
        public static bool fileTuningArea(bool svWrite)
        {
            bool svb = true;
            string svfile = mvars.strStartUpPath + @"\Parameter\TuningArea.txt";
            if (svWrite)
            {
                if (File.Exists(svfile)) { File.Delete(svfile); }
                //FileInfo copyFile = new FileInfo(svfile);
                StreamWriter sTAwrite = File.CreateText(svfile);
                sTAwrite.WriteLine("Mark:" + mvars.TuningArea.Mark);
                sTAwrite.WriteLine("tW:" + mvars.TuningArea.tW);
                sTAwrite.WriteLine("tH:" + mvars.TuningArea.tH);
                sTAwrite.WriteLine("mX:" + mvars.TuningArea.mX);
                sTAwrite.WriteLine("mY:" + mvars.TuningArea.mY);
                sTAwrite.WriteLine("Loading:" + mvars.TuningArea.Loading);
                sTAwrite.WriteLine("rX:" + mvars.TuningArea.rX);
                sTAwrite.WriteLine("rY:" + mvars.TuningArea.rY);
                sTAwrite.Flush(); //清除緩衝區
                sTAwrite.Close(); //關閉檔案
            }
            else
            {
                if (File.Exists(svfile))
                {
                    StreamReader sTAread = File.OpenText(svfile);
                    int svi = 0;
                    while (true)
                    {
                        string data = sTAread.ReadLine();
                        if (data == null) { svi -= 1; break; }
                        if (data.IndexOf(':', 1) == -1) { break; }
                        string[] Svss = data.Split(':');
                        svi = -1;
                        if (Svss[0].IndexOf("Mark", 0) > -1) { mvars.TuningArea.Mark = Svss[1]; svi = 0; }
                        if (Svss[0].IndexOf("tW", 0) > -1 && mp.IsNumeric(Svss[1])) { mvars.TuningArea.tW = Convert.ToInt16(Svss[1]); svi = 1; }
                        if (Svss[0].IndexOf("tH", 0) > -1 && mp.IsNumeric(Svss[1])) { mvars.TuningArea.tH = Convert.ToInt16(Svss[1]); svi = 2; }
                        if (Svss[0].IndexOf("mX", 0) > -1 && mp.IsNumeric(Svss[1])) { mvars.TuningArea.mX = Convert.ToInt16(Svss[1]); svi = 3; }
                        if (Svss[0].IndexOf("mY", 0) > -1 && mp.IsNumeric(Svss[1])) { mvars.TuningArea.mY = Convert.ToInt16(Svss[1]); svi = 4; }
                        if ((Svss[0].IndexOf("Percent", 0) > -1 || Svss[0].IndexOf("Loading", 0) > -1)) { mvars.TuningArea.Loading = Convert.ToInt16(Svss[1]); svi = 5; }
                        if ((Svss[0].IndexOf("sX", 0) > -1 || Svss[0].IndexOf("rX", 0) > -1) && mp.IsNumeric(Svss[1])) { svi = 6; }// { mvars.TuningArea.rX = Convert.ToSingle(Svss[1]); svi = 6; }
                        if ((Svss[0].IndexOf("sY", 0) > -1 || Svss[0].IndexOf("rY", 0) > -1) && mp.IsNumeric(Svss[1])) { svi = 7; }//  { mvars.TuningArea.rY = Convert.ToSingle(Svss[1]); svi = 7; }
                    }
                    sTAread.Close();
                }
                if (mvars.TuningArea.tW <= 0) { mvars.TuningArea.tW = 19; } //pic_Front.Width / 20;     384/20=19.2=19
                if (mvars.TuningArea.tH <= 0) { mvars.TuningArea.tH = 10; } //pic_Front.Height / 20;    216/20=10.8=10
                if (mvars.TuningArea.mX <= 0) { }
                if (mvars.TuningArea.mY <= 0) { }
                if (mvars.TuningArea.Loading <= 0) { mvars.TuningArea.Loading = 30; }
                //if (mvars.TuningArea.rX <= 0) { mvars.TuningArea.rX = 1; }
                //if (mvars.TuningArea.rY <= 0) { mvars.TuningArea.rY = 1; }

                if (File.Exists(svfile) == false)
                {
                    svb = false;
                    StreamWriter sTAwrite = File.CreateText(svfile);
                    sTAwrite.WriteLine("Mark:" + mvars.TuningArea.Mark);
                    sTAwrite.WriteLine("tW:" + mvars.TuningArea.tW);
                    sTAwrite.WriteLine("tH:" + mvars.TuningArea.tH);
                    sTAwrite.WriteLine("mX:" + mvars.TuningArea.mX);
                    sTAwrite.WriteLine("mY:" + mvars.TuningArea.mY);
                    sTAwrite.WriteLine("Loading:" + mvars.TuningArea.Loading);
                    sTAwrite.WriteLine("rX:" + mvars.TuningArea.rX);
                    sTAwrite.WriteLine("rY:" + mvars.TuningArea.rY);
                    sTAwrite.Flush(); //清除緩衝區
                    sTAwrite.Close(); //關閉檔案
                }
            }
            return svb;
        }




        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (chkshowextend.Checked && lbl_Mark.Visible == true) { fileTuningArea(true); }

            mvars.FormShow[2] = false;
            if (mvars.FormShow[5])
            {
                i3pat.BackgroundImage = null;
                i3pat.Close();
                chkshowextend.Checked = false;
            }
            Form1.frm2.Dispose();
        }

        

        private void btnShowCheckBoard_Click(object sender, EventArgs e)
        {
            if (cmbPatSel.Text == patsel[5])
            {
                //int svi = mvars.lstget.Items.Count; //檔案起始
                int svi = Form1.lstget1.Items.Count; //檔案起始
                if (File.Exists(txt_bmpFileNameFull.Text))
                {
                    StreamReader sTAread = File.OpenText(txt_bmpFileNameFull.Text);
                    string[] svs = txt_bmpFileNameFull.Text.Split('\\');
                    while (true)
                    {
                        string data = sTAread.ReadLine();
                        if (data == null) { break; }
                        //mvars.lstget.Items.Add(data);
                        Form1.lstget1.Items.Add(data);
                    }
                    sTAread.Close();

                    //pathBoxID = mvars.lstget.Items[svi].ToString();
                    //int svrows = Convert.ToInt16(mvars.lstget.Items[svi + 1].ToString().Split('x')[0]);
                    pathBoxID = Form1.lstget1.Items[svi].ToString();
                    int svrows = Convert.ToInt16(Form1.lstget1.Items[svi + 1].ToString().Split('x')[0]);
                    tslblpathboxid.Text = pathBoxID;

                    dataBoxID = new string[svrows];

                    svi = 1;
                    do
                    {
                        //dataBoxID[svi - 1] = mvars.lstget.Items[mvars.lstget.Items.Count - svi].ToString();
                        dataBoxID[svi - 1] = Form1.lstget1.Items[Form1.lstget1.Items.Count - svi].ToString();
                        svi++;
                    }
                    while (svi <= svrows);

                    bxw = Convert.ToInt16(txt_tW.Text);
                    bxh = Convert.ToInt16(txt_tH.Text);

                    #region 卸載動態控件
                    if (svpbf != null)
                    {
                        for (int outer = svpbf.GetLowerBound(0); outer <= svpbf.GetUpperBound(0); outer++)
                            for (int inner = svpbf.GetLowerBound(1); inner <= svpbf.GetUpperBound(1); inner++)
                                svpbf[outer, inner].Dispose();
                        #region method1                        
                        //foreach(Control ctl in pic_Front.Controls)
                        //{
                        //    if (ctl is PictureBox && ctl.Tag.ToString() == "add") { ctl.Dispose(); }
                        //}
                        #endregion method1
                    }
                    #endregion 卸載動態控件

                    Bitmap bmpf = new Bitmap(pic_Front.Width, pic_Front.Height);
                    Graphics g3 = Graphics.FromImage(bmpf);
                    int svws = 1;
                    int svhs = 1;
                    if (mvars.UUT.resW > bxw)
                    {
                        svws = Convert.ToInt16(mvars.UUT.resW) / bxw;
                        for (int i = 1; i < svws; i++)
                        {
                            g3.DrawLine(new Pen(Color.FromArgb(63, 255, 255), 1), new Point(Convert.ToInt16(i * bxw / mvars.TuningArea.rX), 0), new Point(Convert.ToInt16(i * bxw / mvars.TuningArea.rX), pic_Front.Height));
                        }
                    }
                    if (mvars.UUT.resH > bxh)
                    {
                        svhs = Convert.ToInt16(mvars.UUT.resH) / bxh;
                        for (int i = 1; i < svhs; i++)
                        {
                            g3.DrawLine(new Pen(Color.FromArgb(63, 255, 255), 1), new Point(0, Convert.ToInt16(i * bxh / mvars.TuningArea.rY)), new Point(pic_Front.Width, Convert.ToInt16(i * bxh / mvars.TuningArea.rY)));
                        }
                    }
                    for (int svh = 0; svh < svhs; svh++)
                    {
                        for (int i = 0; i < svws; i++)
                        {
                            g3.DrawString(dataBoxID[svh].Split(',')[i], new Font("Arial", 9), new SolidBrush(BackColor), new Point(Convert.ToInt16(i * bxw / mvars.TuningArea.rX), Convert.ToInt16(bxh * svh / mvars.TuningArea.rY)));
                        }
                    }
                    pic_Front.Image = bmpf;
                    pic_Front.Refresh();

                    if (mvars.FormShow[5])
                    {
                        i3pat.Close();

                        #region
                        if (mp.upperBound == 0)
                        {
                            label22.Text = "Primary / EDID";
                            chkshowextend.Checked = false;
                            if (mvars.FormShow[5] == false)
                            {
                                i3pat = new i3_Pat();
                                i3pat.BackColor = Color.Black;
                                i3pat.Show();
                                i3pat.Visible = false;
                                i3pat.Location = new System.Drawing.Point(0, 0);
                                i3pat.FormBorderStyle = FormBorderStyle.Sizable;
                                i3pat.ControlBox = true;
                                i3pat.MinimizeBox = false;
                                i3pat.MaximizeBox = false;
                                i3pat.Size = new Size(200, 200);
                                i3pat.TopMost = true;
                                i3pat.Visible = true;
                            }
                            fileTuningArea(true);
                        }
                        else
                        {
                            string[] svs1 = lblexcorner.Text.Split(' ');
                            svs1 = svs1[0].Split(',');
                            label22.Text = "Extend / EDID";
                            i3pat = new i3_Pat();
                            //i3pat.lbl_Mark.Visible = false;
                            i3pat.BackColor = Color.Black;
                            i3pat.Visible = false;
                            i3pat.Location = new System.Drawing.Point(Convert.ToInt16(svs1[0]), Convert.ToInt16(svs1[1]));
                            i3pat.FormBorderStyle = FormBorderStyle.None;
                            i3pat.ControlBox = false;
                            i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                            i3pat.TopMost = true;
                            i3pat.Visible = true;
                            i3pat.Show();
                        }
                        #endregion

                        i3pat.pic_Mark.Visible = false;
                        i3pat.lbl_Mark.Visible = false;
                        bmpf = new Bitmap(i3pat.Width, i3pat.Height);
                        g3 = Graphics.FromImage(bmpf);
                        g3.Clear(Color.Black);
                        for (int i = 1; i < svws; i++)
                        {
                            g3.DrawLine(new Pen(Color.FromArgb(63, 255, 255), 1), new Point(Convert.ToInt16(i * bxw), 0), new Point(Convert.ToInt16(i * bxw), i3pat.Height));
                        }
                        for (int i = 1; i < svhs; i++)
                        {
                            g3.DrawLine(new Pen(Color.FromArgb(63, 255, 255), 1), new Point(0, Convert.ToInt16(i * bxh)), new Point(i3pat.Width, Convert.ToInt16(i * bxh)));
                        }

                        for (int svh = 0; svh < svhs; svh++)
                        {
                            for (int i = 0; i < svws; i++)
                            {
                                g3.DrawString(dataBoxID[svh].Split(',')[i], new Font("Arial", 20), new SolidBrush(BackColor), new Point(i * bxw, bxh * svh));
                            }
                        }
                        i3pat.BackgroundImage = bmpf;
                        i3pat.Refresh();
                    }
                    bmpf.Dispose();


                    this.Focus();
                    if (MessageBox.Show("Drawing the bin content?", mvars.strUInameMe + " " + mvars.UImajor, MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        svpbf = new PictureBox[svhs, svws];
                        if (mvars.FormShow[5]) { i3_Pat.addcontrol(svhs, svws, bxw, bxh); }
                        int svn = 0; ;
                        for (int svh = 0; svh < svhs; svh++)
                        {
                            for (int svw = 0; svw < svws; svw++)
                            {
                                mp.GetBin(pathBoxID + @"\" + dataBoxID[svh].Split(',')[svw] + @"\" + dataBoxID[svh].Split(',')[svw] + ".bin");
                                if (File.Exists(mvars.strStartUpPath + @"\Parameter\demura.bmp")) { File.Delete(mvars.strStartUpPath + @"\Parameter\demura.bmp"); }
                                mp.saveFS_mk6(mvars.strStartUpPath + @"\Parameter\demura.bmp", ref svn, bxw, bxh, 1024, Convert.ToInt16(bxh / 256) * 256);
                                bmpf = new Bitmap(mvars.strStartUpPath + @"\Parameter\demura.bmp");

                                svpbf[svh, svw] = new System.Windows.Forms.PictureBox();
                                svpbf[svh, svw].SetBounds(Convert.ToInt16(svw * bxw / mvars.TuningArea.rX), Convert.ToInt16(svh * bxh / mvars.TuningArea.rY), Convert.ToInt16(bxw / mvars.TuningArea.rX), Convert.ToInt16(bxh / mvars.TuningArea.rY));
                                pic_Front.Controls.AddRange(new Control[] { svpbf[svh, svw] });
                                //this.Controls.AddRange(new Control[] { svpbf[svh, svw] });
                                svpbf[svh, svw].Tag = "add";
                                svpbf[svh, svw].Visible = true;
                                //縮圖↓
                                int imageFromWidth = bxw;
                                int imageFromHeight = bxh;
                                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(() => { return false; });
                                //呼叫Image物件自帶的GetThumbnailImage()進行圖片縮略
                                Image reducedImage = bmpf.GetThumbnailImage(svpbf[svh, svw].Width, svpbf[svh, svw].Height, callb, IntPtr.Zero);
                                //將圖片以指定的格式儲存到到指定的位置
                                reducedImage.Save(mvars.strStartUpPath + @"\Parameter\demura_small.bmp");
                                Bitmap bmps = new Bitmap(mvars.strStartUpPath + @"\Parameter\demura_small.bmp");
                                //縮圖↑
                                svpbf[svh, svw].Image = bmps;
                                svpbf[svh, svw].Refresh();

                                if (mvars.FormShow[5])
                                {
                                    //svpb[svh, svw] = new System.Windows.Forms.PictureBox();
                                    //svpb[svh, svw].SetBounds(svw * bxw, svh * bxh, bxw , bxh);
                                    //i3pat.Controls.AddRange(new Control[] { svpb[svh, svw] });
                                    //svpb[svh, svw].Tag = "add";
                                    //svpb[svh, svw].Visible = true;
                                    //svpb[svh, svw].Image = bmpf;
                                    //svpb[svh, svw].Refresh();

                                    i3_Pat.pb[svh, svw].Image = bmpf;
                                    i3_Pat.pb[svh, svw].Visible = true;
                                    i3_Pat.pb[svh, svw].Refresh();
                                }

                                bmps.Dispose();
                                bmpf.Dispose();
                            }
                        }
                        bmpf.Dispose();
                    }





                    //獲取矩陣長度
                    // Create a two-dimensional integer array.
                    //int[,] svpb = { {2, 4}, {3, 9}, {4, 16}, {5, 25},{6, 36}, {7, 49}, {8, 64}, {9, 81} };

                    // Get the number of dimensions.
                    //int rank = svpb.Rank;
                    //Console.WriteLine($"Number of dimensions: {rank}");
                    //       Number of dimensions: 2

                    //for (int ctr = 0; ctr < rank; ctr++)
                    //    Console.WriteLine($"   Dimension {ctr}: " +
                    //                      $"from {svpb.GetLowerBound(ctr)} to {svpb.GetUpperBound(ctr)}");
                    //          Dimension 0: from 0 to 7
                    //          Dimension 1: from 0 to 1

                    // Iterate the 2-dimensional array and display its values.
                    //Console.WriteLine("   Values of array elements:");
                    //          Values of array elements:

                    //for (int outer = svpb.GetLowerBound(0); outer <= svpb.GetUpperBound(0);
                    //     outer++)
                    //    for (int inner = svpb.GetLowerBound(1); inner <= svpb.GetUpperBound(1);
                    //         inner++)
                    //        Console.WriteLine($"      {'\u007b'}{outer}, {inner}{'\u007d'} = " +
                    //                          $"{svpb.GetValue(outer, inner)}");
                    //             {0, 0} = 2
                    //             {0, 1} = 4
                    //             {1, 0} = 3
                    //             {1, 1} = 9
                    //             {2, 0} = 4
                    //             {2, 1} = 16
                    //             {3, 0} = 5
                    //             {3, 1} = 25
                    //             {4, 0} = 6
                    //             {4, 1} = 36
                    //             {5, 0} = 7
                    //             {5, 1} = 49
                    //             {6, 0} = 8
                    //             {6, 1} = 64
                    //             {7, 0} = 9
                    //             {7, 1} = 81


                    //mvars.UUT.resW = 1920;
                    //mvars.UUT.resH = 1080;
                    //bxw = Convert.ToInt16(txt_tW.Text);
                    //bxh = Convert.ToInt16(txt_tH.Text);
                    //float svrx = Convert.ToSingle(Math.Round(pic_Front.Width / mvars.UUT.resW, 4, MidpointRounding.AwayFromZero));
                    //float svry = Convert.ToSingle(Math.Round(pic_Front.Height / mvars.UUT.resH, 4, MidpointRounding.AwayFromZero));
                    //lbl_Mark.Size = new Size(Convert.ToInt16(pic_Front.Width * bxw / mvars.UUT.resW), Convert.ToInt16(pic_Front.Height * bxh / mvars.UUT.resH));
                    //lbl_Mark.Location = pic_Front.Location;
                    //lbl_Mark.BackColor = Color.Red;
                    //lbl_Mark.Visible = true;





                    g3.Dispose();
                }
            }

            if (cmbPatSel.SelectedIndex == 1)
            {
                Pen p2;
                p2 = new Pen(Color.White, 1);
                int Xs = 16;
                int Ys = 8;
                int Xw = 24;
                int Yw = 27;
                int Xst = 0;
                int Yst = 0;
                int Fsz;

                int Xnd;
                int Ynd;

                


                Xs = Convert.ToInt16(txt_ShowCheckBoardW.Text);
                Ys = Convert.ToInt16(txt_ShowCheckBoardH.Text);

                Bitmap bmpf = new Bitmap(pic_Front.Width, pic_Front.Height);
                Graphics g3 = null;
                Font f3 = null;
                SolidBrush b3 = null;
                //preview
                Xw = Convert.ToInt16((pic_Front.Width / Xs));
                if ((Xw * Xs) != pic_Front.Width) Xw++;
                label31.Text = Xw.ToString();
                Yw = Convert.ToInt16((pic_Front.Height / Ys));
                if ((Yw * Ys) != pic_Front.Height) Yw++;
                label32.Text = Yw.ToString();

                if (Xw >= Yw) { Fsz = Convert.ToInt16(Yw / 6); }
                else { Fsz = Convert.ToInt16(Yw / 6); }
                if (Fsz < 6) Fsz = 6;

                f3 = new Font("Arial", Fsz);
                g3 = Graphics.FromImage(bmpf);
                g3.Clear(Color.Green);
                b3 = new SolidBrush(BackColor);
                for (int y = 0; y < Ys; y++)
                {
                    Ynd = Yst + Yw - 1;
                    for (int x = 0; x < Xs; x++)
                    {
                        Xnd = Xst + Xw - 1;
                        g3.DrawRectangle(p2, Xst, Yst, Xw - 1, Yw - 1);
                        if (Xw >= Yw) 
                        {
                            g3.DrawString("A1", f3, b3, Xst, Yst); 
                        }                       
                        else 
                        {
                            int sx = Xst + Convert.ToInt16((Xw - Fsz) / 3);
                            int sy = Yst + Convert.ToInt16((Yw - Fsz) / 3);
                            string svs = (x / 4 + 1 + ((y / 2) * 4)).ToString() + "-" + (x % 4 + 1 + (y % 2) * 4).ToString();
                            g3.DrawString(svs, f3, b3, sx, Yst + Convert.ToInt16((Yw - Fsz) / 3)); 
                        }
                        Xst = Xnd + 1;
                    }
                    Xst = 0;
                    Yst = Ynd + 1;
                }
                pic_Front.Image = bmpf;
                pic_Front.Refresh();

                if (chkshowextend.Checked) 
                {
                    Xst = 0;
                    Yst = 0;

                    Xw = Convert.ToInt16((mvars.UUT.resW / Xs));
                    if ((Xw * Xs) != mvars.UUT.resW) Xw++;
                    label31.Text = Xw.ToString();
                    Yw = Convert.ToInt16((mvars.UUT.resH / Ys));
                    if ((Yw * Ys) != mvars.UUT.resH) Yw++;
                    label32.Text = Yw.ToString();

                    if (Xw >= Yw) { Fsz = Convert.ToInt16(Yw / 6); }
                    else { Fsz = Convert.ToInt16(Yw / 6); }
                    if (Fsz < 6) Fsz = 6;

                    //f3 = new Font("Arial", Fsz);
                    bmpf = new Bitmap(i3pat.Width, i3pat.Height);
                    g3 = Graphics.FromImage(bmpf);
                    g3.Clear(Color.Green);
                    f3 = new Font("Arial", Fsz);         //測試用,需關閉
                    g3.Clear(Color.Black);              //測試用,需關閉
                    b3 = new SolidBrush(Color.Yellow);  //測試用,需關閉
                    for (int y = 0; y < Ys; y++)
                    {
                        Ynd = Yst + Yw - 1;
                        for (int x = 0; x < Xs; x++)
                        {
                            Xnd = Xst + Xw - 1;
                            g3.DrawRectangle(p2, Xst, Yst, Xw - 1, Yw - 1);
                            //if (Xw >= Yw) { g3.DrawString("A" + y, f3, b3, 0, Yst); }
                            if (Xw >= Yw) { g3.DrawString("A" + y, f3, b3, 60, Yst + 30); }//測試用,需關閉
                            else { g3.DrawString((x / 4 + 1 + ((y / 2) * 4)).ToString() + "-" + (x % 4 + 1 + (y % 2) * 4).ToString(), f3, b3, Xst + Convert.ToInt16((Xw - Fsz) / 3), Yst + Convert.ToInt16((Yw - Fsz) / 3)); }
                            Xst = Xnd + 1;
                        }
                        Xst = 0;
                        Yst = Ynd + 1;
                    }
                    i3pat.BackgroundImage = bmpf;
                    i3pat.Refresh();
                }

                bmpf.Dispose();
                f3.Dispose();
                b3.Dispose();
                g3.Dispose();
                p2.Dispose();
            }
            else if (cmbPatSel.SelectedIndex == 3)
            {
                pic_Front.SizeMode = PictureBoxSizeMode.StretchImage;
                Bitmap bmpf = new Bitmap(txt_bmpFileNameFull.Text);
                Graphics g3 = null;
                g3 = Graphics.FromImage(bmpf);
                pic_Front.Image = bmpf;
                pic_Front.Refresh();
                //
                if (chkshowextend.Checked) { i3pat.BackgroundImage = bmpf; }
                //
                g3.Dispose();
            }
            else if (cmbPatSel.SelectedIndex == 4)
            {
                Bitmap bmpf = new Bitmap(pic_Mark.Width, pic_Mark.Height);
                Graphics g3 = null;
                
                g3 = Graphics.FromImage(bmpf);
                g3.Clear(Color.White);


                LinearGradientBrush linGrBrush = new LinearGradientBrush(
                    new Point(0, pic_Mark.Height),
                    new Point(pic_Mark.Width, pic_Mark.Height),
                    Color.FromArgb(255, 0, 0, 0),   // Opaque red
                    Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                if (pvDir == 0)
                {
                    linGrBrush = new LinearGradientBrush(
                    new Point(pic_Mark.Width, 0),
                    new Point(pic_Mark.Width, pic_Mark.Height),
                    Color.FromArgb(255, 0, 0, 0),   // Opaque red
                    Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                }
                else if (pvDir == 1)
                {
                    linGrBrush = new LinearGradientBrush(
                    new Point(pic_Mark.Width, pic_Mark.Height),
                    new Point(pic_Mark.Width, 0),
                    Color.FromArgb(255, 0, 0, 0),   // Opaque red
                    Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                }
                else if (pvDir == 2)
                {
                    linGrBrush = new LinearGradientBrush(
                    new Point(0, pic_Mark.Height),
                    new Point(pic_Mark.Width, pic_Mark.Height),
                    Color.FromArgb(255, 0, 0, 0),   // Opaque red
                    Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                }
                else if (pvDir == 3)
                {
                    linGrBrush = new LinearGradientBrush(
                    new Point(pic_Mark.Width, pic_Mark.Height),
                    new Point(0, pic_Mark.Height),
                    Color.FromArgb(255, 0, 0, 0),   // Opaque red
                    Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                }




                g3.FillRectangle(linGrBrush, 0, 0, pic_Mark.Width, pic_Mark.Height);

                pic_Mark.Image = bmpf;
                pic_Mark.Refresh();


                if (chkshowextend.Checked)  
                {
                    bmpf = new Bitmap(i3pat.pic_Mark.Width, i3pat.pic_Mark.Height);
                    g3 = Graphics.FromImage(bmpf);
                    g3.Clear(Color.White);

                    //linGrBrush = new LinearGradientBrush(
                    //new Point(0, i3pat.pic_Mark.Height),
                    //new Point(i3pat.pic_Mark.Width, i3pat.pic_Mark.Height),
                    //Color.FromArgb(255, 0, 0, 0),   // Opaque red
                    //Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));



                    if (pvDir == 0)
                    {
                        linGrBrush = new LinearGradientBrush(
                        new Point(i3pat.pic_Mark.Width, 0),
                        new Point(i3pat.pic_Mark.Width, i3pat.pic_Mark.Height),
                        Color.FromArgb(255, 0, 0, 0),   // Opaque red
                        Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                    }
                    else if (pvDir == 1)
                    {
                        linGrBrush = new LinearGradientBrush(
                        new Point(i3pat.pic_Mark.Width, i3pat.pic_Mark.Height),
                        new Point(i3pat.pic_Mark.Width, 0),
                        Color.FromArgb(255, 0, 0, 0),   // Opaque red
                        Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                    }
                    else if (pvDir == 2)
                    {
                        linGrBrush = new LinearGradientBrush(
                        new Point(0, i3pat.pic_Mark.Height),
                        new Point(i3pat.pic_Mark.Width, i3pat.pic_Mark.Height),
                        Color.FromArgb(255, 0, 0, 0),   // Opaque red
                        Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                    }
                    else if (pvDir == 3)
                    {
                        linGrBrush = new LinearGradientBrush(
                        new Point(i3pat.pic_Mark.Width, i3pat.pic_Mark.Height),
                        new Point(0, i3pat.pic_Mark.Height),
                        Color.FromArgb(255, 0, 0, 0),   // Opaque red
                        Color.FromArgb(255, Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text)));
                    }



                    g3.FillRectangle(linGrBrush, 0, 0, i3pat.pic_Mark.Width, i3pat.pic_Mark.Height);

                    i3pat.pic_Mark.Image = bmpf;
                    i3pat.pic_Mark.Refresh();

                }
                g3.Dispose();

                if (pvDir < 3) { pvDir++; }
                else { pvDir = 0; }
            }
            
        }



        private void chk_inner_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Lock.Checked == true) { chk_Lock.Checked = false; }
            if (chk_inner.Checked) 
            {
                Color color = lbl_Mark.BackColor;
                ParseRGB(color);
                hScrollBar1.Value = color.R; hScrollBar2.Value = color.G; hScrollBar3.Value = color.B; //chk_inner.Visible = false;
            }
            else
            {
                Color color = pic_Front.BackColor;
                ParseRGB(color);
                hScrollBar1.Value = color.R; hScrollBar2.Value = color.G; hScrollBar3.Value = color.B; //chk_inner.Visible = false;
                //color = lbl_Mark.BackColor;
                //ParseRGB(color);
                //lbl_Mark.BackColor = Color.White;
                if (mvars.FormShow[5]) { i3pat.lbl_Mark.BackColor = lbl_Mark.BackColor; }
            }
        }

        uint ParseRGB(Color color)
        {
            return (uint)(((uint)color.B << 16) | (ushort)(((ushort)color.G << 8) | color.R));
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //Graphics g = pic_Front.CreateGraphics();
            //g.Clear(Color.White);
            //
            Pen p1;
            p1 = new Pen(Color.Black, 1);
            Pen p2;
            p2 = new Pen(Color.FromArgb(255, 255, 255), 1);
            int Xs = 16;
            int Ys = 8;
            int Xw = 24;
            int Yw = 27;


            Xs = Convert.ToInt16(txt_ShowCheckBoardW.Text);
            Ys = Convert.ToInt16(txt_ShowCheckBoardH.Text);

            Bitmap bmpf = new Bitmap(pic_Front.Width, pic_Front.Height);
            Graphics g3 = null;
            //Font f3 = null;
            //SolidBrush b3 = null;
            //preview

            Xw = Convert.ToInt16((pic_Front.Width));
            Yw = Convert.ToInt16((pic_Front.Height));

            g3 = Graphics.FromImage(bmpf);
            g3.Clear(Color.FromArgb(0, 0, 0));
            //for (int y = 0; y < Yw; y+=2)
            //{
            //    g3.DrawLine(p2, 0, y, Xw, y);
            //}

            for (int y = 0; y < Xw; y += 2)
            {
                g3.DrawLine(p2, y, 0, y, Yw);
            }

            pic_Front.Image = bmpf;
            pic_Front.Refresh();





            if (chkshowextend.Checked)
            {
                Xw = Convert.ToInt16((mvars.UUT.resW ));
                Yw = Convert.ToInt16((mvars.UUT.resH ));
                g3 = i3pat.CreateGraphics();
                g3.Clear(Color.FromArgb(0,0,0));

                for (int y = 0; y < Xw; y+=2)
                {
                    g3.DrawLine(p2, y, 0, y, Yw);
                }
            }
                
            g3.Dispose();
            p1.Dispose();


        }


        private void txt_bmpFileName_DoubleClick(object sender, EventArgs e)
        {
            using (openFileDialog1 = new OpenFileDialog())
            {
                if (cmbPatSel.Text == patsel[3])
                {
                    openFileDialog1.Title = "Open image file";
                    openFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|jpg files (*.jpg)|*.jpg|png files (*.png)|*.png";
                    openFileDialog1.RestoreDirectory = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        txt_bmpFileNameFull.Text = openFileDialog1.FileName;
                        string[] Svstr = txt_bmpFileNameFull.Text.Split('\\');
                        if (Svstr.Length > 0) { txt_bmpFileName.Text = Svstr[Svstr.Length - 1]; }
                    }
                }
                else if (cmbPatSel.Text == patsel[5])
                {
                    openFileDialog1.Title = "Open demura trace file";
                    openFileDialog1.Filter = "trace files (*.trace)|*.trace";
                    openFileDialog1.RestoreDirectory = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        txt_bmpFileNameFull.Text = openFileDialog1.FileName;
                        string[] Svstr = txt_bmpFileNameFull.Text.Split('\\');
                        if (Svstr.Length > 0) { txt_bmpFileName.Text = Svstr[Svstr.Length - 1]; }
                    }

                    btnShowCheckBoard_Click(sender, null);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lbl_Mark.Visible = true;
            int svi = 1;

            mvars.Break = false;
            Stopwatch sw = new Stopwatch();

            re1:
            if (svi == 1) { lbl_Mark.BackColor = Color.Red; }
            else if (svi == 2) { lbl_Mark.BackColor = Color.Green; }
            else if (svi == 3) { lbl_Mark.BackColor = Color.Blue; }
            else if (svi == 4) { lbl_Mark.BackColor = Color.White; }
            if (mvars.FormShow[5])
            {
                i3pat.lbl_Mark.Visible = true;
                i3pat.lbl_Mark.Location = new Point(mvars.TuningArea.mX, mvars.TuningArea.mY);
                i3pat.lbl_Mark.BackColor = lbl_Mark.BackColor;
            }
            Application.DoEvents();
            float tt = 0;
            re:
            sw.Start();
            int i = 0;
            do
            {
                i++;
            } while (i < 6);
            sw.Stop();
            tt += (sw.ElapsedTicks * 1000000F) / Stopwatch.Frequency;
            if (tt < 16655) { goto re; }
            svi++;
            if (svi > 4) svi = 1;
            if (mvars.Break) return;
            goto re1;
        }

        private void chk_showextend_CheckedChanged(object sender, EventArgs e)
        {
            if (chkshowextend.Checked == false) { lbl_EDID.Text = ""; }
        }
        private void chk_showextend_Click(object sender, EventArgs e)
        {
            lblexcorner.Text = "";
            if (chkshowextend.Checked == true)
            {
                int extX = 0;
                int extY = 0;
                mp.checkExtendScreen(ref extX, ref extY);
                lbl_EDID.Text = mvars.UUT.resW.ToString() + "X" + mvars.UUT.resH;
                mvars.TuningArea.rX = Convert.ToSingle(Math.Round((float)mvars.UUT.resW / pic_Front.Width, 4, MidpointRounding.AwayFromZero));
                mvars.TuningArea.rY = Convert.ToSingle(Math.Round((float)mvars.UUT.resH / pic_Front.Height, 4, MidpointRounding.AwayFromZero));
                if (mp.upperBound == 0)
                {
                    label22.Text = "Primary / EDID";
                    chkshowextend.Checked = false;    
                    if (mvars.FormShow[5] == false)
                    {
                        i3pat = new i3_Pat();
                        i3pat.BackColor = pic_Front.BackColor;
                        i3pat.Show();
                        i3pat.Visible = false;
                        i3pat.Location = new System.Drawing.Point(0, 0);
                        i3pat.FormBorderStyle = FormBorderStyle.Sizable;
                        i3pat.ControlBox = true;
                        i3pat.MinimizeBox = false;
                        i3pat.MaximizeBox = false;
                        i3pat.Size = new Size(200, 200);
                        i3pat.TopMost = true;
                        i3pat.Visible = true;
                        //上面需復原
                        //下面須關閉
                        //label22.Text = "Extend / EDID";
                        //i3pat = new i3_Pat();
                        //i3pat.lbl_Mark.Visible = false;
                        //i3pat.BackColor = pic_Front.BackColor;
                        //i3pat.Visible = false;
                        //i3pat.Location = new System.Drawing.Point(extX, extY);
                        //mp.doDelayms(200);
                        //i3pat.FormBorderStyle = FormBorderStyle.None;
                        //i3pat.ControlBox = false;
                        //i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                        //i3pat.TopMost = true;
                        //i3pat.Visible = true;
                        //i3pat.Show();
                    }
                    fileTuningArea(true);
                }
                else
                {
                    label22.Text = "Extend / EDID";
                    i3pat = new i3_Pat();
                    i3pat.lbl_Mark.Visible = false;
                    i3pat.BackColor = pic_Front.BackColor;
                    i3pat.Visible = false;
                    i3pat.Location = new System.Drawing.Point(extX, extY);
                    mp.doDelayms(200);
                    i3pat.FormBorderStyle = FormBorderStyle.None;
                    i3pat.ControlBox = false;
                    i3pat.Size = new System.Drawing.Size(Convert.ToInt16(mvars.UUT.resW), Convert.ToInt16(mvars.UUT.resH));
                    i3pat.TopMost = true;
                    i3pat.Visible = true;
                    i3pat.Show();
                }

                if (cmbPatSel.SelectedIndex == 0) { }
                else if (cmbPatSel.SelectedIndex == 1) { btnShowCheckBoard_Click(sender, null); }
                else if (cmbPatSel.SelectedIndex == 2)
                {
                    if (mvars.FormShow[5] && lbl_Mark.Visible == true)
                    {
                        i3pat.lbl_Mark.Visible = lbl_Mark.Visible;
                        i3pat.lbl_Mark.Location = new Point(mvars.TuningArea.mX, mvars.TuningArea.mY);
                        i3pat.lbl_Mark.Size = new Size(mvars.TuningArea.tW, mvars.TuningArea.tH);
                        i3pat.lbl_Mark.BackColor = lbl_Mark.BackColor;
                    }
                }
                else if (cmbPatSel.SelectedIndex == 4)
                {
                    if (mvars.FormShow[5])
                    {
                        i3pat.pic_Mark.Visible = true;
                        i3pat.pic_Mark.Location = new Point(mvars.TuningArea.mX, mvars.TuningArea.mY);
                        i3pat.lbl_Mark.Size = new Size(mvars.TuningArea.tW, mvars.TuningArea.tH);
                        i3pat.pic_Mark.BackColor = lbl_Mark.BackColor;
                    }
                }
                else if (cmbPatSel.SelectedIndex == 5) { btnShowCheckBoard_Click(sender, null); }


                lblexcorner.Text = extX + "," + extY + " " + mp.screens[mp.upperBound].DeviceName;
                if (mp.screens[mp.upperBound].Primary) { lblexcorner.Text += " Primary"; }
                i3pat.Text = extX + "," + extY;
            }
            else if (chkshowextend.Checked == false)
            {
                if (mp.upperBound != 0) { lbl_EDID.Text = ""; }
                if (mvars.FormShow[5])
                {
                    i3pat.Close();
                }
                fileTuningArea(true);
            }
        }

        private void lbl_exCorner_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mvars.TuningArea.mX = 0;
                mvars.TuningArea.mY = 0;
                lbl_Mark.Left = Convert.ToInt16(label41.Text);
                lbl_Mark.Top = Convert.ToInt16(label42.Text);
                txt_mX.Text = "0";
                txt_mY.Text = "0";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = Math.Round(Convert.ToSingle(textBox1.Text) / Convert.ToSingle(textBox2.Text), 4, MidpointRounding.AwayFromZero).ToString();
            textBox4.Text = Math.Round(Convert.ToSingle(textBox1.Text) / Convert.ToSingle(textBox3.Text), 0).ToString();    // Convert.ToInt16(mvars.TuningArea.tW / mvars.TuningArea.rX);
            textBox4.Text = Convert.ToInt16(1366 / Convert.ToSingle(textBox3.Text)).ToString();
            mp.cFLASHWRITE_C12ABMP_mk2(txt_bmpFileNameFull.Text);

            //int rank = svpbf.Rank;
            //svpbf = new PictureBox[2, 3];

            //for (int svh = 0; svh < 2; svh++)
            //{
            //    for (int svw = 0; svw < 3; svw++)
            //    {
            //        svpbf[svh, svw] = new System.Windows.Forms.PictureBox();
            //        svpbf[svh, svw].SetBounds(0,0,1,1);
            //        pic_Front.Controls.AddRange(new Control[] { svpbf[svh, svw] });
            //    }
            //}



            //for (int outer = svpbf.GetLowerBound(0); outer <= svpbf.GetUpperBound(0);
            //     outer++)
            //    for (int inner = svpbf.GetLowerBound(1); inner <= svpbf.GetUpperBound(1);
            //         inner++)
            //        if (pic_Front.Controls.Contains(svpbf[outer, inner])) { pic_Front.Controls.Remove(svpbf[outer, inner]); }


            // Create a two-dimensional integer array.
            //int[,] integers2d = { {2, 4}, {3, 9}, {4, 16}, {5, 25},{6, 36}, {7, 49}, {8, 64}, {9, 81} };

            // Get the number of dimensions.
            //int rank = svpbf.Rank;
            //Console.WriteLine($"Number of dimensions: {rank}");
            //       Number of dimensions: 2

            //for (int ctr = 0; ctr < rank; ctr++)
            //    Console.WriteLine($"   Dimension {ctr}: " +
            //                      $"from {svpb.GetLowerBound(ctr)} to {svpb.GetUpperBound(ctr)}");
            //          Dimension 0: from 0 to 7
            //          Dimension 1: from 0 to 1

            // Iterate the 2-dimensional array and display its values.
            //Console.WriteLine("   Values of array elements:");
            //          Values of array elements:

            //for (int outer = svpb.GetLowerBound(0); outer <= svpb.GetUpperBound(0);
            //     outer++)
            //    for (int inner = svpb.GetLowerBound(1); inner <= svpb.GetUpperBound(1);
            //         inner++)
            //        Console.WriteLine($"      {'\u007b'}{outer}, {inner}{'\u007d'} = " +
            //                          $"{svpb.GetValue(outer, inner)}");
            //             {0, 0} = 2
            //             {0, 1} = 4
            //             {1, 0} = 3
            //             {1, 1} = 9
            //             {2, 0} = 4
            //             {2, 1} = 16
            //             {3, 0} = 5
            //             {3, 1} = 25
            //             {4, 0} = 6
            //             {4, 1} = 36
            //             {5, 0} = 7
            //             {5, 1} = 49
            //             {6, 0} = 8
            //             {6, 1} = 64
            //             {7, 0} = 9
            //             {7, 1} = 81


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_Mark.Visible = !lbl_Mark.Visible;
        }

        private void txt_mX_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                lbl_Mark.Left = int.Parse(txt.Text);
            }
        }

        private void txt_mY_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                lbl_Mark.Top = int.Parse(txt.Text);
            }
        }

        private void txt_tW_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                lbl_Mark.Width = int.Parse(txt.Text);
            }
        }

        private void txt_tH_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                lbl_Mark.Height = int.Parse(txt.Text);
            }
        }

        private void txt_ms_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (mp.IsNumeric(txt_ms.Text) && int.Parse(txt_ms.Text) > 0)
                    timer1.Interval = int.Parse(txt_ms.Text);
        }

        private void pic_Front_MouseUp(object sender, MouseEventArgs e)
        {
            if (cmbPatSel.SelectedIndex == 6)
            {
                if (chk_inner.Checked)
                {
                    lbl_Mark.BackColor = Color.FromArgb(Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text));
                    if (chkshowextend.Checked == true) { i3pat.lbl_Mark.BackColor = lbl_Mark.BackColor; i3pat.lbl_Mark.Visible = true; }
                }
                else
                {
                    pic_Front.BackColor = Color.FromArgb(Convert.ToInt16(label17.Text), Convert.ToInt16(label18.Text), Convert.ToInt16(label19.Text));
                    if (chkshowextend.Checked == true) { i3pat.BackColor = pic_Front.BackColor; }
                }
            }
        }


        private void RT(object svobjF)
        {
            do
            {
                //System.Threading.Thread.Sleep(Convert.ToInt32(svobjF)); //每次接收不能太快，否則會資料遺失 
                mp.doDelayms(Convert.ToInt32(svobjF));
                if (lblMark.Visible == true) lblMark.Visible = false;
                else if (lblMark.Visible == false) lblMark.Visible = true;
            } while (mvars.Break == false) ;         //如果還沒接收完則繼續接收，設定Server端為Listen()
        }
    }
}
