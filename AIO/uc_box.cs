using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Management;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Linq;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;
using System.Reflection.Emit;

//using System.Runtime.InteropServices;
//using System.Security.Cryptography;
//using AIO.Properties;
//using System.CodeDom.Compiler;

namespace AIO
{
    public partial class uc_box : UserControl
    {
        
        int ciR = 18;       //圓半徑 ↑ ↓ 
        public static Button dgvBtn = null;
        static int svscrolH = 0;
        bool flghscPlus = false;
        int svAng;
        int svCnt;
        byte svHDMIs;
        byte svCurrentLastCol;
        string tempPath = System.IO.Path.GetTempPath();


        public uc_box()
        {
            
            InitializeComponent();
            
            dgv_box.ReadOnly = true;
            dgv_box.AllowUserToAddRows = false;
            dgv_box.AllowUserToResizeRows = false;
            dgv_box.AllowUserToResizeColumns = false;
            dgv_box.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv_box.ShowCellToolTips = false;
            dgvBtn = btn_dgv;
        }
        private void uc_box_Load(object sender, EventArgs e)
        {
            grp_dgvnewpos.Visible = !Form1.chkformsize.Checked;
            //grp_dgvnewpos.Visible = true;

            mvars.FormShow[11] = true;
            mvars.actFunc = "screenconfig";

            if (tempPath.Substring(tempPath.Length - 1, 1) != @"\")
            {
                string str = System.Environment.GetEnvironmentVariable("SystemRoot").Substring(0, 2) + @"\log";
                System.IO.Directory.CreateDirectory(str);
                tempPath = str + @"\";
            }
            System.IO.StreamWriter sw = System.IO.File.CreateText(tempPath + "tmp.txt");
            sw.WriteLine("");
            sw.Flush();
            sw.Close();
            if (System.IO.File.Exists(tempPath + "tmp.txt") == true)
            {
                pnl_dgv.Visible = true;

                System.IO.File.Delete(tempPath + "tmp.txt");
            }

            numUD_bxCs.Value = mvars.boxCols;
            numUD_bxRs.Value = mvars.boxRows;

            // mvars.regbookmark = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";  //Primary   ([0]~[22] = 23 bytes)
            mvars.regbookmark = "0";
            for (int i = 1; i < 23; i++) mvars.regbookmark += ",0";
            mvars.regbookmark = mp.replaceBoxMark(mvars.regbookmark, 21, mvars.in485.ToString());
            mvars.regbookmark = mp.replaceBoxMark(mvars.regbookmark, 22, mvars.inHDMI.ToString());
            lbl_newCv.Text = "0";
            lbl_newRv.Text = "0";
            Skeleton_nBox3(mvars.boxCols, mvars.boxRows);
            if (dgv_box.CurrentCell.ColumnIndex <= (dgv_box.Width / mvars.LBw - 1)) svCurrentLastCol = (byte)(dgv_box.Width / mvars.LBw - 1); lbl_colfrozen.Text = "CurrentLastCol " + svCurrentLastCol.ToString();
            #region  DataGridView 雙緩衝機制
            //Type dgvtype = dgv_box.GetType();
            //PropertyInfo pi = dgvtype.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //pi.SetValue(dgv_box, true, null);
            #endregion DataGridView 雙緩衝機制
            if (mvars.boxCols > 5 || (mvars.boxCols >= 4 && mvars.boxRows > 5))
            {
                h_dgvhscroll.Top = dgv_box.Top + dgv_box.Height;
                svscrolH = h_dgvhscroll.Height;
                h_dgvhscroll.Maximum = mvars.boxCols * mvars.LBw;
                lbl_dgvHscr.Size = new Size(h_dgvhscroll.Size.Width + 2, h_dgvhscroll.Size.Height + 2);
                lbl_dgvHscr.Location = new Point(h_dgvhscroll.Location.X - 1, h_dgvhscroll.Location.Y - 1);
                h_dgvhscroll.Visible = true;
                lbl_dgvHscr.Visible = true;
            }
            if (mvars.boxRows <= 5)
            {
                dgv_box.Width -= 17;
                lbl_dgvHscr.Width -= 17;
                h_dgvhscroll.Width -= 17;
            }
            pnl_dgv.Location = new Point(dgv_box.Location.X + 25, dgv_box.Location.Y + 20);
            pnl_dgv.Size = new Size(dgv_box.Width - 25, dgv_box.Height - 22);
            if (mvars.boxRows > 5) pnl_dgv.Width -= 19; else pnl_dgv.Width -= 2;


            if (Directory.Exists(mvars.strStartUpPath + @"\Parameter\ColorAdjustment"))
            {
                dirSearch(mvars.strStartUpPath + @"\Parameter\ColorAdjustment");
                if (cmb_box.Items.Count > 0)
                {
                    btn_save.Enabled = true;
                    #region tooltip
                    mvars.toolTip1.AutoPopDelay = 3000;
                    mvars.toolTip1.InitialDelay = 500;
                    mvars.toolTip1.ReshowDelay = 500;
                    // Force the ToolTip text to be displayed whether or not the form is active.
                    mvars.toolTip1.ShowAlways = true;
                    mvars.toolTip1.SetToolTip(btn_save, cmb_box.Items[0].ToString());
                    #endregion tooltip
                }
                else btn_save.Enabled = false;
            }
            svAng = 90;

            //0037 add
            if (MultiLanguage.DefaultLanguage == "en-US") label4.Text = "Frontal view";
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") label4.Text = "正面示意";
            else if (MultiLanguage.DefaultLanguage == "zh-CN") label4.Text = "正面示意";
            else if (MultiLanguage.DefaultLanguage == "ja-JP") label4.Text = "前面図";
            //label4.Left = dgv_box.Left - label4.Width - 10;
        }

        void dgv_box_RowPostPaint(object sender, PaintEventArgs e)
        {
            Rectangle r1;
            StringFormat strF = new StringFormat();

            byte svfirstR = (byte)dgv_box.FirstDisplayedCell.RowIndex;
            //byte svfirstC = (byte)dgv_box.FirstDisplayedCell.ColumnIndex;

            for (int i = 0; i < 5; i++)
            {
                if (svfirstR + i + 1 <= mvars.boxRows)
                {
                    r1 = dgv_box.GetCellDisplayRectangle(-1, svfirstR + i, true); //get the column header cell
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width -= 3;
                    r1.Height -= 3;
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), r1);
                    strF.Alignment = StringAlignment.Center;
                    strF.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString((svfirstR + i + 1).ToString("0"),
                    dgv_box.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(DefaultForeColor), r1, strF);
                }
            }

            strF.Dispose();

        }


        //public void pr_picturebox_refresh()
        //{
        //    Graphics l_gr_work = picturebox1.CreateGraphics();
        //    picturebox1.Refresh();
        //    Rectangle l_rt_work = new Rectangle(0, 0, picturebox1.Width, picturebox1.Height);
        //    PaintEventArgs l_pe_work = new PaintEventArgs(l_gr_work, l_rt_work);
        //    picturebox1_Paint_manual(picturebox1, l_pe_work);
        //}
        //private void picturebox1_Paint_manual(object sender, PaintEventArgs e)
        //{
        //    Rectangle l_rt_work = new Rectangle(10, 10, 50, 40);
        //    using (Pen pen = new Pen(Color.Red, 1))
        //    {
        //        e.Graphics.DrawRectangle(pen, l_rt_work);
        //    }
        //}







        //背面畫圖
        //有需要了解 Skeleton_nBox 請參考v0028版
        //有需要了解 Skeleton_nBox1 請參考1219_v0030
        //有需要了解 Skeleton_nBox2 請參考1219_v0030

        //HDMI下往上,485右下角進入
        void Skeleton_nBox3(int svCols, int svRows)
        {
            dgv_box.Visible = false;

            Graphics gra = dgv_box.CreateGraphics();
            Rectangle r1;
            int svR;
            int svC;

            dgv_box.AllowUserToResizeColumns = false;
            dgv_box.AllowUserToResizeRows = false;
            dgv_box.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv_box.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv_box.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_box.RowHeadersWidth = 25;
            dgv_box.RowHeadersDefaultCellStyle.Padding = new Padding(50);

            if (svCols > 5)
            {
                btn_send.Top += 16;
                btn_save.Top += 16;
                btn_single.Top += 16;
            }

            #region dgvNS_CornerPaint
            r1 = dgv_box.GetCellDisplayRectangle(-1, -1, true);
            r1.Y += 1;
            r1.Width--;
            r1.Height = r1.Height - 2;
            gra.FillRectangle(new SolidBrush(DefaultBackColor), r1);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            gra.DrawString("", new Font("Arial", 8, FontStyle.Bold), new SolidBrush(Color.White), r1, format);
            #endregion

            dgv_box.Columns.Clear();
            dgv_box.Rows.Clear();

            #region Column Create
            for (svC = 0; svC < svCols; svC++)
            {
                byte[] tmp = new byte[1];
                tmp[0] = (byte)(svC + 65);
                dgv_box.Columns.Add("Col" + svC.ToString(), System.Text.Encoding.ASCII.GetString(tmp));
                dgv_box.Columns[svC].Width = mvars.LBw;
                dgv_box.Columns[svC].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgv_box.ColumnHeadersHeight = 20;
            }
            //為了水平捲動完整顯示而多加了一欄
            dgv_box.Columns.Add("Col" + svC.ToString(), "");
            dgv_box.Columns[svC].Width = mvars.LBw;
            dgv_box.Columns[svC].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv_box.Columns[svC].Visible = false;
            dgv_box.ColumnHeadersHeight = 20;
            #endregion Column Create

            #region Row Create
            dgv_box.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8);
            dgv_box.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv_box.ShowCellToolTips = false;
            DataGridViewRowCollection rows = dgv_box.Rows;
            for (svR = 0; svR < svRows; svR++)
            {
                rows.Add();
                dgv_box.Rows[svR].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                DataGridViewRow row = dgv_box.Rows[svR];
                row.Height = mvars.LBh;
            }
            #endregion Row Create

            #region FontStyle
            for (svC = svCols - 1; svC >= 0; svC--)
            {
                for (svR = 0; svR < svRows; svR++)
                {
                    dgv_box.Rows[svR].Cells[svC].Style.Alignment = DataGridViewContentAlignment.TopRight;
                    dgv_box.Rows[svR].Cells[svC].Style.Font = new Font("Arial", 10);
                }
            }
            #endregion FontStyle

