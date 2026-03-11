namespace Cowain_Form.FormView
{
    partial class frm_TeachMotorView
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
            this.listView_Motor = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label_MotorID = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label_Pel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_ServoOn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label_ErrorCode = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label_Alm = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_MotionDone = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_Mel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_SaveData = new System.Windows.Forms.Button();
            this.btn_Alarm = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Home = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label_Pos = new System.Windows.Forms.Label();
            this.comboBox_Pitch = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_Speed = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_Repeat = new System.Windows.Forms.Button();
            this.btn_APos = new System.Windows.Forms.Button();
            this.btn_BPos = new System.Windows.Forms.Button();
            this.btn_MoveRight = new System.Windows.Forms.Button();
            this.textBox_Delay = new System.Windows.Forms.TextBox();
            this.btn_MoveLeft = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_BPos = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_APos = new System.Windows.Forms.TextBox();
            this.btn_TorSet = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label_Org = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Speed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView_Motor
            // 
            this.listView_Motor.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Motor.Location = new System.Drawing.Point(16, 78);
            this.listView_Motor.Margin = new System.Windows.Forms.Padding(2);
            this.listView_Motor.Name = "listView_Motor";
            this.listView_Motor.Size = new System.Drawing.Size(428, 395);
            this.listView_Motor.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView_Motor.TabIndex = 5;
            this.listView_Motor.TileSize = new System.Drawing.Size(308, 22);
            this.listView_Motor.UseCompatibleStateImageBehavior = false;
            this.listView_Motor.View = System.Windows.Forms.View.Tile;
            this.listView_Motor.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_Motor_MouseDoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 50);
            this.tabControl1.Location = new System.Drawing.Point(12, 11);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1220, 57);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 17;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 54);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(1212, 0);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "All";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label_MotorID
            // 
            this.label_MotorID.BackColor = System.Drawing.Color.SteelBlue;
            this.label_MotorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_MotorID.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_MotorID.Location = new System.Drawing.Point(481, 79);
            this.label_MotorID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_MotorID.Name = "label_MotorID";
            this.label_MotorID.Size = new System.Drawing.Size(418, 37);
            this.label_MotorID.TabIndex = 19;
            this.label_MotorID.Text = "Loading~~";
            this.label_MotorID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label_Pel
            // 
            this.label_Pel.BackColor = System.Drawing.Color.LightBlue;
            this.label_Pel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Pel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Pel.Location = new System.Drawing.Point(77, 20);
            this.label_Pel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Pel.Name = "label_Pel";
            this.label_Pel.Size = new System.Drawing.Size(20, 18);
            this.label_Pel.TabIndex = 20;
            this.label_Pel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(20, 20);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 20);
            this.label10.TabIndex = 220;
            this.label10.Text = "E+: ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label_Org);
            this.groupBox1.Controls.Add(this.btn_ServoOn);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label_ErrorCode);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label_Alm);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label_MotionDone);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label_Mel);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label_Pel);
            this.groupBox1.Location = new System.Drawing.Point(782, 306);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(445, 206);
            this.groupBox1.TabIndex = 221;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "电机动作讯号";
            // 
            // btn_ServoOn
            // 
            this.btn_ServoOn.BackColor = System.Drawing.SystemColors.Window;
            this.btn_ServoOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_ServoOn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_ServoOn.Location = new System.Drawing.Point(23, 150);
            this.btn_ServoOn.Margin = new System.Windows.Forms.Padding(2);
            this.btn_ServoOn.Name = "btn_ServoOn";
            this.btn_ServoOn.Size = new System.Drawing.Size(272, 39);
            this.btn_ServoOn.TabIndex = 233;
            this.btn_ServoOn.Text = "    Servo ON";
            this.btn_ServoOn.UseVisualStyleBackColor = false;
            this.btn_ServoOn.Click += new System.EventHandler(this.btn_ServoOn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(19, 121);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 20);
            this.label6.TabIndex = 226;
            this.label6.Text = "ErrorCode : ";
            // 
            // label_ErrorCode
            // 
            this.label_ErrorCode.BackColor = System.Drawing.Color.Red;
            this.label_ErrorCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ErrorCode.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_ErrorCode.Location = new System.Drawing.Point(127, 119);
            this.label_ErrorCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_ErrorCode.Name = "label_ErrorCode";
            this.label_ErrorCode.Size = new System.Drawing.Size(157, 26);
            this.label_ErrorCode.TabIndex = 227;
            this.label_ErrorCode.Text = "0";
            this.label_ErrorCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(135, 20);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 20);
            this.label12.TabIndex = 232;
            this.label12.Text = "ALM: ";
            // 
            // label_Alm
            // 
            this.label_Alm.BackColor = System.Drawing.Color.LightBlue;
            this.label_Alm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Alm.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Alm.Location = new System.Drawing.Point(208, 20);
            this.label_Alm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Alm.Name = "label_Alm";
            this.label_Alm.Size = new System.Drawing.Size(20, 18);
            this.label_Alm.TabIndex = 231;
            this.label_Alm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(135, 46);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 20);
            this.label7.TabIndex = 226;
            this.label7.Text = "Motion: ";
            // 
            // label_MotionDone
            // 
            this.label_MotionDone.BackColor = System.Drawing.Color.LightBlue;
            this.label_MotionDone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_MotionDone.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_MotionDone.Location = new System.Drawing.Point(208, 46);
            this.label_MotionDone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_MotionDone.Name = "label_MotionDone";
            this.label_MotionDone.Size = new System.Drawing.Size(20, 18);
            this.label_MotionDone.TabIndex = 225;
            this.label_MotionDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(20, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 222;
            this.label2.Text = "E- : ";
            // 
            // label_Mel
            // 
            this.label_Mel.BackColor = System.Drawing.Color.LightBlue;
            this.label_Mel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Mel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Mel.Location = new System.Drawing.Point(77, 46);
            this.label_Mel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Mel.Name = "label_Mel";
            this.label_Mel.Size = new System.Drawing.Size(20, 18);
            this.label_Mel.TabIndex = 221;
            this.label_Mel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_SaveData);
            this.groupBox2.Controls.Add(this.btn_Alarm);
            this.groupBox2.Controls.Add(this.btn_Stop);
            this.groupBox2.Controls.Add(this.btn_Home);
            this.groupBox2.Location = new System.Drawing.Point(481, 303);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(274, 209);
            this.groupBox2.TabIndex = 222;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "电机动作";
            // 
            // btn_SaveData
            // 
            this.btn_SaveData.BackColor = System.Drawing.SystemColors.Window;
            this.btn_SaveData.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_DataSave;
            this.btn_SaveData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SaveData.Location = new System.Drawing.Point(146, 75);
            this.btn_SaveData.Margin = new System.Windows.Forms.Padding(2);
            this.btn_SaveData.Name = "btn_SaveData";
            this.btn_SaveData.Size = new System.Drawing.Size(98, 46);
            this.btn_SaveData.TabIndex = 223;
            this.btn_SaveData.Text = "    参数储存";
            this.btn_SaveData.UseVisualStyleBackColor = false;
            this.btn_SaveData.Click += new System.EventHandler(this.btn_SaveData_Click);
            // 
            // btn_Alarm
            // 
            this.btn_Alarm.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Alarm.Image = global::Cowain_AutoMotion.Properties.Resources.AlarmReset;
            this.btn_Alarm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Alarm.Location = new System.Drawing.Point(12, 72);
            this.btn_Alarm.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Alarm.Name = "btn_Alarm";
            this.btn_Alarm.Size = new System.Drawing.Size(98, 46);
            this.btn_Alarm.TabIndex = 22;
            this.btn_Alarm.Text = "    异常复归";
            this.btn_Alarm.UseVisualStyleBackColor = false;
            this.btn_Alarm.Click += new System.EventHandler(this.btn_Alarm_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Stop.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_Stop;
            this.btn_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Stop.Location = new System.Drawing.Point(146, 21);
            this.btn_Stop.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(98, 46);
            this.btn_Stop.TabIndex = 20;
            this.btn_Stop.Text = "    移动停止";
            this.btn_Stop.UseVisualStyleBackColor = false;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Home
            // 
            this.btn_Home.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Home.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_Home;
            this.btn_Home.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Home.Location = new System.Drawing.Point(12, 20);
            this.btn_Home.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Home.Name = "btn_Home";
            this.btn_Home.Size = new System.Drawing.Size(98, 46);
            this.btn_Home.TabIndex = 18;
            this.btn_Home.Text = "     原点复归";
            this.btn_Home.UseVisualStyleBackColor = false;
            this.btn_Home.Click += new System.EventHandler(this.btn_Home_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(488, 130);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 224;
            this.label1.Text = "Position: ";
            // 
            // label_Pos
            // 
            this.label_Pos.BackColor = System.Drawing.SystemColors.Control;
            this.label_Pos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Pos.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Pos.Location = new System.Drawing.Point(581, 123);
            this.label_Pos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Pos.Name = "label_Pos";
            this.label_Pos.Size = new System.Drawing.Size(202, 26);
            this.label_Pos.TabIndex = 223;
            this.label_Pos.Text = "0.0";
            this.label_Pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox_Pitch
            // 
            this.comboBox_Pitch.BackColor = System.Drawing.Color.Thistle;
            this.comboBox_Pitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_Pitch.FormattingEnabled = true;
            this.comboBox_Pitch.Items.AddRange(new object[] {
            "0.0001",
            "0.001",
            "0.01",
            "0.1",
            "1",
            "10",
            "50",
            "100"});
            this.comboBox_Pitch.Location = new System.Drawing.Point(104, 20);
            this.comboBox_Pitch.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_Pitch.Name = "comboBox_Pitch";
            this.comboBox_Pitch.Size = new System.Drawing.Size(152, 28);
            this.comboBox_Pitch.TabIndex = 226;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(8, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 24);
            this.label3.TabIndex = 225;
            this.label3.Text = "吋动距离:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDown_Speed);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.btn_Repeat);
            this.groupBox3.Controls.Add(this.btn_APos);
            this.groupBox3.Controls.Add(this.btn_BPos);
            this.groupBox3.Controls.Add(this.btn_MoveRight);
            this.groupBox3.Controls.Add(this.textBox_Delay);
            this.groupBox3.Controls.Add(this.btn_MoveLeft);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox_BPos);
            this.groupBox3.Controls.Add(this.comboBox_Pitch);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox_APos);
            this.groupBox3.Location = new System.Drawing.Point(481, 165);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(746, 133);
            this.groupBox3.TabIndex = 227;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "电机动作";
            // 
            // numericUpDown_Speed
            // 
            this.numericUpDown_Speed.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.numericUpDown_Speed.Location = new System.Drawing.Point(398, 22);
            this.numericUpDown_Speed.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_Speed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Speed.Name = "numericUpDown_Speed";
            this.numericUpDown_Speed.Size = new System.Drawing.Size(46, 25);
            this.numericUpDown_Speed.TabIndex = 228;
            this.numericUpDown_Speed.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(284, 22);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 20);
            this.label9.TabIndex = 235;
            this.label9.Text = "移动速度 (%):";
            // 
            // btn_Repeat
            // 
            this.btn_Repeat.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Repeat.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Repeat.Image = global::Cowain_AutoMotion.Properties.Resources.Repeat;
            this.btn_Repeat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Repeat.Location = new System.Drawing.Point(529, 72);
            this.btn_Repeat.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Repeat.Name = "btn_Repeat";
            this.btn_Repeat.Size = new System.Drawing.Size(179, 53);
            this.btn_Repeat.TabIndex = 229;
            this.btn_Repeat.Text = "          A.B往返";
            this.btn_Repeat.UseVisualStyleBackColor = false;
            this.btn_Repeat.Click += new System.EventHandler(this.btn_Repeat_Click);
            // 
            // btn_APos
            // 
            this.btn_APos.BackColor = System.Drawing.SystemColors.Window;
            this.btn_APos.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_APos.Image = global::Cowain_AutoMotion.Properties.Resources.leftArm;
            this.btn_APos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_APos.Location = new System.Drawing.Point(529, 16);
            this.btn_APos.Margin = new System.Windows.Forms.Padding(2);
            this.btn_APos.Name = "btn_APos";
            this.btn_APos.Size = new System.Drawing.Size(85, 53);
            this.btn_APos.TabIndex = 227;
            this.btn_APos.Text = "          A位置";
            this.btn_APos.UseVisualStyleBackColor = false;
            this.btn_APos.Click += new System.EventHandler(this.btn_APos_Click);
            // 
            // btn_BPos
            // 
            this.btn_BPos.BackColor = System.Drawing.SystemColors.Window;
            this.btn_BPos.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_BPos.Image = global::Cowain_AutoMotion.Properties.Resources.RightArm;
            this.btn_BPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_BPos.Location = new System.Drawing.Point(623, 13);
            this.btn_BPos.Margin = new System.Windows.Forms.Padding(2);
            this.btn_BPos.Name = "btn_BPos";
            this.btn_BPos.Size = new System.Drawing.Size(85, 53);
            this.btn_BPos.TabIndex = 228;
            this.btn_BPos.Text = "           B位置";
            this.btn_BPos.UseVisualStyleBackColor = false;
            this.btn_BPos.Click += new System.EventHandler(this.btn_BPos_Click);
            // 
            // btn_MoveRight
            // 
            this.btn_MoveRight.BackColor = System.Drawing.SystemColors.Window;
            this.btn_MoveRight.Image = global::Cowain_AutoMotion.Properties.Resources.small__right;
            this.btn_MoveRight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_MoveRight.Location = new System.Drawing.Point(146, 53);
            this.btn_MoveRight.Margin = new System.Windows.Forms.Padding(2);
            this.btn_MoveRight.Name = "btn_MoveRight";
            this.btn_MoveRight.Size = new System.Drawing.Size(109, 61);
            this.btn_MoveRight.TabIndex = 228;
            this.btn_MoveRight.Text = "           正方向移动";
            this.btn_MoveRight.UseVisualStyleBackColor = false;
            this.btn_MoveRight.Click += new System.EventHandler(this.btn_MoveRight_Click);
            // 
            // textBox_Delay
            // 
            this.textBox_Delay.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Delay.Location = new System.Drawing.Point(397, 101);
            this.textBox_Delay.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Delay.Name = "textBox_Delay";
            this.textBox_Delay.Size = new System.Drawing.Size(113, 22);
            this.textBox_Delay.TabIndex = 234;
            // 
            // btn_MoveLeft
            // 
            this.btn_MoveLeft.BackColor = System.Drawing.SystemColors.Window;
            this.btn_MoveLeft.Image = global::Cowain_AutoMotion.Properties.Resources.small_left;
            this.btn_MoveLeft.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_MoveLeft.Location = new System.Drawing.Point(12, 53);
            this.btn_MoveLeft.Margin = new System.Windows.Forms.Padding(2);
            this.btn_MoveLeft.Name = "btn_MoveLeft";
            this.btn_MoveLeft.Size = new System.Drawing.Size(109, 61);
            this.btn_MoveLeft.TabIndex = 227;
            this.btn_MoveLeft.Text = "          负方向移动";
            this.btn_MoveLeft.UseVisualStyleBackColor = false;
            this.btn_MoveLeft.Click += new System.EventHandler(this.btn_MoveLeft_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(284, 104);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 20);
            this.label8.TabIndex = 233;
            this.label8.Text = "延迟时间 (ms):";
            // 
            // textBox_BPos
            // 
            this.textBox_BPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_BPos.Location = new System.Drawing.Point(397, 77);
            this.textBox_BPos.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_BPos.Name = "textBox_BPos";
            this.textBox_BPos.Size = new System.Drawing.Size(113, 22);
            this.textBox_BPos.TabIndex = 232;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(283, 79);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 20);
            this.label5.TabIndex = 231;
            this.label5.Text = "B移动值(mm):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(283, 53);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 20);
            this.label4.TabIndex = 225;
            this.label4.Text = "A移动值(mm):";
            // 
            // textBox_APos
            // 
            this.textBox_APos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_APos.Location = new System.Drawing.Point(397, 53);
            this.textBox_APos.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_APos.Name = "textBox_APos";
            this.textBox_APos.Size = new System.Drawing.Size(113, 22);
            this.textBox_APos.TabIndex = 230;
            // 
            // btn_TorSet
            // 
            this.btn_TorSet.BackColor = System.Drawing.SystemColors.Window;
            this.btn_TorSet.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_TorSet.Image = global::Cowain_AutoMotion.Properties.Resources.SetOk;
            this.btn_TorSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_TorSet.Location = new System.Drawing.Point(1118, 142);
            this.btn_TorSet.Margin = new System.Windows.Forms.Padding(2);
            this.btn_TorSet.Name = "btn_TorSet";
            this.btn_TorSet.Size = new System.Drawing.Size(109, 21);
            this.btn_TorSet.TabIndex = 255;
            this.btn_TorSet.Text = "        Tor Set";
            this.btn_TorSet.UseVisualStyleBackColor = false;
            this.btn_TorSet.Visible = false;
            this.btn_TorSet.Click += new System.EventHandler(this.btn_TorSet_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(1075, 123);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 20);
            this.label11.TabIndex = 254;
            this.label11.Text = "TorTest  (%):";
            this.label11.Visible = false;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.numericUpDown1.Location = new System.Drawing.Point(1182, 120);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(46, 25);
            this.numericUpDown1.TabIndex = 253;
            this.numericUpDown1.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown1.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(21, 72);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 20);
            this.label13.TabIndex = 235;
            this.label13.Text = "Org : ";
            // 
            // label_Org
            // 
            this.label_Org.BackColor = System.Drawing.Color.LightBlue;
            this.label_Org.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Org.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Org.Location = new System.Drawing.Point(78, 72);
            this.label_Org.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Org.Name = "label_Org";
            this.label_Org.Size = new System.Drawing.Size(20, 18);
            this.label_Org.TabIndex = 234;
            this.label_Org.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frm_TeachMotorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 541);
            this.Controls.Add(this.btn_TorSet);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label_Pos);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label_MotorID);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listView_Motor);
            this.Font = new System.Drawing.Font("新細明體-ExtB", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frm_TeachMotorView";
            this.Text = "frm_MotorView";
            this.Shown += new System.EventHandler(this.frm_MotorView_Shown);
            this.VisibleChanged += new System.EventHandler(this.frm_MotorView_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Speed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_Motor;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label_MotorID;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label_Pel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_ErrorCode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label_Alm;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_MotionDone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_Mel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_SaveData;
        private System.Windows.Forms.Button btn_Alarm;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Home;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_Pos;
        private System.Windows.Forms.ComboBox comboBox_Pitch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_MoveRight;
        private System.Windows.Forms.Button btn_MoveLeft;
        private System.Windows.Forms.TextBox textBox_Delay;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_BPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_APos;
        private System.Windows.Forms.Button btn_Repeat;
        private System.Windows.Forms.Button btn_BPos;
        private System.Windows.Forms.Button btn_APos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_ServoOn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDown_Speed;
        private System.Windows.Forms.Button btn_TorSet;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label_Org;
    }
}