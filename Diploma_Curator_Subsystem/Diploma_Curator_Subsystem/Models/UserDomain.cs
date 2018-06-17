using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    [Table("user_domains")]
    public class UserDomain
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("user_id")]
        public int UserID { get; set; }
        [Column("domain_id")]
        public int DomainID { get; set; }

        [Column("competition_coef", TypeName = "decimal(4, 3)")]
        public decimal CompetitionCoef { get; set; }

        public User User { get; set; }
        public Domain Domain { get; set; }
    }
}