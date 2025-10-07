using CloudGamesStore.Domain.Entities;
using CloudGamesStore.Domain.Interfaces;
using CloudGamesStore.Infrastructure.Data;
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
    }
}
