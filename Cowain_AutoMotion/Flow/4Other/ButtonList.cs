using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public class ButtonList
    {
        public List<ButtonParam> buttonParams = new List<ButtonParam>();
        public static object obj = new object();
        public enum ButtonType
        {
            开始按钮,
            暂停按钮,
            停止按钮,
            开始按钮灯,
            暂停按钮灯,
            停止按钮灯,
            安全光幕,
            安全门,
            急停按钮,
            三色灯_绿,
            三色灯_黄,
            三色灯_红,
            三色灯_蜂鸣器,
        }
        public ButtonList()
        {
            refresh();
        }
        public bool getButtonStatus(ButtonType buttonType)
        {
            lock (obj)
            {
                bool b_Result = false;
                switch (buttonType)
                {
                    case ButtonType.开始按钮:
                    case ButtonType.暂停按钮:
                    case ButtonType.停止按钮:
                    case ButtonType.急停按钮:
                        foreach (var item in buttonParams)
                        {
                            EnumParam_InputIO enumParam_InputIO;
                            bool b_Convert = Enum.TryParse(item.startButton, out enumParam_InputIO);
                            if (b_Convert)
                            {
                                bool b_Value = HardWareControl.getInputIO(enumParam_InputIO).GetValue();
                                if (b_Value)
                                {
                                    b_Result = true;
                                }
                            }
                        }
                        break;
                    case ButtonType.开始按钮灯:
                    case ButtonType.暂停按钮灯:
                    case ButtonType.停止按钮灯:
                    case ButtonType.三色灯_绿:
                    case ButtonType.三色灯_黄:
                    case ButtonType.三色灯_红:
                    case ButtonType.三色灯_蜂鸣器:
                        foreach (var item in buttonParams)
                        {
                            EnumParam_OutputIO enumParam_InputIO;
                            bool b_Convert = Enum.TryParse(item.startButton, out enumParam_InputIO);
                            if (b_Convert)
                            {
                                bool b_Value = HardWareControl.getOutputIO(enumParam_InputIO).GetValue();
                                if (b_Value)
                                {
                                    b_Result = true;
                                }
                            }
                        }
                        break;
                    case ButtonType.安全光幕:
                    case ButtonType.安全门:
                        b_Result = true;
                        foreach (var item in buttonParams)
                        {
                            EnumParam_InputIO enumParam_InputIO;
                            bool b_Convert = Enum.TryParse(item.startButton, out enumParam_InputIO);
                            if (b_Convert)
                            {
                                bool b_Value = HardWareControl.getInputIO(enumParam_InputIO).GetValue();
                                if (b_Value != true)
                                {
                                    b_Result = false;
                                }
                            }
                        }
                        break;
                }
                return b_Result;
            }
        }
        public void setButtonStatus(ButtonType buttonType, bool value)
        {
            lock(obj)
            {
                switch (buttonType)
                {
                    case ButtonType.开始按钮灯:
                    case ButtonType.暂停按钮灯:
                    case ButtonType.停止按钮灯:
                    case ButtonType.三色灯_绿:
                    case ButtonType.三色灯_黄:
                    case ButtonType.三色灯_红:
                    case ButtonType.三色灯_蜂鸣器:
                        foreach (var item in buttonParams)
                        {
                            EnumParam_OutputIO enumParam_InputIO;
                            bool b_Convert = Enum.TryParse(item.startButton, out enumParam_InputIO);
                            if (b_Convert)
                            {
                                HardWareControl.getOutputIO(enumParam_InputIO).SetIO(value);
                            }
                        }
                        break;
                }
            }
        }
        public void refresh()
        {
            lock(obj)
            {
                buttonParams = SQLSugarHelper.DBContext<ButtonParam>.GetInstance().GetList();
                if(buttonParams.Count==0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        ButtonParam buttonParam = new ButtonParam();
                        buttonParam.ID = i.ToString();
                        SQLSugarHelper.DBContext<ButtonParam>.GetInstance().Insert(buttonParam);
                    }
                }
            }
        }
    }
}
