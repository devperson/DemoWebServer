using ASPMvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HServer.Models.DataAccess
{
    /// <summary>
    /// This is EF data context class it holds all tables.
    /// </summary>
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = true;
        }
        
        static DataBaseContext()
        {
            Database.SetInitializer<DataBaseContext>(new DataBaseInitializer());
        }

        /// <summary>
        /// Configure table to object mapping.
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Configurations.Add(new DoctorModelConfig());
        }
        
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DriverInventory> DriverInventories { get; set; }
        public DbSet<Menu> Menus { get; set; }
    }
}