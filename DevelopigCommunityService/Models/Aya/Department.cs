using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ADDED NAMESPACES
using DevelopigCommunityService.Models.Maher;
//

namespace DevelopigCommunityService.Models.Aya
{
    public class Department
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        [JsonIgnore]
        public bool IsActive { get; set; }

        public virtual ICollection<Instructor> instructors { get; set; } = new HashSet<Instructor>();
    }
}
