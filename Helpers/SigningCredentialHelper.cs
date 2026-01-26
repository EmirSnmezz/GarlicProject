using Microsoft.IdentityModel.Tokens;

public static class SigningCredentialHelper
{
    public static SigningCredentials CreateSigningCredential(SecurityKey securityKey)
    {
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }
}