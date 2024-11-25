public class ResponseData<T>
{
    public int StatusCode { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }

    public ResponseData(int statusCode, T data, string message)
    {
        StatusCode = statusCode;
        Data = data;
        Message = message;
    }
}
