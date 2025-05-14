using APBDkolokwium1.Exceptions;
using APBDkolokwium1.Models;
using APBDkolokwium1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBDkolokwium1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        
        private readonly IDeliveryServices _iDeliveryServices;
        
        public DeliveryController(IDeliveryServices iDeliveryServices)
        {
            _iDeliveryServices = iDeliveryServices;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetA(string id)
        {
            var aDTO = await _iDeliveryServices.getA(id);

            if (aDTO == null)
            {
                return NotFound("not found");
            }

            return Ok(aDTO);
        }
        
        [HttpPost]
        public async Task<IActionResult> postADTO([FromBody] postDeliveryDTO postDeliveryDto)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            
            try
            {
                await _iDeliveryServices.addNewA(postDeliveryDto);
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
