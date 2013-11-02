using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchLogger
{
    public class MatchStat
    {
        public DateTime time = DateTime.MinValue;
        public string level = "";
        public int victory = -1;
        public string victoryType = "unknown";
        public string matchType = "unknown";
        public string team = "unknown";
        int command = -1;
        int avatar = -1;
        int faction = -1;
        public string name = "";
        public string mech = "";
        public int status = -1;
        public int matchscore = -1;
        public int kills = -1;
        public int assists = -1;
        public int damage = -1;
        public int ping = -1;
        public int lance = -1;

        private ulong ConvertToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (ulong)span.TotalSeconds;
        }

        public override string ToString()
        {
            object[] objArray = new object[] { ConvertToTimestamp(time), matchType, level, team, victory, victoryType, command, avatar, faction, name, mech, status, matchscore, kills, assists, damage, ping, lance };
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}", objArray);
        }

        public static string GetFieldNames()
        {
            return "timestamp,matchtype,level,team,victory,victorytype,command,avatar,faction,name,mech,status,matchscore,kills,assists,damage,ping,lance";
        }

        public void HandleMatchStat(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (!s.Contains(':'))
                    return;
                string[] split = s.Split(':');
                if (split.Length < 13)
                    return;
                //0:0:0:playername:@stk-5m_short:1:38:1:2:317:92:0:0
                Int32.TryParse(split[0], out command);
                Int32.TryParse(split[1], out avatar);
                Int32.TryParse(split[2], out faction);
                name = split[3];
                mech = split[4].TrimStart('@').Replace("_short", "");
                Int32.TryParse(split[5], out status);
                Int32.TryParse(split[6], out matchscore);
                Int32.TryParse(split[7], out kills);
                Int32.TryParse(split[8], out assists);
                Int32.TryParse(split[9], out damage);
                Int32.TryParse(split[10], out ping);
                Int32.TryParse(split[11], out lance);
            }
        }
    }
}
