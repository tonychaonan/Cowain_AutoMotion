namespace Cowain_AutoMotion.Simulat
{
    partial class selectCMDItemFrm
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
            this.lbMSG = new System.Windows.Forms.Label();
            this.cBoxCMD = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cBoxInstance = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cBoxType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lbMSG
            // 
            this.lbMSG.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMSG.Location = new System.Drawing.Point(63, 26);
            this.lbMSG.Name = "lbMSG";
            this.lbMSG.Size = new System.Drawing.Size(662, 29);
            this.lbMSG.TabIndex = 0;
            this.lbMSG.Text = "label1";
            this.lbMSG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cBoxCMD
            // 
            this.cBoxCMD.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBoxCMD.FormattingEnabled = true;
            this.cBoxCMD.Location = new System.Drawing.Point(221, 160);
            this.cBoxCMD.Name = "cBoxCMD";
            this.cBoxCMD.Size = new System.Drawing.Size(268, 29);
            this.cBoxCMD.TabIndex = 1;
            this.cBoxCMD.SelectedIndexChanged += new System.EventHandler(this.cBoxCMD_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(124, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "CMD:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(101, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Value:";
            // 
            // txtValue
            // 
            this.txtValue.Font = new System.Drawing.Font("宋体", 15.75F);
            this.txtValue.Location = new System.Drawing.Point(221, 285);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(268, 31);
            this.txtValue.TabIndex = 5;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(660, 247);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 45);
            this.button1.TabIndex = 6;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(64, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 21);
            this.label4.TabIndex = 7;
            this.label4.Text = "Instance:";
            // 
            // cBoxInstance
            // 
            this.cBoxInstance.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBoxInstance.FormattingEnabled = true;
            this.cBoxInstance.Location = new System.Drawing.Point(221, 221);
            this.cBoxInstance.Name = "cBoxInstance";
            this.cBoxInstance.Size = new System.Drawing.Size(268, 29);
            this.cBoxInstance.TabIndex = 8;
            this.cBoxInstance.SelectedIndexChanged += new System.EventHandler(this.cBoxInstance_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(124, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 21);
            this.label1.TabIndex = 10;
            this.label1.Text = "Type:";
            // 
            // cBoxType
            // 
            this.cBoxType.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cBoxType.FormattingEnabled = true;
            this.cBoxType.Items.AddRange(new object[] {
            "条件",
            "步骤"});
            this.cBoxType.Location = new System.Drawing.Point(221, 99);
            this.cBoxType.Name = "cBoxType";
            this.cBoxType.Size = new System.Drawing.Size(268, 29);
            this.cBoxType.TabIndex = 9;
            this.cBoxType.SelectedIndexChanged += new System.EventHandler(this.cBoxType_SelectedIndexChanged);
            // 
            // selectCMDItemFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 415);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cBoxType);
            this.Controls.Add(this.cBoxInstance);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cBoxCMD);
            this.Controls.Add(this.lbMSG);
            this.Name = "selectCMDItemFrm";
            this.Text = "selectItemFrm";
            this.Load += new System.EventHandler(this.selectCMDItemFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbMSG;
        private System.Windows.Forms.ComboBox cBoxCMD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cBoxInstance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cBoxType;
    }
}