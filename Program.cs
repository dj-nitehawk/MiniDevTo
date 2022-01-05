global using FastEndpoints;
global using FastEndpoints.Security;
global using FastEndpoints.Validation;
global using MiniDevTo.Auth;
global using MongoDB.Entities;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Http.Json;
using MiniDevTo.Migrations;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder();
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null); //pascal case for serialization
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJWTBearer(builder.Configuration["JwtSigningKey"]);
builder.Services.AddSwagger();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.DocExpansion(DocExpansion.None);
    o.DefaultModelExpandDepth(0);
});

await DB.InitAsync(database: "MiniDevTo", host: "localhost");
_001_seed_initial_admin_account.SuperAdminPassword = app.Configuration["SuperAdminPassword"];
await DB.MigrateAsync();

app.Run();