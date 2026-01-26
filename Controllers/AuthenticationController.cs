using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController: ControllerBase
{
    [HttpPost("createToken")]
    public IActionResult CreateAccessToken()
    {
        JwtSecurityToken token = new JwtSecurityToken(
            issuer:"emir",
            audience:"emir",
            expires:DateTime.UtcNow.AddHours(1),
            signingCredentials: SigningCredentialHelper.CreateSigningCredential(SecurityKeyHelper.CreateSecurityKey("mySuperSecretSecurityKeysuuuu43y43245y237543"))
        )   ;

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        return Ok(handler.WriteToken(token));
    }
}