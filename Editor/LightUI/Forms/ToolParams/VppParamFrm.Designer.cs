namespace Cowain
{
    partial class VppParamFrm
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
            this.uiButton2 = new Sunny.UI.UIButton();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.uiPanel3 = new Sunny.UI.UIPanel();
            this.txtStepName = new Sunny.UI.UITextBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiAvatar3 = new Sunny.UI.UIAvatar();
            this.txtVPP = new Sunny.UI.UITextBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiPanel3.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Location = new System.Drawing.Point(168, 232);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Radius = 10;
            this.uiButton2.Size = new System.Drawing.Size(107, 45);
            this.uiButton2.TabIndex = 15;
            this.uiButton2.Text = "取消";
            this.uiButton2.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(346, 232);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Radius = 10;
            this.uiButton1.Size = new System.Drawing.Size(107, 45);
            this.uiButton1.TabIndex = 14;
            this.uiButton1.Text = "保存";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // uiPanel3
            // 
            this.uiPanel3.Controls.Add(this.txtStepName);
            this.uiPanel3.Controls.Add(this.uiLabel1);
            this.uiPanel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel3.Location = new System.Drawing.Point(41, 50);
            this.uiPanel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel3.Name = "uiPanel3";
            this.uiPanel3.RectSize = 2;
            this.uiPanel3.Size = new System.Drawing.Size(554, 79);
            this.uiPanel3.TabIndex = 21;
            this.uiPanel3.Text = null;
            this.uiPanel3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtStepName
            // 
            this.txtStepName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtStepName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStepName.Location = new System.Drawing.Point(188, 25);
            this.txtStepName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtStepName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtStepName.Name = "txtStepName";
            this.txtStepName.Padding = new System.Windows.Forms.Padding(5);
            this.txtStepName.ShowText = false;
            this.txtStepName.Size = new System.Drawing.Size(220, 29);
            this.txtStepName.TabIndex = 1;
            this.txtStepName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtStepName.Watermark = "";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(90, 25);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "步骤名称：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiAvatar3);
            this.uiPanel1.Controls.Add(this.txtVPP);
            this.uiPanel1.Controls.Add(this.uiLabel2);
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(41, 139);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.RectSize = 2;
            this.uiPanel1.Size = new System.Drawing.Size(554, 79);
            this.uiPanel1.TabIndex = 22;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiAvatar3
            // 
            this.uiAvatar3.AvatarSize = 20;
            this.uiAvatar3.FillColor = System.Drawing.Color.Transparent;
            this.uiAvatar3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiAvatar3.Location = new System.Drawing.Point(421, 17);
            this.uiAvatar3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiAvatar3.Name = "uiAvatar3";
            this.uiAvatar3.Shape = Sunny.UI.UIShape.Square;
            this.uiAvatar3.Size = new System.Drawing.Size(38, 43);
            this.uiAvatar3.Symbol = 361638;
            this.uiAvatar3.TabIndex = 22;
            this.uiAvatar3.Text = "uiAvatar3";
            // 
            // txtVPP
            // 
            this.txtVPP.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVPP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVPP.Location = new System.Drawing.Point(188, 25);
            this.txtVPP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtVPP.MinimumSize = new System.Drawing.Size(1, 16);
            this.txtVPP.Name = "txtVPP";
            this.txtVPP.Padding = new System.Windows.Forms.Padding(5);
            this.txtVPP.ShowText = false;
            this.txtVPP.Size = new System.Drawing.Size(220, 29);
            this.txtVPP.TabIndex = 1;
            this.txtVPP.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtVPP.Watermark = "";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(134, 27);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiLabel2.TabIndex = 0;
            this.uiLabel2.Text = "VPP：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VppParam
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(641, 294);
            this.ControlBox = false;
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.uiPanel3);
            this.Controls.Add(this.uiButton2);
            this.Controls.Add(this.uiButton1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VppParam";
            this.Text = "VppParam";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.uiPanel3.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIPanel uiPanel3;
        private Sunny.UI.UITextBox txtStepName;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UITextBox txtVPP;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UIAvatar uiAvatar3;
    }
}