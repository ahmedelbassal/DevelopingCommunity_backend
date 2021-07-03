using DevelopigCommunityService.Models.AbstractClasses.Bassal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ADDED NAME SPACES
using DevelopigCommunityService.Models.Aya;
using DevelopigCommunityService.Models.Reham;
using System.ComponentModel.DataAnnotations.Schema;
//

namespace DevelopigCommunityService.Models.Maher
{
    public enum Confirmed
    {
        Yes = 1,
        No = 0
    }
    public class Instructor: AppUser
    {
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public Confirmed Confirmed { get; set; } = Confirmed.No;
        public String EducationBackground { get; set; }
        public string CategoryAccess { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; } = new Department();

        public virtual Organization Organization { get; set; } = new Organization();
       

   

    }
}
