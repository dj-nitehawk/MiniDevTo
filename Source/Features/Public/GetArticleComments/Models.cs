namespace Public.GetArticleComments;

public class Request
{
    public string ArticleID { get; set; }
}

public class CommentModel
{
    public string Poster { get; set; }
    public string Comment { get; set; }
    public DateTime PostedOn { get; set; }
}
