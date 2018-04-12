using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSFR_MainAPI.Data;
using SSFR_MainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.Mobile.Server.Config;
using System.Threading.Tasks;
using System.Web.Http;
using System.Configuration;
using Microsoft.Azure.Mobile.Server;
using OneSignalSharp.Posting;
using OneSignalSharp;
using System.Net;
using System.Text;
using System.IO;

namespace SSFR_MainAPI.Controllers
{
    [Produces("application/json")]
    //[Microsoft.AspNetCore.Mvc.Route("api/Events")]
    public class EventsController : Controller
    {
        private readonly IDBRepository _repository;
        HttpConfiguration conf { get; set; } = new HttpConfiguration();

        public EventsController(IDBRepository repository)
        {
            _repository = repository;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/events")]
        public async Task<IEnumerable<Events>> GetEvents()
        {
            return await _repository.GetEvents();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/event/{id}")]
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
        
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/PostEvent")]
		[ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> PostEvent([Microsoft.AspNetCore.Mvc.FromBody] Events @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _repository.AddEvent(@event);

            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic NzU3MDBmYjAtZmY2NS00NjhlLTg0ZDQtMDYxY2ZiNDExYzli");

            byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                    + "\"app_id\": \"23fbe6ba-7814-4714-aa75-00a3480f5b68\","
                                                    + "\"contents\": {\"en\": \"¡Hey, un nuevo evento a sido agregado!, anda buscalo:\"},"
                                                    + "\"included_segments\": [\"All\"]}");

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
            
            return Ok(data);
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        [Microsoft.AspNetCore.Mvc.Route("api/PutEvent/{id}")]
		[ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> PutEvent([FromRoute] int id, [Microsoft.AspNetCore.Mvc.FromBody] Events @event)
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

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("api/DeleteEvent/{id}")]
		[ProducesResponseType(typeof(bool), 200)]
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
