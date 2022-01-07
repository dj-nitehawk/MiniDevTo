namespace Public.GetArticleList;

public static class Data
{
    internal static Task<List<ArticleModel>> GetRecentArticles()
    {
        return DB
            .Find<Dom.Article, ArticleModel>()
            .Match(a => a.IsApproved)
            .Sort(a => a.CreatedOn, Order.Descending)
            .Project(a => new()
            {
                ArticleID = a.ID,
                AuthorName = a.AuthorName,
                CommentCount = a.Comments.Length,
                Title = a.Title,
                CreatedOn = a.CreatedOn
            })
            .ExecuteAsync();
    }
}
