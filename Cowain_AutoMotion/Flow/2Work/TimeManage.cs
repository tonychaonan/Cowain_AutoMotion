using Cowain_Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public class TimeManages
    {
        public static object obj = new object();
        public static List<TimeManage> timeManages = new List<TimeManage>();
        public static void addTimeManage(string SN)
        {
            lock (obj)
            {
                bool b_Exist = false;
                foreach (var item in timeManages)
                {
                    if (item.SN == SN)
                    {
                        b_Exist = true;
                        break;
                    }
                }
                if (b_Exist != true)
                {
                    TimeManage time = new Cowain_AutoMotion.TimeManage(SN);
                    timeManages.Add(time);
                }
                try
                {
                    if (timeManages.Count > 100)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            timeManages.RemoveAt(0);
                        }
                    }
                }
                catch
                {

                }
            }
        }
        public static TimeManage getTimeManage(string SN)
        {
            lock(obj)
            {
                foreach (var item in timeManages)
                {
                    if (item.SN == SN)
                    {
                        return item;
                    }
                }
                return new TimeManage("TEST1234567890");
            }
        }

    }
    public class TimeManage
    {
        public TimeManageItem productInstartTime = new TimeManageItem();
        public TimeManageItem productOutstopTime = new TimeManageItem();
        public TimeManageItem alignmentStartTime = new TimeManageItem();
        public TimeManageItem alignmentEndTime = new TimeManageItem();
        public string SN = "";
        public string holderSN = "";
        public TimeManage(string SN1)
        {
            SN = SN1;
        }
        public string getAlignmentCT()
        {
            double time1 = (alignmentEndTime.time - alignmentStartTime.time).TotalSeconds;
            if (time1 < 0)
            {
                time1 = 0;
            }
            return time1.ToString("f3");
        }
    }
    public class TimeManageItem
    {
        private DateTime time1 = DateTime.Now;
        private DateTime time2 = DateTime.Now;
        private string timeForStr1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        private string timeForJ81 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
        private string timeForStr1ad = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        private string timeForJ81ad = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
        private string timeForStr1adTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        private string timeForJ81adTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
        public DateTime time
        {
            get { return time1; }
        }
        public DateTime time22
        {
            get { return time2; }
        }
        public string timeForStr
        {
            get { return timeForStr1; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string timeForStradd
        {
            get { return timeForStr1ad; }
        }
        /// <summary>
        /// 结束时间j8
        /// </summary>
        public string timeForJ81add
        {
            get { return timeForJ81ad; }
        }

        public string timeForStraddTime
        {
            get { return timeForStr1adTime; }
        }
        /// <summary>
        /// 结束时间j8
        /// </summary>
        public string timeForJ81addTime
        {
            get { return timeForJ81adTime; }
        }
        public string timeForJ8
        {
            get { return timeForJ81; }
        }
        public void refreshTime()
        {
            time1 = DateTime.Now;
            timeForStr1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            timeForJ81 = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
           
        }
        public void refreshTime(DateTime time_1)
        {
            time1 = time_1;
            timeForStr1 = time_1.ToString("yyyy-MM-dd HH:mm:ss");
            timeForJ81 = time_1.ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
        }
        /// <summary>
        /// 结束时间+5
        /// </summary>
        public void refreshTime2()
        {
            //time2 = DateTime.Now;
            if(MachineDataDefine.LADModel==2)
            {
                timeForStr1ad = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                timeForJ81ad = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
            }
            else
            {
                timeForStr1ad = DateTime.Now.AddSeconds(5).ToString("yyyy-MM-dd HH:mm:ss");
                timeForJ81ad = DateTime.Now.AddSeconds(6).ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
            }
           
        }
        /// <summary>
        /// 拍照第一次移动时为timeout节点
        /// </summary>
        public void refreshTime3()
        {
            timeForStr1adTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            timeForJ81adTime = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ss.fff+0800");
        }
        public void retime()
        {
            time2 = DateTime.Now;
        }
    }
}
