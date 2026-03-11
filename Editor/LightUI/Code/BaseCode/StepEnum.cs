using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain
{
    public enum ActionEnum
    {
        执行,
        等待,
        无,
    }
    public enum StepEnum
    {
        初始化轴,
        初始化IO卡,
        读取输入IO,
        读取输出IO,
        设置关输出IO,
        设置开输出IO,
        回原点,
        绝对运动,
        相对运动,
        停止运动,
        电机励磁,
        电机断磁,
        延时,
        结果,
        流程1,
        流程2,
        流程3,
        流程4,
        流程5,
        流程6,
        流程7,
        指令,
        分支,
    }
}
