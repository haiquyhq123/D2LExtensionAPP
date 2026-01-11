using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;
using System.Numerics;

namespace D2LExtensionWebAPPSSR.Service
{
    public class AssingmentOperations : IAssignmentOperations
    {
        private readonly string connectionString;

        public AssingmentOperations(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void CreateAssignment(string userId,int courseWeekId,string title, string? description, DateTime dueDate, int? grade, int weight, string status, decimal estimatedHours = 1.0m){
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Create_Assignment",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    // parameters
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar, 450)
                    {
                        Value = userId
                    });

                    cmd.Parameters.Add(new SqlParameter("@CourseWeekId", SqlDbType.Int)
                    {
                        Value = courseWeekId
                    });

                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 256)
                    {
                        Value = title
                    });

                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 256)
                    {
                        Value = description == null ? DBNull.Value : description
                    });

                    cmd.Parameters.Add(new SqlParameter("@DueDate", SqlDbType.DateTime)
                    {
                        Value = dueDate
                    });

                    cmd.Parameters.Add(new SqlParameter("@Grade", SqlDbType.Int)
                    {
                        Value = grade == null ? DBNull.Value : grade
                    });

                    cmd.Parameters.Add(new SqlParameter("@Weight", SqlDbType.Int)
                    {
                        Value = weight
                    });

                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 256)
                    {
                        Value = status
                    });

                    cmd.Parameters.Add(new SqlParameter("@EstimatedHours", SqlDbType.Decimal)
                    {
                        Precision = 4,
                        Scale = 1,
                        Value = estimatedHours
                    });

                    // execute
                    connection.Open();
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Create Assignment Operation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Create Assignment: {ex.Message}");
            }

        }
        public void UpdateAssignmentStatus(int assignmentId, string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Update_Assignment_Status",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@AssignmentId", SqlDbType.Int)
                    {
                        Value = assignmentId
                    });

                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 256)
                    {
                        Value = status
                    });

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Update Assignment Status Operation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Update Status: {ex.Message}");
            }
        }
        public List<string> GetCalendar(string userId, DateTime startDate, DateTime endDate)
        {
            var result = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Get_Calendar",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@UsedId", SqlDbType.NVarChar, 450)
                    {
                        Value = userId
                    });

                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)
                    {
                        Value = startDate
                    });

                    cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)
                    {
                        Value = endDate
                    });

                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        string rowLine = sdr["Id"] + "|" + sdr["Title"] + "|" + sdr["DueDate"] + "|" + sdr["Status"] + "|" + sdr["Priority"] + "|" + sdr["EstimatedHours"] + "|" + sdr["CourseTitle"] + "|" + sdr["WeekTitle"];

                        result.Add(rowLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Get Calendar: {ex.Message}");
            }

            return result;
        }
        public List<string> GetImportantTasks(string userId, int days = 7, int numberOfTask = 15)
        {
            var result = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Get_Important_Tasks",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar, 450)
                    {
                        Value = userId
                    });

                    cmd.Parameters.Add(new SqlParameter("@Days", SqlDbType.Int)
                    {
                        Value = days
                    });

                    cmd.Parameters.Add(new SqlParameter("@NumberOfTask", SqlDbType.Int)
                    {
                        Value = numberOfTask
                    });

                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        string rowLine = sdr["Id"] + "|" + sdr["Title"] + "|" + sdr["DueDate"] + "|" + sdr["Status"] + "|" + sdr["Priority"] + "|" + sdr["EstimatedHours"] + "|" + sdr["CourseTitle"] + "|" + sdr["WeekTitle"];

                        result.Add(rowLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Get Important Tasks: {ex.Message}");
            }

            return result;
        }
        public void DeleteAssignment(int weekId, int assignmentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Delete_Assignment",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    // WeekId parameter
                    SqlParameter WId = new SqlParameter
                    {
                        ParameterName = "@WeekId",
                        SqlDbType = SqlDbType.Int,
                        Value = weekId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(WId);

                    // AssignmentId parameter
                    SqlParameter AId = new SqlParameter
                    {
                        ParameterName = "@AssignmentId",
                        SqlDbType = SqlDbType.Int,
                        Value = assignmentId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(AId);

                    // execute
                    connection.Open();
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Delete Assignment Operation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Delete Assignment: {ex.Message}");
            }
        }
        public List<string> GetAssignmentsByWeek(int courseWeekId)
        {
            var result = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandText = @"Select * From Assignment_Detail Where CourseWeekId = @CourseWeekId Order By Priority DESC, DueDate ASC",
                        Connection = connection,
                        CommandType = CommandType.Text
                    };

                    SqlParameter cwid = new SqlParameter
                    {
                        ParameterName = "@CourseWeekId",
                        SqlDbType = SqlDbType.Int,
                        Value = courseWeekId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(cwid);
                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        string rowLine = sdr["Id"] + "|" + sdr["Title"] + "|" + sdr["DueDate"] + "|" + sdr["Status"] + "|" + sdr["Priority"] + "|" + sdr["EstimatedHours"] + "|" + sdr["CourseTitle"] + "|" + sdr["WeekTitle"];
                        result.Add(rowLine);
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine($"Exception Occurred On GetAssignmentsByWeek: {ex.Message}");

            }
            return result;
                

        }
        public List<string> GetAssignmentDetailByUser(string UserId)
        {
            var result = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandText = @"Select * From Assignment_Detail Where UserId = @UserId Order By Priority DESC, DueDate ASC",
                        Connection = connection,
                        CommandType = CommandType.Text
                    };

                    // specify parameter
                    SqlParameter UId = new SqlParameter
                    {
                        ParameterName = "@UserId",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 450,
                        Value = UserId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(UId);
                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        string rowLine = sdr["Id"] + "|" + sdr["Title"] + "|" + sdr["DueDate"] + "|" + sdr["Status"] + "|" + sdr["Priority"] + "|" + sdr["EstimatedHours"] + "|" + sdr["CourseTitle"] + "|" + sdr["WeekTitle"];
                        result.Add(rowLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On GetAssignmentsByWeek: {ex.Message}");

            }
            return result;


        }
    }
}
