namespace ISC_Project.DTOs.PrivateChat
{
    public class UserOnlineStatusDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}

