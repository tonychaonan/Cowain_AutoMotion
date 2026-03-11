namespace Cowain_Form.FormView
{
    partial class frm_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Main));
            this.imageList64 = new System.Windows.Forms.ImageList(this.components);
            this.imageList30 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnHome = new System.Windows.Forms.ToolStripButton();
            this.btnAlarm = new System.Windows.Forms.ToolStripButton();
            this.btnControl = new System.Windows.Forms.ToolStripButton();
            this.btnData = new System.Windows.Forms.ToolStripButton();
            this.btnVision = new System.Windows.Forms.ToolStripButton();
            this.btnSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.btnPause = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelMode = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.picUser = new DevExpress.XtraEditors.PictureEdit();
            this.labType = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUser.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList64
            // 
            this.imageList64.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList64.ImageSize = new System.Drawing.Size(64, 64);
            this.imageList64.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList30
            // 
            this.imageList30.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList30.ImageStream")));
            this.imageList30.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList30.Images.SetKeyName(0, "密码.png");
            this.imageList30.Images.SetKeyName(1, "用户.png");
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(237)))));
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnHome,
            this.btnAlarm,
            this.btnControl,
            this.btnData,
            this.btnVision,
            this.btnSetting,
            this.toolStripSeparator3,
            this.toolStripSeparator1,
            this.btnStart,
            this.btnPause,
            this.btnStop,
            this.toolStripSeparator2});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1364, 71);
            this.toolStrip.TabIndex = 11;
            this.toolStrip.Text = "ToolStrip";
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Image = global::Cowain_AutoMotion.Properties.Resources.Home_Energized;
            this.btnHome.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnHome.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnHome.Margin = new System.Windows.Forms.Padding(0, 1, 12, 2);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(68, 44);
            this.btnHome.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHome.ToolTipText = "主界面";
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnAlarm
            // 
            this.btnAlarm.BackColor = System.Drawing.Color.Transparent;
            this.btnAlarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlarm.Image = global::Cowain_AutoMotion.Properties.Resources.Alarm_De_energized;
            this.btnAlarm.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAlarm.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnAlarm.Margin = new System.Windows.Forms.Padding(0, 1, 12, 2);
            this.btnAlarm.Name = "btnAlarm";
            this.btnAlarm.Size = new System.Drawing.Size(68, 44);
            this.btnAlarm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAlarm.ToolTipText = "报警";
            this.btnAlarm.Click += new System.EventHandler(this.btnAlarm_Click);
            // 
            // btnControl
            // 
            this.btnControl.BackColor = System.Drawing.Color.Transparent;
            this.btnControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnControl.Image = global::Cowain_AutoMotion.Properties.Resources.Config_Disabled;
            this.btnControl.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnControl.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnControl.Margin = new System.Windows.Forms.Padding(0, 1, 12, 2);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(68, 44);
            this.btnControl.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnControl.ToolTipText = "控制";
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // btnData
            // 
            this.btnData.BackColor = System.Drawing.Color.Transparent;
            this.btnData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnData.Image = global::Cowain_AutoMotion.Properties.Resources.Data_Disabled;
            this.btnData.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnData.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnData.Margin = new System.Windows.Forms.Padding(0, 1, 12, 2);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(68, 44);
            this.btnData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnData.ToolTipText = "数据";
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // btnVision
            // 
            this.btnVision.BackColor = System.Drawing.Color.Transparent;
            this.btnVision.Enabled = false;
            this.btnVision.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVision.Image = global::Cowain_AutoMotion.Properties.Resources.Vision_Disabled;
            this.btnVision.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnVision.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnVision.Margin = new System.Windows.Forms.Padding(0, 1, 12, 2);
            this.btnVision.Name = "btnVision";
            this.btnVision.Size = new System.Drawing.Size(68, 44);
            this.btnVision.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnVision.ToolTipText = "视觉";
            this.btnVision.Click += new System.EventHandler(this.btnVision_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.BackColor = System.Drawing.Color.Transparent;
            this.btnSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetting.Image = global::Cowain_AutoMotion.Properties.Resources.Setting_Disabled;
            this.btnSetting.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSetting.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(68, 44);
            this.btnSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSetting.ToolTipText = "设置";
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 500, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 47);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 47);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Image = global::Cowain_AutoMotion.Properties.Resources.Running_De_energized;
            this.btnStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(68, 44);
            this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStart.ToolTipText = "开始";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.Transparent;
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.Image = global::Cowain_AutoMotion.Properties.Resources.Paused_Disabled;
            this.btnPause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPause.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(68, 44);
            this.btnPause.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPause.ToolTipText = "暂停";
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Image = global::Cowain_AutoMotion.Properties.Resources.Stopped_Energized;
            this.btnStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(68, 44);
            this.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStop.ToolTipText = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 47);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4});
            this.statusStrip.Location = new System.Drawing.Point(0, 679);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1364, 22);
            this.statusStrip.TabIndex = 32;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel.Text = "设备状态：";
            this.toolStripStatusLabel.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel1.Text = "               ";
            this.toolStripStatusLabel1.Visible = false;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Enabled = false;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(94, 17);
            this.toolStripStatusLabel2.Text = "  ||  HIVE状态：";
            this.toolStripStatusLabel2.Visible = false;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel3.Text = "               ";
            this.toolStripStatusLabel3.Visible = false;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel4.Text = "内存";
            this.toolStripStatusLabel4.Visible = false;
            // 
            // labelMode
            // 
            this.labelMode.Appearance.BackColor = System.Drawing.Color.Silver;
            this.labelMode.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMode.Appearance.Options.UseBackColor = true;
            this.labelMode.Appearance.Options.UseFont = true;
            this.labelMode.Appearance.Options.UseTextOptions = true;
            this.labelMode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelMode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelMode.Location = new System.Drawing.Point(1201, 38);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(102, 27);
            this.labelMode.TabIndex = 34;
            this.labelMode.Text = "HIVE";
            this.labelMode.Click += new System.EventHandler(this.labelMode_Click);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = global::Cowain_AutoMotion.Properties.Resources.Audio_Logo;
            this.pictureEdit1.Location = new System.Drawing.Point(1308, 3);
            this.pictureEdit1.Name = "pictureEdit1";
            // 
            // 
            // 
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(237)))));
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.pictureEdit1.Size = new System.Drawing.Size(46, 64);
            this.pictureEdit1.TabIndex = 36;
            this.pictureEdit1.Click += new System.EventHandler(this.pictureEdit1_Click);
            // 
            // picUser
            // 
            this.picUser.EditValue = global::Cowain_AutoMotion.Properties.Resources.No_login;
            this.picUser.Location = new System.Drawing.Point(1201, 6);
            this.picUser.Name = "picUser";
            // 
            // 
            // 
            this.picUser.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(237)))));
            this.picUser.Properties.Appearance.Options.UseBackColor = true;
            this.picUser.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.picUser.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.picUser.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.picUser.Size = new System.Drawing.Size(102, 27);
            this.picUser.TabIndex = 36;
            this.picUser.EditValueChanged += new System.EventHandler(this.picUser_EditValueChanged);
            this.picUser.Click += new System.EventHandler(this.btnLogin_Click);
            this.picUser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picUser_MouseDown);
            // 
            // labType
            // 
            this.labType.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(130)))), ((int)(((byte)(81)))));
            this.labType.Appearance.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labType.Appearance.ForeColor = System.Drawing.Color.White;
            this.labType.Appearance.Options.UseBackColor = true;
            this.labType.Appearance.Options.UseFont = true;
            this.labType.Appearance.Options.UseForeColor = true;
            this.labType.Appearance.Options.UseTextOptions = true;
            this.labType.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labType.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labType.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.labType.Location = new System.Drawing.Point(482, 6);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(486, 59);
            this.labType.TabIndex = 39;
            this.labType.Text = "Normal(E/M)";
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.EditValue = global::Cowain_AutoMotion.Properties.Resources.E_SKU;
            this.pictureEdit2.Location = new System.Drawing.Point(482, 6);
            this.pictureEdit2.Name = "pictureEdit2";
            // 
            // 
            // 
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit2.Size = new System.Drawing.Size(486, 58);
            this.pictureEdit2.TabIndex = 41;
            // 
            // frm_Main
            // 
            this.Appearance.Options.UseTextOptions = true;
            this.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 701);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.labType);
            this.Controls.Add(this.picUser);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MdiChildCaptionFormatString = "{1}";
            this.Name = "frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.ShowNewForm);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUser.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList64;
        private System.Windows.Forms.ImageList imageList30;
        private System.Windows.Forms.ToolStrip toolStrip;
        public System.Windows.Forms.ToolStripButton btnHome;
        public System.Windows.Forms.ToolStripButton btnAlarm;
        public System.Windows.Forms.ToolStripButton btnControl;
        public System.Windows.Forms.ToolStripButton btnData;
        public System.Windows.Forms.ToolStripButton btnVision;
        public System.Windows.Forms.ToolStripButton btnSetting;
        public System.Windows.Forms.ToolStripButton btnStart;
        public System.Windows.Forms.ToolStripButton btnPause;
        public System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private DevExpress.XtraEditors.LabelControl labelMode;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private DevExpress.XtraEditors.PictureEdit picUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private DevExpress.XtraEditors.LabelControl labType;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
    }
}