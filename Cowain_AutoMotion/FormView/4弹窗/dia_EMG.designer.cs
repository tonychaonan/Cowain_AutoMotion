namespace Cowain_Form.FormView
{
    partial class dia_EMG
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
            this.label_Title = new System.Windows.Forms.Label();
            this.Btn_Ok = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_異常說明 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_BuzzerOff = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label_Title
            // 
            this.label_Title.BackColor = System.Drawing.Color.Red;
            this.label_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Title.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Title.Location = new System.Drawing.Point(15, 9);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(657, 45);
            this.label_Title.TabIndex = 5;
            this.label_Title.Text = "EMG";
            this.label_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.BackColor = System.Drawing.Color.White;
            this.Btn_Ok.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Btn_Ok.Image = global::Cowain_AutoMotion.Properties.Resources.ReCheck;
            this.Btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_Ok.Location = new System.Drawing.Point(446, 520);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(220, 61);
            this.Btn_Ok.TabIndex = 6;
            this.Btn_Ok.Text = "确认";
            this.Btn_Ok.UseVisualStyleBackColor = false;
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label_異常說明);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox3.Location = new System.Drawing.Point(15, 57);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(657, 77);
            this.groupBox3.TabIndex = 205;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "异常说明";
            // 
            // label_異常說明
            // 
            this.label_異常說明.BackColor = System.Drawing.Color.Azure;
            this.label_異常說明.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_異常說明.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_異常說明.Location = new System.Drawing.Point(12, 21);
            this.label_異常說明.Name = "label_異常說明";
            this.label_異常說明.Size = new System.Drawing.Size(639, 45);
            this.label_異常說明.TabIndex = 203;
            this.label_異常說明.Text = "急停讯号触发 - 请确认设备状况 , 解除后并进行设备回Home";
            this.label_異常說明.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Cowain_AutoMotion.Properties.Resources.EMG;
            this.pictureBox1.Location = new System.Drawing.Point(15, 140);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(651, 374);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 206;
            this.pictureBox1.TabStop = false;
            // 
            // btn_BuzzerOff
            // 
            this.btn_BuzzerOff.BackColor = System.Drawing.Color.White;
            this.btn_BuzzerOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_BuzzerOff.Image = global::Cowain_AutoMotion.Properties.Resources.buzzer_Stop;
            this.btn_BuzzerOff.Location = new System.Drawing.Point(15, 520);
            this.btn_BuzzerOff.Name = "btn_BuzzerOff";
            this.btn_BuzzerOff.Size = new System.Drawing.Size(206, 61);
            this.btn_BuzzerOff.TabIndex = 207;
            this.btn_BuzzerOff.UseVisualStyleBackColor = false;
            this.btn_BuzzerOff.Click += new System.EventHandler(this.btn_BuzzerOff_Click);
            // 
            // dia_EMG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 587);
            this.ControlBox = false;
            this.Controls.Add(this.btn_BuzzerOff);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.Btn_Ok);
            this.Controls.Add(this.label_Title);
            this.KeyPreview = true;
            this.Name = "dia_EMG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dia_EMG_FormClosing);
            this.Load += new System.EventHandler(this.frm_ErrorDlg_Load);
            this.Shown += new System.EventHandler(this.frm_ErrorDlg_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_ErrorDlg_KeyDown);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.Button Btn_Ok;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_異常說明;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_BuzzerOff;
    }
}