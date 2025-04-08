using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.EmailServises
{
    /// <summary>
    /// apibasecodelens@gmail.com
    /// apibase1234
    /// -
    ///para esto se necesita un mail
    ///Luego se selecciona Manage your Google Account en la pagina principal de gmail
    /// >Contraseñas de aplicaciones> apiBase = wfda ejgv spfe uvzi 
    /// buscamos "App passwords" para crear la key que necesitamos: tikx jizl tpyu bbwe
    ///Con ese codigo mailkit puede mandar mails desde nuestro email.
    ///-
    ///En el appsetting se agrega la seccion:   "MailSettings": {...}
    ///Esta config se usa para poder leer lo escrito en el appsetting
    /////El nombre "MailService" no puede ser utilizado porque ya existe en la libreria "mailkit"
    /// </summary>
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _config;
        private readonly EmailSettingsOptions _options;
        private readonly IUserRepository _userRepository;   

        public EmailServices(IOptions<EmailSettingsOptions> options, IConfiguration config, IUserRepository userRepository) 
        {
            _options = options.Value;
            _config = config;
            _userRepository = userRepository;
        }

        public void SendMail (string header, string msj, string emailUser)
        {
            //Se crea todo el mensaje para enviar.
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_options.UserName));

            email.To.Add(MailboxAddress.Parse(emailUser));
            email.Subject = header;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
            Text = msj
            };

            //Se intenta mandar el correo .
            try
            {
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_options.UserName, _options.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (NoMailSentException ex)
            {
                throw new NoMailSentException("Could not Send Confirmation email", ex);
            }
        }



    }
}
