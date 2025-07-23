using System;
using System.Collections.Generic;
using System.Linq;

namespace ISC_Project.Utils
{
    public static class PasswordValidator
    {
        /// <summary>
        /// Kiểm tra một mật khẩu có đủ mạnh hay không và đưa ra các lỗi cụ thể.
        /// Ném ra ArgumentException nếu không hợp lệ.
        /// </summary>
        /// <param name="password">Mật khẩu cần kiểm tra</param>
        public static void Validate(string password)
        {
            var errorMessages = new List<string>();

            // 1. Kiểm tra độ dài
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                errorMessages.Add("phải có ít nhất 8 ký tự");
            }

            // 2. Kiểm tra có chữ thường không
            // Dùng LINQ .Any() để kiểm tra sự tồn tại của ký tự thỏa mãn điều kiện
            if (!password.Any(char.IsLower))
            {
                errorMessages.Add("phải chứa ít nhất một chữ thường");
            }

            // 3. Kiểm tra có chữ viết hoa không
            if (!password.Any(char.IsUpper))
            {
                errorMessages.Add("phải chứa ít nhất một chữ viết hoa");
            }

            // 4. Kiểm tra có chữ số không
            if (!password.Any(char.IsDigit))
            {
                errorMessages.Add("phải chứa ít nhất một chữ số");
            }

            // 5. Kiểm tra có ký tự đặc biệt không
            // Ký tự đặc biệt là ký tự không phải chữ hoặc số
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                errorMessages.Add("phải chứa ít nhất một ký tự đặc biệt");
            }

            // Nếu có bất kỳ lỗi nào, gộp chúng lại và ném ra exception
            if (errorMessages.Any())
            {
                // Gộp các thông báo lỗi lại, cách nhau bằng dấu phẩy
                var fullErrorMessage = "Mật khẩu không đủ mạnh. Mật khẩu " + string.Join(", ", errorMessages) + ".";
                throw new ArgumentException(fullErrorMessage);
            }
        }
    }
}