using D2LExtensionWebAPPSSR.Configuration;
using D2LExtensionWebAPPSSR.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace D2LExtensionWebAPPSSR.Data
{
    public class D2LDBContext : IdentityDbContext<User>
    {
        public D2LDBContext(DbContextOptions<D2LDBContext> options) : base(options)
        {
        }
        public D2LDBContext() : base()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-CHPSHFSL\\SQLEXPRESS;Database=SchemaForD2LExtensionAPP;Trusted_Connection=True;TrustServerCertificate=True;");
            }
            
        }


    }
}
