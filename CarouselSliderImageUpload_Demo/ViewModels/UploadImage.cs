using System.ComponentModel.DataAnnotations;

namespace CarouselSliderImageUpload_Demo.ViewModels
{
    public class UploadImage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter image name")]
        [Display(Name = "Image Name")]
        [StringLength(100)]
        public string ImageName { get; set; } = default!;

        [Required(ErrorMessage = "Please choose image")]
        [Display(Name = "Upload Image")]
        public IFormFile Image { get; set; } = default!;
    }
}
