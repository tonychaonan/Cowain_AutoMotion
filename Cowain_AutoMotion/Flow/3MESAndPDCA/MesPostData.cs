using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion.Flow._3MESAndPDCA
{
    public class MesPostData
    {
        public string empNo = "";
        public string terminalName = MESDataDefine.MESLXData.terminalName;
        public string serial_Number = "";
        public string machine = "";
        public string toolingNo = "";
        public string lotNo = "";
        public string kpsn = "";
        public string workOrder = "";
        public string cavity = "";
        public string testData = "";
        public string results = "";
        public string status = "";
        public string pCmd = "";
        public string defectCode = "";
        public string collectType="";
    }

    public class MesResult
    {
        public bool Result;
        public string RetMsg;
    }
}
