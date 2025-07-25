using ISC_Project.DTOs.PrivateChat;

namespace ISC_Project.Interface
{
    public interface IPrivateChatService
    {
        Task<PrivateChatDto> SendMessageAsync(int senderId, SendMessageDto messageDto);
        Task<List<PrivateChatDto>> GetConversationHistoryAsync(int userId, int otherUserId, int page = 1, int pageSize = 50);
        Task<List<ConversationDto>> GetUserConversationsAsync(int userId);
        Task<bool> MarkMessageAsReadAsync(int messageId, int userId);
        Task<bool> MarkConversationAsReadAsync(int userId, int otherUserId);
        Task<List<UserOnlineStatusDto>> GetOnlineUsersAsync();
        Task<UserOnlineStatusDto?> GetUserOnlineStatusAsync(int userId);
        Task UpdateUserOnlineStatusAsync(int userId, string connectionId, bool isOnline);
        Task<int> GetUnreadMessageCountAsync(int userId);
        Task<List<PrivateChatDto>> GetUnreadMessagesAsync(int userId);
    }
}
