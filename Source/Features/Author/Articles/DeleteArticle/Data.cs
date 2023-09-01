using Dom;

namespace Author.Articles.DeleteArticle;

public static class Data
{
    internal static async Task<bool> DeleteArticle(string articleID, string authorID)
    {
        return (await DB
            .DeleteAsync<Article>(a =>
                a.ID == articleID &&
                a.AuthorID == authorID)
            ).DeletedCount > 0;
    }
}
