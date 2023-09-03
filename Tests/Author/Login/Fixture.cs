using MongoDB.Entities;

namespace Tests.Author.Login;

public class Fixture : TestFixture<Program>
{
    public Fixture(IMessageSink s) : base(s) { }

    public string Password { get; private set; } = default!;
    public Dom.Author Author { get; private set; } = default!;

    protected override async Task SetupAsync()
    {
        Password = Fake.Internet.Password(10);
        Author = Fake.Author(Password);
        await Author.SaveAsync();
    }

    protected override async Task TearDownAsync()
    {
        await Author.DeleteAsync();
    }
}