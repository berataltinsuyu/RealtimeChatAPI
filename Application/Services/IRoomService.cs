using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.Application.Services;

public interface IRoomService
{
    Task<List<RoomResponse>> GetAllRoomsAsync();
    Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request, int userId);
    Task<List<RoomMemberResponse>> GetMembersAsync(int roomId);
    Task JoinRoomAsync(int roomId, int userId);
    Task LeaveRoomAsync(int roomId, int userId);
    Task<bool> IsUserMemberAsync(int roomId, int userId);

}
