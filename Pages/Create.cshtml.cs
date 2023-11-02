using FoodsReview.Models;
using FoodsReview.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodsReview.Pages
{
    public class CreateModel : PageModel
    {
        private readonly FoodsReviewDbContext _context;

        public CreateModel(FoodsReviewDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; } = default!;


        public IList<Review> ReviewList { get; set; } = default!;
        public async Task OnGetAsync()
        {
            if (_context.FoodsReview != null)
            {
                ReviewList = await _context.FoodsReview.ToListAsync();
            }
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.FoodsReview == null || Review == null)
            {
                return Page();
            }
            if (!Review.RecordTime.HasValue)
                Review.RecordTime = DateTime.UtcNow;

            _context.FoodsReview.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
