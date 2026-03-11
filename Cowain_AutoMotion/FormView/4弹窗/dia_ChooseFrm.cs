using Cowain_Machine;
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
    public partial class dia_ChooseFrm : DevExpress.XtraEditors.XtraForm
    {
        //AxisTakeIn ali;
        public dia_ChooseFrm(AxisTakeIn agm)
        {
            InitializeComponent();
            ali = agm;
        }

        private void dia_ChooseFrm_Load(object sender, EventArgs e)
        {
            if(MachineDataDefine.LADModel==1)
            {
                labelControl1.Text = "32x units have been tested under CPK mode.";
            }
            else if(MachineDataDefine.LADModel == 2)
            {
                labelControl1.Text = "10x3 test completed for Operator "+ MachineDataDefine.LADOPID.ToString();
            }
            else
            {
                labelControl1.Text = "10x units have been tested under SCS mode.";
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if (MachineDataDefine.LADModel == 1)
            {
                ali.productCount = 0;
            }
            else if (MachineDataDefine.LADModel == 2)
            {
                if (ali.productCount == 30)
                {
                    ali.productCount = 0;
                    MachineDataDefine.LADOPID = 1;
                }
                else if (ali.productCount == 60)
                {
                    ali.productCount = 30;
                    MachineDataDefine.LADOPID = 2;
                }
                else if (ali.productCount == 90)
                {
                    ali.productCount = 60;
                    MachineDataDefine.LADOPID = 3;
                }
                else if (MachineDataDefine.LADModel == 3)
                {
                    ali.productCount = 0;
                }
            }
                this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            if (MachineDataDefine.LADModel == 1)
            {
                MachineDataDefine.b_UseLAD = false;
            }
            else if (MachineDataDefine.LADModel == 2)
            {
                if (ali.productCount == 30)
                {
                    MachineDataDefine.LADOPID = 2;
                    MachineDataDefine.downLad = true;
                }
                else if (ali.productCount == 60)
                {
                    MachineDataDefine.LADOPID = 3;
                    MachineDataDefine.downLad = true;
                }
                else if (ali.productCount == 90)
                {
                    MachineDataDefine.b_UseLAD = false;
                    MachineDataDefine.downLad = true;
                    MachineDataDefine.LADOPID = 1;
                    ali.productCount = 0;

                }
            }
            else if (MachineDataDefine.LADModel == 3)
            {
                MachineDataDefine.b_UseLAD = false;
            }
            this.Close();
        }
    }
}
