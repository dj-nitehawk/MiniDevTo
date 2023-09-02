using Author.Login;
using MiniDevTo.Auth;
using MongoDB.Entities;

namespace Author;

public class LoginTests : TestBase
{
    public LoginTests(AppFixture fixture) : base(fixture) { }

    private static string _username = default!;
    private static string _password = default!;
    private static string _fullName = default!;

    public override async Task InitializeAsync()
    {
        if (_username is null && _password is null)
        {
            _username = F.Internet.UserName();
            _password = F.Internet.Password(10);

            var author = new Dom.Author
            {
                FirstName = F.Name.FirstName(),
                LastName = F.Name.LastName(),
                UserName = _username,
                Email = F.Internet.Email(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(_password),
                SignUpDate = F.Date.RecentDateOnly()
            };
            await author.SaveAsync();
            _fullName = author.FirstName + " " + author.LastName;
        }
    }

    [Fact]
    public async Task Invalid_Login_Credentials()
    {
        var req = new Request
        {
            UserName = _username,
            Password = F.Internet.Password()
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
            UserName = _username,
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
        res.FullName.Should().Be(_fullName);
        res.Token.Value.Should().Contain(".").And.Subject.Length.Should().BeGreaterThan(10);
    }
}