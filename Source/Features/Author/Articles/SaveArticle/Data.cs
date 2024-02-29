namespace Author.Articles.SaveArticle;

public static class Data
{
    internal static Task<string?> GetAuthorName(string authorID)
    {
        return DB.Find<Dom.Author, string>()
                 .MatchID(authorID)
                 .Project(a => a.FirstName + " " + a.LastName)
                 .ExecuteSingleAsync();
    }

    internal static async Task<string?> CreateOrUpdateArticle(Dom.Article article)
    {
        if (string.IsNullOrEmpty(article.ID)) //create new article
        {
            article.CreatedOn = DateTime.UtcNow;
            await article.SaveAsync();
        }
        else //update existing article
        {
            var res = await DB.Update<Dom.Article>()
                              .Match(
                                  a => a.ID == article.ID &&
                                       a.AuthorID == article.AuthorID)
                              .ModifyOnly(
                                  a => new
                                  {
                                      a.Title,
                                      a.Content
                                  },
                                  article)
                              .ExecuteAsync();

            if (res?.MatchedCount != 1)
                return null;
        }

        return article.ID;
    }
}