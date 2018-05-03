using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    public class UserTask
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int TaskID { get; set; }
        public int RoleID { get; set; }
        public int StatusID { get; set; }

        public Task Task { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
    }
}
