using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public enum StepEnum
    {
        Null,
        条件,
        步骤,
    }
    public enum IfStep
    {
        Null,
        等待输入IO,
        等待连续触发,
        等待TCP赋值不等于空,
        等待TCP赋值包含固定指令,
        等待RS232赋值不等于空,
        等待RS232赋值包含固定指令,
    }
    public enum ExecuteStep
    {
        Null,
        延时,
        执行输入IO,
        执行输出IO,
        TCP字符串赋值,
        RS232字符串赋值,
    }
}
