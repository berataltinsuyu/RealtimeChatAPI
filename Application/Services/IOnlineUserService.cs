using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.Application.Services;

public interface IOnlineUserService
{
    void AddUserConnection(int userId, string username, string connectionId);
    void RemoveUserConnection(int userId, string connectionId);

    bool IsUserOnline(int userId);
    List<int> GetOnlineUserIds();
    List<OnlineUserResponse> GetOnlineUsers();
    List<OnlineUserResponse> GetOnlineUsersByIds(List<int> userIds);
}