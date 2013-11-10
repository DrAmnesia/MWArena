using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Reflection;
 
using Newtonsoft.Json;

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
        private static readonly string logFilename = @"C:\temp\log.txt";

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

            LoggedMatch lm = new LoggedMatch
                             {
                                 MatchHash = "",
                                 FriendlyMatchStats = friendlyList,
                                 EnemyMatchStats = enemyList,
                                 AssociationName = "PUG",
                                 PublishingUserName =
                                     (playerNameSet)
                                         ? ((playerName == null | playerName == "")
                                             ? "UNKNOWN"
                                             : playerName)
                                         : "UNKNOWN"
                             };
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
        public static void httpPostLoggedMatch(LoggedMatch lm)
        {
            string api = "http://mwarena.azurewebsites.net/api/LoggedMatch";
            MakeRequest(api, lm, null, null, typeof(HttpResponseMessage));
        }

        public static string GetApiUrl()
        {

            string apiurl = "http://mwarena.azurewebsites.net/api/"; 
             return apiurl;

        }
        public static string GetApiUrl(string controller)
        {
            return GetApiUrl() + controller;
        }

        public static void SetPlayerName(string pn)
        {
            playerName = pn;
            playerNameSet = true;
            //apiUrl = (System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] == "" || System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] == null) ? "http://v5-Dev//" : System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
            //associationName = (System.Configuration.ConfigurationManager.AppSettings["AssociationName"] == "" || System.Configuration.ConfigurationManager.AppSettings["AssociationName"] == null) ? "TEST" : System.Configuration.ConfigurationManager.AppSettings["AssociationName"];
        }

        public class LoggedMatch
        {
            public string MatchHash { get; set; }
            public string AssociationName { get; set; }
            public List<MatchStat> FriendlyMatchStats { get; set; }
            public List<MatchStat> EnemyMatchStats { get; set; }

            public string PublishingUserName { get; set; }

        }
        public static object MakeRequest(string requestUrl, object JSONRequest, string JSONmethod,
                                     string JSONContentType, Type JSONResponseType)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //WebRequest WR = WebRequest.Create(requestUrl);

                string sb = JsonConvert.SerializeObject(JSONRequest);

                request.Method = (JSONmethod == "" | JSONmethod == null) ? "POST" : JSONmethod;
                request.ContentType = (JSONmethod == "" | JSONmethod == null) ? "application/json" : JSONContentType; // "application/json";

                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Found)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    //  DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    // object objResponse = JsonConvert.DeserializeObject();
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    object objResponse = JsonConvert.DeserializeObject(strsb, JSONResponseType);

                    return objResponse;
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static object MakeRequest<T>(this T JSONResponseType, string requestUrl, object JSONRequest, string JSONmethod,
                                    string JSONContentType)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //WebRequest WR = WebRequest.Create(requestUrl);

                string sb = JsonConvert.SerializeObject(JSONRequest);

                request.Method = JSONmethod; // "POST";
                request.ContentType = JSONContentType; // "application/json";

                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    //  DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    // object objResponse = JsonConvert.DeserializeObject();
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    object objResponse = JSONResponseType.DeserializeJsonObject(strsb);

                    return objResponse;
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        public static T DeserializeJsonObject<T>(this T JsonObjectType, string JsonString) { return JsonConvert.DeserializeObject<T>(JsonString); }


    }

        
}
