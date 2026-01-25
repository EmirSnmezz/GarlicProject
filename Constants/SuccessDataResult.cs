public class SuccessDataResult<T> : DataResult<T>, IDataResult<T> where T: class, new()
{
    public SuccessDataResult(string message, T data): base(true, message, data) {}

    public SuccessDataResult(T data): base(true, data) {}

    public SuccessDataResult(string message): base(true, message) {}
}