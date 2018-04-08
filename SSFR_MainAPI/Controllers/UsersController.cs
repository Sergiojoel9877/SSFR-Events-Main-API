﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSFR_MainAPI.Data;
using SSFR_MainAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSFR_MainAPI.Controllers
{
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IDBRepository _repository;

        public UsersController(IDBRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("Users")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _repository.GetUsers();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromBody] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repository.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _repository.AddUser(user);

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateUser(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.UserExist(id))
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
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repository.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            await _repository.DeleteUser(user);

            return Ok(user);
        }

    }
}