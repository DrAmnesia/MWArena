﻿using System;
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
            {
                httpPostMatchMetricTest(m);
                csv.AppendLine(m.ToString());
            }

            foreach (MatchStat m in enemyList)
            {
                httpPostMatchMetricTest(m);
                csv.AppendLine(m.ToString());
            }
                
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
        public static void httpPostMatchMetricTest(MatchStat m)
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
            matchtopush.AssociationName = "PUG";
            matchtopush.MatchHash = GetMatchHash(m);
            matchtopush.PublishFlag = 1;
            matchtopush.PublishingUserName = (playerNameSet) ? (((playerName == null | playerName == "") ? "bob" : playerName) == m.name) : false;

            // string apiUrl = (System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] == "" || System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] == null) ? "http://mwarena.azurewebsites.net/api/MwoAMatchMetric" : System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];

            HttpUtility.MakeRequest("http://mwarena.azurewebsites.net/api/MwoAMatchMetric", matchtopush, null,
                                                   null, typeof(HttpResponseMessage));



        }

        public static void SetPlayerName(string pn)
        {
            playerName = pn;
            playerNameSet = true;
        }
        public static string GetMatchHash(MatchStat m)
        {
            return String.Format("{0}~{1}~{2}~{3}~{4}~{5}~{6}~{7}", m.time.ToUniversalTime().Year, m.time.ToUniversalTime().DayOfYear, m.name, m.mech, m.status, m.level, m.matchscore, m.damage).Replace(" ", "_");

        }
    }
}
