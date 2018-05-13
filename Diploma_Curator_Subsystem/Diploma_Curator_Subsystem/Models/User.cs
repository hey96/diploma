using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma_Curator_Subsystem.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserDomain> UserDomains { get; set; }
    }
}
