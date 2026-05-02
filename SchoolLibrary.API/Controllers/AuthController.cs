using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Application.Interfaces;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await authService.LoginAsync(dto, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize("Admin")]
        public IActionResult Admin()
        {
            return Ok("Admin authorization");
        }

        [Authorize("Reader")]
        public IActionResult Reader()
        {
            return Ok("Reader authorization");
        }
    }
}
