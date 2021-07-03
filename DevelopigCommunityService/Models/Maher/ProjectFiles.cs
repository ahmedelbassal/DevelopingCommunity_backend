using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ADDE NAME SPACES
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevelopigCommunityService.Models.Somaya;
//

namespace DevelopigCommunityService.Models.Maher
{
    public class ProjectFiles
    {
        public int Id { get; set; }
        
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public string FileUrl { get; set; }


        public virtual Project Project { get; set; } = new Project();
    }
}
