namespace Author.Articles.GetArticle;

public static class Data
{
    internal static Task<Response> GetArticle(string articleID)
    {
        return DB
            .Find<Dom.Article, Response>()
            .MatchID(articleID)
            .Project(a => new()
            {
                ArticleID = a.ID,
                Content = a.Content,
                Title = a.Title
            })
            .ExecuteSingleAsync();
    }
}
