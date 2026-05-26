using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Author;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Services;
using SchoolLibrary.Domain;
using SchoolLibrary.Domain.Constants;
using SchoolLibrary.Domain.Shared;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService authorService;

        public AuthorsController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpPost("create")]
        [Authorize(Roles = $"{UserRoles.Librarian}, {UserRoles.Admin}")]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await authorService.CreateAsync(dto, cancellationToken));
            }
            catch (AlreadyExistsException ex)
            {
                //Console.WriteLine($"{DebugMessages.ApiLayerMessage}\nException: \n{ex}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await authorService.GetByIdAsync(id, cancellationToken));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await authorService.GetAllAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
