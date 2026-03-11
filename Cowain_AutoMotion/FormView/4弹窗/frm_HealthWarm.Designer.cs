namespace Cowain_Form.FormView
{
    partial class frm_HealthWarm
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
            this.labDetail = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labSTime = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView_Sloution = new System.Windows.Forms.ListView();
            this.comboBox_Sloution = new System.Windows.Forms.ComboBox();
            this.btn_静音 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.labDetail.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labDetail
            // 
            this.labDetail.Location = new System.Drawing.Point(141, 75);
            this.labDetail.Name = "labDetail";
            this.labDetail.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.labDetail.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDetail.Properties.Appearance.Options.UseBackColor = true;
            this.labDetail.Properties.Appearance.Options.UseFont = true;
            this.labDetail.Properties.ReadOnly = true;
            this.labDetail.Size = new System.Drawing.Size(295, 96);
            this.labDetail.TabIndex = 231;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(31, 76);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(91, 24);
            this.labelControl5.TabIndex = 228;
            this.labelControl5.Text = "详细信息:";
            // 
            // labSTime
            // 
            this.labSTime.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.labSTime.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSTime.Appearance.Options.UseBackColor = true;
            this.labSTime.Appearance.Options.UseFont = true;
            this.labSTime.Appearance.Options.UseTextOptions = true;
            this.labSTime.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labSTime.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labSTime.Location = new System.Drawing.Point(140, 12);
            this.labSTime.Name = "labSTime";
            this.labSTime.Size = new System.Drawing.Size(295, 37);
            this.labSTime.TabIndex = 229;
            this.labSTime.Text = "2022/10/01 10:00:00";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(31, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(91, 24);
            this.labelControl1.TabIndex = 230;
            this.labelControl1.Text = "开始时间:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_Sloution);
            this.groupBox1.Controls.Add(this.comboBox_Sloution);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(12, 210);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 177);
            this.groupBox1.TabIndex = 232;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项卡";
            // 
            // listView_Sloution
            // 
            this.listView_Sloution.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Sloution.HideSelection = false;
            this.listView_Sloution.Location = new System.Drawing.Point(12, 54);
            this.listView_Sloution.Name = "listView_Sloution";
            this.listView_Sloution.Size = new System.Drawing.Size(405, 119);
            this.listView_Sloution.TabIndex = 201;
            this.listView_Sloution.UseCompatibleStateImageBehavior = false;
            this.listView_Sloution.View = System.Windows.Forms.View.List;
            // 
            // comboBox_Sloution
            // 
            this.comboBox_Sloution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Sloution.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_Sloution.FormattingEnabled = true;
            this.comboBox_Sloution.Location = new System.Drawing.Point(12, 24);
            this.comboBox_Sloution.Name = "comboBox_Sloution";
            this.comboBox_Sloution.Size = new System.Drawing.Size(405, 28);
            this.comboBox_Sloution.TabIndex = 200;
            // 
            // btn_静音
            // 
            this.btn_静音.Location = new System.Drawing.Point(31, 407);
            this.btn_静音.Name = "btn_静音";
            this.btn_静音.Size = new System.Drawing.Size(75, 31);
            this.btn_静音.TabIndex = 233;
            this.btn_静音.Text = "静音";
            this.btn_静音.Click += new System.EventHandler(this.btn_静音_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(327, 407);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 31);
            this.btn_OK.TabIndex = 233;
            this.btn_OK.Text = "确认";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // frm_HealthWarm
            // 
            this.Appearance.BackColor = System.Drawing.Color.Gold;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 450);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_静音);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labDetail);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labSTime);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_HealthWarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NG下料报警";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frm_HealthWarm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.labDetail.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit labDetail;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labSTime;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView_Sloution;
        private System.Windows.Forms.ComboBox comboBox_Sloution;
        private DevExpress.XtraEditors.SimpleButton btn_静音;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
    }
}