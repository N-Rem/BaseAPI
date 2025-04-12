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
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }

        public string Password
        {
            get => Environment.GetEnvironmentVariable("MAIL_PASSWORD") ?? _password;
            set => _password = value;
        }
        private string _password;
    }
}
