public class Result : IResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public Result(bool isSuccess, string message)
    {
        Message = message;
        IsSuccess = isSuccess;
    }

    public Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
}