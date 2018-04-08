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
    [Route("api/Events")]
    public class EventsController : Controller
    {
        private readonly IDBRepository _repository;

        public EventsController(IDBRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("Events")]
        public async Task<IEnumerable<Events>> GetEvents()
        {
            return await _repository.GetEvents();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _repository.GetEvent(id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }
        
        [HttpPost]
        [Route("AddEvent")]
        public async Task<IActionResult> PostEvent([FromBody] Events @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _repository.AddEvent(@event);

            return Ok(data);
        }

        [HttpPut("{id}")]
        [Route("PutEvent")]
        public async Task<IActionResult> PutEvent([FromRoute] int id, [FromBody] Events @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateEvent(@event);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.EventExists(id))
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
        [Route("DeleteEvent")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _repository.GetEvent(id);

            if (@event == null)
            {
                return NotFound();
            }

            await _repository.DeleteEvent(@event);

            return Ok(@event);
        }

    }
}
