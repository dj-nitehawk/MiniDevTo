namespace Admin.ArticleModeration.GetPendingList;

public class ArticleModel
{
    public string ArticleID { get; set; }
    public string Title { get; set; }
    public DateTime CreatedOn { get; set; }
    public string AuthorName { get; set; }
}
