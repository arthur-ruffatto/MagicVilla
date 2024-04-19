using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    //TODO: Add Try/Catch handling to all endpoints
    //TODO: Review patch endpoint after automapper

    //[Route("api/VillaAPI")]
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        public ILogger<VillaAPIController> _logger { get; }
        private readonly ApplicationDbContext _db;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            try
            {
                return Ok(await _db.Villas.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error has occured. Msg: {ex.Message}");
            }
        }

        [HttpGet("Get/{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError($"ID = {id}");
                return BadRequest("Invalid ID");
            }

            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            return villa == null ? NotFound("Villa not found") : Ok(villa);
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            if (villaDTO == null)
                return BadRequest(villaDTO);

            if (await _db.Villas.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(villaDTO.Name.ToLower())) != null)
            {
                //return BadRequest("Villa already exists");
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                SqrMt = villaDTO.SqrMt,
                ImageUrl = villaDTO.ImageUrl,
                Details = villaDTO.Details,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                CreatedAt = DateTime.Now,
            };

            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = model.Id}, model);
        }

        [HttpDelete("Delete/{id}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest("Invalid ID");
            
            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
                return NotFound("Villa not found");

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return Ok("Deleted");
        }

        [HttpPut("Put/{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null)
                return BadRequest("Villa is null");

            if (id == 0 || id != villaDTO.Id)
                return BadRequest("Invalid ID");

            //var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            //if (villa == null)
            //    return NotFound("Villa not found");

            //villa.Name = villaDTO.Name;
            //villa.SqrMt = villaDTO.SqrMt;
            //villa.Occupancy = villaDTO.Occupancy;

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Name = villaDTO.Name,
                SqrMt = villaDTO.SqrMt,
                ImageUrl = villaDTO.ImageUrl,
                Details = villaDTO.Details,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                UpdatedAt = DateTime.Now,
            };

            _db.Villas.Update(model);
            await _db.SaveChangesAsync();

            return Ok("Villa updated");
        }

        [HttpPatch("Patch/{id}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patch)
        {
            if (patch == null)
                return BadRequest("Document is null");

            if (id == 0)
                return BadRequest("Invalid ID");

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); //AsNoTracking makes EF Core to not track the retrieved object

            if (villa == null)
                return NotFound("Villa not found");

            VillaUpdateDTO villaDTO = new()
            {
                Id = id,
                Name = villa.Name,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                SqrMt = villa.SqrMt
            };

            patch.ApplyTo(villaDTO, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Id = villaDTO.Id,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                SqrMt = villa.SqrMt,
                UpdatedAt = DateTime.Now
            };

            _db.Villas.Update(model);
            await _db.SaveChangesAsync();

            return Ok("Villa updated");
        }
    }
}
