public interface IAuthService : IJwtProvider{
        public IResult Register (RegisterModelDTO registeredUser);
        public IDataResult<AccessToken> Login (LoginModelDTO loginModel);

}