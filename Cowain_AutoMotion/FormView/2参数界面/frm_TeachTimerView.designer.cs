namespace Cowain_Form.FormView
{
    partial class frm_TeachTimerView
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
            this.dataGridViewTimer = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Interval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_SaveData = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTimer
            // 
            this.dataGridViewTimer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTimer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.CName,
            this.EName,
            this.Interval});
            this.dataGridViewTimer.Location = new System.Drawing.Point(34, 23);
            this.dataGridViewTimer.Name = "dataGridViewTimer";
            this.dataGridViewTimer.RowTemplate.Height = 24;
            this.dataGridViewTimer.Size = new System.Drawing.Size(938, 269);
            this.dataGridViewTimer.TabIndex = 0;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // CName
            // 
            this.CName.HeaderText = "CName";
            this.CName.Name = "CName";
            this.CName.Width = 300;
            // 
            // EName
            // 
            this.EName.HeaderText = "EName";
            this.EName.Name = "EName";
            this.EName.Width = 300;
            // 
            // Interval
            // 
            this.Interval.HeaderText = "Interval";
            this.Interval.Name = "Interval";
            // 
            // btn_SaveData
            // 
            this.btn_SaveData.BackColor = System.Drawing.Color.Transparent;
            this.btn_SaveData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_SaveData.Image = global::Cowain_AutoMotion.Properties.Resources.Motor_DataSave;
            this.btn_SaveData.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_SaveData.Location = new System.Drawing.Point(753, 347);
            this.btn_SaveData.Name = "btn_SaveData";
            this.btn_SaveData.Size = new System.Drawing.Size(96, 49);
            this.btn_SaveData.TabIndex = 1;
            this.btn_SaveData.Text = "Save";
            this.btn_SaveData.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_SaveData.UseVisualStyleBackColor = false;
            // 
            // frm_TeachTimerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 418);
            this.Controls.Add(this.btn_SaveData);
            this.Controls.Add(this.dataGridViewTimer);
            this.Font = new System.Drawing.Font("新細明體-ExtB", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_TeachTimerView";
            this.Text = "frm_TimerView";
            this.Load += new System.EventHandler(this.frm_TeachTimerView_Load);
            this.Shown += new System.EventHandler(this.frm_TimerView_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTimer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Interval;
        private System.Windows.Forms.Button btn_SaveData;
        private System.Windows.Forms.Timer timer1;
    }
}