using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ISC_Project.Interface;
using ISC_Project.DTOs.PrivateChat;
using System.Security.Claims;
// Thêm các using sau
using ISC_Project.Data;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Hubs
{
    [Authorize]
    public class PrivateChatHub : Hub
    {
        private readonly IPrivateChatService _privateChatService;
        private static readonly Dictionary<int, HashSet<string>> _userConnections = new();
        private readonly ISC_ProjectDbContext _context; // <-- THÊM DÒNG NÀY

        // SỬA CONSTRUCTOR
        public PrivateChatHub(IPrivateChatService privateChatService, ISC_ProjectDbContext context)
        {
            _privateChatService = privateChatService;
            _context = context; // <-- THÊM DÒNG NÀY
        }

        // --- Giữ nguyên tất cả các phương thức hiện có của bạn ---
        // (OnConnectedAsync, OnDisconnectedAsync, SendMessage, ...)

        // THÊM PHƯƠNG THỨC MỚI NÀY VÀO CUỐI CLASS
        public async Task<IEnumerable<object>> GetAllUsers()
        {
            var currentUserId = GetCurrentUserId();
            var users = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId != currentUserId) // Loại bỏ người dùng hiện tại
                .Select(u => new { u.UserId, u.UserName }) // Chỉ lấy thông tin cần thiết
                .ToListAsync();
            return users;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetCurrentUserId();
            if (userId > 0)
            {
                // Thêm connection vào dictionary
                lock (_userConnections)
                {
                    if (!_userConnections.ContainsKey(userId))
                        _userConnections[userId] = new HashSet<string>();

                    _userConnections[userId].Add(Context.ConnectionId);
                }

                // Cập nhật trạng thái online
                await _privateChatService.UpdateUserOnlineStatusAsync(userId, Context.ConnectionId, true);

                // Thông báo cho tất cả clients về user online
                await Clients.All.SendAsync("UserStatusChanged", new { UserId = userId, IsOnline = true });

                // Gửi danh sách tin nhắn chưa đọc cho user vừa kết nối
                var unreadMessages = await _privateChatService.GetUnreadMessagesAsync(userId);
                if (unreadMessages.Any())
                {
                    await Clients.Caller.SendAsync("UnreadMessages", unreadMessages);
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetCurrentUserId();
            if (userId > 0)
            {
                // Xóa connection khỏi dictionary
                lock (_userConnections)
                {
                    if (_userConnections.ContainsKey(userId))
                    {
                        _userConnections[userId].Remove(Context.ConnectionId);
                        if (!_userConnections[userId].Any())
                        {
                            _userConnections.Remove(userId);
                        }
                    }
                }

                // Kiểm tra xem user còn connection nào khác không
                bool isStillOnline;
                lock (_userConnections)
                {
                    isStillOnline = _userConnections.ContainsKey(userId) && _userConnections[userId].Any();
                }

                // Cập nhật trạng thái offline nếu không còn connection nào
                if (!isStillOnline)
                {
                    await _privateChatService.UpdateUserOnlineStatusAsync(userId, Context.ConnectionId, false);
                    await Clients.All.SendAsync("UserStatusChanged", new { UserId = userId, IsOnline = false });
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(SendMessageDto messageDto)
        {
            var senderId = GetCurrentUserId();
            if (senderId <= 0) return;

            try
            {
                // Lưu tin nhắn vào database
                var savedMessage = await _privateChatService.SendMessageAsync(senderId, messageDto);

                // Gửi tin nhắn đến người nhận (nếu đang online)
                var receiverConnectionIds = GetUserConnections(messageDto.ReceiverId);
                if (receiverConnectionIds.Any())
                {
                    await Clients.Clients(receiverConnectionIds).SendAsync("ReceiveMessage", savedMessage);
                }

                // Gửi xác nhận về cho người gửi
                await Clients.Caller.SendAsync("MessageSent", savedMessage);

                // Thông báo về tin nhắn mới cho người nhận (để cập nhật badge số lượng)
                if (receiverConnectionIds.Any())
                {
                    var unreadCount = await _privateChatService.GetUnreadMessageCountAsync(messageDto.ReceiverId);
                    await Clients.Clients(receiverConnectionIds).SendAsync("UnreadCountUpdated", unreadCount);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Không thể gửi tin nhắn: {ex.Message}");
            }
        }

        public async Task MarkMessageAsRead(int messageId)
        {
            var userId = GetCurrentUserId();
            if (userId <= 0) return;

            try
            {
                var success = await _privateChatService.MarkMessageAsReadAsync(messageId, userId);
                if (success)
                {
                    await Clients.Caller.SendAsync("MessageMarkedAsRead", messageId);

                    // Cập nhật số lượng tin nhắn chưa đọc
                    var unreadCount = await _privateChatService.GetUnreadMessageCountAsync(userId);
                    await Clients.Caller.SendAsync("UnreadCountUpdated", unreadCount);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Không thể đánh dấu tin nhắn đã đọc: {ex.Message}");
            }
        }

        public async Task MarkConversationAsRead(int otherUserId)
        {
            var userId = GetCurrentUserId();
            if (userId <= 0) return;

            try
            {
                var success = await _privateChatService.MarkConversationAsReadAsync(userId, otherUserId);
                if (success)
                {
                    await Clients.Caller.SendAsync("ConversationMarkedAsRead", otherUserId);

                    // Cập nhật số lượng tin nhắn chưa đọc
                    var unreadCount = await _privateChatService.GetUnreadMessageCountAsync(userId);
                    await Clients.Caller.SendAsync("UnreadCountUpdated", unreadCount);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Không thể đánh dấu cuộc trò chuyện đã đọc: {ex.Message}");
            }
        }

        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Conversation_{conversationId}");
        }

        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Conversation_{conversationId}");
        }

        public async Task GetOnlineUsers()
        {
            try
            {
                var onlineUsers = await _privateChatService.GetOnlineUsersAsync();
                await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Không thể lấy danh sách người dùng online: {ex.Message}");
            }
        }

        public async Task StartTyping(int receiverId)
        {
            var senderId = GetCurrentUserId();
            if (senderId <= 0) return;

            var receiverConnectionIds = GetUserConnections(receiverId);
            if (receiverConnectionIds.Any())
            {
                await Clients.Clients(receiverConnectionIds).SendAsync("UserStartedTyping", senderId);
            }
        }

        public async Task StopTyping(int receiverId)
        {
            var senderId = GetCurrentUserId();
            if (senderId <= 0) return;

            var receiverConnectionIds = GetUserConnections(receiverId);
            if (receiverConnectionIds.Any())
            {
                await Clients.Clients(receiverConnectionIds).SendAsync("UserStoppedTyping", senderId);
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return 0;
        }

        private List<string> GetUserConnections(int userId)
        {
            lock (_userConnections)
            {
                return _userConnections.ContainsKey(userId)
                    ? _userConnections[userId].ToList()
                    : new List<string>();
            }
        }
    }
}