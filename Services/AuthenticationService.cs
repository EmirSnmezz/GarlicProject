using System.Text;

public class AuthenticationService : IAuthService
{
    IUserService _userService;
    IJwtProvider _jwtProvider;
    public AuthenticationService(IUserService userService, IJwtProvider jwtProvider)
    {
        _userService = userService;

        _jwtProvider = jwtProvider;
    }

    public IResult Register(RegisterModelDTO registeredUser)
{
    var existingUser = _userService.GetUserByEmailOrUserName(registeredUser.Username).Data;

    if(existingUser != null)
        return new ErrorResult("Kullanıcı Zaten Mevcut");

    byte[] passwordHash, passwordSalt;
    HashingHelper.CreatePasswordHash(registeredUser.Password, out passwordHash, out passwordSalt);

    _userService.Add(new User
    {
        Email = registeredUser.Email,
        Name = registeredUser.Name,
        Surname = registeredUser.Surname,
        Username = registeredUser.Username,
        PasswordHash = Convert.ToBase64String(passwordHash), 
        PasswordSalt = Convert.ToBase64String(passwordSalt)
    });

    return new SuccessResult("Kullanıcı Başarıyla Oluşturuldu");
}
    public AccessToken CreateAccessToken(User user)
    {
        System.Console.WriteLine("CreatedDateeee" + user.CreatedDate);
        User userForDb = _userService.GetAll(null).Data.FirstOrDefault(x => x.Username == user.Username);

        if(user is not null)
        {
            var token = _jwtProvider.CreateAccessToken(user);   
            return token;
        }

        return new AccessToken{Token = "null"};
    }

public IDataResult<AccessToken> Login(LoginModelDTO userLogin)
{
    var user = _userService.GetAll(null).Data.FirstOrDefault();

    if(user == null)
        return new ErrorDataResult<AccessToken>("Kullanıcı Bulunamadı");

    byte[] passwordHash = Convert.FromBase64String(user.PasswordHash);
    byte[] passwordSalt = Convert.FromBase64String(user.PasswordSalt);

    if(HashingHelper.VerifyPasswordHash(userLogin.Password, passwordHash, passwordSalt))
    {
        return new SuccessDataResult<AccessToken>(CreateAccessToken(user));
    }

    return new ErrorDataResult<AccessToken>("Kullanıcı Parolası Hatalı");
}
}