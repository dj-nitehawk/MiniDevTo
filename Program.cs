global using FastEndpoints;
global using FastEndpoints.Validation;
using FastEndpoints.Swagger; //add this

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.AddSwagger(); //add this

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwagger(); //add this
app.UseSwaggerUI(); //add this
app.Run();