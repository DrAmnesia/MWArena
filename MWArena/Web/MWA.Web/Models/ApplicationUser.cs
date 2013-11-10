using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;


namespace MWA.Models
{
    using System;
    using Microsoft.AspNet.Identity.EntityFramework;


    public class ApplicationUser : IdentityUser
    {

        public string FactionName { get; set; }


        public string RegimentName { get; set; }


        public string BattalionName { get; set; }


        public string CompanyName { get; set; }


        public string LanceName { get; set; }


        public string RankName { get; set; }


        public string ImageUrl { get; set; }

        public string ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        public string Email { get; set; }
        public virtual ICollection<UserPref> UserPrefs { get; set; }


    }
}



/*  
  
       public class UserAppSetting
        {
            public int UserAppSettingId { get; set; }

            public String AppSettingValue { get; set; }

            public AppSetting AppSetting { get; set; }
        }

        public class AppSetting
        {
            public int AppSettingId { get; set; }
            public int AppSettingPrefix { get; set; }
            public int AppSettingName { get; set; }
            public int IsEnabled { get; set; }

        }

        public class ExternalSystemUserAccount
        {
            public int UserExternalSystemAccountId { get; set; }

            public string ExternalSystemAccountName { get; set; }

            public string ExternalSystemAccountPassword { get; set; }

            public bool IsEnabled { get; set; }

            public virtual ICollection<UserAppSetting> UserAppSettings { get; set; }

            public ExternalSystem ExternalSystem { get; set; }
        }

        public class ExternalSystem
        {
            public int ExternalSystemId { get; set; }

            public string ExternalSystemName { get; set; }

            public string ExternalSystemType { get; set; }

            public string ExternalSystemEndpoint { get; set; }

            public string ExternalSystemIcon { get; set; }

            public string ExternalSystemAbbrv { get; set; }

            public string ExternalSystemDescription { get; set; }

            public bool IsEnabled { get; set; }

        }*/
 