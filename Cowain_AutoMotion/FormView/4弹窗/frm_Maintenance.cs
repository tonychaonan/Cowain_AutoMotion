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
using Cowain_AutoMotion;
using Cowain_Machine.Flow;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class frm_Maintenance : DevExpress.XtraEditors.XtraForm
    {
        public frm_Maintenance(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }

        #region 自定义变量
        public clsMachine pMachine;

        public enum enPlanMode
        {
            日常点检 = 0,
            更换AB胶水,
            更换HM胶水,
            更换针头,
            更换胶阀,
            压力测试,
            镭射标定,
            设备耗材更换,
            MaterialReplacement,
            周点检,
            胶水称重,
            LAD,
            其它,
            original
        }

        private enPlanMode m_CurMode = enPlanMode.original;
        /// <summary>
        /// 计划停机模式
        /// </summary>
        public enPlanMode CurMode
        {
            get
            {
                return m_CurMode;
            }

            set
            {
                m_CurMode = value;
            }
        }
        #endregion
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (xtraTabPageType.PageVisible)
            {
                if (m_CurMode == enPlanMode.original)
                {
                    MsgBoxHelper.DxMsgShowErr("请选择计划停机类型！");
                    return;
                }
                frm_PlanDoubleConfirm doubleConfm = null;
                if (radioButton11.Checked != true)
                {
                    doubleConfm = new frm_PlanDoubleConfirm(ref pMachine);
                    switch (m_CurMode)
                    {
                        case enPlanMode.日常点检:
                        case enPlanMode.更换针头:
                        case enPlanMode.更换胶阀:
                        case enPlanMode.设备耗材更换:
                        case enPlanMode.镭射标定:
                        case enPlanMode.MaterialReplacement:
                        case enPlanMode.压力测试:
                        case enPlanMode.周点检:
                        case enPlanMode.胶水称重:
                        case enPlanMode.其它:
                            //    new frm_PlanDoubleConfirm(m_CurMode.ToString(),false);
                            doubleConfm.NeedSN = false;
                            doubleConfm.PlanType = m_CurMode.ToString();
                            break;
                        case enPlanMode.更换AB胶水:
                        case enPlanMode.更换HM胶水:
                            //new frm_PlanDoubleConfirm(m_CurMode.ToString(),true);
                            doubleConfm.NeedSN = true;
                            doubleConfm.PlanType = m_CurMode.ToString();
                            break;
                    }
                    if (doubleConfm.ShowDialog() == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    MachineDataDefine.b_UseLAD = true;
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                MsgBoxHelper.DxMsgShowInfo("请选择龙门类型！");
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtn = (RadioButton)sender;
            if (rbtn.Tag != null)
            {
                switch (rbtn.Tag.ToString())
                {
                    case "001":
                        m_CurMode = enPlanMode.日常点检;
                        break;
                    case "002":
                        m_CurMode = enPlanMode.更换AB胶水;
                        break;
                    case "003":
                        m_CurMode = enPlanMode.更换HM胶水;
                        break;
                    case "004":
                        m_CurMode = enPlanMode.更换针头;
                        break;
                    case "005":
                        m_CurMode = enPlanMode.更换胶阀;
                        break;
                    case "006":
                        m_CurMode = enPlanMode.设备耗材更换;
                        break;
                    case "007":
                        m_CurMode = enPlanMode.其它;
                        break;
                    case "008":
                        m_CurMode = enPlanMode.镭射标定;
                        break;
                    case "009":
                        m_CurMode = enPlanMode.周点检;
                        break;
                    case "010":
                        m_CurMode = enPlanMode.胶水称重;
                        break;
                    case "011":
                        m_CurMode = enPlanMode.LAD;
                        break;
                    default:
                        m_CurMode = enPlanMode.original;
                        break;
                }

            }
        }
        private void frm_Maintenance_Load(object sender, EventArgs e)
        {
        }

        private void radioButton11_Click(object sender, EventArgs e)
        {
            dia_LADForm lad = new Cowain_AutoMotion.dia_LADForm();
            DialogResult dialogResult = lad.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                m_CurMode = enPlanMode.LAD;
            }
            else
            {
                radioButton11.Checked = false;
            }
        }
    }
}