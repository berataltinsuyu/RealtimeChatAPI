using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.Application.Services;

public interface IMessageService
{
    Task<PagedMessagesResponse> GetRoomMessagesAsync(int roomId, int page, int pageSize);
    Task<MessageResponse> CreateMessageAsync(int roomId, int userId, string content);
    Task MarkAsReadAsync(int roomId, int userId); 
}