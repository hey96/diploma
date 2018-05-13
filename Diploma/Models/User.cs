using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    public class User
    {
        public User()
        {
            Roles = new HashSet<Role>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}