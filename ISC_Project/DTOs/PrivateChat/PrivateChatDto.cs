namespace ISC_Project.DTOs.PrivateChat
{
    public class PrivateChatDto
    {
        public int Id { get; set; }
        public string ConversationId { get; set; } = string.Empty;
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}

