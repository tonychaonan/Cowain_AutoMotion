using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Cowain_AutoDispenser
{
    public enum MachieType
    {
        NA,
        杰士德,
        赛腾,
    }
    public class OtherParameter:JsonHelper
    {
  

        //--------------------------NG产品自动出料
       
        [Category("5复检OK指令"), DisplayName("复检OK指令开关"), Description("true为启用，false 为禁用")]
        public bool b_EnableRecheckOK
        {
            get;
            set;
        } = false;
        [Category("6AB胶排完胶不抬起Z轴功能"), DisplayName("AB胶站位功能"), Description("为true时，排完胶不抬起到安全位，直接擦胶")]
        public bool ABGlueNOUpZ
        {
            get;
            set;
        } = false;
       
     
       
    }
}
