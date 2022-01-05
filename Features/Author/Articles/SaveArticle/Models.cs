using MongoDB.Bson;

namespace Author.Articles.SaveArticle;

public class Request : ArticleModel
{
    [FromClaim]
    public string AuthorID { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.ArticleID)
            .Must(id => ObjectId.TryParse(id, out _))
            .When(x => !string.IsNullOrEmpty(x.ArticleID))
            .WithMessage("Article Id is invalid!");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required!")
            .MinimumLength(10).WithMessage("Title is too short!");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required!")
            .MinimumLength(10).WithMessage("Content is too short!");
    }
}

public class Response
{
    public string Message => "Article saved!";
    public string? ArticleID { get; set; }
}
