using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWA.Models
{
 
        //mwoalogwarrior stats    
        /// <summary>Class MwoAMatchMetric </summary>
        public class MwoAMatchMetric
        {
            public int MwoAMatchMetricId { get; set; }
          
            public string time { get; set; }
            public string level { get; set; }
            public int victory { get; set; }
            public string victoryType { get; set; }
            public string matchType { get; set; }
            public string team { get; set; }
            public string name { get; set; }
            public string mech { get; set; }
            public int status { get; set; }
            public int matchscore { get; set; }
            public int kills { get; set; }
            public int assists { get; set; }
            public int damage { get; set; }
            public int ping { get; set; }
            public int lance { get; set; }

            public string AssociationName { get; set; }

            public int AssociationId { get; set; }

            public int PublishFlag { get; set; }

            public bool PublishingUserName { get; set; }

            public string MatchHash { get; set; }

        }
 
}
