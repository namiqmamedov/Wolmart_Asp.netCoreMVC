using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Models
{
    public class Slider
    {
        public int ID { get; set; }
        [Required,StringLength(1000)]
        public string Image { get; set; }
        [Required, StringLength(1024)]
        public string MainTitle { get; set; }
        [Required, StringLength(1024)]
        public string SubTitle { get; set; }
        [Required, StringLength(2048)]
        public string Description { get; set; }
        [Required, StringLength(1024)]
        public string BannerSubTitle { get; set; }
        [Required, StringLength(1024)]
        public string RedirectURL { get; set; }
    }
}
