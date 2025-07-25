﻿using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.QuestionBank
{
    public class UpdateQuestionDto
    {
        [Required]
        public string QuestionText { get; set; } = string.Empty!;
        [Required]
        [RegularExpression("^(SingleChoice|MultipleChoice|FillInTheBlank)$", ErrorMessage = "QuestionType không hợp lệ.")]
        public string QuestionType { get; set; } = string.Empty;
        public int? SubjectId { get; set; }
        public List<UpdateQuestionOptionDto> Options { get; set; } = new();
    }

    public class UpdateQuestionOptionDto
    {
        public int Id { get; set; }
        [Required]
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}