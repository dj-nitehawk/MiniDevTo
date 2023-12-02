namespace MiniDevTo.Auth;

public class JwtToken
{
    public string Value { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
}