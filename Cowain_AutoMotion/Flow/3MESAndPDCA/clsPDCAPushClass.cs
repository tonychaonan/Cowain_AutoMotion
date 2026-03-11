using Chart;
using Cowain_Form.FormView;
using Cowain_Machine.Flow;
using MotionBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.Flow
{
    public class clsPDCAPushClass : Base
    {
        public clsPDCAPushClass(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, int nSubID, String strEName, String strCName, int ErrCodeBase)
           : base( homeEnum1,  stepEnum1,  instanceName1, parent, nStation, strEName, strCName, ErrCodeBase)
        {
            String strStation = nStation.ToString();
            m_iSubID = nSubID;
            Station = nStation;
            m_tmDelay = new System.Timers.Timer(1000);
            m_tmDelay.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_DelayTimeOut);
        }
        ~clsPDCAPushClass()
        {
        }
        int Station = 0;
        int m_iSubID = 0;
        System.Timers.Timer m_tmDelay;
        bool b_Result = false;
        private void OnTimedEvent_DelayTimeOut(object source, System.Timers.ElapsedEventArgs e) { m_tmDelay.Enabled = false; }
        PDCAStep m_Step;
        LogExcel LogExcel = new LogExcel();
        public Error pError = null;
        /// <summary>
        /// 统计PDCA上传失败次数，当次数大于3时再报警
        /// </summary>
        int NGcount = 0;
        public enum PDCAStep
        {
            开始,
            判断图片压缩包是否存在,
            上传PDCA,
            等待结果,
            完成,
        }
        public override void Stop()
        {
            m_Status = 狀態.待命;
            base.Stop();
        }
        public bool getResult()
        {
            return b_Result;
        }
        public override void HomeCycle(ref double dbTime)
        {
            m_Status = 狀態.待命;
            base.HomeCycle(ref dbTime);
        }

        public override void StepCycle(ref double dbTime)
        {
            m_Step = (PDCAStep)m_nStep;
            switch (m_Step)
            {
                case PDCAStep.开始:
                    m_tmDelay.Enabled = false;
                    m_tmDelay.Interval = 2000;
                    m_tmDelay.Start();
                    m_nStep = (int)PDCAStep.判断图片压缩包是否存在;
                    break;
                case PDCAStep.判断图片压缩包是否存在:
                    string imagePath = "";
                    bool b_Exist = File.Exists(imagePath);
                    if (b_Exist != true && m_tmDelay.Enabled == false)
                    {
                        string strShowMessage = "压缩图片不存在";
                        pError = new Error(ref this.m_NowAddress, strShowMessage, "", (int)MErrorDefine.MErrorCode.压缩图片不存在);
                        pError.AddErrSloution("Retry ", (int)PDCAStep.开始);//Retry，再試一次
                        pError.AddErrSloution("OK (Ignore Upload PDCA)", (int)PDCAStep.完成);
                        pError.ErrorHappen(ref pError, Error.ErrorType.錯誤);
                    }
                    else if (b_Exist)
                    {
                        m_nStep = (int)PDCAStep.上传PDCA;
                    }
                    break;
                case PDCAStep.上传PDCA:
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("产品条码", MESDataDefine.SN);
                    dict.Add("开始时间", MESDataDefine.startDateTime);
                    dict.Add("结束时间", DateTime.Now.ToString());
                    dict.Add("压缩图片路径", DateTime.Now.ToString());
                    dict.Add("电脑账户", DateTime.Now.ToString());
                    dict.Add("电脑密码", DateTime.Now.ToString());
                    dict.Add("段差1", "0");
                    dict.Add("段差2", "0");
                    dict.Add("段差3", "0");
                    dict.Add("段差4", "0");
                    dict.Add("段差5", "0");
                    dict.Add("段差6", "0");
                    dict.Add("段差7", "0");
                    dict.Add("段差8", "0");
                    dict.Add("角度", "0");
                    dict.Add("载具SN", MESDataDefine.holdSN);
                    Post.POSTClass.AddCMD(0, Post.CMDStep.上传PDCA, dict);
                    m_nStep = (int)PDCAStep.等待结果;
                    m_tmDelay.Enabled = false;
                    m_tmDelay.Interval = 8000;
                    m_tmDelay.Start();
                    break;
                case PDCAStep.等待结果:
                    if(Post.POSTClass.getResult(0, Post.CMDStep.上传PDCA).Result=="OK")
                    {
                        m_nStep = (int)PDCAStep.完成;
                    }
                    break;
                case PDCAStep.完成:
                    m_Status = 狀態.待命;
                    break;
            }
        }
        public bool PDCAAction()
        {
            int doStep = (int)PDCAStep.开始;
            bool bRet = DoStep(doStep);
            return bRet;
        }
    }
}
