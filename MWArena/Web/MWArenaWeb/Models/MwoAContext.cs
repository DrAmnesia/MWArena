 
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
              //  Database.SetInitializer<MwoADbContext >(new DropCreateDatabaseIfModelChanges<MwoADbContext>());
                this.Configuration.LazyLoadingEnabled = false;
                this.Configuration.ProxyCreationEnabled = false;
                this.Configuration.AutoDetectChangesEnabled = false;
                this.Configuration.ValidateOnSaveEnabled = false;
            }
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
 
                modelBuilder.Entity<MatchDrop>().HasRequired(e => e.Association).WithMany();
                modelBuilder.Entity<MatchDrop>().HasRequired(e => e.Map).WithMany();
                modelBuilder.Entity<MwoAMatchMetric>().HasRequired(e => e.MatchDrop).WithMany();
            }    
        public DbSet<MWA.Models.MwoAMatchMetric> MwoAMatchMetrics { get; set; }
        public DbSet<vwMatchMetric> vwMatchMetrics { get; set; }
        public DbSet<Chassis> Chassis { get; set; }
        public DbSet<DropDeck12> DropDeck12s { get; set; }
        public DbSet<Association> Associations{ get; set; }
        public DbSet<MatchDrop> MatchDrops { get; set; }
        public DbSet<Map> Maps { get; set; }
        //public DbSet<MWMatch> MWMatches { get; set; }
        //public DbSet<DropDeck> DropDecks { get; set; }

    }
}