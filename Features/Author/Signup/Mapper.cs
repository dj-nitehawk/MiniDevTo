using System.Globalization;

namespace Author.Signup;

public class Mapper : Mapper<Request, Response, Dom.Author>
{
    private static readonly CultureInfo _culture = new CultureInfo("en-US");

    public override Dom.Author ToEntity(Request r) => new()
    {
        Email = r.Email.ToLower(),
        FirstName = _culture.TextInfo.ToTitleCase(r.FirstName),
        LastName = _culture.TextInfo.ToTitleCase(r.LastName),
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(r.Password),
        SignUpDate = DateOnly.FromDateTime(DateTime.UtcNow),
        UserName = r.UserName
    };
}