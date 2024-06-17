using Dental_clinic_WebApi.Module;

namespace Dental_clinic_WebApi.Data
{
    public class BaseDbContext : DbContext 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-HIOKBS7;Initial Catalog=Drug_treatment_clinic;Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True");
        }
    }
    public class ConsumptionDb : BaseDbContext
    {
        public DbSet<Consumption> Patients => Set<Consumption>();
    }

    public class DepartmentsDb : BaseDbContext 
    {
        public DbSet<Departments> Doctors => Set<Departments>();
    }

    public class DrugsDb : BaseDbContext
    {
        public DbSet<Drugs> SpecializationDoctor => Set<Drugs>();
    }

    public class InventoryDb : BaseDbContext
    {
        public DbSet<Inventory> Record => Set<Inventory>();
    }

    public class SuppliersDb : BaseDbContext
    {
        public DbSet<Suppliers> Destination => Set<Suppliers>();
    }

    public class SupplyDb : BaseDbContext
    {
        public DbSet<Supply> Services => Set<Supply>();
    }

    public class UsersDb : BaseDbContext
    {
        public DbSet<Users> Listdiseases => Set<Users>();
    }
}
