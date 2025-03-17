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


            //database logic
            dbConfig db = new dbConfig();

            using (db.dbConn)
            {
                db.dbConn.Open();

                string sql = "insert into login values(@email,@uname,@password);insert into users (fname,sname,email,uname,dob) values(@fname,@sname@email,@uname@dob)" ;

                using (var command = new NpgsqlCommand(sql, db.dbConn))
                {
                    command.Parameters.AddWithValue("email", details.Email);
                    command.Parameters.AddWithValue("uname", details.Username);
                    command.Parameters.AddWithValue("password", details.Password);
                    command.ExecuteNonQuery();
                }
            }

            // Build the response with the slot results and the payout
            var response = new
            {
                message = "You have successfully logged in.",
            };

            return Ok(response);
        }
    }
}
