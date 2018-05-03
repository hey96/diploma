using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    public class Domain
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Task> Tasks { get; set; }
        public ICollection<UserDomain> UserDomains { get; set; }
    }
}
