using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Fund;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Services;
using SchoolLibrary.Application.Shared;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundsController : ControllerBase
    {
        private readonly IFundService fundService;

        public FundsController(IFundService fundService)
        {
            this.fundService = fundService;
        }

        [Authorize(Policy = PolicyName.StaffOnlyPolicyName)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(FundCreateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await fundService.CreateAsync(dto, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(short id, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await fundService.GetByIdAsync(id, cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await fundService.GetAllAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
