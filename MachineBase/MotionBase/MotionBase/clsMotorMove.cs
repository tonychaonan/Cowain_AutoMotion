using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.PTG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotionBase
{
    public class clsMotorMove
    {
        Thread Thread;
        bool busy = false;
        double pos = 0;
        double speed = 0;
        public bool alarm = false;
        private DateTime now1 = DateTime.Now;
        private double currentPos = 0;
        public double MEL = -9999;
        public double PEL = 9999;
        public bool b_MEL = false;
        public bool b_PEL = false;
        public object obj = new object();
        public string errStr = "";
        public bool b_SevOn = false;
        private double AccTime = 0.1;
        private double DccTime = 0.1;
        private double HighTime = 0.1;
        private double Acc = 0;
        private double Dcc = 0;
        public clsMotorMove()
        {
            Thread = new Thread(run);
            Thread.IsBackground = true;
            Thread.Start();
        }
        private void run()
        {
            while (true)
            {
                Thread.Sleep(5);
                if (busy != true)
                {
                    continue;
                }
                if (currentPos == pos)
                {
                    busy = false;
                    continue;
                }
                //执行加速度
                DateTime accdateTime11 = now1.AddSeconds(AccTime);
                while ((accdateTime11 - now1).TotalMilliseconds > 0&& busy)
                {
                    double time = (DateTime.Now - now1).TotalSeconds;
                    double pos22 = 1 / 2.0 * Acc * time * time;
                    addDistance(pos22);
                    Thread.Sleep(5);
                }
                //执行运行速度
                now1 = DateTime.Now;
                DateTime hightdateTime11 = now1.AddSeconds(AccTime+HighTime);
                while ((hightdateTime11 - now1).TotalMilliseconds > 0 && busy)
                {
                    double time = (DateTime.Now - now1).TotalSeconds;
                    double pos22 = 1 / 2.0 * speed * time * time;
                    addDistance(pos22);
                    Thread.Sleep(5);
                }
                //执行减速度
                now1 = DateTime.Now;
                DateTime dccdateTime11 = now1.AddSeconds(AccTime + HighTime+DccTime);
                while ((dccdateTime11 - now1).TotalMilliseconds > 0 && busy)
                {
                    double time = (DateTime.Now - now1).TotalSeconds;
                    double pos22 = 1 / 2.0 * Dcc * time * time;
                    addDistance(pos22);
                    Thread.Sleep(5);
                }
                if(b_MEL||b_PEL||alarm)
                {

                }
                else
                {
                    currentPos = pos;
                }
            }
        }
        private void addDistance(double distance1)
        {
            if (pos >= currentPos)
            {
                if ((currentPos + distance1) >= pos)
                {
                    currentPos = pos;
                    busy = false;
                }
                else
                {
                    currentPos = currentPos + distance1;
                }
            }
            else
            {
                if ((currentPos - distance1) <= pos)
                {
                    currentPos = pos;
                    busy = false;
                }
                else
                {
                    currentPos = currentPos - distance1;
                }
            }
            if (currentPos < MEL)
            {
                b_MEL = true;
                busy = false;
            }
            else if(currentPos > PEL)
            {
                busy = false;
                b_PEL = true;
            }
        }
        public void absMove(double pos1, double speed1, double Acc1, double Dcc1)
        {
            errStr = "";
            if (busy || b_MEL || b_PEL || b_SevOn != true)
            {
                if (busy)
                {
                    errStr = "轴在运动过程中，再次触发运动";
                }
                if (b_MEL)
                {
                    errStr += "轴在负极限";
                }
                if (b_PEL)
                {
                    errStr += "轴在正极限";
                }
                if (b_SevOn != true)
                {
                    errStr += "轴未励磁";
                }
                alarm = true;
                return;
            }
            getRunTime(Math.Abs(pos1 - currentPos), speed1, Acc1, Dcc1);
            pos = pos1;
            speed = speed1;
            now1 = DateTime.Now;
            busy = true;
        }
        public void relMove(double pos1, double speed1, double Acc1, double Dcc1)
        {
            errStr = "";
            if (busy || b_MEL || b_PEL || b_SevOn != true)
            {
                if (busy)
                {
                    errStr = "轴在运动过程中，再次触发运动";
                }
                if (b_MEL)
                {
                    errStr += "轴在负极限";
                }
                if (b_PEL)
                {
                    errStr += "轴在正极限";
                }
                if (b_SevOn != true)
                {
                    errStr += "轴未励磁";
                }
                alarm = true;
                return;
            }
            getRunTime(Math.Abs(pos1), speed1, Acc1, Dcc1);
            pos = pos1 + currentPos;
            speed = speed1;
            now1 = DateTime.Now;
            busy = true;
        }
        private void getRunTime(double distance, double speed1, double Acc1, double Dcc1)
        {
            //如果算上加减速的距离小于distance
             Acc = speed1 / Acc1;//加速度
             Dcc = speed1 / Dcc1;//减速度
            double accDis = 1 / 2.0 * Acc * Acc1 * Acc1;
            double dccDis = 1 / 2.0 * Dcc * Dcc1 * Dcc1;
            if ((accDis + dccDis) < distance)
            {
                AccTime = Acc1;
                DccTime = Dcc1;
                HighTime = (distance - accDis - dccDis) / speed1;
            }
            else
            {
                AccTime = Math.Sqrt(distance / 2 * 2 / Acc);
                DccTime = AccTime;
                HighTime = 0;
            }
        }
        public void resetAlarm()
        {
            alarm = false;
            b_MEL = false;
            b_PEL = false;
        }
        public double getCurrentPos()
        {
            lock (obj)
            {
                return currentPos;
            }
        }
        public void setCurrentPos(double pos)
        {
            lock (obj)
            {
                currentPos = pos;
            }
        }
        public bool isMotionDone()
        {
            return !busy;
        }
        public void stopMotor()
        {
            busy = false;
        }
    }
}
