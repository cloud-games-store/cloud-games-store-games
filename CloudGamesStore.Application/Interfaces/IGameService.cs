using CloudGamesStore.Application.DTOs.GameDtos;

namespace CloudGamesStore.Application.Interfaces
{
    public interface IGameService
    {
        Task<GameDto> GetGameById(int id);
        Task<GameDto> GetByIdWithGenreAsync(int id);
        Task<List<GameDto>> GetAllGames();
        Task<List<GameDto>> GetAllWithGenreAsync();
        Task<GameDto> CreateGame(CreateGameDto gameDto);
        Task<GameDto> UpdateGame(UpdateGameDto gameDto);
        Task DeleteGame(int id);
    }
}
