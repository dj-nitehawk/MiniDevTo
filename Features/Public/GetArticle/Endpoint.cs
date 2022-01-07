namespace Public.GetArticle;

public class Endpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/public/get-article/{ArticleID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        Response = await Data.GetArticle(r.ArticleID);
    }
}