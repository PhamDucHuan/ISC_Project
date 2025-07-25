using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_Project.Models
{
    public class PrivateChat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ConversationId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation properties
        [ForeignKey("SenderId")]
        public virtual User? Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User? Receiver { get; set; }
    }
}
