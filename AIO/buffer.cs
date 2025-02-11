using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIO
{
    class buffer
    {
        private static void DoubleBufferDataGridView( Form1.uc DataGridView dgv, bool flag)
        {
            Type dgvtype = dgv.GetType();
            PropertyInfo pi = dgvtype.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }
    }
}
