const express = require('express');
const app = express();
const http = require('http');
const server = http.createServer(app);
const { Server } = require("socket.io");
const io = new Server(server);

// API của bạn sử dụng đối tượng này để lưu trữ người dùng đang online
const users = {};
// API sử dụng đối tượng này để lưu lịch sử chat (Mô phỏng, thực tế sẽ dùng CSDL)
const messageHistory = {};

// Endpoint để phục vụ file giao diện
app.get('/', (req, res) => {
    res.sendFile(__dirname + '/ChatAI.html');
});

// Toàn bộ logic API xử lý chat real-time nằm ở đây
io.on('connection', (socket) => {
    console.log(`Một người dùng đã kết nối: ${socket.id}`);

    // 1. API: Xử lý khi có người dùng mới
    const userId = socket.id;
    users[userId] = {
        id: userId,
        name: `User-${userId.substring(0, 5)}`, // Tạo tên người dùng tạm thời
        status: 'online'
    };

    // 2. API: Gửi danh sách tất cả người dùng cho client vừa kết nối
    socket.emit('all users', users);

    // 3. API: Gửi thông tin người dùng mới cho tất cả những người khác
    socket.broadcast.emit('user connected', users[userId]);

    // 4. API: Lắng nghe sự kiện một client gửi tin nhắn riêng tư
    socket.on('private message', ({ to, text }) => {
        console.log(`Tin nhắn từ ${userId} đến ${to}: ${text}`);

        // Lưu tin nhắn vào lịch sử (phần này bạn sẽ thay bằng CSDL)
        const conversationId1 = `${userId}-${to}`;
        const conversationId2 = `${to}-${userId}`;
        if (!messageHistory[conversationId1] && !messageHistory[conversationId2]) {
            messageHistory[conversationId1] = [];
        }
        const history = messageHistory[conversationId1] || messageHistory[conversationId2];
        const message = { from: userId, to, text, timestamp: new Date() };
        history.push(message);

        // Chỉ gửi tin nhắn đến người nhận cụ thể (to)
        io.to(to).emit('private message', message);
    });

    // 5. API: Lắng nghe sự kiện client yêu cầu tải lịch sử chat
    socket.on('load history', (otherUserId) => {
        const conversationId1 = `${userId}-${otherUserId}`;
        const conversationId2 = `${otherUserId}-${userId}`;
        const history = messageHistory[conversationId1] || messageHistory[conversationId2] || [];
        // Gửi lịch sử chat về cho client đã yêu cầu
        socket.emit('chat history', { with: otherUserId, history });
    });

    // 6. API: Xử lý khi người dùng ngắt kết nối
    socket.on('disconnect', () => {
        console.log(`Người dùng đã ngắt kết nối: ${userId}`);
        delete users[userId];
        // Gửi thông báo người dùng đã thoát cho tất cả những người khác
        io.emit('user disconnected', userId);
    });
});

const PORT = 3000;
server.listen(PORT, () => {
    console.log(`Server (API) của bạn đang chạy tại http://localhost:${PORT}`);
});