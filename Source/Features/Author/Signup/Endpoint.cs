namespace Author.Signup;

public class Endpoint : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Post("/author/signup");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var author = Map.ToEntity(r);

        var emailIsTaken = await Data.EmailAddressIsTaken(author.Email);
        if (emailIsTaken)
            AddError(r => r.Email, "Email address is already in use!");

        var userNameIsTaken = await Data.UserNameIsTaken(author.UserName.ToLower());
        if (userNameIsTaken)
            AddError(r => r.UserName, "Username is not available!");

        ThrowIfAnyErrors();

        await Data.CreateNewAuthor(author);

        await SendAsync(new()
        {
            Message = "Thank you for signing up as an author!"
        });
    }
}