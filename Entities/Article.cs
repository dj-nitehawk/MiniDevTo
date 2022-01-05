namespace Dom;

public class Article : Entity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorName { get; set; }

    [Preserve] public DateTime CreatedOn { get; set; }
    [Preserve] public bool IsApproved { get; set; }
    [Preserve, IgnoreDefault] public string? RejectionReason { get; set; }
    [Preserve, IgnoreDefault] public Comment[] Comments { get; set; } = Array.Empty<Comment>();

    public class Comment
    {
        public string ID { get; set; }
        public string NickName { get; set; }
        public string Content { get; set; }
    }
}