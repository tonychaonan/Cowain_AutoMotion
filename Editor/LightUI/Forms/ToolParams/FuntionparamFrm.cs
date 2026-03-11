using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain
{
    public partial class FuntionparamFrm : UIForm
    {
        private Action<ToolSetParamBase> saveCurrentParam;
        ToolSetParamBase toolSetParamBase;
        public FuntionparamFrm(ToolSetParamBase toolSetParamBase1, Action<ToolSetParamBase> saveCurrentParam1)
        {
            toolSetParamBase = toolSetParamBase1;
            saveCurrentParam = saveCurrentParam1;
            InitializeComponent();
            txtStepName.Text = toolSetParamBase1.stepName;
            string[] actionEnums = Enum.GetNames(typeof(ActionEnum));
            cBoxAction.Items.AddRange(actionEnums);

          //  string[] stepEnums = Enum.GetNames(typeof(StepEnum));
            ToolSetParam_Funtion toolSetParam_Funtion = toolSetParamBase as ToolSetParam_Funtion;
            txtEventParam.Text = toolSetParam_Funtion.eventParam;
            txtEventName.Text = toolSetParam_Funtion.eventName;
            cBoxAction.Text = toolSetParam_Funtion.action;
            txtJumpParam.Text = toolSetParam_Funtion.jumpParam;
            txtJumpStep.Text = toolSetParam_Funtion.jumpStep;
        }
        public ToolSetParamBase saveParam()
        {
            ToolSetParam_Funtion toolSetParam_Funtion = new ToolSetParam_Funtion(txtStepName.Text, toolSetParamBase.ObjectId);
            toolSetParam_Funtion.eventParam = txtEventParam.Text;
            toolSetParam_Funtion.eventName = txtEventName.Text;
            toolSetParam_Funtion.action = cBoxAction.Text;
            toolSetParam_Funtion.jumpParam = txtJumpParam.Text;
            toolSetParam_Funtion.jumpStep = txtJumpStep.Text;
            return toolSetParam_Funtion;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ToolSetParamBase toolSetParamBase = saveParam();
            saveCurrentParam.Invoke(toolSetParamBase);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
