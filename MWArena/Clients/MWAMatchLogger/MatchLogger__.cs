using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using MWA.Util;
using System.Net.Http;
using MWA.Models;
namespace MatchLogger
{
    public static class MatchLogger
    {
        private static bool playerNameSet = false, matchActive = false;
        private static string playerName = "", activeLevelName = "", activeMatchType = "";
        private static PlayerStat playerStat = new PlayerStat();
        private static List<MatchStat> friendlyList = new List<MatchStat>();
        private static List<MatchStat> enemyList = new List<MatchStat>();
        private static bool allFriendsKilled = true, allEnemiesKilled = true;
        private static DateTime matchEndTime = DateTime.MaxValue;
        private static object logLock = new object();
        private static readonly string logFilename = @"c:\temp\log.txt";
private static MatchDrop matchDrop;

 
        private static string matchHash;
        private static string apiUrl = "";
        private static string associationName = "";

        static void Log(string text)
        {
           /* lock (logLock)
            {
                try { File.AppendAllText(logFilename, string.Format("{0}\r\n", text)); }
                catch { }
            }
            * */
        }

        public static void HandleRoundResultEnemy(byte[] message)
        {
            string s = ASCIIEncoding.ASCII.GetString(message);
 
            MatchStat m = new MatchStat();
            m.team = "enemy";
            m.HandleMatchStat(s);
            lock (enemyList)
            {
                enemyList.Add(m);
            }
        }

        public static void HandleRoundResultFriendly(byte[] message)
        {
            string s = ASCIIEncoding.ASCII.GetString(message);
 
            MatchStat m = new MatchStat();
            m.team = "friendly";
            m.HandleMatchStat(s);
            lock (friendlyList)
            {
                friendlyList.Add(m);
            }
        }

        static void LogCSV()
        {
            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string playerCsvPath = Path.Combine(exePath, "player.csv");
            string matchCsvPath = Path.Combine(exePath, "match.csv");

            apiUrl = (System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] == "" || System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] == null) ? "http://mwarena.azurewebsites.net/api/" : System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
            associationName = (System.Configuration.ConfigurationManager.AppSettings["AssociationName"] == "" || System.Configuration.ConfigurationManager.AppSettings["AssociationName"] == null) ? "PUG" : System.Configuration.ConfigurationManager.AppSettings["AssociationName"];

            StringBuilder csv = new StringBuilder();
            if (!File.Exists(playerCsvPath))
                csv.AppendLine(PlayerStat.GetFieldNames());
            csv.AppendLine(playerStat.ToString());
            File.AppendAllText(playerCsvPath, csv.ToString());
            csv.Clear();
            if (!File.Exists(matchCsvPath))
                csv.AppendLine(MatchStat.GetFieldNames());
     
         
            foreach (MatchStat m in friendlyList)
            {
              //  httpPostMatchMetric(m);
                csv.AppendLine(m.ToString());
            }

            foreach (MatchStat m in enemyList)
            {
            //    httpPostMatchMetric(m);
                csv.AppendLine(m.ToString());
            }
                
            File.AppendAllText(matchCsvPath, csv.ToString());

            LoggedMatch lm = new LoggedMatch();
            lm.MatchHash = "";//GetMatchHash();
            lm.FriendlyMatchStats = friendlyList;
            lm.EnemyMatchStats = enemyList;
            lm.AssociationName = associationName;
            lm.PublishingUserName = (playerNameSet) ? ((playerName == null | playerName == "") ? "UNKNOWN" : playerName) : "UNKNOWN";

