namespace API.DAL.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(Guid id);
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
    Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
    Task<Book> AddBookAsync(Book book);
    Task<Book> UpdateBookAsync(Book book);
    Task<bool> DeleteBookAsync(Guid id);
    Task<bool> IsISBNUniqueAsync(string isbn, Guid? excludeId = null);
    Task<int> GetTotalBooksCountAsync();
}