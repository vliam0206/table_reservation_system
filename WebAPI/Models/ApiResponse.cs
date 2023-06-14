namespace WebAPI.Models;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public object? Data { get; set; }
}
