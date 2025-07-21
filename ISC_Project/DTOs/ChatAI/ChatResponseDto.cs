namespace ISC_Project.DTOs.ChatAI
{
    public class ChatResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string ConversationId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

