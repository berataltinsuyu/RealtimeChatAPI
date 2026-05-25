using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RealtimeChatAPI.Application.Services;

namespace RealtimeChatAPI.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IRoomService _roomService;
    private readonly IOnlineUserService _onlineUserService;

    public ChatHub(
        IMessageService messageService,
        IRoomService roomService,
        IOnlineUserService onlineUserService)
    {
        _messageService = messageService;
        _roomService = roomService;
        _onlineUserService = onlineUserService;
    }

public async Task JoinRoom(int roomId)
{
    var userId = GetCurrentUserId();

    try
    {
        await _roomService.JoinRoomAsync(roomId, userId);
    }
    catch (Exception exception)
    {
        if (!exception.Message.Contains("Zaten") &&
            !exception.Message.Contains("already", StringComparison.OrdinalIgnoreCase))
        {
            throw;
        }
    }

    await Groups.AddToGroupAsync(Context.ConnectionId, GetRoomGroupName(roomId));

    await Clients.Group(GetRoomGroupName(roomId)).SendAsync("UserJoinedRoom", new
    {
        RoomId = roomId,
        UserId = userId
    });
}

    public async Task LeaveRoom(int roomId)
    {
        var userId = GetCurrentUserId();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetRoomGroupName(roomId));

        await Clients.Group(GetRoomGroupName(roomId)).SendAsync("UserLeftRoom", new
        {
            RoomId = roomId,
            UserId = userId
        });
    }

    public async Task SendMessage(int roomId, string content)
    {
        var userId = GetCurrentUserId();

        var message = await _messageService.CreateMessageAsync(roomId, userId, content);

        await Clients.Group(GetRoomGroupName(roomId)).SendAsync("ReceiveMessage", message);
    }

    public async Task MarkAsRead(int roomId)
    {
        var userId = GetCurrentUserId();

        await _messageService.MarkAsReadAsync(roomId, userId);

        await Clients.Group(GetRoomGroupName(roomId)).SendAsync("MessageRead", new
        {
            RoomId = roomId,
            UserId = userId
        });
    }

public override async Task OnConnectedAsync()
{
    var userId = GetCurrentUserId();
    var username = GetCurrentUsername();

    _onlineUserService.AddUserConnection(userId, username, Context.ConnectionId);

    await Clients.Caller.SendAsync("Connected", new
    {
        UserId = userId,
        Username = username,
        ConnectionId = Context.ConnectionId
    });

    await Clients.Others.SendAsync("UserOnline", new
    {
        UserId = userId,
        Username = username
    });

    await base.OnConnectedAsync();
}

public override async Task OnDisconnectedAsync(Exception? exception)
{
    var userId = GetCurrentUserId();

    _onlineUserService.RemoveUserConnection(userId, Context.ConnectionId);

    if (!_onlineUserService.IsUserOnline(userId))
    {
        await Clients.Others.SendAsync("UserOffline", new
        {
            UserId = userId
        });
    }

    await base.OnDisconnectedAsync(exception);
}

    private int GetCurrentUserId()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            throw new HubException("User id claim not found.");
        }

        return int.Parse(userIdClaim);
    }

    private static string GetRoomGroupName(int roomId)
    {
        return $"room-{roomId}";
    }

    private string GetCurrentUsername()
{
    var usernameClaim = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

    if (string.IsNullOrWhiteSpace(usernameClaim))
    {
        throw new HubException("Username claim not found.");
    }

    return usernameClaim;
}
}