namespace Cowain_Form.FormView
{
    partial class frm_TeachValveView
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
            this.listView_Valve = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label_ValveID = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label_Open = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_txCloseSR = new System.Windows.Forms.Label();
            this.label_CloseSR = new System.Windows.Forms.Label();
            this.label_txClose = new System.Windows.Forms.Label();
            this.label_CloseIO = new System.Windows.Forms.Label();
            this.label_txOpenSR = new System.Windows.Forms.Label();
            this.label_OpenSR = new System.Windows.Forms.Label();
            this.label_txOpen = new System.Windows.Forms.Label();
            this.label_OpenIO = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label_Action = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_Close = new System.Windows.Forms.Label();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.comboBox_Mode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_Off = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_Open = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_Repeat = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_Valve
            // 
            this.listView_Valve.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Valve.HideSelection = false;
            this.listView_Valve.Location = new System.Drawing.Point(16, 86);
            this.listView_Valve.Name = "listView_Valve";
            this.listView_Valve.Size = new System.Drawing.Size(428, 357);
            this.listView_Valve.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView_Valve.TabIndex = 5;
            this.listView_Valve.TileSize = new System.Drawing.Size(308, 22);
            this.listView_Valve.UseCompatibleStateImageBehavior = false;
            this.listView_Valve.View = System.Windows.Forms.View.Tile;
            this.listView_Valve.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_Valve_MouseDoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 50);
            this.tabControl1.Location = new System.Drawing.Point(12, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1220, 58);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 17;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 54);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1212, 0);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "All";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label_ValveID
            // 
            this.label_ValveID.BackColor = System.Drawing.Color.SteelBlue;
            this.label_ValveID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_ValveID.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ValveID.Location = new System.Drawing.Point(481, 86);
            this.label_ValveID.Name = "label_ValveID";
            this.label_ValveID.Size = new System.Drawing.Size(747, 37);
            this.label_ValveID.TabIndex = 19;
            this.label_ValveID.Text = "Loading~~";
            this.label_ValveID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label_Open
            // 
            this.label_Open.BackColor = System.Drawing.Color.LightBlue;
            this.label_Open.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Open.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Open.Location = new System.Drawing.Point(136, 27);
            this.label_Open.Name = "label_Open";
            this.label_Open.Size = new System.Drawing.Size(20, 19);
            this.label_Open.TabIndex = 20;
            this.label_Open.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(20, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 20);
            this.label10.TabIndex = 220;
            this.label10.Text = "電磁閥_開 : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_txCloseSR);
            this.groupBox1.Controls.Add(this.label_CloseSR);
            this.groupBox1.Controls.Add(this.label_txClose);
            this.groupBox1.Controls.Add(this.label_CloseIO);
            this.groupBox1.Controls.Add(this.label_txOpenSR);
            this.groupBox1.Controls.Add(this.label_OpenSR);
            this.groupBox1.Controls.Add(this.label_txOpen);
            this.groupBox1.Controls.Add(this.label_OpenIO);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label_Action);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label_Close);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label_Open);
            this.groupBox1.Location = new System.Drawing.Point(481, 299);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(751, 144);
            this.groupBox1.TabIndex = 221;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "電磁閥動作訊號";
            // 
            // label_txCloseSR
            // 
            this.label_txCloseSR.AutoSize = true;
            this.label_txCloseSR.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_txCloseSR.Location = new System.Drawing.Point(262, 107);
            this.label_txCloseSR.Name = "label_txCloseSR";
            this.label_txCloseSR.Size = new System.Drawing.Size(72, 20);
            this.label_txCloseSR.TabIndex = 240;
            this.label_txCloseSR.Text = "關閉SR : ";
            // 
            // label_CloseSR
            // 
            this.label_CloseSR.BackColor = System.Drawing.Color.LightBlue;
            this.label_CloseSR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CloseSR.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_CloseSR.Location = new System.Drawing.Point(365, 106);
            this.label_CloseSR.Name = "label_CloseSR";
            this.label_CloseSR.Size = new System.Drawing.Size(20, 19);
            this.label_CloseSR.TabIndex = 239;
            this.label_CloseSR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_txClose
            // 
            this.label_txClose.AutoSize = true;
            this.label_txClose.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_txClose.Location = new System.Drawing.Point(262, 80);
            this.label_txClose.Name = "label_txClose";
            this.label_txClose.Size = new System.Drawing.Size(85, 20);
            this.label_txClose.TabIndex = 238;
            this.label_txClose.Text = "關閉輸出 : ";
            // 
            // label_CloseIO
            // 
            this.label_CloseIO.BackColor = System.Drawing.Color.LightBlue;
            this.label_CloseIO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CloseIO.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_CloseIO.Location = new System.Drawing.Point(365, 82);
            this.label_CloseIO.Name = "label_CloseIO";
            this.label_CloseIO.Size = new System.Drawing.Size(20, 19);
            this.label_CloseIO.TabIndex = 237;
            this.label_CloseIO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_txOpenSR
            // 
            this.label_txOpenSR.AutoSize = true;
            this.label_txOpenSR.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_txOpenSR.Location = new System.Drawing.Point(262, 52);
            this.label_txOpenSR.Name = "label_txOpenSR";
            this.label_txOpenSR.Size = new System.Drawing.Size(72, 20);
            this.label_txOpenSR.TabIndex = 236;
            this.label_txOpenSR.Text = "開啟SR : ";
            // 
            // label_OpenSR
            // 
            this.label_OpenSR.BackColor = System.Drawing.Color.LightBlue;
            this.label_OpenSR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_OpenSR.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_OpenSR.Location = new System.Drawing.Point(365, 54);
            this.label_OpenSR.Name = "label_OpenSR";
            this.label_OpenSR.Size = new System.Drawing.Size(20, 19);
            this.label_OpenSR.TabIndex = 235;
            this.label_OpenSR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_txOpen
            // 
            this.label_txOpen.AutoSize = true;
            this.label_txOpen.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_txOpen.Location = new System.Drawing.Point(262, 26);
            this.label_txOpen.Name = "label_txOpen";
            this.label_txOpen.Size = new System.Drawing.Size(85, 20);
            this.label_txOpen.TabIndex = 234;
            this.label_txOpen.Text = "開啟輸出 : ";
            // 
            // label_OpenIO
            // 
            this.label_OpenIO.BackColor = System.Drawing.Color.LightBlue;
            this.label_OpenIO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_OpenIO.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_OpenIO.Location = new System.Drawing.Point(365, 27);
            this.label_OpenIO.Name = "label_OpenIO";
            this.label_OpenIO.Size = new System.Drawing.Size(20, 19);
            this.label_OpenIO.TabIndex = 233;
            this.label_OpenIO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(20, 80);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 20);
            this.label12.TabIndex = 232;
            this.label12.Text = "電磁閥_動作 : ";
            // 
            // label_Action
            // 
            this.label_Action.BackColor = System.Drawing.Color.LightBlue;
            this.label_Action.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Action.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Action.Location = new System.Drawing.Point(136, 79);
            this.label_Action.Name = "label_Action";
            this.label_Action.Size = new System.Drawing.Size(20, 19);
            this.label_Action.TabIndex = 231;
            this.label_Action.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(20, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 20);
            this.label2.TabIndex = 222;
            this.label2.Text = "電磁閥_關 : ";
            // 
            // label_Close
            // 
            this.label_Close.BackColor = System.Drawing.Color.LightBlue;
            this.label_Close.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Close.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Close.Location = new System.Drawing.Point(136, 54);
            this.label_Close.Name = "label_Close";
            this.label_Close.Size = new System.Drawing.Size(20, 19);
            this.label_Close.TabIndex = 221;
            this.label_Close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Stop
            // 
            this.btn_Stop.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Stop.Location = new System.Drawing.Point(486, 85);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(109, 62);
            this.btn_Stop.TabIndex = 20;
            this.btn_Stop.Text = "    移動停止";
            this.btn_Stop.UseVisualStyleBackColor = false;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // comboBox_Mode
            // 
            this.comboBox_Mode.BackColor = System.Drawing.Color.Thistle;
            this.comboBox_Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Mode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_Mode.FormattingEnabled = true;
            this.comboBox_Mode.Items.AddRange(new object[] {
            "Mode_Normal",
            "Mode_Test",
            "Mode_OkThenOff",
            "Mode_NgThenOff"});
            this.comboBox_Mode.Location = new System.Drawing.Point(104, 36);
            this.comboBox_Mode.Name = "comboBox_Mode";
            this.comboBox_Mode.Size = new System.Drawing.Size(152, 28);
            this.comboBox_Mode.TabIndex = 226;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(8, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 24);
            this.label3.TabIndex = 225;
            this.label3.Text = "動作模式:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.btn_Off);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.btn_Open);
            this.groupBox3.Controls.Add(this.btn_Stop);
            this.groupBox3.Controls.Add(this.btn_Close);
            this.groupBox3.Controls.Add(this.btn_Repeat);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.comboBox_Mode);
            this.groupBox3.Location = new System.Drawing.Point(481, 129);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(747, 163);
            this.groupBox3.TabIndex = 227;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "馬達動作";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Window;
            this.button2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.Image = global::Cowain_AutoMotion.Properties.Resources.AlarmReset;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(131, 111);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 37);
            this.button2.TabIndex = 294;
            this.button2.Text = "        Pump Off";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_Off
            // 
            this.btn_Off.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Off.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Off.Location = new System.Drawing.Point(616, 19);
            this.btn_Off.Name = "btn_Off";
            this.btn_Off.Size = new System.Drawing.Size(109, 61);
            this.btn_Off.TabIndex = 230;
            this.btn_Off.Text = "訊號OFF";
            this.btn_Off.UseVisualStyleBackColor = false;
            this.btn_Off.Click += new System.EventHandler(this.btn_Off_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Window;
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Image = global::Cowain_AutoMotion.Properties.Resources.AlarmReset;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(12, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 37);
            this.button1.TabIndex = 293;
            this.button1.Text = "        Pump On";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Open
            // 
            this.btn_Open.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Open.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Open.Location = new System.Drawing.Point(353, 19);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(109, 61);
            this.btn_Open.TabIndex = 227;
            this.btn_Open.Text = "         開啟";
            this.btn_Open.UseVisualStyleBackColor = false;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Close.Location = new System.Drawing.Point(353, 86);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(109, 61);
            this.btn_Close.TabIndex = 228;
            this.btn_Close.Text = "          關閉";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_Repeat
            // 
            this.btn_Repeat.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Repeat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Repeat.Location = new System.Drawing.Point(486, 18);
            this.btn_Repeat.Name = "btn_Repeat";
            this.btn_Repeat.Size = new System.Drawing.Size(109, 61);
            this.btn_Repeat.TabIndex = 229;
            this.btn_Repeat.Text = "          A.B往返";
            this.btn_Repeat.UseVisualStyleBackColor = false;
            this.btn_Repeat.Click += new System.EventHandler(this.btn_Repeat_Click);
            // 
            // frm_TeachValveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 454);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label_ValveID);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listView_Valve);
            this.Font = new System.Drawing.Font("新細明體-ExtB", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_TeachValveView";
            this.Text = "frm_ValveView";
            this.Load += new System.EventHandler(this.frm_TeachValveView_Load);
            this.Shown += new System.EventHandler(this.frm_ValveView_Shown);
            this.VisibleChanged += new System.EventHandler(this.frm_ValveView_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_Valve;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label_ValveID;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label_Open;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label_Action;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_Close;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.ComboBox comboBox_Mode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_Repeat;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.Label label_txCloseSR;
        private System.Windows.Forms.Label label_CloseSR;
        private System.Windows.Forms.Label label_txClose;
        private System.Windows.Forms.Label label_CloseIO;
        private System.Windows.Forms.Label label_txOpenSR;
        private System.Windows.Forms.Label label_OpenSR;
        private System.Windows.Forms.Label label_txOpen;
        private System.Windows.Forms.Label label_OpenIO;
        private System.Windows.Forms.Button btn_Off;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}