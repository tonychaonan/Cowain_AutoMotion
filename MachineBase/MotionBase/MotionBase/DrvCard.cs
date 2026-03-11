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
    public class DrvCard:Base
    {
        public DrvCard(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, String strEName, String strCName, int ErrCodeBase)
            : base(homeEnum1, stepEnum1, instanceName1,parent,  ErrCodeBase)
        {
            m_strEName = strEName;
            m_strCName = strCName;
            m_bisOpen = false;
        }
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
            return !m_bisOpen;
        }
    }
}
