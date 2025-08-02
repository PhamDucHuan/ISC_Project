using ISC_Project.DTOs.ChatAI;
using ISC_Project.Interface;
using ISC_Project.Data;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace ISC_Project.Services
{
    public class ChatAIService : IChatAIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChatAIService> _logger;
        private readonly ISC_ProjectDbContext _context;
        private readonly string _aiServiceUrl;

        public ChatAIService(HttpClient httpClient, ILogger<ChatAIService> logger, 
            ISC_ProjectDbContext context, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _context = context;
            _aiServiceUrl = configuration["ChatAI:ServiceUrl"] ?? "https://dev-chatbot-hono.vercel.app/";
        }

        public async Task<ChatResponseDto> GetAIResponseAsync(ChatRequestDto request)
        {
            try
            {
                // Tạo hoặc lấy conversation
                var conversation = await GetOrCreateConversationAsync(request.UserId, request.ConversationId);
                
                // Lưu tin nhắn của user
                await SaveChatMessageAsync(new ChatMessageDto
                {
                    Message = request.Message,
                    ConversationId = conversation.ConversationId,
                    IsFromUser = true,
                    UserId = request.UserId,
                    Timestamp = DateTime.Now
                });

                var jsonContent = JsonSerializer.Serialize(new
                {
                    message = request.Message,
                    conversationId = conversation.ConversationId
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{_aiServiceUrl}/api/chat", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(responseContent);
                    var aiMessage = jsonDocument.RootElement.GetProperty("message").GetString() ?? "Không có phản hồi từ AI";
                    
                    // Lưu phản hồi của AI
                    await SaveChatMessageAsync(new ChatMessageDto
                    {
                        Message = aiMessage,
                        ConversationId = conversation.ConversationId,
                        IsFromUser = false,
                        UserId = request.UserId,
                        Timestamp = DateTime.Now
                    });

                    // Cập nhật thời gian tin nhắn cuối
                    conversation.LastMessageTime = DateTime.Now;
                    await _context.SaveChangesAsync();
                    
                    return new ChatResponseDto
                    {
                        Message = aiMessage,
                        ConversationId = conversation.ConversationId,
                        Timestamp = DateTime.UtcNow,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ChatResponseDto
                    {
                        Message = "",
                        ConversationId = conversation.ConversationId,
                        Timestamp = DateTime.UtcNow,
                        IsSuccess = false,
                        ErrorMessage = "Không thể kết nối đến dịch vụ AI"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi AI service");
                return new ChatResponseDto
                {
                    Message = "",
                    ConversationId = request.ConversationId ?? "",
                    Timestamp = DateTime.UtcNow,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<List<ChatHistoryDto>> GetChatHistoryAsync(int userId)
        {
            try
            {
                var conversations = await _context.ChatConversations
                    .Where(c => c.UserId == userId)
                    .Include(c => c.ChatMessages)
                    .OrderByDescending(c => c.LastMessageTime)
                    .ToListAsync();

                var chatHistories = conversations.Select(c => new ChatHistoryDto
                {
                    ConversationId = c.ConversationId,
                    ConversationTitle = c.ConversationTitle,
                    LastMessageTime = (DateTime)c.LastMessageTime,
                    Messages = c.ChatMessages.Select(m => new ChatMessageDto
                    {
                        Id = m.ChatMessageId,
                        Message = m.Message,
                        ConversationId = c.ConversationId,
                        IsFromUser = m.IsFromUser,
                        UserId = userId,
                        Timestamp = (DateTime)m.Timestamp
                    }).OrderBy(m => m.Timestamp).ToList()
                }).ToList();

                return chatHistories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy lịch sử chat cho user {UserId}", userId);
                return new List<ChatHistoryDto>();
            }
        }

        public async Task SaveChatMessageAsync(ChatMessageDto messageDto)
        {
            try
            {
                var conversation = await _context.ChatConversations
                    .FirstOrDefaultAsync(c => c.ConversationId == messageDto.ConversationId);

                if (conversation != null)
                {
                    var chatMessage = new ChatMessage
                    {
                        ChatConversationId = conversation.ChatConversationId,
                        Message = messageDto.Message,
                        IsFromUser = messageDto.IsFromUser,
                        Timestamp = messageDto.Timestamp
                    };

                    _context.ChatMessages.Add(chatMessage);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lưu tin nhắn chat");
            }
        }

        private async Task<ChatConversation> GetOrCreateConversationAsync(int userId, string? conversationId)
        {
            ChatConversation? conversation = null;

            if (!string.IsNullOrEmpty(conversationId))
            {
                conversation = await _context.ChatConversations
                    .FirstOrDefaultAsync(c => c.ConversationId == conversationId && c.UserId == userId);
            }

            if (conversation == null)
            {
                conversation = new ChatConversation
                {
                    ConversationId = conversationId ?? Guid.NewGuid().ToString(),
                    UserId = userId,
                    ConversationTitle = "Cuộc trò chuyện mới",
                    CreatedAt = DateTime.Now,
                    LastMessageTime = DateTime.Now
                };

                _context.ChatConversations.Add(conversation);
                await _context.SaveChangesAsync();
            }

            return conversation;
        }

        public async Task DeleteConversationAsync(int userId, string conversationId)
        {
            try
            {
                var conversation = await _context.ChatConversations
                    .Include(c => c.ChatMessages)
                    .FirstOrDefaultAsync(c => c.ConversationId == conversationId && c.UserId == userId);

                if (conversation != null)
                {
                    // Xóa tất cả tin nhắn trong cuộc trò chuyện
                    _context.ChatMessages.RemoveRange(conversation.ChatMessages);
                    
                    // Xóa cuộc trò chuyện
                    _context.ChatConversations.Remove(conversation);
                    
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa cuộc trò chuyện {ConversationId} cho user {UserId}", conversationId, userId);
                throw;
            }
        }
    }
}
