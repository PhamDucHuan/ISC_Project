using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_Project.Models
{
    public class UserOnlineStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string ConnectionId { get; set; } = string.Empty;

        public bool IsOnline { get; set; } = true;

        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
