namespace BattleshipApi.Models;

public class ApiResponse<T>
{
    public T? Result { get; set; }
    public string? Message { get; set; }
}
