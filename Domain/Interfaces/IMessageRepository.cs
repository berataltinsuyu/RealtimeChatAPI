using RealtimeChatAPI.Domain.Entities;

namespace RealtimeChatAPI.Domain.Interfaces;

public interface IMessageRepository
{
    Task<List<Message>> GetRoomMessagesAsync(int roomId, int page, int pageSize);
    Task<Message> CreateAsync(Message message);
    Task MarkRoomMessagesAsReadAsync(int roomId, int userId);
}