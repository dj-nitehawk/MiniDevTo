using Author.Login;
using MiniDevTo.Auth;

namespace Tests.Author.Login;

public class Tests : TestClass<Fixture>
{
    public Tests(Fixture f, ITestOutputHelper o) : base(f, o) { }

    [Fact]
    public async Task Invalid_Login_Credentials()
    {
        var req = new Request
        {
            UserName = Fixture.Author.UserName,
            Password = Fake.Internet.Password() //incorrect password
        };

        var (rsp, res) = await Fixture.Client.POSTAsync<Endpoint, Request, ErrorResponse>(req);

        rsp.IsSuccessStatusCode.Should().BeFalse();
        res!.Errors["GeneralErrors"][0].Should().Be("Invalid login credentials!");
    }

    [Fact]
    public async Task Login_Success()
    {
        var req = new Request
        {
            UserName = Fixture.Author.UserName,
            Password = Fixture.Password //correct password
        };

        var (rsp, res) = await Fixture.Client.POSTAsync<Endpoint, Request, Response>(req);

        rsp.IsSuccessStatusCode.Should().BeTrue();

        var permissionNames = Allow.NamesFor(PermCodes.Author);
        res!.UserPermissions.Should().Equal(permissionNames);
        res.FullName.Should().Be(Fixture.Author.FirstName + " " + Fixture.Author.LastName);
        res.Token.Value.Should().Contain(".").And.Subject.Length.Should().BeGreaterThan(10);
    }
}