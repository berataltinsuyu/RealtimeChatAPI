using RealtimeChatAPI.Application.DTOs;
using RealtimeChatAPI.Domain.Entities;
using RealtimeChatAPI.Domain.Interfaces;
using RealtimeChatAPI.Application.Exceptions;

namespace RealtimeChatAPI.Application.Services;

public class MesssageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IRoomRepository _roomRepository;

    public MesssageService(
        IMessageRepository messageRepository,
        IRoomRepository roomRepository)
      {
        _messageRepository = messageRepository;
        _roomRepository = roomRepository;
      }

      public async Task<PagedMessagesResponse> GetRoomMessagesAsync(int roomId, int page, int pageSize)
      {
        var room = await _roomRepository.GetByIdAsync(roomId);

        if (room is null)
        {
            throw new NotFoundException("Oda bulunamadi.");
        }

        if (page <= 0)
        {
            page = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = 20;
        } 

        var messages = await _messageRepository.GetRoomMessagesAsync(roomId, page , pageSize);
        
        var messageResponse = messages.Select(m => new MessageResponse
        {
          Id = m.Id,
          Content = m.Content,
          SentByUsername = m.User?.Username ?? string.Empty,
          SentByUserId = m.UserId,
          SentAt = m.SentAt,
          IsRead = m.IsRead

        }).ToList();

        return new PagedMessagesResponse
        {
          Messages = messageResponse,
          Page = page,
          PageSize = pageSize,
          HasMore = messages.Count == pageSize
        };
      }

      public async Task<MessageResponse> CreateMessageAsync(int roomId, int userId, string content)
      {
         var room = await _roomRepository.GetByIdAsync(roomId);

         if (room is null)
         {
            throw new NotFoundException("Oda bulunamadi.");
         }

         var isMember = await _roomRepository.IsUserMemberAsync(roomId, userId);

         if (!isMember)
        {
          throw new ForbiddenException("Kullanici bu odanin uyesi degil.");
        }
        if (string.IsNullOrWhiteSpace(content))
        {
          throw new BadRequestException("Mesaj icerigi bos olamaz.");
        }

        var message = new Message
        {
            RoomId = roomId,
            UserId = userId,
            Content = content.Trim(),
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        var createdMessage = await _messageRepository.CreateAsync(message);

        return new MessageResponse
        {
          Id = createdMessage.Id,
          Content = createdMessage.Content,
          SentByUserId= createdMessage.UserId,
          SentByUsername = createdMessage.User?.Username ?? string.Empty,
          SentAt = createdMessage.SentAt,
          IsRead = createdMessage.IsRead
        };
      }

      public async Task MarkAsReadAsync(int roomId, int userId)
      {
        var room = await _roomRepository.GetByIdAsync(roomId);

        if (room is null)
        {
            throw new NotFoundException("Oda bulunamadi.");
        }

        var isMember = await _roomRepository.IsUserMemberAsync(roomId, userId);

        if (!isMember)
        {
          throw new ForbiddenException("Kullanici bu odanin uyesi degil.");
        }

        await _messageRepository.MarkRoomMessagesAsReadAsync(roomId, userId);

      }
}
