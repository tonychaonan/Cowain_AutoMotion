namespace lixun_upload_HIVE.Models {
    public class BaseResponse {
        /// <summary>
        /// 结果信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 结果状态
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public object Data { get; set; }
    }
}