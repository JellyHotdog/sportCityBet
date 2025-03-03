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
                slots[0] *= slots[1];
            }
            else if (slots[1] == slots[2] || slots[2] == slots[3])
            {
                slots[0]/=2;
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
            int[] result = calcPoints(bet.Amount);
            int payout = result[0];

            // Build the response with the slot results and the payout
            var response = new
            {
                BetAmount = bet.Amount,
                Payout = payout,
                Message = payout > 0 ? "You win!" : "You lose!",
                SlotResult = $"{result[1]}-{result[2]}-{result[3]}"
            };

            return Ok(response);
        }
    }
}
