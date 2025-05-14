using APBDkolokwium1.Exceptions;
using APBDkolokwium1.Models;
using APBDkolokwium1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBDkolokwium1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliverysController : ControllerBase
    {
        
        private readonly IDeliveryServices _iDeliveryServices;
        
        public DeliverysController(IDeliveryServices iDeliveryServices)
        {
            _iDeliveryServices = iDeliveryServices;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDelivery(string id)
        {
            var Delivery = await _iDeliveryServices.getDelivery(id);

            if (Delivery == null)
            {
                return NotFound("not found");
            }

            return Ok(Delivery);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddNewDelivery([FromBody] postDeliveryDTO postDeliveryDto)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _iDeliveryServices.addNewDelivery(postDeliveryDto);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ConflictException exception)
            {
                return Conflict(exception.Message);
            }


            return Created("", "Utworzono");

        }
        
        
        
        
        
        
    }
}
