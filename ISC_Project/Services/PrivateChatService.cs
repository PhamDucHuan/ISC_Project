using ISC_Project.Data;
using ISC_Project.DTOs.PrivateChat;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class PrivateChatService : IPrivateChatService
    {
        private readonly ISC_ProjectDbContext _context;

        public PrivateChatService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<PrivateChatDto> SendMessageAsync(int senderId, SendMessageDto messageDto)
        {
            // Tạo conversationId duy nhất cho cặp người dùng
            var participants = new[] { senderId, messageDto.ReceiverId }.OrderBy(x => x).ToArray();
            var conversationId = $"{participants[0]}_{participants[1]}";

            var privateChat = new PrivateChat
            {
                ConversationId = conversationId,
                SenderId = senderId,
                ReceiverId = messageDto.ReceiverId,
                Message = messageDto.Message,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.PrivateChats.Add(privateChat);
            await _context.SaveChangesAsync();

            return await GetChatDtoAsync(privateChat);
        }

        public async Task<List<PrivateChatDto>> GetConversationHistoryAsync(int userId, int otherUserId, int page = 1, int pageSize = 50)
        {
            var participants = new[] { userId, otherUserId }.OrderBy(x => x).ToArray();
            var conversationId = $"{participants[0]}_{participants[1]}";

            var messages = await _context.PrivateChats
                .Include(pc => pc.Sender)
                .Include(pc => pc.Receiver)
                .Where(pc => pc.ConversationId == conversationId && !pc.IsDeleted)
                .OrderByDescending(pc => pc.SentAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(pc => new PrivateChatDto
                {
                    Id = pc.Id,
                    ConversationId = pc.ConversationId,
                    SenderId = pc.SenderId,
                    SenderName = pc.Sender!.FullName ?? pc.Sender.UserName ?? "Unknown",
                    ReceiverId = pc.ReceiverId,
                    ReceiverName = pc.Receiver!.FullName ?? pc.Receiver.UserName ?? "Unknown",
                    Message = pc.Message,
                    SentAt = pc.SentAt,
                    IsRead = pc.IsRead,
                    ReadAt = pc.ReadAt
                })
                .ToListAsync();

            return messages.OrderBy(m => m.SentAt).ToList();
        }

        public async Task<List<ConversationDto>> GetUserConversationsAsync(int userId)
        {
            var conversations = await _context.PrivateChats
                .Include(pc => pc.Sender)
                .Include(pc => pc.Receiver)
                .Where(pc => (pc.SenderId == userId || pc.ReceiverId == userId) && !pc.IsDeleted)
                .GroupBy(pc => pc.ConversationId)
                .Select(g => new
                {
                    ConversationId = g.Key,
                    LastMessage = g.OrderByDescending(pc => pc.SentAt).First(),
                    UnreadCount = g.Count(pc => pc.ReceiverId == userId && !pc.IsRead)
                })
                .ToListAsync();

            var conversationDtos = new List<ConversationDto>();

            foreach (var conv in conversations)
            {
                var otherUserId = conv.LastMessage.SenderId == userId 
                    ? conv.LastMessage.ReceiverId 
                    : conv.LastMessage.SenderId;

                var otherUser = await _context.Users.FindAsync(otherUserId);
                var onlineStatus = await _context.UserOnlineStatuses
                    .Where(uos => uos.UserId == otherUserId)
                    .OrderByDescending(uos => uos.ConnectedAt)
                    .FirstOrDefaultAsync();

                conversationDtos.Add(new ConversationDto
                {
                    ConversationId = conv.ConversationId,
                    ParticipantId = otherUserId,
                    ParticipantName = otherUser?.FullName ?? otherUser?.UserName ?? "Unknown",
                    LastMessage = conv.LastMessage.Message,
                    LastMessageTime = conv.LastMessage.SentAt,
                    UnreadCount = conv.UnreadCount,
                    IsOnline = onlineStatus?.IsOnline ?? false,
                    LastSeen = onlineStatus?.LastSeen
                });
            }

            return conversationDtos.OrderByDescending(c => c.LastMessageTime).ToList();
        }

        public async Task<bool> MarkMessageAsReadAsync(int messageId, int userId)
        {
            var message = await _context.PrivateChats
                .FirstOrDefaultAsync(pc => pc.Id == messageId && pc.ReceiverId == userId);

            if (message == null || message.IsRead) return false;

            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkConversationAsReadAsync(int userId, int otherUserId)
        {
            var participants = new[] { userId, otherUserId }.OrderBy(x => x).ToArray();
            var conversationId = $"{participants[0]}_{participants[1]}";

            var unreadMessages = await _context.PrivateChats
                .Where(pc => pc.ConversationId == conversationId && 
                           pc.ReceiverId == userId && 
                           !pc.IsRead)
                .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return unreadMessages.Any();
        }

        public async Task<List<UserOnlineStatusDto>> GetOnlineUsersAsync()
        {
            var onlineUsers = await _context.UserOnlineStatuses
                .Include(uos => uos.User)
                .Where(uos => uos.IsOnline)
                .GroupBy(uos => uos.UserId)
                .Select(g => g.OrderByDescending(uos => uos.ConnectedAt).First())
                .Select(uos => new UserOnlineStatusDto
                {
                    UserId = uos.UserId,
                    UserName = uos.User!.FullName ?? uos.User.UserName ?? "Unknown",
                    IsOnline = uos.IsOnline,
                    LastSeen = uos.LastSeen
                })
                .ToListAsync();

            return onlineUsers;
        }

        public async Task<UserOnlineStatusDto?> GetUserOnlineStatusAsync(int userId)
        {
            var status = await _context.UserOnlineStatuses
                .Include(uos => uos.User)
                .Where(uos => uos.UserId == userId)
                .OrderByDescending(uos => uos.ConnectedAt)
                .FirstOrDefaultAsync();

            if (status == null) return null;

            return new UserOnlineStatusDto
            {
                UserId = status.UserId,
                UserName = status.User!.FullName ?? status.User.UserName ?? "Unknown",
                IsOnline = status.IsOnline,
                LastSeen = status.LastSeen
            };
        }

        public async Task UpdateUserOnlineStatusAsync(int userId, string connectionId, bool isOnline)
        {
            var existingStatus = await _context.UserOnlineStatuses
                .FirstOrDefaultAsync(uos => uos.UserId == userId && uos.ConnectionId == connectionId);

            if (existingStatus != null)
            {
                existingStatus.IsOnline = isOnline;
                existingStatus.LastSeen = DateTime.UtcNow;
            }
            else
            {
                var newStatus = new UserOnlineStatus
                {
                    UserId = userId,
                    ConnectionId = connectionId,
                    IsOnline = isOnline,
                    LastSeen = DateTime.UtcNow,
                    ConnectedAt = DateTime.UtcNow
                };
                _context.UserOnlineStatuses.Add(newStatus);
            }

            // Đánh dấu tất cả connection khác của user này là offline
            if (!isOnline)
            {
                var otherConnections = await _context.UserOnlineStatuses
                    .Where(uos => uos.UserId == userId && uos.ConnectionId != connectionId)
                    .ToListAsync();

                foreach (var conn in otherConnections)
                {
                    conn.IsOnline = false;
                    conn.LastSeen = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadMessageCountAsync(int userId)
        {
            return await _context.PrivateChats
                .CountAsync(pc => pc.ReceiverId == userId && !pc.IsRead && !pc.IsDeleted);
        }

        public async Task<List<PrivateChatDto>> GetUnreadMessagesAsync(int userId)
        {
            var unreadMessages = await _context.PrivateChats
                .Include(pc => pc.Sender)
                .Include(pc => pc.Receiver)
                .Where(pc => pc.ReceiverId == userId && !pc.IsRead && !pc.IsDeleted)
                .OrderBy(pc => pc.SentAt)
                .Select(pc => new PrivateChatDto
                {
                    Id = pc.Id,
                    ConversationId = pc.ConversationId,
                    SenderId = pc.SenderId,
                    SenderName = pc.Sender!.FullName ?? pc.Sender.UserName ?? "Unknown",
                    ReceiverId = pc.ReceiverId,
                    ReceiverName = pc.Receiver!.FullName ?? pc.Receiver.UserName ?? "Unknown",
                    Message = pc.Message,
                    SentAt = pc.SentAt,
                    IsRead = pc.IsRead,
                    ReadAt = pc.ReadAt
                })
                .ToListAsync();

            return unreadMessages;
        }

        private async Task<PrivateChatDto> GetChatDtoAsync(PrivateChat privateChat)
        {
            var sender = await _context.Users.FindAsync(privateChat.SenderId);
            var receiver = await _context.Users.FindAsync(privateChat.ReceiverId);

            return new PrivateChatDto
            {
                Id = privateChat.Id,
                ConversationId = privateChat.ConversationId,
                SenderId = privateChat.SenderId,
                SenderName = sender?.FullName ?? sender?.UserName ?? "Unknown",
                ReceiverId = privateChat.ReceiverId,
                ReceiverName = receiver?.FullName ?? receiver?.UserName ?? "Unknown",
                Message = privateChat.Message,
                SentAt = privateChat.SentAt,
                IsRead = privateChat.IsRead,
                ReadAt = privateChat.ReadAt
            };
        }
    }
}
