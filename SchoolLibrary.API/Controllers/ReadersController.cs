using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Shared;
using SchoolLibrary.Domain;
using SchoolLibrary.Domain.Constants;

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
        public async Task<IActionResult> CreateHistoryRecord(CreateReaderHistoryDto dto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await readerService.CreateReaderHistoryAsync(dto, cancellationToken));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.StaffOnlyPolicyName)]
        [HttpPost("create")]
        public async Task<IResult> Create(CreateReaderDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var reader = await readerService.CreateAsync(dto, cancellationToken);
                return Results.Ok(reader);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.LibraryParticipantsPolicyName)]
        [HttpGet("history")]
        public async Task<IActionResult> GetReaderHistory(string readerId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await readerService.GetReaderHistoryAsync(readerId, cancellationToken));
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
