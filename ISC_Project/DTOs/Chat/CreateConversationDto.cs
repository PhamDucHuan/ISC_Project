using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Chat
{
    public class CreateConversationDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }
    }
}
