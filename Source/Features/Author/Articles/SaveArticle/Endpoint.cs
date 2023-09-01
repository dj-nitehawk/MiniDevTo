namespace Author.Articles.SaveArticle;

public class Endpoint : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Post("/author/articles/save-article");
        Claims(Claim.AuthorID);
        Permissions(Allow.Article_Save_Own);
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        Response.ArticleID = await Data.CreateOrUpdateArticle(
            await Map.ToEntityAsync(r));

        if (Response.ArticleID is null)
            ThrowError("Unable to save the article!");

        await SendAsync(Response);
    }
}