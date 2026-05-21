using RealtimeChatAPI.Domain.Entities;

namespace RealtimeChatAPI.Domain.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(int id);
    Task<List<Room>> GetAllAsync();
    Task<List<RoomMember>> GetMembersAsync(int roomId);
    Task<bool> IsUserMemberAsync(int roomId, int userId);
    Task<Room> CreateAsync(Room room);
    Task AddMemberAsync(RoomMember member);
    Task RemoveMemberAsync(int roomId, int userId);
}