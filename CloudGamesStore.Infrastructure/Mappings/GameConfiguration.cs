using CloudGamesStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGamesStore.Infrastructure.Mappings
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Price)
                .HasPrecision(18, 2)
                .IsRequired();
            
            builder.Property(e => e.IsActive)
                .IsRequired();

            builder.HasOne(x => x.Genre)
                .WithMany(x => x.Games)
                .HasForeignKey(x => x.GenreId);
        }
    }
}
