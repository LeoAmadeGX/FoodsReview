using FoodsReview.Models;
using FoodsReview.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodsReview.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly FoodsReviewDbContext _context;

        public DeleteModel(FoodsReviewDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.FoodsReview == null)
            {
                return NotFound();
            }
            var review = await _context.FoodsReview.FindAsync(id);

            if (review != null)
            {
                Review = review;
                _context.FoodsReview.Remove(Review);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
