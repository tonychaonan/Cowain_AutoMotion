namespace Cowain_Form.FormView
{
    partial class frm_VisionCCD1
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_FunctionRun = new System.Windows.Forms.Button();
            this.groupBox_Setting = new System.Windows.Forms.GroupBox();
            this.button_ShowROI = new System.Windows.Forms.Button();
            this.button_SaveROI = new System.Windows.Forms.Button();
            this.btn_LightSet = new System.Windows.Forms.Button();
            this.label_Light = new System.Windows.Forms.Label();
            this.trackBar_Light = new System.Windows.Forms.TrackBar();
            this.btn_live = new System.Windows.Forms.Button();
            this.comboBox_function = new System.Windows.Forms.ComboBox();
            this.btn_Snap = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Home = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Move = new System.Windows.Forms.Button();
            this.listView_Pos = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox_M6 = new System.Windows.Forms.GroupBox();
            this.label_M6Pos = new System.Windows.Forms.Label();
            this.label_StageT = new System.Windows.Forms.Label();
            this.btn_AddM6 = new System.Windows.Forms.Button();
            this.btn_SubM6 = new System.Windows.Forms.Button();
            this.groupBox_M5 = new System.Windows.Forms.GroupBox();
            this.label_M5Pos = new System.Windows.Forms.Label();
            this.label_StageZ = new System.Windows.Forms.Label();
            this.btn_AddM5 = new System.Windows.Forms.Button();
            this.btn_SubM5 = new System.Windows.Forms.Button();
            this.groupBox_M4 = new System.Windows.Forms.GroupBox();
            this.label_M4Pos = new System.Windows.Forms.Label();
            this.label_StageX = new System.Windows.Forms.Label();
            this.btn_AddM4 = new System.Windows.Forms.Button();
            this.btn_SubM4 = new System.Windows.Forms.Button();
            this.groupBox_M3 = new System.Windows.Forms.GroupBox();
            this.label_M3Pos = new System.Windows.Forms.Label();
            this.label_TrayX = new System.Windows.Forms.Label();
            this.btn_AddM3 = new System.Windows.Forms.Button();
            this.btn_SubM3 = new System.Windows.Forms.Button();
            this.groupBox_M2 = new System.Windows.Forms.GroupBox();
            this.label_M2Pos = new System.Windows.Forms.Label();
            this.label_PickZ = new System.Windows.Forms.Label();
            this.btn_AddM2 = new System.Windows.Forms.Button();
            this.btn_SubM2 = new System.Windows.Forms.Button();
            this.groupBox_M1 = new System.Windows.Forms.GroupBox();
            this.label_M1Pos = new System.Windows.Forms.Label();
            this.label_PickY = new System.Windows.Forms.Label();
            this.btn_AddM1 = new System.Windows.Forms.Button();
            this.btn_SubM1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_Pitch = new System.Windows.Forms.ComboBox();
            this.timer_CCDLive = new System.Windows.Forms.Timer(this.components);
            this.timer_ReFlash = new System.Windows.Forms.Timer(this.components);
            this.groupBox4.SuspendLayout();
            this.groupBox_Setting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Light)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox_M6.SuspendLayout();
            this.groupBox_M5.SuspendLayout();
            this.groupBox_M4.SuspendLayout();
            this.groupBox_M3.SuspendLayout();
            this.groupBox_M2.SuspendLayout();
            this.groupBox_M1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_FunctionRun);
            this.groupBox4.Controls.Add(this.groupBox_Setting);
            this.groupBox4.Controls.Add(this.btn_LightSet);
            this.groupBox4.Controls.Add(this.label_Light);
            this.groupBox4.Controls.Add(this.trackBar_Light);
            this.groupBox4.Controls.Add(this.btn_live);
            this.groupBox4.Controls.Add(this.comboBox_function);
            this.groupBox4.Controls.Add(this.btn_Snap);
            this.groupBox4.Controls.Add(this.comboBox2);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.Location = new System.Drawing.Point(441, 11);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(364, 494);
            this.groupBox4.TabIndex = 302;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CCD 對應function選擇";
            // 
            // btn_FunctionRun
            // 
            this.btn_FunctionRun.BackColor = System.Drawing.SystemColors.Window;
            this.btn_FunctionRun.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_FunctionRun.Image = global::Cowain_AutoMotion.Properties.Resources.leftArm;
            this.btn_FunctionRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_FunctionRun.Location = new System.Drawing.Point(282, 444);
            this.btn_FunctionRun.Name = "btn_FunctionRun";
            this.btn_FunctionRun.Size = new System.Drawing.Size(78, 41);
            this.btn_FunctionRun.TabIndex = 305;
            this.btn_FunctionRun.Text = "         Run";
            this.btn_FunctionRun.UseVisualStyleBackColor = false;
            this.btn_FunctionRun.Click += new System.EventHandler(this.btn_FunctionRun_Click);
            // 
            // groupBox_Setting
            // 
            this.groupBox_Setting.Controls.Add(this.button_ShowROI);
            this.groupBox_Setting.Controls.Add(this.button_SaveROI);
            this.groupBox_Setting.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_Setting.Location = new System.Drawing.Point(10, 148);
            this.groupBox_Setting.Name = "groupBox_Setting";
            this.groupBox_Setting.Size = new System.Drawing.Size(343, 290);
            this.groupBox_Setting.TabIndex = 310;
            this.groupBox_Setting.TabStop = false;
            this.groupBox_Setting.Text = "Vision設置";
            // 
            // button_ShowROI
            // 
            this.button_ShowROI.BackColor = System.Drawing.SystemColors.Window;
            this.button_ShowROI.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_ShowROI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_ShowROI.Location = new System.Drawing.Point(8, 37);
            this.button_ShowROI.Name = "button_ShowROI";
            this.button_ShowROI.Size = new System.Drawing.Size(83, 39);
            this.button_ShowROI.TabIndex = 308;
            this.button_ShowROI.Text = " ROI";
            this.button_ShowROI.UseVisualStyleBackColor = false;
            this.button_ShowROI.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_SaveROI
            // 
            this.button_SaveROI.BackColor = System.Drawing.SystemColors.Window;
            this.button_SaveROI.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_SaveROI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_SaveROI.Location = new System.Drawing.Point(111, 47);
            this.button_SaveROI.Name = "button_SaveROI";
            this.button_SaveROI.Size = new System.Drawing.Size(55, 29);
            this.button_SaveROI.TabIndex = 309;
            this.button_SaveROI.Text = "儲存";
            this.button_SaveROI.UseVisualStyleBackColor = false;
            this.button_SaveROI.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_LightSet
            // 
            this.btn_LightSet.BackColor = System.Drawing.SystemColors.Window;
            this.btn_LightSet.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_LightSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_LightSet.Location = new System.Drawing.Point(305, 102);
            this.btn_LightSet.Name = "btn_LightSet";
            this.btn_LightSet.Size = new System.Drawing.Size(55, 30);
            this.btn_LightSet.TabIndex = 307;
            this.btn_LightSet.Text = "儲存";
            this.btn_LightSet.UseVisualStyleBackColor = false;
            this.btn_LightSet.Click += new System.EventHandler(this.btn_LightSet_Click);
            // 
            // label_Light
            // 
            this.label_Light.AutoSize = true;
            this.label_Light.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Light.Location = new System.Drawing.Point(14, 101);
            this.label_Light.Name = "label_Light";
            this.label_Light.Size = new System.Drawing.Size(94, 19);
            this.label_Light.TabIndex = 306;
            this.label_Light.Text = "光源(100%):";
            // 
            // trackBar_Light
            // 
            this.trackBar_Light.Location = new System.Drawing.Point(107, 97);
            this.trackBar_Light.Maximum = 300;
            this.trackBar_Light.Name = "trackBar_Light";
            this.trackBar_Light.Size = new System.Drawing.Size(200, 45);
            this.trackBar_Light.TabIndex = 305;
            this.trackBar_Light.ValueChanged += new System.EventHandler(this.trackBar_Light_ValueChanged);
            // 
            // btn_live
            // 
            this.btn_live.BackColor = System.Drawing.SystemColors.Window;
            this.btn_live.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_live.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_live.Location = new System.Drawing.Point(131, 54);
            this.btn_live.Name = "btn_live";
            this.btn_live.Size = new System.Drawing.Size(97, 39);
            this.btn_live.TabIndex = 306;
            this.btn_live.Text = "連續取像";
            this.btn_live.UseVisualStyleBackColor = false;
            this.btn_live.Click += new System.EventHandler(this.btn_live_Click);
            // 
            // comboBox_function
            // 
            this.comboBox_function.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_function.FormattingEnabled = true;
            this.comboBox_function.Items.AddRange(new object[] {
            "TP1-1",
            "TP1-2",
            "TP2-1",
            "TP2-2",
            "ZStop_6ZStop",
            "ZStop_1ZStop"});
            this.comboBox_function.Location = new System.Drawing.Point(15, 21);
            this.comboBox_function.Name = "comboBox_function";
            this.comboBox_function.Size = new System.Drawing.Size(161, 24);
            this.comboBox_function.TabIndex = 295;
            this.comboBox_function.SelectedIndexChanged += new System.EventHandler(this.comboBox_function_SelectedIndexChanged);
            // 
            // btn_Snap
            // 
            this.btn_Snap.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Snap.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Snap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Snap.Location = new System.Drawing.Point(13, 54);
            this.btn_Snap.Name = "btn_Snap";
            this.btn_Snap.Size = new System.Drawing.Size(97, 39);
            this.btn_Snap.TabIndex = 305;
            this.btn_Snap.Text = "       取像";
            this.btn_Snap.UseVisualStyleBackColor = false;
            this.btn_Snap.Click += new System.EventHandler(this.btn_Snap_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "X Mark1",
            "X Mark2",
            "Y Mark1",
            "Y Mark2"});
            this.comboBox2.Location = new System.Drawing.Point(207, 22);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(137, 24);
            this.comboBox2.TabIndex = 296;
            this.comboBox2.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Home);
            this.groupBox1.Controls.Add(this.btn_Stop);
            this.groupBox1.Controls.Add(this.btn_Save);
            this.groupBox1.Controls.Add(this.btn_Move);
            this.groupBox1.Controls.Add(this.listView_Pos);
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(811, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(448, 287);
            this.groupBox1.TabIndex = 303;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Machine Pos";
            // 
            // btn_Home
            // 
            this.btn_Home.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Home.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Home.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_Home;
            this.btn_Home.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Home.Location = new System.Drawing.Point(161, 229);
            this.btn_Home.Name = "btn_Home";
            this.btn_Home.Size = new System.Drawing.Size(87, 49);
            this.btn_Home.TabIndex = 235;
            this.btn_Home.Text = "         Home";
            this.btn_Home.UseVisualStyleBackColor = false;
            this.btn_Home.Visible = false;
            this.btn_Home.Click += new System.EventHandler(this.btn_Home_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Stop.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Stop.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_Stop;
            this.btn_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Stop.Location = new System.Drawing.Point(347, 229);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(87, 49);
            this.btn_Stop.TabIndex = 234;
            this.btn_Stop.Text = "         Stop";
            this.btn_Stop.UseVisualStyleBackColor = false;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Save.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Save.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_DataSave;
            this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Save.Location = new System.Drawing.Point(14, 228);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(109, 49);
            this.btn_Save.TabIndex = 233;
            this.btn_Save.Text = "         位置儲存";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Move
            // 
            this.btn_Move.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Move.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Move.Image = global::Cowain_AutoMotion.Properties.Resources.leftArm;
            this.btn_Move.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Move.Location = new System.Drawing.Point(254, 229);
            this.btn_Move.Name = "btn_Move";
            this.btn_Move.Size = new System.Drawing.Size(87, 49);
            this.btn_Move.TabIndex = 232;
            this.btn_Move.Text = "         Move";
            this.btn_Move.UseVisualStyleBackColor = false;
            this.btn_Move.Click += new System.EventHandler(this.btn_Move_Click);
            // 
            // listView_Pos
            // 
            this.listView_Pos.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Pos.Location = new System.Drawing.Point(14, 24);
            this.listView_Pos.Name = "listView_Pos";
            this.listView_Pos.Size = new System.Drawing.Size(417, 200);
            this.listView_Pos.TabIndex = 231;
            this.listView_Pos.UseCompatibleStateImageBehavior = false;
            this.listView_Pos.View = System.Windows.Forms.View.List;
            this.listView_Pos.DoubleClick += new System.EventHandler(this.listView_Pos_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox_M6);
            this.groupBox2.Controls.Add(this.groupBox_M5);
            this.groupBox2.Controls.Add(this.groupBox_M4);
            this.groupBox2.Controls.Add(this.groupBox_M3);
            this.groupBox2.Controls.Add(this.groupBox_M2);
            this.groupBox2.Controls.Add(this.groupBox_M1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBox_Pitch);
            this.groupBox2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox2.Location = new System.Drawing.Point(811, 305);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(447, 214);
            this.groupBox2.TabIndex = 304;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Motor";
            // 
            // groupBox_M6
            // 
            this.groupBox_M6.Controls.Add(this.label_M6Pos);
            this.groupBox_M6.Controls.Add(this.label_StageT);
            this.groupBox_M6.Controls.Add(this.btn_AddM6);
            this.groupBox_M6.Controls.Add(this.btn_SubM6);
            this.groupBox_M6.Location = new System.Drawing.Point(313, 133);
            this.groupBox_M6.Name = "groupBox_M6";
            this.groupBox_M6.Size = new System.Drawing.Size(131, 75);
            this.groupBox_M6.TabIndex = 244;
            this.groupBox_M6.TabStop = false;
            // 
            // label_M6Pos
            // 
            this.label_M6Pos.BackColor = System.Drawing.SystemColors.Control;
            this.label_M6Pos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_M6Pos.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_M6Pos.Location = new System.Drawing.Point(65, 17);
            this.label_M6Pos.Name = "label_M6Pos";
            this.label_M6Pos.Size = new System.Drawing.Size(60, 20);
            this.label_M6Pos.TabIndex = 238;
            this.label_M6Pos.Text = "0.0";
            this.label_M6Pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_StageT
            // 
            this.label_StageT.AutoSize = true;
            this.label_StageT.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_StageT.Location = new System.Drawing.Point(7, 17);
            this.label_StageT.Name = "label_StageT";
            this.label_StageT.Size = new System.Drawing.Size(62, 16);
            this.label_StageT.TabIndex = 239;
            this.label_StageT.Text = "ZSTOP Y: ";
            // 
            // btn_AddM6
            // 
            this.btn_AddM6.BackColor = System.Drawing.SystemColors.Window;
            this.btn_AddM6.Image = global::Cowain_AutoMotion.Properties.Resources.Add;
            this.btn_AddM6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_AddM6.Location = new System.Drawing.Point(10, 39);
            this.btn_AddM6.Name = "btn_AddM6";
            this.btn_AddM6.Size = new System.Drawing.Size(42, 30);
            this.btn_AddM6.TabIndex = 229;
            this.btn_AddM6.UseVisualStyleBackColor = false;
            this.btn_AddM6.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // btn_SubM6
            // 
            this.btn_SubM6.BackColor = System.Drawing.SystemColors.Window;
            this.btn_SubM6.Image = global::Cowain_AutoMotion.Properties.Resources.sub;
            this.btn_SubM6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SubM6.Location = new System.Drawing.Point(70, 41);
            this.btn_SubM6.Name = "btn_SubM6";
            this.btn_SubM6.Size = new System.Drawing.Size(42, 30);
            this.btn_SubM6.TabIndex = 230;
            this.btn_SubM6.UseVisualStyleBackColor = false;
            this.btn_SubM6.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // groupBox_M5
            // 
            this.groupBox_M5.Controls.Add(this.label_M5Pos);
            this.groupBox_M5.Controls.Add(this.label_StageZ);
            this.groupBox_M5.Controls.Add(this.btn_AddM5);
            this.groupBox_M5.Controls.Add(this.btn_SubM5);
            this.groupBox_M5.Location = new System.Drawing.Point(165, 131);
            this.groupBox_M5.Name = "groupBox_M5";
            this.groupBox_M5.Size = new System.Drawing.Size(131, 75);
            this.groupBox_M5.TabIndex = 243;
            this.groupBox_M5.TabStop = false;
            // 
            // label_M5Pos
            // 
            this.label_M5Pos.BackColor = System.Drawing.SystemColors.Control;
            this.label_M5Pos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_M5Pos.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_M5Pos.Location = new System.Drawing.Point(65, 17);
            this.label_M5Pos.Name = "label_M5Pos";
            this.label_M5Pos.Size = new System.Drawing.Size(60, 20);
            this.label_M5Pos.TabIndex = 238;
            this.label_M5Pos.Text = "0.0";
            this.label_M5Pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_StageZ
            // 
            this.label_StageZ.AutoSize = true;
            this.label_StageZ.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_StageZ.Location = new System.Drawing.Point(7, 17);
            this.label_StageZ.Name = "label_StageZ";
            this.label_StageZ.Size = new System.Drawing.Size(51, 17);
            this.label_StageZ.TabIndex = 239;
            this.label_StageZ.Text = "TP2 Y : ";
            // 
            // btn_AddM5
            // 
            this.btn_AddM5.BackColor = System.Drawing.SystemColors.Window;
            this.btn_AddM5.Image = global::Cowain_AutoMotion.Properties.Resources.Add;
            this.btn_AddM5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_AddM5.Location = new System.Drawing.Point(10, 39);
            this.btn_AddM5.Name = "btn_AddM5";
            this.btn_AddM5.Size = new System.Drawing.Size(42, 30);
            this.btn_AddM5.TabIndex = 229;
            this.btn_AddM5.UseVisualStyleBackColor = false;
            this.btn_AddM5.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // btn_SubM5
            // 
            this.btn_SubM5.BackColor = System.Drawing.SystemColors.Window;
            this.btn_SubM5.Image = global::Cowain_AutoMotion.Properties.Resources.sub;
            this.btn_SubM5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SubM5.Location = new System.Drawing.Point(70, 41);
            this.btn_SubM5.Name = "btn_SubM5";
            this.btn_SubM5.Size = new System.Drawing.Size(42, 30);
            this.btn_SubM5.TabIndex = 230;
            this.btn_SubM5.UseVisualStyleBackColor = false;
            this.btn_SubM5.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // groupBox_M4
            // 
            this.groupBox_M4.Controls.Add(this.label_M4Pos);
            this.groupBox_M4.Controls.Add(this.label_StageX);
            this.groupBox_M4.Controls.Add(this.btn_AddM4);
            this.groupBox_M4.Controls.Add(this.btn_SubM4);
            this.groupBox_M4.Location = new System.Drawing.Point(18, 131);
            this.groupBox_M4.Name = "groupBox_M4";
            this.groupBox_M4.Size = new System.Drawing.Size(131, 75);
            this.groupBox_M4.TabIndex = 242;
            this.groupBox_M4.TabStop = false;
            // 
            // label_M4Pos
            // 
            this.label_M4Pos.BackColor = System.Drawing.SystemColors.Control;
            this.label_M4Pos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_M4Pos.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_M4Pos.Location = new System.Drawing.Point(65, 17);
            this.label_M4Pos.Name = "label_M4Pos";
            this.label_M4Pos.Size = new System.Drawing.Size(60, 20);
            this.label_M4Pos.TabIndex = 238;
            this.label_M4Pos.Text = "0.0";
            this.label_M4Pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_StageX
            // 
            this.label_StageX.AutoSize = true;
            this.label_StageX.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_StageX.Location = new System.Drawing.Point(7, 17);
            this.label_StageX.Name = "label_StageX";
            this.label_StageX.Size = new System.Drawing.Size(51, 17);
            this.label_StageX.TabIndex = 239;
            this.label_StageX.Text = "TP1 Y : ";
            // 
            // btn_AddM4
            // 
            this.btn_AddM4.BackColor = System.Drawing.SystemColors.Window;
            this.btn_AddM4.Image = global::Cowain_AutoMotion.Properties.Resources.Add;
            this.btn_AddM4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_AddM4.Location = new System.Drawing.Point(10, 39);
            this.btn_AddM4.Name = "btn_AddM4";
            this.btn_AddM4.Size = new System.Drawing.Size(42, 30);
            this.btn_AddM4.TabIndex = 229;
            this.btn_AddM4.UseVisualStyleBackColor = false;
            this.btn_AddM4.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // btn_SubM4
            // 
            this.btn_SubM4.BackColor = System.Drawing.SystemColors.Window;
            this.btn_SubM4.Image = global::Cowain_AutoMotion.Properties.Resources.sub;
            this.btn_SubM4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SubM4.Location = new System.Drawing.Point(70, 41);
            this.btn_SubM4.Name = "btn_SubM4";
            this.btn_SubM4.Size = new System.Drawing.Size(42, 30);
            this.btn_SubM4.TabIndex = 230;
            this.btn_SubM4.UseVisualStyleBackColor = false;
            this.btn_SubM4.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // groupBox_M3
            // 
            this.groupBox_M3.Controls.Add(this.label_M3Pos);
            this.groupBox_M3.Controls.Add(this.label_TrayX);
            this.groupBox_M3.Controls.Add(this.btn_AddM3);
            this.groupBox_M3.Controls.Add(this.btn_SubM3);
            this.groupBox_M3.Location = new System.Drawing.Point(313, 50);
            this.groupBox_M3.Name = "groupBox_M3";
            this.groupBox_M3.Size = new System.Drawing.Size(131, 75);
            this.groupBox_M3.TabIndex = 241;
            this.groupBox_M3.TabStop = false;
            // 
            // label_M3Pos
            // 
            this.label_M3Pos.BackColor = System.Drawing.SystemColors.Control;
            this.label_M3Pos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_M3Pos.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_M3Pos.Location = new System.Drawing.Point(65, 17);
            this.label_M3Pos.Name = "label_M3Pos";
            this.label_M3Pos.Size = new System.Drawing.Size(60, 20);
            this.label_M3Pos.TabIndex = 238;
            this.label_M3Pos.Text = "0.0";
            this.label_M3Pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_TrayX
            // 
            this.label_TrayX.AutoSize = true;
            this.label_TrayX.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_TrayX.Location = new System.Drawing.Point(12, 17);
            this.label_TrayX.Name = "label_TrayX";
            this.label_TrayX.Size = new System.Drawing.Size(56, 17);
            this.label_TrayX.TabIndex = 239;
            this.label_TrayX.Text = "Head T: ";
            // 
            // btn_AddM3
            // 
            this.btn_AddM3.BackColor = System.Drawing.SystemColors.Window;
            this.btn_AddM3.Image = global::Cowain_AutoMotion.Properties.Resources.Add;
            this.btn_AddM3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_AddM3.Location = new System.Drawing.Point(10, 39);
            this.btn_AddM3.Name = "btn_AddM3";
            this.btn_AddM3.Size = new System.Drawing.Size(42, 30);
            this.btn_AddM3.TabIndex = 229;
            this.btn_AddM3.UseVisualStyleBackColor = false;
            this.btn_AddM3.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // btn_SubM3
            // 
            this.btn_SubM3.BackColor = System.Drawing.SystemColors.Window;
            this.btn_SubM3.Image = global::Cowain_AutoMotion.Properties.Resources.sub;
            this.btn_SubM3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SubM3.Location = new System.Drawing.Point(70, 41);
            this.btn_SubM3.Name = "btn_SubM3";
            this.btn_SubM3.Size = new System.Drawing.Size(42, 30);
            this.btn_SubM3.TabIndex = 230;
            this.btn_SubM3.UseVisualStyleBackColor = false;
            this.btn_SubM3.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // groupBox_M2
            // 
            this.groupBox_M2.Controls.Add(this.label_M2Pos);
            this.groupBox_M2.Controls.Add(this.label_PickZ);
            this.groupBox_M2.Controls.Add(this.btn_AddM2);
            this.groupBox_M2.Controls.Add(this.btn_SubM2);
            this.groupBox_M2.Location = new System.Drawing.Point(165, 50);
            this.groupBox_M2.Name = "groupBox_M2";
            this.groupBox_M2.Size = new System.Drawing.Size(131, 75);
            this.groupBox_M2.TabIndex = 240;
            this.groupBox_M2.TabStop = false;
            // 
            // label_M2Pos
            // 
            this.label_M2Pos.BackColor = System.Drawing.SystemColors.Control;
            this.label_M2Pos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_M2Pos.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_M2Pos.Location = new System.Drawing.Point(65, 17);
            this.label_M2Pos.Name = "label_M2Pos";
            this.label_M2Pos.Size = new System.Drawing.Size(60, 20);
            this.label_M2Pos.TabIndex = 238;
            this.label_M2Pos.Text = "0.0";
            this.label_M2Pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_PickZ
            // 
            this.label_PickZ.AutoSize = true;
            this.label_PickZ.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_PickZ.Location = new System.Drawing.Point(12, 17);
            this.label_PickZ.Name = "label_PickZ";
            this.label_PickZ.Size = new System.Drawing.Size(57, 17);
            this.label_PickZ.TabIndex = 239;
            this.label_PickZ.Text = "Head Z: ";
            // 
            // btn_AddM2
            // 
            this.btn_AddM2.BackColor = System.Drawing.SystemColors.Window;
            this.btn_AddM2.Image = global::Cowain_AutoMotion.Properties.Resources.Add;
            this.btn_AddM2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_AddM2.Location = new System.Drawing.Point(10, 39);
            this.btn_AddM2.Name = "btn_AddM2";
            this.btn_AddM2.Size = new System.Drawing.Size(42, 30);
            this.btn_AddM2.TabIndex = 229;
            this.btn_AddM2.UseVisualStyleBackColor = false;
            this.btn_AddM2.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // btn_SubM2
            // 
            this.btn_SubM2.BackColor = System.Drawing.SystemColors.Window;
            this.btn_SubM2.Image = global::Cowain_AutoMotion.Properties.Resources.sub;
            this.btn_SubM2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SubM2.Location = new System.Drawing.Point(70, 41);
            this.btn_SubM2.Name = "btn_SubM2";
            this.btn_SubM2.Size = new System.Drawing.Size(42, 30);
            this.btn_SubM2.TabIndex = 230;
            this.btn_SubM2.UseVisualStyleBackColor = false;
            this.btn_SubM2.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // groupBox_M1
            // 
            this.groupBox_M1.Controls.Add(this.label_M1Pos);
            this.groupBox_M1.Controls.Add(this.label_PickY);
            this.groupBox_M1.Controls.Add(this.btn_AddM1);
            this.groupBox_M1.Controls.Add(this.btn_SubM1);
            this.groupBox_M1.Location = new System.Drawing.Point(18, 50);
            this.groupBox_M1.Name = "groupBox_M1";
            this.groupBox_M1.Size = new System.Drawing.Size(131, 75);
            this.groupBox_M1.TabIndex = 237;
            this.groupBox_M1.TabStop = false;
            // 
            // label_M1Pos
            // 
            this.label_M1Pos.BackColor = System.Drawing.SystemColors.Control;
            this.label_M1Pos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_M1Pos.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_M1Pos.Location = new System.Drawing.Point(65, 17);
            this.label_M1Pos.Name = "label_M1Pos";
            this.label_M1Pos.Size = new System.Drawing.Size(60, 20);
            this.label_M1Pos.TabIndex = 238;
            this.label_M1Pos.Text = "0.0";
            this.label_M1Pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_PickY
            // 
            this.label_PickY.AutoSize = true;
            this.label_PickY.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_PickY.Location = new System.Drawing.Point(8, 17);
            this.label_PickY.Name = "label_PickY";
            this.label_PickY.Size = new System.Drawing.Size(57, 17);
            this.label_PickY.TabIndex = 239;
            this.label_PickY.Text = "Head X: ";
            // 
            // btn_AddM1
            // 
            this.btn_AddM1.BackColor = System.Drawing.SystemColors.Window;
            this.btn_AddM1.Image = global::Cowain_AutoMotion.Properties.Resources.Add;
            this.btn_AddM1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_AddM1.Location = new System.Drawing.Point(10, 39);
            this.btn_AddM1.Name = "btn_AddM1";
            this.btn_AddM1.Size = new System.Drawing.Size(42, 30);
            this.btn_AddM1.TabIndex = 229;
            this.btn_AddM1.UseVisualStyleBackColor = false;
            this.btn_AddM1.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // btn_SubM1
            // 
            this.btn_SubM1.BackColor = System.Drawing.SystemColors.Window;
            this.btn_SubM1.Image = global::Cowain_AutoMotion.Properties.Resources.sub;
            this.btn_SubM1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SubM1.Location = new System.Drawing.Point(70, 41);
            this.btn_SubM1.Name = "btn_SubM1";
            this.btn_SubM1.Size = new System.Drawing.Size(42, 30);
            this.btn_SubM1.TabIndex = 230;
            this.btn_SubM1.UseVisualStyleBackColor = false;
            this.btn_SubM1.Click += new System.EventHandler(this.btn_ManulMove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(14, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 231;
            this.label1.Text = "吋動距離:";
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
            this.comboBox_Pitch.Location = new System.Drawing.Point(96, 25);
            this.comboBox_Pitch.Name = "comboBox_Pitch";
            this.comboBox_Pitch.Size = new System.Drawing.Size(132, 28);
            this.comboBox_Pitch.TabIndex = 232;
            // 
            // timer_CCDLive
            // 
            this.timer_CCDLive.Interval = 300;
            this.timer_CCDLive.Tick += new System.EventHandler(this.timer_CCDLive_Tick);
            // 
            // timer_ReFlash
            // 
            this.timer_ReFlash.Tick += new System.EventHandler(this.timer_ReFlash_Tick);
            // 
            // frm_VisionCCD1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 528);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_VisionCCD1";
            this.Text = "frm_VisionCC1";
            this.Shown += new System.EventHandler(this.frm_VisionCC1_Shown);
            this.VisibleChanged += new System.EventHandler(this.frm_VisionCC1_VisibleChanged);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox_Setting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Light)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox_M6.ResumeLayout(false);
            this.groupBox_M6.PerformLayout();
            this.groupBox_M5.ResumeLayout(false);
            this.groupBox_M5.PerformLayout();
            this.groupBox_M4.ResumeLayout(false);
            this.groupBox_M4.PerformLayout();
            this.groupBox_M3.ResumeLayout(false);
            this.groupBox_M3.PerformLayout();
            this.groupBox_M2.ResumeLayout(false);
            this.groupBox_M2.PerformLayout();
            this.groupBox_M1.ResumeLayout(false);
            this.groupBox_M1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBox_function;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Home;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Move;
        private System.Windows.Forms.ListView listView_Pos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox_M6;
        private System.Windows.Forms.Label label_M6Pos;
        private System.Windows.Forms.Label label_StageT;
        private System.Windows.Forms.Button btn_AddM6;
        private System.Windows.Forms.Button btn_SubM6;
        private System.Windows.Forms.GroupBox groupBox_M5;
        private System.Windows.Forms.Label label_M5Pos;
        private System.Windows.Forms.Label label_StageZ;
        private System.Windows.Forms.Button btn_AddM5;
        private System.Windows.Forms.Button btn_SubM5;
        private System.Windows.Forms.GroupBox groupBox_M4;
        private System.Windows.Forms.Label label_M4Pos;
        private System.Windows.Forms.Label label_StageX;
        private System.Windows.Forms.Button btn_AddM4;
        private System.Windows.Forms.Button btn_SubM4;
        private System.Windows.Forms.GroupBox groupBox_M3;
        private System.Windows.Forms.Label label_M3Pos;
        private System.Windows.Forms.Label label_TrayX;
        private System.Windows.Forms.Button btn_AddM3;
        private System.Windows.Forms.Button btn_SubM3;
        private System.Windows.Forms.GroupBox groupBox_M2;
        private System.Windows.Forms.Label label_M2Pos;
        private System.Windows.Forms.Label label_PickZ;
        private System.Windows.Forms.Button btn_AddM2;
        private System.Windows.Forms.Button btn_SubM2;
        private System.Windows.Forms.GroupBox groupBox_M1;
        private System.Windows.Forms.Label label_M1Pos;
        private System.Windows.Forms.Label label_PickY;
        private System.Windows.Forms.Button btn_AddM1;
        private System.Windows.Forms.Button btn_SubM1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_Pitch;
        private System.Windows.Forms.Button btn_live;
        private System.Windows.Forms.Button btn_Snap;
        private System.Windows.Forms.Label label_Light;
        private System.Windows.Forms.TrackBar trackBar_Light;
        private System.Windows.Forms.Timer timer_CCDLive;
        private System.Windows.Forms.Timer timer_ReFlash;
        private System.Windows.Forms.Button btn_FunctionRun;
        private System.Windows.Forms.Button btn_LightSet;
        private System.Windows.Forms.Button button_ShowROI;
        private System.Windows.Forms.Button button_SaveROI;
        private System.Windows.Forms.GroupBox groupBox_Setting;
    }
}