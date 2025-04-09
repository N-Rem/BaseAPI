using Application.Models.Requests;
using Application.Models;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Enums;

namespace Application.Services
{
    public class ToolServices : IToolServices
    {
        private readonly IToolRepository _toolRepository;
        public ToolServices (IToolRepository toolRepository)
        {
            _toolRepository = toolRepository;
        }

        public async Task<List<ToolDTO>> GetAllAsync()
        {
            try
            {
                var listTool = await _toolRepository.GetAllAsync()
                    ?? throw new NotFoundException("Tool not Found");

                return ToolDTO.CreateList(listTool);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Tool not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task<ToolDTO> GetByIdAsync(int id)
        {
            var tool = await _toolRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Tool not Found");
            var toolDTO = ToolDTO.Create(tool);
            return toolDTO;
        }

        public async Task<ToolDTO> CreateAsync(ToolCreateRequestDTO request)
        {
            try
            {

                var newTool = new Tool();
                newTool.Name = request.Name;
                newTool.Description = request.Description;
                newTool.Status = Domain.Enums.Status.Active;

                var tool = await _toolRepository.AddAsync(newTool);
                return ToolDTO.Create(tool);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Project not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task UpdateAsync(ToolUpdateRequestDTO request, int id)
        {
            try
            {
                var tool = await FoundToolIdAsync(id);
                tool.Name = request.Name;
                tool.Description = request.Description;
                tool.UserTool = request.UserTool;
                tool.Status = request.Status;

                await _toolRepository.UpdateAsync(tool);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Tool not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var project = await FoundToolIdAsync(id);
                await _toolRepository.DeleteAsync(project);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Tool Not Found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }


        public async Task LogicalDeleteAsync(int id)
        {
            try
            {
                var obj = await FoundToolIdAsync(id);
                obj.Status = Status.Inactive;

                await _toolRepository.UpdateAsync(obj);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Tool Not Found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }


        private async Task<Tool> FoundToolIdAsync(int id)
        {
            var tool = await _toolRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Not Found Tool Id");
            return tool;
        }
    }
}
