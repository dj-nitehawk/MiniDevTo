namespace Admin.Login;

public class Endpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("/admin/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var (adminID, passwordHash) = await Data.GetAdmin(r.UserName);

        if (passwordHash is null || !BCrypt.Net.BCrypt.Verify(r.Password, passwordHash))
            ThrowError("Invalid login details!");

        var adminPemissions = new[]
        {
            Allow.Article_Moderate,
            Allow.Article_Delete,
            Allow.Article_Get_Pending_List,
            Allow.Article_Update,
            Allow.Author_Update_Profile
        };

        Response.UserName = r.UserName;
        Response.UserPermissions = new Allow().NamesFor(adminPemissions);
        Response.Token.ExpiryDate = DateTime.UtcNow.AddHours(4);
        Response.Token.Value = JWTBearer.CreateToken(
            signingKey: Config["JwtSigningKey"],
            expireAt: DateTime.UtcNow.AddHours(4),
            permissions: adminPemissions,
            claims: (Claim.AdminID, adminID));
    }
}