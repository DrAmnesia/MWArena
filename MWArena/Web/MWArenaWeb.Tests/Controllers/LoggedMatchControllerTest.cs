using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MWArenaWeb;
using MWArenaWeb.Controllers;
using MWA.Models;
using MatchLogger;
using MwoArenaWeb.Controllers;
 
namespace MWArenaWeb.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {

        public MwoADbContext db = new MwoADbContext(); 
        [TestMethod]
        public void Get()
        {
            MatchLogger.MatchLogger.LoggedMatch lm = new MatchLogger.MatchLogger.LoggedMatch();
            DropDeck12 dde = new DropDeck12();
            DropDeck12 ddf = new DropDeck12();
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        public static MatchStat ImportToMatchStat(MwoAMatchMetrics_import ims)
        {
            var ms = new MatchStat();
            ms.assists = ims.assists;
            ms.damage = ims.damage;
            ms.kills = ims.kills;
            ms.lance = ims.lance;
            ms.level = ims.level;
            ms.matchType = ims.matchType;
            ms.matchscore = ims.matchscore;
            ms.mech = ims.mech;
            ms.name = ims.name;
            ms.ping = ims.ping;
            ms.team = ims.team;
            ms.time = DateTime.Parse(ims.time);
            ms.victory = ims.victory;
            ms.victoryType = ims.victoryType;
            ms.status = ims.status;
            return ms;

        }

        [TestMethod]
        public void loadMatchStats()
        {
            var lmd = new List<MatchDrop>();
            var matchStats = db.MwoAMatchMetrics_imports.ToList();
            var imps = matchStats.Count;

            foreach (var mmi in matchStats)
            {
                mmi.MatchHash = mmi.CalcMatchHash();
            }

            var hashes=  matchStats.Select(x => x.MatchHash).Distinct();


            foreach (var hash in hashes)
            {
                List<MatchStat> friendlies = new List<MatchStat>();
                List<MatchStat> enemies = new List<MatchStat>();
                foreach (var mmi2 in matchStats.Where(o=>o.MatchHash==hash))
                    {
                        if (mmi2.team=="friendly")
                            friendlies.Add(ImportToMatchStat(mmi2));
                        else
                        {
                            enemies.Add(ImportToMatchStat(mmi2));
                        }
                    }
                MatchLogger.MatchLogger.LoggedMatch loggedMatch= new MatchLogger.MatchLogger.LoggedMatch();
                loggedMatch.FriendlyMatchStats = friendlies;
                loggedMatch.EnemyMatchStats = enemies;
                loggedMatch.AssociationName = "PUG";
                loggedMatch.MatchHash = "";
                var md = MWA.Models.ModelHelper.GetLoggedMatchDrop(loggedMatch,db);
                lmd.Add(md);
            }

            
            Assert.IsTrue(lmd.Count> 0);
        }

        [TestMethod]
        public void GetLoggedMatchDropTest()
        {
            #region TESTDATA: Create LoggedMatch
            MatchLogger.MatchLogger.LoggedMatch lm = new MatchLogger.MatchLogger.LoggedMatch();
            lm.AssociationName = "TEST";

            // DropDeck12 fdd = new DropDeck12{MechName1 = "blr-1gp",MechName2="cn9-a",MechName3= "drg-5nc",MechName4 = "hbk-4p",MechName5 ="shd-2d2",MechName6="shd-2hp",MechName7="shd-2hp",MechName8="stk-3h",MechName9="tbt-7m",MechName10="tbt-3c",MechName11="tdr-5ss",MechName12="vtr-9b"};
            // DropDeck12 edd = new DropDeck12{MechName1 = "as7-d-dc",MechName2="cda-2a",MechName3= "cn9-ac",MechName4 = "cplt-a1",MechName5 = "cplt-c1-founder",MechName6 ="drg-5nc",MechName7="hbk-4g",MechName8="lct-3m",MechName9="shd-5m",MechName10="stk-3h",MechName11="tdr-5ss",MechName12="tdr-5sp" };
            //lm.MatchHash ="2013~307~as7-d-dc.cda-2a.cn9-ac.cplt-a1.cplt-c1-founder.drg-5nc.hbk-4g.lct-3m.shd-5m.stk-3h.tdr-5ss.tdr-5sp~as7-d-dc.cda-2a.cn9-ac.cplt-a1.cplt-c1-founder.drg-5nc.hbk-4g.lct-3m.shd-5m.stk-3h.tdr-5ss.tdr-5sp~alpinepeaks~4929";
            lm.PublishingUserName = "DrAmnesia";
            #endregion

    

            #region TESTDATA: create MatchStats
 
            lm = lm.LoadTestMatchStats();

            #endregion
 
            MatchDrop md = lm.GetLoggedMatchDrop(db);
            
            Assert.IsTrue(md.FriendlyDropDeck12 != null && md.FriendlyDropDeck12.Tonnage > 0);
            Assert.IsTrue(md.EnemyDropDeck12!=null&&md.EnemyDropDeck12.Tonnage>0);
        }

        [TestMethod]
        public void PostLoggedMatchLogicTest()
        {
            #region Create LoggedMatch
            MatchLogger.MatchLogger.LoggedMatch lm = new MatchLogger.MatchLogger.LoggedMatch();
            lm.AssociationName = "TEST";

            // DropDeck12 fdd = new DropDeck12{MechName1 = "blr-1gp",MechName2="cn9-a",MechName3= "drg-5nc",MechName4 = "hbk-4p",MechName5 ="shd-2d2",MechName6="shd-2hp",MechName7="shd-2hp",MechName8="stk-3h",MechName9="tbt-7m",MechName10="tbt-3c",MechName11="tdr-5ss",MechName12="vtr-9b"};
            // DropDeck12 edd = new DropDeck12{MechName1 = "as7-d-dc",MechName2="cda-2a",MechName3= "cn9-ac",MechName4 = "cplt-a1",MechName5 = "cplt-c1-founder",MechName6 ="drg-5nc",MechName7="hbk-4g",MechName8="lct-3m",MechName9="shd-5m",MechName10="stk-3h",MechName11="tdr-5ss",MechName12="tdr-5sp" };
            //lm.MatchHash ="2013~307~as7-d-dc.cda-2a.cn9-ac.cplt-a1.cplt-c1-founder.drg-5nc.hbk-4g.lct-3m.shd-5m.stk-3h.tdr-5ss.tdr-5sp~as7-d-dc.cda-2a.cn9-ac.cplt-a1.cplt-c1-founder.drg-5nc.hbk-4g.lct-3m.shd-5m.stk-3h.tdr-5ss.tdr-5sp~alpinepeaks~4929";
            lm.PublishingUserName = "DrAmnesia";
            #endregion

            #region create player lists,mech lists and matchhash

            List<string> friendlyplayers = new List<string>
                                           { "DrAmnesia",
                                               "Mountainhigh00",
                                               "Cubbyman",
                                               "DougStevens",
                                               "PyAlotz",
                                               "Dunkelgelb",
                                               "Kamenitaris",
                                               "Kaldorn",
                                               "Steeles",
                                               "pdox",
                                               "Pedroig",
                                               "Martis Gradivus",
                                           };
            List<string> enemyplayers = new List<string>
                                        {"Alaric VISIGOTH",
                                            "Ghosthunter15",
                                            "Baldrika",
                                            "RiotHero",
                                            "Uriam",
                                            "RED DOG RAIDER",
                                            "avgleandt",
                                            "Maurox",
                                            "Dimuborg",
                                            "Tess Loren",
                                            "Honkhonk",
                                            "Obeast"
                                        };
            List<string> friendlymechs = new List<string>
                                         {
                                             "drg-5nc",
                                             "cda-2a",
                                             "hbk-4g",
                                             "cplt-c1-founder",
                                             "stk-3h",
                                             "as7-d-dc",
                                             "shd-5m",
                                             "tbt-3c",
                                             "tdr-5ss",
                                             "cplt-a1",
                                             "vtr-9b",
                                             "stk-3h"
                                         }.OrderBy(o => o).ToList();
            ;

            List<string> enemymechs = new List<string>
                                      {
                                          "tdr-5sp",
                                          "shd-2hp",
                                          "tbt-7m",
                                          "cn9-ac",
                                          "blr-1gp",
                                          "lct-3m",
                                          "shd-2d2",
                                          "drg-5nc",
                                          "hbk-4p",
                                          "shd-2hp",
                                          "tdr-5ss",
                                          "cn9-a"
                                      }.OrderBy(o => o).ToList();
            int y = System.DateTime.UtcNow.Year;
            int d = System.DateTime.UtcNow.DayOfYear;
            string l = "alpinepeaks";
            int dmg = 24000000;
            string matchHash = String.Format("y{0}d{1}f{2}e{3}l{4}d{5}", y, d, String.Join(".", friendlymechs),
                String.Join(".", enemymechs), l, dmg);



            #endregion

            //set matchhash
            lm.MatchHash = matchHash;

            #region Create MatchDrop
            MatchDrop md;
            md = new MatchDrop { AssociationName = lm.AssociationName };
            // set association
            md.Association = db.Associations.FirstOrDefault(a => a.AssociationName == lm.AssociationName);
            md.AssociationId = md.AssociationId;
            // fetch or create dropdecks using friendlymechs and enemymechs
            DropDeck12 fdd = ModelHelper.SetDropDeck(friendlymechs, db);
            DropDeck12 edd = ModelHelper.SetDropDeck(enemymechs, db);

            md.FriendlyDropDeck12Id = fdd.DropDeck12Id;
            md.EnemyDropDeck12Id = edd.DropDeck12Id;
            md.FriendlyDropDeck12 = db.DropDeck12s.FirstOrDefault(f => f.DropDeck12Id == fdd.DropDeck12Id);
            md.EnemyDropDeck12 = db.DropDeck12s.FirstOrDefault(e => e.DropDeck12Id == edd.DropDeck12Id);
            md.MatchHash = lm.MatchHash;
            db.MatchDrops.Add(md);
            db.SaveChanges();
            #endregion

            #region create MatchStats
            Random rnd = new Random();
            int seed = rnd.Next(1, 3);
            List<MatchStat> fml = new List<MatchStat>();
            List<MatchStat> eml = new List<MatchStat>();
            for (int i = 0; i < 12; i++)
            {
                var fm = TestHelpers.CreateMatchStat();
                fm.name = friendlyplayers[i];
                fm.mech = friendlymechs[i];
                int fdmg = rnd.Next(100, 400);
                int fAssists = rnd.Next(0, 8);
                fm.damage = fdmg * seed;
                fm.assists = fAssists;
                fm.kills = 1;
                fm.lance = (i + 1) / 4;
                fml.Add(fm);
                var em = TestHelpers.CreateMatchStat();
                em.name = enemyplayers[i];
                em.mech = enemymechs[i];
                em.team = "enemy";
                em.status = 1;
                em.victory = 2;
                em.kills = 0;
                int edmg = rnd.Next(10, 40);
                em.damage = edmg * seed;
                em.lance = (i + 1) / 4;
                eml.Add(em);
            }
            #endregion

            lm.EnemyMatchStats = eml;
            lm.FriendlyMatchStats = fml;

            #region Create MwoAMatchMetric
            foreach (MatchStat fm in lm.FriendlyMatchStats)
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
                matchtopush.MatchHash = lm.MatchHash;
                matchtopush.PublishFlag = 1;
                matchtopush.PublishingUserName = (lm.PublishingUserName == fm.name);
                matchtopush.MatchDrop = db.MatchDrops.FirstOrDefault(m => m.MatchDropId == md.MatchDropId);
                db.MwoAMatchMetrics.Add(matchtopush);
                db.SaveChanges();
            }
            foreach (MatchStat em in lm.EnemyMatchStats)
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
                matchtopush.MatchHash = lm.MatchHash;
                matchtopush.PublishFlag = 1;
                matchtopush.PublishingUserName = (lm.PublishingUserName == em.name);
                matchtopush.MatchDrop = db.MatchDrops.FirstOrDefault(m => m.MatchDropId == md.MatchDropId);
                db.MwoAMatchMetrics.Add(matchtopush);
                db.SaveChanges();
            }
            #endregion

