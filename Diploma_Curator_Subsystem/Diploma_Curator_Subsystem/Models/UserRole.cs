using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    [Table("user_roles")]
    public class UserRole
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("user_id")]
        public int UserID { get; set; }
        [Column("role_id")]
        public int RoleID { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
