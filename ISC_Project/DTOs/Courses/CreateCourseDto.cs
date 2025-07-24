﻿using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Course
{
    public class CourseDto
    {
        public int CoursesId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal DefaultPrice { get; set; }
    }

    public class CreateCourseDto
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DefaultPrice { get; set; }
        [Required]
        public int CourseCategoriesId { get; set; }
    }
}