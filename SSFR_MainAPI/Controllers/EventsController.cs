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
using Microsoft.Azure.Mobile.Server;

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

            //Get Settings for the server project
            HttpConfiguration config = this.conf;
            MobileAppSettingsDictionary settings = this.conf.GetMobileAppSettingsProvider().GetMobileAppSettings();

            //Get the notification hubs credentials for the mobile app
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings.Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;

            //Create a new Notification Hub Client
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnection, notificationHubName);

            //Send the message so that all template registrations that contain "messageParam" receive the notifications. This includes APNS, GCM, WNS, and MPNS
            //Templete registrations
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = "¡El evento " + @event.Name + " Ha sido creado satisfactoriamente!";

            try
            {
                //Send the push notification and log the results
                var result = await hub.SendTemplateNotificationAsync(templateParams);

                //Write the success result to the logs.
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch (Exception ex)
            {
                //Write the failure to the logs.
                config.Services.GetTraceWriter().Error(ex.Message, null, "Push.SendeAsync Error");
            }


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
