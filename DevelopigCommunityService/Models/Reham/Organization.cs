using DevelopigCommunityService.Models.Aya;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.Models.Reham
{
    public class Organization
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is reuired")]
        [StringLength(100)]
        public String Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress] //ef
        public String Email { get; set; }

        public String PhotoUrl { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        public String Phone { get; set; }


        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Address is required")]
        public String Address { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Description is required")]
        public String Description { get; set; }

        [Required(ErrorMessage ="Enter website for organization")]
        public String Website { get; set; }

        public virtual OrganizationType OrganizationType { get; set; }

    }
}
