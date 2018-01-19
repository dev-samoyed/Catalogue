using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Catalogue.Models.Tables
{
    public class CatalogueContext : DbContext
    {
        public DbSet<Administration> Administrations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Division> Divisions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            //modelBuilder.Entity<Employee>()
            //    .HasRequired<Position>(s => s.GetPosition)
            //    .WithMany(g => g.Employees)
            //    .HasForeignKey<int?>(s => s.PositionId);
            modelBuilder.Entity<Position>()
                .HasMany<Employee>(g => g.Employees)
                .WithRequired(s => s.Position)
                .HasForeignKey<int>(s => s.PositionId);
            modelBuilder.Entity<Department>()
                .HasMany<Employee>(g => g.Employees)
                .WithRequired(s => s.Department)
                .HasForeignKey<int>(s => s.DepartmentId);
            modelBuilder.Entity<Administration>()
                .HasMany<Department>(g => g.Departments)
                .WithRequired(s => s.Administration)
                .HasForeignKey<int>(s => s.AdministrationId);

            modelBuilder.Entity<Division>()
                .HasMany<Administration>(g => g.Administrations)
                .WithRequired(s => s.Division)
                .HasForeignKey<int>(s => s.DivisionId);
        }
    }

}
