using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

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
                return bet * slots[1];
            }
            else if (slots[1] == slots[2] || slots[2] == slots[3])
            {
                return bet / 2;
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
            int payout = calcPoints(bet.Amount);

            // Build the response with the slot results and the payout
            var response = new
            {
                BetAmount = bet.Amount,
                Payout = payout,
                Message = payout > 0 ? "You win!" : "You lose!",
                SlotResult = $""
            };

            return Ok(response);
        }
    }
}
