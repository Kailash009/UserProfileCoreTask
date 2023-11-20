﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfile.DAL.Models.Dtos
{
    public class UserAddressDto
    {
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int Pin { get; set; }
        public int Uid { get; set; }
    }
}
