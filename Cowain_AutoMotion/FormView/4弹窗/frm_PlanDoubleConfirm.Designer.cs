namespace Cowain_Form.FormView
{
    partial class frm_PlanDoubleConfirm
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
            this.labType = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labold = new DevExpress.XtraEditors.LabelControl();
            this.labnew = new DevExpress.XtraEditors.LabelControl();
            this.btn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.txt_Old = new DevExpress.XtraEditors.ButtonEdit();
            this.txt_Op = new System.Windows.Forms.TextBox();
            this.timerop = new System.Windows.Forms.Timer(this.components);
            this.txt_New = new System.Windows.Forms.TextBox();
            this.btnnew = new DevExpress.XtraEditors.SimpleButton();
            this.timerSN = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Old.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labType
            // 
            this.labType.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labType.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.labType.Appearance.Options.UseFont = true;
            this.labType.Appearance.Options.UseForeColor = true;
            this.labType.Location = new System.Drawing.Point(13, 13);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(132, 20);
            this.labType.TabIndex = 0;
            this.labType.Text = "PD-03:Glue Change";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(40, 53);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(53, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "Operator:";
            // 
            // labold
            // 
            this.labold.Location = new System.Drawing.Point(40, 79);
            this.labold.Name = "labold";
            this.labold.Size = new System.Drawing.Size(41, 14);
            this.labold.TabIndex = 0;
            this.labold.Text = "Old SN:";
            // 
            // labnew
            // 
            this.labnew.Location = new System.Drawing.Point(40, 105);
            this.labnew.Name = "labnew";
            this.labnew.Size = new System.Drawing.Size(48, 14);
            this.labnew.TabIndex = 0;
            this.labnew.Text = "New SN:";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(99, 138);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(68, 23);
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "取消";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(173, 138);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(68, 23);
            this.btn_OK.TabIndex = 3;
            this.btn_OK.Text = "确认";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // txt_Old
            // 
            this.txt_Old.Location = new System.Drawing.Point(99, 76);
            this.txt_Old.Name = "txt_Old";
            this.txt_Old.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txt_Old.Properties.ReadOnly = true;
            this.txt_Old.Size = new System.Drawing.Size(207, 20);
            this.txt_Old.TabIndex = 5;
            this.txt_Old.TabStop = false;
            this.txt_Old.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txt_Old_ButtonClick);
            // 
            // txt_Op
            // 
            this.txt_Op.Location = new System.Drawing.Point(99, 50);
            this.txt_Op.Name = "txt_Op";
            this.txt_Op.ShortcutsEnabled = false;
            this.txt_Op.Size = new System.Drawing.Size(207, 22);
            this.txt_Op.TabIndex = 0;
            this.txt_Op.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Op_KeyDown);
            this.txt_Op.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Op_KeyPress);
            // 
            // timerop
            // 
            this.timerop.Interval = 500;
            this.timerop.Tick += new System.EventHandler(this.timerop_Tick);
            // 
            // txt_New
            // 
            this.txt_New.Location = new System.Drawing.Point(99, 102);
            this.txt_New.Name = "txt_New";
            this.txt_New.ShortcutsEnabled = false;
            this.txt_New.Size = new System.Drawing.Size(189, 22);
            this.txt_New.TabIndex = 1;
            this.txt_New.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_New_KeyDown);
            // 
            // btnnew
            // 
            this.btnnew.Location = new System.Drawing.Point(289, 102);
            this.btnnew.Name = "btnnew";
            this.btnnew.Size = new System.Drawing.Size(17, 23);
            this.btnnew.TabIndex = 8;
            this.btnnew.Text = "...";
            this.btnnew.Click += new System.EventHandler(this.btnnew_Click);
            // 
            // timerSN
            // 
            this.timerSN.Tick += new System.EventHandler(this.timerSN_Tick);
            // 
            // frm_PlanDoubleConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 171);
            this.Controls.Add(this.btnnew);
            this.Controls.Add(this.txt_New);
            this.Controls.Add(this.txt_Op);
            this.Controls.Add(this.txt_Old);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.labnew);
            this.Controls.Add(this.labold);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_PlanDoubleConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "二次确认";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frm_PlanDoubleConfirm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Old.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labType;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labold;
        private DevExpress.XtraEditors.LabelControl labnew;
        private DevExpress.XtraEditors.SimpleButton btn_Cancel;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.ButtonEdit txt_Old;
        private System.Windows.Forms.TextBox txt_Op;
        private System.Windows.Forms.Timer timerop;
        private System.Windows.Forms.TextBox txt_New;
        private DevExpress.XtraEditors.SimpleButton btnnew;
        private System.Windows.Forms.Timer timerSN;
    }
}