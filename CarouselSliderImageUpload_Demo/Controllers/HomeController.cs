using CarouselSliderImageUpload_Demo.Models;
using CarouselSliderImageUpload_Demo.Services;
using CarouselSliderImageUpload_Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarouselSliderImageUpload_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly CarouselSliderService carouselSliderService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(CarouselSliderService service, IWebHostEnvironment hostEnvironment)
        {
            carouselSliderService = service;
            webHostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var carouselSlider = await carouselSliderService.GetAllAsync();
            return View(carouselSlider);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var carouselSlider = await carouselSliderService.GetByIdAsync(id);

            if (carouselSlider == null)
            {
                return NotFound();
            }

            var carouselViewModel = new CarouselSliderViewModel()
            {
                Id = carouselSlider.Id,
                ImageName = carouselSlider.ImageName,
                Description = carouselSlider.Description,
                ExistingImage = carouselSlider.ImagePath
            };

            return View(carouselViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HomeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                CarouselSlider carouselSlider = new()
                {
                    ImageName = model.ImageName,
                    ImagePath = uniqueFileName,
                    Description = model.Description
                };
                await carouselSliderService.CreateAsync(carouselSlider);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var carouselSlider = await carouselSliderService.GetByIdAsync(id);

            if (carouselSlider == null)
            {
                return NotFound();
            }

            var carouselViewModel = new CarouselSliderViewModel()
            {
                Id = carouselSlider.Id,
                ImageName = carouselSlider.ImageName,
                Description = carouselSlider.Description,
                ExistingImage = carouselSlider.ImagePath
            };

            return View(carouselViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarouselSliderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var carouselSlider = await carouselSliderService.GetByIdAsync(model.Id);
                carouselSlider!.Description = model.Description;
                carouselSlider.ImageName = model.ImageName;

                if (model.Image != null)
                {
                    if (model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, "images", model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    carouselSlider.ImagePath = ProcessUploadedFile(model);
                }
                await carouselSliderService.UpdateAsync(carouselSlider);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var carouselSlider = await carouselSliderService.GetByIdAsync(id);

            if (carouselSlider == null)
            {
                return NotFound();
            }

            var carouselViewModel = new CarouselSliderViewModel()
            {
                Id = carouselSlider.Id,
                ImageName = carouselSlider.ImageName,
                Description = carouselSlider.Description,
                ExistingImage = carouselSlider.ImagePath
            };

            return View(carouselViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carouselSlider = await carouselSliderService.GetByIdAsync(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", carouselSlider!.ImagePath);

            if (await carouselSliderService.DeleteAsync(id))
            {
                if (System.IO.File.Exists(CurrentImage))
                {
                    System.IO.File.Delete(CurrentImage);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private string ProcessUploadedFile(UploadImage model)
        {
            string uniqueFileName = "";

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.CreateVersion7().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Image.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
