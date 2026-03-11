namespace Cowain_Form.FormView
{
    partial class dia_Login
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbxUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.btn_Canel = new System.Windows.Forms.Button();
            this.tx_newPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tx_OldPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.group_Password = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tx_UserName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox_UserName = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.group_Password.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Cowain_AutoMotion.Properties.Resources.LoginKey;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 130);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tbxUserName
            // 
            this.tbxUserName.Location = new System.Drawing.Point(242, 34);
            this.tbxUserName.Name = "tbxUserName";
            this.tbxUserName.Size = new System.Drawing.Size(89, 21);
            this.tbxUserName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(154, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Password:";
            // 
            // tbxPassword
            // 
            this.tbxPassword.Location = new System.Drawing.Point(242, 74);
            this.tbxPassword.Name = "tbxPassword";
            this.tbxPassword.Size = new System.Drawing.Size(173, 21);
            this.tbxPassword.TabIndex = 5;
            this.tbxPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(154, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "User Name:";
            // 
            // btn_Ok
            // 
            this.btn_Ok.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Ok.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Ok.Image = global::Cowain_AutoMotion.Properties.Resources.SetOk;
            this.btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Ok.Location = new System.Drawing.Point(235, 109);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(83, 43);
            this.btn_Ok.TabIndex = 242;
            this.btn_Ok.Text = "       确认";
            this.btn_Ok.UseVisualStyleBackColor = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // btn_Canel
            // 
            this.btn_Canel.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Canel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Canel.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Canel.Image = global::Cowain_AutoMotion.Properties.Resources.SetCanel;
            this.btn_Canel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Canel.Location = new System.Drawing.Point(329, 109);
            this.btn_Canel.Name = "btn_Canel";
            this.btn_Canel.Size = new System.Drawing.Size(86, 43);
            this.btn_Canel.TabIndex = 243;
            this.btn_Canel.Text = "       取消";
            this.btn_Canel.UseVisualStyleBackColor = false;
            this.btn_Canel.Click += new System.EventHandler(this.btn_Canel_Click);
            // 
            // tx_newPassword
            // 
            this.tx_newPassword.Location = new System.Drawing.Point(111, 69);
            this.tx_newPassword.Name = "tx_newPassword";
            this.tx_newPassword.Size = new System.Drawing.Size(180, 21);
            this.tx_newPassword.TabIndex = 245;
            this.tx_newPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(23, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 18);
            this.label3.TabIndex = 244;
            this.label3.Text = "新Password:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Window;
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Image = global::Cowain_AutoMotion.Properties.Resources.SetOk;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(208, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 35);
            this.button1.TabIndex = 247;
            this.button1.Text = "       确认";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tx_OldPassword
            // 
            this.tx_OldPassword.Location = new System.Drawing.Point(111, 44);
            this.tx_OldPassword.Name = "tx_OldPassword";
            this.tx_OldPassword.Size = new System.Drawing.Size(180, 21);
            this.tx_OldPassword.TabIndex = 249;
            this.tx_OldPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(23, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 18);
            this.label4.TabIndex = 248;
            this.label4.Text = "舊Password:";
            // 
            // group_Password
            // 
            this.group_Password.Controls.Add(this.label5);
            this.group_Password.Controls.Add(this.tx_UserName);
            this.group_Password.Controls.Add(this.tx_OldPassword);
            this.group_Password.Controls.Add(this.label3);
            this.group_Password.Controls.Add(this.label4);
            this.group_Password.Controls.Add(this.tx_newPassword);
            this.group_Password.Controls.Add(this.button1);
            this.group_Password.Location = new System.Drawing.Point(124, 177);
            this.group_Password.Name = "group_Password";
            this.group_Password.Size = new System.Drawing.Size(307, 141);
            this.group_Password.TabIndex = 292;
            this.group_Password.TabStop = false;
            this.group_Password.Text = "密碼變更";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(23, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 18);
            this.label5.TabIndex = 295;
            this.label5.Text = "User Name:";
            // 
            // tx_UserName
            // 
            this.tx_UserName.Location = new System.Drawing.Point(111, 17);
            this.tx_UserName.Name = "tx_UserName";
            this.tx_UserName.Size = new System.Drawing.Size(180, 21);
            this.tx_UserName.TabIndex = 294;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Window;
            this.button2.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(388, 161);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 10);
            this.button2.TabIndex = 293;
            this.button2.Text = "v";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox_UserName
            // 
            this.comboBox_UserName.BackColor = System.Drawing.Color.Thistle;
            this.comboBox_UserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_UserName.FormattingEnabled = true;
            this.comboBox_UserName.Items.AddRange(new object[] {
            "操作者",
            "工程师",
            "制造商"});
            this.comboBox_UserName.Location = new System.Drawing.Point(334, 34);
            this.comboBox_UserName.Name = "comboBox_UserName";
            this.comboBox_UserName.Size = new System.Drawing.Size(81, 24);
            this.comboBox_UserName.TabIndex = 328;
            this.comboBox_UserName.SelectedIndexChanged += new System.EventHandler(this.comboBox_UserName_SelectedIndexChanged);
            // 
            // dia_Login
            // 
            this.AcceptButton = this.btn_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Canel;
            this.ClientSize = new System.Drawing.Size(443, 330);
            this.ControlBox = false;
            this.Controls.Add(this.comboBox_UserName);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.group_Password);
            this.Controls.Add(this.btn_Canel);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.tbxPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "dia_Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.dia_Login_Load);
            this.Shown += new System.EventHandler(this.dia_Login_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.group_Password.ResumeLayout(false);
            this.group_Password.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbxUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Ok;
        private System.Windows.Forms.Button btn_Canel;
        private System.Windows.Forms.TextBox tx_newPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tx_OldPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox group_Password;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tx_UserName;
        private System.Windows.Forms.ComboBox comboBox_UserName;
    }
}