            //MatchCompletedPublisher.Publish(lm);
            httpPostLoggedMatch(lm);

        }

        public static void HandleLevelLoad(byte[] buffer)
        {
            string s = ASCIIEncoding.ASCII.GetString(buffer);

            if (s == "mechlab")
            {
                if (matchActive)
                {
                   

                    if (friendlyList == null || enemyList == null || friendlyList.Count == 0 || enemyList.Count == 0)
                        return;
                    matchActive = false;

                    foreach (MatchStat m in friendlyList)
                    {
                        if (m.status == 0)
                        {
                            allFriendsKilled = false;
                            break;
                        }
                    }

                    foreach (MatchStat m in enemyList)
                    {
                        if (m.status == 0)
                        {
                            allEnemiesKilled = false;
                            break;
                        }
                    }

                    if (allEnemiesKilled || allFriendsKilled)
                        playerStat.victoryType = "destruction";
                    else
                        playerStat.victoryType = "capture";
                    
                    playerStat.time = matchEndTime;
                    playerStat.matchType = activeMatchType;
                    playerStat.level = activeLevelName;
                    foreach (MatchStat m in friendlyList)
                    {
                        if (m.name == playerName)
                        {
                            playerStat.mech = m.mech;
                            playerStat.status = m.status;
                        }

                        m.time = matchEndTime;
                        m.victory = playerStat.victory;
                        m.victoryType = playerStat.victoryType;
                        m.level = activeLevelName;
                        m.matchType = activeMatchType;
                    }

                    foreach (MatchStat m in enemyList)
                    {
                        if (playerStat.victory == 2)
                            m.victory = 1;
                        else if (playerStat.victory == 1)
                            m.victory = 2;
                        else
                             m.victory = playerStat.victory; //can this happen? no idea
                        m.time = matchEndTime;
                        m.victoryType = playerStat.victoryType;
                        m.level = activeLevelName;
                        m.matchType = activeMatchType;
                    }

                    LogCSV();
                    //reset all variables in preparation for next match
                    matchEndTime = DateTime.MaxValue;
                    activeLevelName = "";
                    playerStat = new PlayerStat();
                    friendlyList.Clear();
                    enemyList.Clear();
                    allEnemiesKilled = true;
                    allFriendsKilled = true;

                }
            }
            else
            {
                matchActive = true;
                activeLevelName = s;
                playerStat.level = s;
            }
 
        }

        public static void HandlePlayerName(byte[] buffer)
        {
            if (!playerNameSet)
            {
                playerName = ASCIIEncoding.ASCII.GetString(buffer);
                playerNameSet = true;
            }
        }

        public static void HandleCbillStats(byte[] buffer)
        {
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            playerStat.HandleCbillStats(s);
 
        }

        public static void HandleXpStats(byte[] buffer)
        {
            matchEndTime = DateTime.UtcNow;
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            playerStat.HandleXpStats(s);
 
        }

        public static void HandleMatchType(byte[] buffer)
        {
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            activeMatchType = s;
 
        }

        public static void HandleUI(byte[] buffer)
        {
 
            //string s = ASCIIEncoding.ASCII.GetString(buffer);
            //Log(string.Format("UI: {0}", s));
 
        }

        public static void HandleDeathDamageStats(byte[] buffer)
        {
 
           // string s = ASCIIEncoding.ASCII.GetString(buffer);
            //Log(string.Format("DeathDamageStats: {0}", s));
 
        }

        public static void HandleDeathArmorStats(byte[] buffer)
        {
 
            //string s = ASCIIEncoding.ASCII.GetString(buffer);
           // Log(string.Format("DeathArmorStats: {0}", s));
 
        }

        public static void HandleDeathBattleTime(byte[] buffer)
        {
 
            //string s = ASCIIEncoding.ASCII.GetString(buffer);
           // Log(string.Format("DeathBattleTime: {0}", s));
 
        }

        public static void httpPostLoggedMatch(LoggedMatch lm)
        {
            string api = GetApiUrl("LoggedMatch");
            if (api == null || api == "") api = "v5-dev/MWArena/api/LoggedMatch";
             HttpUtility.MakeRequest(api, lm, null, null, typeof(HttpResponseMessage));
        }


        public static void httpPostMatchMetric(MatchStat m)
        {


            MWA.Models.MwoAMatchMetric matchtopush = new MWA.Models.MwoAMatchMetric();
            matchtopush.assists = m.assists;
            matchtopush.damage = m.damage;
            matchtopush.kills = m.kills;
            matchtopush.lance = m.lance;
            matchtopush.level = m.level;
            matchtopush.matchType = m.matchType;
            matchtopush.matchscore = m.matchscore;
            matchtopush.mech = m.mech;
            matchtopush.name = m.name;
            matchtopush.ping = m.ping;
            matchtopush.status = m.status;
            matchtopush.team = m.team;
            matchtopush.victory = m.victory;
            matchtopush.victoryType = m.victoryType;
            matchtopush.time = m.time.ToUniversalTime().ToString();
            matchtopush.AssociationId = 0;
            matchtopush.AssociationName = associationName;
            matchtopush.MatchHash = matchHash;
            matchtopush.PublishFlag = 1;
            matchtopush.PublishingUserName = (playerNameSet) ? (((playerName == null | playerName == "") ? "UNKNOWN" : playerName) == m.name) : false;
            string api = GetApiUrl("MatchDrop/1");
            MatchDrop md;
            matchDrop = (MatchDrop)HttpUtility.MakeRequest(api, null, null, null, typeof(MatchDrop));
            matchtopush.MatchDrop = matchDrop;
            api = GetApiUrl("MwoAMatchMetric");
            
            HttpUtility.MakeRequest(api, matchtopush, null, null, typeof(HttpResponseMessage));
        }

       public static MatchDrop getMatchDrop()
        {
            //TODO: think about what happens when two people are in the same drop drop but log it with different association names
            DropDeck12 friendlyDropDeck12 = GetDropDeck12(true);
            DropDeck12 enemyDropDeck12 = GetDropDeck12(false);
            Association association = getAssociation();

            MWA.Models.MatchDrop matchDropToPush = new MWA.Models.MatchDrop();
            matchDropToPush.Association = association;
            matchDropToPush.AssociationId = association.AssociationId;
            matchDropToPush.AssociationName = association.AssociationName;
            matchDropToPush.FriendlyDropDeck12= friendlyDropDeck12;
            matchDropToPush.EnemyDropDeck12= enemyDropDeck12;
            matchDropToPush.MatchHash=GetMatchHash();
            string api = GetApiUrl("MatchDrop");
            MatchDrop md;
            matchDrop =(MatchDrop) HttpUtility.MakeRequest(api, matchDropToPush, null,null, typeof(MatchDrop));
            return matchDrop;
        }

        public static Association getAssociation()
        {

            Association association= new Association();
             
            association =(Association) association.MakeRequest(apiUrl, associationName, null,null);
            if (association == null)
            {
                association = new Association { AssociationId = 1,  AssociationName = "PUG" };
            }
            return association;
        }

        public static void SetPlayerName(string pn)
        {
            playerName = pn;
            playerNameSet = true;
            apiUrl = (System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] == "" || System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] == null) ? "http://mwarena.azurewebsites.net/api/" : System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
            associationName = (System.Configuration.ConfigurationManager.AppSettings["AssociationName"] == "" || System.Configuration.ConfigurationManager.AppSettings["AssociationName"] == null) ? "PUG" : System.Configuration.ConfigurationManager.AppSettings["AssociationName"];
        }

        public static string GetApiUrl()
        {
            return apiUrl;
        }
        public static string GetApiUrl(string controller)
        {
            return apiUrl+controller;
        }

        public static DropDeck12 GetDropDeck12(bool friendly)
        {
            //todo: init dropdeck chassis in controller
            //      set the order of chassis[1-12]to alphabetical mechname order
            //      add friendlyDropDeck12 and enemyDropDeck12 to MatchDrop
            string api = apiUrl+"DropDeck12";
            List<string> dd12mechs = new List<string>();
            if (friendly==true){
                foreach (MatchStat lm in friendlyList)
                {
                   dd12mechs.Add(lm.mech);
                }
            }
            else{
                foreach (MatchStat lm in enemyList)
                {
                  dd12mechs.Add(lm.mech);
                }
            }
                    
            string[] ddmechs =  new string[12];

            for(int i =0;i<dd12mechs.Count;i++){
                ddmechs[i]= dd12mechs[i];
            }
            DropDeck12 dd12 = new DropDeck12{
                MechName1=dd12mechs[0],
                MechName2=dd12mechs[1],
                MechName3=dd12mechs[2],
                MechName4=dd12mechs[3],
                MechName5=dd12mechs[4],
                MechName6=dd12mechs[5],
                MechName7=dd12mechs[6],
                MechName8=dd12mechs[7],
                MechName9=dd12mechs[8],
                MechName10=dd12mechs[9],
                MechName11=dd12mechs[10],
                MechName12=dd12mechs[11]
            };
            DropDeck12 dd;
            dd = (DropDeck12) HttpUtility.MakeRequest(api, dd12, null, null, typeof(DropDeck12));
            return dd;
        }

         static string GetMatchHash()
        {
            string MatchMechs = String.Empty;
            int DMG = 0;
            MatchStat m = new MatchStat();
            bool isFirst = true;
            foreach (MatchStat lm in friendlyList.OrderBy(o=>o).ToList())
            {
                if (isFirst)
                {
                    m = lm;
                    isFirst = false;
                }
                MatchMechs = MatchMechs + lm.mech + ".";
                DMG += lm.damage;
            }
            MatchMechs = MatchMechs + "-";
            foreach (MatchStat lm in enemyList.OrderBy(o => o).ToList())
            {
                MatchMechs = MatchMechs + lm.mech + ".";
                DMG += lm.damage;
            }
            return String.Format("{0}~{1}~{2}~{3}~{4}", m.time.ToUniversalTime().Year, m.time.ToUniversalTime().DayOfYear, MatchMechs,m.level,DMG).Replace(" ", "_");

        }
    }
}
