using MagicVilla_API.Data;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        public VillaNumberAPIController(ApplicationDbContext db)
        {
            
        }
    }
}
