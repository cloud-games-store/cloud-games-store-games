using CloudGamesStore.Domain.Entities;
using CloudGamesStore.Domain.Interfaces;
using CloudGamesStore.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace CloudGamesStore.Infrastructure.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(GameStoreCheckoutDbContext context, ILogger<GenreRepository> logger)
            : base(context, logger)
        {
        }
    }
}
