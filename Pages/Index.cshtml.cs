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
                try
                {
                    List<Review> reviews = await _context.FoodsReview.ToListAsync();



                    Review = reviews.OrderBy(x => x.RecordTime).ToList();
                }
                catch
                {
                    Review = await _context.FoodsReview.ToListAsync();
                }
            }
        }
    }
}
