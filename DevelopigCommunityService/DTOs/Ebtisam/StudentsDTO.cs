using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.DTOs.Ebtisam
{
    public class StudentsDTO
    {
        public String UserName { get; set; }
        public String Token { get; set; }

        public String UserType { get; set; } = "student";

    }
}
