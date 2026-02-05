using System.Linq.Expressions;

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
        System.Console.WriteLine("USERRRRRRRRRR -> " + emailOrUsername);
        User result = _userDal.GetAll().FirstOrDefault(x => x.Username == emailOrUsername);

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

    public IDataResult<List<User>> GetAll(Expression<Func<User, bool>> filter = null)
    {
        if(filter is null)
        {
            var result = _userDal.GetAll(filter);

            if(result is not null)
            {
               return new SuccessDataResult<List<User>>(data : result);
            }   
        }
        else
        {
            var result = _userDal.GetAll();

            if(result is not null)
            {
               return new SuccessDataResult<List<User>>(data : result);
            }
        }

        return new ErrorDataResult<List<User>>(data:null);
    }
}