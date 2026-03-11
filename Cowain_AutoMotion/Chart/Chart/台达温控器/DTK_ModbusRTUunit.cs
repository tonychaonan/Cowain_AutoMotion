using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chart
{
    public partial class East_Win_ModbusRTUunit : UserControl
    {

        private DTK_ModbusRTU Nordson232cs;
        public bool IsNorDsonState
        {
            get
            {
                return label20.Text == "通讯正常";
            }
            set
            {
                if (b_NOUse)
                {
                    return;
                }

                if (label20.InvokeRequired)
                {
                    label20.BeginInvoke(new Action<bool>((s) =>
                    {
                        label20.Text = s ? "通讯正常" : "通讯失败";
                        label20.BackColor = s ? Color.Lime : Color.Red;
                        
                    }), value);
                }
                else
                {
                    label20.Text = value ? "通讯正常" : "通讯失败";
                    label20.BackColor = value ? Color.Lime : Color.Red;
                }
            }
        }
        /// <summary>
        /// 是否屏蔽通讯
        /// </summary>
        bool b_NOUse = false;
        public void setState(bool b_NOUse1)
        {
            b_NOUse = b_NOUse1;
            if (b_NOUse1)
            {
                if (label20.InvokeRequired)
                {
                    label20.BeginInvoke(new Action(() =>
                    {
                        label20.Text = "通讯未启用";
                        label20.BackColor = Color.Gray;
                    }));
                }
                else
                {
                    label20.Text = "通讯未启用";
                    label20.BackColor = Color.Gray;
                }
            }
        }
        private string presure;
        public string Presure
        {
            get
            {
                return presure;
            }
            set
            {
                if (label5.InvokeRequired)
                {
                    label5.BeginInvoke(new Action(() =>
                    {
                        label5.Text = value + "bar";
                    }));
                }
                else
                {
                    label5.Text = value + "bar";

                }
            }
        }

        private string t1;
        public string T1
        {
            get
            {
                return t1;
            }
            set
            {
                if (label6.InvokeRequired)
                {
                    label6.BeginInvoke(new Action(() =>
                    {
                        label6.Text = value+"℃";
                    }));
                }
                else
                {
                    label6.Text = value + "℃";

                }
            }
        }


        private string t2;
        public string T2
        {
            get
            {
                return t2;
            }
            set
            {
                if (label7.InvokeRequired)
                {
                    label7.BeginInvoke(new Action(() =>
                    {
                        label7.Text = value + "℃";
                    }));
                }
                else
                {
                    label7.Text = value + "℃";

                }
            }
        }
        public East_Win_ModbusRTUunit(DTK_ModbusRTU Nordson232cs)
        {
            InitializeComponent();
            this.Nordson232cs = Nordson232cs;
        }
        //public NorDson232unit(ModbusRTU Nordson232cs)
        //{
        //    InitializeComponent();
        //    this.Nordson232cs = Nordson232cs;
        //}
        private void label20_DoubleClick(object sender, EventArgs e)
        {
            if (IsNorDsonState)
                return;
            DialogResult result = MessageBox.Show("是否重新连接", "提示", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                Nordson232cs.Recont();
            }
        }

       
    }
}
