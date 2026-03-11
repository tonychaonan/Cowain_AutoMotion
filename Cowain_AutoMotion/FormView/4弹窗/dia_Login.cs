using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cowain_Machine.Flow;
using System.IO;
using Cowain;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class dia_Login : Form
    {
        public dia_Login(ref clsMachine pM)
        {
            InitializeComponent();
            comboBox_UserName.Items.Clear();
            comboBox_UserName.Items.Add(JudgeLanguage.JudgeLag("操作员"));
            comboBox_UserName.Items.Add(JudgeLanguage.JudgeLag("工程师"));
            comboBox_UserName.Items.Add(JudgeLanguage.JudgeLag("制造商"));
            pMachine = pM;
        }

        public clsMachine pMachine;



        private void btn_Canel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
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

            if (strLoginUserName == "")  //&& comboBox_UserName.SelectedIndex != 0
            {
                if (comboBox_UserName.SelectedIndex == 0)
                    strLoginUserName = "User";
                else if (comboBox_UserName.SelectedIndex == 1)
                    strLoginUserName = "Eng";
                else if (comboBox_UserName.SelectedIndex == 2)
                    strLoginUserName = "Cowain";
                tbxUserName.Text = strLoginUserName;
            }


            string strLoginPassWord = tbxPassword.Text;

            string strPassword = "", strUserEName = "";
            string strMachinePath = pMachine.GetMachineDataPath();
            //-------------------------------------
            bool bGetPassword = pMachine.GetDataBaseData(strMachinePath, "PWD", "UserName", strLoginUserName, "thePassword", ref strPassword);
            bool bGetName = pMachine.GetDataBaseData(strMachinePath, "PWD", "UserName", strLoginUserName, "EName", ref strUserEName);
            //-------------------------------------
            strPassword = strPassword.Trim();
            strUserEName = strUserEName.Trim();

            if (bGetPassword && bGetName && strPassword == strLoginPassWord)
            {
                if (strUserEName == "Maker")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Maker;
                else if (strUserEName == "MacEng")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.MacEng;
                else if (strUserEName == "ItEng")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.ItEng;
                else if (strUserEName == "OpEng")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Eng;
                else if (strUserEName == "Op")
                    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Operator;
                savelog("Name:" + strLoginUserName + "    Level:" + strUserEName + "      Release Ver: " + MESDataDefine.MESLXData.SW_Version);
            }
            //else if (strLoginPassWord == "1") //Cowain Maker
            //{
            //    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Maker;
            //    savelog("Cowain Maker Login" + "-------Time:" + strTime);
            //}
            else
            {
                pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.UnLogin;
            }
            MachineDataDefine.m_LoginUser = pMachine.m_LoginUser;
            this.Close();
        }

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


        private void dia_Login_Shown(object sender, EventArgs e)
        {
            comboBox_UserName.SelectedIndex = 0;
            if (pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.MacEng ||
               pMachine.m_LoginUser == MotionBase.Sys_Define.enPasswordType.Maker)
            {
                button2.Visible = true;
                this.Height = 215;
                //comboBox_Level.SelectedIndex = 0;
                group_Password.Visible = true;
            }
            else
            {
                button2.Visible = false;

                this.Height = 215;
                group_Password.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.Height == 215)
            {
                group_Password.Visible = true;
                this.Height = 368;
            }
            else
            {
                group_Password.Visible = false;
                this.Height = 215;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string strLoginUserName = tx_UserName.Text;
            string strLoginPassWord = tx_OldPassword.Text;
            string strNewPassword = tx_newPassword.Text;

            string strPassword = "", strUserEName = "";
            string strMachinePath = pMachine.GetMachineDataPath();
            //-------------------------------------
            bool bGetPassword = pMachine.GetDataBaseData(strMachinePath, "PWD", "UserName", strLoginUserName, "thePassword", ref strPassword);
            bool bGetName = pMachine.GetDataBaseData(strMachinePath, "PWD", "UserName", strLoginUserName, "EName", ref strUserEName);
            //-------------------------------------
            strPassword = strPassword.Trim();
            strUserEName = strUserEName.Trim();

            if (bGetPassword && bGetName && strPassword == strLoginPassWord)
            {
                //if (strLoginUserName==)//(strUserEName == "OpEng" || strUserEName == "Op")
                {
                    bool bSavePassword = pMachine.SaveToDataBase(strMachinePath, "PWD", "UserName", strLoginUserName, "thePassword", strNewPassword);
                    MessageBox.Show(this, JudgeLanguage.JudgeLag("修改成功"));
                    //----------------------
                    tx_UserName.Text = "";
                    tx_OldPassword.Text = "";
                    tx_newPassword.Text = "";
                }
                //else
                //{
                //    MessageBox.Show(this, JudgeLanguage.JudgeLag("僅可修改 <級別1> 與 <級別2> 之密碼"));
                //    return;
                //}
            }
            else
            {
                MessageBox.Show(this, JudgeLanguage.JudgeLag(" User Name錯誤 或 原密碼錯誤"));
                return;
            }



        }

        private void dia_Login_Load(object sender, EventArgs e)
        {

        }

        private void comboBox_UserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strLoginUserName = "";
            if (comboBox_UserName.SelectedIndex == 0)
                strLoginUserName = "User";
            else if (comboBox_UserName.SelectedIndex == 1)
                strLoginUserName = "Eng";
            else if (comboBox_UserName.SelectedIndex == 2)
                strLoginUserName = "Cowain";
            tbxUserName.Text = strLoginUserName;
        }
    }
}
