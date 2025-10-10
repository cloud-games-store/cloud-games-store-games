using CloudGamesStore.Domain.Entities;

namespace CloudGamesStore.Application.DTOs.GenreDtos
{
    public class GenreDto
    {
        public int GenreId { get; set; }
        public string Name { get; set; }

        public static GenreDto ToGenreDto(Genre genre)
        {
            return new GenreDto
            {
                GenreId = genre.GenreId,
                Name = genre.Name
            };
        }

        public static Genre ToGenre(UpdateGenreDto updateGenreDto)
        {
            return new Genre
            {
                GenreId = updateGenreDto.GenreId,
                Name = updateGenreDto.Name
            };
        }

        public static Genre ToGenre(CreateGenreDto createGenreDto)
        {
            return new Genre
            {
                Name = createGenreDto.Name
            };
        }
    }
}