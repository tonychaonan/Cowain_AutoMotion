using System;
using System.Linq;
using System.Data;
using System.Text;
using System.IO.Ports;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using MotionBase;

namespace MotionBase
{
    public class DrvIO : Base
    {
        public DrvIO(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String strID, string TableName, bool bisOut = false)
            : base(homeEnum1, stepEnum1, instanceName1,parent, nStation, 0)
        {
            m_strID = strID;
            m_bInverter = false;
            m_strTableName = TableName;
            m_bisOut = bisOut;
            //-----------------------------
            IOList.Add(m_nIOCount, (DrvIO)m_NowAddress);
            m_nIOCount++;
            //-----------------------------
        }

        public DrvIO(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String strID, String TableName, String strCName, bool bisOut = false)
            : base(homeEnum1,stepEnum1,instanceName1, parent, nStation, TableName, strCName, 0)
        {
            m_strID = strID;
            m_strTableName = TableName;
            m_bInverter = false;
            m_bisOut = bisOut;
            //-----------------------------
            IOList.Add(m_nIOCount, (DrvIO)m_NowAddress);
            m_nIOCount++;
            //-----------------------------
        }

        protected string m_strID, m_strTableName;
        protected ushort m_CardID, m_uNodeID, m_uPortID, m_uPinID;
        protected bool m_bisOut, m_bInverter;

        public struct tyIO_Parameter
        {
            public int i_Station;
            public string strEName;
            public string strCName;
            //-------------------
            public string strID;
            public ushort uCardID;
            public ushort uNodeID;
            public ushort uPortID;
            public ushort uPinID;
            public bool bisOut;
            public bool bInverter;
            //--------------------
        }
        public bool GetParameter(ref tyIO_Parameter tyParameter)
        {
            tyParameter.i_Station = m_nStation;
            tyParameter.strID = m_strID;
            tyParameter.strCName = m_strCName;
            tyParameter.strEName = m_strEName;
            //-------------------
            tyParameter.uCardID = m_CardID;
            tyParameter.uNodeID = m_uNodeID;
            tyParameter.uPortID = m_uPortID;
            tyParameter.uPinID = m_uPinID;
            tyParameter.bisOut = m_bisOut;
            tyParameter.bInverter = m_bInverter;
            return true;
        }
        public string GetID() { return m_strID; }
        public override bool LoadMachineData(string strMachinePath)
        {
            if (m_SystemDataBaseType == enSystemDataBaseType.enTXT)       //判斷MachineData檔案格式
                LoadTxtMachineData(strMachinePath);
            else
                LoadMDBMachineData(strMachinePath);
            //------------------------------------
            return base.LoadMachineData(strMachinePath);
        }
        private bool LoadTxtMachineData(string strMachinePath)
        {
            string striniFilePath = strMachinePath.Replace(".mdb", "");
            striniFilePath = striniFilePath + ".txt";
            //---------------------------

            bool bRet = true;
            //String iniPath = strMachinePath;
            String iniPath = striniFilePath;
            String strData = "", strIOID = "";
            String[] strPramData = new string[8];
            StreamReader sr;
            try
            {
                sr = new StreamReader(iniPath, Encoding.Default);
            }
            catch (Exception)
            {
                return false;
            }

            String line;
            while ((line = sr.ReadLine()) != null)
            {
                strIOID = "";
                strData = line.Replace(" ", "");
                strData = strData.Replace("\t", "");
                //-------------------
                int iLength = strData.Length;
                int iIndex = strData.IndexOf(",");
                if (iIndex > 0)
                    strIOID = strData.Remove(iIndex);

                if (strIOID == m_strID)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        strData = strData.Remove(0, iIndex + 1);
                        iIndex = strData.IndexOf(",");
                        if (iIndex >= 0)
                            strPramData[i] = strData.Remove(iIndex);
                        else
                            strPramData[i] = strData;
                    }
                }
            }

            //m_strCName = strPramData[0];
            //m_strEName = strPramData[1];
            m_CardID = Convert.ToUInt16(strPramData[2]);
            m_uNodeID = Convert.ToUInt16(strPramData[3]);
            m_uPortID = Convert.ToUInt16(strPramData[4]);
            m_uPinID = Convert.ToUInt16(strPramData[5]);
            m_bisOut = (strPramData[6] == "1") ? true : false;
            m_bInverter = (strPramData[7] == "1") ? true : false;
            return bRet;
        }
        private bool LoadMDBMachineData(string strMachinePath)
        {
            bool bRet = true;
            //oDB = new Sys_DataBase(strMachinePath);
            System.Data.Common.DbDataReader oRs = null;
            bool RetValue = false;
            try
            {
                string sSQL;
                bool ret = false, retRead = false;
                if (m_strTableName.Contains("Out"))
                    bRet = true; ;
                sSQL = "Select * From  " + m_strTableName + " Where ID=" + "'" + m_strID + "'";
                ret = oDB_load.Find(sSQL, ref oRs);
                if (ret)
                {
                 
                    retRead = oRs.Read();
                    m_CardID = Convert.ToUInt16(oRs["CardID"]);
                    m_strCName = oRs["CName"].ToString();
                    m_strEName = oRs["EName"].ToString();
                    m_uNodeID = Convert.ToUInt16(oRs["NodeID"]);
                    m_uPortID = Convert.ToUInt16(oRs["PortID"]);
                    m_uPinID = Convert.ToUInt16(oRs["PinID"]);
                    m_bisOut = Convert.ToBoolean(oRs["isOut"]);

                   

                    m_bInverter = Convert.ToBoolean(oRs["isInverter"]);
                }
                else
                {
                    sSQL = ("Insert into " + m_strTableName + " (ID,CName,EName,CardID,NodeId,PortID,PinID,isOut,isInverter) Values('" + m_strID + "','" + m_strCName + "','" + m_strEName + "',0,0,0,0,0,0)");
                    ret = oDB_load.Save(sSQL);
                }
                oDB_load.Fun_CloseRS(ref oRs);
                //oRs = null;
                //oDB.Fun_CloseDB();
                //oDB = null;
                //RetValue = (ret && retRead) ? true : false;
            }
            catch (Exception ex)
            {
                oDB.Fun_CloseRS(ref oRs);
                //oRs = null;
                //oDB.Fun_CloseDB();
                //oDB = null;
                return false;
            }
            return bRet;
        }

        //---------------------------
        virtual public bool SetIO(bool bValue)
        {
            return true;
        }
        virtual public bool GetValue()
        {
            return true;
        }
    }
}
