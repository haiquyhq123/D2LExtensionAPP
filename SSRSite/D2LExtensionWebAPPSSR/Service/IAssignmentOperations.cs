namespace D2LExtensionWebAPPSSR.Service
{
    public interface IAssignmentOperations
    {
        void CreateAssignment(string userId, int courseWeekId, string title, string? description, DateTime dueDate, int? grade, int weight,string status,decimal estimatedHours = 1.0m);
        void UpdateAssignmentStatus(int assignmentId, string status);

        void DeleteAssignment(int weekId, int assignmentId);

        List<string> GetCalendar(string userId, DateTime startDate, DateTime endDate);

        List<string> GetImportantTasks(string userId,int days = 7,int numberOfTask = 15);
        List<string> GetAssignmentsByWeek(int courseWeekId);
        public List<string> GetAssignmentDetailByUser(string UserId);
    }
}
