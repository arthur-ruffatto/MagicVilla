using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.DTO
{
    public class VillaNumberUpdateDTO
    {
        [Required]
        public int VillaNumberId { get; set; }
        public string? SpecialDetails { get; set; }
    }
}
