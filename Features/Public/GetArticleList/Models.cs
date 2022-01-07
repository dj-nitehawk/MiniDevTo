using System.Text.Json.Serialization;

namespace Public.GetArticleList;

public class ArticleModel
{
    public string ArticleID { get; set; }
    public string Title { get; set; }
    public int CommentCount { get; set; }
    public string AuthorName { get; set; }
    public string CreationDate { get => CreatedOn.Date.ToShortDateString(); }

    [JsonIgnore]
    public DateTime CreatedOn { get; set; }
}
