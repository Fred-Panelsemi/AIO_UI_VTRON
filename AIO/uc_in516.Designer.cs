
namespace AIO
{
    partial class uc_in516
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox183 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_pwicinfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox183
            // 
            this.textBox183.Font = new System.Drawing.Font("Arial", 9F);
            this.textBox183.Location = new System.Drawing.Point(619, 37);
            this.textBox183.Name = "textBox183";
            this.textBox183.Size = new System.Drawing.Size(24, 21);
            this.textBox183.TabIndex = 883;
            this.textBox183.Text = "20";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(521, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 884;
            this.label2.Text = "Device Address：";
            // 
            // btn_pwicinfo
            // 
            this.btn_pwicinfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_pwicinfo.Font = new System.Drawing.Font("Arial", 9F);
            this.btn_pwicinfo.Location = new System.Drawing.Point(5, 244);
            this.btn_pwicinfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_pwicinfo.Name = "btn_pwicinfo";
            this.btn_pwicinfo.Size = new System.Drawing.Size(76, 23);
            this.btn_pwicinfo.TabIndex = 885;
            this.btn_pwicinfo.Text = "IC Info";
            this.btn_pwicinfo.UseVisualStyleBackColor = false;
            this.btn_pwicinfo.Click += new System.EventHandler(this.btn_pwicinfo_Click);
            // 
            // uc_in516
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_pwicinfo);
            this.Controls.Add(this.textBox183);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "uc_in516";
            this.Size = new System.Drawing.Size(847, 390);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox183;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Button btn_pwicinfo;
    }
}
