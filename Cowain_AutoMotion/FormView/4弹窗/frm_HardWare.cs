using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Cowain_Machine.Flow;
using Cowain_AutoMotion;
using Cowain_Machine;

namespace Cowain_AutoDispenser.FormView._4弹窗
{
    public partial class frm_HardWare : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frm_HardWare()
        {
            InitializeComponent();
        }

        private void frm_HardWare_Load(object sender, EventArgs e)
        {
            radioRemote.EditValue = ConvertHelper.GetDef_Int(MachineDataDefine.HardwareCfg.Remote);
            radioAxis.EditValue = (int)MachineDataDefine.HardwareCfg.AxisTypeEnum;
          //  radiomaterial.EditValue = (int)MachineDataDefine.HardwareCfg.MaterialTypeEnum;
        }

        private void BtnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MachineDataDefine.HardwareCfg.Remote = ConvertHelper.GetDef_Int(radioRemote.EditValue);
            if (MachineDataDefine.HardwareCfg.Remote == 1)
            {
                MachineDataDefine.machineState.b_UseRemoteQualification = true;
            }
            else
            {
                MachineDataDefine.machineState.b_UseRemoteQualification = false;
            }
            MachineDataDefine.HardwareCfg.AxisTypeEnum = (AxisType)radioAxis.EditValue;
            //MachineDataDefine.HardwareCfg.AxisTypeEnum = AxisType.汇川;
            MachineDataDefine.HardwareCfg.WriteParams(MachineDataDefine.HardwareCfg);
          //  MachineDataDefine.HardwareCfg.MaterialTypeEnum = (MaterialType)radiomaterial.EditValue;
            this.DialogResult = DialogResult.OK;
        }

        private void BtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}