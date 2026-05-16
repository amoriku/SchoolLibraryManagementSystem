namespace SchoolLibrary.Application.DTOs
{
    public record QueryDto
    (
        string? Search,
        int Page = 1,
        int PageSize = 20,
        string SortBy = "id",
        string SortOrder = "asc"
    );




}
