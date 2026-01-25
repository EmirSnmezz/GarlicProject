public class UserService : IUserService
{
    IUserDal _userDal;

    public UserService(IUserDal userDal)
    {
        _userDal = userDal;
    }

    public IResult Add(User user)
    {
        _userDal.Add(user);

        return new SuccessResult(message: "Kullanıcı Başarıyla Oluşturuldu");
    }

    public IDataResult<User> GetUserByEmailOrUserName(string emailOrUsername)
    {
        User result = _userDal.Get(user => user.Email == emailOrUsername || user.Username == emailOrUsername);

        if(result is null)
        {
            return new ErrorDataResult<User>(message:"Kullanıcı Bulunamadı.");
        }

        return new SuccessDataResult<User>(data: result, message: "Kullanıcı bilgileri başarıyla bulundu");
    }

    public IResult Remove(User user)
    {
        _userDal.Delete(user);

        return new SuccessResult("Kullanıcı Başarıyla Silindi");
    }

    public IResult Update(User user)
    {
        _userDal.Update(user);

        return new SuccessResult("Kullanıcı bilgileri başarıyla güncellendi");
    }
}