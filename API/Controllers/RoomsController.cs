using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtimeChatAPI.Application.DTOs;
using RealtimeChatAPI.Application.Services;

namespace RealtimeChatAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IOnlineUserService _onlineUserService;

    public RoomsController(IRoomService roomService, IOnlineUserService onlineUserService)
    {
        _roomService = roomService;
        _onlineUserService = onlineUserService;
    }

    [HttpPost]
    public async Task<ActionResult<RoomResponse>> CreateRoom(CreateRoomRequest request)
    {
        var userId = GetCurrentUserId();
        var response = await _roomService.CreateRoomAsync(request, userId);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomResponse>>> GetRooms()
    {
      var response = await _roomService.GetAllRoomsAsync();
      return Ok(response);
    }

    [HttpPost("{id:int}/join")]
    public async Task<IActionResult> JoinRoom(int id)
    {
        var userId = GetCurrentUserId();
        await _roomService.JoinRoomAsync(id, userId);
        return NoContent();
    }

    [HttpPost("{id:int}/leave")]
    public async Task<IActionResult> LeaveRoom(int id)
    {
        var userId = GetCurrentUserId();
        await _roomService.LeaveRoomAsync(id, userId);
        return NoContent();
    }

    [HttpGet("{id:int}/members")]
    public async Task<ActionResult<List<RoomMemberResponse>>> GetMembers(int id)
    {
        var response = await _roomService.GetMembersAsync(id);
        return Ok(response);
    }

    [HttpGet("{id:int}/online")]
    public ActionResult<List<OnlineUserResponse>> GetOnlineUsers(int id)
    {
        var response = _onlineUserService.GetOnlineUsers();

        return Ok(response);
    }

    

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            throw new Exception("Kullanici kimligi bulunamadi.");
        }

        return int.Parse(userIdClaim);
    }
}