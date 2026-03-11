using System;
using System.Linq;
using System.Data;
using System.Text;
using System.IO.Ports;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace MotionBase
{
    public class Error
    {
        public Error(ref Base pBase, string strCDescript = "", string strEDescript = "" , int nErrCode=0)
        {
            m_Happener = pBase;
            m_ErrorCode = nErrCode;
            m_strCDescript=strCDescript;
            m_strEDescript = strEDescript;
            m_ErrorType= ErrorType.錯誤;
            m_Happener狀態 = pBase.m_Status;
        }



        ~Error()
        {
        }
        //--------
        Base m_Happener;
        Base.狀態 m_Happener狀態;
        #region 參數&變數
        //****************************
        public int m_ErrorCode;
        public String m_strCDescript, m_strEDescript;
        //----------------
        public enum ErrorType
        { 
            錯誤,
            警告,
            危險,
            警急停止,
        }
        public ErrorType m_ErrorType;
        //----------------
        public struct errSloution
        {
            public string Sloution;
            public int SloutionStep;
        }
        private int m_SloutionCount = 0;
        public Dictionary<int, errSloution> SloutionList = new Dictionary<int, errSloution>();
        //****************************
        #endregion
        //-------------------------       
        public int GetErrorStation()
        {
           int iStation= m_Happener.GetStationNo();
           return iStation;
        }
        public string GetErrorStationCName()
        {
            string strCName=m_Happener.GetCName();
            return strCName;
        }
        public string GetErrorStationEName()
        {
            string strEName = m_Happener.GetEName();
            return strEName;
        }
        //-------------------------
        public void SetNextSetp(int iStep)
        {
            if(m_Happener狀態 == Base.狀態.動作中)
            {                    
                m_Happener.SetNextStep(iStep);
                m_Happener.m_Status = Base.狀態.動作中;
            }else if (m_Happener狀態 == Base.狀態.回HOME中)
            {
                m_Happener.SetNextHomeStep(iStep);
                m_Happener.m_Status = Base.狀態.回HOME中; 
            }
        }   //錯誤解除&執行Next Step
        public void ErrorHappen(ref Error pError,ErrorType errType)
        {
            m_ErrorType = errType;
	        m_Happener.m_Status = Base.狀態.錯誤發生;      
            m_Happener.ErrorHappen(ref  pError);	
        }
        public void AddErrSloution(string strSloution,int iSloutionStep)
        {
            errSloution pSloution = new errSloution();
            pSloution.Sloution = strSloution;
            pSloution.SloutionStep = iSloutionStep;
            SloutionList.Add(m_SloutionCount, pSloution);
            //------------------
            m_SloutionCount++;
        }
        public void ReleaseErrSloution()
        {
            SloutionList.Clear();
            //------------------
            m_SloutionCount=0;
        }
    }
}
