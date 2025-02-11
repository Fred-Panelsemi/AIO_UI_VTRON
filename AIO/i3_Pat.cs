using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AIO
{
    public partial class i3_Pat : Form
    {
        public static PictureBox[,] pb;

        public i3_Pat()
        {
            InitializeComponent();
        }

        private void i3_Pat_FormClosed(object sender, FormClosedEventArgs e)
        {
            lbl_Mark.BackColor =  Color.FromArgb (0, 192, 0);
            this.BackgroundImage = null;
            Form2.i3pat.Dispose();
            mvars.FormShow[5] = false;
        }

        private void i3_Pat_Load(object sender, EventArgs e)
        {
            mvars.FormShow[5] = true;
        }

        public static void addcontrol(int svhs, int svws, int bxw, int bxh) 
        {
            pb = new PictureBox[svhs, svws];
            for (int svh = 0; svh < svhs; svh++)
            {
                for (int svw = 0; svw < svws; svw++)
                {
                    pb[svh, svw] = new System.Windows.Forms.PictureBox();
                    pb[svh, svw].SetBounds(svw * bxw, svh * bxh, bxw, bxh);
                    Form2.i3pat.Controls.AddRange(new Control[] { pb[svh, svw] });
                    pb[svh, svw].Tag = "add";
                }
            }
        }

        public static void erasecontrol()
        {
            if (pb != null)
            {
                for (int outer = pb.GetLowerBound(0); outer <= pb.GetUpperBound(0); outer++)
                    for (int inner = pb.GetLowerBound(1); inner <= pb.GetUpperBound(1); inner++)
                        if (Form2.i3pat.Controls.Contains(pb[outer, inner])) { pb[outer, inner].Image = null; pb[outer, inner].Refresh(); Form2.i3pat.Controls.Remove(pb[outer, inner]); }
                        //pb[outer, inner].Dispose();
            }
        }

        private void i3_Pat_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.SendToBack();
                if (mvars.FormShow[2])
                {
                    string[] svs = Form2.lblexcorner.Text.Split(',');
                    if (this.Location.X != Convert.ToInt16(svs[0]))
                    {
                        Form2.chkshowextend.Checked = false;
                        Form2.lblexcorner.Text = "";
                        this.Close();
                    }
                }
                if (mvars.FormShow[5])
                {
                    Screen[] screens = Screen.AllScreens;
                    if (screens.GetUpperBound(0) == 0) this.Close(); Form2.chkshowextend.Checked = false;
                }
                this.BringToFront();
            }
            else if (e.KeyCode == Keys.Tab)
            {
                this.SendToBack();
                if (mvars.FormShow[2] == false) this.Close();
                else
                {
                    if (this.Text != "")
                    {
                        string[] svs = this.Text.Split(',');
                        if (this.Location.X != Convert.ToInt16(svs[0]))
                        {
                            this.Location = new Point(Convert.ToInt16(svs[0]), Convert.ToInt16(svs[1]));
                        }
                        this.BringToFront();
                    }
                }
            }
        }
    }
}
