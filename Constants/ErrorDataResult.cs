public class ErrorDataResult<T>: DataResult<T>, IDataResult<T> where T: class, new()
{
    public ErrorDataResult(string message, T data): base(false, message, data) {}

    public ErrorDataResult(T data): base(false, data) {}

    public ErrorDataResult(string message): base(false, message) {}
}