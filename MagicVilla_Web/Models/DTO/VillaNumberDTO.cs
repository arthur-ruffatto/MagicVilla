﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.DTO
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNumberId { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string? SpecialDetails { get; set; }
    }
}
