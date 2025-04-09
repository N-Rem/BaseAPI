using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProjectServices
    {
        Task<List<ProjectDTO>> GetAllAsync();
        Task<ProjectDTO> GetByIdAsync(int id);
        Task<ProjectDTO> CreateAsync(ProjectCreateRequestDTO request);
        Task UpdateAsync(ProjectUpdateRequestDTO request, int id);
        Task DeleteAsync(int id);
        Task LogicalDeleteAsync(int id);
    }
}
