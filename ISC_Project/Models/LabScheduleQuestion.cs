using System;
using System.Collections.Generic;

namespace ISC_Project.Models
{
    public partial class LabScheduleQuestion
    {
        public int LabSchedulesId { get; set; }
        public int QuestionsId { get; set; }
        public int LabScheduleQuestionsId { get; set; }

        public virtual LabSchedule LabSchedules { get; set; } = null!;
        public virtual Question Questions { get; set; } = null!;
    }
}
