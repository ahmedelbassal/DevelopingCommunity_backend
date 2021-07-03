using DevelopigCommunityService.DTOs.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.DTOs.Bassal
{
   

    public class AdminRegisterDTOs : AppUserDTOs
    {
        public String Password { get; set; }
        public String ConfPassword { get; set; }
    }
}
