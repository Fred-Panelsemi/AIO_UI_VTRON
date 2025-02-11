using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nova.Mars.SDK;

namespace PID
{
    public partial class Frm_SBInfo : Form
    {
        private ScanBoardMapRegion _sbMapRegion;
        public ScanBoardMapRegion SBMapRegion
        {
            get 
            {
                _sbMapRegion = new ScanBoardMapRegion();
                _sbMapRegion.SenderIndex = Convert.ToByte(numericUpDown_SenderIndex.Value);
                _sbMapRegion.PortIndex = Convert.ToByte(numericUpDown_PortIndex.Value);
                _sbMapRegion.ConnectIndex = Convert.ToUInt16(numericUpDown_ScanBoardIndex.Value);
                _sbMapRegion.X = Convert.ToUInt16(numericUpDown_X.Value);
                _sbMapRegion.Y = Convert.ToUInt16(numericUpDown_Y.Value);
                _sbMapRegion.Width = Convert.ToUInt16(numericUpDown_Width.Value);
                _sbMapRegion.Height = Convert.ToUInt16(numericUpDown_Height.Value);
                return _sbMapRegion;
            }
            set
            {
                _sbMapRegion = value;
                numericUpDown_SenderIndex.Value = _sbMapRegion.SenderIndex;
                numericUpDown_PortIndex.Value = _sbMapRegion.PortIndex;
                numericUpDown_ScanBoardIndex.Value = _sbMapRegion.ConnectIndex;
                numericUpDown_X.Value = _sbMapRegion.X;
                numericUpDown_Y.Value = _sbMapRegion.Y;
                numericUpDown_Width.Value = _sbMapRegion.Width;
                numericUpDown_Height.Value = _sbMapRegion.Height;
            }
        }


        public Frm_SBInfo()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void numericUpDown_ScanBoardIndex_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}