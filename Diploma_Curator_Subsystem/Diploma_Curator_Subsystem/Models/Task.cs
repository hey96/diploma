using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Alternatives { get; set; }
        public string Math_data { get; set; }
        public int StatusID { get; set; }
        public int DomainID { get; set; }

        public Status Status { get; set; }
        public Domain Domain { get; set; }

        public ICollection<UserTask> UserTasks { get; set; }
    }
}
