namespace Public.GetArticle;

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
                AuthorName = a.AuthorName,
                CommentCount = a.Comments.Length,
                Content = a.Content,
                CreatedOn = a.CreatedOn,
                Title = a.Title
            })
            .ExecuteSingleAsync();
    }
}
