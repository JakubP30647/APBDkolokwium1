using APBDkolokwium1.Exceptions;
using APBDkolokwium1.Models;
using APBDkolokwium1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBDkolokwium1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AController : ControllerBase
    {
        
        private readonly IAServices _iAServices;
        
        public AController(IAServices iAServices)
        {
            _iAServices = iAServices;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetA(string id)
        {
            var aDTO = await _iAServices.getA(id);

            if (aDTO == null)
            {
                return NotFound("not found");
            }

            return Ok(aDTO);
        }
        
        [HttpPost]
        public async Task<IActionResult> postADTO([FromBody] postADTO postAdto)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            
            try
            {
                await _iAServices.addNewA(postAdto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }


            return Created("", "utworzone");
        }
        
        
        
        
        
        
    }
}
