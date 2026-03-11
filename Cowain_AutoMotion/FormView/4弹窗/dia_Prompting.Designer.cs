namespace Cowain_AutoMotion.FormView._4弹窗
{
    partial class dia_Prompting
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tb_Content = new System.Windows.Forms.TextBox();
            this.tb_Title = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(69, 279);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(221, 39);
            this.button1.TabIndex = 7;
            this.button1.Text = "确 认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(433, 279);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(221, 39);
            this.button2.TabIndex = 8;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tb_Content
            // 
            this.tb_Content.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tb_Content.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_Content.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_Content.ForeColor = System.Drawing.Color.Orange;
            this.tb_Content.Location = new System.Drawing.Point(48, 110);
            this.tb_Content.Margin = new System.Windows.Forms.Padding(4);
            this.tb_Content.Name = "tb_Content";
            this.tb_Content.Size = new System.Drawing.Size(636, 42);
            this.tb_Content.TabIndex = 10;
            this.tb_Content.Text = "请确认机台中无产品,再自动运行!";
            this.tb_Content.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Title
            // 
            this.tb_Title.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tb_Title.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_Title.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_Title.ForeColor = System.Drawing.Color.Green;
            this.tb_Title.Location = new System.Drawing.Point(48, 39);
            this.tb_Title.Margin = new System.Windows.Forms.Padding(4);
            this.tb_Title.Name = "tb_Title";
            this.tb_Title.Size = new System.Drawing.Size(636, 42);
            this.tb_Title.TabIndex = 11;
            this.tb_Title.Text = "提 示！";
            this.tb_Title.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dia_Prompting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LawnGreen;
            this.ClientSize = new System.Drawing.Size(760, 381);
            this.ControlBox = false;
            this.Controls.Add(this.tb_Title);
            this.Controls.Add(this.tb_Content);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "dia_Prompting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "dia_Prompting";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tb_Content;
        private System.Windows.Forms.TextBox tb_Title;
    }
}