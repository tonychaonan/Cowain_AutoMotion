using Cowain_AutoMotion.Flow;
using Cowain_Form.FormView;
using Cowain_Machine;
using Cowain_Machine.Flow;
using MotionBase;
using Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Cowain_Machine.Flow.MErrorDefine;

namespace Cowain_AutoMotion
{
    public class OtherMachine
    {
        //Thread thread;
        SocketSever socketSever;
        public OtherMachine()
        {
            //thread = new Thread(work);
            //thread.IsBackground = true;
            //thread.Start();
            socketSever = new SocketSever("192.168.250.10", "800");
            socketSever.ReceiveMessageEvent += work;
        }
        public void work(string msg)
        {
            if (msg.Contains("Request_Work"))
            {
                if (frm_Main.formData?.ChartTime1.RunStatus == Chart.ChartTime.MachineStatus.error_down || MachineDataDefine.RobotDownError==true || MachineDataDefine.StopNG)
                {
                    socketSever.Send("NG\r\n");
                    //ConnectionControl.getSocketControl(EnumParam_ConnectionName.串机).SendMsg("NG\r\n");
                    if (frm_Main.formData?.ChartTime1.RunStatus == Chart.ChartTime.MachineStatus.error_down)
                    {
                        LogAuto.SaveConnetLog("接受到791询问信号:" + "H850设备发送宕机信号");
                    }
                   else if(MachineDataDefine.StopNG)
                     {
                        LogAuto.SaveConnetLog("接受到791询问信号:" + "H850设备连三不连五报警");
                    }
                    else
                    {
                        LogAuto.SaveConnetLog("接受到791询问信号:" + "H860设备发送宕机信号");
                    }
                }
                else
                {
                    socketSever.Send("OK\r\n");
                    LogAuto.SaveConnetLog("发送设备正常运行信号");
                }
            }
            #region
            //while (true)
            //{
            //    Thread.Sleep(Convert.ToInt32(0.5));
            //    if (ConnectionControl.getSocketControl(EnumParam_ConnectionName.串机).StrBack.Trim().Contains("start"))
            //    {
            //        ConnectionControl.getSocketControl(EnumParam_ConnectionName.串机).StrBack = "";
            //        if (frm_Main.formData?.ChartTime1.RunStatus == Chart.ChartTime.MachineStatus.error_down)
            //        {
            //            ConnectionControl.getSocketControl(EnumParam_ConnectionName.串机).SendMsg("NG\r\n");
            //        }
            //        else
            //        {
            //            ConnectionControl.getSocketControl(EnumParam_ConnectionName.串机).SendMsg("OK\r\n");
            //        }
            //    }
            //}
            #endregion
        }
    }
}
