﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ToolCreateRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserTool { get; set; } //puede ser nulo
    }
}
