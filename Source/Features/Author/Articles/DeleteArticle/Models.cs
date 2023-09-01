namespace Author.Articles.DeleteArticle;

public class Request
{
    public string ArticleID { get; set; }

    [FromClaim]
    public string AuthorID { get; set; }
}

public class Response
{
    public string Message { get; set; }
}
