using FoodsReview.Models;
using FoodsReview.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodsReview.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly FoodsReviewDbContext _context;

        public DetailsModel(FoodsReviewDbContext context)
        {
            _context = context;
        }

        public Review Review { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FoodsReview == null)
            {
                return NotFound();
            }

            var review = await _context.FoodsReview.FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            else
            {
                Review = review;
            }
            return Page();
        }
    }
}
