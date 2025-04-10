namespace SharedBlocks.Dtos.Response;

public record Result
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = [];
}

public record Result<T> : Result
{
    public T? Value { get; set; }
    
    public static Result<T> Success(T value) => new() { Value = value, IsSuccess = true };
    public static Result<T> Failure(string errorMessage) => new() { Errors = [errorMessage], IsSuccess = false };
}
