namespace Author.Login;

public class Endpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/author/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var author = await Data.GetAuthor(r.UserName);

        if (author.passwordHash is null)
            ThrowError("No author found with that username!");

        if (!BCrypt.Net.BCrypt.Verify(r.Password, author.passwordHash))
            ThrowError("Password is incorrect!");

        var authorPermissions = new[]
        {
            Allow.Article_Get_Own_List,
            Allow.Article_Save_Own,
            Allow.Author_Update_Own_Profile,
            Allow.Author_Delete_Own_Article
        };

        Response.FullName = author.fullName;
        Response.UserPermissions = new Allow().NamesFor(authorPermissions);
        Response.Token.ExpiryDate = DateTime.UtcNow.AddHours(4);
        Response.Token.Value = JWTBearer.CreateToken(
            signingKey: Config["JwtSigningKey"],
            expireAt: DateTime.UtcNow.AddHours(4),
            permissions: authorPermissions,
            claims: (Claim.AuthorID, author.authorID));

        await SendAsync(Response);
    }
}