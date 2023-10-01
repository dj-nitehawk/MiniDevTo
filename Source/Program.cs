using FastEndpoints.Swagger;
using MiniDevTo.Migrations;

var bld = WebApplication.CreateBuilder();
bld.Services
   .AddAuthorization()
   .AddFastEndpoints()
   .AddJWTBearerAuth(bld.Configuration["JwtSigningKey"])
   .SwaggerDocument();

var app = bld.Build();
app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = null)
   .UseSwaggerGen();

await DB.InitAsync(app.Configuration["DbName"], "localhost");
_001_seed_initial_admin_account.SuperAdminPassword = app.Configuration["SuperAdminPassword"];
await DB.MigrateAsync();

app.Run();

public partial class Program { }