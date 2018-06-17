using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models.SubsystemViewModels
{
    public class TaskIndexData
    {
        public ICollection<Task> Tasks { get; set; }
        public ICollection<Domain> Domains { get; set; }
        public ICollection<Status> Statuses { get; set; }
    }
}
