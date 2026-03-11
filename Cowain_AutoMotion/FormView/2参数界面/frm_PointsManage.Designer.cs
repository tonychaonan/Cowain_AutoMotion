namespace Cowain_AutoMotion
{
    partial class frm_PointsManage
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
            this.listView_Station = new System.Windows.Forms.ListView();
            this.btnAddStation = new System.Windows.Forms.Button();
            this.btnDelStation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelePoint = new System.Windows.Forms.Button();
            this.btnAddPoint = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cBox_X = new System.Windows.Forms.ComboBox();
            this.cBox_Y = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cBox_R = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cBox_Z = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cBox_A = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.cBox_PopA = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cBox_PopR = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cBox_PopZ = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cBox_PopY = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cBox_PopX = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.numXSpeed = new System.Windows.Forms.NumericUpDown();
            this.numYSpeed = new System.Windows.Forms.NumericUpDown();
            this.numZSpeed = new System.Windows.Forms.NumericUpDown();
            this.numRSpeed = new System.Windows.Forms.NumericUpDown();
            this.numASpeed = new System.Windows.Forms.NumericUpDown();
            this.cBoxZSafe = new System.Windows.Forms.CheckBox();
            this.cBoxNoUseX = new System.Windows.Forms.CheckBox();
            this.cBoxNoUseY = new System.Windows.Forms.CheckBox();
            this.cBoxNoUseZ = new System.Windows.Forms.CheckBox();
            this.cBoxNoUseR = new System.Windows.Forms.CheckBox();
            this.cBoxNoUseA = new System.Windows.Forms.CheckBox();
            this.cBoxNoEnablePoint = new System.Windows.Forms.CheckBox();
            this.txtPoint1 = new System.Windows.Forms.TextBox();
            this.txtPoint2 = new System.Windows.Forms.TextBox();
            this.txtPoint3 = new System.Windows.Forms.TextBox();
            this.txtPoint4 = new System.Windows.Forms.TextBox();
            this.txtPoint5 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listView_Points = new System.Windows.Forms.ListView();
            this.cBoxSafePoint = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numXSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numASpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // listView_Station
            // 
            this.listView_Station.HideSelection = false;
            this.listView_Station.Location = new System.Drawing.Point(12, 60);
            this.listView_Station.Name = "listView_Station";
            this.listView_Station.Size = new System.Drawing.Size(162, 387);
            this.listView_Station.TabIndex = 0;
            this.listView_Station.UseCompatibleStateImageBehavior = false;
            this.listView_Station.View = System.Windows.Forms.View.Tile;
            this.listView_Station.Click += new System.EventHandler(this.listView_Station_Click);
            // 
            // btnAddStation
            // 
            this.btnAddStation.Location = new System.Drawing.Point(12, 461);
            this.btnAddStation.Name = "btnAddStation";
            this.btnAddStation.Size = new System.Drawing.Size(75, 23);
            this.btnAddStation.TabIndex = 1;
            this.btnAddStation.Text = "添加";
            this.btnAddStation.UseVisualStyleBackColor = true;
            this.btnAddStation.Click += new System.EventHandler(this.btnAddStation_Click);
            // 
            // btnDelStation
            // 
            this.btnDelStation.Location = new System.Drawing.Point(95, 461);
            this.btnDelStation.Name = "btnDelStation";
            this.btnDelStation.Size = new System.Drawing.Size(75, 23);
            this.btnDelStation.TabIndex = 2;
            this.btnDelStation.Text = "删除";
            this.btnDelStation.UseVisualStyleBackColor = true;
            this.btnDelStation.Click += new System.EventHandler(this.btnDelStation_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(47, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "工位";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(482, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "点位";
            // 
            // btnDelePoint
            // 
            this.btnDelePoint.Location = new System.Drawing.Point(518, 461);
            this.btnDelePoint.Name = "btnDelePoint";
            this.btnDelePoint.Size = new System.Drawing.Size(75, 23);
            this.btnDelePoint.TabIndex = 6;
            this.btnDelePoint.Text = "删除";
            this.btnDelePoint.UseVisualStyleBackColor = true;
            this.btnDelePoint.Click += new System.EventHandler(this.btnDelePoint_Click);
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.Location = new System.Drawing.Point(435, 461);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(75, 23);
            this.btnAddPoint.TabIndex = 5;
            this.btnAddPoint.Text = "添加";
            this.btnAddPoint.UseVisualStyleBackColor = true;
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(191, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "X-Data1:";
            // 
            // cBox_X
            // 
            this.cBox_X.FormattingEnabled = true;
            this.cBox_X.Location = new System.Drawing.Point(259, 141);
            this.cBox_X.Name = "cBox_X";
            this.cBox_X.Size = new System.Drawing.Size(122, 20);
            this.cBox_X.TabIndex = 9;
            // 
            // cBox_Y
            // 
            this.cBox_Y.FormattingEnabled = true;
            this.cBox_Y.Location = new System.Drawing.Point(259, 182);
            this.cBox_Y.Name = "cBox_Y";
            this.cBox_Y.Size = new System.Drawing.Size(122, 20);
            this.cBox_Y.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(191, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "Y-Data2:";
            // 
            // cBox_R
            // 
            this.cBox_R.FormattingEnabled = true;
            this.cBox_R.Location = new System.Drawing.Point(259, 265);
            this.cBox_R.Name = "cBox_R";
            this.cBox_R.Size = new System.Drawing.Size(122, 20);
            this.cBox_R.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(191, 269);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "R-Data4:";
            // 
            // cBox_Z
            // 
            this.cBox_Z.FormattingEnabled = true;
            this.cBox_Z.Location = new System.Drawing.Point(259, 224);
            this.cBox_Z.Name = "cBox_Z";
            this.cBox_Z.Size = new System.Drawing.Size(122, 20);
            this.cBox_Z.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(191, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "Z-Data3:";
            // 
            // cBox_A
            // 
            this.cBox_A.FormattingEnabled = true;
            this.cBox_A.Location = new System.Drawing.Point(259, 315);
            this.cBox_A.Name = "cBox_A";
            this.cBox_A.Size = new System.Drawing.Size(122, 20);
            this.cBox_A.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(191, 319);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "A-Data5:";
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Save.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Save.Image = global::Cowain_AutoMotion.Properties.Resources.file_Save;
            this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Save.Location = new System.Drawing.Point(1000, 411);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(74, 75);
            this.btn_Save.TabIndex = 237;
            this.btn_Save.Text = "Save File";
            this.btn_Save.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // cBox_PopA
            // 
            this.cBox_PopA.FormattingEnabled = true;
            this.cBox_PopA.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cBox_PopA.Location = new System.Drawing.Point(719, 315);
            this.cBox_PopA.Name = "cBox_PopA";
            this.cBox_PopA.Size = new System.Drawing.Size(122, 20);
            this.cBox_PopA.TabIndex = 247;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(651, 319);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 246;
            this.label8.Text = "A-Data5:";
            // 
            // cBox_PopR
            // 
            this.cBox_PopR.FormattingEnabled = true;
            this.cBox_PopR.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cBox_PopR.Location = new System.Drawing.Point(719, 271);
            this.cBox_PopR.Name = "cBox_PopR";
            this.cBox_PopR.Size = new System.Drawing.Size(122, 20);
            this.cBox_PopR.TabIndex = 245;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(651, 275);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 244;
            this.label9.Text = "R-Data4:";
            // 
            // cBox_PopZ
            // 
            this.cBox_PopZ.FormattingEnabled = true;
            this.cBox_PopZ.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cBox_PopZ.Location = new System.Drawing.Point(719, 230);
            this.cBox_PopZ.Name = "cBox_PopZ";
            this.cBox_PopZ.Size = new System.Drawing.Size(122, 20);
            this.cBox_PopZ.TabIndex = 243;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(651, 234);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 242;
            this.label10.Text = "Z-Data3:";
            // 
            // cBox_PopY
            // 
            this.cBox_PopY.FormattingEnabled = true;
            this.cBox_PopY.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cBox_PopY.Location = new System.Drawing.Point(719, 188);
            this.cBox_PopY.Name = "cBox_PopY";
            this.cBox_PopY.Size = new System.Drawing.Size(122, 20);
            this.cBox_PopY.TabIndex = 241;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(651, 192);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 240;
            this.label11.Text = "Y-Data2:";
            // 
            // cBox_PopX
            // 
            this.cBox_PopX.FormattingEnabled = true;
            this.cBox_PopX.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cBox_PopX.Location = new System.Drawing.Point(719, 147);
            this.cBox_PopX.Name = "cBox_PopX";
            this.cBox_PopX.Size = new System.Drawing.Size(122, 20);
            this.cBox_PopX.TabIndex = 239;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(651, 151);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 238;
            this.label12.Text = "X-Data1:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(231, 86);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 29);
            this.label13.TabIndex = 248;
            this.label13.Text = "绑定轴";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(767, 86);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(245, 29);
            this.label14.TabIndex = 249;
            this.label14.Text = "绑定优先级和速度";
            // 
            // numXSpeed
            // 
            this.numXSpeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numXSpeed.Location = new System.Drawing.Point(858, 145);
            this.numXSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numXSpeed.Name = "numXSpeed";
            this.numXSpeed.Size = new System.Drawing.Size(73, 21);
            this.numXSpeed.TabIndex = 250;
            this.numXSpeed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numYSpeed
            // 
            this.numYSpeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numYSpeed.Location = new System.Drawing.Point(858, 188);
            this.numYSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numYSpeed.Name = "numYSpeed";
            this.numYSpeed.Size = new System.Drawing.Size(73, 21);
            this.numYSpeed.TabIndex = 251;
            this.numYSpeed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numZSpeed
            // 
            this.numZSpeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numZSpeed.Location = new System.Drawing.Point(858, 230);
            this.numZSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numZSpeed.Name = "numZSpeed";
            this.numZSpeed.Size = new System.Drawing.Size(73, 21);
            this.numZSpeed.TabIndex = 252;
            this.numZSpeed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numRSpeed
            // 
            this.numRSpeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numRSpeed.Location = new System.Drawing.Point(858, 270);
            this.numRSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRSpeed.Name = "numRSpeed";
            this.numRSpeed.Size = new System.Drawing.Size(73, 21);
            this.numRSpeed.TabIndex = 253;
            this.numRSpeed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numASpeed
            // 
            this.numASpeed.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numASpeed.Location = new System.Drawing.Point(858, 314);
            this.numASpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numASpeed.Name = "numASpeed";
            this.numASpeed.Size = new System.Drawing.Size(73, 21);
            this.numASpeed.TabIndex = 254;
            this.numASpeed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // cBoxZSafe
            // 
            this.cBoxZSafe.AutoSize = true;
            this.cBoxZSafe.ForeColor = System.Drawing.Color.Red;
            this.cBoxZSafe.Location = new System.Drawing.Point(858, 363);
            this.cBoxZSafe.Name = "cBoxZSafe";
            this.cBoxZSafe.Size = new System.Drawing.Size(114, 16);
            this.cBoxZSafe.TabIndex = 255;
            this.cBoxZSafe.Text = "Z先运动到安全位";
            this.cBoxZSafe.UseVisualStyleBackColor = true;
            // 
            // cBoxNoUseX
            // 
            this.cBoxNoUseX.AutoSize = true;
            this.cBoxNoUseX.ForeColor = System.Drawing.Color.Red;
            this.cBoxNoUseX.Location = new System.Drawing.Point(946, 147);
            this.cBoxNoUseX.Name = "cBoxNoUseX";
            this.cBoxNoUseX.Size = new System.Drawing.Size(48, 16);
            this.cBoxNoUseX.TabIndex = 256;
            this.cBoxNoUseX.Text = "禁用";
            this.cBoxNoUseX.UseVisualStyleBackColor = true;
            // 
            // cBoxNoUseY
            // 
            this.cBoxNoUseY.AutoSize = true;
            this.cBoxNoUseY.ForeColor = System.Drawing.Color.Red;
            this.cBoxNoUseY.Location = new System.Drawing.Point(946, 192);
            this.cBoxNoUseY.Name = "cBoxNoUseY";
            this.cBoxNoUseY.Size = new System.Drawing.Size(48, 16);
            this.cBoxNoUseY.TabIndex = 257;
            this.cBoxNoUseY.Text = "禁用";
            this.cBoxNoUseY.UseVisualStyleBackColor = true;
            // 
            // cBoxNoUseZ
            // 
            this.cBoxNoUseZ.AutoSize = true;
            this.cBoxNoUseZ.ForeColor = System.Drawing.Color.Red;
            this.cBoxNoUseZ.Location = new System.Drawing.Point(946, 232);
            this.cBoxNoUseZ.Name = "cBoxNoUseZ";
            this.cBoxNoUseZ.Size = new System.Drawing.Size(48, 16);
            this.cBoxNoUseZ.TabIndex = 258;
            this.cBoxNoUseZ.Text = "禁用";
            this.cBoxNoUseZ.UseVisualStyleBackColor = true;
            // 
            // cBoxNoUseR
            // 
            this.cBoxNoUseR.AutoSize = true;
            this.cBoxNoUseR.ForeColor = System.Drawing.Color.Red;
            this.cBoxNoUseR.Location = new System.Drawing.Point(946, 272);
            this.cBoxNoUseR.Name = "cBoxNoUseR";
            this.cBoxNoUseR.Size = new System.Drawing.Size(48, 16);
            this.cBoxNoUseR.TabIndex = 259;
            this.cBoxNoUseR.Text = "禁用";
            this.cBoxNoUseR.UseVisualStyleBackColor = true;
            // 
            // cBoxNoUseA
            // 
            this.cBoxNoUseA.AutoSize = true;
            this.cBoxNoUseA.ForeColor = System.Drawing.Color.Red;
            this.cBoxNoUseA.Location = new System.Drawing.Point(946, 317);
            this.cBoxNoUseA.Name = "cBoxNoUseA";
            this.cBoxNoUseA.Size = new System.Drawing.Size(48, 16);
            this.cBoxNoUseA.TabIndex = 260;
            this.cBoxNoUseA.Text = "禁用";
            this.cBoxNoUseA.UseVisualStyleBackColor = true;
            // 
            // cBoxNoEnablePoint
            // 
            this.cBoxNoEnablePoint.AutoSize = true;
            this.cBoxNoEnablePoint.ForeColor = System.Drawing.Color.Red;
            this.cBoxNoEnablePoint.Location = new System.Drawing.Point(666, 363);
            this.cBoxNoEnablePoint.Name = "cBoxNoEnablePoint";
            this.cBoxNoEnablePoint.Size = new System.Drawing.Size(72, 16);
            this.cBoxNoEnablePoint.TabIndex = 261;
            this.cBoxNoEnablePoint.Text = "点位禁用";
            this.cBoxNoEnablePoint.UseVisualStyleBackColor = true;
            // 
            // txtPoint1
            // 
            this.txtPoint1.Location = new System.Drawing.Point(1000, 144);
            this.txtPoint1.Name = "txtPoint1";
            this.txtPoint1.Size = new System.Drawing.Size(100, 21);
            this.txtPoint1.TabIndex = 262;
            // 
            // txtPoint2
            // 
            this.txtPoint2.Location = new System.Drawing.Point(1000, 190);
            this.txtPoint2.Name = "txtPoint2";
            this.txtPoint2.Size = new System.Drawing.Size(100, 21);
            this.txtPoint2.TabIndex = 263;
            // 
            // txtPoint3
            // 
            this.txtPoint3.Location = new System.Drawing.Point(1000, 232);
            this.txtPoint3.Name = "txtPoint3";
            this.txtPoint3.Size = new System.Drawing.Size(100, 21);
            this.txtPoint3.TabIndex = 264;
            // 
            // txtPoint4
            // 
            this.txtPoint4.Location = new System.Drawing.Point(1000, 270);
            this.txtPoint4.Name = "txtPoint4";
            this.txtPoint4.Size = new System.Drawing.Size(100, 21);
            this.txtPoint4.TabIndex = 265;
            // 
            // txtPoint5
            // 
            this.txtPoint5.Location = new System.Drawing.Point(1000, 317);
            this.txtPoint5.Name = "txtPoint5";
            this.txtPoint5.Size = new System.Drawing.Size(100, 21);
            this.txtPoint5.TabIndex = 266;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(858, 434);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 32);
            this.button1.TabIndex = 268;
            this.button1.Text = "导出配置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView_Points
            // 
            this.listView_Points.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Points.HideSelection = false;
            this.listView_Points.Location = new System.Drawing.Point(396, 60);
            this.listView_Points.Name = "listView_Points";
            this.listView_Points.Size = new System.Drawing.Size(230, 387);
            this.listView_Points.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView_Points.TabIndex = 269;
            this.listView_Points.TileSize = new System.Drawing.Size(308, 22);
            this.listView_Points.UseCompatibleStateImageBehavior = false;
            this.listView_Points.View = System.Windows.Forms.View.Tile;
            this.listView_Points.Click += new System.EventHandler(this.listView_Points_Click);
            // 
            // cBoxSafePoint
            // 
            this.cBoxSafePoint.FormattingEnabled = true;
            this.cBoxSafePoint.Location = new System.Drawing.Point(978, 361);
            this.cBoxSafePoint.Name = "cBoxSafePoint";
            this.cBoxSafePoint.Size = new System.Drawing.Size(122, 20);
            this.cBoxSafePoint.TabIndex = 270;
            this.cBoxSafePoint.Click += new System.EventHandler(this.cBoxSafePoint_Click);
            // 
            // frm_PointsManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 534);
            this.Controls.Add(this.cBoxSafePoint);
            this.Controls.Add(this.listView_Points);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtPoint5);
            this.Controls.Add(this.txtPoint4);
            this.Controls.Add(this.txtPoint3);
            this.Controls.Add(this.txtPoint2);
            this.Controls.Add(this.txtPoint1);
            this.Controls.Add(this.cBoxNoEnablePoint);
            this.Controls.Add(this.cBoxNoUseA);
            this.Controls.Add(this.cBoxNoUseR);
            this.Controls.Add(this.cBoxNoUseZ);
            this.Controls.Add(this.cBoxNoUseY);
            this.Controls.Add(this.cBoxNoUseX);
            this.Controls.Add(this.cBoxZSafe);
            this.Controls.Add(this.numASpeed);
            this.Controls.Add(this.numRSpeed);
            this.Controls.Add(this.numZSpeed);
            this.Controls.Add(this.numYSpeed);
            this.Controls.Add(this.numXSpeed);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cBox_PopA);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cBox_PopR);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cBox_PopZ);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cBox_PopY);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cBox_PopX);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.cBox_A);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cBox_R);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cBox_Z);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cBox_Y);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cBox_X);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelePoint);
            this.Controls.Add(this.btnAddPoint);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelStation);
            this.Controls.Add(this.btnAddStation);
            this.Controls.Add(this.listView_Station);
            this.Name = "frm_PointsManage";
            this.Text = "frm_PointsManage";
            this.Load += new System.EventHandler(this.frm_PointsManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numXSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numASpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_Station;
        private System.Windows.Forms.Button btnAddStation;
        private System.Windows.Forms.Button btnDelStation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDelePoint;
        private System.Windows.Forms.Button btnAddPoint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cBox_X;
        private System.Windows.Forms.ComboBox cBox_Y;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cBox_R;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cBox_Z;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cBox_A;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.ComboBox cBox_PopA;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cBox_PopR;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cBox_PopZ;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cBox_PopY;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cBox_PopX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numXSpeed;
        private System.Windows.Forms.NumericUpDown numYSpeed;
        private System.Windows.Forms.NumericUpDown numZSpeed;
        private System.Windows.Forms.NumericUpDown numRSpeed;
        private System.Windows.Forms.NumericUpDown numASpeed;
        private System.Windows.Forms.CheckBox cBoxZSafe;
        private System.Windows.Forms.CheckBox cBoxNoUseX;
        private System.Windows.Forms.CheckBox cBoxNoUseY;
        private System.Windows.Forms.CheckBox cBoxNoUseZ;
        private System.Windows.Forms.CheckBox cBoxNoUseR;
        private System.Windows.Forms.CheckBox cBoxNoUseA;
        private System.Windows.Forms.CheckBox cBoxNoEnablePoint;
        private System.Windows.Forms.TextBox txtPoint1;
        private System.Windows.Forms.TextBox txtPoint2;
        private System.Windows.Forms.TextBox txtPoint3;
        private System.Windows.Forms.TextBox txtPoint4;
        private System.Windows.Forms.TextBox txtPoint5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView_Points;
        private System.Windows.Forms.ComboBox cBoxSafePoint;
    }
}