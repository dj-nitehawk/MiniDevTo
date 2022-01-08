namespace Public.AddArticleComment;

public static class Data
{
    internal static Task AddCommentToArticle(string articleID, Dom.Article.Comment comment)
    {
        return DB
            .Update<Dom.Article>()
            .MatchID(articleID)
            .Modify(b => b.AddToSet(a => a.Comments, comment))
            .ExecuteAsync();
    }
}
