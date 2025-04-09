using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IToolServices
    {
        Task<List<ToolDTO>> GetAllAsync();
        Task<ToolDTO> GetByIdAsync(int id);
        Task<ToolDTO> CreateAsync(ToolCreateRequestDTO request);
        Task UpdateAsync(ToolUpdateRequestDTO request, int id);
        Task DeleteAsync(int id);
        Task LogicalDeleteAsync(int id);

    }
}
