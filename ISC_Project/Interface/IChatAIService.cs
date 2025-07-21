using ISC_Project.DTOs.ChatAI;

namespace ISC_Project.Interface
{
    public interface IChatAIService
    {
        Task<ChatResponseDto> GetAIResponseAsync(ChatRequestDto request);
        Task<List<ChatHistoryDto>> GetChatHistoryAsync(int userId);
        Task SaveChatMessageAsync(ChatMessageDto message);
    }
}
