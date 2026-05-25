using System.Collections.Concurrent;
using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.Application.Services;

public class OnlineUserService : IOnlineUserService
{
    private readonly ConcurrentDictionary<int, OnlineUserConnection> _onlineUsers = new();

    public void AddUserConnection(int userId, string username, string connectionId)
    {
        var userConnection = _onlineUsers.GetOrAdd(userId, _ => new OnlineUserConnection
        {
            UserId = userId,
            Username = username
        });

        lock (userConnection.ConnectionIds)
        {
            userConnection.ConnectionIds.Add(connectionId);
        }
    }

    public void RemoveUserConnection(int userId, string connectionId)
    {
        if (!_onlineUsers.TryGetValue(userId, out var userConnection))
        {
            return;
        }

        lock (userConnection.ConnectionIds)
        {
            userConnection.ConnectionIds.Remove(connectionId);

            if (userConnection.ConnectionIds.Count == 0)
            {
                _onlineUsers.TryRemove(userId, out _);
            }
        }
    }

    public bool IsUserOnline(int userId)
    {
        return _onlineUsers.ContainsKey(userId);
    }

    public List<int> GetOnlineUserIds()
    {
        return _onlineUsers.Keys.ToList();
    }

    public List<OnlineUserResponse> GetOnlineUsers()
    {
        return _onlineUsers.Values
            .Select(user => new OnlineUserResponse
            {
                UserId = user.UserId,
                Username = user.Username
            })
            .ToList();
    }

    private class OnlineUserConnection
    {
        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public HashSet<string> ConnectionIds { get; } = new();
    }
}