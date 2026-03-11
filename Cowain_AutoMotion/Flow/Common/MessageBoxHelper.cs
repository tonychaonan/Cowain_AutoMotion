using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion
{
    public class MsgBoxHelper
    {
        /// <summary>
        /// 自定义提示框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        public static DialogResult DxMsgShow(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, HandleWait m_HandleWait = null)
        {
            if (m_HandleWait != null)
                m_HandleWait.CloseWait();
            return DevExpress.XtraEditors.XtraMessageBox.Show(text, caption, buttons, icon);
        }


        /// <summary>
        /// 提示框，默认确认按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        public static DialogResult DxMsgShowInfo(string text, string caption = "提示信息", MessageBoxButtons buttons = MessageBoxButtons.OK, HandleWait m_HandleWait = null)
        {
            return DxMsgShow(text, caption, buttons, MessageBoxIcon.Information, m_HandleWait);
        }
        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="m_HandleWait"></param>
        /// <returns></returns>
        public static DialogResult DxMsgShowInfo(string text, HandleWait m_HandleWait)
        {
            return DxMsgShowInfo(text, "提示信息", MessageBoxButtons.OK, m_HandleWait);
        }

        /// <summary>
        /// 警告框，默认确认按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        public static DialogResult DxMsgShowWarn(string text, string caption = "警告信息", MessageBoxButtons buttons = MessageBoxButtons.OK, HandleWait m_HandleWait = null)
        {
            return DxMsgShow(text, caption, buttons, MessageBoxIcon.Warning, m_HandleWait);
        }
        /// <summary>
        /// 警告框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="m_HandleWait"></param>
        /// <returns></returns>
        public static DialogResult DxMsgShowWarn(string text, HandleWait m_HandleWait)
        {
            return DxMsgShowWarn(text, "警告信息", MessageBoxButtons.OK, m_HandleWait);
        }

        /// <summary>
        /// 问题框，默认是和无按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        public static DialogResult DxMsgShowQues(string text, string caption = "问题信息", MessageBoxButtons buttons = MessageBoxButtons.YesNo, HandleWait m_HandleWait = null)
        {
            return DxMsgShow(text, caption, buttons, MessageBoxIcon.Question, m_HandleWait);
        }
        /// <summary>
        /// 问题框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="m_HandleWait"></param>
        /// <returns></returns>
        public static DialogResult DxMsgShowQues(string text, HandleWait m_HandleWait)
        {
            return DxMsgShowQues(text, "问题信息", MessageBoxButtons.YesNo, m_HandleWait);
        }

        /// <summary>
        /// 错误框，默认确认按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        public static DialogResult DxMsgShowErr(string text, string caption = "错误信息", MessageBoxButtons buttons = MessageBoxButtons.OK, HandleWait m_HandleWait = null)
        {
            return DxMsgShow(text, caption, buttons, MessageBoxIcon.Error, m_HandleWait);
        }

        /// <summary>
        /// 错误框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="m_HandleWait"></param>
        /// <returns></returns>
        public static DialogResult DxMsgShowErr(string text, HandleWait m_HandleWait)
        {
            return DxMsgShowErr(text, "错误信息", MessageBoxButtons.OK, m_HandleWait);
        }


        /// <summary>
        /// 异常信息格式化
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public static void ShowExceptionMsg(Exception ex, string strMsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + strMsg);
            }
            sb.AppendLine("***************************************************************");
            //return sb.ToString();
            DxMsgShowErr(sb.ToString());
        }


    }

    /// <summary>
    /// 操作等待
    /// </summary>
    public class HandleWait
    {
        /// <summary>
        /// 显示操作等待（同步）
        /// </summary>
        /// <param name="waitFrm"></param>
        /// <param name="caption"></param>
        /// <param name="title"></param>
        /// <param name="size"></param>
        public static DevExpress.Utils.WaitDialogForm ShowWait(DevExpress.Utils.WaitDialogForm waitFrm, string caption = "请稍后......", string title = "正在加载数据", int width = 300, int height = 100)
        {
            Size size = new Size(width, height);
            waitFrm = new DevExpress.Utils.WaitDialogForm(caption, title, size);

            return waitFrm;
        }

        /// <summary>
        /// 关闭操作等待（同步）
        /// </summary>
        /// <param name="waitFrm"></param>
        public static void CloseWait(DevExpress.Utils.WaitDialogForm waitFrm)
        {
            waitFrm.Visible = false;
        }

        private SplashScreenManager SplashScreen;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="userControl"></param>
        public void Init(UserControl userControl)
        {
            SplashScreen = new DevExpress.XtraSplashScreen.SplashScreenManager(userControl, typeof(global::Cowain_AutoMotion.FormView.dia_WaitForm), true, true, ParentType.UserControl);
        }

        public void Init(XtraForm userControl)
        {
            SplashScreen = new DevExpress.XtraSplashScreen.SplashScreenManager(userControl, typeof(global::Cowain_AutoMotion.FormView.dia_WaitForm), true, true, (int)ParentType.Form);
        }

        /// <summary>
        /// 显示操作等待
        /// </summary>
        public void ShowWait()
        {
            if (SplashScreen != null && !SplashScreen.IsSplashFormVisible)
            {
                SplashScreen.ShowWaitForm();
            }
        }

        /// <summary>
        /// 关闭操作等待
        /// </summary>
        public void CloseWait()
        {
            if (SplashScreen != null && SplashScreen.IsSplashFormVisible)
            {
                SplashScreen.CloseWaitForm();
            }
        }
    }
}
