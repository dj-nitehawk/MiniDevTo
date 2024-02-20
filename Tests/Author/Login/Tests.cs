using Author.Login;
using MiniDevTo.Auth;

namespace Tests.Author.Login;

public class Tests(App a, State s, ITestOutputHelper o) : TestClass<App, State>(a, s, o)
{
    [Fact]
    public async Task Invalid_Login_Credentials()
    {
        var req = new Request
        {
            UserName = State.Author.UserName,
            Password = Fake.Internet.Password() //incorrect password
        };

        var (rsp, res) = await App.Client.POSTAsync<Endpoint, Request, ErrorResponse>(req);

        rsp.IsSuccessStatusCode.Should().BeFalse();
        res!.Errors["GeneralErrors"][0].Should().Be("Invalid login credentials!");
    }

    [Fact]
    public async Task Login_Success()
    {
        var req = new Request
        {
            UserName = State.Author.UserName,
            Password = State.Password //correct password
        };

        var (rsp, res) = await App.Client.POSTAsync<Endpoint, Request, Response>(req);

        rsp.IsSuccessStatusCode.Should().BeTrue();

        var permissionNames = Allow.NamesFor(Allow.Author);
        res.UserPermissions.Should().Equal(permissionNames);
        res.FullName.Should().Be(State.Author.FirstName + " " + State.Author.LastName);
        res.Token.Value.Should().Contain(".").And.Subject.Length.Should().BeGreaterThan(10);
    }
}