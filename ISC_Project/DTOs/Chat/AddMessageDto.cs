using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Chat
{
    public class AddMessageDto
    {
        [Required]
        public int ConversationId { get; set; }

        [Required]
        public string MessageText { get; set; } = string.Empty;

        [Required]
        public bool IsFromUser { get; set; }
    }
}
