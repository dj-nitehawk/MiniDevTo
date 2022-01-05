using System.Text.Json.Serialization;

namespace Author.Articles.GetMyArticles;

public class Request
{
    [FromClaim]
    public string AuthorID { get; set; }
}

public class Response
{
    public IEnumerable<Article>? Articles { get; set; }

    public class Article
    {
        public string ArticleID { get; set; }
        public string Title { get; set; }
        public string? RejectionReason { get; set; }
        public string ApprovalStatus
        {
            get
            {
                if (IsApproved) return "Approved";
                return RejectionReason is null ? "Pending" : "Rejected";
            }
        }

        [JsonIgnore]
        public bool IsApproved { get; set; }

    }
}
