using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
 
using MwoA.Util;
using MwoA.Models;
using System.Net.Http;
namespace MatchLogger
{
    public static class MatchLogger
    {
        private static bool playerNameSet = false, matchActive = false;
        private static string playerName = "", activeLevelName = "", activeMatchType = "";
        private static StringBuilder team1 = new StringBuilder(), team2 = new StringBuilder();
        private static PlayerStat playerStat = new PlayerStat();
        private static List<MatchStat> friendlyList = new List<MatchStat>();
        private static List<MatchStat> enemyList = new List<MatchStat>();
        private static bool allFriendsKilled = true, allEnemiesKilled = true;
        private static DateTime matchEndTime = DateTime.MaxValue;
       
        public static void HandleRoundResultEnemy(byte[] message)
        {
           
            string s = System.Text.ASCIIEncoding.ASCII.GetString(message);
#if DEBUG
            //add lock
            //team2.AppendFormat("[ENEMY TEAM]:{0}\r\n", s);
#endif
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
           
            string s = System.Text.ASCIIEncoding.ASCII.GetString(message);
#if DEBUG
            //add lock
            //team1.AppendFormat("[FRIENDLY TEAM]:{0}\r\n", s);
#endif
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

       
            StringBuilder csv = new StringBuilder();
            if (!File.Exists("player.csv"))
                csv.AppendLine(PlayerStat.GetFieldNames());
            csv.AppendLine(playerStat.ToString());
            File.AppendAllText("player.csv", csv.ToString());

            csv.Clear();
            if (!File.Exists("match.csv"))
                csv.AppendLine(MatchStat.GetFieldNames());
            foreach (MatchStat m in friendlyList)
            {
                
                httpPostMatchMetricTest(m);
                csv.AppendLine(m.ToString());
            }

            foreach (MatchStat m in enemyList)
            {
                httpPostMatchMetricTest(m);
                csv.AppendLine(m.ToString());
            }
 
            File.AppendAllText("match.csv", csv.ToString());
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
                                m.victory = playerStat.victory;
                            m.time = matchEndTime;
                            m.victoryType = playerStat.victoryType;
                            m.level = activeLevelName;
                            m.matchType = activeMatchType;
                        }
              
                        LogCSV();
                        var matchStatsCompletedArgs = new MatchStatsCompletedArgs();

                        matchStatsCompletedArgs.MatchStats = friendlyList.Concat(enemyList).ToList();
                        
                        onMatchStatsCompleted(matchStatsCompletedArgs);

                        //reset all variables in preparation for next match
                        matchEndTime = DateTime.MaxValue;
                        activeLevelName = "";
                        team1.Clear();
                        team2.Clear();
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

            

            //Log.LogMessage(string.Format("Level loading: {0}", s));
        }

         static void onMatchStatsCompleted(MatchStatsCompletedArgs e)
        {
            EventHandler handler = MatchStatsCompleted;
            if ( MatchStatsCompleted != null)
                 MatchStatsCompleted(handler, e);                       
        }
        public static event EventHandler MatchStatsCompleted;
     


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
#if DEBUG
            //Log.LogMessage(string.Format("CBill stat: {0}", s));
#endif
        }

        public static void HandleXpStats(byte[] buffer)
        {
 
            matchEndTime = DateTime.UtcNow;
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            playerStat.HandleXpStats(s);
#if DEBUG
            //Log.LogMessage(string.Format("XP stat: {0}", s));
#endif
        }

        public static void HandleMatchType(byte[] buffer)
        {
 
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            activeMatchType = s;
#if DEBUG
            //Log.LogMessage(string.Format("Match type: {0}", s));
#endif
        }

        public static void HandleUI(byte[] buffer)
        {
 
#if DEBUG
      
            Guid tracker = Guid.NewGuid();
            string s = ASCIIEncoding.ASCII.GetString(buffer);
 
#endif
        }
       public static void httpPostMatchMetricTest(MatchStat m )
       {
           MwoA.Models.MwoAMatchMetric matchtopush = new MwoA.Models.MwoAMatchMetric();
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
           matchtopush.AssociationName = "PUG";
           matchtopush.MatchHash = GetMatchHash(m);
           matchtopush.PublishFlag = 1;
           matchtopush.PublishingUserName = (playerNameSet)?(playerName==m.name):false;



           HttpUtility.MakeRequest("http://localhost/MwoArena/api/MwoAMatchMetric", matchtopush, null,
                                                  null, typeof(HttpResponseMessage));
            
         
 
        }

        public static void SetPlayerName(string pn){
            playerName = pn;
            playerNameSet=true;
        }
       public static string GetMatchHash(MatchStat m)
       {
           return String.Format("{0}~{1}~{2}~{3}~{4}~{5}~{6}~{7}", m.time.ToUniversalTime().Year, m.time.ToUniversalTime().DayOfYear, m.name, m.mech, m.status, m.level, m.matchscore, m.damage).Replace(" ", "_");

       }

    }
    public class MatchStatsCompletedArgs : EventArgs
    {
        public List<MatchStat> MatchStats { get; set; }
    }

     
}
