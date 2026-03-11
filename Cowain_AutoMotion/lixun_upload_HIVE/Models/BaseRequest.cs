using RestSharp;
using System.Collections.Generic;

namespace lixun_upload_HIVE.Models {
    public class BaseRequest {
        /// <summary>
        /// 请求类型
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 请求内容类型参数
        /// </summary>
        public string ContentType { get; set; } = "application/json";

        /// <summary>
        /// 查询参数
        /// </summary>
        public Dictionary<string, string> Parameter { get; set; } =
            new Dictionary<string, string>();

        /// <summary>
        /// 请求体
        /// </summary>
        public Dictionary<string, object> Body { get; set; } =
            new Dictionary<string, object>();

        /// <summary>
        /// 请求头
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } =
            new Dictionary<string, string>();

        /// <summary>
        /// 文件
        /// </summary>
        public Dictionary<string, string> Files { get; set; } =
            new Dictionary<string, string>();
    }
}