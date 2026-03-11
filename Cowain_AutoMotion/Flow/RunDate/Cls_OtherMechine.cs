using Cowain_Machine.Flow;
using HslCommunication;
using HslCommunication.ModBus;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cowain_AutoDispenser.Flow
{
    public class Cls_OtherMechine
    {
        public OmronFinsUdp OmronFinsUdp = null;

        public bool BConnect { get; private set; }
        public bool bError;
        Thread tReadBool;
       public int result = 0;
        string IP; int port; string address; bool b_Enable; MachieType machineType; int holderCount;
        public Cls_OtherMechine(string IP1, int port1, string address1, bool b_Enable1, MachieType machineType1,int holderCount1)
        {
            IP = IP1;
            port = port1;
            address = address1;
            b_Enable = b_Enable1;
            machineType = machineType1;
            holderCount = holderCount1;
            if (b_Enable)
            {
                Connect();
                InitializeHashtable();
            }
        }
        public void InitializeHashtable()
        {
            BConnect = false;
            tReadBool = new Thread(Read) { IsBackground = true };
            tReadBool.Start();
        }
        public bool getResult()
        {
            if(b_Enable!=true)
            {
                return true;
            }
            bool b_Result = false;
            if(machineType== MachieType.NA)
            {
                b_Result = true;
            }
            else if (machineType == MachieType.杰士德)
            {
                if(result==1)
                {
                    b_Result = true;
                }
                else
                {
                    b_Result = false;
                }
            }
            else if (machineType == MachieType.赛腾)
            {
                if(result> holderCount)
                {
                    b_Result = true;
                }
                else
                {
                    b_Result = false;
                }
            }
            return b_Result;
        }
        private void Connect()
        {
            if (b_Enable != true)
            {
                return;
            }
            try
            {
                OmronFinsUdp = new OmronFinsUdp(IP, port);
                BConnect = false;
                BConnect = OmronFinsUdp.ReadInt16(address, 1).IsSuccess;
            }
            catch (Exception strEx)
            {
                if (!bError)
                {
                    bError = true;
                }
                if (BConnect)
                {
                    BConnect = false;
                }
            }
        }
        int Step = 0;
        public void Read()
        {
            while (true)
            {
                Thread.Sleep(1);
                if(b_Enable!=true)
                {
                    return;
                }
                try
                {
                    switch (Step)
                    {
                        case 0:
                            BConnect = OmronFinsUdp.ReadInt16(address, 1).IsSuccess;
                            Step = 1;
                            Thread.Sleep(500);
                            break;
                        case 1:
                            if (BConnect)
                            {
                                try
                                {
                                    result = OmronFinsUdp.ReadInt16(address).Content;
                                }
                                catch
                                {

                                }
                            }
                            Step = 0;
                            Thread.Sleep(500);
                            break;
                    }
                }
                catch (Exception strEx)
                {
                    if (!bError)
                    {
                        bError = true;
                    }
                }
            }
        }
    }

}
