using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IEmailServices _emailServices;
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository, IEmailServices emailServices)
        {
            _emailServices = emailServices;
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

        public async Task<UserDTO> CreateUserAsync(UserCreateRequestDTO request)
        {
            try
            {
                await EnsureEmailNotExistsAsync(request.Email);
                if (!NameValidator(request.Name)) throw new NotValidFormatExeption("Invalid name format.");
                if (!EmailValidator(request.Email)) throw new NotValidFormatExeption("Invalid email format.");
                if (!UserTypeValidator(request.Type.ToString())) throw new NotValidFormatExeption("Invalid user type.");
                if(!PasswordValidator(request.Password)) throw new NotValidFormatExeption("Invalid Password Format");

                var newAdmin = new User();
                newAdmin.Name = request.Name;
                newAdmin.Email = request.Email;
                newAdmin.Type =request.Type;
                newAdmin.Status = Domain.Enums.Status.Active;
                newAdmin.Password = request.Password;

                var admin = await _userRepository.AddAsync(newAdmin);
                WelcomeMsj(admin);
                return UserDTO.Create(admin);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User not found", ex);
            }
            catch (NotValidFormatExeption ex)
            {
                throw new NotValidFormatExeption("Format not Valid", ex);
            }
            catch (NoMailSentException ex)
            {
                throw new NoMailSentException("Could not Send Confirmation email", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurreddd", ex);
            }
        }



        public async Task UpdateAsync(UserUpdateRequestDTO request, int id)
        {
            
            try
            {
                if (!NameValidator(request.Name))  throw new NotValidFormatExeption("Invalid name format.");
                if (!EmailValidator(request.Email)) throw new NotValidFormatExeption("Invalid email format.");
                if (!UserTypeValidator(request.Type.ToString())) throw new NotValidFormatExeption("Invalid user type.");

                var user = await FoundUserIdAsync(id);
                user.Name = request.Name;
                user.Email = request.Email;
                user.Password = request.Password;
                user.Type = request.Type;

                await _userRepository.UpdateAsync(user);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User Not Found", ex);
            }
            catch (NotValidFormatExeption ex)
            {
                throw new NotValidFormatExeption("Format not Valid", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task RequestPassChangeAsync(PassRecoveryRequestDTO request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);

                //crea un codigo, lo guarda en usuario, guarda la fecha y hora actual en USER y lo manda por mail para recuperar contraseña
                string code = GenerateRecoveryCode();
                user.ResetCodeExpiration = DateTime.Now.AddMinutes(25);
                user.PasswordResetCode = code;
                await _userRepository.UpdateAsync(user);

                RecoverMsj(user, code);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User Not Found", ex);
            }
            catch (NoMailSentException ex)
            {
                throw new NoMailSentException("Could not Send Confirmation email", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task UpdatePassAsync(PassResetRequestDTO request)
        {
            try
            {
                if (!PasswordValidator(request.NewPassword))  throw new NotValidFormatExeption("Invalid Password format.");

                var user = await _userRepository.GetByEmailAsync(request.email);

                if (!ValidateExpiryTime(user)) throw new RestoreCodeTimeException("The code has expired.");
                if (!PassResetValidator(user, request.Code))  throw new RestoreCodeValidationException("Invalid recovery code.");
                
                user.Password = request.NewPassword;
                await _userRepository.UpdateAsync(user);
                NewPassMsj(user, user.Password);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("User Not Found", ex);
            }
            catch (NotValidFormatExeption ex)
            {
                throw new NotValidFormatExeption("Format not Valid", ex);
            }
            catch (RestoreCodeTimeException ex)
            {
                throw new RestoreCodeTimeException("The code has expired.", ex);
            }
            catch (RestoreCodeValidationException ex)
            {
                throw new RestoreCodeValidationException("Invalid recovery code.", ex);
            }
            catch (NoMailSentException ex)
            {
                throw new NoMailSentException("Could not Send Confirmation email", ex);
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

        public async Task LogicalDeleteAsync (int id)
        {
            try
            {
                var user = await FoundUserIdAsync(id);
                user.Status = Status.Inactive;

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


        #region Funciones para no repetir codigo. 
        private async Task<User> FoundUserIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Not Found User Id");
            return user;
        }
        private async Task EnsureEmailNotExistsAsync(String email)
        {
            var exist = await _userRepository.GetByEmailAsync(email);
            if (exist != null) throw new Exception("Email already exists");
        }


        private bool NameValidator(String name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            string pattern = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{3,15}$";
            return Regex.IsMatch(name, pattern);
        }
        private bool EmailValidator(String email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }
        private bool PasswordValidator(String pass)
        {
            if (string.IsNullOrEmpty(pass) || pass.Length < 8)
            {
                return false;
            }

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,100}$";
            return Regex.IsMatch(pass, pattern);
        }
        private bool UserTypeValidator(string typeUser)
        {
            return Enum.TryParse<UserType>(typeUser, out _);
        }



        //Validaciones del cambio de contraseña
        private bool PassResetValidator (User user, string code)
        {
            if (user.PasswordResetCode == code)
            {
                return true;
            }
            return false;
        }
        private bool ValidateExpiryTime (User user)
        {
            if ( user.ResetCodeExpiration > DateTime.Now)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Se crea el codigo de recuperacion
        /// </summary>
        /// <returns></returns>
        public string GenerateRecoveryCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var data = new byte[6];
            var codeChars = new char[6];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }

            for (int i = 0; i < 6; i++)
            {
                int index = data[i] % chars.Length;
                codeChars[i] = chars[index];
            }

            return new string(codeChars);
        }


        //Envio de mails 
        private void WelcomeMsj(User user)
        {
            var msj = $@"<h1>API BASE 1!</h1>
                    <h2>Gracias por unirtenos</h2>
                    <p>, {user.Name} como nuestro {user.Type}</p>";
            var headerMsj = "Bienvenido a ApiBase!";
            var emailUser = user.Email;
            _emailServices.SendMail(headerMsj, msj, emailUser);
        }
        private void RecoverMsj(User user, string code)
        {
            var headerMsj = "Recuperacion de Contraseña!";
            var msj = $@"<h1>API BASE 1!</h1>
                    <h2>Recuperacion de contraseña</h2>
                    <p>, {user.Name} Su codigo de recuperacion es: </p>
                    <h1>{code}</h1>";

            var emailUser = user.Email;
            _emailServices.SendMail(headerMsj, msj, emailUser);
        }
        private void NewPassMsj(User user, string pass)
        {
            var headerMsj = "Cambio de Contraseña";
            var msj = $@"<h1>API BASE 1!</h1>
                    <h2>Gracias por unirtenos</h2>
                    <p>{user.Name} Cambio de contraseña {pass}</p>";
            
            var emailUser = user.Email;
            _emailServices.SendMail(headerMsj, msj, emailUser);
        }
        #endregion
    }
}
