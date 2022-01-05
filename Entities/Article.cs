namespace Dom;

public class Article : Entity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorName { get; set; }
    public DateTime PublishedOn { get; set; }
    public int CommentCount { get; set; }
    public bool IsApproved { get; set; }
    [IgnoreDefault] public string? RejectionReason { get; set; }
}
