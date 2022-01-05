using Dom;
using MongoDB.Bson;

namespace Author.Articles.SaveArticle;

public static class Data
{
    internal static Task<string> GetAuthorName(string authorID)
    {
        return DB
            .Find<Dom.Author, string>()
            .MatchID(authorID)
            .Project(a => a.FirstName + " " + a.LastName)
            .ExecuteSingleAsync();
    }

    internal static async Task<string> CreateOrUpdateArticle(Article article)
    {
        if (article.ID is null) //create new article
        {
            article.CreatedOn = DateTime.UtcNow;
            await article.SaveAsync();
        }
        else //update existing article
        {
            await article.SavePreservingAsync();
        }

        return article.ID!;
    }
}
