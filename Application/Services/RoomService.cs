using RealtimeChatAPI.Application.DTOs;
using RealtimeChatAPI.Domain.Entities;
using RealtimeChatAPI.Domain.Interfaces;
using RealtimeChatAPI.Application.Exceptions;

namespace RealtimeChatAPI.Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUserRepository _userRepository;

    public RoomService(IRoomRepository roomRepository, IUserRepository userRepository)
    {
        _roomRepository = roomRepository;
        _userRepository = userRepository;
    }

    public async Task<List<RoomResponse>> GetAllRoomsAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();

        return rooms.Select(r=> new RoomResponse
        {
          Id = r.Id,
          Name = r.Name,
          CreatedBy = r.CreatedBy.Username,
          CreatedAt = r.CreatedAt,
          MemberCount = r.RoomMembers.Count
        }).ToList();
    }

    public async Task<RoomResponse> CreateRoomAsync(CreateRoomRequest request, int userId)
    {
      var user = await _userRepository.GetByIdAsync(userId);
      if (user == null)
          throw new Exception("Kullanici Bulunamadi.");  

      var room = new Room
      {
        Name = request.Name,
        CreatedByUserId = userId,
      };    

      var createdRoom = await _roomRepository.CreateAsync(room);

      await _roomRepository.AddMemberAsync(new RoomMember
      {
        RoomId = createdRoom.Id,
        UserId = userId
      });

      return new RoomResponse
      {
        Id = createdRoom.Id,
        Name = createdRoom.Name,
        CreatedBy = user.Username,
        CreatedAt = createdRoom.CreatedAt,
        MemberCount = 1
      };
    }

    public async Task<List<RoomMemberResponse>> GetMembersAsync(int roomId)
  {
        var members = await _roomRepository.GetMembersAsync(roomId);

        return members.Select(m => new RoomMemberResponse
        {
          UserId = m.UserId,
          UserName = m.User.Username,
          JoinedAt = m.JoinedAt
        }).ToList();
  }

  public async Task JoinRoomAsync(int roomId, int userId)
  {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null)
            throw new NotFoundException("Oda Bulunamadi.");

        var alreadyMember = await _roomRepository.IsUserMemberAsync(roomId, userId);
        if (alreadyMember)
            throw new BadRequestException("Zaten Odaya Uyesiniz.");

        await _roomRepository.AddMemberAsync(new RoomMember
        {
           RoomId = roomId,
           UserId = userId
        });
  }

  public async Task LeaveRoomAsync(int roomId, int userId)
  {
        var isMember = await _roomRepository.IsUserMemberAsync(roomId, userId);
        if (!isMember)
            throw new ForbiddenException("Odaya Uye Degilsiniz.");

        await _roomRepository.RemoveMemberAsync(roomId, userId);  
  
  }

  public async Task<bool> IsUserMemberAsync(int roomId, int userId)
  {
         return await _roomRepository.IsUserMemberAsync(roomId, userId);
  }

}
