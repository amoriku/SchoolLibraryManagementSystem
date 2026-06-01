using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.DTOs.Book;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Services;
using SchoolLibrary.Domain.Constants;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet("get-by-title")]
        public async Task<IResult> GetByTitle(string title, CancellationToken cancellationToken)
        {
            try
            {
                var book = await bookService.GetByTitleAsync(title, cancellationToken);
                return Results.Ok(book);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.Librarian}")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(BookCreateDto dto, CancellationToken cancellationToken)
        {
            try
            { 
                return Ok(await bookService.CreateAsync(dto, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryDto query, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await bookService.GetAllAsync(query, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
