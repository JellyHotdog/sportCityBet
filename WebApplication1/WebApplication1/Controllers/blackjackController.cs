using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1;
using Npgsql;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class blackjackController : ControllerBase
    {
        Random random = new Random();

        int[] calcPoints(int bet)
        {
            int card1 = random.Next(2, 12);
            int card2 = random.Next(2, 12);
            int playerTotal = 0;
            int aceCount = 0;
            if (card1 == 11 || card2 == 11)
            {
                aceCount++;
                playerTotal = card1 + card2;
            }
            else if (card1 == 11 && card2 == 11)
            {
                aceCount += 2;
                if (card1 == 11)
                {
                    card1 = 1;
                    playerTotal = card1 + card2;
                }
                else
                {
                    card2 = 1;
                    playerTotal = card1 + card2;
                }
            }
            int[] playerCards = { card1, card2 };

            int dealerAce = 0;
            int dealer1 = random.Next(2, 12);
            int dealer2 = random.Next(2, 12);
            int dealerResult = 0;
            int acePosition;
            if (dealer1 == 11 || dealer2 == 11)
            {
                aceCount++;
                dealerResult = dealer1 + dealer2;
                if (dealer1 == 11)
                {
                    acePosition = 0;
                }
                else
                {
                    acePosition = 1;
                }
            }
            else if(dealer1 == 11 && dealer2 == 11)
            {
                dealerAce += 2;
                if (dealer1 == 11)
                {
                    dealer1 = 1;
                    dealerResult = dealer1 + dealer2;
                    acePosition = 0;
                }
                else
                {
                    dealer2 = 1;
                    dealerResult = dealer1 + dealer2;
                    acePosition = 1;
                }
            }

            int[] dealerCards = { dealer1, dealer2 };
            if (dealerResult == 21)
            {
                if (playerCards[4] is not null)
            }
            else
            {
               while (dealerResult < 17)
               {
                    
               } 
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

