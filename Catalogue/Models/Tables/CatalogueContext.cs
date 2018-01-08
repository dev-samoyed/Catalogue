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
        public DbSet<Employees> Employees { get; set; }



    }
}