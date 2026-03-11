using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using Cowain_AutoMotion;
using OmronFinsUI;
using System.Drawing.Drawing2D;
using MotionBase;
using Cowain_Machine.Flow;

namespace Cowain_Form.FormView
{
    public partial class frm_Sensor : DevExpress.XtraEditors.XtraForm
    {
        public frm_Sensor(ref clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
            changeColor = new ChangeColor(Change);
        }

        #region 自定义变量
        /// <summary>
        /// 界面刷新线程
        /// </summary>
        Thread ReFlash_Thread;
        DrvIO pRefshIO;
        DrvIO.tyIO_Parameter IOParameter = new DrvIO.tyIO_Parameter();

        Dictionary<string, Base> showIOList;
        public clsMachine pMachine;
        #endregion

        #region 自定义方法
        /// <summary>
        /// 设置气缸label颜色
        /// </summary>
        /// <param name="alarm">异常信号</param>
        /// <param name="origin">静点信号</param>
        /// <param name="mobile">动点信号</param>
        /// <param name="lab">显示label</param>
        private void CylinderSensor(bool alarm,bool origin,bool mobile, LabelControl lab)
        {
            if (alarm)
            {
                if (lab.BackColor != Color.Red)
                {
                    Invoke(new Action(() => changeColor(lab, Color.Red)));
                    //lab.BackColor = Color.Red;
                }
            }
            else
            {
                if (mobile)
                {
                    if (lab.BackColor != Color.Lime)
                    {
                        Invoke(new Action(() => changeColor(lab, Color.Lime)));
                        //lab.BackColor = Color.Lime;
                    }
                }
                else if (lab.BackColor != Color.Gray)
                {
                    Invoke(new Action(() => changeColor(lab, Color.Gray)));
                    //lab.BackColor = Color.Gray;
                }
            }            
        }

        delegate void ChangeColor(LabelControl lab, Color color);
        ChangeColor changeColor;

        public void Change(LabelControl lab, Color color)
        {
            lab.BackColor = color;
        }

        /// <summary>
        /// 设置感应器是否有信号
        /// </summary>
        /// <param name="alarm">异常信号</param>
        /// <param name="bInput">输入信号</param>
        /// <param name="lab">显示label</param>
        private void IOSensor(bool alarm, bool bInput, LabelControl lab)
        {
            if(alarm)
            {
                if (lab.BackColor != Color.Red)
                {
                    lab.BackColor = Color.Red;
                }
            }
            else
            {
                if (bInput && lab.BackColor != Color.Lime)
                {
                    lab.BackColor = Color.Lime;
                }
                else if (!bInput && lab.BackColor != Color.Gray)
                {
                    lab.BackColor = Color.Gray;
                }
            }            
        }


