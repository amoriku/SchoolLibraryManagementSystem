namespace SchoolLibrary.Application.DTOs
{
    public record TokenResponseDto
    (
        string AccessToken,
        string RefreshToken
    );
}
