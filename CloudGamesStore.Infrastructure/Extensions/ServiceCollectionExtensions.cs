using CloudGamesStore.Domain.Interfaces;
using CloudGamesStore.Infrastructure.Data;
using CloudGamesStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudGamesStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<GameStoreCheckoutDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repository Registration
            services.AddScoped<IGameRepository, GameRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddRepositoryHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.AddHealthChecks()
            //    .AddSqlServer(
            //        configuration.GetConnectionString("DefaultConnection")!,
            //        name: "database",
            //        tags: new[] { "repository", "database" });

            return services;
        }

    }
}
