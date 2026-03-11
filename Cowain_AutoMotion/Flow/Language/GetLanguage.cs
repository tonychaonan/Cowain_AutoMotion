using ReadAndWriteConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion.Flow
{
    public class GetLanguage
    {
        public static List<string> valuestring;
        public static List<string[]> keyAndValueString;
        public static string LanguageNumber;
        public static string LanguageStyle;
        public GetLanguage()
        {
            LanguageNumber = ConfigHelper.GetAppConfig("Paramter", "Language");
            valuestring = ConfigHelper.GetKeyAndValue2("Language");
          
            keyAndValueString = new List<string[]>();
            for (int i = 0; i < valuestring.Count; i++)
            {
                string[] s = valuestring[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                keyAndValueString.Add(s);
            }
        }
        public static string getLanguage(string str)
        {
            for (int i = 0; i < valuestring.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (str == keyAndValueString[i][j])
                    {
                        if (LanguageNumber == "1")
                        {
                            str = keyAndValueString[i][1];
                        }
                        else if (LanguageNumber == "2")
                        {
                            str = keyAndValueString[i][2];
                        }
                        else
                        {
                            str = keyAndValueString[i][3];
                        }
                        return str;
                    }
                }
            }


            return str;
        }
    }
}
