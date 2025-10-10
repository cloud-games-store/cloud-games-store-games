using CloudGamesStore.Application.DTOs.GenreDtos;
using CloudGamesStore.Application.Interfaces;
using CloudGamesStore.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CloudGamesStore.Application.Services
{
    public class GenreService : IGenreService
    {
        private readonly ILogger<GenreService> _logger;
        private readonly IGenreRepository _genreRepository;

        public GenreService(ILogger<GenreService> logger,
            IGenreRepository genreRepository)
        {
            _logger = logger;
            _genreRepository = genreRepository;
        }

        public async Task<GenreDto> GetGenreById(int id)
        {
            var result = await _genreRepository.GetByIdAsync(id);

            return GenreDto.ToGenreDto(result);
        }

        public async Task<List<GenreDto>> GetAllGenres()
        {
            var result = await _genreRepository.GetAllAsync();
            var convertedResult = result.Select(Genre => GenreDto.ToGenreDto(Genre)).ToList();

            return convertedResult;
        }

        public async Task<GenreDto> CreateGenre(CreateGenreDto genreDto)
        {
            var genre = GenreDto.ToGenre(genreDto);
            var result = await _genreRepository.CreateAsync(genre);

            return GenreDto.ToGenreDto(result);
        }

        public async Task<GenreDto> UpdateGenre(UpdateGenreDto gameDto)
        {
            var game = GenreDto.ToGenre(gameDto);
            var result = await _genreRepository.UpdateAsync(game);

            return GenreDto.ToGenreDto(result);
        }

        public async Task DeleteGenre(int id)
        {
            await _genreRepository.DeleteAsync(id);
        }
    }
}
