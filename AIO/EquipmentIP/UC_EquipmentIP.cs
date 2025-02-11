using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AIO.EquipmentIP
{
    public partial class UC_EquipmentIP : UserControl
    {
        #region 设备IP控件触发事件
        //  点击获取按钮触发事件
        public delegate void UC_EquipmentIPEventHandler(object sender, UC_EquipmentIPEventArgs e);

        public event UC_EquipmentIPEventHandler ClickObtainUC_EquipmentIPEvent;
        public event UC_EquipmentIPEventHandler ClickSetUC_EquipmentIPEvent;
        #endregion

        #region 字段
        private Button _btn_Obtain;     //获取按钮
        private Button _btn_Set;          //设置按钮

        private Label _lb_IPAddr;
        private Label _lb_SubnetMask;
        private Label _lb_DefaultGateway;

        private IpInputBox _box_IPAddr;                 // IP地址设置
        private IpInputBox _box_SubnetMask;         // 子网掩码
        private IpInputBox _box_DefaultGateway;    // 默认网关

        #endregion

        #region 属性
        /// <summary>
        /// 设备索引
        /// </summary>
        public decimal EquipmentIndex
        {
            set { numericUpDown1.Value = value;}
            get { return numericUpDown1.Value; }
        }
        /// <summary>
        /// 设备个数
        /// </summary>
        public decimal EquipmentCount
        {
            set { numericUpDown1.Maximum = value; }
            get { return numericUpDown1.Maximum; }
        }
        /// <summary>
        /// IP地址
        /// </summary>
        private string _iPAddr;
        public string IPAddr 
        {
            set 
            {
                _iPAddr = value;
                _box_IPAddr.IpAddressString = _iPAddr;
            }
            get
            {
                _iPAddr = _box_IPAddr.IpAddressString;
                return _iPAddr;
            }
        }

        /// <summary>
        /// 子网掩码
        /// </summary>
        private string _subnetMask;
        private NumericUpDown numericUpDown1;
        private Label label1;

        public string SubnetMask
        {
            set
            {
                _subnetMask = value;
                _box_SubnetMask.IpAddressString = _subnetMask;
            }
            get
            {
                _subnetMask = _box_SubnetMask.IpAddressString;
                return _subnetMask;
            }
        }

        /// <summary>
        /// 默认网关
        /// </summary>
        private string _defaultGateway;
        public string DefaultGateway
        {
            set
            {
                _defaultGateway = value;
                _box_DefaultGateway.IpAddressString = _defaultGateway;
            }
            get
            {
                _defaultGateway = _box_DefaultGateway.IpAddressString;
                return _defaultGateway;
            }
        }

        #endregion
        public UC_EquipmentIP()
        {
            InitializeComponent();
            InitIPUI();
        }

        #region 私有方法
        private void InitIPUI()
        {
            _btn_Obtain = new Button();
            _btn_Obtain.Location = new System.Drawing.Point(10, 155);
            _btn_Obtain.Name = "btn_Obtain";
            _btn_Obtain.Size = new System.Drawing.Size(75, 23);
            _btn_Obtain.TabIndex = 0;
            _btn_Obtain.Text = "Get";
            _btn_Obtain.UseVisualStyleBackColor = true;
            _btn_Obtain.Click += new System.EventHandler(btn_Obtain_Click);
            this.Controls.Add(_btn_Obtain);

            _btn_Set = new Button();
            _btn_Set.Location = new System.Drawing.Point(95, 155);
            _btn_Set.Name = "btn_Set";
            _btn_Set.Size = new System.Drawing.Size(75, 23);
            _btn_Set.TabIndex = 0;
            _btn_Set.Text = "Set";
            _btn_Set.UseVisualStyleBackColor = true;
            _btn_Set.Click += new System.EventHandler(btn_Set_Click);
            this.Controls.Add(_btn_Set);

            _lb_IPAddr = new Label();
            _lb_IPAddr.Size = new System.Drawing.Size(60, 25);
            _lb_IPAddr.Location = new Point(10, 55);
            _lb_IPAddr.Text = "IP Address:";

            _lb_SubnetMask = new Label();
            _lb_SubnetMask.Size = new System.Drawing.Size(60, 25);
            _lb_SubnetMask.Location = new Point(10, 90);
            _lb_SubnetMask.Text = "Subnet Mask:";

            _lb_DefaultGateway = new Label();
            _lb_DefaultGateway.Size = new System.Drawing.Size(60, 25);
            _lb_DefaultGateway.Location = new Point(10, 125);
            _lb_DefaultGateway.Text = "Default Gateway:";

            this.Controls.Add(_lb_IPAddr);
            this.Controls.Add(_lb_SubnetMask);
            this.Controls.Add(_lb_DefaultGateway);

            _box_IPAddr = new IpInputBox(false);
            _box_IPAddr.Location = new Point(80, 50);

            _box_SubnetMask = new IpInputBox(true);
            _box_SubnetMask.Location = new Point(80, 85);

            _box_DefaultGateway = new IpInputBox(false);
            _box_DefaultGateway.Location = new Point(80, 120);

            this.Controls.Add(_box_IPAddr);
            this.Controls.Add(_box_SubnetMask);
            this.Controls.Add(_box_DefaultGateway);
        }

        //设置
        private void btn_Set_Click(object sender, EventArgs e)
        {
            if (ClickSetUC_EquipmentIPEvent == null)
            {
                return;
            }
            ClickSetUC_EquipmentIPEvent(this, new UC_EquipmentIPEventArgs() 
            {
                IPAddr = _iPAddr,
                SubnetMask = _subnetMask,
                DefaultGateway = _defaultGateway
            });
        }

        //获取
        private void btn_Obtain_Click(object sender, EventArgs e)
        {
            if (ClickObtainUC_EquipmentIPEvent == null)
            {
                return;
            }
            ClickObtainUC_EquipmentIPEvent(this, new UC_EquipmentIPEventArgs() 
            {
                IPAddr = _iPAddr,
                SubnetMask = _subnetMask,
                DefaultGateway = _defaultGateway
            });
        }
        #endregion

        private void InitializeComponent()
        {
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(98, 18);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(76, 22);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Screen Index:";
            // 
            // UC_EquipmentIP
            // 
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Name = "UC_EquipmentIP";
            this.Size = new System.Drawing.Size(458, 253);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
