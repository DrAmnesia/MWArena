using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

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
        private static readonly string logFilename = "log.txt";

        static void Log(string text)
        {
            lock (logLock)
            {
                try { File.AppendAllText(logFilename, string.Format("{0}\r\n", text)); }
                catch { }
            }
        }

        public static void HandleRoundResultEnemy(byte[] message)
        {
            string s = ASCIIEncoding.ASCII.GetString(message);
#if DEBUG
            Log(string.Format("[ENEMY TEAM]:{0}\r\n", s));
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
            string s = ASCIIEncoding.ASCII.GetString(message);
#if DEBUG
            Log(string.Format("[FRIENDLY TEAM]:{0}\r\n", s));
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
            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string playerCsvPath = Path.Combine(exePath, "player.csv");
            string matchCsvPath = Path.Combine(exePath, "match.csv");

            StringBuilder csv = new StringBuilder();
            if (!File.Exists(playerCsvPath))
                csv.AppendLine(PlayerStat.GetFieldNames());
            csv.AppendLine(playerStat.ToString());
            File.AppendAllText(playerCsvPath, csv.ToString());
            csv.Clear();
            if (!File.Exists(matchCsvPath))
                csv.AppendLine(MatchStat.GetFieldNames());
            foreach (MatchStat m in friendlyList)
                csv.AppendLine(m.ToString());
            foreach (MatchStat m in enemyList)
                csv.AppendLine(m.ToString());
            File.AppendAllText(matchCsvPath, csv.ToString());
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
#if DEBUG
            Log(string.Format("Level loading: {0}", s));
#endif
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
#if DEBUG
            Log(string.Format("CBill stats: {0}", s));
#endif
        }

        public static void HandleXpStats(byte[] buffer)
        {
            matchEndTime = DateTime.UtcNow;
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            playerStat.HandleXpStats(s);
#if DEBUG
            Log(string.Format("XP stats: {0}", s));
#endif
        }

        public static void HandleMatchType(byte[] buffer)
        {
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            activeMatchType = s;
#if DEBUG
            Log(string.Format("Match type: {0}", s));
#endif
        }

        public static void HandleUI(byte[] buffer)
        {
#if DEBUG
            //string s = ASCIIEncoding.ASCII.GetString(buffer);
            //Log(string.Format("UI: {0}", s));
#endif
        }

        public static void HandleDeathDamageStats(byte[] buffer)
        {
#if DEBUG
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            Log(string.Format("DeathDamageStats: {0}", s));
#endif
        }

        public static void HandleDeathArmorStats(byte[] buffer)
        {
#if DEBUG
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            Log(string.Format("DeathArmorStats: {0}", s));
#endif
        }

        public static void HandleDeathBattleTime(byte[] buffer)
        {
#if DEBUG
            string s = ASCIIEncoding.ASCII.GetString(buffer);
            Log(string.Format("DeathBattleTime: {0}", s));
#endif
        }

        public class LoggedMatch
        {
            public string MatchHash { get; set; }
            public string AssociationName { get; set; }
            public List<MatchStat> FriendlyMatchStats { get; set; }
            public List<MatchStat> EnemyMatchStats { get; set; }

            public string PublishingUserName { get; set; }

        }
    }
}
