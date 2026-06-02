using CrisChat.Api.DTOs;
using CrisChat.Api.Models;
using CrisChat.Api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using CrisChat.Api.Services;

namespace CrisChat.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/", (ChatStore store) =>
            TypedResults.Ok((store.GetAllUsers()).Select(u => u.ToDto())))
            .WithName("GetAllUsers")
            .WithSummary("Obtiene todos los usuarios");

        group.MapPost("/", async Task<Results<Ok<UserDto>, Created<UserDto>>> (CreateUserDto dto, ChatStore store) =>
        {
            var existing = store.GetUserByUsername(dto.Username);
            if (existing is not null)
                return TypedResults.Ok(existing.ToDto());

            var user = dto.ToEntity();
            store.CreateUser(user);
            return TypedResults.Created($"/api/users/{user.Id}", user.ToDto());
        })
            .WithName("CreateUser")
            .WithSummary("Crea un nuevo usuario o retorna el existente");

        return group;
    }
}