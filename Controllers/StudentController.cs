using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentDetailAPI.Model;
using System.Globalization;
using System.Text.RegularExpressions;


namespace StudentDetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpPost]

        public IActionResult InsertDetails(int sid,String sname,String course,int fees,String phonenumber)
        {
            if (sid <= 0)
            {
                return BadRequest("Invalid student ID");
            }
            if (string.IsNullOrWhiteSpace(sname) || !Regex.IsMatch(sname, @"^[a-zA-Z ]+$"))
            {
                return BadRequest("Enter a valid student name");
            }
            if (string.IsNullOrWhiteSpace(course))
            {
                return BadRequest("Course is required");
            }
            string c = course.Trim().ToLower();
            if (c != "c" && c != "c++" && c != "java" && c != "python" && c != ".net")
            {
                return BadRequest("Enter a valid course name(c,c++,java,python,.net)");
            }
            if (fees < 5000)
            {
                return BadRequest("Enter a valid fees amount");
            }
            if (string.IsNullOrWhiteSpace(phonenumber) || !Regex.IsMatch(phonenumber, @"^[0-9]{10}$"))
            {
                return BadRequest("Enter a valid phone number");
            }
            string connectionstring = "data source=localhost;initial catalog=StudentDB;integrated security=sspi;Trustservercertificate=true";
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    cn.Open();
                    string query = "Insert into student values(@sid,@sname,@course,@fees,@phonenumber)";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        
                        cmd.Parameters.AddWithValue("@sid", sid);
                        cmd.Parameters.AddWithValue("@sname", sname);
                        cmd.Parameters.AddWithValue("@course", c);
                        cmd.Parameters.AddWithValue("@fees", fees);
                        cmd.Parameters.AddWithValue("@phonenumber", phonenumber);


                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return Ok("Student Details inserted successfully");
                        }
                        else
                        {
                            return BadRequest("Failed!!!!!!!!!!!!!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
           
        }

            
        

        [HttpGet]

        public IActionResult DisplayDetails()
        {
            string connectionstring = "data source=localhost;initial catalog=StudentDB;integrated security=sspi;Trustservercertificate=true";
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    cn.Open();
                    string query = "Select * from student";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            List<StudentModel> students = new List<StudentModel>();
                            while (dr.Read())
                            {
                                StudentModel s = new StudentModel();
                                s.studentId = Convert.ToInt32(dr["Sid"]);
                                s.studentName = Convert.ToString(dr["Sname"]);
                                s.Course = Convert.ToString(dr["Course"]);
                                s.Fees = Convert.ToInt32(dr["Fees"]);
                                s.PhoneNumber = Convert.ToString(dr["Mobile"]);
                                students.Add(s);

                            }
                            if (students.Count > 0)
                            {
                                return Ok(students);
                            }
                            else
                            {
                                return NotFound("No student details found");
                            }

                        }
                    }
                }
            }
            catch (SqlException)
            {
                return StatusCode(500, "Database Error Occured");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error!!!!!");
            }
        }



        [HttpGet("{ID}")]
        public IActionResult GetStudentDetails(int ID)
        {
            if (ID <= 0)
            {
                return BadRequest("Invalid Student ID");
            }
            string connectionstring = "data source=localhost;initial catalog=StudentDB;integrated security=sspi;Trustservercertificate=true";
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    cn.Open();
                    string query = "Select * from student where Sid=@ID";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                StudentModel s = new StudentModel();
                                s.studentId = Convert.ToInt32(dr["Sid"]);
                                s.studentName = Convert.ToString(dr["Sname"]);
                                s.Course = Convert.ToString(dr["Course"]);
                                s.Fees = Convert.ToInt32(dr["Fees"]);
                                s.PhoneNumber = Convert.ToString(dr["Mobile"]);
                                return Ok(s);
                            }
                            else
                            {
                                return NotFound("Student not found");
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return StatusCode(500, "Database Error Occured");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error!!!!!");
            }
        }

        [HttpPut("{ID}")]

        public IActionResult UpdateDetails(int ID,  String sname, String course, int fees, String phonenumber)
        {
            if (ID <= 0)
            {
                return BadRequest("Invalid Student ID");
            }
            if (string.IsNullOrWhiteSpace(sname) || !Regex.IsMatch(sname, @"^[a-zA-Z ]+$"))
            {
                return BadRequest("Enter a valid student name");
            }
            if (string.IsNullOrWhiteSpace(course))
            {
                return BadRequest("Course is required");
            }
            string c = course.Trim().ToLower();
            if (c != "c" && c != "c++" && c != "java" && c != "python" && c != ".net")
            {
                return BadRequest("Enter a valid course name(c,c++,java,python,.net)");
            }
            if (fees < 5000)
            {
                return BadRequest("Enter a valid fees amount");
            }
            if (string.IsNullOrWhiteSpace(phonenumber) || !Regex.IsMatch(phonenumber, @"^[0-9]{10}$"))
            {
                return BadRequest("Enter a valid phone number");
            }
            string connectionstring = "data source=localhost;initial catalog=StudentDB;integrated security=sspi;Trustservercertificate=true";
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    cn.Open();
                    string query = "Update student set Sname=@sname,Course=@course,Fees=@fees,Mobile=@phonenumber where Sid=@ID";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@sname", sname);
                        cmd.Parameters.AddWithValue("@course", c);
                        cmd.Parameters.AddWithValue("@fees", fees);
                        cmd.Parameters.AddWithValue("@phonenumber", phonenumber);


                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return Ok("Student Details updated successfully");
                        }
                        else
                        {
                            return NotFound("Student not found");
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return StatusCode(500, "Error occurred while updating student details.");
            }
        }
     

        

        [HttpPatch("{ID}")]
        public IActionResult UpdatePartialDetails(int ID, string course, int fee)
        {
            if (ID <= 0)
            {
                return BadRequest("Invalid Student ID");
            }
            if (string.IsNullOrWhiteSpace(course))
            {
                return BadRequest("Course is required");
            }
            string c = course.Trim().ToLower();
            if (c != "c" && c != "c++" && c != "java" && c != "python" && c != ".net")
            {
                return BadRequest("Enter a valid course name(c,c++,java,python,.net)");
            }
            if (fee < 5000)
            {
                return BadRequest("Enter a valid fees amount");
            }
            string connectionstring = "data source=localhost;initial catalog=StudentDB;integrated security=sspi;Trustservercertificate=true";
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    cn.Open();
                    string query = "Update student set Course=@course,Fees=@fees where Sid=@ID";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@course", course);
                        cmd.Parameters.AddWithValue("@fees", fee);


                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return Ok("Student Details updated successfully");
                        }
                        else
                        {
                            return NotFound("Student not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message );
            }
                }
      

        [HttpDelete("{ID}")]
        public IActionResult DeleteStudent(int ID)
        {
            if (ID <= 0)
            {
                return BadRequest("Invalid Student ID");
            }

            string connectionstring = "data source=localhost;initial catalog=StudentDB;integrated security=sspi;Trustservercertificate=true";
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionstring))
                {
                    cn.Open();
                    string query = "Delete from student where Sid=@ID";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);


                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return Ok("Student deleted successfully");
                        }
                        else
                        {
                            return NotFound("Student not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}


