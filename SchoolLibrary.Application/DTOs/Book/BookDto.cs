using SchoolLibrary.Application.DTOs.Author;

namespace SchoolLibrary.Application.DTOs.Book
{
    public record BookDto
    (
        int Id,
        DateTime ReceiptDate,
        string Title,
        //string? Description,
        int? PublishedYear,
        List<AuthorDto> Authors
        //decimal? Price
    );
}
