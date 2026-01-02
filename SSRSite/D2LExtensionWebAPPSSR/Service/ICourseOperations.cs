namespace D2LExtensionWebAPPSSR.Service
{
    public interface ICourseOperations
    {
       
        void CreateCourse(string UserId, string title, string description, string semester, string professor, string coursecode);
        List<string> GetCoursesByUser(string UserId)
        void UpdateCourse(int CourseId, string title, string description, string semester, string professor, string coursecode);
        void DeleteCourse(int CourseId);
    }
}
