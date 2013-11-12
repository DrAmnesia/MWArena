using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MWA.Models
{
    public class UserPref
    {
        public int UserPrefId { get; set; }
        public virtual MechWarrior User { get; set; }
  
        public string UserPrefName { get; set; }

        public string UserPrefValue { get; set; }

        public bool IsEnabled { get; set; }
    }
}