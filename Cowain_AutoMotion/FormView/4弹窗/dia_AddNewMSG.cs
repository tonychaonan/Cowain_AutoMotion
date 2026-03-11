using Cowain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion
{
    public partial class dia_AddNewMSG : Form
    {
        public string MSG = "";
        public dia_AddNewMSG()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            MSG = textBox1.Text;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            MSG = "";
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Contains("\r\n"))
            {
                MSG = textBox1.Text.Trim();
                this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
        public static List<Control> control = new List<Control>(); //Login中所有控件
        private void dia_AddNewMSG_Load(object sender, EventArgs e)
        {
            getControl(this.Controls);
            for (int i = 0; i < control.Count; i++)
            {
                control[i].Text = JudgeLanguage.JudgeLag(control[i].Text);
            }
        }
        private void getControl(Control.ControlCollection etc)
        {

            foreach (Control ct in etc)
            {
                try
                {
                    control.Add(ct);
                }
                catch
                { }

                if (ct.HasChildren)
                {
                    getControl(ct.Controls);
                }
            }
        }

    }
}
