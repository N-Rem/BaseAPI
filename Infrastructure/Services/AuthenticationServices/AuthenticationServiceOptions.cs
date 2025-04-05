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
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretForKey { get; set; }
    }
}
