using System;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Threading.Tasks;

namespace Cowain
{
    public class CTimer
    {
        /// <summary>
        /// 延迟执行时间
        /// </summary>
        private int Interval { get; set; } = 1;
        public bool Enabled { get; set; }
        public Action Callback { get; set; } 
        public DateTime Time { get; set; }
        private void Start_()
        {
            Enabled = true;
            Time = DateTime.Now;
            Task.Run((() =>
            {
                while (Enabled)
                {
                    if ((DateTime.Now - Time).TotalMilliseconds >= Interval)
                    {
                        Enabled = false;
                        Callback?.Invoke();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
            }));
        }
        public void Start(int Interval_)
        {
            Enabled = true;
            Interval = Interval_;
            Start_();
        }
        public void Stop() 
        {
            Enabled = false;
        }
    }
}