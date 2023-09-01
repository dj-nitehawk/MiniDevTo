namespace Admin.ArticleModeration.Approve;

public class Endpoint : Endpoint<Request>
{
    public override void Configure()
    {
        Post("/admin/article-moderation/approve");
        Claims(Claim.AdminID);
        Permissions(Allow.Article_Moderate);
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await Data.ApproveArticle(r.ArticleID);
        await SendOkAsync();
    }
}