namespace SchoolLibrary.Domain.Entities
{
    public sealed class RefreshToken
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOnUtc { get; set; }
    }
}
