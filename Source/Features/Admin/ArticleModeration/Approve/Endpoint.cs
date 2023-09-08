namespace Admin.ArticleModeration.Approve;

public class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/admin/article-moderation/approve");
        Claims(Claim.AdminID);
        AccessControl(
            keyName: "Article_Moderate_Approve",
            behavior: Apply.ToThisEndpoint,
            groupNames: "Admin");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await Data.ApproveArticle(r.ArticleID);
        await SendOkAsync();
    }
}