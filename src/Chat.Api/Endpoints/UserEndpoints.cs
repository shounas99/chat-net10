using CrisChat.Api.Models;
using CrisChat.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CrisChat.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users");

        group.MapGet("/", (ChatStore store ) => TypedResults.Ok( store.GetAllUsers()));

        group.MapPost("/", Results<Ok<User>, Created> (User user, ChatStore store) =>
        {
            var existing = store.GetUserByUsername(user.Username);
            if (existing is not null)
                return TypedResults.Ok(existing);

            store.CreateUser(user);
            return TypedResults.Created($"/api/users/{user.Id}");
        });

        return group;
    }
}