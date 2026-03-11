namespace Cowain_Form.FormView
{
    partial class frm_Auto
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
            this.timer_Reflash = new System.Windows.Forms.Timer(this.components);
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Auto = new System.Windows.Forms.Button();
            this.btn_Pause = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.tx_OpIDFile = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.textBox_UserID = new System.Windows.Forms.TextBox();
            this.btnPDCA = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.btnCCD1Line = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.btn_CycleStop = new System.Windows.Forms.Button();
            this.timer_Step = new System.Windows.Forms.Timer(this.components);
            this.label37 = new System.Windows.Forms.Label();
            this.txtLeftSN = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.txtLeftUC = new System.Windows.Forms.TextBox();
            this.tx_ZStopDisCardCounts = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txt_macchine = new System.Windows.Forms.Label();
            this.lbStationName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timer_CheckStationAndSW = new System.Windows.Forms.Timer(this.components);
            this.Bt_mesupload1 = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tx_CycleTime_Left = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清空显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.richTextBox8 = new System.Windows.Forms.RichTextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRobot = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConnectOtherMachine = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btSignalPDCA = new System.Windows.Forms.Button();
            this.label60 = new System.Windows.Forms.Label();
            this.btSignalHIVE = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.btSignalCCD = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.btSignalMES = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btSignal扫码 = new System.Windows.Forms.Button();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.button_HIVE = new System.Windows.Forms.Button();
            this.label68 = new System.Windows.Forms.Label();
            this.bt_mesStatus = new System.Windows.Forms.Button();
            this.label57 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_Reflash
            // 
            this.timer_Reflash.Tick += new System.EventHandler(this.timer_Reflash_Tick);
            // 
            // btn_Stop
            // 
            this.btn_Stop.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Stop.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Stop.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_Stop;
            this.btn_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Stop.Location = new System.Drawing.Point(1229, 281);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(109, 63);
            this.btn_Stop.TabIndex = 240;
            this.btn_Stop.Text = "       停止";
            this.btn_Stop.UseVisualStyleBackColor = false;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Auto
            // 
            this.btn_Auto.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Auto.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Auto.Image = global::Cowain_AutoMotion.Properties.Resources.Repeat;
            this.btn_Auto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Auto.Location = new System.Drawing.Point(1229, 59);
            this.btn_Auto.Name = "btn_Auto";
            this.btn_Auto.Size = new System.Drawing.Size(109, 60);
            this.btn_Auto.TabIndex = 241;
            this.btn_Auto.Text = "       自动";
            this.btn_Auto.UseVisualStyleBackColor = false;
            this.btn_Auto.Click += new System.EventHandler(this.btn_Auto_Click);
            // 
            // btn_Pause
            // 
            this.btn_Pause.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Pause.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Pause.Image = global::Cowain_AutoMotion.Properties.Resources.pause;
            this.btn_Pause.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Pause.Location = new System.Drawing.Point(1229, 208);
            this.btn_Pause.Name = "btn_Pause";
            this.btn_Pause.Size = new System.Drawing.Size(109, 60);
            this.btn_Pause.TabIndex = 242;
            this.btn_Pause.Text = "       暂停";
            this.btn_Pause.UseVisualStyleBackColor = false;
            this.btn_Pause.Click += new System.EventHandler(this.btn_Pause_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.tx_OpIDFile);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label40);
            this.groupBox3.Controls.Add(this.textBox_UserID);
            this.groupBox3.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox3.Location = new System.Drawing.Point(4, -9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(368, 44);
            this.groupBox3.TabIndex = 276;
            this.groupBox3.TabStop = false;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Gray;
            this.button8.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button8.Location = new System.Drawing.Point(495, 64);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(52, 28);
            this.button8.TabIndex = 370;
            this.button8.Text = "load";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // tx_OpIDFile
            // 
            this.tx_OpIDFile.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tx_OpIDFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tx_OpIDFile.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_OpIDFile.Location = new System.Drawing.Point(165, 68);
            this.tx_OpIDFile.Name = "tx_OpIDFile";
            this.tx_OpIDFile.Size = new System.Drawing.Size(318, 27);
            this.tx_OpIDFile.TabIndex = 369;
            this.tx_OpIDFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 73);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(153, 19);
            this.label12.TabIndex = 368;
            this.label12.Text = "使用者ID档案来源:";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.Location = new System.Drawing.Point(2, 19);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(95, 19);
            this.label40.TabIndex = 277;
            this.label40.Text = "使用者 ID: ";
            // 
            // textBox_UserID
            // 
            this.textBox_UserID.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBox_UserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_UserID.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_UserID.Location = new System.Drawing.Point(112, 14);
            this.textBox_UserID.Name = "textBox_UserID";
            this.textBox_UserID.Size = new System.Drawing.Size(242, 27);
            this.textBox_UserID.TabIndex = 276;
            this.textBox_UserID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnPDCA
            // 
            this.btnPDCA.AutoSize = true;
            this.btnPDCA.BackColor = System.Drawing.Color.Red;
            this.btnPDCA.Location = new System.Drawing.Point(80, 9);
            this.btnPDCA.Name = "btnPDCA";
            this.btnPDCA.Size = new System.Drawing.Size(77, 30);
            this.btnPDCA.TabIndex = 375;
            this.btnPDCA.Text = "Off Line";
            this.btnPDCA.UseVisualStyleBackColor = false;
            this.btnPDCA.Click += new System.EventHandler(this.btnPDCA_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(13, 19);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(66, 11);
            this.label25.TabIndex = 374;
            this.label25.Text = "PDCA网络:";
            // 
            // btnCCD1Line
            // 
            this.btnCCD1Line.AutoSize = true;
            this.btnCCD1Line.BackColor = System.Drawing.Color.Red;
            this.btnCCD1Line.Location = new System.Drawing.Point(211, 9);
            this.btnCCD1Line.Name = "btnCCD1Line";
            this.btnCCD1Line.Size = new System.Drawing.Size(78, 30);
            this.btnCCD1Line.TabIndex = 373;
            this.btnCCD1Line.Text = "Off Line";
            this.btnCCD1Line.UseVisualStyleBackColor = false;
            this.btnCCD1Line.Click += new System.EventHandler(this.btnCCD1Line_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(165, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 11);
            this.label15.TabIndex = 372;
            this.label15.Text = "相机1:";
            // 
            // btn_CycleStop
            // 
            this.btn_CycleStop.BackColor = System.Drawing.SystemColors.Window;
            this.btn_CycleStop.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_CycleStop.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_Stop;
            this.btn_CycleStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_CycleStop.Location = new System.Drawing.Point(1229, 132);
            this.btn_CycleStop.Name = "btn_CycleStop";
            this.btn_CycleStop.Size = new System.Drawing.Size(109, 63);
            this.btn_CycleStop.TabIndex = 291;
            this.btn_CycleStop.Text = "      循环停止";
            this.btn_CycleStop.UseVisualStyleBackColor = false;
            this.btn_CycleStop.Click += new System.EventHandler(this.btn_CycleStop_Click);
            // 
            // timer_Step
            // 
            this.timer_Step.Tick += new System.EventHandler(this.timer_Step_Tick);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label37.Location = new System.Drawing.Point(21, 67);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(23, 11);
            this.label37.TabIndex = 464;
            this.label37.Text = "SN:";
            // 
            // txtLeftSN
            // 
            this.txtLeftSN.Location = new System.Drawing.Point(63, 65);
            this.txtLeftSN.Name = "txtLeftSN";
            this.txtLeftSN.Size = new System.Drawing.Size(214, 20);
            this.txtLeftSN.TabIndex = 461;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label35.Location = new System.Drawing.Point(21, 41);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(23, 11);
            this.label35.TabIndex = 460;
            this.label35.Text = "UC:";
            // 
            // txtLeftUC
            // 
            this.txtLeftUC.Location = new System.Drawing.Point(63, 38);
            this.txtLeftUC.Name = "txtLeftUC";
            this.txtLeftUC.Size = new System.Drawing.Size(214, 20);
            this.txtLeftUC.TabIndex = 457;
            // 
            // tx_ZStopDisCardCounts
            // 
            this.tx_ZStopDisCardCounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tx_ZStopDisCardCounts.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tx_ZStopDisCardCounts.Location = new System.Drawing.Point(37, 59);
            this.tx_ZStopDisCardCounts.Name = "tx_ZStopDisCardCounts";
            this.tx_ZStopDisCardCounts.Size = new System.Drawing.Size(74, 21);
            this.tx_ZStopDisCardCounts.TabIndex = 451;
            this.tx_ZStopDisCardCounts.Text = "0";
            this.tx_ZStopDisCardCounts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.Location = new System.Drawing.Point(6, 38);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(65, 12);
            this.label23.TabIndex = 450;
            this.label23.Text = "生产数量: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(368, 294);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 352;
            this.pictureBox1.TabStop = false;
            // 
            // txt_macchine
            // 
            this.txt_macchine.AutoEllipsis = true;
            this.txt_macchine.AutoSize = true;
            this.txt_macchine.Font = new System.Drawing.Font("新宋体", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_macchine.Location = new System.Drawing.Point(1179, 9);
            this.txt_macchine.Name = "txt_macchine";
            this.txt_macchine.Size = new System.Drawing.Size(159, 35);
            this.txt_macchine.TabIndex = 365;
            this.txt_macchine.Text = "机台状态";
            // 
            // lbStationName
            // 
            this.lbStationName.AutoSize = true;
            this.lbStationName.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbStationName.Location = new System.Drawing.Point(1002, 17);
            this.lbStationName.Name = "lbStationName";
            this.lbStationName.Size = new System.Drawing.Size(127, 28);
            this.lbStationName.TabIndex = 368;
            this.lbStationName.Text = "LH010        ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(934, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 28);
            this.label4.TabIndex = 367;
            this.label4.Text = "站别:";
            // 
            // timer_CheckStationAndSW
            // 
            this.timer_CheckStationAndSW.Enabled = true;
            this.timer_CheckStationAndSW.Interval = 1200000;
            this.timer_CheckStationAndSW.Tick += new System.EventHandler(this.timer_CheckStationAndSW_Tick);
            // 
            // Bt_mesupload1
            // 
            this.Bt_mesupload1.BackColor = System.Drawing.Color.Lime;
            this.Bt_mesupload1.Enabled = false;
            this.Bt_mesupload1.Font = new System.Drawing.Font("新宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_mesupload1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Bt_mesupload1.Location = new System.Drawing.Point(171, 59);
            this.Bt_mesupload1.Name = "Bt_mesupload1";
            this.Bt_mesupload1.Size = new System.Drawing.Size(78, 49);
            this.Bt_mesupload1.TabIndex = 364;
            this.Bt_mesupload1.Text = "PASS";
            this.Bt_mesupload1.UseVisualStyleBackColor = false;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Location = new System.Drawing.Point(4, 347);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1341, 259);
            this.tabControl2.TabIndex = 374;
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage7.Controls.Add(this.groupBox17);
            this.tabPage7.Controls.Add(this.groupBox15);
            this.tabPage7.Location = new System.Drawing.Point(4, 21);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(1333, 234);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "生产信息";
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.Bt_mesupload1);
            this.groupBox17.Controls.Add(this.label10);
            this.groupBox17.Controls.Add(this.tx_ZStopDisCardCounts);
            this.groupBox17.Controls.Add(this.tx_CycleTime_Left);
            this.groupBox17.Controls.Add(this.label23);
            this.groupBox17.Controls.Add(this.label22);
            this.groupBox17.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox17.Location = new System.Drawing.Point(8, 11);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(279, 217);
            this.groupBox17.TabIndex = 456;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "主要信息";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(151, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 456;
            this.label10.Text = "结果 :";
            // 
            // tx_CycleTime_Left
            // 
            this.tx_CycleTime_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tx_CycleTime_Left.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tx_CycleTime_Left.Location = new System.Drawing.Point(37, 109);
            this.tx_CycleTime_Left.Name = "tx_CycleTime_Left";
            this.tx_CycleTime_Left.Size = new System.Drawing.Size(74, 21);
            this.tx_CycleTime_Left.TabIndex = 449;
            this.tx_CycleTime_Left.Text = "0";
            this.tx_CycleTime_Left.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label22.Location = new System.Drawing.Point(3, 87);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 12);
            this.label22.TabIndex = 452;
            this.label22.Text = "CT :";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.txtLeftUC);
            this.groupBox15.Controls.Add(this.label35);
            this.groupBox15.Controls.Add(this.txtLeftSN);
            this.groupBox15.Controls.Add(this.label37);
            this.groupBox15.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox15.Location = new System.Drawing.Point(308, 11);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(298, 217);
            this.groupBox15.TabIndex = 331;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "产品码信息";
            // 
            // tabPage9
            // 
            this.tabPage9.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage9.Location = new System.Drawing.Point(4, 21);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new System.Drawing.Size(1333, 234);
            this.tabPage9.TabIndex = 2;
            this.tabPage9.Text = "轴位置信息";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage11);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(523, 297);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(515, 271);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "主监控";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(509, 265);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空显示ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // 清空显示ToolStripMenuItem
            // 
            this.清空显示ToolStripMenuItem.Name = "清空显示ToolStripMenuItem";
            this.清空显示ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.清空显示ToolStripMenuItem.Text = "清空显示";
            this.清空显示ToolStripMenuItem.Click += new System.EventHandler(this.清空显示ToolStripMenuItem_Click);
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.richTextBox8);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(515, 271);
            this.tabPage11.TabIndex = 7;
            this.tabPage11.Text = "设备归零";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // richTextBox8
            // 
            this.richTextBox8.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox8.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox8.Location = new System.Drawing.Point(0, 0);
            this.richTextBox8.Name = "richTextBox8";
            this.richTextBox8.ReadOnly = true;
            this.richTextBox8.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox8.Size = new System.Drawing.Size(515, 271);
            this.richTextBox8.TabIndex = 1;
            this.richTextBox8.Text = "";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox14);
            this.groupBox6.Controls.Add(this.groupBox13);
            this.groupBox6.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox6.Location = new System.Drawing.Point(4, 41);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(327, 309);
            this.groupBox6.TabIndex = 457;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "设备状态";
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label3);
            this.groupBox14.Controls.Add(this.btnRobot);
            this.groupBox14.Controls.Add(this.label2);
            this.groupBox14.Controls.Add(this.btnConnectOtherMachine);
            this.groupBox14.Controls.Add(this.label1);
            this.groupBox14.Controls.Add(this.btSignalPDCA);
            this.groupBox14.Controls.Add(this.label60);
            this.groupBox14.Controls.Add(this.btSignalHIVE);
            this.groupBox14.Controls.Add(this.label21);
            this.groupBox14.Controls.Add(this.btSignalCCD);
            this.groupBox14.Controls.Add(this.label19);
            this.groupBox14.Controls.Add(this.btSignalMES);
            this.groupBox14.Controls.Add(this.label14);
            this.groupBox14.Controls.Add(this.btSignal扫码);
            this.groupBox14.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox14.Location = new System.Drawing.Point(5, 16);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(301, 189);
            this.groupBox14.TabIndex = 459;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "控制状态";
            this.groupBox14.Enter += new System.EventHandler(this.groupBox14_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(162, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 396;
            this.label3.Text = "机器人";
            // 
            // btnRobot
            // 
            this.btnRobot.BackColor = System.Drawing.Color.Gray;
            this.btnRobot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRobot.Enabled = false;
            this.btnRobot.FlatAppearance.BorderSize = 0;
            this.btnRobot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRobot.Location = new System.Drawing.Point(131, 102);
            this.btnRobot.Name = "btnRobot";
            this.btnRobot.Size = new System.Drawing.Size(23, 23);
            this.btnRobot.TabIndex = 395;
            this.btnRobot.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(162, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 394;
            this.label2.Text = "串机";
            // 
            // btnConnectOtherMachine
            // 
            this.btnConnectOtherMachine.BackColor = System.Drawing.Color.Gray;
            this.btnConnectOtherMachine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnectOtherMachine.Enabled = false;
            this.btnConnectOtherMachine.FlatAppearance.BorderSize = 0;
            this.btnConnectOtherMachine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectOtherMachine.Location = new System.Drawing.Point(131, 63);
            this.btnConnectOtherMachine.Name = "btnConnectOtherMachine";
            this.btnConnectOtherMachine.Size = new System.Drawing.Size(23, 23);
            this.btnConnectOtherMachine.TabIndex = 393;
            this.btnConnectOtherMachine.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(162, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 392;
            this.label1.Text = "PDCA";
            // 
            // btSignalPDCA
            // 
            this.btSignalPDCA.BackColor = System.Drawing.Color.Gray;
            this.btSignalPDCA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btSignalPDCA.Enabled = false;
            this.btSignalPDCA.FlatAppearance.BorderSize = 0;
            this.btSignalPDCA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSignalPDCA.Location = new System.Drawing.Point(131, 31);
            this.btSignalPDCA.Name = "btSignalPDCA";
            this.btSignalPDCA.Size = new System.Drawing.Size(23, 23);
            this.btSignalPDCA.TabIndex = 391;
            this.btSignalPDCA.UseVisualStyleBackColor = false;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label60.Location = new System.Drawing.Point(53, 141);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(29, 12);
            this.label60.TabIndex = 390;
            this.label60.Text = "HIVE";
            // 
            // btSignalHIVE
            // 
            this.btSignalHIVE.BackColor = System.Drawing.Color.Gray;
            this.btSignalHIVE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btSignalHIVE.Enabled = false;
            this.btSignalHIVE.FlatAppearance.BorderSize = 0;
            this.btSignalHIVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSignalHIVE.Location = new System.Drawing.Point(22, 137);
            this.btSignalHIVE.Name = "btSignalHIVE";
            this.btSignalHIVE.Size = new System.Drawing.Size(23, 23);
            this.btSignalHIVE.TabIndex = 389;
            this.btSignalHIVE.UseVisualStyleBackColor = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.Location = new System.Drawing.Point(54, 105);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(47, 12);
            this.label21.TabIndex = 370;
            this.label21.Text = "CCD相机";
            // 
            // btSignalCCD
            // 
            this.btSignalCCD.BackColor = System.Drawing.Color.Gray;
            this.btSignalCCD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btSignalCCD.Enabled = false;
            this.btSignalCCD.FlatAppearance.BorderSize = 0;
            this.btSignalCCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSignalCCD.Location = new System.Drawing.Point(23, 101);
            this.btSignalCCD.Name = "btSignalCCD";
            this.btSignalCCD.Size = new System.Drawing.Size(23, 23);
            this.btSignalCCD.TabIndex = 369;
            this.btSignalCCD.UseVisualStyleBackColor = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(54, 67);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(23, 12);
            this.label19.TabIndex = 368;
            this.label19.Text = "MES";
            // 
            // btSignalMES
            // 
            this.btSignalMES.BackColor = System.Drawing.Color.Gray;
            this.btSignalMES.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btSignalMES.Enabled = false;
            this.btSignalMES.FlatAppearance.BorderSize = 0;
            this.btSignalMES.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSignalMES.Location = new System.Drawing.Point(23, 63);
            this.btSignalMES.Name = "btSignalMES";
            this.btSignalMES.Size = new System.Drawing.Size(23, 23);
            this.btSignalMES.TabIndex = 367;
            this.btSignalMES.UseVisualStyleBackColor = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(53, 33);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 366;
            this.label14.Text = "扫码";
            // 
            // btSignal扫码
            // 
            this.btSignal扫码.BackColor = System.Drawing.Color.Gray;
            this.btSignal扫码.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btSignal扫码.Enabled = false;
            this.btSignal扫码.FlatAppearance.BorderSize = 0;
            this.btSignal扫码.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSignal扫码.Location = new System.Drawing.Point(22, 29);
            this.btSignal扫码.Name = "btSignal扫码";
            this.btSignal扫码.Size = new System.Drawing.Size(23, 23);
            this.btSignal扫码.TabIndex = 159;
            this.btSignal扫码.UseVisualStyleBackColor = false;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label6);
            this.groupBox13.Controls.Add(this.btnScan);
            this.groupBox13.Controls.Add(this.button_HIVE);
            this.groupBox13.Controls.Add(this.label68);
            this.groupBox13.Controls.Add(this.bt_mesStatus);
            this.groupBox13.Controls.Add(this.label57);
            this.groupBox13.Controls.Add(this.btnPDCA);
            this.groupBox13.Controls.Add(this.label15);
            this.groupBox13.Controls.Add(this.label25);
            this.groupBox13.Controls.Add(this.btnCCD1Line);
            this.groupBox13.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox13.Location = new System.Drawing.Point(5, 207);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(298, 99);
            this.groupBox13.TabIndex = 458;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "硬件状态";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 11);
            this.label6.TabIndex = 384;
            this.label6.Text = "扫码枪:";
            // 
            // btnScan
            // 
            this.btnScan.AutoSize = true;
            this.btnScan.BackColor = System.Drawing.Color.Red;
            this.btnScan.Location = new System.Drawing.Point(211, 43);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(78, 28);
            this.btnScan.TabIndex = 385;
            this.btnScan.Text = "Off Line";
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // button_HIVE
            // 
            this.button_HIVE.AutoSize = true;
            this.button_HIVE.BackColor = System.Drawing.Color.Red;
            this.button_HIVE.Location = new System.Drawing.Point(79, 69);
            this.button_HIVE.Name = "button_HIVE";
            this.button_HIVE.Size = new System.Drawing.Size(78, 28);
            this.button_HIVE.TabIndex = 383;
            this.button_HIVE.Text = "Off Line";
            this.button_HIVE.UseVisualStyleBackColor = false;
            this.button_HIVE.Click += new System.EventHandler(this.button_HIVE_Click);
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(37, 77);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(40, 11);
            this.label68.TabIndex = 382;
            this.label68.Text = "HIVE:";
            // 
            // bt_mesStatus
            // 
            this.bt_mesStatus.AutoSize = true;
            this.bt_mesStatus.BackColor = System.Drawing.Color.Red;
            this.bt_mesStatus.Location = new System.Drawing.Point(79, 40);
            this.bt_mesStatus.Name = "bt_mesStatus";
            this.bt_mesStatus.Size = new System.Drawing.Size(78, 28);
            this.bt_mesStatus.TabIndex = 381;
            this.bt_mesStatus.Text = "Off Line";
            this.bt_mesStatus.UseVisualStyleBackColor = false;
            this.bt_mesStatus.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(20, 49);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(59, 11);
            this.label57.TabIndex = 380;
            this.label57.Text = "MES网络:";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.tabControl1);
            this.groupBox11.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox11.Location = new System.Drawing.Point(694, 41);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(529, 316);
            this.groupBox11.TabIndex = 458;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "站位信息";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.pictureBox1);
            this.groupBox12.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox12.Location = new System.Drawing.Point(313, 41);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(374, 313);
            this.groupBox12.TabIndex = 459;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "图片";
            // 
            // frm_Auto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 620);
            this.Controls.Add(this.txt_macchine);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.lbStationName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_CycleStop);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_Pause);
            this.Controls.Add(this.btn_Auto);
            this.Controls.Add(this.btn_Stop);
            this.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Auto";
            this.Text = "frm_Auto";
            this.Load += new System.EventHandler(this.frm_Auto_Load);
            this.VisibleChanged += new System.EventHandler(this.frm_Auto_VisibleChanged);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer_Reflash;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Auto;
        private System.Windows.Forms.Button btn_Pause;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_CycleStop;
        private System.Windows.Forms.Timer timer_Step;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox textBox_UserID;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox tx_OpIDFile;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tx_ZStopDisCardCounts;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label txt_macchine;
        private System.Windows.Forms.Label lbStationName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPDCA;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btnCCD1Line;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Timer timer_CheckStationAndSW;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txtLeftSN;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox txtLeftUC;
        private System.Windows.Forms.Button Bt_mesupload1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btSignalMES;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btSignal扫码;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btSignalCCD;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 清空显示ToolStripMenuItem;
        private System.Windows.Forms.Button bt_mesStatus;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Button btSignalHIVE;
        private System.Windows.Forms.Button button_HIVE;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TextBox tx_CycleTime_Left;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.RichTextBox richTextBox8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btSignalPDCA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConnectOtherMachine;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRobot;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnScan;
    }
}