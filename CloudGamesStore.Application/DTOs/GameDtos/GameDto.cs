using CloudGamesStore.Application.DTOs.GenreDtos;
using CloudGamesStore.Domain.Entities;

namespace CloudGamesStore.Application.DTOs.GameDtos
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int GenreId { get; set; }
        public GenreDto Genre { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public static Game ToGame(GameDto gameDto)
        {
            if (gameDto != null)
            {
                return new Game
                {
                    Name = gameDto.Name,
                    Description = gameDto.Description,
                    GenreId = gameDto.GenreId,
                    Price = gameDto.Price,
                    IsActive = gameDto.IsActive,
                    CreatedAt = gameDto.CreatedAt
                };
            }
            return null;
        }

        public static Game ToGame(CreateGameDto gameDto)
        {
            if (gameDto != null)
            {
                return new Game
                {
                    Name = gameDto.Name,
                    Description = gameDto.Description,
                    GenreId = gameDto.GenreId,
                    Price = gameDto.Price,
                    IsActive = gameDto.IsActive,
                    CreatedAt = gameDto.CreatedAt
                };
            }
            return null;
        }

        public static Game ToGame(UpdateGameDto gameDto)
        {
            if (gameDto != null)
            {
                return new Game
                {
                    Id = gameDto.Id,
                    Name = gameDto.Name,
                    Description = gameDto.Description,
                    GenreId = gameDto.GenreId,
                    Price = gameDto.Price,
                    IsActive = gameDto.IsActive,
                    CreatedAt = gameDto.CreatedAt
                };
            }
            return null;
        }

        public static GameDto? ToGameDto(Game game)
        {
            if (game != null)
            {
                return new GameDto
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    GenreId = game.GenreId,
                    Genre = new GenreDto
                    {
                        GenreId = game.Id,
                        Name = game.Name
                    },
                    Price = game.Price,
                    IsActive = game.IsActive,
                    CreatedAt = game.CreatedAt
                };
            }
            return null;
        }
    }
}
