using CrisChat.Api.Endpoints;
using CrisChat.Api.Services;
using CrisChat.Api.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ChatStore>();
builder.Services.AddSingleton<RequestLoggingMiddleware>();
builder.Services.AddValidation();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => "Hello World!");

app.MapRoomEndpoints();
app.MapUserEndpoints();
app.MapMessageEndpoints();

app.Run();