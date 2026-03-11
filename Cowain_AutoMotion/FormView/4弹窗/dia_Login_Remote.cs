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
using Cowain_AutoMotion.Flow;
using System.IO;
using Post;
using Cowain_AutoMotion;
using System.Threading;
using Cowain_Machine;
using Cowain;

namespace Cowain_Form.FormView
{
    public partial class dia_Login_Remote : DevExpress.XtraEditors.XtraForm
    {
        #region 自定义变量
        public clsMachine pMachine;
        // public LXPOSTClass post;
        //记录上次输入时间，用于判定是否是手工输入
        DateTime lasttime = DateTime.Now;

        private string m_UserName = string.Empty;
        private string m_Level = string.Empty;
        private string m_Function = string.Empty;
        private string m_cardID = "null ";
        /// <summary>
        /// 是否授权登录
        /// </summary>
        public bool isAuthorize = false;
        #endregion

        #region 自定义方法
        /// <summary>
        /// 登录失败时的状态
        /// </summary>
        private void ShowInfoFales()
        {
            txt_User.Text = "";
            //text_BadgeID.Text = "";
            txt_Function.Text = "";
            labLevel.Text = "NULL";
            btn_Login.Enabled = false;
        }

        /// <summary>
        /// 成功登录时的状态
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="function"></param>
        /// <param name="level"></param>
        private void ShowLoginInfo(string userName, string level, string function)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    txt_User.Text = userName;
                    txt_Function.Text = function;
                    labLevel.Text = "LEVEL" + "  " + level;
                    btn_Login.Enabled = true;
                    btn_Login.BackColor = Color.WhiteSmoke;
                }));
            }
            else
            {
                txt_User.Text = userName;
                txt_Function.Text = function;
                labLevel.Text = "LEVEL" + "  " + level;
                if (isAuthorize && level == "3" || !isAuthorize)
                {

                    btn_Login.Enabled = true;
                }
                btn_Login.BackColor = Color.WhiteSmoke;
            }
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

        public dia_Login_Remote(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
        }



        private void txt_Badge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                string badge = txt_Badge.Text.Trim();
                if(badge=="")
                {
                    return;
                }
                foreach (char item in badge)
                {
                    if(!Char.IsDigit(item))
                    {
                        txt_Badge.Text = "";
                        return;
                    }
                }
                m_cardID = badge;
                string sendMsg = string.Empty;
                //从MES获取信息
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("工站", MESDataDefine.MESLXData.terminalName);
              
                if (isAuthorize)
                {
                    keyValuePairs.Add("操作员卡号", badge + "," + "Authorize");
                }
                else
                {
                    keyValuePairs.Add("操作员卡号", badge + "," + "Main");
                }
                POSTClass.AddCMD(0, CMDStep.用户登录, keyValuePairs);
                Task.Delay(500).Wait();
                // string loginResult = post.GetLoginInfo(badge,ref sendMsg);
                try
                {
                    MesData mesData = POSTClass.getResult(0, CMDStep.用户登录);
                    if (mesData.Result != "OK")
                    {
                        ShowInfoFales();
                    }
                    else
                    {
                        m_UserName = mesData.dataReturn["Name"];
                        m_Level = mesData.dataReturn["Access_Level"];
                        m_Function = mesData.dataReturn["Function"];
                        ShowLoginInfo(m_UserName, m_Level, m_Function);
                    }
                    txt_Badge.SelectAll();
                    txt_Badge.Focus();
                }
                catch
                { }
            }
            else
            {
                //DateTime curtime = DateTime.Now;
                //TimeSpan span = curtime - lasttime;
                //if (span.TotalMilliseconds > 100 && txt_Badge.Text.Length != 10)
                //{
                //    txt_Badge.Text = "";
                //}

                //lasttime = curtime;
            }
        }

        private void btn_Login_MouseClick(object sender, MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Left)
            {
                if (!isAuthorize)
                {
                    MachineDataDefine.Login_Name = "null";
                    MachineDataDefine.Login_Function = "null";
                    MachineDataDefine.Login_CardID = "null";
                    MachineDataDefine.Authorize_Name = "null";
                    MachineDataDefine.Authorize_Function = "null";
                    MachineDataDefine.Authorize_CardID = "null";
                    if (m_Level == "3")
                    {
                        var result = DialogResult.OK;
                        //if (m_Function.Contains("OSS"))
                            if (!m_Function.Contains("GTK") && !m_Function.Contains("ICT") && !m_Function.Contains("Apple"))
                            {
                           // this.Close();
                            dia_Login_Remote Authorize_Login = new dia_Login_Remote(ref pMachine);
                            Authorize_Login.isAuthorize = true;
                           
                            result = Authorize_Login.ShowDialog();
                        }
                        if (result == DialogResult.OK)
                        {
                            pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Maker;
                            MachineDataDefine.Authorizeis = true;
                            // this.Close();
                        }
                        else
                        {
                            MachineDataDefine.Authorizeis = false;
                            MachineDataDefine.Login_Name = m_UserName;
                            MachineDataDefine.Login_Function = m_Function;
                            MachineDataDefine.Login_CardID = m_cardID;
                            this.Close();
                            return;
                        }
                    //    pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Maker;
                    }
                    else if (m_Level == "2")
                    {
                        pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Eng;
                    }
                    else if (m_Level == "1")
                    {
                        pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.Operator;
                    }
                    else
                    {
                        pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.UnLogin;
                    }
                    //  savelog("Name:" + m_UserName + "    Level:" + m_Level + "      Release Ver: " + MESDataDefine.StrVersion);
                    MachineDataDefine.Login_Name = m_UserName;
                    MachineDataDefine.Login_Function = m_Function;
                    MachineDataDefine.Login_CardID = m_cardID;

                  //  LogAuto.Notify("登录失败", (int)MachineStation.主监控, LogLevel.Info);
                }
                else
                {
                    MachineDataDefine.m_LoginUser = pMachine.m_LoginUser;
                    MachineDataDefine.m_LoginUserName = m_UserName;
                    MachineDataDefine.m_LoginCardID = txt_Badge.Text;
                    MachineDataDefine.Authorize_Name = m_UserName;
                    MachineDataDefine.Authorize_Function = m_Function;
                    MachineDataDefine.Authorize_CardID = m_cardID;
                    // LogAuto.Notify("登录成功", (int)MachineStation.主监控, LogLevel.Info);
                }

                this.DialogResult = DialogResult.OK;
               this.Close();
            }
        }

        private void btn_Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ////注销
                //pMachine.m_LoginUser = MotionBase.Sys_Define.enPasswordType.UnLogin;
                //MachineDataDefine.m_LoginUser = pMachine.m_LoginUser;
                //MachineDataDefine.m_LoginUserName = "";
                //MachineDataDefine.m_LoginCardID = "";

                //this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            if (txt_Badge.Text.Length != 10)
            {
                txt_Badge.Text = "";
            }
        }

        private void txt_Badge_KeyDown(object sender, KeyEventArgs e)
        {
            timer.Enabled = true;
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {

        }
        public static List<Control> control = new List<Control>(); //Login中所有控件
        private void dia_Login_Remote_Load(object sender, EventArgs e)
        {
            btn_Login.Enabled = false;
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

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}