using CloudGamesStore.Domain.Entities;
using CloudGamesStore.Domain.Interfaces;
using CloudGamesStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGamesStore.Infrastructure.Repositories
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        public GameRepository(GameStoreCheckoutDbContext context, ILogger<GameRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<Game> GetByIdWithGenreAsync(int id)
        {
            return await _dbSet.Include(x => x.Genre).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Game>> GetAllWithGenreAsync()
        {
            return await _dbSet.Include(x => x.Genre).ToListAsync();
        }
    }
}
