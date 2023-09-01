using Author.Login;
using MiniDevTo.Auth;

namespace Author;

public class LoginTests : TestBase
{
    public LoginTests(AppFixture fixture) : base(fixture) { }

    private static readonly string _userName = Guid.NewGuid().ToString("N");
    private static readonly string _password = Guid.NewGuid().ToString("N");

    [Fact, Priority(1)]
    public async Task SignUp_Success()
    {
        var req = new Signup.Request
        {
            UserName = _userName,
            Password = _password,
            Email = $"{Guid.NewGuid():N}@blah.com",
            FirstName = "first",
            LastName = "last"
        };

        var (rsp, res) = await App.GuestClient.POSTAsync<Signup.Endpoint, Signup.Request, Signup.Response>(req);

        rsp.IsSuccessStatusCode.Should().BeTrue();
        res!.Message.Should().Be("Thank you for signing up as an author!");
    }

    [Fact, Priority(2)]
    public async Task Invalid_Login_Credentials()
    {
        var req = new Request
        {
            UserName = _userName,
            Password = Guid.NewGuid().ToString()
        };

        var (rsp, res) = await App.AuthorClient.POSTAsync<Endpoint, Request, ErrorResponse>(req);

        rsp.IsSuccessStatusCode.Should().BeFalse();
        res!.Errors["GeneralErrors"][0].Should().Be("Invalid login credentials!");
    }

    [Fact, Priority(3)]
    public async Task Login_Success()
    {
        var req = new Request
        {
            UserName = _userName,
            Password = _password
        };

        var (rsp, res) = await App.AuthorClient.POSTAsync<Endpoint, Request, Response>(req);

        rsp.IsSuccessStatusCode.Should().BeTrue();

        var permissionCodes = new[]
        {
            Allow.Article_Get_Own_List,
            Allow.Article_Save_Own,
            Allow.Author_Update_Own_Profile,
            Allow.Author_Delete_Own_Article
        };
        var permissionNames = new Allow().NamesFor(permissionCodes);
        res!.UserPermissions.Should().Equal(permissionNames);
        res.FullName.Should().Be("First Last");
        res.Token.Value.Should().Contain(".").And.Subject.Length.Should().BeGreaterThan(10);
    }
}