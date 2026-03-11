namespace Cowain_AutoMotion.FormView._4弹窗
{
    partial class frm_ErrorDown
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
            this.btn_ErrorDown_SendState = new System.Windows.Forms.Button();
            this.cbBox_ErrorDown = new System.Windows.Forms.ComboBox();
            this.cbBox_ErrorMessage = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_ErrorDown_SendErrorData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_ErrorDown_SendState
            // 
            this.btn_ErrorDown_SendState.Location = new System.Drawing.Point(45, 180);
            this.btn_ErrorDown_SendState.Name = "btn_ErrorDown_SendState";
            this.btn_ErrorDown_SendState.Size = new System.Drawing.Size(127, 45);
            this.btn_ErrorDown_SendState.TabIndex = 0;
            this.btn_ErrorDown_SendState.Text = "确认发送报警状态";
            this.btn_ErrorDown_SendState.UseVisualStyleBackColor = true;
            this.btn_ErrorDown_SendState.Click += new System.EventHandler(this.btn_ErrorDown_SendState_Click);
            // 
            // cbBox_ErrorDown
            // 
            this.cbBox_ErrorDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBox_ErrorDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBox_ErrorDown.FormattingEnabled = true;
            this.cbBox_ErrorDown.Items.AddRange(new object[] {
            "Cylinder Error_气缸报警",
            "Sensor Error_感应器报警",
            "SW Error_软件报警",
            "Vision Error_视觉报警",
            "Safety Error_安全门报警",
            "Scanning Error_扫码枪报警",
            "Motion Error_马达轴报警",
            "Glue Error_胶路报警",
            "Material Shortage Error_缺料报警",
            "MES Error_MES报警",
            "ExternalRunner Error_外部流道报警"});
            this.cbBox_ErrorDown.Location = new System.Drawing.Point(119, 70);
            this.cbBox_ErrorDown.Name = "cbBox_ErrorDown";
            this.cbBox_ErrorDown.Size = new System.Drawing.Size(272, 21);
            this.cbBox_ErrorDown.TabIndex = 1;
            this.cbBox_ErrorDown.SelectedIndexChanged += new System.EventHandler(this.cbBox_ErrorDown_SelectedIndexChanged);
            // 
            // cbBox_ErrorMessage
            // 
            this.cbBox_ErrorMessage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBox_ErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBox_ErrorMessage.FormattingEnabled = true;
            this.cbBox_ErrorMessage.Items.AddRange(new object[] {
            "Cylinder Error_气缸报警",
            "Sensor Error_感应器报警",
            "SW Error_软件报警",
            "Vision Error_视觉报警",
            "Safety Error_安全门报警",
            "Scanning Error_扫码枪报警",
            "Motion Error_马达轴报警",
            "Glue Error_胶路报警",
            "Material Shortage Error_缺料报警",
            "MES Error_MES报警",
            "Others Error_其它报警"});
            this.cbBox_ErrorMessage.Location = new System.Drawing.Point(119, 121);
            this.cbBox_ErrorMessage.Name = "cbBox_ErrorMessage";
            this.cbBox_ErrorMessage.Size = new System.Drawing.Size(272, 21);
            this.cbBox_ErrorMessage.TabIndex = 2;
            this.cbBox_ErrorMessage.SelectedIndexChanged += new System.EventHandler(this.cbBox_ErrorMessage_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "报警类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "报警内容：";
            // 
            // btn_ErrorDown_SendErrorData
            // 
            this.btn_ErrorDown_SendErrorData.Location = new System.Drawing.Point(264, 180);
            this.btn_ErrorDown_SendErrorData.Name = "btn_ErrorDown_SendErrorData";
            this.btn_ErrorDown_SendErrorData.Size = new System.Drawing.Size(127, 45);
            this.btn_ErrorDown_SendErrorData.TabIndex = 5;
            this.btn_ErrorDown_SendErrorData.Text = "结束报警状态";
            this.btn_ErrorDown_SendErrorData.UseVisualStyleBackColor = true;
            this.btn_ErrorDown_SendErrorData.Click += new System.EventHandler(this.btn_ErrorDown_SendErrorData_Click);
            // 
            // frm_ErrorDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 482);
            this.ControlBox = false;
            this.Controls.Add(this.btn_ErrorDown_SendErrorData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbBox_ErrorMessage);
            this.Controls.Add(this.cbBox_ErrorDown);
            this.Controls.Add(this.btn_ErrorDown_SendState);
            this.MaximumSize = new System.Drawing.Size(538, 521);
            this.MinimumSize = new System.Drawing.Size(538, 521);
            this.Name = "frm_ErrorDown";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ErrorDown";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_ErrorDown_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ErrorDown_SendState;
        private System.Windows.Forms.ComboBox cbBox_ErrorDown;
        private System.Windows.Forms.ComboBox cbBox_ErrorMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_ErrorDown_SendErrorData;
    }
}