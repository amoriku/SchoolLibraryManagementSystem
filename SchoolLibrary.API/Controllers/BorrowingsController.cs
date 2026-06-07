using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Borrow;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Shared;
using SchoolLibrary.Domain.Constants;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingsController : ControllerBase
    {
        private readonly IBorrowingService borrowingService;

        public BorrowingsController(IBorrowingService borrowingService)
        {
            this.borrowingService = borrowingService;
        }

        [Authorize(Roles = UserRoles.Reader)]
        [HttpGet("active")]
        public async Task<IResult> GetActive(CancellationToken cancellationToken)
        {
            try
            {
                return Results.Ok(await borrowingService.GetActiveBorrowings(cancellationToken));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.StaffOnlyPolicyName)]
        [HttpPost("return")]
        public async Task<IResult> Return(int itemCopyId, CancellationToken cancellationToken)
        {
            try
            {
                await borrowingService.ReturnAsync(itemCopyId, cancellationToken);
                return Results.Ok("The book has successfully returned");
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.StaffOnlyPolicyName)]
        [HttpPost("create")]
        public async Task<IResult> Create(CreateBorrowingDto request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await borrowingService.CreateAsync(request, cancellationToken);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.StaffOnlyPolicyName)]
        [HttpGet]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                return Results.Ok(await borrowingService.GetAllAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