            #region 485走線
            Form1.nvsendercls_p(ref Form1.nvsender, 1, svCols * svRows);    //485共有多少單屏串接
            Form1.screenInfoList = new List<LEDScreenInfo>();               //必須否則會越來越多 Form1.screenInfoList
            LEDScreenInfo _screenInfoList = new LEDScreenInfo();
            List<PrimaryMapRegion> _primaryInfoList = new List<PrimaryMapRegion>();
            if (mvars.in485 <= 2)
            {
                #region 485 背面走線 X起點在右側
                for (svC = svCols - 1; svC >= 0; svC--)
                {
                    int svYbottom = (svRows - 1) * 540;
                    if ((svCols - 1 - svC) % 2 == 0)
                    {
                        for (svR = 0; svR < svRows; svR++)
                        {
                            r1 = dgv_box.GetCellDisplayRectangle(svC, svR, true);
                            r1.X += mvars.LBw * svC;
                            r1.Y += mvars.LBh * svR;

                            PrimaryMapRegion _sbMapRegion;
                            _sbMapRegion.Rec = 0;
                            _sbMapRegion.Sen = 0;
                            _sbMapRegion.Po = Convert.ToByte(svR + svRows * (svCols - 1 - svC) + 1);
                            _sbMapRegion.Ca = 2;
                            _sbMapRegion.W = mvars.scrW;
                            _sbMapRegion.H = mvars.scrH;
                            _sbMapRegion.X = _sbMapRegion.W * (svCols - 1 - svC);
                            if (mvars.in485 == 1)
                                _sbMapRegion.Y = _sbMapRegion.H * (svRows - 1 - svR);
                            else
                                _sbMapRegion.Y = _sbMapRegion.H * svR;
                            _sbMapRegion.Gap = _sbMapRegion.Y;
                            //_sbMapRegion.Gap = svR * 12969;
                            _primaryInfoList.Add(_sbMapRegion);

                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 4, (_sbMapRegion.W * svC).ToString());                 //背面走線 X起點在右側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 5, _sbMapRegion.Y.ToString());                         //Y起點在下側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 6, _sbMapRegion.W.ToString());
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 7, _sbMapRegion.H.ToString());
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 8, _sbMapRegion.X.ToString());                         //正面走線 X起點在左側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 9, Math.Abs(svYbottom - _sbMapRegion.Y).ToString());   //Y起點在上側用
                            Form1.nvsender[0].regPoCards++;
                        }
                    }
                    else
                    {
                        for (svR = 0; svR < svRows; svR++)
                        {
                            r1 = dgv_box.GetCellDisplayRectangle(svC, svR, true);
                            r1.X += mvars.LBw * svC;
                            r1.Y += mvars.LBh * svR;

                            PrimaryMapRegion _sbMapRegion;
                            _sbMapRegion.Rec = 0;
                            _sbMapRegion.Sen = 0;
                            _sbMapRegion.Po = Convert.ToByte((svCols - 1 - svC) * svRows + svRows - svR);
                            _sbMapRegion.Ca = 2;
                            _sbMapRegion.W = mvars.scrW;
                            _sbMapRegion.H = mvars.scrH;
                            _sbMapRegion.X = _sbMapRegion.W * (svCols - 1 - svC);
                            if (mvars.in485 == 1)
                                _sbMapRegion.Y = _sbMapRegion.H * (svRows - 1 - svR);
                            else
                                _sbMapRegion.Y = _sbMapRegion.H * svR;
                            _sbMapRegion.Gap = _sbMapRegion.Y;
                            //_sbMapRegion.Gap = svR * 12969;
                            _primaryInfoList.Add(_sbMapRegion);

                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 4, (_sbMapRegion.W * svC).ToString());         //背面走線 X起點在右側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 5, _sbMapRegion.Y.ToString());                 //Y起點在下側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 6, _sbMapRegion.W.ToString());
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 7, _sbMapRegion.H.ToString());
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 8, _sbMapRegion.X.ToString());                 //正面走線 X起點在左側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 9, (svYbottom - _sbMapRegion.Y).ToString());   //Y起點在上側用
                            Form1.nvsender[0].regPoCards++;
                        }
                    }
                }
                #endregion 485 背面走線 X起點在右側
            }
            else if (mvars.in485 > 2 && mvars.in485 <= 4)
            {
                #region 485 背面走線 X起點在左側
                for (svC = 0; svC < svCols; svC++)
                {
                    int svYbottom = (svRows - 1) * 540;
                    if (svC % 2 == 0)
                    {
                        for (svR = 0; svR < svRows; svR++)
                        {
                            r1 = dgv_box.GetCellDisplayRectangle(svC, svR, true);
                            r1.X += mvars.LBw * svC;
                            r1.Y += mvars.LBh * svR;

                            PrimaryMapRegion _sbMapRegion;
                            _sbMapRegion.Rec = 0;
                            _sbMapRegion.Sen = 0;
                            _sbMapRegion.Po = Convert.ToByte(svR + svRows * svC + 1);
                            _sbMapRegion.Ca = 2;
                            _sbMapRegion.W = mvars.scrW;
                            _sbMapRegion.H = mvars.scrH;
                            _sbMapRegion.X = _sbMapRegion.W * (svCols - 1 - svC);
                            if (mvars.in485 % 2 == 0)
                                _sbMapRegion.Y = _sbMapRegion.H * (svRows - 1 - svR);
                            else
                                _sbMapRegion.Y = _sbMapRegion.H * svR;
                            _sbMapRegion.Gap = _sbMapRegion.Y;
                            //_sbMapRegion.Gap = svR * 12969;
                            _primaryInfoList.Add(_sbMapRegion);

                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 4, (_sbMapRegion.W * svC).ToString());                 //背面走線 X起點在右側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 5, _sbMapRegion.Y.ToString());                         //Y起點在下側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 6, _sbMapRegion.W.ToString());
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 7, _sbMapRegion.H.ToString());
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 8, _sbMapRegion.X.ToString());                         //正面走線 X起點在左側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 9, Math.Abs(svYbottom - _sbMapRegion.Y).ToString());   //Y起點在上側用
                            Form1.nvsender[0].regPoCards++;
                        }
                    }
                    else
                    {
                        for (svR = 0; svR < svRows; svR++)
                        {
                            r1 = dgv_box.GetCellDisplayRectangle(svC, svR, true);
                            r1.X += mvars.LBw * svC;
                            r1.Y += mvars.LBh * svR;

                            PrimaryMapRegion _sbMapRegion;
                            _sbMapRegion.Rec = 0;
                            _sbMapRegion.Sen = 0;
                            _sbMapRegion.Po = Convert.ToByte(svC * svRows + svRows - svR);
                            _sbMapRegion.Ca = 2;
                            _sbMapRegion.W = mvars.scrW;
                            _sbMapRegion.H = mvars.scrH;
                            _sbMapRegion.X = _sbMapRegion.W * (svCols - 1 - svC);
                            if (mvars.in485 % 2 == 0)
                                _sbMapRegion.Y = _sbMapRegion.H * (svRows - 1 - svR);
                            else
                                _sbMapRegion.Y = _sbMapRegion.H * svR;
                            _sbMapRegion.Gap = _sbMapRegion.Y;
                            //_sbMapRegion.Gap = svR * 12969;
                            _primaryInfoList.Add(_sbMapRegion);

                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 4, (_sbMapRegion.W * svC).ToString());         //背面走線 X起點在右側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 5, _sbMapRegion.Y.ToString());                 //Y起點在下側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 6, _sbMapRegion.W.ToString());
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 7, _sbMapRegion.H.ToString());
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 8, _sbMapRegion.X.ToString());                 //正面走線 X起點在左側用
                            Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 9, (svYbottom - _sbMapRegion.Y).ToString());   //Y起點在上側用
                            Form1.nvsender[0].regPoCards++;
                        }
                    }
                }
                #endregion 485 背面走線 X起點在左側
            }
            else if (mvars.in485 == 5)
            {
                if (mvars.inHDMI == 5)
                {
                    #region 485 特殊走線
                    int svYbottom = (svRows - 1) * 540;
                    byte svRc = 1;
                    for (svR = svRows - 1; svR >= 0; svR--)
                    {
                        if ((svRows - 1 - svR) % 2 == 0)
                        {
                            for (svC = svCols - 1; svC >= 0; svC--)
                            {
                                r1 = dgv_box.GetCellDisplayRectangle(svC, svR, true);
                                r1.X += mvars.LBw * svC;
                                r1.Y += mvars.LBh * svR;
                                PrimaryMapRegion _sbMapRegion;
                                _sbMapRegion.Rec = 0;
                                _sbMapRegion.Sen = 0;
                                _sbMapRegion.Po = svRc;
                                _sbMapRegion.Ca = 2;
                                _sbMapRegion.W = mvars.scrW;
                                _sbMapRegion.H = mvars.scrH;
                                _sbMapRegion.X = _sbMapRegion.W * svC;
                                if (mvars.in485 == 5)
                                    _sbMapRegion.Y = _sbMapRegion.H * (svRows - 1 - svR);
                                else
                                    _sbMapRegion.Y = _sbMapRegion.H * svR;
                                _sbMapRegion.Gap = _sbMapRegion.Y;
                                //_sbMapRegion.Gap = svR * 12969;
                                _primaryInfoList.Add(_sbMapRegion);
                                svRc++;

                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 4, _sbMapRegion.X.ToString());                 //背面走線 X起點在右側用
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 5, _sbMapRegion.Y.ToString());                         //Y起點在下側用
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 6, _sbMapRegion.W.ToString());
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 7, _sbMapRegion.H.ToString());
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 8, Math.Abs((svCols - 1) * mvars.scrW - _sbMapRegion.X).ToString());                 //正面走線 X起點在左側用
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 9, Math.Abs(svYbottom - _sbMapRegion.Y).ToString());   //Y起點在上側用
                                Form1.nvsender[0].regPoCards++;
                            }
                        }
                        else
                        {
                            for (svC = svCols - 1; svC >= 0; svC--)
                            {
                                r1 = dgv_box.GetCellDisplayRectangle(svC, svR, true);
                                r1.X += mvars.LBw * svC;
                                r1.Y += mvars.LBh * svR;
                                PrimaryMapRegion _sbMapRegion;
                                _sbMapRegion.Rec = 0;
                                _sbMapRegion.Sen = 0;
                                _sbMapRegion.Po = svRc;
                                _sbMapRegion.Ca = 2;
                                _sbMapRegion.W = mvars.scrW;
                                _sbMapRegion.H = mvars.scrH;
                                _sbMapRegion.X = _sbMapRegion.W * svC;
                                if (mvars.in485 == 5)
                                    _sbMapRegion.Y = _sbMapRegion.H * (svRows - 1 - svR);
                                else
                                    _sbMapRegion.Y = _sbMapRegion.H * svR;
                                _sbMapRegion.Gap = _sbMapRegion.Y;
                                //_sbMapRegion.Gap = svR * 12969;
                                _primaryInfoList.Add(_sbMapRegion);
                                svRc++;

                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 4, _sbMapRegion.X.ToString());         //背面走線 X起點在右側用
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 5, _sbMapRegion.Y.ToString());                 //Y起點在下側用
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 6, _sbMapRegion.W.ToString());
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 7, _sbMapRegion.H.ToString());
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 8, Math.Abs((svCols - 1) * mvars.scrW - _sbMapRegion.X).ToString());                 //正面走線 X起點在左側用
                                Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[_sbMapRegion.Po - 1], 9, Math.Abs(svYbottom - _sbMapRegion.Y).ToString());   //Y起點在上側用
                                Form1.nvsender[0].regPoCards++;
                            }
                        }
                    }
                    #endregion 特殊走線
                }
            }
            _screenInfoList.primaryInfoList = _primaryInfoList;
            _screenInfoList.ScreenX = 0;
            _screenInfoList.ScreenY = 0;
            _screenInfoList.LEDDisplyType = AIO.LEDDisplyType.ComplexType;
            _screenInfoList.VirtualMode = ScreenVirtualMode.Disable;
            Form1.screenInfoList.Add((LEDScreenInfo)_screenInfoList);
            #endregion 485走線

            #region Drawing
            int svcR = 255;
            int svcG = 255;
            int svcB = 255;
            int svcLM = 500;
            svHDMIs = 1;
            string[] svmk;
            if (mvars.inHDMI == 5)
            {
                //0037 與dgv_box_draw互相運作
                if (mvars.in485 == 5)
                {
                    #region 特殊佈線1(背面走線485右上接入+右側HDMI接入(FHD反Z))
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    for (svR = 0; svR < svRows; svR++)
                    {
                        //svcR = 77 * (svR / 2 + 7) / 2;
                        //if (55 * (svR / 2 + 3) / 2 >= 0 && 55 * (svR / 2 + 3) / 2 <= 255)
                        //    svcG = 55 * (svR / 2 + 3) / 2;
                        //if (128 * (svR / 2) >= 0 && 128 * (svR / 2) <= 255)
                        //    svcB = 128 * (svR / 2);
                        //if (svcR >= 255) svcR = 224;
                        //if (svcG >= 255) svcG = 32;
                        //if (svcB >= 255) svcB = 64;
                        //if (svcR == 255 && svcG == 255 && svcB == 255) { svcR = 224; svcG = 224; svcB = 224; }

                        if (svHDMIs > 1)
                        {
                            svmk = Form1.nvsender[0].regBoxMark[Form1.nvsender[0].regPoCards - 1].Split(',');

                            if (svCols <= svRows)
                                svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                            else
                                svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                            if (svmk[15] == svcR.ToString()) svcR = Math.Abs(Convert.ToUInt16(svmk[15]) - 100);
                            if (svmk[16] == svcG.ToString()) svcG = Math.Abs(Convert.ToUInt16(svmk[16]) - 100);
                            if (svmk[17] == svcB.ToString()) svcB = Math.Abs(Convert.ToUInt16(svmk[17]) - 100);
                        }

                        if (svR % 2 == 0)
                        {
                            for (svC = svCols - 1; svC >= 0; svC--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svC = svCols - 1; svC >= 0; svC--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        //if (svRc != 1) svHDMIs++;     FHD特殊串接1,僅4個屏只有一串HDMI
                    }
                    #endregion 特殊佈線1(背面走線485右上接入+右側HDMI接入(FHD反Z))
                }
                else if (mvars.in485 == 1)
                {
                    #region 2x2FHD(背面走線485右下接入+HDMI右下接入)
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    for (svC = svCols - 1; svC >= 0; svC--)
                    {
                        if (svC % 2 == 1)
                        {
                            for (svR = svRows - 1; svR >= 0; svR--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svR = 0; svR < svRows; svR++)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                    }
                    #endregion 2x2FHD(背面走線485右下接入+HDMI右下接入)
                }
                else if (mvars.in485 == 2)
                {
                    //0037 新增
                    #region 2x2FHD(背面走線485右上接入+HDMI右上接入)
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    for (svC = svCols - 1; svC >= 0; svC--)
                    {
                        if (svC % 2 == 1)
                        {
                            for (svR = 0; svR < svRows; svR++)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svR = svRows - 1; svR >= 0; svR--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                    }
                    #endregion 2x2FHD(背面走線485右上接入+HDMI右上接入)
                }
                else if (mvars.in485 == 3)
                {
                    //0037 新增
                    #region 2x2FHD(背面走線485左上接入+HDMI左上接入)
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    //for (svC = svCols - 1; svC >= 0; svC--)
                    for (svC = 0; svC < svCols; svC++)
                    {
                        if (svC % 2 == 0)
                        {
                            for (svR = 0; svR < svRows; svR++)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svR = svRows - 1; svR >= 0; svR--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                    }
                    #endregion 2x2FHD(背面走線485左上接入+HDMI左上接入)
                }
                else if (mvars.in485 == 4)
                {
                    //0037 新增
                    #region 2x2FHD(背面走線485左下接入+HDMI左下接入)
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    //for (svC = svCols - 1; svC >= 0; svC--)
                    for (svC = 0; svC < svCols; svC++)
                    {
                        if (svC % 2 == 1)
                        {
                            for (svR = 0; svR < svRows; svR++)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svR = svRows - 1; svR >= 0; svR--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                    }
                    #endregion 2x2FHD(背面走線485左下接入+HDMI左下接入)
                }
            }
            else if (mvars.inHDMI == 6)
            {
                //0038
                if (mvars.in485 == 1)
                {
                    #region 2x2FHD(背面走線485右下接入+HDMI右下接入)
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    for (svC = svCols - 1; svC >= 0; svC--)
                    {
                        if (svC % 2 == 1)
                        {
                            for (svR = svRows - 1; svR >= 0; svR--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        //Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svR = 0; svR < svRows; svR++)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        //Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                    }
                    #endregion 2x2FHD(背面走線485右下接入+HDMI右下接入)
                }
                else if (mvars.in485 == 2)
                {
                    #region 2x2FHD(背面走線485右上接入+HDMI右上接入)
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    for (svC = svCols - 1; svC >= 0; svC--)
                    {
                        if (svC % 2 == 1)
                        {
                            for (svR = 0; svR < svRows; svR++)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        //Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svR = svRows - 1; svR >= 0; svR--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        //Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                    }
                    #endregion 2x2FHD(背面走線485右上接入+HDMI右上接入)
                }
                else if (mvars.in485 == 3)
                {
                    #region 2x2FHD(背面走線485左上接入+HDMI左上接入)
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    //for (svC = svCols - 1; svC >= 0; svC--)
                    for (svC = 0; svC < svCols; svC++)
                    {
                        if (svC % 2 == 0)
                        {
                            for (svR = 0; svR < svRows; svR++)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        //Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svR = svRows - 1; svR >= 0; svR--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        //Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                    }
                    #endregion 2x2FHD(背面走線485左上接入+HDMI左上接入)
                }
                else if (mvars.in485 == 4)
                {
                    #region 2x2FHD(背面走線485左下接入+HDMI左下接入)
                    int svRc = 1;
                    svcR = 255;
                    svcG = 128;
                    svcB = 128;
                    //for (svC = svCols - 1; svC >= 0; svC--)
                    for (svC = 0; svC < svCols; svC++)
                    {
                        if (svC % 2 == 1)
                        {
                            for (svR = 0; svR < svRows; svR++)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        //Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                        else
                        {
                            for (svR = svRows - 1; svR >= 0; svR--)
                            {
                                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                                {
                                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                                    if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                                    {
                                        //Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                        if (svRc == 1)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                        else if (svRc == 4)
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                        else
                                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                                    }
                                }
                                svRc++;
                                if (svRc > 4) { svRc = 1; svHDMIs++; }
                            }
                        }
                    }
                    #endregion 2x2FHD(背面走線485左下接入+HDMI左下接入)
                }
            }
            else if (mvars.inHDMI == 1)
            {
                #region HDMI由下往上
                //485背面走線 X右側接入，HDMI跟隨從右側搜尋起
                int svRc = 1;
                for (svC = svCols - 1; svC >= 0; svC--)
                {
                    svcR = 77 * (svC + 7) / 2;
                    if (55 * (svC + 3) / 2 >= 0 && 55 * (svC + 3) / 2 <= 255)
                        svcG = 55 * (svC + 3) / 2;
                    if (128 * (svC / 1) >= 0 && 128 * (svC / 1) <= 255)
                        svcB = 128 * (svC / 1);
                    if (svcR >= 255) svcR = 224;
                    if (svcG >= 255) svcG = 32;
                    if (svcB >= 255) svcB = 64;
                    if (svcR == 255 && svcG == 255 && svcB == 255) { svcR = 224; svcG = 224; svcB = 224; }

                    if (svHDMIs > 1)
                    {
                        svmk = Form1.nvsender[0].regBoxMark[Form1.nvsender[0].regPoCards - 1].Split(',');
                        if (svCols <= svRows)
                            svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                        else
                            svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                        if (svmk[15] == svcR.ToString()) svcR = Math.Abs(Convert.ToUInt16(svmk[15]) - 100);
                        if (svmk[16] == svcG.ToString()) svcG = Math.Abs(Convert.ToUInt16(svmk[16]) - 100);
                        if (svmk[17] == svcB.ToString()) svcB = Math.Abs(Convert.ToUInt16(svmk[17]) - 100);
                    }

                    if (svcR + svcG + svcB < svcLM)
                    {
                        int svmin = svcR;
                        if (svmin < svcG && svmin < svcB) svcR = (svcLM - svcR - svcG - svcB);
                        else if (svmin > svcG && svmin > svcB)
                        {
                            if (svcG > svcB) svcB = svcLM - svcR - svcG - svcB;
                            else svcG = svcLM - svcR - svcG - svcB;
                        }
                        else if (svmin < svcG && svmin > svcB) svcG = svcLM - svcR - svcG - svcB;
                        else if (svmin > svcG && svmin < svcB) svcB = svcLM - svcR - svcG - svcB;
                        if (svcR > 255) svcR = 255;
                        if (svcG > 255) svcG = 255;
                        if (svcB > 255) svcB = 255;
                    }

                    svRc = 1;
                    for (svR = svRows - 1; svR >= 0; svR--)
                    {
                        for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                        {
                            svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                            if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                if (svRc == 1)
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                else if (svRc == 4)
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                else
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                            }
                        }
                        svRc++;
                        if (svRc > 4)
                        {
                            svHDMIs++;
                            svRc = 1;
                        }
                    }
                    if (svRc != 1) svHDMIs++;
                }
                #endregion HDMI由下往上
            }
            else if (mvars.inHDMI == 3)
            {
                #region HDMI由上往下
                //485背面走線 X右側接入，HDMI跟隨從右側搜尋起
                int svRc = 1;
                for (svC = svCols - 1; svC >= 0; svC--)
                {
                    svcR = 77 * (svC + 7) / 2;
                    if (55 * (svC + 3) / 2 >= 0 && 55 * (svC + 3) / 2 <= 255)
                        svcG = 55 * (svC + 3) / 2;
                    if (128 * (svC / 1) >= 0 && 128 * (svC / 1) <= 255)
                        svcB = 128 * (svC / 1);
                    if (svcR >= 255) svcR = 224;
                    if (svcG >= 255) svcG = 32;
                    if (svcB >= 255) svcB = 64;
                    if (svcR == 255 && svcG == 255 && svcB == 255) { svcR = 224; svcG = 224; svcB = 224; }

                    if (svHDMIs > 1)
                    {
                        svmk = Form1.nvsender[0].regBoxMark[Form1.nvsender[0].regPoCards - 1].Split(',');
                        if (svCols <= svRows)
                            svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                        else
                            svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                        if (svmk[15] == svcR.ToString()) svcR = Math.Abs(Convert.ToUInt16(svmk[15]) - 100);
                        if (svmk[16] == svcG.ToString()) svcG = Math.Abs(Convert.ToUInt16(svmk[16]) - 100);
                        if (svmk[17] == svcB.ToString()) svcB = Math.Abs(Convert.ToUInt16(svmk[17]) - 100);
                    }

                    if (svcR + svcG + svcB < svcLM)
                    {
                        int svmin = svcR;
                        if (svmin < svcG && svmin < svcB) svcR = (svcLM - svcR - svcG - svcB);
                        else if (svmin > svcG && svmin > svcB)
                        {
                            if (svcG > svcB) svcB = svcLM - svcR - svcG - svcB;
                            else svcG = svcLM - svcR - svcG - svcB;
                        }
                        else if (svmin < svcG && svmin > svcB) svcG = svcLM - svcR - svcG - svcB;
                        else if (svmin > svcG && svmin < svcB) svcB = svcLM - svcR - svcG - svcB;
                        if (svcR > 255) svcR = 255;
                        if (svcG > 255) svcG = 255;
                        if (svcB > 255) svcB = 255;
                    }

                    svRc = 1;
                    for (svR = 0; svR < svRows; svR++)
                    {
                        for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                        {
                            svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                            if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                if (svRc == 1)
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                else if (svRc == 4)
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                else
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                            }
                        }
                        svRc++;
                        if (svRc > 4)
                        {
                            svHDMIs++;
                            svRc = 1;
                        }
                    }
                    if (svRc != 1) svHDMIs++;
                }
                #endregion HDMI由上往下
            }
            else if (mvars.inHDMI == 2)
            {
                #region HDMI由右往左(checked)
                //485背面走線 X右側接入，HDMI跟隨從右側搜尋起
                for (svR = svRows - 1; svR >= 0; svR--)
                {
                    svcR = 77 * (svR + 7) / 2;
                    if (55 * (svR + 3) / 2 >= 0 && 55 * (svR + 3) / 2 <= 255)
                        svcG = 55 * (svR + 3) / 2;
                    if (128 * (svR / 1) >= 0 && 128 * (svR / 1) <= 255)
                        svcB = 128 * (svR / 1);
                    if (svcR >= 255) svcR = 224;
                    if (svcG >= 255) svcG = 32;
                    if (svcB >= 255) svcB = 64;
                    if (svcR == 255 && svcG == 255 && svcB == 255) { svcR = 224; svcG = 224; svcB = 224; }

                    if (svHDMIs > 1)
                    {
                        svmk = Form1.nvsender[0].regBoxMark[Form1.nvsender[0].regPoCards - 1].Split(',');

                        if (svCols <= svRows)
                            svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                        else
                            svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                        if (svmk[15] == svcR.ToString()) svcR = Math.Abs(Convert.ToUInt16(svmk[15]) - 100);
                        if (svmk[16] == svcG.ToString()) svcG = Math.Abs(Convert.ToUInt16(svmk[16]) - 100);
                        if (svmk[17] == svcB.ToString()) svcB = Math.Abs(Convert.ToUInt16(svmk[17]) - 100);
                    }

                    if (svcR + svcG + svcB < svcLM)
                    {
                        int svmin = svcR;
                        if (svmin < svcG && svmin < svcB) svcR = (svcLM - svcR - svcG - svcB);
                        else if (svmin > svcG && svmin > svcB)
                        {
                            if (svcG > svcB) svcB = svcLM - svcR - svcG - svcB;
                            else svcG = svcLM - svcR - svcG - svcB;
                        }
                        else if (svmin < svcG && svmin > svcB) svcG = svcLM - svcR - svcG - svcB;
                        else if (svmin > svcG && svmin < svcB) svcB = svcLM - svcR - svcG - svcB;
                        if (svcR > 255) svcR = 255;
                        if (svcG > 255) svcG = 255;
                        if (svcB > 255) svcB = 255;
                    }

                    int svRc = 1;
                    for (svC = svCols - 1; svC >= 0; svC--)
                    {
                        for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                        {
                            svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                            if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                if (svRc == 1)
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                else if (svRc == 4)
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                else
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                            }
                        }
                        svRc++;
                        if (svRc > 4) { svRc = 1; svHDMIs++; }
                    }
                    if (svRc != 1) svHDMIs++;
                }
                #endregion HDMI由右往左
            }
            else if (mvars.inHDMI == 4)
            {
                #region HDMI由左往右
                //485背面走線 X右側接入，HDMI跟隨從右側搜尋起
                for (svR = svRows - 1; svR >= 0; svR--)
                {
                    svcR = 77 * (svR + 7) / 2;
                    if (55 * (svR + 3) / 2 >= 0 && 55 * (svR + 3) / 2 <= 255)
                        svcG = 55 * (svR + 3) / 2;
                    if (128 * (svR / 1) >= 0 && 128 * (svR / 1) <= 255)
                        svcB = 128 * (svR / 1);
                    if (svcR >= 255) svcR = 224;
                    if (svcG >= 255) svcG = 32;
                    if (svcB >= 255) svcB = 64;
                    if (svcR == 255 && svcG == 255 && svcB == 255) { svcR = 224; svcG = 224; svcB = 224; }

                    if (svHDMIs > 1)
                    {
                        svmk = Form1.nvsender[0].regBoxMark[Form1.nvsender[0].regPoCards - 1].Split(',');

                        if (svCols <= svRows)
                            svmk = Form1.nvsender[0].regBoxMark[svHDMIs - 1].Split(',');
                        else
                            svmk = Form1.nvsender[0].regBoxMark[(svHDMIs - 1)].Split(',');
                        if (svmk[15] == svcR.ToString()) svcR = Math.Abs(Convert.ToUInt16(svmk[15]) - 100);
                        if (svmk[16] == svcG.ToString()) svcG = Math.Abs(Convert.ToUInt16(svmk[16]) - 100);
                        if (svmk[17] == svcB.ToString()) svcB = Math.Abs(Convert.ToUInt16(svmk[17]) - 100);
                    }

                    if (svcR + svcG + svcB < svcLM)
                    {
                        int svmin = svcR;
                        if (svmin < svcG && svmin < svcB) svcR = (svcLM - svcR - svcG - svcB);
                        else if (svmin > svcG && svmin > svcB)
                        {
                            if (svcG > svcB) svcB = svcLM - svcR - svcG - svcB;
                            else svcG = svcLM - svcR - svcG - svcB;
                        }
                        else if (svmin < svcG && svmin > svcB) svcG = svcLM - svcR - svcG - svcB;
                        else if (svmin > svcG && svmin < svcB) svcB = svcLM - svcR - svcG - svcB;
                        if (svcR > 255) svcR = 255;
                        if (svcG > 255) svcG = 255;
                        if (svcB > 255) svcB = 255;
                    }

                    int svRc = 1;
                    for (svC = 0; svC < svCols; svC++)
                    {
                        for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                        {
                            svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                            if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == svC && Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == svR)
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, svHDMIs.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, svRc.ToString());
                                if (svRc == 1)
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                                else if (svRc == 4)
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                                else
                                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, svcR.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, svcG.ToString());
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, svcB.ToString());
                            }
                        }
                        svRc++;
                        if (svRc > 4) { svRc = 1; svHDMIs++; }
                    }
                    if (svRc != 1) svHDMIs++;
                }
                #endregion HDMI由左往右
            }

            #endregion Drawing without HDMI編號

            #region 各單屏紀錄解析度
            if (mvars.inHDMI == 6)  //[1][2][3][10][11][12][13][15][16][17]
            {
                for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                {
                    svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 10, "1920");
                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 11, "1080");
                    if (Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == 0 || Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == 2)
                    {
                        //svC
                        if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 1 || Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 3)
                        {
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, "1");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "1");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 12, "960");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 13, "0");
                        }
                        else if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 0 || Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 2)
                        {
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, "2");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 12, "0");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 13, "0");
                        }

                        if (Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == 0)
                        {
                            if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) < 2)
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, "1");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, "55");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, "72");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, "72");
                            }
                            else
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, "2");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, "155");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, "28");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, "28");
                            }
                        }
                        else
                        {
                            if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) < 2)
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, "3");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, "255");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, "128");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, "128");
                            }
                            else
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, "4");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, "124");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, "255");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, "128");
                            }
                        }
                    }
                    else if (Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == 1 || Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == 3)
                    {
                        //svC
                        if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 1 || Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 3)
                        {
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, "3");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "2");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 12, "960");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 13, "540");
                        }
                        else if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 0 || Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 2)
                        {
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 2, "4");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 3, "99");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 12, "0");
                            Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 13, "540");
                        }

                        if (Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]) == 1)
                        {
                            if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) < 2)
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, "1");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, "55");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, "72");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, "72");
                            }
                            else
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, "2");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, "155");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, "28");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, "28");
                            }
                        }
                        else
                        {
                            if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) < 2)
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, "3");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, "255");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, "128");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, "128");
                            }
                            else
                            {
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 1, "4");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 15, "124");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 16, "255");
                                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 17, "128");
                            }
                        }
                    }
                }


            }
            else
            {
                svHDMIs--;
                svmk = Form1.nvsender[0].regBoxMark[0].Split(',');
                int sveC = 1;
            reRes:
                int svXmin = 100000;
                int svXmax = 0;
                int svYmin = 100000;
                int svYmax = 0;
                for (int svi = 0; svi < Form1.nvsender[0].regPoCards; svi++)
                {
                    svmk = Form1.nvsender[0].regBoxMark[svi].Split(',');
                    if (Convert.ToUInt16(svmk[1]) == sveC)
                    {
                        svmk = Form1.nvsender[0].regBoxMark[svi].Split(',');
                        if (Convert.ToUInt16(svmk[4]) < svXmin) svXmin = Convert.ToUInt16(svmk[4]);
                        if (Convert.ToUInt16(svmk[4]) > svXmax) svXmax = Convert.ToUInt16(svmk[4]);
                        if (Convert.ToUInt16(svmk[5]) < svYmin) svYmin = Convert.ToUInt16(svmk[5]);
                        if (Convert.ToUInt16(svmk[5]) > svYmax) svYmax = Convert.ToUInt16(svmk[5]);
                    }
                }
                for (int svi = 0; svi < Form1.nvsender[0].regPoCards; svi++)
                {
                    svmk = Form1.nvsender[0].regBoxMark[svi].Split(',');
                    if (Convert.ToUInt16(svmk[1]) == sveC)
                    {
                        Form1.nvsender[0].regBoxMark[svi] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[svi], 10, (svXmax - svXmin + Convert.ToUInt16(svmk[6])).ToString());
                        Form1.nvsender[0].regBoxMark[svi] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[svi], 11, (svYmax - svYmin + Convert.ToUInt16(svmk[7])).ToString());
                        Form1.nvsender[0].regBoxMark[svi] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[svi], 12, (svXmax - Convert.ToUInt16(svmk[4])).ToString());
                        Form1.nvsender[0].regBoxMark[svi] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[svi], 13, svmk[5]);
                        //v0037 modify
                        //Form1.nvsender[0].regBoxMark[svi] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[svi], 13, (svYmax - Convert.ToUInt16(svmk[5])).ToString());
                    }
                }
                if (sveC < svHDMIs) { sveC++; goto reRes; }

                for (byte svhdmi = 1; svhdmi <= svHDMIs; svhdmi++)
                {
                    sveC = 0;
                    byte svlast = 0;
                    for (byte i = 0; i < Form1.nvsender[0].regPoCards; i++)
                    {
                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 0, svHDMIs.ToString());
                        svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                        if (svmk[1] == svhdmi.ToString() && svmk[2] == (sveC + 1).ToString())
                        {
                            sveC++;
                            svlast = i;
                        }
                    }
                    for (int i = Form1.nvsender[0].regPoCards - 1; i >= 0; i--)
                    {
                        svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                        if (svmk[1] == svhdmi.ToString() && svmk[2] == (sveC + 1).ToString())
                        {
                            sveC++;
                            svlast = (byte)i;
                        }
                    }
                    if (sveC > 1 && sveC <= 4) Form1.nvsender[0].regBoxMark[svlast] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[svlast], 3, "99");
                    else if (sveC == 1) Form1.nvsender[0].regBoxMark[svlast] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[svlast], 3, "100");
                }
            }
        #endregion 各單屏紀錄解析度


        ex:
            #region dgvNS_RowPostPaint
            dgv_box.Paint += new PaintEventHandler(dgv_box_RowPostPaint);
            #endregion

            // 最後一欄的遮罩用Label lbl_col
            lbl_col.Visible = false;
            r1 = dgv_box.GetCellDisplayRectangle(svCols, -1, true);
            lbl_col.Location = new Point(dgv_box.Left + r1.X, dgv_box.Top + r1.Y);
            lbl_col.Size = new Size(r1.Width, dgv_box.Height - svscrolH - 2);
            lbl_col.Visible = true;
            lbl_col.BackColor = dgv_box.BackgroundColor;
            lbl_col.Text = "";

            btn_dgv.Location = new Point(dgv_box.Left + 2, dgv_box.Top + 2);
            btn_dgv.Height = dgv_box.ColumnHeadersHeight - 1;
            btn_dgv.Width = dgv_box.RowHeadersWidth - 1;
            if (svRows <= 4) h_dgvhscroll.SetBounds(dgv_box.Left + 1, dgv_box.Top + dgv_box.Height - 18, dgv_box.Width - 2, 17);
            else h_dgvhscroll.SetBounds(dgv_box.Left + 1, dgv_box.Top + dgv_box.Height - 18, dgv_box.Width - 19, 17);

            dgv_box.GridColor = dgv_box.BackgroundColor;
            dgv_box.ClearSelection();
            if (h_dgvhscroll.Visible == true)
            {
                h_dgvhscroll.Value = h_dgvhscroll.Maximum;
                dgv_box.FirstDisplayedScrollingColumnIndex = dgv_box.Columns.Count - 1 - 4;
            }
            dgv_box.Visible = true;

            pnl_dgv.Paint += new PaintEventHandler(pnl_dgv_Paint);
            dgv_box_draw(pnl_dgv, pnl_dgv.CreateGraphics());
        }


        private void pnl_dgv_Paint(object sender, PaintEventArgs e)
        {
            if (mvars.FormShow[6] != true)
                dgv_box_draw(pnl_dgv, e.Graphics);
        }

        void dgv_box_draw(Panel panel, Graphics gra1)
        {
            Pen p4 = new Pen(Color.FromArgb(208, 64, 255, 0), 3);
            Pen pH = new Pen(Color.FromArgb(128, 128, 255), 3);
            Font fnt = new Font("Arial", 7, FontStyle.Regular);
            StringFormat strF = new StringFormat();
            strF.Alignment = StringAlignment.Center;
            SolidBrush strSB = new SolidBrush(Color.FromArgb(128, 0, 0));
            SolidBrush sb4 = new SolidBrush(Color.FromArgb(255, 255, 224));

            int svOTcXh;
            int svOTcX4;
            int svINcX4;
            int svINcXa;
            int svINcXh;
            int svcY;
            string[] svmk;
            string[] svmkf;
            byte svcol;
            byte svrow;
            int svcolf;
            int svrowf;
            byte svmcoW = (byte)(ciR * 14 / 10 - 1);                //hdmi的圓直徑 18*14/10-1==24
            byte svmcoH = (byte)(ciR * 2);                          //hdmi的圓直徑 18*2==36
            byte svhdmiR = (byte)Math.Round(ciR * 83d / 100, 0);    //hdmi的圓直徑 18*83/100==15
            byte svfirstR = (byte)dgv_box.FirstDisplayedCell.RowIndex;
            byte svfirstC = (byte)dgv_box.FirstDisplayedCell.ColumnIndex;
            if (flghscPlus) svfirstC = (byte)(h_dgvhscroll.Value / mvars.LBw);

            Rectangle r1 = dgv_box.GetCellDisplayRectangle(-1, -1, true);
            if (mvars.boxCols > 5) svcol = 5; else svcol = (byte)mvars.boxCols;
            if (mvars.boxRows > 5) svrow = 5; else svrow = (byte)mvars.boxRows;

            // Fred 2025/01/22 威創客製需求更改 >> 新增正面圖 並保留背面圖供客人切換
            System.Drawing.Bitmap draw = new System.Drawing.Bitmap(170 * mvars.boxCols + 2, 96 * mvars.boxRows + 2);
            gra1 = Graphics.FromImage(draw);
            //----------------------------------------------------------------------
            //mvars.actFunc = mvars.actFunc;
            label3.Text = "1st C" + svfirstC.ToString() + "," + "R" + svfirstR.ToString();
            ciR = 18;
            for (int svid = 0; svid < Form1.nvsender[0].regPoCards; svid++)
            {
                svmk = Form1.nvsender[0].regBoxMark[svid].Split(',');
                svcol = (byte)(Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]));
                svrow = (byte)(Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]));

                if (svcol >= svfirstC && svrow >= svfirstR)//&& svcol < svfirstC + 5 && svrow < svfirstR + 5
                {
                    r1 = dgv_box.GetCellDisplayRectangle(svcol, svrow, false);
                    
                    r1.X -= 25;
                    r1.Y -= 20;


                    gra1.FillRectangle(new SolidBrush(Color.FromArgb(Convert.ToUInt16(svmk[15]), Convert.ToUInt16(svmk[16]), Convert.ToUInt16(svmk[17]))), r1.X, r1.Y, r1.Width, r1.Height);
                    gra1.DrawRectangle(Pens.Black, r1.X, r1.Y, r1.Width, r1.Height);
                    p4 = new Pen(sb4.Color, 2);
                    svOTcXh = r1.X + svhdmiR + 5;                       //圓的圓心X 15
                    svcY = r1.Y + r1.Height * 35 / 100 + svhdmiR;       //圓的圓心Y 96*35/100+15==47

                    if (Convert.ToUInt16(svmk[3]) < 99)
                        gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 128, 255)), svOTcXh - svhdmiR, svcY - svhdmiR, svhdmiR * 2, svhdmiR * 2);
                    gra1.DrawEllipse(new Pen(Color.LightGreen), svOTcXh - svhdmiR, svcY - svhdmiR, svhdmiR * 2, svhdmiR * 2);
                    if (isback == true)
                    {
                        gra1.DrawString("HDMI" + "\r\n" + "OUT", fnt, new SolidBrush(Color.White), svOTcXh, svcY - svhdmiR + 5, strF);
                    }
                        

                    svOTcX4 = svOTcXh + svhdmiR * 2 + 2;
                    gra1.FillRectangle(new SolidBrush(sb4.Color), svOTcX4 - svmcoW / 2, svcY - svmcoH / 2, svmcoW, svmcoH);

                    if (isback == true)
                    {
                        gra1.DrawString("micro" + "\r\n" + "USB", fnt, strSB, svOTcX4, svcY - svmcoH / 2, strF);
                        gra1.DrawString("OUT", new Font("Arial", 8, FontStyle.Bold), strSB, svOTcX4 + 1, svcY + 2, strF);
                    }
                        

                    svINcX4 = svOTcXh + svhdmiR * 4 + 2;
                    gra1.FillRectangle(sb4, svINcX4 - svmcoW / 2, svcY - svmcoH / 2, svmcoW, svmcoH);
                    if (isback == true)
                    {
                        gra1.DrawString("micro" + "\r\n" + "USB", fnt, strSB, svINcX4, svcY - svmcoH / 2, strF);
                        gra1.DrawString("IN", new Font("Arial", 8, FontStyle.Bold), strSB, svINcX4 + 1, svcY + 2, strF);
                    }
                        

                    svINcXa = svOTcXh + svhdmiR * 6 + 1;
                    if (svid == 0)
                    {
                        gra1.DrawLine(p4, svINcXa + 2, svcY - svmcoH / 2, svINcXa + 2, svcY + 15);
                        gra1.FillRectangle(sb4, svINcXa - svmcoW / 2, svcY - svmcoH / 2, svmcoW + 5, svmcoH / 3 + 1);
                       
                        gra1.FillRectangle(new SolidBrush(Color.FromArgb(255, 240, 255)), svINcXa - svmcoW / 2, svcY + 12, svmcoW + 5, svmcoH / 2);
                        

                        if (isback == true)
                        {
                            gra1.DrawString("TypeA", fnt, strSB, svINcXa + 1, svcY - svmcoH / 2, strF);
                            gra1.DrawString("PC", new Font("Arial", 10, FontStyle.Bold), strSB, svINcXa + 4, svcY + 13, strF);
                        }
                    }
                    else
                    {
                        gra1.DrawRectangle(new Pen(sb4.Color), svINcXa - svmcoW / 2, svcY - svmcoH / 2, svmcoW + 5, svmcoH / 3);
                        if (isback == true)
                        {
                            gra1.DrawString("TypeA", fnt, strSB, svINcXa + 1, svcY - svmcoH / 2, strF);
                        }
                            
                    }

                    svINcXh = svOTcXh + svhdmiR * 8 + 8;
                    if (svmk[3].Substring(0, 1) == "1")
                    {
                        gra1.DrawEllipse(new Pen(Color.LightGreen, 3), svINcXh - svhdmiR + 1, svcY - svhdmiR - 3, svhdmiR * 2, svhdmiR * 2);
                        gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 128, 255)), svINcXh - svhdmiR, svcY - svhdmiR - 2, svhdmiR * 2 + 2, svhdmiR * 2 + 2);
                    }
                    if (svmk[3].Substring(0, 1) != "1")
                    {
                        gra1.FillEllipse(new SolidBrush(Color.FromArgb(128, 128, 255)), svINcXh - svhdmiR, svcY - svhdmiR, svhdmiR * 2, svhdmiR * 2);
                        gra1.DrawEllipse(new Pen(Color.LightGreen), svINcXh - svhdmiR, svcY - svhdmiR, svhdmiR * 2, svhdmiR * 2);
                    }

                    if (Convert.ToUInt16(svmk[15]) < 16 && Convert.ToUInt16(svmk[16]) < 16 && Convert.ToUInt16(svmk[17]) < 16)
                    {
                        svmk[15] = (255 - Convert.ToUInt16(svmk[15])).ToString();
                        svmk[16] = (255 - Convert.ToUInt16(svmk[16])).ToString();
                        svmk[17] = (255 - Convert.ToUInt16(svmk[17])).ToString();
                    }
                    else
                    {
                        if (Convert.ToUInt16(svmk[15]) < 128) svmk[15] = "40"; else svmk[15] = strSB.Color.R.ToString();
                        if (Convert.ToUInt16(svmk[16]) < 128) svmk[16] = "40"; else svmk[16] = strSB.Color.G.ToString();
                        if (Convert.ToUInt16(svmk[17]) < 128) svmk[17] = "40"; else svmk[17] = strSB.Color.B.ToString();
                    }

                    gra1.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), svINcXh - 15, r1.Y + 4, 35, 16);
                    if (isback == true)
                    {
                        gra1.DrawString("ID. " + (svid + 1).ToString(), new Font("Arial", 9, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 255)), svINcXh + 2, r1.Y + 4, strF);
                    }
                        

                    if (svmk[3].Substring(0, 1) == "1")
                    {
                        if (isback == true)
                        {
                            gra1.DrawString("HDMI" + "\r\n" + "IN .", new Font("Arial", 8, FontStyle.Regular),
                            new SolidBrush(Color.White), svINcXh + 2, svcY - 12, strF);
                            gra1.DrawString(svmk[1], new Font("Arial", 12, FontStyle.Bold),
                                new SolidBrush(Color.White),
                                svINcXh + 10, svcY + 3, strF);
                        }
                            
                    }
                    else
                    {
                        if (isback == true)
                        {
                            gra1.DrawString("HDMI" + "\r\n" + "IN", fnt, new SolidBrush(Color.White), svINcXh, svcY - svhdmiR + 5, strF);
                        }
                    }
                        

                    if (Form1.nvsender[0].regPoCards > 1)
                    {
                        //0037 與Skeleton_nBox3互相運作
                        if (mvars.inHDMI == 5)
                        {
                            if (mvars.in485 == 5)
                            {
                                #region 特殊佈線1(背面走線485右上接入+右側HDMI接入(FHD反Z))
                                svmkf = Form1.nvsender[0].regBoxMark[svid].Split(',');
                                if (svmk[3].Substring(0, 1) == "1")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版
                                    //HDMI
                                    gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 9);      //下
                                    gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X, svcY + svhdmiR + 9);    //out左橫
                                                                                                                 //485
                                    gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svcY + ciR + 12);       //下
                                    gra1.DrawLine(p4, svOTcX4, svcY + ciR + 12, r1.X, svcY + ciR + 12);    //out左橫
                                }
                                else if (svmk[3] == "2")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版
                                    if (svid == 1)
                                    {
                                        //與上一點銜接
                                        //HDMI in
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svcY + svhdmiR + 9);  //下
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR + 9, r1.X + r1.Width, svcY + svhdmiR + 9);   //in右橫
                                                                                                                               //485 in
                                        gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svcY + 30);   //下
                                        gra1.DrawLine(p4, svINcX4, svcY + 30, r1.X + r1.Width, svcY + 30);       //in右橫

                                        //到下一點
                                        //HDMI out
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 9);              //下
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X + r1.Width, r1.Y + r1.Height);   //左上out-右下角落
                                                                                                                             //485 out
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svcY + ciR + 25);   //下
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR + 25, r1.X + r1.Width, r1.Y + r1.Height);      //左上out-右下角落
                                    }
                                    else
                                    {
                                        //與上一點銜接
                                        //HDMI in
                                        gra1.DrawLine(pH, svINcXh, svcY - svhdmiR, svINcXh, svcY - svhdmiR - 6);  //上
                                        gra1.DrawLine(pH, svINcXh, svcY - svhdmiR - 6, r1.X, r1.Y);   //左上角落-右下in
                                                                                                      //485 in
                                        gra1.DrawLine(p4, svINcX4, svcY - ciR, svINcX4, svcY - ciR - 25);   //上
                                        gra1.DrawLine(p4, svINcX4, svcY - ciR - 25, r1.X, r1.Y);      //左上角落-右下in

                                        //到下一點
                                        //HDMI out
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 9);  //下
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X, svcY + svhdmiR + 9);  //下
                                                                                                                   //485 out
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svcY + ciR + 12);   //下
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR + 12, r1.X, svcY + ciR + 12);   //下
                                    }
                                }
                                else if (svmk[3] == "99")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版

                                    //與上一點銜接
                                    //HDMI
                                    gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svcY + svhdmiR + 9);  //下
                                    gra1.DrawLine(pH, svINcXh, svcY + svhdmiR + 9, r1.X + r1.Width, svcY + svhdmiR + 9);  //下
                                                                                                                          //485
                                    gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svcY + ciR + 12);   //下
                                    gra1.DrawLine(p4, svINcX4, svcY + ciR + 12, r1.X + r1.Width, svcY + ciR + 12);   //下
                                }
                                #endregion 特殊佈線1(背面走線485右上接入+右側HDMI接入(FHD反Z))
                            }
                            else if (mvars.in485 == 1)
                            {
                                #region 2x2FHD(背面走線485右下接入+HDMI右下接入)
                                int svdd;
                                svmkf = Form1.nvsender[0].regBoxMark[svid].Split(',');
                                if (svmk[3].Substring(0, 1) == "1")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版
                                    //HDMI
                                    gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR, svOTcXh, r1.Y);

                                    //485
                                    svdd = svcY - ciR - 20;
                                    gra1.DrawLine(p4, svOTcX4, svcY - ciR, svOTcX4, svdd);
                                    gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                    gra1.DrawLine(p4, svINcX4, svdd, svINcX4, r1.Y);

                                }
                                else if (svmk[3] == "2")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版
                                    if (svid == 1)
                                    {
                                        svdd = svcY + svhdmiR + 27;
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, svINcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, svOTcXh, r1.Y + r1.Height);
                                        svdd = svcY + svhdmiR + 8;
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, r1.X, svdd);


                                        gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);
                                        svdd = svcY + ciR + 15;
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svdd);
                                        gra1.DrawLine(p4, svOTcX4, svdd, r1.X, svdd);
                                    }
                                    else
                                    {
                                        svdd = svcY + svhdmiR + 8;
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, r1.X + r1.Width, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 9);
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X + r1.Width / 2, r1.Y + r1.Height);

                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, r1.Y + r1.Height);
                                        svdd = svcY + ciR + 15;
                                        gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svdd);
                                        gra1.DrawLine(p4, svINcX4, svdd, r1.X + r1.Width, svdd);
                                    }


                                }
                                else if (svmk[3] == "99")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版
                                    gra1.DrawLine(pH, svINcXh, svcY - svhdmiR, svINcXh, svcY - svhdmiR - 9);              //上
                                    gra1.DrawLine(pH, svINcXh, svcY - svhdmiR - 9, r1.X + r1.Width / 2, r1.Y);
                                    //485
                                    svdd = svcY - ciR - 20;
                                    gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, r1.Y);
                                    gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                    gra1.DrawLine(p4, svINcX4, svcY - ciR, svINcX4, svdd);
                                }
                                #endregion 2x2FHD(背面走線485右下接入+HDMI右下接入)
                            }
                            else if (mvars.in485 == 2)
                            {
                                //0037 新增(先Skeleton_nBox3計算座標)
                                #region 2x2FHD(背面走線485右上接入+HDMI右上接入)
                                int svdd;
                                svmkf = Form1.nvsender[0].regBoxMark[svid].Split(',');
                                if (svmk[3].Substring(0, 1) == "1")
                                {
                                    //HDMI
                                    gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, r1.Y + r1.Height);  //往下

                                    //485
                                    //svdd = svcY - ciR - 20;
                                    //gra1.DrawLine(p4, svOTcX4, svcY - ciR, svOTcX4, svdd);
                                    //gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                    //gra1.DrawLine(p4, svINcX4, svdd, svINcX4, r1.Y);

                                    gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, r1.Y + r1.Height);  //往下

                                }
                                else if (svmk[3] == "2")
                                {
                                    if (svid == 1)
                                    {
                                        //svdd = svcY + svhdmiR + 27;
                                        //gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        //gra1.DrawLine(pH, svOTcXh, svdd, svINcXh, svdd);
                                        //gra1.DrawLine(pH, svOTcXh, svdd, svOTcXh, r1.Y + r1.Height);
                                        //svdd = svcY + svhdmiR + 8;
                                        //gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                        //gra1.DrawLine(pH, svOTcXh, svdd, r1.X, svdd);

                                        //gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);
                                        //svdd = svcY + ciR + 15;
                                        //gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svdd);
                                        //gra1.DrawLine(p4, svOTcX4, svdd, r1.X, svdd);

                                        //485 
                                        svdd = svcY - ciR - 10;
                                        gra1.DrawLine(p4, svOTcX4, r1.Y, svOTcX4, svdd);        //上面"1"的 OUT 往下畫到到"2"的 IN
                                        gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);        //橫線
                                        gra1.DrawLine(p4, svINcX4, svdd, svINcX4, svcY - ciR);  //橫線右側往下到 IN

                                        svdd = svcY + ciR + 15;
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svdd);
                                        gra1.DrawLine(p4, svOTcX4, svdd, r1.X, svdd);
                                        //HDMI 左上斜右下
                                        svdd = svcY + svhdmiR + 20;
                                        gra1.DrawLine(pH, svOTcXh, r1.Y, svINcXh, svcY - svhdmiR - 7);
                                        gra1.DrawLine(pH, svINcXh, svcY - svhdmiR - 7, svINcXh, svcY - svhdmiR);

                                        svdd = svcY + svhdmiR + 8;
                                        //gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        //gra1.DrawLine(pH, svINcXh, svdd, r1.X, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 9);
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X, svcY + svhdmiR + 9);

                                    }
                                    else
                                    {
                                        //485 
                                        svdd = svcY + ciR + 15;
                                        gra1.DrawLine(p4, svINcX4, svdd, r1.X + r1.Width, svdd);    //上一點的OUT進入自己的IN前置橫線
                                        gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svdd);      //到自己IN的往上短縱線

                                        //485 自己的OUT出線
                                        //gra1.DrawLine(p4, svOTcX4, svcY - ciR, svOTcX4, r1.Y);      //自己的OUT出線往上縱線

                                        svdd = svcY - ciR - 10;
                                        gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, svcY - ciR);      //自己的OUT出線往上短縱線
                                        gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);            //橫線
                                        gra1.DrawLine(p4, svINcX4, svdd, svINcX4, r1.Y);            //橫線右側往上準備進入下一點的短縱線

                                        //HDMI
                                        svdd = svcY + svhdmiR + 8;
                                        gra1.DrawLine(pH, r1.X + r1.Width, svdd, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, svINcXh, svcY + svhdmiR);

                                        gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR, svOTcXh, r1.Y);
                                        //gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR - 15, r1.X + r1.Width / 2, r1.Y);
                                        //gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR, svOTcXh, svcY + svhdmiR + 9);
                                        //gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X + r1.Width / 2, r1.Y + r1.Height);

                                        //svdd = svcY + svhdmiR + 8;
                                        //gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        //gra1.DrawLine(pH, svINcXh, svdd, r1.X + r1.Width, svdd);
                                        //gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 9);
                                        //gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X + r1.Width / 2, r1.Y + r1.Height);

                                        //gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, r1.Y + r1.Height);
                                        //svdd = svcY + ciR + 15;
                                        //gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svdd);
                                        //gra1.DrawLine(p4, svINcX4, svdd, r1.X + r1.Width, svdd);
                                    }


                                }
                                else if (svmk[3] == "99")
                                {
                                    //gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, r1.Y + r1.Height);              //單一條縱線
                                    gra1.DrawLine(pH, svOTcXh, r1.Y + r1.Height, svOTcXh, svcY + svhdmiR + 15);
                                    gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 15, svINcXh, svcY + svhdmiR + 15);
                                    gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svcY + svhdmiR + 15);

                                    //485
                                    //svdd = svcY - ciR - 20;
                                    //gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, r1.Y);
                                    //gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                    //gra1.DrawLine(p4, svINcX4, svcY - ciR, svINcX4, svdd);

                                    gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);      //上一點的OUT到自己的IN往上縱線
                                }
                                #endregion 2x2FHD(背面走線485右上接入+HDMI右上接入)
                            }
                            else if (mvars.in485 == 3)
                            {
                                //0037 新增(先Skeleton_nBox3計算座標)
                                #region 2x2FHD(背面走線485左上接入+HDMI左上接入)
                                int svdd;
                                svmkf = Form1.nvsender[0].regBoxMark[svid].Split(',');
                                if (svmk[3].Substring(0, 1) == "1")
                                {
                                    //HDMI
                                    gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, r1.Y + r1.Height);  //往下

                                    gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, r1.Y + r1.Height);  //往下

                                }
                                else if (svmk[3] == "2")
                                {
                                    if (svid == 1)
                                    {
                                        //485 
                                        svdd = svcY - ciR - 10;
                                        gra1.DrawLine(p4, svOTcX4, r1.Y, svOTcX4, svdd);        //上面"1"的 OUT 往下畫到到"2"的 IN
                                        gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);        //橫線
                                        gra1.DrawLine(p4, svINcX4, svdd, svINcX4, svcY - ciR);  //橫線右側往下到 IN

                                        svdd = svcY + ciR + 8;
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svdd);
                                        gra1.DrawLine(p4, svOTcX4, svdd, r1.X + r1.Width, svdd);
                                        //HDMI 左上斜右下
                                        svdd = svcY - svhdmiR - 7;
                                        gra1.DrawLine(pH, svOTcXh, r1.Y, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, svINcXh, svcY - svhdmiR);

                                        svdd = svcY + svhdmiR + 20;
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, r1.X + r1.Width, svdd);

                                    }
                                    else
                                    {
                                        //485 
                                        svdd = svcY + ciR + 8;
                                        gra1.DrawLine(p4, svINcX4, svdd, r1.X, svdd);    //上一點的OUT進入自己的IN前置橫線
                                        gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svdd);      //到自己IN的往上短縱線

                                        svdd = svcY - ciR - 10;
                                        gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, svcY - ciR);      //自己的OUT出線往上短縱線
                                        gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);            //橫線
                                        gra1.DrawLine(p4, svINcX4, svdd, svINcX4, r1.Y);            //橫線右側往上準備進入下一點的短縱線

                                        //HDMI
                                        svdd = svcY + svhdmiR + 20;
                                        gra1.DrawLine(pH, r1.X, svdd, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, svINcXh, svcY + svhdmiR);

                                        gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR, svOTcXh, r1.Y);
                                    }


                                }
                                else if (svmk[3] == "99")
                                {
                                    //gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, r1.Y + r1.Height);              //單一條縱線
                                    gra1.DrawLine(pH, svOTcXh, r1.Y + r1.Height, svOTcXh, svcY + svhdmiR + 15);
                                    gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 15, svINcXh, svcY + svhdmiR + 15);
                                    gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svcY + svhdmiR + 15);

                                    //485
                                    gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);      //上一點的OUT到自己的IN往上縱線
                                }
                                #endregion 2x2FHD(背面走線485左上接入+HDMI左上接入)
                            }
                            else if (mvars.in485 == 4)
                            {
                                //0037 新增(先Skeleton_nBox3計算座標)
                                #region 2x2FHD(背面走線485左下接入+HDMI左下接入)
                                int svdd;
                                svmkf = Form1.nvsender[0].regBoxMark[svid].Split(',');
                                if (svmk[3].Substring(0, 1) == "1")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版
                                    //HDMI
                                    gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR, svOTcXh, r1.Y);

                                    //485
                                    svdd = svcY - ciR - 20;
                                    gra1.DrawLine(p4, svOTcX4, svcY - ciR, svOTcX4, svdd);
                                    gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                    gra1.DrawLine(p4, svINcX4, svdd, svINcX4, r1.Y);

                                }
                                else if (svmk[3] == "2")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版
                                    if (svid == 1)
                                    {
                                        //485
                                        gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);
                                        svdd = svcY - ciR - 4;
                                        gra1.DrawLine(p4, svOTcX4, svcY - ciR, svOTcX4, svdd);
                                        gra1.DrawLine(p4, svOTcX4, svdd, r1.X + r1.Width, svdd);

                                        svdd = svcY + svhdmiR + 27;
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, svINcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, svOTcXh, r1.Y + r1.Height);
                                        //svdd = svcY + svhdmiR + 8;
                                        //gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                        //gra1.DrawLine(pH, svOTcXh, svdd, r1.X, svdd);

                                        svdd = svcY - svhdmiR - 12;
                                        gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR, svOTcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, r1.X + r1.Width, svdd);

                                    }
                                    else
                                    {
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, r1.Y + r1.Height);
                                        svdd = svcY - ciR - 4;
                                        gra1.DrawLine(p4, svINcX4, svcY - ciR, svINcX4, svdd);
                                        gra1.DrawLine(p4, svINcX4, svdd, r1.X, svdd);

                                        svdd = svcY - svhdmiR - 12;
                                        gra1.DrawLine(pH, svINcXh, svcY - svhdmiR, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, r1.X, svdd);

                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 9);
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X + r1.Width / 2, r1.Y + r1.Height);

                                    }


                                }
                                else if (svmk[3] == "99")
                                {
                                    //寫HDMI匯集在辨識"1","2","99"前查看請找v0031版
                                    gra1.DrawLine(pH, svINcXh, svcY - svhdmiR, svINcXh, svcY - svhdmiR - 9);              //上
                                    gra1.DrawLine(pH, svINcXh, svcY - svhdmiR - 9, r1.X + r1.Width / 2, r1.Y);
                                    //485
                                    svdd = svcY - ciR - 20;
                                    gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, r1.Y);
                                    gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                    gra1.DrawLine(p4, svINcX4, svcY - ciR, svINcX4, svdd);
                                }
                                #endregion 2x2FHD(背面走線485左下接入+HDMI左下接入)
                            }
                        }
                        else
                        {
                            if (svid == Form1.nvsender[0].regPoCards - 1)
                            {
                                svmkf = Form1.nvsender[0].regBoxMark[svid - 1].Split(',');
                            }
                            else
                                svmkf = Form1.nvsender[0].regBoxMark[svid + 1].Split(',');
                            int svdd;


                            #region 485
                            svcolf = Convert.ToUInt16(svmkf[4]) / Convert.ToUInt16(svmkf[6]);
                            svrowf = Convert.ToUInt16(svmkf[5]) / Convert.ToUInt16(svmkf[7]);

                            if (svrow == svrowf)
                            {
                                if (svcol > svcolf)
                                {
                                    if (mvars.boxRows == 1)
                                    {
                                        if (mvars.in485 < 3)
                                        {
                                            //背面走線由右到左都是在下方的右邊的out到左邊的in
                                            svdd = svcY + ciR + 9;
                                            gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svdd);
                                            gra1.DrawLine(p4, svOTcX4, svdd, r1.X, svdd);
                                            if (svid > 0)
                                            {
                                                gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svdd);
                                                gra1.DrawLine(p4, svINcX4, svdd, r1.X + r1.Width, svdd);
                                            }
                                        }
                                        else
                                        {
                                            if (svid == Form1.nvsender[0].regPoCards - 1)
                                            {
                                                if (svid % 2 == 0)
                                                {
                                                    svdd = svcY + ciR + 9;
                                                    gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svdd);
                                                    gra1.DrawLine(p4, svINcX4, svdd, r1.X, svdd);
                                                }
                                                else
                                                {
                                                    svdd = svcY - ciR - 9;
                                                    gra1.DrawLine(p4, svINcX4, svcY - ciR, svINcX4, svdd);
                                                    gra1.DrawLine(p4, svINcX4, svdd, r1.X, svdd);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        svdd = svcY + ciR + 9;
                                        gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svdd);
                                        gra1.DrawLine(p4, svOTcX4, svdd, r1.X, svdd);

                                        if (svrow == mvars.boxRows - 1)
                                        {
                                            svdd = svcY - ciR - 10;
                                            gra1.DrawLine(p4, svOTcX4, r1.Y, svOTcX4, svdd);
                                            gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                            gra1.DrawLine(p4, svINcX4, svdd, svINcX4, svcY - ciR);
                                        }
                                        else if (svrow == svfirstR)
                                        {
                                            gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);
                                        }
                                    }
                                }
                                else if (svcol < svcolf)
                                {
                                    if (mvars.boxRows == 1)
                                    {
                                        if (mvars.in485 < 3)
                                        {
                                            //背面走線由右到左都是在下方的右邊的out到左邊的in
                                            svdd = svcY + ciR + 9;
                                            if (svid == Form1.nvsender[0].regPoCards - 1)
                                            {
                                                gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svdd);
                                                gra1.DrawLine(p4, svINcX4, svdd, r1.X + r1.Width, svdd);
                                            }
                                        }
                                        else
                                        {
                                            if (svid == 0)
                                            {
                                                svdd = svcY - ciR - 9;
                                                gra1.DrawLine(p4, svOTcX4, svcY - ciR, svOTcX4, svdd);
                                                gra1.DrawLine(p4, svOTcX4, svdd, r1.X + r1.Width, svdd);
                                            }
                                            else
                                            {
                                                if (svid % 2 == 1)
                                                {
                                                    svdd = svcY - ciR - 9;
                                                    gra1.DrawLine(p4, svINcX4, svcY - ciR, svINcX4, svdd);
                                                    gra1.DrawLine(p4, svINcX4, svdd, r1.X, svdd);
                                                    svdd = svcY + ciR + 9;
                                                    gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, svdd);
                                                    gra1.DrawLine(p4, svOTcX4, svdd, r1.X + r1.Width, svdd);
                                                }
                                                else
                                                {
                                                    svdd = svcY + ciR + 9;
                                                    gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, svdd);
                                                    gra1.DrawLine(p4, svINcX4, svdd, r1.X, svdd);
                                                    svdd = svcY - ciR - 9;
                                                    gra1.DrawLine(p4, svOTcX4, svcY - ciR, svOTcX4, svdd);
                                                    gra1.DrawLine(p4, svOTcX4, svdd, r1.X + r1.Width, svdd);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (svrow == svfirstR && svcol % 2 == 0)
                                        {
                                            svdd = svcY - ciR - 9;
                                            gra1.DrawLine(p4, svOTcX4, svdd, r1.X + r1.Width, svdd);
                                            gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, svcY - ciR);
                                        }
                                        else if (svrow == svfirstR && svcol % 2 == 1)
                                        {
                                            svdd = svcY - ciR - 9;
                                            gra1.DrawLine(p4, svOTcX4, svdd, r1.X + r1.Width, svdd);
                                            gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, svcY - ciR);
                                        }
                                        else
                                        {
                                            svdd = svcY + ciR + 9;
                                            gra1.DrawLine(p4, svOTcX4, svdd, r1.X + r1.Width, svdd);
                                            gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, svcY + ciR);
                                        }



                                        if (svrow == mvars.boxRows - 1)
                                        {
                                            svdd = svcY - ciR - 10;
                                            gra1.DrawLine(p4, svOTcX4, r1.Y, svOTcX4, svdd);
                                            gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                            gra1.DrawLine(p4, svINcX4, svdd, svINcX4, svcY - ciR);
                                        }
                                        else if (svrow == svfirstR)
                                        {
                                            gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (svrow > svrowf)
                                {
                                    svdd = svcY - ciR - 10;
                                    //由下往上(in485=1的id0,id15會進) 2x5 svid==0
                                    if (svid != Form1.nvsender[0].regPoCards - 1)
                                    {
                                        gra1.DrawLine(p4, svOTcX4, svcY - ciR, svOTcX4, svdd);
                                        gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                        gra1.DrawLine(p4, svINcX4, svdd, svINcX4, r1.Y);
                                    }
                                    else if (svid == Form1.nvsender[0].regPoCards - 1)
                                    {
                                        gra1.DrawLine(p4, svINcX4, svcY - ciR, svINcX4, svdd);
                                        gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                        gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, r1.Y);
                                    }
                                    //避免起始多畫
                                    if (svid > 0 && svid < Form1.nvsender[0].regPoCards - 1)
                                    {
                                        if (svrow == mvars.boxRows - 1)
                                        {
                                            svdd = svcY + ciR + 9;
                                            gra1.DrawLine(p4, svINcX4, svdd, r1.X + r1.Width * ((4 - mvars.in485) / 2), svdd);
                                            gra1.DrawLine(p4, svINcX4, svdd, svINcX4, svcY + ciR);
                                        }
                                        else
                                            gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);
                                    }
                                }
                                else if (svrow < svrowf)
                                {
                                    //由上往下(in485=2的id=0,id15會進)
                                    svdd = svcY - ciR - 10;
                                    if (svid != Form1.nvsender[0].regPoCards - 1) gra1.DrawLine(p4, svOTcX4, svcY + ciR, svOTcX4, r1.Y + r1.Height);
                                    else if (svid == Form1.nvsender[0].regPoCards - 1) gra1.DrawLine(p4, svINcX4, svcY + ciR, svINcX4, r1.Y + r1.Height);
                                    if (svid > 0 && svid < Form1.nvsender[0].regPoCards - 1)
                                    {
                                        if (svrow == svfirstR && svid % mvars.boxRows == 0)
                                        {
                                            if ((mvars.in485 == 4 && svcol % 2 == 1) || (mvars.in485 == 3 && svcol % 2 == 0))
                                            {
                                                svdd = svcY - ciR - 9;
                                                gra1.DrawLine(p4, svINcX4, svdd, r1.X + r1.Width * ((4 - mvars.in485) / 2), svdd);
                                                gra1.DrawLine(p4, svINcX4, svdd, svINcX4, svcY - ciR);
                                            }
                                            else
                                            {
                                                svdd = svcY + ciR + 9;
                                                gra1.DrawLine(p4, svINcX4, svdd, r1.X + r1.Width * ((4 - mvars.in485) / 2), svdd);
                                                gra1.DrawLine(p4, svINcX4, svdd, svINcX4, svcY + ciR);
                                            }
                                        }
                                        else
                                        {
                                            gra1.DrawLine(p4, svOTcX4, svdd, svOTcX4, r1.Y);
                                            gra1.DrawLine(p4, svOTcX4, svdd, svINcX4, svdd);
                                            gra1.DrawLine(p4, svINcX4, svdd, svINcX4, svcY - ciR);
                                        }
                                    }
                                }
                            }
                            #endregion 485


                            #region HDMI

                            if (svmk[22] == "1" || svmk[22] == "4")
                            {
                                svdd = svcY + svhdmiR + Convert.ToUInt16(svmk[22]) % 3 * 20;
                                if (svmk[22] == "4") svdd += svcol % 2 * 7;
                            }
                            else
                            {
                                svdd = svcY + svhdmiR + 20;
                            }


                            if (Convert.ToUInt16(svmk[3]) <= 99)
                            {
                                if (svmk[3] == "1")
                                {
                                    if (svmk[22] == "1") gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR, svOTcXh, r1.Y);
                                    else if (svmk[22] == "2")
                                    {
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, r1.X, svdd);
                                    }
                                    else if (svmk[22] == "3") gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, r1.Y + r1.Height);
                                    else if (svmk[22] == "4")
                                    {
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, r1.X + r1.Width, svdd);
                                    }
                                    else if (svmk[22] == "6")
                                    {
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 6);      //下
                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 6, r1.X, svcY + svhdmiR + 6);    //out左橫
                                    }
                                }
                                else if (svmk[3] == "2")
                                {
                                    if (svmk[22] == "1")
                                    {
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, svOTcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, svOTcXh, r1.Y + r1.Height);

                                        gra1.DrawLine(pH, svOTcXh, svcY - svhdmiR, svOTcXh, r1.Y);

                                    }
                                    else if (svmk[22] == "2")
                                    {
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, r1.X + r1.Width, svdd);

                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, r1.X, svdd);
                                    }
                                    else if (svmk[22] == "3")
                                    {
                                        gra1.DrawLine(pH, svOTcXh, r1.Y, svINcXh, svcY - svhdmiR - 7);
                                        gra1.DrawLine(pH, svINcXh, svcY - svhdmiR - 7, svINcXh, svcY - svhdmiR);

                                        gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, r1.Y + r1.Height);
                                    }
                                    else if (svmk[22] == "4")
                                    {
                                        if (svcY + svhdmiR + 5 * Convert.ToUInt16(svmk[22]) < svdd)
                                        {
                                            gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                            gra1.DrawLine(pH, svOTcXh, svdd, r1.X + r1.Width, svdd);
                                            svdd = svcY + svhdmiR + 5 * Convert.ToUInt16(svmk[22]);
                                            gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                            gra1.DrawLine(pH, svINcXh, svdd, r1.X, svdd);
                                        }
                                        else if (svcY + svhdmiR + 5 * Convert.ToUInt16(svmk[22]) >= svdd)
                                        {
                                            gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svdd);
                                            gra1.DrawLine(pH, svOTcXh, svdd, r1.X + r1.Width, svdd);
                                            svdd = svcY + svhdmiR + 5 * Convert.ToUInt16(svmk[22]) + 7;
                                            gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                            gra1.DrawLine(pH, svINcXh, svdd, r1.X, svdd);
                                        }
                                    }
                                    else if (svmk[22] == "6")
                                    {
                                        if (Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 0 || Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]) == 2)
                                        {
                                            //與上一點銜接
                                            //HDMI in
                                            gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svcY + svhdmiR + 6);  //下
                                            gra1.DrawLine(pH, svINcXh, svcY + svhdmiR + 6, r1.X + r1.Width, svcY + svhdmiR + 6);   //in右橫
                                            //到下一點
                                            //HDMI out
                                            gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 9);              //下
                                            gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 9, r1.X + r1.Width, r1.Y + r1.Height);   //左上out-右下角落
                                        }
                                        else
                                        {
                                            //與上一點銜接
                                            //HDMI in
                                            gra1.DrawLine(pH, svINcXh, svcY - svhdmiR, svINcXh, svcY - svhdmiR - 6);  //上
                                            gra1.DrawLine(pH, svINcXh, svcY - svhdmiR - 6, r1.X, r1.Y);   //左上角落-右下in
                                            //到下一點
                                            //HDMI out
                                            gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR, svOTcXh, svcY + svhdmiR + 6);  //下
                                            gra1.DrawLine(pH, svOTcXh, svcY + svhdmiR + 6, r1.X, svcY + svhdmiR + 6);  //下
                                        }
                                    }
                                }
                                else if (svmk[3] == "99")
                                {
                                    if (svmk[22] == "1")
                                    {
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, svOTcXh, svdd);
                                        gra1.DrawLine(pH, svOTcXh, svdd, svOTcXh, r1.Y + r1.Height);
                                    }
                                    else if (svmk[22] == "2")
                                    {
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                        gra1.DrawLine(pH, svINcXh, svdd, r1.X + r1.Width, svdd);
                                    }
                                    else if (svmk[22] == "3")
                                    {
                                        gra1.DrawLine(pH, svOTcXh, r1.Y, svINcXh, svcY - svhdmiR - 7);
                                        gra1.DrawLine(pH, svINcXh, svcY - svhdmiR - 7, svINcXh, svcY - svhdmiR);
                                    }
                                    else if (svmk[22] == "4")
                                    {
                                        if (svcY + svhdmiR + 5 * Convert.ToUInt16(svmk[22]) < svdd)
                                        {
                                            svdd = svcY + svhdmiR + 5 * Convert.ToUInt16(svmk[22]);
                                            gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                            gra1.DrawLine(pH, svINcXh, svdd, r1.X, svdd);
                                        }
                                        else if (svcY + svhdmiR + 5 * Convert.ToUInt16(svmk[22]) >= svdd)
                                        {
                                            svdd = svcY + svhdmiR + 5 * Convert.ToUInt16(svmk[22]) + 7;
                                            gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svdd);
                                            gra1.DrawLine(pH, svINcXh, svdd, r1.X, svdd);
                                        }
                                    }
                                    else if (svmk[22] == "6")
                                    {
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR, svINcXh, svcY + svhdmiR + 6);  //下
                                        gra1.DrawLine(pH, svINcXh, svcY + svhdmiR + 6, r1.X + r1.Width, svcY + svhdmiR + 6);  //下
                                    }
                                }
                            }

                            #endregion HDMI
                        }
                    }
                }
            }
            // Fred 2025/01/22 威創客製需求更改 >> 新增正面圖 並保留背面圖供客人切換
            draw.Save("背面圖.png");
            gra1.Dispose();

            if (isback == false)
            {
                draw.RotateFlip(RotateFlipType.RotateNoneFlipX);
                using (Graphics g = Graphics.FromImage(draw))
                {
                    for (int svid = 0; svid < Form1.nvsender[0].regPoCards; svid++)
                    {
                        svmk = Form1.nvsender[0].regBoxMark[svid].Split(',');
                        svcol = (byte)(Convert.ToUInt16(svmk[4]) / Convert.ToUInt16(svmk[6]));
                        svrow = (byte)(Convert.ToUInt16(svmk[5]) / Convert.ToUInt16(svmk[7]));

                        r1 = dgv_box.GetCellDisplayRectangle(svcol, svrow, false);

                        r1.X -= 25;
                        r1.Y -= 20;
                        svOTcXh = r1.X + svhdmiR + 5;                       //圓的圓心X 15
                        svcY = r1.Y + r1.Height * 35 / 100 + svhdmiR;       //圓的圓心Y 96*35/100+15==47

                        svINcXh = r1.X + 20;
                        // ID 需要新的判斷方式
                        svINcXh = svOTcXh + svhdmiR * 8 + 8;
                        var horizontalMirrored_ID = MirrorPoint(svINcXh + 2, r1.Y + 4, draw.Width, draw.Height, true, false);
                        g.DrawString("ID. " + (svid + 1).ToString(), new Font("Arial", 9, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 255, 255)), horizontalMirrored_ID.x, horizontalMirrored_ID.y, strF);
                        // HDMI IN
                        g.DrawString("HDMI" + "\r\n" + "IN", fnt, new SolidBrush(Color.White), svOTcXh, svcY - svhdmiR + 5, strF);
                        // HDMI IN 編號 需要新邏輯
                        if (svmk[3].Substring(0, 1) == "1")
                        {
                            svINcXh = svOTcXh + svhdmiR * 8 + 8;
                            var horizontalMirrored_HDMI_In = MirrorPoint(svINcXh + 10, svcY + 3, draw.Width, draw.Height, true, false);
                            g.DrawString(svmk[1], new Font("Arial", 12, FontStyle.Bold),
                                    new SolidBrush(Color.White),
                                    horizontalMirrored_HDMI_In.x + 20, horizontalMirrored_HDMI_In.y, strF);
                        }

                        // HDMI OUT
                        svINcXh = svOTcXh + svhdmiR * 8 + 8;
                        if (svmk[3].Substring(0, 1) == "1")
                        {
                            g.DrawString("HDMI" + "\r\n" + "OUT .", new Font("Arial", 8, FontStyle.Regular),
                             new SolidBrush(Color.White), svINcXh + 2, svcY - 12, strF);
                        }
                        else
                        {
                            g.DrawString("HDMI" + "\r\n" + "OUT", fnt, new SolidBrush(Color.White), svINcXh, svcY - svhdmiR + 5, strF);

                        }
                        // Micro out
                        svOTcX4 = svOTcXh + svhdmiR * 6 + 8;
                        g.DrawString("micro" + "\r\n" + "USB", fnt, strSB, svOTcX4, svcY - svmcoH / 2, strF);
                        g.DrawString("OUT", new Font("Arial", 8, FontStyle.Bold), strSB, svOTcX4 + 1, svcY + 2, strF);
                        // Micro out
                        svINcX4 = svOTcXh + svhdmiR * 4 + 8;
                        g.DrawString("micro" + "\r\n" + "USB", fnt, strSB, svINcX4, svcY - svmcoH / 2, strF);
                        g.DrawString("IN", new Font("Arial", 8, FontStyle.Bold), strSB, svINcX4 + 1, svcY + 2, strF);
                        // Type A IN
                        svINcXa = svOTcXh + svhdmiR * 2 + 3;
                        if (svid == 0)
                        {

                            g.DrawString("TypeA", fnt, strSB, svINcXa + 1, svcY - svmcoH / 2, strF);
                            svINcXa = svOTcXh + svhdmiR * 6 + 1;
                            var horizontalMirrored = MirrorPoint(svINcXa + 4, svcY + 13, draw.Width, draw.Height, true, false);
                            g.DrawString("PC", new Font("Arial", 10, FontStyle.Bold), strSB, horizontalMirrored.x, horizontalMirrored.y, strF);

                        }
                        else
                        {

                            g.DrawRectangle(new Pen(sb4.Color), svINcXa - svmcoW / 2, svcY - svmcoH / 2, svmcoW + 5, svmcoH / 3);
                            g.DrawString("TypeA", fnt, strSB, svINcXa + 1, svcY - svmcoH / 2, strF);
                        }
                    }
                }
                draw.Save("正面圖.png");
            }
            //----------------------------------------------------------------------
            


            p4.Dispose();
            pH.Dispose();
            fnt.Dispose();
            strF.Dispose();
            strSB.Dispose();
            sb4.Dispose();

            //gra1.Dispose();
            //pnl_dgv.Paint -= new PaintEventHandler(pnl_dgv_Paint);
            pictureBox1.Image = draw;
        }

        // Fred 2025/01/22 威創客製需求更改 >> 新增正面圖 並保留背面圖供客人切換
        bool isback = false;

        static (int x, int y) MirrorPoint(int x, int y, int imageWidth, int imageHeight, bool horizontal, bool vertical)
        {
            int newX = horizontal ? imageWidth - x - 1 : x; // 水平镜像
            int newY = vertical ? imageHeight - y - 1 : y;  // 垂直镜像
            return (newX, newY);
        }
        //----------------------------------------------------------------------

        private Image CutImage(Image SourceImage, Point StartPoint, Rectangle CutArea)
        {
            Bitmap NewBitmap = new Bitmap(CutArea.Width, CutArea.Height);
            Graphics tmpGraph = Graphics.FromImage(NewBitmap);
            tmpGraph.DrawImage(SourceImage, CutArea, StartPoint.X, StartPoint.Y, CutArea.Width, CutArea.Height, GraphicsUnit.Pixel);
            tmpGraph.Dispose();
            return NewBitmap;
        }




        void dirSearch(string sDir)
        {
            cmb_box.Items.Clear();
            try
            {
                //先找出所有目錄
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    //先針對目錄的檔案做處理
                    foreach (string f in Directory.GetFiles(d, "*.exe"))
                    {
                        if (f.IndexOf("host.", 0) == -1)
                        {
                            string svs1 = "";
                            string[] svs = f.Split('\\')[f.Split('\\').Length - 1].Split('.');
                            for (int svi = 0; svi < svs.Length; svi++)
                            {
                                if (svs[svi].ToLower() == "exe")
                                {
                                    break;
                                }
                                svs1 += "." + svs[svi];
                            }
                            svs1 = svs1.Substring(1, svs1.Length - 1);
                            cmb_box.Items.Add(svs1);
                        }
                    }
                    //此目錄完再針對每個子目錄做處理
                    dirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            if (cmb_box.Items.Count <= 0)
            {
                foreach (string f in Directory.GetFiles(sDir, "*.exe"))
                {
                    if (f.IndexOf("host.", 0) == -1)
                    {
                        string svs1 = "";
                        string[] svs = f.Split('\\')[f.Split('\\').Length - 1].Split('.');
                        for (int svi = 0; svi < svs.Length; svi++)
                        {
                            if (svs[svi].ToLower() == "exe")
                            {
                                break;
                            }
                            svs1 += "." + svs[svi];
                        }
                        svs1 = svs1.Substring(1, svs1.Length - 1);
                        cmb_box.Items.Add(svs1);
                    }
                }
            }
            lst_get1.Refresh();
            if (cmb_box.Items.Count > 0) cmb_box.SelectedIndex = 0;

            //lst_get1.Items.AddRange(filelist);
            /*
            //查詢資料夾內容清單
            string[] filecollection;
            string filepath = "f:\\uploads";
            FileInfo thefileinfo;

            filecollection = Directory.GetFiles(filepath, "*.txt");
            for (int i = 0; i < filecollection.Length; i++)
            {
                thefileinfo = new FileInfo(filecollection[i]);
                Console.WriteLine(thefileinfo.Name.ToString());
            }
            */
        }



        public Color GetPixelFromScreen(int x, int y)
        {
            int width = 1;
            int height = 1;
            Bitmap img = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.CopyFromScreen(
                new Point(x, y),
                new Point(0, 0),
                new Size(width, height));
            }
            return img.GetPixel(0, 0);
        }





















        private void button1_Click(object sender, EventArgs e)
        {
            #region ↓ demo data create
            //Form1.screenInfoList = new List<LEDScreenInfo>();   //必須否則會越來越多 Form1.screenInfoList
            //List<ScanBoardMapRegion> _ScanBoardInfoList = new List<ScanBoardMapRegion>();
            //Form1.screenInfoList.Add(_screenInfoList);
            //List<ScanBoardMapRegion> _ScanBoardInfoList = new List<ScanBoardMapRegion>();


            LEDScreenInfo _screenInfoList = new LEDScreenInfo();
            List<PrimaryMapRegion> _primaryInfoList = new List<PrimaryMapRegion>();

            #region ↓ 欄4列2
            PrimaryMapRegion _sbMapRegion;
            _sbMapRegion.Rec = 0;
            _sbMapRegion.Sen = 0;
            _sbMapRegion.Po = 1;
            _sbMapRegion.Ca = 2;
            _sbMapRegion.X = 0;
            _sbMapRegion.Y = 0;
            _sbMapRegion.W = mvars.scrW;
            _sbMapRegion.H = mvars.scrH;
            _sbMapRegion.Gap = _sbMapRegion.Y;
            _primaryInfoList.Add(_sbMapRegion);

            _sbMapRegion.Rec = 0;
            _sbMapRegion.Sen = 0;
            _sbMapRegion.Po = 2;
            _sbMapRegion.Ca = 2;
            _sbMapRegion.X = 0;
            _sbMapRegion.Y = 540;
            _sbMapRegion.W = mvars.scrW;
            _sbMapRegion.H = mvars.scrH;
            _sbMapRegion.Gap = _sbMapRegion.Y;
            _primaryInfoList.Add(_sbMapRegion);

            _sbMapRegion.Rec = 0;
            _sbMapRegion.Sen = 0;
            _sbMapRegion.Po = 3;
            _sbMapRegion.Ca = 2;
            _sbMapRegion.X = 960;
            _sbMapRegion.Y = 540;
            _sbMapRegion.W = mvars.scrW;
            _sbMapRegion.H = mvars.scrH;
            _sbMapRegion.Gap = _sbMapRegion.Y;
            _primaryInfoList.Add(_sbMapRegion);

            _sbMapRegion.Rec = 0;
            _sbMapRegion.Sen = 0;
            _sbMapRegion.Po = 3;
            _sbMapRegion.Ca = 2;
            _sbMapRegion.X = 960;
            _sbMapRegion.Y = 0;
            _sbMapRegion.W = mvars.scrW;
            _sbMapRegion.H = mvars.scrH;
            _sbMapRegion.Gap = _sbMapRegion.Y;
            _primaryInfoList.Add(_sbMapRegion);

            _screenInfoList.primaryInfoList = _primaryInfoList;
            _screenInfoList.ScreenX = 0;
            _screenInfoList.ScreenY = 0;
            _screenInfoList.LEDDisplyType = AIO.LEDDisplyType.ComplexType;
            _screenInfoList.VirtualMode = ScreenVirtualMode.Disable;


            //Form1.screenInfoList[svScr] = (LEDScreenInfo)_screenInfoList.Clone();
            #endregion ↑ 欄4列2

            #endregion ↑ demo data create

            //獲取顯卡的詳細資料
            //getDCdetailInfo();
            //獲取顯卡的詳細資料
            //ManagementObjectSearcher FlashDevice = new ManagementObjectSearcher("select * from win32_VideoController");//声明一个用于检索设备管理信息的对象
            //foreach (ManagementObject FlashDeviceObject in FlashDevice.Get())//循环遍历WMI实例中的每一个对象
            //{
            //    string printName = FlashDeviceObject["name"].ToString();//在文本框中显示显示设备的名称
            //    string aristotle = FlashDeviceObject["PNPDeviceID"].ToString();//在文本框中显示显示设备的PNPDeviceID
            //    MessageBox.Show(printName + "\r\n" + printName);
            //}
            //取得現在顯示的儲存格
            //MessageBox.Show(dgv_box.CurrentCell.ColumnIndex + "," + dgv_box.CurrentCell.RowIndex);

            pnl_dgv.Paint += new PaintEventHandler(pnl_dgv_Paint);
        }
        public static void getDCdetailInfo()
        {
            //必須引用 System.Managerment from 加入(R)的組件
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_VideoController");
            foreach (ManagementObject info in searcher.Get())
            {
                string a = "配接器相容性 " + info["AdapterCompatibility"].ToString();
                string b = "配接器類型 " + info["AdapterDACType"].ToString();
                //string c = "視訊模式描述" + info["VideoModeDescription"].ToString();
                string d = "字幕" + info["Caption"].ToString();
                //string e = "目前比特每圖元" + info["CurrentBitsPerPixel"].ToString();
                //string f = "目前的水準解析度" + info["CurrentHorizontalResolution"].ToString();
                string g = "視頻處理器" + info["VideoProcessor"].ToString();
                //string h = "最大刷新率" + info["MaxRefreshRate"].ToString();
                string i = "設備ID" + info["DeviceID"].ToString();
                string j = "描述" + info["Description"].ToString();
            }
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            string svi3h = "";
            if (MultiLanguage.DefaultLanguage == "en-US")
            {
                if (mp.InputBox("Screen Config", " run『Screen Uniformity Adjustment』?", ref svi3h, this.Left + 350, this.Top + 300, 1, 1, 3, "") == DialogResult.Cancel) return;
                if (mvars.FormShow[6] == false) { Form1.i3init = new i3_Init(); Form1.i3init.Show(); Form1.i3init.Location = new Point(this.Left + 350, this.Top + 300); }
                Form1.i3init.Visible = true;
                Form1.i3init.Text = " ";
                i3_Init.lbl_1.AutoSize = true;
                i3_Init.lbl_1.BackColor = Control.DefaultBackColor;
                i3_Init.lbl_1.Location = new Point(30, 50);
                i3_Init.lbl_1.Text = "    Screen Adjustment initialize ... ";
                i3_Init.lbl_1.Visible = true;
                //i3_Init.btn_0.Visible = false;
                //i3_Init.btn_1.Visible = false;
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            {
                if (mp.InputBox("大屏拼接", "是否執行『大屏均勻性調整』?", ref svi3h, this.Left + 350, this.Top + 300, 1, 1, 3, "") == DialogResult.Cancel) return;
                if (mvars.FormShow[6] == false) { Form1.i3init = new i3_Init(); Form1.i3init.Show(); Form1.i3init.Location = new Point(this.Left + 350, this.Top + 300); }
                Form1.i3init.Visible = true;
                Form1.i3init.Text = " ";
                i3_Init.lbl_1.AutoSize = true;
                i3_Init.lbl_1.BackColor = Control.DefaultBackColor;
                i3_Init.lbl_1.Location = new Point(30, 50);
                i3_Init.lbl_1.Text = "    大屏調整功能啟動中 ... ";
                i3_Init.lbl_1.Visible = true;
                //i3_Init.btn_0.Visible = false;
                //i3_Init.btn_1.Visible = false;
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CN")
            {
                if (mp.InputBox("大屏拼接", "是否执行『大屏均匀性调整』?", ref svi3h, this.Left + 350, this.Top + 300, 1, 1, 3, "") == DialogResult.Cancel) return;
                if (mvars.FormShow[6] == false) { Form1.i3init = new i3_Init(); Form1.i3init.Show(); Form1.i3init.Location = new Point(this.Left + 350, this.Top + 300); }
                Form1.i3init.Visible = true;
                Form1.i3init.Text = " ";
                i3_Init.lbl_1.AutoSize = true;
                i3_Init.lbl_1.BackColor = Control.DefaultBackColor;
                i3_Init.lbl_1.Location = new Point(30, 50);
                i3_Init.lbl_1.Text = "    大屏调整功能启动中 ... ";
                i3_Init.lbl_1.Visible = true;
                //i3_Init.btn_0.Visible = false;
                //i3_Init.btn_1.Visible = false;
            }
            else if (MultiLanguage.DefaultLanguage == "ja-JP")
            {
                if (mp.InputBox("全体の設定", "『全体均一性调整』実行するかどうか?", ref svi3h, this.Left + 450, this.Top + 300, 1, 1, 3, "") == DialogResult.Cancel) return;
                if (mvars.FormShow[6] == false) { Form1.i3init = new i3_Init(); Form1.i3init.Show(); Form1.i3init.Location = new Point(this.Left + 450, this.Top + 300); }
                Form1.i3init.Visible = true;
                Form1.i3init.Text = " ";
                i3_Init.lbl_1.AutoSize = true;
                i3_Init.lbl_1.BackColor = Control.DefaultBackColor;
                i3_Init.lbl_1.Location = new Point(30, 50);
                i3_Init.lbl_1.Text = "    大画面調整機能が起動します ... ";
                i3_Init.lbl_1.Visible = true;
                //i3_Init.btn_0.Visible = false;
                //i3_Init.btn_1.Visible = false;
            }


            i3_Init.dgvota.Visible = false;
            i3_Init.dgvformmsg.Visible = false;


            Form1.i3init.ControlBox = false;
            Form1.i3init.Size = new Size(i3_Init.lbl_1.Left + i3_Init.lbl_1.Width + 100, i3_Init.lbl_1.Top + i3_Init.lbl_1.Height + 100);
            Form1.i3init.BringToFront();
            mp.doDelayms(100);

            // FileName 是要執行的檔案
            Process ui = new Process();
            if (mvars.demoMode)
                ui.StartInfo.FileName = mvars.strStartUpPath + @"\Parameter\SendMessage.exe";   //WT_UI
            else
                ui.StartInfo.FileName = mvars.strStartUpPath + @"\Parameter\ColorAdjustment\" + cmb_box.Items[0].ToString() + ".exe";
            ui.Start();
            mp.doDelayms(100);

            //"ColorAdjustment"
            byte i = 0;
            do
            {
                mp.doDelayms(500);
                i++;
                if (Form1.FindWindow(null, "ColorAdjustment") != 0 || Form1.FindWindow(null, "WT_UI") != 0) break;
            }
            while (i < 100);
            mp.doDelayms(1500);
            Form1.Form1_hide();
            //Form1.ShowInTaskbar = true;
            Form1.notifyIcon.Visible = true;
            Form1.notifyIcon.ShowBalloonTip(10, mvars.strUInameMe, "v" + mvars.UImajor, ToolTipIcon.Info);
        }

        private void dgv_box_MouseMove(object sender, MouseEventArgs e)
        {
            label2.Text = "滑鼠座標 " + e.X.ToString() + "," + e.Y.ToString();
        }


        //查看 pnl_CellPaint 須至v0028 

        private void btn_resGen_Click(object sender, EventArgs e)
        {
            /*
            Image img = Resource1.panelsemi;
            ====================================================

            ====================================================
            using (ResXResourceReader reader = new ResXResourceReader(mvars.strStartUpPath + @"C:\MyResources.resx"))
            {
                foreach (DictionaryEntry entry in reader)
                {
                    string s = string.Format("{0} ({1})= '{2}'",
                      entry.Key, entry.Value.GetType(), entry.Value);
                    MessageBox.Show(s);
                }
            }
            ====================================================

            ====================================================
            Bitmap bmp = new Bitmap(@".\SplashScreen.jpg");
            MemoryStream imageStream = new MemoryStream();
            bmp.Save(imageStream, ImageFormat.Jpeg);
            ResXResourceWriter writer = new ResXResourceWriter("AppResources.resx");
            writer.AddResource("SplashScreen", imageStream);
            writer.Generate();
            writer.Close();
            ====================================================

            ====================================================
            //不保存文件直接到窗体控件 实例
            Bitmap imageG = TMDHleperCS.FreeScreenshotControlIntPtr(h, 590, 276, 310, 310);
            MemoryStream ms = new MemoryStream();
            imageG.Save(ms, ImageFormat.Png);
            pictureBox1.Image = Image.FromStream(ms);
            ====================================================

            ====================================================
            string str = System.Environment.CurrentDirectory;   //一直到/bin/Debug
            str = System.Windows.Forms.Application.StartupPath;   //一直到/bin/Debug
            str = System.IO.Directory.GetCurrentDirectory();   //一直到/bin/Debug
            str = System.AppDomain.CurrentDomain.BaseDirectory; //一直到/bin/Debug
            str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//一直到/bin/Debug
            str = System.Windows.Forms.Application.ExecutablePath;//一直到/bin/Debug/AIO.exe
            str = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;//一直到/bin/Debug/AIO.exe
            str = this.GetType().Assembly.Location;//一直到/bin/Debug/AIO.exe

            str= @".\bin";
            MessageBox.Show(str);
            return;
            ====================================================


            ====================================================
            string resourcepath = @"d:\Resource1.resx";
            Assembly assm = Assembly.GetExecutingAssembly();
            Stream istr = assm.GetManifestResourceStream(resourcepath);
            StreamReader sr = new StreamReader(istr);
            string str = sr.ReadLine();
            Array.Clear(BinArr, 0, BinArr.Length);
            //while (true)
            //{
            //    int r = i.Read(BinArr, 0, BinArr.Length);
            //    if (r <= 0)
            //        return;

            //}
            ====================================================

            ====================================================
            //var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            //var iconPath = Path.Combine(outPutDirectory, "Folder\\Img.jpg");
            //MessageBox.Show(outPutDirectory);
            ====================================================


            //讀取內嵌資源
            //Assembly asm = Assembly.GetExecutingAssembly();
            //string xmlName = asm.GetName().Name;
            //Stream stream = asm.GetManifestResourceStream(xmlName + ".XMLFile1.xml");

            //讀取資源檔(只能使用內崁資源)
            ResXResourceReader resx = new ResXResourceReader("EDID.resx"); //D:\AIO\20230506\AIO\bin\Debug\EDID.resx


            //ResourceManager rm = new ResourceManager("UseResourceBinary.Resources",Assembly.GetExecutingAssembly());
            //Bitmap screen = (Bitmap)Image.FromStream(rm.GetStream("SplashScreen"));
            //将Image转换成流数据，并保存为byte[]
            //MemoryStream mstream = new MemoryStream();
            //Image img = null;
            //img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

            ////screen.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);

            //byte[] byData = new Byte[mstream.Length];
            //mstream.Position = 0;
            //mstream.Read(byData, 0, byData.Length);
            //mstream.Close();

            */

            string svname;
            byte[] BinArr = new byte[256];

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select EDID bin file";
            //dialog.InitialDirectory = "c:\\"; 
            dialog.FileName = "";
            dialog.RestoreDirectory = true;     //關掉視窗時會還原原先的開啟目錄位置
            dialog.Filter = "Bin files (*.bin)|*.bin";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                svname = dialog.FileName.Split('\\')[dialog.FileName.Split('\\').Length - 1];
                if (svname.IndexOf(".bin", 0) != -1) svname = svname.Substring(0, svname.Length - ".bin".Length);
                BinArr = File.ReadAllBytes(dialog.FileName);
                if (BinArr.Length != 256) { MessageBox.Show("File Length <> 256 bytes" + "\r\n" + "\r\n" + "FileLen " + BinArr.Length, "Error"); }
                else
                {
                    MemoryStream memoryStream = new MemoryStream(BinArr, 0, BinArr.Length);
                    memoryStream.Write(BinArr, 0, BinArr.Length);
                    ResXResourceWriter writer = new ResXResourceWriter("EDID.resx");
                    writer.AddResource("19200540", memoryStream);
                    writer.Generate();
                    writer.Close();
                }
            }

            Array.Clear(BinArr, 0, BinArr.Length);


            var assm = Assembly.GetExecutingAssembly();

            //foreach (var file in assm.GetManifestResourceNames())
            //{
            //    if (file == "AIO.EDID.resource") MessageBox.Show("AIO.EDID.resource");
            //}
            //return;

            //var stream = assm.GetManifestResourceStream("AIO.Resource1.resources");
            // stream = assm.GetManifestResourceStream("AIO.EDID.resources");
            Stream istr = assm.GetManifestResourceStream("AIO.EDID.resources");
            //StreamReader sr = new StreamReader(istr);


            Stream sr = Assembly.GetExecutingAssembly().GetManifestResourceStream("AIO.EDID.resources");

            sr.Read(BinArr, 0, BinArr.Length);

            //    ResourceManager rm = new ResourceManager("EDID.resx", Assembly.GetExecutingAssembly());
            MemoryStream ms = new MemoryStream(BinArr, 0, BinArr.Length);
            //rm.GetStream(ms);



        }

        //二進制轉圖片(System.IO，System.Drawing)
        public System.Drawing.Image ReturnPhoto(byte[] streamByte)
        {
            MemoryStream ms = new MemoryStream(streamByte);
            Image img = Image.FromStream(ms);
            return img;
        }

        //圖片轉二進制，將Image轉換成流數據，並保存爲byte[] (System.IO，System.Drawing)
        public byte[] PhotoImageInsert(Image imgPhoto)
        {
            MemoryStream mstream = new MemoryStream();
            imgPhoto.Save(mstream, ImageFormat.Bmp);
            byte[] byData = new Byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length);
            mstream.Close();
            return byData;
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            string strpath = System.Environment.CurrentDirectory + @"\Parameter\EDID\";
            string svname;
            for (int svi = 0; svi < Form1.nvsender[0].regBoxMark.Length; svi++)
            {
                string[] svmk = Form1.nvsender[0].regBoxMark[svi].Split(',');
                svname = svmk[10] + "X" + svmk[11];

                byte[] binArr = new byte[256];

                foreach (var spath in Directory.GetFiles(strpath))
                {
                    if (spath.IndexOf(svname, 0) != -1) { lst_get1.Items.Insert(Convert.ToInt16(svmk[2]) - 1, "ID" + svmk[2] + ":" + svname + ",dy:" + svmk[16]); break; }
                }
            }
            lst_get1.Items.Insert(Form1.nvsender[0].regBoxMark.Length, "");
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            string v = "";
            if (btn_send.Tag == null || btn_send.Tag.ToString() == "")
            {
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    if (mp.InputBox("Screen Config", "『Send Settings to Screen 』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    if (mp.InputBox("大屏拼接", "是否執行『發送大屏設置』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    if (mp.InputBox("大屏拼接", "是否執行『发送大屏设置』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
                }
                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                {
                    if (mp.InputBox("大画面設定", "是否執行『全体の設定送信』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
                }
                pnl_busy.BringToFront();
                pnl_busy.Paint += new PaintEventHandler(pnl_busy_Paint);
                tme_busy.Enabled = true;
                matrixbox(lst_get1);
            }
            else if (btn_send.Tag.ToString() == "Read")
            {
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    if (mp.InputBox("Screen Config", "『Read Screen Settings 』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    if (mp.InputBox("大屏拼接", "是否執行『回讀大屏設置』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    if (mp.InputBox("大屏拼接", "是否執行『回读大屏设置』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
                }
                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                {
                    if (mp.InputBox("大画面設定", "是否執行『全体的な設定を読み返す』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
                }
                matrixRead(lst_get1);
            }
            pnl_busy.SendToBack();
            tme_busy.Enabled = false;
            pnl_busy.Paint -= new PaintEventHandler(pnl_busy_Paint);
            btn_send.Tag = "";
        }

        public static byte[] LT8668rd_arr = null;
        void matrixbox(ListBox lstget1)
        {
            Form1.ActiveForm.Enabled = false;
            lst_get1.Items.Clear();
            DialogResult svd = DialogResult.None;
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                mvars.errCode = "-1"; goto eX;
            }

        reDo:
            string strpath = System.Environment.CurrentDirectory + @"\Parameter\EDID\";
            if (Directory.Exists(strpath) == false) { mvars.errCode = "-2"; goto eX; }
            mp.markreset(9999, false);
            string svdeviceID = mvars.deviceID;
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            byte svFPGAsel = mvars.FPGAsel;
            mvars.FPGAsel = 2;
            byte svL = 1;

            #region 3. SERIES MODE
            lst_get1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, "  Series Mode ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, "  串接模式 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, "  串接模式 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, "  シリアルモード ▪▪▪"); }
            mvars.lblCmd = "PRIID_SERIESMODE";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mIDSERIESMODE();
            lst_get1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, "Series Mode ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, "串接模式 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, "串接模式 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, "シリアルモード ▪▪▪ 終了"); }
                lst_get1.Items.RemoveAt(0);
                lst_get1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-3"; goto eX; }
            #endregion 3. SERIES MODE

            #region 4. ID distribute
            lst_get1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, "  ID Distributing ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, "  ID 配置中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, "  ID 配置中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, "  ID 分配 ▪▪▪"); }
            mvars.lblCmd = "PRIID_AUTOID";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mAUTOID("1");
            lst_get1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lst_get1.Items.Insert(1, "ID Distribute ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lst_get1.Items.Insert(1, "ID 配置 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lst_get1.Items.Insert(1, "ID 配置 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lst_get1.Items.Insert(1, "ID 分配 ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(2);
                lstget1.Items.RemoveAt(0);
                lstget1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-4"; goto eX; }
            #endregion 4. ID distribute

            #region wait
            svCnt = 0;
            timer1.Enabled = true;
            if (MultiLanguage.DefaultLanguage == "en-US") { svd = mp.msgBox("", " Is the ID display as expected ?", 0, 0, 250, 100, 4); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svd = mp.msgBox("", " ID 顯示是否符合預期 ?", 0, 0, 250, 100, 4); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svd = mp.msgBox("", " ID 显示是否符合预期 ?", 0, 0, 250, 100, 4); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svd = mp.msgBox("", " Is the ID display as expected ?", 0, 0, 250, 100, 4); }
            do
            {
                if (svd != DialogResult.None)
                {
                    timer1.Enabled = false;
                    lbl_cnt.Text = "timer1_cnt";
                    break;
                }
                mp.doDelayms(300);
            }
            while (svd == DialogResult.None);
            if (svd == DialogResult.Cancel) goto eX;

            int svWaitSec;
            int svrow;
            if (svCnt < 8)
            {
                lstget1.Items.Insert(0, "▪▪▪▪➧");
                svWaitSec = 8 - svCnt;
                svrow = 1;
                if (mvars.demoMode) svWaitSec = 1;
                do
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒"); }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒"); }
                    Application.DoEvents();
                    mp.doDelayms(1000);
                    lstget1.Items.RemoveAt(svrow);
                    svWaitSec--;
                } while (svWaitSec > 0);
                lstget1.Items.RemoveAt(0);
            }
            #endregion wait

            #region 5. ID write
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Series Writing ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  串接寫入中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  串接写入中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  串接書き込み ▪▪▪"); }
            mvars.lblCmd = "PRIID_WRITEID";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mWRGETDEVID();
            lstget1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "Series Write ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "串接寫入 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "串接写入 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "串接書き込み ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(2);
                lstget1.Items.RemoveAt(0);
                lstget1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-5"; goto eX; }
            #endregion 5. ID write

            byte svredoCnt;

            #region 6. 各屏韌體版本回讀
            string[] svmk;
            string svver = "";
            svredoCnt = 0;
            for (int i = Form1.nvsender[0].regPoCards - 1; i >= 0; i--)
            {
                svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                //=========================================================================== deviceID
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(i + 1, 2);
                //===========================================================================
                lstget1.Items.Insert(0, "▪▪▪▪➧  ID" + (i + 1).ToString());

            redoFWread:
                #region MCU version regBoxMark[17]
                mvars.lblCmd = "MCU_VERSION";
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(300);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    mvars.verMCU = "Demo-1234-1234";
                }
                else
                {
                    mp.mhVersion();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                    {
                        mvars.verMCU = "-1";
                        if (mvars.in485 != 5)
                        {
                            if (mvars.in485 == 1 && mvars.inHDMI == 1) { }
                            else
                            {
                                if (svredoCnt > 2)
                                {
                                    mvars.errCode = "-6.1." + (i + 1).ToString(); goto eX;
                                }
                                else
                                {
                                    svredoCnt++;
                                    goto redoFWread;
                                }
                            }
                        }
                    }
                }
                if (mvars.verMCU != "-1")
                {
                    //App-230012-P0052,Boot-230221-0003
                    if (mvars.verMCU.Substring(0, 1) == "A") svver = "1" + mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4);
                    else if (mvars.verMCU.Substring(0, 1) == "B") svver = "2" + mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4);
                    else if (mvars.verMCU.Substring(0, 1) == "D") svver = "4" + mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4);
                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 18, svver);
                }
                #endregion MCU version

                #region FPGA version regBoxMark[18]
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                mvars.lblCmd = "FPGA_SPI_R";
                Form1.pvindex = 0;
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(300);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    svver = "FF.03-FF.02";
                }
                else
                {
                    mp.mhFPGASPIREAD();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    {
                        mvars.verFPGA = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                        svver = mvars.verFPGA;
                        svver = svver.Split('-')[0].Split('.')[1] + svver.Split('-')[1].Split('.')[1];
                        svver = svver.Replace('F', '0');
                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 19, svver);
                    }
                    else
                    {
                        mvars.verFPGA = "-1";
                        if (mvars.in485 != 5)
                        {
                            if (mvars.in485 == 1 && mvars.inHDMI == 1) { }
                            else
                            {
                                if (svredoCnt > 2)
                                {
                                    mvars.errCode = "-6.2." + (i + 1).ToString(); goto eX;
                                }
                                else
                                {
                                    svredoCnt++;
                                    goto redoFWread;
                                }
                            }
                        }
                    }
                }
                #endregion FPGA version

                #region EDID version regBoxMark[19]
                Byte[] arr = new byte[2]; //Byte[] rd_arr = new byte[2];

                uc_box.LT8668rd_arr = new byte[1];
                byte VerHi, VerLo;
                mvars.msgA = "1";
                mvars.lblCmd = "LT8668_Bin_WrRd";
                if (mvars.demoMode == false)
                {
                    arr[0] = 0x82; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) VerHi = mvars.ReadDataBuffer[7];
                    else VerHi = 255;
                    arr[0] = 0x83; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) VerLo = mvars.ReadDataBuffer[7];
                    else VerLo = 255;
                    if (VerHi == 255 && VerLo == 255)
                    {
                        if (mvars.in485 != 5)
                        {
                            if (mvars.in485 == 1 && mvars.inHDMI == 1)
                            {
                                svL = (byte)(1 + Form1.nvsender[0].regPoCards - i);
                                if (svL == 4)
                                {
                                    //只有4個屏
                                    mvars.errCode = "-6.255." + (i + 1).ToString();
                                    goto eX;
                                }
                            }
                            else
                            {
                                if (svredoCnt > 2)
                                {
                                    mvars.errCode = "-6.3." + (i + 1).ToString(); goto eX;
                                }
                                else
                                {
                                    svredoCnt++;
                                    goto redoFWread;
                                }
                            }
                        }
                        else
                        {
                            svL = (byte)(1 + Form1.nvsender[0].regPoCards - i);
                            if (svL == 4)
                            {
                                //只有4個屏
                                mvars.errCode = "-6.255." + (i + 1).ToString();
                                goto eX;
                            }
                        }
                    }
                    if (mvars.verMCU == "-1" && mvars.verFPGA == "-1" && VerHi == 255) lstget1.Items.Insert(1, "  is not exist");
                    else lstget1.Items.Insert(1, " " + mvars.verMCU.Split('-')[mvars.verMCU.Split('-').Length - 1] + "," + mvars.verFPGA + "," + VerHi + "." + VerLo);
                }
                else
                {
                    VerHi = 1;
                    VerLo = 16;
                    lstget1.Items.Insert(1, " P0052,61.71-61.70,1.16");
                }
                if (VerHi != 255 && VerLo != 255) Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 20, VerHi.ToString() + VerLo.ToString());
                #endregion EDID  version 
            }
            lstget1.Items.Insert(0, "");
            lstget1.Items.Insert(0, "");
            #endregion 6. 各屏韌體版本回讀

            //goto eX;

            #region 7. FPFA delay歸零與設定解析度(EDID)
            string svname;
            svredoCnt = 0;
            for (int i = Form1.nvsender[0].regPoCards - svL; i >= 0; i--)
            {
                svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                svname = svmk[10] + "X" + svmk[11];
                //=========================================================================== deviceID
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(i + 1, 2);
                //===========================================================================
                lstget1.Items.Insert(0, "▪▪▪▪➧  ID" + (i + 1).ToString());
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " Resolution " + svname + " adjustment ▪▪▪ "); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " 解析度 " + svname + " 調整中 ▪▪▪ "); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " 分辨率 " + svname + " 调整中 ▪▪▪ "); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " 解像度 " + svname + " 調整 ▪▪▪ "); }
                mp.doDelayms(50);

                mvars.lblCmd = "FPGA_SPI_W";
                #region Delay 單屏delay 回歸 0
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                Form1.pvindex = 65;
                lstget1.Items.Insert(1, "  R" + Form1.pvindex + "，0 ▪▪▪");
            refpgaR65:
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(300);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                }
                else
                {
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                    mp.doDelayms(500);
                }
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    lstget1.Items.RemoveAt(1);
                else
                {
                    if (svredoCnt > 2)
                    {
                        lstget1.Items.RemoveAt(1);
                        mvars.errCode = "-7.1," + (i + 1).ToString(); goto eX;
                    }
                    else
                    {
                        svredoCnt++;
                        goto refpgaR65;
                    }
                }
                #endregion Delay 單屏delay 回歸 0

                #region EDID
                UInt16 checksum;
                byte[] gBinArr = new byte[256];
                bool svload = false;
                lstget1.Items.Insert(1, "  EDID code load ▪▪▪");
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
                        lstget1.Items.RemoveAt(1);
                        lstget1.Items.Insert(1, "  checksum 0x" + checksum.ToString("X4"));
                        break;
                    }
                }
                mp.doDelayms(500);
                lstget1.Items.RemoveAt(1);
                if (svload)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  EDID Writing"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  EDID 寫入"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  EDID 写入"); }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  EDID 写入"); }
                    //↓ ConfPara_btn_Click
                    Byte[] arr = new byte[17];
                    //Configure Parameter
                    //Enable I2C  
                    mvars.lblCmd = "EnableI2C_Wr";
                    if (mvars.demoMode)
                    {
                        arr[0] = 0xFF; arr[1] = 0xE0;
                        arr[0] = 0xEE; arr[1] = 0x01;
                        arr[0] = 0x5E; arr[1] = 0xC1;
                        arr[0] = 0x58; arr[1] = 0x00;
                        arr[0] = 0x59; arr[1] = 0x50;
                        arr[0] = 0x5A; arr[1] = 0x10;
                        arr[0] = 0x5A; arr[1] = 0x00;
                        arr[0] = 0x58; arr[1] = 0x21;
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
                    lstget1.Items.RemoveAt(1);

                    #region Wait
                    svWaitSec = 2;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Please wait " + svWaitSec + "sec"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  請稍後 " + svWaitSec + "秒"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  请稍后 " + svWaitSec + "秒"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  待って " + svWaitSec + "秒"); }
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        lstget1.Items.RemoveAt(1);
                        svWaitSec--;
                    } while (svWaitSec > 0);
                    #endregion Wait

                    string OffsetAddrText = "070000";
                    lstget1.Items.Insert(1, "  Addr " + OffsetAddrText + " write");
                    //↓ BlockErase_btn_Click
                    arr = new byte[17];
                    byte[] flash_addr_arr = mp.StringToByteArray(OffsetAddrText);
                    //Block Erase
                    mvars.lblCmd = "Block_Erase_Bin_Wr";
                    byte svEarseByte = 0x02;    // 0x01(normal,half-block erase (32KB)) , 0x02 = Secter Erase (4KB) for protect DHCP
                    if (mvars.demoMode)
                    {
                        arr[0] = 0x5A; arr[1] = 0x04;
                        arr[0] = 0x5A; arr[1] = 0x00;
                        arr[0] = 0x5B; arr[1] = flash_addr_arr[0];
                        arr[0] = 0x5C; arr[1] = flash_addr_arr[1];
                        arr[0] = 0x5D; arr[1] = flash_addr_arr[2];
                        arr[0] = 0x5A; arr[1] = svEarseByte;
                        arr[0] = 0x5A; arr[1] = 0x00;
                    }
                    else
                    {
                        arr[0] = 0x5A; arr[1] = 0x04; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5B; arr[1] = flash_addr_arr[0]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[23:16] 
                        arr[0] = 0x5C; arr[1] = flash_addr_arr[1]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[15:8] 
                        arr[0] = 0x5D; arr[1] = flash_addr_arr[2]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[7:0] 
                        arr[0] = 0x5A; arr[1] = svEarseByte; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                    }

                    #region Wait 1 秒
                    svWaitSec = 1;
                    svrow = 2;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒"); }
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        lstget1.Items.RemoveAt(svrow);
                        svWaitSec--;
                    } while (svWaitSec > 0);
                    #endregion Wait 1 秒

                    lstget1.Items.RemoveAt(1);
                    lstget1.Items.Insert(1, "  EDID bin write");
                    //↓ Bin_Wr_Loop_btn_Click
                    mvars.lblCmd = "LT8668_Bin_Wr";
                    mp.LT8668_Bin_Wr_Loop(OffsetAddrText, gBinArr);      //070000,256
                    //WrDis_btn_Click
                    arr = new byte[17];
                    //WRDI
                    mvars.lblCmd = "WrDis_Bin_Wr";
                    if (mvars.demoMode)
                    {
                        arr[0] = 0x5A; arr[1] = 0x08;
                        arr[0] = 0x5A; arr[1] = 0x00;
                    }
                    else
                    {
                        arr[0] = 0x5A; arr[1] = 0x08; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                    }
                    lstget1.Items.RemoveAt(1);

                    //Scaler
                    arr = new byte[2];
                    //WRDI
                    mvars.lblCmd = "Scaler_Bin_Wr";
                    if (mvars.demoMode)
                    {
                        if (Form1.nvsender[0].regPoCards == 1)
                        {
                            lstget1.Items.Insert(1, "  Sacle ON");
                            //ON
                            arr[0] = 0xFF; arr[1] = 0xE0;
                            arr[0] = 0xEE; arr[1] = 0x01;
                            arr[0] = 0xFF; arr[1] = 0xE0;
                            arr[0] = 0xB0; arr[1] = 0x00;
                            arr[0] = 0xFF; arr[1] = 0xE0;
                            arr[0] = 0xEE; arr[1] = 0x00;
                        }
                        else
                        {
                            lstget1.Items.Insert(1, "  Sacle OFF");
                            //OFF
                            arr[0] = 0xFF; arr[1] = 0xE0;
                            arr[0] = 0xEE; arr[1] = 0x01;
                            arr[0] = 0xFF; arr[1] = 0xE0;
                            arr[0] = 0xB0; arr[1] = 0x01;
                            arr[0] = 0xFF; arr[1] = 0xE0;
                            arr[0] = 0xEE; arr[1] = 0x00;
                        }
                    }
                    else
                    {
                        if (Form1.nvsender[0].regPoCards == 1)
                        {
                            lstget1.Items.Insert(1, "  Sacle ON");
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
                            lstget1.Items.Insert(1, "  Sacle OFF");
                            //OFF
                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xEE; arr[1] = 0x01; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xB0; arr[1] = 0x01; mp.LT8668_Bin_Wr(0x86, 2, arr);    //Scale OFF
                            mp.doDelayms(100);
                            arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                            arr[0] = 0xEE; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);    //
                        }
                    }
                    #region Wait 2 秒
                    svWaitSec = 2;
                    svrow = 2;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒"); }
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        lstget1.Items.RemoveAt(svrow);
                        svWaitSec--;
                    } while (svWaitSec > 0);
                #endregion Wait 2 秒

                #region ScaleONOFF
                rescaleONOFF:
                    if (Form1.nvsender[0].regPoCards == 1)
                    {
                        mvars.lblCmd = "LT8668_SCALE ON";
                    }
                    else
                    {
                        mvars.lblCmd = "LT8668_SCALE OFF";
                    }
                    if (mvars.demoMode)
                    {
                        mvars.lCount++;
                        mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                        mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    }
                    else
                    {
                        if (Form1.nvsender[0].regPoCards == 1) mp.LT8668_ScaleONOPFF(0);
                        else mp.LT8668_ScaleONOPFF(1);
                    }
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                        lstget1.Items.RemoveAt(1); // "  Sacle OFF"
                    else
                    {
                        if (svredoCnt > 2)
                        {
                            lstget1.Items.RemoveAt(1);  // "  Sacle OFF"
                            mvars.errCode = "-7.3." + (i + 1).ToString();
                            goto eX;
                        }
                        else
                        {
                            svredoCnt++;
                            goto rescaleONOFF;
                        }
                    }

                    lstget1.Items.RemoveAt(1); // " Resolution " + svname + " adjustment ▪▪▪ "

                    #endregion ScaleONOFF
                }
                else
                {
                    mvars.errCode = "-7.2." + (i + 1).ToString();
                    goto eX;
                }
                #endregion EDID   

                if (mvars.demoMode == false)
                {
                    mvars.lblCmd = "PRIID_OFF";
                    mp.mpIDONOFF(0);
                }
                lstget1.Items.RemoveAt(0);  //ID ")
                mp.doDelayms(500);
                if (i == 0)
                {
                    lstget1.Items.RemoveAt(0);
                    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Resolution setup ▪▪▪ Done"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "解析度設定 ▪▪▪ 完成"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "分辨率设置 ▪▪▪ 完成"); }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "解像度の設定 ▪▪▪ 終了"); }
                }
            }
            lstget1.Items.Insert(0, "");
            #endregion 7. FPFA delay歸零與設定解析度(EDID)

            #region 8. RESET(Boardcast 24 sec)
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "EDID Reset ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "EDID リセット ▪▪▪ "); }
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            mvars.FPGAsel = 2;
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "LT8668_Reset_L";
                mp.LT8668_Reset_L();
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-8.1"; goto eX; }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 6;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  (1/2) Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  (1/2) 請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  (1/2) 请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  (1/2) 待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                //mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "LT8668_Reset_H";
                mp.LT8668_Reset_H();
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-8.2"; goto eX; }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 6;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  (2/2) Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  (2/2) 請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  (2/2) 请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  (2/2) 待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "EDID Reset ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "EDID リセット ▪▪▪ 終了"); }

            lstget1.Items.Insert(0, "");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Screen Reset ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "全て リセット ▪▪▪ "); }
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                //mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "MCU_SW_Rst";
                mp.McuSW_Reset();
            }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 12;
            if (mvars.demoMode) svWaitSec = 2;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            lstget1.Items.RemoveAt(0);  //Screen Reset ▪▪▪
            lstget1.Items.RemoveAt(0);  //""
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Screen Reset ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "全て リセット ▪▪▪ 終了"); }
            lstget1.Items.RemoveAt(1);
            #endregion 8. RESET(24 sec)

            #region 9. 設座標與單屏delay (重點 由最後屏到第一屏) lst_W6000
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Coordinate setting"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "座標設定中"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "座标设定中"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "座標設定"); }
            lstget1.Items.Insert(1, "");
            for (int i = Form1.nvsender[0].regPoCards - svL; i >= 0; i--)
            {
                svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                //=========================================================================== deviceID
                //mvars.deviceID = mvars.deviceID.Substring(0, 2) + svmk[2].PadLeft(2, '0');
                //mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svmk[2].PadLeft(2, '0')), 2);      //" 05" + mp.DecToHex(i, 2)
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(i + 1, 2);      //" 05" + mp.DecToHex(i, 2) 1225
                //===========================================================================
                lstget1.Items.Insert(0, "▪▪▪▪➧  ID" + (i + 1).ToString());

                #region 0x60000 Read (-..2)
                //List<string> arrW60K = new List<string>(new string[2048]);
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R60K Read▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  回讀 R60K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  回读 R60K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R60K 読む ▪▪▪"); }
                Form1.lstmcuW60000.Items.Clear();
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
                lstget1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9." + (i + 1).ToString() + ".1"; goto eX; }
                else
                {
                    //List<string> arrW60K = new List<string>(new string[2048]);
                    if (mvars.strR60K.Length > 1)
                    {
                        if (mvars.strR60K.Split('~').Length != 2048)
                        {
                            mvars.errCode = "-9." + (i + 1).ToString() + ".2";
                            mp.funSaveLogs("(Err) MCU 0x60000 content,ID" + mvars.deviceID + ",strR60K:" + mvars.strR60K);
                            mp.funSaveLogs("(Err)↑MCU 0x60000 content Content was cleared");
                            mvars.strR60K = "";
                            //goto eX; 
                        }
                        else Form1.lstmcuW60000.Items.AddRange(mvars.strR60K.Split('~'));
                    }
                    if (mvars.strR60K == "") for (int j = 0; j < 2048; j++) Form1.lstmcuW60000.Items.Add("0,0");
                }
                #endregion 0x60000 Read

                mvars.lblCmd = "FPGA_SPI_W";
                int svm;
                #region Delay 單屏設置 (-...3)
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                Form1.pvindex = 65;
                svm = Convert.ToInt32(svmk[5]);    //svsel (0 = 480,1440,,,)  (1 = 0,960,,,)
                svm %= 2160;
                lstget1.Items.Insert(1, "  R" + Form1.pvindex + "，" + svm.ToString() + " ▪▪▪");
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
                lstget1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                else { mvars.errCode = "-9." + (i + 1).ToString() + ".3"; goto eX; }
                #endregion Delay 單屏設置 

                #region X 座標(1=0,0=480)(1=960,0=1440) 分左右畫面 (-...4)
                for (int svsel = 1; svsel >= 0; svsel--)
                {
                    mp.doDelayms(200);
                    //==================== 分左右畫面
                    mvars.FPGAsel = (byte)svsel;
                    //==================== 
                    svm = Convert.ToInt32(svmk[12]) + Convert.ToInt32(svmk[6]) / 2 * (1 - svsel);    //svsel (0 = 480,1440,,,)  (1 = 0,960,,,)
                    svm %= 3840;
                    Form1.pvindex = 66;     //X
                    byte[] tmp = new byte[1];
                    tmp[0] = (byte)(svsel + 65);
                    lstget1.Items.Insert(1, "  R" + Form1.pvindex + "，FPGA " + System.Text.Encoding.ASCII.GetString(tmp) + "(" + svsel + ")，" + svm.ToString() + " ▪▪▪");
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
                    lstget1.Items.RemoveAt(1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                    else { mvars.errCode = "-9." + (i + 1).ToString() + ".4"; goto eX; }
                }
                #endregion X 座標(1=0,0=480)(1=960,0=1440) 分左右畫面

                #region Y 座標 單屏設置 (-...5)
                mp.doDelayms(200);
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                Form1.pvindex = 67;     //Y
                svm = Convert.ToInt32(svmk[13]);    //svsel (0 = 480,1440,,,)  (1 = 0,960,,,)
                svm %= 2160;
                lstget1.Items.Insert(1, "  R" + Form1.pvindex + "，" + svm.ToString() + " ▪▪▪");
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
                lstget1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                else { mvars.errCode = "-9." + (i + 1).ToString() + ".5"; goto eX; }
                #endregion Y 座標 單屏設置

                #region 0x60000 Write (-...6/.7)
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R60K Write▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  寫入 R60K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  写入 R60K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R60K 書く▪▪▪"); }
                if (Form1.lstmcuW60000.Items.Count > 0)
                {
                    int j = 0;

                    #region v0030 re-build and 1023-->1024
                    string svs = "";
                    #region R65 (Delay)              
                    for (j = 1023; j >= 0; j--) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("65,", 0) != -1) break; }
                    //if (j < 0) j = 0;
                    if (j < 0)
                    {
                        for (j = 0; j <= 1023; j++) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("0,0", 0) != -1) break; }
                        if (j > 1023) j = 0;
                    }
                    Form1.lstmcuW60000.Items.RemoveAt(j);
                    //Form1.lstmcuW60000.Items.Insert(j, "65," + svmk[5]);
                    svm = Convert.ToInt32(svmk[5]);    //svsel (0 = 480,1440,,,)  (1 = 0,960,,,)
                    svm %= 2160;
                    Form1.lstmcuW60000.Items.Insert(j, "65," + svm.ToString());
                    svs = "  R65," + svmk[5] + "-";

                    for (j = 2047; j >= 1024; j--) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("65,", 0) != -1) break; }
                    if (j < 1024)
                    {
                        for (j = 1024; j >= 2047; j++) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("0,0", 0) != -1) break; }
                        if (j > 2047) j = 1024;
                    }
                    Form1.lstmcuW60000.Items.RemoveAt(j);
                    //Form1.lstmcuW60000.Items.Insert(j, "65," + svmk[5]);
                    //svs += svmk[5] + "\r\n";
                    Form1.lstmcuW60000.Items.Insert(j, "65," + svm.ToString());
                    svs += svm.ToString() + "\r\n";
                    #endregion R65 (Delay)

                    #region R66 (X)
                    for (j = 1023; j >= 0; j--) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("66,", 0) != -1) break; }
                    if (j < 0)
                    {
                        for (j = 0; j <= 1023; j++) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("0,0", 0) != -1) break; }
                        if (j > 1023) j = 1;
                    }
                    svm = Convert.ToInt32(svmk[12]) + Convert.ToInt32(svmk[6]) / 2;
                    svm %= 3840;
                    Form1.lstmcuW60000.Items.RemoveAt(j);
                    Form1.lstmcuW60000.Items.Insert(j, "66," + svm.ToString());     //右畫面(FPGA A) 0 = 480,1440,2880
                    svs += "R66," + svm + "-";

                    for (j = 2047; j >= 1024; j--) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("66,", 0) != -1) break; }
                    if (j < 1024)
                    {
                        for (j = 1024; j <= 2047; j++) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("0,0", 0) != -1) break; }
                        if (j > 2047) j = 1024 + 1;
                    }
                    Form1.lstmcuW60000.Items.RemoveAt(j);
                    //Form1.lstmcuW60000.Items.Insert(j, "66," + svmk[12]);           //左畫面(FPGA B) 1 = 0,960,1920 
                    //svs += svmk[12] + "\r\n";
                    svm = Convert.ToInt32(svmk[12]);
                    svm %= 3840;
                    Form1.lstmcuW60000.Items.Insert(j, "66," + svm.ToString());           //左畫面(FPGA B) 1 = 0,960,1920 
                    svs += svm.ToString() + "\r\n";

                    #endregion R66 (X)

                    #region R67 (Y)
                    for (j = 1023; j >= 0; j--) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("67,", 0) != -1) break; }
                    if (j < 0)
                    {
                        for (j = 0; j <= 1023; j++) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("0,0", 0) != -1) break; }
                        if (j > 1023) j = 2;
                    }

                    Form1.lstmcuW60000.Items.RemoveAt(j);
                    //Form1.lstmcuW60000.Items.Insert(j, "67," + svmk[13]);
                    //svs += "R67," + svmk[13] + "-";
                    svm = Convert.ToInt32(svmk[13]);    //svsel (0 = 480,1440,,,)  (1 = 0,960,,,)
                    svm %= 2160;
                    Form1.lstmcuW60000.Items.Insert(j, "67," + svm.ToString());
                    svs += "R67," + svm.ToString() + "-";

                    for (j = 2047; j >= 1024; j--) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("67,", 0) != -1) break; }
                    if (j < 1024)
                    {
                        for (j = 1024; j <= 2047; j++) { if (Form1.lstmcuW60000.Items[j].ToString().IndexOf("0,0", 0) != -1) break; }
                        if (j > 2047) j = 1024 + 2;
                    }

                    Form1.lstmcuW60000.Items.RemoveAt(j);
                    //Form1.lstmcuW60000.Items.Insert(j, "67," + svmk[13]);
                    //svs += svmk[13];
                    Form1.lstmcuW60000.Items.Insert(j, "67," + svm.ToString());
                    svs += svm.ToString();
                    #endregion R67 (Y)

                    #endregion v0030 re-build and 1023-->1024
                }
                byte[] BinArr = new byte[mvars.GAMMA_SIZE];
                UInt16 checksum;
                for (int svi = 0; svi < 512; svi++)
                {
                    BinArr[svi * 4 + 0] = (Byte)(Convert.ToInt32(Form1.lstmcuW60000.Items[svi].ToString().Split(',')[0]) / 256);
                    BinArr[svi * 4 + 1] = (Byte)(Convert.ToInt32(Form1.lstmcuW60000.Items[svi].ToString().Split(',')[0]) % 256);
                    BinArr[svi * 4 + 2] = (Byte)(Convert.ToInt32(Form1.lstmcuW60000.Items[svi].ToString().Split(',')[1]) / 256);
                    BinArr[svi * 4 + 3] = (Byte)(Convert.ToInt32(Form1.lstmcuW60000.Items[svi].ToString().Split(',')[1]) % 256);
                    BinArr[svi * 4 + 0 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(Form1.lstmcuW60000.Items[svi + 1024].ToString().Split(',')[0]) / 256);
                    BinArr[svi * 4 + 1 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(Form1.lstmcuW60000.Items[svi + 1024].ToString().Split(',')[0]) % 256);
                    BinArr[svi * 4 + 2 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(Form1.lstmcuW60000.Items[svi + 1024].ToString().Split(',')[1]) / 256);
                    BinArr[svi * 4 + 3 + mvars.GAMMA_SIZE / 2] = (Byte)(Convert.ToInt32(Form1.lstmcuW60000.Items[svi + 1024].ToString().Split(',')[1]) % 256);
                }
                checksum = mp.CalChecksum(BinArr, 0, (UInt16)(BinArr.Length - 3));
                BinArr[BinArr.Length - 2] = (byte)(checksum / 256);
                BinArr[BinArr.Length - 1] = (byte)(checksum % 256);
                if (mvars.demoMode)
                {
                    mvars.lblCmd = "MCU_FLASH_W60000";
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                }
                else
                {
                    mvars.lblCmd = "MCU_FLASH_W60000";
                    mp.mhMCUFLASHWRITE("60000", ref BinArr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9." + (i + 1).ToString() + ".6"; goto eX; }
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
                lstget1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9." + (i + 1).ToString() + ".7"; goto eX; }
                else
                {
                    Form1.lstmcuR60000.Items.Clear();
                    Form1.lstmcuR60000.Items.AddRange(mvars.strR60K.Split('~'));
                }
                #endregion 0x60000 Write

                #region 0x66000 Read (-..8/.9)
                //List<string> arrW60K = new List<string>(new string[2048]);
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R66K Read▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  回讀 R66K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  回读 R66K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R66K 読む ▪▪▪"); }
                Form1.lstmcuW66000.Items.Clear();
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    mvars.strR66K = "";
                }
                else
                {
                    mvars.lblCmd = "MCU_FLASH_R66000";
                    mp.mhMCUFLASHREAD("66000".PadLeft(8, '0'), 8192);
                }
                lstget1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9." + (i + 1).ToString() + ".8"; goto eX; }
                else
                {
                    //List<string> arrW60K = new List<string>(new string[2048]);
                    if (mvars.strR66K.Length > 1)
                    {
                        if (mvars.strR66K.Split('~').Length != 2048)
                        {
                            mvars.errCode = "-9." + (i + 1).ToString() + ".9";
                            mp.funSaveLogs("(Err) MCU 0x66000 content,ID" + mvars.deviceID + ",strR66K:" + mvars.strR66K);
                            mp.funSaveLogs("(Err) MCU 0x66000 content Content was cleared ↑");
                            mvars.strR66K = "";
                            //goto eX; 
                        }
                        else Form1.lstmcuW66000.Items.AddRange(mvars.strR66K.Split('~'));
                    }
                    if (mvars.strR66K == "") for (int j = 0; j < 2048; j++) Form1.lstmcuW66000.Items.Add("0,0");
                }
                #endregion 0x66000 Read

                #region 0x66000 Write (-...10/.7)
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R66K Write▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  寫入 R66K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  写入 R66K ▪▪▪"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R66K 書く▪▪▪"); }
                string[] svmkf = Form1.nvsender[0].regBoxMark[i].Split(',');
                //v0037 0x66000改為從0開始寫入
                for (int j = 0; j < svmkf.Length; j++)
                {
                    Form1.lstmcuW66000.Items.RemoveAt(j);
                    Form1.lstmcuW66000.Items.Insert(j, j + "," + svmkf[j]);
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
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9." + (i + 1).ToString() + ".6"; goto eX; }
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
                    mvars.lblCmd = "MCU_FLASH_R66000";
                    mp.mhMCUFLASHREAD("66000", BinArr);
                }
                lstget1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9." + (i + 1).ToString() + ".7"; goto eX; }
                else
                {
                    Form1.lstmcuR66000.Items.Clear();
                    Form1.lstmcuR66000.Items.AddRange(mvars.strR66K.Split('~'));
                    mp.funSaveLogs("(Read) ID" + mvars.deviceID + ",strR66K," + mvars.strR66K);
                }
                #endregion 0x66000 Write

                lstget1.Items.RemoveAt(0);
            }
            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Coordinate setting ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "座標設定 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "座標設定 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "座標設定 ▪▪▪ 終了"); }
            #endregion 9. 設座標與單屏delay  (重點 由最後屏到第一屏)

            #region 10. RESET(14 sec)
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            mvars.FPGAsel = 2;
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Screen Reset ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "全て リセット ▪▪▪ "); }
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                //mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "MCU_SW_Rst";
                mp.McuSW_Reset();
            }

            #region wait(14 sec)
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 14;
            if (mvars.demoMode) svWaitSec = 2;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait(14 sec)

            if (mvars.demoMode == false)
            {
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                mvars.FPGAsel = 2;
                mvars.lblCmd = "PRIID_SHOW";
                mp.mpIDONOFF(1);
            }

            lstget1.Items.RemoveAt(0);  //Screen Reset ▪▪▪
            lstget1.Items.RemoveAt(0);  //""
            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            #endregion 8. RESET(14 sec)

            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;

            pnl_busy.SendToBack();

        eX:
            //設定本視窗獲得焦點
            if (Form1.frm1.Handle != Form1.GetF()) Form1.SetF(Form1.frm1.Handle);
            //設定本視窗獲得焦點
            if (svd != DialogResult.Cancel)
            {
                if (mvars.errCode != "0")
                {
                    //lstget1.Items.Insert(0, "▪▪▪▪➧ " + mvars.errCode);
                    //if (mvars.errCode == "-1")
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " H.W. Communication error"); }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " 硬體通訊錯誤"); }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " 硬体通讯错误"); }
                    //    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " ハードウェア 間違い"); }
                    //}
                    //if (mvars.errCode == "-2")
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " H.W. Communication error"); }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " 硬體通訊錯誤"); }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " 硬体通讯错误"); }
                    //    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " ハードウェア 間違い"); }
                    //}
                    //lstget1.Items.Insert(2, "");


                    string svs = " H.W. Communication error";
                    if (mvars.errCode == "-1" || mvars.errCode == "-2")
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { svs = " H.W. Communication error"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = " 硬體通訊錯誤"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = " 硬体通讯错误"; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svs = " ハードウェア 間違い"; }
                    }
                    else
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { svs = " Error Code : "; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = " 錯誤碼 : "; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = " 错误码 : "; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svs = " 异常码 : "; }
                        svs += mvars.errCode;
                    }
                    MessageBox.Show(svs);
                }
                else
                {
                    DialogResult svR = DialogResult.OK;
                    if (MultiLanguage.DefaultLanguage == "en-US")
                    {
                        svR = mp.msgBox("ScreenConfig", "Are you satisfied with the setting ? " + "\r\n" + "\r\n" + @"   Yes，run『Screen Uniformity Adjustment』 to align the quality differences of each single screen or 『Exit ScreenConfig』 " + "\r\n" + "\r\n" + "   No，re-do『Send Settings to Screen』", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 5);
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    {
                        svR = mp.msgBox("大屏拼接", "是否符合預期串接結果 ? " + "\r\n" + "\r\n" + @"   是，執行『大屏均勻性調整』來對齊各單屏畫質差異或離開大屏拼接" + "\r\n" + "\r\n" + "   否，重新『發送大屏設置』", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 5);
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    {
                        svR = mp.msgBox("大屏拼接", "是否符合预期串接结果 ? " + "\r\n" + "\r\n" + @"   是，执行『大屏均匀性调整』来对齐各单屏画质差异或离开大屏拼接" + "\r\n" + "\r\n" + "   否，重新『发送大屏设置』", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 5);
                    }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                    {
                        svR = mp.msgBox("全体の設定", "是否符合預期串接結果 ? " + "\r\n" + "\r\n" + @"   是，執行『大屏均勻性調整』來對齊各單屏畫質差異或離開大屏拼接" + "\r\n" + "\r\n" + "   No，re-do『Send Settings to Screen』", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 5);
                    }
                    if (svR == DialogResult.Cancel)
                    {
                        lstget1.Items.Clear();
                        goto reDo;
                    }
                }
                if (mvars.demoMode == false)
                {
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                    mvars.FPGAsel = 2;
                    mvars.lblCmd = "PRIID_OFF";
                    mp.mpIDONOFF(0);
                }
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "00";
            }
            if (mvars.demoMode == false) { mp.CommClose(); }
            Form1.ActiveForm.Enabled = true;
        }




        public static void matrixRead(ListBox lstget1)
        {
            Form1.ActiveForm.Enabled = false;
            lstget1.Items.Clear();
            string svdeviceID = mvars.deviceID;
            mvars.deviceID = "05A0";
            byte svFPGAsel = mvars.FPGAsel;
            mvars.FPGAsel = 2;
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                mvars.errCode = "-1"; goto eX;
            }

            string strpath = System.Environment.CurrentDirectory + @"\Parameter\EDID\";
            if (Directory.Exists(strpath) == false) { mvars.errCode = "-2"; goto eX; }
            mp.markreset(9999, false);   //次數在表單使用有效，mp則先隨便設。在表單執行 mvars.flgDelFB=false

            #region 3. SERIES MODE
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Series Mode ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  串接模式 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  串接模式 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  シリアルモード ▪▪▪"); }
            mvars.lblCmd = "PRIID_SERIESMODE";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mIDSERIESMODE();
            lstget1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "Series Mode ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "串接模式 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "串接模式 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "シリアルモード ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(0);
                lstget1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-3"; goto eX; }
            #endregion 3. SERIES MODE

            #region 4. ID distribute
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  ID Distributing ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  ID 配置中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  ID 配置中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  ID 分配 ▪▪▪"); }
            mvars.lblCmd = "PRIID_AUTOID";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mAUTOID("1");
            lstget1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "ID Distribute ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "ID 配置 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "ID 配置 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "ID 分配 ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(2);
                lstget1.Items.RemoveAt(0);
                lstget1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-4"; goto eX; }
            #endregion 4. ID distribute

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            int svWaitSec = 8;
            int svrow = 1;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(svrow);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            #region 5. ID write
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Series Writing ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  串接寫入中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  串接写入中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  串接書き込み ▪▪▪"); }
            mvars.lblCmd = "PRIID_WRITEID";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mWRGETDEVID();
            lstget1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "Series Write ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "串接寫入 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "串接写入 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "串接書き込み ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(2);
                lstget1.Items.RemoveAt(0);
                lstget1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-5"; goto eX; }
            #endregion 5. ID write

            #region 6. 各屏韌體版本回讀
            string[] svmk = null;
            string svver = "";
            for (int i = Form1.nvsender[0].regPoCards - 1; i >= 0; i--)
            {
                svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                //=========================================================================== deviceID
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + svmk[2].PadLeft(2, '0');
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(Convert.ToInt16(svmk[2].PadLeft(2, '0')), 2);      //" 05" + mp.DecToHex(i, 2)
                //===========================================================================
                lstget1.Items.Insert(0, "▪▪▪▪➧  ID" + svmk[2]);

                #region MCU version regBoxMark[17]
                mvars.lblCmd = "MCU_VERSION";
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(300);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    mvars.verMCU = "demo-demo-demo";
                }
                else
                {
                    mp.mhVersion();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) > -1)
                    {
                        //lstget1.Items.Insert(1, " MCU " + mvars.verMCU);
                    }
                    else { mvars.errCode = "  ID" + svmk[2] + "-6." + svmk[2] + ".1"; goto eX; }
                }
                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 18, mvars.verMCU);
                #endregion MCU version

                #region FPGA version regBoxMark[18]
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                mvars.lblCmd = "FPGA_SPI_R";
                Form1.pvindex = 0;
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(300);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    svver = "d000,d001";
                }
                else
                {
                    mp.mhFPGASPIREAD();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    {
                        svver = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                    }
                    else { mvars.errCode = "  ID" + svmk[2] + "-6." + svmk[2] + ".2"; goto eX; }
                }
                mvars.verFPGA = svver;
                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 19, svver);
                //lstget1.Items.Insert(2, " FPGA " + svver);

                #endregion FPGA version

                #region EDID version regBoxMark[19]
                Byte[] arr = new byte[2]; //Byte[] rd_arr = new byte[2];


                //LT8668rd_arr = new byte[1];
                //mvars.msgA = "1";
                //mvars.lblCmd = "LT8668_Bin_WrRd";
                //arr[0] = 0x82; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref LT8668rd_arr);
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                //{
                //    svver = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1] + "h";
                //}
                //else { mvars.errCode = "  ID" + svmk[2] + "-6." + svmk[2] + ".3"; goto eX; }
                //arr[0] = 0x83; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref LT8668rd_arr);
                //if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                //{
                //    svver += "," + mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1] + "h";
                //}
                //else { mvars.errCode = "  ID" + svmk[2] + "-6." + svmk[2] + ".4"; goto eX; }


                uc_box.LT8668rd_arr = new byte[1];
                byte VerHi = 0, VerLo = 0;
                mvars.msgA = "1";
                mvars.lblCmd = "LT8668_Bin_WrRd";
                if (mvars.demoMode == false)
                {
                    arr[0] = 0x82; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    {
                        VerHi = mvars.ReadDataBuffer[7];
                        mvars.lblCmd = "LT8668_Bin_WrRd";
                        arr[0] = 0x83; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                        if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) VerLo = mvars.ReadDataBuffer[7];
                        else { mvars.errCode = "  ID" + svmk[2] + " -6." + svmk[2] + ".4"; goto eX; }
                    }
                    else { mvars.errCode = "  ID" + svmk[2] + " -6." + svmk[2] + ".3"; goto eX; }
                    lstget1.Items.Insert(1, " " + mvars.verMCU.Split('-')[mvars.verMCU.Split('-').Length - 1] + "," + mvars.verFPGA + "," + VerHi + "." + VerLo);
                }
                else
                {
                    VerHi = 1;
                    VerLo = 16;
                    lstget1.Items.Insert(1, " P0052,61.71-61.70,1.16");
                }
                Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 20, VerHi + "." + VerLo);


                #endregion EDID  version 

            }
            #endregion 6. 各屏韌體版本回讀



            #region 10. 0x66000 Read (v0030)
            List<string> arrW66K = new List<string>(new string[2048]);
            mvars.strR66K = "";
            Form1.lstmcuW66000.Items.Clear();
            Form1.lstmcuR66000.Items.Clear();
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R66K Read▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  回讀 R66K ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  回读 R66K ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R66K 読む ▪▪▪"); }
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                mvars.strR66K = "";
            }
            else
            {
                mvars.lblCmd = "MCU_FLASH_R66000";
                mp.mhMCUFLASHREAD("66000".PadLeft(8, '0'), 8192);
            }
            lstget1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-10." + svmk[2] + ".1"; goto eX; }
            else
            {
                if (mvars.strR66K.Length > 1)
                {
                    if (mvars.strR66K.Split('~').Length != 2048) { mvars.errCode = "-10." + svmk[2] + ".2"; goto eX; }
                    else Form1.lstmcuW66000.Items.AddRange(mvars.strR66K.Split('~'));
                }
                else for (int j = 0; j < 2048; j++) Form1.lstmcuW66000.Items.Add("0,0");
            }
            #endregion 10. 0x66000 Read (v0030)



            lstget1.Items.Insert(0, "");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Screen config Read ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "回讀大屏設置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "回读大屏设置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "全体的な設定を読み返す ▪▪▪ 終了"); }


            if (mvars.demoMode == false)
            {
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                mvars.FPGAsel = 2;
                mvars.lblCmd = "PRIID_SHOW";
                mp.mpIDONOFF(1);
            }
        //if (mvars.demoMode == false) { mp.CommClose(); }

        eX:
            //設定本視窗獲得焦點
            if (Form1.frm1.Handle != Form1.GetF()) Form1.SetF(Form1.frm1.Handle);
            //設定本視窗獲得焦點

            if (mvars.errCode != "0")
            {
                //lstget1.Items.Insert(0, "▪▪▪▪➧ " + mvars.errCode);
                //if (mvars.errCode == "-1")
                //{
                //    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " H.W. Communication error"); }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " 硬體通訊錯誤"); }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " 硬体通讯错误"); }
                //    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " ハードウェア 間違い"); }
                //}
                //if (mvars.errCode == "-2")
                //{
                //    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " H.W. Communication error"); }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " 硬體通訊錯誤"); }
                //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " 硬体通讯错误"); }
                //    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " ハードウェア 間違い"); }
                //}
                //lstget1.Items.Insert(2, "");

                string svs = " H.W. Communication error";
                if (mvars.errCode == "-1" || mvars.errCode == "-2")
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { svs = " H.W. Communication error"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = " 硬體通訊錯誤"; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = " 硬体通讯错误"; }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { svs = " ハードウェア 間違い"; }
                }
                else
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { svs = " Error Code : "; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = " 錯誤碼 : "; }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = " 错误码 : "; }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { svs = " 异常码 : "; }
                    svs += mvars.errCode;
                }
                MessageBox.Show(svs);
            }
            if (mvars.demoMode == false)
            {
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                mvars.FPGAsel = 2;
                mvars.lblCmd = "PRIID_OFF";
                mp.mpIDONOFF(0);
            }
            mvars.deviceID = svdeviceID;
            mvars.FPGAsel = svFPGAsel;
            if (mvars.demoMode == false) { mp.CommClose(); }
            Form1.ActiveForm.Enabled = true;
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

        private void btn_single_Click(object sender, EventArgs e)
        {
            string v = "";
            if (MultiLanguage.DefaultLanguage == "en-US")
            {
                if (mp.InputBox("Screen Config", "『Restore Stand Alone 』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT")
            {
                if (mp.InputBox("大屏拼接", "是否執行『復原單屏』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
            }
            else if (MultiLanguage.DefaultLanguage == "zh-CN")
            {
                if (mp.InputBox("大屏拼接", "是否執行『复原单屏』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
            }
            else if (MultiLanguage.DefaultLanguage == "ja-JP")
            {
                if (mp.InputBox("大画面設定", "是否執行『単一画面に戻す』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;
            }
            pnl_busy.BringToFront();
            pnl_busy.Paint += new PaintEventHandler(pnl_busy_Paint);
            tme_busy.Enabled = true;
            independentbox(lst_get1);
            mp.doDelayms(100);
            pnl_busy.SendToBack();
            tme_busy.Enabled = false;
            pnl_busy.Paint -= new PaintEventHandler(pnl_busy_Paint);
        }


        void independentbox(ListBox lstget1)
        {
            Form1.ActiveForm.Enabled = false;
            lstget1.Items.Clear();
            DialogResult svd = DialogResult.None;
            string svdeviceID = mvars.deviceID;
            byte svFPGAsel = mvars.FPGAsel;
            if (mvars.demoMode == false && mp.Sp1open(Form1.tslblCOM.Text).Substring(0, 5) == "false")
            {
                if (mvars.deviceID == "0310") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                if (mvars.deviceID == "0300") { MessageBox.Show("Please select single XB", mvars.strUInameMe + "_v" + mvars.UImajor); return; }
                Form1.tslblHW.Text = "232"; Form1.tslblHW.BackColor = Control.DefaultBackColor; Form1.tslblHW.ForeColor = Color.Black;
                Form1.tslblCOM.ForeColor = Color.Red;
                mvars.errCode = "-1"; goto eX;
            }

            string strpath = System.Environment.CurrentDirectory + @"\Parameter\EDID\";
            if (Directory.Exists(strpath) == false) { mvars.errCode = "-2"; goto eX; }
            mp.markreset(9999, false);   //次數在表單使用有效，mp則先隨便設。在表單執行 mvars.flgDelFB=false
            mvars.deviceID = "05A0";
            mvars.FPGAsel = 2;
            byte svL = 1;

            #region 3. SERIES MODE
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " Series Mode ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " 串接模式 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " 串接模式 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " シリアルモード ▪▪▪"); }
            mvars.lblCmd = "PRIID_SERIESMODE";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mIDSERIESMODE();
            lstget1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "Series Mode ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "串接模式 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "串接模式 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "シリアルモード ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(0);
                lstget1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-3"; goto eX; }
            #endregion 3. SERIES MODE

            #region 4. ID distribute
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " ID Distributing ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " ID 配置中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " ID 配置中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " ID 分配 ▪▪▪"); }
            mvars.lblCmd = "PRIID_AUTOID";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mAUTOID("1");
            lstget1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "ID Distribute ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "ID 配置 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "ID 配置 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "ID 分配 ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(2);
                lstget1.Items.RemoveAt(0);
                lstget1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-4"; goto eX; }
            #endregion 4. ID distribute

            #region wait
            svCnt = 0;
            timer1.Enabled = true;
            if (MultiLanguage.DefaultLanguage == "en-US") { svd = mp.msgBox("", " Is the ID display as expected ?", 0, 0, 250, 100, 4); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svd = mp.msgBox("", " ID 顯示是否符合預期 ?", 0, 0, 250, 100, 4); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { svd = mp.msgBox("", " ID 显示是否符合预期 ?", 0, 0, 250, 100, 4); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { svd = mp.msgBox("", " Is the ID display as expected ?", 0, 0, 250, 100, 4); }
            do
            {
                if (svd != DialogResult.None)
                {
                    timer1.Enabled = false;
                    lbl_cnt.Text = "timer1_cnt";
                    break;
                }
                mp.doDelayms(300);
            }
            while (svd == DialogResult.None);
            if (svd == DialogResult.Cancel) goto eX;

            int svWaitSec;
            int svrow;
            if (svCnt < 8)
            {
                lstget1.Items.Insert(0, "▪▪▪▪➧");
                svWaitSec = 8 - svCnt;
                svrow = 1;
                if (mvars.demoMode) svWaitSec = 1;
                do
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒"); }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒"); }
                    Application.DoEvents();
                    mp.doDelayms(1000);
                    lstget1.Items.RemoveAt(svrow);
                    svWaitSec--;
                } while (svWaitSec > 0);
                lstget1.Items.RemoveAt(0);
            }
            #endregion wait

            #region 5. ID write
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Series Writing ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  串接寫入中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  串接写入中 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  串接書き込み ▪▪▪"); }
            mvars.lblCmd = "PRIID_WRITEID";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mWRGETDEVID();
            lstget1.Items.RemoveAt(1);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "Series Write ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "串接寫入 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "串接写入 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "串接書き込み ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(2);
                lstget1.Items.RemoveAt(0);
                lstget1.Items.Insert(0, "");
            }
            else { mvars.errCode = "-5"; goto eX; }
            #endregion 5. ID write

            byte svredoCnt;

            #region 6. 各屏韌體版本回讀
            string[] svmk;
            string svver = "";
            svredoCnt = 0;
            for (int i = Form1.nvsender[0].regPoCards - 1; i >= 0; i--)
            {
                svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                //=========================================================================== deviceID
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(i + 1, 2);
                //===========================================================================
                lstget1.Items.Insert(0, "▪▪▪▪➧  ID" + (i + 1).ToString());

            redoFWread:
                #region MCU version regBoxMark[17]
                mvars.lblCmd = "MCU_VERSION";
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(300);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    mvars.verMCU = "Demo-1234-1234";
                }
                else
                {
                    mp.mhVersion();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) == -1)
                    {
                        mvars.verMCU = "-1";
                        if (mvars.in485 != 5)
                        {
                            if (mvars.in485 == 1 && mvars.inHDMI == 1) { }
                            else
                            {
                                if (svredoCnt > 2)
                                {
                                    mvars.errCode = "-6.1." + (i + 1).ToString(); goto eX;
                                }
                                else
                                {
                                    svredoCnt++;
                                    goto redoFWread;
                                }
                            }
                        }
                    }
                }
                if (mvars.verMCU != "-1")
                {
                    //App-230012-P0052,Boot-230221-0003
                    if (mvars.verMCU.Substring(0, 1) == "A") svver = "1" + mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4);
                    else if (mvars.verMCU.Substring(0, 1) == "B") svver = "2" + mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4);
                    else if (mvars.verMCU.Substring(0, 1) == "D") svver = "4" + mvars.verMCU.Substring(mvars.verMCU.Length - 4, 4);
                    Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 18, svver);
                }
                #endregion MCU version

                #region FPGA version regBoxMark[18]
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                mvars.lblCmd = "FPGA_SPI_R";
                Form1.pvindex = 0;
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(300);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    svver = "FF.03-FF.02";
                }
                else
                {
                    mp.mhFPGASPIREAD();
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
                    {
                        mvars.verFPGA = mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1];
                        svver = mvars.verFPGA;
                        svver = svver.Split('-')[0].Split('.')[1] + svver.Split('-')[1].Split('.')[1];
                        svver = svver.Replace('F', '0');
                        Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 19, svver);
                    }
                    else
                    {
                        mvars.verFPGA = "-1";
                        if (mvars.in485 != 5)
                        {
                            if (mvars.in485 == 1 && mvars.inHDMI == 1) { }
                            else
                            {
                                if (svredoCnt > 2)
                                {
                                    mvars.errCode = "-6.2." + (i + 1).ToString(); goto eX;
                                }
                                else
                                {
                                    svredoCnt++;
                                    goto redoFWread;
                                }
                            }
                        }
                    }
                }
                #endregion FPGA version

                #region EDID version regBoxMark[19]
                Byte[] arr = new byte[2]; //Byte[] rd_arr = new byte[2];

                uc_box.LT8668rd_arr = new byte[1];
                byte VerHi, VerLo;
                mvars.msgA = "1";
                mvars.lblCmd = "LT8668_Bin_WrRd";
                if (mvars.demoMode == false)
                {
                    arr[0] = 0x82; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) VerHi = mvars.ReadDataBuffer[7];
                    else VerHi = 255;
                    arr[0] = 0x83; mp.LT8668_Bin_WrRd(0x86, 1, arr, 1, ref uc_box.LT8668rd_arr);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) VerLo = mvars.ReadDataBuffer[7];
                    else VerLo = 255;
                    if (VerHi == 255 && VerLo == 255)
                    {
                        if (mvars.in485 != 5)
                        {
                            if (mvars.in485 == 1 && mvars.inHDMI == 1)
                            {
                                svL = (byte)(1 + Form1.nvsender[0].regPoCards - i);
                                if (svL == 4)
                                {
                                    //只有4個屏
                                    mvars.errCode = "-6.254." + (i + 1).ToString();
                                    goto eX;
                                }
                            }
                            else
                            {
                                if (svredoCnt > 2)
                                {
                                    mvars.errCode = "-6.3." + (i + 1).ToString(); goto eX;
                                }
                                else
                                {
                                    svredoCnt++;
                                    goto redoFWread;
                                }
                            }
                        }
                        else
                        {
                            svL = (byte)(1 + Form1.nvsender[0].regPoCards - i);
                            if (svL == 4)
                            {
                                //只有4個屏
                                mvars.errCode = "-6.255." + (i + 1).ToString();
                                goto eX;
                            }
                        }
                    }
                    if (mvars.verMCU == "-1" && mvars.verFPGA == "-1" && VerHi == 255) lstget1.Items.Insert(1, "  is not exist");
                    else lstget1.Items.Insert(1, " " + mvars.verMCU.Split('-')[mvars.verMCU.Split('-').Length - 1] + "," + mvars.verFPGA + "," + VerHi + "." + VerLo);
                }
                else
                {
                    VerHi = 1;
                    VerLo = 16;
                    lstget1.Items.Insert(1, " P0052,61.71-61.70,1.16");
                }
                if (VerHi != 255 && VerLo != 255) Form1.nvsender[0].regBoxMark[i] = mp.replaceBoxMark(Form1.nvsender[0].regBoxMark[i], 20, VerHi.ToString() + VerLo.ToString());
                #endregion EDID  version 
            }
            lstget1.Items.Insert(0, "");
            lstget1.Items.Insert(0, "");
            #endregion 6. 各屏韌體版本回讀

            svredoCnt = 0;

            #region 8. Boardcast RESET
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "EDID Reset ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "EDID リセット ▪▪▪ "); }
            lstget1.Items.Insert(1, "");
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            mvars.FPGAsel = 2;
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "LT8668_Reset_L";
                mp.LT8668_Reset_L();
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-8.1"; goto eX; }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 6;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  (1/2) Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  (1/2) 請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  (1/2) 请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  (1/2) 待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "LT8668_Reset_H";
                mp.LT8668_Reset_H();
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-7.2"; goto eX; }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 6;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  (2/2) Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  (2/2) 請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  (2/2) 请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  (2/2) 待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "EDID Reset ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "EDID リセット ▪▪▪ 終了"); }

            lstget1.Items.Insert(0, "");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Screen Reset ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "全て リセット ▪▪▪ "); }
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                //mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "MCU_SW_Rst";
                mp.McuSW_Reset();
            }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 12;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            lstget1.Items.RemoveAt(0);
            //if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Screen Reset ▪▪▪ Done"); }
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ 完成"); }
            //else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ 完成"); }
            //else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "全て リセット ▪▪▪ 終了"); }
            #endregion 8. Boardcast RESET


            byte[] BinArr;
            #region 6. 回讀0x66000(v0037改為不需要回讀) FPGA設回單屏座標(0,0)，FPGA單屏delay歸零，寫入單屏EDID，SacleOFF (重點 由最後屏到第一屏)
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, " Coordinate / EDID / Sacle reset"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "座標 / EDID / Scale 回復單屏設定"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "座标 / EDID / Scale 回復單屏設定"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "座標 / EDID / Scale 單屏設定戻す"); }
            for (int i = Form1.nvsender[0].regPoCards - svL; i >= 0; i--)
            {
                svmk = Form1.nvsender[0].regBoxMark[i].Split(',');
                //=========================================================================== deviceID
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + mp.DecToHex(i + 1, 2);      //" 05" + mp.DecToHex(i, 2) 1225
                //===========================================================================
                Form1.lstmcuW60000.Items.Clear();

                lstget1.Items.Insert(0, "▪▪▪▪➧  ID" + (i + 1).ToString());

                //0x60000 Read(DIsabled)(查閱請參考 1219)

                //0x66000 Read (v0030) (v0037 disable)

                #region EDID
                UInt16 checksum;
                byte[] gBinArr = new byte[256];
                bool svload = false;
                //string svname = svmk[10] + "X" + svmk[11];
                string svname = svmk[6] + "X" + svmk[7];
                lstget1.Items.Insert(1, "  EDID " + svname + " ▪▪▪");
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
                        lstget1.Items.RemoveAt(1);
                        lstget1.Items.Insert(1, "  checksum 0x" + checksum.ToString("X4"));
                        break;
                    }
                }
                mp.doDelayms(500);
                lstget1.Items.RemoveAt(1);
                if (svload)
                {
                    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " EDID Writing"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " EDID 寫入"); }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " EDID 写入"); }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " EDID 写入"); }
                    //↓ ConfPara_btn_Click
                    Byte[] arr = new byte[17];
                    //Configure Parameter
                    //Enable I2C  
                    mvars.lblCmd = "EnableI2C_Wr";
                    if (mvars.demoMode)
                    {
                        arr[0] = 0xFF; arr[1] = 0xE0;
                        arr[0] = 0xEE; arr[1] = 0x01;
                        arr[0] = 0x5E; arr[1] = 0xC1;
                        arr[0] = 0x58; arr[1] = 0x00;
                        arr[0] = 0x59; arr[1] = 0x50;
                        arr[0] = 0x5A; arr[1] = 0x10;
                        arr[0] = 0x5A; arr[1] = 0x00;
                        arr[0] = 0x58; arr[1] = 0x21;
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
                    lstget1.Items.RemoveAt(1);

                    #region Wait 1 秒
                    svWaitSec = 1;
                    svrow = 1;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒 - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒 - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒 - LT8668"); }
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        lstget1.Items.RemoveAt(svrow);
                        svWaitSec--;
                    } while (svWaitSec > 0);
                    #endregion Wait 1 秒

                    string OffsetAddrText = "070000";
                    lstget1.Items.Insert(1, " Addr " + OffsetAddrText);
                    //↓ BlockErase_btn_Click
                    arr = new byte[17];
                    byte[] flash_addr_arr = mp.StringToByteArray(OffsetAddrText);
                    //Block Erase
                    mvars.lblCmd = "Block_Erase_Bin_Wr";
                    byte svEarseByte = 0x02;    // 0x01(normal,half-block erase (32KB)) , 0x02 = Secter Erase (4KB) for protect DHCP
                    if (mvars.demoMode)
                    {
                        arr[0] = 0x5A; arr[1] = 0x04;
                        arr[0] = 0x5A; arr[1] = 0x00;
                        arr[0] = 0x5B; arr[1] = flash_addr_arr[0];
                        arr[0] = 0x5C; arr[1] = flash_addr_arr[1];
                        arr[0] = 0x5D; arr[1] = flash_addr_arr[2];
                        arr[0] = 0x5A; arr[1] = svEarseByte;
                        arr[0] = 0x5A; arr[1] = 0x00;
                    }
                    else
                    {
                        arr[0] = 0x5A; arr[1] = 0x04; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5B; arr[1] = flash_addr_arr[0]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[23:16] 
                        arr[0] = 0x5C; arr[1] = flash_addr_arr[1]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[15:8] 
                        arr[0] = 0x5D; arr[1] = flash_addr_arr[2]; mp.LT8668_Bin_Wr(0x86, 2, arr);  // flash address[7:0] 
                        arr[0] = 0x5A; arr[1] = svEarseByte; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                    }
                    lstget1.Items.RemoveAt(1);

                    #region Wait 1 秒
                    svWaitSec = 1;
                    svrow = 1;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  BlockErase Please wait " + svWaitSec + "sec"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  BlockErase 請稍後 " + svWaitSec + "秒"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  BlockErase 请稍后 " + svWaitSec + "秒"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  BlockErase 待って " + svWaitSec + "秒"); }
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        lstget1.Items.RemoveAt(svrow);
                        svWaitSec--;
                    } while (svWaitSec > 0);
                    #endregion Wait 1 秒

                    //↓ Bin_Wr_Loop_btn_Click
                    mvars.lblCmd = "LT8668_Bin_Wr";
                    mp.LT8668_Bin_Wr_Loop(OffsetAddrText, gBinArr);      //070000,256

                    #region Wait 1 秒
                    svWaitSec = 1;
                    svrow = 1;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒 - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒 - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒 - LT8668"); }
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        lstget1.Items.RemoveAt(svrow);
                        svWaitSec--;
                    } while (svWaitSec > 0);
                    #endregion Wait 1 秒

                    //WrDis_btn_Click
                    arr = new byte[17];
                    //WRDI
                    mvars.lblCmd = "WrDis_Bin_Wr";
                    if (mvars.demoMode)
                    {
                        arr[0] = 0x5A; arr[1] = 0x08;
                        arr[0] = 0x5A; arr[1] = 0x00;
                    }
                    else
                    {
                        arr[0] = 0x5A; arr[1] = 0x08; mp.LT8668_Bin_Wr(0x86, 2, arr); //mp.doDelayms(100);
                        arr[0] = 0x5A; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr); //mp.doDelayms(100);
                    }

                    #region Wait 1 秒
                    svWaitSec = 1;
                    svrow = 1;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒 - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒 - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒 - LT8668"); }
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        lstget1.Items.RemoveAt(svrow);
                        svWaitSec--;
                    } while (svWaitSec > 0);
                    #endregion Wait 1 秒

                    //Scaler
                    arr = new byte[2]; //Byte[] rd_arr = new byte[2];
                    //WRDI
                    mvars.lblCmd = "Scaler_Bin_Wr";
                    if (mvars.demoMode)
                    {
                        lstget1.Items.Insert(1, " Sacle ON");
                        //ON
                        arr[0] = 0xFF; arr[1] = 0xE0;
                        arr[0] = 0xEE; arr[1] = 0x01;
                        arr[0] = 0xFF; arr[1] = 0xE0;
                        arr[0] = 0xB0; arr[1] = 0x00;
                        arr[0] = 0xFF; arr[1] = 0xE0;
                        arr[0] = 0xEE; arr[1] = 0x00;
                    }
                    else
                    {
                        lstget1.Items.Insert(1, " Sacle ON");
                        //ON
                        arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0xEE; arr[1] = 0x01; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0xB0; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0xFF; arr[1] = 0xE0; mp.LT8668_Bin_Wr(0x86, 2, arr);
                        arr[0] = 0xEE; arr[1] = 0x00; mp.LT8668_Bin_Wr(0x86, 2, arr);
                    }
                    lstget1.Items.RemoveAt(1);

                    #region Wait 5 秒
                    svWaitSec = 5;
                    svrow = 1;
                    do
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(svrow, "  Please wait " + svWaitSec + "sec - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(svrow, "  請稍後 " + svWaitSec + "秒 - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(svrow, "  请稍后 " + svWaitSec + "秒 - LT8668"); }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(svrow, "  待って " + svWaitSec + "秒 - LT8668"); }
                        Application.DoEvents();
                        mp.doDelayms(1000);
                        lstget1.Items.RemoveAt(svrow);
                        svWaitSec--;
                    } while (svWaitSec > 0);
                    #endregion Wait 2 秒

                    #region ScaleONOFF
                    lstget1.Items.Insert(1, "  Scale ON ▪▪▪");
                    mvars.lblCmd = "LT8668_SCALE ON";
                    if (mvars.demoMode)
                    {
                        mvars.lCount++;
                        mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                        mp.doDelayms(1000);
                        mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                    }
                    else
                    {
                        mp.LT8668_ScaleONOPFF(0);
                    }
                    lstget1.Items.RemoveAt(1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1)
                    {
                        mvars.errCode = "-6.4." + (i + 1).ToString(); goto eX;
                    }
                    #endregion ScaleONOFF
                }
                else
                {
                    { mvars.errCode = "-6.5." + (i + 1).ToString(); goto eX; }
                }
                #endregion EDID   

                mvars.lblCmd = "FPGA_SPI_W";
                #region Delay 單屏設置
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                Form1.pvindex = 65;
                lstget1.Items.Insert(1, "  Delay，R" + Form1.pvindex + "，0 ▪▪▪");
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                }
                else
                {
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                }
                lstget1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                else { mvars.errCode = "-6.6." + (i + 1).ToString(); goto eX; }
                #endregion Delay 

                #region X 座標(1=0,0=480)分左右畫面
                for (int svsel = 1; svsel >= 0; svsel--)
                {
                    //==================== 分左右畫面
                    mvars.FPGAsel = (byte)svsel;
                    //==================== 
                    //int svm = Convert.ToInt32(svmk[4]) + Convert.ToInt32(svmk[6]) / 2 * (1 - svsel);    //svsel=0:480,1440  svsel=1:0,960
                    int svm = Convert.ToInt32(svmk[6]) / 2 * (1 - svsel);    //svsel=0:480  svsel=1:0 (還原的話就只有這2個值)
                    Form1.pvindex = 66;     //X
                    byte[] tmp = new byte[1];
                    tmp[0] = (byte)(svsel + 65);
                    lstget1.Items.Insert(1, "  X，R" + Form1.pvindex + "，FPGA." + System.Text.Encoding.ASCII.GetString(tmp) + "，" + svm + " ▪▪▪");
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
                    lstget1.Items.RemoveAt(1);
                    if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                    else { mvars.errCode = "-6.7." + (i + 1).ToString(); goto eX; }
                }
                #endregion X 座標(1=0,0=480)分左右畫面

                #region Y 座標 單屏設置
                //==================== 單屏設置
                mvars.FPGAsel = 2;
                //====================
                Form1.pvindex = 67;     //Y
                lstget1.Items.Insert(1, "  Y，R" + Form1.pvindex + "，0 ▪▪▪");
                if (mvars.demoMode)
                {
                    mvars.lCount++;
                    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                    mp.doDelayms(1000);
                    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
                }
                else
                {
                    mp.mhFPGASPIWRITE(mvars.FPGAsel, 0);
                }
                lstget1.Items.RemoveAt(1);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1) { }
                else { mvars.errCode = "-6.8." + (i + 1).ToString(); goto eX; }
                #endregion Y 座標 單屏設置


                Form1.lstmcuW60000.Items.Clear();
                BinArr = new byte[mvars.GAMMA_SIZE];

            //0x60000 Write(直接清空 0x60000)(Disabled) (查閱請參考 1219)

            passR60K:

                if (mvars.demoMode == false)
                {
                    mvars.lblCmd = "PRIID_OFF";
                    mp.mpIDONOFF(0);
                }
                lstget1.Items.RemoveAt(0);
            singleErr:
                mp.doDelayms(500);
            }
            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Single Screen Set ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "回復單屏設定 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "回復單屏設定 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "單屏設定戻す ▪▪▪ 完成"); }
            #endregion 6. 回讀0x66000 FPGA設回單屏座標(0,0)，FPGA單屏delay歸零，寫入單屏EDID，SacleOFF (重點 由最後屏到第一屏)

            #region 7. ID Boardcast Restore
            lstget1.Items.Insert(0, "");
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " ID Reset ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " ID 回復 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " ID 回复 ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " ID 戻る ▪▪▪"); }

            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";

            mvars.lblCmd = "PRIID_RESTORE";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else mp.mWRDEVID(1);
            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            if (mvars.lGet[mvars.lCount - 1].IndexOf("DONE", 0) != -1)
            {
                if (mvars.demoMode == false)
                {
                    mvars.lblCmd = "PRIID_SHOW";
                    mp.mpIDONOFF(1);
                    mp.doDelayms(500);
                }

                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "ID Reset ▪▪▪ Done"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "ID 回復 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "ID 回复 ▪▪▪ 完成"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "ID 戻る ▪▪▪ 終了"); }
                lstget1.Items.RemoveAt(1);
            }
            else
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "ID Reset ▪▪▪ Error"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "ID 回復 ▪▪▪ 異常"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "ID 回复 ▪▪▪ 異常"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "ID 戻る ▪▪▪ 未完"); }
                mvars.errCode = "-7";
                goto eX;
            }
            #endregion 7. ID Boardcast Restore

            #region 9. Flash(0x60000/0x62000/0x64000) Boardcast Empty  還原亮度預設值 240灰，清除0x66000(v0037 Add but disable)
            lstget1.Items.Insert(0, "");
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            mvars.FPGAsel = 2;
            #region 清除 0x60000
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R60K to Clear ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  清除 R60K ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  清除 R60K ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R60K 消す▪▪▪"); }
            BinArr = new byte[mvars.FPGA_MTP_SIZE];
            mvars.lblCmd = "MCU_FLASH_W60000";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mp.mhMCUFLASHWRITE("60000", ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9.6"; goto eX; }
            }
            lstget1.Items.RemoveAt(1);
            #endregion 0x60000

            #region 清除 0x62000
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R62K to Clear ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  清除 R62K ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  清除 R62K ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R62K 消す▪▪▪"); }
            BinArr = new byte[mvars.GAMMA_MTP_SIZE];
            mvars.lblCmd = "MCU_FLASH_W62000";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mp.mhMCUFLASHWRITE("62000", ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9.62"; goto eX; }
            }
            lstget1.Items.RemoveAt(1);
            #endregion 0x62000

            #region 清除 0x64000
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R64K to Clear ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  清除 R64K ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  清除 R64K ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R64K 消す▪▪▪"); }
            BinArr = new byte[mvars.GAMMA_MTP_SIZE];
            mvars.lblCmd = "MCU_FLASH_W64000";
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mp.mhMCUFLASHWRITE("64000", ref BinArr);
                if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9.64"; goto eX; }
            }
            lstget1.Items.RemoveAt(1);
            #endregion 0x64000

            #region 清除 0x66000 (v0037 Add but disable)
            //if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  R66K to Clear ▪▪▪"); }
            //else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  清除 R66K ▪▪▪"); }
            //else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  清除 R66K ▪▪▪"); }
            //else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  R66K 消す▪▪▪"); }
            //BinArr = new byte[mvars.GAMMA_MTP_SIZE];
            //mvars.lblCmd = "MCU_FLASH_W66000";
            //if (mvars.demoMode)
            //{
            //    mvars.lCount++;
            //    mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
            //    mp.doDelayms(1000);
            //    mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            //}
            //else
            //{
            //    mp.mhMCUFLASHWRITE("66000", ref BinArr);
            //    if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-9.66"; goto eX; }
            //}
            //lstget1.Items.RemoveAt(1);
            #endregion 0x66000

            #region 燈板Gamma可調(工程模式) (on : 1 / off : 0) Drop 240 gray (借用 Form1.lstmcuW60000 Form1.lstmcuR60000 實體)

            mvars.dropvalue = "240";  //v0037

            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Restore drop " + mvars.dropvalue + " ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  還原 drop 預設值 " + mvars.dropvalue + " ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  还原 drop 预设值 " + mvars.dropvalue + " ▪▪▪"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  Restore drop " + mvars.dropvalue + " ▪▪▪"); }



            ///借用 Form1.lstmcuW60000 實體
            mvars.lstmcuW62000 = Form1.lstmcuW60000;
            mvars.lstmcuR62000 = Form1.lstmcuR60000;
            ///借用 Form1.lstmcuW60000



            mvars.lstmcuR62000.Items.Clear();
            mvars.lstmcuW62000.Items.Clear();
            mvars.lstmcuW62000.Items.AddRange(mvars.uiregadrdefault.Split('~'));

            for (int i = 5; i <= 5; i++)
            {
                string[] svs = mvars.lstmcuW62000.Items[i].ToString().Split(',');
                mvars.lstmcuW62000.Items.RemoveAt(i);
                string svss = svs[0] + ",1";
                mvars.lstmcuW62000.Items.Insert(i, svss);
            }
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
            string[] sRegDec = new string[svme];   //addr
            string[] sDataDec = new string[svme];  //data
            //lbl_GMAdrop.Text = svme.ToString();
            for (int i = svms; i < svme; i++)
            {
                string[] svs = mvars.lstmcuW62000.Items[i].ToString().Split(',');
                mvars.lstmcuW62000.Items.RemoveAt(i);
                string svss = svs[0] + "," + (Convert.ToInt16(svs[1]) * Convert.ToInt16(mvars.dropvalue) / 255).ToString();
                mvars.lstmcuW62000.Items.Insert(i, svss);
            }

            for (int i = 0; i < svme; i++)
            {
                sRegDec[i] = mvars.lstmcuW62000.Items[i].ToString().Split(',')[0];
                sDataDec[i] = mvars.lstmcuW62000.Items[i].ToString().Split(',')[1];
            }

            mvars.lblCmd = "MCUFLASH_W62000";
            mp.doDelayms(10);
            mp.cUIREGADRwENG("62000", sRegDec, sDataDec);

            mvars.lstmcuW62000.Items.Clear();
            mvars.lstmcuR62000.Items.Clear();
            lstget1.Items.RemoveAt(1);
            #endregion

            lstget1.Items.RemoveAt(0);
            #endregion 9. Flash(0x60000/0x62000/0x64000) Boardcast Empty

            #region 8. Boardcast RESET
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "EDID Reset ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "EDID リセット ▪▪▪ "); }
            lstget1.Items.Insert(1, "");
            mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
            mvars.FPGAsel = 2;
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "LT8668_Reset_L";
                mp.LT8668_Reset_L();
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-8.2"; goto eX; }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 6;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  (1/2) Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  (1/2) 請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  (1/2) 请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  (1/2) 待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "LT8668_Reset_H";
                mp.LT8668_Reset_H();
            }
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) > -1) { mvars.errCode = "-7.2"; goto eX; }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 6;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  (2/2) Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  (2/2) 請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  (2/2) 请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  (2/2) 待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "EDID Reset ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "EDID 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "EDID リセット ▪▪▪ 終了"); }

            lstget1.Items.Insert(0, "");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Screen Reset ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ "); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "全て リセット ▪▪▪ "); }
            if (mvars.demoMode)
            {
                mvars.lCount++;
                mvars.lCmd[mvars.lCount] = mvars.lblCmd + ",";
                //mp.doDelayms(1000);
                mvars.lGet[mvars.lCount - 1] = mvars.lblCmd + ",DONE,1";
            }
            else
            {
                mvars.lblCmd = "MCU_SW_Rst";
                mp.McuSW_Reset();
            }

            #region wait
            lstget1.Items.Insert(0, "▪▪▪▪➧");
            svWaitSec = 12;
            if (mvars.demoMode) svWaitSec = 1;
            do
            {
                if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, "  Please wait " + svWaitSec + "sec"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, "  請稍後 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, "  请稍后 " + svWaitSec + "秒"); }
                else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, "  待って " + svWaitSec + "秒"); }
                Application.DoEvents();
                mp.doDelayms(1000);
                lstget1.Items.RemoveAt(1);
                svWaitSec--;
            } while (svWaitSec > 0);
            lstget1.Items.RemoveAt(0);
            lstget1.Items.RemoveAt(0);
            #endregion wait

            lstget1.Items.RemoveAt(0);
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Screen Reset ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "大屏 重置 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "全て リセット ▪▪▪ 終了"); }
            #endregion 8. Boardcast RESET

            lstget1.Items.Insert(0, "");
            if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(0, "Restore Single Screen ▪▪▪ Done"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(0, "復原單屏 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(0, "復原單屏 ▪▪▪ 完成"); }
            else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(0, "單一画面設定 ▪▪▪ 終了"); }

            if (mvars.demoMode == false)
            {
                mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                mvars.FPGAsel = 2;
                mvars.lblCmd = "PRIID_SHOW";
                mp.mpIDONOFF(1);
            }

            //if (mvars.demoMode == false) { mp.CommClose(); }
            pnl_busy.SendToBack();

        eX:
            //設定本視窗獲得焦點
            if (Form1.frm1.Handle != Form1.GetF()) Form1.SetF(Form1.frm1.Handle);
            //設定本視窗獲得焦點
            if (svd != DialogResult.Cancel)
            {
                if (mvars.errCode != "0")
                {
                    //lstget1.Items.Insert(0, "▪▪▪▪➧ " + mvars.errCode);
                    //if (mvars.errCode == "-1")
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " H.W. Communication error"); }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " 硬體通訊錯誤"); }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " 硬体通讯错误"); }
                    //    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " ハードウェア 間違い"); }
                    //}
                    //if (mvars.errCode == "-2")
                    //{
                    //    if (MultiLanguage.DefaultLanguage == "en-US") { lstget1.Items.Insert(1, " H.W. Communication error"); }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CHT") { lstget1.Items.Insert(1, " 硬體通訊錯誤"); }
                    //    else if (MultiLanguage.DefaultLanguage == "zh-CN") { lstget1.Items.Insert(1, " 硬体通讯错误"); }
                    //    else if (MultiLanguage.DefaultLanguage == "ja-JP") { lstget1.Items.Insert(1, " ハードウェア 間違い"); }
                    //}
                    //lstget1.Items.Insert(2, "");

                    string svs = " H.W. Communication error";
                    if (mvars.errCode == "-1" || mvars.errCode == "-2")
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { svs = " H.W. Communication error"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = " 硬體通訊錯誤"; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = " 硬体通讯错误"; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svs = " ハードウェア 間違い"; }
                    }
                    else
                    {
                        if (MultiLanguage.DefaultLanguage == "en-US") { svs = " Error Code : "; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CHT") { svs = " 錯誤碼 : "; }
                        else if (MultiLanguage.DefaultLanguage == "zh-CN") { svs = " 错误码 : "; }
                        else if (MultiLanguage.DefaultLanguage == "ja-JP") { svs = " 异常码 : "; }
                        svs += mvars.errCode;
                    }
                    MessageBox.Show(svs);
                }
                else
                {
                    //設定本視窗獲得焦點
                    //if (Form1.frm1.Handle != Form1.GetF()) Form1.SetF(Form1.frm1.Handle);
                    //設定本視窗獲得焦點

                    if (MultiLanguage.DefaultLanguage == "en-US")
                    {
                        mp.msgBox("Restore Atand Alone", "Are you satisfied with the setting ? " + "\r\n" + "\r\n" + "   No，re-do『Restore Stand Alone』", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 3);
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                    {
                        mp.msgBox("復原單屏", "是否符合預期結果 ? " + "\r\n" + "\r\n" + "   否，請重新執行『復原單屏』", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 3);
                    }
                    else if (MultiLanguage.DefaultLanguage == "zh-CN")
                    {
                        mp.msgBox("复原单屏", "是否符合预期结果 ? " + "\r\n" + "\r\n" + "   否，请重新执行『复原单屏』", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 3);
                    }
                    else if (MultiLanguage.DefaultLanguage == "ja-JP")
                    {
                        mp.msgBox("単一画面に戻す", "是否符合預期結果 ? " + "\r\n" + "\r\n" + "   否，請重新執行『単一画面に戻す』", Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 300, 3);
                    }

                }
                if (mvars.demoMode == false)
                {
                    mvars.deviceID = mvars.deviceID.Substring(0, 2) + "A0";
                    mvars.FPGAsel = 2;
                    mvars.lblCmd = "PRIID_OFF";
                    mp.mpIDONOFF(0);
                }
                mvars.deviceID = svdeviceID;
                mvars.FPGAsel = svFPGAsel;
            }
            if (mvars.demoMode == false) { mp.CommClose(); }
            Form1.ActiveForm.Enabled = true;
        }

        private void btn_send_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string value = "";
                string svs = "『Read Screen Settings 』";
                string svtitle = "Screen Config";
                if (MultiLanguage.DefaultLanguage == "en-US")
                {
                    svs = "『 Read Screen Settings 』";
                    svtitle = "Screen Config";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CHT")
                {
                    svs = "『 回讀大屏設置 』";
                    svtitle = "大屏拼接";
                }
                else if (MultiLanguage.DefaultLanguage == "zh-CN")
                {
                    svs = "『 回读大屏设置 』";
                    svtitle = "大屏拼接";
                }
                else if (MultiLanguage.DefaultLanguage == "ja-JP")
                {
                    svs = "『 全体的な設定を読み返す 』";
                    svtitle = "大画面設定";
                }

                // if (mp.InputBox("大画面設定", "是否執行『全体的な設定を読み返す』?", ref v, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 1, 1, 3, "") == DialogResult.Cancel) return;


                if (mp.InputBox(svtitle, svs, ref value, Form1.ActiveForm.Left + 300, Form1.ActiveForm.Top + 100, 200, 150, 1, "") == DialogResult.OK)
                    btn_send.Tag = "Read";
                else
                    btn_send.Tag = "";
            }

        }



        private void h_dgvhscroll_ValueChanged(object sender, EventArgs e)
        {
            int col = h_dgvhscroll.Value / mvars.LBw - 1 + svCurrentLastCol;
            lbl_colfrozen.Text = "hscrValue " + col.ToString();
            byte svrow = 0;
            if (dgv_box.Width > 878) svrow = (byte)(dgv_box.FirstDisplayedCell.RowIndex + 4);

            if (flghscPlus == false)
            {
                col -= svCurrentLastCol;
                if (dgv_box.CurrentCell.ColumnIndex == mvars.boxCols - 1) col += 1; else col += 1;
                dgv_box.CurrentCell = dgv_box[col, svrow];
                byte[] tmp = new byte[] { (byte)(65 + col) };
                string svsort = Encoding.ASCII.GetString(tmp);
                lbl_colfrozen.Text += ",Curr[" + svsort + ", " + svrow.ToString() + "]";
            }
            else
            {
                dgv_box.CurrentCell = dgv_box[col + 1, svrow];
                byte[] tmp = new byte[] { (byte)(66 + col) };
                string svsort = Encoding.ASCII.GetString(tmp);
                lbl_colfrozen.Text += ",Curr[" + svsort + ", " + svrow.ToString() + "]";
            }
            dgv_box.ClearSelection();
            //lblnewCv.Text = h_dgvhscroll.Value.ToString();    //檢測用
            //label1.Text = col.ToString();                     //檢測用
        }

        private void h_dgvhscroll_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue) flghscPlus = true;
            else if (e.NewValue < e.OldValue) flghscPlus = false;

            pnl_dgv.Invalidate();
            #region paint事件 單次觸發 (Disabled)
            //dgv_box.Paint += new PaintEventHandler(dgv_box_CellPaint);

            //Graphics l_gr_work = dgv_box.CreateGraphics();
            //Rectangle l_rt_work = new Rectangle(0, 0, dgv_box.Width, dgv_box.Height);
            //PaintEventArgs l_pe_work = new PaintEventArgs(l_gr_work, l_rt_work);
            //dgv_box_CellPaint(dgv_box, l_pe_work);
            //l_pe_work.Dispose();
            #endregion paint事件 單次觸發 (Disabled)      
        }

        #region 忙碌中
        Point[] ps;
        private void tme_busy_Tick(object sender, EventArgs e)
        {
            pnl_busy.Invalidate();
        }

        private void pnl_busy_Paint(object sender, PaintEventArgs e)
        {
            busy(pnl_busy, e.Graphics);
        }

        void busy(Panel panel, Graphics g)
        {
            int r1 = pnl_busy.Width / 6;
            int r2 = r1 * 2;
            int svCx = pnl_busy.Width / 2;
            int svCy = pnl_busy.Height / 2;

            int px;
            int py;
            int i;
            int j;

            for (i = 0; i < 360; i += 30)
            {
                List<Point> lst = new List<Point>();
                j = i - 20;
                for (int n = j; n <= i; n += 5)
                {
                    px = svCx + (int)(r2 * Math.Round(Math.Cos(Math.PI / 180 * n), 6));
                    py = svCy - (int)(r2 * Math.Round(Math.Sin(Math.PI / 180 * n), 6));
                    lst.Add(new Point(px, py));
                    ps = lst.ToArray();
                }
                j = i;
                for (int n = j; n >= i - 20; n -= 5)
                {
                    px = svCx + (int)(r1 * Math.Round(Math.Cos(Math.PI / 180 * n), 6));
                    py = svCy - (int)(r1 * Math.Round(Math.Sin(Math.PI / 180 * n), 6));
                    lst.Add(new Point(px, py));
                    ps = lst.ToArray();
                }
                g.FillPolygon(new SolidBrush(Color.FromArgb(255, 240, 240, 240)), ps);
            }

            svAng %= 360;
            i = svAng;

            for (int m = 0; m < 3; m++)
            {
                i -= 30;
                List<Point> lst = new List<Point>();
                j = i - 20;
                for (int n = j; n <= i; n += 5)
                {
                    px = svCx + (int)(r2 * Math.Round(Math.Cos(Math.PI / 180 * n), 6));
                    py = svCy - (int)(r2 * Math.Round(Math.Sin(Math.PI / 180 * n), 6));
                    lst.Add(new Point(px, py));
                    ps = lst.ToArray();
                }
                j = i;
                for (int n = j; n >= i - 20; n -= 5)
                {
                    px = svCx + (int)(r1 * Math.Round(Math.Cos(Math.PI / 180 * n), 6));
                    py = svCy - (int)(r1 * Math.Round(Math.Sin(Math.PI / 180 * n), 6));
                    lst.Add(new Point(px, py));
                    ps = lst.ToArray();
                }
                g.FillPolygon(new SolidBrush(Color.FromArgb(128, 120 + m * 20, 120 + m * 20, 120 + m * 20)), ps);
            }
            svAng += 30;
            if (svAng >= 360) svAng = 0;
            g.Dispose();
        }
        #endregion 忙碌中




        #region 建立滑鼠右鍵功能表
        private void dgv_box_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                //#region 建立滑鼠右鍵功能表
                //ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                //this.ContextMenuStrip= contextMenuStrip;
                //ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
                //toolStripMenuItem.Text = "按鈕1";
                //toolStripMenuItem.Click += new EventHandler(toolStripMenuItem1_Click);
                //contextMenuStrip.Items.Add(toolStripMenuItem);
                //#endregion 建立滑鼠右鍵功能表
            }
            //dgv_box.ClearSelection();
            //dgv_box.Paint += new PaintEventHandler(dgv_box_CellPaint);
        }


        private void dgv_box_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = System.Windows.Forms.Cursor.Position;
                Color color = GetPixelFromScreen(p.X, p.Y);
                label1.Text = color.R + "," + color.G + "," + color.B;
            }
            //dgv_box.Paint += new PaintEventHandler(dgv_box_CellPaint);
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("OK");
        }
        #endregion 建立滑鼠右鍵功能表

        private void timer1_Tick(object sender, EventArgs e)
        {
            svCnt++;
            lbl_cnt.Text = svCnt.ToString();
        }

        private void dgv_box_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                pnl_dgv.Invalidate();
                //pnl_dgv.Paint += new PaintEventHandler(pnl_dgv_Paint);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte svfirstR = (byte)dgv_box.FirstDisplayedCell.RowIndex;
            byte svfirstC = (byte)dgv_box.FirstDisplayedCell.ColumnIndex;

            dgv_box.Rows[svfirstR].Cells[svfirstC].Value = Image.FromFile(mvars.strStartUpPath + @"\Parameter\tmp1.bmp");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "顯示正面圖")
            {
                //button2.BackColor = Color.Yellow; //symbolizes light turned on

                if (MultiLanguage.DefaultLanguage == "en-US") label4.Text = "Frontal view";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") label4.Text = "正面示意";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") label4.Text = "正面示意";
                else if (MultiLanguage.DefaultLanguage == "ja-JP") label4.Text = "前面図";

                button2.Text = "顯示背面圖";
                isback = false;
                dgv_box_draw(pnl_dgv, pnl_dgv.CreateGraphics());
            }
            else if (button2.Text == "顯示背面圖")
            {
                //button2.BackColor = Color.Green; //symbolizes light turned off


                if (MultiLanguage.DefaultLanguage == "en-US") label4.Text = "Rear view";
                else if (MultiLanguage.DefaultLanguage == "zh-CHT") label4.Text = "背面示意";
                else if (MultiLanguage.DefaultLanguage == "zh-CN") label4.Text = "背面示意";
                else if (MultiLanguage.DefaultLanguage == "ja-JP") label4.Text = "後ろ表示";
             

                isback = true;
                button2.Text = "顯示正面圖";
                dgv_box_draw(pnl_dgv, pnl_dgv.CreateGraphics());
            }
        }
    }

    #region 雙緩衝機制
    public class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |  //不擦除背景,減少閃爍
                          ControlStyles.OptimizedDoubleBuffer | //雙緩衝
                          ControlStyles.UserPaint,              //使用自定義的重繪事件,減少閃爍
                          true);
        }
        //找到panel控件定義與初始化的地方
        //this.panel1 = new System.Windows.Forms.Panel();
        //改成 this.panel1 = new DoubleBufferPanel();
    }
    #endregion 雙緩衝機制
}
