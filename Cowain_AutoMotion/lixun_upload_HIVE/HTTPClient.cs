using lixun_upload_HIVE.Models;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RestSharp.Serialization.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using System.Web;

namespace lixun_upload_HIVE
{
    public class HTTPClient
    {
        private readonly string _apiUrl;

        private readonly RestClient client;

        public static string jsonSt2 { get; set; }

        public HTTPClient()
        {
            //_apiUrl = "http://192.168.10.5:88/";

            _apiUrl = "http://10.0.0.2:5008/v5/";
            client = new RestClient();

        }

        private async Task<BaseResponse> UploadFile(Dictionary<string, string> fileDictionary, string route)
        {

            var obj = new UpLoadJsonModel();
            obj.input_time = "2023-04-08T13:13:39.786782+0800";
            //Thread.Sleep(2000);
            obj.output_time = "2023-04-08T13:14:39.786782+0800";
            obj.data = new DataModel()
            {
                output_ct = "5.780"
            };
            //obj.blobs = new List<FileModel>() {
            //    new FileModel() {file_name = "1.bmp" }
            //};

            var boundary = DateTime.Now.Ticks.ToString("X");

            var baseRequest = new BaseRequest()
            {
                //ContentType = "multipart/form-data; boundary=" + boundary,
                ContentType = "application/json",
                Method = Method.POST,
                Route = route,
                Files = fileDictionary,
                Body = {{"jsondata",obj}},
                //Headers =
                //{
                //    {"User-Agent","PostmanRuntime/7.31.3" },
                //    {"Accept","*/*" },
                //    {"Accept-Encoding","gzip, deflate, br" },
                //    {"Connection","keep-alive" },
                //}
            };

            return await ExecuteAsync(baseRequest);
        }

        private async Task<BaseResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var response = await ActionTask(baseRequest);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return new BaseResponse()
                {
                    IsSuccessful = true,
                    Message = "error",
                    Data = response.Content
                };
            }

            if (response.StatusCode != HttpStatusCode.OK)
                return new BaseResponse() { Message = response.ErrorMessage, IsSuccessful = false };

            if (response.Content != null)
            {
                return new BaseResponse()
                {
                    IsSuccessful = true,
                    Message = "ok",
                    Data = response.Content
                };
            }

            return response.ErrorMessage == null
                ? new BaseResponse() { Message = response.StatusDescription, IsSuccessful = false }
                : new BaseResponse() { Message = response.ErrorMessage, IsSuccessful = false };

        }

        private async Task<IRestResponse> ActionTask(BaseRequest baseRequest)
        {
            var request = new RestRequest(new Uri(_apiUrl + baseRequest.Route), baseRequest.Method);

            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
            {
                switch (request.Method)
                {
                    case Method.GET:
                        {
                            foreach (var item in baseRequest.Parameter)
                            {
                                request.AddParameter(item.Key, item.Value);
                            }
                            break;
                        }
                    case Method.POST:
                        foreach (var item in baseRequest.Parameter)
                        {
                            request.AddQueryParameter(item.Key, item.Value);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }


            if (baseRequest.Body != null)
            {
                request.AddJsonBody(baseRequest.Body);
            }

            //request.AddParameter("Content-Disposition: form-data; name=\'jsondata\'", JsonConvert.SerializeObject(baseRequest.Body["jsondata"]));

            if (baseRequest.Headers != null)
            {
                request.AddHeaders(baseRequest.Headers);
            }

            


            //if (baseRequest.ContentType != "multipart/form-data")
            //    return await client.ExecuteAsync(request);
            //foreach (var requestFile in baseRequest.Files)
            //{
            //    //request.AddFile(requestFile.Key, requestFile.Value, baseRequest.ContentType);

            //    string contentType = MimeMapping.GetMimeMapping(requestFile.Value);
            //    var bytes = File.ReadAllBytes(requestFile.Value);
            //    //request.AddHeader("Content-Type", contentType);
            //    request.AddFileBytes(requestFile.Key, bytes,requestFile.Value,contentType);

            //}

            

            return await client.ExecuteAsync(request);
        }

    }
}