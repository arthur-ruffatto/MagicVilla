using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTO
{
    public class VillaCreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30, ErrorMessage = "Name should be less than 30 characters")]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int SqrMt { get; set; }
        [Required]
        public int Occupancy { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public string Amenity { get; set; }
    }
}
