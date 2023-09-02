using Bogus;
using MongoDB.Entities;

namespace Tests.Fixtures;

public class AuthorFixture : IAsyncLifetime
{
    private static readonly Faker F = new();

    public string Password { get; private set; } = default!;
    public Dom.Author Author { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        Password = F.Internet.Password(10);
        Author = new Dom.Author
        {
            FirstName = F.Name.FirstName(),
            LastName = F.Name.LastName(),
            UserName = F.Internet.UserName(),
            Email = F.Internet.Email(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
            SignUpDate = F.Date.RecentDateOnly()
        };
        await Author.SaveAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
