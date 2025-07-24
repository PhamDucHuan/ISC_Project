﻿using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Class
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
    }

    public class CreateClassDto
    {
        [Required]
        [StringLength(100)]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ClassCode { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int ClassTypeId { get; set; }
    }
}