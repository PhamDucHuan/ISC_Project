namespace ISC_Project.DTOs.ChatAI
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string ConversationId { get; set; } = string.Empty;
        public bool IsFromUser { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

