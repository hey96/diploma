using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models.SubsystemViewModels
{
    public class UserIndexData
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Domain> Domains { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
