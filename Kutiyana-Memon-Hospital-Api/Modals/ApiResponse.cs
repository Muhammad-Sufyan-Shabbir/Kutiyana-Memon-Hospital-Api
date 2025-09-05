namespace Kutiyana_Memon_Hospital_Api.API.Modals
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data {  get; set; }

        public ApiResponse() { }
        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(string message, bool success = false)
        {
            Success = success;
            Message = message;
        }

    }
}
