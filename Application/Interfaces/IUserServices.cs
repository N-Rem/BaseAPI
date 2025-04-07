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
         Task<UserDTO> CreateUserAsync(UserCreateRequestDTO request);
        Task UpdateAsync(UserUpdateRequestDTO request, int id);
        Task RequestPassChangeAsync(PassRecoveryRequestDTO request);
        Task UpdatePassAsync(PassResetRequestDTO request);
        Task LogicalDeleteAsync(int id);
        Task DeleteAsync(int id);

    }
}
