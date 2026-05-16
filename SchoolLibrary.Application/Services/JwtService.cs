//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Tokens;
//using SchoolLibrary.Application.Interfaces;
//using SchoolLibrary.Domain.Constants;
//using SchoolLibrary.Domain.Entities;
//using SchoolLibrary.Infrastructure;
//using System.IdentityModel.Tokens.Jwt;
//using System.Runtime.InteropServices.Marshalling;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;

//namespace SchoolLibrary.Application.Services
//{
//    public class JwtService(IConfiguration configuration) : IJwtService
//    {
//        private readonly string jwtSecret = configuration["JwtConfig:Secret"]
//            ?? throw new InvalidOperationException("Jwt secret not found");

//        private readonly string jwtIssuer = configuration["JwtConfig:Issuer"]
//            ?? throw new InvalidOperationException("Jwt issuer not found");

//        private readonly string jwtAudience = configuration["JwtConfig:Audience"]
//            ?? throw new InvalidOperationException("Jwt audience not found");

//        public string GenerateRefreshToken()
//        {
//            byte[] randomNumber = new byte[64];
//            using var rng = RandomNumberGenerator.Create();
//            rng.GetBytes(randomNumber);
//            return Convert.ToBase64String(randomNumber);
//        }

//        public string GenerateToken(string? username, string userId, string userRole = UserRoles.Reader)
//        {
//            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtSecret));

//            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

//            DateTime expirationDate = DateTime.UtcNow.AddDays(1);

//            List<Claim> claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Role, userRole),
//                new Claim(ClaimTypes.NameIdentifier, userId),
//                new Claim(ClaimTypes.Name, !string.IsNullOrEmpty(username) ? username : "undefined")
//            };

//            SecurityTokenDescriptor tokenDescriptor = new()
//            {
//                Subject = new(claims),
//                Issuer = jwtIssuer,
//                Audience = jwtAudience,
//                Expires = expirationDate,
//                SigningCredentials = credentials
//            };

//            JwtSecurityTokenHandler tokenHandler = new()
//            {
//                SetDefaultTimesOnTokenCreation = false
//            };

//            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

//            return tokenHandler.WriteToken(token);
//        }

//        public async Task<ApplicationUser?> ValidateRefreshTokenAsync(AppDbContext context, string userId, string refreshToken)
//        {
//            ApplicationUser? user = await context.ApplicationUsers.FindAsync(userId);

//            //if (user is null
//            //    || user.RefreshToken != refreshToken)
//            //{
//            //    throw new SecurityTokenException("Invalid refresh token");
//            //}

//            return user;
//        }

//        public async Task<IDictionary<string, object>> ValidateTokenAsync(string token)
//        {
//            byte[] key = Encoding.UTF8.GetBytes(jwtSecret);

//            SymmetricSecurityKey securityKey = new(key);

//            TokenValidationParameters validationParameters = new TokenValidationParameters()
//            {
//                ValidIssuer = jwtIssuer,
//                ValidAudience = jwtAudience,
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                IssuerSigningKey = securityKey,
//                ValidateIssuerSigningKey = true,
//            };

//            JwtSecurityTokenHandler tokenHandler = new();

//            TokenValidationResult result = await tokenHandler.ValidateTokenAsync(token, validationParameters);

//            if (!result.IsValid)
//            {
//                throw new SecurityTokenException("Invalid token");
//            }

//            return result.Claims;
//        }
//    }
//}
