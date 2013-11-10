using System;
using System.Collections.Generic;
using System.Linq;
using MatchLogger;

namespace MWA.Models
{
    public static  class TestHelpers
    {
        public static MatchLogger.MatchLogger.LoggedMatch LoadTestMatchStats(this MatchLogger.MatchLogger.LoggedMatch lm)
        {
            return lm.LoadTestMatchStats("rivercity_night");
        }

        public static MatchLogger.MatchLogger.LoggedMatch LoadTestMatchStats(this MatchLogger.MatchLogger.LoggedMatch lm, string map)
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
                var fm = CreateMatchStat(map);
                fm.name = friendlyplayers[i];
                fm.mech = friendlymechs[i];
                int fdmg = rnd.Next(100, 400);
                int fAssists = rnd.Next(0, 8);
                fm.damage = fdmg * seed;
                fm.assists = fAssists;
                fm.kills = 1;
                fm.lance = (i / 4) + 1;
                fm.matchscore = fm.damage / (seed * 3);
                fml.Add(fm);
                var em = CreateMatchStat(map);
                em.name = enemyplayers[i];
                em.mech = enemymechs[i];
                em.team = "enemy";
                em.status = 1;
                em.victory = 2;
                em.kills = 0;
                int edmg = rnd.Next(10, 40);
                em.damage = edmg * seed;
                em.lance = (i / 4) + 1;
                eml.Add(em);
            }

            lm.EnemyMatchStats = eml;
            lm.FriendlyMatchStats = fml;
            return lm;
        }

        public static MatchStat CreateMatchStat()
        {
            return CreateMatchStat("alpinepeaks");
        }

        public static MatchStat CreateMatchStat(string Map)
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
                            level = Map,
                            matchscore = 1,
                            matchType = "Conquest",
                            team = "friendly",
                            name = "DrAmensia",
                            mech = "JR7-F",
                            victory = 1,
                            victoryType = "capture",
                            ping = 100,
                            status = 0,
                            time = DateTime.UtcNow
                        };
            return mtemp;
        }
    }
}