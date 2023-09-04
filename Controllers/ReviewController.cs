using FoodsReview.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace FoodsReview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly FoodsReviewDbContext _context;

        public ReviewController(FoodsReviewDbContext context)
        {
            _context = context;
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateReview(int id, string convenient, string foodName, string recorder, string memo)
        {
            var review = await _context.FoodsReview.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            review.Convenient = convenient;
            review.FoodName = foodName;
            review.Recorder = recorder;
            review.Memo = memo;
            review.RecordTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(); // 或其他適當的回應
        }
    }
}
