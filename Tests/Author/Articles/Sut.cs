﻿using Dom;
using FastEndpoints.Security;
using Microsoft.Extensions.Configuration;
using MiniDevTo.Auth;

namespace Tests.Author.Articles;

public class Sut : AppFixture<Program>
{
    //this is a stateful AppFixture because author-id is needed to configure the httpclient (in order to generate the JWT with a AuthorID claim).

    public List<string> ArticleIDs { get; set; } = [];

    string _authorID = default!;

    protected override async Task SetupAsync()
    {
        var author = Fake.Author();
        await author.SaveAsync();
        _authorID = author.ID;

        var jwtKey = Services.GetRequiredService<IConfiguration>()["JwtSigningKey"];
        var bearerToken = JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = jwtKey!;
                o.User.Permissions.AddRange(Allow.Author);
                o.User[Claim.AuthorID] = _authorID; //this is why this fixture is stateful
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