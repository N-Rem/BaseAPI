using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EmailServises
{
    public class EmailSettingsOptions
    {
        public const string EmailService = "MailSettings";
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
