using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using lixun_upload_HIVE.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;
using System.IO;

namespace lixun_upload_HIVE
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var up = new UploadToHive("http://10.0.0.2:5008/v5/capture/machinedata");

            var filePath1 = @"C:\Users\Cowain\Desktop\1.bmp";
            var filePath2 = @"C:\Users\Cowain\Desktop\2.bmp";
            var filePath3 = @"C:\Users\Cowain\Desktop\Desktop.zip";
            var input_time = "2023-04-11T15:13:39.786782+0800";
            var output_time = "2023-04-11T15:14:39.786782+0800";
            var output_ct = "5.780";

            var filePaths = new List<string>() { filePath1, filePath2, filePath3 };
            var objBlobs = new List<FileModel>();

            foreach (var filePath in filePaths)
            {
                objBlobs.Add(new FileModel() { file_name = Path.GetFileName(filePath) });
            }
            var obj = new UpLoadJsonModel();
            obj.unit_sn = "ESFHDYSFTE";
            obj.input_time = input_time;
            //Thread.Sleep(2000);
            obj.output_time = output_time;
            obj.data = new DataModel()
            {
                output_ct = output_ct
            };
            obj.blobs = objBlobs;

            var content = JsonConvert.SerializeObject(obj);

            var res = up.MultipartFormDataContentTest(content, filePaths: filePaths);
            if (res.ErrorCode == null && res.ErrorText == null)
            {
                Console.WriteLine(res.Status);
            }
            else
            {
                Console.WriteLine(res.ErrorText);
            }

            Console.ReadLine();

        }

    }
}
