namespace SharedBlocks.Dtos.Books;

public class BookDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string Language { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int AvailableStock { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public List<ReviewDTO>? Reviews { get; set; }
}