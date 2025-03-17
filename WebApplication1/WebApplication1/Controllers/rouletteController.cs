using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1;
using Npgsql;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class rouletteController : ControllerBase
    {
        Random random = new Random();

        
        int[] calcPoints(int bet, string? colour, int? betNum)
        {
            int result = random.Next(0, 37);

            if (result == betNum)
            {
                bet *= 35;
            }
            else if (colour == "red" && result % 2 == 0 || colour == "Red" && result % 2 == 0)
            {
                bet *= 2;
            }
            else if (colour == "black" && result % 2 != 0  || colour == "Black" && result % 2 != 0)
            {
                bet *= 2;
            }
            else
            {
                bet = 0;
            }

            int[] values = { bet, result };
            return values;
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
            int[] result = calcPoints(bet.Amount, bet.Colour, bet.Number);

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
                SlotResult = result[1]
            };

            return Ok(response);
        }
    }
}

