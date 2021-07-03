using DevelopigCommunityService.Models.Bassal;
using DevelopigCommunityService.Models.Ebtisam;
using DevelopigCommunityService.Models.Maher;
using DevelopigCommunityService.Models.Reham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.Models.Somaya
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CodeView { get; set; }
        public virtual ICollection<Student> Students { get; set; }
            = new HashSet<Student>();
        public virtual ICollection<Individual> Individuals { get; set; }
            = new HashSet<Individual>();
        public virtual ICollection<ProjectFiles> ProjectFiles { get; set; }
            = new HashSet<ProjectFiles>();
        public virtual ICollection<ProjectVideos> projectVideos { get; set; }
            = new HashSet<ProjectVideos>();
        public virtual ICollection<ProjectPhotos> projectPhotos { get; set; }
            = new HashSet<ProjectPhotos>();

    }
}
