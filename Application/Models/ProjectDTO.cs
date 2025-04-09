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
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.Active;


        public static ProjectDTO Create(Project p)
        {
            var dto = new ProjectDTO();
            dto.Id = p.Id;
            dto.Name = p.Name;
            dto.Description = p.Description;
            dto.Status = p.Status;

            return dto;
        }

        public static List<ProjectDTO?> CreateList(IEnumerable<Project> projects)
        {
            List<ProjectDTO?> listDto = [];

            foreach (var p in projects)
            {
                listDto.Add(Create(p));
            }

            return listDto;
        }

    }
}
