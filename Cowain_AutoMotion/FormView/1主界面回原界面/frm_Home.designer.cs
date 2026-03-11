namespace Cowain_Form.FormView
{
    partial class frm_Home
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer_ReFlash = new System.Windows.Forms.Timer(this.components);
            this.listView_Home = new System.Windows.Forms.ListView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_Ver = new System.Windows.Forms.Label();
            this.group_Pick1 = new System.Windows.Forms.GroupBox();
            this.lbStationName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.group_Pick1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_Home;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(308, 457);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 65);
            this.button1.TabIndex = 1;
            this.button1.Text = "     归原";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_Stop;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(449, 457);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 65);
            this.button2.TabIndex = 2;
            this.button2.Text = "     停止";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer_ReFlash
            // 
            this.timer_ReFlash.Interval = 500;
            this.timer_ReFlash.Tick += new System.EventHandler(this.timer_ReFlash_Tick);
            // 
            // listView_Home
            // 
            this.listView_Home.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Home.HideSelection = false;
            this.listView_Home.Location = new System.Drawing.Point(27, 28);
            this.listView_Home.Name = "listView_Home";
            this.listView_Home.Size = new System.Drawing.Size(544, 423);
            this.listView_Home.TabIndex = 6;
            this.listView_Home.UseCompatibleStateImageBehavior = false;
            this.listView_Home.View = System.Windows.Forms.View.List;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(579, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(396, 266);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label_Ver
            // 
            this.label_Ver.AutoSize = true;
            this.label_Ver.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Ver.Location = new System.Drawing.Point(167, 19);
            this.label_Ver.Name = "label_Ver";
            this.label_Ver.Size = new System.Drawing.Size(153, 17);
            this.label_Ver.TabIndex = 8;
            this.label_Ver.Text = " 软件 Release: 2.0.0          ";
            // 
            // group_Pick1
            // 
            this.group_Pick1.Controls.Add(this.lbStationName);
            this.group_Pick1.Controls.Add(this.label12);
            this.group_Pick1.Controls.Add(this.label_Ver);
            this.group_Pick1.Controls.Add(this.label11);
            this.group_Pick1.Location = new System.Drawing.Point(579, 300);
            this.group_Pick1.Name = "group_Pick1";
            this.group_Pick1.Size = new System.Drawing.Size(396, 220);
            this.group_Pick1.TabIndex = 310;
            this.group_Pick1.TabStop = false;
            // 
            // lbStationName
            // 
            this.lbStationName.AutoSize = true;
            this.lbStationName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbStationName.Location = new System.Drawing.Point(110, 58);
            this.lbStationName.Name = "lbStationName";
            this.lbStationName.Size = new System.Drawing.Size(66, 17);
            this.lbStationName.TabIndex = 311;
            this.lbStationName.Text = "LH010----";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(12, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 17);
            this.label12.TabIndex = 310;
            this.label12.Text = "StationName:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(6, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(153, 18);
            this.label11.TabIndex = 1;
            this.label11.Text = "Machine Parameter";
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.button3.Location = new System.Drawing.Point(1139, 435);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(139, 53);
            this.button3.TabIndex = 22;
            this.button3.Text = "操作员登录";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // frm_Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 534);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.group_Pick1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.listView_Home);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("新宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Home";
            this.Text = "frm_Home";
            this.Load += new System.EventHandler(this.frm_Home_Load);
            this.Shown += new System.EventHandler(this.frm_Home_Shown);
            this.VisibleChanged += new System.EventHandler(this.frm_Home_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.group_Pick1.ResumeLayout(false);
            this.group_Pick1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer_ReFlash;
        private System.Windows.Forms.ListView listView_Home;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_Ver;
        private System.Windows.Forms.GroupBox group_Pick1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbStationName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button3;
    }
}