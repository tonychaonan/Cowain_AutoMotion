using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotionBase
{
    public class ErrorDefine
    {
        // 1 ~ 5000 -->Machine用 ; 5001 ~ 30000 -->各站使用 ; 30000 -->控制元件使用
        public enum enErrorCode
        {   
            程式未Initial  =        -1,
            //---------------------------
            無異常         =         0,
            //---------------------------
            Initial中      =        100,
            CardOPen       =        101,
            LoadingData    =        102,
            CheckVerifykey =        103,
            CheckPassword  =        104,
            //---------------------------       
            Initial成功    =       1001,
            Initial失敗    =       1002,
            軸卡Initial失敗=       1003,
            IO_Data讀取失敗 =      1004,
            Machine_Data讀取失敗  =1005,
            //---------------------------
            //---------------------------
            电磁阀Open逾时 = 30100,
            电磁阀Close逾时 = 30101,

            //---------------------------
            电机驱动器异常 = 31000,
            电机未Servo_On = 31001,
            电机正极限_On = 31002,
            电机负极限_On = 31003,
            电机未Motion_Done = 31004,
            电机回Home逾时 = 31005,
            电机Repeat移动逾时 = 31006,
            电机Abs位置移动逾时 = 31007,
            电机Rev位置移动逾时 = 31008,
            位置超出软体极限 = 31009,
            //---------------------------
            MotionBuffer动做中电机驱动器异常 = 32000,
            SetMotionBufferData异常 = 32001,
            CleraBuffer异常 = 32002,

        }
    }
}
