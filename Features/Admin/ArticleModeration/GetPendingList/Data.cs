namespace Admin.ArticleModeration.GetPendingList;

public static class Data
{
    internal static Task<List<ArticleModel>> GetPendingArticles()
    {
        return DB
            .Find<Dom.Article, ArticleModel>()
            .Match(a => !a.IsApproved && a.RejectionReason == null)
            .Project(a => new()
            {
                ArticleID = a.ID,
                AuthorName = a.AuthorName,
                CreatedOn = a.CreatedOn,
                Title = a.Title,
            })
            .Sort(a => a.CreatedOn, Order.Ascending)
            .ExecuteAsync();
    }
}