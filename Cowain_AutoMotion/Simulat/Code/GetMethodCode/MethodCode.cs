using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public class GetMethodCode
    {
        /// <summary>
        /// 截取整个方法函数
        /// </summary>
        /// <param name="a_strPath">文件全路径</param>
        /// <param name="a_intStartLineNo">代码行号</param>
        /// <returns></returns>
        public List<string> GetMethodContent(string a_strPath, string enumTypeStr)
        {
            int a_intStartLineNo = getLineNo(a_strPath, enumTypeStr);
            string[] l_ArrayCodeContent = File.ReadAllLines(a_strPath);
            List<string> strList = new List<string>();
            //int l_intFlag = 0;

            //for (int l_intCodeLine = a_intStartLineNo - 1; l_intCodeLine < l_ArrayCodeContent.Length; l_intCodeLine++)
            //{
            //    string l_oneline = l_ArrayCodeContent[l_intCodeLine].ToString();

            //    if (l_oneline.Contains("{")) l_intFlag--;
            //    if (l_oneline.Contains("}")) l_intFlag++;

            //    if (l_intFlag == 0 && l_intCodeLine != a_intStartLineNo - 1)
            //    {
            //        strList.Add(l_oneline);
            //        break;
            //    }
            //    else
            //    {
            //        strList.Add(l_oneline);
            //    }
            //}
            return l_ArrayCodeContent.ToList();
        }
        private int getLineNo(string a_strPath, string enumTypeStr)
        {
            string[] strings = File.ReadAllLines(a_strPath);
            for (int i = 0; i < strings.Length; i++)
            {
                string[] lineStrs = strings[i].Trim().Split(' ');
                string lineStrNew = "";
                for (int j = 0; j < lineStrs.Length; j++)
                {
                    if (lineStrs[j] != "")
                    {
                        lineStrNew += lineStrs[j].Trim();
                    }
                }
                string typeLine = "m_nStep=(int)" + enumTypeStr + ".";
                if (lineStrNew.Contains(typeLine))
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
