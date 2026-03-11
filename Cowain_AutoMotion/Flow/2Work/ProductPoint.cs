using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion.Flow._2Work
{
  public  class ProductPoint
    {
        public string SN = "TEST";
        public string UC = "";
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime startTime = DateTime.Now;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime = new DateTime();
        /// <summary>
        /// 上一次的结束时间
        /// </summary>
        public DateTime lastEndTime = DateTime.Now;
        public bool pass = false;
        public List<data> datas = new List<data>();
        /// <summary>
        /// 压缩包名称
        /// </summary>
        public string ZIPName = "";
        /// <summary>
        /// 压缩包全路径
        /// </summary>
        public string fullFileName = "";
    }
    public class data
    {
        public string test;
        public double value;
        public double lower_limit;
        public double upper_limit;
    }
}
