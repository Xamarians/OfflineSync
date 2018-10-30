namespace OfflineSyncDemo.Services
{
    class RestResult : IRestResult
    {
        public bool IsSuccess
        {
            get
            {
                return Success;
            }
        }
        public bool Success { get; set; }

        public string Message { get; set; }
        /// <summary>
        /// OK = 200, BadRequest = 400, InternalServerError = 500
        /// </summary>
        public int StatusCode { get; set; }
    }

    class RestResult<T> : RestResult, IRestResult<T>
    {
        public T Data { get; set; }
        public string Notification { get; set; }
    }
    public class APIResponse<T>
    {
        public T Data;
        public string Messages { get; set; }
        public bool Success { get; set; }
        public string Notification { get; set; }

    }
}
