using CrisChat.Api.Services;
using CrisChat.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CrisChat.Api.Endpoints;

public static class RoomEndpoints
{
    public static RouteGroupBuilder MapRoomEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/rooms").WithTags("Rooms");

        group.MapGet("/", (ChatStore store) => TypedResults.Ok(store.GetAllRooms()));

        group.MapPost("/", (Room room, ChatStore store) =>
        {
            store.CreateRoom(room);
            return TypedResults.Created($"/api/rooms/{room.Id}", room);
        });

        group.MapPut("/{id:int}", async Task<Results<Ok<Room>, NotFound>> (int id, Room room, ChatStore store) =>
        {
            var updated = store.UpdateRoom(id, room.Name, room.Description);
            if(updated is null)
                return TypedResults.NotFound();
            return TypedResults.NotFound();
        });

        group.MapDelete("/{id:int}", Results<NoContent, NotFound> (int id, ChatStore store) =>
        {
            if(!store.DeleteRoom(id))
            {
                return TypedResults.NotFound();     
            }
            return TypedResults.NoContent();
        });
        return group;
    }
}