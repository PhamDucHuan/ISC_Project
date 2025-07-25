namespace ISC_Project.DTOs.PrivateChat
{
    public class ConversationDto
    {
        public string ConversationId { get; set; } = string.Empty;
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}

