using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.UI.WebControls;
using Microsoft.Win32;
using lixun_upload_HIVE.Models;
using Newtonsoft.Json;

namespace lixun_upload_HIVE
{
    public class UploadToHive
    {
        private readonly string _apiUrl = "http://10.0.0.2:5008/v5/capture/machinedata";

        private readonly HttpClient _client;

        private readonly HttpRequestMessage _request;

        public UploadToHive(string apiUrl)
        {
            _client = new HttpClient();
            _request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        }

        private string Execute(HttpRequestMessage request)
        {

            var response = _client.SendAsync(_request).Result;
            //response.EnsureSuccessStatusCode();

            var resStr = response.Content.ReadAsStringAsync().Result;

            return resStr;
        }

        private string JsonTest(string content)
        {
            _request.Content = new StringContent(content);

            return Execute(_request);
        }

        private string MultipartContentTest(string content)
        {
            var requestContent = new MultipartContent("form-data",
                DateTime.Now.Ticks.ToString("X"));

            var fileStream = new FileStream(
                "C:\\Users\\mayin\\Desktop\\cat.bmp", FileMode.Open,
                FileAccess.Read);

            var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes("C:\\Users\\mayin\\Desktop\\cat.bmp"));

            requestContent.Add(fileContent);
            requestContent.Add(new StringContent(content));

            _request.Content = requestContent;

            return Execute(_request);

        }

        public HiveResponse MultipartFormDataContentTest(string content, List<string> filePaths)
        {

            var requestContent =
                new MultipartFormDataContent(DateTime.Now.Ticks.ToString("X"));

            for (var index = 0; index < filePaths.Count; index++)
            {
                var filePath = filePaths[index];

                var fileName = Path.GetFileName(filePath);

                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fileName,
                    Name = "file" + (index + 1)
                };

                requestContent.Add(fileContent);
            }

            //var content = GenriteContent(sn, input_time, output_time, output_ct, filePaths);

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            stringContent.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("form-data")
                {
                    Name = "jsondata"
                };

            requestContent.Add(stringContent);

            _request.Content = requestContent;

            return JsonConvert.DeserializeObject<HiveResponse>(Execute(_request));
        }

        private string GenriteContent(string sn, string input_time, string output_time, string output_ct, List<string> filePaths)
        {
            var objBlobs = new List<FileModel>();

            foreach (var filePath in filePaths)
            {
                objBlobs.Add(new FileModel() { file_name = Path.GetFileName(filePath) });
            }
            var obj = new UpLoadJsonModel();
            obj.unit_sn = "ESFHDYSFTE";
            obj.input_time = input_time ;
            //Thread.Sleep(2000);
            obj.output_time = output_time;
            obj.data = new DataModel()
            {
                output_ct = output_ct
            };
            obj.blobs = objBlobs;

            return JsonConvert.SerializeObject(obj);
        }
    }
}
