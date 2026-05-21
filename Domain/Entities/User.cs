namespace RealtimeChatAPI.Domain.Entities;

public class User
{
      public int Id { get; set;}
      public string Username { get ; set;} = string.Empty;
      public string PasswordHash { get; set;} = string.Empty;
      public DateTime CreatedAt { get; set;} = DateTime.UtcNow;

      public ICollection<RoomMember> RoomMembers { get; set;} = new List<RoomMember>();
      public ICollection<Message> Messages { get; set;} = new List<Message>();
}
