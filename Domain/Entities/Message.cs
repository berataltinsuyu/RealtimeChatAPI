namespace RealtimeChatAPI.Domain.Entities;

public class Message
{
  public int Id { get; set; }
  public string Content { get; set; } = string.Empty;
  public DateTime SentAt { get; set; } = DateTime.UtcNow;
  public bool IsRead { get; set; } = false;

  public int RoomId { get; set;}
  public Room Room { get; set; } = null!;

  public int UserId { get; set;}
  public User User { get; set; } = null!;

}
