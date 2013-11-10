using System;
using System.Collections.Generic;
using System.Text;

namespace MatchLogger
{
    public class PlayerStat
    {
        public DateTime time = DateTime.MinValue;
        public string level = "";
        public string mech = "";
        public int victory = -1;
        public string victoryType = "unknown";
        public string matchType = "";
        public int status = -1;
        //cb
        public int basecb = -1;
        public int defensivekillcb = -1;
        public int killassistscb = -1;
        public int compdestructcb = -1;
        public int killscb = -1;
        public int saviorkillcb = -1;
        public int damagedonecb = -1;
        public int spottingassistcb = -1;
        public int tagnarcbonuscb = -1;
        public int salvagebonuscb = -1;
        public int subtotalcb = -1;
        public int mechbonuscb = -1;
        public int conquestbonuscb = -1;
        public int cadetbonuscb = -1;
        public int premiumbonuscb = -1;
        public int teamdamagecb = -1;
        public int teamkillcb = -1;

        //xp
        public int basexp = -1;
        public int capturexp = -1;
        public int conquestwinxp = -1;
        public int captureassistxp = -1;
        public int compdestructxp = -1;
        public int killsxp = -1;
        public int killassistsxp = -1;
        public int saviorkillxp = -1;
        public int defensivekillxp = -1;
        public int spottingassistsxp = -1;
        public int uavkillxp = -1;
        public int tagnarcbonusxp = -1;
        public int firstvictoryxp = -1;
        public int subtotalxp = -1;
        public int mechbonusxp = -1;
        public int premiumbonusxp = -1;
        public int teamkillxp = -1;

        private ulong ConvertToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (ulong)span.TotalSeconds;
        }

