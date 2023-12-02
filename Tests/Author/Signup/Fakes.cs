using Author.Signup;
using Bogus;

namespace Tests.Author.Signup;

static class Fakes
{
    internal static Request Request(this Faker f)
        => new()
        {
            FirstName = f.Name.FirstName(),
            LastName = f.Name.LastName(),
            UserName = f.Internet.UserName(),
            Email = f.Internet.Email(),
            Password = f.Internet.Password()
        };
}