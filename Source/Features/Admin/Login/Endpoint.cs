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

        Response.UserName = r.UserName;
        Response.UserPermissions = Allow.NamesFor(PermCodes.Admin);
        Response.Token.ExpiryDate = DateTime.UtcNow.AddHours(4);
        Response.Token.Value = JWTBearer.CreateToken(
            signingKey: Config["JwtSigningKey"],
            expireAt: DateTime.UtcNow.AddHours(4),
            permissions: PermCodes.Admin,
            claims: (Claim.AdminID, adminID));
    }
}