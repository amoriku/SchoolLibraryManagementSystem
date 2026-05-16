namespace SchoolLibrary.Application.DTOs.Book
{
    public record BookDto
    (
        int Id,
        DateTime receiptDate,
        string Title,
        //string? Description,
        int? publishedYear,
        decimal? price
    );
}
