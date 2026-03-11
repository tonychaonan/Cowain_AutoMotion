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
    public partial class ShowInfo : Form
    {
        public ShowInfo()
        {
            InitializeComponent();

        }
        public ShowInfo(int type, string typeMSG, string errMSG) : this()
        {
            InitializeComponent();
            label1.Text = typeMSG;
            if (type == 0)
            {
                label1.ForeColor = Color.Green;
            }
            else
            {
                label1.ForeColor = Color.Red;
            }
            label2.Text = errMSG;
        }
        public void setText(int type, string typeMSG, string errMSG)
        {
            label1.Text = typeMSG;
            if (type == 0)
            {
                label1.ForeColor = Color.Green;
            }
            else
            {
                label1.ForeColor = Color.Red;
            }
            label2.Text = errMSG;
        }

        private static ShowInfo showinfo = new ShowInfo(1, "自动排胶中警告", "安全门未关闭，请关闭安全门或者屏蔽！\r\n屏蔽后还会自动排胶！！！");
        public void showInfo(int type, string typeMSG, string errMSG)
        {
            setText(type, typeMSG, errMSG);
            this.Show();
        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
