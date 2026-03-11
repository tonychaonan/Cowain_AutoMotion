using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadAndWriteConfig;
using Cowain_AutoMotion.Flow;

namespace Cowain
{
    class JudgeLanguage
    {
        public static string JudgeLag(string str)
        {
            #region
            //string s=ConfigHelper.GetAppConfig("Paramter", "Language");
            //if ((ConfigHelper.GetAppConfig("Paramter", "Language")) == "3")
            //{
            //    str = ConfigHelper.GetAppConfig("Language", str, "VN");
            //}
            //else if ((ConfigHelper.GetAppConfig("Paramter", "Language")) == "2")
            //{
            //    str = ConfigHelper.GetAppConfig("Language", str, "UK");
            //}
            //else
            //{
            //    str = ConfigHelper.GetAppConfig("Language", str, "CN");
            //}
            #endregion
            str = GetLanguage.getLanguage(str);
            return str;
        }
    }
}
