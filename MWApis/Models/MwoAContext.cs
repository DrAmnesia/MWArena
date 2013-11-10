 
using System.Text.RegularExpressions;
using System.Data.Entity;
using MWA.Models;
 

namespace MWA.Models
{
    public class MWApiContext : DbContext 
    {

        public MWApiContext()
           : base("name=ExpArena")
            {
               // Database.SetInitializer<MWApiContext>(new DropCreateDatabaseIfModelChanges<MWApiContext>());
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

        ///<Summary>An IQueryable collection of vwMatchDropDeckMetric Entities</Summary>
        //public DbSet<vwMatchDropDeckMetric> vwMatchDropDeckMetrics { get; set; }

        ///<Summary>An IQueryable collection of vwVariantAssocMapMetric Entities</Summary>
        public DbSet<vwVariantAssocMapMetric> vwVariantAssocMapMetrics { get; set; }

        ///<Summary>An IQueryable collection of vwVariantAssocMetric Entities</Summary>
        public DbSet<vwVariantAssocMetric> vwVariantAssocMetrics { get; set; }

        ///<Summary>An IQueryable collection of MwoAMatchMetrics_import Entities</Summary>
        public DbSet<MwoAMatchMetrics_import> MwoAMatchMetrics_imports { get; set; }

        ///<Summary>An IQueryable collection of vwVariantMatchStatsSummary Entities</Summary>
       // public DbSet<vwVariantMatchStatsSummary> vwVariantMatchStatsSummarys { get; set; }

        ///<Summary>An IQueryable collection of vwWeightClassMatchStatsSummary Entities</Summary>
       // public DbSet<vwWeightClassMatchStatsSummary> vwWeightClassMatchStatsSummarys { get; set; }
 
        //public DbSet<MWMatch> MWMatches { get; set; }
        //public DbSet<DropDeck> DropDecks { get; set; }

    }
}