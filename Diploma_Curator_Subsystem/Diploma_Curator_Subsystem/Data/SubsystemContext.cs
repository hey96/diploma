using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Diploma_Curator_Subsystem.Models;

namespace Diploma_Curator_Subsystem.Data
{
    public class SubsystemContext : DbContext
    {
        public SubsystemContext(DbContextOptions<SubsystemContext> options) : base(options)
        {
        }

        public DbSet<Domain> Domains { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Diploma_Curator_Subsystem.Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDomain> UserDomains { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
    }
}
