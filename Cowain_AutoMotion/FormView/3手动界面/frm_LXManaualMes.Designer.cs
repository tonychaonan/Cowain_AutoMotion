namespace Cowain_Form.FormView
{
    partial class frm_LXManaualMes
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
            this.button1 = new System.Windows.Forms.Button();
            this.tb_uc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_sn = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_PDCA = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comBox_Type = new System.Windows.Forms.ComboBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.UC_NO = new System.Windows.Forms.ComboBox();
            this.button9 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.lb_FailSNInfo = new System.Windows.Forms.ListBox();
            this.button7 = new System.Windows.Forms.Button();
            this.btn_OpenPicFailCsv = new System.Windows.Forms.Button();
            this.tb_send = new System.Windows.Forms.TextBox();
            this.tb_receive = new System.Windows.Forms.TextBox();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(17, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "获取SN";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_uc
            // 
            this.tb_uc.Location = new System.Drawing.Point(151, 78);
            this.tb_uc.Multiline = true;
            this.tb_uc.Name = "tb_uc";
            this.tb_uc.Size = new System.Drawing.Size(280, 22);
            this.tb_uc.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(104, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "UC码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(104, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "SN码：";
            // 
            // tb_sn
            // 
            this.tb_sn.Location = new System.Drawing.Point(151, 108);
            this.tb_sn.Multiline = true;
            this.tb_sn.Name = "tb_sn";
            this.tb_sn.Size = new System.Drawing.Size(280, 21);
            this.tb_sn.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(17, 104);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 27);
            this.button2.TabIndex = 5;
            this.button2.Text = "Check SN";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_PDCA
            // 
            this.btn_PDCA.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_PDCA.Location = new System.Drawing.Point(17, 162);
            this.btn_PDCA.Name = "btn_PDCA";
            this.btn_PDCA.Size = new System.Drawing.Size(78, 28);
            this.btn_PDCA.TabIndex = 8;
            this.btn_PDCA.Text = "上传PDCA";
            this.btn_PDCA.UseVisualStyleBackColor = true;
            this.btn_PDCA.Click += new System.EventHandler(this.btn_PDCA_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.Location = new System.Drawing.Point(17, 133);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 27);
            this.button4.TabIndex = 11;
            this.button4.Text = "上传MES";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(16, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "发送数据：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(461, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "接收数据：";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.Location = new System.Drawing.Point(107, 162);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(121, 28);
            this.button5.TabIndex = 16;
            this.button5.Text = "显示PDCA反馈数据";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(104, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "选择模式：";
            // 
            // comBox_Type
            // 
            this.comBox_Type.FormattingEnabled = true;
            this.comBox_Type.Items.AddRange(new object[] {
            "0",
            "1"});
            this.comBox_Type.Location = new System.Drawing.Point(173, 137);
            this.comBox_Type.Name = "comBox_Type";
            this.comBox_Type.Size = new System.Drawing.Size(68, 18);
            this.comBox_Type.TabIndex = 19;
            this.comBox_Type.Text = "0";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.UC_NO);
            this.groupBox11.Controls.Add(this.button9);
            this.groupBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox11.Location = new System.Drawing.Point(17, 8);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(211, 62);
            this.groupBox11.TabIndex = 471;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "扫码测试";
            // 
            // UC_NO
            // 
            this.UC_NO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UC_NO.FormattingEnabled = true;
            this.UC_NO.Items.AddRange(new object[] {
            "左流道",
            "右流道"});
            this.UC_NO.Location = new System.Drawing.Point(6, 23);
            this.UC_NO.Name = "UC_NO";
            this.UC_NO.Size = new System.Drawing.Size(82, 25);
            this.UC_NO.TabIndex = 308;
            this.UC_NO.Text = "左流道";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(104, 19);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(77, 30);
            this.button9.TabIndex = 454;
            this.button9.Text = "触发";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(259, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(160, 10);
            this.label9.TabIndex = 472;
            this.label9.Text = "0:REC(有mini)   1: ADD（无mini)";
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button6.Location = new System.Drawing.Point(261, 162);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(126, 28);
            this.button6.TabIndex = 473;
            this.button6.Text = "手动测试上传图片";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // lb_FailSNInfo
            // 
            this.lb_FailSNInfo.FormattingEnabled = true;
            this.lb_FailSNInfo.HorizontalScrollbar = true;
            this.lb_FailSNInfo.ItemHeight = 10;
            this.lb_FailSNInfo.Items.AddRange(new object[] {
            " "});
            this.lb_FailSNInfo.Location = new System.Drawing.Point(955, 15);
            this.lb_FailSNInfo.Name = "lb_FailSNInfo";
            this.lb_FailSNInfo.ScrollAlwaysVisible = true;
            this.lb_FailSNInfo.Size = new System.Drawing.Size(305, 434);
            this.lb_FailSNInfo.TabIndex = 478;
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button7.Location = new System.Drawing.Point(788, 112);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(154, 40);
            this.button7.TabIndex = 477;
            this.button7.Text = "大量重新上传图片";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // btn_OpenPicFailCsv
            // 
            this.btn_OpenPicFailCsv.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_OpenPicFailCsv.Location = new System.Drawing.Point(788, 41);
            this.btn_OpenPicFailCsv.Name = "btn_OpenPicFailCsv";
            this.btn_OpenPicFailCsv.Size = new System.Drawing.Size(154, 40);
            this.btn_OpenPicFailCsv.TabIndex = 476;
            this.btn_OpenPicFailCsv.Text = "加载传图失败信息";
            this.btn_OpenPicFailCsv.UseVisualStyleBackColor = true;
            // 
            // tb_send
            // 
            this.tb_send.Location = new System.Drawing.Point(5, 225);
            this.tb_send.Multiline = true;
            this.tb_send.Name = "tb_send";
            this.tb_send.Size = new System.Drawing.Size(454, 234);
            this.tb_send.TabIndex = 479;
            // 
            // tb_receive
            // 
            this.tb_receive.Location = new System.Drawing.Point(465, 225);
            this.tb_receive.Multiline = true;
            this.tb_receive.Name = "tb_receive";
            this.tb_receive.Size = new System.Drawing.Size(454, 234);
            this.tb_receive.TabIndex = 480;
            // 
            // frm_LXManaualMes
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1276, 513);
            this.Controls.Add(this.tb_receive);
            this.Controls.Add(this.tb_send);
            this.Controls.Add(this.lb_FailSNInfo);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.btn_OpenPicFailCsv);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.comBox_Type);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btn_PDCA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_sn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_uc);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("新宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_LXManaualMes";
            this.Text = "fimManaualMes";
            this.Load += new System.EventHandler(this.frm_LXManaualMes_Load);
            this.groupBox11.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_uc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_sn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_PDCA;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comBox_Type;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.ComboBox UC_NO;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ListBox lb_FailSNInfo;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btn_OpenPicFailCsv;
        private System.Windows.Forms.TextBox tb_send;
        private System.Windows.Forms.TextBox tb_receive;
    }
}