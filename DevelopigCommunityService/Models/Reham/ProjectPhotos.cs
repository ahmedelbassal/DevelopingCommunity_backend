using DevelopigCommunityService.Models.Somaya;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.Models.Reham
{
    public class ProjectPhotos
    {
        public int Id { get; set; }
        public String Url { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

    }
}
