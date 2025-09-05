namespace Kutiyana_Memon_Hospital_Api.Http
{
    public class ResponseModel<T>
    {
        public int MessageType { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public int HttpStatusCode { get; set; }
        public IList<string> Errors { get; set; }
        // New constructor to allow setting properties directly
        public ResponseModel(T result, int httpStatusCode, string message, int messageType)
        {
            Result = result;
            HttpStatusCode = httpStatusCode;
            Message = message;
            MessageType = messageType;
            Errors = new List<string>();
        }
        public ResponseModel()
        {
            Errors = new List<string>();
        }
    }
}
