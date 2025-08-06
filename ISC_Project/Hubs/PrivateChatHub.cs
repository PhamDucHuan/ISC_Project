using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ISC_Project.Interface;
using ISC_Project.DTOs.PrivateChat;
using System.Security.Claims;
using ISC_Project.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace ISC_Project.Hubs
{
    [Authorize]
    public class PrivateChatHub : Hub
    {
        private readonly IPrivateChatService _privateChatService;
        private readonly ISC_ProjectDbContext _context;
        private readonly ILogger<PrivateChatHub> _logger;
        
        // Sử dụng ConcurrentDictionary để thread-safe
        private static readonly ConcurrentDictionary<int, ConcurrentBag<string>> UserConnections = new();
        private static readonly ConcurrentDictionary<string, int> ConnectionToUser = new();
        private static readonly ConcurrentDictionary<int, DateTime> LastActivity = new();
        
        // Rate limiting
        private static readonly ConcurrentDictionary<int, Queue<DateTime>> MessageRateLimit = new();
        private const int MaxMessagesPerMinute = 30;

        public PrivateChatHub(
            IPrivateChatService privateChatService, 
            ISC_ProjectDbContext context,
            ILogger<PrivateChatHub> logger)
        {
            _privateChatService = privateChatService;
            _context = context;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = GetCurrentUserId();
                var userName = GetCurrentUserName();
                
                if (userId <= 0)
                {
                    _logger.LogWarning("User connection failed - invalid userId");
                    Context.Abort();
                    return;
                }

                // Thêm connection mapping
                ConnectionToUser[Context.ConnectionId] = userId;
                UserConnections.AddOrUpdate(userId, 
                    new ConcurrentBag<string> { Context.ConnectionId },
                    (_, existing) => 
                    {
                        existing.Add(Context.ConnectionId);
                        return existing;
                    });

                // Cập nhật last activity
                LastActivity[userId] = DateTime.UtcNow;

                // Cập nhật trạng thái online trong database
                await _privateChatService.UpdateUserOnlineStatusAsync(userId, Context.ConnectionId, true);

                // Join personal group
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");

                // Thông báo user online (chỉ gửi cho users khác)
                await Clients.AllExcept(Context.ConnectionId)
                    .SendAsync("UserStatusChanged", new { 
                        UserId = userId, 
                        UserName = userName,
                        IsOnline = true,
                        LastSeen = DateTime.UtcNow
                    });

                // Gửi thông tin kết nối thành công
                await Clients.Caller.SendAsync("Connected", new {
                    UserId = userId,
                    Context.ConnectionId,
                    ConnectedAt = DateTime.UtcNow
                });

                // Gửi tin nhắn chưa đọc
                var unreadMessages = await _privateChatService.GetUnreadMessagesAsync(userId);
                if (unreadMessages != null && unreadMessages.Any())
                {
                    await Clients.Caller.SendAsync("UnreadMessages", unreadMessages);
                }

                // Gửi danh sách users online
                var onlineUsers = await GetOnlineUsersInternal();
                await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);

                _logger.LogInformation("User {UserId} connected with ConnectionId {ConnectionId}", userId, Context.ConnectionId);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync for ConnectionId {ConnectionId}", Context.ConnectionId);
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                if (ConnectionToUser.TryRemove(Context.ConnectionId, out var userId))
                {
                    var userName = GetCurrentUserName();
                    
                    // Xóa connection
                    if (UserConnections.TryGetValue(userId, out var connections))
                    {
                        // Tạo bag mới không chứa connection này
                        var newConnections = new ConcurrentBag<string>(
                            connections.Where(c => c != Context.ConnectionId)
                        );
                        
                        if (newConnections.IsEmpty)
                        {
                            UserConnections.TryRemove(userId, out _);
                            LastActivity[userId] = DateTime.UtcNow;
                            
                            // User offline
                            await _privateChatService.UpdateUserOnlineStatusAsync(userId, Context.ConnectionId, false);
                            await Clients.All.SendAsync("UserStatusChanged", new { 
                                UserId = userId,
                                UserName = userName,
                                IsOnline = false,
                                LastSeen = DateTime.UtcNow
                            });
                        }
                        else
                        {
                            UserConnections[userId] = newConnections;
                        }
                    }

                    _logger.LogInformation("User {UserId} disconnected. ConnectionId: {ConnectionId}, Exception: {Exception}", 
                        userId, Context.ConnectionId, exception?.Message);
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnDisconnectedAsync for ConnectionId {ConnectionId}", Context.ConnectionId);
            }
        }

        public async Task SendMessage(int receiverId, string message)
        {
            try
            {
                var senderId = GetCurrentUserId();
                if (senderId <= 0)
                {
                    await Clients.Caller.SendAsync("Error", "Unauthorized");
                    return;
                }

                // Validate input
                if (string.IsNullOrWhiteSpace(message) || message.Length > 1000)
                {
                    await Clients.Caller.SendAsync("Error", "Tin nhắn không hợp lệ hoặc quá dài");
                    return;
                }

                // Rate limiting check
                if (!CheckRateLimit(senderId))
                {
                    await Clients.Caller.SendAsync("Error", "Bạn đang gửi tin nhắn quá nhanh. Vui lòng chờ một chút.");
                    return;
                }

                // Validate receiver exists
                var receiverExists = await _context.Users.AnyAsync(u => u.UserId == receiverId);
                if (!receiverExists)
                {
                    await Clients.Caller.SendAsync("Error", "Người nhận không tồn tại");
                    return;
                }

                var messageDto = new SendMessageDto 
                { 
                    ReceiverId = receiverId, 
                    Message = message.Trim()
                };

                // Lưu tin nhắn
                var savedMessage = await _privateChatService.SendMessageAsync(senderId, messageDto);

                // Gửi đến người nhận
                var receiverConnectionIds = GetUserConnections(receiverId);
                if (receiverConnectionIds.Any())
                {
                    await Clients.Clients(receiverConnectionIds).SendAsync("ReceiveMessage", new {
                        savedMessage.Id,
                        SenderId = senderId,
                        savedMessage.SenderName,
                        Content = savedMessage.Message,
                        savedMessage.SentAt,
                        IsRead = false
                    });

                    // Cập nhật unread count cho receiver
                    var unreadCount = await _privateChatService.GetUnreadMessageCountAsync(receiverId);
                    await Clients.Clients(receiverConnectionIds).SendAsync("UnreadCountUpdated", unreadCount);
                }

                // Xác nhận gửi thành công cho sender
                await Clients.Caller.SendAsync("MessageSent", new {
                    savedMessage.Id,
                    ReceiverId = receiverId,
                    Content = savedMessage.Message,
                    savedMessage.SentAt,
                    Status = "sent"
                });

                _logger.LogInformation("Message sent from {SenderId} to {ReceiverId}", senderId, receiverId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message from user {SenderId} to {ReceiverId}", GetCurrentUserId(), receiverId);
                await Clients.Caller.SendAsync("Error", "Không thể gửi tin nhắn. Vui lòng thử lại.");
            }
        }

        public async Task MarkMessagesAsRead(int senderId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId <= 0) return;

                await _privateChatService.MarkConversationAsReadAsync(userId, senderId);
                
                // Thông báo messages đã được đọc
                var senderConnectionIds = GetUserConnections(senderId);
                if (senderConnectionIds.Any())
                {
                    await Clients.Clients(senderConnectionIds).SendAsync("MessagesMarkedAsRead", new {
                        ReaderId = userId,
                        ConversationWith = senderId
                    });
                }

                // Cập nhật unread count
                var unreadCount = await _privateChatService.GetUnreadMessageCountAsync(userId);
                await Clients.Caller.SendAsync("UnreadCountUpdated", unreadCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking messages as read for user {UserId}", GetCurrentUserId());
                await Clients.Caller.SendAsync("Error", "Không thể đánh dấu tin nhắn đã đọc");
            }
        }

        public async Task JoinConversation(int otherUserId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId <= 0) return;

                var conversationId = GenerateConversationId(userId, otherUserId);
                await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
                
                // Tự động mark messages as read khi join conversation
                await MarkMessagesAsRead(otherUserId);
                
                await Clients.Caller.SendAsync("JoinedConversation", new { 
                    ConversationId = conversationId,
                    OtherUserId = otherUserId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining conversation for user {UserId}", GetCurrentUserId());
            }
        }

        public async Task LeaveConversation(int otherUserId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId <= 0) return;

                var conversationId = GenerateConversationId(userId, otherUserId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
                
                await Clients.Caller.SendAsync("LeftConversation", new { 
                    ConversationId = conversationId,
                    OtherUserId = otherUserId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error leaving conversation for user {UserId}", GetCurrentUserId());
            }
        }

        public async Task StartTyping(int receiverId)
        {
            try
            {
                var senderId = GetCurrentUserId();
                if (senderId <= 0) return;

                var receiverConnectionIds = GetUserConnections(receiverId);
                if (receiverConnectionIds.Any())
                {
                    await Clients.Clients(receiverConnectionIds).SendAsync("UserStartedTyping", new {
                        UserId = senderId,
                        UserName = GetCurrentUserName()
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StartTyping for user {UserId}", GetCurrentUserId());
            }
        }

        public async Task StopTyping(int receiverId)
        {
            try
            {
                var senderId = GetCurrentUserId();
                if (senderId <= 0) return;

                var receiverConnectionIds = GetUserConnections(receiverId);
                if (receiverConnectionIds.Any())
                {
                    await Clients.Clients(receiverConnectionIds).SendAsync("UserStoppedTyping", new {
                        UserId = senderId,
                        UserName = GetCurrentUserName()
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StopTyping for user {UserId}", GetCurrentUserId());
            }
        }

        public async Task GetOnlineUsers()
        {
            try
            {
                var onlineUsers = await GetOnlineUsersInternal();
                await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting online users");
                await Clients.Caller.SendAsync("Error", "Không thể lấy danh sách người dùng online");
            }
        }

        public async Task<IEnumerable<object>> GetAllUsers()
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var users = await _context.Users
                    .AsNoTracking()
                    .Where(u => u.UserId != currentUserId)
                    .Select(u => new { 
                        u.UserId, 
                        u.UserName,
                        IsOnline = UserConnections.ContainsKey(u.UserId),
                        LastActivity = LastActivity.GetValueOrDefault(u.UserId, DateTime.MinValue)
                    })
                    .ToListAsync();
                
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return Enumerable.Empty<object>();
            }
        }

        // Private helper methods
        private int GetCurrentUserId()
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

        private string GetCurrentUserName()
        {
            return Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        }

        private List<string> GetUserConnections(int userId)
        {
            return UserConnections.TryGetValue(userId, out var connections) 
                ? connections.ToList() 
                : new List<string>();
        }

        private string GenerateConversationId(int userId1, int userId2)
        {
            var min = Math.Min(userId1, userId2);
            var max = Math.Max(userId1, userId2);
            return $"conversation_{min}_{max}";
        }

        private bool CheckRateLimit(int userId)
        {
            var now = DateTime.UtcNow;
            var queue = MessageRateLimit.GetOrAdd(userId, _ => new Queue<DateTime>());

            lock (queue)
            {
                // Xóa messages cũ hơn 1 phút
                while (queue.Count > 0 && (now - queue.Peek()).TotalMinutes > 1)
                {
                    queue.Dequeue();
                }

                // Kiểm tra rate limit
                if (queue.Count >= MaxMessagesPerMinute)
                {
                    return false;
                }

                queue.Enqueue(now);
                return true;
            }
        }

        private async Task<IEnumerable<object>> GetOnlineUsersInternal()
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var onlineUserIds = UserConnections.Keys.Where(id => id != currentUserId).ToList();
                
                if (!onlineUserIds.Any())
                    return Enumerable.Empty<object>();

                var users = await _context.Users
                    .AsNoTracking()
                    .Where(u => onlineUserIds.Contains(u.UserId))
                    .Select(u => new {
                        u.UserId,
                        u.UserName,
                        IsOnline = true,
                        LastActivity = LastActivity.GetValueOrDefault(u.UserId, DateTime.UtcNow),
                        ConnectionCount = UserConnections[u.UserId].Count
                    })
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOnlineUsersInternal");
                return Enumerable.Empty<object>();
            }
        }

        // Cleanup method - có thể gọi từ background service
        public static void CleanupInactiveConnections()
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-30);
            var inactiveUsers = LastActivity
                .Where(kvp => kvp.Value < cutoff)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var userId in inactiveUsers)
            {
                UserConnections.TryRemove(userId, out _);
                LastActivity.TryRemove(userId, out _);
                MessageRateLimit.TryRemove(userId, out _);
            }
        }
    }
}
