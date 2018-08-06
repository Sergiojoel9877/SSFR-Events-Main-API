using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SSFR_MainAPI.Data;
using SSFR_MainAPI.Models;

namespace SSFR_MainAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IDBRepository _repository;
        private readonly IConfiguration _configuration;

        public AccountController(IDBRepository repository, IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = repository;
        }   
       
        // POST: api/Account
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserSignUp user)
        {
            if (ModelState.IsValid)
            {
                if (!await _repository.UserRegExits(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    await _repository.RegUser(user);

                    return Ok("Registered");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
        // PUT: api/Account/5
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserSignUp user)
        {
            if (ModelState.IsValid)
            {
                var allUsers = await _repository.GetRegUsers();

                var canLogin = allUsers.Any((u) => u.Password == user.Password && u.Email == user.Email);

                if (canLogin)
                {
                    var claims = new[] {

                        new Claim("email", user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                    };

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                                    _configuration["Jwt:Audience"],
                                                    claims,
                                                    DateTime.UtcNow,
                                                    DateTime.UtcNow.AddMinutes(45),
                                                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"])),
                                                    SecurityAlgorithms.HmacSha256));

                    var userdata = await _repository.FindByEmail(user.Email);

                    if (userdata != null)
                    {
                        //return Ok(userdata);
                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                }
            }
            else
            {
                BadRequest();
            }

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody]UserSignUp user)
        {
            if (ModelState.IsValid)
            {
                var changed = await _repository.UpdateRegUser(user);

                if (changed)
                {
                    return Ok();
                }
            }
            else
            {
                return BadRequest();
            }
            
            return NoContent();
        }
    }
}
