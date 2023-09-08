namespace Admin.ArticleModeration.Reject;

public class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/admin/article-moderation/reject");
        Claims(Claim.AdminID);
        AccessControl(
            keyName: "Article_Moderate_Reject",
            behavior: Apply.ToThisEndpoint,
            groupNames: "Admin");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await Data.RejectArticle(r.ArticleID, r.RejectionReason);
        await SendOkAsync();
    }
}