using MongoDB.Bson;

namespace Public.AddArticleComment;

public class Mapper : Mapper<Request, object, Dom.Article.Comment>
{
    public override Dom.Article.Comment ToEntity(Request r) => new()
    {
        Content = r.Comment,
        NickName = r.NickName,
        ID = ObjectId.GenerateNewId().ToString(),
        DateAdded = DateTime.UtcNow
    };
}