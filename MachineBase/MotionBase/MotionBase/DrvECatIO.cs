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
    public class DrvECatIO : DrvIO
    {
        #region 建構&解構式
        /// <summary>
        /// 声明IO必须参数
        /// </summary>
        /// <param name="parent">运行环境</param>
        /// <param name="nStation">工位号</param>
        /// <param name="strID">IO的ID</param>
        /// <param name="TableName">IO所在数据库Table名称</param>
        /// <param name="strCName">IO定义</param>
        public DrvECatIO(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, string strID, string TableName, string strCName) //IO由資料庫取得
            : base(homeEnum1,stepEnum1,instanceName1, parent, nStation, strID, TableName, strCName)
        {
            name = strCName;
        }

        public DrvECatIO(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, string strID, string TableName) //IO由資料庫取得
            : base(homeEnum1,stepEnum1,instanceName1, parent, nStation, strID, TableName)
        {
        }

        public DrvECatIO(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, string strID, string strEName, string strCName, ref DrvECatCard drvCard, ushort uNodeID, ushort uPortID, ushort uPinID, bool bisOut)
            : base(homeEnum1,stepEnum1,instanceName1, parent, nStation, strID, strEName, strCName, bisOut)
        {
            m_drCard = drvCard;
            //m_uCardID = m_drCard.GetCardNo();
            m_uNodeID = uNodeID;
            m_uPortID = uPortID;
            m_uPinID = uPinID;
            name = strCName;
        }

        ~DrvECatIO()
        {
        }
        #endregion
        //-------------------------
        DrvECatCard m_drCard = null;
        private bool value = false;
        private string name = "";
        //-------------------------
        public override bool GetValue()
        {
            if(name.Contains("急停"))
            {
                return true;
            }
            if(name.Contains("门禁"))
            {
                return true;
            }
            return value;
        }
        public override bool SetIO(bool bValue)
        {
            value = bValue;
            return true;
        }
    }
}
