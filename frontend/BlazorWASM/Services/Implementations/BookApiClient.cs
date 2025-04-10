using SharedBlocks.Dtos.Books;
using System.Net.Http.Json;

namespace BlazorWASM.Services.Implementations;

public class BookApiClient(HttpClient httpClient) : IBookApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _baseUrl = "api/books";

    public async Task<BooksResponseDTO> GetAllBooksAsync(int page = 1, int pageSize = 10)
    {
        return await _httpClient.GetFromJsonAsync<BooksResponseDTO>($"{_baseUrl}?page={page}&pageSize={pageSize}")
            ?? new BooksResponseDTO { Success = false, Message = "Failed to retrieve books" };
    }

    public async Task<BookResponseDTO> GetBookByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<BookResponseDTO>($"{_baseUrl}/{id}")
                ?? new BookResponseDTO { Success = false, Message = "Failed to retrieve book" };
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new BookResponseDTO { Success = false, Message = "Book not found" };
        }
    }

    public async Task<BooksResponseDTO> GetBooksByAuthorAsync(string author)
    {
        return await _httpClient.GetFromJsonAsync<BooksResponseDTO>($"{_baseUrl}/author/{Uri.EscapeDataString(author)}")
            ?? new BooksResponseDTO { Success = false, Message = "Failed to retrieve books by author" };
    }

    public async Task<BooksResponseDTO> GetBooksByGenreAsync(string genre)
    {
        return await _httpClient.GetFromJsonAsync<BooksResponseDTO>($"{_baseUrl}/genre/{Uri.EscapeDataString(genre)}")
            ?? new BooksResponseDTO { Success = false, Message = "Failed to retrieve books by genre" };
    }

    public async Task<BookResponseDTO> AddBookAsync(CreateBookDTO bookDto)
    {
        var response = await _httpClient.PostAsJsonAsync(_baseUrl, bookDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BookResponseDTO>()
                ?? new BookResponseDTO { Success = false, Message = "Failed to parse response" };
        }

        return new BookResponseDTO { Success = false, Message = $"Error: {response.ReasonPhrase}" };
    }

    public async Task<BookResponseDTO> UpdateBookAsync(UpdateBookDTO bookDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{bookDto.Id}", bookDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BookResponseDTO>()
                ?? new BookResponseDTO { Success = false, Message = "Failed to parse response" };
        }

        return new BookResponseDTO { Success = false, Message = $"Error: {response.ReasonPhrase}" };
    }

    public async Task<BookResponseDTO> DeleteBookAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BookResponseDTO>()
                ?? new BookResponseDTO { Success = false, Message = "Failed to parse response" };
        }

        return new BookResponseDTO { Success = false, Message = $"Error: {response.ReasonPhrase}" };
    }

    public async Task<bool> IsISBNUniqueAsync(string isbn, Guid? excludeId = null)
    {
        string url = $"{_baseUrl}/check-isbn?isbn={Uri.EscapeDataString(isbn)}";

        if (excludeId.HasValue)
        {
            url += $"&excludeId={excludeId}";
        }

        return await _httpClient.GetFromJsonAsync<bool>(url);
    }
}