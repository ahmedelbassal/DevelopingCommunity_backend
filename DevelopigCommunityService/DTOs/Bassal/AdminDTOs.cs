using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.DTOs.Bassal
{
    public class AdminDTOs
    {
        public String UserName { get; set; }
        public String Token { get; set; }

        public String UserType { get; set; } = "admin";
    }
}
