using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class UserProjectUpdateRequestDTO
    {
            public int UserId { get; set; }
            public int ProjectId { get; set; }

            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Status Status { get; set; } = Status.Active;
     
    }
}

