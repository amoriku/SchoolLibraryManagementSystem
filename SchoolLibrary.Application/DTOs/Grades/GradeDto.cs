using SchoolLibrary.Application.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Application.DTOs.Grades
{
    public record GradeDto
    (
        short Id,
        string DisplayName 
    );
}
