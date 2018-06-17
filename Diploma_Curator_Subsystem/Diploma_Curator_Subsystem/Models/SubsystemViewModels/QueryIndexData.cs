using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models.SubsystemViewModels
{
    public class QueryIndexData
    {
        public Query Query { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<UserDomain> UserDomains { get; set; }
    }
}
