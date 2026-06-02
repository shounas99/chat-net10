using CrisChat.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CrisChat.Api.Endpoints;
using CrisChat.Api.Models;

public static class MessageEndpoints
{
    public static RouteGroupBuilder MapMessageEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/rooms/{roomId:int}/messages");

        group.MapGet("/", (int roomId, ChatStore store) =>
            TypedResults.Ok(store.GetMessagesByRoom(roomId))
        );
        group.MapPost("/", (int roomId, Message message, ChatStore store) =>
        {
            message.RoomId = roomId;
            store.CreateMessage(message);
            return TypedResults.Created($"/api/rooms/{roomId}/messages/{message.Id}", message);
        });
        return group;
    }
}