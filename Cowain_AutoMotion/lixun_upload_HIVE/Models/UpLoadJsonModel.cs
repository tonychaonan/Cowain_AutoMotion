using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lixun_upload_HIVE.Models
{
    public class UpLoadJsonModel
    {
        public string unit_sn { get; set; }

        public SerialsModel serials { get; set; } = new SerialsModel();

        public string pass { get; set; } = "TRUE";

        public string input_time { get; set; }

        public string output_time { get; set; }

        public DataModel data { get; set; }

        public List<FileModel> blobs { get; set; }


    }


    public class SerialsModel
    {
        public string carrier_sn { get; set; } = "CCCCC";

        public string lotno { get; set; } = "CoilBBBBB";

    }

   public class DataModel
    {
        public string Vender { get; set; } = "COWAIN";

        public string output_ct { get; set; }

        public int hivestate { get; set; } = 1;

        public string sw_version { get; set; } = "cwn_1.0.0.0_230201_POR";

        public string limits_version { get; set; } = "cwn_1.0.0.0_230201_POR";
    }

    public class FileModel
    {
        public string file_name { get; set; }
    }
}
