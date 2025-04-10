namespace SharedBlocks.Dtos.Books;

public class ReviewDTO
{
    public Guid Id { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
}