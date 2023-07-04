using CardManagementApi.Models;
using CardManagementApi.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CardManagementApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Card> CreateCard([FromBody] string number)
        {
            // Validate the card format
            if (number.Length != 15 || !number.All(char.IsDigit))
            {
                return BadRequest("Invalid card format.");
            }

            // Create a new card
            var card = new Card { Number = number, Balance = 0 };
            _context.Cards.Add(card);
            _context.SaveChanges();

            return Ok(card);
        }

        [HttpPost("{cardId}/pay")]
        [Authorize]
        public ActionResult Pay(int cardId, [FromBody] decimal amount)
        {
            // Retrieve the card from the database
            var card = _context.Cards.Find(cardId);
            if (card == null)
            {
                return NotFound("Card not found.");
            }

            // Calculate the payment fee
            var fee = PaymentFeeService.Instance.GetCurrentFee();
            var totalAmount = amount + (amount * fee);

            // Check if the card has enough balance
            if (card.Balance < totalAmount)
            {
                return BadRequest("Insufficient balance.");
            }

            // Create a new payment
            var payment = new Payment
            {
                CardId = cardId,
                Amount = amount,
                PaymentDate = DateTime.UtcNow,
                Fee = fee
            };

            // Update the card balance
            card.Balance -= totalAmount;

            _context.Payments.Add(payment);
            _context.SaveChanges();

            return Ok(payment);
        }

        [HttpGet("{cardId}/balance")]
        [Authorize]
        public ActionResult<decimal> GetCardBalance(int cardId)
        {
            // Retrieve the card from the database
            var card = _context.Cards.Find(cardId);
            if (card == null)
            {
                return NotFound("Card not found.");
            }

            return Ok(card.Balance);
        }
    }
}
