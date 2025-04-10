using API.DAL.Entities;
using API.DAL.Repositories;
using AutoMapper;
using SharedBlocks.Dtos.Books;

namespace API.SERVICE.Services.Implementations;

public class BookService(IBookRepository bookRepository, IMapper mapper) : IBookService
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<BooksResponseDTO> GetAllBooksAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            var books = await _bookRepository.GetAllBooksAsync();
            var totalCount = await _bookRepository.GetTotalBooksCountAsync();

            var paginatedBooks = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return new BooksResponseDTO
            {
                Success = true,
                Message = "Books retrieved successfully",
                Data = _mapper.Map<IEnumerable<BookDTO>>(paginatedBooks),
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };
        }
        catch (Exception ex)
        {
            return new BooksResponseDTO
            {
                Success = false,
                Message = $"Error retrieving books: {ex.Message}"
            };
        }
    }

    public async Task<BookResponseDTO> GetBookByIdAsync(Guid id)
    {
        try
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return new BookResponseDTO
                {
                    Success = false,
                    Message = "Book not found"
                };
            }

            return new BookResponseDTO
            {
                Success = true,
                Message = "Book retrieved successfully",
                Data = _mapper.Map<BookDTO>(book)
            };
        }
        catch (Exception ex)
        {
            return new BookResponseDTO
            {
                Success = false,
                Message = $"Error retrieving book: {ex.Message}"
            };
        }
    }

    public async Task<BooksResponseDTO> GetBooksByAuthorAsync(string author)
    {
        try
        {
            var books = await _bookRepository.GetBooksByAuthorAsync(author);

            return new BooksResponseDTO
            {
                Success = true,
                Message = "Books retrieved successfully",
                Data = _mapper.Map<IEnumerable<BookDTO>>(books),
                TotalCount = books.Count()
            };
        }
        catch (Exception ex)
        {
            return new BooksResponseDTO
            {
                Success = false,
                Message = $"Error retrieving books by author: {ex.Message}"
            };
        }
    }

    public async Task<BooksResponseDTO> GetBooksByGenreAsync(string genre)
    {
        try
        {
            var books = await _bookRepository.GetBooksByGenreAsync(genre);

            return new BooksResponseDTO
            {
                Success = true,
                Message = "Books retrieved successfully",
                Data = _mapper.Map<IEnumerable<BookDTO>>(books),
                TotalCount = books.Count()
            };
        }
        catch (Exception ex)
        {
            return new BooksResponseDTO
            {
                Success = false,
                Message = $"Error retrieving books by genre: {ex.Message}"
            };
        }
    }

    public async Task<BookResponseDTO> AddBookAsync(CreateBookDTO bookDto)
    {
        try
        {
            // Check if ISBN is unique
            var isIsbnUnique = await _bookRepository.IsISBNUniqueAsync(bookDto.ISBN);
            if (!isIsbnUnique)
            {
                return new BookResponseDTO
                {
                    Success = false,
                    Message = "ISBN already exists"
                };
            }

            var book = _mapper.Map<Book>(bookDto);
            var result = await _bookRepository.AddBookAsync(book);

            return new BookResponseDTO
            {
                Success = true,
                Message = "Book added successfully",
                Data = _mapper.Map<BookDTO>(result)
            };
        }
        catch (Exception ex)
        {
            return new BookResponseDTO
            {
                Success = false,
                Message = $"Error adding book: {ex.Message}"
            };
        }
    }

    public async Task<BookResponseDTO> UpdateBookAsync(UpdateBookDTO bookDto)
    {
        try
        {
            // Check if the book exists
            var existingBook = await _bookRepository.GetBookByIdAsync(bookDto.Id);
            if (existingBook == null)
            {
                return new BookResponseDTO
                {
                    Success = false,
                    Message = "Book not found"
                };
            }

            // Check if the ISBN is unique (if changed)
            if (existingBook.ISBN != bookDto.ISBN)
            {
                var isIsbnUnique = await _bookRepository.IsISBNUniqueAsync(bookDto.ISBN, bookDto.Id);
                if (!isIsbnUnique)
                {
                    return new BookResponseDTO
                    {
                        Success = false,
                        Message = "ISBN already exists"
                    };
                }
            }

            var book = _mapper.Map<Book>(bookDto);
            // Preserve the existing reviews
            book.Reviews = existingBook.Reviews;

            var result = await _bookRepository.UpdateBookAsync(book);

            return new BookResponseDTO
            {
                Success = true,
                Message = "Book updated successfully",
                Data = _mapper.Map<BookDTO>(result)
            };
        }
        catch (Exception ex)
        {
            return new BookResponseDTO
            {
                Success = false,
                Message = $"Error updating book: {ex.Message}"
            };
        }
    }

    public async Task<BookResponseDTO> DeleteBookAsync(Guid id)
    {
        try
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return new BookResponseDTO
                {
                    Success = false,
                    Message = "Book not found"
                };
            }

            var result = await _bookRepository.DeleteBookAsync(id);

            return new BookResponseDTO
            {
                Success = result,
                Message = result ? "Book deleted successfully" : "Failed to delete book"
            };
        }
        catch (Exception ex)
        {
            return new BookResponseDTO
            {
                Success = false,
                Message = $"Error deleting book: {ex.Message}"
            };
        }
    }

    public async Task<bool> IsISBNUniqueAsync(string isbn, Guid? excludeId = null)
    {
        return await _bookRepository.IsISBNUniqueAsync(isbn, excludeId);
    }
}