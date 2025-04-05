using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserServices
    {
         Task<List<UserDTO>> GetAllAsync();
         Task<UserDTO> GetByIdAsync(int id);
         Task<UserDTO> CreateAdminAsync(UserCreateRequestDTO request);
         Task<UserDTO> CreateClientAsync(UserCreateRequestDTO request);
         Task<UserDTO> CreateEmployeeAsync(UserCreateRequestDTO request);
        Task UpdateAsync(UserUpdateRequestDTO request, int id);
        Task DeleteAsync(int id);

    }
}
