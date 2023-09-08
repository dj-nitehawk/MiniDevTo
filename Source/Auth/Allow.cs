namespace MiniDevTo.Auth;

public static partial class Allow
{
    public const string Article_Delete = "100";
    public const string Article_Update = "101";
    public const string Author_Update = "102";

    static partial void Groups()
    {
        AddToAuthor(Author_Update);

        AddToAdmin(Article_Delete);
        AddToAdmin(Article_Update);
        AddToAdmin(Author_Update);
    }
}