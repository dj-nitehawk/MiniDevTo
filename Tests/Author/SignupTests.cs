using Author.Signup;

namespace Author;

public class SignupTests : TestBase
{
    public SignupTests(AppFixture fixture) : base(fixture) { }

    [Fact]
    public async void SignUp_Input_Validation_Failures()
    {
        var req = new Request();
        var (rsp, res) = await App.GuestClient.POSTAsync<Endpoint, Request, ErrorResponse>(req);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res!.Errors.Count.Should().Be(4);
        res.Errors.Keys.Should().Equal(
            nameof(Request.FirstName),
            nameof(Request.Email),
            nameof(Request.UserName),
            nameof(Request.Password));
    }

    [Fact]
    public async Task Duplicate_Email_And_Username_Check()
    {
        var req = new Request
        {
            FirstName = F.Name.FirstName(),
            LastName = F.Name.LastName(),
            UserName = F.Internet.UserName(),
            Email = F.Internet.Email(),
            Password = F.Internet.Password()
        };

        var (rsp1, _) = await App.GuestClient.POSTAsync<Endpoint, Request, Response>(req);
        rsp1.IsSuccessStatusCode.Should().BeTrue();

        var (rsp2, res2) = await App.GuestClient.POSTAsync<Endpoint, Request, ErrorResponse>(req);
        rsp2.IsSuccessStatusCode.Should().BeFalse();
        res2!.Errors.Keys.Should().Equal(
            nameof(Request.Email),
            nameof(Request.UserName));
    }

    [Fact]
    public async Task SignUp_Success()
    {
        var req = new Request
        {
            FirstName = F.Name.FirstName(),
            LastName = F.Name.LastName(),
            UserName = F.Internet.UserName(),
            Email = F.Internet.Email(),
            Password = F.Internet.Password()
        };

        var (rsp, res) = await App.GuestClient.POSTAsync<Endpoint, Request, Response>(req);

        rsp.IsSuccessStatusCode.Should().BeTrue();
        res!.Message.Should().Be("Thank you for signing up as an author!");
    }
}