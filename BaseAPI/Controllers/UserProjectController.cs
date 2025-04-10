﻿using Application.Interfaces;
using Application.Models.Requests;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserProjectController : ControllerBase
    {
        private readonly IUserProjectServices _userProjectServices;
        public UserProjectController(IUserProjectServices userProjectServices)
        {
            _userProjectServices = userProjectServices;
        }



        [HttpGet("[Action]")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userProjects = await _userProjectServices.GetAllAsync();
                return Ok(userProjects);
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

        [HttpGet("[Action]/{id}")]

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var userProject = await _userProjectServices.GetByIdAsync(id);
                return Ok(userProject);
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

        [HttpPost("[Action]")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create([FromBody] UserProjectCreateRequestDTO request)
        {
            try
            {
                var userProjectDTO = await _userProjectServices.CreateAsync(request);
                return Ok(userProjectDTO);
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

        [HttpPut("[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UserProjectUpdateRequestDTO reques, [FromRoute] int id)
        {
            try
            {
                await _userProjectServices.UpdateAsync(reques, id);
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

        [HttpDelete("[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _userProjectServices.DeleteAsync(id);
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
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> LogicalDelete([FromRoute] int id)
        {
            try
            {
                await _userProjectServices.LogicalDeleteAsync(id);
                return Ok("UserProject logically deleted.");
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


    }
}
