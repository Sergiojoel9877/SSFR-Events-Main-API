using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSFR_MainAPI.Data;
using SSFR_MainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSFR_MainAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Guests")]
    public class GuestsController : Controller
    {
        private readonly IDBRepository _repository;

        public GuestsController(IDBRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("Guests")]
        public async Task<IEnumerable<Guest>> GetGuests()
        {
            return await _repository.GetGuests();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var guest = await _repository.GetGuest(id);

            if (guest == null)
            {
                return NotFound();
            }

            return Ok(guest);
        }

        [HttpPost]
        [Route("AddGuest")]
        public async Task<IActionResult> PostGuest([FromBody] Guest guest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _repository.AddGuest(guest);

            return Ok(data);
        }

        [HttpPut("{id}")]
        [Route("PutGuest")]
        public async Task<IActionResult> PutGuest([FromRoute] int id, [FromBody] Guest guest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != guest.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateGuest(guest);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.GuestExits(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Route("DeleteGuest")]
        public async Task<IActionResult> DeleteGuest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var guest = await _repository.GetGuest(id);

            if (guest == null)
            {
                return NotFound();
            }

            await _repository.DeleteGuest(guest);

            return Ok(guest);
        }

    }
}
