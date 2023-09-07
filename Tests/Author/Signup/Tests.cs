using Author.Signup;

namespace Tests.Author.Signup;

public class Tests : TestClass<Fixture>
{
    public Tests(Fixture f, ITestOutputHelper o) : base(f, o) { }

    [Fact]
    public async void SignUp_Input_Validation_Failures()
    {
        var req = new Request();
        var (rsp, res) = await Fixture.Client.POSTAsync<Endpoint, Request, ErrorResponse>(req);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(4);
        res.Errors.Keys.Should().Equal(
            nameof(Request.FirstName),
            nameof(Request.Email),
            nameof(Request.UserName),
            nameof(Request.Password));
    }

    [Fact]
    public async Task Duplicate_Email_And_Username_Check()
    {
        var req = Fake.Request();

        var (rsp1, _) = await Fixture.Client.POSTAsync<Endpoint, Request, Response>(req);
        rsp1.IsSuccessStatusCode.Should().BeTrue();

        var (rsp2, res2) = await Fixture.Client.POSTAsync<Endpoint, Request, ErrorResponse>(req);
        rsp2.IsSuccessStatusCode.Should().BeFalse();
        res2.Errors.Keys.Should().Equal(
            nameof(Request.Email),
            nameof(Request.UserName));

        await DB.DeleteAsync<Dom.Author>(a => a.UserName == req.UserName);
    }

    [Fact]
    public async Task SignUp_Success()
    {
        var req = Fake.Request();

        var (rsp, res) = await Fixture.Client.POSTAsync<Endpoint, Request, Response>(req);

        rsp.IsSuccessStatusCode.Should().BeTrue();
        res.Message.Should().Be("Thank you for signing up as an author!");

        await DB.DeleteAsync<Dom.Author>(a => a.UserName == req.UserName);
    }
}