namespace Public.AddArticleComment;

public class Request
{
    public string ArticleID { get; set; }
    public string NickName { get; set; }
    public string Comment { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.ArticleID)
            .NotEmpty().WithMessage("Need article ID!");

        RuleFor(x => x.NickName)
            .NotEmpty().WithMessage("Need a nickname!")
            .MinimumLength(3).WithMessage("Nickname is too short!");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Need a comment!")
            .MinimumLength(3).WithMessage("Comment is too short!");
    }
}