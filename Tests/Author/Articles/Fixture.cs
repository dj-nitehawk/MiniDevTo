using Dom;
using FastEndpoints.Security;
using Microsoft.Extensions.Configuration;
using MiniDevTo.Auth;

namespace Tests.Author.Articles;

public class Fixture(IMessageSink s) : AppFixture<Program>(s)
{
    //this is a stateful AppFixture because author id is needed to configure the client's AuthorID claim

    public List<string> ArticleIDs { get; set; } = [];

    string _authorID = default!;

    protected override async Task SetupAsync()
    {
        var author = Fake.Author();
        await author.SaveAsync();
        _authorID = author.ID!;

        var jwtKey = Services.GetRequiredService<IConfiguration>()["JwtSigningKey"];
        var bearerToken = JWTBearer.CreateToken(
            jwtKey!,
            u =>
            {
                u[Claim.AuthorID] = _authorID; //this is why this fixture is stateful
                u.Permissions.AddRange(Allow.Author);
            });

        Client = CreateClient(c => c.DefaultRequestHeaders.Authorization = new("Bearer", bearerToken));
    }

    protected override async Task TearDownAsync()
    {
        Client.Dispose();
        await DB.DeleteAsync<Dom.Author>(_authorID);
        await DB.DeleteAsync<Article>(a => a.AuthorID == _authorID);
    }
}