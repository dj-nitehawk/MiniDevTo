namespace Author.Signup;

public static class Data
{
    internal static Task<bool> EmailAddressIsTaken(string email)
    {
        return DB
            .Find<Dom.Author>()
            .Match(a => a.Email == email)
            .ExecuteAnyAsync();
    }

    internal static Task<bool> UserNameIsTaken(string loweCaseUserName)
    {
        return DB
            .Find<Dom.Author>()
            .Match(a => a.UserName.ToLower() == loweCaseUserName)
            .ExecuteAnyAsync();
    }

    internal static Task CreateNewAuthor(Dom.Author author)
    {
        return author.SaveAsync();
    }
}
