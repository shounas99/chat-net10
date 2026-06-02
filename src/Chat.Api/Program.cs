using CrisChat.Api.Endpoints;
using CrisChat.Api.Models;
using CrisChat.Api.Services;
using EDChat.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ChatStore>();
builder.Services.AddSingleton<RequestLoggingMiddleware>();
builder.Services.AddValidation();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapGet("/", () => "Hello World!");

app.MapRoomEndpoints();
app.MapUserEndpoints();
app.MapMessageEndpoints();

app.Run();