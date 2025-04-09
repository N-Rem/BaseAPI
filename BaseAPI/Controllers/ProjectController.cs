using Application.Interfaces;
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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;
        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }


        [HttpGet("[Action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var projects = await _projectServices.GetAllAsync();
                return Ok(projects);
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
        [Authorize(Policy = "ClientEmployeeAdmin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var project = await _projectServices.GetByIdAsync(id);
                return Ok(project);
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
        public async Task<IActionResult> Create([FromBody] ProjectCreateRequestDTO request)
        {
            try
            {
                var projectDTO = await _projectServices.CreateAsync(request);
                return Ok(projectDTO);
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

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Update([FromBody] ProjectUpdateRequestDTO reques, [FromRoute] int id)
        {
            try
            {
                await _projectServices.UpdateAsync(reques, id);
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
                await _projectServices.DeleteAsync(id);
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
                await _projectServices.LogicalDeleteAsync(id);
                return Ok("project logically deleted.");
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
