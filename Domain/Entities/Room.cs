namespace RealtimeChatAPI.Domain.Entities;

public class Room
{
  public int Id { get; set; }
  public string Name { get; set;} = string.Empty;
  public DateTime CreatedAt { get; set;} = DateTime.UtcNow;

  public int CreatedByUserId { get; set;}
  public User CreatedBy { get; set; } = null!;

  public ICollection<RoomMember> RoomMembers { get; set;} = new List<RoomMember>();
  public ICollection<Message> Messages { get; set;} = new List<Message>();
}