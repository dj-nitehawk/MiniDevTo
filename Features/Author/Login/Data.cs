namespace Author.Login;

public static class Data
{
    internal static Task<Author> GetAuthor(string userName)
    {
        return DB
            .Find<Dom.Author, Author>()
            .Match(a => a.UserName == userName)
            .Project(a => new(
                a.ID,
                a.FirstName + " " + a.LastName,
                a.PasswordHash))
            .ExecuteSingleAsync();
    }

    internal record struct Author(
        string authorID,
        string fullName,
        string passwordHash)
    { }
}