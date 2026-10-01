namespace MangaProject.Domain.Entities;

public class Manga
{
    public required Guid Guid { get; set; }
    public required string Title { get; set; }
    public required int Volume { get; set; }
    public required string Author { get; set; }
    public required decimal Price { get; set; }
    public required string UserId { get; set; }
}
