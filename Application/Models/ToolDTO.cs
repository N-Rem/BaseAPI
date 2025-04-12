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
    public class ToolDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? UserTool { get; set; }


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.Active;


        public static ToolDTO Create(Tool t)
        {
            var dto = new ToolDTO();
            dto.Id = t.Id;
            dto.Name = t.Name;
            dto.Description = t.Description;
            dto.UserTool = t.UserTool;
            dto.Status = t.Status;

            return dto;
        }

        public static List<ToolDTO?> CreateList(IEnumerable<Tool> tools)
        {
            List<ToolDTO?> listDto = [];

            foreach (var t in tools)
            {
                listDto.Add(Create(t));
            }

            return listDto;
        }
    }



}
