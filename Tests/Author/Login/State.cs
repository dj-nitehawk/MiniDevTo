namespace Tests.Author.Login;

public class State : StateFixture
{
    public string Password { get; private set; } = default!;
    public Dom.Author Author { get; private set; } = default!;

    protected override Task SetupAsync()
    {
        Password = Fake.Internet.Password();
        Author = Fake.Author(Password);

        return Author.SaveAsync();
    }

    protected override async Task TearDownAsync()
    {
        await Author.DeleteAsync();
    }
}