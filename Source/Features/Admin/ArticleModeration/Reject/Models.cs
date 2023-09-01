namespace Admin.ArticleModeration.Reject;

public class Request
{
    public string ArticleID { get; set; }
    public string RejectionReason { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.ArticleID)
            .NotEmpty().WithMessage("Article ID cannot be blank!");

        RuleFor(x => x.RejectionReason)
            .NotEmpty().WithMessage("Rejection reason cannot be blank!")
            .MinimumLength(10).WithMessage("Rejection reason is too short!");
    }
}