using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtimeChatAPI.Application.DTOs;
using RealtimeChatAPI.Application.Services;

namespace RealtimeChatAPI.API.Controllers;

[ApiController]
[Route("api/rooms/{roomId:int}/messages")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedMessagesResponse>> GetRoomMessages(
        int roomId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var response = await _messageService.GetRoomMessagesAsync(roomId, page, pageSize);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> CreateMessage(
        int roomId,
        SendMessageRequest request)
    {
        var userId = GetCurrentUserId();

        var response = await _messageService.CreateMessageAsync(roomId, userId, request.Content);

        return Ok(response);
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            throw new Exception("User id claim not found.");
        }

        return int.Parse(userIdClaim);
    }
}