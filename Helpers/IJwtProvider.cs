using Microsoft.AspNetCore.Authentication.BearerToken;

public interface IJwtProvider
{
    void CreateAccessToken();
}