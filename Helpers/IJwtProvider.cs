using Microsoft.AspNetCore.Authentication.BearerToken;

public interface IJwtProvider
{
    AccessToken CreateAccessToken(User user);
}