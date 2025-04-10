using API.SERVICE.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedBlocks.Dtos.Books;

namespace API.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BooksController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService;

    [HttpGet]
    public async Task<ActionResult<BooksResponseDTO>> GetAllBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _bookService.GetAllBooksAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponseDTO>> GetBookById(Guid id)
    {
        var result = await _bookService.GetBookByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("author/{author}")]
    public async Task<ActionResult<BooksResponseDTO>> GetBooksByAuthor(string author)
    {
        var result = await _bookService.GetBooksByAuthorAsync(author);
        return Ok(result);
    }

    [HttpGet("genre/{genre}")]
    public async Task<ActionResult<BooksResponseDTO>> GetBooksByGenre(string genre)
    {
        var result = await _bookService.GetBooksByGenreAsync(genre);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookResponseDTO>> AddBook(CreateBookDTO bookDto)
    {
        var result = await _bookService.AddBookAsync(bookDto);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetBookById), new { id = result.Data.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookResponseDTO>> UpdateBook(Guid id, UpdateBookDTO bookDto)
    {
        if (id != bookDto.Id)
            return BadRequest(new BookResponseDTO { Success = false, Message = "ID mismatch" });

        var result = await _bookService.UpdateBookAsync(bookDto);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
                return NotFound(result);

            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<BookResponseDTO>> DeleteBook(Guid id)
    {
        var result = await _bookService.DeleteBookAsync(id);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
                return NotFound(result);

            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("check-isbn")]
    public async Task<ActionResult<bool>> CheckIsbnUnique([FromQuery] string isbn, [FromQuery] Guid? excludeId = null)
    {
        var result = await _bookService.IsISBNUniqueAsync(isbn, excludeId);
        return Ok(result);
    }
}