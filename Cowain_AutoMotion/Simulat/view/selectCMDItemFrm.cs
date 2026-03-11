using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.Simulat
{
    public partial class selectCMDItemFrm : Form
    {
        StepEnum stepEnum1 = StepEnum.条件;
        SimulatParamItem1 simulatParamItem1;
        public selectCMDItemFrm(SimulatParamItem1 simulatParamItem11)
        {
            InitializeComponent();
            simulatParamItem1 = new SimulatParamItem1(simulatParamItem11.stepEnum, simulatParamItem11.instanceStr, simulatParamItem11.stepStr, simulatParamItem11.valueStr);
            //<条件>EnumParam_InputIO._001夹爪托盘感应器1==1
            cBoxType.Text = simulatParamItem11.stepEnum.ToString();
            Enum.TryParse(cBoxType.Text, out stepEnum1);
            showType(stepEnum1);
        }
        private void showType(StepEnum stepEnum)
        {
            lbMSG.Text = "";
            if (stepEnum1 == StepEnum.条件)
            {
                //
                cBoxCMD.Items.Clear();
                string[] names = Enum.GetNames(typeof(IfStep));
                for (int i = 0; i < names.Length; i++)
                {
                    cBoxCMD.Items.Add(names[i]);
                }
            }
            else
            {
                //
                cBoxCMD.Items.Clear();
                string[] names = Enum.GetNames(typeof(ExecuteStep));
                for (int i = 0; i < names.Length; i++)
                {
                    cBoxCMD.Items.Add(names[i]);
                }
            }
            //
            cBoxInstance.Items.Clear();
            cBoxInstance.Items.Add("无");
            string[] names1 = Enum.GetNames(typeof(EnumParam_InputIO));
            for (int i = 0; i < names1.Length; i++)
            {
                cBoxInstance.Items.Add(names1[i]);
            }
            string[] names2 = Enum.GetNames(typeof(EnumParam_ConnectionName));
            for (int i = 0; i < names2.Length; i++)
            {
                cBoxInstance.Items.Add(names2[i]);
            }
            if (stepEnum == simulatParamItem1.stepEnum)
            {
                cBoxCMD.Text = simulatParamItem1.stepStr;
                cBoxInstance.Text = simulatParamItem1.instanceStr;
                txtValue.Text = simulatParamItem1.valueStr;
            }
        }
        private void selectCMDItemFrm_Load(object sender, EventArgs e)
        {

        }

        private void cBoxCMD_SelectedIndexChanged(object sender, EventArgs e)
        {
            showChange();
        }
        private void cBoxInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            showChange();
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            showChange();
        }
        private void showChange()
        {
            string type = "";
            string cmd = "";
            string instance = "";
            string value = "";
            if (cBoxType.Text == "")
            {
                type = "";
            }
            else
            {
                type = cBoxType.Text;
            }
            if (cBoxCMD.Text == "")
            {
                cmd = "Null";
            }
            else
            {
                cmd = cBoxCMD.Text;
            }
            if (cBoxInstance.Text == "")
            {
                instance = "Null";
            }
            else
            {
                instance = cBoxInstance.Text;
            }
            if (txtValue.Text == "")
            {
                value = "Null";
            }
            else
            {
                value = txtValue.Text;
            }
            string str1 = "==";
            if (type == StepEnum.步骤.ToString())
            {
                str1 = "=";
            }
            string str = "<" + type + ">" + "<" + cmd + ">"
                      + "<" + instance + ">" + "<" + value + ">";
            lbMSG.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cBoxType.Text != "")
            {
                Enum.TryParse(cBoxType.Text, out stepEnum1);
                simulatParamItem1.stepEnum = stepEnum1;
                simulatParamItem1.stepStr = cBoxCMD.Text;
                simulatParamItem1.instanceStr = cBoxInstance.Text;
                simulatParamItem1.valueStr = txtValue.Text;
            }
            this.Close();
        }
        public string setValue(ref SimulatParamItem1 simulatParamItem2)
        {
            simulatParamItem2.stepEnum = simulatParamItem1.stepEnum;
            simulatParamItem2.stepStr = simulatParamItem1.stepStr;
            simulatParamItem2.instanceStr = simulatParamItem1.instanceStr;
            simulatParamItem2.valueStr = simulatParamItem1.valueStr;
            string str = "<" + simulatParamItem2.stepEnum + ">" + "<" + simulatParamItem2.stepStr + ">"
                      + "<" + simulatParamItem2.instanceStr + ">" + "<" + simulatParamItem2.valueStr + ">";
            return str;
        }
        private void cBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBoxType.SelectedIndex == 0)
            {
                stepEnum1 = StepEnum.条件;
            }
            else
            {
                stepEnum1 = StepEnum.步骤;
            }
            showType(stepEnum1);
            showChange();
        }
    }
}
