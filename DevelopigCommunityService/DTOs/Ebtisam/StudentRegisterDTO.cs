using DevelopigCommunityService.DTOs.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.DTOs.Ebtisam
{
    public class StudentRegisterDTO: AppUserDTOs
    {
        public int? StudentId {  get; set; }
        public String Password { get; set; }
        public String ConfPassword { get; set; }
    }
}
