using CrisChat.Api.Models;

namespace CrisChat.Api.DTOs;

public record MessageDto(int Id, string Content, DateTime SentAt, int UserId, string Username, int RoomId);

public record CreateMessageDto(string Content, int UserId, string Username, int RoomId);