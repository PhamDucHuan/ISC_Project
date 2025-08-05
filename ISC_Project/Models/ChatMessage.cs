using System;
using System.Collections.Generic;

namespace ISC_Project.Models
{
    public partial class ChatMessage
    {
        public int ChatMessageId { get; set; }
        public string? Message { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? ChatConversationId { get; set; }
        public int? SenderId { get; set; }

        public virtual ChatConversation? ChatConversation { get; set; }
        public virtual User? Sender { get; set; }
    }
}
