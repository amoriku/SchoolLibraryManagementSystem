using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.DTOs.Book;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Services;
using SchoolLibrary.Application.Shared;
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


        [HttpGet("nearest-availability-date")]
        public async Task<IResult> GetNearestAvailabilityDate(int bookId, CancellationToken cancellationToken)
        {
            try
            {
                var date = await bookService.GetNearestAvailabilityDateAsync(bookId, cancellationToken);
                return Results.Ok(date);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
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

        [Authorize(Policy = PolicyName.StaffOnlyPolicyName)]
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

        [HttpGet("history")]
        public async Task<IResult> GetBookHistory(int bookId, CancellationToken cancellationToken)
        {
            try
            {
                var history = await bookService.GetBookHistoryAsync(bookId, cancellationToken);
                return Results.Ok(history);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
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
