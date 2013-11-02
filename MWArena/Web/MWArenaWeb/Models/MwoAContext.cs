 
using System.Text.RegularExpressions;
using System.Data.Entity;
using MWA.Models;
 

namespace MWA.Models
{
    public class MwoADbContext : DbContext 
    {
        
            public MwoADbContext()
                : base("name=ExpArena")
            {
                Database.SetInitializer<MwoADbContext >(new DropCreateDatabaseIfModelChanges<MwoADbContext>());
                this.Configuration.LazyLoadingEnabled = false;
                this.Configuration.ProxyCreationEnabled = false;
                this.Configuration.AutoDetectChangesEnabled = false;
                this.Configuration.ValidateOnSaveEnabled = false;
            }
      
        public DbSet<MWA.Models.MwoAMatchMetric> MwoAMatchMetrics { get; set; }
    }
}