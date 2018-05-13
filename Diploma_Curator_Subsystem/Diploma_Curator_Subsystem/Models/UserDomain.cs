using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    public class UserDomain
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int DomainID { get; set; }
        [Column(TypeName = "decimal(4, 3)")]
        public decimal CompetitionCoef { get; set; }
        
        public User User { get; set; }
        public Domain Domain { get; set; }
    }
}
