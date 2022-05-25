global using FastEndpoints;
global using FastEndpoints.Security;
global using FluentValidation;
global using MiniDevTo.Auth;
global using MongoDB.Entities;
using FastEndpoints.Swagger;
using MiniDevTo.Migrations;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(builder.Configuration["JwtSigningKey"]);
builder.Services.AddSwaggerDoc();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(s => s.SerializerOptions = o => o.PropertyNamingPolicy = null);
app.UseOpenApi();
app.UseSwaggerUi3(c => c.ConfigureDefaults());

await DB.InitAsync(database: "MiniDevTo", host: "localhost");
_001_seed_initial_admin_account.SuperAdminPassword = app.Configuration["SuperAdminPassword"];
await DB.MigrateAsync();

app.Run();