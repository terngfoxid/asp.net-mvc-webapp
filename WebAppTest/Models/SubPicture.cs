using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTest.Models
{
    public class SubPicture
    {
        public int? ID { get; set; }

        public string? Path { get; set; }

        public int? idBanner { get; set; }

    }
}
