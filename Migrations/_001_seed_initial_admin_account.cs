namespace MiniDevTo.Migrations;

public class _001_seed_initial_admin_account : IMigration
{
    internal static string SuperAdminPassword { get; set; }

    public async Task UpgradeAsync()
    {
        await new Dom.Admin
        {
            Email = "domo@arigatho.des",
            ID = "SUPERADMIN01",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(SuperAdminPassword),
            UserName = "DanielSilver"
        }.SaveAsync();
    }
}
