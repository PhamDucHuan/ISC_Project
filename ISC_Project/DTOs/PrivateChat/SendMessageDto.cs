using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.PrivateChat
{
    public class SendMessageDto
    {
        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;
    }
}
