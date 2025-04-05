using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "invalid Email Address")]
        public string Email { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserType Type { get; set; }

        //si esta daddo de baja o no/ activo o inactivo
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.Active;

 
        public static UserDTO Create(User u)
        {
            var dto = new UserDTO();
            dto.Id = u.Id;
            dto.Name = u.Name;
            dto.Email = u.Email;
            dto.Type = u.Type;
            dto.Status = u.Status;

            return dto;
        }

        public static List<UserDTO?> CreateList(IEnumerable<User> users)
        {
            List<UserDTO?> listDto = [];

            foreach (var u in users)
            {
                listDto.Add(Create(u));
            }

            return listDto;
        }

    }

}
