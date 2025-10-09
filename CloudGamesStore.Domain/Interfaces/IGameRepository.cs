using CloudGamesStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGamesStore.Domain.Interfaces
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<Game> GetByIdWithGenreAsync(int id);
        Task<List<Game>> GetAllWithGenreAsync();
    }
}
