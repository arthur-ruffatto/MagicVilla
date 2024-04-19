using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    //TODO: Add Try/Catch handling to all endpoints

    //[Route("api/VillaAPI")]
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        public ILogger<VillaAPIController> _logger { get; }
        private readonly IVillaRepository _db;
        private readonly IMapper _mapper;

        public VillaAPIController(ILogger<VillaAPIController> logger, IVillaRepository db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villas = await _db.GetAllAsync();
                return Ok(_mapper.Map<List<VillaDTO>>(villas));
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

            var villa = await _db.GetAsync(x => x.Id == id);
            return villa == null ? NotFound("Villa not found") : Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            if (villaDTO == null)
                return BadRequest(villaDTO);

            if (await _db.GetAsync(x => x.Name.ToLower().Equals(villaDTO.Name.ToLower())) != null)
            {
                //return BadRequest("Villa already exists");
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }

            Villa model = _mapper.Map<Villa>(villaDTO);

            await _db.CreateAsync(model);

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
            
            var villa = await _db.GetAsync(x => x.Id == id);
            if (villa == null)
                return NotFound("Villa not found");

            await _db.RemoveAsync(villa);

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

            Villa model = _mapper.Map<Villa>(villaDTO);

            await _db.UpdateAsync(model);

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

            var villa = await _db.GetAsync(x => x.Id == id, tracked: false); //AsNoTracking makes EF Core to not track the retrieved object

            if (villa == null)
                return NotFound("Villa not found");

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            patch.ApplyTo(villaDTO, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);

            await _db.UpdateAsync(model);

            return Ok("Villa updated");
        }
    }
}
