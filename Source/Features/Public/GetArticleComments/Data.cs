using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Public.GetArticleComments;

public static class Data
{
    internal static Task<List<CommentModel>> GetCommentsForArticle(string articleID)
    {
        return DB
            .Queryable<Dom.Article>()
            .Where(a => a.ID == articleID)
            .SelectMany(a => a.Comments)
            .Select(c => new CommentModel
            {
                Comment = c.Content,
                PostedOn = c.DateAdded,
                Poster = c.NickName
            })
            .ToListAsync();
    }
}
