using Microsoft.EntityFrameworkCore;
using RealtimeChatAPI.Domain.Entities;
using RealtimeChatAPI.Domain.Interfaces;
using RealtimeChatAPI.Infrastructure.Data;

namespace RealtimeChatAPI.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository (AppDbContext context)
    {
      _context = context;
    }

    public async Task<List<Message>> GetRoomMessagesAsync(int roomId, int page, int pageSize)
    {
        return await _context.Messages
            .Include(m => m.User)
            .Where(m => m.RoomId == roomId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Message> CreateAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task MarkRoomMessagesAsReadAsync (int roomId, int userId)
    {
        var unreadMessages = await _context.Messages
            .Where(m=> m.RoomId == roomId && m.UserId != userId && !m.IsRead)
            .ToListAsync();
          
        foreach (var message in unreadMessages)
        {
          message.IsRead = true;
        }

        await _context.SaveChangesAsync();
    }
}