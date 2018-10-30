namespace SyncPocService.Models
{
    public interface IResponse
    {
        bool Success { get; set; }
        string Message { get; set; }
    }

    public interface IResponse<TData> : IResponse
    {
        TData Data { get; set; }
    }

    public class ApiResponse : IResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ApiResponse()
        {

        }

        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
        }

    }

    public class ApiResponse<TData> : ApiResponse, IResponse<TData>
    {
        public TData Data { get; set; }

        public ApiResponse(TData data)
        {
            Success = true;
            Data = data;
        }
    }
}
