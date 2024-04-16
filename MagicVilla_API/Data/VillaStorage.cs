using MagicVilla_API.Models.DTO;

namespace MagicVilla_API.Data
{
    public static class VillaStorage
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new() { Id = 1, Name = "Pool View" },
                new() { Id = 2, Name = "Beach View" }
            };
    }
}
