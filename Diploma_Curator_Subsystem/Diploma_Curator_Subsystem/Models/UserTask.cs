using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    [Table("user_tasks")]
    public class UserTask
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("user_id")]
        public int UserID { get; set; }
        [Column("task_id")]
        public int TaskID { get; set; }
        [Column("role_id")]
        public int RoleID { get; set; }
        [Column("status")]
        public int StatusID { get; set; }

        public Task Task { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
    }
}