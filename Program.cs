global using FastEndpoints;
global using FastEndpoints.Security;
global using FluentValidation;
global using MiniDevTo.Auth;
global using MongoDB.Entities;
using FastEndpoints.Swagger;
using MiniDevTo.Migrations;

var bld = WebApplication.CreateBuilder();
bld.Services
   .AddFastEndpoints()
   .AddJWTBearerAuth(bld.Configuration["JwtSigningKey"])
   .SwaggerDocument();

var app = bld.Build();
app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = null)
   .UseSwaggerGen();

await DB.InitAsync(database: "MiniDevTo", host: "localhost");
_001_seed_initial_admin_account.SuperAdminPassword = app.Configuration["SuperAdminPassword"];
await DB.MigrateAsync();

app.Run();