using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    //[Route("api/VillaAPI")]
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStorage.villaList);
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
                return BadRequest("Invalid ID");

            var villa = VillaStorage.villaList.FirstOrDefault(x => x.Id == id);
            return villa == null ? NotFound("Villa not found") : Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null)
                return BadRequest(villaDTO);
            
            if (villaDTO.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError, "Villa ID should be zero.");

            if (VillaStorage.villaList.FirstOrDefault(x => x.Name.Equals(villaDTO.Name, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                //return BadRequest("Villa already exists");
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }
                
            
            villaDTO.Id = VillaStorage.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStorage.villaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id}, villaDTO);
        }

        [HttpDelete("{id}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest("Invalid ID");
            
            var villa = VillaStorage.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
                return NotFound("Villa not found");

            VillaStorage.villaList.Remove(villa);

            return Ok("Deleted");
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if (villaDTO == null)
                return BadRequest("Villa is null");

            if (id == 0 || id != villaDTO.Id)
                return BadRequest("Invalid ID");

            var villa = VillaStorage.villaList.FirstOrDefault(x => x.Id == id);
            
            if (villa == null)
                return NotFound("Villa not found");

            villa.Name = villaDTO.Name;
            villa.Sqrmt = villaDTO.Sqrmt;
            villa.Occupancy = villaDTO.Occupancy;

            return Ok("Villa updated");
        }

        [HttpPatch("{id}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patch)
        {
            if (patch == null)
                return BadRequest("Document is null");

            if (id == 0)
                return BadRequest("Invalid ID");

            var villa = VillaStorage.villaList.FirstOrDefault(x => x.Id == id);

            if (villa == null)
                return NotFound("Villa not found");

            patch.ApplyTo(villa);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Villa updated");
        }
    }
}
