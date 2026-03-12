using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using D2LExtensionWebAPPSSR.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D2LExtensionWebAPPSSR.Repositories
{
    public interface IPlannerDataRepository
    {
        Task<IEnumerable<AssignmentItem>> GetAssignmentsDueBetweenAsync(string userId, DateTime startDate, DateTime endDate);
        Task SaveReportAsync(ReportEntity report);
        Task<ReportEntity> GetReportByIdAsync(Guid reportId);
        Task<IEnumerable<(string Id, string Email, string UserName)>> GetAllUsersAsync();
    }

    public class PlannerDataRepository : IPlannerDataRepository
    {
        private readonly string _connectionString;

        public PlannerDataRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<AssignmentItem>> GetAssignmentsDueBetweenAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var assignments = new List<AssignmentItem>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT Id, Title, CourseTitle, DueDate, Status, Priority, EstimatedHours
                FROM Assignment_Detail 
                WHERE UserId = @UserId 
                  AND DueDate BETWEEN @Start AND @End
                  AND Status <> 'Finish'
                ORDER BY Priority DESC, DueDate ASC";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Start", startDate);
            cmd.Parameters.AddWithValue("@End", endDate);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                assignments.Add(new AssignmentItem
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    CourseTitle = reader.IsDBNull(reader.GetOrdinal("CourseTitle")) ? "General" : reader.GetString(reader.GetOrdinal("CourseTitle")),
                    DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                    Status = reader.GetString(reader.GetOrdinal("Status")),
                    Priority = reader.GetInt32(reader.GetOrdinal("Priority")),
                    EstimatedHours = reader.GetDecimal(reader.GetOrdinal("EstimatedHours"))
                });
            }

            return assignments;
        }

        public async Task SaveReportAsync(ReportEntity report)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                INSERT INTO WeeklyReports (Id, UserId, WeekStartDate, WeekEndDate, AiRecommendations, CreatedAt)
                VALUES (@Id, @UserId, @WeekStartDate, @WeekEndDate, @AiRecommendations, @CreatedAt)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", report.Id);
            cmd.Parameters.AddWithValue("@UserId", report.UserId);
            cmd.Parameters.AddWithValue("@WeekStartDate", report.WeekStartDate);
            cmd.Parameters.AddWithValue("@WeekEndDate", report.WeekEndDate);
            cmd.Parameters.AddWithValue("@AiRecommendations", report.AiRecommendations);
            cmd.Parameters.AddWithValue("@CreatedAt", report.CreatedAt);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<ReportEntity> GetReportByIdAsync(Guid reportId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT * FROM WeeklyReports WHERE Id = @Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", reportId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ReportEntity
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    UserId = reader.GetString(reader.GetOrdinal("UserId")),
                    WeekStartDate = reader.GetDateTime(reader.GetOrdinal("WeekStartDate")),
                    WeekEndDate = reader.GetDateTime(reader.GetOrdinal("WeekEndDate")),
                    AiRecommendations = reader.GetString(reader.GetOrdinal("AiRecommendations")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };
            }
            return null;
        }

        public async Task<IEnumerable<(string Id, string Email, string UserName)>> GetAllUsersAsync()
        {
            var users = new List<(string Id, string Email, string UserName)>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT Id, Email, UserName FROM AspNetUsers";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add((
                    reader.GetString(reader.GetOrdinal("Id")),
                    reader.GetString(reader.GetOrdinal("Email")),
                    reader.GetString(reader.GetOrdinal("UserName"))
                ));
            }
            return users;
        }
    }
}