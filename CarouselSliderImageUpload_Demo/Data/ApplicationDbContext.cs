using CarouselSliderImageUpload_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace CarouselSliderImageUpload_Demo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarouselSlider> CarouselSliders { get; set; } = default!;
    }
}
