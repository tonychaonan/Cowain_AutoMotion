namespace Cowain_Form.FormView
{
    partial class frm_TeachIOView
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.listView_Output = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView_Input = new System.Windows.Forms.ListView();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listView_Output
            // 
            this.listView_Output.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Output.HideSelection = false;
            this.listView_Output.Location = new System.Drawing.Point(575, 82);
            this.listView_Output.Name = "listView_Output";
            this.listView_Output.Size = new System.Drawing.Size(500, 367);
            this.listView_Output.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView_Output.TabIndex = 24;
            this.listView_Output.TileSize = new System.Drawing.Size(308, 22);
            this.listView_Output.UseCompatibleStateImageBehavior = false;
            this.listView_Output.View = System.Windows.Forms.View.Tile;
            this.listView_Output.DoubleClick += new System.EventHandler(this.listView_Output_MouseDoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 50);
            this.tabControl1.Location = new System.Drawing.Point(29, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(849, 53);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 23;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 54);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(841, 0);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "All";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView_Input
            // 
            this.listView_Input.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listView_Input.HideSelection = false;
            this.listView_Input.Location = new System.Drawing.Point(28, 82);
            this.listView_Input.Name = "listView_Input";
            this.listView_Input.Size = new System.Drawing.Size(500, 367);
            this.listView_Input.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView_Input.TabIndex = 22;
            this.listView_Input.TileSize = new System.Drawing.Size(308, 22);
            this.listView_Input.UseCompatibleStateImageBehavior = false;
            this.listView_Input.View = System.Windows.Forms.View.Tile;
            // 
            // frm_TeachIOView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 460);
            this.Controls.Add(this.listView_Output);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listView_Input);
            this.Font = new System.Drawing.Font("新細明體-ExtB", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_TeachIOView";
            this.Text = "frm_IOView";
            this.Shown += new System.EventHandler(this.frm_IOView_Shown);
            this.VisibleChanged += new System.EventHandler(this.frm_IOView_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListView listView_Output;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView listView_Input;
    }
}