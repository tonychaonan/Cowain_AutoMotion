using Cowain_Form.FormView;
using Cowain_Machine.Flow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.Flow
{
    /// <summary>
    /// 运行过程中保存的数据
    /// </summary>
    public class RunDataDefine
    {

        /// <summary>
        /// 运行过程中保存的数据
        /// </summary>
        public static RunDataDefineClass RunDataS = new RunDataDefineClass();
       
        private static Thread th;
 
        public  RunDataDefine()
        {
          
        }
        public static void ReadParams()
        {          
            RunDataS.ReaderParams(Program.StrBaseDic, "RunDataS", ref RunDataS);
            if (RunDataS == null)
            {
                RunDataS = new RunDataDefineClass();
                RunDataS.ReadBufferDate(Program.StrBaseDic, "RunDataS", ref RunDataS);
            }
            RunDataS.SetSaveFile(Program.StrBaseDic, "RunDataS", RunDataS);
            
        }

   

        /// <summary>
        /// 初始化预先参数
        /// </summary>
        public static void InitData()
        {
           // DispenserDataDefine.DispenserDataS.GlueDateS.DouDisGlueNow = RunDataS.DouDisGlueNow;
        }

    }
    public class RunDataDefineClass : JsonHelper
    {
        /// <summary>
        /// 胶水已经使用量(专门用来保存的)
        /// </summary>
        public double[] DouDisGlueNow = new double[] { 1, 1 };
        
        /// <summary>
        /// 胶水出胶次数（1.正常做料算1次 2.自动排胶算1次 3.称胶时算1次）
        /// </summary>
        public int[] PurgeIDX = new int[] { 1, 1 };

    }


}
