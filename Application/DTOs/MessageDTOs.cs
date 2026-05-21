namespace RealtimeChatAPI.Application.DTOs;

public class MessageResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string SentByUsername { get; set; } = string.Empty;
    public int SentByUserId { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}

public class SendMessageRequest
{
    public string Content { get; set;} = string.Empty;
}

public class PagedMessagesResponse
{
    public List<MessageResponse> Messages { get; set; } = new();
    public int Page { get; set;}
    public int PageSize { get; set;}
    public bool HasMore { get; set; }
}