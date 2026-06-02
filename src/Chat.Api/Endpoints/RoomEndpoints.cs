using CrisChat.Api.DTOs;
using CrisChat.Api.Mappers;
using CrisChat.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CrisChat.Api.Endpoints;

public static class RoomEndpoints
{
    public static RouteGroupBuilder MapRoomEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/rooms").WithTags("Rooms");

        group.MapGet("/", (ChatStore store) =>
            TypedResults.Ok(store.GetAllRooms().Select(r => r.ToDto())))
        .WithName("GetAllRooms")
        .WithSummary("Obtiene todas las salas");

        group.MapPost("/", (CreateRoomDto dto, ChatStore store) =>
        {
            var room = dto.ToEntity();
            store.CreateRoom(room);
            return TypedResults.Created($"/api/rooms/{room.Id}", room.ToDto());
        })
        .AddEndpointFilter(async (context, next) =>
        {
            var dto = context.GetArgument<CreateRoomDto>(0);
            var store = context.HttpContext.RequestServices.GetRequiredService<ChatStore>();

            if (store.GetAllRooms().Any(r => r.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
                return TypedResults.Conflict(new { error = $"Ya existe una sala con el nombre '{dto.Name}'" });

            return await next(context);
        })
        .WithName("CreateRoom")
        .WithSummary("Crea una nueva sala");

        group.MapPut("/{id:int}", Results<Ok<RoomDto>, NotFound> (int id, UpdateRoomDto dto, ChatStore store) =>
        {
            var updated = store.UpdateRoom(id, dto.Name, dto.Description);
            if (updated is null)
                return TypedResults.NotFound();

            return TypedResults.Ok(updated.ToDto());
        })
        .WithName("UpdateRoom")
        .WithSummary("Actualiza una sala existente");

        group.MapDelete("/{id:int}", Results<NoContent, NotFound> (int id, ChatStore store) =>
        {
            var deleted = store.DeleteRoom(id);
            if (!deleted)
                return TypedResults.NotFound();

            return TypedResults.NoContent();
        })
        .WithName("DeleteRoom")
        .WithSummary("Elimina una sala");

        return group;
    }
}