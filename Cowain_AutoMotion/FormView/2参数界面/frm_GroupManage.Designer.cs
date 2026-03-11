namespace Cowain_AutoMotion
{
    partial class frm_GroupManage
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
            this.label1 = new System.Windows.Forms.Label();
            this.listView_Station = new System.Windows.Forms.ListView();
            this.cBoxType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listView_Hard = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(239, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 29);
            this.label1.TabIndex = 7;
            this.label1.Text = "所有工位集合";
            // 
            // listView_Station
            // 
            this.listView_Station.HideSelection = false;
            this.listView_Station.Location = new System.Drawing.Point(247, 50);
            this.listView_Station.Name = "listView_Station";
            this.listView_Station.Size = new System.Drawing.Size(176, 387);
            this.listView_Station.TabIndex = 4;
            this.listView_Station.UseCompatibleStateImageBehavior = false;
            this.listView_Station.View = System.Windows.Forms.View.List;
            this.listView_Station.SelectedIndexChanged += new System.EventHandler(this.listView_Station_SelectedIndexChanged);
            // 
            // cBoxType
            // 
            this.cBoxType.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBoxType.FormattingEnabled = true;
            this.cBoxType.Items.AddRange(new object[] {
            "输入IO",
            "输出IO",
            "气缸"});
            this.cBoxType.Location = new System.Drawing.Point(39, 169);
            this.cBoxType.Name = "cBoxType";
            this.cBoxType.Size = new System.Drawing.Size(145, 29);
            this.cBoxType.TabIndex = 8;
            this.cBoxType.SelectedIndexChanged += new System.EventHandler(this.cBoxType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(46, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 29);
            this.label2.TabIndex = 9;
            this.label2.Text = "选择类型";
            // 
            // listView_Hard
            // 
            this.listView_Hard.HideSelection = false;
            this.listView_Hard.Location = new System.Drawing.Point(501, 50);
            this.listView_Hard.Name = "listView_Hard";
            this.listView_Hard.Size = new System.Drawing.Size(246, 387);
            this.listView_Hard.TabIndex = 10;
            this.listView_Hard.UseCompatibleStateImageBehavior = false;
            this.listView_Hard.View = System.Windows.Forms.View.List;
            this.listView_Hard.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(502, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(245, 29);
            this.label3.TabIndex = 11;
            this.label3.Text = "当前工位硬件集合";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(634, 443);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "删除";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(551, 443);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "添加";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.SystemColors.Window;
            this.btn_Save.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Save.Image = global::Cowain_AutoMotion.Properties.Resources.file_Save;
            this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Save.Location = new System.Drawing.Point(1077, 379);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(74, 75);
            this.btn_Save.TabIndex = 238;
            this.btn_Save.Text = "Save File";
            this.btn_Save.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // frm_GroupManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 483);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listView_Hard);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cBoxType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView_Station);
            this.Name = "frm_GroupManage";
            this.Text = "frm_TeachGroupForHardWare";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView_Station;
        private System.Windows.Forms.ComboBox cBoxType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView_Hard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_Save;
    }
}