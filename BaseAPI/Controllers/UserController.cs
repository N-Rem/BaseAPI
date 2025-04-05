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
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }


        [HttpGet("[Action]")]
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
        public async Task<IActionResult> CreateClient([FromBody] UserCreateRequestDTO request)
        {
            try
            {
                var newUserDTO = await _userServices.CreateClientAsync(request);
                return Ok(newUserDTO);
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
        public async Task<IActionResult> CreateEmployee([FromBody] UserCreateRequestDTO request)
        {
            try
            {
                var newUserDTO = await _userServices.CreateEmployeeAsync(request);
                return Ok(newUserDTO);
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
        public async Task<IActionResult> CreateAdmin([FromBody] UserCreateRequestDTO request)
        {
            try
            {
                var newUserDTO = await _userServices.CreateAdminAsync(request);
                return Ok(newUserDTO);
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




    }
}
