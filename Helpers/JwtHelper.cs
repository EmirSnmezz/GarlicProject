using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class JwtHelper : IJwtProvider
{
    private readonly IConfiguration _configuration;
    private readonly TokenOptions _tokenOptions;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _tokenOptions = configuration.GetSection("Security").Get<TokenOptions>();

        if (_tokenOptions == null)
            throw new Exception("Security ayarları appsettings.json'da bulunamadı.");
        if (string.IsNullOrEmpty(_tokenOptions.SecretKey))
            throw new Exception("SecretKey boş olamaz.");
    }

    public AccessToken CreateAccessToken(User user)
    {
        var accessTokenExpiration = DateTime.UtcNow.AddHours(1);

        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecretKey);
        var signingCredentials = SigningCredentialHelper.CreateSigningCredential(securityKey);

        var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, accessTokenExpiration);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(jwt);

        return new AccessToken()
        {
            Token = token,
            Expiration = accessTokenExpiration
        };
    }

    public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, DateTime expiration)
    {
          var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),  // User Id
                new Claim(ClaimTypes.Name, user.Username),     // Username
                new Claim(ClaimTypes.Email, user.Email ?? ""), // Email
                new Claim("FullName", $"{user.Name} {user.Surname}"), // İsteğe bağlı
                new Claim("admin", "admin") // Role veya flag
            };

        var jwt = new JwtSecurityToken(
            issuer: tokenOptions.Issuer,
            audience: tokenOptions.Audience,
            expires: expiration,
            notBefore: DateTime.UtcNow,
            claims: claims,
            signingCredentials: signingCredentials
        );

        return jwt;
    }
}
