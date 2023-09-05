using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTest.Models
{
    public class Banner
    {
        public int ID { get; set; }


        [Required(ErrorMessage="Title is required")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? Path { get; set; }

        [NotMapped]
        public SubPicture[]? SubPictures { get; set; }
    }
}
