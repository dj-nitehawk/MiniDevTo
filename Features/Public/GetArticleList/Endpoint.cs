namespace Public.GetArticleList;

public class Endpoint : EndpointWithoutRequest<List<ArticleModel>>
{
    public override void Configure()
    {
        Get("/public/get-article-list");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest _, CancellationToken ct)
    {
        //alternative for SendAsync()
        Response = await Data.GetRecentArticles();
    }
}