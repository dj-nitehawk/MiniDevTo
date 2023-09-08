namespace Author.Articles.GetMyArticles;

public class Endpoint : Endpoint<Request, IEnumerable<Article>>
{
    public override void Configure()
    {
        Get("/author/articles/get-my-articles");
        Claims(Claim.AuthorID);
        AccessControl(
            keyName: "Article_Get_Own_List",
            behavior: Apply.ToThisEndpoint,
            groupNames: "Author");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        Response = await Data.GetArticlesForAuthor(r.AuthorID);
    }
}