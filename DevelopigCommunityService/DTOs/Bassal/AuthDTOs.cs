using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevelopigCommunityService.DTOs.Bassal
{
    public class AuthDTOs
    {
        public int? Id { get; set; }
        public String NameId { get; set; }
        public bool IsAdmin { get; set; }

        public String Type { get; set; }
    }
}
