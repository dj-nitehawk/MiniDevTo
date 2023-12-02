namespace Author.Signup;

public static class Data
{
    internal static Task<bool> EmailAddressIsTaken(string email)
        => DB.Find<Dom.Author>()
             .Match(a => a.Email == email)
             .ExecuteAnyAsync();

    internal static Task<bool> UserNameIsTaken(string lowerCaseUserName)
        => DB.Find<Dom.Author>()
             .Match(a => a.UserName.ToLower() == lowerCaseUserName)
             .ExecuteAnyAsync();

    internal static Task CreateNewAuthor(Dom.Author author)
        => author.SaveAsync();
}