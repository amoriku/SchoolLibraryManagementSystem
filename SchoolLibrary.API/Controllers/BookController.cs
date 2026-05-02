using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Book;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Services;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

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

        [HttpGet("get-all-by-title")]
        public async Task<IActionResult> GetAllByTitle(string title, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await bookService.GetAllByTitleAsync(title, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
