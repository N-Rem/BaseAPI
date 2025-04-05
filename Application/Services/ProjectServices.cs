using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProjectServices : IProjectServices
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectServices(IProjectRepository projectRepository) {  _projectRepository = projectRepository; }


        public async Task<List<ProjectDTO>> GetAllAsync()
        {
            try
            {
                var listProject = await _projectRepository.GetAllAsync()
                    ?? throw new NotFoundException("Project not Found");

                return ProjectDTO.CreateList(listProject);
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

        public async Task<ProjectDTO> GetByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Project not Found");
            var projectDTO = ProjectDTO.Create(project);
            return projectDTO;
        }

        public async Task<ProjectDTO> CreateAsync(ProjectCreateRequestDTO request)
        {
            try
            {

                var newProject = new Project();
                newProject.Name = request.Name;
                newProject.Description = request.Description;
                newProject.Status = Domain.Enums.Status.Active;

                var project = await _projectRepository.AddAsync(newProject);
                return ProjectDTO.Create(project);
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

        public async Task UpdateAsync(ProjectUpdateRequestDTO request, int id)
        {
            try
            {
                var project = await FoundProjectIdAsync(id);
                project.Name = request.Name;
                project.Description = request.Description;
                project.Status = request.Status;

                await _projectRepository.UpdateAsync(project);
            }
            catch (NotFoundException ex) {
                throw new NotFoundException("Project not found", ex);
            }
            catch(Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var project = await FoundProjectIdAsync(id);
                await _projectRepository.DeleteAsync(project);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Project Not Found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }



        private async Task<Project> FoundProjectIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Not Found Project Id");
            return project;
        }

    }
}
