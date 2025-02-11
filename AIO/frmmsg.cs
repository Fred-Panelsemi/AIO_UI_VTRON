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
    public partial class frmmsg : Form
    {
        

        public frmmsg()
        {
            InitializeComponent();
        }

        private void frmmsg_Load(object sender, EventArgs e)
        {
            mvars.lstget.SetBounds(12, 26, 494, 184);
            mvars.lstMsgIn.SetBounds(12, 245, 494, 79);
            Font fnt = new Font("Arial", 9, FontStyle.Bold);
            mvars.lstget.Font = fnt;
            this.Controls.AddRange(new Control[] { mvars.lstget, mvars.lstMsgIn });
            mvars.FormShow[1] = true;
        }

        private void frmmsg_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Controls.Remove(mvars.lstget);
            this.Controls.Remove(mvars.lstMsgIn);
            mvars.FormShow[1] = false;
        }
    }
}
