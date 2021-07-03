using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.DTOs.Bassal
{
    public class IndividualChangePasswordDTO
    {
        public int Id { get; set; }
        public String NewPassword { get; set; }
        public String ConfNewPassword { get; set; }
    }
}
