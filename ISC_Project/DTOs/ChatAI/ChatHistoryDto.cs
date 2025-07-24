namespace ISC_Project.DTOs.ChatAI
{
    public class ChatHistoryDto
    {
        public string ConversationId { get; set; } = string.Empty;
        public List<ChatMessageDto> Messages { get; set; } = new List<ChatMessageDto>();
        public DateTime LastMessageTime { get; set; }
        public string ConversationTitle { get; set; } = string.Empty;
    }
}

