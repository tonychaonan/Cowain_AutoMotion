using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Cowain_AutoMotion.SQLSugarHelper;

namespace Cowain_AutoMotion.FormView._4弹窗
{
    public partial class UserInformationForm : Form
    {
        #region  自定义字段
        public enum Powerlevel
        {
            Level1,
            Level2,
            Level3,
        }

        public enum Identity
        {
            製造商,
            設備工程師,
            製程工程師,
            工程師,
            生產員,
        }

        /// <summary>
        /// true新增 false修改
        /// </summary>
        bool isEditable = false;
        public  PWD myPWD;

        //记录上次输入时间，用于判定是否是手工输入
        DateTime lasttime = DateTime.Now;

        #endregion

        #region 自定义方法
        /// <summary>
        /// 是否包含数字和字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool Check(string str)
        {
            str = str.ToUpper();
            bool isABC = false;
            bool isNum = false;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 'A' && str[i] < 'Z')
                {
                    isABC = true;
                }
                if (str[i] >= '0' && str[i] < '9')
                {
                    isNum = true;
                }
            }

            return isABC && isNum;
        }
        private bool CheckErr()
        {
            if (cmb_identity.SelectedIndex < 0)
            {
                MsgBoxHelper.DxMsgShowErr("请选择用户身份！");
                cmb_identity.Focus();
                return false;
            }
            if (tbx_UserName.Text.Trim().Equals(""))
            {
                MsgBoxHelper.DxMsgShowErr("请输入用户名！");
                tbx_UserName.Focus();
                tbx_UserName.SelectAll();
                return false;
            }
            if (cmb_PowerLever.SelectedIndex < 0)
            {
                MsgBoxHelper.DxMsgShowErr("请选择用户权限等级！");
                cmb_PowerLever.Focus();
                return false;
            }
            if (txtCardID.Text.Trim().Equals(""))
            {
                MsgBoxHelper.DxMsgShowErr("请输入用户ID卡号！");
                txtCardID.Focus();
                txtCardID.SelectAll();
                return false;
            }
            if (txtUserID.Text.Trim().Equals(""))
            {
                MsgBoxHelper.DxMsgShowErr("请输入用户工号！");
                txtUserID.Focus();
                txtUserID.SelectAll();
                return false;
            }
            if (tbx_PassWord.Text.Trim().Equals(""))
            {
                MsgBoxHelper.DxMsgShowErr("请输入用户密码！");
                tbx_PassWord.Focus();
                tbx_PassWord.SelectAll();
                return false;
            }

            if (!Check(tbx_PassWord.Text.Trim()))
            {
                MsgBoxHelper.DxMsgShowErr("密码必须包含字母和数字！");
                tbx_PassWord.Focus();
                tbx_PassWord.SelectAll();
                return false;
            }

            if (tbx_PassWord.Text.Trim().Length < 6)
            {
                MsgBoxHelper.DxMsgShowErr("密码长度必须超过5位！");
                tbx_PassWord.Focus();
                tbx_PassWord.SelectAll();
                return false;
            }

            //检查用户名是否已经存在
            if (isEditable)
            {
                var listMotors = DBContext<PWD>.GetInstance().GetList();
                foreach (var item in listMotors)
                {
                    if (myPWD.UserName.Trim() == ConvertHelper.GetDef_Str(item.UserName).Trim())
                    {
                        MsgBoxHelper.DxMsgShowErr("已存在用户名["+ myPWD.UserName + "]！");
                        tbx_UserName.Focus();
                        tbx_UserName.SelectAll();
                        return false;
                    }
                    if(myPWD.UserID.Trim() == ConvertHelper.GetDef_Str(item.UserID).Trim())
                    {
                        MsgBoxHelper.DxMsgShowErr("已存在用户工号[" + myPWD.UserID.Trim() + "]！");
                        txtUserID.Focus();
                        txtUserID.SelectAll();
                        return false;
                    }
                    if (myPWD.CardID.Trim() == ConvertHelper.GetDef_Str(item.CardID).Trim())
                    {
                        MsgBoxHelper.DxMsgShowErr("已存在用户ID卡号！");
                        txtCardID.Focus();
                        txtCardID.SelectAll();
                        return false;
                    }
                }
            }
            else
            {
                //修改时判断
                Expression<Func<PWD, bool>> cd = null;
                var listMotors = DBContext<PWD>.GetInstance().GetList(cd = f => f.ID != myPWD.ID);
                foreach (var item in listMotors)
                {
                    if (myPWD.UserName.Trim() == ConvertHelper.GetDef_Str(item.UserName).Trim())
                    {
                        MsgBoxHelper.DxMsgShowErr("已存在用户名[" + myPWD.UserName + "]！");
                        tbx_UserName.Focus();
                        tbx_UserName.SelectAll();
                        return false;
                    }
                    if (myPWD.UserID.Trim() == ConvertHelper.GetDef_Str(item.UserID).Trim())
                    {
                        MsgBoxHelper.DxMsgShowErr("已存在用户工号[" + myPWD.UserID.Trim() + "]！");
                        txtUserID.Focus();
                        txtUserID.SelectAll();
                        return false;
                    }
                    if (myPWD.CardID.Trim() == ConvertHelper.GetDef_Str(item.CardID).Trim())
                    {
                        MsgBoxHelper.DxMsgShowErr("已存在用户ID卡号！");
                        txtCardID.Focus();
                        txtCardID.SelectAll();
                        return false;
                    }
                }
            }

            //判断卡号和密码是否一样
            if (tbx_PassWord.Text.Trim() == txtCardID.Text.Trim())
            {
                MsgBoxHelper.DxMsgShowErr("用户密码不能与ID卡号相同！");
                tbx_PassWord.Focus();
                tbx_PassWord.SelectAll();
                return false;
            }


            //判断输入的两次密码是否正确
            if (tbx_PassWord.Text.Trim() == tbx_SecondPassWard.Text.Trim())
            {
                myPWD.thePassWord = tbx_PassWord.Text.Trim();
                myPWD.CName = cmb_identity.SelectedItem.ToString();
                int index = cmb_PowerLever.SelectedIndex;
                if (index == 0)
                {
                    myPWD.EName = "Op";
                }
                else if (index == 1)
                {
                    myPWD.EName = "Eng";
                }
                else
                {
                    myPWD.EName = "Maker";
                }
                myPWD.CardID = txtCardID.Text.Trim();
                myPWD.UserID = txtUserID.Text.Trim();

            }
            else
            {
                //pb_passward.Image = Image.FromFile(@"D:\科瑞恩资料\Cowain_AutoMotion\Cowain_AutoMotion\Cowain_AutoMotion\Picture\错误.png");
                //pb_secondPassward.Image = Image.FromFile(@"D:\科瑞恩资料\Cowain_AutoMotion\Cowain_AutoMotion\Cowain_AutoMotion\Picture\错误.png");

                MsgBoxHelper.DxMsgShowErr("两次输入的密码不一致！");
                return false;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pWD"></param>
        /// <param name="localIsEditable"></param>
        public UserInformationForm( PWD pWD,bool localIsEditable)
        {
            InitializeComponent();
            isEditable = localIsEditable;
            myPWD = pWD;
        }


        /// <summary>
        ///  窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInformationForm_Load(object sender, EventArgs e)
        {
            //初始化textbox
            //textBoxes = new TextBox[] { tbx_UserName, tbx_PassWard, tbx_SecondPassWard };
            //for (int i = 0; i < textBoxes.Length; i++)
            //{          
            //    if (!isEditable && i==0)
            //    {
            //        textBoxes[i].Text = myPWD.UserName;
            //        textBoxes[i].Enabled = false;
            //    }
            //    else
            //    {
            //        textBoxes[i].Clear();
            //    }
            //}


            //初始化combox
            List<string> powerList =  Enum.GetNames(typeof(Powerlevel)).ToList();
            for (int i = 0; i < powerList.Count; i++)
            {
                cmb_PowerLever.Properties.Items.Add(powerList[i]);
            }
            if (!isEditable)
            {
                
                if (myPWD.EName.Contains("Maker"))
                {
                    this.cmb_PowerLever.SelectedItem = "Level3";
                }
                else if (myPWD.EName.Contains("Eng"))
                {
                    this.cmb_PowerLever.SelectedItem = "Level2";
                }
                else
                {
                    this.cmb_PowerLever.SelectedItem = "Level1";
                }

                tbx_UserName.Text = myPWD.UserName.Trim();
                txtCardID.Text = myPWD.CardID.Trim();
                txtUserID.Text = myPWD.UserID.Trim();
            }
            else
            {
                this.cmb_PowerLever.SelectedIndex = 0;
            }

            List<string> identityList = Enum.GetNames(typeof(Identity)).ToList();
            for (int i = 0; i < identityList.Count; i++)
            {
                cmb_identity.Properties.Items.Add(identityList[i]);
            }
            if (!isEditable)
            {
                this.cmb_identity.Text= myPWD.CName;
            }
            else
            {
                this.cmb_identity.SelectedIndex = 0;
            }

            this.ControlBox = false;
        }


        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Ok_Click(object sender, EventArgs e)
        {
            #region 
            //先对所有的pictureBox清除一边
            //if (pb_passward.Image != null)
            //{
            //    pb_passward.Image = null;
            //}
            //if (pb_secondPassward.Image != null)
            //{
            //    pb_secondPassward.Image = null;
            //}
            //if (pb_userName.Image !=null)
            //{
            //    pb_userName.Image = null;
            //}

            //for (int i = 0; i < textBoxes.Length; i++)
            //{
            //    if (textBoxes[i].Text.Trim() == string.Empty)
            //    {
            //        if(i==0)
            //        {
            //            pb_userName.Image = Image.FromFile(@"D:\科瑞恩资料\Cowain_AutoMotion\Cowain_AutoMotion\Cowain_AutoMotion\Picture\错误.png");
            //            isError = true;
            //        }
            //        else if (i==1)
            //        {
            //            pb_passward.Image = Image.FromFile(@"D:\科瑞恩资料\Cowain_AutoMotion\Cowain_AutoMotion\Cowain_AutoMotion\Picture\错误.png");
            //            isError = true;
            //        }
            //        else if (i==2)
            //        {
            //            pb_secondPassward.Image = Image.FromFile(@"D:\科瑞恩资料\Cowain_AutoMotion\Cowain_AutoMotion\Cowain_AutoMotion\Picture\错误.png");
            //            isError = true;
            //        }
            //    }
            //    else
            //    {
            //        if (i==0)
            //        {
            //            myPWD.UserName = textBoxes[i].Text;
            //        }
            //    }
            //}
            #endregion
            //if (isEditable)
            //{
                myPWD.UserID = txtUserID.Text.Trim();
                myPWD.UserName = tbx_UserName.Text.Trim();
                myPWD.CardID = txtCardID.Text.Trim();
                myPWD.CName = cmb_identity.Text.Trim();
                myPWD.EName = cmb_PowerLever.Text.Trim();
                myPWD.thePassWord = tbx_PassWord.Text.Trim();
            //}
            bool ok = CheckErr();
            if(!ok)
            {
                return;
            }


            this.Close();
        }


        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cacel_Click(object sender, EventArgs e)
        {
            myPWD = null;
            this.Close();
        }

        private void txtCardID_KeyPress(object sender, KeyPressEventArgs e)
        {
            DateTime curtime = DateTime.Now;
            TimeSpan span = curtime - lasttime;
            if (span.TotalMilliseconds > 50)
            {
                txtCardID.Text = "";
            }

            lasttime = curtime;
        }
    }
}
