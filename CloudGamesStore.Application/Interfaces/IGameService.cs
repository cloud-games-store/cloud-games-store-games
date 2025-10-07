using CloudGamesStore.Application.DTOs.GameDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGamesStore.Application.Interfaces
{
    public interface IGameService
    {
        Task<GameDto> GetGameById(int id);
        Task<List<GameDto>> GetAllGames();
        Task<GameDto> CreateGame(CreateGameDto gameDto);
        Task<GameDto> UpdateGame(UpdateGameDto gameDto);
        Task DeleteGame(int id);
    }
}
