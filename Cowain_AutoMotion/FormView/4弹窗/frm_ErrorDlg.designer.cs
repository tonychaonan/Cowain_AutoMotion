namespace Cowain_Form.FormView
{
    partial class frm_ErrorDlg
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
            this.components = new System.ComponentModel.Container();
            this.label_Title = new System.Windows.Forms.Label();
            this.Btn_Ok = new System.Windows.Forms.Button();
            this.comboBox_Sloution = new System.Windows.Forms.ComboBox();
            this.listView_Sloution = new System.Windows.Forms.ListView();
            this.label_station = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_異常說明 = new System.Windows.Forms.Label();
            this.btn_BuzzerOff = new System.Windows.Forms.Button();
            this.Btn_help = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labCategory = new DevExpress.XtraEditors.LabelControl();
            this.labCode = new DevExpress.XtraEditors.LabelControl();
            this.labSTime = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labUser = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labDetail = new DevExpress.XtraEditors.MemoEdit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.labDetail.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label_Title
            // 
            this.label_Title.BackColor = System.Drawing.Color.Red;
            this.label_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Title.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Title.Location = new System.Drawing.Point(557, 9);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(469, 45);
            this.label_Title.TabIndex = 5;
            this.label_Title.Text = "Error";
            this.label_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Title.Visible = false;
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.BackColor = System.Drawing.Color.White;
            this.Btn_Ok.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Btn_Ok.Image = global::Cowain_AutoMotion.Properties.Resources.SetOk;
            this.Btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_Ok.Location = new System.Drawing.Point(385, 515);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(99, 56);
            this.Btn_Ok.TabIndex = 6;
            this.Btn_Ok.Text = "确认";
            this.Btn_Ok.UseVisualStyleBackColor = false;
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // comboBox_Sloution
            // 
            this.comboBox_Sloution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Sloution.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_Sloution.FormattingEnabled = true;
            this.comboBox_Sloution.Location = new System.Drawing.Point(12, 24);
            this.comboBox_Sloution.Name = "comboBox_Sloution";
            this.comboBox_Sloution.Size = new System.Drawing.Size(445, 28);
            this.comboBox_Sloution.TabIndex = 200;
            this.comboBox_Sloution.SelectedIndexChanged += new System.EventHandler(this.comboBox_Sloution_SelectedIndexChanged);
            // 
            // listView_Sloution
            // 
            this.listView_Sloution.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Sloution.HideSelection = false;
            this.listView_Sloution.Location = new System.Drawing.Point(12, 54);
            this.listView_Sloution.Name = "listView_Sloution";
            this.listView_Sloution.Size = new System.Drawing.Size(445, 119);
            this.listView_Sloution.TabIndex = 201;
            this.listView_Sloution.UseCompatibleStateImageBehavior = false;
            this.listView_Sloution.View = System.Windows.Forms.View.List;
            this.listView_Sloution.SelectedIndexChanged += new System.EventHandler(this.listView_Sloution_SelectedIndexChanged);
            // 
            // label_station
            // 
            this.label_station.BackColor = System.Drawing.Color.Azure;
            this.label_station.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_station.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_station.Location = new System.Drawing.Point(12, 21);
            this.label_station.Name = "label_station";
            this.label_station.Size = new System.Drawing.Size(445, 37);
            this.label_station.TabIndex = 202;
            this.label_station.Text = "Type";
            this.label_station.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_Sloution);
            this.groupBox1.Controls.Add(this.comboBox_Sloution);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(15, 332);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(469, 177);
            this.groupBox1.TabIndex = 203;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "处理选项";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_station);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.Location = new System.Drawing.Point(557, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(469, 70);
            this.groupBox2.TabIndex = 204;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "机构&&站号";
            this.groupBox2.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label_異常說明);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox3.Location = new System.Drawing.Point(557, 133);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(469, 100);
            this.groupBox3.TabIndex = 205;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "异常说明";
            this.groupBox3.Visible = false;
            // 
            // label_異常說明
            // 
            this.label_異常說明.BackColor = System.Drawing.Color.Azure;
            this.label_異常說明.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_異常說明.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_異常說明.Location = new System.Drawing.Point(12, 21);
            this.label_異常說明.Name = "label_異常說明";
            this.label_異常說明.Size = new System.Drawing.Size(445, 76);
            this.label_異常說明.TabIndex = 203;
            this.label_異常說明.Text = "Type";
            this.label_異常說明.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_BuzzerOff
            // 
            this.btn_BuzzerOff.BackColor = System.Drawing.Color.White;
            this.btn_BuzzerOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_BuzzerOff.Image = global::Cowain_AutoMotion.Properties.Resources.buzzer_Stop;
            this.btn_BuzzerOff.Location = new System.Drawing.Point(27, 515);
            this.btn_BuzzerOff.Name = "btn_BuzzerOff";
            this.btn_BuzzerOff.Size = new System.Drawing.Size(99, 56);
            this.btn_BuzzerOff.TabIndex = 206;
            this.btn_BuzzerOff.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_BuzzerOff.UseVisualStyleBackColor = false;
            this.btn_BuzzerOff.Click += new System.EventHandler(this.btn_BuzzerOff_Click);
            // 
            // Btn_help
            // 
            this.Btn_help.BackColor = System.Drawing.Color.White;
            this.Btn_help.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Btn_help.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_help.Location = new System.Drawing.Point(206, 515);
            this.Btn_help.Name = "Btn_help";
            this.Btn_help.Size = new System.Drawing.Size(99, 56);
            this.Btn_help.TabIndex = 207;
            this.Btn_help.Text = "屏蔽胶阀报警";
            this.Btn_help.UseVisualStyleBackColor = false;
            this.Btn_help.Click += new System.EventHandler(this.Btn_help_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(50, 296);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(53, 24);
            this.labelControl8.TabIndex = 219;
            this.labelControl8.Text = "User:";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(50, 137);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(105, 24);
            this.labelControl3.TabIndex = 220;
            this.labelControl3.Text = "Error Msg:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(50, 83);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(114, 24);
            this.labelControl2.TabIndex = 221;
            this.labelControl2.Text = "Error Code:";
            // 
            // labCategory
            // 
            this.labCategory.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.labCategory.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCategory.Appearance.Options.UseBackColor = true;
            this.labCategory.Appearance.Options.UseFont = true;
            this.labCategory.Appearance.Options.UseTextOptions = true;
            this.labCategory.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labCategory.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labCategory.Location = new System.Drawing.Point(195, 130);
            this.labCategory.Name = "labCategory";
            this.labCategory.Size = new System.Drawing.Size(258, 37);
            this.labCategory.TabIndex = 223;
            this.labCategory.Text = "Material";
            // 
            // labCode
            // 
            this.labCode.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.labCode.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCode.Appearance.Options.UseBackColor = true;
            this.labCode.Appearance.Options.UseFont = true;
            this.labCode.Appearance.Options.UseTextOptions = true;
            this.labCode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labCode.Location = new System.Drawing.Point(195, 76);
            this.labCode.Name = "labCode";
            this.labCode.Size = new System.Drawing.Size(258, 37);
            this.labCode.TabIndex = 224;
            this.labCode.Text = "ERR2002";
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
            this.labSTime.Location = new System.Drawing.Point(195, 22);
            this.labSTime.Name = "labSTime";
            this.labSTime.Size = new System.Drawing.Size(259, 37);
            this.labSTime.TabIndex = 225;
            this.labSTime.Text = "2022/10/01 10:00:00";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(50, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(112, 24);
            this.labelControl1.TabIndex = 226;
            this.labelControl1.Text = "Start Time:";
            // 
            // labUser
            // 
            this.labUser.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.labUser.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labUser.Appearance.Options.UseBackColor = true;
            this.labUser.Appearance.Options.UseFont = true;
            this.labUser.Appearance.Options.UseTextOptions = true;
            this.labUser.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labUser.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labUser.Location = new System.Drawing.Point(195, 289);
            this.labUser.Name = "labUser";
            this.labUser.Size = new System.Drawing.Size(258, 37);
            this.labUser.TabIndex = 223;
            this.labUser.Click += new System.EventHandler(this.labUser_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(50, 193);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(123, 24);
            this.labelControl5.TabIndex = 220;
            this.labelControl5.Text = "Error Detail:";
            // 
            // labDetail
            // 
            this.labDetail.Location = new System.Drawing.Point(195, 183);
            this.labDetail.Name = "labDetail";
            this.labDetail.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.labDetail.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.labDetail.Properties.Appearance.Options.UseBackColor = true;
            this.labDetail.Properties.Appearance.Options.UseFont = true;
            this.labDetail.Properties.ReadOnly = true;
            this.labDetail.Size = new System.Drawing.Size(258, 96);
            this.labDetail.TabIndex = 227;
            // 
            // frm_ErrorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(160)))), ((int)(((byte)(150)))));
            this.ClientSize = new System.Drawing.Size(504, 575);
            this.ControlBox = false;
            this.Controls.Add(this.labDetail);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labUser);
            this.Controls.Add(this.labCategory);
            this.Controls.Add(this.labCode);
            this.Controls.Add(this.labSTime);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.Btn_help);
            this.Controls.Add(this.btn_BuzzerOff);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Btn_Ok);
            this.Controls.Add(this.label_Title);
            this.KeyPreview = true;
            this.Name = "frm_ErrorDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_ErrorDlg_FormClosing);
            this.Load += new System.EventHandler(this.frm_ErrorDlg_Load);
            this.Shown += new System.EventHandler(this.frm_ErrorDlg_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_ErrorDlg_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.labDetail.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.Button Btn_Ok;
        private System.Windows.Forms.ComboBox comboBox_Sloution;
        private System.Windows.Forms.ListView listView_Sloution;
        private System.Windows.Forms.Label label_station;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_異常說明;
        private System.Windows.Forms.Button btn_BuzzerOff;
        private System.Windows.Forms.Button Btn_help;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labCategory;
        private DevExpress.XtraEditors.LabelControl labCode;
        private DevExpress.XtraEditors.LabelControl labSTime;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labUser;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.MemoEdit labDetail;
    }
}