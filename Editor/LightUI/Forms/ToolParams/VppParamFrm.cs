using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain
{
    public partial class VppParamFrm : UIForm
    {
        private Action<ToolSetParamBase> saveCurrentParam;
        ToolSetParamBase toolSetParamBase;
        public VppParamFrm(ToolSetParamBase toolSetParamBase1, Action<ToolSetParamBase> saveCurrentParam1)
        {
            InitializeComponent();
            toolSetParamBase = toolSetParamBase1;
            saveCurrentParam = saveCurrentParam1;

            //  string[] stepEnums = Enum.GetNames(typeof(StepEnum));

            ToolSetParam_VPP toolSetParam_Funtion = toolSetParamBase as ToolSetParam_VPP;
            txtStepName.Text = toolSetParam_Funtion.stepName;
            txtVPP.Text = toolSetParam_Funtion.VPPName;
        }
        public ToolSetParamBase saveParam()
        {
            ToolSetParam_VPP toolSetParam_Funtion = new ToolSetParam_VPP(txtStepName.Text, toolSetParamBase.ObjectId);
            toolSetParam_Funtion.stepName = txtStepName.Text;
            toolSetParam_Funtion.VPPName = txtVPP.Text;
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
