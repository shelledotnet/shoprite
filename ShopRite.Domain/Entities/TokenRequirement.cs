﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopRite.Application.Dto
{
    public class TokenRequirement
    {
        public string? Token { get; set; }

        public SecurityToken? SecurityToken  { get; set; }

    }
}
