using Microsoft.EntityFrameworkCore;

namespace FoodsReview.Models.DB
{
    public class FoodsReviewDbContext : DbContext
    {
        public FoodsReviewDbContext(DbContextOptions<FoodsReviewDbContext> options)
            : base(options)
        {
        }

        public DbSet<Review> FoodsReview { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=FoodsReviewDB.db");
    }
}
