namespace Cowain_Machine.Flow
{
    partial class frm_DownTime
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_showMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bt_back = new System.Windows.Forms.Button();
            this.bt_ok = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button_BackGantry = new System.Windows.Forms.Button();
            this.button_FrontGantry = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bt_13 = new System.Windows.Forms.Button();
            this.bt_12 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bt_27 = new System.Windows.Forms.Button();
            this.bt_26 = new System.Windows.Forms.Button();
            this.bt_25 = new System.Windows.Forms.Button();
            this.bt_24 = new System.Windows.Forms.Button();
            this.bt_23 = new System.Windows.Forms.Button();
            this.bt_22 = new System.Windows.Forms.Button();
            this.bt_21 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_showMessage
            // 
            this.lb_showMessage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_showMessage.AutoSize = true;
            this.lb_showMessage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lb_showMessage.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_showMessage.Location = new System.Drawing.Point(2, 92);
            this.lb_showMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_showMessage.Name = "lb_showMessage";
            this.lb_showMessage.Size = new System.Drawing.Size(169, 20);
            this.lb_showMessage.TabIndex = 6;
            this.lb_showMessage.Text = "请选择停机原因：";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(142, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "HIVE记录界面";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.bt_back);
            this.panel1.Controls.Add(this.bt_ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 373);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(518, 107);
            this.panel1.TabIndex = 1;
            // 
            // bt_back
            // 
            this.bt_back.BackColor = System.Drawing.SystemColors.ControlLight;
            this.bt_back.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_back.Location = new System.Drawing.Point(43, 23);
            this.bt_back.Margin = new System.Windows.Forms.Padding(2);
            this.bt_back.Name = "bt_back";
            this.bt_back.Size = new System.Drawing.Size(125, 60);
            this.bt_back.TabIndex = 1;
            this.bt_back.Text = "返回";
            this.bt_back.UseVisualStyleBackColor = false;
            this.bt_back.Click += new System.EventHandler(this.bt_back_Click);
            // 
            // bt_ok
            // 
            this.bt_ok.BackColor = System.Drawing.SystemColors.ControlLight;
            this.bt_ok.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_ok.Location = new System.Drawing.Point(192, 23);
            this.bt_ok.Margin = new System.Windows.Forms.Padding(2);
            this.bt_ok.Name = "bt_ok";
            this.bt_ok.Size = new System.Drawing.Size(125, 60);
            this.bt_ok.TabIndex = 0;
            this.bt_ok.Text = "确定";
            this.bt_ok.UseVisualStyleBackColor = false;
            this.bt_ok.Click += new System.EventHandler(this.bt_ok_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lb_showMessage, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.27168F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.72832F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 244F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(522, 482);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl1.Location = new System.Drawing.Point(2, 129);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(518, 240);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button_BackGantry);
            this.tabPage3.Controls.Add(this.button_FrontGantry);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(510, 214);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "龙门选择";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button_BackGantry
            // 
            this.button_BackGantry.BackColor = System.Drawing.Color.Gainsboro;
            this.button_BackGantry.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_BackGantry.Location = new System.Drawing.Point(118, 128);
            this.button_BackGantry.Margin = new System.Windows.Forms.Padding(2);
            this.button_BackGantry.Name = "button_BackGantry";
            this.button_BackGantry.Size = new System.Drawing.Size(275, 42);
            this.button_BackGantry.TabIndex = 2;
            this.button_BackGantry.Text = "后龙门";
            this.button_BackGantry.UseVisualStyleBackColor = false;
            this.button_BackGantry.Click += new System.EventHandler(this.button_BackGantry_Click);
            // 
            // button_FrontGantry
            // 
            this.button_FrontGantry.BackColor = System.Drawing.Color.Gainsboro;
            this.button_FrontGantry.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_FrontGantry.Location = new System.Drawing.Point(117, 45);
            this.button_FrontGantry.Margin = new System.Windows.Forms.Padding(2);
            this.button_FrontGantry.Name = "button_FrontGantry";
            this.button_FrontGantry.Size = new System.Drawing.Size(276, 42);
            this.button_FrontGantry.TabIndex = 3;
            this.button_FrontGantry.Text = "前龙门";
            this.button_FrontGantry.UseVisualStyleBackColor = false;
            this.button_FrontGantry.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.tabPage1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(510, 214);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "停机种类";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel3.Controls.Add(this.bt_13);
            this.panel3.Controls.Add(this.bt_12);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(2, 2);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(506, 210);
            this.panel3.TabIndex = 2;
            // 
            // bt_13
            // 
            this.bt_13.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_13.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_13.Location = new System.Drawing.Point(38, 131);
            this.bt_13.Margin = new System.Windows.Forms.Padding(2);
            this.bt_13.Name = "bt_13";
            this.bt_13.Size = new System.Drawing.Size(275, 42);
            this.bt_13.TabIndex = 1;
            this.bt_13.Text = "计划停机";
            this.bt_13.UseVisualStyleBackColor = false;
            this.bt_13.Click += new System.EventHandler(this.bt_Click);
            // 
            // bt_12
            // 
            this.bt_12.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_12.Enabled = false;
            this.bt_12.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_12.Location = new System.Drawing.Point(37, 48);
            this.bt_12.Margin = new System.Windows.Forms.Padding(2);
            this.bt_12.Name = "bt_12";
            this.bt_12.Size = new System.Drawing.Size(276, 42);
            this.bt_12.TabIndex = 1;
            this.bt_12.Text = "工程停机";
            this.bt_12.UseVisualStyleBackColor = false;
            this.bt_12.Click += new System.EventHandler(this.bt_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bt_27);
            this.tabPage2.Controls.Add(this.bt_26);
            this.tabPage2.Controls.Add(this.bt_25);
            this.tabPage2.Controls.Add(this.bt_24);
            this.tabPage2.Controls.Add(this.bt_23);
            this.tabPage2.Controls.Add(this.bt_22);
            this.tabPage2.Controls.Add(this.bt_21);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(510, 214);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "计划停机";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bt_27
            // 
            this.bt_27.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_27.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_27.Location = new System.Drawing.Point(358, 16);
            this.bt_27.Margin = new System.Windows.Forms.Padding(2);
            this.bt_27.Name = "bt_27";
            this.bt_27.Size = new System.Drawing.Size(137, 42);
            this.bt_27.TabIndex = 9;
            this.bt_27.Text = "其它";
            this.bt_27.UseVisualStyleBackColor = false;
            this.bt_27.Click += new System.EventHandler(this.bt_Click);
            // 
            // bt_26
            // 
            this.bt_26.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_26.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_26.Location = new System.Drawing.Point(186, 152);
            this.bt_26.Margin = new System.Windows.Forms.Padding(2);
            this.bt_26.Name = "bt_26";
            this.bt_26.Size = new System.Drawing.Size(137, 42);
            this.bt_26.TabIndex = 8;
            this.bt_26.Text = "设备耗材更换";
            this.bt_26.UseVisualStyleBackColor = false;
            this.bt_26.Click += new System.EventHandler(this.bt_Click);
            // 
            // bt_25
            // 
            this.bt_25.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_25.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_25.Location = new System.Drawing.Point(186, 80);
            this.bt_25.Margin = new System.Windows.Forms.Padding(2);
            this.bt_25.Name = "bt_25";
            this.bt_25.Size = new System.Drawing.Size(137, 42);
            this.bt_25.TabIndex = 7;
            this.bt_25.Text = "更换胶阀";
            this.bt_25.UseVisualStyleBackColor = false;
            this.bt_25.Click += new System.EventHandler(this.bt_Click);
            // 
            // bt_24
            // 
            this.bt_24.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_24.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_24.Location = new System.Drawing.Point(186, 16);
            this.bt_24.Margin = new System.Windows.Forms.Padding(2);
            this.bt_24.Name = "bt_24";
            this.bt_24.Size = new System.Drawing.Size(137, 42);
            this.bt_24.TabIndex = 6;
            this.bt_24.Text = "更换针头";
            this.bt_24.UseVisualStyleBackColor = false;
            this.bt_24.Click += new System.EventHandler(this.bt_Click);
            // 
            // bt_23
            // 
            this.bt_23.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_23.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_23.Location = new System.Drawing.Point(20, 152);
            this.bt_23.Margin = new System.Windows.Forms.Padding(2);
            this.bt_23.Name = "bt_23";
            this.bt_23.Size = new System.Drawing.Size(137, 42);
            this.bt_23.TabIndex = 5;
            this.bt_23.Text = "更换HM胶水";
            this.bt_23.UseVisualStyleBackColor = false;
            this.bt_23.Click += new System.EventHandler(this.bt_Click);
            // 
            // bt_22
            // 
            this.bt_22.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_22.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_22.Location = new System.Drawing.Point(20, 80);
            this.bt_22.Margin = new System.Windows.Forms.Padding(2);
            this.bt_22.Name = "bt_22";
            this.bt_22.Size = new System.Drawing.Size(137, 42);
            this.bt_22.TabIndex = 4;
            this.bt_22.Text = "更换AB胶水";
            this.bt_22.UseVisualStyleBackColor = false;
            this.bt_22.Click += new System.EventHandler(this.bt_Click);
            // 
            // bt_21
            // 
            this.bt_21.BackColor = System.Drawing.Color.Gainsboro;
            this.bt_21.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_21.Location = new System.Drawing.Point(20, 16);
            this.bt_21.Margin = new System.Windows.Forms.Padding(2);
            this.bt_21.Name = "bt_21";
            this.bt_21.Size = new System.Drawing.Size(137, 42);
            this.bt_21.TabIndex = 3;
            this.bt_21.Text = "日常点检";
            this.bt_21.UseVisualStyleBackColor = false;
            this.bt_21.Click += new System.EventHandler(this.bt_Click);
            // 
            // frm_DownTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(522, 482);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(538, 521);
            this.MinimumSize = new System.Drawing.Size(538, 521);
            this.Name = "frm_DownTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DownTime";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frm_DownTime_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lb_showMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bt_ok;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button bt_13;
        private System.Windows.Forms.Button bt_12;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button bt_22;
        private System.Windows.Forms.Button bt_21;
        private System.Windows.Forms.Button bt_back;
        private System.Windows.Forms.Button bt_23;
        private System.Windows.Forms.Button bt_24;
        private System.Windows.Forms.Button bt_25;
        private System.Windows.Forms.Button bt_26;
        private System.Windows.Forms.Button bt_27;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button_BackGantry;
        private System.Windows.Forms.Button button_FrontGantry;
    }
}

