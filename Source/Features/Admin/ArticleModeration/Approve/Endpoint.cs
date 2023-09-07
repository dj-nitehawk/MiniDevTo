namespace Admin.ArticleModeration.Approve;

public class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/admin/article-moderation/approve");
        Claims(Claim.AdminID);
        AccessControlKey("Article_Moderate_Approve", Apply.ToThisEndpoint);
        //Permissions(Allow.Article_Moderate_Approve);
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await Data.ApproveArticle(r.ArticleID);
        await SendOkAsync();
    }
}