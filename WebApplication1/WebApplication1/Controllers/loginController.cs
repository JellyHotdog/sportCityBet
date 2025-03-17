using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class loginController : Controller
    {
        [HttpPost]
        public IActionResult Login([FromBody] LoginDetails details)
        {

            int uid;
            //database logic
            dbConfig db = new dbConfig();

            //create account
            using (db.dbConn)
            {
                db.dbConn.Open();

                string sql = 
                    "insert into login values(@email,@uname,@password);" +
                    "insert into users (fname,sname,email,uname,dob) values(@fname,@sname,@email,@uname,@dob);" +
                    "insert into points (1000, (select userid from users where email = @email))" +
                    "select userid from users where email = @email";

                using (var command = new NpgsqlCommand(sql, db.dbConn))
                {
                    command.Parameters.AddWithValue("email", details.Email);
                    command.Parameters.AddWithValue("uname", details.Username);
                    command.Parameters.AddWithValue("password", details.Password);
                    command.Parameters.AddWithValue("fname", details.FirstName);
                    command.Parameters.AddWithValue("sname", details.LastName);
                    command.Parameters.AddWithValue("dob", details.DateOfBirth);

                    uid = (int)command.ExecuteScalar();
                }
            }

            // Build the response with the slot results and the payout
            var response = new
            {
                message = "You have successfully logged in.",
                usrid = uid
            };

            return Ok(response);
        }
    }
}
