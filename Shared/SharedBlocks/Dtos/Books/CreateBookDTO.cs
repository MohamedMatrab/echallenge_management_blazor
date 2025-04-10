namespace SharedBlocks.Dtos.Books;

public class CreateBookDTO
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string Language { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int AvailableStock { get; set; }
}