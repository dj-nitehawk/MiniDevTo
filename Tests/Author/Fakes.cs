using Bogus;

namespace Tests.Author;

static class Fakes
{
    internal static Dom.Author Author(this Faker f, string? password = null)
        => new()
        {
            FirstName = f.Name.FirstName(),
            LastName = f.Name.LastName(),
            UserName = f.Internet.UserName(),
            Email = f.Internet.Email(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password ?? f.Internet.Password()),
            SignUpDate = f.Date.RecentDateOnly()
        };
}