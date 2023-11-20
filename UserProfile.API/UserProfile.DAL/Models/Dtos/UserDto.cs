using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfile.DAL.Models.Dtos
{
    public class UserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? Gender { get; set; }
        public DateTime DOB { get; set; }
        public string? Qualification { get; set; }
        public List<UserAddressDto>? Addresses { get; set; }
    }
}
