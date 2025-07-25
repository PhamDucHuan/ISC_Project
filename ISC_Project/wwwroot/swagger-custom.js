// Chờ cho toàn bộ nội dung trang tải xong
document.addEventListener('DOMContentLoaded', function () {
    // Tạo một khoảng chờ nhỏ để đảm bảo Swagger UI đã render xong
    setTimeout(function () {
        const infoContainer = document.querySelector('.swagger-ui .info');

        if (infoContainer) {
            // -- BẮT ĐẦU THAY ĐỔI --

            // 1. Tạo một thẻ div mới để làm hộp chứa (wrapper) cho nút
            const buttonWrapper = document.createElement('div');

            // 2. Thêm style cho hộp chứa để đẩy nội dung sang phải
            buttonWrapper.style.display = 'flex';
            buttonWrapper.style.justifyContent = 'flex-end'; // Quan trọng: Đẩy item sang phải
            buttonWrapper.style.marginBottom = '25px';      // Giữ khoảng cách với tiêu đề bên dưới

            // -- KẾT THÚC THAY ĐỔI --

            // Tạo phần tử nút như cũ
            const homeButton = document.createElement('a');
            homeButton.href = '/';
            homeButton.innerText = '← Về trang chủ';

            // Style cho nút (không cần margin nữa vì wrapper đã xử lý)
            homeButton.style.display = 'inline-block';
            homeButton.style.color = '#fff';
            homeButton.style.backgroundColor = '#3b3b3b';
            homeButton.style.textDecoration = 'none';
            homeButton.style.padding = '10px 20px';
            homeButton.style.border = 'none';
            homeButton.style.borderRadius = '4px';
            homeButton.style.fontWeight = 'bold';
            homeButton.style.transition = 'all 0.2s ease';

            homeButton.onmouseover = function () {
                this.style.backgroundColor = '#555';
            };
            homeButton.onmouseout = function () {
                this.style.backgroundColor = '#3b3b3b';
            };
            // 3. Đặt nút vào bên trong hộp chứa
            buttonWrapper.appendChild(homeButton);

            // 4. Đặt hộp chứa (đã có nút bên trong) vào khu vực .info
            infoContainer.prepend(buttonWrapper);
        }
    }, 100);
});