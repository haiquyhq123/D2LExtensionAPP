using System;
using System.Collections.Generic;

namespace D2LExtensionWebAPPSSR.Models
{
    public class AssignmentItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CourseTitle { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public decimal EstimatedHours { get; set; }
    }

    public class ReportEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string AiRecommendations { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class WeeklyReportViewModel
    {
        public Guid ReportId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public List<AssignmentItem> Assignments { get; set; } = new();
        public string AiRecommendations { get; set; }
        public List<int> AssignmentsPerDay { get; set; } = new();
    }

    public class TestReportRequest
    {
        public string UserId { get; set; }
        public string TestEmailAddress { get; set; }
        public string TestUserName { get; set; }
    }
}