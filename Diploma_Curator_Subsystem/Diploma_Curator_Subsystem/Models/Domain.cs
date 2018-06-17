using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    [Table("domains")]
    public class Domain
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("name")]
        public string Name { get; set; }

        public ICollection<Task> Tasks { get; set; }
        public ICollection<UserDomain> UserDomains { get; set; }
    }
}
