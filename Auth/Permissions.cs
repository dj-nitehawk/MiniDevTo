namespace MiniDevTo.Auth;

public class Allow : Permissions
{

    //ADMIN PERMISSIONS
    public const string Article_Moderate = "100";
    public const string Article_Delete = "101";
    public const string Article_Get_Pending_List = "102";
    public const string Article_Update = "103";
    public const string Author_Update_Profile = "104";

    //AUTHOR PERMISSIONS
    public const string Article_Get_Own_List = "200";
    public const string Article_Save_Own = "201";
    public const string Author_Update_Own_Profile = "202";
    public const string Author_Delete_Own_Article = "202";
}
