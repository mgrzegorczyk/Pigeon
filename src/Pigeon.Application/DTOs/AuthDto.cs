namespace Pigeon.Application.DTOs;

public class AuthDto
{
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}