using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.FormView._4弹窗
{
    public partial class dia_Prompting : Form
    {

        public dia_Prompting(string titleText,string contentText)
        {
            InitializeComponent();
            this.tb_Title.Text = titleText;
            this.tb_Content.Text = contentText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
