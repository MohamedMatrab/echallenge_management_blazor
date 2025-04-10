namespace API.DAL.Repositories;

public class BookRepository(AppDbContext context) : IBookRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _context.Books
            .Include(b => b.Reviews)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book> GetBookByIdAsync(Guid id)
    {
        var book = (await _context.Books
            .Include(b => b.Reviews)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id))
            ?? throw new KeyNotFoundException();
        return book;
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
    {
        return await _context.Books
            .Where(b => b.Author.Contains(author))
            .Include(b => b.Reviews)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre)
    {
        return await _context.Books
            .Where(b => b.Genre == genre)
            .Include(b => b.Reviews)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book> AddBookAsync(Book book)
    {
        book.Id = Guid.NewGuid();
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        var existingBook = await _context.Books.FindAsync(book.Id) ?? throw new KeyNotFoundException("Book not found !");

        _context.Entry(existingBook).CurrentValues.SetValues(book);

        if (book.Reviews != null)
        {
            // Remove existing reviews
            var existingReviews = await _context.Reviews
                .Where(r => EF.Property<Guid>(r, "BookId") == book.Id)
                .ToListAsync();

            foreach (var review in existingReviews)
            {
                _context.Reviews.Remove(review);
            }

            // Add new reviews
            foreach (var review in book.Reviews)
            {
                _context.Reviews.Add(review);
            }
        }

        await _context.SaveChangesAsync();
        return existingBook;
    }

    public async Task<bool> DeleteBookAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsISBNUniqueAsync(string isbn, Guid? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return !await _context.Books
                .AnyAsync(b => b.ISBN == isbn && b.Id != excludeId.Value);
        }
        else
        {
            return !await _context.Books
                .AnyAsync(b => b.ISBN == isbn);
        }
    }

    public async Task<int> GetTotalBooksCountAsync()
    {
        return await _context.Books.CountAsync();
    }
}