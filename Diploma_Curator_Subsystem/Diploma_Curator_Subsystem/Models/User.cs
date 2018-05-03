using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserDomain> UserDomains { get; set; }
    }
}
