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
    public partial class uc_in516 : UserControl
    {
        public static TextBox txt183 = null;


        public uc_in516()
        {
            InitializeComponent();

            txt183 = textBox183;
        }

        private void btn_pwicinfo_Click(object sender, EventArgs e)
        {
            mp.markreset(99, false);
            string sverr = "0";
            mvars.flgSelf = true;

            mvars.lblCmd = "PWIC_INFO";
            mp.mhPWICINFO((Convert.ToByte(txt183.Text) * 2).ToString());    //跟525一樣是"40"
            if (mvars.lGet[mvars.lCount - 1].IndexOf("ERROR", 0) != -1) { sverr = "-1"; goto Err; }
            MessageBox.Show(mvars.lGet[mvars.lCount - 1].Split(',')[mvars.lGet[mvars.lCount - 1].Split(',').Length - 1], mvars.strUInameMe + mvars.UImajor);
            return;
        Err:
            MessageBox.Show("ErrCode " + sverr + "," + mvars.lGet[mvars.lCount - 1], mvars.strUInameMe + mvars.UImajor);
        }
    }
}
