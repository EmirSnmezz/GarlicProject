using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;


[Route("Authentication")]
public class AuthenticationController: Controller
{

    IAuthService _authService;
    IUserService _userService;

    public AuthenticationController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }
    

    [HttpPost("Login")]
    public IActionResult Login(LoginModelDTO user)
    {
        var isAuth = _authService.Login(user);
        
        if(isAuth.IsSuccess)
        {
           var userInDb = _userService.GetAll(null).Data.FirstOrDefault(x => x.Username == user.UserName);
           var token = _authService.CreateAccessToken(userInDb);
           
 Response.Cookies.Append("jwtToken", token.Token, new CookieOptions
{
    HttpOnly = true,
    Secure = false, // Localhost/HTTP çalıştığın için false kalmalı
    SameSite = SameSiteMode.Lax, // Strict yerine Lax yap
    Expires = token.Expiration,
    Path = "/"
});


    return RedirectToAction("Index", "Admin");
        }

        return BadRequest(isAuth.Message);
        
    }

     [HttpPost("Register")]
    public IActionResult Register([FromBody] RegisterModelDTO user)
    {
        var isThereUser = _userService.GetUserByEmailOrUserName(user.Username);

        if(!isThereUser.IsSuccess)
        {
           var registerUser = _authService.Register(user);
           if(registerUser.IsSuccess)
            return Ok(registerUser.IsSuccess);
        }

        return BadRequest();
        
    }
}