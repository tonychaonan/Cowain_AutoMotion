using System;
using System.Linq;
using System.Data;
using System.Text;
using System.IO.Ports;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;

namespace MotionBase
{
    public class DrvECatCard:DrvCard
    {
        public DrvECatCard(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, String strEName, String strCName, ushort CardNumberID, int ErrCodeBase)   //第一張 DncNet CardNumberID由0開始
            : base(homeEnum1, stepEnum1, instanceName1,parent, strEName, strCName, ErrCodeBase)
        {
            nCardNumberID = CardNumberID;
            m_strEName = strEName;
            m_strCName = strCName;
            //--------------
            for (int i = 0; i < 16; i++)
                gCardNoList[i] = 99; //無開卡預設狀態為99
            //--------------
            m_bisOpen = false;
        }
        public DrvECatCard(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, String strEName, String strCName, ushort CardNumberID,bool bOpenCard ,int ErrCodeBase)   //第一張 DncNet CardNumberID由0開始
            : base(homeEnum1, stepEnum1, instanceName1,parent, strEName, strCName, ErrCodeBase)
        {
            nCardNumberID = CardNumberID;
            m_strEName = strEName;
            m_strCName = strCName;
            //--------------
            for (int i = 0; i < 16; i++)
                gCardNoList[i] = 99; //無開卡預設狀態為99
            //-------------
            m_bisOpen = false;
            if (bOpenCard)  //開卡 & Init
                OpenCard();
        }
        ~DrvECatCard()
        {
            CloseCard();
        }
        //---------------------------

        static bool m_bisOpenCard = false;
        static ushort m_existcard = 0;
        //--------
        ushort nCardNumberID;
        ushort  rc;
        ushort gCardNo = 0;
        ushort[] gCardNoList = new ushort[16];
        //---------------------------
        protected bool m_bisOpen;
        //---------------------------
        public bool CardisOpen()
        {
            return m_bisOpen;
        }

        virtual public bool OpenCard()
        {
            return m_bisOpen;
        } 
        virtual public bool CloseCard()
        {
            return true;
        }
        public bool CardInit()
        {
            m_bisOpen = true;
            return true;
        }
        public ushort GetCardNo()
        {
            return gCardNo;
        }
    }
}
