using Microsoft.Extensions.DependencyInjection;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Services;

namespace SchoolLibrary.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IReaderService, ReaderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFundService, FundService>();
            //services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IReserveService, ReserveService>();
            services.AddScoped<IBorrowingService, BorrowingService>();

            return services;
        }
    }
}
