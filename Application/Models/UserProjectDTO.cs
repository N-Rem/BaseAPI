using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserProjectDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.Active;

        public static UserProjectDTO Create(UserProject up)
        {
            var dto = new UserProjectDTO();
            dto.Id = up.Id;
            dto.UserId = up.UserId;
            dto.ProjectId = up.ProjectId;
            dto.Status = up.Status;

            return dto;
        }

        public static List<UserProjectDTO?> CreateList(IEnumerable<UserProject> usersProjects)
        {
            List<UserProjectDTO?> listDto = [];

            foreach (var up in usersProjects)
            {
                listDto.Add(Create(up));
            }

            return listDto;
        }

    }

}
