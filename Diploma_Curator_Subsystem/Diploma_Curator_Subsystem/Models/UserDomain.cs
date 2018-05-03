using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    public class UserDomain
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int DomainID { get; set; }
        public double CompetitionCoef { get; set; }
        
        public User User { get; set; }
        public Domain Domain { get; set; }
    }
}
