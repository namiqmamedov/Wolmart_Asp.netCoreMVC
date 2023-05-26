using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.Models
{
    public class Setting
    {
        public int ID { get; set; }

        [Required,StringLength(255)]
        public string Key { get; set; }
        [Required, StringLength(255)]
        public string Value { get; set; }
    }
}
