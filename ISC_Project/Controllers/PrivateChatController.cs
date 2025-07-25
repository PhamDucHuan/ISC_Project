using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ISC_Project.Interface;
using ISC_Project.DTOs.PrivateChat;
using System.Security.Claims;

namespace ISC_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrivateChatController : ControllerBase
    {
        private readonly IPrivateChatService _privateChatService;

        public PrivateChatController(IPrivateChatService privateChatService)
        {
            _privateChatService = privateChatService;
        }

        /// <summary>
        /// Gửi tin nhắn riêng tư
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto messageDto)
        {
            try
            {
                var senderId = GetCurrentUserId();
                var result = await _privateChatService.SendMessageAsync(senderId, messageDto);
                return Ok(new { success = true, data = result, message = "Tin nhắn đã được gửi thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi gửi tin nhắn: {ex.Message}" });
            }
        }

        /// <summary>
        /// Lấy lịch sử cuộc trò chuyện với một người dùng cụ thể
        /// </summary>
        [HttpGet("conversation/{otherUserId}")]
        public async Task<IActionResult> GetConversationHistory(int otherUserId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            try
            {
                var userId = GetCurrentUserId();
                var messages = await _privateChatService.GetConversationHistoryAsync(userId, otherUserId, page, pageSize);
                return Ok(new { success = true, data = messages, message = "Lấy lịch sử cuộc trò chuyện thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi lấy lịch sử cuộc trò chuyện: {ex.Message}" });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả cuộc trò chuyện của người dùng
        /// </summary>
        [HttpGet("conversations")]
        public async Task<IActionResult> GetUserConversations()
        {
            try
            {
                var userId = GetCurrentUserId();
                var conversations = await _privateChatService.GetUserConversationsAsync(userId);
                return Ok(new { success = true, data = conversations, message = "Lấy danh sách cuộc trò chuyện thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi lấy danh sách cuộc trò chuyện: {ex.Message}" });
            }
        }

        /// <summary>
        /// Đánh dấu tin nhắn đã đọc
        /// </summary>
        [HttpPut("mark-read/{messageId}")]
        public async Task<IActionResult> MarkMessageAsRead(int messageId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _privateChatService.MarkMessageAsReadAsync(messageId, userId);
                
                if (success)
                {
                    return Ok(new { success = true, message = "Đã đánh dấu tin nhắn là đã đọc" });
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy tin nhắn hoặc tin nhắn đã được đọc" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi đánh dấu tin nhắn đã đọc: {ex.Message}" });
            }
        }

        /// <summary>
        /// Đánh dấu tất cả tin nhắn trong cuộc trò chuyện đã đọc
        /// </summary>
        [HttpPut("mark-conversation-read/{otherUserId}")]
        public async Task<IActionResult> MarkConversationAsRead(int otherUserId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _privateChatService.MarkConversationAsReadAsync(userId, otherUserId);
                
                return Ok(new { success = true, message = "Đã đánh dấu cuộc trò chuyện là đã đọc", hasUpdates = success });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi đánh dấu cuộc trò chuyện đã đọc: {ex.Message}" });
            }
        }

        /// <summary>
        /// Lấy danh sách người dùng đang online
        /// </summary>
        [HttpGet("online-users")]
        public async Task<IActionResult> GetOnlineUsers()
        {
            try
            {
                var onlineUsers = await _privateChatService.GetOnlineUsersAsync();
                return Ok(new { success = true, data = onlineUsers, message = "Lấy danh sách người dùng online thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi lấy danh sách người dùng online: {ex.Message}" });
            }
        }

        /// <summary>
        /// Lấy trạng thái online của một người dùng cụ thể
        /// </summary>
        [HttpGet("user-status/{userId}")]
        public async Task<IActionResult> GetUserOnlineStatus(int userId)
        {
            try
            {
                var status = await _privateChatService.GetUserOnlineStatusAsync(userId);
                
                if (status != null)
                {
                    return Ok(new { success = true, data = status, message = "Lấy trạng thái người dùng thành công" });
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy thông tin trạng thái người dùng" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi lấy trạng thái người dùng: {ex.Message}" });
            }
        }

        /// <summary>
        /// Lấy số lượng tin nhắn chưa đọc
        /// </summary>
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadMessageCount()
        {
            try
            {
                var userId = GetCurrentUserId();
                var count = await _privateChatService.GetUnreadMessageCountAsync(userId);
                return Ok(new { success = true, data = count, message = "Lấy số lượng tin nhắn chưa đọc thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi lấy số lượng tin nhắn chưa đọc: {ex.Message}" });
            }
        }

        /// <summary>
        /// Lấy danh sách tin nhắn chưa đọc
        /// </summary>
        [HttpGet("unread-messages")]
        public async Task<IActionResult> GetUnreadMessages()
        {
            try
            {
                var userId = GetCurrentUserId();
                var messages = await _privateChatService.GetUnreadMessagesAsync(userId);
                return Ok(new { success = true, data = messages, message = "Lấy danh sách tin nhắn chưa đọc thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi khi lấy danh sách tin nhắn chưa đọc: {ex.Message}" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("Không thể xác định người dùng hiện tại");
        }
    }
}
