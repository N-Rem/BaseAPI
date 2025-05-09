﻿using Application.Interfaces;
using Application.Models.Requests;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }


        [HttpGet("[Action]")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userServices.GetAllAsync();
                return Ok(users);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); //404 not found
            }
            catch (Exception ex) //Agarra cualquier error inesperado.
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("[Action]/{id}")]
        [Authorize(Policy = "ClientEmployeeAdmin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var user = await _userServices.GetByIdAsync(id);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); //404 not found
            }
            catch (Exception ex) //Agarra cualquier error inesperado.
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("[Action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] UserCreateRequestDTO request)
        {
            try
            {
                var newUserDTO = await _userServices.CreateUserAsync(request);
                return Ok(newUserDTO);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoMailSentException ex)
            {
                throw new NoMailSentException("Could not Send Confirmation email", ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpPut("[Action]/{id}")]
        [Authorize(Policy = "ClientEmployeeAdmin")]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequestDTO reques, [FromRoute] int id)
        {
            try
            {
                await _userServices.UpdateAsync(reques, id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoMailSentException ex)
            {
                throw new NoMailSentException("Could not Send Confirmation email", ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _userServices.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpPut("[action]/{id}")]
        [Authorize(Policy = "ClientEmployeeAdmin")]
        public async Task<IActionResult> LogicalDelete([FromRoute] int id)
        {
            try
            {
                await _userServices.LogicalDeleteAsync(id);
                return Ok("User logically deleted.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }




        [HttpPut("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPassChange([FromBody] PassRecoveryRequestDTO request)
        {
            try
            {
                await _userServices.RequestPassChangeAsync(request);
                return Ok("Password recovery email sent.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoMailSentException ex)
            {
                throw new NoMailSentException("Could not Send Confirmation email", ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePass([FromBody] PassResetRequestDTO request)
        {
            try
            {
                await _userServices.UpdatePassAsync(request);
                return Ok("Password updated successfully.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RestoreCodeTimeException ex)
            {
                throw new RestoreCodeTimeException("The code has expired.", ex);
            }
            catch (RestoreCodeValidationException ex)
            {
                throw new RestoreCodeValidationException("Invalid recovery code.", ex);
            }
            catch (NoMailSentException ex)
            {
                throw new NoMailSentException("Could not Send Confirmation email", ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        
    }
}
