namespace SchoolLibrary.Application.DTOs.Book
{
    public record BookCreateDto
    (
        string Title,
        List<int> AuthorIds,
        int? PublishedYear,
        decimal? Price,
        string? Description
    );

    
}
