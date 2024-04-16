using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    //[Route("api/VillaAPI")]
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<VillaDTO> GetVillas()
        {
            return VillaStorage.villaList;
        }

        [HttpGet("{id: int}")]
        public VillaDTO GetVilla(int id)
        {
            var villa = VillaStorage.villaList.FirstOrDefault(x => x.Id == id);
            return villa;//?? new VillaDTO();
        }
    }
}
