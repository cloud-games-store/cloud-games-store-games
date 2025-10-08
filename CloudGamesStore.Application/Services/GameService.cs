using CloudGamesStore.Application.DTOs.GameDtos;
using CloudGamesStore.Application.Interfaces;
using CloudGamesStore.Domain.Entities;
using CloudGamesStore.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGamesStore.Application.Services
{
    public class GameService : IGameService
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGameRepository _gameRepository;

        public GameService(ILogger<GameService> logger,
            IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        public async Task<GameDto> CreateGame(CreateGameDto gameDto)
        {
            var game = GameDto.ToGame(gameDto); 
            var result = await _gameRepository.CreateAsync(game);

            return GameDto.ToGameDto(result);
        }

        public async Task DeleteGame(int id)
        {
            await _gameRepository.DeleteAsync(id);
        }

        public async Task<List<GameDto>> GetAllGames()
        {
            var result = await _gameRepository.GetAllAsync();
            var convertedResult = result.Select(game => GameDto.ToGameDto(game)).ToList();

            return convertedResult;
        }

        public async Task<GameDto> GetGameById(int id)
        {
            var result = await _gameRepository.GetByIdAsync(id);

            return GameDto.ToGameDto(result);
        }
        
        public async Task<GameDto> GetByIdWithGenreAsync(int id)
        {
            var result = await _gameRepository.GetByIdWithGenreAsync(id);

            return GameDto.ToGameDto(result);
        }

        public async Task<GameDto> UpdateGame(UpdateGameDto gameDto)
        {
            var game = GameDto.ToGame(gameDto);
            var result = await _gameRepository.UpdateAsync(game);

            return GameDto.ToGameDto(result);
        }
    }
}
