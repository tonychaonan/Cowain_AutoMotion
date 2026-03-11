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
using Cowain_AutoMotion.Flow;
using Cowain_Machine.Flow;
using Cowain;
using System.Threading;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class frm_PlanDoubleConfirm : DevExpress.XtraEditors.XtraForm
    {
        #region 自定义变量
        
        private string m_PlanType = string.Empty;
        /// <summary>
        /// 计划停机类型
        /// </summary>
        public string PlanType
        {
            get
            {
                return m_PlanType;
            }

            set
            {
                labType.Text = value;
                m_PlanType = value;
            }
        }

        private bool m_NeedSN = false;
        /// <summary>
        /// 是否需要输入SN
        /// </summary>
        public bool NeedSN
        {
            get
            {
                return m_NeedSN;
            }

            set
            {
                labold.Visible = value;
                labnew.Visible = value;
                txt_Old.Visible = value;
                txt_New.Visible = value;
                btnnew.Visible = value;
                m_NeedSN = value;
            }
        }

        public clsMachine pMachine;
        #endregion

        public frm_PlanDoubleConfirm(ref clsMachine pM)
        {
            InitializeComponent();

            pMachine = pM;
        }


        public frm_PlanDoubleConfirm(string plantype,bool needsn)
        {
            InitializeComponent();

            m_PlanType = plantype;
            m_NeedSN = needsn;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            //if (txt_Op.Text.Trim() == "" && txt_Op.Text.Trim().Length < 8)
            //{
            //    MsgBoxHelper.DxMsgShowErr("卡号异常！");
            //    return;
            //}

            //if (m_NeedSN)
            //{
            //    if (txt_Old.Text.Trim() == "")
            //    {
            //        MsgBoxHelper.DxMsgShowErr("请输入旧SN！");
            //        return;
            //    }

            //    if (txt_New.Text.Trim() == "")
            //    {
            //        MsgBoxHelper.DxMsgShowErr("请输入新SN！");
            //        return;
            //    }

            //    if (txt_Old.Text == txt_New.Text)
            //    {
            //        MsgBoxHelper.DxMsgShowErr("新旧SN不能相同！");
            //        return;
            //    }
            //}

            //MachineDataDefine.m_CardID = txt_Op.Text.Trim();
            //if (m_NeedSN)
            //{
            //    MachineDataDefine.m_OldSN = txt_Old.Text.Trim();
            //    MachineDataDefine.m_NewSN = txt_New.Text.Trim();
            //}
            //else
            //{
            //    MachineDataDefine.m_OldSN = "";
            //    MachineDataDefine.m_NewSN = "";
            //}

            this.DialogResult = DialogResult.OK;            
        }

        //记录上次输入时间，用于判定是否是手工输入
        DateTime lasttime = DateTime.Now;

        private void txt_Op_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && txt_Op.Text.Length >= 10 && !m_NeedSN)
            {
                btn_OK.PerformClick();
            }
            else
            {
                //DateTime curtime = DateTime.Now;
                //TimeSpan span = curtime - lasttime;
                //if (span.TotalMilliseconds > 50)
                //{
                //    txt_Op.Text = "";
                //}

                //lasttime = curtime;
            }
            
        }

        private void txt_Old_KeyPress(object sender, KeyPressEventArgs e)
        {
            DateTime curtime = DateTime.Now;
            TimeSpan span = curtime - lasttime;
            if (span.TotalMilliseconds > 50)
            {
                txt_Old.Text = "";
            }

            lasttime = curtime;
        }

        private void txt_New_KeyPress(object sender, KeyPressEventArgs e)
        {
            DateTime curtime = DateTime.Now;
            TimeSpan span = curtime - lasttime;
            if (span.TotalMilliseconds > 50)
            {
                txt_New.Text = "";
            }

            lasttime = curtime;
        }

        private void txt_Old_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
           
        }

        private void txt_New_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            
        }

        private void txt_Op_KeyDown(object sender, KeyEventArgs e)
        {
            timerop.Enabled = true;
        }

        private void timerop_Tick(object sender, EventArgs e)
        {
            timerop.Enabled = false;
            if (txt_Op.Text.Length != 10)
            {
                txt_Op.Text = "";
            }
        }

        private void btnnew_Click(object sender, EventArgs e)
        {
           
        }

        private void txt_New_KeyDown(object sender, KeyEventArgs e)
        {
            timerSN.Enabled = true;
        }

        private void timerSN_Tick(object sender, EventArgs e)
        {
            timerSN.Enabled = false;
            if (txt_New.Text.Length != 41)
            {
                txt_New.Text = "";
            }
        }

        MESLXData mESLXData = new MESLXData();
        private void frm_PlanDoubleConfirm_Load(object sender, EventArgs e)
        {
           
        }
    }
}