using CloudGamesStore.Application.DTOs.GameDtos;
using CloudGamesStore.Application.DTOs.GenreDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGamesStore.Application.Interfaces
{
    public interface IGenreService
    {
        Task<GenreDto> GetGenreById(int id);
        Task<List<GenreDto>> GetAllGenres();
        Task<GenreDto> CreateGenre(CreateGenreDto gameDto);
        Task<GenreDto> UpdateGenre(UpdateGenreDto gameDto);
        Task DeleteGenre(int id);
    }
}
