using SharedBlocks.Dtos.Books;

namespace BlazorWASM.Services;

public interface IBookApiClient
{
    Task<BooksResponseDTO> GetAllBooksAsync(int page = 1, int pageSize = 10);
    Task<BookResponseDTO> GetBookByIdAsync(Guid id);
    Task<BooksResponseDTO> GetBooksByAuthorAsync(string author);
    Task<BooksResponseDTO> GetBooksByGenreAsync(string genre);
    Task<BookResponseDTO> AddBookAsync(CreateBookDTO bookDto);
    Task<BookResponseDTO> UpdateBookAsync(UpdateBookDTO bookDto);
    Task<BookResponseDTO> DeleteBookAsync(Guid id);
    Task<bool> IsISBNUniqueAsync(string isbn, Guid? excludeId = null);
}