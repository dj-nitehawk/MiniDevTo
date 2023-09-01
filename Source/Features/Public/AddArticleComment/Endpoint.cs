namespace Public.AddArticleComment;

public class Endpoint : Endpoint<Request, EmptyResponse, Mapper>
{
    public override void Configure()
    {
        Post("/public/add-article-comment");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await Data.AddCommentToArticle(r.ArticleID, Map.ToEntity(r));
        await SendOkAsync();
    }
}