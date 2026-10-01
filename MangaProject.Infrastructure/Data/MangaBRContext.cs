using MangaProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MangaProject.Infrastructure.Data;

public sealed class MangaBRContext: DbContext
{
    public DbSet<Manga> Mangas => Set<Manga>();

    public MangaBRContext(DbContextOptions<MangaBRContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(m => m.Guid);
            entity.Property(m => m.Title).IsRequired();
            entity.Property(m => m.Volume).IsRequired();
            entity.Property(m => m.Author).IsRequired();            
            entity.Property(m => m.Price).IsRequired();
            entity.Property(m => m.UserId).IsRequired();
        });
    }
}
