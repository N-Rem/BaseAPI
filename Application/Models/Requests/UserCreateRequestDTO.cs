using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class UserCreateRequestDTO
    {
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "invalid Email Address")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters.")]
        public string Password { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserType Type { get; set; }
    }
}
