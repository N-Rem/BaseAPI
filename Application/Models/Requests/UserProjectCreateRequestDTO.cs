using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class UserProjectCreateRequestDTO
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }

    }
}
