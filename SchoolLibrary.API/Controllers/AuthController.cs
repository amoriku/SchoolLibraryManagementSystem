using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Shared;
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

        [Authorize(Policy = PolicyName.AnyUserPolicyName)]
        [HttpDelete("logout")]
        public async Task<IResult> Logout(CancellationToken cancellationToken)
        {
            try
            {
                await authService.Logout(cancellationToken);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IResult> Refresh(CancellationToken cancellationToken)
        {
            try 
            {
                return Results.Ok(await authService.RefreshAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.AnyUserPolicyName)]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
        {
            var user = await authService.GetCurrentUserAsync(cancellationToken);

            return user == null ? Unauthorized("You`r not authorized") : Ok(user);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var tokenResponse = await authService.LoginAsync(dto, cancellationToken);
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.AnyUserPolicyName)]
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
