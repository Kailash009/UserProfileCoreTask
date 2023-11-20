using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfile.DAL.Models.Dtos;

namespace UserProfile.DAL.Models
{
    public class User
    {
        [Key]
        public int Uid { get; set; }
        public string ?Name { get; set; }
        public string  ?Email { get; set; }
        public string ?MobileNo { get; set; }
        public string ?Gender { get; set; }
        public string ?DOB { get; set; }
        public string? Qualification { get; set; }
        public List<UserAddress>? Addresses { get; set; }
    }
}
