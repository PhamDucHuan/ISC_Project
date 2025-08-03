using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class ChatService : IChatService
    {
        private readonly ISC_ProjectDbContext _context;

        // Inject DbContext vào service thông qua constructor
        public ChatService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<ChatConversation> CreateConversationAsync(int userId, string title)
        {
            // Kiểm tra xem user có tồn tại không (nếu cần)
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                throw new ArgumentException("User không tồn tại.");
            }

            var newConversation = new ChatConversation
            {
                UserId = userId,
                ConversationTitle = title,
                ConversationId = Guid.NewGuid().ToString(), // Tạo ID dạng chuỗi duy nhất
                CreatedAt = DateTime.UtcNow,
                LastMessageTime = DateTime.UtcNow
            };

            _context.ChatConversations.Add(newConversation);
            await _context.SaveChangesAsync();

            return newConversation;
        }

        public async Task<ChatMessage> AddMessageAsync(int conversationId, string messageText, bool isFromUser)
        {
            var conversation = await _context.ChatConversations.FindAsync(conversationId);
            if (conversation == null)
            {
                throw new ArgumentException("Cuộc trò chuyện không tồn tại.");
            }

            var newMessage = new ChatMessage
            {
                ChatConversationId = conversationId,
                Message = messageText,
                IsFromUser = isFromUser,
                Timestamp = DateTime.UtcNow
            };

            // Cập nhật thời gian tin nhắn cuối cùng của cuộc trò chuyện
            conversation.LastMessageTime = newMessage.Timestamp;

            _context.ChatMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            return newMessage;
        }

        public async Task<IEnumerable<ChatConversation>> GetConversationsByUserIdAsync(int userId)
        {
            return await _context.ChatConversations
                                 .Where(c => c.UserId == userId)
                                 .OrderByDescending(c => c.LastMessageTime) // Hiển thị cuộc trò chuyện mới nhất lên đầu
                                 .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByConversationIdAsync(int conversationId)
        {
            return await _context.ChatMessages
                                 .Where(m => m.ChatConversationId == conversationId)
                                 .OrderBy(m => m.Timestamp) // Sắp xếp tin nhắn theo thứ tự thời gian
                                 .ToListAsync();
        }
    }
}