            int matches = lm.EnemyMatchStats.Count + lm.FriendlyMatchStats.Count;
            Assert.IsTrue(matches == 24);
        }  

   

        [TestMethod]
        public void Put()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }

    public static class TestHelpers
    {
        public static MatchLogger.MatchLogger.LoggedMatch LoadTestMatchStats(this MatchLogger.MatchLogger.LoggedMatch lm)
        {
            #region TESTDATA: create player lists,mech lists and matchhash

            List<string> friendlyplayers = new List<string>
                                           { "DrAmnesia",
                                               "Mountainhigh00",
                                               "Cubbyman",
                                               "DougStevens",
                                               "PyAlotz",
                                               "Dunkelgelb",
                                               "Kamenitaris",
                                               "Kaldorn",
                                               "Steeles",
                                               "pdox",
                                               "Pedroig",
                                               "Martis Gradivus",
                                           };
            List<string> enemyplayers = new List<string>
                                        {"Alaric VISIGOTH",
                                            "Ghosthunter15",
                                            "Baldrika",
                                            "RiotHero",
                                            "Uriam",
                                            "RED DOG RAIDER",
                                            "avgleandt",
                                            "Maurox",
                                            "Dimuborg",
                                            "Tess Loren",
                                            "Honkhonk",
                                            "Obeast"
                                        };
            List<string> friendlymechs = new List<string>
                                         {
                                             "drg-5nc",
                                             "cda-2a",
                                             "hbk-4g",
                                             "cplt-c1-founder",
                                             "stk-3h",
                                             "as7-d-dc",
                                             "shd-5m",
                                             "tbt-3c",
                                             "tdr-5ss",
                                             "cplt-a1",
                                             "vtr-9b",
                                             "stk-3h"
                                         }.OrderBy(o => o).ToList();
           

            List<string> enemymechs = new List<string>
                                      {
                                          "tdr-5sp",
                                          "shd-2hp",
                                          "tbt-7m",
                                          "cn9-ac",
                                          "blr-1gp",
                                          "lct-3m",
                                          "shd-2d2",
                                          "drg-5nc",
                                          "hbk-4p",
                                          "shd-2hp",
                                          "tdr-5ss",
                                          "cn9-a"
                                      }.OrderBy(o => o).ToList();

            #endregion

            var fml = new List<MatchStat>();
            var eml = new List<MatchStat>();
            Random rnd = new Random();
            int seed = rnd.Next(1, 3);
            for (int i = 0; i < 12; i++)
            {
                var fm = CreateMatchStat();
                fm.name = friendlyplayers[i];
                fm.mech = friendlymechs[i];
                int fdmg = rnd.Next(100, 400);
                int fAssists = rnd.Next(0, 8);
                fm.damage = fdmg * seed;
                fm.assists = fAssists;
                fm.kills = 1;
                fm.lance = (i/ 4)+1;
                fm.matchscore = fm.damage/(seed*3);
                fml.Add(fm);
                var em = CreateMatchStat();
                em.name = enemyplayers[i];
                em.mech = enemymechs[i];
                em.team = "enemy";
                em.status = 1;
                em.victory = 2;
                em.kills = 0;
                int edmg = rnd.Next(10, 40);
                em.damage = edmg * seed;
                em.lance = (i/ 4)+1;
                eml.Add(em);
            }

            lm.EnemyMatchStats = eml;
            lm.FriendlyMatchStats = fml;
            return lm;
        }
        public static MatchStat CreateMatchStat()
        {
            var mtemp = new MatchStat
            {
                assists = 0,
                avatar = 1,
                command = 1,
                damage = 1000000,
                faction = 1,
                kills = 0,
                lance = 1,
                level = "alpinepeaks",
                matchscore = 1,
                matchType = "Conquest",
                team = "friendly",
                name = "DrAmensia",
                mech = "JR7-F",
                victory = 1,
                victoryType = "capture",
                ping = 100,
                status = 0,
                time = System.DateTime.UtcNow
            };
            return mtemp;
        }
    }
}
