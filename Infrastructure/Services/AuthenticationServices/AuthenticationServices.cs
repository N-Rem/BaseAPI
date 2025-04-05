using Application.Models.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.AuthenticationServices
{
    public class AuthenticationServices
    {
        private readonly AuthenticationServiceOptions _options;
        private readonly IUserRepository _userRepository;
        public AuthenticationServices (AuthenticationServiceOptions options, IUserRepository userRepository)
        {
            _options = options;
            _userRepository = userRepository;
        }

        //Retorna un usuario valido
        public async Task<User?> ValidUser(AuthenticationRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) { return null; }

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) return null;

            if (user.Type == UserType.Admin || user.Type == UserType.Employee || user.Type == UserType.Owner)
            {
                if (user.Password == request.Password) { return user; }
            }
            return null;
        }





    }
}
