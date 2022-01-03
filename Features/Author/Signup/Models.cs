namespace Author.Signup;

public class Request
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class Validator : Validator<Request>
{
    public Validator()
    {

    }
}

public class Response
{
    public string Message { get; set; }
}
