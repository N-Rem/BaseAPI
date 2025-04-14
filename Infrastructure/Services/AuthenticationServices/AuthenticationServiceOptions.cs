using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.AuthenticationServices
{
    //AuthenticationServiceOptions = clase que representa la configuración.
    //Agarra lo que hay en appsettings.json.
    //Permite no hardcodear valores sensibles (como el SecretForKey) y mantenerlo todo configurable y limpio.
    public class AuthenticationServiceOptions
    {
        public const string AuthenticationService = "AuthenticationService";
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretForKey { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        //private string _SecretForKey;
        //public string SecretForKey
        //{
        //    get => string.IsNullOrEmpty(_SecretForKey)
        //        ? Environment.GetEnvironmentVariable("JWT_SECRET")
        //        : _SecretForKey;
        //    set => _SecretForKey = value;
        //}
    }
}
