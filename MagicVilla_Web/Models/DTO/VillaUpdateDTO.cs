using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.DTO
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(30, ErrorMessage = "Name should be less than 30 characters")]
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int SqrMt { get; set; }
        public int Occupancy { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
