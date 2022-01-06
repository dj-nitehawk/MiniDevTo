namespace Admin.ArticleModeration.Reject;

public class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/admin/article-moderation/reject");
        Claims(Claim.AdminID);
        Permissions(Allow.Article_Moderate);
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await Data.RejectArticle(r.ArticleID, r.RejectionReason);
        await SendOkAsync();
    }
}