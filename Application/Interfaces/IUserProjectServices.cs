using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserProjectServices
    {
        Task<List<UserProjectDTO>> GetAllAsync();
        Task<UserProjectDTO> GetByIdAsync(int id);
        Task<UserProjectDTO> CreateAsync(UserProjectCreateRequestDTO request);
        Task UpdateAsync(UserProjectUpdateRequestDTO request, int id);
        Task DeleteAsync(int id);

    }
}
