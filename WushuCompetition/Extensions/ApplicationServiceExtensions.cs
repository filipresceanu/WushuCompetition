using WushuCompetition.Data;
using WushuCompetition.Repository.Interfaces;
using WushuCompetition.Repository;
using WushuCompetition.Services.Interfaces;
using WushuCompetition.Services;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Configurations;

namespace WushuCompetition.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddScoped<ICompetitionRepository, CompetitionRepository>();
            services.AddScoped<ICompetitionService, CompetitionService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAgeCategoryRepository, AgeCategoryRepository>();
            services.AddScoped<IAgeCategoryService, AgeCategoryService>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService,AccountService>();
            services.AddScoped<IRoundRepository, RoundRepository>();
            services.Configure<JwtConfig>(config.GetSection("JwtConfig"));
            services.Configure<EmailConfig>(config.GetSection("EmailConfig"));

            return services;
        }
    }
}
