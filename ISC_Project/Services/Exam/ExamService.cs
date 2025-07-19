using ISC_Project.Data;
using ISC_Project.DTOs.Exam;
using ISC_Project.Interface.Exam;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ISC_Project.Services.Exam
{
    public class ExamService: IExamService
    {
        private readonly ISC_ProjectDbContext _context;

        public ExamService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateExamAsync(ExamCreationDto examDto, int teacherUserId)
        {
            // 1. Xác thực giáo viên và môn học
            var teacherProfile = await _context.TeacherProfiles
                .FirstOrDefaultAsync(t => t.UserId == teacherUserId);

            if (teacherProfile == null || !teacherProfile.SubjectId.HasValue)
            {
                throw new Exception("Không tìm thấy giáo viên hoặc giáo viên chưa được phân công môn học.");
            }

            // 2. Tạo bài kiểm tra (LabSchedule)
            var labSchedule = new LabSchedule
            {
                LabName = examDto.Name,
                LabStartDate = examDto.StartDate,
                LabEndDate = examDto.EndDate,
                UserId = teacherUserId,
                SubjectId = teacherProfile.SubjectId.Value,
                Status = "Scheduled"
            };
            _context.LabSchedules.Add(labSchedule);
            await _context.SaveChangesAsync(); // Lưu để lấy LabSchedules_ID

            // 3. Thêm các câu hỏi đã chọn vào bài kiểm tra
            foreach (var questionId in examDto.QuestionIds)
            {
                var examQuestion = new LabScheduleQuestion
                {
                    LabSchedulesId = labSchedule.LabSchedulesId,
                    QuestionsId = questionId
                };
                _context.LabScheduleQuestions.Add(examQuestion);
            }

            // 4. Gán bài kiểm tra cho các lớp
            foreach (var classId in examDto.ClassIds)
            {
                var scheduleClass = new LabScheduleClass
                {
                    LabSchedulesId = labSchedule.LabSchedulesId,
                    ClassId = classId
                };
                _context.LabScheduleClasses.Add(scheduleClass);
            }

            await _context.SaveChangesAsync();
            return labSchedule.LabSchedulesId;
        }

        public async Task<StartedExamDto> StartExamAsync(int labScheduleId, int studentUserId)
        {
            // 1. Tạo một lượt làm bài mới
            var submission = new StudentSubmission
            {
                SubmissionTime = DateTime.UtcNow,
                UserId = studentUserId,
                LabSchedulesId = labScheduleId,
                // Các trường khác có thể set giá trị mặc định
            };
            _context.StudentSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            // 2. Lấy danh sách câu hỏi và hoán đổi vị trí
            var questions = await _context.LabScheduleQuestions
                .Where(lq => lq.LabSchedulesId == labScheduleId)
                .Include(lq => lq.Questions)
                    .ThenInclude(q => q.QuestionOptions)
                .Select(lq => lq.Questions)
                .ToListAsync();

            // Logic hoán đổi ngẫu nhiên
            var random = new Random();
            var shuffledQuestions = questions.OrderBy(q => random.Next()).ToList();

            // 3. Chuyển đổi sang DTO để gửi về cho client
            var resultDto = new StartedExamDto
            {
                SubmissionId = submission.SubmissionsId,
                ShuffledQuestions = shuffledQuestions.Select(q => new QuestionForStudentDto
                {
                    QuestionId = q.QuestionsId,
                    QuestionText = q.QuestionsText,
                    Options = q.QuestionOptions.Select(o => new OptionDto
                    {
                        OptionId = o.QuestionOptionsId,
                        OptionText = o.OptionText
                    }).ToList()
                }).ToList()
            };

            return resultDto;
        }

        public async Task SubmitAnswerAsync(SubmitAnswerDto answerDto)
        {
            var studentAnswer = new StudentMcqanswer
            {
                SubmissionsId = answerDto.SubmissionId,
                QuestionsId = answerDto.QuestionId,
                QuestionOptionsId = answerDto.SelectedOptionId
            };
            _context.StudentMcqanswers.Add(studentAnswer);
            await _context.SaveChangesAsync();
        }

        public async Task<ExamResultDto> FinalizeAndGradeExamAsync(int submissionId)
        {
            // 1. Lấy tất cả câu trả lời của học sinh cho lượt làm bài này
            var studentAnswers = await _context.StudentMcqanswers
                .Where(a => a.SubmissionsId == submissionId)
                .ToListAsync();

            if (!studentAnswers.Any())
            {
                throw new Exception("Không tìm thấy bài nộp.");
            }

            var questionIds = studentAnswers.Select(a => a.QuestionsId).ToList();

            // 2. Lấy tất cả các câu hỏi liên quan và các lựa chọn của chúng
            var questionsInExam = await _context.Questions
                .Where(q => questionIds.Contains(q.QuestionsId))
                .Include(q => q.QuestionOptions) // Lấy tất cả các lựa chọn
                .ToDictionaryAsync(q => q.QuestionsId); // Chuyển thành Dictionary để tra cứu nhanh

            // 3. Lấy từ điển đáp án đúng
            var correctOptionsDict = questionsInExam.Values
                .SelectMany(q => q.QuestionOptions)
                .Where(o => o.IsCorrect == true)
                .ToDictionary(o => o.QuestionsId, o => o);


            var detailedResults = new List<QuestionResultDto>();
            int correctCount = 0;

            // 4. Xây dựng kết quả chi tiết cho từng câu hỏi
            foreach (var studentAnswer in studentAnswers)
            {
                Question question = questionsInExam[(int)studentAnswer.QuestionsId];
                var correctOption = correctOptionsDict.GetValueOrDefault(studentAnswer.QuestionsId);
                var studentOption = question.QuestionOptions.FirstOrDefault(o => o.QuestionOptionsId == studentAnswer.QuestionOptionsId);

                bool isAnswerCorrect = correctOption != null && studentAnswer.QuestionOptionsId == correctOption.QuestionOptionsId;
                if (isAnswerCorrect)
                {
                    correctCount++;
                }

                detailedResults.Add(new QuestionResultDto
                {
                    QuestionId = question.QuestionsId,
                    QuestionText = question.QuestionsText,
                    YourAnswerOptionId = (int)studentAnswer.QuestionOptionsId,
                    YourAnswerText = studentOption?.OptionText ?? "Không trả lời",
                    CorrectAnswerOptionId = correctOption?.QuestionOptionsId ?? 0,
                    CorrectAnswerText = correctOption?.OptionText ?? "Không có đáp án đúng",
                    IsCorrect = isAnswerCorrect
                });
            }

            double finalScore = (double)correctCount / questionsInExam.Count * 10; // Thang điểm 10

            // 5. Lưu điểm vào bảng Student_Grades
            var grade = new StudentGrade
            {
                Score = finalScore,
                GradedTime = DateTime.UtcNow,
                SubmissionId = submissionId
            };
            _context.StudentGrades.Add(grade);
            await _context.SaveChangesAsync();

            // 6. Trả về DTO kết quả cuối cùng
            return new ExamResultDto
            {
                SubmissionId = submissionId,
                FinalScore = Math.Round(finalScore, 2), // Làm tròn điểm
                Results = detailedResults
            };
        }
    }
}
