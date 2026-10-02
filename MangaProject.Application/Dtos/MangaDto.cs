namespace MangaProject.Application.Dtos;

public record MangaDto(
    Guid Guid,
    string Title,
    int Volume,
    string Author,
    decimal Price    
);

public record MangaCreateDto(
    string Title,
    int Volume,
    string Author,
    decimal Price
);

public record UpdateMangaDto(
    string Title,
    int Volume,
    string Author,
    decimal Price
);

