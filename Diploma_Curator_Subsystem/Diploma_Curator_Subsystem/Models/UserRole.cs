using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    public class UserRole
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
