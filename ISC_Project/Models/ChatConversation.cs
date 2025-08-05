using System;
using System.Collections.Generic;

namespace ISC_Project.Models
{
    public partial class ChatConversation
    {
        public ChatConversation()
        {
            ChatMessages = new HashSet<ChatMessage>();
        }

        public int ChatConversationId { get; set; }
        public string? ConversationId { get; set; }
        public string? ConversationTitle { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public int? Participant1Id { get; set; }
        public int? Participant2Id { get; set; }

        public virtual User? Participant1 { get; set; }
        public virtual User? Participant2 { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
