using ISC_Project.DTOs.ChatAI;
using ISC_Project.Interface;
using System.Text;
using System.Text.Json;

namespace ISC_Project.Services
{
    public class ChatAIService : IChatAIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChatAIService> _logger;
        private readonly string _aiServiceUrl;

        public ChatAIService(HttpClient httpClient, ILogger<ChatAIService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _aiServiceUrl = configuration["ChatAI:ServiceUrl"] ?? "http://localhost:3000";
        }

        public async Task<ChatResponseDto> GetAIResponseAsync(ChatRequestDto request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new
                {
                    message = request.Message,
                    conversationId = request.ConversationId
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{_aiServiceUrl}/api/chat", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var aiResponse = JsonSerializer.Deserialize<dynamic>(responseContent);
                    
                    return new ChatResponseDto
                    {
                        Message = aiResponse?.GetProperty("message").GetString() ?? "Không có phản hồi từ AI",
                        ConversationId = request.ConversationId ?? Guid.NewGuid().ToString(),
                        Timestamp = DateTime.UtcNow,
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ChatResponseDto
                    {
                        Message = "",
                        ConversationId = request.ConversationId ?? "",
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
            // Tạm thời trả về danh sách rỗng, có thể implement database sau
            return new List<ChatHistoryDto>();
        }

        public async Task SaveChatMessageAsync(ChatMessageDto message)
        {
            // Tạm thời không implement, có thể thêm database sau
            await Task.CompletedTask;
        }
    }
}
