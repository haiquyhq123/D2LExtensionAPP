using D2LExtensionWebAPPSSR.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
namespace D2LExtensionWebAPPSSR.Service
{
    public class CourseOperations : ICourseOperations
    {
        private readonly string connectionString;
        public CourseOperations(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }
        public void CreateCourse(string UserId, string title, string coursecode, string? description = null, string? semester = null, string? professor = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Create_Course",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure

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
                        Value = description == null?DBNull.Value:description,
                        Direction = ParameterDirection.Input
                      
                    };
                    cmd.Parameters.Add(descriptionParam);
                    SqlParameter semesterParam = new SqlParameter
                    {
                        ParameterName = "@semester",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = semester == null?DBNull.Value:semester,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(semesterParam);
                    SqlParameter professorParam = new SqlParameter
                    {
                        ParameterName = "@professor",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = professor == null ? DBNull.Value : professor,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(professorParam);
                    SqlParameter courseCodeParam = new SqlParameter
                    {
                        ParameterName = "@coursecode",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = coursecode,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(courseCodeParam);

                    // open connection
                    connection.Open();
                    // start execute procedure
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Create Operation");

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Create: {ex.Message}");
            }
            
        }

        public void DeleteCourse(int CourseId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Delete_Course",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure

                    };
                    // specify parameter
                    SqlParameter CId = new SqlParameter
                    {
                        ParameterName = "@CourseId",
                        SqlDbType = SqlDbType.Int,
                        Value = CourseId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(CId);
                 
                    // open connection
                    connection.Open();
                    // start execute procedure
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Delete Course Operation");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Delete: {ex.Message}");
            }

        }

        public List<string> GetCoursesByUser(string UserId)
        {
            var result = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Get_Courses_By_User",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure

                    };
                    // specify parameter
                    SqlParameter UId = new SqlParameter
                    {
                        ParameterName = "@Userid",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 450,
                        Value = UserId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(UId);
                    // open connection
                    connection.Open();
                    // start execute procedure
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        string rowLine = sdr["Id"].ToString() + "|" + sdr["Title"] + "|" + sdr["CourseCode"].ToString() + "|" + sdr["Semester"].ToString() + "|" + sdr["Professor"].ToString() + "|" + sdr["CreateDate"].ToString();
                        result.Add(rowLine);
                    }
                
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
            return result;
        }

        public void UpdateCourse(int CourseId, string title, string description, string semester, string professor, string coursecode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "Update_Course",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure

                    };
                    // specify parameter
                    SqlParameter CId = new SqlParameter
                    {
                        ParameterName = "@CourseId",
                        SqlDbType = SqlDbType.Int,
                        Value = CourseId,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(CId);

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
                    SqlParameter semesterParam = new SqlParameter
                    {
                        ParameterName = "@semester",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = semester == null ? DBNull.Value : semester,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(semesterParam);
                    SqlParameter professorParam = new SqlParameter
                    {
                        ParameterName = "@professor",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = professor == null ? DBNull.Value : professor,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(professorParam);
                    SqlParameter courseCodeParam = new SqlParameter
                    {
                        ParameterName = "@coursecode",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 256,
                        Value = coursecode,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add(courseCodeParam);

                    // open connection
                    connection.Open();
                    // start execute procedure
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Update Operation");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred On Update: {ex.Message}");
            }
        }
    }
}
