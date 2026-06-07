using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Reserve;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Shared;
using SchoolLibrary.Domain;
using SchoolLibrary.Domain.Constants;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReserveService reserveService;

        public ReservationsController(IReserveService reserveService)
        {
            this.reserveService = reserveService;
        }

        [Authorize(Roles = UserRoles.Reader)]
        [HttpGet("active")]
        public async Task<IResult> GetActive(CancellationToken cancellationToken)
        {
            try
            {
                return Results.Ok(await reserveService.GetActiveAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.AnyUserPolicyName)]
        [HttpPost("cancel")]
        public async Task<IResult> Cancel(int reserveId, CancellationToken cancellationToken)
        {
            try
            {
                return Results.Ok(await reserveService.CancelAsync(reserveId, cancellationToken));
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
                var reservations = await reserveService.GetAllAsync(cancellationToken);
                return Results.Ok(reservations);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = PolicyName.AnyUserPolicyName)]
        [HttpPost("reserve")]
        public async Task<IResult> Reserve(ReserveCreateDto request, CancellationToken cancellationToken)
        {
            try
            {
                await reserveService.CreateAsync(request, cancellationToken);
                return Results.Ok("Successful reservation");
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
