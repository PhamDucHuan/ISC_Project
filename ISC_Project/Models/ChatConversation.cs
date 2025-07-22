using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_Project.Models
{
    public class ChatConversation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ConversationId { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ConversationTitle { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastMessageTime { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
