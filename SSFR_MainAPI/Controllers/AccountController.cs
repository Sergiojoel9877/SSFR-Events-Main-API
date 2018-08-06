using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSFR_MainAPI.Data;
using SSFR_MainAPI.Models;

namespace SSFR_MainAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IDBRepository _repository;

        public AccountController(IDBRepository repository)
        {
            _repository = repository;
        }
        //// GET: api/Account
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Account/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        
        // POST: api/Account
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                if (_repository.UserSignUp)
                {

                }
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        // PUT: api/Account/5
        [HttpPost]
        public void Login(int id, [FromBody]string value)
        {
            if (ModelState.IsValid)
            {

            }
            else
            {
                BadRequest();
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
