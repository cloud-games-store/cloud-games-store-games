using CloudGamesStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloudGamesStore.Infrastructure.Data
{
    public class GameStoreCheckoutDbContext : DbContext
    {
        public GameStoreCheckoutDbContext(DbContextOptions<GameStoreCheckoutDbContext> options)
            : base(options) { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameStoreCheckoutDbContext).Assembly);

            modelBuilder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = 1,
                    Name = "RPG",
                },
                new Genre
                {
                    GenreId = 2,
                    Name = "Adventure",
                },
                new Genre
                {
                    GenreId = 3,
                    Name = "Racing",
                });

            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = 1,
                    Name = "Game 1",
                    Description = "Game 1 RPG",
                    GenreId = 1,
                    Price = 150,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Game
                {
                    Id = 2,
                    Name = "Game 2",
                    Description = "Game 2 Adventure",
                    GenreId = 2,
                    Price = 200,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                },
                new Game
                {
                    Id = 3,
                    Name = "Game 3",
                    Description = "Game 3 Racing",
                    GenreId = 1,
                    Price = 270,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                });
        }
    }
}
