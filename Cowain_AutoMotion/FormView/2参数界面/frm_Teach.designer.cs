namespace Cowain_Form.FormView
{
    partial class frm_Teach
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage设备重要参数 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage设备重要参数);
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 50);
            this.tabControl1.Location = new System.Drawing.Point(6, 9);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1332, 591);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 18;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage设备重要参数
            // 
            this.tabPage设备重要参数.Location = new System.Drawing.Point(4, 54);
            this.tabPage设备重要参数.Name = "tabPage设备重要参数";
            this.tabPage设备重要参数.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage设备重要参数.Size = new System.Drawing.Size(1324, 533);
            this.tabPage设备重要参数.TabIndex = 6;
            this.tabPage设备重要参数.Text = "设备重要参数";
            this.tabPage设备重要参数.UseVisualStyleBackColor = true;
            // 
            // frm_Teach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 641);
            this.Controls.Add(this.tabControl1);
            this.Name = "frm_Teach";
            this.Text = "frm_Teach";
            this.Load += new System.EventHandler(this.frm_Teach_Load);
            this.Shown += new System.EventHandler(this.frm_Teach_Shown);
            this.VisibleChanged += new System.EventHandler(this.frm_Teach_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage设备重要参数;
    }
}