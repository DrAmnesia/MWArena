using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using MatchLogger;


namespace MWA.Models
{
    public static class ModelHelper
    {
        /*public static DropDeck12 SetChassis(this DropDeck12 dropdeck12, MwoADbContext db)
        {
            
        }
         * */

        public static MatchDrop GetLoggedMatchDrop(this LoggedMatch value, MwoADbContext db)
        {       
                
                List<String> friendlyMechs = value.FriendlyMatchStats.Select(f => f.mech).OrderBy(m => m).ToList();
                List<String> enemyMechs = value.EnemyMatchStats.Select(f => f.mech).OrderBy(m => m).ToList();
                int tdmg = value.FriendlyMatchStats.Select(f => f.damage).Sum()+ value.EnemyMatchStats.Select(e => e.damage).Sum();
                MatchStat ms = value.FriendlyMatchStats.FirstOrDefault(l => l.level != "" && l.level != null);
                DropDeck12 fdd = SetDropDeck(friendlyMechs, db);
                DropDeck12 edd = SetDropDeck(enemyMechs, db);
                Association assoc = db.Associations.FirstOrDefault(a => a.AssociationName == value.AssociationName);
                
            var md=new MatchDrop
            {
                AssociationName = value.AssociationName,
                AssociationId = assoc.AssociationId,
                Association=db.Associations.FirstOrDefault(i=>i.AssociationId==assoc.AssociationId),
                FriendlyDropDeck12 = db.DropDeck12s.FirstOrDefault(f => f.DropDeck12Id == fdd.DropDeck12Id),
                FriendlyDropDeck12Id = fdd.DropDeck12Id,
                EnemyDropDeck12 = db.DropDeck12s.FirstOrDefault(e => e.DropDeck12Id == edd.DropDeck12Id),
                EnemyDropDeck12Id = edd.DropDeck12Id,
                Map = db.Maps.FirstOrDefault(a => a.MapAltName1 == ms.level)
            };
            md.MatchHash = md.CalcMatchHash(tdmg);

            if (db.MatchDrops.Any(m => m.MatchHash == md.MatchHash && m.AssociationName == value.AssociationName))
            {
                md = db.MatchDrops.FirstOrDefault(m => m.MatchHash == md.MatchHash && m.AssociationName == value.AssociationName);
            }
            else
            {
                db.MatchDrops.Add(md);
                db.SaveChanges();

                foreach (MatchStat fm in value.FriendlyMatchStats)
                {
                    MWA.Models.MwoAMatchMetric matchtopush = new MWA.Models.MwoAMatchMetric();
                    matchtopush.assists = fm.assists;
                    matchtopush.damage = fm.damage;
                    matchtopush.kills = fm.kills;
                    matchtopush.lance = fm.lance;
                    matchtopush.level = fm.level;
                    matchtopush.matchType = fm.matchType;
                    matchtopush.matchscore = fm.matchscore;
                    matchtopush.mech = fm.mech;
                    matchtopush.name = fm.name;
                    matchtopush.ping = fm.ping;
                    matchtopush.status = fm.status;
                    matchtopush.team = fm.team;
                    matchtopush.victory = fm.victory;
                    matchtopush.victoryType = fm.victoryType;
                    matchtopush.time = fm.time.ToUniversalTime().ToString();
                    matchtopush.MatchHash = value.MatchHash;
                    matchtopush.PublishFlag = 1;
                    matchtopush.PublishingUserName = (value.PublishingUserName == fm.name);
                    matchtopush.MatchDrop = db.MatchDrops.FirstOrDefault(m => m.MatchDropId == md.MatchDropId);
                    db.MwoAMatchMetrics.Add(matchtopush);
                    db.SaveChanges();
                }
                foreach (MatchStat em in value.EnemyMatchStats)
                {
                    MWA.Models.MwoAMatchMetric matchtopush = new MWA.Models.MwoAMatchMetric();
                    matchtopush.assists = em.assists;
                    matchtopush.damage = em.damage;
                    matchtopush.kills = em.kills;
                    matchtopush.lance = em.lance;
                    matchtopush.level = em.level;
                    matchtopush.matchType = em.matchType;
                    matchtopush.matchscore = em.matchscore;
                    matchtopush.mech = em.mech;
                    matchtopush.name = em.name;
                    matchtopush.ping = em.ping;
                    matchtopush.status = em.status;
                    matchtopush.team = em.team;
                    matchtopush.victory = em.victory;
                    matchtopush.victoryType = em.victoryType;
                    matchtopush.time = em.time.ToUniversalTime().ToString();
                    matchtopush.MatchHash = value.MatchHash;
                    matchtopush.PublishFlag = 1;
                    matchtopush.PublishingUserName = (value.PublishingUserName == em.name);
                    matchtopush.MatchDrop = db.MatchDrops.FirstOrDefault(m => m.MatchDropId == md.MatchDropId);
                    db.MwoAMatchMetrics.Add(matchtopush);
                    db.SaveChanges();
                }
            }
            return md;
        }

