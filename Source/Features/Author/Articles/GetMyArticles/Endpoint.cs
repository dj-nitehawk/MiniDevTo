namespace Author.Articles.GetMyArticles;

public class Endpoint : Endpoint<Request, IEnumerable<Article>>
{
    public override void Configure()
    {
        Get("/author/articles/get-my-articles");
        Claims(Claim.AuthorID);
        Permissions(Allow.Article_Get_Own_List);
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        Response = await Data.GetArticlesForAuthor(r.AuthorID);
    }
}