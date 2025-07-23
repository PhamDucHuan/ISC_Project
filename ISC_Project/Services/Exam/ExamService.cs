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

        public async Task<int> CreateRandomExamAsync(ExamCreationDto examDto, int teacherUserId)
        {
            var teacherProfile = await _context.TeacherProfiles.FirstOrDefaultAsync(t => t.UserId == teacherUserId);
            if (teacherProfile == null || !teacherProfile.SubjectId.HasValue)
            {
                throw new Exception("Không tìm thấy giáo viên hoặc giáo viên chưa được phân công môn học.");
            }

            // Kiểm tra xem ngân hàng đề có đủ câu hỏi không
            var totalQuestionsInBank = await _context.Questions
                .CountAsync(q => q.SubjectId == teacherProfile.SubjectId.Value);

            if (totalQuestionsInBank < examDto.NumberOfQuestions)
            {
                throw new Exception($"Ngân hàng đề chỉ có {totalQuestionsInBank} câu hỏi, không đủ để tạo bài kiểm tra {examDto.NumberOfQuestions} câu.");
            }

            // Tạo bài kiểm tra và LƯU SỐ LƯỢNG CÂU HỎI
            var labSchedule = new LabSchedule
            {
                LabName = examDto.Name,
                LabStartDate = examDto.StartDate,
                LabEndDate = examDto.EndDate,
                UserId = teacherUserId,
                SubjectId = teacherProfile.SubjectId.Value,
                Status = "Scheduled",
                NumberOfRandomQuestions = examDto.NumberOfQuestions // Lưu số lượng câu hỏi
            };
            _context.LabSchedules.Add(labSchedule);
            await _context.SaveChangesAsync();

            // Gán bài kiểm tra cho các lớp (logic này giữ nguyên)
            foreach (var classId in examDto.ClassIds)
            {
                var scheduleClass = new LabScheduleClass { LabSchedulesId = labSchedule.LabSchedulesId, ClassId = classId };
                _context.LabScheduleClasses.Add(scheduleClass);
            }

            await _context.SaveChangesAsync();
            return labSchedule.LabSchedulesId;
        }

        // ✅ 2. SỬA LẠI PHƯƠNG THỨC BẮT ĐẦU LÀM BÀI
        public async Task<StartedExamDto> StartExamAsync(int labScheduleId, int studentUserId)
        {
            var labSchedule = await _context.LabSchedules.FindAsync(labScheduleId);
            if (labSchedule == null || !labSchedule.NumberOfRandomQuestions.HasValue)
            {
                throw new Exception("Bài kiểm tra không hợp lệ hoặc không được cấu hình để lấy câu hỏi ngẫu nhiên.");
            }

            // 1. Lấy ID của tất cả câu hỏi có thể một cách hiệu quả
            var allQuestionIds = await _context.Questions
                .Where(q => q.SubjectId == labSchedule.SubjectId)
                .Select(q => q.QuestionsId)
                .ToListAsync();

            // 2. Xáo trộn danh sách ID trong bộ nhớ (cách này rất nhanh)
            var random = new Random();
            var shuffledIds = allQuestionIds.OrderBy(id => random.Next()).ToList();

            // 3. Lấy ra số lượng ID cần thiết cho bài kiểm tra
            var selectedQuestionIds = shuffledIds.Take(labSchedule.NumberOfRandomQuestions.Value).ToList();

            // 4. Bây giờ, mới truy vấn CSDL để lấy toàn bộ thông tin chi tiết của các câu hỏi đã được chọn
            var randomQuestions = await _context.Questions
                .Where(q => selectedQuestionIds.Contains(q.QuestionsId))
                .Include(q => q.QuestionOptions)
                .ToListAsync();

            // Phần còn lại của code vẫn giữ nguyên...
            var submission = new StudentSubmission
            {
                SubmissionTime = DateTime.UtcNow,
                UserId = studentUserId,
                LabSchedulesId = labScheduleId,
            };
            _context.StudentSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            var resultDto = new StartedExamDto
            {
                SubmissionId = submission.SubmissionsId,
                // Xáo trộn lại danh sách cuối cùng để hiển thị cho sinh viên theo thứ tự ngẫu nhiên
                ShuffledQuestions = randomQuestions.OrderBy(q => random.Next()).Select(q => new QuestionForStudentDto
                {
                    QuestionId = q.QuestionsId,
                    QuestionText = q.QuestionsText,
                    Options = q.QuestionOptions.OrderBy(o => random.Next()).Select(o => new OptionDto
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
