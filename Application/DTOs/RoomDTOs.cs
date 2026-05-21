namespace RealtimeChatAPI.Application.DTOs;

public class CreateRoomRequest
{
    public string Name { get; set;} = string.Empty;
}

public class RoomResponse
{
    public int Id { get; set;}
    public string Name { get; set;} = string.Empty;
    public string CreatedBy { get; set;} = string.Empty;
    public DateTime CreatedAt { get; set;} 
    public int MemberCount { get; set; }
}

public class RoomMemberResponse
{
    public int UserId { get; set;}
    public string UserName { get; set;} = string.Empty;
    public DateTime JoinedAt { get; set;}
}