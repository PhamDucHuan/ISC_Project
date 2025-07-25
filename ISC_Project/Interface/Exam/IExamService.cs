﻿using ISC_Project.DTOs.Exam;

namespace ISC_Project.Interface.Exam
{
    public interface IExamService
    {
        Task<int> CreateRandomExamAsync(ExamCreationDto examDto, int teacherUserId);

        Task<StartedExamDto> StartExamAsync(int labScheduleId, int studentUserId);

        Task SubmitAnswerAsync(SubmitAnswerDto answerDto);

        Task<ExamResultDto> FinalizeAndGradeExamAsync(int submissionId);
    }
}
