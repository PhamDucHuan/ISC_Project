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
        public string ConversationId { get; set; } = null!;
        public string ConversationTitle { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
