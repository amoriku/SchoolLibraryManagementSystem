using SchoolLibrary.Domain.Exceptions;

namespace SchoolLibrary.Domain.ValueObjects
{
    public record FullName
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string? MiddleName { get; init; }

        public string Full => string.IsNullOrWhiteSpace(MiddleName)
            ? $"{FirstName} {LastName}"
            : $"{FirstName} {LastName} {MiddleName}";

        private FullName() { }

        public FullName(string firstName, string lastName, string? middleName = "")
        {
            if (string.IsNullOrEmpty(firstName.Trim()))
            {
                Console.WriteLine("[Domain] string is null or empty: First Name is required");
            }

            if (string.IsNullOrEmpty(lastName.Trim()))
            {
                Console.WriteLine("[Domain] string is null or empty: Last Name is required");
            }

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            MiddleName = middleName?.Trim();
        }

        public override string ToString()
        {
            return Full;
        }
    }
}
