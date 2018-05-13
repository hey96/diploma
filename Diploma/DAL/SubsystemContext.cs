using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Diploma.Models;

namespace Diploma.DAL
{
    public class SubsystemContext : DbContext
    {
        public SubsystemContext() : base(nameOrConnectionString: "connection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}