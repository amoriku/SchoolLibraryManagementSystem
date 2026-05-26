using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Constants;
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

        [Authorize(Roles = UserRoles.Admin)]
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("remove")]
        public async Task<IActionResult> Remove(string userId)
        {
            try
            {
                await userService.DeleteAsync(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(UserCreateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await userService.CreateAsync(dto, cancellationToken));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
