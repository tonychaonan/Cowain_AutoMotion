using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotionBase
{
    public class Sys_Define
    {
        public static UInt64 m_cardHandle;            //轴卡操作句柄
        public static string m_strDeviceConfig = ".\\deviceconfig.xml";
        public static string m_strSystemConfig = ".\\systemconfig.xml";
        public static bool m_bCloseAutoHomingFinish = false;



        public static bool m_bRecordCardLoadRatio = false;  //Record CardLoadRatio
        public static int m_iRecordCardLoadRatioTimer = 30000;  //預設30秒紀錄一次Card的LoadRatio


        public static bool m_bCommandLogEnable = false;
        public static bool m_bCheckCrdSpcae = false;
        //----------------------------------------------

        public static int m_nUserID = 0;
        public static int m_nRetSpace = 0;

        public enum enPasswordType
        {
            UnLogin = 0,
            Operator = 1,
            Eng = 2,
            ItEng = 3,
            MacEng = 4,
            Maker = 5,
            Designers = 6,
        }

        public struct tyAXIS_XY
        {
            public double X;
            public double Y;
            public void Initial()
            {
                X = Y = 0;
            }
        }
        public struct tyAXIS_XYZ
        {
            public double X;
            public double Y;
            public double Z;
            public void Initial()
            {
                X = Y = Z = 0;
            }
        }

        //--------------------
        public struct tyAXIS_XT
        {
            public double X;
            public double T;
        }
        public struct tyAXIS_XZT
        {
            public double X;
            public double Z;
            public double T;
        }
        public struct tyAXIS_XYT
        {
            public double X;
            public double Y;
            public double T;
        }
        public struct tyAXIS_XYZT
        {
            public double X;
            public double Y;
            public double Z;
            public double T;
        }

        public struct tyAXIS_XYZR
        {
            public double X;
            public double Y;
            public double Z;
            public double R;
            public void Initial()
            {
                X = Y = Z = R = 0;
            }
        }
        public struct tyAXIS_XYZRA
        {
            public double X;
            public double Y;
            public double Z;
            public double R;
            public double A;

            //public string Num;
            //public string Binding;
            public void Initial()
            {
                X = Y = Z = R = A = 0;
                //Num = Binding = "";
            }
        }

        public struct tyAXIS_XXY
        {
            public double X1;
            public double Y;
            public double X2;
        }
        public struct tyAXIS_XXYZ
        {
            public double X1;
            public double Y;
            public double X2;
            public double Z;
        }
        public struct tyAXIS_YZ
        {
            public double Y;
            public double Z;
        }
        public struct tyAXIS_ZS
        {
            public double Z;
            public double Speed;
        }
        public struct tyAXIS_XYZZ
        {
            public double X;
            public double Y;
            public double Z1;
            public double Z2;
        }
        //--------------------

        public enum enMotionBufferStatus
        {
            en_Idle = 0,
            en_Action,
            en_Stop,
            //-----------
            en_LimitOn = 97,
            en_Warning = 98,
            en_Fault = 99,
        }
        public enum enMotionType
        {
            en_Line = 0,
            en_Arc,
            en_3PointArc,
        }
        public enum enMotionCircleMode
        {
            en_XY = 0,
            en_XZ,
            en_YZ,
            en_XYZ,
            en_XYZR,
            en_XYZRA,
        }
        public enum enAixsType
        {
            en_XYZ = 0,
            en_XYZR,
            en_XYZA,
            en_XYZRA,
        }

        public struct tyMotionStatus
        {
            public enMotionBufferStatus MotionStatus;
            public int m_iBlockNum;    //已完成區段數
            public void initial()
            {
                MotionStatus = enMotionBufferStatus.en_Idle;
                m_iBlockNum = 0;
            }
        }

        public struct tyMotionBufferData_XYZRA
        {
            public int ID;
            public enMotionType MotionType;
            public enMotionCircleMode CircleMode;
            public tyAXIS_XYZRA tyMidPosition;
            public tyAXIS_XYZRA tyTargetPosition;
            //----------------
            public double SpeedRate;
            public double AccSpeedRate;
            //----------------
            public bool[] bIOStatus;
            public double dbStart_Delay;
            public double dbEnd_Delay;
            //----------------
            public bool bEndDelay_ActionIOStatus;
            public bool bStartDelay_ActionIOStatus;
            public bool bActionIOStatus;
            public void Initial()
            {
                ID = 0;
                MotionType = enMotionType.en_Line;
                CircleMode = enMotionCircleMode.en_XY;
                //-----------
                tyMidPosition.Initial();
                tyTargetPosition.Initial();
                //-----------
                bActionIOStatus = false;
                //-----------
                bIOStatus = new bool[10];
                for (int i = 0; i < bIOStatus.Length; i++)
                    bIOStatus[i] = false;
                //------------
                SpeedRate = AccSpeedRate = 10.0;
                dbStart_Delay = dbEnd_Delay = 0;
                bStartDelay_ActionIOStatus = bEndDelay_ActionIOStatus = bActionIOStatus = false;
            }
        }


        public static void RecordMessageLog(string strSavedirectoryName, string strMessage)
        {
            try
            {
                DateTime dtNow = DateTime.Now;
                String strPath = System.IO.Directory.GetCurrentDirectory();
                String strNowPath = strPath.Replace("\\bin\\x64\\Debug", "");
                String strRecordPath = strNowPath + "\\Record";
                String strErrorLogPath = strRecordPath + "\\" + strSavedirectoryName;
                String strRecordYear = strErrorLogPath + "\\" + dtNow.Year.ToString();
                String strRecordMonth = strRecordYear + "\\" + string.Format("{0:D2}", dtNow.Month);
                //-------------------------
                System.IO.Directory.CreateDirectory(strRecordPath);
                System.IO.Directory.CreateDirectory(strErrorLogPath);
                System.IO.Directory.CreateDirectory(strRecordYear);
                System.IO.Directory.CreateDirectory(strRecordMonth);
                //-------------------------
                string strTxtFileName = "";
                string strWriteValue = "";

                //-------------------------
                string strRecordDate = dtNow.Year.ToString() + "/" + string.Format("{0:D2}", dtNow.Month) +
                            "/" + string.Format("{0:D2}", dtNow.Day) + "\t";
                string strRecordTime = dtNow.ToLongTimeString() + "\t";
                //-------------------------
                //----------------------------------
                strTxtFileName = dtNow.Year.ToString() + "-" + dtNow.Month.ToString() + "-" + dtNow.Day.ToString();
                strTxtFileName = strTxtFileName + ".txt";
                strTxtFileName = strRecordMonth + "\\" + strTxtFileName;
                //-------------------------------------------      
                strWriteValue = strRecordDate + strRecordTime + strMessage;
                //------------------------
                {
                    System.IO.FileStream fs = new System.IO.FileStream(strTxtFileName,
                    System.IO.FileMode.Create | System.IO.FileMode.Append);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
                    sw.WriteLine(strWriteValue);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }
            catch
            {
                return;
            }

        }

    }
}
