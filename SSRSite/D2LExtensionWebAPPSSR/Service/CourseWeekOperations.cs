using Microsoft.Data.SqlClient;
using System.Data;

namespace D2LExtensionWebAPPSSR.Service
{
    public class CourseWeekOperations : ICourseWeekOperations
    {
        private readonly string connectionString;

        public CourseWeekOperations(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void CreateCourseWeek(int CourseId, string title, string? description = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Create_CourseWeek",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlParameter courseIdParam = new SqlParameter
                    {
                        ParameterName = "@courseid",
                        SqlDbType = SqlDbType.Int,
                        Value = CourseId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(courseIdParam);

                    SqlParameter titleParam = new SqlParameter
                    {
                        ParameterName = "@title",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = title,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(titleParam);

                    SqlParameter descriptionParam = new SqlParameter
                    {
                        ParameterName = "@description",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = description == null ? DBNull.Value : description,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(descriptionParam);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Create CourseWeek Operation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Create CourseWeek: {ex.Message}");
            }
        }

        public void UpdateCourseWeek(int CourseWeekId, string title, string? description)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Update_CourseWeek",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlParameter cwIdParam = new SqlParameter
                    {
                        ParameterName = "@courseweekid",
                        SqlDbType = SqlDbType.Int,
                        Value = CourseWeekId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(cwIdParam);

                    SqlParameter titleParam = new SqlParameter
                    {
                        ParameterName = "@title",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = title,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(titleParam);

                    SqlParameter descriptionParam = new SqlParameter
                    {
                        ParameterName = "@description",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = description == null ? DBNull.Value : description,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(descriptionParam);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Update CourseWeek Operation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Update CourseWeek: {ex.Message}");
            }
        }

        public void DeleteCourseWeek(int CourseWeekId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Delete_CourseWeek",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlParameter cwIdParam = new SqlParameter
                    {
                        ParameterName = "@courseweekid",
                        SqlDbType = SqlDbType.Int,
                        Value = CourseWeekId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(cwIdParam);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Delete CourseWeek Operation");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Delete CourseWeek: {ex.Message}");
            }
        }

        public List<string> GetCourseWeeksByCourse(int CourseId)
        {
            var result = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Get_CourseWeeks_By_Course",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlParameter courseIdParam = new SqlParameter
                    {
                        ParameterName = "@courseid",
                        SqlDbType = SqlDbType.Int,
                        Value = CourseId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(courseIdParam);

                    connection.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();

                    while (sdr.Read())
                    {
                        string rowLine = sdr["Id"].ToString() + "|" + sdr["CourseId"].ToString() + "|" + sdr["Title"].ToString() + "|" + sdr["Description"].ToString();

                        result.Add(rowLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Get CourseWeeks: {ex.Message}");
            }

            return result;
        }
    }
}

