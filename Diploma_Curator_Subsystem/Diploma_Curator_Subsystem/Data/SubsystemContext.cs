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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserID, ur.RoleID });
            modelBuilder.Entity<UserDomain>().HasKey(ur => new { ur.UserID, ur.DomainID });
            //modelBuilder.Entity<UserTask>().HasKey(ur => new { ur.UserID, ur.TaskID });

            //modelBuilder.Entity<Query>().Property(q => q.Created).HasDefaultValueSql("getdate()");

            //modelBuilder.Entity<UserRole>()
            //    .HasOne<User>(ur => ur.User)
            //    .WithMany(u => u.UserRoles)
            //    .HasForeignKey(ur => ur.UserID);
            //modelBuilder.Entity<UserRole>()
            //    .HasOne<Role>(ur => ur.Role)
            //    .WithMany(u => u.UserRoles)
            //    .HasForeignKey(ur => ur.RoleID);
        }

        public DbSet<Domain> Domains { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Diploma_Curator_Subsystem.Models.Task> Tasks { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDomain> UserDomains { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
    }
}
