using Application.Interfaces;
using Application.Models.Requests;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolController : ControllerBase
    {
        private readonly IToolServices _toolServices;
        public ToolController(IToolServices toolServices) 
        { 
        _toolServices = toolServices;
        }



        [HttpGet("[Action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tools = await _toolServices.GetAllAsync();
                return Ok(tools);
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
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var tool = await _toolServices.GetByIdAsync(id);
                return Ok(tool);
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
        public async Task<IActionResult> Create([FromBody] ToolCreateRequestDTO request)
        {
            try
            {
                var toolDTO = await _toolServices.CreateAsync(request);
                return Ok(toolDTO);
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
        public async Task<IActionResult> Update([FromBody] ToolUpdateRequestDTO reques, [FromRoute] int id)
        {
            try
            {
                await _toolServices.UpdateAsync(reques, id);
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
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _toolServices.DeleteAsync(id);
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

    }
}
