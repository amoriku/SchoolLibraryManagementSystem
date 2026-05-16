using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Shared;
using System.Security.Claims;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> Get(string userId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await userService.GetByIdAsync(userId, cancellationToken));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message); 
            }
        } 

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await userService.GetAllAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
