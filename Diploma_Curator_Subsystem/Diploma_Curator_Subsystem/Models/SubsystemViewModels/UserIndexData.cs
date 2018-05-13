using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models.SubsystemViewModels
{
    public class UserIndexData
    {
        public ICollection<User> Users { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Domain> Domains { get; set; }
        public ICollection<UserDomain> UserDomains { get; set; }
    }
}