        public static DropDeck12 SetDropDeck(List<String> ddMechs, MwoADbContext db)
        {
            int ddMechCount = ddMechs.Count();
            List<String> ddMechsClean = ddMechs.Where(m => !(m == null || m == String.Empty)).ToList();

            //!(b.Diameter == null || b.Diameter.Trim() == string.Empty)
            List<KeyValuePair<int, string>> chassisFakeDict = new List<KeyValuePair<int, string>>();
            //chassisFakeDict = db.Chassis.Where(v => ddMechs.Contains(v.AltName2)).Select(k => new KeyValuePair<int,string>(k.BaseVariantId,k.BaseVariantName)).ToList();
 
             chassisFakeDict = db.Chassis.Join(ddMechsClean, c => c.AltName2, o => o, (c, o) => new
                  {
                     k = c.BaseVariantId,
                    v=c.AltName2
                  }).AsEnumerable().Select(o => new KeyValuePair<int, string>(o.k, o.v)).ToList(); ;
             foreach (string mn in ddMechs)
            {
                if (mn.IsNullOrWhiteSpace()) chassisFakeDict.Add(new KeyValuePair<int, string>(0,"UNK-0"));
            }
            if (chassisFakeDict.Count < 12)
            {
                int diff = 12 - chassisFakeDict.Count;
                for (int i = 0; i < diff; i++)
                {
                    chassisFakeDict.Add(new KeyValuePair<int, string>(0, "UNK-0"));
                }
            }
            chassisFakeDict.Sort(keyCompare);
          
            DropDeck12 dd = new DropDeck12
            {

                MechName1 = (ddMechCount >= 1) ? chassisFakeDict[0].Value: "UNK-0",
                MechName2 = (ddMechCount >= 2) ? chassisFakeDict[1].Value : "UNK-0",
                MechName3 = (ddMechCount >= 3) ? chassisFakeDict[2].Value : "UNK-0",
                MechName4 = (ddMechCount >= 4) ? chassisFakeDict[3].Value : "UNK-0",
                MechName5 = (ddMechCount >= 5) ? chassisFakeDict[4].Value : "UNK-0",
                MechName6 = (ddMechCount >= 6) ? chassisFakeDict[5].Value : "UNK-0",
                MechName7 = (ddMechCount >= 7) ? chassisFakeDict[6].Value : "UNK-0",
                MechName8 = (ddMechCount >= 8) ? chassisFakeDict[7].Value : "UNK-0",
                MechName9 = (ddMechCount >= 9) ? chassisFakeDict[8].Value : "UNK-0",
                MechName10 = (ddMechCount >= 10) ? chassisFakeDict[9].Value : "UNK-0",
                MechName11 = (ddMechCount >= 11) ? chassisFakeDict[10].Value : "UNK-0",
                MechName12 = (ddMechCount >= 12) ? chassisFakeDict[11].Value : "UNK-0",
                ChassisId1 = (ddMechCount >= 1) ? chassisFakeDict[0].Key:0,
                ChassisId2 = (ddMechCount >= 1) ? chassisFakeDict[1].Key : 0,
                ChassisId3 = (ddMechCount >= 1) ? chassisFakeDict[2].Key : 0,
                ChassisId4 = (ddMechCount >= 1) ? chassisFakeDict[3].Key : 0,
                ChassisId5 = (ddMechCount >= 1) ? chassisFakeDict[4].Key : 0,
                ChassisId6 = (ddMechCount >= 1) ? chassisFakeDict[5].Key : 0,
                ChassisId7 = (ddMechCount >= 1) ? chassisFakeDict[6].Key : 0,
                ChassisId8 = (ddMechCount >= 1) ? chassisFakeDict[7].Key : 0,
                ChassisId9 = (ddMechCount >= 1) ? chassisFakeDict[8].Key : 0,
                ChassisId10 = (ddMechCount >= 1) ? chassisFakeDict[9].Key : 0,
                ChassisId11 = (ddMechCount >= 1) ? chassisFakeDict[10].Key : 0,
                ChassisId12 = (ddMechCount >= 1) ? chassisFakeDict[11].Key : 0
            };
           
            dd.Chassis1 = (!dd.MechName1.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId1 ) : dd.Chassis1 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis2 = (!dd.MechName2.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId2) : dd.Chassis2 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis3 = (!dd.MechName3.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId3) : dd.Chassis3 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis4 = (!dd.MechName4.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId4) : dd.Chassis4 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis5 = (!dd.MechName5.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId5) : dd.Chassis5 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis6 = (!dd.MechName6.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId6) : dd.Chassis6 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis7 = (!dd.MechName7.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId7) : dd.Chassis7 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis8 = (!dd.MechName8.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId8) : dd.Chassis8 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis9 = (!dd.MechName9.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId9) : dd.Chassis9 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis10 = (!dd.MechName10.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId10) : dd.Chassis10 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis11 = (!dd.MechName11.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId11) : dd.Chassis11 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.Chassis12 = (!dd.MechName12.IsNullOrWhiteSpace()) ? db.Chassis.FirstOrDefault(c => c.Id == dd.ChassisId12) : dd.Chassis12 = db.Chassis.FirstOrDefault(c => c.Id == 0);
           
            if (dd.Chassis1 == null) dd.Chassis1 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis2 == null) dd.Chassis2 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis3 == null) dd.Chassis3 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis4 == null) dd.Chassis4 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis5 == null) dd.Chassis5 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis6 == null) dd.Chassis6 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis7 == null) dd.Chassis7 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis8 == null) dd.Chassis8 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis9 == null) dd.Chassis9 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis10 == null) dd.Chassis10 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis11 == null) dd.Chassis11 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            if (dd.Chassis12 == null) dd.Chassis12 = db.Chassis.FirstOrDefault(c => c.Id == 0);
            dd.ChassisId1 = dd.Chassis1.Id;
            dd.ChassisId2 = dd.Chassis2.Id;
            dd.ChassisId3 = dd.Chassis3.Id;
            dd.ChassisId4 = dd.Chassis4.Id;
            dd.ChassisId5 = dd.Chassis5.Id;
            dd.ChassisId6 = dd.Chassis6.Id;
            dd.ChassisId7 = dd.Chassis7.Id;
            dd.ChassisId8 = dd.Chassis8.Id;
            dd.ChassisId9 = dd.Chassis9.Id;
            dd.ChassisId10 = dd.Chassis10.Id;
            dd.ChassisId11 = dd.Chassis11.Id;
            dd.ChassisId12 = dd.Chassis12.Id;
            dd.MechCount = dd.CalcMechCount();
            dd.Tonnage = dd.CalcTonnage();
            dd = GetUniqueDropDeck(dd, db);
            return dd;
        }

        public static int CalcMechCount(this DropDeck12 dd)
        {
            int mechCount = 0;
            if (dd.ChassisId1 != 0) mechCount++;
            if (dd.ChassisId2 != 0) mechCount++;
            if (dd.ChassisId3 != 0) mechCount++;
            if (dd.ChassisId4 != 0) mechCount++;
            if (dd.ChassisId5 != 0) mechCount++;
            if (dd.ChassisId6 != 0) mechCount++;
            if (dd.ChassisId7 != 0) mechCount++;
            if (dd.ChassisId8 != 0) mechCount++;
            if (dd.ChassisId9 != 0) mechCount++;
            if (dd.ChassisId10 != 0) mechCount++;
            if (dd.ChassisId11 != 0) mechCount++;
            if (dd.ChassisId12 != 0) mechCount++;
            return mechCount;
        }

        public static string MechsHash(this DropDeck12 dd)
        {
            return String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}",
                dd.ChassisId1,
                dd.ChassisId2,
                dd.ChassisId3,
                dd.ChassisId4,
                dd.ChassisId5,
                dd.ChassisId6,
                dd.ChassisId7,
                dd.ChassisId8,
                dd.ChassisId9,
                dd.ChassisId10,
                dd.ChassisId11,
                dd.ChassisId12);

        }

        public static int CalcTonnage(this DropDeck12 dd)
        {
            int tonnage = 0;
            if (dd.ChassisId1 != 0) tonnage = tonnage+dd.Chassis1.Tonnage;
            if (dd.ChassisId2 != 0) tonnage = tonnage + dd.Chassis2.Tonnage;
            if (dd.ChassisId3 != 0) tonnage = tonnage + dd.Chassis3.Tonnage;
            if (dd.ChassisId4 != 0) tonnage = tonnage + dd.Chassis4.Tonnage;
            if (dd.ChassisId5 != 0) tonnage = tonnage + dd.Chassis5.Tonnage;
            if (dd.ChassisId6 != 0) tonnage = tonnage + dd.Chassis6.Tonnage;
            if (dd.ChassisId7 != 0) tonnage = tonnage + dd.Chassis7.Tonnage;
            if (dd.ChassisId8 != 0) tonnage = tonnage + dd.Chassis8.Tonnage;
            if (dd.ChassisId9 != 0) tonnage = tonnage + dd.Chassis9.Tonnage;
            if (dd.ChassisId10 != 0) tonnage = tonnage + dd.Chassis10.Tonnage;
            if (dd.ChassisId11 != 0) tonnage = tonnage + dd.Chassis11.Tonnage;
            if (dd.ChassisId12 != 0) tonnage = tonnage + dd.Chassis12.Tonnage;
            return tonnage;
        }
        public static DropDeck12 GetUniqueDropDeck(DropDeck12 ddCriteria, MwoADbContext db)
        {

            DropDeck12 dd12 = db.DropDeck12s.FirstOrDefault(dd =>
              dd.ChassisId1 == ddCriteria.ChassisId1 &&
              dd.ChassisId2 == ddCriteria.ChassisId2 &&
              dd.ChassisId3 == ddCriteria.ChassisId3 &&
              dd.ChassisId4 == ddCriteria.ChassisId4 &&
              dd.ChassisId5 == ddCriteria.ChassisId5 &&
              dd.ChassisId6 == ddCriteria.ChassisId6 &&
              dd.ChassisId7 == ddCriteria.ChassisId7 &&
              dd.ChassisId8 == ddCriteria.ChassisId8 &&
              dd.ChassisId9 == ddCriteria.ChassisId9 &&
              dd.ChassisId10 == ddCriteria.ChassisId10 &&
              dd.ChassisId11 == ddCriteria.ChassisId11 &&
              dd.ChassisId12 == ddCriteria.ChassisId12);
            if (dd12 == null)
            {
                db.DropDeck12s.Add(ddCriteria);
                db.SaveChanges();
            }
            else
            {
                ddCriteria = dd12;
            }
            return ddCriteria;
        }

        public static string CalcMatchHash(this MatchDrop md,int totdmg )
        {
            string matchHash = String.Empty;
            if (totdmg==0 )
                matchHash = String.Format("{0}{1}{2}{3}:0-0", System.DateTime.UtcNow.Year,
                    System.DateTime.UtcNow.DayOfYear, 1000000, 0);
            else
            {
                int lvl = (md.Map!=null)?md.Map.MapId:0;
             matchHash = String.Format("{0}{1}{2}{3}:{4}-{5}", System.DateTime.UtcNow.Year,
                System.DateTime.UtcNow.DayOfYear, totdmg, lvl, md.FriendlyDropDeck12.MechsHash(),
                md.EnemyDropDeck12.MechsHash());
            }
        return matchHash;
        }
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        static int keyCompare(KeyValuePair<int, string> a, KeyValuePair<int, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }
    }
}