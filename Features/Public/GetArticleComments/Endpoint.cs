namespace Public.GetArticleComments;

public class Endpoint : Endpoint<Request, List<CommentModel>>
{
    public override void Configure()
    {
        Get("/public/get-article-comments/{ArticleID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        Response = await Data.GetCommentsForArticle(r.ArticleID);
    }
}