﻿using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Role
{
    // DTO để trả về
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty!;
        public bool IsAdmin { get; set; }
    }

    // DTO để tạo mới
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Tên vai trò không được để trống.")]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty!;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty!;

        public bool IsAdmin { get; set; }
    }
}