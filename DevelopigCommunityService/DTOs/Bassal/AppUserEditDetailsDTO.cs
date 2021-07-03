using DevelopigCommunityService.DTOs.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.DTOs.Bassal
{
    public class AppUserEditDetailsDTO
    {
        public int? DeptId { get; set; }
        public int? Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int Age { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
    }
}
