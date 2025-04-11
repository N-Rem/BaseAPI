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

        private string _password;
        public string Password
        {
            get => string.IsNullOrEmpty(_password)
                ? Environment.GetEnvironmentVariable("MAIL_PASSWORD")
                : _password;
            set => _password = value;
        }
    }
}