        private void DoReflash()
        {
            while (true)
            {
                try
                {
                    //#region 外部流道
                    //if (frm_Main.frm_Conveyor.CV.BConnect)
                    //{
                    //    #region A流道
                    //    //A1
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M410_流道A阻挡气缸1报警], frm_Main.frm_Conveyor.CV.baInput[10], frm_Main.frm_Conveyor.CV.baInput[11], labM300);
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M413_流道A升降气缸报警], frm_Main.frm_Conveyor.CV.baInput[16], frm_Main.frm_Conveyor.CV.baInput[17], labM303);
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M414_流道A推杆气缸报警], frm_Main.frm_Conveyor.CV.baInput[18], frm_Main.frm_Conveyor.CV.baInput[19], labM304);
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[4], labX4);//到位
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[5], labX5);//离开

                    //    //A2
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M411_流道A阻挡气缸2报警], frm_Main.frm_Conveyor.CV.baInput[12], frm_Main.frm_Conveyor.CV.baInput[13], labM301);
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M450_超时预留1], frm_Main.frm_Conveyor.CV.baInput[6], labX6);//到位
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M450_超时预留1], frm_Main.frm_Conveyor.CV.baInput[7], labX7);//离开
                    //    //A3
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M412_流道A阻挡气缸3报警], frm_Main.frm_Conveyor.CV.baInput[14], frm_Main.frm_Conveyor.CV.baInput[15], labM302);
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M451_流道B阻挡1到流道B升降位卡料], frm_Main.frm_Conveyor.CV.baInput[8], labX10);//到位
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M452_流道B阻挡2到下工站阻挡1卡料], frm_Main.frm_Conveyor.CV.baInput[9], labX11);//离开
                    //    #endregion

                    //    #region B流道
                    //    //B1
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M426_扫码位挡停报警], frm_Main.frm_Conveyor.CV.baInput[25], frm_Main.frm_Conveyor.CV.baInput[26], labM305);
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[20], labX35);//到位
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[21], labX36);//离开
                    //    //B2
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M416_流道B升降气缸报警], frm_Main.frm_Conveyor.CV.baInput[31], frm_Main.frm_Conveyor.CV.baInput[32], labM327);//73 74
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M447_流道B升降载具到位超时], frm_Main.frm_Conveyor.CV.baInput[29], labX27);//到位
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M453_流道A阻挡1到下流道A阻挡2卡料], frm_Main.frm_Conveyor.CV.baInput[30], labX30);//离开
                    //    //B3
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M415_流道B阻挡气缸报警], frm_Main.frm_Conveyor.CV.baInput[27], frm_Main.frm_Conveyor.CV.baInput[28], labM306);
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M440_流道B阻挡位载具到位超时], frm_Main.frm_Conveyor.CV.baInput[23], labX24);//到位
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M440_流道B阻挡位载具到位超时], frm_Main.frm_Conveyor.CV.baInput[24], labX25);//离开
                    //    #endregion

                    //    #region C流道
                    //    //C
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M417_流道C阻挡气缸1报警], frm_Main.frm_Conveyor.CV.baInput[42], frm_Main.frm_Conveyor.CV.baInput[43], labM307);
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M418_流道C升降气缸报警], frm_Main.frm_Conveyor.CV.baInput[44], frm_Main.frm_Conveyor.CV.baInput[45], labM308);
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M441_流道C升降位载具到位超时], frm_Main.frm_Conveyor.CV.baInput[36], labX10000);//到位
                    //    //C1
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M419_流道C阻挡气缸2报警], frm_Main.frm_Conveyor.CV.baInput[46], frm_Main.frm_Conveyor.CV.baInput[47], labM309);
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M420_流道C进料1升降气缸报警], frm_Main.frm_Conveyor.CV.baInput[48], frm_Main.frm_Conveyor.CV.baInput[49], labM310);
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M442_流道C工位1载具到位超时], frm_Main.frm_Conveyor.CV.baInput[37], labX10001);//到位
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M448_流道C进料1卡料], frm_Main.frm_Conveyor.CV.baInput[40], labX10004);//离开进入机台
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M448_流道C进料1卡料], frm_Main.frm_Conveyor.CV.baInput[38], labX10002);//离开进入工位2
                    //    //C2
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M421_流道C进料2升降气缸报警], frm_Main.frm_Conveyor.CV.baInput[50], frm_Main.frm_Conveyor.CV.baInput[51], labM311);
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M443_流道C进料2载具到位超时], frm_Main.frm_Conveyor.CV.baInput[39], labX10003);//到位
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M449_流道C进料2卡料], frm_Main.frm_Conveyor.CV.baInput[41], labX10005);//离开进入机台
                    //    #endregion

                    //    #region D流道
                    //    //D1
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M422_流道D阻挡气缸1报警], frm_Main.frm_Conveyor.CV.baInput[58], frm_Main.frm_Conveyor.CV.baInput[59], labM312);
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M424_流道D出料1升降气缸报警], frm_Main.frm_Conveyor.CV.baInput[62], frm_Main.frm_Conveyor.CV.baInput[63], labM314);
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M444_流道D出料1机台载具到位超时], frm_Main.frm_Conveyor.CV.baInput[54], labX10102);//到位
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M444_流道D出料1机台载具到位超时], frm_Main.frm_Conveyor.CV.baInput[55], labX10103);//离开
                    //    //D2
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M423_流道D阻挡气缸2报警], frm_Main.frm_Conveyor.CV.baInput[60], frm_Main.frm_Conveyor.CV.baInput[61], labM313);
                    //    CylinderSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M425_流道D出料2升降气缸报警], frm_Main.frm_Conveyor.CV.baInput[64], frm_Main.frm_Conveyor.CV.baInput[65], labM315);
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M446_流道D出料2机台载具到位超时], frm_Main.frm_Conveyor.CV.baInput[52], labX10100);//到位
                    //    IOSensor(frm_Main.frm_Conveyor.CV.baAlarm[(int)EnumAlarm.M445_流道D出料2向出料1放料超时], frm_Main.frm_Conveyor.CV.baInput[53], labX10101);//离开
                    //    #endregion

                    //    #region 点位
                    //    //A1
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[4], labA1);//到位
                    //    //A2
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[6], labA2);//到位
                    //    //A3
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[8], labA3);//到位

                    //    //B1
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[20], labB1);//到位
                    //    //B2
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[29], labB2);//到位
                    //    //B3
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[23], labB3);//到位

                    //    //C
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[36], labC);//到位
                    //    //C1
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[37], labC1);//到位
                    //    //C2
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[39], labC2);//到位

                    //    //D1
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[54], labD1);//到位
                    //    //D2
                    //    IOSensor(false, frm_Main.frm_Conveyor.CV.baInput[52], labD2);//到位
                    //    #endregion
                    //}
                    #endregion

                    #region 内部流道
                    //int nRetCode = 0;
                    //bool leftOpenSR = false;//左流道扫码阻挡气缸动点信号
                    //bool leftCloseSR = false;//左流道扫码阻挡气缸静点信号
                    //bool rightOpenSR = false;//右流道扫码阻挡气缸动点信号
                    //bool rightCloseSR = false;//右流道扫码阻挡气缸静点信号
                    //if (pMachine.GetInitialStatus(ref nRetCode))
                    //{
                    //    for (int i = 0; i < showIOList.Count; i++)
                    //    {
                    //        pRefshIO = (DrvIO)showIOList[i].m_NowAddress;
                    //        pRefshIO.GetParameter(ref IOParameter);

                    //        #region 左流道C1-D1

                    //        if (IOParameter.strID.Equals("X0200"))
                    //        {
                    //            //扫码阻挡气缸动点信号
                    //            leftOpenSR = pRefshIO.GetValue();
                    //        }
                    //        else if (IOParameter.strID.Equals("X0201"))
                    //        {
                    //            //扫码阻挡气缸动点信号
                    //            leftCloseSR = pRefshIO.GetValue();
                    //        }
                    //        else if (IOParameter.strID.Equals("X0212"))
                    //        {
                    //            //扫码到位
                    //            IOSensor(false, pRefshIO.GetValue(), labX0212);
                    //        }
                    //        else if (IOParameter.strID.Equals("X0213"))
                    //        {
                    //            //CCD到位
                    //            IOSensor(false, pRefshIO.GetValue(), labX0213);
                    //        }
                    //        else if (IOParameter.strID.Equals("X0214"))
                    //        {
                    //            //出料信号
                    //            IOSensor(false, pRefshIO.GetValue(), labX0214);
                    //        }
                    //        else if (IOParameter.strID.Equals("Y1200"))
                    //        {
                    //            //扫码阻挡气缸
                    //            IOSensor(leftOpenSR == leftCloseSR ? true : false, pRefshIO.GetValue(), labY1200);
                    //        }
                    //        #endregion

                    //        #region 右流道C2-D2
                    //        else if (IOParameter.strID.Equals("X0300"))
                    //        {
                    //            //扫码阻挡气缸动点信号
                    //            rightOpenSR = pRefshIO.GetValue();
                    //        }
                    //        else if (IOParameter.strID.Equals("X0301"))
                    //        {
                    //            //扫码阻挡气缸动点信号
                    //            rightCloseSR = pRefshIO.GetValue();
                    //        }
                    //        else if (IOParameter.strID.Equals("X0312"))
                    //        {
                    //            //扫码到位
                    //            IOSensor(false, pRefshIO.GetValue(), labX0312);
                    //        }
                    //        else if (IOParameter.strID.Equals("X0313"))
                    //        {
                    //            //CCD到位
                    //            IOSensor(false, pRefshIO.GetValue(), labX0313);
                    //        }
                    //        else if (IOParameter.strID.Equals("X0314"))
                    //        {
                    //            //出料信号
                    //            IOSensor(false, pRefshIO.GetValue(), labX0314);
                    //        }
                    //        else if (IOParameter.strID.Equals("Y1300"))
                    //        {
                    //            //扫码阻挡气缸
                    //            IOSensor(rightOpenSR == rightCloseSR ? true : false, pRefshIO.GetValue(), labY1300);
                    //            //pSelectIO.SetIO(!bValue);
                    //        }
                    //        #endregion
                    //    }
                    //}
                    #endregion

                    Thread.Sleep(500);
                }
                catch (Exception err)
                {
                    
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void Draw()
        {
            GraphicsPath my = new GraphicsPath();
            my.AddEllipse(new Rectangle(0, 0, 48, 48));

            this.labA1.Region = new Region(my);
            this.labA2.Region = new Region(my);
            this.labA3.Region = new Region(my);
            this.labB1.Region = new Region(my);
            this.labB2.Region = new Region(my);
            this.labB3.Region = new Region(my);
            this.labC.Region = new Region(my);
            this.labC1.Region = new Region(my);
            this.labC2.Region = new Region(my);
            this.labD1.Region = new Region(my);
            this.labD2.Region = new Region(my);
        }

        private void frm_Sensor_Load(object sender, EventArgs e)
        {
            Draw();
            ReFlash_Thread = new Thread(DoReflash);
            ReFlash_Thread.Priority = ThreadPriority.Lowest;
            ReFlash_Thread.IsBackground = true;
            ReFlash_Thread.Start();

            if (pMachine != null)
            {
              //  showIOList = pMachine.GetIOList();
            }
        }

        private void lab_MouseDown(object sender, MouseEventArgs e)
        {
            if ((int)pMachine.m_LoginUser <= 1)
            {
                //LV1及以下权限无法操作
                return;
            }
        }

        private void lab_MouseUp(object sender, MouseEventArgs e)
        {
            if ((int)pMachine.m_LoginUser <= 1)
            {
                //LV1及以下权限无法操作
                return;
            }
        }

        private void labY1200_Click(object sender, EventArgs e)
        {
            if ((int)pMachine.m_LoginUser <= 1 || showIOList == null)
            {
                //LV1及以下权限无法操作
                return;
            }
            for (int i = 0; i < showIOList.Count; i++)
            {
                pRefshIO = (DrvIO)showIOList[showIOList.Keys.ToList()[i]].m_NowAddress;
                pRefshIO.GetParameter(ref IOParameter);

                if (IOParameter.strID.Equals("Y1200"))
                {
                    pRefshIO.SetIO(!pRefshIO.GetValue());
                    break;
                }
            }
        }

        private void labY1300_Click(object sender, EventArgs e)
        {
            if ((int)pMachine.m_LoginUser <= 1 || showIOList == null)
            {
                //LV1及以下权限无法操作
                return;
            }
            for (int i = 0; i < showIOList.Count; i++)
            {
                pRefshIO = (DrvIO)showIOList[showIOList.Keys.ToList()[i]].m_NowAddress;
                pRefshIO.GetParameter(ref IOParameter);

                if (IOParameter.strID.Equals("Y1300"))
                {
                    pRefshIO.SetIO(!pRefshIO.GetValue());
                    break;
                }
            }
        }
    }
}