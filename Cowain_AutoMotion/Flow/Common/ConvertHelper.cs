using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 将值转换成string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string GetDef_Str(object obj,string def="")
        {
            string val = def;
            try
            {
                val = obj.ToString();
            }
            catch
            {

            }
            return val;
        }

        /// <summary>
        /// 将值转换成Int
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static int GetDef_Int(object obj, int def=0)
        {
            int val = def;
            try
            {
                val = Convert.ToInt32(obj.ToString());
            }
            catch(Exception err)
            {

            }
            return val;
        }

        /// <summary>
        /// 将值转换成decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static decimal GetDef_Dec(object obj, decimal def = 0)
        {
            decimal val = def;
            try
            {
                val = decimal.Parse(obj.ToString());
            }
            catch
            {

            }
            return val;
        }
        /// <summary>
        /// 将值转换成double
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static double GetDef_Double(object obj, double def = 0)
        {
            double val = def;
            try
            {
                val = double.Parse(obj.ToString());
            }
            catch
            {

            }
            return val;
        }

        /// <summary>
        /// 将值转换成decimal并保留指定位数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="len"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static decimal GetDef_Dec(object obj,int len, decimal def = 0)
        {
            decimal val = def;
            try
            {
                val = decimal.Parse(string.Format("{0:f"+len+"}", decimal.Parse(obj.ToString())));
            }
            catch
            {

            }
            return val;
        }

        /// <summary>
        /// 将值转换成datetime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDef_DateTime(object obj)
        {
            DateTime val = DateTime.Parse("2000-1-1");
            try
            {
                val = DateTime.Parse(obj.ToString());
            }
            catch
            {

            }

            return val;
        }

        /// <summary>
        /// 将值转换成bool
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static bool GetDef_Bool(object obj,bool def = false)
        {
            bool val = def;
            try
            {
                val = Boolean.Parse(obj.ToString());
            }
            catch (Exception)
            {
                
            }

            return val;
        }
    }

    
}
