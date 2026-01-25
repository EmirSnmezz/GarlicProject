public interface IUserService
{
    IResult Add (User user);
    IDataResult<User> GetUserByEmailOrUserName(string emailOrUsername);
    IResult Remove(User user);
    IResult Update (User user);
}