using Application.Interfaces;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.AuthenticationServices
{
    public class AuthenticationServices : IAuthenticationService
    {
        private readonly AuthenticationServiceOptions _options;
        private readonly IUserRepository _userRepository;

        //cuando se quiere inyectar una configuración para el appsettings.json o proveedor similar se hace a través de la interfaz IOptions<T>.
        //IOptions<T> le dice al framework esto es una configuración, gestionala.."
        public AuthenticationServices (IOptions<AuthenticationServiceOptions> options, IUserRepository userRepository)
        {
            _options = options.Value;
            _userRepository = userRepository;
        }

        //Retorna un usuario valido
        public async Task<User?> ValidateUser(AuthenticationRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) { return null; }

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) return null;

            if (user.Type == UserType.Admin || user.Type == UserType.Employee || user.Type == UserType.Client)
            {
                if (user.Password == request.Password) { return user; }
            }
            return null;
        }

        //retoran el JWT
        public async Task<string> Authenticate (AuthenticationRequestDTO request)
        {
            var user = await ValidateUser(request) 
                ?? throw new NotFoundException("NotFound User");

            //El secreto que se guarda en una varable codificado
            var sercurityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey));

            //El secreto de hashea = transforma el dato en una cadena de caracteres fija mediante una función:
            var credentials = new SigningCredentials(sercurityPassword, SecurityAlgorithms.HmacSha256);

            //Se crean los distintos Claims "datos no sencibles"
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("id", user.Id.ToString()));
            claimsForToken.Add(new Claim("role", user.Type.ToString()));
            claimsForToken.Add(new Claim("given_name", user.Name.ToString()));
            claimsForToken.Add(new Claim("email", user.Email.ToString()));
            claimsForToken.Add(new Claim("status", user.Status.ToString()));

            //si se quiere poner claims para roles espesificas: 
            //if(user.Type == UserType.Admin) 
            //{ }

            //creamos el jwt:
            var jwtToken = new JwtSecurityToken(
                _options.Issuer, //quien lo creo
                _options.Audience, //a quien va dirigido
                claimsForToken, //los calims
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(2), //tiempo de vida del token
                credentials); //el secreto hasheado


            ///Creamos el token se seguridad con el jwt!
            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtToken);

            return tokenToReturn.ToString();
        }
    }
}
