namespace MiniDevTo.Auth;

public static class PermCodes
{
    public static string[] Admin { get; } =
    {
        Allow.Article_Moderate_Reject,
        Allow.Article_Moderate_Approve,
        Allow.Article_Delete,
        Allow.Article_Get_Pending_List,
        Allow.Article_Update,
        Allow.Author_Update
    };

    public static string[] Author { get; } =
    {
        Allow.Article_Get_Own_List,
        Allow.Article_Save_Own,
        Allow.Author_Update,
        Allow.Author_Delete_Own_Article
    };
}