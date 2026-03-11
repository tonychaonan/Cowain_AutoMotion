using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion.Flow.Common
{
    public class Monitor
    {
        private PerformanceCounter cpu = null;
        private PerformanceCounter raw = null;
        private Process ps = Process.GetCurrentProcess();
        public Monitor()
        {
            //raw = new PerformanceCounter("Process", "Working Set - Private", ps.ProcessName);
            //cpu = new PerformanceCounter("Process", "% Processor Time", ps.ProcessName);
        
          
        }
        public float GetCpu()
        {
            return cpu.NextValue();
        }
        public float GetRaw()
        {
            return raw.NextValue() / 1048576f;
        }
    }
}
