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
using System.IO;
using Cowain_AutoMotion.Flow;
using System.Linq.Expressions;
using static Cowain_AutoMotion.SQLSugarHelper;
using Cowain_AutoMotion;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class dia_Login_New : DevExpress.XtraEditors.XtraForm
    {
        public dia_Login_New()
        {
            InitializeComponent();
        }

        #region 自定义变量
        public clsMachine pMachine;
        //记录上次输入时间，用于判定是否是手工输入
        DateTime lasttime = DateTime.Now;
        #endregion

        #region 自定义方法
        public dia_Login_New(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }
        /// <summary>
        /// 日志存储
        /// </summary>
        /// <param name="message"></param>
        private void savelog(string message)
        {


            try
            {
                string fileName;
                fileName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"));
                string outputPath = @"D:\DATA\登录记录";
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                string fullFileName = Path.Combine(outputPath, fileName);
                System.IO.FileStream fs;
                //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                StreamWriter sw;
                if (!File.Exists(fullFileName))
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "   " + message);
                    sw.Close();
                    fs.Close();

                }
                else
                {
                    fs = new System.IO.FileStream(fullFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "   " + message);
                    sw.Close();
                    fs.Close();
                }

            }
            catch
            {


            }

        }
        #endregion

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            String strFilePath = System.IO.Directory.GetCurrentDirectory();
            strFilePath = strFilePath + "\\Cowain_AutoMotion.exe";
            //-------------------------------
            System.IO.FileInfo fi = new System.IO.FileInfo(strFilePath);
            //---------
            String strTime = fi.LastWriteTime.ToShortDateString();
            //---------

            // GetDataBaseData(strMachinePath, "MachineData", "ItemIndex", "HoldTray_SxSy", "Data1", ref m_tyHoldBasePos.X);
            string strLoginUserName = tbxUserName.Text;

            //if (strLoginUserName == "")  //&& comboBox_UserName.SelectedIndex != 0
            //{
            //    if (comboBox_UserName.SelectedIndex == 0)
            //        strLoginUserName = "User";
            //    else if (comboBox_UserName.SelectedIndex == 1)
            //        strLoginUserName = "Eng";
            //    else if (comboBox_UserName.SelectedIndex == 2)
            //        strLoginUserName = "Cowain";
            //    tbxUserName.Text = strLoginUserName;
            //}


            //string strLoginPassWord = tbxPassword.Text;

            string strPassword = "", strUserEName = "";
            string strMachinePath = pMachine.GetMachineDataPath();
            //-------------------------------------
            bool bGetPassword = pMachine.GetDataBaseData(strMachinePath, "PWD", "UserName", strLoginUserName, "thePassword", ref strPassword);
            bool bGetName = pMachine.GetDataBaseData(strMachinePath, "PWD", "UserName", strLoginUserName, "EName", ref strUserEName);
            //-------------------------------------
            strPassword = strPassword.Trim();
            strUserEName = strUserEName.Trim();
            strUserEName = "Maker";
            //if (bGetPassword && bGetName /*&& strPassword == strLoginPassWord*/)
            //{
                if (strUserEName == "Maker")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Maker;
                else if (strUserEName == "MacEng")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.MacEng;
                else if (strUserEName == "ItEng")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.ItEng;
                else if (strUserEName == "OpEng"|| strUserEName == "Eng")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Eng;
                else if (strUserEName == "Op")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Operator;
                savelog("Name:" + strLoginUserName + "    Level:" + strUserEName + "      Release Ver: " + MESDataDefine.MESLXData.SW_Version);

            //}
            //else if (strLoginPassWord == "1") //Cowain Maker
            //{
            //    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Maker;
            //    savelog("Cowain Maker Login" + "-------Time:" + strTime);
            //}
            //else
            //{
            //        //this.Close();
            //        MsgBoxHelper.DxMsgShowErr("登录失败！");
            //        pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.UnLogin;
            //    }
                MachineDataDefine.m_LoginUser = pMachine.m_LoginUser;
            MachineDataDefine.m_LoginUserName = strLoginUserName.Trim();
            MachineDataDefine.m_LoginCardID = txtCardID.Text;

            //this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void dia_Login_New_Shown(object sender, EventArgs e)
        {
            //comboBox_UserName.SelectedIndex = 0;
            //if (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng ||
            //   pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker)
            //{
            //    button2.Visible = true;
            //    this.Height = 215;
            //    //comboBox_Level.SelectedIndex = 0;
            //    group_Password.Visible = true;
            //}
            //else
            //{
            //    button2.Visible = false;

            //    this.Height = 215;
            //    group_Password.Visible = false;
            //}
        }

        private void btn_Canel_Click(object sender, EventArgs e)
        {
            pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.UnLogin;
            MachineDataDefine.m_LoginUser = pMachine.m_LoginUser;
            MachineDataDefine.m_LoginUserName = "";
            MachineDataDefine.m_LoginCardID = "";
        }

        
        private void txtCardID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string cardid = txtCardID.Text.Trim();
                if (cardid.Equals(""))
                {
                    return;
                }
                //将关联的ID卡号和工号显示出来
                Expression<Func<PWD, bool>> cd = null;
                var listMotors = DBContext<PWD>.GetInstance().GetList(cd = f => f.CardID == cardid);
                if (listMotors.Count > 1)
                {
                    MsgBoxHelper.DxMsgShowErr("已存在用户ID卡号！");
                    return;
                }

                if (listMotors.Count == 0)
                {
                    MsgBoxHelper.DxMsgShowErr("未找到相关用户ID卡信息！");
                    return;
                }

                txtUserID.Text = listMotors[0].UserID;
                tbxUserName.Text = listMotors[0].UserName;

                //tbxPassword.Focus();
                //tbxPassword.SelectAll();
                btn_Ok.PerformClick();
            }
            else
            {
                DateTime curtime = DateTime.Now;
                TimeSpan span = curtime - lasttime;
                if (span.TotalMilliseconds > 100)
                {
                    txtCardID.Text = "";
                }

                lasttime = curtime;
            }            
        }

        private void tbxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btn_Ok.PerformClick();
            }
        }
    }
}