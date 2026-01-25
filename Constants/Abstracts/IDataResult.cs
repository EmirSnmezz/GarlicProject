public interface IDataResult<T> : IResult where T: class, new()
{
    T Data {get; set;}
}