using DevelopigCommunityService.Models.Aya;
using DevelopigCommunityService.Models.Bassal;
using DevelopigCommunityService.Models.Ebtisam;
using DevelopigCommunityService.Models.Maher;
using DevelopigCommunityService.Models.Reham;
using DevelopigCommunityService.Models.Somaya;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.Context
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<OrganizationType> OrganizationTypes { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Individual> Individuals { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectPhotos> ProjectPhotos { get; set; }
        public virtual DbSet<ProjectVideos> ProjectVideos { get; set; }
        public virtual DbSet<ProjectFiles> ProjectFiles { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }




    }
}
