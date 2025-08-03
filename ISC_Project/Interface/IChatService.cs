using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IChatService
    {
        /// <summary>
        /// Lấy danh sách tất cả các cuộc trò chuyện của một người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Danh sách các cuộc trò chuyện.</returns>
        Task<IEnumerable<ChatConversation>> GetConversationsByUserIdAsync(int userId);

        /// <summary>
        /// Lấy tất cả tin nhắn trong một cuộc trò chuyện cụ thể.
        /// </summary>
        /// <param name="conversationId">ID của cuộc trò chuyện.</param>
        /// <returns>Danh sách các tin nhắn đã được sắp xếp.</returns>
        Task<IEnumerable<ChatMessage>> GetMessagesByConversationIdAsync(int conversationId);

        /// <summary>
        /// Tạo một cuộc trò chuyện mới.
        /// </summary>
        /// <param name="userId">ID của người dùng tạo cuộc trò chuyện.</param>
        /// <param name="title">Tiêu đề của cuộc trò chuyện.</param>
        /// <returns>Đối tượng cuộc trò chuyện vừa được tạo.</returns>
        Task<ChatConversation> CreateConversationAsync(int userId, string title);

        /// <summary>
        /// Thêm một tin nhắn mới vào cuộc trò chuyện.
        /// </summary>
        /// <param name="conversationId">ID của cuộc trò chuyện.</param>
        /// <param name="messageText">Nội dung tin nhắn.</param>
        /// <param name="isFromUser">Đánh dấu tin nhắn này từ người dùng hay từ hệ thống/bot.</param>
        /// <returns>Đối tượng tin nhắn vừa được tạo.</returns>
        Task<ChatMessage> AddMessageAsync(int conversationId, string messageText, bool isFromUser);
    }
}
