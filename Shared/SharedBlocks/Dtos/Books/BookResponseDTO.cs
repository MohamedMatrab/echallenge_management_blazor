namespace SharedBlocks.Dtos.Books;

public class BookResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public BookDTO? Data { get; set; }
}