using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchLogger
{
    public class LoggedMatch
    {
        public string MatchHash { get; set; }
        public string AssociationName { get; set; }
        public List<MatchStat> FriendlyMatchStats { get; set; }
        public List<MatchStat> EnemyMatchStats { get; set; }

        public string PublishingUserName { get; set; }

    }

    public static class MatchCompletedPublisher
    {
        public delegate void MatchCompleteddHandler(object sender, MatchCompletedEventArgs e);
        public static event MatchCompleteddHandler OnMatchCompleted;

        

        public static void Publish(LoggedMatch match)
        {
            // Make sure someone is listening to event
            if (OnMatchCompleted==null) return;
            string publisher = "MWA Overlay";
            MatchCompletedEventArgs args = new MatchCompletedEventArgs(match);
            OnMatchCompleted(publisher, args);
        }
    }

    public class MatchCompletedEventArgs : EventArgs
    {
        public LoggedMatch Match { get; private set; }

        public MatchCompletedEventArgs(LoggedMatch match)
        {
            Match = match;
        }
    }

}
