using Author.Login;
using MiniDevTo.Auth;

namespace Author;

public class LoginTests : TestBase, IClassFixture<AuthorFixture>
{
    private readonly AuthorFixture _seed;

    public LoginTests(AppFixture app, AuthorFixture author) : base(app)
    {
        _seed = author;
    }

    [Fact]
    public async Task Invalid_Login_Credentials()
    {
        var req = new Request
        {
            UserName = _seed.Author.UserName,
            Password = F.Internet.Password() //incorrect password
        };

        var (rsp, res) = await App.AuthorClient.POSTAsync<Endpoint, Request, ErrorResponse>(req);

        rsp.IsSuccessStatusCode.Should().BeFalse();
        res!.Errors["GeneralErrors"][0].Should().Be("Invalid login credentials!");
    }

    [Fact]
    public async Task Login_Success()
    {
        var req = new Request
        {
            UserName = _seed.Author.UserName,
            Password = _seed.Password //correct password
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
        res.FullName.Should().Be(_seed.Author.FirstName + " " + _seed.Author.LastName);
        res.Token.Value.Should().Contain(".").And.Subject.Length.Should().BeGreaterThan(10);
    }
}