using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Reserve;
using SchoolLibrary.Application.Interfaces;
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

        [Authorize(Roles = $"{UserRoles.Librarian}, {UserRoles.Admin}")]
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

        [Authorize]
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
