using SchoolLibrary.Application.DTOs.Author;
using SchoolLibrary.Domain;

namespace SchoolLibrary.Application.DTOs.Book
{
    public record BookDto
    (
        int Id,
        DateTime ReceiptDate,
        string Title,
        //string? Description,
        int? PublishedYear,
        int? Quantity,
        List<AuthorDto> Authors
        //decimal? Price
    );
}
