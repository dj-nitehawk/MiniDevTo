namespace Author.Articles.DeleteArticle;

public class Endpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Delete("/author/articles/delete-article/{ArticleID}");
        Claims(Claim.AuthorID);
        AccessControl(
            keyName: "Author_Delete_Own_Article",
            behavior: Apply.ToThisEndpoint,
            groupNames: "Author");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        if (await Data.DeleteArticle(r.ArticleID, r.AuthorID))
        {
            Response.Message = "Article Deleted!";
            return;
        }

        ThrowError("Article Delete Failed!");
    }
}