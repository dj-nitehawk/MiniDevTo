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

        if (string.IsNullOrEmpty(passwordHash) || !BCrypt.Net.BCrypt.Verify(r.Password, passwordHash))
            ThrowError("Invalid login details!");

        Response.UserName = r.UserName;
        Response.UserPermissions = Allow.NamesFor(Allow.Admin);
        Response.Token.ExpiryDate = DateTime.UtcNow.AddHours(4);
        Response.Token.Value = JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = Config["JwtSigningKey"]!;
                o.ExpireAt = DateTime.UtcNow.AddHours(4);
                o.User.Permissions.AddRange(Allow.Admin);
                o.User.Claims.Add((Claim.AdminID, adminID));
            });
    }
}