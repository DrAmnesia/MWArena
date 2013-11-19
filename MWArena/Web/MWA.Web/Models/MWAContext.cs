using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
namespace MWA.Models
{





    public class MwaDBContext : IdentityDbContext<MechWarrior>
    {
        public MwaDBContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserPref> UserPrefs { get; set; }
        public DbSet<MWA.Models.MwoAMatchMetric> MwoAMatchMetrics { get; set; }
        public DbSet<vwMatchMetric> vwMatchMetrics { get; set; }
        public DbSet<Chassis> Chassis { get; set; }
        public DbSet<DropDeck12> DropDeck12s { get; set; }
        public DbSet<Association> Associations { get; set; }
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

        ///<Summary>An IQueryable collection of ExternalSystem Entities</Summary>
        public DbSet<ExternalSystem> ExternalSystems { get; set; }

        ///<Summary>An IQueryable collection of ExternalSystemUserAccount Entities</Summary>
        public DbSet<ExternalSystemUserAccount> ExternalSystemUserAccounts { get; set; }

        ///<Summary>An IQueryable collection of AppSetting Entities</Summary>
        public DbSet<AppSetting> AppSettings { get; set; }

        ///<Summary>An IQueryable collection of UserAppSetting Entities</Summary>
        public DbSet<UserAppSetting> UserAppSettings { get; set; }

        ///<Summary>An IQueryable collection of vwPilotVariantAssocMetric Entities</Summary>
        public DbSet<vwPilotVariantAssocMetric> vwPilotVariantAssocMetrics { get; set; }



    }


}
