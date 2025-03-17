using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1;
using Npgsql;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class slotsController : ControllerBase
    {
        Random random = new Random();

        int[] calcPoints(int bet)
        {

            int[] slots = { bet, random.Next(2, 6), random.Next(2, 6), random.Next(2, 6) };

            if (slots[1] == slots[2] && slots[2] == slots[3])
            {
                slots[0] *= slots[1];
            }
            else if (slots[1] == slots[2] || slots[2] == slots[3])
            {
                slots[0] /= 2;
            }
            else
            {
                slots[0] = 0;
            }
            return slots;
        }


        [HttpPost]
        public IActionResult PlaySlot([FromBody] Bet bet)
        {
            // Validate bet amount
            if (bet == null || bet.Amount <= 0)
            {
                return BadRequest("Please provide a valid bet amount.");
            }

            // Call the slot machine logic to calculate the result
            int[] result = calcPoints(bet.Amount);

            //database logic
            dbConfig db = new dbConfig();

            using (db.dbConn)
            {
                db.dbConn.Open();

                string sql = "update points set balence = balance + @payout where user_id = @user_id";

                using (var command = new NpgsqlCommand(sql, db.dbConn))
                {
                    command.Parameters.AddWithValue("payout", result[0] > 0 ? result[0] : result[0] - bet.Amount);
                    command.Parameters.AddWithValue("user_id", bet.UserId);
                    command.ExecuteNonQuery();
                }
            }

            // Build the response with the slot results and the payout
            var response = new
            {
                BetAmount = bet.Amount,
                Payout = result[0],
                Message = result[0] > 0 ? "You win!" : "You lose!",
                SlotResult = $"{result[1]}-{result[2]}-{result[3]}"
            };

            return Ok(response);
        }
    }
}
