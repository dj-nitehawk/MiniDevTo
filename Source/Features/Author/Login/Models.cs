namespace Author.Login;

public class Request
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required!");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required!");
    }
}

public class Response
{
    public string FullName { get; set; }
    public IEnumerable<string> UserPermissions { get; set; }
    public JwtToken Token { get; set; } = new();
}