using ISC_Project.DTOs.ChatAI;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISC_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatAIController : ControllerBase
    {
        private readonly IChatAIService _chatAiService;
        private readonly ILogger<ChatAIController> _logger;

        public ChatAIController(IChatAIService chatAiService, ILogger<ChatAIController> logger)
        {
            _chatAiService = chatAiService;
            _logger = logger;
        }

        [HttpPost("send-message")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequestDto request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Không thể xác định người dùng" });
                }

                request.UserId = userId;
                
                var response = await _chatAiService.GetAIResponseAsync(request);
                
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { message = response.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chat request");
                return StatusCode(500, new { message = "There are an error when sent chat" });
            }
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetChatHistory()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Không thể xác định người dùng" });
                }

                var history = await _chatAiService.GetChatHistoryAsync(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy lịch sử chat");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy lịch sử chat" });
            }
        }

        [HttpPost("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var testRequest = new ChatRequestDto
                {
                    Message = "Test connection",
                    UserId = 1
                };

                var response = await _chatAiService.GetAIResponseAsync(testRequest);
                
                return Ok(new 
                { 
                    connected = response.IsSuccess,
                    message = response.IsSuccess ? "Kết nối thành công" : response.ErrorMessage
                });
            }
            catch (Exception ex)
            {
                return Ok(new 
                { 
                    connected = false, 
                    message = $"Không thể kết nối: {ex.Message}" 
                });
            }
        }

        [HttpGet("conversations")]
        [Authorize]
        public async Task<IActionResult> GetConversations()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Không thể xác định người dùng" });
                }

                var history = await _chatAiService.GetChatHistoryAsync(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách cuộc trò chuyện");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách cuộc trò chuyện" });
            }
        }

        [HttpDelete("conversation/{conversationId}")]
        [Authorize]
        public async Task<IActionResult> DeleteConversation(string conversationId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Không thể xác định người dùng" });
                }

                await _chatAiService.DeleteConversationAsync(userId, conversationId);
                return Ok(new { message = "Đã xóa cuộc trò chuyện thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa cuộc trò chuyện");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xóa cuộc trò chuyện" });
            }
        }
    }
}
