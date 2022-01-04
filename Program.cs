global using FastEndpoints;
global using FastEndpoints.Validation;
global using MongoDB.Entities;

using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder();
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null); //pascal case for serialization
builder.Services.AddFastEndpoints();
builder.Services.AddSwagger();

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwagger();
app.UseSwaggerUI();

await DB.InitAsync(
    database: "MiniDevTo",
    host: "localhost");

app.Run();