using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MotionBase;
using OpenOffice_Connect;

namespace MotionBase
{
    public class ScanTimer:Base
   {
       #region Timer建構式
        public ScanTimer(Type homeEnum1, Type stepEnum1, string instanceName1,Base parent, int nStation, String TimerID, String strEName, String strCName, double db設定值, int ErrCode=0)
            : base(homeEnum1, stepEnum1, instanceName1,parent, nStation, strEName, strCName, ErrCode)
       {
           m_TimerID = TimerID;
           m_bTimerStart = false;
           m_dbTimerCount = 0;
           m_db設定值 = db設定值;
           //------------
           TimerList.Add(m_nTimerCount, m_NowAddress);
           m_nTimerCount++;
           //------------------------------------------
       }
      #endregion
      //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
       String m_TimerID;
       bool m_bTimerStart;
       double m_dbTimerCount,m_db設定值;

        public struct tyTimer_Parameter
        {
            public string strTimerID;
            public string strEName;
            public string strCName;
            //-------------------
            public string strInterval;
            //--------------------
        }
        public bool GetParameter(ref tyTimer_Parameter tyParameter)
        {
            tyParameter.strTimerID = m_TimerID;
            tyParameter.strCName = m_strCName;
            tyParameter.strEName = m_strEName;
            //-------------------
            int nInterval = Convert.ToInt32(m_db設定值);
            tyParameter.strInterval = nInterval.ToString();

            return true;
        }
        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        public override bool LoadMachineData(string strMachinePath)
       {

            bool bRet = true;
            oDB = new Sys_DataBase(strMachinePath);
            System.Data.Common.DbDataReader oRs = null;
            bool RetValue = false;
            try
            {
                string sSQL;
                bool ret = false, retRead = false;
                sSQL = "Select * From  Timers Where ID=" + "'" + m_TimerID + "'";
                ret = oDB.Fun_RsSQL(sSQL, ref oRs);
                if (ret)
                {
                    retRead = oRs.Read();
                    m_strCName = oRs["CName"].ToString();
                    m_strEName = oRs["EName"].ToString();
                    m_db設定值 = Convert.ToDouble(oRs["Interval"]);
                }
                else
                {
                    sSQL = ("Insert into Timers (ID,CName,EName,Interval) Values('" + m_TimerID + "','" + m_strCName + "','" + m_strEName + "'1000)");
                    ret = oDB.Fun_ExecSQL(sSQL);
                }
                oDB.Fun_CloseRS(ref oRs);
                oRs = null;
                oDB.Fun_CloseDB();
                oDB = null;
                //RetValue = (ret && retRead) ? true : false;
            }
            catch (Exception ex)
            {
                oDB.Fun_CloseRS(ref oRs);
                oRs = null;
                oDB.Fun_CloseDB();
                oDB = null;
                return false;
            }
            return bRet;
        }
        public string GetTimerID() { return m_TimerID; }
       public void SetTimer(double db_ms) //單位 Sec
       {
           m_db設定值 = db_ms;  // (db_Sec * 1000);
       }
       public void Start()
       {
           m_dbTimerCount = 0;
           m_bTimerStart = true;
       }
       public override  void Stop()
       {
           m_bTimerStart = false;
           base.Stop();
       }
       public bool isTimeOut()
       {
           return (!m_bTimerStart);
       }
       public override void Cycle(ref double dbTime)
       {
           if (m_bTimerStart)
           {
               m_dbTimerCount += dbTime;
               if (m_dbTimerCount > m_db設定值)
                   m_bTimerStart = false;
           }          
           base.Cycle(ref dbTime);
       }

   }
}
