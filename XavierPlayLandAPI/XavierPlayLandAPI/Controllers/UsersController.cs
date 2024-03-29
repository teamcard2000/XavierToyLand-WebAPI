﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.ConstrainedExecution;
using XavierPlayLandAPI.Filters.ActionFilters;
using XavierPlayLandAPI.Filters.ExceptionFilters;
using XavierPlayLandAPI.Filters;

namespace XavierPlayLandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ValidateEntityIdFilter(EntityType.User)]
        public IActionResult GetUser(int id) 
        { 
            var user = _userRepository.GetUserById(id);
            return Ok(user);
        }

        [HttpPost]
        [ValidateAddEntityFilter(EntityType.User)]
        public IActionResult AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userRepository.CreateUser(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ValidateEntityIdFilter(EntityType.User)]
        [ValidateUpdateEntityFilter(EntityType.User)]
        [HandleUpdateExceptionsFilter]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest("User ID mismatch!");
            }

            try
            {
                _userRepository.UpdateUser(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [ValidateEntityIdFilter(EntityType.User)]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user?.Id != id) 
            {
                return BadRequest("User ID mismatch!");
            }

            _userRepository.DeleteUser(id);
            return NoContent();
        }
    }
}
