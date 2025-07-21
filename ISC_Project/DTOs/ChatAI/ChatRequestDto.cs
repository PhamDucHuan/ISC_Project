namespace ISC_Project.DTOs.ChatAI
{
    public class ChatRequestDto
    {
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? ConversationId { get; set; }
    }
}

