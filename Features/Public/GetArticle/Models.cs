using System.Text.Json.Serialization;

namespace Public.GetArticle;

public class Request
{
    public string ArticleID { get; set; }
}

public class Response
{
    public string ArticleID { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int CommentCount { get; set; }
    public string AuthorName { get; set; }
    public string CreationDate { get => CreatedOn.Date.ToShortDateString(); }

    [JsonIgnore]
    public DateTime CreatedOn { get; set; }
}
