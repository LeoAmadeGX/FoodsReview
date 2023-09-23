using FoodsReview.Models;
using FoodsReview.Models.DB;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodsReview.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FoodsReviewDbContext _context;

        public IndexModel(FoodsReviewDbContext context)
        {
            _context = context;
        }

        public IList<Review> Review { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.FoodsReview != null)
            {
                Review = await _context.FoodsReview.OrderBy(x => x.RecordTime).ToListAsync();
            }
        }
    }
}
