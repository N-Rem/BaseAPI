using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            try
            {
                var listUser = await _userRepository.GetAllAsync()
                    ?? throw new NotFoundException("Users not Found");

                return UserDTO.CreateList(listUser);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("User not Found");
            var userDTO = UserDTO.Create(user);
            return userDTO;
        }

        public async Task<UserDTO> CreateAdminAsync(UserCreateRequestDTO request)
        {
            try
            {
                await MailExistsAsync(request.Email);

                var newAdmin = new User();
                newAdmin.Name = request.Name;
                newAdmin.Email = request.Email;
                newAdmin.Type = Domain.Enums.UserType.Admin;
                newAdmin.Status = Domain.Enums.Status.Active;
                newAdmin.Password = request.Password;

                var admin = await _userRepository.AddAsync(newAdmin);
                return UserDTO.Create(admin);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }
        public async Task<UserDTO> CreateClientAsync(UserCreateRequestDTO request)
        {
            try
            {
                await MailExistsAsync(request.Email);

                var newClient = new User();
                newClient.Name = request.Name;
                newClient.Email = request.Email;
                newClient.Type = Domain.Enums.UserType.Client;
                newClient.Status = Domain.Enums.Status.Active;
                newClient.Password = request.Password;

                var client = await _userRepository.AddAsync(newClient);
                return UserDTO.Create(client);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }
        public async Task<UserDTO> CreateEmployeeAsync(UserCreateRequestDTO request)
        {
            try
            {
                await MailExistsAsync(request.Email);

                var newEmployee = new User();
                newEmployee.Name = request.Name;
                newEmployee.Email = request.Email;
                newEmployee.Type = Domain.Enums.UserType.Employee;
                newEmployee.Status = Domain.Enums.Status.Active;
                newEmployee.Password = request.Password;

                var employee = await _userRepository.AddAsync(newEmployee);
                return UserDTO.Create(employee);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User not found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }



        public async Task UpdateAsync(UserUpdateRequestDTO request, int id)
        {
            try
            {
                var user = await FoundUserIdAsync(id);
                user.Name = request.Name;
                user.Email = request.Email;
                user.Password = request.Password;
                user.Status = request.Status;
                user.Type = request.Type;

                await _userRepository.UpdateAsync(user);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User Not Found", ex);
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
                var user = await FoundUserIdAsync(id);
                await _userRepository.DeleteAsync(user);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User Not Found", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }



        private async Task<User> FoundUserIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Not Found User Id");
            return user;
        }
        private async Task MailExistsAsync(String email)
        {
            var exist = await _userRepository.GetByEmailAsync(email);
            if (exist != null) throw new Exception("Email already exists");
        }

    }

}
