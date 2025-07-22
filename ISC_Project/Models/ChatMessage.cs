using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_Project.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ConversationId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public bool IsFromUser { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("ConversationId")]
        public virtual ChatConversation? Conversation { get; set; }
    }
}
