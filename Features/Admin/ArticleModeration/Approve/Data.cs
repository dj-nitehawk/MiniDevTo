namespace Admin.ArticleModeration.Approve;

public static class Data
{
    internal static Task ApproveArticle(string articleID)
    {
        return DB
            .Update<Dom.Article>()
            .MatchID(articleID)
            .Modify(a => a.IsApproved, true)
            .ExecuteAsync();
    }
}
