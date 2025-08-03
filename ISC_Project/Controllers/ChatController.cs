using ISC_Project.DTOs.Chat;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        // Inject IChatService vào controller
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>
        /// Endpoint để tạo một cuộc trò chuyện mới.
        /// </summary>
        [HttpPost("conversations")]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var conversation = await _chatService.CreateConversationAsync(dto.UserId, dto.Title);
                // Trả về đối tượng vừa tạo cùng với status code 201 Created
                return CreatedAtAction(nameof(GetConversationById), new { conversationId = conversation.ChatConversationId }, conversation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi (log the error) nếu cần
                return StatusCode(500, "Lỗi hệ thống khi tạo cuộc trò chuyện.");
            }
        }

        /// <summary>
        /// Endpoint để gửi một tin nhắn mới.
        /// </summary>
        [HttpPost("messages")]
        public async Task<IActionResult> AddMessage([FromBody] AddMessageDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var message = await _chatService.AddMessageAsync(dto.ConversationId, dto.MessageText, dto.IsFromUser);
                return Ok(message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Trả về 404 nếu không tìm thấy conversation
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi hệ thống khi gửi tin nhắn.");
            }
        }

        /// <summary>
        /// Endpoint để lấy tất cả cuộc trò chuyện của một người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        [HttpGet("conversations/user/{userId}")]
        public async Task<IActionResult> GetConversations(int userId)
        {
            var conversations = await _chatService.GetConversationsByUserIdAsync(userId);
            return Ok(conversations);
        }

        /// <summary>
        /// Endpoint để lấy lịch sử tin nhắn của một cuộc trò chuyện.
        /// </summary>
        /// <param name="conversationId">ID của cuộc trò chuyện</param>
        [HttpGet("messages/{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var messages = await _chatService.GetMessagesByConversationIdAsync(conversationId);
            if (messages == null || !messages.Any())
            {
                // Có thể trả về Ok với mảng rỗng thay vì NotFound, tùy vào yêu cầu
                // return Ok(new List<ChatMessage>());
            }
            return Ok(messages);
        }

        // Endpoint này chỉ dùng để hỗ trợ CreatedAtAction ở trên, không bắt buộc phải có logic phức tạp
        // Nó giúp client biết được URL để truy cập tài nguyên vừa tạo.
        [HttpGet("conversations/{conversationId}", Name = "GetConversationById")]
        [ApiExplorerSettings(IgnoreApi = true)] // Ẩn khỏi giao diện Swagger
        public async Task<IActionResult> GetConversationById(int conversationId)
        {
            // Logic để lấy conversation theo ID có thể được thêm vào service nếu cần
            // Ở đây chỉ trả về Ok để ví dụ
            return Ok();
        }
    }
}
