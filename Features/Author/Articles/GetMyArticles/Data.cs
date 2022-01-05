namespace Author.Articles.GetMyArticles;

public static class Data
{
    internal static Task<List<Response.Article>?> GetArticlesForAuthor(string authorID)
    {
        return DB
            .Find<Dom.Article, Response.Article>()
            .Match(a => a.AuthorID == authorID)
            .Project(a => new()
            {
                IsApproved = a.IsApproved,
                ArticleID = a.ID,
                RejectionReason = a.RejectionReason,
                Title = a.Title
            })
            .ExecuteAsync();
    }
}
