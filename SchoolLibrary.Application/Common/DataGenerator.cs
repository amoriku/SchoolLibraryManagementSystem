namespace SchoolLibrary.Application.Common
{
    public static class DataGenerator
    {
        private static readonly Random random = new Random();

        // 978-5-XXXX-XXXX-X
        public static string GenerateIsbn13()
        {
            return $"LIB-{DateTime.UtcNow.Ticks.ToString().Substring(11)}";

            //int countryCode = 5;
            //int publisherCode = random.Next(1000, 10000);
            //string registrationCode = random.Next(0, 10000).ToString("D4");
            //int controlDigitCode = random.Next(1, 9);

            //return $"978-{countryCode}-{publisherCode}-{registrationCode}-{controlDigitCode}";
        }
    }
}
