using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Application.DTOs.Grades;
using SchoolLibrary.Application.Interfaces;

namespace SchoolLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController(IGradeService gradeService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var grades = await gradeService.GetAllAsync(cancellationToken);
                return Ok(grades);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IResult> Create(CreateGradeDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var grade = await gradeService.CreateAsync(dto, cancellationToken);
                return Results.Ok(grade);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
