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
using MotionBase;
using Cowain_AutoDispenser;
using Cowain_Machine;
using Cowain_AutoMotion;
using Cowain;

namespace Cowain_Form.FormView
{
    public partial class frm_HealthWarm : DevExpress.XtraEditors.XtraForm
    {
        public frm_HealthWarm()
        {
            InitializeComponent();
        }

        public frm_HealthWarm(ref clsMachine pm,string ngmsg)
        {
            InitializeComponent();
            pMachine = pm;
            ng_msg = ngmsg;
        }

        #region 自定义变量
        clsMachine pMachine;
        /// <summary>
        /// 龙门类型
        /// </summary>
        private int m_Gantry = -1;
        /// <summary>
        /// 健康度剩余时间
        /// </summary>
        private decimal m_Hour = -1;
        /// <summary>
        /// ng下料信息
        /// </summary>
        private string ng_msg = "";
        #endregion
        public static List<Control> control = new List<Control>(); //Login中所有控件
        private void frm_HealthWarm_Load(object sender, EventArgs e)
        {
            labSTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");   
            labDetail.Text = string.Format("{0}", ng_msg);
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
        private void btn_静音_Click(object sender, EventArgs e)
        {
            if (!MachineDataDefine.machineState.b_Usehummer)
            {
            clsMSignalTower.NoUse_m_bBuzzer = true;
            pMachine.m_SgTower.SetBuzzerOff();
             }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            #region 权限检查
            if ((int)pMachine.m_LoginUser <= 1)
            {
                if (!MachineDataDefine.machineState.b_UseMesLogin)
                {
                    dia_Login_New m_LoginDlg = new dia_Login_New(ref pMachine);
                    if (m_LoginDlg.ShowDialog() == DialogResult.OK)
                    {
                        if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.m_LoginUserName.Trim().Equals(""))
                        {
                            MsgBoxHelper.DxMsgShowErr("登录失败！");
                            return;
                        }
                        else if ((int)pMachine.m_LoginUser <= 1)
                        {
                            MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
                            return;
                        }
                        else
                        {
                            //labUser.Text = MachineDataDefine.m_LoginUserName.Trim();
                            pMachine.NeedRef = true;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    dia_Login_Remote m_LoginDlg = new dia_Login_Remote(ref pMachine);
                    if (m_LoginDlg.ShowDialog() == DialogResult.OK)
                    {
                        if (pMachine.m_LoginUser == Sys_Define.enPasswordType.UnLogin || MachineDataDefine.m_LoginUserName.Trim().Equals(""))
                        {
                            MsgBoxHelper.DxMsgShowErr("登录失败！");
                            return;
                        }
                        else if ((int)pMachine.m_LoginUser <= 1)
                        {
                            MsgBoxHelper.DxMsgShowErr("权限不够，请登录Level2或Level3用户！");
                            return;
                        }
                        else
                        {
                            //labUser.Text = MachineDataDefine.m_LoginUserName.Trim();
                            pMachine.NeedRef = true;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                //------------

            }
            #endregion
            if (!MachineDataDefine.machineState.b_Usehummer)
            {
                clsMSignalTower.NoUse_m_bBuzzer = true;
                pMachine.m_SgTower.SetBuzzerOff();
            }
            if(MachineDataDefine.RobotDownError == true)
            {
                MachineDataDefine.RobotDownForm = false;
            }
            this.Close();
        }
    }
}