        public override string ToString()
        {
            object[] objArray = new object[] { ConvertToTimestamp(time), matchType, mech, status, level, victory, victoryType, basecb, defensivekillcb, killassistscb, compdestructcb, killscb, saviorkillcb, damagedonecb, spottingassistcb, tagnarcbonuscb, salvagebonuscb, subtotalcb,
                mechbonuscb, conquestbonuscb, cadetbonuscb, premiumbonuscb, teamdamagecb, teamkillcb, basexp, capturexp, conquestwinxp, captureassistxp, compdestructxp, killsxp, killassistsxp, saviorkillxp, defensivekillxp,
                spottingassistsxp, uavkillxp, tagnarcbonusxp, firstvictoryxp, subtotalxp, mechbonusxp, premiumbonusxp, teamkillxp};
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40}", objArray);
        }

        public static string GetFieldNames()
        {
            return "timestamp,matchtype,mech,status,level,victory,victorytype,basecb,defensivekillcb,killassistscb,compdestructcb,killscb,saviorkillcb,damagedonecb,spottingassistcb,tagnarcbonuscb,salvagebonuscb,subtotalcb," +
            "mechbonuscb,conquestbonuscb,cadetbonuscb,premiumbonuscb,teamdamagecb,teamkillcb,basexp,capturexp,conquestwinxp,captureassistxp,compdestructxp,killsxp,killassistsxp,saviorkillxp,defensivekillxp," +
            "spottingassistsxp,uavkillxp,tagnarcbonusxp,firstvictoryxp,subtotalxp,mechbonusxp,premiumbonusxp,teamkillxp";
        }

        public void HandleCbillStats(string s)
        {
            if (string.IsNullOrEmpty(s) || !s.Contains("|"))
                return;
            string[] split = s.Split('|');

            foreach (string ss in split)
            {
                if (string.IsNullOrEmpty(ss) || !ss.Contains(":"))
                    return;

                string[] stat = ss.Split(':');
                if (stat.Length < 2)
                    return;
                switch (stat[0])
                {
                    case "<KEYWINLOSS>":
                        Int32.TryParse(stat[1], out basecb);
                        Int32.TryParse(stat[4], out victory);
                        break;
                    case "@ui_defensivekill":
                        Int32.TryParse(stat[1], out defensivekillcb);
                        break;
                    case "@ui_killassists":
                        Int32.TryParse(stat[1], out killassistscb);
                        break;
                    case "@ui_compdestruct":
                        Int32.TryParse(stat[1], out compdestructcb);
                        break;
                    case "@ui_kills":
                        Int32.TryParse(stat[1], out killscb);
                        break;
                    case "@ui_saviorkill":
                        Int32.TryParse(stat[1], out saviorkillcb);
                        break;
                    case "@ui_damagedone":
                        Int32.TryParse(stat[1], out damagedonecb);
                        break;
                    case "@ui_spottingassist":
                        Int32.TryParse(stat[1], out spottingassistcb);
                        break;
                    case "@HUD_TAGNARC_BONUS":
                        Int32.TryParse(stat[1], out tagnarcbonuscb);
                        break;
                    case "@ui_salvagebonus":
                        Int32.TryParse(stat[1], out salvagebonuscb);
                        break;
                    case "@ui_subtotal":
                        Int32.TryParse(stat[1], out subtotalcb);
                        break;
                    case "@ui_mechbonus":
                        Int32.TryParse(stat[1], out mechbonuscb);
                        break;
                    case "@ui_conquestbonus":
                        Int32.TryParse(stat[1], out conquestbonuscb);
                        break;
                    case "@ui_cadetbonus":
                        Int32.TryParse(stat[1], out cadetbonuscb);
                        break;
                    case "@ui_premiumbonus":
                        Int32.TryParse(stat[1], out premiumbonuscb);
                        break;
                    case "@ui_teamdamage":
                        Int32.TryParse(stat[1], out teamdamagecb);
                        break;
                    case "@ui_teamkill":
                        Int32.TryParse(stat[1], out teamkillcb);
                        break;
                }
            }
        }


        public void HandleXpStats(string s)
        {
            if (string.IsNullOrEmpty(s) || !s.Contains("|"))
                return;
            string[] split = s.Split('|');

            foreach (string ss in split)
            {
                if (string.IsNullOrEmpty(ss) || !ss.Contains(":"))
                    return;

                string[] stat = ss.Split(':');
                if (stat.Length < 2)
                    return;
                switch (stat[0])
                {
                    case "<KEYWINLOSS>":
                        Int32.TryParse(stat[1], out basexp);
                        break;
                    case "@ui_capture":
                        Int32.TryParse(stat[1], out capturexp);
                        break;
                    case "@ui_conquestwin":
                        Int32.TryParse(stat[1], out conquestwinxp);
                        break;
                    case "@ui_captureassist":
                        Int32.TryParse(stat[1], out captureassistxp);
                        break;
                    case "@ui_compdestruct":
                        Int32.TryParse(stat[1], out compdestructxp);
                        break;
                    case "@ui_kills":
                        Int32.TryParse(stat[1], out killsxp);
                        break;
                    case "@ui_killassists":
                        Int32.TryParse(stat[1], out killassistsxp);
                        break;
                    case "@ui_saviorkill":
                        Int32.TryParse(stat[1], out saviorkillxp);
                        break;
                    case "@ui_defensivekill":
                        Int32.TryParse(stat[1], out defensivekillxp);
                        break;
                    case "@ui_spottingassists":
                        Int32.TryParse(stat[1], out spottingassistsxp);
                        break;
                    case "@HUD_UAVKILL":
                        Int32.TryParse(stat[1], out uavkillxp);
                        break;
                    case "@HUD_TAGNARC_BONUS":
                        Int32.TryParse(stat[1], out tagnarcbonusxp);
                        break;
                    case "@ui_firstVictory":
                        Int32.TryParse(stat[1], out firstvictoryxp);
                        break;
                    case "@ui_subtotal":
                        Int32.TryParse(stat[1], out subtotalxp);
                        break;
                    case "@ui_mechbonus":
                        Int32.TryParse(stat[1], out mechbonusxp);
                        break;
                    case "@ui_premiumbonus":
                        Int32.TryParse(stat[1], out premiumbonusxp);
                        break;
                    case "@ui_teamkill":
                        Int32.TryParse(stat[1], out teamkillxp);
                        break;
                }
            }
        }
    }
}
