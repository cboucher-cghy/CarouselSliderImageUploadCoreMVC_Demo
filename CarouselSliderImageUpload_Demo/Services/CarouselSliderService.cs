using CarouselSliderImageUpload_Demo.Data;
using CarouselSliderImageUpload_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace CarouselSliderImageUpload_Demo.Services
{
    public class CarouselSliderService
    {
        private readonly ApplicationDbContext dbContext;

        public CarouselSliderService(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<List<CarouselSlider>> GetAllAsync()
        {
            return await dbContext.CarouselSliders.ToListAsync();
        }

        public async Task<CarouselSlider?> GetByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await dbContext.CarouselSliders.FindAsync(id);
        }

        public async Task<CarouselSlider> CreateAsync(CarouselSlider carouselSlider)
        {
            dbContext.Add(carouselSlider);
            await dbContext.SaveChangesAsync();
            return carouselSlider;
        }

        public async Task<CarouselSlider> UpdateAsync(CarouselSlider carouselSlider)
        {
            dbContext.Update(carouselSlider);
            await dbContext.SaveChangesAsync();
            return carouselSlider;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var carouselSlider = await dbContext.CarouselSliders.FindAsync(id);
            if (carouselSlider == null)
            {
                return false;
            }

            dbContext.CarouselSliders.Remove(carouselSlider);
            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
