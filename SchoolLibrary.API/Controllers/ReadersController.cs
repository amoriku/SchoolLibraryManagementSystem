using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadersController : ControllerBase
    {
        private readonly IReaderService readerService;

        public ReadersController(IReaderService readerService)
        {
            this.readerService = readerService;
        }

        [HttpGet]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                return Results.Ok(await readerService.GetAllAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [HttpPost("create-history-record")]
        public async Task<IActionResult> CreateHistoryRecord(UserHistoryCreateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await readerService.CreateUserHistoryRecordAsync(dto, cancellationToken));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-user-history")]
        public async Task<IActionResult> GetUserHistory(string userId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await readerService.GetUserHistoryAsync(userId, cancellationToken));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
