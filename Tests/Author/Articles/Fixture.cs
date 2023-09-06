﻿using FastEndpoints.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniDevTo.Auth;

namespace Tests.Author.Articles;

public class Fixture : TestFixture<Program>
{
    public Fixture(IMessageSink s) : base(s) { }

    public List<string> ArticleIDs { get; set; } = new();

    private string _authorID = default!;

    protected override async Task SetupAsync()
    {
        var author = Fake.Author();
        await author.SaveAsync();
        _authorID = author.ID!;

        var jwtKey = Services.GetRequiredService<IConfiguration>()["JwtSigningKey"];
        var bearerToken = JWTBearer.CreateToken(jwtKey!, u =>
        {
            u[Claim.AuthorID] = _authorID;
            u.Permissions.Add(PermCodes.Author);
        });

        Client = CreateClient(c =>
        {
            c.DefaultRequestHeaders.Authorization = new("Bearer", bearerToken);
        });
    }

    protected override async Task TearDownAsync()
    {
        Client.Dispose();
        await DB.DeleteAsync<Dom.Author>(_authorID);
        await DB.DeleteAsync<Dom.Article>(a => a.AuthorID == _authorID);
    }
}