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
using System.Globalization;

namespace MotionBase
{
    public class DrvECatMotor : DrvMotor
    {
        public DrvECatMotor(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String strID, String strEName, String strCName, int ErrCodeBase = 0)   //馬達CardID由資料庫取得
            : base(homeEnum1,stepEnum1,instanceName1, parent, nStation, strID, strEName, strCName, ErrCodeBase)
        {
        }
        public DrvECatMotor(Type homeEnum1, Type stepEnum1, string instanceName1, Base parent, int nStation, String strID, String strEName, String strCName, ref DrvECatCard drvCard, int ErrCodeBase = 0)   //CardID由建構式時帶入 
             : base(homeEnum1,stepEnum1,instanceName1, parent, nStation, strID, strEName, strCName, ErrCodeBase)
        {
            nDrvCard = drvCard;
        }
        //---------------------------
        DrvECatCard nDrvCard = null;
        //-------------------------------------
        bool m_bisDoHome = false;
        clsMotorMove clsMotorMove1 = new clsMotorMove();
        //------------------------------------
        /// <summary>
        /// 设置电子齿轮比
        /// </summary>
        /// <param name="n分子 编码器的分辨率"></param>
        /// <param name="n分母 随便填 会读取数据库"></param>
        /// <returns></returns>
        public override bool Set齒輪比(uint n分子, uint n分母)
        {
            return true;
        }
        public override bool SetPosPosition(double dbPos)
        {
            return true;
        }

        public override bool SetPPSPosition(int nPPS)
        {
            return true;
        }

        public override bool SetPPSCommand(int nPPS)
        {
            return true;
        }
        /// <summary>
        /// 凌臣编码器模块
        /// </summary>
        /// <param name="nPPS"></param>
        /// <returns></returns>
        public override bool SetPDOCommand(int nPPS)
        {
            return true;
        }
        public override bool AlarmReset()
        {
            clsMotorMove1.resetAlarm();
            return false;
        }
        //-----------------------------------
        public override int GetNowPositionPPS()
        {
            return 0;
        }
        public override int GetNowCommandPPS()
        {
            return 0;
        }
        public override double GetPosition()
        {
            return clsMotorMove1.getCurrentPos();
        }
        public override double GetCommand()
        {
            return 0;
        }
        /// <summary>
        /// 凌臣编码器模块
        /// </summary>
        /// <param name="nPPS"></param>
        /// <returns></returns>
        public override double GetPDOCommand()
        {
            return 0;
        }
        //------------------------
        public override bool MotorDoHome()
        {
            m_bHomeCompleted = true;
            return true;
        }
        public override bool isHomeCompletedForAPI()
        {
            return m_bHomeCompleted;
        }
        public override bool isHomeCompleted()
        {
            return m_bHomeCompleted;
        }
        public override bool SetSevON(bool bOn)
        {
            clsMotorMove1.b_SevOn = true;
            return true;
        }
        public override bool MotorStop()
        {
            clsMotorMove1.stopMotor();
            return true;
        }
        protected override bool MotorAMove(double dbPos, double dbSp = 100)
        {
            double speed1 = m_HiSpeed / m_dbPluseRev * m_dbUnitRev * (dbSp / 100.0);
            clsMotorMove1.absMove(dbPos, speed1, m_dbHiAccTime, m_dbLoDesTime);
            if (clsMotorMove1.errStr != "")
            {
                return false;
            }
            return true;
        }
        protected override bool MotorRMove(double dbDist, double dbSp = 100)
        {
            double speed1 = m_HiSpeed / m_dbPluseRev * m_dbUnitRev * (dbSp / 100.0);
            clsMotorMove1.relMove(dbDist, speed1, m_dbHiAccTime, m_dbLoDesTime);
            if (clsMotorMove1.errStr != "")
            {
                return false;
            }
            return true;
        }
        public override bool isMotionDone()
        {
            return clsMotorMove1.isMotionDone();
        }
        public override bool isAlarm()
        {
            return clsMotorMove1.alarm;
        }
        public override bool isSevOn()
        {
            return clsMotorMove1.b_SevOn;
        }
        public override bool isPEL()
        {
            return clsMotorMove1.b_PEL;
        }
        public override bool isMEL()
        {
            return clsMotorMove1.b_MEL;
        }

        public override bool isMELorPEL()
        {
            if (isMEL() || isMEL())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool isHome()
        {
            return m_bisDoHome;
        }
        public override string getErrorCode()
        {
            return clsMotorMove1.errStr;
        }
    }
}
