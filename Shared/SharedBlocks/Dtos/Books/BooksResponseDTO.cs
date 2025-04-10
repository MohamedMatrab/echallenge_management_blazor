namespace SharedBlocks.Dtos.Books;

public class BooksResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<BookDTO>? Data { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
}