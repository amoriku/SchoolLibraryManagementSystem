using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Application.Interfaces;
using System.Security.Claims;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ITokenProvider tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            this.authService = authService;
            this.tokenProvider = tokenProvider;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await authService.RegisterAsync(dto, cancellationToken);
                return Ok(user);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"[API:AuthController] Error when register new user: \n{ex}");
                return BadRequest(ex.Message);
            }         
        }

        [HttpGet("current")]
        [Authorize]
        public IActionResult GetCurrent()
        {
            string? userId = authService.GetCurrentUser();

            return string.IsNullOrEmpty(userId) ? Unauthorized("You`r not authorized") : Ok(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var tokenResponse = await authService.LoginAsync(dto, cancellationToken);

                Response.Cookies.Append("user_session", tokenResponse!.AccessToken,
                    new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        HttpOnly = true,  
                    });

                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("revoke-refresh-tokens")]
        public async Task<IActionResult> RevokeRefreshTokens(CancellationToken cancellationToken)
        {
            try
            {
                await tokenProvider.RevokeRefreshTokensAsync(cancellationToken);
                return Ok("Tokens revoked");
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(ex.Message);
            }

            
        }

        [Authorize]
        [HttpGet("test-auth")]
        public IActionResult TestAuth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isAuthenticated = User.Identity?.IsAuthenticated;
            var claims = User.Claims.ToDictionary(c => $"{c.Type}: {c.Value}");

            return Ok(new
            {
                IsAuthenticated = isAuthenticated,
                UserId = userId,
                Claims = claims,
                HasHttpContext = HttpContext != null
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-auth")]
        public IActionResult Admin()
        {
            return Ok("Admin authorization");
        }

        [Authorize(Roles = "Reader")]
        [HttpGet("reader-auth")]
        public IActionResult Reader()
        {
            return Ok("Reader authorization");
        }
    }
}
