using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTO
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30, ErrorMessage = "Name should be less than 30 characters")]
        public string Name { get; set; }
        public int Sqrmt { get; set; }
        public int Occupancy { get; set; }
    }
}
