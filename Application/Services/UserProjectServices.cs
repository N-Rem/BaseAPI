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
    public class UserProjectServices : IUserProjectServices
    {
        private readonly IUserProjectRepository _userProjectRepository;
        public UserProjectServices(IUserProjectRepository userProjectRepository)
        {
            _userProjectRepository = userProjectRepository;
        }


        public async Task<List<UserProjectDTO>> GetAllAsync()
        {
            try
            {
                var listUserProject = await _userProjectRepository.GetAllAsync()
                    ?? throw new NotFoundException("UserProject not Found");

                return UserProjectDTO.CreateList(listUserProject);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("UserProject not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task<UserProjectDTO> GetByIdAsync(int id)
        {
            var userProject = await _userProjectRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("UserProject not Found");
            var userProjectDTO = UserProjectDTO.Create(userProject);
            return userProjectDTO;
        }

        public async Task<UserProjectDTO> CreateAsync(UserProjectCreateRequestDTO request)
        {
            try
            {
                var newUserProject = new UserProject();
                newUserProject.ProjectId = request.ProjectId;
                newUserProject.UserId = request.UserId;
                newUserProject.Status = Domain.Enums.Status.Active;

                var userProject = await _userProjectRepository.AddAsync(newUserProject);
                return UserProjectDTO.Create(userProject);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("UserProject not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task UpdateAsync(UserProjectUpdateRequestDTO request, int id)
        {
            try
            {
                var userProject = await FoundUserProjectIdAsync(id);
                userProject.ProjectId = request.ProjectId;
                userProject.UserId = request.UserId;
                userProject.Status = request.Status;

                await _userProjectRepository.UpdateAsync(userProject);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("UserProject not found", ex);
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
                var userProject = await FoundUserProjectIdAsync(id);
                await _userProjectRepository.DeleteAsync(userProject);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("UserProject Not Found", ex);
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
                var obj = await FoundUserProjectIdAsync(id);
                obj.Status = Status.Inactive;

                await _userProjectRepository.UpdateAsync(obj);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("UserProject Not Found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }
        private async Task<UserProject> FoundUserProjectIdAsync(int id)
        {
            var userProject = await _userProjectRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Not Found UserProject Id");
            return userProject;
        }

    }
}
