namespace D2LExtensionWebAPPSSR.Service
{
    public interface ICourseWeekOperations
    {
        void CreateCourseWeek(int CourseId, string title, string? description = null);
        void UpdateCourseWeek(int CourseWeekId, string title, string? description);
        void DeleteCourseWeek(int CourseWeekId);
        List<string> GetCourseWeeksByCourse(int CourseId);
    }
}
